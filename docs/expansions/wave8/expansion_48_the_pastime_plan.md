# ASHFALL — Expansion 48 Design Bible
# THE PASTIME
### Wave 8 · Hobbies, Clubs, Games, Music, Craft Circles, Tournaments, and the Evening Worth Having

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Recreation` (`SurvivorDowntimeSystem`), `Ashfall.Core.Survivors` (`NeedsSystem`), `Ashfall.Core.Inventory`
**Proposed host owner:** `PastimeHostSession` (extends `RecreationSaveStore` + `SurvivorDowntimeSystem`)
**Existing save sections:** `recreation` (`recreation_save.json`, `RecreationState`)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no recreation-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has a working downtime system. `SurvivorDowntimeSystem` defines
`HobbyDef` (`hobby_id`, `display_name`, `duration_hours` default 2,
`base_stress_relief` default 15, `morale_effect` default 5,
`required_item_ids`, `optional_item_ids`, `required_room_tags`,
`compatible_trait_tags`, `incompatible_trait_tags`, `social_min`,
`social_max`, `brawl_risk`, `output_item_id`, `tags`),
`ActiveHobbySession` (`sessionId`, `hobbyId`, `roomId`, `participantIds`,
`startedDay`, `isFinished`), `SurvivorHobbyProfile` (`survivorId`,
`favoriteHobbyId`, `skillLevel`, `skillXp`, `totalSessionsCompleted`,
`stressRelievedTotal`), and `RecreationState` (`schemaVersion`,
`activeSessions`, `profiles`, `sessionHistory`). The API is live:
`RegisterHobby`, `LoadCatalog`, `GetHobbyCatalog`, `GetHobby`,
`GetOrCreateProfile`, `StartSession` (validates unknown hobby, participant
bounds, and required items), `CompleteSession` (applies relief and calls
`NeedsSystem.Modify` for morale; a brawl costs both participants five morale),
`TickDay`, `CaptureState`, and `RestoreState`, with events `OnHobbyStarted`,
`OnHobbyCompleted`, and `OnHobbyBrawl`. The save section `recreation` and the
`SurvivorDowntimePanel` already exist, along with `RecreationSaveStore`.

What does not exist: content. `recreation.json` holds exactly six hobbies —
whittling (outputs `item_carved_figurine`), guitar, harmonica, card games,
sketching (outputs `item_wasteland_sketch`), and storytelling. There are no
clubs, no leagues, no tournaments, no ensembles, no reading circles, no field
days, no hobby materials beyond two output items, no venues, no seasonal
patterns, and no records of what the shelter has made together.

**The Pastime** fills that system with a culture of leisure: the clubs people
join, the games they play, the songs they practice, the things they make, and
the evenings that make a hard week survivable. It extends the live system and
never duplicates a morale, relationship, ceremony, or food authority.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| `SurvivorDowntimeSystem` | Hobbies, sessions, profiles | Extends with content and clubs |
| `NeedsSystem` | Needs and morale | Uses `Modify` only; no second needs |
| `CeremonySystem` | Ceremonies, festivals, truces | Never owns festivals; pastimes may prepare for them |
| 26 The Common Table | Meals and food culture | Never owns food events; may use mess room |
| 41 The Quiet (Wave 6) | Sleep, rest, noise, quiet hours | Books rooms through it; never overrides quiet |
| 35 The Habit (Wave 5) | Substances and dependency | No substance content; pastime spaces stay neutral |
| `SurvivorRelationsSystem` | Bonds and affinity | Club rosters are not relationship ledgers |
| `SkillProgressionSystem` | Professional skills | Hobby skill stays local to profiles |
| `Library` / 43 The Question | Books and knowledge | Reading circles borrow, never own texts |
| `PressHostSession` (Wave 4) | Printing | Song sheets and score cards are print jobs |
| `Chronicle` / 03 Records | Records | Awards and records filed, never scored |
| `WeatherSystem` (Wave 5) | Weather | Field days respect it; never change it |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The work is done, the meal is eaten, and there are two hours before the lights
go down. For three years the shelter has filled that gap with six hobbies and a
card deck with a missing queen.

**The Pastime** is the expansion about what people do with the evening: clubs
and leagues, songs and stories, craft circles and reading circles, field days
and tournaments, and the records of handmade things that outlast the people who
made them. It is the expansion about rest as infrastructure.

### 1.2 The five loops it adds

```
  Choose ──► Gather ──► Play ──► Make ──► Record
     │          │          │        │         │
     ▼          ▼          ▼        ▼         ▼
   Hobbies,  Clubs,     Games,   Crafts,   Cabinets,
   seasons   venues     music    gifts     records
                                        │
                                        ▼
                          Teach ──► Include ──► Repeat
```

### 1.3 What the player manages

1. **Hobbies.** What people do alone and together.
2. **Materials.** Tools, strings, reeds, cards, books, chalk, balls.
3. **Venues.** Rooms, tables, stages, fields, and their schedules.
4. **Clubs.** Who meets, when, and what they do.
5. **Leagues and tournaments.** Rules, rounds, fairness, rest days.
6. **Ensembles.** Practice, performance, and instrument care.
7. **Craft circles.** Shared materials and handmade outputs.
8. **Reading circles.** Borrowed books and read-aloud practice.
9. **Field days.** Outdoor play, weather windows, and games for every body.
10. **Records.** Cabinets of made things, names, and years.

### 1.4 What it is not

- Not a second morale, relationship, ceremony, or food system.
- Not gambling; no currency changes hands over games.
- Not competition-as-cruelty; no humiliation, no shaming, no last-place jokes.
- Not productivity theatre; rest does not have to earn its place.
- Not substance content; the habit expansion owns that.
- Not a child-worker pipeline; children play, nothing more.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Recreation/SurvivorDowntimeSystem.cs` | Hobbies, sessions, profiles | `LIVE` |
| `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` | Morale and stress | `LIVE` |
| `src/Host/RecreationSaveStore.cs` | `recreation` persistence | `LIVE` |
| `src/UI/SurvivorDowntimePanel.cs` | Session commands | `LIVE` |
| `Assets/Ashfall.Core/Narrative/CeremonySystem.cs` | Ceremonies (separate owner) | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterNoiseSystem.cs` | Noise and quiet hours | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `recreation.json` | 3,726 B | **6 hobbies**; two output items |
| Club, league, venue, material catalogs | absent | confirmed none |
| Song, story, game, field-day catalogs | absent | confirmed none |
| `ceremonies.json` | 4,382 B | separate owner, not touched |

### 2.3 Confirmed gaps

- **GAP-48-1 — Six hobbies for a whole shelter.**
- **GAP-48-2 — No hobby materials or tools.**
- **GAP-48-3 — No venues or schedules.**
- **GAP-48-4 — No clubs or recurring groups.**
- **GAP-48-5 — No leagues or tournaments.**
- **GAP-48-6 — No ensembles or performances.**
- **GAP-48-7 — No craft or reading circles.**
- **GAP-48-8 — No field days or outdoor play.**
- **GAP-48-9 — No records of made things.**
- **GAP-48-10 — Hobby skill exists mechanically but has no progression content.**

### 2.4 Non-duplication statement

This expansion will **not** add a second morale, stress, relationship,
ceremony, food, sleep, noise, or substance system. It extends
`SurvivorDowntimeSystem` with catalogs and commands, routes every morale and
stress effect through `NeedsSystem.Modify`, books rooms through the existing
noise and room owners, borrows books from their owners, and files records with
`StandingRecord`. All new state is additive inside `RecreationState`. No new
save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Rest is not a reward.** An evening belongs to the people who
worked the day, without conditions.

**Pillar 2 — Play makes culture.** Songs, games, and handmade things are how a
shelter becomes a place with a name.

**Pillar 3 — Everyone plays somehow.** There is no single correct body, age, or
temperament for a pastime.

**Pillar 4 — Competition is for the game, never the person.** Rules, fairness,
and rest days are part of the equipment.

**Pillar 5 — Records honor makers.** A cabinet of things, with names and years,
is a kind of history.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Clubs | Recurring, low-key, welcoming | Cliques and gatekeeping |
| Tournaments | Rules, rounds, handicaps | Crushing a novice |
| Music | Practice, mistakes, improvement | Perfection culture |
| Crafts | Gifts and keepsakes | Production quotas |
| Reading | Read-aloud and discussion | Literacy shaming |
| Field days | Weather, fans, every body | Records or ruin |
| Records | Names and years | Leaderboards |
| Rest | Honest idleness | Guilt or shame |

### 3.3 Content limits

- No gambling, betting, or currency wagers of any kind.
- No children in adult competitions; child leagues are cooperative.
- No hazing, initiation cruelty, or mockery of ability.
- No substance content; the habit expansion owns dependency.
- No literacy shaming; read-aloud is a shared pleasure.
- No productivity framing; hobbies produce items, not quotas.
- No new save section.

---

## 4. THE PASTIME WORLD

### 4.1 Interior rooms

- **`room_card_room`** — the table with the missing-queen deck and its successor.
- **`room_music_room`** — two chairs, a guitar, a wall of quiet.
- **`room_craft_room`** — benches, shared tools, a shelf for works in progress.
- **`room_reading_room`** — borrowed books and a lamp for read-aloud.
- **`room_club_room`** — the rotating room clubs book for evenings.
- **`room_stage_hall`** — the mess hall after the tables move.
- **`room_record_cabinet`** — the shelf of made things with names and years.
- **`room_game_store`** — boards, pieces, cards, rubber balls, chalk.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_field_day` | The Old Field | 2 | Outdoor games and meets |
| `loc_river_swim` | The Quiet Bend | 2 | Swimming and river days |
| `loc_kite_hill` | Kite Hill | 1 | Kites and wind |
| `loc_stone_court` | The Stone Court | 2 | Ball games and nets |
| `loc_sled_slope` | The Sled Slope | 2 | Winter play |
| `loc_sketch_bluff` | The Sketch Bluff | 1 | Outdoor drawing |
| `loc_night_fire` | The Story Fire | 2 | Storytelling aloud |
| `loc_fishing_jetty` | The Quiet Jetty | 2 | Patience and hooks |
| `loc_orchard_game` | The Orchard Rows | 1 | Hiding, chasing, picking |
| `loc_record_wall` | The Record Wall | 1 | Names, years, and works |

