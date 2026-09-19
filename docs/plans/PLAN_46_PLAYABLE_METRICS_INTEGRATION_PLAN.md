# PLAN 46 — PLAYABLE METRICS INTEGRATION PLAN

> **Package:** `C2[20]` / Plan 46 — *Playable Metrics: Measure the Player, Decide the Difficulty*
> **Census title:** Reproducible Balance Evidence, Local Play Metrics, and Measurement-Driven Difficulty Decisions
> **Source plan (authoritative):** `Next-steps-plans/shipped_to_chat/Plan_46_Playable_Metrics_Balance_Decisions_Player_Telemetry.md` (read in full for this plan)
> **Corpus baseline:** `C-integration-plans/C2_planintegration[20].md`
> **Census status at planning time:** `AUDIT-PENDING` (`docs/plans/UNCLAIMED_CORPUS_CENSUS.md`, row `C2[20]`)
> **Plan type:** planning authority only — this document changes no production code, data, or tests. Execution requires a foreman-signed claim in `WORKTREE_OWNERSHIP.md` before Phase 1 begins.
> **Planned at:** current `Zcode_Branch` HEAD (post-`65357b8a`), 2026-09-19 session state.

---

# 1. Objective

Deliver the measurement layer ASHFALL currently lacks, in three dependency-ordered deliveries, exactly as scoped by the source plan:

1. **46A — Reproducible balance corpus.** Replace the 28 unattributed CSV files in `artifacts/balance/` with an in-repo, scripted, attributed sweep harness: scenarios as checked-in data, every artifact stamped with a run header (git SHA, scenario, seed set, build configuration), a generated summary panel in `docs/balance/README.md`, first-ever numeric difficulty targets in `docs/balance/TARGETS.md`, and an ADR log in `docs/balance/DECISIONS.md`. A nightly drift gate fails CI when an unrelated PR moves a pinned balance baseline beyond a declared tolerance.
2. **46B — Local, private, opt-in player-action metrics.** A `PlaySessionRecorder` in engine-free Core that joins (a) player *action* records (generalising the existing `ObserveSigil` instrumentation, not forking it) with (b) the existing `DayStateChangeEvent` *consequence* stream already captured by Plan 31C's `DayRecord`, written as bounded JSONL under `user://`, **never over any network**, hard-off in release exports, producing a local first-hour funnel / dead-end / stuck-day report via a headless selftest verb.
3. **46C — Close the loop through humans.** A documented, human-reviewed difficulty-tuning ritual: measurements become ADRs citing sweep artifacts; ADRs become foreman-signed tuning changes through the *existing* change process for `difficulty_presets.json` (currently claimed by the XP-W1 package). **This plan never auto-mutates difficulty data, never adds adaptive difficulty, and never phones home.** The precedent is DEC-07: `BALANCE_SIM_radiation_exposure_20A.md` findings F1–F8 were "recorded as authoritative baselines" and then *signed off or declined by a foreman* (`docs/plans/wave9_part2/C3_DECISION.md`), never auto-applied.

**Bounded non-goals:** no network telemetry of any kind; no player-identifying data; no remote config; no silent/adaptive difficulty director; no Unity work; no new gameplay systems; no mutation of `Assets/StreamingAssets/Data/`; no changes to the XP-W1-claimed Difficulty authority beyond read-only consumption; no migration of the existing onboarding sigil vocabulary.

# 2. Current Reality

Everything below was re-verified against current source during planning (Rule 7). Where the source plan's baseline (`ccac926e`) and current HEAD disagree, HEAD wins and the drift is noted.

## 2.1 The unreproducible corpus (46A premise)

- `artifacts/balance/` contains **28 CSV files on disk / 29 git-tracked paths** (the 29th is `.gdignore`, so Godot ignores the directory for import; the CSVs themselves **are committed to git**). The source plan counted 27 at `ccac926e`; the count at HEAD is 28 — the corpus is still growing without attribution, which *is* the bug.
- Two incompatible header dialects coexist, proving multiple unrecorded producers:
  - `seed,scenario,day,hunger,thirst,fatigue,warmth,morale,health,radiationDose,lifetimeExposure,foodConsumed,waterConsumed,hungerCritical,thirstCritical,healthLossPerDay` (the `fed_*` family), produced by `Ashfall.Core.Tests/BalanceFedLoopTelemetryTests.cs` (8 `[InlineData]` theory rows: seeds 42/123/999 × scenarios DailyRation/Scarcity/SevereScarcity × 7/14/30 days) via the test-only helper `Ashfall.Core.Tests/TelemetryArtifactWriter.cs` (`TryWriteLines(directory, filename, lines)` — swallows `IOException`/`UnauthorizedAccessException` and returns `false`).
  - `Seed,Day,Difficulty,Ammo_Casings,Ammo_Lead,Ammo_Powder,Ammo_Crafted,Ammo_Fired,Ammo_Stock,Tooling_Lathe,Tooling_Press,Tooling_Bench,Radio_Decrypted,Radio_Triangulated,Social_PrivacyFatigue_Avg,Social_Disputes,Excavation_MethanePPM,Excavation_ShoringPermille,Excavation_CaveIns` (`shelter_operations_100day.csv`) — a different producer entirely, consistent with `Ashfall.Core.Tests/Balance/ShelterOperationsBalanceSim.cs`.
- `scripts/balance/` **does not exist** (verified: `ls scripts/` → `audit_assets.py, ci, composio_asset_pipeline.py, _dev_tools, generate_item_icons.py, maintenance, pipeline, README.md, run_test.sh, tools`). There is no scripted, checked-in way to regenerate any CSV.
- `docs/balance/` exists with 8 methodology/findings documents (`BALANCE_SIM_radiation_exposure_20A.md`, `BALANCE_SIM_EXPEDITION_DESTINATIONS.md`, `BALANCE_SIM_STARTING_COHORTS.md`, `BALANCE_SIM_STARTING_PROFILES.md`, `COLLECTIBLE_BALANCE_AUDIT.md`, `COLLECTIBLE_SCAVENGING_BASELINE.md`, `MICRO_LOCATION_HAZARD_RISK_REWARD.md`, `WILDLIFE_TRAPPING_BASELINE.md`) — but **no `README.md`, no `TARGETS.md`, no `DECISIONS.md`**. Findings are written down; targets and tuning decisions are not.
- Existing balance test classes (all in `Ashfall.Core.Tests`): `BalanceFedLoopTelemetryTests`, `ResourceMassBalanceSimulationTests`, `StartingCohortBalanceSimulationTests`, `EcologyBalanceSimulationTests`, `TravelEncounterBalanceSimulatorTests`, `Plan12EBalanceSimulationTests`, `Radiation/Plan20ARadiationBalanceSweepTests` (9 cases), `Shelter/Plan20BShieldingBalanceSweepTests` (12 cases), `Balance/ShelterOperationsBalanceSim`, `Inventory/Plan21LongCampaignSoakTests`, `Expeditions/Plan76BalanceSimulationTests`. The convention is: **in-test harness, deterministic ticks, paired-run fingerprint assertions, CSVs emitted as a side effect via `TelemetryArtifactWriter`.** No run header is stamped on any artifact.
- The one existing run-header precedent is perf-side: `artifacts/runtime-scale-results.json` records `SchemaVersion`, `BenchmarkId`, and a `Context` block (`WorkloadId, CampaignDays, Seed, RosterTier, CatalogTier, JournalTier, ExpeditionTier, WorldStateTier, BuildConfiguration, Platform, Runtime…`). 46A's run header generalises this shape for balance artifacts.
- `PerformanceCampaignHarness` (`Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`) already composes a legal deterministic campaign in engine-free Core: `CampaignDayCoordinator` + `SurvivorRosterSystem`, `Inventory`, `JournalSystem`, `WeatherSystem`, `ExpeditionSystem`, `LocationEvolutionSystem`, `WildlifeMigrationSystem`, `LandmarkDegradationSystem`, `SeededRng(context.Seed)`, registered through a private `PerfDayOwner : IDayAdvanceOwner` (phase + `TickDay(int day, List<DayStateChangeEvent> events)`), advanced by `AdvanceDays(int days)`. This is the composition pattern 46A reuses; it is perf-oriented (stub ticks), so 46A extends the *pattern* with real consumption policies, it does not misuse the perf harness as a balance oracle.

## 2.2 The day-record substrate (46B premise)

- `Assets/Ashfall.Core/Campaign/DayRecord.cs` (Plan 31C, engine-free, `[Serializable]`):
  - `DayRecord { int schemaVersion = DayRecordBuilder.CurrentSchemaVersion; string sessionId; long seed; int day; string[] ownerOrder; List<DayRecordOwner> owners; List<DayRecordEvent> events; }`
  - `DayRecordOwner { string ownerId; double durationMs; bool failed; string? failureCode; }`
  - `DayRecordEvent { string kind; string sourceOwnerId; string primaryId; string secondaryId; float numeric; string causeId; string actorId; }` — note: `causeId`/`actorId` exist in the schema but are **not populated** by the builder at HEAD (they default to `string.Empty`); a schema consumer must tolerate empties.
  - `DayRecordBuilder.CurrentSchemaVersion = 1`; `static DayRecord FromDay(long seed, string? sessionId, int day, DayAdvancedEventArgs? args)` (pure; null-safe); `static string ToJsonLine(DayRecord record)` (project `SystemTextJsonSerializer`, `IncludeFields`, compact).
