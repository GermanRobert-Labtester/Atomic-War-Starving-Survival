# Feature / Task Plan: Movement and War I — The Iron Road (a restoration campaign) & Siege Year (a campaign arc over the Long Siege engine)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — evidence pass 1 complete (2026-09-29, §2b) — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). **Treat as a first full draft to be expanded and finalised.** Every open point is listed in §16 (Expansion backlog) and §17 (Open Mysteries).

> **Subjects covered (2 of the 16 in the "Movement and war / Shelter as a place / Power, paper and place / Land, ruins and starts" batch):**
> 17. **The Iron Road** — restore a rail line across the region. (Prefix `IR`.)
> 18. **Siege Year** — defend the shelter through a build-up, a siege and a breaking. (Prefix `SY`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `docs/expansions/wave3/expansion_25_the_iron_road_plan.md` (Exp. 25, 70 KB design bible: cast, 15 beats, 5+1 endings, 30 side quests) and `.ai/plans/long-siege-2026-09-29.md` (the multi-day siege engine, prefix `LS`). Where this plan and either disagrees on *facts*, source wins (Rule 7); where it disagrees on *story*, those documents win and this plan adapts.
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data, no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **7b**, **9b** and **17** carry story texture and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in code. No authority, path, decision, acceptance criterion or verification step is changed by any prose section.

---

## 0. Prologue — Two Ways a Place Stops Being Only a Place

> *"A rail line is a promise that somebody will be at the other end. A siege is a promise that
> somebody will not let you leave. Both keep a timetable. Only one of them is polite about it."*

There are two kinds of long-distance thought in this game: *going out* and *being kept in*. The Iron
Road is the first taken seriously: an argument, five nodes wide, that the wasteland is not a set of
places you walk between but a **corridor you can keep alive**. Siege Year is the second taken
seriously: an argument that a shelter is not a room but a **season you have to get through**, with a
front porch, a long middle, and an aftermath that has your name on it.

These two subjects are paired here because they are *the same map read in opposite directions*.
Every segment of track the player restores is a segment a besieger can cut. Every day of siege
is a day the line either carries relief or does not. The plan is written so that neither subject is
finished without the other's hook, and neither subject *requires* the other to ship (§10).

**Tone & register.** Two registers, deliberately kept apart:

- **The Iron Road is written in the voice of the work.** Dispatcher's logbook, gang tally sheet,
  bridge inspector's slip. Warm, physical, procedural. Nothing here is a speech; everything is a
  *measurement somebody was proud of*. Bridges have names, and the names are older than the people
  arguing about them.
- **Siege Year is written in the voice of the wait.** Notice-board, sentry rota, ration slate,
  the dull dread of an ordinary morning that might be the last ordinary morning. Never a battle
  scene. *Write a year, not a fight.* The besieger is a schedule, not a villain.

**What the two share.** Both are stories about **ownership of ground**: Exp. 25's question — *when
you rebuild a line, who owns the ground it crosses?* — and the Long Siege's question — *what will
we have agreed to by the time this ends?* — are one question asked at two scales. The shelter that
restores a line becomes a target *because* it now has something worth cutting.

**The second layer.** The pairing is one image seen from both ends. A rail line is a promise that
somebody will be at the other end; a siege is a promise that somebody will not let you leave — and
both are kept by people who write things down. What the two subjects share is a *timetable*: the
corridor runs on one, the besieger keeps the other, and the shelter lives in the gap between
schedules with a pen. The Line Book and the notice board are the same document addressed to
different moods. And both documents are dated — which is the only way a promise outlives the
people who made it.

---

## 1. Goal & Outcome

### 1.1 Iron Road (IR)

> *Design intent: the player should be able to point at a map and say "I did that stretch."*

- **Goal:** Turn the existing rail owners (`RailwaySystem`, `RailTrackMaintenanceLedger`, `RailwayInterlockEngine`, the logistics/grinding/rerailing catalogs) into a **restoration campaign**: an authored set of **Faults** on each corridor segment, a **derived Restoration Rung** per segment (Lost → Surveyed → Cleared → Relaid → Certified → Running), a **Railhead** (the furthest certified node from the shelter yard), a **Survey → Clear → Relay → Load-test → Certify → First Train** verb chain that routes through the *existing* repair/dispatch verbs, and a **Line Book** (a chronicle of who did what to which segment). The campaign carries Exp. 25's storyline ("The Line That Holds") on a mechanical spine that the existing engines can actually verify.
- **Outcome (observable):**
  1. On a fixed seed the five authored corridor segments start as authored **Faults** (a rockslide on the silo spur, a scoured pier on the dam trestle, a raider-picked stretch on the mainline, a jammed points on the switchyard branch, a leaning signal frame on the bypass) rather than as the near-pristine numbers the seed produces today (§2 finding E6).
  2. Each Fault blocks dispatch *until cleared through the owner's own verb* (`ClearTrackObstacle`, `RepairTrack`, `RepairBridge`, `RailTrackMaintenanceLedger.Maintain`), never by a new resolver.
  3. A **load test** runs the intended locomotive class over a segment at the intended tonnage through `EvaluateTrackFeasibility` and, if it passes, stamps a **certified tonnage** on that segment. Dispatch above certified tonnage is *advised against* (advisory only — DEC-IR-04; it is never refused, so no save can be bricked by a missing certificate).
  4. A **first train** over a certified corridor closes the segment's restoration and writes one **Line Book** entry.
  5. The Railhead advances only by certification; the region's map shows it. Save/load mid-restoration round-trips; a legacy save with no overlay loads as "campaign not started" and every rail behaviour equals today's.
- **Non-Goals (Iron Road):** no new dispatch, pathfinding, topology or train-movement code (`RailwaySystem` stays the only owner that moves a train); no second wear/gauge model (the maintenance ledger remains the physical layer); no new item catalog beyond the few named in §8; no new routed panel; no player-driven train physics; no ride-along expedition mode; no rail *town* economy (that is Exp. 25 §7.5 and Exp. 93 *A Town on the Siding*); no Unity.
- **"Done" (Iron Road):** §11 IR acceptance passes via `bin/run-scoped-tests`; parity holds on a saved corpus with the campaign inactive; the handoff lists untouched shared paths.

### 1.2 Siege Year (SY)

> *Design intent: the player should feel a siege as a season with a front porch — dread that
> starts before the first arrow and does not end when the last one is put away.*

- **Goal:** Wrap the Long Siege engine (`SiegeSystem`, prefix `LS`) in a **year-shaped arc** with three movements: a **Build-up** (authored **Signs** the besiegers cannot help leaving — scouts, surveyed roads, stockpiles, silence — read from real owners, and a **Warning Order** that gives the player a bounded number of days of notice); the **Siege** (the LS daily tick, unchanged); and a **Breaking** (a **Terms Year** and a **Scar Ledger** that make the aftermath visible for 30 days after the siege ends). The arc adds **no combat**, no resolver call and no second siege state — it is a calendar, a set of read-only projections, and an aftermath ledger that hangs off the LS ending.
- **Outcome (observable):**
  1. When a doctrine row's **warning gate** arms (aggression and visibility from the Iron Raiders owner, plus the doctrine's authored scouting rate), a Warning Order opens with a **notice** of roughly 13–38 days (doctrine base + seeded jitter, difficulty-scaled at the read site, clamped 8–38) and emits one **Sign** per day-window from a doctrine's Sign table; each Sign names the existing surface it was *read from* (sentry log, Watch acoustics, road traffic, trade prices).
  2. During the notice the player uses **existing verbs only** (repair/reset, stock, drill, dispatch a runner ahead, close a road); a **Readiness Board** projects the real owners' values and is never stored.
  3. The first raid after notice expiry uses the *existing* raid path. If repelled, the LS engine starts the siege (DEC-LS-01 unchanged). If it is not repelled, the ordinary raid consequences apply and the Warning Order **dissolves without a siege** — a warning is not a promise.
  4. On any of the five LS endings the arc opens a **Breaking**: a 30-day **Scar Ledger** (what the wall, the ledger, and the names on the wall now carry) and, for *Negotiated*, a **Terms Year** (obligations that expire on a fixed day, visibly, in the chronicle).
  5. Save/load in Build-up, mid-siege and mid-Breaking round-trips; no doctrine rows ⇒ no arc ⇒ every raid behaves as today.
- **Non-Goals (Siege Year):** no change to `ResolvePreCombatRaid`, raid strength, perimeter/Watch state or the LS daily tick; no new faction system; no new save section; no new routed panel; no besieger identity beyond authored doctrine rows; no explanation of motive (LS-OM-1…6 stay open); no outposts (Year Two owns `OutpostSettlementSystem`); no Unity.
- **"Done" (Siege Year):** §11 SY acceptance passes; LS acceptance (raid parity, one resolver call per day) still passes; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One optional hook shape, shipped dark until both ends exist (§10): a **cut** (the Siege's Starve/Sap pressures may block a corridor segment through the existing `SetBlocked`/sabotage seam) and a **relief train** (a certified corridor lets the LS *Relieved* ending carry a train run instead of a runner). Neither subject reads the other's *state*; each exposes one boolean/enum to a shared adapter.

---

## 1b. Texture, Mystery & Voice

**The Iron Road: a line is the argument you keep winning against rust.**

Rail is the only expansion where a *bridge is more important than a rifle* (Exp. 25 §1.1). The
campaign's literary shape is therefore not a set of fights but a set of **certifications**. To the
player a segment is either *known*, *clear*, *sound*, *proved*, or *running*, and each rung is a
person saying so in writing. The Line Book is the campaign's memory: every rung is initialled.
That initialling is the emotional payload — the line matters because named people put their names
on it, and a name on a bridge is a debt.

**Siege Year: a shelter is a calendar someone else is holding.**

The point of a *year* is that the besieger's patience is the one thing the player cannot spend.
The Build-up is where the shelter learns it is being read; the Siege is where it learns what it
is worth; the Breaking is where it learns who it now is. None of these is a difficulty setting.
They are three answers to the same question the shelter asked on Day 1: *do we stay?*

**What the player is never told.**

- **Who built the corridor.** The line predates the shelter; no plan explains the gauge, the
  bypass, or why a spur runs to a silo. The authored inspector's slips imply a plan and stop.
- **What the Toll besiegers are assessing.** *Toll* is a doctrine name (DEC-LS-08). The Signs show
  their *method* (measuring, counting, pacing the wall) and never their *reason*.
- **Whether the Warning Order was a kindness.** The notice is seeded, not authored per sign.
  Whether the besiegers *wanted* to be seen is not modelled, and must not be.
- **What "Certified" is certified against.** The load test proves a number, not safety. Bridges
  that pass still fail in storms (Exp. 25 beat 12). The plan preserves that gap on purpose.

**Voice — sample fragments (content candidates for `line_book_lines.json`, `siege_year_signs.json`).**

> "Delta to Dam, span eleven, pier three. Scour to the second course. We could patch it. We will not.
> Somebody's grandfather put a name on this pier, and I would like it to still be legible."
> — bridge inspector's slip (IR)

> "Certified for 120. Say it back to me. One hundred and twenty. *Not* one-fifty because the wind is
> kind today." — Vera Kast, to the load crew (IR)

> "Day nineteen of notice. Two men on the ridge for the third morning. They are not hiding. That is
> the message." — sentry log (SY)

> "The prices at the gate changed before anyone came. Salt up, rope up, lamp oil up. Somebody is
> shopping for a long visit." — quartermaster's slate (SY)

> "Terms: eleven months. I have written the date on the wall so nobody can say they were not told."
> — Negotiated ending, chronicle stub (SY)

**Design texture beats.**

- **Initial every rung (IR).** Each rung's certificate names a person (a survivor, a gang chief, an
  inspector). Loss of that person makes the *certificate* **Stale**, not the track worse — a fact
  that lands hard the first time it happens.
- **Announce before acting (SY).** Every Sign is a projection, never a hidden roll. The player
  must never lose to something they were not shown (inherits LS-P3/DEC-LS-05).
- **One line, one reading.** The Railhead is *derived*; the Warning Order is *derived*. If a value
  can be recomputed from an owner, it is never stored (Rule 5).
- **A year is longer than a fight.** The arc's minimum runtime is 60 days from first Sign to last
  Scar so the aftermath cannot be skipped by a fast win.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §7b/§9b remain the day-entry
chronicles; this section is the **objects** those entries leave behind. §17's register is
unchanged; the fragments below are content candidates and deliberate silences, not new recorded
questions.)*

**What the corridor leaves lying around.**

> "Line Book, cover: a hand-drawn map with the gauge written in two hands. The second hand is the shelter's."

> "Certificate, rung *Sound*, initialled. The initialling is the emotional payload; the track is only the excuse."

> "Kilometre eleven: the ties burned for a fire. The survey note says 'someone was cold' and nothing else."

**What the siege leaves lying around.**

> "Board: water 38 days, food 24. Written plain because somebody once told the quartermaster that fear is a kind of ignorance."

> "Pegs in the dark, regular intervals, ending at the north wall. Whoever drove them was not in a hurry. The regularity is the message."

> "Terms, written on the wall with the date, so that nobody can say they were not told."

**Scenes the player may piece together.**

> "The load test passes and Kast signs with a hand she does not sign with. The book records the number. Nothing records the hand."

> "A voice on the open band reads a number, a date, a number. Nobody wants to be the one who writes the date. The Keeper writes the date."

**Held silences (texture, not register rows).**

- What the second hand on the gauge drawing knew. The line predates the shelter and the drawing predates the people arguing about it; the survey that would settle it does not exist. Texture only.
- What the besieger's Year Entry would say if it were ever shown (§16 lists it as *never shown*). The other side keeps a chronicle; this plan declines to translate it and the silence is the design.

**Fourth pass — the line and the wait (texture only; §17 register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §17's register is unchanged; the
fragments below are content candidates for `line_book_lines.json` / `siege_year_signs.json` and
deliberate silences, not new recorded questions.)*

**The shape of the polish.** Both halves run on paper that was signed. The Iron Road initialled by
hand and the Siege Year dated on a wall are the same act of *putting a promise where weather cannot
edit it*. Prose should keep the two vocabularies in adjacent columns — the bridge inspector's
measurement beside the sentry's count — and never let either win. A name on a bridge is a debt; a
date on a wall is a debt; the Line Book and the notice board are one ledger of them.

**What the corridor leaves lying around.**

> "Certificate, rung *Sound*, a second initial beneath the first. Two initials is not a promotion;
> it is a date arriving."

