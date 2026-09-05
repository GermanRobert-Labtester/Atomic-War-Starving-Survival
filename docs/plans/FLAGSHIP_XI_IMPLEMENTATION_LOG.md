# Flagship Integration Plan XI — Implementation Log

Plans 154–157: Morale Contagion · Pathogen Outbreak · Subterranean Networks · PsyOps.

---

## Phase 0 — Baseline (2026-09-05)

Status: PASS (with recorded divergence)

Commands run:

- `dotnet build Ashfall.csproj` → **PASS** (0 errors, 0 warnings)
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` → **FAIL (pre-existing)** — 615 compile errors, all in
  concurrent streams' work-in-progress test files referencing Core APIs that do not exist yet
  (e.g. `TrapSite.baitStolen`, `TrapDefinition.theftChance`, `RadioDistressSystem.TryTriggerMoralChoice`,
  `DebtConsequenceHostBridge.Bounties`, `DutyRosterCatalog.GetSeasonForDay`, `RadioTuner.EvaluateFrequency`).
  These files belong to other in-flight feature streams (WildlifeTrapping weathering/bycatch, RadioDistress
  moral choices, Debt bounties, TravelEncounter route affinity, DutyRoster seasons, WeatherGate interactions).

### Divergence D1 — test verification path (MATERIAL, recorded)

The canonical gate `dotnet test Ashfall.Core.Tests` cannot go green in this tree through no fault of this
milestone: the foreign WIP test files do not compile. Per repo convention (`_verify_*.csproj` is a tracked
gitignore pattern), this milestone verifies through a **gitignored companion project**
`_verify_flagship11.csproj` that references `Assets/Ashfall.Core/Ashfall.Core.csproj` and compiles **only**
this milestone's test files (under `Ashfall.Core.Tests/Flagship11/`). Those test files are committed as
usual and join the standard suite automatically once the foreign streams land and the main test project
compiles again. No foreign file is modified, stubbed, or reverted.

Also recorded:

- Working tree carries ~1.3k staged/modified files from concurrent streams (audio cache batch + Core/tests).
  This milestone commits **path-scoped only** its own files; shared files
  (`CatalogIntegrityValidator.cs`, `CatalogIntegrityRules.cs`, `SaveSectionRegistry.cs`,
  `ContentUtilizationScanner.cs`) carry foreign unstaged drift and are committed as-is when touched
  (their newest worktree state is preserved in the commit; nothing is lost).
- `dotnet --version` = 10.0.302 (global.json rollForward latestMajor).
- No active concurrent edits during this session's start (no `.cs` mtimes within 10 min).

---

## Slice 1 — Shared authority reconnaissance

Status: PASS (5 parallel read-only audits, all facts verified against current worktree)

### Authority map (as audited 2026-09-05)

| Domain | Authority (verified) | Key APIs |
|---|---|---|
| Morale | `Ashfall.Core.Survivors.NeedsSystem` — `SurvivorNeedsState.Morale` 0..100, **HIGHER = WORSE**, 50 neutral | `Modify(id, NeedKind.Morale, delta)` NeedsSystem.cs:181 |
| Breakdown | `MentalHealthCrisisSystem.TriggerCrisis(id, stressInput, CrisisProfile)` — stress >80 Critical, >60 Severe, >40 Moderate | MentalHealthCrisisSystem.cs:72; ward capacity 2 |
| Social bonds | `SurvivorRelationsSystem` (affinity −100..100, trust, resentment, grief, bondType, ordinal `a\|b` key) + `TraumaBondSystem` (strength 0..1) | `RelatedIds`, `ModifyAffinity`, `GetBondStrength` |
| Social coordinator | `SurvivorSocialCoordinator` (wraps Leadership/Friction/Ration/TraumaBond/Atrophy) | `TickDay(day, survivors)`, `CaptureState/RestoreState` |
| Rooms/assignment | `ShelterAssignmentSystem` — one active assignment/survivor | `AreInSameRoom`, `GetAssignmentsForRoom`, `Assign`/`Unassign` (:108/:136) |
| Subgroups | Duty-roster roles (`DutyRosterIds.AssignmentRoles`) + room clusters; coordinator already groups by role (`TickFrictionPairs`) | `GetRoleOf(id)` |
| Disease | `Ashfall.Core.Disease.DiseaseSystem` (1270 lines) — 16 authored diseases, vectors water/air/blood/spore, incubation, spread, quarantine, treatments, outbreak events, seeded RNG, checksummed save v5 | `BindCatalog`, `TickDaily(day, candidates)`, `Quarantine/EndQuarantine`, `Infect`-family, `SuspectFromEvidence` pipeline bridge |
| Radiation | `RadiationSystem` — `GetDosimeter(id)`, host `RadStateFor(id).RadiationDose` 0..100 | read-only query for severity coupling |
| Rooms (medical) | `MedicalWardSystem` — `MedicalBed{Isolation}`, `bed_isolation` wired | `Admit/Discharge` |
| World | `WastelandMapSystem` (loc_* nodes, BFS routes) + `locations.json` (151 expedition destinations) | `Discover`, `PlanRoute` |
| Expedition | `ExpeditionSystem` — one expedition per survivor (`_active` keyed by survivorId), phases Outbound/Looting/Inbound/Camp/…; Camp = proven sub-phase seam | `Start`, `TickHours(hours, rng)`, `Estimate`, `TryGrantLoot`, `SetEncounterChanceMultiplier`, `Retreat` |
| Loot | `ScavengingTableCatalog.RollLoot(tableId, rng, filter)` | tables keyed by location_type |
| Weather | `WeatherSystem.Current` (Clear/Rain/Overcast/Ashfall/FalloutStorm/Blizzard/BlackRain) | flood mapping precedent: `SumpFloodingSystem.TickDay` :461-470 |
| Shoring/structural | `ExcavationSystem.ApplyShoring` (halves risk) + `ExcavationHazardSystem` (per-sector methane/flood/shoring/trapped-miners) | closest structural authority |
| Inventory costs | `Inventory.BeginTransaction(InventoryBill)` → `TryCommit/Cancel` | WildlifeTrappingHostSession.cs:57 pattern |
| Radio | `RadioHostSession` (MHz tuning, stations incl. `Jammed` state, `BroadcastIntercepted` event, `radio` save section v2); `RadioBroadcastCatalog` (5 files); `FactionRadioEngine`; `CommsArraySystem` (ArrayTier 1..3 + `SetPowerState` = transmitter/power precedent); `ShelterRadioStationSystem` | canonical event: Core event → host session subscribes → `LastEvent` + re-fire |
| Power | `PowerGridSystem` — rooms with DrawWatts/priority/breakers; consumers gated via `IsRoomPowered` / `!IsBrownout` | comms-array precedent Main.Plans198_201.cs:240-253 |
| Factions | THREE parallel standing authorities: `FactionStanceEngine` (trust −100..100, trade), `FactionWarSystem` (standing −100..100, war factions), `FactionBranchCoordinator` (prpf/military/rebel). NO single loyalty store | `ModifyTrust` / `ModifyStanding` / `ModifyStanding` |
| Faction ids (real) | holdfast_factions.json: `faction_the_office`, `faction_the_cutters`, `faction_the_fleet`, `faction_black_flotilla`, `faction_supply_corps`, `faction_railway_guild`, `faction_hydro_barons`, `faction_ordnance_foundry`, `faction_scavengers`; stations: `faction_civil_defense`, `faction_independent_survivors`, `faction_central_garrison`; branches: `faction_prpf`, `faction_military`, `faction_rebel`; `warlords_sector_4` | propaganda targets must come from this list |
| Recruitment | Distress-signal recruit flow (`DistressSignalDefinition.RecruitSurvivorId`) + door-encounter `factionStandingDelta` precedent (DoorEncounterSystem) | defectors must route through these |
| Traits | `SurvivorDefinition.traitIds` (survivors.json) via `SurvivorRosterSystem.FindDefinition(id)`; no central constants class; no claustrophobia yet | substring/Contains reads |
| Ticks | `CampaignDayCoordinator` phase 1..5, ordinal ownerId sort within phase. Phase 3: duty_roster → medical_disease → phase0_psychology → survivor_social → survivors_needs. Phase 4: debt_ledger → expeditions_caravans → narrative_quests_verdict → (new: psyops) → (new: subterranean) → world_evolution | Main.CampaignOwners.cs:13-53 |
| RNG | `ISeededRng` (xorshift64*); `CampaignRngStream.CampaignStreamIds` + `DeriveSeed`; derive per-tick seeds from persisted state (no stream state in saves) | Ports.cs:72, CampaignRngStream.cs |
| Save | `SaveSectionRegistry.All` + `SectionFileNames` + codec (`SaveChecksum`, throw-future/migrate-past) → `src/Host/XxxSaveStore.cs` via `SaveStoreHub.FromCodec` → `CaptureSection(key, payload)` in Main.SaveOrchestrator → envelope `manifestVersion` 2 | recipe in recon agent 5 report |
| Content utilization | `ContentUtilizationScanner` maps: AuthoritativeCatalogs + loader map + registryMap + runtime-consumer map; baseline `artifacts/content-utilization-baseline.json` — refresh after adding catalogs | ContentUtilizationGate fails on NEW ORPHAN |

### Divergence D2 (MATERIAL, amended) — Plan 155 extends the Disease Expansion

Recon found a complete fictional-disease authority (`Ashfall.Core.Disease.DiseaseSystem`: vectors =
transmission classes, incubation, spread engine, quarantine, treatments, outbreak events, checksummed
persistence). Building the plan's `PathogenSystem` as a second infection engine would duplicate ~90% of it
(plan §155.1 forbids this). Amendment, per plan §155.1's own preference list:

- `pathogens.json` = **strain catalog**: `pathogen_*` rows each with `strain_of` (strict ref to an authored
  `disease_*` parent) + fictional overrides (`incubation_days`, `lethality`, `infectivity`, severity,
  `radiation_severity_gain` abstract, `mutation_chance_per_day`, `mutation_targets`, `treatment_tags`).
- `PathogenStrainCatalog` (Core, `Ashfall.Core.Disease`) loads strains; an adapter **merges them into the
  `DiseaseCatalog` as derived `DiseaseDefinition`s** so the existing engine (spread/quarantine/treatment/save
  by id) runs strains with zero parallel state.
- Small `PathogenStrainSystem` owns only what the base engine lacks: deterministic mutation transitions
  (seeded, per active infection; persisted selection survives save) + radiation-severity coupling via an
  injected read-only dose delegate (never writes dose).
- Cure/research project: extends the strain system as a bounded, abstract, non-procedural project
  (`pathogen_cure_*` ids, item+labor costs via canonical inventory; unlock = efficacy modifier).
- Quarantine stays `DiseaseSystem.Quarantine/EndQuarantine` + pipeline `treatment_quarantine` + ward
  isolation beds. No new room authority.

### Divergence D3 (minor) — morale polarity mapping

`SurvivorNeedsState.Morale` is 0..100 where HIGHER = WORSE (documented NeedsSystem.cs:6-8). Plan's
"breakdown when morale <10%" maps to **Morale crossing ≥90 upward** (prev < 90, now ≥ 90, transition-based,
once). Hope = pressure driving Morale down; despair = pressure driving Morale up. Panic modeled as its own
channel feeding crisis stress input.

### Design decisions (Plans 154/156/157)

- **154 MoraleContagionSystem** (`Assets/Ashfall.Core/Survivors/`, `Ashfall.Core.Survivors`): owns ONLY the
  contagion channel state (hope/despair/panic intensities 0..1 per survivor, contagion event instances,
  isolation markers, schism cooldowns/pressure ledger). Reads bonds via `SurvivorRelationsSystem` +
  `TraumaBondSystem`, co-location via `ShelterAssignmentSystem.AreInSameRoom` + duty-role equality, morale
  via `NeedsSystem`. Applies net channel pressure via `NeedsSystem.Modify(Morale)` (buffered deltas,
  ordinal-sorted, commit after evaluation). Breakdowns route through `MentalHealthCrisisSystem.TriggerCrisis`
  (no parallel case list). Schisms operate on duty-role subgroups; sustained pressure ≥ threshold over
  consecutive days; `OnMoraleSchismTriggered` typed payload with stable ids. Social isolation =
  `ShelterAssignmentSystem.Unassign` + duty-role clear + contagion-owned isolation marker + daily cost.
  HopeBeacon = new `ShelterRoomDef` (CommonArea function) in `shelter_rooms.json` + hope source read by
  contagion when the room is built/staffed; costs enforced through the room's build_cost + staffing.
  Save: new `morale_contagion` section (checksummed codec).
- **156 SubterraneanSystem** (`Assets/Ashfall.Core/Subterranean/`): owns underground nodes (`sub_node_*`),
  discovery, structural integrity, oxygen, flood, shoring, connectivity (generated deterministically from
  campaign/world seed + surface anchor, generated once then persisted verbatim; restore never regenerates).
  Expedition bridge: underground sorties are canonical `ExpeditionSystem` expeditions whose location is an
  underground zone; the bridge supplies hazard context (encounter multiplier, oxygen drain per underground
  day with forced-retreat request, claustrophobia morale delta via the `ShelterDecorSystem.GetRoomMoraleDelta`
  consumption pattern, flood pressure from `WeatherSystem.Current` per the `SumpFloodingSystem` mapping,
  loot via new `table_loot_*` rows + `ScavengingTableCatalog`). Shoring = `TryShoreNode` with
  `Inventory.BeginTransaction` atomic billing. Save: `subterranean` section. UI: `SubterraneanMapPanel`
  (MapAtlasPanel pattern; undiscovered nodes hidden). New trait `trait_claustrophobe` in survivors.json
  conventions.
- **157 PsyOpsSystem** (`Assets/Ashfall.Core/Radio/`): owns campaigns (`psyops_campaign_*`), broadcast
  slots, reach model (S-unit conventions), jamming (abstract strength/coverage/cost/duration), counter-
  propaganda, campaign fatigue. Loyalty shifts ONLY via host-injected delegate routing to the existing three
  faction authorities (faction-scoped; unrelated factions untouched). Broadcasts gate on `CommsArraySystem`
  tier + power (host-supplied query). `OnBroadcastIntercepted` mirrors `RadioHostSession.BroadcastIntercepted`
  (host re-fires; core never touches UI). Leaflets = expedition encounter choice data using the
  `factionStandingDelta` precedent. UI: revive stub `TroposphericRadioRelayPanel` with typed
  `Bind(PsyOpsHostSession)` (removes an AGENTS.md stub entry in the same commit). Save: `psyops` section.
- **Tick placement**: pathogen strain tick inside phase-3 `medical_disease` owner immediately after
  `_disease.TickDaily` (Main.CampaignOwners.cs:325); contagion step appended inside `SurvivorsNeedsDayOwner`
  after decor morale (reads final day morale); subterranean + psyops as phase-4 day owners
  (`subterranean_network`, `psyops`) which sort after `expeditions_caravans` and before `world_evolution`
  as required by the plan's daily order.

## Slice 3 — Morale contagion (Plan 154)

Status: PASS (core behavior + host wiring + save + UI readout + beacon)

Changed:
- `Assets/Ashfall.Core/Survivors/MoraleContagionSystem.cs` — `IMoraleContagion` +
  `MoraleContagionSystem`: hope/despair/panic channels (typed enum), catalog-driven
  source events (snapshot template values), social-influence weights
  (eligibility × proximity × bond × resistance), buffered same-tick deltas committed
  after evaluation, ordinal-sorted iteration, ZERO RNG. Breakdown = transition-based
  Morale ≥90 crossing (polarity D3) routed through the canonical crisis authority
  (delegate); 7-day cooldown; re-arms on leaving the band. Schism = duty-role
  subgroup with ≥50% members ≥0.5 despair pressure sustained 3 days; cooldown 21d;
  `OnMoraleSchismTriggered` stable payload, one per day (ordinal-first).
  `TryApplySocialIsolation` calls canonical unassign + duty-clear ports, cuts
  influence both ways, +1 morale/day cost. HopeBeacon = idempotent ambient
  `contagion_hope_beacon` hope source while installed+staffed+powered.
  Capture deep-copies; Restore is non-operative (guard flag available).
- `Assets/Ashfall.Core/Survivors/MoraleContagionSave.cs` — versioned (v1) checksummed
  codec mirroring RadioSaveCodec; rejects tamper/checksumless/future; domain↔save mappers.
- `src/Host/MoraleContagionSaveStore.cs` — `SaveStoreHub.FromCodec` façade
  (coverage-gate compliant), section `morale_contagion`.
- `src/Host/MoraleContagionHostSession.cs` — thin session (LastEvent/StateChanged),
  beacon install marker + power/staffing gates, `GetInfluenceLines` read model.
- `src/Main.MoraleContagion.cs` — Setup/Save triad; ports wired to NeedsSystem,
  ShelterAssignmentSystem, DutyRosterSystem, TraumaBondSystem,
  MentalHealthCrisisSystem (`CrisisProfile.AcuteStress`); `InstallHopeBeacon`
  charges scrap_metal×4 + cloth×2 + battery×1 via atomic `InventoryBill`.
- `src/Main.CampaignOwners.cs` — contagion step in `SurvivorsNeedsDayOwner` after
  decor morale (reads final day morale; deltas land in today's state).
- `src/Main.SaveOrchestrator.cs` — `SaveMoraleContagion()` in SaveAll,
  `SetupMoraleContagion()` in restore path.
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` — `morale_contagion` section
  + filename (NOTE: shared file; foreign in-flight edits preserved as-is per D1).
