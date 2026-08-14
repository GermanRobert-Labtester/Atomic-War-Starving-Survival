# ASHFALL DEBUG LOOPS — AUDIT LOG
Each loop = 4 sweep audits. Every finding fixed before the loop closes.
Verification per loop: dotnet build Ashfall.csproj + dotnet test + godot selftest gates.

## Loop 0 — Data layer (pre-loop): CatalogIntegrityValidator built + wired (--data-integrity-selftest)
Findings: 884 errors -> 0 errors. Fixed: 39 missing items, 12 missing factions, 27 missing
narrative events, 11 missing locations, questline_master.json (194 code quest ids), repoints
(scavenger_camp, garrison, warlord, militia, Medical, sv_* dev ids, item_* prefix drift,
location near-misses), trait grants, family survivors, PhantomMemory host wiring (ISeededRng
NextDouble, Main.cs handlers + save store, orphaned block removal).
Gate: DATA_INTEGRITY_SELFTEST PASS (0 errors / 311 info warnings). Tests 319/319. All selftests PASS.

## Loop 1 — 4 sweeps (all findings fixed, verified green)
A. Determinism: 3 save paths emitted Dictionary/HashSet iteration order (PhantomMemoryEngine,
   DutyRoster assignments, LocationLayout parents + room-id sets) -> ordinal-sorted emission;
   cross-host checksum parity.
B. Save/load: PhantomMemoryEngine.CaptureState returned the LIVE state (aliasing) -> fresh
   copy + regression tests (snapshot isolation, ordinal order); DoseLedgerSystem.CaptureState
   same class -> fixed. Core CrossingIds completed with canonical Quests/Locations/Items/Flags/
   Npcs/Knowledge/Mutations/Endings/Encounters/Crises groups (single source of truth in core);
   removed orphaned Phantom Memory block appended outside Main.cs class; wired phantom buttons
   + PhantomMemorySaveStore; ISeededRng.NextDouble added (interface + both implementations).
C. Null safety: GetX call sites checked (all guarded). No new findings.
D. Host lifecycle: widgets QueueFree children on rebuild; modals expose events only. Clean.
Verify: build 0 warnings, tests 333/333, data-integrity + expansions (236/236) + bridge PASS.

## Loop 2 — 4 sweeps (1 fix, verified green)
A. Reflection: only SaveChecksum's intentional field reflection in core; no dynamic
   Invoke/GetMethod surface in _Game. Clean.
B. Event leaks: EventBus.Subscribe in GameBootstrap.InitLate.Radio.cs untracked ->
   `_subscriptions.Track(Unsubscribe)` added (matches bootstrap pattern); all other
   EventBus sites now pair with Unsubscribe/_subscriptions.
C. Unity DTO null-deref chains (raw [0] indexing, FromJson chains): none.
D. Exceptions: no empty catches (LogRotationManager catches log IO only); 0 build warnings.
Verify: build 0 errors, tests 333/333.

## Loop 3 — 4 sweeps (1 fix, verified green)
A. Tests quality: no no-op asserts, no Thread.Sleep/Delay, no empty theories;
   RegisterQuestline_NullIsSafe strengthened (nulls don't corrupt catalog,
   valid registration + lookup round-trip asserted).
B. Data edge cases: no NaN/Infinity/negative quantities/empty values in catalogs.
C. Core math: no unguarded division by collection counts (15 guarded sites).
D. Verify: build 0 warnings, tests 333/333.

## Loop 4 — 4 sweeps (0 findings)
A. Culture: no float/double Parse or interpolated ToString without InvariantCulture in
   save paths; SaveChecksum verified G9/G17 + field-name-sorted + length-prefixed.
B. Serializer round-trip: SaveChecksumTests (11 facts) covers JsonUtility<->SystemTextJson parity.
C. Host edge cases: PhantomMemorySaveStore thin pattern, try/catch + null guards.
D. No raw dictionary indexer reads without TryGetValue/ContainsKey in core (37 guarded sites).

## Loop 5 — 4 sweeps (0 findings)
A. Enum casts: Greenhouse stage casts are constant-safe; unknown restore values fail closed.
B. Shim honesty: BridgeGap throws on semantic gaps by default; all 17 gap sites intentional.
C. Test coverage: all 27 test files healthy, no empty theory data.
D. Host wiring: phantom/dose handlers null-guarded via SetupPhantom.

