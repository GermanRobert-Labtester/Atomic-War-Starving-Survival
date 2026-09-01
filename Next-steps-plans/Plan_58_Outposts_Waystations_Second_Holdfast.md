# Plan 58 — The Continuation: Outposts, Waystations, and a Second Holdfast

> **Wave:** Continuity Wave 9 — *Weight, Durability & the Shop Window*
> **Depends on (hard, all of them):** 32A/32B/32C (the graph), 20A/23A (dose and power as place
> facts), 24A/24B (fitness + needs), 30A/30C (an autonomous world), 33 (intel channels), 35
> (production rails), 38A/38C (calendar + commitments), 41C/43C/44A (generations, consent, relations),
> 34C (cross-campaign record), **53C (intake approval — this plan must pass it like any other)**.
> **Coordination:** the parallel `Plan_160_Expedition_Colony_Outpost_System` designs this feature's
> *content*; this plan defines the state model and rails so 160 doesn't create a second campaign
> authority. Nine waves of evidence say the danger is not scope — it is a second source of truth.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | Forward bases already exist as a system | `Assets/Ashfall.Core/WaystationSystem.cs` is wired into the host: `src/Host/ExpansionHostSession.cs:23,41,49` (property, ctor param, `?? new WaystationSystem()`), `:103` construction, `:219` reports `$"bunks {Waystation.State.bunksOccupied}/{WaystationSystem.MaxBunks}"` — **capacity semantics already modelled** |
| 2 | A routed surface exists for it | `R("waystation_network", …)` in `PanelRegistryBootstrap.cs`; Wave 1's 30-fake-console list (BUG-UI-002) does **not** include it — the surface is closer to real than most |
| 3 | Expedition camps already do multi-day field life | `ExpeditionSystem` camp events (`OnCampEntered`, `OnCampNightSegmentResolved`, `OnCampEncounterSurfaced/Resolved`, `OnCampDawnResolved` — enumerated in `SILENCE_AUDIT.md` §4.3) + `DiveInstanceRunner`, `ExpeditionVehicleSystem`, waystation resupply noted in Wave 4's 32B step 7 |
| 4 | The rails that a continuation needs were the *subject* of Waves 4–6 | graph + routes (32A/32B), knowledge/reveal (32C), positional dose (20A), power as dependency (23A), fitness/labour (24A/35C), commitments (38C), generations (41C), consent/leadership (43), relations (44), legacy record (34C) |
| 5 | Population instruments exist to bind | `CensusClaimSystem.cs`, `VoluntaryRegisterSystem.cs` (both live inside `DoseLedgerHostSession`), `CohortSystem` (41C), `GenerationalSuccessionEngine` (chapters/years) |
| 6 | The fiction constrains expansion, and the design already noticed | `docs/expansions/expansion_the_holdfast_plan.md`: the recommended concept is the one that "respects the **closed Sector 4 map**" — so a wider world must be justified as *reachability*, not as map size (and 32A's honest node count is 6) |
| 7 | Save architecture is slot-scoped, not site-scoped | `SaveSlotRoot.ResolveBaseDirectory` routes per slot (`SaveStoreHub.cs:17,51,76`); a second holdfast has no ownership model yet — the exact place a second authority gets invented by accident |
| 8 | Retention pressure arrives with it | Wave 9's 55A: per-site logs, memorials, census rows multiply every unbounded collection — the continuation is the reason retention policy must exist first |
| 9 | Intake policy is live before this plan | 53A's register + 53C's gate; this plan's `RAILS_REQUIRED` field is the checklist above, and its "day-2 relevance" answer is the slice's day-8 |
| 10 | The registry promised the direction, not the system | `ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` §27: "Forward Waystations & Caravans → Build trading post fortification quests" — the ask is quests on existing rails, not a new domain |

---

## Task 58A — Decide what a second settlement *is*, before writing a line of it

**Goal:** one ADR that makes the continuation a projection of existing authorities, and a scope
decision the fiction can defend.

**Files:** new `docs/design/ADR-0001-outposts.md`, `docs/design/PILLARS.md` (53B),
`Assets/Ashfall.Core/WaystationSystem.cs` (read), `ExpeditionSystem.cs` (camp model),
`WastelandMapSystem.cs` (32A), `SaveSlotRoot.cs` / `SaveStoreHub.cs` / `SaveSectionRegistry.cs`,
`Region*/Faction*` catalogs, `docs/roadmap/RAILS.md`, `docs/roadmap/INTAKE.md` (53C form),
`Next-steps-plans/Plan_160_…` (cross-reference).

### Substeps

1. **File the intake form first** (53C): pillars touched, category `SYSTEM` (the highest bar),
   `RAILS_REQUIRED` with each plan's status, the duplicate search (waystations, camps, outposts,
   colonies, expeditions — and the answer to "why not just the graph + camps?"), day-2 relevance,
   and the metric it moves.
