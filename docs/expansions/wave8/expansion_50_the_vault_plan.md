# ASHFALL — Expansion 50 Design Bible
# THE VAULT
### Wave 8 · Accessions, Conservation, Transcription, Microfiche, Recordings, Salons, Exhibits, and the Ethics of What a Shelter Saves

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Culture` (`CulturalArchiveVaultSystem`, `CulturalArchiveTomeCatalog`), `Ashfall.Core` (`ArchiveDeskSystem` materials seam), `Ashfall.Core.Inventory`
**Proposed host owner:** `VaultHostSession` (extends `Main.FlagshipInstitutions` wiring + `CulturalArchiveSaveStore`)
**Existing save sections:** `cultural_archives` (`cultural_archive_save.json`, `CulturalArchiveVaultSave`)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no vault-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has a working deep-vault institution. `CulturalArchiveVaultSystem`
defines `ArchiveDocumentState` (`document_id`, `physical_degradation_permille`
0..1000, `is_chemically_stabilized`, `transcription_permille` 0..1000,
`active_scholar_id`, `microfiche_copy_count`, `knowledge_preserved`, `status` =
archived | transcribing | transcribed | lost), `ArchiveProjectState`
(`document_id`, `kind` = restoration | transcription, `survivor_id`,
`started_day`, `last_progress_day`), `ArchiveRecordingState` (`recording_id`,
`category` = music_performance | oral_history | survivor_testimony |
radio_archive | commemorative, `operator_id`, `recorded_day`), `SalonState`
(`active`, `modifier_key` "salon_stress_resistance", `start_day`,
`duration_days`, `cooldown_until_day`), `ArchiveChronicleEntry` (`chronicle_id`,
`campaign_day`, `event_type`, `summary_key`, `participants`, `author_id`,
`volume_id`), and `CulturalArchiveVaultSave` (`schema_version`, `documents`,
`active_projects`, `recordings`, `chronicle_entries`, `next_chronicle_ordinal`,
`degradation_remainder`). The constants are explicit and load-bearing:
`RestorationReliefPermille` 350, `LegibilityLimitPermille` 900,
`LostThresholdPermille` 1000, `BaseDailyDegradationPermille` 2f,
`SalonDefaultDurationDays` 5, `SalonCooldownDays` 10. Events are live:
`OnDocumentRestored`, `OnMicroficheCreated`, `OnTomeTranscribed`,
`OnDocumentLost`, `OnArchiveRecordingCreated`, `OnSalonStarted`,
`OnSalonEnded`, `OnChronicleEntryAdded`, and `OnDocumentationChanged`. The host
wires the vault in `src/Main.FlagshipInstitutions.cs` with an inventory port,
an availability ledger, `CulturalArchiveTomeCatalogLoader`, the
`CulturalArchiveSaveStore`, and cross-domain event bindings. `ArchiveDeskSystem`
owns a related ink-and-transcription job surface under its own `archive_desk`
section.

What does not exist: content and practice. `cultural_archive_tomes.json`
contains exactly **twelve tomes** — a municipal mechanics handbook, hand-copied
meditations, a botany course text, a metallurgy reference, orchestral scores, a
field surgery primer, a water purification manual, a municipal register, a boxed
set of children's primers, a seed almanac, a radio service manual, and a civil
defense shelter guide. There are no accessions, no conservation materials, no
climate data, no recording blanks or playback, no exhibits, no loans, no
reading room, no chronicle volumes beyond the empty save field, and no policy
about who may read, seal, or copy what.

**The Vault** turns a storage room with a clock on it into a living cultural
institution: what enters, what is saved, who may see it, what is copied, what
is read aloud, and what is deliberately left alone. It extends the live vault
and never duplicates a records, research, press, or memory authority.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| `CulturalArchiveVaultSystem` | Documents, degradation, projects | Extends with content and practice |
| `ArchiveDeskSystem` | Ink and transcription jobs | Borrows materials; never duplicates jobs |
| 43 The Question (Wave 7) | Research and prewar decryption | Vault preserves culture; research progress stays there |
| 03 The Standing Record | Records and charters | Files volumes; never rewrites the record owner |
| 30 The Press (Wave 4) | Printing | Prints labels, cards, and reader handouts |
| 24 The Long Goodbye | Memory and mourning | Routes memorial material through it |
| 48 The Pastime (Wave 8) | Clubs, evening play | Salons are vault events; clubs remain 48's |
| 28 The Lesson (Wave 4) | Schooling | Loans primers and readers to it |
| 38 The Ward (Wave 6) | Care and surgery | Loans a primer; never owns medical knowledge |
| `ShelterNoiseSystem` (Wave 6) | Quiet hours | Salon evenings book through it |
| `VentilationSystem` | Air and moisture source | Reads climate state; no new climate authority |
| `ShelterFireHazardSystem` (Exp 47) | Fire load and safety | Vault paper is a fire load; safety routes there |
| `MemorialSystem` (Wave 3) | Names of the dead | Vault preserves, never owns, memorial records |
| 45 The Envoy (Wave 7) | Gifts and diplomacy | A copied page may travel as a gift; the envoy decides |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The vault is dry, dark, and losing a page every day. Its clock is real:
`BaseDailyDegradationPermille = 2f`, legibility fails at 900, and at 1000 the
thing is gone. The shelter has twelve tomes, a case of children's primers, an
orchestral score nobody can play in full, and a field surgery primer the ward
genuinely needs.

**The Vault** is the expansion about cultural triage: accession, conservation,
transcription, microfiche, recordings, salons, exhibits, and the ethics of
deciding which of the things a civilization leaves behind will still be readable
in fifty years. It is the expansion about the difference between storage and
stewardship.

### 1.2 The five loops it adds

```
  Accede ──► Stabilize ──► Copy ──► Share ──► Chronicle
     │          │            │        │          │
     ▼          ▼            ▼        ▼          ▼
   Provenance, repairs,   fiche,   salons,   volumes,
   consent     climate    copies   loans     reads
                                        │
                                        ▼
                          Record ──► Read ──► Decide
```

### 1.3 What the player manages

1. **Accessions.** What enters the vault, from whom, and on what terms.
2. **Provenance.** Where a thing came from and who made it.
3. **Conservation.** Stabilizing, repairing, rebinding, and storing.
4. **Climate.** Dryness, cards, shelves, and the room's honesty about itself.
5. **Transcription.** The queue, legibility, scholars, and ink.
6. **Microfiche.** Copying, readers, and permanence.
7. **Recordings.** Music, testimony, oral history, and consent.
8. **Salons.** Readings, showings, lectures, and evenings.
9. **Exhibits and loans.** Cases, labels, borrowers, and returns.
10. **Chronicles.** Year volumes, indexes, and public readings.

### 1.4 What it is not

- Not a second research or decryption system; that is 43.
- Not a second records, press, school, or memory authority.
- Not a library for its own sake; what is kept must be reachable.
- Not a looting destination; accession has consent and provenance.
- Not a doomsday wall of dying books; triage is honest and partial.
- Not a mysticism plot about a lost master text.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Culture/CulturalArchiveVaultSystem.cs` | Vault simulation | `LIVE` |
| `Assets/Ashfall.Core/Culture/CulturalArchiveTomeCatalog.cs` | Tome loading | `LIVE` |
| `Assets/Ashfall.Core/ArchiveDeskSystem.cs` | Ink and transcription jobs | `LIVE` |
| `src/Main.FlagshipInstitutions.cs` | Host wiring and events | `LIVE` |
| `src/Host/CulturalArchiveSaveStore.cs` | `cultural_archives` save | `LIVE` |
| `src/UI/ArchiveDeskPanel.cs` | Job surface | `LIVE` |
| `Assets/Ashfall.Core/VentilationSystem.cs` | Air state (read seam) | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `cultural_archive_tomes.json` | 9,530 B | **12 tomes** |
| Accession, climate, exhibit, loan catalogs | absent | confirmed none |
| Recording blanks, playback, salon catalogs | absent | confirmed none |
| Chronicle volumes beyond save field | absent | confirmed none |
| `archive_inks.json` | exists | ink owner, borrowed not duplicated |

### 2.3 Confirmed gaps

- **GAP-50-1 — Twelve tomes for a whole civilization's archive.**
- **GAP-50-2 — No accession or consent content.**
- **GAP-50-3 — No conservation materials or repair content.**
- **GAP-50-4 — No climate or storage content.**
- **GAP-50-5 — No microfiche reader or permanence practice.**
- **GAP-50-6 — No recording sessions, blanks, or playback.**
- **GAP-50-7 — No salon content beyond the state row.**
- **GAP-50-8 — No exhibits, loans, or reading room.**
- **GAP-50-9 — No chronicle volumes or public readings.**
- **GAP-50-10 — The degradation clock runs with nothing meaningful to save.**

