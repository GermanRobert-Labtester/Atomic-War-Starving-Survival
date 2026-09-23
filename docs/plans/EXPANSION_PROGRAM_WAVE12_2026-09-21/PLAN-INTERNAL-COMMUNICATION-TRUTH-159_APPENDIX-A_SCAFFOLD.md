# PLAN-INTERNAL-COMMUNICATION-TRUTH-159 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-PRINT-MEDIA-TRUTH-128`](../EXPANSION_PROGRAM_WAVE10_2026-09-21/PLAN-PRINT-MEDIA-TRUTH-128.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `PMT-128A` | press input model + consumption through the inventory seam. |
| `PMT-128B` | reach model + per-place-type test. |
| `PMT-128C` | content sourcing rules (authored entries only) + a fixture issue. |

## 2. Source inventory (9 files, Core + host)

| File | Lines |
|---|---:|
| `Communication/InternalCommunicationSystem.cs` | 509 |
| `Communication/TimeCapsuleSystem.cs` | 460 |
| `Communications/CommunicationsSystem.cs` | 561 |
| `Radio/NvisCommunicationsSystem.cs` | 295 |
| `host:Host/TimeCapsuleHostSession.cs` | 99 |
| `host:Host/TimeCapsuleSaveStore.cs` | 31 |
| `host:Host/TimeCapsuleSelfTest.cs` | 129 |
| `host:Main.TimeCapsule.cs` | 82 |
| `host:UI/TimeCapsulePanel.cs` | 215 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `nvis_communications_catalog.json` | object(2 keys) |
| `communications_networks.json` | object(5 keys) |
| `communication_templates.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: `Main.TimeCapsule.cs`
- Proposed method names: `SetupCommunicationIC2Scaffold` / `SaveCommunicationIC2Scaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Communication/IC2159ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class IC2159ScaffoldTests
{
    [Fact] public void ICT159A_TODO() { /* bulletin model + audience resolution from Plan 141. */ }
    [Fact] public void ICT159B_TODO() { /* internal mail + delivery states; obligation link where applicable. */ }
    [Fact] public void ICT159C_TODO() { /* time capsule lifecycle (create → sealed → open-day/condition → opened) + no-earl */ }
    [Fact] public void ICT159D_TODO() { /* read-record-only rule: no generated text on open. */ }
    [Fact] public void ICT159E_TODO() { /* persistence round-trip for all three record types. */ }
    [Fact] public void IC2159_AuthorityConformance_TODO() { /* pattern parity with PLAN-PRINT-MEDIA-TRUTH-128 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Communication/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
