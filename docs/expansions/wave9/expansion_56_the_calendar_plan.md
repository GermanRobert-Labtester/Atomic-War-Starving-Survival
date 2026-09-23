# ASHFALL — Expansion 56 Design Bible
# THE CALENDAR
### Wave 9 · Ceremonies, Feasts, Vigils, Truces, Preparations, Disasters on the Day, and the Civic Year

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Narrative` (`CeremonySystem`, ceremony definitions and state), `Ashfall.Core.Survivors` (`NeedsSystem` morale path), `Ashfall.Core.Inventory`
**Proposed host owner:** `CalendarHostSession` (extends `CeremonySaveStore` + `CeremonyFestivalPanel`)
**Existing save sections:** `ceremony` (`ceremony_save.json`, `CeremonySaveState`)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no ceremony-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already models civic ceremony. `CeremonySystem` (`SystemId` =
"ceremony_system") defines `CeremonyItemRequirement` (`ItemId`, `Quantity`),
`CeremonyDefinition` (`Id`, `DisplayName`, `PreparationDays` default 3,
`RequiredRoomId` default "room_common_mess_hall", `MinPopulation`,
`RequiredItems`, `MoraleBoost` default 25f, `StressRelief` default 20f,
`TruceDurationDays`, `TruceEligible`, `DisasterPool`, `Description`), the
scheduled-ceremony state (`CeremonyId`, `ScheduledDay`,
`PreparationDaysRemaining`, `InvitedFactions`, `AcceptedFactions`,
`ActiveTruceDaysRemaining`, `OccurredDisasterId`), and `CeremonySaveState`
(`systemId`, `CompletedCeremonyIds`, `TotalCeremoniesHeld`,
`TotalDisastersEncountered`). The API is live: `LoadCatalog`,
`ScheduleCeremony(ceremonyId, currentDay, currentPopulation, out error)`,
`ContributeResource(itemId, quantity)`, `InviteFaction(factionId,
currentStanding)`, `TickDay(currentDay, out outcomeSummary)`, `CaptureState`,
and `RestoreState`. The save store and `CeremonyFestivalPanel` already exist.

The data `ceremonies.json` contains exactly **five ceremonies**: Shelter
Founding Day Anniversary (prep 3, morale 25), Remembrance Vigil of the Lost
(prep 2, morale 15), Long Night Solstice Bonfire (prep 4, morale 30, truce
eligible), Grand Treaty Barter Fair (prep 5, morale 20, truce eligible), and
Ashfall Hydroponic Harvest Feast (prep 3, morale 28). Each has a disaster pool
of two entries. What does not exist: the year. There is no calendar, no
seasonal cycle of observances, no preparation practice or kitchen planning, no
guest hosting, no vigil etiquette, no disaster recovery when a day goes wrong,
no ceremony records or chronicle ties, and no story about why a shelter that
almost died would set aside a day at all.

**The Calendar** is the expansion about the civic year: the small holy days
of a secular community — founding days, vigils, solstices, treaty fairs, and
harvest feasts — with preparations, invited guests, faction truces, disasters
on the day, and the year after year accumulation of observances. It extends
the live ceremony system and never duplicates a faith, memory, food, records,
or faction authority.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| `CeremonySystem` | Ceremonies, preparation, truces | Extends with the civic year |
| 13 The Faithful and the Fractured | Faith, belief, fracture | Civic observances only; no doctrine |
| 24 The Long Goodbye | Mourning and memory | The vigil routes through it |
| 26 The Common Table (Wave 3) | Food, rations, preservation | Feasts order from it; never own food |
| `NeedsSystem` | Morale and stress | Uses `Modify` only |
| 45 The Envoy (Wave 7) | Diplomacy and treaties | Truces are ceremony outcomes; envoys negotiate |
| `ShelterReputationSystem` | Standing | Invitations read it; never write it |
| `FactionStanding` ports | Relations | Read-only |
| 48 The Pastime (Wave 8) | Clubs, games, songs | Performances and games at festivals route through it |
| 46 The Long Change (Wave 7) | Year notes | The calendar feeds the chronicle |
| 03 The Standing Record | Records | Files the ceremony roll |
| 12 The Second Generation | Children | Naming days and first-years observe here |
| 37 The Quickening (Wave 6) | Births | Welcoming days route through its owners |
| 33 The Weather (Wave 5) | Seasons | Outdoor days respect forecasts |
| 50 The Vault (Wave 8) | Culture | Chronicle volumes and songs are preserved there |
| 55 The Quarter (Wave 9) | Blocks and meetings | Observances are shelter-wide, not neighborhood-managed |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter has five ceremonies and no year. It celebrates its founding when
someone remembers, holds a vigil when someone dies, and lets the solstice pass
because the boiler needed attention. Its children are growing up in a place
with no red-letter days at all.

**The Calendar** is the expansion about the civic year: a fixed cycle of
observances with preparations, guest lists, food, music, truces, weather
contingencies, and the honest fact that the day you planned will sometimes go
wrong. It is the expansion about the smallest and most stubborn act of
civilization there is: deciding that a date matters.

### 1.2 The five loops it adds

```
  Set ──► Prepare ──► Invite ──► Hold ──► Remember
    │        │           │         │          │
    ▼        ▼           ▼         ▼          ▼
  Calendar, supplies,  guests,   the day,   roll and
  seasons   kitchen    truces    disasters  chronicle
                                        │
                                        ▼
                          Recover ──► Revise ──► Keep
```

### 1.3 What the player manages

1. **The calendar.** Which days are kept and when.
2. **Preparation.** Supplies, rooms, banners, wood, and time.
3. **Food.** Feast orders, shared dishes, and the kitchen's role.
4. **Guests.** Neighbors, outposts, factions, and seating.
5. **Truces.** The days when the valley agrees to be quiet together.
6. **The day.** Ceremony order, vigil etiquette, music, and games.
7. **Disasters.** Storm, outbreak, alarm, and the recovery afterward.
8. **Observance.** Vigils, naming days, first-years, and anniversaries.
9. **Recovery.** What happens when a day is ruined or someone is lost.
10. **Records.** The ceremony roll, chronicle ties, and year over year.

### 1.4 What it is not

- Not a religion; observances are civic and have no doctrine.
- Not a second faith, memory, food, or diplomacy system.
- Not a morale fountain; boosts are authored and earned.
- Not a forced-fun system; nobody is required to attend.
- Not a truce hack; truces are faction decisions read from standing.
- Not a party-planning minigame with timers and failures.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Narrative/CeremonySystem.cs` | Definitions, schedule, truce | `LIVE` |
| `src/Host/CeremonySaveStore.cs` | `ceremony` save | `LIVE` |
| `src/UI/CeremonyFestivalPanel.cs` | Current surface | `LIVE` |
| `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` | Morale sink | `LIVE` |
| Faction standing ports | Invitation checks | `LIVE` |
| `EpilogueChronicleBuilder` | Year records | `LIVE` |
| `WildlifeSeasonalCalendar` | Seasons | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Rows | Notes |
|---|---|---|
| `ceremonies.json` | **5** | prep 2–5, morale 15–30, two truce-eligible |
| Companion event catalogs | present | the live event system, not ceremonies |
| Calendar, feast, guest, vigil catalogs | absent | confirmed none |
| Ceremony roll and chronicle ties | absent | confirmed none |

### 2.3 Confirmed gaps

- **GAP-56-1 — Five ceremonies and no year.**
- **GAP-56-2 — No calendar or seasonality content.**
- **GAP-56-3 — No preparation or supply practice.**
- **GAP-56-4 — No feast or kitchen coordination content.**
- **GAP-56-5 — No guest, seating, or hosting content.**
- **GAP-56-6 — No truce practice or violation content.**
- **GAP-56-7 — No vigil or observance etiquette.**
- **GAP-56-8 — No disaster-on-the-day recovery content.**
- **GAP-56-9 — No ceremony roll or chronicle ties.**
- **GAP-56-10 — The live system runs with no civic culture around it.**

### 2.4 Non-duplication statement

This expansion will **not** add a second faith, memory, food, diplomacy,
morale, or records system. It extends `CeremonySystem` with content and
practice, sends morale and stress through `NeedsSystem.Modify`, routes vigils
through the memory owner, orders feasts from the food owners, reads faction
standing without writing it, performs through the pastime owner, and files the
roll with `StandingRecord`. All new state is additive inside
`CeremonySaveState`. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — A date is a promise to the future.** Keeping a day is how a
shelter says it intends to still exist next year.