2. **Answer the hard question in writing**: is this a *new* system or the existing camp + waystation +
   graph machinery given persistence and staffing? The evidence says the latter; the ADR must say so
   and cap the delta accordingly.
3. **Define the object model in one table**: `outpost = { graph node, garrison/staff (survivors on
   duty), supply line (route), capacity (bunks), throughput (a producer with 35C's triple),
   knowledge state (32C rung), control (30B), obligations (38C) }` — every field names the authority
   that already owns it; **a field with no owner is a red flag**, and the list of red flags is the
   design's real content.
4. **Choose the state ownership**: an outpost's state belongs to the *campaign* save (one authority,
   47C-style whitelist: a new `outposts` section in `SaveSectionRegistry`, a `SaveStoreHub` façade,
   checksummed from birth) — never a parallel save tree.
5. **Choose the slot/site model**: multi-site within one campaign (recommended, keeps one envelope) vs
   multiple campaigns (needs 55B corpus thinking); decide once, document the reason, and note what it
   forecloses.
6. **Set the fiction boundary**: an outpost is a *reachability* consequence (graph + routes + dose +
   territory), not a bigger map — write the node budget (e.g. graph grows to N nodes, of which M are
   outdatable) so scope can't silently inflate.
7. **Define the player-visible payoff, falsifiably**: what does a day at an outpost give that the
   holdfast can't? (rations from a warm bed at distance; a warning channel; a place to send the
   unfit; a second dose sink; a way to survive a sector denial) — each must be a sentence a playtest
   can confirm or refute.
8. **Name the failure mode explicitly**: a second holdfast doubles every stateful loop (production,
   retention, rationing, leadership, calendar) — so this plan is only viable *after* 55A's retention
   and 45A's ladder exist, and the ADR cites them as blockers.
9. **Write the non-goals**: no second campaign-authority, no base-building sim with its own resource
   graph, no new UI framework, no unit control at an outpost (combat stays where it is), no
   procedurally generated territory.
10. **Split the work into three shippable slices** (58A → 58B → 58C) each independently playable,
    each adding no new authority, each with a 46B funnel/metric line.
11. **Have a second tool review the ADR** with only the pillars + this evidence table (the repo's
    cross-tool rule) and record the decision in `docs/balance/DECISIONS.md`-style form.
12. **Register the plan** in 53A's plan register with `STATUS: proposed / blocked-by: 55A, 53C, 32A`
    and `PREMISE_VERIFIED_AT` = the sha of this audit.
13. **Docs**: the ADR, plus a `docs/systems/OUTPOSTS.md` ownership table before any code exists.

**DoD:** a signed ADR that caps scope, names every owner, and can be refused by intake.

---

## Task 58B — The outpost loop: staff it, feed it, lose it

**Goal:** one working outpost lifecycle using only existing machinery, so the continuation is
measurable before it is broad.

**Files:** `WaystationSystem.cs` (+ its section), `ExpeditionSystem.cs`, `src/Host/ExpeditionHostSession.cs`,
`DutyRosterSystem.cs`/`DutyRosterAssignmentEngine.cs` (24A verdict), `TravelingCaravanSystem.cs`,
`CommitmentSystem.cs` (38C), `LedgerDebtSystem.cs`, `CensusClaimSystem.cs`/`VoluntaryRegisterSystem.cs`,
`RegionalTreatySystem.cs`, `FactionWarSystem` (control), `AtmosphereTextSystem`/voice (42/49A),
`DayEventKinds` (31A), new `assets/.../outposts.json`, `src/UI/WaystationNetworkPanel.cs`,
`Ashfall.Core.Tests/OutpostLifecycleTests.cs`.

