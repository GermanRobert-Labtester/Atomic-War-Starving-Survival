# PLAN-BIONICS-ENHANCEMENT-78 — Implants, Integration, Maintenance & Ethics

**Wave 7 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-VERTICAL-BODY-INDUSTRY-05, PLAN-SCIENCE-EDUCATION-38,
PLAN-ENERGY-NUCLEAR-48.
**Implementation scaffold:** [`PLAN-BIONICS-ENHANCEMENT-78_APPENDIX-A_SCAFFOLD.md`](PLAN-BIONICS-ENHANCEMENT-78_APPENDIX-A_SCAFFOLD.md) — source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no real medical devices, no graphic surgery, no combat-only
power fantasy; the body authority (`SurvivorBodyState`, `AmputationSystem`,
`BionicsSystem`) stays canonical.

## Outcome
Bionics already exist end-to-end at Core level (Plan 177: `bionics.json`,
`BionicsSystem` 14/14 + host 4/4, surgery through the limb authority, no double
charge, integration/rehab, maintenance/decay, typed malfunctions, grid-gated
charger, passive immunity, combat-damage command; Plan 176 electrostatic
contract live). What is missing is a **full player loop** around them:
indication, fitting, adaptation, upkeep, failure, and the social/ethical cost.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Indication | medical + body state | assess, advise, consent | realistic expectations, ethics |
| Surgery | `BionicsSystem` via limb authority | schedule, prepare | outcome, recovery, complications |
| Integration | rehab engine + `SurvivorBodyState` | train, therapy | capability ramp (never instant) |
| Upkeep | maintenance/decay + charger | charge, service, replace | reliability, failure risk |
| Malfunction | typed malfunction events | diagnose, repair | temporary loss, danger |
| Enhancement | implants with capability caps | choose loadout | bounded bonuses (+200bp cap precedent) |
| Social | relations/faith/politics | reactions | stigma, admiration, policy |
| Ethics/policy | governance | rules for enhancement | shelter factions, access |

## Evidence
- Core: `Medical/BionicsSystem.cs`, `AmputationSystem`, `RehabilitationProgressionEngine` (orphan), `ProstheticConditionWearEngine` (orphan), `SurgicalGraftRejectionEngine` (orphan), `SurvivorBodyState` (DEC-38), `EquipLimbGate` (DEC-21).
- Data: `bionics.json`; power grid/charger contracts; `disease_catalog` for infection risk.
- Sealed prior: Plan 177 (14/14 + 4/4), Plan 176 electrostatic, DEC-03/21/38/41/42.
- Contracts: capability caps; maintenance decays; passive implants immune to electrical disruption; no double charge.

## Packages
- **BI-78A** indication/consent: assessment, expectations, refusal; journal record.
- **BI-78B** fitting loop: rehab phases visible (fitting/adaptation/mastery), quality factor, phantom pain variant (DEC-35).
- **BI-78C** upkeep: charging, condition wear, service tasks; wear engine consumer.
- **BI-78D** malfunctions/rejection: typed events, diagnosis, repair/removal; graft rejection engine consumer.
- **BI-78E** enhancement choices: authored implant catalogue with caps and tradeoffs (energy draw, maintenance burden).
- **BI-78F** social/policy: reactions by belief/faction; shelter policy on access (ties Plan 69).
- **BI-78G** content volumes: +8 implants, +8 complications, +6 policy rows; abstract/fictional.