> "Line Book, page ruled for the First Train entry. The ruling is old. The ruling is patient."

**What the siege leaves lying around.**

> "Sentry log, day twenty-one: 'the two men on the ridge have a dog now.' The log records the dog.
> The log does not speculate."

**Held silences (texture, not register rows).**

- What the silo spur was for. A spur runs to a silo and no plan explains it (§1b); the inspector's
  slips imply a purpose and stop, and must keep stopping. Texture only.
- Whether a signed number and a signed warning weigh the same. Both are dated and initialled
  (§1b); the plan keeps them in one ledger and never ranks the two hands.

---

## 2. Evidence table (verified 2026-09-29 against the live worktree; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `RailwaySystem` (854 lines) is the canonical owner of nodes, segments, trains, dispatch and pathfinding; `EnsureSegmentState` seeds `integrity = base_integrity` (float 0..1) and `bridgeIntact = !bridge_required`. | `Assets/Ashfall.Core/Expeditions/RailwaySystem.cs` L202–216 | LIVE |
| E2 | Traversal rule: a segment is traversable iff `integrity ≥ 0.40`, bridge intact when required, not `isSabotaged`, and train mass ≤ `max_train_mass`. Same rule in `IsRailCorridorOperational` / `GetOperationalRailCorridor`. | `RailwaySystem.cs` L301–314, L792–827 | LIVE |
| E3 | Public verbs already exist for every restoration act: `RepairTrack(segmentId, integrityRestored)` (consumes 1 `steel_rail_segment` or 10 `scrap_metal`), `RepairBridge(segmentId)` (2 `steel_rail_segment` + 2 `railroad_ties`, sets `bridgeIntact`, integrity ≥ 0.75), `ClearTrackObstacle` (5 `scrap_metal`, clears `isSabotaged`), `DispatchTrain`, `DispatchExpedition`, `ClearDerailment`. | `RailwaySystem.cs` L238–299, L372, L678, L725 | LIVE |
| E4 | A **second, advisory** physical layer exists: `RailTrackMaintenanceLedger` (`rail_track_maintenance` save section) tracks per-segment `GaugeStability`, `TrackWearPermille`, `BridgeIntegrityPermille`, `MaxBridgeLoadTons`, `IsBlockedByDebris`; `Evaluate`/`ApplyRun`/`Maintain`/`SetBlocked`; **it never moves a train and dispatch does not consult it** (INTEGRATION_PLANS.md L118 "deferred: wiring maintenance feasibility into RailwaySystem dispatch refusal"). | `Rail/RailTrackMaintenanceLedger.cs`; `INTEGRATION_PLANS.md` L118 | LIVE |
| E5 | `RailTrackMaintenanceEngine`: locomotive weight table (handcar 2t, steam shunter 30t, diesel rig 120t, armoured battle train 250t); `EvaluateTrackFeasibility` returns `Passable / SpeedRestricted / BridgeLoadRefusal / GaugeSpreadRefusal`, derailment risk in permille (base gauge risk 10/80/300/1000 + wear×0.2), threshold 400; `ApplyTrainWear` (wear +15% of tons, bridge fatigue +5% of tons over 50t; gauge tier steps at wear 800 / 950); `PerformMaintenance` (repair power = (mat×500 + min(labor,100)×500)/1000). | `Rail/RailTrackMaintenanceEngine.cs` L96–260 | LIVE |
| E6 | **`SeedFromTopology` ignores authored `base_integrity`**: it writes `TrackWearPermille = 100`, `BridgeIntegrityPermille = 1000` (the ternary `bridgeRequired ? 1000 : 1000` is constant) and `GaugeStability = Pristine` for every segment; the authored 0.3–0.85 `base_integrity` reaches only `RailwaySystem.integrity`. Result today: the maintenance ledger reports a *pristine* corridor while dispatch (E2) already blocks the dam trestle (`integrity 0.3`, `bridgeIntact false`). The two layers disagree on day one. | `RailTrackMaintenanceLedger.cs` L100–124 vs `RailwaySystem.cs` L202–216; `rail_network.json` | LIVE (finding) |
| E7 | `rail_network.json`: 5 nodes (Holdfast Central Yard [Terminal], River Delta Junction [Junction], Missile Silo Rail Spur [Depot], Hydro Dam Terminus [Terminus], Old Railway Switchyard [Junction]); 5 segments (18, 24, 32, 12, 28 km; `base_integrity` 0.85/0.40/0.30/0.55/0.60; only the trestle has `bridge_required`; `max_train_mass` 250/200/180/220/200; hazard tags `raider_patrol`, `derailment_risk`, `rockslide`, `trestle_bridge`, `ambush_point`); 5 cars (diesel switcher, freight hopper, flak gun car, hospital surgery car, handcar). | `Assets/StreamingAssets/Data/rail_network.json` | LIVE |
| E8 | By E2 and E7, on a fresh campaign **only the dam trestle is impassable today** (integrity 0.30 < 0.40; bridge not intact). Silo spur (0.40) passes the `< 0.40` test by a hair. The "restoration" content is therefore *thin*: one broken segment. | arithmetic on E2/E7 | LIVE (finding) |
| E9 | Logistics edges (5), interlock junctions (3) + maintenance profiles (`maint_yard`: 1 `item_track_maintenance_kit`, obstruction/day 0.04, decay 0.03), grinding profiles/heads, rerailing equipment (`rerail_hydraulic_actuator`: 300 W, 1 day, 90%, restores train 18 and track integrity 0.08). | `rail_logistics_catalog.json`, `railway_interlock_catalog.json`, `rail_grinding_catalog.json`, `rerailing_equipment_catalog.json` | LIVE |
| E10 | Host seams: `Main.Railway.Integration.cs` (134 lines), `Main.RailTrackMaintenance.cs` (`RecordRailRun`, `EvaluateRailTrack`, `MaintainRailSegment`, `GetRailTrackMaintenanceCensus`, `RailTrackMaintenanceHostSession`), `Main.DraisineRerailing.Integration.cs`, `Main.RouteInfrastructure.Integration.cs`, `Host/RailGrindingSaveStore.cs`. `RailwaySystem.OnTrainDispatched` drives wear recording. | `src/` listing | LIVE |
| E11 | Exp. 25 authored the story: Vera Kast, Dorn Hale, Mara Osk, Lin Vey, Hask Orr, Grale, Koval, Pim; 15 beats; 8 branching choices; endings *Open Line / Toll Road / Broken Span / Town's Line / Iron Commons / Fade*. Its proposed systems (`TrackMaintenanceSystem`, `BridgeSystem`, `LocomotiveSystem`, `RailScheduleSystem`, `RailTownSystem`, `RailInterdictionSystem`, `GaugeSystem`, `RailCommonsSystem`) are **proposals**; the live substitute for the first is the ledger in E4. | `docs/expansions/wave3/expansion_25_the_iron_road_plan.md` §5, §7 | LIVE (design) |
| E12 | Long Siege engine plan (LS) is **DRAFT, not implemented**: `SiegeSystem`, doctrines (Toll, Quiet Ring, Diggers), five pressures (Probe, Starve, Noise, Parley, Sap), five endings (Lifted, Relieved, Negotiated, Ground down, Fallen), state nested in `settlement_defenses`. `class SiegeSystem` appears nowhere in `Assets` or `src` (re-checked 2026-09-29). | `.ai/plans/long-siege-2026-09-29.md`; grep | LIVE (confirmed absent) |
| E13 | Raid path: `ResolvePreCombatRaid` called from `Main.Muster.cs` (Iron Raiders raid) and one debug command; Iron Raiders exposes aggression 0..1, visibility, `EvaluateRaidChance`, `ExecuteRaid`. | `Defense/DefenseSystem.cs` L290; `Muster/IronRaidersSystem.cs`; `Main.Muster.cs` | LIVE (per LS E1–E4) |
| E14 | Campaign calendar/day owner exists (`CampaignCalendar`, `CampaignDayCoordinator`); day-owner registration seam is `Main.SubsystemComposition.cs` / phase-5 owners (rail ticks emit `rail_track_maintenance_ticked`). | `Campaign/`; INTEGRATION_PLANS L118 | LIVE |
| E15 | Save registry: sections `railway`, `rail_track_maintenance`, `rail_interlock`, `settlement_defenses`; SaveSectionRegistry pin 245 at 2026-09-24. **Never assume a new section is allowed** — nest additively (Rule 5). | `Save/SaveSectionRegistry.cs`; INTEGRATION_PLANS L118 | LIVE |
| E16 | Two chronicle writers: `CulturalArchiveVaultSystem.TryRecordChronicleEntry` (stores a `summary_key`, dedupes) and `JournalSystem.TryAddRawEntry` (free text, deduped per key). `ArchiveDeskSystem` is a transcription queue, not a chronicle. | `Culture/CulturalArchiveVaultSystem.cs` L385; `Journal/JournalSystem.cs` L281 | **RESOLVED (§2b)** |
| E17 | Chapter Profiles (Year Two P1B) define per-storyline Reckoning days and day windows; they are the correct activation gate for a campaign arc. | `.ai/plans/y2-p1b-chapter-profiles-2026-09-29.md` | LIVE (plan) |
| E18 | `OutpostSettlementSystem` (Year Two owner) has `OutpostInstance { ConditionPermille, GarrisonSurvivorIds, DaysSinceSupply, IsStarving, IsOverrun, RationReserve }`. Outposts are Year Two's; this plan reads nothing from them. | `Settlements/OutpostSettlementSystem.cs` | LIVE (read-only) |
| E19 | Difficulty scalars exist (`hostile_encounter_mult`, `equipment_decay_mult`, `crisis_deadline_mult`, …); XP-WAVE1 difficulty authority is COMPLETE. Scalars are applied at read sites, never inside resolvers. | `difficulty_presets.json`; INTEGRATION_PLANS | LIVE |
| E20 | `TrainDispatchStatus` has Idle/Preparing/EnRoute/Derailment/RobberyAmbush/Arrived; `RobberyAmbush` and `Derailment` are existing dispatch outcomes. | `RailwaySystem.cs` L11–20 | LIVE |

**Two findings that shape the plan (recorded so nobody rediscovers them mid-package):**

1. **E6/E8 — the restoration story cannot be told from the shipped numbers.** Only one segment is broken; the ledger says all are pristine. The campaign therefore **authors Faults** and gives each a *native footprint* (`isSabotaged`, `bridgeIntact`, an integrity cap, a ledger gauge/wear/bridge value) applied through **one additive method per owner** (DEC-IR-03), so the existing traversal rule (E2) refuses dispatch on its own — rather than editing `rail_network.json`'s `base_integrity`, which other consumers already read, or adding a second gate.
2. **E4/E2 — two layers, one truth needed.** The dispatch authority (`RailwaySystem`, float integrity) and the physical ledger (`RailTrackMaintenanceLedger`, permille wear) must agree on what a Restoration Rung means. The plan derives rungs from **dispatch authority first**, and lets the ledger *refine* (never contradict) the reading (§6.1).

---

## 2b. Evidence pass 1 — premises checked against source (2026-09-29)

Every `VERIFY` and every load-bearing LIVE claim in §2 was re-read against the worktree. **E1–E7, E13, E14, E18, E19, E20 were confirmed line for line** (`RailwaySystem.cs` is 854 lines; the `< 0.40` traversal test is at L306, L802 and L823; `RepairTrack`, `RepairBridge`, `ClearTrackObstacle`, `DispatchTrain` and `ClearDerailment` are public at L238, L263, L284, L372 and L725; `SeedFromTopology` writes `TrackWearPermille = 100`, `BridgeIntegrityPermille = 1000` and `PristineStandard` for every segment at `RailTrackMaintenanceLedger.cs` L115–118 exactly as E6 says; `rail_network.json` holds 5 segments with `base_integrity` 0.85 / 0.40 / 0.30 / 0.55 / 0.60 and only the trestle `bridge_required`). E12 is still true (`class SiegeSystem` appears nowhere in `Assets` or `src`). What changed:

| # | Open item | Result | Edit made |
|---|---|---|---|
| E16 | Chronicle writer | **Two writers exist, with different powers.** `CulturalArchiveVaultSystem.TryRecordChronicleEntry(campaignDay, eventType, summaryKey, participants, authorId, volumeId)` stores a **`summary_key`, not free text**, and refuses duplicates (`duplicate_chronicle`); `JournalSystem.TryAddRawEntry(knowledgeKey, text, author, day)` takes free text, deduped once per key. `ArchiveDeskSystem` is a *transcription queue*, **not** a chronicle. | Line Book entries with a **fixed** sentence use `TryRecordChronicleEntry` with an authored key; entries that carry a **composed** line (a day, a segment name) use the journal (DEC-IR-12) |
| — | Which panel renders `RailwaySystem` | `RailwayTerminalPanel : IBindablePanel` (`Bind(RailwaySystem)`) and `RailGrindingPanel`. `Plans130To133Panel` also holds a `RailwaySystem` reference and states it "remains the authority for train condition, track integrity, and derailment state". | §4 presentation names `RailwayTerminalPanel` for the Line Book strip |
| — | **Two `TrackSegmentState` classes** | `Ashfall.Core.Expeditions.TrackSegmentState` (camelCase: `integrity`, `bridgeIntact`) and `Ashfall.Core.Rail.TrackSegmentState` (PascalCase: `SegmentId`, …). Different namespaces, so nothing fails to compile — but the same name means two things. | every rung read states the namespace: the **Expeditions** class is dispatch truth; the **Rail** class is the advisory ledger |
| — | Duty-roster post `post_track_gang` | **Does not exist.** `duty_roles.json` has six roles: `night_watch`, `mess`, `hatch_opener`, `intake_sleeper`, `expedition`, `ward`. | new **DEC-IR-11**: one additive `duty_roles.json` row (`track_gang`), owned by the duty roster; until then Assign gang uses the `expedition` role's crews as the labour source |
| — | Interlock reservation for The Meet (M10) | **Exists.** `RailwayInterlockEngine.RequestRoute(junctionId, routeId, expeditionId)` and `ReleaseRoute(junctionId, expeditionId)`; `BuildAvailability()` lists open, blocked and restricted routes. | §11 beat 13 uses `RequestRoute`/`ReleaseRoute`; VERIFY closed |
| — | "Sleep debt" owner | There is no sleep-debt concept. Rest is `NeedKind.Fatigue` in `NeedsSystem` (needs: Hunger, Thirst, **Fatigue**, Warmth, Morale, Health, Hygiene, Numbness, RadiationAnxiety). | the Rest Sign reads average **Fatigue** |
| — | Storm event name for the `storm` stale trigger | **No rail-facing storm event exists.** `DisasterResponseSystem` has `DisasterType.Flooding` but for *shelter rooms* only. Weather is a daily `WeatherKind` (FalloutStorm, BlackRain, Blizzard, IceStorm …) and **weather never lowers rail integrity today**. | `storm` becomes a *derived* trigger: a heavy-weather day on a rung's route since certification makes the certificate **stale** (an inspection rule, not a physical loss) — read daily from `WeatherSystem`, not subscribed (DEC-IR-13) |
| — | Chapter Profile field names (`ChapterProfileId`, `maxSiegeArcs`) | `y2-p1b-chapter-profiles` does not define either name yet. | recorded as a **cross-plan contract to agree**, not a source fact |
| — | Host self-test naming | Flags are kebab-case with a `-selftest` suffix and are registered in `HostCliRegistry.cs` as `(name, aliases[])` (e.g. `--rail-track-maintenance-selftest` at L1035). | `--siege-year-selftest` follows the pattern; one registry row (`INT`) |

