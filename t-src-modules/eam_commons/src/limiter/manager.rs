use crate::limiter::api_limiter::ApiLimiter;
use crate::models::CallResult;

use chrono::{DateTime, Duration, Utc};
use std::collections::HashMap;
use std::sync::atomic::{AtomicU64, Ordering};
use std::sync::{Arc, Mutex};
use std::{thread, time::Duration as StdDuration};

type CooldownCallback = Box<dyn Fn() + Send + Sync>;
type RemainingChangedCallback = Box<dyn Fn(&str, usize, usize) + Send + Sync>;

pub struct RateLimiterManager {
    pub cooldown_seconds: i64,
    pub sub_limiters: HashMap<String, ApiLimiter>,
    pub cooldown_until: Option<DateTime<Utc>>,
    pub last_cooldown_end: Option<DateTime<Utc>>,
    pub limited_endpoints: Vec<(String, String)>,

    pub cooldown_id: Arc<AtomicU64>,
    pub cooldown_listeners: Vec<CooldownCallback>,
    pub cooldown_reset_listeners: Arc<Mutex<Vec<CooldownCallback>>>,

    pub last_known_remaining: HashMap<String, usize>,
    pub api_remaining_changed_listeners: Arc<Mutex<Vec<RemainingChangedCallback>>>,
}

impl RateLimiterManager {
    pub fn register_cooldown_listener<F>(&mut self, callback: F)
    where
        F: Fn() + Send + Sync + 'static,
    {
        self.cooldown_listeners.push(Box::new(callback));
    }

    fn notify_cooldown_triggered(&self) {
        for listener in &self.cooldown_listeners {
            listener();
        }
    }

    pub fn register_cooldown_reset_listener<F>(&mut self, callback: F)
    where
        F: Fn() + Send + Sync + 'static,
    {
        self.cooldown_reset_listeners
            .lock()
            .unwrap()
            .push(Box::new(callback));
    }

    pub fn register_remaining_changed_listener<F>(&mut self, callback: F)
    where
        F: Fn(&str, usize, usize) + Send + Sync + 'static,
    {
        if let Ok(mut listeners) = self.api_remaining_changed_listeners.lock() {
            listeners.push(Box::new(callback));
        }
    }

    fn notify_remaining_changed(&mut self, api: &str, remaining: usize, limit: usize) {
        let previous = self.last_known_remaining.get(api).copied();

        if previous != Some(remaining) {
            self.last_known_remaining.insert(api.to_string(), remaining);
            if let Ok(listeners) = self.api_remaining_changed_listeners.lock() {
                for listener in listeners.iter() {
                    listener(api, remaining, limit);
                }
            }
        }
    }

    pub fn is_cooldown(&self) -> bool {
        matches!(self.cooldown_until, Some(until) if Utc::now() < until)
    }

    pub fn trigger_cooldown(&mut self) {
        let cooldown_end = Utc::now() + Duration::seconds(self.cooldown_seconds);
        self.cooldown_until = Some(cooldown_end);
        self.last_cooldown_end = Some(cooldown_end);

        self.notify_cooldown_triggered();

        let new_id = self.cooldown_id.fetch_add(1, Ordering::SeqCst) + 1;

        let cooldown_duration = self.cooldown_seconds;
        let listeners = Arc::clone(&self.cooldown_reset_listeners);
        let cooldown_id = Arc::clone(&self.cooldown_id);

        thread::spawn(move || {
            thread::sleep(StdDuration::from_secs(cooldown_duration as u64));
            let current_id = cooldown_id.load(Ordering::SeqCst);

            if current_id == new_id {
                if let Ok(listeners) = listeners.lock() {
                    for listener in listeners.iter() {
                        listener();
                    }
                }
            }
        });
    }

    pub fn can_call(&mut self, api: &str) -> bool {
        if self.is_cooldown() {
            return false;
        }

        let reset_ts = self
            .last_cooldown_end
            .filter(|end| *end <= Utc::now())
            .map(|dt| dt.timestamp_millis());

        if let Some(limiter) = self.sub_limiters.get(api) {
            let remaining = limiter.remaining(reset_ts);
            self.notify_remaining_changed(api, remaining, limiter.limit); // Optional: only notify if changed
            return remaining > 0;
        }

        false
    }

    pub fn register(&mut self, limiter: ApiLimiter) {
        self.sub_limiters.insert(limiter.api_name.clone(), limiter);
    }

    pub fn get_limiter_key_from_url(&self, url: &str) -> Option<String> {
        self.limited_endpoints
            .iter()
            .find(|(pattern, _)| url.contains(pattern))
            .map(|(_, key)| key.clone())
    }

    pub fn is_rate_limited_response(body: &str) -> bool {
        body.contains("try again later")
            || body.contains("5 minutes")
            || body.contains("too many requests")
    }

    pub fn record_api_use(&mut self, api: &str, result: CallResult) {
        if let Some(limiter) = self.sub_limiters.get(api) {
            limiter.record(result.clone());

            // After recording, get the new state
            if matches!(result, CallResult::Success | CallResult::RateLimited) {
                let remaining = limiter.remaining(None);
                self.notify_remaining_changed(api, remaining, limiter.limit);
            }
        }
    }

    pub fn start_api_monitor(&self) {
        let sub_limiters = self.sub_limiters.clone();
        let last_known_remaining = Arc::new(Mutex::new(self.last_known_remaining.clone()));
        let listeners = Arc::clone(&self.api_remaining_changed_listeners);

        thread::spawn(move || loop {
            thread::sleep(StdDuration::from_secs(1));

            for (api_name, limiter) in &sub_limiters {
                let remaining = limiter.remaining(None);

                let mut known = last_known_remaining.lock().unwrap();
                let previous = known.get(api_name).copied();

                if previous != Some(remaining) {
                    known.insert(api_name.clone(), remaining);

                    if let Ok(locked_listeners) = listeners.lock() {
                        for listener in locked_listeners.iter() {
                            listener(api_name, remaining, limiter.limit);
                        }
                    }
                }
            }
        });
    }
}
