# ASHFALL — Expansion 30 Design Bible
# THE PRESS
### Wave 4 · Paper, Printing, Broadsheets, Almanacs, Public Notices, and the Written Record

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-21
**Domain owners touched:** `Ashfall.Core.Narrative` (PaperPrintingCatalog, ArchiveDeskSystem), `src/UI` (UndergroundPrintingPressPanel), `Ashfall.Core.Radio` (broadcast authority — untouched)
**Proposed host owner:** `PressHostSession` (extends the archive desk and printing surfaces)
**Existing save sections:** `archive_desk` (and adjacent narrative stores)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has the pieces of a print culture without the machine.
`PaperPrintingCatalog` (9.3 KB) is the live narrative catalog for paper and
printing knowledge. `ArchiveDeskSystem` and `ArchiveInkCatalogLoader` own
records, ink, and scribing. `CulturalArchiveVaultSystem` and
`PrewarArchiveCatalog` hold recovered documents. `src/UI/UndergroundPrintingPressPanel.cs`
already exists for the clandestine leaflet context, and `Main.ExpandedShelterSystems.cs`
wires printing surfaces. The journal and chronicle pipeline
(`JournalCatalogData`, `JournalSelfTest`, `EpilogueChronicleBuilder`) already
turns events into written memory. `library_manuals.json` (21.9 KB) and
`lost_tech_manuals.json` (23.5 KB) are the document supply. The radio
(`RadioProgramProductionSystem`) is a broadcasting authority, not a printing one.

What does not exist: paper making, a printing press as a shelter room, type and
composition, publications as items, broadsheets and almanacs as content, public
notices as an information system, and a bound archive of what the shelter
printed.

**The Press** turns paper, type, and ink into the shelter's public voice: the
broadsheet, the almanac, the manual, the poster, the notice — and the decision
about what gets printed and what stays rumor.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

Before the Exchange, a town's memory lived in print. After it, memory lives in
whoever is still alive to remember.

**The Press** is the expansion about writing things down and handing them out:
rags into paper, type into pages, a press into broadsheets, and a shelter into a
public that reads. It extends the live archive and printing catalogs with a real
paper mill, a press room, authored publications, a notice system, and a bound
archive that survives the people who wrote it.

The expansion's hard rules follow the live owners: `ArchiveDeskSystem` keeps
records and ink, the radio keeps broadcasting, the census broadcast stays with
the verdict system, the journal keeps chronicling, and the expansion adds only
the print medium and its objects. No second archive, radio, or information
authority is created.

### 1.2 The five loops it adds

```
   Rags ──► Pulp ──► Paper ──► Press ──► Publication
     │                            │
     ▼                            ▼
   Water, lime,               Broadsheet, almanac,
   pressing                   pamphlet, poster
                                  │
                    ┌─────────────┤
                    ▼             ▼
                 Notices       Archive ──► Bound volumes
                    │             │
                    ▼             ▼
                 Rumor vs.     Manuals ──► School + Work
                 print         (The Lesson)
```

### 1.3 What the player manages

1. **Paper.** Rags, pulp, screens, pressing, drying, and quality.
2. **Type.** Making, sorting, composing, and locking type; type is metal.
3. **Press.** Setup, impression, drying, and spoilage.
4. **Publications.** Broadsheets, almanacs, primers, pamphlets, posters, forms.
5. **Notices.** Public information, announcements, and rumor correction.
6. **Archive.** Binding, cataloging, storing, and lending printed matter.
7. **Trade.** Selling print as a regional service.

### 1.4 What it is not

- Not a second archive. `ArchiveDeskSystem` and the archive vault remain owners.
- Not radio, not the census broadcast, not a second journal.
- Not a propaganda-buff system. Information effects are authored, modest, and
  never a mind-control mechanic.
