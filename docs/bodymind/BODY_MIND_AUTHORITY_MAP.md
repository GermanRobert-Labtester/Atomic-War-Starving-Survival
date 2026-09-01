# Body & Mind Authority Map

This document establishes the single source of truth for every medical, physical, administrative, and psychological domain in ASHFALL.

| State / Domain | Authoritative System | Plan 27 Responsibility | Storage / Representation |
| :--- | :--- | :--- | :--- |
| **Physical Acute Radiation** | `Ashfall.Core.Radiation.RadiationSystem` | Biological consequence, symptoms, acute sickness | `SurvivorRadState.RadiationDose` |
| **Physical Lifetime Exposure** | `Ashfall.Core.Radiation.RadiationSystem` | Accumulated lifetime rem/mSv | `SurvivorRadState.LifetimeRadiationExposure` |
| **Administrative Dose Record** | `Ashfall.Core.DoseLedgerSystem` | Booked readings, ledger entries, calibration state | `DoseEntry.cumulativeMsv`, `DoseReading` |
| **Administrative Classifications / Forgery** | `Ashfall.Core.DoseLedgerSystem` | Forged chits, band overrides, institutional standing | `DoseEntry.hasForgedCleanBill`, `adminOverride` |
| **Dose Bands & Vocabulary** | `Assets/StreamingAssets/Data/dose_registers.json` | Green/Amber/Red/Black bands, palliative plans, cohort guesses | `DoseRegistersCatalog` |
| **Palliative & Sick List** | `Ashfall.Core.SickListSystem` | Bed order, comfort rounds, morphine allocation | `SickListState` |
| **Cohort & Baseline Chalk** | `Ashfall.Core.CohortRegisterSystem` | Children's baseline board, chalk guesses | `CohortRegisterState` |
| **Voluntary Work Signatures** | `Ashfall.Core.VoluntaryRegisterSystem` | High-risk shift signatures | `VoluntaryRegisterState` |
| **Autopsy Procedures & Execution** | `Ashfall.Core.AutopsySystem` | Tool gates, procedure duration, risk, findings | `AutopsyState`, `AutopsyCase` |
| **Cause-of-Death & Body Lifecycle** | `Ashfall.Core.DeathState` / `MemorialSystem` | Remains status, burial timeline, death record | `MemorialSystemState` |
| **Pathology / Disease Intel** | `Ashfall.Core.DiseaseSystem` | Pathogen strain discovery, quarantine status | `DiseaseState` |
| **Medical Knowledge Yields** | `Ashfall.Core.ResearchSystem` | Tech tree unlocks from dissection | `ResearchKnowledgeCatalog` |
| **Forensic Evidence & Cold Cases** | `Ashfall.Core.Verdict` / Evidence Ledger | Murder/poisoning/tampering proof | `EvidenceRecord` |
| **Kinship, Consent & Grief** | `Ashfall.Core.Survivors.SurvivorRelationsSystem` | Kin consent, grief burden, morale consequences | `SurvivorRelationsState` |
| **Psychological Contamination** | `Ashfall.Core.Maritime.PsychologicalContaminationSystem` | Dread/trauma exposure from horrific sites | `PsychContaminationSave` |
| **Somatic Flashbacks** | `Ashfall.Core.SomaticFlashbackSystem` | Physical memory triggers and freeze states | `SomaticFlashbackState` |
| **Combat Trauma** | `Ashfall.Core.CombatTraumaSystem` | Combat-induced trauma and companion grounding | `CombatTraumaState` |
| **Guilt & Insomnia** | `Ashfall.Core.GuiltInsomniaSystem` | Sleep disruption from moral choices and atrocity | `GuiltInsomniaState` |

---

## Non-Negotiable Invariant Contracts
1. **Administrative Separation:** A forged clean-bill chit or altered ledger entry changes only what the Dose Register institution believes. It does NOT mutate `SurvivorRadState.RadiationDose` or `LifetimeRadiationExposure`.
2. **Deterministic Forensics:** Autopsies reveal canonical upstream state (poison, trauma, infection, radiation) and cannot procedurally fabricate an arbitrary murderer.
3. **No Dual Sanity Systems:** Psychological contamination is a contextual exposure model routing into existing trauma, flashback, and insomnia systems; it is not a parallel sanity meter.
