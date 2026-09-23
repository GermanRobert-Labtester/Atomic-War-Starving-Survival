# PLAN-SILENT-FAILURE-35 — Typed Errors, Diagnostics & Observable Failure

**Wave:** 4 (2026-09-21) · **Kind:** GAP SEALING
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-INPUT-HARDENING-25, PLAN-SAVE-GOVERNANCE-12, PLAN-RELEASE-OPS-20.
**Expanded appendix:** [`PLAN-SILENT-FAILURE-35_APPENDIX-A_CATCH_INVENTORY.md`](PLAN-SILENT-FAILURE-35_APPENDIX-A_CATCH_INVENTORY.md)
— the full catch-site census across Core+host (**692 sites**, classified by
body: 1 `SWALLOW_EMPTY` · 285 `LOG_AND_CONTINUE` · 159 `PROPAGATE` · 247
`OTHER` needing review — a more precise measure than the plan's original
grep estimate). SF-35A/35B work from this inventory.
**Non-goals:** no crash reporter service, no network telemetry, no PII, no
exception-shape rewrite.

---

## 1. Outcome

The repo has a catch policy gate and a telemetry recorder, but the audit still
finds **13 fully bare `catch {}` blocks and 40 narrow `catch (Exception) {}`
blocks** in Core/host, and the integration ledger repeatedly records the same
class of defect: a system that *compiled*, *ran*, and *did nothing* — the
methane producer with no source, the telemetry wind gate hardcoded `0f`, a
audio bridge never instantiated. Those are silent failures that no test caught
until a forensic sweep.

This plan makes failure *typed, logged, visible, and countable*.

Deliverables:

1. a **silent-failure inventory**: every catch, ignored result, null-return
   contract, and swallowed error, with a verdict;
2. a **typed error policy**: Core returns `ActionResult`/typed errors; host
   adapters decide presentation; player-initiated failures are always visible;
3. a **diagnostics bundle**: one CLI command producing version report, save
   metadata, catalog integrity summary, recent errors, and system info — with
   no secrets or PII;
4. a **local error budget**: counts of typed failures per session, used by
   balance/QA rather than shipped anywhere;
5. **user-visible failure surfaces** for load/save/mod/catalog faults.

---

## 2. Evidence (2026-09-21)

| Fact | Value | Command |
|---|---:|---|
| Fully bare catches (`catch {`) | 13 | `grep -rn` in Core+src |
| Narrow catches (`catch (Exception) {`) | 40 | grep |
| Catch policy gate | `catch-policy-gate.sh` | `scripts/ci` |
| Typed result type | `ActionResult` (Core) | `Assets/Ashfall.Core/ActionResult.cs` |
| Logging | `ILog`, `NullLog`, `GodotLog` | Core/host |
| Telemetry | `PlaySessionRecorder` JSONL under `user://`, zero network, zero PII | Plan 46 evidence |
| Forensic skills | `ashfall-problem-identifier`, `ashfall-silent` | `.agents/skills` |
| Prior silent-failure repairs | methane producer, recon wind gate, audio bridge | INTEGRATION_PLANS 2026-09-20 |

---

## 3. Packages

### SF-35A — Silent-failure inventory
- Generate `docs/architecture/SILENT_FAILURE_INVENTORY.md`:
  `site | kind (catch_swallow, ignored_result, null_contract, default_fallback)
  | owner | verdict (fix, typed, allowlisted) | reason`.
- Include ignored `ActionResult`, `TryGet*` results discarded, `?? default`
  on load paths, and empty event handlers.
- **Acceptance:** every catch site classified; allowlist requires a reason and
  owner; the catch-policy gate reads the inventory.
- **Verify:** generator `--check` + `bash scripts/ci/catch-policy-gate.sh`.

### SF-35B — Typed error policy
- Rule: Core methods that can fail for a gameplay reason return
  `ActionResult`/typed result; exceptions are for programming errors; adapters
  log at a severity and choose presentation. No catch-and-continue without a
  typed result or an inventory entry.
- Convert the 53 catch sites family by family (catalog loaders, save codecs,
  host adapters).
- **Acceptance:** converted sites return/propagate typed failures; focused
  tests assert the failure branch; no behavior change on the success path.
- **Verify:** focused suites per family.

### SF-35C — Diagnostics bundle
- `--diagnostics-bundle <path>` collects: `VersionReport`, save-store matrix
  summary, catalog integrity summary, module/host status, last N typed errors,
  platform info, and the session recorder tail — **no secrets, no PII, no
  full save contents**.
- **Acceptance:** bundle produced in a scratch dir; a scan proves no secret
  patterns; the bundle is sufficient to explain a user's reported version.
- **Verify:** `godot --headless --path . -- --diagnostics-bundle /tmp/ashfall-diag`
  + the secret scan gate.

### SF-35D — Local error budget
- Count typed failures per session/domain in the recorder; expose a summary in
  the diagnostics bundle and the QA surface. Used for triage; never uploaded.
