# Plan Portfolio Integration Status — Forensic Report

**Date:** 2026-09-12
**Scope:** Read-only inventory of historical ASHFALL plans vs current repository evidence
**Live ledger:** `INTEGRATION_PLANS.md` — **no open batch**
**Primary evidence:** closeouts, authority matrices, `docs/foreman/PLAN_170_199_FORENSIC_AUDIT.md`, `docs/foreman/PARTIAL_IMPLEMENTATION_REBASE.md`, `KNOWN_DEBT.md`, and spot-checked Core/host sources

---

# 1. Target

Answer, for the historical plan portfolio:

1. What was integrated fully?
2. What was integrated without UI?
3. What exists without host/UI wiring?
4. What is only partial?
5. What was started and not finished?
6. What was never started?
7. What remaining work each incomplete plan would still need?

---

# 2. Executive Finding

1. **There is no single executable plan backlog.** `INTEGRATION_PLANS.md` is empty of packages. Historical plan trees are preserved debt (`DEBT-PLAN-SPRAWL`), not live authority.
2. **Plan numbers are not unique identities.** Multiple eras reused the same integers for unrelated systems. Treating `Main.Plans178_181.cs` as “historical Plans 178–181” is false.
3. **Many closeouts claim COMPLETE while still listing deferred UI, producers, or consequence adapters.** Classification below prefers source/debt evidence over title-page status.
4. **The only recently evidence-rebased incomplete portfolio is Plans 170–199** (feature proposals) plus deferred work from Plans 166–169.

---

# 3. Number-collision map (mandatory before reading any row)

| Number band | Meaning A (docs/master plans) | Meaning B (implemented `src/Main.Plans*.cs` / closeouts) |
|---|---|---|
| 158–161 | Modular vehicles / macro weather / trade routes / succession (`PLANS_158_161_MASTER_PLAN.md`) — **mostly unstarted** | Plan 158 = cordage/textiles archive; Plan 159 = tanning/leather archive — **live material-lore systems** |
| 178–181 | Art/culture, psychology/phobias, certification, difficulty (`PLAN_170_199_FORENSIC_AUDIT.md`) | Childhood/generational, prisoners, mutation trees, stealth (`Main.Plans178_181.cs`) |
| 198–201 | Health history / human migration in the 170–199 proposal set | CBRN / comms array / ceremonies / robotics — **player-facing closeout** |
| 102–105 vs 202–205 | Foundry accords / trade specialties already used 102–105 | Plastic pyrolysis / perimeter extend / mushrooms / airdrop renumbered to **202–205** |

**Rule:** always name the subsystem, never promote work by bare plan number.

---

# 4. Status taxonomy used here

| Label | Meaning |
|---|---|
| **FULL** | Core + data + save + host triad + player route/UI + focused verification evidence |
| **NO_UI** | Core/data/save/host tick exist; dedicated player panel/route missing or deferred |
| **NO_WIRING** | Core and/or data exist; production host day-owner / save-store / route incomplete |
| **PARTIAL** | Adjacent live authority exists; named contract from the plan is missing |
| **STARTED** | Recon/master plan/design bible or early phase only; primary systems absent |
| **UNSTARTED** | No proposed authority and no safe equivalent |
| **BLOCKED** | Needs product/architecture decision or upstream debt before any package |
| **DATA_ONLY** | Catalog/prose expansion; runtime producers/placement deferred |

---

# 5. Live governance state

| Item | Status | Evidence |
|---|---|---|
| Current integration batch | None open | `INTEGRATION_PLANS.md` |
| Plans 166–169 deferred debt | ACCEPTED | `KNOWN_DEBT.md` rows for consequence routing, water delivery, reload replay |
| Plans 170–199 portfolio | ACCEPTED historical debt; promotion-gated | `DEBT-PLANS170-199-PORTFOLIO`; forensic audit + partial rebase |
| Promotion candidates (design only) | 184 accessibility apply · 196 food type/temp · 173 radio programs | Rebase; **not** authorized builder packages yet |

