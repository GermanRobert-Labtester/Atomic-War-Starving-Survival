# ASHFALL — Wave 10 Part 1 Task A3: Recorded Micro-Deferral Sweep Report

**Document role:** execution-grade forensic sweep and reconciliation of explicitly recorded micro-deferrals across completed claims, handoffs, closeouts, and debt ledgers.

**Author:** Integrator (user-authorized continuation)\
**Date:** 2026-09-17\
**Contract authority:** `Seal-steps/1442102_ASHFALL_WAVE10_IMPLEMENTATION_UNBLOCKER_PLAN_PART1.md` §4\

---

## 1. Sources Searched

The sweep conducted an exhaustive search for documented deferral language (`deferred`, `not-yet-authored`, `remaining`, `flagged`, `quirk`, `later`, `follow-up`, `decision memo`) across:

1. **Active and historical claim ledgers:** `WORKTREE_OWNERSHIP.md`
2. **Integration plan checkpoints and closeouts:**
   - `docs/plans/PLAN_24_CLOSEOUT.md` (Plan 24 Survivor Ledger)
   - `docs/plans/C1_planintegration[5]_IMPLEMENTATION_LOG.md` (Plan 24 Survivor Journey)
   - `docs/plans/wave8_part2/C1_{PREMISE_EVIDENCE,DECISION,CHANGE_MATRIX,ACCEPTANCE,HANDOFF}.md` (Plan 211 Black Market)
   - `docs/plans/wave8_part2/C2_{PREMISE_EVIDENCE,DECISION}.md` (Amputation Travel & Visuals)
   - `docs/plans/wave8_part2/C3_{PREMISE_EVIDENCE,DECISION,CHANGE_MATRIX,ACCEPTANCE,HANDOFF}.md` (Endgame Portfolio 174/175/191/192/199)
   - `docs/plans/wave8_part2/D1_{PREMISE_EVIDENCE,CHANGE_MATRIX,ACCEPTANCE,HANDOFF}.md` (Verification Truth)
   - `docs/plans/wave8_part2/D2_{PREMISE_EVIDENCE,CHANGE_MATRIX,ACCEPTANCE,HANDOFF}.md` (SurvivorInspection Retirement)
   - `docs/plans/wave8_part2/D3_{PREMISE_EVIDENCE,CHANGE_MATRIX,ACCEPTANCE,HANDOFF}.md` (Shutdown Cleanliness)
   - `docs/plans/wave9_part2/*` (Merchant Restock, SignalTrust Retirement, Radiation Balance, Distress Audio/Tranches)
   - `docs/plans/wave10_part1/WAVE9_PART1_CLOSEOUT.md` & `A1_BRIEFING_DEFERRED.md`
3. **Repository debt register:** `KNOWN_DEBT.md`
4. **Authoritative data schemas and catalogs:** `Assets/StreamingAssets/Data/items.json`, `radio.json`, `duty_roles.json`

---

## 2. Inventory of Recorded Micro-Deferrals

Structured schema: `Item | Parent package | Recorded text | Type | Current evidence | Required owner | Disposition | Verification`