- Host writer `src/Main.DayRecord.cs`: `const string DayRecordEnvVar = "ASHFALL_DAY_RECORD"`; enabled iff the env var equals `"1"` (ordinal); `AppendDayRecordIfEnabled(int day, DayAdvancedEventArgs? args)` builds the record with `sessionId = _saveLoadHost?.ActiveSlotId?.ToString() ?? "unsaved"` and `seed = CampaignSeedForGeneration()`, appends one JSONL line to `ProjectSettings.GlobalizePath("user://day-record.jsonl")` via `File.AppendAllText`, and swallows failures into `GD.PushWarning`. The file header states the contract: **dev/debug only, off in release, observational only — never read back into simulation state.** Called from exactly one site: `ShowBriefingForDay(int day, DayAdvancedEventArgs? args)` in `src/Main.Campaign.cs`.
- The day-event stream it records is `DayAdvancedEventArgs { int Day; DayOwnerReport[] OwnerReports; }` / `DayOwnerReport { string OwnerId; double DurationMs; … }` / `DayStateChangeEvent { string Kind; string SourceOwnerId; string PrimaryId; string SecondaryId; float Numeric; … }` from `Assets/Ashfall.Core/Campaign/CampaignDayCoordinator.cs`.
- `docs/telemetry/DAY_RECORD.md` pins the policy 46B inherits: integer `schemaVersion`; dev/debug-only writing, off in release; JSONL append-safe under the user path (never repo-absolute); seed+day+session/slot id, **never account/user identity**; monotonic per-owner durations only — **no wall-clock timestamps**; diagnostics never read back into simulation state.
- Env-var gating is an established repo convention: `ASHFALL_DATA` (`CatalogPath.cs`), `ASHFALL_MODS_DIR` (`ModRuntime.cs`), `ASHFALL_USER_DIR` (`SaveSlotRoot.cs`, `HostCli.cs`), `ASHFALL_LOG_DIR` (`HostCli.cs`), `ASHFALL_UI_NODE_DIAGNOSTICS` (`UiNodeDiagnostics.cs`, truthy-parse), `ASHFALL_DAY_RECORD` (above).

## 2.3 The sigil instrumentation (46B premise)

- `public void ObserveSigil(string sigil)` lives in `src/Main.Onboarding.cs`: it forwards to `_onboardingJourney.RecordSigil(sigil)` (creating the journey via `SetupOnboarding()` if needed, suppressed entirely when `UserSettingsStore.Current.TutorialMode == 2`) and resets `_onboardingLastInteractionSeconds`.
- **40 call sites / ~37 distinct literal sigils** exist across `src/` (verified by grep): `protocol.ration/maintenance/radio`, `store.opened`, `duty.assigned`, `weather.read`, `inventory.used`, `food.ration_consumed`, `expedition.dispatched`, `research.started`, `water.treatment_started`, `condenser.built` + 6 `condenser.*_blocked_*` variants, `deepwell.built` + 5 `deepwell.*` variants, `power.breaker_toggled`, `power.breaker_reset`(+`_missing_item`), `power.battery_bank_installed`(+2 blocked variants), `power.generator_serviced`(+2 blocked variants), `power.preset_shed`, `power.preset_defaults`. Blocked-reason suffixes (`*_blocked_missing_items`, `*_blocked_<reason>`) already encode outcome semantics in the sigil string itself.
- `OnboardingJourney` / `OnboardingCatalog` (`Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs`) own the *guidance* state machine: `OnboardingStageDef(Id, Title, Objective, ShowMeWhereRoute, (Sigil, Threshold)[] Requirements)`, fixed `Order` (Protocol → Inspect → Rationing → Assignment → Weather → InventoryUse → …), `DaySentinel = "day.at_least"`. Sigil names are specified stable across saves. **The journey is a gameplay-guidance authority with persistence; 46B must not fork it** — it subscribes to the same sigil stream as a second, measurement-only consumer.
- `AshfallInputActions` (`src/Host/AshfallInputActions.cs`) is the canonical InputMap action registry (keyboard + joypad defaults; e.g. `Close = "ashfall_close"`). Input-mode classification (keyboard/mouse/controller) for 46B step 10 is derivable host-side from Godot `Input` events; no Core change is needed for it.

## 2.4 Difficulty authority (46A/46C boundary)

- `Assets/Ashfall.Core/Difficulty/` (engine-free, **claimed by `claim-xp-wave1-difficulty-2026-09-18` — see §7**): `DifficultyPresetCatalog` (`schema_version 1`, `presets`, `default_preset_id`, `Index()`, `TryGet`, `Validate`), `DifficultyPreset` (`id` must be snake_case `difficulty_*`, `display_name`, `description_key`, `scalars`, `starting_bonus_item_ids`), `DifficultyScalars` (8 multipliers — `hunger_rate_mult, thirst_rate_mult, radiation_gain_mult, disease_onset_mult, hostile_encounter_mult, market_price_mult, equipment_decay_mult, crisis_deadline_mult` — each validated finite within `[0.25, 2.5]`), `DifficultyScalarsProvider` (immutable typed view; `Legacy` = all-ones `difficulty_standard`; `FromPreset`), `DifficultyDirector` (`ResolvePreset(string? campaignPresetId)` → falls back to `default_preset_id`; `ResolveProvider`), `DifficultyPresetCatalogLoader.Load(dataDirectory, IFileIO)` reading `difficulty_presets.json`.
- `Assets/StreamingAssets/Data/difficulty_presets.json` (claimed): 4 presets × 8 scalars — `difficulty_sparing` (0.75 needs, 1.25 deadline), `difficulty_standard` (all 1.0), `difficulty_austere` (1.35 needs, 0.8 deadline), `difficulty_dirge` (1.75 needs, 0.65 deadline).
- Host: `src/Main.Difficulty.cs` — `SetupDifficulty()` loads the catalog via `CatalogPath.ResolveDataDir()`/`_dataDir`, constructs the director, resolves `_difficultyScalars = _difficulty.ResolveProvider(null)` (no per-campaign preset selection wired yet — that is queued package `CF-XP01-DIFFICULTY-FULL-BINDING`), on failure logs and falls back to `DifficultyScalarsProvider.Legacy`. One live scalar consumer at HEAD (`HostileEncounterMult`, `src/Main.EvolvingWorld.cs:193`).
- **Premise correction (Rule 7):** the Wave 12 A1 prerequisite audit recorded "no canonical difficulty ID/preset authority exists for 34B" as of 2026-09-18. The XP-W1 claim executed the same day created the catalog/director; per-campaign selection and persistence remain open under `CF-XP01-DIFFICULTY-FULL-BINDING`. Plan 46 therefore treats "difficulty labels exist" as *substantially satisfied at HEAD* and "preset selection persistence" as a *tracked external dependency*, not a blocker for 46A/46B (metrics record whatever preset the director resolves, defaulting included).
- Precedent for measurement→decision flow: `docs/balance/BALANCE_SIM_radiation_exposure_20A.md` (+20B addendum) pinned findings F1–F8 with in-test harnesses (`Plan20ARadiationBalanceSweepTests` 9/9, `Plan20BShieldingBalanceSweepTests` 12/12, paired-run fingerprints, zero RNG); DEC-07 (`docs/governance/DECISION_REGISTER.md`) records them `SIGNED` as "authoritative baselines" with recheck trigger "Major difficulty tuning pass"; the F1 re-scale proposal was explicitly "foreman decision, not applied" and was ultimately **DECLINED** by the signed `C3_DECISION.md`. This is exactly the human-in-the-loop discipline 46C institutionalises.

## 2.5 Settings, version, and CLI seams

- `UserSettingsData` (`Assets/Ashfall.Core/Settings/UserSettingsData.cs`) is engine-free, `[Serializable]`, snake_case `[JsonPropertyName]` fields, `SchemaVersion = 1`, with a hand-maintained `Clone()` that copies every field (a new field MUST be added there too). Host store `UserSettingsStore` (`src/Settings/UserSettings.cs`) persists to `user://settings.json` with hardened `UserSettingsCodec.DeserializeWithRecovery` (malformed JSON recovers to defaults without throwing) and atomic temp-file save. Existing gameplay-preference rows: `tutorial_mode`, `confirm_end_day`, `verbose_radio_log`, `auto_save_on_day` — the opt-in toggle belongs beside these.
- Build/version provenance: `Assets/Ashfall.Core/VersionReport.cs` — the engine-agnostic `--version` report; build/game version is **host-supplied from project settings** (shape pinned by `Ashfall.Core.Tests/VersionReportContractTests.cs`). 46B's `build_version` field uses the same host-supplied source.
- Selftest verb convention: `HostCliRegistry` (`Assets/Ashfall.Core/HostCliRegistry.cs`) registers descriptors (`"--7-day-smoke-selftest"`, `--bridge-selftest`, `--data-integrity-selftest`, `--content-utilization-selftest`, …) with aliases; `IsSelfTest` is derived from the `-selftest` suffix; `src/Main.Application.cs` dispatches; failures exit through `HostCli.EmitUnhandledSelfTestFailure`. 46A/46B add their verbs here.
- CI gates: `docs/ci/CI_GATE_MANIFEST.json` is a `gates[]` array of `{ gate_id, name, category, command, timeout_seconds }`; `scripts/run_test.sh` caps focused xUnit runs at 180 s and rejects excluded targets (`TEST_POLICY.md`).

## 2.6 Governance state

- `INTEGRATION_PLANS.md`: current batch is XP Expansion W1 (`XP-WAVE1-DIFFICULTY-AUTHORITY`); C2[20]/Plan 46 is **not** among the 8 unblocked plans and is listed only as a prerequisite-audit target ("C1[16]/Plan 49 — DECIDED-DEFERRED pending C2[18]/Plan 42 and C2[20]/Plan 46 premise audits", `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` §4).
- `docs/plans/wave12_part1_1/A1_PLAN49_PREREQUISITE_AUDIT.md` (read in full): Plan 49 requires "Plan 46B reachability evidence"; Plan 46 "requires difficulty labels and release/onboarding rails" (C2[12]/Plan 34 `PARTIALLY-SEALED`, C2[16]/Plan 39 `AUDIT-PENDING`). Consequence for this plan: 46C's release-gate integration is specified as an *interface* (a checklist line + report artifact contract), not an edit to Plan 39's unclaimed release-gate files.
- EN-01 cross-check (Rule 7 applied to `Seal-steps/ashfall-enhanced-expansion-program-en-01-en-08-and-xp-pillar-rescoping-2026-09-19.md`): that document is an **unauthorised proposal set** ("Authorization status: none authorized"). Its EN-01 (Difficulty-Consequence Weave, §C.1.2 step 7) specifies a "balance-soak harness (seeded, 30/60/90-day) asserting difficulty monotonicity" and a standard-preset parity fingerprint — a *natural downstream consumer* of 46A's sweep infrastructure (scenario data + run headers + drift gate are exactly what a monotonicity soak needs). It does **not** reference Plan 46 by name; no claim in that document is treated as authority here. If EN-01 is ever authorised, its soak harness should be a scenario pack on 46A's runner, not a new harness.