All locations require valid item references and scanner registration.

### 4.3 The weekly rhythm

Two evenings of open hobbies, one club night, one practice night, one quiet
night with no obligations, and Sundays for field days when weather allows. The
expansion's clock is the week, not the day.

---

## 5. MAIN STORYLINE — "WHAT THE EVENING IS FOR"

### 5.1 Central conflict

It is the first spring after a bad winter, and the shelter is tired in a way
that sleep does not fix. **Dovie Amberly** starts playing guitar in the mess
hall at eight because the alternative is a room where nobody speaks.
**Ruel Tand** produces a deck of cards and a rule sheet and announces a league.
**Mina Fern** wants a craft column that does not exist — three benches, a
mortise set, and the right to make things nobody needs. **Nella Kitt** wants
clubs, because the shelter has ninety-four people and most of them do not know
each other's names. **Siss Dane**, fourteen, wants a tournament that children
can enter without being beaten by adults in the first round.

The fight is not about whether to play. It is about the rooms, the quiet hours,
the work rota, and whether the shelter can justify three benches and a night
off when the cistern needs painting. **Prue Ivett** proposes the compromise
that becomes the expansion's spine: a pastime calendar that borrows the mess
hall on fixed evenings, respects quiet hours, and treats rest as scheduled
maintenance for people. Then the first tournament happens, an adult loses
badly to a child in a game of skill, and everyone discovers what kind of
community they are.

The expansion's question: **what is leisure for when it has to be earned, and
must it be earned?**

### 5.2 Theme (unspoken)

**A shelter survives on work; a community survives on Tuesday evenings.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_music_dovie_amberly` | Dovie Amberly | Music | Evenings and ensembles |
| `npc_games_ruel_tand` | Ruel Tand | Games | Leagues and rules |
| `npc_crafts_mina_fern` | Mina Fern | Crafts | Benches and gifts |
| `npc_stories_hob_ottery` | Hob Ottery | Stories | Read-aloud and tall tales |
| `npc_clubs_nella_kitt` | Nella Kitt | Clubs | Membership and nights |
| `npc_youth_siss_dane` | Siss Dane | Youth | Child leagues and play |
| `npc_sport_bailey_sollo` | Bailey Sollo | Field days | Sports and weather |
| `npc_tournaments_prue_ivett` | Prue Ivett | Organizer | Calendars and fairness |

### 5.4 Story beats (15)

1. **The Flat Evening.** The shelter sits in silence and someone plays.
2. **The Deck.** A card league forms and plays three nights a week.
3. **The Room Fight.** The mess hall, the quiet hours, and the calendar.
4. **The Benches.** Mina gets three benches and a materials shelf.
5. **The Clubs.** Six clubs sign their first members on one night.
6. **The First Tournament.** Rules are written and a child enters.
7. **The Song.** Dovie teaches a song that the shelter keeps.
8. **The Field Day.** Weather grants one good afternoon.
9. **The Rain.** A planned day is ruined and the alternative works.
10. **The Bench Sitter.** A resident who cannot play finds a role.
11. **The Gift Table.** Craft outputs are given, not sold.
12. **The Reading Circle.** Hob reads aloud and nobody is ashamed.
13. **The Second Tournament.** Handicaps make the game worth playing.
14. **The Record Wall.** The first year of made things is hung up.
15. **What the Evening Is For.** The calendar becomes ordinary.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Calendar | fixed / flexible / none | order vs. spontaneity |
| Venues | shared rooms / dedicated / outdoor bias | room politics |
| Clubs | many small / few large / mixed | social shape |
| Competition | leagues / friendly only / none | drive vs. calm |
| Handicaps | full / light / none | fairness |
| Records | cabinet / wall / none | memory vs. modesty |
| Rest night | protected / optional / none | rest as policy |
| Final | culture as institution / habit / private | identity |

### 5.6 Endings (5 + fade)

1. **The Common Evening** — the shelter has a weekly rhythm of play, and the
   work week is measured differently.
2. **The Handmade Year** — the record cabinet fills with gifts, keepsakes, and
   names, and the shelter can date its own culture.
3. **The Fair Field** — leagues and handicaps make competition a pleasure
   instead of a hierarchy.
4. **The Protected Night** — a rest night is written into the rota and the
   shelter keeps it even in a hard week.
5. **The Quiet Table** — some residents choose solo pastimes, and the shelter
   honors that as play too.
6. **Fade** — a mess hall at eight in the evening, cards on a table, someone
   tuning a guitar, and a child winning at dominoes.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_pastime_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_pastime_flat_evening`, `quest_pastime_deck`, `quest_pastime_room`,
`quest_pastime_benches`, `quest_pastime_clubs`, `quest_pastime_tournament`,
`quest_pastime_song`, `quest_pastime_field_day`, `quest_pastime_rain`,
`quest_pastime_bench_sitter`, `quest_pastime_gift_table`,
`quest_pastime_reading`, `quest_pastime_second_tournament`,
`quest_pastime_record_wall`, `quest_pastime_what_evening_is_for`.

### 6.2 Side quests (30)

**Hobbies (5)**
- `quest_pastime_stock_hobbies` — hobby shelves stocked
- `quest_pastime_new_hobby` — new hobby taught
- `quest_pastime_solo_corner` — solo space kept
- `quest_pastime_beginner` — first session guided
- `quest_pastime_repair` — tool repaired

**Materials (5)**
- `quest_pastime_strings` — strings replaced
- `quest_pastime_reeds` — reeds and small parts
- `quest_pastime_cards` — deck remade
- `quest_pastime_chalk` — charcoal and paper
- `quest_pastime_boards` — boards and pieces

**Clubs (5)**
- `quest_pastime_club_night` — night held
- `quest_pastime_club_room` — room booked
- `quest_pastime_club_officer` — officers rotate
- `quest_pastime_club_welcome` — new member welcomed
- `quest_pastime_club_sleep` — quiet-hours kept

**Games (5)**
- `quest_pastime_rules` — rule sheet written
- `quest_pastime_handicap` — handicap tested
- `quest_pastime_rest_day` — rest day kept
- `quest_pastime_child_league` — child league runs
- `quest_pastime_fairness` — a disputed call settled

**Music and stories (5)**
- `quest_pastime_practice` — practice logged
- `quest_pastime_song_teach` — song taught to three
- `quest_pastime_performance` — evening performed
- `quest_pastime_read_aloud` — read-aloud held
- `quest_pastime_story_fire` — fire stories told

**Records and purpose (4)**
-  `quest_pastime_record_shelf` — records shelf built
-  `quest_pastime_nameplate` — nameplate made
-  `quest_pastime_gift` — a gift given
-  `quest_pastime_role` — a role found for the bench sitter

