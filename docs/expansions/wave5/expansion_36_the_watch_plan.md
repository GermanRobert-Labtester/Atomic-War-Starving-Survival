# ASHFALL — Expansion 36 Design Bible
# THE WATCH
### Wave 5 · Night Watch, Patrols, Perimeter, Detection, Gates, Territory, and Readiness

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-22
**Domain owners touched:** `Ashfall.Core.World` (PatrolTerritoryAuthority), `Ashfall.Core.Combat` (SoundRangingThreatEngine), `Ashfall.Core.Narrative` (PatrolEncounterValidator, NightWatchCatalog), `Ashfall.Core.Radio` (PatrolRadioHooks)
**Proposed host owner:** `WatchHouseHostSession` (extends watch, patrol, and territory surfaces)
**Existing save sections:** patrol territory state, night watch catalog state, radio signal state
**Existing CLI verbs:** `--territory-selftest` (if present), `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has eyes on the dark. `PatrolTerritoryAuthority` (9.7 KB)
defines `DynamicTerritoryAuthority` and `ITerritoryAuthority` with
`SetTerritory`, `GetTerritoryState`, `GetController`, `IsClaimant`, `IsClaimedBy`,
`SetNode`, and `RestoreState`, and it records `TerritoryNodeRecord` and
`DynamicTerritoryState`. `PatrolEncounterValidator` (16.4 KB) validates patrol
encounters against the world. `SoundRangingThreatEngine` (17 KB) runs
`SoundRangingStationState`, `AcousticSensorNode`, `HostileFireObservation`, and
`AcousticThreatEstimate`, with calibration drift and confirming observations.
`PatrolRadioHooks` and `WeatherGateRadioHooks` surface patrol and weather calls.
`NightWatchCatalog` is a small narrative catalog, and the narrative data carries
`night_watch_expansion.json`, `night_watch_logbook.json`, and
`patrol_debriefs.json`. `faction_territory.json` (20 KB) holds territory nodes
and controllers.

What does not exist: watch posts as places, patrol routes as content, perimeter
systems with real sensors and alarms, detection content beyond the threat
engine's inputs, gate protocol, incident logs and debriefs as playable records,
readiness levels and drills, and the night-shift welfare that keeps a watch
alive.

**The Watch** turns that machinery into the shelter's first line: the people who
stand in the dark, walk the line, hear the first wrong sound, and decide what to
do before anyone else is awake.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

Every shelter sleeps because someone is awake. The Watch is about the someone.

**The Watch** is the expansion about security as a daily practice: watch posts,
patrol routes, perimeter sensors and alarms, detection and verification, gate
protocol, territory claims, incident logs, debriefs, drills, and the fatigue
that comes with the night shift. It extends the live territory, patrol, and
acoustic-detection systems with authored posts, routes, alarms, protocols, and
consequences, and it hands every incident to the system that owns the response:
combat, rescue, medical, or civil policy.

The expansion's hard rules follow the live owners: `PatrolTerritoryAuthority`
keeps territory, `SoundRangingThreatEngine` keeps acoustic detection,
`PatrolEncounterValidator` keeps patrol encounter validation, combat keeps
combat, rescue keeps rescue, medical keeps care, and espionage keeps
intelligence. No second security, combat, or intelligence system is created.

### 1.2 The five loops it adds

```
   Post ──► Watch ──► Detect ──► Verify ──► Decide
     │         │          │          │          │
     ▼         ▼          ▼          ▼          ▼
   Towers,   Shifts,    Sound,     Eyes,      Alarm,
   gates,    fatigue    motion,    patrol     handoff,
   fencing              light                 response
                                                  │
                                                  ▼
                                          Incident ──► Debrief ──► Fix
                                                  │
                                                  ▼
                                          Territory ──► Claims, contests,
                                          patrols        agreements
```

### 1.3 What the player manages

1. **Posts.** Towers, gate posts, fence lines, listening points, and roving
   watches.
2. **Shifts.** Who stands watch, for how long, and with what relief.
3. **Detection.** Sound, motion, light, animals, and instrument readings.
4. **Verification.** Challenge, identify, and confirm before escalation.
5. **Alarm.** Levels, channels, and the chain from watcher to shelter.
6. **Response.** Handoffs to combat, rescue, medical, or civil policy.
7. **Gates.** Entry, exit, identification, curfew, and quarantine handoff.
8. **Territory.** Claims, patrols, contests, and agreements.
9. **Readiness.** Drills, readiness levels, and lessons from incidents.
10. **Welfare.** Night-shift fatigue, rotation, sleep, and dependency risk.

### 1.4 What it is not

- Not a second combat system. Combat stays with combat.
- Not a second intelligence or espionage system. Operations stay with The Quiet
  Hand.
- Not a second rescue or fire system. Dispatch stays with The Alarm.
- Not a second predator system. Wildlife stays with The Wild.
- Not a second road escort system. Route security stays with The Long Road.
- Not a repressive checkpoint sim; gates are protocol, not cruelty.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/World/PatrolTerritoryAuthority.cs` | Territory control and claims | `LIVE` |
| `Assets/Ashfall.Core/Narrative/PatrolEncounterValidator.cs` | Encounter validation | `LIVE` |
| `Assets/Ashfall.Core/Combat/SoundRangingThreatEngine.cs` | Acoustic sensors and estimates | `LIVE` |
| `Assets/Ashfall.Core/Narrative/NightWatchCatalog.cs` | Watch lore catalog | `LIVE` |
| `Assets/Ashfall.Core/Radio/PatrolRadioHooks.cs` | Patrol radio calls | `LIVE` |
| `Assets/Ashfall.Core/Radio/WeatherGateRadioHooks.cs` | Weather calls | `LIVE` |
| `Assets/Ashfall.Core/Combat/*` | Combat authority | `LIVE` |
| `Assets/StreamingAssets/Data/faction_territory.json` | Territory nodes | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `faction_territory.json` | 20 KB | nodes and controllers |
| `night_watch_expansion.json` | narrative | watch prose |
| `night_watch_logbook.json` | narrative | log entries |
| `patrol_debriefs.json` | narrative | debrief prose |
| Post / route / alarm / gate / drill data | none | confirmed absent |

### 2.3 Confirmed gaps

- **GAP-36-1 — No watch posts.** Towers, gates, and listening points are prose.
- **GAP-36-2 — No patrol routes.** Patrols have no authored content.
- **GAP-36-3 — No perimeter systems.** Fences, alarms, and lights are absent.
- **GAP-36-4 — No detection content** beyond raw engine inputs.
- **GAP-36-5 — No alarm protocol or alert chain.**
- **GAP-36-6 — No gate protocol.** Entry and exit are unmodeled.
- **GAP-36-7 — No incident logs or debriefs in play.**
- **GAP-36-8 — No readiness levels or drills.**
- **GAP-36-9 — No night-shift welfare content.** Fatigue is invisible.
- **GAP-36-10 — Territory data has no operational layer.**

### 2.4 Non-duplication statement

This expansion will **not** add a second territory, patrol, detection, combat,
rescue, medical, intelligence, or road-security system. It extends
`PatrolTerritoryAuthority` with operational claims and contests, extends the
sound-ranging engine with authored detection profiles, extends
`PatrolEncounterValidator` with content, and hands every response to its live
owner. It adds state only as additive sub-objects of the existing patrol and
radio stores. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Security is a shift, not an adventure.** The watch is boring on
purpose; the boredom is the service.

**Pillar 2 — Detection before force.** The first job is to see, hear, and
confirm; the second job belongs to someone else.

**Pillar 3 — Fatigue is the quiet enemy.** A tired watch misses things, and the
expansion treats sleep as a security system.

**Pillar 4 — Protocol protects everyone.** A gate that behaves predictably
protects visitors and residents alike.

**Pillar 5 — Debriefs make the next night safer.** Every incident produces a
lesson and a change.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A night watch | Cold, quiet, alert | Action fantasy |
| An alarm | Procedure and response | Chaos |
| A gate check | Polite protocol | Humiliation |
| A territory dispute | Argument and maps | War fantasy |
| A debrief | Honest review | Blame |
| A tired sentry | Relief and rotation | Punishment |

### 3.3 Content limits

- No reprisal of civilians, no flogging, no torture, no humiliation mechanics.
- Watch service never becomes a reward for violence or a punishment assignment.
- Territory contests are resolved by patrol, agreement, or the live combat
  system's rules — never by ethnic or cultural framing.
- Visitors and refugees are handled with dignity even when turned away.
- Night duty is depicted honestly including its cost, never glamorized.

---