**What did not change:** the four authored Faults, the Restoration Rungs, the rule that dispatch authority speaks first, and the one-additive-method-per-owner discipline. The evidence removed three invented dependencies (a duty post, a storm event, a sleep-debt owner) and confirmed the rail spine.

**One consequence worth stating plainly.** Weather does not touch the rail today, so *The Storm* (beat 12) cannot be a physical event without a new mechanism. The plan keeps it as a **story rule the certificate obeys** — *"a certificate is only as good as the last calm day"* — which suits the tone and asks nothing of the weather owner.

---

## 3. Authority table (one authority per concern — CLAUDE.md Rule 5)

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Topology, dispatch, pathfinding, train movement | `RailwaySystem` | **One** additive public method `ApplyCampaignFootprint(segmentId, sabotaged?, bridgeIntact?, integrityCap?)` — idempotent, may only *lower* condition, called once per authored Fault at chapter activation. Dispatch then refuses natively through the existing traversal rule (E2); **no new traversal code and no provider hook.** **DEC-IR-03, `INT`.** |
| Track wear, gauge, bridge load (physical layer) | `RailTrackMaintenanceLedger` / `RailTrackMaintenanceEngine` | Read-only `Evaluate`; `Maintain`/`SetBlocked` called through existing public methods; **nested additive** `Restoration` DTO in `RailMaintenanceState` (DEC-IR-02) |
| Material costs of repair | `RailwaySystem.RepairTrack/RepairBridge/ClearTrackObstacle` + `RailTrackMaintenanceLedger.Maintain` | none — every restoration act ends in one of these calls |
| Signals, locks, route reservations | `RailwayInterlockEngine` | read-only (the Meet beat asks it who waits) |
| Re-railing / recovery | Draisine/rerailing owners | read-only; a derailment still uses `ClearDerailment` |
| Rail *restoration* state (faults, rungs, certificates, Railhead, Line Book pointers) | — | `RailRestorationSystem` (pure Core) — **derived where possible; stored only for human acts (survey day, certificate)** — DEC-IR-01/02 |
| Raid resolution, traps, perimeter | `DefenseSystem`, `PerimeterDefenseSystem` | none |
| Raid opportunity, besieger aggression/visibility | `IronRaidersSystem` | read-only signals to the Sign engine |
| Siege state, doctrines, pressures, endings | `SiegeSystem` (LS, unimplemented) | none; SY *wraps* it by reading its phase/ending events through a public read API (DEC-SY-02) |
| Warning Order, Signs, Readiness Board, Terms Year, Scar Ledger | — | `SiegeYearSystem` (pure Core) — **nested additive** `year` DTO inside the LS `siege` DTO in `settlement_defenses` (DEC-SY-02) |
| Stores/water/morale/sleep/wire | real owners | read-only (Readiness Board) |
| Chronicle / Line Book / Scar entries | Plan 34 chronicle + `ArchiveDeskState` | append through the existing public writer (E16) |
| Difficulty | `DifficultyScalarsProvider` | scales notice length and sign frequency at the read site only |

**Non-duplication statement.** There is *one* rail owner (`RailwaySystem`), *one* physical ledger, *one* siege engine, *one* raid resolver, *one* chronicle, *one* save-section registry. This plan adds two small state owners — `RailRestorationSystem` and `SiegeYearSystem` — that hold **only** facts no existing owner can hold: *who initialled what, on which day* (IR) and *what the shelter was warned of and what it now carries* (SY). Everything else is projected.

---

## 4. Claimed Paths (proposed; `INT` = integrator-owned)

**Core (IR):** `Assets/Ashfall.Core/Rail/RailRestorationSystem.cs` (new, pure), `Rail/RailFaultCatalog.cs` (new loader), `Rail/RailRestorationRungs.cs` (new, pure derivation), `Rail/RailLoadTest.cs` (new, pure — wraps `EvaluateTrackFeasibility`), `Rail/RailTrackMaintenanceLedger.cs` (**additive nested DTO field + one additive `ApplyCampaignFootprint`**, `INT`), `Expeditions/RailwaySystem.cs` (**one additive method `ApplyCampaignFootprint`**, `INT`), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`).

**Core (SY):** `Assets/Ashfall.Core/Defense/SiegeYearSystem.cs` (new, pure), `Defense/SiegeYearSigns.cs` (new), `Defense/SiegeYearReadiness.cs` (new, read-only projection), `Defense/SiegeYearBreaking.cs` (new: Terms Year, Scar Ledger), `Defense/DefenseSystem.cs` / `SiegeSystem.cs` (**additive nested DTO field only**, `INT`, and only after LS exists).

**Data (IR):** `rail_faults.json`, `rail_restoration_milestones.json`, `rail_certificates.json`, `line_book_lines.json`, `rail_load_tests.json`.
**Data (SY):** `siege_year_signs.json`, `siege_year_warning_orders.json`, `siege_year_scars.json`, `siege_year_terms.json`, `siege_year_lines.json`.

**Host:** `src/Main.RailTrackMaintenance.cs` (`INT`: campaign start, faults applied at chapter activation, provider hook binding), `src/Main.Railway.Integration.cs` (`INT`), `src/Main.Defense.Integration.cs` (`INT`, daily Sign tick), `src/Main.Muster.cs` (`INT`, warning-gate read), phase-5 day-owner registration (`src/Main.SubsystemComposition.cs`, `INT`).

**Presentation (both):** extend existing surfaces only — the rail/route panel(s) in `src/UI/` (`RailwayTerminalPanel`, which binds `RailwaySystem`; `RailGrindingPanel` is the grinding view — verified 2026-09-29, §2b) for a **Line Book strip**; `DefenseGridPanel` and `NightWatchPanel` for a **Warning/Year strip**. No new routed panel (DEC-IR-08, DEC-SY-07).

**Tests:** `Ashfall.Core.Tests/Rail/RailRestorationSystemTests.cs`, `RailRestorationRungsTests.cs`, `RailLoadTestTests.cs`, `Ashfall.Core.Tests/Save/RailRestorationSaveTests.cs`; `Ashfall.Core.Tests/Defense/SiegeYearSystemTests.cs`, `SiegeYearSignsTests.cs`, `SiegeYearBreakingTests.cs`, `Ashfall.Core.Tests/Save/SiegeYearSaveTests.cs`; extend `Rail/RailTrackMaintenanceLedgerTests.cs` and `Expeditions/RailwaySystemTests.cs` for footprint-never-applied parity.

---

# PART ONE — THE IRON ROAD (IR)

## 5. Iron Road — state and data model

### 5.1 What is stored, and why so little

The rule for this whole part: **if a value can be recomputed from an owner, it is not stored.** The
restoration campaign stores only *human acts* — who surveyed, who cleared, who initialled — and the
handful of ints that make those acts checkable later.

```text
RailRestorationState                       // nested, additive, in RailMaintenanceState.Restoration (null = campaign not started)
{
  int    SchemaVersion        = 1
  int    ActivatedDay                      // day the chapter activated the campaign
  string ChapterProfileId                  // Year Two P1B profile that activated it (name to be agreed with P1B, §2b)
  bool   FootprintsApplied                 // true after every authored Fault's native footprint was applied once
  int    LineBookSeq                       // monotonic entry counter (stable ids for chronicle writes)
  string GangPostId                        // duty-roster post feeding labour hours ("" = none staffed)
  Dictionary<string, SegmentRestoration> Segments   // key = canonical segment id
}

SegmentRestoration
{
  string SegmentId
  int    SurveyedDay          = -1         // -1 = never surveyed
  string SurveyorId                          // survivor id
  int    SurveyCompletenessPct = 0           // 0..100, what the last survey could see
  List<string> SeenFaultIds                  // faults the player currently knows about
  List<FaultProgress> Progress               // open work: { FaultId, HoursDone, Passes }
  List<FaultClearance> Cleared               // history: { FaultId, Day, ById, Verb }
  int    CertifiedTons        = 0            // 0 = no certificate
  string CertifiedLoco                       // LocomotiveClass name at test time
  int    CertifiedDay         = -1
  string CertifierId                          // survivor id
  int    IntegrityAtCertPermille             // dispatch integrity (float ×1000) when certified
  bool   BridgeIntactAtCert
  int    LedgerFingerprint                   // StableHash of ledger reading at last passing test
  bool   CertificateStale
  string StaleReason                         // "orphaned" | "worn" | "bridge" | "storm" | ""
  int    FirstTrainDay        = -1
  int    LastRunDay           = -1
  bool   LastRunClean         = true
  int    RunCount             = 0
}
```

**What is deliberately *not* stored:** the rung (derived, §6.1), the Railhead (derived, §6.4),
corridor operability (owner, E2), wear/gauge/bridge (ledger, E4), fuel/mass/consist (train, E1), the
Line Book text (rendered from `line_book_lines.json` + these ints at read time; only the chronicle
*pointer* is written through the chronicle owner, E16).

**Size.** Five segments × ~200 bytes of state + history lists capped at 32 rows each. A long campaign
stays under 12 KB. Caps and eviction (oldest `Cleared` rows fold into a running count) are DEC-IR-09.

### 5.2 Save wiring

- Nested additively in `RailMaintenanceState` (`rail_track_maintenance` section, E15). No new section
  (DEC-IR-02); if P0 shows the section's checksum contract forbids additive nesting, fall back to a
  sibling DTO inside `railway` — **never** a new registry entry without a foreman signature.
- `RestoreState`: null/missing `Restoration` ⇒ campaign not started ⇒ every behaviour equals today.
  Schema newer than supported ⇒ throw the same `InvalidOperationException` the ledger already throws.
- Unknown segment ids in a restored overlay are dropped with one warning (topology may shrink in a
  mod); unknown fault ids keep their history but stop counting toward rungs.

### 5.3 Catalogs (summary; row specs §8)

| File | Owner loader | Content |
|---|---|---|
| `rail_faults.json` | `RailFaultCatalog` (new) | 14 authored Faults (5 segments), each with kind, native footprint, reveal difficulty, clear mode, labour/materials |
| `rail_certificates.json` | same | minimum certified tonnage per segment; certificate stale rules; Running window (30 days) |
| `rail_load_tests.json` | same | test consists (handcar, shunter, diesel rig, armoured train) and cargo steps |
| `rail_restoration_milestones.json` | same | 10 milestones with predicate keys, Exp. 25 beat hooks, rewards (existing owners only) |
| `line_book_lines.json` | same | template lines per event type, 3–5 variants each, selected by `StableHash(segment, event, day)` (not RNG) |

---

## 6. Iron Road — rules

### 6.1 Restoration Rung (derived, never stored)

For a segment *S* at day *d*, the rung is the **highest** rung whose predicate holds, evaluated
top-down. Inputs are *read* from owners; the overlay supplies only the human-act fields.

```text
Rung 5  RUNNING    Certified ∧ FirstTrainDay ≥ 0 ∧ (d − LastRunDay) ≤ RunningWindowDays (30) ∧ LastRunClean
Rung 4  CERTIFIED  CertifiedTons ≥ MinCertifiedTons(S) ∧ ¬CertificateStale ∧ Rung3-conditions still true
Rung 3  RELAID     no open Wear/Bridge/Gauge Fault ∧ dispatchIntegrity ≥ 0.70 ∧ (¬bridgeRequired ∨ bridgeIntact)
                   ∧ ledger.Evaluate(S, Handcar, 0).Outcome ≠ GaugeSpreadRefusal