- Not a newspaper simulation with fabricated only-headlines; every publication
  has a purpose and a reader.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Narrative/PaperPrintingCatalog.cs` | Paper and print lore | `LIVE` |
| `Assets/Ashfall.Core/Narrative/ArchiveDeskSystem.cs` | Records and ink | `LIVE` |
| `Assets/Ashfall.Core/Narrative/ArchiveInkCatalogLoader.cs` | Ink catalog | `LIVE` |
| `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs` | Recovered documents | `LIVE` |
| `Assets/Ashfall.Core/Radio/RadioProgramProductionSystem.cs` | Broadcast authority | `LIVE` |
| `src/UI/UndergroundPrintingPressPanel.cs` | Clandestine leaflet context | `LIVE` |
| `src/Main.ExpandedShelterSystems.cs` | Printing surface wiring | `LIVE` |
| `Assets/Ashfall.Core/Journal/*`, `src/Journal/*` | Chronicle pipeline | `LIVE` |
| `Assets/Ashfall.Core/Endgame/EpilogueChronicleBuilder.cs` | Final chronicle | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `PaperPrintingCatalog.cs` | 9.3 KB | narrative catalog, no gameplay |
| `library_manuals.json` | 21.9 KB | document supply |
| `lost_tech_manuals.json` | 23.5 KB | recoverable documents |
| `education_session_records.json` | 32 KB | lesson records |
| `currents_pamphlets.json` | narrative | leaflets |
| Paper/ink/type/publication data | none | confirmed absent |

### 2.3 Confirmed gaps

- **GAP-30-1 — No paper making.** No rag-to-paper chain in gameplay.
- **GAP-30-2 — No press room or press machine.** The panel exists; the works do not.
- **GAP-30-3 — No type.** No type making, sorting, or composition.
- **GAP-30-4 — No publications.** No broadsheets, almanacs, primers, or posters as
  authored items.
- **GAP-30-5 — No notice system.** Public information is not modeled; rumor is not
  contrasted with print.
- **GAP-30-6 — No bound archive.** Documents exist; binding, catalogs, and lending
  do not.
- **GAP-30-7 — No print-spoilage and quality model.**
- **GAP-30-8 — No print trade.** The region has no print market.

### 2.4 Non-duplication statement

This expansion will **not** add a second archive, journal, radio, broadcast, or
espionage system. It extends `ArchiveDeskSystem` with press records, extends the
existing printing panel rather than replacing it, keeps the underground leaflet
path as the clandestine channel, routes all public-information effects through
authored content rather than a new morale multiplier, and adds state only as an
additive sub-object of the existing archive/desk store. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Print is memory that does not sleep.** A name written down outlives
the person who remembers it.

**Pillar 2 — Paper is made, not found.** Rags and water and screens; scarcity
that makes every sheet count.

**Pillar 3 — Type is reused, and that is the point.** A single set of type can
print a thousand sheets.

**Pillar 4 — Print competes with rumor.** The shelter decides what official
information is, and the region decides whether to believe it.

**Pillar 5 — What gets printed is a choice.** A shelter can print food notices
or fundraisers for its own legend.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| First broadsheet | Ink on fingers, one page | Triumph montage |
| Paper making | Vats, screens, drying | Craft-show |
| Almanac | Practical, funny, human | Walls of lore |
| Notice | Plain, signed, dated | Propaganda poster |
| Archive | Dust, order, care | Museum reverence |
| Rumor vs. print | Argument between people | Mind control |

### 3.3 Content limits

- No real-world newspapers, mastheads, logos, or slogans.
- No propaganda mechanics that reward manipulation of the shelter.
- No libel, harassment, or public shaming as a reward.
- No censorship content that humiliates; the debate is authored and respectful.
- Children's involvement in print is light and educational.

---

## 4. THE PRESS WORLD

### 4.1 Interior rooms

- **`room_paper_mill`** — vats, screens, press, and drying.
- **`room_press_room`** — the press, the stone, and the ink.
- **`room_type_room`** — type cases, composing sticks, and caches.
- **`room_bindery`** — boards, thread, glue, and finished books.
- **`room_notice_board`** — where official paper goes.
- **`room_reading_room`** — the archive's public face.
- **`room_editor_desk`** — choices, drafts, and the almanac.
- **`room_drying_loft`** — paper and pages drying overhead.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_rag_pick` | The Rag Pick | 3 | Rag and fiber supply |
| `loc_paper_ruins` | The Paper Mill | 5 | Screens, vats, salvage |
| `loc_type_foundry` | The Type Foundry | 5 | Type metal and molds |
| `loc_print_shop` | The Print Shop | 4 | Press salvage and parts |
| `loc_news_kiosk` | The Kiosk | 3 | Trade and distribution |
| `loc_pamphlet_alley` | The Alley | 4 | Clandestine leaflets |
| `loc_archive_depot` | The Depot | 4 | Bulk paper storage |
| `loc_old_newsroom` | The Newsroom | 5 | Desks, files, and a morgue |
| `loc_ink_works` | The Ink Works | 4 | Soot, oil, and pigment |
| `loc_library_depot` | The Library Depot | 4 | Books and binding |

All locations require valid item references and scanner registration.

### 4.3 The print day

Pulp in the morning, print in the afternoon, dry overnight, bind at week's end.
The press's rhythm is slower than the radio's and faster than the archive's, and
that middle tempo is its identity.

---

## 5. MAIN STORYLINE — "WHAT WE PRINTED"

### 5.1 Central conflict

The shelter has a chronicle kept in one hand, a notice board with three nails,
and a region running on rumor. When a false rumor about the shelter's water
causes a trade partner to stop delivery, **Livia**, the shelter's record-keeper,
proposes a broadsheet: one page, dated, signed, and handed out at the market.
**Oskar Vane**, who printed a trade sheet before the Exchange, has a broken press
and no paper. **Hester** can make paper if the shelter can spare rags and water.
**Jun** can set type if someone can make it.

The shelter discovers that print is not nostalgia. It is infrastructure for
trust: a dated page from a named shelter is worth more than a shouted promise,
and the region will pay for it.

The expansion's question: **what does a shelter put its name on, and what does
it leave to rumor?**

### 5.2 Theme (unspoken)

**A printed page is a promise with a date on it.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_printer_oskar_vane` | Oskar Vane | Printer | Press, type, and impressions |
| `npc_papermaker_hester` | Hester | Paper maker | Rags, pulp, and sheets |
| `npc_editor_livia` | Livia | Editor | Publications and notices |
| `npc_typesetter_jun` | Jun | Typesetter | Composition and proofing |
| `npc_almanac_ferro` | Ferro | Almanac writer | Weather, seasons, and advice |
| `npc_clerk_bram` | Bram | Notice clerk | Official notices and records |
| `npc_child_pip` | Pip | Runner | Distribution and sales |
| `npc_trader_marla` | Marla | Print trader | Paper, ink, and print sales |

### 5.4 Story beats (15)

1. **The Rumor.** False information costs the shelter a trade route.
2. **The Broken Press.** Oskar's press is surveyed and assessed.
3. **The Rags.** Hester starts a paper run.
4. **The First Sheet.** The first usable paper.
5. **The Type.** Type is cast or salvaged; composition begins.
6. **The First Broadsheet.** One page, dated, signed.
7. **The Market.** The broadsheet is handed out and read.
8. **The Almanac.** Ferro proposes a seasonal almanac.
9. **The Manual.** The press prints a manual for the school.
10. **The Notice.** Official notices become a system.
11. **The Dispute.** What gets printed and what does not.
12. **The Rumor War.** Someone prints against the shelter.
13. **The Archive.** Bound volumes and a catalog.
14. **The Trade.** Print becomes a regional service.
15. **What We Printed.** Final disposition of the press.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Press scale | hand / screw / water | capacity vs. cost |
| Paper use | notices / broadsheets / manuals | priority |
| Distribution | free / sold / traded | access vs. revenue |
| Notice policy | open / signed / sealed | trust vs. control |
| Almanac | practical / folk / none | science vs. comfort |
| Rumor | ignore / answer / investigate | credibility |
| Archive | public / restricted / bound only | memory vs. access |
| Final | press as institution / trade / relic | identity |

### 5.6 Endings (5 + fade)

1. **The Printed Record** — the shelter's memory is bound and readable; the
   region trusts its pages.
2. **The Weekly** — a regular broadsheet becomes the region's news.
3. **The Manual Press** — print serves the school and the trades.
4. **The Quiet Board** — the press prints notices only; the rest stays rumor.
5. **The Burned Bindery** — paper and records are lost to fire or storm.
6. **Fade** — the press sits covered; the chronicle stays handwritten.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_press_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_press_rumor`, `quest_press_broken_press`, `quest_press_rags`,
`quest_press_first_sheet`, `quest_press_type`, `quest_press_first_broadsheet`,
`quest_press_market`, `quest_press_almanac`, `quest_press_manual`,
`quest_press_notice`, `quest_press_dispute`, `quest_press_rumor_war`,
`quest_press_archive`, `quest_press_trade`, `quest_press_what_we_printed`.

### 6.2 Side quests (30)

**Paper (5)**
- `quest_press_rag_run` — collect rags and fiber
- `quest_press_pulp` — beat and prepare pulp
- `quest_press_screen` — make a mould and screen
- `quest_press_dry` — press and dry sheets
- `quest_press_quality` — sort paper by quality

**Press (5)**
- `quest_press_repair` — repair the press
- `quest_press_stone` — find a press stone
- `quest_press_ink` — make printing ink
- `quest_press_type_cast` — cast type
- `quest_press_lock` — lock and proof a form

**Publications (5)**
- `quest_press_broadsheet_run` — a regular broadsheet
- `quest_press_almanac_write` — write the almanac
- `quest_press_primer_run` — print a school primer
- `quest_press_poster` — posters for a public need
- `quest_press_form` — print ration and trade forms

**Information (5)**
- `quest_press_notice_system` — establish official notices
- `quest_press_rumor_check` — verify a rumor
- `quest_press_rebuttal` — answer a false claim
- `quest_press_prices` — publish market prices
- `quest_press_weather` — publish weather warnings

**Archive (5)**
- `quest_press_binding` — bind a volume
- `quest_press_catalog` — catalog the archive
- `quest_press_lending` — start a lending shelf
- `quest_press_preserve` — protect paper from damp
- `quest_press_index` — index by subject

### 6.3 Repeatable quests (8)

`quest_press_repeat_print`, `quest_press_repeat_paper`,
`quest_press_repeat_notice`, `quest_press_repeat_almanac`,
`quest_press_repeat_bind`, `quest_press_repeat_trade`,
`quest_press_repeat_rag`, `quest_press_repeat_proof`.

### 6.4 Dynamic hooks

Live events (radio broadcasts, market changes, weather, verdict announcements,
school terms, disputes) attach authored print follow-ups through existing seams.
No new event bus.

### 6.5 Constraints

- Radio broadcasting remains with the radio authority.
- Census and verdict announcements remain with their owner.
- The clandestine leaflet panel remains the clandestine channel.
- Print effects are authored content, not unseen multipliers.
- Every publication consumes paper, ink, and press time.
- No publication may duplicate a live system's broadcast responsibility.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `PaperMillSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** rags, pulp, forming, pressing, drying, and paper grades.
**Consumes:** `Inventory`, water (via `SanitationSystem`/`FluidLogisticsSystem`),
`CraftingSystem`, dried fiber from the Thread expansion where present.
**Data:** `paper_recipes.json`. **Rules:** paper is made in batches; quality is
deterministic from fiber, water, and skill; every sheet is counted.

### 7.2 `PressSystem` (new, `Ashfall.Core.Narrative`)

**Owns:** type, composition, impressions, proofing, and press wear. **Consumes:**
`PaperMillSystem`, `ArchiveDeskSystem` (ink), `CraftingSystem`,
`SkillProgressionSystem`. **Data:** `press_jobs.json`, `type_catalog.json`,
`ink_recipes.json`. **Rules:** type is reused; forms are locked and proofed;
mistakes waste paper; press wear is real.

### 7.3 `PublicationSystem` (new, `Ashfall.Core.Narrative`)

**Owns:** authored publications as items with distribution metadata. **Consumes:**
`PressSystem`, `Inventory`, `TradingSystem`. **Data:** `publications.json`,
`almanacs.json`. **Rules:** a publication is a real item with a real count; it can
be read, given, sold, or archived; no infinite copies.

### 7.4 `NoticeSystem` (new, thin, `Ashfall.Core.Narrative`)

**Owns:** official notices, dates, signatures, and public-information effects.
**Consumes:** `PublicationSystem`, `NeedsSystem` morale (authored, modest),
`VerdictCensusBroadcast` (announcement ownership preserved).
**Data:** `bulletins.json`. **Rules:** notices are dated and signed; their effect
is authored per notice; no generic propaganda multiplier.

### 7.5 `PressArchiveSystem` (extend `ArchiveDeskSystem`)

**Owns:** binding, cataloging, lending, and storage of printed matter.
**Consumes:** `ArchiveDeskSystem`, `LibraryStudySystem` (manuals), `Bindery` room.
**Data:** `press_archive.json`. **Rules:** bound volumes persist in save; lending
carries return risk; damp and fire are real threats.

### 7.6 Systems explicitly not added

- No second archive, journal, or radio system.
- No propaganda mind-control or morale multiplier.
- No infinite-copy publication.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `paper_recipes.json` (new)

```json
{
  "schema_version": 1,
  "recipes": [
    {
      "recipe_id": "paper_rag_common",
      "display_name": "Common Rag Paper",
      "rags": 12,
      "water": 20,
      "hours": 8,
      "sheets": 24,
      "grade": "common",
      "tags": ["notice", "pamphlet"]
    }
  ]
}
```

### 8.2 `press_jobs.json` (new)

Job rows: form type, paper, ink, impressions, hours, spoilage, and output.

### 8.3 `type_catalog.json` (new)

Type rows: size, metal, cast hours, useful life, and font role.

### 8.4 `ink_recipes.json` (new)

Ink rows: soot, oil, gum, hours, color, and use.

### 8.5 `publications.json` (new)

Publication rows: type, pages, reader, purpose, distribution, and effects.

### 8.6 `almanacs.json` (new)

Almanac rows: season, contents, advice entries, and morale/practical effects.

### 8.7 `bulletins.json` (new)

Notice rows: kind, signer, duration, effect, and truthfulness.

### 8.8 `press_archive.json` (new)

Archive rows: volume, contents, bind quality, condition, and access.

### 8.9 Items

New items appended to `items.json`: `item_rag_bundle`, `item_pulp_bucket`,
`item_paper_mould`, `item_press_frame`, `item_press_stone`,
`item_composing_stick`, `item_type_case`, `item_printing_ink`,
`item_broadsheet`, `item_almanac_book`, `item_pamphlet`, `item_poster`,
`item_blank_form`, `item_bound_volume`, `item_binding_thread`,
`item_book_board`, `item_reading_ledger`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

The archive/desk store remains the live save owner. New sub-objects (paper
batches, press jobs, type inventory, publications, notices, archive volumes) are
additive inside it. No new save section.

### 9.2 State to persist

- Paper batches and stock.
- Press jobs and wear.
- Type inventory and condition.
- Publications printed and held.
- Notices posted and their durations.
- Bound volumes and lending records.

### 9.3 Determinism

- Paper yield and press spoilage are deterministic given inputs and skill.
- No wall-clock or unseeded randomness; daily ticks use the live path.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with existing archive state untouched; no paper, press, type,
publication, notice, or bound-volume state exists until started.

### 9.5 Checksum

Invariant-culture floats; integer counts for impressions and sheets.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `PaperMillPanel` (new) | Rags, pulp, sheets | `PressHostSession` |
| `PressPanel` (new) | Type, form, print | same |
| `PublicationPanel` (new) | What was printed | same |
| `NoticePanel` (new) | Official notices | same |
| `AlmanacPanel` (new) | Seasonal publication | same |
| `ArchivePanel` (extend) | Volumes, catalogs, lending | existing |
| `UndergroundPrintingPressPanel` (untouched) | Clandestine channel | existing |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Paper and ink costs are shown before printing.
- Notices state what they will and will not do.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Text-dense content uses readable scale and contrast.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a press pulling, paper tearing, type
settling, a bindery needle, a stamp on a notice. No cue is required; text carries
meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `ArchiveDeskSystem` | Records, ink, press archive |
| `PaperPrintingCatalog` | Print knowledge |
| `CulturalArchiveVaultSystem` | Recovered documents |
| `LibraryStudySystem` | Printed manuals |
| `Journal` / chronicle pipeline | Printed milestones |
| `RadioProgramProductionSystem` | Broadcast stays separate |
| `VerdictCensusBroadcast` | Announcement ownership preserved |
| `TradingSystem` | Print and paper trade |
| `NeedsSystem` | Authored notice effects |
| `SchoolingSystem` (Wave 4) | Primers and manuals |
| `Inventory` | Paper, ink, publications |
| `SanitationSystem` / water | Paper making |
| `EpilogueChronicleBuilder` | What the shelter printed |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `PaperPrintingCatalog`, `ArchiveDeskSystem`,
`ArchiveInkCatalogLoader`, `UndergroundPrintingPressPanel`, journal pipeline, and
data sizes. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author paper recipes, press jobs, type, ink,
publications, almanacs, bulletins, press archive; append items. Register
validators and scanner.

**Phase 2 — Pure Core.** `PaperMillSystem`, `PressSystem`, `PublicationSystem`,
`NoticeSystem`, `PressArchiveSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `PressHostSession`, selftest coverage, fresh journey.

**Phase 5 — UI.** New and extended panels with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak including paper scarcity, press wear, rumor
competition, and archive growth.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Paper recipes | 12 |
| Press jobs | 20 |
| Type sets | 8 |
| Ink recipes | 8 |
| Publications | 40 |
| Almanac entries | 30 |
| Bulletins | 25 |
| Archive rows | 15 |
| Items | 17 |
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
| Second archive/radio | Critical | Live owners untouched |
| Propaganda multiplier | Critical | Authored effects only |
| Infinite copies | High | Real paper and ink |
| Print trivializes radio | High | Medium separation, distinct roles |
| Censorship tone | High | Respectful authored debate |
| Paper too easy | Medium | Rags, water, time |
| Determinism break | Low | Live tick paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `paper_recipes.json` | 12 | 3,000 |
| `press_jobs.json` | 20 | 4,000 |
| `type_catalog.json` | 8 | 1,500 |
| `ink_recipes.json` | 8 | 2,000 |
| `publications.json` | 40 | 10,000 |
| `almanacs.json` | 30 | 6,000 |
| `bulletins.json` | 25 | 5,000 |
| `press_archive.json` | 15 | 3,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 17 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~65,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R30-1 | Second archive | Low | Critical | Extend live desk |
| R30-2 | Propaganda multiplier | Med | Critical | Authored effects |
| R30-3 | Infinite copies | Med | High | Paper and ink |
| R30-4 | Radio overlap | Med | High | Distinct mediums |
| R30-5 | Censorship tone | Med | High | Respectful debate |
| R30-6 | Paper trivial | Med | Med | Rag and water costs |
| R30-7 | Archive damp/fire | Low | Med | Authored risk, recovery |
| R30-8 | Determinism | Low | High | Live tick paths |
| R30-9 | Content overrun | Med | Med | Budget §13 |
| R30-10 | Notice as mind control | Low | High | Explicit exclusion |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Does print ever change behavior mechanically?** Recommended: only through
   authored notice effects and trade/market information, never a global multiplier.
2. **Can publications be read for skill?** Recommended: primers and manuals feed
   `LibraryStudySystem`; broadsheets are information only.
3. **Who owns the notice board?** Recommended: `NoticeSystem`, with its records in
   the archive store.
4. **Does the press compete with the radio?** Recommended: they complement; radio
   is fast and wide, print is durable and portable.
5. **Can a hostile settlement print against the shelter?** Recommended: yes, as
   authored rumor-war content with no real-world analogues.

---

## 17. APPENDIX D — PAPER RECIPE TABLE (12 RECIPES)

| # | Recipe | Rags | Water | Hours | Sheets | Grade |
|---|---|---|---|---|---|---|
| 1 | Common Rag | 12 | 20 | 8 | 24 | common |
| 2 | Fine Rag | 16 | 24 | 10 | 20 | fine |
| 3 | Linen Sheet | 14 | 22 | 9 | 22 | fine |
| 4 | Straw Paper | 8 | 26 | 8 | 18 | coarse |
| 5 | Wood Pulp | 6 | 30 | 10 | 30 | coarse |
| 6 | Recycled Sheet | 10 | 18 | 6 | 20 | mixed |
| 7 | Blotting | 12 | 20 | 7 | 26 | common |
| 8 | Wrapping | 14 | 18 | 6 | 28 | coarse |
| 9 | Writing Laid | 16 | 22 | 12 | 18 | fine |
| 10 | Board Stock | 20 | 26 | 14 | 12 | rigid |
| 11 | Special White | 18 | 26 | 12 | 16 | fine |
| 12 | Emergency Sheet | 8 | 16 | 5 | 14 | poor |

Paper quality decides what can be printed: a common sheet takes a notice; a fine
sheet takes an almanac or a manual that must last.

---

## 18. APPENDIX E — PRESS JOB TABLE (20 JOBS)

| # | Job | Paper | Ink | Impressions | Hours | Spoilage |
|---|---|---|---|---|---|---|
| 1 | Single Notice | 1 | 1 | 40 | 2 | 5% |
| 2 | Broadsheet | 2 | 2 | 80 | 4 | 8% |
| 3 | Pamphlet | 4 | 2 | 120 | 6 | 8% |
| 4 | Poster | 1 | 3 | 30 | 3 | 10% |
| 5 | Blank Form | 1 | 1 | 60 | 3 | 4% |
| 6 | Price List | 1 | 1 | 50 | 2 | 4% |
| 7 | Weather Sheet | 1 | 1 | 40 | 2 | 5% |
| 8 | Primer Page | 1 | 2 | 100 | 4 | 8% |
| 9 | Manual Page | 2 | 2 | 80 | 5 | 8% |
| 10 | Manual Section | 6 | 3 | 60 | 9 | 10% |
| 11 | Almanac Page | 2 | 3 | 90 | 5 | 8% |
| 12 | Map Sheet | 2 | 2 | 40 | 6 | 12% |
| 13 | Ration Form | 1 | 1 | 70 | 3 | 4% |
| 14 | Trade Contract | 2 | 1 | 40 | 4 | 6% |
| 15 | Pass Sheet | 1 | 1 | 50 | 2 | 4% |
| 16 | Certificate | 1 | 1 | 30 | 3 | 8% |
| 17 | School Text | 4 | 2 | 100 | 7 | 8% |
| 18 | Song Sheet | 1 | 2 | 60 | 3 | 6% |
| 19 | Recipe Card | 1 | 1 | 80 | 3 | 5% |
| 20 | Charity Appeal | 2 | 2 | 70 | 4 | 8% |

Every job consumes real paper and ink and produces a finite number of copies.
The press is not a photocopier; it is a workshop with a quota.

---

## 19. APPENDIX F — TYPE TABLE (8 SETS)

| # | Type Set | Metal | Cast Hours | Life | Role |
|---|---|---|---|---|---|
| 1 | Body Small | lead alloy | 12 | 2000 | notices |
| 2 | Body Standard | lead alloy | 14 | 2500 | broadsheets |
| 3 | Headline | lead alloy | 16 | 1500 | titles |
| 4 | Display | lead alloy | 18 | 1000 | posters |
| 5 | Numerals | lead alloy | 10 | 2500 | tables |
| 6 | Fine Text | tin alloy | 20 | 1800 | almanacs |
| 7 | Map Type | lead alloy | 12 | 1200 | maps |
| 8 | Ornament | lead alloy | 8 | 800 | decoration |

Type is the press's capital: expensive to make once and reusable for years. The
Metal Foundry and the Kiln both feed it, which is why the press arrives late in a
settlement's growth and then does not stop.

---

## 20. APPENDIX G — INK TABLE (8 RECIPES)

| # | Ink | Soot | Oil | Gum | Hours | Use |
|---|---|---|---|---|---|---|
| 1 | Black Common | 4 | 2 | 1 | 3 | notices |
| 2 | Black Fine | 6 | 3 | 2 | 4 | manuals |
| 3 | Brown | 4 | 2 | 1 | 3 | wrapping |
| 4 | Red | 2 | 2 | 2 | 5 | headings |
| 5 | Blue | 2 | 2 | 2 | 5 | maps |
| 6 | Stamp Ink | 3 | 2 | 1 | 3 | records |
| 7 | Weatherproof | 5 | 4 | 2 | 6 | posters |
| 8 | Washable | 3 | 2 | 1 | 3 | practice |

Ink is where the archive's existing ink recipes meet the press's demand. The
expansion does not invent a new ink path; it consumes the one that exists.

---

## 21. APPENDIX H — PUBLICATION TABLE (40 PUBLICATIONS)

| # | Publication | Pages | Reader | Purpose | Distribution |
|---|---|---|---|---|---|
| 1 | Shelter Broadsheet | 1 | all | news and notices | free |
| 2 | Market Sheet | 1 | traders | prices | sold |
| 3 | Weather Warning | 1 | all | safety | posted |
| 4 | Ration Notice | 1 | all | policy | posted |
| 5 | Trade Contract | 2 | partners | agreement | signed |
| 6 | Pass Sheet | 1 | travelers | permission | issued |
| 7 | Almanac | 12 | all | season planning | sold |
| 8 | Farm Almanac | 16 | growers | planting | sold |
| 9 | School Primer | 20 | children | literacy | issued |
| 10 | Number Book | 16 | children | arithmetic | issued |
| 11 | Pump Manual | 24 | engineers | maintenance | archived |
| 12 | Water Manual | 20 | engineers | testing | archived |
| 13 | Clinic Guide | 18 | medics | treatment | archived |
| 14 | First Aid Card | 1 | all | emergency | issued |
| 15 | Preserving Guide | 16 | kitchen | food | archived |
| 16 | Recipe Book | 30 | cooks | menu | sold |
| 17 | Seed Catalogue | 12 | growers | varieties | sold |
| 18 | Building Notes | 20 | builders | methods | archived |
| 19 | Masonry Guide | 18 | masons | kiln work | archived |
| 20 | Textile Patterns | 16 | weavers | production | archived |
| 21 | Boot Patterns | 10 | cobblers | footwear | archived |
| 22 | Radio Log Sheet | 1 | operators | logging | issued |
| 23 | Rail Timetable | 2 | crews | schedules | posted |
| 24 | Map Sheet | 1 | all | navigation | sold |
| 25 | Zone Map | 4 | scavengers | safety | sold |
| 26 | Song Sheet | 2 | all | morale | free |
| 27 | Story Leaflet | 1 | all | morale | free |
| 28 | Memorial List | 1 | all | remembrance | posted |
| 29 | School Notice | 1 | parents | schedule | posted |
| 30 | Work Roster | 1 | workers | shifts | posted |
| 31 | Duty Ledger | 4 | clerks | records | archived |
| 32 | Ledger Form | 1 | clerks | accounting | issued |
| 33 | Certificate | 1 | workers | certification | issued |
| 34 | Charter Copy | 4 | all | rights | archived |
| 35 | Treaty Copy | 4 | partners | agreement | archived |
| 36 | Charity Appeal | 1 | region | aid | posted |
| 37 | Trade Catalogue | 8 | partners | goods | sold |
| 38 | Wanted Notice | 1 | region | safety | posted |
| 39 | Memory Book | 40 | all | chronicle | archived |
| 40 | The First Year | 80 | archive | history | archived |

Publication is where the press becomes a shelter institution: not one newspaper,
but a printer's list that spans safety, food, trade, education, and memory.

---

## 22. APPENDIX I — ALMANAC ENTRY TABLE (30 ENTRIES)

| # | Entry | Season | Kind | Effect |
|---|---|---|---|---|
| 1 | First Frost | autumn | warning | prepare |
| 2 | Plant Window | spring | advice | planting |
| 3 | Storm Season | summer | warning | shelter |
| 4 | Ice Roads | winter | advice | routes |
| 5 | Long Light | summer | note | morale |
| 6 | Short Dark | winter | note | morale |
| 7 | Seed Time | spring | advice | farming |
| 8 | Harvest Moon | autumn | note | festival |
| 9 | Fog Weeks | autumn | warning | travel |
| 10 | Dry Winds | summer | warning | fire |
| 11 | Mud Season | spring | note | hauling |
| 12 | Cold Snaps | winter | warning | layers |
| 13 | Bird Return | spring | sign | planting |
| 14 | Insect Hatch | summer | warning | crops |
| 15 | Mushroom Time | autumn | advice | foraging |
| 16 | River High | spring | warning | flooding |
| 17 | River Low | summer | note | water |
| 18 | First Snow | winter | note | preparation |
| 19 | Thaw Start | spring | sign | travel |
| 20 | Heat Peak | summer | warning | work hours |
| 21 | Grain Ripening | summer | advice | harvest |
| 22 | Preserve Push | autumn | advice | storage |
| 23 | Ice Harvest | winter | advice | cold storage |
| 24 | Lambing | spring | advice | herds |
| 25 | Shearing | summer | advice | wool |
| 26 | Culling | autumn | advice | herds |
| 27 | Storage Check | winter | advice | audit |
| 28 | Repair Weeks | spring | advice | maintenance |
| 29 | Ration Review | winter | advice | policy |
| 30 | Year's End | winter | note | reflection |

The almanac is practical before it is poetic: it tells the shelter what the year
is about to do. Its morale value comes from the fact that it is usually right.

---

## 23. APPENDIX J — BULLETIN TABLE (25 BULLETINS)

| # | Bulletin | Signer | Duration | Effect |
|---|---|---|---|---|
| 1 | Ration Change | steward | week | clarity |
| 2 | Water Notice | engineer | day | safety |
| 3 | Trade Prices | clerk | week | market |
| 4 | Lost Item | clerk | week | recovery |
| 5 | Work Roster | steward | day | efficiency |
| 6 | Drill Call | watch lead | day | readiness |
| 7 | Clinic Hours | medic | week | care |
| 8 | School Term | teacher | month | education |
| 9 | Weather Watch | observer | day | safety |
| 10 | Zone Warning | watch lead | week | safety |
| 11 | Census Note | clerk | month | trust |
| 12 | Memorial Notice | clerk | year | grief |
| 13 | Feast Day | steward | day | morale |
| 14 | Travel Advisory | watch lead | week | routes |
| 15 | Quarantine Note | medic | week | health |
| 16 | Fuel Saving | steward | week | conservation |
| 17 | Seed Issue | grower | season | planting |
| 18 | Tool Return | quartermaster | week | inventory |
| 19 | Library Hours | librarian | month | access |
| 20 | Fire Rules | watch lead | permanent | safety |
| 21 | Hygiene Rules | medic | permanent | health |
| 22 | Trade Offer | clerk | week | trade |
| 23 | Aid Request | steward | month | aid |
| 24 | Correction | editor | day | trust |
| 25 | Rumor Answer | editor | day | truth |

A notice is a small, dated, signed act of trust. The list is deliberately
prosaic: the press proves itself on ration changes and water notices before it
ever prints a history.

---

## 24. APPENDIX K — ARCHIVE TABLE (15 VOLUMES)

| # | Volume | Contents | Bind | Condition | Access |
|---|---|---|---|---|---|
| 1 | Chronicle Year One | journal entries | fine | good | open |
| 2 | Water Records | tests and repairs | common | fair | open |
| 3 | Ration Ledgers | policy history | common | good | open |
| 4 | Medical Records | cases and outcomes | fine | fair | restricted |
| 5 | Trade Book | deals and prices | common | good | open |
| 6 | School Book | pupils and lessons | common | good | open |
| 7 | Manual Set | technical manuals | fine | good | open |
| 8 | Almanac Set | yearly almanacs | common | fair | open |
| 9 | Map Book | survey sheets | fine | good | open |
| 10 | Memorial List | the dead | fine | good | open |
| 11 | Treaty Copies | agreements | fine | good | restricted |
| 12 | Charter Copy | founding rules | fine | good | open |
| 13 | Recipe Book | kitchen record | common | fair | open |
| 14 | Pattern Book | textile patterns | common | fair | open |
| 15 | The First Year | bound history | fine | new | open |

Binding is how a stack of pages becomes a volume, and a volume is how a shelter
keeps a promise to people who are not born yet.

---

## 25. APPENDIX L — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_press_rumor` | 4 | Track the rumor; find the cost |
| `quest_press_broken_press` | 4 | Survey and assess the press |
| `quest_press_rags` | 3 | Collect rags and test fiber |
| `quest_press_first_sheet` | 5 | Pulp, form, press, dry |
| `quest_press_type` | 5 | Cast or salvage type |
| `quest_press_first_broadsheet` | 5 | Compose, proof, print |
| `quest_press_market` | 4 | Hand out and observe |
| `quest_press_almanac` | 5 | Write and print |
| `quest_press_manual` | 4 | Print for the school |
| `quest_press_notice` | 4 | Establish the notice board |
| `quest_press_dispute` | 4 | Decide what is printed |
| `quest_press_rumor_war` | 5 | Answer a hostile page |
| `quest_press_archive` | 5 | Bind and catalog |
| `quest_press_trade` | 4 | Sell print as a service |
| `quest_press_what_we_printed` | 3 | Final disposition |

---

## 26. APPENDIX M — NPC DOSSIERS (BRIEF)

**Oskar Vane** — printer. Sets type the way other people breathe and treats a
clean pull as a small happiness. Wants the press running before he dies.

**Hester** — paper maker. Knows that paper is mostly patience. Sorts rags by
feel and can tell a good sheet by the sound it makes when it is lifted.

**Livia** — editor. Decides what the shelter says out loud. Believes every
printed claim should be signed and dated, including her own mistakes.

**Jun** — typesetter. Composes in the head, proofs aloud, and finds typography
comforting in a way that embarrasses him slightly.

**Ferro** — almanac writer. Tracks weather and seasons on a wall and writes the
entries in plain language because plain language is what people act on.

**Bram** — notice clerk. Posts, dates, and files everything. Keeps a copy of
every notice because copies are the whole point.

**Pip** — runner. Carries the broadsheet to the market and comes back with news
and exact change, which the adults find both useful and a little embarrassing.

**Marla** — print trader. Buys paper and sells pages; prices by paper quality
and by how badly the buyer needs people to read something.

---

## 27. APPENDIX N — LOCATION DETAIL

- **The Rag Pick** — rag supply from traders and salvage; sorting is the skill.
- **The Paper Mill** — vats, screens, press frames, and a floor that holds water.
- **The Type Foundry** — type metal, molds, and trays; hot and exacting work.
- **The Print Shop** — press, stone, ink, and drying lines.
- **The Kiosk** — distribution, sales, and the region's read news.
- **The Alley** — clandestine leaflets; distinct from the open press.
- **The Depot** — bulk paper storage where damp is the enemy.
- **The Newsroom** — desks, files, and a morgue full of old editions.
- **The Ink Works** — soot, oil, and gum; a smell that stays in clothes.
- **The Library Depot** — books, binding, and lending ledgers.

---

## 28. APPENDIX O — PAPER MODEL

| Fiber | Sheet quality | Yield | Best use |
|---|---|---|---|
| Linen rag | fine | low | manuals, almanacs |
| Cotton rag | fine | low | records |
| Mixed rag | common | medium | notices |
| Straw | coarse | high | wrapping |
| Wood pulp | coarse | high | bulk |
| Recycled | mixed | medium | practice, forms |
| Special fiber | fine | very low | certificates |

Paper yield is deterministic from fiber, water, and skill. The shelter can always
make paper; it cannot always make good paper, and the difference is visible in
the print.

---

## 29. APPENDIX P — PRESS WEAR MODEL

| Impression count | Condition | Effect | Maintenance |
|---|---|---|---|
| 0–500 | new | full quality | none |
| 500–1500 | good | full | oil |
| 1500–3000 | worn | +spoilage | adjust |
| 3000–5000 | tired | +10% spoilage | replace parts |
| 5000+ | failing | +25% spoilage | rebuild |

Press wear is the expansion's argument against infinite print. The press is a
machine that eats itself slowly, and the shelter that maintains it keeps its
voice.

---

## 30. APPENDIX Q — RUMOR VERSUS PRINT MODEL

| Situation | Rumor | Print |
|---|---|---|
| Water quality | distorted | dated test |
| Trade prices | inflated | posted list |
| Shelter policy | misquoted | signed notice |
| Danger zones | vague | mapped |
| Deaths | exaggerated | listed |
| Treaties | denied | copied |
| Aid requests | ignored | posted |
| Hostile claims | unchallenged | answered |

The press's real function is not persuasion but verification. A shelter that
prints dates and signatures becomes the reference the region checks rumors
against, and that reputation is worth more than any single page.

---

## 31. APPENDIX R — WORKED 360-DAY PRESS SCENARIO

**Days 1–30.** Rumor costs a route; press surveyed; rags collected; first pulp
made; first usable sheets dried.

**Days 31–60.** Type salvaged and cast; first form locked; first broadsheet
printed; market reaction is curious and skeptical.

**Days 61–90.** Regular broadsheet begins; weather warnings posted; trade prices
published; rumor damage begins to reverse.

**Days 91–150.** Almanac written and printed; school primer printed; manual
section printed; press wear is measured and maintained.

**Days 151–220.** Winter printing slows; the archive binds its first three
volumes; notice board becomes the shelter's official voice.

**Days 221–300.** A hostile sheet appears; the shelter prints a dated answer; the
region compares and decides.

**Days 301–360.** Print trade begins; the archive catalogs; the first bound
history is finished and shelved.

---

## 32. APPENDIX S — VIGNETTE (TONE SAMPLE)

> Oskar inks the form and pulls the bar and the press answers with a sound like a
> door closing, and the first page comes up backwards and perfect, and he stands
> there for a moment before he turns it over.

> Hester lifts a sheet by the corner and taps it with a finger, and it rings,
> and she nods because a sheet that rings will take ink and last.

> At the notice board, Bram posts the ration change and dates it and signs it,
> and a farmhand reads it twice and says that is better than being told, because
> now he can check.

---

## 33. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Rag shortage | no paper | salvage, trade, reuse |
| Poor pulp | weak sheets | re-beat, re-sort |
| Press break | no print | repair, parts |
| Type loss | no text | recast, sort |
| Ink shortage | no print | soot, trade |
| Spoiled run | waste | proof better, adjust |
| Damp archive | text loss | dry, rebind, copy |
| Fire in archive | records lost | copies, offsite |
| Rumor wins | credibility | dates, corrections |
| Hostile sheet | dispute | answer, verify |

No failure is a game over. The deepest failure is a shelter with a press, paper,
and nothing it is willing to sign.

---

## 34. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] `ArchiveDeskSystem` remains the records and ink owner.
- [ ] Radio broadcasting remains with the radio authority.
- [ ] Verdict and census announcements remain with their owner.
- [ ] The clandestine leaflet panel is untouched.
- [ ] Notices have authored effects only; no global multiplier.
- [ ] Every publication consumes real paper and ink.
- [ ] No real-world newspaper or slogan is copied.
- [ ] Censorship content is debated respectfully.
- [ ] Archive loss is tragic but recoverable.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live tick paths only.

---

## 35. APPENDIX V — GLOSSARY

- **Pulp** — beaten fiber in water ready for forming.
- **Mould** — frame and screen that forms a sheet.
- **Couching** — transferring a wet sheet to felt.
- **Form** — locked type ready to print.
- **Pull** — one press impression.
- **Proof** — a test impression.
- **Broadsheet** — a single large page.
- **Almanac** — a seasonal reference.
- **Bulletin** — a dated official notice.
- **Signature** — a folded sheet ready for binding.

---

## 36. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `PaperMillSystem` | rags, water | sheets | archive |
| `PressSystem` | paper, ink, type | publications | radio |
| `PublicationSystem` | jobs | publication items | notices |
| `NoticeSystem` | bulletins | posted notices | broadcast |
| `PressArchiveSystem` | volumes | catalogs, lending | records |
| `ArchiveDeskSystem` | ink, paper | records | press |
| `LibraryStudySystem` | manuals | study | print |
| `RadioProgramProductionSystem` | scripts | broadcast | print |
| `VerdictCensusBroadcast` | verdicts | announcements | press |
| `Journal` | events | chronicle | print |
| `TradingSystem` | print goods | trade | press |
| `NeedsSystem` | notices | authored effects | print |
| `EpilogueChronicleBuilder` | milestones | chronicle | — |

---

## 37. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`paper_recipes.json`** — `recipe_id`, `display_name`, `rags`, `water`,
`hours`, `sheets`, `grade`, `tags`.

**`press_jobs.json`** — `job_id`, `display_name`, `paper`, `ink`,
`impressions`, `hours`, `spoilage`, `output`, `tags`.

**`type_catalog.json`** — `set_id`, `display_name`, `metal`, `cast_hours`,
`life`, `role`, `tags`.

**`ink_recipes.json`** — `ink_id`, `display_name`, `soot`, `oil`, `gum`,
`hours`, `use`, `tags`.

**`publications.json`** — `publication_id`, `display_name`, `pages`, `reader`,
`purpose`, `distribution`, `effects[]`, `tags`.

**`almanacs.json`** — `entry_id`, `display_name`, `season`, `kind`,
`effect`, `text`, `tags`.

**`bulletins.json`** — `bulletin_id`, `display_name`, `signer`, `duration`,
`effect`, `text`, `tags`.

**`press_archive.json`** — `volume_id`, `display_name`, `contents[]`, `bind`,
`condition`, `access`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid references, or out-of-range numbers.

---

## 38. APPENDIX Y — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Sheets produced | paper supply | PaperMillSystem |
| Sheet quality | print capability | PaperMillSystem |
| Impressions | press output | PressSystem |
| Spoilage rate | press health | PressSystem |
| Publications held | information reach | PublicationSystem |
| Notices posted | public voice | NoticeSystem |
| Rumor corrections | credibility | NoticeSystem |
| Volumes bound | memory | PressArchiveSystem |
| Lending returns | archive health | PressArchiveSystem |
| Print trade value | regional role | TradingSystem |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score.

---

## 39. APPENDIX Z — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Radio and broadcast authorities remain untouched.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §34.
- [ ] Phase 7 soak shows scrap, regenerate, and sustain.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel archive, press, radio, or notice authority exists.

---

## 40. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. Does the press print maps through the live cartography system, or only sheet
   copies of existing maps?
2. Can a shelter print false notices, and what does the finding cost?
3. Should archive lending ever lose a volume, and how is it recovered?
4. Does paper quality degrade if stored in damp rooms?
5. Should the press be buildable outside the shelter for trade reasons?
6. Should the almanac's advice be mechanically true or only flavor?
7. Who signs a notice when the signer is dead?
8. Should hostile print be authored only, never player-generated propaganda?

None of these may be decided unilaterally; each changes balance and tone.

---

## 41. APPENDIX AB — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Printed birth and cohort records |
| 1 | 13 The Faithful | Printed hymns, calendars, and teachings |
| 1 | 14 Above the Ash | Flight logs and air notices |
| 1 | 15 The Deep Root | Seed catalogues and field almanacs |
| 1 | 16 The Rebuilt Body | Printed fit and maintenance guides |
| 2 | 17 The Long Evening | Song sheets and story leaflets |
| 2 | 18 The Underneath | Cave maps and depth records |
| 2 | 19 The Bitter Air | Hazard notices and safety sheets |
| 2 | 20 The Quiet Hand | Clandestine leaflets; forgotten cipher sheets |
| 2 | 21 The Grid | Load maps and wiring diagrams |
| 3 | 22 The Clean Flow | Water test records and hygiene notices |
| 3 | 23 The Alarm | Drill notices and fire rules |
| 3 | 24 The Long Goodbye | Memorial lists and last words |
| 3 | 25 The Iron Road | Timetables and freight bills |
| 3 | 26 The Common Table | Recipe books and ration forms |
| 4 | 27 The Thread | Pattern books and size sheets |
| 4 | 28 The Lesson | Primers, manuals, and certificates |
| 4 | 29 The Glass | Charts, optical diagrams, maps |
| 4 | 31 The Kiln | Kiln orders, brick counts, plans |

Each hook is additive. The Press can ship alone, and every other expansion can
ship without it.

---

## 42. APPENDIX AC — ENDING PROSE SKETCHES

**The Printed Record.** The archive is bound, dated, cataloged, and open, and
whenever the region wants to know what happened it asks the shelter and the
shelter can answer with a page.

**The Weekly.** A broadsheet goes out every week, sometimes thin, sometimes late,
and never missed, and the region plans around it.

**The Manual Press.** The press prints primers, manuals, and patterns, and the
school and the workshops stop waiting for the archive to be found again.

**The Quiet Board.** Only notices are printed, and the rest remains rumor, and
the shelter's voice is small and precise and trusted within its walls.

**The Burned Bindery.** Fire takes paper and years, and the shelter starts again
with copies remembered by heart, which is the loss the expansion was warning
about.

**Fade.** The press sits covered under a sheet, and the chronicle stays
handwritten, and the shelter manages the way it always has.

---

## 43. APPENDIX AD — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Press as photocopier | removes scarcity | paper and ink per run |
| Propaganda buffs | manipulation reward | authored effects only |
| Newspaper of nothing | fake content | real purpose per page |
| Archive as free storage | no risk | damp, fire, lending |
| Radio and print identical | wasted systems | fast versus durable |
| Censorship as fun | tone break | respectful debate |
| Paper without cost | trivial | rags, water, time |
| Type as decoration | underused | reusable capital |
| Almanac as lore dump | unread | practical entries |
| Print replacing memory | wrong fiction | print supplements people |

The list exists because print is easy to make either magical or meaningless. The
live owners keep it bounded: paper is made, ink is made, type is capital, and
people still have to read.

---

## 44. APPENDIX AE — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Paper recipes | 12 | 3,000 |
| Press jobs | 20 | 4,000 |
| Type sets | 8 | 1,500 |
| Ink recipes | 8 | 2,000 |
| Publications | 40 | 10,000 |
| Almanac entries | 30 | 6,000 |
| Bulletins | 25 | 5,000 |
| Archive rows | 15 | 3,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 17 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~65,500** |

---

## 46. APPENDIX AF — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_press_rag_run` | 3 | Collect, sort, weigh |
| `quest_press_pulp` | 4 | Soak, beat, test |
| `quest_press_screen` | 3 | Build, tension, test |
| `quest_press_dry` | 3 | Press, hang, finish |
| `quest_press_quality` | 3 | Sort, grade, store |
| `quest_press_repair` | 4 | Assess, replace, oil |
| `quest_press_stone` | 3 | Locate, move, seat |
| `quest_press_ink` | 4 | Gather, mix, test |
| `quest_press_type_cast` | 5 | Mold, pour, sort |
| `quest_press_lock` | 4 | Compose, lock, proof |
| `quest_press_broadsheet_run` | 4 | Write, set, print, distribute |
| `quest_press_almanac_write` | 5 | Research, draft, edit, print |
| `quest_press_primer_run` | 4 | Design, set, print, bind |
| `quest_press_poster` | 3 | Choose, set, post |
| `quest_press_form` | 3 | Draft, set, print |
| `quest_press_notice_system` | 4 | Rules, board, clerk |
| `quest_press_rumor_check` | 4 | Listen, verify, document |
| `quest_press_rebuttal` | 4 | Draft, prove, publish |
| `quest_press_prices` | 3 | Survey, list, print |
| `quest_press_weather` | 3 | Observe, write, post |
| `quest_press_binding` | 4 | Fold, sew, cover |
| `quest_press_catalog` | 3 | List, label, shelve |
| `quest_press_lending` | 4 | Rules, ledger, retrieve |
| `quest_press_preserve` | 3 | Dry, cover, rotate |
| `quest_press_index` | 3 | Read, tag, record |

---

## 47. APPENDIX AG — DISTRIBUTION MODEL

| Method | Reach | Cost | Risk |
|---|---|---|---|
| Notice board | shelter | none | ignored |
| Hand-out | market | paper | loss |
| Sale | region | time | none |
| Trade goods | partners | paper | copied |
| Mail via road | far | courier | lost |
| Rail bundle | farthest | freight | delayed |
| School issue | children | paper | torn |
| Archive copy | future | storage | damp |

Distribution is where print meets the road and the rail. A shelter can print all
it likes, but a page only changes a decision when someone far away reads it.

---

## 48. APPENDIX AH — PRINT TRADE MODEL

| Product | Buyer | Price band | Demand |
|---|---|---|---|
| Broadsheet | region | low | high |
| Almanac | farmers | medium | seasonal |
| Manual | trades | high | steady |
| Primer | families | low | steady |
| Map sheet | travelers | medium | steady |
| Form | partners | low | steady |
| Certificate | workers | medium | rising |
| Poster | patrons | low | occasional |
| Blank paper | printers | medium | high |
| Bound volume | archives | high | rare |

Print trade gives the shelter a product that is cheap to move, hard to counterfeit
without a press, and useful to everyone it meets.

---

## 49. APPENDIX AI — INK AND PIGMENT EXTENSION TABLE

| Pigment | Source | Color | Lightfast | Cost |
|---|---|---|---|---|
| Soot | stoves | black | good | none |
| Iron gall | oak, iron | black-blue | excellent | low |
| Red ochre | earth | red | good | low |
| Yellow ochre | earth | yellow | good | low |
| Umber | earth | brown | good | low |
| Woad | plant | blue | fair | med |
| Madder | roots | red | fair | med |
| Verdigris | copper | green | poor | med |
| Lamp black | oil lamp | deep black | good | low |
| Chalk white | lime | white | good | none |

Pigments extend the ink recipes without touching the archive's ink ownership:
the press consumes ink; it does not redefine it.

---

## 50. APPENDIX AJ — SCHOOL AND PRESS INTERFACE TABLE

| Print job | School use | Pages | Frequency |
|---|---|---|---|
| Primer | first readers | 20 | yearly |
| Number book | arithmetic | 16 | yearly |
| Alphabet card | young band | 1 | termly |
| Lesson sheet | teacher | 1 | weekly |
| Certificate | graduates | 1 | termly |
| Notice | parents | 1 | termly |
| Manual page | older band | 2 | monthly |
| Reading list | all | 1 | termly |

The Press and The Lesson are natural partners: the school consumes print, and the
press gains a reason to exist beyond notices. Neither expansion requires the
other, but together they turn literacy into a production chain.

---

## 51. APPENDIX AK — REGIONAL PRINT MAP

| Settlement | Strength | Need | Trade |
|---|---|---|---|
| The shelter | press, archive | paper | sells print |
| Market Town | distribution | press | moves pages |
| Fog Ridge Camp | no paper | notices | buys broadsheets |
| Spring Village | rags | manuals | sells fiber |
| Foundry Enclave | type metal | paper | trades type |
| Deep Bunker | documents | printing | lends records |
| River Flotilla | transport | maps | sells maps |
| Coal Stage | fuel | forms | buys paper |

Print travels the same roads as grain and glass; the shelter that prints early
becomes the region's reference shelf.

---

## 52. APPENDIX AL — LORE: THE PRINT TRADITION

The fiction:

- **The Paper Mill** was a valley mill that made paper from flax and rag; its
  screens and vats survived, and its water rights still matter.
- **The Print Shop** produced a trade sheet for the settlements before the
  Exchange; its press stone was found under a collapsed wall.
- **The Newsroom** was the town's paper office; its files are half readable and
  entirely useful.
- **The Kiosk** was a distribution point; the custom of the weekly sheet comes
  from it.
- **The Library Depot** bound and stored records; the shelter's bindery follows
  its layout.

No real newspaper, masthead, or publisher is copied. The tradition is generic and
local.

---

## 53. APPENDIX AM — WORKED RUMOR SCENARIO

**Day 1.** A trader repeats a claim that the shelter's water is bad. Two deliveries
are held.

**Day 3.** The claim reaches the market. The shelter tests its water and
prints the result on one page: date, method, numbers, signature.

**Day 5.** The broadsheet is handed out at the market. Half the traders shrug;
one reads it carefully and resumes delivery.

**Day 9.** A second claim arrives, this time about rationing. The shelter posts
the actual ration notice and the ledger summary.

**Day 14.** The trader who spread the first claim is asked by other traders to
produce a page of his own. He cannot.

**Day 21.** The shelter's pages are now the reference the market checks against.
The first rumor war ends without a single printed insult, which is the point.

---

## 54. APPENDIX AN — SENSITIVE TOPICS TABLE

| Topic | Risk | Handling |
|---|---|---|
| Censorship | Repression | Debated, never rewarded |
| Libel | Harm | Prohibited in shelter policy |
| Confiscation | Abuse | Hearing and record |
| Literacy access | Inequality | Open board and school tie |
| Cost of paper | Exclusion | Free notices, sold goods |
| Rumor wars | Escalation | Verification, not insult |
| Archive restrictions | Secrecy | Medical and treaty only |
| Child distribution | Exploitation | Light errands, supervision |
| Death notices | Grief | Family-first policy |
| Correction culture | Pride | Corrections celebrated, not punished |

The press touches speech directly, and the expansion's contract is that print is
used for verification, record, and care — never for humiliating people or
manufacturing consent.

---

## 55. APPENDIX AO — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Are costs honest? | lifecycle + a11y tests |
| Tone | Is speech handled with care? | content review |
| Balance | Is paper scarce but usable? | 360-day soak |

---

## 56. APPENDIX AP — PRESS ROOM LAYOUT NOTES

| Station | Function | Adjacency | Constraint |
|---|---|---|---|
| Pulp vat | beat fiber | water | wet floor |
| Mould table | form sheets | vat | flat surface |
| Press | squeeze water | table | heavy |
| Drying loft | dry sheets | heat | airflow |
| Type cases | store type | press | light |
| Composing bench | set form | cases | level |
| Press bed | print | ink, paper | strong floor |
| Drying lines | dry pages | press | space |
| Bindery | fold and sew | archive | dry |
| Notice board | display | door | public |

The layout matters because the expansion's craft is physical: wet stations next
to water, dry stations near heat, the press on the strongest floor, and the
public board where people actually walk.

---

## 57. APPENDIX AQ — PUBLICATION EDITORIAL RULES TABLE

| Rule | Applies to | Enforcement |
|---|---|---|
| Date every page | all | editor review |
| Sign every claim | notices, broadsheets | clerk review |
| Attribute quotes | all | editor review |
| Correct errors publicly | all | next edition |
| No unnamed accusations | all | editor veto |
| Family first for deaths | memorials | clerk policy |
| Medical facts from clinic | health | medic sign-off |
| Prices verified | market sheets | clerk review |
| Maps from survey | maps | surveyor sign-off |
| Manuals from masters | manuals | master sign-off |

Editorial rules are the shelter's own standard, authored as content and enforced
by the people who sign the pages. The expansion never autogenerates a lie and
never rewards one.

---

## 58. APPENDIX AR — PRESS MILESTONE TABLE

| Milestone | Condition | Effect |
|---|---|---|
| First sheet | paper run | paper available |
| First type | type cast | composition |
| First form | form locked | print ready |
| First pull | press works | print history |
| First broadsheet | distributed | public voice |
| First manual | printed | school feed |
| First almanac | printed | seasonal guide |
| First notice board | established | official channel |
| First bound volume | bound | archive growth |
| First print sale | sold | trade income |
| First correction | printed | trust |
| First answer to rumor | printed | credibility |
| First hostile sheet answered | printed | region reference |
| First year bound | archived | history |

Milestones are the expansion's pacing: each one is a real object or act, and the
shelter's press story is the sequence of them.

---

## 59. APPENDIX AS — PAPER STORAGE AND DECAY MODEL

| Storage | Damp risk | Decay/year | Condition |
|---|---|---|---|
| Dry shelf | none | 1% | excellent |
| Archive room | low | 2% | good |
| Cellar | medium | 8% | fair |
| Loft | low | 3% | good |
| Damp corner | high | 20% | poor |
| Sealed chest | none | 1% | excellent |
| Bundled | medium | 6% | fair |
| Open shelf | low | 4% | good |

Paper decay is slow until it is fast. The shelter can ignore its archive for
years and then discover a season of records has become pulp, which is exactly why
the expansion puts the archive in a dry room with a keeper.

---

## 60. APPENDIX AT — BINDERY MODEL

| Step | Inputs | Hours | Quality effect |
|---|---|---|---|
| Fold signatures | printed sheets | 2 | alignment |
| Sew sections | thread | 4 | durability |
| Glue spine | glue | 1 | flexibility |
| Attach boards | board, cloth | 2 | protection |
| Cover | cloth, leather | 2 | wear |
| Press | weight | 12 | flatness |
| Trim | knife | 1 | clean edges |
| Label | ink, leather | 1 | catalog |

Binding is the step that turns a stack into a volume. It is slow, unglamorous,
and the only reason the shelter's knowledge will still exist in five years.

---

## 61. APPENDIX AU — READING PUBLIC MODEL

| Public | Size | Reads | Values |
|---|---|---|---|
| Children | 6–20 | primers | pictures, names |
| Workers | 10–40 | notices, rosters | clarity |
| Trades | 4–15 | manuals | accuracy |
| Parents | 5–25 | school, health | reassurance |
| Traders | 3–12 | prices, maps | reliability |
| Elders | 2–10 | memorials, history | memory |
| Visitors | varies | broadsheets | first impressions |
| The archive | permanent | everything | preservation |

The public is not a marketing demographic; it is the shelter's own people plus
the region it trades with. Every publication should have a named public, and if
it does not, it should not be printed.

---

## 62. APPENDIX AV — PRINT QUALITY BAND TABLE

| Band | Paper | Type | Ink | Effect |
|---|---|---|---|---|
| Rough | coarse | worn | thin | readable |
| Plain | common | good | good | clean |
| Clear | fine | good | good | professional |
| Fine | fine | crisp | fine | archive grade |
| Prestige | special | crisp | fine | presentation |
| Faded | any | any | poor | hard to read |
| Damaged | any | any | any | partial |

Print quality is deterministic from paper grade, type condition, and ink. The
shelter can print cheaply and often, or slowly and well, and the archive will
remember which it chose.

---

## 63. APPENDIX AW — CONTENT VOLUME SUMMARY (FINAL)

| Category | Rows | Prose estimate |
|---|---|---|
| Paper recipes | 12 | 3,000 |
| Press jobs | 20 | 4,000 |
| Type sets | 8 | 1,500 |
| Ink recipes | 8 | 2,000 |
| Publications | 40 | 10,000 |
| Almanac entries | 30 | 6,000 |
| Bulletins | 25 | 5,000 |
| Archive volumes | 15 | 3,000 |
| Editorial rules | 10 | 1,500 |
| Milestones | 14 | 2,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 17 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~69,500** |

---

## 64. APPENDIX AX — PRINTER'S APPRENTICE TRAINING TABLE

| Stage | Skill | Duration | Task | Standard |
|---|---|---|---|---|
| Fetch | 1 | week | paper, ink | no waste |
| Mix ink | 1 | week | grinding | correct color |
| Set simple type | 2 | month | notices | no typos |
| Lock form | 2 | month | chases | tight lock |
| Pull proof | 2 | month | proofing | readable |
| Run job | 3 | season | full run | clean pages |
| Compose text | 3 | season | broadsheet | correct |
| Make ready | 4 | year | press setup | even pressure |
| Print fine | 4 | year | manuals | archive grade |
| Teach | 5 | years | apprentice | independent |

Printing is a trade with a ladder, and the ladder is the same shape as the
foundry's or the weave's: fetch, assist, run, master, teach. The press is not
magic; it is a skill the shelter can grow.

---

## 65. APPENDIX AY — SHEET AND RUN TEST TABLE

| Test | Method | Pass | Fail action |
|---|---|---|---|
| Sheet ring | tap corner | clear ring | re-pulp |
| Ink hold | ink strip | even | adjust sizing |
| Tear | fold and pull | clean | add fiber |
| Dry time | humidity test | set hours | adjust ink |
| Print clarity | proof check | legible | clean type |
| Registration | align marks | true | adjust form |
| Pressure | look at reverse | even | make ready |
| Fold | fold line | no crack | improve paper |

Testing is how the press avoids wasting a run. The tests are simple, physical,
and teachable, which fits the shelter's whole relationship with quality: it is
something you check with your hands.

---

## 66. APPENDIX AZ — FIRST YEAR OF THE PRESS

| Month | Focus | Milestone |
|---|---|---|
| 1 | Rags and pulp | first sheets |
| 2 | Press repair | press turns |
| 3 | Type | composition works |
| 4 | First broadsheet | public voice |
| 5 | Market | distribution |
| 6 | Weather sheets | practical use |
| 7 | Prices | trade trust |
| 8 | Almanac | seasonal guide |
| 9 | Primer | school feed |
| 10 | Manual | knowledge stored |
| 11 | Notice system | official channel |
| 12 | Archive binding | memory kept |

Each month is one real thing the press adds to the shelter's life, which keeps
the expansion paced around capability rather than page counts.

---

## 67. CLOSING STATEMENT

ASHFALL already keeps records, owns ink, catalogs recovered documents, produces
radio, and chronicles what happens. What it lacks is the press: rags to paper,
type to form, form to page, page to a dated and signed promise that survives the
person who wrote it. The Press adds that world without adding a second archive or
a second broadcast authority. It adds a first broadsheet, an almanac that knows
the seasons, a manual printed for the school, and the shelter's name in ink where
the whole region can read it.

> Wave 4 note: this plan is one of five Wave 4 expansion bibles (27–31). Each is
> self-contained; none requires another to ship. The shared Wave 4 index lives at
> `docs/expansions/wave4/WAVE4_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `PaperPrintingCatalog` (9.3 KB), `ArchiveDeskSystem`,
> `ArchiveInkCatalogLoader`, `CulturalArchiveVaultSystem`,
> `src/UI/UndergroundPrintingPressPanel.cs`, the journal/chronicle pipeline,
> `library_manuals.json` (21.9 KB), and `lost_tech_manuals.json` (23.5 KB).