---

# 6. Portfolio by status

## 6.1 FULL — integrated end-to-end (representative verified waves)

These have closeouts **and** host partials / panels / routes. Remaining polish may still exist; they are not “not started.”

| Plan / wave | Subsystem | Evidence | Residual expected work |
|---|---|---|---|
| 02–09 consolidated | Relics, vinyl, letters, faction-war host, audio/visual/medical gaps | `docs/plans/PLANS_02_09_FLAGSHIP_CONSOLIDATED_CLOSEOUT.md` | Ordinary maintenance only |
| 146–149 | EB-PVD, mine flail, microfluidic diagnostics, rail grinding | Unified closeout + `Main.Plans146_149.cs` + panels; handlers wired | Expedition travel consumers for route modifiers; deeper UI command coverage if still thin |
| 162 | Agriculture + nutrition diversity | Impl log PASS; `FarmingPanel`; host session | Cross-plan weather/raid hooks noted as later phases |
| 163 | Defense traps + raid seam | Impl log PASS; `DefenseGridPanel` | Keep perimeter vs trap ownership split |
| 164 | Psychological arc | Impl log PASS; psychology panel wiring in `Main.UiPanels.cs` | Do not fork into Plan-179 “unified phobia profile” without new package |
| 165 | Wildlife ecosystem / bestiary | `WildlifeEcosystemSystem` + `BestiaryPanel` + save + knowledge gate test | Do not add second bestiary ledger (Plan 187 retired as equivalent) |
| 198–201 *(CBRN wave)* | Chem warfare, comms array, ceremonies, robotics | `PLANS_198_201_CLOSEOUT.md`; UI test gate | Cross-plan scenarios A–F; robotics→combat/expedition actor mapping |
| 202–205 | Plastic pyrolysis, perimeter extend, mushrooms, cargo airdrop | Closeouts + `Main.Plans202_205.cs` | Confirm each sub-wave’s UI depth before claiming flagship-complete |
| B74–B77 | Geothermal ORC, ballistics, aeroponics, pneumatic dispatch | Closeouts claim host-implemented; panels in `Main.UiPanels.cs` | Scenario polish |
| B66–B69 host wiring | Metallurgy via foundry, seismic, cryo vault | `PLAN_B66_B69_HOST_WIRING_CLOSEOUT.md` | Optional cipher-wheel UI (B67); cryo panel polish if still thin |
| Flagship institutions / caravan–surgery–power–defense | Caravan trade, surgical ward, power subgrids, perimeter | `FLAGSHIP_SYSTEMS_INTEGRATION_COMPLETION_REPORT.md` | Ongoing balance only |
| Many content expansions with COMPLETE closeouts | e.g. 10, 11, 12, 14, 26, 33, 44, 46, 58, 60–63, 67, 73, 74, 76, 89, 92–94, 99, 101, 104*, 106, 109, 112, 113, 116, 122–124, 128, 140, 141, 145, … | Individual domain closeouts | Spot-check any `*` rows that later addenda mark as “runtime was not wired” |

\*Plan 104 closeout contains a correction addendum that earlier COMPLETE claims were false at one commit; treat as **verify before trusting**.

## 6.2 NO_UI — integrated without dedicated player UI

| Plan | What exists | Expected remaining work |
|---|---|---|
| **166** Salvage / reverse engineering | Core + catalog + research/crafting gates; focused tests | Workshop/lab player surface for dismantle/preview; broader lab-facility UI |
| **167** Espionage | `EspionageSystem` + host session + save + tests; **no `EspionagePanel`** | Intelligence map / mission UI; canonical faction consequence adapters (`DEBT-PLAN167-CONSEQUENCE-ROUTING`); radio presentation; rescue-quest routing |
| **168** Fluid logistics | Topology solver + WT bridge + host session; **no PlumbingPanel** | Production plumbing UI; Greenhouse/Disease delivery path (`DEBT-PLAN168-WATER-DELIVERY`); drinking ownership adapters |
| **169** Procedural narrative | Coordinator + templates + host session | Player quest UI for procedural instances; domain objective adapters; incident-context collection; reward/consequence routing |
| **78–81** Decon / geodetic / kinetic / chem recon | Core + save + host wiring + Wave-5 tests | Wave-6 UI panels (explicitly deferred) |
| **118–121** Fischer–Tropsch / UV corona / carbon composites / GPR | Core + catalogs + selftests; host production projections deferred | Dedicated panels, live day-owner registration where missing, per-system save-store wiring |
| **34** Research | Research system live | Player-facing **start research** UI still called out as missing |
| **87** Relic recipes | Data expansion complete | Relic repair/restoration player route (`StartRepair` unwired historically) |