## 4. THE WATCH WORLD

### 4.1 Interior rooms

- **`room_watch_room`** — duty board, logs, and coffee.
- **`room_gate_room`** — the entry post and the register.
- **`room_alarm_room`** — bells, rattle, radio, and the chain board.
- **`room_listening_post`** — quiet, instruments, and a chair.
- **`room_maps_room`** — patrol maps and pins.
- **`room_armory_anterior`** — lockers and checkout (combat owns the contents).
- **`room_briefing_room`** — debriefs and drills.
- **`room_rest_room`** — sleep for the night watch.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_gate_main` | The Main Gate | 4 | Entry and exit |
| `loc_west_fence` | The West Fence | 4 | Perimeter line |
| `loc_tower_north` | Tower North | 5 | Long sightline |
| `loc_listening_pit` | The Listening Pit | 4 | Acoustic post |
| `loc_dead_ground` | The Dead Ground | 5 | Blind spot and lesson |
| `loc_boundary_stone` | The Boundary Stone | 3 | Territory mark |
| `loc_patrol_cut` | The Cut | 4 | Patrol route shortcut |
| `loc_burned_outpost` | The Burned Outpost | 6 | Old post and cautionary tale |
| `loc_river_watch` | The River Watch | 4 | Crossing watch |
| `loc_visitor_line` | The Visitor Line | 3 | Where visitors wait |

All locations require valid item references and scanner registration.

### 4.3 The watch night

Handover at dusk, first round, listening watch, second round, pre-dawn check,
report at dawn. The expansion's clock is the night itself, and the debrief
closes it every morning.

---

## 5. MAIN STORYLINE — "WHO KEPT THE NIGHT"

### 5.1 Central conflict

The shelter's perimeter is habit. **Lowen Far** stands the same post every
night, nobody writes anything down, and the alarm is a bucket and a stick. When
an unknown party approaches the west fence and leaves without being challenged,
the shelter discovers that it has no protocol, no post, no log, and no idea what
it saw.

**Brann Cole**, the watch captain, wants a real system: posts, routes, alarms,
logs, and drills. **Auri Sen** the scout wants patrols that matter. **Renn** at
the listening post has a theory about the sound and no channel to report it. The
steward wants gates and rules; the medic wants night-shift welfare because the
watch is falling asleep at the post.

At the same time, a neighbouring claim on the river crossing is expanding, and
the shelter must decide whether territory is defended, negotiated, or shared —
without turning a boundary stone into a war.

The expansion's question: **who is awake, what did they see, and what did the
shelter do about it?**

### 5.2 Theme (unspoken)

**Sleep is a resource, and someone always pays for it.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_watch_captain_brann_cole` | Brann Cole | Watch captain | Posts, routes, and readiness |
| `npc_sentry_lowen_far` | Lowen Far | Sentry | The west fence and the missed party |
| `npc_scout_auri_sen` | Auri Sen | Scout | Patrol routes and territory reading |
| `npc_listener_renn` | Renn | Listener | Acoustic post and signal habits |
| `npc_handler_cora` | Cora | Dog handler | Companion patrols and alarms |
| `npc_child_dusk` | Dusk | Child | Night lessons and quiet courage |
| `npc_steward_iven` | Iven | Steward | Gate rules and civil policy |
| `npc_medic_wynn` | Wynn | Medic | Fatigue, sleep, and night health |

### 5.4 Story beats (15)

1. **The Approach.** A party reaches the west fence unchallenged.
2. **The Shrug.** The shelter realizes it has no record.
3. **The Post.** A real watch post is built and staffed.
4. **The Rounds.** Patrol routes are written and walked.
5. **The Listening Pit.** Renn's theory is tested.
6. **The Alarm.** A chain is designed and drilled.
7. **The Log.** The first night log is kept.
8. **The Gate.** Entry protocol is written.
9. **The Boundary.** A neighbouring claim is discovered.
10. **The Contest.** Territory is argued and mapped.
11. **The Dead Ground.** A blind spot is found the hard way.
12. **The Night Off.** Fatigue causes a failure; welfare is fixed.
13. **The Debrief.** Incident review becomes routine.
14. **The Winter Watch.** Cold, dark, and short shifts tested.
15. **Who Kept the Night.** Final disposition of the watch.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Watch size | small / standard / heavy | coverage vs. fatigue |
| Alarm | loud / quiet / layered | safety vs. disruption |
| Gates | open / posted / strict | access vs. control |
| Territory | defend / negotiate / share | reach vs. peace |
| Patrols | dense / sparse / reactive | presence vs. cost |
| Detection | instruments / senses / both | investment |
| Night welfare | rotate / fixed / volunteers | fairness |
| Final | watch as institution / post / memory | identity |

### 5.6 Endings (5 + fade)

1. **The Kept Perimeter** — posts, patrols, alarms, logs, and drills make the
   shelter genuinely hard to surprise.
2. **The Shared Boundary** — the territory dispute ends in a written agreement
   and a joint patrol.
3. **The Reliable Watch** — the system works because the roster respects sleep.
4. **The Loud Night** — a false alarm and a real incident teach the shelter
   what alarms are for.
