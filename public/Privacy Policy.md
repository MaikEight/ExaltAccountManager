**Privacy Policy for Exalt Account Manager**

**Effective Date:** 15.03.2025

Welcome to the Privacy Policy for **Exalt Account Manager (EAM)**. This document explains how we collect, use, and protect your information when you use our software.

---
## **1. Who We Are**
EAM is developed and maintained by **Maik Kühne IT-Dienstleistungen** (operated by Maik8, GitHub: MaikEight). It is a client-side application that allows users to manage their Realm of the Mad God (RotMG) accounts efficiently.

For any privacy concerns, you can contact us at:
- **Email:** privacy@maik8.de
- **Discord:** [Support Server](https://discord.exalt-account-manager.eu/)

---
## **2. What Data We Collect & Why**
EAM only collects **limited data** necessary for its operation and improvement. We do **not** collect personally identifiable information (e.g., real name, email, phone number) unless specified below.

### **2.1 Legal Basis for Processing**
We process user data based on the following legal grounds under the GDPR:
- **Contractual Necessity (Art. 6(1)(b) GDPR)** → For authentication (Auth0 login), processing payments (Stripe), and settings sync.
- **Legitimate Interest (Art. 6(1)(f) GDPR)** → For security measures, fraud prevention, and anonymized analytics.
- **User Consent (Art. 6(1)(a) GDPR)** → For optional settings sync and allowing users to opt-out of analytics.

### **2.2 Data We Collect**
- **Discord ID** (used for authentication via Auth0)
- **Auth0 User ID** (authentication metadata storage)
- **Stripe Customer ID** (for paying users, stored in Auth0 metadata and our backend for subscription management)
- **Optional user settings** (if sync is enabled, these settings are stored in Auth0 metadata)
- **Anonymized analytics** (usage statistics, unique session counts, non-identifiable hardware information)

### **2.3 Payment Information**
- **Stripe handles all payment processing** securely.
- We **do not** store credit card information or billing details.
- We only store a reference to the **Stripe Customer ID** to manage subscriptions.
- Stripe may retain transaction data for legal/tax purposes.

---
## **3. Data Retention & Deletion**
- **Auth0 & Stripe metadata** → Retained **as long as the user has an active account**.
- **Anonymized analytics** → Stored indefinitely unless a user requests deletion.
- **Deleted accounts** → Upon request, all personal data is deleted immediately, except payment records required by tax laws.
- **Error Logs** → Logs exist **client-side** and will be deleted by the app after a maximum of 90 days upon app start.
- **Audit Logs** → These logs track **game-account data and changes to them** (e.g., fetching and storing new data for that account).
- **Debug Logs** → The client maintains a **debug log in the form of a text file**, which is **overwritten with each app start**.

Users can request data deletion via **the in-app settings menu** or email **forget@maik8.de**.

---
## **4. Where Data is Stored & Transferred**
All user-related data is stored **within the European Union**:
- **EAM self-hosted servers**: NetCup & Hetzner (Germany)
- **Stripe & Auth0**: May transfer data outside the EU

If data is transferred outside the EU, we ensure GDPR compliance using:
- **Standard Contractual Clauses (SCCs)** approved by the European Commission.
- Other GDPR-compliant safeguards where applicable.

---
## **5. Your GDPR Rights**
Under the GDPR, you have the following rights:

### **5.1 Right to Access & Data Export**
- Users can request a copy of their stored data in **JSON format**.
- Requests can be made **from inside the application** or via email (privacy@maik8.de).

### **5.2 Right to Deletion (“Right to be Forgotten”)**
- Users can request data deletion via the **in-app settings menu** or by emailing **forget@maik8.de**.
- Deletion requests are processed **immediately** (no grace period).
- Payment records **cannot be deleted** due to legal obligations.

### **5.3 Right to Object & Restrict Processing**
- **Users can opt-out of analytics tracking from within the app**.

---
## **6. Security Measures**
We implement industry-standard security measures, including:
- **SSL encryption (HTTPS)** for all communications.
- **OAuth2 authentication** for secure login.
- **Locally stored sensitive data (such as passwords) is encrypted using the Windows Data Protection API.**

---
## **7. Privacy Policy Updates & Notifications**
- The Privacy Policy is available via **a dedicated website linked from within EAM**.
- If legally required, major updates will be communicated via **an in-app popup**.
- Continued use of EAM implies acceptance of the updated policy.

---
## **8. Contact Information**
If you have any questions or requests about this Privacy Policy, you can reach us at:
- **Email:** privacy@maik8.de
- **Discord:** [Support Server](https://discord.exalt-account-manager.eu/)