### 2.4 Non-duplication statement

This expansion will **not** add a second research, records, press, school,
memory, ink, noise, or climate system. It extends `CulturalArchiveVaultSystem`
with catalogs and commands, borrows ink and job materials from
`ArchiveDeskSystem`, prints through the press owner, loans to the school and
ward through their owners, books salons through the quiet-hours owner, and files
chronicles with the record owner. All new state is additive inside
`CulturalArchiveVaultSave`. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Everything decays; stewardship is choosing.** Nobody can save it
all, and pretending otherwise is how nothing gets saved.

**Pillar 2 — A copy is a promise.** Microfiche, transcription, and recordings
are how a thing outlives its paper.

**Pillar 3 — Access is the point.** A vault that nobody can borrow from is a
grave with shelves.

**Pillar 4 — Consent applies to culture.** Testimony, letters, and names belong
to the people they came from.

**Pillar 5 — Small and true beats grand and lost.** Saving one readable thing
completely is a success.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Triage | Honest lists, hard choices | Omnipotent rescue |
| Repair | Steady hands, ordinary tools | Miracle restoration |
| Copies | Fan-out, redundancy | One perfect copy |
| Testimony | Consent, seals, dignity | Prying |
| Salons | Reading aloud together | Cultured superiority |
| Exhibits | Reachable, labeled | Glass tombs |
| Loss | Named and recorded | Despair loop |
| Chronicle | Years and names | Grandiosity |

### 3.3 Content limits

- No real-world texts, songs, films, or institutions copied; all invented.
- No human remains or grave goods; those route to the memory owners.
- No torture, interrogation, or coerced testimony content.
- No looting of a personal diary for drama; consent is explicit.
- No literacy shaming; reading is taught and celebrated, never tested.
- No fire-safety neglect; paper storage routes through the fire owner.
- No new save section.

---

## 4. THE VAULT WORLD

### 4.1 Interior rooms

- **`room_vault_shelf_hall`** — the long shelves and their climate cards.
- **`room_conservation_bench`** — shears, thread, glue, and weighed paper.
- **`room_film_room`** — recording blanks, playback, and quilted walls.
- **`room_reading_room`** — tables, lamps, and borrowed books.
- **`room_accession_desk`** — the register, the provenance forms, the keys.
- **`room_fiche_booth`** — the reader, spares, and the copy ledger.
- **`room_salon_hall`** — readings, showings, and evenings.
- **`room_chronicle_room`** — year volumes and the reading lectern.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_old_library` | The Old Library | 3 | Source of found books |
| `loc_school_archive` | The School Archive | 2 | Primer loans |
| `loc_print_loft` | The Print Loft | 2 | Labels and handouts |
| `loc_sound_shed` | The Sound Shed | 3 | Recording sessions |
| `loc_ward_shelf` | The Ward Shelf | 3 | Medical primer loan |
| `loc_reading_garden` | The Reading Garden | 2 | Summer salons |
| `loc_gift_table_vault` | The Gift Table | 2 | Copied pages as gifts |
| `loc_old_chapel_scrolls` | The Chapel Shelves | 3 | Found documents |
| `loc_market_wall_pages` | The Market Wall | 2 | Public chronicle readings |
| `loc_year_volume_stone` | The Year Stone | 2 | Chronicle era markers |

All locations require valid item or map-node references and scanner registration.

### 4.3 The weekly rhythm

Conservation four mornings, transcription afternoons, a salon on the first
quiet-compatible evening, recording sessions by appointment, and a public
chronicle reading each year. The expansion's clock is the shelf's clock.

---

## 5. MAIN STORYLINE — "WHAT THE VAULT KEEPS"

### 5.1 Central conflict

**Hesper Ives** runs a vault whose shelves are losing a page every day and
whose register is fourteen lines long. **Selby Ond** can stabilize twenty
documents a month and can prove it with a card. **Enid Rellie** wants the
transcription queue honest, because a transcribed page is a page that cannot be
lost. **Nils Pimmett** wants a fiche reader and a copier, because one copy in
one vault is a single accident away from nothing. **Iris Belk** wants to
record the old songs before the people who know them forget the words, and she
wants consent on every recording. **Asta Dumont** wants a salon, because a
vault nobody enters is a very expensive cupboard. **Kelso Obry** wants the
climate cards to stop lying.

The fight arrives from two directions at once: a family brings a sealed letter
written by their grandmother and asks the vault to keep it for fifty years
without reading it, and the ward asks for the field surgery primer to be
borrowed and used. The vault can honor both only if it decides what it is for.
Then the degradation clock answers for them: a municipal register crosses its
legibility limit during the same month, and the shelter learns what a deadline
on a shelf looks like.

The expansion's question: **what does a shelter owe the future it will never
meet?**

### 5.2 Theme (unspoken)

**A civilization is what someone bothered to copy.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_curator_hesper_ives` | Hesper Ives | Curator | Policy and triage |
| `npc_conservator_selby_ond` | Selby Ond | Conservator | Stabilization and repair |
| `npc_recordist_iris_belk` | Iris Belk | Recordist | Sessions and consent |
| `npc_transcriber_enid_rellie` | Enid Rellie | Transcriber | Queue and accuracy |
| `npc_microfiche_nils_pimmett` | Nils Pimmett | Microfiche | Copies and permanence |
| `npc_salon_asta_dumont` | Asta Dumont | Salon host | Evenings and readings |
| `npc_store_kelso_obry` | Kelso Obry | Store keeper | Climate and materials |
| `npc_apprentice_ebba_fray` | Ebba Fray | Apprentice | Shelves and cards |

### 5.4 Story beats (15)

1. **The Clock.** The degradation math is put on the wall.
2. **The Register.** The first accession is entered with provenance.
3. **The Queue.** Transcription becomes an ordered list.
4. **The Letter.** A sealed family letter enters the vault.
5. **The Fiche.** Copies begin and the reader works.
6. **The Songs.** Recording sessions start with consent forms.
7. **The Salon.** The first evening fills the hall.
8. **The Primers.** Children's books are lent to the school.
9. **The Ward.** The surgery primer is borrowed and returned.
10. **The Climate.** Cards, shelves, and the dry room's truth.
11. **The Volume.** The first chronicle volume is bound.
12. **The Visitor.** A copied page travels as a gift.
13. **The Last Copy.** A document at 899 is saved by hours.
14. **The Shelf.** Ebba is given a section and its cards.
15. **What the Vault Keeps.** The policy is written and read aloud.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Triage | depth / breadth / people-first | strategy |
| Copies | fiche all / fiche best / minimal | permanence |
| Access | open / borrowers / sealed-heavy | trust vs. safety |
| Testimony | recorded / transcribed / oral only | intimacy |
| Salons | frequent / monthly / rare | culture vs. quiet |
| Loans | generous / gated / none | reach vs. risk |
| Climate | strict / ordinary / improvised | care |
| Final | public archive / steward / trust | identity |

### 5.6 Endings (5 + fade)

1. **The Kept Word** — a disciplined vault with honest triage, a real queue,
   and a decade of saved things, each with a card.
2. **The Reading Room** — the vault becomes a lending institution, and more
   people read the stored books in a year than in all the years before.
3. **The Sealed Drawer** — the vault holds what it is trusted with, including
   things nobody reads, and the trust is the point.
4. **The Thousand Copies** — microfiche, transcription, and recordings put the
   shelter's culture in more than one place, and the fire drill now includes
   the copy box.
5. **The Quiet Shelf** — the vault keeps fewer things better, and every card
   says why.
