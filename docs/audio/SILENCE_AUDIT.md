# ASHFALL Silence Audit

> **Task 7A deliverable.** Authoritative map of where sound *should* exist but doesn't,
> ranked by player impact. This audit is the spec for Task 7B (cue + VO production) and
> Task 7C (dynamic ambience). Feeds the `ashfall-audio-qa` skill.
>
> **Date:** 2026-08-30
> **Cue source:** `src/Audio/AudioCueCatalog.cs` (49 cues, 12 buses)
> **Bridge source:** `src/Audio/AudioEventBridge.cs`
> **Selftest source:** `src/Audio/AudioSelfTest.cs`

---

## 1. Executive Summary

The audio system is structurally complete but almost entirely **unwired**.

- **49 cues** are registered, and all 49 resource paths resolve to files on disk (0 silent).
- **13 of those 49 cues** are actually triggered by game code (via `PlayCue`, the
  `AudioEventBridge`, and `AudioManager` convenience methods). The remaining **36 are
  dead registrations** — defined in the catalog, never played.
- `AudioEventBridge` subscribes to exactly **2 Core domains** (Radiation, Weather) out of
  ~15 event-emitting systems. Combat, medical, economy, expeditions, crafting, shelter,
  and radio are all silent.
- **Bug found:** `PlayGameplayMusic()` and `PlayMainMenuMusic()` referenced `.wav` paths
  but the assets are `.ogg` — gameplay music silently failed to load. Fixed in 7B.
- **118 radio broadcasts** across 4 JSON files carry **0 audio references**. **5 VO files**
  exist on disk but no cue or JSON entry points to them.
- **19 of 22 weather states** produce no sound on transition (only FalloutStorm, BlackRain,
  Blizzard are mapped).

The soundscape is a catalog with no playback. The single highest-leverage fix is wiring
the existing cues to the existing Core events — most of the work is host-side event
subscription, not new asset production.

---

## 2. Methodology

1. Extracted all 49 cue definitions (ID, bus, path, loop, volume, cooldown) from
   `AudioCueCatalog.RegisterAll()`.
2. Extracted all `event Action<...>` declarations across `Assets/Ashfall.Core/` (excluding
   HeadlessDemo/Test files) to enumerate the full trigger surface.
3. Grepped `src/` for `PlayCue` / `NotifyGameFlow` / convenience-method call sites
   outside the `Audio/` folder to find which cues are actually fired.
4. Cross-referenced every `res://` cue path against `find assets/audio -type f` to verify
   on-disk resolution.
5. Enumerated `WeatherKind` enum values (22 states) and `CombatEvent.Kind` strings (26 kinds)
   from source.
6. Confirmed 0 `audio_cue` fields in all 4 radio JSON files.
7. Ran `generate-audio-catalog.py --check` — passes (catalog doc in sync, 49 cues).
8. **Limitation:** `AudioSelfTest` requires the Godot runtime (`ResourceLoader`,
   `ProjectSettings`). Godot is available at `/home/robertsrff/.local/bin/godot` but the
   initial audit pass verified resource resolution statically by file-existence check.
   All 49 paths resolve. The selftest should be run after 7B code changes.

---

## 3. Trigger Coverage Matrix

### 3.1 Cues that ARE triggered (13 / 49)

| Cue ID | Domain | Trigger site | Event source |
|---|---|---|---|
| `rad_alert_acute` | Radiation | `AudioEventBridge` + `PlayRadiationAlert()` | `RadiationSystem.OnStatusGained` / `SurvivorsHostSession.cs:128` |
| `rad_alert_chronic` | Radiation | `AudioEventBridge.OnRadiationStatusGained` | `RadiationSystem.OnStatusGained` (ChronicIllness) |
| `weather_fallout_storm` | Weather | `AudioEventBridge.OnWeatherChanged` | `WeatherSystem.OnWeatherChanged` (FalloutStorm) |
| `weather_black_rain` | Weather | `AudioEventBridge.OnWeatherChanged` | `WeatherSystem.OnWeatherChanged` (BlackRain) |
| `weather_blizzard` | Weather | `AudioEventBridge.OnWeatherChanged` | `WeatherSystem.OnWeatherChanged` (Blizzard) |
| `weather_alert` | Weather | `PlayWeatherAlert()` | `WorldHostSession.cs:56` |
| `day_transition` | Game flow | Direct `PlayCue` | `Main.Holdfast.cs:237` |
| `game_over` | Game flow | Direct `PlayCue` | `Main.GameFlow.cs:512` |
| `save_success` | Game flow | Direct `PlayCue` | `Main.SaveOrchestrator.cs:302` |
| `ui_click` | UI | `PlayUiClick()` | `AshfallUiHelpers.cs:541` |
| `radio_static` | Radio | `PlayRadioStatic()` | `RadioHostSession.cs:106` |
| `music_gameplay` | Music | `PlayGameplayMusic()` | `Main.GameFlow.cs:97` (was broken — `.wav` path, fixed in 7B) |
| `amb_bunker` | Ambience | `StartBunkerAmbience()` | `Main.GameFlow.cs:98` |