Rung 2  CLEARED    no open Obstruction/Sabotage Fault ∧ ¬isSabotaged (native) ∧ ¬IsBlockedByDebris (ledger)
Rung 1  SURVEYED   SurveyedDay ≥ 0
Rung 0  LOST       otherwise
```

Two properties that matter:

1. **Rungs can fall without anyone touching the overlay.** A storm that lowers integrity, a raider who
   sets `isSabotaged`, a bridge that fails — each drops the rung the next time it is read. The
   overlay never needs an "update" call to stay truthful.
2. **Faults gate by kind, not by id.** Adding a mod fault to `rail_faults.json` changes rungs without
   any code change.

Rung is a **read model** (`RailRestorationRungs.Derive(segment, readers, overlay, day)`), pure,
ordinal-stable, zero allocation beyond one small struct. It never writes.

### 6.2 Verbs — every one ends in an existing owner call

| Verb | Requires | Effect | Existing owner call |
|---|---|---|---|
| **Survey** `(segmentId, surveyorId)` | surveyor alive and free; segment reachable *from the Railhead* (you survey outward, one segment past the last cleared one) | sets `SurveyedDay`; reveals faults with `reveal_difficulty ≤ surveyorSkillBand`; records `SurveyCompletenessPct`; Line Book entry; **no RNG** | none (read + overlay write) |
| **Assign gang** `(postId)` | a duty-roster role `track_gang` exists (**it does not today** — six roles ship; DEC-IR-11 adds one additive row) or, until then, the `expedition` role's crews are the labour source | labour hours per day = Σ assigned survivors × `hours_per_survivor_day` (8) × skill modifier | duty roster owner (read) |
| **Work fault** `(faultId, crewIds[])` | fault seen; crew free; prerequisite faults cleared | adds crew-hours to `Progress.HoursDone`; when `HoursDone ≥ labour_hours` the clear call fires | see §6.2.1 |
| **Load test** `(segmentId, locoClass, cargoTons, testerId)` | rung ≥ 3 (Relaid) | evaluates via ledger; may cost wear via `ApplyRun`; produces pass / restricted / refused / near-miss | `RailTrackMaintenanceLedger.Evaluate`, `.ApplyRun` |
| **Certify** `(segmentId, certifierId, tons)` | last load test passed for ≥ `tons` at the *same fingerprint* | writes certificate fields; Line Book entry | none |
| **First train** | Certified segment(s) on the path | not a verb: the player dispatches through the existing `DispatchTrain` / `DispatchExpedition`; the campaign *observes* `OnTrainDispatched` + arrival and stamps `FirstTrainDay`, `LastRunDay`, `LastRunClean`, `RunCount` | `RailwaySystem` (observed only) |
| **Re-certify** | stale certificate | identical to Load test → Certify; a different certifier is allowed | as above |

**6.2.1 How a fault is cleared (the only place costs appear).**

`clear_mode` on the Fault row is one of:

- `native` — the row names an owner verb and arguments (`ClearTrackObstacle`; `RepairTrack(seg, 0.15)`
  repeated `passes` times; `RepairBridge`). The restoration system calls the verb **when labour is
  complete**; the owner charges materials and may return `Blocked`, in which case labour is *held*
  (`HoursDone` stays at threshold) and the UI names the missing item — the player never loses hours
  to a missing spike.
- `ledger` — the row names a `Maintain(materialPermille, labourHours)` pass on the physical ledger
  (`repair_material_permille` is derived from spent kits: 1 `item_track_maintenance_kit` = 500‰).
- `work_order` — the row lists materials the restoration system debits **through the same inventory
  port `RailwaySystem` already uses** (never a private stock); used only when no owner verb exists
  for the act (e.g. re-pinning a signal frame). At most three fault rows in the shipped catalog use
  it; the validator flags any `work_order` row whose materials could be spent by a `native` verb.

Labour accrues **per day** from the gang post and is applied in stable fault-id order to keep replays
deterministic; a fault can be worked by at most one crew per day (DEC-IR-07).

### 6.3 Load test

A load test is a *rehearsal on paper with a ruler*, not a train run. It never moves a train and never
derails one (DEC-IR-06).

```text
LoadTest(segment, loco, cargo, tester)
  eval   = ledger.Evaluate(segment.id, loco, cargo)             // Outcome, DerailmentRiskPermille, SpeedMult
  tons   = weight(loco) + cargo
  if eval.Outcome in {BridgeLoadRefusal, GaugeSpreadRefusal}:  return FAIL(reason = eval.RefusalReason)
  wear   = ledger.ApplyRun(segment.id, loco, cargo)              // yes: the rehearsal costs the rail (E5: +15% of tons)
  if eval.DerailmentRiskPermille >= 400:                         // engine threshold
       roll = stream(day, segmentId, "loadtest").NextInt(1000)
       if roll < eval.DerailmentRiskPermille:  return NEAR_MISS(tons × 90 / 100)   // conservative certificate
  if eval.Outcome == SpeedRestricted:             return PASS_RESTRICTED(tons, speed = eval.SpeedMultiplierPermille)
  return PASS(tons)
```

- **Fingerprint.** A passing test stores `StableHash(integrityPermille, bridgeIntact, wearPermille,
  gauge, bridgeIntegrityPermille)` on the segment. **Certify** refuses if the current fingerprint
  differs (the track changed since the test), preserving the rule *you certify what you measured*.
- **Cost of caution.** Each test wears the rail (a 150 t rehearsal adds 22‰ wear and, over 50 t,
  fatigues the bridge by 7‰). Players who test constantly pay for it in the maintenance ledger.
- **Near-miss is information, not punishment.** A near-miss writes a Line Book entry ("a sleeper
  moved under the third axle") and certifies 90% of the tested tonnage.
- **Determinism.** Same seed, same day, same segment, same consist ⇒ same roll. The stream is a
  `CampaignStreamIds` fork keyed `(day, segmentHash)`; `System.Random` is never used.

### 6.4 Railhead (derived)

```text
Railhead(d) = the node N maximising cumulativeKm(yard → N)
              over nodes reachable from rail_node_holdfast_terminal
              through segments all at rung ≥ CERTIFIED (rung 4) on day d.
```

Ties (equal distance) break on ordinal node id. If the first segment is below Certified the Railhead
is the yard itself (`0 km`). The Railhead is the one number the map, the Line Book header and the
milestones read; it is never stored, so a stale certificate visibly *walks it back*.

For the shipped topology the Railhead ladder is: yard (0) → Delta (18 km) → Switchyard (30 km) → Silo
(58 km via the bypass; 42 km via the spur, whichever is Certified) → Dam (50 km).

### 6.5 Stale certificates — "the paper outlives the person"

A certificate becomes **Stale** (rung 4 → 3, *the track is unchanged*) when any of:

| Trigger | `StaleReason` | Detected by |
|---|---|---|
| Dispatch integrity has fallen ≥ 100‰ below `IntegrityAtCertPermille` | `worn` | daily read of the owner |
| `BridgeIntactAtCert` was true and `bridgeIntact` is now false | `bridge` | daily read |
| A heavy-weather day (FalloutStorm, BlackRain, Blizzard, IceStorm) fell on the rung's route since certification — weather never lowers rail integrity today, so this is an **inspection rule**, not a loss (§2b, DEC-IR-13) | `storm` | daily read of `WeatherSystem` |
| The certifier is no longer among living survivors present in the shelter or on a rail-crew expedition | `orphaned` | daily read of the survivor roster |

Re-certification is a load test plus a Certify by anyone qualified. An **orphaned** certificate is the
campaign's quietest gut-punch: nothing about the rail changed, but the *person who vouched for it* is
gone, and the game asks the shelter to vouch again with somebody else's name.

### 6.6 Worked examples (all numbers from E1–E7; re-verify in P0)

**Example 1 — the mainline, day 1.**
`rail_segment_holdfast_to_delta`: base integrity 0.85, 18 km, `max_train_mass 250`. Chapter activation
applies Fault `fault_mainline_barricade` (Obstruction) → owner footprint `isSabotaged = true`. E2 ⇒
`CanTraverseSegment` false ⇒ no train leaves the yard. Rung: **Lost**. The player Surveys (1 day, the
surveyor sees the barricade — difficulty 1 — and the curve spread — difficulty 3 — only if their
skill band is ≥ 3) → **Surveyed**. A 24-hour gang shift completes the Work order → the restoration
system calls `ClearTrackObstacle` (5 `scrap_metal`) → `isSabotaged = false` → rung **Cleared** (or
**Surveyed** if an unseen *Wear* fault remains open — the player is told "survey incomplete: 1
unknown" but never *what*).

**Example 2 — the trestle load ladder.**
`rail_segment_delta_to_dam`: 32 km, `bridge_required`, `max_train_mass 180`, integrity 0.30. Native
state at start: `bridgeIntact = false`.
1. `RepairBridge`: consumes 2 `steel_rail_segment` + 2 `railroad_ties`; sets `bridgeIntact`; integrity
   → `max(0.30, 0.75) = 0.75`.
2. Ledger footprint at activation put the pier at `BridgeIntegrityPermille = 720`, gauge Minor spread,
   wear 350‰. Effective capacity = `180 × 720 / 1000 = 129 t`.
3. Load test: DieselFreightRig (120 t) + 20 t cargo = 140 t > 129 ⇒ **BridgeLoadRefusal**.
4. Fault `fault_trestle_pier_scour` clears by `ledger` passes: `Maintain(400‰, 40 h)` ⇒ repair power
   `(400×500 + 40×500)/1000 = 220` ⇒ bridge `+110‰` ⇒ 830‰ ⇒ capacity 149 t. Second pass ⇒ 940‰ ⇒
   169 t.
5. Test again at 120 + 30 = 150 t ⇒ passes; fingerprint stored; **Certify 150 t** → rung 4
   (MinCertifiedTons for this segment = 100).
6. First real run (a 150 t consist) applies wear `150 × 15 / 100 = 22‰` and bridge fatigue
   `150 × 5 / 100 = 7‰` through the existing `OnTrainDispatched` path — the certificate is *not*
   invalidated (well inside 100‰) but the numbers move, visibly, in the Line Book.

**Example 3 — orphaned.** Vera Kast certifies the trestle at 150 t on day 141. On day 150 she is
lost on a survey trip. Next daily read: `CertifierId` not among living survivors ⇒ `CertificateStale
= true`, `StaleReason = "orphaned"` ⇒ trestle rung 4 → 3; on a run where only the mainline and the trestle are Certified, the
Railhead falls from *Dam (50 km)* to *Delta (18 km)*. No wear changed. The Line Book adds: *"Certificate for span eleven no longer has a name to
stand behind it."*

---

## 7. Iron Road — authored content

### 7.1 Fault catalog (14 rows; footprints are the *only* mutation applied at activation)

Legend — **K**: kind · **Foot**: native footprint applied once through `ApplyCampaignFootprint` ·
**Rev**: survey reveal difficulty 1–5 (1 = visible from the yard) · **Clr**: clear mode · **Hrs**:
crew-hours · **Blk**: blocks dispatch natively when open.

| # | Fault id | Segment | K | Foot | Rev | Clr | Hrs | Blk |
|---|---|---|---|---|---|---|---|---|
| 1 | `fault_mainline_barricade` | Holdfast–Delta | Obstruction | `isSabotaged=true` | 1 | native `ClearTrackObstacle` | 24 | yes |
| 2 | `fault_mainline_curve_spread` | Holdfast–Delta | Gauge | ledger gauge `MinorSpread`, wear 330 | 3 | ledger `Maintain(300‰, 30h)` ×1 | 30 | no |
| 3 | `fault_silo_rockfall` | Delta–Silo | Obstruction | `isSabotaged=true` | 1 | native `ClearTrackObstacle` | 60 | yes |
| 4 | `fault_silo_sunk_ballast` | Delta–Silo | Wear | integrity cap 0.32 | 2 | native `RepairTrack(0.15)` ×3 | 45 | yes (integrity < 0.40) |
| 5 | `fault_silo_cutting_drain` | Delta–Silo | Gauge | ledger gauge `SevereDistortion`, wear 520 | 4 | ledger `Maintain(500‰, 60h)` ×2 | 120 | no |
| 6 | `fault_trestle_deck` | Delta–Dam | Bridge | `bridgeIntact=false` (already native) | 1 | native `RepairBridge` | 72 | yes |
| 7 | `fault_trestle_pier_scour` | Delta–Dam | Bridge | ledger `BridgeIntegrityPermille=720` | 4 | ledger `Maintain(400‰, 40h)` ×2 | 80 | no |
| 8 | `fault_trestle_approach_wear` | Delta–Dam | Wear | integrity cap 0.30 (native) | 2 | native `RepairTrack(0.15)` ×2 | 30 | yes |
| 9 | `fault_switchyard_jammed_points` | Delta–Switchyard | Gauge | ledger gauge `SevereDistortion`, wear 620 | 2 | ledger `Maintain(500‰, 50h)` ×2 | 100 | no |
| 10 | `fault_switchyard_scrap_choke` | Delta–Switchyard | Obstruction | ledger `IsBlockedByDebris=true` | 1 | ledger `SetBlocked(false)` after 16 h | 16 | no (advisory only) |
| 11 | `fault_bypass_leaning_frame` | Switchyard–Silo | Signal | none native; `work_order` (signal frame) | 3 | work_order: 1 `steel_rail_segment`, 2 `railroad_ties` | 36 | no |
| 12 | `fault_bypass_wear` | Switchyard–Silo | Wear | integrity cap 0.48 | 2 | native `RepairTrack(0.15)` ×2 | 30 | no (≥ 0.40) |
| 13 | `fault_bypass_ditch_flood` | Switchyard–Silo | Gauge | ledger gauge `MinorSpread`, wear 410 | 3 | ledger `Maintain(300‰, 30h)` ×1 | 30 | no |
| 14 | `fault_dam_terminus_turntable` | Delta–Dam | Signal | none native; `work_order` (turntable bearings) | 5 | work_order: 1 `scrap_metal`×12 | 40 | no |

**Design notes on the table.**

- **Blocking faults** number five (1, 3, 4, 6, 8): the yard is walled in on day 1, the mainline is
  the tutorial (24 hours, 5 scrap), and no other segment can be *reached* until it is cleared.
- **Rev 4–5 faults are the campaign's honesty test:** a player who surveys with a novice will
  certify a bridge that has a hidden scour. The Line Book will say *"survey incomplete: 2 unknown"*
  every time. The game never tells them what.
- **Everything in the table is a number in existing units.** Wear in permille, gauge in the existing
  enum, integrity as the existing float, bridge in permille. There is no new physical quantity.
- **Cost realism check (Example 2 math generalised).** Total blocking-fault labour = 24+60+45+72+30 =
  231 crew-hours. At a gang of 4 × 8 h = 32 h/day that is ~7.2 working days for the dispatch-blocking
  work alone; the whole line (all 14 faults: 24+30+60+45+120+72+80+30+100+16+36+30+30+40 = 713 h) is
  ~22 gang-days, plus testing. This is deliberately **a season, not an afternoon** and
  **a fifth of the Siege Year notice window at its longest** (see §10 X).

### 7.2 Milestones (10 rows in `rail_restoration_milestones.json`)

Each milestone is a **read predicate** over the derived rungs/Railhead plus an *existing-owner* reward
routed through the reputation / trade-route / chronicle owners. No milestone writes a new counter.

| Id | Name | Predicate (derived) | Exp. 25 beat | Reward (existing owner) |
|---|---|---|---|---|
| M1 | The Walk | any segment rung ≥ 1 | 1 The Survey | chronicle entry |
| M2 | The First Cut | mainline rung ≥ 2 | 3 The Cutting | +reputation "railwaymen" via existing reputation API (if faction exists; else none) |
| M3 | Mainline Proved | mainline rung ≥ 4 | 4 The First Train (dispatch pending) | trade-route rail leg becomes offerable (existing `GetOperationalRailCorridor` sees it) |
| M4 | Delta Reached | Railhead = Delta | 5 The Crossing | Delta Junction depot/market unlock (existing location data) |
| M5 | The Switchyard | Delta–Switchyard rung ≥ 4 | 6 The Tunnel (bypass) | salvage table access flag (existing) |
| M6 | The Spur | Delta–Silo rung ≥ 4 | 9 The Gang | silo depot supply flag |
| M7 | The Ring Closes | Switchyard–Silo rung ≥ 4 ∧ M5 ∧ M6 | 13 The Meet (setup) | second route to Silo (owner pathfinding sees it) |
| M8 | The Trestle Stands | Delta–Dam rung ≥ 4 | 8 The Trestle | dam terminus supply flag |
| M9 | First Full Run | a train completed Holdfast→Dam→Holdfast with `LastRunClean` on every segment | 14 The Reckoning (pre) | chronicle entry; achievement (existing `achievements.json` owner) |
| M10 | The Meet | two trains passed at a junction under interlock reservation | 13 The Meet | chronicle entry; hook to *The Treaty Table* (Right of Way) |

Achievement/reward wiring uses the existing owners; the plan adds **rows**, not systems.

### 7.3 Beat-to-rung map (how Exp. 25's 15 beats ride the spine)

| Beat | Mechanical anchor here | Note |
|---|---|---|
| 1 Survey | Survey verb, first segment | M1 |
| 2 The Argument | Branch choice *Line vs road* (Exp. 25 §5.5); no rung effect | authored dialogue; unchanged |
| 3 The Cutting | fault 10 + 3/5 (drainage) | the flooded cutting is the *silo cutting drain* (fault 5) |
| 4 The First Train | first dispatch over a Certified segment | M3 |
| 5 The Crossing | Delta (Grale's armoured train) | interdiction is Exp. 25 §7.6; only the *Cut* hook is read here |
| 6 The Tunnel | bypass (segment E) | tunnel/bypass choice is the **bypass** vs **spur** route choice |
| 7 The Town | Depot Town (Exp. 25; also Exp. 93) | rail-town economy out of scope |
| 8 The Trestle | segment C | M8 |
| 9 The Gang | gang post staffing; Mara Osk refuses access — a **Right of Way** condition (Pair 5) | |
| 10 The Theft | runtime `isSabotaged` on a segment | rung falls; owner state only |
| 11 The Toll | interdiction; read-only here | |
| 12 The Storm | `storm` stale trigger (§6.5) | |
| 13 The Meet | M10; `RailwayInterlockEngine.RequestRoute` / `ReleaseRoute` decide who waits | API verified (§2b) |
| 14 The Reckoning | Line Standing readout (§7.4) at the profile's Reckoning day | |
| 15 The Line That Holds | ending selection unchanged (Exp. 25) | |

### 7.4 Line Standing readout (input to Exp. 25's endings, not a new ending system)

At the Chapter Profile's Reckoning day (and on demand in the UI) the campaign renders:

```text
LineStanding { rungs[5], railheadNodeId, railheadKm, cleanRunsLast30, daysSinceLastCleanRun,
               staleCertificates, openBlockingFaults, milestonesReached }
