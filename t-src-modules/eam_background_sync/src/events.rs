use chrono::{DateTime, Utc};
use log::warn;
use serde::{Deserialize, Serialize};
use std::panic::AssertUnwindSafe;
use std::sync::{Arc, Mutex};
use uuid::Uuid;

/// Represents granular sync events emitted by the background syncer.
#[derive(Clone, Debug, Serialize, Deserialize)]
pub enum BackgroundSyncEvent {
    ModeChanged(String),
    AccountStarted(String),
    AccountProgress(String, AccountProgressState),
    AccountFinished(String, String),
    AccountFailed(String, String),
    AccountCharListSync {
        id: Uuid,
        email: String,
        dataset: String,
    },
    DailyLoginDone,
    DailyLoginProgress {
        done: usize,
        left: usize,
        left_emails: Vec<String>,
        failed_emails: Vec<String>,
        estimated_time: DateTime<Utc>,
    },
}

#[derive(Clone, Debug, Serialize, Deserialize)]
pub enum AccountProgressState {
    FetchingAccount,
    FetchingCharList,
    SyncingCharList,
    WaitingForCooldown,
    Done,
    Failed,
}

/// Listener signature type
pub type EventListener = Arc<dyn Fn(BackgroundSyncEvent) + Send + Sync>;

/// Global event registry to allow multiple components to listen for sync events.
#[derive(Default, Clone)]
pub struct BackgroundSyncEventHub {
    listeners: Arc<Mutex<Vec<EventListener>>>,
}

impl BackgroundSyncEventHub {
    pub fn new() -> Self {
        Self {
            listeners: Arc::new(Mutex::new(Vec::new())),
        }
    }

    /// Add a listener that will be called on every sync event.
    pub fn register_listener<F>(&self, callback: F)
    where
        F: Fn(BackgroundSyncEvent) + Send + Sync + 'static,
    {
        let mut listeners = self.listeners.lock().unwrap();
        listeners.push(Arc::new(callback));
    }

    /// Broadcasts an event to all registered listeners.
    pub fn emit(&self, event: BackgroundSyncEvent) {
        let listeners = self.listeners.lock().unwrap();
        for listener in listeners.iter() {
            let result = std::panic::catch_unwind(AssertUnwindSafe(|| {
                listener(event.clone());
            }));

            if result.is_err() {
                warn!("A background sync event listener panicked.");
            }
        }
    }
}