### 3.2 Cues that are NOT triggered (36 / 49)

| Domain | Registered cues | Wired? |
|---|---|---|
| UI | `ui_click`, `ui_confirm`, `ui_warning`, `ui_cancel`, `ui_tab_change`, `ui_modal_open`, `ui_modal_close`, `ui_invalid_action` | **No** — no panel calls `PlayCue` |
| Radiation (extra) | `rad_geiger_burst`, `rad_geiger_loop`, `rad_contamination` | **No** |
| Weather (extra) | `weather_alert`, `weather_wind_gust` | **No** |
| Ambience | `amb_bunker`, `amb_surface` | **No** — no loop start/stop in game loop |
| Music | `music_menu`, `music_gameplay` | **No** |
| Radio | `radio_static`, `radio_tune`, `radio_signal_lock`, `radio_morse` | **No** |
| Shelter | `shelter_door_open`, `shelter_door_seal`, `shelter_ventilation`, `shelter_generator`, `shelter_pipe_clang`, `shelter_water_drip`, `shelter_air_filter` | **No** |
| Actions | `action_item_pickup`, `action_crafting`, `action_repair`, `action_trade`, `action_water_pour`, `action_pill_bottle`, `action_injection` | **No** |
| Medical | `med_heartbeat`, `med_coughing` | **No** |
| Danger | `danger_explosion`, `danger_alarm_klaxon`, `danger_glass_break`, `danger_debris` | **No** |

---

## 4. Silence Matrix by Domain

For each domain: the Core events that exist, whether a cue is registered, and whether it
is wired.

### 4.1 Weather (22 states, 3 wired)

| WeatherKind | Cue registered? | Wired? |
|---|---|---|
| FalloutStorm | `weather_fallout_storm` | Yes |
| BlackRain | `weather_black_rain` | Yes |
| Blizzard | `weather_blizzard` | Yes |
| Ashfall | — | **No** |
| Clear | — | **No** |
| Rain | — | **No** |
| Overcast | — | **No** |
| AcidSnow | — | **No** |
| BioFog | — | **No** |
| BlackSnow | — | **No** |
| BloodRain | — | **No** |
| EMPStorm | — | **No** |
| GlassStorm | — | **No** |
| RadHail | — | **No** |
| AlgaeBloom | — | **No** |
| AshLightning | — | **No** |
| ParticulateFog | — | **No** |
| ThermalInversion | — | **No** |
| IceStorm | — | **No** |
| Silence | — | **No** |
| FalseSpring | — | **No** |
| SilentSpring | — | **No** |

The bridge's `_ => null` default means 19 states transition with zero audio feedback.

### 4.2 Combat (26 event kinds, 0 wired)

`TacticalCombatSystem.OnCombatEvent` is never subscribed by `AudioEventBridge`. All 26
`AddEvent` kinds are silent:

