# ASHFALL — Expansion 41 Design Bible
# THE QUIET
### Wave 6 · Sleep, Rest, Privacy, Noise, Crowding, and the Night Hours

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-23
**Domain owners touched:** `Ashfall.Core.Shelter` (ShelterNoiseSystem, ShelterAssignmentSystem, ShelterSocialDynamicsSystem, ShelterDecorSystem, ShelterAtmosphereSystem), `Ashfall.Core.Needs` (NeedsSystem), `Ashfall.Core.Survivors` (CaregivingSystem, SurvivorRelationsSystem)
**Proposed host owner:** `RestHostSession` (extends noise, assignment, and needs surfaces)
**Existing save sections:** shelter noise state, shelter assignment state, survivor needs
**Existing CLI verbs:** `--noise-selftest` (if present), `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already measures noise and needs. `ShelterNoiseSystem` defines
`NoiseSourceType` (Machinery, HumanActivity, IndustrialProcess, Alarm,
Ventilation, Generator, Construction, MusicRecreation, Argument),
`NoiseFrequency`, `NoiseSource` (`SourceId`, `Type`, `RoomId`, `NoiseOutput` =
50f, `Frequency`, `DurationHours` = 24f, `IsActive`, `CanBeSoundproofed`),
`RoomAcousticProfile` (`BaseNoiseLevel`, `WallSoundproofing`,
`DoorSoundproofing`, `EffectiveNoiseLevel`), `NoiseEvent` (`EventId`,
`EventType`, `Day`, `RoomId`, `Description`, `NoiseLevel`,
`DetectionRiskAdded`), and `ShelterNoiseState` (`SchemaVersion`,
`NextSequence`, `OverallNoiseLevel` = 20f, `DetectionRisk` = 5f,
`QuietHoursActive`, `QuietHoursStart` = 22, `QuietHoursEnd` = 6,
`RoomProfiles`, `Sources`, `Events`), with `OnNoiseSpike`,
`OnThreatDetectionRiskIncreased`, and `OnQuietHoursChanged` events.
`NeedsSystem` carries Fatigue, Numbness, Morale, and the rest of the survivor's
condition. `ShelterAssignmentSystem` places survivors in rooms.
`ShelterSocialDynamicsSystem` handles social outcomes and mediation.
`CaregivingSystem` tracks recovery bonds and `CaregiverFatigueDrain`.

There is no sleep data at all. No sleep schedules, no rest rooms, no
soundproofing content, no privacy rules, no crowding model, no sleep records,
and no night culture. Fatigue exists as a number that goes down and never as a
life.

**The Quiet** turns rest into a first-class system: sleep schedules, rest
spaces, soundproofing, quiet hours, crowding and privacy, night rosters, sensory
relief, and the records that show a shelter respects the people who keep it
awake. It extends the live noise, needs, assignment, and social owners.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| 17 The Long Evening | Leisure, music, festivals, hobbies | Manages noise from them, never their content |
| 23 The Alarm | Emergency evacuation and drills | Requests quiet hours exceptions |
| 36 The Watch | Night duty and readiness | Shares the night roster and fatigue surfaces |
| 37 The Quickening | Nursery routines | Shares night care and quiet rules |
| 38 The Ward | Patient rest | Provides rest standards and quiet zones |
| 27 The Thread | Bedding and cloth | Orders soft goods, never defines them |
| 40 The Wheel | Machines and mounts | Requests vibration and sound isolation |
| 24 The Long Goodbye | Grief and quiet sorrow | Routes through its owners |
| 21 The Grid | Generators and loads | Requests quiet hours load policy |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The ventilation hums, the generator drums, the workshop rings, someone is
arguing about a borrowed tool, and nobody in the shelter has slept properly in
a month.

**The Quiet** is the expansion about rest as infrastructure: sleep schedules,
quiet rooms, soundproofing, quiet hours, privacy, crowding, night rosters, and
the small human courtesies that let a shelter of tired people keep working. It
takes the existing noise number and turns it into the reason the night shift
falls asleep at the post.

### 1.2 The five loops it adds

```
  Noise ──► Measure ──► Isolate ──► Schedule ──► Rest
    │          │           │           │           │
    ▼          ▼           ▼           ▼           ▼
  Sources,   Levels,    Walls, do-  Shifts,     Sleep, recovery,
  events     readings   ments, rugs quiet hours  privacy, calm
                                                   │
                                                   ▼
                                            Record ──► Fix the worst room
```

### 1.3 What the player manages

1. **Noise.** Sources, levels, spikes, and their effect on sleep.
2. **Isolation.** Soundproofing walls, doors, baffles, mounts, and rugs.
3. **Schedules.** Sleep windows, shift rosters, naps, and recovery rest.
4. **Quiet hours.** When the shelter is quiet, and who is exempt.
5. **Privacy.** Screens, door rules, and personal space.
6. **Crowding.** Density, crowding, and its social consequences.
7. **Night culture.** Who is awake, who has been awake, and how they are
   treated.
8. **Sensory relief.** Calm rooms and breaks for people who need them.
9. **Records.** Sleep debt, noise complaints, and what the shelter changed.
10. **Culture.** The habits that make rest normal instead of shameful.

### 1.4 What it is not

- Not a morale-meter upgrade. Rest routes through `NeedsSystem` and the live
  social systems.
- Not a second noise system. It extends `ShelterNoiseSystem`.
- Not a punishment or shaming system. Fatigue is met with rotation and rest.
- Not a mental-health diagnosis system. Sensory and stress cases route to the
  live owners.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Shelter/ShelterNoiseSystem.cs` | Noise, quiet hours, detection risk | `LIVE` |
| `Assets/Ashfall.Core/Needs/NeedsSystem.cs` | Fatigue, morale, numbness | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` | Room assignment | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs` | Social outcomes, mediation | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs` | Room comfort | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterAtmosphereSystem.cs` | Comfort and cleanliness | `LIVE` |
| `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` | Recovery and caregiver fatigue | `LIVE` |
| `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` | Relationships and conflicts | `LIVE` |
| `Assets/Ashfall.Core/Recreation/SurvivorDowntimeSystem.cs` | Downtime | `LIVE` |
| `Assets/Ashfall.Core/Medical/MentalHealthCrisisSystem.cs` | Crisis support | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| Sleep, rest, privacy, crowding data | absent | confirmed none |
| Noise catalogs | absent | sources exist only as runtime state |
| Quiet hours fields | code defaults | 22:00–06:00 |
| Room acoustic profiles | runtime | no authored rooms |

### 2.3 Confirmed gaps

- **GAP-41-1 — No sleep model beyond the fatigue number.**
- **GAP-41-2 — No sleep schedules or rosters.**
- **GAP-41-3 — No rest rooms or quiet space content.**
- **GAP-41-4 — No soundproofing materials or effects content.**
- **GAP-41-5 — No quiet-hours policy content.**
- **GAP-41-6 — No privacy rules or screens.**
- **GAP-41-7 — No crowding model.**
- **GAP-41-8 — No night culture or night roster content.**
- **GAP-41-9 — No sensory relief rooms.**
- **GAP-41-10 — No sleep records or complaint review.**

### 2.4 Non-duplication statement

This expansion will **not** add a second noise, needs, assignment, social, or
mental-health system. It extends `ShelterNoiseSystem` with authored sources and
isolation materials, `NeedsSystem` with sleep debt consequences, the assignment
owner with rest spaces, and the social owner with crowding mediation. It adds
state only as additive sub-objects of the existing noise and assignment stores.
No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Sleep is labor's other half.** A shift is only real if the people
on it can sleep afterwards.

**Pillar 2 — Quiet is a shared good, not a private preference.** The shelter
negotiates quiet the way it negotiates water.

**Pillar 3 — Isolation is a material problem.** Rugs, seals, and mounts do more
than rules.

**Pillar 4 — Privacy is dignity.** Screens, doors, and knock rules are
infrastructure for a crowded life.

**Pillar 5 — Rest is never shameful.** No mechanic ever mocks a person for
sleeping, napping, or needing calm.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Sleep | Stolen hours and rosters | Laziness jokes |
| Insomnia | Patience and routine | Pathology spectacle |
| Noise complaint | Mediation | Snitching |
| Quiet hours | Collective agreement | Authoritarian silence |
| Sensory relief | Calm room and company | "Broken" framing |
| Crowding | Dignity under density | Misery tourism |
| Night culture | Respect for the awake | Martyrdom |
| Records | Patterns and fixes | Surveillance |

### 3.3 Content limits