| Item | Parent package | Recorded text | Type | Current evidence | Required owner | Disposition | Verification |
|---|---|---|---|---|---|---|---|
| **Radio Weather Predictions** | C2 Plan 20C §39 / `WORKTREE_OWNERSHIP.md` line 26 | `"the radio layer has no authored weather predictions yet (documented; the radio/station distinction activates when that data exists)"` | `DECISION-NEEDED` | `WeatherStationSystem` / `WeatherIntelligenceCoordinator` is the sole prediction authority. `DailyBriefingReportBuilder` attributes forecast misses to station calibration vs unwarned storm. `radio.json` contains static narrative radio lines (`radio_broadcast_01`, `05`, lines 173, 344), but no structured weather prediction catalog or bridge exists. | Radio / Weather / DailyBriefing | `DECISION-NEEDED` (Route decision: does radio weather remain diegetic static flavor on 88.5 MHz, or should a future expansion author structured radio forecast feeds? Stops at signature per Rule 10; no mechanism invented). | `CatalogIntegrityValidatorTests`, `DailyBriefingCrisisTests`, `WeatherEffectsCatalogTests` |
| **`water_sample_contaminated` Equipability Quirk** | C2 Plan 21 / `WORKTREE_OWNERSHIP.md` line 30 | `"foreman flag: water_sample_contaminated equipability quirk untouched"` | `DECISION-NEEDED` | In `Assets/StreamingAssets/Data/items.json` line 3426, `water_sample_contaminated` has `isEquipable: true`, `equipSlot: "Body"`, `radProtection: 20`. No gameplay system or test asserts or consumes this equipability. | Inventory / Item Catalog | `DECISION-NEEDED` (Recommendation: remove `isEquipable: true`, `equipSlot`, and `radProtection` in an approved items data tranche or retain as deliberate wasteland lore quirk. Stops at signature per Rule 10). | `CatalogIntegrityValidatorTests` |
| **Medical Ward Staffing Authority** | Plan 24 / `C1_planintegration[5]_IMPLEMENTATION_LOG.md` & `WORKTREE_OWNERSHIP.md` line 24 | `"Ward staffing = decision memo presented in the Plan 24 log (recommendation: option b, data-authored ward duty role); NO staffing authority fabricated — awaiting foreman signature."` | `DECISION-NEEDED` | Medical ward currently operates without a dedicated duty role; `duty_roles.json` defines 5 canonical roles. Option B proposed in Plan 24 log. | Duty Roster / Medical Ward | `DECISION-NEEDED` (Route to foreman/user; awaiting signature before modifying `duty_roles.json` or `MedicalWardSystem`). | `Plan24DutyRosterFitnessTests` |
| **Affliction-Specific Recovery Ramp** | Plan 24 / `C1_planintegration[5]_IMPLEMENTATION_LOG.md` & `WORKTREE_OWNERSHIP.md` line 24 | `"The affliction-specific recovery ramp = design note awaiting signature (admissions carry no cause field; no dormant data)."` | `DECISION-NEEDED` | Admissions in `MedicalWardSystem` carry no cause/affliction-specific field. | Medical Ward | `DECISION-NEEDED` (Route to foreman/user; awaiting signature). | `Plan24SurvivorJourneyTests` |
| **Amputation Equipment Restrictions** | Wave 8 Part 2 C2 / `docs/plans/wave8_part2/C2_DECISION.md` & `WORKTREE_OWNERSHIP.md` line 19 | `"Phase 2 (equipment restriction) correctly SPLIT — the equipment model has no handedness/slot-limb field, so it needs a separate signed schema package (C2 §2)."` | `CODE-GAP-LARGE` | `ExpeditionSystem` survivor speed multiplier is wired; equipment model lacks limb-slot semantics. | Inventory / Equipment / Survivors | `PROMOTED-TO-QUEUE` (Promoted to Wave 10 queue; requires signed schema extension package). | `C2AmputationTravelTests` |
| **Subject-Focus Queries & 31B Snapshots** | Wave 10 A1 / `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` & `WORKTREE_OWNERSHIP.md` line 13 | `"Subject-focus queries, 31B snapshots remain deferred."` | `CODE-GAP-SMALL` | Plan 31 Briefing Route Map implemented; subject queries and panel snapshots deferred to full UI replay diagnostics wave. | UI / Briefing | `DECIDED-DEFERRED` (Queued with Plan 31B UI remainder). | `Plan31BriefingRouteTests` |
| **Presenter Skill Tree** | Plan 173 / `KNOWN_DEBT.md` line 47 | `"Plan 173 presenter skill tree is retired/stale."` | `STALE/RETIRED` | Stated as retired in `KNOWN_DEBT.md`. Radio broadcasting does not feature individual RPG skill progression. | Governance / Radio | `STALE/RETIRED` | `KNOWN_DEBT.md` |
| **Phobia Growth System** | Plan 177 / `KNOWN_DEBT.md` line 36 | `"Phobia growth system is retired/superseded by trauma/sanity model."` | `STALE/RETIRED` | Superseded by psychological trauma and morale contagion model in Core. | Governance / Psychology | `STALE/RETIRED` | `KNOWN_DEBT.md` |
| **Black-Market Trade Actions** | Wave 8 Part 1 deferral | `"Black-market immediate canonical-inventory settlement"` | `ALREADY-RESOLVED` | Fully sealed in Wave 8 Part 2 C1 (`docs/plans/wave8_part2/C1_HANDOFF.md`, `Plan211BlackMarketSettlementTests.cs` 18/18). | Economy / Black Market | `ALREADY-RESOLVED` (Sealed Wave 8 Part 2 C1). | `Plan211BlackMarketSettlementTests` |
| **Merchant Restock Priority** | Wave 9 Part 1 deferral | `"Merchant restock priority formula and trade ledger updates"` | `ALREADY-RESOLVED` | Fully sealed in Wave 9 Part 2 C1 (`Plan147RestockPriorityTests.cs` 14/14). | Economy / Barter | `ALREADY-RESOLVED` (Sealed Wave 9 Part 2 C1). | `Plan147RestockPriorityTests` |
| **SignalTrust Availability Consumer** | Wave 9 Part 1 deferral | `"SignalTrust availability consumer wiring"` | `ALREADY-RESOLVED` | Audited and retired in Wave 9 Part 2 C2 (`docs/radio/SIGNAL_TRUST_CONTRACT.md`). | Radio / Signal Trust | `ALREADY-RESOLVED` (Retired Wave 9 Part 2 C2). | `SignalTrustTests` |
| **Autopsy Procedures Catalog Quarantine** | Wave 8 Part 1 deferral | `"Autopsy procedures catalog test quarantine"` | `ALREADY-RESOLVED` | Promoted and passing in Wave 8 Part 2 C3 (`AutopsyProceduresCatalogTests.cs` 7/7). | Medical / Autopsy | `ALREADY-RESOLVED` (Sealed Wave 8 Part 2 C3). | `AutopsyProceduresCatalogTests` |
| **Distress Follow-Up Audio Content Tranches** | Wave 9 Part 1 deferral | `"Distress follow-up audio cue validation and content tranches"` | `ALREADY-RESOLVED` | Authored and sealed in Wave 9 Part 2 D1 (`Assets/StreamingAssets/Data/radio_distress_signals.json`). | Radio / Distress Signals | `ALREADY-RESOLVED` (Sealed Wave 9 Part 2 D1). | `DistressFollowUpTests` |
| **Radiation Balance Findings F1–F8** | Wave 9 Part 1 deferral | `"Radiation balance simulation findings F1-F8"` | `ALREADY-RESOLVED` | Audited and documented in Wave 9 Part 2 C3 (`docs/balance/BALANCE_SIM_radiation_exposure_20A.md`). | Radiation / Balance | `ALREADY-RESOLVED` (Sealed Wave 9 Part 2 C3). | `Plan20BShieldingBalanceSweepTests` |
| **Briefing Crisis Consumer** | Wave 9 Part 1 deferral | `"DailyBriefing crisis prediction consumer"` | `ALREADY-RESOLVED` | Fully sealed in Wave 9 Part 1 B1 (`DailyBriefingCrisisTests.cs` 8/8). | Campaign / Daily Briefing | `ALREADY-RESOLVED` (Sealed Wave 9 Part 1 B1). | `DailyBriefingCrisisTests` |

