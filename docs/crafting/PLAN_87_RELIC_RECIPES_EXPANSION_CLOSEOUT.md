# Plan 87 — Relic Recipes Expansion Closeout

**Status: COMPLETE — data expansion with placement deferred**

> Relic catalog expansion complete.
> Collectible/expedition placement deferred until Plan 47/76 IDs are committed.

Pure data + narrative-authoring pass. **No Core code changed. No save schema changed.**

## Authoritative source file

`Assets/StreamingAssets/Data/relic_recipes.json` — confirmed via
`RelicCatalogLoader.cs` (`FileName` constant). The brief's `relic_inks.json`
does not exist anywhere in the repository; it was a phantom reference.
See `RELIC_RESTORATION_RUNTIME_CONTRACT.md` for the full resolved contract.

A material discovery during audit: the catalog already contained 30 records —
the 6 cultural relics (`category: "relic"`) plus 24 later functional
`relic_tech_*` records. Plan 87's "six relics" evidence matched exactly the
cultural tier, so the 6→15 target applies to cultural relics; the 24 tech
records are preserved untouched (final total: 39 recipes).

## Existing six (unchanged)

gramophone (8h/5), film_projector (6h/5), ham_radio (12h/5),
music_box (4h/3), typewriter (3h/3), camera (5h/3).

## Final nine additions

| Relic | Time (h) | Morale | Components (all existing items) | Event | Flag |
|---|---:|---:|---|---|---|
| mantel_clock | 4 | 3 | spring_mechanism, mechanical_parts, machine_oil | narrative_mantel_clock_restored | relic_restored_mantel_clock |
| sewing_machine | 6 | 5 | mechanical_parts, machine_oil, leather_strap | narrative_sewing_machine_restored | relic_restored_sewing_machine |
| telescope | 9 | 5 | item_optical_flat, mechanical_components, scrap_wood | narrative_telescope_restored | relic_restored_telescope |
| hand_printing_press | 12 | 5 | mechanical_components, paper_stock, machine_oil, scrap_wood | narrative_hand_printing_press_restored | relic_restored_hand_printing_press |
| violin | 7 | 5 | wood_block, copper_wire_10m_of_10m, mechanical_parts | narrative_violin_restored | relic_restored_violin |
| laboratory_microscope | 8 | 3 | item_optical_flat, item_cast_borosilicate_glass_blank, mechanical_parts, machine_oil | narrative_laboratory_microscope_restored | relic_restored_laboratory_microscope |
| brass_compass | 3 | 3 | item_magnetic_bearing_coil, mechanical_parts, item_cast_borosilicate_glass_blank | narrative_brass_compass_restored | relic_restored_brass_compass |
| box_kite | 2 | 3 | cloth, scrap_wood, rope | narrative_box_kite_restored | relic_restored_box_kite |
| coffee_grinder | 4 | 3 | mechanical_parts, spring_mechanism, scrap_wood | narrative_coffee_grinder_restored | relic_restored_coffee_grinder |

## Rejected / replaced proposals

- **Slide projector — replaced by laboratory_microscope.** The existing
  `film_projector` already occupies the shared-visual-presentation niche.
- No replacement-pool relics were needed; the remaining eight survived the
  semantic-niche audit against the existing six.

## New component items

**None.** All 27 component references across the nine new recipes reuse the
existing `items.json` vocabulary. New-item threshold review in
`RELIC_COMPONENT_INVENTORY.md` (clock_spring, telescope_lens, violin_string,
printing_ink, compass_magnet, kite components — all rejected in favor of
existing equivalents). No new item requires Core behavior.

## Narrative events

Nine new records added to `Assets/StreamingAssets/Data/events.json`, following
the existing `{id, title, bodyText, weight, minDay}` schema and the
`narrative_<relic_id>_restored` convention of the original six. Every
`dialogue_event_id` is a Tier-2 validated reference (integrity PASS).

## World flags

Nine new flags following the existing `relic_restored_<relic_id>` convention,
one per relic, no collisions (verified programmatically). Flags are emitted by
the runtime as `flag_<world_flag>` deltas on repair completion.

## Save behavior

No change. Restoration state persists through the existing
`WorkshopState.completedRelicIds` via `CraftingHostSession`
(`save.WorkshopState`); one-shot completion, morale, and event semantics are
guarded by the existing `WorkshopReverseEngineeringSystemTests`.

## Plan 47 / Plan 76 status

- **Plan 47 (collectibles):** `collectibles.json` has 40 entries, none
  relic-linked; no committed relic collectible category or IDs. **Deferred.**
- **Plan 76 (expedition destinations):** 75 destinations exist but relic
  discovery routes through scavenging/loot tables (Plan 46 handoff); no
  committed relic-placement IDs. **Deferred.**

## Verification results

| Check | Result |
|---|---|
| Catalog count (cultural tier) | **PASS** — 15 relics (6 + 9) |
| Relic ID uniqueness | **PASS** (39 unique recipe IDs) |
| Component refs | **PASS** — all resolve in items.json |
| `research_unlock_id` contract | **PASS** — unchanged (16 resolved unlocks) |
| `dotnet build Ashfall.Core.Tests` | **PASS** — 0 errors |
| `dotnet test Ashfall.Core.Tests` | **PASS** — 9713/9713 (full re-run; one flaky unrelated `PanelLifecycleTests` UI failure in the first run, passes in isolation and has zero relic references) |
| `dotnet build Ashfall.csproj` | **PASS** — 0 errors |
| `--data-integrity-selftest` | **PASS** — 0 errors, 0 warnings, 298 catalogs |
| `--content-utilization-selftest` | **PASS** |
| `--bridge-selftest` | **PASS** — exit 0 |
| Core code changes | **None** |
| Save schema changes | **None** |

## Restoration-text tone

All nine restoration texts and event bodies follow the existing six's
register: concrete machine detail, one human response, no symbolism
explaining. Drafts from the plan's §87I quality bar were used, compressed
where the event schema's existing body lengths demanded it.