- No shaming of sleep, naps, or needing quiet; no "lazy" framing anywhere.
- No mental-health diagnosis by the player; stress and sensory cases route to
  the live mental-health owners.
- No surveillance culture; sleep records are clinical and private, never a
  policing tool.
- No punishment for noise complaints; conflicts route to mediation.
- No soundproofing that hides abuse; an isolated room is never a place a person
  cannot leave.
- No new save section.

---

## 4. THE QUIET WORLD

### 4.1 Interior rooms

- **`room_sleep_quarters`** — bunks, curtains, and a night lamp.
- **`room_quiet_room`** — the designated silent room with a door that seals.
- **`room_day_sleep`** — blackout cloth and no visitors for night workers.
- **`room_calm_room`** — soft floor, low light, and a chair by the wall.
- **`room_night_office`** — the sleep keeper's desk and the rosters.
- **`room_linen_soft`** — rugs, felt, and soft stores.
- **`room_rest_corner`** — a bench near the warmest wall.
- **`room_machine_mount`** — where isolation work is measured and logged.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_quiet_yard` | The Quiet Yard | 1 | Outdoor calm |
| `loc_works_wall` | The Works Wall | 3 | Noise boundary |
| `loc_generator_shed` | The Generator Shed | 4 | The loudest place |
| `loc_vent_outlet` | The Vent Outlet | 3 | Hum source |
| `loc_night_walk` | The Night Walk | 2 | Insomnia route |
| `loc_stargazing_bench` | The Stargazing Bench | 1 | Calm and sky |
| `loc_sleep_tent` | The Sleep Tent | 3 | Overflow rest |
| `loc_dawn_bench` | The Dawn Bench | 1 | Early risers |
| `loc_carpet_rack` | The Carpet Rack | 2 | Soft stores |
| `loc_tool_wall` | The Tool Wall | 3 | Ringing metal |

All locations require valid item references and scanner registration.

### 4.3 The night

From the generator starting to the ventilation settling, the shelter's night has
a shape. The expansion's clock is the night cycle, and the sleep keeper's board
shows every hour of it.

---

## 5. MAIN STORYLINE — "HOURS THAT BELONG TO NOBODY"

### 5.1 Central conflict

**Ines** the sleep keeper has watched the shelter's fatigue for a year and
finally has the numbers. The night shift sleeps in the loudest room; the
workshop rings through the wall; the nursery and the ward both run on the same
broken rota; and the day-sleep room is a corridor with a blanket over the
window. **Tace** has not slept properly in weeks and will not say so. **Bo** the
carpenter can make a room quiet with rugs, seals, and a door that fits, and wants
the shelter to treat rest as a repair job with a schedule. **Fenn** plays late
and does not understand why the wall behind him is a problem until he tries to
sleep next to the generator. **Grell** works nights and has been awake for
twenty-two hours.

Then a fatigue error hurts someone, and the shelter stops treating sleep as a
personal weakness. The expansion's question: **whose hours are they, and who
gets to be tired?**

### 5.2 Theme (unspoken)

**A shelter that cannot rest cannot last.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_sleep_keeper_ines` | Ines | Sleep keeper | Rosters, records, negotiation |
| `npc_insomniac_tace` | Tace | Insomniac | The human cost, quiet dignity |
| `npc_carpenter_bo` | Bo | Quiet carpenter | Soundproofing and doors |
| `npc_musician_fenn` | Fenn | Late musician | Noise vs. culture negotiation |
| `npc_shift_worker_grell` | Grell | Night worker | Day sleep and rota fairness |
| `npc_reader_ame` | Ame | Reader | Calm room and quiet company |
| `npc_rest_aide_tarrow` | Tarrow | Ward rest aide | Recovery rest standards |
| `npc_engineer_vesh` | Vesh | Night engineer | Machinery quieting |

### 5.4 Story beats (15)

1. **The Numbers.** Ines presents a month of fatigue data.
2. **The Worst Room.** The loudest quarter is identified.
3. **The Rugs.** Bo's first isolation work changes one room completely.
4. **The Rota.** Sleep windows are written onto the shift board.
5. **The Door.** A fitted door and seal cut the vent hum in half.
6. **The Night Shift.** Grell's day-sleep room is protected.
7. **The Argument.** Fenn's late music sparks the shelter's first noise
   mediation.
8. **The Quiet Hours.** The shelter agrees on times, exceptions, and courtesy.
9. **The Mounts.** Machines are isolated from the floor by felt and sand.
10. **The Calm Room.** A sensory room opens for people who need it.
11. **The Ward.** Tarrow sets recovery rest standards with the ward.
12. **The Fatigue Error.** An accident traces to a missed sleep window.
13. **The Records.** Sleep debt becomes a number the shelter acts on.
14. **The Winter Nights.** Long dark hours test quiet and warmth together.
15. **Hours That Belong to Nobody.** The shelter decides what rest means.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Quiet hours | strict / layered / voluntary | silence vs. trust |
| Day sleep | protected rooms / screens / tents | dignity vs. cost |
| Isolation | materials / rules / both | engineering vs. policy |
| Night culture | one shelter / two shifts / tolerance | unity vs. friction |
| Calm room | open / referred / private | access vs. privacy |
| Music | banned / hours / rooms | silence vs. life |
| Rota | eight-hour / staggered / flexible | simplicity vs. fairness |
| Final | quiet as institution / habit / memory | identity |

### 5.6 Endings (5 + fade)

1. **The Sleeping Shelter** — rosters, rooms, and materials give everyone a real
   chance at rest.
2. **The Loud Heart** — the shelter keeps its music and its machines and learns
   to schedule them kindly.
3. **The Protected Hours** — quiet hours are settled and the night shift is
   treated as people.
4. **The Kind Room** — the calm room becomes permanent and ordinary.
5. **The Wired Week** — a hard stretch tests the system and finds its weak
   hours, and the shelter fixes them without blame.
6. **Fade** — a dark room, a soft rug, a closed door, and the ventilation
   turned down for the night.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_quiet_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_quiet_numbers`, `quest_quiet_worst_room`, `quest_quiet_rugs`,
`quest_quiet_rota`, `quest_quiet_door`, `quest_quiet_night_shift`,
`quest_quiet_argument`, `quest_quiet_hours`, `quest_quiet_mounts`,
`quest_quiet_calm_room`, `quest_quiet_ward`, `quest_quiet_fatigue_error`,
`quest_quiet_records`, `quest_quiet_winter_nights`, `quest_quiet_hours_ours`.

### 6.2 Side quests (30)

**Sleep (5)**
- `quest_quiet_schedule` — write a sleep schedule
- `quest_quiet_window` — protect a sleep window
- `quest_quiet_nap` — make naps normal
- `quest_quiet_debt` — track sleep debt
- `quest_quiet_insomnia` — a routine for a person who cannot sleep

**Rest spaces (5)**
- `quest_quiet_bunks` — build bunks
- `quest_quiet_curtains` — curtains and screens
- `quest_quiet_day_room` — day-sleep room
- `quest_quiet_sleep_tent` — overflow rest
- `quest_quiet_warmth` — rest near warmth

**Sound (5)**
- `quest_quiet_seal` — door seals
- `quest_quiet_rug` — rug and felt layer
- `quest_quiet_baffle` — wall baffles
- `quest_quiet_mount` — machine mounts
- `quest_quiet_measure` — measure a room

**Quiet hours (5)**
- `quest_quiet_policy` — write the hours
- `quest_quiet_exception` — exception list
- `quest_quiet_notice` — post the hours
- `quest_quiet_reminder` — a kind reminder system
- `quest_quiet_review` — review complaints

**Crowding (5)**
- `quest_quiet_space` — personal space rules
- `quest_quiet_density` — count and redistribute
- `quest_quiet_mediation` — noise mediation
- `quest_quiet_doors` — knock rules
- `quest_quiet_common` — quiet common room

**Night culture (5)**
- `quest_quiet_roster_night` — night roster fairness
- `quest_quiet_meal` — a hot meal at 3 a.m.
- `quest_quiet_company` — quiet company for the awake
- `quest_quiet_dawn` — dawn ritual
- `quest_quiet_thanks` — thanking the night shift

### 6.3 Repeatable quests (8)

`quest_quiet_repeat_measure`, `quest_quiet_repeat_rota`,
`quest_quiet_repeat_seal`, `quest_quiet_repeat_rug`,
`quest_quiet_repeat_review`, `quest_quiet_repeat_calm`,
`quest_quiet_repeat_night`, `quest_quiet_repeat_debt`.

### 6.4 Dynamic hooks

Live events (noise spikes, quiet-hours changes, shift roster updates, accidents,
conflicts, seasonal darkness, ward admissions, dependency stress) attach
authored follow-ups through existing seams. No new event bus.

### 6.5 Constraints

- Noise and quiet hours stay with `ShelterNoiseSystem`.
- Fatigue, morale, and numbness stay with `NeedsSystem`.
- Rooms and density stay with `ShelterAssignmentSystem`.
- Conflicts route to `ShelterSocialDynamicsSystem` mediation.
- Stress and sensory cases route to the live mental-health owners.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `SleepSystem` (new, `Ashfall.Core.Needs`)

**Owns:** sleep windows, sleep debt, nap content, and sleep quality inputs.
**Consumes:** `NeedsSystem` (fatigue, warmth, morale), `ShelterNoiseSystem`
(noise level), `DutyRoster` (shifts), `CaregivingSystem` (recovery rest).
**Data:** `sleep_schedules.json`, `sleep_profiles.json`.
**Rules:** fatigue recovery is a function of hours, noise, warmth, and darkness;
sleep debt accumulates visibly; nothing about sleep is shameful; the system
writes only through the live needs owner.

### 7.2 `RestSpaceSystem` (extend `ShelterAssignmentSystem`)

**Owns:** rest rooms, bunks, screens, day-sleep rooms, recovery corners, and the
privacy index. **Consumes:** assignment owner, `ShelterDecorSystem`,
`ShelterAtmosphereSystem`, `Inventory`. **Data:** `rest_rooms.json`.
**Rules:** a rest space has noise, light, warmth, and privacy values; assignment
honors sleep windows; no room is assigned that a person cannot leave.

### 7.3 `SoundproofingSystem` (extend `ShelterNoiseSystem`)

**Owns:** isolation materials, room treatments, and measured results.
**Consumes:** the live wall and door soundproofing fields, `Inventory`
(rugs, felt, seals, sand, timber), `MachineToolSystem` (Wave 6) for mounts.
**Data:** `soundproofing.json`. **Rules:** every treatment has a measured effect
on the live effective noise level; results are recorded; nothing claims perfect
silence.

### 7.4 `QuietHoursSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** quiet-hours policy, exception lists, reminders, and reviews.
**Consumes:** `ShelterNoiseSystem` quiet hours fields, `NoticeSystem` (Wave 4),
`DutyRoster`, `AlarmSystem` (Wave 3) exemptions.
**Data:** `quiet_hours.json`. **Rules:** quiet hours are agreed and posted;
exceptions are explicit and owned; enforcement is reminder-first; complaints
route to mediation, never to punishment.