# 3. Required Delta

| # | From (current reality) | To (required state) |
|---|---|---|
| D1 | 28 committed CSVs with no producer, no run header, two header dialects | Every balance artifact regenerated in-repo from checked-in scenario JSON, stamped with a run header (git SHA, scenario, seeds, days, build config, runtime), one column dictionary |
| D2 | Balance knowledge scattered across 8 methodology docs; no targets, no decision log | `docs/balance/README.md` (generated summary panel), `TARGETS.md` (numeric targets + noise floor + review date), `DECISIONS.md` (ADR log, one entry per tuning change) |
| D3 | No CI protection against balance regressions | Nightly drift gate over the reference scenario set with declared tolerances; proof-of-failure test |
| D4 | Player behaviour unmeasured; 40 sigil sites feed guidance only | Opt-in local `PlaySessionRecorder` joins action records + day-event consequences; bounded rotated JSONL under `user://`; first-hour funnel / dead-end / stuck-day report generated locally |
| D5 | No scripted players; measurement requires a human | Deterministic policy archetypes (cautious/greedy/neglectful/expert) run through the same recorder headlessly via `--play-metrics-selftest` |
| D6 | Tuning changes leave no evidence trail | 46C ritual: every tuning ADR cites a sweep artifact + scenario + seed set + SHA; a docs gate test fails evidence-free ADRs; a per-release funnel report is a named release-checklist interface |
| D7 | `ObserveSigil` is a single-consumer onboarding hook | One additive fan-out (observer callback) so measurement subscribes without forking the onboarding authority or changing any sigil string |

# 4. Evidence

## 4.1 ⚠ Numbering collision — read before touching anything

Two unrelated features share the number 46 in this repository:

| | **Scavenging "Plan 46" (DONE, unrelated)** | **This plan (C2[20])** |
|---|---|---|
| Source | `piagentsplans/46-scavenging-tables.md` | `Next-steps-plans/shipped_to_chat/Plan_46_Playable_Metrics_Balance_Decisions_Player_Telemetry.md`; corpus `C-integration-plans/C2_planintegration[20].md` |
| Subject | Expedition scavenging loot tables, location-type affinity | Reproducible balance sweeps, local play metrics, measurement-driven difficulty decisions |
| Documents | `docs/expeditions/PLAN_46_SCAVENGING_TABLES_CLOSEOUT.md`, `PLAN_46_SCAVENGING_TABLES_BASELINE.md`, `PLAN_46_LOCATION_TYPE_AFFINITY_MATRIX.md`, `PLAN_46_EXPEDITION_TABLE_BINDINGS.md` | This document (`docs/plans/PLAN_46_PLAYABLE_METRICS_INTEGRATION_PLAN.md`) |
| Status | Implemented and closed | `AUDIT-PENDING` (census row `C2[20]`) |

The census itself flags the collision verbatim: *"closeout exists for Plan 46 but subject differs → numbering collision (`docs/expeditions/PLAN_46_SCAVENGING_TABLES_CLOSEOUT.md`)"* (`docs/plans/UNCLAIMED_CORPUS_CENSUS.md`, row `C2[20]`). **No scavenging-tables document is evidence for anything in this plan**, and nothing in this plan touches expedition loot tables. The same collision pattern exists for Plans 39/44/48/51/55/59/60/133/… (39 closeout-filename collisions were recorded by the Wave 10 census task), so file names alone are never proof of subject.

## 4.2 Source-plan evidence inventory, re-verified at HEAD

The source plan's nine-row evidence inventory (`ccac926e`) was re-run at current HEAD:

1. **"27 seeded balance CSVs"** → now **28 CSVs** + tracked `.gdignore`; headers verified in §2.1. Premise intact, count drifted (more unattributed output has landed).
2. **"Nothing references them"** → confirmed: `scripts/balance/` absent; the only producer code found is test-side (`BalanceFedLoopTelemetryTests`, `ShelterOperationsBalanceSim` convention).
3. **"No design-record home"** → partially stale: `docs/balance/` now exists (8 docs) but still lacks README/TARGETS/DECISIONS. Premise intact for targets/decisions.
4. **"No player telemetry"** → confirmed: every `telemetry` string in `src/`/`Assets/Ashfall.Core` is diegetic (orbital harrow, cohort supply, pump nodes); the only real diagnostic writers are `Main.DayRecord.cs` (opt-in) and perf artifacts.
5. **"Substrate exists"** → confirmed and stronger than the source knew: `DayStateChangeEvent` flows through `DayAdvancedEventArgs` to exactly one consumer (`DayRecordBuilder`), and 40 `ObserveSigil` sites form an action vocabulary already.
6. **"Perf measured but advisory"** → `artifacts/runtime-scale-results.json` present with structured context blocks; used as the run-header precedent.
7. **"Onboarding unmeasurable"** → confirmed: `docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md` Day-1 rows are selftest-based machine checks.
8. **"Difficulty doesn't exist yet"** → **stale**: `DifficultyPresetCatalog`/`DifficultyDirector`/4-preset `difficulty_presets.json` exist at HEAD under the XP-W1 claim; per-campaign binding is queued (`CF-XP01-DIFFICULTY-FULL-BINDING`). Plan 46 records `preset_id` from the resolved provider and treats deeper binding as an external dependency.
9. **"Content utilisation has a metric; play has none"** → confirmed: `--content-utilization-selftest` runs (619 catalogs / 12,161 definitions at the A1 audit baseline); no play-side equivalent exists.

## 4.3 Anchor documents read in full for this plan

`Next-steps-plans/shipped_to_chat/Plan_46_Playable_Metrics_Balance_Decisions_Player_Telemetry.md` (the correct source — see §4.1); `Assets/Ashfall.Core/Campaign/DayRecord.cs`; `src/Main.DayRecord.cs`; `docs/telemetry/DAY_RECORD.md`; `docs/balance/BALANCE_SIM_radiation_exposure_20A.md`; `docs/plans/wave12_part1_1/A1_PLAN49_PREREQUISITE_AUDIT.md`; `Assets/Ashfall.Core/Difficulty/{DifficultyDirector,DifficultyPresetCatalog,DifficultyScalarsProvider}.cs`; `src/Main.Difficulty.cs`; `Assets/StreamingAssets/Data/difficulty_presets.json`; `Assets/Ashfall.Core/Performance/Workloads/PerformanceCampaignHarness.cs`; `Ashfall.Core.Tests/BalanceFedLoopTelemetryTests.cs`; `Ashfall.Core.Tests/TelemetryArtifactWriter.cs`; `Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs`; `src/Main.Onboarding.cs` (sigil hook); `Assets/Ashfall.Core/Settings/UserSettingsData.cs`; `src/Settings/UserSettings.cs`; `Assets/Ashfall.Core/VersionReport.cs`; `Assets/Ashfall.Core/HostCliRegistry.cs`; `docs/governance/DECISION_REGISTER.md` (DEC-07, DEC-12, DEC-20); `TEST_POLICY.md`; `WORKTREE_OWNERSHIP.md`; `INTEGRATION_PLANS.md`; `docs/plans/UNCLAIMED_CORPUS_CENSUS.md`; the EN-01 section of `Seal-steps/ashfall-enhanced-expansion-program-en-01-en-08-and-xp-pillar-rescoping-2026-09-19.md` (treated as proposal, not authority).

## 4.4 Privacy/determinism precedent

- DEC-12 (`DECLINED`): "Client obfuscation / DRM rejected for open single-player .NET domain core" — the project's standing refusal of client-side surveillance-adjacent machinery. 46B's local-only, opt-in, identity-free design is the only design consistent with this register.
- `DAY_RECORD.md` policy table: dev/debug-only writing, off in release, no account/user identity, no wall-clock timestamps, diagnostics never read back. 46B inherits all five rules verbatim and adds redaction (no paths, no prose, ids from validated vocabularies only).
- `DEC-07`: balance findings become *signed baselines* through a human decision record. 46C automates the evidence plumbing, never the decision.

# 5. Existing Extension Seams

| Seam | Location | How Plan 46 extends it (never forks it) |
|---|---|---|
| Day-record pipeline | `DayRecordBuilder.FromDay` + `Main.DayRecord.cs` env-gated JSONL writer | 46B's recorder is a **sibling consumer** of the same `DayAdvancedEventArgs` (consequence half) and reuses the env-gated append-with-`PushWarning` writer shape verbatim |
| Day-event stream | `CampaignDayCoordinator` → `DayAdvancedEventArgs.OwnerReports[].Events` (`DayStateChangeEvent`) | Read-only join source for consequence records; no coordinator change |
| Sigil instrumentation | `Main.Onboarding.cs ObserveSigil(string)` (40 call sites) | Add **one** additive observer fan-out (`event Action<string>?` or second call) so the recorder receives the same sigil; zero sigil strings change; onboarding journey remains the guidance authority |
| Onboarding stage vocabulary | `OnboardingCatalog.Order`, `DaySentinel = "day.at_least"` | 46B's funnel *references* existing sigil ids; new measurement needs are met by adding sigils at real action sites through the existing hook (foreman-visible diffs), not a parallel vocabulary |
| Deterministic campaign composition | `PerformanceCampaignHarness` (coordinator + systems + `PerfDayOwner` pattern + `AdvanceDays`) | 46A's sweep runner follows this composition with real consumption/policy owners instead of perf stub ticks |
| Balance test conventions | `Plan20A/20B` sweeps: deterministic ticks, paired-run fingerprints, in-test assertions | 46A moves the *artifact production* into a scripted path while keeping determinism/fingerprint assertions in xUnit |
| Run-header precedent | `artifacts/runtime-scale-results.json` context block | Generalised into `BalanceRunHeader` stamped on every regenerated artifact |
| Difficulty authority | `DifficultyDirector.ResolveProvider` → `DifficultyScalarsProvider.PresetId` | Read-only: metrics *record* the resolved preset id; tuning decisions flow to humans, never back into the catalog from tooling |
| Settings opt-in | `UserSettingsData` + `UserSettingsStore` (hardened codec, atomic save) | One additive `play_metrics_enabled` bool (default **true but inert without the debug/env gate** — see §9 decision D-46-01), `Clone()` updated |
| Selftest verbs | `HostCliRegistry` descriptors + `Main.Application.cs` dispatch + `EmitUnhandledSelfTestFailure` | Two new descriptors: `--balance-sweep` (46A) and `--play-metrics-selftest` (46B) |
| Version provenance | `VersionReport` host-supplied build version | Source of the recorder's `build_version` field |
| CI manifest | `docs/ci/CI_GATE_MANIFEST.json` `gates[]` rows | New gates added only by the integrator at the phase that owns them (nightly drift gate is a *proposal* until foreman-signed) |
| Content reachability | `ContentUtilizationScanner` + `--content-utilization-selftest` | 46C step "retire unreachable content by evidence" consumes scanner + synthetic-session reachability; no scanner change in this plan |

