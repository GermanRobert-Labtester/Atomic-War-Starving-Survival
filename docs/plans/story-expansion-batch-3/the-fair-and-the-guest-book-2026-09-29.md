# Feature / Task Plan: The Fair (a seasonal gathering over the existing barter, caravan and petition seams) & The Guest Book (hospitality over the existing door and visitor owners)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 3, plans 1 of 4 (subjects 33–34).

> **Subjects covered (2 of the 8 in the "gatherings and signals" batch):**
> 33. **The Fair** — one authored week a year when the region's caravans, petitions, prices and
> rumours converge on one held ground. (Prefix `FA`.)
> 34. **The Guest Book** — the ledger of people who stay without joining: guests, pilgrims,
> wayfarers, and the small debts that sleeping under a roof creates. (Prefix `GB`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `docs/expansions/wave3/expansion_26_the_common_table_plan.md` (food culture — the Fair *eats*,
> it does not cook), `docs/expansions/wave7/expansion_45_the_envoy_plan.md` (diplomacy — the Fair
> is *attendance*, not protocol), `.ai/plans/the-underworld-2026-09-29.md` (E8: `ShelterBarterSystem`
> seam), `.ai/plans/living-region-2026-09-29.md` (graded news; gate petitions), and the batch-2
> door-adapter seam (index §4).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Week the Region Comes to Us

> *"A fair is a truce that has learned to keep accounts."*

Twelve settlements exist in the data as points on a map. For one week a year, several of them are
in the same place — and the map has no way to say so. The Fair is that week: an authored gathering
on held ground, where the caravans that already cross the region happen to cross each other, where
petitions queue behind barter, where prices are set in public for once, and where every rumour in
the region changes hands at least twice.

The Guest Book is the Fair's quieter half and its year-round one. Long before the first stall goes
up, strangers have been sleeping in the shelter's spare beds — a pilgrim on the Faith road, a
driver waiting on a part, a cousin of somebody's cousin, a child of the region passing through.
The book is a ledger with no jurisdiction: it does not recruit, vet, or naturalise. It records who
slept here, and what was borrowed, and what was returned.

**Tone & register.** Civic, warm, worn. The vocabulary is the market square and the front room:
*stall, dues, tally, ground-fee, bed, night, debt, return*. Prose should read like a town clerk
who has run this week twenty times and still stops to watch the first wagon come in. No commerce
glamour, no carnival. The romance of the Fair belongs to the players who attend it; the ledger
declines to confirm it.

**Mystery & texture.** Two things are held open on purpose. The Fair has no recorded *first year*:
the ground-fee tradition, the three-day truce and the tally stone all predate the shelter's own
records (§12). And the Guest Book's earliest surviving entry is in a hand nobody at the shelter
writes (§12). Neither is a puzzle with a solution in this plan. Both are load-bearing atmosphere:
a tradition older than its practitioners is the only kind that can bind strangers.

**The second layer.** A fair is the region agreeing, for one week, to be *legible to itself* —
prices in public, petitions out loud, rumours traceable to a mouth. The Guest Book is the same
agreement kept at the scale of one roof: you may pass through, and the passage will be written
down, and neither of us will pretend the writing is nothing. Both are acts of hospitality with
arithmetic attached, and the arithmetic is what makes them durable: a truce with a tally stone
outlasts a truce with a handshake.

---

## 1. Goal & Outcome

> *Design intent: the player should be able to say "the Fair is in six days" and feel the region
> lean toward one place. The week should feel like weather arriving — earned, expected, and never
> quite the same twice.*

### 1.1 The Fair (FA)

- **Goal:** One authored **Fair Row** per profile/season (ground id, days, dues, truce flag,
  expected caravans, petition window) activating over the *existing* owners: caravans reroute to
  the ground through the existing route authority, barter runs through `ShelterBarterSystem` with
  an authored **fair premium table** (closed, per-syndicate or per-faction), petitions from the
  Living Region's gate seam queue in one **petition window**, and a **Tally Stone** derives the
  week's public prices as a pure read model (never stored).
- **Outcome (observable):** on a fixed seed, the Fair row for the season opens on its authored
  day; two of the four authored caravans reach the ground; the fair premium applies only at the
  ground during the window and nowhere else; the petition window routes answers through the
  existing gate verbs; the Tally Stone shows the week's price index derived from real trades; on
  close, the ground returns to ordinary behaviour and no residue remains anywhere; save/load
  mid-Fair round-trips; with no Fair rows every owner behaves identically to today.
- **Non-Goals:** no second market, price or caravan authority; no new economy; no new faction; no
  minigames (Expansion 48 owns leisure); no food-culture mechanics (Expansion 26 owns cuisine); no
  diplomatic protocol (Expansion 45 owns it); no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Guest Book (GB)

- **Goal:** A small **Guest Ledger** (additive nested DTO — no new save section) of *stays*: guest
  name, arrival day, expected nights, bed assigned through the existing roster seam, small debts
  (`borrowed`, `returned`, `owed`) as ordinary stock transactions, and a **departure record**.
  Guests never join the roster, never take jobs, and never appear in survivor counts. Pilgrims
  (Faith and Schism seam), petitioners (Living Region seam) and drivers awaiting parts (Long Line
  seam) may stay as guests through one optional adapter each, shipped dark.
- **Outcome (observable):** on a fixed seed, a petitioner admitted at the gate opens a stay of
  authored length; a borrowed tool is issued and returned through the inventory owner; an
  unreturned item at departure becomes a small recorded debt and nothing more; the book's entry
  survives save/load; with no adapter hooks the guest flow is absent and every owner is unchanged.
- **Non-Goals:** no recruitment or naturalisation (Plan 204 owns it); no vetting (The Quiet War
  owns the knock); no population model; no guest combat, quests or dialogue trees; no new save
  section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes; solo/no-guest parity holds; handoff lists untouched shared
  paths.

### 1.3 The seam between them (X)

- **Goal:** One optional hook, shipped dark until both ends exist: Fair week raises the *rate* of
  guest arrivals (a read of the Fair row's presence only — never a probability the guest flow can
  be balanced around), and the Guest Book's departures may leave one **Fair rumour** line into the
  week's chatter. Neither subject reads the other's state beyond those two booleans/strings.

---

## 1b. Texture, Mystery & Voice

**The Fair: a truce with a tally stone.**

The week's emotional centre is the *public price*. All year, value is a private negotiation; for
seven days it is a number on a stone, and everyone can see when it moves. Write the stone like a
notice board that cannot argue: plain, dated, totalled. The truce flag is the same in prose —
never narrate the truce as safe; narrate it as *kept*.

**The Guest Book: hospitality has a format.**

Entry, night, borrowed, returned, departure. Five columns. The prose's whole power is in what the
columns refuse to hold — no reasons, no judgements, no character references. A guest book is the
most neutral document a household owns, and the only one strangers are allowed to read themselves
in.

**What the player is never told.**

- Who chose the ground. The Fair Row carries a ground id and a tradition (`fa_ground_1` in the
  closed table); the choosing is older than the shelter's records and is not modelled.
- What the tally stone was before it was a tally stone. It is a stone, it is inscribed, and the
  inscriptions under the current ones are not legible in any authored data.
- Who wrote the Guest Book's first surviving entry. The ledger stores `hand` as an opaque tag for
  provenance only; the first hand is not in the shelter's roster, alive or dead.
- Why departures are recorded when nothing consumes the record. The book is written for a reader
  who is not modelled. That is its dignity and its mystery.

**Voice — sample fragments (content candidates for `fair_lines.json` / `guest_book_lines.json`).**

> "Day one of the Fair. The stone says salt is down. Half the region will be wrong about that by
> Thursday and the stone will still say it." — tally board (FA)

> "Ground-fee, paid, two wagons. The fee is older than the fee schedule. The schedule is just
> where we wrote down what the fee has always been." — square clerk (FA)

> "Petition window, second morning. They asked for grain. They asked in front of everyone, which
> is what the window is for." — petition clerk (FA)

> "Guest, night four. Borrowed: one hooded lamp. Returned: one hooded lamp, filled. The filling
> was not owed. It is in the book anyway." — guest book (GB)

> "Arriving: a pilgrim. Expected two nights. The bed is the one by the cold wall and she chose it
> on purpose, which the book does not record and I have recorded here." — house parent (GB)

**Design texture beats.**

- **The truce flag is a promise the plan must keep mechanically.** If the row says truce, no
  authored event at the ground may contradict it. Trust in the Fair is built once and spent once.
- **The Tally Stone derives; it never stores.** A price index that can be edited is politics; a
  price index that can only be *observed* is weather.
- **Guests are never survivors.** The moment a stay becomes recruitment, the book becomes a menu
  and the mystery dies. (DEC-GB-01.)
- **Departures are recorded for their own sake.** Resist every instinct to make the record
  *useful*. Its usefulness is the wrong question; its existence is the feature.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the week leaves lying around.**

> "Tally stone, week's end. The last number is struck through once, in the hand that struck
> through it last year."

> "Ground-fee receipt: two wagons, one mule, one disagreement. The receipt records the mule."

> "Petition granted in public, at the window, on the third morning. The thanks were also public,
> which nobody planned."

**What the house leaves lying around.**

> "Guest book, page 112. Entry in a hand the house does not use. The entry is only a name and a
> number of nights, and the name is spelled the old way."

> "Lamp returned full. The borrower filled it before returning it. The book has no column for
> that and somebody drew a small mark in the margin, every year, in a different hand."

**Held silences (texture, not register rows).**

- What the tally stone's buried inscriptions counted. The current layer is the Fair's; the layer
  beneath is not legible in any authored data and must never be transcribed. Texture only.
- Whether the Fair ever failed to happen. The rows are authored per season and the season before
  the first row is not recorded; whether there was a year without a Fair is not asserted and must
  not be.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `TravelingCaravanSystem` with four authored caravans; `ShelterBarterSystem` barter seam incl. contraband broker caravan. | `.ai/plans/convoy-wars-and-inside-a-house-2026-09-29.md` E-note; `Economy/ShelterBarterSystem.cs` (cited L523, UW E8) | LIVE (per corpus; re-verify) |
| E2 | `BlackMarketSystem` premiums, credit limits, access tiers; `black_market_inventory.json` rows for 3 syndicates. | `.ai/plans/the-underworld-2026-09-29.md` E1–E2 | LIVE (per corpus) |
| E3 | Trade embargo owner gates routes (`IsRouteBlocked`, progress multipliers); no inspection. | UW E9 `Economy/TradeEmbargoSystem.cs` L212–249 | LIVE |
| E4 | Graded news seam (Heard/Told/Seen), gate petitions LR-P6 (open / ration / refuse / redirect), conserved population. | `.ai/plans/living-region-2026-09-29.md` §1, LR-P4/P6/P7 | PROPOSED (plan is DRAFT) |
| E5 | Door-adapter seam agreed once across QW / LR / PY (batch-2 index §4); `VetCandidate` exists and is host-wired. | QW E-notes (visitor integration, `FactionBountySystem` parallels) | PROPOSED (soft dependency) |
| E6 | Pilgrims/missionaries exposed at the gate; zealotry owner (`ZealotrySystem`, Plan 175) keeps its meaning. | `.ai/plans/faith-and-schism-2026-09-29.md` §1 | PROPOSED (soft dependency) |
| E7 | Inventory owner transactions (`borrowed`/`returned`) can be modeled as ordinary issue/return pairs; roster seam can be read for bed assignment without writes. | inventory & duty-roster owners (see Plan 22/Plan 24 evidence in their logs) | **VERIFY (P0)** |
| E8 | Where a location id for a *held ground* is authored today (`location_*` / `loc_*` namespaces; one expedition node with no interior per SN/RB finding). | `.ai/plans/second-nature-and-ruins-of-the-before-2026-09-29.md` findings | **VERIFY (P0)** |
| E9 | Whether `ShelterExpansionState`-style additive nested DTOs are checksum-safe in the door/visitor save owner (Guest Ledger placement). | codec / snapshot tests | **VERIFY (P0)** |
| E10 | Existing seasonal calendar seam to anchor the Fair window (ritual calendar, Expansion 13, integrated; Year-of-Ash day counts). | `YearOfAsh/YearOfAshTimelineSystem.cs` (per umbrella F4); ritual calendar owner | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Trade, prices, premiums | `BlackMarketSystem` / `ShelterBarterSystem` | one authored fair-premium table, applied only at the ground in-window (DEC-FA-03) |
| Caravan routing | `TravelingCaravanSystem` + route authority | Fair row may *invite* a caravan (read of authored presence); routing stays the route owner's |
| Petitions | Living Region gate seam (LR-P6) | petition *window* scheduling only; answers are the gate verbs |
| News | Living Region graded-news seam | optional read: Fair chatter as `Heard` lines; writes none |
| Guest stays, debts | inventory owner + roster seam (read) | `GuestLedger` (pure Core), nested additive DTO in the door/visitor save owner — **DEC-GB-02** |
| Truce flag | Fair row (authored) | enforces *no authored event at ground* while set; no combat owner changes |
| Price index | — | `TallyStone` (pure Core, derived, zero stored state) |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Economy/FairSchedule.cs` (new, pure), `Economy/TallyStone.cs` (new, pure), `Social/GuestLedger.cs` (new, pure)
**Data:** `fair_rows.json`, `fair_premiums.json`, `guest_book_lines.json`, `fair_lines.json`
**Host:** door/visitor host session (`INT`), day-owner registration (`src/Main.CampaignOwners.cs`, `INT`), barter host session (`INT`)
**Presentation:** extend the existing market/board surface with a Fair band and the door surface with a "Staying" list — no new routed panel
**Tests:** `Ashfall.Core.Tests/Economy/FairScheduleTests.cs`, `TallyStoneTests.cs`, `Ashfall.Core.Tests/Social/GuestLedgerTests.cs`, `Ashfall.Core.Tests/Save/GuestLedgerSaveTests.cs`

## 5. Packages

### FA-P0 — Premise audit (Auditor; read-only): close E7–E10; confirm the door-adapter contract wording with its owners; confirm fair-premium application point.
### FA-P1 — Fair rows + schedule (Core + data): authored rows, activation window, truce flag, ship-dark. **Accept:** round-trip; old saves load; no rows → identical behaviour.
### FA-P2 — Premium table + Tally Stone (Core, pure): closed premium table; derived price index; validator with row-level failures. **Accept:** determinism; no stored price state.
### FA-P3 — Petition window + chatter (host adapters): window scheduling over the gate verbs; optional `Heard` lines. **Accept:** each effect one owner call; no writes to news authority.
### GB-P1 — Guest Ledger (Core + data + host): stays, debts, departures; nested additive DTO. **Accept:** round-trip; guests never in survivor counts; solo parity.
### GB-P2 — Gate adapters (host, `INT`): one optional adapter per source (pilgrim, petitioner, wayfarer), all shipped dark. **Accept:** each adapter ≤ one boolean/string into the seam; no adapter → no guests.
### X-P1 — Seam hook (host, shipped dark): Fair presence raises guest-arrival rate via read only; departures may leave one rumour line. **Accept:** neither side reads the other's state beyond the documented fields.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no Fair rows and no guest adapters → barter, caravans, petitions, news, roster and inventory outputs identical on a saved corpus.
3. Conservation: all dues, premiums, borrows and returns balance through the inventory owner; nothing is created or destroyed at the ground.
4. Determinism: identical Fair windows, premium applications, petition orderings and guest arrivals on replay (`CampaignStreamIds` fork; never `System.Random`).
5. Save round-trip mid-Fair and mid-stay; old saves load; additive DTO checksum-safe (E9 closed at P0).
6. Guests are never survivors: no roster join, no job, no survivor count, ever (DEC-GB-01).
7. While the truce flag is set, no authored event may contradict it at the ground (DEC-FA-02).
8. No deferred row in `fair_rows.json` activates without its own premise check; the tally stone stores nothing.

## 7. Cross-plan boundaries
- **The Common Table (Exp. 26):** the Fair eats; it does not cook. No cuisine mechanics here.
- **The Envoy (Exp. 45):** attendance is not protocol. No treaty or diplomatic state here.
- **The Pastime (Exp. 48):** no games or tournaments. The Fair's entertainment is the region itself.
- **The Quiet War / The Living Region / The Plague Year:** one shared door adapter, unchanged; this plan is a fourth consumer only.
- **The Underworld:** fair premiums may differ per syndicate but the heat/trust owners are untouched; the Fair is a truce and the truce is enforced by the row, not by the enforcers.
- **Year Two:** the Fair may anchor to the campaign calendar read-only; the year-two chronicle may cite a Tally Stone week as a modifier input, gated as ever.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-FA-01 | One authored Fair Row per profile/season; the ground id is a closed table value; no authoring of the ground's history. | design | Yes |
| DEC-FA-02 | Truce flag enforced by *content exclusion*: no authored hostile event at the ground while set; no combat owner changes. | tone/rule | Yes |
| DEC-FA-03 | Fair premiums apply only at the ground id within the window; all other behaviour bit-identical. | rule | Yes |
| DEC-FA-04 | The Tally Stone derives from real trades and stores nothing; it is presentation of the week's own arithmetic. | architecture | Yes |
| DEC-GB-01 | Guests never become survivors through any path in this plan; recruitment stays Plan 204's. | rule | Yes |
| DEC-GB-02 | `GuestLedger` nests additively in the door/visitor save owner; no new save section. | architecture | Yes; confirm owner in P0 |
| DEC-GB-03 | Debts are ordinary inventory transactions; no debt authority, no interest, no enforcement. | rule | Yes |
| DEC-GB-04 | The first guest entry's hand is never explained; provenance tags are opaque (§12). | tone | Yes — **needs canon note** |
| DEC-X-01 | The seam hook ships dark until both halves exist; each side exposes exactly one boolean/string. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Fair`, `Guest`, `Stay`, `Tally`, `MarketDay`)
- [ ] Premise re-verified (Rule 7); door-adapter contract re-read; no overlapping live claim on barter, caravan, petition or door paths
- [ ] Signed decisions in hand (at minimum DEC-FA-01, DEC-GB-01)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing barter, caravan, gate-petition and door tests (list from P0 selector)
- [ ] Host selftests for the barter and door sessions (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: a Fair premium cannot be scoped to one ground and window without changing global pricing; a guest stay cannot be persisted without a new save section; the truce flag cannot be kept without touching a combat owner; the door-adapter contract would change for existing consumers; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the week larger than the player's ability to audit it. Any future plan that
answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| FA-OM-1 | Who chose the ground, and in what first year? | The row carries a ground id and a tradition; naming the founders converts a truce into a legend with owners. | Canon owner only, as a signed decision. |
| FA-OM-2 | What was the tally stone before it was a tally stone? | The buried inscriptions are not legible in any authored data. An older ledger under a newer one is atmosphere; a translation would be archaeology. | Never — deliberately sealed. |
| FA-OM-3 | What would break the truce? | DEC-FA-02 excludes contradiction mechanically; whether anything *could* is not modelled and must not be. The truce's credibility depends on the question staying open. | Never — tone-locked. |
| FA-OM-4 | Why do the same caravans return to a week that costs them dues? | The four caravans are authored to attend; motive is not authored (inherits convoy-wars CW-OM discipline). | A named content pass, per caravan, signed. |
| FA-OM-5 | Does anyone else keep a tally stone? | Six settlements, one ground, one stone in the data. Whether the others keep their own is not asserted. | Living Region owner, if ever authored. |
| GB-OM-1 | Who wrote the first surviving entry? | The `hand` tag is provenance-only and opaque (DEC-GB-04). The book's first hand is the house's oldest silence. | Canon owner only, with a canon note. |
| GB-OM-2 | Why record departures when nothing consumes the record? | The book is written for a reader who is not modelled. Its dignity is the refusal to justify itself. | Never — a rule, not a gap. |
| GB-OM-3 | What happened to the guests who did not depart? | The ledger has no column for that and must never gain one. Every departure is recorded; the alternative is the player's to notice. | Never — tone-locked by DEC-GB-01's boundary. |
| GB-OM-4 | Is the lamp-filled margin mark a custom? | It appears yearly in different hands (§1c). Custom mechanics belong to evenings-and-memory-work; whether this is one is not asserted. | The EV plan's owner, if ever linked. |
| GB-OM-5 | Does the book's oldest hand belong to anyone in the region? | The name is spelled the old way; the region's naming conventions are not modelled at that resolution. | Never — texture by omission. |
