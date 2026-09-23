# ASHFALL — Expansion 55 Design Bible
# THE QUARTER
### Wave 9 · Dormitories, Privacy, Friction, Mediation, House Rules, Shared Walls, and the Slow Work of Living Beside People

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Shelter` (`ShelterSocialDynamicsSystem`, `SurvivorPrivacyProfile`, `SocialIncidentRecord`), `Ashfall.Core.Survivors` (`NeedsSystem` morale path, `SurvivorRelationsSystem` read seam), `Ashfall.Core.Shelter` (`ShelterNoiseSystem` coordination)
**Proposed host owner:** `QuarterHostSession` (extends `ShelterSocialSaveStore` + `ShelterSocialPanel`)
**Existing save sections:** `shelter_social_dynamics` (`ShelterSocialSave`)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no social-dynamics-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already models the friction of communal living. `ShelterSocialDynamicsSystem`
(`SystemId` = "shelter_social_dynamics") defines `SocialOutcome` (`Id`,
`DisplayName`, `MoraleDelta`, `RelationshipDelta`, `MemoryTag`, `CanMediate`,
`MediationSkillId`), `SocialEventDefinition` (`Id`, `DisplayName`, `RoomTags`,
`RequiredRoomIds`, `MinimumOccupants` default 2, `CooldownDays` default 3,
`BaseWeight` default 100, `Description`, `Outcomes`), `SocialEventCatalogData`,
`SurvivorPrivacyProfile` (`SurvivorId`, `AssignedRoomId`,
`PrivacyFatiguePermille` 0..1000, `LastSolitaryRestDay`), `SocialIncidentRecord`
(`IncidentId`, `EventId`, `RoomId`, `ParticipantIds`, `OutcomeId`, `Day`,
`IsMediated`, `MediatorId`, `Resolved`), and `ShelterSocialSave` (`systemId`,
`schemaVersion`, `privacyProfiles`, `recentIncidents`, `eventCooldowns`,
`currentDay`). The API is live: `BindMediatorSkillProvider`,
`LoadCatalog`, `GetOrCreatePrivacyProfile`, `RegisterSurvivorRoom`,
`EvaluateRoomDynamics(roomId, occupantIds, day)`, plus the events
`OnIncidentTriggered`, `OnIncidentMediated`, and `OnSocialStateChanged`, and
the save store and `ShelterSocialPanel` already exist. The data
`shelter_social_events.json` contains exactly **eight events**: midnight bunk
noise friction, a personal-space transgression, solitary recovery, an evening
mess hall gathering, a ration-distribution debate, a memorial plaque
remembrance, workbench mutual assistance, and a returned scout's
decompression.

What does not exist: the practice of living together. There is no dorm
assignment policy, no privacy plan, no house rules, no mediation path with
follow-up, no shared-space standards, no corridor culture, no quarterly
friction review, and no story about the eight people who share a wall with
you whether or not any of you chose them.

**The Quarter** is the expansion about the place a shelter actually lives:
the dormitory, the corridor, the shared table, the drying line, and the
ordinary agreements that keep ninety-four people from hating each other. It
extends the live social system and never duplicates a needs, relationship,
justice, noise, or belongings authority.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| `ShelterSocialDynamicsSystem` | Events, outcomes, privacy fatigue | Extends with practice and content |
| `SurvivorRelationsSystem` | Bonds, feuds, affinity | Reads; mediation records keep outcomes there |
| `NeedsSystem` | Morale and stress | Uses `Modify` only |
| `ShelterNoiseSystem` (Wave 6) | Noise sources and quiet hours | Coordinates; never owns noise |
| 41 The Quiet (Wave 6) | Sleep and rest policy | Respects; quarter rules never override it |
| 08 The Verdict | Formal justice and tribunals | Quarter mediation is everyday friction only |
| 26 The Common Table (Wave 3) | Meals and communal food | Mess-hall culture reads its owner |
| 48 The Pastime (Wave 8) | Clubs and evening play | Shared rooms book through both |
| 210 Personal Belongings | Belongings and keepsakes | Read-only ties; no belongings authority |
| `DutyRoster` (Exp 02) | Work shifts | House rota is separate and small; no second work rota |
| `ShelterSecurity` (Plan 138) | Locks and access | Quarter reads door state only |
| 37 The Quickening / 12 | Children and care | Family quarters route through care owners |
| `StandingRecord` (Exp 03) | Records | Files agreements and reviews |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

Ninety-four people, sixty-two beds, four walls between the night shift and the
day shift, one drying line in a corridor, and a rule about whose turn it is to
clean the head that nobody wrote down and everybody enforces.

**The Quarter** is the expansion about communal living: dorm assignments,
privacy that has to be planned rather than assumed, the friction events that
happen when tired people share a wall, mediation that ends in an agreement
instead of a grudge, house rules that are posted and revisable, shared rooms,
and the slow accumulation of a neighborhood inside a bunker. It is the
expansion about the part of a shelter that is not infrastructure at all.

### 1.2 The five loops it adds

```
  Assign ──► Live ──► Friction ──► Mediate ──► Agree
     │         │         │           │          │
     ▼         ▼         ▼           ▼          ▼
   Beds,    Rounds,   Events,    Mediators,  Agreements,
   rooms    corners   incidents  skills      follow-up
                                        │
                                        ▼
                          Post ──► Review ──► Revise
```

### 1.3 What the player manages

1. **Beds and rooms.** Assignments, occupancy, family quarters, and moves.
2. **Privacy.** Fatigue, solitary rest, curtains, corners, and quiet hours.
3. **Shared spaces.** Corridors, washrooms, drying lines, and common rooms.
4. **Friction.** The events that happen, their outcomes, and their memory.
5. **Mediation.** Who mediates, what skill they use, and how follow-up works.
6. **House rules.** The small written rules of shared living, posted and fair.
7. **Agreements.** Promises between residents, recorded and revisited.
8. **Neighborhood.** Blocks, corridors, and informal mutual aid.
9. **Reviews.** Quarterly friction review and rule revision.
10. **Records.** Incident histories, agreements, and trends.

### 1.4 What it is not

- Not a formal justice system; tribunals stay with their owner.
- Not a second noise, sleep, or rest system.
- Not a relationship model; bonds stay with their owner.
- Not a belongings system; keepsakes stay theirs.
- Not a punishment system; the quarter has no discipline authority.
- Not a social-score system; no resident is rated.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs` | Events, outcomes, privacy | `LIVE` |
| `src/Host/ShelterSocialSaveStore.cs` | `shelter_social_dynamics` save | `LIVE` |
| `src/UI/ShelterSocialPanel.cs` | Current social surface | `LIVE` |
| `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` | Morale sink | `LIVE` |
| `SurvivorRelationsSystem` | Bonds and feuds | `LIVE` |
| `ShelterNoiseSystem` (Wave 6) | Noise and quiet hours | `LIVE` |
| `CaregivingSystem` | Family and care | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `shelter_social_events.json` | 7,377 B | **8 events** |
| Dorm, rule, agreement, mediation catalogs | absent | confirmed none |
| Room standard and privacy content | absent | confirmed none |
| Review and record content | absent | confirmed none |

### 2.3 Confirmed gaps

- **GAP-55-1 — Eight events and no living practice.**
- **GAP-55-2 — No dorm assignment or family-quarter content.**
- **GAP-55-3 — No privacy plan, curtains, or solitary-rest content.**
- **GAP-55-4 — No house rules or shared-space standards.**
- **GAP-55-5 — No mediation path or follow-up content.**
- **GAP-55-6 — No agreements between residents.**
- **GAP-55-7 — No corridor or neighborhood content.**
- **GAP-55-8 — No friction review or rule revision.**
- **GAP-55-9 — No incident histories or trend records.**
- **GAP-55-10 — The live events run with no practice around them.**

### 2.4 Non-duplication statement

This expansion will **not** add a second justice, noise, sleep, bond,
belongings, morale, or rota system. It extends `ShelterSocialDynamicsSystem`
with events and practice, sends morale through `NeedsSystem.Modify`, keeps
mediation outcomes in the relationship owner, coordinates with the quiet-hours
owner, and files agreements with `StandingRecord`. All new state is additive
inside `ShelterSocialSave`. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Privacy is planned, not assumed.** A curtain, a corner, and a
posted hour are infrastructure.

**Pillar 2 — Friction is data.** The quarter does not judge incidents; it
counts them and fixes causes.