### Substeps

1. **Establish** costs labour + materials through 35C's production triple (input bill, duty hours,
   duration) and a graph adjacency requirement — you can only go where the routes say you can.
2. **Staff** it from the roster: postings are duty assignments (24A fitness gates who may go;
   43B policy may make posting optional or compulsory; 44A relations decide whether a bonded pair
   splits, with 44's drift consequences).
3. **Feed** it from the same consumption authority as the holdfast (22A/22B): one ration ledger with
   a per-site draw — no second food system (the single most important non-goal).
4. **Supply line**: routes (32B) carry throughput; weather (20C), territory (30B), and closure
   (32B step 9) can starve an outpost honestly; caravan arrival is the delivery event (30C).
5. **Intel value**: an outpost is an ear — it extends radio coverage (33C) and knowledge rung (32C),
   so its payoff is partly *information*, which the game has finally learned to price.
6. **Territory**: control (30B) determines safety and can flip while you're there; a post in hostile
   ground has an explicit, forecastable risk, and losing it is authored, not random.
7. **Obligations**: 38C commitments may bind an outpost (deliver N by day X); an unreachable outpost
   can make a deadline impossible — the good kind of pressure, and the kind 38C step 9's
   satisfiability test must catch.
8. **Population**: `CensusClaimSystem` / `VoluntaryRegisterSystem` gain a site dimension, so people
   are counted where they are — paperwork as governance (43B), and the honest way to avoid a
   shadow-population bug.
9. **Production**: an outpost is a producer with 35A's delivery contract (sink bound or refusal
   reported) — and its output must respect storage (35B), which is how a distant larder behaves.
10. **Failure and loss**: abandon, overrun, or evacuate paths — memorial pipeline (41A) for deaths,
    grievance and consent consequences (43C), and a record in the legacy ledger (34C).
11. **UI**: extend the `waystation_network` surface with live state (no new console); every line
    click-throughs (31B) and the panel is keyboard-operable (37B).
12. **Metrics**: what it moves — food pressure per day, dose saved by shorter routes, obligations met,
    and 46B's funnel step "first outpost established" with its discovery rate.
13. **Tests**: lifecycle (establish→staff→supply→lose), starvation via route closure, posting
    fitness gating, census site counts, obligations with an unreachable outpost rejected by the
    satisfiability check, retention caps under 200 outpost-days, determinism, save round-trip, and
    **one negative test per new field: if an authority doesn't own it, the field doesn't ship.**
14. **Run the checklist** + `--expedition-selftest` + `--expansions-selftest`.

**DoD:** you can build a post, lose it honestly, and the campaign remembers both — with no second
authorities anywhere in the code.

---

## Task 58C — The second holdfast: continuation across campaigns, without power fantasy

**Goal:** carry the *record* forward — sites, names, decisions, endings — and let a new campaign
begin from a world that remembers, with no mechanical inheritance unless pillars approve it.

**Files:** `LegacyLedger.cs` (34C), `GenerationalSuccessionEngine`/`CenturySeedPanel` (chapter/year
clocks), `EpilogueMatrixRuntime`/`EpilogueContextFactory` (19A), `SaveSlotRoot.cs`,
`RegionalTreatySystem.cs`, `CensusClaimSystem.cs`, `FactionWarSystem.cs`, `StandingRecordEngine`,
`docs/design/ADR-0002-continuation.md`, `docs/saves/SAVE_MODEL.md`,
`Ashfall.Core.Tests/ContinuationTests.cs`.

### Substeps

1. **Write the distinction down first**: *generational succession* = the same holdfast across years
   (41C, exists); *continuation* = the next holdfast in the same remembered world (this task). If the
   code can't tell them apart, neither can the player.
2. **Consume 34C's record**, not a new one: endings, sites held, obligations met or broken, named
   dead — the legacy ledger is already records-only by design, and that constraint is the whole
   defence against a progression treadmill.
3. **Seed the next world from the previous one**: territory control, treaties, standing with
   factions, and which places carry graves/memorials (41B) — authored as *starting conditions*, a
   data shape, never a bonus multiplier.
4. **Publish the no-bonus rule** as a pillar-level constraint with an explicit exception process
   (53B's pillars + 46C's decision record), because "unlock a bigger generator" is one commit away
   and would undo the fiction.