```

Mappings are **advisory suggestions** the Exp. 25 ending selector may read; they are not endings:

| Ending (Exp. 25) | Suggested readout condition |
|---|---|
| The Open Line | ≥ 4 segments Running; `staleCertificates == 0`; no active toll |
| The Toll Road | ≥ 3 segments Running with a toll obligation active (interdiction/treaty owner) |
| The Broken Span | trestle was ever Running and is now rung ≤ 3 at the Reckoning (a *fall*, not a never) |
| The Town's Line / The Iron Commons | requires the Right-of-Way state from *The Treaty Table*; unavailable if that plan is absent |
| Fade | `cleanRunsLast30 == 0` and Railhead ≤ Delta |

### 7.5 Items (existing ids used; new ids only if P0 finds none)

Existing item ids seen in owner code: `steel_rail_segment`, `railroad_ties`, `scrap_metal`,
`item_track_maintenance_kit` (interlock profile), `item_hydraulic_actuator` (rerailing). **No new item
is added by this plan.** If P0 shows a *Surveyor's Chain* or *Plumb-and-level* is required for the
Survey verb's fiction, they arrive as two rows in the existing item catalog with a scanner
registration (`ContentUtilizationScanner`) and no new mechanics.

---

## 7b. The Line in Eleven Entries (prose texture — not authority)

*A Line Book as it might read across one restoration. Every number in it is derivable from §6.
This is what a player's chronicle should feel like when the game has been honest.*

**Day 62 — Survey, Holdfast to Delta.** Kast walked it with a chain and a notebook, two survivors
and Pim, who is not supposed to be here. Eighteen kilometres. The ties are good; the rail is
good; someone made a fire of both at kilometre eleven. Survey band 3 of 5: saw the barricade, saw the
curve. *"Survey incomplete: 1 unknown."* She wrote that line herself and underlined it.

**Day 65 — The barricade is a pile of wood.** Twenty-four hours, four of us, five scrap for the
cleaning bill. Koval says a barricade is an opinion; a rail is a fact. He is wrong about the second
half but I did not say so.

**Day 68 — Mainline: Cleared. Not Relaid.** The unknown turned out to be a spread curve. It has
been there longer than the barricade. Whoever fixed the barricade last spent their care on the wrong
thing. Thirty crew-hours, one maintenance kit at 500‰ — we spent 300 of it. The remaining 200 is
stored in my head as a debt.

**Day 71 — Load test, 150 tons, mainline.** Passed. Kast said the number aloud twice in front of the
crew, and then wrote it in the book: *certified for 150.* Then she signed. I have not seen her sign
anything else with that hand.

**Day 74 — First train.** Fourteen minutes from the yard to kilometre nine, no derailment, no
speech. Koval touched the boiler and said nothing for the whole afternoon. Railhead: Delta, eighteen
kilometres. It is the first time the shelter has a number for how far it goes.

**Day 88 — The Spur: rockfall and sunk ballast.** Sixty hours to shift the rock; forty-five hours and
three passes of new rail to bring the ballast up from 0.32 to 0.77. The cutting drain is still
unknown. Kast will not certify a line she has not seen draining. I am starting to see why.

**Day 103 — Trestle deck.** Two rails, two ties, one plank walk. It is not a bridge until it is
tested. *"A repaired bridge is a hope with a good paint job,"* said the inspector — who is Kast — and
then asked me to hold the plumb line.

**Day 112 — Pier three.** Scour to the second course, hidden until the fourth survey. We patched
twice. Capacity is 169 tons with the pier at 940‰. We will not run 180 over it. Not because the
number is unsafe; because it is *unproved*.

**Day 141 — Certificate: span eleven, 150 tons.** Kast's name and mine. If either of us is not here
in a year I would like the paper to say so.

**Day 150 — Orphaned.** *"Certificate for span eleven no longer has a name to stand behind it."*
The track did not change. The bridge is exactly as strong as it was yesterday. The shelter is less
sure of it, and that is the whole point of the certificate.

**Day 156 — Re-certified.** New name in the book. The same 150 tons. Koval crossed at dusk with one
car and a lantern and said "I would like to tell you this bridge is fine." He did not finish the
sentence. It is the most honest thing anyone has said about a bridge.


---

## 8. Data catalog specifications (both parts)

All files live in `Assets/StreamingAssets/Data/`, snake_case, integer `schema_version: 1`, validated by
`CatalogIntegrityValidator` (`INT`) and registered with `ContentUtilizationScanner`. **A row's presence is
not proof of reachability**: every catalog below names its consumer in §12.

### 8.1 Iron Road catalogs

**`rail_faults.json`** — one row per authored Fault.

```json
{
  "schema_version": 1,
  "faults": [
    {
      "fault_id": "fault_trestle_pier_scour",
      "segment_id": "rail_segment_delta_to_dam",
      "kind": "Bridge",
      "display_name": "Scour at pier three",
      "reveal_difficulty": 4,
      "blocks_dispatch": false,
      "footprint": { "ledger_bridge_integrity_permille": 720 },
      "clear_mode": "ledger",
      "clear_args": { "repair_material_permille": 400, "labour_hours": 40, "passes": 2 },
      "labour_hours": 80,
      "prerequisite_fault_ids": ["fault_trestle_deck"],
      "line_book_key": "fault_cleared_pier_scour",
      "survey_line_key": "survey_saw_pier_scour"
    }
  ]
}
```

Validator rules (each is one failing test): `segment_id` ∈ `rail_network.json` segments; `kind` ∈
{`Obstruction`,`Sabotage`,`Wear`,`Gauge`,`Bridge`,`Signal`}; `reveal_difficulty` 1..5;
`clear_mode` ∈ {`native`,`ledger`,`work_order`}; `native` rows name a verb in the allow-list
{`ClearTrackObstacle`,`RepairTrack`,`RepairBridge`}; `footprint` keys ⊆ {`sabotaged`,`bridge_intact`,
`integrity_cap`,`ledger_gauge`,`ledger_wear_permille`,`ledger_bridge_integrity_permille`,
`ledger_blocked`}; a footprint may only *lower* condition relative to the segment's authored
`base_integrity`; prerequisites form a DAG; `work_order` rows whose materials a `native` verb could
spend are rejected; every `line_book_key` exists in `line_book_lines.json`.

**`rail_certificates.json`**

```json
{
  "schema_version": 1,
  "running_window_days": 30,
  "stale_wear_drop_permille": 100,
  "near_miss_tonnage_percent": 90,
  "segments": [
    { "segment_id": "rail_segment_holdfast_to_delta",        "min_certified_tons": 150 },
    { "segment_id": "rail_segment_delta_to_silo",            "min_certified_tons": 120 },
    { "segment_id": "rail_segment_delta_to_dam",             "min_certified_tons": 100 },
    { "segment_id": "rail_segment_delta_to_switchyard",      "min_certified_tons": 120 },
    { "segment_id": "rail_segment_switchyard_to_silo_alt",   "min_certified_tons": 120 }
  ]
}
```

Every `min_certified_tons` must be ≤ that segment's `max_train_mass` (E7); a validator row fails
otherwise (cannot require a certificate the segment can never hold).

**`rail_load_tests.json`** — `consists`: `{ "consist_id", "loco": "ManualHandcar|LightSteamShunter|DieselFreightRig|ArmoredBattleTrain", "cargo_steps": [0,10,20,30,60,90] }`. The UI offers only these steps so the test surface is
finite and testable.

**`rail_restoration_milestones.json`** — the ten rows in §7.2:

```json
{ "milestone_id": "milestone_railhead_delta", "display_name": "Delta Reached",
  "predicate": { "kind": "railhead_at_least_node", "node_id": "rail_node_delta_junction" },
  "exp25_beat": 5,
  "rewards": [ { "kind": "chronicle", "key": "milestone_railhead_delta" },
               { "kind": "flag", "flag_id": "flag_rail_delta_market_open" } ] }
```

Predicate kinds (closed set, each a pure function of derived rungs/Railhead/overlay):
`any_segment_rung_at_least`, `segment_rung_at_least`, `railhead_at_least_node`, `all_segments_clean_run`,
`interlock_meet_recorded`, `all_of`, `any_of`. Reward kinds: `chronicle`, `flag`, `reputation`
(existing API), `achievement` (existing), `trade_route_unlock` (existing owner reads corridor operability
itself; the row is a *label*). Unknown kinds are a validator error, not a runtime fallback.

**`line_book_lines.json`** — `{ "event": "fault_cleared_pier_scour", "variants": ["…","…","…"] }`. Selected
by `StableHash(segmentId, eventKey, day) % variants.Count` (**not RNG**, so replays never shift).
Every event key referenced by any other catalog must exist here (validator).

### 8.2 Siege Year catalogs

**`siege_year_warning_orders.json`** — one row per LS doctrine (ids must match `siege_doctrines.json`).

```json
{
  "schema_version": 1,
  "orders": [
    {
      "doctrine_id": "doctrine_toll",
      "warning_gate": { "min_aggression": 0.50, "min_visibility": 0.40, "min_campaign_day": 30,
                        "cooldown_days": 120, "requires_installation": true },
      "notice": { "base_days": 24, "jitter_min": -5, "jitter_max": 10, "clamp_min": 8, "clamp_max": 38 },
      "gathering_fraction": 0.40,
      "arrival_grace_days": 7,
      "bluff_permille": 120,
      "sign_rhythm": [ [0.05,"Trace"], [0.20,"Trace"], [0.40,"Presence"], [0.60,"Presence"],
                       [0.75,"Pressure"], [0.90,"Pressure"], [0.97,"Arrival"] ],
      "reveals_doctrine_at_tier": "Pressure"
    }
  ]
}
```

Validator: `doctrine_id` ∈ `siege_doctrines.json`; rhythm offsets strictly increasing in (0,1); tiers ∈ enum; every
rhythm tier has ≥ 2 candidate Sign rows for that doctrine (so no-repeat draws never exhaust); `bluff_permille` 0..300.

**`siege_year_signs.json`** — 24 rows (8 per starter doctrine; §9.3).

```json
{ "sign_id": "sign_toll_pegs_in_the_dark", "doctrine_id": "doctrine_toll", "tier": "Pressure",
  "source": "watch_acoustics", "requires": [],
  "text": "Pegs, driven at regular intervals. Not our ground and not our pace.",
  "reading": "Surveyed line of approach; the wall is being measured, not tested.",
  "weight": 3 }
```

`source` ∈ {`sentry_log`,`watch_acoustics`,`road_traffic`,`market_prices`,`radio_traffic`,`animals`,`weather_smoke`,`gate_visitors`,`rail_ledger`};
`requires` ∈ {`rail_running`, `deep_works_active`, `radio_station_on_air`} (optional; a sign whose requirement is unmet is
excluded from the draw, never faked). `text` is what the player reads; `reading` is what the *game* means, shown only in the
codex after the arc ends (never during — the player interprets, then learns whether they were right).

**`siege_year_scars.json`**, **`siege_year_terms.json`**, **`siege_year_lines.json`** — §9.5–§9.6 row shapes below.

---

# PART TWO — SIEGE YEAR (SY)

## 9. Siege Year — rules and content

### 9.1 State (nested in the LS `siege` DTO inside `settlement_defenses`; DEC-SY-02)

```text
SiegeYearState
{
  int    SchemaVersion = 1
  Phase  Phase            // None | Notice | Gathering | Arrival | Siege | Breaking | Closed | Dissolved
  string DoctrineId, BesiegerId
  int    WarningOpenedDay, NoticeDays, NoticeEndDay
  bool   WillArrive       // seeded once at open; never re-rolled (bluff)
  bool   DoctrineRevealed
  List<SignShown>  Signs  // { SignId, Day, Source }  capped 12
  int    SiegeStartDay = -1, SiegeEndDay = -1
  string EndingId         // LS ending id, when known
  int    BreakingEndDay = -1
  string TermsId = ""; int TermsEndDay = -1
  // Snapshots — historical *facts*, never authority (Rule 5); each is one int, taken once
  int    PreSiegeFoodDays, PreSiegeWaterDays
  int    MoraleTroughPermille = 1000   // running minimum of average morale ×1000 during the siege
  List<string> BrokenAtEnd            // installation ids broken at SiegeEndDay
  int    ArcsThisChapter
}
```

**Not stored:** the Readiness Board (projection), Scars (derived from owners + the four snapshots above), the
Year Entry (rendered), the notice countdown (`NoticeEndDay − today`).

**The state holds no defender values that an owner already holds.** The two `PreSiege*` ints and the morale trough
are **history**: the owner still answers *what is it now*; SY answers *what was it then*. That distinction is the
justification for storing them and is asserted by a test (§14).

### 9.2 The arc: phases and the Warning gate

```text
None ──gate armed──▶ Notice ──0.60·N──▶ Gathering ──N──▶ Arrival(≤7 d) ──raid repelled & LS doctrine──▶ Siege ──LS ending──▶ Breaking ──30..60 d──▶ Closed
   ▲                                                      │
   └───────────────────── Dissolved ◀─── raid not repelled  │  or no raid in grace  │  or WillArrive == false
