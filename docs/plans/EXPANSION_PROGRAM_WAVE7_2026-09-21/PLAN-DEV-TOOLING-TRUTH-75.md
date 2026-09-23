# PLAN-DEV-TOOLING-TRUTH-75 — Dev Console, Debug Views, Seed Tools & QA Commands

**Wave 7 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SELFTEST-TRUTH-23, PLAN-INPUT-HARDENING-25.
**Implementation scaffold:** [`PLAN-DEV-TOOLING-TRUTH-75_APPENDIX-A_SCAFFOLD.md`](PLAN-DEV-TOOLING-TRUTH-75_APPENDIX-A_SCAFFOLD.md) — source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no shipping cheats in release builds unless explicitly gated; no
network debug services.

## Outcome
Developers work through 217 CLI verbs, headless selftests, and hand edits.
There is no documented debug console/overlay toolset, no seed-injection command,
and no guard preventing debug affordances from leaking into release. This plan
provides a truthful, gated dev toolset.

| Tool | Purpose | Guard |
|---|---|---|
| Seed/show-state commands | reproduce a day/seed scenario | dev builds only; no save mutation without confirmation |
| Debug overlays | day owner timings, needs/radiation readouts, entity counts | toggle; zero cost when off |
| Content inspector | catalog row/consumer lookup (uses Plan 22 data) | read-only |
| Event log viewer | recent Core events by day (uses Plans 21/35) | read-only |
| Save surgery | edit/inspect save sections safely | explicit confirm; backup first |
| Replay/rollback | regenerate from seed to a target day | only in dev |
| Watch/fault injection | simulate owner overrun, bad catalog, bad mod for Plan 57 | scratch dirs only |

## Evidence
- 217 selftest verbs; `HostCliRegistry` (1,413 lines); 29 `HostCli.*` partials.
- Debug affordances already exist in headless runs (`--*` flags, diagnostics).
- Risks documented: release verification honesty (Plan 48 audit); secrets rule.
- Skills: `ashfall-time-travel-debugger`, `ashfall-silent`, `ashfall-tune`.

## Packages
- **DT-75A** dev-console contract: command list, permissions, and a `dev_mode` build flag that strips it from release exports.
- **DT-75B** seed/state commands: set seed, jump to day, grant/remove item — all with a confirmation and a save backup.
- **DT-75C** overlay suite: owner timing, key needs, dose, roster, active events; measured cost, off by default.
- **DT-75D** inspector commands: `inspect item <id>`, `inspect catalog <id>`, `inspect owner <id>` printing the consumer/owner chain (generated from plans 22/71 data).
- **DT-75E** fault injection: `inject bad-catalog`, `inject owner-overrun`, `inject bad-mod` for Plan 57 probes (scratch only).
- **DT-75F** release guard: an export check fails if dev commands/overlays are compiled into a release preset.

## Acceptance & verification
- Dev build exposes all tools; release build contains none; overlay off costs zero measurable frame time.
- `godot --headless --path . -- --dev-tools-selftest`; export guard; `dotnet build` 0/0.

## Risks
Debug drift into release → compile-time guard + export check; commands documented in one place.

---

## 6. Expanded census (bespoke: developer surfaces)

This plan governs developer tooling, so the census covers demos, tooling, the
CLI surface, and scripts rather than domain filenames.

| Metric | Value |
|---|---:|
| Headless demos (Core) | 33 |
| `tools/` entries | 66 |
| CLI flags in the registry | 231 |
| Scripts under `scripts/` | 101 |

**Headless demos:**