- `Assets/StreamingAssets/Data/shelter_rooms.json` — `room_hope_beacon`
  (CommonArea, capacity 3, build_cost 4×scrap_metal/2×cloth/1×battery).
- `src/UI/SurvivorRelationsPanel.cs` — "Settlement Mood" text-status block
  (influence lines + beacon state; not color-only) via `BindContagion`.

Tests: `Ashfall.Core.Tests/Flagship11/MoraleContagionSystemTests.cs` — 16 tests
covering the §154.15 matrix: determinism, proximity grading, bond scaling,
hope/despair polarity, distinct panic + crisis stress, buffered multi-source,
breakdown once-per-crossing + re-arm + cooldown, isolation (authorities called,
influence cut, cost, source-side), beacon counter-pressure + decay, schism
sustain/threshold/cooldown/min-size/reset, event validation/idempotency, save
round-trip with identical continuation, restore non-operative, codec tamper
rejection, real-catalog resolution. 32/32 Flagship11 tests PASS.

Result: `dotnet build Ashfall.csproj` 0 errors/0 warnings; companion tests 32/32;
`--data-integrity-selftest` PASS (10573 ids; room_hope_beacon registered);
host boot completes ("Headless interactive boot completed").

Divergences:
- D4: plan field `narrativeEventId` omitted from contagion_events.json — no
  resolvable narrative-event id authority exists to reference; narrative surfacing
  is host-side via typed events + LastEvent.