**Field days (1)**
-  `quest_pastime_season_meet` — seasonal meet held

### 6.3 Repeatable quests (8)

`quest_pastime_repeat_hobby`, `quest_pastime_repeat_club`,
`quest_pastime_repeat_practice`, `quest_pastime_repeat_game`,
`quest_pastime_repeat_craft`, `quest_pastime_repeat_read`,
`quest_pastime_repeat_field`, `quest_pastime_repeat_record`.

### 6.4 Dynamic hooks

Live events (`OnHobbyStarted`, `OnHobbyCompleted`, `OnHobbyBrawl`, morale
changes, weather seasons, quiet-hour state, injuries, births, deaths) attach
authored follow-ups through existing seams. No new event bus.

### 6.5 Constraints

- Morale and stress only through `NeedsSystem.Modify`.
- Relationships stay with `SurvivorRelationsSystem`; club rosters are local.
- Ceremonies stay with `CeremonySystem`.
- Rest and noise stay with the quiet owners.
- Reads borrow books; the library and archive owners remain authoritative.
- No currency, no gambling, no substance content.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `HobbyCatalogExpansion` (extend `SurvivorDowntimeSystem`)

**Owns:** additional hobbies across solitary, social, craft, music, physical,
and quiet categories. **Consumes:** items, room tags, traits. **Data:**
`recreation.json` (additive rows). **Rules:** every hobby maps to a real item
need or a real room tag; outputs are real items; no hobby requires a missing
authority.

### 7.2 `ClubSystem` (new, thin, `Ashfall.Core.Recreation`)

**Owns:** clubs, membership lists, meeting cadence, officers, and rooms.
**Consumes:** `SurvivorDowntimeSystem` sessions, room bookings, quiet hours.
**Data:** `pastime_clubs.json`. **Rules:** clubs meet on a cadence; officers
rotate yearly; a club that stops meeting is archived, not punished; membership
never affects work rota.

### 7.3 `LeagueSystem` (new, `Ashfall.Core.Recreation`)

**Owns:** leagues, tournament brackets, rules, handicap tables, standings,
rest days, and dispute resolution. **Consumes:** hobby sessions, profiles.
**Data:** `pastime_tournaments.json`, `pastime_rules.json`. **Rules:** every
match has an agreed handicap; children have their own cooperative league;
standings are records, not rankings of worth; no wagers.

### 7.4 `EnsembleSystem` (new, thin, `Ashfall.Core.Recreation`)

**Owns:** ensembles, practice schedules, performance evenings, and instrument
maintenance requests. **Consumes:** music hobbies, workshop repair owner,
press for sheets. **Data:** `pastime_songs.json`, `pastime_ensembles.json`.
**Rules:** instruments are maintained through the workshop owner; performances
are public and free; practice respects quiet hours.

### 7.5 `CraftCircleSystem` (new, `Ashfall.Core.Recreation`)

**Owns:** group craft sessions, shared material pools, works in progress, and
gift records. **Consumes:** inventory, workshop tools, output items. **Data:**
`pastime_crafts.json`, `pastime_materials.json`. **Rules:** shared materials
are pooled, not owned by a bench; works in progress have a shelf; outputs are
gifts, keepsakes, or usable items, never currency.

### 7.6 `ReadingCircleSystem` (new, thin, `Ashfall.Core.Recreation`)

**Owns:** reading circles, borrowed-book schedules, read-aloud evenings, and
discussion notes. **Consumes:** library and archive owners (borrow only).
**Data:** `pastime_reading.json`. **Rules:** books are returned; read-aloud is
a shared pleasure, never a literacy test; no one is graded.

### 7.7 `FieldDaySystem` (new, thin, `Ashfall.Core.Recreation`)

**Owns:** field days, seasonal meets, game rules, weather windows, and
inclusive variants. **Consumes:** `WeatherSystem` (read-only), locations.
**Data:** `pastime_field_days.json`. **Rules:** every game has a seated,
low-mobility, and no-contact variant; weather can postpone but not cancel the
spirit of the day; nobody is picked last.

### 7.8 `PastimeRecordSystem` (new, thin, `Ashfall.Core.Recreation`)

**Owns:** the record cabinet and wall: made things, names, years, and songs
learned. **Consumes:** session history, craft outputs, standings as records.
**Data:** `pastime_records.json`. Records through `StandingRecord`. **Rules:**
records name makers and dates; standings are kept as history, not pressure;
the cabinet is not a leaderboard.

### 7.9 Systems explicitly not added

- No second morale, stress, or needs system.
- No second relationship, ceremony, or food system.
- No gambling, currency, or wager mechanics.
- No substance content.
- No productivity quotas.
- No child labor.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `recreation.json` (extend, additive hobbies)

```json
{
  "schema_version": 1,
  "hobbies": [
    {
      "hobby_id": "hobby_chess",
      "display_name": "Tabletop Chess",
      "duration_hours": 2,
      "base_stress_relief": 18.0,
      "morale_effect": 7,
      "required_item_ids": ["item_chess_set"],
      "optional_item_ids": [],
      "required_room_tags": ["common", "living_quarters"],
      "compatible_trait_tags": ["patient", "strategic"],
      "incompatible_trait_tags": ["hyperactive"],
      "social_min": 2,
      "social_max": 2,
      "brawl_risk": 0.0,
      "output_item_id": "",
      "tags": ["table", "quiet", "two-player"]
    }
  ]
}
```

### 8.2 `pastime_clubs.json` (new)

Clubs: club id, name, kind, cadence, room tags, social range, officer roles.

### 8.3 `pastime_tournaments.json` (new)

Tournaments: format, rounds, handicaps, rest days, eligibility, awards.

### 8.4 `pastime_rules.json` (new)

Rules: game, victory condition, fair play rules, dispute process, variants.

### 8.5 `pastime_songs.json` (new)

Songs: title, kind, parts, learned-by, performance note, sheet print ref.

### 8.6 `pastime_crafts.json` (new)

Crafts: craft, materials, bench need, output items, gift use.

### 8.7 `pastime_materials.json` (new)

Materials: item id, craft use, source, pool or private, consumable.

### 8.8 `pastime_reading.json` (new)

Reading: circle id, book ref, cadence, read-aloud state, discussion note.

### 8.9 `pastime_field_days.json` (new)

Field days: season, games, variants, weather rules, spectator notes.

### 8.10 `pastime_records.json` (new)

Records: record id, maker, kind, year, cabinet or wall, story line.

### 8.11 Items

New items appended to `items.json`: `item_chess_set`, `item_dominoes`,
`item_card_deck_standard`, `item_card_deck_handmade`, `item_carving_knife`,
`item_guitar_strings`, `item_harmonica_reeds`, `item_sketch_charcoal`,
`item_story_book`, `item_ball_leather`, `item_kite`, `item_marbles`,
`item_sheet_music`, `item_practice_pad`, `item_token_pouch`,
`item_prize_ribbon`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`RecreationState` remains the live save owner. New sub-objects (clubs,
leagues, ensembles, circles, field days, records) are additive inside it. No
new save section.

### 9.2 State to persist

- Extra hobby catalog bindings and learned hobbies.
- Club rosters, cadence, officers, and meeting history.
- League tables, handicaps, brackets, and rest days.
- Ensemble membership and performance history.
- Craft works in progress and shared material pools.
- Reading circle schedules and borrowed books.
- Field day results and weather postponements.
- Record cabinet entries with maker, year, and kind.

### 9.3 Determinism

- Session outcomes derive from hobby definitions, participants, items, and
  profiles; where a roll is needed it uses the existing live path.
- Brackets and pairings derive from seeds already present; a watchable
  deterministic shuffle is used, never wall-clock.
- Weather postponements read `WeatherSystem` state.
- Craft outputs are item definitions, not random rarity tables.
- Paired replay hashes must match; no `System.Random`.

### 9.4 Migration

Legacy saves load with profiles, sessions, and history intact; no clubs,
leagues, or records exist until started. Favorite hobbies and skill levels
carry forward; a profile with no favorite hobby keeps an empty field.

### 9.5 Checksum

Invariant-culture floats; integer day, count, and year fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `SurvivorDowntimePanel` (extend) | Hobbies and sessions | `PastimeHostSession` |
| `ClubPanel` (new) | Clubs and nights | same |
| `LeaguePanel` (new) | Tournaments and fairness | same |
| `EnsemblePanel` (new) | Practice and performances | same |
| `CraftCirclePanel` (new) | Benches and gifts | same |
| `FieldDayPanel` (new) | Seasonal meets | same |
| `RecordCabinetPanel` (new) | Made things and names | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Hobby lists mark solo, quiet, seated, and low-mobility options clearly.
- The calendar is visible before the week begins; nobody misses a night by
  accident.