**Pillar 2 — Preparation is the ceremony.** The cooking, the bringing of
chairs, and the sweeping of the hall are the actual observance.

**Pillar 3 — Nobody is compelled.** Attendance is a choice, labor is
voluntary, and absence is not disloyalty.

**Pillar 4 — The day may go wrong.** Disasters are authored, recoverable, and
part of the story, not a failure screen.

**Pillar 5 — The year remembers itself.** A roll, a volume, and a song make
next year's day real.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Founding day | Speeches, small gifts, repairs | Nationalist pomp |
| Vigil | Names, silence, lamps | Spectacle grief |
| Solstice | Fire, food, long night games | Mysticism |
| Treaty fair | Booths, exchanges, guests | Diplomatic theatre |
| Harvest feast | Dishes, first tastes, thanks | Excess |
| Truces | Talks, respect, a day off | Magical peace |
| Disasters | Storm, illness, alarm, recovery | Ruined-day punishment |
| Records | Rolls, songs, volumes | Grandiosity |

### 3.3 Content limits

- No real faiths, holidays, or observances copied; all days are invented.
- No religious doctrine or proselytizing; observances are civic.
- No forced participation or morale punishment for absence.
- No feast gluttony framing; food is shared, honored, and limited by stores.
- No truce magic; truces are agreements with terms, durations, and violations.
- No mourning spectacle; vigils route through the memory owner's practices.
- No new save section.

---

## 4. THE CALENDAR WORLD

### 4.1 Interior rooms

- **`room_calendar_office`** — the year board, the roll, and the date nails.
- **`room_feast_kitchen`** — the kitchen in feast mode and its order board.
- **`room_vigil_room`** — lamps, names, and one bench.
- **`room_festival_hall`** — the mess hall under banners.
- **`room_guest_quarters`** — beds and a shelf for visitors.
- **`room_truce_table`** — the table where terms are read.
- **`room_preparation_store`** — supplies held for days that matter.
- **`room_roll_room`** — the ceremony roll and the year volumes.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_founding_stone` | The Founding Stone | 3 | Founding day |
| `loc_bonfire_ring` | The Long Night Ring | 3 | Solstice fire |
| `loc_fair_field` | The Fair Field | 2 | Treaty fair |
| `loc_harvest_rows` | The Harvest Rows | 2 | Harvest feast |
| `loc_vigil_walk` | The Vigil Walk | 3 | Lantern path |
| `loc_guest_gate` | The Guest Gate | 3 | Arrivals and welcome |
| `loc_music_stand` | The Music Stand | 1 | Performances |
| `loc_first_year_tree` | The First Year Tree | 2 | Children's marks |
| `loc_quiet_anniversary` | The Quiet Stone | 2 | Private remembrance |
| `loc_roll_stone` | The Roll Stone | 2 | Year marks of observances |

All locations require valid item or map-node references and scanner registration.

### 4.3 The rhythm

The year is planned in one meeting; each day is prepared for 2–5 days; guests
arrive by the gate; the day is held; the roll is written the next morning; and
the year is read at the winter end. The expansion's clock is the year.

---

## 5. MAIN STORYLINE — "THE DAYS WE SET ASIDE"

### 5.1 Central conflict

**Larkin Stow** keeps a year board with five nails in it and begins to argue
that a shelter with a founding day and no calendar is a shelter that does not
believe in its own future. **Marle Griss** wants a feast that uses the
harvest's first tastes instead of its last stores. **Zell Wist** wants
ceremonies that people actually attend, which means short speeches and good
music. **Teal Thane** wants banners, and defends the expense of cloth in
winter by pointing out that a hall that looks like a celebration is a hall
where people stay. **Brun Eames** wants guests at the treaty fair, and
volunteers to host them properly. **Kerr Laine** wants the kitchen to be part
of the ceremony rather than its caterer.

Then the first real calendar year tests the system. The solstice bonfire is
ruined by a storm and moved inside, where it becomes better than planned. The
harvest feast is interrupted by a disease scare and a partial quarantine,
and the shelter eats separately in shifts and calls it a feast anyway. The
treaty fair's truce holds for three days and one violation, and the shelter
learns that a truce is not peace but a date on which people agree not to
fight. And then a first-year child is given their name in front of the whole
shelter at the winter end, and everybody understands what the calendar was
for.

The expansion's question: **what does a shelter owe the dates it remembers?**

### 5.2 Theme (unspoken)

**A calendar is a shelter saying: we expect to be here next year.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_calendar_larkin_stow` | Larkin Stow | Calendar keeper | The year and the roll |
| `npc_feast_marle_griss` | Marle Griss | Feast organizer | Dishes and harvest |
| `npc_ceremony_zell_wist` | Zell Wist | Master of ceremonies | Order and brevity |
| `npc_banners_teal_thane` | Teal Thane | Banners | Cloth, color, halls |
| `npc_guest_brun_eames` | Brun Eames | Guest host | Visitors and seating |
| `npc_kitchen_kerr_laine` | Kerr Laine | Kitchen | Feast orders |
| `npc_lights_mace_niall` | Mace Niall | Lighting | Fire, lamps, stages |
| `npc_apprentice_wrenna_quin` | Wrenna Quin | Apprentice | Roll and small days |

### 5.4 Story beats (15)

1. **The Year Board.** Five nails and an argument.
2. **The Twelve.** A civic year is drafted.
3. **The Prep.** The first preparation week runs.
4. **The Hall.** Banners, chairs, and swept floors.
5. **The Founding.** The first kept founding day.
6. **The Vigil.** A candle path for the lost.
7. **The Storm.** The solstice is ruined and saved.
8. **The Guests.** The fair invites the valley.
9. **The Truce.** Three days, one violation.
10. **The Scare.** The harvest feast in shifts.
11. **The First Year.** A child is named before the shelter.
12. **The Quiet.** A private anniversary is honored.
13. **The Roll.** The year's ceremonies are recorded.
14. **The Reading.** The year end is read aloud.
15. **The Days We Set Aside.** The calendar becomes ordinary.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Year | fixed calendar / flexible / minimal | tradition |
| Prep | elaborate / modest / last-minute | effort |
| Guests | open / invited / shelter-only | openness |
| Feasts | first tastes / full stores / token | food ethics |
| Truces | always propose / case-by-case / never | diplomacy |
| Vigils | public / quiet / private option | mourning |
| Disasters | push through / postpone / cancel | resilience |
| Final | institution / seasons / quiet days | identity |

### 5.6 Endings (5 + fade)

1. **The Kept Year** — twelve observances, a full roll, and children who know
   the dates by heart.
2. **The Open Gate** — the treaty fair becomes the valley's fixed meeting, and
   neighbors plan their year around the shelter's calendar.
3. **The Truce Year** — truces become a regular, honest practice with terms
   and witnesses, and the valley has quiet days.
4. **The Quiet Observance** — the shelter keeps fewer, deeper days, and the
   private anniversaries matter as much as the public ones.
5. **The Long Table** — the feast becomes the year's center, and the harvest's
   first taste is shared before its last is stored.
6. **Fade** — a hall full of mismatched chairs, a banner reused for its fourth
   year, a roll with fresh ink, and a child's name read aloud.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_calendar_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_calendar_year_board`, `quest_calendar_twelve`, `quest_calendar_prep`,
`quest_calendar_hall`, `quest_calendar_founding`, `quest_calendar_vigil`,
`quest_calendar_storm`, `quest_calendar_guests`, `quest_calendar_truce`,
`quest_calendar_scare`, `quest_calendar_first_year`, `quest_calendar_quiet`,
`quest_calendar_roll`, `quest_calendar_reading`,
`quest_calendar_days_we_set_aside`.

### 6.2 Side quests (30)

**The year (5)**
- `quest_calendar_draft` — year drafted
- `quest_calendar_dates` — dates fixed
- `quest_calendar_rooms` — rooms booked
- `quest_calendar_seasons` — seasons respected
- `quest_calendar_notes` — year notes kept

**Preparation (5)**
- `quest_calendar_supplies` — supplies gathered
- `quest_calendar_repair` — hall repaired
- `quest_calendar_banners` — banners made
- `quest_calendar_chairs` — seating built
- `quest_calendar_wood` — bonfire wood laid