- **Acceptance:** counts are deterministic per seed; a test proves a forced
  failure increments the right counter.

### SF-35E — User-visible failure surfaces
- For player-visible operations (save/load, mod load, catalog integrity at
  startup), a typed notice replaces silent defaults: modal/toast + journal line
  with the failing subsystem; the game continues only when safe.
- **Acceptance:** a corrupt save and a bad mod each produce a visible, typed
  message and a safe state; the save/load failure selftest extends to cover
  them.
- **Verify:** `godot --headless --path . -- --save-load-ui-failure-selftest`.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Catch conversion overflows scope | family batches; success-path behavior pinned first |
| Diagnostics leak secrets/PII | explicit allowlist of fields; secret scan; bundle excludes full saves |
| Error counts become noise | counted by domain and severity; surfaced only in diagnostics |
| Visible errors annoy on optional content | visible only for player-initiated or safety-critical failures |

## 5. Verification

```bash
python3 scripts/ci/generate-silent-failure-inventory.py --check
bash scripts/ci/catch-policy-gate.sh
godot --headless --path . -- --diagnostics-bundle /tmp/ashfall-diag
godot --headless --path . -- --save-load-ui-failure-selftest
python3 scripts/ci/secret-scan.py --check
```

---

## 6. Expanded census (bespoke: failure surface)

This plan hunts silent failures, so the census counts catch sites by shape
across Core.

| Metric | Value |
|---|---:|
| Files with catch | 275 |
| Catch clauses | 461 |
| Empty catches | 20 |
| Swallow-and-return catches | 1 |

**Worst files (empty + swallow):**

| File | Empty | Swallow | Total catch |
|---|---:|---:|---:|
| `HostDefaults.cs` | 0 | 1 | 2 |
| `Expeditions/DiscoveryConsequenceSystem.cs` | 1 | 0 | 1 |
| `Medical/AfflictionQuestWorkBridge.cs` | 1 | 0 | 1 |
| `Medical/ChronicConditionSystem.cs` | 1 | 0 | 1 |
| `Narrative/NpcMemorySystem.cs` | 1 | 0 | 1 |
| `Shelter/ShelterMaintenanceSystem.cs` | 1 | 0 | 1 |
| `Survivors/ChildDevelopmentSystem.cs` | 1 | 0 | 1 |
| `Survivors/BackstorySystem.cs` | 1 | 0 | 1 |
| `Survivors/AgingSystem.cs` | 1 | 0 | 1 |
| `Survivors/SurvivorRoutineSystem.cs` | 1 | 0 | 1 |
| `Survivors/SurvivorRoleSystem.cs` | 1 | 0 | 1 |
| `Combat/CombatFactionStandingBridge.cs` | 1 | 0 | 1 |
| `Culture/CultureCreationSystem.cs` | 1 | 0 | 1 |
| `Difficulty/DifficultySettingsSystem.cs` | 1 | 0 | 1 |
| `Cognition/MemoryDecaySystem.cs` | 1 | 0 | 1 |
| `InformationFlow/RumorSystem.cs` | 1 | 0 | 1 |
| `Water/WaterSourceSystem.cs` | 1 | 0 | 1 |
| `Psychology/PsychologicalProfileSystem.cs` | 1 | 0 | 1 |
| `Accessibility/AccessibilitySettingsSystem.cs` | 1 | 0 | 1 |
| `Emergency/EmergencyAlertSystem.cs` | 1 | 0 | 1 |

## 7. Expanded surface: failure contract

