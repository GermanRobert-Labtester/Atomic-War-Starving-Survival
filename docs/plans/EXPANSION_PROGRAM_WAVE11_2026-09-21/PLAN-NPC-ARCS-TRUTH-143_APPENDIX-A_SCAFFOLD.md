# PLAN-NPC-ARCS-TRUTH-143 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-NARRATIVE-ARC-EVENT-TRUTH-176`](../EXPANSION_PROGRAM_WAVE14_2026-09-21/PLAN-NARRATIVE-ARC-EVENT-TRUTH-176.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `NAE-176A` | event model + trigger/owner table. |
| `NAE-176B` | once-per-campaign firing ledger + reload tests. |
| `NAE-176C` | deterministic intra-day ordering test (paired runs). |

## 2. Source inventory (4 files, Core + host)

| File | Lines |
|---|---:|
| `NpcArcs/NpcArcCatalog.cs` | 161 |
| `NpcArcs/NpcArcSystem.cs` | 198 |
| `host:Host/HostCli.NpcArcSelfTest.cs` | 164 |
| `host:Main.NpcArcs.cs` | 67 |

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: `Main.NpcArcs.cs`
- Proposed method names: `SetupNpcArcsNAScaffold` / `SaveNpcArcsNAScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/NpcArcs/NA143ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class NA143ScaffoldTests
{
    [Fact] public void NAT143A_TODO() { /* arc model + stage/gate table. */ }
    [Fact] public void NAT143B_TODO() { /* gate-source tests (each source one fixture). */ }
    [Fact] public void NAT143C_TODO() { /* pause/resume + death-mid-arc tests. */ }
    [Fact] public void NAT143D_TODO() { /* completion-state test consumed by a Plan 130/137 fixture. */ }
    [Fact] public void NAT143E_TODO() { /* save round-trip; no stage replay on load. */ }
    [Fact] public void NA143_AuthorityConformance_TODO() { /* pattern parity with PLAN-NARRATIVE-ARC-EVENT-TRUTH-176 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/NpcArcs/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
