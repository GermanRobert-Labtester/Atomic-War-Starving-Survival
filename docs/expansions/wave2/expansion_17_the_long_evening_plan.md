# ASHFALL — Expansion 17 Design Bible
# THE LONG EVENING
### Wave 2 · Leisure, Music, Performance, Art, Games, and Festival Culture

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-20
**Domain owners touched:** `Ashfall.Core.Recreation`, `Ashfall.Core.Culture`, `Ashfall.Core.Survivors` (Hobby), `Ashfall.Core` (VinylMorale), `Ashfall.Core.InformationFlow` (Rumor)
**Proposed host owner:** `CultureHostSession` (extends `RecreationSaveStore` + cultural archive stores)
**Existing save sections:** `recreation`, `cultural_archives`, vinyl/record state, archive desk
**Existing CLI verbs:** `--recreation-selftest`, `--culture-selftest`, `--vinyl-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already knows that survivors need more than calories. `NeedsSystem` tracks
morale; `MoraleContagionSystem` spreads it; `SurvivorDowntimeSystem` runs hobbies
with real stress relief, trait compatibility, social range, and even brawl risk;
`HobbySystem` records practice; `VinylMoraleSystem` plays scavenged records and
already contains a cultural-broadcast bridge; `CultureCreationSystem` produces
artworks across six media and six themes; `CulturalArchiveVaultSystem` preserves
tomes; `DocumentationSystem` records the shelter's life. But the authored content is
a sketch: **six hobbies**, a cassette corpus, a tome list, and almost no places,
people, or systems for culture as a *shared, public, recurring* part of shelter life.

**The Long Evening** turns that sketch into a culture: performances, festivals,
sports, games, galleries, broadcasts, and the nightly gathering that gives a shelter
a reason to keep the lights on. It is the expansion about what people do with the
hours they are not surviving.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin; **`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

Every shelter has an evening. The work shift ends, the ration is eaten, the lamps
dim to save power, and there are two or three hours left before sleep. In the first
months those hours are spent repairing gear and worrying. In the years after, if a
shelter is lucky, those hours become something else: a guitar passed around, a card
game with rules nobody remembers correctly, a story told to children who were born
underground, a radio program listened to in silence, a play performed with a bucket
for a crown.

Culture is not a luxury in ASHFALL. It is how a shelter remembers that it is a
shelter and not a machine. It is also, quietly, a survival technology: morale is a
need, grief is a hazard, and a festival that brings two rival shelters to the same
table can prevent a war.

**The Long Evening** expands the leisure and culture layer from six hobbies into a
full social life: live performance, festivals, competition, gambling, art as
currency and memory, and public culture that can unite or divide a settlement.

### 1.2 The five loops it adds

```
   Hobby practice ──► Mastery ──► Performance ──► Audience morale
        │                                            │
        ▼                                            ▼
   Materials & rooms ──► Artwork / song / play ──► Display, trade, memory
        │                                            │
        ▼                                            ▼
   Evening calendar ──► Festival / game / sport ──► Cohesion, rivalry, rumor
        │                                            │
        ▼                                            ▼
   Broadcast bridge ──► Regional culture ──► Recruitment, faction standing
```

### 1.3 What the player manages

1. **Evening time.** `SurvivorDowntimeSystem` already spends hours on hobbies. The
   expansion makes the evening an allocatable block: rest, hobby, performance,
   festival, or extra shift.
2. **Hobby mastery.** `HobbySystem` records practice; the expansion gives practice
   levels, mastery perks, and performance readiness.
3. **Public culture.** A performance or festival is a *scheduled event* with
   preparation, audience, morale, and risk (a bad show, a brawl, a fire).
4. **Art as property and memory.** `CultureCreationSystem` artworks become displayable,
   tradeable, commemorative objects with provenance and value.
5. **Games and wagers.** Card games already exist with brawl risk; the expansion adds
   tournaments, wager rules, and gambling consequences without creating a new economy.
6. **Festivals.** Recurring, authored, calendar-bound events that consume supplies and
   produce cohesion, rumor, and diplomatic contact.
7. **Broadcast culture.** Vinyl and live performance can be broadcast, using the
   live `VinylMoraleSystem` broadcast-bridge fields, to reach the region.

### 1.4 What it is not

- Not a second morale system. Everything routes through `NeedsSystem` and
  `MoraleContagionSystem`.
- Not a second economy. Art and wagers use `Inventory` and existing trade value.
- Not a minigame collection. Systems are simulation-first; any interactive element is
  optional and never required.
- Not a content mill. Every artwork, song, and play is authored and fictional.
- Not a real-world religion, holiday, or sport. All festivals and games are fictional.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Recreation/SurvivorDowntimeSystem.cs` | Hobby sessions, room/trait/social gating, stress relief, brawl risk | `LIVE` |
| `Assets/Ashfall.Core/Survivors/HobbySystem.cs` | Hobby practice records | `LIVE` |
| `Assets/Ashfall.Core/VinylMoraleSystem.cs` | Records, play state, morale, flashback suppression, broadcast bridge | `LIVE` |
| `Assets/Ashfall.Core/Narrative/VinylRecordCatalog.cs` | Record definitions | `LIVE` |
| `Assets/Ashfall.Core/Narrative/VinylRecordAcquisitionMap.cs` | Record acquisition | `LIVE` |
| `Assets/Ashfall.Core/Culture/CultureCreationSystem.cs` | Artwork creation: media + theme + quality + impact | `LIVE` |
| `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs` | Tome preservation | `LIVE` |
| `Assets/Ashfall.Core/Culture/CulturalArchiveTomeCatalog.cs` | Tome catalog | `LIVE` |
| `Assets/Ashfall.Core/Culture/DocumentationSystem.cs` | Shelter record-keeping | `LIVE` |
| `Assets/Ashfall.Core/Culture/ArchiveChronicleMilestones.cs` | Chronicle milestones | `LIVE` |
| `Assets/Ashfall.Core/InformationFlow/RumorSystem.cs` | Rumor truth/decay | `LIVE` |
| `Assets/Ashfall.Core/Narrative/CulinaryRationCatalog.cs` | Ration culture | `LIVE` |
| `Assets/Ashfall.Core/Narrative/RefrigerationFermentationCatalog.cs` | Fermentation culture | `LIVE` |
| `src/Host/RecreationSaveStore.cs` | Recreation persistence | `LIVE` |
| `src/UI/RumorBoardPanel.cs`, `src/UI/PropagandaPanel.cs` | Related UI | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Entries | Notes |
|---|---|---|
| `recreation.json` | **6 hobbies** | whittling, guitar, harmonica, card games, sketching, storytelling |
| `cassette_sets.json` | ~30 KB of sets/parts | pre-war recorded testimony |
| `cultural_archive_tomes.json` | ~9.5 KB | technical and cultural tomes |
| `collectibles.json` | ~40 collectibles | discovery collection |
| `trophies.json` | trophy rows | hunt trophies as morale display |
| `archive_inks.json` | ink definitions | archive preservation |
| `radio.json`, `year_of_ash_radio.json` | broadcast catalog | radio content |

### 2.3 Confirmed gaps

- **GAP-17-1 — Six hobbies.** `recreation.json` cannot express the leisure life of a
  mature shelter: no sport, no theater, no dance, no woodcut, no choir, no chess.
- **GAP-17-2 — No live performance.** Nothing schedules a performance with an audience,
  a stage, preparation, or a result.
- **GAP-17-3 — No festival system.** Ceremonies exist for ritual (Expansion 13's
  domain) but no secular festival calendar, no seasonal celebration, no inter-shelter
  gathering.
- **GAP-17-4 — No sport or competition.** No games beyond cards, no tournament, no
  team, no arena.
- **GAP-17-5 — No art economy.** `CultureCreationSystem` records artworks but nothing
  displays, values, trades, or commemorates them at scale.
- **GAP-17-6 — No museum or gallery.** No location, room, or exhibit system.
- **GAP-17-7 — Vinyl is a near-island.** The broadcast bridge exists but there is no
  authored broadcast-culture content or regional audience model.
- **GAP-17-8 — No culture locations or NPCs.** No theater, music hall, gallery,
  stadium, or print shop in `locations.json`.
- **GAP-17-9 — Cassettes are inert.** The cassette corpus is not consumed by any
  listening/evening system.

### 2.4 Non-duplication statement

This expansion will **not** add a second morale system, a second needs system, a
second economy, a second rumor system, a second ceremony/ritual system
(`SpiritualMeaningCoordinator` owns ritual time), a second radio authority, or a
second save section. It extends `SurvivorDowntimeSystem`, `HobbySystem`,
`VinylMoraleSystem`, and `CultureCreationSystem`, and consumes `NeedsSystem`,
`MoraleContagionSystem`, `Inventory`, `RadioTuner`, and `RumorSystem`.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Culture is infrastructure.** A festival is morale, memory, and diplomacy
at once. It is not decoration; it is how a shelter survives its own grief.

**Pillar 2 — The evening is finite.** Leisure competes with sleep, extra shifts, and
the power budget. Every good evening has a cost.

**Pillar 3 — Bad art is still art.** A terrible song sung sincerely does more for a
shelter than a perfect silent one. Quality matters, but participation matters more.

**Pillar 4 — Public culture creates friction.** A play can insult a faction. A sport
can hurt someone. A wager can ruin a survivor. Culture is not safe.

**Pillar 5 — Memory is the long harvest.** The expansion's deepest loop is
commemoration: artworks and performances that outlive their makers and feed the
chronicle.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A performance | Cracked voice, attentive silence, a lamp | Stage glamour |
| A festival | Shared food, a game, a rumor traded | Carnival spectacle |
| A sport | Mud, an injury, an argument over rules | Arena spectacle |
| Gambling | A small stake, a debt, guilt | Casino glamour |
| A gallery | A wall of objects with names | Museum of the apocalypse |
| Commemoration | A song written for one dead person | Triumphal anthem |

### 3.3 Content limits

- All songs, plays, games, and festivals are fictional and setting-specific.
- No real-world sport, holiday, religious music, or copyrighted work.
- Gambling is consequential, never glamorized, and never targets a real addiction.
- No sexual content, no exploitation of minors, no demeaning caricature.

---

## 4. THE LONG EVENING WORLD

### 4.1 Interior rooms

- **`room_common_hall`** — the primary gathering space; extends mess-hall seating.
- **`room_music_corner`** — instruments, a stool, and enough room for an audience.
- **`room_gallery_wall`** — a display wall for artworks and trophies.
- **`room_museum_store`** — climate-controlled storage for fragile pieces.
- **`room_stage_small`** — a raised platform for plays and performances.
- **`room_game_room`** — tables, cards, dice, and a wager ledger.
- **`room_press_room`** — printing for playbills, newspapers, and pamphlets.
- **`room_listening_room`** — a turntable, a cassette player, and benches.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_buried_theater` | The Buried Theater | 5 | Pre-war auditorium; stage salvage, acoustics |
| `loc_film_depot` | The Film Depot | 6 | Reels, projectors, screens for cinema |
| `loc_press_works` | The Press Works | 5 | Printing press and type salvage |
| `loc_music_hall_ruins` | The Fallen Music Hall | 6 | Instruments, sheet music, a grand piano |
| `loc_stadia` | The Old Ground | 4 | Sports ground; rules and equipment |
| `loc_gallery_vault` | The Picture Vault | 7 | Pre-war art storage; contested ownership |
| `loc_cassette_shop` | The Tape Shop | 4 | Cassette corpus, players, blank tape |
| `loc_grandstand` | The Grandstand | 5 | The ruins of a racecourse; gambling history |
| `loc_festival_field` | The Common Field | 3 | Neutral ground for inter-shelter festivals |
| `loc_signal_pavilion` | The Signal Pavilion | 6 | Broadcast tower for cultural radio |

