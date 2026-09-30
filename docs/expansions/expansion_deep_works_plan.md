# ASHFALL — THE DEEP WORKS
### The shelter goes down · A drift is a promise to keep digging, propping and pumping

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/deep-works-2026-09-29.md`
**Family:** "The Shelter Under Pressure" — `docs/expansions/expansion_shelter_under_pressure_index.md`.
**Tone lock (inherited):** cold, exhausted, human, restrained. The dark is not evil; it is just heavy. No real countries, people or copied text.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.

---

> *"The shaft went down. This is about going sideways — into rock nobody surveyed, under a sky
> nobody remembers."*
>
> There is a sound a shelter makes when it stops being a building and becomes a settlement: the
> first time somebody says *the works* and means somewhere other than the room they are standing
> in. A drift is not a room. A drift is a decision to **keep a place**.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

ASHFALL has four undergrounds and almost none of them meet.

There is an **authored tunnel network** on the world map: three junctions, three segments, hazards named Darkness, ToxicGas and Flooding, a day tick that wears the integrity down. There is a **generated subterranean network** of ten nodes across three depth tiers — metro annexes, cellars, a mine slope, a storm cistern, a bunker vault, a deep sump gallery — with oxygen, flood pressure, shoring levels and ventilation; the player finds them from *surface anchors*, walks into them as expeditions, and walks out. There are **excavation sites** and **hazard sectors** with methane, spores, bulkheads and a cave-in rescue clock. And there is the **shelter's own shaft** — a blueprint that digs one level deeper, costs stability, and adds nothing but a level number.

Grep for any of them in the others and you find one real seam and a lot of registries. The one seam: a rising subterranean node feeds flood water, one way, into the hazard sector of the same id. The authored tunnels are referenced by *no* file that also mentions the shelter's shaft, deep well or hazard sectors. The shaft the shelter digs never breaks into a gallery. A gallery the player finds can never become *theirs*. The dark has no owner; nobody has to look after it.

The Deep Works is the expansion where **the shelter goes down and stays down.** You dig until the shaft breaks into a gallery; you prop it, air it, drain it, staff it and decide what it is for — a second way out, a cistern, a cold store, a place to dig. Every drift is a commitment: it wears, it floods, it lets things in, and a crew has to be paid for in labour and timber every week for as long as it is open. And it has a door, a bulkhead, that you will be tempted to close.

The promise: **you will own a piece of the dark, and it will cost you a little every day to keep it.**

Breakthrough is cheap and celebrated. **Holding** is weekly and dull and fatal. The underground
does not resist you; it simply keeps its own books, and the books are written in a hand that is not
yours.

The vocabulary is the pit and the shift gang — *bulkhead, schedule, gang, shoring, spoil, pump
time*. Dread arrives as arithmetic: a roof one decision weaker than it was last week. And the
skipped week is the sharpest mechanic in the expansion — it changes nothing *directly*, so decay is
discovered rather than broadcast. By the time the Works Book says something is wrong, it has been
wrong for a fortnight.

### 1.2 Pillars

1. **A drift is a commitment.** Dig it and it wants keeping: props, air, water, hands.
2. **A hole lets things in.** Breaking through can bring gas, water, and problems. The bulkhead is your answer.
3. **Every drift has a job.** Exit, Cistern, Store, or Diggings — one at a time, by choice.
4. **Existing dark, existing rules.** Nodes, shoring, oxygen, methane, bulkheads, rescue: all already exist. The Works connects them and asks the player to care.
5. **The last verb is collapse.** You can bring a drift down on purpose. It is a real answer, and a real loss.
6. **Ship dark.** No held drift, no Works — the underground behaves exactly as it does today.

### 1.3 Not this

Not a second subterranean, excavation or tunnel system. Not a new hazard model. Not a mining tycoon. Not a change to the expedition path into the dark — you may still walk in. The Works is what you do with a piece of the dark you decide to keep.

---

## 2. What the code and data actually say (audit)

| Area | What exists | What is missing |
|---|---|---|
| Generated network | `SubterraneanSystem`: 10 nodes, depth tiers 1–3; per node: integrity, shoring 0–3, oxygen, ventilation, water, blocked; daily tick (oxygen drains only for occupants; flood and decay run for every node); public ops `TryShoreNode`, `TryInstallVentilation`, `TryClearBlockage` with atomic billing; expeditions register a destination per zone. | Nodes are discovered from surface anchors only; the shelter is never an anchor; nothing marks a node as *held*. |
| Authored network | `TunnelNetworkSystem` (on the world map): junctions, segments with hazards, `RepairSegment`, `ReinforceSegment`, `ClearHazard`, `CanTraverse`, `EvaluateSurfaceBypass`. | Zero references to the other three. |
| Hazard sectors | `ExcavationHazardSystem`: methane, flood permille, spores, shoring health per **sector id** (created lazily, any string); `TryToggleBulkhead`, `TryApplyMitigation` (8 mitigations), `TriggerCaveInRescue`. A one-way flood projection from each subterranean node into the sector of the same id already runs. | Methane and spores have no source from a node; no sector is tied to any place a player chose to keep. |
| Shelter shaft | `ShelterExpansionSystem`: depth levels, stability rating, `TryStartDepthExcavation` (blueprint-driven: 14 days, scrap metal 25, rubble 15, planks 6, stability cost 12, up to level 5 in data), project crews via `TryAssignCrew`. | A level is a number; it opens onto nothing. |
| Water | `DeepWellSystem` (built pump, condition, yield ledger); sump pump; geothermal strata catalog. | Not connected to any gallery. |
| UI | `SubterraneanOperationsPanel`, `SubterraneanCartographyPanel`, `ExcavationPanel`, `MineFlailPanel`. | No panel shows a *kept* drift. |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

1. **Two depth semantics.** Blueprint data allows depth levels up to 5; the class default is 3. `TryStartDepthExcavation` is blueprint-driven; `StartDepthExcavation` hard-codes 12 days and 20 scrap / 10 rubble. → Which path production uses is a P0 question. (DEC-DW-02)
2. **Two hazard models for the same dark — already bridged, one way.** Nodes carry their own flood and oxygen; sectors carry methane, flood and spores. `src/Main.Subterranean.cs` L85–103 already projects node water into the sector **with the same id as the node**, increases only, so a drainage mitigation is never overwritten. → The Works adopts that convention (sector id = node id), keeps both models, adds no second bridge, and writes a breach's methane only to the sector; it never writes the node's water. (DEC-DW-05)
3. **No "collapse on purpose".** Cave-ins are seeded and passive. Countermining needs one additive public method on the node owner. (DEC-DW-07)
4. **Authored tunnels are surface-to-surface.** They connect named locations; a held drift may register an extra segment through the tunnel owner's public `RegisterSegment` so travel can bypass the surface — VERIFY it accepts a node id. (DEC-DW-08)

---

# PART II — THE WORKS

## 3. Breakthrough

When the shelter's deepest unlocked level is at least a generated node's depth tier, the player may **break through**: a construction project (crew, timber, days, stability cost) that ends with the node becoming **Held** — linked to the shaft, tied to a hazard sector, added to the Works Book.

On completion the drift's bulkhead is **sealed**. Opening it is a decision. When it is opened for the first time the shelter *learns what the dark was carrying*: the node's standing water already reaches the sector through the existing flood bridge, and its authored air class (**stale**, **thin** or **foul**) sets a one-time methane reading on the sector through the existing hazard call (VERIFY the class-to-ppm table in P0). A dry, stale drift is a small mercy. A flooded gallery is a Thursday you will remember.

## 4. What a drift is for

| Job | What it gives | What it costs |
|---|---|---|
| **Exit** | A second way out and back: sally, runner, relief for *The Long Siege*; optional under-ash travel | A garrison, or a bulkhead you are willing to lose |
| **Cistern** | Bounded daily water from a cistern-type node | Drainage, props, and a filter that clogs |
| **Store** | A cool, stable place — a **Place** for records *(Record Keepers)* and, if a storage modifier exists, slower spoilage | Damp risk; a lock |
| **Diggings** | A steady small haul drawn from the node's *own* authored scavenging table | A gang, timber, and a schedule of wear |

A drift has one job at a time. Changing it costs a day and, if the old job used a special fitting, a part.

## 5. The Works Book and the Schedule

Each held drift has a line in the Works Book:

- **Props** — shoring level (0–3), already on the node.
- **Air** — ventilation and oxygen, already on the node.
- **Water** — sump and, when needed, pumps; the node's flood pressure.
- **Gang** — the survivors assigned (existing duty roster / project crew).
- **Wear** — the node's integrity, falling daily and by weather.

The **Schedule** is one weekly decision: what to spend on this drift — timber for props, a battery for a blower, a shift for the pump — or to let it go a week. Deferred upkeep is not a penalty in itself; it simply lets the existing seeded collapse roll run against a weaker roof. And when it goes, the existing rescue clock starts on the gang.

## 6. The bulkhead

Every drift's link to the shelter is a bulkhead already in the game. **Seal** it and the shelter is safe from what is inside — and cut off from what is in it. **Open** it and the drift breathes into the shelter: air, damp, cold, gas. It cannot be sealed on trapped miners, and that is a rule the player will hit at the worst moment.

## 7. Collapse on purpose

When the enemy digs (*The Long Siege*, *Sap*) or the drift is a threat, the player may **bring it down**: a permanent seal, no reversal, no salvage. It ends the drift's job, kills the sap, and takes anyone inside. It requires a crew to set the charges and a decision that costs something — the Exit you were counting on, the Store where the records lived, the last cistern.

## 8. Under the ash

An Exit drift can be registered as a **tunnel segment**: a route from the shelter under the surface to a named location, travelling at the tunnel's pace and, when fallout or weather closes the surface, bypassing it. Travel time, integrity and hazards come from the existing tunnel owner. It is the only place the four undergrounds visibly touch.

---

# PART III — HOW IT MEETS THE WORLD

## 9. Four stories

**The first opening.** The bulkhead has been sealed for two days. The player opens it. The gallery has been dry for thirty years; it smells of iron and old paper. Ilya says nothing for a long time, then asks whether the cistern's on the other side of that wall.

**Timber week.** Three drifts, one supply of props. The Schedule says two can be shored this week. The third will hold, probably. Mikhail leads the gang in the third.

**The pump.** The cistern is the only water that isn't a rumour, and the filter is clogging. It costs a battery a week and a man on a stool. In week nine the man on the stool has stopped talking.

**Down.** The Diggers' sap breaks the east wall on the eleventh night. The gang is in the gallery, and the charges are set. The crew chief looks at the player for a long time.

## 10. Voice samples

> **Works Book.** *East drift — Store. Props 2/3. Air: blower on. Water: 14%, rising. Gang: 2. Integrity 71. Next: props, one timber short.*

> **Overheard.** *"It breathes." — "It's a hole." — "It breathes."*

> **Journal.** *Day 88. We opened the bulkhead. The air was cold and it smelled of stone. We closed it again in an hour. We will open it again tomorrow.*

## 11. Boundaries with other expansions

| With | Boundary |
|---|---|
| **The Long Siege** | The Siege owns the *sap* action; the Works owns the drift and the collapse. An Exit drift is a runner/sally/relief route. |
| **The Ration Wars** | A Store drift may slow spoilage only via an existing storage hook (P0). |
| **The Record Keepers** | A dry held drift is a Place; custody and damp are theirs. |
| **The Drowned Coast** | Flood pressure reads weather/tide signals through existing weather owners; no shared numbers. |
| **The Plague Year** | Air and ventilation may help a ward; no new disease state. |
| **Year Two** | A second shelter's own digging is Year Two's; this plan covers the first shelter's shaft and its drifts. |
| **The Reconstruction Tree** | A dive or dig may surface a fragment through the shared fragment seam. |
| **Radio Free Ashfall** | A drift may be a relay site via an existing radio placement (VERIFY); no new station. |

## 12. Content plan

- W1: drift jobs (4), Works Book lines, first-opening and Schedule text.
- W2: Cistern, Store and Diggings loops; breach events (gas, water).
- W3: Exit route, under-ash travel, collapse-on-purpose text.
- W4: cross-plan hooks (siege sap, records place, storage modifier) — ship dark until both ends exist.

## 13. Non-goals (restated)

No new hazard, oxygen, flood or excavation model; no change to expedition entry into the dark; no new resource; no new save section; no new routed panel; no Unity.

## 14. Risks

| Risk | Mitigation |
|---|---|
| Two hazard models double-count | The existing flood bridge is kept as is; a breach writes only methane to the sector and never touches the node's water. |
| Upkeep becomes chores | One weekly Schedule decision per drift; costs are data. |
| Free resource loop | Diggings use the node's own scavenging table at a capped rate; no new resource. |
| Collapse feels cheap | It costs the drift and its job; no salvage. |
| Blueprint path confusion | P0 fixes which shaft path production calls before any change. |

---

## The deeper layer — objects, scenes & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — not a claim,
not an authorization. The register below is unchanged; the fragments are content candidates and
deliberate silences, not new recorded questions.)*

**The second layer.** A drift is a promise made to a hole in the ground, and the Works Book is the
shelter's confession that maintenance is a form of love — weekly, dull, fatal, and never finished.
The generated network has no author; the gang's schedule does. Two books under each other, and
only the top one is in the shelter's handwriting.

**What the expansion leaves lying around.**

> "Shoring slip: timber 4, sound. Initialled by someone who initials nothing else this week."

> "Pump-time column, week 6: 2. The pump does not know it is being rationed; the column does."

> "Bulkhead plate stamped with the node id and the sector id, which are the same number. The requisition does not say why."

**Scenes the player may piece together.**

> "Breakthrough is celebrated. Holding is weekly. The celebration has a hand; the holding has a column."

> "The roof takes its first load and `OnCaveIn` emits a fact — not an account. Accounts belong to another expansion and will not be written here."

**Held silences (texture — the register below is unchanged).**

- Where the spoil goes is not merely unexplained — it is unmodelled. The missing column is the point: this is a shelter, not a quarry.
- Whether anyone walked the network before the anchors found it. Discovery is from surface anchors only; earlier footsteps are not modelled and must never be added.

---

## What stays unsaid (tone register — deliberately unanswered)

These are **not gaps and not content backlog.** They are the questions the expansion refuses to
answer, and the refusal is the atmosphere. Any future plan that answers one must say so explicitly
in its own decision register.

**Why every hazard sector lazily accepts *any* string id.** Sector id equals node id, and nothing
asserts that this was designed. Explaining it would authorise a second map.

**Who cut the first tunnel into the generated network.** Discovery is from surface anchors only.
The network pre-existed every anchor the player has found.

**Where the spoil goes.** The expansion models bulkheads, shoring and pump time. It does not model
spoil. Its absence is the point — this is a shelter, not a quarry.

**Why `ExpansionTunnel = 2` is unused and already named exactly right.** The enum anticipated this
expansion. That will not be explained. The artefact reads as foreshadowing.

**What a gang hears when the roof takes its first load.** `OnCaveIn` emits a fact. It does not emit
an account. Accounts belong to *The Record Keepers*.