**Feasts (5)**
- `quest_calendar_first_taste` — first tastes saved
- `quest_calendar_dishes` — dishes planned
- `quest_calendar_shared_table` — shared dishes
- `quest_calendar_kitchen_shift` — kitchen honored
- `quest_calendar_stores_note` — stores respected

**Guests and truces (5)**
- `quest_calendar_invite` — invitations sent
- `quest_calendar_gate` — gate welcome
- `quest_calendar_seating` — seating fair
- `quest_calendar_terms` — truce terms read
- `quest_calendar_witness` — witness recorded

**Observances (5)**
- `quest_calendar_lamps` — vigil lamps lit
- `quest_calendar_names` — names read
- `quest_calendar_naming` — naming day held
- `quest_calendar_anniversary` — private day honored
- `quest_calendar_song` — festival song sung

**Records (5)**
- `quest_calendar_roll_entry` — roll written
- `quest_calendar_volume` — year volume bound
- `quest_calendar_stone` — stone marked
- `quest_calendar_review` — year reviewed
- `quest_calendar_lesson` — lesson kept

### 6.3 Repeatable quests (8)

`quest_calendar_repeat_prep`, `quest_calendar_repeat_hold`,
`quest_calendar_repeat_roll`, `quest_calendar_repeat_lamps`,
`quest_calendar_repeat_feast`, `quest_calendar_repeat_guest`,
`quest_calendar_repeat_review`, `quest_calendar_repeat_reading`.

### 6.4 Dynamic hooks

Live events (`ScheduleCeremony`, `ContributeResource`, `InviteFaction`,
`TickDay` with `outcomeSummary`, disasters, weather, births, deaths, standing
changes) attach authored follow-ups through existing seams. No new event bus.

### 6.5 Constraints

- Definitions, preparation, and truces stay with `CeremonySystem`.
- Morale and stress use `NeedsSystem.Modify` only.
- Vigils route through the memory owners.
- Feasts order from the food owners.
- Faction standing is read-only.
- Performances route through the pastime owner.
- The roll files with `StandingRecord` and the chronicle owner.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `CalendarSystem` (new, `Ashfall.Core.Narrative`)

**Owns:** the civic year: observances, dates, seasons, and the year board.
**Consumes:** the live ceremony catalog, seasonal calendar, weather.
**Data:** `calendar_year.json`. **Rules:** the year is planned in one meeting;
dates are fixed before preparation begins; seasons and weather are respected;
a day may be postponed without shame.

### 7.2 `PreparationSystem` (extend `CeremonySystem`)

**Owns:** preparation weeks, supplies, rooms, banners, seating, wood, and
repairs. **Consumes:** live `ContributeResource`, inventory, workshop.
**Data:** `calendar_prep.json`. **Rules:** preparation is the ceremony;
contributions are voluntary; the required items stay as authored; an unfinished
preparation postpones rather than fails.

### 7.3 `FeastSystem` (new, thin, `Ashfall.Core.Narrative`)

**Owns:** feast menus, first tastes, shared dishes, kitchen shifts, and store
limits. **Consumes:** kitchen and food owners. **Data:** `calendar_feasts.json`.
**Rules:** feasts use first tastes and honored dishes; stores are never gutted
for a day; the kitchen is honored in the program, not thanked in a line.

### 7.4 `GuestSystem` (new, `Ashfall.Core.Narrative`)

**Owns:** guest lists, invitations, gate welcome, seating, quarters, and
departures. **Consumes:** faction standing (read-only), outpost rosters,
quarters. **Data:** `calendar_guests.json`. **Rules:** guests are housed
decently and never used as leverage; seating is arranged by the host with
reasons; a refusal is recorded without insult.

### 7.5 `TruceSystem` (extend `CeremonySystem`)

**Owns:** truce proposals, terms, durations, witnesses, and violation reports.
**Consumes:** the live `InviteFaction` path and `TruceDurationDays`.
**Data:** `calendar_truces.json`. **Rules:** terms are read aloud; witnesses
sign; violations are reported honestly and do not cancel the calendar; a truce
is a date, not a peace.

### 7.6 `ObservanceSystem` (new, thin, `Ashfall.Core.Narrative`)

**Owns:** vigils, naming days, first-years, anniversaries, and private
observance. **Consumes:** memory owners, birth owners, care owners. **Data:**
`calendar_observances.json`. **Rules:** vigils are dignified and short; names
are read only with consent; private anniversaries are honored without being
broadcast.

### 7.7 `DisasterDaySystem` (extend `CeremonySystem`)

**Owns:** the live `DisasterPool` outcomes: storms, outbreaks, alarms, and
supply failures, plus their recovery: postponement, indoor move, shift
splitting, and the recovery note. **Consumes:** weather, disease, alarm, and
food owners. **Data:** `calendar_disasters.json`. **Rules:** a ruined day is
never a punishment; every disaster has a recovery path; the year's roll
records what happened and what the shelter did.

### 7.8 `CeremonyRollSystem` (new, thin, `Ashfall.Core.Narrative`)

**Owns:** the roll, year volumes, stone marks, and review lessons. **Data:**
`calendar_roll.json`. Records through `StandingRecord` and the vault's
chronicle volumes. **Rules:** the roll records days, attendance counts,
disasters, and lessons; year volumes bind through the vault; the stone carries
the year and the number of observances kept.

### 7.9 Systems explicitly not added

- No second faith, memory, food, diplomacy, morale, or records system.
- No religious doctrine, conversion, or proselytizing.
- No forced attendance or absence penalties.
- No truce magic or war-disruption mechanics.
- No feast gluttony, waste, or currency.
- No new RNG stream beyond the live day path.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `calendar_year.json` (new)

```json
{
  "schema_version": 1,
  "observances": [
    {
      "observance_id": "obs_founding_day",
      "display_name": "Founding Day",
      "ceremony_id": "ceremony_founding_day",
      "season": "spring",
      "day_index": 40,
      "prep_days": 3,
      "room_id": "room_festival_hall",
      "tags": ["civic", "anchor"]
    }
  ]
}
```

### 8.2 `calendar_prep.json` (new)

Prep: observance, supplies, rooms, banners, seating, wood, repair tasks.

### 8.3 `calendar_feasts.json` (new)

Feasts: menu, first tastes, shared dishes, kitchen shifts, store limits.

### 8.4 `calendar_guests.json` (new)

Guests: invitee, kind, gate day, quarters, seating, departure, refusal note.

### 8.5 `calendar_truces.json` (new)

Truces: term, party, duration days, witnesses, violation, result.

### 8.6 `calendar_observances.json` (new)

Observances: vigil, naming, anniversary, private, etiquette, consent.

### 8.7 `calendar_disasters.json` (new)

Disasters: kind, triggers, recovery path, notes, roll entry.

### 8.8 `calendar_roll.json` (new)

Roll: day, observance, held, attendance, disaster, lesson, year volume.

### 8.9 `calendar_songs.json` (new)

Songs: festival songs and their performance notes (performed via the pastime
owner).

### 8.10 `calendar_lessons.json` (new)

Lessons: year, observation, change for next year, keeper.

### 8.11 Items

New items appended to `items.json`: `item_banner_cloth`,
`item_festival_ribbon`, `item_vigil_lamp`, `item_lantern_path_candle`,
`item_feast_platter`, `item_first_taste_bowl`, `item_guest_shelf`,
`item_roll_ledger`, `item_year_nail`, `item_calendar_board`,
`item_gift_small_jar`, `item_solstice_log`, `item_naming_ribbon`,
`item_quiet_stone_chisel`, `item_roll_stone_chisel`, `item_festival_chair`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`CeremonySaveState` remains the live save owner. New sub-objects (calendar,
preparations, feasts, guests, truces, observances, disasters, roll, lessons)
are additive inside it. No new save section.

### 9.2 State to persist

- The planned year and observance dates.
- Preparation state and supplies contributed.
- Feast menus and store planning.
- Guest lists, arrivals, housing, and departures.
- Truce terms, witnesses, and violations.
- Vigil, naming, and anniversary state.
- Disaster outcomes and recovery actions.
- The roll, year volumes, and lessons.

### 9.3 Determinism

- Ceremony scheduling and outcomes stay on the live `TickDay` path with the
  authored `DisasterPool`.
- Morale and stress use the live boosts and relief constants.
- Truce duration uses `TruceDurationDays`; violations are recorded events.
- Guest arrival and weather use the live day and weather paths.
- Paired replay hashes must match; no `System.Random`.

### 9.4 Migration