All locations require valid item references and scanner registration.

### 4.3 The evening clock

The expansion adds an authored **evening window** (typically two hours before the
sleep shift). The window is not a new clock; it is a derived schedule from the live
`Clock` and `DutyRoster` systems. Activities claim the window; overlapping activities
compete for participants, rooms, and power.

---

## 5. MAIN STORYLINE — "THE LONG EVENING"

### 5.1 Central conflict

The shelter has survived long enough that people have started to want things again —
not just food and safety, but music and stories and games. A musician named **Ora
Fenn** begins playing in the mess hall after shift, and attendance grows. At the same
time, a rival shelter proposes a joint festival on the Common Field. The shelter's
administrator, **Halden Vey**, sees the festival as a diplomatic opportunity and the
music as a waste of power.

The conflict sharpens when Ora writes a song about a massacre the shelter has spent
years not discussing — a song that names names, including a founder's. The festival
becomes contested: perform the song and risk a diplomatic incident, or suppress it
and become the kind of shelter that suppresses songs.

The expansion's question: **what is a shelter willing to remember publicly, and who
gets to decide what its evenings are for?**

### 5.2 Theme (unspoken)

**A shelter that has nothing to sing about will not last as long as one that does.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_musician_ora_fenn` | Ora Fenn | Musician | The evening's center; a quiet radical |
| `npc_administrator_halden_vey` | Halden Vey | Administrator | Guards power and order; not a villain |
| `npc_painter_lio_marr` | Lio Marr | Painter | Art as memory and as trade |
| `npc_player_tam_ost` | Tam Ost | Card player | Gambling, debt, and charm |
| `npc_coach_sera_du` | Sera Du | Athlete/coach | Competition and injury |
| `npc_archivist_mora_keel` | Mora Keel | Archivist | Custodian of what is kept and what is burned |
| `npc_child_choir_pim` | Pim | Child performer | The next generation's stake in culture |
| `npc_rival_emissary_dorn` | Dorn Vale | Rival emissary | The festival's other side |

### 5.4 Story beats (14)

1. **The First Evening.** Ora plays; attendance grows; Halden notices.
2. **The Wager.** Tam starts a card ledger; a debt begins.
3. **The Invitation.** The rival shelter proposes a joint festival.
4. **The Bill.** Halden posts the festival's power and ration cost.
5. **The Song.** Ora writes about the massacre; the shelter splits.
6. **The Archive.** Mora is asked to suppress or preserve the song.
7. **The First Sport.** Sera organizes a game; an injury starts a quarrel.
8. **The Gallery.** Lio's paintings become trade goods and political symbols.
9. **The Broadcast.** The Signal Pavilion is restored; a regional audience tunes in.
10. **The Rival Song.** The rival shelter brings its own version of the story.
11. **The Festival.** The Common Field gathering; a real diplomatic test.
12. **The Fire.** A festival accident; the evening turns dangerous.
13. **The Debt.** Tam's gambling debt comes due.
14. **What We Keep.** Final disposition: the song, the festival, the archive.

### 5.5 Branching choices (7)

| Choice | Options | Axis |
|---|---|---|
| The song | perform / edit / suppress | memory vs. peace |
| The festival | host / attend / decline | diplomacy vs. risk |
| Evening policy | leisure / work / mixed | morale vs. output |
| Art trade | sell / keep / gift | value vs. identity |
| Gambling | permit / regulate / ban | freedom vs. harm |
| Broadcast | open / censored / silent | openness vs. control |
| Archive | preserve everything / curate / burn | memory vs. comfort |

### 5.6 Endings (5 + fade)

1. **The Open Evening** — culture becomes central; morale and diplomacy rise.
2. **The Quiet Hall** — culture is suppressed; order holds and something is lost.
3. **The Burnt Archive** — the song is destroyed; the shelter becomes comfortable and forgetful.
4. **The Festival Truce** — the joint festival prevents a war and creates a tradition.
5. **The Debt Called In** — gambling corruption reaches the shelter's core.
6. **Fade** — the evening continues without a decision.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_eve_`. Schema follows `year_of_ash_quests.json`.

### 6.1 Main questline (14)

`quest_eve_first_evening`, `quest_eve_the_wager`, `quest_eve_invitation`,
`quest_eve_festival_bill`, `quest_eve_the_song`, `quest_eve_the_archive`,
`quest_eve_first_sport`, `quest_eve_the_gallery`, `quest_eve_the_broadcast`,
`quest_eve_rival_song`, `quest_eve_the_festival`, `quest_eve_the_fire`,
`quest_eve_the_debt`, `quest_eve_what_we_keep`.

### 6.2 Side quests (28)

**Music and performance (6)**
- `quest_eve_tune_instrument` — repair or make an instrument
- `quest_eve_first_song` — write a first song
- `quest_eve_choir` — form a choir
- `quest_eve_play_badly` — perform while untrained
- `quest_eve_encore` — an audience demands more
- `quest_eve_silent_night` — a performance cancelled by grief

**Art (5)**
- `quest_eve_paint_the_wall` — a mural project
- `quest_eve_portrait_of_the_dead` — a memorial portrait
- `quest_eve_art_forgery` — a claimed pre-war piece is a copy
- `quest_eve_gallery_trade` — sell or display
- `quest_eve_child_art` — children's artwork and what it reveals

**Games and wagers (5)**
- `quest_eve_card_rules` — codify the shelter's card rules
- `quest_eve_the_stake` — a wager with real goods
- `quest_eve_debt_collector` — a debt turns ugly
- `quest_eve_dice_night` — a gambling night and its consequences
- `quest_eve_chess_master` — a long-running rivalry

**Sport (4)**
- `quest_eve_field_rules` — agree on a sport
- `quest_eve_injury` — a player is hurt
- `quest_eve_team_rival` — two factions field teams
- `quest_eve_the_match` — a contest with stakes

**Festival (5)**
- `quest_eve_festival_prep` — supplies, permissions, program
- `quest_eve_festival_food` — feeding guests
- `quest_eve_festival_security` — keeping the peace
- `quest_eve_festival_program` — what is performed publicly
- `quest_eve_festival_after` — the morning after