---

## 3. Dispositions & Breakdown

- **ALREADY-RESOLVED:** 7 items (Black-Market trade actions, Merchant restock priority, SignalTrust availability, Autopsy procedures quarantine, Distress follow-up content, Radiation balance F1–F8, Briefing crisis consumer).
- **DECISION-NEEDED:** 4 items (Radio weather predictions, `water_sample_contaminated` equipability quirk, Ward staffing decision, Affliction-specific recovery ramp). All stop at signature per Rule 10.
- **CODE-GAP-LARGE:** 1 item (Amputation equipment restrictions — requires handedness/limb-slot schema extension). Promoted to Wave 10 queue.
- **CODE-GAP-SMALL / DECIDED-DEFERRED:** 1 item (Subject-focus queries & 31B snapshots — scheduled for UI diagnostic wave).
- **STALE/RETIRED:** 2 items (Presenter skill tree, Phobia growth system).

---

## 4. Authored Content Rows

- **No new mechanism was invented:** Per §4.14 and Rule 10, no synthetic radio prediction schema was fabricated, because the weather station (`WeatherStationSystem` / `WeatherIntelligenceCoordinator`) is the sole authoritative weather predictor, and radio currently serves as narrative and static civil-defense announcements.
- **`water_sample_contaminated`:** Remained untouched in `items.json` line 3426 pending foreman signature on whether to strip equipability attributes or preserve as wasteland lore.