### 7.5 `CrowdingSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** density, personal space, privacy index, and crowding consequences.
**Consumes:** `ShelterAssignmentSystem`, `ShelterSocialDynamicsSystem`,
`NeedsSystem` (numbness, morale), room data. **Data:** `crowding_rules.json`.
**Rules:** crowding raises friction and lowers rest; consequences route through
the live social and needs owners; redistribution is a real solution and
proceeds through assignment.

### 7.6 `SensoryReliefSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** calm rooms, sensory breaks, and referral routing. **Consumes:**
`ShelterNoiseSystem`, mental-health owners, `NeedsSystem`.
**Data:** `sensory_rooms.json`. **Rules:** calm is offered and never imposed;
people are never labeled by the system; the player sees a room and a person, not
a diagnosis.

### 7.7 `NightCultureSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** night rosters, night meals, night company, dawn rituals, and the
night-shift record. **Consumes:** the watch (36), ward (38), and nursery (37)
surface owners, `KitchenNutritionSystem`, `DutyRoster`.
**Data:** `night_roster.json`, `sleep_records.json`. **Rules:** the people who
keep the shelter awake are thanked and fed; the night record shows who has been
awake how long; no martyrdom is rewarded.

### 7.8 Systems explicitly not added

- No second noise, needs, assignment, social, or mental-health system.
- No laziness or shame content.
- No surveillance or punishment mechanics.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `sleep_schedules.json` (new)

```json
{
  "schema_version": 1,
  "schedules": [
    {
      "schedule_id": "sleep_night_shift",
      "display_name": "Night Shift Sleep",
      "window_start": 8,
      "window_end": 15,
      "min_hours": 6,
      "protected": true,
      "room_tag": "day_sleep",
      "tags": ["roster", "protected"]
    }
  ]
}
```

### 8.2 `sleep_profiles.json` (new)

Profiles: light sleeper, deep sleeper, napper, short sleeper, recovery rest.

### 8.3 `rest_rooms.json` (new)

Rooms: bunks, screens, noise, light, warmth, privacy, occupancy.

### 8.4 `soundproofing.json` (new)

Treatments: seal, rug, baffle, felt, sand, double wall, mount, effect, cost.

### 8.5 `quiet_hours.json` (new)

Policy: start, end, exceptions, notice, review cadence, escalation path.

### 8.6 `noise_catalog.json` (new)

Authored noise sources per room type: machinery, ventilation, generator,
workshop, music, argument, construction, alarm, with level and frequency.

### 8.7 `crowding_rules.json` (new)

Rules: density bands, personal space, privacy index, consequence, remedy.

### 8.8 `sensory_rooms.json` (new)

Rooms: calm room, soft room, low-light nook, with access and referral rules.

### 8.9 `night_roster.json` (new)

Roster: role, shift, person, hours awake, rest window, meal, relief.

### 8.10 `sleep_records.json` (new)

Records: person, night, hours, noise, interruptions, debt, action taken.

### 8.11 Items

New items appended to `items.json`: `item_wool_ear_stops`, `item_sleep_mask`,
`item_bunk_frame`, `item_privacy_screen`, `item_soft_rug`, `item_door_seal`,
`item_felt_pad`, `item_machine_mount`, `item_blackout_cloth`,
`item_reading_lamp`, `item_hammock`, `item_cot`, `item_door_weight`,
`item_cork_sheet`, `item_quiet_bell`, `item_night_cup`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`ShelterNoiseState` and the assignment state remain the live save owners. New
sub-objects (sleep schedules, rest rooms, soundproofing, quiet hours, crowding,
sensory rooms, night roster, sleep records) are additive inside them. No new
save section.

### 9.2 State to persist

- Sleep windows, debt, and quality inputs.
- Rest room assignments and privacy values.
- Soundproofing treatments and measured effects.
- Quiet-hours policy and exceptions.
- Crowding bands and redistribution.
- Sensory room access.
- Night rosters and hours-awake records.
- Sleep records and review actions.

### 9.3 Determinism

- Sleep recovery derives from hours, noise, warmth, and darkness through the
  live needs system.
- Noise levels come from live sources and profiles; treatments modify the live
  fields.
- Crowding consequences route through live social and needs paths.
- Naps and windows resolve by day and hour, never wall-clock.
- Paired replay hashes must match; no `System.Random`.

### 9.4 Migration

Legacy saves load with noise, quiet hours, and assignments untouched; no sleep
schedule, rest room, or record state exists until started. Existing noise
sources and room profiles keep working; authored catalogs add to them.

### 9.5 Checksum

Invariant-culture floats; integer hour and night counts.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `SleepPanel` (new) | Windows, debt, quality | `RestHostSession` |
| `RestRoomPanel` (new) | Rooms, privacy, assignment | same |
| `QuietHoursPanel` (new) | Policy, exceptions, notices | same |
| `NoiseMapPanel` (new) | Sources and levels per room | same |
| `CrowdingPanel` (new) | Density and space | same |
| `CalmRoomPanel` (new) | Sensory access | same |
| `NightPanel` (new) | Roster, meals, hours awake | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Noise is shown as a readable map with text labels, never sound-only.
- Sleep debt is a plain number with a plain explanation, never a red shaming
  bar.
- Quiet hours are readable in 24-hour and plain-language forms.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Content can be read aloud without embarrassment.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a door closing softly, a curtain
drawn, a ventilation hum fading, a mug set down at 3 a.m., a night bell rung
once. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `ShelterNoiseSystem` | Noise sources, profiles, quiet hours |
| `NeedsSystem` | Fatigue, morale, numbness |
| `ShelterAssignmentSystem` | Rooms and density |
| `ShelterSocialDynamicsSystem` | Mediation |
| `ShelterDecorSystem` | Comfort and soft goods |
| `ShelterAtmosphereSystem` | Comfort and cleanliness |
| `CaregivingSystem` | Recovery rest |
| `SurvivorDowntimeSystem` (Wave 2) | Recruitment of leisure time |
| `MentalHealthCrisisSystem` | Stress and sensory referrals |
| `DutyRoster` (Exp 02) | Shifts and rosters |
| `AlarmSystem` (Wave 3) | Quiet-hours exceptions |
| `NoticeSystem` (Wave 4) | Posted quiet hours |
| `MachineToolSystem` (Wave 6) | Mounts and vibration |
| `WatchHouseHostSession` (Wave 5) | Night roster sharing |
| `WardHostSession` (Wave 6) | Recovery rest standards |
| `NurseryHostSession` (Wave 6) | Night care quiet rules |
| `McMurdoAtmosphere` | Hum and comfort |
| `EpilogueChronicleBuilder` | Rest milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm noise, needs, assignment, social, decor,
atmosphere, caregiving, downtime, and mental-health owners. Record file:line;
change nothing.