**Pillar 3 — Mediation ends in an agreement.** A resolution without a
follow-up is a mood, not a repair.

**Pillar 4 — Rules are posted and revisable.** A house rule nobody can read
is not a rule.

**Pillar 5 — Nobody is rated.** The quarter tracks conditions, never people.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Assignments | Practical, consultative | Bureaucratic cruelty |
| Privacy | Curtains, corners, hours | Surveillance or control |
| Friction | Small, human, fixable | Melodrama |
| Mediation | Two chairs, an agreement | Therapy theatre |
| Rules | Short, posted, revisable | Legalism |
| Neighborhood | Mutual aid, borrowed things | Cliques |
| Reviews | Causes and fixes | Blame |
| Conflict | Resolved or respectfully persistent | Forced friendship |

### 3.3 Content limits

- No surveillance, monitoring, or reporting on residents.
- No rating, reputation score, or social credit.
- No forced reconciliation; some frictions end in distance and that is valid.
- No violence framing; friction is noise, space, fairness, and habit.
- No substance, romance, or bedroom detail; those stay with their owners.
- No eviction or punishment authority in the quarter.
- No new save section.

---

## 4. THE QUARTER WORLD

### 4.1 Interior rooms

- **`room_quarter_office`** — the assignment board and the rule board.
- **`room_mediation_room`** — two chairs, a table, and a window.
- **`room_dorm_a`** — bunks, curtains, and the waking-shift corner.
- **`room_dorm_b`** — family bays and quiet pods.
- **`room_common_room`** — the shared table, the stove, and the bookcase.
- **`room_washroom_row`** — basins, drying lines, and the rota.
- **`room_quiet_corner`** — one chair, one lamp, no obligations.
- **`room_corridor_market`** — the informal exchange shelf.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_corridor_north` | The North Corridor | 3 | The noisiest wall |
| `loc_drying_yard` | The Drying Yard | 2 | Wash-day culture |
| `loc_bench_green` | The Bench Green | 1 | Outside sitting |
| `loc_shared_shed` | The Shared Shed | 2 | Borrowed tools |
| `loc_quiet_garden` | The Quiet Garden | 2 | Solitary rest |
| `loc_block_table` | The Block Table | 2 | Neighborhood meetings |
| `loc_family_bay_walk` | The Family Walk | 2 | Children and prams |
| `loc_night_walk` | The Night Loop | 3 | Shift-change route |
| `loc_agreement_wall` | The Agreement Wall | 2 | Posted promises |
| `loc_quarter_stone` | The Quarter Stone | 2 | Review marks |

All locations require valid item or map-node references and scanner registration.

### 4.3 The rhythm

Assignments before occupancy, rounds weekly, mediation within three days of a
friction, house rule review quarterly, and the annual quarter reading at the
stone. The expansion's clock is the shift change.

---

## 5. MAIN STORYLINE — "THE WALLS BETWEEN US"

### 5.1 Central conflict

**Vesper Callow** keeps the assignment board and has moved the same six people
three times to protect one night-shift sleeper. **Dory Larkin** argues that
privacy is not a favor the shelter grants but a standard it builds, and
brings drawings of curtains that cost almost nothing. **Winn Mennis** mediates
the same two residents for the third time and starts writing agreements down
because memory is not fairness. **Sil Roke** wants a family bay instead of
bunks, because a child of four should not learn to sleep through arguments.
**Nia Hollen** runs the shared shed where tools are borrowed and returned,
and believes the corridor exchange is the truest measure of a shelter.

Then the north corridor friction becomes a real incident: a personal-space
transgression by a tired person, a damaged curtain, and a night shift
shattered for eleven people. The quarter's answer is not punishment — it is
better assignment, a posted rule, and a mediation with a follow-up date. And
then the survey from the residents says something the leadership did not
expect: most of the friction in the shelter comes from three rooms and one
bathroom rota, and most people would rather have a quiet corner than a bigger
bunk.

The expansion's question: **what does a person owe the people they sleep
beside?**

### 5.2 Theme (unspoken)

**A dormitory is a neighborhood that has not admitted it yet.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_quarter_vesper_callow` | Vesper Callow | Quarter steward | Assignments and reviews |
| `npc_mediator_winn_mennis` | Winn Mennis | Mediator | Sessions and agreements |
| `npc_privacy_dory_larkin` | Dory Larkin | Privacy | Curtains, corners, hours |
| `npc_dorm_sil_roke` | Sil Roke | Dorm lead | Beds and family bays |
| `npc_neighbor_nia_hollen` | Nia Hollen | Neighborhood | Shed and exchange |
| `npc_common_ambrose_cove` | Ambrose Cove | Common room | Shared space and upkeep |
| `npc_house_bett_oriel` | Bett Oriel | Housekeeper | Washrooms and rota |
| `npc_corridor_tarny_beech` | Tarny Beech | Night corridor | Shift-change order |

### 5.4 Story beats (15)

1. **The Board.** Six moves are reviewed and the map is redrawn.
2. **The Curtain.** Privacy gets a budget and a workshop job.
3. **The Corner.** The quiet corner opens and is always occupied.
4. **The Rules.** House rules are posted and argued over.
5. **The Incident.** Friction becomes a real case.
6. **The Mediation.** Two chairs, an agreement, and a follow-up date.
7. **The Rota.** The washroom rota is rebuilt fairly.
8. **The Bay.** Family bays replace two bunk rows.
9. **The Shed.** The exchange shelf gets a ledger and a norm.
10. **The Wall.** A wall is sound-treated for the night shift.
11. **The Survey.** Residents name their own friction causes.
12. **The Review.** Quarterly review revises three rules.
13. **The Neighborhood.** Corridors hold their own small meetings.
14. **The Reading.** The quarter stone is cut and the year is read.
15. **The Walls Between Us.** Living together becomes a practice.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Assignments | steward-decides / consultative / lottery | fairness models |
| Privacy | standard everywhere / where asked / minimal | investment |
| Mediation | required for all / offered / opt-in | autonomy |
| Rules | written and posted / informal / none | clarity |
| Corridors | managed / self-run / silent | culture |
| Families | bays / mixed dorms / separate wing | care |
| Reviews | quarterly / annual / event-driven | attention |
| Final | quarter as institution / neighborhood / household | identity |

### 5.6 Endings (5 + fade)

1. **The Quiet House** — privacy is standard, friction falls, and the
   night-shift sleeper gets seven hours for the first time in a year.
2. **The Fair Share** — assignments, rotas, and rules are consultative and
   revisable, and the quarter has a culture of its own.
3. **The Agreement Wall** — mediation ends in posted promises with follow-up
   dates, and the wall becomes a small honest monument to living together.
4. **The Neighborhood** — corridors run their own exchanges, meetings, and
   mutual aid, and the shelter discovers it has streets.
5. **The Doors That Close** — every dorm gets a door that closes, and the
   shelter learns that privacy is not distance but respect with hinges.