| Event kind | Source file | Audio? |
|---|---|---|
| `encounter_start` | TacticalCombatSystem.cs:187 | No |
| `fire` | Actions.cs:161 | No |
| `miss` | Actions.cs:540 | No |
| `enemy_fire` (hit) | Damage.cs:107 | No |
| `enemy_fire` (miss) | Damage.cs:111 | No |
| `mutual_kill` | Damage.cs:27 | No |
| `downed` | Damage.cs:39 | No |
| `death` | Damage.cs:57 | No |
| `bleed` | Damage.cs:143 | No |
| `ash_dunes` | Damage.cs:161 | No |
| `victory` | Damage.cs:200 | No |
| `defeat` | Damage.cs:218 | No |
| `stance` | Actions.cs:29 | No |
| `jam_persist` | Actions.cs:63 | No |
| `weapon_jam` | Actions.cs:106 | No |
| `weapon_burst` | Actions.cs:116 | No |
| `suppress` | Actions.cs:234 | No |
| `clear_jam` | Actions.cs:255 | No |
| `reload` | Actions.cs:291 | No |
| `repair` | Actions.cs:373 | No |
| `lane` | Actions.cs:388 | No |
| `trap` | Actions.cs:407 | No |
| `decon` | Actions.cs:433 | No |
| `bandage` | Actions.cs:462 | No |
| `retreat` | Actions.cs:494 | No |
| `retreat_fail` | Actions.cs:509 | No |
| `last_stand` | Actions.cs:525 | No |

### 4.3 Expeditions (15 events, 0 wired, 0 cues)

`ExpeditionSystem` events: `OnExpeditionStarted`, `OnExpeditionTick`, `OnPhaseChanged`,
`OnLootAdded`, `OnEncounterTriggered`, `OnVehicleBreakdown`, `OnExpeditionCompleted`,
`OnExpeditionFailed`, `OnStateChanged`, `OnCampEntered`, `OnCampSuppliesReserved`,
`OnCampNightSegmentResolved`, `OnCampEncounterSurfaced`, `OnCampEncounterResolved`,
`OnCampDawnResolved`.

No expedition cues are registered. No bridge subscription.

### 4.4 Medical / Disease (7 events, 0 wired)

`DiseaseSystem` events: `OnInfection`, `OnQuarantineStarted`, `OnQuarantineEnded`,
`OnOutbreakDeclared`, `OnOutbreakContained`, `OnOutcomeResolved`, `OnEventRaised`.

Cues `med_heartbeat` and `med_coughing` exist but have no trigger. No disease cue exists.

### 4.5 Economy / Trade (3 events, 0 wired)

`MarketSystem`: `OnDemandAdjusted`, `OnEconomyChanged`, `OnStateChanged`.
`TradeScreenSeam`: `Changed`.

Cue `action_trade` exists but has no trigger.

### 4.6 Crafting (3 events, 0 wired)

`CraftingSystem`: `OnCraftStarted`, `OnCraftCompleted`, `OnCraftResultOverflow`.

Cue `action_crafting` exists but has no trigger.

### 4.7 Death (0 events, 0 cues)

`NeedsSystem.EvaluateDeath` / `ForceDeath` fire no event. `SurvivorFateSystem.ReportDeath`
returns a fate record but emits no event. There is **no death cue** and **no death event**
to subscribe to — this is a Core gap, not just a wiring gap.

### 4.8 Radio (0 wired, 0 JSON audio refs)

4 cues registered (`radio_static`, `radio_tune`, `radio_signal_lock`, `radio_morse`).
None triggered. 118 broadcasts across `radio.json` (50), `year_of_ash_radio.json` (50),
`verdict_radio.json` (13), `radio_distress_signals.json` (5) carry 0 `audio_cue` fields.
5 VO files on disk (`vo_ch11_stockpile`, `vo_ch3_ash_road`, `vo_ch7_milband`,
`vo_kind_hatch`, `vo_kind_parley`) are unreferenced orphans.

---

## 5. Reused Cues / Differentiation Issues

One asset backing multiple cue IDs is acceptable for UI affordances but problematic when
distinct gameplay events share an indistinguishable sound:

| Shared asset | Cue IDs | Issue |
|---|---|---|
| `ui_click.wav` | `ui_click`, `ui_cancel`, `ui_tab_change`, `ui_modal_close` | Acceptable — same tactile family |
| `ui_confirm.wav` | `ui_confirm`, `ui_modal_open`, `save_success` | `save_success` at -10 dB is distinct enough; acceptable |
| `ui_warning.wav` | `ui_warning`, `ui_invalid_action` | Acceptable |
| `sfx_radiation_alarm.mp3` | `rad_alert_acute` (-2 dB), `rad_alert_chronic` (-6 dB) | **Borderline** — same alarm, only volume differs. Player cannot tell acute from chronic by sound alone. |
| `sfx_contamination_warning.mp3` | `rad_contamination`, `weather_black_rain` | **Problem** — a weather event and a radiation event sound identical. Different domains, same cue. |
| `sfx_wind_gust_harsh.mp3` | `weather_blizzard`, `weather_wind_gust` | **Problem** — ambient gust and storm bed are the same file; the storm lacks presence. |
| `sfx_alarm_klaxon.mp3` | `weather_alert`, `danger_alarm_klaxon` | **Problem** — weather alert and generic danger share the klaxon. If both fire, the player hears one klaxon covering two unrelated threats. |
| `main_menu.ogg` | `music_menu`, `game_over` | **Problem** — game over reuses the menu theme. Undermines the finality of the ending. |
| `sfx_pipe_clang.mp3` | `shelter_pipe_clang`, `day_transition` | **Problem** — the day-transition sting is a pipe clang, which reads as shelter ambience, not a time advance. |