5. **The Quiet Failure** — the watch is understaffed and something walks in.
6. **Fade** — a bucket and a stick still stand at the gate.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_watch_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_watch_approach`, `quest_watch_shrug`, `quest_watch_post`,
`quest_watch_rounds`, `quest_watch_listening_pit`, `quest_watch_alarm`,
`quest_watch_log`, `quest_watch_gate`, `quest_watch_boundary`,
`quest_watch_contest`, `quest_watch_dead_ground`, `quest_watch_night_off`,
`quest_watch_debrief`, `quest_watch_winter_watch`, `quest_watch_who_kept_night`.

### 6.2 Side quests (30)

**Posts (5)**
- `quest_watch_post_build` — build a post
- `quest_watch_post_staff` — staff a post
- `quest_watch_post_repair` — repair a post
- `quest_watch_listening_post` — set a listening post
- `quest_watch_fence_line` — walk and fix the fence

**Patrols (5)**
- `quest_watch_route_write` — write a patrol route
- `quest_watch_route_walk` — walk a route
- `quest_watch_route_change` — revise after an incident
- `quest_watch_night_patrol` — night patrol
- `quest_watch_dawn_report` — dawn report

**Detection (5)**
- `quest_watch_sound_map` — map ambient sound
- `quest_watch_calibrate` — calibrate sensors
- `quest_watch_motion_line` — set trip lines
- `quest_watch_light_line` — set lights
- `quest_watch_animal_alarm` — use animals as alarms

**Gates (5)**
- `quest_watch_gate_protocol` — write entry rules
- `quest_watch_register` — keep the gate register
- `quest_watch_visitor_line` — manage visitors
- `quest_watch_curfew` — set and review curfew
- `quest_watch_quarantine_handoff` — handoff to medical

**Territory (5)**
- `quest_watch_mark_boundary` — mark the boundary
- `quest_watch_map_claims` — map claims
- `quest_watch_scout_claim` — scout a rival claim
- `quest_watch_negotiate` — negotiate passage
- `quest_watch_joint_patrol` — joint patrol

**Readiness (5)**
- `quest_watch_drill_intruder` — intruder drill
- `quest_watch_drill_alarm` — alarm drill
- `quest_watch_drill_blackout` — blackout drill
- `quest_watch_night_welfare` — fix the roster
- `quest_watch_review` — monthly review

### 6.3 Repeatable quests (8)

`quest_watch_repeat_shift`, `quest_watch_repeat_patrol`,
`quest_watch_repeat_log`, `quest_watch_repeat_calibrate`,
`quest_watch_repeat_drill`, `quest_watch_repeat_gate`,
`quest_watch_repeat_debrief`, `quest_watch_repeat_rounds`.

### 6.4 Dynamic hooks

Live events (patrol encounters, territory changes, acoustic estimates, radio
calls, weather gates, predator activity, road arrivals) attach authored
follow-ups through existing seams. No new event bus.

### 6.5 Constraints

- Territory remains with `PatrolTerritoryAuthority`.
- Patrol encounters remain validated by `PatrolEncounterValidator`.
- Acoustic detection remains in `SoundRangingThreatEngine`.
- Force stays with combat; rescue with The Alarm; care with medical.
- Intelligence stays with The Quiet Hand.
- Route security stays with The Long Road.
- Gates are protocol, never humiliation.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `WatchPostSystem` (new, `Ashfall.Core.World`)

**Owns:** posts, towers, gates, fence lines, listening points, and their
condition and coverage. **Consumes:** `PatrolTerritoryAuthority`,
`Inventory`, `BuildWorksSystem` (Wave 4), `NeedsSystem`.
**Data:** `watch_posts.json`. **Rules:** a post needs staff, light, sightlines,
and relief; coverage is a real map, not a bonus.

### 7.2 `PatrolRouteSystem` (new, `Ashfall.Core.World`)

**Owns:** routes, schedules, debriefs, and encounter context. **Consumes:**
`PatrolEncounterValidator`, `PatrolTerritoryAuthority`, `DutyRoster`,
`NeedsSystem`, route infrastructure. **Data:** `patrol_routes.json`.
**Rules:** routes are walked, logged, and revised; a route that is never walked
protects nothing.

### 7.3 `PerimeterSystem` (new, `Ashfall.Core.World`)

**Owns:** fences, trip lines, alarms, lights, and rattle systems. **Consumes:**
`Inventory`, `PowerGridSystem`, `WatchPostSystem`, `BuildWorksSystem`.
**Data:** `perimeter_systems.json`. **Rules:** every sensor has a failure mode
and a maintenance cost; false alarms are real and teach calibration.

### 7.4 `DetectionSystem` (new, `Ashfall.Core.World`)

**Owns:** detection profiles, verification steps, and confidence. **Consumes:**
`SoundRangingThreatEngine`, `PerimeterSystem`, `WatchPostSystem`,
`CompanionAnimalSystem` (scent/guard animals). **Data:**
`detection_profiles.json`. **Rules:** detection produces a bearing, a
confidence, and a verification requirement; it never produces an enemy.

### 7.5 `AlarmSystem` (new, thin, `Ashfall.Core.World`)

**Owns:** alarm levels, channels, chains, and drills. **Consumes:**
`DetectionSystem`, `PatrolRadioHooks`, `NoticeSystem` (Wave 4), `DutyRoster`.
**Data:** `alarm_protocols.json`. **Rules:** alarms are levels with defined
responses; a level means the same thing every night; drills prove it.

### 7.6 `GateProtocolSystem` (new, thin, `Ashfall.Core.World`)

**Owns:** entry and exit protocol, registers, curfew, visitor handling, and
handoffs. **Consumes:** `NoticeSystem`, `DiseaseQuarantineCoordinator` (Wave 2),
`PolicySystem`, `TradingSystem` (arrivals only). **Data:** `gate_rules.json`.
**Rules:** protocol is predictable and dignified; visitors are registered, not
humiliated; medical handoff is automatic where required.

### 7.7 `TerritoryOpsSystem` (extend `PatrolTerritoryAuthority`)

**Owns:** claims, contests, agreements, boundary marks, and joint patrols.
**Consumes:** live territory authority, `StandingRecord` (Exp 03),
`PassageAgreementSystem` (Wave 5), `PatrolRouteSystem`.
**Data:** `territory_claims.json`. **Rules:** claims are map objects with
controllers and contested status; contests resolve through patrol presence,
agreement, or the live combat rules; no ethnic or cultural framing.

### 7.8 `WatchReadinessSystem` (new, thin, `Ashfall.Core.World`)

**Owns:** readiness levels, drills, incident reviews, and lessons. **Consumes:**
`IncidentLog`, `AlarmSystem`, `DutyRoster`, `SchoolingSystem` (training).
**Data:** `readiness_drills.json`. **Rules:** readiness rises with drills and
falls with missed shifts; every incident produces a lesson the shelter can
implement.

### 7.9 Systems explicitly not added

- No second combat, rescue, medical, intelligence, or route-security system.
- No repressive checkpoint mechanics or humiliation content.
- No ethnic or cultural territory framing.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `watch_posts.json` (new)

```json
{
  "schema_version": 1,
  "posts": [
    {
      "post_id": "post_west_fence",
      "display_name": "West Fence Point",
      "node_id": "loc_west_fence",
      "sightlines": ["west", "southwest"],
      "shelter_from": "wind",
      "capacity": 1,
      "requires": ["lamp", "log"],
      "relief_hours": 4,
      "tags": ["fence", "night", "exposed"]
    }
  ]
}
```

### 8.2 `patrol_routes.json` (new)

Routes: waypoints, distance, hours, coverage, danger, and debrief rules.

### 8.3 `perimeter_systems.json` (new)

Systems: fence, line, light, rattle, alarm, with cost, upkeep, failure, and
detection chance.

### 8.4 `detection_profiles.json` (new)

Profiles: sound, motion, light, scent, and vision, with confidence and
verification steps.

### 8.5 `alarm_protocols.json` (new)

Protocols: level, trigger, channels, response, and drill.

### 8.6 `gate_rules.json` (new)

Rules: entry, exit, visitor, curfew, quarantine handoff, and register.

### 8.7 `territory_claims.json` (new)

Claims: node, controller, contested, boundary marks, and agreement status.

### 8.8 `incident_log.json` (new)

Entries: night, post, detection, verification, action, handoff, and lesson.

### 8.9 `readiness_drills.json` (new)

Drills: type, participants, time target, failure modes, and readiness effect.

### 8.10 `watch_roster.json` (new)

Roster: post, shift, survivor, fatigue, relief, and rotation rule.

### 8.11 Items

New items appended to `items.json`: `item_watch_bell`, `item_signal_horn`,
`item_trip_wire`, `item_road_flare` (shared), `item_lantern`,
`item_listening_cup`, `item_watch_log`, `item_duty_sash`, `item_gate_seal`,
`item_challenge_token`, `item_chalk_mark`, `item_parapet_sandbag`,
`item_alarm_rattle`, `item_night_glass` (optics tie), `item_watch_map`,
`item_relief_whistle`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Patrol territory state and the watch catalog remain the live save owners. New
sub-objects (posts, routes, perimeter, detection, alarms, gates, claims,
incidents, drills, rosters) are additive inside them. No new save section.

### 9.2 State to persist

- Post construction, staff, and condition.
- Patrol routes and completed rounds.
- Perimeter systems and maintenance.
- Detection records and calibration.
- Alarm levels and drill history.
- Gate registers and curfew status.
- Territory claims and agreements.
- Incident logs and lessons.

### 9.3 Determinism

- Territory control uses `PatrolTerritoryAuthority`.
- Detection uses `SoundRangingThreatEngine` with its calibration and confirming
  observations.
- Patrol encounters validate through `PatrolEncounterValidator`.
- Fatigue and rotation resolve deterministically from the live roster.
- Any randomness uses the live seeded paths.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with existing territory and radio state untouched; no post,
route, perimeter, alarm, gate, claim, incident, or drill state exists until
started. Existing patrol encounters and territory controllers keep working.

### 9.5 Checksum

Invariant-culture floats; integer counts for rounds, nights, and confirmed
observations.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `WatchBoardPanel` (new) | Posts, coverage, roster | `WatchHouseHostSession` |
| `PatrolPanel` (new) | Routes, rounds, debriefs | same |
| `PerimeterPanel` (new) | Fences, alarms, lights | same |
| `DetectionPanel` (new) | Contacts, confidence, verification | same |
| `AlarmPanel` (new) | Levels and drill status | same |
| `GatePanel` (new) | Registers and protocol | same |
| `TerritoryPanel` (new) | Claims and agreements | same |
| `IncidentPanel` (new) | Logs and lessons | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Contacts display as bearing, confidence, and verification need — never as
  enemies on a minimap.
- Fatigue is shown on the roster, not hidden in a morale number.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Alarms have visual equivalents; no audio-only alerting.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a bell, a rattle, boots on gravel, a
challenge whistle, a gate opening, a logbook closing at dawn. No cue is
required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `PatrolTerritoryAuthority` | Territory claims and control |
| `PatrolEncounterValidator` | Patrol encounter validation |
| `SoundRangingThreatEngine` | Acoustic detection |
| `NightWatchCatalog` | Watch prose |
| `PatrolRadioHooks` | Patrol calls |
| `Combat` | Force response after verification |
| `DiseaseQuarantineCoordinator` (Wave 2) | Gate medical handoff |
| `WeatherGate` (Wave 5) | Weather alerts |
| `CompanionAnimalSystem` (Wave 5) | Guard and scent animals |
| `SoundRangingThreatEngine` | Sensor calibration |
| `PowerGridSystem` | Lights and alarms |
| `BuildWorksSystem` (Wave 4) | Post and fence construction |
| `DutyRoster` (Exp 02) | Watch shifts |
| `NeedsSystem` | Fatigue and safety |
| `NoticeSystem` (Wave 4) | Gate and curfew notices |
| `SchoolingSystem` (Wave 4) | Watch training |
| `EpilogueChronicleBuilder` | Watch milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `PatrolTerritoryAuthority`,
`PatrolEncounterValidator`, `SoundRangingThreatEngine`, `NightWatchCatalog`,
patrol radio hooks, faction territory data, and narrative watch files. Record
file:line; change nothing.

**Phase 1 — Data + validators.** Author posts, routes, perimeter, detection,
alarms, gates, claims, incidents, drills, rosters; append items. Register
validators and scanner.

**Phase 2 — Pure Core.** `WatchPostSystem`, `PatrolRouteSystem`,
`PerimeterSystem`, `DetectionSystem`, `AlarmSystem`, `GateProtocolSystem`,
`TerritoryOpsSystem`, `WatchReadinessSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `WatchHouseHostSession`, selftest coverage, fresh
journey.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak: fatigue, false alarms, incidents, drills,
and territory contests.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Watch posts | 15 |
| Patrol routes | 12 |
| Perimeter systems | 12 |
| Detection profiles | 12 |
| Alarm protocols | 8 |
| Gate rules | 10 |
| Territory claims | 10 |
| Incident entries | 40 |
| Drills | 10 |
| Rosters | 6 |
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
| Second combat/intel system | Critical | Handoffs to live owners |
| Repressive gate tone | Critical | Dignity protocol |
| Territory as ethnic framing | Critical | Map-and-agreement only |
| Fatigue ignored | High | Roster and welfare |
| Detection as enemy radar | High | Bearing and confidence only |
| Alarm spam | Medium | Levels and drills |
| Determinism break | Low | Live seeded paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `watch_posts.json` | 15 | 4,000 |
| `patrol_routes.json` | 12 | 3,500 |
| `perimeter_systems.json` | 12 | 3,000 |
| `detection_profiles.json` | 12 | 3,000 |
| `alarm_protocols.json` | 8 | 2,500 |
| `gate_rules.json` | 10 | 2,500 |
| `territory_claims.json` | 10 | 3,000 |
| `incident_log.json` | 40 | 6,000 |
| `readiness_drills.json` | 10 | 3,000 |
| `watch_roster.json` | 6 | 2,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~63,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R36-1 | Second combat system | Low | Critical | Force stays combat |
| R36-2 | Second intel system | Low | Critical | Quiet Hand untouched |
| R36-3 | Repressive gates | Med | Critical | Dignity protocol |
| R36-4 | Territory ethno-framing | Low | Critical | Map and agreement |
| R36-5 | Fatigue invisible | Med | High | Roster and welfare |
| R36-6 | Radar-like detection | Med | High | Bearing and confidence |
| R36-7 | Alarm fatigue | Med | Med | Levels and drills |
| R36-8 | Determinism | Low | High | Live seeded paths |
| R36-9 | Content overrun | Med | Med | Budget §13 |
| R36-10 | Watch as punishment | Low | Med | Service framing |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can the alarm be false?** Recommended: yes, with calibration and drills
   reducing but never eliminating false alarms.
