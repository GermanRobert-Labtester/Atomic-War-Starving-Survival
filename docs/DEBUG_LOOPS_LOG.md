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








