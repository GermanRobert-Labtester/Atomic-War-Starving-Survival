# PLAN-HOST-CLI-CONTRACT-86 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, host-inclusive scan).
**Scaffolding authority:** [`PLAN-HOST-COMPOSITION-GOVERNANCE-71`](../EXPANSION_PROGRAM_WAVE7_2026-09-21/PLAN-HOST-COMPOSITION-GOVERNANCE-71.md) — this plan adopts the authority's patterns (below) rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `HC-71A` | generated inventory: every partial, its setups, its fields, its session types, its save methods. |
| `HC-71B` | naming/collision gate: unique Setup/Save names; new duplicates fail with both file paths. |
| `HC-71C` | orphan partial sweep: unused fields/setups retired with evidence (do not delete a partial that owns a live ses |

## 2. Source inventory (30 files, Core + host)

| File | Lines |
|---|---:|
| `HostCliRegistry.cs` | 1414 |
| `host:Host/HostCli.AdvancedIndustrialRecon.cs` | 418 |
| `host:Host/HostCli.Cartography.cs` | 129 |
| `host:Host/HostCli.Collectibles.cs` | 496 |
| `host:Host/HostCli.Difficulty.cs` | 241 |
| `host:Host/HostCli.DynamicWorld.cs` | 188 |
| `host:Host/HostCli.EvolvingWorld.cs` | 327 |
| `host:Host/HostCli.ExpansionDepth.cs` | 145 |
| `host:Host/HostCli.ExpeditionPlaytest.cs` | 543 |
| `host:Host/HostCli.ExportParity.cs` | 236 |
| `host:Host/HostCli.FactionCommuniqueSelfTests.cs` | 112 |
| `host:Host/HostCli.Mods.cs` | 100 |
| `host:Host/HostCli.MoralChoice.cs` | 200 |
| `host:Host/HostCli.NpcArcSelfTest.cs` | 164 |
| `host:Host/HostCli.Onboarding.cs` | 183 |
| `host:Host/HostCli.PanelTests.cs` | 4159 |
| `host:Host/HostCli.Plans122to125.cs` | 1017 |
| `host:Host/HostCli.Plans139_141.cs` | 136 |
| `host:Host/HostCli.Plans162_165.cs` | 490 |
| `host:Host/HostCli.PlansB86_B89.cs` | 633 |
| `host:Host/HostCli.SelfTestManifest.cs` | 36 |
| `host:Host/HostCli.SelfTests.cs` | 1230 |
| `host:Host/HostCli.SkyDefense.cs` | 130 |
| `host:Host/HostCli.StartingSupplies.cs` | 155 |
| `host:Host/HostCli.Summary.cs` | 128 |
| `host:Host/HostCli.VehicleGarage.cs` | 243 |
| `host:Host/HostCli.WastelandInhabitants.cs` | 144 |
| `host:Host/HostCli.WorldExploration.cs` | 116 |
| `host:Host/HostCli.WorldPlaytest.cs` | 1009 |
| `host:Host/HostCli.cs` | 834 |

## 3. Data bindings

No catalog name-matched; verify loader paths before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupHostHCScaffold` / `SaveHostHCScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method; no new timer.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Host/HC86ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class HC86ScaffoldTests
{
    [Fact] public void CLI86A_TODO() { /* descriptor census + parity gate: registry dump vs dispatch switch; fail on eithe */ }
    [Fact] public void CLI86B_TODO() { /* exit-code contract: constants + usage table + assertions in the dispatcher; test */ }
    [Fact] public void CLI86C_TODO() { /* generated help: replace any hand-written verb list with descriptor rendering; `- */ }
    [Fact] public void CLI86D_TODO() { /* `--cli-manifest`: deterministic JSON (sorted by flag) consumed by a check script */ }
    [Fact] public void CLI86E_TODO() { /* headless-compat probe: bounded run of headless-compatible descriptors; failures  */ }
    [Fact] public void HC86_AuthorityConformance_TODO() { /* pattern parity with PLAN-HOST-COMPOSITION-GOVERNANCE-71 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Host/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win over new ones; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