6. **Fade** — a reading room with a lamp, a borrowed book at a table, a fiche
   reader humming, and a register with a new line in careful ink.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_vault_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_vault_clock`, `quest_vault_register`, `quest_vault_queue`,
`quest_vault_letter`, `quest_vault_fiche`, `quest_vault_songs`,
`quest_vault_salon`, `quest_vault_primers`, `quest_vault_ward`,
`quest_vault_climate`, `quest_vault_volume`, `quest_vault_visitor`,
`quest_vault_last_copy`, `quest_vault_shelf`, `quest_vault_what_it_keeps`.

### 6.2 Side quests (30)

**Accession (5)**
- `quest_vault_donation` — donation accepted
- `quest_vault_loan_in` — loan registered
- `quest_vault_found` — found document accessioned
- `quest_vault_provenance` — provenance recorded
- `quest_vault_consent` — consent obtained

**Conservation (5)**
- `quest_vault_stabilize` — document stabilized
- `quest_vault_repair` — pages repaired
- `quest_vault_rebind` — volume rebound
- `quest_vault_boxes` — boxes and shelves
- `quest_vault_gloves` — handling rules

**Copies (5)**
- `quest_vault_fiche_cut` — fiche produced
- `quest_vault_reader` — reader repaired
- `quest_vault_copy_ledger` — ledger kept
- `quest_vault_duplicate` — second copy stored
- `quest_vault_exchange` — copy exchanged

**Recordings (5)**
- `quest_vault_record_song` — song recorded
- `quest_vault_record_story` — story recorded
- `quest_vault_record_consent` — consent filed
- `quest_vault_playback` — session played back
- `quest_vault_label` — recordings labelled

**Salons (5)**
- `quest_vault_salon_read` — reading held
- `quest_vault_salon_show` — showing held
- `quest_vault_salon_talk` — lecture held
- `quest_vault_salon_quiet` — quiet hours respected
- `quest_vault_salon_open` — new voice invited

**Chronicles (5)**
- `quest_vault_year_volume` — volume bound
- `quest_vault_index` — index written
- `quest_vault_public_read` — public reading
- `quest_vault_gift_page` — page gifted
- `quest_vault_card` — every shelf card current

### 6.3 Repeatable quests (8)

`quest_vault_repeat_conserve`, `quest_vault_repeat_transcribe`,
`quest_vault_repeat_fiche`, `quest_vault_repeat_salon`,
`quest_vault_repeat_record`, `quest_vault_repeat_card`,
`quest_vault_repeat_loan`, `quest_vault_repeat_read`.

### 6.4 Dynamic hooks

Live events (`OnDocumentRestored`, `OnDocumentLost`, `OnMicroficheCreated`,
`OnTomeTranscribed`, `OnArchiveRecordingCreated`, `OnSalonStarted`,
`OnSalonEnded`, `OnChronicleEntryAdded`, births, deaths, school terms, ward
needs, weather) attach authored follow-ups through existing seams. No new event
bus.

### 6.5 Constraints

- Degradation, restoration, and transcription stay with the vault system.
- Ink and job materials stay with `ArchiveDeskSystem`.
- Printing stays with the press owner.
- School and ward loans go through their owners.
- Memorial material routes through the memory owner.
- Climate reads existing ventilation and temperature state; no new authority.
- No currency; copies and gifts are not goods.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `AccessionSystem` (new, `Ashfall.Core.Culture`)

**Owns:** accessions, provenance, consent terms, loans-in, seals, and refusals.
**Consumes:** inventory, survivor identity, memory owners for sensitive items.
**Data:** `accession_policy.json`. **Rules:** every item has provenance and a
term; sealed items are stored without being read; refusal is recorded and
never punished; personal material requires the owner's consent.

### 7.2 `ConservationSystem` (extend `CulturalArchiveVaultSystem`)

**Owns:** stabilization, page repair, rebinding, boxes, handling rules, and
materials. **Consumes:** `ArchiveDeskSystem` materials, workshop tools,
inventory. **Data:** `conservation_materials.json`. **Rules:** restoration
relieves degradation by the live `RestorationReliefPermille`; work uses real
materials; a repaired item's history is written on its card.

### 7.3 `FicheSystem` (extend `CulturalArchiveVaultSystem`)

**Owns:** microfiche production, readers, copy ledgers, off-site copies, and
redundancy. **Consumes:** inventory sheets and reader parts. **Data:**
`vault_fiche.json`. **Rules:** copying sets `knowledge_preserved` through the
live path; copies are tracked and one copy travels to a second store; a reader
needs a lamp and a shard of quiet.

### 7.4 `RecordingSystem` (extend `CulturalArchiveVaultSystem`)

**Owns:** recording sessions, consent forms, labels, playback, and access.
**Consumes:** `room_film_room`, blanks, playback machine. **Data:**
`vault_recordings.json`. **Rules:** categories stay as authored
(music_performance, oral_history, survivor_testimony, radio_archive,
commemorative); testimony requires explicit consent and can be sealed for a
period; playback never edits a voice.

### 7.5 `SalonSystem` (extend `CulturalArchiveVaultSystem`)

**Owns:** salons: readings, showings, lectures, and evenings, including the
live `salon_stress_resistance` modifier and cooldowns. **Consumes:**
`ShelterNoiseSystem`, seating, readers. **Data:** `vault_salons.json`.
**Rules:** salons honor quiet hours; cooldowns prevent turning culture into a
grind; anyone may read; a salon that becomes a performance hierarchy is
corrected by hosting rotation.

### 7.6 `ExhibitSystem` (new, thin, `Ashfall.Core.Culture`)

**Owns:** exhibits, cases, labels, loans to rooms and school, returns, and
condition checks. **Consumes:** rooms, school, ward, press for labels.
**Data:** `vault_exhibits.json`, `vault_loans.json`. **Rules:** exhibits are
reachable and labeled; loans are signed and returned; a loan that endangers an
item is declined with the reason recorded.

### 7.7 `ChronicleVolumeSystem` (extend `CulturalArchiveVaultSystem`)

**Owns:** year volumes, indexes, annual readings, and era markers. **Consumes:**
`ArchiveChronicleEntry` records, press binding, year stone. **Data:**
`chronicle_volumes.json`. **Rules:** volumes are bound yearly from real
entries; the public reading is short; a missing year is noted honestly.

### 7.8 `VaultClimateSystem` (new, thin, `Ashfall.Core.Culture`)

**Owns:** climate cards, dry state, shelf load, and honest reporting.
**Consumes:** `VentilationSystem` state, room temperature via the thermal
owner. **Data:** `vault_climate.json`. **Rules:** cards report the actual state;
a damp week is recorded and mitigated; no new climate simulation is created.

### 7.9 Systems explicitly not added

- No second research, records, press, school, memory, or ink system.
- No new climate or weather simulation.
- No currency, no priced copies.
- No coercion, torture, or interrogation content.
- No real-world texts or media.
- No grave goods or human remains.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `vault_documents.json` (new, catalogue additions)

```json
{
  "schema_version": 1,
  "documents": [
    {
      "document_id": "doc_municipal_register_1971_b",
      "display_name": "Municipal Register, Second Copy",
      "kind": "register",
      "degradation_permille": 240,
      "stabilized": false,
      "transcription_needed": true,
      "tags": ["civic", "reference"]
    }
  ]
}
```

### 8.2 `accession_policy.json` (new)

Policy: kinds, consent requirement, seal rules, refusal reasons, storage term.

### 8.3 `conservation_materials.json` (new)

Materials: item id, use, source, consumable, handling note.

### 8.4 `vault_climate.json` (new)

Climate: shelf zone, humidity band, temperature band, card day, mitigation.

### 8.5 `vault_fiche.json` (new)

Fiche: document, copies, reader, storage, ledger, redundancy state.

### 8.6 `vault_recordings.json` (new)

Recordings: category, performer, consent, seal until, label, access.

### 8.7 `vault_salons.json` (new)

Salons: kind, host rotation, room, quiet note, cooldown days.

### 8.8 `vault_exhibits.json` (new)

Exhibits: case, items, location, label, check cadence.

### 8.9 `vault_loans.json` (new)

Loans: borrower, item, term, condition out, condition in, return day.

### 8.10 `chronicle_volumes.json` (new)

Volumes: year, entries, index, binding, reading day, keeper.

### 8.11 Items

New items appended to `items.json`: `item_archive_box`,
`item_blotting_paper`, `item_linen_thread`, `item_bookbinder_glue`,
`item_microfiche_sheet`, `item_fiche_reader_lamp`, `item_recording_blank`,
`item_playback_machine`, `item_humidity_card`, `item_cotton_gloves`,
`item_shelf_label`, `item_catalogue_card`, `item_exhibit_case`,
`item_press_board`, `item_repair_kit`, `item_reading_lamp`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`CulturalArchiveVaultSave` remains the live save owner. New sub-objects
(accessions, policy, climate cards, fiche ledger, recordings, salons, exhibits,
loans, volumes) are additive inside it. No new save section.

### 9.2 State to persist

- Documents with degradation, stabilization, and status.
- Accession register with provenance, terms, and seals.
- Conservation cards and material use.
- Fiche copies, readers, and off-site redundancy.
- Recording sessions with consent and seal dates.
- Salon history and cooldowns.
- Exhibits, loans, and returns.
- Chronicle volumes, indexes, and reading dates.
- Climate card history by shelf zone.

### 9.3 Determinism

- Degradation continues on the live daily path with `degradation_remainder`.
- Restoration relief uses the live constant; transcription progress derives
  from assigned scholars and days worked.
- Copy counts derive from sessions, not random duplication.
- Salon modifiers use the live state fields.
- Climate reads ventilation and temperature state.
- Paired replay hashes must match; no `System.Random`.

### 9.4 Migration

Legacy saves load with documents, projects, recordings, chronicles, and the
degradation remainder intact; no accessions, policy, cards, or loans exist until
started. Documents already above the legibility limit load as transcribed-only
where the live status allows, and as lost where it does not.

### 9.5 Checksum

Invariant-culture floats; integer permille, day, and copy fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `ArchiveDeskPanel` (extend) | Jobs and inks | `VaultHostSession` |
| `VaultShelfPanel` (new) | Documents and clocks | same |
| `AccessionPanel` (new) | Register and consent | same |
| `FichePanel` (new) | Copies and readers | same |
| `RecordingPanel` (new) | Sessions and seals | same |
| `SalonPanel` (new) | Evenings and readings | same |
| `ChronicleVolumePanel` (new) | Volumes and readings | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Degradation is shown as a number and a date, never as vague dread.
- Sealed items show as sealed; no panel ever peeks.
- Reading room content can be shown or read aloud; no timed tests.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Loan terms and due days are visible before acceptance.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a page turning carefully, a fiche
reader's fan, a bound volume creaking, a lamp being lit in a reading room, a
chair drawn to a lectern. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `CulturalArchiveVaultSystem` | Documents, copies, salons, chronicles |
| `ArchiveDeskSystem` | Inks and job materials |
| `CulturalArchiveTomeCatalog` | Catalogue loading |
| `VentilationSystem` | Climate read |
| `ShelterThermalSystem` | Room temperature read |
| `ShelterNoiseSystem` (Wave 6) | Salon scheduling |
| `PressHostSession` (Wave 4) | Labels, handouts, binding |
| `School` (Wave 4) | Primer and reader loans |
| `MedicalWardSystem` (Wave 6) | Primer loan |
| `MemorialSystem` (Wave 3) | Memorial material |
| `StandingRecord` (Exp 03) | Volume filing |
| `Radio` family | Radio-archive recordings reference broadcasts |
| 43 The Question (Wave 7) | Research uses readable copies |
| 45 The Envoy (Wave 7) | Copied pages as gifts |
| `ShelterFireHazardSystem` (Exp 47) | Paper fire load and rules |
| `EpilogueChronicleBuilder` | Volume summaries |
| `Inventory` | Materials and outputs |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm vault system, tome catalog, desk
system, host wiring, save store, panels, ventilation, thermal, press, and record
owners. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author the ten catalogs and the document
catalogue additions; register validators and scanner.

**Phase 2 — Pure Core.** `AccessionSystem`, conservation and fiche extensions,
`RecordingSystem`, `SalonSystem`, `ExhibitSystem`,
`ChronicleVolumeSystem`, `VaultClimateSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `VaultHostSession`, focused selftest coverage, fresh
journey from the clock to the public reading.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Year-long soak: triage beats the clock, copies
redundant, salons sustainable, loans returned.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Documents (catalogue additions) | 40 |
| Accession entries | 24 |
| Conservation cards | 30 |
| Climate cards | 12 |
| Fiche entries | 20 |
| Recordings | 24 |
| Salons | 16 |
| Exhibits | 12 |
| Loans | 16 |
| Chronicle volumes | 10 |
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
| Research duplication | Critical | 43 boundary |
| Records duplication | High | File through owner |
| Loot framing | High | Consent and provenance |
| Real-content copying | High | Invented corpus |
| Fire neglect | Medium | Fire owner rules |
| Climate simulation | Medium | Read-only state |
| Salon grind | Medium | Cooldowns |
| Determinism break | Low | Live degradation path |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `vault_documents.json` | 40 | 6,000 |
| `accession_policy.json` | 24 | 4,000 |
| `conservation_materials.json` | 16 | 2,500 |
| `vault_climate.json` | 12 | 2,000 |
| `vault_fiche.json` | 20 | 3,000 |
| `vault_recordings.json` | 24 | 4,000 |
| `vault_salons.json` | 16 | 3,000 |
| `vault_exhibits.json` | 12 | 2,000 |
| `vault_loans.json` | 16 | 2,500 |
| `chronicle_volumes.json` | 10 | 2,500 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~62,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R50-1 | Research overlap | Low | Critical | Boundary |
| R50-2 | Records overlap | Med | High | Filing |
| R50-3 | Loot framing | Med | High | Consent |
| R50-4 | Real texts | Low | High | Invention |
| R50-5 | Fire neglect | Med | Medium | Owner rules |
| R50-6 | Climate sim | Low | Medium | Read-only |
| R50-7 | Salon grind | Med | Medium | Cooldowns |
| R50-8 | Determinism | Low | High | Live path |
| R50-9 | Content overrun | Med | Medium | Budget §13 |
| R50-10 | Despair tone | Med | Medium | Triage framing |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can sealed items ever be opened?** Recommended: only by the depositor, or
   on a written term that names an opener and a date.