2. **Do gates ever refuse entry, and on what grounds?** Recommended: yes, on
   medical quarantine and safety grounds, always with dignity and appeal.
3. **Is territory ever taken by force?** Recommended: only through the live
   combat rules, and never through this expansion's mechanics.
4. **Does night duty create dependency risk?** Recommended: yes, as a stress
   source feeding the live dependency system.
5. **Are watch posts destroyable and rebuildable?** Recommended: yes, with
   authored damage.

---

## 17. APPENDIX D — WATCH POST TABLE (15 POSTS)

| # | Post | Node | Sightlines | Capacity | Relief | Exposed |
|---|---|---|---|---|---|---|
| 1 | West Fence | west fence | W, SW | 1 | 4h | wind |
| 2 | Main Gate | gate | S, SE | 2 | 6h | visitors |
| 3 | Tower North | tower | N, NE, E | 2 | 6h | height |
| 4 | Listening Pit | pit | acoustic | 1 | 4h | damp |
| 5 | River Watch | river | crossing | 1 | 4h | water |
| 6 | East Fence | east line | E | 1 | 4h | open |
| 7 | South Gate | gate | S | 1 | 6h | traffic |
| 8 | Dead Ground | blind spot | short | 1 | 3h | unsafe |
| 9 | Roof Watch | shelter | all | 1 | 4h | cold |
| 10 | Visitor Line | visitor | approach | 1 | 6h | crowd |
| 11 | Boundary Stone | marker | W | 1 | 4h | exposure |
| 12 | Cut Patrol | cut | route | 1 | 4h | brush |
| 13 | Burned Outpost | ruins | N | 1 | 4h | story |
| 14 | Kennel Post | yard | animals | 1 | 6h | noise |
| 15 | Signal Ridge | ridge | long | 1 | 4h | wind |

Posts are places with sightlines, shelter, capacity, and relief rules. Coverage
is a map the player can read, not a hidden security number.

---

## 18. APPENDIX E — PATROL ROUTE TABLE (12 ROUTES)

| # | Route | Waypoints | Distance | Hours | Coverage | Danger |
|---|---|---|---|---|---|---|
| 1 | Inner Ring | shelter loop | 1.2 km | 1.5 | core | low |
| 2 | Outer Ring | fence line | 3.5 km | 3 | perimeter | med |
| 3 | West Line | west posts | 2 km | 2 | west | med |
| 4 | East Line | east posts | 2 km | 2 | east | med |
| 5 | River Line | river watch | 2.5 km | 2.5 | crossing | med |
| 6 | Boundary Walk | stone | 4 km | 3.5 | claim | high |
| 7 | Cut Route | cut | 1.5 km | 1.5 | shortcut | med |
| 8 | Ruins Check | burned outpost | 3 km | 3 | north | high |
| 9 | Visitor Line | gate | 1 km | 1 | access | low |
| 10 | Night Ring | shelter loop | 1.2 km | 1.5 | core | low |
| 11 | Dawn Line | fence line | 2.5 km | 2 | perimeter | low |
| 12 | Joint Line | boundary | 4 km | 3.5 | shared | med |

Routes are walked, timed, logged, and revised. A route nobody walks is a line
on a map, and the expansion makes the difference visible in the log.

---

## 19. APPENDIX F — PERIMETER SYSTEM TABLE (12 SYSTEMS)

| # | System | Cost | Upkeep | Detection | Failure |
|---|---|---|---|---|---|
| 1 | Wood Fence | med | seasonal | low | rot |
| 2 | Wire Line | low | yearly | med | cut |
| 3 | Trip Wire | low | weekly | med | snap |
| 4 | Rattle Can | low | weekly | low | wind |
| 5 | Bell Line | low | monthly | med | tangle |
| 6 | Lamp Line | med | fuel | low | wind |
| 7 | Watch Lamp | low | fuel | low | burn |
| 8 | Acoustic Node | high | calibration | high | drift |
| 9 | Foot Plate | med | monthly | med | freeze |
| 10 | Scent Line | low | refresh | med | rain |
| 11 | Animal Alarm | med | feed | high | animal |
| 12 | Signal Mirror | low | polish | low | cloud |

Every sensor has a failure mode and an upkeep cost. The shelter's perimeter is
maintained like any other infrastructure, and false alarms are part of the bill.

---

## 20. APPENDIX G — DETECTION PROFILE TABLE (12 PROFILES)

| # | Profile | Channel | Confidence | Verification |
|---|---|---|---|---|
| 1 | Footfall | sound | med | listen and watch |
| 2 | Voices | sound | high | count and bearing |
| 3 | Engine | sound | high | identify direction |
| 4 | Metal | sound | med | locate |
| 5 | Animal | sound, sight | med | species check |
| 6 | Motion | sight | med | eyes-on |
| 7 | Silhouette | sight | low | challenge |
| 8 | Light | sight | med | source check |
| 9 | Smoke | sight | high | origin check |
| 10 | Scent | smell | low | animal confirm |
| 11 | Trip | contact | high | patrol check |
| 12 | Absence | any | low | investigate |

Detection produces a bearing, a confidence, and a required verification step.
It never declares a hostile; it declares that something is out there, and the
watch decides what to do about it.

---

## 21. APPENDIX H — ALARM PROTOCOL TABLE (8 PROTOCOLS)

| # | Level | Trigger | Channels | Response | Drill |
|---|---|---|---|---|---|
| 1 | Note | low confidence | log only | observe | monthly |
| 2 | Watch | med confidence | runner | post alert | monthly |
| 3 | Alert | confirmed contact | bell, radio | posts manned | quarterly |
| 4 | Call | contact at fence | bell, siren | captain on deck | quarterly |
| 5 | Lockdown | breach | siren, doors | shelter closes | half-yearly |
| 6 | Medical | injury | runner, bell | medic called | quarterly |
| 7 | Fire | fire | fire drill (23) | evacuation | quarterly |
| 8 | All Clear | resolved | bell, board | stand down | after |