**Phase 1 — Data + validators.** Author the ten catalogs; append items; register
validators and scanner.

**Phase 2 — Pure Core.** `SleepSystem`, `RestSpaceSystem`, `SoundproofingSystem`,
`QuietHoursSystem`, `CrowdingSystem`, `SensoryReliefSystem`,
`NightCultureSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `RestHostSession`, selftest coverage, fresh journey
from the numbers to the sleeping shelter.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak: a fatigue error, a winter night, a music
dispute, and a crowded month.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Sleep schedules | 10 |
| Sleep profiles | 6 |
| Rest rooms | 10 |
| Soundproofing treatments | 12 |
| Quiet-hours policies | 5 |
| Noise sources | 20 |
| Crowding rules | 8 |
| Sensory rooms | 4 |
| Night rosters | 8 |
| Sleep records | 20 |
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
| Shame content | Critical | No-shame contract |
| Second noise/needs system | Critical | Extend live owners |
| Surveillance tone | High | Private records |
| Soundproofing hides harm | High | Doors always openable |
| Fatigue ignored | High | Rosters and debt |
| Quiet tyranny | Medium | Negotiated policy |
| Determinism break | Low | Live seeded paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `sleep_schedules.json` | 10 | 2,500 |
| `sleep_profiles.json` | 6 | 1,500 |
| `rest_rooms.json` | 10 | 3,000 |
| `soundproofing.json` | 12 | 3,000 |
| `quiet_hours.json` | 5 | 2,000 |
| `noise_catalog.json` | 20 | 3,500 |
| `crowding_rules.json` | 8 | 2,000 |
| `sensory_rooms.json` | 4 | 1,500 |
| `night_roster.json` | 8 | 2,500 |
| `sleep_records.json` | 20 | 3,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~57,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R41-1 | Shame framing | Low | Critical | No-shame contract |
| R41-2 | Second noise system | Low | Critical | Extend live owner |
| R41-3 | Surveillance culture | Med | High | Private records |
| R41-4 | Isolation hides abuse | Low | Critical | Openable doors |
| R41-5 | Fatigue invisible | Med | High | Debt and rosters |
| R41-6 | Quiet as control | Med | High | Negotiated policy |
| R41-7 | Sensory labeling | Med | High | Room not diagnosis |
| R41-8 | Determinism | Low | High | Live paths |
| R41-9 | Content overrun | Med | Med | Budget §13 |
| R41-10 | Overlap with leisure | Med | Med | Boundary §0.1 |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Are quiet hours enforced or reminded?** Recommended: reminded, with a
   mediated review after repeated conflicts.
2. **Does sleep debt create hard penalties?** Recommended: it raises fatigue and
   error risk through live systems, never a hidden multiplier.
3. **Is the calm room open access or referral?** Recommended: open access with
   a voluntary referral path.
4. **Can music be banned?** Recommended: no; it is scheduled and roomed, never
   prohibited.
5. **Who owns the night roster?** Recommended: the sleep keeper with the watch,
   ward, and nursery hosts as co-signers.

---

## 17. APPENDIX D — SLEEP SCHEDULE TABLE (10 SCHEDULES)

| # | Schedule | Window | Minimum | Protected | Room |
|---|---|---|---|---|---|
| 1 | Day shift | 22–06 | 7h | yes | quarters |
| 2 | Night shift | 08–15 | 6h | yes | day sleep |
| 3 | Watch night | 07–13 | 5h | yes | day sleep |
| 4 | Ward night | 09–15 | 5h | yes | rest corner |
| 5 | Nursery rota | split | 4h | yes | day sleep |
| 6 | Split shift | 13–17, 22–02 | 6h | yes | quarters |
| 7 | Recovery rest | all day | 8h | yes | ward |
| 8 | Post-call | 10–16 | 6h | yes | quiet room |
| 9 | Child afternoon | 13–15 | 2h | no | quarters |
| 10 | Flexible | chosen | 6h | yes | any quiet |

The schedule table is the expansion's core promise: a shelter writes down when
people may sleep and then defends those hours. The protected column is the part
that matters, because a schedule nobody defends is just a wish.

---

## 18. APPENDIX E — SLEEP PROFILE TABLE

| Profile | Trait | Needs | Sensitivity | Support |
|---|---|---|---|---|
| Light sleeper | wakes easily | quiet | noise | sealed room |
| Deep sleeper | hard to wake | alarm | none | responsibility |
| Napper | short blocks | two naps | timing | flexible rota |
| Short sleeper | needs little | 5h | none | useful |
| Long sleeper | needs a lot | 9h | noise | protected |
| Recovery rest | healing | all day | disturbance | ward rules |

Profiles are content, not conditions. The shelter's job is to fit a schedule to
a person instead of fitting a person to a schedule, and the table shows how
ordinary that work is.

---

## 19. APPENDIX F — REST ROOM TABLE

| Room | Bunks | Noise | Light | Warmth | Privacy |
|---|---|---|---|---|---|
| Quarters | 6 | 35 | dim | warm | curtains |
| Quiet room | 2 | 12 | dark | warm | sealed |
| Day sleep | 4 | 15 | blackout | warm | screened |
| Rest corner | 1 | 25 | dim | warm | screen |
| Sleep tent | 4 | 30 | dim | cool | open |
| Recovery | 4 | 20 | dim | warm | ward |
| Nursery | cots | 25 | low | warm | keeper |
| Calm room | chair | 10 | soft | warm | door |
| Night office | 0 | 20 | lamp | warm | desk |
| Bunk nook | 2 | 30 | dim | warm | screen |

Rest rooms are measured the way the shelter measures food and water. The noise
and light columns come from live profiles, which means a room is quiet because
of what was built into it, not because the player decided it should be.

---

## 20. APPENDIX G — SOUNDPROOFING TABLE

| Treatment | Effect | Cost | Install | Best against |
|---|---|---|---|---|
| Door seal | −10 | low | 1h | hum |
| Soft rug | −5 | low | 1h | footsteps |
| Wall felt | −8 | low | 2h | speech |
| Cork sheet | −7 | med | 3h | vibration |
| Double wall | −15 | high | days | machinery |
| Sandbag frame | −12 | med | 4h | impact |
| Machine mount | −10 source | med | 2h | drum |
| Curtain layer | −4 | low | 1h | light, sound |
| Baffle board | −9 | med | 3h | echo |
| Door weight | −6 | low | 1h | rattle |
| Pipe wrap | −7 | med | 2h | flow hum |
| Vent plenum | −11 | high | days | vent roar |

The table makes silence a material project. Every row is a thing a carpenter can
build, a result that can be measured, and a cost the shelter pays, which is why
Bo's first rug matters more than any speech about quiet.

---

## 21. APPENDIX H — QUIET HOURS TABLE

| Version | Start | End | Exceptions | Enforcement |
|---|---|---|---|---|
| Strict | 21 | 07 | alarms, ward | reminder |
| Layered | 22 | 06 | watch, ward, nursery | reminder |
| Voluntary | none | none | all | none |
| Split | 13–15, 22–06 | — | ward, nursery | reminder |
| Winter | 20 | 08 | ward, watch | reminder |

Quiet hours are a negotiated policy, not a decree. The exception column keeps
the policy honest: a shelter that silences its own hospital at night has
forgotten what quiet is for.

---

## 22. APPENDIX I — NOISE SOURCE TABLE (20 SOURCES)

| # | Source | Type | Level | Frequency | Room |
|---|---|---|---|---|---|
| 1 | Generator | Generator | 75 | low | shed |
| 2 | Ventilation | Ventilation | 55 | medium | all |
| 3 | Lathe | Machinery | 60 | high | shop |
| 4 | Saw frame | Machinery | 65 | medium | saw shed |
| 5 | Trip hammer | Machinery | 70 | low | shop |
| 6 | Mill stones | Machinery | 55 | low | mill |
| 7 | Pump | Machinery | 50 | low | race |
| 8 | Workshop bench | HumanActivity | 45 | medium | shop |
| 9 | Kitchen | HumanActivity | 50 | medium | kitchen |
| 10 | Dining | HumanActivity | 55 | medium | hall |
| 11 | Corridor traffic | HumanActivity | 40 | high | all |
| 12 | Music evening | MusicRecreation | 50 | mixed | hall |
| 13 | Late argument | Argument | 55 | medium | quarters |
| 14 | Construction | Construction | 65 | medium | site |
| 15 | Alarm test | Alarm | 80 | high | shelter |
| 16 | Foundry | IndustrialProcess | 70 | low | foundry |
| 17 | Kiln blower | IndustrialProcess | 60 | medium | kiln |
| 18 | Chemical vent | IndustrialProcess | 55 | medium | works |
| 19 | Nursery cry | HumanActivity | 50 | high | nursery |
| 20 | Night footsteps | HumanActivity | 30 | high | all |

Twenty sources, each with a level and a frequency, give the shelter a real
soundscape. The list is also the diagnostic menu: when sleep fails in a room,
the player reads down the column and finds the source.

---

## 23. APPENDIX J — CROWDING RULE TABLE

| Band | Density | Effect | Remedy |
|---|---|---|---|
| Comfortable | 1 per 6 m² | none | none |
| Tight | 1 per 4 m² | friction | redistribute |
| Crowded | 1 per 3 m² | rest loss | new space |
| Overcrowded | 1 per 2 m² | conflict | tent, build |
| Crisis | shared bunks | morale | emergency |
| Recovering | falling | relief | keep plan |

Crowding is treated as a measured condition with a remedy, never as a moral
failure of the people living in it. The remedy column is the whole point: the
shelter's answer to a crowded room is a bigger room or a fairer distribution.

---

## 24. APPENDIX K — SENSORY ROOM TABLE

| Room | Feature | Access | Rules | Referral |
|---|---|---|---|---|
| Calm room | soft walls, low light | open | quiet, no shoes | voluntary |
| Soft nook | rug, cushion | open | one person | none |
| Dark corner | blackout cloth | open | no light | none |
| Green corner | plants | open | water them | none |

Sensory relief is offered as ordinary space, and the referral column is never a
diagnosis. The shelter sees a person who wants a quiet corner, not a label, and
the rooms are open to anyone who needs them for any reason.

---

## 25. APPENDIX L — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_quiet_numbers` | 4 | A month of fatigue data |
| `quest_quiet_worst_room` | 3 | Loudest room identified |
| `quest_quiet_rugs` | 4 | First isolation work |
| `quest_quiet_rota` | 4 | Sleep windows written |
| `quest_quiet_door` | 3 | Fitted door and seal |
| `quest_quiet_night_shift` | 4 | Day sleep protected |
| `quest_quiet_argument` | 4 | First noise mediation |
| `quest_quiet_hours` | 5 | Quiet hours agreed |
| `quest_quiet_mounts` | 4 | Machines isolated |
| `quest_quiet_calm_room` | 3 | Calm room opens |
| `quest_quiet_ward` | 4 | Recovery rest standards |
| `quest_quiet_fatigue_error` | 5 | Error traced and fixed |
| `quest_quiet_records` | 3 | Debt becomes actionable |
| `quest_quiet_winter_nights` | 5 | Long dark hours tested |
| `quest_quiet_hours_ours` | 3 | Final disposition |