## Loop 6 — 4 sweeps (0 findings, convergence)
A. All 10 HeadlessDemos wired in Godot host and PASS: brine 21/21, cluster 19/19, endings 11/11,
   holdfast 25/25, ledger-debt 27/27, census 31/31, caravan 16/16, + crossing/arbitration/greenhouse/ice-road.
B. Gate warnings audited: all 311 are the single benign "id defined in multiple files" category
   (flag/mutation id reuse across catalogs by design); 0 unresolved/missing references.
C. Loop convergence: 3 consecutive zero-finding loops (4, 5, 6).
D. Full verify: build 0 warnings, tests 333/333, all selftests PASS.

## EXPANSION 06 SPRINT — Phase 0-1 (The Muster, foundations + gate + founding catalog)
- Core: `Assets/Ashfall.Core/Muster/` — QuestApproach enum + IApproachQuestline interface
  (formalized §XIII pattern), MusterSystem (Day-180+ escalation orchestrator: questline
  registry, validated per-questline approach selection, Day-260 trigger, ending-key
  resolution, fresh-copy ordinal CaptureState, deep-copy RestoreState, events).
- Founding catalog: quest_the_muster_uprising (4 strategies: A amnesty / B standing
  ground / C nobody stays / D blood price) + quest_the_rate_card_war (4 approaches per
  §II: A undercut / B audit / C seize / D broker) + quest_the_unsigned_order (forkless).
- Data: currents.json gains faction_hydro_barons (15th current, §II template fields,
  is_active false until NPC wiring); year_of_ash_quests.json registers the 3 quest ids
  (restored container shape after a script-format slip, caught by the loader test);
  year_of_ash_items.json gains item_hydro_baron_queue_chit (native YOA schema).
- Wiring: `--muster-selftest` CLI flag; MusterHeadlessDemo PASS 13/13; 13 unit tests.
- Cross-tool QA (rule §Prompt 26): independent review agent found + drove fixes for
  missing rate-card approaches B/D, wrong item schema/file, §II template deviations on
  the currents entry, unregistered quest ids, event/Enum.Parse minors — all fixed.
- Verify: build 0 warnings, tests 346/346, muster 13/13, gate 0 errors/312 info,
  expansions 236/236 GREEN.

## EXPANSION 06 SPRINT — Phase 2 (Godot host wiring + presentation)
- Core: CurrentsCatalogLoader (engine-agnostic, IFileIO/IJsonSerializer ports) reads
  currents.json -> 15 CurrentDefinition entries; 4 unit tests (fifteen found, hydro-barons
  template fields, dormant majority, missing-dir empty).