**Collision check (mandatory before design):** no existing system records player actions to disk (`ObserveSigil` is in-memory journey state); no existing system regenerates `artifacts/balance/`; no existing CLI verb runs balance sweeps; `docs/telemetry/` contains only `DAY_RECORD.md`. The only overlap risk is with the XP-W1 Difficulty claim (handled as read-only consumption, §7) and with Plan 39's unclaimed release gate (handled as an interface, §19 Phase 7).

# 6. Proposed Architecture

Three deliveries, one rule each:

**46A — the sweep is data + a runner, not a test side effect.**
Scenario definitions move into checked-in JSON (`scripts/balance/scenarios/*.json`). An engine-free Core model (`BalanceSweepScenario`) validates them; an engine-free runner (`BalanceSweepRunner`) composes a real campaign (following the `PerformanceCampaignHarness` composition, with *real* needs/radiation/economy owners and deterministic policy archetypes instead of perf stub ticks) and produces rows + a run header. A host CLI verb (`--balance-sweep --scenario <id> --out <dir>`) executes one scenario through the real host composition; `scripts/balance/run_sweep.py` orchestrates the matrix, stamps headers, and regenerates `docs/balance/README.md`. The 28 orphan CSVs are archived to `docs/archive/balance/pre-46A/` with a provenance note — never silently deleted.

**46B — the recorder is a sibling consumer of streams that already exist.**
`PlaySessionRecorder` (Core, engine-free, pure) accepts (a) action records fed by the host from the single `ObserveSigil` fan-out plus a small set of explicitly enumerated host action sites (panel opened/closed by route id, save, quit, day-advanced), and (b) the same `DayAdvancedEventArgs` `DayRecordBuilder` already consumes. It writes nothing itself; a host partial (`src/Main.PlayMetrics.cs`) owns gating, file policy, rotation, and failure swallowing — exactly the `Main.DayRecord.cs` shape. A separate engine-free aggregator (`PlaySessionReport`) turns JSONL sessions into the funnel/dead-end/stuck tables; a selftest verb runs scripted policy sessions headlessly and emits the report, making the whole pipeline CI-testable without a human.

**46C — evidence flows to humans; humans flow decisions through existing change control.**
`docs/balance/TARGETS.md` (numbers + noise floor + review date), `docs/balance/DECISIONS.md` (ADR log; each entry must cite artifact + scenario + seed set + SHA, enforced by a docs gate test), `docs/balance/REVIEW_RITUAL.md` (monthly ritual: read report → ≤3 decisions, each with owner + plan number), and a named release-checklist interface line for the per-release funnel report. Any resulting tuning edit to `difficulty_presets.json` is a normal, separately-claimed change under the difficulty authority's owner — 46C produces the *evidence*, never the edit. No adaptive difficulty, no director mutation, no remote config: consistent with DEC-07's "recorded, then signed" flow and the source plan's 46C step 3 ("no adaptive difficulty without an explicit rule … never silent").

# 7. Ownership Matrix

Claim check performed against `WORKTREE_OWNERSHIP.md` at HEAD: **no active claim covers any Plan 46 path.** Relevant adjacent claims:

| Path / concern | Current claim state | Plan 46 posture |
|---|---|---|
| `Assets/Ashfall.Core/Difficulty/`, `Assets/StreamingAssets/Data/difficulty_presets.json`, `Ashfall.Core.Tests/Difficulty/` | **ACTIVE** — `claim-xp-wave1-difficulty-2026-09-18` (`XP-WAVE1-DIFFICULTY-AUTHORITY`) | **Read-only consumer.** Record `PresetId`; never edit catalog/data/tests. Any tuning ADR outcome is handed to that authority's owner as a separate claimed change. |
| `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`, `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` | Integrator/foreman-only ledgers | Plan 46 execution opens with a foreman claim row (`claim-c2-20-plan46-playable-metrics-<date>`) listing the exact paths below; census status flips only at closeout by the integrator |
| `docs/ci/CI_GATE_MANIFEST.json` | Integrator-owned shared seam | New gate rows land only in the phase that needs them, via the integrator, with `--check`-style regeneration discipline |
| `src/Main.Campaign.cs` (`ShowBriefingForDay`) | Shared seam (many historical claims; the 31C call site lives here) | One additive line (`AppendPlayMetricsDayJoinIfEnabled(day, args)`) beside the existing `AppendDayRecordIfEnabled(day, args)`; integrator-reviewed |
| `docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md`, release-checklist surfaces | Plan 39 scope is `AUDIT-PENDING`/unclaimed | 46C defines the *interface* (report artifact path + pass criteria); editing Plan 39's gate is out of scope until that package is claimed |
| Everything under §20 File Impact Map not listed above | Unclaimed at HEAD | Claimed by the Plan 46 package row at Phase 0 |

Package shape: one claim, three foreman-visible phase gates (46A seal → 46B seal → 46C seal), matching the source plan's execution order `46A → 46B → 46C`.

# 8. Data Flow

## 8.1 46A sweep flow

```
scripts/balance/scenarios/<id>.json           (checked-in scenario: seed list, days, roster tier,
        │                                      policy archetype, consumption schedule, world config)
        ▼
scripts/balance/run_sweep.py                  (matrix orchestration; computes git SHA + build config;
        │                                      one subprocess per (scenario, seed-set) run)
        ▼
godot --headless --path . -- --balance-sweep --scenario <id> --out <dir>
        │
        ▼
BalanceSweepScenarioLoader (Core, validates JSON against schema, rejects unknown fields)
        ▼
BalanceSweepRunner (Core, engine-free):
   composes campaign exactly once per seed — CampaignDayCoordinator + real owners
   (NeedsSystem, RadiationSystem, Inventory, Economy, Weather, Expeditions, …)
   + ScriptedPolicy (archetype: cautious|greedy|neglectful|expert) issuing deterministic
   actions through the owners' public APIs; seeded only via CampaignRngManager/SeededRng
        ▼
artifacts/balance/<scenario>_<seedset>_<days>d.csv   (one header line = BalanceRunHeader JSON
        │                                               comment row + column dictionary row)
        ▼
run_sweep.py aggregates → docs/balance/README.md   (generated panel: survival@7/30/90,
                                                     time-to-first-critical, time-to-first-death,
                                                     morale floor, dose curve, exhaustion day;
                                                     regenerated, never hand-edited; --check mode)
```

## 8.2 46B play-metrics flow

```
Player input → existing command sites (panels, ration policy, dispatch, consume, save, quit)
        │
        ├─ ObserveSigil(sigil)  ──► OnboardingJourney.RecordSigil   (guidance authority, unchanged)
        │                  │
        │                  └─► NEW additive fan-out: SigilObserved?.Invoke(sigil)
        │                                                    │
        ├─ explicit action sites (panel route open/close, save, quit, day-advance click)
        │                                                    │
        ▼                                                    ▼
Main.PlayMetrics.cs (host): gate check (env + debug + settings) → PlaySessionRecorder.Record(...)
        │                                    DayAdvancedEventArgs ──► PlaySessionRecorder.JoinDay(...)
        │                                    (sibling of DayRecordBuilder.FromDay — same args, read-only)
        ▼
user://play-metrics/session-<randomId>.jsonl   (bounded: per-file byte cap, max N session files,
        │                                        oldest rotated out at session end)
        ▼
godot --headless -- --play-metrics-selftest    (scripted policy session → recorder → aggregator)
        │
        ▼
PlaySessionReport (Core aggregator) → local markdown tables (funnel %, median time-to-step,
                                     top dead-end panels, stuck-day count, input-mode histogram)
                                     → artifacts/play-metrics/ (git-ignored) or stdout
```

**Never happens in either flow:** network I/O of any kind; reading metrics back into simulation; writing to campaign saves; mutating `difficulty_presets.json` or any `Assets/StreamingAssets/Data/` file; collecting usernames, absolute paths, machine ids, or prose.

# 9. State Model

- **46A:** stateless tooling. Scenario JSON is immutable input; the runner holds a throwaway campaign per seed; artifacts are write-once outputs. No save section, no runtime state.
- **46B:** `PlaySessionRecorder` holds *session-memory only* state: current session id, monotonic session clock (durations, never wall-clock — `DAY_RECORD.md` rule), per-day action counts for stuck detection, funnel step completion bitset. All of it dies with the process; the JSONL file is the only persistence, and it is **diagnostics, never read back** (same rule as DayRecord). Session id is a host-generated random (`Guid.NewGuid().ToString("N")` produced in `Main.PlayMetrics.cs`, passed into Core as a string — Core never generates identity; this is not deterministic-Core RNG use, it never touches simulation).
- **Settings state:** `UserSettingsData.play_metrics_enabled` (snake_case), `Clone()`-updated, codec-recoverable. **Decision point D-46-01 (foreman confirms at Phase 0):** default `true`, but inert unless the process is a debug build or `ASHFALL_PLAY_METRICS=1` is set — implementing the source plan's "off by default in release, on by default in dev" while keeping release exports physically incapable of recording. Effective enablement: `enabled = (env == "1") || (env != "0" && OS.IsDebugBuild() && settings.PlayMetricsEnabled)`. `PRIVACY.md` documents the default, the toggle, and the deletion route in plain language.
- **46C:** documentation state only (TARGETS/DECISIONS/RITUAL), plus one docs-gate test. No runtime state.