```

- **Gate (all must hold on a day tick):** `phase ∈ {None, Closed, Dissolved}`; `cooldown_days` since last arc; campaign day ≥
  `min_campaign_day`; Iron Raiders `aggression ≥ min_aggression` and `visibility ≥ min_visibility` (E13 — read only);
  `ArcsThisChapter < chapterProfile.maxSiegeArcs` (default 1; Year Two P1B must supply it — a cross-plan contract, §2b); and, if
  `requires_installation`, ≥ 1 established defense installation (so a siege has something to break).
- **Open:** compute `NoticeDays = clamp(round((base + jitter) × crisis_deadline_mult), clamp_min, clamp_max)` where `jitter =
  stream(day, besiegerHash, "notice").NextInt(jitter_min, jitter_max+1)` and `crisis_deadline_mult` is the read-site
  difficulty scalar (E19). Roll `WillArrive = stream(day, besiegerHash, "bluff").NextInt(1000) ≥ bluff_permille`.
- **Signs:** for each `[offset, tier]` in `sign_rhythm`, on day `WarningOpenedDay + round(offset × NoticeDays)` draw **one** Sign of that
  tier by seeded weighted draw, excluding already-shown ids and rows whose `requires` is unmet. **At most one Sign per day**
  (collisions push the later sign forward one day; the arrival sign is never pushed past `NoticeEndDay`).
- **Reveal:** the doctrine label (*Toll / Quiet Ring / Diggers*) becomes visible at `reveals_doctrine_at_tier`, or earlier if the
  player has completed a **Scout the Ridge** (§9.4). *Motive is never revealed* (LS-OM-1…6).
- **Arrival window:** after `NoticeEndDay`, up to `arrival_grace_days`. SY **does not force a raid.** It sets a read flag
  (`ArrivalWindowOpen`) that the Iron Raiders host reads when it next decides whether to raid; if it raids and the raid is
  repelled *and* LS's own rule (DEC-LS-01) allows a siege, LS starts the siege and SY moves to **Siege** on LS's
  `SiegeStarted` event. If the raid succeeds ⇒ **Dissolved** (ordinary raid consequences; no arc). If `WillArrive == false` or the grace
  window passes with no raid ⇒ **Dissolved** with the Bluff line ("The ridge is empty this morning.").

**Standalone mode (DEC-SY-03).** With no LS engine present, the arc still runs Notice → Gathering → Arrival; a repelled
Arrival raid opens a **Breaking-lite** (Wall and Names scars only, 14 days) instead of a Siege. SY therefore ships value *before*
LS ships, and upgrades to the full year when LS exists. `grep -r "class SiegeSystem"` is the P0 switch (E12).

### 9.3 Sign table (24 rows; doctrine dossiers are *public faces only*)

**Doctrine: Toll — "the Assessors."** Patient, exacting, polite. Their method is *counting*. Base notice 24 d; bluff 12%.

| # | Tier | Source | Text (content candidate) |
|---|---|---|---|
| T1 | Trace | road_traffic | Three carts came up the north road empty and went back full of nothing. The drivers asked what the gate charges. |
| T2 | Trace | market_prices | Salt, rope and lamp oil are up a tenth at Delta. Somebody is shopping for a long visit. |
| T3 | Presence | sentry_log | Two men walking the far ridge at an even pace, one behind the other, lips moving. Counting. |
| T4 | Presence | gate_visitors | A surveyor asks for a cup of water and, before he leaves, what the well yields per day. |
| T5 | Presence | rail_ledger *(requires `rail_running`)* | The depot clerk at Delta says a man asked what our trains weigh. He wrote the answer down twice. |
| T6 | Pressure | watch_acoustics | Pegs, driven at regular intervals. Not our ground and not our pace. |
| T7 | Pressure | radio_traffic | A polite voice on the open band reads a number, then a date, then a number. Nobody answers. |
| T8 | Arrival | sentry_log | A table on the ridge. A lamp. Two chairs. Nobody is sitting in them. |

**Doctrine: Quiet Ring — "the Wardens."** Their method is *silence*. Fewest signs, longest edge. Base notice 18 d; bluff 6%.

| # | Tier | Source | Text |
|---|---|---|---|
| Q1 | Trace | animals | The dogs stopped coming to the midden. Something is between us and the hills. |
| Q2 | Trace | sentry_log | Smoke on all four compass points. None of it is cooking. |
| Q3 | Trace | market_prices | The traders will not quote us today. "Not today," they say, and turn the cart. |
| Q4 | Presence | road_traffic | Six days and nobody on the road: not raiders, not tinkers, not the water-woman. |
| Q5 | Presence | watch_acoustics | Whistles answering whistles at distances that do not fit one party. |
| Q6 | Presence | gate_visitors | A man turned back at our gate. "I was told not to come this way." He did not say by whom. |
| Q7 | Pressure | radio_traffic | Our own frequency is being echoed back, one second late. |
| Q8 | Arrival | sentry_log | The ring closed at dawn: a person on every ridge, fifty paces apart, all facing in. |

**Doctrine: Diggers — "the Company."** Their method is *ground*. Most signs, longest notice, highest bluff. Base 30 d; bluff 20%.

| # | Tier | Source | Text |
|---|---|---|---|
| D1 | Trace | animals | The rats left the lower storeroom in a line, at noon. |
| D2 | Trace | watch_acoustics *(requires `deep_works_active` for the Deep Works flavour; else generic)* | A hum in the sump at odd hours, lower than the pumps. |
| D3 | Trace | weather_smoke | Fine yellow dust on the windward vents. Not our soil. |
| D4 | Presence | sentry_log | Survey stakes with coloured rags along the slope, in a line that ends at our wall. |
| D5 | Presence | gate_visitors | Two "prospectors" ask to buy our slag heap. They pay in good coin and do not haggle. |
| D6 | Presence | road_traffic | Ore carts on a road that never had ore. |
| D7 | Pressure | watch_acoustics | Tapping — three beats and a rest. Not ours. It stops when we stop. |
| D8 | Arrival | sentry_log | A headframe on the ridge. Nobody said mining. |

**Draw rules (per doctrine).** Each tier has ≥ 2 candidates so no-repeat draws never exhaust. Weights are data (1–5).
`requires`-gated rows drop out cleanly. A player with *no* Deep Works, *no* rail and *no* radio still gets a complete arc.

### 9.4 Readiness Board (read-only projection) and the small verbs

`SiegeYearReadiness.Project(IReadinessReaders readers, int day)` returns rows; **it writes nothing**. Each row carries the
value, the unit, the owner's name, and the list of *existing* commands that raise it (labels only — the panel's button *is*
the existing command, per CLAUDE.md UI rule).

| Row | Value | Owner read | "Raise by" (existing commands, labels) |
|---|---|---|---|
| Food days | pantry kcal ÷ daily consumption | kitchen/nutrition | stockpile, preserve, ration table (Ration Wars) |
| Water days | (storage + deep-well daily yield) ÷ consumption | water treatment, deep well, sump | treat, drill, pump |
| Fuel/power hours | stored kWh ÷ load | power grid | shed load, stock fuel |
| Medicine kits | kits ÷ (survivors ÷ 10) | medical inventory | craft, trade |
| Wire | fraction of installations armed/sound | perimeter defense | repair, reset |
| Watch fill | % of night-watch posts staffed | night watch / duty roster | assign, rota |
| Nerve | average morale and lowest-quartile morale | survivors | ceremony, vinyl, recreation |
| Rest | average `NeedKind.Fatigue` (there is no sleep-debt concept) | `NeedsSystem` | reduce rota |
| Runners | survivors qualified to carry word | expedition/skills | train, choose |
| Road | open supply routes known | trade routes / Living Region | scout, treaty |
| Rail *(only when IR active)* | corridor operational + Railhead km | `RailwaySystem` | restore, certify |

**Days-of-siege-we-can-meet** = `min(Food, Water, Fuel)`; the board shows it beside *"longest siege in the chronicle: N days"* (or
"unknown"). It never shows the besieger's patience, and never suggests a target — the number is a mirror, not a forecast.

**Two new verbs, both thin:**

- **Scout the Ridge** — dispatch one survivor on an ordinary expedition (existing dispatch); on return the *next* Sign is drawn
  from tier+1 and the doctrine label reveals. Cost: the survivor's day and the ordinary expedition risk. **It never reveals `WillArrive`.**
- **Set the Notice-board** — write one line to the shelter's public board (existing archive/board owner) that the shelter's
  **Nerve** row reads from: publishing the readiness figures raises Nerve *if* the figures are good, lowers it if they are bad. Honesty
  is a mechanic. (Consumes no resource; changes only what survivors are told.)

### 9.5 Breaking, Scars, Terms Year

On LS's `SiegeEnded(endingId)`, the arc snapshots `BrokenAtEnd` (installation ids) and enters **Breaking** for the ending's
`breaking_days` (data: Lifted 21, Relieved 30, Negotiated 30, Ground down 45, Fallen 60). During Breaking, the **Scar Ledger** is a
projection:

| Scar kind | Derived from | Resolves when | Example line |
|---|---|---|---|
| Wall | each id in `BrokenAtEnd` still broken/damaged in `DefenseSystem` | the owner shows it repaired | "North gate: still one plank short." |
| Names | Memorial owner: deaths with day ∈ [`SiegeStartDay`, `SiegeEndDay`] | never — this is the shelter's wall | "Eleven names, cut in order of the day." |
| Stores | now-vs-`PreSiege*` days (owner now / snapshot) | now ≥ 90% of snapshot | "Pantry: 41% of what we had. We are counting in halves." |
| Nerve | `MoraleTroughPermille` band | Breaking ends | "The worst week: nobody said it aloud." |
| Terms | `TermsId`, `TermsEndDay` | `TermsEndDay` reached | "Terms: eleven months. The date is written on the wall." |

- **Terms Year** exists only after *Negotiated*. Rows in `siege_year_terms.json` describe obligations (`kind`, `amount`, `per_days`,
  `duration_days`); the *enforcement* is LS-P6's route through the treaty/debt owners. SY **displays** the expiry and writes a
  chronicle line 30 days before it. It owns no enforcement (DEC-SY-06).
- **The Year Entry** (a single chronicle write at end of Breaking) is assembled from data: doctrine label (if revealed), notice length,
  siege length, the ending's name, count and names from the Memorial owner, Nerve band (never the number), Stores band, Terms date.

**Row shapes.**

```json
{ "scar_id": "scar_wall_broken", "kind": "Wall", "severity_bands": [0.0, 0.34, 0.67, 1.0],
  "lines": ["One plank short.", "Half a gate.", "No gate."] }
{ "terms_id": "terms_toll_eleven_months", "doctrine_id": "doctrine_toll", "duration_days": 330,
  "obligations": [ { "kind": "tribute_goods", "item_class": "salt", "amount": 12, "per_days": 30 },
                   { "kind": "road_access",   "route": "north_road", "per_days": 0 } ] }