- Host: MusterHostSession (day escalation from sector clock, approach selection, events,
  save/restore) + MusterSaveStore (user://, thin pattern); both exit paths persist.
- Presentation: CurrentsRosterWidget (15-current panel, escalation banner) +
  ApproachSelectionModal (generic data-driven approach fork, reused per questline).
- Main.cs: 3 menu buttons, SetupMuster on demand, rate-card modal flow, day-260 escalate.
- Smoke: --muster-uitest headless (roster>=15, modal, escalate-to-300, select A, ending
  key) PASS; save cleaned up after smoke to keep dev saves pristine.
- Verify: build 0 warnings, tests 350/350, muster 13/13, uitest PASS x2 (deterministic),
  gate 0 errors, expansions 236/236 GREEN.

## EXPANSION 06 SPRINT — Phase 3 (auto-escalation, Section V activation, camp widget)
- Core: MusterSystem founding catalog grows 3 -> 8 questlines (six Section V currents:
  cold count A/B, provisioned forkless, long walk A/B, scavenger guild A/B, iron raiders
  forkless; cold count ending keys feed the measured-truth matrix pair). IsCurrentWired()
  added. CoalitionCampSystem (Section VI.2/VI.4): forms only at Day 260+, rally, single
  strategy with per-approach effects (B +15 lockout, C -10 +3 dispersed, D zeroes
  lockout/members and loses Vask), clamped save/load, snapshot isolation.
- Data: six Section V currents flipped is_active (now 9 active / 6 dormant; hydro-barons
  stays false per Section II template); 5 quest ids registered (year_of_ash_quests.json
  now 32).
- Host: MusterHostSession gains Camp + combined MusterHostSave envelope; tick path
  auto-escalates (OnTickYearOfAshClicked -> AutoEscalateMuster); 2 strategy buttons +
  rally button; DeserterCoalitionCampWidget (members/strategy/lockout/Vask panel).
- Debug pass (post-implementation): fixed camp Form missing the Day-260 gate; fixed
  stale dormant-count test; fixed wrong demo assertion (hydro-barons IS wired — dormancy
  is data-side only); extended uitest to cover camp+strategy.
- Verify: build 0 warnings, tests 360/360, muster 23/23, uitest PASS x2, expansions
  236/236, journal + bridge + all 15 demos PASS, gate 0 errors.

## EXPANSION 06 SPRINT — Phase 4 (content + witnesses + epilogue matrix) — FINISHED
- Data (§XIII finish line): 8 Section IX items (57 total), 6 Section VIII locations
  spec-exact (66 total), 9 Section XI world-history entries (74 total, canon day/trigger/
  location values per bible), Harven succession radio broadcast (day 240), 8 Muster
  questlines with real 3-stage content (32 total), muster_witnesses.json (3 accounts),
  muster_epilogues.json (9 outcomes: 8 matrix keys + the_measured_truth).
- Core: WitnessCatalogLoader + EpilogueMatrixLoader (ports, engine-agnostic);
  JournalVoice extended with the 9 Muster knowledge keys, bias-weighted framings for
  all 8 traits (Empath does not write the dark accounts; Sociopath records them as
  transactions); MusterSystem.EndingKeyForAny.
- Host: JournalWitnessPanel — framing keyed to the RECORDING survivor's RiskBiasTrait
  (Section III mechanic), author-bias cycle button; epilogue-matrix viewer (resolved vs
  open outcomes + prose); session loads witnesses/epilogues; tick path refreshes panel.
- Debug + cross-tool QA fixes: syntax/type errors; case-sensitive assert; radio canon
  day 238 -> 240; AMMEND typo; world-history triggers/locations aligned to bible;
  locations displayName/description spec-exact (The Cut etc.); author_bias removed from
  witness data (survivor-keyed framing replaces it); explicit Sociopath cases.
- Tests: +9 (witness load, matrix coverage incl. every catalog ending key, bias
  weighting, sociopath transaction, radio parse, quest stages, EndingKeyForAny).
- Verify: build 0 warnings, tests 369/369, muster 25/25, uitest PASS x2, expansions
  236/236, all 17 selftests PASS, gate 0 errors / 315 info across 58 catalogs.

## EXPANSION 07 SPRINT — THE DOSE (missing slices) — FINISHED
- Audit vs docs/expansions/expansion_07_the_dose_IMPLEMENTATION.md: 4 core systems +
  codec + host session/store + selftest existed; MISSING: dose_registers.json (A4),
  tests for 3 of 4 systems (A5), the Dose Register surface (B/C).
- Data: dose_registers.json (4 bands / 3 plans / 3 guesses / 4 registers / the four
  antagonists with dispositions + one-button actions); the 4 NPCs registered in
  characters.json with real master-list location ids.
- Core: DoseRegistersCatalogLoader (engine-agnostic, BandLabel); CaptureState
  rewritten in SickListSystem/CohortSystem/VoluntaryRegisterSystem (fresh deep copy +
  ordinal — all three returned the LIVE state); fixed SickListSystem.Diagnose never
  adding new diagnoses to _state.bands (silent save loss).
- Host: session loads the catalog; DoseRegisterSurface (one folder, four tabs,
  chaired rows, diegetic one-button actions Book/Name/Assign/Sign, flux-§ marker,
  catalog labels); --dose-uitest smoke.
- Debug + cross-tool QA fixes: my tests used the wrong guess vocab (core validates
  low/medium/high per A1); review found VoluntaryRegisterSystem.Volunteer divergence
  (double-sign same task diverged live vs saved state) — refused + 2 regression
  tests; § marker now per-reading fluxAmbiguous; plan/guess rendered via labels.
- Tests: +36 (3 system suites + catalog + characters registration + double-sign).
- Verify: build 0 warnings, tests 405/405, dose selftest + uitest PASS x2, expansions
  236/236, all 19 selftests PASS, gate 0 errors / 319 info across 59 catalogs.

## LOOP 7 — COLD REBUILD + 4 sweeps (1 fix, verified green)
- Cold rebuild: rm -rf .godot/mono/temp + Ashfall.Core bin/obj -> build 0 errors,
  tests 405/405, expansions 236/236 GREEN.
- Sweep A (exhaustive CaptureState audit, 31 systems): zero live-state returns and
  zero live mutations on capture; 1 determinism find — KnowledgeBase.CaptureState
  emitted HashSet order into the save array -> ordinal-sorted (checksum stability).
- Sweep B (event hygiene): DoseRegisterSurface.BindSession unbinds before bind,
  _ExitTree unsubscribes; Muster widgets never subscribe; Main subscriptions are
  single-shot via setup guards. Clean.
- Sweep C (gate triage): all 319 warnings are the single benign cross-catalog id
  reuse category; 0 unresolved/missing references.
- Sweep D (smoke determinism): muster + dose uitests PASS 5/5 consecutive; no
  stray user:// saves left (muster cleans its own).
- Verify: build 0 warnings, tests 405/405, all 18 selftests PASS, uitests x5,
  gate 0 errors.

## NOBODY'S CHARTER — Phase 6 closing slice (world-history second paragraphs)
- Audit vs pipeline: Phases 1-5 ids/data all present (charter quests, item, mutation,
  10 enc_nc_ encounters, 5 crises in crossing_encounters.json + CrossingCatalog);
  Phase 6's "world_history second paragraph at records_room/weighbridge" was MISSING.
- Data: 5 ending entries added to world_history.json (79 total) — ending_crossing_scale
  / _underwrite / _compact / _none / _walked with the bible's house-voice prose,
  discoverable at loc_crossing_records_room or loc_crossing_weighbridge,
  discovery_trigger ending_reached.
- Tests: CrossingEndingsWorldHistoryTests (master-list parity, discovery locations,
  prose match, trigger).
- Hardening: docs/CI.md gains the Godot cold-build trust-signal section (commands +
  current counts) per Phase 7.
- Gate improved: 275 findings (was 319) — 0 errors, all warnings benign.
- Verify: build 0 warnings, tests 408/408, crossing 33/33, arbitration 58/58,
  endings 11/11, expansions 236/236, muster 25/25, dose PASS, gate 0 errors.

## PARITY AUDIT (Unity-identifies-X vs Godot-binding) — schema alignment sweep
- Root cause class: Unity's JsonUtility binds JSON keys to DTO fields (snake_case
  loaders), while Godot's SystemTextJsonSerializer (case-insensitive only, no
  snake_case policy) left camelCase DTO fields dead or the whole container throwing.
