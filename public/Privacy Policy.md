# **Privacy Policy for Exalt Account Manager (EAM)**
**Effective Date:** 15.03.2025  
_Last Updated: 15.03.2025_

Welcome to the Privacy Policy for **Exalt Account Manager (EAM)**. This document describes how we collect, use, store, and protect your personal data when you use our software in compliance with the **General Data Protection Regulation (GDPR)**.

---

## **1. Who We Are**
EAM is developed and maintained by **Maik Kühne IT-Dienstleistungen** (operated by Maik8, GitHub: [MaikEight](https://github.com/MaikEight)). We are based in Germany.

- **Controller**:  
  **Name**: Maik Kühne IT-Dienstleistungen  
  **Address**:  
  Maik Kühne  
  Postfach 1103  
  37171 Nörten-Hardenberg  
  Germany  
- **Email**: [privacy@maik8.de](mailto:privacy@maik8.de)  
- **Discord**: [Support Server](https://discord.exalt-account-manager.eu/)

If you have privacy concerns, please reach out to us using the contact details above.

---

## **2. Data We Collect & Purposes**

### **2.1 Legal Basis for Processing**
Under the GDPR, we process your personal data based on:
- **Contractual Necessity (Art. 6(1)(b) GDPR)**: For providing authentication via Auth0, managing subscriptions via Stripe, and syncing settings if you opt in.  
- **Legitimate Interests (Art. 6(1)(f) GDPR)**: For security, fraud prevention, and certain limited analytics (where not consent-based).  
- **Consent (Art. 6(1)(a) GDPR)**: Where you explicitly opt in to analytics, and you may withdraw this consent at any time.

### **2.2 Types of Data Collected**

1. **Discord ID & Auth0 User ID**  
   - **Purpose**: Authentication for advanced EAM features (e.g., subscription management, settings sync).  
   - **Storage**: Held in your Auth0 “app_metadata” and used only to identify your account.

2. **Stripe Customer ID** (for paid users)  
   - **Purpose**: Subscription management and payment handling.  
   - **Storage**: Stored in Auth0 metadata and on our backend to link your EAM subscription status.

3. **Optional User Settings**  
   - **Purpose**: If you enable settings sync, these preferences (e.g., UI preferences) are stored in Auth0 metadata.  
   - **Control**: You can disable or delete synced settings anytime.

4. **Analytics Data**  
   - **Purpose**: To improve EAM by understanding usage patterns, app performance, and feature adoption.  
   - **How It Works**:  
     - **Approx. Location** (country, city) is derived server-side from your IP using an IP-range lookup. We do **not** store the IP itself.  
     - **Hashed Client Hardware ID (HWID)** is sent unless you choose the “Anonymized” option.  
     - **Anonymized** or **Opt-Out** modes are available (see “Analytics & Consent” below).  

5. **Payment Information**  
   - Handled securely by **Stripe**. We never store credit card numbers or billing info.  
   - We only store a reference to your **Stripe Customer ID** and subscription details for account verification and subscription management.

### **2.3 Analytics & Consent Options**
You can choose how much data to share via in-app settings:
1. **Full Analytics**  
   - Sends **hashed HWID** and approximate location (country, city).  
   - Helps us diagnose recurring issues per device and optimize the app.  
2. **Anonymized**  
   - Replaces your hashed HWID with **“ANONYMIZED”**, but still stores approximate location.  
   - We cannot link data to a specific device.  
3. **Opt-Out**  
   - No analytics data is sent at all.

You can change these settings at any time. If you withdraw consent for analytics, we stop collecting new data immediately.

---

## **3. Logs (Client-Side Only)**

### **3.1 Error Logs**
- **Location**: Stored locally in the app’s interface (Logs section).  
- **Retention**: 90 days; older entries are deleted upon the next application start.  
- **Usage**: For troubleshooting by you or, if you choose, shared with us for support.

### **3.2 Debug Logs**
- **Location**: Stored in a local `log.txt` file on your device.  
- **Retention**: Overwritten with each app startup (only the latest session is kept).  
- **Usage**: Helpful for diagnosing difficult bugs. Again, only accessible to us if you manually provide it.

### **3.3 Audit Logs**
- **Location**: Stored locally in the app’s Logs section.  
- **Retention**: Permanent unless you manually delete them.  
- **Usage**: Tracks changes to your game accounts (e.g., which account was launched, when data was fetched). Useful for personal review of your account usage history.

> **Note**: We do **not** automatically receive any logs. They remain on your device, and you may share them with us at your discretion (e.g., for troubleshooting).

---

## **4. Data Retention & Deletion**

1. **Auth0 & Stripe Metadata**  
   - Retained as long as you have an active account or subscription.  
   - Deleted upon account closure, unless necessary for tax or legal obligations.

2. **Anonymized Analytics**  
   - Stored indefinitely, as it’s non-identifiable.  
   - If you opt out or withdraw consent, no further analytics are collected.

3. **Payment Records**  
   - We rely on **Stripe** to handle payments and comply with tax laws (potentially up to 10 years in Germany).  
   - We cannot erase financial transaction data required by law.

4. **User-Initiated Deletion**  
   - **In-App**: You can delete or reset your user data or logs.  
   - **Via Email**: Send requests to [forget@maik8.de](mailto:forget@maik8.de).  
   - **Immediate Action**: We delete requested data promptly, unless retained for legal or contractual reasons.

---

## **5. Where Data Is Stored & Transferred**
- We store data on **self-hosted servers** in Germany (NetCup & Hetzner).  
- **Stripe & Auth0**: May process data in the U.S. or other countries. We ensure GDPR compliance through Standard Contractual Clauses (SCCs) or equivalent safeguards.

---

## **6. Your GDPR Rights**
Under the GDPR, you have the right to:

1. **Access & Data Portability**  
   - Obtain a copy of your personal data in a commonly used format (e.g., JSON).  
2. **Rectification**  
   - Request correction of inaccurate or incomplete data.  
3. **Erasure (“Right to be Forgotten”)**  
   - Request deletion of your data, processed promptly unless legal obligations require retention.  
4. **Withdraw Consent**  
   - Revoke any consent-based processing (e.g., analytics) at any time via in-app settings or by contacting us.  
5. **Object or Restrict Processing**  
   - Object to certain types of processing or request temporary restriction of data usage.  
6. **Lodge a Complaint**  
   - If you believe your rights are infringed, you may complain to your local Data Protection Authority (e.g., the [BfDI](https://www.bfdi.bund.de/) in Germany).

To exercise any of these rights, contact us at [privacy@maik8.de](mailto:privacy@maik8.de).

---

## **7. Security Measures**
- **Encryption in Transit**: We use SSL/TLS (HTTPS) to secure data sent over the internet.  
- **Local Encryption**: Sensitive data (e.g., Auth0 refresh tokens) is encrypted on your device with Windows Data Protection API.  
- **Limited Server-Side Data**: We do **not** store raw IP addresses, only approximate location from an IP lookup.  
- **No Automated Decision-Making**: We do not use automated profiling that produces legal or significant effects on you.

---

## **8. Age Requirements**
We do not explicitly restrict minors from using EAM. However, advanced features (e.g., subscription management, settings sync) require **Discord login**, which is subject to [Discord’s Terms of Service](https://discord.com/terms) (minimum age typically 13+). By choosing to log in via Discord, you confirm you meet Discord’s age requirements.

---

## **9. Policy Updates & Notifications**
- We may update this Privacy Policy to reflect changes in our practices or for legal reasons.  
- Major changes will be communicated in-app or via a prominent notice.  
- The latest version is always linked within EAM. Continued use of EAM after updates indicates acceptance of the revised policy.

---

## **10. Contact Information**
If you have any privacy-related questions or requests (including data access, rectification, or deletion), please reach out:

- **Email**: [privacy@maik8.de](mailto:privacy@maik8.de)  
- **Discord**: [Support Server](https://discord.exalt-account-manager.eu/)  
- **Data Deletion Requests**: [forget@maik8.de](mailto:forget@maik8.de)

---

**Thank you for using Exalt Account Manager!**  
We are committed to protecting your privacy and handling your data responsibly.