- No color-only signals; no time pressure in leisure screens.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Read-aloud content can be shown as text or heard, never tested.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a guitar being tuned, cards being
riffled, a chair pulled to a table, applause in a small room, a ball against a
wall, a page turning. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `SurvivorDowntimeSystem` | Hobbies, sessions, profiles, clubs |
| `NeedsSystem` | Morale and stress via `Modify` |
| `CeremonySystem` | Pastimes may prepare, never own festivals |
| `ShelterNoiseSystem` (Wave 6) | Room bookings and quiet hours |
| `ShelterThermalSystem` | Warm rooms for evening sessions |
| `WeatherSystem` (Wave 5) | Field days and postponements |
| `Library` / 43 The Question | Borrowed books |
| `PressHostSession` (Wave 4) | Song sheets and score cards |
| `ShelterWorkshopSystem` (Wave 6) | Instrument and tool repair |
| `TextileSystem` (Wave 4) | Felt, cloth, and doll materials |
| `KilnworksHostSession` (Wave 4) | Game pieces and tokens |
| `Inventory` | Materials and outputs |
| `StandingRecord` (Exp 03) | Records and awards |
| `MemorialSystem` (Wave 3) | Played-in-memory evenings |
| `ChildDevelopmentSystem` (Wave 6) | Youth leagues and play |
| `EpilogueChronicleBuilder` | Culture lines in the record |
| `FieldGuide` (Plan 20A/28) | Outdoor game safety reading |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm the downtime system, needs owner, save
store, panel, room registry, and quiet-hours owner. Record file:line; change
nothing.

**Phase 1 — Data + validators.** Extend `recreation.json`; author the nine new
catalogs; register validators and scanner.

**Phase 2 — Pure Core.** `ClubSystem`, `LeagueSystem`, `EnsembleSystem`,
`CraftCircleSystem`, `ReadingCircleSystem`, `FieldDaySystem`,
`PastimeRecordSystem`; extend the hobby catalog.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `PastimeHostSession`, focused selftest coverage,
fresh journey from the flat evening to the record wall.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Year-long soak: clubs persist, tournaments stay fair,
rest nights survive hard weeks, records accumulate.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Hobbies (total, incl. existing) | 30 |
| Clubs | 12 |
| Tournaments | 8 |
| Rules sets | 12 |
| Songs | 16 |
| Crafts | 20 |
| Materials | 16 |
| Reading circles | 6 |
| Field days | 8 |
| Records | 24 |
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
| Second morale system | Critical | `NeedsSystem` only |
| Gambling drift | High | No wagers by contract |
| Competition cruelty | High | Handicaps and rules |
| Quiet-hours collisions | Medium | Book through owner |
| Ceremony overlap | Medium | Boundary §0.1 |
| Records as leaderboard | Medium | History framing |
| Grind framing | Medium | No quotas |
| Determinism break | Low | Seeded pairings |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `recreation.json` (hobbies) | 24 new | 4,500 |
| `pastime_clubs.json` | 12 | 2,500 |
| `pastime_tournaments.json` | 8 | 2,000 |
| `pastime_rules.json` | 12 | 2,500 |
| `pastime_songs.json` | 16 | 3,000 |
| `pastime_crafts.json` | 20 | 3,500 |
| `pastime_materials.json` | 16 | 2,500 |
| `pastime_reading.json` | 6 | 1,500 |
| `pastime_field_days.json` | 8 | 2,000 |
| `pastime_records.json` | 24 | 3,500 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~58,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R48-1 | Second morale | Low | Critical | Needs owner |
| R48-2 | Gambling | Low | High | Contract |
| R48-3 | Cruelty | Med | High | Handicaps |
| R48-4 | Quiet collision | Med | Medium | Book through owner |
| R48-5 | Ceremony overlap | Med | Medium | Boundary |
| R48-6 | Leaderboards | Med | Medium | History framing |
| R48-7 | Grind | Med | Medium | No quotas |
| R48-8 | Determinism | Low | High | Seeded pairings |
| R48-9 | Content overrun | Med | Medium | Budget §13 |
| R48-10 | Child labor slip | Low | High | Play only |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **How many hobbies is enough?** Recommended: thirty total, covering solo,
   social, quiet, seated, musical, craft, and physical play.
2. **Do clubs ever affect work rota?** Recommended: no; membership is leisure
   only.
3. **Can tournaments have prizes?** Recommended: ribbons, tokens, and honor
   only; no currency, no goods of economic weight.
4. **Are records mandatory?** Recommended: no; makers may decline and the wall
   records the year without a name.
5. **Does a rest night ever yield to a crisis?** Recommended: yes, explicitly,
   but it must be restored within one month and the reason recorded.

---

## 17. APPENDIX D — HOBBY TABLE (24 NEW HOBBIES)

| # | Hobby | Social | Relief | Morale | Needs | Output |
|---|---|---|---|---|---|---|
| 1 | Chess | 2 | 18 | 7 | chess set | none |
| 2 | Dominoes | 2-4 | 16 | 6 | dominoes | none |
| 3 | Handmade cards | 2-6 | 20 | 8 | handmade deck | deck |
| 4 | Wood carving | 1 | 18 | 6 | carving knife | carving |
| 5 | Stone carving | 1 | 15 | 5 | gouge, stone | carving |
| 6 | Knitting circle | 2-6 | 22 | 8 | yarn, needles | garment |
| 7 | Mending bees | 2-8 | 20 | 8 | thread, patches | mended cloth |
| 8 | Choir practice | 4-10 | 24 | 10 | sheet music | none |
| 9 | Drum circle | 3-8 | 21 | 9 | practice pad | none |
| 10 | Story circle | 3-8 | 22 | 9 | none | story |
| 11 | Tall tales | 2-6 | 17 | 8 | none | story |
| 12 | Poetry reading | 2-8 | 19 | 7 | paper, pencil | poem |
| 13 | Paper folding | 1-2 | 14 | 5 | scrap paper | folded figure |
| 14 | Map doodling | 1 | 13 | 4 | paper, charcoal | doodle map |
| 15 | Flower pressing | 1 | 12 | 4 | paper, press | pressed sheet |
| 16 | Bird watching | 1-2 | 16 | 6 | chart | count note |
| 17 | Star naming | 1-3 | 15 | 5 | none | star log |
| 18 | Running | 1-4 | 20 | 7 | none | stamina |
| 19 | Stretching | 1-6 | 14 | 5 | mat | none |
| 20 | Ball games | 4-10 | 23 | 9 | leather ball | none |
| 21 | Kite flying | 1-3 | 17 | 7 | kite | none |
| 22 | Marbles | 2-6 | 13 | 5 | marbles | token |
| 23 | Swimming | 1-6 | 24 | 9 | none | none |
| 24 | Fire stories | 4-12 | 25 | 10 | none | story |

Twenty-four hobbies added to the six that exist, and the social column is the
design: some need a table, some need a partner, and several need nobody at all.
The relief values stay inside the range the live system already uses, because
this plan is content, not a rebalance, and the quietest hobbies are deliberately
not the weakest ones.

---

## 18. APPENDIX E — CLUB TABLE (12 CLUBS)

| # | Club | Kind | Cadence | Room | Range | Officer |
|---|---|---|---|---|---|---|
| 1 | Card club | games | twice weekly | card room | 2-12 | rotating |
| 2 | Craft column | crafts | weekly | craft room | 3-10 | rotating |
| 3 | Music circle | music | twice weekly | music room | 2-8 | lead |
| 4 | Reading circle | reading | weekly | reading room | 4-12 | host |
| 5 | Runners | sport | daily | loop | 2-10 | pace lead |
| 6 | Ball club | sport | twice weekly | court | 6-16 | captain |
| 7 | Kite club | outdoor | wind days | hill | 2-8 | lead |
| 8 | Mending bee | crafts | weekly | craft room | 4-12 | rotating |
| 9 | Choir | music | weekly | stage | 6-20 | lead |
| 10 | Bird watchers | quiet | weekly | quiet corner | 2-6 | list keeper |
| 11 | Star club | quiet | clear nights | roof walk | 2-6 | list keeper |
| 12 | Story fire | stories | monthly | story fire | 6-30 | host |

