# C3 — Handoff

**Task:** Wave 8 Part 2, TASK C3 — Endgame Portfolio Disposition
**Status:** DONE
**Date:** 2026-09-17
**Next owner:** Foreman (portfolio ledger); the owners of any future HOLD→PROMOTE promotion.

## Outcome

- Five-row disposition signed: **0 PROMOTE · 1 RETIRE · 4 HOLD**.
- Plan 191 retired as a standalone system with status banners on both historical
  copies; `DEBT-PLANS170-199-PORTFOLIO` updated with the dispositions and
  recheck conditions.
- Zero production, data, or test changes.

## Remaining HOLD conditions (measurable)

| Plan | Recheck when… |
|---|---|
| 174 | a signed extension point on `SurvivorEnrichmentService`/`TradeSpecialtySystem` (not a new `BackstorySystem`) has a consumed gameplay surface |
| 175 | a signed cross-run profile-store owner (versioned/checksummed, outside campaign slots) exists and Plan 34/149 completion-fact producers are verified |
| 192 | a player-route DTO with standing/raid/save seams is signed in a map amendment |
| 199 | product names a human population owner distinct from fauna |

## Out of C3 scope (unchanged)

181 difficulty, 193 chronic conditions, 194 emergency alerts — not part of the
five-plan C3 set.

## Rollback

- Revert the docs-only commit; no code/save/render impact. The Plan 191 banner
  and debt note are additive.

## Verification rerun

```
python3 scripts/ci/generate-docs-index.py --check
python3 scripts/ci/generate-architecture-map.py --check
dotnet build Ashfall.csproj
```
