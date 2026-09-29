# Feature / Task Plan: The Deep Works — held drifts joining the shelter shaft to the generated underground

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit)

> Prose companion: `docs/expansions/expansion_deep_works_plan.md`. Family index: `docs/expansions/expansion_shelter_under_pressure_index.md`.
> Not a claim. `SubterraneanSystem`, `TunnelNetworkSystem`, `ExcavationHazardSystem` and `ShelterExpansionSystem` keep their meaning. This plan **connects them** through their public APIs and adds one small *held-drift* ledger. It adds no hazard, oxygen, flood, excavation or resource model.

> **Editorial polish (prose pass):** sections **0**, **1b** and **12** are narrative texture only. No
> authority, claimed path, decision, acceptance criterion or verification step changes. Sample lines
> are content candidates for `works_lines.json` rows; they belong in data, never in code.

---

## 0. Prologue — Breakthrough

> *"The shaft went down. This is about going sideways — into rock nobody surveyed, under a sky
> nobody remembers."*

There is a sound a shelter makes when it stops being a building and becomes a settlement: the
first time somebody says *the works* and means somewhere other than the room they are standing in.
A drift is not a room. A drift is a **decision to keep a place** — to timber it, pump it, roster a
gang to it, and accept that it will one day be more expensive to hold than to abandon.

The Deep Works is the plan about that expense. Breakthrough is cheap and celebrated; holding is
weekly and dull and fatal. The underground does not resist you. It simply keeps its own books, and
the books are written in a hand that is not yours.

**Tone & register.** Industrial, procedural, weary. The vocabulary is the pit and the shift
gang: *bulkhead, schedule, gang, shoring, spoil, pump time*. Prose should feel like a works
diary kept by someone competent and tired. Dread arrives as arithmetic — a roof that is one
decision weaker than it was last week — not as spectacle.

**Mystery & texture.** The generated underground has ten nodes and no author. Nobody drew this
network; it was inferred from surface anchors and it never quite agrees with them. Held drifts are
the only place in the game where the player asserts *ownership* over terrain that was never
surveyed. §12 keeps the questions that ownership raises permanently open.

## 1. Goal & Outcome

> *Design intent: holding a drift should feel like keeping a promise made to a hole in the ground.
> The verbs are boring on purpose. The day you skip one is the day the story turns.*

- **Goal:** Let the player **break through** from the shelter shaft into a generated subterranean node and **hold** it: a linked drift with one **job** (Exit / Cistern / Store / Diggings), a weekly **Schedule** (props, air, drainage, gang), a **bulkhead** already in the game, an optional under-ash **tunnel segment**, and a **collapse-on-purpose** verb.
- **Outcome (observable):** on a fixed seed with shaft level ≥ a node's depth tier, a breakthrough project (crew, timber, days, stability cost) completes and marks the node Held with its bulkhead sealed; first opening reads the node's air class once and sets a methane reading on the sector whose id equals the node id; a Diggings gang draws seeded loot from the node's own scavenging table at a capped rate; deferred upkeep lets the existing seeded collapse roll run against a weaker roof and, on collapse, starts the existing rescue clock on the gang; an Exit drift registers a tunnel segment whose traversal and bypass come from the tunnel owner; collapse-on-purpose seals the drift permanently; no held drift → the underground behaves identically to today; save/load round-trips.
- **Non-Goals:** no new hazard/oxygen/flood/excavation model; no change to expedition entry into the dark; no new resource; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff lists untouched shared paths.

## 1b. Texture, Mystery & Voice

**The Works Book.**

Name the ledger in the UI. A *Works Book* is a physical object — ruled, initials, crossings-out,
the smear where somebody wrote in the damp. It is the only place in the shelter where a decision
to abandon something is recorded as a fact rather than a feeling. Let the book show history:
`sealed`, `collapsed on purpose`, `never opened again`. The player will read their own failures
back to themselves in a clerk's hand.

**What the player is never told.**

- Why the node ids match the hazard sector ids so exactly. The plan *observes* the coincidence
  (E7: sector id = node id) and never explains it. Keep it that way.