---

## 26. APPENDIX M — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_quiet_schedule` | 3 | Schedule written |
| `quest_quiet_window` | 3 | Window protected |
| `quest_quiet_nap` | 3 | Naps normalized |
| `quest_quiet_debt` | 4 | Debt tracked |
| `quest_quiet_insomnia` | 4 | Routine offered |
| `quest_quiet_bunks` | 4 | Bunks built |
| `quest_quiet_curtains` | 3 | Curtains hung |
| `quest_quiet_day_room` | 4 | Day room made |
| `quest_quiet_sleep_tent` | 3 | Tent pitched |
| `quest_quiet_warmth` | 3 | Warm rest spot |
| `quest_quiet_seal` | 3 | Seals fitted |
| `quest_quiet_rug` | 3 | Rugs laid |
| `quest_quiet_baffle` | 3 | Baffles mounted |
| `quest_quiet_mount` | 4 | Mounts installed |
| `quest_quiet_measure` | 3 | Room measured |
| `quest_quiet_policy` | 4 | Hours written |
| `quest_quiet_exception` | 3 | Exceptions listed |
| `quest_quiet_notice` | 3 | Hours posted |
| `quest_quiet_reminder` | 3 | Reminders agreed |
| `quest_quiet_review` | 4 | Complaints reviewed |
| `quest_quiet_space` | 3 | Space rules written |
| `quest_quiet_density` | 4 | Density reduced |
| `quest_quiet_mediation` | 4 | Mediation held |
| `quest_quiet_doors` | 3 | Knock rules agreed |
| `quest_quiet_common` | 3 | Common room quieted |
| `quest_quiet_roster_night` | 4 | Night roster fair |
| `quest_quiet_meal` | 3 | Night meal served |
| `quest_quiet_company` | 3 | Quiet company |
| `quest_quiet_dawn` | 3 | Dawn ritual |
| `quest_quiet_thanks` | 3 | Night shift thanked |

---

## 27. APPENDIX N — NPC DOSSIERS (BRIEF)

**Ines** — sleep keeper. Records hours the way the quartermaster records grain.
Believes rest is a shared resource and negotiates it like one, with patience and
numbers instead of authority.

**Tace** — insomniac. Has not slept properly in weeks and does not want to be a
problem. Speaks little; the shelter's response to Tace is the expansion's real
test.

**Bo** — quiet carpenter. Builds doors that fit, frames that do not rattle, and
rooms that keep their promises. Measures results and posts them.

**Fenn** — musician. Plays late because late is when the hall is free, and
volunteers to move to the far room without being asked once the wall becomes a
problem.

**Grell** — night worker. Has worked more night shifts than anyone and wants a
day-sleep room that nobody uses as a corridor.

**Ame** — reader. Opens the calm room, keeps it stocked with books and blankets,
and never asks anyone why they came.

**Tarrow** — ward rest aide. Sets recovery rest standards and guards them from
the ward's own busy-ness.

**Vesh** — night engineer. Quiets the ventilation, wraps the pipes, and checks
the generator mounts at three in the morning with a lamp and a chatty mood.

---

## 28. APPENDIX O — LOCATION DETAIL

- **The Quiet Yard** — the one place outdoors where voices drop by themselves.
- **The Works Wall** — the boundary where the shelter measures its loudest
  neighbor.
- **The Generator Shed** — the source of the drum and the reason for the
  sandbags.
- **The Vent Outlet** — where the hum is born and where pipe wrap begins.
- **The Night Walk** — a route for people who cannot sleep and should not
  pretend otherwise.
- **The Stargazing Bench** — calm, sky, and nobody counting anything.
- **The Sleep Tent** — overflow rest with real blankets and a real curtain.
- **The Dawn Bench** — for the early risers and the night shift ending.
- **The Carpet Rack** — soft stores and the smell of wool.
- **The Tool Wall** — where ringing metal teaches everyone why mounts matter.

---

## 29. APPENDIX P — SLEEP QUALITY MODEL

| Factor | Good | Poor | Effect |
|---|---|---|---|
| Hours | 7+ | under 5 | debt |
| Noise | under 20 | over 50 | interruptions |
| Warmth | warm | cold | wake risk |
| Light | dark | lit | depth |
| Privacy | screen | open | security |
| Timing | consistent | shifting | depth |
| Stress | calm | crisis | delay |
| Pain | managed | untreated | fractures |

The model is the bridge between the expansion and the live needs system. Every
row is an input the player can see and change, and none of them is a personal
quality judgement about the sleeper.

---

## 30. APPENDIX Q — WORKED 360-DAY REST SCENARIO

**Days 1–20.** Ines presents the fatigue data; the loudest quarter is measured;
the night shift's room is identified as a corridor with a blanket.

**Days 21–50.** Bo fits three doors and lays four rugs; the works wall gets
felt; measured noise in the worst room drops by half.

**Days 51–80.** Sleep windows written onto the shift board; day-sleep room
protected; Grell gets a real window and stops napping in the mess hall.

**Days 81–110.** Fenn's late music raises the first complaint; mediation is
held; the hall gets scheduled hours and the far room becomes the music room.

**Days 111–140.** Quiet hours agreed and posted; reminder system starts; the
ward and nursery take explicit exceptions and the policy survives its first
month.