Twelve clubs, each with a room, a cadence, and a rotating or lead role. The
club system's whole purpose is in the range column: it exists so that ninety-
four people who do not know each other's names can end a season knowing at
least eleven of them.

---

## 19. APPENDIX F — TOURNAMENT TABLE (8 EVENTS)

| # | Event | Format | Rounds | Handicap | Rest | Award |
|---|---|---|---|---|---|---|
| 1 | Card league | round robin | 6 | cards | weekly | ribbon |
| 2 | Chess ladder | ladder | open | time odds | none | ribbon |
| 3 | Dominoes night | knockout | 4 | first to five | weekly | token |
| 4 | Marble meet | brackets | 5 | target size | none | token |
| 5 | Ball cup | teams | 4 | player count | weekly | ribbon |
| 6 | Foot race | heats | 3 | age bands | weekly | ribbon |
| 7 | Song contest | showcase | 2 | none | none | ribbon |
| 8 | Child league | cooperative | open | none | none | star card |

Eight events, and the handicap column is the ethics: time odds against the
strongest players, smaller targets for the sharpest eyes, age bands for the
races, and a child league that has no loser's bracket because the round robin
crowns participation. Rest days are equipment, not mercy.

---

## 20. APPENDIX G — RULES SHEET TABLE

| # | Game | Win condition | Fair play | Dispute | Variant |
|---|---|---|---|---|---|
| 1 | Rummy | first out | no peeking | replay hand | pair rules |
| 2 | Chess | mate | touch-move | arbiter | clock odds |
| 3 | Dominoes | first out | open hand | check board | five-draw |
| 4 | Marbles | last marble | no thumb | ring reset | target size |
| 5 | Cards by night | best of five | no table talk | deal again | co-op |
| 6 | Ball game | goals | no contact | half replay | seated pass |
| 7 | Foot race | first across | no cutting | re-run | banded |
| 8 | Kite | longest air | no tangling | judged day | smallest kite |
| 9 | Domino tower | tallest | no gluing | measured | widest base |
| 10 | Story duel | judges pick | no cutting off | second round | pair tell |
| 11 | Song circle | none | no mocking | host call | joined chorus |
| 12 | Child games | fun score | everybody plays | host call | no score |

Twelve rule sheets written by the people who use them, and every dispute
process ends in replay rather than punishment. The last two rows are the tone
guardrails: one event with no win condition at all and one where the score is
kept only for love of the game.

---

## 21. APPENDIX H — SONG TABLE (16 SONGS)

| # | Song | Kind | Parts | Learned by | Print |
|---|---|---|---|---|---|
| 1 | Ashfall morning | round | 3 | choir | sheet |
| 2 | The long count | hymn-like | 4 | choir | sheet |
| 3 | Wire and bone | work song | lead + call | crew | sheet |
| 4 | The green door | children | unison | children | card |
| 5 | Nine names | memorial | 2 | all | sheet |
| 6 | Kettle song | kitchen | unison | kitchen | card |
| 7 | North stair | walking | 2 | all | card |
| 8 | The quiet shift | lullaby | 2 | few | sheet |
| 9 | Rain on tin | ballad | lead + 3 | choir | sheet |
| 10 | The long road home | marching | unison | all | sheet |
| 11 | Seed and stone | planting | refrain | growers | card |
| 12 | The lamp and the wall | evening | 3 | choir | sheet |
| 13 | First frost | seasonal | 2 | all | card |
| 14 | The year stone | closing | 4 | all | sheet |
| 15 | Little hands | children | unison | children | card |
| 16 | I knew your name | memorial | lead | few | sheet |

Sixteen songs the shelter makes its own, from work calls to a lullaby for the
quiet shift, and the print column shows the press owner being used rather than
duplicated. The two memorial songs are included deliberately: a community that
sings only happy songs is not telling the truth about itself.

---

## 22. APPENDIX I — CRAFT TABLE (20 CRAFTS)

| # | Craft | Materials | Bench | Output | Use |
|---|---|---|---|---|---|
| 1 | Carved bird | scrap wood | wood | figurine | gift |
| 2 | Carved box | wood | wood | keepsake box | gift |
| 3 | Knitted hat | yarn | none | hat | warm use |
| 4 | Knitted doll | yarn, cloth | none | doll | children |
| 5 | Patched coat | thread, patch | none | mended coat | use |
| 6 | Woven mat | cord, scrap | weaving | mat | rooms |
| 7 | Clay cup | clay | kiln | cup | use |
| 8 | Clay lamp base | clay | kiln | lamp base | rooms |
| 9 | Doll house | wood, cloth | wood | toy house | children |
| 10 | Wooden puzzle | wood | wood | puzzle | children |
| 11 | Handmade deck | card stock | press | deck | games |
| 12 | Bound booklet | paper, thread | none | booklet | writing |
| 13 | Pressed flower card | paper, flower | none | card | gift |
| 14 | Bead token set | clay, cord | kiln | tokens | games |
| 15 | Story quilt | cloth, thread | none | quilt | memorial |
| 16 | Kite frame | wood, paper | wood | kite | play |
| 17 | Sketched portrait | charcoal, paper | none | portrait | keepsake |
| 18 | Star chart | paper, ink | print | chart | club |
| 19 | Song book | paper, cord | press | songbook | choir |
| 20 | Nameplate | wood, paint | wood | nameplate | records |

Twenty crafts, and the use column is the economics: these are gifts, children's
toys, and useful things, never trade goods. The story quilt is the expansion's
quietest monument, because the shelter has burying to do and a memorial owner
to route it through, and a quilt is a way to do it without owning a new kind
of monument.

---

## 23. APPENDIX J — MATERIALS TABLE

| # | Material | Use | Source | Pool | Consumable |
|---|---|---|---|---|---|
| 1 | Scrap wood | carving, frames | workshop | shared | yes |
| 2 | Yarn | knitting | salvage, spinning | shared | yes |
| 3 | Thread | mending, binding | salvage | shared | yes |
| 4 | Cloth scraps | dolls, quilts | textile | shared | yes |
| 5 | Clay | cups, tokens | clay pit | shared | yes |
| 6 | Charcoal | sketching | kiln | shared | yes |
| 7 | Paper | books, cards | press | shared | yes |
| 8 | Ink | charts, books | reagent | shared | yes |
| 9 | Paint drops | nameplates | workshop | shared | yes |
| 10 | Card stock | decks | press | shared | yes |
| 11 | Cord | kites, nets | cordage | shared | yes |
| 12 | Stones | carving | river | shared | no |
| 13 | Yarn dye | colors | reagent | shared | yes |
| 14 | Beads | tokens | kiln | shared | no |
| 15 | Pressed flowers | cards | orchard | private | yes |
| 16 | Reeds | harmonica | salvage | private | yes |

Sixteen materials, all of them already-existing item families, and the pool
column is a small lesson in community: shared materials belong to the craft
room, private materials belong to a person, and the difference is written down
so that neither becomes a fight.

---

## 24. APPENDIX K — FIELD DAY TABLE

| # | Day | Games | Variants | Weather rule | Note |
|---|---|---|---|---|---|
| 1 | Spring meet | races, ball | seated pass | light rain ok | opening |
| 2 | River day | swim, float | shallow watch | warm only | safety pairs |
| 3 | Kite day | kites | smallest kite | wind needed | hill |
| 4 | Summer meet | ball, races | age bands | shade breaks | water |
| 5 | Orchard day | chase, pick | walking chase | dry | fruit |
| 6 | Autumn meet | ball, marbles | target size | all but storm | closing |
| 7 | First snow | sled, snow games | seated sled | snow required | warm rooms |
| 8 | Quiet day | stretching, watch | seated | any | no scores |

Eight field days across the year, each with inclusive variants and a weather
rule that postpones rather than cancels. The quiet day is the important row: a
meet with no scores at all, held beside the others, so that the shelter says
with its calendar that not every good afternoon needs a winner.

---

## 25. APPENDIX L — RECORD CABINET TABLE

