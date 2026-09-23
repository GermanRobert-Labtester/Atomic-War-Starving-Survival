# PLAN-KNOCK-WHITELIST-TRUTH-155 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-YEAR-OF-ASH-TRUTH-146`](PLAN-YEAR-OF-ASH-TRUTH-146.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `YAT-146A` | timeline model + phase boundary table. |
| `YAT-146B` | arc contracts (onset/offset/effects) + one scripted arc test each. |
| `YAT-146C` | war chain stage gates + catalog resolution + stall fallback. |

## 2. Source inventory (20 files, Core + host)

| File | Lines |
|---|---:|
| `DutyRoster/ShelterEncounterSystem.cs` | 394 |
| `Encounters/OrphanKnockWhitelist.cs` | 64 |
| `Expeditions/EncounterChoiceResolver.cs` | 133 |
| `Expeditions/ExpeditionEncounterBridge.cs` | 355 |
| `Expeditions/TravelEncounterCombatBinder.cs` | 57 |
| `Narrative/EncounterCatalog.cs` | 160 |
| `Narrative/EncounterChoiceEffectDispatcher.cs` | 95 |
| `Narrative/MicroLocationEncounterLoader.cs` | 109 |
| `Narrative/NarrativeEncounterSystem.cs` | 560 |
| `Narrative/PatrolEncounterValidator.cs` | 337 |
| `Narrative/TravelEncounterCatalog.cs` | 355 |
| `Narrative/TravelEncounterHeadlessDemo.cs` | 159 |
| `Narrative/TravelEncounterSelectionContext.cs` | 62 |
| `Narrative/TravelEncounterSystem.cs` | 1093 |
| `StandingRecord/SiteEncounterSystem.cs` | 252 |
| `YearOfAsh/DoorEncounterCatalogLoader.cs` | 56 |
| `YearOfAsh/DoorEncounterSystem.cs` | 401 |
| `host:Host/EncounterChoiceSaveStore.cs` | 47 |
| `host:Host/TravelEncounterSaveStore.cs` | 48 |
| `host:YearOfAsh/DoorEncounterModal.cs` | 177 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `anomalous_expedition_encounters.json` | object(2 keys) |
| `narrative_encounters_expansion.json` | object(2 keys) |
| `narrative_encounters_npc_arcs.json` | object(2 keys) |
| `crossing_encounters.json` | object(3 keys) |
| `door_encounters.json` | 80 |
| `narrative_encounters.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupEncountersKWScaffold` / `SaveEncountersKWScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Encounters/KW155ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class KW155ScaffoldTests
{
    [Fact] public void KWT155A_TODO() { /* category table (windows, prerequisites). */ }
    [Fact] public void KWT155B_TODO() { /* context input wiring (threat/refuge/war owners). */ }
    [Fact] public void KWT155C_TODO() { /* seeded day-based selection + paired-run determinism test. */ }
    [Fact] public void KWT155D_TODO() { /* refusal outcome table + test per outcome. */ }
    [Fact] public void KWT155E_TODO() { /* persistence: pending/cooldown round-trip, no re-roll on load. */ }
    [Fact] public void KW155_AuthorityConformance_TODO() { /* pattern parity with PLAN-YEAR-OF-ASH-TRUTH-146 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Encounters/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
