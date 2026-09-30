# ASHFALL — Year Two: The Long Thaw (Days 361–720)
# Decision Packet (Package P0)

**Date:** 2026-09-30
**Authority:** `.ai/plans/year-two-the-long-thaw-2026-09-29.md` §6
**Parent Plan:** `.ai/plans/y2-p0-premise-audit-2026-09-29.md` (`STATUS: APPROVED BY USER`)
**Approval Record:** User, 2026-09-29, session authorization: *"I authorise the each seperate plan!"*
**Scope:** Formal ratification and recorded choices for architectural decisions DEC-Y2-01 through DEC-Y2-14.

---

## 1. Decision Register

| ID | Title & Scope | Governing Rule | Decision / Ratified Outcome | Status | Impacted Packages |
|---|---|---|---|---|---|
| **DEC-Y2-01** | **Canon Extension: Maintenance Schedule for Thirteen** | Canon | **APPROVED:** Thirteen has been kept by an automated quarterly maintenance schedule filed under a pre-war department that no longer exists. No miracle, no supernatural entity, no secret dweller. Bureaucracy outliving its creators. | **RATIFIED** | P5, P8 (W2) |
| **DEC-Y2-02** | **Storyline-Specific Reckoning & Reading Timing** | Canon & Systems | **DECIDED BY USER (2026-09-29):** The Reckoning phase days and Chapter 1 close (Reading day) are properties of the active Chapter Profile, not universal constants. Default profile maintains Call Day 240 / Reading Day 360. | **DECIDED (USER)** | P1, P1B, P2 |
| **DEC-Y2-03** | **Climate Authority for Days 361–720** | Architecture | **APPROVED:** `YearOfAshTimelineSystem` delegates to `YearTwoClimateCatalog` (`year_two_climate.json`) for `day > 360`. Both replaying Year 1's −45 °C Deep Ash and freezing indefinitely at +4 °C are strictly rejected. | **RATIFIED** | P1 |
| **DEC-Y2-04** | **Held Verdict & Tempest Sterilization Conditions** | Design | **APPROVED:** Epilogue-only advisory in v1. Does not impose real-time gameplay debuffs during active Day 361–720 survival. | **RATIFIED** | P3, P7 |
| **DEC-Y2-05** | **Late Presentation of a Held Count** | Design | **APPROVED:** No late presentation in v1. Once the Reading day closes Chapter 1 with count held, Standing B is established and cannot be retroactively converted to Standing A. | **RATIFIED** | P3 |
| **DEC-Y2-06** | **Outpost Supply Mode Compatibility** | Compatibility | **APPROVED:** The four legacy outposts retain `tether` supply mode (drawn from inventory). Only new Year Two positions (such as Thirteen) enforce `convoy` supply run mode. | **RATIFIED** | P5, P6 |
| **DEC-Y2-07** | **Council Succession Ledger Ownership** | Architecture | **DECIDED IN P0 AUDIT:** The Council succession ledger is nested within `ApprenticeshipState` (`apprenticeship` save section), as mentorship, succession readiness, and acting designations are already owned by `ApprenticeshipSystem`. | **DECIDED (P0 AUDIT)** | P4c |
| **DEC-Y2-08** | **UI Panel Surfaces Architecture** | UI Seams (`INT`) | **APPROVED:** All Year Two surfaces are added as sub-views/pages on existing panels (`ChroniclePanel`, `ApprenticeshipPanel`, `ShelterOperationsPanel`). Zero new routed overlay panels created in v1. | **RATIFIED** | P2, P3, P4, P5 |
| **DEC-Y2-09** | **Storyline-Aware Year One Ending Selection** | Compatibility | **DECIDED BY USER (2026-09-29):** The thin host context is retired for new campaigns in favor of `BuildCampaignOutcomeSnapshot()` and active faction branch resolution. Saves in progress retain the legacy profile. | **DECIDED (USER)** | P1B |
| **DEC-Y2-10** | **Registration Authority ("Who Is Written")** | Architecture | **DECIDED IN P0 AUDIT:** `VoluntaryRegisterSystem` (`voluntary_register` save section) is designated as the sovereign authority for the dweller enrollment fact. | **DECIDED (P0 AUDIT)** | P4d |
| **DEC-Y2-11** | **Black Flotilla Exodus at Day 360** | Design | **APPROVED:** Resolving `ending_exodus_to_sea` operates as a Standing modifier (a friendly flotilla port established), not a total roster wipe or hard game over. | **RATIFIED** | P3, P7 |
| **DEC-Y2-12** | **Chapter Profile Catalog Architecture** | Architecture | **APPROVED:** `chapter_profiles.json` schema defines legacy baseline plus 5 family profiles, covering all 135 faction branch endings with family defaults. | **RATIFIED** | P1B |
| **DEC-Y2-13** | **Verdict Day-Gate Time Translation Seam** | Architecture | **APPROVED:** Single boundary adapter `ReckoningClock` translates `campaignDay -> verdictDay` at the `VerdictHostSession` entry. Zero mutation of authored quest or radio day gates. | **RATIFIED** | P1B |
| **DEC-Y2-14** | **Standing D — The Late Call Resolution** | Design | **APPROVED:** Standing D handles campaigns reaching Day 360 with an uncounted verdict. Resolves into A, B, or C when the Call is reached in Year Two. | **RATIFIED** | P3 |

---

## 2. Implementation Handoff & Governance

With this Decision Packet formally ratified:
1. **Package P0 is 100% complete and verified.**
2. **Package P1 (Horizon Lift)** is unlocked and unblocked to implement `YearTwoClimateCatalog` and extend `YearOfAshTimelineSystem` past Day 360.
3. **Package P1B (Chapter Profiles)** has its architecture, profile taxonomy, and time translation rules sealed.
4. All non-negotiable rules from AGENTS.md (Godot authority, Core engine-free, deterministic RNG, single authority per concern) are respected.
