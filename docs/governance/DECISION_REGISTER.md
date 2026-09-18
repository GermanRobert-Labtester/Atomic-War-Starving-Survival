# ASHFALL Standing Decision Register

**Authority:** Wave 10 Part 2 — Task E1\
**Status:** **ACTIVE / STANDING REGISTER**\
**Last Updated:** 2026-09-18\
**Owner:** Foreman / Integrator\
**Invariant:** Every decision item must have a terminal verdict (`SIGNED`, `DECLINED`, `DEFERRED-WITH-CONDITION`, or `RETIRED`). Zero items may remain unsigned without a condition.

---

## 1. Decision Inventory

| Decision ID | Topic | Memo / Source | Routed Date | Owner | Current Verdict | Condition | Execution Package | Evidence | Recheck Trigger |
|---|---|---|---|---|---|---|---|---|---|
| `DEC-01` | Medical Ward Staffing Authority | `docs/plans/C1_planintegration[5]_IMPLEMENTATION_LOG.md` | 2026-09-16 | Duty Roster / Medical | `DEFERRED-WITH-CONDITION` | Option B (data-authored `duty_role_medical_ward` in `duty_roles.json`) selected in principle; deferred until medical ward capacity reaches >4 patients in campaign progression. | Wave 11 Medical Expansion | `duty_roles.json`, `MedicalWardSystem.cs` | When patient bed capacity or triage rules expand beyond Phase 0 baseline |
| `DEC-02` | Black-Market Trade Actions Settlement | `docs/plans/wave8_part2/C1_DECISION.md` | 2026-09-17 | Economy / Black Market | `SIGNED` | Immediate canonical-inventory settlement; no unsigned shadow funds or fake goods ledgers. | Wave 8 Part 2 C1 (`SEALED`) | `Plan211BlackMarketSettlementTests.cs` (18/18 PASS), `BlackMarketSettlementService.cs` | Closed; permanent architectural rule |
| `DEC-03` | Amputation Equipment Restrictions | `docs/plans/wave8_part2/C2_DECISION.md` | 2026-09-17 | Inventory / Equipment | `DEFERRED-WITH-CONDITION` | Equipment model lacks handedness/limb-slot fields; requires schema extension package rather than inline patch. | Wave 11 Equipment Schema Tranche | `C2AmputationTravelTests.cs`, `AmputationSystem.cs` | Schema migration wave for equipable item slots |
| `DEC-04` | Endgame Portfolio Disposition (174/175/191/192/199) | `docs/plans/wave8_part2/C3_DECISION.md` | 2026-09-17 | Roadmap / Endgame | `SIGNED` | **191 RETIRED** (item inspection + barter appraisal already live); **174, 175, 192, 199 HOLD** with concrete criteria. | Wave 8 Part 2 C3 (`SEALED`) | `Next-steps-plans/Plan_191_*.md` RETIRED banners; `C3_HANDOFF.md` | Wave 12 Endgame Milestone review |
| `DEC-05` | Merchant Restock Priority Formula | `docs/plans/wave9_part2/C1_DECISION.md` | 2026-09-17 | Economy / Barter | `SIGNED` | Priority-weighted tier restock with deterministic PRNG; trade ledger updates canonically. | Wave 9 Part 2 C1 (`SEALED`) | `Plan147RestockPriorityTests.cs` (14/14 PASS), `ShelterBarterSystem.cs` | Closed; permanent architectural rule |
| `DEC-06` | SignalTrust Availability Consumer | `docs/plans/wave9_part2/C2_DECISION.md` | 2026-09-17 | Radio / Signal Trust | `SIGNED` | Zero-consumer SignalTrust pool retired to avoid parallel trust authority; canonical faction trust remains sole authority. | Wave 9 Part 2 C2 (`SEALED`) | `docs/radio/SIGNAL_TRUST_CONTRACT.md` | Closed; permanent architectural rule |
| `DEC-07` | Radiation Balance Findings F1–F8 | `docs/balance/BALANCE_SIM_radiation_exposure_20A.md` | 2026-09-17 | Radiation / Balance | `SIGNED` | Simulation findings recorded as authoritative baselines; shield curves and acute sickness thresholds pinned. | Wave 9 Part 2 C3 (`SEALED`) | `Plan20BShieldingBalanceSweepTests.cs` (12/12 PASS) | Major difficulty tuning pass |
| `DEC-08` | F1/F9 Governance & Documentation Sync | `docs/plans/wave9_part2/D2_DECISION.md` | 2026-09-17 | Governance | `SIGNED` | Re-aligned documentation, rulebook sync, and debt ledgers with canonical Godot architecture. | Wave 9 Part 2 D2 (`SEALED`) | `docs/INDEX.md`, `KNOWN_DEBT.md` | Periodic governance audits |
| `DEC-09` | `water_sample_contaminated` Equipability Quirk | `docs/plans/WAVE10_MICRO_DEFERRAL_SWEEP.md` | 2026-09-17 | Inventory / Catalog | `DEFERRED-WITH-CONDITION` | Retained as intentional wasteland lore quirk (`isEquipable: true`, body slot, rad protection 20); do not strip without catalog review. | Wave 11 Data Hygiene Tranche | `items.json` line 3426 | Catalog integrity audit wave |
| `DEC-10` | Game State Undo Mechanism | Pre-foreman Rulebook / `AGENTS.md` | 2026-09-12 | Core / Save | `DECLINED` | Architectural rejection: Invariant 4 strictly forbids undo mechanics in deterministic survival management. | None (Declined) | `AGENTS.md` Rule 4, `JOURNEYS.md` | Immutable project invariant |
| `DEC-11` | Voice-Over Audio Pipeline | `KNOWN_DEBT.md` | 2026-09-12 | Audio / UI | `DEFERRED-WITH-CONDITION` | Full VO audio generation deferred until dialogue string freeze and audio bus loudness calibration. | Wave 12 Audio Finalization | `docs/audio/AUDIO_POLICY.md` | Campaign dialogue text freeze |
| `DEC-12` | Anti-Tamper Client Obfuscation | Historical Architecture Review | 2026-09-12 | Engine / Security | `DECLINED` | Client obfuscation / DRM rejected for open single-player .NET domain core. | None (Declined) | Architecture guidelines | Immutable project invariant |
| `DEC-13` | Localization Pipeline Expansion | `docs/plans/C1_planintegration[7].md` | 2026-09-15 | UI / Localization | `DEFERRED-WITH-CONDITION` | Multi-language translation catalog deferred until UI string extraction and string freeze in Wave 12. | Wave 12 Localization | `docs/ui/LOCALIZATION.md` | UI string extraction pass |
| `DEC-14` | Plans 170–199 HOLD Recheck Conditions | `docs/plans/wave8_part2/C3_DECISION.md` | 2026-09-17 | Roadmap / Proposals | `SIGNED` | Explicit recheck triggers codified: 174 (origin seam), 175 (profile store), 192 (route DTOs), 199 (population owner). | Wave 8 Part 2 C3 (`SEALED`) | `C3_DECISION.md` §3 | Next roadmap planning milestone |
| `DEC-15` | Radio Weather Prediction Authority | `docs/plans/WAVE10_MICRO_DEFERRAL_SWEEP.md` | 2026-09-17 | World / Radio / Briefing | `SIGNED` | `WeatherStationSystem` remains the sole forecast authority. Radio retains diegetic static/atmospheric emergency lines. | Wave 10 Part 1 Task A3 (`SEALED`) | `DailyBriefingReportBuilder.cs`, `WeatherStationSystem.cs` | Closed; permanent architectural rule |
| `DEC-16` | Affliction-Specific Recovery Ramps | `docs/plans/WAVE10_MICRO_DEFERRAL_SWEEP.md` | 2026-09-17 | Medical / Survivors | `DEFERRED-WITH-CONDITION` | Medical ward admissions continue using global bed-tier recovery curves until patient admission records author an explicit cause field. | Wave 11 Medical Expansion | `MedicalWardSystem.cs` | Authoring of clinical admission cause schemas |
| `DEC-17` | Presenter Skill Tree | `KNOWN_DEBT.md` line 47 | 2026-09-12 | Radio / Progression | `RETIRED` | Radio station production operates through equipment and program production, not individual character RPG skill trees. | None (Retired) | `KNOWN_DEBT.md`, `Plan173RadioProgramProductionTests.cs` | Closed; retired architectural concept |
| `DEC-18` | Phobia Growth System | `KNOWN_DEBT.md` line 36 | 2026-09-12 | Survivors / Psychology | `RETIRED` | Superseded by psychological trauma, guilt insomnia, and morale contagion systems in Core. | None (Retired) | `KNOWN_DEBT.md`, `MoraleContagionSystem.cs` | Closed; retired architectural concept |
| `DEC-19` | Survivor Inspection Orphan Projection | `docs/plans/wave8_part2/D2_DECISION.md` | 2026-09-17 | Survivors / Presentation | `RETIRED` | Zero-consumer projection deleted; live component owners and `SurvivorDetailPanel` remain authoritative. | Wave 8 Part 2 D2 (`SEALED`) | `D2_HANDOFF.md`, deleted `SurvivorInspectionHostSession.cs` | Closed; deleted orphan projection |
| `DEC-20` | C2[12] User-Level Completion-History Boundary | User authorization, 2026-09-18; `C-integration-plans/C2_planintegration[12].md` | 2026-09-18 | Endgame / Completion History | `SIGNED` | A named user-level store may retain append-only, checksum-validated history derived from canonical ending context. It owns no ending calculation, campaign-save state, difficulty semantics, rewards, unlocks, profile, prestige, or New Game+. | Wave 11 Part 2 B3 (`PARTIALLY-SEALED`) | `docs/plans/wave11_part2/B3_PLAN34_IMPLEMENTATION_LOG.md`; `CampaignCompletionHistory.cs`; B3 tests/selftest | Recheck only when a canonical difficulty authority (34B) or an explicit history/chronicle read contract (34C) is proposed. |

---

## 2. Verdict Definitions

- **`SIGNED`**: Foreman-approved and integrated. The decision is authoritative and permanent.
- **`DECLINED`**: Formally rejected. The proposed mechanic or architecture will not be implemented.
- **`DEFERRED-WITH-CONDITION`**: Bounded postponement. Contains a named blocking condition, execution package, and recheck trigger. Never indefinite.
- **`RETIRED`**: Historical or speculative concept that was superseded, orphaned, or eliminated from scope.

---

## 3. Governance Cadence

The Decision Register is reviewed and reconciled:
1. At the conclusion of every execution wave (Wave closeout gate).
2. During milestone release preparation.
3. Whenever a named `Recheck Trigger` condition is met.