- FIXED 1 — YearOfAsh loader DTOs were written against an imagined schema:
  items (name/category/weightKg), events (description/day/hazardType/temperatureDeltaC),
  locations (sector/riskLevel/radiationUsv), radio (dayTrigger/message/signalStrength
  string/source), survivors (moralAlignment/age/healthPercent/radiationDoseMsv/guilt/
  backstory/confession/factionAffinity/traits) — all aligned to the real JSON.
- FIXED 2 — RadioBroadcastTerminal rendered BLANK broadcasts (called dead
  callSign/bodyText/minDay): now source/message/dayTrigger; signalStrength is a
  STRING ("S7") in the file — a float field made the whole container throw (0 radios).
- FIXED 3 — dose_registers.json threshold_msv/action_label never bound (camelCase
  DTOs): renamed to snake_case per loader convention; BandThresholdsBind + NPC
  action_label binding tests added.
- Binding regression gates added: every YOA item/event/location/radio/survivor now
  asserts the real fields populate (would fail instantly if a field ever unbinds).
- Verified all other loaders match their files: Holdfast (snake_case ✓), Crossing
  (snake_case ✓), DoorEncounters (camelCase ✓), Phantom triggers (snake_case ✓),
  Currents/Witness/Epilogue (snake_case ✓).
- Verify: build 0 warnings, tests 411/411, all selftests + uitests PASS, gate 0
  errors / 275 info across 59 catalogs.

