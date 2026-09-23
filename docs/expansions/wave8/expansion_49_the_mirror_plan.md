# ASHFALL — Expansion 49 Design Bible
# THE MIRROR
### Wave 8 · Heliograph Stations, Sight Lines, Codes, Relay Chains, Signal Windows, and the Ethics of Visible Messages

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core` (`HeliographSystem`, `Ashfall.Core.World` map nodes), `Ashfall.Core` (`WeatherSystem` read seam)
**Proposed host owner:** `MirrorNetworkHostSession` (extends `HeliographHostSession` + `HeliographSaveStore`)
**Existing save sections:** `heliograph` (`heliograph_save.json`, `HeliographState`)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no heliograph-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has a heliograph system. `HeliographSystem` defines
`HeliographStationDefinition` (`station_id`, `map_node_id`, `condition` 100f,
`is_operational`), `HeliographStationState` (`station_id`, `map_node_id`,
`condition`), `HeliographCatalog` (`schema_version`, `stations`),
`HeliographMessageState` (`message_id`, `origin_station_id`,
`target_station_id`, `payload_key`, `reveal_location_id`, `distress_signal_id`,
`transmitted_day`, `status`, `block_reason`), and `HeliographState`
(`system_id`, `stations`, `messages`, `delivered_count`). The API is live:
`RegisterStation`, `SetStationCondition`, `LoadCatalog`, `Transmit` (with
validation for invalid messages, re-recorded ids, and a
`ValidateTransmission` block reason), `CaptureState`, and `RestoreState`, with
events `OnMessageDelivered`, `OnMessageBlocked`, and `OnStateChanged`. The
constant `MinimumVisibility01 = 0.35f` is a real visibility gate, and the
system already has ports for map-node discovery (`_isMapNodeKnown`,
`_discoverMapNode`) and distress dispatch (`_dispatchDistress`). The host
session `HeliographHostSession` and the `heliograph` save section exist, and
`Plans94To97Panel` provides a first surface.

What does not exist: content and network practice. `heliograph.json` contains
exactly **two stations and 290 bytes**: a holdfast station and a hidden relay
bunker, both at condition 100. There are no sight-line checks, no codes, no
windows, no relay chains, no acknowledgements, no night lamps, no exposure
rules, no station care, and no message forms.

**The Mirror** gives the shelter a real optical network: stations that can see
each other, a code practice, weather windows that decide whether a message can
travel, relays that hand messages along the ridge, and the hard truth that a
light flashed across a valley is a message anyone on the ridge can read. It
extends the live system and never touches the radio family.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| `HeliographSystem` | Stations, transmissions, catalog | Extends with content and practice |
| Radio family (`Radio/*`) | Frequencies, broadcasts, distress | Never touches; the mirror is a parallel, powerless channel |
| `AlarmSystem` (Wave 3) | Shelter alarm states | Carries a fact outward; never owns an alarm |
| `WastelandMapSystem` | Map nodes and discovery | Reads and reveals through existing ports |
| 46 The Long Change | World observation | Stations are not observation posts; log books differ |
| `WeatherSystem` (Wave 5) | Visibility and weather | Reads `MinimumVisibility01`; never changes weather |
| 45 The Envoy (Wave 7) | Negotiation and treaties | Carries words; never negotiates |
| `ShelterFireHazardSystem` (Exp 47) | Fire and flame safety | Night lamps are fire loads; safety routes through it |
| 36 The Watch (Wave 5) | Patrols and horizons | Ridge watchers are separate; exposure risk is shared |
| `StandingRecord` (Exp 03) | Records | Files station logs and delivery proof |
| 43 The Question | Codes and inquiry | Method studies signals; mirror practice is operational |
| `PowerGridSystem` (Wave 2) | Power | The mirror needs none; that is its point |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

There is a dusty mirror in a relay bunker four kilometers up the ridge, and
nobody has flashed it in six years. The radio works, mostly, when the ionosphere
cooperates and when the batteries are charged and when nobody is listening on
the same channel. The mirror works when the sun is out and the air is clear and
one person can see another person on a hill.

**The Mirror** is the expansion about the oldest network in the world: light,
patience, and a code. It is about siting stations, keeping mirrors bright,
writing brevity, waiting for windows, handing messages along a chain, and
understanding that anything you flash across a valley is public. It is the
expansion about the difference between sending a message and being seen
sending it.

### 1.2 The five loops it adds

```
  Site ──► Sight ──► Code ──► Flash ──► Confirm
    │        │         │        │          │
    ▼        ▼         ▼        ▼          ▼
  Nodes,   Lines,   Books,   Windows,   Acks,
  masts    checks   brevity  lamps      logs
                                        │
                                        ▼
                          Care ──► Rotate ──► Decide
```

### 1.3 What the player manages

1. **Stations.** Where they stand and what they can see.
2. **Sight lines.** Terrain, elevation, and obstruction between nodes.
3. **Codes.** Brevity, countersigns, and the cost of wearing one out.
4. **Windows.** Weather, sun angle, and when a message can travel.
5. **Relays.** Multi-hop chains and hand-off discipline.
6. **Acknowledgements.** Proof of delivery and the discipline of waiting.
7. **Exposure.** Who can see the flash, and what that reveals.
8. **Night lamps.** Low-light signaling, fuel, and fire safety.
9. **Station care.** Mirrors, tripods, shutters, and stores.
10. **Records.** Logs, delivery proof, and the ethics of what is written.

### 1.4 What it is not

- Not radio; frequencies, broadcasts, and distress stay with their owners.
- Not a spy system; signals are visible, and deception is costly and limited.
- Not a magic instant network; hops cost time and weather.
- Not a second map; it reveals map nodes through the existing ports.
- Not a military targeting system; messages are words, not fire.
- Not a mysticism plot about ancient light codes.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/HeliographSystem.cs` | Stations, transmissions, gates | `LIVE` |
| `src/Host/HeliographHostSession.cs` | Transmit and save surface | `LIVE` |
| `src/Host/HeliographSaveStore.cs` | `heliograph` persistence | `LIVE` |
| `src/UI/Plans94To97Panel.cs` | First signal surface | `LIVE` |
| `Assets/Ashfall.Core/WeatherSystem.cs` | Visibility and weather | `LIVE` |
| `Assets/Ashfall.Core/WastelandMapSystem.cs` | Map nodes and discovery | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `heliograph.json` | **290 B** | 2 stations, both condition 100 |
| Signal code, window, relay catalogs | absent | confirmed none |
| Night lamp catalog | absent | confirmed none |
| Station log and message form content | absent | confirmed none |
| `comms_targets.json` | 4,027 B | comms array owner; not touched |

### 2.3 Confirmed gaps

- **GAP-49-1 — Two stations and no network.**
- **GAP-49-2 — No sight-line or siting content.**
- **GAP-49-3 — No codebooks or brevity practice.**
- **GAP-49-4 — No visibility window content beyond the constant.**
- **GAP-49-5 — No relay chain or hand-off content.**
- **GAP-49-6 — No acknowledgement protocol content.**
- **GAP-49-7 — No exposure or security practice.**
- **GAP-49-8 — No night lamp content.**
- **GAP-49-9 — No station care or stores content.**
- **GAP-49-10 — No message forms, logs, or delivery proof content.**

### 2.4 Non-duplication statement

This expansion will **not** add a second radio, alarm, map, patrol, fire,
weather, or records system. It extends `HeliographSystem` with stations,
codes, windows, relays, lamps, and logs; reads the existing visibility gate;
reveals map nodes only through the existing ports; books lamp safety through
the fire owner; and files records with `StandingRecord`. All new state is
additive inside `HeliographState`. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Light obeys geography.** A station is only as good as what it can
see, and the ridge decides.

**Pillar 2 — Brevity is a craft.** The best message is short, unambiguous, and
rehearsed.

**Pillar 3 — Weather owns the schedule.** A window is a gift, not a right.

**Pillar 4 — Every flash is public.** A visible message is a spoken one;
privacy is a decision, not a feature.

**Pillar 5 — A delivery is proven, not assumed.** Acknowledgements are the
backbone of a network, and waiting is part of sending.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Siting | Maps, walks, elevation | Mystical vantage points |
| Codes | Worked examples, brevity | Secret lore |
| Windows | Weather, sun, ash | Convenient always-on |
| Relays | Hand-off discipline, patience | Instant teleportation |
| Exposure | Honest risk, countersigns | Spy-thriller intrigues |
| Night lamps | Oil, shutter, safety | Magic lanterns |
| Failure | Blocked, worn, resent | Betrayal melodrama |
| Records | Logs and proof | Surveillance logs |

### 3.3 Content limits

- No jamming warfare, signal interception drama, or torture-adjacent content.
- No encrypted fantasy; codes are brevity and countersign, not mathematics.
- No real-world military signal protocols copied; all forms are invented.
- No night-lamp fire hazards ignored; fire safety routes through the owner.
- No messages that command violence; the mirror carries words, and quests keep
  consequences with the systems that own them.
- No new save section.

---

## 4. THE MIRROR WORLD

### 4.1 Interior rooms

- **`room_signal_office`** — maps, sight-line cards, and the window board.
- **`room_mirror_shop`** — polishing, mounts, shutters, and spare glass.
- **`room_code_room`** — codebooks, blanks, and the countersign drawer.
- **`room_station_store`** — lamps, oil, tripods, cloth, and paint.
- **`room_log_room`** — station logs and delivery proof.
- **`room_signal_watch`** — the east window where the ridge is watched.
- **`room_ack_bench`** — flags, slates, and the acknowledgement board.
- **`room_signal_school`** — trainees, chalk sight-lines, and practice keys.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_relay_bunker` | The Relay Bunker | 3 | First station and cache |
| `loc_ridge_mast` | The Ridge Mast | 4 | Main line-of-sight station |
| `loc_west_shoulder` | The West Shoulder | 3 | Second hop and shadow |
| `loc_bent_pine` | The Bent Pine | 2 | Wooded obstruction lesson |
| `loc_lake_hill` | The Lake Hill | 3 | Long water crossing |
| `loc_smoke_ridge` | The Smoke Ridge | 4 | Ash and haze windows |
| `loc_night_cairn` | The Night Cairn | 3 | Lamp station |
| `loc_false_spur` | The False Spur | 4 | Exposure lesson |
| `loc_school_roof` | The School Roof | 1 | Training line |
| `loc_stone_terrace` | The Stone Terrace | 2 | Final station and log house |

All locations require valid map node references and scanner registration.

### 4.3 The daily rhythm

Sunrise check, midday long window, evening short window, night lamp only by
schedule and only for agreed traffic. The expansion's clock is the sun.

---

## 5. MAIN STORYLINE — "WHAT THE LIGHT SAYS"

### 5.1 Central conflict

**Merrin Ahl** has kept the holdfast station working for years without ever
having anyone to talk to. When the relay bunker's mirror is found intact,
**Teague Crale** argues that the network should be rebuilt to the outposts and
the neighboring settlements because light cannot be jammed, cannot be
overheard by accident, and costs nothing but patience. **Isky Sull** writes a
codebook and discovers that brevity is harder than secrecy. **Cleo Vetch**
walks the ridge with a compass and levels and finds that the beautiful straight
line on the map crosses a shoulder too low to see over. **Hoyt Ond** wants a
relay walker at every hop, because a chain is only as strong as its tiredest
person.

Then the first real message goes out on a clear afternoon and a watcher's
flash answers from the false spur — not hostile, just a neighbor who saw the
light and wants to know what it said. The shelter must decide what a visible
network means: an open road anyone can read, a quiet channel with countersigns
and silence discipline, or a tool used only when it must be. The ash storm
that closes the radio for three days decides part of it for them, because a
message still has to get through, and the mirror is the only thing that
works.

The expansion's question: **what do you owe to everyone who can see you
speak?**

### 5.2 Theme (unspoken)

**A visible message is a message to everyone who can see it.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_keeper_merrin_ahl` | Merrin Ahl | Keeper | Station standards |
| `npc_smith_teague_crale` | Teague Crale | Mirror smith | Glass, mounts, shutters |
| `npc_coder_isky_sull` | Isky Sull | Coder | Brevity and countersigns |
| `npc_walker_hoyt_ond` | Hoyt Ond | Relay walker | Hops and hand-offs |
| `npc_surveyor_cleo_vetch` | Cleo Vetch | Surveyor | Sight lines and siting |
| `npc_night_ossian_byre` | Ossian Byre | Night signal | Lamps and schedules |
| `npc_apprentice_gret_ulla` | Gret Ulla | Apprentice | Keys and practice |
| `npc_stores_yor_quen` | Yor Quen | Stores | Lamps, cloth, spares |

### 5.4 Story beats (15)

1. **The Dusty Mirror.** The relay bunker's station is found and cleaned.
2. **First Light.** A training message crosses the yard.
3. **The Walk.** Cleo surveys the ridge and rejects the pretty line.
4. **The Chain.** Five siting choices make a network.
5. **The Book.** Isky writes brevity and runs out of short words.
6. **The Watch.** Someone answers from the false spur.
7. **The Windows.** Weather becomes the schedule.
8. **The Lamp.** Night traffic begins with fire rules.
9. **The Storm.** Ash closes everything but light.
10. **The Long Hop.** A message crosses four stations in one afternoon.
11. **The Ack.** Delivery is proven, and the waiting is taught.
12. **The Countersign.** Codes wear out and the drill changes them.
13. **The Care.** Mirrors are polished, tripods tightened, glass stored.
14. **The Key.** Gret takes the midday window alone.
15. **What the Light Says.** The network's purpose is chosen.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Network | reach far / short chain / one station | ambition |
| Codes | open words / countersigns / minimal | privacy vs. honesty |
| Windows | all weather / fair only / urgent only | exposure |
| Night lamps | scheduled / emergency / none | risk vs. speed |
| Exposure | announce / semi-quiet / silent | relationship |
| Records | full logs / delivery only / none | proof vs. privacy |
| Care | rigorous / seasonal / when broken | reliability |
| Final | open road / quiet channel / reserved tool | identity |

### 5.6 Endings (5 + fade)

1. **The Speaking Ridge** — a five-station network links the valley, and a
   message can cross it in an afternoon.
2. **The Quiet Channel** — countersigns, windows, and silence discipline make
   the network discreet without pretending it is secret.
3. **The Open Book** — the network adopts openness: codes published to
   neighbors, logs shared, and no message anyone would be ashamed to have
   read.
4. **The Chain Unbroken** — the network outlasts its founders because every
   hop has a named successor and a maintained cache.
5. **The Kept Eye** — when radio weather returns, the mirror is kept in
   working order as the network of last resort, dusted and drilled.
6. **Fade** — a mirror flashing twice at noon, an answering flash from a far
   ridge, and a log entry: received, understood, replied.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_mirror_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_mirror_dusty`, `quest_mirror_first_light`, `quest_mirror_walk`,
`quest_mirror_chain`, `quest_mirror_book`, `quest_mirror_watch`,
`quest_mirror_windows`, `quest_mirror_lamp`, `quest_mirror_storm`,
`quest_mirror_long_hop`, `quest_mirror_ack`, `quest_mirror_countersign`,
`quest_mirror_care`, `quest_mirror_key`, `quest_mirror_what_light_says`.

### 6.2 Side quests (30)

**Stations (5)**
- `quest_mirror_registry` — stations registered
- `quest_mirror_mast` — mast raised
- `quest_mirror_tripod` — tripod set
- `quest_mirror_paint` — station painted
- `quest_mirror_cache` — cache stocked

**Sight lines (5)**
- `quest_mirror_level` — levels taken
- `quest_mirror_shoulder` — shoulder bypassed
- `quest_mirror_tree` — tree trimmed or move
- `quest_mirror_water` — long water crossing
- `quest_mirror_shadow` — evening shadow check

**Codes (5)**
- `quest_mirror_brevity` — short forms written
- `quest_mirror_countersign_drill` — countersigns changed
- `quest_mirror_blanks` — unknown form handled
- `quest_mirror_wear` — worn code retired
- `quest_mirror_practice` — trainee key drill

**Windows (5)**
- `quest_mirror_sunrise` — first window kept
- `quest_mirror_midday` — long window used
- `quest_mirror_haze` — haze test
- `quest_mirror_ash` — ash storm attempt
- `quest_mirror_schedule` — board maintained

**Relays (5)**
- `quest_mirror_hop` — one hop handed
- `quest_mirror_walker` — walker rota
- `quest_mirror_wait` — waiting discipline
- `quest_mirror_proof` — proof filed
- `quest_mirror_chain_drill` — four-hop drill

**Care and lamp (5)**
- `quest_mirror_polish` — mirrors polished
- `quest_mirror_glass` — spare glass stored
- `quest_mirror_shutter` — shutters freed
- `quest_mirror_lamp_fill` — lamp filled safely
- `quest_mirror_lamp_rule` — fire rules kept

### 6.3 Repeatable quests (8)

`quest_mirror_repeat_window`, `quest_mirror_repeat_polish`,
`quest_mirror_repeat_log`, `quest_mirror_repeat_practice`,
`quest_mirror_repeat_cache`, `quest_mirror_repeat_walk`,
`quest_mirror_repeat_ack`, `quest_mirror_repeat_countersign`.

### 6.4 Dynamic hooks

Live events (`OnMessageDelivered`, `OnMessageBlocked`, visibility changes,
weather transitions, dusk, map discovery, false-spur contacts) attach authored
follow-ups through existing seams. No new event bus.

### 6.5 Constraints

- Radio, alarms, maps, patrols, weather, and fire stay with their owners.
- Map reveals go through `_discoverMapNode` only.
- Distress dispatch goes through the existing port only.
- Lamp flame safety routes through the fire owner.
- All records file through `StandingRecord`.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `StationNetworkSystem` (extend `HeliographSystem`)

**Owns:** station registry, condition, availability, and network topology.
**Consumes:** map nodes, sight-line results, stores. **Data:**
`heliograph.json` (additive stations). **Rules:** a station is operational only
when its condition is above threshold and its sight line exists; condition
decays with weather and use; stations without a cache cannot winter.

### 7.2 `SightLineSystem` (new, thin, `Ashfall.Core.Signal`)

**Owns:** line-of-sight checks between map nodes: distance, elevation, terrain
class, obstruction, and dusk shadow. **Consumes:** map node data, terrain
records. **Data:** `signal_siting.json`. **Rules:** a line exists or it does
not; the system never invents a vantage point; a broken line reports why.

### 7.3 `SignalCodeSystem` (new, `Ashfall.Core.Signal`)

**Owns:** codebooks, short forms, countersigns, unknown-form handling, and
code retirement. **Consumes:** message forms, practice results. **Data:**
`signal_codes.json`. **Rules:** every form is short, unambiguous, and
rehearsed; countersigns rotate on schedule; a worn code is retired with a
drill; no code exists purely to hide a shameful message.

### 7.4 `WindowSystem` (new, thin, `Ashfall.Core.Signal`)

**Owns:** windows: sunrise, midday, evening, haze, ash, storm, and night.
**Consumes:** `WeatherSystem` visibility, sun angle from the calendar.
**Data:** `signal_windows.json`. **Rules:** transmissions blocked by visibility
use the live `MinimumVisibility01` gate; windows are published a day ahead;
night traffic requires lamps and a schedule.

### 7.5 `RelaySystem` (new, `Ashfall.Core.Signal`)

**Owns:** relay chains, hop order, walkers, hand-off state, and delivery proof.
**Consumes:** stations, windows, logs. **Data:** `relay_chains.json`,
`ack_protocols.json`. **Rules:** a message crosses one hop per window at most
unless the chain is continuous; each hop records a hand-off; a message without
an ack is pending, never assumed delivered.

### 7.6 `SignalSecuritySystem` (new, thin, `Ashfall.Core.Signal`)

**Owns:** exposure assessment, countersign state, false-traffic handling,
silence discipline, and neighbor visibility notes. **Consumes:** sight lines,
watch reports (read-only). **Data:** `signal_security.json`, `false_traffic.json`.
**Rules:** exposure is calculated honestly from who can see the line; silent
periods are lawful and never punished; false traffic is answered with a
countersign request, not retaliation.

### 7.7 `NightLampSystem` (new, thin, `Ashfall.Core.Signal`)

**Owns:** night signaling lamps, shutters, fuel, schedules, and safety rules.
**Consumes:** inventory fuel, fire owner for placement rules. **Data:**
`night_lamp_catalog.json`. **Rules:** lamps are fire loads and are placed by
the fire rules; fuel is drawn from real stores; night traffic is scheduled so
the shelter sleeps.

### 7.8 `SignalLogSystem` (new, thin, `Ashfall.Core.Signal`)

**Owns:** station logs, message forms, delivery proof, and archives. **Data:**
`signal_logs.json`, `message_forms.json`. Records through `StandingRecord`.
**Rules:** every transmission and block is logged with a reason; logs are
written as facts; private matters are referenced, not transcribed.

### 7.9 Systems explicitly not added

- No second radio, alarm, map, or patrol system.
- No jamming, interception, or spy network.
- No encrypted fantasy or real protocol copies.
- No fire, weather, or power authority.
- No instant messaging across the valley.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `heliograph.json` (extend, additive stations)

```json
{
  "schema_version": 1,
  "stations": [
    {
      "station_id": "heliograph_ridge_mast",
      "map_node_id": "loc_ridge_mast",
      "condition": 100.0
    }
  ]
}
```

### 8.2 `signal_codes.json` (new)

```json
{
  "schema_version": 1,
  "codes": [
    {
      "code_id": "code_need_water",
      "display_name": "Water needed",
      "form": "two long, pause, one short",
      "countersign": "code_ack_received",
      "tags": ["essential", "short"]
    }
  ]
}
```

### 8.3 `signal_windows.json` (new)

Windows: kind, hours, visibility need, traffic type, lamp required.

### 8.4 `relay_chains.json` (new)

Chains: chain id, station order, walkers, hop time, caches.

### 8.5 `ack_protocols.json` (new)

Acks: form, expectation, wait period, escalation, no-ack consequence.

### 8.6 `signal_security.json` (new)

Security: line, viewers, exposure level, countersign need, silence rule.

### 8.7 `night_lamp_catalog.json` (new)

Lamps: lamp id, fuel, burn hours, shutter, placement, fire note.

### 8.8 `signal_logs.json` (new)

Logs: station, day, message, status, hand-off, ack, note.

### 8.9 `signal_siting.json` (new)

Siting: node pair, distance, elevation, obstruction, pass or fail, reason.

### 8.10 `message_forms.json` (new)

Forms: form id, purpose, length limit, allowed fields, review.

### 8.11 Items

New items appended to `items.json`: `item_signal_mirror`,
`item_shutter_board`, `item_signal_tripod`, `item_signal_lamp`,
`item_lamp_oil`, `item_codebook`, `item_countersign_slate`,
`item_sighting_compass`, `item_signal_flags`, `item_ack_flags`,
`item_station_paint`, `item_spare_glass`, `item_logbook`,
`item_mirror_cloth`, `item_sun_shade`, `item_signal_stone`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`HeliographState` remains the live save owner. New sub-objects (stations,
codes in use, windows used, relay chains, acks, exposure notes, lamps, logs)
are additive inside it. No new save section.

### 9.2 State to persist

- Station registry, condition, and cache state.
- Codebook version, countersign rotation day, and retired codes.
- Window history and blocked attempts with reasons.
- Relay chain assignments and walker rosters.
- Acknowledgement state per message.
- Exposure notes and silence periods.
- Lamp stock, fuel, and schedules.
- Station logs and delivery proof.

### 9.3 Determinism

- Visibility uses the live weather path and `MinimumVisibility01`.
- Windows derive from calendar and weather, not wall-clock.
- Hop timing derives from chain length and walker state.
- Code rotation derives from day counts.
- Ack timeouts derive from authored windows.
- Paired replay hashes must match; no `System.Random`.

### 9.4 Migration

Legacy saves load with both existing stations intact; no codes, chains, acks,
or logs exist until started. A station with condition below threshold loads as
needing care, not as lost.

### 9.5 Checksum

Invariant-culture floats; integer day, hop, and condition fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `Plans94To97Panel` (extend) | Transmit and status | `MirrorNetworkHostSession` |
| `SignalNetworkPanel` (new) | Stations and sight lines | same |
| `CodePanel` (new) | Codebook and countersigns | same |
| `WindowPanel` (new) | Windows and forecasts | same |
| `RelayPanel` (new) | Chains and acks | same |
| `LampPanel` (new) | Night scheduling and safety | same |
| `SignalLogPanel` (new) | Logs and delivery proof | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Blocked transmissions always state a reason: visibility, line, condition,
  ack pending.
- Windows are shown as a tomorrow-ahead schedule, never a hidden timer.
- Exposure is shown as a list of who can plausibly see the line.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Duties are texted and menu-reachable; no reflex input.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a shutter clack, a mirror easing in
its mount, a lamp being lit, wind on a ridge, a chalk on a slate. No cue is
required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `HeliographSystem` | Stations, messages, gates |
| `WeatherSystem` (Wave 5) | Visibility and windows |
| `WastelandMapSystem` | Node read and reveal |
| `Radio` family | None; parallel channel |
| `AlarmSystem` (Wave 3) | Outbound facts only |
| `ShelterFireHazardSystem` (Exp 47) | Lamp placement and fire load |
| `Inventory` | Mirrors, fuels, spares |
| `ShelterWorkshopSystem` (Wave 6) | Mounts, shutters, tripods |
| `RouteInfrastructureSystem` (Wave 5) | Ridge walk routes |
| 36 The Watch (Wave 5) | Shared horizon awareness |
| 45 The Envoy (Wave 7) | Carries words, never negotiates |
| `StandingRecord` (Exp 03) | Logs and proof |
| `DutyRoster` (Exp 02) | Walker and keeper shifts |
| `FieldGuide` (Plan 20A/28) | Reading the sky windows |
| `EpilogueChronicleBuilder` | Network history lines |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm heliograph system, host, store, panel,
weather, map, fire, and record owners. Record file:line; change nothing.

**Phase 1 — Data + validators.** Extend `heliograph.json`; author the nine new
catalogs; register validators and scanner.

**Phase 2 — Pure Core.** `StationNetworkSystem` extensions, `SightLineSystem`,
`SignalCodeSystem`, `WindowSystem`, `RelaySystem`, `SignalSecuritySystem`,
`NightLampSystem`, `SignalLogSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `MirrorNetworkHostSession`, focused selftest
coverage, fresh journey from the dusty mirror to the four-hop drill.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Year-long soak: windows, blocked traffic, ack proof,
storm-only survival, lamp safety.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Stations | 10 |
| Sight-line pairs | 20 |
| Codes | 40 |
| Windows | 12 |
| Relay chains | 6 |
| Ack protocols | 8 |
| Security notes | 14 |
| Lamps | 8 |
| Logs | 24 |
| Message forms | 16 |
| Items | 16 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 55,000–70,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Radio duplication | Critical | Parallel channel contract |
| Spy-thriller drift | High | Visible-message ethics |
| Real protocol copying | High | Invented forms only |
| Instant network | Medium | Hop time and windows |
| Lamp fire risk | Medium | Fire owner rules |
| Map authority break | Medium | Existing ports only |
| Ack busywork | Medium | Proof and logs |
| Determinism break | Low | Weather path only |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `heliograph.json` | 10 | 2,000 |
| `signal_codes.json` | 40 | 6,000 |
| `signal_windows.json` | 12 | 2,500 |
| `relay_chains.json` | 6 | 2,000 |
| `ack_protocols.json` | 8 | 2,000 |
| `signal_security.json` | 14 | 3,000 |
| `night_lamp_catalog.json` | 8 | 2,000 |
| `signal_logs.json` | 24 | 4,000 |
| `signal_siting.json` | 20 | 4,000 |
| `message_forms.json` | 16 | 3,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~61,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R49-1 | Radio overlap | Low | Critical | Contract |
| R49-2 | Spy drift | Med | High | Ethics |
| R49-3 | Protocol copying | Med | High | Invented |
| R49-4 | Instant nets | Med | Medium | Hops |
| R49-5 | Lamp fire | Med | Medium | Fire owner |
| R49-6 | Map bypass | Low | Medium | Ports |
| R49-7 | Ack tedium | Med | Medium | Proof value |
| R49-8 | Determinism | Low | High | Weather |
| R49-9 | Content overrun | Med | Medium | Budget §13 |
| R49-10 | Tone mysticism | Med | Medium | Pillars §3 |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **How far can a chain reach?** Recommended: five hops, valley-scale, with a
   full chain crossing in one to two days.
2. **Are codes secret or public?** Recommended: brevity forms are public;
   countersigns are rotated and kept by stations, and the plan never treats
   secrecy as a win condition.
3. **Does night traffic exist?** Recommended: yes, scheduled and lamp-lit, with
   fire safety and sleep protection.
4. **Who owns a station's log?** Recommended: the station keeper, filed with
   the record owner, with private matters referenced rather than transcribed.
5. **What happens to the network when radio is restored?** Recommended: it is
   maintained as the network of last resort and drilled each season.

---

## 17. APPENDIX D — STATION TABLE (10 STATIONS)

| # | Station | Node | Elevation | Hop | Cache | Notes |
|---|---|---|---|---|---|---|
| 1 | Holdfast | loc_holdfast | low | — | yes | existing |
| 2 | Relay bunker | loc_hidden_relay_bunker | mid | 1 | yes | existing |
| 3 | Ridge mast | loc_ridge_mast | high | 2 | no | main line |
| 4 | West shoulder | loc_west_shoulder | mid | 3 | no | shadow edge |
| 5 | Bent pine | loc_bent_pine | low | — | no | obstruction |
| 6 | Lake hill | loc_lake_hill | mid | 4 | no | water cross |
| 7 | Smoke ridge | loc_smoke_ridge | high | 5 | no | haze lesson |
| 8 | Night cairn | loc_night_cairn | mid | — | yes | lamp |
| 9 | School roof | loc_school_roof | low | — | no | training |
| 10 | Stone terrace | loc_stone_terrace | high | 5 | yes | log house |

Ten stations, two of them inherited from the live seed file and eight authored
around them. The cache column is the small discipline that keeps a network
alive: a station that cannot feed its keeper in bad weather stops being a
station, and the plan marks which ones can winter.

---

## 18. APPENDIX E — SIGHT-LINE TABLE (20 PAIRS)

| # | From | To | Distance | Obstruction | Result | Reason |
|---|---|---|---|---|---|---|
| 1 | Holdfast | Relay | 4 km | none | pass | clear ridge |
| 2 | Relay | Ridge mast | 3 km | none | pass | line raised |
| 3 | Ridge mast | West shoulder | 5 km | none | pass | long clear |
| 4 | West shoulder | Lake hill | 6 km | low spur | fail | 40 m short |
| 5 | Lake hill | Stone terrace | 7 km | none | pass | water line |
| 6 | Smoke ridge | Stone terrace | 8 km | haze | pass | fair only |
| 7 | Holdfast | Ridge mast | 6 km | shoulder | fail | shoulder |
| 8 | Relay | Bent pine | 2 km | trees | fail | forest |
| 9 | Ridge mast | Smoke ridge | 9 km | none | pass | long clean |
| 10 | Night cairn | Relay | 3 km | none | pass | lamp line |
| 11 | School roof | School gate | 200 m | none | pass | training |
| 12 | School roof | Holdfast roof | 400 m | chimney | fail | chimney |
| 13 | West shoulder | Stone terrace | 10 km | none | pass | no cache |
| 14 | Lake hill | Smoke ridge | 6 km | haze | pass | midday |
| 15 | Bent pine | Lake hill | 4 km | trees | fail | forest |
| 16 | Night cairn | Holdfast | 4 km | ridge | fail | ridge line |
| 17 | Relay | West shoulder | 6 km | none | pass | alt route |
| 18 | Ridge mast | Night cairn | 5 km | none | pass | lamp |
| 19 | Stone terrace | Far spur | 12 km | unknown | fail | unvisited |
| 20 | Holdfast | School roof | 300 m | wall | fail | wall |

Twenty pairs, eleven passing and nine failing, and the failures are the real
content: a network is built as much by what cannot be seen as by what can. The
fortieth-meter shoulder that sinks a beautiful straight line and the chimney
that blocks a school roof are the two lessons every new surveyor gets.

---

## 19. APPENDIX F — CODE TABLE (SAMPLE 20 OF 40)

| # | Code | Form | Use | Countersign |
|---|---|---|---|---|
| 1 | All well | one long, one short | daily | ack all |
| 2 | Water needed | two long | request | ack water |
| 3 | Medical needed | three short | urgency 2 | ack medical |
| 4 | Come to station | alternating | summons | ack coming |
| 5 | Message follows | two short, pause, two short | traffic | ack ready |
| 6 | Message ends | one long, pause, one long | close | ack end |
| 7 | Understood | one long, one short, pause | ack | none |
| 8 | Not understood | three short, pause, short | retry | resend |
| 9 | Repeat last | two short, pause, long | retry | repeat |
| 10 | Who watches | long, pause, alternating | security | countersign |
| 11 | Countersign please | three alternating | security | code |
| 12 | Wrong countersign | two long, pause, short | refuse | none |
| 13 | Silence until noon | two long, pause, long | quiet | ack silence |
| 14 | Route open | short, short, long | travel | ack route |
| 15 | Route closed | long, short, long | travel | ack closed |
| 16 | Weather closing | short, pause, long, long | warning | ack weather |
| 17 | Trading party | three long, pause, short | visitor | ack party |
| 18 | Strangers seen | long, pause, short, short | warning | ack seen |
| 19 | Lamp only | short, long, pause, short | night | ack lamp |
| 20 | End of year | four long, pause, long | ceremony | ack year |

Twenty of forty forms, deliberately simple enough to flash from memory in bad
weather. The countersign column is the whole security model: not secrecy but
verification, so that a stranger's flash gets a question rather than a secret,
and the shelter never mistakes an imitation for a friend.

---

## 20. APPENDIX G — WINDOW TABLE

| # | Window | Hours | Visibility | Traffic | Lamp |
|---|---|---|---|---|---|
| 1 | Sunrise | 06-08 | 0.55 | short | no |
| 2 | Midday long | 11-14 | 0.70 | full | no |
| 3 | Afternoon | 14-16 | 0.60 | short | no |
| 4 | Evening | 17-19 | 0.45 | short | no |
| 5 | Haze | any | 0.40 | essential | no |
| 6 | Ash light | any | 0.35 | essential | no |
| 7 | Storm | none | below gate | none | no |
| 8 | Night one | 21-22 | lamp | scheduled | yes |
| 9 | Night two | 23-24 | lamp | emergency | yes |
| 10 | Night quiet | 00-05 | none | none | no |
| 11 | Winter short | 10-13 | 0.45 | short | no |
| 12 | Full clear | any | 0.85 | full | no |

Twelve windows, and the ash-light row is the expansion's quiet hero: at 0.35
visibility — exactly the live system's minimum — a short essential message can
still cross while the radio is dead and the valley is gray. The night-quiet row
is the shelter's promise that the network does not cost sleep.

---

## 21. APPENDIX H — RELAY CHAIN TABLE

| # | Chain | Order | Hops | Walkers | Winter |
|---|---|---|---|---|---|
| 1 | Valley line | holdfast → relay → ridge | 2 | 2 | yes |
| 2 | West line | ridge → shoulder → lake | 3 | 3 | no |
| 3 | Terrace line | lake → smoke → terrace | 4 | 4 | yes |
| 4 | Night line | relay → cairn | 1 | 1 | yes |
| 5 | Training line | school → holdfast yard | 1 | 1 | yes |
| 6 | Long line | holdfast → terrace | 5 | 5 | no |

Six chains from a single training hop to the five-station long line. The winter
column is the practical truth: the long line sleeps under snow, and the shelter
plans its year around which chains wake in spring.

---

## 22. APPENDIX I — ACK PROTOCOL TABLE

| # | Ack | Expected | Wait | If absent |
|---|---|---|---|---|
| 1 | Understood | immediate | 1 window | resend |
| 2 | Written down | same day | 2 windows | resend once |
| 3 | Passed on | hand-off | 1 hop | walker visit |
| 4 | Refused | countersign | 1 window | retire code |
| 5 | Requesting detail | follow-up | 2 windows | answer |
| 6 | Delivery proof | final | 1 day | log pending |
| 7 | Not received | negative | 1 window | repeat |
| 8 | Emergency ack | urgent | immediate | second station |

Eight acknowledgement forms with wait periods, and the negative acknowledgement
is the most important row: a network where "not received" is a normal, expected
message is a network that can be trusted with anything, because silence is
never mistaken for success.

---

## 23. APPENDIX J — EXPOSURE TABLE

| # | Line | Who can see | Exposure | Countersign | Rule |
|---|---|---|---|---|---|
| 1 | Holdfast-relay | shelter, ridge farms | low | yes | normal |
| 2 | Relay-ridge | anyone on ridge | med | yes | normal |
| 3 | Ridge-shoulder | valley floor | med | yes | normal |
| 4 | Shoulder-lake | fishers | med | yes | fair only |
| 5 | Lake-smoke | lake camps | high | yes | essential |
| 6 | Smoke-terrace | far valley | high | yes | essential |
| 7 | Night cairn | river road | high | yes | lamp only |
| 8 | False spur | unknown watchers | high | yes | silence |
| 9 | Training line | shelter only | none | no | open |
| 10 | Long line | entire valley | high | yes | essential |

Ten exposure notes, and the false-spur row carries the expansion's central
lesson: the most dangerous exposure is the one you did not know was there, and
the answer is a countersign, not a fight. The training line is exposed to
nobody, which is why trainees learn there first.

---

## 24. APPENDIX K — NIGHT LAMP TABLE

| # | Lamp | Fuel | Burn | Shutter | Placement | Fire note |
|---|---|---|---|---|---|---|
| 1 | Stone lamp | oil | 2 h | yes | cairn | stone base |
| 2 | Hooded lamp | oil | 3 h | yes | station wall | away from wood |
| 3 | Signal lantern | oil | 4 h | yes | mast | wind shield |
| 4 | Hand lamp | oil | 1 h | no | walking | safe type |
| 5 | School lamp | oil | 2 h | yes | yard | training |
| 6 | Terrace lamp | oil | 3 h | yes | log house | hearth rules |
| 7 | Emergency lamp | oil | 1 h | yes | bunker | cache |
| 8 | Spare lantern | oil | 2 h | yes | store | shelf |

Eight lamps and one rule repeated in every row: a lamp is a small fire, and it
lives by the fire service's placement rules. The emergency lamp lives in the
cache with the oil, because the night that needs it is the night nobody can
walk to the store.

---

## 25. APPENDIX L — MESSAGE FORM TABLE

| # | Form | Purpose | Length | Fields | Review |
|---|---|---|---|---|---|
| 1 | Situation | status report | 6 words | subject, state | none |
| 2 | Request | ask for help | 8 words | need, place | log |
| 3 | Offer | give help | 8 words | thing, place | log |
| 4 | Visitor | party notice | 6 words | party, day | countersign |
| 5 | Warning | danger | 6 words | danger, area | log |
| 6 | Weather | forecast | 6 words | window, day | log |
| 7 | Trade | exchange | 10 words | offer, want | log |
| 8 | Letter | personal relay | sealed | letter ref | privacy |
| 9 | Memorial | death notice | 8 words | name, day | respect |
| 10 | Birth | arrival notice | 8 words | name, day | joy |
| 11 | Return | coming home | 6 words | party, day | none |
| 12 | Confirmation | ack | 4 words | message ref | none |
| 13 | Silence | quiet period | 4 words | until day | log |
| 14 | Counter | security | 4 words | countersign | log |
| 15 | Refusal | cannot send | 4 words | reason class | log |
| 16 | Year | closing note | 12 words | year, hope | reading |

Sixteen forms, and the deliberate presence of birth, death, and letter forms
signals what the network is really for: not only danger and trade, but the
ordinary news that keeps separated communities one place. The letter form is
sealed and referenced rather than transcribed, because not every message needs
to be public merely because the light is.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_mirror_dusty` | 4 | Station cleaned |
| `quest_mirror_first_light` | 3 | Training flash sent |
| `quest_mirror_walk` | 4 | Ridge surveyed |
| `quest_mirror_chain` | 5 | Network sited |
| `quest_mirror_book` | 4 | Codes written |
| `quest_mirror_watch` | 4 | Watcher answered |
| `quest_mirror_windows` | 3 | Schedule published |
| `quest_mirror_lamp` | 4 | Night traffic safe |
| `quest_mirror_storm` | 4 | Ash message sent |
| `quest_mirror_long_hop` | 5 | Four hops crossed |
| `quest_mirror_ack` | 3 | Proof received |
| `quest_mirror_countersign` | 4 | Codes rotated |
| `quest_mirror_care` | 4 | Stations serviced |
| `quest_mirror_key` | 4 | Apprentice operates |
| `quest_mirror_what_light_says` | 3 | Purpose chosen |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_mirror_registry` | 3 | Stations registered |
| `quest_mirror_mast` | 4 | Mast raised |
| `quest_mirror_tripod` | 3 | Tripod set |
| `quest_mirror_paint` | 3 | Station painted |
| `quest_mirror_cache` | 3 | Cache stocked |
| `quest_mirror_level` | 3 | Levels taken |
| `quest_mirror_shoulder` | 4 | Shoulder bypassed |
| `quest_mirror_tree` | 3 | Tree cleared |
| `quest_mirror_water` | 3 | Water crossing proven |
| `quest_mirror_shadow` | 3 | Shadow checked |
| `quest_mirror_brevity` | 3 | Short forms written |
| `quest_mirror_countersign_drill` | 4 | Countersigns changed |
| `quest_mirror_blanks` | 3 | Unknown form handled |
| `quest_mirror_wear` | 3 | Worn code retired |
| `quest_mirror_practice` | 3 | Trainee drilled |
| `quest_mirror_sunrise` | 3 | First window kept |
| `quest_mirror_midday` | 3 | Long window used |
| `quest_mirror_haze` | 3 | Haze test passed |
| `quest_mirror_ash` | 4 | Ash message sent |
| `quest_mirror_schedule` | 3 | Board maintained |
| `quest_mirror_hop` | 3 | Hop handed off |
| `quest_mirror_walker` | 3 | Walker rota set |
| `quest_mirror_wait` | 3 | Waiting taught |
| `quest_mirror_proof` | 3 | Proof filed |
| `quest_mirror_chain_drill` | 5 | Four-hop drill |
| `quest_mirror_polish` | 3 | Mirrors polished |
| `quest_mirror_glass` | 3 | Spares stored |
| `quest_mirror_shutter` | 3 | Shutters freed |
| `quest_mirror_lamp_fill` | 3 | Lamp filled safely |
| `quest_mirror_lamp_rule` | 3 | Fire rules kept |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Merrin Ahl** — keeper. Has flashed the holdfast station at noon for years
with nobody answering, and cleaned the mirror every week anyway. Believes a
station kept ready is a promise that may never be called in.

**Teague Crale** — mirror smith. Can find the true in a warped mirror with a
finger and a candle, and hates silver polish as a waste of good time. Believes
light is the cheapest thing the shelter owns.

**Isky Sull** — coder. Writes short forms in a chair by the window and reads
them aloud to check that they sound like words. Believes brevity is a kindness
to the tired.

**Hoyt Ond** — relay walker. Walks five hours to hand over a piece of paper
without complaint and refuses to run. Believes a chain is a set of promises
with boots.

**Cleo Vetch** — surveyor. Rejected the prettiest straight line on the map
because of a shoulder, and enjoys being right about geography. Believes the
land is the boss of the drawing.

**Ossian Byre** — night signal. Lights lamps, closes shutters, and counts burn
hours in his head. Believes the night watch deserves messages too.

**Gret Ulla** — apprentice. Sixteen, keeps the keys, and practices on the
school roof line until the forms are muscle. Believes the pause between
longs is a place where mistakes live.

**Yor Quen** — stores. Counts oil, cloth, and glass and hides none of them.
Believes a spare part in a drawer is an optimistic statement about the future.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Relay Bunker** — dust, a mirror, and a cache nobody has touched.
- **The Ridge Mast** — the long line and the wind that tests it.
- **The West Shoulder** — the forty meters that broke the pretty map.
- **The Bent Pine** — a tree that taught the network its first no.
- **The Lake Hill** — seven kilometers of water and sun.
- **The Smoke Ridge** — ash light at exactly the working minimum.
- **The Night Cairn** — a stone base, a hooded lamp, and a schedule.
- **The False Spur** — a flash from a ridge nobody surveyed.
- **The School Roof** — trainees, chalk, and the shortest line there is.
- **The Stone Terrace** — logs, cache, and the far end of the chain. 

---

## 30. APPENDIX Q — MIRROR NETWORK CHARTER

| Clause | Promise |
|---|---|
| Visible | Any message may be read by anyone who sees it |
| Brief | Forms are short, rehearsed, and unambiguous |
| Verified | Strangers are answered with countersigns |
| Scheduled | Windows are published; sleep is protected |
| Proved | Delivery requires an acknowledgement |
| Cached | No station winters without stores |
| Cared | Mirrors, mounts, and glass are kept ready |
| Open | No message is sent that would shame its writer if read |
| Filed | Every transmission and block is logged with a reason |
| Last | When radio fails, the light still speaks |

The charter is the expansion's first-class design object. A visible network
cannot promise secrecy, so it promises honesty instead: messages short,
delivery proven, strangers verified, and nothing flashed that the shelter would
not be willing to have read aloud at the far end.

---

## 32. APPENDIX R — WORKED SIGNAL YEAR

**Spring, week one.** Teague opens the relay bunker and finds the mirror under
six years of dust and exactly where it was left. The first cleaning takes a day
and a half and produces a reflection that startles the whole crew.

**Spring, week two.** The training line on the school roof sends and receives
its first message: all well. Gret flashes it eleven times before Isky accepts
it, and the twelfth is clean.

**Spring, week four.** Cleo walks the ridge with a compass, a level, and a
notebook, and comes back with eleven passing lines and nine failures. The
rejected pretty line is drawn in red on the wall map, and the red line becomes
the most instructive object in the signal office.

**Spring, week seven.** Five stations are sited. The west shoulder gets a mast
and a cache, and Hoyt is made its first walker because he volunteered and
because nobody else had walked the route.

**Spring, week nine.** Isky's codebook reaches forty forms. The shelter has
words for water, weather, visitors, births, and deaths, and the words are short
enough to flash in a haze.

**Summer, week two.** The first long transmission goes out at midday and comes
back acknowledged in the same window. The log entry is two lines and the
shelter reads it aloud at supper.

**Summer, week five.** A flash answers from the false spur. It is a neighbor
with a mirror and no code. Isky answers with a countersign request, gets
nothing, and logs the contact. The network has met its first stranger and
neither shot nor lied.

**Summer, week eight.** The window board goes up: sunrise, midday, afternoon,
evening, and the ash-light minimum. The shelter learns that a message sent at
the wrong hour is not sent.

**Autumn, week one.** Ossian lights the first scheduled night lamp and the
cairn answers across four kilometers of dark. Fire rules were read aloud first,
and the lamp sits on stone.

**Autumn, week four.** The ash storm closes the valley for three days. The
radio is dead the whole time, and a short essential message crosses the long
line on day two in an hour of working visibility. The storm is what decides the
shelter to keep the light forever, not the summer.

**Autumn, week six.** The four-hop drill crosses five stations and returns an
acknowledgement in a single afternoon. The chain is exercised end to end, and
the bottleneck turns out to be a shutter that sticks, not a person.

**Winter, week one.** Countersigns are rotated, as practiced. One station
misses the drill and is answered with the wrong-countersign form, and the
station's keeper walks three kilometers to apologize in person, which is
exactly how a network should learn its security.

**Winter, week four.** The long line sleeps under snow and the short chains
keep working. The terrace cache is checked, counted, and closed for the season,
and the log records the closing like a door.

**Year one, last week.** Gret takes the midday window alone and gets the ack on
the third try. The year's log is filed, and the shelter's message forms cover
birth, death, weather, water, visitors, trade, and one sealed letter that no
one read but two people received.

---

## 33. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Teague turns the mirror into the sun and the light lands on his own eye and he
yelps, and that is how the whole crew learns that the first rule of the mirror
shop is to look at the floor until the beam is aimed.

> Cleo stands on the shoulder with the level and says the line is forty meters
> short, and the map's beautiful straight line gets a red mark through it, and
> she is happier about the red line than about any of the eleven that passed.

> Isky writes: need water. He replaces it with: water. He replaces it with two
> longs, and the whole difference between radio and light is that on the ridge
> there is no time for a preposition.

> The lamp is lit at nine and the far cairn answers at ten past, and Ossian
> puts his hand on the warm stone base and thinks that the night has just
> become smaller, in a good way.

---

## 34. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Bad siting | line never works | survey again |
| Dirty mirror | missed windows | polish rota |
| Wrong hour | no answer | window board |
| Long forms | errors in haze | brevity drill |
| No counter | strangers answered | rotate codes |
| No ack | lost confidence | proof rule |
| Lamp unsafe | fire risk | fire owner rules |
| No cache | station dies | restock |
| Silent needs | broken chain | walker visit |
| Public shame | message read by all | write with open care |

No failure here is dramatic, and that is the design: an optical network fails
by inches — dust, pride, a forgotten shutter — and recovers by habits small
enough to keep in a pocket.

---

## 35. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] No radio, alarm, map, or patrol duplication.
- [ ] Visibility uses the live `MinimumVisibility01` gate.
- [ ] Map reveals use `_discoverMapNode` only.
- [ ] Distress dispatch uses the existing port only.
- [ ] Lamps route placement through the fire owner.
- [ ] No real-world signal protocols are copied.
- [ ] No jamming, spy, or interrogation content.
- [ ] Messages never command violence.
- [ ] Save additions are additive inside `heliograph`.
- [ ] Determinism uses the weather path only.

---

## 36. APPENDIX V — GLOSSARY

- **Heliograph** — a mirror and shutter used to flash daylight messages.
- **Sight line** — whether one node can actually see another.
- **Window** — an hour when visibility allows traffic.
- **Form** — a short coded phrase with fixed meaning.
- **Countersign** — the question and answer that verifies a station.
- **Hop** — one station-to-station leg of a chain.
- **Hand-off** — a walker receiving a message and carrying it onward.
- **Ack** — the acknowledgement that makes delivery real.
- **Exposure** — who can see a line, honestly counted.
- **Cache** — the stores that let a station winter.

---

## 37. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `HeliographSystem` | stations | messages | weather |
| `StationNetworkSystem` | nodes | registry | maps |
| `SightLineSystem` | terrain | siting | terrain |
| `SignalCodeSystem` | forms | code state | messages |
| `WindowSystem` | weather | windows used | weather |
| `RelaySystem` | chains | hand-offs | stations |
| `SignalSecuritySystem` | sight lines | exposure notes | codes of others |
| `NightLampSystem` | inventory | lamp state | fire policy |
| `SignalLogSystem` | events | logs | nothing |
| `WeatherSystem` | nothing | nothing | nothing |
| `WastelandMapSystem` | reveal ports | nothing | nothing |
| `AlarmSystem` | facts | nothing | nothing |
| `ShelterFireHazardSystem` | lamp load | nothing | nothing |
| `Inventory` | items | nothing | nothing |
| `ShelterWorkshopSystem` | repair | nothing | nothing |
| `DutyRoster` | shifts | nothing | nothing |
| `StandingRecord` | records | records | nothing |

---

## 38. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`heliograph.json`** (extend) — existing `stations` rows with the same schema;
new rows must reference real map nodes.

**`signal_codes.json`** — `code_id`, `display_name`, `form`, `countersign`,
`tags[]`.

**`signal_windows.json`** — `window_id`, `kind`, `hours`, `visibility_need`,
`traffic`, `lamp_required`, `tags[]`.

**`relay_chains.json`** — `chain_id`, `station_order[]`, `walkers[]`,
`hop_hours`, `caches[]`, `tags[]`.

**`ack_protocols.json`** — `ack_id`, `form`, `expected`, `wait_windows`,
`if_absent`, `tags[]`.

**`signal_security.json`** — `note_id`, `line`, `viewers[]`, `exposure`,
`countersign`, `rule`, `tags[]`.

**`night_lamp_catalog.json`** — `lamp_id`, `fuel`, `burn_hours`, `shutter`,
`placement`, `fire_note`, `tags[]`.

**`signal_logs.json`** — `log_id`, `station_id`, `day`, `message_id`, `status`,
`hand_off`, `ack`, `note`, `tags[]`.

**`signal_siting.json`** — `pair_id`, `from_node`, `to_node`, `distance_km`,
`obstruction`, `result`, `reason`, `tags[]`.

**`message_forms.json`** — `form_id`, `purpose`, `length_limit`, `fields[]`,
`review`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid node or code references, or out-of-range
numbers.

---

## 39. APPENDIX Y — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Lines surveyed | network quality | Siting |
| Lines passing | coverage | Siting |
| Windows used | traffic | Windows |
| Messages sent | reach | Messages |
| Blocked messages | reliability | Messages |
| Acks received | proof | Acks |
| Hops completed | chain health | Relays |
| Codes in use | brevity | Codes |
| Countersign drills | security | Drills |
| Station condition | care | Stations |

Telemetry is diagnostic only; it never gates content and never ranks a station
or a keeper.

---

## 40. APPENDIX Z — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive inside `heliograph`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows windows, blocked traffic, acks, and a storm crossing.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No radio, spy, or protocol-copy content exists.

---

## 41. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. Can an unknown station ever be admitted to the network, and how?
2. Do countersigns ever fail because a keeper was careless, and what happens?
3. Is a sealed letter form truly sealed, and who enforces that?
4. Can a station refuse traffic for shelter reasons, and who decides?
5. Does the network ever carry a message the sender regrets, and how is that
   handled with the neighbors who read it?
6. Are station logs inspectable by anyone in the shelter?
7. What happens when a walker dies on the ridge?
8. Does the network keep operating when the keepers disagree?

None of these may be decided unilaterally; each changes tone and balance.

---

## 42. APPENDIX AB — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 14 Above the Ash | Air routes and signal stations |
| 2 | 21 The Grid | No power needed; contrast with radio |
| 3 | 23 The Alarm | Alert relay outward |
| 3 | 25 The Iron Road | Ridge path and rail spur walks |
| 4 | 30 The Press | Printed codebooks and forms |
| 5 | 33 The Weather | Windows and ash visibility |
| 5 | 34 The Long Road | Walker routes and caches |
| 5 | 36 The Watch | Shared horizon awareness |
| 6 | 40 The Wheel | Mirror mounts and shutters |
| 6 | 41 The Quiet | Night traffic and sleep |
| 7 | 43 The Question | Signal study and method |
| 7 | 44 The Outpost | The far end of the chain |
| 7 | 45 The Envoy | Messages between meetings |
| 7 | 46 The Long Change | Sight lines change over years |
| 8 | 47 The Brigade | Lamp fire safety |

Each hook is additive. The Mirror can ship alone, and every other expansion
can ship without it.

---

## 43. APPENDIX AC — ENDING PROSE SKETCHES

**The Speaking Ridge.** Five stations link the valley and the long line crosses
in an afternoon, and the shelter measures distance in windows.

**The Quiet Channel.** Countersigns, schedules, and silence discipline make the
network discreet without ever pretending it is invisible.

**The Open Book.** The codes are published to the neighbors, the logs are
shared, and nothing is sent that the shelter would not read aloud at the far
end.

**The Chain Unbroken.** Every hop has a successor, every cache is stocked, and
the network outlives the people who built it.

**The Kept Eye.** When the radio weather returns, the mirror is dusted weekly
and drilled each season, the network of last resort kept bright.

**Fade.** A mirror flashing twice at noon, an answering flash from a far ridge,
and a log entry: received, understood, replied.

---

## 44. APPENDIX AD — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Radio duplication | authority break | parallel channel |
| Spy drama | tone break | visible-message ethics |
| Protocol copying | legal/tone risk | invented forms |
| Instant reach | unreal | hops and windows |
| Secret codes | thriller logic | countersigns |
| Lamp hazards ignored | safety | fire owner rules |
| Ack omission | false confidence | proof required |
| Map takeover | authority break | reveal ports |
| Mystical light | tone break | work and weather |
| Shameful messages | ethics | open-care writing |

The list exists because optical signaling is easy to write as magic or
intrigue. The expansion's rule is that the mirror is a tool, the ridge is the
boss, and a message that cannot bear being read is a message the shelter should
not send.

---

## 45. APPENDIX AE — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Stations | 10 | 2,000 |
| Sight pairs | 20 | 4,000 |
| Codes | 40 | 6,000 |
| Windows | 12 | 2,500 |
| Chains | 6 | 2,000 |
| Acks | 8 | 2,000 |
| Security notes | 14 | 3,000 |
| Lamps | 8 | 2,000 |
| Logs | 24 | 4,000 |
| Forms | 16 | 3,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~61,500** |

---

## 46. APPENDIX AF — FIRST YEAR OF THE LIGHT

| Season | Focus | Milestone |
|---|---|---|
| Spring | Clean, survey | stations sited |
| Summer | Codes, traffic | first long line |
| Autumn | Storm, night | ash crossing, lamp |
| Winter | Drill, rotate | four-hop proof |

Four seasons is the honest arc for a network: a spring of looking, a summer of
speaking, an autumn that proves it under ash, and a winter of drills that make
the proof repeatable.

---

## 47. APPENDIX AG — CACHE CONTENT TABLE

| # | Cache | Contents | Check | Winter |
|---|---|---|---|---|
| 1 | Relay bunker | oil, glass, cloth | monthly | yes |
| 2 | West shoulder | oil, food, blanket | monthly | no |
| 3 | Night cairn | oil, wick, stone | monthly | yes |
| 4 | Stone terrace | food, fuel, glass | monthly | yes |
| 5 | Ridge mast | cloth, spares | seasonal | no |
| 6 | Lake hill | rope, food | monthly | no |
| 7 | School roof | chalk, slates | weekly | yes |
| 8 | Holdfast yard | full kit | weekly | yes |

Eight caches, each with a check cadence and a winter flag. The lake hill row
has rope in it because a cache is also a rescue station in miniature, and the
people who stock it are the people who walk the route.

---

## 48. APPENDIX AH — COUNTERSIGN ROTATION TABLE

| # | Season | Sign | Known by | Retired | Drill |
|---|---|---|---|---|---|
| 1 | Spring 1 | stone | keepers | spring 2 | yes |
| 2 | Summer 1 | water | keepers | summer 2 | yes |
| 3 | Autumn 1 | ash | keepers | autumn 2 | yes |
| 4 | Winter 1 | lamp | keepers | winter 2 | yes |
| 5 | Spring 2 | seed | keepers | spring 3 | yes |
| 6 | Summer 2 | ridge | keepers | summer 3 | yes |
| 7 | Autumn 2 | glass | keepers | autumn 3 | yes |
| 8 | Winter 2 | quiet | keepers | winter 3 | yes |

Eight rotations over two years, each retired with a drill and never reused.
Rotation is the network's only real secret, and it is a small one on purpose:
the plan would rather have an honest network that rotates on schedule than a
clever one that trusts a worn word for a decade.

---

## 49. APPENDIX AI — STORM LOG TABLE

| Day | Condition | Window | Message | Result |
|---|---|---|---|---|
| 1 | ash arriving | none | none | wait |
| 2 | ash thick | 0.36 | water short | delivered |
| 2 | ash thick | 0.34 | reply | blocked |
| 3 | ash thinning | 0.42 | ack water | delivered |
| 3 | ash thinning | 0.50 | need fuel | delivered |
| 4 | clearing | 0.60 | route check | delivered |
| 4 | clearing | 0.70 | all well | delivered |
| 5 | clear | 0.85 | year note | delivered |

The storm log is included in full because it is the expansion's argument for
existing. On day two, at 0.36 visibility, one short message crossed a valley
that the radio could not reach at all, and one reply failed by a hundredth of a
point — which is why the log records the failures beside the successes.

---

## 50. APPENDIX AJ — NETWORK COVENANT

| Clause | Promise |
|---|---|
| Ready | Stations are dusted and bright in every season |
| Surveyed | Lines are walked, not assumed |
| Brief | Forms are short enough for bad light |
| Verified | Strangers get countersigns, never secrets |
| Scheduled | Windows are published; sleep is honored |
| Proved | Delivery requires an ack, always |
| Exposed | Every message assumes it will be read |
| Cached | No station winters empty |
| Succeeded | Every hop has a named successor |
| Last | Whatever else fails, the light still speaks |

The covenant is the expansion's first-class design object, and its seventh line
is the one that separates this network from every spy story ever written: the
shelter does not assume privacy, it earns trust, and the light is honest
because it has no choice.

---

## 51. APPENDIX AK — STATION SUCCESSION TABLE

| Station | Keeper | Successor | Handover |
|---|---|---|---|
| Holdfast | Merrin | Gret | one window |
| Relay bunker | Teague | walker pair | one cleaning |
| Ridge mast | Hoyt | walker | one route |
| West shoulder | walker pair | rotating | one hop |
| Bent pine | none | survey crew | training only |
| Lake hill | walker pair | rotating | one hop |
| Smoke ridge | Ossian | night keeper | one night |
| Night cairn | Ossian | Gret | one lamp |
| School roof | Gret | trainee | one chalk line |
| Stone terrace | Hoyt | far keeper | one season |

Ten stations with named keepers and successors, and the two training-only rows
included to show that not every node carries traffic. The far keeper row is the
hardest handover in the network, which is why it takes a season and not an
afternoon.

---

## 52. APPENDIX AL — SURVEY NOTE TABLE

| # | Note | Says | Lesson |
|---|---|---|---|
| 1 | Shoulder forty | forty short | measure twice |
| 2 | Chimney roof | blocked low | short lines fail |
| 3 | Lake haze | noon only | water wakes late |
| 4 | Pine edge | trees move | trim or move |
| 5 | Evening ridge | shadow early | sunset is real |
| 6 | Smoke ridge | fair only | ash is a wall |
| 7 | Terrace winter | snow buries | cache or sleep |
| 8 | Far spur | unknown eyes | survey outward |

Eight survey notes, written on cards and kept in the signal office beside the
rejected red lines. The last note is the one Cleo insists every new surveyor
copies by hand, because a network that only looks inward does not know who is
watching it.

---

## 53. APPENDIX AM — FAR-SPUR CONTACT TABLE

| # | Contact | Signal | Countersign | Answer | Log |
|---|---|---|---|---|---|
| 1 | Day one sighting | alternating | asked | none | noted |
| 2 | Day two repeat | same form | asked again | none | noted |
| 3 | Day four pattern | three short | none sent | none | noted |
| 4 | Day six mirror | long-short | asked | wrong | refused |
| 5 | Day nine gap | none | none | none | watch |
| 6 | Day twelve form | message follows | asked | none | noted |
| 7 | Day fifteen form | all well | asked | all well | exchanged |
| 8 | Day sixteen gift | page form | none | ack page | logged |

Eight far-spur log entries across a fortnight, and the ending is the point:
the stranger learns the form, gives the countersign, and becomes the network's
first outside station. The log keeps every refusal and every unanswered flash
because a first contact that pretends to be easy is a first contact no player
will believe.

---

## 54. APPENDIX AN — WINDOW FAILURE TABLE

| # | Failed window | Cause | Attempt | Outcome | Lesson |
|---|---|---|---|---|---|
| 1 | Midday | haze | water need | blocked | wait shorter |
| 2 | Afternoon | cloud | ack | blocked | not urgent |
| 3 | Evening | sun low | all well | blocked | morning rule |
| 4 | Ash light | 0.34 | reply | blocked | margin needed |
| 5 | Ash light | 0.36 | water | delivered | act at once |
| 6 | Night | lamp wet | urgent | relit | spare wick |
| 7 | Winter | low sun | short form | delivered | shorten more |
| 8 | Storm | gate | none | none | wait |

Eight failed windows kept beside the successes, and the fourth and fifth rows
are the same message an hour apart with opposite outcomes. The window system's
truth is in that pair: an optical network lives or dies by a hundredth of a
visibility point, and the shelter keeps both entries so it never flatters
itself about reliability.

---

## 55. CLOSING STATEMENT

ASHFALL already has the bones of optical signaling: stations, conditions, a
message record with delivery and block states, a visibility gate, map-node
reveal ports, and a distress dispatch port. It has two stations and no practice.
The Mirror gives the shelter a working ridge network: sight lines walked with a
compass, a codebook worn smooth by use, windows published a day ahead, relays
handed person to person, acknowledgements that make delivery real, lamps lit
under fire rules, and the honest understanding that any light you flash can be
read by anyone with eyes on the valley. It adds no radio, no spycraft, and no
instant answers — just patience made visible.

> Wave 8 note: this plan is one of five Wave 8 expansion bibles (47–51). Each is
> self-contained; none requires another to ship. The shared Wave 8 index lives
> at `docs/expansions/wave8/WAVE8_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `HeliographSystem` (`HeliographStationDefinition`,
> `HeliographStationState`, `HeliographCatalog`, `HeliographMessageState`,
> `HeliographState`, `MinimumVisibility01 = 0.35f`, `RegisterStation`,
> `SetStationCondition`, `LoadCatalog`, `Transmit`, `CaptureState`,
> `RestoreState`, events `OnMessageDelivered` / `OnMessageBlocked` /
> `OnStateChanged`, and the map reveal plus distress dispatch ports),
> `HeliographHostSession` and `HeliographSaveStore` under the `heliograph`
> section, `Plans94To97Panel`, and `heliograph.json` (290 B, two stations:
> `heliograph_holdfast` at `loc_holdfast` and `heliograph_relay` at
> `loc_hidden_relay_bunker`).