## Acceptance & verification
- No instant integration; caps enforced; decay and failure deterministic; no double-charge on surgery.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/Plan177BionicsTests.cs`; host wiring tests; `--medical-selftest`.

## Risks
Power creep → caps, upkeep, failure, and energy cost; combat still preparation-based.

---

## 6. Expanded census (5 files · 1,484 lines)

Scope: `Assets/Ashfall.Core/Medical/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BionicsSystem.cs` | 805 | System | — | 0 | 0 | 2 |
| `ProstheticConditionWearEngine.cs` | 157 | System | **yes** | 0 | 0 | 0 |
| `RehabilitationProgressionEngine.cs` | 112 | System | **yes** | 0 | 0 | 0 |
| `RehabilitationSlateProjection.cs` | 126 | Support | — | 0 | 0 | 0 |
| `SurgicalGraftRejectionEngine.cs` | 284 | System | **yes** | 0 | 0 | 4 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `bionics.json` | object[2 keys] |

**State surfaces:** `BionicsSystem.cs`, `SurgicalGraftRejectionEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Medical/` |
| Test references | 7 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain files: 5. Other plans referencing their names: **8**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 4 |
| `EVIDENCE` | 4 |
| `PLAN-ACUTE-TRAUMA-CARE-124` | 4 |
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 4 |
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `PLAN-UNBLOCK-03` | 2 |
| `PLAN-MAINTENANCE-DECAY-TRUTH-119` | 2 |
| `PLAN-SURGICAL-WARD-TRUTH-213` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `BI-78A` | no name match — resolve at claim time |
| `BI-78B` | `RehabilitationProgressionEngine.cs`, `RehabilitationSlateProjection.cs` |
| `BI-78C` | `ProstheticConditionWearEngine.cs`, `RehabilitationProgressionEngine.cs`, `SurgicalGraftRejectionEngine.cs` |
| `BI-78D` | `SurgicalGraftRejectionEngine.cs`, `ProstheticConditionWearEngine.cs`, `RehabilitationProgressionEngine.cs` |
| `BI-78E` | no name match — resolve at claim time |
| `BI-78F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **2** · Test files: **7** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Main.Bionics.cs`, `src/UI/CyberneticsPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 7 | `Ashfall.Core.Tests/Medical/Plan177BionicsHostWiringTests.cs`, `Ashfall.Core.Tests/Medical/Plan177BionicsTests.cs`, `Ashfall.Core.Tests/Medical/ProstheticConditionWearEngineTests.cs`, `Ashfall.Core.Tests/Medical/RehabilitationProgressionEngineTests.cs`, `Ashfall.Core.Tests/Medical/RehabilitationSlateProjectionTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **1**; isolated: **3**.

| From | → To |
|---|---|
| `RehabilitationSlateProjection` | `RehabilitationProgressionEngine` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `bionics` |
| `equipment_condition` |
| `surgical_ward` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **0** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| — | no CLI flag shares a token with this domain |

**Verdict:** no CLI or selftest flag shares a token with this domain — the outcome is not operator-observable yet. Add coverage in the owning plan if it must be verifiable.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnConditionChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnConditionStarted` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConditionStopped` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnDeviceConditionChanged` | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **7**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/bionics.json` |
| `Assets/StreamingAssets/Data/narrative/carbide_tool_wear_audits.json` |
| `Assets/StreamingAssets/Data/narrative/deadbeat_escapement_wear_logs.json` |
| `Assets/StreamingAssets/Data/narrative/operating_theater_surgical_logs.json` |
| `Assets/StreamingAssets/Data/narrative/typographic_lead_wear_logs.json` |
| `Assets/StreamingAssets/Data/narrative_progression.json` |
| `Assets/StreamingAssets/Data/surgical_procedures.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (11 files, 83 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Progression` | 11 | 83 |

**Verdict:** 83 cases sit under matching regions — run those first (`Progression`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **7**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Host/BionicsSaveStore.cs` |
| `src/Host/EquipmentConditionHostSession.cs` |
| `src/Host/SurgicalWardSaveStore.cs` |
| `src/Main.Bionics.cs` |
| `src/UI/AnalogConditionGauge.cs` |
| `src/UI/EquipmentConditionPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `bionics` | no |
| `equipment_condition` | no |
| `surgical_ward` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `bionics` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **6**
(CODEX_ONLY 4, GAMEPLAY_CONSUMED 2).

| Catalog | Classification |
|---|---|
| `narrative/carbide_tool_wear_audits.json` | CODEX_ONLY |
| `narrative/deadbeat_escapement_wear_logs.json` | CODEX_ONLY |
| `narrative/operating_theater_surgical_logs.json` | CODEX_ONLY |
| `narrative/typographic_lead_wear_logs.json` | CODEX_ONLY |
| `narrative_progression.json` | GAMEPLAY_CONSUMED |
| `surgical_procedures.json` | GAMEPLAY_CONSUMED |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 8
**Surface:** save sections 3 (laddered 0) · RNG streams 1 · host files 8 · catalogs 13 · test regions 1 · flags 0

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-BIONICS-ENHANCEMENT-78
wave: 7
status: PROPOSED — foreman claim required
packages: BI-78A, BI-78B, BI-78C, BI-78D, BI-78E, BI-78F, BI-78G
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Host/BionicsSaveStore.cs  # §19 candidate host surface
  - src/Host/EquipmentConditionHostSession.cs  # §19 candidate host surface
  - src/Host/SurgicalWardSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/bionics.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/carbide_tool_wear_audits.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Progression/
dependencies:
  - coordinate: 8 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