## 6.3 NO_WIRING / DATA_ONLY — catalogs or Core islands without full runtime reachability

| Plan | Classification | Expected remaining work |
|---|---|---|
| **127** Verdict corruption/history expansion | DATA_ONLY | Wire new ladder keys into Verdict progress producers (only original six keys fire today) |
| **95** Journal voice prose | DATA_ONLY / deferred producers | Runtime producers that request the 12 Plan-95 situation keys |
| **67** Cassette sets | Data + narrative; set-cassette playback runtime absent | Collection/playback runtime if product wants set-level UX |
| **68** Shelter carvings | Data complete | Room-aware selection; event-conditioned carvings |
| **102/103** Foundry accords/treaties (data waves) | Data complete; typed consequences deferred | Consequence policies + dialogue/standing/epilogue consumers |
| Assorted expansion rows marked “placement deferred” | DATA_ONLY | Expedition/collectible placement once destination IDs committed |

## 6.4 PARTIAL — adjacent authority live; plan contract incomplete

### A. Plans 166–169 deferred seams (accepted debt)

| Debt | Expected package contents |
|---|---|
| `DEBT-PLAN167-CONSEQUENCE-ROUTING` | Map `EspionageConsequenceIntent` → one canonical faction/world consumer each; idempotency; save; cross-system test |
| `DEBT-PLAN168-WATER-DELIVERY` | Authority map; no-double-ledger transfer; deterministic delivery/reload; focused integration test |
| `DEBT-PLANS166-169-RELOAD-REPLAY` | Bounded continuous-vs-reload campaign replay for the four systems only |

### B. Plans 170–199 proposal portfolio (forensic audit + rebase)