**Archive and broadcast (3)**
- `quest_eve_cassette_listen` — the cassette corpus
- `quest_eve_broadcast_night` — the first cultural broadcast
- `quest_eve_archive_fire` — preserve or lose the archive

### 6.3 Repeatable quests (8)

`quest_eve_repeat_evening`, `quest_eve_repeat_practice`,
`quest_eve_repeat_listen`, `quest_eve_repeat_game`,
`quest_eve_repeat_perform`, `quest_eve_repeat_story`,
`quest_eve_repeat_gallery`, `quest_eve_repeat_broadcast`.

### 6.4 Dynamic hooks

Live systems already emit events for hobby sessions, vinyl plays, rumors, morale,
artwork creation, and archive milestones. The generator can attach authored
follow-ups without a new event bus.

### 6.5 Constraints

- No activity may bypass `NeedsSystem`; culture grants morale through the needs owner.
- No wager may create currency outside `Inventory` or the funds authority.
- No performance may be mandatory; opting out is always possible, at a small social cost.
- No festival may be free; it consumes real supplies and power.
- No content may depict real-world works or faiths.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `PerformanceSystem` (new, `Ashfall.Core.Recreation`)

**Owns:** performance definitions, preparation, scheduling, audience, quality roll,
and outcome facts. **Consumes:** `SurvivorDowntimeSystem` hobbies, `HobbySystem`
practice, rooms, `NeedsSystem`, `MoraleContagionSystem`. **Data:** `performances.json`.
**Rules:** a performance needs a room, a performer, and an audience; quality is a
deterministic function of practice, preparation, and environment; a failed show
costs morale rather than granting it.

```csharp
public sealed class PerformanceSystem
{
    public bool Schedule(string performanceId, string performerId, int day);
    public PerformanceResult Resolve(int day);
    public IReadOnlyList<PerformanceResult> History { get; }
}
```

### 7.2 `FestivalSystem` (new, `Ashfall.Core.Recreation`)

**Owns:** festival calendar, preparation phases, supply cost, guest capacity,
program, and outcome. **Consumes:** `Clock`/seasons, `Inventory`, `WeatherSystem`,
`FactionStanceEngine`, `RumorSystem`. **Data:** `festivals.json`.
**Rules:** festivals are scheduled and visible; they can be cancelled; they can
produce diplomatic contact, rumor, and a disaster event.

### 7.3 `ArtEconomySystem` (new, `Ashfall.Core.Culture`)

**Owns:** artwork value, provenance, display, trade, and commemoration.
**Consumes:** `CultureCreationSystem` artwork records, `Inventory`, `TradingSystem`.
**Data:** `artworks.json`, `museum_exhibits.json`.
**Rules:** value is authored and contextual; a commemorative work is worth more to
the shelter than to a buyer; no parallel currency.

### 7.4 `SportSystem` (new, `Ashfall.Core.Recreation`)

**Owns:** sport definitions, teams, matches, injuries, and stakes.
**Consumes:** `SurvivorDowntimeSystem`, `NeedsSystem`, `MedicalPipelineCoordinator`
for injuries. **Data:** `sports.json`.
**Rules:** injuries are real and route to the medical pipeline; competition raises
friction as well as morale.

### 7.5 `GamesOfChanceSystem` (new, `Ashfall.Core.Recreation`)

**Owns:** games, wager rules, stakes, debt, and gambling harm.
**Consumes:** `Inventory`, `Funds` authority, `NeedsSystem`, `RumorSystem`.
**Data:** `games_of_chance.json`.
**Rules:** no new currency; debt is a real obligation; a gambling addiction routes
through the existing psychological/dependency systems, never a parallel one.

### 7.6 `CulturalBroadcastSystem` (new, thin, `Ashfall.Core.Culture`)

**Owns:** cultural broadcast programs, regional audience, and reception.
**Consumes:** `VinylMoraleSystem` broadcast-bridge fields, `RadioTuner`/radio engine
where available, `FactionStanceEngine`. **Data:** `cultural_broadcasts.json`.
**Rules:** broadcasting is a radio transmission; it does not create a second radio
system; reception produces facts consumed by standing and recruitment.

### 7.7 `MuseumSystem` (new, thin, `Ashfall.Core.Culture`)

**Owns:** exhibits, curation, provenance display, and visitor morale.
**Consumes:** `ArtEconomySystem`, `CulturalArchiveVaultSystem`, `trophies`.
**Data:** `museum_exhibits.json`.

### 7.8 Systems explicitly not added

- No second morale, needs, economy, currency, rumor, ritual, or radio system.
- No gacha, no loot boxes, no monetized games.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `recreation.json` (extend 6 → 40)

Existing schema preserved (`hobby_id`, `display_name`, `duration_hours`,
`base_stress_relief`, `morale_effect`, `required_item_ids`, `optional_item_ids`,
`required_room_tags`, `compatible_trait_tags`, `incompatible_trait_tags`,
`social_min`, `social_max`, `brawl_risk`, `output_item_id`, `tags`). New hobbies
include: choir, drumming, dancing, chess, dice, woodcut printing, knitting, carving,
gardening-for-pleasure, radio listening, poetry, journaling, cooking-for-taste,
fishing-for-pleasure, birdwatching, map-drawing, knot-craft, whittling advanced,
puppetry, and storytelling advanced.

### 8.2 `performances.json` (new)

```json
{
  "schema_version": 1,
  "performances": [
    {
      "performance_id": "performance_shelter_ballad",
      "display_name": "The Shelter Ballad",
      "medium": "song",
      "duration_hours": 1,
      "required_room_tags": ["common", "stage"],
      "required_item_ids": ["item_acoustic_guitar"],
      "min_practice_level": 2,
      "audience_min": 4,
      "audience_max": 30,
      "base_morale_effect": 6.0,
      "failure_morale_cost": 2.0,
      "preparation_days": 2,
      "themes": ["memory", "loss", "endurance"],
      "tags": ["music", "public", "evening"]
    }
  ]
}
```

### 8.3 `festivals.json` (new)

Festival rows: id, name, season/day anchor, preparation days, supply cost, guest
capacity, program slots, morale/cohesion effect, rumor effect, disaster pool,
diplomatic weight.

### 8.4 `artworks.json` (new)

Artwork templates: medium, theme, quality band, base value, commemorative flag,
display tags, provenance rules.

### 8.5 `sports.json` (new)

Sport rows: name, players per side, space, equipment, duration, injury base,
stakes, morale effect, friction effect.

### 8.6 `games_of_chance.json` (new)

Game rows: name, players, equipment, stake rules, skill weight, luck weight,
harm profile, debt rules.

### 8.7 `museum_exhibits.json` (new)

Exhibit rows: piece, provenance, required room, morale effect, knowledge effect,
controversy flag.

### 8.8 `cultural_broadcasts.json` (new)

Program rows: content, source (vinyl/performance/live), frequency band, audience
band, reception effect, content warning flags.

### 8.9 `cassette_sets.json` (extend + wire)

The cassette corpus is existing content. The expansion adds a listening model:
cassettes are played in the listening room, consumed as evening activities, and can
be broadcast. No schema break; the corpus gains a consumption mapping.

### 8.10 Items

New items appended to `items.json`: `item_acoustic_guitar` (exists), `item_drum`,
`item_flute_bone`, `item_paint_set`, `item_canvas`, `item_chess_set`,
`item_dice_bone`, `item_playbill`, `item_stage_lamp`, `item_museum_case`,
`item_cassette_player`, `item_blank_cassette`, `item_trophy_shelf`,
`item_choir_book`, `item_wager_ledger`.

### 8.11 Rooms and locations

Rooms and locations are authored in `shelter_rooms.json` and `locations.json` using
their existing schemas.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`src/Host/RecreationSaveStore.cs` captures hobby sessions. The expansion adds
additive sub-objects inside the same envelope; cultural archive state extends its
existing store. No new save section.

### 9.2 State to persist

- Hobby practice levels and mastery (mostly live; extend).
- Performance history and scheduled performances.
- Festival history, programs, and completed prep.
- Artwork records and display placement (records exist; placement is additive).
- Sports records, injuries (injury routes to medical).
- Game/wager ledger and debts.
- Broadcast history.
- Museum exhibits.

### 9.3 Determinism

- Performance quality, festival success, sport outcomes, and gambling rolls use the
  host-forked `ISeededRng`.
- No wall-clock time; the evening window derives from the campaign clock.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with no performances, festivals, sports, wagers, or exhibits. Hobby
and vinyl state is untouched.

### 9.5 Checksum