| # | Record | Maker | Kind | Year | Place |
|---|---|---|---|---|---|
| 1 | First figurine | Rill | carving | 1 | cabinet |
| 2 | Nine-names quilt | mending bee | quilt | 1 | wall |
| 3 | Chess set | Mina | woodwork | 2 | cabinet |
| 4 | Songbook | choir | book | 2 | shelf |
| 5 | Child star chart | Siss | map | 2 | wall |
| 6 | River sketch | Dovie | sketch | 2 | cabinet |
| 7 | Hundred-card deck | Ruel | cards | 3 | cabinet |
| 8 | Clay cups | craft column | pottery | 3 | in use |
| 9 | Doll house | wood bench | toy | 3 | playroom |
| 10 | Story fire tape | Hob | story | 3 | shelf |
| 11 | Kite of wind | Bailey | kite | 4 | wall |
| 12 | Winter quilt | mending bee | quilt | 4 | wall |
| 13 | Star chart II | star club | map | 4 | shelf |
| 14 | Boots mended | mending bee | repair | 4 | in use |
| 15 | Poem book | reading circle | book | 5 | shelf |
| 16 | Nameplates | Prue | woodwork | 5 | cabinet |
| 17 | Child puzzle | Siss | toy | 5 | playroom |
| 18 | Portrait wall | sketch club | art | 6 | wall |
| 19 | Song book II | choir | book | 6 | shelf |
| 20 | Ten-year quilt | mending bee | quilt | 10 | wall |
| 21 | First deck | Ruel | cards | 1 | cabinet |
| 22 | Lullaby sheet | Dovie | music | 3 | shelf |
| 23 | Orchard sketch | Mina | sketch | 6 | wall |
| 24 | The year book | all | book | 10 | shelf |

Twenty-four cabinet entries with makers and years, and the in-use and playroom
rows are the point: the record system honors things that are still being worn,
poured, and played with, not just things entombed behind glass. A cabinet of
broken playthings would be a museum; this one is a home.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_pastime_flat_evening` | 3 | First music evening |
| `quest_pastime_deck` | 3 | League formed |
| `quest_pastime_room` | 4 | Calendar agreed |
| `quest_pastime_benches` | 4 | Craft space built |
| `quest_pastime_clubs` | 4 | Clubs meet |
| `quest_pastime_tournament` | 4 | First cup played |
| `quest_pastime_song` | 3 | Song taught |
| `quest_pastime_field_day` | 3 | Field day held |
| `quest_pastime_rain` | 3 | Alternative works |
| `quest_pastime_bench_sitter` | 4 | Role found |
| `quest_pastime_gift_table` | 3 | First gifts given |
| `quest_pastime_reading` | 3 | Read-aloud held |
| `quest_pastime_second_tournament` | 3 | Handicaps proven |
| `quest_pastime_record_wall` | 4 | Records hung |
| `quest_pastime_what_evening_is_for` | 3 | Calendar made ordinary |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_pastime_stock_hobbies` | 3 | Shelves stocked |
| `quest_pastime_new_hobby` | 3 | Hobby taught |
| `quest_pastime_solo_corner` | 3 | Solo space kept |
| `quest_pastime_beginner` | 3 | First session guided |
| `quest_pastime_repair` | 3 | Tool repaired |
| `quest_pastime_strings` | 3 | Strings replaced |
| `quest_pastime_reeds` | 3 | Reeds fitted |
| `quest_pastime_cards` | 3 | Deck remade |
| `quest_pastime_chalk` | 3 | Charcoal made |
| `quest_pastime_boards` | 3 | Boards finished |
| `quest_pastime_club_night` | 3 | Night held |
| `quest_pastime_club_room` | 3 | Room booked |
| `quest_pastime_club_officer` | 3 | Officer rotated |
| `quest_pastime_club_welcome` | 3 | Member welcomed |
| `quest_pastime_club_sleep` | 3 | Quiet hours kept |
| `quest_pastime_rules` | 3 | Rules written |
| `quest_pastime_handicap` | 3 | Handicap tested |
| `quest_pastime_rest_day` | 3 | Rest day kept |
| `quest_pastime_child_league` | 4 | Child league runs |
| `quest_pastime_fairness` | 3 | Dispute settled |
| `quest_pastime_practice` | 3 | Practice logged |
| `quest_pastime_song_teach` | 4 | Song taught |
| `quest_pastime_performance` | 3 | Evening performed |
| `quest_pastime_read_aloud` | 3 | Read-aloud held |
| `quest_pastime_story_fire` | 3 | Stories told |
| `quest_pastime_record_shelf` | 3 | Shelf built |
| `quest_pastime_nameplate` | 3 | Nameplate made |
| `quest_pastime_gift` | 3 | Gift given |
| `quest_pastime_role` | 4 | Role found |
| `quest_pastime_season_meet` | 4 | Meet held |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Dovie Amberly** — music. Plays guitar in the mess hall at eight because the
alternative is silence, and teaches by letting people hear her make mistakes.
Believes an evening is a load-bearing wall.

**Ruel Tand** — games. Wrote the first rule sheet and changed it twice after
losing. Believes a league is a promise that next week will come.

**Mina Fern** — crafts. Asked for three benches and got them, and now runs the
least productive room in the shelter on purpose. Believes making something
useless is practice for being human.

**Hob Ottery** — stories. Reads aloud to twelve people and adjusts to the
slowest reader in the row. Believes a story should never be a test.

**Nella Kitt** — clubs. Learned the names of all ninety-four residents in one
winter and uses them correctly. Believes a club is a reason to leave your bunk.

**Siss Dane** — youth. Fourteen, unbeaten at dominoes, and campaigns for a
child league without a loser bracket. Believes adults should sometimes be
allowed to lose.

**Bailey Sollo** — field days. Walks the field the morning of every meet and
invents one variant for whoever cannot play. Believes a game is only a game if
everybody who came can play.

**Prue Ivett** — organizer. Keeps the calendar on the wall, protects the rest
night, and has declined three requests to schedule anything over it. Believes
the empty square is the most important one on a rota.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Old Field** — worn grass, a chalk line, and a ball that lives there.
- **The Quiet Bend** — river swimming with pairs and a shallows watch.
- **Kite Hill** — wind, string, and one tree that has eaten three kites.
- **The Stone Court** — a flat rock, a painted line, and a net of cord.
- **The Sled Slope** — snow, two boards, and a warm room at the bottom.
- **The Sketch Bluff** — a view that fits on paper and never quite twice.
- **The Story Fire** — a ring of stones and a bench for listeners.
- **The Quiet Jetty** — hooks, patience, and no talking required.
- **The Orchard Rows** — hiding places, fruit, and a season of chase.
- **The Record Wall** — names, years, and a quilt that took a decade. 

---

## 30. APPENDIX Q — FAIR PLAY CHARTER

| Clause | Promise |
|---|---|
| Handicaps | The strongest player gives odds |
| Rest | Nobody plays tired or hurt |
| Novices | First games are guided |
| Children | Own league, cooperative rules |
| Bodies | Seated and low-mobility variants |
| Disputes | Replay, never punishment |
| Wagers | No money, goods, or favors |
| Quitting | Always allowed, always without shame |
| Records | History, never pressure |
| Rooms | Quiet hours outrank any game |

The fair play charter is the expansion's first-class social object. It encodes
the difference between a shelter that uses games to build a hierarchy and one
that uses them to pass an evening together, and it does so in ten lines a child
can read.

---

## 32. APPENDIX R — WORKED PASTIME YEAR

**Week one.** Dovie plays in the mess hall at eight, three people listen, and
nobody says the word morale. Ruel produces a deck and a rule sheet and by
Friday there are eleven players and a waiting chair.

**Week three.** Prue pins a calendar on the wall: two open evenings, one club
night, one practice night, one protected quiet night. The quiet-night square is
empty and stays empty, and somebody tries to book it for a repair job and is
told no by three separate people.

**Week five.** Mina gets three benches and a shared shelf. The first output is
a carved bird that nobody needs, and it goes on a shelf anyway because the shelf
is the point.

**Week seven.** Six clubs sign members on one night: cards, crafts, music,
reading, runners, ball. Nella writes the names on a board and Rill discovers
that she has been in the shelter for a year and did not know the name of the
woman who fixes the pumps.

**Week ten.** The first tournament produces a final between Bailey and Siss,
age fourteen. Siss wins the first hand, Bailey adjusts her time odds, and the
room learns to cheer for both, which turns out to be possible.

**Week fourteen.** Dovie teaches a three-part round to eleven people. The third
part comes in late the first evening, on time by the third evening, and the
choir has a second song.

**Week eighteen.** The first field day gets light rain and moves to the mess
hall for seated variants. It is a good afternoon anyway, and Bailey writes the
rain rule into the next calendar.

**Week twenty-two.** Hob reads aloud on a Tuesday; a resident who has never
read in public asks to try one paragraph and does, and the circle waits
patiently without correcting a single word.

