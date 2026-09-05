# Micro-Location Radio Tower — Radio Integration (F19)

Flagship plan §9 deliverable. Proven by `Ashfall.Core.Tests.MicroLocationRadioIntegrationTests` (11 tests).

## Coil item definition and actual radio use

`antenna_coil` (items.json, type Component, 0.2 kg) was **never dead** — F19's job was to prove it, not to invent a use:

- **Produced** by `recipe_workshop_radio_component_refit` ("Tuner Antenna Coil Fabrication", `workshop_recipes.json`).
- **Consumed** by five authored relic repairs (`relic_recipes.json`) through the canonical `WorkshopReverseEngineeringSystem.StartRepair` → `Inventory.TryConsumeBill` path:
  `ham_radio` (Vintage Ham Radio Set), `relic_signal_amplifier_stage`, `relic_field_encrypted_radio`, `relic_portable_radar_scope`, `relic_iff_transponder`.
- Referenced by `narrative_questlines.json` / `dynamic_questlines.json` / `expansion_item_tags.json`.

The functional-use tests prove the full loop: the coil alone **cannot** complete a repair (`F19_04` — `missing_components`, atomic bill refuses to consume), and with the full authored bill the repair consumes exactly one coil through the same transaction any other coil source uses (`F19_05`, `F19_06`). No source-specific "use radio tower coil" path exists.

## Journal integration

`read_radio_log` unlocks `micro_radio_tower_log` through the canonical `JournalSystem.TryDiscoverKnowledge` (single KnowledgeBase dedup gate — exactly once, second attempt returns null). The key stays in the `micro_` namespace per the Plan-49 audit convention. Voice text is composed generically by `JournalVoice`; no brittle prose assertions were added (§9.6).

The choice is authored non-depleting: the log can be read before or after the cabinet salvage; the site only depletes on the coil choice (`F19_08`).

## Frequency discovery support decision

**Deferred — no canonical store to hook.** `RadioTuner` state is the tuned frequency + signal evaluation; there is no persistent "known frequencies / discovered stations" registry that a micro-location effect could write through. Per §9.7's hierarchy, no world flag was added that no radio code reads. When a frequency-registry authority exists, the tower's frequency log is the natural authored hook.

## Progression gates

An item grant is not a free upgrade (§9.8): the coil must be combined with the authored bill (vacuum tube, soldering kit, copper wire…) inside the workshop flow. Radio room construction, antenna installation, research, and power requirements are untouched by the micro-location.

## One-shot behavior

`open_radio_cabinet` depletes the site; re-resolution cannot re-grant; save/reload preserves depletion and the production selector can never re-surface the tower (`F19_02`).

## Deterministic behavior

Fixed authored quantity (1), no RNG in the grant or in the repair's component consumption.

## Save/load behavior

Grant-then-save-then-use (§12.3): the coil round-trips through `Inventory.CaptureState/RestoreState` and still completes a real relic repair after restore (`F19_11`).

## Skill-weighting findings

Micro-location selection supports only stance multipliers today; no player-skill condition or weight input exists in the selection path. Skill-based detection weighting for `micro_radio_tower` (electronics/signal-ear) would be a selection-layer extension — documented as deferred, nothing implemented in the radio system.

## Tests

`F19_01`–`F19_11` in `MicroLocationRadioIntegrationTests`.

## Deferred hooks

Frequency/station discovery (needs a known-frequencies authority), skill-weighted detection, antenna-calibration quest lines building on the tower journal.