Invariant-culture floats; integer-permille preferred for quality and morale deltas.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `RecreationPanel` (extend) | Hobbies, evening schedule, rooms | `CultureHostSession` |
| `PerformancePanel` (new) | Schedule, prepare, resolve performances | same |
| `FestivalPanel` (new) | Calendar, program, supplies, guests | same |
| `GalleryPanel` (new) | Artworks, display, trade, provenance | same |
| `SportPanel` (new) | Teams, matches, stakes, injuries | same |
| `GamesPanel` (new) | Games, wagers, debts, harm | same |
| `BroadcastPanel` (new) | Cultural programs and reception | same |
| `MuseumPanel` (new) | Exhibits and visitor morale | same |
| `ListeningPanel` (new) | Vinyl and cassette evening sessions | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- The evening schedule is readable in text and never color-only.
- Keyboard/controller close/back preserved; focus maintained across refresh.
- Wager stakes and harm are stated plainly; no hidden odds.
- Performances can be skipped with a clear, honest social cost.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: guitar string, audience murmur, applause,
a dropped card, a whistle, a stage lamp, a printer press. No cue is required; text
always carries meaning. The expansion deliberately avoids triumphant fanfare.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `NeedsSystem` | Morale via existing owner only |
| `MoraleContagionSystem` | Performance and festival facts |
| `SurvivorDowntimeSystem` | Event scheduling within the evening window |
| `HobbySystem` | Practice levels and mastery |
| `VinylMoraleSystem` | Play sessions and broadcast bridge |
| `CultureCreationSystem` | Artwork records extended with economy |
| `CulturalArchiveVaultSystem` | Tome preservation and museum |
| `RumorSystem` | Festival and performance rumors |
| `RadioTuner` / radio engine | Cultural broadcast |
| `FactionStanceEngine` | Diplomatic festival and broadcast reception |
| `Inventory` | Instruments, art, wager stakes |
| `PowerGridSystem` | Stage lamps, listening room, press |
| `MedicalPipelineCoordinator` | Sport injuries |
| `GuiltInsomniaSystem` | Suppressed-song and gambling guilt |
| `MemorialSystem` | Commemorative artworks |
| `EpilogueChronicleBuilder` | Culture milestones in the chronicle |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `SurvivorDowntimeSystem`, `HobbySystem`,
`VinylMoraleSystem`, `CultureCreationSystem`, `CulturalArchiveVaultSystem`,
`RumorSystem`, save stores, and related panels. Record file:line.

**Phase 1 — Data + validators.** Extend `recreation.json`; author performances,
festivals, artworks, sports, games, exhibits, broadcasts. Register validators and
scanner. No gameplay.

**Phase 2 — Pure Core.** `PerformanceSystem`, `FestivalSystem`,
`ArtEconomySystem`, `SportSystem`, `GamesOfChanceSystem`,
`CulturalBroadcastSystem`, `MuseumSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `CultureHostSession`, extended selftest verbs, fresh journey.

**Phase 5 — UI.** Extended and new surfaces with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, songs, plays, prose, audio.

**Phase 7 — Balance.** 30/90/180-day soak including evening allocation and festival cycles.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Hobbies | 34 new (6 → 40) |
| Performances | 40 |
| Festivals | 12 |
| Artwork templates | 30 |
| Sports | 10 |
| Games of chance | 10 |
| Museum exhibits | 25 |
| Cultural broadcasts | 20 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 14 |
| Side quests | 28 |
| Repeatable | 8 |
| Items | 15 |
| Songs/plays (authored) | 25 |
| Endings | 5 + fade |
| Prose estimate | 60,000–75,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Culture trivializes morale | High | Costs, competition, finite evening |
| Gambling becomes a currency exploit | High | No new currency; real debt; harm routing |
| Second morale/needs system | High | Route through existing owners |
| Tone drift to carnival | Medium | Tone table enforced in review |
| Content overrun | Medium | Authored budget per file |
| Save bloat (history) | Medium | Cap history; summarize old evenings |
| Determinism break | Low | Host-forked RNG only |
| UI overload | Medium | One panel per system; shared shell |

---

## 13. TEST AND VERIFICATION PLAN

### 13.1 New test files

- `Ashfall.Core.Tests/Recreation/PerformanceSystemTests.cs`
- `Ashfall.Core.Tests/Recreation/FestivalSystemTests.cs`
- `Ashfall.Core.Tests/Culture/ArtEconomyTests.cs`
- `Ashfall.Core.Tests/Recreation/SportSystemTests.cs`
- `Ashfall.Core.Tests/Recreation/GamesOfChanceTests.cs`
- `Ashfall.Core.Tests/Culture/CulturalBroadcastTests.cs`
- `Ashfall.Core.Tests/Recreation/CultureSaveRoundTripTests.cs`
- `Ashfall.Core.Tests/Recreation/CultureDeterminismTests.cs`
- `Ashfall.Core.Tests/Content/RecreationCatalogIntegrityTests.cs`

### 13.2 Required assertions

- Hobby stress relief stays within live bounds; no unbounded morale.
- Performances require room, performer, and audience; failure costs morale.
- Festivals consume supplies and power; cancellation is safe.
- Sport injuries route to the medical pipeline.
- Wagers never create currency; debt is an obligation.
- Broadcast uses the existing radio path; no second radio authority.
- Artwork value is authored; commemorative value differs from trade value.
- Round-trip restores practice, performances, festivals, artworks, wagers, exhibits.
- Legacy loads neutral; paired replay hash equality.

### 13.3 Commands

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Recreation/
bash scripts/run_test.sh Ashfall.Core.Tests/Culture/
godot --headless --path . -- --recreation-selftest
godot --headless --path . -- --culture-selftest
godot --headless --path . -- --vinyl-selftest
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
dotnet build Ashfall.csproj --no-restore
```

---

## 14. ACCEPTANCE CRITERIA

Core authority documented and engine-free; data canonical with valid schema and
passing integrity; persistence round-trips with neutral legacy and Triad parity;
determinism proven; host reachable by a real route/event; player can observe the
outcome; focused tests green; docs updated. Compile-green is not acceptance.

---

## 15. CROSS-EXPANSION HOOKS

| Expansion | Hook |
|---|---|
| 12 The Second Generation | Children's choir; coming-of-age performances |
| 13 The Faithful | Secular festival vs. sacred observance; shared calendar |
| 14 Above the Ash | Broadcast tower; cultural radio across the region |
| 15 The Deep Root | Harvest festival; ration culture |
| 16 The Rebuilt Body | Performing with prosthetics; machine musicians |
| 18 The Underneath | Deep-earth acoustics; cave galleries |
| 19 The Bitter Air | A play about the gas; quarantine songs |
| 20 The Quiet Hand | Censorship, rumor, and information control |
| 21 The Grid | The power cost of the evening; dark evenings |

---

## 16. LORE AND CONTINUITY CHECK

### 16.1 Must not contradict

- The live six hobbies and their stress/morale numbers.
- `VinylMoraleSystem`'s record and broadcast fields.
- `CultureCreationSystem`'s media/theme vocabulary.
- `RumorSystem` as the rumor authority.
- The restrained tone and fiction-only rule.

### 16.2 New canon

- The evening window and the Common Field festival.
- The massacre song and the question of public memory.
- Cultural broadcast as a regional force.
- The gallery and museum as shelters of memory.

### 16.3 Provenance

Registered in `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` when integrated.

---

## 17. APPENDIX A — HOBBY EXPANSION TABLE (40 TOTAL)

| # | Hobby | Social | Relief | Morale | Room | Risk |
|---|---|---|---|---|---|---|
| 1 | Wood Whittling *(LIVE)* | 1 | 15 | 5 | workshop | 0 |
| 2 | Acoustic Guitar *(LIVE)* | 1–4 | 24 | 10 | common | 0.02 |
| 3 | Harmonica *(LIVE)* | 1–2 | 18 | 6 | common | 0.01 |
| 4 | Card Games *(LIVE)* | 2–4 | 20 | 8 | common | 0.05 |
| 5 | Sketching *(LIVE)* | 1–2 | 14 | 4 | living | 0 |
| 6 | Storytelling *(LIVE)* | 2–6 | 22 | 9 | common | 0.01 |
| 7 | Choir | 4–10 | 26 | 12 | common | 0 |
| 8 | Drumming | 1–6 | 20 | 8 | common | 0.02 |
| 9 | Dancing | 2–8 | 25 | 11 | common | 0.03 |
| 10 | Chess | 2 | 16 | 5 | game room | 0 |
| 11 | Dice | 2–5 | 18 | 7 | game room | 0.06 |
| 12 | Woodcut Printing | 1–2 | 15 | 4 | press | 0.01 |
| 13 | Knitting | 1 | 13 | 3 | living | 0 |
| 14 | Carving | 1 | 15 | 5 | workshop | 0.01 |
| 15 | Garden Pleasure | 1–3 | 17 | 6 | greenhouse | 0 |
| 16 | Radio Listening | 1–6 | 19 | 7 | listening | 0 |
| 17 | Poetry | 1 | 14 | 5 | living | 0 |
| 18 | Journaling | 1 | 12 | 3 | living | 0 |
| 19 | Cooking for Taste | 2–4 | 20 | 8 | kitchen | 0.01 |
| 20 | Fishing Pleasure | 1–2 | 18 | 6 | water | 0.01 |
| 21 | Birdwatching | 1–2 | 14 | 4 | surface | 0 |
| 22 | Map Drawing | 1 | 13 | 3 | study | 0 |
| 23 | Knot Craft | 1–2 | 14 | 4 | workshop | 0 |
| 24 | Puppetry | 1–4 | 21 | 9 | common | 0 |
| 25 | Storytelling Advanced | 3–8 | 26 | 12 | common | 0.01 |
| 26 | Wooden Toymaking | 1 | 15 | 5 | workshop | 0.01 |
| 27 | Mask Making | 1–2 | 16 | 6 | workshop | 0.01 |
| 28 | Flag Making | 1–3 | 15 | 5 | workshop | 0 |
| 29 | Soap Carving | 1 | 12 | 3 | living | 0 |
| 30 | Glass Beadwork | 1 | 14 | 4 | workshop | 0.01 |
| 31 | Song Writing | 1 | 16 | 6 | music corner | 0 |
| 32 | Drum Circle | 4–12 | 27 | 13 | common | 0.03 |
| 33 | Memorization Games | 2–6 | 18 | 7 | common | 0 |
| 34 | Shadow Puppets | 2–6 | 20 | 9 | common | 0 |
| 35 | Letter Writing | 1 | 13 | 4 | living | 0 |
| 36 | Memorial Craft | 1 | 17 | 5 | workshop | 0 |
| 37 | Star Watching | 1–3 | 15 | 5 | surface | 0 |
| 38 | Rain Listening | 1–2 | 13 | 4 | common | 0 |
| 39 | Old Film Nights | 2–10 | 22 | 10 | common | 0 |
| 40 | Festival Craft | 2–8 | 24 | 11 | workshop | 0.02 |