**Recommendation:** 7B should produce distinct assets for `weather_black_rain`,
`weather_blizzard`, `danger_alarm_klaxon`, and `game_over`. The acute/chronic radiation
pair can stay shared if pitch-shifted at the call site.

---

## 6. Bus Routing Sanity

`AudioBusNames` defines 12 buses. `AudioSelfTest` only validates 7
(Master, Music, Ambience, SFX, UI, Voice, Alerts). The other 5 — `Generator`,
`Ventilation`, `Radio`, `Medical`, `Surface` — are **defined but unused**: no cue routes
to them. This is dead topology.

Routing concerns:
- **Alerts bus** carries 7 cues: `rad_alert_acute`, `rad_alert_chronic`,
  `rad_contamination`, `weather_alert`, `weather_black_rain`, `shelter_air_filter`,
  `danger_alarm_klaxon`. A fallout-storm scenario can stack WeatherAlert + RadAlertAcute
  + RadContamination + DangerAlarmKlaxon within a 5-second window — 4 alerts on one bus.
- `shelter_ventilation` and `shelter_water_drip` route to **Ambience**, not the unused
  `Ventilation` bus. This is fine (keeps them on a controllable bus) but means the
  dedicated bus is dead.
- `med_heartbeat` / `med_coughing` route to **SFX**, not the unused `Medical` bus.

**Recommendation:** Either wire the 5 dead buses (move medical cues to `Medical`,
ventilation cues to `Ventilation`, etc.) or remove them from `AudioBusNames` to avoid
misleading future authors. Wiring is preferred — it gives players independent volume
sliders for medical and ventilation sounds.

---

## 7. Orphan Files (on disk, no cue references them)

| File | Type | Recommendation |
|---|---|---|
| `assets/audio/radio/vo_ch11_stockpile.wav` | VO | Wire to a radio broadcast in 7B |
| `assets/audio/radio/vo_ch3_ash_road.wav` | VO | Wire to a radio broadcast in 7B |
| `assets/audio/radio/vo_ch7_milband.wav` | VO | Wire to a radio broadcast in 7B |
| `assets/audio/radio/vo_kind_hatch.wav` | VO | Wire to a radio broadcast in 7B |
| `assets/audio/radio/vo_kind_parley.wav` | VO | Wire to a radio broadcast in 7B |
| `assets/audio/sfx/radiation_alert.wav` | Legacy duplicate | Delete or retire — superseded by `sfx_radiation_alarm.mp3` |
| `assets/audio/sfx/weather_alert.wav` | Legacy duplicate | Delete or retire — superseded by `sfx_alarm_klaxon.mp3` |

The 5 VO files are the only produced voice assets in the project and are completely
disconnected. Wiring these is the cheapest VO win in 7B.

---

## 8. Stacking / Cooldown Risk

Reviewed all 49 cooldown values for simultaneous-fire scenarios:

| Scenario | Cues that could fire together | Risk |
|---|---|---|
| Fallout storm + acute radiation | `weather_fallout_storm` (SFX, 10s) + `rad_alert_acute` (Alerts, 5s) + `rad_contamination` (Alerts, 5s) + `weather_alert` (Alerts, 5s) | **High** — 3 alerts on one bus in 5s. Needs bus ducking or priority gating. |
| BlackRain onset | `weather_black_rain` (Alerts, 10s) + `rad_contamination` (Alerts, 5s) | **Medium** — same asset, same bus, overlapping windows. Player hears double contamination warning. |
| Blizzard + wind gust | `weather_blizzard` (SFX, 10s) + `weather_wind_gust` (SFX, 3s) | **Low** — same asset, but gust repeats under the storm bed. Intended layering, not a bug. |
| Weather alert + danger klaxon | `weather_alert` (Alerts, 5s) + `danger_alarm_klaxon` (Alerts, 10s) | **Medium** — identical asset, identical bus. If both fire, the klaxon plays twice over itself. |

