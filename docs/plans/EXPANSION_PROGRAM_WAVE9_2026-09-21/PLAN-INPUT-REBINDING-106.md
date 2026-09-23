# PLAN-INPUT-REBINDING-106 — Remap UI, Conflict Detection & Controller Glyphs

**Wave 9 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SETTINGS-INTEGRITY-54, PLAN-INPUT-HARDENING-25, PLAN-LAUNCH-FACE-06.
**Non-goals:** no new input action, no second binding file, no per-panel key
handling; the 22 declared actions from `project.godot` stay the action set.

## 1. Outcome
Plan 6 inventories the **22 declared input actions**; Plan 25 hardens handling;
Plan 54 owns settings persistence. Rebinding is the missing contract: every
action is remappable, conflicts are detected before they are saved, bindings
round-trip through the settings codec, and controller glyphs match the device
in use. Today a player cannot change a binding and be told why one is refused.

| Deliverable | Detail |
|---|---|
| Remap surface | one row per declared action with primary/secondary bindings; grouped by category |
| Conflict detection | same key/button on two actions in the same context → blocked with both action names shown; allowed across distinct contexts only where a context split exists |
| Persistence | bindings round-trip through the existing settings codec; defaults restorable in one action |
| Glyphs | keyboard vs controller glyph set follows the last-used device; unknown device falls back to text labels |
| Reset semantics | per-action, per-category, and full reset — each asserted |

## 2. Evidence
- `project.godot` `[input]`: 22 actions (Plan 6 Appendix A), 4 joypad event references in the current file.
- `Assets/Ashfall.Core/Audio/AudioSettingsCodec.cs` shows the established settings-codec pattern; the input settings follow the same owner seam (Plan 54).
- Plan 25 owns input hardening; Plan 6 owns the action inventory; this plan is the surface between them.

## 3. Packages
- **IRB-106A** action table rendering from the registry-backed declarations.
- **IRB-106B** conflict detector + refusal message (both actions named) and tests.
- **IRB-106C** settings codec round-trip for bindings + defaults reset.
- **IRB-106D** glyph set selection by device class + fallback labels.
- **IRB-106E** focused UI/keyboard-navigation test for the remap surface (Plan 51 patterns).

## 4. Acceptance & verification
- Remapping two actions to one key is refused with both names; remap to a free key saves and survives restart.
- Reset paths return to defaults exactly; codec round-trip byte-stable on repeated save/load.
- Glyph set switches on device change; unknown device shows labels.
- `bash scripts/run_test.sh` on the input/settings region + the focused UI render check.

## 5. Risks
Conflict policy too strict → allowed pairs are listed explicitly per context, and the table is tested.
Binding file as a second authority → bindings persist through the existing settings codec, not a new file.

---

## 6. Expanded census (bespoke: input settings surface)

The plan's subject is the input-binding surface, so the census covers the
declared action set (`project.godot`) and the settings codec files — not source
filenames.

**Declared input actions:** 22 — `ashfall_close`, `ashfall_confirm`, `ashfall_events`, `ashfall_expeditions`, `ashfall_forecast`, `ashfall_guidance`, `ashfall_help`, `ashfall_holdfast`, `ashfall_holdfast_build`, `ashfall_holdfast_status`, `ashfall_journal`, `ashfall_journal_tab_1`, `ashfall_journal_tab_2`, `ashfall_journal_tab_3`, `ashfall_journal_tab_4`, `ashfall_journal_tab_5`, `ashfall_nav_down`, `ashfall_nav_left`, `ashfall_nav_right`, `ashfall_nav_up`, `ashfall_next_tab`, `ashfall_weather_history`
**Joypad events in the input map:** 9.

| Settings file | Lines |
|---|---:|
| `AudioSettingsCodec.cs` | 189 |
| `AudioSettingsData.cs` | 175 |
| `UserSettingsCodec.cs` | 231 |
| `UserSettingsData.cs` | 146 |

## 7. Expanded surface: binding contract

| Rule | Detail |
|---|---|
| Action set | the declared actions are the closed set; no panel-local keys |
| Persistence | bindings round-trip through the settings codec |
| Conflict | same key on two actions in one context → refused with both action names |
| Reset | per-action / per-category / all, each asserted |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Action parity | declared actions == remappable rows in the table |
| Codec round-trip | save → load → save byte-stable |
| Conflict | fixture refuses duplicates with both names |
| Glyphs | device-class switch + unknown-device text fallback |

## 9. Rollout sequence

1. Action table rendered from the declared set.
2. Conflict detector + refusal message.
3. Codec round-trip + reset paths.
4. Glyph set selection.
5. Focused UI/keyboard test.
6. Regression: action parity check.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Action table | every declared action appears exactly once |
| Conflict | refused with both names; no silent overwrite |
| Persistence | round-trips; defaults restorable |
| Glyphs | switch on device change; text fallback |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not add actions.

---

## 12. Cross-plan coupling

Domain method: plan-body `.cs` enumeration.
Domain files: 4. Other plans referencing them: **5**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-SETTINGS-INTEGRITY-54` | 4 |
| `PLAN-PLATFORM-PARITY-53` | 2 |
| `PLAN-AUDIO-MIX-AUTHORITY-97` | 2 |
| `PLAN-AUDIO-CONDITION-TRUTH-255` | 2 |
| `PLAN-ACCESSIBILITY-CLOSURE-51` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `IRB-106A` | no name match — resolve at claim time |
| `IRB-106B` | no name match — resolve at claim time |
| `IRB-106C` | `AudioSettingsCodec.cs`, `UserSettingsCodec.cs`, `AudioSettingsData.cs` |
| `IRB-106D` | no name match — resolve at claim time |
| `IRB-106E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 4. Host files: **8** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 8 | `src/Audio/AudioSelfTest.cs`, `src/Audio/AudioSettings.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Main.Onboarding.cs`, `src/Settings/AccessibilityPresentation.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Audio/AudioSettingsRecoveryTests.cs`, `Ashfall.Core.Tests/Localization/LocalizationPilotTests.cs`, `Ashfall.Core.Tests/Localization/LocalizationServiceTests.cs`, `Ashfall.Core.Tests/Settings/UserSettingsRecoveryTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **2**; isolated: **0**.

| From | → To |
|---|---|
| `AudioSettingsCodec` | `AudioSettingsData` |
| `UserSettingsCodec` | `UserSettingsData` |

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

Matching flags in `HostCliRegistry.cs`: **3** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--audio-selftest` |
| `--audio-test` |
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

Host files (`src/`) whose names share a domain token: **13**
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
| `src/Host/AshfallInputActions.cs` |
| `src/Main.Audio.cs` |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 5
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 12 · catalogs 6 · test regions 1 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-INPUT-REBINDING-106
wave: 9
status: PROPOSED — foreman claim required
packages: IRB-106A, IRB-106B, IRB-106C, IRB-106D, IRB-106E
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
  - coordinate: 5 other plan(s) name these artifacts (§12)
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
