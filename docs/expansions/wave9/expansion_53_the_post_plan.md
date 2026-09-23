# ASHFALL — Expansion 53 Design Bible
# THE POST
### Wave 9 · Letters, Sorting, Postboxes, Couriers, Pneumatic Tubes, the Dead-Letter Shelf, and the Ethics of Withholding

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Narrative` (`LetterDeliverySystem`, `SurvivorLetterDeliverySystem`), `Ashfall.Core.Survivors` (`NeedsSystem` morale path), `Ashfall.Core.Inventory`
**Proposed host owner:** `PostOfficeHostSession` (new host over the two live letter systems; additive save ownership proposed inside the `narrative` section)
**Existing save sections:** none dedicated (confirmed: no `letter_delivery` section exists; the two Core systems are unwired)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no postal-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has two letter systems. `LetterDeliverySystem` (`SystemId` =
"letter_delivery_system") defines `LetterDeliveryState`, `LetterDeliveryRecord`
(`letterId`, `foundDay`, `resolvedDay`, `recipientSurvivorId`,
`resolutionNotes`, `moraleDeltaApplied`), and a record list state, with the API
`AddressLetter(letterId, recipientSurvivorId, day)`,
`DeliverLetter(letterId, day, notes, customMoraleDelta = 6.0f)`,
`WithholdLetter(letterId, day, notes)`, `MarkUnanswered(letterId, day, notes)`,
and `RestoreState`. `SurvivorLetterDeliverySystem` (`SystemId` =
"survivor_letter_delivery") defines the canonical lifecycle constants
`not_found`, `found`, `addressed`, `delivered`, `withheld`, and `unanswered`,
a `SurvivorLetterRecordState` (`letter_id`, `delivery_state`,
`matched_survivor_id`, `found_day`, `resolved_day`, `morale_delta_applied`), a
`SurvivorRef` (`SurvivorId`, `Name`, `Role`, `IsAlive`), and
`DefaultDeliveryMoraleBonus = 8f`. The narrative data is rich:
`narrative/letters_expansion.json`, `narrative/survivor_letters_lost_kin.json`,
`narrative/unsent_letters_batch_2.json`, and
`narrative/pneumatic_carrier_capsule_logs.json`.

What does not exist: the post office. Both Core systems are **unwired** — no
host session references them and no save section registers them, so letters
cannot be found, addressed, delivered, withheld, or persisted in the running
game. There are no sorting rules, no addresses, no postboxes, no routes, no
pouches, no courier shifts, no pneumatic network, no dead-letter shelf, no
receipts, no letter-writing content, and no policy about what a shelter may do
with a letter it is afraid to deliver.

**The Post** wires the letters that already exist into a service: a sorting
room, an address system, courier rounds, internal tubes, a dead-letter shelf
with dignity, and a withholding board with strict ethics. It extends the two
live systems and never duplicates a radio, records, or vault authority.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| `LetterDeliverySystem` | Letter records and states | Wires and extends it |
| `SurvivorLetterDeliverySystem` | Survivor match and morale bonus | Wires and extends it |
| `NeedsSystem` | Morale and stress | Uses `Modify` only |
| 49 The Mirror (Wave 8) | Light messages | Carries paper; the mirror carries flashes |
| Radio family | Broadcasts and distress | Letters are private; radio stays untouched |
| 34 The Long Road (Wave 5) | Routes and convoys | Couriers ride routes; never own them |
| 44 The Outpost (Wave 7) | Remote sites | Mail moves between sites; sites stay theirs |
| 50 The Vault (Wave 8) | Preserved culture | Personal letters stay personal; the vault takes only consented family archives |
| 24 The Long Goodbye (Wave 3) | Memory and mourning | Letters from the dead route through memory owners |
| 30 The Press (Wave 4) | Paper and printing | Paper, envelopes, and forms are print jobs |
| 03 The Standing Record | Records | Ledgers and receipts file there |
| `DutyRoster` (Exp 02) | Shifts | Courier shifts ride the roster |
| 55 The Quarter (Wave 9) | Neighbors and disputes | Mail is the neutral service; no gossip authority |
| 03/20 Espionage | Intelligence | No interception content exists |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

There are letters in the bunker. Some were written before the doors closed,
some were never sent, and one has been waiting in a dead drop for six years
for a person who may be two hundred meters away or may be dead. Nobody has
sorted them, nobody has delivered them, and nobody has decided what a shelter
is allowed to do with a letter it knows will hurt someone.

**The Post** is the expansion about mail: a window, a pigeonhole wall, an
address board, foot routes and convoy pouches, pneumatic tubes that carry a
canister from the kitchen to the clinic, a dead-letter shelf that keeps what
cannot be delivered, and a withholding board that has rules. It is the
expansion about the oldest morale system there is: a piece of paper with your
name on it.

### 1.2 The five loops it adds

```
  Collect ──► Sort ──► Address ──► Carry ──► Deliver
      │         │         │          │          │
      ▼         ▼         ▼          ▼          ▼
   Bunker    boxes,   boards,    pouches,   doors,
   finds     routes   names      tubes      receipts
                                        │
                                        ▼
                          Hold ──► Review ──► Return
```

### 1.3 What the player manages

1. **Collection.** Letters found, letters written, letters handed in.
2. **Sorting.** Pigeonholes, routes, priorities, and misdirects.
3. **Addressing.** Names, quarters, sites, and the address board.
4. **Couriers.** Round walkers, convoy pouches, and courier shifts.
5. **Pneumatics.** Tubes, carriers, station boxes, and blockages.
6. **Delivery.** Doors, receipts, and the moment a name is called.
7. **Withholding.** The board, its rules, its reviews, and its timer.
8. **Dead letters.** Undeliverable, unclaimed, returned, and kept.
9. **Writing.** Paper, evenings, replies, and unopened letters.
10. **Records.** Ledgers, receipts, routes, and delivery figures.

### 1.4 What it is not

- Not a radio or mirror system; paper is physical and slow.
- Not an intelligence network; nobody opens letters that are not theirs.
- Not a vault; private letters stay private, and family archives need consent.
- Not a courier-combat system; routes are ordinary and dangerous only in
  honest, existing ways.
- Not a gossip system; the post does not carry rumor.
- Not a currency; stamps are marks of origin, not money.
- Not a new save section on its own; additive inside existing envelopes.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Narrative/LetterDeliverySystem.cs` | Letter records and states | `LIVE`, unwired |
| `Assets/Ashfall.Core/Narrative/SurvivorLetterDeliverySystem.cs` | Address match and morale | `LIVE`, unwired |
| `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` | Morale sink | `LIVE` |
| `src/Host/` | Host sessions | **no letter host exists** |
| `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` | Save sections | **no letter section exists** |
| `src/UI/` | Panels | **no mail panel exists** |

### 2.2 Live data (counted)

| Catalog | Location | Notes |
|---|---|---|
| `letters_expansion.json` | narrative | letter corpus |
| `survivor_letters_lost_kin.json` | narrative | lost-kin letters |
| `unsent_letters_batch_2.json` | narrative | unsent letters |
| `pneumatic_carrier_capsule_logs.json` | narrative | tube flavor |
| Post office, route, box catalogs | absent | confirmed none |
| Withholding policy and dead-letter shelves | absent | confirmed none |

### 2.3 Confirmed gaps

- **GAP-53-1 — Both letter systems are unwired; no host, panel, or save.**
- **GAP-53-2 — No sorting, pigeonhole, or route content.**
- **GAP-53-3 — No address system or board.**
- **GAP-53-4 — No postboxes or delivery points.**
- **GAP-53-5 — No courier rounds, pouches, or shifts.**
- **GAP-53-6 — No pneumatic tube network content.**
- **GAP-53-7 — No withholding policy, review, or timer.**
- **GAP-53-8 — No dead-letter shelf practice.**
- **GAP-53-9 — No letter-writing or reply content.**
- **GAP-53-10 — No ledger, receipt, or delivery record content.**

### 2.4 Non-duplication statement

This expansion will **not** add a second radio, mirror, records, vault, memory,
or morale system. It wires and extends the two live letter systems, routes
morale through `NeedsSystem.Modify`, moves paper through the press owner, rides
routes owned by the road system, files ledgers with `StandingRecord`, and
returns letters from the dead through the memory owners. New state is additive
inside the `narrative` save envelope. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Mail is a right.** Every resident can send and receive, and
nobody is charged for it.

