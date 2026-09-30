# Feature / Task Plan: Crews and Companions — expeditions become parties with named crew and animals

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_crews_and_companions_plan.md`. Family index: `docs/expansions/expansion_new_ways_to_play_index.md`.
> Not a claim. `ExpeditionSystem` is used in many places; this plan **adds a coordinator beside it** and does not change its one-expedition-per-survivor rule.

> **Editorial polish (prose pass):** sections **0**, **1b**, **1c** and **12** are narrative texture only. No
> authority, claimed path, decision, acceptance criterion or verification step changes. Sample lines
> are content candidates for `party_quarrels.json` / `camp_rituals.json` / `companion_party_moments.json`
> rows; they belong in data, never in code.

---

## 0. Prologue — The Party

> *"A shelter is what you are willing to leave. A party is who you are willing to leave it with."*

An expedition is a ledger entry: a survivor, a location, a stamina cost, an outcome. A **party** is
what happens when four of those entries decide to be about each other. Nothing in the engine
requires it. That is exactly why it matters.

The road in this world does not have encounters so much as it has *appointments* — and a party is
the decision to keep them in company. There is a hound that will not leave the lead's heels, a
medic who sleeps badly and works anyway, a hauler who has not spoken since the third day, and a
fire that is either a camp or a ritual depending on who is still awake to call it one.

**Tone & register.** Warm, worn, unsentimental. The vocabulary is the trail: *watch, pace, rations,
bond, quarrel, camp*. Prose should feel like a conversation carried on at walking pace — half
finished sentences, the same joke told badly twice, a long silence that is not unfriendly. Never
write the party as a unit. Write it as four people and two animals who happen to be going the same
way.

**Mystery & texture.** The plan's real subject is the **road bond** — the write that happens when
something extreme occurs and the relationship system records it without being asked. Bonds are not
rewards and are not earned; they are *damage with witnesses*. §12 keeps open the questions that a
group of people and animals walking into the ash will always raise and never answer.

**The second layer.** Four ledger entries decide to be about each other, and the engine does not
require it — which is the entire reason it registers as grace. The party is the one institution in
this world that exists purely because someone chose company over efficiency: one encounter roll
instead of four, a slower pace, a shared watch, a fire that costs fuel and buys nothing measurable.
The bonds it writes are damage with witnesses. Nobody earns them. They happen to people who
happened to be walking together when the world did something. Company over efficiency is the only
arithmetic in this world that ever rounds up.

## 1. Goal & Outcome

> *Design intent: a party of one is an expedition. A party of two is a promise. Everything in this
> plan is about the difference.*

- **Goal:** A **party** = up to 4 survivors + up to 2 companions travelling together, implemented as *N ordinary expeditions plus a small party coordinator*: shared destination/pace/outcome, roles, a cohesion number, seeded injury/loss distribution, camp rituals, and road-bond writes through existing social systems. Provide one **crew contract** other plans read.
- **Outcome (observable):** on a fixed seed a party of three plus a hound dispatches to a catalog location; exactly **one** encounter is rolled per tick for the party (not per member); a medic role changes an injury outcome; camp raises cohesion; an event writes a trauma bond through the existing system; a lost companion produces the existing grief effect; a party of one behaves identically to today; save/load mid-trip preserves everything.
- **Non-Goals:** no change to `ExpeditionSystem`'s one-per-survivor rule or start path; no new stamina/inventory/health authority; no new save section; no new routed panel; no boats/wagons (crew *contract* only); no squad tactics; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; solo-expedition parity holds; handoff lists untouched shared paths.

## 1b. Texture, Mystery & Voice

**Cohesion is not morale.**

Cohesion is 0–100 and derived from relationships and shared history. It is a *weather report on a
small group* — and like a weather report it should be legible, unarguable, and slightly too late to
act on. Low cohesion enables quarrels. It does not cause them. Keep that distinction visible in the
panel: cohesion describes, it does not prescribe.

**Animals are members, not equipment.**

DEC-CC-07 says a companion never starts alone and the handler must be present. Lean all the way
into that in the writing. A hound with a role and a temper and a bond is a character; a hound with
a pack-capacity bonus is a container. `companion_party_moments.json` should carry the former.

**What the player is never told.**

- Why followers' encounter chance is exactly 0 and not merely low. The plan suppresses the roll via
  the existing hook (DEC-CC-04). Whether the road is *quieter* or merely *unobserved* is left alone.
- Who the road bond is between. `TraumaBondSystem` records a write. It does not record who was
  looking at whom when it happened.
- Whether the party's name is the party's idea or the player's. `Party.name` is free text and the
  plan declines to attribute authorship.
- What a lost companion's grief is *for*. It is the existing effect, unmodified. Never narrate past
  it.

**Voice — sample fragments (content candidates for `party_quarrels.json` / `camp_rituals.json`).**

> "Camp, day six. We said the words because we have always said the words. It was the only thing
> today that did not change."

> "The quarrel was about the watch order. It was not about the watch order."

> "The goat will not drink here. Three of us have stopped arguing with the goat."

> "He carried the pack the whole way and did not say so, which is how you find out who a person is
> on a road where nothing else is interesting."

**Design texture beats.**

- **One encounter roll per party per tick is the plan's whole architecture — and its best story
  device.** It says: *this happened to us*, not *this happened to me four times*.
- **Roles are verbs, not classes.** Pathfinder, hauler, watch, medic each feed an existing hook.
  Name them in the UI as things people *do*.
- **Loss is drawn, not chosen (DEC-CC-05).** The seeded weighted draw is the plan's cruellest
  honest feature. Do not let the UI pre-announce it.
- **A party of one must be invisible.** Solo parity is a test in §6.2 — and it is also a promise that
  nothing here is mandatory.

---

## 1c. The Deeper Layer — scenes, artifacts & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the shelter leaves lying around.**

> "Watch order, rewritten twice. The second rewrite is in a different hand and the first hand has not objected."

> "Camp words, said because we have always said the words. Nobody remembers who started. The words have outlived the remembering."

> "Pack tally: he carried it the whole way. The tally does not record that. The tally records weight."

**Scenes the player may piece together.**

> "The goat will not drink here. Three of us have stopped arguing with the goat, which the cohesion number will describe tomorrow, too late."

> "Day nine silence. Not unfriendly. On a road where nothing else is interesting, a person can be found out quietly."

**Held silences (texture, not register rows).**

- Whether the road is quieter for followers or merely unobserved (DEC-CC-04). The encounter chance is suppressed exactly to 0 and the plan leaves the difference alone. Texture only.
- Who the road bond is *between*. `TraumaBondSystem` records a write; it does not record who was looking at whom, and the write must never be made to explain itself.

**Fourth pass — the fire that costs fuel and buys nothing (texture only; §12 register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §12 gains no row and loses no silence;
fragments remain content candidates for `party_quarrels.json` / `camp_rituals.json` /
`companion_party_moments.json` and gain no schema where no prose field exists.)*

**The shape of the polish.** Write the party at walking pace and let sentences arrive half-finished,
the way they do when the speaker is saving breath for the hill. The plan's grace is that nothing
here is required by the engine — so the prose must never sound rewarded. A road bond is damage with
witnesses; the witnesses are the only warmth in the sentence and they must never be quoted agreeing
with each other.

**What the shelter leaves lying around.**

> "Camp words, night 7. Two people now say them in the same wrong order. Neither has corrected the
> other."

> "Watch chit, day 7: the goat is listed as personnel on one line and as cargo on none."

> "The hound slept across the threshold. The threshold is not a door on a road. The hound has
> opinions about doors."

**Held silences (texture, not register rows).**

- Who named the party. `Party.name` is free text and the plan declines to attribute authorship
  (§1b); the decline is load-bearing and must stay. Texture only.
- What the one suppressed encounter roll would have been. Followers' chance is exactly 0
  (DEC-CC-04); the road that did not happen is the only road nobody walked and it must stay
  unwritten.

---

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | One expedition per survivor; `_active` keyed by `survivorId`; `Start(...)` refuses a second. | `Expeditions/ExpeditionSystem.cs` L400–454 | LIVE |
| E2 | Query hooks: stamina-drain multiplier, survivor speed multiplier, pack-capacity bonus, encounter-chance multiplier (all `Func<string,float>`). | `ExpeditionSystem.cs` L349–387 | LIVE |
| E3 | Camp API: `EnterCamp`, `ReserveCampSupplies`, `CampTick`, `ResolveCampEncounter`. | `ExpeditionSystem.cs` L842–1050 | LIVE |
| E4 | Companion system: 5 species, roles Guard/Pack/Morale, one handler, bond, care, sickness, guard/pack/morale/grief queries; section `companion_animals`. | `Ecology/CompanionAnimalSystem.cs`; `companion_animals.json`; `Save/SaveSectionRegistry.cs` L289 | LIVE |
| E5 | Host flips companion `on_expedition` when its handler has an active expedition. | `src/Main.Companion.cs` L214–225 | LIVE |
| E6 | `crew_min/crew_max` on naval vessel defs unread. | `naval_vessels.json`; grep | GAP |
| E7 | Trauma bonds, relationship decay, survivor roles, skills, social coordinator. | `Survivors/TraumaBondSystem.cs`, `RelationshipDecaySystem.cs`, `SurvivorRoleSystem.cs`, `SurvivorSocialCoordinator.cs` | LIVE (VERIFY write APIs) |
| E8 | Encounter choice resolution per survivor via bridge. | `Expeditions/ExpeditionEncounterBridge.cs`; `src/Host/ExpeditionHostSession.cs` L1155 | LIVE (VERIFY choice API, how encounters are rolled per tick) |
| E9 | Duty roster governs availability. | `DutyRoster/DutyRosterSystem.cs` | LIVE |
| E10 | 75 expedition locations. | `expeditions.json` | LIVE |
| E11 | Whether N simultaneous solo expeditions to the same location produce N encounter rolls per tick (the double-count risk). | `ExpeditionSystem.TickHours` L751 | **VERIFY (P0, critical)** |
| E12 | Panels: `ExpeditionPanel`, `ExpeditionCampPanel`. | `src/UI/` | LIVE |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Expedition state (per survivor) | `ExpeditionSystem` | none; N ordinary starts |
| Companion state/care/grief | `CompanionAnimalSystem` | read; role at party level stored in party DTO |
| Relationships/trauma bonds | existing social systems | writes only via their public APIs |
| Availability | duty roster | consumer |
| Party grouping, roles, cohesion, seeded loss draw | — | `PartyCoordinator` (pure Core), nested `parties[]` in the `expeditions` save DTO — **DEC-CC-02** |
| Encounter roll suppression for followers | via E2 hook | a per-survivor query that returns 0 for followers |
| Crew contract | — | `CrewContract` value type, read-only for DC/LF |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Expeditions/PartyCoordinator.cs` (new, pure), `Expeditions/PartyRoles.cs` (new), `Expeditions/PartyCohesion.cs` (new), `Expeditions/CrewContract.cs` (new), `Expeditions/ExpeditionSystem.cs` (additive nested DTO field only; **no logic change**), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`)
**Data:** `party_roles.json`, `camp_rituals.json`, `party_quarrels.json`, `companion_party_moments.json`, `expedition_encounter_variants.json` (additive role-gated options)
**Host:** `src/Host/ExpeditionHostSession.cs` (`INT`, dispatch + encounter routing), `src/Main.Companion.cs` (`INT`, presence sync), one day-owner registration if needed (`src/Main.CampaignOwners.cs`, `INT`)
**Presentation:** existing `ExpeditionPanel`, `ExpeditionCampPanel` (**DEC-CC-06**)
**Tests:** `Ashfall.Core.Tests/Expeditions/PartyCoordinatorTests.cs`, `PartyCohesionTests.cs`, `CrewContractTests.cs`, `Ashfall.Core.Tests/Save/PartySaveTests.cs`; extend expedition and companion tests

## 5. Packages

### CC-P0 — Premise audit (Auditor; read-only) — **critical: E11**
- Prove or disprove E11 (N solo expeditions → N encounter rolls); enumerate every caller of `ExpeditionSystem.Start`, `Active`, and the four query hooks (who already sets them); confirm relationship/trauma-bond write APIs (E7); confirm encounter-choice API per survivor (E8); foreman signs DEC-CC-01…10.
- **Accept:** E11 answered with a test or `path:line`; the hook owner conflict (if any) identified before design.

### CC-P1 — Party model (Core, pure + nested DTO)
- `Party { partyId, name, memberIds[≤4], companionIds[≤2], leadId, roles, cohesion, startedDay }`; validation (free on roster, handler present for each companion, one party per survivor); nested in the `expeditions` DTO, additive, default empty.
- **Accept:** round-trip; old saves load; a party of one writes nothing extra; validation rejects all invalid parties with reasons.

### CC-P2 — Dispatch as N ordinary starts (Host, `INT`)
- Dispatch calls `ExpeditionSystem.Start` once per member with the same definition and coordinated stance/night/vehicle; followers get an encounter-chance multiplier of 0 via the existing hook; lead's multiplier scaled by party size (data).
- **Accept:** exactly one encounter roll per tick for the party (fixed-seed test); a party of one identical to a solo start (parity test); no change to `ExpeditionSystem` logic.

### CC-P3 — Roles & query providers (Core)
- Roles from skills/role system; roles feed the *existing* hooks: pathfinder → speed, hauler/goat → pack-capacity, watch → encounter multiplier, medic → injury outcome.
- **Accept:** each role's effect is bounded and table-driven; roles absent → neutral multipliers.

### CC-P4 — Cohesion & camp rituals (Core + content)
- Cohesion derived from relationships + shared history; camp rituals (data) with cohesion deltas via camp choice machinery; low cohesion enables quarrels.
- **Accept:** deterministic; cohesion bounded 0–100; rituals never bypass camp authority.

### CC-P5 — Party encounters, injury & loss (Core + host)
- Encounter resolved once at party level with role-gated options (additive variants); injury/loss target chosen by a **seeded** weighted draw (stream via `CampaignStreamIds` fork keyed `(day, partyId, tick)`); death → survivor legacy; companion loss → existing grief.
- **Accept:** same seed → same target; medic changes outcome; survivor and companion conservation (members before = members after + recorded losses).

### CC-P6 — Companions as members (Core + host)
- Companion party presence (role, terrain fit, temper, bond-gated loyalty); `on_expedition` continues to be driven by handler presence; companion risk is party-level only.
- **Accept:** a companion never starts an expedition alone; handler absent → not in party; bond-gated behaviour deterministic.

### CC-P7 — Road bonds (Core via existing APIs)
- Extreme events write to trauma-bond/relationship systems through their public APIs; ledger line for each write.
- **Accept:** no direct mutation of relationship state; each write has one journal line.

### CC-P8 — Crew contract (Core, read-only)
- `CrewContract` produced from a party or a roster subset; consumed by *The Drowned Coast* (boat crew_min/max) and *The Long Line: Freight* (driver/escort) as read-only.
- **Accept:** no consumer writes party state; contract fields validated against vessel/wagon requirements.

### CC-P9 — Presentation
- Extend `ExpeditionPanel` (party builder, role picks, cohesion) and `ExpeditionCampPanel` (rituals). Focus/back preserved.
- **Accept:** presenter tests; panels hold no authority.

### CC-P10 — Content waves W1–W5 & governance close.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Solo parity: a party of one → `--expedition-selftest` and expedition/companion tests unchanged.
3. One encounter roll per party per tick.
4. Determinism: identical loss targets and quarrels on replay.
5. Save round-trip mid-trip with a party, a companion, and cohesion.
6. Conservation of members and companions across every outcome.
7. No writes to relationship/trauma/companion/legacy state except through public APIs.

## 7. Cross-plan boundaries
- **The Drowned Coast:** boat crews read the crew contract (`crew_min/crew_max`); DC owns hulls/berths.
- **The Long Line: Freight:** wagon crews read the crew contract (driver, escort); LF owns runs.
- **The Quiet War:** a returning party may bring a stranger through the shared gate adapter.
- **Radio Free Ashfall:** party check-ins are short broadcasts; silence past a threshold raises alarm (read-only).
- **The Plague Year:** party illness through existing disease exposure; parties returning from an outbreak region trigger Screen.
- **Shelter Governance:** crew conscription is a policy scope; parties never override duty law.
- **The Reconstruction Tree:** expedition finds route as fragment sources through existing seams; a party's Pathfinder/Medic disciplines count as bearers only via skills.
- **Year Two:** apprentices may join as *trainee* (non-lead); children never.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-CC-01 | A party is N ordinary expeditions + a coordinator; one-per-survivor stays. | architecture | Yes |
| DEC-CC-02 | Party state nests in the `expeditions` DTO. | architecture | Yes; confirm in P0 |
| DEC-CC-03 | Party cap 4 survivors + 2 companions. | scope | Yes |
| DEC-CC-04 | Encounters roll once per party (lead), followers suppressed via existing hook. | design | Yes; depends on E11 |
| DEC-CC-05 | Loss target via seeded weighted draw; medic role changes outcome. | design | Yes |
| DEC-CC-06 | No new routed panel; extend expedition/camp panels. | UI | Yes |
| DEC-CC-07 | Companions never start alone; handler presence required. | rule | Yes |
| DEC-CC-08 | One crew contract shape for boats and wagons. | architecture | Yes |
| DEC-CC-09 | Road bonds only through existing social APIs. | architecture | Yes |
| DEC-CC-10 | Children never join; apprentices as trainees only (if Year Two present). | rule | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Party`, `Crew`, `Cohesion`)
- [ ] Premise re-verified (Rule 7); ledger files re-read; no overlapping live claim (expedition and companion paths are heavily shared)
- [ ] Signed decisions in hand

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing expedition/companion/trauma-bond tests (list from P0 selector)
- [ ] `--expedition-selftest` and companion selftests (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: E11 shows follower encounter suppression cannot be done through the existing hooks; hook ownership conflicts with another setter; a party would require a second start path or a change to `ExpeditionSystem` logic; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered** — not gaps, not TODOs, not deferred work. They
keep the road larger than the party walking it. Any future plan that answers one must name the
signed decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| CC-OM-1 | Where did the five companion species come from? | E4 authorises species, roles, bond and grief. It never authorises provenance. A hound with a backstory is a quest; a hound with a bond is a companion. | Never — texture by omission. |
| CC-OM-2 | Why is a follower's encounter chance exactly zero? | DEC-CC-04 suppresses the roll through the existing hook. Whether the road is quieter or merely unobserved is left to the player. | Never — the abstraction is the design. |
| CC-OM-3 | Who names the party? | `Party.name` is free text and authorship is not recorded. | Never — a rule, not a gap. |
| CC-OM-4 | What is a road bond between a person and an animal? | CC-P7 routes writes through existing social APIs. Whether the relationship system knows the difference is not asserted. | The social owners, if they ever expose a bond kind. |
| CC-OM-5 | Do parties that never came back have names? | 75 locations exist (E10); expeditions that failed are not archived as parties. The road keeps no roll of honour. | *The Record Keepers*, if a memorial ever accepts a party. |
| CC-OM-6 | Why do camp rituals work? | CC-P4 gives cohesion deltas through camp choice machinery. The mechanism is bounded; the meaning is not authored. | Never — DEC-CC-09's boundary. |
