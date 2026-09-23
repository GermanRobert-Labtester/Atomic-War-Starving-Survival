# PLAN-HEIRLOOM-PHANTOM-TRUTH-149 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-SECRETS-CONFESSION-TRUTH-127`](../EXPANSION_PROGRAM_WAVE10_2026-09-21/PLAN-SECRETS-CONFESSION-TRUTH-127.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `SCT-127A` | secret model + pressure source table. |
| `SCT-127B` | disclosure paths + one test per path. |
| `SCT-127C` | pressure typed input into Plan 64. |

## 2. Source inventory (11 files, Core + host)

| File | Lines |
|---|---:|
| `Narrative/DwellerHeirloomCatalog.cs` | 103 |
| `Narrative/WireConfessionCatalog.cs` | 87 |
| `PhantomMemoryEngine.cs` | 455 |
| `Phantoms/ConfessionSecretCatalog.cs` | 129 |
| `Phantoms/ConfessionSecretSystem.cs` | 288 |
| `Phantoms/HeirloomCatalog.cs` | 108 |
| `Phantoms/HeirloomSystem.cs` | 486 |
| `Phantoms/PhantomTriggerDto.cs` | 56 |
| `host:Host/PhantomMemoryHostSession.cs` | 400 |
| `host:Host/PhantomMemorySaveStore.cs` | 51 |
| `host:UI/PhantomMemoryPanel.cs` | 335 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `phantom_heirlooms.json` | 12 |
| `phantom_triggers.json` | 20 |
| `confession_secrets.json` | 38 |
| `dweller_heirlooms_master.json` | object(3 keys) |
| `heirloom_seed_viability_reports.json` | 7 |
| `wire_confessions.json` | object(3 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupPhantomsHPScaffold` / `SavePhantomsHPScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Phantoms/HP149ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class HP149ScaffoldTests
{
    [Fact] public void HPT149A_TODO() { /* heirloom lifecycle + item-authority rule. */ }
    [Fact] public void HPT149B_TODO() { /* provenance record + display-from-record test. */ }
    [Fact] public void HPT149C_TODO() { /* phantom trigger fields + seeded, day-based evaluation tests. */ }
    [Fact] public void HPT149D_TODO() { /* bequest path gated by Plan 43 state + conservation check. */ }
    [Fact] public void HPT149E_TODO() { /* save round-trip; no trigger re-fire on load. */ }
    [Fact] public void HP149_AuthorityConformance_TODO() { /* pattern parity with PLAN-SECRETS-CONFESSION-TRUTH-127 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Phantoms/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