Alarms are levels with fixed meanings and defined responses. The expansion's
rule is that a bell means exactly one thing every time it rings.

---

## 22. APPENDIX I — GATE RULE TABLE (10 RULES)

| # | Rule | Applies to | Evidence | Appeal |
|---|---|---|---|---|
| 1 | Register entry | all visitors | name, purpose | steward |
| 2 | Escort rule | unknown parties | escort present | steward |
| 3 | Curfew | residents | time | steward |
| 4 | Weapons check | visitors | declared | steward |
| 5 | Trade entry | traders | manifest | steward |
| 6 | Medical hold | sick arrivals | medic | medic |
| 7 | Quarantine | outbreak | coordinator | medic |
| 8 | Emergency entry | any | humanitarian | captain |
| 9 | Exit log | residents | time, destination | steward |
| 10 | Night lock | all | time | captain |

Gate rules are printed, posted, and applied predictably. Every refusal has an
evidence requirement and an appeal path, which is the difference between a gate
and a wall.

---

## 23. APPENDIX J — TERRITORY CLAIM TABLE (10 CLAIMS)

| # | Claim | Node | Controller | Contested | Agreement |
|---|---|---|---|---|---|
| 1 | Home Perimeter | shelter | shelter | no | internal |
| 2 | West Fields | fields | shelter | no | internal |
| 3 | River Crossing | river | rival | yes | none |
| 4 | Boundary Stone | stone | shelter | yes | pending |
| 5 | Old Highway | highway | shared | no | passage |
| 6 | Cut Route | cut | shelter | no | internal |
| 7 | Ruins North | ruins | none | no | open |
| 8 | Ash Flats | flats | rival | yes | trade |
| 9 | Joint Line | boundary | shared | no | joint patrol |
| 10 | Visitor Line | gate | shelter | no | protocol |

Territory is claimed on a map, controlled by patrol presence, and resolved by
agreement or the live combat rules. The expansion never frames a claim as a
cultural or ethnic right; it is a map and a conversation.

---

## 24. APPENDIX K — INCIDENT LOG TABLE (40 ENTRIES — REPRESENTATIVE)

| # | Entry | Detection | Action | Lesson |
|---|---|---|---|---|
| 1 | Quiet night | none | logged | none |
| 2 | Animal sound | sound | listened | species habit |
| 3 | Wind rattle | contact | checked | false alarm |
| 4 | Footfall west | sound | watched | regular route |
| 5 | Light south | sight | checked | lamp line |
| 6 | Voices far | sound | logged | range limit |
| 7 | Trip line | contact | patrol | fox |
| 8 | Engine east | sound | posted | visitor |
| 9 | Silhouette | sight | challenged | trader |
| 10 | Smoke north | sight | investigated | camp |
| 11 | Scent line | smell | animal check | wolf |
| 12 | Absence | any | investigated | empty |
| 13 | Fence cut | patrol | repaired | weak point |
| 14 | Gate queue | routine | registered | traffic |
| 15 | Sick arrival | medical | held | quarantine |
| 16 | Curfew breach | routine | warned | policy |
| 17 | Weapons found | gate | declared | rule works |
| 18 | Night approach | sound | challenged | lost traveler |
| 19 | Boundary marker | patrol | confirmed | claim |
| 20 | Rival patrol | sight | observed | no contact |
| 21 | Rival approach | sight | challenged | talked |
| 22 | Claim dispute | routine | escalated | steward |
| 23 | False alarm | contact | stood down | calibration |
| 24 | Missed post | roster | covered | fatigue |
| 25 | Sleeping sentry | routine | relieved | welfare |
| 26 | Dead ground | routine | patrolled | blind spot |
| 27 | Alarm failure | drill | repaired | test needed |
| 28 | Radio call | radio | logged | channel |
| 29 | Weather call | radio | posted | storm |
| 30 | Blackout drill | drill | passed | readiness |
| 31 | Intruder drill | drill | passed | readiness |
| 32 | Joint patrol | agreement | logged | trust |
| 33 | Passage granted | agreement | escorted | protocol |
| 34 | Passage refused | agreement | escorted out | appeal |
| 35 | Medical handoff | medical | recorded | care |
| 36 | Fire call | fire | handed off | alarm |
| 37 | Rescue call | rescue | handed off | alarm |
| 38 | Broken alarm | patrol | replaced | upkeep |
| 39 | Quiet month | none | logged | none |
| 40 | Year review | review | published | lessons |

Incident logs are the watch's memory. Most entries are boring, a few are
decisive, and the boring ones are what make the decisive ones readable.

---

## 25. APPENDIX L — READINESS DRILL TABLE (10 DRILLS)

| # | Drill | Participants | Time target | Failure mode |
|---|---|---|---|---|
| 1 | Alarm bell | all | 3 min | slow spread |
| 2 | Post manning | watch | 5 min | missed post |
| 3 | Gate lock | gate crew | 2 min | stuck bar |
| 4 | Intruder call | watch | 5 min | confusion |
| 5 | Blackout | all | 10 min | lost lamps |
| 6 | Fire handoff | watch, fire | 5 min | wrong call |
| 7 | Medical handoff | watch, medic | 5 min | delay |
| 8 | Evacuation | all | 15 min | stragglers |
| 9 | Patrol recall | patrols | 10 min | range |
| 10 | Joint drill | neighbours | 30 min | comms |