- Where the air in a `foul` node has been. It is foul by classification, not by cause.
- Whether the ten nodes are ten places or one place described ten ways.
- What `TryCollapseNode` is actually for. The verb exists because a works engineer would need it.
  That is the whole justification and no more should be added.

**Voice — sample fragments (content candidates for `works_lines.json`).**

> "Schedule week 6: timber 4, pump time 2, gang 3. Nothing went wrong. That is not the same as
> nothing changing."

> "Bulkhead sealed 11 days. Opened it once. Methane reading took the sector from green to a colour
> I do not have a name for."

> "We called it the Diggings because calling it the Listening made people nervous."

> "Collapse on purpose, day 44. Signed. Nobody argued. Nobody signed twice."

**Design texture beats.**

- **Skipped weeks must not announce themselves.** "Skipped weeks change nothing directly" (P5) is
  the plan's most atmospheric mechanic. Decay should be *discovered*, never broadcast.
- **One job at a time is a character constraint.** It says: this is a small shelter with a small
  gang, not a mining corporation. Do not soften it with queueing.
- **Collapse-on-purpose is the heaviest verb in the game.** It is permanent, it yields no salvage,
  and it cannot be undone by `TryClearBlockage`. Give it the longest confirmation beat of any
  command in the UI and the plainest wording.
- **The rescue clock is the emotional payoff.** A collapse with a gang inside is the only time this
  plan raises its voice. Everything else is set-up for that one day.

---

