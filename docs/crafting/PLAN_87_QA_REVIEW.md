# Plan 87 — Cross-Tool QA Review

*Reviewer: independent QA pass over commit `f2fa9804` (diff + spec only).
Caveat: performed by the same agent session that implemented Plan 87; every
claim below was re-derived from source, not from implementation notes.*

## Verdict: QA PASS with findings

No data defects found in the 15-relic expansion. One medium **pre-existing**
exposure documented (R1), three low/info findings with follow-up recommendations.

## Mechanical re-verification (re-derived)

| Check | Result |
|---|---|
| 15 cultural / 24 tech / 39 total; tech tier byte-identical | PASS |
| relic_id / world_flag uniqueness across all 39 | PASS |
| All `required_components` resolve in items.json | PASS |
| All `dialogue_event_id` resolve in events.json | PASS |
| All `research_unlock_id` empty (16-pinned contract preserved) | PASS |
| Time/morale inside existing distribution (2–12 h, 3/5) | PASS |
| Flag convention `relic_restored_<relic_id>` | PASS |
| data-integrity / content-utilization / bridge selftests | PASS |

## Findings

### R1 — MEDIUM (pre-existing) — relic restoration has no player route

- No production code calls `WorkshopReverseEngineeringSystem.StartRepair`
  (the only src `StartRepair` is SilentFoundry's unrelated foundry repair).
- `WorkshopPanel.RenderLegacy()` (`src/UI/WorkshopPanel.cs:220`) is an
  **empty method** — its comment claims it "keeps it functional"; it does not.
  Production binds the ShelterWorkshop path (`Main.PlayerSurfaces.cs:233`,
  consuming `workshop_recipes.json`), never the legacy relic overload.
- `ContentUtilizationScanner:1000` maps `relic_recipes.json` to a
  `RelicPanel` that **does not exist** in src.
- Consequence: the catalog is loaded, ticked, and saved
  (`CraftingHostSession:113/344/358`), and the morale/event/flag deltas are
  emitted by `CompleteJob` — but no player action can start a relic repair,
  so the human-moment loop fires only in tests/headless flows.
- **This applies equally to the original six relics** — it predates Plan 87
  and matches the AGENTS.md UI-13/UI-21 finding class (EXISTS ≠ WIRED ≠
  PLAYER-FACING). Plan 87's scope explicitly excluded new workshop UI (§3).
- **Disposition:** follow-up UI wiring task — bind the relic catalog into a
  panel, wire StartRepair/StartDismantle through `CraftingHostSession`, and
  route `OnActionCompleted` deltas (morale/flag) + `LastEvent`-style feedback,
  following the UI-11/UI-12 resolution pattern. No data changes required.

### R2 — LOW (pre-existing) — component obtainability is at parity, not better

`item_optical_flat`, `item_magnetic_bearing_coil`, `mechanical_components`,
and `machine_oil` (new recipes) have no loot/craft/excavation path — but the
same is true of most existing-six components (`phonograph_needle`,
`projector_bulb`, `film_reel`, `music_box_comb`, `spring_key`,
`typewriter_ribbon`, `camera_lens_cleaner`, `photographic_film`). The new
relics are not harder to supply than the originals; the gap is
economy-wide and belongs to the Plan 46 handoff.

### R3 — LOW — `relic_component` tag inconsistency

Existing-six components carry `relic_component` in
`expansion_item_tags.json`; the new components are untagged. Verified the
tag has **no runtime gate** (data-only enrichment), so zero functional
impact. Recommended one-line data follow-up for convention consistency.

### R4 — INFO — `relic_antique_brass_telescope` is not a relic

The ID exists only as an art-asset stem alias in `AssetRegistry.cs:128`
(texture mapping for `item_theodolite_brass_precision`). No collision with
the new `telescope` relic; semantic-niche audit unaffected.

### R5 — INFO — Plan 47/76 deferral re-confirmed with concrete hooks

- **Plan 47:** all 40 collectibles bind to `item_id`s with gameplay effects
  (16 categories, incl. `cultural_artifact`). Relics have no item identity,
  so collectible hooks require item authorship — correctly deferred.
- **Plan 76/46:** the 54 scavenging tables use a simple
  `{item_id, weight, min/max_quantity, rarity_tier}` schema. Placing relic
  components into themed tables (e.g., `item_optical_flat` → school/military
  tables) is a **data-only** follow-up that would close the R2 gap for all
  15 relics at once.

## Semantic-niche audit — CONFIRMED

All 15 cultural niches are distinct. Specific re-checks:
- violin vs gramophone/music_box — live performance vs recorded/mechanical (text-authored distinction, per plan §87E);
- hand_printing_press vs typewriter — many public copies vs one personal copy;
- laboratory_microscope vs film_projector — slide projector correctly replaced;
- mantel_clock vs `relic_mechanical_timer_switch` (tech tier) — cultural timekeeping vs functional interval switch, different categories and niches;
- box_kite vs `relic_improvised_anemometer` — play vs measurement.

## Recommended follow-ups (separate tasks)

1. **Relic repair UI route** (R1) — the only blocking gap between the data
   and the player-facing loop.
2. **Plan 46 handoff execution** (R2/R5) — place relic components (old and
   new) into themed scavenging tables; data-only.
3. **Tag the nine new components** `relic_component` (R3); data-only.

## Follow-up dispositions (executed)

1. **R1 — DONE.** `WorkshopPanel.BindRelicWorkshop` binds the relic catalog
   alongside the shelter crafting system; the empty `RenderLegacy` stub is
   now a full restoration view (catalog grouped cultural/technical,
   per-component held/need readouts, RESTORED markers, start/abandon with
   busy-state gating). Main binds it on the `workshop` route
   (`Main.PlayerSurfaces.cs`) and routes completion deltas to the real
   authorities: shelter-wide morale via `SurvivorsHostSession.Needs.Modify`
   (Phase0 shelter-delta pattern) and `flag_*` deltas via
   `CampaignConsequenceLedger.Set` (`Main.World.cs WireRelicRestorationDeltas`).
   Both are one-shot by Core's `completedRelicIds` guard + ledger idempotence.
2. **R2/R5 — DONE.** All 26 cultural-relic components now appear in themed
   scavenging tables (observatory → optics, printworks/municipal_archive →
   press components, concert_hall → violin, school/hospital → microscope,
   apartment_block → domestic relics, geological_survey/power_substation →
   compass coil, etc.). Zero unobtainable components remain.
3. **R3 — DONE.** All 26 relic-recipe components now carry
   `relic_component` in `expansion_item_tags.json` (the original six's
   components were partially untagged too; the set is now complete).
