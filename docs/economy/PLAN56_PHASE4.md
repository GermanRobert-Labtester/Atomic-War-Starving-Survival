# Plan 56 Phase 4 — Provenance Surfaced in the Market UI

> Completes the arc: `regionalSupply` provenance is now visible to the player
> as an accessibility-safe text tag on every market row.

## 1. The label

`RegionalSupplyRouter.ProvenanceLabel(catalog, originRegion, goodId)` —
pure, testable vocabulary (pinned so UI copy stays stable):

| Condition | Label |
|---|---|
| Origin region produces the good (annotation in the origin's tag pool) | `locally made` |
| Universal/general supply (`regionalSupply: general`) | `general supply` |
| Origin does not produce it | `imported` |
| Unknown/unannotated good | *(empty — no tag rendered)* |

## 2. Panel wiring

`EconomyMarketPanel`:
- `CurrentRegion` property (default `"settlement"` — the shelter market);
  hosts bind the region they are evaluating from.
- Each trade row appends a **text tag** `[locally made]` / `[imported]` /
  `[general supply]` — Plan 14 compliant: never color-only, readable by
  screen readers, same-size font tier, fixed-width column so rows align.

## 3. Snapshot regression pin

`SnapshotHarness.Targets` gains `market_default` (1280×800) with the
`EconomyMarketSnapshotFixture` — binds a real catalog-loaded
`EconomyHostSession` and evaluates provenance from the shelter region.

Verified under `xvfb-run` (real renderer):
- First diff: `market_default` captured as `NEW`; all pre-existing goldens
  `MATCH` (two unrelated pre-existing drifts: `journal_default` 0.80%,
  `shelter_hud_default` 0.00% — day-dependent content, not Plan 56 scope).
- Golden promoted to `snapshots/market_default.png` (LFS-tracked) →
  re-diff: `[MATCH] market_default` — future provenance/price-row drift is
  now regression-caught.
- Note: `godot --headless --import` crashes in this environment (mono/CLR
  `propagate_notification` during the import batch — pre-existing, occurs
  without Plan 56 changes), so the new PNG has no `.import` sidecar. Harmless:
  the snapshot diff decodes goldens via raw `Image.LoadFromFile` (verified),
  not the imported texture.

## 4. Verification

- `Plan56Phase4Tests` (5): locally-made/imported/general-supply mapping,
  empty-for-unknown safety, and a vocabulary lock across all 48 goods × 5
  regions (labels are always one of the four allowed strings).
- Full battery: tests 6605/6605 · both builds clean · data-integrity /
  bridge / economy / caravan selftests PASS · `--ui-snapshots` diff run
  with `market_default` MATCH.
