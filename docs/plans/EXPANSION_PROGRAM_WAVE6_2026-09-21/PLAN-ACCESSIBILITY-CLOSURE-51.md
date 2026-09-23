# PLAN-ACCESSIBILITY-CLOSURE-51 — Remap, Assistive Tech, Audio Description & Cognitive Load

**Wave 6 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-LAUNCH-FACE-06 F6-1 (focus/input), PLAN-UI-SURFACE-15.
**Non-goals:** no new settings framework, no external assistive services.

## Outcome
The a11y authority map (`DEBT-184-EXPANDED-A11Y-AUTHORITY-MAP`, RETIRED) closed
floor/colorblind work and left four gaps out of scope: **input remapping,
assistive tech, audio descriptions, cognitive load**. This plan closes them on
the existing settings/input owners.

| Gap | Deliverable | Owner |
|---|---|---|
| Remap | full action rebinding (keyboard + controller), conflict detection, reset | `UserSettings`, `KeyBindingApplicator`, `AshfallInputActions` |
| AT | screen-reader text for panels/modals; UI Automation-style export of labels/roles; narration of critical events | panel descriptors, `AccessibilityPresentation` |
| Audio description | text alternatives for critical audio cues (Plan 169 already emits visuals; add description lines + captions) | audio catalogs + accessibility coordinator |
| Cognitive | reduced-stimulation preset, motion caps, slower modal auto-dismiss, simplified panel density mode | `UserSettings`, `AccessibilitySettingsSystem` |

## Evidence
- `src/Settings/`: `UserSettings`, `UserSettingsStore` pattern, `KeyBindingApplicator`, `AccessibilityPresentation`.
- `Assets/Ashfall.Core/Accessibility/AccessibilitySettingsSystem.cs` (host-unreachable), `Settings/ColorblindColorMapper.cs`, `UserSettingsCodec`.
- `--settings-selftest` asserts four-flag effects; `--ui-a11y-selftest` covers 237 UI files.
- Audio: Plan 169 `AudioAccessibilityCoordinator` (visual notifications, ducking), cue catalogs.
- Retired map lists remap/AT/audio-desc/cognitive as OUT with conditions.

## Packages
- **AC-51A** rebinding: binding tables persisted in settings; conflict detection; default reset; controller glyphs.
- **AC-51B** screen-reader surface: every routed panel exposes label/role/state rows; a probe enumerates them; modal focus order ties to F6-1.
- **AC-51C** audio description/captions: every critical cue gets a caption and a description line; non-critical ambience is explicitly exempt with a reason.
- **AC-51D** cognitive preset: one toggle sets motion caps, density, auto-dismiss, and reduces non-essential notification volume.
- **AC-51E** coverage gate: new panels must declare a11y rows; the selftest fails on missing declarations.

## Acceptance & verification
- Settings persist across restart; a rebinding conflict is explained, not ignored.
- Screen-reader probe covers all routed panels; critical cues all have captions.
- `godot --headless --path . -- --settings-selftest`; `--ui-a11y-selftest`; `bash scripts/run_test.sh Ashfall.Core.Tests/Settings/`; `bash scripts/run_test.sh Ashfall.Core.Tests/UI/`.

## Risks
Over-instrumentation slows panels → declarations are generated and cached; no per-frame narration.

---

## 6. Expanded census (1 files · 261 lines)