Legacy saves load with the five existing ceremonies and any completed ids
intact; the calendar, roll, and lessons start empty. A shelter that already
kept a founding day keeps it on the roll with the recorded year.

### 9.5 Checksum

Invariant-culture floats; integer day, attendance, and year fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `CeremonyFestivalPanel` (extend) | Live ceremonies | `CalendarHostSession` |
| `YearBoardPanel` (new) | The civic calendar | same |
| `PreparationPanel` (new) | Supplies and rooms | same |
| `FeastPanel` (new) | Menus and stores | same |
| `GuestPanel` (new) | Invitations and housing | same |
| `TrucePanel` (new) | Terms and witnesses | same |
| `ObservancePanel` (new) | Vigils and naming days | same |
| `RollPanel` (new) | The roll and lessons | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Attendance is never required and absence carries no penalty text.
- Vigils and naming days can be attended, observed quietly, or declined.
- Disasters always show their recovery options before the outcome.
- Truce terms are readable in full before any signature.
- Keyboard/controller close/back preserved; focus maintained on refresh.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a chair dragged across a floor, a
candle set down, a banner snapping in wind, a hall going quiet before a name
is read, a fire catching in a ring of stones. No cue is required; text carries
meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `CeremonySystem` | Definitions, prep, truce, disasters |
| `NeedsSystem` | Morale and stress via `Modify` |
| `MemorialSystem` (Wave 3) | Vigils and names |
| 13 Faith | Civic-only boundary |
| `KitchenNutritionSystem` | Feast orders |
| `FoodPreservationSystem` | Store limits |
| `FactionStanding` ports | Invitation reads |
| `ShelterReputationSystem` | Standing reads |
| 45 The Envoy (Wave 7) | Treaty practice boundary |
| 48 The Pastime (Wave 8) | Music, games, ensembles |
| 50 The Vault (Wave 8) | Year volumes and songs |
| 46 The Long Change (Wave 7) | Year notes and stone |
| `WeatherSystem` (Wave 5) | Outdoor days and contingencies |
| Disease owner (Plan 09) | Outbreak disasters |
| `AlarmSystem` (Wave 3) | Alarm disasters |
| `ChildDevelopmentSystem` (Wave 6) | Naming days and first-years |
| 37 The Quickening (Wave 6) | Welcomings |
| `DutyRoster` (Exp 02) | Preparation labor (voluntary) |
| `StandingRecord` (Exp 03) | The roll |
| `Inventory` | Supplies and gifts |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm the ceremony system, save store,
panel, needs, memory, food, faction, rank, weather, disease, alarm, and
record owners. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author the ten catalogs; register validators
and scanner.

