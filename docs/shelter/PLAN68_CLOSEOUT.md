# Plan 68 — Wall Carving Templates — Closeout

**Status: COMPLETE** — pure data + narrative expansion. Zero Core/host code changes.

`NEW SYSTEM JUSTIFICATION: NOT REQUIRED` — the catalog is data-present and consumer-absent; no loader change was needed (see Consumer below).

## Summary

`wall_carving_templates.json` expanded from 15 templates (5 per band) to exactly **60** (20 high / 20 medium / 20 low). Schema untouched: `morale_band` / `morale_min` / `morale_max` / bare-string `templates` / `carving_chance`. All 15 original templates preserved verbatim (KEEP per §1.4 — all are tonally correct, distinct, and pinned by contract tests).

## Baseline

- 3 bands × 5 templates. Windows: high 60–100, medium 30–59, low 0–29. `carving_chance` 0.3 / 0.2 / 0.15.
- Baseline suite: 9407/9407 tests, data-integrity 0 findings, build clean.

## Consumer

`grep -rn "wall_carv\|WallCarv"` across Core/src finds **no runtime loader**: the content-utilization scanner maps the file to `MemorialSystem`/`MemorialPanel` aspirationally, and no Core/host/UI code parses it. Band selection, RNG, repeat handling, and save-state ownership are therefore **not exercised by any runtime today**. The pre-existing `Plan68WallCarvingTests.cs` (previously quarantined in the csproj, per the established pattern) records the same finding and validates the JSON directly through a probe DTO. When a carving consumer lands, it should adopt this shape; selection/RNG/persistence audits (§29–§35) become actionable at that point.

## Final counts

20 / 20 / 20 = 60. No empty strings, no whitespace-only entries, all ≤140 characters.

## Existing template audit

| Band | Verdicts |
|---|---|
| high | 5/5 KEEP — tally, sun, stick figures, HOPE/STILL, imaginary cake (all pinned by test) |
| medium | 5/5 KEEP — 47-day tally, date question, crossing-out list, black-sky drawing, miss coffee |
| low | 5/5 KEEP — bare tallies, scratched-out name, tiny I'm sorry, empty clock, WHY |

Replaced: 0. Light edits: 0.

## Tone grammar

- **high** — modest hope as evidence: tallies that keep going, children's drawings, names of the living, planting plans, repair pride, house rules, dry jokes. No triumphalism.
- **medium** — documentary routine: ration grids, filter/battery counts, duty rosters, corrected lines, dry complaints, unfinished schedules.
- **low** — grief/fear/exhaustion with restraint: stopped tallies, worn prayers, gouged counts, warnings nobody explains, one-word carvings, minimal ambiguous marks.

## Duplication audit

- Exact duplicates (normalized): **0** within bands, **0** across bands.
- Opening-word distribution — high: a×8/someone×3/the×2; medium: the×5/a×4/someone×2; low: a×8/the×4 — no construction dominates.
- Motif share: tally/mark-based entries 4/20 (high), 3/20 (medium), 3/20 (low) — under the 20–25% ceiling (§22).
- Low-band name motifs: 5/20 — at the test ceiling (≤5), balanced by warnings, prayer, apology, burial marks, and minimal/ambiguous marks.
- Cliché gate (last hope / darkness swallowed / against all odds / light at the end / never give up / tomorrow will come / ashes of the old world / …): **0 hits**.

## Selection

No runtime selector exists. `carving_chance` per band and band windows are preserved unchanged for the future consumer. Adding templates necessarily changes any future seeded sequence — accepted per §31; RNG authority remains unimplemented and untouched.

## Persistence

No save state exists for carvings today. Bare-string schema means any future string-persisted saves are immune to catalog reordering (§35); index-based persistence would need ordering stability — documented here for the future implementer.

## Validation

| Command | Result |
|---|---|
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter Plan68WallCarving` | **13/13 PASS** (file unquarantined from csproj after compile+pass verification, per repo precedent) |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **9439/9439 PASS** |
| `godot --headless -- --data-integrity-selftest` | **PASS** — 0 findings, 298 catalogs |
| `godot --headless -- --content-utilization-selftest` | **PASS** — CI gate PASS |
| `dotnet build Ashfall.csproj` | **PASS** — 0 warnings, 0 errors |
| Script audits (counts, dups, phrase/motif frequency, anchors, clichés, length) | **PASS** — see Duplication audit |

## Deferred

- Room-aware selection / room metadata (Plan 41 integration).
- Event-conditioned and survivor-attributed carvings (§75.5–75.6).
- Runtime carving consumer with band selection, seeded RNG, no-repeat rotation, and save persistence — a future host feature, not a Plan 68 defect.
- Folklore/grave-domain cross-linking (Plans 30A/69) — no verbatim duplication present.