## MIGRATION SPRINT — ENCOUNTERS PORT (expedition core into Ashfall.Core)
- Ported the Unity ExpeditionSystem's travel/looting/inbound mechanics 1:1 into
  Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs: phases Outbound->Looting->
  Inbound->Completed/Failed, stances (Speed 1.5x per Mathf.RoundToInt parity),
  push-luck extension + auto-retreat at 3 looting ticks, loot chance
  0.5+danger*0.05 with capacity cap, stamina drain + encumbrance penalty with
  collapse failure, encounter rolls on EVERY leg, unique expeditionId,
  ISeededRng per tick (no RNG in save), fresh-copy ordinal CaptureState,
  clamped deep-copy RestoreState, events on every mutation.
- Host: ExpeditionHostSession (demo defs + tick/push/retreat actions),
  ExpeditionSaveStore (user://), --expedition-selftest (10/10).
- Cross-tool review required fixes: B1 encounter rolls on outbound/inbound too
  (Unity parity), B2 unique expeditionId (was == locationId), B3 Retreat must
  raise OnStateChanged; all fixed with 3 regression tests.
- Debug pass: test expectation bugs vs Unity rounding (RoundToInt(1.5)=2),
  registry-driven stamina drain, snapshot index-order artifact, demo rng
  fragility (danger 10 -> guaranteed loot).
- Documented deviations (migration doc): night-scavenge +0.1, bicycle +0.5,
  stamina-0 immediate fail, flashlight stored-unread, save shape differs from
  Unity (adoption step pending).
- Verify: build 0 warnings, tests 428/428, expedition 10/10, all 19 selftests +
  both uitests PASS, gate 0 errors / 275 info.

## MIGRATION SPRINT — MEDICAL PORT (Chemical Dependency into Ashfall.Core)
- Ported the Unity ChemicalDependencySystem 1:1 (constants, severity map,
  consumption threshold parity, managed detox 96h success threshold, cold
  turkey 72h, tremor/morale penalties as host-effect EVENTS on survivor ids).
- Fixed a latent UNITY BUG while porting: Unity used detoxProgressHours < 0 as
  the cold-turkey sentinel, so withdrawal fell out of the cold-turkey branch
  after one tick and the 72h completion could never fire. The port adds an
  explicit inColdTurkey flag (with legacy progress<0 conversion on Restore)
  and the 72h withdrawal actually completes; documented in migration notes.
- Host: MedicalHostSession (applies effect events as host totals) +
  MedicalSaveStore (user://) + --medical-selftest (15/15).
- Cross-tool review: found BeginManagedDetox never cleared inColdTurkey (a
  survivor switching cold-turkey -> managed stayed in cold-turkey forever);
  fixed + program-switch regression test.
- Tests: 14 (consumption parity, threshold refusal, detox completion, cold
  turkey penalties + 72h completion, decay, severity scaling, program switch,
  snapshot isolation, ordinal order, round-trip, checksum).
- Also committed the leftover engine-agnostic
  Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs (verified 1:1 vs
  the Unity original, zero engine refs).
- Verify: build 0 warnings, tests 454/454, medical 15/15, all 20 selftests +
  both uitests PASS, gate 0 errors / 275 info across 59 catalogs.














## MIGRATION SPRINT — NARRATIVE PORT (encounter core + catalog)
- Core (Assets/Ashfall.Core/Narrative): EncounterDefinition/EncounterChoice +
  NarrativeEncounterSystem — Unity EncounterSO selection math 1:1 (stance
  multipliers stealth x0.5 / speed x1.5, danger/location filters), weighted
  selection via ISeededRng, resolution history with morale/guilt, fresh-copy
  ordinal CaptureState, deep RestoreState; loader for narrative_encounters.json
  (the three Unity factories as data).
- Host: NarrativeHostSession + NarrativeSaveStore (user://) +
  --narrative-selftest (10/10).
- Also committed pre-existing engine-agnostic WIP (verified): Radiation core +
  tests, Shelter/MaterialShieldingSystem, Survivors/NeedsSystem, MathfCompat.
- Verify: build 0 warnings, tests 488/488 x4, all 21 selftests + uitests PASS,
  gate 0 errors / 276 info across 60 catalogs.

## MIGRATION SPRINT — SURVIVORS PORT (roster core + catalog)
- Core (Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs): SurvivorDefinition +
  SurvivorRosterSystem — catalog-driven joins, death with reasons, events,
  fresh-copy ordinal CaptureState, deep RestoreState; SurvivorCatalogLoader
  reads survivors.json (102 entries, binding-verified).
- Host: SurvivorsHostSession extended — Roster system wired into AddSurvivor,
  LoadCatalog(dataDir); --survivors-selftest (14/14) routed correctly after
  removing a pre-existing alias that shadowed it; --survivors-uitest PASS.
- Debug + hardening: fixed CS0136 shadowing in the newly-appeared Crafting WIP;
  intermittent full-suite failures under parallel class execution (Crafting/
  Radiation WIP tests pass isolated) eliminated via
  [assembly: CollectionBehavior(DisableTestParallelization = true)] —
  suite 509/509 x5 deterministic.
- Tests: 12 roster tests + catalog binding/parity.
- Verify: build 0 warnings, tests 509/509 x5, survivors 14/14 + uitest PASS,
  all 21 selftests + uitests PASS, gate 0 errors / 276 info across 60 catalogs.

## ECONOMY DEBUG LOOP (Phase 4) — protocol cycles
- Baseline hash (cross-process): ed4927fb84140b73b2c45c7a16dd2db0 (economy selftest
  output, pre-loop). Hash CHANGES to bca960f47d9f7646391e708cc1149bef after cycle 2
  because the selftest gained save-integrity PASS lines — simulation untouched;
  justified surface growth, noted per protocol.
- Cycle 01 — Probe: truncated save payload. Defect: NO (probe expectation wrong).
  Repro: Deserialize("{\"version\":1,\"demand\":[{\"itemId\":\"g") throws JsonException.
  Root cause: established serializer contract = throw-on-malformed, callers catch
  (host store already returns null). Fix: probe corrected to assert the actual
  contract. Files: Ashfall.Core.Tests/EconomyProbeTests.cs. Gates: 11/11 probes,
  selftest 11/11.
- Cycle 02 — Probe: checksum mismatch (probe list). Defect: YES (host store).
  Repro: tamper tickCount in economy_save.json -> loads silently.
  Root cause: EconomySaveStore serialized the bare MarketState with no integrity
  envelope (sibling stores verify checksums).
  Fix: host-side EconomySaveEnvelope { Checksum, State }; TryLoad recomputes and
  refuses mismatches; path-overloads added for slot testing.
  Regression: selftest save-integrity block (write -> tamper -> refuse).
  Files: src/Host/EconomySaveStore.cs, src/Host/HostCli.cs.
  Gates: 558/558 tests, economy selftest 11/11 + integrity PASS, hash bca960f4 (both runs).
- Cycle 03 — Probe: reload continuity through the REAL save slot (mid-sequence
  save -> reload -> continue). Defect: NO. 40-day uninterrupted run vs 20+save+
  20 resumed: checksums identical. Files: src/Host/HostCli.cs (probe). GREEN.
- Cycle 04 — Probe: UI path (missing-icon fallback, open/close leak). Defect: NO.
  EconomyMarketPanel builds; node-count leak meter flat across repeated
  refreshes; 1 fallback icon exercised; ECONOMY_UITEST PASS.
- Cycle 05 — Probe: seed fuzz with interleaved buy/sell/barter (100 seeds x 200
  ticks). Defect: NO. No exceptions, no NaN/Inf, all ledger totals finite.
- Cycle 06 — Probe: UI smoke determinism. Defect: NO. ECONOMY_UITEST PASS 5/5;
  no stray user:// saves.
- Cycle 07 — Probe: cross-process determinism. Defect: NO. Two processes hash
  identical (29591ce1, selftest surface grew with continuity probe - noted).
- Cycle 08 — Probe: legacy bare save (pre-checksum shape) must migrate.
  Defect: YES (host store). Repro: bare MarketState JSON on the slot -> TryLoad
  returned null (state silently dropped on upgrade). Root cause: TryLoad only
  parsed the envelope shape. Fix: legacy fallback parse (bare MarketState with
  systemId) accepted with one-time warn; tamper gate unchanged for envelopes.
  Regression: selftest legacy-save block. Files: src/Host/EconomySaveStore.cs,
  src/Host/HostCli.cs. Gates: 559/559, selftest 11/11 + integrity/legacy/
  continuity PASS, uitest PASS.
