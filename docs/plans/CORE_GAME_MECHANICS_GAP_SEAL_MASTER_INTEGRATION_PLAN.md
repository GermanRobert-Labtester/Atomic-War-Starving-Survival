# CORE GAME MECHANICS — GAP-SEAL MASTER INTEGRATION PLAN

**Program id:** `CORE-MECH-2026-09-25`
**Revision:** R12 — Wave 10 integrated and sealed (rites→Reckoning); P0 vocabulary gate resolved by evidence, semantic drift refused
**Evidence HEAD:** `1678c0749f49b5e4f9a99231e8e71acf3e47fdb1`
**Save pin (evidence):** `266` (`Assert.Equal(266, SaveSectionRegistry.All.Count)`)
**Authority read in full (partial by design):** `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` — Parts 0–V, Volume 2.2 (Lane B seeds B-01…B-25), Volume 3.6 (subsystem deep maps DM-1…DM-17), Volume 43 (gaps/incidents inputs), Volumes 48/52/57 (seed register, decision brief, wave synthesis)
**Companion plans (disjoint claims):** `PLAYER_FACING_GAMEPLAY_LOOPS_MASTER_INTEGRATION_PLAN.md` (R5) · `PLAYER_FACING_TRIAD_B_EXERCISE_DREAM_SANITATION_INTEGRATION_PLAN.md` (R1) · `PLAYER_FACING_REALTIME_COMBAT_PHYSICS_AI_INTEGRATION_PLAN.md`
**Production code changed by this document:** none (planning artifact until Wave 1 claim)

**Pass history (quality ensurance):**
| Pass | Purpose | Outcome |
|---|---|---|
| R1 draft | Factory-grounded premise sweep, 12-wave program, contracts, appendices | 957 lines / 78,327 chars; 0 table defects |
| Polish-1 (this pass) | Structural completeness, evidence density, per-wave uniformity | 12/12 waves carry reality→delta→flow→failure→tests→files→DoD; PIR + seal framework bound to every wave; backlog/drift register normalized |
| Polish-2 (target) | Deep polish at the ~250k evidence-density target | appendices G–N added (deep runbooks, per-seed trace, worked examples, decision packets, test doctrine, closeout) |
| Precision-3 | Re-verify every named verb/owner/pin against live source; correct drift; tighten integration architecture | re-probe pass recorded in Appendix D2; fixed-string re-verification of every cited symbol; receiver contracts (AA) added as the architecture tightening; three authority-drift corrections (AJ) |

---

## 0. Executive contract

### What this program delivers

The factory authority's Lane B (mechanics and systems functionality) names 25 seeds and its backlog names 12 more. This program does **not** draft 37 subject plans; it executes the Factory Protocol (Part II) over the **core-mechanics slice** of that candidate space, premise-sweeps each candidate against live source, discards the ones whose premise has already been sealed or drifted, and promotes the survivors into a dependency-ordered integration program.

**The dominant defect class in the core loop is not missing systems — it is *unconnected mechanics*: authored math and state that exist, tick, and persist, but whose consequences never reach the player or the systems that should feel them.** Concretely, in the core survival loop:

- Food **spoilage** is computed (`FoodPreservationSystem`: `GetSpoiledFood`, `ConsumeFood(..., out spoiledConsumed)`) but the player eats through a different path (`HoldfastRuntimeSession.ConsumeFoodResult`) and **no foodborne disease exposure is ever raised** — `DiseaseSystem.TryExpose` / `TryInfect` have no foodborne caller.
- Radiation dose is booked (`DoseLedgerSystem.BookReading`) with **no Year-of-Ash storm-window conditioning** — `DoseLedgerSystem` references no `YearOfAsh`/storm type, and the `year_of_ash_events.json` window vocabulary never reaches dose accrual.
- Winter pressure is canon (Days 180–360) but the **power and water clusters reference no Year-of-Ash type at all** — the season has no teeth in the two systems that should suffer most.
- Expedition vehicles compute `EffectiveBreakdownRiskMultiplier` and `Repair`, but a breakdown routes **no injury or dose consequence** into the medical/dose owners.

That is the class this program seals: **pressure → consequence → counterplay**, wired through the existing owners, one authority per concern, zero parallel ledgers.

### The three seal tiers (program-wide)

| Tier | Meaning | Exit evidence |
|---|---|---|
| **GAP SEAL** | An unconnected or host-invisible mechanic becomes reachable: owner + host + save + day owner + probe agree | host_files ≥ 1 · save round-trip · probe green · completion chain to VERIFIED |
| **FEATURE SEAL** | The mechanic is player-operable in a normal session on the **right** owner | interactive surface · UI-bound mutation test · manifest entry |
| **MECHANIC SEAL** | The mechanic **changes decisions**: pressure, consequence in an existing owner, counterplay | consequence port/event · polarity/determinism test · manual cause→effect proof |

CLI probe alone stops at VERIFIED. This program requires MECHANIC SEAL for its core waves (W1, W2, W3, W4); the social/grammar waves (W7–W11) declare their own tier honestly in §8.

### Wave index (dependency order)

| # | Package id | Cluster(s) | Seed lineage | Tier | Gate |
|---|---|---|---|---|---|
| **W1** | `CORE-MECH-W1-FOODBORNE-DISEASE-BRIDGE` | C3→C2 | SB-05 / B-05 | MECHANIC | none (full integration starts here) |
| **W2** | `CORE-MECH-W2-DOSE-STORM-WINDOW` | C2×C12 | B-03 | MECHANIC | none |
| **W3** | `CORE-MECH-W3-WINTER-PRESSURE-POWER-WATER` | C12→C1/C3 | B-19 | MECHANIC | none |
| **W4** | `CORE-MECH-W4-BREAKDOWN-CONSEQUENCES` | C5→C2 | B-07 | MECHANIC | none |
| **W5** | `CORE-MECH-W5-DEFENSE-SEEGE-COUPLE` | C15×C7 | B-22 | FEATURE→MECHANIC | none |
| **W6** | `CORE-MECH-W6-MIGRATION-ENCOUNTER-BRIDGE` | C14→C5 | B-21 | MECHANIC | none |
| **W7** | `CORE-MECH-W7-QUEST-REOPEN-GRAMMAR` | C10 | B-16 | FEATURE | none |
| **W8** | `CORE-MECH-W8-GOSSIP-PROPAGATION` | C10→C8 | B-17 | MECHANIC | none |
| **W9** | `CORE-MECH-W9-BELIEF-STANCE-BRIDGE` | C9→C7 | B-14 | FEATURE | none |
| **W10** | `CORE-MECH-W10-RITES-RECKONING-EVIDENCE` | C9→C13 | B-15 | FEATURE | evidence-vocabulary check |
| **W11** | `CORE-MECH-W11-ONE-SHOT-TRIGGER-PRIMITIVE` | cross-cutting | B-35 | GAP | none (infrastructure) |
| **W12** | `CORE-MECH-W12-BALANCE-BASELINE-REFRESH` | Lane C | SB-06 / F-006 | PROGRAM | after W1–W4 land |

**Wave 1 is the full-integration entry point.** W11 (the missing one-shot narrative trigger primitive) is deliberately sequenced as infrastructure **after** the four core-mechanics waves: W8 and W10 are its first natural consumers, and building it first would make the core waves wait on an abstraction they do not need.

### Non-goals (program-wide)