```

### 9.6 Authored voice banks

**Notice-board lines (Set the Notice-board, `siege_year_lines.json`).**

- Good figures: *"Water for forty days at present use. Food for thirty-one. The board will be updated at noon."*
- Bad figures: *"Water for nineteen days. Food for eleven. We are not going to say it will be fine."*
- Mixed: *"We have more water than food. We are going to talk about that."*

**Sentry-log stubs.** *"Third morning. Same two men. They wave now."* · *"Do not shoot the man with the notebook. Write down what the notebook says."* · *"The smoke does not thin out. Someone is feeding it."*

**Bluff (Dissolved by grace expiry).** *"Day thirty-one. The ridge is empty. I have been told this is good news. I would like whoever told me to stand watch on an empty ridge for a month."*

**Breaking — five ending lines (chronicle stubs).**

- **Lifted:** *"They took their table down at dusk and left the chairs."*
- **Relieved:** *"The whistle came from the east before the light did."*
- **Negotiated:** *"Terms: eleven months. I have written the date on the wall so nobody can say they were not told."*
- **Ground down:** *"Nobody won. Both sides are quieter."*
- **Fallen:** *"We are still counting who is left."* (wording reviewed with the tone owner: no gloating, no dwelling)

---

## 9b. A Year in Twelve Entries (prose texture — not authority)

*Written as the shelter's chronicle might render a Toll arc. Warning opened day 200, notice 27 days (sign days 201, 205, 211, 216, 220, 224, 226 from the §9.2 rhythm at offsets ×27), Arrival 227, siege 227–239, Breaking to 269, Terms 330 days from 239.*

**Day 201 — Something is being priced.** Warning opened yesterday; the first Sign lands today. Salt is
up a tenth at Delta. Rope too. Lamp oil, which nobody has ever thought about, is a third dearer than
in spring. The quartermaster says merchants don't raise the price of oil unless someone has bought a
great deal of it. Nobody is worried yet. We are *noticing*.

**Day 205 — Three empty carts.** They came up the north road empty and went back full of nothing. The
drivers asked what the gate charges. The boy at the gate — too honest, too young — told them.

**Day 211 — A surveyor asks for water.** He is courteous. Before he leaves he asks what the well
yields per day. The boy is not there; the watch captain gives him a number that is not the number.
The captain will not mention it to anyone, which is how we know she is worried.

**Day 213 — The board goes up.** Water for thirty-eight days at current use. Food for twenty-four.
We wrote it plain because somebody once told the quartermaster that fear is a kind of ignorance.
By dusk the shelter's Nerve has moved the way a held breath moves.

**Day 216 — Two men on the ridge.** They walk at an even pace, one behind the other, lips moving.
The watch captain says they are counting. I say counting what. She says: *steps*.

**Day 220 — Pegs.** Driven in the dark, at regular intervals, along a line that ends at our north
wall. Whoever is driving them is not in a hurry, and that is the message.

**Day 224 — A voice on the open band.** Reads a number, then a date, then another number. Then
silence. Nobody on our side wants to be the one who writes the date down. The Keeper writes the
date down.

**Day 226 — A table on the ridge.** A lamp. Two chairs. Nobody sits. The boy at the gate asks whether
this means they are polite. The watch captain says it means they can wait.

**Day 227 — The Arrival.** A raid comes at dusk and is repelled. By the rota's arithmetic it should
have cost us more than it did. The quartermaster says that is what an assessor *wants* you to think
before a bill.

**Day 227–239 — Twelve days.** One pressure a day, from the daily board: Starve, Noise, Probe, Parley.
This entry is intentionally not written out; the LS plan owns those days. What SY owns is the *page
count*: twelve pages, each ending in a number.

**Day 239 — Negotiated.** Terms: eleven months. Salt, twelve measures every thirty days. The north
road open to their carts. The date is written on the wall so nobody can say they were not told.

**Day 240 — The Names.** Eleven, cut in order of the day. The Keeper reads them at the evening
meeting without adjectives. The north gate is still one plank short. The pantry stands at forty-one
percent of what it was. We are counting in halves.

**Day 269 — The Year Entry.** *Twenty-seven days of warning. Twelve days of siege. Eleven names.
Terms until day 569. The watch captain asked to be recorded as having said: 'I wish we had believed
the two men.'* The chronicle does not say what she wished for the boy at the gate. The chronicle
keeps its own counsel.


---

# PART THREE — THE SEAM, THE WORK AND THE RECORD

## 10. Cross-plan boundaries and hooks

Every hook below **ships dark** (a `Null*` implementation, no visible surface) until both ends exist, per the
LS-P8 pattern. Each is a no-op when the other plan is absent, and has exactly one test shape.

| Plan / owner | Boundary | Hook (if any) |
|---|---|---|
| **Exp. 25 The Iron Road** (design bible) | This plan is its *first implementation spine*. Cast, 15 beats, 8 choices and endings are Exp. 25's and unchanged. Its proposed `TrackMaintenanceSystem` is **superseded** by the live ledger (E4); `BridgeSystem`, `LocomotiveSystem`, `RailScheduleSystem`, `RailTownSystem`, `RailInterdictionSystem`, `GaugeSystem`, `RailCommonsSystem` remain *unstarted* proposals. | none |
| **Exp. 93 A Town on the Siding** | Iron Siding (`loc_settlement_iron_siding`) is a prose-only place; the corridor may *name* it as a depot but the plan never invents history for it (Exp. 93 §4). | none |
| **The Long Line: Freight (LF)** | LF owns trade-route legs and the company rungs. IR adds *availability only*: `RailwaySystem.GetOperationalRailCorridor` already feeds `TradeRouteSystem` (E-comment in code); restoration makes it return non-null. **LF must never read the overlay** — it reads the owner. | none |
| **The Long Siege (LS)** | SY wraps it. LS's five pressures, five endings, doctrines and resolver call cadence are unchanged. SY reads `SiegeStarted` / `SiegeEnded` through a public event or read API (LS-P1/P6); if absent, SY runs in Standalone mode (§9.2). | X1, X2 |
| **The Ration Wars (RW)** | The Readiness Board's Food row links to the Table Rule; SY never changes it. | none |
| **The Deep Works (DW)** | A Diggers Sign may cite a real Deep Works drift; SY never writes to it. | Sign `requires: deep_works_active` |
| **The Quiet War (QW)** | SY's Set-the-Notice-board is honest by construction; a QW informant may falsify a Sign *reading* — QW's business, through its own seam. | none |
| **The Plague Year (PY)** | A sealed gate during Notice is a PY Gate Protocol; SY never forces it. | none |
| **The Living Region (LR)** | Road-cut flags read by SY's Road row; IR corridor status reads into LR's region "rail" vocabulary only if LR ships one. | none |
| **Crews and Companions (CC)** | Survey crews and the track gang are ordinary parties/duty posts once CC exists. | none |
| **Year Two** | Chapter Profiles gate both arcs (`maxSiegeArcs`, `railCampaignEnabled`). Outposts are Year Two's; neither part reads them. | activation gate only |
| **Shelter Governance (SG)** | Terms Year obligations and Right-of-Way rulings are Assembly business *if present*. | none |
| **The Record Keepers (RK)** | The Line Book and Year Entry are chronicle records; custody, loss and copying belong to RK. | none |
| **Pair 2: Convoy Wars** | Armoured battle trains (the 250 t loco class) and interdiction belong there; IR only exposes rungs and the Cut. | X1 |
| **Pair 4: Memory Work** | SY's Names scar is *read from the Memorial owner*, not authored here. | none |
| **Pair 5: The Treaty Table** | Right of Way (who may run trains where), tolls and Terms enforcement route through treaty/embargo/debt owners. | none (IR/SY only *display*) |

**X1 — Rail Cut.** `IRailCutSink.TryCut(segmentId, reasonKey)`; IR's implementation calls
`RailwaySystem.ApplyCampaignFootprint(segmentId, sabotaged:true)` (may only *lower* condition — E2 then refuses dispatch natively) and
writes a Line Book entry. LS calls it from a doctrine's Starve/Sap pressure (data: `cuts_segment_ids` per doctrine — a doctrine may cut
*nothing*). `NullRailCutSink` is the default. **A cut is cleared exactly like a Sabotage fault** (§6.2.1).

**X2 — Relief Carrier.** `IReliefCarrier.CanReach(originNodeId, shelterNodeId)` returns `bool` and a `days` integer read from
`GetOperationalRailCorridor` + segment `distance_km` at the corridor's Running speed. LS's *Relieved* ending asks it; LS alone decides
what a *faster* relief means (a data number on its own side). IR never changes a siege.

**X3 — Rail sign.** Sign rows with `requires: rail_running` are excluded unless ≥ 1 segment is at rung 5 (§6.1).

## 11. Acceptance criteria

**Iron Road (IR)**

1. **Authority agreement (CLAUDE.md "integrated").** Core authority + host owner + event path + persistence + observable outcome agree for
   each verb (Survey, Work fault, Load test, Certify) and for the observed run.
2. **Parity.** With the campaign inactive (`Restoration == null`), a saved corpus's rail behaviour — dispatch results, derailment
   outcomes, ledger census, `IsRailCorridorOperational` — is byte-identical to today.
3. **Native refusal.** Every blocking Fault refuses dispatch through the *existing* traversal rule (E2) with no new traversal code; clearing
   the Fault (through the owner's verb) restores traversability in the same tick.
4. **Footprints only lower.** `ApplyCampaignFootprint` on either owner never raises integrity/bridge/gauge and is idempotent (second call is a no-op).
5. **Derivation purity.** `RailRestorationRungs.Derive` is a pure function of (owner readings, overlay, day): same inputs ⇒ same rung; no writes; rungs fall without overlay writes.
6. **Load test determinism.** Same seed/day/segment/consist ⇒ same outcome; never derails a real train; wear applied exactly once.
7. **Fingerprint honesty.** Certify fails after any change to integrity, bridge, wear or gauge since the passing test.
8. **Stale rules.** Each of the four triggers (`worn`, `bridge`, `storm`, `orphaned`) has a test; an orphaned certificate lowers the rung **without** changing any physical value.
9. **Save round-trip mid-restoration** with open progress on two faults, one stale certificate and one first-train stamp; older saves load neutral.
10. **No second authority.** A repository search confirms no new counter for wear, gauge, bridge, fuel, mass or dispatch; the overlay contains only human-act fields (§5.1).

**Siege Year (SY)**

11. **Raid parity.** With no doctrine warning rows or with the gate unarmed, Iron Raiders raids resolve identically on a saved corpus; LS's "at most one resolver call per siege day" still holds.
12. **No forced raid.** Arrival never triggers a raid; the raid owner's own decision is the only trigger (test asserts `ExecuteRaid` is not called by SY).
13. **Notice determinism.** Same seed + same difficulty scalar ⇒ same `NoticeDays`, same `WillArrive`, same Sign schedule (ids and days).
14. **≤ 1 Sign/day; no-repeat; unmet `requires` excluded.** Property test over 200 seeds: never two Signs in one day, never a repeated id, never a `requires`-gated row without its precondition.
15. **Readiness purity.** `Project` writes nothing (owner fakes assert zero mutations) and equals the owners' values in a scripted week.
16. **Dissolve paths.** Not-repelled raid, grace-window expiry and `WillArrive == false` each reach `Dissolved` with the correct chronicle line and **no** Scar Ledger.
17. **Breaking derivation.** Wall/Names/Stores/Nerve scars equal owner reads + the four snapshots on a scripted siege; a repaired installation drops its Wall scar the same day.
18. **Standalone mode.** With no LS engine, an arc reaches Breaking-lite (14 days, Wall + Names only) and never touches `settlement_defenses` outside its nested DTO.
19. **Save round-trip** in Notice, Gathering, Arrival, Siege and Breaking; older saves load as `Phase.None`.
20. **Snapshot honesty.** A test proves the three snapshot ints are *never read as authority* (mutating them does not change any owner value or any owner-derived readout).

**Seam (X)**

21. Each of X1/X2/X3 is a no-op when the other end is absent, and does exactly one specified thing when both exist; no hook reads the other plan's state.

## 12. Packages

Each package is a **derived plan** when executed (`.ai/plans/<pkg>-<date>.md`, copied from `template.md`, approved by the user), per the Year Two pattern. Roles are CLAUDE.md roles: **Auditor** (read-only), **Coder**, **Tester**, **Integrator**.

### IR packages

| Pkg | Role | Scope | Accept |
|---|---|---|---|
| **IR-P0** | Auditor | *(Pass 1, §2b, already closed the panel, interlock, Fatigue, storm, post, chronicle and CLI-naming items; what remains is the Chapter Profile field contract with Year Two P1B and the labour-API choice.)* Close: two-`TrackSegmentState` naming collision (which is authoritative for what); duty-roster post `post_track_gang` and labour API; survivor skill band for Survey; the storm/flood event name for the `storm` stale trigger; interlock reservation API for the Meet; chronicle writer (E16); Chapter Profile flags; whether a UI panel renders `RailwaySystem` today; confirm no live claim on `Rail/`, `Expeditions/RailwaySystem.cs`, `Main.RailTrackMaintenance.cs`. | each VERIFY answered with `path:line` or a test; decisions DEC-IR-02/03/09 confirmed or re-chosen |
| **IR-P1** | Coder | `RailFaultCatalog`, five catalogs (§8.1), loader, validator rules (§8.1 list), scanner registration. | validator passes; every failing rule has one test; `bin/validate-config` green |
| **IR-P2** | Coder | Pure `RailRestorationRungs.Derive`, `Railhead`, `LineStanding`; `RailRestorationState` DTO (nested, additive). | derivation tests; round-trip; legacy save neutral |
| **IR-P3** | Integrator | `RailwaySystem.ApplyCampaignFootprint` and `RailTrackMaintenanceLedger.ApplyCampaignFootprint` (additive, idempotent, lower-only); host activation at chapter start applies each Fault once. | parity (footprint never applied ⇒ identical); idempotence; lower-only property test |
| **IR-P4** | Coder | Verbs: Survey, Work fault (labour accrual, native/ledger/work_order clearing), Load test, Certify. | acceptance 3, 6, 7 |
| **IR-P5** | Coder + Integrator | Observe `OnTrainDispatched`/arrival to stamp runs; daily stale checks; phase-5 day owner; `rail_restoration_ticked` event. | acceptance 8; day-owner replay determinism |
| **IR-P6** | Coder | Milestones, Line Book (`StableHash` variant pick), chronicle pointer writes. | each milestone predicate has one test; entries deterministic |
| **IR-P7** | Coder | Line Book strip on the existing rail/route surface; focus/back preserved; presenter tests. | panel holds no authority |
| **IR-P8** | Coder | X1/X2/X3 hooks, dark. | acceptance 21 |
| **IR-P9** | Story Director | Content waves W1–W4 (§16) + governance close (ledger, debt, plan header, move to integrated). | handoff per `AI_AGENT_WORKFLOW.md` |

### SY packages

| Pkg | Role | Scope | Accept |
|---|---|---|---|
| **SY-P0** | Auditor | Close E12 (LS status: does `SiegeSystem` exist yet?), Iron Raiders read API (aggression/visibility ranges), `crisis_deadline_mult` read site, survivor/morale/sleep read ports, installation-broken list API on `DefenseSystem`, Memorial owner death-day query, chronicle writer, notice-board owner, Chapter Profile `maxSiegeArcs`. Decide Standalone vs Full mode. | each answered; mode chosen |
| **SY-P1** | Coder | `SiegeYearState` DTO, nested in the LS `siege` DTO (Full) or in `settlement_defenses` alongside it (Standalone). | round-trip; older saves neutral |
| **SY-P2** | Coder | Catalogs (§8.2), loader, validator (rhythm monotone, ≥ 2 candidates/tier, doctrine ids, `requires` enum). | validator green |
| **SY-P3** | Coder | Gate, open, Signs (seeded), phase machine, dissolve paths. | acceptance 12, 13, 14, 16 |
| **SY-P4** | Coder | Readiness projection (ports), Scout the Ridge, Set the Notice-board. | acceptance 15 |
| **SY-P5** | Coder | Breaking: snapshots, Scar derivation, Terms display, Year Entry. | acceptance 17, 20 |
| **SY-P6** | Integrator | Host: daily Sign tick in `Main.Defense.Integration.cs`; arrival-window flag read in `Main.Muster.cs`; day-owner registration. | acceptance 11 parity |
| **SY-P7** | Coder | Warning/Year strip on `DefenseGridPanel` and `NightWatchPanel`. | presenter tests; focus/back preserved |
| **SY-P8** | Coder | Standalone-mode Breaking-lite; hooks X1/X3 consumer side. | acceptance 18, 21 |
| **SY-P9** | Story Director | Content waves + governance close. | handoff |

**Execution order.** IR-P0 ∥ SY-P0 → IR-P1..P6 (linear) ∥ SY-P1..P6 (linear) → IR-P7/P8, SY-P7/P8 → P9. **Neither part waits on the
other.** SY waits on LS only for *Full* mode (Standalone ships first).

## 13. Decision register (proposals — unsigned)

| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-IR-01 | Restoration state is human acts only; rungs, Railhead and corridor status are derived. | architecture | Yes |
| DEC-IR-02 | Overlay nests in `RailMaintenanceState.Restoration`; no new save section. | architecture | Yes; confirm vs checksum in P0 |
| DEC-IR-03 | One additive `ApplyCampaignFootprint` per owner (idempotent, lower-only) instead of a dispatch provider hook. | architecture | Yes; `INT` |
| DEC-IR-04 | Over-certificate dispatch is advisory only; never refused. | rule | Yes |
| DEC-IR-05 | Faults are authored in `rail_faults.json`; `rail_network.json` `base_integrity` is not edited. | compatibility | Yes |
| DEC-IR-06 | Load tests never derail a real train; they cost wear and may return a near-miss (90% certificate). | rule | Yes |
| DEC-IR-07 | One crew per fault per day; labour applies in stable fault-id order. | determinism | Yes |
| DEC-IR-08 | No new routed panel; Line Book strip on the existing rail/route surface. | UI | Yes |
| DEC-IR-09 | History lists capped at 32 rows/segment with fold-into-count eviction. | tuning | Yes |
| DEC-IR-10 | Exp. 25 endings are *suggested* by Line Standing, never selected by this plan. | boundary | Yes |
| DEC-IR-11 | One additive `duty_roles.json` row `track_gang` (six roles ship; none is a track post); until then `expedition` crews are the labour source. | data | Yes |
| DEC-IR-12 | Fixed-sentence Line Book entries use `TryRecordChronicleEntry` (summary key); composed lines use `JournalSystem.TryAddRawEntry`. | seam | Yes |
| DEC-IR-13 | The `storm` stale trigger is a daily read of heavy `WeatherKind`s, not an event subscription; weather never lowers rail integrity. | rule | Yes |
| DEC-SY-01 | Siege Year adds no combat, no resolver call, no second siege state. | rule | Yes |
| DEC-SY-02 | State nests inside the LS `siege` DTO in `settlement_defenses`; no new section. | architecture | Yes; confirm in P0 |
| DEC-SY-03 | Standalone mode ships before LS (Breaking-lite); Full mode upgrades it. | scope | Yes |
| DEC-SY-04 | At most one arc per storyline chapter (`maxSiegeArcs`), with a 120-day cooldown. | design | Yes |
| DEC-SY-05 | Arrival never forces a raid; the raid owner's own decision is the only trigger. | rule | Yes |
| DEC-SY-06 | Terms Year is *displayed* by SY, *enforced* by the treaty/debt owners via LS-P6. | boundary | Yes |
| DEC-SY-07 | No new routed panel; extend `DefenseGridPanel` and `NightWatchPanel`. | UI | Yes |
| DEC-SY-08 | Signs are announced projections; `reading` is shown only after the arc ends. | design | Yes |
| DEC-SY-09 | Bluff (`WillArrive == false`) is seeded once and never revealed by any verb. | design | Yes |
| DEC-SY-10 | Difficulty scales notice length via the read-site scalar only, never the resolver. | rule | Yes; depends on E19 |

## 14. Test plan (focused; **no full suite** without `RUN FULL TESTS`)

Run via `bin/run-scoped-tests` against the changed modules only; ≤ 10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`.
Existing equivalent tests are checked **first** (`Rail/RailTrackMaintenanceLedgerTests.cs`, `Rail/RailTrackMaintenanceEngineTests.cs`,
`Expeditions/RailwaySystemTests.cs`, `RailwayInterlockEngineTests.cs`, defense/raid tests) and extended rather than duplicated.