Drills turn the roster into a system. Each one has a time target and a recorded
failure mode so the next drill can fix the last mistake.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_watch_approach` | 4 | Party reaches fence unobserved |
| `quest_watch_shrug` | 4 | No record, no protocol |
| `quest_watch_post` | 5 | Build and staff a post |
| `quest_watch_rounds` | 4 | Write and walk routes |
| `quest_watch_listening_pit` | 4 | Test the acoustic theory |
| `quest_watch_alarm` | 5 | Design and drill the chain |
| `quest_watch_log` | 4 | Keep the night log |
| `quest_watch_gate` | 4 | Write entry protocol |
| `quest_watch_boundary` | 4 | Discover the rival claim |
| `quest_watch_contest` | 5 | Argue and map the claim |
| `quest_watch_dead_ground` | 4 | Find the blind spot |
| `quest_watch_night_off` | 5 | Fatigue failure and welfare fix |
| `quest_watch_debrief` | 4 | Routine reviews |
| `quest_watch_winter_watch` | 5 | Cold-weather shifts |
| `quest_watch_who_kept_night` | 3 | Final disposition |

---

## 27. APPENDIX N — NPC DOSSIERS (BRIEF)

**Brann Cole** — watch captain. Wants a system where a tired person can still
succeed. Believes protocol is a kindness to the frightened.

**Lowen Far** — sentry. Stood the same post every night for two years and owns
the miss that changed everything. Wants training and relief, not forgiveness.

**Auri Sen** — scout. Reads ground and habit and comes back with maps that make
patrols useful. Prefers the boundary walk to any indoor work.

**Renn** — listener. Sits in the pit with a cup against the wall and can tell a
deer from a person from a cart. Vindicated slowly and enjoying it quietly.

**Cora** — dog handler. Runs scent and guard lines with animals the shelter
knows by name and can read the difference between alert and noise.

**Dusk** — child. Stands the early evening post with an adult because children
can see in the dim and like the quiet, and learns what watching means.

**Iven** — steward. Writes gate rules that are strict enough to matter and fair
enough to keep. Keeps the appeal log and reads it monthly.

**Wynn** — medic. Treats sentries for cold, exhaustion, and the rabbit-startle
of night work and fights for the rotation that keeps them alive.

---

## 28. APPENDIX O — LOCATION DETAIL

- **The Main Gate** — register, barrier, and the visitor line.
- **The West Fence** — the stretch that failed once and is watched hardest.
- **Tower North** — the best sightline and the coldest post.
- **The Listening Pit** — a scraped hollow with a seat and a cup.
- **The Dead Ground** — a dip that hides a person from every post, and a lesson.
- **The Boundary Stone** — an old marker with a new argument attached.
- **The Cut** — a brush shortcut that saves three minutes and hides everything.
- **The Burned Outpost** — a post that was lost and is now a training site.
- **The River Watch** — a crossing that decides who visits.
- **The Visitor Line** — a rope, a bench, and a polite wait.

---

## 29. APPENDIX P — DETECTION AND VERIFICATION MODEL

| Confidence | Meaning | Required step |
|---|---|---|
| Low | something is possible | listen and watch |
| Medium | something is likely | eyes-on or patrol check |
| High | something is present | challenge or alert |
| Confirmed | identity known | respond per protocol |

| Verification | Method | Time |
|---|---|---|
| Listen | cup, quiet | 1 min |
| Watch | eyes, glass | 2 min |
| Challenge | whistle, call | 1 min |
| Patrol check | walk out | 5–15 min |
| Instrument | sensor reading | 1 min |
| Animal | dog, goose | 2 min |

Detection ends at verification, and verification ends at protocol. The
expansion's watch is a chain of small, human confirmations rather than a
radar screen.

---

## 30. APPENDIX Q — FATIGUE AND NIGHT WELFARE MODEL

| Fatigue | Symptoms | Risk | Fix |
|---|---|---|---|
| Fresh | none | low | none |
| Tired | slow checks | med | rotate |
| Drowsy | missed cues | high | sleep, relief |
| Exhausted | sleeping on post | very high | stand down |
| Danger | collapsed | extreme | medical |

| Rotation | Hours | Recovery | Notes |
|---|---|---|---|
| 4 on / 8 off | 4 | full | best |
| 6 on / 6 off | 6 | partial | common |
| 8 on / 8 off | 8 | poor | tolerated |
| 12 on / 12 off | 12 | bad | emergency |
| Fixed nights | varies | poor | unpopular |
| Volunteers | varies | fair | respected |

Fatigue is the expansion's invisible antagonist. The roster is the weapon
against it, and the medic is the person who proves the need.

---

## 31. APPENDIX R — TERRITORY MODEL

| State | Meaning | Shelter action |
|---|---|---|
| Controlled | ours, patrolled | maintain |
| Shared | agreement | joint patrol |
| Contested | claim disputed | patrol, negotiate |
| Rival | other's claim | observe, respect |
| Open | unclaimed | scout, decide |
| Forbidden | hazard | avoid |

| Resolution | Cost | Outcome |
|---|---|---|
| Presence | patrol hours | de facto control |
| Agreement | negotiation | written sharing |
| Trade | goods | access for resources |
| Combat | live rules | winner takes claim |
| Withdrawal | prestige | claim released |

Territory is resolved on a map, in a room, or under the live combat rules —
never through a separate conquest system. The expansion keeps the boundary a
conversation first.

---

## 32. APPENDIX S — WORKED 360-DAY WATCH SCENARIO

**Days 1–20.** Approach missed; no record; Brann proposes a system.

**Days 21–60.** First post built; routes written; logs kept; the first false
alarm occurs and is recorded as calibration data.

**Days 61–100.** Listening pit established; Renn's sound map catches a cart the
posts had missed; the alarm chain is designed.

**Days 101–150.** Gate protocol written; visitor line established; one refusal
escalated and appealed; the register stays accurate.

**Days 151–210.** Rival claim found at the river crossing; patrols map it; a
contested month follows without a single hostile action.

**Days 211–260.** Dead ground found the hard way; posts repositioned; a drill
series raises readiness across the roster.

**Days 261–310.** Fatigue failure: a sentry sleeps, an incident occurs at the
fence, and the roster is rebuilt around four-hour reliefs.

**Days 311–360.** Winter watch with cold shifts, short nights, and full logs;
the joint patrol agreement is signed; the year's debrief is published.

---

## 33. APPENDIX T — VIGNETTE (TONE SAMPLE)

> Lowen walks the west fence at two in the morning and stops at the same gap
> every night and listens, and the listening is not ceremony; it is the only
> reason the gap is safe.

> Renn sets the cup against the wall and closes one eye and counts, and the
> sound is a cart and not a person, and the difference is what Renn has been
> trying to tell the shelter for a month.

> At the gate, Iven writes a name and a purpose and a time, and the traveler
> from the ferry reads the posted rules and nods, because rules that are posted
> are rules a person can argue with, and arguing is better than wondering.

---

## 34. APPENDIX U — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Missed approach | unknown contact | posts, routes |
| Sleeping sentry | breach risk | roster, welfare |
| False alarm | fatigue | calibration |
| Alarm failure | silence | test, repair |
| Blind spot | surprise | reposition posts |
| Gate breach | incident | protocol, drill |
| Territory loss | access loss | negotiate, patrol |
| Radio silence | no call | channel drill |
| Log loss | memory loss | rebuild, format |
| Fatigue collapse | medical | stand down, care |

No failure is a game over. The deepest failure is a shelter that never writes
down what happened on a night, and therefore learns nothing from any of them.

---

## 35. APPENDIX V — CONTENT REVIEW CHECKLIST

- [ ] `PatrolTerritoryAuthority` remains the territory authority.
- [ ] `PatrolEncounterValidator` keeps encounter validation.
- [ ] `SoundRangingThreatEngine` keeps acoustic detection.
- [ ] Combat, rescue, medical, and intelligence stay with their owners.
- [ ] Detection shows bearing and confidence, never enemies.
- [ ] Gates are predictable and dignified, with appeals.
- [ ] Territory is map-and-agreement, never ethnic framing.
- [ ] Fatigue is visible and fixable.
- [ ] No repressive or humiliating content exists.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded paths only.

---

## 36. APPENDIX W — GLOSSARY

- **Post** — a staffed watch position with sightlines.
- **Route** — a walked patrol path with waypoints.
- **Coverage** — what posts and routes actually see.
- **Detection** — a bearing and confidence from a sensor or sense.
- **Verification** — the human step that confirms detection.
- **Alarm level** — a fixed meaning with a defined response.
- **Gate protocol** — the written entry and exit rules.
- **Claim** — a territory node with a controller.
- **Contested** — a disputed claim.
- **Readiness** — drill-verified response capability.

---

## 37. APPENDIX X — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `PatrolTerritoryAuthority` | claims | control | posts |
| `TerritoryOpsSystem` | authority | agreements | control |
| `PatrolRouteSystem` | routes | rounds, logs | encounters |
| `PatrolEncounterValidator` | world | validation | routes |
| `SoundRangingThreatEngine` | sensors | estimates | combat |
| `DetectionSystem` | engine | profiles, confidence | combat |
| `WatchPostSystem` | posts | coverage | territory |
| `PerimeterSystem` | inventory | sensor state | detection |
| `AlarmSystem` | detection | levels | response |
| `GateProtocolSystem` | policy | registers | medical |
| `WatchReadinessSystem` | drills | readiness | roster |
| `Combat` | calls | force | watch |
| `DiseaseQuarantineCoordinator` | arrivals | holds | gates |
| `DutyRoster` | roster | shifts | fatigue |
| `NeedsSystem` | fatigue | morale | roster |
| `EpilogueChronicleBuilder` | milestones | chronicle | — |

---

## 38. APPENDIX Y — DATA SCHEMA DETAIL (NEW CATALOGS)

**`watch_posts.json`** — `post_id`, `display_name`, `node_id`, `sightlines[]`,
`shelter_from`, `capacity`, `requires[]`, `relief_hours`, `tags`.

**`patrol_routes.json`** — `route_id`, `display_name`, `waypoints[]`,
`distance_km`, `hours`, `coverage`, `danger`, `debrief_rule`, `tags`.

**`perimeter_systems.json`** — `system_id`, `display_name`, `cost`, `upkeep`,
`detection`, `failure`, `tags`.

**`detection_profiles.json`** — `profile_id`, `display_name`, `channel`,
`confidence`, `verification`, `tags`.

**`alarm_protocols.json`** — `level_id`, `display_name`, `trigger`,
`channels[]`, `response`, `drill`, `tags`.

**`gate_rules.json`** — `rule_id`, `display_name`, `applies_to`, `evidence`,
`appeal`, `tags`.

**`territory_claims.json`** — `claim_id`, `display_name`, `node`, `controller`,
`contested`, `agreement`, `tags`.

**`incident_log.json`** — `entry_id`, `night`, `post`, `detection`, `action`,
`handoff`, `lesson`, `tags`.

**`readiness_drills.json`** — `drill_id`, `display_name`, `participants[]`,
`time_target`, `failure_modes[]`, `readiness_effect`, `tags`.

**`watch_roster.json`** — `roster_id`, `post`, `shift`, `survivor`,
`fatigue`, `relief`, `rotation_rule`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid references, or out-of-range numbers.

---

## 39. APPENDIX Z — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Post coverage | perimeter health | WatchPost |
| Route completion | patrol discipline | PatrolRoute |
| Detection confidence | sensor quality | Detection |
| False alarm rate | calibration | Alarm |
| Alarm response time | readiness | Alarm |
| Gate register accuracy | governance | Gate |
| Incident lessons implemented | learning | Readiness |
| Fatigue incidents | welfare | Roster |
| Claim agreements | diplomacy | Territory |
| Drill pass rate | capability | Readiness |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score.

---

## 40. APPENDIX AA — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Detection ends at verification; force stays with combat.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows fatigue, false alarms, incidents, and drills.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel territory, combat, intelligence, or rescue system exists.

---

## 41. APPENDIX AB — OPEN QUESTIONS FOR REVIEW

1. Can a false alarm be fully eliminated by calibration, or is a small rate
   permanent?
2. Does a sleeping sentry face discipline or a welfare review?
3. Can gates be bypassed, and what is the consequence?
4. Are territory claims ever lost without combat?
5. Do joint patrols share detection data automatically?
6. Should the incident log be public or staff-only?
7. Does night work feed the dependency system as a stress source?
8. Can watch service be assigned as a duty, or is it always voluntary?

None of these may be decided unilaterally; each changes balance and tone.

---

## 42. APPENDIX AC — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Children learning the watch |
| 1 | 13 The Faithful | Night rites and vigils |
| 1 | 14 Above the Ash | Air watch and raid warning |
| 1 | 15 The Deep Root | Fence lines for fields and herds |
| 1 | 16 The Rebuilt Body | Night vision aids and prosthetics |
| 2 | 17 The Long Evening | Music on night watch |
| 2 | 18 The Underneath | Sealed watch rooms and tunnels |
| 2 | 19 The Bitter Air | Masked night posts |
| 2 | 20 The Quiet Hand | Watch as cover and counter-intel |
| 2 | 21 The Grid | Lights, alarms, and blackouts |
| 3 | 22 The Clean Flow | Gate hygiene and water |
| 3 | 23 The Alarm | Watch as first response |
| 3 | 24 The Long Goodbye | Watch and grief |
| 3 | 25 The Iron Road | Rail-side watch and signals |
| 3 | 26 The Common Table | Night meals and tea |
| 4 | 27 The Thread | Cold-weather watch gear |
| 4 | 28 The Lesson | Watch training and drills |
| 4 | 29 The Glass | Night glass and signal mirrors |
| 4 | 30 The Press | Incident notices and logs |
| 4 | 31 The Kiln | Post construction and parapets |
| 5 | 32 The Wild | Animal alarms and predator watch |
| 5 | 33 The Weather | Storm watch and closures |
| 5 | 34 The Long Road | Convoy watch and escorts |
| 5 | 35 The Habit | Fatigue and dependency |

Each hook is additive. The Watch can ship alone, and every other expansion can
ship without it.

---

## 43. APPENDIX AD — ENDING PROSE SKETCHES

**The Kept Perimeter.** Posts, routes, alarms, logs, and drills make the shelter
hard to surprise, and the watch becomes the quiet profession it always should
have been.

**The Shared Boundary.** The river claim ends in a written agreement and a joint
patrol, and the boundary stone stays a stone.

**The Reliable Watch.** The system works because the roster respects sleep, and
the night shift stops being a punishment.

**The Loud Night.** A false alarm wakes the shelter, and a real incident follows
the next week, and both are handled because the drill worked.

**The Quiet Failure.** The watch is understaffed, the fence is unwatched, and
something walks in; the log records the lesson the shelter paid for.

**Fade.** A bucket and a stick still stand at the gate, and the shelter sleeps,
mostly.

---

## 44. APPENDIX AE — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Radar minimap | wrong genre | bearing and confidence |
| Watch as combat | authority break | handoffs to combat |
| Gate humiliation | cruelty | dignity and appeal |
| Territory ethno-framing | harmful | map and agreement |
| Fatigue invisible | unfair | roster and welfare |
| Alarm spam | noise | levels and drills |
| Sentry as punishment | dehumanizing | service framing |
| Log as decoration | no learning | lessons implemented |
| Drill as menu | shallow | timed and recorded |
| Watch pointless | wasted system | real incidents |

The list exists because security content is easy to turn into either a power
fantasy or a tool of cruelty. The expansion's rule is that the watch is a
profession of attention and protocol, and the shelter's safety is built out of
small boring habits.

---

## 45. APPENDIX AF — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Watch posts | 15 | 4,000 |
| Patrol routes | 12 | 3,500 |
| Perimeter systems | 12 | 3,000 |
| Detection profiles | 12 | 3,000 |
| Alarm protocols | 8 | 2,500 |
| Gate rules | 10 | 2,500 |
| Territory claims | 10 | 3,000 |
| Incident entries | 40 | 6,000 |
| Drills | 10 | 3,000 |
| Rosters | 6 | 2,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~63,500** |

---

## 46. APPENDIX AG — FIRST YEAR OF THE WATCH

| Month | Focus | Milestone |
|---|---|---|
| 1 | Missed approach | no record exists |
| 2 | First post | staffed nights |
| 3 | Routes | patrols written |
| 4 | Listening pit | sound map |
| 5 | Alarm chain | drilled |
| 6 | Night log | first month kept |
| 7 | Gate protocol | visitors registered |
| 8 | Boundary | claim discovered |
| 9 | Contest | negotiation begins |
| 10 | Dead ground | posts repositioned |
| 11 | Welfare | roster rebuilt |
| 12 | Year review | lessons published |

A year of the watch is a year of small corrections, and by the end the shelter
has something it never had before: an accurate memory of its own nights.

---

## 47. APPENDIX AH — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_watch_post_build` | 4 | Site, build, lamp, staff |
| `quest_watch_post_staff` | 3 | Roster, brief, cover |
| `quest_watch_post_repair` | 3 | Assess, fix, test |
| `quest_watch_listening_post` | 3 | Choose, dig, seat |
| `quest_watch_fence_line` | 4 | Walk, mark, fix, log |
| `quest_watch_route_write` | 4 | Walk, time, write, approve |
| `quest_watch_route_walk` | 3 | Walk, note, report |
| `quest_watch_route_change` | 4 | Review, redraw, brief |
| `quest_watch_night_patrol` | 4 | Kit, walk, check, log |
| `quest_watch_dawn_report` | 3 | Gather, write, file |
| `quest_watch_sound_map` | 4 | Listen, note, map, verify |
| `quest_watch_calibrate` | 3 | Test, adjust, record |
| `quest_watch_motion_line` | 3 | Set, test, mark |
| `quest_watch_light_line` | 3 | Site, fuel, light |
| `quest_watch_animal_alarm` | 4 | Choose, train, place, test |
| `quest_watch_gate_protocol` | 4 | Draft, debate, post, apply |
| `quest_watch_register` | 3 | Format, write, audit |
| `quest_watch_visitor_line` | 3 | Rope, bench, rules |
| `quest_watch_curfew` | 4 | Set, publish, review |
| `quest_watch_quarantine_handoff` | 4 | Rule, drill, handoff |
| `quest_watch_mark_boundary` | 4 | Walk, choose, mark, record |
| `quest_watch_map_claims` | 4 | Scout, draw, verify |
| `quest_watch_scout_claim` | 5 | Approach, observe, withdraw |
| `quest_watch_negotiate` | 5 | Propose, argue, agree |
| `quest_watch_joint_patrol` | 4 | Agree, pair, walk, log |
| `quest_watch_drill_intruder` | 4 | Brief, run, time, review |
| `quest_watch_drill_alarm` | 3 | Brief, run, review |
| `quest_watch_drill_blackout` | 4 | Prepare, run, time, fix |
| `quest_watch_night_welfare` | 5 | Survey, rotate, pilot, adopt |
| `quest_watch_review` | 3 | Gather, discuss, publish |