| Rule | Detail |
|---|---|
| Typed failures | a failure names its cause; no bare catch that continues silently |
| Visible in dev | dev builds log/report; player builds degrade per a documented fallback |
| No half-state | a failed operation leaves a consistent state or rolls back |
| Gate | a new empty catch fails the static scan |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Static scan | empty/swallow counts above; target: each classified |
| Focused fixtures | forcing the failure path yields a typed report |
| Regression | counts compared per batch |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section) and classification of every row.
2. Add typed reports to the worst offenders.
3. Gate for new empty catches.
4. Regression: counts decrease batch over batch.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Empty catch | removed or justified with a comment and a fallback |
| Swallow-return | replaced with a typed result or a logged default |
| Gate | fails on a new empty catch |
| Fixture | forced failure produces a visible report |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not rewrite control flow.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 24. Other plans referencing them: **27**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `EVIDENCE` | 12 |
| `PLAN-SURVIVORS-FAMILY-TRUTH-264` | 3 |
| `PLAN-MEDICAL-FAMILY-TRUTH-263` | 2 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `PLAN-RELEASE-OPS-20` | 1 |
| `PLAN-INPUT-HARDENING-25` | 1 |
| `PLAN-TRANSPORT-EXPEDITION-30` | 1 |
| `PLAN-ARCHITECTURE-BOUNDARY-31` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Accessibility/AccessibilitySettingsSystem.cs` |
| `Assets/Ashfall.Core/ActionResult.cs` |
| `Cognition/MemoryDecaySystem.cs` |
| `Combat/CombatFactionStandingBridge.cs` |
| `Culture/CultureCreationSystem.cs` |
| `Difficulty/DifficultySettingsSystem.cs` |
| `Emergency/EmergencyAlertSystem.cs` |
| `Expeditions/DiscoveryConsequenceSystem.cs` |
| `HostDefaults.cs` |
| `InformationFlow/RumorSystem.cs` |
| `Medical/AfflictionQuestWorkBridge.cs` |
| `Medical/ChronicConditionSystem.cs` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `SF-35A` | `PLAN-SILENT-FAILURE-35_APPENDIX-A_CATCH_INVENTORY.md`, `docs/architecture/SILENT_FAILURE_INVENTORY.md` |
| `SF-35B` | `catch-policy-gate.sh` |
| `SF-35C` | no name match — resolve at claim time |
| `SF-35D` | no name match — resolve at claim time |
| `SF-35E` | `PLAN-SILENT-FAILURE-35_APPENDIX-A_CATCH_INVENTORY.md`, `docs/architecture/SILENT_FAILURE_INVENTORY.md` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 24. Host files: **70** · Test files: **139** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 70 | `src/Host/AirlockSecurityHostSession.cs`, `src/Host/ApprenticeshipHostSession.cs`, `src/Host/ArchiveDeskHostSession.cs`, `src/Host/AutopsyHostSession.cs`, `src/Host/BallisticShieldHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 139 | `Ashfall.Core.Tests/Accessibility/Plan184AccessibilitySettingsIntegrationTests.cs`, `Ashfall.Core.Tests/ActionResultTests.cs`, `Ashfall.Core.Tests/AirlockSecuritySystemTests.cs`, `Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`, `Ashfall.Core.Tests/ArchiveDeskSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **44** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `airlock_security` |
| `apprenticeship` |
| `archive_desk` |
| `autopsy` |
| `ballistic_shield` |
| `black_projects_archive` |
| `child_development` |
| `collectible_discovery` |
| `combat` |
| `equipment_condition` |
| `expanded_shelter` |
| `expansion_quest` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **33** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--accessibility-selftest` |
| `--combat-breaching-selftest` |
| `--combat-selftest` |
| `--difficulty-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--inventory-save-selftest` |
| `--inventory-selftest` |
| `--inventory-uitest` |
| `--personal-quest-selftest` |
| `--relationship-decay-selftest` |
| `--rumor-network-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **45**.

| Event | First declaration |
|---|---|
| `OnActionCompleted` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |
| `OnActionExecuted` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnActionResolved` | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` |
| `OnActionSelected` | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` |
| `OnArchiveChanged` | `Assets/Ashfall.Core/ArchiveDeskSystem.cs` |
| `OnAutopsyChanged` | `Assets/Ashfall.Core/AutopsySystem.cs` |
| `OnChildBooked` | `Assets/Ashfall.Core/CohortSystem.cs` |
| `OnChronicFibrosisMarked` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnChronicIllnessRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCombatPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnCombatPerkEarned` | `Assets/Ashfall.Core/Combat/CombatPerks.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/accessibility_profiles.json` |
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` |
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/apprenticeship_catalog.json` |
| `Assets/StreamingAssets/Data/archive_categories.json` |
| `Assets/StreamingAssets/Data/archive_inks.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/autopsy_procedures.json` |
| `Assets/StreamingAssets/Data/backstory_templates.json` |
| `Assets/StreamingAssets/Data/ballistic_shield_catalog.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/bounty_board.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **15** (183 files, 1422 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Combat` | 10 | 84 |
| `Culture` | 7 | 40 |
| `Difficulty` | 5 | 24 |
| `Emergency` | 1 | 6 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Inventory` | 15 | 114 |
| `NarrativeConsequence` | 1 | 20 |

**Verdict:** 1422 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Combat`, `Culture`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **287**
(60 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ApprenticeshipSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **55**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `autopsy` | no |
| `ballistic_shield` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `chemical_dependency` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **17**.

| Stream |
|---|
| `acoustic_detection` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **101**
(CODEX_ONLY 40, GAMEPLAY_CONSUMED 40, OPTIONAL 5, UNRESOLVED 16).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `bounty_board.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |

**Verdict:** 16 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **5**.

| Flag |
|---|
| `flag_become_warlord` |
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_expelled_survivor` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** hub · **Coupling (incoming plans):** 27
**Surface:** save sections 55 (laddered 0) · RNG streams 17 · host files 27 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SILENT-FAILURE-35
wave: —
status: PROPOSED — foreman claim required
packages: SF-35A, SF-35B, SF-35C, SF-35D, SF-35E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/accessibility_profiles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --accessibility-selftest
dependencies:
  - coordinate: 27 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
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

**Pre-claim actions:** author or confirm: wave.