# 10. API/Contracts

New engine-free Core surface (all `Ashfall.Core`, no Godot/UnityEngine/engine-serialization references):

```csharp
namespace Ashfall.Core.Balance
{
    public sealed class BalanceSweepScenario            // validated scenario model
    {
        public const int CurrentSchemaVersion = 1;
        public int schema_version { get; set; }
        public string id { get; set; }                  // snake_case, e.g. "fed_dailyration"
        public int[] seeds { get; set; }                // explicit, non-empty
        public int days { get; set; }                   // [1..400]
        public string roster_tier { get; set; }         // ScaleTier vocabulary
        public string policy { get; set; }              // cautious|greedy|neglectful|expert
        public string difficulty_preset_id { get; set; }// must resolve in difficulty_presets.json
        public ConsumptionScheduleDef consumption { get; set; }  // food/water intervals+units
        public WorldConfigDef world { get; set; }       // weather profile id, hazard cadence
        public bool Validate(out string error);
    }
    public static class BalanceSweepScenarioLoader { public static BalanceSweepScenario LoadFromJson(string json); }

    public sealed class BalanceRunHeader                // stamped on every artifact
    {
        public int schema_version;                      // header schema, independent of scenario schema
        public string git_sha;                          // supplied by orchestrator (host/py), never guessed
        public string scenario_id; public int[] seeds; public int days;
        public string build_configuration;              // Debug/Release + godot/dotnet versions (host-supplied)
        public string difficulty_preset_id;
        public string generated_by;                     // "ashfall --balance-sweep" + verb version
    }

    public sealed class BalanceSweepRunner              // deterministic; one instance per (scenario, seed)
    {
        public BalanceSweepRunner(BalanceSweepScenario scenario, int seed, IBalancePolicy policy);
        public BalanceSweepResult Run();                // rows + per-day metrics + fingerprint
    }
    public interface IBalancePolicy                     // scripted archetype; deterministic actions only
    {
        void OnDayStart(IBalanceWorld world, int day);  // ration/dispatch/repair decisions through owner APIs
    }
    public sealed class BalanceSweepResult
    {
        public IReadOnlyList<string> CsvRows;           // column dictionary v1 (§11)
        public IReadOnlyList<BalanceDayMetrics> Days;   // typed per-day metrics (survival, first-critical, …)
        public string Fingerprint;                      // SaveChecksum-style hash of typed metrics
    }
}

namespace Ashfall.Core.Telemetry
{
    public static class PlaySessionActions              // closed action vocabulary (test-pinned)
    {
        public const string SessionStart = "session_start";
        public const string SessionEnd   = "session_end";
        public const string PanelOpened  = "panel_opened";   public const string PanelClosed = "panel_closed";
        public const string Sigil        = "sigil";
        public const string RationPolicySet = "ration_policy_set";
        public const string Dispatch     = "dispatch";       public const string ChoiceResolved = "choice_resolved";
        public const string Consume      = "consume";
        public const string Save         = "save";           public const string Quit = "quit";
        public const string DayAdvanced  = "day_advanced";
        public static bool IsKnown(string action);
    }

    [Serializable] public sealed class PlaySessionEvent  // one JSONL line (§11 schema)
    {
        public int schemaVersion; public string recordType;
        public string sessionId;  public string buildVersion; public string presetId;
        public long seed;         public int day;             public long tSessionMs;   // monotonic duration
        public string action;     public string targetId;     public string outcome;    public string sigil;
        public string kind;       public string sourceOwnerId; public string primaryId; // day-join records:
        public string secondaryId; public float numeric;                                 // DayStateChangeEvent mirror
    }

    public sealed class PlaySessionRecorder             // pure; no IO; no wall clock; no RNG
    {
        public const int CurrentSchemaVersion = 1;
        public PlaySessionRecorder(string sessionId, string buildVersion);  // identity injected by host
        public void SetContext(long seed, string presetId);                 // host supplies on campaign load
        public void Record(string action, string targetId, string outcome, int day, long tSessionMs);
        public void RecordSigil(string sigil, int day, long tSessionMs);
        public void JoinDay(DayAdvancedEventArgs args, int day, long tSessionMs); // same args DayRecord consumes
        public IReadOnlyList<PlaySessionEvent> Drain();                     // host flushes + appends
        public string ToJsonLine(PlaySessionEvent evt);
    }

    public sealed class FirstHourFunnel                 // ordered measurement-only steps over recorded events
    {
        public static readonly FunnelStep[] Steps;      // guidance_opened, first_craft?, first_dispatch,
    }                                                   // first_ration_decision, first_storm_survived,
                                                        // first_day_past_tutorial, first_death_witnessed
                                                        // (each step = predicate over action/target/sigil + day)

    public sealed class PlaySessionReport               // engine-free aggregator over JSONL sessions
    {
        public static PlaySessionReport Aggregate(IEnumerable<PlaySessionEvent> events);
        public string ToMarkdown();                     // funnel %, median t-to-step, dead-end panels,
    }                                                   // stuck days, input-mode histogram
}
```

Host surface (src/): `src/Main.PlayMetrics.cs` (gating, session id, file policy/rotation, flush points, `SigilObserved` fan-out subscription), `src/Host/BalanceSweepSelfTest.cs` (`--balance-sweep`), `src/Host/PlayMetricsSelfTest.cs` (`--play-metrics-selftest`), descriptors in `HostCliRegistry.cs`, dispatch in `src/Main.Application.cs`, one additive line in `src/Main.Campaign.cs` (`ShowBriefingForDay`), session start/end hooks in `src/Main.GameFlow.cs`, input-mode classification helper in `src/Host/` reading Godot `Input` (aggregated category only: keyboard/mouse/controller — never device ids).

Contract rules inherited verbatim from `DAY_RECORD.md`: integer schemaVersion with backward-reading tooling; JSONL append-safe under `user://` (never repo-absolute); dev/debug-only writing, hard-off in release; monotonic durations only; never read back into simulation; no identity.

# 11. Data Changes

**No changes to `Assets/StreamingAssets/Data/`.** All new data is tooling data or user-local diagnostics.

| Path | Kind | Content |
|---|---|---|
| `scripts/balance/scenarios/*.json` (≈6 files, new) | checked-in tooling data | Scenario definitions reproducing the existing corpus families: `fed_dailyration`, `fed_scarcity`, `fed_severescarcity`, `power_econ_combinedstress`, `power_econ_stable`, `rad_zone_sweep` (covering the 28 existing files), plus new-scenario slots for season severity / ration policy / gear lifespan / power shedding / identity-relation effects (source 46A step 10) as follow-on packs |
| `artifacts/balance/**` (regenerated) | generated artifacts (git-tracked, policy decided at Phase 2: keep tracked with headers, or `.gitignore` + regenerate in CI — foreman picks; current state is tracked, with `.gdignore` present) | Column dictionary v1: `git_sha,scenario,seed,day,hunger,thirst,fatigue,warmth,morale,health,radiationDose,lifetimeExposure,foodConsumed,waterConsumed,hungerCritical,thirstCritical,healthLossPerDay` (existing `fed_*` columns preserved for comparability, `git_sha`+`scenario` promoted to columns so every row is self-attributing) |
| `docs/archive/balance/pre-46A/` (new) | archive | The 28 orphan CSVs moved here with a README noting provenance ("producer unrecoverable; superseded by run-attributed corpus") |
| `user://play-metrics/session-*.jsonl` | user-local diagnostics | 46B output; bounded (≤16 session files, ≤2 MiB each — caps pinned in tests); deleted via documented route |
| `UserSettingsData` | settings DTO | one additive field `play_metrics_enabled` (§9) |
| `docs/balance/{README,TARGETS,DECISIONS,REVIEW_RITUAL}.md`, `docs/telemetry/{PLAY_METRICS,PRIVACY}.md` | docs | §19 Phase 3/6/7 |

# 12. Save/Load

**No new save sections; no changes to any save codec, envelope, or migration ladder.** Explicitly:

- 46A constructs throwaway campaigns inside the runner; it may *use* `CaptureSavePayload`-style serialization for fingerprinting but persists nothing to player saves.
- 46B's recorder state is session-memory only; the JSONL is diagnostics and is **never read back** (DayRecord rule). Campaign saves, slots, and `SaveSectionRegistry` are untouched.
- The only persisted bit is `play_metrics_enabled` in `user://settings.json` — the settings DTO, strictly separate from simulation saves, with codec recovery (unknown/malformed → safe default). Rollback removes the field without breaking old or new settings files (System.Text.Json ignores unknown properties on read; `Clone()` is the only hand-maintained copy site and is updated in the same commit).
- `JoinDay` receives the same `DayAdvancedEventArgs` instance the briefing and DayRecord consume and must be **observational**: a test asserts the args object is unchanged after the recorder pass (see §18).

# 13. Determinism