Each row preserves the live schema and stays within the live relief/morale range.

---

## 18. APPENDIX B — PERFORMANCE TABLE

| Performance | Medium | Practice | Audience | Morale | Prep days | Theme |
|---|---|---|---|---|---|---|
| Shelter Ballad | song | 2 | 4–30 | 6 | 2 | memory |
| First Song | song | 1 | 2–10 | 4 | 1 | hope |
| Choir Anthem | choral | 3 | 6–40 | 8 | 3 | community |
| Harmonica Blues | song | 1 | 2–12 | 5 | 1 | loss |
| Drum Vigil | rhythm | 2 | 4–25 | 6 | 2 | memorial |
| The Long Walk | play | 3 | 8–50 | 9 | 4 | endurance |
| The Bucket Crown | play | 2 | 6–40 | 7 | 3 | resistance |
| Shadow Play | puppet | 2 | 4–30 | 6 | 3 | nature |
| Poetry Night | poetry | 2 | 3–20 | 5 | 1 | loss |
| Story of the Exchange | story | 3 | 6–40 | 8 | 3 | memory |
| The Founders | story | 3 | 6–40 | 7 | 4 | community |
| Censored Song | song | 4 | 10–40 | 10 | 3 | resistance |
| Dance Night | dance | 2 | 6–35 | 8 | 2 | hope |
| The Names Read | oration | 2 | 10–60 | 9 | 2 | memorial |
| Last Words Set | mixed | 4 | 8–40 | 9 | 4 | memorial |
| The Machine Song | song | 3 | 6–30 | 7 | 3 | resistance |
| Children's Hour | mixed | 1 | 2–20 | 5 | 1 | hope |
| The Debt Play | play | 3 | 8–40 | 6 | 4 | community |
| Winter Set | mixed | 3 | 6–35 | 8 | 3 | endurance |
| The Rival Story | story | 4 | 10–50 | 9 | 4 | memory |
| Harvest Song | song | 2 | 4–30 | 6 | 2 | nature |
| The Quiet Hour | music | 2 | 4–25 | 5 | 1 | loss |
| Battle Lament | song | 3 | 6–40 | 7 | 3 | loss |
| The Open Road | story | 2 | 4–30 | 6 | 2 | hope |
| Mask Play | play | 3 | 8–40 | 8 | 4 | resistance |
| The Wall | oration | 3 | 10–60 | 9 | 3 | memorial |
| Ration Blues | song | 1 | 2–15 | 4 | 1 | hope |
| The Fire Play | play | 4 | 8–40 | 8 | 4 | loss |
| New Arrival | mixed | 2 | 4–30 | 6 | 2 | community |
| The Long Night | mixed | 4 | 10–50 | 10 | 4 | endurance |
| Signal Song | song | 3 | 6–30 | 7 | 3 | hope |
| The Archive | story | 3 | 8–40 | 8 | 3 | memory |
| The Broken Tool | play | 2 | 6–30 | 6 | 3 | community |
| Ash Dance | dance | 3 | 6–35 | 8 | 3 | nature |
| The Unnamed | oration | 4 | 10–60 | 11 | 4 | memorial |
| The Debt Called | play | 3 | 8–40 | 6 | 4 | resistance |
| First Frost | mixed | 2 | 4–30 | 6 | 2 | nature |
| The Second Life | story | 4 | 10–50 | 10 | 4 | hope |
| Last Show | mixed | 4 | 10–60 | 12 | 5 | memorial |
| The Long Evening | mixed | 5 | 15–80 | 14 | 5 | community |

Quality scales with practice, preparation, and audience; a rushed or untrained
performance can fail and cost morale. The 40-row table gives the expansion enough
range to carry a campaign's evenings.

---

## 19. APPENDIX C — FESTIVAL TABLE

| Festival | Anchor | Prep | Supplies | Guests | Morale | Diplomatic |
|---|---|---|---|---|---|---|
| Founding Evening | day 1 | 3 | food + fuel | shelter | +12 | low |
| First Thaw | spring | 4 | food + lamp oil | shelter | +10 | low |
| The Long Light | summer | 5 | food + power | 2 shelters | +14 | high |
| Harvest Share | autumn | 4 | food | 2 shelters | +12 | medium |
| The Quiet Week | winter | 2 | minimal | shelter | +6 | low |
| Names Night | memorial | 3 | candles + food | shelter | +9 | low |
| The Common Field | any | 6 | large | 3+ shelters | +16 | very high |
| Trade Fair | any | 5 | goods | traders | +11 | high |
| The Makers' Day | craft | 3 | materials | shelter | +8 | low |
| The Long Walk | endurance | 4 | food + water | shelter | +10 | low |
| New Arrivals | intake | 3 | food | shelter | +9 | medium |
| The Last Evening | year end | 5 | food + fuel | shelter | +13 | medium |

Festival supply costs are real and appear in the shelter's ration and power math.
The Common Field is the expansion's diplomatic centerpiece: a festival that can
prevent a war or start one.

---

## 20. APPENDIX D — SPORTS AND GAMES TABLE

| Sport | Players | Space | Equipment | Injury base | Morale | Friction |
|---|---|---|---|---|---|---|
| Push Ball | 5 | field | ball | 0.05 | 8 | 2 |
| Relay Run | 4 | corridor | baton | 0.03 | 6 | 1 |
| Stone Lift | 1 | yard | stones | 0.08 | 5 | 2 |
| Rope Pull | 6 | yard | rope | 0.04 | 7 | 3 |
| Wall Handball | 2 | yard | ball | 0.05 | 5 | 1 |
| Pipe Climb | 1 | shaft | harness | 0.10 | 6 | 2 |
| Weight Carry | 1 | yard | weights | 0.07 | 5 | 1 |
| Maze Race | 2 | tunnels | none | 0.06 | 6 | 2 |
| Target Throw | 2 | range | stones | 0.04 | 5 | 1 |
| Team Tag | 8 | field | none | 0.02 | 7 | 2 |

| Game | Players | Skill | Luck | Stake | Harm |
|---|---|---|---|---|---|
| Card Pairs | 2–4 | 0.4 | 0.6 | goods | low |
| Dice High | 2–6 | 0.1 | 0.9 | goods | med |
| Chess | 2 | 0.95 | 0.05 | honor | none |
| Knuckle Bones | 2–4 | 0.3 | 0.7 | goods | low |
| The Long Odds | 2–8 | 0.2 | 0.8 | high goods | high |
| Coin Call | 2 | 0.0 | 1.0 | goods | low |
| Token Draw | 2–5 | 0.5 | 0.5 | goods | low |
| Ring Toss | 2–6 | 0.6 | 0.4 | goods | none |
| Memory Grid | 2 | 0.8 | 0.2 | honor | none |
| The Wager Ledger | any | 0.5 | 0.5 | debt | high |

Gambling harm routes to `NeedsSystem` morale and, where a dependency develops, to
the existing dependency/psychological systems. No distinct addiction model is added.

---

## 21. APPENDIX E — ARTWORK MEDIA AND THEMES

`CultureCreationSystem` already defines six media (Painting, Sculpture,
MusicComposition, Poetry, Storytelling, Craftwork) and six themes (Hope, Loss,
Resistance, Nature, Community, Memorial). The expansion adds value/display fields
without changing those enums, plus:

