# PLAN-SKY-DEFENSE-TRUTH-135 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-BASE-DEFENSE-RAIDS-61`](../EXPANSION_PROGRAM_WAVE6_2026-09-21/PLAN-BASE-DEFENSE-RAIDS-61.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `BD-61A` | warning net: sensors + watch produce warnings with time-to-contact and a false-alarm band; false alarms have a |
| `BD-61B` | watch/muster: readiness score drives response; overwork and low fitness reduce it (duty fitness owner). |
| `BD-61C` | raid resolution: attackers use doctrine/faction state; defense uses readiness + fortification + air support; o |

## 2. Source inventory (6 files, Core + host)

| File | Lines |
|---|---:|
| `SkyDefense/SkyDefenseBatterySystem.cs` | 459 |
| `SkyDefense/SkyDefenseOrdnanceCatalog.cs` | 101 |
| `host:Host/HostCli.SkyDefense.cs` | 130 |
| `host:Host/SkyDefenseBatterySaveStore.cs` | 26 |
| `host:Main.SkyDefense.cs` | 85 |
| `host:UI/SkyDefenseBatteryPanel.cs` | 636 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `sky_defense_ordnance.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: `Main.SkyDefense.cs`
- Proposed method names: `SetupSkyDefenseSDScaffold` / `SaveSkyDefenseSDScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/SkyDefense/SD135ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class SD135ScaffoldTests
{
    [Fact] public void SDT135A_TODO() { /* readiness model + owner table. */ }
    [Fact] public void SDT135B_TODO() { /* ordnance accounting through the inventory seam + conservation test. */ }
    [Fact] public void SDT135C_TODO() { /* interception rules per threat class + seeded variance test. */ }
    [Fact] public void SDT135D_TODO() { /* alarm input wiring from Plan 80 (no local alarm state). */ }
    [Fact] public void SDT135E_TODO() { /* save round-trip + no-re-roll-on-load test. */ }
    [Fact] public void SD135_AuthorityConformance_TODO() { /* pattern parity with PLAN-BASE-DEFENSE-RAIDS-61 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/SkyDefense/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
