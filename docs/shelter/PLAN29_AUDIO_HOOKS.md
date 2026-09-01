# Plan 29 — Audio Hooks (Task 29B §29B.21, Plan 07B handoff)

> **Status:** brief (Phase 3 pilot). This document specifies machine audio **semantics** —
> what each state should sound like and, critically, the non-audio equivalent that every
> cue must have (§29B.22, Plan 14). Asset production follows Plan 07B's pipeline and
> registry; nothing here ships an asset.

---

## 1. Rules (Plan 29 §9.3, §17)

- Machine identity sound is consistent: the degraded variations derive from the healthy
  family (same base loop, changed texture), never a new unrelated sound.
- Critical fault cues are rare, distinct, and actionable — never background furniture.
- Healthy loops are low-prominence and spatial; they must not stack loudly across
  adjacent machines (the plant room holds two: blower + exhaust).
- **Every audio tell has a non-audio equivalent** (text cue + UI severity). The game is
  fully readable muted (Plan 14 / §29B.22).
- Audio state changes fire on threshold transitions, not continuously (§14).

## 2. HEPA Filtration Stack — "The Lung"

Room: `room_filtration` · owner: `StartingLevelSystem.airFilterHealthPercent` ·
family: `hepa_*` · non-audio equivalents live in the room tooltip (identity overlay) and
the Day-1 directive log (`airHazardWarning` line).

| State | Loop / cue (proposed) | Priority | Repeats? | Text/visual equivalent (authoritative) |
|---|---|---|---|---|
| Healthy | `hepa_loop_healthy` — low blower bed, steady | low, looped, −18 LUFS-ish bed | continuous | Tooltip status "Filtration Stack: Active"; dashboard filter metric |
| Worn (50–69) | healthy loop + slight pitch sag | loop variant | — | status text "wearing"; filter % in HUD readouts |
| Service due (< 50) | `hepa_intake_whistle` (one-shot on crossing, rare re-cue) | **warning** — distinct | cooldown | text tell "The intake whistles on the draw" · `airHazardWarning` flag · filter % readout < 50 |
| Radon spike (≥ 50 Bq/m³) | `hepa_radon_hum` — pitch shift under heavier mix | warning | continuous while ≥ 50 | text tell "The intake hum changes pitch" · radon readout climbing |
| Critical (< 25) | whistle + faint hiss of bleed-through | same cue, louder layer | continuous while < 25 | radon readout climbing; `airHazardWarning` stays raised |
| Failed (0) | blower strain → skip | cue | once per crossing | air quality floor (10%), radon maxed — hard numbers on the HUD |
| Maintenance interaction | `hepa_service_cue` (scrap) / `hepa_replace_cue` (cartridge) | interaction, one-shot | per action | directive log lines the actions already emit |

Truthfulness note: the whistle is justified acoustically — a clogged filter raises
pressure differential; the owner's own warning floor (filter < 50) is the trigger, so the
cue can never sound while the plant is healthy (pinned by `ShelterMachineTellTests`).

## 3. The Silent Foundry (cupola)

No nickname (canon name). Room: `room_foundry`. Condition owner:
`SilentFoundrySystem` facility components; floors from `GetSafetyWarnings()`.

| State | Loop / cue (proposed) | Priority | Repeats? | Text/visual equivalent |
|---|---|---|---|---|
| Healthy idle | low shell-tick ambience only (foundry is "silent" by name) | loop, quiet | — | SilentFoundryPanel heat stage readout |
| At heat (healthy) | deep broad hum, no whistles | loop | — | heat stage label + waste-heat briefing |
| Tuyeres worn (< 35) | `foundry_tuyere_knock` — three-beat knock under blast | cue, distinct | continuous while below | `GetSafetyWarnings()` tuyeres line (owner text) |
| Exhaust losing pull (< 30) | `foundry_exhaust_whine` | cue | continuous | owner line: "fumes will concentrate on the charging floor" |
| Refractory critical (< 25) | spall ring after each heat | cue, distinct | continuous | owner line: "shell could fail under heat" |
| Personality (always) | `foundry_heat_shimmer` — residual shimmer above the cupola | info | continuous | text tell "The air above the cupola shimmers" |
| Personality (always) | `foundry_vibration_tune` — floor vibration pitch rises with brick wear | info | continuous | text tell "The floor vibration has a pitch" |
| Incident (breakout) | handled by expansion audio, not Plan 29 | — | — | incident record + downtime |
| Maintenance interaction | hammer/patch cue on `StartRepair` | one-shot | per repair | repair log with before/after condition |