| Field | Meaning |
|---|---|
| `base_value` | authored trade value |
| `commemorative_value` | shelter-internal value when the subject is a named dead |
| `display_room_tag` | where the piece can hang |
| `provenance` | who made it, when, and from what |
| `controversy` | whether public display causes friction |

Controversial art is the expansion's sharpest social lever: a portrait of a
disgraced founder, a mural naming a collaborator, a song about the massacre.

---

## 22. APPENDIX F — ECONOMY AND BALANCE MODEL

- **Evening allocation.** A survivor has ~2 hours. Hobby relief competes with rest
  and extra shifts. A performance or festival claims the whole window.
- **Supply cost.** Festivals consume food, fuel, and power. A rich festival is a
  real sacrifice.
- **Art value.** Art can be sold, but commemorative pieces lose shelter morale if
  sold. The player chooses between cash and meaning.
- **Wagers.** Wagers move existing goods; they never create currency. Debt is a
  real obligation that can be called in.
- **Broadcast.** Cultural broadcasts cost power and reach a region; reception
  affects rumor, standing, and recruitment.
- **Payoff.** Culture does not feed anyone. It keeps people functional, which in
  this game is the difference between a shelter and a morgue.

---

## 23. APPENDIX G — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `recreation.json` | +34 | 5,000 |
| `performances.json` | 40 | 8,000 |
| `festivals.json` | 12 | 3,000 |
| `artworks.json` | 30 | 4,000 |
| `sports.json` | 10 | 2,000 |
| `games_of_chance.json` | 10 | 2,500 |
| `museum_exhibits.json` | 25 | 3,500 |
| `cultural_broadcasts.json` | 20 | 3,000 |
| Quest objectives | 47 quests | 14,000 |
| NPC prose | 8 NPCs | 7,000 |
| Songs/plays | 25 | 6,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 15 | 2,200 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~66,700** |

---

## 24. APPENDIX H — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R17-1 | Culture trivializes morale | Med | High | Costs, finite evening, competition |
| R17-2 | Gambling exploits currency | Med | High | No new currency; debt; harm |
| R17-3 | Second needs/morale | Low | High | Route through owners |
| R17-4 | Tone drift to carnival | Med | Med | Tone table review |
| R17-5 | Content overrun | Med | Med | Budget §23 |
| R17-6 | Save bloat | Med | Med | Cap history |
| R17-7 | Determinism | Low | High | Host-forked RNG |
| R17-8 | UI overload | Med | Med | Shared shell; one panel per system |
| R17-9 | Censorship system abused | Med | Med | Friction and guilt consequences |
| R17-10 | Real-world content leak | Low | Critical | Fictional-only review |

---

## 25. APPENDIX I — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Does the evening window reduce the work day?** Recommended: no; it competes
   with rest and optional extra shifts.
2. **Can a festival be hosted at a non-shelter location?** Recommended: yes, at the
   Common Field, with security and diplomatic risk.
3. **Can gambling debt seize property?** Recommended: yes, through the existing
   inventory transfer, with a clear warning.
4. **Is cultural broadcast one-directional?** Recommended: initially yes; two-way
   contact is a later phase.
5. **Can the archive be burned?** Recommended: yes, as a major irreversible choice.

---

## 27. APPENDIX J — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_eve_first_evening` | 3 | Ora plays; attendance grows; Halden objects |
| `quest_eve_the_wager` | 4 | Tam opens a ledger; first debt; a warning |
| `quest_eve_invitation` | 3 | Rival shelter proposes a joint festival |
| `quest_eve_festival_bill` | 4 | Cost the festival; ration and power argument |
| `quest_eve_the_song` | 5 | Ora writes it; the shelter splits; names named |
| `quest_eve_the_archive` | 4 | Mora asked to suppress or preserve; decide |
| `quest_eve_first_sport` | 4 | Sera organizes a game; an injury; a quarrel |
| `quest_eve_the_gallery` | 4 | Lio's paintings become trade and politics |
| `quest_eve_the_broadcast` | 5 | Restore the pavilion; first regional broadcast |
| `quest_eve_rival_song` | 4 | Rival version of the story; two truths |
| `quest_eve_the_festival` | 6 | Prepare, host, perform, keep the peace |
| `quest_eve_the_fire` | 4 | Festival accident; rescue and blame |
| `quest_eve_the_debt` | 4 | Tam's debt called; consequences |
| `quest_eve_what_we_keep` | 3 | Final disposition; epilogue |

Stage requirements reference valid item IDs, rooms, and locations. No stage may set
morale directly; all morale flows through `NeedsSystem`.

---

## 28. APPENDIX K — SIDE QUEST DETAIL

**Music and performance**
- `quest_eve_tune_instrument` — find strings, resin, or a replacement body; tune by ear.
- `quest_eve_first_song` — write a song with a survivor who has never sung.
- `quest_eve_choir` — recruit voices; resolve who sings which part.
- `quest_eve_play_badly` — perform under-practiced; absorb the reaction.
- `quest_eve_encore` — the audience will not go to bed; decide the cost.
- `quest_eve_silent_night` — a death cancels the evening; decide whether silence is a rite.

**Art**
- `quest_eve_paint_the_wall` — a mural on a wall everyone passes.
- `quest_eve_portrait_of_the_dead` — paint a named survivor; the family must approve.
- `quest_eve_art_forgery` — a sold pre-war piece is a clever copy.
- `quest_eve_gallery_trade` — sell a commemorative piece or keep it.
- `quest_eve_child_art` — children's drawings reveal a fear adults missed.

**Games and wagers**
- `quest_eve_card_rules` — write down the rules before the argument.
- `quest_eve_the_stake` — a wager with real rations.
- `quest_eve_debt_collector` — a debt is called with pressure.
- `quest_eve_dice_night` — a long night and its morning.
- `quest_eve_chess_master` — a rivalry that spans months.

**Sport**
- `quest_eve_field_rules` — agree on a sport everyone will accept.
- `quest_eve_injury` — a player breaks something; medical pipeline handles it.
- `quest_eve_team_rival` — two factions field teams; the match becomes politics.
- `quest_eve_the_match` — stakes, crowd, and a disputed call.

**Festival**
- `quest_eve_festival_prep` — supplies, permissions, program, security.
- `quest_eve_festival_food` — feeding guests on a ration budget.
- `quest_eve_festival_security` — keeping rival factions apart.
- `quest_eve_festival_program` — what is performed in public.
- `quest_eve_festival_after` — the morning after; debts and friendships.

**Archive and broadcast**
- `quest_eve_cassette_listen` — the cassette corpus is played for the first time.
- `quest_eve_broadcast_night` — first cultural transmission; expect interference.
- `quest_eve_archive_fire` — a press-room fire threatens the record of the shelter.

---

## 29. APPENDIX L — NPC DOSSIERS (BRIEF)

**Ora Fenn** — musician. Plays with more honesty than skill and knows it. Writes the
massacre song because she cannot stop hearing it. She is not naive about the
consequences; she simply believes a shelter that forgets is already dead.

**Halden Vey** — administrator. Guards power because power is what keeps the ration
fair. Sees the evening as a leak in the workday and the song as a fuse. Not cruel;
overwhelmed, and therefore dangerous to anything unaccounted for.

**Lio Marr** — painter. Makes art that is useful: portraits for the dead, murals for
the living, pieces that trade. Discovers that a portrait can be worth more as a
commodity than as a memorial, and hates that he understands the trade.

**Tam Ost** — card player. Charming, quick, and in over their head. The wager arc's
face: not a villain, just someone who cannot stop raising.

**Sera Du** — athlete and coach. Believes competition makes a shelter sharper. Gets
her first serious injury and discovers the team she built is more durable than her leg.

**Mora Keel** — archivist. Custodian of what is kept and what is burned. The
suppression decision is hers and she will not make it alone.

**Pim** — child performer. Sings in the choir and hears more of the massacre song
than adults realize. The reason the archive question cannot be purely tactical.

**Dorn Vale** — rival emissary. Carries the other shelter's version of the story
and genuinely believes it. The festival's counterpart and the expansion's mirror.

---

## 30. APPENDIX M — LOCATION DETAIL

- **The Buried Theater** — an auditorium under ash; the stage survived, the seats did not.
- **The Film Depot** — reels in cans, projectors in crates, a screen folded like a map.
- **The Press Works** — a flatbed press, type trays, and paper that crumbles if you rush.
- **The Fallen Music Hall** — a grand piano under a collapsed roof, still in tune.
- **The Old Ground** — a flat field with faded lines; someone's rules painted on a post.
- **The Picture Vault** — climate-stable storage; ownership is already contested.
- **The Tape Shop** — cassettes by the crate; some play, some hiss, some are blank.
- **The Grandstand** — a racecourse ruin; old betting slips still in the drain.
- **The Common Field** — neutral ground; a stage, a well, and no walls.
- **The Signal Pavilion** — a broadcast tower that wants power and patience.

---

