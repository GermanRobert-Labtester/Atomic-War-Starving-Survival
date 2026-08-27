# ASHFALL — Host CLI Command Catalog

**Last Verified:** 2026-08-27
**Total Registered Actions:** 90 entries / 135 flag tokens (aliases included)

> **GENERATED FILE — do not edit by hand.**
> Source of truth: the live `godot --headless --path . -- --host-help`
> output (`HostCli.PrintHelp` in `src/Host/HostCli.cs` and its partials).
> Owning runner code for each verb lives under `src/` (grep the flag name).
>
> Regenerate: `bash scripts/ci/generate-cli-catalog.sh`
> Drift gate: `bash scripts/ci/generate-cli-catalog.sh --check` (fails on drift)

| Primary Flag | Aliases | Description |
|---|---|---|
| `--7-day-smoke-selftest` | `--seven-day-smoke-selftest`, `--deterministic-smoke-selftest` | 7-day deterministic smoke run: map discovery + weather rolls + survivor needs drift + mid-run save/reload round-trip across 10 verification gates |
| `--asset-coverage-report` | — | Full non-gating sweep of every catalog id (core + expansions) vs loadable art; prints per-category coverage and the missing list |
| `--asset-registry-selftest` | — | Verify that catalog IDs (items/survivors/locations) resolve to actual texture assets under assets/ |
| `--bridge-selftest` | — | Report UnityEngine shim removal (shim is gone; always exits 0) |
| `--core-selftest` | — | Ice road + census headless demos |
| `--data-integrity-selftest` | — | Cross-reference every id in the 129 StreamingAssets catalogs (recipe→item, quest→location, events, door encounters, survivors, factions, ranges, duplicates) |
| `--panel-bind-lifecycle-selftest` | `--panel-bind-selftest`, `--panel-lifecycle-selftest` | Real Godot-node callback tests for panel bind → unbind → rebind, event propagation, and session-switch |
| `--save-load-ui-failure-selftest` | `--save-load-failure-selftest`, `--save-load-failure-uitest`, `--save-load-selftest` | Save/load UI failure-path smoke test: missing, corrupt, and checksum-invalid saves show recoverable user messages and leave live session intact |
| `--save-store-checksum-selftest` | `--save-store-checksums-selftest`, `--checksum-sweep-selftest` | Source-scan all SaveStore files for checksum coverage + 5 in-memory round-trip probes (Weather, Map, Survivors, SaveChecksum stability, null-field guard) |
| `--standalone-selftest` | — | SkyLayerArmor, VigilStateMachine, GenerationalSuccession, EpilogueMatrix, DiveInstance |
| `--arbitration-selftest` | — | CrossingArbitrationHeadlessDemo |
| `--black-flotilla-selftest` | `--maritime-selftest`, `--expansion-09-selftest` | The Black Flotilla (Exp 09): catalog load, deterministic scavenge, dive rooms/air/noise, contamination, visit state, save round-trip |
| `--brine-selftest` | `--salt-steam-selftest` | BrineWaterHeadlessDemo (S2 salt & steam) |
| `--census-selftest` | — | CensusHeadlessDemo |
| `--cluster-selftest` | `--order-12c-selftest` | Cluster12CHeadlessDemo (S3 order 12-C + quest snapshot) |
| `--combat-selftest` | — | Combat Expansion: catalog (JSON), ballistics, weapon condition, determinism, save round-trip |
| `--crossing-selftest` | — | CrossingHeadlessDemo (Exp 04) |
| `--deep-coast-host-selftest` | `--deep-coast-playthrough` | Deep-coast host playthrough: survey → decision → dive → scavenge → save/restore |
| `--deep-coast-selftest` | `--deep-coast-route-selftest` | District 8 deep-coast route: stages, decisions, Ice Road gating, dive handoff, v5 save |
| `--disease-selftest` | `--disease-expansion-selftest` | Disease Expansion: catalog, quarantine, protocols, determinism, save round-trip |
| `--duty-roster-selftest` | — | DutyRosterHeadlessDemo (Exp 02) |
| `--endings-selftest` | `--shelf-selftest` | EndingsHeadlessDemo (S4 endings exclusive + roundtrip) |
| `--expansions-selftest` | `--all-expansions-selftest` | Run full 7-expansion verification suite (Holdfast, Duty Roster, Standing Record, Crossing, Arbitration, LedgerDebt, Glass Orchard) |
| `--greenhouse-selftest` | `--glass-orchard-selftest` | GreenhouseHeadlessDemo (Exp 05) |
| `--holdfast-briefing` | — | Print location count and every Holdfast quest briefing |
| `--holdfast-selftest` | — | Holdfast S1 survival loop, ice road, and trade verification |
| `--ice-road-selftest` | — | IceRoadHeadlessDemo (Exp 01) |
| `--ice-road-tick-demo` | — | Unlock, clerk, 30 day ticks, print catalog + briefing |
| `--ledger-debt-selftest` | — | LedgerDebtHeadlessDemo |
| `--moral-choice-selftest` | — | Moral choice: catalog + scripted arc + bands + reconcile events + journal hook + save/tamper checks |
| `--muster-selftest` | `--expansion-06-selftest` | MusterHeadlessDemo (Exp 06 the Muster) |
| `--phase0-selftest` | — | Phase-0 effects: phantom work-eff/refusal, flashbacks, trade specialty, final-wish buff, respiratory stamina + save roundtrip |
| `--silent-foundry-selftest` | — | Silent Foundry (Exp 10): trade stance, trust momentum, recipes, and save round-trip |
| `--standing-record-selftest` | — | StandingRecordHeadlessDemo (Exp 03) |
| `--verdict-selftest` | `--expansion-08-selftest` | The Verdict (Exp 08): machine log, reckoning phases, evidence, census, save |
| `--warlord-host-selftest` | — | Warlord host playthrough: YearOfAsh wiring, standing, v3 save/tamper |
| `--warlord-selftest` | `--warlord-ai-selftest` | Adaptive warlord AI: doctrines, territory, tribute, determinism, v3 save |
| `--warlord-ui-selftest` | — | Warlord tribute payment loop + collector voice + FactionsPanel card |
| `--audio-selftest` | `--audio-test` | Audio cue catalog, AudioManager wiring, and sound event verification |
| `--caravan-selftest` | `--traveling-caravan-selftest` | Traveling caravan economy, inventory generation, and barter ticks |
| `--chemical-dependency-save-selftest` | — | Chemical dependency system save store round-trip, tolerance, and withdrawal states |
| `--dose-ledger-selftest` | — | Dose Ledger save write → reload → restore → checksum/tamper checks |
| `--duty-roster-save-selftest` | — | Duty Roster save write → reload → restore → checksum/tamper checks |
| `--economy-selftest` | — | Run the engine-agnostic economy headless demo (goods load, market ticks, barter, save/load round-trip) |
| `--expansion-hub-save-selftest` | — | Expansion hub save write → reload → restore → checksum/tamper checks |
| `--expedition-encounter-bridge-selftest` | — | ExpeditionEncounterBridge bare-notice + resolved surface smoke test |
| `--expedition-selftest` | — | Expedition domain: sorties, encounter resolution, loot drops, and save round-trip |
| `--holdfast-save-selftest` | — | S1 save write → reload → restore → checksum/tamper checks |
| `--holdfast-trade-save-selftest` | — | Holdfast trade ledger and save store round-trip and tamper checks |
| `--inventory-save-selftest` | — | Inventory system save store round-trip, item serialization, and checksum verification |
| `--journal-save-selftest` | — | Journal system save store round-trip, entry ordering, and tamper checks |
| `--journal-selftest` | — | Journal domain + save roundtrip |
| `--journal-weather-panel-selftest` | — | Journal and Weather forecast panel integration and live data binding |
| `--medical-selftest` | — | Medical domain: patient triage, treatment protocols, affliction progression, and save round-trip |
| `--medical-ward-save-selftest` | — | Medical ward save store round-trip, bed allocation, and affliction persistence |
| `--narrative-selftest` | — | Narrative domain: dialog trees, echoes, flags, and story event resolution |
| `--radio-selftest` | — | Radio persistence: history/frequency/played-dedup survive save/load; tamper rejected |
| `--settings-selftest` | `--settings-test` | SettingsManager state, resolution, audio buses, and keybindings save/load |
| `--survivors-selftest` | — | Survivors domain: needs decay, skill progression, trauma, and morale |
| `--utility-ai-selftest` | — | Utility AI decision scoring, survivor behaviors, and action selection |
| `--weather-save-selftest` | — | Weather system save store round-trip, forecast queue, and atmospheric condition persistence |
| `--world-selftest` | — | World domain: map nodes, sector navigation, hazard regions, and landmark states |
| `--year-of-ash-save-selftest` | — | Year of Ash save write → reload → restore → checksum/tamper checks |
| `--dashboard-uitest` | — | Game Dashboard panel UI construction, HUD binding, and metrics display |
| `--day1-selftest` | `--day-1-selftest`, `--day1-playable-selftest` | Day 1 onboarding, needs depletion, and shelter survival verification |
| `--day1-to-day2-selftest` | `--day1-to-day2`, `--day1-to-day2-milestone-selftest` | Day 1 to Day 2 transition, overnight triage, and milestone progression |
| `--dose-uitest` | — | Dose Ledger panel UI construction, radiation tiers, and dose history |
| `--duty-roster-uitest` | — | Duty Roster panel UI construction, role assignments, and shift scheduling |
| `--economy-uitest` | — | Economy market panel UI construction, price shock display, and barter grid |
| `--expedition-panel-uitest` | `--expedition-panel-lifecycle` | Expedition panel encounter-notice lifecycle: open→surface→close→reopen→surface |
| `--holdfast-runtime-uitest` | `--holdfast-runtime-ui-test`, `--holdfast-runtime-selftest` | Godot Holdfast terminal browse → trade → failed trade → save → reload |
| `--inventory-uitest` | `--inventory-selftest` | Inventory panel UI construction, item grid, and slot binding |
| `--journal-uitest` | — | Build ledger UI, cycle tabs, quit |
| `--muster-uitest` | — | The Muster panel UI construction, faction stance cards, and vote tally |
| `--phase0-uitest` | — | Phase 0 expansion UI preview and workstation panels |
| `--playable-shell-selftest` | `--shell-selftest`, `--playable-loop-selftest` | Playable shell game loop, scene transitions, and day advancement |
| `--player-panels-uitest` | `--player-panels-ui-test` | Bind and render Survivors, Medical, Weather, Radio, Shelter panels |
| `--shelter-hazard-loop-selftest` | `--shelter-hazard-selftest`, `--duty-roster-loop-selftest` | Shelter hazard loop and duty roster assignment verification |
| `--shelter-operations-selftest` | `--shelter-ops-selftest`, `--operations-selftest` | Medical triage, expedition sorties, radio network, crafting, and respiratory affliction verification |
| `--silent-foundry-uitest` | — | Silent Foundry trade panel UI construction, binding, and trade loop |
| `--survivors-uitest` | — | Survivors panel UI construction, roster cards, and affliction badges |
| `--ui-layout-selftest` | `--layout-selftest` | Verify fixed 1920x1080 UI layout bounds, responsive containers, and panel alignments |
| `--ui-snapshot-regenerate` | `--ui-snapshots-regen` | Recapture all snapshot targets and OVERWRITE snapshots/ goldens (needs real display) |
| `--ui-snapshot-uitest` | `--ui-snapshots` | Capture all snapshot targets, DIFF against snapshots/ goldens (needs real display, not --headless) |
| `--utility-ai-uitest` | — | Utility AI debug view, consideration curves, and behavior trees |
| `--verdict-uitest` | — | Build THE MACHINE'S REGISTER panel; assert 13 transmissions render + leak-free |
| `--user-data-dir` | — | <path> Override user:// base directory for isolated test runs (or set ASHFALL_USER_DIR) |
| `--log-dir` | — | <path> Configure log output directory for headless runs (or set ASHFALL_LOG_DIR) |
| `--host-help` | `--help` | This list |
| `--version` | `-v` | Show build, data schema, and save schema versions |