**Recommendation:** 7C should implement bus-level ducking: when an Alerts cue fires,
duck Ambience by -6 dB for the cue duration. Also deduplicate the klaxon asset (see
Section 5).

---

## 9. Top 20 Silence Gaps (ranked by player impact)

Ranked by: frequency of occurrence × emotional/informational weight × how often a player
encounters it in a normal session.

| # | Gap | Domain | Why it matters | Fix type | 7B Status |
|---|---|---|---|---|---|
| 1 | **Combat has zero audio** | Combat | Combat is the highest-tension moment in the game; 26 event kinds, 0 cues, 0 wiring. Fire, jam, hit, downed, death, victory, defeat — all silent. | Wire `OnCombatEvent` + new cues | **CLOSED** — 8 cues + 8 SFX + bridge |
| 2 | **UI panels have zero feedback** | UI | Every button press, tab switch, modal open/close, and invalid action is silent. 8 cues registered, 0 called. The game feels broken. | Wire `PlayCue` in panel code | Partial — `ui_confirm` on panel open, `ui_invalid_action` on rejected trade |
| 3 | **No ambience loops play** | Ambience | `amb_bunker` and `amb_surface` are registered as loops but never started. The shelter and surface are dead quiet. | Wire in game loop / scene load | Partial — `amb_bunker` wired via `StartBunkerAmbience` |
| 4 | **No music plays** | Music | `music_menu` and `music_gameplay` are registered but never triggered. No menu music, no gameplay underscore. | Wire in scene transitions | Partial — `music_gameplay` wired (path bug fixed) |
| 5 | **Survivor death is silent** | Death | No death event, no death cue. A survivor dying produces no audio — the most emotionally weighted event in the game. | Core event + cue + wiring | **CLOSED** — `SurvivorFateSystem.OnSurvivorFate` → `med_survivor_death` cue |
| 6 | **19 weather states have no transition sound** | Weather | Only 3 of 22 WeatherKind states produce audio. Ashfall, IceStorm, EMPStorm, BloodRain, and 15 others transition silently. | Wire `OnWeatherChanged` + new cues | **CLOSED** — expanded to 14 of 22 states |
| 7 | **Radio is non-functional** | Radio | 4 radio cues registered, 0 triggered. Tuning, static, signal lock, Morse — all silent. 118 broadcasts, 0 audio. | Wire radio UI + JSON `audio_cue` | **CLOSED** — `radio_tune` + `radio_signal_lock` on intercept, 10 VO cues wired |
| 8 | **5 produced VO files are orphaned** | Radio | The only voice acting in the project is unreferenced. Cheapest VO win available. | Add `audio_cue` to radio JSON | **CLOSED** — 10 VO cues + assets + `audio_cue` in radio JSON |
| 9 | **Expeditions are silent** | Expeditions | 15 events (departure, breakdown, return, camp, encounters), 0 cues, 0 wiring. Expedition is a major gameplay loop. | Wire `ExpeditionSystem` events + new cues | **CLOSED** — 5 events wired to existing cues |
| 10 | **Medical events are silent** | Medical | Disease outbreak, quarantine, infection, outcome — all silent. `med_heartbeat`/`med_coughing` exist but never fire. | Wire `DiseaseSystem` events | **CLOSED** — outbreak, infection, quarantine seal/clear, outcome wired |
| 11 | **Crafting has no feedback** | Crafting | `action_crafting` registered, `OnCraftCompleted` never wired. Crafting is a core loop; no completion sound. | Wire `CraftingSystem.OnCraftCompleted` | **CLOSED** — OnCraftCompleted wired |
| 12 | **Trade has no feedback** | Economy | `action_trade` registered, `MarketSystem` events never wired. No deal-confirm sound. | Wire `MarketSystem` / trade UI | **CLOSED** — `action_trade` on Buy/Sell, `ui_invalid_action` on reject |
| 13 | **Shelter door open/seal silent** | Shelter | `shelter_door_open`/`shelter_door_seal` registered, never triggered. Entering/leaving the bunker is a key moment. | Wire in airlock/door code | Partial — open + seal wired via expedition start/return |
| 14 | **Generator/ventilation never audible** | Shelter | `shelter_generator`, `shelter_ventilation` registered as loops, never started. Power state is invisible to the ear. | Wire in `PowerGridSystem` state | **CLOSED** — `ShelterAudioController` syncs loops to power/starting-level state |
| 15 | **Air filter degradation silent** | Shelter | `shelter_air_filter` registered, never triggered. Filter failure is a survival-critical alert. | Wire in air filter system | **CLOSED** — `ShelterAudioController` + `AutopsyHostSession` on hazard warning |
| 16 | **Geiger counter never clicks** | Radiation | `rad_geiger_burst` and `rad_geiger_loop` registered, never triggered. Radiation exposure has no audible geiger feedback. | Wire in radiation tick / dose | **CLOSED** — bridge `OnDoseChanged` + `SurvivorsHostSession` on zone exposure |
| 17 | **Item pickup silent** | Actions | `action_item_pickup` registered, never triggered. Scavenging is a core loop; no pickup sound. | Wire in inventory/scavenge code | Partial — wired via expedition completion |
| 18 | **Danger cues never fire** | Danger | `danger_explosion`, `danger_glass_break`, `danger_debris` registered, never triggered. Explosions and breaches are silent. | Wire in hazard/event code | Partial — `danger_alarm_klaxon` via expeditions + shelter brownout/trip; `danger_debris` via expeditions |
| 19 | **Game over reuses menu music** | Music | `game_over` → `main_menu.ogg`. The ending has no distinct audio identity. | Produce a new game-over asset | **CLOSED** — distinct `game_over.ogg` produced |
| 20 | **Day transition is a pipe clang** | Game flow | `day_transition` → `sfx_pipe_clang.mp3`. The day advance reads as shelter ambience, not a time transition. | Produce a distinct day-sting asset | **CLOSED** — distinct `sfx_day_bell.mp3` produced |