**Week twenty-six.** The bench sitter problem arrives: a resident with a
healing leg can play nothing on the calendar. Bailey invents a scorer's chair
and a calling role, and the resident keeps the standings for the rest of the
year, and the standings have never been better kept.

**Week thirty-two.** The first gift table: carved birds, knitted hats, clay
cups, and a pressed-flower card. Nothing is sold; everything is chosen by the
person who receives it, which produces more arguments than the tournament did.

**Week thirty-eight.** The second tournament runs with handicaps and rest days
and finishes with twenty participants instead of twelve, because the people who
were beaten badly the first time came back.

**Week forty-four.** The reading circle finishes its first borrowed book and
returns it on time, and Hob writes the date on a card for the record shelf.

**Week fifty.** The cabinet gets its first year: eleven entries with makers and
dates. The shelter has something no survival log contains, which is proof that
it was a place where people did things for no reason at all.

---

## 33. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Dovie plays three chords and gets one wrong and does not stop, and Rill
> watches her not stop, and that is the whole lesson and it takes four seconds.

> Ruel writes the rule: the strongest player gives odds. Bailey argues. Ruel
> says write your own rule then, and Bailey does, and the two rules end up on
> the same sheet, and the league has a constitution by Thursday.

> Siss wins the first hand and the room goes quiet in the bad way, and then
> Bailey laughs and shuffles and says again, and the quiet goes away and does
> not return for the rest of the year.

> Hob reads one paragraph and the resident's voice shakes and nobody looks up,
> and when she stops the circle just waits, and after fifteen seconds she reads
> the next paragraph too, and by June she reads the page.

---

## 34. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| No calendar | clubs collide, quiet broken | pin a week |
| Rooms hoarded | resentment | rotate bookings |
| No materials | hobbies fade | shared shelf |
| No handicaps | novices quit | rewrite rules |
| Quiet overrun | complaints, bad sleep | quiet owner enforcement |
| Gifts as goods | trade pressure | gift table only |
| Records as ranking | pressure and pride | history framing |
| Club cliques | exclusion | welcome roles |
| No solo space | introverts excluded | quiet corner |
| No rest night | exhaustion | protect the square |

The recovery column is deliberately light because the failure column is light:
no leisure system can ruin a shelter, but a shelter with no leisure quietly
ruins itself. The last two rows are the ones the plan protects hardest.

---

## 35. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] No second morale, stress, or needs system.
- [ ] All morale and stress effects use `NeedsSystem.Modify`.
- [ ] No wagers, currency, or economy goods at stake.
- [ ] Children play only; no child labor or adult competition.
- [ ] Quiet hours route through the live noise owner.
- [ ] Ceremonies stay with `CeremonySystem`.
- [ ] Relationships stay with the relationship owner.
- [ ] Records are history, never a leaderboard.
- [ ] Save additions are additive inside `recreation`.
- [ ] Determinism uses seeded pairings only; no wall-clock.

---

## 36. APPENDIX V — GLOSSARY

- **Pastime** — a hobby, club, game, craft, or circle done for its own sake.
- **Club** — a recurring group with a room and a cadence.
- **League** — a structured series with rules, handicaps, and rest days.
- **Handicap** — odds that make a match worth playing for both sides.
- **Rest day** — scheduled non-competition, part of the equipment.
- **Gift table** — where craft outputs are chosen, never traded.
- **Record cabinet** — made things with makers and years.
- **Quiet corner** — a solo space inside a social shelter.
- **Read-aloud** — shared reading without correction or testing.
- **Protected night** — the empty square on the calendar.

---

## 37. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `SurvivorDowntimeSystem` | hobbies, items | sessions | needs |
| `ClubSystem` | profiles | rosters | work rota |
| `LeagueSystem` | profiles | brackets | needs |
| `EnsembleSystem` | hobbies | practice | rooms policy |
| `CraftCircleSystem` | inventory | works | inventory totals |
| `ReadingCircleSystem` | library | borrow state | book owners |
| `FieldDaySystem` | weather | meets | weather |
| `PastimeRecordSystem` | history | records | standings pressure |
| `NeedsSystem` | nothing | nothing | nothing |
| `CeremonySystem` | nothing | nothing | nothing |
| `ShelterNoiseSystem` | bookings | quiet state | nothing |
| `ShelterWorkshopSystem` | repair queue | nothing | nothing |
| `WeatherSystem` | forecast | nothing | nothing |
| `PressHostSession` | sheets | prints | nothing |
| `StandingRecord` | records | records | nothing |
| `ChildDevelopmentSystem` | play | nothing | nothing |
| `MemorialSystem` | losses | memory | nothing |

---

## 38. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`recreation.json`** (extend) — existing `hobbies` rows with the same schema;
new rows must reference real items and real room tags.

**`pastime_clubs.json`** — `club_id`, `display_name`, `kind`, `cadence`,
`room_tags[]`, `social_min`, `social_max`, `officer_roles[]`, `tags[]`.

**`pastime_tournaments.json`** — `tournament_id`, `format`, `rounds`,
`handicap`, `rest_days`, `eligibility`, `award`, `tags[]`.

**`pastime_rules.json`** — `rule_id`, `game`, `win_condition`, `fair_play[]`,
`dispute`, `variants[]`, `tags[]`.

**`pastime_songs.json`** — `song_id`, `title`, `kind`, `parts`, `learned_by`,
`print_ref`, `tags[]`.

**`pastime_crafts.json`** — `craft_id`, `display_name`, `materials[]`, `bench`,
`output_item_id`, `use`, `tags[]`.

**`pastime_materials.json`** — `item_id`, `craft_uses[]`, `source`, `pool`,
`consumable`, `tags[]`.

**`pastime_reading.json`** — `circle_id`, `book_ref`, `cadence`,
`read_aloud`, `discussion_note`, `tags[]`.

**`pastime_field_days.json`** — `day_id`, `season`, `games[]`, `variants[]`,
`weather_rule`, `spectator_note`, `tags[]`.

**`pastime_records.json`** — `record_id`, `maker_id`, `kind`, `year`, `place`,
`story_line`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid item or room references, or out-of-range
numbers.

---

## 39. APPENDIX Y — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Sessions completed | participation | Downtime |
| Distinct participants | reach | Downtime |
| Clubs meeting | social health | Clubs |
| Quiet corners used | inclusion | Rooms |
| Tournaments run | culture | Leagues |
| Novices retained | fairness | Leagues |
| Works completed | making | Crafts |
| Gifts given | generosity | Craft circles |
| Read-alouds held | literacy warmth | Reading |
| Rest nights protected | rest policy | Calendar |

Telemetry is diagnostic only; it never ranks residents and never gates a
pastime behind a number.

---

## 40. APPENDIX Z — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive inside `recreation`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows clubs persisting and rest nights surviving.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No wager, quota, or humiliation content exists.

---

## 41. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. How many clubs can meet in a week before the calendar becomes a second job?
2. Do hobby skill levels ever gate a hobby, or only suggest it?
3. Can a tournament be paused for a crisis and resumed, and by whom?
4. Should gifts ever be refused, and how is refusal handled without hurt?
5. Does the shelter keep child leagues when adults want to play too?
6. Are records anonymous on request, and does that break the wall's story?
7. How loud can a good evening be before it becomes a quiet-hours problem?
8. What happens to a club when its founder dies?

None of these may be decided unilaterally; each changes tone and balance.

---

## 42. APPENDIX AB — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Children's play and leagues |
| 1 | 13 Faith | Songs that reach both rooms |
| 1 | 16 The Rebuilt Body | Adaptive play and prosthetics |
| 2 | 17 The Long Evening | Older residents as story keepers |
| 2 | 19 The Bitter Air | Indoor evenings on bad-air days |
| 3 | 24 The Long Goodbye | Memorial songs and quilts |
| 3 | 26 The Common Table | Mess hall evenings and food culture |
| 4 | 27 The Thread | Yarn, cloth, and mending bees |
| 4 | 28 The Lesson | Hobby teaching and children's clubs |
| 4 | 30 The Press | Song sheets and score cards |
| 4 | 31 The Kiln | Clay pieces and tokens |
| 5 | 33 The Weather | Field days and rain rules |
| 5 | 35 The Habit | Neutral pastime spaces |
| 6 | 37 The Quickening | Parent-and-child play |
| 6 | 40 The Wheel | Instrument repair and benches |
| 6 | 41 The Quiet | Quiet corners and protected nights |
| 7 | 43 The Question | Reading circles borrowing texts |
| 7 | 46 The Long Change | Outdoor play and seasonal ground |

