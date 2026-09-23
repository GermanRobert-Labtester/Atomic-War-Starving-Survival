# PLAN-STANDING-RECORD-TRUTH-139 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-DISCOVERY-STATE-108`](../EXPANSION_PROGRAM_WAVE9_2026-09-21/PLAN-DISCOVERY-STATE-108.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `DST-108A` | state machine doc + subject/owner table. |
| `DST-108B` | transition tests: rumored→verified (arrival), verified→stale (time/damage), stale→verified (revisit). |
| `DST-108C` | degradation semantics test: damage removes knowledge, place record intact, recovery path restores only earned  |

## 2. Source inventory (14 files, Core + host)

| File | Lines |
|---|---:|
| `Combat/CombatFactionStandingBridge.cs` | 256 |
| `Factions/FactionStandingIdResolver.cs` | 131 |
| `Factions/PrpfStandingSystem.cs` | 204 |
| `Governance/StandingGateRegistry.cs` | 204 |
| `Maritime/BlackFlotillaStanding.cs` | 85 |
| `StandingRecord/LocationLayoutSystem.cs` | 517 |
| `StandingRecord/LocationMemorySystem.cs` | 388 |
| `StandingRecord/SiteEncounterSystem.cs` | 252 |
| `StandingRecord/StandingRecordCatalog.cs` | 161 |
| `StandingRecord/StandingRecordEngine.cs` | 177 |
| `StandingRecord/StandingRecordHeadlessDemo.cs` | 144 |
| `host:Host/StandingRecordHostSession.cs` | 141 |
| `host:UI/StandingRecordAtlasPanel.cs` | 516 |
| `host:UI/StandingRecordPanel.cs` | 345 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `standing_record_layouts.json` | 14 |
| `standing_record_memory.json` | 52 |
| `standing_record_factions.json` | object(2 keys) |
| `standing_record_quests.json` | object(2 keys) |
| `standing_gates.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupStandingRecordSRScaffold` / `SaveStandingRecordSRScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/StandingRecord/SR139ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class SR139ScaffoldTests
{
    [Fact] public void SRT139A_TODO() { /* record model + writer rule (one writer). */ }
    [Fact] public void SRT139B_TODO() { /* memory fade rules in game days + never-fade list tests. */ }
    [Fact] public void SRT139C_TODO() { /* layout stability test (save → load → revisit equals continuous visit). */ }
    [Fact] public void SRT139D_TODO() { /* encounter trigger table + fixture per class. */ }
    [Fact] public void SRT139E_TODO() { /* save round-trip; no re-roll on load. */ }
    [Fact] public void SR139_AuthorityConformance_TODO() { /* pattern parity with PLAN-DISCOVERY-STATE-108 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/StandingRecord/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