**Pillar 2 — Slowness is honest.** A letter takes the time the road takes, and
the waiting is part of the message.

**Pillar 3 — Nobody opens another's letter.** Seals are respected without
exception by the service itself.

**Pillar 4 — Withholding is a burden, not a power.** Holding a letter requires
two reviewers, a stated reason, a timer, and a plan to deliver.

**Pillar 5 — The undeliverable are kept.** A dead letter is a person's words
waiting for a name, and it is stored with dignity, never destroyed.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Sorting | Quiet work, pigeonholes | Bustling comedy |
| Delivery | A name called, a door | Dramatic reveals |
| Withholding | Ethics, timers, reviews | Censorship power fantasy |
| Dead letters | Shelves and hopeful indexes | Graveyard imagery |
| Couriers | Weather, distance, receipts | Action-adventure |
| Letters | Voices, handwriting, news | Twist factories |
| Tubes | Thunk, suction, blockages | Fantasy teleport |
| Writing | Encouraged, supplied, slow | Productivity targets |

### 3.3 Content limits

- No opening sealed letters for drama; only the named recipient may open.
- No interception, surveillance, or intelligence content.
- No torture, coercion, or blackmail through letters.
- No real-world postal services, stamps, or addresses copied.
- No gossip engine; the post is not a rumor mill.
- No letter that orders violence; words carry consequences through existing
  owners only.
- No new save section.

---

## 4. THE POST WORLD

### 4.1 Interior rooms

- **`room_post_office`** — the window, the counter, and the seal jar.
- **`room_sorting_room`** — the pigeonhole wall and the route board.
- **`room_dead_letter_shelf`** — the shelf, its index, and its lamp.
- **`room_withholding_room`** — two chairs, a drawer, and the timer board.
- **`room_tube_plant`** — the compressor, the switches, and the carriers.
- **`room_letter_room`** — tables, paper, and quiet.
- **`room_address_board`** — the board of names and quarters.
- **`room_pouch_store`** — bags, boxes, and route cards.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_post_box_row` | The Box Row | 2 | Public postboxes |
| `loc_route_gate` | The Route Gate | 3 | Couriers depart |
| `loc_waystation_post` | The Waystation Post | 3 | Mail relay on the road |
| `loc_outpost_mail` | The Outpost Window | 3 | Remote mail |
| `loc_convoy_pouch` | The Convoy Stand | 3 | Pouches onto convoys |
| `loc_lost_drop` | The Lost Drop | 4 | Where the six-year letter waited |
| `loc_letter_tree` | The Letter Tree | 2 | Letters left for passers |
| `loc_tube_exit_clinic` | The Clinic Box | 2 | Pneumatic destination |
| `loc_tube_exit_kitchen` | The Kitchen Box | 2 | Pneumatic destination |
| `loc_mail_stone` | The Mail Stone | 2 | Where route milestones are cut |

All locations require valid item or map-node references and scanner registration.

### 4.3 The rhythm

Sorting every morning, courier rounds weekly, convoy pouches with departures,
tube carriers all day, the withholding board when a case appears, and the
dead-letter index reviewed each season. The expansion's clock is the round.

---

## 5. MAIN STORYLINE — "WHAT THE MAIL CARRIES"

### 5.1 Central conflict

**Sorrel Nib** has been the shelter's informal letter keeper for years and has
a drawer of letters she has never had the authority to deliver. When the post
office opens with a proper window and a pigeonhole wall, the first week
produces three problems the drawer had been hiding: a letter addressed to a
resident from a sender who died in the first winter, a letter sealed and
addressed to nobody at all, and a letter for a person who is alive, present,
and better off not knowing what it says.

**Ysolda Rell** chairs the withholding board and writes its rules in one
afternoon: two reviewers, a stated reason, a timer, and an obligation to
deliver or return. **Elda Prine** wants courier rounds to the outposts and the
waystations, because a post office that only serves the bunker is a cupboard
with a sign. **Mern Hark** wants the pneumatic tubes working, because the
clinic and the kitchen are four hundred meters apart and a canister beats a
runner. **Ume Sallow** keeps the dead-letter shelf and believes that words
waiting for a name deserve a lamp and an index.

The expansion's question: **when is keeping a letter from someone the same as
lying to them?**

### 5.2 Theme (unspoken)

**A letter is a person's voice with a stamp on it.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_postmaster_sorrel_nib` | Sorrel Nib | Postmaster | The service and its rules |
| `npc_sorter_quilla_taffy` | Quilla Taffy | Sorter | Pigeonholes and routes |
| `npc_courier_elda_prine` | Elda Prine | Courier lead | Rounds and receipts |
| `npc_tubes_mern_hark` | Mern Hark | Pneumatics | Tubes and carriers |
| `npc_dead_letter_ume_sallow` | Ume Sallow | Dead letters | Shelf and index |
| `npc_walker_norr_wicker` | Norr Wicker | Route walker | Weather and distance |
| `npc_withholding_ysolda_rell` | Ysolda Rell | Board chair | Ethics and timers |
| `npc_apprentice_lisk_amble` | Lisk Amble | Apprentice | Rounds and ledgers |

### 5.4 Story beats (15)

1. **The Drawer.** The old letters are counted and logged.
2. **The Window.** The post office opens.
3. **The First Delivery.** A letter reaches its person.
4. **The Board.** Addresses and names are fixed.
5. **The Round.** The first courier walk to the waystation.
6. **The Tubes.** The pneumatic line opens with a thunk.
7. **The Shelf.** The dead-letter index begins.
8. **The Case.** The board hears its first withholding request.
9. **The Timer.** A held letter reaches its deadline.
10. **The Reply.** The shelter answers a stray letter.
11. **The Outpost.** Mail moves between sites.
12. **The Lost Kin.** A letter for someone presumed dead finds a reader.
13. **The Evening.** Letter-writing becomes an ordinary night.
14. **The Round Alone.** Lisk carries the pouch alone, supervised from afar.
15. **What the Mail Carries.** The post becomes ordinary.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Service | universal / limited / bunker-only | openness |
| Rounds | weekly / fortnightly / on departures | speed vs. cost |
| Withholding | two-reviewer / one-senior / never | ethics |
| Dead letters | kept / returned / published | dignity |
| Tubes | full network / key links / none | convenience |
| Lost kin | search / notify / wait | hope vs. pain |
| Writing supplies | unrestricted / rationed / none | culture |
| Final | public post / quiet service / archive of letters | identity |

### 5.6 Endings (5 + fade)

1. **The Open Mail** — everyone sends and receives, rounds run weekly, and
   letter day is a real day in the shelter's week.
2. **The Held Hand** — the withholding board matures into a trusted practice,
   and every held letter is eventually delivered or returned, with reasons.
3. **The Long Round** — the courier network reaches every waystation and
   outpost, and the post becomes the valley's slow nervous system.
4. **The Shelf of Names** — the dead-letter index grows, each entry lamp-lit
   and hopeful, and a stranger walking in twenty years can find their
   grandmother's words.
5. **The Written Year** — the shelter writes: replies, complaints, thanks,
   recipes, and letters home, and the post office has a waiting list for pens.