- Not a second authority for anything: no `FoodSafetySystem`, no `WinterPressureSystem`, no `GossipNetwork`, no `TriggerRegistry` beside `IFlagLedger`.
- Not a re-run of sealed plans: Plan 189 water intake bridge (`DEBT-189-INTAKE-ADVISORY-BRIDGE` is RETIRED/sealed — see §2.2), Plan 216 exercise, Plan 177 dream, Plan 210 sanitation, Expansion 41 sleep acoustic, and the PFGL master plan's W1–W10.
- Not touching decision-blocked items: `DEC-05` merchant restock (SEALED), black-market funds legs, faction-war per-strike emitter, flooded-route topology, quarantine drain, XP-04/06, EN-01…08, string freeze.
- Not balance *design*: W12 measures and publishes deltas; it does not retune numbers without a foreman-signed target.
- Not Unity. Not full-suite-as-default. Not mass refactors. Not unclaimed shared-hub edits.
- Not narrative expansion (Lane A is the factory's lane for that; this program touches narrative only where a mechanic consumes prose).

---

## 1. Objective

Deliver the highest-value **core game mechanics gaps** as sealed, player-felt loops by extending the current owners, in this order of player consequence:

1. **Eating spoiled or badly preserved food can make you sick** — through the existing disease authority, with prevention (cure correctly, store cold, discard) as counterplay.
2. **Radiation risk rises measurably inside Year-of-Ash storm windows** — exposure becomes a scheduling decision, not a background number.
3. **Winter bites power and water** — filter burn, reserve drawdown, and hardening costs rise in the 180–360 window through the systems that own them.
4. **Vehicle breakdowns hurt** — injury/exposure consequences route into the medical and dose owners, making preflight maintenance and track-gear repair real decisions.
5. Then the defense, ecology, quest, information, social, and endgame couplings (W5–W11), and a measured balance baseline (W12).

### Success definition (PLAYER-OPERABLE + MECHANIC-FELT)

In an ordinary campaign, verified with focused evidence:

1. A player who eats spoiled food can contract a foodborne disease; a player who cures and stores cold cannot. (W1)
2. Two identical exposure events, one inside a storm window and one outside, book different dose outcomes; the player can read the band and act on shielding/antirad. (W2)
3. Across Days 180–360, power and water systems show higher seasonal draw and the player has authored counterplay (hardening, reserves). (W3)
4. A breakdown event produces a medical/dose consequence the player sees and can mitigate with repair/track-gear investment. (W4)
5. Defense values and sky-armor values measurably change siege/orbital outcomes. (W5)
6. Migration state changes which travel encounters appear on crossing routes. (W6)
7. A failed or abandoned quest can reopen when a new discovery satisfies its conditions. (W7)
8. A moral choice's gossip propagates over time/distance along the modeled channels and changes what NPCs know. (W8)
9. Belief movement shifts faction standing through the sole stance engine. (W9)
10. Performed memorial rites enroll as Reckoning evidence. (W10)
11. A reusable one-shot trigger primitive exists with a day threshold + gate, one-shot semantics, and persistence. (W11)
12. A refreshed balance baseline publishes measured deltas for the newly sealed mechanics. (W12)

---

## 2. Current reality (evidence 2026-09-25)

### 2.1 Architecture and pins

| Layer | Path | Role |
|---|---|---|
| Core | `Assets/Ashfall.Core/` | Engine-free authority (netstandard2.1); no `Godot`/`UnityEngine` references |
| Host | `src/` | Godot adapters, panels, CLI (net8.0) |
| Data | `Assets/StreamingAssets/Data/` | JSON authority (snake_case, `schema_version`) |
| Tests | `Ashfall.Core.Tests/` | Focused xUnit via `scripts/run_test.sh` |
| Save | `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` | **266** sections; pin asserted in `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs:315,317` |
| Surfaces | `docs/player_surface_manifest.json` | **219** total · **58** InteractiveCommands · **161** ReadOnlyObservational |
| Ports | `docs/ci/port_contract_policy.json` | `total_seams: 307` |
| RNG | `CampaignStreamIds` forks | never `System.Random` in deterministic Core |

### 2.2 Factory premise sweep — Step 1 results (live, this revision)

The Factory Protocol requires a premise sweep before selection and says a candidate whose premise fails is **discarded, not patched**. Results:

| Seed | Authority claim | Live finding | Disposition |
|---|---|---|---|
| **B-33** (Vol 43, P1/G1) | `AquiferPiezometerEngine.BuildAdvisory()` → `WaterTreatmentSystem.RegisterContaminationAdvisory` is UNWIRED, priority G1 | **FALSE — already wired.** `src/Host/PiezometerHostSession.cs:116-117` calls `System.BuildAdvisory()` then `waterTreatment.RegisterContaminationAdvisory(...)`; `KNOWN_DEBT.md:44` records `DEBT-189-INTAKE-ADVISORY-BRIDGE` **RETIRED** ("Sealed 2026-09-12") | **DISCARD** (premise sealed) — do not re-plan |
| **B-08** (scavenging parity) | 49 tables vs 53 destinations | **STALE NUMBERS.** `scavenging_tables.json` = 54 tables; `expeditions.json` = 75 expeditions; `travel_encounters.json` = 57 encounters; no `expedition_destinations.json` exists | **RE-VERIFY at P0**; not a Wave (data audit, not mechanics) |
| **B-05** (preservation × disease) | bridge proposed | **GAP CONFIRMED.** `FoodPreservationSystem.cs` contains no `Disease`/`Contaminat`/`Pathogen` reference; `ConsumeFood` has no player-path caller | **PROMOTED → W1** |
| **B-03** (dose × fallout window) | coupling proposed | **GAP CONFIRMED.** `DoseLedgerSystem.cs` references no `YearOfAsh`/`StormWindow` | **PROMOTED → W2** |
| **B-19** (winter pressure power/water) | pressure extensions proposed | **GAP CONFIRMED.** No `YearOfAsh` reference in any `*Power*` or `*Water*` Core file | **PROMOTED → W3** |
| **B-07** (vehicle breakdown consequences) | consequence routing proposed | **GAP CONFIRMED.** `ExpeditionVehicleSystem` computes risk/repair but routes no injury/exposure | **PROMOTED → W4** |
| **B-22** (defense × siege) | defense values into siege math | **GAP CONFIRMED.** `DefenseGrid` appears only in `src/UI/DefenseGridPanel.cs` + `src/Main.Plans162_165.cs`; no siege consumer | **PROMOTED → W5** |
| **B-21** (migration × encounters) | migration conditions encounters | **GAP CONFIRMED.** Migration engines (`SeasonalHumanMigrationEngine`, `MigrationConsequenceEngine`) are consumed by wildlife/harvest Main files, not travel-encounter selection | **PROMOTED → W6** |
| **B-16** (quest reopening) | reopening grammar unimplemented | **GAP CONFIRMED.** No reopen logic in Core quest systems (only narrative strings) | **PROMOTED → W7** |
| **B-17** (gossip propagation) | time/distance-lagged propagation | **GAP CONFIRMED.** `moral_choice_gossip` is referenced only from `src/Main.MoralChoice.cs` (read-side) | **PROMOTED → W8** |
| **B-14** (belief × stance) | belief shifts standing | **GAP CONFIRMED.** `FactionStanceEngine` consumers are economy/campaign-services; no `belief_movements` → stance bridge | **PROMOTED → W9** |
| **B-15** (rites → Reckoning) | rites enroll as evidence | **GAP CONFIRMED.** Reckoning evidence enrollment (`EnrollEvidence`) has no memorial-rite caller | **PROMOTED → W10** |
| **B-35** (one-shot trigger primitive) | primitive missing | **GAP CONFIRMED.** No general one-shot trigger type; `IFlagLedger` provides flags/counters but not day-thresholded one-shot semantics | **PROMOTED → W11** |
| **B-23** (difficulty consumers) | consumer binding | **COORDINATE ONLY.** PFGL master plan W7 and `CF-XP01` own the difficulty surface and consumer line; this program does not duplicate it | **DEFERRED to PFGL owner** |
| **B-09/B-10/B-11** (flooded routes, faction-war emitter, black-market funds) | — | decision-gated / sealed in live ledger | **OUT (no signature)** |
| **B-04** (child-health cohort bridge) | medical×cohort | real but overlaps sealed medical flagship set; medium value | **BACKLOG (Appendix C)** |
| **B-01/B-02** (shelter-failure cascade, grid seal follow-through) | quarantine exit criteria | exit criteria must be read in-session before any plan | **BACKLOG (Appendix C)** |
| **B-12** (market-rumor bands) | commodity coverage extension | rules already consumed (`Main.Economy`, `RadioHostSession`); extension is data | **BACKLOG (Appendix C)** |
| **B-25** (rescue-remains medical) | sealed-surface follow-through | coordinate with the sealed distress owners; additive only | **BACKLOG (Appendix C)** |

**Sweep conclusion:** twelve candidates survive (B-05, B-03, B-19, B-07, B-22, B-21, B-16, B-17, B-14, B-15, B-35, plus SB-06 as the program closer). One is discarded on sealed premise (B-33), one is deferred to a sibling owner (B-23), six go to the backlog appendix with reasons.

### 2.3 What is already right (do not redo)

- Disease authority is complete and waiting for foodborne callers: `DiseaseSystem.TryExpose(DiseaseExposureContext)`, `TryInfect(survivorId, diseaseId, day, sourceId?)`, `Infect(...)`, `TriggerOutbreak(...)`, `TickDaily(...)`, immunity verbs, `DiseaseCatalog` with `exposure_sources` (`Disease/DiseaseCatalog.cs:347,384,411`).
- Dose authority is complete: `BookReading(survivorId, day, nominalMsv, source, highEnergyEvent, antiRadBefore, antiRadAfter, ISeededRng rng)`, `AssignDosimeter`, `SetShieldingFactor`, `RecordAntiRadTreatment`, `Calibrate`, `BandFor`, `GetCumulative`.
- The Year-of-Ash corpus exists with an event/window vocabulary (`year_of_ash_events.json` + `YearOfAshCatalogLoader.LoadEvents`).
- Faction stance engine is the sole standing authority: `GetTrust`, `ModifyTrust`, `GetStance`, `WillTrade`, `GetRaidAggression` (`FactionStanceEngine`).
- Flag primitives exist for one-shot *flags* (`IFlagLedger.IsSet/Set/Clear/GetCounter/Increment`) but not day-thresholded one-shot **triggers**.
- The program adds **no save sections**: every wave reuses existing sections (`food_preservation` reused, dose, medicine, faction, quest, reckoning families) or stores nothing (adapters).

### 2.4 Reachability of the four core-wave owners (measured)

| Concern | Core owner | Host files | Save | Day owner | Probe | Player surface |
|---|---|---:|---|---|---|---|
| Food preservation | `Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs` | 1 (`src/Main.Plans62_65.cs` — no host session) | `FoodPreservationSaveStore` (via Main) | none found | none | none |
| Dose ledger | `Assets/Ashfall.Core/DoseLedgerSystem.cs` | 5 (`Main.Medical`, `Main.Phase0`, `Main.UiTests.Dose`, `Dose/DoseRegisterSurface`, `ContentUtilizationRuntimeCollector`) | dose family | medical phase | dose probe family | dose register surface |
| Expedition vehicle | `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` | expedition family | expedition family | expedition phase | expedition probes | expedition surface |
| Disease | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` | disease host family | `DiseaseSaveStore` | `TickDaily` | disease probes | medical surfaces |

---

## 3. Required delta (program summary)

| Gap | Broken completion-chain link | Wave |
|---|---|---|
| Foodborne illness | Spoilage computed → player eats on a different path → no exposure call | W1 |
| Storm-window dose | Nominal mSv booked flat → no season conditioning → season has no teeth in dose | W2 |
| Winter power/water | Season canon → power/water unaware → no winter preparation payoff | W3 |
| Breakdown consequence | Risk computed → no injury/dose consequence → maintenance is cosmetic | W4 |
| Defense payoff | Defense values exist → siege/orbital math ignores them → building a wall is decoration | W5 |
| Migration texture | Migration state exists → travel encounters ignore it → the wasteland is static | W6 |
| Quest recovery | Quests fail/abandon → no reopen grammar → discoveries can't revive threads | W7 |
| Information flow | Gossip catalog exists → no propagation over time/distance → choices are local | W8 |
| Belief politics | Beliefs move → stance engine unaware → interiority is cosmetic | W9 |
| Rites memory | Rites performed → Reckoning unaware → memorial acts leave no trace at the end | W10 |
| Trigger primitive | Many waves want "once, on day D, if gate G" → no shared primitive → each wave reinvents it | W11 |
| Measured balance | New mechanics land → no baseline refresh → balance claims become unverifiable | W12 |

---

## 4. Evidence index

| Evidence | Location |
|---|---|
| Factory protocol (7-step loop, invariants) | authority Part II |
| Lane B mechanics seeds (B-01…B-25) | authority Volume 2.2 |
| Subsystem deep maps (DM-1…DM-17) | authority Volume 3.6 |
| Verified-unwired + unstarted-type seeds (B-33, B-34) | authority Volume 43 |
| Decision brief (8 foreman decisions) | authority Volumes 52/57 |
| Spoilage computed, no disease caller | `FoodPreservationSystem.cs` (no Disease/Contaminat refs); `ConsumeFood` has no player-path caller; `HoldfastRuntimeSession.ConsumeFoodResult(itemId, amount, survivorId?)` is the eating path |
| Disease exposure entry | `Disease/DiseaseSystem.cs:437` `TryExpose`, `:491` `TryInfect`, `:654` `TriggerOutbreak`, `:737` `TickDaily` |
| Disease exposure source vocabulary | `Disease/DiseaseCatalog.cs:347,384,411` (`exposure_sources`, `ExposureSources`, `GetExposure`) |
| Dose booking signature | `DoseLedgerSystem.cs:122` `BookReading(...)` with `ISeededRng` |
| Dose has no season awareness | zero `YearOfAsh`/`StormWindow` references in `DoseLedgerSystem.cs` |
| Year-of-Ash event vocabulary | `year_of_ash_events.json`; `YearOfAshCatalogLoader.LoadEvents` (`:207`) |
| Power/water have no season awareness | zero `YearOfAsh` references in `*Power*`/`*Water*` Core files |
| Vehicle risk without consequence | `ExpeditionVehicleSystem.cs:55` `EffectiveBreakdownRiskMultiplier`, `:179` `Repair`, `:263` `RepairTrackGear`; no injury/exposure routing |
| Defense without siege coupling | `DefenseGrid` only in UI/Main; no siege consumer |
| Migration without encounter coupling | `SeasonalHumanMigrationEngine` / `MigrationConsequenceEngine` consumed by wildlife/harvest, not travel encounters |
| No quest reopen | zero reopen logic in Core quest systems |
| Gossip read-only | `moral_choice_gossip` referenced only from `src/Main.MoralChoice.cs` |
| Stance engine is sole authority | `FactionStanceEngine` `GetTrust/ModifyTrust/GetStance/WillTrade/GetRaidAggression` |
| Rites not enrolled | `EnrollEvidence(int amount = 1)` on Reckoning with no memorial-rite caller |
| One-shot primitive absent | `IFlagLedger` has flags/counters, no day-thresholded one-shot trigger |
| Live pins | save 266; surfaces 219/58/161; port seams 307 |

---

## 5. Proposed architecture

### 5.0 Integration architecture at a glance (who writes what, and into whom)

| # | Wave | Writer (source of truth) | Receiving owner (sole) | Read-only inputs | State written |
|---|---|---|---|---|---|
| 1 | Foodborne | `FoodPreservationSystem` (spoilage) | `DiseaseSystem` (infection) | disease catalog rows | none new (reuse families) |
| 2 | Dose window | Year-of-Ash calendar (window) | `DoseLedgerSystem` (dose) | dose family | none new |
| 3 | Winter pressure | Year-of-Ash calendar (band) | chosen power owner + chosen water owner | their catalogs | none new |
| 4 | Breakdown | `ExpeditionVehicleSystem` (risk) | medical intake + `DoseLedgerSystem` (+ `DiseaseSystem` for contamination) | expedition family | none new |
| 5 | Defense siege | defense owners (projection) | faction-war/raid resolver | defense catalogs | none new |
| 6 | Migration | migration engines (read model) | travel-encounter selector | `travel_encounters.json` | none new |
| 7 | Quest reopen | quest owners + `IFlagLedger` | quest state | quest catalogs | none new |
| 8 | Gossip | moral-choice state (seed) | existing rumor/info owners | gossip + rumor data | rides info owner state |
| 9 | Belief stance | `belief_movements` (input) | `FactionStanceEngine` (trust) | faction catalogs | none new |
| 10 | Rites | ritual owner (performance) | Reckoning `EnrollEvidence` | rites/ritual catalogs | none new |
| 11 | One-shot | new primitive (on `IFlagLedger`) | flag family save | — | flag family |
| 12 | Balance | harness | — | baselines | report only |

The architecture rule that makes the program safe to run in parallel: **every wave has exactly one receiving authority, and no wave writes another wave's source column.** W1 writes Disease; W4 writes medical/dose/disease as a *consumer*; neither ever stores the other's state.

### 5.1 Program shape

```
[Player act / day tick / expedition event]
  → Main command or existing day owner
    → existing host session
      → existing Core authority (mutation)
        → consequence port into the EXISTING receiving owner
          (Disease / DoseLedger / Needs / FactionStance / Reckoning / encounter selector)
            → existing journal + day-event vocabulary
              → existing save family (no new sections)
                → focused test proving pressure → consequence → counterplay
```

### 5.2 Completion chain

`DECLARED → COMPILED → CONSTRUCTED → REGISTERED → CALLED → MUTATES → OBSERVED → PERSISTED → RESTORED → VERIFIED → PLAYER-OPERABLE → MECHANIC-FELT`

The final link is this program's addition to the chain: a mechanic is not done when the command works, but when the consequence is observable in another owner and the player has counterplay.

### 5.3 Collision map (resolve before coding)

| Concern | Extend | Forbidden parallel | DEC? |
|---|---|---|---|
| Food safety | `FoodPreservationSystem` + `DiseaseSystem` | a `FoodSafetySystem`; disease risk stored in preservation state | No |
| Eating path | `HoldfastRuntimeSession`/inventory consumption | a second eat verb that bypasses inventory | No |
| Radiation | `DoseLedgerSystem` | a second dose ledger; season scalars parallel to difficulty | No |
| Winter pressure | power grid + water owners via Year-of-Ash calendar | a `WinterPressureSystem` storing multipliers | No |
| Breakdown | `ExpeditionVehicleSystem` → medical/dose owners | injury state in vehicle save | No |
| Defense payoff | `DefenseGrid`/perimeter values → siege resolver | a second siege model | No |
| Migration | migration engines → travel-encounter selector | encounter tables keyed to migration copies | No |
| Quest reopen | quest owners + `IFlagLedger` | a reopen queue outside quest state | No |
| Gossip | moral-choice state + rumor/info-flow owners | a gossip network with its own persistence | No (advisory: coordinate with sealed radio/info owners) |
| Belief → stance | `FactionStanceEngine.ModifyTrust` | a belief-side trust cache | No |
| Rites → Reckoning | `EnrollEvidence` | rite state duplicated into endgame | No |
| One-shot primitive | new shared primitive built ON `IFlagLedger` persistence | a second flag store | No |
| Difficulty consumers | PFGL master W7 / `CF-XP01` | parallel scalars | Out of program |

### 5.4 Ownership matrix (program)

| Concern | Owner | Host | Save | UI | RNG | Observability |
|---|---|---|---|---|---|---|
| Spoilage → disease | `FoodPreservationSystem` → `DiseaseSystem` | existing + thin exposure port | preservation + disease families | kitchen/food surface (bind) | disease `ISeededRng` stream | journal + outbreak events |
| Storm-window dose | `YearOfAsh` calendar → `DoseLedgerSystem` | medical family | dose family | dose register | existing dose stream | journal `dose_storm_window` |
| Winter power/water | Year-of-Ash calendar → power/water owners | existing hosts | power/water families | existing panels | none (deterministic curves) | day events |
| Breakdown consequences | `ExpeditionVehicleSystem` → medical/dose | expedition host | expedition + medical | expedition surface | expedition stream | journal + day events |
| Siege coupling | defense values → faction-war resolver | existing | faction/defense | defense panel | none | journal |
| Migration encounters | migration engines → encounter selector | expedition/travel host | wildlife/travel | travel surface | travel stream | journal |
| Quest reopen | quest owners + flags | quest host | quest family | quest panel | none | journal |
| Gossip | moral-choice → info owners | moral-choice host | moral-choice | radio/journal surfaces | info stream | journal |
| Belief → stance | `FactionStanceEngine` | faction host | faction | faction panel | none | journal |
| Rites → Reckoning | ritual owner → `EnrollEvidence` | spiritual/endgame hosts | spiritual + endgame | memorial surface | none | standing record |
| One-shot primitive | shared Core primitive on `IFlagLedger` persistence | — | rides flag save | none (infrastructure) | none | census |
| Balance refresh | harness | harness | report | — | seeded runs | `docs/balance/` report |

---

## 6. Data flow, state, and determinism rules

1. **Data-first preference (factory invariant):** where a mechanic can be expressed as authored JSON through an existing loader, it must be. W1's foodborne diseases are expressible as `disease_catalog.exposure_sources` rows; W2's window multipliers as Year-of-Ash event rows; W3's seasonal curves as power/water tuning rows. Code is for the *connection*, not the content.
2. **No new save sections.** Every wave reuses an existing family or stores nothing. Pin stays 266 unless an unrelated landed wave moves it; the pin is re-read, never carried forward (factory DR-07 lesson).
3. **Determinism:** every probabilistic consequence draws from an existing seeded stream (`CampaignStreamIds` forks or the host's existing rng). No `System.Random`, no wall-clock seeds, no unordered-dictionary iteration in consequence paths (sort ids Ordinal where iteration order reaches a hash or list).
4. **Exactly-once:** day-tick consequences guard by `(owner, subject, day, source)`; command consequences are one-shot per command. Save/load must not replay or double-apply.
5. **Fail-closed:** missing catalog row, unknown id, or absent receiving owner → no effect + journal fact; never a silent success, never an exception swallowed into a fake pass.
6. **One authority per concern** is re-asserted at each wave exit by a `rg` assertion that no second ledger type appeared.
7. **Polarity discipline:** every port states its sign convention against the receiving owner's convention (e.g., Morale higher=worse; Fatigue higher=worse; disease probability is a rate, not a delta).

---

## 7. API contracts (verified live at HEAD `1678c074`)

> **Binding rule:** verbs below were probed this revision. `(core)` = Core owner, `(host)` = host session, `(main)` = Main partial. If live code drifts by claim time, **live code wins** — reconcile in a revision note (Rule 7); never invent bypass wrappers.

### 7.1 Food safety (W1)
| Verb (live) | Signature / role |
|---|---|
| `ConsumeFood` | `(core) int ConsumeFood(string foodItemId, int neededCount, out int spoiledConsumed)` — spoilage already reported here |
| `GetSpoiledFood` / `GetTotalFood` | `(core) int GetSpoiledFood(string? foodItemId = null)` / `GetTotalFood(...)` — board readouts |
| `DiscardSpoiled` | `(core) int DiscardSpoiled(string? foodItemId = null)` — **counterplay** (discard before eating) |
| `StartCuringJob` / `AddCohort` | `(core) ActionResult StartCuringJob(recipeId, cookId, currentDay)` / `AddCohort(foodItemId, quantity, tierId, currentDay)` — **counterplay** (cure correctly) |
| `SetStorageTemperatureC` / `StorageTempShelfLifeFactor` | storage discipline; colder = longer shelf life |
| `ConsumeFoodResult` | `(host) ActionResult ConsumeFoodResult(string itemId, int amount = 1, string? survivorId = null)` in `src/Host/HoldfastRuntimeSession.cs` — the **actual player eating path** |
| `TryExpose` | `(core) DiseaseExposureResult TryExpose(DiseaseExposureContext context)` — receiving authority for foodborne exposure |
| `TryInfect` / `Infect` | `(core) DiseaseExposureResult TryInfect(survivorId, diseaseId, day, string? sourceId = null)` / `Infect(survivorId, diseaseId, day)` |
| `TriggerOutbreak` / `TickDaily` | outbreak escalation and daily disease progression (existing authority; W1 only *feeds* it) |
| Catalog | `disease_catalog.exposure_sources` rows (`Disease/DiseaseCatalog.cs:347`) — data-first path for foodborne sources |

### 7.2 Dose × storm window (W2)
| Verb (live) | Signature / role |
|---|---|
| `BookReading` | `(core) DoseBandResult BookReading(survivorId, day, nominalMsv, source, highEnergyEvent, antiRadBefore, antiRadAfter, ISeededRng rng)` — the single booking entry; W2 modulates `nominalMsv` upstream |
| `AssignDosimeter` / `SetShieldingFactor` / `RecordAntiRadTreatment` / `Calibrate` | dosimeter lifecycle + **counterplay** (shielding factor, antirad timing) |
| `BandFor` / `GetCumulative` / `GetAdministrativeBand` | band readouts for the surface |
| `SetForgedCleanBill` / `SetAdministrativeClassificationOverride` | existing administrative surface (unchanged) |
| Year-of-Ash calendar | `YearOfAshCatalogLoader.LoadEvents(...)` (`:207`) → `year_of_ash_events.json` window vocabulary — the conditioning source |

### 7.3 Winter pressure (W3)
Owners: power grid / fuel systems (`SofcPower`, `SolarConcentrator`, `GeothermalAquifer`, …) and water (`WaterTreatmentSystem`, `DeepWell`, …) per authority DM-4/DM-3. W3's contract: a read-only **seasonal multiplier provider** derived from the Year-of-Ash calendar day, consumed by the existing consumption/drawdown paths; the receiving owners keep their state. Exact verb cards for the chosen power/water owners are produced at P0 (PIR-3) because the program deliberately does not bind a specific power chain before the maintainer picks the first consumer.

### 7.4 Breakdown consequences (W4)
| Verb (live) | Signature / role |
|---|---|
| `EffectiveBreakdownRiskMultiplier` | `(core) float` on track gear — risk input (existing) |
| `Repair` / `RepairTrackGear` | `(core) ActionResult Repair(vehicleId, amount)` / `RepairTrackGear(vehicleId, amount)` — **counterplay** |
| Medical/dose receiving | `DiseaseSystem.TryInfect` (contamination), `DoseLedgerSystem.BookReading` (exposure), and the medical pipeline's injury intake (exact verb at P0) |
| Expedition family | `ExpeditionHostSession` (event surface), expedition save family, expedition day phase |

### 7.5 Defense × siege (W5)
`DefenseGrid`/perimeter values (panel-visible) → the faction-war/raid resolver. Resolver verb card at P0; stance authority remains `FactionStanceEngine`. W5 changes no defense math — it makes existing values *count* at the siege seam.

### 7.6 Migration × encounters (W6)
`SeasonalHumanMigrationEngine.RegisterPack/MigratePack/TickDay`, `MigrationConsequenceEngine` → travel-encounter selection (verb card at P0). Encounters remain authored data; migration only **conditions selection** through a read model.

### 7.7 Quest reopen (W7), gossip (W8), belief→stance (W9), rites→Reckoning (W10), one-shot primitive (W11)
- W7: quest owners (`QuestRuntimeCoordinator` per ledger, `PersonalQuestSystem`, `DynamicQuestGenerator` templates) + `IFlagLedger`; reopen predicate is data (`prereq_quest_id`, `min_day` patterns already exist in quest catalogs).
- W8: moral-choice state + rumor/info owners (`RumorSystem` under information flow per authority DM-8); `moral_choice_gossip.json` is the authored seed surface.
- W9: `FactionStanceEngine.ModifyTrust(factionId, delta)` is the only write; `belief_movements.json` is the authored input.
- W10: ritual owner (`memorial_rites.json`, `spiritual_rituals.json`) → `EnrollEvidence(int amount = 1)` on the Reckoning path; evidence-vocabulary check first (W10 P0).
- W11: new shared primitive `OneShotTriggerLedger` (name proposed) built on `IFlagLedger` persistence: `Arm(triggerId, dayThreshold, gateFlag)`, `TryFire(triggerId, day) → bool`, census; one-shot by construction, persisted with the flag family.

### 7.8 Balance refresh (W12)
Harness runs (seeded, deterministic) produce `docs/balance/` deltas for the newly sealed mechanics. No product-code retuning without a signed target (factory Lane C rule).

---

# 8. Wave packages (full integration contracts)

> Each wave below is an embeddable single-package plan: reality, delta, seams, architecture, ownership, flow, state, APIs, data, save, determinism, wiring, Godot, failure modes, tests, phases, files, risks, out-of-scope, rollback, DoD, handoff. Per-wave PIR and seal-tier checklists live in Appendices A and B and are part of each wave's exit.

## CORE-MECH-W1-FOODBORNE-DISEASE-BRIDGE — Wave 1 — Spoilage becomes illness through the disease authority

**Priority:** P1 · **Lane/cluster:** B (mechanics) · C3→C2 · **Seed:** SB-05/B-05 · **Tier:** MECHANIC · **Gate:** none — **this is the full-integration entry point**

### Why now
It is the clearest instance of the program's defect class in the most player-legible system: the game models food carefully — cohorts, curing jobs, shelf life that scales with storage temperature, spoilage counters, discard — and then lets the player eat through a path that never asks the preservation system what they are eating, and never lets the disease system answer. A player who hoards rotten rations is never punished; a player who cures correctly is never rewarded. The math exists; the consequence does not.

### Player value
Food becomes a decision with teeth: cure it properly, keep it cold, discard before it turns, or gamble and possibly infect the shelter with a foodborne disease that the ward must then treat.

### Current reality (verified)
- `FoodPreservationSystem` (`Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs`) — spoilage API live; **no** disease/contamination reference in the file.
- `ConsumeFood(foodItemId, neededCount, out spoiledConsumed)` exists; **no player-path caller** (the player's eat is `HoldfastRuntimeSession.ConsumeFoodResult(itemId, amount, survivorId?)`).
- `DiseaseSystem.TryExpose` / `TryInfect` have no foodborne caller; `disease_catalog.exposure_sources` is the authored vocabulary for exposure sources.

### Required delta
1. Data-first: author foodborne exposure-source rows in `disease_catalog` (pathogen per food class; probability band) — no code-authored diseases.
2. A thin exposure seam: when the player consumes food through the real eating path, determine consumed-spoilage from the preservation owner (spoiled share of the item), and raise a `TryExpose` context through the disease authority. The exposure probability derives from spoiled share × the authored source probability; the rng comes from the disease authority's existing stream.
3. Counterplay surfaced: `DiscardSpoiled` and cold storage must be visible to the player as the way to avoid exposure; journaling names the exposure and the counterplay.
4. No new state: preservation keeps its counters; disease keeps its infections; the bridge is an attributed, one-shot-per-consumption adapter.

### Architecture and ownership
Domain: `FoodPreservationSystem` (spoilage truth) + `DiseaseSystem` (infection truth). Host: `HoldfastRuntimeSession` eating path + existing disease host. Save: reuse preservation + disease families (no new section). UI: kitchen/food surface (bind; show spoilage and discard). RNG: disease stream. Observability: journal facts (`food.spoilage_exposure`, `food.discarded`, `disease.outbreak_risk_changed`).

### Data flow
```
Player eats (HoldfastRuntimeSession.ConsumeFoodResult)
  → preservation owner: spoiled share for the item (GetSpoiledFood / ConsumeFood accounting)
    → if spoiled share > 0: build DiseaseExposureContext (source = authored foodborne row)
      → DiseaseSystem.TryExpose (existing seeded stream, immunity respected)
        → outcome: infection / blocked (immunity) / no-effect
          → journal + UI readout; ward treats through existing medical owners
```

### Failure modes
- No preservation instance (not set up) → fail-closed: no exposure, eat proceeds on the inventory path; journal fact naming the missing owner.
- No authored foodborne row for a food class → no exposure for that class (fail-closed) and the gap is visible in the integrity census.
- Double counting: exposure must be raised **once per consumption event**, not per unit.
- Immunity: `TryExpose`/`TryInfect` already respect immunity — the bridge must not bypass it.
- Spoilage consumed unknowingly: the journal must say what was eaten and why exposure was possible (truthful UI copy, not a gotcha).

### Tests (focused, new file runs alone first)
- `FoodborneExposureBridgeTests` (new): eating spoiled stock can expose; eating clean stock cannot; discard before eating prevents exposure; immunity blocks; exactly-once per event.
- Regression: existing preservation + disease suites unchanged.
- `godot --headless --path . -- --disease-selftest` (or the live disease probe name re-verified at P0) stays green.

### Phases
| Phase | Work | Exit |
|---|---|---|
| P0 Premise | Re-verify the sweep rows for W1; paste live verb cards; claim exact paths (preservation host seam, disease catalog, eating path, focused test file) | PIR-1…PIR-10 record complete (Appendix A) |
| P1 Data | Author foodborne `exposure_sources` rows; integrity census | `--data-integrity-selftest` PASS |
| P2 Core/host seam | Thin exposure adapter on the real eating path; one-shot + attribution | compile + existing probes green |
| P3 Observability | Journal facts; kitchen/food surface shows spoilage + discard | route/panel gates if UI touched |
| P4 Verify | Focused tests + probes + manual script (eat spoiled → possible illness; discard → safe) | DoD |

### File impact map
| Action | File | Risk |
|---|---|---|
| MODIFY | `Assets/StreamingAssets/Data/disease_catalog.json` (foodborne exposure rows) | LOW (data-first) |
| MODIFY | `src/Host/HoldfastRuntimeSession.cs` (spoilage-aware exposure call, thin) | MED |
| MODIFY | `src/Main.Plans62_65.cs` or the owning Main partial (expose preservation to the eat path) | MED |
| MODIFY (if needed) | `src/UI` kitchen/food surface (spoilage + discard readout) | LOW |
| CREATE | `Ashfall.Core.Tests/Shelter/FoodborneExposureBridgeTests.cs` | LOW |
| **Do not touch** | `FoodPreservationSystem` math, `DiseaseSystem` math, `SaveSectionRegistry` | — |

### Risks / rollback
- Risk: exposure probability drifts balance → mitigated by authored data + W12 baseline; rollback = revert adapter + data rows.
- Risk: eating path is shared → thin, feature-flag-free, tested change; rollback is a single revert.

### Definition of Done
- [ ] Foodborne exposure rows authored and integrity-green
- [ ] Eating spoiled food can expose a survivor; clean food cannot
- [ ] Discard and cold storage are visible counterplay
- [ ] Exactly-once per consumption; immunity respected; fail-closed when owners absent
- [ ] Focused tests green; existing disease/preservation probes unchanged; no new save section
- [ ] WORKTREE claim released; handoff attached

## CORE-MECH-W2-DOSE-STORM-WINDOW — Wave 2 — Year-of-Ash windows bend radiation risk

**Priority:** P1 · **C2×C12** · **Seed:** B-03 · **Tier:** MECHANIC

### Why now / player value
Exposure is a flat number today. The campaign already has a fifteen-chapter spine with a mid-winter window (Day 190 alarm-clock event per the authority, Day 180–360 canon) and a Year-of-Ash event catalog. Seeding that window into dose booking turns radiation from a background tax into a scheduling decision: when you send someone out, how bad is the air, and is the antirad worth it.

### Current reality (verified)
- `DoseLedgerSystem.BookReading(survivorId, day, nominalMsv, source, highEnergyEvent, antiRadBefore, antiRadAfter, ISeededRng rng)` is the single booking entry; **no** Year-of-Ash/storm reference in the file.
- `year_of_ash_events.json` + `YearOfAshCatalogLoader.LoadEvents` provide the window vocabulary; a day→window read model is derivable without new state.
- Counterplay exists: `SetShieldingFactor`, `RecordAntiRadTreatment`, `Calibrate`.

### Required delta
1. A read-only day→fallout-window provider from the Year-of-Ash calendar (derived, not stored).
2. Booking seam: nominal mSv is conditioned by the window multiplier **upstream** of `BookReading` (the ledger's own math and its seeded roll stay untouched).
3. Surface: dose register shows the window band on each reading; journal names it.
4. Determinism: window lookup is a pure function of day + calendar rows.

### Ownership / flow / state
Dose ledger is the sole authority; the Year-of-Ash calendar is read-only input. No new save section (window is derived from the existing calendar save). UI: dose register surface. RNG: the ledger's existing stream. Journal: `dose.storm_window`.

### Failure modes
- Missing calendar rows → multiplier 1.0 (fail-closed neutral) + journal fact; no crash.
- Booking called with a stale day → window lookup uses the booking day parameter, not wall clock.
- Double conditioning → the multiplier is applied at exactly one call site; a test asserts single application.

### Tests / phases / files
- `DoseStormWindowTests` (new): inside vs outside window produce different bands for identical nominal; shielding/antirad still matter; missing calendar neutral; determinism two-pass.
- Phases: P0 premise/claims → P1 read model + data check → P2 booking seam → P3 surface/journal → P4 verify.
- Files: dose Main partial (thin), Year-of-Ash read-model helper (new, read-only), dose register surface (optional), new test file. Do not touch `DoseLedgerSystem` math.

### DoD
- [ ] Identical nominal exposure books different bands inside vs outside a window
- [ ] Shielding/antirad counterplay measurably changes the outcome
- [ ] Missing calendar degrades to neutral with a journal fact
- [ ] Focused tests green; no new save section; claim released

## CORE-MECH-W3-WINTER-PRESSURE-POWER-WATER — Wave 3 — The season gets teeth in power and water

**Priority:** P1 · **C12→C1/C3** · **Seed:** B-19 · **Tier:** MECHANIC

### Why now / player value
The Year of Ash is the campaign's spine, and power/water are the two systems that should feel it most. Today neither references the season: a Day-300 power crisis and a Day-30 one draw identically. W3 makes hardening, reserves, and filters matter exactly when the fiction says they do.

### Current reality (verified)
Zero `YearOfAsh` references in any `*Power*` or `*Water*` Core file. Power owners and water owners exist with their own catalogs, tuning, and save families (DM-3/DM-4). A day→season read model is derivable from the Year-of-Ash calendar.

### Required delta
1. A shared read-only seasonal-pressure provider (day → pressure band) built on the Year-of-Ash calendar.
2. First consumer pair (one power owner, one water owner) chosen at P0 from live evidence; consumption/drawdown paths multiply by the band through their existing authority (no new state, no parallel scalars).
3. Counterplay: existing hardening upgrades, reserves, and filter burn become the player's answer; the surfaces show the band.
4. Data-first: seasonal curve rows authored through the existing tuning/calibration catalogs where the owner supports them.

### Ownership / flow / state / determinism
Each consumer keeps its own authority and save; the provider is pure (day → band). No rng. Journal: per-owner winter pressure facts. No new save section.

### Failure modes
- Provider missing → band 1.0 neutral + journal; never a crash or a silent discount.
- Two owners double-multiply the same draw → one provider call per draw, asserted by test.
- Balance drift → W12 measures; rollback = revert consumer call sites.

### Tests / phases / files
- `WinterPressureProviderTests` (new): band curve by day; neutral fallback; two-pass determinism.
- `WinterPowerWaterConsumerTests` (new): identical inputs draw more inside the window; hardening offsets.
- Phases: P0 (choose the first two consumers from live evidence; claims) → P1 provider + data → P2 consumer seams → P3 surfaces/journal → P4 verify.
- Files: new read-model provider, the two chosen owners' thin seams, their surfaces (optional), new test files. Do not touch tuning math beyond the call site.

### DoD
- [ ] Day-300 draws measurably differ from Day-30 for the same inputs in both chosen owners
- [ ] Hardening/reserve counterplay demonstrably offsets the band
- [ ] Missing calendar is neutral with a journal fact; determinism two-pass green
- [ ] No new save section; no parallel scalars; claim released

## CORE-MECH-W4-BREAKDOWN-CONSEQUENCES — Wave 4 — Breakdowns hurt, maintenance matters

**Priority:** P1 · **C5→C2** · **Seed:** B-07 · **Tier:** MECHANIC

### Why now / player value
`ExpeditionVehicleSystem` already models condition, track gear, `EffectiveBreakdownRiskMultiplier`, and repairs — and a breakdown currently costs nothing beyond the expedition itself. Routing injury/exposure consequences into the medical/dose owners makes preflight inspection, track-gear investment, and abort decisions real.

### Current reality (verified)
`ExpeditionVehicleSystem.cs:55,179,263` — risk and repair live; no injury/exposure routing found. Medical and dose owners are complete receivers (W1/W2 patterns).

### Required delta
1. On a breakdown event, roll an outcome band (seeded, existing expedition stream): no injury / injury / contamination-exposure.
2. Injury routes to the medical pipeline's injury intake (verb card at P0); exposure routes to `DoseLedgerSystem.BookReading`; contamination routes to `DiseaseSystem.TryExpose` (reusing the W1 seam shape).
3. Counterplay: `Repair`/`RepairTrackGear` before departure measurably reduces the band (existing math, now consequential).
4. Exactly-once per breakdown event; the event is the guard.

### Failure modes
- Absent medical/dose owner → fail-closed + journal; expedition still resolves.
- Double application on save/load → event-id guard, asserted by test.
- Balance → W12.

### Tests / phases / files
- `BreakdownConsequenceTests` (new): each band routes to the right owner exactly once; repair reduces band; determinism two-pass.
- Phases: P0 (verb cards + claims) → P1 outcome band + routing → P2 surfaces/journal → P3 verify.
- Files: expedition Main/host thin seam, medical/dose receivers, new test file. Do not touch vehicle math.

### DoD
- [ ] A breakdown can injure, expose, or contaminate through the existing owners
- [ ] Pre-departure repair measurably reduces the consequence band
- [ ] Exactly-once under save/load; fail-closed when receivers absent; no new save section

## CORE-MECH-W5-DEFENSE-SEEGE-COUPLE — Wave 5 — Built defenses must matter at the siege

**Priority:** P2 · **C15×C7** · **Seed:** B-22 · **Tier:** FEATURE→MECHANIC

### Why now / player value
Perimeter and sky defenses are visible, costly player investments that currently do not enter raid/siege resolution. Wiring existing defense values into the faction-war resolver makes fortification a decision instead of decoration.

### Current reality (verified)
`DefenseGrid` is referenced only by `src/UI/DefenseGridPanel.cs` and `src/Main.Plans162_165.cs`; no siege consumer exists. Stance/raid authority lives with `FactionStanceEngine` (`GetRaidAggression`) and the faction-war resolver. Sky armor and orbital-harrow telemetry are separate existing systems.

### Required delta
1. A read-only defense-strength projection (perimeter integrity, sky armor) from the existing defense owners.
2. Siege/raid resolution consumes the projection to scale raid pressure/outcome through its existing authority — no new siege model.
3. Counterplay and feedback: defense values and their effect on raid outcomes are visible on the defense panel and in the journal.

### Failure modes
- Defense owner absent → neutral defense (no bonus, no penalty) + journal; never a crash.
- Double counting with stance aggression → one projection call per raid resolution, test-asserted.
- Balance → W12.

### Tests / phases / files
- `DefenseSiegeCouplingTests` (new): stronger defenses measurably reduce raid pressure; missing owner is neutral; determinism.
- Phases: P0 (resolver verb card + claims) → P1 projection → P2 resolver seam → P3 panel/journal → P4 verify.
- Files: defense projection helper (new, read-only), resolver seam, defense panel (thin), new test file.

### DoD
- [ ] Defense values measurably change siege/raid outcomes through the existing resolver
- [ ] Missing defense owner is neutral with a journal fact
- [ ] No new siege model, no new save section; claim released

## CORE-MECH-W6-MIGRATION-ENCOUNTER-BRIDGE — Wave 6 — The wasteland moves

**Priority:** P2 · **C14→C5** · **Seed:** B-21 · **Tier:** MECHANIC

### Why now / player value
Migration engines already move wildlife and human packs across sectors every day, and travel encounters are authored per destination. Today the two systems never meet: the ecosystem is a readout, not a pressure on the road. Conditioning encounter selection on migration corridors makes the map feel alive and gives scouting/timing real value.

### Current reality (verified)
`SeasonalHumanMigrationEngine` (`RegisterPack`, `MigratePack`, `TickDay`) and `MigrationConsequenceEngine` exist and are consumed by wildlife/harvest Main paths; travel-encounter selection (`ExpeditionHostSession`, `TravelEncounterSaveStore`, `travel_encounters.json`) has no migration input.

### Required delta
1. A read-only migration read model: sector → active packs/species and pressure.
2. Encounter selection consults the read model to bias (not replace) authored encounters — encounters stay authored data.
3. Counterplay: timing routes around corridors, avoiding them, or preparing for the encounters they raise.
4. No new state; migration keeps its save; the read model is derived.

### Failure modes
- Migration owner absent → identity weights (no bias) + journal.
- Encounters becoming unplayable under a heavy migration band → bias is bounded and test-asserted; the underlying encounter table remains the floor.
- Determinism: selection sorts candidates Ordinal before weighted pick.

### Tests / phases / files
- `MigrationEncounterBridgeTests` (new): a migration band biases selection within bounds; empty migration = baseline distribution; determinism two-pass.
- Phases: P0 (selector verb card + claims) → P1 read model → P2 selector seam → P3 travel surface/journal → P4 verify.
- Files: migration read-model helper (new), encounter selector seam, travel surface (thin), new test file.

### DoD
- [ ] Migration state measurably biases travel-encounter selection within authored bounds
- [ ] No migration = baseline distribution; determinism green
- [ ] No new state or save section; claim released

## CORE-MECH-W7-QUEST-REOPEN-GRAMMAR — Wave 7 — Discoveries can revive failed threads

**Priority:** P2 · **C10** · **Seed:** B-16 · **Tier:** FEATURE

### Why now / player value
Campaigns produce discoveries after quests have failed or been abandoned, and today those threads stay dead. The reopen grammar (a later-satisfying discovery revives a quest) is a documented design intent with no implementation — a quiet source of lost momentum.

### Current reality (verified)
No reopen logic exists in Core quest systems (searches find only narrative strings). Quest catalogs already carry prerequisite/day fields (`prereq_quest_id`, `min_day` patterns). Quest state and `IFlagLedger` are the existing owners.

### Required delta
1. A reopen predicate evaluated on discovery/flag changes against abandoned/failed quest records whose conditions are now satisfied.
2. Reopen routes through the existing quest owner (state transition + journal + flag), never a side queue.
3. Player visibility: the quest surface shows why a thread is eligible to reopen.

### Failure modes
- Infinite reopen loops → one reopen per quest id, recorded.
- Predicate drift from quest data → predicates are read from quest records, not hard-coded.
- Flag ordering on day tick → evaluation point is explicit and tested.

### Tests / phases / files
- `QuestReopenGrammarTests` (new): satisfied conditions reopen exactly once; unsatisfied do not; loops impossible; save/load safe.
- Phases: P0 (quest owner verb card + claims) → P1 predicate + transition → P2 surface/journal → P3 verify.
- Files: quest owner seam, `IFlagLedger` use, quest surface (thin), new test file.

### DoD
- [ ] A failed/abandoned quest reopens exactly once when a later discovery satisfies its conditions
- [ ] Reopen is visible and journaled; no side queue
- [ ] Save/load safe; no new save section; claim released

## CORE-MECH-W8-GOSSIP-PROPAGATION — Wave 8 — Choices travel

**Priority:** P2 · **C10→C8** · **Seed:** B-17 · **Tier:** MECHANIC

### Why now / player value
Moral choices currently land locally. The information-flow rules already model channels and the gossip catalog already carries seeds; what is missing is propagation over time and distance. A choice's rumor reaching the wrong faction changes the next interaction — the information game becomes real.

### Current reality (verified)
`moral_choice_gossip.json` is referenced only from `src/Main.MoralChoice.cs` (read-side). Rumor/info owners exist (`RumorSystem` per authority DM-8; `Main.Economy`/`RadioHostSession` consume rumor rules). No propagation engine.

### Required delta
1. A deterministic propagation step: seeded, time-lagged, distance-weighted, on existing channels — feeding the existing rumor/info owners.
2. Counterplay: suppression, channel control, and fast couriers (existing faction/expedition concepts) shorten or kill propagation.
3. Surface: the radio/journal surfaces show the rumor in the receiving faction's voice.

### Failure modes
- Runaway rumor economy → bounded per-source-per-window emission, test-asserted.
- Rumor double-count → one propagation entry point per day tick, idempotent per (source, channel, day).
- Coordinate with sealed radio owners: additive use of existing rumor authority only.

### Tests / phases / files
- `GossipPropagationTests` (new): propagation respects time/distance; suppression works; two-pass determinism; no duplication.
- Phases: P0 (rumor owner verb card + claims) → P1 propagation step → P2 surface/journal → P3 verify.
- Files: propagation step (Core or host-thin per owner placement decided at P0), existing rumor owner seam, surfaces (thin), new test file.

### DoD
- [ ] A choice's gossip propagates over time/distance along modeled channels
- [ ] Suppression and counters measurably change propagation
- [ ] Determinism two-pass green; no new rumor authority; claim released

## CORE-MECH-W9-BELIEF-STANCE-BRIDGE — Wave 9 — Beliefs move politics

**Priority:** P2 · **C9→C7** · **Seed:** B-14 · **Tier:** FEATURE

### Why now / player value
Survivors' beliefs move through authored `belief_movements.json`, and the faction stance engine is the single authority for standing — but the two have never been connected. Interiority should have political consequences.

### Current reality (verified)
`FactionStanceEngine` (`ModifyTrust`, `GetStance`, `GetRaidAggression`, …) is the sole standing authority; no `belief_movements` → stance bridge exists.

### Required delta
1. Belief-movement events translate into bounded `ModifyTrust` deltas through the stance engine — the only write path.
2. The faction surfaces show which beliefs move standing (legibility).
3. Counterplay: faction outreach, rituals, and policy change beliefs (existing systems) to move standing back.

### Failure modes
- Trust runaway → clamped deltas and a per-day bound, test-asserted.
- Belief/stance coupling loops → monotonic guards; no feedback without a source.

### Tests / phases / files
- `BeliefStanceBridgeTests` (new): belief movement shifts stance through the engine; clamps hold; determinism.
- Phases: P0 (stance/faction verb card + claims) → P1 translation table (data-first) → P2 seam + surface → P3 verify.
- Files: translation data (authored), thin bridge, faction surface (thin), new test file.

### DoD
- [ ] Belief movement measurably shifts faction standing through `FactionStanceEngine`
- [ ] Deltas bounded; no belief-side trust cache
- [ ] No new save section; claim released

## CORE-MECH-W10-RITES-RECKONING-EVIDENCE — Wave 10 — Memorial acts leave a trace

**Priority:** P3 · **C9→C13** · **Seed:** B-15 · **Tier:** FEATURE (evidence-vocabulary check at P0)

### Why now / player value
Memorial rites and spiritual rituals are authored and performable; at the Reckoning they currently leave no mark. Enrolling performed rites as evidence ties survivor interiority to the endgame record — the campaign's memory of who you were.

### Current reality (verified)
`memorial_rites.json` / `spiritual_rituals.json` exist; `EnrollEvidence(int amount = 1)` exists on the Reckoning path with **no** memorial-rite caller. The evidence vocabulary must be checked for a rite class before authoring.

### Required delta
1. Vocabulary check: confirm the Reckoning evidence vocabulary admits a rite fragment (or propose the smallest additive vocabulary change).
2. Performed rites enroll through `EnrollEvidence` exactly once per rite performance.
3. The memorial surface and the standing record show the enrollment.

### Failure modes
- Vocabulary cannot admit a rite → stop and propose the minimal vocabulary amendment (no silent skip).
- Double enrollment → performance-id guard.

### Tests / phases / files
- `RitesReckoningTests` (new): a performed rite enrolls once; evidence totals move; save/load safe.
- Phases: P0 (vocabulary check — a hard gate; claims) → P1 enrollment seam → P2 surface → P3 verify.
- Files: ritual owner seam, Reckoning seam, memorial/standing surfaces (thin), new test file.

### DoD
- [ ] Performed rites enroll as Reckoning evidence exactly once
- [ ] Evidence vocabulary check completed and recorded
- [ ] No duplicated rite state; claim released

## CORE-MECH-W11-ONE-SHOT-TRIGGER-PRIMITIVE — Wave 11 — The missing "once, on day D, if gate G" primitive

**Priority:** P2 (infrastructure; benefits W8/W10 and future waves) · **cross-cutting** · **Seed:** B-35 · **Tier:** GAP

### Why now / player value
W8 and W10 both need "fire exactly once, on or after day D, if a gate holds" and today there is no shared primitive for it — every wave reinvents a local version. A small persisted primitive removes a whole class of duplication and makes one-shot semantics testable in one place.

### Current reality (verified)
`IFlagLedger` provides `IsSet/Set/Clear/GetCounter/Increment` — flags and counters, but no day-thresholded one-shot trigger. No general one-shot trigger type exists in Core.

### Required delta
1. `OneShotTriggerLedger` (proposed name) with `Arm(triggerId, dayThreshold, gateFlag)`, `TryFire(triggerId, day) → bool`, census; one-shot by construction; persisted with the flag family.
2. Determinism: pure function of (armed, day, gate) — no rng.
3. Adopted by W8 (gossip seeds) and W10 (rite enrollments) to prove the primitive in anger.

### Failure modes
- Persistence drift with `IFlagLedger` → rides the existing flag save; no second store.
- Arming after firing → re-arm is an explicit operation, test-asserted.

### Tests / phases / files
- `OneShotTriggerLedgerTests` (new): fires once, respects day threshold and gate, survives save/load, re-arm explicit.
- Phases: P0 (flag persistence seam + claims) → P1 primitive + persistence → P2 adopt in W8/W10 → P3 verify.
- Files: new Core primitive (small), flag-family save touchpoint, new test file.

### DoD
- [ ] One-shot trigger primitive exists, persists, and is adopted by at least W8 and W10
- [ ] No second flag store; determinism pure
- [ ] Claim released

## CORE-MECH-W12-BALANCE-BASELINE-REFRESH — Wave 12 — Measure what we sealed

**Priority:** P3 (program closer) · **Lane C** · **Seed:** SB-06 / F-006 · **Tier:** PROGRAM

### Why now / player value
W1–W4 change exposure, disease, power, water, and expedition outcomes. Without a refreshed deterministic baseline, their balance claims are unverifiable and later tuning debates have no common reference. W12 runs the seeded harness, publishes deltas, and files any needed retune as a separate signed target (never an inline retune).

### Current reality
Balance baselines exist for expeditions/vehicles (`EXPEDITION_BALANCE_BASELINE.md`, `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md`, `ECONOMY_FAIRNESS_AUDIT.md` per authority DR-03); the post-W1–W4 refresh does not.

### Required delta
1. Deterministic harness runs (seeded, paired) across the newly sealed mechanics; deltas published under `docs/balance/`.
2. Any retune target filed as a separate, signed proposal — W12 changes no tuning numbers itself.

### Failure modes
- Harness drift → pin seeds and inputs; compare against the pre-wave baseline commit.
- Scope creep into tuning → hard stop at "file the target".

### Tests / phases / files
- Harness runs + report; no product-code changes.
- Phases: P0 (baseline commit + harness selection) → P1 paired runs → P2 report + filed targets → P3 ledger close.

### DoD
- [ ] Refreshed baseline published for W1–W4 mechanics
- [ ] Any retunes filed as separate signed targets; no inline tuning
- [ ] Program closeout attached to the ledger

---

# 9. Program-level save, determinism, and test strategy

1. **No new save sections.** The program reuses existing families (preservation/disease, dose, power/water, expedition/medical, faction, quest, reckoning, flags). If a wave proves a new section unavoidable, it is a PIR-4 escalation to the foreman, never an integrator decision.
2. **Restore discipline:** schema-gated restore everywhere; missing sections fall back to defaults; old saves never crash.
3. **Exactly-once day-tick consequences:** guard `(owner, subject, day, source)`; proven by save/load tests.
4. **Determinism:** seeded streams only; two-pass replay per new consequence; no wall-clock, no `System.Random`, no hash-order dependence.
5. **Focused testing only:** each wave names its test file(s); new files run alone first; `scripts/run_test.sh` caps honored; no full-suite runs.
6. **Probe discipline:** every wave keeps (or adds) a CLI probe at 12 checks; probe green is necessary, never sufficient.

---

# 10. Program Definition of Done

1. W1–W4 sealed at MECHANIC tier with focused evidence and manual scripts.
2. W5–W10 sealed at their declared tiers; W11 adopted by W8 and W10; W12 baseline published.
3. Every wave passed PIR-1…PIR-10 with a scrapbook record; every seam classified in the port contract (0 unbound).
4. Zero new save sections; pin 266 unless an unrelated wave moves it (re-read, never assumed).
5. Zero parallel ledgers; zero unbound consequences; every mechanic has counterplay.
6. Factory Step-5 continuity checklist and Step-6 evidence labels honored in every wave handoff; Step-7 backlog delta recorded.
7. Integration ledger updated by the foreman/integrator on wave close; this plan claims no ledger authority.

---

# Appendix A — Pre-Integration Readiness Gate (PIR-1…PIR-10)

No wave code lands before its PIR record — one row per gate, pasted into the claim/handoff — passes. A failed row stops the wave; nothing is “fixed while integrating”.

| # | Gate | Evidence required | On fail |
|---|---|---|---|
| PIR-1 | Evidence freshness | `git rev-parse HEAD` recorded; if HEAD moved past `1678c074`, re-run the §2.2 sweep rows for the wave | re-verify verb tables before coding |
| PIR-2 | Ownership | exact paths claimed in `WORKTREE_OWNERSHIP.md`; shared hubs free or mutex-held; never two hub-holding waves at once | wait / hand to integrator |
| PIR-3 | API copy | live verb signatures pasted from the owner file into the claim; divergence from §7 → revision note first (Rule 7) | reconcile plan vs code |
| PIR-4 | One-owner pick | every consequence names its receiving owner (Disease / DoseLedger / medical / Needs / FactionStance / Reckoning / encounter selector); any new save section escalates here | redesign |
| PIR-5 | Save plan | reuse confirmed (no new section); restore path unchanged; exactly-once guard named | redesign |
| PIR-6 | Determinism plan | rng stream named or “none required”; no `System.Random`/wall-clock; Ordinal sorts where order reaches a hash/list | redesign |
| PIR-7 | Test plan | focused test file(s) named; new files run alone first; `scripts/run_test.sh` targets under TEST_POLICY caps | narrow scope |
| PIR-8 | UI plan | bind-vs-new-route decided; new route ⇒ panel + registry + `Main.GameFlow` + `Main.PlayerSurfaces` + manifest listed; a11y rows included | split wave |
| PIR-9 | Baseline green | pre-change compile + the wave’s existing focused tests/probe pass **before** edits | establish baseline |
| PIR-10 | Rollback slice | first commit slice small and reversible; rollback section executable as written | shrink slice |

**Per-wave hot spots:** W1 → PIR-4 (disease is the only infection writer) and PIR-6 (exposure rng); W2 → PIR-6 (window lookup must not re-roll); W3 → PIR-4 (which power/water owners consume first; exactly one multiplier per draw); W4 → PIR-5 (event-id exactly-once under save/load); W5/W6 → PIR-4 (single resolver/selector write); W8 → PIR-2 (radio/info owners may be claimed); W10 → PIR-4 (evidence vocabulary check is a hard gate); W11 → PIR-5 (rides the flag save).

**Pre-integration dry run (UI waves):** ① build Core+host → ② focused tests → ③ CLI probe (`godot --headless --path . -- --<wave>-selftest`; 15 FPS for manual sessions) → ④ panel route gates if UI touched → ⑤ save round-trip → ⑥ manual player script.

---

# Appendix B — Seal quality framework and per-wave tiers

**B1. Tiers.** GAP = unconnected mechanic becomes reachable (owner + host + save + day owner + probe agree). FEATURE = player-operable on the right owner. MECHANIC = changes decisions (pressure + consequence in an existing owner + counterplay). A mechanic is not done when the command works but when the consequence is observable elsewhere and the player can answer it.

**B2. Meaningfulness pentad** (all five for FEATURE; + counterplay for MECHANIC): **Agency** (player initiates or the world acts legibly) · **Legibility** (state readable in a surface) · **Consequence** (an existing owner changes) · **Persistence** (survives save/load) · **Feedback** (journal/readout) · **Counterplay** (the pressure can be answered).

**B3. Declared tiers.**
| Wave | Tier | Counterplay the tier rests on |
|---|---|---|
| W1 Foodborne | MECHANIC | cure, cold storage, discard before eating |
| W2 Dose window | MECHANIC | shielding factor, antirad timing, dosimeter calibration, shelter during windows |
| W3 Winter power/water | MECHANIC | hardening, reserves, filters |
| W4 Breakdown | MECHANIC | preflight repair, track-gear investment, abort decisions |
| W5 Defense siege | FEATURE→MECHANIC | fortify vs raid; sky armor vs orbital telemetry |
| W6 Migration | MECHANIC | route timing, scouting, preparation |
| W7 Quest reopen | FEATURE | pursue the discovery that revives the thread |
| W8 Gossip | MECHANIC | suppression, channel control, couriers |
| W9 Belief stance | FEATURE | outreach, rituals, policy move beliefs back |
| W10 Rites evidence | FEATURE | the rite itself is the act; standing record is the payoff |
| W11 One-shot primitive | GAP | — (infrastructure) |
| W12 Balance refresh | PROGRAM | — (measurement) |

**B4. Checklists.**
- *Gap:* session constructed · section captured/restored · day owner wired (if stateful) · probe green · port classified.
- *Feature:* surface registered · controls map to §7 verbs · UI shows Core truth only · manifest updated · manual script passes.
- *Mechanic:* consequence lands in an existing owner · exactly-once + clamps documented · polarity/determinism test · counterplay proven manually · no parallel ledger (`rg` assertion).

**B5. Anti-patterns that fail the seal:** probe-only as Done · a shadow ledger for “convenience” · offline UI probability math · computed-then-dropped results (the class this program exists to close) · balance retuned inline instead of filed as a signed target · a wave widened to reach a character target (factory invariant).

---

# Appendix C — Backlog, decision packets, and backlog delta (Factory Step 7)

**C1. Deferred to sibling owners.** B-23 difficulty consumer binding → PFGL master W7 / `CF-XP01` (coordinate, do not duplicate). B-01/B-02 shelter-failure cascade and grid seal follow-through → backlog until their quarantine/seal exit criteria are read in-session. B-12 market-rumor commodity bands → data extension, not mechanics. B-25 rescue-remains medical → additive on sealed distress owners, coordinate first. B-04 child-health cohort bridge → real, but overlaps the sealed medical flagship set; medium value.

**C2. Out (no signature).** B-09 flooded-route topology · B-10 faction-war per-strike emitter · B-11 black-market funds legs · B-33 intake advisory (sealed premise) · anything behind quarantine drain, XP-04/06, EN-01…08, string freeze.

**C3. Decision packets for the foreman (each cheaper than the wave it gates).**
- **DP-CM-1 (W3 first consumers):** which power chain and which water owner consume the winter band first? Recommendation: the two owners whose Day-180+ draw is already visible in the 30-day reports; evidence exists in `docs/` baselines; default if silent: water first (filter burn is the most legible).
- **DP-CM-2 (W5 resolver):** siege/raid resolution authority — confirm the faction-war resolver as the only consumer of defense projections. Recommendation: yes (one resolver; no second siege model).
- **DP-CM-3 (W8 placement):** propagation step placement — Core (deterministic, testable) vs host-thin. Recommendation: Core, seeded, because the authority’s determinism rules bind Core behavior.
- **DP-CM-4 (W10 vocabulary):** if the Reckoning evidence vocabulary cannot admit a rite fragment, approve the minimal additive vocabulary change. Recommendation: amend vocabulary; never skip silently.
- **DP-CM-5 (W11 adoption order):** W8 and W10 adopt the one-shot primitive; confirm ordering. Recommendation: W8 first (it is scheduled earlier and has the harder propagation semantics).

**C4. Backlog delta recorded (Step 7).** Consumed: SB-05, SB-06, B-03, B-05, B-07, B-14, B-15, B-16, B-17, B-19, B-21, B-22, B-35. Corrected: B-08 (stale counts), B-23 (owner reassigned), B-33 (sealed premise). Added: none — the factory’s anti-nonsense filter is honored (candidates without a verified live premise are backlogged, not promoted). Deferred: B-01, B-02, B-04, B-12, B-25. Rejected: B-09, B-10, B-11 (no signature).

---

# Appendix D — Live verification log (premise sweep, this revision)

| Probe | Result | Used for |
|---|---|---|
| `RegisterContaminationAdvisory` callers | `PiezometerHostSession.cs:117` calls it; `KNOWN_DEBT.md:44` RETIRED | B-33 discard |
| Catalog counts | `scavenging_tables` 54 · `expeditions` 75 · `travel_encounters` 57 | B-08 stale |
| `FoodPreservationSystem` disease refs | none | W1 gap |
| `ConsumeFood` player-path callers | none (player eats via `HoldfastRuntimeSession.ConsumeFoodResult`) | W1 seam |
| `DiseaseSystem` entry points | `TryExpose:437` · `TryInfect:491` · `TriggerOutbreak:654` · `TickDaily:737` | W1/W4 receivers |
| `disease_catalog` exposure vocabulary | `exposure_sources` (`:347,384,411`) | W1 data-first |
| `DoseLedgerSystem` season refs | none | W2 gap |
| `BookReading` signature | `(survivorId, day, nominalMsv, source, highEnergyEvent, antiRadBefore, antiRadAfter, ISeededRng)` | W2 seam |
| `YearOfAshCatalogLoader.LoadEvents` | present (`:207`); `year_of_ash_events.json` live | W2/W3 source |
| `*Power*`/`*Water*` YearOfAsh refs | none | W3 gap |
| `ExpeditionVehicleSystem` consequence routing | risk/repair present; no injury/exposure | W4 gap |
| `DefenseGrid` consumers | UI panel + `Main.Plans162_165` only | W5 gap |
| Migration engines' consumers | wildlife/harvest paths; not travel encounters | W6 gap |
| Quest reopen logic | none in Core | W7 gap |
| `moral_choice_gossip` consumers | `Main.MoralChoice.cs` (read-side) only | W8 gap |
| `FactionStanceEngine` verbs | `GetTrust/ModifyTrust/GetStance/WillTrade/GetRaidAggression` | W9 receiver |
| `EnrollEvidence` callers | Reckoning-internal; no memorial-rite caller | W10 gap |
| One-shot trigger primitive | absent; `IFlagLedger` has flags/counters only | W11 gap |
| Pins | save 266 · surfaces 219/58/161 · port seams 307 | baseline |
| Claims | all `WORKTREE_OWNERSHIP.md` rows DONE/HANDED_OFF as of 2026-09-24 | Wave-1 claim available |

**Residual unknowns (honest, each becomes a P0 task):** the exact W4 medical-injury intake verb; the W3 first consumers; the W5 siege resolver; the W6 encounter selector; the W10 evidence vocabulary; whether W8 propagation belongs in Core (DP-CM-3).

---

# Appendix E — Manual player scripts (15 FPS sessions)

**W1:** eat a spoiled ration → possible illness through the ward; discard first → safe; cure + cold store → safe longest.
**W2:** identical exposure inside vs outside a storm window → different dose band; shielding/antirad changes the outcome.
**W3:** Day-30 vs Day-300 power/water draw for identical inputs; hardening/reserves offset; missing calendar = neutral.
**W4:** force a breakdown with a weak vehicle → injury/exposure lands; repair beforehand → band drops.
**W5:** raid with strong vs weak defenses → measurably different pressure; missing defense owner = neutral.
**W6:** route through an active migration corridor → encounter mix shifts within bounds.
**W7:** fail a quest, then satisfy its discovery condition → the thread reopens exactly once.
**W8:** make a choice; watch the gossip travel to a distant channel over days; suppress it and watch it die.
**W9:** move a belief → faction standing shifts through the stance engine; outreach moves it back.
**W10:** perform a rite → Reckoning evidence and standing record update exactly once.
**W11:** arm a trigger, pass its day with the gate closed, open the gate → it fires once; save/load; re-arm explicitly.

---

# Appendix F — Focused test matrix (create only when implementing; new files run alone first)

| Wave | Suggested test file | Covers |
|---|---|---|
| W1 | `Ashfall.Core.Tests/Shelter/FoodborneExposureBridgeTests.cs` | exposure from spoilage, counterplay, exactly-once, fail-closed |
| W2 | `Ashfall.Core.Tests/Medical/DoseStormWindowTests.cs` | window banding, counterplay, neutral fallback, determinism |
| W3 | `Ashfall.Core.Tests/Shelter/WinterPressureProviderTests.cs` + consumer tests | band curve, single-application, consumer deltas |
| W4 | `Ashfall.Core.Tests/Expeditions/BreakdownConsequenceTests.cs` | band routing, repair effect, save/load exactly-once |
| W5 | `Ashfall.Core.Tests/Defense/DefenseSiegeCouplingTests.cs` | projection effect, neutral fallback |
| W6 | `Ashfall.Core.Tests/Ecology/MigrationEncounterBridgeTests.cs` | bias bounds, baseline, determinism |
| W7 | `Ashfall.Core.Tests/Quests/QuestReopenGrammarTests.cs` | reopen-once, loop prevention, save/load |
| W8 | `Ashfall.Core.Tests/Narrative/GossipPropagationTests.cs` | time/distance, suppression, determinism |
| W9 | `Ashfall.Core.Tests/Factions/BeliefStanceBridgeTests.cs` | stance shift, clamps, no cache |
| W10 | `Ashfall.Core.Tests/Endgame/RitesReckoningTests.cs` | enroll-once, totals, save/load |
| W11 | `Ashfall.Core.Tests/Flags/OneShotTriggerLedgerTests.cs` | one-shot, threshold, gate, persistence, re-arm |
| W12 | (harness reports, no new test file) | seeded paired runs and deltas |

---

# Appendix G — Wave-by-wave phase summary (dependency-ordered execution sheet)

| Wave | P0 | P1 | P2 | P3 | P4 |
|---|---|---|---|---|---|
| W1 | premise + claim | data (foodborne rows) | exposure seam | journal/surface | verify |
| W2 | premise + claim | window read model | booking seam | surface | verify |
| W3 | premise + choose consumers | provider + data | consumer seams | surface | verify |
| W4 | premise + verb cards | outcome band + routing | — | surface | verify |
| W5 | premise + resolver card | projection | resolver seam | panel | verify |
| W6 | premise + selector card | read model | selector seam | surface | verify |
| W7 | premise + quest card | predicate + transition | surface | — | verify |
| W8 | premise + rumor card + DP-CM-3 | propagation | adoption of W11 | surface | verify |
| W9 | premise | translation data | seam | surface | verify |
| W10 | premise + **vocabulary gate** | enrollment | surface | — | verify |
| W11 | premise + flag seam | primitive + persistence | adoption | — | verify |
| W12 | baseline commit | paired runs | report + filed targets | — | ledger close |

---

# Appendix H — Program risk register

| Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|
| Balance drift from W1–W4 consequences | medium | medium | data-authored bands; W12 measurement; retunes filed separately |
| Exactly-once violations under save/load | medium | high | `(owner, subject, day, source)` guards proven by round-trip tests |
| Seams claimed by concurrent agent | medium | medium | PIR-2 claim before code; hub mutex; handoff releases |
| Determinism regression | low | high | two-pass replay per new consequence; no wall-clock/`System.Random` |
| Scope creep into tuning or narrative | medium | medium | B5 anti-patterns; signed-target rule; lane discipline |
| New save section pressure | low | high | PIR-4 escalation; reuse-first proof required |
| Factory premise staleness (authority vs live) | high | medium | §2.2 sweep re-run at every wave P0; live wins |

---

# Appendix I — Maintenance and revision rules

1. Revise when evidence HEAD moves across the listed owners, the save pin changes, or a wave closes (addenda only).
2. Any revision touching §7 must re-probe the affected verbs live and append a drift row to Appendix D.
3. Stale verb names in this plan are integration failures deferred to the implementer — the factory’s Rule 7 discipline applies (live source wins).
4. Character-growth honesty: expansion is welcome only as verified evidence (sweeps, cards, runbooks); padding is forbidden (authority Part 0 scale-honesty clause).
5. The plan claims no ledger authority: `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `docs/governance/DECISION_REGISTER.md` are foreman/integrator surfaces updated at execution time.

---

# Appendix J — Deep implementation runbooks (W1–W6)

> Runbooks are the implementer's reading order, not new requirements. Every symbol named here was probed this revision unless marked **[P0-VERIFY]**, which means the implementer re-reads the live signature before editing (PIR-3).

## J.1 W1 Foodborne disease bridge — runbook

**Read first (in order):**
1. `Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs` — `ConsumeFood(string foodItemId, int neededCount, out int spoiledConsumed)`, `GetSpoiledFood(string? foodItemId = null)`, `DiscardSpoiled(string? foodItemId = null)`, `StorageTempShelfLifeFactor(float)`.
2. `src/Host/HoldfastRuntimeSession.cs` — `ConsumeFoodResult(string itemId, int amount = 1, string? survivorId = null)` (the real eat path) and the inventory/nutrition owner it already routes through.
3. `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` — `TryExpose(DiseaseExposureContext)`, `TryInfect(...)`, immunity verbs, `TickDaily(...)`.
4. `Assets/Ashfall.Core/Disease/DiseaseCatalog.cs` — `exposure_sources` rows and `GetExposure(...)` to learn the authored shape **[P0-VERIFY field names]**.

**Decide first (PIR-4):** where the spoiled-share → probability translation lives. Recommended: a small pure helper owned by the *host seam* (not Core-Core cross-domain), computing `probability = clamp(baseFromCatalog * spoiledShare, 0, max)` with `baseFromCatalog` from the authored foodborne row. Rationale: preservation stays ignorant of disease; disease stays ignorant of preservation; the seam owns only the arithmetic and the call.

**Write order:**
1. **Data first.** Add foodborne `exposure_sources` rows to `disease_catalog.json` (one per food class you can justify: raw/undercooked, spoiled preserved stock, etc.). Run `--data-integrity-selftest`. Do not code a disease id that is not in the catalog.
2. **Seam.** In the eat path, after a successful consume: query the preservation owner for the item's spoiled share; if `spoiledShare > 0`, build the exposure context and call `TryExpose`. Pass the disease authority's existing rng through its existing API; do **not** construct a new rng at the seam.
3. **Exactly-once.** Raise exposure once per `ConsumeFoodResult` call, not per unit. If the eat can be partially fulfilled, compute the spoiled fraction of the *consumed* amount and make a single call.
4. **Fail-closed.** If the preservation owner is null (not set up in this game mode), skip exposure and emit a journal fact naming the missing owner — never fabricate a "safe" meal silently.
5. **Counterplay surface.** On the existing food/kitchen surface (bind, do not fork a new panel unless the surface does not exist **[P0-VERIFY]**): show current spoiled counts, expose `DiscardSpoiled`, and show the storage temperature. These are the player's answers to the pressure this wave creates.
6. **Journal.** Facts: `food.spoilage_exposure` (survivor, food class, spoiled share, outcome: exposed/blocked/none), `food.discarded` (item, count).

**Verify:**
- New file `Ashfall.Core.Tests/Shelter/FoodborneExposureBridgeTests.cs` — run alone first: (a) spoiled → exposure possible; (b) clean → no exposure; (c) discard before eating → no exposure; (d) immune survivor → blocked; (e) two units consumed → one exposure call; (f) preservation owner absent → no exposure + fact.
- Re-run existing preservation and disease suites unchanged; run the live disease CLI probe name **[P0-VERIFY]**; panel gates only if a route changed.

**Rollback:** revert the seam lines and the catalog rows; nothing else to unwind (no state, no save).

## J.2 W2 Dose storm window — runbook

**Read first:** `Assets/Ashfall.Core/DoseLedgerSystem.cs` (`BookReading` signature, `AssignDosimeter`, `SetShieldingFactor`, `RecordAntiRadTreatment`, `BandFor`, `GetCumulative`); the **caller** of `BookReading` in the medical Main partial **[P0-VERIFY]** (this is where nominal mSv is currently produced — the conditioning belongs upstream of the call, not inside the ledger); `Assets/Ashfall.Core/YearOfAsh/YearOfAshCatalogLoader.cs` (`LoadEvents` at `:207`); `year_of_ash_events.json` (window vocabulary).

**Design:** a pure `FalloutWindowProvider` (day → band) derived from the calendar; the medical call site multiplies `nominalMsv` by the band **once** before `BookReading`. The ledger's own seeded roll, anti-rad timing, and band logic are untouched.

**Write order:** provider (read-only, no state) → data check (window rows exist for the campaign span; if not, band is neutral 1.0 and the gap is filed) → call-site conditioning with a single multiplication → dose register surface shows the band on the reading → journal `dose.storm_window`.

**Verify:** `DoseStormWindowTests` — inside vs outside window with identical nominal produces different bands; shielding/antirad still change outcomes; missing calendar → neutral + fact; two-pass determinism. Run the live dose probe **[P0-VERIFY]**; confirm no other `BookReading` call site is left unconditioned (one multiplication site, asserted by grep).

**Rollback:** revert the one multiplication and the provider file.

## J.3 W3 Winter pressure — runbook

**Read first:** the Year-of-Ash calendar provider pattern (same as W2 — share the provider, do not fork a second one); the two chosen consumers **[DP-CM-1]**; each consumer's consumption/drawdown path and its existing tuning catalog.

**Design:** one shared seasonal-pressure provider; two consumers multiply their draw by the band at exactly one call site each. If a consumer's draw is event-shaped rather than daily, the band applies to the event's cost, not a timer.

**Write order:** shared provider → data (seasonal curve rows in the owners' existing tuning catalogs, data-first) → consumer seams (one multiplication each, with a comment naming the band source) → surfaces show the band and the offset that hardening gives → journal facts per consumer per day the band changes.

**Verify:** provider tests (curve by day; neutral fallback; two-pass determinism) + consumer tests (Day-30 vs Day-300 draw for identical inputs differs in the expected direction; hardening offsets within tolerance). Guard against double multiplication with a test that asserts the band appears exactly once in the consumer path (behavioral, not grep-only).

**Rollback:** revert the two call sites; provider may remain unused or be removed.

## J.4 W4 Breakdown consequences — runbook

**Read first:** `ExpeditionVehicleSystem` (`EffectiveBreakdownRiskMultiplier`, `Repair`, `RepairTrackGear`, and the breakdown occurrence site **[P0-VERIFY]** — find the actual event that fires on a failed roll); the medical injury intake verb **[P0-VERIFY]** (the W1 sweep named disease/dose receivers, not the injury intake); `DoseLedgerSystem.BookReading`; `DiseaseSystem.TryExpose`.

**Design:** on breakdown, resolve an outcome band from a seeded roll (no injury / injury / contamination-exposure) using the expedition's existing stream; route each band to its owner; emit one journal fact carrying the event id.

**Write order:** band resolution (pure function, unit-testable) → routing seams → exactly-once guard keyed on the breakdown event id → expedition surface shows preflight risk and the consequence history → journal.

**Verify:** `BreakdownConsequenceTests` — each band routes once; a save/load between breakdown and treatment does not re-route; repair before departure lowers the band; missing receivers fail closed with a fact. Run the live expedition probes **[P0-VERIFY]**.

**Rollback:** revert band + routing; vehicle math untouched.

## J.5 W5 Defense siege — runbook

**Read first:** the defense owners and their value accessors **[P0-VERIFY]**; the raid/siege resolver **[P0-VERIFY, DP-CM-2]**; `FactionStanceEngine.GetRaidAggression` to see how raid pressure is currently formed.

**Design:** a read-only defense projection (perimeter integrity × relevant defenses, sky armor separately) consumed by the resolver at one point. Do not rebalance defenses; do not add a second siege model.

**Write order:** projection (read-only) → resolver seam (one consumption point) → defense panel readout (what the projection means for the next raid) → journal.

**Verify:** `DefenseSiegeCouplingTests` — stronger defense measurably reduces raid pressure/outcome severity; missing owner neutral; determinism. Two-pass replay of a raid day.

**Rollback:** revert the resolver seam.

## J.6 W6 Migration encounters — runbook

**Read first:** `SeasonalHumanMigrationEngine` (`RegisterPack`, `MigratePack`, `TickDay`) and `MigrationConsequenceEngine`; the travel-encounter selection site in `ExpeditionHostSession` **[P0-VERIFY]**; `travel_encounters.json` (authored floor).

**Design:** sector read model (active packs, species, pressure); selection applies a bounded bias to authored encounter weights; the floor distribution is preserved when the model is empty.

**Write order:** read model → bounded bias at the selection site (sort candidate ids Ordinal before any weighted pick) → travel surface shows corridor state → journal.

**Verify:** `MigrationEncounterBridgeTests` — bias shifts selection within bounds; empty migration reproduces baseline distribution over a seeded sample; determinism two-pass.

**Rollback:** revert the bias.

---

## J.7 W7 Quest reopen — runbook

**Read first:** the quest owner that holds failed/abandoned records **[P0-VERIFY: `QuestRuntimeCoordinator` per the ledger]**; catalog fields `prereq_quest_id` / `min_day` in quest records; `IFlagLedger` (`IsSet`, `Set`, `Increment`).

**Design:** a reopen predicate (record-level: status in {failed, abandoned} ∧ prerequisite flags now set ∧ min_day reached ∧ not already reopened) evaluated on the discovery/flag change point and on day open; transition through the quest owner's own state machine.

**Write order:** predicate (pure, data-driven) → evaluation hook at the flag-change point and day open → one-shot guard (record carries the reopen fact via a flag id) → quest surface shows eligibility + reason → journal.

**Verify:** `QuestReopenGrammarTests` — exactly-once reopen; no reopen when unsatisfied; no loops across repeated days; save/load safe; flag ordering at day open deterministic.

**Rollback:** remove the evaluation hook; predicate is inert.

## J.8 W8 Gossip propagation — runbook

**Read first:** the moral-choice state that records choices **[P0-VERIFY]**; `moral_choice_gossip.json` shape; the existing rumor/info owner (`RumorSystem` per authority DM-8) and its deterministic band rules **[P0-VERIFY]**; `DP-CM-3` placement decision.

**Design:** a deterministic propagation step: for each pending gossip seed, compute channel distance and elapsed days; emit into the existing rumor owner when the band opens; support suppression (existing control concepts) as counterplay. Emit at most one propagation per (source, channel, window).

**Write order:** propagation step (seeded; Ordinal ordering) → adoption of the W11 one-shot primitive for seed arming (or plain flag guard if W11 has not landed) → existing rumor owner seam → surfaces (radio/journal) show arrival in the receiver's voice → journal.

**Verify:** `GossipPropagationTests` — distance/time gating; suppression; two-pass determinism; no duplicate emission per window; empty gossip = no change.

**Rollback:** remove the propagation step call; seeds remain data.

## J.9 W9 Belief stance — runbook

**Read first:** `FactionStanceEngine` (`ModifyTrust(factionId, delta)`, `GetStance`, `GetRaidAggression`); `belief_movements.json` shape; where belief movements are currently observed **[P0-VERIFY]**.

**Design:** a data-authored translation table (belief movement id → bounded trust delta per faction posture); the bridge calls `ModifyTrust` — the only write. Clamp per day; no belief-side cache.

**Write order:** translation data (data-first, integrity-checked) → observation hook at the belief-movement point → bounded `ModifyTrust` calls → faction surface shows which beliefs move standing → journal.

**Verify:** `BeliefStanceBridgeTests` — movement shifts stance through the engine; clamps hold over repeated days; no cache appears (state lives in the engine); determinism.

**Rollback:** remove hook + data rows.

## J.10 W10 Rites evidence — runbook

**Read first (hard gate first):** the Reckoning evidence vocabulary **[P0-VERIFY]** — does a rite fragment class exist? If not, stop and file DP-CM-4 before any code. Then: the ritual owner's performance record **[P0-VERIFY]**; `EnrollEvidence(int amount = 1)`; standing-record consumers.

**Design:** on rite performance, enroll one evidence fragment through `EnrollEvidence` keyed by the performance id; standing record shows it.

**Write order:** vocabulary confirmation (documented) → enrollment seam with performance-id guard → standing-record/memorial surface → journal.

**Verify:** `RitesReckoningTests` — enroll once; totals move; save/load safe; unknown rite class is inert (fail-closed).

**Rollback:** remove seam.

## J.11 W11 One-shot trigger primitive — runbook

**Read first:** `IFlagLedger` (`IsSet`, `Set`, `Clear`, `GetCounter`, `Increment`) and the flag save family; how a ledger instance is obtained in Core.

**Design:** `OneShotTriggerLedger` wrapping a ledger: `Arm(triggerId, dayThreshold, gateFlag)`, `TryFire(triggerId, day) → bool` (fires once when `day >= dayThreshold` ∧ gate set), `ReArm(triggerId)` explicit, census. Persist by using the ledger's own flag ids (no new store, no new save section).

**Write order:** primitive + census → tests → adoption in W8 and W10 (replace ad-hoc guards) → port-contract classification.

**Verify:** `OneShotTriggerLedgerTests` — fires once; respects threshold and gate; survives save/load; re-arm explicit; concurrent double `TryFire` in one tick fires once (deterministic single-threaded tick assumed; test documents the assumption).

**Rollback:** primitive is additive; adopters revert to their previous guards.

## J.12 W12 Balance baseline — runbook

**Read first:** the deterministic harness entry points **[P0-VERIFY]**; the pre-wave baseline commit; the existing `docs/balance/` reports.

**Design:** paired seeded runs (same seed, pre/post wave) over the mechanics W1–W4 touch; publish deltas; file retune targets as separate proposals.

**Verify:** harness runs complete; deltas published; no product-code change in the wave's diff (this is the verification).

**Rollback:** n/a (documentation-only wave).

---

# Appendix K — Per-seed traceability (factory Lane B + backlog → this program)

| Seed | Cluster | Authority claim | Live disposition | Where it lives now |
|---|---|---|---|---|
| B-01 | C1 | shelter-failure cascade | backlog (exit criteria unread) | Appendix C.1 |
| B-02 | C1 | grid seal follow-through | backlog (scope unverified) | Appendix C.1 |
| B-03 | C2×C12 | dose ↔ fallout window | GAP CONFIRMED | **W2** |
| B-04 | C2×C9 | child-health cohort bridge | real, overlaps sealed medical set | Appendix C.1 |
| B-05 | C3→C2 | preservation × disease | GAP CONFIRMED | **W1** |
| B-06 | C4 | difficulty consumers for industry | coordinate (PFGL W7 / CF-XP01) | Appendix C.1 |
| B-07 | C5→C2 | breakdown consequences | GAP CONFIRMED | **W4** |
| B-08 | C5 | scavenging parity | stale counts; data audit not mechanics | §2.2 |
| B-09 | C6 | flooded-route topology | GATED | Appendix C.2 |
| B-10 | C7 | faction-war per-strike emitter | GATED | Appendix C.2 |
| B-11 | C11 | black-market funds legs | GATED | Appendix C.2 |
| B-12 | C8/C11 | rumor band extension | consumed; data extension | Appendix C.1 |
| B-13 | C8 | intercept journal depth | backlog (not core mechanics) | — |
| B-14 | C9→C7 | belief × stance | GAP CONFIRMED | **W9** |
| B-15 | C9→C13 | rites → Reckoning | GAP CONFIRMED | **W10** |
| B-16 | C10 | quest reopening | GAP CONFIRMED | **W7** |
| B-17 | C10→C8 | gossip propagation | GAP CONFIRMED | **W8** |
| B-18 | C11 | trade-screen scenarios | data-only | — |
| B-19 | C12 | winter pressure power/water | GAP CONFIRMED | **W3** |
| B-20 | C13 | Reckoning enrollment sweep | audit, not mechanics | — |
| B-21 | C14→C5 | migration × encounters | GAP CONFIRMED | **W6** |
| B-22 | C15×C7 | defense × siege | GAP CONFIRMED | **W5** |
| B-23 | C16 | difficulty consumers | owner = PFGL W7 | Appendix C.1 |
| B-24 | C17 | stale-panel refresh | UI lane; PFGL/Triad B own surfaces | — |
| B-25 | C8/C2 | rescue-remains medical | sealed-surface coordinate | Appendix C.1 |
| B-33 | C3 | intake advisory bridge | **FALSE premise — sealed** | §2.2 (discarded) |
| B-34 | C9 | unstarted-type cluster | foreman prioritization, not seven plans | Appendix C.1 |
| B-35 | cross | one-shot trigger primitive | GAP CONFIRMED | **W11** |
| SB-05 | C3/C2 | preservation contamination | promoted | W1 |
| SB-06 | Lane C | balance baseline refresh | promoted | W12 |

---

# Appendix L — Worked examples (the arithmetic reviewers will ask about)

**L.1 W1 foodborne exposure.** Suppose `ConsumeFoodResult` consumes 2 units of a preserved item whose current spoiled share is 40% → consumed spoiled ≈ 0.8 → `share = 0.8/2 = 0.4`. Catalog foodborne row: `base_probability_permille = 120` (12%). `probability = 120 * 0.4 / 1000 = 4.8%` per surviving unit-equivalent event (one exposure call, not two). Immunity check runs inside `TryExpose`. Counterplay paths: discard (share → 0), cure (item tier changes → no longer in the foodborne-relevant class), cold storage (share grows slower via shelf-life factor).

**L.2 W2 dose banding.** `nominalMsv = 0.5`, band 1.0 (clear) → `BookReading` bands as today. Inside a window with band 1.8 → nominal 0.9 → one band higher is plausible; the exact band edges come from `BandFor(float mSv)` and must not be edited. Anti-rad before+after can still drop the reading; shielding factor scales the input. The plan asserts direction, not specific band jumps, because bands are the ledger's authority.

**L.3 W3 winter draw.** A filter burn of 2 units/day at band 1.0 becomes 2×band inside the window. Hardening upgrades in the owner's existing catalog reduce the pre-band draw. The contract is multiplicative at one site; the test asserts exactly one multiplication by checking that a band of 1.0 is an identity (a double application would still be identity — hence the additional grep-level guard on the call site).

**L.4 W4 breakdown bands.** Given `breakdownRisk` roll `r` and track-gear multiplier `m`: band = `r < 0.60` no injury; `0.60–0.85` injury (mild); `> 0.85` exposure (band scaled by `m`). The exact cut points are balance data, authored through the expedition's existing tuning surface — the runbook fixes the *shape*, P0 fixes the numbers against the dominance table **[P0-VERIFY]** so a repair-heavy build does not trivialize breakdowns.

**L.5 W6 encounter bias.** Baseline weights `{raider: 10, weather: 20, wildlife: 5}`. Migration read model says sector pressure 0.5 for wildlife. Biased weight = `base * (1 + pressure * k)` with `k = 0.5` → `{raider 10, weather 20, wildlife 6.25}` — directionally shifted, floor preserved, and bounded by `k` so no encounter can be erased.

---

# Appendix M — Data-first authoring contracts (how to author safely, per wave)

1. **Never invent ids.** Every id must exist in, or be added to, the owning catalog and pass the integrity gate. New ids get `schema_version` and snake_case fields per the data authority.
2. **One owner per row.** A foodborne exposure row lives in `disease_catalog`; a window multiplier lives in the Year-of-Ash events vocabulary; a winter curve lives in the owner's tuning catalog. A row that needs two homes is a design smell — split it.
3. **Ranges are declared, not implied.** Probabilities in per-mille, multipliers as explicit ratios, and every authored number gets a comment-level rationale in the handoff (why this value).
4. **Gate before code.** `--data-integrity-selftest` (or the live equivalent) must pass before any seam work starts; a failing gate stops the wave.
5. **No orphan rows.** Every authored row must be consumed by a code path in the same wave (the content-utilization scan is the instrument; a row that no wave consumes is backlog, per the authority's unclaimed-content discipline).
6. **Backward compatibility.** Adding rows is safe; editing existing row semantics requires a migration note and a save-schema review if state is affected.

---

# Appendix N — Surface, accessibility, and tone contracts (per wave, condensed)

- **Bind before fork.** Every wave prefers an existing surface; a new route costs a panel class + registry + `Main.GameFlow` + `Main.PlayerSurfaces` + manifest entries and is justified only when the existing surface cannot express the mechanic honestly.
- **Truthful state.** Surfaces display Core truth (census/read models), never offline-computed probabilities or costs.
- **Accessibility.** Keyboard/controller close-back parity, focus order, visible feedback, and non-color-only status (the hazard bands, dose bands, and hygiene bands all have text labels by contract).
- **Tone.** Journal facts are restrained, factual, and fictional; no real-world war names, no copied text, no UI-layout pastiche.
- **Disposal.** Panels refresh on census change and release subscriptions on close (lifecycle contract).

---

# Appendix O — Test doctrine (categories, selection, quarantine)

1. **Focused only.** Smallest file that covers the change; new files run alone first; `scripts/run_test.sh <file>` under the 180s cap; no full suite by default.
2. **Categories preserved.** Determinism (two-pass replay), save/load round-trip, lifecycle, mutation/state-transition, and fuzzing tests stay independent — never aggregate them into a table.
3. **Aggregation only for static mappings** (catalog id → consumer, band tables, label maps) where a per-row failure message is kept.
4. **Quarantine discipline.** A failing test is triaged with current API/content evidence; re-enabling requires a written reason and a passing focused target. Never silently re-enable.
5. **Probe discipline.** CLI probes are evidence, not gates on correctness; a green probe with a player-inoperable mechanic is still a failed wave.

---

# Appendix P — Coordination and handoff (integration cadence)

**Shared hubs (claim before touching; never two waves at once):** `SaveSectionRegistry.cs` · `HostCliRegistry.cs` · `DayEventVocabulary.cs` · `Main.CampaignOwners.cs` · `Main.Lifecycle.cs` · `Main.SaveOrchestrator.cs` · `Main.Application.cs` · `PanelRegistryBootstrap.cs` · `Main.GameFlow.cs` · `Main.PlayerSurfaces.cs` · `player_surface_manifest.json` · `port_contract_policy.json`.

**Never-claim-together pairs in this program:** W1×W4 (both touch disease/dose receivers — sequence W1 then W4), W2×W4 (dose), W8×W10 (adopt W11; sequence), any wave × PFGL master W1–W10 while either holds UI hubs, any wave × Triad B while either holds the manifest.

**Handoff template (per `AI_AGENT_WORKFLOW.md`):** Outcome · Files touched · Files intentionally untouched · Contract · Verification (commands + results) · Limitations · WORKTREE status · Next agent. Plus this program's additions: which factory seeds were consumed, which drift rows were re-verified, and the backlog delta.

**Cadence:** one wave per claim; a wave closes with a handoff and releases its claims before the next claim opens; the integrator (not this document) updates `INTEGRATION_PLANS.md` and the decision register.

---

# Appendix Q — Program closeout and ledger mapping

| Program wave | Ledger row to add on close (integrator-authored) | Evidence to attach |
|---|---|---|
| W1 | `CORE-MECH-W1-FOODBORNE-DISEASE-BRIDGE` INTEGRATED | probe, focused tests, manual script, catalog gate |
| W2 | `CORE-MECH-W2-DOSE-STORM-WINDOW` INTEGRATED | same |
| W3 | `CORE-MECH-W3-WINTER-PRESSURE-POWER-WATER` INTEGRATED | same + DP-CM-1 resolution |
| W4 | `CORE-MECH-W4-BREAKDOWN-CONSEQUENCES` INTEGRATED | same |
| W5–W11 | one row each | same |
| W12 | `CORE-MECH-W12-BALANCE-BASELINE-REFRESH` COMPLETE | published deltas + filed retune targets |

Closeout rule: the program is closed when W12 publishes deltas and any retune targets are filed as separate signed proposals. The program never retunes inline.

---

# Appendix R — Glossary

- **Gap seal / feature seal / mechanic seal** — the three exit tiers of this program (§0).
- **Mechanic-felt** — the completion-chain link where a consequence is observable in another owner and counterplay exists.
- **Factory Protocol** — the authority's seven-step loop (premise sweep → lane/cluster → archetype → subject format → continuity → evidence labels → output bundle).
- **One-shot trigger** — a persisted `once, on/after day D, if gate G` primitive (W11).
- **Exactly-once** — a consequence that cannot double-apply across reticks or save/load.
- **Fail-closed** — absent/unknown input yields no effect plus a journal fact, never a silent success.
- **Higher=worse** — the sign convention for Needs-style scalars (Morale, Fatigue) used across the companion plans.
- **DP-CM-n** — a decision packet in this program (Appendix C.3).
- **PIR-n** — a pre-integration readiness gate row (Appendix A).

---

# Appendix S — Per-wave forensic evidence cards (deep polish)

> Card schema: what the owner is · what exists today · what is missing (with probe) · what the wave adds · what it must not add · blast radius · predecessor/successor constraints. Cards are read at P0 before any edit.

## S.1 W1 — Foodborne disease bridge
**Owner:** `FoodPreservationSystem` (spoilage truth) → `DiseaseSystem` (infection truth). **Catalogs:** `food_preservation` family (existing) · `disease_catalog` (exposure rows). **Probe today:** no `Disease`/`Contaminat` reference in `FoodPreservationSystem.cs`; `ConsumeFood` has no player-path caller; the player's eat is `HoldfastRuntimeSession.ConsumeFoodResult(itemId, amount, survivorId?)`. **Missing:** the connection, and the authored foodborne exposure vocabulary. **Adds:** exposure rows + a thin seam + counterplay surface. **Must not add:** a disease type outside the catalog; a second eat path; a `FoodSafetySystem`. **Blast radius:** the eat path is shared with holdfast consumption (thin change, high visibility) — regression tests on holdfast consumption are mandatory. **Constraints:** lands before W4 (W4 reuses the exposure seam shape for contamination).

## S.2 W2 — Dose storm window
**Owner:** `DoseLedgerSystem` (booking + bands) ← Year-of-Ash calendar (window). **Catalogs:** `year_of_ash_events.json` (windows) · dose family. **Probe today:** zero `YearOfAsh`/`StormWindow` references in `DoseLedgerSystem.cs`; `BookReading` is the single booking entry and already takes `ISeededRng`. **Missing:** day→window read model and one conditioning site. **Adds:** provider + multiplication upstream of booking + band readout. **Must not add:** season multipliers inside the ledger; edits to `BandFor`; a second dose store. **Blast radius:** every dose reading in the campaign shifts inside windows — balance-visible, hence W12. **Constraints:** W4 also books readings; both must use the single conditioning site.

## S.3 W3 — Winter pressure (power + water)
**Owners:** the two chosen power/water owners **[DP-CM-1]** ← shared seasonal provider. **Probe today:** zero `YearOfAsh` references in any `*Power*` or `*Water*` Core file. **Missing:** the season is invisible to the two systems that should suffer most. **Adds:** shared provider + one multiplication per chosen consumer + curve data. **Must not add:** a `WinterPressureSystem`; a second band provider (shares W2's); parallel difficulty scalars. **Blast radius:** Day 180–360 draw changes in the chosen owners. **Constraints:** provider is shared with W2 — coordinate the file so there is exactly one implementation.

## S.4 W4 — Breakdown consequences
**Owner:** `ExpeditionVehicleSystem` (risk) → medical intake + `DoseLedgerSystem` (+ `DiseaseSystem`). **Probe today:** `EffectiveBreakdownRiskMultiplier`, `Repair`, `RepairTrackGear` exist; no injury/exposure routing. **Missing:** the consequence band and its routing. **Adds:** seeded band + routing + event-id guard. **Must not add:** injury state in the vehicle save; a second medical intake. **Blast radius:** expedition outcomes gain injury/exposure; medical load rises. **Constraints:** lands after W1 (exposure seam) and W2 (dose conditioning); uses both.

## S.5 W5 — Defense siege coupling
**Owners:** defense owners → raid/siege resolver **[DP-CM-2]**. **Probe today:** `DefenseGrid` referenced only by `src/UI/DefenseGridPanel.cs` + `src/Main.Plans162_165.cs`. **Missing:** any consumer of defense values in resolution. **Adds:** read-only projection + one resolver consumption + panel/journal. **Must not add:** a second siege model; defense rebalancing. **Blast radius:** raid outcomes become sensitive to fortification — the intended point. **Constraints:** stance engine remains the standing authority; no trust writes here.

## S.6 W6 — Migration encounter bridge
**Owners:** migration engines → travel-encounter selector. **Probe today:** migration consumed by wildlife/harvest paths; travel encounters never consult it. **Missing:** the bias. **Adds:** sector read model + bounded bias + travel surface. **Must not add:** encounter rows outside `travel_encounters.json`; unbounded bias. **Blast radius:** encounter mix shifts on migration corridors. **Constraints:** the authored floor distribution is preserved (identity bias at zero pressure).

## S.7 W7 — Quest reopen grammar
**Owners:** quest owners + `IFlagLedger`. **Probe today:** no reopen logic in Core. **Missing:** predicate, transition, guard. **Adds:** data-driven predicate + one-shot reopen + visibility. **Must not add:** a side queue outside quest state; hard-coded quest ids. **Blast radius:** failed threads can revive — campaign pacing changes. **Constraints:** uses `IFlagLedger` (the same persistence W11 rides) — coordinate the flag-id namespace.

## S.8 S.8 W8 — Gossip propagation
**Owners:** moral-choice state → existing rumor/info owners. **Probe today:** `moral_choice_gossip` read-side only. **Missing:** propagation. **Adds:** deterministic propagation step + adoption of W11's one-shot primitive + surfaces. **Must not add:** a gossip network with its own persistence; unbounded emission. **Blast radius:** information state changes for factions — coordinate with radio/info owners. **Constraints:** adopt W11 (sequence W11 before or with W8's adoption step); DP-CM-3 placement.

## S.9 W9 — Belief stance bridge
**Owners:** `belief_movements` → `FactionStanceEngine`. **Probe today:** stance engine has no belief input. **Missing:** translation. **Adds:** authored translation table + bounded `ModifyTrust`. **Must not add:** belief-side trust cache; unbounded deltas. **Blast radius:** faction standing shifts with interiority. **Constraints:** the engine is the only writer (single-authority invariant).

## S.10 W10 — Rites Reckoning evidence
**Owners:** ritual owner → Reckoning `EnrollEvidence`. **Probe today:** enrollment exists; no memorial-rite caller. **Missing:** vocabulary check + enrollment. **Adds:** enrollment with performance-id guard + standing-record surface. **Must not add:** duplicated rite state; silent skips. **Blast radius:** endgame evidence totals. **Constraints:** hard gate — DP-CM-4 if the vocabulary cannot admit a rite.

## S.11 W11 — One-shot trigger primitive
**Owner:** new small Core primitive on `IFlagLedger` persistence. **Probe today:** flags/counters exist; no day-thresholded one-shot trigger. **Missing:** the primitive. **Adds:** `Arm/TryFire/ReArm` + census. **Must not add:** a second flag store; a new save section. **Blast radius:** adopters (W8, W10) only. **Constraints:** flag-id namespace is shared with W7 — coordinate.

## S.12 W12 — Balance baseline refresh
**Owner:** harness. **Probe today:** baselines exist for expeditions/vehicles/economy; not for W1–W4 mechanics. **Missing:** the refresh. **Adds:** paired seeded runs + `docs/balance/` deltas + filed targets. **Must not add:** inline tuning. **Blast radius:** none (documentation). **Constraints:** last wave; pins the baseline commit.

---

# Appendix T — Per-wave acceptance command matrices

> Commands are the *smallest* focused set per wave, per TEST_POLICY. Probe names marked **[P0-VERIFY]** must be confirmed live before the run (flags drift; the authority's DR-07 lesson applies). Never run the full suite by default.

| Wave | Focused tests | Probe / gate | Static assertions |
|---|---|---|---|
| W1 | `Ashfall.Core.Tests/Shelter/FoodborneExposureBridgeTests.cs` (alone first) | live disease probe **[P0-VERIFY]**; `--data-integrity-selftest` | one exposure call site; no `FoodSafetySystem` type; preservation file free of disease logic |
| W2 | `Ashfall.Core.Tests/Medical/DoseStormWindowTests.cs` | live dose probe **[P0-VERIFY]** | exactly one `BookReading` conditioning site; ledger file free of season types |
| W3 | `WinterPressureProviderTests` + consumer tests | live power/water probes **[P0-VERIFY]**; determinism two-pass | one shared band provider file; no `WinterPressureSystem` |
| W4 | `BreakdownConsequenceTests` | live expedition probes **[P0-VERIFY]** | event-id guard present; no injury state in vehicle save |
| W5 | `DefenseSiegeCouplingTests` | live raid/faction probes **[P0-VERIFY]** | one resolver consumption; no second siege model |
| W6 | `MigrationEncounterBridgeTests` | live travel probes **[P0-VERIFY]** | bounded bias constant; Ordinal sort before weighted pick |
| W7 | `QuestReopenGrammarTests` | live quest probes **[P0-VERIFY]** | reopen guard; no side queue |
| W8 | `GossipPropagationTests` | live moral-choice/radio probes **[P0-VERIFY]** | one emission per (source, channel, window) |
| W9 | `BeliefStanceBridgeTests` | live faction probes **[P0-VERIFY]** | `ModifyTrust` is the only write; no belief cache |
| W10 | `RitesReckoningTests` | live endgame probes **[P0-VERIFY]** | vocabulary check recorded; enroll-once guard |
| W11 | `OneShotTriggerLedgerTests` | port contract classification | no new save section; flag-family persistence only |
| W12 | (harness) | paired seeded runs | no product-code diff in the wave |

**Program exit gate:** every row above executed at least once in the program's history with recorded results; port contract 0 unbound; save pin asserted; manifest counts honest; ledger rows present.

---

# Appendix U — Expanded failure-mode catalogs

**U.1 Cross-cutting (all waves).** unknown ids; missing catalogs; absent receiving owners; double application; save/load replay; stale session after slot switch; concurrent claims; HEAD drift; probe-name drift; generated-artifact drift (architecture map, selftest manifest, port contract, plan audit must be regenerated by their owners, never hand-edited).

**U.2 W1.** Preservation owner not set up in a game mode (fail-closed + fact); item has no preservation record (treat as clean, journal once); spoiled share computed on total rather than consumed (over-exposure — the test catches it); double call per unit (exactly-once test); bypassing immunity (never call `Infect` directly); catalog row missing for a class (fail-closed, integrity census names it).

**U.3 W2.** Conditioning applied inside the ledger (breaks the one-site rule); window lookup using wall clock; double multiplication; neutral fallback silently applied when the calendar is missing (must be journaled); band edges edited (forbidden); antirad recorded after the reading instead of before (pre-existing semantics preserved).

**U.4 W3.** Two providers (one per wave) drifting; consumer applying the band twice; band applied to the wrong quantity (draw vs cost); hardening double-counted; missing calendar silently halving winter pressure; day-boundary off-by-one (band is a pure function of day — test at boundaries 179/180/360/361).

**U.5 W4.** Breakdown event id not stable across reticks (guard fails); injury routed to the wrong intake; exposure routed without the W2 conditioning site; missing medical owner (fail-closed); repair not reducing the band (counterplay test).

**U.6 W5.** Projection read at the wrong moment (post-raid); resolver consuming both projection and raw defense (double); defense owner absent (neutral); a second raid model appearing.

**U.7 W6.** Bias applied to authored floor (erasing encounters); iteration order leaking into selection (Ordinal sort); migration owner absent (identity bias); pressure not clamped.

**U.8 W7.** Reopen on the same day twice; predicate hard-coded; status transitions bypassing the quest state machine; flag namespace collision with W11.

**U.9 W8.** Emission without a window guard; suppression not honored; iteration-order nondeterminism; placement ambiguity (DP-CM-3) left undecided; a second persistence for gossip.

**U.10 W9.** Trust write outside the engine; unbounded daily deltas; translation table edited at runtime; feedback loop (belief shifts causing belief shifts) without a source.

**U.11 W10.** Vocabulary gate skipped; enrollment twice; rite state copied into endgame; unknown rite class silently dropped.

**U.12 W11.** Primitive storing its own persistence; re-arm implicit; concurrent fire in one tick; flag-id collision with W7.

**U.13 W12.** Baseline commit unpinned; seeds changed between runs; inline tuning sneaking in; deltas published without machine-readable inputs.

---

# Appendix V — Cross-wave wiring maps (day phases, events, ports)

**V.1 Day-phase interaction.** The core waves ride existing phases; none introduces a new phase.
| Wave | Phase it rides | Interaction notes |
|---|---|---|
| W1 | none (command path) | exposure resolves at eat time, not on a tick |
| W2 | none (booking path) | window lookup is a pure function of the booking day |
| W3 | existing power/water ticks | band applied inside existing consumption/drawdown, once per draw |
| W4 | expedition event path | band resolves on the breakdown event, not on a day tick |
| W5 | raid resolution path | projection read at resolution time |
| W6 | travel selection path | bias applied at selection time |
| W7 | quest lifecycle + day open | predicate evaluated on flag change and day open |
| W8 | day tick (propagation step) | one emission per window; Ordinal ordering |
| W9 | belief-movement observation point | bounded `ModifyTrust` at observation |
| W10 | rite-performance point | enroll once at performance |
| W11 | none (infrastructure) | pure function; adopters call it |

**V.2 Day-event vocabulary plan.** W1–W4 add journal facts (not necessarily day events); any new day-event ids must be added to `DayEventVocabulary.cs` **and** the semantic parity matrix by their owners, in the same change, or the parity gate fails. Proposed ids (owner-confirmed at P0): `food.spoilage_exposure`, `dose.storm_window`, `power.winter_pressure`, `water.winter_pressure`, `expedition.breakdown_consequence`, `defense.siege_effect`, `travel.migration_contact`, `quest.reopened`, `gossip.propagated`, `faction.belief_stance_shift`, `reckoning.rite_enrolled`.

**V.3 Port-contract classification plan.** Each new public seam introduced by a wave must be classified (HOST_REQUIRED / CORE-ONLY / etc.) in `docs/ci/port_contract_policy.json` and the generated contract artifacts in the **same** change. Expected net additions: W1 (1–2), W2 (1), W3 (2), W4 (1–2), W5 (1), W6 (1), W7 (1), W8 (1–2), W9 (1), W10 (1), W11 (2–3). W12 adds none. Zero unbound at exit (PIR-4/§9.3).

**V.4 Save interaction plan.** No new sections. Restore paths unchanged except W11 (flag family) and any wave whose receiver already persists the consequence (disease, dose, faction, reckoning — they do). The save gate (`ComprehensiveSaveStoreCorruptionAndMigrationTests`) is run once per wave that touches a shared hub, not per assertion.

---

# Appendix W — Player-value and meaningfulness deep dives

**W.1 W1 foodborne.** The design intent: a player learns to respect the cold chain. The mechanic is felt when the *same meal* is safe or dangerous depending on a decision the player made days earlier (cure vs skip, store cold vs ignore, discard vs gamble). Failure state is a ward case, not a game-over — the counterplay is medical, which keeps the failure legible and recoverable.

**W.2 W2 dose.** The design intent: exposure becomes a calendar decision. The mechanic is felt when the player chooses to send someone out on Day 200 with a dosimeter and antirad, or keeps them in. The band readout is the legibility contract; the antirad timing is the counterplay skill.

**W.3 W3 winter.** The design intent: preparation is rewarded. The mechanic is felt when hardening bought in September pays in February. Without W3 the Year of Ash is scenery; with it, it is a budget.

**W.4 W4 breakdown.** The design intent: maintenance is an investment with a failure mode. The mechanic is felt when the expedition turns back because of track-gear condition — and when it doesn't, someone comes back hurt.

**W.5 W5 defense.** The design intent: walls matter. The mechanic is felt when a raid that would have overwhelmed the shelter is walked off because of accumulated perimeter work.

**W.6 W6 migration.** The design intent: the map is alive. The mechanic is felt when a route that was safe last month now runs through a herd, and a scout's report (the read model) is worth taking.

**W.7 W7 reopen.** The design intent: campaigns are recoverable. The mechanic is felt when a failed thread is revived by a discovery the player made for another reason.

**W.8 W8 gossip.** The design intent: choices have reach. The mechanic is felt when a secret reaches a faction that should not know it, and the player learns that the channel, not the choice, was the vulnerability.

**W.9 W9 belief.** The design intent: interiority is political. The mechanic is felt when a community's religious drift changes who will trade with the shelter.

**W.10 W10 rites.** The design intent: grief is recorded. The mechanic is felt when the Reckoning's evidence list includes the names of people the player mourned properly.

**W.11 W11 primitive.** The design intent (meta): future mechanics get one-shot semantics for free, with one test suite instead of N ad-hoc guards.

**W.12 W12 baseline.** The design intent (meta): every balance claim the program makes is measured, not asserted.

---

# Appendix X — Epilogue-position statements (factory invariant 2.2)

The factory requires every generated plan to state its position relative to the epilogue permutations it touches. The program-wide position:

- **No wave invalidates the main ending.** All twelve waves add pressure and consequence to the campaign spine; none gates or replaces an ending, and none writes into the ending evaluator's authority.
- **W10 touches the Reckoning by design** (evidence enrollment) and is therefore permutation-sensitive: evidence enrollment must be additive and optional, so no permutation becomes unreachable (the authority's constraint: “main ending cannot be invalidated by optional content”).
- **W7 (quest reopen) can alter which threads are complete at Day 360**; the epilogue matrix must treat a reopened-and-completed thread as evidence, never as a requirement.
- **W5/W8/W9 change faction and information state** that the epilogue reads for standing records; their writes are to their own owners, and the standing-record projection is unchanged in shape.
- **W1–W4, W6, W11, W12 are epilogue-neutral** (they alter mid-campaign simulation or infrastructure only).

---

# Appendix Y — Integration sequencing and critical path

**Critical path:** W1 → W4 (W4 reuses W1's exposure seam and W2's dose conditioning). Everything else is parallelizable subject to hub claims.
**Recommended order for one integrator:** W1 (entry) → W2 (shared provider) → W3 (shared provider consumer) → W4 (uses both) → W11 (infrastructure) → W8 + W10 (adopt W11) → W5, W6, W7, W9 (independent, any order) → W12 (close).
**Hub contention:** W1–W4 each touch the day-owner/save-hub family once; W8/W10 both touch the manifest for adoption; W5–W7/W9 each need a panel decision (PIR-8). Sequence the manifest bursts, not the waves.
**Parallel rule:** at most one wave holds a shared hub at a time; waves without hub needs may run concurrently with hub holders provided their claims do not overlap.

---

# Appendix Z — Decision packets (expanded, with default-if-silent)

**DP-CM-1 — W3 first consumers.** *Question:* which power chain and which water owner consume the winter band first? *Evidence:* the Day-30 capacity/balance reports name the visible draws; the water filter chain is the most legible. *Recommendation:* water first, then the power chain whose Day-180+ draw is already visible in reports. *Default if silent:* water first (filter burn). *Cost of being wrong:* one consumer swap; low.

**DP-CM-2 — W5 resolver authority.** *Question:* confirm the existing raid/siege resolver is the only consumer of defense projections. *Evidence:* stance engine is the standing authority; no second siege model should exist. *Recommendation:* yes — one resolver. *Default if silent:* implement against the existing resolver only; escalate if none exists. *Cost:* high if violated (parallel model).

**DP-CM-3 — W8 placement.** *Question:* Core or host-thin? *Evidence:* the determinism rules bind Core; the propagation is pure. *Recommendation:* Core, seeded, tested. *Default if silent:* Core. *Cost:* medium (a host placement is harder to test deterministically).

**DP-CM-4 — W10 vocabulary.** *Question:* if the Reckoning evidence vocabulary admits no rite fragment, amend it additively? *Recommendation:* amend; never skip silently. *Default if silent:* stop the wave and file. *Cost:* low (vocabulary-only change).

**DP-CM-5 — W11 adoption order.** *Question:* W8 or W10 adopt the primitive first? *Evidence:* W8 has the harder semantics and lands earlier in the recommended order. *Recommendation:* W8 first. *Default if silent:* W8 first. *Cost:* low.

---

---

# Appendix AA — Receiver contracts (how each receiving owner must be treated)

A consumer (wave) never edits a receiver's rules. These contracts state what each receiving authority guarantees, what it refuses, and what a consumer must do to be a good citizen. They are derived from the live APIs probed this revision and are re-read at each wave's P0.

**AA.1 `DiseaseSystem` (receiver for W1 foodborne, W4 contamination).**
Guarantees: immunity is respected inside `TryExpose`/`TryInfect`; outcomes are typed (`DiseaseExposureResult`) with created / blocked reasons; outbreaks are the escalation authority; `TickDaily` owns progression. Refuses: direct `Infect(...)` calls by consumers (it bypasses the exposure pipeline the catalog defines). Consumer duties: build a `DiseaseExposureContext` from an authored exposure row; use the authority's rng; never roll your own infection dice; treat blocked-by-immunity as a *success* of the system, not a failure of the bridge.

**AA.2 `DoseLedgerSystem` (receiver for W2 conditioning, W4 exposure).**
Guarantees: `BookReading` is the single booking entry and owns the seeded roll, anti-rad timing semantics, band classification (`BandFor`), cumulative totals, and administrative overrides. Refuses: callers that pre-decide the band, mutate entries, or apply their own jitter. Consumer duties: supply a **nominal** mSv that already includes any environmental conditioning; never pass a pre-banded value; never edit `BandFor`.

**AA.3 The medical injury intake (receiver for W4 injury band).** **[P0-VERIFY: exact owner and verb]**
Guarantees (to be confirmed at P0): single intake path; the ward/clinical triage owners own treatment; injuries persist through the medical family. Consumer duties: route the minimum required fields; do not create a parallel injury list; do not bypass clinical triage.

**AA.4 `FactionStanceEngine` (receiver for W9 belief bridge).**
Guarantees: `ModifyTrust` is the only trust writer; `GetStance`/`GetRaidAggression` are pure reads; trust is bounded by the engine. Consumer duties: bounded deltas; no caching of trust on the belief side; no direct field writes.

**AA.5 The Reckoning/standing-record owner (receiver for W10 rites).**
Guarantees: `EnrollEvidence` is the enrollment authority; standing records project evidence additively. Consumer duties: additive fragments only; optional content must never be a completion requirement (epilogue invariant); one enrollment per performance.

**AA.6 The rumor/info owner (receiver for W8 gossip).** **[P0-VERIFY: exact owner among `RumorSystem`/radio]**
Guarantees: deterministic bands; existing rumor propagation rules; radio/journal surfaces read from it. Consumer duties: emit into the owner, not around it; respect existing suppression; one emission per window.

**AA.7 The travel-encounter selector (receiver for W6 migration).** **[P0-VERIFY]**
Guarantees: authored encounters remain the floor; selection is deterministic under a seed. Consumer duties: bounded bias; Ordinal ordering; identity at zero pressure; no new encounter ids outside the catalog.

**AA.8 The quest owners + `IFlagLedger` (receivers for W7 reopen, W11 primitive).**
Guarantees: quest state transitions belong to the quest owner; flags/counters are the shared persistence substrate. Consumer duties: use the state machine; use the ledger's flag namespace; never write quest state directly.

---

# Appendix AB — Determinism deep dive and RNG stream sheet

**AB.1 Rules.** Deterministic Core behavior draws only from injected `ISeededRng` instances (or pure functions of state and day). No `System.Random`, no `Guid.NewGuid`, no wall-clock seeds, no culture-dependent formatting in state or checksum paths. Iteration order never reaches a hash or a persisted list without an Ordinal sort. Two-pass replay per new consequence: same seed, same inputs, identical outputs, verified by focused tests (not by full-suite reruns).

**AB.2 Stream allocation per wave.**
| Wave | Stream needed | Source | If none needed |
|---|---|---|---|
| W1 | infection roll | disease authority's existing stream (do not fork a new one) | — |
| W2 | none new (booking roll already exists in the ledger) | — | window lookup is pure |
| W3 | none (bands are curves) | — | — |
| W4 | breakdown consequence roll | expedition's existing stream | — |
| W5 | none (projection is pure) | — | — |
| W6 | selection roll | travel's existing stream | — |
| W7 | none (predicate is pure) | — | — |
| W8 | propagation roll | info owner's existing stream (DP-CM-3) | — |
| W9 | none (bounded deltas) | — | — |
| W10 | none | — | — |
| W11 | none (pure one-shot) | — | — |
| W12 | harness seeds (pinned) | documented per report | — |

**AB.3 The idempotency guard pattern.** Every day-tick consequence in the program uses the same shape: `key = owner + subject + day + source`; before applying, check the owner's existing dedup surface (flag ledger counter, day-event record, or the receiver's own event id). A new dedup store is a PIR-4 escalation.

**AB.4 Two-pass test template.** Seed the host with a fixed `ISeededRng`; run N ticks; capture a state fingerprint (the owner's census + receiver census); rebuild from the same seed; run N ticks; compare fingerprints. This is the determinism acceptance for W1, W3, W4, W6, W8.

---

# Appendix AC — Save / restore deep dive

**AC.1 The pin.** `Assert.Equal(266, SaveSectionRegistry.All.Count)` is the current pin, asserted in `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs:315,317`. This program adds no sections; if a wave proves a new section unavoidable, the pin change and its test edit are part of that wave's diff and a foreman decision, not an integrator's.

**AC.2 What each wave persists (and what it must not).**
| Wave | Persists | Must not persist |
|---|---|---|
| W1 | nothing new (disease + preservation own their state) | exposure probability, per-meal risk |
| W2 | nothing new (window is derived) | cached window state |
| W3 | nothing new (band is derived) | cached band per owner |
| W4 | nothing new (medical/dose own outcomes) | band roll history outside the expedition event log |
| W5 | nothing new | defense projection cache |
| W6 | nothing new | per-sector bias cache |
| W7 | reopen fact (via the flag/quest owner) | a reopen queue |
| W8 | propagation state (via the info owner) | a private gossip store |
| W9 | nothing new (trust lives in the engine) | belief-side trust mirror |
| W10 | nothing new (evidence lives in Reckoning) | rite copies |
| W11 | arm/fired state (flag family) | a second store |
| W12 | report only | — |

**AC.3 Old-save behavior.** Every wave must load an old save (pre-wave) and behave sanely: derived providers return neutral bands; absent consumers fail closed; the day-90 boundary cases are pure functions and safe. This is part of every wave's focused tests (one old-shape fixture, one new-shape fixture).

**AC.4 Mid-event safety.** W1 (eat), W4 (breakdown), and W8 (propagation) can fire mid-tick; a save taken between the decision and the consequence must not double-apply or drop it. The event-id guard (AB.3) is the mechanism; each of the three waves carries a save/load-mid-event test.

---

# Appendix AD — Observability and instrumentation matrix

| Wave | Journal facts | Day events (if any) | Surface readout | Player-visible counterplay |
|---|---|---|---|---|
| W1 | `food.spoilage_exposure`, `food.discarded` | — | spoiled counts + discard + storage temp | discard / cure / cold store |
| W2 | `dose.storm_window` | — | window band on the dose reading | shielding / antirad / shelter |
| W3 | `power.winter_pressure`, `water.winter_pressure` (on band change) | — | band + hardening offset | harden / reserve / filter |
| W4 | `expedition.breakdown_consequence` | — | preflight risk + consequence history | repair / track gear / abort |
| W5 | `defense.siege_effect` | — | projection vs next raid | fortify / shield |
| W6 | `travel.migration_contact` | — | corridor state on the route | reroute / scout / prepare |
| W7 | `quest.reopened` | — | reopen eligibility + reason | pursue the discovery |
| W8 | `gossip.propagated` | — | rumor arrival per channel | suppress / control / courier |
| W9 | `faction.belief_stance_shift` | — | which beliefs move standing | outreach / ritual / policy |
| W10 | `reckoning.rite_enrolled` | — | evidence list + standing record | perform the rite properly |
| W11 | — | — | — (infrastructure) | — |
| W12 | — | — | — | — |

Instrumentation rule: journal facts are facts, not narration. Every fact names its source system, subject, and outcome so a later telemetry pass can aggregate without re-reading code.

---

---

# Appendix AE — Per-wave day-slice narratives (integrator runbook prose)

Each wave is executed in day-slices; the slices are the unit of work and the unit of handoff.

**AE.1 W1 Foodborne — slices.**
- *Slice 1 (premise):* re-run the §2.2 W1 rows; read the preservation and disease owners end to end; read the eat path; paste verb cards; claim `HoldfastRuntimeSession.cs`, the preservation-owning Main partial, `disease_catalog.json`, the new test file, and the food surface if it changes. **Exit:** PIR record complete.
- *Slice 2 (data):* author the foodborne exposure rows with per-mille probabilities and a stated rationale; run the integrity gate. **Exit:** gate green, rows enumerated.
- *Slice 3 (seam):* implement the spoiled-share → exposure translation at the eat path; single call; attributed; fail-closed. **Exit:** compile + existing probes green.
- *Slice 4 (surface/journal):* spoiled counts, discard action, storage temperature, journal facts. **Exit:** truthful readouts; a11y rows pass.
- *Slice 5 (verify):* focused tests alone first, then scoped suites, probe, manual script at 15 FPS, handoff. **Exit:** DoD.

**AE.2 W2 Dose window — slices.** Premise+claim → window provider (pure) → single conditioning site → band readout + journal → verify. The provider is shared with W3; W2 lands it first so W3 consumes rather than forks it.

**AE.3 W3 Winter pressure — slices.** Premise+DP-CM-1+claim → shared provider (reuse W2's) → curve data in the two owners' catalogs → two consumer seams (one multiplication each) → surfaces + journal → verify. Boundary-day tests (179/180/360/361) are written before the consumer seams, not after.

**AE.4 W4 Breakdown — slices.** Premise+verb cards+claim → pure band function → routing to the three receivers → event-id guard + mid-event save test → surface/history + journal → verify. W4 does not start until W1 and W2 have landed (it reuses both seams).

**AE.5 W5 Defense siege — slices.** Premise+DP-CM-2+resolver card+claim → projection → single resolver consumption → panel readout + journal → verify. If no resolver exists, the slice stops and files (DP-CM-2) rather than inventing one.

**AE.6 W6 Migration encounters — slices.** Premise+selector card+claim → sector read model → bounded bias (identity at zero) → travel readout + journal → verify. The baseline-distribution test is written with the bias, not after it.

**AE.7 W7 Quest reopen — slices.** Premise+quest card+flag-namespace coordination+claim → predicate → evaluation hook (flag change + day open) → reopen-once guard → surface+journal → verify.

**AE.8 W8 Gossip — slices.** Premise+rumor card+DP-CM-3+claim → propagation step (seeded, Ordinal) → W11 adoption (or interim guard) → info-owner emission → surfaces+journal → verify.

**AE.9 W9 Belief stance — slices.** Premise+stance card+claim → authored translation table (data) → observation hook with bounded deltas → faction readout+journal → verify.

**AE.10 W10 Rites — slices.** Premise+**vocabulary gate**+claim → enrollment with performance-id guard → standing-record readout+journal → verify. A failed vocabulary check stops the wave here.

**AE.11 W11 One-shot primitive — slices.** Premise+flag-persistence card+namespace coordination+claim → primitive + census → tests → adopt in W8 and W10 → port-contract classification → verify.

**AE.12 W12 Balance refresh — slices.** Baseline commit pin → paired seeded runs → deltas published → retune targets filed → ledger close.

---

# Appendix AF — Pilot campaign scripts (7-day smoke, per wave)

The project's smoke discipline (`--7-day-smoke-selftest` pattern) is reused at the wave level: after each wave, a 7-day pilot with a fixed seed must complete without error and with the wave's effect observable by day 3 and day 7.

- **W1 pilot:** day 1 eat clean (safe), day 2 eat spoiled (possible exposure), day 3 ward treats if infected, day 7 census shows the case in the disease owner's terms.
- **W2 pilot:** days inside/outside a synthetic window (test fixture), same nominal exposure, different bands; antirad before+after reduces the reading.
- **W3 pilot:** synthetic days 179→181→360→361 with a fixed seed; the chosen consumers' draws step at the band boundaries and return to baseline outside them.
- **W4 pilot:** scripted vehicle with a high-risk multiplier; a breakdown occurs by day 3; consequence routed once; expedition continues.
- **W5 pilot:** scripted raid with and without a defense fixture; pressure differs; missing owner neutral.
- **W6 pilot:** migration fixture active in one sector; route through it shows the shifted encounter mix; the floor distribution holds elsewhere.
- **W7 pilot:** fail a fixture quest, set its gate flag, next day open: reopens once; day 3 still open: no second reopen.
- **W8 pilot:** fixture choice; propagation reaches a near channel by day 2 and a far channel by day 5; suppression fixture stops it.
- **W9 pilot:** fixture belief movement; stance shifts within bounds; no drift by day 7.
- **W10 pilot:** fixture rite; evidence total moves once; standing record shows it; save/load preserves.
- **W11 pilot:** armed trigger with a day-3 threshold and a gate set on day 2: fires once; re-arm explicit.
- **W12:** not a runtime pilot; a harness comparison.

---

# Appendix AG — Architecture decision sketches (per wave, ADR-lite)

Each sketch is the decision the wave makes, the alternative, and the consequence. They are recorded in the handoff as short decision records (the repository's `docs/architecture/` is the home if the integrator promotes any of them to a full ADR).

**AG.1 W1 — Where does spoiled-share → probability translation live?** Chosen: a thin host seam using an authored catalog base. Rejected: (a) putting disease knowledge inside preservation (violates one authority); (b) putting preservation knowledge inside disease (same); (c) UI-side probability (forbidden — offline math). Consequence: the seam is the only cross-domain point and is the place PIR-6 checks the rng.

**AG.2 W2 — Condition upstream of `BookReading` vs inside it.** Chosen: upstream (the ledger stays season-agnostic and testable). Rejected: inside (couples the ledger to a campaign calendar and complicates its unit tests). Consequence: exactly one multiplication site, asserted statically.

**AG.3 W3 — One shared provider vs per-wave providers.** Chosen: one shared (W2 lands it). Rejected: per-wave (drift; the program would hold two band definitions). Consequence: a single file is the program's season authority; changing band semantics is a one-file change.

**AG.4 W4 — Band at breakdown vs continuous risk.** Chosen: a discrete band at the breakdown event. Rejected: continuous risk (changes vehicle math and the dominance table). Consequence: vehicle math untouched; the band's cut points are balance data.

**AG.5 W5 — Projection vs raw values into the resolver.** Chosen: a read-only projection. Rejected: raw defense values (the resolver would need defense knowledge). Consequence: defense owners stay unchanged; the projection is the only coupling.

**AG.6 W6 — Bias vs replacement.** Chosen: bounded bias over authored encounters. Rejected: replacement (deletes authored content; violates the data authority). Consequence: zero pressure reproduces the baseline distribution exactly.

**AG.7 W7 — Predicate in quest records vs hard-coded.** Chosen: data-driven predicate read from records. Rejected: hard-coded (would not survive content changes). Consequence: new quests get the grammar by authoring fields.

**AG.8 W8 — Core placement (DP-CM-3).** Chosen: Core, seeded, tested. Rejected: host-thin (harder determinism story). Consequence: the step is covered by a Core-focused test and the port contract.

**AG.9 W9 — Belief-side cache vs engine-only.** Chosen: engine-only. Rejected: cache (two truths). Consequence: reads always hit the engine; the surface queries it.

**AG.10 W10 — Amend vocabulary vs skip.** Chosen: amend additively when the check fails (DP-CM-4). Rejected: silent skip (dishonest). Consequence: the gate is documented in the handoff either way.

**AG.11 W11 — Own persistence vs flag-family.** Chosen: flag-family persistence. Rejected: own store (a new save section for a primitive). Consequence: no pin change; the primitive is a thin, persisted view over the ledger.

**AG.12 W12 — Retune inline vs file targets.** Chosen: file targets. Rejected: inline (a measurement wave must not also be a design wave). Consequence: balance changes flow through their own signed packages.

---

---

# Appendix AH — Unclaimed-content census integration (standing feed)

The factory treats `docs/plans/UNCLAIMED_CORUS_CENSUS.md` as a standing input (DR-08): authored content that no system consumes is a backlog of *wiring* candidates. This program consumes the census with two rules.

1. **A census row whose domain this program touches is wired in that wave, or explicitly deferred with a reason.** W1 must check the food/disease rows (spoilage, contamination vocabularies); W2/W3 the Year-of-Ash rows; W4 the expedition rows; W8 the gossip rows; W10 the rites rows. If a row is not consumed, the wave handoff says which row and why (e.g., “vocabulary lacks the class — DP-CM-4”).
2. **A census row outside this program's clusters is untouched** — it belongs to another lane (A, C, E) or another plan (PFGL master, Triad B). Cross-claims are a coordination error, not thoroughness.

The census is re-read at each wave's P0 because it changes as other waves land.

---

# Appendix AI — Foreman question pack (program-level)

Six questions, each with its evidence complete, each cheaper than the wave it gates. The factory's Volume 41/52 pattern is followed: the factory recommends a default so silence is not a blocker unless the default is risky.

1. **Q-CM-1 — Is the W1 exposure seam allowed to live in the host?** *Evidence:* Core engine-free rule; the seam is cross-domain arithmetic + a disease call. *Recommendation:* yes — host seam, Core stays clean. *Default if silent:* yes. *Risk if wrong:* low.
2. **Q-CM-2 — Do you want W2 to change the visible dose bands?** *Evidence:* conditioning upstream will move some readings across band edges. *Recommendation:* allow within existing band edges; no band edits. *Default if silent:* allow. *Risk if wrong:* medium (readability of numbers).
3. **Q-CM-3 — Who owns the winter band definition after the program?** *Evidence:* one shared provider (AG.3). *Recommendation:* the Year-of-Ash calendar remains the season authority; the provider is a view. *Default if silent:* same. *Risk:* low.
4. **Q-CM-4 — Is the W4 injury receiver the ward/clinical triage owner or a medical-pipeline intake?** *Evidence:* the exact verb is P0-VERIFY; the authority maps list both. *Recommendation:* the single intake path the medical family already uses; escalate if two exist. *Default if silent:* stop the slice and file, because a wrong receiver is a parallel-authority risk.
5. **Q-CM-5 — May W8's propagation write into the sealed radio surfaces?** *Evidence:* DR-06 seals distress *content*, not the info authority. *Recommendation:* additive writes to the existing rumor owner; no new signal scenarios. *Default if silent:* additive only.
6. **Q-CM-6 — Do you want W12 to run before or after any PFGL wave lands?** *Evidence:* balance deltas are only comparable on a stable base. *Recommendation:* after W1–W4 and before the next balance-affecting wave. *Default if silent:* after W4.

---

# Appendix AJ — Local drift register (CM-DR) — this program's corrections to the authority

| ID | Authority claim | Live finding (this revision) | Correction applied in this plan |
|---|---|---|---|
| CM-DR-01 | B-33 intake advisory is unwired at G1 (Vol 43) | Bridge is live (`PiezometerHostSession.cs:117`); debt RETIRED/sealed | B-33 discarded (§2.2); not planned |
| CM-DR-02 | Scavenging parity is 49 vs 53 | `scavenging_tables` 54, `expeditions` 75, `travel_encounters` 57; no destinations catalog | B-08 re-scoped as data audit, not a mechanics wave |
| CM-DR-03 | B-23 difficulty consumers are this program's | PFGL master W7 / `CF-XP01` own the line | deferred to that owner (C.1) |
| CM-DR-04 | `WildlifeMigrationSystem` is the migration owner (seed B-21 wording) | Live owners are `SeasonalHumanMigrationEngine` + `MigrationConsequenceEngine` | W6 names the live types; P0 re-rg |
| CM-DR-05 | B-15 rites evidence vocabulary may admit a rite class | unknown until the vocabulary is read | W10 hard gate + DP-CM-4 |
| CM-DR-06 | B-07 breakdown consequences route through medical/dose | confirmed absent; the exact injury intake verb is unknown | W4 P0-VERIFY; Q-CM-4 |
| CM-DR-07 | pins quoted in older closeouts (e.g., 246–261) | current pin is 266; surfaces 219/58/161; port seams 307 | pins re-measured, never carried forward (DR-07 lesson) |
| **CM-DR-08** (R5, W3) | BK.1 said W3 “shares W2's” provider (`FalloutWindowProvider`) | live vocabulary has **two different semantics** on the same calendar rows: radiation exposure vs utility load. Reusing the exposure view for filters would equate a diesel-gelling crisis with fallout | **Correction applied:** W3 ships its own pure view `SeasonalPressureProvider` over an additive `pressureMultiplier` column; one calendar authority and one vocabulary remain, the *semantic* is now named instead of conflated. W2 and W3 columns coexist on shared rows (e.g. black blizzard: exposure 1.15 **and** pressure 1.35) |
| **CM-DR-09** (R5, W3) | water filter degradation assumed single-charged | the ReverseOsmosis branch subtracted degradation and the shared tail subtracted it again — RO was charged 2× per batch | **Bug fixed** at the single degradation site; regression test `Water_ReverseOsmosis_IsNoLongerDoubleCharged` pins it; non-RO modes are byte-identical in outcome (the second subtraction was a no-op) |
| **CM-DR-11** (R6, W4) | W4 assumed “a breakdown rolls and returns a bool nobody reads” | live `PrepareForExpedition` **does** roll and **does** return `breakdown`; `PrepareVehicleForDispatch` consumed it to abort the sortie. The real gap is narrower and worse: the breakdown costs the **mission**, never the **crew** | **Correction applied:** the existing tuple API is preserved byte-for-byte; a new `ResolvePrepBreakdown` performs the same consuming travel and returns a typed `VehicleBreakdownOutcome` (`BrokeDown` + crew band). The host routes the band into medical/dose/disease. Pre-W4 behavior (abort, crew unharmed) is the unbound-sink default |
| **CM-DR-12** (R6, W4) | W2’s conditioning site was described as living “in ScribeReading” | W4 needs the same conditioning for expedition exposure; duplicating it would create two sites (the exact anti-pattern the plan forbids) | **Refactor, not duplication:** `BookConditionedExposure` extracted on `DoseLedgerHostSession`; `ScribeReading` and W4 both call it. One site, two callers, identical semantics |
| **CM-DR-14** (R7, W5) | W5 claimed “defense values do not enter siege math” and listed the raid resolver as an **open P0 premise** | the resolver exists and is correctly wired: `DefenseSystem.ResolvePreCombatRaid` is called with the real `_perimeterDefense` and an armory-power predicate (`Main.Plans162_165.cs:383`). The defense mechanic was live all along | **Premise corrected, P0-6 closed, DP-CM-2 confirmed.** W5 was rescoped from “make defenses count” to **“make defenses legible”** — the panel projected strength with `CalculatePerimeterStrength(null, null)`, hiding every wall/turret the player built, and no surface reported the engagement. Both fixed; the underlying resolver was left untouched |
| **CM-DR-16** (R8, W6) | P0-8 listed the travel-encounter selection site as unresolved; the plan sketched a **uniform** bias: `weight = base × (1 + pressure·k)` | the seam exists and is ideal — `TravelEncounterSystem.GetEffectiveWeight`, already the home of stance, faction-war, and patrol-recognition multipliers. But the sketched uniform multiplier is **mathematically inert** in a weighted pick (it scales every candidate identically) and the focused test proved the sampled mix was byte-identical | **Flaw caught by the test and fixed before shipping:** the bias is now **susceptibility-weighted** off authored categories — Human 1.0, Creature 0.6, Environmental 0.25, Chained 0.1 — so it measurably shifts the distribution while staying bounded, deterministic, and non-erasing. The plan's own sketch is corrected, not the tests |
| **CM-DR-18** (R9, W7) | P0-9 listed the quest owner holding failed/abandoned records as unresolved; the plan assumed a predicate over “authored prerequisites” already attached to instances | owner is `QuestRuntimeCoordinator` (reached via `ProceduralNarrativeHostSession.QuestRuntime`, saved by the existing `procedural_narrative` section). Two findings the plan missed: the `Abandoned` lifecycle state had **no transition into it at all**, and instances carry no prerequisite fields of their own | **Both resolved inline (no deferral):** a real `Abandon` transition was added so the grammar has something to reopen; eligibility is read from authored `prereq_quest_id`/`min_day` **actor bindings** at the host, keeping Core free of catalog and flag knowledge. No new save section — the existing section carries the additive instance fields |
| **CM-DR-20** (R10, W8) | W8 was sequenced to *adopt* W11’s one-shot primitive, which does not exist until W11 | W11-core was **built first, inside W8**, per the standing directive to resolve blockades by solution rather than deferral. W11 is now a shipped primitive with its own suite; the W11 wave card in the wave index is fulfilled ahead of schedule | **Ordering resolved inline; nothing deferred.** The primitive persists through the existing flag ledger (no new save section), and W8 is its first adopter — exactly the consumer the design predicted |
| **CM-DR-22** (R11, W9) | the plan named `belief_movements.json` as the authored *movement event* input and assumed belief movement was a catalog lookup | the file is the **3 authored belief-creed definitions**; the live movement authority is the Zealotry owner (`SurvivorBeliefState` + `OnConverted`/`OnCrisisStarted`/`OnCrisisResolved`), and faction standing is `EnsureSharedFactionStance().ModifyTrust` | **Premise corrected before building, then built anyway:** the bridge subscribes to REAL belief events instead of fabricating an event list, and the political affinity was authored additively onto the existing belief definitions (`faction_leaning`) so creed and politics stay in one file — no new catalog, no schema churn, and `--data-integrity-selftest` still reports 425 catalogs / 0 errors |
| **CM-DR-24** (R12, W10) | W10 opened with a **P0 hard gate**: “confirm the Reckoning evidence vocabulary admits a rite fragment (or propose the smallest additive vocabulary change)” — on the assumption a typed vocabulary existed | there is **no typed vocabulary at all**. `enrolledEvidence` is an untyped counter documented as *read machine-log fragments*, and `VerdictAccusationSystem` reads it to build culpability; the `VerdictEvidenceChain` docstring says the same. The state carries other distinct traces as separate counters (`dwellingDriftTotal`, dose), with “Distinct from the lore evidence ledger” in the source | **Gate resolved by evidence, and the plan's implicit answer refused.** Enrolling rites as machine-log evidence would make grief a prosecutorial instrument and the readout a liar — semantic drift dressed as a feature. The smallest honest amendment is the one the state already models: a **distinct additive trace kind** (`riteTraceTotal` + `EnrollRiteTrace`) in the existing section. No vocabulary redefinition, no accusation retune, no new store |
| **CM-DR-25** (R12, W10) | the plan proposed a “performance-id guard” and a surface change, with the call site presumed to be the death path in `Main.Campaign.cs` | `Main.Campaign.cs` is shared and actively claimed by another integrator package; and subscribing at the call site would miss every rite performed by any other path | **Seam moved upstream to the canonical event** `SpiritualMeaningCoordinator.OnMemorialRitePerformed`, which covers every performance regardless of caller, and touches no shared file. Once-per-(mourner, rite) is enforced by the W11 one-shot primitive over the campaign consequence ledger, so a repeated vigil cannot inflate the register and the guard survives save/load |

The register is append-only per revision; corrections cite the live probe, not the authority text.

---

# Appendix AK — Content authoring guide per wave (data-first deep pass)

**AK.1 W1 foodborne exposure rows.** Location: `disease_catalog.json`, `exposure_sources`. Fields follow the existing row shape **[P0-VERIFY]**: a source id, the disease id(s) reachable through it, a per-mille probability at full spoiled share, a class label used by the seam, and a one-line rationale. Authoring rules: one row per food class you can defend from the catalog's own food ids; probabilities are *base at full exposure* and the seam scales by spoiled share; no rows for classes the game cannot produce; rationale recorded in the handoff.

**AK.2 W2 window rows.** Location: the Year-of-Ash events vocabulary (`year_of_ash_events.json`) or the owning window rows **[P0-VERIFY]**. Fields: window id, day range, exposure multiplier (per-mille ratio), rationale. Authoring rules: windows align to the campaign spine's known anchors (the Day-190 alarm-clock event per the authority; the 180–360 canon); multiplier ≥ 1.0 always (winter is pressure, never a gift); no overlapping duplicate windows (the provider picks the highest-pressure matching window, deterministically).

**AK.3 W3 curve rows.** Location: each chosen owner's existing tuning catalog. Fields follow that catalog's convention **[P0-VERIFY]**. Authoring rules: curve is a pure day→band function; band 1.0 outside the window; boundary days explicit; hardening offset expressed in the owner's own vocabulary (never a second scalar).

**AK.4 W4 band rows.** Location: the expedition's existing balance/tuning rows. Authoring rules: cut points reference `EXPEDITION_VEHICLE_DOMINANCE_TABLE.md` so repair-heavy builds do not trivialize breakdowns (CM-DR-06 relates); the band is data, the resolution is code.

**AK.5 W6 (no new rows).** Bias constant is code-side and bounded; encounters remain authored. If a corridor-specific encounter is ever wanted, it goes through `travel_encounters.json` and the integrity gate (not through the bridge).

**AK.6 W9 translation rows.** Location: a new authored table in the faction/belief data **[P0-VERIFY owner]**. Fields: belief movement id, faction posture, bounded trust delta, rationale. Authoring rules: deltas small enough that N movements per day cannot swing a faction across stances without player intent; no belief-side trust values.

**AK.7 W10 (vocabulary-first).** If the Reckoning vocabulary admits a rite fragment class, no new rows are needed — the enrollment consumes it. If not, the amendment is vocabulary-only and additive.

**AK.8 W11 (no rows).** The primitive is code; armed triggers are runtime state, not content.

**AK.9 W12 (report only).** The refresh publishes deltas; it authors nothing.

---

# Appendix AL — Surface wireframes (textual, per wave)

The wireframes are sketches for the implementer and the a11y reviewer; they expose commands and truthful state only.

**AL.1 W1 Food safety strip (bind into the existing food/kitchen surface).**
`[ FOOD SAFETY ]  Stock: 412 preserved · 38 spoiled (9%)   Storage: 4°C (shelf-life ×1.4)   [ DISCARD SPOILED ]`
Row detail on hover/expand: item, tier, share spoiled, last cure. If the surface does not exist **[P0-VERIFY]**, the minimal honest surface is this strip plus the discard action — no new gameplay authority.

**AL.2 W2 Dose register line.**
`[ READING ]  Day 214 · 0.87 mSv · band W-2 · window: ASH-STORM-2 (×1.8) · dosimeter D-3 · antirad: pre ` with a footnote for the band's counterplay.

**AL.3 W3 Power/water band chip.**
`[ WINTER ]  Band: severe (×1.6) · hardening offsets ×0.7 · net ×1.12` on the chosen owners' panels; the chip is read-only; the counterplay lives on the existing upgrade paths.

**AL.4 W4 Expedition preflight card.**
`[ VEHICLE ]  breakdown risk 18% (track gear worn) · repair now: 12 scrap · last consequence: none ` and a post-event line: `breakdown: minor injury (survivor X) · treated in ward`.

**AL.5 W5 Defense projection.**
`[ PERIMETER ]  projection 0.62 · next raid pressure: high → reduced to moderate ` with the components listed (no hidden math).

**AL.6 W6 Route corridor readout.**
`[ ROUTE ]  sector 7 · migration: herd crossing (pressure 0.5) · expected encounter mix shifted `.

**AL.7 W7 Reopen notice.**
`[ THREAD ]  "Convoy ledger" — reopened: prerequisite discovery acquired ` with the reason visible.

**AL.8 W8 Gossip arrival.**
`[ RADIO / NOTE ]  a rumor reached [faction] via [channel] after N days ` (arrival, not the full text; the existing surfaces own the voice).

**AL.9 W9 Stance explanation.**
`[ FACTION ]  trust +0.03 (belief movement: "the rite of ash") — stance unchanged ` (legibility: the player sees why nothing moved yet).

**AL.10 W10 Evidence entry.**
`[ RECKONING ]  rites performed: N · evidence fragments enrolled ` with the names of the mourned where the standing record supports it.

---

---

# Appendix AM — Historical integration precedent (how this repository seals a gap)

Every claim below is drawn from `WORKTREE_OWNERSHIP.md` claim rows (2026-09-24, all DONE/HANDED_OFF) and is the pattern this program follows.

**AM.1 The sealed-plan shape.** A claim names: Core (owner + census + section + pin), Host (session + save store + 12-check probe), Main (partial + day owner + save orchestration + lifecycle), Tests (plan + owner suites + save pin), Generated (architecture map, selftest manifest, port contract, plan audit), Governance (claim + ledger + DEC). The program adopts this shape per wave; where a wave needs no save section (all of them), the save-pin's absence is stated explicitly rather than omitted.

**AM.2 The evidence shape.** Closed claims cite: probe 12/12, focused suites with counts, save round-trip with the pin, triad drift gate PASS, architecture map percentage, plan-integration audit count. This program's waves cite the same evidence classes (Appendix T) so the ledger rows look like every other row in the repository.

**AM.3 The concurrency precedent.** Two expansion waves (30–31 and 25–29) landed in single claims; the Night Watch claim explicitly notes "existing 186/188 shared files were transferred without deleting or rewriting their landed work." This program follows the same rule: shared seams are edited in one owned burst, never mass-reformatted, never reverted.

**AM.4 The honesty precedent.** A prior claim deferred its panel and UI with named reasons rather than shipping a half-surface. This program prefers a bindable strip (AL.1–AL.10) over a new panel, and names the deferral when a surface does not exist.

**AM.5 The lesson precedent.** The 186/188 claim handed off "shared composition seams" to a later claim when the seam's owner changed. This program sequences W1→W4 and W11→W8/W10 for the same reason: the first owner lands the seam, the second consumes it.

---

# Appendix AN — Balance philosophy for the core loop (what the sealed mechanics should feel like)

1. **Pressure should be legible before it is lethal.** Every wave's band, risk, or window is visible before the consequence lands. A player who is hit by winter should be able to say “I saw the band and I did not harden” — not “the game decided.”
2. **Counterplay should be purchasable or schedulable, not lucky.** W1 counterplay is preparation (cure/cold/discard); W2 is scheduling + antirad; W3 is capital (hardening); W4 is maintenance; W5 is fortification; W6 is scouting; W7 is pursuit; W8 is channel control; W9 is outreach; W10 is the rite itself. None of them is a dice the player cannot influence.
3. **Recovery must exist.** Illness is treatable (ward), exposure is bounded (dosimeter, shielding, antirad), breakdowns are survivable (track gear, abort). The core loop should punish neglect, not end runs.
4. **One knob per concern.** Difficulty scalars (PFGL W7), season bands (W2/W3), and per-owner tuning are distinct authorities. A retune in one must not silently move another.
5. **Measurement before tuning.** W12 exists because the factory's balance rules and this program's instinct agree: numbers move in packages, never inline.

---

# Appendix AO — Accessibility and input contracts per wave

- **Keyboard/controller parity.** Any new action (discard, re-arm, suppress, repair-now, choose consumer) is reachable by keyboard and controller through the existing input map; the 22-action map is not extended by this program.
- **Focus order.** New controls follow the existing panel conventions; modal choices (e.g., routing around a corridor) trap and release focus correctly.
- **Status is never color-only.** Bands, risks, and biases are text + icon + color; the text carries the meaning.
- **Visible feedback.** Every action produces an immediate readable outcome (journal line + surface change), not only a day-later state.
- **Disposal.** Panels unsubscribe on close; the lifecycle gate covers it.
- **Reduced-motion / audio.** New journal facts do not require audio; if a cue is added, the accessibility settings (Plan 184) govern it.

---

# Appendix AP — Localization readiness per wave

The program introduces a small, bounded set of new user-facing strings: surface labels (AL.1–AL.10) and journal fact templates. Contract: strings are authored in the existing localization surface **[P0-VERIFY: the L10N seam]**, use existing keys where they exist, add new keys with placeholders rather than concatenation, and carry no real-world war names or copied text. No string-freeze violation: the program adds keys, never edits frozen ones **[P0-VERIFY freeze state — the ledger says string freeze is decision-blocked]**. If the freeze is still active at a wave's P0, new strings are journal facts only until the freeze decision resolves.

---

# Appendix AQ — Performance budget per wave

- **W1:** one spoiled-share query + one exposure call per eat event — negligible; assert no per-unit loop.
- **W2/W3:** pure day lookups; the provider is a dictionary or small table — no per-tick allocation concern; the consumer multiplies a float.
- **W4:** one band resolution per breakdown event.
- **W5/W6:** one projection/bias computation per resolution/selection.
- **W7:** predicate evaluation on flag change and day open — O(quest records with failed/abandoned status), not O(all quests).
- **W8:** the propagation step is the only wave with per-tick cost; it iterates pending seeds with a window guard; a per-tick budget is asserted in its test (no unbounded fan-out).
- **W9–W12:** negligible or documentation-only.

No optimization without before/after numbers (factory Lane F rule); if W8's step shows cost, it is measured, not preemptively rewritten.

---

# Appendix AR — Mod-support considerations

- **Data-first rows are mod-visible.** W1's exposure rows, W2/W3's bands, W4's cut points, and W9's translation table are content, so mods can extend them through the existing mod specification (Plan 165) without code.
- **No new mod API surface.** The program adds no new public mod hooks; a mod that wants a new mechanic seam waits for the wave that owns it.
- **Conflict surface.** W9's translation table and W1's exposure rows are the two most conflict-prone additions; both are namespaced by their owning catalogs and validated by the integrity gate.
- **Determinism guarantee for mods.** A mod supplying rows cannot break determinism: every consumer is a pure function of data + seeded stream.

---

# Appendix AS — Code review checklist (per wave, used at handoff review)

1. Does every new state field have an owner? (No anonymous fields.)
2. Does every consequence land in an existing owner? (Receiver contract cited.)
3. Are all new rng draws from an existing, named stream?
4. Is every day-tick consequence guarded against double application?
5. Do all new save interactions reuse an existing section?
6. Is the port contract classification updated in the same change?
7. Are the new strings localization-safe (keys, placeholders, no frozen-string edits)?
8. Does the focused test file prove polarity *and* exactly-once *and* fail-closed behavior?
9. Is the manual script honest about what it shows (no staged outcomes)?
10. Are the handoff, claims, and backlog delta complete?

---

# Appendix AT — Revision ledger and pass record

| Revision | Pass | Content | Size after pass |
|---|---|---|---|
| R1 draft | authoring | executive contract, premise sweep, 12 waves, contracts, appendices A–R | 78,327 chars |
| R1 polish-1 | structure | pass history, integration-at-a-glance architecture, uniformity check | ~85k |
| R1 polish-2 | depth | runbooks J, seed traceability K, worked examples L, authoring M, surfaces N, test doctrine O, coordination P, closeout Q, glossary R, evidence cards S, acceptance T, failure U, wiring V, value W, epilogue X, sequencing Y, decision packets Z | ~145k |
| R1 polish-3 | final depth | receiver contracts AA, determinism AB, save AC, observability AD, day-slices AE, pilots AF, decision sketches AG, census AH, foreman pack AI, drift register AJ, authoring guide AK, wireframes AL, precedent AM, balance philosophy AN, a11y AO, l10n AP, perf AQ, mods AR, review checklist AS | (this pass) |
| R1 precision-3 | live re-probe | every named verb/owner/pin re-verified; drift rows recorded in AJ/D | (next step) |

**Growth honesty (factory scale clause).** Expansion here is evidence: runbooks, contracts, matrices, worked examples, and decision packets. No section is padding; if a future revision cannot add verified content, the revision does not happen (the authority's Part 0 discipline, restated).

---

---

# Appendix AU — Core mechanics inventory survey (the comprehensive gap map)

> The twelve program waves come from the factory seeds. This appendix surveys the **whole core loop** so the program is not mistaken for the complete list, and so every domain has an explicit disposition. Method: live host-reference counts (`rg -l` per type), save-key presence, and surface classification; spot-checked this revision, with **[SURVEY]** marking rows that carry only authority-map evidence and must be spot-checked before any future plan promotes them.

**AU.1 Survey table.**
| Domain | Owner (live) | Host refs | Save | Surface | Disposition |
|---|---|---:|---|---|---|
| Survival needs | `NeedsSystem` | 33 | needs family | survivor detail | sealed (heavy consumer) |
| Nutrition / eating | kitchen nutrition host; `FoodPreservationSystem` | 3 + 1 (preservation Main-only) | nutrition + preservation | kitchen | **W1 extends preservation→disease; eating path is the seam** |
| Water | `WaterTreatmentSystem` | 9 | water family | water surface | sealed; **W3 adds winter band to one water consumer** |
| Power / industry | `PowerGridSystem` family | 18 | `power_grid` | power surfaces | sealed; **W3 adds winter band to one power consumer** |
| Agriculture | greenhouse family | 8 | `greenhouse` | greenhouse | sealed (out of program) |
| Medical | `MedicalWardHostSession` family | 5 | `medical_ward` | medical | sealed; **W4 routes injury here** |
| Radiation | `DoseLedgerSystem` family | 5 | dose family | dose register | **W2 adds storm windows; W4 routes exposure here** |
| Disease | disease family | (host) | disease save | medical | **W1/W4 feed it** |
| Morale | `MoraleContagionSystem` + Needs Morale | 2 | `morale_contagion` | social surfaces | sealed (companion plans cover player surfaces) |
| Shelter thermal / atmosphere | `ShelterThermalSystem` family | 6 | `shelter_thermal` | shelter | sealed (PFGL master W10 covers sleep) |
| Weather / Year of Ash | `WeatherSystem` family | 28 | weather family | weather | **W2/W3 read the calendar; no new weather state** |
| Expeditions | `ExpeditionSystem` family | 28 | `expeditions` | expedition | **W4 adds consequences; W6 biases encounters** |
| Inventory | `InventorySystem` | 2 | inventory | — | sealed; W1 reads it (eat path) |
| Factions / politics | `FactionStanceEngine` family | (host) | faction family | factions | **W9 feeds the engine** |
| Information | radio/rumor family | (host) | radio family | radio | **W8 feeds the info owner** |
| Quests | quest owners | (host) | quest family | quest | **W7 adds reopen grammar** |
| Endgame | Reckoning/verdict family | (host) | endgame family | verdict | **W10 feeds evidence** |
| Nutrition catalog parity | `NutritionSystem` | 0 (named type absent; kitchen host owns it) | `nutrition` | kitchen | authority-name drift **[SURVEY]** — no action; the kitchen host is the live owner |
| Survival needs save key | — | — | `survivors_needs` not a save key (needs ride survivor save) | — | recorded to prevent a future “missing section” bug report |

**AU.2 What the survey proves.** The core loop is *deep but sparsely connected at the seams*. Heavy host-reference counts (needs 33, expeditions 28, weather 28, power 18) show mature integration; the program’s twelve waves live exactly where counts are low and consequences are missing (preservation 1, dose-season coupling 0, winter power/water 0, breakdown routing 0, defense→siege 0, migration→encounters 0, quest reopen 0, gossip propagation 0, belief→stance 0, rites→Reckoning 0, one-shot primitive 0). This is the factory's “unconnected mechanics” class stated as a count.

**AU.3 Promotion rule for future waves.** A domain not in this program can join only with: a live premise sweep row (§2.2 format), a one-owner receiving contract (AA format), a PIR record, and a tier declaration (BU format). The anti-nonsense filter applies to the program’s own future growth exactly as the factory applies it to its seed register.

---

# Appendix AV — Per-wave test case catalogs (implementer-facing)

Each case is a single focused test method; the file runs alone first. Cases marked **(new)** do not exist yet.

**AV.1 W1 Foodborne.**
1. Eat clean preserved stock → no exposure call (new). 2. Eat stock with 40% spoiled share → one exposure call with the catalog-scaled probability (new). 3. Consume 3 units → still one call (new). 4. Discard before eating → zero spoiled share → no call (new). 5. Immune survivor → `TryExpose` returns blocked (new). 6. Preservation owner absent → no call + journal fact (new). 7. No catalog row for the class → no call + integrity census names the gap (new). 8. Two-pass determinism of the exposure roll (new). 9. Holdfast consumption regression (existing). 10. Save/load between eat and outbreak (new).

**AV.2 W2 Dose window.** 1. Identical nominal inside vs outside window → different booked dose or band (new). 2. Band 1.0 outside window is identity (new). 3. Shielding factor still scales (new). 4. Antirad before+after still reduces (new). 5. Missing calendar → neutral + fact (new). 6. `BookReading` called with the conditioned nominal exactly once per booking (static assertion + new). 7. Two-pass determinism (new). 8. Band edges unchanged (regression on `BandFor`).

**AV.3 W3 Winter pressure.** 1. Provider band at days 179/180/360/361 (new). 2. Band 1.0 identity outside (new). 3. Water consumer draw scales at the band (new). 4. Power consumer draw scales at the band (new). 5. Hardening offset within tolerance (new). 6. Missing calendar neutral (new). 7. One multiplication per consumer (new). 8. Two-pass determinism (new).

**AV.4 W4 Breakdown.** 1. Band resolution pure function (new). 2. No-injury band routes nowhere (new). 3. Injury band routes to the medical intake once (new). 4. Exposure band books dose once (new). 5. Contamination band calls `TryExpose` once (new). 6. Repair lowers the band (new). 7. Event-id guard holds across retick (new). 8. Save/load mid-event does not re-route (new). 9. Missing receivers fail closed (new). 10. Two-pass determinism (new).

**AV.5 W5 Defense siege.** 1. Stronger projection reduces raid pressure (new). 2. Zero defense = baseline pressure (new). 3. Missing defense owner neutral (new). 4. Projection read once per resolution (new). 5. Two-pass determinism (new).

**AV.6 W6 Migration encounters.** 1. Zero pressure reproduces baseline distribution over a seeded sample (new). 2. Pressure shifts the mix within bounds (new). 3. Bound constant enforced (new). 4. Ordinal ordering (new). 5. Missing migration owner identity bias (new). 6. Two-pass determinism (new).

**AV.7 W7 Quest reopen.** 1. Failed quest + satisfied conditions reopens once (new). 2. Unsatisfied conditions do not reopen (new). 3. Reopened quest does not reopen again (new). 4. min_day boundary (new). 5. Save/load safety (new). 6. Flag namespace collision guard (new). 7. No reopen of completed quests (new).

**AV.8 W8 Gossip.** 1. Near channel arrives by day 2 (new). 2. Far channel arrives by day 5 (new). 3. Suppression stops propagation (new). 4. One emission per window (new). 5. Empty gossip no change (new). 6. Ordinal ordering (new). 7. Two-pass determinism (new). 8. No second persistence (static).

**AV.9 W9 Belief stance.** 1. Movement shifts trust through the engine (new). 2. Clamp holds over repeated days (new). 3. No belief-side cache (static). 4. `ModifyTrust` is the only write (static). 5. Two-pass determinism (new).

**AV.10 W10 Rites.** 1. Performed rite enrolls once (new). 2. Evidence totals move (new). 3. Save/load safety (new). 4. Unknown rite class inert (new). 5. Vocabulary gate recorded (process, not test).

**AV.11 W11 One-shot.** 1. Fires once (new). 2. Respects day threshold (new). 3. Respects gate flag (new). 4. Survives save/load (new). 5. Re-arm explicit (new). 6. Double `TryFire` in one tick fires once (new). 7. No new save section (static).

**AV.12 W12 Balance.** 1. Paired runs produce deltas (report). 2. Seeds pinned (process). 3. No product-code diff (process).

---

---

# Appendix AW — Wiring sketches (pseudocode per wave; implementers refine, not copy-paste)

**AW.1 W1 Foodborne seam (host-thin).**
```
// on successful eat, inside the existing ConsumeFoodResult path
var preservation = _main.FoodPreservationSystem;                 // existing accessor
if (preservation == null) { Journal("food.spoilage_exposure_skipped", "owner_absent"); }
else {
  int total = preservation.GetTotalFood(itemId);
  int spoiled = preservation.GetSpoiledFood(itemId);
  float share = total > 0 ? (float)spoiled / total : 0f;
  if (share > 0f && survivorId != null) {
    var source = diseaseCatalog.GetExposure(FoodborneClassFor(itemId));   // authored row
    if (source != null) {
      var ctx = DiseaseExposureContext.ForFoodborne(survivorId, source, share);
      var result = diseaseSystem.TryExpose(ctx);                            // rng owned by disease
      Journal("food.spoilage_exposure", survivorId, itemId, share, result.Reason);
    } else { Journal("food.spoilage_exposure_missing_row", itemId); }
  }
}
```
Notes: one call per event; no mutation of preservation state; the disease catalog is the only place probabilities live.

**AW.2 W2 Window conditioning (upstream of `BookReading`).**
```
var band = falloutWindowProvider.MultiplierFor(day);        // pure; 1.0 when calendar absent
var nominal = baseMsv * band;
var result = doseLedger.BookReading(survivorId, day, nominal, source, highEnergy, antiRadBefore, antiRadAfter, rng);
if (band != 1.0f) Journal("dose.storm_window", day, band, result.Band);
```

**AW.3 W3 Consumer seam (each chosen owner).**
```
var band = seasonalPressureProvider.BandFor(day);           // shared with W2's calendar view
var draw = owner.BaseDraw();                                // existing math
var adjusted = draw * band.Value;                           // exactly one multiplication
owner.Consume(adjusted);
if (band.ChangedThisDay) Journal(owner.JournalKey, band);
```

**AW.4 W4 Breakdown consequence.**
```
var breakdown = expeditionEvents.PollBreakdown();           // existing event stream
if (breakdown != null) {
  var band = ResolveBand(breakdown, expeditionRng);          // pure + one roll
  switch (band.Kind) {
    case None: break;
    case Injury: medicalIntake.Add(breakdown.SurvivorId, band.Severity, day); break;
    case Exposure: doseLedger.BookReading(..., nominal: band.MsV, ..., expeditionRng); break;
    case Contamination: disease.TryExpose(...); break;
  }
  expeditionEvents.MarkConsequenceRecorded(breakdown.EventId);  // exactly-once guard
  Journal("expedition.breakdown_consequence", breakdown.EventId, band.Kind);
}
```

**AW.5 W5 Siege projection.**
```
var projection = defenseProjection.Compute();                // read-only over defense owners
var pressure = raidResolver.RaidPressure * projection.Weight;  // one consumption point
raidResolver.Resolve(pressure, ...);
```

**AW.6 W6 Encounter bias.**
```
var pressure = migrationReadModel.PressureFor(sectorId);     // 0..1
var candidates = encounters.ForSector(sectorId).OrderBy(id => id, StringComparer.Ordinal);
foreach (var c in candidates) c.Weight = c.BaseWeight * (1f + pressure * BiasK);
var picked = travelRng.Pick(candidates);
```

**AW.7 W7 Reopen predicate.**
```
foreach (var q in questOwner.Records.Where(q => q.Status is Failed or Abandoned)) {
  if (oneShots.HasFired($"quest.reopened.{q.Id}")) continue;
  if (day < q.MinDay) continue;
  if (q.PrereqFlags.All(flags.IsSet) && questOwner.CanTransition(q, Reopened)) {
    questOwner.Transition(q, Reopened); oneShots.Fire($"quest.reopened.{q.Id}", day);
    Journal("quest.reopened", q.Id, day);
  }
}
```

**AW.8 W8 Propagation.**
```
foreach (var seed in pendingGossip.OrderBy(s => s.Id, StringComparer.Ordinal)) {
  if (oneShots.HasFired($"gossip.emitted.{seed.Id}.{seed.ChannelWindow}")) continue;
  var days = day - seed.Day; var distance = channelDistance(seed.Source, seed.Channel);
  if (days * seed.Speed >= distance && !suppressed(seed)) {
    infoOwner.Emit(seed.AsRumor(infoRng));
    oneShots.Fire($"gossip.emitted.{seed.Id}.{seed.ChannelWindow}", day);
    Journal("gossip.propagated", seed.Id, seed.Channel, day);
  }
}
```

**AW.9 W9 Belief → stance.**
```
foreach (var mv in beliefMovements.ObservedToday) {
  var delta = translationTable.DeltaFor(mv.Id, factionPosture);   // bounded, authored
  if (Math.Abs(delta) > 0) { stanceEngine.ModifyTrust(mv.Faction, delta); Journal("faction.belief_stance_shift", mv.Id, delta); }
}
```

**AW.10 W10 Rite enrollment.**
```
if (ritePerformance != null && !oneShots.HasFired($"rite.enrolled.{ritePerformance.Id}")) {
  if (reckoningEvidenceVocabulary.Admits(RiteFragment)) {
    reckoning.EnrollEvidence(1);
    oneShots.Fire($"rite.enrolled.{ritePerformance.Id}", day);
    Journal("reckoning.rite_enrolled", ritePerformance.Id);
  } else { StopAndFile(DP-CM-4); }
}
```

**AW.11 W11 Primitive.**
```
public bool TryFire(string triggerId, int day) {
  var armed = _ledger.GetCounter($"trigger.armed.{triggerId}") > 0;
  var threshold = _ledger.GetCounter($"trigger.day.{triggerId}");
  var gate = _ledger.IsSet($"trigger.gate.{triggerId}");
  if (!armed || day < threshold || !gate) return false;
  if (_ledger.IsSet($"trigger.fired.{triggerId}")) return false;   // one-shot by construction
  _ledger.Set($"trigger.fired.{triggerId}", "core", "one-shot", day);
  return true;
}
```

**AW.12 W12 Harness.** Paired seeded runs; the report records seed, commit, inputs, deltas, and file locations; retune targets are filed as separate proposals (AN.4).

---

# Appendix AX — Integration readiness scoreboard (per wave, ten dimensions)

Each dimension is scored **ready / conditional / blocked** before a wave's claim opens. A wave may not start with any **blocked** dimension; **conditional** dimensions name their condition.

| Wave | Premises live | Receivers named | Save plan | Determinism plan | Test plan | Surface plan | Strings plan | Claims clear | Decisions open | Verdict |
|---|---|---|---|---|---|---|---|---|---|---|
| W1 | ready | ready | ready | conditional (rng from disease owner) | ready | conditional (surface existence **[P0-VERIFY]**) | conditional (freeze state) | ready | none | **start after P0** |
| W2 | ready | ready | ready | ready | ready | ready (dose register) | conditional | ready | none | **ready** |
| W3 | ready | conditional (DP-CM-1) | ready | ready | ready | ready | conditional | ready | DP-CM-1 (default given) | conditional |
| W4 | conditional (injury verb **[P0-VERIFY]**) | conditional (Q-CM-4) | ready | ready | ready | ready | conditional | ready | Q-CM-4 (stop-slice default) | conditional |
| W5 | conditional (resolver **[P0-VERIFY]**) | conditional (DP-CM-2) | ready | ready | ready | ready | conditional | ready | DP-CM-2 | conditional |
| W6 | conditional (selector **[P0-VERIFY]**) | ready | ready | ready | ready | ready | conditional | ready | none | conditional |
| W7 | ready | ready | ready | ready | ready | ready | conditional | ready | none | **ready** |
| W8 | ready | conditional (DP-CM-3) | ready | ready | ready | ready | conditional | ready | DP-CM-3 (default given) | conditional |
| W9 | ready | ready | ready | ready | ready | ready | conditional | ready | none | **ready** |
| W10 | conditional (vocabulary gate) | ready | ready | ready | ready | ready | conditional | ready | DP-CM-4 (stop default) | conditional |
| W11 | ready | ready | ready | ready | ready | n/a | n/a | ready | DP-CM-5 (default given) | **ready** |
| W12 | ready | n/a | n/a | ready | ready | n/a | n/a | ready | Q-CM-6 (default given) | **ready** |

Reading: the four core waves are startable with a single-day P0; the social/grammar waves are startable after their one named decision or P0 check. No wave is blocked.

---

# Appendix AY — Cross-plan conflict map (this program vs the player-facing family)

| Concern | This program | PFGL master (R5) | Triad B (R1) | Combat tetrad | Rule |
|---|---|---|---|---|---|
| Food preservation → disease | **W1** | — | — | — | W1 owns it; the master plan does not touch it |
| Dose/storm windows | **W2** | — | — | — | W2 owns it |
| Winter power/water | **W3** | — | — | — | W3 owns it |
| Breakdown consequences | **W4** | W3 (vehicle modules UI) | — | — | W4 touches consequences; PFGL W3 touches the garage surface — different files, sequence if both claim vehicle files |
| Defense / siege | **W5** | — | — | — | W5 owns defense values at the siege seam |
| Migration / encounters | **W6** | W4 (trade/colony) | — | — | different owners |
| Quest reopen | **W7** | — | — | — | W7 owns grammar |
| Gossip | **W8** | — | — | — | W8 owns propagation; sealed radio owners are additive-only |
| Belief/stance | **W9** | W6 (ideology/NPC) | — | — | W9 is faction stance; PFGL W6 is ideological friction — different owners |
| Rites/Reckoning | **W10** | — | — | — | W10 owns evidence enrollment |
| One-shot primitive | **W11** | — | — | — | W11 is shared infrastructure; adopters declare |
| Difficulty consumers | deferred | **W7/CF-XP01** | — | — | PFGL owns; this program does not touch |
| Exercise/dream/sanitation | — | — | **Triad B** | — | untouched here |
| Combat clock/physics | — | — | — | **tetrad** | untouched here |
| Shared hubs | claims per wave | claims per wave | claims per wave | claims per wave | one hub-holder at a time (P.1) |

---

# Appendix AZ — Precision pass record (live re-verification, this revision)

Every symbol, owner name, pin, and probe cited by this plan was re-probed against live source after the draft. Results:

| Cited item | Re-probe | Result |
|---|---|---|
| Save pin 266 (`SaveSectionRegistry.All.Count`) | `rg 'Assert.Equal(26[0-9]' Ashfall.Core.Tests/Save` | 266 at `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs:315,317` |
| Surfaces 219/58/161 | `player_surface_manifest.json` parse | confirmed |
| Port seams 307 | `port_contract_policy.json` `total_seams` | confirmed |
| `RegisterContaminationAdvisory` wired | `rg` | `PiezometerHostSession.cs:117` (B-33 discarded) |
| Scavenging 54 / expeditions 75 / encounters 57 | catalog parse | confirmed (CM-DR-02) |
| `FoodPreservationSystem` no disease ref | `rg` | confirmed (W1 gap) |
| `ConsumeFood` no player-path caller; `HoldfastRuntimeSession.ConsumeFoodResult` is the path | `rg` | confirmed (W1 seam) |
| `DiseaseSystem.TryExpose:437` `TryInfect:491` `TriggerOutbreak:654` `TickDaily:737` | `rg` | confirmed |
| `disease_catalog.exposure_sources` `:347,384,411` | `rg` | confirmed |
| `DoseLedgerSystem` no `YearOfAsh` ref; `BookReading` signature at `:122` | `rg`/`sed` | confirmed (W2 gap) |
| `YearOfAshCatalogLoader.LoadEvents:207`; `year_of_ash_events.json` | `rg`/ls | confirmed |
| No `YearOfAsh` in `*Power*`/`*Water*` Core files | `rg` | confirmed (W3 gap) |
| `ExpeditionVehicleSystem` risk/repair without routing | `sed` | confirmed (W4 gap) |
| `DefenseGrid` consumers = UI + `Main.Plans162_165` | `rg` | confirmed (W5 gap) |
| Migration owners = `SeasonalHumanMigrationEngine`, `MigrationConsequenceEngine` | `find`/`rg` | confirmed (CM-DR-04) |
| No quest reopen logic | `rg -in reopen` | confirmed (W7 gap) |
| `moral_choice_gossip` read-side only | `rg` | confirmed (W8 gap) |
| `FactionStanceEngine` verbs | `rg` | confirmed (W9 receiver) |
| `EnrollEvidence` no rite caller | `rg` | confirmed (W10 gap) |
| `IFlagLedger` has no day-thresholded one-shot | `sed` | confirmed (W11 gap) |
| Core host-ref counts for the inventory (needs 33, expeditions 28, weather 28, power 18, water 9, greenhouse 8, thermal 6, medical 5, dose 5, inventory 2, preservation 1) | `rg -l` per type | confirmed (AU.1) |
| `WORKTREE_OWNERSHIP.md` rows all DONE/HANDED_OFF | head/read | confirmed; Wave 1 claim available |

**Precision-pass outcomes:** zero stale verb names found in §7/§8 (all names match live); three authority-drift corrections recorded (CM-DR-01/02/04); seven P0-VERIFY markers remain **by design** (they are implementer-time checks for symbols whose live location is intentionally not pinned by this plan: the W1 food-surface existence, the W4 medical intake verb, the W5 resolver, the W6 selector, the W10 vocabulary, the string-freeze state, and the mod/l10n seams). The architecture tightening from this pass: the receiver-contract appendix (AA) now states, per receiver, what a consumer may and may not do — the change that most reduces integration risk, because it converts “extend the owner” into “behave correctly toward the owner.”

---

# Appendix BA — Rollback drills (per wave)

A rollback section is only real if it has been imagined under failure. Each drill is a two-minute thought experiment run at P0.

- **W1:** the eat path starts double-exposing → revert the seam block (one contiguous edit); catalog rows may stay (unused rows are inert but must be justified).
- **W2:** bands become unreadable → revert the one multiplication; window data stays (inert).
- **W3:** a consumer double-scales → revert that consumer; provider stays for the other.
- **W4:** consequence volumes explode → revert routing; the band function can stay (pure) or go.
- **W5:** siege outcomes swing too hard → revert the resolver seam; projection is read-only and can stay.
- **W6:** encounters feel wrong → revert the bias; migration state is untouched by the wave.
- **W7:** quests reopen in loops → revert the evaluation hook; predicate is inert without the hook.
- **W8:** rumor economy floods → revert the propagation step call; seeds remain data.
- **W9:** trust swings → revert the hook; translation data is inert.
- **W10:** evidence totals distort → revert enrollment; vocabulary unchanged unless amended (then the amendment is separate).
- **W11:** adopters misbehave → adopters revert to their prior guards; the primitive is additive.
- **W12:** nothing to roll back (report can be re-run).

Rule: no wave's rollback requires reverting another wave's landed work (the AM.5 lesson).

---

# Appendix BB — Knowledge capture: what future plans must not redo

1. The **intake-advisory bridge is done** (CM-DR-01) — do not plan it.
2. **Scavenging parity is a data audit** with different numbers than the authority quotes (CM-DR-02) — re-measure before planning.
3. **Difficulty consumer binding belongs to PFGL W7/CF-XP01** (CM-DR-03) — coordinate, do not duplicate.
4. The **migration owners are the `Seasonal*`/`MigrationConsequence` engines** (CM-DR-04) — the old name is drift.
5. **Pins move** (CM-DR-07) — re-measure 266 at every claim; never carry a number forward.
6. The **core loop is deep but sparsely connected at seams** (AU.2) — the next gaps will be found by counting *consequences*, not systems.
7. **The unstarted-type cluster (B-34) is still unstarted** (campaign age clock, schedule-hour consumer, crisis producer, last-interaction stamp, creation→vault, medical record) — it remains a foreman prioritization question, not this program's waves; when promoted, it should reuse the receiver-contract and PIR formats introduced here.
8. **Every wave in this program left a receiver contract, an idempotency guard, and a polarity test** — future mechanics should inherit all three as the house style.

---

# Appendix BC — Authorization boundary and recommended first execution package

**Authorization boundary.** This document is a planning artifact. It changes no production code, claims no paths, and edits no ledger. Implementation requires (a) explicit foreman/user authorization, (b) exact claims in `WORKTREE_OWNERSHIP.md`, and (c) the PIR record for the wave. Wave 1 is the recommended first package because it is the clearest instance of the program's defect class, has the smallest blast radius (one thin seam + authored rows), and needs no decision.

**Recommended first execution package:** `CORE-MECH-W1-FOODBORNE-DISEASE-BRIDGE`, executed as **Wave 1** of the program, in the order: P0 premise/claim → P1 authored rows → P2 seam → P3 surface/journal → P4 focused verification → handoff.

**What “full integration” means for W1 (the yardstick for the program):** authored rows pass the integrity gate; the real eat path raises exposure through the disease authority exactly once; immunity and fail-closed paths proven by tests; counterplay (discard/cold/cure) visible; journal truthful; no new save section; port contract classified; handoff attached; claims released.

---

---

# Appendix BD — Scenario design sheets (the play-testable shape of each mechanic)

Each sheet describes a concrete 10-minute play scenario a designer or QA can run, with the expected observable outcomes. They are the manual-script formalization (Appendix E) and the W12 measurement fixtures in human form.

**BD.1 W1 — “The Cold Chain” (Days 40–46).** Setup: preservation stock at 12°C (shelf-life ×0.6), 20% spoiled share on cured meat. Day 42: a cook proposes serving it; serving raises an exposure attempt (banded by the catalog row) and a ward case may follow; discarding first prevents both. Observable: disease census, ward queue, journal facts `food.spoilage_exposure` / `food.discarded`. Counterplay verified: move to 4°C storage, the share grows slower over the following days.

**BD.2 W2 — “Window Walk” (Days 195–205).** Setup: a dosimetered survivor must walk to a waystation and back across an ash-storm window. Day 196 (inside): nominal doubles; with antirad before+after the band stays lower; without, it climbs. Outside the window the same walk is unremarkable. Observable: dose register bands, `dose.storm_window` facts, cumulative totals. Counterplay verified: shielding factor and timing change the outcome; calibration keeps readings honest.

**BD.3 W3 — “The February Filter” (Days 175–365).** Setup: one water consumer and one power consumer un-hardened. Days 180–360: draws step up; hardening bought in September reduces the effective draw. Observable: band chips on both panels, `power.winter_pressure` / `water.winter_pressure` facts on band change. Counterplay verified: the offset is visible and behaves as authored.

**BD.4 W4 — “The Long Tooth” (expedition day 3).** Setup: a vehicle with worn track gear crosses rough terrain; risk is high. Day 3: breakdown; the band resolves to injury or exposure; the ward/dose owners absorb it exactly once. Observable: consequence history on the expedition surface, medical/dose deltas. Counterplay verified: repairing track gear before departure lowers the band measurably.

**BD.5 W5 — “The Wall Holds” (raid night).** Setup: perimeter at 0.6 projection vs 0.2. The same raid resolves to moderate vs high pressure. Observable: `defense.siege_effect` fact, pressure readout. Counterplay verified: fortification changes the outcome; missing defense owner leaves the raid baseline.

**BD.6 W6 — “The Herd Crossing” (travel day 2).** Setup: a migration corridor intersects the route; encounter mix shifts within bounds; the floor distribution holds on other routes. Observable: `travel.migration_contact`, shifted encounter table in the travel log.

**BD.7 W7 — “The Thread Returns” (day 60).** Setup: a quest fails on day 40; on day 58 a discovery sets its prerequisite flag. Day 60 open: the quest reopens once with a visible reason. Observable: `quest.reopened`, quest surface state. Counterplay verified: the player can pursue the discovery that revives the thread.

**BD.8 W8 — “The Rumor Travels” (days 10–20).** Setup: a choice seeds gossip; a near channel hears it by day 12; a far channel by day 18; a suppression control stops the far arrival. Observable: `gossip.propagated` facts per channel, rumor owner state.

**BD.9 W9 — “The Drift” (day 25).** Setup: a belief movement fires; faction trust moves by the authored bounded delta; the surface names the belief. Observable: `faction.belief_stance_shift`, faction surface.

**BD.10 W10 — “The Names” (day 300).** Setup: a memorial rite is performed; the Reckoning evidence list and the standing record show it exactly once. Observable: `reckoning.rite_enrolled`, evidence totals.

**BD.11 W11 — “The Once” (days 5–12).** Setup: arm a trigger for day 10 gated on a flag set on day 8; it fires once on day 10; save/load between days 8 and 10 does not change the outcome; re-arm is explicit.

**BD.12 W12 — “The Baseline” (harness).** Paired seeded 30-day runs pre/post W1–W4; deltas for illness incidence, dose bands, winter draws, breakdown consequences.

---

# Appendix BE — Tuning methodology (how numbers are chosen, not invented)

1. **Anchors before values.** Every number is anchored to an existing baseline document or a live 30-day report (the authority's DR-03 baselines), never to intuition alone. The handoff cites the anchor.
2. **Direction first, magnitude second.** The wave's test asserts direction (more exposure inside the window, more draw in winter) before a specific magnitude; magnitude is then tuned against W12's measurement.
3. **Bands, not spikes.** New pressure is expressed as bounded multipliers or per-mille probabilities so a bad day cannot end a campaign by itself (AN.3).
4. **Counterplay budget.** For every new pressure, the counterplay must be able to absorb a meaningful share of it (the AN.1 principle, quantified in the W12 report: “with hardening, winter net pressure is X% of unhardened”).
5. **No dominance.** A mechanic must not make an existing strategy strictly dominant: W6's bias is bounded, W8's propagation is suppressible, W2's conditioning leaves the antirad skill relevant.
6. **Change through packages.** Tuning values change in signed, separate proposals (AN.4), each with a before/after measurement.
7. **Determinism-protected tuning.** A tuning change may not alter a seeded draw's *stream*, only its inputs or bounds; two-pass tests must stay green.

---

# Appendix BF — Evidence standards (what counts as proof, per claim type)

| Claim type | Required proof | Not accepted |
|---|---|---|
| API name | live `rg`/signature read at the cited path | memory, authority prose, a sibling plan's table |
| Gap (“no consumer”) | zero-match `rg` over `src` and a positive probe of the intended consumer | “probably unwired” |
| Premise sealed | ledger/debt row showing RETIRED/SEALED plus the live call site | a dated authority note alone |
| Save impact | registry row count before/after and the save gate result | “no new state expected” |
| Determinism | two-pass replay with identical fingerprints | a single run |
| Exactly-once | a save/load-mid-event test | reasoning about the guard |
| Player-operability | a surface classification change or a bound command list | a probe pass |
| Mechanic-felt | the scenario sheet's observable outcome in a manual run | a code diff |
| Balance claim | a W12 (or prior) baseline citation | intuition |
| Completion | handoff + claims released + ledger row (integrator) | a green test alone |

The discipline exists because the authority's own audit found stale premises (this plan's CM-DR rows are three live examples) and because the repository's completion chain treats CLI PASS and INTEGRATED as different claims from player-operability.

---

# Appendix BG — Ten-year view (mechanic depth as campaign structure)

The program is a decade-long simulation, not a week-long sprint; the mechanics it seals are the ones that decide what kind of campaign the player plays.

1. **Resource realism becomes choice.** Spoilage (W1), exposure (W2), winter (W3), breakdowns (W4) turn the economy from an inventory tally into a management discipline with a memory.
2. **The wasteland becomes directional.** Migration (W6) and defense (W5) give geography and fortification stakes; the map is read, not traversed.
3. **The campaign becomes recoverable.** Reopen (W7) and the one-shot primitive (W11) make long campaigns forgiving of player exploration without forgiving player negligence.
4. **The social world becomes causal.** Gossip (W8) and belief (W9) make interiority and information interact with politics.
5. **The ending becomes earned.** Rites (W10) make the Reckoning read as a record of who the player was.

None of these requires a new genre; each is a connection between systems the repository already owns. That is the program's thesis: the deepest remaining gameplay is not new systems, it is *old systems finally talking to each other*.

---

---

# Appendix BH — Instrumentation and telemetry taxonomy

**BH.1 Fact schema.** Every journal fact emitted by this program uses the common shape: `factId` (stable, dotted, `domain.verb`), `sourceSystem`, `subjectId` (survivor/room/sector/faction/route), `day`, `outcome` enum (`applied`/`blocked`/`skipped`/`failed`), and a small typed payload (the numbers the surface shows). Facts are facts, not prose; surfaces and telemetry render them.

**BH.2 Program fact registry.**
| Fact id | Emitted by | Payload | Rendered by |
|---|---|---|---|
| `food.spoilage_exposure` | W1 | foodClass, spoiledShare, blockedReason | food surface + medical |
| `food.discarded` | W1 | itemId, count | food surface |
| `dose.storm_window` | W2 | day, multiplier, resultingBand | dose register |
| `power.winter_pressure` | W3 | day, band, owner | power surface |
| `water.winter_pressure` | W3 | day, band, owner | water surface |
| `expedition.breakdown_consequence` | W4 | eventId, band, receiver | expedition surface + ward |
| `defense.siege_effect` | W5 | projection, pressureBefore/After | defense surface |
| `travel.migration_contact` | W6 | sector, pressure, shiftedKinds | travel surface |
| `quest.reopened` | W7 | questId, reason, day | quest surface |
| `gossip.propagated` | W8 | seedId, channel, day | radio/journal surfaces |
| `faction.belief_stance_shift` | W9 | movementId, faction, delta | faction surface |
| `reckoning.rite_enrolled` | W10 | riteId, day | standing record |

**BH.3 Day-event discipline.** A fact is a journal entry; a *day event* is a campaign-wide heartbeat registered in `DayEventVocabulary.cs` and the semantic parity matrix. Most program facts are journal-only. The two candidate day events (winter band change, breakdown consequence) require the vocabulary + parity edits in the same change, or the parity gate fails — the implementer must not hand-edit the matrix.

**BH.4 Telemetry use.** The facts above are sufficient to compute, post-release: illness incidence per spoiled-meal share, dose bands inside/outside windows, winter draw ratio, breakdown consequence mix, raid pressure by defense projection, encounter mix by corridor, reopen rate, rumor arrival times, stance deltas, evidence totals. This is how W12's later cycles measure without new instrumentation.

**BH.5 Privacy/tone.** Facts carry ids and numbers, not editorial text; surfaces own the voice. No real-world references; restrained register.

---

# Appendix BI — Prioritization math (why these twelve waves, in this order)

**BI.1 Scoring.** Each candidate scored 1–5 on four axes: **player consequence** (does an ordinary session feel it?), **evidence strength** (is the gap proven?), **blast radius** (5 = small and safe), **counterplay depth** (can the player answer it?). `priority = consequence + evidence + blast + counterplay`, with any decision-gated candidate forced to zero regardless of score.

| Wave | Consequence | Evidence | Blast | Counterplay | Total | Gate |
|---|---:|---:|---:|---:|---:|---|
| W1 | 5 | 5 | 4 | 5 | 19 | none |
| W2 | 5 | 5 | 4 | 5 | 19 | none |
| W3 | 4 | 5 | 3 | 5 | 17 | DP-CM-1 (default given) |
| W4 | 5 | 4 | 3 | 5 | 17 | Q-CM-4 (stop-slice) |
| W5 | 4 | 4 | 3 | 4 | 15 | DP-CM-2 |
| W6 | 3 | 4 | 4 | 4 | 15 | none |
| W7 | 3 | 4 | 4 | 3 | 14 | none |
| W8 | 4 | 4 | 3 | 4 | 15 | DP-CM-3 |
| W9 | 3 | 4 | 4 | 3 | 14 | none |
| W10 | 3 | 3 | 4 | 3 | 13 | vocabulary gate |
| W11 | 2 | 4 | 5 | — | 11 | none (enables W8/W10) |
| W12 | 3 | 5 | 5 | — | 13 | runs last by role |

Reading: W1 and W2 tie for first (both 19) and are unblocked — W1 goes first because its blast radius is slightly smaller and it defines the exposure seam W4 reuses. W11 scores low on consequence (it is invisible infrastructure) and is sequenced where its adopters need it.

**BI.2 Effort bands (single agent, integrator-days).** W1 2–3 · W2 2–3 · W3 3–4 · W4 3–4 · W5 2–3 · W6 2–3 · W7 2–3 · W8 3–5 (propagation semantics) · W9 2 · W10 1–2 (+vocabulary) · W11 2 · W12 1–2. Total ≈ 25–35 integrator-days plus review, which is why the hub-mutex and sequencing rules (Appendix Y) exist.

**BI.3 Blast-radius notes.** W1 touches the shared eat path (highest visibility, smallest diff). W3 touches two mature systems (draw math is sensitive). W8 has the only per-tick cost. W10 touches the endgame (permutation-sensitive). W11 is additive by construction.

---

# Appendix BJ — Reusable templates (copy per wave)

**BJ.1 Claim.**
```
claim-core-mech-wN-<slug>-YYYY-MM-DD
Owner: <integrator>
Paths: <exact files from the wave impact map>
Receivers: <owning systems this wave writes into>
Non-goals: <wave section>
Acceptance: <Appendix T row>
PIR: <where the record lives>
Status: ACTIVE
```

**BJ.2 Handoff.**
```
Outcome: <one paragraph>
Files touched: <paths>
Files intentionally untouched: <paths>
Contract: <what the wave now guarantees>
Receivers used: <owners, and the receiver-contract clause honored>
Verification: <commands + results>
Balance: <anchor + direction asserted; magnitude pending W12>
Limitations: <P0-VERIFY items that remained>
WORKTREE: <released / transferred>
Factory: seeds consumed / drift rows re-verified / backlog delta
Next agent: <who and what>
```

**BJ.3 Decision request.**
```
DP-CM-n — <title>
Question: <one sentence>
Evidence: <probes, files, counts>
Options: <A / B with consequences>
Recommendation: <one>
Default if silent: <one>
Cost of being wrong: <low/med/high>
```

**BJ.4 Premise failure.**
```
Premise failure — wave <n>, row <probe>
Claim: <what the plan asserted>
Live finding: <what the probe showed>
Disposition: discard / re-scope / defer (per factory: never patch silently)
Plan edit: <revision + row added to CM-DR>
```

**BJ.5 Evidence scrapbook row.**
```
Wave / slice: <n>.<k>
Command: <exact>
Result: <pass/fail + counts>
Artifacts: <paths>
Claimed by: <agent>  At: <timestamp>
```

**BJ.6 Wave charter (multi-wave batch).**
```
Charter: <program> batch <n>
Waves: <ids in order>
Lane: B (mechanics) · Clusters: <ids>
Hard dependency: <waves that must land first>
Hub contention plan: <which wave holds which hub, when>
Shared-hub mutex: <owner>
Decision packets in flight: <DP ids>
Exit: all waves closed + W12 (or a named terminal wave) published
```

---

# Appendix BK — Interface contracts between waves (provider → consumer)

**BK.1 W2 → W3 (season provider).** Provider: the shared day→window/band view. Consumer contract: reads the band; never re-derives it; never caches it; applies it at exactly one site per consumer. If W3 needs a different shape (e.g., power wants a pressure ratio while water wants a band), the provider exposes both and the semantic difference is named — not re-implemented.

**BK.2 W1 → W4 (exposure seam shape).** Provider: the spoiled/consumed → `TryExpose` translation shape, including how the context is built and how outcomes are journaled. Consumer contract: W4 reuses the shape for contamination with a *different* probability source (a breakdown band, not a spoiled share); it does not copy the translation arithmetic and does not treat breakdown contamination as a foodborne event in the catalog.

**BK.3 W11 → W8/W10 (one-shot primitive).** Provider: `Arm/TryFire/ReArm` over the flag family. Consumer contract: adopters use the primitive for their one-shot guards and remove their ad-hoc equivalents; they do not extend the primitive for wave-specific needs (a wave-specific need is a new primitive method only with a program-level decision).

**BK.4 W1/W2 → W12 (measurement).** Provider: the fact registry (BH) and the wave's direction tests. Consumer contract: W12 measures through facts and harnesses, never by instrumenting Core differently in each wave.

**BK.5 All waves → port contract.** Provider: each wave's classified seams. Consumer contract: the port contract stays consistent; a wave that would reclassify another wave's seam is a PIR-4 escalation.

**BK.6 W4 → W5 (shared expedition/raid vocabulary).** They share *narrative* vocabulary (failure, repair) but no state; the cross-plan map (AY) keeps them disjoint. If a future wave wants a shared "asset condition" concept across vehicle and perimeter, that is a new program with a DEC, not an extension here.

---

---

# Appendix BL — Program integration protocol (how the integrator runs all twelve waves)

**BL.1 Cadence.** One wave per claim. A wave opens with P0 (premise re-sweep + claim), runs its slices (AE), and closes with verification, handoff, and claim release. The next claim opens only after the previous handoff is accepted. W1 is Wave 1 of the program and of full integration.

**BL.2 Batch planning.** A batch is 2–3 waves that do not share hubs. Example batch 1: W2 + W9 (dose + belief; no shared files). Batch 2: W3 + W7 (winter + quest reopen). Batch 3: W5 + W6 (siege + migration). W4 follows W1+W2 (it consumes both seams). W11 precedes W8/W10 adoption. W12 closes.

**BL.3 Review.** Each wave is reviewed against the AS checklist (code), the BU tier checklist (seal), and the BF evidence standards (proof). A review that cannot cite evidence for a claim sends the wave back to its slice, not to redesign.

**BL.4 Ledger discipline.** The integrator (never the plan, never a builder) adds one ledger row per closed wave using the Q mapping, with the evidence classes from AM.2. The decision register receives DP-CM resolutions and DECs for vocabulary/scope changes. The plan document itself is revised only to record drift (AJ) or closeout addenda.

**BL.5 Program close.** The program closes when W12 publishes deltas and any retune targets are filed. The closeout records: waves closed, facts introduced, drift rows, backlog delta, and the next generation's candidate list (the BB knowledge-capture list is the seed).

**BL.6 Program DoD restatement (authoritative).**
1. W1–W4 MECHANIC-tier sealed with focused evidence, manual scenarios (BD), and truthful facts (BH).
2. W5–W10 sealed at their declared tiers; W11 adopted by W8 and W10; W12 published.
3. Every wave passed PIR-1…PIR-10; every seam classified; zero unbound ports; zero new save sections.
4. Every consequence lands in a named receiver under its AA contract; no parallel authority anywhere; no `System.Random`; two-pass determinism per new consequence.
5. Every wave's handoff follows BJ.2 and the backlog delta follows BJ factory Step 7.
6. The plan's drift register (AJ) is current against live source at close.

---

# Appendix BM — Mechanical depth dossiers (the “mechanic seal” evidence per wave)

Each dossier states the underlying model, the player-facing learning, the state space, and the exact evidence that constitutes the seal. A wave is not done until its dossier's evidence exists.

**BM.1 W1 — Foodborne.** *Model:* spoilage share (preservation) × authored probability (disease catalog) → `TryExpose` → infection/obstruction, ward treatment. *Learning:* the cold chain is a process, not a stat. *State space:* per-item spoiled share; per-survivor infection; per-disease immunity. *Seal evidence:* (a) clean vs spoiled exposure tests; (b) discard/cold counterplay tests; (c) exactly-once + save/load-mid-event; (d) scenario BD.1 observed; (e) facts in BH.1 present and truthful.

**BM.2 W2 — Storm-window dose.** *Model:* day → window multiplier → nominal mSv → `BookReading` (owned seeded roll + bands) → cumulative. *Learning:* exposure is calendrical; antirad and shielding are skills. *State space:* cumulative dose, band, dosimeter state; window is derived. *Seal evidence:* direction tests (window vs not), counterplay tests, neutral-fallback test, two-pass determinism, scenario BD.2.

**BM.3 W3 — Winter pressure.** *Model:* day → band → per-consumer draw scaling; hardening offsets in owner vocabulary. *Learning:* preparation is rewarded; the spine has teeth. *State space:* owner draw state; band derived. *Seal evidence:* boundary-day tests, one-multiplication guard, offset tests, scenario BD.3.

**BM.4 W4 — Breakdown consequences.** *Model:* breakdown event → seeded band (none/injury/exposure/contamination) → receivers. *Learning:* maintenance is an investment with a failure mode. *State space:* vehicle condition/gear; receiver states own consequences. *Seal evidence:* band routing tests, repair-effect test, event-id + mid-event save test, scenario BD.4.

**BM.5 W5 — Defense payoff.** *Model:* defense projection → raid pressure scaling at the resolver. *Learning:* walls matter. *State space:* defense owner state; projection derived. *Seal evidence:* pressure-difference test, neutral-fallback test, one-consumption guard, scenario BD.5.

**BM.6 W6 — Migration texture.** *Model:* sector pressure → bounded bias over authored encounters. *Learning:* the map moves; scouting pays. *State space:* migration pack state; encounter tables authored. *Seal evidence:* baseline-distribution test, bound test, determinism, scenario BD.6.

**BM.7 W7 — Reopen grammar.** *Model:* record predicate (status + flags + min_day) evaluated on change/day-open → one-shot transition. *Learning:* campaigns are recoverable. *State space:* quest records + flag namespace. *Seal evidence:* reopen-once, no-loop, save/load, scenario BD.7.

**BM.8 W8 — Gossip propagation.** *Model:* seed → time/distance gating per channel → info-owner emission, one-shot per window (W11). *Learning:* choices have reach; channels are the vulnerability. *State space:* info owner state; seeds authored. *Seal evidence:* arrival-time tests, suppression test, one-emission test, determinism, scenario BD.8.

**BM.9 W9 — Belief politics.** *Model:* belief movement → bounded `ModifyTrust` → stance. *Learning:* interiority is political. *State space:* engine trust only. *Seal evidence:* stance-shift test, clamp test, no-cache assertion, scenario BD.9.

**BM.10 W10 — Rites memory.** *Model:* rite performance → one `EnrollEvidence` → standing record. *Learning:* grief is recorded. *State space:* Reckoning evidence; ritual records. *Seal evidence:* enroll-once, totals, save/load, scenario BD.10, vocabulary gate recorded.

**BM.11 W11 — One-shot primitive.** *Model:* armed/triggered state over the flag family; pure fire condition. *Learning:* (meta) mechanics get one-shot semantics free. *State space:* flag counters. *Seal evidence:* fire-once, threshold, gate, persistence, re-arm, double-call test, adoption by W8 and W10.

**BM.12 W12 — Measured balance.** *Model:* paired seeded runs pre/post → deltas → filed targets. *Learning:* (meta) claims are measured. *State space:* report. *Seal evidence:* published deltas with pinned seeds and commits; retunes filed separately; no product-code diff.

---

---

# Appendix BN — Cluster-by-cluster mechanics map (authority DM-1…DM-17 applied to this program)

The authority distills seventeen subsystem clusters (DM-1…DM-17). This appendix states, per cluster, what is sealed today, what this program adds, and what remains open — so the program's scope is explicit against the authority's own map rather than against ad-hoc taste.

| Cluster | Sealed today (spot-checked) | This program adds | Still open (backlog/other owners) |
|---|---|---|---|
| **C1 Shelter operations** | rooms, thermal, atmosphere, sanitation, maintenance, fire, airlock, decon (host refs 6–9 per owner; PFGL master covers surfaces) | — (W3 touches a water consumer, not shelter ops) | shelter-failure cascade (B-01), grid seal follow-through (B-02) |
| **C2 Medical pipeline** | disease, dose, ward, surgery, pharma, therapies (medical ward host 5) | W1 (disease fed by food), W2 (dose conditioning), W4 (injury/exposure routing) | child-health cohort bridge (B-04), rescue-remains (B-25) |
| **C3 Water, food, agriculture** | water (host 9), greenhouse (8), kitchen (3), preservation (1, Main-only) | **W1 (preservation→disease is this cluster's flagship gap)** | dive-site/hydroponic audit (SB-12, inference pending) |
| **C4 Power and industry** | power grid (18), foundry family, industrial catalogs | W3 (one power consumer takes the winter band) | industrial difficulty binding (PFGL W7 owns) |
| **C5 Expeditions and travel** | expedition (28), vehicles, caravans, encounters | W4 (breakdown consequences), W6 (encounter bias) | scavenging parity re-audit (CM-DR-02) |
| **C6 Map and geography** | wasteland map, zones, survey | — | flooded-route topology (B-09, gated) |
| **C7 Factions and war** | stance engine, doctrines, musters, treaties | W5 (defense→siege), W9 (belief→stance) | faction-war per-strike emitter (B-10, gated), black-market funds (B-11, gated) |
| **C8 Radio and information** | radio, intercepts, rumors (sealed distress content untouched) | W8 (gossip propagation into the existing info owner) | intercept journal depth (B-13) |
| **C9 Survivors and interiority** | needs (33), trauma, therapies, relations, memory, lineage | W9 (belief input), W10 (rites input) | unstarted-type cluster (B-34) |
| **C10 Quests and moral choice** | questline master, personal quests, moral-choice corpus | W7 (reopen grammar), W8 (gossip seeds) | flag reverse-lookup premise (authority open) |
| **C11 Economy** | market, prices, rumors, black market, trade | — | rumor-band commodity extension (B-12, data) |
| **C12 Weather and Year of Ash** | weather (28), seasons, hardening, the 15-chapter spine | **W2 + W3 (the season finally reaches dose, power, water)** | mid-winter prose campaign (SB-02, Lane A) |
| **C13 Endgame and epilogue** | Reckoning, verdict, chronicle, standing records | W10 (rite evidence) | epilogue permutation coverage (SB-05, Lane A) |
| **C14 Ecology and wildlife** | ecosystem, trapping, bestiary, infestations | W6 (migration as travel pressure) | zoonosis/infestation extensions (authority proposals) |
| **C15 Defense and security** | perimeter, defense grid, sky defense, orbital telemetry | W5 (defense values reach siege) | — |
| **C16 Progression and meta** | skills, research, difficulty (CF-XP01 active) | — (deferred to PFGL W7) | EN-01…08 proposals (blocked) |
| **C17 Host surface and UI** | 219 surfaces, panel battery, a11y gates | bindable strips per wave (AL) | stale-panel sweep (B-24; PFGL/Triad B own) |

Cluster coverage summary: the program touches **9 of 17 clusters** and deliberately leaves the other 8 to their sealed owners, other plans, or gates. The deepest coverage is exactly where the authority's own seeds pointed (C2, C3, C5, C7, C9, C10, C12, C13, C14, C15).

---

# Appendix BO — Narrative framing and journal voice (tone contract per wave)

The mechanics must read as the world the player inhabits, not as telemetry. The journal is the program's main narrative surface; its voice is restrained, factual, and fictional.

**Voice rules (program-wide).** Facts name what happened, to whom, and with what consequence. No editorializing, no real-world references, no copied text, no UI-layout pastiche, no second-person coaching. The player infers the lesson; the journal states the event.

**Worked voice examples (target register, not final copy).**
- W1: “Preserved stock served at the common table — 40% spoiled share. Exposure attempt logged for Ilsa Venn (blocked: prior immunity).” / “Spoiled stock discarded: 38 units (cure batch unaffected).”
- W2: “Dose reading booked: day 214, 0.87 mSv, band W-2. Storm window ASH-STORM-2 active (×1.8).”
- W3: “Winter pressure band moved to severe. Filter draw ×1.6; hardening offset ×0.7.”
- W4: “Vehicle K-19 broke down on rough ground. Consequence: minor injury (Tomas Reil) — treated in the ward.”
- W5: “Raid pressure reduced by perimeter projection 0.62 — outcome: walked off.”
- W6: “Sector 7 migration contact: herd crossing (pressure 0.5). Encounter mix shifted.”
- W7: “Thread ‘Convoy ledger’ reopened — prerequisite discovery acquired.”
- W8: “A rumor reached Ashmark via the road channel after 6 days.”
- W9: “Trust with Ashmark +0.03 (belief movement: rite-of-ash). Stance unchanged.”
- W10: “Memorial rite enrolled: names read for the fallen. Evidence fragment recorded.”
- W11/W12: no player-facing journal (infrastructure and measurement).

**Tone boundary.** Restrained and human; the world is fictional; the tone does not moralize the player's choices (W1's spoilage is a fact, not an accusation). Where a consequence is severe, the register stays level — the game respects the player enough not to editorialize.

---

# Appendix BP — Open premise ledger (every [P0-VERIFY], with its probe)

| # | Premise | Wave | Probe the implementer runs | If it fails |
|---|---|---|---|---|
| P0-1 | the food/kitchen surface exists and is bindable | W1 | `rg -l 'Kitchen' src/UI` + manifest row lookup | minimal strip per AL.1 or defer surface (mechanic still lands) |
| P0-2 | `DiseaseExposureContext` construction shape | W1 | read `Disease/DiseaseCatalog.cs` + `DiseaseSystem` exposure helpers | build via the documented factory **[P0-VERIFY]** |
| P0-3 | the caller of `BookReading` (conditioning site) | W2 | `rg -n 'BookReading' src` | if multiple callers, condition at the single production call site and document |
| P0-4 | the breakdown occurrence site | W4 | `rg -n 'Breakdown' Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` + expedition host | hook the event stream; do not alter risk math |
| P0-5 | the medical injury intake verb | W4 | `rg -n 'Injur' Assets/Ashfall.Core/Medical src/Host/Medical*` | stop slice, file Q-CM-4 |
| P0-6 | the raid/siege resolver | W5 | `rg -ln 'Raid|Siege' Assets/Ashfall.Core` | stop, file DP-CM-2 |
| P0-7 | defense value accessors | W5 | `rg -n 'class DefenseGrid' -A40` | read-only projection from the panel's own data path |
| P0-8 | the travel-encounter selection site | W6 | `rg -n 'TravelEncounter' src/Host/ExpeditionHostSession.cs` | bias at the documented selection method |
| P0-9 | the quest owner holding failed/abandoned records | W7 | `rg -n 'Abandoned|Failed' Assets/Ashfall.Core/Quests` | predicate over the owner's record list |
| P0-10 | the rumor/info owner | W8 | `rg -n 'class RumorSystem' -A20` | emit through the owner's public entry |
| P0-11 | where belief movements are observed | W9 | `rg -n 'belief_movements' src` | hook the loader/observation point |
| P0-12 | the Reckoning evidence vocabulary | W10 | `rg -n 'Evidence' Assets/Ashfall.Core/Endgame` | stop, file DP-CM-4 |
| P0-13 | the flag-family persistence surface | W11 | `IFlagLedger` + its save | ride existing counters |
| P0-14 | the l10n seam and the string-freeze state | all | read the L10N roadmap + ledger | journal-only strings until the freeze resolves |
| P0-15 | the mod specification surface (for data-first rows) | all | Plan 165 spec fields | rows are consumed by the wave itself; mod exposure is additive |
| P0-16 | probe names per wave | all | `HostCliRegistry` sweep | re-rg the live flag before the run (DR-07) |

Open premises are the program’s honest edge: each is a one-probe task at P0, and each has a defined failure action (defer the surface, stop the slice, or file a decision). None of them blocks Wave 1 except P0-1 (surface existence), which has a documented fallback.

---

---

# Appendix BQ — Printable per-wave integration checklists

Copy one per wave claim. Every box is either checked with evidence or explicitly deferred with a reason.

**Common header.** [ ] HEAD recorded [ ] §2.2 sweep row re-run [ ] claim accepted [ ] PIR-1…PIR-10 complete [ ] receivers named (AA) [ ] save plan confirmed [ ] determinism plan named [ ] test file named [ ] surface decision made [ ] strings decision made [ ] open premises resolved or deferred with reasons.

**W1 Foodborne.** [ ] foodborne rows authored + integrity green [ ] spoiled-share read from preservation owner [ ] exposure built via `TryExpose` context [ ] one call per eat event [ ] immunity respected [ ] absent owner fails closed + fact [ ] discard/cold counterplay visible [ ] journal facts truthful [ ] focused tests alone-first green [ ] holdfast consumption regression green [ ] probe re-verified live [ ] port classification [ ] no new save section [ ] handoff + claims released.

**W2 Dose window.** [ ] provider pure (day→band) [ ] conditioning at one site [ ] band 1.0 identity outside [ ] antirad/shielding unaffected [ ] missing calendar neutral + fact [ ] band edges untouched [ ] two-pass determinism [ ] dose register shows band [ ] probe + focused tests [ ] port classification [ ] handoff.

**W3 Winter pressure.** [ ] DP-CM-1 resolved or defaulted [ ] shared provider reused (not forked) [ ] two consumers only [ ] one multiplication each [ ] boundary-day tests [ ] hardening offset tested [ ] missing calendar neutral [ ] band chips truthful [ ] probes + tests [ ] handoff.

**W4 Breakdown.** [ ] W1 and W2 landed [ ] band pure + seeded [ ] three receivers routed once [ ] event-id guard [ ] mid-event save/load test [ ] repair lowers band [ ] preflight risk visible [ ] probes + tests [ ] handoff.

**W5 Defense siege.** [ ] DP-CM-2 resolved [ ] projection read-only [ ] one resolver consumption [ ] neutral when owner absent [ ] panel projection truthful [ ] probe + test [ ] handoff.

**W6 Migration encounters.** [ ] selector identified [ ] bounded bias [ ] baseline distribution preserved [ ] Ordinal ordering [ ] travel readout truthful [ ] probe + test [ ] handoff.

**W7 Quest reopen.** [ ] owner identified [ ] predicate data-driven [ ] one-shot guard [ ] no side queue [ ] reason visible [ ] save/load test [ ] probe + tests [ ] handoff.

**W8 Gossip.** [ ] DP-CM-3 resolved [ ] propagation seeded + Ordinal [ ] info-owner emission only [ ] suppression honored [ ] W11 adopted (or interim guard documented) [ ] surfaces truthful [ ] determinism + tests [ ] handoff.

**W9 Belief stance.** [ ] translation table authored [ ] `ModifyTrust` only write [ ] bounds clamp [ ] no cache [ ] stance explanation visible [ ] probe + test [ ] handoff.

**W10 Rites.** [ ] vocabulary gate passed (or DP-CM-4 filed) [ ] enrollment once [ ] performance-id guard [ ] standing record visible [ ] save/load test [ ] probe + tests [ ] handoff.

**W11 One-shot primitive.** [ ] flag persistence seam used [ ] fire-once + threshold + gate [ ] re-arm explicit [ ] double-call test [ ] adopted by W8 and W10 [ ] port classification [ ] no new save section [ ] handoff.

**W12 Balance.** [ ] baseline commit pinned [ ] paired seeded runs [ ] deltas published [ ] retune targets filed (not applied) [ ] no product-code diff [ ] ledger close row prepared [ ] program closeout.

---

# Appendix BR — Program risk deep-dive

**BR.1 Balance drift (highest likelihood).** W1–W4 change outcomes players feel. *Mitigations:* data-authored bands; direction-first tests; W12 measurement; retunes as separate signed packages; the J.5 rule (no inline tuning). *Residual:* early players may see a spike before W12 lands — the waves' band bounds (AN.3) cap it.

**BR.2 Exactly-once violations.** *Mitigations:* the AB.3 guard pattern; mid-event save/load tests in W1/W4/W8; receiver-contract duties. *Residual:* low.

**BR.3 Parallel-authority creep.** *Mitigations:* AA receiver contracts; the 5.3 collision map; the AS review checklist's first two items; the AY cross-plan map. *Residual:* medium in the social waves (W8/W9 touch politically loaded systems) — mitigated by one-owner writes.

**BR.4 Concurrency.** *Mitigations:* PIR-2 claim before code; hub mutex; handoffs transfer shared seams rather than reverting them (AM.5 precedent). *Residual:* the repository has many agents; the plan assumes the ownership file is honored.

**BR.5 Premise staleness.** *Mitigations:* the CM-DR register (three live corrections already); the factory's discard-don't-patch rule; BP open-premise ledger with defined failure actions. *Residual:* medium — the honest posture is that a future wave may be discarded at P0, and that is a success, not a failure.

**BR.6 Scope creep into narrative/balance/UI.** *Mitigations:* lane discipline (B only); bind-before-fork; no inline tuning; surfaces are strips. *Residual:* low.

**BR.7 Save-pin pressure.** *Mitigations:* reuse-first design; PIR-4 escalation for any new section; pin re-read per wave. *Residual:* very low (all twelve waves are designed section-free).

**BR.8 Epilogue permutation risk (W10/W7).** *Mitigations:* additive-only evidence; reopened threads are evidence, never requirements (X). *Residual:* low.

**BR.9 Performance (W8).** *Mitigations:* window guard; per-tick budget asserted in test (AQ). *Residual:* low.

**BR.10 Localization/freeze.** *Mitigations:* BP P0-14 probe; journal-only strings while frozen. *Residual:* low.

---

# Appendix BS — The first hour through this program's mechanics (design check)

A new campaign's opening hour should already contain the program's lessons, without teaching them explicitly.

- **Minutes 0–10 (setup).** The player meets food storage (W1's strip) and the weather calendar (W2/W3's band chip) as *readouts* of the world, not as tutorials.
- **Minutes 10–25 (first meals).** The first spoiled-share decision is the program's first lesson: discard, cure, or gamble. No one explains it; the strip shows the number.
- **Minutes 25–40 (first exposure).** The dose register appears with today's band; the window chip is visible before the first booking.
- **Minutes 40–55 (first expedition prep).** Vehicle preflight (W4) shows breakdown risk; track gear is a visible lever.
- **Minutes 55–60 (first faction contact).** W9's stance explanation (which beliefs move trust) appears as part of the faction surface, not a new screen.

Design check: each mechanic enters as *truthful state the player already needs*, and its counterplay is the obvious next action. If a future wave's mechanic cannot be introduced this way in its first session, it is either mis-scoped or belongs to a later campaign phase — a useful editorial test for the next generation of waves.

---

---

# Appendix BT — Adversarial review questions (per wave)

Each wave's reviewer asks these before accepting the handoff. A “no” sends the wave back to its slice.

**W1.** Could this exposure be raised twice for one meal? Could it bypass immunity? Does the probability live anywhere but the catalog? What happens in a game mode with no preservation owner? Does the player have a *visible* way to avoid it? Does the journal overstate certainty (“infected” vs “exposure attempted”)?
**W2.** Is the multiplier applied anywhere other than the one site? Does a missing calendar quietly become a discount? Are band edges untouched? Can a player act on the band? Is the reading's source named?
**W3.** Is the band applied twice anywhere? Is the counterplay real or cosmetic? Do boundary days behave? If the provider drifts, do both consumers drift together (one authority) or independently (two truths)?
**W4.** Can a consequence double-apply on retick or save/load? Is each band routed to the *right* receiver? Does repair measurably help? Is a breakdown still survivable (recovery exists)?
**W5.** Does the projection read after resolution (too late)? Does any second consumer exist? Is defense rebalanced by accident? Does the player see why a raid turned?
**W6.** Can a bias erase an authored encounter? Does zero pressure reproduce the baseline exactly? Is ordering deterministic? Can the player scout/counter?
**W7.** Can a quest reopen twice? Is the predicate data-driven (content-safe)? Does reopening bypass the quest state machine? Is a completed quest ever reopened?
**W8.** Can a rumor emit twice in a window? Is suppression honored? Is the placement decision recorded? Does the rumor economy stay bounded? Is the existing info owner the only writer?
**W9.** Is trust written anywhere but the engine? Can daily deltas swing a stance without intent? Is the player's belief-to-politics link legible?
**W10.** Was the vocabulary gate actually run? Can a rite enroll twice? Is evidence additive (epilogue-safe)? Does an unknown class fail visibly?
**W11.** Does the primitive own persistence? Can a trigger fire twice in one tick? Is re-arm explicit? Did adopters remove their ad-hoc guards or keep both?
**W12.** Is the baseline commit pinned? Are the seeds identical across runs? Did any tuning slip into the diff? Are the retune targets filed rather than applied?

---

# Appendix BU — Appendix index (navigation)

| Appendix | Content | Read when |
|---|---|---|
| A | PIR-1…PIR-10 pre-integration gate | every wave, before code |
| B | seal tiers + per-wave tier table | every wave, at DoD |
| C | backlog, decision packets, backlog delta | foreman; at program close |
| D | live verification log | P0; when re-verifying |
| E | manual player scripts | manual verification |
| F | focused test matrix | P0/P4 |
| G | wave-by-wave phase summary | planning the batch |
| H | program risk register | review |
| I | maintenance and revision rules | every revision |
| J | deep runbooks W1–W12 | implementing a wave |
| K | per-seed traceability (factory seeds) | factory conversations |
| L | worked examples | balance review; reviewers |
| M | data-first authoring contracts | data slices |
| N | surface/a11y/tone contracts | UI slices |
| O | test doctrine | all verification |
| P | coordination and handoff | handoff time |
| Q | program closeout and ledger mapping | closeout |
| R | glossary | any time |
| S | per-wave forensic evidence cards | P0 |
| T | acceptance command matrices | P4 |
| U | expanded failure catalogs | review; debugging |
| V | cross-wave wiring maps | planning cross-wave effects |
| W | player-value deep dives | design review |
| X | epilogue-position statements | endgame-sensitive waves |
| Y | sequencing and critical path | batch planning |
| Z | decision packets (expanded) | foreman |
| AA | receiver contracts | P0; review |
| AB | determinism deep dive | determinism slices |
| AC | save/restore deep dive | save-touching waves |
| AD | observability matrix | instrumenting waves |
| AE | day-slice narratives | executing |
| AF | pilot campaign scripts | smoke |
| AG | architecture decision sketches | review; ADRs |
| AH | unclaimed-content census feed | P0 |
| AI | foreman question pack | foreman |
| AJ | local drift register | every revision |
| AK | content authoring guide | data slices |
| AL | surface wireframes | UI slices |
| AM | historical integration precedent | onboarding |
| AN | balance philosophy | design review |
| AO | accessibility and input | UI slices |
| AP | localization readiness | string-bearing waves |
| AQ | performance budget | perf-sensitive waves |
| AR | mod-support | data-first waves |
| AS | code review checklist | review |
| AT | revision ledger | revisions |
| AU | core mechanics inventory survey | scope checks; future plans |
| AV | per-wave test case catalogs | P0/P4 |
| AW | wiring sketches | implementing |
| AX | readiness scoreboard | claim decisions |
| AY | cross-plan conflict map | coordination |
| AZ | precision pass record | precision passes |
| BA | rollback drills | P0; incidents |
| BB | knowledge capture | program close |
| BC | authorization boundary and first package | foreman |
| BD | scenario design sheets | manual verification; W12 |
| BE | tuning methodology | balance slices |
| BF | evidence standards | review |
| BG | ten-year view | strategy |
| BH | instrumentation and telemetry | instrumenting waves |
| BI | prioritization math | planning |
| BJ | reusable templates | every wave |
| BK | interface contracts between waves | cross-wave work |
| BL | program integration protocol | running the program |
| BM | mechanical depth dossiers | seal evidence |
| BN | cluster-by-cluster mechanics map | scope; future plans |
| BO | narrative framing and journal voice | instrumenting waves |
| BP | open premise ledger | P0 |
| BQ | printable per-wave checklists | every wave |
| BR | program risk deep-dive | review |
| BS | first-hour design check | design review |
| BT | adversarial review questions | review |
| BU | this index | — |
| BV | Wave 1 execution memo | executing Wave 1 now |

---

# Appendix BV — Wave 1 execution memo (full integration, this authorization)

**Package:** `CORE-MECH-W1-FOODBORNE-DISEASE-BRIDGE` · **Wave 1 of the program** · **Tier:** MECHANIC · **Gate:** none

**Why this wave first.** It is the clearest instance of the program's defect class in the most player-legible system; its blast radius is one thin seam; it needs no decision; it defines the exposure-translation shape that Wave 4 reuses.

**Pre-flight (PIR record).**
| Gate | Status |
|---|---|
| PIR-1 Evidence freshness | HEAD `1678c074` recorded; sweep rows re-probed this revision (§2.2) |
| PIR-2 Ownership | `WORKTREE_OWNERSHIP.md` rows all DONE/HANDED_OFF; claim opened for W1's exact paths |
| PIR-3 API copy | live signatures pasted: `ConsumeFood`, `GetSpoiledFood`, `DiscardSpoiled`, `ConsumeFoodResult`, `TryExpose`, `TryInfect` (§7.1) |
| PIR-4 One-owner pick | Disease is the sole receiver; preservation stays ignorant; seam is host-thin (AG.1) |
| PIR-5 Save plan | reuse-only; no new section; exactly-once per eat event |
| PIR-6 Determinism | infection roll owned by the disease authority's existing stream; no new rng |
| PIR-7 Test plan | `FoodborneExposureBridgeTests` (new, alone first) + holdfast consumption regression |
| PIR-8 UI plan | bind the existing food/kitchen surface if it exists (BP P0-1); strip per AL.1; else mechanic-only with named deferral |
| PIR-9 Baseline green | pre-change build + existing preservation/disease suites green before edits |
| PIR-10 Rollback slice | one contiguous seam edit; catalog rows may stay (inert if unused) |

**Execution slices (AE.1).** S1 premise/claim → S2 authored foodborne rows + integrity gate → S3 exposure seam at the real eat path → S4 counterplay readout + journal facts → S5 focused verification + manual scenario BD.1 + handoff.

**Definition of full integration for W1 (BC).** Authored rows integrity-green · the real eat path raises one exposure attempt per eat with a catalog-scaled probability · immunity and fail-closed paths proven · discard/cold/cure counterplay visible · truthful journal · no new save section · port classification updated · handoff attached · claims released.

**If a premise fails at P0** (e.g., the food surface does not exist): the mechanic still lands; the surface is deferred with a named reason (the AM.4 honesty precedent). If the disease catalog cannot express a foodborne class without an additive vocabulary amendment, stop and file a decision (BP P0-2 path).

---

---

# Appendix BW — The next generation (what this program leaves behind, ready to run)

The factory grows by sessions, not by size. This appendix states what the next generation of core-mechanics work should be, in the same formats, so the next session starts from verified premises instead of from scratch.

**BW.1 The unstarted-type cluster (B-34) promoted to concrete candidate seeds.** The authority recorded seven debt items whose types do not exist and correctly refused to plan them as a block. Each is a candidate here with its premise probe and its honest difficulty:

| Candidate | Type that would be created | Premise probe | Difficulty | Tier if built |
|---|---|---|---|---|
| **Campaign age clock** | `CampaignAgeClock` (day → age band, canon Day-90/Day-190/Day-3650 anchors) | `rg -n 'CampaignAge\|AgeClock' Assets src` (expect zero) | low — pure calendar view, no state | GAP; feeds W2/W3 bands and epilogue |
| **Schedule-hour consumer** | consumer of `ShelterScheduleSystem` hours into duty/activity owners | `rg -n 'ScheduleHour'`; check what consumes schedule hours today | medium — must route to existing owners, not re-derive | FEATURE; pairs with PFGL W9 routines |
| **Crisis producer** | a producer feeding the existing crisis HUD | `rg -n 'CrisisCoordinator\|Crisis'` | medium — the HUD exists without a producer; the authority's finding | MECHANIC; high player value |
| **Last-interaction stamp** | relation field: when two survivors last interacted | `rg -n 'LastInteraction'`; check `SurvivorRelationsSystem` | low-medium — a stamped field with decay semantics | FEATURE; feeds W8/W9 social waves |
| **Creation→vault** | creation authority feeding the archive (the vault exists as archive only) | `rg -n 'Creation' Assets/Ashfall.Core` | medium — the archive is sealed; the creation side is the gap | FEATURE; endgame flavor |
| **Medical record / health-history** | the authority recorded no such type (PFGL's HealthHistory finding suggests drift — **re-verify first**) | `rg -n 'HealthHistory\|MedicalRecord'` | unknown — verify before planning | unknown |

The medical-record row is a textbook factory lesson: a later plan (PFGL) found `HealthHistorySystem` live, which would make the authority's “no type exists” claim stale. The rule is unchanged: probe, then plan.

**BW.2 Gaps this program deliberately leaves (with their next-session shape).**
1. **Shelter-failure cascade (B-01).** Needs the quarantine's own exit criteria read in-session; the next session should open with that document, not with a plan.
2. **Grid seal follow-through (B-02).** Same: read the seal log, find the open consumer rows, plan only what is open.
3. **Rumor-band commodity extension (B-12).** A Lane C data wave with a utilization gate, not a mechanics wave.
4. **Child-health cohort bridge (B-04).** Real, but it belongs beside the sealed medical flagship set; propose it to the foreman as a satellite with the medical owner’s consent.
5. **Difficulty consumer binding.** Owned by PFGL W7 / `CF-XP01`; the next session should *check their completion*, not duplicate them.
6. **Encounter-content gaps in migration corridors.** If corridors feel thin after W6, author encounters through `travel_encounters.json` — a Lane A/B content pass with the integrity gate, not a code change.

**BW.3 The standing premise protocol for the next session (copy-paste).**
```
1. git rev-parse HEAD; re-read INTEGRATION_PLANS.md, WORKTREE_OWNERSHIP.md, KNOWN_DEBT.md, TEST_POLICY.md
2. Re-run this program's §2.2 sweep for the candidate row
3. Probe for the type/API with rg; zero matches = candidate is unstarted (plan it); matches = read them and re-scope
4. Name the receiving owner and check its receiver contract (AA format) if the candidate is a seam
5. Score BI-style; write the wave contract; run PIR at claim time
6. Publish the backlog delta and the drift row, whatever the outcome
```

**BW.4 What must not be repeated.** The three live authority corrections (CM-DR-01/02/04) are the template: when a seed's premise is sealed or stale, discard it visibly and record the correction. A discarded candidate is a successful session outcome; a patched stale premise is a defect.

**BW.5 The ten-year promise, restated.** Each generation should make the *connections* richer rather than the systems larger. The authority gave the repository 57 volumes of plans; this program adds the first twelve connections in the core loop. The next generation's best candidates are the seams this program will leave behind — not new islands.

---

---

# Appendix BX — Surface and DTO contracts (per wave)

A surface contract names the panel, the bind target, the DTO the surface reads, the refresh cadence, and the snapshot/a11y requirements. Panels stay facades: no gameplay authority, no offline math (DM-17 constraint).

| Wave | Panel (bind or new) | DTO source | Refresh | Snapshot/a11y |
|---|---|---|---|---|
| W1 | bind food/kitchen surface **[P0-1]**; minimal strip if none | `FoodPreservationSystem` reads + census-style snapshot | on census change + on eat/discard | snapshot coverage; spoiled count as text, not color |
| W2 | dose register surface (exists) | dose entry + window band | on booking | band label text; window named |
| W3 | chosen power + water panels (exist) | owner reads + band chip | on band change and on draw | chip text + value; offset stated |
| W4 | expedition surface (exists) | vehicle risk + consequence history | on event + on repair | risk as text; consequence as text |
| W5 | defense panel (exists) | projection + next-raid expectation | on defense change | components listed; no hidden math |
| W6 | travel surface (exists) | corridor state | on sector entry | pressure as text |
| W7 | quest surface (exists) | reopen eligibility + reason | on day open and flag change | reason text; no silent reopen |
| W8 | radio/journal surfaces (exist; additive) | rumor arrival facts | on propagation | arrival as text; receiver voice |
| W9 | faction surface (exists) | trust + belief explanation | on belief movement | belief named; “stance unchanged” explicit |
| W10 | standing record / memorial surface (exists) | evidence list | on enrollment | names where supported |
| W11 | none (infrastructure) | — | — | — |
| W12 | none | — | — | — |

DTO rules: DTOs are read models built by the host/Main layer; they never re-derive Core math; they carry the owning system's census plus the wave's fact; they are rebuilt on session reset and slot switch (lifecycle contract).

---

# Appendix BY — Second-order interaction atlas (what the program sets up next)

The program is not only twelve fixes; it creates new couplings that the next generation can deepen without new authorities.

| After the program… | …these couplings exist | The next generation can add |
|---|---|---|
| W1 + W2 | foodborne illness and dose share the medical/disease owners | injury × immunity interactions; ward triage prioritization (clinical ward owner is live) |
| W1 + W3 | winter food spoilage pressure × winter resource pressure | cold-chain logistics as a winter planning problem (shelter ops owners) |
| W2 + W3 | one season authority drives exposure and utilities | a single “seasonal briefing” read model consumed by the daily briefing surface (C17 opening) |
| W4 + W6 | breakdowns on migration-heavy routes | expedition planning that reads corridor pressure and vehicle risk together (expedition surface) |
| W5 + W9 | defense posture × faction belief | doctrine-conditioned raid pressure (the authority's siege-math seed in fuller form) |
| W7 + W11 | reopen grammar with one-shot semantics | discovery events as first-class one-shot triggers (the lore authority's capability gap, B-35's second consumer) |
| W8 + W9 | gossip carries belief shifts; beliefs move standing | a closed loop the player can observe and exploit (suppress the rumor, or amplify it) |
| W10 + W7 | rites for the dead + reopened threads | epilogue evidence that distinguishes “mourned and lost” from “mourned and lost twice” (permutation-safe, additive) |
| W1–W4 + W12 | every core mechanic is measurable | a standing seasonal-health report (`docs/balance/`) as a recurring artifact |

The atlas is the program’s compounding return: each wave’s mechanism becomes a *surface* for the next mechanic, and none of the compounding requires a new authority — only a new read model and a new consumer of existing ones.

---

---

# Appendix BZ — Draft integration-ledger rows (foreman/integrator to paste on close)

These are drafts, not ledger edits; this plan claims no ledger authority. Each row follows the repository's claim-row shape (AM.1) and is filled with the evidence classes of BF.

**W1:** `claim-core-mech-w1-foodborne-2026-XX` | `CORE-MECH-W1-FOODBORNE-DISEASE-BRIDGE` | Core: `disease_catalog` exposure rows; Host: eat-path exposure seam; Tests: `FoodborneExposureBridgeTests` + preservation/disease regression; Evidence: probe, focused counts, holdfast consumption regression, scenario BD.1, no new save section; Status: INTEGRATED on acceptance.

**W2:** `claim-core-mech-w2-dose-window-2026-XX` | `CORE-MECH-W2-DOSE-STORM-WINDOW` | Core: none (provider + call-site); Evidence: direction + counterplay + neutral-fallback + two-pass; Status: INTEGRATED.

**W3:** `claim-core-mech-w3-winter-pressure-2026-XX` | `CORE-MECH-W3-WINTER-PRESSURE-POWER-WATER` | DP-CM-1 resolution recorded; two consumers; boundary tests; Status: INTEGRATED.

**W4:** `claim-core-mech-w4-breakdown-2026-XX` | `CORE-MECH-W4-BREAKDOWN-CONSEQUENCES` | W1+W2 landed; three receivers; mid-event save test; Status: INTEGRATED.

**W5–W11:** one row each in the same shape, with DP-CM resolutions attached where applicable (DP-CM-2, DP-CM-3, DP-CM-4, DP-CM-5).

**W12:** `CORE-MECH-W12-BALANCE-BASELINE-REFRESH` | COMPLETE | published deltas + filed retune targets; program closeout.

**Decision register entries the integrator records:** DP-CM-1 (first consumers), DP-CM-2 (resolver), DP-CM-3 (placement), DP-CM-4 (vocabulary, if needed), DP-CM-5 (adoption order), plus any DEC the waves escalate (a new save section, a vocabulary amendment, a second authority).

**Plan closeout addendum (filled at close, revision R2):** the drift register (AJ) re-probed at close HEAD; the backlog delta restated; the next-generation list (BW) updated with what actually closed.

---

---

# Appendix CA — Precision-pass symbol verification (R2 closeout, fixed-string probes)

Every symbol the plan binds an implementer to was re-verified with fixed-string probes at R2 close:

| Symbol | Probe | Result |
|---|---|---|
| `ConsumeFood(string foodItemId, int neededCount, out int spoiledConsumed)` | `rg -F` | `FoodPreservationSystem.cs:239` |
| `GetSpoiledFood` / `DiscardSpoiled` | `rg -c` | 2 / 1 files |
| `HoldfastRuntimeSession.ConsumeFoodResult` | `rg -c` | 2 files (host + CLI path) |
| `DiseaseSystem.TryExpose` / `TryInfect` | `rg -c` | 9 / 1 files |
| `disease_catalog.exposure_sources` | `rg -c` | 1 file |
| `DoseLedgerSystem.BookReading` | `rg -c` | 4 files (Core + callers + tests) |
| `YearOfAshCatalogLoader.LoadEvents` | `rg -c` | 3 files |
| `ExpeditionVehicleSystem.EffectiveBreakdownRiskMultiplier` / `RepairTrackGear` | `rg -c` | 1 / 2 files |
| `FactionStanceEngine.ModifyTrust` | `rg -c` | 10 files |
| Reckoning `EnrollEvidence` | `rg -c` | 4 files |
| `IFlagLedger` + `GetCounter` | `rg` | interface present; counters present |

**R2 closeout statement.** No stale verb names remain in §7, §8, the runbooks, or the sketches. Sixteen open premises remain **by design** (BP), each with a probe and a defined failure action. The plan's integration architecture is tightened by the receiver contracts (AA), the interface contracts (BK), the hub sequencing (Y), and the cross-plan map (AY). The revision is complete; Wave 1 execution proceeds under Appendix BV.

---

---

# Appendix CB — Verification quick-reference (copy-paste blocks)

**Premise sweep (run at every wave P0).**
```bash
git rev-parse HEAD
rg -n 'Assert.Equal(26[0-9]' Ashfall.Core.Tests/Save -g '*.cs'          # pin
python3 - <<'PY'
import json
m=json.load(open('docs/player_surface_manifest.json'))
print(m['totalSurfaces'], m['interactiveSurfaces'], m['readOnlySurfaces'])
p=json.load(open('docs/ci/port_contract_policy.json')); print(p['total_seams'])
PY
```

**W1 premises.**
```bash
rg -n 'Disease|Contaminat|Pathogen' Assets/Ashfall.Core/Shelter/FoodPreservationSystem.cs   # expect none
rg -n 'ConsumeFood' src --glob '*.cs' | grep -v FoodPreservationSystem                     # expect none
rg -n 'TryExpose|TryInfect' Assets/Ashfall.Core/Disease/DiseaseSystem.cs
rg -n 'exposure_sources' Assets/Ashfall.Core/Disease/DiseaseCatalog.cs
```

**W2/W3 premises.**
```bash
rg -n 'YearOfAsh|StormWindow' Assets/Ashfall.Core/DoseLedgerSystem.cs                       # expect none
rg -ln 'YearOfAsh' Assets/Ashfall.Core --glob '*Power*' --glob '*Water*'                    # expect none
rg -n 'BookReading' src Assets/Ashfall.Core --glob '*.cs'
```

**W4–W11 premises.**
```bash
rg -n 'Injur' Assets/Ashfall.Core/Medical src/Host --glob '*.cs' | head                    # W4 receiver
rg -ln 'Raid|Siege' Assets/Ashfall.Core                                                      # W5 resolver
rg -n 'TravelEncounter' src/Host/ExpeditionHostSession.cs                                    # W6 selector
rg -in 'reopen' Assets/Ashfall.Core --glob '*Quest*.cs'                                      # W7 expect none
rg -n 'EnrollEvidence' Assets/Ashfall.Core --glob '*.cs' | head                             # W10 callers
sed -n '1,30p' Assets/Ashfall.Core/Flags/IFlagLedger.cs                                     # W11 substrate
```

**Static guards after each wave.**
```bash
rg -n 'FoodSafetySystem|WinterPressureSystem|GossipNetwork|TriggerRegistry' Assets src --glob '*.cs'   # expect none
rg -n 'new Random\(|System\.Random|Guid\.NewGuid' $(git diff --name-only HEAD~1 | grep -E '\.cs$')        # expect none
```

**Focused tests and probes.**
```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/FoodborneExposureBridgeTests.cs   # W1, alone first
godot --headless --path . -- --<wave>-selftest                                          # probe name re-verified live
```

**Rules.** One new test file runs alone first. Probes are evidence, not gates. Never run the full suite by default. Aggregate only static mappings with per-row messages; keep determinism/save/lifecycle/mutation tests independent.

---

# Appendix CC — Program sign-off block (foreman/integrator)

```
Program: CORE-MECH-2026-09-25 R2
Plan review:        ☐ approved   ☐ approved with notes   ☐ returned
Premise sweep:      12 promoted · 1 discarded (B-33 sealed) · 1 deferred (B-23) · 6 backlog
Waves:              12 (W1–W12) · tiers declared · gates named
PIR:                Appendix A (10 gates) — required per wave before code
Seal framework:     Appendix B (gap/feature/mechanic + meaningfulness pentad)
Precision pass:     Appendix CA — no stale verb names; 16 open premises by design
Wave 1 authorization: ☐ authorized to integrate now
First claim:        claim-core-mech-w1-foodborne-2026-XX (paths per W1 impact map)
```

**Reviewer's note (optional, one paragraph):** _what was strong, what was missing, what must be true before Wave 1 code lands._

---

# Appendix CE — Wave 1 claim card (paste into WORKTREE_OWNERSHIP.md on authorization)

```
claim-core-mech-w1-foodborne-2026-XX
Package: CORE-MECH-W1-FOODBORNE-DISEASE-BRIDGE (Wave 1 of CORE-MECH)
Owner: <integrator>
Data: Assets/StreamingAssets/Data/disease_catalog.json (additive foodborne exposure_sources rows)
Host: src/Host/HoldfastRuntimeSession.cs (thin exposure seam at the real eat path)
Main: src/Main.Plans62_65.cs (expose the preservation owner to the eat path; accessors only)
Surface: existing food/kitchen surface (bind; strip per Appendix AL.1) — or defer with a named reason
Tests: Ashfall.Core.Tests/Shelter/FoodborneExposureBridgeTests.cs (new; runs alone first)
No new save section; no port reclassification of existing seams (new seam classified in the same change)
Receivers: DiseaseSystem (sole) under the AA.1 receiver contract
Acceptance: focused tests + holdfast consumption regression + integrity gate + scenario BD.1 + handoff
Status: ACTIVE
```



# Appendix CD — Core-loop dependency diagram and first-failure analysis

```
SEASON AUTHORITY (Year-of-Ash calendar)
   ├── W2 fallback window ──> DoseLedgerSystem ──> bands, cumulative
   └── W3 pressure band ──┬──> water owner draw
                          └──> power owner draw

FOOD AUTHORITY (FoodPreservationSystem: cohorts, cures, spoilage, shelf life)
   └── W1 spoiled share ──> [seam] ──> DiseaseSystem ──> ward, outbreaks

EXPEDITION AUTHORITY (ExpeditionVehicleSystem: condition, gear, risk)
   ├── W4 breakdown band ─┬──> medical intake
   │                      ├──> DoseLedgerSystem (W2 conditioning applies)
   │                      └──> DiseaseSystem (contamination; W1 seam shape)
   └── W6 migration read model ──> travel-encounter selector

DEFENSE AUTHORITY (perimeter/sky values)
   └── W5 projection ──> raid resolver (FactionStanceEngine remains standing authority)

SOCIAL AUTHORITY
   ├── moral choices ──> W8 propagation ──> rumor/info owner ──> radio/journal
   ├── belief movements ──> W9 bounded delta ──> FactionStanceEngine
   └── quests + flags ──> W7 reopen grammar
                                        └── W11 one-shot primitive (flag family)

RITUAL AUTHORITY ──> W10 enrollment ──> Reckoning evidence ──> standing record

MEASUREMENT ──> W12 paired seeded runs ──> docs/balance/ deltas ──> filed retune targets
```

**What breaks first (failure-order analysis).** If two waves land in the wrong order: (1) W4 before W2 — breakdown exposure books unconditioned dose, making the storm window invisible in one of its two consumers; (2) W3 before W2 — two band providers exist and drift; (3) W8 before W11 — the ad-hoc guard and the primitive disagree on re-arm semantics; (4) W10 before the vocabulary check — evidence enrollment writes into a vocabulary that cannot admit it. The sequence (Y) exists to prevent exactly these four.

**What is safe to reorder.** W5, W6, W7, and W9 are mutually independent and independent of W1–W4; they may land in any order or in parallel, subject only to claim/hub discipline (P.1).

**Coupling growth.** The atlas (BY) shows that the program's value compounds: by W12, twelve new couplings exist among systems that already own their states, and the next generation inherits read models rather than needing new authorities (BW).

**The single sentence.** This program does not add a game; it adds the twelve conversations the existing game has been waiting to have — spoilage with illness, seasons with survival, breakdowns with bodies, walls with raiders, migrations with roads, choices with rumors, beliefs with politics, grief with endings, and triggers with the waves that follow.

---

# Appendix CF — Wave 1 closeout addendum (integrated 2026-09-25)

**Package:** `CORE-MECH-W1-FOODBORNE-DISEASE-BRIDGE` — **SEALED at MECHANIC tier.**

**What landed (four changes, in the planned order).**
1. **Data (data-first, S2).** `Assets/StreamingAssets/Data/disease_catalog.json` gains one additive `exposure_sources` row: `spoiled_preserved_stock` → `disease_typhoid_waterborne`, `base_probability` 0.35, `mitigating_trait_id` empty (the field is catalog-only and unconsumed by code, so an empty value avoids implying a counterplay that does not exist).
2. **Core (pure, S3).** New `Assets/Ashfall.Core/Disease/FoodborneExposureMath.cs`: `SpoiledShare(total, spoiled)` (0..1, fail-closed at 0), `EffectiveProbability(base, share)` (clamped, NaN-neutral), `ExposureModifierForShare(share)`, and the source-id constant. No state, no rng, no engine reference — the only place preservation and disease are translated.
3. **Host (seam, S3).** `src/Host/HoldfastRuntimeSession.cs` gains a one-shot `FoodConsumed(itemId, amount, survivorId)` event, raised **once per successful meal** (both the inventory path and the fallback path); never for water, blocked eats, or failures. The session remains ignorant of preservation and disease.
4. **Main (wiring, S3/S4).** `src/Main.Plans62_65.cs` subscribes `OnFoodConsumedForSpoilage`: reads the preservation owner's `GetTotalFood`/`GetSpoiledFood` → share → the catalog row → `DiseaseSystem.TryExpose` with `ProbabilityModifier = share`, `BypassImmunity = false`, `Day = _simDay` → truthful journal fact (`food_spoilage_exposure_…`) stating either the contraction or the passed/blocked outcome.

**Receiver contract honored (AA.1).** Disease is the sole receiver; the seam calls `TryExpose` (never `Infect` directly), so immunity, vector countermeasures, the already-infected check, and the authority's seeded roll all remain in the disease owner. The bridge invents no probability — the catalog row is the authority.

**Fail-closed paths (all tested or code-evident).** No preservation owner → no exposure. Untracked item → no exposure. Nothing spoiled → no exposure. Catalog row missing → no exposure. Disease host absent → no exposure. Unknown disease → `unknown_disease` blocked.

**Evidence.**
| Check | Result |
|---|---|
| `FoodborneExposureBridgeTests` (new; alone first) | **10/10 PASS** |
| `DiseaseSystemTests` (regression) | **16/16 PASS** |
| `Ashfall.Core.Tests/Shelter/` (preservation + kitchen regression) | **829/829 PASS** |
| `CatalogIntegrityValidatorTests` | **10/10 PASS** |
| `godot --headless --path . -- --data-integrity-selftest` | **PASS** — 425 catalogs, 0 errors |
| host build `Ashfall.csproj` | **0 errors / 0 warnings** |
| save pin | **266 unchanged** (no new section) |
| port contract | no new public Core seam beyond a static helper; no reclassification required |

**Balance.** Direction asserted (spoiled share ⇒ exposure attempt; clean stock or discard ⇒ none); magnitude deferred to W12 per AN.4 (no inline tuning). Anchor for the 0.35 base: the catalog's existing `foul_water_draw` precedent (0.4) — foodborne risk sits just below the waterborne route, which matches the fiction (spoiled preserved stock is a poor substitute for bad water).

**Deliberately not done (named, per the plan's honesty rules).** UI strip (AL.1) — no surface change was claimed in this wave; the mechanic lands first and the strip is a bind-only follow-up. W4 reuse of the seam shape. W12 measurement.

**Next.** W2 (`CORE-MECH-W2-DOSE-STORM-WINDOW`) per Appendix Y ordering: the shared seasonal provider lands there first, and W3 consumes it.

---

---

# Appendix CG — Wave 2 closeout addendum (integrated 2026-09-25)

**Package:** `CORE-MECH-W2-DOSE-STORM-WINDOW` — **SEALED at MECHANIC tier.**

**Precision re-verification before coding (PIR-1/P0-3).** Re-probed at HEAD `1678c074` with Wave 1 landed: `BookReading` production call sites are `DoseLedgerHostSession.ScribeReading` (the player's “Book a reading” path) and a CLI retention probe (left untouched — probes are evidence, not the game path); `year_of_ash_events.json` is the canonical **Day 180–360** vocabulary (52 events, 49 distinct days, fields `id/day/phase/title/description/temperatureDeltaC/hazardType`) with radiological rows already present (`radioactive_flood`, `radon_hazard`, `radio_transmission`, plus artillery/black-blizzard/steam/toxic rows). Conclusion: the plan's AW.2 sketch was correct in shape; the vocabulary needed an additive field rather than new rows, and the conditioning site was pinned to the host session.

**What landed (four changes, planned order).**
1. **Data (S2).** `year_of_ash_events.json`: additive `exposureMultiplier` on 10 radiological/high-blast rows — radioactive flood 1.8, radon 1.6, direct shelling 1.5, steam hammer 1.35, artillery 1.25, toxic combustion 1.2, black blizzard 1.15, radio 1.15, meteorological/geological 1.1. **Every other row stays exactly 1.0**, so untouched content keeps its previous behavior byte-for-byte in effect.
2. **Core (pure, S3).** New `Assets/Ashfall.Core/YearOfAsh/FalloutWindowProvider.cs`: day → multiplier with a ±1-day radius, strongest-wins overlap, 8× authored sanity clamp, NaN/≤1 rows ignored, `Empty` neutral provider, and `WindowFor(day)` provenance (event id + hazard type) for truthful readouts. Additive field on `YearOfAshEventEntry`. No state, no rng, no engine reference.
3. **Host (single conditioning site, S3).** `DoseLedgerHostSession` gains optional `DayProvider` and `FalloutWindowProviderRef`; `ScribeReading` multiplies the nominal reading **once** and names the window in its returned text.
4. **Main (wiring, S3).** `SetupDoseLedger` binds `() => _simDay` and builds the provider from the Year-of-Ash catalog, wrapped in a fail-closed catch: any load error ⇒ neutral multiplier, never a crash and never a silently reduced exposure.

**Receiver contract honored (AA.2).** The dose ledger keeps owning every reading rule: the seeded roll, anti-rad timing, band edges (`BandFor` untouched), cumulative totals, and administrative overrides. The season conditions only the *nominal input* — one multiplication, at one site.

**Fail-closed paths (tested).** No provider ⇒ 1.0 everywhere. No authored row ⇒ 1.0. NaN/zero/negative rows ignored. Runaway authored values clamped. Overlapping windows resolve to the strongest day. All authored rows stay inside the 180–360 canon (asserted).

**Evidence.**
| Check | Result |
|---|---|
| `DoseStormWindowTests` (new; alone first) | **12/12 PASS** |
| `Ashfall.Core.Tests/Medical/` (regression) | **507/507 PASS** |
| `FoodborneExposureBridgeTests` (Wave 1 regression) | **10/10 PASS** |
| `godot --headless --path . -- --data-integrity-selftest` | **PASS** — 425 catalogs, 0 errors |
| host build `Ashfall.csproj` | **0 errors / 0 warnings** |
| save pin | **266 unchanged** |
| `DoseLedgerSystem` math / `BandFor` | **untouched** (grep-verified by construction; no edit) |

**Balance.** Direction asserted only (window day ⇒ higher nominal ⇒ higher cumulative under an identical seed). The ten multipliers are **authored data** pending W12 measurement; no inline tuning (AN.4).

**Deliberately not done (named).** The dose register's per-reading band chip (AL.2) — the readout now *names the window in text*, which satisfies legibility for v1; a dedicated chip is a bind-only follow-up. W3's reuse of this provider (the shared-provider contract BK.1) — W3 is next and now has its provider to consume. W4's exposure routing.

**Next.** W3 (`CORE-MECH-W3-WINTER-PRESSURE-POWER-WATER`), consuming this same provider per the BK.1 interface contract (one provider, two consumers, one multiplication each).

---

---

# Appendix CH — Wave 3 closeout addendum (integrated 2026-09-25)

**Package:** `CORE-MECH-W3-WINTER-PRESSURE-POWER-WATER` — **SEALED at MECHANIC tier (water consumer); power consumer deferred by decision (DP-CM-1 as authored).**

**What landed.**
1. **Data (S2).** `year_of_ash_events.json`: additive `pressureMultiplier` on eight winter-strain rows — deep freeze 1.6, black blizzard 1.35, ventilation choke 1.3, diesel gelling 1.25, thirsty season 1.2, barometer 1.15, steam hammer 1.1, permafrost 1.1. Every other row stays exactly 1.0, and Wave 2's `exposureMultiplier` column is untouched — a diesel crisis is utility pressure, not fallout (CM-DR-08).
2. **Core view (S3).** New pure `SeasonalPressureProvider`: ±1-day radius, strongest-wins overlap, 4× clamp, NaN/≤1 rows ignored, `Empty` neutral, provenance for readouts.
3. **Core owner seam (S3).** `WaterTreatmentSystem.SeasonalFilterLoadMultiplier` — a read-once `(day) → multiplier` view applied at the **single** filter-degradation site, clamped 0–4, defaulting to 1.0 so an unwired system is provably unchanged.
4. **Bug fix (forensic, same seam).** ReverseOsmosis was charged **twice** per batch (branch subtracted, shared tail subtracted again). Now exactly once; a regression test pins the corrected value.
5. **Main (S3).** Water setup binds the provider from the Year-of-Ash catalog, fail-closed to neutral on any load error.

**Evidence.**
| Check | Result |
|---|---|
| `SeasonalPressureProviderTests` (new; alone first) | **12/12 PASS** |
| `Ashfall.Core.Tests/Shelter/` (water regression, incl. the new suite) | **841/841 PASS** |
| `DoseStormWindowTests` (W2 regression) | **12/12 PASS** |
| `FoodborneExposureBridgeTests` (W1 regression) | **10/10 PASS** |
| `--data-integrity-selftest` | **PASS** — 425 catalogs, 0 errors |
| host build `Ashfall.csproj` | **0 errors / 0 warnings** |
| save pin | **266 unchanged** |

**Why the power consumer was deferred, not forced.** The plan's DP-CM-1 pre-registered “water first” as the default, precisely because the power chain might not have a clean seam. It does not: `PowerGridSystem.TickDay` derives fuel need internally from generation with no external demand input. Conditioning it would mean adding an additive Core seam to a mature, heavily-consumed power file — a different blast radius than this wave's claim, and exactly the kind of widening the factory forbids (“one plan = one bounded outcome”). It is filed as **W3b**, with the deferral asserted in code so it cannot vanish.

**Balance.** Direction asserted (a deep-freeze day degrades the filter faster; unwired = baseline). The eight multipliers are authored data pending W12.

**Next.** W4 (`CORE-MECH-W4-BREAKDOWN-CONSEQUENCES`) — the first wave that *consumes* W1 and W2 (exposure seam shape + conditioned dose), which is why the sequence placed it fourth.

---

---

# Appendix CI — Wave 4 closeout addendum (integrated 2026-09-25)

**Package:** `CORE-MECH-W4-BREAKDOWN-CONSEQUENCES` — **SEALED at MECHANIC tier.** This is the first wave that *consumes* W1 and W2, exactly as the sequence predicted.

**What the forensic pass found first (CM-DR-11).** The plan said a breakdown “rolls and returns a bool nobody reads.” Half of that was wrong: `PrepareForExpedition` really does roll (`condition < 20 && rng < 0.3`), really does set `isBrokenDown`/`breakdownCause`, and `PrepareVehicleForDispatch` really does read the bool — to **abort the sortie**. The real gap is sharper and more damning: a breakdown costs the **mission** and never the **crew**.

**What landed (full solution, four layers).**
1. **Core (owner).** `VehicleBreakdownKind` + `VehicleBreakdownOutcome` (`BrokeDown`, band, severity, nominal mSv, cause, vehicle, survivor slot) and `ResolvePrepBreakdown(vehicleId, distanceKm, rng)`, which performs the *same* consuming travel and resolves the crew band from a pure function of condition and the injected seed. `PrepareForExpeditionCore` was extracted so the **legacy tuple API is byte-for-byte unchanged** while the new path is fully deterministic under one seed. Event: `OnBreakdownResolved`, raised once per breakdown.
2. **Host (one consuming call).** `ExpeditionHostSession` now calls `ResolvePrepBreakdown` instead of the bare tuple, attaches the dispatching `survivorId`, and invokes a new `BreakdownConsequenceSink`. Unbound sink ⇒ pre-W4 behavior preserved (abort, crew unharmed). The pre-existing `inst.isBrokenDown` repair gate *is* the exactly-once guard.
3. **Shared dose site (refactor, not duplication).** `DoseLedgerHostSession.BookConditionedExposure` extracted (CM-DR-12): `ScribeReading` and W4 both call it, so the storm-window conditioning remains **one** site with two callers.
4. **Main (routing).** Injury → `NeedsSystem.Modify(Health, -x)` plus a `HealthHistorySystem.LogHealthEvent` record (CM-DR-13); exposure → the shared conditioned booking with a campaign-rng fork; contamination → `DiseaseSystem.TryExpose` using the authored `micro_hazard_contamination` row (the W1 seam shape — never a direct `Infect`, so immunity still applies); every branch writes one truthful journal fact.

**Evidence.**
| Check | Result |
|---|---|
| `BreakdownConsequenceTests` (new; alone first) | **10/10 PASS** |
| `Ashfall.Core.Tests/Expeditions/` (regression) | **346/346 PASS** |
| W1 / W2 / W3 focused suites | **10/10 · 12/12 · 12/12 PASS** |
| host build `Ashfall.csproj` | my files compile clean; see note below |
| save pin | **266 unchanged** (no new section) |

**Host-build note (transparency).** The full host build is currently red from a **concurrent agent's** in-flight edit to `src/Main.PlayerSurfaces.cs` (`Ashfall.Core.World.WeatherKind` does not exist), and separately from their untracked `src/UI/PfglOctetBoardPanels.cs` earlier in the session. Neither file is in this wave's claim. Every file this wave touched compiles without error; the blocker is owned by another lane and was deliberately not “fixed” here.

**Balance.** Direction asserted (a worn vehicle can injure, expose, or contaminate the dispatcher; repair is the counterplay and the gate). Band weights (55/25/12/8) and the exposure magnitude are authored constants pending W12 (BE.2, BE.7) — no inline tuning.

**Next.** W5 (`CORE-MECH-W5-DEFENSE-SEEGE-COUPLE`), whose P0 hinges on the unresolved P0-6 premise (the raid/siege resolver) and DP-CM-2.

---

## Size and steadiness note (R6)

The plan is **262k characters**, above the 150–170k target now in force and above the 250k ceiling that preceded it. That is a deliberate, quantified trade, not drift: ~192k of the body is 86 appendices of *verified* material (runbooks, evidence cards, test catalogs, receiver contracts, closeouts with real command output), and the four sealed waves' evidence lives there.

Two reduction paths exist if the target is enforced:
- **Consolidation (~−35 to −45k):** fold the overlapping pairs (wiring sketches into runbooks, test catalogs into acceptance matrices, scenario sheets into manual scripts, cluster map into the inventory survey, checklists into one). Costs navigation convenience; loses no unique fact.
- **Depth trim (~−90k):** drop the deep-polish appendix tier entirely (sketches, dossiers, evidence cards, index). Faster to read, materially less implementation-grade.

Neither was applied unilaterally because both trade verified depth for a character target, and the same request asks for quality ensurance. The choice is the foreman's; this section exists so the decision is explicit rather than accidental.

---

# Appendix CJ — Wave 5 closeout addendum (integrated 2026-09-25)

**Package:** `CORE-MECH-W5-DEFENSE-SEEGE-COUPLE` — **SEALED at MECHANIC tier (legibility seal).**

**The forensic pass overturned the wave's premise (CM-DR-14).** The plan said perimeter defenses were “decoration” and listed the raid resolver as an unresolved P0 premise. Both were wrong: `DefenseSystem.ResolvePreCombatRaid` is a real, single, correctly-wired resolver that already receives the real perimeter and a power-aware predicate. **DP-CM-2 is confirmed** — one resolver, no second siege model, nothing invented.

What was actually broken was the *reporting*, and it was worse than a missing number:

| Surface defect | Consequence for the player |
|---|---|
| `DefenseGridPanel` called `CalculatePerimeterStrength(null, null)` | every wall, turret, and powered emplacement they built was **invisible in their own strength readout** |
| the engagement result was never surfaced | after a raid the player learned nothing about what their defenses did |

**What landed.**
1. **Host.** `DefenseHostSession` gains `EmplacementPoweredProvider` (fail-closed when unbound), `LastEngagement`, and `RecordEngagement(...)`. The session stays a facade — it owns no defense math.
2. **UI.** The panel now projects with the **attached perimeter and real power state**, showing walls, turrets, and power alongside traps and penalties, plus a `last raid` line (repelled/breached, raiders neutralized, captured) drawn from the real engagement object.
3. **Main.** `ResolveRaidDefenses` wires the power provider, records the engagement, and writes one truthful journal fact naming the outcome and the defense contribution.
4. **No Core math changed.** `DefenseSystem`, `PerimeterDefenseSystem`, and `FactionStanceEngine` are untouched; the save pin is untouched.

**Evidence.**
| Check | Result |
|---|---|
| `DefenseProjectionTruthTests` (new; alone first) | **6/6 PASS** |
| `Ashfall.Core.Tests/Defense/` (regression) | **42/42 PASS** |
| W4 / W3 focused suites | **10/10 · 12/12 PASS** |
| `--data-integrity-selftest` | **PASS** — 425 catalogs, 0 errors |
| host build `Ashfall.csproj` | **0 errors / 0 warnings** |
| save pin | **266 unchanged** |

The suite includes two regression witnesses that will fail if anyone re-introduces the old behavior: a null perimeter must project zero walls/turrets, and a fortified perimeter under a fixed seed must never leave *more* raiders than a bare one.

**Deferred (named).** Sky-armor → orbital-harrow telemetry (CM-DR-15) ships as **W5b**: it is a separate authoritative chain with its own save family, and bundling it would cross two authorities in one bounded outcome.

**Next.** W6 (`CORE-MECH-W6-MIGRATION-ENCOUNTER-BRIDGE`), whose P0 targets the travel-encounter selection site and the `SeasonalHumanMigrationEngine` / `MigrationConsequenceEngine` pair (CM-DR-04 names them as the live owners).

---

---

# Appendix CK — Wave 6 closeout addendum (integrated 2026-09-25)

**Package:** `CORE-MECH-W6-MIGRATION-ENCOUNTER-BRIDGE` — **SEALED at MECHANIC tier.**

**P0-8 resolved.** The selection seam is `TravelEncounterSystem.GetEffectiveWeight` — the single place where stance, faction-war, and patrol-recognition multipliers already live. The bias joined them there rather than creating a second selector (the plan's collision map forbids the latter, and the seam made it unnecessary).

**The wave caught a design flaw in its own plan (CM-DR-16).** The plan sketched a uniform bias, `base × (1 + pressure·k)`. That is **mathematically inert**: scaling every candidate by the same factor cannot change which one a weighted pick selects. The focused test proved it — the sampled distribution was byte-identical with and without pressure. The fix, shipped before integration:

| Authored category | Susceptibility | Fiction |
|---|---:|---|
| Human | 1.00 | people move for people |
| Creature | 0.60 | herds and swarms follow the same corridors |
| Environmental | 0.25 | weather and terrain barely notice |
| Chained | 0.10 | story beats nudge, not swing |

**What landed.**
1. **Core.** `RegionEncounterPressureProvider` (region → 0..1, unbound = identity), a region-aware `GetEffectiveWeight` overload, the authored `MigrationEncounterBiasK` (0.5), and `MigrationSusceptibility` — read purely from authored catalog fields, no new content rows, no new selector, no eligibility changes.
2. **Main.** `BindMigrationToTravelEncounters` normalizes live `HumanMigrationHostSession.RegionWeights` against the strongest region and rebinds on the daily tick, so setup order (migration before expeditions or after) never leaves the bridge unbound. The provider is replaced, never duplicated.
3. **No authored encounter was touched.** The catalog is the same file, byte-identical; the mechanic lives in weighting.

**Evidence.**
| Check | Result |
|---|---|
| `MigrationEncounterBridgeTests` (new; alone first) | **10/10 PASS** |
| `Ashfall.Core.Tests/Expeditions/` (regression) | **356/356 PASS** |
| `Ashfall.Core.Tests/Narrative/` (travel encounters) | **306/306 PASS** |
| `--data-integrity-selftest` | **PASS** — 425 catalogs, 0 errors |
| host build `Ashfall.csproj` | **0 errors / 0 warnings** |
| save pin / catalog | **266 unchanged / no authored row touched** |

The suite pins the invariants that make this safe: unbound and zero pressure are **exact identity**, pressure can lift but never lower or erase, the eligible set is identical with and without pressure, and two systems under the same provider and seed pick identically.

**Balance.** Direction asserted (a migrating region measurably changes the encounter mix). Susceptibility values and `k` are authored constants pending W12; the W5b / W3b deferrals stand as named.

**Next.** W7 (`CORE-MECH-W7-QUEST-REOPEN-GRAMMAR`) — a data-driven reopen predicate over the quest owner, using `IFlagLedger` (the same flag namespace W11 will later formalize).

---

---

# Appendix CL — Wave 7 closeout addendum (integrated 2026-09-25)

**Package:** `CORE-MECH-W7-QUEST-REOPEN-GRAMMAR` — **SEALED at MECHANIC tier.**

**P0-9 resolved, with two findings the plan had not anticipated (CM-DR-18).** The lifecycle owner is `QuestRuntimeCoordinator`, reached through `ProceduralNarrativeHostSession.QuestRuntime` and already persisted by the `procedural_narrative` section — so this wave needed **no new save section and no pin change**. But the forensic pass found:

1. **The `Abandoned` lifecycle state had no transition into it.** The enum value existed; nothing could ever set it. The reopen grammar would have had nothing to reopen, so `Abandon` was added rather than deferred.
2. **Instances carry no prerequisite fields of their own.** The plan assumed a predicate over authored prerequisites already attached to each quest. They are not, so eligibility reads authored `prereq_quest_id` / `min_day` **actor bindings** at the host — which also keeps Core free of catalog and flag knowledge.

**Guard placement corrected (CM-DR-19).** The plan sketched the one-shot guard in the flag ledger. The owner already persists every instance, so the guard lives **on the instance** (`reopenedOnce`, `reopenedDay`) — which makes it save-safe by construction. The `quest.reopened.*` flag namespace is reserved for W11's one-shot primitive rather than written twice today.

**What landed.**
1. **Core.** `Abandon(instanceId)`, `Reopen(instanceId, day, eligibility)`, `GetReopenCandidates(eligibility)`, `OnQuestAbandoned` / `OnQuestReopened`, and the two additive instance fields. Completed, expired, and still-active quests are never reopened; the one-shot guard is per instance, forever.
2. **Main.** `EvaluateQuestReopenOpportunities(day)` runs immediately after the daily expiry pass in `TickPlan169Narrative`, lists candidates through the owner's own API, re-checks eligibility before each transition, and writes one journal fact per revive.
3. **No quest catalog row, no manifest entry, and no new section were touched.**

**A persistence hole was caught before it shipped.** The focused suite's `ReopenFlag_SurvivesSaveAndLoad` failed on first run: `CloneQuest` copies instance fields one by one, and the two new fields were not in the list — meaning a save/load would have let a reloaded campaign revive the same thread a second time. Fixed, and now pinned by the test.

**Evidence.**
| Check | Result |
|---|---|
| `QuestReopenGrammarTests` (new; alone first) | **13/13 PASS** |
| `Ashfall.Core.Tests/Quests/` (regression) | **47/47 PASS** |
| Save round-trip gate | **1598/1598 PASS** |
| `--data-integrity-selftest` | **PASS** — 425 catalogs, 0 errors |
| host build `Ashfall.csproj` | **0 errors / 0 warnings** |
| save pin | **266 unchanged** |

The suite also pins the edges that keep the grammar safe: a reloaded save cannot reopen again, an old save without the new fields reads as “never reopened” (not “already used”), and active/completed/expired quests are immune.

**Next.** W8 (`CORE-MECH-W8-GOSSIP-PROPAGATION`) — deterministic, distance-lagged propagation into the existing rumor/info owner, adopting W11’s one-shot primitive for per-window emission guards.

---

---

# Appendix CM — Wave 8 + W11 core closeout addendum (integrated 2026-09-25)

**Packages:** `CORE-MECH-W8-GOSSIP-PROPAGATION` and the **W11 core primitive**, both **SEALED**.

**The ordering blockade was resolved by solution, not deferral.** W8 was sequenced to *adopt* W11's one-shot trigger, which did not exist yet. Rather than shipping a local guard and formalizing it later, the primitive was **built first, inside this wave** (per the standing directive to resolve blockades inline). W11's wave card is fulfilled ahead of schedule, with its own suite, and W8 is its first adopter — the consumer the design always predicted.

**The gap itself was narrower and better than the plan claimed (CM-DR-21).** The information-flow owner is **complete**: `RumorSystem` already models hubs, propagation speed, decay, and interception. The authored `moral_choice_gossip.json` is prose/decay shape, and — the detail that made this clean — `MoralChoiceResolution` already carries `propagatesOnDay` (*“gossip leaves the witnessing circle on resolvedDay + 1..3”*). The missing piece was never a propagation model. It was a **seed**: a moral choice resolved, and the rumor network never heard about it.

**What landed.**
1. **W11 core — `OneShotTriggerLedger`.** `Arm(triggerId, dayThreshold, gateFlag)`, `TryFire(triggerId, day)`, explicit `ReArm`, Ordinal-stable `Census`. Fires at most once ever per arm; a fired trigger cannot be resurrected by `Arm`; history is rebuilt from the flag ledger, so a **reloaded campaign cannot replay a one-shot effect**. No new save section, no second store.
2. **W8 core — `MoralChoiceGossipSeed`.** Pure choice→seed translation: private choices never travel, quiet choices stay home, suppression is an explicit input, decisive choices travel *more accurately* than marginal ones, and untruth fades faster. Decay/speed are seeded and bounded; invalid input fails closed.
3. **Main.** `SeedMoralChoiceGossip` runs after a successful `TryResolveMoralChoice`, honoring the authored `propagatesOnDay`, and hands the seed to the **canonical `RumorSystem`** through a one-shot trigger keyed `moral.<questId>`. The info owner is not set up ⇒ the choice still resolves; nothing is invented.
4. **Untouched.** `RumorSystem` propagation math, the authored catalog, save registry, and every other owner.

**Evidence.**
| Check | Result |
|---|---|
| `OneShotTriggerLedgerTests` (new; alone first) | **7/7 PASS** |
| `GossipPropagationTests` (new; alone first) | **9/9 PASS** |
| `MoralChoice/` · `Flags/` · `InformationFlow/` regressions | **42/42 · 7/7 · 19/19 PASS** |
| `--data-integrity-selftest` | **PASS** — 425 catalogs, 0 errors |
| host build `Ashfall.csproj` | **0 errors / 0 warnings** |
| save pin | **266 unchanged** |

The primitive suite includes the invariant that matters most for a shared building block: **history survives save/load** — a fresh primitive over the same ledger still knows the trigger fired and refuses to fire again.

**Next.** W9 (`BELIEF-STANCE-BRIDGE`) — bounded `ModifyTrust` deltas from authored belief movements into the sole standing authority. W11 is now complete ahead of schedule, so the wave index's remaining cards are W9, W10, W12 plus the named W3b/W5b follow-ups.

---

---

# Appendix CN — Wave 9 closeout addendum (integrated 2026-09-25)

**Package:** `CORE-MECH-W9-BELIEF-STANCE-BRIDGE` — **SEALED at FEATURE tier.**

**The premise was wrong, and was corrected before any code was written (CM-DR-22).** The plan named `belief_movements.json` as a table of belief-*movement events*. It is not: it is the three authored belief-creed **definitions** (creed, comfort themes, blind spots, practices). The live movement authority is the **Zealotry owner** — per-survivor `belief_id` / conviction / fervor, publishing `OnConverted` and `OnCrisisStarted` facts. This is the same class of correction W8 made with `propagatesOnDay`: the plan reached for an authored event list where the codebase already had a real event stream.

So rather than discard the wave or invent an event catalog, the design was rewritten around the evidence:

- **The bridge subscribes to real belief events.** A conversion is a belief movement because `ZealotrySystem` says so — not because a JSON row pretended to be one.
- **The political affinity was authored additively onto the existing belief definitions** (`faction_leaning`, signed per faction). Creed and politics now live in one file, and the spiritual catalog remains the single authored source for belief meaning. No new catalog file, no schema churn, and the integrity selftest still reports **425 catalogs / 0 errors**.

**The runaway guard is in Core, not in the host (CM-DR-23).** A conversion wave can fire dozens of events in a single day; an unbounded `ModifyTrust` at the event site would let one mass conversion swing standing by hundreds of points in one tick. `BeliefStanceBridge` therefore enforces a **per-(belief, faction, day) budget** and clamps authored leanings to a ceiling. The bridge only ever **proposes** shifts — standing is written solely through `EnsureSharedFactionStance().ModifyTrust`, so the sole-write-path rule holds by construction, and the bridge has no trust field of its own.

**What landed.**
1. **Data.** `faction_leaning` on all three belief definitions — small, signed, authored (e.g. the Ash Witnesses read as loyal to the Archivists, unwelcome to Black Ops; the Listeners find favor with the Black Cross).
2. **Core.** Pure `BeliefStanceBridge`: conviction-weighted adherence, crisis **inversion** (friends of the belief lose standing, detractors gain a little), outreach **recovery** (the counterplay), per-day budget rollover, Ordinal-stable output for surfaces, and a `DescribeStandingInfluence` legibility view.
3. **Main.** Conversions and faith crises apply their shifts inside the existing zealotry event stream. **Unregistered factions are skipped** so no phantom faction rows can be conjured by a belief id; a missing stance authority fails closed (faith still works, only its political echo is absent); every applied move is journaled as a fact.
4. **Untouched.** `FactionStanceEngine` internals, the save registry, and every other owner. No new save section.

**Evidence.**
| Check | Result |
|---|---|
| `BeliefStanceBridgeTests` (new; alone first) | **14/14 PASS** |
| `Spiritual/` regression | **12/12 PASS** |
| Save round-trip gate | **1598/1598 PASS** |
| `--data-integrity-selftest` | **PASS** — 425 catalogs, 0 errors (new authored field accepted) |
| host build `Ashfall.csproj` | **0 errors / 0 warnings** |
| save pin | **266 unchanged** |

The suite's sharpest test is `MassConversion_CannotRunawayTrust`: 50 same-day conversions, asserted against the daily budget. That is the mechanic's safety proof, not a formality.

**Next.** W10 (`RITES-RECKONING`) — memorial rites feeding the Reckoning contract with the W11 one-shot primitive already available. W12 (balance baseline) then closes the sequence, with the named W3b/W5b follow-ups still open.

---

---

# Appendix CO — Wave 10 closeout addendum (integrated 2026-09-25)

**Package:** `CORE-MECH-W10-RITES-RECKONING-EVIDENCE` — **SEALED at FEATURE tier**, with its P0 hard gate resolved by evidence rather than by deferral.

**The gate found something the plan had assumed into existence (CM-DR-24).** W10 opened by requiring that the Reckoning's *evidence vocabulary* admit a rite fragment. The sweep's finding: **there is no typed vocabulary.** `enrolledEvidence` is an untyped counter whose documented meaning is *read machine-log fragments* — the `VerdictEvidenceChain` docstring states it outright — and `VerdictAccusationSystem` reads that counter to build culpability.

So the plan's implicit answer had to be **refused**. Enrolling memorial rites as machine-log evidence would have made grief a prosecutorial instrument against the player and left the diegetic readout lying about what the instruments detected. That is semantic drift wearing a feature's clothes.

The honest amendment is the one `ReckoningState` already models for every other trace kind (`dwellingDriftTotal`, cumulative dose, each with its own comment about being distinct): **one additive trace kind** — `riteTraceTotal` + `EnrollRiteTrace`, in the **existing** reckoning state and save section. No vocabulary redefinition, no accusation retune, no new store, no new save section.

**The seam moved upstream (CM-DR-25).** The plan presumed the call site was the death-path vigil in `src/Main.Campaign.cs` — a file that is shared and actively claimed by another integrator package. Rather than collide, the bridge subscribes to the **canonical event** `SpiritualMeaningCoordinator.OnMemorialRitePerformed`, which covers every rite performance no matter who performs it, and touches no shared file. Once-per-(mourner, rite) is enforced by the W11 one-shot primitive over the campaign consequence ledger, so a repeated vigil cannot inflate the register and the guard survives save/load.

**What landed.**
1. **Core.** `riteTraceTotal` + `EnrollRiteTrace` + `RiteTraceCount`, wired into the state capture and restore that copy fields one by one — the same class of persistence hole W7's suite caught, avoided here by construction. Old saves read as zero, which is the truthful legacy default.
2. **Core (legibility).** An additive deterministic `VerdictReadout.RiteTraceLine`, returning empty when no rite was recorded so callers can append unconditionally. The existing `LineFor` is untouched: **the instruments do not mourn**, and a rite is something the shelter chose to keep, not something the machines detected.
3. **Host.** The spiritual owner subscribes once, guards with the one-shot primitive, enrolls, marks the verdict dirty, and writes a journal fact. No verdict owner ⇒ the rite still happened; it simply left no mark.

**Evidence.**
| Check | Result |
|---|---|
| `RitesReckoningEvidenceTests` (new; alone first) | **11/11 PASS** |
| `Verdict/` regression | **61/61 PASS** |
| Save round-trip gate | **1598/1598 PASS** |
| `--data-integrity-selftest` | **PASS** — 425 catalogs, 0 errors |
| host build `Ashfall.csproj` | **0 errors / 0 warnings** |
| save pin | **266 unchanged** |

Two of those tests exist specifically to hold the P0 decision in place: `RitesDoNotInflateMachineLogEvidence` and `PhaseGate_StillKeysOnEvidenceNotRites` — a campaign that held ten vigils but read no logs has still opened no gate, and the accusation math can never count grief.

**Next.** W12 (`BALANCE-BASELINE-REFRESH`) closes the program: measure the ten integrated waves against the pre-wave baseline and publish the deltas, without retuning any number that would need a foreman-signed target.

---

# END OF CORE-MECH-2026-09-25 R12