| Demo | Lines |
|---|---:|
| `BrineWaterHeadlessDemo.cs` | 131 |
| `CensusHeadlessDemo.cs` | 128 |
| `Cluster12CHeadlessDemo.cs` | 110 |
| `CombatHeadlessDemo.cs` | 289 |
| `CrossingArbitrationHeadlessDemo.cs` | 166 |
| `CrossingHeadlessDemo.cs` | 113 |
| `DeepCoastHeadlessDemo.cs` | 380 |
| `DiseaseHeadlessDemo.cs` | 490 |
| `DutyRosterHeadlessDemo.cs` | 153 |
| `EconomyHeadlessDemo.cs` | 103 |
| `EndgameHeadlessDemo.cs` | 102 |
| `EndingsHeadlessDemo.cs` | 105 |
| `ExpeditionHeadlessDemo.cs` | 120 |
| `ReconTelemetryHeadlessDemo.cs` | 94 |
| `SilentFoundryHeadlessDemo.cs` | 188 |
| `GreenhouseHeadlessDemo.cs` | 110 |
| `HoldfastHeadlessDemo.cs` | 115 |
| `IceRoadHeadlessDemo.cs` | 138 |
| `InfrastructureHeadlessDemo.cs` | 168 |
| `LedgerDebtHeadlessDemo.cs` | 337 |

… and 13 more demos.

## 7. Expanded surface: tooling contract

| Rule | Detail |
|---|---|
| Release mode | dev overlays/console are stripped or inert in release builds |
| Verb truth | every demo resolves to a real CLI verb (Plan 86's parity table) |
| Seed/state commands | dev commands can set seed and dump state deterministically |
| Fault injection | dev-only; never reachable from player paths |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Release guard | scan proves dev surfaces are absent/disabled in release |
| Verb parity | demos ↔ flags, both directions |
| Seed command | set seed → run → same checksum as a scripted run |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section) — demos, tools, flags, scripts.
2. Verb parity check; retire or wire dead demos.
3. Release-mode guard.
4. Seed/state command verification.
5. Regression: parity + guard.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Demo | resolves to a verb or is retired |
| Overlay/console | absent in release; inert when disabled |
| Flag | parity with its handler (Plan 86) |
| Script | named owner; no orphan tooling |

**Non-goals unchanged:** this expansion adds census and verification detail; no new developer surface is introduced.

---

## 12. Cross-plan coupling

Domain method: plan-body `.cs` enumeration.
Domain files: 20. Other plans referencing them: **19**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-CORE-ROOT-FAMILY-TRUTH-262` | 6 |
| `PLAN-SCENARIO-AUTHORING-102` | 2 |
| `PLAN-EXPEDITION-FAMILY-TRUTH-269` | 2 |
| `EVIDENCE` | 1 |
| `PLAN-MARITIME-DEEPWATER-27` | 1 |
| `PLAN-TRANSPORT-EXPEDITION-30` | 1 |
| `PLAN-PANDEMIC-PUBLIC-HEALTH-47` | 1 |
| `PLAN-COMBAT-DEPTH-62` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `DT-75A` | no name match — resolve at claim time |
| `DT-75B` | no name match — resolve at claim time |
| `DT-75C` | `DutyRosterHeadlessDemo.cs` |
| `DT-75D` | no name match — resolve at claim time |
| `DT-75E` | no name match — resolve at claim time |
| `DT-75F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 20; intra-domain edges: **0**; isolated files:
**20**.

**Top edges (by source name):**

| From | → To |
|---|---|
| — | no intra-domain references found |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `BrineWaterHeadlessDemo` | 0 |
| `CensusHeadlessDemo` | 0 |
| `Cluster12CHeadlessDemo` | 0 |
| `CombatHeadlessDemo` | 0 |
| `CrossingArbitrationHeadlessDemo` | 0 |
| `CrossingHeadlessDemo` | 0 |
| `DeepCoastHeadlessDemo` | 0 |
| `DiseaseHeadlessDemo` | 0 |
| `DutyRosterHeadlessDemo` | 0 |
| `EconomyHeadlessDemo` | 0 |

