# Feature / Task Plan: The Signal Chain (a visual relay across the region) & The Listening Hour (the seventh station's authored silence)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 3, plans 2 of 4 (subjects 35–36).

> **Subjects covered (2 of the 8 in the "gatherings and signals" batch):**
> 35. **The Signal Chain** — a line of ground relay points along the region's ridges: fires by
> night, mirrors by day, patterns from a closed table, and one pattern that is not in the table.
> (Prefix `SC`.)
> 36. **The Listening Hour** — the seventh station goes silent for one hour and only receives. A
> practice, not a system: restraint as broadcast craft, and the quietest hour in the game.
> (Prefix `LH`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `.ai/plans/radio-free-ashfall-2026-09-29.md` (the station, carrier, signature, mailbag),
> `docs/plans/PLAN_B67_RADIO_CRYPTANALYSIS_CLOSEOUT.md` (triangulation intercept grid — bearing,
> not meaning), `docs/plans/UNBLOCK_EXPANSION36_NIGHT_WATCH_INTEGRATION_PLAN.md` (sound ranging;
> the Watch hears), `.ai/plans/the-sky-2026-09-29.md` (visibility context only), and the batch-2
> relief-train hook (iron-road §1.3).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — Fires Along the Ridge

> *"Radio is how the region talks. The chain is how the region says: I am still here."*

The chain is the oldest network in the valley that still works. Stone bases on the ridges, spaced
a day apart on foot, each with a fire pit and a mirror frame and a windbreak somebody built by
hand a long time ago. Nobody authored its first link. The shelter inherits it the way it inherits
the weather: as infrastructure nobody alive remembers commissioning.

A signal on the chain is not a message in the ordinary sense. It is one of a closed table of
patterns — *all is well, come, do not come, watch the road, a birth, a death, a fire that is not
ours* — and the patterns are old enough that three settlements use the same ones without having
agreed to. Crewing a link is a duty like any other. Lighting one on purpose is a decision like
few others.

The Listening Hour is the other kind of signal: the absence of one. Every night at a fixed hour
the seventh station stops transmitting and leaves its receiver open — an hour of craft silence
that the region's listeners learned about the way one learns about a neighbour's habits. Nothing
about the hour is mechanical necessity. Everything about it is practice. The station's log keeps
what the hour heard, and the log is a one-line format.

**Tone & register.** Signal-corps plain and late-night quiet. The chain's vocabulary is *link,
pattern, lit, dark, crewed, passed*; the hour's vocabulary is *carrier, log, one line*. Prose for
the chain should read like a watch commander's standing orders — terse, physical, weather-aware.
Prose for the hour should read like the inside of a dark studio: second person, present tense,
very low volume. Never write either as mysticism. The chain is stones and fuel; the hour is a
discipline.

**Mystery & texture.** Two silences carry the pair. The chain's builders are unknown and its
bases are *over-built* for their purpose — the stonework predates every settlement's own records
(§12). And the Listening Hour's log accumulates entries the shelter did not transcribe: signals
logged in the hour are recorded by format, not by author, and one of the recurring patterns is
not in the closed table (§12). Neither mystery is a quest. Both are the region being older than
its inhabitants.

**The second layer.** A network of fires is a strange kind of writing: it stores nothing, forgets
everything by morning, and binds people more tightly than any ledger. The chain's lesson is that
*attention is older than information* — someone on a ridge, watching, in all weathers, for the
one night in a thousand when the pattern says *come*. The Listening Hour is the same lesson
inverted: the station's most valuable act is not its voice but its hour of not-voice, because
silence is the only channel in which the far away can be heard at all.

---

## 1. Goal & Outcome

> *Design intent: the player should be able to light one link and understand, without a tooltip,
> that somewhere on the next ridge someone will now be watching for it.*

### 1.1 The Signal Chain (SC)

- **Goal:** An authored **Chain Row** set (5–7 relay points bound to existing location ids, each
  with a ground-truth reach neighbour), a closed **Pattern Table** (≤ 8 patterns, snake_case ids),
  a **Link Crew** duty through the existing duty-roster seam, and one verb — **Light** (consumes
  fuel through the inventory owner, visible for its authored window subject to the existing
  weather/visibility read). Received patterns surface one line on the existing board/briefing
  surface. Optionally, a crewed link lights **itself** on an authored trigger row (all dark).
- **Outcome (observable):** on a fixed seed, a crewed link on day N-1 receives the pattern lit at
  the neighbour link; the pattern arrives as one line on the board with its source link and
  window; fuel is consumed through the inventory owner; a link uncrewed for K days relays nothing
  and says so; a visibility-closed window swallows a light and the log records the attempt; with
  no chain rows every owner behaves identically to today; save/load mid-crew round-trips.
- **Non-Goals:** no radio integration (Radio Free Ashfall owns signals-as-voice); no map/travel
  change (cartography owns the map); no telegraph, no message composition, no arbitrary strings;
  no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Listening Hour (LH)

- **Goal:** One authored **Hour Row** (time, duration, profile-gated) that makes the seventh
  station's broadcast surface *decline* transmission for the window while its receive path stays
  open; a **Hour Log** of one-line entries (received pattern-id or `carrier`, source bearing if
  the triangulation owner exposes it read-only, window stamp); and one optional custom hook
  (evenings-and-memory-work seam) that lets the shelter gather for the hour. Affinity treatment
  is a proposal only (DEC-LH-02): the hour is *kept-air-time* in the fiction and must be granted
  or denied by RF's owner, never assumed.
- **Outcome (observable):** on a fixed seed, at the hour the broadcast UI shows "silence" and
  refuses a booked slot with a reason code; the log gains at most one line per hour; a received
  line carries no voice, no text body and no attribution; RF's trust/signature metrics change only
  through RF's own public methods; with no Hour Row the station behaves identically to today;
  save/load mid-hour round-trips.
- **Non-Goals:** no second radio authority; no new signal source; no decode, no cryptanalysis
  (B67 owns bearings); no horror content; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes; RF parity holds with the hour disabled; handoff lists
  untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One optional hook, shipped dark: a chain pattern lit at the far link may surface in
  the Hour Log as one line (`pattern` + window), and — separately — the iron-road relief-train
  hook may read one boolean (`chain_lit_for`) so a certified corridor *and* a lit chain can carry
  relief. Neither side reads the other's state beyond those fields.

---

## 1b. Texture, Mystery & Voice

**The Chain: attention is older than information.**

The pattern table is the plan's literature: seven one-word intentions and a windbreak. Write each
pattern as a *sentence the region can say without a language* — and let the unlisted pattern sit
in the log as a shape nobody chose. The stonework is the second voice: over-built bases, dressed
corners, a fire pit rebuilt a hundred times on foundations that never needed rebuilding.

**The Hour: restraint as craft.**

The Listening Hour is the only feature in the game whose content is *withholding*. Its panel state
is a single word. Its log format is one line. The prose must resist explaining the discipline —
the hour is kept because someone started keeping it, and that is the whole of the mechanic.

**What the player is never told.**

- Who built the chain's bases. The stonework predates every settlement's records (§12); the
  survey that would date it does not exist in any authored data.
- What the unlisted pattern means. It recurs; it is logged by shape; it is not in the table and
  must never be added to it.
- Whether the Listening Hour was started for reception or for restraint. The station's own
  history is not authored (RF discipline), and the reason is not modelled.
- Whether the hour hears anything that is *for* the shelter. The log records shapes and bearings;
  address is not part of the format.

**Voice — sample fragments (content candidates for `chain_lines.json` / `listening_hour_lines.json`).**

> "Link 4, crewed. Wind from the east, visibility long. The mirror is the easy part. The watching
> is the part." — chain standing orders (SC)

> "Pattern: do-not-come. Lit at the second bell and covered at the fourth. It was answered before
> it was covered, which is the point of the chain." — link log (SC)

> "There is a shape on the ridge at link 2 that is not in the table. It has appeared four times.
> The log keeps it as a shape. Nobody has proposed a word for it." — link log (SC)

> "21:00. Carrier down. The room is very quiet and the meter shows the quiet and the meter is the
> only listener we control." — hour log (LH)

> "21:12. Bearing north-west, one shape, unlisted. The hour is not ours. We are only here for
> it." — hour log (LH)

**Design texture beats.**

- **Patterns are a closed table forever.** The unlisted shape is texture; the moment it is named,
  the chain becomes a puzzle and the region becomes small.
- **A lit signal forgets itself by morning.** Nothing on the chain is stored beyond the log line.
  Ephemerality is the network's entire character.
- **The hour refuses, politely.** A declined booking must return a reason code and a plain line —
  the fiction is restraint, not malfunction.
- **Attribution is not in the format.** Neither log may carry a sender name. The chain has no
  letters and the hour has no callers.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the ridge leaves lying around.**

> "Fire pit, rebuilt. The stones of the pit are younger than the stones under it, and the stones
> under it are younger than the footing."

> "Mirror frame, greased at the hinge. Somebody's standing order. The order has no author and has
> never been rescinded."

> "Link log, three nights blank. The blanks are ruled. Ruling a blank is how the chain says
> 'watched, nothing to pass.'"

**What the hour leaves lying around.**

> "Hour log, one line: bearing, shape, window. The line is in pencil. Everything else the station
> keeps is in ink."

> "Booking sheet: 21:00 — silence. Booked every night for a year by the same hand, then by
> others. The booking is the custom."

**Held silences (texture, not register rows).**

- What the chain's footing was built to carry. The bases are over-built (§12); the load they were
  designed for is not in any table and must not be invented. Texture only.
- Whether the unlisted shape is one sender or several. The log records shapes and bearings;
  authorship is not part of the format and must never be added to it.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Radio: 6 NPC stations + player station; schedule grid, Voice Trust, Signature, mailbag; decline/absence semantics exist ("silence is a legible state"). | `.ai/plans/radio-free-ashfall-2026-09-29.md` §1b; `radio_stations.json`; `Radio/RadioStationCatalog.cs` | LIVE (per corpus) |
| E2 | Triangulation/intercept grid: bearing exists as an owner (B67 closeout); "bearing, not meaning". | `docs/plans/PLAN_B67_RADIO_CRYPTANALYSIS_CLOSEOUT.md` | LIVE (closeout) |
| E3 | Night Watch sound ranging and patrol readiness read surfaces; the Watch "hears everything and is not authorised to decide anything". | `docs/plans/UNBLOCK_EXPANSION36_NIGHT_WATCH_INTEGRATION_PLAN.md`; `.ai/plans/long-siege-2026-09-29.md` | LIVE (closeout) |
| E4 | Weather windows exist with visibility-affecting rows (`year_of_ash_storm_windows.json`, 14 windows; `weather_gameplay_effects.json`). | `.ai/plans/the-sky-2026-09-29.md` E10/E14 | LIVE |
| E5 | Duty-roster posts are assignable (Rounds duty precedent; `duty_roster_*.json` exists). | `.ai/plans/works-below-and-machine-in-the-walls-2026-09-29.md` §1.1; `docs/plans/PLAN_24_CLOSEOUT.md` | LIVE (per corpus) |
| E6 | Inventory transactions can carry fuel consumption for a light (fuel tallies exist in convoy/seige plans); ordinary transaction seam. | `.ai/plans/convoy-wars-and-inside-a-house-2026-09-29.md` §1c | **VERIFY (P0)** |
| E7 | Location id namespaces (`location_*` / `loc_*`) can host relay points; reach adjacency between two locations is derivable or authorable. | `.ai/plans/second-nature-and-ruins-of-the-before-2026-09-29.md` findings | **VERIFY (P0)** |
| E8 | RF broadcast surface can decline a booked slot with a reason code without editing RF's schedule owner. | RF plan §1/§1b (booking at player's own slot) | **VERIFY (P0)** |
| E9 | Evening custom machinery can host one authored custom as an optional hook (EV custom seam). | `.ai/plans/evenings-and-memory-work-2026-09-29.md` §1.1 | PROPOSED (soft dependency) |
| E10 | Additive nested DTO checksum-safety in the RF/duty save owner for the Hour Log and crew state. | codec / snapshot tests | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Broadcast, schedule, trust, signature | `RadioStationSystem` family (RF) | one decline-with-reason affordance at the hour; no other change |
| Bearings | triangulation owner (B67) | read-only; a bearing may appear in a log line |
| Visibility | weather owners | read-only; a closed window swallows a light |
| Crew duty | duty-roster owner | one post type ("link crew") |
| Fuel / costs | inventory owner | Light consumes through ordinary transactions |
| Pattern grammar | — | `SignalPatterns` (pure Core, closed table + validator, zero runtime state) |
| Chain / hour state | — | `ChainLinks` + `HourLog` (pure Core) nested additively in the duty/RF save owner — **DEC-SC-05 / DEC-LH-03** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Signals/SignalPatterns.cs` (new, pure), `Signals/ChainLinks.cs` (new, pure), `Signals/ListeningHour.cs` (new, pure)
**Data:** `signal_patterns.json`, `signal_chain_links.json`, `listening_hour_rows.json`, `chain_lines.json`, `listening_hour_lines.json`
**Host:** RF host session (`INT`), duty-roster registration (`INT`), day-owner registration (`src/Main.CampaignOwners.cs`, `INT`)
**Presentation:** one "Chain" band on the existing board/briefing surface; one "silence" state and hour log line on the RF surface — no new routed panel
**Tests:** `Ashfall.Core.Tests/Signals/SignalPatternsTests.cs`, `ChainLinksTests.cs`, `ListeningHourTests.cs`, `Ashfall.Core.Tests/Save/SignalsSaveTests.cs`

## 5. Packages

### SC-P0 — Premise audit (Auditor; read-only): close E6–E10; confirm the RF decline affordance and the EV custom seam wording; confirm location reach adjacency authoring.
### SC-P1 — Pattern table + chain links (Core + data): closed patterns, authored links, reach rules, validator with row-level failures. **Accept:** determinism; closed table enforced; ship-dark with no rows.
### SC-P2 — Light verb + crew duty (host): fuel through inventory; one duty post; visibility read. **Accept:** conservation; uncrewed links relay nothing and say so.
### SC-P3 — Board band + trigger rows (host + data): one-line surfaces; optional authored auto-light triggers, all dark. **Accept:** no write to any signal authority; no string escapes the table.
### LH-P1 — Hour row + decline (Core + RF host, `INT`): window, decline-with-reason, receive stays open. **Accept:** RF metrics move only via RF public methods; parity when disabled.
### LH-P2 — Hour log + optional custom hook (Core + host): one line per hour, no attribution; custom hook shipped dark. **Accept:** format invariant (pattern/bearing/window only); no custom → no gathering.
### X-P1 — Seams (host, shipped dark): pattern→hour-log line; `chain_lit_for` boolean to the iron-road relief hook. **Accept:** fields documented; no other coupling.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no chain rows and no hour row → radio, duty, weather, inventory and board outputs identical on a saved corpus.
3. Conservation: fuel consumed equals fuel spent; nothing else is created, moved or destroyed by a light.
4. Determinism: identical pattern tables, relay orderings and hour-log contents on replay (`CampaignStreamIds` fork; never `System.Random`).
5. Save round-trip mid-crew and mid-hour; old saves load; additive DTO checksum-safe (E10 closed at P0).
6. Format invariants hold: no log line anywhere carries attribution, free text or a pattern outside the closed table.
7. RF parity: with the hour disabled every RF behaviour is bit-identical; with it enabled, only RF's own public methods change RF state (DEC-LH-02 resolved or declined at P0).
8. The unlisted shape is never named, never translated, never added to the table (DEC-SC-04).

## 7. Cross-plan boundaries
- **Radio Free Ashfall:** one decline affordance and one log line; RF owns voice, trust, signature, mailbag. The Listening Hour has no voice of its own.
- **The Long Siege / iron-road (relief train):** one boolean (`chain_lit_for`) to the existing hook shape; shipped dark until both ends exist.
- **Night Watch (Exp. 36):** the Watch hears; the chain sees. No shared state; a watch report may quote a chain line as data.
- **The Quiet War:** the chain is a channel, and channels can be watched; whether anyone watches is not modelled here and left to QW's discipline.
- **The Sky:** visibility context only; the Harrow never touches the chain.
- **Year Two:** the hour log may feed the chronicle as a modifier input, gated as ever.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-SC-01 | The pattern table is closed and permanent (≤ 8 patterns); validator rejects any row outside it. | rule | Yes |
| DEC-SC-02 | Relay is reach-bound: a link receives only from its authored neighbour; no transitive forwarding in v1. | rule | Yes |
| DEC-SC-03 | Lit signals are ephemeral: nothing persists beyond one log line. | architecture | Yes |
| DEC-SC-04 | The unlisted shape is logged by shape only; no id, no name, no translation, ever. | tone | Yes — **needs canon owner note** |
| DEC-SC-05 | Chain state nests additively in the duty-roster save owner; no new save section. | architecture | Yes; confirm in P0 |
| DEC-LH-01 | The hour is one authored row per profile; decline is polite, explicit and reason-coded. | design | Yes |
| DEC-LH-02 | Whether the hour counts as kept-air-time for RF affinity is RF's owner's decision, not this plan's. | boundary | **Needs RF owner ruling** |
| DEC-LH-03 | Hour log nests additively in the RF save owner; format is pattern/bearing/window only. | architecture | Yes; confirm in P0 |
| DEC-LH-04 | The hour's origin is never narrated; the practice is its own explanation. | tone | Yes |
| DEC-X-01 | Both seams ship dark; each exposes exactly one boolean/string. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Signal`, `Beacon`, `Chain`, `ListeningHour`, `SilentHour`)
- [ ] Premise re-verified (Rule 7); RF/B67/Night-Watch evidence re-read; no overlapping live claim on radio, duty or weather paths
- [ ] Signed decisions in hand (at minimum DEC-SC-01, DEC-LH-01; DEC-LH-02 ruled)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing radio, duty-roster and weather-window tests (list from P0 selector)
- [ ] RF host selftest with the hour row on and off (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: the RF decline affordance cannot be added without editing RF's schedule owner; a chain line cannot be kept free of attribution by construction; the pattern table cannot be closed without a string field anywhere in the seam; the hour cannot be persisted without a new save section; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the chain longer than the map and the hour quieter than the player's
curiosity. Any future plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| SC-OM-1 | Who built the bases, and for what load? | The stonework is over-built and unattributed (§12). Dating it would turn infrastructure into history with owners. | Canon owner only, as a signed decision. |
| SC-OM-2 | What does the unlisted shape say? | It recurs and is logged by shape (DEC-SC-04). Naming it converts a presence into a message, and the region shrinks by one mystery. | Never — deliberately sealed. |
| SC-OM-3 | Why do three settlements share one pattern grammar without agreeing to it? | The table is authored as common; convergence, inheritance and coincidence are all consistent with the data and the plan refuses to choose. | Never — texture by omission. |
| SC-OM-4 | Does anyone else crew the far links? | Reach is authored per link; beyond the last authored link the chain is not modelled and must not be. | A named expansion of the table, signed, row by row. |
| SC-OM-5 | What would make the shelter light `come`? | The pattern exists in the table; no authored situation in this plan uses it. Its unused weight is the point. | A content pass that names its decision. |
| LH-OM-1 | Who started the Listening Hour, and why that hour? | DEC-LH-04 keeps the practice self-explanatory; the station's history is RF's and RF does not narrate it. | Canon owner only, with RF's owner. |
| LH-OM-2 | What does the hour hear on the nights it hears nothing? | The log format records presence; absence leaves no line and must never gain one. | Never — a rule, not a gap. |
| LH-OM-3 | Is the unlisted shape in the hour log the same as the chain's? | Both are logged by shape only (DEC-SC-04, DEC-LH-03). The plan observes the echo and refuses to link them. | Never — the pair must stay unresolved together (mystery-index §3 discipline). |
| LH-OM-4 | Does anyone else keep an hour of silence? | Six stations, one hour in the data. Whether the others have habits is not asserted. | Radio Free Ashfall's owner, if ever authored. |
| LH-OM-5 | Why is the hour's line in pencil? | Texture (§1c). The station writes in ink except here; no rule explains it and none may be invented. | Never — texture by omission. |