---

## 10. Verification Status

| Check | Method | Result |
|---|---|---|
| All 70 cue paths resolve on disk | `find assets/audio` cross-reference | **Pass** — 0 silent |
| Catalog generator `--check` | `python3 scripts/ci/generate-audio-catalog.py --check` | **Pass** — 70 cues, in sync |
| `AudioSelfTest` headless run | `godot --headless -- --audio-selftest` | **Pass** — 245 pass, 0 fail, 70 cues resolved |
| `dotnet build Ashfall.csproj` | `dotnet build` | **Pass** — 0 warnings, 0 errors |
| `dotnet test` Core suite | `dotnet test` | **Pass** — 5233 pass, 0 fail |
| `--data-integrity-selftest` | `godot --headless` | **Pass** — 137 catalogs, 0 errors |
| `--bridge-selftest` | `godot --headless` | **Pass** |
| Asset orphan sweep | `./scripts/ci/asset-orphan-sweep.sh` | **Pass** — 0 orphans |

---

## 11. Recommendations for 7B / 7C

### 7B progress (completed in this session)

**AudioEventBridge now subscribes to 9 Core domains** (was 2):
1. Radiation — `OnStatusGained` + `OnDoseChanged` → geiger burst on exposure
2. Weather — `OnWeatherChanged` expanded from 3 to 14 of 22 states
3. Combat — `OnCombatEvent` mapping 17 event kinds to 8 cues + 8 SFX
4. Crafting — `OnCraftCompleted` → `action_crafting`
5. Expeditions — 6 events mapped (departure, encounter, breakdown, return, failure, loot)
6. Disease — outbreak, infection, quarantine seal/clear, outcome
7. SurvivorFate — `OnSurvivorFate` → `med_survivor_death`
8. PowerGrid — via `ShelterAudioController` (generator loop, klaxon on trip/brownout)
9. StartingLevel — via `ShelterAudioController` (ventilation loop, air-filter hazard)

**Assets produced:**
- 8 combat SFX (procedural sox): gunshot, jam, reload, hit, downed, victory, defeat, start
- `game_over.ogg` — distinct somber drone (was reusing menu theme)
- `sfx_day_bell.mp3` — distinct day-transition bell (was reusing pipe clang)
- 10 radio VO assets (Year-of-Ash + Verdict story-critical broadcasts)
- 3 medical SFX: `sfx_survivor_death`, `sfx_med_quarantine_seal`, `sfx_med_quarantine_clear`