5. **Name the inherited obligations**: the new holdfast starts inside promises the old one made
   (38C commitments, 47C treaties), which is where continuity becomes gameplay rather than decoration.
6. **Give the player an explicit choice**: continue, or start clean — clean must be fully supported
   and equally interesting (a 46A sweep per mode, or one of them is theatre).
7. **Keep one campaign authority**: the continuation writes *seed inputs* to a new campaign; it never
   reaches into the previous save, and the envelope whitelist (47C-style) keeps the shapes separate.
8. **Site memory on the map**: graves, ruins of your own failed outposts, and a name that appears in
   the standing record — reachable through 32C's knowledge ladder, so discovery still matters.
9. **Test the emotional claim honestly**: add a 54B playtest condition (returning-player session on a
   continued world) with the specific question "what from last time did you notice?" — a continuity
   feature nobody perceives is a data structure.
10. **Bound it**: cap inherited entities (sites, names, obligations) per 55A's retention policy, or
    a fifth campaign has a 200-line preamble nobody reads.
11. **Deprecate nothing silently**: if continuation supersedes the Century Seed framing, say so in
    the registry and `docs/` with evidence (29B), and mark the old plan superseded (29C).
12. **Tests**: seed derivation determinism, no-stat-bonus assertion (a guard test that fails if a
    legacy field appears in any balance calculation), clean-start parity, obligation carryover,
    retention caps, save-model isolation (a continued campaign cannot read the prior envelope).
13. **Docs**: `ADR-0002-continuation.md` + `docs/saves/SAVE_MODEL.md` (campaign vs legacy vs slot).
14. **Run the checklist** + `ashfall-seed-replay` across a campaign boundary + release gate.

**DoD:** the next campaign starts in a world that remembers you, and is harder or no easier for it.

---

## Cross-Task Dependencies

```
32A/32B/32C ──► 58A steps 2–3,6  ·  20A/23A ──► supply/route cost
24A/24B/35C ──► 58B steps 1–3,9  ·  38C ──► 58B step 7, 58C step 5
30A/30B/30C ──► 58B steps 4,6    ·  33 ──► 58B step 5
41A/41B/41C ──► 58B step 10, 58C steps 1,3,8  ·  43B/43C/44A ──► 58B steps 2,10
34C (records only) ──► 58C steps 2–4            55A (retention) ──► 58C step 10  ▸ BLOCKER
53C (intake) ──► this whole plan is inadmissible without it
```

**Prerequisite status at audit time (per `rails` reality):** 32A ❌ not started · 38C ❌ ·
41C ❌ · 43C ❌ · 34C ❌ · 55A ❌ — **this plan is therefore queued, not recommended**: the honest
first action is 58A's intake form, whose correct output is `blocked-by` with a date.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors (outpost ids/tiers)
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. bash scripts/ci/plan-intake-check.sh plan_58                  # (53C) rails + duplicate search
7. godot --headless --path . -- --expedition-selftest            # route/camp maths intact
8. godot --headless --path . -- --expansions-selftest            # waystation/faction surfaces
9. bash scripts/ci/triad-drift-gate.sh                           # new outposts section registered
10. retention soak: 200 outpost-days within 55A ceilings
11. negative tests: no unowned field, no stat bonus from legacy, no unreachable obligations
12. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Data | UI | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|---|
| 58A | 0 | 0 | 0 | 0 | 0 (docs + register) | Medium (design) | NONE (decisions only) |
| 58B | 2–3 | 4–6 | 1–2 | 2 | 16–22 | **High** | MEDIUM–HIGH (touches supply, roster, census) |
| 58C | 2 | 2–3 | 1 | 1 | 10–14 | High | MEDIUM (save-model isolation) |

**Guardrails (the whole point of this plan):** one authority per fact — one ration ledger, one
roster, one save envelope, one campaign state; no new resource type; no new UI framework; no
inherited mechanical bonus; no procedural territory; no node-count growth beyond the ADR budget; no
field in the outpost model that can't name its owning authority; and no implementation before
53C's gate says the rails are there. If the ADR concludes "the camps + waystations + graph already
answer this," the correct outcome of Wave 9's most ambitious plan is **a smaller one, or none**.