| Plan | Status | Current adjacent owner | Expected remaining work (if promoted) |
|---|---|---|---|
| **170** Seasonal events | FULL equivalent | `SeasonalEventSystem` | None — do not duplicate |
| **171** Dynamic quests | FULL equivalent | Quest runtime + procedural narrative | Only if a concrete missing quest contract is proven |
| **172** Radiation mutation | FULL equivalent | `MutationSystem` + panel + save | None — keep medical owner |
| **173** Radio program production | PARTIAL / promotion candidate | Schedule + station reception live; **no program production** | Adapter map: one schedule slot, one delivery fact, one propaganda input; program save state; **no second schedule/receiver** |
| **174** Backstories | UNSTARTED | — | Narrative ownership decision + templates + save + host |
| **175** Meta / New Game+ | BLOCKED | — | Prove completion/achievement + profile storage first |
| **176** Aging | PARTIAL / BLOCKED | Legacy succession engine only | Live age chronology; **do not extend legacy migration code** |
| **177** Dreams | PARTIAL | Trauma/insomnia/psych exist | Dream templates + narrative-event contract |
| **178** Art/culture *(proposal)* | PARTIAL | Cultural archive vault exists | Creation→archive interface; distinct from `Main.Plans178` childhood system |
| **179** Psychology/phobias *(proposal)* | PARTIAL | Psychological arc + therapy exist | Unified profile/phobia catalog decision |
| **180** Certification | PARTIAL | Skills live | Decide derived vs persisted certification |
| **181** Difficulty | UNSTARTED | — | Product approval + bounded simulation acceptance |
| **182** Relationship drift | PARTIAL | Relations + daily tick | Instrument real interaction producers before decay |
| **183** Child development | PARTIAL | Cohorts are setup, not stages | Lifecycle stages; do not reuse starting cohorts |
| **184** Accessibility | PARTIAL / promotion candidate | Settings persist + panel expose 4 flags | Presentation-policy + reapply lifecycle so flags affect runtime |
| **185** Memory decay | PARTIAL | Phantom/knowledge adjacent | Bounded mutable-knowledge definition + decay rates |
| **186** Shelter maintenance | PARTIAL | Component degradation owners | Event/inspection projection only — **no unified durability ledger** |
| **187** Bestiary | FULL equivalent | Wildlife observations + `BestiaryPanel` | None — retired from partial queue |
| **188** Daily routines | PARTIAL | Shelter schedule + duty roster | Sub-day clock decision; no second scheduler |
| **189** Water sources | PARTIAL / BLOCKED | Treatment/hydrogeology | Blocked behind Plan 168 water delivery debt |
| **190** Item lore | PARTIAL | Provenance systems | Separate immutable lore from mutation provenance |
| **191** Identification | UNSTARTED | — | Inventory instance + economy pricing contracts |
| **192** Trade routes *(player-owned)* | PARTIAL | Caravan catalog exists | Player route ownership + standing/raid/save seams |
| **193** Chronic conditions | PARTIAL | Medical pipeline | Medical-record owner + non-stigmatizing UX |
| **194** Emergency alerts | PARTIAL | Hazard producers + HUD | Typed producer facts, priority, ack/persistence |
| **195** Specialization roles | PARTIAL | Skills/duty/apprenticeship | Capability→duty projection; no parallel role counter |
| **196** Food type/spoilage | PARTIAL / promotion candidate | Food preservation live | Name sole perishable authority + food-type + room temperature input |
| **197** Diplomacy/treaties | FULL equivalent | `RegionalTreatySystem` + panel/save | Only missing treaty consequences if evidenced |
| **198** Health history *(proposal)* | PARTIAL | Medical pipeline/dose | Record privacy/retention/consumers — **≠ CBRN Plan 198** |
| **199** Human/faction migration *(proposal)* | PARTIAL | Wildlife migration only | World/caravan/faction map — **≠ Comms Plan 199** |

### C. Other notable partials

| Plan | Status | Expected remaining work |
|---|---|---|
| **28** Ecology | PARTIAL — Phases 1–2 live; 4–7 open | Infestation runtime, ecological web consumers; do not revive retired Ecology island |
| **48** Weather route gates | Query/display; dispatch blocking deferred | Expedition dispatch validation consumer |
| **49** Micro-locations | History/check helpers | Host depletion enforcement before loot grants |
| **60** Vehicles | Ownership live; terrain gating follow-up | Route gating consumers for stored terrain |
| **146–149** A10 note | Route modifiers query APIs | Wire into `ExpeditionSystem` travel estimate if still open |
| **198–201** *(CBRN)* follow-ups | FULL core loop | Multi-system journey scenarios; robot combat/expedition mapping |

## 6.5 STARTED — recon / master plan / early phases without finishing the proposed systems

| Plan / pack | What exists | What does **not** exist | Expected remaining work |
|---|---|---|---|
| **158–161 master** (vehicles/weather/trade/succession) | Reconnaissance authority map | `ModularVehicleSystem`, `MacroWeatherSystem`, `SuccessionSystem` (spot-check absent) | Full flagship implementation **or** explicit retirement; do not confuse with cordage/tanning Plans 158–159 |
| **162–165** cross-plan phases G+ | Individual systems PASS | Several later “NOT STARTED” phase rows in impl log | Finish documented cross-plan hooks only |
| **202–205** recon amendments | Implemented as extensions where owners existed | Any roadmap items that still invent duplicate catalogs | Stay on extend-not-fork path |
| Expansion design bibles (Holdfast, Duty Roster, Year of Ash, Muster, Dose, Verdict, Black Flotilla, Exp 3–4, Deep Lore) | Design prose; some systems later hardened separately | Many bible IDs still PROPOSED | Promote only as packages against current owners |