---

## 48. APPENDIX AI — ALARM CHANNEL TABLE

| Channel | Range | Speed | Reliability | Use |
|---|---|---|---|---|
| Bell | shelter | fast | high | general |
| Rattle | post-to-post | fast | med | local |
| Whistle | short | fast | high | challenge |
| Runner | local | slow | high | message |
| Radio | region | fast | weather-dependent | patrol |
| Horn | ridge | medium | high | long call |
| Lamp signal | line-of-sight | slow | med | silent |
| Flags | daylight | slow | med | standby |

Channels complement each other, and each has a failure mode the shelter plans
around. A watch never depends on a single way to call for help.

---

## 49. APPENDIX AJ — NIGHT SHIFT SCHEDULE TABLE

| Shift | Hours | Post | Staff | Relief |
|---|---|---|---|---|
| First | 18–22 | all | 2 | 4h |
| Middle | 22–02 | key posts | 2 | 4h |
| Deep | 02–06 | listening, tower | 2 | 4h |
| Dawn | 06–08 | gate, fence | 2 | 4h |
| Standby | 08–18 | on call | 1 | roster |
| Rotation | weekly | all | all | review |

The night is split so that no one stands a post longer than four hours. The
schedule is the expansion's most important safety device, and it is invisible
unless the shelter ignores it.