2. **Who may borrow?** Recommended: any resident in good standing, with signed
   terms and condition checks.
3. **Are copies free?** Recommended: yes; copies are not goods and are never
   priced.
4. **What happens when the clock wins?** Recommended: the document is marked
   lost with a card that records what it was and who tried.
5. **Does the vault accept items with no provenance?** Recommended: yes, with
   the provenance field marked unknown rather than invented.

---

## 17. APPENDIX D — DOCUMENT CATALOGUE TABLE (40 DOCUMENTS)

| # | Document | Kind | Degradation | Stabilized | Transcribe |
|---|---|---|---|---|---|
| 1 | Municipal mechanics handbook | technical | 180 | yes | no |
| 2 | Hand-copied meditations | reflection | 90 | yes | no |
| 3 | Agricultural botany text | farming | 210 | yes | no |
| 4 | Metallurgy reference | foundry | 260 | yes | pending |
| 5 | Orchestral scores | music | 330 | no | pending |
| 6 | Field surgery primer | medical | 150 | yes | no |
| 7 | Water works manual | utility | 190 | yes | no |
| 8 | Municipal register 1971 | civic | 610 | no | urgent |
| 9 | Children's primers | education | 120 | yes | no |
| 10 | Seed almanac | farming | 240 | yes | pending |
| 11 | Radio service manual | radio | 280 | no | pending |
| 12 | Shelter placement guide | civic | 350 | no | pending |
| 13 | Register second copy | civic | 420 | no | pending |
| 14 | Weather almanacs | weather | 300 | no | pending |
| 15 | Herb recipes | food | 220 | yes | no |
| 16 | Legal charters | civic | 380 | no | pending |
| 17 | Machine shop tables | workshop | 170 | yes | no |
| 18 | Survey field notes | survey | 260 | no | pending |
| 19 | Poetry hand-copied | literature | 140 | yes | no |
| 20 | Ledger of names | civic | 480 | no | pending |
| 21 | Repair manuals set | workshop | 230 | no | pending |
| 22 | Chemistry bench notes | reagent | 290 | no | pending |
| 23 | Midwife notes | medical | 160 | yes | no |
| 24 | Star charts | knowledge | 200 | yes | no |
| 25 | Historical reader | education | 310 | no | pending |
| 26 | Song collection | music | 350 | no | pending |
| 27 | Story annual | literature | 280 | no | pending |
| 28 | Building code book | construction | 340 | no | pending |
| 29 | Veterinary notes | farming | 250 | no | pending |
| 30 | Map commentary | geography | 370 | no | pending |
| 31 | Cookery notebook | food | 130 | yes | no |
| 32 | Electricity primer | grid | 210 | yes | no |
| 33 | First aid cards | medical | 100 | yes | no |
| 34 | Mouth organ tunes | music | 360 | no | pending |
| 35 | Letter collections | literature | 520 | no | urgent |
| 36 | Assay tables | foundry | 280 | no | pending |
| 37 | Orchard grafting notes | farming | 240 | yes | no |
| 38 | Radio log copies | radio | 430 | no | pending |
| 39 | Prayer book, hand-copied | reflection | 320 | no | pending |
| 40 | The diary of the second winter | literature | 580 | no | urgent |

Forty documents with a degradation column that is the expansion's whole
drama: the register, the letter collections, and the second winter diary are
past or near the nine hundred legibility line, and everything else waits in a
queue that cannot hold them all. The catalogue is deliberately dull in the
middle and urgent at the edges, which is what a real archive looks like.