- **46A:** the runner consumes RNG only through the existing seeded contracts (`CampaignRngManager(masterSeed)`, `SeededRng(seed)`); policies are pure functions of (day, world view) with no `System.Random`, no wall-clock seeding, no hash-order iteration. Pins: same (scenario, seed) ⇒ byte-identical typed metrics (`Fingerprint` equality test); the corpus test re-runs a reference scenario in-process twice and asserts identical CSV rows; scenario JSON validation rejects unspecified seeds so a sweep can never silently go non-deterministic.
- **46B:** the recorder performs **zero** simulation I/O in the Core sense — it allocates strings from values it is handed and appends to a drain list. It consumes no RNG, reads no simulation state, and its session clock is a monotonic duration (never used by gameplay). **Replay-fidelity pin:** a paired test runs the same seeded campaign slice twice — recorder enabled vs disabled — and asserts identical `SaveChecksum` over the captured state, proving the recorder cannot perturb the simulation. A second pin asserts `DayAdvancedEventArgs` deep-equality before/after `JoinDay`.
- **46C:** documentation-only; the ADR gate test is deterministic over repo files.
- TEST_POLICY compliance: determinism/replay tests are independent (never aggregated); sweep execution lives in the host verb/nightly gate, not in the 180-second focused xUnit budget.

# 14. System/Event Wiring