---

## 50. APPENDIX AK — GATE REGISTER FIELD TABLE

| Field | Required | Example | Purpose |
|---|---|---|---|
| Date | yes | day 214 | record |
| Time | yes | 14:20 | record |
| Name | yes | Marn | identity |
| Origin | yes | ferry | route |
| Purpose | yes | trade | intent |
| Escort | if needed | Auri | safety |
| Items | if trade | tools | manifest |
| Health | if flagged | cough | medical |
| Exit time | on leaving | 16:05 | record |
| Notes | optional | repeat visitor | memory |

The register is a small document with a large effect: it turns the gate from a
mood into a record, and it is the first thing the steward reads after an
incident.

---

## 51. APPENDIX AL — SENSOR MAINTENANCE TABLE

| Sensor | Daily | Weekly | Season | Failure |
|---|---|---|---|---|
| Trip wire | walk | tension | replace | snap |
| Rattle | listen | reset | replace | wind |
| Bell line | check | untangle | restring | tangle |
| Lamp | fuel | wick | replace | burn out |
| Acoustic node | listen | test | calibrate | drift |
| Foot plate | clear | test | re-bed | freeze |
| Scent line | check | refresh | replace | rain |
| Animal post | feed | train | rotate | animal ill |

Maintenance tables are how the watch stays honest. A perimeter is not a purchase;
it is a weekly chore list that the whole shelter depends on.

---

## 52. APPENDIX AM — JOINT PATROL TABLE

| Arrangement | Frequency | Pairing | Communication | Escalation |
|---|---|---|---|---|
| Boundary walk | weekly | 1 shelter, 1 rival | whistle | captain |
| River watch | weekly | 2 shelter, 1 rival | radio | steward |
| Trade escort | per convoy | 1 shelter, 1 rival | runner | captain |
| Emergency call | on event | mixed | radio | both |
| Mapping | seasonal | scouting pair | notes | steward |
| Drill | quarterly | mixed | bell | both |

Joint patrols are the expansion's answer to contested territory: shared work on
a shared line. They are slower, more annoying, and far cheaper than the
alternative.

---

## 53. APPENDIX AN — LORE: THE WATCH TRADITION

The fiction:

- **The Main Gate** was a checkpoint before the war and its register custom
  survived the authority that started it.
- **The Boundary Stone** predates the settlement and both sides of the current
  disagreement treat it as older than their claim.
- **The Burned Outpost** was lost in a night nobody wrote down, and it is now
  the first place every new watcher is taken.
- **The Listening Pit** was a well that dried up and found a second career.
- **The Dead Ground** has a name because the shelter learned it the hard way,
  and the name is a standing order to walk it.

No real military unit, checkpoint, or agency is copied. The tradition is generic
and local.

---

## 54. APPENDIX AO — SENSITIVE TOPICS TABLE

| Topic | Risk | Handling |
|---|---|---|
| Gate refusals | Cruelty | Dignity, appeal |
| Territory disputes | Conflict | Map and agreement |
| Night violence | Spectacle | Handoff to combat |
| Fatigue failure | Blame | Welfare, not shame |
| Refugee arrivals | Exploitation | Medical and humane |
| Watch injuries | Gore | Restraint |
| Children on watch | Exploitation | Supervised, light |
| Drills and fear | Trauma | Calm, procedural |
| Incident deaths | Grief | Rare, logged, honored |
| Security and privacy | Abuse | Protocol and review |

The watch is where the shelter meets strangers at night, and the expansion's
contract is that the shelter is careful, predictable, and kind within the limits
of safety.

---

## 55. APPENDIX AP — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Is the watch state honest? | lifecycle + a11y tests |
| Tone | Is security humane? | content review |
| Balance | Is fatigue real? | 360-day soak |

---

## 56. APPENDIX AQ — OPEN IMPLEMENTATION NOTES

- Territory must resolve through `PatrolTerritoryAuthority` or the live combat
  rules; no parallel conquest state.
- Detections must originate in the sound-ranging engine or an authored profile
  that feeds it; never a separate enemy-spawning system.
- Posts and perimeter systems should register as world objects with condition,
  so save/load and build works integrate naturally.
- Alarms should write to existing notice and radio surfaces, not a new channel.
- Gate protocols should register with the live policy and quarantine surfaces.
- Rosters should ride on the live duty roster so fatigue and sleep are shared
  with every other shift in the shelter.
- Incident logs should be readable content and printable through the press.
- Drills must record real times and failures to be worth keeping.

---

## 57. APPENDIX AR — WATCH SERVICE TABLE

| Role | Requirement | Training | Rotation |
|---|---|---|---|
| Sentry | adult | 1 week | 4h posts |
| Patrol | adult, fit | 2 weeks | routes |
| Listener | any adult | 1 week | pit |
| Gate clerk | literate | 1 week | shifts |
| Dog handler | adult | 4 weeks | pair |
| Captain | experienced | months | lead |
| Drill lead | experienced | 2 weeks | drills |
| Medic support | medic | n/a | on call |

Watch service is a job with training and rotation. The expansion treats standing
watch as a real trade, which is what makes the roster and the welfare rules
matter.

---

## 58. APPENDIX AS — WATCH EQUIPMENT CHECKOUT TABLE

| Item | Post | Checkout | Return | Loss |
|---|---|---|---|---|
| Bell | gate | roster | dawn | report |
| Horn | tower | roster | dawn | report |
| Lantern | posts | roster | dawn | refuel |
| Whistle | patrol | roster | dawn | replace |
| Night glass | tower | signed | signed | expensive |
| Logbook | all | post | completed | archive |
| Trip wire | fence | store | repair | replace |
| Rattle | line | store | store | replace |
| Map | patrol | signed | signed | copy |
| Challenge token | gate | roster | dawn | return |

Equipment checkout is how a watch stays equipped. Every item is signed out,
signed back, and accounted for at dawn, which is the same discipline the gate
applies to visitors.

---

## 59. APPENDIX AT — INCIDENT REVIEW METHOD TABLE

| Step | Question | Evidence |
|---|---|---|
| Timeline | What happened, hour by hour? | log |
| Detection | What was seen or heard? | detection |
| Verification | What was confirmed? | verification |
| Decision | What did the watch do? | log |
| Handoff | Who took over? | handoff |
| Outcome | What happened? | record |
| Lesson | What changes now? | review |
| Owner | Who implements it? | assignment |
| Date | When is it reviewed? | calendar |
| Close | Was it fixed? | verification |

Every incident produces a method, not a mood. The review table is what turns a
bad night into a better protocol, and it is the last thing the watch does before
it sleeps.

---

## 60. CLOSING STATEMENT

ASHFALL already models territory control, patrol encounter validation, acoustic
detection with calibration and confirmed bearings, radio patrol calls, and a
night-watch narrative. What it lacks is the watch as a lived system: posts,
routes, fences, alarms, detection profiles, gate protocol, incidents, debriefs,
drills, rosters, and territory agreements. The Watch adds that world without
adding a second combat, intelligence, or rescue system. It adds a log that
records the quiet nights, a roster that respects sleep, a gate that is safe and
humane at once, and the answer to the oldest shelter question: who is awake, and
what did they see?

> Wave 5 note: this plan is one of five Wave 5 expansion bibles (32–36). Each is
> self-contained; none requires another to ship. The shared Wave 5 index lives at
> `docs/expansions/wave5/WAVE5_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `PatrolTerritoryAuthority` (`DynamicTerritoryAuthority`, `SetTerritory`,
> `GetController`, `IsClaimant`, `IsClaimedBy`, `TerritoryNodeRecord`),
> `PatrolEncounterValidator`, `SoundRangingThreatEngine`
> (`SoundRangingStationState` with calibration drift and confirming
> observations, `AcousticSensorNode`, `HostileFireObservation`,
> `AcousticThreatEstimate`), `NightWatchCatalog`, `PatrolRadioHooks`,
> `faction_territory.json` (20 KB), and the narrative `night_watch_expansion.json`,
> `night_watch_logbook.json`, and `patrol_debriefs.json`.