## 6.6 UNSTARTED / BLOCKED proposals

| Plan | Status | Expected first package |
|---|---|---|
| **174** Backstories | UNSTARTED | Ownership decision + templates |
| **175** New Game+ | BLOCKED | Completion/achievement + profile boundary |
| **181** Difficulty presets | UNSTARTED | Product approval |
| **191** Item identification | UNSTARTED | Instance + pricing contracts |
| **189** Water sources | BLOCKED on 168 delivery | After water-delivery debt cleared |
| Historical **158–161 strategic systems** as named in the master plan | STARTED (recon only) / effectively UNSTARTED for the four proposed systems | Re-recon against current vehicle/weather/caravan/legacy owners, then one package |

---

# 7. Expected-work cheat sheet (by incompleteness class)

| If status is… | A future package must add… |
|---|---|
| **NO_UI** | One presentation owner (panel/route), truthful commands into existing host session, focused UI/selftest — **no new Core ledger** |
| **NO_WIRING** | Host Setup/Save/Tick triad, SaveSectionRegistry row if stateful, day-owner enrollment, content-utilization consumer |
| **DATA_ONLY** | Producers that emit/consume the new IDs (quest/journal/expedition/Verdict), or explicitly keep archival |
| **PARTIAL** | Named interface into the **existing** owner; save/event contract; focused test; non-goals that forbid parallel authority |
| **STARTED / UNSTARTED** | Premise evidence in `INTEGRATION_PLANS.md`, exact path claims, acceptance, focused command — before any builder edit |
| **BLOCKED** | Architecture/product decision document only |

---

# 8. Confidence & unknowns

| Claim | Confidence | Notes |
|---|---|---|
| No open live batch | High | `INTEGRATION_PLANS.md` |
| 170–199 classifications | High | Direct forensic + rebase on 2026-09-12 |
| 166–169 NO_UI + deferred delivery/consequences | High | Closeouts + `KNOWN_DEBT.md` + missing Espionage/Plumbing panels |
| “FULL” for older numbered closeouts (pre-100) | Medium | Closeout claims + presence of host wiring patterns; not every row re-verified in browser/Godot this pass |
| Exact completeness of every 202–205 UI command | Medium | Host partial and closeouts exist; not every panel action path re-traced this pass |
| Expansion bible → runtime mapping | Low–Medium | Some expansions later hardened under other names; treat bibles as design, not status |

**This pass did not:** run the full test suite, launch Godot, or modify production code.

---

# 9. Evidence index

- `INTEGRATION_PLANS.md`
- `KNOWN_DEBT.md`
- `docs/foreman/PLAN_170_199_FORENSIC_AUDIT.md`
- `docs/foreman/PARTIAL_IMPLEMENTATION_REBASE.md`
- `docs/architecture/PLANS_166_169_AUTHORITY_MATRIX.md`
- `docs/integration/PLANS_166_169_UNIFIED_CLOSEOUT.md`
- `docs/integration/PLANS_146_149_UNIFIED_CLOSEOUT.md`
- `docs/PLANS_118_121_ADVANCED_INDUSTRIAL_RECON_CLOSEOUT.md`
- `docs/plans/PLANS_198_201_CLOSEOUT.md`
- `docs/plans/PLANS_78_81_FLAGSHIP_CLOSEOUT.md`
- `docs/plans/PLANS_158_161_MASTER_PLAN.md` / `PLANS_158_161_RECONNAISSANCE.md`
- `docs/plans/PLANS_162_165_IMPLEMENTATION_LOG.md`
- `docs/plans/PLANS_202_205_RECONNAISSANCE.md`
- Spot-checks: `EspionageHostSession` present / `EspionagePanel` absent; `ModularVehicleSystem`/`MacroWeatherSystem`/`SuccessionSystem` absent; `Main.Plans158.cs` = textiles not vehicles; `Main.Plans178_181.cs` ≠ forensic 178–181