## 2. Evidence table (verified 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Generated network: 10 nodes, tiers 1–3; `SubterraneanNodeState` fields (integrity, shoringLevel 0–3, oxygenLevel, ventilationInstalled, waterLevel, blocked, lastHazardDay); zone fields include `surface_anchor_id`, `connection_rules`, `oxygen_class` (foul/stale/thin), `scavenging_table_id`. | `Subterranean/SubterraneanSystem.cs` L12–27; `subterranean_zones.json` | LIVE |
| E2 | Public ops with atomic billing: `TryShoreNode`, `TryInstallVentilation`, `TryClearBlockage`; host wrappers `ShoreNode`/`InstallVentilation`/`ClearBlockage`. | `SubterraneanSystem.cs` L338–412; `src/Host/SubterraneanHostSession.cs` L167–195 | LIVE |
| E3 | Daily tick: flood and decay for every node, oxygen drain for occupants; seeded cave-in per `(day, node)`; `OnCaveIn`, `OnNodeFlooded`, `OnForcedRetreat`. | `SubterraneanSystem.cs` L235–330 | LIVE |
| E4 | Save is checksummed via `SubterraneanSaveCodec` (version 1). | `Subterranean/SubterraneanSave.cs` L46–48 | LIVE (**VERIFY** additive fields vs checksum) |
| E5 | Discovery is from surface anchors only (`DiscoverFromAnchor`, `OnSurfaceLocationReached`). | `SubterraneanSystem.cs` L188; `SubterraneanHostSession.cs` L100 | LIVE |
| E6 | Flood bridge: node water → hazard sector **of the same id**, increases only. | `src/Main.Subterranean.cs` L85–103 | LIVE |
| E7 | Sectors: lazily created by any string id; methane, flood permille, spores, shoring; `TryToggleBulkhead` (blocked while trapped miners), `TryApplyMitigation` (8), `TriggerCaveInRescue`, `AddMethane`, `AddFloodWater`. | `Excavation/ExcavationHazardSystem.cs` L148–300, L338–351 | LIVE |
| E8 | Shelter shaft: blueprint `bp_deep_shaft_expansion` (14 days; scrap 25, rubble 15, planks 6; stability cost 12; `max_depth_level` 5); class default `MaxDepthLevel` 3; `TryStartDepthExcavation` (blueprint) vs `StartDepthExcavation` (hard-coded 12 days, 20 scrap, 10 rubble). | `shelter_construction.json`; `Shelter/ShelterExpansionSystem.cs` L97, L571–600, L812–838 | LIVE (**VERIFY** production path) |
| E9 | `ConstructionProjectType` has an unused `ExpansionTunnel = 2`. | `ShelterExpansionSystem.cs` L27–34; no other reference | LIVE / GAP |
| E10 | Project crews: `TryAssignCrew`, `RelieveCrew`, `ProgressAssignedProjects`. | `ShelterExpansionSystem.cs` L596–625 | LIVE |
| E11 | Tunnel owner: `RegisterSegment(id, name, from, to, lengthHours, difficulty, integrity)`, `CanTraverse`, `EvaluateSurfaceBypass`, `ReinforceSegment`, `ClearHazard`, `TickDay`; owned by `WastelandMapSystem.Tunnels`. | `Underground/TunnelNetworkSystem.cs` L137–322; `src/Main.TunnelNetwork.cs` | LIVE (**VERIFY** `RegisterSegment` accepts a node id) |
| E12 | Seeded loot resolver: `ScavengingTableCatalog.RollLoot(tableId, ISeededRng, itemFilter)`. | `Expeditions/ScavengingTableCatalog.cs` L41 | LIVE |
| E13 | Deep well: `TryBuild`, `SetEnabled`, `PerformMaintenance`, `TickDay`, yield ledger. | `DeepWellSystem.cs` L115–232 | LIVE |
| E14 | No collapse-on-purpose method on the node owner. | grep | GAP |
| E15 | A water intake port for a cistern job (where drinking/raw water enters the existing water owner). | water owners | **VERIFY (P0)** |
| E16 | A storage/spoilage modifier hook that a Store drift could use. | food/kitchen owners | **VERIFY (P0)** |
| E17 | Panels: `SubterraneanOperationsPanel`, `SubterraneanCartographyPanel`, `ExcavationPanel`. | `src/UI/` | LIVE |
| E18 | Class-to-methane mapping for `oxygen_class` values. | needs table | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Node state, shoring, ventilation, collapse rolls | `SubterraneanSystem` | additive `TryCollapseNode` (public, atomic) — **DEC-DW-07** |
| Sector hazards, bulkheads, rescue | `ExcavationHazardSystem` | calls only (sector id = node id) |
| Shaft, stability, project crews | `ShelterExpansionSystem` | breakthrough as a project (existing slot or additive blueprint) — **DEC-DW-03** |
| Tunnel segments and bypass | `TunnelNetworkSystem` | `RegisterSegment` call for Exit drifts |
| Loot tables | `ScavengingTableCatalog` | read via `RollLoot` |
| Held-drift ledger (heldSinceDay, job, gangIds, linkedShaftLevel, sealed flag) | — | `WorksLedger` (pure Core), nested `works[]` in the `subterranean` save DTO — **DEC-DW-01** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Subterranean/WorksLedger.cs` (new, pure), `Subterranean/WorksJobs.cs` (new), `Subterranean/WorksSchedule.cs` (new), `Subterranean/SubterraneanSystem.cs` (additive `TryCollapseNode` + nested DTO field only), `Subterranean/SubterraneanSave.cs` (additive field only), `Shelter/ShelterExpansionSystem.cs` (additive project handling only, **INT** if shared), `CatalogIntegrityValidator.cs` (`INT`), `Random/` stream id (`INT`)
**Data:** `works_jobs.json`, `works_breach_air_classes.json`, `works_schedule_costs.json`, `works_lines.json`; additive blueprint `bp_drift_breakthrough` in `shelter_construction.json` (**INT**, owner file)
**Host:** `src/Main.Subterranean.cs` (`INT`, day tick + held drift wiring), `src/Host/SubterraneanHostSession.cs` (`INT`), `src/Main.TunnelNetwork.cs` (`INT`, Exit registration), day-owner registration (`src/Main.CampaignOwners.cs`, `INT`)
**Presentation:** extend `SubterraneanOperationsPanel`, `SubterraneanCartographyPanel` (held drifts) — **DEC-DW-09**; no new routed panel
**Tests:** `Ashfall.Core.Tests/Subterranean/WorksLedgerTests.cs`, `WorksScheduleTests.cs`, `WorksBreachTests.cs`, `Ashfall.Core.Tests/Save/WorksSaveTests.cs`; extend subterranean and shelter-expansion tests

## 5. Packages

### DW-P0 — Premise audit (Auditor; read-only)
- Close E4, E8, E11, E15, E16, E18: additive save fields vs the codec checksum; which shaft-start path production calls; `RegisterSegment` with node ids; water-intake port; storage modifier hook; class→methane table; enumerate every reader of `SubterraneanNetworkState`, `ConstructionProjectType.ExpansionTunnel`, and every caller of `TryStartDepthExcavation`/`StartDepthExcavation`; foreman signs DEC-DW-01…10.
- **Accept:** each VERIFY answered with `path:line` or a test; breakthrough project slot decided; the two shaft paths reconciled or declared out of scope.

### DW-P1 — Held-drift model (Core, pure + nested DTO)
- `HeldDrift { nodeId, heldSinceDay, job, gangIds[], linkedShaftLevel, bulkheadSealed, everOpened }`; eligibility (`shaftLevel ≥ depth_tier`, one drift per node, node not blocked); nested additive `works[]`, default empty; sector id = node id.
- **Accept:** round-trip; old saves load (and checksum stays valid per P0); no rows → no change; validation rejects ineligible drifts with reasons.

### DW-P2 — Breakthrough project (Core + host, `INT`)
- Project through the existing crew pipeline (existing `ExpansionTunnel` slot or a blueprint row per DEC-DW-03): crew, timber, days by tier, stability cost; completion → drift Held, bulkhead sealed.
- **Accept:** cost billing atomic; stability cannot cross the safe floor; same seed → same completion; project without crew makes no progress.

### DW-P3 — First opening & breach (Core + host)
- First unseal reads the node's air class → one methane setting on the sector via `AddMethane`; flood already flows through the existing bridge; no writes to node water.
- **Accept:** table-driven (E18); node water untouched; a second opening produces no second reading.

### DW-P4 — Jobs (Core + content)
- **Exit** (route for siege/sally/relief; optional tunnel segment), **Cistern** (bounded daily water through the E15 port), **Store** (place flag; spoilage only via E16), **Diggings** (gang draws `RollLoot` from the node's table at a data-capped rate). One job at a time; a change costs a day.
- **Accept:** each job's effect bounded by data; Diggings never exceeds cap; a job with an unmet dependency (E15/E16 absent) is hidden, not faked.

### DW-P5 — Schedule & wear (Core)
- Weekly spend per drift via `TryShoreNode` / `TryInstallVentilation` / `TryClearBlockage` and pump time; skipped weeks change nothing directly; the existing seeded collapse roll and `TriggerCaveInRescue` handle consequences.
- **Accept:** costs data-driven; no direct mutation of integrity except through owner ops; a collapse with a gang starts the rescue clock.

### DW-P6 — Under-ash travel (Core + host, soft)
- Exit drift → `RegisterSegment` from the shelter location to the node's `surface_anchor_id` location; travel and bypass through the tunnel owner.
- **Accept:** segment registered once (idempotent); traversal/bypass numbers come from the tunnel owner; if E11 fails the package is deferred, not hacked.

### DW-P7 — Collapse on purpose (Core + host)
- `TryCollapseNode(nodeId)`: permanent seal; integrity floor; drift removed from the Works Book; gang inside trapped → existing rescue clock; siege sap signalled (soft).
- **Accept:** atomic and irreversible; no salvage; cannot be undone by `TryClearBlockage`; conservation of survivors (trapped ≠ lost silently).

### DW-P8 — Presentation
- Extend `SubterraneanOperationsPanel` (Works Book, Schedule, job, bulkhead) and `SubterraneanCartographyPanel` (held markers); focus/back preserved.
- **Accept:** presenter tests; panels hold no authority.

### DW-P9 — Cross-plan hooks
- Siege sap/collapse (LS), records Place (RK), storage modifier (RW) — ship dark until both ends exist.
- **Accept:** each hook is a no-op when the other plan is absent.

### DW-P10 — Content waves W1–W4 & governance close.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no held drift → subterranean, tunnel, hazard and shaft outputs identical on a saved corpus.
3. Conservation: a breakthrough or collapse never creates or destroys survivors or items outside owner-billed transactions.
4. Determinism: identical breach, Diggings rolls and collapse outcomes on replay (`CampaignStreamIds` fork; never `System.Random`).
5. Save round-trip mid-project with a held drift, a gang and a sealed bulkhead.
6. Node water is never written by this plan; a breach writes only methane to the sector.
7. Writes go only through the owners' public commands.

## 7. Cross-plan boundaries
- **The Long Siege:** sap action vs drift/collapse; Exit as route.
- **The Ration Wars:** Store → spoilage only via an existing hook.
- **The Record Keepers:** a dry drift is a Place.
- **The Drowned Coast / The Plague Year:** read existing weather/air owners; no shared numbers.
- **Year Two:** a second shelter's digging is Year Two's.
- **The Reconstruction Tree:** a dig may surface a fragment through the shared seam.
- **Radio Free Ashfall:** relay siting via an existing placement (VERIFY); no new station.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-DW-01 | Held-drift ledger nests in the `subterranean` save DTO; no new section. | architecture | Yes; confirm vs checksum in P0 |
| DEC-DW-02 | Reconcile or scope out the two shaft-start paths before any change. | governance | P0 decides |
| DEC-DW-03 | Breakthrough uses the unused `ExpansionTunnel` slot (or an additive blueprint row). | architecture | Prefer existing slot |
| DEC-DW-04 | Bulkhead sealed on completion; first opening is a decision. | design | Yes |
| DEC-DW-05 | Sector id = node id; breach writes only methane; existing flood bridge unchanged. | architecture | Yes |
| DEC-DW-06 | One job per drift; change costs a day. | rule | Yes |
| DEC-DW-07 | One additive public `TryCollapseNode` on the node owner. | architecture | Yes |
| DEC-DW-08 | Exit drifts register a tunnel segment only if E11 allows node ids. | scope | P0 decides |
| DEC-DW-09 | No new routed panel; extend the subterranean panels. | UI | Yes |
| DEC-DW-10 | Diggings use the node's own scavenging table at a capped rate. | rule | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Held`, `Drift`, `Works`, `Breakthrough`, `Countermine`)
- [ ] Premise re-verified (Rule 7); ledger files re-read; no overlapping live claim on subterranean/excavation/tunnel/shelter-expansion paths
- [ ] Signed decisions in hand

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing subterranean, tunnel-network, excavation-hazard and shelter-expansion tests (list from P0 selector)
- [ ] `--tunnel-network-selftest` / subterranean selftests (VERIFY args in `HostCli.TunnelNetwork.cs`, `HostCli.Subsidence.cs`)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: additive fields break the subterranean save checksum with no versioned path; the breakthrough would need a second construction pipeline; a job cannot function without a parallel resource or a second flood/hazard model; `TryCollapseNode` cannot be atomic through the owner; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered** — not gaps, not TODOs, not deferred work. They
keep the underground larger than the ledger that records it. Any future plan that answers one must
name the signed decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| DW-OM-1 | Why does every hazard sector lazily accept *any* string id? | E7's lazily-created sectors make the node-id coincidence possible without asserting it was designed. Explaining it would authorise a second map. | Never — architecture and atmosphere both depend on it. |
| DW-OM-2 | Who cut the first tunnel into the generated network? | Discovery is from surface anchors only (E5). The network pre-existed every anchor the player has found. | A canon owner, if the underground is ever given a history. |
| DW-OM-3 | Where does the spoil go? | The plan models bulkheads, shoring and pump time. It does not model spoil. Its absence is the point — this is a shelter, not a quarry. | Never — DEC-DW-03's boundary. |
| DW-OM-4 | Why is `ExpansionTunnel = 2` unused and already named? | DEC-DW-03 proposes to spend it. It will not be explained why the enum anticipated this exact plan. | Never — an artefact that reads as foreshadowing. |
| DW-OM-5 | Is the cistern water worse than the deep well's? | E15 asks for an intake port and nothing more. Water quality is the water owner's business and it has not spoken. | Water owner, if it ever exposes a quality input. |
| DW-OM-6 | What does a gang hear when the roof takes its first load? | `OnCaveIn` emits a fact. It does not emit an account. Accounts are *The Record Keepers*' concern. | The Record Keepers, via a read-only Place hook. |