## 31. APPENDIX N — LISTENING MODEL (CASSETTES AND VINYL)

The cassette corpus is existing content; the expansion gives it a consumption loop:

| Step | Behavior |
|---|---|
| Acquire | Scavenge a cassette item or set |
| Listen | Play in the listening room as an evening activity |
| Reveal | Each part advances a story set; listening has a sequence |
| Affect | Morale, memory, grief, or rumor depending on the set |
| Broadcast | Rare sets can be broadcast for regional effect |
| Archive | Sets can be preserved in the archive or traded |

Listening is not a minigame. It is a quiet, optional, long-running thread that turns
the 30 KB of existing cassette prose into a real evening experience.

---

## 32. APPENDIX O — MUSEUM EXHIBIT TABLE (25 ROWS)

| # | Exhibit | Source | Morale | Knowledge | Controversy |
|---|---|---|---|---|---|
| 1 | Founding Ration Tin | item | +2 | 0 | no |
| 2 | First Tool | item | +3 | 1 | no |
| 3 | Massacre Names Plaque | memorial | +4 | 2 | high |
| 4 | Pre-War Photograph | collectible | +2 | 1 | no |
| 5 | Bent Spoon | relic | +3 | 0 | med |
| 6 | Cassette Set | cassette | +3 | 1 | med |
| 7 | Child's Drawing | art | +4 | 0 | no |
| 8 | Founder Portrait | art | +3 | 0 | high |
| 9 | Trophy Mount | trophy | +2 | 0 | no |
| 10 | Broken Radio | item | +2 | 1 | no |
| 11 | Dosimeter | item | +1 | 2 | no |
| 12 | Letter Never Sent | document | +3 | 1 | med |
| 13 | First Harvest Grain | agriculture | +3 | 1 | no |
| 14 | Prosthetic Hook | medical | +3 | 1 | med |
| 15 | Map of the Old City | cartography | +2 | 2 | no |
| 16 | Sheet Music | music | +3 | 1 | no |
| 17 | Playbill | press | +2 | 0 | no |
| 18 | Found Photograph Wall | collectible | +4 | 0 | no |
| 19 | Faction Treaty Copy | document | +2 | 2 | high |
| 20 | The Censored Song Sheet | music | +3 | 1 | high |
| 21 | Rival Shelter Banner | faction | +2 | 1 | med |
| 22 | Quarantine Sign | medical | +2 | 1 | med |
| 23 | Machine Fragment | robotics | +2 | 1 | no |
| 24 | Dead Child's Toy | memorial | +4 | 0 | high |
| 25 | The Long Evening Program | festival | +3 | 0 | no |

Exhibits are displayable, and controversy affects friction and faction standing.

---

## 33. APPENDIX P — BROADCAST PROGRAM TABLE (20 ROWS)

| # | Program | Source | Reach | Reception | Risk |
|---|---|---|---|---|---|
| 1 | Evening Music Hour | vinyl | local | morale + | low |
| 2 | The Names Read | live | regional | memorial | low |
| 3 | Ration Report | live | regional | rumor | low |
| 4 | Festival Announcement | live | regional | diplomacy | med |
| 5 | The Massacre Song | live | regional | standing | high |
| 6 | Weather Roundup | live | regional | utility | none |
| 7 | Trade Tunes | vinyl | regional | trade | low |
| 8 | Children's Hour | live | local | morale | none |
| 9 | The Rival Story | live | regional | standing | high |
| 10 | Work Song Set | vinyl | local | morale | none |
| 11 | Memorial Program | live | regional | grief | low |
| 12 | The Long Walk Serial | live | regional | morale | low |
| 13 | Cartography Talk | live | regional | navigation | none |
| 14 | Medical Hour | live | regional | health | none |
| 15 | The Debt Ledger | live | local | gossip | med |
| 16 | Old World Records | vinyl | regional | memory | low |
| 17 | Signal Silence Hour | none | regional | mystery | med |
| 18 | Recruitment Call | live | regional | recruitment | med |
| 19 | The Censored Hour | live | local | friction | high |
| 20 | Last Evening Broadcast | live | regional | memorial | low |

Broadcast reception routes through `FactionStanceEngine` and `RumorSystem`; the
remote radio engine owns the transmission itself.

---

## 34. APPENDIX Q — ROOM DETAIL

- **Common Hall** — the shelter's living room; the evening's default venue.
- **Music Corner** — a stool, a shelf of instruments, and a blanket for the floor.
- **Gallery Wall** — hung pieces with handwritten labels; names and dates.
- **Museum Store** — cold, dry, locked; the pieces too fragile to hang.
- **Small Stage** — a platform of pallets with a curtain on a wire.
- **Game Room** — a table with worn edges and a ledger nobody trusts.
- **Press Room** — ink, type, and the smell of a wet broadsheet.
- **Listening Room** — benches facing a turntable and a cassette deck.

Room requirements are additive tags; the existing room system owns construction and
power. Culture rooms compete with production rooms for floor space.

---

## 35. APPENDIX R — WORKED 90-DAY CULTURE SCENARIO

**Days 1–10.** Ora plays in the mess hall. Attendance is 4, then 9, then 15. Halden
notes the lamp cost. Tam starts a card ledger; a survivor loses a week's tobacco.

**Days 11–25.** The rival shelter's invitation arrives. The player costs the
festival: 40 food, 12 fuel, 300 W for two days. The argument is real. Sera organizes
Push Ball; the first injury is a sprained wrist, handled by the clinic.

**Days 26–45.** Ora writes the song. Two survivors demand she not perform it. Mora
is asked to file it under restricted. The player can suppress, edit, or schedule it.
The gallery gains its first sold painting and its first refused sale.

**Days 46–60.** The Signal Pavilion is wired. The first broadcast reaches two
shelters; one answers with its own story. Rumor spreads faster than truth.

**Days 61–75.** The Common Field festival. Security holds for two days and breaks
on the third: a fight, a fire, and a saved child. Diplomacy depends on the program.

**Days 76–90.** The debt is called. The archive fire threatens the casette
drawers. The player chooses what to keep: the song, the gallery, the ledger, or the
silence. The chronicle records the choice.

This arc is the expansion's intended shape; every branch must be survivable.

---

## 36. APPENDIX S — PERFORMANCE QUALITY MODEL

Quality is a deterministic function of fixed inputs:

```
quality = clamp(
    practice_level * 18
  + preparation_days * 6
  + room_quality * 10
  + audience_fit * 8
  - environmental_penalty
  - fatigue_penalty,
  0, 100)
```

- `practice_level` comes from `HobbySystem` (0–5).
- `preparation_days` is authored per performance.
- `room_quality` comes from the room's tags and condition.
- `audience_fit` compares theme to the audience's mood and recent grief.
- Environmental penalties: blackout, cold, noise, quarantine.
- Fatigue: a performer who worked a double shift performs worse.

Outcomes: `masterwork` (rare), `strong`, `solid`, `flat`, `failed`. A failed
performance costs a small amount of morale, never health.

---

## 37. APPENDIX T — RUMOR AND INFORMATION FLOW

Culture is the rumor economy's richest source. Performance, festival, and broadcast
events emit typed rumor seeds through the live `RumorSystem`:

| Source | Rumor seed | Decay |
|---|---|---|
| A new song | its subject and its author | slow |
| A festival | who came, who did not, who fought | medium |
| A broadcast | what was said on air | fast |
| A gambling debt | who owes whom | medium |
| A suppression | what was forbidden | very slow |
| A sold painting | who bought the dead | slow |

Suppression always leaks. That is the expansion's quiet rule: a censored song
becomes the most requested song in the shelter.

---

