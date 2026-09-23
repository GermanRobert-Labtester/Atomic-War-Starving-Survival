# PLAN-LAUNCH-FACE-06 — Player Surface, Input/Focus, Release & Truth Closure

**Program:** ASHFALL Expansion & Integration Program (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required. C2[15]/37,
C2[21]/48 and E1/53 are the three "available" head plans already documented
in `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md` §3; this plan sequences
them alongside the surface work created by Plans 01/04/05 and the truth gates
from Plan 02.
**Owner role on execution:** Foreman for head selection; Builders per package;
Integrator for shared surfaces (`Main.PlayerSurfaces.cs`, panel registry,
release workflow, CI gates).
**Depends on:** PLAN-INTEGRATION-KIT-02 K1/K2 (surface manifest + integration
selftest); PLAN-ORPHAN-SEAL-01 waves for surface volume.
**Expanded appendix:** [`PLAN-LAUNCH-FACE-06_APPENDIX-A_INPUT_ACTIONS.md`](PLAN-LAUNCH-FACE-06_APPENDIX-A_INPUT_ACTIONS.md)
— the input action inventory: **22 actions** (23 key / 9 joypad events) from `project.godot` (F6-1 parity gate input).

**Non-goals:** no new gameplay authority; no store launch date; no translation
catalog (DEC-13 remains deferred); no VO production (DEC-11 remains deferred).

---

## 1. Outcome

The programme ends with the game *presentable*: every integrated authority has
a real player surface or an explicit CLI-only classification; the input layer
has focus, controller bindings, and a contract gate; the release process has
versions, tags, a changelog, and a hotfix path; and every public claim the
build makes is backed by a live system. Six packages.

---

## 2. Premise evidence

### 2.1 Input/focus/controller (C2[15]/Plan 37, source re-verified 2026-09-21)
From `Next-steps-plans/shipped_to_chat/Plan_37_Hands_On_The_Wheel_Input_Focus_Controller.md`:

| Fact | Evidence |
|---|---|
| 21 actions defined (incl. 4 nav actions) | `project.godot [input]` |
| typed wrapper exists, 13 predicates | `src/Host/AshfallInputActions.cs:138–207` |
| 3 predicates never called | `IsConfirmOrAccept`, `IsExpeditions`, `IsHoldfast` — 0 call sites |
| directional nav actions unhandled | no `ashfall_nav_*` reference outside the actions file |
| no focus order | `grep "FocusMode\|MoveFocus" src/UI/*.cs` → 0; `GrabFocus` only in `MainMenuPanel.cs:62,66`, `ModalManager.cs:195,204,213` |
| no controller bindings | `grep -c "InputEventJoypad" project.godot` → 0 |
| no rebinding surface | settings store has audio/etc., no key-rebinding UI |
| fixed viewport | 1920×1080, `canvas_items`, `keep_height` |

### 2.2 Release craft (C2[21]/Plan 48)
From `Next-steps-plans/shipped_to_chat/Plan_48_Release_Craft_Versioning_Changelog_Hotfix.md`:

| Fact | Evidence |
|---|---|
| 0 git tags | `git tag \| wc -l` |
| version is a bare string with `"unknown"` fallback | `project.godot:11`, `src/Host/HostCli.cs:521–527` |
| no changelog/release doc | `find … -iname "*changelog*"` → only `VersionReportContractTests.cs` |
| version *report* contract is real | `Assets/Ashfall.Core/VersionReport.cs:60`; `VersionReportContractTests` |
| 46 CI gates / 45 fast | `docs/ci/CI_GATE_MANIFEST.json` |
| `release-gate.sh` absent | proposed in Wave 5's 39A |
| export builds unproven | `.github/workflows/build.yml` uses raw `godot --export-release`, no boot step |
| save compatibility window unspecified | codecs V1→V3 exist; no policy or support-window test |
| rulebook describes Bit lane/snap/export | `AGENTS.md` "Saving and Publishing Changes" (workspace-level inherited text; the nested ASHFALL project is git/Godot — noted, not silently rewritten) |

### 2.3 Program surface debt
Plans 01/04/05 add authorities and loops; without this plan they would create
new "reachable but unsurfaced" systems. The existing guards are real and must
be extended, not bypassed: `PanelRouteGateTests`, `PlayerSurfaceCoverageGateTests`,
`PlayerSurfaceLivenessGateTests`, `--panel-bind-lifecycle-selftest`,
`ui-a11y` selftest (237 UI files), the 69-panel snapshot suite.

### 2.4 Truth gates already proven
`generate-port-contract.py --check` (262 seams / 180 HOST_REQUIRED / 0 DEFERRED),
`generate-architecture-map.py --check`, `generate-save-store-matrix.py --check`,
`generate-docs-index.py --check`, `generate-catalog-registry.py --check`,
`--data-integrity-selftest`, `--content-utilization-selftest`,
`--bridge-selftest`, RNG source gate, filename registry gate.

---

## 3. Packages

### F6-1 — Input, Focus, Controller, Rebinding (execute C2[15]/Plan 37)
- **Outcome:** every declared action has a handler; a focus order exists for
  every routed panel; controller bindings exist for the core navigation and
  confirm/close set; a rebinding surface writes the existing settings store;
  a contract gate keeps action list ↔ handler list in parity.
- **Paths:** `project.godot [input]`, `src/Host/AshfallInputActions.cs`,
  `src/Main.GameFlow.cs`, `src/Main.PanelLifecycle.cs`, `src/UI/ModalManager.cs`,
  `src/Settings/*` (bindings only), new `scripts/ci/input-map-gate.sh` (the
  file already exists — extend it), new `Ashfall.Core.Tests/InputMapContractTests.cs`.
- **Acceptance:** 0 uncalled predicates; all `ashfall_nav_*` wired; joypad
  events mapped; focus traversal on every routed panel verified by a headless
  focus probe; rebinding persists and survives restart; existing keyboard
  behavior unchanged.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/InputMapContractTests.cs`;
  `bash scripts/ci/input-map-gate.sh`; `godot --headless --path . -- --panel-bind-lifecycle-selftest`.

### F6-2 — Release Craft (execute C2[21]/Plan 48)
- **Outcome:** one versioning scheme across game/data/save schemas; a
  changelog generated from git history; annotated tags; `release-gate.sh`;
  a hotfix path with checksum-preserving migration; export verified by a boot
  smoke; a written save-compatibility support window with a test naming it.
- **Paths:** `project.godot`, `Assets/Ashfall.Core/VersionReport.cs`,
  `Ashfall.Core.Tests/VersionReportContractTests.cs`, `scripts/release/`
  (existing scripts directory), `scripts/ci/release-gate.sh` (new),
  `.github/workflows/release.yml` + `hotfix.yml`, `CHANGELOG.md` (new),
  `docs/release/*` (new).
- **Acceptance:** `release-gate.sh` runs the fast gate set + export + boot
  smoke; a dry-run tag produces a changelog section; the compatibility window
  is asserted by a test; `build.yml` uses the canonical export script and a
  failing verification cannot pass.
- **Verify:** `bash scripts/ci/release-gate.sh --dry-run`;
  `bash scripts/run_test.sh Ashfall.Core.Tests/VersionReportContractTests.cs`;
  `bash scripts/ci/godot-export-linux.sh` + boot smoke.

### F6-3 — E1/Plan 53 Ambition Governance (complete the existing claim)
- **Outcome:** E1A–E1P packages either completed or returned `STALE_PLAN` with
  current evidence. The standing gate registry (Plan 59,
  `StandingGateRegistry`) becomes the intake surface for every new ambition:
  a proposal is admissible only with an authority map, a consumer, and a gate.
- **Paths:** per `claim-e1-plan53-2026-09-19` in `WORKTREE_OWNERSHIP.md`
  (`scripts/ci/plan_governance_contract.py`, `PlanGovernanceContractTests.cs`
  and the E1 docs tree).
- **Acceptance:** every E1 package has a verdict; the governance contract test
  stays green; no governance engine is host-unreachable at the end
  (`ContentOrphanCertificationEngine` is resolved per PLAN-UNBLOCK-03 U5).
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/PlanGovernanceContractTests.cs`.

### F6-4 — Player surface coverage for the whole programme
- **Outcome:** every authority integrated by Plans 01/04/05 is either a routed
  panel region, a day-event/journal consequence, or a CLI-only authority with
  a written reason. No "invisible" integrated system.
- **Paths:** `src/Main.PlayerSurfaces.cs`, `Main.UiPanels.cs`,
  `Main.PanelLifecycle.cs`, existing panels (extend, do not proliferate),
  `Ashfall.Core.Tests/UI/PlayerSurfaceCoverageGateTests.cs`,
  `PlayerSurfaceLivenessGateTests.cs`, snapshot suite.
- **Acceptance:** coverage gate green over the expanded manifest; liveness gate
  proves each route opens a live read model (not a stale copy); 69 snapshots
  re-rendered and reviewed for the changed panels; a11y selftest green.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/UI/`;
  `godot --headless --path . -- --panel-bind-lifecycle-selftest`;
  `godot --headless --path . -- --ui-a11y-selftest`.

### F6-5 — Store and capability truth
- **Purpose:** the ledger is full of "integrated and sealed" claims; store
  text must not run ahead of the build.
- **Outcome:** `StoreCapabilityManifest` (Plan 57, `store_capability_claims.json`)
  is wired to a CLI audit that checks each claim against live systems and
  passing gates; the store kit cannot name an unbacked capability.
- **Paths:** `Assets/Ashfall.Core/Launch/StoreCapabilityManifest.cs` bind,
  `src/Host/HostCli.Store.cs` (or existing host CLI), claim catalog, focused
  tests.
- **Acceptance:** every claim has a named live proof (authority + host path +
  gate); unbacked claims are rejected, not softened; audit output is a report
  artifact.
- **Verify:** `godot --headless --path . -- --store-truth-selftest`.

### F6-6 — Final truth closure
- **Outcome:** the programme's own gates are the acceptance record:
  - reachability: 0 unexplained orphans (allowlist empty or justified);
  - register: 0 non-terminal rows without a condition;
  - ledger: 0 rows using "integrated" without a host path;
  - save matrix, catalog registry, architecture map, docs index all `--check` green;
  - rulebooks synced (`sync-agent-rulebooks.py`) after any `AGENTS.md` update;
  - snapshot, a11y, input, port-contract, RNG, filename gates green;
  - one bounded soak (e.g. 7-day and 30-day seeded runs) with replay equality.
- **Paths:** ledgers (foreman), CI manifest, docs.
- **Acceptance:** a signed closeout document citing every gate output and the
  command that produced it.
- **Verify:**
  ```bash
  python3 scripts/ci/generate-authority-reachability.py --check
  python3 scripts/ci/check-ledger-truth.py --check
  python3 scripts/ci/check-register-truth.py --check
  python3 scripts/ci/generate-save-store-matrix.py --check
  python3 scripts/ci/generate-catalog-registry.py --check
  python3 scripts/ci/generate-architecture-map.py --check
  python3 scripts/ci/generate-docs-index.py --check
  python3 scripts/ci/generate-port-contract.py --check
  python3 scripts/ci/agent-fast-verify.py
  ```

---

## 4. Sequencing and dependencies

```
KIT (02) ─▶ ORPHAN (01) ─▶ CULTURE (04) ─┐
                       └▶ BODY/IND (05) ─┼─▶ FACE (06) F6-4/F6-6
UNBLOCK (03) ─────────────────────────────┘
FACE F6-1/F6-2/F6-3 may run in parallel with 04/05 (different paths).
```

Hard order constraints:
- F6-4 requires the manifest expansion (K2/K5) and at least the culture and
  industry day owners so liveness probes have live data.
- F6-2 (release) should land before any external build is handed out; it does
  not depend on 04/05 content.
- F6-5 (store truth) must land after F6-4 and F6-6 gates exist, or its claims
  would be audited against an incomplete registry.

## 5. Risk register

| Risk | Mitigation |
|---|---|
| Focus/controller work touches every panel | one dispatcher + one focus policy module; per-panel changes are additive `FocusNeighbor` declarations; probe proves traversal |
| Release work blocked by red gates | Plan 02 ledger truth first; `release-gate.sh` runs the fast set only, quarantine stays real-file-only |
| Store claims overreach | F6-5 is a rejection gate, not a marketing checklist |
| Surface proliferation | extend existing panels; new routes need a descriptor + gate + liveness proof |
| Snapshot churn | re-render only changed panels; review diffs; no mass rewrite |
| Scope drift into localization/VO | DEC-11/13 stay deferred; F6 gates hardcoded strings only |

## 6. Rollback

F6-1/F6-2 are independently revertible (input map/settings and release tooling).
F6-4/F6-5/F6-6 are read-only gates plus descriptor additions; a bad route can
be removed without save impact. No save migration is introduced by this plan
except the release compatibility policy, which is documentation + a test.

## 7. Final acceptance (program-level "sealed" definition)

The expansion programme is sealed when all of the following are true and
evidenced in one closeout:

1. Every authority is `reachable`, `core_only_declared`, or `retired`.
2. `--integration-selftest` passes over the complete manifest.
3. Culture and body/industry verticals each have a seeded 30-day replay equal
   across a mid-run save/reload.
4. Input actions ↔ handlers parity and focus traversal probes pass.
5. A dry-run release produces version + changelog + tag + boot smoke.
6. Store claims audit rejects unbacked claims.
7. Ledger, register, debt, and rulebook texts agree with the gates.

---

## 6. Expanded census (bespoke: launch surface)

| Metric | Value |
|---|---:|
| Declared input actions | 22 |
| Player surface route ids | 192 |
| `Main.*` partials | 146 |

**Actions:** `ashfall_close`, `ashfall_confirm`, `ashfall_events`, `ashfall_expeditions`, `ashfall_forecast`, `ashfall_guidance`, `ashfall_help`, `ashfall_holdfast`, `ashfall_holdfast_build`, `ashfall_holdfast_status`, `ashfall_journal`, `ashfall_journal_tab_1`, `ashfall_journal_tab_2`, `ashfall_journal_tab_3`, `ashfall_journal_tab_4`, `ashfall_journal_tab_5`, `ashfall_nav_down`, `ashfall_nav_left`, `ashfall_nav_right`, `ashfall_nav_up`, `ashfall_next_tab`, `ashfall_weather_history`

## 7. Expanded surface: launch contract

| Rule | Detail |
|---|---|
| First-run | new game reaches a playable state without detours |
| Input | every declared action has a handler (Plan 25 parity) |
| Surfaces | every route resolves to a panel or is retired |
| Truth | the launch screen shows real state (save presence, versions) |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Action parity | declared == handled |
| Route parity | route id ↔ panel registry |
| First-run | scripted smoke reaches day 1 |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section).
2. Parity checks (actions, routes).
3. First-run smoke.
4. Regression: parity re-run.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Action | handled; no dead binding |
| Route | resolves or retired |
| First-run | playable without manual steps |
| Screen | shows real state |

**Non-goals unchanged:** the launch face composes existing surfaces.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 32. Other plans referencing them: **27**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-RELEASE-OPS-20` | 6 |
| `PLAN-UNBLOCK-03` | 4 |
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `PLAN-INPUT-HARDENING-25` | 3 |
| `PLAN-AGENT-WORKFLOW-GOVERNANCE-59` | 3 |
| `PLAN-HOST-COMPOSITION-GOVERNANCE-71` | 3 |
| `PLAN-INTEGRATION-KIT-02` | 2 |
| `EVIDENCE` | 2 |

**Governed artifacts (first 12):**

| Path |
|---|
| `AGENTS.md` |
| `Ashfall.Core.Tests/InputMapContractTests.cs` |
| `Ashfall.Core.Tests/UI/PlayerSurfaceCoverageGateTests.cs` |
| `Ashfall.Core.Tests/VersionReportContractTests.cs` |
| `Assets/Ashfall.Core/Launch/StoreCapabilityManifest.cs` |
| `Assets/Ashfall.Core/VersionReport.cs` |
| `CHANGELOG.md` |
| `Main.PanelLifecycle.cs` |
| `Main.PlayerSurfaces.cs` |
| `Main.UiPanels.cs` |
| `Next-steps-plans/shipped_to_chat/Plan_37_Hands_On_The_Wheel_Input_Focus_Controller.md` |
| `Next-steps-plans/shipped_to_chat/Plan_48_Release_Craft_Versioning_Changelog_Hotfix.md` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `F6-1` | `Next-steps-plans/shipped_to_chat/Plan_37_Hands_On_The_Wheel_Input_Focus_Controller.md`, `Ashfall.Core.Tests/InputMapContractTests.cs`, `PLAN-LAUNCH-FACE-06_APPENDIX-A_INPUT_ACTIONS.md` |
| `F6-2` | `Next-steps-plans/shipped_to_chat/Plan_48_Release_Craft_Versioning_Changelog_Hotfix.md`, `release-gate.sh`, `scripts/ci/release-gate.sh` |
| `F6-3` | `PlanGovernanceContractTests.cs`, `scripts/ci/plan_governance_contract.py`, `store_capability_claims.json` |
| `F6-4` | `Ashfall.Core.Tests/UI/PlayerSurfaceCoverageGateTests.cs`, `Main.PlayerSurfaces.cs`, `PlayerSurfaceLivenessGateTests.cs` |
| `F6-5` | `Assets/Ashfall.Core/Launch/StoreCapabilityManifest.cs`, `store_capability_claims.json`, `src/Host/HostCli.Store.cs` |
| `F6-6` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 28. Host files: **58** · Test files: **49** · Data files: **39**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 58 | `src/Audio/AudioSelfTest.cs`, `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/AshfallInputActions.cs`, `src/Host/CatalogPath.cs`, `src/Host/ChemicalReconHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 49 | `Ashfall.Core.Tests/ActionResultTests.cs`, `Ashfall.Core.Tests/CatalogIntegrityValidatorTests.cs`, `Ashfall.Core.Tests/Codex/CodexProjectionTests.cs`, `Ashfall.Core.Tests/CollectibleNarrativeQualityTests.cs`, `Ashfall.Core.Tests/DataRuleComplianceTests.cs` |
| Data (`StreamingAssets/Data/`) | 39 | `Assets/StreamingAssets/Data/bunker_graffiti_postings.json`, `Assets/StreamingAssets/Data/confession_secrets.json`, `Assets/StreamingAssets/Data/currents.json`, `Assets/StreamingAssets/Data/dose_quests.json`, `Assets/StreamingAssets/Data/economy_goods.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **17** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `collectible_discovery` |
| `dose_ledger` |
| `dynamic_quests` |
| `economy` |
| `foundry` |
| `narrative` |
| `narrative_questlines` |
| `nuclear_core_lifecycle` |
| `personal_quests` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **29** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--asset-coverage-report` |
| `--audio-selftest` |
| `--audio-test` |
| `--chemical-dependency-save-selftest` |
| `--data-integrity-selftest` |
| `--dose-ledger-selftest` |
| `--dose-uitest` |
| `--economy-selftest` |
| `--economy-uitest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **21**.

| Event | First declaration |
|---|---|
| `OnActionCompleted` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |
| `OnActionExecuted` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnActionResolved` | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` |
| `OnActionSelected` | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` |
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnCodexUnlocked` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` |
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractRenegotiated` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractSigned` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnCraftCompleted` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/autonomy_actions.json` |
| `Assets/StreamingAssets/Data/bounty_board.json` |
| `Assets/StreamingAssets/Data/bunker_graffiti_postings.json` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/codex_entries.json` |
| `Assets/StreamingAssets/Data/confession_secrets.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **17** (204 files, 1712 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Codex` | 2 | 29 |
| `Crafting` | 1 | 11 |
| `Economy` | 41 | 329 |
| `Foundry` | 8 | 73 |
| `Governance` | 5 | 27 |
| `InformationFlow` | 3 | 19 |
| `Integration` | 16 | 74 |

**Verdict:** 1712 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Balance`, `Codex`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **796**
(233 of them panels/HUD).

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
| `src/Audio/SurfaceAmbienceController.cs` |
| `src/Disease/DiseaseHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **39**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `collectible_discovery` | no |
| `crafting` | no |
| `disease` | no |
| `dose_ledger` | yes |
| `dynamic_quests` | no |
| `economy` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **15**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `cupola_foundry` |
| `disease` |
| `economy` |
| `foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **362**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 56, OPTIONAL 5, UNRESOLVED 22).

| Catalog | Classification |
|---|---|
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `bounty_board.json` | GAMEPLAY_CONSUMED |
| `bunker_graffiti_postings.json` | UNRESOLVED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `codex_entries.json` | UNRESOLVED |
| `confession_secrets.json` | OPTIONAL |

**Verdict:** 22 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **4**.

| Flag |
|---|
| `flag_become_warlord` |
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_honored_debt` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** governance · **Coupling (incoming plans):** 27
**Surface:** save sections 39 (laddered 1) · RNG streams 15 · host files 26 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-LAUNCH-FACE-06
wave: —
status: PROPOSED — foreman claim required
packages: author package list at claim time
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/audio_accessibility_cues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - coordinate: 27 other plan(s) name these artifacts (§12)
  - wave seed — claim before dependent plans
  - touches 1 versioned save ladder(s) — extend, never fork
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
| packages | **no** |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** author or confirm: packages.
