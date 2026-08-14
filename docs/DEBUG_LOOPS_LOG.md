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