| Group | Cases (target 3–10 per behaviour) | Trait |
|---|---|---|
| Rung derivation | (a) each rung boundary, table-driven over a static row set; (b) rung falls after external `isSabotaged`; (c) Railhead ladder for the shipped topology; (d) Railhead ties break ordinal; (e) purity (no writes, owner fakes) | fast |
| Footprints | lower-only property test (200 random pre-states); idempotent second call; parity when never applied | fast |
| Verbs | Survey completeness by skill band; Work fault accrues across days; `native` verb `Blocked` holds labour; ledger/`work_order` clearing; validator rejects overlapping `work_order` | fast |
| Load test | determinism; refusal cases; near-miss 90%; wear applied once; fingerprint mismatch on Certify | fast |
| Stale | four triggers; orphaned changes no physical value; re-certify by another certifier | fast |
| Save | round-trip mid-restoration; legacy neutral; unknown segment/fault ids | integration |
| Catalog | validator rules (aggregated static mapping with per-row output) | fast |
| SY gate/notice | notice range at each difficulty scalar; `WillArrive` frequency over 2,000 seeds ≈ bluff rate ± 3 pts | fast |
| SY Signs | ≤ 1/day, no repeat, `requires` exclusion (property over 200 seeds) | fast |
| SY phases | dissolve paths ×3; Arrival never forces a raid; Standalone Breaking-lite | fast |
| SY readiness | purity; equals owners in a scripted week | fast |
| SY breaking | scar derivation; scar expiry on repair; snapshot honesty | fast |
| Save (SY) | round-trip in each phase; legacy neutral | integration |
| Replay | paired run with an interrupted save mid-Notice ⇒ identical Sign schedule and Year Entry hash | integration |
| Hooks | X1/X2/X3 dark no-op + live one-effect test each | fast |

Host probes (bounded, headless, 15 FPS): extend `--rail-track-maintenance-selftest` (existing, 12/12) and add one
`--siege-year-selftest` (kebab-case, `-selftest` suffix, one `HostCliRegistry.cs` row — pattern verified §2b).

## 15. Risk register

| # | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R1 | Two segment-state types (`Rail.TrackSegmentState` vs `Expeditions.TrackSegmentState`) are conflated. | med | high | P0 names each authoritative surface; every reading in §6.1 cites the type; test per boundary |
| R2 | Footprints applied to an in-progress save brick the yard. | med | high | footprints run only at chapter activation on a *fresh* activation flag; never on load; parity test; kill-switch flag in the chapter profile |
| R3 | Survey incompleteness feels like a cheat. | med | med | UI always shows *"N unknown"*; a second survey is cheap; the rule is announced in the codex up front |
| R4 | Players spam load tests. | low | low | tests cost wear (E5); fingerprint tests; no free retries |
| R5 | Notice is too generous (cheap prep) or too short (unfair). | med | med | difficulty scalar at read site; ranges published in the codex; tuning is data |
| R6 | SY Standalone and Full drift apart. | med | med | one state DTO; Full adds nested fields only; shared test fixtures |
| R7 | LS is never implemented. | med | med | Standalone mode has independent value (DEC-SY-03) |
| R8 | The line grows into a second economy (rail towns). | med | high | Non-Goals; no schedule/town code here; Exp. 25 §7.5 explicitly out of scope |
| R9 | A doctrine's `cuts_segment_ids` makes rail unusable for the whole siege. | med | med | cuts clear like Sabotage; a doctrine may cut **at most one** segment; validator rule |
| R10 | Names scar handled without care. | low | high | tone review; reads the Memorial owner only; never authored per-name; Pair 4 owns vocabulary |

## 16. Expansion backlog — *to be expanded and finalised*

These are the intentionally unfinished threads; the next pass should promote or discard each, one at a time.

**Iron Road**

- [ ] **Rail towns.** Kilometre Nine, Depot Town, Iron Siding: what each *owes* the corridor (schedule slots, water, wages). Exp. 25 §7.5 + Exp. 93; needs `RailTownSystem` decision.
- [ ] **More geography.** Only five nodes exist. A second ring (a tunnel, a river span, a wreck line) is authored in Exp. 25 §4.2 — each new node needs `rail_network.json` + logistics + interlock rows and a **Fault set** in `rail_faults.json`.
- [ ] **Side quests (30)** in Exp. 25 §6.2: assign each to a Fault, a rung or a milestone so none is orphaned.
- [ ] **Armoured train content** and interdiction (`RobberyAmbush` status is live; the toll is not) — Convoy Wars.
- [ ] **Gang as people.** Named gang members with development traits (Development Traits owner) and injuries; needs CC.
- [ ] **A survey that changes the map.** Fog-of-line on the region panel until surveyed.
- [ ] **Signal/interlock faults** beyond `work_order`.
- [ ] **Storm event owner** for the `storm` stale trigger (P0 names it).
- [ ] **Line Standing → endings wiring** with Exp. 25's ending selector (DEC-IR-10).

**Siege Year**

- [ ] **More Signs.** 24 → 48 (add seasonal variants and a second Toll voice). Weight tuning.
- [ ] **A fourth doctrine** (the LS plan owns doctrines; SY needs one warning row each).
- [ ] **Faction-specific arcs.** Do the Iron Garrison and the Ash Militia (faction lore §05) have their own *Signs*?
- [ ] **Children in the year.** How do school, play and nightmares show in Nerve? (Needs Memory Work + Education.)
- [ ] **A besieger's Year Entry** — what the *other* side's chronicle would say, never shown.
- [ ] **Scars that become buildings.** The Wall scar as a room in the shelter museum (Culture/`ShelterMuseumSystem`).
- [ ] **Terms Year enforcement wording** with the Treaty Table.

**Shared**

- [ ] The relief run: an authored, one-off *"first relief train"* beat when X2 is live and a corridor is Running during a Siege.
- [ ] Achievements for both parts (existing `achievements.json` owner).
- [ ] Localization keys for all authored lines (existing Localization owner; string freeze D22 blocks).

## 17. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These are **intentionally unanswered** — not gaps, not TODOs, not deferred work. They keep the two subjects larger than the boards that
meter them. Any future plan that answers one must name the signed decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| IR-OM-1 | Who laid the line, and why does a spur run to a silo? | Exp. 25 and `rail_network.json` give topology and no history; the inspector's slips imply a plan and stop. | Never — texture by omission. |
| IR-OM-2 | Whose initials were on the first certificate, before the shelter's? | Bridge slips carry old paint; a Survey at difficulty 5 may read a stamp. It must remain unattributed. | Never. |
| IR-OM-3 | What does "Certified" certify? | The load test proves a number, not safety; the trestle still fails in a storm (Exp. 25 beat 12). That gap is the design. | Never — a rule, not a gap. |
| IR-OM-4 | Why did the raiders lift the mainline rail? | A Fault is an *effect*. The authored text says who did it (a barricade), not why. | Convoy Wars / The Underworld, jointly, if signed. |
| IR-OM-5 | Is the ring (spur + bypass) intentional? | The corridor's geometry reads like a design; nobody within the fiction can confirm. | The Long Inquest (Pair 6), at its discretion. |
| IR-OM-6 | What does Pim see on the survey walk? | A child walked with the surveyor in the prose (§7b). The game writes no line for what Pim saw. | Never. |
| SY-OM-1 | Why did they let us see the signs? | Notice is seeded, not authored per sign; whether the besieger *wanted* to be seen is not modelled (DEC-SY-08). | Never — a rule. |
| SY-OM-2 | Was the bluff a bluff? | `WillArrive == false` is a coin; the *reason* is never given (DEC-SY-09). | Never. |
| SY-OM-3 | Why are the Assessors polite? | *Toll* is a doctrine name (DEC-LS-08); its manner is a doctrine's data, not an explanation. | Never — locked by LS decision. |
| SY-OM-4 | Who put the two chairs on the ridge? | The Arrival sign is an authored *image*, not a fact with an author. | Never. |
| SY-OM-5 | Whose voice reads the number on the open band? | A Sign is a radio texture; the Radio Free plan may (with a signed decision) let a player's station overhear it, but the *speaker* stays unnamed. | Radio Free Ashfall, at its discretion. |
| SY-OM-6 | What did the Names cost the ones who read them? | The Keeper reads without adjectives (§9b). What that costs the Keeper is not modelled. | Memory Work (Pair 4), at its discretion. |

## 18. Pre-flight, verification and stop conditions

**Pre-flight (before *any* package starts):**

- [ ] Read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `AI_AGENT_WORKFLOW.md`, `.ai/state.md` in order (CLAUDE.md).
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`).
- [ ] No equivalent existing system found (search: `Restoration`, `Railhead`, `Certif`, `WarningOrder`, `SiegeYear`, `Sign`).
- [ ] Premises re-verified (Rule 7); **E6 and E8 re-checked with a one-line test** (`SeedFromTopology` ignoring `base_integrity`; only the trestle blocked today).
- [ ] Signed decisions in hand for the package's DEC rows; no overlapping live claim on `Rail/`, `Expeditions/RailwaySystem.cs`, `Defense/`, `Main.Muster.cs`, `Main.Defense.Integration.cs`.
- [ ] Plan status is `APPROVED BY USER` **before any code commit** (Rule 8).

**Verification:**

- [ ] `bin/run-scoped-tests` on the new test classes and the *existing* rail, interlock, defense, muster and Iron Raiders tests (list from the P0 selector); each < 30 s.
- [ ] `--rail-track-maintenance-selftest` (existing, expect 12/12 unchanged when inactive) and `--siege-year-selftest` (new; naming pattern verified).
- [ ] `--data-integrity-selftest` and the port-contract selftest pass; save-registry pin unchanged (no new section).
- [ ] `.ai/state.md` updated per package; handoff in `AI_AGENT_WORKFLOW.md` format listing untouched shared paths.

**Stop conditions (Rule 10):** stop and report to the foreman/user if **(a)** any restoration act would need dispatch, pathfinding or traversal logic beyond `ApplyCampaignFootprint`; **(b)** the overlay cannot nest without a new save section; **(c)** LS's event/read API is missing *and* Standalone mode cannot satisfy acceptance 18; **(d)** a Sign or Scar would require inventing a besieger's motive; **(e)** any package overlaps a live claim; **(f)** `SeedFromTopology`'s behaviour (E6) has *already* been changed by another plan (then re-derive §7.1 from the new baseline); **(g)** the Names scar cannot be read without authoring per-person text.

**Termination criteria ("done" for each package):** the package's acceptance rows pass via scoped tests; parity rows still pass; the derived plan file is marked `FULLY INTEGRATED` (multiple times, at the top) **and moved to** `.ai/plans/integrated/<category>/` immediately (CLAUDE.md §8); nothing else.
