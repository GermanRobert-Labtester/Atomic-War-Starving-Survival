# PLAN-HOST-CLI-CONTRACT-86 — Host CLI Descriptor Truth, Exit Codes & Machine Manifest

**Wave 8 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INTEGRATION-KIT-02, PLAN-SELFTEST-TRUTH-23, PLAN-AUTOMATED-QA-CAMPAIGNS-74.
**Implementation scaffold:** [`PLAN-HOST-CLI-CONTRACT-86_APPENDIX-A_SCAFFOLD.md`](PLAN-HOST-CLI-CONTRACT-86_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-HOST-COMPOSITION-GOVERNANCE-71` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new verbs for their own sake, no second registry, no shell
scripts replacing the C# dispatcher.

## 1. Outcome
`HostCliRegistry.cs` (1,413 lines) already owns verb metadata: `HostCliAction`
+ `HostCliActionDescriptor` (`Category`, `PrimaryFlag`, `Aliases`,
`Description`, `ValuePlaceholder`, `AllFlags`, `IsSelfTest`, `IsTest`,
`HeadlessCompatible`, `TestId`, `FormatHelpLine()`), with `AllDescriptors`,
`FlagMap`, `CoreDescriptors`, `ExpansionDescriptors`. `src/Host/HostCli.cs`
exposes 218 distinct `--*-selftest` flags. What is missing is the **contract**:
descriptor↔dispatch parity, exit-code semantics, a machine-readable manifest
for CI, and a truth probe for `HeadlessCompatible`.

| Deliverable | Detail |
|---|---|
| Descriptor census | every descriptor generated from the registry; every dispatched flag must have one; every descriptor must dispatch |
| Exit-code contract | documented table (0 ok · 1 assertion failure · 2 usage · 3 environment/save) and enforced by the dispatcher |
| Generated help | `--help`/category help rendered from descriptors only — no hand-written verb lists |
| Machine manifest | `--cli-manifest` emits the descriptor table (id, flags, category, selftest, headless, test id) for CI consumption |
| Headless truth | each `HeadlessCompatible` descriptor is executed headless once; a descriptor that requires a window is corrected or reclassified |

## 2. Evidence
- `Assets/Ashfall.Core/HostCliRegistry.cs`: descriptor class + registry; 1,413 lines.
- `src/Host/HostCli.cs`: dispatcher; 218 unique `--*-selftest` flags.
- `src/Host/HostCli.PanelTests.cs`: `bridge_selftest` reports the empty
  `src/Bridge/` shim as removed.
- `scripts/run_test.sh`, `python3 scripts/ci/agent-fast-verify.py` consume flags by name today.

## 3. Packages
- **CLI-86A** descriptor census + parity gate: registry dump vs dispatch switch; fail on either-side-only rows.
- **CLI-86B** exit-code contract: constants + usage table + assertions in the dispatcher; tests per class.
- **CLI-86C** generated help: replace any hand-written verb list with descriptor rendering; `--help` golden test.
- **CLI-86D** `--cli-manifest`: deterministic JSON (sorted by flag) consumed by a check script; manifest diffed in CI.
- **CLI-86E** headless-compat probe: bounded run of headless-compatible descriptors; failures triaged, classification corrected.

## 4. Acceptance & verification
- Parity gate green: descriptor set == dispatch set; help lists all descriptors.
- Exit codes 0/1/2/3 covered by focused tests.
- Manifest check compares two runs byte-equal (deterministic ordering).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Host/` (or the focused CLI test region); one `--cli-manifest` headless run.

## 5. Risks
Descriptor metadata drift → the manifest gate owns the table; a new verb
without a descriptor fails. Exit-code churn breaking scripts → old codes keep
their meaning; new classes are additive.
# PLAN-HOST-CLI-CONTRACT-86 — Host CLI Descriptor Truth, Exit Codes & Machine Manifest

**Wave 8 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INTEGRATION-KIT-02, PLAN-SELFTEST-TRUTH-23, PLAN-AUTOMATED-QA-CAMPAIGNS-74.
**Implementation scaffold:** [`PLAN-HOST-CLI-CONTRACT-86_APPENDIX-A_SCAFFOLD.md`](PLAN-HOST-CLI-CONTRACT-86_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-HOST-COMPOSITION-GOVERNANCE-71` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new verbs for their own sake, no second registry, no shell
scripts replacing the C# dispatcher.

## 1. Outcome
`HostCliRegistry.cs` (1,413 lines) already owns verb metadata: `HostCliAction`
+ `HostCliActionDescriptor` (`Category`, `PrimaryFlag`, `Aliases`,
`Description`, `ValuePlaceholder`, `AllFlags`, `IsSelfTest`, `IsTest`,
`HeadlessCompatible`, `TestId`, `FormatHelpLine()`), with `AllDescriptors`,
`FlagMap`, `CoreDescriptors`, `ExpansionDescriptors`. `src/Host/HostCli.cs`
exposes 218 distinct `--*-selftest` flags. What is missing is the **contract**:
descriptor↔dispatch parity, exit-code semantics, a machine-readable manifest
for CI, and a truth probe for `HeadlessCompatible`.

| Deliverable | Detail |
|---|---|
| Descriptor census | every descriptor generated from the registry; every dispatched flag must have one; every descriptor must dispatch |
| Exit-code contract | documented table (0 ok · 1 assertion failure · 2 usage · 3 environment/save) and enforced by the dispatcher |
| Generated help | `--help`/category help rendered from descriptors only — no hand-written verb lists |
| Machine manifest | `--cli-manifest` emits the descriptor table (id, flags, category, selftest, headless, test id) for CI consumption |
| Headless truth | each `HeadlessCompatible` descriptor is executed headless once; a descriptor that requires a window is corrected or reclassified |

## 2. Evidence
- `Assets/Ashfall.Core/HostCliRegistry.cs`: descriptor class + registry; 1,413 lines.
- `src/Host/HostCli.cs`: dispatcher; 218 unique `--*-selftest` flags.
- `src/Host/HostCli.PanelTests.cs`: `bridge_selftest` reports the empty
  `src/Bridge/` shim as removed.
- `scripts/run_test.sh`, `python3 scripts/ci/agent-fast-verify.py` consume flags by name today.

## 3. Packages
- **CLI-86A** descriptor census + parity gate: registry dump vs dispatch switch; fail on either-side-only rows.
- **CLI-86B** exit-code contract: constants + usage table + assertions in the dispatcher; tests per class.
- **CLI-86C** generated help: replace any hand-written verb list with descriptor rendering; `--help` golden test.
- **CLI-86D** `--cli-manifest`: deterministic JSON (sorted by flag) consumed by a check script; manifest diffed in CI.
- **CLI-86E** headless-compat probe: bounded run of headless-compatible descriptors; failures triaged, classification corrected.

## 4. Acceptance & verification
- Parity gate green: descriptor set == dispatch set; help lists all descriptors.
- Exit codes 0/1/2/3 covered by focused tests.
- Manifest check compares two runs byte-equal (deterministic ordering).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Host/` (or the focused CLI test region); one `--cli-manifest` headless run.

## 5. Risks
Descriptor metadata drift → the manifest gate owns the table; a new verb
without a descriptor fails. Exit-code churn breaking scripts → old codes keep
their meaning; new classes are additive.

---

## 6. Expanded census (bespoke: CLI surface)

This plan governs the CLI descriptor contract, so the census covers the
registry, its descriptors/flags, the host files, and the CLI tests.
(Corrected: the categories and host-file patterns were tightened after an
initial pass reported zero for both.)

| Metric | Value |
|---|---:|
| `HostCliRegistry.cs` lines | 1414 |
| Descriptor constructions | 143 |
| Distinct flags | 231 |
| Selftest flags | 181 |
| Categories | 6 |
| `src/Host/` files | 390 |
| `src/Host/HostCli*` files | 29 |
| CLI test files | 4 |

**Sample flags (first 20):** `---`, `--7-day-smoke-selftest`, `--accessibility-selftest`, `--advanced-industrial-recon-selftest`, `--agriculture-selftest`, `--all-expansions-selftest`, `--amphibious-draisine-selftest`, `--aquaponics-selftest`, `--arbitration-selftest`, `--asset-coverage-report`, `--asset-registry-selftest`, `--atmosphere-selftest`, `--audio-selftest`, `--audio-test`, `--black-flotilla-selftest`, `--bridge-selftest`, `--brine-selftest`, `--campaign-journey-selftest`, `--caravan-selftest`, `--carbon-composite-selftest`

## 7. Expanded surface: descriptor contract

| Rule | Detail |
|---|---|
| Parity | descriptor set == dispatch set; either-side-only rows fail |
| Exit codes | 0 ok · 1 assertion · 2 usage · 3 environment/save |
| Help | rendered from descriptors only; no hand lists |
| Manifest | `--cli-manifest` emits the descriptor table deterministically |
| Headless truth | every `HeadlessCompatible` descriptor runs headless once |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Parity gate | descriptor ↔ dispatch comparison |
| Manifest | two runs byte-equal (sorted) |
| Exit codes | one focused case per class |
| Headless probe | per-descriptor run, failures triaged |

## 9. Rollout sequence

1. Census (this section).
2. Parity gate wiring.
3. Exit-code constants + tests.
4. Generated help + manifest check.
5. Headless-compat probe.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Descriptor | has a dispatch; metadata matches behavior |
| Flag | appears in help and manifest |
| Exit code | documented and enforced |
| Headless | classification proven by execution |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not add verbs.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 6. Other plans referencing them: **257**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-LAUNCH-FACE-06` | 2 |
| `PLAN-ARCHITECTURE-BOUNDARY-31` | 2 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 2 |
| `PLAN-DETERMINISM-CROSS-HOST-89` | 2 |
| `PLAN-DEPRECATED-TREE-RETIREMENT-94` | 2 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-INTEGRATION-KIT-02` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Assets/Ashfall.Core/HostCliRegistry.cs` |
| `HostCliRegistry.cs` |
| `PLAN-HOST-CLI-CONTRACT-86_APPENDIX-A_SCAFFOLD.md` |
| `scripts/run_test.sh` |
| `src/Host/HostCli.PanelTests.cs` |
| `src/Host/HostCli.cs` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `CLI-86A` | `Assets/Ashfall.Core/HostCliRegistry.cs`, `HostCliRegistry.cs` |
| `CLI-86B` | `PLAN-HOST-CLI-CONTRACT-86_APPENDIX-A_SCAFFOLD.md`, `src/Host/HostCli.PanelTests.cs` |
| `CLI-86C` | no name match — resolve at claim time |
| `CLI-86D` | no name match — resolve at claim time |
| `CLI-86E` | no name match — resolve at claim time |
| `CLI-86A` | `Assets/Ashfall.Core/HostCliRegistry.cs`, `HostCliRegistry.cs` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 5. Host files: **59** · Test files: **6** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 59 | `src/Audio/AudioSelfTest.cs`, `src/Host/HiddenAgendaSelfTest.cs`, `src/Host/HoldfastTradeSaveStoreSelfTest.cs`, `src/Host/HostCli.AdvancedIndustrialRecon.cs`, `src/Host/HostCli.Cartography.cs` |
| Tests (`Ashfall.Core.Tests/`) | 6 | `Ashfall.Core.Tests/Endgame/CrossRunProfileStoreTests.cs`, `Ashfall.Core.Tests/HostCliHelpContractTests.cs`, `Ashfall.Core.Tests/Legacy/Plan140GenerationalLegacyIntegrationTests.cs`, `Ashfall.Core.Tests/Tooling/ArchitectureTestMapGateTests.cs`, `Ashfall.Core.Tests/Tooling/SelfTestManifestGateTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **6** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan_trade_network` |
| `chemical_recon` |
| `hidden_agenda` |
| `holdfast` |
| `holdfast_trade` |
| `recon_telemetry` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **26** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--gpr-cartography-selftest` |
| `--help` |
| `--hidden-agenda-selftest` |
| `--hidden-agendas-selftest` |
| `--holdfast-briefing` |
| `--holdfast-runtime-selftest` |
| `--holdfast-runtime-ui-test` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **8**.

| Event | First declaration |
|---|---|
| `OnChapterAdvanced` | `Assets/Ashfall.Core/Legacy/GenerationalSuccessionEngine.cs` |
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractRenegotiated` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractSigned` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnDayAdvanced` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnStageAdvanced` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/caravan_trade_routes.json` |
| `Assets/StreamingAssets/Data/death_legacy_templates.json` |
| `Assets/StreamingAssets/Data/deep_lore_locations.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/duty_roles.json` |
| `Assets/StreamingAssets/Data/duty_roster_locations.json` |
| `Assets/StreamingAssets/Data/duty_roster_marks.json` |
| `Assets/StreamingAssets/Data/duty_roster_quests.json` |
| `Assets/StreamingAssets/Data/duty_roster_seasons.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (161 files, 1283 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `DutyRoster` | 5 | 49 |
| `Economy` | 41 | 329 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |
| `Legacy` | 1 | 5 |
| `Quests` | 4 | 25 |
| `Shelter` | 87 | 754 |

**Verdict:** 1283 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `DutyRoster`, `Economy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **512**
(229 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/AudioCueCatalog.cs` |
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioManager.cs` |
| `src/Audio/AudioSelfTest.cs` |
| `src/Audio/AudioSettings.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ExpansionAudioBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **41**, of which versioned-ladder sections:
**3**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `caravan` | no |
| `caravan_trade_network` | no |
| `chemical_recon` | no |
| `contractor_roster` | no |
| `death_legacy` | no |
| `deep_well` | no |
| `dose_ledger` | yes |
| `duty_roster` | no |
| `dynamic_quests` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **8**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `deep_coast` |
| `duty_roster` |
| `economy` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **140**
(CODEX_ONLY 74, GAMEPLAY_CONSUMED 42, OPTIONAL 7, UNRESOLVED 17).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `dose_locations.json` | GAMEPLAY_CONSUMED |
| `dose_quests.json` | GAMEPLAY_CONSUMED |

**Verdict:** 17 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_expelled_survivor` |
| `flag_honored_debt` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** governance · **Coupling (incoming plans):** 257
**Surface:** save sections 41 (laddered 3) · RNG streams 8 · host files 22 · catalogs 22 · test regions 9 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-HOST-CLI-CONTRACT-86
wave: 8
status: PROPOSED — foreman claim required
packages: CLI-86A, CLI-86B, CLI-86C, CLI-86D, CLI-86E, CLI-86A, CLI-86B, CLI-86C, CLI-86D, CLI-86E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/audio_accessibility_cues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/audio_cues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 257 other plan(s) name these artifacts (§12)
  - touches 3 versioned save ladder(s) — extend, never fork
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
