# ASHFALL — THE LONG SIEGE
### A raid is a night. A siege is a season · The wire holds; the people inside it are what gets tested

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/long-siege-2026-09-29.md`
**Family:** "The Shelter Under Pressure" — `docs/expansions/expansion_shelter_under_pressure_index.md`.
**Tone lock (inherited):** cold, exhausted, human, restrained. Besiegers are people with supply problems. No real countries, people or copied text.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.
**Naming note:** this was first proposed as "The Night Watch". That is Expansion 36 (*The Watch*), already built — staffing, perimeter, gate, acoustics. This plan **does not touch it**; it gives the Watch something to watch *for*.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

Today the shelter is attacked in single, self-contained moments. The Iron Raiders' state machine rolls a daily raid chance from aggression and visibility. If a raid opens, `crewDanger` is 3 or 6, the traps and emplacements resolve first, and whatever breaches escalates to a fight. Then the day ends and the wire is exactly as it was. The comment in the source says *"siege weeks bleed into raider weeks"*. Nothing in the code makes a week.

The Long Siege is the expansion where **the attack does not end at dawn.** A besieging party takes the ground outside. They cut the roads, dig in, make noise at night, send a rider under a white cloth, probe the wire, and *wait*. The shelter has the same hands, the same food, the same walls. The question is not who wins a fight. It is who runs out first — of food, of water, of sleep, of nerve, of patience — and what the shelter is willing to become to be the one who doesn't.

The promise: **you will hold, or you will not, and either way you will know exactly how long it took and what it cost.**

### 1.2 Pillars

1. **A siege is a state, not an event.** It has a day count, phases, and a way to end that is not "fight".
2. **Both sides have a clock.** The besiegers have supply and resolve; the defenders have stores, water and nerve. The player reads both.
3. **Existing walls, new time.** Traps, emplacements, watch and gate keep their meaning. The siege calls them again, day after day, and lets attrition happen.
4. **Pressure comes in kinds.** Probe, starve, noise, parley, sap. Each attacks a different resource, and each has a defence that is not a bigger gun.
5. **Endings are five, not one.** Lifted, relieved, negotiated, ground down, fallen.
6. **Ship dark.** No doctrine row, no siege — raids behave exactly as they do today.

### 1.3 Not this

Not a tower-defence layer. Not a new combat engine. Not a replacement for the Watch, the perimeter, the traps or the raid resolver. Not a war simulator (the faction war already exists; a siege *reads* it). Not about outposts under pressure — Year Two owns that.

---

## 2. What the code and data actually say (audit)

| Area | What exists | What is missing |
|---|---|---|
| Raid resolution | `DefenseSystem.ResolvePreCombatRaid(day, raiderStrength, isNight, perimeter, poweredProvider, targetingRng, captureRng, guardFraction)` → `DefenseEngagementResult` (neutralized, captured, remaining, repelled, breached). Raid log capped at 50; state saved as `settlement_defenses`. | One call is one raid. No memory between calls beyond a log. |
| Raid source | `IronRaidersSystem`: aggression, visibility, `EvaluateRaidChance`, `ExecuteRaid`. Host path `Main.Muster.cs` L158–200 is the **only** production caller of the resolver (plus one debug command). Strength is 3 or 6. | Raids are tiny and one-off; there is no besieger with an identity, supply or resolve. |
| Watch | *The Watch* (Expansion 36): `NightWatchHostSession` — duty roster staffing, perimeter, `ShelterSecuritySystem` gate, `SoundRangingThreatEngine`, `NightWatchPatrolReadinessEngine`. | Built to detect *events*; nothing gives it a sustained presence to read. |
| Perimeter | `PerimeterDefenseSystem`, traps with HP and repair, `perimeter_defenses.json`, `defenses.json`. | Damage accumulates in traps only; nothing wears the *people*. |
| Gate | Airlock / door encounters / visitor integration (the three-system gate). | A parley has no place to happen. |
| Water | `DeepWellSystem` (build state, condition, yield ledger); sump pump. | Water is never a siege resource. |
| Rationing | *The Ration Wars* (sibling plan) adds Table Rules. | No "Siege Table". |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

1. **Who starts a siege.** Raids enter through one host path (Iron Raiders). A siege is proposed to *begin* from a repelled raid when doctrine and aggression allow — **so the raid source does not change**, and other factions gain the ability only by authored rows. (DEC-LS-01)
2. **Where siege state lives.** It is defense state: additive nested DTO in `settlement_defenses`, not a new section. (DEC-LS-02)
3. **Roads and expeditions.** There is **no expedition start gate** in `ExpeditionSystem` (VERIFY). A siege "cutting the roads" therefore needs either a host-level dispatch refusal or an additive gate predicate. (DEC-LS-04)
4. **Year Two.** *Year Two* introduces `OutpostPressureModel`. The Siege pressures the **shelter**; Year Two pressures **outposts**. No shared numbers.

---

# PART II — THE SIEGE

## 3. Shape of a siege

A siege has an **identity** (who), a **doctrine** (how), a **camp** (where, by name), and two clocks.

| Besieger clock | Defender clock |
|---|---|
| **Supply** — falls each day; raised by raiding the region | **Stores** — food and water, read from their real owners |
| **Resolve** — falls with supply, casualties and failed pressure; rises when the shelter looks weak | **Nerve** — derived from the shelter's real morale and sleep; never a second meter |
| **Patience** — a doctrine constant: how many days before they change approach | **The wire** — traps and emplacements, read from the defense owner |

Only the besiegers' supply, resolve and patience are new state. Everything on the defender's side is *read*.

## 4. Five pressures

| Pressure | What it is | What it touches | Defence |
|---|---|---|---|
| **Probe** | A small raid on a seeded edge of the wire | The existing raid resolver at low strength; trap HP | Repair, traps, emplacements |
| **Starve** | The roads are cut | Foraging, expeditions, trade arrivals (via the dispatch gate) | The Siege Table, stores, a runner |
| **Noise** | Drums, fires, shouted names at night | Sleep and morale (through the Watch's acoustics reading) | Rotation, earplugs of a kind, a lamp behind the shutters |
| **Parley** | A rider under a cloth with terms | The gate; the Assembly (if present) | Hear it, refuse it, answer with a counter |
| **Sap** | They dig | The *Deep Works* (sibling plan) | Listening posts, a countermine |

A doctrine (authored) is a weighted mix of these, a patience, and a temper. *The Toll* probes and starves. *The Quiet Ring* makes noise and sends riders. *The Diggers* sap and wait.

## 5. What you can do

- **Repair and reset** — the existing trap commands, now a daily cost with a reason.
- **The Siege Table** — one Table Rule preset (with *The Ration Wars* if present) that the shelter adopts for the duration.
- **A runner** — one survivor slips out at night. A seeded chance to get through, to get killed, to be caught and questioned (the last one leaks something; see *The Quiet War*). If through, the runner carries a message to an ally or an outpost.
- **A sally** — a party (with *Crews and Companions* if present; otherwise one volunteer) strikes at the camp to burn stores or spike the drums. It moves the besiegers' supply and resolve — and risks the people you send.
- **A parley** — you may hear terms. Terms are authored (a tithe, a hostage, a road toll); accepting one ends the siege with obligations you will feel later.
- **Hold.** Do nothing but keep the wire. It is an option, and it is often the right one.

## 6. Five ways it ends

1. **Lifted.** Their resolve reaches zero. They leave. The camp remains for a few days, and it can be scavenged.
2. **Relieved.** A relieving party arrives (an outpost, an ally, a faction with a reason). Not automatic; you must have sent a runner or built a bond.
3. **Negotiated.** Terms are accepted. The obligation persists as an ordinary debt or treaty in the owner that already tracks them.
4. **Ground down.** The shelter's stores or nerve fail first; the player must make a terrible choice (surrender, break out, open the door). Consequences run through existing owners.
5. **Fallen.** A breach that the resolver reports as `Breached` and the shelter cannot recover from that night. This is the only ending that is not a choice, and it must be *earned* by a long run of days the player could see.

## 7. What a bad week looks like

Day 9. The Toll's probes have cost you three traps and a child's sleep. The road to the salt depot has been shut since Day 3. Ilya wants to send a runner; Mikhail says the runner will be caught. The Table is at Half. The Watch reports drums from the east ridge, nothing from the south, which is either good or the reason for the drums. A rider under a white cloth is at the airlock with terms you can just about live with. You have a lamp, a well, and nine days.

---

# PART III — HOW IT MEETS THE WORLD

## 8. Four stories

**The drum.** For seven nights, from the east ridge. On the eighth night it stops. The Watch reports nothing, and nobody sleeps.

**The runner.** She goes out at moonset. You hear nothing. Six days later a rider you don't recognise stops at the ridge and holds up something small and red. You will not know for another season if it was hers.

**Terms.** The tithe is reasonable. It is a fifth of the salt. The Assembly, if you have one, is divided; the Keeper is writing it down; the quartermaster says the Book can bear it. It is the sort of number a sensible shelter accepts, and that is what makes you afraid of it.

**The well.** For twelve days the Deep Well has been the only water that is not a promise. On the thirteenth its yield line starts to fall, and nobody in the shelter will say why in the same words.

## 9. Voice samples

> **Watch log.** *Drums, east. 03:10. Stopped 03:40. Nothing on the south wire. Nothing on the west wire. Nobody has slept in the dormitory since Tuesday.*

> **The rider.** *"A fifth of the salt, a road toll on the east, and the boy comes to us for a season. It is not a bad offer. That is the problem with it."*

> **Journal.** *Day 31. The wire held. That is the whole entry.*

## 10. Boundaries with other expansions

| With | Boundary |
|---|---|
| **The Watch (Exp. 36)** | Untouched; the siege reads its readiness and acoustics. |
| **The Ration Wars** | The Siege Table is a preset; the Siege owns when it is adopted. |
| **The Deep Works** | Sap and countermine: the Siege owns the *action*; the Works owns the drift and the collapse. |
| **The Quiet War** | Captured runners and inside informants use the QW gate and informant system; no shared state. |
| **The Plague Year** | A sealed gate during siege is a Gate Protocol; the Siege cannot force it. |
| **The Living Region** | Roads blocked by a siege are read as a region flag; no LR state is written. |
| **Crews and Companions** | Sallies are parties if the coordinator exists. |
| **Year Two** | Outposts under pressure are Year Two's; relieved-by-outpost is an ending that consumes Year Two's supply flag. |
| **Shelter Governance** | Terms are Assembly business if the Assembly exists. |

## 11. Content plan

- W1: doctrines (3), pressure text for each, first two endings (Lifted, Ground down).
- W2: Parley terms (authored, ~8), runner outcomes, Relieved.
- W3: Sally events, Negotiated obligations, Fallen (carefully, and only from a long run of visible days).
- W4: cross-plan hooks (Siege Table, sap, sally) — ship dark until both ends exist.

## 12. Non-goals (restated)

No change to raid strength, the resolver, trap arithmetic or the Watch; no new combat; no new faction system; no world map layer; no new routed panel; no Unity.

## 13. Risks

| Risk | Mitigation |
|---|---|
| Siege feels like waiting | Each day offers a decision; the Hold option is a real one. |
| Unwinnable feeling | Both clocks are visible; the besieger's clock always moves. |
| Spam of daily raids | Probes are seeded and capped by doctrine patience; the resolver is called at most once per day. |
| Overlap with the Watch | Zero writes to Watch state; read-only. |
| Overlap with Year Two pressure | Separate numbers, no shared fields. |