## 38. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] No real-world song, play, sport, holiday, or artwork is used.
- [ ] All festivals and games are fictional and setting-specific.
- [ ] No real religious observance is depicted (that is Expansion 13's domain).
- [ ] Morale flows only through `NeedsSystem` / `MoraleContagionSystem`.
- [ ] No new currency; wagers move existing goods.
- [ ] Gambling harm routes through existing dependency/psychological systems.
- [ ] Sport injuries route through the medical pipeline.
- [ ] Performances and festivals have real costs.
- [ ] Suppression has friction and guilt consequences.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses the host-forked RNG only.

---

## 39. APPENDIX V — GLOSSARY

- **Evening window** — the derived daily leisure block before the sleep shift.
- **Performance** — a scheduled public work with performer, audience, and result.
- **Festival** — a recurring scheduled gathering with supply cost and diplomacy.
- **Mastery** — a practice level in a hobby or performance skill.
- **Provenance** — the story of who made a piece, when, and for whom.
- **Commemorative value** — worth to the shelter when the subject is a named dead.
- **Broadcast** — a cultural transmission over the existing radio path.
- **The Common Field** — neutral festival ground.

---

## 40. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `NeedsSystem` | — | morale | (owner) |
| `MoraleContagionSystem` | morale fact | contagion | — |
| `SurvivorDowntimeSystem` | schedule | sessions | morale |
| `HobbySystem` | practice | mastery | — |
| `VinylMoraleSystem` | records | play state | morale directly |
| `CultureCreationSystem` | media/theme | artwork | economy |
| `RumorSystem` | seeds | rumors | — |
| `RadioTuner` | frequency | transmission | — |
| `FactionStanceEngine` | reception | standing | — |
| `Inventory` | items | transfers | — |
| `PowerGridSystem` | draw | room power | — |
| `MedicalPipelineCoordinator` | injury | treatment | — |
| `MemorialSystem` | subjects | commemoratives | — |
| `EpilogueChronicleBuilder` | milestones | chronicle | — |

---

## 42. APPENDIX X — EVENING SCHEDULE MODEL

The evening window is derived, not hardcoded. The host computes it from the live
campaign clock and duty roster:

| Window | Duration | Default use |
|---|---|---|
| Post-shift | 30 min | wash, ration, settle |
| Main evening | 90 min | hobby, performance, game, listening |
| Late evening | 30 min | private reflection, journaling, sleep prep |

Activities claim blocks. A performance claims the main block. A festival claims the
whole evening plus preparation days. Extra shifts claim the main block and cost
morale. The model never extends the day; it only allocates the hours that exist.

### 42.1 Competition rules

- A survivor cannot attend two activities in the same block.
- A room can host one public activity per block.
- Power draw for culture competes with every other load on the grid.
- Grief suppresses attendance; a shelter in mourning will skip the evening.

---

## 43. APPENDIX Y — ART PROVENANCE TABLE (20 EXAMPLE PIECES)

| # | Piece | Medium | Theme | Maker | Display | Controversy |
|---|---|---|---|---|---|---|
| 1 | The First Wall | mural | Hope | Lio Marr | common hall | no |
| 2 | Names of the Lost | painting | Memorial | Lio Marr | gallery | med |
| 3 | The Founder | portrait | Community | Lio Marr | gallery | high |
| 4 | Ash Garden | painting | Nature | unnamed | gallery | no |
| 5 | The Bucket Crown | sculpt | Resistance | Tam Ost | stage | med |
| 6 | Ration Tin Study | sketch | Loss | Lio Marr | museum | no |
| 7 | The Long Walk | poem | Endurance | Ora Fenn | press | no |
| 8 | Machine Hymn | music | Resistance | Ora Fenn | broadcast | high |
| 9 | Child's Shelter | drawing | Hope | Pim | gallery | no |
| 10 | The Debt | sculpt | Loss | Tam Ost | game room | med |
| 11 | First Harvest | painting | Nature | unnamed | gallery | no |
| 12 | The Censored Verse | poem | Resistance | anonymous | hidden | high |
| 13 | The Quiet Hour | music | Loss | Ora Fenn | listening | no |
| 14 | Fog on the Glass | painting | Nature | Lio Marr | museum | no |
| 15 | The Prosthetic Hand | sculpt | Community | Lio Marr | clinic | med |
| 16 | The Rival Banner | craft | Resistance | unnamed | museum | high |
| 17 | Little Ration Extra | craft | Hope | Pim | common | no |
| 18 | The Last Show | mixed | Memorial | Ora Fenn | stage | med |
| 19 | Wall of Small Hands | mural | Community | children | school | no |
| 20 | The Second Ledger | poem | Memorial | anonymous | archive | high |

Provenance is recorded on creation. Anonymous pieces are still attributed to a
survivor internally; only the public label omits the name. That distinction powers
the suppression and forgery quests.

---

## 44. APPENDIX Z — FESTIVAL PROGRAM TEMPLATE

A festival program is authored in slots; the host fills them from scheduled
performances:

| Slot | Duration | Content | Requirement |
|---|---|---|---|
| Opening | 15 min | welcome, rules | speaker |
| Music | 30 min | song or choir | performer + practice |
| Food | 30 min | shared meal | supplies |
| Competition | 45 min | sport or game | teams + space |
| Story | 30 min | play or serial | performers |
| Memorial | 15 min | names, silence | memorial data |
| Closing | 15 min | announcements, rumor | speaker |

A missing slot is a visible gap; guests remember what was not there. The program is
the festival's diplomatic content, and the player edits it under pressure.

---

## 45. APPENDIX AA — CULTURE AND THE CHRONICLE

`EpilogueChronicleBuilder` already assembles the campaign's ending record. Culture
events supply it with authored milestones:

| Milestone | Trigger | Chronicled as |
|---|---|---|
| First evening | first performance | "The shelter began to sing." |
| Festival hosted | first successful festival | "The Common Field was broken in." |
| Song suppressed | archive restriction | "A song was filed away." |
| Song performed | public performance | "A song was sung anyway." |
| Gallery opened | first exhibit | "The wall learned to remember." |
| Archive burned | fire or choice | "The record stopped." |
| Last broadcast | final transmission | "The signal ended on a name." |

These milestones give a culture-focused campaign a distinct epilogue without a new
ending authority. They append to the existing chronicle rows.

---

## 46. APPENDIX AB — DATA SCHEMA DETAIL (NEW CATALOGS)

**`performances.json`** — as shown in §8.2, plus optional fields:
`difficulty_band`, `failure_consequence`, `requires_light`, `crowd_cap`,
`diplomatic_note` (for broadcast/festival pieces).

**`festivals.json`** — fields: `festival_id`, `display_name`, `season_anchor`,
`day_anchor`, `preparation_days`, `supply_cost[]`, `guest_capacity`,
`program_slots[]`, `morale_effect`, `cohesion_effect`, `rumor_profile`,
`disaster_pool[]`, `diplomatic_weight`, `tags`.

**`artworks.json`** — fields: `template_id`, `medium`, `theme`, `quality_band`,
`base_value`, `commemorative_value`, `display_room_tags[]`, `provenance_rule`,
`controversy`, `tags`.

**`sports.json`** — fields: `sport_id`, `display_name`, `players_per_side`,
`space_tag`, `equipment[]`, `duration_hours`, `injury_base_permille`,
`stakes_allowed`, `morale_effect`, `friction_effect`, `tags`.

**`games_of_chance.json`** — fields: `game_id`, `display_name`, `players_min`,
`players_max`, `equipment[]`, `skill_weight_permille`, `luck_weight_permille`,
`harm_profile`, `debt_rules`, `tags`.

**`museum_exhibits.json`** — fields: `exhibit_id`, `source_item_id`,
`display_room_id`, `morale_effect`, `knowledge_effect`, `controversy`,
`provenance_text`, `tags`.

**`cultural_broadcasts.json`** — fields: `program_id`, `source_kind`
(`vinyl`|`live`|`cassette`), `frequency_band`, `reach_band`, `reception_effects[]`,
`content_flags[]`, `tags`.

All new catalogs carry `schema_version: 1` and are validated for ID presence,
duplicate IDs, valid room/location references, bounded numeric fields, and valid
item references. Invalid rows fail the integrity gate rather than loading silently.

---

## 47. APPENDIX AC — OPENING VIGNETTE (TONE SAMPLE)

> Ora plays after the shift because the mess hall is empty and the acoustics are
> good and nobody asked her to. By the third night there are nine people. By the
> ninth night there are twenty, and Halden is standing at the back with his arms
> crossed, counting the lamps.
>
> The song is not a good song. It is a true one. She plays it once, quietly, and the
> hall does not applaud, because half of them know the names and the other half are
> learning them for the first time, and the only sound is the ventilation and the
> hum of the lights she is costing the grid.
>
> Tam deals the cards in the corner, and wins, and does not look happy about it.

This sets the register: no stage lights, no triumph, and a real cost to every
evening. All Long Evening prose should be written at this temperature.

---

## 48. APPENDIX AD — BALANCE CONSTANTS (PROPOSED)

| Constant | Value | Rationale |
|---|---|---|
| Max evening morale per survivor per day | +3 | prevents leisure from replacing the needs loop |
| Max festival morale | +16 | large but bounded |
| Max broadcast reach | 3 shelters | regional, not global |
| Max wager stake fraction | 25% of goods | prevents economic collapse |
| Max gambling harm per day | 1 event | prevents death spirals |
| Performance prep cap | 5 days | prevents infinite stacking |
| Culture power share cap | 15% of grid | keeps life support dominant |
| Archive fire loss cap | 40% of archive | allows recovery |
| Masterwork rate | 2% at max practice | rare but reachable |

These constants are authored data where possible and code constants where they must
be enforced. None may be exempted by a quest or an item.

---

## 49. CLOSING STATEMENT

ASHFALL already gives a shelter the machinery of survival: food, power, water,
medicine, defense, and the small mercies of a hobby. What it lacks is the evening —
the hours when people stop surviving and start being a community. The Long Evening
adds performance, festival, sport, art, games, and broadcast culture on top of the
existing downtime and artwork systems. It adds no second morale owner, no new
currency, and no fantasy of plenty. It adds songs, a stage, a gallery wall, a card
table, and a question every shelter eventually has to answer: what do we do with the
hours we are not dying?