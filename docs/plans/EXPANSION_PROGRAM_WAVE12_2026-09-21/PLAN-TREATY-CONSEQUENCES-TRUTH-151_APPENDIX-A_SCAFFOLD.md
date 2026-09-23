# PLAN-TREATY-CONSEQUENCES-TRUTH-151 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-WARLORDS-DIPLOMACY-29`](../EXPANSION_PROGRAM_WAVE3_2026-09-21/PLAN-WARLORDS-DIPLOMACY-29.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|

## 2. Source inventory (11 files, Core + host)

| File | Lines |
|---|---:|
| `Diplomacy/DiplomaticTreatyCatalog.cs` | 137 |
| `Foundry/SilentFoundrySystem.TreatyLabor.cs` | 179 |
| `Narrative/RegionalTreatyCatalog.cs` | 152 |
| `RegionalTreatyCatalogLoader.cs` | 96 |
| `RegionalTreatyFeed.cs` | 111 |
| `RegionalTreatySystem.cs` | 389 |
| `Treaties/TreatyConsequences.cs` | 191 |
| `host:Host/RegionalTreatyHostSession.cs` | 63 |
| `host:Host/RegionalTreatySaveStore.cs` | 57 |
| `host:UI/AquiferTreatyConcessionPanel.cs` | 101 |
| `host:UI/RegionalTreatyPanel.cs` | 161 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `foundry_treaty_consequences.json` | object(3 keys) |
| `treaty_templates.json` | object(2 keys) |
| `regional_treaty_protocols.json` | object(3 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupTreatiesTRScaffold` / `SaveTreatiesTRScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Treaties/TR151ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class TR151ScaffoldTests
{
    [Fact] public void TCT151A_TODO() { /* term table (type, state owner, breach condition). */ }
    [Fact] public void TCT151B_TODO() { /* keep/breach/expiry outcome rows wired to owners. */ }
    [Fact] public void TCT151C_TODO() { /* breach fixture per term type + notice path check. */ }
    [Fact] public void TCT151D_TODO() { /* value-movement rows handed to Plan 96 (tribute/trade). */ }
    [Fact] public void TCT151E_TODO() { /* persistence via the owner's section + reload test. */ }
    [Fact] public void TR151_AuthorityConformance_TODO() { /* pattern parity with PLAN-WARLORDS-DIPLOMACY-29 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Treaties/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