**Phase 2 — Pure Core.** `CalendarSystem`, `PreparationSystem`, `FeastSystem`,
`GuestSystem`, `TruceSystem`, `ObservanceSystem`, `DisasterDaySystem`,
`CeremonyRollSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `CalendarHostSession`, focused selftest coverage,
fresh journey from the year board to the winter reading.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Multi-year soak: preparation costs, attendance, truces,
disasters, and year-over-year morale.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Observances (total) | 12 |
| Preparation plans | 12 |
| Feasts | 6 |
| Guest classes | 8 |
| Truce terms | 10 |
| Observance etiquettes | 10 |
| Disasters | 12 |
| Roll years | 10 |
| Songs | 10 |
| Lessons | 12 |
| Items | 16 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 60,000–75,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Faith overlap | High | Civic-only boundary |
| Memory overlap | High | Vigils route through owner |
| Food overlap | Medium | Orders through owners |
| Diplomacy overlap | High | Read standing only |
| Forced attendance | High | Voluntary by contract |
| Truce simplicity | Medium | Terms and violations |
| Disaster punishment | High | Recovery paths |
| Determinism break | Low | Live day path |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `calendar_year.json` | 12 | 3,000 |
| `calendar_prep.json` | 12 | 3,000 |
| `calendar_feasts.json` | 6 | 2,000 |
| `calendar_guests.json` | 8 | 2,500 |
| `calendar_truces.json` | 10 | 3,000 |
| `calendar_observances.json` | 10 | 2,500 |
| `calendar_disasters.json` | 12 | 3,000 |
| `calendar_roll.json` | 24 | 4,000 |
| `calendar_songs.json` | 10 | 2,000 |
| `calendar_lessons.json` | 12 | 2,500 |
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
| R56-1 | Faith overlap | Med | High | Civic boundary |
| R56-2 | Memory overlap | Med | High | Owner routing |
| R56-3 | Food overlap | Low | Medium | Orders |
| R56-4 | Diplomacy overlap | Med | High | Read-only |
| R56-5 | Compulsion | Low | High | Voluntary |
| R56-6 | Truce magic | Med | Medium | Terms |
| R56-7 | Punishment days | Med | High | Recovery |
| R56-8 | Determinism | Low | High | Live path |
| R56-9 | Content overrun | Med | Medium | Budget |
| R56-10 | Tone pomp | Med | Medium | Small speeches |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **How many observances make a year?** Recommended: twelve, with four
   anchors (founding, vigil, solstice, harvest) and eight smaller days.
2. **Are truces proposed at every fair?** Recommended: no; case by case, with
   terms read and witnesses signed.
3. **Do guests from factions attend in person?** Recommended: where standing
   and safety allow, with quarters and a gate welcome; otherwise by letter.
4. **Can a ceremony be cancelled?** Recommended: postponed, never cancelled;
   the roll records the postponement and the reason.
5. **Who chooses the year's lessons?** Recommended: the year review meeting,
   with one lesson per observances file.

---

## 17. APPENDIX D — OBSERVANCE TABLE (12 DAYS)

| # | Observance | Season | Day | Prep | Morale | Kind |
|---|---|---|---|---|---|---|
| 1 | Founding Day | spring | 40 | 3 | 25 | anchor |
| 2 | The Green Week | spring | 90 | 2 | 18 | civic |
| 3 | First Sowing | spring | 120 | 2 | 20 | work-day |
| 4 | Long Day | summer | 172 | 3 | 22 | civic |
| 5 | The Fair | summer | 200 | 5 | 20 | truce |
| 6 | Midwinter Barter | summer | 240 | 3 | 18 | civic |
| 7 | Harvest Feast | autumn | 280 | 3 | 28 | anchor |
| 8 | The Roll Day | autumn | 310 | 2 | 15 | records |
| 9 | Long Night Bonfire | winter | 350 | 4 | 30 | truce |
| 10 | Remembrance Vigil | winter | 358 | 2 | 15 | vigil |
| 11 | Naming Day | winter | 362 | 2 | 25 | children |
| 12 | Year End | winter | 365 | 1 | 20 | readings |

The twelve days of the civic year, and the shape is deliberate: work-days,
record days, and truce days sit beside the anchors, because a calendar made
only of parties is a wish list. The naming day tucked two days before year end
is the expansion's warmest design choice, because a shelter that names its
children in front of everyone is a shelter that has decided to be a place.

---

## 18. APPENDIX E — PREPARATION TABLE

| # | Observance | Supplies | Rooms | Work |
|---|---|---|---|---|
| 1 | Founding Day | banners, small jars | hall, stone | speeches, repair list |
| 2 | Green Week | seeds, paint | rows, bench | planting, fence |
| 3 | First Sowing | seed lots | rows | planting, blessing none |
| 4 | Long Day | lamps, water | hall, field | games, shade |
| 5 | The Fair | booths, platters | field, gate | booths, welcome |
| 6 | Midwinter Barter | cloth, jars | hall | exchange tables |
| 7 | Harvest Feast | first tastes | kitchen, hall | dishes, chairs |
| 8 | The Roll Day | ledger, ink | roll room | writing, reading |
| 9 | Long Night Bonfire | logs, lanterns | ring, hall | fire, games |
| 10 | Remembrance Vigil | lamps, names | vigil room | lamps, silence |
| 11 | Naming Day | ribbons, book | hall | names, gifts |
| 12 | Year End | roll, volume | roll room | reading, stone |

Twelve preparation plans, and the supplies column shows the year's real cost:
banners, jars, seeds, logs, and ink. The Long Day row's shade work is included
because a summer celebration with no shade is a heat casualty drill, and the
year board's preparation checklist refuses to treat comfort as decoration.

---

## 19. APPENDIX F — FEAST TABLE

| # | Feast | First taste | Shared dish | Store limit | Kitchen |
|---|---|---|---|---|---|
| 1 | Harvest Feast | first root | great stew | 10% | full day |
| 2 | The Fair | new bread | booth loaves | 8% | two days |
| 3 | Long Day | early greens | cold plates | 5% | half day |
| 4 | Long Night | winter jam | fire pot | 12% | full day |
| 5 | Naming Day | sweet cakes | shared cake | 3% | morning |
| 6 | Year End | last jar | table soup | 5% | evening |

Six feasts with first tastes, shared dishes, and store limits, and the limit
column is the ethics written as arithmetic: no day is allowed to eat the
winter. The naming-day cakes at three percent are the year's most expensive
and most justified luxury, and the last jar on year end is a deliberate
ceremony of finishing rather than hoarding.

---

## 20. APPENDIX G — GUEST TABLE

| # | Guest class | Invite | Quarters | Seating | Departure |
|---|---|---|---|---|---|
| 1 | Waystation keeper | letter | guest room | near host | with thanks |
| 2 | Outpost family | letter | guest room | family row | with gifts |
| 3 | Neighbor settlement | letter | guest room | middle table | with escort |
| 4 | Trader party | standing | guest room | fair booth | by gate |
| 5 | Ridgeline families | letter | guest room | quiet row | with bread |
| 6 | Faction delegate | standing | guest room | host table | with witness |
| 7 | Wandering pair | gate | warm corner | informal | by gate |
| 8 | Child visitor | parent | family bay | children's row | with ribbon |

Eight guest classes with housing, seating, and departure practices, and the
seventh row is the honest one: a wandering pair arrives at the gate, and the
calendar's hospitality rule is that a warm corner and a meal are always
available whether or not the day was planned. The child visitor leaves with a
ribbon, which is what every shelter's first festival always ends up doing.

---

## 21. APPENDIX H — TRUCE TABLE

| # | Term | Party | Days | Witness | Result |
|---|---|---|---|---|---|
| 1 | No raids | outlaws | 3 | recorder | held |
| 2 | Road open | ridge | 5 | envoy | held |
| 3 | Water share | river | 10 | keeper | held |
| 4 | No toll | traders | 3 | fair | held |
| 5 | Guest safe | neighbors | 3 | host | held |
| 6 | No claim | quarry | 5 | witness | violated once |
| 7 | Fire watch | forest | season | ranger | held |
| 8 | Grave respect | valley | permanent | all | held |
| 9 | Market day | all | monthly | fair host | held |
| 10 | Winter aid | lowlands | each winter | both | held |

The tenth row is the year's quietest success: a winter aid agreement that
existed before the fair and was formalized because the calendar gave everyone
a date to say it on. The violated quarry row is included because honesty is
part of a truce: one violation, reported, witnessed, and answered without
ending the relationship.

---

## 22. APPENDIX I — DISASTER TABLE

| # | Disaster | Trigger | Recovery | Roll note |
|---|---|---|---|---|
| 1 | Storm on bonfire | weather | move inside | saved by hall |
| 2 | Outbreak at feast | disease port | shifts | saved by care |
| 3 | Alarm mid-ceremony | alarm | resume after | saved by drill |
| 4 | Supply shortfall | stores | substitute | saved by kitchen |
| 5 | Guest no-show | roads | letters | noted |
| 6 | Guest too many | gate | warm corner | noted |
| 7 | Speech too long | crowd | shortened | lesson |
| 8 | Faction dispute | standing | separate tables | resolved |
| 9 | Fire in ring | sparks | rake, water | saved by watch |
| 10 | Snow early | weather | indoor day | saved by hall |

Ten disasters with recoveries, and every row's recovery column contains the
same idea in different clothes: the shelter planned for the day, and when the
day went wrong, the plan absorbed it. The seventh row is the only one that is
the shelter's own fault, and it becomes the year's most quoted lesson.

---

## 23. APPENDIX J — OBSERVANCE ETIQUETTE TABLE

| # | Observance | Etiquette | Consent rule | Private option |
|---|---|---|---|---|
| 1 | Founding Day | short speeches | none | no |
| 2 | Green Week | work clothes | none | no |
| 3 | First Sowing | quiet hands | none | no |
| 4 | Long Day | shade, water | none | no |
| 5 | The Fair | friendly haggling | none | no |
| 6 | Midwinter Barter | modest gifts | none | yes |
| 7 | Harvest Feast | first taste shared | none | no |
| 8 | Roll Day | read the year | names only if asked | no |
| 9 | Long Night | fire discipline | none | no |
| 10 | Vigil | names with consent | required | yes |
| 11 | Naming Day | a name aloud | parents choose | yes |
| 12 | Year End | four minutes | none | yes |

The twelfth row's four-minute limit is the expansion's favorite rule: the
year end reading is timed, not because the year is small, but because the
people listening are standing. The vigil and naming rows carry every consent
rule in the calendar, and the private options on the last four days mean no
one is ever required to grieve or celebrate in public.

---

## 24. APPENDIX K — ROLL TABLE

| # | Year | Observances held | Attendance | Disasters | Volume |
|---|---|---|---|---|---|
| 1 | Year 1 | 7 | rising | 3 | bound |
| 2 | Year 2 | 9 | steady | 2 | bound |
| 3 | Year 3 | 10 | steady | 3 | bound |
| 4 | Year 4 | 11 | rising | 2 | bound |
| 5 | Year 5 | 12 | full | 4 | bound |
| 6 | Year 6 | 12 | full | 2 | bound |
| 7 | Year 7 | 12 | full | 3 | bound |
| 8 | Year 8 | 12 | full | 1 | bound |

Eight years of rolls, and the shape tells the story: seven observances in year
one because the shelter learned which days people kept coming back to, and
none ever dropped once it became a habit. The disasters column stays between
one and four every year, which is the honest figure for a shelter that lives
in weather.

---

## 25. APPENDIX L — SONG TABLE

| # | Song | Occasion | Parts | Learned by | Note |
|---|---|---|---|---|---|
| 1 | The long count | founding | 4 | all | slow |
| 2 | Seed and hand | sowing | 3 | choir | work song |
| 3 | Shade and water | long day | 2 | all | light |
| 4 | Fair morning | fair | 3 | hall | bright |
| 5 | First taste | harvest | 4 | all | round |
| 6 | The roll read back | roll day | 2 | reader | spoken-sung |
| 7 | Long night fire | bonfire | 5 | all | chorus |
| 8 | Names in lamps | vigil | 2 | few | quiet |
| 9 | New name | naming | 3 | all | soft |
| 10 | Another year | year end | 5 | all | closing |

Ten songs for ten occasions, performed through the pastime owner, and the
eighth is the calendar's heart: a two-part song sung while the lamps are lit,
with the names spoken between the verses, quiet enough that the hall can hear
the wicks. The tenth closes the year and is deliberately the easiest to sing,
because a closing song must not require practice to join.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_calendar_year_board` | 3 | Year board made |
| `quest_calendar_twelve` | 4 | Twelve days set |
| `quest_calendar_prep` | 4 | First prep week |
| `quest_calendar_hall` | 3 | Hall readied |
| `quest_calendar_founding` | 4 | Founding kept |
| `quest_calendar_vigil` | 4 | Vigil held |
| `quest_calendar_storm` | 5 | Storm day saved |
| `quest_calendar_guests` | 4 | Guests hosted |
| `quest_calendar_truce` | 5 | Truce held |
| `quest_calendar_scare` | 4 | Feast in shifts |
| `quest_calendar_first_year` | 4 | Naming day |
| `quest_calendar_quiet` | 3 | Private day |
| `quest_calendar_roll` | 4 | Roll written |
| `quest_calendar_reading` | 3 | Year read |
| `quest_calendar_days_we_set_aside` | 3 | Calendar ordinary |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_calendar_draft` | 3 | Year drafted |
| `quest_calendar_dates` | 3 | Dates fixed |
| `quest_calendar_rooms` | 3 | Rooms booked |
| `quest_calendar_seasons` | 3 | Seasons checked |
| `quest_calendar_notes` | 3 | Notes kept |
| `quest_calendar_supplies` | 3 | Supplies gathered |
| `quest_calendar_repair` | 4 | Hall repaired |
| `quest_calendar_banners` | 3 | Banners made |
| `quest_calendar_chairs` | 3 | Chairs built |
| `quest_calendar_wood` | 3 | Wood laid |
| `quest_calendar_first_taste` | 3 | First tastes saved |
| `quest_calendar_dishes` | 4 | Dishes planned |
| `quest_calendar_shared_table` | 3 | Shared dishes |
| `quest_calendar_kitchen_shift` | 3 | Kitchen honored |
| `quest_calendar_stores_note` | 3 | Stores respected |
| `quest_calendar_invite` | 3 | Invitations sent |
| `quest_calendar_gate` | 3 | Gate welcome |
| `quest_calendar_seating` | 4 | Seating fair |
| `quest_calendar_terms` | 4 | Terms read |
| `quest_calendar_witness` | 3 | Witness recorded |
| `quest_calendar_lamps` | 3 | Lamps lit |
| `quest_calendar_names` | 3 | Names read |
| `quest_calendar_naming` | 4 | Naming held |
| `quest_calendar_anniversary` | 3 | Private honored |
| `quest_calendar_song` | 3 | Song sung |
| `quest_calendar_roll_entry` | 3 | Roll written |
| `quest_calendar_volume` | 4 | Volume bound |
| `quest_calendar_stone` | 3 | Stone marked |
| `quest_calendar_review` | 4 | Year reviewed |
| `quest_calendar_lesson` | 3 | Lesson kept |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Larkin Stow** — calendar keeper. Kept a year board with five nails and
argued until it had twelve. Believes a date is a promise to the future.

**Marle Griss** — feast organizer. Saves the harvest's first taste for the
feast and the last jar for year end. Believes a shared dish is a sentence the
whole shelter wrote.

**Zell Wist** — master of ceremonies. Cuts speeches, shortens programs, and
puts the music before the speeches on purpose. Believes brevity is a form of
respect.

**Teal Thane** — banners. Sews cloth for a hall in winter and defends it with
the only argument that works: people stay longer where it looks like someone
cared. Believes decoration is infrastructure.

**Brun Eames** — guest host. Meets every arrival at the gate and never lets a
visitor stand alone. Believes hospitality is the cheapest diplomacy.

**Kerr Laine** — kitchen. Cooks the feast and wants the kitchen in the
program, not thanked at the end. Believes feeding people is the ceremony.

**Mace Niall** — lighting. Lights fires, hangs lamps, and checks the ring
before and after. Believes a festival is one careless spark from a serious
day.

**Wrenna Quin** — apprentice. Seventeen, keeps the roll, and writes the small
days' entries in careful ink. Believes a record is how a day becomes real.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Founding Stone** — a speech, a repair list, and a small jar for
  everyone.
- **The Long Night Ring** — logs, lanterns, and a rake kept close.
- **The Fair Field** — booths, bread, and a truce read aloud.
- **The Harvest Rows** — first tastes saved from first picking.
- **The Vigil Walk** — a lantern path and names spoken quietly.
- **The Guest Gate** — a welcome mat and a bowl of water at the door.
- **The Music Stand** — instruments, a stool, and the short program.
- **The First Year Tree** — ribbons and the marks of new names.
- **The Quiet Stone** — a private anniversary with no ceremony.
- **The Roll Stone** — day marks for a calendar that intends to continue. 

---

## 30. APPENDIX Q — CALENDAR CHARTER

| Clause | Promise |
|---|---|
| Kept | The year has dates, and they are planned before they arrive |
| Prepared | The work is the ceremony, and the work is shared |
| Voluntary | Nobody must attend, prepare, or celebrate |
| Fed | No day eats the winter; first tastes are shared |
| Hosted | Guests are housed decently and never used as leverage |
| Witnessed | Truce terms are read aloud and signed |
| Quiet | Vigils are short, names need consent, and grief may be private |
| Recovered | A ruined day is absorbed, recorded, and survived |
| Rolled | Every observance is entered in the roll |
| Yearned | The calendar expects another year, and says so |

The calendar charter is the expansion's first-class design object, nailed
beside the year board where the preparation lists go up. Its last clause is
the whole point of a civic calendar: the shelter writes dates it cannot yet
guarantee, and the writing is the promise.

---

## 32. APPENDIX R — WORKED CALENDAR YEAR

**Spring, month one.** Larkin nails twelve dates to the year board and loses
an argument about the eighth. The final calendar has four anchors and eight
small days, and the first preparation week begins the next morning with a list
of supplies that nobody has read aloud before.

**Spring, month two.** Founding Day is kept at the stone: short speeches, a
repair list read aloud, and a small jar for every resident. The shelter
repairs the hall's east door the same week because the list said so, and the
ceremony's actual content turns out to be the door.

**Spring, month three.** Green Week is a work observance: seeds, paint, and a
fence fixed. Attendance is voluntary and high because the day is outside and
the work is real, and the roll notes that the fence will outlast everyone who
built it.

**Summer, month five.** The Fair invites the valley. Brun meets eleven guests
at the gate, seats them with reasons, and witnesses three truce terms read
aloud: no raids for three days, the road open for five, water shared for ten.
There is one violation at the quarry table, reported the same evening, and the
fair continues, which is what a truce is for.

**Summer, month six.** Midwinter Barter runs on modest gifts and an exchange
table, and the rule that nothing is priced saves the day when two families
bring the same thing. The roll records: duplication resolved by gift.

**Autumn, month seven.** The Harvest Feast shares the first taste of every
first root and keeps ten percent of stores untouched. Marle's great stew feeds
everyone, and the kitchen is written into the program between the songs
because Kerr asked for it and was right.

**Autumn, month eight.** The Roll Day happens with the ledger open and a
reader who can read four minutes of names without stopping. The year is not
yet over, but the roll catches up, and the shelter discovers that writing a
year down makes the next one feel possible.

**Winter, month nine.** The Long Night Bonfire gets its storm. The wind
arrives at six, the ring cannot be lit, and the shelter moves the whole thing
into the hall: lamps, games, the fire pot, and the songs. It is better than
the planned version, and the roll says so in one line.

**Winter, month ten.** The Remembrance Vigil lights sixty-one lamps for the
shelter's dead, reads the names that families asked to have read, and keeps
silence for the rest. The vigil is eleven minutes long, and the private
option means four residents watch from the vigil walk instead of sitting in
the hall.

**Winter, month eleven.** The disease scare lands on the harvest feast's
second day, and the shelter eats in shifts for two days with a mask rule and a
separated room. The roll calls it a feast in shifts and the disaster notes
record that nobody went hungry and nobody got sicker, and the year's most
quoted sentence comes from the kitchen: we are feeding everyone, just not at
once.

**Winter, month twelve.** Naming Day gives three children their names in
front of the whole shelter, and the first-year tree gets three ribbons. The
child who is four and has been waiting all month to hear her name read stands
on a chair to be seen, and nobody moves her.

**Winter, last days.** The year end is read standing, timed at four minutes,
and closes with a song everybody knows. The roll is bound into a volume and
sent to the vault, the roll stone gets its year mark, and Larkin begins the
next year board with the same twelve nails and a lesson from the storm: plan
the outdoor day twice.

---

## 33. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Larkin reads the founding speech, which is four hundred words, and Zell has
> cut two hundred of them, and the ceremony ends on time, and the door is on
> the repair list and gets fixed, and the day is remembered as the year the
> speeches got shorter and the building got better.

> Marle holds back the first root from the pot and cuts it small and passes it
> around, and the youngest child gets the first piece, and the harvest feast
> becomes about that root more than the other nine dishes combined.

> The storm takes the bonfire and the shelter takes the storm, and the whole
> hall plays games by lamplight while the wind throws rain against the door,
> and the roll records one line: moved inside, better.

> The vigil's lamps are lit in a line, and the reader pauses at every name
> because the family is standing there, and the pause is the ceremony, and the
> silence after the last lamp is the rest of it.

---

## 34. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| No calendar | days drift | year board |
| No prep | last-minute chaos | prep week |
| Overpriced feast | stores eaten | 10% limit |
| Compelled attendance | resentment | voluntary rule |
| Long speeches | crowd leaves | four-minute rule |
| Broken truce | bitterness | report and witness |
| Ruined day | lost ceremony | recovery path |
| Spectacle grief | hollow | consent and quiet option |
| No roll | year forgotten | roll and volume |
| No lesson | repeat disaster | year review |

Every recovery here is a rule the calendar already contains, and the plan's
promise is that a year can be ruined in a hundred ways and still be worth
keeping, because the roll records what happened instead of pretending the day
was perfect.

---

## 35. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] No real faiths, holidays, or observances are copied.
- [ ] No doctrine, conversion, or proselytizing exists.
- [ ] Attendance and preparation are voluntary by contract.
- [ ] Morale and stress use `NeedsSystem.Modify` only.
- [ ] Vigils route through the memory owner.
- [ ] Feasts order from the food owners and never gut stores.
- [ ] Faction standing is read-only; truces stay ceremony outcomes.
- [ ] Disasters always have an authored recovery path.
- [ ] Save additions are additive inside `ceremony`.
- [ ] Determinism uses the live day path only.

---

## 36. APPENDIX V — GLOSSARY

- **Observance** — a day the shelter keeps, civic and optional.
- **Anchor** — one of the four major days of the year.
- **Preparation** — the work that makes the day real.
- **Feast** — a shared meal with first tastes and a store limit.
- **Vigil** — a short, dignified remembrance with consent.
- **Naming day** — when a child's name is said aloud to everyone.
- **Truce** — a dated agreement with terms, witnesses, and violations.
- **Disaster on the day** — the authored interruption and its recovery.
- **Roll** — the year's record of observances and lessons.
- **Year end** — the four-minute reading that closes the calendar.

---

## 37. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `CeremonySystem` | time, items | ceremonies, truces | standing |
| `CalendarSystem` | seasons | year board | ceremonies |
| `PreparationSystem` | inventory | prep state | inventory totals |
| `FeastSystem` | kitchen | menus | stores |
| `GuestSystem` | standing | guests | standing |
| `TruceSystem` | terms | truce state | diplomacy |
| `ObservanceSystem` | memory | observances | memory |
| `DisasterDaySystem` | weather | outcomes | weather |
| `CeremonyRollSystem` | history | roll | nothing |
| `NeedsSystem` | nothing | nothing | nothing |
| `MemorialSystem` | names | nothing | nothing |
| `KitchenNutritionSystem` | dishes | nothing | nothing |
| `FactionStanding` | standing | nothing | nothing |
| `WeatherSystem` | forecast | nothing | nothing |
| `PastimeHostSession` | songs | nothing | nothing |
| `VaultHostSession` | volumes | nothing | nothing |
| `StandingRecord` | records | records | nothing |

---

## 38. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`calendar_year.json`** — `observance_id`, `display_name`, `ceremony_id`,
`season`, `day_index`, `prep_days`, `room_id`, `tags[]`.

**`calendar_prep.json`** — `observance_id`, `supplies[]`, `rooms[]`,
`banners`, `seating`, `wood`, `repairs[]`, `tags[]`.

**`calendar_feasts.json`** — `feast_id`, `first_taste`, `shared_dish`,
`store_limit_pct`, `kitchen_hours`, `tags[]`.

**`calendar_guests.json`** — `guest_class`, `invite_kind`, `quarters`,
`seating`, `departure`, `refusal_note`, `tags[]`.

**`calendar_truces.json`** — `term_id`, `party_id`, `duration_days`,
`witness_id`, `violation`, `result`, `tags[]`.

**`calendar_observances.json`** — `observance_id`, `etiquette`, `consent_rule`,
`private_option`, `tags[]`.

**`calendar_disasters.json`** — `disaster_id`, `kind`, `trigger`, `recovery`,
`roll_note`, `tags[]`.

**`calendar_roll.json`** — `day`, `observance_id`, `held`, `attendance`,
`disaster_id`, `lesson`, `volume_id`, `tags[]`.

**`calendar_songs.json`** — `song_id`, `occasion`, `parts`, `learned_by`,
`note`, `tags[]`.

**`calendar_lessons.json`** — `lesson_id`, `year`, `observation`, `change`,
`keeper_id`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid ceremony or item references, or out-of-range
numbers.

---

## 39. APPENDIX Y — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Observances planned | intention | Calendar |
| Observances held | follow-through | Roll |
| Attendance | belonging | Roll |
| Preparation days | effort | Prep |
| Feast store draw | food ethics | Feasts |
| Guests hosted | openness | Guests |
| Truces held | valley quiet | Truces |
| Violations reported | honesty | Truces |
| Disasters recovered | resilience | Roll |
| Lessons kept | wisdom | Lessons |

Telemetry is diagnostic only; it never gates content and never ranks a
resident, a cook, or a year.

---

## 40. APPENDIX Z — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive inside `ceremony`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows a year kept, a day ruined and saved, a truce held.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No faith, compulsion, or punishment-day content exists.

---

## 41. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. Does the calendar survive a change of shelter leadership?
2. Can an observance be added mid-year, or must it wait for the review?
3. Who may invite a faction, and who may revoke an invitation?
4. Does a truce violation ever have consequences beyond a witnessed report?
5. Are guests ever denied entry, and how is that decision recorded?
6. Can a family ask for a name to be read at the vigil without attending?
7. What happens to the year board when a resident dies mid-year?
8. Does the calendar ever get smaller, and who decides that?

None of these may be decided unilaterally; each changes tone and balance.

---

## 42. APPENDIX AB — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Naming days and first-years |
| 1 | 13 Faith | Civic-only boundary |
| 2 | 17 The Long Evening | Elders reading the roll |
| 3 | 24 The Long Goodbye | Vigil practice and names |
| 3 | 26 The Common Table | Feast dishes and stores |
| 4 | 28 The Lesson | Children's parts in ceremonies |
| 4 | 30 The Press | Programs, rolls, and song sheets |
| 5 | 33 The Weather | Outdoor days and contingencies |
| 5 | 34 The Long Road | Guest arrivals and escorts |
| 6 | 37 The Quickening | Welcoming days |
| 6 | 41 The Quiet | Silence rules at vigils |
| 7 | 45 The Envoy | Treaty practice and witnesses |
| 7 | 46 The Long Change | Year notes and stones |
| 8 | 48 The Pastime | Songs, games, and performances |
| 8 | 50 The Vault | Year volumes and preserved songs |
| 9 | 55 The Quarter | Blocks at festivals and vigils |

Each hook is additive. The Calendar can ship alone, and every other expansion
can ship without it.

---

## 43. APPENDIX AC — ENDING PROSE SKETCHES

**The Kept Year.** Twelve observances, a full roll, and children who know the
dates by heart.

**The Open Gate.** The fair becomes the valley's fixed meeting, and neighbors
plan their year around the shelter's calendar.

**The Truce Year.** Truces become a regular, honest practice with terms and
witnesses, and the valley has quiet days it can name.

**The Quiet Observance.** The shelter keeps fewer, deeper days, and the private
anniversaries matter as much as the public ones.

**The Long Table.** The feast becomes the year's center, and the harvest's
first taste is shared before its last is stored.

**Fade.** A hall full of mismatched chairs, a banner reused for its fourth
year, a roll with fresh ink, and a child's name read aloud.

---

## 44. APPENDIX AD — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Real holidays | legal/tone risk | invented days |
| Doctrine | faith overlap | civic only |
| Compelled joy | cruelty | voluntary |
| Morale fountain | balance break | authored boosts |
| Feast gutting | food harm | store limits |
| Truce magic | diplomacy break | terms and witnesses |
| Grief spectacle | harm | consent and quiet |
| Ruined-day punishment | unfair | recovery paths |
| Grand speeches | tedium | four-minute rule |
| No records | amnesia | roll and volume |

The list exists because a festival system is easy to write as either a morale
vending machine or a pageant. The expansion's rule is that the calendar is
small, civic, voluntary, and written down, and that its best moment is a hall
of mismatched chairs going quiet. 

---

## 45. APPENDIX AE — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Observances | 12 | 3,000 |
| Preparation plans | 12 | 3,000 |
| Feasts | 6 | 2,000 |
| Guest classes | 8 | 2,500 |
| Truce terms | 10 | 3,000 |
| Etiquettes | 12 | 2,500 |
| Disasters | 10 | 3,000 |
| Roll years | 8 | 3,000 |
| Songs | 10 | 2,000 |
| Lessons | 12 | 2,500 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~57,500** |

---

## 46. APPENDIX AF — FIRST CALENDAR YEAR

| Month | Focus | Milestone |
|---|---|---|
| 1 | Board | twelve dates |
| 2 | Prep | first week |
| 3 | Founding | first kept day |
| 4 | Green Week | work observance |
| 5 | Long Day | summer games |
| 6 | Fair | valley guests |
| 7 | Barter | exchange tables |
| 8 | Harvest | first taste |
| 9 | Roll | ledger caught up |
| 10 | Bonfire | storm day saved |
| 11 | Vigil | sixty-one lamps |
| 12 | Naming | three ribbons |

Twelve months in the order the shelter chose: civic days early, work days in
season, the fair when the roads are good, the feast when the harvest is in,
and the hardest days saved for the dark — which is where a calendar earns its
keep.

---

## 47. APPENDIX AG — LESSON TABLE

| # | Year | Observation | Change | Keeper |
|---|---|---|---|---|
| 1 | 1 | speeches ran long | cut to four minutes | Zell |
| 2 | 2 | outdoor day rained | plan twice | Larkin |
| 3 | 3 | feast ate stores | 10% cap | Marle |
| 4 | 4 | vigil too long | eleven minutes | Larkin |
| 5 | 5 | guests unseated | seating plan | Brun |
| 6 | 5 | truce unwitnessed | witness rule | Larkin |
| 7 | 6 | children missed | child row | Teal |
| 8 | 7 | kitchen unthanked | program line | Kerr |
| 9 | 8 | private day ignored | quiet option | Larkin |
| 10 | 9 | roll gaps | weekly roll | Wrenna |

Ten lessons across nine years, and the pattern is the calendar getting
incrementally better at being a calendar: shorter, earlier, fairer, and more
quiet. The ninth lesson is the one the shelter is proudest of, because adding
a private option to a public ceremony is how an institution proves it
understands the people it is for.

---

## 48. APPENDIX AH — YEAR REVIEW TABLE

| # | Question | Answer source | Action |
|---|---|---|---|
| 1 | Which days were kept? | roll | repeat |
| 2 | Which days were missed? | roll | reschedule |
| 3 | Which days were best? | attendance | protect |
| 4 | Which disasters hit? | disasters | plan |
| 5 | Which guests returned? | guests | invite |
| 6 | Which truces held? | truces | extend |
| 7 | Which rules changed? | lessons | post |
| 8 | What was too long? | survey | shorten |
| 9 | What was missed? | survey | add |
| 10 | What is next year? | all | draft |

Ten review questions asked once a year with the roll open, and the answers are
supposed to be boring: which days worked, which dishes were best, which
speech ran long. A year review that produces drama has probably been conducted
badly, and the table's tenth question is the only one that needs imagination.

---

## 49. APPENDIX AI — CALENDAR COVENANT

| Clause | Promise |
|---|---|
| Dated | The year has fixed days, and they are planned in advance |
| Voluntary | Attendance, labor, and celebration are choices |
| Prepared | The work is shared and the kitchen is honored |
| Fed | No day eats the winter, and first tastes are for everyone |
| Open | Guests are welcome within standing and safety |
| Witnessed | Truces are read aloud, signed, and reported honestly |
| Gentle | Vigils are short, names need consent, grief may be private |
| Recovered | A ruined day is absorbed and recorded |
| Rolled | The year is written down and bound |
| Expected | The calendar assumes another year, and plans for it |

The calendar covenant is the expansion's first-class design object, kept in
the roll room beside the year volumes. Its last clause is the reason the
shelter keeps a calendar at all: a community that writes down next year's
dates has made a quiet decision about its own survival, and the decision is
the ceremony.

---

## 50. APPENDIX AJ — CALENDAR SUCCESSION TABLE

| Role | First | Successor | Handover |
|---|---|---|---|
| Calendar keeper | Larkin | Wrenna | one full year |
| Feast organizer | Marle | Kerr | one harvest |
| Master of ceremonies | Zell | Teal | one program |
| Banners | Teal | seamstress | one hall |
| Guest host | Brun | gate watch | one fair |
| Kitchen | Kerr | cook | one feast |
| Lighting | Mace | watch | one bonfire |
| Apprentice | Wrenna | next recruit | one roll year |

The succession table is measured in years and harvests, and the calendar
keeper's handover takes a full year because a calendar is not a file to hand
over but a rhythm to inherit. The apprentice's roll year is the last column
and the most important: the person who writes the year down is the person who
will keep it.

---

## 51. APPENDIX AK — HOSTING TABLE

| # | Duty | Host | Check | Note |
|---|---|---|---|---|
| 1 | Gate meet | Brun | list | every arrival |
| 2 | Tea first | gate watch | kettle | before business |
| 3 | Quarters | guest host | beds | before night |
| 4 | Seating | Brun | reasons | no lone guest |
| 5 | Meals | Kerr | shift plan | with family |
| 6 | Program | Zell | time | four minutes |
| 7 | Goodbyes | Brun | bread | by the gate |
| 8 | Letters after | Larkin | thanks | within a week |
| 9 | Gifts | Teal | wrap | small and useful |
| 10 | Witness | recorder | terms | signed copy |

Ten hosting duties with checks, and the second row is the calendar's oldest
and best rule: tea before business. The table also shows what hospitality
costs in labor — a gate, a kettle, beds, bread, and a letter — and the
calendar's position is that this is the cheapest diplomacy the shelter will
ever practice.

---

## 52. APPENDIX AL — VIGIL TABLE

| # | Element | Rule | Consent | Note |
|---|---|---|---|---|
| 1 | Lamps | one per name | family choice | lit in a line |
| 2 | Names | read aloud | asked in advance | initials if preferred |
| 3 | Silence | after last lamp | all | one minute |
| 4 | Length | eleven minutes | all | no speeches |
| 5 | Private option | vigil walk | optional | observed quietly |
| 6 | Children | seated row | parents choose | explained gently |
| 7 | Music | one quiet song | choir | before names |
| 8 | Closing | lamp left lit | all | until morning |

Eight vigil rules, and the lamp left burning until morning is the small ritual
that ends the day without ending the care. The initials row exists because some
families do not want a name spoken in public, and a vigil that cannot offer
initials is a vigil that does not understand grief.

---

## 53. APPENDIX AM — TRUCE WITNESS TABLE

| # | Role | Duty | Signs | Note |
|---|---|---|---|---|
| 1 | Recorder | reads terms | yes | one copy each |
| 2 | Host | opens the table | yes | witness |
| 3 | Guest witness | confirms | yes | from the other party |
| 4 | Keeper | keeps the copy | no | in the roll room |
| 5 | Violation reporter | reports | no | same day preferred |
| 6 | Mediator | reads again monthly | no | checks terms |

Six roles in a truce, and the recorder's job is the one the valley's meetings
depend on: terms that are only spoken are terms that will be remembered
differently by everyone in the field. The mediator's monthly re-read is the
quiet maintenance that keeps a three-day truce from quietly ending on the
eighth day.

---

## 54. CLOSING STATEMENT

ASHFALL already models ceremony with real feeling: definitions with
preparation days, required rooms and items, morale and stress values, faction
invitations accepted or refused, truce durations, a disaster pool that can
interrupt the day, and a save record of ceremonies held and disasters endured.
It has five ceremonies and no calendar. The Calendar adds the year: twelve
observances with preparation, food, guests, truces, vigils, naming days,
disasters and recoveries, a roll, year volumes, and lessons kept for next
year. It adds no faith and no compulsion, and it leaves the shelter with the
smallest and most stubborn civic achievement there is — a date that everyone
agreed to expect.

> Wave 9 note: this plan is one of five Wave 9 expansion bibles (52–56). Each is
> self-contained; none requires another to ship. The shared Wave 9 index lives
> at `docs/expansions/wave9/WAVE9_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `CeremonySystem` (`SystemId` = "ceremony_system",
> `CeremonyDefinition` with `PreparationDays`, `RequiredRoomId`,
> `MinPopulation`, `RequiredItems`, `MoraleBoost` default 25f, `StressRelief`
> default 20f, `TruceDurationDays`, `TruceEligible`, `DisasterPool`;
> `CeremonySaveState` with `CompletedCeremonyIds`, `TotalCeremoniesHeld`,
> `TotalDisastersEncountered`; `ScheduleCeremony`, `ContributeResource`,
> `InviteFaction`, `TickDay(outcomeSummary)`, `CaptureState`, `RestoreState`),
> `CeremonySaveStore` under `ceremony`, `CeremonyFestivalPanel`, and
> `ceremonies.json` (five ceremonies: Founding Day morale 25 prep 3,
> Remembrance Vigil morale 15 prep 2, Long Night Solstice Bonfire morale 30
> prep 4 truce-eligible, Grand Treaty Barter Fair morale 20 prep 5
> truce-eligible, Ashfall Hydroponic Harvest Feast morale 28 prep 3; each with
> a two-entry disaster pool).