- D5 (baseline, pre-existing, not this milestone): `ExpeditionRadarPanel._Ready` and
  `FactionsNarrativePanel._Ready` raise ObjectDisposedException on disposed Labels
  during headless boot — both are foreign in-flight UI files; boot completes anyway.
  Not touched, not attributed to Flagship XI.
- UI influence readout lands in the existing `SurvivorRelationsPanel` (no
  standalone `SurvivorUI` exists in this repo).

Remaining: host-side triggers for contagion sources (world events calling
`StartContagionEvent` — e.g. death → funeral grief) arrive with Slice 8
cross-system integration; Schism event consumption by narrative in Slice 7/8.

### Verification path note

All milestone gates run through the companion project (D1) until the foreign streams land:
`dotnet test _verify_flagship11.csproj` + `dotnet build Ashfall.csproj` + godot headless selftests
(`--data-integrity-selftest`, `--content-utilization-selftest`, new milestone selftest).

---

## Slice 4 — Pathogen strain lineage (Plan 155, amended per D2)

Status: PASS (engine hooks + strain layer + save + 100-day replay)

Changed:
- `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` — three ADDITIVE engine hooks:
  (1) `EffectiveLethalityModifier` optional delegate consulted in
  `ResolveOutcomes` (null = untouched math) for abstract radiation severity;
  (2) `MutateInfection(survivorId, from, to)` — fictional mutation as a pure
  state transition preserving clinical history (infected day, days sick,
  treatment ledger, quarantine), raises `OnStrainMutated` + `disease_strain_mutated`
  and re-checks outbreak containment on the vacated entry;
  (3) `GetDefinition(id)` + `RegisterStrain(def)` — first-class strain rows
  (catalog definition + simulation entry, idempotent).
