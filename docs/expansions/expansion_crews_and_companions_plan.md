# ASHFALL — CREWS AND COMPANIONS
### Expeditions become parties with named crew and animals · Nobody goes alone, and nobody comes back the same

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/crews-and-companions-2026-09-29.md`
**Family:** "New ways to play" — `docs/expansions/expansion_new_ways_to_play_index.md`.
**Tone lock (inherited):** cold, exhausted, human, restrained. Animals are companions, not mascots; loss is real and quiet. No real countries, people or copied text.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

An expedition in ASHFALL is one person. The system says so in a comment: *one expedition per survivor*. Mikhail walks to the allotments, loots, pushes his luck or retreats, and comes home with a weight in kilograms. Meanwhile the shelter has, separately, a hound that loves Ilya and a goat that is heavier than it looks, and the two facts touch only where the hound's "on expedition" flag flips because its handler left.

Crews and Companions turns the walk into a **party**. Two, three, four people — and up to two animals — go out **together**, with roles, with a shared pool of nerve and stamina, with a way of arguing at camp and a way of not talking about what happened. The road writes into the people: bonds form and fray, someone sleeps badly, someone stops volunteering. The animals are not gear. They are *members*, with a handler, a temper, and a way of being lost.

The promise: **you will send people out together and learn who they are by who comes back, and in what order.**

### 1.2 Pillars

1. **A party is people.** Named, with roles, with relationships that the road changes.
2. **Roles make choices.** A medic, a pathfinder, a hauler, a watch, a handler: each opens options a solo walker never has.
3. **Cohesion is a resource.** It is built at camp and spent on the road.
4. **Animals are members.** They can save the party, be hurt, be lost — and be grieved.
5. **The road is a shared trauma.** Extreme events write into the existing trauma-bond and relationship systems.
6. **Ship dark.** A solo expedition works exactly as today; a "party" of one *is* today.

### 1.3 Not this

Not a tactical squad game. Not a second expedition system. Not a fleet of parallel sorties. No new inventory or stamina system: a party is a *set of existing expeditions plus a coordinator*, with the existing authorities keeping their meaning.

---

## 2. What the code and data actually say (audit)

| # | Finding | Evidence | Status |
|---|---|---|---|
| F1 | **One expedition per survivor**: `Start(def, survivorId, day, stance, night, bicycle, flashlight, vehicle, stamina, weather, speed)`; `_active` is keyed by `survivorId`; a second start for the same survivor is refused. State carries stamina, loot weight, danger, encounter chance, push-luck flag, vehicle id, weather multiplier. | `Expeditions/ExpeditionSystem.cs` L184–215, L400–454 | LIVE |
| F2 | The expedition system exposes **query hooks**: stamina-drain multiplier, survivor speed multiplier, **pack-capacity bonus**, encounter-chance multiplier — all `Func<string, float>` keyed by survivor. | `ExpeditionSystem.SetStaminaDrainMultiplier`, `SetSurvivorSpeedMultiplierQuery`, `SetPackCapacityBonusQuery`, `SetEncounterChanceMultiplier` (L349–387) | LIVE |
| F3 | Camp exists: `EnterCamp`, `ReserveCampSupplies`, `CampTick`, `ResolveCampEncounter`; panels `ExpeditionCampPanel`, `ExpeditionPanel`. | `ExpeditionSystem.cs` L842–1050; `src/UI/` | LIVE |
| F4 | **Companion animals** (Plan 174): 5 species — Ash Hound (guard/morale), Feral Goat (pack 25 kg/morale), Cotton Hare (morale), Rad Dog (guard/pack 15 kg), Iron Crow (guard/morale); care, feeding, bond 0..max, training levels, sickness treatment, **roles Guard / Pack / Morale**, assignment to **one handler survivor**, guard modifier, pack capacity bonus, morale support (bp), grief morale delta; save section `companion_animals`. | `Ecology/CompanionAnimalSystem.cs`; `companion_animals.json`; `src/Main.Companion.cs`; `Save/SaveSectionRegistry.cs` L289 | LIVE |
| F5 | The host marks a companion **on expedition** when its handler has an active expedition; the animal has no separate risk, action, or position. | `src/Main.Companion.cs` L214–225 | LIVE / GAP |
| F6 | Naval vessel defs carry `crew_min`/`crew_max` (raft 1/1 … barge crew of up to six). **Nothing reads them.** | `naval_vessels.json`; grep | GAP |
| F7 | Social substrate exists: relationship decay, **trauma bonds**, survivor roles, skill progression with atrophy, social coordinator. | `Survivors/RelationshipDecaySystem.cs`, `TraumaBondSystem.cs`, `SurvivorRoleSystem.cs`, `SkillProgressionSystem.cs`, `SurvivorSocialCoordinator.cs` | LIVE (VERIFY write APIs) |
| F8 | Vehicles/garage/armour grades exist for expeditions; `ExpeditionVehicleProfile` is a per-expedition input. | `ExpeditionVehicleSystem.cs`, `Expeditions/VehicleGarageSystem.cs`; `vehicles.json` | LIVE |
| F9 | 75 authored expedition locations (distance ticks, danger, encounter chance, stamina drain, scavenging table). | `expeditions.json` | LIVE |
| F10 | Encounter choices are resolved per survivor (`survivorId`) via the encounter bridge. | `Expeditions/ExpeditionEncounterBridge.cs`; `src/Host/ExpeditionHostSession.cs` L1155 | LIVE (VERIFY choice API) |
| F11 | Duty roster governs who is free to go. | `DutyRoster/DutyRosterSystem.cs` | LIVE |
| F12 | Selftests: expedition, companion animals. | `src/Host/HostCli*.cs` | LIVE (VERIFY args) |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

- **One-per-survivor is a deliberate rule (F1).** Systems win: it stays. A party is **N survivors each on an ordinary expedition**, bound by a small **party coordinator** that shares the destination, pace, and outcome. No rewrite of `ExpeditionSystem`; no second start path.
- **Companions have one handler (F4).** Systems win: the handler-assignment stays. A companion *travels with its handler's expedition* and gains a party-level presence (role, risk, loss) *through* the coordinator, not by becoming a survivor.
- **Crew fields unused (F6).** *The Drowned Coast* and *The Long Line: Freight* both want crews. This plan provides the one **crew contract** they read (§6); it does not implement boats or wagons.

---

# PART II — THE STORY

## 3. The Party

### 3.1 Making a party

The dispatch screen (the existing expedition panel) asks for a **lead**. The player then adds up to **three more survivors** and up to **two companions** (each companion's handler must be in the party). Everyone must be free on the duty roster. The party has a **name** the player may give; it is written into the ledger.

### 3.2 Roles

Roles are **derived from what people can do**, then *chosen* for the trip:

| Role | Drawn from | Opens |
|---|---|---|
| **Lead** | anyone | decides push-luck / retreat |
| **Pathfinder** | navigation/scavenging skill | faster travel; fewer wrong turns |
| **Hauler** | strength/pack | carry weight; the goat's kilos count |
| **Medic** | field medicine | treat an injury on the road; unlocks medical options in encounters |
| **Watch** | alertness | raises night safety; reduces ambush |
| **Handler** | animal training | a companion's actions and calm |

A party of one is the current game. A party of four has options no solo walker could take, and *disagreements* no solo walker could have.

### 3.3 Cohesion

**Cohesion** (0–100) is derived from relationships among members, shared history, and camp. It is *built* by camp time, shared meals, and success; *spent* by long marches, losses, and hard choices. High cohesion lowers the chance of a bad choice in the group's favour; low cohesion produces **quarrels** (a choice the party takes against the lead's recommendation).

### 3.4 Camp

Camp is where a party becomes a crew. The existing camp actions gain **rituals**: a shared meal, a watch order, a story, a silence. Each is a small authored choice with a cohesion effect and a line in the ledger. Nothing here invents a new camp authority; it adds *content* on the camp choice machinery.

### 3.5 Injury, loss, and who it happens to

An encounter can hurt **any member**. Who is chosen is decided by a **seeded draw** weighted by role and position (the hauler is slower; the watch is nearer the danger). A medic on the road changes the outcome of an injury; a companion may take the hit meant for its handler. Loss is a real event: death routes through the existing survivor legacy; a lost companion routes through the existing grief system.

### 3.6 Companions as members

| Companion | Party presence |
|---|---|
| **Ash Hound** | guard: raises watch quality; may alert; may be hurt guarding |
| **Feral Goat** | pack: carries; slow; sturdy; will not enter certain terrain |
| **Cotton Hare** | morale: small comfort; easy to lose; deeply mourned |
| **Rad Dog** | guard/pack: dangerous, useful; disease risk |
| **Iron Crow** | scout/guard: sees ahead; will not stay |

Each has a **temper** and a **terrain** fit (from existing `terrain_tags`). Bonds with handlers matter: a high-bond animal will not leave a wounded handler.

### 3.7 The road writes into people

Extreme events (a loss, a near-death, a shared refusal) write to the **existing** trauma-bond and relationship systems. A survivor who came back from a party where someone died is not the same; a pair that survived together may not talk for a week, or may move bunks. The ledger records it.

## 4. Four stories

**The Long Walk East.** Three people, a goat, a hound, a supply run. The goat will not cross the culvert. The pathfinder wants to go on without it. The handler will not.

**The Medic's Choice.** A party of four; one member is hurt at the halfway mark. The medic can treat and lose the day, or push on and lose the hurt.

**The Hare.** It is small and it is the only thing Thea has spoken to in three weeks. The party has to decide whether to wait for it.

**Quarrel.** Low cohesion. The lead recommends retreat. Two members vote to go on. The player learns something about the lead's authority that no menu told them.

## 5. Voice samples

- *Ledger:* "Party *Third Lantern*: Mikhail (lead), Ilya (hauler), Thea (medic), hound *Ash*. Destination: the allotments. Back on day 6, four of four."
- *Camp:* "Nobody said anything about the culvert. Ilya shared his last pear."
- *Injury:* "Ilya went down at the second gate. Thea had him on his feet in two minutes and back on the road in twenty. They didn't speak the rest of the day."
- *Loss:* "The hound did not come back from the fence. Mikhail sat by the door for an hour. Then he went and fed the goat."

## 6. The crew contract (for other plans)

Other expansions need "a crew": a boat (*The Drowned Coast*), a wagon (*The Long Line: Freight*). This plan provides the **one shape**:

> **Crew contract** — `{ roleNeeded[], crewMin, crewMax, memberIds[], cohesion }`, produced by the party coordinator, consumed read-only. A boat's `crew_min/crew_max` and a wagon's driver/escort are *instances* of it.

No plan builds a second crew concept.

## 7. Content plan

- **W1 — Camp rituals:** 12 rituals × 3 outcomes (36).
- **W2 — Role options in encounters:** 20 authored role-gated options added to existing encounters (as variants; no edits).
- **W3 — Quarrels:** 8 quarrel scenes × 3 resolutions (24).
- **W4 — Companion moments:** 5 species × 6 (30).
- **W5 — Road-bond lines:** 20 (after-event writes).

## 8. Non-goals (restated)

No rewrite of the expedition system. No new stamina/inventory/health authority. No new save section (nested in `expeditions`, DEC-CC-02). No new routed panel (DEC-CC-06). No squad tactics.

## 9. Risks

- **Bookkeeping explosion.** *Bound:* party size cap 4 + 2; cohesion is one number; roles are chosen once.
- **One-per-survivor is broken by accident.** *Bound:* the coordinator only *groups*; each survivor still has exactly one `ExpeditionState`.
- **Animal death as cheap drama.** *Bound:* authored, rare, weighted by bond; grief through the existing system.
- **Save size / compatibility.** *Bound:* additive nested DTO; a party of one writes nothing extra.