---

## 18. APPENDIX E — ACCESSION TABLE (24 ENTRIES)

| # | Item | Source | Terms | Seal | Storage |
|---|---|---|---|---|---|
| 1 | Family letter | donation | keep, unopened | 50 years | drawer |
| 2 | Photograph set | donation | keep, show | none | box |
| 3 | Work diary | donation | keep, borrow | none | shelf |
| 4 | Recipe book | find | keep, copy | none | shelf |
| 5 | Song sheet | donation | keep, record | none | box |
| 6 | Child drawing | donation | keep, show | none | wall |
| 7 | Medal box | donation | keep, hide | until kin | drawer |
| 8 | School register | loan-in | borrow, return | none | shelf |
| 9 | Tool catalogue | find | keep, copy | none | shelf |
| 10 | Map board | find | keep, show | none | hall |
| 11 | Letters home | donation | keep, seal | 25 years | drawer |
| 12 | Notebook poems | donation | keep, read | none | shelf |
| 13 | Machinery plates | find | keep, use | none | workshop |
| 14 | Seed packets | find | keep, use | none | cold shelf |
| 15 | Radio cards | find | keep, guide | none | shelf |
| 16 | Baby record | donation | keep, seal | until child | drawer |
| 17 | Plane tables | loan-in | borrow, return | none | desk |
| 18 | Water colors | donation | keep, show | none | wall |
| 19 | Story reel | find | keep, play | none | film room |
| 20 | Work boots note | donation | keep, read | none | shelf |
| 21 | Census copy | find | keep, limited | names sealed | shelf |
| 22 | Choir voice tape | donation | keep, play | none | film room |
| 23 | Court summary | donation | keep, seal | 30 years | drawer |
| 24 | Grandmother's ring note | donation | keep, sealed | family only | drawer |

Twenty-four accession entries, and the seal column is where the expansion's
ethics live: six items are held under terms that the vault cannot violate, and
the catalogue is honest about terms it does not know. The census row is the
sharpest test, because a civic record with sealed names is both useful and
dangerous, and the vault's rule is that the terms win.

---

## 19. APPENDIX F — CONSERVATION CARD TABLE

| # | Document | Work | Materials | Days | Result |
|---|---|---|---|---|---|
| 1 | Register 1971 | stabilize | blotting, box | 3 | relieved 350 |
| 2 | Letter collection | relax, flatten | weights, paper | 5 | relieved 350 |
| 3 | Winter diary | repair spine | thread, glue | 4 | relieved 350 |
| 4 | Mechanics handbook | rebind | board, cloth | 6 | improved 350 |
| 5 | Botany text | deacidify | wash, dry | 4 | relieved 350 |
| 6 | Song collection | mend pages | tissue, paste | 3 | relieved 350 |
| 7 | Surgery primer | clean, box | cloth, gloves | 2 | relieved 350 |
| 8 | Seed almanac | mend edges | tissue | 2 | relieved 350 |
| 9 | Radio manual | rebound | thread, board | 5 | relieved 350 |
| 10 | Charters | press | boards, weights | 4 | relieved 350 |
| 11 | Reader | new cover | cloth, thread | 3 | improved 350 |
| 12 | Star charts | roll, tube | tube, cloth | 1 | relieved 350 |

Twelve conservation cards with materials and days, and every row's relief is
the live constant of 350 permille. The cards are the expansion's honesty
device: restoration is a real fraction of a clock, not a cure, and the shelf
figure keeps its remaining decay after every careful week of work.

---

## 20. APPENDIX G — CLIMATE CARD TABLE

| # | Shelf | Humidity | Temperature | Card | Mitigation |
|---|---|---|---|---|---|
| 1 | A (registers) | 52% | 18 C | weekly | dry packs |
| 2 | B (literature) | 50% | 18 C | weekly | none |
| 3 | C (technical) | 55% | 19 C | weekly | vent |
| 4 | D (music) | 48% | 17 C | weekly | none |
| 5 | E (medical) | 51% | 18 C | weekly | dry packs |
| 6 | F (children) | 53% | 19 C | weekly | none |
| 7 | G (civic) | 58% | 20 C | daily | vent, fan |
| 8 | H (fiction) | 49% | 18 C | weekly | none |
| 9 | I (maps) | 47% | 17 C | weekly | flat storage |
| 10 | J (letters) | 54% | 18 C | daily | dry, seal |
| 11 | K (oversize) | 56% | 19 C | weekly | roll or flat |
| 12 | L (years) | 50% | 18 C | weekly | none |

Twelve climate cards, two shelves on daily watch, and the trick in the G row is
that a permanently damp corner exists and the vault's answer is not a
simulation but a fan, a vent, and somebody writing the number down. The cards
are posted where borrowers can read them, which is a quiet way of saying the
vault has nothing to hide about its own conditions.

---

## 21. APPENDIX H — FICHE LEDGER TABLE

| # | Document | Copies | Stored | Reader | Check |
|---|---|---|---|---|---|
| 1 | Mechanics handbook | 3 | vault, school, gift | yes | yearly |
| 2 | Surgery primer | 3 | vault, ward, outpost | yes | yearly |
| 3 | Water manual | 2 | vault, plant | yes | yearly |
| 4 | Seed almanac | 2 | vault, farm | yes | yearly |
| 5 | Children's primers | 4 | vault, school, two rooms | yes | yearly |
| 6 | Municipal register | 2 | vault, terrace | yes | yearly |
| 7 | Botany text | 2 | vault, farm | yes | yearly |
| 8 | Radio manual | 2 | vault, station | yes | yearly |
| 9 | Star charts | 2 | vault, school | yes | yearly |
| 10 | Song collection | 3 | vault, choir, outpost | yes | yearly |

Ten fiche entries, each in at least two places, some in four. The ledger is the
expansion's answer to fire and flood and time: a single copy is a hope, and
two copies in two buildings is a plan. The outpost and terrace rows exist to
show that the shelter's culture is not a bunker possession, it is a valley one.

---

## 22. APPENDIX I — RECORDING TABLE (24 RECORDINGS)

| # | Recording | Category | Consent | Seal | Access |
|---|---|---|---|---|---|
| 1 | Work songs | music_performance | choir | none | open |
| 2 | Winter lullaby | music_performance | singer | none | open |
| 3 | First winter story | oral_history | teller | none | open |
| 4 | Foundry years | oral_history | teller | none | open |
| 5 | Walk from the coast | oral_history | teller | none | open |
| 6 | Testimony: flood day | survivor_testimony | speaker | 10 years | sealed |
| 7 | Testimony: raid night | survivor_testimony | speaker | until kin | sealed |
| 8 | Testimony: mine collapse | survivor_testimony | speaker | none | open |
| 9 | Radio archive: first broadcast | radio_archive | station | none | open |
| 10 | Radio archive: distress calls | radio_archive | station | 5 years | limited |
| 11 | Memorial reading | commemorative | family | none | open |
| 12 | Founding day | commemorative | all | none | open |
| 13 | Children's choir | music_performance | parents | none | open |
| 14 | Grandfather's recipes | oral_history | teller | none | open |
| 15 | Testimony: hospital year | survivor_testimony | speaker | 15 years | sealed |
| 16 | Work chant | music_performance | crew | none | open |
| 17 | Story: the green door | oral_history | teller | none | open |
| 18 | Instrument practice | music_performance | player | none | open |
| 19 | Testimony: the trial | survivor_testimony | speaker | 20 years | sealed |
| 20 | Radio archive: weather years | radio_archive | station | none | open |
| 21 | Names reading | commemorative | all | none | open |
| 22 | Grandmother's song | music_performance | family | none | open |
| 23 | Testimony: the crossing | survivor_testimony | speaker | 10 years | sealed |
| 24 | Year reading | commemorative | all | none | open |

Twenty-four recordings, six of them sealed for a period the speaker chose, and
the seal column is respected in every screen and every quest. The distress-call
archive is limited on purpose: some radio history is valuable and still sharp,
and the vault's answer is a dated seal and a note, not a lock and a lie.

---

## 23. APPENDIX J — SALON TABLE (16 EVENTS)

