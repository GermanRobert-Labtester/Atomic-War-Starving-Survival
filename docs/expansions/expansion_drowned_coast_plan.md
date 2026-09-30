# ASHFALL — THE DROWNED COAST
### A maritime campaign · Boats, harbours and dives · Read the water, or be read by it

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/drowned-coast-2026-09-29.md`
**Family:** "The world moves without you" — `docs/expansions/expansion_world_moves_without_you_index.md`.
**Builds on, does not replace:** `docs/expansions/expansion_09_the_black_flotilla_plan.md` (dives, contamination, flotilla standing) and `docs/plans/EXPANSION_PROGRAM_WAVE3_2026-09-21/PLAN-MARITIME-DEEPWATER-27.md` (PROPOSED tide/dive/ice-road tranche). This is the **campaign layer** over that machinery; §2.1 lists the stale premises the audit found in Plan 27.
**Tone lock (inherited):** cold, exhausted, human, restrained. No magic; the water is dangerous, not haunted. No real countries, wars, people, or copied text.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.

---

> *"The sea did not rise. The land remembered what it used to be, and went back to it, one winter at
> a time."*
>
> The coast is not a destination. It is a **schedule**: four days of tide, six windows, a berth that
> is dry on Tuesday and a memory by the following thaw. Everything maritime here is a matter of
> timing, and timing is the one thing a shelter with a day-advance cannot negotiate.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

The corpus already has a sea: fourteen dive sites, five hulls from a drum-raft to an armoured river barge, a four-day tide, a fleet that trades access for silence, a District 8 dock that can be reopened four ways. What it does not have is a **coast** — a place you *live on* the edge of, learn slowly, and come to feel obliged to. Today you can dive; you cannot *be a person who goes to sea*.

The Drowned Coast is the campaign that makes water a home ground. You begin with a raft you should not trust. You end holding — or losing — the last working harbour on a coast that is going under a little every winter. Between those two, the loop is the same one the water forces on everyone:

**read the tide → choose a boat → decide what the harbour will let you do → go → come back changed (or don't).**

A harbour here is six small polities with dues. A boat is a hull number and a fuel line and a
favourite. A dive is a window that opens and does not wait. And the **waterline** is a slow,
published, unreadable-in-advance arithmetic that quietly converts *berth* into *shoreline* and
never converts it back.

This is the one antagonist in the game that never attacks and never relents. The two-day
announcement before any waterline step is therefore the expansion's moral centre: the coast may be
implacable, but it is never *surprising*. A player who loses a berth they were warned about loses it
to the world. A player who loses one without warning loses it to the design.

### 1.2 Pillars

1. **Water is a schedule.** The tide is deterministic and knowable. Skill is planning around it, not reflexes.
2. **A boat is a person's whole month.** Hull, fuel, crew and berth are stocks that can run out. A boat lost is a story, not a number.
3. **Harbours have politics.** Every berth costs something — dues, silence, favours — and every harbour is someone's.
4. **The coast is drowning.** The waterline creeps. What you can reach this winter you may not be able to reach next.
5. **The dive is a coda, not the game.** Dives are the deepest room of a coast campaign that begins on the surface.
6. **Ship dark.** With no coast data present the game is exactly today's.

### 1.3 Not this

No new map or expedition system. No real nautical data. No monsters; the horror is pressure, cold, bad air and other people. No resurrection of the retired `MaritimeExplorationSystem` (see §2.1).

---

## 2. What the code and data actually say (audit)

| # | Finding | Evidence | Status |
|---|---|---|---|
| F1 | Five vessels: improvised raft (cap 1, oar, hull 80), rowboat (2, oar, 120), fishing skiff (3, motor, 150), motorboat (4, motor, 200), armoured river barge (6, motor, 500, cargo 1200). Fields: fuel item/rate, oar stamina, corrosion profile, draft, combat rating, repair materials, tags. | `naval_vessels.json`; `Assets/Ashfall.Core/Expeditions/ExpeditionNavalSystem.cs` (`NavalVesselDef`) | LIVE |
| F2 | Vessel *instances* carry hull, fuel, `isDocked`, `currentPortId` (default `loc_holdfast`). **No `CaptureState`/`RestoreState` on `ExpeditionNavalSystem`.** | `ExpeditionNavalSystem.cs` L38–47, grep | **GAP / VERIFY** — vessel ownership is not visibly persisted |
| F3 | Route estimate consumes route, freeze state, weather factor; piracy risk from route hazard × combat rating; corrosion from toxic contamination; projection to vehicle profile. | `ExpeditionNavalSystem.cs` `EstimateRoute`, `PiracyRisk`, `ApplyWaterCorrosion`, `ProjectToVehicleProfile`, `RollPiracyEncounter` | LIVE |
| F4 | The host owns the naval system in two places: `Main.EnsureNavalSystem()` (loads `naval_vessels.json`) and a private `ExpeditionHostSession._naval` (piracy/projection). | `src/Main.NavalExpeditions.Integration.cs`; `src/Host/ExpeditionHostSession.cs` L360 | LIVE / **VERIFY** whether the two agree (two instances = a possible second authority) |
| F5 | 14 dive sites (12 `exp09_*`, 2 `exp23_*`) with oxygen budget, noise floor, rooms, safes, loot, `tide_window`, `keeper_thread_id`, `location_id`, `discovery` text. | `dive_sites.json`; `Maritime/DiveSiteCatalog.cs` | LIVE |
| F6 | Tide is a **4-day cycle**; windows `Any / Slack / LowOnly / HighOnly / FallingOnly / UnsafeAtPeak`; `IsWindowOpen`, `DaysUntilOpen`. | `Maritime/TideCalendar.cs` | LIVE |
| F7 | The maritime host loads the dive catalog and has its own save (`maritime`, `maritime_save.json`); the atlas panel reads tide windows. | `src/Host/MaritimeHostSession.cs`; `Save/SaveSectionRegistry.cs` L74, L453; `src/UI/MaritimeAtlasPanel.cs` | LIVE |
| F8 | `MaritimeDiveSystem` (Core) has tests but **no `src/` reference**; `DiveInstanceRunner` is constructed by `ExpeditionHostSession`. | grep `src/`; `ExpeditionHostSession.cs` L40, L1641 | **VERIFY** which is the live dive authority |
| F9 | Black Flotilla standing is a separate static authority with host session and selftests. | `Maritime/BlackFlotillaStanding.cs`; `src/Host/BlackFlotillaStandingHostSession.cs`; `--black-flotilla-standing-selftest` | LIVE |
| F10 | District 8 Deep Coast: 5 stages (Sealed → Surveyed → PerimeterOpen → DockAccessible → DeepBerthOperational), 4 access decisions (Stabilize/Salvage/Fleet-controlled/Municipal-controlled), structural integrity, contamination, fleet levy. Owned by the Holdfast save envelope. | `Assets/Ashfall.Core/District8DeepCoastSystem.cs`; `src/Host/DeepCoastHostSession.cs` | LIVE |
| F11 | Coastal sectors exist in the evolving world: `sector_8_docklands`, `sector_8_reed_flats`, `sector_8_deep_shelf`, `sector_8_estuary`, `sector_8_lowlands`. | `world_evolution_seeds.json` | LIVE |
| F12 | Freeze state and an ice road exist (`YearOfAshIceRoadSystem`, `weather_seasons.json`); `EstimateRoute` accepts a freeze state. | `YearOfAsh/YearOfAshIceRoadSystem.cs`; `ExpeditionNavalSystem.EstimateRoute` | LIVE |
| F13 | **Retired:** `MaritimeExplorationSystem` (Plan 207, 750 lines) and its data `maritime_zones.json` were removed as a duplicate of the live dive authority; the data was **moved, not deleted**, to `docs/archive/retired-data/`. | `WORKTREE_OWNERSHIP.md` L11866–11874 (`claim-retire-maritime-exploration-duplicate-2026-09-29`) | LIVE (worktree deletions uncommitted — they show as ` D` in `git status`) |
| F14 | Naval selftests: `--maritime-selftest`, `--black-flotilla-selftest`, `--deep-coast-selftest`, `--deep-coast-host-selftest`, `--deep-coast-route-selftest`, `--deep-coast-playthrough`. | `src/Host/HostCli*.cs` | LIVE (VERIFY args) |
| F15 | Map has no waterline concept: sectors and locations are static, and no system drowns a berth. | `world_evolution_seeds.json`; grep | GAP |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

- **Plan 27 is stale.** Its "Outcome" section lists `MaritimeExplorationSystem` and `maritime_zones.json` as machinery that exists. F13 says they were retired and archived. **Systems win**: this plan does not use them; DC-P0 asks the foreman to correct Plan 27's rows (the correction is a docs edit for the integrator, not for this plan).
- **Two naval owners (F4).** The host has `Main._navalSystem` and `ExpeditionHostSession._naval`. Until DC-P0 proves they share state, "vessel ownership" is unresolved and this plan does not add a third.
- **Live dive authority (F8).** `MaritimeDiveSystem` has no host reference. If the live path is `DiveInstanceRunner`, `MaritimeDiveSystem` is the next duplicate candidate — *the foreman's decision, not this plan's*.

---

# PART II — THE STORY

## 3. The coast

Six harbours. Each is a place that already exists in data (locations named from `dive_sites.json`), given a temperament.

| Harbour | Who holds it | Its temper | What it wants |
|---|---|---|---|
| **Cape Beacon** (lighthouse commune) | itself | careful; lamp-keepers; keeps the only reliable sea-mark | logbooks, lamp oil, no noise |
| **Black Flotilla Outpost** (tethered hulks) | the Fleet | transactional; the quartermaster trades approach coordinates for untainted cargo | clean cargo, silence, a berth fee |
| **Maritime Icebreaker Dock** | disputed | half-derelict; a payroll strongroom below | claim tags, a promise |
| **Shelf Roadstead Crane** | Fleet scavengers | refuses to open the lower decks | that you not ask |
| **Ferry Crossing** (the Drown) | traders | the river's clock; a quarantine is first felt here | tolls |
| **The Docklands (District 8)** | four possible holders | the reopening; the campaign's hinge | a decision |

### 3.1 The boats

The five hulls are the campaign's **ladder**: raft → rowboat → skiff → motorboat → barge. Each hull is a decision about fuel and crew before it is a decision about speed.

| Hull | Feel | What it costs you |
|---|---|---|
| **Scrap-drum raft** | groans on every wave; no shelter from spray | a survivor's nerve; oar stamina |
| **Rowboat** | quiet, honest, cold | two pairs of arms, two days |
| **Fishing skiff** | the first real boat | fuel — a standing draw on stores |
| **Motorboat** | fast and loud | noise; the wrong people hear |
| **Armoured river barge** | a floor you can stand on | crew of six; the Fleet notices |

A hull is **bought from the harbour, built from wreck, or inherited** (one authored quest); it is *named by the player* and the name is carried into the ledger and, later, the epitaph.

### 3.2 The tide, played

The tide's four-day cycle is the calendar of the whole campaign. Every dive site has a window, and windows disagree: Cape Beacon's steamer at slack water, the flooded metro only at low, the submerged siphon only at high. The player plans a week around it. The Board shows the next open day for each known site — **only for the sites the player has charted**.

### 3.3 Charts (what the player knows)

A **Chart** is knowledge, not an item: which sites the player has learned (from `discovery` text: a logbook margin, a trade of coordinates, a quartermaster's price), which berths are safe, which windows are real. Charts are how the coast unlocks. A chart can be *stolen, traded, or wrong* (a rival's chart is a day off).

### 3.4 Dues, berths, standing

Every harbour asks something. Dues are paid in what that harbour values (see table). A berth can be **held** (standing), **rented** (per-tide) or **refused**. Standing is the existing record (Black Flotilla standing plus the general standing record) — this plan reads it.

### 3.5 The waterline

The coast drowns slowly and predictably. A **waterline** — a per-harbour value that rises in storm season and recedes in the dry — decides whether a berth is *usable*, *awash* or *lost*. A lost berth is gone for the rest of the campaign. This gives the player a reason to *choose*: two harbours can be held; the third will go under.

### 3.6 Dives (the coda)

Dives keep the fourteen authored sites and existing systems (oxygen budget, noise, rooms, safes, contamination aftercare). What the campaign adds is **context**: you need the boat, the tide, the berth, the chart, and a crew who will still be at the surface when you come up.

### 3.7 Ice

In deep winter the sea becomes a road. The freeze state already changes route estimates; the coast campaign treats winter as a **second modality of the same water** — a raft is useless and a sledge is not. A harbour that was awash in autumn may be *walkable* in January.

## 4. The five movements

**I — The Slack.** A raft, a rowboat, a lamp. Cape Beacon takes the shelter's logbooks and gives a tide table. The player learns that the sea will not be hurried. *Ends on:* the first successful dive at slack water.

**II — The Flotilla's Season.** The Fleet's hulks, a berth fee, a quartermaster who counts. The player learns what "clean cargo" means. *Ends on:* a standing with the Fleet the player can no longer call neutral.

**III — The Shelf.** A skiff and then a motorboat. The icebreaker dock, the sunken patrol craft, the crane that will not open. *Ends on:* the first time the player brings back something they wish they had left.

**IV — The Waterline.** Storm season. Three harbours; two will hold. The player *chooses*, and tells no one. *Ends on:* a harbour going under while its people watch.

**V — The Convoy That Never Crossed.** Three transports at the edge of a trench. The Docklands decision. *Ends on:* the coast the player leaves behind.

Coda by Chapter Profile (Year Two): in Year Two the coast is either **a second shelter's door** (the harbour the player kept becomes a candidate outpost/waystation site — Year Two owns that custody), or a **ledger** (what the player took from the sea, listed).

## 5. Voice samples

- *Tide table, Cape Beacon:* "Slack at dawn. Low by night. If you are on the water after that, you have made a plan that includes swimming."
- *Quartermaster:* "I sell coordinates. I do not sell what is at them."
- *Log line:* "Day 221. Skiff *Ferrel*. Two down, one on the line. Air was fine. It was the second room that was wrong."
- *Waterline:* "The Ferry landing is under a hand's width. It will be under a hand more by the second storm."
- *Aftercare:* "Ilya stopped sleeping in the hold. He did not say why."

## 6. Content plan

- **W1 — Tide tables & chart lines:** 6 harbours × 4 phases × 2 registers (48).
- **W2 — Harbour voices:** dues, berth, refusal, standing ladder: 6 × 5 (30).
- **W3 — Voyage events:** 24 (weather ×6, piracy ×6, hull failure ×4, stranded ×4, fog/no-signal ×4).
- **W4 — Waterline chronicle:** 12 (per-harbour flood steps) + 6 (losses).
- **W5 — Movements I–V bridging journal lines:** 40.

## 7. Non-goals (restated)

No new map, expedition or dive system. No restoring the retired system. No new save section (nested in `maritime`, DEC-DC-03). No new routed panel. No change to District 8's four decisions.

## 8. Risks

- **Two naval owners** silently disagree. *Bound:* DC-P0 proves or unifies before any vessel state is added.
- **The dive system that "exists" isn't the live one.** *Bound:* DC-P0 F8 check; the plan adds no dive code before it is settled.
- **The waterline punishes without warning.** *Bound:* every step is announced two days ahead (Board line with the cause).
- **Content tone drift toward horror spectacle.** *Bound:* voice lock; the sea is cold, not cruel.

---

## The deeper layer — objects, scenes & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — not a claim,
not an authorization. The register below is unchanged; the fragments are content candidates and
deliberate silences, not new recorded questions.)*

**The second layer.** The waterline is the only antagonist in this corpus with no intentions at
all — and the one that takes things permanently. The two-day announcement is the coast's single
courtesy and its moral centre: the coast may be implacable but it is never *surprising*. This
expansion teaches a skill no other system demands — losing on a schedule, with notice, without
villainy.

**What the expansion leaves lying around.**

> "Berth book: Berth 4, dues paid. Maintained for a berth that is awash. The book is the last place Berth 4 exists."

> "Chart, nine years old. A rock is named. The name is the only part of the chart that is still true."

> "Dive window slip: opens Tuesday, closes Wednesday. A receipt for a window nobody can hold open."

**Scenes the player may piece together.**

> "The announcement is read aloud at the gate. Two days' notice, and then the water decides."

> "A movement passes the old boat ground. Nobody points. The chart stays in someone's bag and stays there."

**Held silences (texture — the register below is unchanged).**

- What the dive windows are windows *onto*. Tide windows open and do not wait; what they frame is the diver's, never the model's.
- Why ice is a modality and not a rescue. Walkable and closed are two kinds of silence, and neither is an answer.

---

## What stays unsaid (tone register — deliberately unanswered)

These are **not gaps and not content backlog.** They are the questions the expansion refuses to
answer, and the refusal is the atmosphere. Any future plan that answers one must say so explicitly
in its own decision register.

**What the six waterline curves are measuring.** Pure function, per harbour, six authored shapes, no
stated cause. Explaining them turns a slow antagonist into a puzzle.

**Who the `keeper_thread_id` on a dive site was.** The field is real and unread in the fiction. It is
allowed to remain an artefact that reads like a name.

**Whether the Black Flotilla and the harbours are the same polity.** They are read from different
standing owners and this expansion deliberately does not reconcile them.

**Why there are five movements and not four.** The fifth is a coda. Numbering it V rather than IV is
a choice nobody recorded.

**What the retired maritime exploration system knew.** It is retired and sealed. Its contents are
sealed with it, and this document will not use it as lore.
