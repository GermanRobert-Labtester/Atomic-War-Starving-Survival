# PLAN-SETTINGS-INTEGRITY-54 — Settings Schema, Migration, Defaults & Corruption Recovery

**Wave 6 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SAVE-GOVERNANCE-12, PLAN-INPUT-HARDENING-25.
**Non-goals:** no cloud settings, no account system, no telemetry-backed config.

## Outcome
Settings are player state and a support surface: `UserSettings` +
`UserSettingsStore`, `UserSettingsData`/`UserSettingsCodec` in Core,
`AccessibilityPresentation`, `ColorblindColorMapper`, `KeyBindingApplicator`.
There is no declared schema version policy, no corruption recovery story, and
no settings-drift gate. This plan closes that.

| Deliverable | Detail |
|---|---|
| Schema | versioned settings document; additive-only fields; unknown fields preserved or ignored explicitly |
| Migration | per-version migration table; defaults documented per field; no silent value change |
| Corruption recovery | invalid settings reset to safe defaults with a typed notice and a backup of the bad file |
| Defaults truth | one defaults table used by tests and the reset button; no scattered literals |
| Drift gate | every setting is read somewhere; every read exists in the schema (ties to PLAN-DATA-CONSUMER-22) |
| Platform split | input/save-path settings are platform-scoped; display/audio settings portable |

## Evidence
- `src/Settings/UserSettings.cs` (+ `.uid`), `AccessibilityPresentation`, `KeyBindingApplicator`.
- Core: `Settings/UserSettingsData.cs`, `UserSettingsCodec.cs`, `ColorblindColorMapper.cs`, `Accessibility/AccessibilitySettingsSystem.cs` (host-unreachable).
- `--settings-selftest` asserts four accessibility flag effects (Path α seal).
- `DEC-11`/`DEC-13` conditions depend on settings-adjacent surfaces.

## Packages
- **ST-54A** schema + version: one document, version field, additive policy, documented defaults.
- **ST-54B** migration table: fixture from each historical shape → current; no field silently resets without a notice.
- **ST-54C** corruption recovery: malformed/truncated/oversized settings load safe; the bad file is preserved for support.
- **ST-54D** defaults table: single source; reset restores exactly it; test compares.
- **ST-54E** drift gate: unread setting or unlisted read fails CI with the field name.

## Acceptance & verification
- Legacy fixture loads with expected values; corrupt file recovers with a notice.
- `godot --headless --path . -- --settings-selftest`; `bash scripts/run_test.sh Ashfall.Core.Tests/Settings/`; the drift gate.

## Risks
Migration overreach → fixtures pin each historic shape; only additive changes allowed.

---

## 6. Expanded census (4 files · 741 lines)

Scope: `Assets/Ashfall.Core/Settings/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
DTO/Type 2 · Support 2

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `AudioSettingsCodec.cs` | 189 | Support | **yes** | 0 | 0 | 0 |
| `AudioSettingsData.cs` | 175 | DTO/Type | **yes** | 0 | 0 | 0 |
| `UserSettingsCodec.cs` | 231 | Support | — | 0 | 0 | 0 |
| `UserSettingsData.cs` | 146 | DTO/Type | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Settings/` |
| Test references | 8 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
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

Domain method: plan-body `.cs` enumeration.
Domain files: 5. Other plans referencing them: **6**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-INPUT-REBINDING-106` | 4 |
| `PLAN-ACCESSIBILITY-CLOSURE-51` | 2 |
| `PLAN-PLATFORM-PARITY-53` | 2 |
| `PLAN-AUDIO-MIX-AUTHORITY-97` | 2 |
| `PLAN-AUDIO-CONDITION-TRUTH-255` | 2 |
| `PLAN-UI-SURFACE-15` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `ST-54A` | no name match — resolve at claim time |
| `ST-54B` | no name match — resolve at claim time |
| `ST-54C` | `AudioSettingsCodec.cs`, `AudioSettingsData.cs`, `UserSettingsCodec.cs` |
| `ST-54D` | no name match — resolve at claim time |
| `ST-54E` | `AudioSettingsCodec.cs`, `AudioSettingsData.cs`, `UserSettingsCodec.cs` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 5. Host files: **9** · Test files: **5** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 9 | `src/Audio/AudioSelfTest.cs`, `src/Audio/AudioSettings.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Main.Onboarding.cs`, `src/Settings/AccessibilityPresentation.cs` |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/Audio/AudioSettingsRecoveryTests.cs`, `Ashfall.Core.Tests/Localization/LocalizationPilotTests.cs`, `Ashfall.Core.Tests/Localization/LocalizationServiceTests.cs`, `Ashfall.Core.Tests/Settings/ColorblindColorMapperTests.cs`, `Ashfall.Core.Tests/Settings/UserSettingsRecoveryTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **5**; isolated: **0**.

| From | → To |
|---|---|
| `AudioSettingsCodec` | `AudioSettingsData` |
| `ColorblindColorMapper` | `UserSettingsData` |
| `UserSettingsCodec` | `ColorblindColorMapper` |
| `UserSettingsCodec` | `UserSettingsData` |
| `UserSettingsData` | `ColorblindColorMapper` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

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

Matching flags in `HostCliRegistry.cs`: **4** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--audio-selftest` |
| `--audio-test` |
| `--data-integrity-selftest` |
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

Catalog JSON files whose names share a domain token: **4**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/shelter_audio_cues.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (5 files, 28 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |

**Verdict:** 28 cases sit under matching regions — run those first (`Audio`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **12**
(0 of them panels/HUD).

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
| `src/Main.Audio.cs` |
| `src/Settings/UserSettings.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **0**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| — | no section key shares a token with this domain |

**Verdict:** no save section matches — persistence is owned under a differently-named section, or absent.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **2**
(OPTIONAL 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 6
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 12 · catalogs 6 · test regions 1 · flags 4

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SETTINGS-INTEGRITY-54
wave: 6
status: PROPOSED — foreman claim required
packages: ST-54A, ST-54B, ST-54C, ST-54D, ST-54E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/audio_accessibility_cues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/audio_cues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --audio-selftest
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