Rules: degraded variants derive from the healthy family (do not author new identities);
critical cues are rare and distinct; the foundry's two loops never stack at full level
(the exhaust cue ducks the base loop, per §17 "do not stack loud loops").

## 4. Accessibility / fallback (Plan 14)

Every cue above has a text equivalent that the simulation itself already emits or that
the tell projection supplies verbatim (`text_cue`), so **no diagnostic state is audio-only**.
Visual hooks (§29B.23) are optional secondary channels; animation is never the sole
carrier of fault information. Cue families: `hepa_*`, `foundry_*` — registered through the
Plan 07B cue pipeline when produced; missing assets fall back to text/status surfaces with
no simulation change (overlay rule).

---

## 5. Main Generator & Battery Bank

Room: `room_main` · owner: `PowerGridSystem` (FuelUnits / BatteryReserveWh) ·
family: `generator_*`

| State | Loop / cue (proposed) | Priority | Repeats? | Text/visual equivalent |
|---|---|---|---|---|
| Healthy idle | `generator_loop_healthy` — diesel idle through slab, inverter flat note | low, looped | continuous | dashboard power metric |
| Fuel low (< 20) | `generator_fuel_cough` — two coughs before catch | warning | continuous while < 20 | text tell "The generator coughs twice" · dashboard tell row |
| Battery thin (< 10) | `generator_relay_chatter` — relay chatter on heavy load | warning | continuous while < 10 | text tell "The lights dip" · brownout risk readout |
| Brownout leg (< 5%) | `generator_brownout_flicker` — panel flicker on dirty bus | warning | continuous while < 5 | text tell "The panel flickers" · dashboard tell row |
| Personality (always) | `generator_vibration_tick` — metronome tick on mount at idle | info | continuous | text tell "The generator ticks on its mount" |
| Harmless personality | relay click (one-shot glitch, not a loop) | — | — | glitch log only |

## 6. Exhaust Plant & Ducting

Room: `room_filtration` · owner: `VentilationSystem` (exhaustFilterSaturation / ductIntegrity) ·
family: `ventilation_*`

| State | Loop / cue (proposed) | Priority | Repeats? | Text/visual equivalent |
|---|---|---|---|---|
| Healthy idle | `ventilation_loop_healthy` — slow stack draft | low, looped | continuous | dashboard filter metric |
| Saturation loaded (> 80) | `ventilation_rattle` — exhaust leg rattle | warning | continuous while > 80 | text tell "The exhaust leg rattles" · dashboard tell row |
| Pre-warning (> 60) | `ventilation_soot_smell` — soot smell before gauge moves | warning | continuous while > 60 | text tell "The ventilation leg carries a soot smell" · dashboard tell row |
| Harmless personality | phantom draft (one-shot glitch) | — | — | glitch log only |

## 7. Brine Still & Filter Bank — "The Still"

Room: `room_water_pump` · owner: `WaterTreatmentSystem` (filterIntegrity) ·
family: `water_*`

| State | Loop / cue (proposed) | Priority | Repeats? | Text/visual equivalent |
|---|---|---|---|---|
| Healthy idle | `water_loop_healthy` — slow boil, steady gauge | low, looped | continuous | water treatment panel readout |
| Membranes nearly spent (< 40) | `water_flutter` — long draw, stutter | warning | continuous while < 40 | text tell "The still runs long and draws slow" · dashboard tell row |
| Personality (always) | `water_distillation_hum` — catalogue-defying frequency that changes with chemistry | info | continuous | text tell "The still hums at a frequency that is not in the catalogue" |
| Real fault (< 30) | flutter + gauge flutter | warning | continuous | glitch_27_pressure_flutter title + repair kit |

