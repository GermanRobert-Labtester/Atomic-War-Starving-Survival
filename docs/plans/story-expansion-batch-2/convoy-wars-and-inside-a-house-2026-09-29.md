# Feature / Task Plan: Movement and War II — Convoy Wars (armoured vehicles, escort and raid) & Inside a House (rank, orders and loyalty crises)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — evidence pass 1 complete (2026-09-29, §2b) — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). **Treat as a first full draft to be expanded and finalised.** Every open point is listed in §18 (Expansion backlog) and §19 (Open Mysteries).

> **Subjects covered (2 of the 16 in this batch):**
> 19. **Convoy Wars** — build armoured vehicles and escort or raid convoys. (Prefix `CW`.)
> 20. **Inside a House** — live inside a faction, with ranks, orders and loyalty crises. (Prefix `IH`.)
>
> **Companions that already exist and are extended, not replaced:** `.ai/plans/long-line-freight-2026-09-29.md` (LF — the trade-route company), `.ai/plans/crews-and-companions-2026-09-29.md` (CC — parties), `.ai/plans/underworld-2026-09-29.md` (UW — heat and hunters), `.ai/plans/iron-road-and-siege-year-2026-09-29.md` (Pair 1 — the rail line and the siege), `docs/lore/05_FACTIONS.md`. Where this plan disagrees with source on *facts*, source wins (Rule 7).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data, no ledger and no other plan. Paths below are *proposed*; `INT` marks integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **7b**, **10b** and **19** carry story texture and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in code. No authority, path, decision, acceptance criterion or verification step is changed by any prose section.

---

## 0. Prologue — The Road and the Roof

> *"Everybody in the wasteland belongs to something. Most of it is a road. A few of it is a roof.
> The road takes your goods and gives you a schedule. The roof takes your name and gives you a table."*

There are two ways the world reaches into a shelter that has learned to keep its door shut. One is
**the road**: four caravans on fixed circuits, each carrying somebody's salt, somebody's grain,
somebody's coal and iron, somebody's small bright things. The road is where the war *goes* — not the
battlefields, which nobody can afford, but the supply columns, the shared water, the toll posts. The
other is **the house**: a faction that does not merely trade with you but *keeps* you — with a rank, an
order, a bed at its table, and a day it expects you to remember.

These two subjects are paired because they are the same question asked of two kinds of loyalty:
*whose goods are these, and whose are you?* A convoy you escort is a promise to a stranger. A rank you
accept is a promise to a body. The interesting moment in both is the same moment — an order arrives
that costs you something you did not know you would be asked for.

**Tone & register.**

- **Convoy Wars is written in the voice of the column.** Load sheets, fuel tallies, the tired
  arithmetic of *who rides where, who sleeps when*. Armour is bolted on with a wrench and a joke.
  The best scenes are the **dull kilometres before** and the **quiet count after**. Never a firefight
  narrated blow by blow: the Formation Table decides, and the prose reports the *margin*.
- **Inside a House is written in the voice of the room.** Mess-hall rules, roll-call, the way a
  sergeant says a name, the way a cell speaks in the first person plural. Warmth and menace share a
  table. Every House is *reasonable* from the inside (DEC-IH-10), and the horror, where there is any, is
  that the reasons are good.

**What the two share.** *Obligation as texture.* A convoy's hold is full of other people's
obligations; a house is nothing *but* obligation with a face. The seam (§12) makes them meet: a House
may order you to escort — or to raid — a caravan whose captain once poured you a cup of water.

**The second layer.** Both halves of this plan are about the moment obligation stops being a
sentiment and starts being a *schedule*. The road converts loyalty into logistics — a hold full of
other people's promises, a fuel number that does not care who made them — while the house converts
it into furniture: a rank, a chair, a day you are expected to remember. The order that costs you
something you did not know you would be asked for is the same sentence in both vocabularies. One
of them is written on a load sheet and one of them is said across a table. And both are entered the same way:
quietly, in a column somebody else will read.

---

## 1. Goal & Outcome

### 1.1 Convoy Wars (CW)

> *Design intent: the vehicle you bolted plate onto should decide something on the road.*

- **Goal:** Turn the existing vehicle owners (`VehicleGarageSystem` with its armor grades, modules and integrity; `ExpeditionSystem` with vehicle profiles; `TravelingCaravanSystem` with four authored caravans) into a **convoy campaign**: a derived **Convoy Rating** read from real garage records; an **Escort** operation that attaches your armoured column to one of the four authored caravans and travels its authored route node by node; a **Raid** operation that lays an ambush at a node the caravan will reach; a small deterministic **Formation Table** that resolves encounters (with an optional bind to tactical combat through the existing binder); and a **Road War** ledger of what your choices cost with each road power (trust, embargo, bounty) through the existing owners.
- **Outcome (observable):**
  1. The player can read, for any set of garage vehicles, a **Convoy Rating** (Guard, Range, Haul, Noise) computed from armor grade, integrity, modules, fuel and crew — never stored.
  2. The player can **Escort** any of the four caravans from the node where it can be met to its next terminal; each hop yields at most one seeded encounter whose outcome is one of five rungs (Rout / Beaten off / Costly / Mauled / Overrun) and writes real wear to the garage, real cargo to the caravan, and real trust to the faction.
  3. The player can **Raid** a caravan: the raid resolves on the same table, marks the caravan robbed through **one additive writer** (E6), moves loot through the existing inventory/caravan ports, and opens a **Road War** with the caravan's faction (trust, embargo, bounty — all existing owners).
  4. The armoured vehicles the player builds feed the trade-route risk engine's escort input (`escortStrengthPermille`) as a read-only number.
  5. Save/load mid-operation round-trips; a legacy save with no CW state loads as "no operation" and every caravan/garage/expedition behaviour equals today's.
