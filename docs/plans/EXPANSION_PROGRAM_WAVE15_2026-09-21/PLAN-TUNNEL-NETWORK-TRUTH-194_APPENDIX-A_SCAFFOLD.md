# PLAN-TUNNEL-NETWORK-TRUTH-194 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-DEEP-STRATA-83`](../EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-DEEP-STRATA-83.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `DS-83A` | shaft/level model: depth bands, shoring requirements, lift logistics, stability. |
| `DS-83B` | strata survey+extraction: yields from authored strata; GPR reveals seams. |
| `DS-83C` | hazard cycle: methane accumulation, flooding, subsidence with mitigation protocols; incidents typed. |

## 2. Source inventory (1 files, Core + host)

| File | Lines |
|---|---:|
| `Underground/TunnelNetworkSystem.cs` | 441 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `underground_tunnels.json` | object(3 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupUndergroundTNScaffold` / `SaveUndergroundTNScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Underground/TN194ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class TN194ScaffoldTests
{
    [Fact] public void TNT194A_TODO() { /* graph model + endpoint attachment check. */ }
    [Fact] public void TNT194B_TODO() { /* dig cost + consumption tests. */ }
    [Fact] public void TNT194C_TODO() { /* collapse risk/blockage fixtures. */ }
    [Fact] public void TNT194D_TODO() { /* passage capacity + travel-leg contract. */ }
    [Fact] public void TNT194E_TODO() { /* save round-trip; no silent state change on load. */ }
    [Fact] public void TN194_AuthorityConformance_TODO() { /* pattern parity with PLAN-DEEP-STRATA-83 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Underground/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