6. **Fade** — a corridor at shift change, a curtain drawn, a shared table with
   two chairs left together, and a rule board with a new signature on it.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_quarter_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_quarter_board`, `quest_quarter_curtain`, `quest_quarter_corner`,
`quest_quarter_rules`, `quest_quarter_incident`, `quest_quarter_mediation`,
`quest_quarter_rota`, `quest_quarter_bay`, `quest_quarter_shed`,
`quest_quarter_wall`, `quest_quarter_survey`, `quest_quarter_review`,
`quest_quarter_neighborhood`, `quest_quarter_reading`,
`quest_quarter_living_together`.

### 6.2 Side quests (30)

**Rooms (5)**
- `quest_quarter_assign` — assignment made
- `quest_quarter_move` — move completed
- `quest_quarter_family` — family bay set
- `quest_quarter_wake` — shift-corner built
- `quest_quarter_cedar` — bedding refreshed

**Privacy (5)**
- `quest_quarter_curtain_job` — curtains hung
- `quest_quarter_solitary` — solitary rest taken
- `quest_quarter_hours` — quiet hours posted
- `quest_quarter_door` — door fitted
- `quest_quarter_bunk` — bunk privacy kit

**Shared spaces (5)**
- `quest_quarter_wash_rota` — wash rota fair
- `quest_quarter_drying` — drying line rules
- `quest_quarter_common_book` — common room upkeep
- `quest_quarter_corridor_store` — corridor clear
- `quest_quarter_lamp` — corridor lamp fixed

**Friction and mediation (5)**
- `quest_quarter_log_case` — case logged
- `quest_quarter_session` — session held
- `quest_quarter_agreement` — agreement written
- `quest_quarter_followup` — follow-up kept
- `quest_quarter_distance` — respectful distance

**Rules and reviews (5)**
- `quest_quarter_rule_post` — rule posted
- `quest_quarter_rule_revise` — rule revised
- `quest_quarter_quarter_review` — review held
- `quest_quarter_survey_run` — survey run
- `quest_quarter_notice` — notice posted

**Neighborhood (5)**
- `quest_quarter_shed_ledger` — shed ledger
- `quest_quarter_borrow` — borrowed and returned
- `quest_quarter_block_meet` — block meeting
- `quest_quarter_help` — neighbor helped
- `quest_quarter_stone` — stone marked

### 6.3 Repeatable quests (8)

`quest_quarter_repeat_round`, `quest_quarter_repeat_case`,
`quest_quarter_repeat_review`, `quest_quarter_repeat_rota`,
`quest_quarter_repeat_corner`, `quest_quarter_repeat_shed`,
`quest_quarter_repeat_notice`, `quest_quarter_repeat_reading`.

### 6.4 Dynamic hooks

Live events (`OnIncidentTriggered`, `OnIncidentMediated`,
`OnSocialStateChanged`, noise spikes from the quiet owner, caregiving
changes, roster changes, deaths) attach authored follow-ups through existing
seams. No new event bus.

### 6.5 Constraints

- Friction events and privacy fatigue stay with the live social system.
- Noise and quiet hours stay with `ShelterNoiseSystem`.
- Bonds, feuds, and affinity stay with `SurvivorRelationsSystem`.
- Morale uses `NeedsSystem.Modify` only.
- Formal justice stays with the verdict owner.
- Belongings stay with their owner.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `QuarterSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** the assignment board, room standards, occupancy, moves, family
bays, and shift zones. **Consumes:** rooms, roster, care owners. **Data:**
`quarter_rooms.json`. **Rules:** assignments are made with a reason, avoided
without penalty; families and shift-workers have authored needs; a move is
recorded with its cause.

### 7.2 `PrivacySystem` (extend `ShelterSocialDynamicsSystem`)

**Owns:** privacy standards, curtains, doors, solitary rest, quiet corners,
and posted hours. **Consumes:** live `PrivacyFatiguePermille` and
`LastSolitaryRestDay`, workshop, textile owner. **Data:**
`quarter_privacy.json`. **Rules:** fatigue is a real number that rises in
crowding and falls with solitary rest; standards are buildable; hours are
negotiated with the noise owner.

### 7.3 `NeighborSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** corridors, blocks, informal meetings, the exchange shelf, and
mutual aid. **Consumes:** rooms, inventory for exchanges. **Data:**
`quarter_neighborhood.json`. **Rules:** the corridor is not a room to manage
but a place to keep; exchanges are gifts or swaps, never currency; meetings
are optional.

### 7.4 `MediationSystem` (extend `ShelterSocialDynamicsSystem`)

**Owns:** mediators, skills, sessions, agreements, follow-up dates, and
respectful distance. **Consumes:** the live `CanMediate`/`MediationSkillId`
fields, relationship owner. **Data:** `quarter_mediation.json`,
`quarter_agreements.json`. **Rules:** mediation is offered, never forced;
agreements are written in the participants' words; a follow-up date is set;
distance is a valid outcome.

### 7.5 `HouseRuleSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** house rules, posting, revision, and the rule board. **Consumes:**
review results, notice owner. **Data:** `quarter_rules.json`. **Rules:** rules
are short, posted where they apply, and revisable by review; an unposted rule
is not enforceable; rules never override quiet hours or safety.

### 7.6 `SharedSpaceSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** washrooms, drying lines, common room upkeep, corridor storage, and
shared rota. **Consumes:** sanitation, textile, workshop. **Data:**
`quarter_shared.json`. **Rules:** shared rotas are fair by rotation and
recorded; corridor storage has limits; a shared room has an upkeep owner.

### 7.7 `QuarterReviewSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** friction reviews, resident surveys, rule revisions, and the quarter
stone. **Data:** `quarter_reviews.json`. Records through `StandingRecord`.
**Rules:** reviews name causes, never people; surveys are anonymous; the stone
records year marks for whoever lives there next.

### 7.8 `QuarterRecordSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** incident histories, agreement records, and trend summaries. **Data:**
`quarter_records.json`. Records through `StandingRecord`. **Rules:** histories
are facts and fixes; no resident is named in a trend; the record exists to
prevent repeat friction, not to remember grudges.

### 7.9 Systems explicitly not added

- No second justice, noise, sleep, bond, belongings, morale, or work system.
- No surveillance, rating, or social credit.
- No forced reconciliation or punishment authority.
- No romance, bedroom, or substance content.
- No new currency.
- No new RNG stream beyond the live social roll.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `quarter_rooms.json` (new)

```json
{
  "schema_version": 1,
  "rooms": [
    {
      "room_id": "room_dorm_a",
      "display_name": "Dormitory A",
      "beds": 12,
      "privacy_standard": "curtain",
      "shift_zone": "night",
      "family_bay": false,
      "tags": ["quarters", "shared"]
    }
  ]
}
```

### 8.2 `quarter_privacy.json` (new)

Privacy: standard, build, cost, fatigue relief, hours, note.

### 8.3 `quarter_mediation.json` (new)

Mediation: mediator, skill, session length, rooms, follow-up cadence.

### 8.4 `quarter_agreements.json` (new)

Agreements: participants, terms, day, follow-up, status, keeper.

### 8.5 `quarter_rules.json` (new)

Rules: rule id, text, scope, posted where, revision day.

### 8.6 `quarter_shared.json` (new)

Shared spaces: room, upkeep owner, rota, standard, check cadence.

### 8.7 `quarter_neighborhood.json` (new)

Neighborhood: corridor, block, meeting cadence, exchange shelf, note.

### 8.8 `quarter_reviews.json` (new)

Reviews: quarter, causes, rule changes, survey summary, stone mark.

### 8.9 `quarter_records.json` (new)

Records: incident, cause, fix, follow-up, resolved, trend note.

### 8.10 `quarter_events_expansion.json` (new, additive to the live catalog)

Events: id, display, room tags, occupants, cooldown, outcomes.

### 8.11 Items

New items appended to `items.json`: `item_curtain_track`,
`item_curtain_cloth`, `item_bunk_screen`, `item_sound_blanket`,
`item_quiet_corner_chair`, `item_drying_line`, `item_door_felt`,
`item_rota_board`, `item_rule_board`, `item_agreement_card`,
`item_mediation_table`, `item_exchange_ledger`, `item_corridor_lamp`,
`item_floor_rug_muted`, `item_peg_hooks`, `item_quarter_stone_chisel`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`ShelterSocialSave` remains the live save owner. New sub-objects (assignments,
privacy builds, mediation cases, agreements, rules, shared rota, reviews,
records) are additive inside it. No new save section.

### 9.2 State to persist

- Room assignments, occupancy, and shift zones.
- Privacy standards built and fatigue history.
- Mediation cases, agreements, and follow-up dates.
- House rules and their revisions.
- Shared-space rotas and upkeep state.
- Neighborhood meetings and exchanges.
- Reviews, surveys, and stone marks.
- Incident records with causes and fixes.

### 9.3 Determinism

- Friction events stay on the live `EvaluateRoomDynamics` seeded path with
  its cooldowns and weights.
- Privacy fatigue uses the live permille fields.
- Mediation outcomes derive from skills and agreements, not chance.
- Reviews derive from recorded incidents and anonymous survey totals.
- Paired replay hashes must match; no `System.Random`.

### 9.4 Migration

Legacy saves load with privacy profiles and incidents intact; new practice
state (assignments, rules, agreements, reviews) starts empty. Existing
incidents load as history and are not re-evaluated or re-triggered.

### 9.5 Checksum