---

## 5. Decisions Routed to Foreman / User

1. **Radio Weather Predictions Architecture Decision:**
   - *Question:* Should radio stations broadcast dynamic structured weather forecasts that can be compared against the weather station in `DailyBriefingReportBuilder`, or should radio weather remain purely atmospheric narrative broadcasts in `radio.json` (e.g. `radio_broadcast_05`) while the weather station remains the sole predictive authority?
   - *Recommendation:* Retain the weather station as the sole forecast authority. The briefing attribution ("the station predicted otherwise — check calibration" / "no station forecast covering today") already provides complete, fair gameplay legibility.
2. **`water_sample_contaminated` Equipability Decision:**
   - *Question:* Should `items.json` line 3426 (`isEquipable: true`, `equipSlot: "Body"`, `radProtection: 20`) be stripped of equipability attributes (`isEquipable: false`, null slot), or retained as intentional wasteland dark humor / anomaly lore?
   - *Recommendation:* Strip `isEquipable: true` and `radProtection: 20` to avoid misleading inventory filtering.
3. **Medical Ward Staffing Decision:**
   - *Question:* Approve Option B (data-authored `duty_role_medical_ward` in `duty_roles.json`) to staff the medical ward through the canonical duty roster assignment engine.
4. **Affliction-Specific Recovery Ramp Decision:**
   - *Question:* Author an explicit affliction-cause field on medical ward patient admission records, or continue using global bed-tier recovery curves.

---

## 6. Code Gaps Promoted to Queue

1. **Amputation Equipment Restrictions (Split from Wave 8 C2):**
   - Promoted to Wave 10 Part 2 queue. Requires schema addition of handedness and limb-slot compatibility on `ItemDefinition` / `EquipmentSlot`, and gating equipping 2-handed weapons or boots when limbs are amputated.

---

## 7. Parent-Plan Truth Reconciliation

- **Plan 20C (Weather Presentation):** Reconciled. The §39 forecast-miss attribution is 100% operational in `WeatherWorldDayOwner` and `DailyBriefingReportBuilder`. The parent plan remains COMPLETE; the lack of a separate dynamic radio forecast mechanism is now formally classified as a routed architectural decision rather than an uncompleted defect.
- **Plan 21 (Condition Authority):** Reconciled. All six rad-protective items have authored degrade rates. The `water_sample_contaminated` equipability quirk is formally routed to the decision register.
- **Plan 24 (Survivor Ledger):** Reconciled. Remains `CLOSED-WITH-DEFERRALS` as correctly recorded in `docs/plans/PLAN_24_CLOSEOUT.md`, pending foreman signature on ward staffing and recovery ramps.

---

## 8. Verification

- All 15 tracked deferrals have verified evidence, classification, and terminal disposition.
- No speculative systems or unsigned balance changes were introduced.
- Verified test targets:
  - `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/` (112/112 PASS)
  - `bash scripts/run_test.sh Ashfall.Core.Tests/World/` (439/439 PASS)
  - `bash scripts/run_test.sh Ashfall.Core.Tests/Radio/` (323/323 PASS)
  - Full fast CI tier: all 48 gates pass cleanly.