Each hook is additive. The Pastime can ship alone, and every other expansion
can ship without it.

---

## 43. APPENDIX AC — ENDING PROSE SKETCHES

**The Common Evening.** The shelter has a weekly rhythm of play, and the work
week is measured by the evenings that end it.

**The Handmade Year.** The cabinet fills with gifts, keepsakes, and names, and
the shelter can date its own culture to the year it started making things.

**The Fair Field.** Leagues and handicaps make competition a pleasure, and the
people who lose the most keep coming back, which is the only metric that
matters.

**The Protected Night.** A rest night is written into the rota, and when a hard
week tries to take it, three people say no and the square stays empty.

**The Quiet Table.** Some residents choose solo pastimes, and the shelter
counts a quiet corner and a bird chart as play, because it is.

**Fade.** A mess hall at eight in the evening: cards on a table, a guitar being
tuned, and a child winning at dominoes while an adult takes the loss well.

---

## 44. APPENDIX AD — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Morale reimplementation | authority break | `NeedsSystem.Modify` |
| Gambling | economy breach | no wagers |
| Humiliation | cruelty | handicaps and replay |
| Productivity quotas | grind | no quotas |
| Literacy tests | shame | read-aloud |
| Festivals takeover | owner breach | ceremony owner |
| Leaderboards | pressure | history framing |
| Quiet-breaking | sleep harm | book through owner |
| Adult-only design | exclusion | seated variants |
| Rest denied | exhaustion | protected night |

The list exists because leisure is easy to turn into another job. The
expansion's rule is that an evening has no output target, and the relief is the
point.

---

## 45. APPENDIX AE — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Hobbies | 24 new | 4,500 |
| Clubs | 12 | 2,500 |
| Tournaments | 8 | 2,000 |
| Rules | 12 | 2,500 |
| Songs | 16 | 3,000 |
| Crafts | 20 | 3,500 |
| Materials | 16 | 2,500 |
| Reading circles | 6 | 1,500 |
| Field days | 8 | 2,000 |
| Records | 24 | 3,500 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~58,500** |

---

## 46. APPENDIX AF — WEEKLY CALENDAR TABLE

| Day | Evening | Room | Quiet rule |
|---|---|---|---|
| Monday | open hobbies | common | normal |
| Tuesday | club night | club room | normal |
| Wednesday | practice | music room | normal |
| Thursday | open hobbies | common | normal |
| Friday | league night | card room | lights late |
| Saturday | story fire | outside | weather |
| Sunday | protected rest | everywhere | early quiet |

The weekly calendar is the expansion's structural spine. The protected Sunday
square is the one that must survive every later system, and the Friday late
lights are the one deliberate exception, negotiated with the quiet owner rather
than imposed on it.

---

## 47. APPENDIX AG — PASTIME COVENANT

| Clause | Promise |
|---|---|
| Rest | An evening belongs to the person who worked the day |
| Fair | The strongest give odds |
| Open | Nobody is picked last, nobody is excluded |
| Quiet | Sleep outranks any game |
| Make | Things are made for use and giving |
| Keep | Makers' names go on their work |
| Teach | Skill is passed hand to hand |
| Include | Every body has a way to play |
| No wagers | Nothing of value is staked |
| Return | Borrowed books come home |

The covenant is the expansion's first-class design object. Work builds the
shelter; the covenant governs what the shelter does with the hours it has
earned, and it is written to be read aloud at the record wall once a year.

---

## 48. APPENDIX AH — CLUB SUCCESSION TABLE

| Club | Founder | Successor | Handover |
|---|---|---|---|
| Card club | Ruel | Siss | one league |
| Craft column | Mina | bench holder | one season |
| Music circle | Dovie | choir lead | one song |
| Reading circle | Hob | reader | one book |
| Runners | Bailey | pace lead | one week |
| Ball club | Bailey | captain | one cup |
| Kite club | Siss | wind watcher | one hill day |
| Mending bee | Mina | rotating | one quilt |
| Choir | Dovie | section leads | one concert |
| Bird watchers | Hob | list keeper | one spring |
| Star club | Dovie | list keeper | one clear night |
| Story fire | Hob | host | one fire |

Twelve clubs with named successors, because the quiet way a culture dies is
when its founder gets tired and nobody knows who holds the key to the room. The
handover column is one shared activity, done together, before the key changes
hands.

---

## 49. APPENDIX AI — SESSION LEADER TABLE

| # | Leader role | Duty | Helps whom | Limit |
|---|---|---|---|---|
| 1 | Hobby host | set up, teach | beginners | 2 sessions |
| 2 | Club officer | room, roster | members | 1 year |
| 3 | Rules keeper | write rules | players | 1 season |
| 4 | Handicapper | set odds | all players | 1 league |
| 5 | Bench keeper | tools | makers | 1 season |
| 6 | Material keeper | pool stock | makers | 1 season |
| 7 | Circle host | borrow, return | readers | 1 book |
| 8 | Field marshal | variants | every body | 1 meet |
| 9 | Record keeper | names, years | makers | 1 year |
| 10 | Quiet guard | protect rest | everyone | always |

Ten leader roles, each with a limit so that the person who makes the evening
possible also gets to have one. The quiet guard's row is deliberately spelled
out as always and for everyone, because the right to a rest night is the one
role nobody is allowed to take on alone.

---

## 50. APPENDIX AJ — QUIET CORNER TABLE

| # | Corner | For | Where | Rule |
|---|---|---|---|---|
| 1 | Reading nook | one reader | library | lamp only |
| 2 | Bird window | watcher | west wall | no talking |
| 3 | Sketch seat | drawer | bluff | weather |
| 4 | Star roof | night watcher | roof walk | clear nights |
| 5 | Yarn chair | knitter | craft room | one chair |
| 6 | Mending table | one mender | craft room | shared |
| 7 | Model shelf | builder | workshop | own shelf |
| 8 | Letter seat | writer | quarters | quiet hour |
| 9 | Fish jetty | angler | river | alone fine |
| 10 | Tune room | player | music room | one at a time |

Ten quiet corners for the residents who love the shelter and still need an hour
where nobody asks them anything. The list is the expansion's answer to the fear
that a pastime system becomes a mandate to be social, and it is deliberately
built into the room plan rather than added later as a patch.

---

## 51. APPENDIX AK — FIRST-YEAR CULTURE SCHEDULE

| Month | Milestone | Kept by |
|---|---|---|
| 1 | First evening | Dovie |
| 2 | League formed | Ruel |
| 3 | Calendar pinned | Prue |
| 4 | Benches built | Mina |
| 5 | Clubs chartered | Nella |
| 6 | First cup | Ruel |
| 7 | Song taught | Dovie |
| 8 | Field day | Bailey |
| 9 | Rain rule | Bailey |
| 10 | Scorer's chair | Siss |
| 11 | Gift table | Mina |
| 12 | Record year | Hob |

Twelve months from an empty evening to a recorded culture, with one keeper
named per milestone. The schedule is also a warning: every culture needs
somebody whose job is the calendar, and the plan names them early so that the
year does not depend on one exhausted founder.

---

## 52. CLOSING STATEMENT

ASHFALL already models downtime honestly: stress relief, morale, favorite
hobbies, skill levels, brawls when the table gets tense, and a save record of
sessions completed. It has six hobbies and a board of blank profiles. The
Pastime fills the system with thirty ways to spend an evening, clubs that meet
because people want to see each other, tournaments with handicaps and rest
days, songs the shelter teaches itself, benches where things are made and
given, and a cabinet with names and years on it. It adds no second morale, no
wager, no quota — just the part of life that makes the rest of it worth
keeping.

> Wave 8 note: this plan is one of five Wave 8 expansion bibles (47–51). Each is
> self-contained; none requires another to ship. The shared Wave 8 index lives
> at `docs/expansions/wave8/WAVE8_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `SurvivorDowntimeSystem` (`HobbyDef`, `ActiveHobbySession`,
> `SurvivorHobbyProfile`, `RecreationState`, `StartSession`, `CompleteSession`,
> `TickDay`, `GetOrCreateProfile`, `NeedsSystem.Modify` for morale, brawl
> handling, events `OnHobbyStarted` / `OnHobbyCompleted` / `OnHobbyBrawl`),
> `recreation.json` (3,726 B, six hobbies, two output items
> `item_carved_figurine` and `item_wasteland_sketch`), the `recreation` save
> section with `RecreationSaveStore`, and `SurvivorDowntimePanel`.