| # | Salon | Kind | Host | Room | Cooldown |
|---|---|---|---|---|---|
| 1 | Reading: winter diary | reading | Hesper | reading room | 10 days |
| 2 | Showing: star charts | showing | Nils | hall | 10 days |
| 3 | Talk: water works | lecture | Enid | hall | 10 days |
| 4 | Reading: poems | reading | Asta | reading room | 10 days |
| 5 | Listening: work songs | listening | Iris | film room | 10 days |
| 6 | Talk: orchard grafting | lecture | Selby | hall | 10 days |
| 7 | Reading: children's hour | reading | Ebba | reading room | 10 days |
| 8 | Showing: municipal maps | showing | Kelso | hall | 10 days |
| 9 | Listening: first broadcast | listening | Iris | film room | 10 days |
| 10 | Talk: seed saving | lecture | Enid | hall | 10 days |
| 11 | Reading: the green door | reading | Ebba | garden | 10 days |
| 12 | Listening: choir | listening | Asta | hall | 10 days |
| 13 | Talk: reading the sky | lecture | Nils | hall | 10 days |
| 14 | Reading: letters read allowed | reading | Hesper | reading room | 10 days |
| 15 | Listening: memorial reading | listening | Asta | hall | 10 days |
| 16 | Reading: year volume | reading | Hesper | hall | 10 days |

Sixteen salons with rotating hosts, and the ten-day cooldown does quiet
structural work: it prevents the vault from becoming the shelter's only evening
entertainment and keeps its events special. The garden and children's rows
widen the salon beyond a hall and a lectern, which is how a culture stops being
a room and starts being a habit.

---

## 24. APPENDIX K — EXHIBIT AND LOAN TABLE

| # | Item | Exhibit | Borrower | Term | Check |
|---|---|---|---|---|---|
| 1 | Star charts | hall case | school | term | weekly |
| 2 | Water manual fiche | plant | utilities | month | monthly |
| 3 | Surgery primer | ward shelf | ward | month | monthly |
| 4 | Seed almanac | farm room | farm | season | seasonal |
| 5 | Children's primers | school | school | term | weekly |
| 6 | Map board | hall case | none | display | monthly |
| 7 | Photo set | hall case | none | display | monthly |
| 8 | Work diary | shelf | reader | month | monthly |
| 9 | Radio manual | station | radio | month | monthly |
| 10 | Song book | choir | choir | season | seasonal |
| 11 | Star reader | reading room | children | week | weekly |
| 12 | First-aid cards | clinic | clinic | permanent | yearly |

Twelve exhibit and loan rows, and the ward and school rows are the ones that
justify the whole institution: a vault that lends a surgery primer to the ward
and a primer to a child is not a museum, it is infrastructure. The permanent
first-aid row is a deliberate exception, because some things should simply live
where they are needed.

---

## 25. APPENDIX L — CHRONICLE VOLUME TABLE

| # | Year | Entries | Index | Bound | Read |
|---|---|---|---|---|---|
| 1 | Year one | 22 | yes | cloth | public |
| 2 | Year two | 31 | yes | cloth | public |
| 3 | Year three | 28 | yes | cloth | public |
| 4 | Year four | 40 | yes | cloth | public |
| 5 | Year five | 36 | yes | cloth | public |
| 6 | Year six | 33 | yes | cloth | public |
| 7 | Year seven | 45 | yes | cloth | public |
| 8 | Year eight | 38 | yes | cloth | public |
| 9 | Year nine | 41 | yes | cloth | public |
| 10 | Year ten | 44 | yes | cloth | public |

Ten volumes with real entry counts and a public reading each year, and the
counts rising through year four and dipping after hard years is the honest shape
of an institution that records what actually happened. The bound column stays
cloth for a reason: a chronicle should be mendable, not precious.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_vault_clock` | 3 | Clock understood |
| `quest_vault_register` | 4 | Register opened |
| `quest_vault_queue` | 4 | Queue ordered |
| `quest_vault_letter` | 4 | Seal accepted |
| `quest_vault_fiche` | 4 | First copies made |
| `quest_vault_songs` | 4 | Sessions recorded |
| `quest_vault_salon` | 3 | First salon held |
| `quest_vault_primers` | 3 | School served |
| `quest_vault_ward` | 3 | Primer borrowed and returned |
| `quest_vault_climate` | 4 | Cards honest |
| `quest_vault_volume` | 4 | Volume bound |
| `quest_vault_visitor` | 3 | Copy gifted |
| `quest_vault_last_copy` | 4 | Document saved at 899 |
| `quest_vault_shelf` | 4 | Section handed over |
| `quest_vault_what_it_keeps` | 3 | Policy read aloud |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_vault_donation` | 3 | Donation accepted |
| `quest_vault_loan_in` | 3 | Loan registered |
| `quest_vault_found` | 3 | Find accessioned |
| `quest_vault_provenance` | 3 | Provenance recorded |
| `quest_vault_consent` | 3 | Consent obtained |
| `quest_vault_stabilize` | 3 | Stabilized |
| `quest_vault_repair` | 3 | Pages repaired |
| `quest_vault_rebind` | 4 | Volume rebound |
| `quest_vault_boxes` | 3 | Boxes made |
| `quest_vault_gloves` | 3 | Rules posted |
| `quest_vault_fiche_cut` | 3 | Fiche produced |
| `quest_vault_reader` | 3 | Reader repaired |
| `quest_vault_copy_ledger` | 3 | Ledger current |
| `quest_vault_duplicate` | 3 | Copy stored away |
| `quest_vault_exchange` | 3 | Copy exchanged |
| `quest_vault_record_song` | 3 | Song recorded |
| `quest_vault_record_story` | 3 | Story recorded |
| `quest_vault_record_consent` | 3 | Consent filed |
| `quest_vault_playback` | 3 | Playback checked |
| `quest_vault_label` | 3 | Label written |
| `quest_vault_salon_read` | 3 | Reading held |
| `quest_vault_salon_show` | 3 | Showing held |
| `quest_vault_salon_talk` | 3 | Lecture held |
| `quest_vault_salon_quiet` | 3 | Quiet kept |
| `quest_vault_salon_open` | 3 | New voice invited |
| `quest_vault_year_volume` | 4 | Volume bound |
| `quest_vault_index` | 3 | Index written |
| `quest_vault_public_read` | 3 | Public reading |
| `quest_vault_gift_page` | 3 | Page gifted |
| `quest_vault_card` | 3 | Cards current |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Hesper Ives** — curator. Runs triage with a card and a clock and has said no
to beautiful documents that could not survive the trip. Believes a policy is
the only thing standing between a vault and a hoard.

**Selby Ond** — conservator. Works with a bone folder and a weight and can tell
a dying page by its smell. Believes repair is a conversation with the maker.

**Iris Belk** — recordist. Sets the machine, asks consent twice, and stops
recording the moment someone hesitates. Believes a voice is a loan, not a
capture.

**Enid Rellie** — transcriber. Copies at one table in one hand and reads every
line aloud before it goes into the file. Believes a transcription is a promise
to someone she will never meet.

**Nils Pimmett** — microfiche. Made three copies of the surgery primer and
cried when the reader first worked. Believes one copy is a hope and two is a
plan.

**Asta Dumont** — salon host. Fills a hall without making anyone perform and
rotates hosts so no voice becomes the only voice. Believes culture is a
Thursday, not a monument.

**Kelso Obry** — store. Writes the humidity number down whether it is good or
bad and posts it where borrowers can see. Believes a card that lies is worse
than no card.

**Ebba Fray** — apprentice. Eighteen, given the children's section and its
cards, and reads aloud to a rug of listeners twice a month. Believes the
smallest shelf is the most important one.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Old Library** — collapsed shelves and one intact drawer.
- **The School Archive** — primer loans and a return box.
- **The Print Loft** — labels, handouts, and a press that knows the vault.
- **The Sound Shed** — quilted walls and a machine that must be warmed.
- **The Ward Shelf** — the surgery primer, back on time.
- **The Reading Garden** — benches, shade, and summer salons.
- **The Gift Table** — copied pages going out with visitors.
- **The Chapel Shelves** — folded documents and no theology.
- **The Market Wall** — the year volume read aloud where people pass.
- **The Year Stone** — era marks for a chronicle that keeps growing. 

---

## 30. APPENDIX Q — STEWARDSHIP CHARTER

| Clause | Promise |
|---|---|
| Triage | Not everything can be saved, and we say which |
| Consent | Personal things belong to their people |
| Seals | A closed drawer stays closed |
| Copies | Nothing unique is stored alone |
| Repairs | Every treatment is dated and signed |
| Climate | The cards never lie |
| Access | Borrowing is a right, returning is a promise |
| Loans | Out to the school, the ward, and the rooms |
| Reading | The year is read aloud by a rotating voice |
| Loss | What is lost is recorded with its name |

The stewardship charter is the expansion's first-class design object, and its
last line is the one that keeps the vault humane: loss is not hidden, it is
recorded, so the shelter's carelessness or luck is part of the record rather
than a gap in it.

