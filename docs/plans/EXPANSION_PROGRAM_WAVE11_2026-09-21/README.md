# ASHFALL Expansion & Integration Program — Wave 11 (2026-09-21)

Fifteen new plans (131–145). **Selection was evidence-driven**: a directory
audit compared all 120 Core subsystem directories against the 126 existing
plans, and every plan below covers a subsystem with zero-to-low plan coverage.

**None is a claim.** Foreman owns `INTEGRATION_PLANS.md` / `WORKTREE_OWNERSHIP.md`.

## Plan 1, expanded again (M–O)

| File | Size | Content |
|---|---:|---|
| `…_APPENDIX-M_CATALOG_BINDING.md` | ~25 KB | verified catalog bindings per orphan: existence, record counts, loader references (54 orphans name-match a catalog) |
| `…_APPENDIX-N_SURFACE_ROUTES.md` | ~6 KB | candidate surface route ids per orphan from the 192 declared routes |
| `…_APPENDIX-O_VERIFICATION_COMMANDS.md` | ~13 KB | a concrete focused command per orphan (109 test regions · 218 selftest flags) |
| **Plan 1 total (A–O)** | ~330 KB | |

## Uncovered subsystems mounted (131–145)

| # | Plan | Subsystem (previously unplanned) |
|---|---|---|
| 131 | Player Command Truth | `PlayerCommand/` (5 files: action log, context, preview, result, codes) |
| 132 | Narrative Consequence Truth | `NarrativeConsequence/` (graph, simulator, validator) |
| 133 | Utility AI Truth | `UtilityAI/` (action, scorer, system, headless demo) |
| 134 | Thirdonary Covenant Truth | `Thirdonary/` (4 files) + the `thirdonary` save section |
| 135 | Sky Defense Truth | `SkyDefense/` (battery system, ordnance catalog) |
| 136 | Moral Choice Truth | `MoralChoice/` (16 files: flags, chains, faction reactions, gossip) |
| 137 | Endgame Evaluation Truth | `Endgame/` (11 files; reconciles **two** resolver paths) |
| 138 | Feedback Surface Truth | `Feedback/` (8 files: catalogs, dedup, service/interface) |
| 139 | Standing Record Truth | `StandingRecord/` (6 files: site memory, layout, encounters) |
| 140 | Advanced Machinery Contracts Truth | `AdvancedMachinery/` (1 contracts-only file — consumers before implementations) |
| 141 | Institutions Truth | `Institutions/` (availability interface, assignment ledger) |
| 142 | Memory Decay Truth | `Cognition/MemoryDecaySystem.cs` (protected facts, fade rules) |
| 143 | NPC Arcs Truth | `NpcArcs/` (catalog, system) |
| 144 | Sanatorium Truth | `Sanatorium/` (facility system, therapy catalog) |
| 145 | Starting Level Truth | `StartingLevel/` (state, system; preset×scenario precedence) |

## Programme state (141 plans)

| Wave | Plans | Chars |
|---|---|---|
| 1 | 6 + Appendices A–O | ~330 KB |
| 2 | 10 | ~83 KB |
| 3 | 10 | ~65 KB |
| 4 | 10 | ~62 KB |
| 5 | 10 | ~63 KB |
| 6 | 20 | ~64 KB |
| 7 | 15 | ~72 KB |
| 8 | 15 | ~62 KB |
| 9 | 15 | ~55 KB |
| 10 | 15 | ~53 KB |
| 11 | 15 | this wave |

Counts are indicative; Plan 100 replaces README tables with a generated index.

## Ordering

- **137** first: resolving two ending authorities is a correctness fix, not a feature.
- **131, 138** pair (codes ↔ messages); **136, 132** pair (choices ↔ consequence rules).
- **133, 139, 142** share the determinism contract (Plan 89): land together.
- **140** is a decision plan: it retires contracts or names consumers before any build.
- **141, 143, 144** feed existing owners (69/43/64): no model rewrites.
- **134** depends on the save ladder (Plan 87) for its section.
- **135** after Plan 80's alarm owner; **145** after Plan 102's scenario schema.

## Standing rules

Core engine-free; one authority per concern; JSON authority with live
consumers; seeded RNG with replay equality; typed errors and visible failure;
provenance for content; focused verification only; fictional original content.


## Claim readiness

Every plan in this programme carries a **§24 Claim readiness** block (claim
template, verification commands, dependencies, checklist). The cross-wave
index — execution order, hubs, free starts, and residual gaps — is
[`../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md`](../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md).