- `Assets/Ashfall.Core/Disease/PathogenStrainSystem.cs` — the strain layer:
  `AttachStrains` merges `pathogen_*` strains into the engine as derived
  definitions (vector/countermeasure/tell inherited from the parent disease,
  outcome/spread classes overridden); `TickMutations` — deterministic
  per-active-infection rolls, seeds derived FNV-1a(day, survivorId, strainId)
  so nothing about RNG is persisted; ordinal iteration; mutation chance
  multiplies by (1 + dose/100 × radiation_severity_gain), clamped 0..1;
  `RadiationSeverityPressure` — bounded abstract lethality pressure fed to the
  engine hook (dose is READ-ONLY via the canonical radiation authority);
  fictional cure projects (bounded required-days, host charges materials,
  completion unlocks CureEfficacyLethalityRelief through future treatment flow
  and marks the strain cured). Capture deep-copies; restore is non-operative.
- `Assets/Ashfall.Core/Disease/PathogenStrainSave.cs` + `src/Host/PathogenStrainSaveStore.cs`
  — versioned checksummed codec + `SaveStoreHub.FromCodec` façade, section
  `pathogen_strains` (mutation results persist in the disease engine's own section).
- `src/Main.PathogenStrains.cs` — Setup/Save triad; radiation query wired to
  `SurvivorsHostSession.RadStateFor(id)?.RadiationDose`; `StartPathogenCureProject`
  charges antibiotics×2 + medical_kit×1 + chemicals×3 atomically.
- `src/Main.CampaignOwners.cs` — strain tick inside `MedicalDiseaseDayOwner`
  immediately after `_disease.TickDaily` (mutations → cure research → triage).
- `src/Main.SaveOrchestrator.cs` + `SaveSectionRegistry.cs` — `pathogen_strains`
  section wired into SaveAll/restore (registry file: foreign edits preserved as-is).
- `ContentUtilizationScanner.cs` — all four Flagship XI catalogs registered
  (AuthoritativeCatalogs + loaderPatterns + registryMap + runtime consumers);
  utilization gate PASS with 0 orphans, 164 gameplay-consumed.

Tests: `Ashfall.Core.Tests/Flagship11/PathogenStrainSystemTests.cs` — 9 tests:
strain merge (vector/countermeasure inheritance, engine-native infection,
authored diseases untouched), idempotent attach + honest orphan rejection,
mutation chance math (dose scaling + clamp), deterministic mutation transition
preserving history + quarantine, zero-chance stability, read-only radiation
coupling through the engine hook, cure validation/advance/unlock, save
round-trip + restore non-operative + tamper rejection, and the §155.15
**100-day deterministic replay** (same seed ⇒ identical trace; save/restore at
day 40 lands on the identical final trace). 41/41 Flagship11 tests PASS.

Result: `dotnet build Ashfall.csproj` 0/0; companion tests 41/41;
`--content-utilization-selftest` PASS; earlier `--data-integrity-selftest` PASS.

Divergences:
- D2 (recorded at Slice 1): strains are NOT a parallel engine; the infection
  model IS the disease engine. `pathogens.json` rows are lineage variants.
- Radiation couples severity via the engine hook (bounded 0.3 lethality
  pressure) and mutation pressure via chance multiplier — abstract values only.
- Patient-zero provenance (DoD 60): the engine does not track provenance; no UI
  claims any — "unknown" is the honest default. Deferred until the engine
  tracks exposure chains.

Remaining: medical-ward surface already shows strains through DiseaseHostSession
snapshots (they are disease rows); no dedicated panel needed. Outbreak→morale
cross-system trigger (`contagion_outbreak_fear` on outbreak declared) lands in
Slice 8.

---

## Slice 5 — Subterranean exploration networks (Plan 156)

Status: PASS (system + expedition bridge + save + host wiring + trait)

Changed:
- `Assets/Ashfall.Core/Subterranean/SubterraneanSystem.cs` — node state
  (discovery, structuralIntegrity, shoringLevel 0..3, oxygenLevel,
  ventilationInstalled, waterLevel, blocked); `EnsureNetwork(campaignSeed)` —
  deterministic one-time generation from catalog + seed (seeded per-node
  integrity/oxygen/water variance; ids = catalog ids verbatim; generated flag
  persisted; restore never regenerates); discovery flows surface-anchor →
  tier-1 nodes → one connected step down; `ApplyUndergroundDay` — environmental
  hazards run for the WHOLE generated network (the underground keeps its own
  time), oxygen drains only for occupants, low-O2 health request via canonical
  delegate, forced-retreat event under 10 O2, flood pressure from the canonical
  weather query (SumpFlooding scale; saturation blocks + erodes every saturated
  day, event on transition only), cave-in roll seeded per (day, nodeId) with
  abstract risk factors damped 25%/shoring level; `TryShoreNode`/
  `TryInstallVentilation`/`TryClearBlockage` — atomic `InventoryBill` billing
  (scrap_wood×4 + steel_rebar×2 [+scrap_metal×2 on second set]; scrap_metal×3 +
  battery×1; scrap_wood×2), permanent risk reduction, structure-sound gating;
  `UndergroundMoraleDelta` — claustrophobia bearer penalty (−3) only, zero
  otherwise. FNV-1a seeded rolls; no RNG state persisted.
- `Assets/Ashfall.Core/Subterranean/SubterraneanSave.cs` +
  `src/Host/SubterraneanSaveStore.cs` — versioned checksummed codec +
  SaveStoreHub façade, section `subterranean`; old campaigns load as "no
  network" and generate lazily from `CampaignRngStream.MasterSeed`.
- `src/Host/SubterraneanHostSession.cs` — thin session + the expedition
  bridge (156.6): every zone registers as a CANONICAL `ExpeditionDefinition`
  (distance by depth tier, danger/encounter risk from zone risk, loot through
  the zone's `scavenging_table_id`, `requiresDiscovery=true`) — no second
  roster; `TickDay` feeds each active underground sortie's environment hazards,
  applies claustrophobia morale via the needs authority, and routes forced
  retreats back through `ExpeditionSystem.Retreat`.
- `src/Main.Subterranean.cs` — Setup/Save triad; weather read from
  `WorldHostSession.Weather.Current`; health/morale deltas through NeedsSystem;
  claustrophobia read from `SurvivorDefinition.traitIds`.
- `src/Main.CampaignOwners.cs` — phase-4 `subterranean_network` day owner
  (ordinal after `expeditions_caravans`, before `world_evolution`).
- `SaveSectionRegistry.cs` + `Main.SaveOrchestrator.cs` — section + triad wired.
- `survivors.json` — `trait_claustrophobe` on 2 survivors (2-line surgical diff;
  self-registers via the traitIds definition-key mechanism).
- Content utilization: `subterranean_zones.json` loader edge registered in Slice 4's pass.

Tests: `Ashfall.Core.Tests/Flagship11/SubterraneanSystemTests.cs` — 11 tests:
deterministic generation + stable ids + seed variance, EnsureNetwork idempotence
after live mutation, anchor→connection discovery, oxygen drain/ventilation
recovery/health request/forced retreat, weather-driven flood (rise, seep-out,
saturation block + erosion), seeded deterministic cave-in with shoring damping
(roll window chosen so unshored fires, shored rides out), atomic shoring with
material gating and cost escalation, blockage clearing rules, claustrophobia
delta scoping, save round-trip + EnsureNetwork no-op after restore, old-save
lazy generation. 52/52 Flagship11 tests PASS.

Result: `dotnet build Ashfall.csproj` 0 errors; `--data-integrity-selftest`
PASS (262 catalogs); companion tests 52/52.

Divergences:
- Underground sorties are plain expeditions to registered underground
  destinations; oxygen/collapse hazards apply per underground DAY (the bridge's
  daily tick) rather than per travel tick — documented; a per-tick hazard seam
  can hook `SetEncounterChanceMultiplier` later without re-architecture.
- Loot tables reuse the canonical underground tables (metro_station,
  collapsed_structure, waterworks, etc.); a sealed-pre-war table expansion is a
  data-only follow-up.
- D6 (baseline): concurrent foreign streams landed broken files mid-slice
  (`CulturalArchiveSaveStore` etc. missing `using Ashfall.Core.Save;`,
  `Main.FlagshipInstitutions.cs` missing usings/day-owner member) — applied
  minimal mechanical fixes to unblock the shared build; their stream continued
  editing and self-completed the rest. Recorded, not attributed to Flagship XI.

Remaining: SubterraneanMapPanel UI + narrative echoes land in Slices 7-8.

---

## Slice 6 — PsyOps (Plan 157)

Status: PASS (system + faction routing + save + host wiring + tick)

Changed:
- `Assets/Ashfall.Core/Radio/PsyOpsSystem.cs` — campaign instances from
  `propaganda_campaigns.json` (validation: unknown id, duplicate campaign, one
  active channel per target faction); abstract reach model —
  `EffectiveReach = base × (transmitterReady ? 1 : 0.3 stall) × (1 − 0.6·jamming)
  × (0.25 if countered) × (0.6 fatigue after 7 consecutive broadcast days)`;
  signed loyalty pressure (Fear negative, all other themes positive) scaled by
  reach and receptiveness; ideological-pressure ledger (−100..100, 5%/day decay)
  as the influence record for targets without a runtime trust aggregate;
  abstract player jamming (per faction, strength/duration) and counter-
  propaganda (per campaign); seeded intercept rolls per (day, campaignId) —
  base 0.15, +0.2 under jamming; ordinal iteration; FNV-1a seeds; no RNG state
  persisted. Loyalty shifts reach real authorities ONLY via the host-routed
  `LoyaltyShiftRequested` delegate (faction-scoped). Capture deep-copies;
  restore is non-operative (applies no pressure, fires nothing).
- `Assets/Ashfall.Core/Radio/PsyOpsSave.cs` + `src/Host/PsyOpsSaveStore.cs` —
  versioned checksummed codec + SaveStoreHub façade, section `psyops`.
- `src/Host/PsyOpsHostSession.cs` — thin session (LastEvent/StateChanged) +
  `BroadcastIntercepted` bridge for intel/narrative consumers.
- `src/Main.PsyOps.cs` — Setup/Save triad; transmitter gate = comms array
  (`_commsArray.State.ArrayTier ≥ 1 && IsPowered` — the existing transmitter
  authority) AND grid not in brownout; `RouteLoyaltyShift`: garrison/warlord →
  `FactionWarSystem.ModifyStanding`, prpf/military/rebel →
  `FactionBranchCoordinator.ModifyStanding`, holdfast trade factions → pressure
  ledger only (no runtime trust aggregate exists for them in this host — D7).
- `src/Main.CampaignOwners.cs` — phase-4 `psyops` day owner (after expeditions,
  before world_evolution, matching the plan's daily order).
- `src/Main.SaveOrchestrator.cs` — `SetupPsyOps`/`SavePsyOps` wired.
- Content utilization edges (Slice 2 wiring) now resolve: `PsyOpsSystem` exists.

Tests: `Ashfall.Core.Tests/Flagship11/PsyOpsSystemTests.cs` — 9 tests:
campaign validation incl. one-channel-per-target (synthetic duplicate-target
catalog), expiry exactly-once, reach model (stall/jam/counter ordering),
faction-scoped shifts with Fear-negative theme sign, ledger accumulate/decay/
clamp, intercept determinism (identical traces) + jamming raises intercept
frequency (100-day statistical window), campaign fatigue, save round-trip +
restore non-operative + identical continuation + tamper rejection, and the
real catalog starting all 8 campaigns on 8 distinct targets.
61/61 Flagship11 tests PASS.

Result: `dotnet build Ashfall.csproj` 0 errors; `--data-integrity-selftest`
PASS (262 catalogs); `--content-utilization-selftest` PASS (0 orphans).

Divergences:
- D7: holdfast trade factions have no runtime trust aggregate in the current
  host (FactionStanceEngine instances are per-domain session state); psyops
  influence on them accrues in the system's own ideological-pressure ledger and
  is surfaced through readouts — routed shifts touch only authorities that
  actually exist (FactionWarSystem, FactionBranchCoordinator).
- Transmitter gate reads the comms array's saved state (`State.ArrayTier`,
  `State.IsPowered`) — the live system exposes state via `State`, not getters
  (the class was rewritten by a concurrent stream mid-slice; the gate uses the
  current surface).

Remaining: `TroposphericRadioRelayPanel` revival (typed PsyOps binding) +
leaflet expedition encounter data land in Slices 7-8.