---

## 32. APPENDIX R — WORKED VAULT YEAR

**Month one.** Hesper puts the degradation math on the wall: two permille a
day, legibility at nine hundred, and a list of forty documents with remaining
days beside each. The register is opened with the first line in careful ink,
and the queue is ordered worst-first instead of nicest-first.

**Month two.** Selby stabilizes six documents in eight days and proves the
relief on a card. The register 1971 moves from 610 to 260 and buys the shelter
two hundred days it did not have.

**Month three.** The sealed family letter arrives. Hesper reads the terms
aloud, marks the drawer, and puts it away without opening it. The family asks
whether anyone will ever read it, and the answer is: that is yours to decide,
not ours.

**Month four.** Nils cuts the first fiches and stores three copies in three
places. The surgery primer goes to the ward shelf with a signed loan card, and
comes back after a month with a coffee mark that is cleaned, recorded, and
forgiven.

**Month five.** Iris records the work songs and the winter lullaby, consent
filed for both, and stops a third session when the singer hesitates. The vault
has voices now, and the hall hears them on the first listening salon.

**Month six.** The first salon fills the reading room. Asta rotates the host
position immediately, and the second salon is hosted by Ebba, who is eighteen
and nervous and good at it.

**Month seven.** The children's primers go to the school for a term and come
back with a cracked spine. Selby rebinds them in cloth, and the school
expresses an interest in a return box, which becomes a permanent fixture.

**Month eight.** The climate cards catch the damp corner in shelf G and the
answer is a fan, a vent, and a daily number on a public board. Nobody wanted
the vault to hide its own weather, and now it does not.

**Month nine.** The first chronicle volume is bound from real entries, indexed
by Ebba, and read aloud at the market wall to about forty people. The reading
is eleven minutes long and gets better in year two because the editor learns
what a crowd will listen to.

**Month ten.** A trade visitor receives a copied page as a gift. The vault's
culture leaves the bunker for the first time in the form of a single sheet, and
the envoy's owner records the gift.

**Month eleven.** The second winter diary reaches 899. Enid transcribes it in
three days at one table, and the copy becomes the readable version while the
original is boxed. The card says: original fragile, copy complete, both kept.

**Month twelve.** Hesper hands Ebba the children's section and its cards, and
the policy is read aloud at the year reading, including the line about loss.
The register has forty lines, the fiche ledger has fifteen, the vault is dry,
and the clock is still running, which is the correct state of affairs.

---

## 33. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Enid copies one line at a time and reads each aloud before writing the next,
> and someone asks why, and she says: because the person I am copying would hate
> to be misquoted, and they are not here to correct me.

> Iris sets the machine and asks again if it is all right, and the singer says
> yes twice, and the second yes is the one the machine hears.

> Hesper holds the sealed letter for a moment and does not open it, and the
> weight of it in her hand is exactly the weight of the trust, and she puts it in
> the drawer and turns the key and that is the whole ceremony.

> The market wall reading ends and an old man stays behind and says he wrote a
> line in volume four, and the curator finds it and reads it back to him, and he
> is surprised that the vault kept his name.

---

## 34. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| No triage | everything half-saved | worst-first queue |
| Clock missed | document lost | cards with dates |
| Single copy | one fire ends it | fiche fan-out |
| Seal broken | trust destroyed | policy and apology |
| Bad climate | rot unnoticed | daily card |
| No salon | vault unused | host rotation |
| Loan lost | shelf gap | terms and checks |
| Transcription error | wrong record | read-aloud rule |
| Gift refused | closed culture | open circulation |
| Loss hidden | dishonest record | loss cards |

The recovery column is the same practice in every row — write it down, tell the
truth, and make a copy — which is what a vault is: a set of habits for being
wrong in a survivable way.

---

## 35. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] No research, records, press, school, or memory duplication.
- [ ] Degradation runs on the live daily path.
- [ ] Restoration uses the live relief constant.
- [ ] Seals are honored in every panel and quest.
- [ ] Consent is required for testimony and recordings.
- [ ] Climate reads existing ventilation and temperature state.
- [ ] Paper storage routes through the fire owner.
- [ ] No real-world texts, songs, or media are copied.
- [ ] Save additions are additive inside `cultural_archives`.
- [ ] Determinism uses the live degradation path.

---

## 36. APPENDIX V — GLOSSARY

- **Accession** — the act of taking something into the vault with terms.
- **Provenance** — where a thing came from, or an honest unknown.
- **Seal** — a term that keeps something unread until a date or condition.
- **Stabilization** — work that slows decay without pretending to cure it.
- **Fiche** — a reduced copy that outlives its original paper.
- **Testimony** — a recorded account given with explicit consent.
- **Salon** — a hosted evening of reading, showing, or listening.
- **Loan** — an item out of the vault under signed terms and a return day.
- **Volume** — a bound year of chronicle entries with an index.
- **Loss card** — the record of a thing that decayed past saving.

---

## 37. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `CulturalArchiveVaultSystem` | time | documents | research |
| `AccessionSystem` | inventory | register | policy of others |
| `ConservationSystem` | materials | cards | time |
| `FicheSystem` | inventory | copies | originals |
| `RecordingSystem` | sessions | recordings | consent of others |
| `SalonSystem` | rooms | salons | quiet policy |
| `ExhibitSystem` | rooms | loans | items permanently |
| `ChronicleVolumeSystem` | entries | volumes | record owner |
| `VaultClimateSystem` | ventilation | cards | climate |
| `ArchiveDeskSystem` | inks | jobs | vault state |
| `VentilationSystem` | nothing | nothing | nothing |
| `ShelterNoiseSystem` | quiet | nothing | nothing |
| `PressHostSession` | jobs | prints | nothing |
| `School` | loans | nothing | nothing |
| `MedicalWardSystem` | loans | nothing | nothing |
| `MemorialSystem` | memory | nothing | nothing |
| `StandingRecord` | records | records | nothing |
| `Inventory` | items | nothing | nothing |

---

## 38. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`vault_documents.json`** — `document_id`, `display_name`, `kind`,
`degradation_permille`, `stabilized`, `transcription_needed`, `tags[]`.

**`accession_policy.json`** — `policy_id`, `kind`, `consent_required`,
`seal_rules[]`, `refusal_reasons[]`, `storage_term`, `tags[]`.

**`conservation_materials.json`** — `item_id`, `use`, `source`, `consumable`,
`handling_note`, `tags[]`.

**`vault_climate.json`** — `shelf_id`, `humidity_band`, `temperature_band`,
`card_cadence`, `mitigation`, `tags[]`.

**`vault_fiche.json`** — `document_id`, `copies`, `storage_places[]`, `reader`,
`ledger_state`, `tags[]`.

**`vault_recordings.json`** — `recording_id`, `category`, `performer_id`,
`consent`, `seal_until`, `label`, `access`, `tags[]`.

**`vault_salons.json`** — `salon_id`, `kind`, `host_id`, `room_id`,
`quiet_note`, `cooldown_days`, `tags[]`.

**`vault_exhibits.json`** — `exhibit_id`, `case`, `items[]`, `location_id`,
`label`, `check_cadence`, `tags[]`.

**`vault_loans.json`** — `loan_id`, `borrower_id`, `item_id`, `term_days`,
`condition_out`, `condition_in`, `return_day`, `tags[]`.

**`chronicle_volumes.json`** — `volume_id`, `year`, `entries`, `index`,
`binding`, `reading_day`, `keeper_id`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid item or room references, or out-of-range
numbers.

---

## 39. APPENDIX Y — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Documents saved | triage outcome | Vault |
| Documents lost | honesty | Vault |
| Average degradation | shelf health | Cards |
| Copies per document | redundancy | Fiche |
| Recordings consented | trust | Recordings |
| Salons held | reach | Salons |
| Loans returned | access | Loans |
| Volumes bound | continuity | Chronicles |
| Reading attendance | public life | Salons |
| Climate incidents | storage quality | Cards |

Telemetry is diagnostic only; it never gates content and never becomes a score
against a conservator or a decade.

---

## 40. APPENDIX Z — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive inside `cultural_archives`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows triage beating the clock with copies and cards.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No peek-at-sealed, loot, or real-content behavior exists.

---

## 41. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. Can a seal be extended or shortened by the depositor's family after death?
2. Who decides when a document is too damaged to lend?
3. Does a transcribed copy ever replace the original for display?
4. Can the vault decline an accession for space, and how is that recorded?
5. Is a loss ever anyone's fault, and does the card name it?
6. Can outposts hold copies, and who checks them?
7. How does the vault handle a donation offered in exchange for a favor?
8. What happens when two families claim one document?

