use roxmltree::Document;
use serde::{Deserialize, Serialize};
use std::fmt;

/// Represents the state/result of an API request
#[derive(Serialize, Deserialize, Debug, Clone, PartialEq)]
pub enum RequestState {
    Success,
    WrongPassword,
    TooManyRequests,
    Captcha,
    AccountSuspended,
    AccountInUse,
    Error,
}

impl fmt::Display for RequestState {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        match self {
            RequestState::Success => write!(f, "Success"),
            RequestState::WrongPassword => write!(f, "WrongPassword"),
            RequestState::TooManyRequests => write!(f, "TooManyRequests"),
            RequestState::Captcha => write!(f, "Captcha"),
            RequestState::AccountSuspended => write!(f, "AccountSuspended"),
            RequestState::AccountInUse => write!(f, "AccountInUse"),
            RequestState::Error => write!(f, "Error"),
        }
    }
}

impl RequestState {
    /// Returns the string representation used in frontend communication
    pub fn as_str(&self) -> &'static str {
        match self {
            RequestState::Success => "Success",
            RequestState::WrongPassword => "WrongPassword",
            RequestState::TooManyRequests => "TooManyRequests",
            RequestState::Captcha => "Captcha",
            RequestState::AccountSuspended => "AccountSuspended",
            RequestState::AccountInUse => "AccountInUse",
            RequestState::Error => "Error",
        }
    }
}

/// Parse the request state from an XML response.
/// 
/// Checks for an `<Error>` node and determines the appropriate request state
/// based on the error message content.
pub fn parse_request_state(xml: &str) -> RequestState {
    let doc = match Document::parse(xml) {
        Ok(doc) => doc,
        Err(_) => return RequestState::Error,
    };

    // Look for <Error> node anywhere in the document
    for node in doc.descendants() {
        if node.has_tag_name("Error") {
            if let Some(error_text) = node.text() {
                let error_lower = error_text.to_lowercase();

                if error_lower.contains("passworderror") {
                    return RequestState::WrongPassword;
                } else if error_lower.contains("wait") || error_lower.contains("try again later") {
                    return RequestState::TooManyRequests;
                } else if error_lower.contains("captchalock") {
                    return RequestState::Captcha;
                } else if error_lower.contains("suspended") {
                    return RequestState::AccountSuspended;
                } else if error_lower.contains("account in use") {
                    return RequestState::AccountInUse;
                } else {
                    return RequestState::Error;
                }
            }
            // Empty error node is still an error
            return RequestState::Error;
        }
    }

    // No error node found means success
    RequestState::Success
}

/// Parse account name from XML response (from <Account><Name>)
pub fn parse_account_name(xml: &str) -> Option<String> {
    let doc = Document::parse(xml).ok()?;
    
    // Find <Account> node and then <Name> inside it
    for account_node in doc.descendants().filter(|n| n.has_tag_name("Account")) {
        for name_node in account_node.children().filter(|n| n.has_tag_name("Name")) {
            if let Some(name) = name_node.text() {
                return Some(name.to_string());
            }
        }
    }
    
    None
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_parse_request_state_success() {
        let xml = r#"<?xml version="1.0"?>
        <Chars>
            <Account>
                <Name>TestAccount</Name>
            </Account>
        </Chars>"#;

        assert_eq!(parse_request_state(xml), RequestState::Success);
    }

    #[test]
    fn test_parse_request_state_wrong_password() {
        let xml = r#"<?xml version="1.0"?><Error>PasswordError</Error>"#;
        assert_eq!(parse_request_state(xml), RequestState::WrongPassword);
    }

    #[test]
    fn test_parse_request_state_rate_limited() {
        let xml = r#"<?xml version="1.0"?><Error>please wait 5 minutes</Error>"#;
        assert_eq!(parse_request_state(xml), RequestState::TooManyRequests);
    }

    #[test]
    fn test_parse_request_state_captcha() {
        let xml = r#"<?xml version="1.0"?><Error>CaptchaLock</Error>"#;
        assert_eq!(parse_request_state(xml), RequestState::Captcha);
    }

    #[test]
    fn test_parse_request_state_suspended() {
        let xml = r#"<?xml version="1.0"?><Error>Account suspended</Error>"#;
        assert_eq!(parse_request_state(xml), RequestState::AccountSuspended);
    }

    #[test]
    fn test_parse_request_state_in_use() {
        let xml = r#"<?xml version="1.0"?><Error>Account in use</Error>"#;
        assert_eq!(parse_request_state(xml), RequestState::AccountInUse);
    }

    #[test]
    fn test_parse_request_state_generic_error() {
        let xml = r#"<?xml version="1.0"?><Error>Some unknown error</Error>"#;
        assert_eq!(parse_request_state(xml), RequestState::Error);
    }

    #[test]
    fn test_parse_request_state_invalid_xml() {
        let xml = "not valid xml";
        assert_eq!(parse_request_state(xml), RequestState::Error);
    }

    #[test]
    fn test_parse_account_name() {
        let xml = r#"<?xml version="1.0"?>
        <Chars>
            <Account>
                <Name>TestPlayer</Name>
            </Account>
        </Chars>"#;

        assert_eq!(parse_account_name(xml), Some("TestPlayer".to_string()));
    }

    #[test]
    fn test_parse_account_name_not_found() {
        let xml = r#"<?xml version="1.0"?><Error>PasswordError</Error>"#;
        assert_eq!(parse_account_name(xml), None);
    }
}