Invariant-culture floats; integer day and permille fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `ShelterSocialPanel` (extend) | Incidents and privacy | `QuarterHostSession` |
| `AssignmentPanel` (new) | Beds and rooms | same |
| `PrivacyPanel` (new) | Standards and fatigue | same |
| `MediationPanel` (new) | Sessions and agreements | same |
| `HouseRulesPanel` (new) | Rules and revisions | same |
| `SharedSpacePanel` (new) | Rotas and upkeep | same |
| `QuarterReviewPanel` (new) | Reviews and stone | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Fatigues are numbers with trends, never judgments about a person.
- Mediation screens quote the participants' own agreement terms.
- House rules are readable in full from the panel that posts them.
- No resident is named in trend data; no score exists anywhere.
- Keyboard/controller close/back preserved; focus maintained on refresh.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a curtain being drawn, a door
closing softly, two chairs set at a table, a pen on a rule board, a corridor
lamp humming. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `ShelterSocialDynamicsSystem` | Events, privacy, mediation |
| `NeedsSystem` | Morale via `Modify` |
| `SurvivorRelationsSystem` | Bonds and feuds (read, record outcomes) |
| `ShelterNoiseSystem` (Wave 6) | Quiet hours and noise coordination |
| 41 The Quiet (Wave 6) | Sleep policy respected |
| 26 The Common Table (Wave 3) | Mess hall culture |
| 48 The Pastime (Wave 8) | Shared room bookings |
| `DutyRoster` (Exp 02) | Shift zones and house rota |
| `CaregivingSystem` | Family quarters and care |
| `ChildDevelopmentSystem` (Wave 6) | Children in shared spaces |
| `ShelterSecurity` (Plan 138) | Door state and locks |
| `SanitationSystem` (Wave 3) | Washrooms and rotas |
| `TextileSystem` (Wave 4) | Curtains and felt |
| `ShelterWorkshopSystem` (Wave 6) | Privacy builds |
| 08 The Verdict | Formal justice boundary |
| `StandingRecord` (Exp 03) | Agreements and reviews |
| `JournalSystem` | Personal records of friction |
| `EpilogueChronicleBuilder` | Quarter history lines |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm the social system, save store, panel,
needs, relations, noise, care, roster, and record owners. Record file:line;
change nothing.

**Phase 1 — Data + validators.** Author the ten catalogs and the additive
event expansion; register validators and scanner.