**Bugs fixed:**
- `PlayGameplayMusic()` / `PlayMainMenuMusic()` referenced `.wav` but assets are `.ogg`
- `ResolveVoiceOver()` now uses `RadioEventKind.ParleyResolution` for `vo_kind_parley`
- `PlayVoiceOver()` now routes through the cue catalog for validation/trim/cooldown

**Score: 14 of 20 gaps CLOSED, 6 PARTIAL, 0 OPEN.** The closed gaps cover all
highest-impact domains (combat, weather, expeditions, medical, crafting, game-over,
day-transition, death, shelter, geiger, trade, radio, VO). The 6 partial gaps
(#2 UI, #3 ambience, #4 music, #13 shelter door, #17 item pickup, #18 danger)
need additional scattered `PlayCue` calls in host/UI code for full coverage.

### Remaining 7B work (if continued)
1. **Wire UI panel cues** (gap #2) — add `PlayCue` calls in panel button handlers
2. **Wire radio cues** (gap #7) — trigger `radio_tune`/`radio_signal_lock` in radio UI
3. **Wire trade** (gap #12) — `MarketSystem` events → `action_trade`
4. **Wire shelter systems** (gaps #13-15) — door, generator, ventilation, air filter
5. **Wire geiger** (gap #16) — radiation tick → `rad_geiger_burst`/`rad_geiger_loop`
6. **Add death event** (gap #5) — Core change: `NeedsSystem.OnSurvivorDeath`
7. **Wire remaining 4 orphan VO files** (gap #8) — `vo_ch3_ash_road`, `vo_ch7_milband`,
   `vo_ch11_stockpile`, `vo_kind_hatch`

### 7C (dynamic ambience)
1. Implement bus-level ducking for the Alerts stacking scenarios in Section 8.
2. Drive `amb_bunker` / `amb_surface` from `WeatherSystem.Current` and `PowerGridSystem`
   load tier (the state machine for 7C).
3. Wire the 5 dead buses (`Medical`, `Ventilation`, `Radio`, `Surface`, `Generator`) or
   remove them from `AudioBusNames`.
4. Add state-transition stings for weather onset (storm-rising sting before the storm bed).

### Environment limitation
Godot is available at `/home/robertsrff/.local/bin/godot`. The `--audio-selftest` and
`--data-integrity-selftest` can be run after 7B/7C code changes. The initial 7A audit
verified resource resolution statically; 7B changes should be verified by running the
selftest before commit.

---

## 12. Radio / Shelter / Dosimeter Addendum (2026-08-31)

The follow-on 7B batches closed the referenced radio paths and the shelter-loop part of
gaps #14–15, while making Geiger bursts universal rather than limiting them to a manual
exposure control.

- Ten authored Year-of-Ash and Verdict broadcasts now point to catalog-backed `audio_cue`
  values, and tuner/static/signal-lock/Morse feedback is live in the radio host.
- Five historic radio sources are now routed; two effectively silent historic sources are
  retained but superseded by new relay/beacon clips. Five further Verdict transmissions
  received new narrow-band runtime VO. The catalog contains 67 resolved cues.
- `shelter_generator` is a low-level loop on the dedicated **Generator** bus while the
  live grid has generation and fuel; it stops on fuel loss, a zeroed generator, session
  replacement, or shutdown.
- `shelter_ventilation` is a loop on the dedicated **Ventilation** bus whenever the
  Holdfast atmosphere session is active. `shelter_air_filter` fires once when
  `airHazardWarning` first becomes true, including a hazardous loaded state.
- `RadiationSystem.OnDoseChanged` now emits `rad_geiger_burst` only for a positive dose
  delta. The continuous `rad_geiger_loop` remains a 7C item because Core has no explicit
  exposure-end event with which to stop it safely.

The global UI button helper and Holdfast trade terminal were already live, so these
batches avoid duplicating click or confirmation playback. That batch's audio selftest
ended at **232/232** with 68 resolved cues and no silent resource paths; the later
disease-crisis addendum records the current 70-cue result.

### Survivor-death correction (2026-08-31)

The earlier audit statement that Core had no survivor-death event is superseded by the
current architecture: `NeedsSystem.OnDied` is the low-level survival event and
`SurvivorFateSystem.OnSurvivorFate` is the all-cause, idempotent campaign-death event.
The latter is now bound by `AudioEventBridge` to the new `med_survivor_death` cue, which
uses a distinct non-vocal `sfx_survivor_death.wav` asset on the **Medical** bus. This
closes silence gap #5 without adding a competing Core event or duplicate death path.

### Disease-crisis lifecycle closure (2026-08-31)

The existing disease mapping now covers the player-facing infection and isolation
lifecycle, rather than only outbreak declaration. `AudioEventBridge` binds
`DiseaseSystem.OnQuarantineStarted` to the new `med_quarantine_seal` cue and maps
`OnQuarantineEnded`, `OnOutbreakContained`, and recovered `OnOutcomeResolved` to
`med_quarantine_clear`. The pre-existing infection (`med_heartbeat`) and outbreak
(`med_coughing`) paths remain intact.

Fatal `OnOutcomeResolved` events intentionally make no direct disease-audio request:
the campaign's authoritative `SurvivorFateSystem` routes that death once through
`med_survivor_death`. This prevents a fatal disease result from stacking a clearance or
second loss sound over the survivor-fate cascade. The two new original PCM clips are
validated by `--audio-selftest`; the bridge self-test checks event order and verifies
that disposal removes all six disease subscriptions.

### Weather and danger differentiation closure (2026-08-31)

The three remaining cross-domain asset collisions from Section 5 are now resolved:
`weather_black_rain` has a dedicated rain texture rather than the radiation
contamination warning, `weather_blizzard` has a storm bed rather than the short wind
gust, and `danger_alarm_klaxon` has a dedicated mechanical infrastructure warning rather
than the weather klaxon. The original generic contamination, gust, and weather-alert
assets remain registered only for their original cues. The acute/chronic radiation pair
is now differentiated as well: chronic radiation has its own lower, distinct alarm
asset rather than relying on a future pitch-only treatment.

### Optional SFX completion (2026-08-31)

The five optional differentiation assets are complete and registered:

| Need | Live resolution |
|---|---|
| Chronic versus acute radiation | `rad_alert_chronic` now uses `sfx_radiation_chronic_alarm.wav`; acute retains its existing alarm. |
| EMP conditions | `EMPStorm` and `AshLightning` map to `weather_emp_storm`. |
| Glass hazards | `GlassStorm` and `RadHail` map to `weather_glass_storm`. |
| Corrosive precipitation | `AcidSnow` and `BloodRain` map to `weather_corrosive_precipitation`. |
| Surface storm presence (prepared) | `amb_surface_storm` replaces the normal surface loop during storm weather after an explicit host request to start surface ambience. |

Surface ambience deliberately does not use expedition state as a location proxy: an
expedition proves only that a team is travelling, not that the player-facing scene is on
the surface. The controller subscribes to `WeatherSystem` while active and stops both
surface loops on shutdown or when bunker ambience takes over. The current bunker-only
game flow does not request surface mode, so the loop is intentionally registered and
tested but inactive until a real surface presentation owns that transition.

Final optional-batch verification: **74 catalog cues resolve**, and
`--audio-selftest` reports **266 pass, 0 fail**.

---

## 12. 7B Radio / VO Batch Addendum (2026-08-31)

This batch resolves the first ten story-critical broadcast paths: five Year-of-Ash
faction/distress transmissions and five Verdict transmissions now have an `audio_cue`
that resolves through `AudioCueCatalog`, and loader tests pin both catalog bindings.

| Surface | Result |
|---|---|
| Receiver interaction | `Listen()` now emits tuner movement + static and, when a station is received, a signal-lock cue. Bearing observations and outgoing beacons emit Morse. |
| Broadcast VO | Year-of-Ash terminal plays one newly unlocked voiced transmission per refresh; Verdict plays the first newly fired voiced transmission per tick, preventing speech stacking after a large time jump. |
| Menu music | The menu starts its music on UI construction and resumes it after returning from a run. |
| Cue authority | Voice playback now uses catalog IDs, resource validation, trim, and cooldowns rather than bypassing the catalog with a raw resource path. |

The catalog now has **67 cues**. The two historic `vo_kind_*.wav` source files were
measured at effectively silent loudness and are retained but no longer playable;
new `*_relay.wav` and `*_beacon.wav` replacements are the registered runtime paths.
The remaining radio corpus stays intentionally text-only for later VO waves.
