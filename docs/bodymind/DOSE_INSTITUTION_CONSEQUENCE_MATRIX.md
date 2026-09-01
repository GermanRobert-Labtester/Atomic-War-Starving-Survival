# Dose Institution Consequence Matrix

This document maps how Dose Register classifications interface with shelter systems, duty assignments, and medical care.

| Administrative Band | Surface Expeditions | Reactor / Hazmat Shifts | Shelter Domestic Shifts | Clean-Room Beds | Palliative Rounds |
| :--- | :--- | :--- | :--- | :--- | :--- |
| **Green (`band_green`)** | Unrestricted | Eligible | Unrestricted | Standard Bay | Not Indicated |
| **Amber (`band_amber`)** | Advisory Caution | Limited Rotation | Unrestricted | Standard Bay | Occasional Monitoring |
| **Red (`band_red`)** | Restricted without Lead Shielding | Restricted (Requires Leadership Override) | Unrestricted (Kitchen, Workshop, Hydroponics) | Priority Allocation | Regular Comfort Rounds |
| **Black (`band_black`)** | Prohibited | Strictly Prohibited | Light Duty / Bed Rest | Dedicated Bed | Daily Morphine / Palliative Schedule |

---

## Key Policy Principles
1. **No Blanket Harmless Exclusion:** Red and Black band survivors are never excluded from harmless tasks (kitchen, tailoring, archive work, counseling, reading). Exclusion applies strictly to high-rad environments (reactor bays, contaminated wasteland ruins).
2. **Leadership Waivers:** If an essential technician in Red band must fix the reactor, an explicit emergency override is required, generating moral and relationship consequences.
3. **Forgery Impact:** A forged clean-bill chit allows a survivor to pass the Screening Station checkpoint (`loc_the_screening_station`), but will not prevent ARS progression if physical radiation dose continues to climb.