**Phase 2 — Pure Core.** `QuarterSystem`, `PrivacySystem`, `NeighborSystem`,
`MediationSystem`, `HouseRuleSystem`, `SharedSpaceSystem`,
`QuarterReviewSystem`, `QuarterRecordSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `QuarterHostSession`, focused selftest coverage,
fresh journey from the board to the reading.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Multi-year soak: crowding, privacy builds, mediation,
rule revisions, and falling friction.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Rooms | 24 |
| Privacy standards | 12 |
| Mediation skills | 8 |
| Agreements | 20 |
| House rules | 20 |
| Shared spaces | 12 |
| Neighborhood blocks | 8 |
| Reviews | 10 |
| Records | 24 |
| Events (additive) | 16 |
| Items | 16 |
| Locations | 10 |
| Rooms (design set) | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 55,000–70,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Justice duplication | High | Boundary and referral |
| Noise duplication | Critical | Quiet owner coordination |
| Rating drift | Critical | No scores by contract |
| Surveillance drift | Critical | No monitoring |
| Relationship duplication | High | Read and record only |
| Belongings duplication | Medium | Read-only ties |
| Forced reconciliation | High | Distance is valid |
| Determinism break | Low | Live seeded events |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `quarter_rooms.json` | 24 | 3,500 |
| `quarter_privacy.json` | 12 | 2,500 |
| `quarter_mediation.json` | 8 | 2,000 |
| `quarter_agreements.json` | 20 | 4,000 |
| `quarter_rules.json` | 20 | 3,000 |
| `quarter_shared.json` | 12 | 2,500 |
| `quarter_neighborhood.json` | 8 | 2,000 |
| `quarter_reviews.json` | 10 | 2,500 |
| `quarter_records.json` | 24 | 4,000 |
| `quarter_events_expansion.json` | 16 | 4,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~60,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R55-1 | Justice overlap | Med | High | Referral |
| R55-2 | Noise overlap | Med | Critical | Quiet owner |
| R55-3 | Rating drift | Low | Critical | No scores |
| R55-4 | Surveillance | Low | Critical | Contract |
| R55-5 | Relations overlap | Med | High | Read/record |
| R55-6 | Belongings overlap | Low | Medium | Read-only |
| R55-7 | Forced amends | Med | High | Distance valid |
| R55-8 | Determinism | Low | High | Live events |
| R55-9 | Content overrun | Med | Medium | Budget |
| R55-10 | Dour tone | Med | Medium | Small fixes |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **How are rooms assigned?** Recommended: steward proposes with a reason;
   residents may request and appeal; no lotteries unless chosen.
2. **Is mediation ever mandatory?** Recommended: no; it is offered, and
   respectful distance is a first-class outcome.
3. **Who may propose a house rule?** Recommended: any resident, via the
   notice owner, with review.
4. **Can a resident refuse a dorm move?** Recommended: yes, with the reason
   recorded and an alternative offered.
5. **What does the quarter stone record?** Recommended: the year and one
   number — incidents reviewed — and nothing about individuals.

---

## 17. APPENDIX D — ROOM TABLE (24 ROOMS)

| # | Room | Beds | Privacy | Zone | Family |
|---|---|---|---|---|---|
| 1 | Dormitory A | 12 | curtain | night | no |
| 2 | Dormitory B | 12 | curtain | day | no |
| 3 | Dormitory C | 8 | curtain | mixed | no |
| 4 | Family bay one | 4 | door | mixed | yes |
| 5 | Family bay two | 4 | door | mixed | yes |
| 6 | Quiet pods | 6 | screen | any | no |
| 7 | Recovery bay | 2 | door | any | no |
| 8 | Elder alcove | 2 | curtain | day | no |
| 9 | Apprentice bay | 6 | curtain | mixed | no |
| 10 | Night shift room | 8 | sound blanket | night | no |
| 11 | Common room | n/a | n/a | any | no |
| 12 | Washroom row | n/a | n/a | any | no |
| 13 | Quiet corner | n/a | n/a | any | no |
| 14 | Mediation room | n/a | n/a | any | no |
| 15 | Quarter office | n/a | n/a | day | no |
| 16 | Exchange shelf | n/a | n/a | any | no |
| 17 | Drying room | n/a | n/a | any | no |
| 18 | Reading room tie | n/a | n/a | any | no |
| 19 | Craft corner | n/a | n/a | day | no |
| 20 | Children's corner | n/a | n/a | day | yes |
| 21 | Study table | n/a | n/a | day | no |
| 22 | Tool loan shed | n/a | n/a | any | no |
| 23 | Laundry folding | n/a | n/a | day | no |
| 24 | Bench hall | n/a | n/a | any | no |

Twenty-four rooms with beds, privacy standards, shift zones, and family flags,
and the quiet-pods row exists because the survey found something the leadership
did not predict: more people want one chair and a lamp than a bigger bunk. The
night-shift room with its sound blanket is the single most requested build in
the quarter's first year.

---

## 18. APPENDIX E — PRIVACY STANDARD TABLE

| # | Standard | Build | Cost | Relief | Turns |
|---|---|---|---|---|---|
| 1 | Curtain | track, cloth | low | 15 | 2 days |
| 2 | Bunk screen | frame, felt | low | 20 | 1 day |
| 3 | Door felt | strip, hook | very low | 10 | hours |
| 4 | Sound blanket | quilt, hooks | medium | 25 | 1 day |
| 5 | Quiet pod | screen, lamp | medium | 40 | 3 days |
| 6 | Shift corner | curtain, sign | low | 20 | 2 days |
| 7 | Corridor lamp | fixture | low | 5 | hours |
| 8 | Floor rug | matting | low | 10 | hours |
| 9 | Peg wall | hooks, board | low | 5 | hours |
| 10 | Family door | door, frame | high | 60 | week |
| 11 | Recovery door | door, light | high | 50 | week |
| 12 | Corner chair | chair, lamp | very low | 30 | hours |

Twelve privacy standards with real costs and real fatigue relief, and the row
that does the most work is the fourth: a quilt hung from hooks, which is the
cheapest way a shelter has ever bought its night shift six extra hours of
sleep. The door rows cost a week each and are worth it exactly once per room.

---

## 19. APPENDIX F — MEDIATION TABLE

| # | Mediator | Skill | Session | Room | Follow-up |
|---|---|---|---|---|---|
| 1 | Winn | listening | 1 hour | mediation room | 7 days |
| 2 | Vesper | assignment | 30 min | quarter office | 14 days |
| 3 | Dory | boundary | 1 hour | bench green | 7 days |
| 4 | Sil | dorm life | 30 min | dorm table | 14 days |
| 5 | Nia | exchange | 30 min | shed | 7 days |
| 6 | Ambrose | shared space | 1 hour | common room | 14 days |
| 7 | Bett | rota | 30 min | washroom row | 7 days |
| 8 | Tarny | shift | 30 min | night loop | 14 days |

Eight mediators with different skills, and the follow-up column is the
expansion's central mechanism: a session without a date is a conversation, and
the date is what turns it into a repair. The shed and washroom rows exist
because most real friction is not about feelings at all — it is about borrowed
tools and a rota that quietly became unfair.

---

## 20. APPENDIX G — AGREEMENT TABLE

| # | Agreement | Parties | Terms | Follow-up | Status |
|---|---|---|---|---|---|
| 1 | Night quiet | two residents | no boots after 22 | 7 days | held |
| 2 | Borrow return | shed users | return in 2 days | 14 days | held |
| 3 | Rota fairness | washroom row | rotate weekly | 14 days | revised |
| 4 | Space boundary | bunk neighbors | peg wall line | 7 days | held |
| 5 | Lamp shade | corridor | no lamp after 23 | 7 days | held |
| 6 | Door courtesy | dorm C | knock twice | 14 days | held |
| 7 | Exchange shelf | corridor | label gifts | 30 days | held |
| 8 | Storage limit | corridor | nothing under 60 cm | 14 days | held |

Eight agreements written in the participants' own words, and the third row is
the one that changed a rule instead of a person: a rota that was quietly
unfair was revised for everyone, which is how the quarter prefers to solve
friction — fix the system first and the people second. The rest are held,
which is the quietest compliment an agreement can receive.

---

## 21. APPENDIX H — HOUSE RULE TABLE

| # | Rule | Scope | Posted | Revised |
|---|---|---|---|---|
| 1 | Quiet after 22 | all quarters | all dorm doors | year 1 |
| 2 | No boots past the mat | all quarters | all dorm doors | year 1 |
| 3 | Corridor clear | all corridors | corridor ends | year 1 |
| 4 | Label borrowed tools | shed | shed door | year 2 |
| 5 | Rota rotates weekly | washrooms | washroom row | year 2 |
| 6 | Lamp hoods after 23 | corridors | lamp posts | year 2 |
| 7 | Knock twice | dorm C | dorm C door | year 3 |
| 8 | Drying line by turn | drying room | line wall | year 2 |
| 9 | Gifts are labelled | shelf | shelf | year 3 |
| 10 | No food in dorms | all quarters | dorm doors | year 4 |
| 11 | Shared table left clear | common | table | year 3 |
| 12 | Night shift first at breakfast | mess | mess door | year 4 |

Twelve house rules with posting and revision columns, and the last row is the
quarter's proudest and smallest victory: a rule that gives the night shift
first access to breakfast, written by a vote of the people who never get it.
A house rule is a institution, and the table shows them accumulating one
revision at a time.

---

## 22. APPENDIX I — SHARED SPACE TABLE

| # | Space | Upkeep | Rota | Standard | Check |
|---|---|---|---|---|---|
| 1 | Washroom row | Bett | weekly | clean basins | daily |
| 2 | Drying room | Bett | by turn | dry lines | weekly |
| 3 | Common room | Ambrose | daily sweep | clear table | daily |
| 4 | Quiet corner | Dory | weekly | lamp, chair | weekly |
| 5 | Exchange shelf | Nia | monthly | labelled | weekly |
| 6 | Tool shed | Nia | monthly | inventory | monthly |
| 7 | Bench hall | Ambrose | weekly | swept | weekly |
| 8 | Children's corner | Sil | daily | tidy | daily |
| 9 | Study table | Vesper | weekly | clear | weekly |
| 10 | Laundry folding | Bett | by turn | folded | daily |
| 11 | Corridor lamps | Tarny | monthly | working | weekly |
| 12 | Night loop | Tarny | weekly | lit | weekly |

Twelve shared spaces with upkeep owners and rotas, and the upkeep column is
the quarter's most important abstraction: every shared room has a name, and a
room with a name is a room that gets cleaned. The children's corner is daily
because children use it daily, and the quiet corner is weekly because the
person who keeps it believes a lamp with dust on it is a broken promise.

---

## 23. APPENDIX J — NEIGHBORHOOD TABLE

| # | Corridor | Block | Meeting | Exchange | Keeper |
|---|---|---|---|---|---|
| 1 | North | A block | monthly | shelf | Nia |
| 2 | South | B block | monthly | shelf | Ambrose |
| 3 | East | C block | monthly | shelf | Sil |
| 4 | West | D block | monthly | shelf | Bett |
| 5 | Loop | night | monthly | none | Tarny |
| 6 | Family walk | family | monthly | toys | Dory |
| 7 | Workshop row | trades | monthly | tools | Nia |
| 8 | Garden path | green | seasonal | seeds | Sil |

Eight corridors with blocks, meetings, and exchange points, and the family walk
row is the one that turns a corridor into a street: prams, toys, chalk, and a
neighbor who always has an extra. The west row having no exchange is
deliberate; not every corridor wants a shelf, and the quarter respects that.

---

## 24. APPENDIX K — REVIEW TABLE

| # | Quarter | Causes named | Rules changed | Stone mark |
|---|---|---|---|---|
| 1 | Year 1 Q1 | noise, rota | 2 | none |
| 2 | Year 1 Q2 | space, storage | 1 | none |
| 3 | Year 1 Q3 | shift sleep | 2 | yes |
| 4 | Year 1 Q4 | exchange, lamps | 1 | yes |
| 5 | Year 2 Q1 | rota, food | 2 | none |
| 6 | Year 2 Q2 | boots, drying | 1 | none |
| 7 | Year 2 Q3 | none | 0 | yes |
| 8 | Year 2 Q4 | shift sleep | 0 | yes |

Eight quarterly reviews with causes, changes, and stone marks, and the third
year's two zero-change quarters are the point: a review that finds nothing
wrong and records the finding honestly is the system working, and the stone
marks stop being about incidents and start being about the year someone
finally slept with the window open. The causes are plural because the review
names conditions, never people.

---

## 25. APPENDIX L — INCIDENT RECORD TABLE

| # | Day | Event | Cause | Fix | Resolved |
|---|---|---|---|---|---|
| 1 | 12 | bunk noise | night shift | curtain | yes |
| 2 | 25 | space breach | no boundary | peg line | yes |
| 3 | 41 | rota dispute | vague rota | weekly turn | yes |
| 4 | 58 | storage block | corridor boxes | shelf limit | yes |
| 5 | 77 | lamp fight | hood missing | lamp hoods | yes |
| 6 | 96 | food in dorm | crumbs, ants | rule posted | yes |
| 7 | 121 | tool missing | no ledger | shed ledger | yes |
| 8 | 150 | shower queue | one basin | rota split | yes |
| 9 | 188 | night noise | boots past mat | boot rule | yes |
| 10 | 220 | drying row | no line turn | line rule | yes |
| 11 | 260 | kids' corner | toys gone | basket | yes |
| 12 | 299 | corridor lamp | bulb out | spare shelf | yes |

Twelve incidents across a year, every one resolved, and the fix column is
the expansion's quiet thesis: none of the fixes is a punishment, and none of
them names a person, because the quarter's job is to change the conditions
that made a tired person loud or a kind person thoughtless.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_quarter_board` | 4 | Assignments remade |
| `quest_quarter_curtain` | 3 | Curtains hung |
| `quest_quarter_corner` | 3 | Corner opens |
| `quest_quarter_rules` | 4 | Rules posted |
| `quest_quarter_incident` | 5 | Case handled |
| `quest_quarter_mediation` | 4 | Agreement written |
| `quest_quarter_rota` | 3 | Rota fair |
| `quest_quarter_bay` | 4 | Family bays |
| `quest_quarter_shed` | 3 | Shed ledger |
| `quest_quarter_wall` | 4 | Wall treated |
| `quest_quarter_survey` | 3 | Survey run |
| `quest_quarter_review` | 4 | Review revises |
| `quest_quarter_neighborhood` | 4 | Blocks meet |
| `quest_quarter_reading` | 3 | Stone read |
| `quest_quarter_living_together` | 3 | Practice ordinary |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_quarter_assign` | 3 | Assignment made |
| `quest_quarter_move` | 4 | Move done |
| `quest_quarter_family` | 4 | Bay set |
| `quest_quarter_wake` | 3 | Shift corner |
| `quest_quarter_cedar` | 3 | Bedding refreshed |
| `quest_quarter_curtain_job` | 3 | Curtains hung |
| `quest_quarter_solitary` | 3 | Solitary rest |
| `quest_quarter_hours` | 3 | Hours posted |
| `quest_quarter_door` | 4 | Door fitted |
| `quest_quarter_bunk` | 3 | Bunk kit |
| `quest_quarter_wash_rota` | 3 | Rota fair |
| `quest_quarter_drying` | 3 | Line rules |
| `quest_quarter_common_book` | 3 | Upkeep kept |
| `quest_quarter_corridor_store` | 3 | Corridor clear |
| `quest_quarter_lamp` | 3 | Lamp fixed |
| `quest_quarter_log_case` | 3 | Case logged |
| `quest_quarter_session` | 4 | Session held |
| `quest_quarter_agreement` | 4 | Agreement written |
| `quest_quarter_followup` | 3 | Follow-up kept |
| `quest_quarter_distance` | 3 | Distance respected |
| `quest_quarter_rule_post` | 3 | Rule posted |
| `quest_quarter_rule_revise` | 3 | Rule revised |
| `quest_quarter_quarter_review` | 4 | Review held |
| `quest_quarter_survey_run` | 3 | Survey run |
| `quest_quarter_notice` | 3 | Notice posted |
| `quest_quarter_shed_ledger` | 3 | Ledger kept |
| `quest_quarter_borrow` | 3 | Tool returned |
| `quest_quarter_block_meet` | 3 | Block met |
| `quest_quarter_help` | 3 | Neighbor helped |
| `quest_quarter_stone` | 3 | Stone marked |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Vesper Callow** — quarter steward. Has moved the same six people three times
to protect one sleeper and wants the board to stop being a puzzle. Believes an
assignment is a small justice.

**Winn Mennis** — mediator. Mediated the same two residents three times before
starting to write agreements down. Believes memory is not fairness.

**Dory Larkin** — privacy. Draws curtains that cost nothing and argues for
standards over favors. Believes privacy is built, not granted.

**Sil Roke** — dorm lead. Wants a family bay because a child of four should
not learn to sleep through arguments. Believes the smallest residents set the
standard for the building.

**Nia Hollen** — neighborhood. Runs the shed, keeps the ledger, and knows
which corridor borrows and which forgets. Believes the exchange shelf is the
truest measure of a shelter.

**Ambrose Cove** — common room. Sweeps the shared table daily and believes a
clear table is an invitation. Believes a room with a name gets cleaned.

**Bett Oriel** — housekeeper. Runs the washroom rota fairly and notices when a
rule quietly stops being fair. Believes a fair rota prevents most arguments
before they start.

**Tarny Beech** — night corridor. Works the night loop and speaks for the
people whose day begins when everyone else's ends. Believes the night shift is
a neighborhood too.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The North Corridor** — the loudest wall and the first curtain.
- **The Drying Yard** — sheets, wind, and a line turn order.
- **The Bench Green** — two benches and the best argument weather.
- **The Shared Shed** — tools, a ledger, and one hammer that always returns.
- **The Quiet Garden** — one chair, one lamp, no obligations.
- **The Block Table** — corridor meetings and a posted rule board.
- **The Family Walk** — prams, chalk, and a neighbor with an extra.
- **The Night Loop** — shift-change footprints and a hooded lamp.
- **The Agreement Wall** — promises with dates on them.
- **The Quarter Stone** — the year and one number. 

---

## 30. APPENDIX Q — QUARTER CHARTER

| Clause | Promise |
|---|---|
| Assigned | Every bed has a reason, and every person can ask |
| Private | A curtain, a corner, and an hour are standards |
| Shared | Every shared room has a name and an upkeep owner |
| Heard | Friction is logged with its cause and never with a name |
| Mediated | Disagreement is offered a table, never sentenced |
| Free | Distance is a valid outcome and is respected |
| Posted | A rule that is not posted is not enforced |
| Revised | Rules change when reviews find causes |
| Counted | No resident is ever rated or scored |
| Kept | The quarter stone records the year and nothing personal |

The quarter charter is the expansion's first-class design object, posted at the
assignment board and at every corridor end. Its ninth clause is the one the
system is built to honor: a quarter with numbers on its walls is a quarter
that has stopped being a home, and the plan refuses every feature that would
put a number there.

---

## 32. APPENDIX R — WORKED QUARTER YEAR

**Month one.** Vesper maps the dormitory and finds that six people have been
moved three times for one sleeper. The board is redrawn with reasons written
beside every assignment, and the first thing built is not a bunk but a curtain
track over the night-shift row.

**Month two.** Dory's curtain plan gets a workshop slot and a small budget, and
twelve curtains go up in nine days. The privacy fatigue column in the panel
shows its first real decline, and the quarter discovers that a shelter can buy
sleep with cloth.

**Month three.** The quiet corner opens in the old stores alcove with one
chair and one lamp, and within a week there is a waiting list that nobody
manages because the corner manages itself: people simply leave when they are
done, and the waiting happens on the bench outside.

**Month four.** The house rules go up after an argument about boots that
lasted longer than the original noise. Seven rules are posted, two are
rewritten after complaints, and the word "posted" becomes a joke that means
"decided."

**Month five.** The north corridor incident happens: a space transgression by
a tired resident, a torn curtain, and eleven night-shift sleepers awake. The
quarter logs it with causes and no names, assigns a mediator, and sets a
follow-up date which, being written down, is actually kept.

**Month six.** The mediation produces the first agreement: no boots past the
mat after twenty-two, and a knock-twice rule for the dorm door. Both parties
sign it in their own words, and the agreement wall gets its first card.

**Month seven.** The washroom rota is rebuilt after a review finds that one
corridor has been doing an extra turn for four months. The fix is a printed
rotation and a rule, and the review's lesson is recorded: fairness is
maintenance.

**Month eight.** Family bay one is built from two bunk rows and a door from
the workshop. A child of four sleeps through the night for the first time in
the shelter's history, which is recorded in the quarter's log as a
construction milestone because that is what it is.

**Month nine.** Nia's exchange shelf gets a ledger and a norm: gifts are
labelled, tools are returned in two days, and nothing is sold. The shed's
hammer returns for the first time in a month, and the ledger's first line is
about the hammer.

**Month ten.** The wall between the night-shift room and the common room is
sound-treated with quilts and felt, and the hammering happens at a civil hour
because the quarter has learned its own lesson.

**Month eleven.** The resident survey is run anonymously and surprises
nobody who was paying attention: most friction comes from three rooms and one
bathroom rota, and the most requested change is more quiet corners. The
survey is read at a block meeting and three rules are revised.

**Month twelve.** The quarter stone is cut with the year and one number — the
incidents reviewed — and the year is read aloud at the block table. The
reading takes four minutes. Nobody is named. At the end, Tarny asks whether
the night shift can have first access to breakfast, and the quarter votes, and
the answer is yes.

---

## 33. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Dory hangs the first curtain and steps back, and the night-shift sleeper
> looks at it for a while and then says: that is the first wall I have had in
> two years, and Dory does not say anything clever because there is nothing
> clever to say.

> Winn writes the agreement and reads it back slowly, and both parties correct
> him twice, and the final card says no boots after ten and knock twice, which
> is a treaty between two people about a coridor.

> The quiet corner is occupied when Dory checks it at noon and occupied when
> she checks at four, and she decides not to manage it, and that decision is
> the contribution the corner needed.

> Tarny stands at the block table and asks for breakfast, and the room votes,
> and the ayes have it by four, and she goes back to the night loop and tells
> her shift, and somebody cries a little into a cup of tea, which the minutes
> record as: vote passed.

---

## 34. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| No assignments | random friction | board and reasons |
| No privacy | fatigue climbs | curtains and corners |
| Unwritten rules | unfair enforcement | post and revise |
| Unlogged friction | repeated causes | record with no names |
| No follow-up | agreements decay | dates in the panel |
| Forced mediation | resentment | distance allowed |
| Ignored rota | quiet unfairness | printed rotation |
| Silent survey | wrong fixes | anonymous totals |
| Named trends | blame and shame | conditions only |
| No stone | no memory | year mark |

Every recovery here is a small piece of paperwork, and the plan's honest
position is that communal living is not fixed by heroism but by boards,
curtains, dates, and a printed rota that somebody finally made fair.

---

## 35. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] No surveillance, monitoring, or reporting on residents.
- [ ] No scores, ratings, or social credit anywhere.
- [ ] No forced reconciliation; distance is valid.
- [ ] Friction events stay on the live seeded path.
- [ ] Morale routes only through `NeedsSystem.Modify`.
- [ ] Bonds and feuds stay with `SurvivorRelationsSystem`.
- [ ] Quiet hours stay with `ShelterNoiseSystem`.
- [ ] Formal justice stays with the verdict owner.
- [ ] No names appear in trend records.
- [ ] Save additions are additive inside `shelter_social_dynamics`.

---

## 36. APPENDIX V — GLOSSARY

- **Quarter** — the shared living space and its practice.
- **Assignment** — a bed with a written reason.
- **Privacy fatigue** — the live permille that rises with crowding.
- **Solitary rest** — a scheduled hour that lowers fatigue.
- **Friction** — a live social event with outcomes.
- **Mediation** — an offered session with a follow-up date.
- **Agreement** — terms written in the participants' own words.
- **House rule** — a short, posted, revisable rule of shared living.
- **Block** — a corridor neighborhood with a table.
- **Stone** — the year and one number, and nothing personal.

---

## 37. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `ShelterSocialDynamicsSystem` | rooms | events | needs |
| `QuarterSystem` | roster | assignments | rooms |
| `PrivacySystem` | fatigue | standards | noise |
| `NeighborSystem` | corridors | exchanges | inventory totals |
| `MediationSystem` | skills | agreements | relations |
| `HouseRuleSystem` | reviews | rules | quiet policy |
| `SharedSpaceSystem` | sanitation | rotas | sanitation |
| `QuarterReviewSystem` | incidents | reviews | names in trends |
| `QuarterRecordSystem` | history | records | nothing |
| `NeedsSystem` | nothing | nothing | nothing |
| `SurvivorRelationsSystem` | bonds | nothing | nothing |
| `ShelterNoiseSystem` | quiet state | nothing | nothing |
| `CaregivingSystem` | family | nothing | nothing |
| `DutyRoster` | shifts | nothing | nothing |
| `TextileSystem` | cloth | nothing | nothing |
| `ShelterWorkshopSystem` | builds | nothing | nothing |
| `StandingRecord` | records | records | nothing |

---

## 38. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`quarter_rooms.json`** — `room_id`, `display_name`, `beds`, `privacy_standard`,
`shift_zone`, `family_bay`, `tags[]`.

**`quarter_privacy.json`** — `standard_id`, `build`, `cost`, `fatigue_relief`,
`build_days`, `tags[]`.

**`quarter_mediation.json`** — `mediator_id`, `skill`, `session_minutes`,
`room_id`, `follow_up_days`, `tags[]`.

**`quarter_agreements.json`** — `agreement_id`, `parties[]`, `terms`,
`day`, `follow_up_day`, `status`, `keeper_id`, `tags[]`.

**`quarter_rules.json`** — `rule_id`, `text`, `scope`, `posted_where[]`,
`revision_day`, `tags[]`.

**`quarter_shared.json`** — `space_id`, `upkeep_id`, `rota`, `standard`,
`check_cadence`, `tags[]`.

**`quarter_neighborhood.json`** — `corridor_id`, `block`, `meeting_cadence`,
`exchange`, `keeper_id`, `tags[]`.

**`quarter_reviews.json`** — `review_id`, `quarter`, `causes[]`,
`rules_changed`, `survey_summary`, `stone_mark`, `tags[]`.

**`quarter_records.json`** — `incident_id`, `event_id`, `cause`, `fix`,
`follow_up_day`, `resolved`, `trend_note`, `tags[]`.

**`quarter_events_expansion.json`** — `id`, `display_name`, `room_tags[]`,
`minimum_occupants`, `cooldown_days`, `outcomes[]`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid room or event references, or out-of-range
numbers.

---

## 39. APPENDIX Y — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Average privacy fatigue | crowding health | Privacy |
| Solitary rest taken | relief | Privacy |
| Incidents per quarter | friction load | Records |
| Mediations held | repair | Mediation |
| Agreements kept | follow-through | Agreements |
| Rules posted | clarity | Rules |
| Rules revised | fairness | Reviews |
| Shared rooms with owners | upkeep | Shared |
| Block meetings held | neighborhood | Neighborhood |
| Stone marks | memory | Reviews |

Telemetry is diagnostic only; it never gates content and never becomes a
score against a resident.

---

## 40. APPENDIX Z — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive inside `shelter_social_dynamics`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows fatigue falling and friction with no names recorded.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No rating, surveillance, or forced-reconciliation content exists.

---

## 41. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. Can a resident trade beds with another resident without the steward?
2. Does privacy fatigue ever force a medical referral, and to whom?
3. Can a house rule be suspended in a crisis, and who signs that?
4. Is an agreement ever voided, and what happens then?
5. Do children get a voice in block meetings, and from what age?
6. Can a corridor opt out of the exchange shelf entirely?
7. What happens to an agreement when one participant dies?
8. Does the quarter ever ask a resident to leave a room for safety, and how
   is that different from eviction?

None of these may be decided unilaterally; each changes tone and balance.

---

## 42. APPENDIX AB — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Children in shared spaces |
| 1 | 17 The Long Evening | Elders' alcoves and day zones |
| 2 | 20 The Quiet Hand | Trust between neighbors |
| 3 | 26 The Common Table | Mess hall culture |
| 3 | 24 The Long Goodbye | Memorial plaques and vigils |
| 4 | 27 The Thread | Curtains, felt, and bedding |
| 4 | 48 The Pastime (Wave 8) | Shared rooms and evenings |
| 5 | 36 The Watch | Shift-change corridors |
| 6 | 37 The Quickening | Family bays and infants |
| 6 | 41 The Quiet | Quiet hours and sleep |
| 7 | 44 The Outpost | Quarter life at remote sites |
| 7 | 46 The Long Change | Rooms that change with the years |
| 8 | 47 The Brigade | Corridor clear and fire routes |
| 8 | 50 The Vault | Quiet reading spaces |
| 9 | 54 The Uninvited | Dorm bedding checks |
| 9 | 56 The Calendar | Block meetings and feast days |

Each hook is additive. The Quarter can ship alone, and every other expansion
can ship without it.

---

## 43. APPENDIX AC — ENDING PROSE SKETCHES

**The Quiet House.** Privacy is standard, friction falls, and the night-shift
sleeper gets seven hours for the first time in a year.

**The Fair Share.** Assignments, rotas, and rules are consultative and
revisable, and the quarter has a culture of its own.

**The Agreement Wall.** Mediation ends in posted promises with follow-up
dates, and the wall becomes a small honest monument to living together.

**The Neighborhood.** Corridors run their own exchanges, meetings, and mutual
aid, and the shelter discovers it has streets.

**The Doors That Close.** Every dorm gets a door that closes, and the shelter
learns that privacy is not distance but respect with hinges.

**Fade.** A corridor at shift change, a curtain drawn, a shared table with two
chairs left together, and a rule board with a new signature on it.

---

## 44. APPENDIX AD — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Social credit | surveillance | no scores |
| Forced friendship | cruelty | distance valid |
| Punishment authority | justice overlap | referral |
| Noise takeover | authority break | quiet owner |
| Relationship rewrite | duplication | read and record |
| Rules without posting | unfairness | post and revise |
| Anonymous blame | cowardice | causes, never names |
| Perpetual disputes | grind | mediation with dates |
| Managed corners | crowding | let the corner manage itself |
| No stone | amnesia | year and one number |

The list exists because communal living is easy to write as either a soap
opera or a panopticon. The expansion's rule is that the quarter is small,
revisable, and private, and that its highest achievement is a boring corridor.

---

## 45. APPENDIX AE — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Rooms | 24 | 3,500 |
| Privacy standards | 12 | 2,500 |
| Mediators | 8 | 2,000 |
| Agreements | 20 | 4,000 |
| House rules | 12 | 3,000 |
| Shared spaces | 12 | 2,500 |
| Neighborhoods | 8 | 2,000 |
| Reviews | 8 | 2,500 |
| Incidents | 12 | 4,000 |
| Events (additive) | 16 | 4,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~61,500** |

---

## 46. APPENDIX AF — FIRST QUARTER YEAR

| Month | Focus | Milestone |
|---|---|---|
| 1 | Board | assignments with reasons |
| 2 | Curtains | sleep bought with cloth |
| 3 | Corner | quiet chair occupied |
| 4 | Rules | posted and revised |
| 5 | Incident | case logged |
| 6 | Agreement | first card on the wall |
| 7 | Rota | fairness restored |
| 8 | Bay | child sleeps through |
| 9 | Shed | hammer returns |
| 10 | Wall | night shift protected |
| 11 | Survey | causes named |
| 12 | Stone | year read aloud |

Twelve months from a redrawn board to a cut stone, and the order is the arc:
privacy before rules, rules before mediation, mediation before reviews. A
quarter that writes its rules before it builds its curtains has confused a
complaint with a plan.

---

## 47. APPENDIX AG — RESIDENT SURVEY TABLE

| # | Question | Top answer | Share | Action |
|---|---|---|---|---|
| 1 | Loudest place | north corridor | 34% | sound wall |
| 2 | Hardest hour | 05-07 | 41% | shift corner |
| 3 | Most wanted | quiet corner | 52% | second corner |
| 4 | Unfairest thing | wash rota | 28% | printed rotation |
| 5 | Best change | curtains | 61% | standard build |
| 6 | Worst feeling | no door | 22% | family bays |
| 7 | Most used | common room | 57% | daily upkeep |
| 8 | Least used | study table | 9% | repurpose |

Eight anonymous survey results with shares and actions, and the fourth row is
the one that changed a rule: a rota that quietly added turns to one corridor
for four months, found by asking instead of inspecting. The survey's real
value is in the least-used row: the quarter learns it built a study table
nobody wanted and turns it into a second quiet corner without ceremony.

---

## 48. APPENDIX AH — HOUSE RULE REVISION TABLE

| # | Rule | First wording | Revision | Year |
|---|---|---|---|---|
| 1 | Quiet hours | after 22:00 | after 22:00, night shift excepted | 1 |
| 2 | Boots | no boots inside | no boots past the mat | 1 |
| 3 | Corridor | keep clear | nothing under 60 cm | 2 |
| 4 | Tools | return them | return in 2 days | 2 |
| 5 | Rota | rotate | rotate weekly, printed | 2 |
| 6 | Lamps | hooded at night | hooded after 23:00 | 2 |
| 7 | Drying | by turn | by turn, named on the board | 2 |
| 8 | Food | no food in dorms | no food in dorms, sealed water ok | 4 |

Eight rules with first wording and revision, and the eighth row is the
quarter's best evidence of a working system: a rule that changed once in four
years and changed because a review found it was wrong, in writing, with a
date, which is the whole difference between a house rule and a habit that has
gone unexamined.

---

## 49. APPENDIX AI — QUARTER COVENANT

| Clause | Promise |
|---|---|
| Reason | Every bed has a written reason, and every request is heard |
| Privacy | Curtains, corners, and hours are standards, not favors |
| Logged | Friction is recorded by cause and never by name |
| Offered | Mediation is offered; distance is respected |
| Written | Agreements live in the participants' own words |
| Posted | No rule is enforced that is not posted |
| Revised | Reviews change rules rather than blaming people |
| Owned | Every shared room has a name and a keeper |
| Unscored | No resident is ever rated, ranked, or watched |
| Kept | The stone records the year and one number |

The quarter covenant is the expansion's first-class design object, posted at
both ends of every corridor and signed by whoever keeps the assignment board.
Its ninth clause is the one the plan was designed around: a shelter's social
system can either measure people or shelter them, and this one chooses.

---

## 50. APPENDIX AJ — QUARTER SUCCESSION TABLE

| Role | First | Successor | Handover |
|---|---|---|---|
| Steward | Vesper | Sil | one board cycle |
| Mediator | Winn | Nia | one full case |
| Privacy | Dory | Bett | one curtain build |
| Dorm lead | Sil | apprentice | one move day |
| Neighborhood | Nia | Tarny | one block meeting |
| Common room | Ambrose | Bett | one week of sweeps |
| Housekeeper | Bett | Sil | one rota cycle |
| Night corridor | Tarny | night deputy | one shift week |

The succession table is measured in board cycles, cases, and rotas, and the
mediator's handover takes a full case because mediating is learned by sitting
in the second chair and writing the agreement out in someone else's words.
The night corridor's successor is a deputy rather than a person, because the
night shift elects its own voice and the table respects that.

---

## 51. APPENDIX AK — FRICTION CAUSE TABLE

| # | Cause | Where | Fix | Prevented by |
|---|---|---|---|---|
| 1 | Night shift noise | north corridor | curtain, felt | sound wall |
| 2 | No private hour | all dorms | corner, hours | privacy standard |
| 3 | Unfair rota | washroom row | printed rotation | quarterly review |
| 4 | Borrowed tools | shed | ledger, norm | labeled return |
| 5 | Corridor storage | all corridors | 60 cm rule | shelf limits |
| 6 | Lamp glare | night corridor | hoods | hood rule |
| 7 | Drying queue | drying room | line turn | named board |
| 8 | Breakfast crush | mess | shift priority | first-access rule |
| 9 | Food in dorms | all dorms | sealed rule | posted rule |
| 10 | Family crowding | mixed dorms | family bays | bay doors |

Ten causes with fixes and preventions, and the table is the expansion's
evidence that friction is almost never about people: nine of the ten causes
are spatial, scheduled, or procedural, and the tenth is about a room that was
never designed for a family. The quarter's whole method is to fix the first
nine with boards and curtains and the tenth with a door.

---

## 52. APPENDIX AL — NIGHT SHIFT TABLE

| # | Need | Provided by | Checked by | Note |
|---|---|---|---|---|
| 1 | Dark at 07 | curtains | Tarny | heavy cloth |
| 2 | Quiet at 08 | corridor rule | Sil | boot mat |
| 3 | Breakfast kept | mess hold | Kerr tie | covered plate |
| 4 | Shower window | rota split | Bett | 30 minutes |
| 5 | Laundry turn | rota | Bett | night slot |
| 6 | Lamp shade | hoods | Tarny | after 23 |
| 7 | Warm meal | kitchen hold | Otta tie | low heat |
| 8 | Shift voice | block meeting | Tarny | monthly |

Eight provisions for the night shift, and the third row is the one that
matters most in practice: a covered plate in the mess at seven in the morning
is a small institutional kindness that costs nothing and changes someone's
entire week. The table is written from the night shift's own list, which Tarny
brings to the block meeting and which the quarter treats like any other round
of causes and fixes.

---

## 53. CLOSING STATEMENT

ASHFALL already models communal friction with unusual care: room-based social
events with outcomes, morale and relationship deltas, mediation flags, a
privacy-fatigue permille that rises with crowding and falls with solitary rest,
incident records, cooldowns, and eight authored events that include a midnight
bunk disturbance and a returned scout's decompression. What it lacks is the
practice of living together: assignments with reasons, buildable privacy,
posted rules, mediation that ends in an agreement, shared-space rotas, corridor
neighborhoods, and quarterly reviews that change one thing at a time. The
Quarter adds that practice and refuses the genre's temptations — no ratings, no
surveillance, no forced friendships — and leaves the shelter with a rule board
somebody signed and a quiet corner that is always occupied.

> Wave 9 note: this plan is one of five Wave 9 expansion bibles (52–56). Each is
> self-contained; none requires another to ship. The shared Wave 9 index lives
> at `docs/expansions/wave9/WAVE9_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `ShelterSocialDynamicsSystem` (`SystemId` =
> "shelter_social_dynamics", `SocialOutcome`, `SocialEventDefinition`,
> `SurvivorPrivacyProfile` with `PrivacyFatiguePermille` 0..1000 and
> `LastSolitaryRestDay`, `SocialIncidentRecord` with `IsMediated`/`MediatorId`/
> `Resolved`, `ShelterSocialSave`, `BindMediatorSkillProvider`, `LoadCatalog`,
> `RegisterSurvivorRoom`, `EvaluateRoomDynamics`, events
> `OnIncidentTriggered` / `OnIncidentMediated` / `OnSocialStateChanged`),
> `ShelterSocialSaveStore` under `shelter_social_dynamics`,
> `ShelterSocialPanel`, and `shelter_social_events.json` (7,377 B, eight
> events: midnight bunk noise friction, personal space transgression, solitary
> recovery, evening mess hall gathering, ration distribution debate, memorial
> plaque remembrance, workbench mutual assistance, returned scout
> decompression).