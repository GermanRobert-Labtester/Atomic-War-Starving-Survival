# D1 — Handoff

**Task:** Wave 8 Part 2, TASK D1 — Unblock Verification Truth
**Status:** **DONE** (standing failures 0; comment/ambiguity recorded; map clean)
**Date:** 2026-09-17
**Next owner:** Foreman / integrator (ledger update), then any population of the
deferred drone/caster/lidar work if a signed plan re-scopes it.

## Outcome

- The inherited ambiguous baseline is now accounted for: every standing failure
  was fixed as a stale contract/fixture/generated-doc, or re-captured as a
  deliberate additive field.
- Full suite: **11,697 / 11,697 PASS, 0 failures**.
- Host build: **0 warnings / 0 errors**.
- Architecture map, docs index, and catalog registry `--check` all in sync.
- No production behavior changed.

## What was intentionally NOT done

1. **`src/Main.Plans126_129.cs` header** — left as-is. The claim that Plans
   127–129 are "pending" is not disproven: no tethered-recon-drone, continuous
   steel-casting, or atmospheric-lidar system exists in source, and the
   127/128/129 closeouts in the tree cover different subjects (plan-number
   drift). Rewriting it would be guessing; recorded here instead.
2. **No real production regressions were found.** All 8 failures were stale
   verification artifacts, not live defects.
3. **No new test-contract linter** was built (D1 Phase 4.3: keep it lightweight).

## Remaining debt / follow-ups

| Item | Disposition |
|---|---|
| Plan 126–129 numbering drift (drone/caster/lidar vs. holdfast/foundry closeouts) | AMBIGUOUS — needs a foreman numbering/authority decision before any header or plan-status edit |
| `MANUAL_PLAYTHROUGH_CHECKLIST.md` header date (2026-08-27) | Stale verification date; a playthrough pass should re-stamp it |
| `TradeSpecialtySystem` static `ProfessionInfo` / `ProfessionItemCategories` | Test-isolation hazard remains in production static state (worked around in the test); a future pass could make the registry instance-scoped |

## Rollback

- Revert the D1 commit. No production code, save schema, data, or generated
  serialization is affected; reverting restores the prior (red) test pins.
- The re-captured `sump_flooding_phase0.json` is the only binary-ish artifact;
  reverting it restores the earlier shape and re-reds the B5B8 fixture test.

## Verification rerun

```
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj        # 11697/11697
dotnet build Ashfall.csproj                                     # 0/0
python3 scripts/ci/generate-architecture-map.py --check         # OK 193 subsystems
python3 scripts/ci/generate-docs-index.py --check               # OK 2416 docs
python3 scripts/ci/generate-catalog-registry.py --check         # OK 617 catalogs
```
