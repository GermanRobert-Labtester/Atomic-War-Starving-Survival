# ASHFALL — Host CLI Command Catalog

**Last Verified:** 2026-09-25<br>
**Total Registered Actions:** 272 entries / 417 flag tokens (aliases included)

> **GENERATED FILE — do not edit by hand.**
> Source of truth: the live `godot --headless --path . -- --host-help`
> output (`HostCli.PrintHelp` in `src/Host/HostCli.cs` and its partials).
> Owning runner code for each verb lives under `src/` (grep the flag name).
>
> Regenerate: `bash scripts/ci/generate-cli-catalog.sh`
> Drift gate: `bash scripts/ci/generate-cli-catalog.sh --check` (fails on drift)

| Primary Flag | Aliases | Description |
|---|---|---|
| `--7-day-smoke-selftest` | `--seven-day-smoke-selftest`, `--deterministic-smoke-selftest`, `--deterministic-smoke-run` | 7-day deterministic smoke run: map discovery + weather rolls + survivor needs drift + mid-run save/reload round-trip across 10 verification gates |
| `--accessibility-selftest` | `--ui-accessibility-selftest`, `--ui-access-selftest` | Verify focus order, non-empty labels, modal close handling, and accessibility compliance across UI panels |
| `--asset-coverage-report` | — | Full non-gating sweep of every catalog id (core + expansions) vs loadable art; prints per-category coverage and the missing list |
| `--asset-registry-selftest` | — | Verify that catalog IDs (items/survivors/locations) resolve to actual texture assets under assets/ |
| `--starting-cohort-lifecycle-selftest` | `--cohort-lifecycle-selftest` | Plan 138 fresh-vs-restore lifecycle: preserve old slots, apply an alternate cohort, honor an empty saved roster, and reject failed restores without reseeding |
| `--starting-supplies-profile` | — | <id> Preselect an authored starting-store profile in the New Game selector; invalid IDs fall back to Standard Holdfast |
| `--bridge-selftest` | — | Report UnityEngine shim removal (shim is gone; always exits 0) |
| `--power-grid-catalog-selftest` | — | Verify power_grid.json loads at runtime via the Core loader, canonical room IDs resolve (room_water_pump/room_workshop), and fluid power derivation is nominal |
| `--core-selftest` | — | Ice road + census headless demos |
| `--data-integrity-selftest` | — | Cross-reference every id in the 129 StreamingAssets catalogs (recipe→item, quest→location, events, door encounters, survivors, factions, ranges, duplicates) |
| `--difficulty-selftest` | — | XP-01 difficulty catalog, scalar consumers, starting bonuses, fail-closed selection, and save checksum binding |
| `--export-parity-selftest` | — | [--parity-target <dir>] Packaged-data parity: exported build's catalogs byte-identical + parseable vs the data authority, exact Linux casing, no LFS pointers, ELF exe + PCK present |
| `--catalog-boot-preflight` | — | Machine-readable preflight: checks all catalogs are present, well-formed, and reports classification (required/optional/dev-only) with any load errors |
| `--panel-bind-lifecycle-selftest` | `--panel-bind-selftest`, `--panel-lifecycle-selftest` | Real Godot-node callback tests for panel bind → unbind → rebind, event propagation, and session-switch |
| `--port-contract-selftest` | `--port-contracts-selftest` | Validate all Core integration seams and host subsystem wiring contracts against port_contract_policy.json (Plan 36) |
| `--save-load-ui-failure-selftest` | `--save-load-failure-selftest`, `--save-load-failure-uitest`, `--save-load-selftest` | Save/load UI failure-path smoke test: missing, corrupt, and checksum-invalid saves show recoverable user messages and leave live session intact |
| `--save-store-checksum-selftest` | `--save-store-checksums-selftest`, `--checksum-sweep-selftest` | Source-scan all SaveStore files for checksum coverage + 5 in-memory round-trip probes (Weather, Map, Survivors, SaveChecksum stability, null-field guard) |
| `--runtime-scale-selftest` | `--runtime-scale`, `--performance-selftest`, `--perf-selftest` | Performance budget validation: 30/180/360-day campaign workloads, day-advance latency, save/load/checksum, allocations, retained memory, and lifecycle leak tests; writes artifacts/runtime-scale-results.json |
| `--scene-binding-selftest` | `--scene-bindings-selftest` | Headless-instantiate every registered production scene and validate each unique_name_in_owner binding contract (Ticket #125 scene-ownership gate); exits 0 when all required nodes resolve with the expected Godot types |
| `--content-utilization-selftest` | `--content-utilization` | Scan every JSON catalog under StreamingAssets/Data, classify each by reachable consumer (gameplay / UI / codex / orphan), write artifacts/content-utilization.{json,md}, and run the CI gate against artifacts/content-utilization-baseline.json (Ticket #127 content-runtime gate) |
| `--standalone-selftest` | — | SkyLayerArmor, VigilStateMachine, GenerationalSuccession, EpilogueMatrix, DiveInstance |
| `--campaign-fuzz-selftest` | — | Core-level campaign fuzz harness gate (Task #129); delegates to Ashfall.Core.Tests.CampaignFuzz suite |
| `--composition-root-selftest` | — | Composition root architecture gate: verifies ComposeCampaign() is the single entry point (Task #131) |
| `--real-campaign-journey-selftest` | `--campaign-journey-selftest`, `--real-main-journey-selftest` | Real Main-composed player journey: New Game -> ComposeCampaign() -> real gameplay action -> real day advance through the coordinator -> SaveAll -> full in-memory reset -> Continue -> restored composed state (Plan #5) |
| `--rail-track-maintenance-selftest` | `--iron-road-selftest` | Expansion 25 Iron Road: gauge stability, track/bridge wear, dispatch feasibility advisory, workgang repair, and the per-segment maintenance ledger |
| `--glassworks-selftest` | `--the-glass-selftest` | Expansion 29 The Glass: vitrification batch annealing, purity tiers, corrective lens grinding, theodolite calibration, and vision prescriptions |
| `--broadsheet-press-selftest` | `--the-press-selftest` | Expansion 30 The Press: movable-type wear and reset, ink and paper consumables, print runs by publication kind, audience reach and morale stabilization, rumor debunk correction, and the bound archive of what the shelter printed |
| `--kilnworks-selftest` | `--the-kiln-selftest` | Expansion 31 The Kiln: batch firing stages, thermal shock, draw grades, lime calcination yield, refractory lining wear and reline, kiln fuel reserve, and fired-output tallies |
| `--agriculture-selftest` | — | Agriculture Expansion (Plan 162): crop strain catalog, greenhouse growth, mutation RNG, compost, nutrition |
| `--orphan-seal-wave1-selftest` | — | ORPHAN-SEAL-PRIORITY-W1: ten priority orphan authorities — catalog, command, state round-trip |
| `--commitments-selftest` | — | Plan 38 commitments & deadlines: catalog, warning ladder, exactly-once miss + consequence routing, met settlement, save round-trip |
| `--session-durability-selftest` | — | Plan 39 session durability: slot capacity/isolation, interrupted-write + backup recovery audit, soak stability verdicts, capture round-trip |
| `--playable-metrics-selftest` | — | Plan 46 playable metrics: bounded recorder stream, first-hour funnel, aggregation grades, capture round-trip |
| `--survivor-voice-selftest` | — | Plan 42 survivor voice: catalog selection, cooldowns, dispatch arbitration, barrel history, capture round-trip |
| `--content-certification-selftest` | — | Plan 49 content orphan certification: family manifest, live-evidence rows, clean/dormant/orphan verdicts |
| `--holdfast-presentation-selftest` | — | Plan 51 holdfast presentation slate: room/actor/map projections, hazard + crisis bands, motion profile |
| `--scarcity-audio-selftest` | — | Plan 52 scarcity audio: weather->bed/cue authority mapping, silence states, ducking, geiger bands |
| `--seven-day-slice-selftest` | — | Plan 54 seven-day slice: authored beats, frozen scenario hash, beat verification + scorecard |
| `--retention-selftest` | — | Plan 55 retention & save budgeting: authored policy overlay, bounded canonical collections, protected obligations, capture round-trip |
| `--outpost-settlement-selftest` | `--outposts-selftest` | Plan 58 outposts & second holdfast: authored catalog, establish/garrison/supply lifecycle, daily consume, capture round-trip |
| `--territory-control-selftest` | `--territory-selftest` | Plan 134 faction territory & supply line control: contested nodes, fortification, garrison, supply line status, capture round-trip |
| `--cooking-selftest` | `--cooking-test` | Plan 136 wildlife trapping food pipeline & cooking system: recipe loading, ingredient consumption, decontamination, skill progression, capture/restore |
| `--aquaponics-selftest` | — | Plan B87 closed-loop aquaponics: catalog, growth, power/DO crash, harvest, nutrient export, save round-trip |
| `--arbitration-selftest` | — | CrossingArbitrationHeadlessDemo |
| `--black-flotilla-selftest` | `--maritime-selftest`, `--expansion-09-selftest` | The Black Flotilla (Exp 09): catalog load, deterministic scavenge, dive rooms/air/noise, contamination, visit state, save round-trip |
| `--brine-selftest` | `--salt-steam-selftest` | BrineWaterHeadlessDemo (S2 salt & steam) |
| `--census-selftest` | — | CensusHeadlessDemo |
| `--cluster-selftest` | `--order-12c-selftest` | Cluster12CHeadlessDemo (S3 order 12-C + quest snapshot) |
| `--combat-breaching-selftest` | — | Plan B86 combat breaching: catalog, quiet/loud clearance, vehicle gate, mid-breach save fields |
| `--combat-selftest` | — | Combat Expansion: catalog (JSON), ballistics, weapon condition, determinism, save round-trip |
| `--crossing-selftest` | — | CrossingHeadlessDemo (Exp 04) |
| `--deep-coast-host-selftest` | `--deep-coast-playthrough` | Deep-coast host playthrough: survey → decision → dive → scavenge → save/restore |
| `--deep-coast-selftest` | `--deep-coast-route-selftest` | District 8 deep-coast route: stages, decisions, Ice Road gating, dive handoff, v5 save |
| `--defense-selftest` | — | Shelter Defense Expansion (Plan 163): trap catalog, installation, engagement, alarm, capture handoff |
| `--direction-finding-selftest` | — | Plan B88 HF/DF: catalog, baselines, skywave, fingerprint≠fix, RadioSave V3 triangulation nest |
| `--disease-selftest` | `--disease-expansion-selftest` | Disease Expansion: catalog, quarantine, protocols, determinism, save round-trip |
| `--duty-roster-selftest` | — | DutyRosterHeadlessDemo (Exp 02) |
| `--endings-selftest` | `--shelf-selftest` | EndingsHeadlessDemo (S4 endings exclusive + roundtrip) |
| `--expansions-selftest` | `--all-expansions-selftest` | Run full 7-expansion verification suite (Holdfast, Duty Roster, Standing Record, Crossing, Arbitration, LedgerDebt, Glass Orchard) |
| `--greenhouse-selftest` | `--glass-orchard-selftest` | GreenhouseHeadlessDemo (Exp 05) |
| `--psychology-selftest` | — | Psychology Arc Expansion (Plan 164): breakdown arcs, sustained-stress triggers, catharsis, treatment |
| `--wildlife-selftest` | — | Wildlife Ecosystem (Plan 165): fauna catalog, predation/radiation pressure, apex, taming, save round-trip |
| `--trapping-selftest` | — | Wildlife trapping host path: TrySetTrap billing, broken-trap replacement, atomic failure, trap-recipe identity |
| `--holdfast-briefing` | — | Print location count and every Holdfast quest briefing |
| `--holdfast-selftest` | — | Holdfast S1 survival loop, ice road, and trade verification |
| `--ice-road-selftest` | — | IceRoadHeadlessDemo (Exp 01) |
| `--ice-road-tick-demo` | — | Unlock, clerk, 30 day ticks, print catalog + briefing |
| `--ledger-debt-selftest` | — | LedgerDebtHeadlessDemo |
| `--moral-choice-selftest` | — | Moral choice: catalog + scripted arc + bands + reconcile events + journal hook + save/tamper checks |
| `--evolving-world-selftest` | — | Evolving-world activation: seeds, live weather-fed ticks, migration, expedition consequences, scarcity, save envelope, 360-day scenario |
| `--world-playtest-selftest` | — | Fixed-seed 30-day evolving-world campaign proof: snapshots, downstream reads, bounds, determinism, and midpoint save/load parity |
| `--selftest-manifest` | — | Emit the machine-readable self-test manifest JSON (scripts/ci/generate-selftest-manifest.py) |
| `--test-manifest` | — | Alias for --selftest-manifest |
| `--list-selftests` | — | List every registered selftest and run its signature live (runtime/CLI parity audit) |
| `--list-tests` | — | Alias for --list-selftests |
| `--selftests` | — | Alias for --list-selftests |
| `--list-selftest` | — | Alias for --list-selftests |
| `--muster-selftest` | `--expansion-06-selftest` | MusterHeadlessDemo (Exp 06 the Muster) |
| `--faction-ecology-selftest` | — | Plan 25 faction ecology vertical slice (action board, E-P1 chain, witness, camp scene, muster path) |
| `--phase0-selftest` | — | Phase-0 effects: phantom work-eff/refusal, flashbacks, trade specialty, final-wish buff, respiratory stamina + save roundtrip |
| `--precision-metrology-selftest` | — | Plan B89 precision metrology: grades, registered consumers only, workshop projection, disturbance, save round-trip |
| `--silent-foundry-selftest` | — | Silent Foundry (Exp 10): trade stance, trust momentum, recipes, and save round-trip |
| `--standing-record-selftest` | — | StandingRecordHeadlessDemo (Exp 03) |
| `--verdict-selftest` | `--expansion-08-selftest` | The Verdict (Exp 08): machine log, reckoning phases, evidence, census, save |
| `--warlord-host-selftest` | — | Warlord host playthrough: YearOfAsh wiring, standing, v3 save/tamper |
| `--warlord-selftest` | `--warlord-ai-selftest` | Adaptive warlord AI: doctrines, territory, tribute, determinism, v3 save |
| `--warlord-ui-selftest` | — | Warlord tribute payment loop + collector voice + FactionsPanel card |
| `--world-exploration-selftest` | `--plan11-selftest` | World exploration: deep-strata excavation, cipher hunts, living geography evolution, and location memory |
| `--cartography-selftest` | `--plan16-selftest` | Cartography and infrastructure: 60-node wasteland map, 6 waystations, 4 caravan circuits, 12 accords, and damaged map zones |
| `--expansion-depth-selftest` | `--plan18-selftest` | Expansion deepening: Holdfast (24 quests), Standing Record (52 memories, 22 quests), Crossing (20 quests, 14 encounters), Verdict (16 questlines, 9 NPCs) |
| `--wasteland-inhabitants-selftest` | `--plan20-selftest`, `--inhabitants-selftest` | Wasteland inhabitants: NPC catalog, faction presence, encounter density, and settlement population verification |
| `--audio-selftest` | `--audio-test` | Audio cue catalog, AudioManager wiring, and sound event verification |
| `--caravan-selftest` | `--traveling-caravan-selftest` | Traveling caravan economy, inventory generation, and barter ticks |
| `--chemical-dependency-save-selftest` | — | Chemical dependency system save store round-trip, tolerance, and withdrawal states |
| `--contraband-stash-selftest` | `--contraband-selftest` | Plan 147 contraband stash discovery: day gate, once-only claim, canonical grant, checksummed save round-trip |
| `--dose-ledger-selftest` | — | Dose Ledger save write → reload → restore → checksum/tamper checks |
| `--duty-roster-save-selftest` | — | Duty Roster save write → reload → restore → checksum/tamper checks |
| `--economy-selftest` | — | Run the engine-agnostic economy headless demo (goods load, market ticks, barter, save/load round-trip) |
| `--expansion-hub-save-selftest` | — | Expansion hub save write → reload → restore → checksum/tamper checks |
| `--expedition-encounter-bridge-selftest` | — | ExpeditionEncounterBridge bare-notice + resolved surface smoke test |
| `--expedition-selftest` | — | Expedition domain: sorties, encounter resolution, loot drops, and save round-trip |
| `--expedition-playtest-selftest` | — | Plans 51: deterministic 30-day expedition campaign, vehicle balance ledger, breakdown, wear, and save/resume proof |
| `--synthetic-lubricant-selftest` | — | Plan 118: catalog-backed synthetic lubricant production, catalyst state, atomic feed/output, and consumer registration |
| `--uv-corona-selftest` | — | Plan 119: bounded electrical-fault observations, seeded sensor noise, battery use, and capture/restore |
| `--carbon-composite-selftest` | — | Plan 120: material aging, deterministic cure quality, explicit component projection, and capture/restore |
| `--gpr-cartography-selftest` | — | Plan 121: terrain/mode survey trade-offs, uncertain buried observations, map-lead idempotence, and capture/restore |
| `--advanced-industrial-recon-selftest` | — | Plans 118-121: deterministic 60-day Core industrial/reconnaissance integration with midpoint save/replay proof |
| `--patrol-encounter-selftest` | `--travel-encounter-selftest` | Patrol catalog, cooldown, recognition, resolution, and save/restore lifecycle |
| `--research-catalog-selftest` | — | Research knowledge catalog: load count, DAG validity, and cross-catalog unlock references (Plan 34) |
| `--radio-catalog-selftest` | — | Radio station catalog: JSON authority, schedules, and signal model (AF-B1 / Plan 60) |
| `--holdfast-save-selftest` | — | S1 save write → reload → restore → checksum/tamper checks |
| `--holdfast-trade-save-selftest` | — | Holdfast trade ledger and save store round-trip and tamper checks |
| `--inventory-save-selftest` | — | Inventory system save store round-trip, item serialization, and checksum verification |
| `--starting-supplies-selftest` | `--starting-profile-selftest` | Plan 134 six-profile fresh-inventory matrix, fallback, idempotence, and save bypass |
| `--journal-save-selftest` | — | Journal system save store round-trip, entry ordering, and tamper checks |
| `--journal-selftest` | — | Journal domain + save roundtrip |
| `--journal-weather-panel-selftest` | — | Journal and Weather forecast panel integration and live data binding |
| `--medical-selftest` | — | Medical domain: patient triage, treatment protocols, affliction progression, and save round-trip |
| `--medical-ward-save-selftest` | — | Medical ward save store round-trip, bed allocation, and affliction persistence |
| `--narrative-selftest` | — | Narrative domain: dialog trees, echoes, flags, and story event resolution |
| `--narrative-continuity-selftest` | — | Plan 50: normalize narrative graphs (questline stages, event chains, quest refs), lint dangling refs/reachability/flag set-vs-read/case discipline, write artifacts/narrative-continuity.{json,md} |
| `--npc-arc-selftest` | — | Plan 52 recurring NPC arcs: resolution precedence, encounter→quest memory, save round-trip, distress suppression |
| `--oral-lore-selftest` | — | Oral Lore Codex: load 16 songs/poems from narrative catalogs, verify query by id/tag/genre |
| `--radio-selftest` | — | Radio persistence: history/frequency/played-dedup survive save/load; tamper rejected |
| `--settings-selftest` | `--settings-test` | SettingsManager state, resolution, audio buses, and keybindings save/load |
| `--survivors-selftest` | — | Survivors domain: needs decay, skill progression, trauma, and morale |
| `--utility-ai-selftest` | — | Utility AI decision scoring, survivor behaviors, and action selection |
| `--weather-save-selftest` | — | Weather system save store round-trip, forecast queue, and atmospheric condition persistence |
| `--dynamic-world-selftest` | `--plan19-selftest` | Dynamic world systems: weather forecasting lookahead, station tiers, 6 seasonal phases, 18+ seasonal events, Orbital Harrow kinetic impact templates, sky armor cascades, salvage, and save persistence |
| `--wasteland-inhabitants-selftest` | `--plan20-selftest`, `--inhabitants-selftest` | Wasteland inhabitants: 32-entry field guide (20 fauna + 12 flora), 6 wasteland settlements, 18 named NPCs with standing-reactive greetings, 6 repeatable side-work quests, 24 route-aware travel encounters + 4 multi-stage chains with stance weighting and deterministic RNG |
| `--world-selftest` | — | World domain: map nodes, sector navigation, hazard regions, and landmark states |
| `--year-of-ash-save-selftest` | — | Year of Ash save write → reload → restore → checksum/tamper checks |
| `--dashboard-uitest` | — | Game Dashboard panel UI construction, HUD binding, and metrics display |
| `--day1-selftest` | `--day-1-selftest`, `--day1-playable-selftest` | Day 1 onboarding, needs depletion, and shelter survival verification |
| `--day1-to-day2-selftest` | `--day1-to-day2`, `--day1-to-day2-milestone-selftest` | Day 1 to Day 2 transition, overnight triage, and milestone progression |
| `--dose-uitest` | — | Dose Ledger panel UI construction, radiation tiers, and dose history |
| `--duty-roster-uitest` | — | Duty Roster panel UI construction, role assignments, and shift scheduling |
| `--economy-uitest` | — | Economy market panel UI construction, price shock display, and barter grid |
| `--expedition-panel-uitest` | `--expedition-panel-lifecycle` | Expedition panel encounter-notice lifecycle: open→surface→close→reopen→surface |
| `--onboarding-journey-selftest` | `--onboarding-selftest` | First-hour onboarding journey: water → power → food → research → expedition, with resume after save/load and state-true signals |
| `--mod-selftest` | `--mods-selftest` | Deterministic JSON mod manifest validation, whitelist enforcement, layering, failure isolation, and catalog-integrity staging |
| `--holdfast-runtime-uitest` | `--holdfast-runtime-ui-test`, `--holdfast-runtime-selftest` | Godot Holdfast terminal browse → trade → failed trade → save → reload |
| `--inventory-uitest` | `--inventory-selftest` | Inventory panel UI construction, item grid, and slot binding |
| `--journal-uitest` | — | Build ledger UI, cycle tabs, quit |
| `--muster-uitest` | — | The Muster panel UI construction, faction stance cards, and vote tally |
| `--phase0-uitest` | — | Phase 0 expansion UI preview and workstation panels |
| `--playable-shell-selftest` | `--shell-selftest`, `--playable-loop-selftest` | Playable shell game loop, scene transitions, and day advancement |
| `--player-panels-uitest` | `--player-panels-ui-test` | Bind and render Survivors, Medical, Weather, Radio, Shelter panels |
| `--shelter-hazard-loop-selftest` | `--shelter-hazard-selftest`, `--duty-roster-loop-selftest` | Shelter hazard loop and duty roster assignment verification |
| `--shelter-decor-selftest` | `--shelter-interior-selftest`, `--memorial-wall-selftest` | Live items.json decor, inventory mount/remove, NeedsSystem morale, memorial-wall projection, save, and panel verification |
| `--shelter-operations-selftest` | `--shelter-ops-selftest`, `--operations-selftest` | Medical triage, expedition sorties, radio network, crafting, respiratory afflictions, and the routed shelter operations board |
| `--water-sources-selftest` | — | Deep well, atmospheric condenser, aquifer piezometer, and water-treatment commands plus their snapshots (water sources surface gate) |
| `--silent-foundry-uitest` | — | Silent Foundry trade panel UI construction, binding, and trade loop |
| `--plans198-201-uitest` | `--plans198-201-selftest` | CBRN/comms/ceremony/robotics panels: route, bind, command, state delta, feedback |
| `--decon-airlock-uitest` | — | Decon Airlock UI data grid panel bindings |
| `--workshop-relic-uitest` | `--workshop-relic-selftest` | Workshop dual-bind relic restoration smoke: render, select, repair, deltas, save/reload |
| `--decon-airlock-selftest` | — | Decon Airlock UI data grid panel bindings |
| `--geodetic-survey-uitest` | — | Geodetic Survey UI data grid panel bindings |
| `--geodetic-survey-selftest` | — | Geodetic Survey UI data grid panel bindings |
| `--kinetic-storage-uitest` | — | Kinetic Storage UI data grid panel bindings |
| `--kinetic-storage-selftest` | — | Kinetic Storage UI data grid panel bindings |
| `--chemical-recon-uitest` | — | Chemical Recon UI data grid panel bindings |
| `--chemical-recon-selftest` | — | Chemical Recon UI data grid panel bindings |
| `--recon-telemetry-uitest` | `--recon-telemetry-selftest` | Recon Telemetry UI data grid panel bindings |
| `--geothermal-uitest` | — | Geothermal Aquifer UI panel bindings |
| `--geothermal-aquifer-selftest` | — | Geothermal Aquifer UI panel bindings |
| `--ebpvd-coating-uitest` | — | EB-PVD Thermal Barrier Coating UI panel bindings |
| `--ebpvd-coating-selftest` | — | EB-PVD Thermal Barrier Coating UI panel bindings |
| `--microfluidic-diagnostic-uitest` | — | Microfluidic Diagnostics UI panel bindings |
| `--microfluidic-diagnostic-selftest` | — | Microfluidic Diagnostics UI panel bindings |
| `--mine-flail-uitest` | — | Mine-Clearing Flail UI panel bindings |
| `--mine-flail-selftest` | — | Mine-Clearing Flail UI panel bindings |
| `--rail-grinding-uitest` | — | Rail Grinding Corridor UI panel bindings |
| `--rail-grinding-selftest` | — | Rail Grinding Corridor UI panel bindings |
| `--survivors-uitest` | — | Survivors panel UI construction, roster cards, and affliction badges |
| `--ui-layout-selftest` | `--layout-selftest` | Verify fixed 1920x1080 UI layout bounds, responsive containers, and panel alignments |
| `--ui-snapshot-regenerate` | `--ui-snapshots-regen` | Recapture all snapshot targets and OVERWRITE snapshots/ goldens (needs real display) |
| `--ui-snapshot-uitest` | `--ui-snapshots` | Capture all snapshot targets, DIFF against snapshots/ goldens (needs real display, not --headless) |
| `--utility-ai-uitest` | — | Utility AI debug view, consideration curves, and behavior trees |
| `--verdict-uitest` | — | Build THE MACHINE'S REGISTER panel; assert 13 transmissions render + leak-free |
| `--sofc-power-selftest` | — | Plan 122 solid-oxide fuel cell: catalog, electrochemistry engine, power/water gating, save round-trip |
| `--sound-ranging-selftest` | — | Plan 123 sound ranging: catalog, threat engine, bearing/registration math, determinism |
| `--cvd-diamond-selftest` | — | Plan 124 CVD diamond synthesis: catalog, plasma-phase engine, batch lifecycle, save round-trip |
| `--amphibious-draisine-selftest` | — | Plan 125 amphibious draisine: catalog, crossing engine, cargo/load gates, water crossings |
| `--late-tech-mobility-selftest` | — | Combined Plans 122–125 harness: all four late-tech systems composed through one CLI world |
| `--plans-122-125-selftest` | — | Plans 122–125 aggregate: catalog + engine + wiring + persistence checks for SOFC/sound ranging/CVD diamond/amphibious draisine |
| `--plans-122-125-balance-soak` | — | Plans 122–125 balance soaks: bounded multi-day soak over the four late-tech systems, writes the plan 122–125 balance reports |
| `--insar-selftest` | — | Plan 139 InSAR geodesy: repeat passes, decorrelation, deformation classes, travel/excavation projections |
| `--hydraulic-extrusion-selftest` | — | Plan 140 hydraulic extrusion: phases, defect rolls, tool wear, rejected/premium outcomes |
| `--runflat-tire-selftest` | — | Plan 141 run-flat tires: install gating, hazard reduction, heat/fuel penalty, severe-failure paths |
| `--plans-139-141-selftest` | — | Plans 139–141 aggregate: InSAR + hydraulic extrusion + run-flat wiring, persistence, and panel reachability |
| `--sky-defense-selftest` | — | Flagship Task 7 counter-battery: telemetry track intake, magazine logistics, deterministic volley, service, crew claim, save round-trip, and player-panel construction |
| `--vehicle-garage-selftest` | — | Plan 50 overland vehicle garage: modification install/uninstall, component wear, service, immobilization gate, recovery completion, and expedition-profile decoration |
| `--shelter-physics-selftest` | `--shelter-actor-physics-selftest` | Shelter physics and actor movement selftests: interior traversal, hazard interaction |
| `--propaganda-selftest` | — | Plan 168: Propaganda and morale warfare system, campaigns, broadcasts, save persistence, and UI binding |
| `--rumor-network-selftest` | — | Plan 203: Wasteland information flow, rumors, intelligence gathering, save persistence, and UI binding |
| `--shelter-security-selftest` | — | Plan 138: Shelter defense, security clearance levels, breach alerts, save persistence, and UI binding |
| `--personal-quests-selftest` | — | Plan 200: Survivor personal quests, character arcs, stage progression, save persistence, and UI binding |
| `--time-capsule-selftest` | — | Plan 212: Time capsule & legacy messages system, scheduled opening, save persistence, and UI binding |
| `--internal-communication-selftest` | `--shelter-communications-selftest` | Plan 211: Internal shelter notices, identity refusals, expiry, save persistence, and Shelter Social UI binding |
| `--death-legacy-selftest` | — | Plan 206: Survivor death records, wills, estate inheritance, disputes, save persistence, and UI binding |
| `--relationship-decay-selftest` | — | Plan 182: Relationship decay, social drift, bond maintenance, save persistence, and UI binding |
| `--visitor-integration-selftest` | — | Plan 214: admitted visitor stays, temporary housing, processing requirements, recruitment handoff, and UI binding |
| `--personal-belongings-selftest` | — | Plan 210: survivor keepsake claims, favorites, gifts, loss reporting, inheritance, and UI binding |
| `--research-unlock-selftest` | `--research-unlocks-selftest` | Plan 141 research unlock bridge: catalog load, downstream unlock queries, capability grants, recipe unlocks, and inventory awards |
| `--unified-ending-selftest` | `--epilogue-selftest` | Plan 145 unified ending resolver: epilogue evaluation, personalized chronicle, survivor fates, legacy trait awards, and save round-trip |
| `--npc-memory-selftest` | `--npc-memory-test` | Plan 147 per-NPC memory: trust, grudge, favors owed, forgiveness, dialogue tone, and trade pricing modifiers |
| `--ideological-friction-selftest` | `--ideology-selftest` | Plan 148 ideological friction: confrontations, conversions, bunker factions, and mediation |
| `--romance-family-selftest` | `--romance-selftest` | Plan 150 romance & family dynamics: courtship stages, partnership, bonded pairs, family units, adoption |
| `--vehicle-customization-selftest` | `--vehicle-modules-selftest` | Plan 152 vehicle module slots, effective stats, bunk capacity, and base camps |
| `--user-data-dir` | — | <path> Override user:// base directory for isolated test runs (or set ASHFALL_USER_DIR) |
| `--log-dir` | — | <path> Configure log output directory for headless runs (or set ASHFALL_LOG_DIR) |
| `--dependency-taper-selftest` | `--the-habit-selftest` | Chemical dependency taper and withdrawal integration probe |
| `--antenatal-care-selftest` | `--the-quickening-selftest` | Antenatal and maternal health integration probe |
| `--clinical-ward-selftest` | `--the-ward-selftest` | Clinical ward triage integration probe |
| `--chemical-reagent-selftest` | `--the-reagent-selftest` | Chemical reagent synthesis integration probe |
| `--mechanical-driveline-selftest` | `--the-wheel-selftest` | Mechanical driveline integration probe |
| `--sleep-acoustic-selftest` | `--the-quiet-selftest` | Sleep acoustic rest integration probe |
| `--shelter-archive-selftest` | `--archive-system-selftest` | Shelter archive integration probe |
| `--dream-system-selftest` | `--dreams-selftest` | Dream system integration probe |
| `--accessibility-settings-selftest` | `--accessibility-options-selftest` | Accessibility settings integration probe |
| `--memory-decay-selftest` | `--memory-system-selftest` | Memory decay integration probe |
| `--interpersonal-conflict-selftest` | `--conflict-system-selftest` | Interpersonal conflict integration probe |
| `--exercise-selftest` | `--physical-training-selftest` | Exercise and physical training integration probe |
| `--affliction-bridge-selftest` | `--affliction-bridges-selftest`, `--affliction-quest-work-selftest` | Affliction quest-work bridge probe |
| `--radiation-mutation-selftest` | `--mutation-system-selftest` | Radiation mutation integration probe |
| `--radio-production-selftest` | `--radio-program-production-selftest` | Radio program production probe |
| `--working-animals-selftest` | `--companion-animal-selftest` | Working animals integration probe |
| `--black-market-selftest` | `--underworld-economy-selftest` | Black market integration probe |
| `--culture-creation-selftest` | `--art-culture-selftest` | Culture creation integration probe |
| `--psychological-profile-selftest` | `--phobia-system-selftest`, `--unified-psychology-selftest` | Psychological profile integration probe |
| `--skill-certification-selftest` | `--skill-tier-selftest`, `--certifications-selftest` | Skill certification integration probe |
| `--child-development-selftest` | `--child-stages-selftest` | Child development integration probe |
| `--bestiary-selftest` | `--creature-encounters-selftest`, `--bestiary-ui-selftest` | Bestiary integration probe |
| `--health-history-selftest` | `--medical-records-selftest`, `--vaccination-history-selftest` | Health history integration probe |
| `--leadership-succession-selftest` | `--succession-selftest`, `--leadership-challenges-selftest` | Leadership succession integration probe |
| `--aging-selftest` | `--elderly-survivor-selftest` | Survivor aging and life-stage compatibility probes |
| `--atmosphere-selftest` | `--shelter-atmosphere-selftest` | Shelter atmosphere compatibility probes |
| `--audio-access-selftest` | `--audio-accessibility-selftest` | Audio accessibility compatibility probes |
| `--backstory-selftest` | `--backstories-selftest` | Survivor backstory compatibility probes |
| `--campaign-legacy-selftest` | `--legacy-selftest` | Generational legacy compatibility probes |
| `--communique-board-selftest` | `--faction-communique-board-selftest` | Faction communique compatibility probes |
| `--difficulty-settings-selftest` | `--difficulty-sliders-selftest` | Difficulty configuration compatibility probes |
| `--dynamic-quest-selftest` | `--dynamic-quests-selftest` | Dynamic quest compatibility probes |
| `--governance-selftest` | `--shelter-governance-selftest` | Shelter governance compatibility probes |
| `--hidden-agenda-selftest` | `--hidden-agendas-selftest` | Hidden-agenda compatibility probes |
| `--human-migration-selftest` | `--migration-selftest` | Human migration compatibility probes |
| `--keepsakes-selftest` | — | Personal keepsake compatibility probes |
| `--maintenance-selftest` | `--shelter-maintenance-selftest` | Shelter maintenance compatibility probes |
| `--mechanical-origin-selftest` | `--origin-mechanics-selftest` | Origin mechanics compatibility probes |
| `--meta-progression-selftest` | `--meta-selftest` | Cross-run meta-progression compatibility probes |
| `--mod-contract-selftest` | `--mod-support-selftest` | Mod contract and support compatibility probes |
| `--needs-perf-selftest` | `--needs-performance-selftest` | Needs performance compatibility probes |
| `--personal-quest-selftest` | — | Personal quest compatibility probes |
| `--propaganda-campaign-selftest` | — | Propaganda campaign compatibility probes |
| `--reputation-selftest` | `--shelter-reputation-selftest` | Shelter reputation compatibility probes |
| `--routines-selftest` | `--survivor-routines-selftest` | Survivor routine compatibility probes |
| `--rumors-selftest` | — | Wasteland rumor compatibility probes |
| `--security-selftest` | — | Shelter security compatibility probes |
| `--shelter-identity-selftest` | `--shelter-naming-selftest` | Shelter identity and naming compatibility probes |
| `--shelter-noise-selftest` | — | Shelter noise compatibility probes |
| `--social-drift-selftest` | — | Social drift compatibility probes |
| `--standing-gates-selftest` | — | Standing-gate compatibility probes |
| `--storm-forecast-selftest` | `--the-weather-selftest`, `--weather-cascade-selftest` | Weather forecast and cascade compatibility probes |
| `--survivor-death-selftest` | `--wills-selftest` | Survivor death and will compatibility probes |
| `--the-wild-selftest` | `--wildlife-harvest-selftest` | Wildlife migration and harvest compatibility probes |
| `--time-capsules-selftest` | — | Time-capsule compatibility probes |
| `--trade-route-selftest` | `--trade-routes-selftest` | Trade route compatibility probes |
| `--tunnel-selftest` | `--tunnel-network-selftest` | Tunnel network compatibility probes |
| `--visitors-selftest` | — | Visitor integration compatibility probes |
| `--host-help` | `--help` | This list |
| `--version` | `-v` | Show build, data schema, and save schema versions |