| Wire | From → To | Mechanism |
|---|---|---|
| Sigil fan-out | `Main.Onboarding.cs ObserveSigil` → recorder | One additive `event Action<string>? SigilObserved` (or a second direct call guarded by the metrics gate) invoked after `RecordSigil`; onboarding behaviour unchanged when no subscriber |
| Day join | `Main.Campaign.cs ShowBriefingForDay` → recorder | One additive line `AppendPlayMetricsDayJoinIfEnabled(day, args)` beside `AppendDayRecordIfEnabled` |
| Session lifecycle | `Main.GameFlow.cs` (new game / load / quit) → recorder | `SessionStart`/`SetContext(seed, presetId)` on campaign start or load; `SessionEnd` + rotation on quit/return-to-menu |
| Action sites | explicit enumeration (§8.2): panel route open/close (via the existing `PanelRegistry` open actions), save, quit, ration policy set, dispatch, choice resolved, consume | Each site calls `PlayMetricsRecord(action, targetId, outcome)` — a gate-checked host helper; no gameplay branch is altered; blocked actions record `outcome="blocked:<reason>"` reusing existing reason codes |
| Preset context | `Main.Difficulty.cs` → recorder | `_difficultyScalars.PresetId` read at `SetContext` time; re-read on load |
| Input mode | host `Input` events → recorder | Aggregated category transitions only (`input_mode=keyboard|mouse|controller`), recorded on change, no device detail |
| Sweep execution | `run_sweep.py` → `--balance-sweep` verb → Core runner | Scenario path + out dir as CLI args; header fields (git SHA, versions) supplied by the orchestrator/host, never guessed inside Core |
| Report generation | `--play-metrics-selftest` | Runs scripted policy sessions (reusing 46A's `IBalancePolicy` archetypes through the real host) → recorder → `PlaySessionReport` → stdout + `artifacts/play-metrics/` (git-ignored) |
| Difficulty tuning (46C) | ADR → human → separate claimed package | Documentation reference only; zero runtime wiring |

# 15. Godot Integration

- Host-only code lives in `src/` (`net8.0`), Core in `Assets/Ashfall.Core/` (`netstandard2.1`); the engine-free boundary is preserved (new Core files reference neither `Godot` nor engine serialization; JSON via the project's `SystemTextJsonSerializer`).
- File policy mirrors `Main.DayRecord.cs`: `ProjectSettings.GlobalizePath("user://play-metrics/…")`, `File.AppendAllText`, `try/catch` → `GD.PushWarning` (recording must never crash gameplay), no autoloads, no new scene-tree nodes, no `_Process` work — the recorder is push-driven from existing call sites.
- Release hard-off: `OS.IsDebugBuild()` is false in release export templates, so the recorder is never constructed there regardless of settings/env (defence in depth with the env gate); a selftest asserts the release-mode gate evaluates false.
- `--balance-sweep` / `--play-metrics-selftest` follow the existing headless selftest path (`HostCliRegistry` descriptor + `Main.Application.cs` dispatch + `EmitUnhandledSelfTestFailure`), so they run in CI exactly like `--content-utilization-selftest`.
- Settings UI: one toggle row in the existing settings panel ("Record local play diagnostics (this device only)") with the `PRIVACY.md` one-liner; preserves keyboard/controller focus and close/back behaviour per the UI rules.
- If a runtime check is needed during implementation, headless selftests run at the project-standard 15 FPS; no Unity invocation anywhere.

# 16. Narrative/Content Integration

Genuinely inapplicable: this package authors no narrative content, no diegetic text, and no player-facing prose beyond one settings toggle label and documentation; metrics records contain ids and reason codes only (prose is explicitly banned by the redaction rules), so there is no localization or tone surface to integrate.

# 17. Failure Modes

| # | Failure | Detection | Mitigation |
|---|---|---|---|
| F1 | Env var `ASHFALL_PLAY_METRICS=1` leaks into a release export | Release-gate selftest asserts `OS.IsDebugBuild()==false` ⇒ recorder not constructed even with env set | Hard `IsDebugBuild` gate ANDed with env; PRIVACY.md documents the three gates |
| F2 | Unbounded metrics file growth | Rotation test (17th session deletes oldest); byte-cap test truncates flush at 2 MiB with a `session_end(reason="cap")` record | Caps pinned in tests; rotation at session end |
| F3 | Prose/path/username leaks into records | Redaction test scans emitted JSONL for `/`, `\`, `Users`, home-dir token, and any string outside the action/target/outcome vocabularies | `targetId`/`outcome` validated against known vocabularies (route ids, catalog ids, reason codes); unknown values degrade to `"other"` |
| F4 | Recorder exception kills gameplay | Fault-injection test (full disk / locked file) | All IO inside `try/catch → GD.PushWarning` (DayRecord pattern); recorder never throws across the host boundary |
| F5 | Recorder perturbs simulation | Paired-run checksum test (§13) fails | Recorder is push-only; no reads of mutable sim state |
| F6 | Funnel vocabulary drift (a sigil renamed, funnel silently empty) | `FirstHourFunnel` steps test-pinned against `OnboardingCatalog` sigils + enumerated action sites; CI fails on unknown reference | Single fan-out keeps one vocabulary; renaming a sigil breaks the pin test loudly |
| F7 | Sweep non-determinism (policy uses unseeded randomness) | Same-seed fingerprint test; `System.Random` grep gate over `Ashfall.Core/Balance/` | Policies receive only `ISeededRng`-backed world views |
| F8 | Corpus compared across waves without re-baseline (stale-vs-new numbers) | `DECISIONS.md` ADR gate requires before/after table + artifact citation for any tuning claim | 46A step 8 ritual: regenerate once per wave, explicitly |
| F9 | Drift gate false-positives on noise | Noise floor stated in `TARGETS.md` (seeds needed before a delta is meaningful); tolerance bands per metric | Gate compares against declared tolerances, not exact equality |
| F10 | Session-id collision or persistence across launches | Test asserts two recorder constructions differ and no id appears in two files | `Guid.NewGuid` per process, in-memory only |
| F11 | `JoinDay` mutates shared args (poisoning briefing/DayRecord) | Deep-equality assertion before/after (§13) | Read-only enumeration, like `DayRecordBuilder` |
| F12 | Settings default surprises a dev-build player | First-run notice line in PRIVACY.md + settings toggle + env `=0` override | Decision D-46-01 requires foreman sign-off at Phase 0 |
| F13 | `--balance-sweep` runtime blows the focused-test budget | Verb runs outside xUnit; xUnit covers only schema/determinism/drift-proof units | TEST_POLICY: sweeps are a nightly gate with a declared window |
| F14 | Two agents touch the same seam (Main.Campaign.cs, CI manifest) | Phase 0 claim lists exact lines; integrator owns shared seams | Single claim, phase-gated |

# 18. Test Strategy

Per `TEST_POLICY.md`: focused targets only, `bash scripts/run_test.sh <file>`, new files run alone first, <100 cases per builder, determinism/save/lifecycle tests never aggregated, sweeps outside the 180 s xUnit budget.

| Test file (new) | Cases (est.) | What it proves |
|---|---|---|
| `Ashfall.Core.Tests/Balance/BalanceSweepScenarioTests.cs` | 8–10 | Schema validation (bad version, empty seeds, unknown field, bad preset id, out-of-range days/multiplier); JSON round-trip; ids snake_case |
| `Ashfall.Core.Tests/Balance/BalanceCorpusTests.cs` | 8–12 | Same (scenario, seed) ⇒ identical `Fingerprint` and identical CSV rows; run header complete (sha/scenario/seeds/days/config/preset present); column-dictionary stability pin; **drift assertion proof-of-failure** (intentional 5% regression on a scratch baseline fails the comparer); README `--check` regeneration mode detects hand edits |
| `Ashfall.Core.Tests/Telemetry/PlaySessionRecorderTests.cs` | 12–16 | Schema field pin (every `PlaySessionEvent` field present, version 1); opt-out ⇒ zero records drained (gate simulated); redaction (F3 vectors); session-id injection (F10); JSONL round-trip parse; `JoinDay` copies `DayStateChangeEvent` fields faithfully incl. empty `causeId/actorId`; vocabulary rejection (`IsKnown`) |
| `Ashfall.Core.Tests/Telemetry/PlaySessionReportTests.cs` | 8–10 | Funnel detection on a scripted event stream (each step fires at the right event, order-respecting); median time-to-step; dead-end detection (N repeats / panel open-close oscillation); stuck-day detection (day-join with zero actions); input-mode histogram aggregation; empty-input report still well-formed |
| `Ashfall.Core.Tests/Telemetry/PlayMetricsReplayFidelityTests.cs` | 3–4 | Paired seeded campaign slice recorder-on vs recorder-off ⇒ identical `SaveChecksum`; `DayAdvancedEventArgs` deep-equality across `JoinDay`; recorder drain does not alter a second drain (idempotent) |
| `Ashfall.Core.Tests/Docs/BalanceDecisionsGateTests.cs` (or existing docs-gate home) | 3–5 | Every `DECISIONS.md` entry cites artifact path + scenario + seed set + git SHA (evidence-free ADR fails); `TARGETS.md` declares noise floor + review date; README panel contains every declared section |
| Host selftests | — | `--balance-sweep --scenario fed_dailyration --seeds 42 --days 3` headless smoke (artifact + header emitted); `--play-metrics-selftest` synthetic policy session ⇒ funnel report generated, opt-out run writes no file, release-mode gate evaluates false |

Existing suites that must stay green when their seam is touched: `Ashfall.Core.Tests/Campaign/DayRecordTests.cs` (day-join sibling), `Ashfall.Core.Tests/Onboarding/*` (sigil fan-out), `Ashfall.Core.Tests/Difficulty/*` (read-only consumption), settings codec tests (new field round-trip + recovery), `VersionReportContractTests.cs` (build provenance source untouched).

# 19. Dependency-Ordered Phases

Execution order honouring the source plan (`46A → 46B → 46C`) and its wave-7 dependency note (45A → 46A → 47A → 46B → … → 46C): content-acceptance rails (Plan 45, SEALED) already exist, so 46A may start once claimed.

**Phase 0 — Premise re-audit + claim (read-only, foreman).** Re-verify §2/§4 against execution-day HEAD (CSV count, sigil count, XP-W1 claim status, preset-binding progress); foreman signs `claim-c2-20-plan46-playable-metrics-<date>` with the §20 path list and rules on decision **D-46-01** (settings default) and the **artifacts tracking policy** (keep CSVs git-tracked with headers vs `.gitignore` + CI regeneration). *Gate: claim row + two decision records exist.*

**Phase 1 — 46A Core sweep model (builder).** `BalanceSweepScenario` + loader + validation, `BalanceRunHeader`, `IBalancePolicy` + four archetypes (cautious/greedy/neglectful/expert), `BalanceSweepRunner` composing the campaign on the `PerformanceCampaignHarness` pattern with real owners, `BalanceSweepResult` + fingerprint. Tests first: scenario + corpus-determinism units. *Gate: new test files pass alone via `scripts/run_test.sh`; zero production wiring yet.*

**Phase 2 — 46A execution path (builder).** `--balance-sweep` host verb, `scripts/balance/run_sweep.py`, `scripts/balance/scenarios/*.json` covering the six existing corpus families, corpus regeneration with headers, orphan archival to `docs/archive/balance/pre-46A/` with provenance README. *Gate: regenerated corpus reproduces the shape of the old `fed_*` numbers under `difficulty_standard` (parity table in the phase log — divergences are findings, not silently accepted); verb smoke passes headless.*

**Phase 3 — 46A records + gates (builder + integrator).** `docs/balance/README.md` generator (+`--check`), `TARGETS.md` (numbers, rationale, noise floor, review date), `DECISIONS.md` (ADR format + first entry: the 46A re-baseline), `BalanceCorpusTests` drift proof-of-failure, nightly drift-gate proposal added to `docs/ci/CI_GATE_MANIFEST.json` by the integrator (category "Balance", declared window, tolerances from TARGETS). *Gate: docs-gate test green; integrator signs manifest row.*

**Phase 4 — 46B Core recorder (builder).** `PlaySessionActions`, `PlaySessionEvent`, `PlaySessionRecorder`, `FirstHourFunnel`, `PlaySessionReport`; all §18 Telemetry tests first. *Gate: recorder tests pass alone; replay-fidelity test proves zero sim impact.*

**Phase 5 — 46B host wiring (builder).** `UserSettingsData.play_metrics_enabled` (+`Clone`, codec round-trip test), `src/Main.PlayMetrics.cs` (three-gate check, session id, file policy/rotation, flush points), `SigilObserved` fan-out in `Main.Onboarding.cs`, day-join line in `Main.Campaign.cs`, session hooks in `Main.GameFlow.cs`, enumerated action sites, settings-panel toggle, input-mode classification. *Gate: onboarding + DayRecord + settings suites green; host build 0 errors/0 warnings; opt-out writes nothing.*

**Phase 6 — 46B selftest + report + docs (builder).** `--play-metrics-selftest` (scripted policy sessions through the real host), local markdown report, `docs/telemetry/PLAY_METRICS.md` (schema + reading a report), `docs/telemetry/PRIVACY.md` (stance, defaults, deletion route — store-listing-citable). *Gate: selftest passes headless; PRIVACY statements mechanically true (redaction test is the proof).*

**Phase 7 — 46C loop closure (builder + integrator).** `docs/balance/REVIEW_RITUAL.md`, ADR gate test, release-checklist *interface* line (report artifact contract for Plan 39 to consume when claimed — no edit to Plan 39 files), per-release balance-delta generation note for the changelog owner, long-tail metric definitions (38C deadlines / 41C generations / 34C legacy rows) recorded as TARGETS sections. *Gate: docs-gate green; first ritual scheduled with named owner.*

**Phase 8 — Closeout (integrator/foreman only).** Census `C2[20]` → SEALED with evidence links; `INTEGRATION_PLANS.md`/`KNOWN_DEBT.md` ledger rows; handoff per `AI_AGENT_WORKFLOW.md`.

# 20. File Impact Map

**New — Core (engine-free):**
| File | Reason |
|---|---|
| `Assets/Ashfall.Core/Balance/BalanceSweepScenario.cs` | Scenario model + validation + loader (46A) — scenarios become reviewable data |
| `Assets/Ashfall.Core/Balance/BalanceRunHeader.cs` | Attribution header stamped on every artifact (46A step 3) |
| `Assets/Ashfall.Core/Balance/BalanceSweepRunner.cs` + `IBalancePolicy` + 4 archetype policies | Deterministic scripted campaigns measuring design intent, not dice noise (46A step 9) |
| `Assets/Ashfall.Core/Telemetry/PlaySessionRecorder.cs` | The 46B recorder + event schema + actions vocabulary — extends the DayRecord pattern, no parallel pipe |
| `Assets/Ashfall.Core/Telemetry/FirstHourFunnel.cs` | Measurement-only funnel over recorded events (46B step 5) |
| `Assets/Ashfall.Core/Telemetry/PlaySessionReport.cs` | Local aggregator → markdown tables (46B step 8) |

**New — host:**
| File | Reason |
|---|---|
| `src/Main.PlayMetrics.cs` (+`.uid`) | Gating, session id, file IO/rotation, flush — the `Main.DayRecord.cs` sibling |
| `src/Host/BalanceSweepSelfTest.cs` | `--balance-sweep` verb (46A execution through real composition) |
| `src/Host/PlayMetricsSelfTest.cs` | `--play-metrics-selftest` synthetic sessions (46B step 9/12) |

**New — tests:** the seven §18 files (each with its stated reason; created test-first per policy).

**New — scripts/docs/data:**
`scripts/balance/run_sweep.py` (orchestrator), `scripts/balance/scenarios/*.json` (≈6), `docs/balance/README.md` (generated), `docs/balance/TARGETS.md`, `docs/balance/DECISIONS.md`, `docs/balance/REVIEW_RITUAL.md`, `docs/telemetry/PLAY_METRICS.md`, `docs/telemetry/PRIVACY.md`, `docs/archive/balance/pre-46A/` (orphan archival + provenance note).

**Modified (additive only):**
| File | Change | Reason |
|---|---|---|
| `Assets/Ashfall.Core/Settings/UserSettingsData.cs` | +1 bool `play_metrics_enabled`, `Clone()` row | Opt-in toggle (46B step 1) |
| `src/Settings/UserSettings.cs` | none expected (codec is field-driven) — touch only if the codec needs the key registered | Settings round-trip |
| `src/UI/SettingsPanel.cs` | +1 toggle row with privacy one-liner | User-facing opt-in/out |
| `src/Main.Onboarding.cs` | +1 additive `SigilObserved` fan-out inside `ObserveSigil` | Subscribe without forking (46B step 3) |
| `src/Main.Campaign.cs` | +1 line `AppendPlayMetricsDayJoinIfEnabled(day, args)` in `ShowBriefingForDay` | Consequence join (46B step 4) |
| `src/Main.GameFlow.cs` | session start/end hooks | Session lifecycle records |
| `src/Main.PlayerSurfaces.cs` + enumerated action-site partials (`Main.Inventory.cs`, `Main.Expeditions.cs`, `Main.Holdfast.cs`, …) | `PlayMetricsRecord(action, target, outcome)` at existing command points — **no new sigils required for v1**; the 40 existing sigil sites are reused | Action half of the record (46B steps 2–3) |
| `src/Host/AshfallInputActions.cs` | input-mode classification helper hook (aggregate category only) | 46B step 10 accessibility-parity signal |
| `Assets/Ashfall.Core/HostCliRegistry.cs`, `src/Main.Application.cs` | 2 descriptors + 2 dispatch cases | Selftest verbs |
| `docs/ci/CI_GATE_MANIFEST.json` | +2 rows (balance sweep nightly; play-metrics selftest) | Gates (integrator-owned) |
| `.gitignore` | `artifacts/play-metrics/` (always); `artifacts/balance/` only if Phase 0 picks CI-regeneration policy | Artifact hygiene |
| `docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md` | funnel-report review row | 46C step 1 interface |
| `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` | claim row (Phase 0), status flip (Phase 8) | Governance — foreman/integrator only |

**Explicitly untouched:** `Assets/StreamingAssets/Data/**` (including `difficulty_presets.json`), `Assets/Ashfall.Core/Difficulty/**`, `Ashfall.Core.Tests/Difficulty/**` (XP-W1 claim), `DayRecord.cs`/`Main.DayRecord.cs` (sibling, not modified), `OnboardingJourney.cs` (guidance authority), all save codecs and `SaveSectionRegistry`, `ContentUtilizationScanner.cs`, any Unity path.

# 21. Risks

| Risk | Severity | Handling |
|---|---|---|
| Privacy regression (the source plan names this *the* 46B risk) | High if wrong | Three independent gates, redaction test, DEC-12-consistent local-only stance, PRIVACY.md mechanically verified, D-46-01 foreman sign-off before any code |
| Recorder subtly perturbs gameplay (observer effect) | High | Replay-fidelity checksum test; push-only design; no reads of mutable state; IO failures swallowed |
| Sweep numbers treated as targets before TARGETS exists | Medium | Phase order: corpus regeneration (P2) precedes targets (P3); every README table cites scenario+seeds+SHA |
| Drift gate noise churn | Medium | Noise floor + tolerances in TARGETS; nightly-only; proof-of-failure test |
| XP-W1 claim drift (preset binding lands mid-package) | Medium | Phase 0 re-audit re-reads claim status; recorder reads `PresetId` dynamically so binding progress is automatically reflected |
| Scope creep into adaptive difficulty | High if it happens | Hard rule in §1/§22; any "director" proposal is a separate signed decision (source 46C step 3); this plan contains zero runtime difficulty mutation |
| Corpus regeneration changes "known" numbers | Low | Parity table in Phase 2 gate; divergences become DECISIONS.md findings, not silent edits |
| Shared-seam races (`Main.Campaign.cs`, CI manifest) | Medium | Single claim, integrator-owned seams, phase-gated |
| 28 old CSVs misread as still-authoritative | Low | Archival with provenance README; README.md points at the new corpus |

# 22. Out of Scope

- Any network telemetry, analytics SDK, crash-uploader, or remote config (banned absolutely; DEC-12-consistent).
- Adaptive/silent difficulty, a "director" that mutates `DifficultyScalars` at runtime, or any tooling that writes `difficulty_presets.json` (46C produces evidence; humans tune through the claimed difficulty authority).
- Migrating, renaming, or re-thresholding existing onboarding sigils or `OnboardingJourney` stages.
- Changes to Plan 39's release gate (interface only), Plan 34B preset selection UI (queued under `CF-XP01-DIFFICULTY-FULL-BINDING`), or Plan 42 voice work (prerequisite-sibling, `AUDIT-PENDING`).
- The scavenging-tables "Plan 46" (closed; unrelated — §4.1).
- New gameplay systems, new save sections, per-player identity, wall-clock analytics, multi-session cross-device anything.
- EN-01…EN-08 proposals (unauthorised; EN-01's future monotonicity soak may later land as a 46A scenario pack, by its own signature).

# 23. Rollback Strategy

Every phase is additive; rollback is deletion + additive-line removal, in reverse phase order:

1. **46C:** delete `REVIEW_RITUAL.md` + ADR gate test + checklist row. No runtime footprint.
2. **46B:** remove the two descriptors/dispatch cases, the settings toggle row, the `UserSettingsData` field (old settings files with the key still load — unknown JSON properties are ignored; `Clone()` revert in same commit), `Main.PlayMetrics.cs`, the one-line day-join, the fan-out line in `ObserveSigil` (onboarding unaffected), and Core `Telemetry/` files. Delete `user://play-metrics/` (user-local). No save migration exists because nothing was ever persisted into saves.
3. **46A:** remove the verb, runner, scenario files, and script; restore the archived CSVs from `docs/archive/balance/pre-46A/` (they are moved, never deleted, precisely so rollback is a `git mv`).
4. **Governance:** integrator reverts claim/census rows.

No data migration, no save-format concern, no player-visible behavioural residue after any rollback step.

# 24. Definition of Done

**46A:** every CSV in `artifacts/balance/` is regenerable by `python3 scripts/balance/run_sweep.py` from checked-in scenarios; every artifact carries a run header (git SHA, scenario, seeds, days, build config); `docs/balance/README.md` is generated (and `--check`-clean); `TARGETS.md` states numeric targets + noise floor + review date; `DECISIONS.md` holds the re-baseline ADR; determinism + drift proof-of-failure tests pass; the 28 orphans are archived with provenance.

**46B:** with gates on, a debug session writes bounded, rotated, redacted JSONL under `user://play-metrics/` joining actions to day-event consequences; with gates off (or any release build) **zero bytes** are written; `--play-metrics-selftest` produces a funnel report (funnel %, median time-to-step, dead-end panels, stuck days, input-mode histogram) from scripted players with no human and no network; replay-fidelity checksum test proves the recorder cannot alter the simulation; `PLAY_METRICS.md` + `PRIVACY.md` published and mechanically true.

**46C:** every tuning-relevant ADR cites artifact + scenario + seed set + SHA (gate-enforced); `REVIEW_RITUAL.md` names owner + cadence; the release-checklist interface line is documented for Plan 39; at least the re-baseline decision is traceable end-to-end as the worked example ("the data said X, so a signed human changed/decided Y").

**Package:** all §18 tests green via `scripts/run_test.sh`; host build 0 errors/0 warnings; `--data-integrity-selftest`, `--bridge-selftest`, and the two new verbs pass headless; census row `C2[20]` flips to SEALED with evidence links; no `StreamingAssets/Data/` diff exists.

# 25. Implementation Handoff

Executing agent: read this plan, the source plan, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, and `AI_AGENT_WORKFLOW.md` before touching anything. Start at Phase 0 (read-only premise re-audit against execution-day HEAD — CSV count, sigil count, XP-W1 claim state, `CF-XP01` binding progress) and obtain the foreman claim plus decisions **D-46-01** (settings default) and the **artifacts tracking policy** before Phase 1. Work phase-by-phase with tests first; hand off per phase with command/result evidence.

## MUST PRESERVE

- `DayRecord`/`DayRecordBuilder`/`Main.DayRecord.cs` exactly as-is — 46B is a sibling consumer of `DayAdvancedEventArgs`, never a modification.
- `OnboardingJourney`/`OnboardingCatalog` as the sole guidance authority; all 40 existing sigil call sites and their strings byte-identical.
- The XP-W1-claimed Difficulty authority (`Assets/Ashfall.Core/Difficulty/`, `difficulty_presets.json`, its tests) — read-only consumption via `DifficultyScalarsProvider.PresetId`.
- Determinism: recorder consumes no RNG, reads no mutable sim state, never feeds back; sweeps seed only through `CampaignRngManager`/`SeededRng`; no `System.Random`, no wall-clock seeding.
- Engine-free Core: no `Godot`/engine references in any new `Assets/Ashfall.Core/` file.
- `TEST_POLICY.md` discipline: focused targets, `scripts/run_test.sh`, determinism/save tests unaggregated, sweeps outside the 180 s budget.
- The 28 existing CSVs until Phase 2 archives them with provenance (move, never delete).
- DEC-12/DEC-07 posture: local-only evidence, human-signed decisions.

## MUST ADD

- Core: `BalanceSweepScenario`+loader+validation, `BalanceRunHeader`, `BalanceSweepRunner`+`IBalancePolicy`+4 archetypes, `PlaySessionRecorder`+`PlaySessionEvent`+`PlaySessionActions`, `FirstHourFunnel`, `PlaySessionReport` (all engine-free).
- Host: `Main.PlayMetrics.cs` (three-gate enablement: `ASHFALL_PLAY_METRICS` env ∧/`OS.IsDebugBuild()` ∧/`play_metrics_enabled` per §9; session id injection; rotation ≤16 files/≤2 MiB; `PushWarning` failure policy), `SigilObserved` fan-out, one-line day join, session lifecycle hooks, `--balance-sweep` and `--play-metrics-selftest` verbs.
- Settings: `play_metrics_enabled` field + `Clone()` row + settings-panel toggle with privacy one-liner.
- Scripts/docs: `run_sweep.py`, ≈6 scenario JSONs, generated `docs/balance/README.md`, `TARGETS.md`, `DECISIONS.md`, `REVIEW_RITUAL.md`, `PLAY_METRICS.md`, `PRIVACY.md`, `docs/archive/balance/pre-46A/` provenance note.
- Tests: the seven §18 files including replay-fidelity (recorder on/off ⇒ identical `SaveChecksum`), redaction, opt-out-zero-bytes, drift proof-of-failure, and ADR-evidence gate.

## MUST NOT DO

- No network I/O, analytics SDK, crash upload, or remote config — anywhere, any build.
- No player-identifying data: no usernames, absolute paths, machine ids, free-text prose, or content strings in any record; ids from validated vocabularies only.
- No auto-mutation of `difficulty_presets.json` or any `StreamingAssets/Data/` file; no runtime/adaptive difficulty; no "director" logic.
- No new save section, save codec change, or reading diagnostics back into simulation state.
- No edits to the XP-W1 claim's Difficulty paths, to `DayRecord` files, or to onboarding sigil strings/stages.
- No Unity invocation; no `System.Random` in Core; no wall-clock timestamps in records (durations only).
- No census/ledger edits by the builder (foreman/integrator only); no starting work without the Phase 0 claim.
- Do not cite or reuse anything from `docs/expeditions/PLAN_46_SCAVENGING_TABLES_*.md` — different subject (§4.1).

## VERIFY WITH

- `bash scripts/run_test.sh Ashfall.Core.Tests/Balance` and `bash scripts/run_test.sh Ashfall.Core.Tests/Telemetry` (new files first, alone).
- `dotnet build Ashfall.csproj --nologo` → 0 errors / 0 warnings.
- `godot --headless --path . -- --data-integrity-selftest` → 0 errors; `--bridge-selftest` → exit 0.
- `godot --headless --path . -- --balance-sweep --scenario fed_dailyration --seeds 42 --days 3` → artifact + complete run header.
- `python3 scripts/balance/run_sweep.py --scenario <reference> --seeds 42,123,999` → twice ⇒ byte-identical CSVs.
- `godot --headless --path . -- --play-metrics-selftest` → funnel report; repeated with gates off ⇒ no file created.
- Privacy assertions: opt-out writes nothing; redaction test passes; release-mode gate evaluates false.
- Drift gate: intentional 5% scratch regression fails the comparer.

## FIRST SAFE IMPLEMENTATION STEP

Phase 0, read-only: re-run the §2 premise checks at execution HEAD (`ls artifacts/balance | wc -l`; `grep -c "ObserveSigil(" -r src/`; read the current `WORKTREE_OWNERSHIP.md` claim table and the XP-W1 binding status), write the two decision records (D-46-01 settings default; artifacts tracking policy), and have the foreman land the single claim row `claim-c2-20-plan46-playable-metrics-<date>` listing the §20 paths. Nothing else begins before that row exists.