**Class split:** hub 0 · sink 0 · source 0 · isolated 20.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 20. Host files: **3** · Test files: **13** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/HostCli.PanelTests.cs`, `src/Host/HostCli.SelfTests.cs`, `src/Host/HostCli.cs` |
| Tests (`Ashfall.Core.Tests/`) | 13 | `Ashfall.Core.Tests/BrineWaterHeadlessDemoTests.cs`, `Ashfall.Core.Tests/CensusClaimSystemTests.cs`, `Ashfall.Core.Tests/Cluster12CHeadlessDemoTests.cs`, `Ashfall.Core.Tests/CombatHeadlessDemoTests.cs`, `Ashfall.Core.Tests/CombatSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **22** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chemical_recon` |
| `combat` |
| `contractor_roster` |
| `crossing` |
| `deep_well` |
| `disease` |
| `dose_ledger` |
| `duty_roster` |
| `economy` |
| `endgame` |
| `expedition` |
| `expedition_stealth` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **40** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--arbitration-selftest` |
| `--brine-selftest` |
| `--census-selftest` |
| `--cluster-selftest` |
| `--combat-breaching-selftest` |
| `--combat-selftest` |
| `--crossing-selftest` |
| `--deep-coast-host-selftest` |
| `--deep-coast-playthrough` |
| `--deep-coast-route-selftest` |
| `--deep-coast-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **21**.

| Event | First declaration |
|---|---|
| `OnCensusUpdated` | `Assets/Ashfall.Core/CensusClaimSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCombatPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnCombatPerkEarned` | `Assets/Ashfall.Core/Combat/CombatPerks.cs` |
| `OnDutyVacated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnEconomyChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnExpeditionCompleted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionFailed` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionStarted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionTick` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnIceRoadClosed` | `Assets/Ashfall.Core/IceRoadSystem.cs` |
| `OnIceRoadOpened` | `Assets/Ashfall.Core/IceRoadSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/cupola_foundry_catalog.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/disease_catalog.json` |
| `Assets/StreamingAssets/Data/duty_roles.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **10** (119 files, 831 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Combat` | 10 | 84 |
| `DutyRoster` | 5 | 49 |
| `Economy` | 41 | 329 |
| `Endgame` | 11 | 101 |
| `Foundry` | 8 | 73 |
| `Greenhouse` | 1 | 17 |
| `Holdfast` | 1 | 13 |
| `Telemetry` | 2 | 11 |
| `Tooling` | 35 | 117 |
| `Water` | 5 | 37 |

**Verdict:** 831 cases sit under matching regions — run those first (`Combat`, `DutyRoster`, `Economy`, `Endgame`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **85**
(27 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/ChemicalReconHostSession.cs` |
| `src/Host/ChemicalReconSaveStore.cs` |
| `src/Host/CombatHostSession.cs` |
| `src/Host/CombatSaveStore.cs` |
| `src/Host/ContractorRosterHostSession.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/DeepCoastHostSession.cs` |
| `src/Host/DeepWellHostSession.cs` |
| `src/Host/DeepWellSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **22**, of which versioned-ladder sections:
**2**.

| Section key | Laddered |
|---|---|
| `chemical_recon` | no |
| `combat` | no |
| `contractor_roster` | no |
| `crossing` | no |
| `deep_well` | no |
| `disease` | no |
| `dose_ledger` | yes |
| `duty_roster` | no |
| `economy` | no |
| `endgame` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **11**.

| Stream |
|---|
| `aquaponics_disease` |
| `black_market_debt_event` |
| `combat` |
| `cupola_foundry` |
| `deep_coast` |
| `disease` |
| `duty_roster` |
| `economy` |
| `expedition` |
| `foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **47**
(CODEX_ONLY 15, GAMEPLAY_CONSUMED 25, OPTIONAL 1, UNRESOLVED 6).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |

**Verdict:** 6 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_branch_mercy_road_locked` |
| `flag_honored_debt` |
| `flag_repaired_infrastructure` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** hub · **Coupling (incoming plans):** 19
**Surface:** save sections 22 (laddered 2) · RNG streams 11 · host files 25 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DEV-TOOLING-TRUTH-75
wave: 7
status: PROPOSED — foreman claim required
packages: DT-75A, DT-75B, DT-75C, DT-75D, DT-75E, DT-75F
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/ChemicalReconHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/anomalous_expedition_encounters.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/combat_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Combat/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 19 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 2 versioned save ladder(s) — extend, never fork
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