**Days 141–170.** Machine mounts installed under the lathe and generator;
ventilation plenum lined; night noise falls enough for the first honest sleep
survey.

**Days 171–200.** The calm room opens; Ame keeps it warm; Tace starts sleeping
in short blocks and nobody makes it a story.

**Days 201–230.** Tarrow sets ward rest standards; rounds become quieter;
recovery times improve through the live caregiving systems.

**Days 231–260.** A fatigue error in the works traces to a missed sleep window;
shift records show who had been awake; the rota is corrected without blame.

**Days 261–290.** Winter darkness: quiet hours lengthen; the dawn bench becomes
a shared ritual; the nursery and watch share a night roster and lose fewer
hours.

**Days 291–320.** Sleep debt becomes a weekly number the shelter reads and
acts on; two rooms are re-assigned by density.

**Days 321–360.** Year review: twelve treatments, one policy, one error, and a
shelter that finally sleeps.

---

## 31. APPENDIX R — VIGNETTES (TONE SAMPLE)

> Bo hangs the door and checks it twice and then asks Grell to try it, and Grell
> closes the door and stands in the quiet for a while and says nothing, and Bo
> writes the number on the wall and makes no speech at all.

> Ines reads the sleep record and sees three nights in a row of five hours and
> does not write the word tired, and instead writes changed the window, and the
> difference between those two sentences is the whole expansion.

> Tace sits in the calm room with a blanket and does not explain, and Ame puts
> down a cup and leaves, and nobody asks a single question, and that is why Tace
> comes back.

> Fenn plays until nine and then stops in the middle of a song and says the
> hour out loud, and the room laughs, and the music room gets its own walls, and
> the shelter keeps its music and its sleep both.

---

## 32. APPENDIX S — NO-SHAME CONTRACT

| Clause | Promise |
|---|---|
| Sleep | Never mocked, never ranked |
| Naps | Normal, permitted, useful |
| Insomnia | Met with routine and company |
| Quiet | A shared good, negotiated |
| Debt | A number to fix, not a character flaw |
| Records | Private and clinical |
| Complaints | Mediated, never punished |
| Sensory needs | Rooms, not labels |
| Night work | Fed, thanked, rested |
| Fatigue errors | System failures, fixed by practice |

The contract is the expansion's first-class design object. Fatigue content turns
cruel very easily, and this table is the line the expansion refuses to cross.

---

## 33. APPENDIX T — NOISE MEDIATION PROTOCOL

| Step | Action | Tone |
|---|---|---|
| Listen | hear the person affected | calm |
| Measure | read the live noise level | factual |
| Understand | find the reason for the noise | fair |
| Offer | suggest rooms, hours, materials | practical |
| Agree | write the arrangement | clear |
| Try | test for one week | patient |
| Review | read the result | honest |
| Revise | change one thing, or none | respectful |

Mediation is the expansion's answer to conflict about sound, and its principle
is that both the person who needs quiet and the person who needs to make noise
are residents with legitimate needs. The shelter's job is an arrangement, not a
verdict.

---

## 34. APPENDIX U — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Missed window | fatigue | restore rota |
| Loud room | debt | isolate |
| Door rattle | wake risk | fit seal |
| Generator drum | whole shelter | mount, walls |
| Vent roar | night noise | plenum |
| Crowding | friction | redistribute |
| Quiet violation | conflict | mediate |
| Fatigue error | injury or loss | review, correct |
| Calm room misuse | harm to access | restate rules |
| Long winter | darkness | lengthen hours |

No failure shames a person and no failure is silent. The shelter's response to
every row is the same shape: measure, fix, write down, and try again.

---

## 35. APPENDIX V — CONTENT REVIEW CHECKLIST

- [ ] No shaming of sleep, naps, or quiet needs exists.
- [ ] `ShelterNoiseSystem` remains the noise and quiet-hours authority.
- [ ] `NeedsSystem` remains the fatigue authority.
- [ ] `ShelterAssignmentSystem` remains the room authority.
- [ ] Conflicts route to live mediation.
- [ ] Stress and sensory cases route to live mental-health owners.
- [ ] Records are private and clinical.
- [ ] Every isolated room can be opened from inside.
- [ ] Night workers are fed, thanked, and rested.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded paths only.

---

## 36. APPENDIX W — GLOSSARY

- **Window** — a protected block of hours when a person may sleep.
- **Debt** — accumulated shortfall that makes fatigue worse.
- **Isolation** — reducing sound with materials, not rules.
- **Effective level** — the noise a room actually delivers.
- **Quiet hours** — the agreed shared times for lowered sound.
- **Privacy index** — how much a room lets a person be unobserved.
- **Density** — how many people a space holds well.
- **Calm room** — open space for sensory relief.
- **Hours awake** — the number the night roster keeps honestly.
- **Reminder** — the first and usually only enforcement step.

---

## 37. APPENDIX X — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `ShelterNoiseSystem` | sources | levels, events | fatigue |
| `SleepSystem` | noise, needs | windows, debt | needs state |
| `NeedsSystem` | fatigue | needs | schedules |
| `RestSpaceSystem` | rooms | privacy values | assignment law |
| `SoundproofingSystem` | inventory | treatments | source levels |
| `QuietHoursSystem` | policy | hours, exceptions | enforcement |
| `CrowdingSystem` | density | bands, remedies | social state |
| `SensoryReliefSystem` | rooms | access | diagnosis |
| `NightCultureSystem` | rosters | records | shifts |
| `ShelterSocialDynamicsSystem` | conflicts | mediation | noise |
| `CaregivingSystem` | recovery | rest needs | care bonds |
| `MentalHealthCrisisSystem` | referrals | support | sleep state |
| `DutyRoster` | shifts | rosters | fatigue |
| `EpilogueChronicleBuilder` | milestones | chronicle | rest state |

---

## 38. APPENDIX Y — DATA SCHEMA DETAIL (NEW CATALOGS)

**`sleep_schedules.json`** — `schedule_id`, `display_name`, `window_start`,
`window_end`, `min_hours`, `protected`, `room_tag`, `tags`.

**`sleep_profiles.json`** — `profile_id`, `display_name`, `trait`, `needs`,
`sensitivity`, `support`, `tags`.

**`rest_rooms.json`** — `room_id`, `display_name`, `bunks`, `noise`, `light`,
`warmth`, `privacy`, `tags`.

**`soundproofing.json`** — `treatment_id`, `display_name`, `effect`, `cost`,
`install_hours`, `best_against`, `tags`.

**`quiet_hours.json`** — `policy_id`, `display_name`, `start`, `end`,
`exceptions[]`, `enforcement`, `tags`.

**`noise_catalog.json`** — `source_id`, `display_name`, `type`, `level`,
`frequency`, `room_id`, `tags`.

**`crowding_rules.json`** — `band_id`, `display_name`, `density`, `effect`,
`remedy`, `tags`.

**`sensory_rooms.json`** — `room_id`, `display_name`, `feature`, `access`,
`rules[]`, `referral`, `tags`.

**`night_roster.json`** — `role_id`, `display_name`, `shift`, `person`,
`hours_awake`, `rest_window`, `meal`, `tags`.

**`sleep_records.json`** — `record_id`, `person`, `night`, `hours`, `noise`,
`interruptions`, `debt`, `action`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid references, or out-of-range numbers.

---

## 39. APPENDIX Z — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Average hours slept | rest health | Sleep |
| Sleep debt | risk | Sleep |
| Room noise levels | isolation results | Noise |
| Interruptions | quality | Records |
| Protected windows kept | respect | Roster |
| Quiet-hour notices | politics | QuietHours |
| Mediations held | conflict health | Social |
| Crowding bands | space | Crowding |
| Night meals served | care | Night |
| Fatigue-related errors | safety | Works, Ward |

Telemetry is diagnostic only; it never gates content and never becomes a score
against a person.

---

## 40. APPENDIX AA — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Noise, needs, assignment, and social authorities remain untouched.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows a fatigue error, a winter night, and a crowded month.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No shaming, surveillance, or unopenable isolation exists.

---

## 41. APPENDIX AB — OPEN QUESTIONS FOR REVIEW

1. Should quiet hours be city-wide or room-specific in their effect?
2. Does sleep debt ever force a person to stand down from a shift?
3. Who decides when a calm room is too busy?
4. Should noise complaints be anonymous by default?
5. Can the shelter build its way out of crowding, or must it redistribute?
6. Are naps counted as rest or as privilege in the roster view?
7. Does music get a room, hours, or both?
8. How does the winter darkness change the default schedule?