6. **Fade** — a pigeonhole wall in the morning light, a name called, a door
   opening, and a receipt signed with a thumbprint.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_post_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_post_drawer`, `quest_post_window`, `quest_post_first_delivery`,
`quest_post_board`, `quest_post_round`, `quest_post_tubes`, `quest_post_shelf`,
`quest_post_case`, `quest_post_timer`, `quest_post_reply`, `quest_post_outpost`,
`quest_post_lost_kin`, `quest_post_evening`, `quest_post_round_alone`,
`quest_post_what_mail_carries`.

### 6.2 Side quests (30)

**Collection and sorting (5)**
- `quest_post_collect` — letters collected
- `quest_post_sort` — sorted correctly
- `quest_post_misdirect` — misdirect fixed
- `quest_post_priority` — urgent mail marked
- `quest_post_unclaimed` — unclaimed checked

**Addresses (5)**
- `quest_post_name_board` — names posted
- `quest_post_forward` — forwarding set
- `quest_post_ambiguous` — ambiguous address solved
- `quest_post_letters_for_children` — child mail
- `quest_post_initials` — initials book

**Couriers (5)**
- `quest_post_pouch` — pouch packed
- `quest_post_round_paid` — receipts kept
- `quest_post_weather` — weather hold
- `quest_post_two_walkers` — pair rule
- `quest_post_waystation` — relay cache

**Tubes (5)**
- `quest_post_tube_build` — tube run built
- `quest_post_carrier` — carriers made
- `quest_post_blockage` — blockage cleared
- `quest_post_pressure` — compressor serviced
- `quest_post_switch` — switches labelled

**Withholding and dead letters (5)**
- `quest_post_review` — review held
- `quest_post_deliver_held` — held letter delivered
- `quest_post_return_held` — held letter returned
- `quest_post_shelf_lamp` — shelf lit
- `quest_post_index` — index written

**Writing (5)**
- `quest_post_paper` — paper stocked
- `quest_post_evening_hold` — writing evening
- `quest_post_answer_stranger` — stranger answered
- `quest_post_keepsake` — keepsake letter
- `quest_post_seal` — seals made

### 6.3 Repeatable quests (8)

`quest_post_repeat_sort`, `quest_post_repeat_round`,
`quest_post_repeat_pouch`, `quest_post_repeat_tube`,
`quest_post_repeat_index`, `quest_post_repeat_paper`,
`quest_post_repeat_receipt`, `quest_post_repeat_review`.

### 6.4 Dynamic hooks

Live events (letters found, survivor deaths and births, outpost departures,
route conditions, weather, booth/panel actions, morale changes) attach
authored follow-ups through the existing letter systems and host adapters. No
new event bus.

### 6.5 Constraints

- Letter records and states stay with the two live letter systems.
- Morale routes through `NeedsSystem.Modify` only.
- Routes, convoys, and weather stay with their owners.
- Paper and forms come from the press owner.
- Ledgers file with `StandingRecord`.
- Letters from the dead route through the memory owners.
- No espionage, interception, or surveillance content.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `PostOfficeSystem` (new, `Ashfall.Core.Narrative`)

**Owns:** the service: window hours, sorting rounds, pigeonholes, priorities,
and counters. **Consumes:** the two live letter systems, `DutyRoster`.
**Data:** `post_offices.json`. **Rules:** the window is open on schedule; every
letter has a state; the office never opens a sealed letter; service is free.

### 7.2 `AddressSystem` (new, thin, `Ashfall.Core.Narrative`)

**Owns:** names, quarters, site directions, forwarding, initials, and the
address board. **Consumes:** survivor roster, outpost roster. **Data:**
`post_forms.json`, `post_boxes.json`. **Rules:** ambiguous addresses go to the
board, never to guesswork; a person who moves is forwarded; children may have
letters and initials entries like anyone else.

### 7.3 `CourierSystem` (new, `Ashfall.Core.Narrative`)

**Owns:** rounds, pouches, pair rules, receipts, relay caches, and weather
holds. **Consumes:** route owner, weather, duty roster. **Data:**
`post_routes.json`, `post_couriers.json`. **Rules:** a round is carried by a
person, not a system; two walkers on hazard routes; weather holds are honored;
every pouch has a receipt chain.

### 7.4 `PneumaticSystem` (new, thin, `Ashfall.Core.Narrative`)

**Owns:** tube runs, carriers, compressor state, switches, and blockages.
**Consumes:** shelter rooms and workshop for builds. **Data:**
`post_pneumatic.json`. **Rules:** tubes move canisters between fixed stations;
a blockage is cleared physically; the compressor is a small machine with
maintenance; tubes never carry anything alive.

### 7.5 `WithholdingBoardSystem` (new, `Ashfall.Core.Narrative`)

**Owns:** the board, its two-reviewer rule, timers, reasons, delivery
obligations, and returns. **Consumes:** the live `WithholdLetter` path.
**Data:** `post_withholding.json`. **Rules:** any letter may be requested for
holding, never silently; two reviewers sign; the reason is recorded; a timer
forces delivery or return within the authored window; the sender's words are
never altered or destroyed; sealed letters remain sealed regardless of review.

### 7.6 `DeadLetterSystem` (new, thin, `Ashfall.Core.Narrative`)

**Owns:** the shelf, index, search, claim, return, and the seasonal review.
**Consumes:** unmatched letters, survivor roster, memory owners. **Data:**
`post_dead_letters.json`. **Rules:** dead letters are kept, indexed by
addressee, and searchable forever; a claim by kin is honored; nothing is
published without consent.

### 7.7 `LetterWritingSystem` (new, thin, `Ashfall.Core.Narrative`)

**Owns:** paper stock, writing evenings, reply prompts, keepsakes, and seals.
**Consumes:** press owner, `NeedsSystem` for the morale of writing and
receiving. **Data:** `post_letters_catalog.json`. **Rules:** supplies are free
where stock allows; writing is encouraged, never assigned; replies to strangers
are optional and celebrated.

### 7.8 `PostRecordSystem` (new, thin, `Ashfall.Core.Narrative`)

**Owns:** ledgers, receipts, route logs, and delivery figures. **Data:**
`post_receipts.json`. Records through `StandingRecord`. **Rules:** every
delivery is receipted; statistics are service health, never a ranking of
couriers; ledgers record delays honestly.

### 7.9 Systems explicitly not added

- No second radio, mirror, records, vault, memory, or morale system.
- No interception, surveillance, or intelligence content.
- No currency, postage pricing, or stamp economy.
- No gossip or rumor engine.
- No letter-opening drama; seals are sacred.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `post_offices.json` (new)

```json
{
  "schema_version": 1,
  "offices": [
    {
      "office_id": "post_office_shelter",
      "display_name": "Shelter Post Office",
      "window_hours": "06:00-20:00",
      "pigeonholes": 24,
      "boxes": ["box_dorm_a", "box_workshop", "box_clinic"],
      "staff": 2,
      "tags": ["main"]
    }
  ]
}
```

### 8.2 `post_routes.json` (new)

Routes: route id, end points, walk hours, hazards, weather holds, relay cache.

### 8.3 `post_pneumatic.json` (new)

Pneumatics: run id, stations, length, carrier type, compressor load, blockage.

### 8.4 `post_boxes.json` (new)

Boxes: box id, room or site, capacity, key holder, check cadence.

### 8.5 `post_withholding.json` (new)

Policy: request reasons, reviewer count, timer days, delivery obligation,
return rule, sealed rule.

### 8.6 `post_dead_letters.json` (new)

Shelf: letter id, addressee, found day, search attempts, claim status, index.

### 8.7 `post_letters_catalog.json` (new)

Letter corpus additions: id, voice, subject, length, tags, state default.

### 8.8 `post_couriers.json` (new)

Couriers: role, round, pair rule, load, training path, weather rule.

### 8.9 `post_receipts.json` (new)

Receipts: receipt id, pouch, courier, out day, in day, condition, notes.

### 8.10 `post_forms.json` (new)

Forms: form id, purpose, fields, print ref, seal requirement.

### 8.11 Items

New items appended to `items.json`: `item_post_box_small`,
`item_letter_paper`, `item_envelope`, `item_wax_seal`, `item_sorting_board`,
`item_courier_pouch`, `item_pneumatic_carrier`, `item_pneumatic_tube`,
`item_postmark_stamp`, `item_delivery_receipt`, `item_address_board`,
`item_ink_pad`, `item_letter_knife`, `item_registry_book`,
`item_dead_letter_index`, `item_writing_pen`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

The two live letter systems keep their own record state. New sub-objects
(offices, routes, boxes, withholding cases, shelf entries, receipts, writing
supplies) are additive inside the `narrative` save envelope, which already
owns story state; the plan recommends an additive `letters` sub-object there
and flags a dedicated section as an open decision for the foreman. No existing
save authority is rewritten.

### 9.2 State to persist

- Letters with states, recipients, days, and morale applied.
- Sorting state and pigeonhole occupancy.
- Address board and forwarding entries.
- Routes, rounds, pouches, and receipt chains.
- Pneumatic runs, carrier positions, and compressor state.
- Withholding cases with reviewers, reasons, and timers.
- Dead-letter shelf entries and search history.
- Writing supplies and evening records.

### 9.3 Determinism

- Letter discovery and addressing use authored content and roster matching.
- Morale applies once per letter through the live path.
- Round timing derives from route length and weather windows.
- Withholding timers use campaign days.
- Pneumatic travel is a day-tick, not wall-clock.
- Paired replay hashes must match; no `System.Random`.

### 9.4 Migration

Legacy saves load with no letters in flight; the drawer's authored letters
appear as `found` on first service open, respecting any prior state if the
narrative section already recorded one. A letter delivered before the service
exists keeps its delivered state.

### 9.5 Checksum

Invariant-culture floats; integer day and count fields.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `PostOfficePanel` (new) | Window, sorting, delivery | `PostOfficeHostSession` |
| `AddressBoardPanel` (new) | Names and forwarding | same |
| `CourierPanel` (new) | Rounds and receipts | same |
| `PneumaticPanel` (new) | Tubes and carriers | same |
| `WithholdingPanel` (new) | Cases and timers | same |
| `DeadLetterPanel` (new) | Shelf and index | same |
| `WritingPanel` (new) | Supplies and evenings | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- A sealed letter is shown as sealed; no panel can preview it.
- Withholding screens state the rule, the reviewers, the timer, and the
  obligation in plain language before any decision.
- Dead-letter entries are searchable by name and shown with dignity, never as
  a failure counter.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- No timed interactions; letters wait.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a pigeonhole slot, a wax seal
pressed, a canister thunking through a tube, a door knock, a pen on paper, a
shelf lamp switched on. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `LetterDeliverySystem` | Records and states |
| `SurvivorLetterDeliverySystem` | Match, morale, lifecycle |
| `NeedsSystem` | Morale via `Modify` |
| `PressHostSession` (Wave 4) | Paper, envelopes, forms |
| `RouteInfrastructureSystem` (Wave 5) | Courier routes |
| `WeatherSystem` (Wave 5) | Weather holds |
| 44 The Outpost (Wave 7) | Site mail |
| 49 The Mirror (Wave 8) | Flash notice of a pouch |
| `DutyRoster` (Exp 02) | Courier shifts |
| 50 The Vault (Wave 8) | Consented family archives only |
| `MemorialSystem` (Wave 3) | Letters from the dead |
| 12 The Second Generation | Children's letters |
| `StandingRecord` (Exp 03) | Ledgers and receipts |
| `JournalSystem` | Letter entries in journals |
| `EpilogueChronicleBuilder` | Post history lines |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm both letter systems, narrative data,
press, route, weather, roster, memory, and record owners. Record file:line;
change nothing.

**Phase 1 — Data + validators.** Author the ten catalogs; register validators
and scanner; register the letters corpus with the scanner.

**Phase 2 — Pure Core.** `PostOfficeSystem`, `AddressSystem`, `CourierSystem`,
`PneumaticSystem`, `WithholdingBoardSystem`, `DeadLetterSystem`,
`LetterWritingSystem`, `PostRecordSystem`.

**Phase 3 — Persistence.** Wire both letter systems into the save envelope,
additive sub-object, migration, round-trip, determinism.

**Phase 4 — Host + CLI.** `PostOfficeHostSession`, focused selftest coverage,
fresh journey from the drawer to the shelf.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Year-long soak: rounds, withheld cases, dead letters,
write-backs, and morale effects.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Offices | 4 |
| Routes | 8 |
| Pneumatic runs | 6 |
| Postboxes | 12 |
| Withholding reasons | 10 |
| Dead-letter entries | 20 |
| Letter corpus additions | 40 |
| Courier roles | 8 |
| Receipts | 24 |
| Forms | 12 |
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
| Morale duplication | Critical | `NeedsSystem` only |
| Espionage drift | High | No interception by contract |
| Vault overlap | Medium | Consent-only archives |
| Against-consent reveals | Critical | Sealed-letter rule |
| Radio overlap | Medium | Paper vs. broadcast boundary |
| Withholding abuse | High | Two reviewers and timers |
| Dead-letter grimness | Medium | Lamp and index framing |
| Determinism break | Low | Campaign day ticks |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `post_offices.json` | 4 | 1,000 |
| `post_routes.json` | 8 | 2,500 |
| `post_pneumatic.json` | 6 | 2,000 |
| `post_boxes.json` | 12 | 2,000 |
| `post_withholding.json` | 10 | 3,000 |
| `post_dead_letters.json` | 20 | 4,000 |
| `post_letters_catalog.json` | 40 | 8,000 |
| `post_couriers.json` | 8 | 2,000 |
| `post_receipts.json` | 24 | 3,500 |
| `post_forms.json` | 12 | 2,500 |
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
| R53-1 | Morale overlap | Low | Critical | Needs owner |
| R53-2 | Spy drift | Low | High | Contract |
| R53-3 | Vault overlap | Low | Medium | Consent |
| R53-4 | Sealed breach | Low | Critical | Hard rule |
| R53-5 | Radio overlap | Low | Medium | Boundary |
| R53-6 | Withhold abuse | Med | High | Reviewers |
| R53-7 | Tone grimness | Med | Medium | Index framing |
| R53-8 | Determinism | Low | High | Day ticks |
| R53-9 | Content overrun | Med | Medium | Budget |
| R53-10 | Grind | Med | Medium | Rounds as rhythm |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Does the postal service get its own save section, or ride `narrative`?**
   Recommended: additive inside `narrative`, with a dedicated section only if
   the foreman prefers an explicit store.
2. **Can anyone send mail anywhere?** Recommended: yes within reachable sites,
   with waiting lists where capacity is short.
3. **What may a withholding board hold?** Recommended: any letter, on request,
   with two reviewers, a reason, a timer, and an obligation to deliver or
   return; sealed letters are exempt from review entirely.
4. **Are dead letters ever published?** Recommended: no; quoted only with the
   writer's or kin's consent, routed through the vault and memory owners.
5. **Do couriers ever refuse a round?** Recommended: yes for weather, injury,
   and safety, with no stigma and a rescheduled pouch.

---

## 17. APPENDIX D — LETTER CORPUS TABLE (40 LETTERS)

| # | Letter | Voice | Subject | State | Note |
|---|---|---|---|---|---|
| 1 | Before the doors | engineer | last shift | found | bunker find |
| 2 | To my daughter | parent | goodbye | found | sealed |
| 3 | Unsent apology | sibling | quarrel | unsent | never sent |
| 4 | The recipe | grandmother | food | found | recipe inside |
| 5 | Shift roster note | foreman | work | found | utility |
| 6 | Lost kin, first | cousin | alive? | found | search case |
| 7 | Lost kin, second | aunt | address | found | address old |
| 8 | The debts | neighbor | favor | unsent | honest |
| 9 | Thank you letter | child | grown | found | pencil |
| 10 | Complaint draft | worker | unfairness | unsent | never sent |
| 11 | Love letter | young couple | parting | found | sealed |
| 12 | Seeds enclosed | farmer | gift | found | seeds inside |
| 13 | The map note | scout | route | found | useful |
| 14 | Letter to nobody | unknown | reflection | found | addressed blank |
| 15 | The winter list | quartermaster | supplies | found | utility |
| 16 | Get well note | friend | illness | found | kind |
| 17 | The refusal | clerk | refusal | unsent | painful |
| 18 | A joke letter | prankster | humor | found | morale |
| 19 | The prayer copy | elder | comfort | found | private |
| 20 | Last inventory | storekeeper | care | found | detailed |
| 21 | To the next crew | driller | advice | found | trades |
| 22 | The music sheet note | musician | song | found | with score |
| 23 | A child's drawing letter | child | family | found | drawing |
| 24 | The funeral words | friend | eulogy | found | grief |
| 25 | Treaty scrap | delegate | peace | found | historic |
| 26 | The water complaint | resident | pipes | unsent | mundane |
| 27 | Letter from the road | traveler | journey | found | stamp old |
| 28 | The apology, second | sibling | amends | found | late |
| 29 | Seed swop offer | gardener | exchange | unsent | friendly |
| 30 | The night report | watch | quiet | found | routine |
| 31 | Letter to the outpost | settler | homesick | found | deliverable |
| 32 | The recipe reply | cook | thanks | unsent | reply |
| 33 | A stranger's thanks | stranger | help | found | warm |
| 34 | The list of names | survivor | memory | found | memorial tie |
| 35 | Last will note | elder | estate | found | sealed |
| 36 | The toolbox note | repairer | advice | found | utility |
| 37 | Birthday letter | parent | child | found | delayed |
| 37 | A letter to the future | student | hope | found | vault tie |
| 39 | The complaint answer | steward | fairness | unsent | careful |
| 40 | The blank letter | unknown | none | found | envelope only |

Forty letters, and the states matter: most are found, several were never sent,
and four are sealed, which means the service will deliver them and never read
them. The blank envelope at the end is the corpus asking the shelter a
question it cannot answer, which is exactly what a dead-letter shelf is for.

---

## 18. APPENDIX E — ROUTE TABLE

| # | Route | Ends | Walk hours | Hazard | Weather hold |
|---|---|---|---|---|---|
| 1 | Bunker loop | post to post | 1 | none | no |
| 2 | Waystation | shelter to waystation | 6 | road | wind |
| 3 | Outpost | waystation to outpost | 8 | road | any storm |
| 4 | River stop | shelter to jetty | 4 | water | flood |
| 5 | Orchard round | shelter to rows | 2 | none | no |
| 6 | Ridge relay | shelter to seismics | 5 | slope | ice |
| 7 | Convoy spur | shelter to stand | 3 | traffic | none |
| 8 | School run | shelter to school | 1 | none | no |

Eight routes from a one-hour bunker loop to an eight-hour outpost walk, and the
weather-hold column is the courier system's respect for the road: a shelter
that sends a walker into a storm to save a day of mail has misunderstood what
mail is.

---

## 19. APPENDIX F — PNEUMATIC RUN TABLE

| # | Run | Stations | Length | Carrier | Load |
|---|---|---|---|---|---|
| 1 | Kitchen line | kitchen to mess | 80 m | small | notes |
| 2 | Clinic line | clinic to pharmacy | 60 m | small | scripts |
| 3 | Workshop line | workshop to store | 120 m | medium | parts lists |
| 4 | Office line | office to archive | 100 m | small | records |
| 5 | Power line | plant to control | 90 m | small | readings |
| 6 | Dorm line | office to dorms | 150 m | small | letters |

Six runs and one rule: tubes carry paper, small parts, and notes, never
anything alive and never anything that needs a person to explain it. The dorm
line is the one that turns the post office from a room into a service, because
a letter now arrives where a person sleeps.

---

## 20. APPENDIX G — POSTBOX TABLE

| # | Box | Location | Capacity | Key | Check |
|---|---|---|---|---|---|
| 1 | Dorm A | corridor | 40 | resident | daily |
| 2 | Dorm B | corridor | 40 | resident | daily |
| 3 | Workshop | door | 20 | lead | daily |
| 4 | Clinic | entry | 20 | nurse | daily |
| 5 | Kitchen | wall | 15 | cook | daily |
| 6 | School | hallway | 30 | teacher | daily |
| 7 | Gate | post | 30 | watch | daily |
| 8 | Outpost | window | 50 | keeper | with rounds |
| 9 | Waystation | shelf | 30 | keeper | with rounds |
| 10 | Row office | shelter | 60 | postmaster | always |
| 11 | Box row | yard | 12 | public | weekly |
| 12 | Mail stone | route | 10 | none | season |

Twelve boxes, and the last one has no key because it is a stone with a hollow
under it where people leave letters for whoever walks past next. It is the
oldest postal system in the world and the expansion keeps it, because a mail
service with a public box and a stone is more real than one with only a
counter.

---

## 21. APPENDIX H — WITHHOLDING POLICY TABLE

| # | Reason | Reviewers | Timer | Obligation |
|---|---|---|---|---|
| 1 | Death news unconfirmed | 2 | 7 days | confirm or deliver |
| 2 | Sender at risk | 2 | 14 days | protect, then deliver |
| 3 | Recipient in crisis | 2 | 3 days | care first, then deliver |
| 4 | Child recipient | 2 | 7 days | guardian plan |
| 5 | Disputed kinship | 2 | 14 days | verify |
| 6 | Seal in doubt | 2 | 7 days | verify seal, never read |
| 7 | Address unsolvable | 2 | 30 days | shelf if unsolved |
| 8 | Safety threat named | 2 | 14 days | involve watch, then deliver |
| 9 | Requested by sender | 2 | timer as asked | honor until |
| 10 | Requested by recipient | 2 | 14 days | hold and return |

Ten reasons, one rule, and the timer column is the promise that holding is
always temporary. The eighth row is the only one that touches the watch owner,
and it does so because a letter can carry a real threat; the plan refuses to
pretend otherwise while also refusing to make the post an intelligence arm.

---

## 22. APPENDIX I — DEAD-LETTER SHELF TABLE

| # | Entry | Addressee | Found | Attempts | Status |
|---|---|---|---|---|---|
| 1 | Before the doors | "my daughter" | year 1 | 4 | open |
| 2 | Letter to nobody | unknown | year 1 | 0 | shelf |
| 3 | Lost kin first | name unknown | year 1 | 6 | open |
| 4 | Lost kin second | old address | year 2 | 5 | open |
| 5 | The old stamp | pre-war address | year 2 | 3 | shelf |
| 6 | The blank envelope | none | year 2 | 0 | shelf |
| 7 | Cousin abroad | far settlement | year 3 | 2 | traveling |
| 8 | The wrong nickname | nickname | year 3 | 7 | solved |
| 9 | The railway wife | station name | year 3 | 4 | open |
| 10 | The found recipe | no surname | year 4 | 2 | claimed |
| 11 | The last will note | "next of kin" | year 4 | 1 | sealed |
| 12 | A stranger's thanks | no address | year 5 | 0 | shelf |
| 13 | The typed page | initials only | year 5 | 3 | open |
| 14 | The child's letter | "grandpa" | year 5 | 5 | solved |
| 15 | The docket | office number | year 6 | 1 | shelf |
| 16 | The water complaint | "the man upstairs" | year 6 | 2 | solved |
| 17 | The apology | first name | year 7 | 8 | open |
| 18 | The seed letter | "the farmer" | year 8 | 1 | claimed |
| 19 | The prayer copy | "the old lady" | year 9 | 3 | solved |
| 20 | The blank reply | none | year 10 | 0 | shelf |

Twenty shelf entries across a decade, with a status column that shows the
system's real work: some are solved, some are claimed, some are still open
after eight attempts, and four will simply sit on a lit shelf forever, which
the expansion treats as a valid and dignified ending for a piece of paper.

---

## 23. APPENDIX J — COURIER TABLE

| # | Role | Round | Pair rule | Load | Weather rule |
|---|---|---|---|---|---|
| 1 | Lead courier | outpost | with walker | 6 kg | hold, reschedule |
| 2 | Route walker | waystation | with lead | 5 kg | hold |
| 3 | River runner | jetty | solo | 3 kg | flood hold |
| 4 | Ridge carrier | seismics | solo | 3 kg | ice hold |
| 5 | Convoy hand | stand | with driver | 8 kg | none |
| 6 | School runner | school | solo | 3 kg | none |
| 7 | Tube keeper | plant | solo | none | none |
| 8 | Apprentice | training | with any | 2 kg | supervised |

Eight courier roles with pair rules and loads, and the river and ridge rows
show the shelter's honest limits: a solo walker on a short safe route is fine,
and a solo walker on ice is not, and the difference is written in the table
where every shift planner will read it.

---

## 24. APPENDIX K — RECEIPT TABLE

| # | Receipt | Pouch | Out | In | Condition |
|---|---|---|---|---|---|
| 1 | R-001 | waystation | day 30 | day 30 | sealed |
| 2 | R-002 | waystation | day 37 | day 38 | sealed, wet |
| 3 | R-003 | outpost | day 44 | day 46 | sealed |
| 4 | R-004 | outpost | day 51 | day 54 | held by storm |
| 5 | R-005 | jetty | day 58 | day 58 | sealed |
| 6 | R-006 | outpost | day 65 | day 68 | opened by accident |
| 7 | R-007 | waystation | day 72 | day 73 | sealed |
| 8 | R-008 | stand | day 79 | day 80 | sealed |
| 9 | R-009 | outpost | day 86 | day 89 | sealed |
| 10 | R-010 | waystation | day 93 | day 93 | resealed |
| 11 | R-011 | outpost | day 100 | day 103 | sealed |
| 12 | R-012 | school | day 107 | day 107 | hand to hand |

Twelve receipts, and the two bad rows are the service's real character: one
pouch held three days by a storm, and one opened by accident, which is
recorded, apologized for, and reported to the sender. A post office that only
logs perfect deliveries isnt recording anything worth reading.

---

## 25. APPENDIX L — FORM TABLE

| # | Form | Purpose | Fields | Print | Seal |
|---|---|---|---|---|---|
| 1 | Delivery receipt | proof | pouch, dawn | yes | no |
| 2 | Withholding request | hold a letter | reason, requesters | yes | yes |
| 3 | Return note | send back | reason | yes | no |
| 4 | Forward slip | new address | old, new | yes | no |
| 5 | Address board card | names | name, room | yes | no |
| 6 | Route card | round sheet | route, walker | yes | no |
| 7 | Pouch label | routing | route, pouch | yes | no |
| 8 | Shelf entry | index | addressee, day | yes | no |
| 9 | Claim form | kin claim | name, proof | yes | no |
| 10 | Lost letter notice | search | details | yes | no |
| 11 | Sealed-register line | seal record | id, day | yes | no |
| 12 | Courier roster | shifts | names, rounds | yes | no |

Twelve forms, and every one is a print job so the press owner stays the paper
authority. The sealed-register line is the quiet one: it records that a sealed
letter exists and that nobody opened it, which is how the service proves its
rule over years instead of promising it once.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_post_drawer` | 3 | Old letters logged |
| `quest_post_window` | 3 | Office opens |
| `quest_post_first_delivery` | 3 | First letter home |
| `quest_post_board` | 4 | Addresses fixed |
| `quest_post_round` | 4 | First round walked |
| `quest_post_tubes` | 4 | Tubes running |
| `quest_post_shelf` | 3 | Shelf indexed |
| `quest_post_case` | 5 | First hold case |
| `quest_post_timer` | 4 | Deadline resolved |
| `quest_post_reply` | 3 | Stranger answered |
| `quest_post_outpost` | 4 | Outpost mail |
| `quest_post_lost_kin` | 5 | Lost kin resolved |
| `quest_post_evening` | 3 | Writing evening |
| `quest_post_round_alone` | 4 | Apprentice carries |
| `quest_post_what_mail_carries` | 3 | Post ordinary |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_post_collect` | 3 | Letters collected |
| `quest_post_sort` | 3 | Sorted |
| `quest_post_misdirect` | 3 | Misdirect fixed |
| `quest_post_priority` | 3 | Urgent marked |
| `quest_post_unclaimed` | 3 | Unclaimed checked |
| `quest_post_name_board` | 3 | Names posted |
| `quest_post_forward` | 3 | Forwarding set |
| `quest_post_ambiguous` | 4 | Ambiguous solved |
| `quest_post_letters_for_children` | 3 | Child mail |
| `quest_post_initials` | 3 | Initials book |
| `quest_post_pouch` | 3 | Pouch packed |
| `quest_post_round_paid` | 3 | Receipts kept |
| `quest_post_weather` | 3 | Weather hold |
| `quest_post_two_walkers` | 3 | Pair rule |
| `quest_post_waystation` | 3 | Relay cache |
| `quest_post_tube_build` | 4 | Tube run built |
| `quest_post_carrier` | 3 | Carriers made |
| `quest_post_blockage` | 3 | Blockage cleared |
| `quest_post_pressure` | 3 | Compressor serviced |
| `quest_post_switch` | 3 | Switches labelled |
| `quest_post_review` | 4 | Review held |
| `quest_post_deliver_held` | 4 | Held delivered |
| `quest_post_return_held` | 3 | Held returned |
| `quest_post_shelf_lamp` | 3 | Shelf lit |
| `quest_post_index` | 3 | Index written |
| `quest_post_paper` | 3 | Paper stocked |
| `quest_post_evening_hold` | 3 | Evening held |
| `quest_post_answer_stranger` | 3 | Stranger answered |
| `quest_post_keepsake` | 3 | Keepsake kept |
| `quest_post_seal` | 3 | Seals made |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Sorrel Nib** — postmaster. Kept a drawer of letters for years because she
would not deliver them without rules, and now writes the rules she always
wanted. Believes the post is a promise the whole shelter makes to one person
at a time.

**Quilla Taffy** — sorter. Knows the pigeonhole wall like a keyboard and can
read a smudged address the way other people read handwriting. Believes sorting
is a form of attention.

**Elda Prine** — courier lead. Walks the long round twice a month and refuses
to send anyone solo into ice. Believes a pouch is a person's news and should
move like it.

**Mern Hark** — pneumatics. Maintains the compressor, labels the switches, and
loves the sound of a carrier arriving. Believes speed between two people should
not require a runner's legs.

**Ume Sallow** — dead letters. Keeps the shelf lamp lit, the index current,
and a list of every name still open. Believes an undelivered letter is not
finished.

**Norr Wicker** — route walker. Knows every waystation keeper by name and every
muddy kilometer by heart. Believes weather is a colleague.

**Ysolda Rell** — board chair. Writes the withholding rules, signs nothing
alone, and sets the timer that keeps holding honest. Believes the hardest
letter should be decided twice.

**Lisk Amble** — apprentice. Sixteen, carries the school run, and signs every
receipt with a careful hand. Believes a receipt is a promise signed by somebody's
thumb.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Box Row** — twelve doors and one that sticks in rain.
- **The Route Gate** — couriers, pouches, and the weather board.
- **The Waystation Post** — a shelf, a stamp, and a kettle.
- **The Outpost Window** — mail that crossed eight hours of road.
- **The Convoy Stand** — a pouch tied to a rail and a driver who waits.
- **The Lost Drop** — where a letter waited six years under a rock.
- **The Letter Tree** — a hollow, a habit, and a small honest network.
- **The Clinic Box** — a canister thunk and a nurse's relief.
- **The Kitchen Box** — notes that travel faster than feet.
- **The Mail Stone** — route marks cut by people who walked them. 

---

## 30. APPENDIX Q — MAIL CHARTER

| Clause | Promise |
|---|---|
| Free | Mail is not bought, sold, or taxed |
| Sealed | No letter is opened outside its addressee |
| Slow | Delivery takes the road's time, and the delay is explained |
| Kept | Undelivered words are stored, indexed, and lit |
| Held | A held letter has two reviewers, a reason, and a timer |
| Returned | A held letter is delivered or returned, never destroyed |
| Private | The service does not report, gossip, or sell information |
| Reach | Every reachable site has a box and a round |
| Receipted | Every pouch is signed out and back |
| Written | The shelter teaches no literacy test, but keeps pens in reach |

The mail charter is the expansion's first-class design object, posted at the
window where every resident can read it. Its seventh line is the one that
distinguishes a post office from an intelligence service, and the service is
designed so that breaking it would require more than one person's decision.

---

## 32. APPENDIX R — WORKED POST YEAR

**Week one.** Sorrel opens the drawer with Quilla and logs forty letters in one
afternoon. Eleven are addressed to people who live in the shelter; two are
sealed; one has no addressee at all. The window opens the next morning with a
board, a bell, and a rule card.

**Week two.** The first delivery is a recipe letter that has waited four years.
The recipient reads it standing at the counter and then asks, very quietly,
whether there are more. Sorrel says yes, and the office suddenly has a queue.

**Week three.** Quilla fixes the pigeonhole wall and the address board goes up
with every resident's name, room, and initials. The ambiguous-address protocol
resolves three letters in one morning and prevents a fourth from being
delivered to the wrong dorm, which is the day the board earns its wall.

**Week five.** Elda walks the waystation round and returns with a receipt
signed by a keeper who has not seen mail in two months. The pouch comes back
heavier than it went, which becomes the round's standing joke and its actual
purpose.

**Week six.** Mern starts the compressor and the first canister thunks from
the office to the clinic in eleven seconds. A nurse sends back a note that says
a single word, and the tube plant acquires a small, permanent purpose.

**Week eight.** Ume opens the dead-letter shelf with a lamp and an index.
Four entries go on it in the first week, including the letter addressed to
nobody, which Ume files under its own name because a name is a kind of respect.

**Week ten.** The withholding board hears its first case: a letter telling a
resident that their brother is dead, unconfirmed. Ysolda reads the rule card
aloud, two reviewers sign, and the letter is held for seven days while the
shelter confirms. The confirmation takes five days, the letter is delivered in
a room with tea, and the deadline is met.

**Week fourteen.** The outpost sends its first mail bag and receives one back.
The bag contains a homesick letter, a supply list, and a pressed flower, and the
return bag contains two letters, seeds, and a receipt signed by the keeper.

**Week eighteen.** A lost-kin letter from the drawer finds a reader: the name
in it matches a resident's childhood nickname, and the letter is delivered to
someone who has not heard their old name in a decade. The shelter's response is
to put every old nickname into the initials book, which turns out to be the
single most useful thing the address system ever does.

**Week twenty-two.** The first letter-writing evening is held in the letter
room with paper from the press, and eleven people come, six of whom write to
someone who is dead, which the evening allows and the mail charter quietly
supports by delivering such letters to the shelf when there is no hand to take
them.

**Week thirty.** Lisk carries the school run alone for the first time and comes
back with a receipt, a thank-you note, and a complaint about a sticky box
hinge. All three are filed.

**Week forty.** The seasonal shelf review finds seventeen entries, two solved,
one claimed by kin, and one that will never be solved and is re-lamped anyway.
The index grows a column for handwriting descriptions because half the shelf's
work is matching a scrawl to a person.

**Week fifty.** The year ledger closes with delivery figures, delay figures,
four withholding cases resolved within their timers, and one deliberate return.
The window stays open, the boxes are checked at dawn, and the mail stone gets
its first route mark.

---

## 33. APPENDIX S — VIGNETTES (TONE SAMPLE)

> Quilla reads the smudged address and says the name out loud to the empty
> office twice, because a letter is easier to sort when you know a person is
> on the other end of it.

> The two reviewers sign and the timer is set, and Ysolda puts the letter in
> the drawer and says the sentence she insists everybody on the board say
> aloud: this is somebody's words, and we are only the delay.

> Ume turns the shelf lamp on in the morning and off at night, and when
> someone asks why a shelf needs a lamp, she says because it is where people
> are still waiting.

> Lisk signs the receipt with a careful hand and the keeper signs under it with
> a thumbprint because there is no ink in the pouch, and the two signatures sit
> together in the ledger as proof that the paper arrived.

---

## 34. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| No sorting | lost letters | pigeonholes and board |
| Wrong delivery | privacy hurt | apology and protocol |
| Opened seal | trust broken | report and return |
| Withheld forever | betrayal | timers enforced |
| Weather walk | injury | holds respected |
| Tube blockage | delays | physical clear |
| Dead shelf dark | despair | lamp and index |
| Unclaimed kin | waiting | index and searches |
| No receipts | doubted service | signed pouches |
| Paper shortage | expressions stop | press stock |

Every failure here is recoverable with a rule that already exists in the plan,
which is the post's real design: a service whose mistakes are survivable by
paperwork is a service that can be trusted with people's worst news.

---

## 35. APPENDIX U — CONTENT REVIEW CHECKLIST

- [ ] No interception, opening, or surveillance of sealed letters.
- [ ] Morale routes only through `NeedsSystem.Modify`.
- [ ] Withholding requires two reviewers, a reason, and a timer.
- [ ] Dead letters are kept, indexed, and never destroyed.
- [ ] Courier routes ride the road owner; weather stays with its owner.
- [ ] Paper and forms route through the press owner.
- [ ] Letters from the dead route through the memory owner where relevant.
- [ ] No currency, postage prices, or stamp economy.
- [ ] Save additions are additive inside the narrative envelope.
- [ ] Determinism uses campaign days only.

---

## 36. APPENDIX V — GLOSSARY

- **Pigeonhole** — a sorting slot for a route, quarter, or person.
- **Round** — a courier's scheduled walk with a pouch.
- **Pouch** — the sealed bag carried on a round.
- **Receipt** — the signature chain that proves a pouch arrived.
- **Tube** — a pneumatic run between two shelter stations.
- **Carrier** — the canister that rides a tube.
- **Hold** — a letter temporarily withheld under the board's rules.
- **Timer** — the deadline that forces delivery or return.
- **Dead letter** — a letter without a findable addressee.
- **Index** — the shelf's searchable record of undelivered words.

---

## 37. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `LetterDeliverySystem` | letters | states | morale |
| `SurvivorLetterDeliverySystem` | roster | match | needs |
| `PostOfficeSystem` | duty roster | office state | letters |
| `AddressSystem` | roster | board | letters |
| `CourierSystem` | routes | rounds | routes |
| `PneumaticSystem` | rooms | tube state | rooms |
| `WithholdingBoardSystem` | cases | holds | letters |
| `DeadLetterSystem` | shelf | index | nothing |
| `LetterWritingSystem` | supplies | evenings | needs |
| `PostRecordSystem` | receipts | ledgers | letters |
| `NeedsSystem` | nothing | nothing | nothing |
| `PressHostSession` | forms | prints | nothing |
| `RouteInfrastructureSystem` | nothing | nothing | nothing |
| `WeatherSystem` | holds | nothing | nothing |
| `MemorialSystem` | memory | nothing | nothing |
| `StandingRecord` | ledgers | records | nothing |
| `Inventory` | paper | nothing | nothing |

---

## 38. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`post_offices.json`** — `office_id`, `display_name`, `window_hours`,
`pigeonholes`, `boxes[]`, `staff`, `tags[]`.

**`post_routes.json`** — `route_id`, `ends[]`, `walk_hours`, `hazards[]`,
`weather_hold`, `relay_cache`, `tags[]`.

**`post_pneumatic.json`** — `run_id`, `stations[]`, `length_m`, `carrier_type`,
`compressor_load`, `blockage_state`, `tags[]`.

**`post_boxes.json`** — `box_id`, `location_id`, `capacity`, `key_holder`,
`check_cadence`, `tags[]`.

**`post_withholding.json`** — `reason_id`, `label`, `reviewers`, `timer_days`,
`obligation`, `sealed_exempt`, `tags[]`.

**`post_dead_letters.json`** — `letter_id`, `addressee`, `found_day`,
`attempts[]`, `claim_status`, `index_note`, `tags[]`.

**`post_letters_catalog.json`** — `letter_id`, `voice`, `subject`, `length`,
`tags[]`, `default_state`.

**`post_couriers.json`** — `role_id`, `round`, `pair_rule`, `load_kg`,
`training_path`, `weather_rule`, `tags[]`.

**`post_receipts.json`** — `receipt_id`, `pouch_id`, `courier_id`, `out_day`,
`in_day`, `condition`, `notes`, `tags[]`.

**`post_forms.json`** — `form_id`, `purpose`, `fields[]`, `print_ref`,
`seal_required`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid route or item references, or out-of-range
numbers.

---

## 39. APPENDIX Y — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Letters in flight | service load | States |
| Deliveries per round | reach | Receipts |
| Average wait | slowness honesty | Receipts |
| Misdirects | sorting quality | Office |
| Holds opened | board health | Withholding |
| Holds resolved on time | discipline | Withholding |
| Shelf entries | undelivered words | Dead letters |
| Shelf entries solved | hope | Dead letters |
| Tube trips | convenience | Pneumatics |
| Writing evenings | culture | Evenings |

Telemetry is diagnostic only; it never gates content and never ranks a courier
or a sorter.

---

## 40. APPENDIX Z — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 both letter systems are wired into persistence additively.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows rounds, holds, shelf growth, and write-backs.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No interception, opening, or currency content exists.

---

## 41. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. Can a letter be sent outside the valley, and what happens to the reply?
2. Who inherits a postbox key when a resident dies?
3. Can a resident ask never to receive mail from a specific person?
4. Does the shelter ever print a stamp, and what does it mean if it does?
5. Are couriers armed, and does the route owner's watch escort matter?
6. Can the board hold a letter from a child to a parent, and for how long?
7. Is a dead-letter shelf ever emptied, and by whom?
8. Does the service ever deliver a letter twice because the first was lost?

None of these may be decided unilaterally; each changes tone and balance.

---

## 42. APPENDIX AB — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Children's letters and initials |
| 1 | 13 Faith | Private letters kept private |
| 2 | 17 The Long Evening | News from outside for elders |
| 3 | 24 The Long Goodbye | Letters from the dead |
| 3 | 25 The Iron Road | Rail mail and station boxes |
| 4 | 28 The Lesson | School runs and pen pals |
| 4 | 30 The Press | Paper, envelopes, and forms |
| 5 | 34 The Long Road | Courier rounds and waystations |
| 5 | 36 The Watch | Gate box and route safety |
| 6 | 41 The Quiet | Night sorting and quiet hours |
| 7 | 44 The Outpost | Site mail and homesickness |
| 7 | 45 The Envoy | Letters between meetings |
| 7 | 46 The Long Change | Addresses that move as the world does |
| 8 | 49 The Mirror | Flash notice of a pouch |
| 8 | 50 The Vault | Consented family archives |
| 9 | 55 The Quarter | The neutral service between neighbors |

Each hook is additive. The Post can ship alone, and every other expansion can
ship without it.

---

## 43. APPENDIX AC — ENDING PROSE SKETCHES

**The Open Mail.** Everyone sends and receives, rounds run weekly, and letter
day is a real day in the shelter's week.

**The Held Hand.** The board matures into a trusted practice, and every held
letter is delivered or returned with its reason written beside it.

**The Long Round.** The courier network reaches every waystation and outpost,
and the post becomes the valley's slow nervous system.

**The Shelf of Names.** The dead-letter index grows, each entry lamp-lit and
hopeful, and a stranger walking in twenty years can find their grandmother's
words.

**The Written Year.** The shelter writes: replies, complaints, thanks, recipes,
and letters home, and the post office keeps a waiting list for pens.

**Fade.** A pigeonhole wall in the morning light, a name called, a door
opening, and a receipt signed with a thumbprint.

---

## 44. APPENDIX AD — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Opened letters | trust death | seals sacred |
| Spy post | tone break | no interception |
| Currency stamps | economy break | free mail |
| Held forever | betrayal | timers |
| Destroyed dead letters | cruelty | shelf and lamp |
| Gossip engine | harmful | neutral service |
| Instant delivery | unreal | rounds and weather |
| Twist-factory letters | hollow | voices and news |
| Morale duplication | authority break | Needs owner |
| Vault takeover | authority break | consent only |

The list exists because a post office is easy to write as a plot device. The
expansion's rule is that mail is infrastructure: slow, private, receipted, and
kept, and its most dramatic moments are a timer and a lamp.

---

## 45. APPENDIX AE — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Offices | 4 | 1,000 |
| Routes | 8 | 2,500 |
| Tube runs | 6 | 2,000 |
| Postboxes | 12 | 2,000 |
| Withholding reasons | 10 | 3,000 |
| Dead-letter entries | 20 | 4,000 |
| Letters | 40 | 8,000 |
| Couriers | 8 | 2,000 |
| Receipts | 12 | 2,000 |
| Forms | 12 | 2,500 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~60,000** |

---

## 46. APPENDIX AF — FIRST MAIL YEAR

| Month | Milestone | Kept by |
|---|---|---|
| 1 | Drawer logged | Sorrel |
| 2 | Window opens | Quilla |
| 3 | Address board | Quilla |
| 4 | First round | Elda |
| 5 | Tubes open | Mern |
| 6 | Shelf lit | Ume |
| 7 | First hold | Ysolda |
| 8 | Outpost bag | Elda |
| 9 | Lost kin solved | Sorrel |
| 10 | Writing evening | Lisk |
| 11 | Long round | Norr |
| 12 | Year ledger | all |

Twelve months from a drawer to a service, with one keeper named per milestone.
The shelf arrives before the first hold on purpose: a service that can keep
undelivered words is ready for the harder job of deciding which letters should
wait.

---

## 47. APPENDIX AG — SORTING BOARD TABLE

| # | Pigeonhole | Route | Priority | Note |
|---|---|---|---|---|
| 1 | Dorm A | bunker | normal | quarters |
| 2 | Dorm B | bunker | normal | quarters |
| 3 | Clinic | bunker | urgent | scripts |
| 4 | Workshop | bunker | normal | parts |
| 5 | Kitchen | bunker | normal | notes |
| 6 | School | school run | normal | children |
| 7 | Gate | routes | normal | departures |
| 8 | Waystation | waystation | weekly | caches |
| 9 | Outpost | outpost | weekly | long |
| 10 | Convoy | stand | departures | pouches |
| 11 | Shelf | dead letter | never | review |
| 12 | Hold | withholding | by case | drawer |

Twelve pigeonholes with their own rhythms, and the shelf and hold slots sit on
the wall beside the ordinary ones so that the two hardest destinations are
never hidden in a back room. The sort board's design principle is that a
service's integrity should be visible from the counter.

---

## 48. APPENDIX AH — WITHHELD CASE LOG

| # | Case | Reason | Reviewers | Timer | Result |
|---|---|---|---|---|---|
| 1 | Death unconfirmed | 1 | 2 | 7 days | delivered |
| 2 | Child recipient | 4 | 2 | 7 days | guardian plan |
| 3 | Sender at risk | 2 | 2 | 14 days | delivered |
| 4 | Recipient crisis | 3 | 2 | 3 days | delivered |
| 5 | Seal in doubt | 6 | 2 | 7 days | verified, delivered |
| 6 | Disputed kinship | 5 | 2 | 14 days | verified |
| 7 | Sender request | 9 | 2 | asked | held 30 days |
| 8 | Recipient request | 10 | 2 | 14 days | returned |

Eight withheld cases, and the results column shows five deliveries, two
verifications, one hold honored at the sender's request, and exactly one
return. The expansion's position is written in those numbers: holding letters
is sometimes necessary, and the board that holds them exists to make holding a
brief, witnessed exception rather than a quiet power.

---

## 49. APPENDIX AI — POST COVENANT

| Clause | Promise |
|---|---|
| Free | Every resident may send and receive |
| Sealed | Nobody outside the addressee opens a letter |
| Slow | Delay is honest and explained |
| Carried | A person walks each round; weather is respected |
| Receipted | Every pouch is signed out and home |
| Held | Wooden holds have two signatures and a timer |
| Kept | Undelivered words live on a lit, indexed shelf |
| Returned | Nothing is destroyed; everything ends delivered or returned |
| Quiet | The service never reports, gossips, or sells |
| Written | Pens and paper are within every reach |

The post covenant is the expansion's first-class design object, carved small
enough to sit under the window glass. It is the shelter's promise that the
most private thing it carries is also the thing it guards most carefully, and
that the slowest system in the valley is the one that never lies about a
letter.

---

## 50. APPENDIX AJ — POST SUCCESSION TABLE

| Role | First | Successor | Handover |
|---|---|---|---|
| Postmaster | Sorrel | Quilla | one drawer |
| Sorter | Quilla | Lisk | one wall |
| Courier lead | Elda | Norr | one round |
| Pneumatics | Mern | apprentice | one blockage |
| Dead letters | Ume | Quilla | one index |
| Route walker | Norr | Lisk | one weather |
| Board chair | Ysolda | Sorrel | one case |
| Apprentice | Lisk | next recruit | one receipt book |

The succession table is how the service outlives its founders. The board-chair
handover takes a real case because the successor must learn, in company, what
it feels like to be the delay between a person and their worst news — and that
is not something anybody can learn from a rule card.

---

## 51. CLOSING STATEMENT

ASHFALL already holds two letter systems, a lifecycle with delivered,
withheld, and unanswered states, a survivor match with a morale bonus, and a
corpus of letters written by people who may not be alive. Both systems are
unwired: no host, no save section, no panel, no service. The Post builds the
service around them — a window, a sorting wall, an address board, couriers with
pouches and receipts, pneumatic tubes that thunk between the kitchen and the
clinic, a dead-letter shelf with a lamp and an index, and a withholding board
whose whole purpose is to make sure that the hardest decision in the service
is made twice, in writing, with a deadline. It adds no espionage, no pricing,
and no gossip, and it delivers the oldest morale system there is.

> Wave 9 note: this plan is one of five Wave 9 expansion bibles (52–56). Each is
> self-contained; none requires another to ship. The shared Wave 9 index lives
> at `docs/expansions/wave9/WAVE9_INDEX.md`. The safe pre-signature step is
> Phase 1 (data schemas and validators), which is additive and reversible.
> Evidence anchors: `LetterDeliverySystem` (`LetterDeliveryState`,
> `LetterDeliveryRecord`, `AddressLetter`, `DeliverLetter`, `WithholdLetter`,
> `MarkUnanswered`, `RestoreState`, default morale delta 6.0),
> `SurvivorLetterDeliverySystem` (`not_found` / `found` / `addressed` /
> `delivered` / `withheld` / `unanswered`, `SurvivorLetterRecordState`,
> `SurvivorRef`, `DefaultDeliveryMoraleBonus = 8f`), the narrative corpora
> `letters_expansion.json`, `survivor_letters_lost_kin.json`,
> `unsent_letters_batch_2.json`, and `pneumatic_carrier_capsule_logs.json`,
> and the confirmed absences: no host wiring for either system, no
> `letter_delivery` section in `SaveSectionRegistry`, and no mail panel in
> `src/UI`.