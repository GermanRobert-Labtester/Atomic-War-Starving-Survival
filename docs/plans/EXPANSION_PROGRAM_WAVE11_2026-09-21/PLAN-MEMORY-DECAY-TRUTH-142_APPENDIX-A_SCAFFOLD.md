# PLAN-MEMORY-DECAY-TRUTH-142 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-BACKSTORY-REVEAL-TRUTH-126`](../EXPANSION_PROGRAM_WAVE10_2026-09-21/PLAN-BACKSTORY-REVEAL-TRUTH-126.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `BRT-126A` | fact model + visibility scopes. |
| `BRT-126B` | revelation hooks per source system with one test each. |
| `BRT-126C` | truth/conflict rule tests (authored record rendered; contradiction reported). |

## 2. Source inventory (13 files, Core + host)

| File | Lines |
|---|---:|
| `Cognition/MemoryDecaySystem.cs` | 536 |
| `Narrative/NpcMemorySystem.cs` | 463 |
| `PhantomMemoryEngine.cs` | 455 |
| `StandingRecord/LocationMemorySystem.cs` | 388 |
| `Survivors/RelationshipDecaySystem.cs` | 428 |
| `host:Host/PhantomMemoryHostSession.cs` | 400 |
| `host:Host/PhantomMemorySaveStore.cs` | 51 |
| `host:Host/RelationshipDecayHostSession.cs` | 78 |
| `host:Host/RelationshipDecaySaveStore.cs` | 31 |
| `host:Host/RelationshipDecaySelfTest.cs` | 96 |
| `host:Main.RelationshipDecay.cs` | 82 |
| `host:UI/PhantomMemoryPanel.cs` | 335 |
| `host:UI/RelationshipDecayPanel.cs` | 191 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `standing_record_memory.json` | 52 |
| `npc_memory_dialogue.json` | object(2 keys) |
| `relationship_decay_profiles.json` | object(2 keys) |
| `memory_decay_rates.json` | object(3 keys) |

## 4. Host attachment

- Candidate host partials: `Main.RelationshipDecay.cs`
- Proposed method names: `SetupCognitionMDScaffold` / `SaveCognitionMDScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Cognition/MD142ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class MD142ScaffoldTests
{
    [Fact] public void MDY142A_TODO() { /* retention class + fade rate table. */ }
    [Fact] public void MDY142B_TODO() { /* protected-fact set + tests (milestones, save-referenced ids). */ }
    [Fact] public void MDY142C_TODO() { /* recall fidelity tests (exact / degraded / absent). */ }
    [Fact] public void MDY142D_TODO() { /* no-erasure guard: records referenced by other owners remain intact. */ }
    [Fact] public void MDY142E_TODO() { /* day-based determinism test (OS clock change no-op). */ }
    [Fact] public void MD142_AuthorityConformance_TODO() { /* pattern parity with PLAN-BACKSTORY-REVEAL-TRUTH-126 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Cognition/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
