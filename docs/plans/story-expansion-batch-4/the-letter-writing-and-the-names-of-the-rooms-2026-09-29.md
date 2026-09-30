# Feature / Task Plan: The Letter-Writing (correspondence carried between settlements) & The Names of the Rooms (the shelter's folk names)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 4, plans 4 of 4 (subjects 47–48).

> **Subjects covered (2 of the 8 in the "rhythm and circulation" batch):**
> 47. **The Letter-Writing** — correspondence carried between settlements: letters as objects
> with addresses, a post that exists only of travellers, and the out-tray nobody empties.
> (Prefix `LT`.)
> 48. **The Names of the Rooms** — the shelter's folk names: what the household actually calls
> each room, contested names, and one name that has fallen out of mouth but not out of format.
> (Prefix `NR`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `.ai/plans/radio-free-ashfall-2026-09-29.md` (the mailbag: letters to the station, "never
> evidence, only address"), `.ai/plans/the-fair-and-the-guest-book-2026-09-29.md` (guests carry
> things; the Guest Book's departures), `.ai/plans/living-region-2026-09-29.md` (graded news; the
> four unmapped vocabularies), `.ai/plans/the-mending-and-the-second-language-2026-09-29.md`
> (coinage; fading words), `.ai/plans/works-below-and-machine-in-the-walls-2026-09-29.md`
> (household nicknames — machines only).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Out-Tray and the Doorway

> *"A letter is an address with a weight. A room's name is a letter to the people who come
> after."*

There is no post in this world. There are travellers, and travellers carry things. A letter is
therefore a small act of faith with a stamp on it: written for a settlement days away, given to
someone going vaguely in that direction, and forgotten for a season. The shelter's letter book
records only what can be proved — to whom, carried by whom, delivered or not — because letters,
like guests, are addresses before they are messages. The out-tray by the door is where the
unanswered live: the letter to Iron Basin that has been in the tray since the thaw, weight and
address and nothing else.

The names of the rooms are letters of a different kind — correspondence with the future. The
plans and the stock list call the rooms what the before's drawings called them: north corridor,
clinic, bay 3. The household calls them something else: the warm room, the loud corner, the room
where the map is. Neither vocabulary is official. One is what the shelter *says* and one is what
the shelter *files*, and the plan gives the said one a table of its own.

**Tone & register.** Postal and domestic-geographic. The letter's vocabulary is *tray, carried,
delivered, unanswered, returned*; the rooms' is *name, folk name, contested, forgotten, doorway*.
Prose for the letters should read like a postmaster's ledger kept in a settlement with no post
office; prose for the rooms like a household's map drawn from memory — imprecise, affectionate,
argumentative.

**Mystery & texture.** Two silences hold the pair. In the out-tray there is one letter
**addressed to the shelter by name, in a hand of the before's school, dated before the shelter
existed** — recorded, weighed, undelivered (§12). And one room carries a folk name in every mouth
and every record *except* the format's: the table holds the official name and the name everyone
uses is in no row (§12). Neither is a puzzle. Both are the ordinary strangeness of places and
mails that outlast their authors.

**The second layer.** Names and letters are the two ways a household reaches beyond its walls:
names reach forward in time, letters reach sideways in space. Both fail the same way — the name
falls out of use, the letter falls out of the tray's bottom — and both leave traces that are
purely administrative, which is why they are so strange to read. The plan's quiet thesis is that
the shelter's two vocabularies, and its two out-trays, are the same institution: the ongoing
correspondence between this household and everything that is not this household.

---

## 1. Goal & Outcome

> *Design intent: the player should pick up a letter from the out-tray and feel the distance it
> has to cross — and walk into a room and hear the household name it without thinking.*

### 1.1 The Letter-Writing (LT)

- **Goal:** A **Letter Book** (additive nested DTO — no new save section) of *letters*: address
  (settlement id), writer-role (never a name — DEC-LT-03), placed-day, carrier-role (`guest`,
  `caravan`, `walker`), delivered-day or `returned`/`at_tray` status — **and no content field**
  (DEC-LT-01); one verb — **Send** (places a letter in the tray through ordinary inventory
  semantics: a letter is an item), and one derived **Out-Tray Read** (what waits, how long, at
  what weight). Carriage resolves through the existing caravan/guest seams as one optional hook
  per carrier, shipped dark.
- **Outcome (observable):** on a fixed seed, sending a letter to Iron Basin adds a letter row and
  places the item in the tray; a departing caravan's hook may carry it; the row resolves to
  `delivered` on the arrival the route owner already computes, or `returned` per an authored
  return rule; the out-tray read renders each waiting letter's address, weight and days-waiting;
  the anomalous letter renders its row in the same flat format as every other; with no letter
  rows every owner behaves identically to today; save/load mid-transit round-trips.
- **Non-Goals:** no message content, composition UI or reply generation; no post economy; no new
  news authority (Living Region owns news); no mailbag changes (RF owns its letters); no new save
  section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Names of the Rooms (NR)

- **Goal:** A **Room-Names Table** (≤ 24 rows: room id, folk name, `name_contested` flag with the
  second name, `name_forgotten` flag) surfaced beside the official names on the existing board/
  site surfaces; derived **Name State** (current / contested / forgotten); and one optional read
  hook: any surface naming a room may show "official — folk" in plain type. Naming is *fixed
  authorship* plus one constrained player verb — **Call** (choosing among authored alternates for
  contested rooms only; never free text — DEC-NR-02).
- **Outcome (observable):** on a fixed seed, every authored room renders its folk name beside the
  official one; a contested room renders both names in equal type until `Call` records a
  household choice, after which the choice renders first and the other name is *not removed*; the
  forgotten name renders in the table and in no mouth (its use-count is not modelled and no
  surface speaks it); with no rows every surface behaves identically to today; save/load
  round-trips.
- **Non-Goals:** no renaming of ids or catalog entries (names live beside ids, never over them);
  no map or cartography changes; no machine names (Machine in the Walls owns those); no new coinage
  (Second Language owns words); no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: a letter's address may render the
  destination's folk room-name at the far end ("to Iron Basin — the warm room there, if they have
  one") as one read-only string; and a room's name row may carry `written_by_letter` as a
  provenance tag — a name that arrived by post. One string each way, shipped dark.

---

## 1b. Texture, Mystery & Voice

**A letter is an address with a weight.**

The format's refusal of content (DEC-LT-01) is the plan's entire literary engine. Every letter is
*envelope-shaped*: who, from where, how long, how heavy. Prose must never peek inside — the
moment a letter's contents are known, the correspondence becomes a quest and the distance
collapses.

**Two vocabularies at every doorway.**

The room-names surface should render both names in plain type — "bay 3 — the warm room" — and
the prose should treat the pair as the shelter's constant low argument with its own floor plan.
The folk name always wins in speech; the official name always wins in files; the plan keeps both
winning.

**What the player is never told.**

- What the anomalous letter says or who wrote it. Its hand is of the before's school and its date
  is before the shelter; the format has no content field and the plan refuses to build one (§12).
- Why the out-tray is never emptied. `at_tray` is a status with no owner; the tray's persistence
  is not modelled and must not be.
- Who first called the warm room the warm room. Folk names have no authors (inherits MN's
  custom-provenance silence); the first mouth is not in the data.
- Whether the forgotten name is still true. `name_forgotten` marks a name out of use; whether the
  room it names is still what it was is not asserted.

**Voice — sample fragments (content candidates for `letter_lines.json` / `room_names.json`).**

> "Letter book: to Iron Basin, carried by caravan, placed day 61, delivered day 88. The book is
> proud of the 27 days and says nothing of the contents, which the book has never had a column
> for." — letter book (LT)

> "Out-tray: three waiting, one since the thaw. Weight, address, days. Three facts, and none of
> them is the interesting one." — letter book (LT)

> "The anomalous letter renders like every other: address, hand class, day, weight. The format
> does not know it is strange." — letter book (LT)

> "Room names: 'bay 3 — the warm room.' Official first, folk second, and everyone says the second
> one first." — board (NR)

> "Contested: the loud corner / the workshop. Both names in equal type until the household
> chooses, and the chooser must live with both." — board (NR)

**Design texture beats.**

- **No content field, ever** (DEC-LT-01). The envelope is the whole object; making it carry a
  message would turn a post into a prompt.
- **Names live beside ids, never over them** (DEC-NR-01). Ids are the file's language; folk names
  are the household's; overwriting either is vandalism.
- **Contested names resolve to a first place, not a deletion** (DEC-NR-03). The losing name stays
  printed — the argument is part of the record.
- **The forgotten name is rendered but never spoken.** No surface voice line may use it; its
  presence is the format remembering what the mouth forgot.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the tray leaves lying around.**

> "Letter, to Fen Crossing: weight 30g. Somebody has written 'light' on the envelope and the book
> has dutifully recorded the weight as 30g."

> "Returned letter, unopened: the address was correct and the season was not. The book records
> both facts and declines to arbitrate."

> "The anomalous letter's paper is different. The book records 'hand class: before.' The book's
> format is doing its best."

**What the doorways leave lying around.**

> "Name card, contested room: two names, one screw, the card turns. The card has been turned
> twice in the house's memory and both turns are remembered differently."

> "Name row, forgotten: kept in the table, used in no mouth. The row is the name's whole
> survival."

> "'The room where the map is.' The map has moved twice. The name has not." — texture only

**Held silences (texture, not register rows).**

- Whether the anomalous letter was ever meant to arrive. Its date precedes the shelter (§12); the
  postal fiction cannot explain it and the plan declines to let it. Texture only.
- Who turned the contested card and when. Two turns, two memories (§1c); the household's record
  of its own argument is deliberately as unreliable as arguments are.

---

## 1.4 Worked examples (non-normative)

**A letter's life (fixed seed).**

> Day 61 — `Send(letter_0042)`: address Iron Basin, writer-role `quartermaster`, carrier pending.
> The item sits in the tray; the out-tray read renders "Iron Basin, 30g, day 0 waiting".
>
> Day 74 — a guest's departure hook picks up `letter_0042`; the row reads `carrier: guest`.
> Day 88 — the settlement's arrival the route owner already computed resolves the row to
> `delivered`; the letter book records placed-day, carrier-role, delivered-day, and nothing else.
>
> Meanwhile `letter_0001` sits at the tray's bottom since the thaw: the anomalous letter, rendered
> in the same flat format — address, hand class: before, weight, days. It renders like every
> other letter, which is the point.

**A contested doorway (fixed seed).**

> The loud corner / the workshop: both names in equal type for eleven days. On day 83 the
> household's `Call` records the choice — "the workshop" renders first; "the loud corner" stays
> printed beside it in the same type. The forgotten name in row 14 renders in the table and in no
> voice line anywhere; its use is not modelled and the format simply remembers.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Letters-as-items can ride inventory semantics; the Guest Book's borrows and the Loan Shelf precedent transactions-as-rows. | batch-3/4 plans (GB-P1, LN-P1) | PROPOSED (plans are DRAFT) |
| E2 | Caravan/guest seams can expose one carriage hook (`TravelingCaravanSystem`, four authored caravans; guest departures). | `.ai/plans/convoy-wars-and-inside-a-house-2026-09-29.md`; `.ai/plans/the-fair-and-the-guest-book-2026-09-29.md` | PROPOSED (soft dependency) |
| E3 | RF mailbag semantics: letters drawn deterministically, "never evidence, only address" — this plan inherits that ethic for physical post and changes RF none. | `.ai/plans/radio-free-ashfall-2026-09-29.md` §1b | LIVE (plan is DRAFT) |
| E4 | Living Region owns news; settlements are authored ids (Iron Basin, Ash Flats, Fen Crossing appear in LR sample lines). | `.ai/plans/living-region-2026-09-29.md` §1b | PROPOSED (plan is DRAFT) |
| E5 | Room/site ids exist in two namespaces (`location_*`, `loc_*`); SN/RB finding; no interiors modelled. | `.ai/plans/second-nature-and-ruins-of-the-before-2026-09-29.md` | LIVE (per corpus) |
| E6 | Machine in the Walls owns *machine* nicknames only ("the nickname is not the machine's; it is a household's"). | `.ai/plans/works-below-and-machine-in-the-walls-2026-09-29.md` §1b | PROPOSED (plan is DRAFT) |
| E7 | Second Language owns coined *words*; room names are *names* and its register defers naming to cartography/GB silences. | `.ai/plans/the-mending-and-the-second-language-2026-09-29.md` §1 Non-Goals | PROPOSED (plan is DRAFT) |
| E8 | Whether a letter item can carry additive row state (carrier-role, status) in the door/visitor or inventory save owner without schema drift. | additive-DTO precedents; codec tests | **VERIFY (P0)** |
| E9 | Board/site surfaces can render "official — folk" name pairs at existing naming points. | batch-3/4 render precedents | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| News, truth grades | Living Region | nothing; a delivered letter may optionally cite a news line, never write one |
| Radio letters | RF mailbag | nothing; the two posts must never merge (DEC-LT-04) |
| Carriage | caravan / guest seams | one optional hook each; routing stays theirs |
| Names of things | ids and catalogs | names live beside ids; no id or catalog row is renamed |
| Machine names | Machine in the Walls | nothing; boundary only |
| Coined words | Second Language | nothing; names are not words (DEC-NR-04) |
| Letters | — | `LetterBook` (pure Core; row facts only) nested additively — **DEC-LT-02** |
| Room names | — | `RoomNames` (pure Core, closed table + validator) + name rows in data |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Social/LetterBook.cs` (new, pure), `Shelter/RoomNames.cs` (new, pure)
**Data:** `letter_rows.json`, `room_names.json`, `letter_lines.json`
**Host:** inventory host session (`INT`), caravan/guest carriage hooks (`INT`), board/site naming points (`INT`)
**Presentation:** an "Out-Tray" band beside the existing door surface; "official — folk" pairs at existing room-naming points — no new routed panel
**Tests:** `Ashfall.Core.Tests/Social/LetterBookTests.cs`, `Shelter/RoomNamesTests.cs`, `Ashfall.Core.Tests/Save/LetterRoomNamesSaveTests.cs`

## 5. Packages

### LT-P0 — Premise audit (Auditor; read-only): close E8–E9; confirm letter-item semantics and carriage-hook wording; confirm the settlement-id vocabulary with the Living Region owner.
### LT-P1 — Letter book + rows (Core + data): facts-only format, tray statuses, return rule, validator row-level. **Accept:** no content field exists anywhere in the diff (§6.3); determinism; round-trip.
### LT-P2 — Send verb + out-tray band (host): ordinary transactions; carriage hooks dark. **Accept:** conservation; the anomalous letter renders flat like all others.
### NR-P1 — Room-names table + name state (Core + data): ≤ 24 rows, contested and forgotten flags, alternates-only `Call`. **Accept:** no id renamed; forgotten names render but never speak (§6.4).
### NR-P2 — Naming points + `Call` verb (host): plain pairs; constrained choice. **Accept:** contested resolution never deletes the losing name.
### X-P1 — Seam hooks (host, dark): far-end name string; `written_by_letter` provenance. **Accept:** one string each way; no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no letter rows and no name rows → inventory, news, radio, caravan and board outputs identical on a saved corpus.
3. Envelope invariant: the diff contains no content, body or message field for letters anywhere (DEC-LT-01).
4. Voice invariant: no surface voice line may speak a `name_forgotten` row (DEC-NR-05); names never overwrite ids (DEC-NR-01).
5. Conservation: letter items balance through the inventory owner; carriage resolves exactly once per row (E8 closed at P0).
6. Determinism: identical letter rows, tray renders and name states on replay (`CampaignStreamIds` fork; never `System.Random`).
7. Save round-trip mid-transit; old saves load; checksum-safe (E8).
8. The registered silences of RF, Living Region, Machine in the Walls and Second Language stay unanswered (§7).

## 7. Cross-plan boundaries
- **Radio Free Ashfall:** the mailbag is its letters-by-air; this is post-by-foot. Two posts, two ledgers, never merged (DEC-LT-04).
- **The Guest Book / The Fair (batch 3):** guests carry letters; the Guest Book's departures remain its own record. The Fair's rumour lines are speech, not post.
- **Living Region:** news is its authority; a delivered letter may cite a line and never writes one. The settlement ids are its vocabulary.
- **Machine in the Walls / Second Language (batch 3):** machine nicknames and coined words are theirs; room names are names, and the plan keeps all three ledgers apart.
- **The Cabinet & The Dig (batch 3):** an envelope is not an exhibit unless the Cabinet's owner says so; display is its verb.
- **The Wardrobe / Night Shift (batch 4):** the night book's lines may cite a room by its folk name; no state is shared.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-LT-01 | Letters are envelopes: address, roles, dates, weight — and no content field, ever. | tone/rule | Yes |
| DEC-LT-02 | The letter book stores row facts only and nests additively; no new save section. | architecture | Yes; confirm in P0 |
| DEC-LT-03 | Writer and carrier are roles, never names. | tone | Yes |
| DEC-LT-04 | Physical post and the mailbag are two posts; they must never merge. | boundary | Yes |
| DEC-NR-01 | Names live beside ids; no id or catalog row is ever renamed. | rule | Yes |
| DEC-NR-02 | `Call` chooses among authored alternates only; free text is forbidden. | rule | Yes |
| DEC-NR-03 | Contested resolution reorders, never deletes; the losing name stays printed. | tone | Yes |
| DEC-NR-04 | Room names are names, not words: no coinage rules apply and none are borrowed. | boundary | Yes |
| DEC-NR-05 | Forgotten names render in the table and in no voice line; the format remembers what the mouth forgot. | tone | Yes — **needs canon note** |
| DEC-X-01 | Both seam hooks ship dark; one string each way. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Letter`, `Post`, `RoomName`, `FolkName`, `Nickname`)
- [ ] Premise re-verified (Rule 7); RF, LR, MW and SL registers re-read; their silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-LT-01, DEC-NR-01, DEC-NR-05)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing inventory, guest and board tests (list from P0 selector)
- [ ] Inventory/carriage host selftests with letter rows on and off (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: any surface would require a letter content field; a forgotten name cannot be kept out of voice lines by construction; `Call` would need free text; carriage would read beyond the documented hook fields; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the tray deeper than its letters and the house taller than its floor plan.
Any future plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| LT-OM-1 | What does the anomalous letter say? | The format has no content field and the plan refuses to build one (DEC-LT-01); its date precedes the shelter and the postal fiction cannot explain it. | Never — deliberately sealed. |
| LT-OM-2 | Who wrote it, in the before's hand? | Hand class is recorded; the writer is not (§12). Naming one converts an envelope into a ghost story. | Canon owner only, as a signed decision. |
| LT-OM-3 | Why is the out-tray never emptied? | `at_tray` is a status with no owner; the tray's persistence is not modelled and must not be. | Never — a rule, not a gap. |
| LT-OM-4 | Do letters ever arrive for the shelter? | The book records letters *placed*; whether the far settlements write back is not asserted and no inbound mechanic exists. | A signed content pass, if the post is ever widened. |
| LT-OM-5 | What did "light" mean on the 30g envelope? | Texture (§1c); the book dutifully records 30g and the annotation is a human note the format declines to weigh. | Never — texture by omission. |
| NR-OM-1 | Who first called the warm room the warm room? | Folk names have no authors (inherits MN's provenance silence); the first mouth is not in the data. | Never — deliberately sealed. |
| NR-OM-2 | What is the forgotten name still true of? | `name_forgotten` marks disuse, not falsehood (§12); whether the room changed or the word did is not asserted. | Never — tone-locked by DEC-NR-05. |
| NR-OM-3 | Who turned the contested card, and when? | Two turns, two memories (§1c); the household's record of its own argument is kept as unreliable as arguments are. | Never — texture by omission. |
| NR-OM-4 | Are there folk names for things that are not rooms? | The table is rooms only; whether the house names its tools, corners or hours is unmodeled and reserved to other registers. | The queued batch-4 siblings' owners, if ever linked. |
| NR-OM-5 | Does the name that arrived by post have a sender? | `written_by_letter` is provenance without a postmark (§1.3); the pair with LT-OM-2 must stay unresolved together (mystery-index §3 discipline). | Never — the pair stays open. |