None of these may be decided unilaterally; each changes tone and balance.

---

## 42. APPENDIX AC — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Children's sleep and quiet |
| 1 | 13 The Faithful | Night vigils and silence |
| 1 | 15 The Deep Root | Quiet fields and night air |
| 1 | 16 The Rebuilt Body | Rest for prosthetics users |
| 2 | 17 The Long Evening | Music, games, and late life |
| 2 | 18 The Underneath | Deep quiet rooms |
| 2 | 19 The Bitter Air | Masked sleep and sealed rooms |
| 2 | 21 The Grid | Generator quieting and load policy |
| 3 | 22 The Clean Flow | Hum from pumps and pipes |
| 3 | 23 The Alarm | Alarms during quiet hours |
| 3 | 24 The Long Goodbye | Quiet company and grief |
| 3 | 25 The Iron Road | Night train noise |
| 3 | 26 The Common Table | Night meals and tea |
| 4 | 27 The Thread | Rugs, curtains, and bedding |
| 4 | 28 The Lesson | Learning the rest rules |
| 4 | 29 The Glass | Lamps, dark glass, night light |
| 4 | 30 The Press | Posted quiet hours and notices |
| 4 | 31 The Kiln | Cork, brick, and door fit |
| 5 | 32 The Wild | Night sounds and animals |
| 5 | 33 The Weather | Winter darkness and storms |
| 5 | 34 The Long Road | Road noise and rest stops |
| 5 | 35 The Habit | Sleep and recovery |
| 5 | 36 The Watch | Night roster and hours awake |
| 6 | 37 The Quickening | Nursery quiet and night care |
| 6 | 38 The Ward | Recovery rest standards |
| 6 | 39 The Reagent | Vent noise and mounts |
| 6 | 40 The Wheel | Machine vibration and isolation |

Each hook is additive. The Quiet can ship alone, and every other expansion can
ship without it.

---

## 43. APPENDIX AD — ENDING PROSE SKETCHES

**The Sleeping Shelter.** Rosters, rooms, and materials give everyone a real
chance at rest, and the shelter's fatigue numbers fall for the first time in a
year.

**The Loud Heart.** The music and the machines still run, and both are scheduled
kindly, and nobody has to choose between a living shelter and a quiet one.

**The Protected Hours.** Quiet hours hold, the night shift is treated as people,
and the day-sleep room has a door that means something.

**The Kind Room.** The calm room becomes permanent and ordinary, and nobody
remembers a time when a person had to explain why they needed it.

**The Wired Week.** A hard stretch finds the weak hours, and the shelter fixes
them without blame, and the record shows exactly how it did it.

**Fade.** A dark room, a soft rug, a closed door, and the ventilation turned
down for the night, and a shelter that has learned to be quiet on purpose.

---

## 44. APPENDIX AE — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Lazy jokes | shaming | no-shame contract |
| Sleep as stat | hollow | windows and rooms |
| Quiet by decree | authoritarian | negotiated policy |
| Snitching | social harm | mediation |
| Diagnosis labels | harmful | rooms, not labels |
| Isolation room abuse | serious harm | openable doors |
| Fatigue ignored | unsafe | debt and rosters |
| Records as surveillance | distrust | private clinical records |
| Music banned | cultural loss | scheduled and roomed |
| Silent shelter ending | bleak | sleeping shelter |

The list exists because fatigue and quiet are easy to turn into a punitive
system or a bleak one. The expansion's rule is that rest is infrastructure, and
quiet is something a community builds together.

---

## 45. APPENDIX AF — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Sleep schedules | 10 | 2,500 |
| Sleep profiles | 6 | 1,500 |
| Rest rooms | 10 | 3,000 |
| Soundproofing treatments | 12 | 3,000 |
| Quiet hours policies | 5 | 2,000 |
| Noise sources | 20 | 3,500 |
| Crowding rules | 8 | 2,000 |
| Sensory rooms | 4 | 1,500 |
| Night rosters | 8 | 2,500 |
| Sleep records | 20 | 3,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~57,000** |

---

## 46. APPENDIX AG — FIRST YEAR OF THE QUIET

| Month | Focus | Milestone |
|---|---|---|
| 1 | Numbers | fatigue data presented |
| 2 | Worst room | isolation begins |
| 3 | Rota | sleep windows written |
| 4 | Door | sealed rooms |
| 5 | Night shift | day sleep protected |
| 6 | Mediation | music and hours settled |
| 7 | Mounts | machines quieted |
| 8 | Calm room | opens |
| 9 | Ward | rest standards |
| 10 | Error | reviewed and fixed |
| 11 | Winter | long hours tested |
| 12 | Review | year audited |

A year of the quiet is a year of small material fixes and one honest
conversation, and the shelter ends it with lower numbers, softer rooms, and a
night culture that treats the awake as people.

---

## 47. APPENDIX AH — SLEEP COVENANT

| Clause | Promise |
|---|---|
| Windows | Every worker has protected sleep hours |
| Rooms | Every sleeper has a room that can be quiet |
| Materials | Isolation is built, measured, and posted |
| Quiet | Hours are agreed, posted, and reminded |
| Privacy | Screens, knock rules, and doors that close |
| Crowding | Density is measured and remedied |
| Calm | Sensory space is open to anyone |
| Night | The awake are fed, thanked, and rested |
| Records | Debt is tracked to be fixed, never to shame |
| Errors | Fatigue failures change practice, not people |

The covenant is the expansion's whole argument written in one table: a shelter
that respects sleep will make fewer mistakes, keep more of its people, and last
longer than one that rewards exhaustion. The quiet is not the absence of life;
it is the condition that lets life continue.

---

## 48. APPENDIX AI — REST SUPPLY TABLE

| Supply | Use | Source wave | Reorder | Substitute |
|---|---|---|---|---|
| Wool ear stops | sleep | 27 | 20 | cloth wads |
| Sleep mask | darkness | 27 | 10 | folded cloth |
| Soft rug | floor noise | 27 | 6 | felt scraps |
| Door seal | noise | 27 | 8 | cloth strip |
| Felt pad | machine | 27 | 12 | leather |
| Cork sheet | vibration | 39 | 6 | sandbag |
| Blackout cloth | day sleep | 27 | 4 | blanket |
| Reading lamp | calm room | 29 | 3 | candle |
| Hammock | overflow | 27 | 2 | cot |
| Cot | rest | 31 | 4 | bunk |
| Door weight | rattle | 31 | 2 | stone |
| Quilt | warmth | 27 | 10 | two blankets |

The quiet is built from the other waves' stores. The table is also a reminder
that rest is not a free system: rugs, seals, and soft goods all come from real
workshops with real queues.

---

## 49. APPENDIX AJ — HOURS AWAKE TABLE

| Hours | State | Risk | Action |
|---|---|---|---|
| 0–8 | rested | low | none |
| 8–14 | working | low | meal, break |
| 14–18 | long | med | relief |
| 18–22 | extended | high | stand down soon |
| 22–26 | dangerous | very high | relief now |
| 26+ | critical | extreme | medical review |

The table is the night roster's reason for existing. The action column is
protective rather than punitive, and the highest band routes to the ward because
a person who has been awake for more than a day is a patient waiting to happen.

---

## 50. APPENDIX AK — NIGHT MEAL TABLE

| Meal | Time | Kitchen | Keeps | Served to |
|---|---|---|---|---|
| Mid shift | 02:00 | warm pot | 2h | watch, ward |
| Dawn | 05:30 | bread, tea | 1h | ending shifts |
| Nursery | 03:00 | light food | 1h | keepers |
| Ward | 04:00 | broth | 2h | night nurse |
| Emergency | any | cold ration | days | any call |
| Quiet | all night | kettle only | — | everyone |

A hot meal at three in the morning is the cheapest way a shelter can say that
the night shift matters. The table makes that care a schedule rather than an
act of heroism by the cook.

---

## 51. APPENDIX AL — QUIET MEASUREMENT TABLE

| Measurement | Where | Tool | Frequency | Recorded |
|---|---|---|---|---|
| Room level | each room | meter or ear | weekly | yes |
| Spike | source | meter | event | yes |
| After treatment | treated room | meter | once | yes |
| Night average | quarters | meter | nightly | yes |
| Day average | day sleep | meter | daily | yes |
| Generator | shed wall | meter | weekly | yes |
| Vent | outlet | meter | weekly | yes |
| Workshop | bench | meter | weekly | yes |

Measurement is what keeps the quiet honest. Without numbers, isolation becomes a
feeling, and a shelter that relies on feelings argues forever about whether a
room got quieter or just got used.

