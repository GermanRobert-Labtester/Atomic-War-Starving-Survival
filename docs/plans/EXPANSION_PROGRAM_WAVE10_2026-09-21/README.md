# ASHFALL Expansion & Integration Program — Wave 10 (2026-09-21)

Fifteen new plans (116–130): **fourteen gap-sealing, one major expansion**.
Plan 1 gained a fifth expansion round (appendices J–L).

**None is a claim.** Foreman owns `INTEGRATION_PLANS.md` / `WORKTREE_OWNERSHIP.md`.

## Plan 1, expanded again (J–L)

| File | Size | Content |
|---|---:|---|
| `…_APPENDIX-J_TEST_COVERAGE.md` | ~15 KB | complete test inventory per orphan: every referencing test file with its `[Fact]`/`[Theory]` counts |
| `…_APPENDIX-K_API_SIGNATURES.md` | ~88 KB | public member signatures per orphan (up to 30 per type; omitted counts noted) — the API a host adapter binds to |
| `…_APPENDIX-L_RISK_SCORECARD.md` | ~8 KB | transparent triage score (size + determinism risk + state + missing attachment + test debt), ranked |
| **Plan 1 total (A–L)** | ~285 KB | |

Top scorecard risks: `CommunicationsSystem` (7), `SeasonalCelebrationSystem` (6),
`DisasterResponseSystem` (5), `ShelterExpansionSystem` (5) — expensive or risky
seals, start there.

## Gap sealing (116–129)

| # | Plan | Gap |
|---|---|---|
| 116 | Noise & Light Discipline | the only noise system is the `_Game` file pending Plan 94; detection contract vs Plan 61 is unstated |
| 117 | Thermal Exposure | both clothing engines orphaned; ambient + layers + heating combine without a contract (double-count risk) |
| 118 | Preservation & Spoilage | pressing engine orphaned, lyophilization engine dead, spoil curves unstated |
| 119 | Maintenance & Decay | `equipment_decay_mult` scalar + `ShelterMaintenanceSystem` orphan; no decay/repair contract |
| 120 | Rumor Propagation | `EconomyMarketRumorRules` exists; origin/distortion/expiry unstated |
| 121 | Investigation & Evidence | `Verdict/` is a full subsystem (EvidenceLedger, EvidenceChain, Accusation); chain contract unstated |
| 122 | Commitments & Obligations | `CommitmentSystem` + both letter-delivery systems orphaned; promises have no record |
| 123 | Mortuary & Memorial | memorial data exists; death pipeline, rites, bereavement hand-off unstated |
| 124 | Acute Trauma Care | six medical engines orphaned; wound/triage/surgery/rehab path unowned (F14 schema explicitly out of scope) |
| 125 | Recipe Reachability | no check that recipes are craftable, stations buildable, substitutions legal |
| 126 | Backstory Revelation | `BackstorySystem` orphaned; know/when/from-whom contract unstated |
| 127 | Secrets & Confession | `ConfessionSecretSystem` orphaned; held-secret lifecycle unstated |
| 128 | Print Media | `PublicBroadsheetPressEngine` orphaned; press capacity and distribution reach unstated |
| 129 | Morale & Unrest | `MoraleMarkSystem` exists inside the roster; collective marks/thresholds have no contract |

## Major expansion (130)

| # | Plan | Frontier |
|---|---|---|
| 130 | Muster & Coalition | `Muster/` subsystem (camp, coalition, cold count, faction actions, epilogue matrix) with the `muster` save section — ownership and ending inputs documented |

## Programme state (126 plans)

| Wave | Plans | Chars |
|---|---|---|
| 1 | 6 + Appendices A–L | ~285 KB |
| 2 | 10 | ~83 KB |
| 3 | 10 | ~65 KB |
| 4 | 10 | ~62 KB |
| 5 | 10 | ~63 KB |
| 6 | 20 | ~64 KB |
| 7 | 15 | ~72 KB |
| 8 | 15 | ~62 KB |
| 9 | 15 | ~55 KB |
| 10 | 15 | this wave |

Counts are indicative; Plan 100 replaces README tables with a generated index.

## Ordering

- **Start with Plan 1 Appendix L**; the top-scored seals set the wave order.
- **117, 118, 119** share the item/inventory owner: land with Plan 93/112, focus on food/shelter aid.
- **121, 123, 124** add typed inputs to existing owners (37/64/47/78/81): no model rewrites.
- **120, 128** are the media pair: rumour owns state, print creates rows via it.
- **116** binds after Plan 94 decides the port; packages A/C/E land earlier.
- **125** before any content-authored recipe or industrial chains expand.
- **122, 126, 127** are the narrative/memory trio against 18/43/64/121/126 boundaries.
- **130** after Plan 29/61/80 own their seams; the epilogue input table is the deliverable.

## Standing rules

Core engine-free; one authority per concern; JSON authority with live
consumers; seeded RNG with replay equality; typed errors and visible failure;
provenance for content; focused verification only; fictional original content.


## Claim readiness

Every plan in this programme carries a **§24 Claim readiness** block (claim
template, verification commands, dependencies, checklist). The cross-wave
index — execution order, hubs, free starts, and residual gaps — is
[`../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md`](../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md).
