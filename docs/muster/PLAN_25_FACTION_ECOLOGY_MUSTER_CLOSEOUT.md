# Plan 25 Closeout — Faction Ecology & the Muster (25H.19)

> Completed 2026-09-01 (full-pass execution, per-batch commits on main). Scope and authority: `docs/plans/PLAN_25_FACTION_ECOLOGY_INTEGRATION_PLAN.md` + the 25.1 forensic audits in `docs/factions/` and `docs/muster/`.

## Delivered content (starting → final)

| Category | Start | Final | Authority |
|---|---:|---:|---|
| Peacetime faction actions (runtime-consumed) | 0 | **12** | `muster_faction_actions.json` (new) via `FactionActionBoard` |
| Faction-culture codex entries | 0 | **6** (C1–C6) | `muster_faction_culture.json` (new) via `FactionCultureCatalogLoader` |
| Muster witnesses | 3 | **15** | `muster_witnesses.json` (schema v2) |
| Testimony variants | 3 (flat) | **36** conditional | v2 testimonies, first-match on real flags |
| Camp scenes | 0 | **4** | `muster_camp_scenes.json` (new) via `CampSceneDirector` |
| Escalation chains (pre-war) | 0 | **6** | `faction_war_events.json` E-P1..P6 |
| Mid-war context chains | 0 | **6** | E-W1..W6 (gated on real 06C battles) |
| War-weariness chains | 0 | **4** | E-R1..R4 → pressure toward Muster path |
| Muster paths | 0 | **3** (negotiated / victors / unsettled) | `MusterPathEvaluator` |
| Cross-plan flag map | — | 45 flags, 0 orphans | `whitelists/plan25_flags.json` |

## Runtime seams built first (no content without a consumer)

1. `FactionActionBoard` + catalog loader — standing-band variants from each faction system's **own** scalar; effects through additive `AdjustTrust`/`AdjustMembers`/`AdjustLockoutRisk` seams; persisted resolution history (idempotence); item effects via host sink.
2. Witness schema **v2** + `WitnessSelector` + `IWitnessEligibility` port — v1 files load forever; dead subjects never testify; deterministic priority/ordinal ordering with faction-diversity cap.
3. `MusterPathEvaluator` — pure derivation over host-mapped `MusterPathInput`; additive `MusterState.musterPath`.
4. War-event extension — `requires_flag`/`produces_flag`/`standing_delta` (choice) + `FlagTrigger` in the closed grammar; runner-produced flags persisted; standing routed through the host applier (Core never mutates war standing).

## Host wiring

`MusterHostSession` (board, scenes, culture, `DeliverWitnesses`, `ResolveFactionAction`, `StageCampScene`), `MusterSaveStore` (additive `FactionActions`/`CampScenesSeen`; witness results inside `MusterState`), `FactionActionPanel` UI (offers + choices + culture codex section; JournalWitnessPanel idiom), journal integration for action resolutions, `RegionalTreatyFeed` (narrative → mechanical treaty adapter; the host now ships a RegionalTreatySystem catalog — closes the pre-existing production gap), `--faction-ecology-selftest` verb (registered in Parse + PrintHelp, help-contract green).

## Test evidence

- `dotnet test` — all Plan 25 suites green: FactionActionBoardTests 16, WitnessSelectionTests 13, MusterPathEvaluatorTests 13, FactionWarFlagExtensionTests 9, FactionWarChainRunnerTests + ContentCatalogTests (incl. trigger-table totality with 17 new stages), RegionalTreatyFeedTests 3, FactionEcologySelftestTests (27-check end-to-end demo over real data).
- Host: `dotnet build Ashfall.csproj` 0 errors; `--data-integrity-selftest` PASS 0 findings / 161 catalogs; `--faction-ecology-selftest` PASS 27/27; `--muster-selftest` PASS 25/25; `--muster-uitest` PASS; `--bridge-selftest` PASS.
- Timeline: `docs/factions/PLAN_25_POLITICAL_TIMELINE.md` (repo-pacing anchored; deviation from the plan-doc's war→Muster order recorded — the authored 06C chain lives at days 480–605, Muster canon is day 260).
- QA: `docs/muster/PLAN_25_POLITICAL_QA_MATRIX.md`; continuity: `docs/muster/PLAN_25_LATE_GAME_CONTINUITY_MATRIX.md`.

## Continuity decisions

- Continuity outranked narrative preference: escalation is pre-Muster (grievances → E-P chains), the hot war remains the authored post-Thaw 06C arc; weariness feeds the existing ceasefire (588), never ends the war.
- Witnesses substituted where flags don't exist (per plan §25G.12): "spared warlord" → Messenger's Keeper (`flag_messenger_kept` exists); "rescuee" → replaced by claimant/strike/dissenter set; "raised child" and "expedition survivor" deferred rather than fabricated.
- Dead subjects never testify; absence is a variant/fallback, never resurrection. `MusterOpeningDay = 260`, 06C chain ids/days, and all save formats untouched (additive only).

## Known limitations & deferred work

1. Witness alive/dead census binding: `IWitnessEligibility.IsSubjectAlive` is wired to a pass-all host adapter — survivor-census + lineage + palliative bindings are the first follow-up (port already in place).
2. Faction-action item economy: `item_id` effects declared in data (A4 uses `item_water_filter_advanced`) but the host item sink is not bound — resource transfers land with the shelter inventory adapter.
3. Culture codex currently renders inside `FactionActionPanel`; a dedicated codex panel integration (Plan 14 conventions) is follow-up polish.
4. Scale of authored variants: 3 band variants on several actions (poor/good fall back to neutral by design, documented in the QA matrix).
5. Manual 15-step late-game journey (25H.18) is scripted by the selftest demo; a full telemetry playtest session is recommended before release gating.
6. `Suspended`/`Expired` treaty statuses and `violation_penalty_affinity` remain unwired in `RegionalTreatySystem` (pre-existing gap, out of Plan 25 scope).

## Verification gate (final)

`dotnet build Ashfall.Core.Tests` clean · `dotnet test` green (Plan 25 suites) · `dotnet build Ashfall.csproj` 0 errors · `--data-integrity-selftest` PASS · `--bridge-selftest` PASS · `--muster-selftest` PASS · `--muster-uitest` PASS · `--faction-ecology-selftest` PASS.