- **Non-Goals (Convoy Wars):** no new vehicle catalog (all eight vehicles, twenty modules and five armor grades already exist); no change to armor-grade numbers; no new combat engine (tactical combat is *optional* and bound through `TravelEncounterCombatBinder`); no second caravan or route economy (LF owns the player's own runs); no real-time driving; no rail (Pair 1); no PvP faction war engine (Muster owns war); no new routed panel; no Unity.
- **"Done" (CW):** §13 CW acceptance passes via `bin/run-scoped-tests`; parity holds on a saved corpus with no operation; handoff lists untouched shared paths.

### 1.2 Inside a House (IH)

> *Design intent: the player should be able to say "I am a corporal of the Watch" and be right,
> and feel it cost them something on a Tuesday.*

- **Goal:** Add **service** on top of the existing faction-branch owner (`FactionBranchCoordinator` with its Military/Rebel/Independent branches, PONR lock, endings and hidden PRPF): a **Commission** (house, rank rung, merit), a five-rung **Rank ladder** per House with house-specific names, **Orders** issued as ordinary **Commitments** (existing owner: due day, warning lead, consequence routing), house **Duties, Dues and Perks** routed through existing owners, and authored **Loyalty Crises** that fire at rank thresholds and order conflicts. A House feels like a place: rank is *derived* from merit the existing owners already record; only the *held* rung and the promotion decisions are stored.
- **Outcome (observable):**
  1. After the player commits to a House through the existing branch verbs, a **Commission** exists with rung 1; no commission ⇒ no IH behaviour anywhere (parity).
  2. Every ~7 days (seeded, difficulty-scaled) the House issues **≤ 2 open Orders** as `CommitmentDefinition` rows registered with the existing `CommitmentSystem`; orders are satisfied by ordinary progress events (delivery, escort completed, nights held) and pay/penalise through the Commitment owner's own routing.
  3. **Merit** is derived from met/missed House commitments + standing; when merit crosses a rung threshold a **Promotion** is *offered* (accept/decline — both are stored and both matter).
  4. At authored moments a **Loyalty Crisis** (3 options, each routed through existing owners: moral choice, standing, flags, relationships) fires; each House has four.
  5. Resignation before the PONR is allowed with a **Desertion** consequence; after PONR it is refused (existing rule).
  6. Save/load mid-service round-trips; legacy saves load with no commission.
- **Non-Goals (Inside a House):** no second faction, standing, alignment, PONR or ending system (the branch coordinator owns them); no new save section; no real countries, armies, wars or people (CLAUDE.md tone rule); no player-vs-House combat system; no NPC house AI; guilds and the four caravan factions are **not** Houses in v1; no new routed panel; no Unity.
- **"Done" (IH):** §13 IH acceptance passes; branch-coordinator parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

**Orders that name a convoy.** A House Order may be *"escort caravan X"* or *"raid caravan X"*; CW records the operation result as **progress** on the Commitment through the existing `RecordProgress` (a quantity of 1). CW never reads the House; IH never reads a convoy. Each exposes one small event to a shared adapter (§12). A **Loyalty Crisis** may cite an *escorted-before* fact (`RoadMemory`) — read-only.

---

## 1b. Texture, Mystery & Voice

**Convoy Wars: the margin is the story.**

The Formation Table never says *"you fought."* It says *the margin was thirty points, in your
favour*. The chronicle turns a margin into a sentence: *"We were the stronger column by a hand's
width. The other side knew it and did the sensible thing."* The point of the armour is not to win
fights; it is to make the other side's arithmetic come out badly *before* anyone raises a weapon.
That is why Convoy Wars pays the player back in **encounters that did not happen**, and why the
Screened, Fast and Hidden stances each buy a different kind of absence.

**Inside a House: rank is a kind of debt with a title.**

A promotion is a *bigger promise* with a nicer chair. The design keeps that visible: every rung
comes with an obligation (an Order cadence, a Due, a Duty) and a **Perk** that makes leaving harder.
Declining a promotion is a real act with a real price (merit resets to the rung's floor, the
sponsor's warmth drops one step). Nobody in a House is a cartoon; each has a reason.

**What the player is never told.**

- **Who started the Road War.** Raids in the game have consequences, not a *first cause*. The four
  caravan factions have grievances older than the shelter; none is authored as "the beginning".
- **Why the House chose you.** The sponsor's reason for extending a commission is never stated.
  Whether it was talent, need, or observation is left open (§19).
- **What the PRPF's orders are *for*.** The hidden House issues only *quiet* orders whose purpose
  the game never explains. That silence is the design.
- **Whether the columns are ever "clean".** No escort resolves without a cost; the cleanest
  outcome (Rout) still spends fuel and time. There is no free convoy.

**Voice — sample fragments (content candidates for `convoy_lines.json`, `house_lines.json`).**

> "Fuel gauge on the Base reads 41. Four hops at forty-six a hop, and the caravanserai is the second
> hop, not the fourth. You do the arithmetic. Then you do it again, because that is what a
> column does." — the convoy master's tally (CW)

> "They looked at the plate, and they looked at the gun, and they looked at the number of us. Then
> they looked at each other. Nobody fired. I would like that to be the whole story." — chronicle,
> *Rout* (CW)

> "We did not take the flour. We took the *look* on the steward's face as she counted, and that will
> cost us more than flour." — chronicle, after a Raid (CW)

> "Corporal. It is a word that means someone else is counting you. I have decided that is
> comforting." — Enlistee, first week (IH)

> "The Circle voted eight to one. The one has been asked to explain himself. That is what a vote is
> for." — Cell notes (IH)

> "You may resign. Nobody has ever needed to. We say it anyway, so you know we said it." — the
> Steward of the Long Table (IH)

**Design texture beats.**

- **Every stance buys an absence (CW).** Close buys *time*; Screened buys *surprise*; Fast buys
  *exposure*; Hidden buys *silence*. Each gives up exactly the thing the other buys.
- **One writer per fact (CW/IH).** The caravan's `isRobbed` gets one writer; the House's held rung gets
  one writer; everything else is read.
- **No rank without a debt (IH).** Every rung introduces one Order cadence increment, one Due, one Perk.
- **A House is a table.** IH prose always names a *room* (mess, cellar, long table, quiet room).

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §7b/§10b remain the eleven-entry
chronicles; this section is the **objects** those entries leave behind. §19's register is
unchanged; the fragments below are content candidates and deliberate silences, not new recorded
questions.)*

**What the column leaves lying around.**

> "Load sheet: somebody's salt, somebody's grain, somebody's coal. It is a list of obligations with weights."

> "Fuel tally, 41 on the Base. The arithmetic is done twice, because that is what a column does."

> "Plate, gun, headcount — looked at in that order. The look between the two parties is not on any sheet."

**What the house leaves lying around.**

> "Commission letter. The sponsor's reason is a blank with a flourish. The flourish is the whole characterisation."

> "Roll call: one name slower than the others. The sergeant says every name the same way, which is how the slower name is heard."

> "Resignation paper, unsigned, in a drawer. 'You may resign. Nobody has ever needed to. We say it anyway.'"

**Scenes the player may piece together.**

> "The margin was thirty points. Nobody fired. The chronicle writes the sentence; the prose reports the margin."

> "The Circle voted eight to one. The one explains himself. The explanation is minuted, and the minutes are neutral."

**Held silences (texture, not register rows).**

- Why the House chose *you*. The sponsor's reason is never stated (§19) and the commission letter keeps its blank; the flourish must never be decoded. Texture only.
- Whether the PRPF's quiet orders are instructions or observations. The hidden House issues orders whose purpose the game never explains; whether anyone reads the answers is not modelled and must not be.

**Fourth pass — obligation with a face (texture only; §19 register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §19's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions.)*

**The shape of the polish.** Both halves keep their books in the same grammar: a name, a weight, a
line somebody else will read. The load sheet and the roll call differ only in what they admit to
totalling. Prose should stay overheard and dry — the horror, where there is any, is that the
reasons are good — and let the untotalled column do the feeling.

**What the column leaves lying around.**

> "Load sheet, third column: 'owed by.' The column is not totalled. A column that is not totalled
> is a relationship."

> "A cup of water, kept on a load sheet as a name. The caravan does not know this. The sheet does."

**What the house leaves lying around.**

> "Circle minutes, eight to one. The one's explanation is minuted in the same hand as the vote, and
> the hand takes no side."

**Held silences (texture, not register rows).**

- What the House's chair faces. A rank, a chair, a day you are expected to remember (§0); the room
  is described by its obligations and its windows are not modelled. Texture only.
- Whether the debt between road and house ever settles. The seam makes them meet (§0); no ledger is
  ever closed in the fiction and none may be written closed.

---

## 2. Evidence table (verified 2026-09-29 against the live worktree; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `VehicleGarageSystem` owns per-vehicle `VehicleCustomizationRecord`: `installedSlots` (cargo/protection/mobility/engine), `chassisStressPermille`, `engineFoulingPermille`, `transmissionWearPermille`, `isImmobilized`, and **armor**: `armorGradeId`, `armorIntegrityPermille`, `armorIntegrityMaxPermille`, `armorMaterialProfileId`, `armorPurity`; plus `VehicleRecoveryMission` for stranded vehicles. | `Expeditions/VehicleGarageSystem.cs` L11–57 | LIVE |
| E2 | **Armor grades already exist and are tested**: `vehicle_armor_grades.json` — five grades 0–4 (Stock, Scrap Plate, Sheet Plate, Composite Plate, Alloyed Heavy Plate): mitigation 0/100/150/200/250‰, integrity pool 0/100/140/180/240‰, speed delta 0/−0.02/−0.04/−0.07/−0.10, fuel ×1.00/1.02/1.05/1.08/1.12; grades 3–4 not compatible with `coastal`. Tests `Plan213VehicleArmorGradeTests`, `VehicleArmorGradesTests`. **CLAUDE.md's "Active queue" lists `CF-P6-VEHICLE-ARMOR-GRADES` as *available, unexecuted* — this appears stale.** | `Data/vehicle_armor_grades.json`; `Expeditions/VehicleArmorGradeCatalog.cs`; two test files | **LIVE (finding; confirm in P0)** |
| E3 | Eight vehicles (Utility Quad, Dirt Bike, Cargo Truck, Steam Halftrack, Armored Mobile Base, Salvage Dredger, Scout Motorcycle, Ambulance Rig): max fuel 18–200, cargo 18–380, speed ×0.70–2.40, fuel/km 0.18–0.95, breakdown threshold 0.15–0.30. Twenty modules (armor/cargo/living/weapon/utility) with `defense_bonus`, `speed_modifier`, `cargo_bonus`, `bunk_capacity`, `installation_days`. | `vehicles.json`, `vehicle_modules.json`, `docs/EXPEDITION_VEHICLE_DOMINANCE_TABLE.md` | LIVE |
| E4 | `TravelingCaravanSystem` (`traveling_caravan_system`): four authored caravans with `faction_id`, `route_node_ids` (5 nodes each), `stay_duration_days` (2–3), `guard_count` (4–6), `specialty_goods`, `inventory`, `embargoBlocked`, `isRobbed`. | `TravelingCaravanSystem.cs` L12–52; `caravans.json` | LIVE |
| E5 | The four routes: **Salt & Saline Flotilla** (`faction_the_fleet`, deep coast; 5 guards): black flotilla outpost → radiation zone alpha → merchant caravanserai → abandoned depot → Holdfast. **Verge Grain Convoy** (`faction_rebuilders`, ash flats; 4 guards): caravanserai → depot → arsenal ruin → Holdfast → radiation zone alpha. **Foundry Coal & Iron** (`faction_silent_foundry`; 6 guards): depot → arsenal ruin → flotilla outpost → radiation zone alpha → Holdfast. **Free Trader Circuit** (`faction_the_scale`; 4 guards; stays 3 days): caravanserai → depot → flotilla outpost → radiation zone alpha → Holdfast. Six distinct locations. | `caravans.json` | LIVE |
| E6 | **`isRobbed` has no writer.** It is a public field read in three places (`TravelingCaravanSystem.cs` L197, L232, L371) and copied on capture (L440); no code sets it to `true`. NPC caravans cannot currently be robbed by anyone. | grep `isRobbed = true` / `MarkRobbed` → none | **LIVE (finding; re-verify P0)** |
| E7 | `ExpeditionSystem.Start` takes an optional `ExpeditionVehicleProfile` (vehicle id, speed, breakdown chance, cargo kg); **one expedition per survivor**; state carries `vehicleId`, `vehicleBrokenDown`; **no per-vehicle exclusivity** across survivors (confirmed 2026-09-29). | `Expeditions/ExpeditionSystem.cs` L400–462, L776–784 | LIVE (confirmed) |
| E8 | `TradeRouteRiskBindingEngine.EvaluateTransitRisk(..., escortStrengthPermille, ...)` already accepts an escort strength 0..1000 and returns raid probability, disruption, attrition, escort mitigation. | `Economy/TradeRouteRiskBindingEngine.cs` L42–70 | LIVE |
| E9 | `TravelEncounterCombatBinder.IsHostileChoice` / `TryBind` bind a hostile travel choice to tactical combat; `TacticalCombatSystem` has realtime AI, arenas, breaching, doctrine capability. | `Expeditions/TravelEncounterCombatBinder.cs`; `Combat/` | LIVE |
| E10 | `FactionStanceEngine` (`GetTrust`, `ModifyTrust`, `SetTrust`, `GetStance`, `WillTrade`), `FactionEmbargoLedger` (`TryAddEmbargo(factionId, scope, startDay, durationDays, sourceId)`, `IsEmbargoed`), `FactionBountySystem` (`IssuePatrolBounty`, `ResolveBounty`, `HasActiveBounty`) are existing consequence owners. | `Economy/FactionStanceEngine.cs`; `FactionEmbargoLedger.cs`; `Factions/FactionBountySystem.cs` | LIVE |
| E11 | `FactionBranchCoordinator` (`weight_of_choices` save via `WeightOfChoicesSave`): `Military`/`Rebel`/`Independent` branch systems (15 branches each, entry moral bands, PONR flag, three endings per branch), `Prpf` (hidden third power: `TryJoinPrpf`, `OpposePrpf`, morality-band gate), `CanCommit`, `CommitBranch`, `LockPonr`, `ResolveEnding`, `ModifyStanding`, `ShiftFactionAlignment`, `GetBranchOptions`, `GetFactionStandingSummaries` (with `IsJoined`, `IsHostile`, `IsAllied`, `IsOpposed`). Events `OnBranchCommitted`, `OnPonrLocked`, `OnEndingResolved`. | `Factions/FactionBranchCoordinator.cs` | LIVE |
| E12 | Branch ids `faction_military`, `faction_rebel`, `faction_independent` (systems namespace) and `faction_prpf`; **no rank concept exists** in the branch owners (grep for `Rank`/`rank` finds only unrelated files). | `Data/*_faction_branch.json`; `Factions/PrpfIds.cs`; grep | LIVE (finding) |
| E13 | `CommitmentSystem` (`commitments`, `IDayAdvanceOwner`): `RegisterCommitment(CommitmentDefinition)`, `RecordProgress(id, qty)`, `Settle`, `GetCommitments(day)`, save `{met_ids, missed_ids, progress_by_id, warned_ids}`; definition carries `counterparty`, `start_day`, `due_day`, `warning_lead_days`, `target_quantity`, `target_id`, `condition_type` (`resource_delivery`), `consequence_class`, `consequence_target`, `consequence_magnitude`; events `OnWarningIssued/OnCommitmentMet/OnCommitmentMissed/OnConsequenceRouted`. Three authored commitments exist. | `Commitments/*.cs`; `commitments.json` | LIVE |
| E14 | `FactionActionBoard` (12 authored faction actions, trust bands, `IFactionActionItemSink`, `Resolve`) is the existing *offer* mechanism; it offers *choices*, it does not issue *obligations*. | `Muster/FactionActionBoard.cs`; `muster_faction_actions.json` | LIVE |
| E15 | `muster_faction_culture.json` — 25 culture entries (per-faction prose) read by `FactionCultureCatalog`; `FactionDisplayNameCatalog`, `FactionStandingIdResolver` reconcile namespaces. | `Data/muster_faction_culture.json`; `Factions/` | LIVE |
| E16 | Lore namespace defect: `docs/lore/00_OVERVIEW.md` records two faction-id namespaces (lore/UI vs systems) — *new content must not pick a side until reconciled.* IH uses **systems ids** and the resolver. | `docs/lore/00_OVERVIEW.md` | LIVE (documented) |
| E17 | The word *convoy* names a **catalog loader and UI strings** (`CaravanCatalogLoader`, `TradeScreenPresenter`, `TradeTextCatalog`), a distress destination (`DistressDestinationResolver`), and the Ice Road; there is **no convoy system**. | `grep -ril convoy` | LIVE (finding) |
| E18 | `ArmoredCrawlerExpeditionSystem` (crawler state, installed modules, crew roster, remote camps) and `armored_crawler_modules.json` exist; a *crawler* is a separate expedition vehicle class. | `Expeditions/ArmoredCrawlerExpeditionSystem.cs` | LIVE |
| E19 | Long Line Freight (LF) draft: one driver + optional escort per run, contracts, legs; LF explicitly excludes *ride-along* runs from v1 (DEC-LF-09); its risk binding takes `escortStrengthPermille`. | `.ai/plans/long-line-freight-2026-09-29.md` | LIVE (plan) |
| E20 | CC draft: a **party** = N ordinary expeditions + a coordinator (one-per-survivor stays); cap 4 survivors + 2 companions. | `.ai/plans/crews-and-companions-2026-09-29.md` | LIVE (plan; not implemented) |
| E21 | Save families: caravan state in section **`caravan`** (`CaravanSaveStore`, `TravelingCaravanState`; sibling `caravan_trade_network`); garage in `vehicle_garage`; branch state in `weight_of_choices`; commitments in `commitments`. Never assume a new section is allowed. | `Save/SaveSectionRegistry.cs` L51, L83, L234; `src/Host/CaravanSaveStore.cs` | **RESOLVED (§2b)** |
| E22 | Difficulty scalars: `hostile_encounter_mult`, `equipment_decay_mult`, `market_price_mult`, `crisis_deadline_mult` applied at read sites (XP-WAVE1 COMPLETE). | `difficulty_presets.json` | LIVE |

**Four findings that shape this plan (recorded so nobody rediscovers them mid-package):**

1. **E2 — armor grades are done.** The queue's "vehicle armor grades" package should be *closed or corrected* before anyone starts it; CW **consumes** grades and adds none.
2. **E6 — nothing can rob a caravan.** "Raid convoys" therefore requires exactly one additive writer on the caravan owner (DEC-CW-04) — the smallest possible change.
3. **E12 — no rank exists.** Rank must be a derived reading over merit already recorded by the Commitment and standing owners; the only stored acts are *held rung* and *promotion decisions*.
4. **E17 — "convoy" is not a game concept yet.** CW gives it one small, derivable meaning: *an operation over N ordinary expeditions and one caravan*.

---

## 2b. Evidence pass 1 — premises checked against source (2026-09-29)

Every `VERIFY` in §2 was re-read against the worktree, and the load-bearing LIVE claims were spot-checked. **Confirmed unchanged:** E2 (five armor grades — `grade_0_stock` to `grade_4_alloyed_heavy_plate` — and two test classes, `VehicleArmorGradesTests` and `Plan213VehicleArmorGradeTests`, exist; the queue note in `CLAUDE.md` for `CF-P6-VEHICLE-ARMOR-GRADES` is stale), E6 (`isRobbed` has **no writer** anywhere in `Assets` or `src`), E4, E9–E11. What was resolved or corrected:

| # | Open item | Result | Edit made |
|---|---|---|---|
| E7 | Can one vehicle be used by two survivors? | **Yes — nothing prevents it.** `ExpeditionSystem.Start` records `vehicleId` on the expedition and checks no other expedition's use of the same vehicle (the only vehicle logic is breakdown at L776–784). | Convoy Operations add a **vehicle-exclusivity read** (one vehicle, one live expedition) as a *plan-level rule checked before Muster*, and DEC-CW-11 asks the expedition owner for a one-line guard; until then the Convoy strip refuses to list a vehicle already out |
| E21 | Caravan save section name | **`caravan`** (`CaravanSaveStore.SectionName`, `TravelingCaravanState`), with a sibling **`caravan_trade_network`** (`CaravanTradeSaveStore`). The registry maps the legacy key `caravans` → `caravan`. The traveling-caravan system id is `traveling_caravan_system`. | RoadMemory nests in the `caravan` state (DEC-CW-06) |
| — | Branch host wiring file | `src/Main.FactionBranch.cs`; the coordinator exposes `OnBranchCommitted(FactionBranchKind, branchId)`. | §4 host names it |
| — | Faction / branch panel for the **House strip** | `FactionsPanel`, `FactionMatrixPanel`, `FactionDetailPanel` (and `FactionCultureCodexPanel`) exist in `src/UI/`. | House strip extends `FactionDetailPanel` |
| — | Convoy strip panels | `VehicleGaragePanel` and `TravelingCaravanPanel` exist. | Convoy strip extends both |
| — | PRPF join event | **Exists:** `PrpfStandingSystem.OnJoined` and `OnOpposed` (also `OnStandingChanged`, `OnAlignmentChanged`); `IsJoined` is a property. | IH subscribes; the "poll `Prpf.IsJoined`" fallback is removed |
| — | Per-night watch event | **None.** `DutyRosterSystem` defines `DutyRosterWatchShift` but raises no event when a shift is kept. | `hold_watch` is **counted from the day tick** by reading the roster (DEC-IH-11) |
| — | Visitor-stay event | `VisitorIntegrationSystem` raises `OnVisitorAdmitted`, `OnHousingAssigned(record, roomId, housingType)`, `OnVisitorIntegrated`, `OnVisitorRecruited`. | `harbour` counts days from `OnHousingAssigned` until the record leaves the housed state |
| — | Survivor-wound API | **No dedicated wound owner.** Injury text and effects live in the expedition owner; health moves through `NeedsSystem.ApplyAttributedDelta(id, NeedKind.Health, delta, source)`. | Convoy casualties use that attributed seam with source `convoy:<opId>`; DEC-CW-12 |
| — | Events / dilemmas pipeline | An `IEventBus` exists (`Events/IEventBus.cs`); crisis delivery goes through it or the narrative arc owners. | Loyalty Crisis rows are published on the bus (DEC-IH-06 confirmed in principle; consumer wiring is P-level) |
| — | Host self-test naming | `--vehicle-garage-selftest` (L538), `--caravan-selftest` with alias `--traveling-caravan-selftest` (L1345) in `HostCliRegistry.cs`. | `--convoy-wars-selftest` and `--inside-a-house-selftest` follow the pattern; two registry rows (`INT`) |

**What did not change:** Convoy Operations over N ordinary expeditions and one caravan; RoadMemory; the Checkpoint and Salt Run designs; the House ladder read over merit; one additive writer for `isRobbed` (E6). The evidence removed four assumed events (per-night watch, PRPF polling, a wound owner, a vehicle-exclusivity guard) and named the real ones.

**One consequence worth stating plainly.** The plan's most cinematic idea — two survivors in one truck — is currently *possible by accident*. A convoy that shares a vehicle is not a design; it is a bug the game has not noticed yet. Convoy Wars ships the guard as its first honest act.

---

## 3. Authority table (one authority per concern — CLAUDE.md Rule 5)

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Vehicle records, armor grade, integrity, modules, recovery | `VehicleGarageSystem` | none to state; **one additive** `ApplyConvoyDamage(vehicleId, integrityDeltaPermille, stressDeltaPermille)` *only if P0 finds no existing public armor-damage verb* (DEC-CW-06, `INT`) |
| Expeditions / drivers / vehicle profile | `ExpeditionSystem` | none |
| Caravans: route, stay, guards, inventory, robbed flag | `TravelingCaravanSystem` | **one additive writer** `TryMarkRobbed(caravanId, lootPermille)` (DEC-CW-04, `INT`) |
| Trade-route risk | `TradeRouteRiskBindingEngine` | read-only input: Convoy Rating → `escortStrengthPermille` |
| Trust, embargo, bounty | `FactionStanceEngine`, `FactionEmbargoLedger`, `FactionBountySystem` | writes through their public methods only |
| Tactical combat | `TravelEncounterCombatBinder`, `TacticalCombatSystem` | optional bind; result mapped to the same five rungs |
| Convoy operations, Road Memory, Road War history | — | `ConvoyWarsSystem` (pure Core) — **nested additive** DTO in the caravan save (DEC-CW-02) |
| Branch commit, PONR, endings, standing, alignment, PRPF | `FactionBranchCoordinator` | none |
| Obligations (due day, warning, consequence routing) | `CommitmentSystem` | **House Orders are Commitments** registered at runtime (no new obligation type of state) |
| Faction culture prose | `FactionCultureCatalog` | read-only |
| Commission (held rung, promotions) | — | `HouseServiceSystem` (pure Core) — **nested additive** DTO in `WeightOfChoicesSave` (DEC-IH-02) |
| Events / dilemmas delivery | existing `IEventBus` (`Events/IEventBus.cs`) — verified §2b | Loyalty Crisis rows delivered through it (DEC-IH-06) |
| Chronicle | Plan 34 chronicle / `ArchiveDeskState` | append-only writes |

**Non-duplication statement.** One garage, one caravan owner, one obligation owner, one branch coordinator, one stance/embargo/bounty triple, one chronicle. This plan adds two small state owners — `ConvoyWarsSystem` (operations and Road Memory) and `HouseServiceSystem` (held rung and promotion decisions) — that hold **only** facts no existing owner can hold. Convoy Rating, merit, rung eligibility, Road War exposure and every readout are **derived**.

---

## 4. Claimed Paths (proposed; `INT` = integrator-owned)

**Core (CW):** `Assets/Ashfall.Core/Expeditions/ConvoyWarsSystem.cs` (new, pure), `Expeditions/ConvoyRating.cs` (new, pure read model), `Expeditions/ConvoyFormationTable.cs` (new, pure), `Expeditions/ConvoyCatalog.cs` (new loader), `TravelingCaravanSystem.cs` (**one additive writer**, `INT`), `Expeditions/VehicleGarageSystem.cs` (**at most one additive method**, `INT`), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`).

**Core (IH):** `Assets/Ashfall.Core/Factions/HouseServiceSystem.cs` (new, pure), `Factions/HouseRankLadder.cs` (new, pure derivation), `Factions/HouseOrderIssuer.cs` (new), `Factions/HouseCrisisCatalog.cs` (new loader), `Factions/FactionBranchCoordinator.cs` / `WeightOfChoicesSave` (**additive nested DTO field only**, `INT`).

**Data (CW):** `convoy_routes.json`, `convoy_threats.json`, `convoy_tuning.json`, `convoy_lines.json`, `convoy_road_war.json`.
**Data (IH):** `house_ranks.json`, `house_orders.json`, `house_crises.json`, `house_perks.json`, `house_lines.json`.

**Host:** `src/Main.VehicleGarage.cs` (`INT`), the caravan host wiring (`src/Main.GameFlow.cs` / `Main.ExpandedShelterSystems.cs` — `INT`), branch host wiring (`src/Main.FactionBranch.cs`, verified §2b), phase-5 day-owner registration (`src/Main.SubsystemComposition.cs`, `INT`).

**Presentation (both):** extend existing surfaces only — the vehicle garage panel and the trade/caravan panel for a **Convoy strip**; `FactionDetailPanel` (with `FactionsPanel`/`FactionMatrixPanel`) for a **House strip**. No new routed panel (DEC-CW-09, DEC-IH-08).

**Tests:** `Ashfall.Core.Tests/Expeditions/ConvoyRatingTests.cs`, `ConvoyFormationTableTests.cs`, `ConvoyWarsSystemTests.cs`, `Save/ConvoyWarsSaveTests.cs`; `Ashfall.Core.Tests/Factions/HouseRankLadderTests.cs`, `HouseServiceSystemTests.cs`, `HouseOrderIssuerTests.cs`, `HouseCrisisTests.cs`, `Save/HouseServiceSaveTests.cs`; extend caravan and branch-coordinator tests for writer-never-called parity.

---

# PART ONE — CONVOY WARS (CW)

## 5. Convoy Wars — state and data model

### 5.1 What is stored

Almost nothing. The **Convoy Rating**, the hop fuel estimate, the encounter chance, the caravan's ETA and
the Road War exposure are all *reads*. The stored facts are the operation the player committed to, and a
small *memory* of what happened, because no existing owner can remember *"I escorted the Salt Run twice
and raided the Grain Convoy once"*.

```text
ConvoyWarsState                          // nested additive DTO in the traveling-caravan save (DEC-CW-02)
{
  int SchemaVersion = 1
  int OpSeq                              // monotonic operation counter (stable ids)
  List<ConvoyOperation> Ops              // active only; cap 2 (DEC-CW-05)
  List<ConvoyLedgerEntry> History        // cap 32, oldest folds into RoadMemory counters
  Dictionary<string, RoadMemoryEntry> RoadMemory   // key = caravan id
}

ConvoyOperation
{
  string OpId                            // "op:<seq>"
  ConvoyKind Kind                        // Escort | Raid
  string CaravanId
  ConvoyState State                      // Mustered | RollingOut | OnRoute | Resolved | Abandoned
  int StartDay
  string AttachNodeId                    // Escort: node where the column joins; Raid: ambush node
  int HopIndex                           // next hop to resolve (Escort) ; 0 for Raid
  List<ConvoyMember> Members             // one per vehicle
  List<int> StanceByHop                  // stance enum per hop, chosen before rolling out; editable until the hop starts
  List<HopLogEntry> Log                  // { Hop, Day, NodeId, EncounterKind, MarginBand, Rung, Effects[] }
  int FeePermille                        // Escort: agreed fee share of caravan goods; Raid: 0
}

ConvoyMember { string VehicleId, string DriverId, List<string> CrewIds }

RoadMemoryEntry { string CaravanId, int Escorts, int Raids, int Passes, int LastEscortDay = -1,
                  int LastRaidDay = -1, string LastRung }

ConvoyLedgerEntry { int Day, string OpId, ConvoyKind Kind, string CaravanId, string Rung,
                    int TrustDelta, int EmbargoDays, int LootPermille }
```

**Deliberately not stored:** vehicle stress/fuel/integrity (garage), caravan position/stay/goods
(caravan owner), trust/embargo/bounty (their owners), Convoy Rating, Road Standing band, road-war rung
(a *count* of Raids in `RoadMemory` decides it), the Convoy strip's text (rendered).

**Size.** ≤ 2 operations × ≤ 4 members + ≤ 32 ledger rows + 4 memory rows ≈ 6 KB worst case.

### 5.2 Save wiring

- Nested additively in the traveling-caravan save DTO (E21 — P0 confirms the section name). No new
  section (DEC-CW-02). A missing `ConvoyWarsState` ⇒ no operation ⇒ every behaviour equals today.
- `RestoreState`: unknown caravan ids in `RoadMemory` are kept but inert; an operation whose caravan no
  longer exists moves to `Abandoned` with a ledger line and no consequence.
- Operations reference vehicles and survivors **by id**; a missing vehicle/driver at restore moves the
  op to `Abandoned` (never crashes, never invents a member).

### 5.3 Catalogs (summary; row specs §11)

| File | Loader | Content |
|---|---|---|
| `convoy_routes.json` | `ConvoyCatalog` (new) | route hops per authored caravan pair: `from_node`, `to_node`, `hop_km`; refuel nodes |
| `convoy_threats.json` | same | six location threat rows: kind, threat‰, strength, band label, prose keys |
| `convoy_tuning.json` | same | stance table, Guard constants, rung thresholds, per-rung effects, reprisal ladder, Noise limits |
| `convoy_road_war.json` | same | trust/embargo/bounty ladders, road-pass flag ids, fee rows |
| `convoy_lines.json` | same | template lines for rungs, stances, raid outcomes (variant pick by `StableHash`, not RNG) |

---

## 6. Convoy Wars — rules

### 6.1 Convoy Rating (derived, never stored)

For a set of vehicles *M* (each with its garage record, its modules, its crew) the read model returns
four numbers:

```text
V_v      = ( 100  +  5 × Σ defense_bonus(modules_v)  +  mitigation_permille(armorGrade_v) / 5
              +  armorIntegrityPermille_v / 20 )  ×  crewFactor_v / 1000
crewFactor_v = min(1000, 500 + 250 × extraCrew_v)          // extraCrew = crew beyond the driver, capped at 2
Guard    = V_(1) + V_(2) + 0.8 × Σ_{i ≥ 3} V_(i)           // sorted descending; column discount
Range    = floor( min_v  fuelNow_v / hopFuel_v(stance = Close) )     // hops the thinnest tank can make
Haul     = Σ_v ( cargo_capacity_v + Σ cargo_bonus(modules_v) )       // kg-equivalents the hold can take
Noise    = Σ_v noisePoints_v                                          // from convoy_tuning (vehicle + module tags)
EscortStrengthPermille = clamp( Guard × 1000 / GuardForFullEscort, 0, 1000 )   // feeds LF's risk engine (read-only)
```

- `hopFuel_v = hop_km × fuel_consumption_per_km_v × armorFuelMultiplier_v × stanceFuel(stance)` — every
  factor is an *existing* number (E2, E3) except `hop_km` and `stanceFuel` (data, §11).
- **Broken vehicles count zero.** An `isImmobilized` vehicle contributes 0 to Guard and Range; a vehicle
  whose `armorIntegrityPermille` is 0 loses the mitigation term but keeps its modules.
- `Noise` decides whether **Hidden** is allowed (`Noise ≤ HiddenNoiseLimit`, default 40).

The read model is `ConvoyRating.Compute(IGarageReader, IExpeditionVehicleReader, catalog, members)`;
pure, no writes, ordinal-stable tie-breaking on vehicle id.

### 6.2 Stances (chosen per hop, before rolling out; editable until the hop starts)

| Stance | Exposure (encounter chance ×) | Guard adjust | Fuel × | What it buys | What it costs |
|---|---:|---:|---:|---|---|
| **Close** | 900‰ | +40 | 1.00 | tight column; fewer losses when hit | slow to see danger; the most encounters |
| **Screened** | 700‰ | 0 | 1.10 | scouts ahead; fewer surprise hits | one vehicle is a scout (its Guard counts 0 this hop) |
| **Fast** | 550‰ | −60 | 1.20 | dash: shorter exposure | stretched column; heavier fuel; more damage when hit |
| **Hidden** | 350‰ | −30 | 0.95 | night running; lowest exposure | only if `Noise ≤ limit`; +1 day on the hop (data); no trailer |

Each stance is a **different absence** (§1b): time, surprise, exposure, silence. No stance dominates on
the shipped numbers (tested — §16 "no strict dominance" table over the six threat rows).

### 6.3 Escort operation

```text
Escort(caravanId, attachNodeId, members, stances[])
  precondition: caravan route contains attachNodeId with ≥1 downstream hop to Holdfast;
                every member vehicle serviceable (¬isImmobilized), driver free, fuel ≥ Range for the hops chosen
  1. Roll out to attachNodeId by ordinary expedition(s) (existing owner; Design A, DEC-CW-03).
     If the column arrives after the caravan has left the node (read from routeIndex/daysAtCurrentNode/stay_duration_days):
     op → Abandoned ("missed the caravan"), no consequence, fuel spent.
  2. Column joins on arrival: State = OnRoute. For each hop in caravan order:
        encounter := roll(stance, threat(destinationNode))         // §6.5
        if encounter: resolve on the Formation Table (or bind to tactical combat — §6.6)
        apply effects (§6.7); write HopLogEntry
  3. At Holdfast: Resolved. Pay fee (§6.8); RoadMemory.Escorts++; ledger entry; chronicle line.
```

Hops are **the caravan's** hops: the escort cannot speed the caravan up or slow it down. Each hop
takes the caravan's `stay_duration_days` (2–3), which makes a two-hop escort a **four-to-six day**
commitment and a four-hop escort a **week or more** — by design, an escort *costs the shelter its
drivers for a season of the road*.

### 6.4 Raid operation

```text
Raid(caravanId, ambushNodeId, members, stance)
  precondition: ambushNodeId ∈ caravan route, ≠ loc_holdfast (no raiding your own doorstep, DEC-CW-07);
                caravan not already robbed; not embargoBlocked-by-us; ≥1 vehicle with Guard ≥ MinRaidGuard
  1. Roll out to ambushNodeId (ordinary expedition). Arrive ≤ caravan ETA − 0 days, else Abandoned.
  2. On the caravan's arrival: options —
        Demand a toll   → success chance from Guard ratio & the faction's stance (§6.8); yields TollPermille (15%)
        Take the cargo  → Formation Table with attacker margin (§6.5) and the caravan's Strength
        Let them pass   → RoadMemory.Passes++ (no consequence; a fact the House may cite)
  3. Loot moves through the existing inventory/loot port; `TryMarkRobbed(caravanId, lootPermille)` sets
     `isRobbed` (E6) and removes the goods from the caravan's inventory through *its* public surface.
  4. Road War consequences (§6.8). Chronicle line.
```

Caravan Strength (attacker's opposition) is derived: `guard_count × GuardUnit (90) + node.fortification + reprisalBonus(RoadMemory.Raids)`.
Ambush surprise: **Hidden** stance gives the attacker **+80** on the first exchange (a data value) and is
the only stance that can stack surprise.

### 6.5 Encounters and the Formation Table

**Encounter roll (per hop, at arrival):**

```text
p = threat_permille(node) × exposure(stance) / 1000                       // e.g., Depot 450 × Close 900 / 1000 = 405
encounter = stream(opHash, hop).NextInt(1000) < p        // CampaignStreamIds fork; never System.Random
if difficulty scalar `hostile_encounter_mult` != 1000: p' = clamp(p × mult / 1000, 0, 950)     // read-site only (E22)
```

**Margin and rung (deterministic given the roll's *variance* term):**

```text
variance = stream(opHash, hop, "var").NextInt(-60, 61)          // ±60 to keep one number from deciding the world
margin   = Guard + stanceGuardAdjust(stance) + surprise − Strength(node) + variance
rung     = margin ≥ +300 → ROUT
           margin ≥ +100 → BEATEN_OFF
           margin ≥  −99 → COSTLY
           margin ≥ −300 → MAULED
           otherwise     → OVERRUN
```

For a **Raid** the same table is read with **attacker margin** (`Guard − CaravanStrength`), and the
rungs are renamed **Yield / Partial yield / Stalemate / Driven off / Broken** (§6.4, §11).

**Node kinds** (v1 supports three; `Exposure` is deferred — §18):

| Kind | Meaning | Table |
|---|---|---|
| `Ambush` | hostile band; strength in `convoy_threats.json` | Formation Table |
| `Checkpoint` | a toll/inspection post (the Fleet's port, market law at the caravanserai) | *no fight*: pay `toll_permille` of cargo or refuse (→ Ambush strength `refusal_strength`) |
| `Breakdown` | rough ground / hot ground; vehicle risk only | per vehicle: `breakdown_threshold` × exposure vs seeded roll; failure → `isImmobilized` via garage owner + recovery mission |

### 6.6 Optional tactical bind

At an `Ambush` encounter the player may choose **Fight it out** instead of *Stand* (the Table). The host
calls `TravelEncounterCombatBinder.TryBind` (E9); the combat outcome maps **once** to a rung:

| Combat result | Rung |
|---|---|
| victory, 0 casualties | ROUT |
| victory, ≥ 1 casualty | BEATEN_OFF (0) / COSTLY (≥ 2) |
| withdrawal (Fast stance only) | MAULED |
| defeat | OVERRUN |

The bind is **optional and never required**; with no combat scene available the Table alone resolves
(DEC-CW-08).

### 6.7 Effects per rung (data: `convoy_tuning.json`)

| Rung | Garage (via owner) | Cargo (escorted caravan) | Crew | Notes |
|---|---|---|---|---|
| ROUT | none | 0% | none | fuel/time only |
| BEATEN_OFF | −30‰ integrity, 1 vehicle (lowest integrity) | 0% | none | |
| COSTLY | −80‰ integrity on 2 vehicles; +stress 40‰ | −10% | 1 minor injury | |
| MAULED | −150‰ on 2; one `isImmobilized` + recovery mission | −30% | 1 serious injury | |
| OVERRUN | two immobilized + recovery missions | `TryMarkRobbed(600‰)` | injuries per crew | op → Abandoned; convoy dissolves |

Damage routes through the garage owner's existing damage path (or the single additive method, DEC-CW-06); cargo loss
through the caravan owner's inventory; injuries through the existing injury owner (no dedicated wound owner exists; health moves through `NeedsSystem.ApplyAttributedDelta(id, Health, delta, "convoy:<opId>")` — §2b).

### 6.8 Road War (consequences, all through existing owners)

| Act | Trust (`FactionStanceEngine.ModifyTrust`) | Embargo (`FactionEmbargoLedger.TryAddEmbargo`) | Bounty (`FactionBountySystem`) | Other |
|---|---|---|---|---|
| **Escort resolved** | `+6` (+2 per consecutive escort of that faction, cap +20) | — | clears `HasActiveBounty` for that faction if ≥ 2 escorts | Road Pass flag 30 days; fee (below) |
| **Toll taken** | `−4` | — | — | RoadMemory note |
| **Raid — Yield / Partial** | `−12 −4 × priorRaids` (cap −40) | `10 + 5 × priorRaids` days (cap 40) | `IssuePatrolBounty` if the faction has patrols and rung ≥ Partial | reprisal guard bonus next time |
| **Raid — Stalemate** | `−6` | none | none | |
| **Raid — Driven off / Broken** | `−3` | none | none | your vehicles are the story |

**Reprisal ladder (derived from `RoadMemory.Raids`, never stored as a level):** 0 → *Unwatched*; 1 →
*Watchful* (+0 Strength); 2 → *Wary* (+45 Strength); ≥ 3 → *Hostile* (+90 Strength and the faction's
caravans skip the shelter's route entirely for 10 days *through the embargo ledger*, not by editing routes).

**Fee (Escort).** A row per caravan in `convoy_road_war.json` gives `fee_permille` of the caravan's
`specialty_goods` (default 8%), delivered through the existing `IFactionActionItemSink` port (E14) at Holdfast,
plus trust. No new currency.

### 6.9 Worked examples (all numbers from E2–E5; re-verify at P0)

**Example 1 — a Guard number.** *Armored Mobile Base* + `reinforced_hull` (+20) + `mounted_gun` (+10) +
`smoke_launcher` (+5), armor grade 3 (mitigation 200‰, pool 180), integrity full, driver + gunner:
`V0 = 100 + 5×35 + 200/5 + 180/20 = 100 + 175 + 40 + 9 = 324`; crewFactor = 500 + 250×1 = 750 ⇒ **V = 243**.
A *Cargo Truck* with `spike_strips` (+8), grade 1 (100‰, pool 100), driver only:
`V0 = 100 + 40 + 20 + 5 = 165`; crewFactor 500 ⇒ **V = 82**. Column *AMB + Truck + Truck*:
`Guard = 243 + 82 + 0.8×82 ≈ 391`.

**Example 2 — the depot, three stances.** Depot ambush: threat 450‰, strength 320.
- **Close**: p = 450×900/1000 = **405‰**; margin = 391 + 40 − 320 = **+111** ⇒ **BEATEN_OFF** (±60 variance may make it +51..+171: still *Beaten off* or *Costly*).
- **Fast**: p = 450×550/1000 = **247‰**; margin = 391 − 60 − 320 = **+11** ⇒ **COSTLY**.
- **Hidden** (Noise: AMB 30 + trucks 10 + 10 = 50 > 40): **not allowed**. Drop one truck ⇒ Noise 40 ⇒ allowed: Guard 243+82 = 325; p = 450×350/1000 = **158‰**; margin = 325 − 30 − 320 = **−25** ⇒ **COSTLY**, but only a 16% chance of meeting anyone at all.
So *Close* trades exposure for safety when hit; *Hidden* trades safety when hit for **not being hit**.

**Example 3 — fuel realism.** Depot → Holdfast is authored at 42 km. AMB with grade 3: `0.95 × 1.08 = 1.026`/km ⇒
**43.1** fuel per hop Close; over the four-hop Salt Run that is **172** of the AMB's 200 tank. Each Cargo Truck (grade 1):
`0.5 × 1.02 = 0.51` ⇒ **21.4**/hop ⇒ **86** over four hops **> its 80 tank**. The truck *must* refuel at the caravanserai
(a `refuel: true` node) — a real decision, priced in existing fuel units.

**Example 4 — a Raid on the Grain Convoy.** Verge Grain (4 guards): Strength = 4×90 + node fortification (arsenal ruin 30) +
reprisal(0) = **390**. Column Guard 431 (Close) ⇒ margin +41 ⇒ **Stalemate** (loot 15%); Guard 520 ⇒ +130 ⇒ **Partial yield** (40%);
Guard ≥ 720 ⇒ **Yield** (60%). Trust −12 (first raid), embargo 10 days; a second raid: trust −16, embargo 15 days, Strength +45.

---

## 7. Convoy Wars — authored content

### 7.1 Route hops (authored in `convoy_routes.json`; P0 reconciles with the location graph)

| From | To | km | Used by |
|---|---|---:|---|
| black flotilla outpost | radiation zone alpha | 48 | Salt Run; Foundry; Free Trader |
| radiation zone alpha | merchant caravanserai | 36 | Salt Run |
| merchant caravanserai | abandoned depot | 30 | Salt Run; Grain; Free Trader |
| abandoned depot | Holdfast | 42 | Salt Run |
| abandoned depot | arsenal ruin | 34 | Grain; Foundry |
| arsenal ruin | Holdfast | 50 | Grain |
| Holdfast | radiation zone alpha | 44 | Grain (outbound) |
| arsenal ruin | black flotilla outpost | 60 | Foundry |
| abandoned depot | black flotilla outpost | 56 | Free Trader |
| radiation zone alpha | Holdfast | 44 | Foundry; Free Trader |

`refuel_nodes`: merchant caravanserai (market law), Holdfast.

### 7.2 Threat rows (six locations; `convoy_threats.json`)

| Location | Kind | Threat ‰ | Strength | Face (authored, band, no motive) |
|---|---|---:|---:|---|
| black flotilla outpost | Checkpoint | 150 | 150 (refusal) | the Fleet's port inspectors; polite, thorough, armed |
| radiation zone alpha | Breakdown | 500 | — | hot, broken ground; the road bends around something no one names |
| merchant caravanserai | Checkpoint | 60 | 60 (refusal) | market law; refuel; the cheapest safe night |
| abandoned depot | Ambush | 450 | 320 | **the Cutters** — a road band that lives on toll and takes the rest |
| arsenal ruin | Ambush | 550 | 480 | old automated defences and the scavenger crews who learned them |
| Holdfast | Checkpoint | 0 | — | home |

**The Cutters** are the campaign's first named road power: a band, not a faction; the depot is theirs by
habit. *Cutters* are not in any faction catalog (E5); they exist **as a threat row only** (DEC-CW-10) and are
deliberately never given a leader, a doctrine or a reason (§19).

### 7.3 The four caravans as four characters (the "road powers")

| Caravan | Road captain (authored face) | Manner | What they want from the shelter |
|---|---|---|---|
| **Salt & Saline Flotilla** (`faction_the_fleet`) | *Purser Talvi Renn* | exact, wry, counts everything twice | clean escort through the hot ground; a friend at Holdfast |
| **Verge Grain Convoy** (`faction_rebuilders`) | *Steward Oduya Bell* | plain, tired, honest about weights | grain reaches the ward; she does not want to be thanked |
| **Foundry Coal & Iron** (`faction_silent_foundry`) | *Foreman Hadrian Kolb* | loud, proud, respects plate | escorts that do not slow the heavy cars |
| **Free Trader Circuit** (`faction_the_scale`) | *Marta Quill, factor* | quick, funny, keeps every promise small | a column that doesn't make the customers nervous |

(Names are fictional and content candidates; if `characters.json` already holds a caravan-captain row, the existing id wins and
this table is updated — Rule 7.)

### 7.4 The Season of the Road (optional four-chapter arc; gate `convoyWarsEnabled` in the Chapter Profile)

| Ch | Title | Beat | Mechanical anchor |
|---|---|---|---|
| 1 | **The Circuit** | Marta Quill asks for one column for one leg. First escort; the stances are taught by her patter, not a tutorial. | Free Trader; attach at flotilla outpost (1 risky hop) |
| 2 | **Grain and Iron** | Steward Bell needs grain moved through the arsenal; Foreman Kolb needs plates moved *fast*. The two jobs cannot both be done well. | Grain (arsenal ruin hop) vs Foundry (depot→arsenal hop) |
| 3 | **The Cutters** | The depot toll rises. Options: pay, break them, or hire them. Each closes a door. | authored choice; threat row `Ambush` at depot is *modified only through data rows*, never code |
| 4 | **Salt** | The Fleet's purser asks for a favour that costs someone else's convoy (an IH seam, §12). | Salt Run; radiation zone alpha `Breakdown` |

**Endings are readouts, not systems** (Epilogue matrix reads them): *Road Charter* (≥ 3 factions with ≥ 2 escorts and no raid),
*Toll Keeper* (Cutters co-opted), *Struck-Off* (Hostile with ≥ 2 factions), *Quiet Road* (few operations, no incidents — the road
forgot you, which is its own answer). Each has three sentences of authored text in `convoy_lines.json`.

### 7.5 Contract prose (content candidates)

> **Escort offer — Free Trader Circuit.** *"One column, two hops, back by the fourth day. I pay in what I carry, which is
> cheerful and slightly stolen. Do you drive fast or careful?"* — Marta Quill
>
> **Checkpoint — black flotilla outpost.** *"Cargo declared, fuel declared, faces declared. We do not stop what we cannot
> inspect. We stop what refuses to be inspected."* — Fleet port inspector
>
> **Breakdown — radiation zone alpha.** *"The road bends here for a reason nobody remembers. Nobody straightens it."* — convoy master's tally

### 7.6 Aftermath lines (five rungs × three variants each in data; samples)

- **ROUT.** *"The other side looked at the plate and the number of us and decided to be elsewhere."*
- **BEATEN_OFF.** *"A few scratches on the Base's flank. The steward asked whether that was the price. I said it was the receipt."*
- **COSTLY.** *"We were the stronger column by a hand's width. It cost a plate and a night's temper."*
- **MAULED.** *"The Truck is on its side. We are not going to talk about the Truck."*
- **OVERRUN.** *"The caravan is gone. The road has closed over it like water. We do the count."*

---

## 7b. A Column in Eleven Entries (prose texture — not authority)

*A convoy master's tally book across two escorts of the Salt Run, attaching at the caravanserai. Numbers derive from §6: hops
caravanserai → depot (30 km, Ambush 450‰/320) → Holdfast (42 km); the Salt Run stays 2 days per hop.*

**Day 41 — Purser Renn at the gate.** She counts our vehicles before she counts our faces. One leg from the caravanserai, two hops,
four days. The hot ground upstream is hers to cross alone; she says it with a straight face and a very slight smile. *"The Fleet
does not ask for company through the bad ground. It asks for company where the ground has opinions."*

**Day 42 — The Base and two Trucks.** We took the Armored Base because it has a gun and a temper, and two Cargo Trucks with spike
strips because they are cheap and because the Base cannot carry everything. Guard 391 by the sheet. Fuel for two hops (30 km, then 42): the Base
burns 31 and 43, the Trucks 15 and 21. The Truck's 80-unit tank is the thin one; the Truck decides how far we go, not the Base.

**Day 43 — Stances.** Close for both hops. Purser Renn says Close is what a column chooses when it wants the customers to feel
safe. I said Close is what a column chooses when it wants them to *stay* customers. She said that was the same sentence.

**Day 44 — The caravanserai.** Refuel to the neck. The column joins the Salt Run on the market's own cobbles; the Fleet's tally
clerk counts twice, nods once. Purser Renn buys a cup for every driver and says it is for the *count*, not the cup.

**Day 46 — The depot.** Chance 405 in a thousand, and we drew under it. Two vehicles on the ridge — not the Cutters' whole band.
Margin +111 with the variance. They looked at the plate, and they looked at the gun, and they looked at the number of us. Nobody
fired. I would like that to be the whole story.

**Day 46 — The tally.** One scratch on the Base. Fuel 31 for the hop. The Truck's tank at 65. The Cutters' toll, we hear later, has
gone up by a quarter. We do not hear from whom.

**Day 48 — Holdfast.** The Salt Run unloads. Purser Renn pays in what she carries: eight percent of the clean water and a quiet
*"thank you"* that costs her more than the water. *Trust +6.* The chronicle notes: *Salt & Saline Flotilla, Escort 1.*

**Day 71 — The second time.** The steward tells us the Cutters have raised the toll again. We do not get to decide whether they
raise it; we get to decide *who we are while it happens*.

**Day 72 — A quiet column.** One Truck stays home. Noise 40, no trailer. Hidden. Chance 158 in a thousand. We draw over it.
Nobody meets anybody. Purser Renn says we arrived *"in a sentence with no verbs"*. I put that in the book.

**Day 76 — Holdfast, again.** Escort 2. Trust +8 (a second escort of the same faction). A Road Pass, thirty days, written on the
back of the tally sheet in the Purser's own hand.

**Day 77 — The count.** Escorts: two. Raids: none. Passes: none. Road standing with the Fleet: *Friend of the road*. The road, as far
as the road can tell, has decided to know our name. It does not say what it thinks of it.


---

# PART TWO — INSIDE A HOUSE (IH)

## 8. Inside a House — state and data model

### 8.1 What is stored, and why so little

The branch coordinator already stores *which path the player is on*, *the PONR*, *standing and alignment*. The
Commitment owner already stores *which orders were met, missed, and how far progress got*. What no owner stores is
the **shape of a service**: the rung the player *holds*, the promotions they accepted or declined, the crises they
faced, and how warm the sponsor is. That is all this plan stores.

```text
HouseServiceState                          // nested additive DTO in WeightOfChoicesSave (DEC-IH-02); null = no commission
{
  int SchemaVersion = 1
  Commission Current                       // null when not serving
  List<PastCommission> Past                // cap 6: { HouseId, FromDay, ToDay, PeakRung, EndReason }
}

Commission
{
  string HouseId                           // faction_military | faction_rebel | faction_independent | faction_prpf
  string BranchId                          // the branch path chosen through the existing coordinator
  int    CommissionedDay
  int    HeldRung = 1                      // 1..5 — the ONLY rung stored; eligibility is derived
  int    RungSinceDay
  string SponsorId                         // authored NPC id from house_ranks.json
  int    Warmth = 2                        // 0 Cold, 1 Cool, 2 Warm, 3 Close
  int    MeritAdjust                       // sum of crisis/decline adjustments — the only merit not derivable
  int    OrderSeq                          // monotonic order counter (stable commitment ids)
  int    LastOrderDay = -1
  int    LastDueDay  = -1
  int    DuesMissed
  List<PromotionRecord> Promotions         // { Rung, OfferedDay, Decision: Pending|Accepted|Declined, DecidedDay }
  List<CrisisRecord>    Crises             // cap 24: { CrisisId, Day, OptionId }
}
```

**Not stored:** merit, eligible rung, open orders (Commitment owner), perks (derived from held rung),
standing/alignment (coordinator), the House strip text.

**Size.** ~1.2 KB typical, ≤ 3 KB worst case.

### 8.2 Save wiring

- Nested additively in `WeightOfChoicesSave` (the branch coordinator's save; E11). No new section (DEC-IH-02). Missing state ⇒ no
  commission ⇒ **zero** IH behaviour (parity, acceptance 11).
- `RestoreState`: an unknown `HouseId` keeps the record inert; a `HeldRung` outside 1..5 clamps and logs one warning; Commitments
  are restored by their own owner, and **orders are never re-registered from `HouseServiceState`** (avoiding double-registration);
  a mismatch (state says an order is open but the Commitment owner has no such id) is reconciled by *closing* the state's view,
  not by inventing a commitment.

### 8.3 Catalogs (summary; row specs §11)

| File | Loader | Content |
|---|---|---|
| `house_ranks.json` | `HouseRankLadder` (new) | 4 houses × 5 rungs: names, merit thresholds, min days at rung, sponsors, rooms |
| `house_orders.json` | `HouseOrderIssuer` (new) | 24 authored orders: kind, target, quantity, due, merit ±, consequence, requirements |
| `house_crises.json` | `HouseCrisisCatalog` (new) | 17 authored Loyalty Crises: trigger, situation, three options with closed-set effects |
| `house_perks.json` | same | 12 perks (closed effect kinds), each tied to a rung |
| `house_lines.json` | same | template lines for orders, dues, promotions, resignation (variant pick by `StableHash`) |

---

## 9. Inside a House — rules

### 9.1 Houses in v1

| House | Id (systems namespace, E16) | Existing verbs the plan rides | Room |
|---|---|---|---|
| The Military | `faction_military` | `MilitaryBranchSystem` (15 branches, PONR, 3 endings each) | the Mess |
| The Rebel cells | `faction_rebel` | `RebelBranchSystem` | the Cellar |
| The Independents | `faction_independent` | `IndependentBranchSystem` | the Long Table |
| The Quiet (hidden) | `faction_prpf` | `PrpfStandingSystem` (`TryJoinPrpf`) | the Quiet Room |

Display names come from `FactionDisplayNameCatalog` / `FactionStandingIdResolver` (E15, E16); **this plan authors no namespace
choice.** Guilds and the four caravan factions are *not* Houses in v1 (§18).

### 9.2 Commission lifecycle

```text
None ──existing commit/join verb──▶ Commissioned(rung 1)
   ▲                                     │  merit ≥ threshold(rung+1) ∧ days ≥ minDays ∧ no open crisis
   │                                     ▼
   │                              PromotionOffered ──accept──▶ Commissioned(rung+1)
   │                                     └──decline──▶ Commissioned(same rung; MeritAdjust −, Warmth −1)
   ├───── resign (¬PONR) ────────────────┘   (Desertion consequence, §9.7)
   └───── EndingResolved / demotion to 0 ──▶ Past
```

- **Creation trigger:** the coordinator's `OnBranchCommitted(kind, branchId)` event for Military/Rebel/Independent, and
  PRPF's `OnJoined` event (`PrpfStandingSystem.OnJoined`, verified §2b). Creation is *reactive*: IH never
  calls `CommitBranch` itself.
- **End trigger:** `OnEndingResolved`, resignation, or death of the last surviving member the House recognises (edge case,
  P0 decides).

### 9.3 Merit (derived)

```text
Merit = Σ_{met house commitments}  merit_met(orderId)
      − Σ_{missed house commitments} merit_missed(orderId)
      + clamp( standing(houseId) / 10, −10, +10 )
      + Commission.MeritAdjust
```

- Order ids are `house:<houseId>:<orderId>:<seq>`; `CommitmentSystem.MetIds` / `MissedIds` are filtered by that prefix and
  looked up in `house_orders.json` for the merit values — **no merit total is stored anywhere**.
- `standing` is `FactionStandingSummary.Standing` (E11); the ±10 clamp keeps standing from *buying* a rank.
- `MeritAdjust` is the sum of Loyalty Crisis outcomes and declined promotions (each a *human act*).

### 9.4 Rank ladder (data; thresholds are the shipped defaults)

| Rung | Merit ≥ | Min days at rung before offer | Order interval (days) | Open orders max | Dues from |
|---:|---:|---:|---:|---:|---|
| 1 | 0 | — | 10 | 1 | — |
| 2 | 20 | 14 | 9 | 1 | rung 2 |
| 3 | 60 | 28 | 8 | 2 | |
| 4 | 120 | 45 | 7 | 2 | |
| 5 | 220 | 70 | 6 | 2 | |

*No rank without a debt (§1b): every rung shortens the order interval by a day, and from rung 2 a weekly due begins.*

| House | R1 | R2 | R3 | R4 | R5 |
|---|---|---|---|---|---|
| Military | Enlistee | Corporal | Warden-Sergeant | Captain of Ten | Marshal of the Shelter |
| Rebel cells | Signatory | Cell Hand | Cell Lead | Circle Voice | Table Speaker |
| Independents | Guest | Fellow | Steward | Elder | Keeper |
| The Quiet | Listener | Recorder | Steward-in-Quiet | Warden | Hand |

*The Quiet's rung names are **hidden until reached**.* Names are content candidates; the fictional-only rule applies (CLAUDE.md
tone).

### 9.5 Orders (Commitments issued by a House)

- **Cadence & cap:** interval by held rung (§9.4), ±seeded jitter (`stream(day, houseHash, "order").NextInt(-1, 3)`); at most the rung's
  *open orders max*; issued only when the previous order is settled or the interval elapsed, whichever is later.
- **Selection:** weighted seeded draw among rows where `house == HouseId`, `min_rung ≤ HeldRung`, `branch_filter` is empty or contains the
  current branch, `requires_flags` are satisfied, the row is off cooldown (21 days), and the row's *kind* has a live adapter (a row
  whose adapter is absent is excluded, never faked — e.g., `escort_convoy` without CW).
- **Registration:** the issuer builds a `CommitmentDefinition` (E13) and calls `CommitmentSystem.RegisterCommitment`:
  `type = house_order`, `counterparty = HouseId`, `start_day = today`, `due_day = today + due_days × crisis_deadline_mult (read site)`,
  `warning_lead_days` from the row, `target_id`, `target_quantity`, `condition_type` from the row, `consequence_class = standing_penalty`,
  `consequence_target = HouseId`, `consequence_magnitude` from the row (negative).
- **Progress adapters (one per kind, all through `RecordProgress`; none writes to another owner):**

| Kind | Progress source (existing owner) |
|---|---|
| `deliver_goods` | inventory delivery event / a **Deliver** command on the House strip that removes the items through the inventory owner and calls `RecordProgress` |
| `hold_watch` | nights of watch counted from the day tick by reading `DutyRosterSystem` (no per-night event exists — §2b, DEC-IH-11) |
| `dispatch_expedition` | `OnExpeditionStarted`/completion for the named location |
| `escort_convoy` / `raid_convoy` | CW's `OnOperationResolved(kind, caravanId, rung)` (adapter dark until CW exists) |
| `harbour` | days a named visitor stays housed (`VisitorIntegrationSystem.OnHousingAssigned` … `OnVisitorIntegrated` — verified §2b) |
| `silence` | *days elapsed without a breach* — a breach is read from the owner (trade stance/embargo, radio, expedition); progress accrues only on clean days |

- **Consequences:** `OnConsequenceRouted` (E13) already routes a missed commitment's `consequence_magnitude`; IH's adapter turns it
  into `FactionBranchCoordinator.ModifyStanding(houseId, magnitude)` if the Commitment host has not already (P0 checks — no
  double-charge).

### 9.6 Duties, Dues, Perks

- **Dues (from rung 2):** a weekly `house_due` Commitment (same path, `merit_missed = −5`, `Warmth −1` on the first miss). The Quiet asks none;
  its silence is its due.
- **Perks (closed set of effect kinds; §10.3):** `weekly_supply` (items via `IFactionActionItemSink`), `trust_floor` (a floor on the
  House's trust through the stance owner's public surface), `access_flag` (a flag through the existing flag ledger),
  `bounty_clear_on_promotion` (`FactionBountySystem.ClearBountiesForFaction`). Perks are **derived from held rung** each day; nothing
  is granted twice (idempotent by `(perkId, week)`).

### 9.7 Loyalty Crises

- **Trigger predicates (closed set):** `rung_reached(n)`, `days_since_commission(n)`, `order_missed_streak(n)`, `order_conflict`
  (open orders from two different Houses, or an order that contradicts an open Ration/Treaty commitment), `escorted_before(factionId)`
  (reads CW's `RoadMemory`; dark until CW), `standing_below(factionId, n)`, `promotion_offered`.
- **Fire rules:** at most one crisis per 21 days; at most one *pending*; tie-break on crisis id; a crisis never fires during a siege
  day or a plague quarantine (soft-read; ignored if those plans are absent).
- **Delivery:** through the existing event pipeline (DEC-IH-06). P0 decides whether rows live in `house_crises.json` in the *same schema
  as `events.json` choices* (`choiceId`, `text`, `moraleDelta`, `effects[]`) or whether the event loader can consume the new file.
- **Effects (closed set of kinds, each through its owner):** `standing_delta(faction, n)`, `alignment_shift(faction, n)`, `merit_adjust(n)`,
  `warmth_delta(n)`, `set_flag(id)`, `relationship_delta(survivorA, survivorB, n)` (existing social API), `resign`, `demote`, `promote`.

### 9.8 Resignation, desertion, and the PONR

- **Before the PONR (E11):** `Resign()` is allowed. It writes: standing `−(10 + 4×HeldRung)`; flag `flag_house_resigned_<house>`; `Warmth → 0`;
  a `PastCommission` row; for Military, the *Deserter* branches (`branch_mil_7_deserter`, `branch_mil_14_fugitive_deserter`) become
  **eligible** through the coordinator's own `CanCommit` (IH does not commit them).
- **After the PONR:** refused with the authored line *"You may not. We would like to say we are sorry."* (`house_lines.json`).
- **Ending resolved:** the commission moves to `Past` with `EndReason = ending id`.

### 9.9 Worked examples

**Example 1 — merit and the first promotion (Military).** Commissioned day 90, standing +40 (⇒ +4). Orders (rung 1, interval 10):
day 90 `o_mil_ration_levy` (20 `dried_rations`, due 10, merit +8/−6), day 100 `o_mil_ammo_muster` (30 `ammo_308`, due 12, +10/−8).
Ration levy delivered day 97 ⇒ Merit 8+4 = **12**. Ammo delivered day 108 ⇒ Merit 8+10+4 = **22 ≥ 20**; days at rung 18 ≥ 14; no open crisis ⇒
**Promotion offered day 108** (Pending). Accept ⇒ `HeldRung = 2` (Corporal), `RungSinceDay = 108`; order interval becomes 9, the
weekly due (4 `dried_rations`) begins on day 115, and perk `weekly_supply` (3 `canned_food` / 7 days) starts.

**Example 2 — declining.** Same state, player declines. `MeritAdjust −20` ⇒ Merit **2**; `Warmth 2 → 1` (Cool). The player stays Enlistee; the
next offer needs Merit ≥ 20 *again* — i.e., ~2–3 further orders. The decline is remembered (`Promotions[…].Decision = Declined`) and one
sponsor line changes tone. **Nothing else** changes; no penalty is hidden.

**Example 3 — a crisis with a price.** Rung 2 Military, day 118. Crisis `crisis_mil_the_name_on_the_sheet` fires (§10.2). Option B ("strike the
name and say why") writes `merit_adjust −5`, `warmth_delta −1`, and a `relationship_delta` the survivor's friends will feel. It is
neither the "good" nor the "bad" option: it is the one where the player *said it out loud*.

---

## 10. Inside a House — authored content

### 10.1 The 24 orders (`house_orders.json`; item ids are existing ids seen in shipped data — re-verify at P0)

**Legend** — *Kind*: deliver / watch / dispatch / escort / raid / harbour / silence · *Due*: days · *M±*: merit met / missed.

| Id | House | R≥ | Kind | Target · Qty | Due | M± | Line (content candidate) |
|---|---|---:|---|---|---:|---|---|
| `o_mil_ration_levy` | Military | 1 | deliver | `dried_rations` · 20 | 10 | +8/−6 | *"Rations for the mess. The mess does not ask twice. It counts."* |
| `o_mil_ammo_muster` | Military | 1 | deliver | `ammo_308` · 30 | 12 | +10/−8 | *"Muster the rounds. Do not muster the speeches."* |
| `o_mil_hold_the_wall` | Military | 2 | watch | 3 nights | 9 | +12/−10 | *"Three nights on the wall, by the House's roster and not yours."* |
| `o_mil_patrol_road` | Military | 2 | dispatch | `loc_cut_abandoned_depot` · 1 | 12 | +12/−10 | *"A patrol to the depot. Nobody has asked what the Cutters call it."* |
| `o_mil_escort_supply` | Military | 3 | escort | any caravan · 1 | 14 | +20/−15 | *"Escort a supply column. The House would like the column to arrive."* (needs CW) |
| `o_mil_levy_water` | Military | 3 | deliver | `clean_water` · 40 | 10 | +15/−12 | *"Water is not a gift. It is a levy, and you have been a good subject."* |
| `o_mil_hand_over_radio` | Military | 4 | deliver | `military_radio` · 1 | 8 | +18/−14 | *"Your radio is the House's radio now. It always was; today we say it aloud."* |
| `o_reb_scrap_for_the_cell` | Rebel | 1 | deliver | `scrap_metal` · 25 | 10 | +8/−6 | *"The cell needs metal and does not need to say why."* |
| `o_reb_medical_cache` | Rebel | 1 | deliver | `medical_kit` · 4 | 12 | +10/−8 | *"The cell keeps a clinic. It is the only reason the cell is loved."* |
| `o_reb_night_signal` | Rebel | 2 | watch | 2 nights | 8 | +10/−8 | *"Lamp in the window, two nights. Someone is counting the lamps."* |
| `o_reb_harbour_courier` | Rebel | 2 | harbour | a named courier · 5 days | 7 | +12/−10 | *"A courier will arrive tired. Do not ask what she carries."* |
| `o_reb_cut_the_cart` | Rebel | 3 | raid | any caravan · 1 | 14 | +22/−16 | *"The Circle has voted on a cart. You have been asked to be its hands."* (needs CW) |
| `o_reb_silence_on_fleet` | Rebel | 3 | silence | no trade with `faction_the_fleet` · 7 days | 7 | +14/−12 | *"For a week we do not buy the Fleet's salt. The Circle will explain later."* |
| `o_reb_circle_levy` | Rebel | 4 | deliver | `diesel_fuel` · 30 | 10 | +16/−14 | *"Fuel for the Circle. It will not be spent on anything you will be shown."* |
| `o_ind_open_table` | Indep. | 1 | deliver | `dried_rations` · 15 | 10 | +8/−5 | *"The long table feeds whoever comes. It asks you to bring your share."* |
| `o_ind_share_a_filter` | Indep. | 1 | deliver | `water_filter` · 1 | 12 | +9/−6 | *"A filter for the long table. Somebody's children drink from it."* |
| `o_ind_tally_night` | Indep. | 2 | watch | 2 nights | 8 | +10/−8 | *"Watch the tally. The tally has been wrong before."* |
| `o_ind_host_a_guest` | Indep. | 2 | harbour | a named guest · 7 days | 7 | +12/−9 | *"A guest for a week. The House does not say why; it says please."* |
| `o_ind_escort_neutral` | Indep. | 3 | escort | any caravan · 1 | 14 | +18/−12 | *"Escort a neutral column. The word neutral does a great deal of work."* (needs CW) |
| `o_ind_mediate` | Indep. | 4 | dispatch | `loc_cut_merchant_caravanserai` · 1 | 10 | +16/−12 | *"Go to the caravanserai and be the person nobody is afraid of."* |
| `o_prp_quiet_days` | Quiet | 1 | silence | no outgoing radio · 3 days | 3 | +10/−10 | *"Three days of quiet. It matters. It will not be explained."* |
| `o_prp_a_list` | Quiet | 2 | deliver | `electronic_scrap` · 10 | 8 | +10/−8 | *"Parts for something. Do not ask what."* |
| `o_prp_watch_a_door` | Quiet | 3 | watch | 4 nights | 8 | +14/−12 | *"Watch a door. Not yours. Not ours to name."* |
| `o_prp_do_nothing` | Quiet | 4 | silence | no expeditions · 5 days | 5 | +18/−16 | *"For five days, do nothing at all. It is the hardest order we give."* |

### 10.2 Loyalty Crises (17 rows; 3 options each — effects in the closed set of §9.7)

*Each option's summary lists the effects it writes. **No option is labelled good or bad.** All effects route through the named owners.*

**Military**

- **`crisis_mil_the_name_on_the_sheet`** *(rung 2)* — The House levy sheet asks for names "fit for the line". One name is a survivor with a bad lung, whose friends will notice the omission *and* the inclusion.
  A) *Submit the sheet unedited* — `merit_adjust +10`, `standing_delta(military, +2)`, `relationship_delta(friends, −6)`.
  B) *Strike the name and say why, in writing* — `merit_adjust −5`, `warmth_delta −1`, `relationship_delta(friends, +6)`.
  C) *Offer yourself in their place* — `merit_adjust +15`, `set_flag(flag_self_levy)`, a health/stamina cost the existing needs owner applies.
- **`crisis_mil_the_hand_over`** *(rung 3)* — A deserter has taken shelter in your cellar. The Warden-Sergeant asks for him back and does not raise his voice.
  A) *Surrender him* — `merit_adjust +20`, `standing_delta(military, +3)`, `set_flag(flag_handed_deserter)`; the moral-choice owner records the act.
  B) *Hide him* — `merit_adjust −25`, `standing_delta(military, −10)`, `set_flag(flag_harbouring_deserter)`.
  C) *Send him away with rations* — `merit_adjust −10`, `standing_delta(military, −3)`, `relationship_delta(dissenters, +4)`.
- **`crisis_mil_the_correct_order`** *(rung 4)* — An order that is legal and wrong: withhold water from a settlement that has not paid the levy.
  A) *Obey* — `merit_adjust +15`, `standing_delta(military, +5)`, a flag the Living Region reads as "House-enforced shortage".
  B) *Obey slowly* — the Commitment due day is honoured by a day; `merit_adjust +5`, `warmth_delta −1`.
  C) *Refuse in writing* — `merit_adjust −20`, `standing_delta(military, −8)`, `warmth_delta −1`; the refusal is a chronicle line.
- **`crisis_mil_the_oath`** *(promotion to rung 5)* — The Marshal's chair asks for a public oath that names the shelter as a *dependent post*.
  A) *Swear* — promote; `set_flag(flag_oath_sworn)`; the Independents' and Rebels' trust move by a data amount.
  B) *Swear with reservation, aloud* — promote; `warmth_delta −1`; the reservation is a chronicle line.
  C) *Decline and resign* — `resign` (only if ¬PONR; else option unavailable).

**Rebel cells**

- **`crisis_reb_the_vote`** *(rung 2)* — The cell votes to sabotage a supply cart that carries medicine for a ward as well as rifles.
  A) *Vote yes* — `merit_adjust +12`, `standing_delta(rebel, +4)`, `standing_delta(rebuilders, −6)`.
  B) *Vote no, stay* — `merit_adjust −10`, `warmth_delta −1`.
  C) *Leave the cellar* — `resign` if ¬PONR.
- **`crisis_reb_the_informer`** *(rung 3)* — Someone in the cell is leaking. The Cell Lead asks you to look at your own survivors.
  A) *Investigate* — `merit_adjust +10`; a hook for The Quiet War (dark if absent); `relationship_delta` costs to each suspect.
  B) *Refuse* — `merit_adjust −12`, `warmth_delta −1`.
  C) *Warn the suspected* — `merit_adjust −20`, `relationship_delta(suspect, +8)`; if found out the sponsor's warmth falls to 0.
- **`crisis_reb_names_on_a_board`** *(rung 4)* — The Circle wants your survivors' names for a board of martyrs. It reads like a list of targets.
  A) *Give the names* — `merit_adjust +18`, `standing_delta(rebel, +4)`; the moral owner records it.
  B) *Give one name, your own* — `merit_adjust +12`, `set_flag(flag_named_myself)`.
  C) *Give none* — `merit_adjust −22`, `warmth_delta −1`.
- **`crisis_reb_the_speakers_chair`** *(promotion to rung 5)* — The Table Speaker's chair comes with a "revolution tax" on your stores.
  A) *Pay the tax* — promote; a stores draw through the inventory owner (data amount).
  B) *Negotiate the tax down* — promote; `warmth_delta −1`.
  C) *Refuse the chair* — `merit_adjust −25`; stay at rung 4.

**Independents**

- **`crisis_ind_the_open_door`** *(rung 2)* — A Fellow asks you to take in a family. The House says decide together. You are full.
  A) *Admit them* — `relationship_delta` gains; a survivors-conserved intake via the visitor owner; a stores cost.
  B) *Refuse* — `warmth_delta −1`, `merit_adjust −5`.
  C) *Put it to the shelter's vote* — `merit_adjust +6`; if Shelter Governance exists, its Assembly decides (dark otherwise → falls back to A/B by majority of survivors).
- **`crisis_ind_the_tally`** *(rung 3)* — A Steward has padded the tally to feed her own children.
  A) *Expose her* — `merit_adjust +12`, `relationship_delta(steward, −10)`.
  B) *Forgive, with restitution* — `merit_adjust +8`, `warmth_delta +1`.
  C) *Cover for her* — `merit_adjust −15`; `set_flag(flag_covered_tally)`; if it surfaces later, warmth 0.
- **`crisis_ind_the_elders_verdict`** *(rung 4)* — The Elders ask you to judge a member whom you like, for a small crime.
  A) *Judge fairly* — `merit_adjust +14`; `relationship_delta(member, −4)`.
  B) *Recuse* — `merit_adjust +2`, `warmth_delta −1`.
  C) *Argue for mercy* — `merit_adjust +6`, `standing_delta(independent, +2)`.
- **`crisis_ind_the_keepers_paper`** *(promotion to rung 5)* — A Keeper keeps the house's records, including a founding paper that says less than the wall does.
  A) *Keep it quiet* — promote; `set_flag(flag_kept_founding_paper)`.
  B) *Read it aloud at the long table* — promote; `warmth_delta −1`; `standing_delta(independent, +3)`.
  C) *Burn it* — promote; a `Record Keepers` hook (dark if absent) records a Gap.

**The Quiet**

- **`crisis_prp_the_listeners_question`** *(rung 1→2)* — You are asked to describe a visitor's habits, calmly.
  A) *Answer* — `merit_adjust +8`, `warmth_delta +1`. B) *Decline* — `merit_adjust −6`. C) *Lie* — `merit_adjust +2`; a QW hook (dark if absent).
- **`crisis_prp_the_quiet_order`** *(rung 2)* — Do nothing for three days. It matters, and will never be explained.
  A) *Comply* — `merit_adjust +10`. B) *Break the silence to ask why* — `merit_adjust −8`, `warmth_delta −1`. C) *Comply and write it down* — `merit_adjust +4`, `set_flag(flag_wrote_it_down)`.
- **`crisis_prp_the_recorded`** *(rung 3)* — A recording of you exists.
  A) *Ask to hear it* — `merit_adjust −4`, `warmth_delta +1`. B) *Ask to burn it* — `merit_adjust −10`. C) *Ask nothing* — `merit_adjust +6`.
- **`crisis_prp_the_hand`** *(rung 4)* — The Hand asks you to renounce a stated virtue *aloud*, in the Quiet Room.
  A) *Renounce it* — promote pending; `alignment_shift(prpf, +?)` (data). B) *Renounce a lesser one* — `warmth_delta −1`. C) *Decline* — `merit_adjust −20`.

**Seam crisis (dark until CW exists)**

- **`crisis_x_the_order_to_raid_a_friend`** *(rung ≥ 3, any House)* — An order asks you to raid a caravan whose captain you have escorted twice.
  A) *Obey* — `merit_adjust +20`; CW's `RoadMemory.Raids` moves through CW's own path; trust with the caravan's faction falls through the stance owner.
  B) *Refuse* — `merit_adjust −20`, `warmth_delta −1`.
  C) *Warn the captain* — `merit_adjust −30`, `standing_delta(caravanFaction, +6)`; **the sponsor's warmth falls to 0 if the warning is discovered** (a seeded discovery roll, data).

### 10.3 Perks (12 rows; closed effect kinds)

| House | R2 | R3 | R4 |
|---|---|---|---|
| Military | `weekly_supply` 3 `canned_food` | `bounty_clear_on_promotion` | `trust_floor` +20 |
| Rebel | `access_flag` `flag_reb_safe_house` | `weekly_supply` 1 `medical_kit` | `trust_floor` +20 |
| Independents | `access_flag` `flag_ind_open_road` | `weekly_supply` 4 `dried_rations` | `trust_floor` +20 |
| The Quiet | `access_flag` `flag_prpf_quiet_pass` | `weekly_supply` 2 `iodine_pills` | `trust_floor` +20 |

(*Rung 5 has no perk on purpose: the top of a House is a **duty**, not a reward — §19.*)

---

## 10b. A Commission in Eleven Entries (prose texture — not authority)

*A Military Enlistee's book across the first promotion and one crisis. Numbers derive from §9.9 (commissioned day 90, standing +40).*

**Day 90 — The Mess.** They call it the Mess because nobody could agree on a better word. Long tables, one stove, a Sergeant-Major
called Aldrey who says our names as though he were reading them off a manifest. Enlistee. It is a word that means someone else is
counting you. I have decided that is comforting.

**Day 90 — The first order.** Twenty *dried rations*, due in ten days. *"The mess does not ask twice. It counts."* Aldrey said it
without smiling. Nobody smiled. It is the friendliest thing I have heard all week.

**Day 97 — Rations delivered.** Seven days early. Merit **12** (8 for the levy, 4 for standing). Aldrey wrote it on the board in a hand
that suggested he wrote everything on the board in that hand.

**Day 100 — The second order.** Thirty rounds of .308, due in twelve days. *"Muster the rounds. Do not muster the speeches."* The
Quartermaster looked at the sum and at me. I said nothing. It seemed the right thing.

**Day 108 — Rounds delivered.** Merit **22**. Eighteen days at rung. No open crisis. The board says *Promotion offered.* I have read the
word *offered* eleven times; it has a different weight than *awarded*.

**Day 108 — I accept.** Corporal. The stripe is a piece of cloth that used to be a shirt. Aldrey says the House does not give
rewards, it gives *more to do*. He also says I will be sorry, and that everyone is, and that it is still worth it. I asked which of
the three was the true one. He said *"All of them, Corporal. That is what makes it a rank."*

**Day 115 — The weekly due.** Four *dried rations*, every seven days. It is a small thing. I have noticed I think about it on the
sixth day.

**Day 118 — The sheet.** A levy list. *"Names fit for the line."* One of them has a lung like wet paper and a friend who will
know whether I wrote it down. I struck the name. I said why, in writing, in a hand that did not look like mine. Merit −5. Aldrey read it
and set it aside without comment. That was the comment.

**Day 119 — Cool.** He is still polite. He is *cool* polite, which is a different temperature. The board lists my merit at 17. I do not
know whether I would have preferred to be liked.

**Day 133 — The friend's friend.** The survivor with the lung is still on the wall at night. The friends do not say thank you. They
say *nothing*, and it is the loudest kind.

**Day 150 — The count.** Rung 2. Merit 41. Warmth: *Cool*. Four orders met (the two rung-one levies, the wall, the depot patrol), one struck name, one perk (three tins of food a week that
I try not to think of as pay). The House has not asked me for anything I did not know it would ask for. That is the whole trick.


---

# PART THREE — CATALOGS, SEAM AND DELIVERY

## 11. Data catalog specifications (both parts)

All files live in `Assets/StreamingAssets/Data/`, snake_case, integer `schema_version: 1`, validated by
`CatalogIntegrityValidator` (`INT`), registered with `ContentUtilizationScanner`. **A row's presence is not proof of
reachability**: each catalog names its consumer in §14.

### 11.1 Convoy Wars

**`convoy_tuning.json`** — every number in §6 that is *not* an existing owner's number.

```json
{
  "schema_version": 1,
  "guard": { "base": 100, "defense_multiplier": 5, "mitigation_divisor": 5, "integrity_divisor": 20,
             "crew_base_permille": 500, "crew_step_permille": 250, "crew_extra_cap": 2,
             "column_discount_permille": 800, "guard_for_full_escort": 900, "guard_unit_per_caravan_guard": 90 },
  "stances": [
    { "id": "close",    "exposure_permille": 900, "guard_adjust":  40, "fuel_permille": 1000, "extra_days": 0 },
    { "id": "screened", "exposure_permille": 700, "guard_adjust":   0, "fuel_permille": 1100, "extra_days": 0, "scout_vehicle": true },
    { "id": "fast",     "exposure_permille": 550, "guard_adjust": -60, "fuel_permille": 1200, "extra_days": 0 },
    { "id": "hidden",   "exposure_permille": 350, "guard_adjust": -30, "fuel_permille":  950, "extra_days": 1, "noise_limit": 40 }
  ],
  "rungs": [
    { "id": "rout",       "margin_min":  300, "garage": [],                                     "cargo_loss_permille":   0, "injuries": [] },
    { "id": "beaten_off", "margin_min":  100, "garage": [{"count":1,"integrity_delta":-30}],    "cargo_loss_permille":   0, "injuries": [] },
    { "id": "costly",     "margin_min":  -99, "garage": [{"count":2,"integrity_delta":-80,"stress_delta":40}], "cargo_loss_permille": 100, "injuries": ["minor"] },
    { "id": "mauled",     "margin_min": -300, "garage": [{"count":2,"integrity_delta":-150},{"immobilize":1}], "cargo_loss_permille": 300, "injuries": ["serious"] },
    { "id": "overrun",    "margin_min": -9999,"garage": [{"immobilize":2}],                     "cargo_loss_permille": 600, "injuries": ["per_crew"], "robbed": true }
  ],
  "variance": { "min": -60, "max": 60 },
  "surprise_hidden_raid": 80,
  "noise_points": { "vehicle_armored_mobile_base": 30, "vehicle_steam_halftrack": 20, "vehicle_cargo_truck": 10,
                    "default": 5, "module_trailer": 10, "module_mounted_gun": 5 },
  "reprisal": { "wary_strength": 45, "hostile_strength": 90, "hostile_skip_days": 10 },
  "raid": { "min_guard": 150, "toll_permille": 150, "loot_permille": {"yield":600,"partial":400,"stalemate":150} }
}
```

Validator rules (one failing test each): stances unique; `margin_min` strictly decreasing by rung; `hidden` is the only stance with
`noise_limit`; every `noise_points` key ∈ `vehicles.json` ∪ `vehicle_modules.json` (`module_` prefix stripped); `guard_for_full_escort > 0`;
`column_discount_permille` ≤ 1000; each rung's `garage` entries name only `integrity_delta`, `stress_delta`, `immobilize`; `robbed` only on
the final rung; no stance dominates (strict-dominance check over the six threat rows — §16).

**`convoy_threats.json`**

```json
{ "schema_version": 1, "threats": [
  { "location_id": "loc_cut_abandoned_depot", "kind": "Ambush", "threat_permille": 450, "strength": 320,
    "fortification": 0, "band_label_key": "convoy_band_cutters", "arrival_line_key": "convoy_arrive_depot" },
  { "location_id": "loc_cut_merchant_caravanserai", "kind": "Checkpoint", "threat_permille": 60, "strength": 60,
    "toll_permille": 20, "refuel": true, "arrival_line_key": "convoy_arrive_caravanserai" }
] }
```

Validator: `location_id` ∈ locations catalog **and** appears in ≥ 1 caravan route (E5); `kind` ∈ {`Ambush`,`Checkpoint`,`Breakdown`}; `strength`
required for Ambush/Checkpoint; `toll_permille` required for Checkpoint; every route node of every caravan has a threat row (E5 ⇒ six rows).

**`convoy_routes.json`** — `{ "from_node", "to_node", "hop_km" }` rows (§7.1). Validator: every consecutive pair in each caravan's
`route_node_ids` exists; `hop_km` 1..200; `refuel_nodes` ⊆ threat rows.

**`convoy_road_war.json`** — trust/embargo/bounty ladders and per-caravan fee rows:

```json
{ "schema_version": 1,
  "escort": { "trust_first": 6, "trust_step": 2, "trust_cap": 20, "road_pass_days": 30, "bounty_clear_after_escorts": 2 },
  "raid":   { "trust_base": -12, "trust_step": -4, "trust_cap": -40, "embargo_base_days": 10, "embargo_step_days": 5,
              "embargo_cap_days": 40, "patrol_bounty_min_rung": "partial" },
  "fees":   [ { "caravan_id": "caravan_flotilla_salt_run", "fee_permille": 80 },
              { "caravan_id": "caravan_verge_grain_convoy", "fee_permille": 80 },
              { "caravan_id": "caravan_foundry_coal_iron", "fee_permille": 80 },
              { "caravan_id": "caravan_free_trader_circuit", "fee_permille": 80 } ] }
```

### 11.2 Inside a House

**`house_ranks.json`**

```json
{ "schema_version": 1, "houses": [
  { "house_id": "faction_military", "room_key": "house_room_mess", "sponsor_id": "npc_house_mil_aldrey",
    "rungs": [
      { "rung": 1, "name_key": "rank_mil_1", "merit_min": 0,   "min_days": 0,  "order_interval_days": 10, "open_orders_max": 1, "dues": false },
      { "rung": 2, "name_key": "rank_mil_2", "merit_min": 20,  "min_days": 14, "order_interval_days": 9,  "open_orders_max": 1, "dues": true  },
      { "rung": 3, "name_key": "rank_mil_3", "merit_min": 60,  "min_days": 28, "order_interval_days": 8,  "open_orders_max": 2, "dues": true  },
      { "rung": 4, "name_key": "rank_mil_4", "merit_min": 120, "min_days": 45, "order_interval_days": 7,  "open_orders_max": 2, "dues": true  },
      { "rung": 5, "name_key": "rank_mil_5", "merit_min": 220, "min_days": 70, "order_interval_days": 6,  "open_orders_max": 2, "dues": true  } ],
    "dues": [ { "item_id": "dried_rations", "quantity": 4, "per_days": 7 } ] }
] }
```

Validator: four houses; each has five rungs with strictly increasing `merit_min` and non-decreasing `open_orders_max`; `order_interval_days`
strictly decreasing; every `house_id` resolves through `FactionStandingIdResolver` (E15); every `sponsor_id` exists in the characters catalog **or**
is flagged `authored_pending` (validator warns, does not fail — sponsors are content candidates until written).

**`house_orders.json`** — the 24 rows of §10.1:

```json
{ "order_id": "o_mil_ration_levy", "house_id": "faction_military", "min_rung": 1, "branch_filter": [],
  "kind": "deliver_goods", "target_id": "dried_rations", "quantity": 20, "due_days": 10, "warning_lead_days": 3,
  "merit_met": 8, "merit_missed": -6, "consequence_magnitude": -8, "cooldown_days": 21,
  "requires": [], "line_key": "order_mil_ration_levy" }
```

Validator: `kind` ∈ the seven adapter kinds; `target_id` ∈ items catalog (deliver) or locations (dispatch) or a documented token
(`silence`, `harbour`, `hold_watch`); `merit_missed ≤ 0 ≤ merit_met`; `consequence_magnitude ≤ 0`; `requires` ∈ {`convoy_wars`, `night_watch`, `visitor_integration`}
(gates only; a row with an absent requirement is excluded, never faked); a row's `quantity`/`due_days` must be achievable (item stack limits from the
items catalog; a due < warning lead is an error).

**`house_crises.json`** — the 17 rows of §10.2:

```json
{ "crisis_id": "crisis_mil_the_hand_over", "house_id": "faction_military", "trigger": { "kind": "rung_reached", "n": 3 },
  "requires": [], "situation_key": "crisis_mil_hand_over_situation",
  "options": [
    { "option_id": "a", "text_key": "crisis_mil_hand_over_a",
      "effects": [ {"kind":"merit_adjust","n":20}, {"kind":"standing_delta","faction":"faction_military","n":3}, {"kind":"set_flag","flag":"flag_handed_deserter"} ] },
    { "option_id": "b", "text_key": "crisis_mil_hand_over_b",
      "effects": [ {"kind":"merit_adjust","n":-25}, {"kind":"standing_delta","faction":"faction_military","n":-10}, {"kind":"set_flag","flag":"flag_harbouring_deserter"} ] },
    { "option_id": "c", "text_key": "crisis_mil_hand_over_c",
      "effects": [ {"kind":"merit_adjust","n":-10}, {"kind":"standing_delta","faction":"faction_military","n":-3} ] }
  ] }
```

Validator: exactly three options; effects ⊆ the closed set of §9.7; `standing_delta`/`alignment_shift` target a faction id resolved by the resolver; `resign` only
where ¬PONR can be evaluated; every flag id is unique and unread flags warn; every `*_key` exists in the localization catalog (string-freeze D22 note, §18).

**`house_perks.json`** — 12 rows (§10.3); `effect` ∈ {`weekly_supply`, `trust_floor`, `access_flag`, `bounty_clear_on_promotion`}; a `weekly_supply` item must exist and
the weekly amount must be ≤ the weekly dues *value* at that rung (a perk never out-pays its due — enforced by a value table in the validator).

---

## 12. Cross-plan boundaries and hooks

Each hook **ships dark** (a `Null*` implementation) until both ends exist and is a no-op when the other plan is absent, with exactly one test shape.

| Plan / owner | Boundary | Hook |
|---|---|---|
| **Long Line: Freight (LF)** | LF owns the player's own runs. CW never edits a contract or a run. CW's Convoy Rating supplies LF's `escortStrengthPermille` read-only. | **X3** `IEscortStrengthProvider` |
| **Crews and Companions (CC)** | A CW operation *is* a CC party of kind Convoy when CC exists (DEC-CC-01: N ordinary expeditions + a coordinator). CW must not fork the party model. | party-kind registration (dark) |
| **The Underworld (UW)** | One heat, never two (DEC-UW-09). A raid raises UW's heat only through its single bridge; CW writes the faction-level bounty, UW owns the person-level mark. | none in v1 |
| **The Ration Wars (RW)** | A Table Rule may cite escort fees as *received* goods through the RW ledger's normal intake; CW adds no ledger. | none |
| **The Quiet War (QW)** | A crisis option "investigate" hands a name to QW's public seam; nothing else. | dark seam |
| **Shelter Governance (SG)** | A crisis option "put it to the vote" calls the Assembly if present. | dark seam |
| **The Record Keepers (RK)** | "Burn it" records a Gap through RK's public writer if present. | dark seam |
| **Year Two** | Chapter Profile flags `convoyWarsEnabled` and `houseServiceEnabled` (P1B) gate both parts. Outposts are Year Two's; neither part reads them. | activation gates |
| **Pair 1 (Iron Road / Siege Year)** | A siege *Notice* or a rail *Cut* may raise CW's threat rows for the affected route through **data rows only** (a `convoy_threats.json` override keyed by a flag), never code. A Warning Order does not change an order's due day. | flag-keyed override rows (dark) |
| **Pair 5 (Treaty Table)** | Tolls, embargoes and Right of Way belong there. CW's Checkpoint toll reads a treaty owner's rate if present; else `toll_permille`. | read seam (dark) |
| **Pair 4 (Memory Work)** | A "Names" wall is not a CW/IH concern. | none |
| **The Living Region (LR)** | A House-enforced shortage flag (`crisis_mil_the_correct_order` A) is a flag LR *reads*; LR decides what it means. | flag |
| **The Muster / Iron Raiders** | `faction_action_board` offers remain choices; IH orders are obligations. The two never register under one id (prefix `house:`). | none |

**X1 — Operation resolved.** CW raises `OnOperationResolved(ConvoyKind kind, string caravanId, string rung)`. IH's `escort_convoy`/`raid_convoy`
adapters subscribe and call `CommitmentSystem.RecordProgress(orderId, 1)` for the matching open order. **CW never reads IH state; IH never reads a
convoy.**

**X2 — Road Memory read.** IH's `escorted_before(factionId)` predicate reads `RoadMemory[c].Escorts` for caravans whose faction matches, through a
read-only interface (`IRoadMemoryReader`). `NullRoadMemoryReader` returns 0.

**X3 — Escort strength.** `IEscortStrengthProvider.Read(vehicleIds) → 0..1000` implemented by `ConvoyRating`; LF supplies `escortStrengthPermille` from
it *if* the provider is bound and a Chartered run names those vehicles. LF's tests are unchanged when unbound.

## 13. Acceptance criteria

**Convoy Wars (CW)**

1. **Authority agreement (CLAUDE.md "integrated").** Core authority + host owner + event path + persistence + observable outcome agree for Escort and Raid.
2. **Parity.** With no `ConvoyWarsState` and the writer never called, caravan, garage, expedition and trade-risk behaviour is identical on a saved corpus (E1–E8).
3. **Derived rating.** `ConvoyRating.Compute` equals a hand-computed value for Examples 1–4 (§6.9) and writes nothing.
4. **Determinism.** Same seed, same op, same hop ⇒ same encounter, same variance, same rung.
5. **No strict dominance.** Over the six threat rows and the shipped four stances, no stance is strictly better on *(hit chance, Guard adjust, fuel, days)* for every row — a data test with per-row output.
6. **One writer.** `isRobbed` is set only through `TryMarkRobbed`; the writer refuses a caravan already robbed, at Holdfast, or unknown.
7. **Effects through owners.** Damage via the garage owner (or the single additive method), trust via the stance owner, embargo via the ledger, loot via `TryGrantLoot`; **no** CW-held copy of any of these values.
8. **Reprisal derivation.** `Unwatched/Watchful/Wary/Hostile` is a pure function of `RoadMemory.Raids`; nothing stores a level.
9. **Abandon paths.** Missed caravan, missing vehicle at restore, and unknown caravan each → `Abandoned` without a crash and without a consequence.
10. **Save round-trip** mid-operation (one Escort at hop 1 with a chosen stance list; one Raid awaiting arrival); older saves load with no operation.

**Inside a House (IH)**

11. **Parity.** With no commission, the branch coordinator, commitments, standing and events behave identically on a saved corpus; IH registers **no** day-owner work.
12. **Merit is derived.** `Merit` equals the §9.3 formula on Examples 1–2, is not stored, and changes only by owner facts + `MeritAdjust`.
13. **Orders are Commitments.** Every order is registered through `RegisterCommitment`, ids prefixed `house:`; no parallel obligation store; restore never re-registers.
14. **No double charge.** A missed order's standing penalty is applied exactly once (Commitment owner *or* IH adapter, never both).
15. **Crisis closed set.** Every crisis effect kind is in the closed set; each effect calls exactly one owner.
16. **PONR respected.** `Resign()` before PONR succeeds with the §9.8 writes; after PONR it is refused; the coordinator's own state is unchanged either way.
17. **Rank claims only after the coordinator does.** No commission exists before an existing commit/join verb fired; none survives `OnEndingResolved`.
18. **Save round-trip** mid-service with a pending promotion, an open order and one crisis in history; older saves load with no commission.

**Seam (X)**

19. X1/X2/X3 are no-ops when the other end is absent and do exactly one specified thing when both exist; no hook reads the other plan's state.

## 14. Packages

Each package is a **derived plan** when executed (`.ai/plans/<pkg>-<date>.md`, from `template.md`, approved by the user). Roles per CLAUDE.md.

### CW packages

| Pkg | Role | Scope | Accept |
|---|---|---|---|
| **CW-P0** | Auditor | *(Pass 1, §2b, closed E7, E21, the panels, the event and wound seams and CLI naming.)* Remaining: E6 re-verify at the moment of coding (no `isRobbed` writer); the expedition definition rows for Design A (`exp_convoy_*`) or choose Design B; Commitment consequence routing (E13). | each remaining item answered with `path:line` or a test |
| **CW-P1** | Coder | Catalogs (§11.1), loader, validator rules, scanner registration. | validator green; each rule has a failing test |
| **CW-P2** | Coder | `ConvoyRating` (pure read model), `ConvoyFormationTable` (pure). | acceptance 3, 4, 5 |
| **CW-P3** | Integrator | `TravelingCaravanSystem.TryMarkRobbed` (+ `VehicleGarageSystem.ApplyConvoyDamage` if needed). | acceptance 2, 6 |
| **CW-P4** | Coder | `ConvoyWarsSystem` state, Escort operation, abandon paths, save DTO nesting. | acceptance 1 (escort), 9, 10 |
| **CW-P5** | Coder | Raid operation, Road War consequences, reprisal derivation. | acceptance 6, 7, 8 |
| **CW-P6** | Coder | Optional tactical bind mapping; Breakdown/Checkpoint kinds. | mapping table test |
| **CW-P7** | Integrator | Host: Design A/B wiring, day-owner, event `OnOperationResolved`. | parity; day-owner replay |
| **CW-P8** | Coder | Convoy strip on the garage and trade/caravan panels; presenter tests. | panels hold no authority |
| **CW-P9** | Story Director | Season of the Road content waves (§7.4) + governance close. | handoff |

### IH packages

| Pkg | Role | Scope | Accept |
|---|---|---|---|
| **IH-P0** | Auditor | *(Pass 1 closed the PRPF event, per-night watch, visitor-stay and event-bus items — §2b.)* Remaining: branch-committed events for Independent; `WeightOfChoicesSave` nesting; Commitment consequence routing (E13 — who applies the standing penalty?); resolver output for the four houses; whether any rank-like field exists. | each VERIFY answered; DEC-IH-02/03/06 confirmed |
| **IH-P1** | Coder | Catalogs (§11.2), loaders, validator. | validator green |
| **IH-P2** | Coder | `HouseRankLadder` (pure derivation: merit, eligibility, perks). | acceptance 12 |
| **IH-P3** | Coder | `HouseServiceSystem` state + creation/end triggers + resignation. | acceptance 16, 17, 18 |
| **IH-P4** | Coder | `HouseOrderIssuer`, adapters (one per kind). | acceptance 13, 14 |
| **IH-P5** | Coder | Crises: triggers, effects (closed set), delivery. | acceptance 15 |
| **IH-P6** | Integrator | Host wiring, day-owner, dues/perks idempotence. | acceptance 11 |
| **IH-P7** | Coder | House strip on the faction/branch panel; the **Deliver** command. | presenter tests |
| **IH-P8** | Coder | X1/X2 hooks (dark). | acceptance 19 |
| **IH-P9** | Story Director | Content waves + governance close. | handoff |

**Order.** CW-P0 ∥ IH-P0 → CW-P1..P8 (linear) ∥ IH-P1..P8 (linear) → P9. **Neither part waits on the other.**

## 15. Decision register (proposals — unsigned)

| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-CW-01 | A convoy is an operation over N ordinary expeditions and one caravan; no new vehicle, caravan or economy authority. | architecture | Yes |
| DEC-CW-02 | State nests in the traveling-caravan save DTO; no new section. | architecture | Yes; confirm in P0 |
| DEC-CW-03 | Design A: drivers run ordinary expeditions with the expedition encounter chance zeroed for them (`SetEncounterChanceMultiplier`); Design B: host-side reservation only. | architecture | P0 chooses |
| DEC-CW-04 | One additive writer `TravelingCaravanSystem.TryMarkRobbed(caravanId, lootPermille)`. | architecture | Yes; `INT` |
| DEC-CW-05 | ≤ 2 operations, ≤ 4 vehicles each. | scope | Yes |
| DEC-CW-06 | Garage damage through the existing path; at most one additive `ApplyConvoyDamage` if none exists. | architecture | P0 decides |
| DEC-CW-07 | No raiding at Holdfast; no raid on an already-robbed caravan. | rule | Yes |
| DEC-CW-08 | Tactical bind is optional; the Table always resolves without it. | design | Yes |
| DEC-CW-09 | No new routed panel; extend the garage and trade/caravan panels. | UI | Yes |
| DEC-CW-10 | The Cutters are threat rows only — no faction, leader, doctrine or motive. | tone | Yes |
| DEC-CW-11 | Ask the expedition owner for a one-line vehicle-exclusivity guard (one vehicle, one live expedition); until then the Convoy strip refuses to list a vehicle already out. | rule | Yes |
| DEC-CW-12 | Convoy casualties use `NeedsSystem.ApplyAttributedDelta(id, Health, …, "convoy:<opId>")`; no wound owner is invented. | seam | Yes |
| DEC-IH-01 | Rank is derived; only the held rung, promotion decisions and `MeritAdjust` are stored. | architecture | Yes |
| DEC-IH-02 | State nests in `WeightOfChoicesSave`; no new section. | architecture | Yes; confirm in P0 |
| DEC-IH-03 | Orders are Commitments (`house:` ids); no new obligation type. | architecture | Yes |
| DEC-IH-04 | Houses in v1: Military, Rebel, Independent, the Quiet. Guilds and caravan factions are excluded. | scope | Yes |
| DEC-IH-05 | Use systems-namespace ids through the resolver; no namespace decision is made here. | compatibility | Yes |
| DEC-IH-06 | Crises deliver through the existing event pipeline. | architecture | P0 decides schema |
| DEC-IH-07 | Perks are a closed set; rung 5 has no perk (a duty, not a reward). | design | Yes |
| DEC-IH-08 | No new routed panel; extend the faction/branch panel. | UI | Yes |
| DEC-IH-09 | Resignation only before the PONR; the coordinator's rules are untouched. | rule | Yes |
| DEC-IH-10 | Every House is reasonable from the inside; fictional names and reasons only. | tone | Yes |
| DEC-IH-11 | `hold_watch` is counted from the day tick by reading the duty roster; no per-night event is requested from the owner. | seam | Yes |

## 16. Test plan (focused; **no full suite** without `RUN FULL TESTS`)

Via `bin/run-scoped-tests` on changed modules only; ≤ 10–15 test-edit steps per failure then auto-flag in `.ai/state.md`. Existing tests are extended before
new ones are added: `Expeditions/Plan213VehicleArmorGradeTests.cs`, `VehicleArmorGradesTests.cs`, caravan tests, branch-coordinator tests, commitment tests.

| Group | Cases (3–10 per behaviour) | Trait |
|---|---|---|
| Rating | Examples 1–4 hand-computed; broken vehicle counts 0; zero-integrity loses mitigation; Noise/Hidden gate; purity | fast |
| Table | rung boundaries (table-driven, static rows); variance bounds; determinism; stance × threat matrix; **no strict dominance** (per-row output) | fast |
| Writer | `TryMarkRobbed` refuses unknown/robbed/Holdfast; loot conservation (caravan inventory + hold = original) | fast |
| Operations | Escort happy path; missed caravan; abandon on restore; op cap; vehicle reservation | fast |
| Road War | trust ladder, embargo ladder, reprisal derivation from a count, bounty clear after 2 escorts | fast |
| Save (CW) | round-trip mid-op; legacy neutral | integration |
| Merit/Rank | Examples 1–2; rung boundaries (table-driven); standing clamp; `MeritAdjust` effects | fast |
| Orders | issue cadence by rung; cooldown; requirement-gated exclusion; one adapter test per kind; no double charge | fast |
| Crises | trigger predicates (each kind); fire rules (21-day spacing); closed-set effects; PONR gating | fast |
| Resign | before/after PONR; Deserter branch eligibility through `CanCommit` only | fast |
| Save (IH) | round-trip; restore never re-registers commitments | integration |
| Replay | paired run with an interrupted save mid-operation and mid-order ⇒ identical ledger and Commitment ids | integration |
| Hooks | X1/X2/X3 dark no-op + live one-effect test each | fast |

Host probes (bounded, headless, 15 FPS): extend the existing vehicle-garage and caravan selftests; add `--convoy-wars-selftest` and `--house-service-selftest`
(naming pattern verified: `--vehicle-garage-selftest`, `--caravan-selftest`; §2b).

## 17. Risk register

| # | Risk | Lik. | Impact | Mitigation |
|---|---|---|---|---|
| R1 | The caravan writer (E6) is a shared-seam edit. | med | med | one method; parity test when never called; `INT`; DEC-CW-04 |
| R2 | Design A double-fires encounters (expedition + Table). | med | high | zero the expedition encounter multiplier for convoy drivers; test; fall back to Design B |
| R3 | One vehicle used by two survivors (E7). | med | med | host reservation check; test |
| R4 | Merit farming by cheap deliveries. | med | med | cooldown 21 days; one open order at rungs 1–2; dues from rung 2; no perk at rung 5 |
| R5 | Double charge on missed orders (Commitment + IH adapter). | med | med | P0 names the single applier; acceptance 14 |
| R6 | Faction namespace mismatch (E16). | med | high | resolver only; no namespace decision; validator resolves every id |
| R7 | Crisis delivery schema mismatch with the event pipeline. | med | med | P0 chooses; the closed effect set is the contract |
| R8 | CW/IH co-dependence (escort orders). | low | med | orders excluded when CW absent (never faked); X1 one-way |
| R9 | Tone: raiding and loyalty turn cartoonish. | med | high | DEC-CW-10, DEC-IH-10; tone review on every authored line; no glorified violence; margins not blow-by-blow |
| R10 | Balance: Guard numbers dominate or trivialise. | med | med | all numbers are data; no-dominance test; dominance table doc is the anchor |
| R11 | Armor-grade queue item is stale and gets executed anyway. | low | med | CW-P0 asks the foreman to close or correct it (finding E2) |

## 18. Expansion backlog — *to be expanded and finalised*

**Convoy Wars**

- [ ] **Exposure node kind** (radiation zone alpha as a real hazard: dose to crew, hazmat) via the existing radiation owner.
- [ ] **More caravans and routes**; a *rail convoy* using `car_armored_turret` and the flak-gun car (Pair 1 corridor).
- [ ] **The Cutters as a story** (Season Ch3 choices: pay / break / hire) — data rows for each branch.
- [ ] **Camps on the road** — `EnterCamp` exists for expeditions; a convoy night camp with watch shifts.
- [ ] **Recovery missions as play** (`VehicleRecoveryMission` exists; today it is a timer).
- [ ] **Captured vehicles**, **towing**, **fuel dumps**, **convoy radio** (`field_radio` module + comms).
- [ ] **NPC raiders vs the shelter's own LF runs** (LF ride-along DEC-LF-09 revisited).
- [ ] **Tolls as property** (who owns a checkpoint) — Treaty Table.
- [ ] **Fleet's sealed crates** (`item_logistics_cipher_sheet` is a Salt Run good) — never explained (§19).

**Inside a House**

- [ ] **Guild Houses** (Scavenger Guild culture has 25 entries) and **caravan Houses** (the Fleet, the Scale).
- [ ] **Sponsor scenes** — one authored scene per sponsor per rung.
- [ ] **Coup and succession** — a House that changes its Marshal.
- [ ] **Leaving with followers** — a resignation that takes survivors.
- [ ] **House against House** — joint operations; open orders from two Houses (order-conflict crisis is the seed).
- [ ] **Children in a House** — never soldiers; what a House teaches a child in the Mess.
- [ ] **Generational service** — Year Two Generations: a House remembering a parent.
- [ ] **Rung 6** — a *secret* rank in the Quiet.
- [ ] **String freeze D22** — every `*_key` is a localization key; the freeze blocks new keys until signed.

## 19. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

**Intentionally unanswered.** Any future plan that answers one must name the signed decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| CW-OM-1 | Who are the Cutters? | DEC-CW-10 keeps them a threat row: no leader, doctrine or motive. | Never — texture by omission. |
| CW-OM-2 | Why does the road bend at radiation zone alpha? | The Breakdown row names the bend; no plan explains it. | Ruins of the Before (Pair 7), if signed. |
| CW-OM-3 | What is in the Fleet's sealed crates? | `item_logistics_cipher_sheet` appears among the Salt Run goods; nothing reads it. | The Long Inquest (Pair 6), at its discretion. |
| CW-OM-4 | Who began the Road War? | Raids have consequences, not first causes. | Never. |
| CW-OM-5 | Why does the Verge Convoy carry grain *out* of Holdfast on one leg? | Route data; no reason authored. | Never. |
| CW-OM-6 | What is the arsenal ruin still defending? | An Ambush row with a strength; the defence is unnamed. | Ruins of the Before, at its discretion. |
| IH-OM-1 | Why did the House choose you? | The sponsor's reason is never stated. | Never — a rule. |
| IH-OM-2 | What are the Quiet's orders *for*? | Silence is the Quiet's due (§9.6); purpose is unauthored. | Never. |
| IH-OM-3 | What does the fifth rung owe? | Rung 5 has no perk on purpose (DEC-IH-07). | Never. |
| IH-OM-4 | What does the Independents' founding paper say? | It "says less than the wall does" (§10.2); the paper is never written. | Never. |
| IH-OM-5 | Who counts the lamps? | Rebel order text (`o_reb_night_signal`); nobody is named. | Never. |
| IH-OM-6 | What does Aldrey write on the board? | The prose (§10b) names the hand, not the words. | Never. |

## 20. Pre-flight, verification and stop conditions

**Pre-flight (before *any* package starts):**

- [ ] Read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `AI_AGENT_WORKFLOW.md`, `.ai/state.md` in order.
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`).
- [ ] No equivalent existing system found (search: `Convoy`, `Escort`, `Commission`, `HouseService`, `MeritAdjust`, `TryMarkRobbed`).
- [ ] Premises re-verified (Rule 7): **E2 (armor grades exist), E6 (no `isRobbed` writer), E12 (no rank concept)** each with a one-line test.
- [ ] Signed decisions in hand; no overlapping live claim on `Expeditions/`, `TravelingCaravanSystem.cs`, `Factions/`, `Commitments/`, `Main.VehicleGarage.cs`.
- [ ] Plan status is `APPROVED BY USER` **before any code commit** (Rule 8).

**Verification:**

- [ ] `bin/run-scoped-tests` on new tests and the *existing* garage, armor-grade, caravan, branch-coordinator and commitment tests; each < 30 s.
- [ ] Existing vehicle-garage and caravan selftests unchanged when inactive; new selftests for CW and IH.
- [ ] `--data-integrity-selftest` and port-contract selftest pass; save-registry pin unchanged.
- [ ] `.ai/state.md` updated per package; handoff in `AI_AGENT_WORKFLOW.md` format listing untouched shared paths.

**Stop conditions (Rule 10):** stop and report if **(a)** `TryMarkRobbed` cannot be added without changing caravan movement or trade arithmetic; **(b)** Design A cannot silence
expedition encounters for convoy drivers *and* Design B cannot reserve vehicles; **(c)** the overlay cannot nest without a new save section; **(d)** the
Commitment owner cannot accept runtime-registered definitions; **(e)** a crisis needs an effect outside the closed set; **(f)** the faction resolver cannot map the four
houses; **(g)** any package overlaps a live claim; **(h)** a line of authored text would need a real country, war or person.

**Termination criteria ("done" for each package):** acceptance rows pass via scoped tests; parity rows still pass; the derived plan file is marked
`FULLY INTEGRATED` (multiple times, at the top) **and moved to** `.ai/plans/integrated/<category>/` immediately (CLAUDE.md §8); nothing else.