---

## 52. APPENDIX AM — SEASONAL DARKNESS TABLE

| Season | Daylight | Quiet hours | Day sleep | Notes |
|---|---|---|---|---|
| Spring | long | 22–06 | normal | outdoor calm |
| Summer | longest | 23–05 | curtains | heat, late life |
| Autumn | falling | 22–06 | normal | earlier dark |
| Winter | shortest | 20–08 | protected | long nights |
| Ash | dim | 21–07 | window covers | dust |
| Storm | dark | flexible | protected | lockdown |

Winter makes everything the quiet cares about harder, and the table is how the
shelter plans for it. Longer quiet hours, protected day sleep, and a dawn bench
that fills with people who are glad to see the sun again.

---

## 53. APPENDIX AN — PRIVACY FIXTURE TABLE

| Fixture | Room | Effect | Cost | Install |
|---|---|---|---|---|
| Curtain | bunks | sight | low | 1h |
| Screen | corners | sight | low | 1h |
| Door | quiet room | sound, sight | med | day |
| Latch | any door | security | low | 1h |
| Knock rule | all | dignity | none | — |
| Lamp hood | night desk | light | low | 1h |
| Bed curtain | ward | dignity | low | 2h |
| Changing nook | quarters | privacy | low | 2h |
| Lock box | each person | belongings | low | 1h |
| Reading screen | lamp | light | low | 1h |

Privacy fixtures are small objects with large effects. The lock box row matters
especially: a crowded shelter that gives every person a place for their own
things is a shelter that treats density as a condition and dignity as a right.

---

## 54. APPENDIX AO — ROOMMATE RULES TABLE

| Rule | Reason | Enforcement | Exception |
|---|---|---|---|
| Knock first | privacy | courtesy | emergency |
| Lights low at night | shared sleep | reminder | illness |
| Ear stops free | noise | supply | — |
| Curtains respected | privacy | courtesy | none |
| Storage personal | dignity | lock box | none |
| Guests by consent | safety | agreement | none |
| Quiet after hours | sleep | reminder | ward |
| Disputes mediated | peace | social owner | none |
| Space rotated | fairness | assignment | medical |
| No shame | culture | all | none |

Roommate rules are the smallest scale of the whole expansion: a shelter works
because people who share a room agree about light, sound, and stuff. The last
row is the culture that makes the other nine possible.

---

## 55. APPENDIX AP — CRY AND COMFORT TABLE

| Event | Night response | Quiet rule | Record |
|---|---|---|---|
| Infant cry | nursery keeper | door closed | routine |
| Child nightmare | nearby adult | soft voice | optional |
| Grief at night | company | quiet corner | private |
| Pain flare | ward or salve | screen | clinical |
| Panic | calm room | company | private |
| Bad dream | lamp, water | low voice | none |
| Cough | ward check | isolation if needed | clinical |
| Loneliness | night walk | company | none |

Night distress is met with presence and quiet, never with shame or a louder
rule. The table is the expansion's promise that the shelter's quiet is a place
where people can be heard when they need to be.

---

## 56. APPENDIX AQ — NIGHT SHIFT RIGHTS TABLE

| Right | Detail | Owner | Review |
|---|---|---|---|
| Sleep window | protected hours | Ines | monthly |
| Hot meal | served mid shift | kitchen | weekly |
| Dark room | blackout and seal | Bo | seasonal |
| Fair rota | rotation across people | Ines | monthly |
| Relief | someone to call | roster | weekly |
| Thanks | named in the record | all | monthly |
| Health check | fatigue review | ward | monthly |
| Voice | rota complaints heard | mediation | as needed |
| Rest day | one after long runs | rota | weekly |
| Dignity | no jokes about sleep | culture | always |

The rights table exists because the night shift is the part of the shelter
everyone relies on and no one sees. Writing the rights down is how the
expansion keeps its promise that the awake are treated as people rather than as
coverage.

---

## 57. APPENDIX AR — QUIET COMPLAINT LOG TABLE

| Column | Example | Purpose |
|---|---|---|
| Date | day 214 | record |
| Room | quarters west | locate |
| Source | late music | diagnose |
| Level | 55 | measure |
| Person | anonymous | protect |
| Mediation | held | process |
| Agreement | music room | remedy |
| Follow-up | week later | verify |
| Result | resolved | close |
| Change | hours posted | learn |

The complaint log is a mediation record and never a blacklist. The anonymity
and result columns are the design's conscience: the shelter wants to fix the
noise, not to know who complained, and the change column is how a week of
arguing becomes a posted quiet hour.

---

## 58. APPENDIX AS — REST ROUTINE TABLE

| Routine | For | Steps | Time |
|---|---|---|---|
| Wind-down | all | lamp low, wash, tea | 30m |
| No-screen hour | all | quiet reading | 60m |
| Warm drink | night shift | mug, small food | 15m |
| Stretch | light sleepers | slow movement | 10m |
| Dark room | day sleep | cover, seal, mask | 5m |
| Wake light | dawn | lamp, window | 15m |
| Nap kit | nappers | cot, mask, stop | — |
| Sleep log | keepers | hours, noise | 5m |

Routines are the part of rest that people actually control, and the table turns
them into ordinary content. The wind-down and wake-light rows matter most,
because a shelter that cannot darken a room or light a morning will have
insomnia regardless of its quiet hours.

---

## 59. APPENDIX AT — CROWDING REMEDY TABLE

| Remedy | Cost | Effect | Time | Best for |
|---|---|---|---|---|
| Redistribute | none | balances | days | tight |
| Screens | low | privacy | 1 day | crowded |
| Bunk tiers | med | density | 3 days | overcrowded |
| Tent space | low | overflow | 1 day | crisis |
| Build room | high | capacity | weeks | chronic |
| Rotate sleep | none | sharing | immediate | shift mismatch |
| Common room | low | offload | days | crowding |
| Store move | low | space | 1 day | clutter |

The remedy table is how the expansion keeps its promise that crowding is a
problem with solutions. Every row is or a real fix or a bridge to one, and none
of them asks tired people to simply tolerate less space.

---

## 60. APPENDIX AU — NIGHT COURTESY TABLE

| Courtesy | Who | When | Why |
|---|---|---|---|
| Soft close | all | quiet hours | sleep |
| Lamp hood | night reader | always | light |
| Step light | corridors | night | eyes |
| Kettle refill | last riser | night | next person |
| Boots off | quarters | night | noise |
| Voice low | corridors | night | respect |
| Door hold | all | shift change | sleep |
| Mug return | kitchen | morning | order |

Courtesy is the expansion's real enforcement mechanism. The table is short,
ordinary, and cheaper than any policy, and it is the thing that actually lets a
crowded shelter sleep without turning quiet into a rule imposed by force.

---

## 61. CLOSING STATEMENT

ASHFALL already measures noise, quiet hours, fatigue, crowding through
assignment, and social friction. What it lacks is rest as a lived system: sleep
schedules, quiet rooms, soundproofing, privacy, night rosters, and the records
that make fatigue visible before it becomes an error. The Quiet adds that
system without adding a second noise or needs owner and without ever shaming a
person for sleeping. It adds a rug that halves a hum, a door that closes, a
roster that respects the night, and a shelter that treats rest as the other half
of work.

> Wave 6 note: this plan is one of five Wave 6 expansion bibles (37–41). Each is
> self-contained; none requires another to ship. The shared Wave 6 index lives at
> `docs/expansions/wave6/WAVE6_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `ShelterNoiseSystem` (`NoiseSourceType` with Machinery, HumanActivity,
> IndustrialProcess, Alarm, Ventilation, Generator, Construction, MusicRecreation,
> and Argument; `NoiseSource` with `NoiseOutput`, `Frequency`, `DurationHours`,
> `IsActive`, `CanBeSoundproofed`; `RoomAcousticProfile` with `BaseNoiseLevel`,
> `WallSoundproofing`, `DoorSoundproofing`, `EffectiveNoiseLevel`;
> `ShelterNoiseState` with `OverallNoiseLevel` = 20f, `DetectionRisk` = 5f,
> `QuietHoursActive`, `QuietHoursStart` = 22, `QuietHoursEnd` = 6, and the
> `OnNoiseSpike`, `OnThreatDetectionRiskIncreased`, and `OnQuietHoursChanged`
> events), `NeedsSystem` (Fatigue, Numbness, Morale), `ShelterAssignmentSystem`,
> `ShelterSocialDynamicsSystem`, `ShelterDecorSystem`, and `CaregivingSystem`
> (`CaregiverFatigueDrain`).