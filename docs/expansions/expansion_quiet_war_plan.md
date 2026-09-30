# ASHFALL — THE QUIET WAR
### Espionage, informants and door visitors who aren't who they claim · The war fought at the airlock

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/quiet-war-2026-09-29.md`
**Family:** "New ways to play" — `docs/expansions/expansion_new_ways_to_play_index.md`.
**Name note:** Expansion 41 *The Quiet* (sleep/soundproofing, sealed) is unrelated; ids here are `quiet_war` / `QW-`.
**Tone lock (inherited):** cold, exhausted, human, restrained. No real agencies, wars, people or copied text. No glorified violence; no torture set-pieces — interrogation is a decision with a cost, not a scene.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.

---

> *"Everyone who comes to the door has a story. That is not the same as having a life."*
>
> There is a moment — before the bolts are drawn — when the shelter is entirely certain about a
> stranger, and entirely wrong. This expansion never tells you which moment that is. It teaches you
> to be wrong *consistently*, and then it lets you find out what that consistency cost.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

Someone knocks. The camera shows a man in stripped winter gear holding a coughing child and a sealed filter, asking for a five-day lease on a warm corner. The game has eighty such knocks authored, each with choices, standing deltas and consequences. It also has ten *infiltrator profiles* — a saboteur who asks about power, a logistics spotter, a false defector, a water-poisoner — each with a deception skill, a loyalty mask, behaviour flags, forged credentials, a confession threshold and a defector probability. And it has a counter-intelligence system that can vet, interrogate, detain and turn.

**They have never met.** The door does not produce agents. The agents do not arrive by the door. The airlock can already say *Admit, Inspect, Quarantine, Turn away, Defend*, and none of those verbs has ever been the wrong answer for a reason the player could not see coming.

The Quiet War joins them. Every visitor now has a **Claim** (what they say) and a **Truth** (what they are), and the player's only tools are the ones a real shelter has: **time, questions, paper, a watcher, and each other's word.** Some people are exactly who they say. Some are worse. Most are neither — they are frightened people with one lie each.

The promise: **you will learn to be wrong about someone, and to decide what being wrong is allowed to cost.**

What is actually at stake is not espionage. It is the slow, corrosive discovery that the player's
own judgement is a **measurable, wear-prone instrument** — that the shelter has been running an
uncontrolled experiment on its own capacity to read a face, and that some of the people it already
let inside were never theirs.

Notice what the tools are: **time, questions, paper, a watcher, and each other's word.** No
scanner. No probability readout. No tag that says *agent*. A real shelter does not have those, and
the moment one is added the interview room becomes a menu. Every verb in §3.3 is something a person
can actually do to another person at a threshold in winter.

### 1.2 Pillars

1. **A claim is a statement, not a fact.** Every visitor has one; the game knows the truth and does not say.
2. **Suspicion is a resource — for both sides.** Vetting spends time and goodwill; a false accusation spends more.
3. **Tells, not tags.** The player reads behaviour flags and credentials, not labels.
4. **The war is quiet.** It is fought in door hours, dead drops and radio bursts — not gunfire.
5. **Most lies are small.** A desperate liar and a saboteur look alike for a week.
6. **Ship dark.** With no claim/truth data present, the airlock and door behave exactly as today.

### 1.3 Not this

Not a social-deduction minigame. No mind-reading. No mandatory torture. No replacement of Airlock, Door, Visitor or Counter-intelligence systems. No new faction.

---

## 2. What the code and data actually say (audit)

| # | Finding | Evidence | Status |
|---|---|---|---|
| F1 | **The airlock**: door state (Secure/Cycling/Open/Breached), sentry, `VisitorArrives(visitorId, visitorType)`, `ResolveIncident(VisitorDecision)` with **Admit / Inspect / Quarantine / TurnAway / Defend**, incident log, door repair, daily tick. Host session, panel, save section `airlock_security`. | `Assets/Ashfall.Core/AirlockSecuritySystem.cs`; `src/Host/AirlockSecurityHostSession.cs`; `src/UI/AirlockSecurityPanel.cs`; `Save/SaveSectionRegistry.cs` L113 | LIVE |
| F2 | **Door encounters**: 80 authored entries (visitor name, faction, description, day window, threat level 0–4, choices with required trait/item, morale/guilt/standing deltas, granted items). Resolved through `DoorEncounterSystem`, hosted by the Year of Ash session; `Main.YearOfAsh` resolves against a **`DemoRoster`**. | `door_encounters.json`; `YearOfAsh/DoorEncounterSystem.cs`; `src/Main.YearOfAsh.cs` L382–411 | LIVE / **VERIFY** (demo roster vs live roster) |
| F3 | **Visitor integration**: admit a stay, housing, processing tasks, **monitoring level** (None/Low/Medium/High), recruit, daily tick; a record carries `SourceVisitorId` — *"for example an AirlockSecuritySystem admission visitor id"* — for idempotent handoff. 5 authored visitor templates. Section `visitor_integration`; panel. | `Visitors/VisitorIntegrationSystem.cs` L115–139; `visitor_templates.json`; `src/UI/VisitorIntegrationPanel.cs` | LIVE |
| F4 | **Counter-intelligence**: register infiltrator profiles; `VetCandidate(candidateId, officerId)`, `Interrogate`, `DetainSuspect`, `AcceptDefector`, `ResolveSabotage`, daily tick; **undercover agents keyed by `survivorId`**. Host session, section `counter_intelligence`. | `Factions/CounterIntelligenceSystem.cs`; `src/Main.FactionBranch.cs` L45–49 | LIVE |
| F5 | **10 infiltrator profiles** (Cutter saboteur, Compact informant, Flotilla smuggler, rebel agitator, raider sleeper, false defector, logistics spotter, water poisoner, signal interceptor, embedded recruiter) with deception skill, loyalty mask, behaviour flags, forged credentials, sabotage targets, confession threshold/triggers, defector probability. | `infiltrator_profiles.json`; `Factions/InfiltratorCatalog.cs` | LIVE |
| F6 | `VetCandidate` has **no caller under `src/`** (tests only); nothing generates a candidate from a door visitor. | grep `src/` | **GAP (the central one)** |
| F7 | **Informant network**: the *player's* assets — archetype, target faction, method (dead drop / direct briefing / radio burst / cutout courier), loyalty, suspicion, yield, compromised, double agent; recruit/retire/run operation/sweep/interrogate captive. Host session, section `informant_network`. | `Espionage/InformantNetworkSystem.cs`, `InformantNetworkTradecraftEngine.cs`; `src/Host/InformantNetworkHostSession.cs` | LIVE |
| F8 | **Faction intelligence operations**: 8 ops (target faction/subsystem, suspicion per tick, cooldown, detection difficulty) + 4 dead-drop templates; `ShelterEspionageSystem` and `EspionageConsequenceRouter`. | `faction_intelligence.json`; `Factions/ShelterEspionageSystem.cs`, `EspionageConsequenceRouter.cs`; `src/Main.ShelterEspionage.Integration.cs` | LIVE |
| F9 | Accusation machinery exists on the verdict side. | `Verdict/VerdictAccusationSystem.cs` | LIVE (VERIFY reach) |
| F10 | No hidden identity on a door visitor: the door encounter's `visitorFaction` is its only identity. | `door_encounters.json` | GAP |
| F11 | Selftests: hidden-agenda self-test, visitor-integration self-test, counter-intelligence tests. | `src/Host/HiddenAgendaSelfTest.cs`, `VisitorIntegrationSelfTest.cs` | LIVE (VERIFY args) |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

- **Three "front doors" (F1, F2, F3).** Airlock, door encounters, and visitor integration all describe strangers arriving. Systems win: none is replaced. The plan makes the **airlock the decision point** (its verbs already match: *Inspect*, *Quarantine*), door encounters the **authored story** for a knock, and visitor integration the **stay**. *The Living Region* and *The Plague Year* need the same seam; the family index records **one shared gate adapter**.
- **Door encounters resolved against a `DemoRoster` (F2).** If the live roster is not what the encounter sees, no claim can be about a real survivor. P0 must settle this before anything else.
- **Agents keyed by `survivorId` (F4).** A visitor has no survivor id until recruited. Systems win: truth is attached to the **visitor id**, and converts to an undercover-agent record *only* when the visitor becomes a survivor.

---

# PART II — THE STORY

## 3. The Claim and the Truth

### 3.1 The record

Every arrival gets a **Claim** and a hidden **Truth**:

| | What it holds |
|---|---|
| **Claim** | name, stated faction, stated reason, offered credential (a forged paper, a stamped token, a name to vouch for), stated need |
| **Truth** | one of: **Honest** · **Desperate liar** (one small lie) · **Agent** (an authored infiltrator profile) · **Defector-in-waiting** · **Bait** (sent to be turned away, to measure the shelter) |

The Truth is **derived deterministically** from the seed and world state — *faction pressure* (who is at war with whom), *regional condition* (the Living Region: a Failing region sends desperate people and few agents), *station Signature* (Radio Free Ashfall: a shelter that broadcasts attracts attention), and *outbreak* (Plague Year: a sick visitor is more likely honest). It is **not** hand-authored per visitor, but it is drawn *from* the authored profiles and door encounters — the ten profiles are the agent pool.

### 3.2 The tells

Truth leaks through **behaviour**, not labels. The profile's own data supplies the tells:

| Profile field | Becomes |
|---|---|
| `behavior_flags` (e.g., *avoids the watch*, *asks about power*) | what the visitor does in the first three days |
| `forged_credentials` | a paper that can be checked, if the player has a way |
| `deception_skill`, `loyalty_mask` | how hard the tell is to notice |
| `confession_threshold`, `confession_triggers` | what breaks them (e.g., *evidence cache found*) |
| `defector_probability` | whether a caught agent can be turned |

### 3.3 The player's tools (verbs, all existing)

| Verb | Backed by | Cost |
|---|---|---|
| **Inspect** | airlock `Inspect` | time; goodwill |
| **Ask** (a short interview with authored questions) | new content on choice machinery | a day's attention |
| **Check the paper** | credential check against dossier (see §4) | a contact or a trade |
| **Watch** | visitor monitoring level | a watcher's duty hours |
| **Set a condition** | visitor processing tasks | the visitor's patience |
| **Vet** | `VetCandidate` (officer) | an officer's day |
| **Bait** | a planted false fact (dead-drop template) | a prepared lie |
| **Detain / Quarantine / Turn away** | airlock + counter-intelligence | standing; a possible wrong |

### 3.4 What being wrong costs

| You… | If the visitor was… | Result |
|---|---|---|
| Admit | Honest | a mouth to feed and a friend |
| Admit | Agent | the agent works their flags: power questions, water access, an ear on the radio |
| Turn away | Honest | a name on the *Count*; a story other settlements tell |
| Turn away | Agent | nothing — and the agent's faction learns what your gate does |
| Detain | Honest | a *false accusation*: standing hit with the visitor's true faction; grievance in the corridors (*Shelter Governance*) |
| Detain | Agent | a captive, and a decision about the interrogation |
| Turn | Defector | a source that is worth more than the guard it cost |

## 4. Dossiers: the player's own war

The Informant Network stops being a separate panel and becomes **the source of Claims you can check**. A **dossier** on a faction — built from informants, faction intelligence operations, dead drops, and radio intercepts — tells the player *which profiles that faction fields, what credentials it forges, and when it is likely to send someone.* Dossiers are **derived** from the existing informant and operation records; they are read, not stored twice.

Running your own assets has the mirror risk: an informant can be compromised, doubled, or burned; a *sweep* finds one. The existing tradecraft engine already models this.

## 5. Four movements

**I — The First Lie.** A visitor with one small lie. *Ends on:* the first person the player realises they were wrong about, in either direction.

**II — The Watcher's Week.** Monitoring costs duty hours; the shelter is short. *Ends on:* the first agent that acts.

**III — The Turned.** A captured agent, a defector, an informant of the player's own who has stopped answering. *Ends on:* a decision about whether to trust a source.

**IV — The Bait.** The player plants a lie and finds who repeats it. *Ends on:* the Quiet War's shape — who was talking to whom all along.

## 6. Voice samples

- *Gate log:* "Day 140. Name given: Marek. Faction given: Garrison deserter. Credential: a stamped token. He asked twice about the generator. He did not ask about the food."
- *Watcher:* "He sat where he could see the door. He does that. I do that."
- *Vet report:* "Nothing wrong with his paper. Nothing wrong with anything. That is the thing I do not like."
- *Confession:* "I didn't come to hurt anyone. I came because they told me they'd let my sister out."
- *Accusation:* "You called me a spy in front of forty people. I am a baker."

## 7. Content plan

- **W1 — Claims:** 40 claim templates × 3 registers (honest, desperate, agent) — hooks into the 80 authored door encounters as *variants*, never edits.
- **W2 — Tells:** 10 profiles × 5 behavioural lines (50).
- **W3 — Interview:** 12 questions × 4 answer shades (48).
- **W4 — Dossiers:** 8 factions × 4 (needs, forges, sends, fears) = 32.
- **W5 — Aftermath:** false-accusation and turned-agent lines (30).

## 8. Non-goals (restated)

No replacement of Airlock/Door/Visitor/CI systems. No new save section (nested, DEC-QW-02). No new routed panel (DEC-QW-08). No mandatory interrogation. No mind-reading.

## 9. Risks

- **Deduction becomes a puzzle box.** *Bound:* tells are probabilistic and small; the game never guarantees a right answer.
- **Paranoia becomes the only strategy.** *Bound:* false accusations cost more than false admissions on average; honest visitors are the majority by construction.
- **Three front-door systems collide.** *Bound:* P0 names one decision point and one stay owner.
- **Demo roster** (F2) makes claims about non-real survivors. *Bound:* P0 gate.

---

## The deeper layer — objects, scenes & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — not a claim,
not an authorization. The register below is unchanged; the fragments are content candidates and
deliberate silences, not new recorded questions.)*

**The second layer.** Reading a person is a craft that wears out as it is used — and this
expansion's discipline is that the game never knows more than the evidence supports. Neither does
the desk. Tells accumulate; none is diagnostic; certainty is a habit the reader brings, and the
file stays thin on purpose.

**What the expansion leaves lying around.**

> "Permit file: the name is spelled *right* this time. That is the tell, and it is not in the manual."

> "Interview sheet: the pause before question 3 has been measured. The measurement is not evidence."

> "Turn-away slip, filed. The shelter declining to know is also an act, and it is recorded as one."

**Scenes the player may piece together.**

> "Three days of tells accumulate and none of them is diagnostic. On the fourth day the file is thin and the certainty is not."

> "A defector's record is produced. The record is a state. Nobody in this expansion will call it a conversion."

**Held silences (texture — the register below is unchanged).**

- What the visitor's three days were like before the door. Only the tells arrive; the history stays on the road.
- Whether the reader is ever measured. The player's judgement is wear-prone by design; nobody keeps a file on the file-keeper.

---

## What stays unsaid (tone register — deliberately unanswered)

These are **not gaps and not content backlog.** They are the questions the expansion refuses to
answer, and the refusal is the atmosphere. Any future plan that answers one must say so explicitly
in its own decision register.

**Who trains the infiltrators.** Naming a sponsor turns a pressure system into a plot. The ten
profiles imply an apparatus without describing one.

**Whether the "signal interceptor" is a person or a technique.** Ambiguity is what makes the
*Radio Free Ashfall* bridge interesting. Resolving it would remove the dread from both expansions.

**Whether the three days of tells reflect the truth or rehearse it.** Both readings are supported
by the data. Choosing one would make tells diagnostic instead of cumulative — and the whole design
depends on them being cumulative.

**What happens to a detainee after the camera stops.** The plan routes to existing custody APIs and
deliberately does not narrate past them.

**Whether there was ever a real defector.** `AcceptDefector` produces a record. The game never
asserts sincerity, only state.