Scope: `Assets/Ashfall.Core/Accessibility/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `AccessibilitySettingsSystem.cs` | 261 | System | **yes** | 0 | 1 | 12 |

**Totals:** 0 banned refs · 1 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `audio_accessibility_cues.json` | object[3 keys] |
| `accessibility_profiles.json` | object[3 keys] |

**State surfaces:** `AccessibilitySettingsSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Accessibility/` |
| Test references | 1 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 1 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 5. Other plans referencing them: **6**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-SETTINGS-INTEGRITY-54` | 2 |
| `EVIDENCE` | 1 |
| `PLAN-ASSET-PIPELINE-19` | 1 |
| `PLAN-SILENT-FAILURE-35` | 1 |
| `PLAN-AUDIO-MIX-AUTHORITY-97` | 1 |
| `PLAN-AUDIO-CONDITION-TRUTH-255` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `AccessibilitySettingsSystem.cs` |
| `Assets/Ashfall.Core/Accessibility/AccessibilitySettingsSystem.cs` |
| `Settings/ColorblindColorMapper.cs` |
| `accessibility_profiles.json` |
| `audio_accessibility_cues.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `AC-51A` | `AccessibilitySettingsSystem.cs`, `Assets/Ashfall.Core/Accessibility/AccessibilitySettingsSystem.cs` |
| `AC-51B` | no name match — resolve at claim time |
| `AC-51C` | `audio_accessibility_cues.json` |
| `AC-51D` | no name match — resolve at claim time |
| `AC-51E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 4. Host files: **4** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/HostCli.PanelTests.cs`, `src/Settings/AccessibilityPresentation.cs`, `src/UI/AshfallUiHelpers.cs`, `src/UI/SettingsPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Accessibility/Plan184AccessibilitySettingsIntegrationTests.cs`, `Ashfall.Core.Tests/Audio/Plan169AudioAccessibilityIntegrationTests.cs`, `Ashfall.Core.Tests/Settings/ColorblindColorMapperTests.cs`, `Ashfall.Core.Tests/Settings/UserSettingsRecoveryTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **0** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| — | no section key shares a token with this domain |

**Verdict:** no section key shares a token with this domain — either the authority is derived/stateless, or its persistence key is named after a different owner. Not a conclusion; verify in the owning system.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **11** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--accessibility-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--ui-accessibility-selftest` |
| `--user-data-dir` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/accessibility_profiles.json` |
| `Assets/StreamingAssets/Data/atmosphere_profiles.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/infiltrator_profiles.json` |
| `Assets/StreamingAssets/Data/narrative/survivor_profiles_expansion.json` |
| `Assets/StreamingAssets/Data/nuclear_core_profiles.json` |
| `Assets/StreamingAssets/Data/nutrition_profiles.json` |
| `Assets/StreamingAssets/Data/psychology_profiles.json` |
| `Assets/StreamingAssets/Data/relationship_decay_profiles.json` |
| `Assets/StreamingAssets/Data/shelter_audio_cues.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **7** (152 files, 1201 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Economy` | 41 | 329 |
| `Integration` | 16 | 74 |
| `Nutrition` | 1 | 5 |
| `Presentation` | 1 | 5 |
| `Shelter` | 87 | 754 |

**Verdict:** 1201 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Economy`, `Integration`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **281**
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

Matched save sections: **30**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `caravan_trade_network` | no |
| `economy` | no |
| `equipment_condition` | no |
| `expanded_shelter` | no |
| `holdfast_trade` | no |
| `host_event` | no |
| `kitchen_nutrition` | no |
| `nuclear_core_lifecycle` | no |
| `nutrition` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **9**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `economy` |
| `psychology` |
| `psychology_arc_behavior` |
| `psychology_arc_trigger` |
| `psychology_recovery` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **87**
(CODEX_ONLY 67, GAMEPLAY_CONSUMED 10, OPTIONAL 5, UNRESOLVED 5).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `environmental_atmosphere_expansion.json` | GAMEPLAY_CONSUMED |
| `expansion_survivor_fields.json` | GAMEPLAY_CONSUMED |
| `hardcore_economy_tuning.json` | GAMEPLAY_CONSUMED |
| `infiltrator_profiles.json` | GAMEPLAY_CONSUMED |

**Verdict:** 5 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 6
**Surface:** save sections 30 (laddered 0) · RNG streams 9 · host files 22 · catalogs 22 · test regions 7 · flags 11

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ACCESSIBILITY-CLOSURE-51
wave: 6
status: PROPOSED — foreman claim required
packages: AC-51A, AC-51B, AC-51C, AC-51D, AC-51E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/accessibility_profiles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/atmosphere_profiles.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --accessibility-selftest
dependencies:
  - coordinate: 6 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
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