## 8. Shelter Boiler

Room: `room_main` · owner: `ShelterThermalSystem` (boilerFuelLevel / boilerActive) ·
family: `boiler_*`

| State | Loop / cue (proposed) | Priority | Repeats? | Text/visual equivalent |
|---|---|---|---|---|
| Healthy idle | `boiler_loop_healthy` — steady fire, jacket tick | low, looped | continuous | thermal panel readout |
| Personality (always) | `boiler_jacket_tick` — two-beat tick on cool | info | continuous | text tell "The jacket ticks as the fire settles" |
| Fuel low (< 15) | `boiler_cutout_sputter` — sputter before cut | warning | continuous while < 15 | text tell "The boiler sputters" · dashboard tell row |
| Real fault (< 10) | sputter + cutout | warning | continuous | glitch_28_boiler_cutout title + repair kit |
| Harmless once | `boiler_sigh` — long exhalation through jacket, not on cycle | info | once | glitch_29_boiler_sigh title |

## 9. Airlock Machinery

Room: `room_airlock` · owner: `AirlockSecuritySystem` (door state / pending incidents) ·
family: `airlock_*`

| State | Loop / cue (proposed) | Priority | Repeats? | Text/visual equivalent |
|---|---|---|---|---|
| Healthy idle | `airlock_loop_healthy` — heavy latch, equalisation hiss | low, looped | continuous | airlock panel status |
| Pending incident | `airlock_seal_drag` — drag on seal mid-cycle | warning | continuous while incident active | text tell "The door drags on its seal" · dashboard tell row |
| Seal cycling (real fault) | same cue, mechanical rhythm | warning | continuous (cooldown 7d) | glitch_24_seal_cycles title + gasket kit |
| Personality (always) | `airlock_machinery_grind` — grind on up-stroke, seats clean | info | continuous | text tell "The airlock machinery grinds" |
| Harmless personality | intercom burst (one-shot) | — | once | glitch_23_old_intercom_burst |

## 11. New harmless glitch audio hooks (Phase 8)

| Glitch id | Machine | Presentation | Audio cue (proposed) | Repeat |
|---|---|---|---|---|
| `glitch_29_boiler_sigh` | `machine_boiler` | The boiler sighs once through the jacket — a long exhalation of steam not on the cycle. | `boiler_sigh` — low-pressure steam release, non-rhythmic | once |
| `glitch_30_generator_hum_drop` | `machine_generator` | The generator hum drops half an octave for three seconds and recovers. | `generator_hum_drop` — pitch-drop variant of healthy loop, 3s | once |
| `glitch_31_water_still_gurgle` | `machine_water_still` | The water still gurgles once through the trap — a single bubble not on any cycle. | `water_gurgle` — single-bubble through trap | once |

These are explicitly harmless personality moments with no condition gate. They are
journal-persisted one-shots (`glitch_noted_*`), so old saves default un-noted and
discover them once. No repair kit, no severity styling.

## 12. Production notes for Plan 07B (extended)

- New families: `generator_*`, `ventilation_*`, `water_*`, `boiler_*`, `airlock_*` —
  each derived from a single healthy loop with degraded texture layers.
- Adjacent-machine stacking: `room_main` holds generator + boiler; total simultaneous
  loops ≤ 2 (generator idle ducks boiler loop, per §17).
- Personality cues (`hepa_housing_tick`, `boiler_jacket_tick`, `foundry_heat_shimmer`, `foundry_vibration_tune`, `generator_vibration_tick`, `ventilation_soot_smell`, `water_distillation_hum`, `airlock_machinery_grind`) are low-prominence info
  layers that never trigger warning styling.
- Harmless once glitches (`phantom_draft`, `repeating_relay_click`, `old_intercom_burst`, `boiler_sigh`, `generator_hum_drop`, `water_still_gurgle`) are journal-persisted and never stack with diagnostic cues.
- All new cues have text equivalents in the dashboard tell row (§29B.16); no diagnostic
  state is audio-only.