None of these may be decided unilaterally; each changes tone and balance.

---

## 42. APPENDIX AB — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Children's primers and story hours |
| 1 | 13 Faith | Reflection texts and quiet reading |
| 2 | 17 The Long Evening | Elder testimony and songs |
| 2 | 19 The Bitter Air | Climate cards and filtered air |
| 3 | 22 The Clean Flow | Water manuals in use |
| 3 | 24 The Long Goodbye | Memorial material and readings |
| 4 | 27 The Thread | Cloth binding and repair |
| 4 | 28 The Lesson | School loans and readers |
| 4 | 30 The Press | Labels, handouts, binding |
| 4 | 31 The Kiln | Clay storage and cases |
| 5 | 34 The Long Road | Copies for waystations |
| 6 | 38 The Ward | Surgery primer loan |
| 6 | 40 The Wheel | Reader mechanisms and repairs |
| 6 | 41 The Quiet | Salon evenings and quiet hours |
| 7 | 43 The Question | Readable copies for research |
| 7 | 44 The Outpost | Fiche copies at the far end |
| 7 | 45 The Envoy | Copied pages as gifts |
| 7 | 46 The Long Change | Year volumes across decades |
| 8 | 47 The Brigade | Paper fire load and protection |
| 8 | 48 The Pastime | Salons and clubs sharing a week |
| 8 | 49 The Mirror | A copy travels by light |

Each hook is additive. The Vault can ship alone, and every other expansion
can ship without it.

---

## 43. APPENDIX AC — ENDING PROSE SKETCHES

**The Kept Word.** A disciplined vault with honest triage, real cards, and a
decade of saved things, each with a name and a date.

**The Reading Room.** The vault becomes a lending institution, and more people
read the stored books in one year than in all the years before.

**The Sealed Drawer.** The vault holds what it is trusted with, including what
nobody reads, and the trust itself becomes the shelter's most valuable holding.

**The Thousand Copies.** Culture lives in more than one place, and the fire
drill now includes the copy box, which is a sentence no archivist ever expected
to write.

**The Quiet Shelf.** The vault keeps fewer things better, and every card says
why, and the policy is short enough to read aloud.

**Fade.** A reading room with a lamp, a borrowed book at a table, a fiche
reader humming, and a register with a new line in careful ink.

---

## 44. APPENDIX AD — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Save-everything | nothing saved well | triage |
| Miracle repair | unreal | 350 permille relief |
| Single copy | fragile | fiche fan-out |
| Peeking seals | trust death | honor terms |
| Loot accession | ethics | provenance |
| Real texts | legal/tone | invention |
| Glass tomb | unreachable | loans and reading |
| Hidden loss | dishonest record | loss cards |
| Climate simulation | authority break | read-only cards |
| Salons as grind | fatigue | cooldowns |

The list exists because an archive is easy to write as either a hoard or a
museum. The expansion's rule is that a vault is a practice: cards, copies,
loans, and a reading in the hall once a year.

---

## 45. APPENDIX AE — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Documents | 40 | 6,000 |
| Accessions | 24 | 4,000 |
| Conservation cards | 12 | 2,500 |
| Climate cards | 12 | 2,000 |
| Fiche entries | 10 | 2,000 |
| Recordings | 24 | 4,000 |
| Salons | 16 | 3,000 |
| Exhibits and loans | 12 | 2,500 |
| Volumes | 10 | 2,500 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~59,500** |

---

## 46. APPENDIX AF — FIRST FIVE YEARS OF THE SHELF

| Year | Focus | Milestone |
|---|---|---|
| 1 | Register | first accessions |
| 2 | Copies | fiche fan-out |
| 3 | Voices | recordings consented |
| 4 | Rooms | salons and loans |
| 5 | Volumes | five years bound |

Five years is the minimum honest arc for an institution: a year of deciding
what it is for, a year of making sure nothing lives alone, a year of listening
to people, a year of opening the doors, and a year that proves the record is
continuous.

---

## 47. APPENDIX AG — VAULT COVENANT

| Clause | Promise |
|---|---|
| Clock | The shelf's remaining years are posted for anyone to read |
| Triage | We save what we can save well and record what we lose |
| Consent | No voice, letter, or name enters without its owner |
| Seal | A closed drawer is not opened, ever |
| Copy | Nothing unique stays unique |
| Card | Every treatment and loan is dated and signed |
| Climate | The room's weather is public |
| Loan | The school and the ward come before the display |
| Read | The year is read aloud by a rotating voice |
| Hand | Every shelf has a keeper and a successor |

The covenant is the expansion's first-class design object. It is written to be
taped inside the accession desk drawer, where the curator sees it before every
new line, and it is short because a promise that requires interpretation is not
a promise.

---

## 48. APPENDIX AH — SHELF KEEPER SUCCESSION TABLE

| Shelf | Keeper | Successor | Handover |
|---|---|---|---|
| A registers | Hesper | Ebba | one queue |
| B literature | Enid | reader | one volume |
| C technical | Nils | apprentice | one fiche |
| D music | Iris | choir lead | one session |
| E medical | Selby | ward aide | one loan |
| F children | Ebba | teacher | one hour |
| G civic | Kelso | apprentice | one card week |
| H fiction | Enid | reader | one shelf |
| I maps | Cleo tie | surveyor | one chart |
| J letters | Hesper | Ebba | one seal |
| K oversize | Selby | apprentice | one box |
| L years | Asta | reader | one reading |

Twelve shelves with keepers and successors, and the one handover that takes
longest is the sealed letters row, because a shelf whose whole point is not
being read requires the most careful transfer of trust in the building.

---

## 49. APPENDIX AI — PUBLIC READING TABLE

| # | Year | Reader | Place | Minutes | Attended |
|---|---|---|---|---|---|
| 1 | 1 | Hesper | market wall | 11 | 40 |
| 2 | 2 | Ebba | market wall | 9 | 55 |
| 3 | 3 | Asta | hall | 12 | 70 |
| 4 | 4 | Nils | market wall | 10 | 65 |
| 5 | 5 | child reader | garden | 8 | 80 |
| 6 | 6 | Iris | hall | 12 | 75 |
| 7 | 7 | Enid | market wall | 11 | 90 |
| 8 | 8 | apprentice | garden | 10 | 85 |
| 9 | 9 | rotating | hall | 13 | 100 |
| 10 | 10 | child reader | market wall | 9 | 110 |

Ten public readings with rotating readers, and the two child readers in the
table are the whole point: a year reading that a child performs is a culture
being handed forward in public, and the minutes stay short because the crowd
is standing at a market wall.

---

## 50. CLOSING STATEMENT

ASHFALL already models cultural loss with unusual honesty: a degradation clock
at two permille a day, a legibility limit, a lost threshold, restoration relief,
transcription, microfiche permanence, recordings by category, salons with a
real stress modifier, and a chronicle ledger. It has twelve tomes and an empty
register. The Vault fills the institution with practice: accessions with
consent, conservation with cards, copies in more than one place, recorded songs
and testimony with seals, salons that fill a hall, loans that reach a classroom
and a ward, and year volumes read aloud. It keeps no new save section, no new
authority, and no second library — only the daily work of making sure that when
the shelter's grandchildren ask what their grandparents knew, something is
still readable.

> Wave 8 note: this plan is one of five Wave 8 expansion bibles (47–51). Each is
> self-contained; none requires another to ship. The shared Wave 8 index lives
> at `docs/expansions/wave8/WAVE8_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `CulturalArchiveVaultSystem` (`ArchiveDocumentState`,
> `ArchiveProjectState`, `ArchiveRecordingState`, `SalonState`,
> `ArchiveChronicleEntry`, `CulturalArchiveVaultSave`,
> `RestorationReliefPermille = 350`, `LegibilityLimitPermille = 900`,
> `LostThresholdPermille = 1000`, `BaseDailyDegradationPermille = 2f`,
> `SalonDefaultDurationDays = 5`, `SalonCooldownDays = 10`, all live events),
> host wiring in `src/Main.FlagshipInstitutions.cs` (`EnsureCulturalArchive`,
> tome catalog loading, save store, event bindings), `ArchiveDeskSystem` for
> inks and transcription jobs under `archive_desk`, `CulturalArchiveSaveStore`
> under `cultural_archives`, and `cultural_archive_tomes.json` (9,530 B, 12
> tomes including the field surgery primer, children's primers, orchestral
> scores, and the civil defense guide).