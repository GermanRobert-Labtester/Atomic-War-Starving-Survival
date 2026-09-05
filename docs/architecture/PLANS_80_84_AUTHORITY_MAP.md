# PLANS 80–84 (TASKS B21–B25) AUTHORITY MAP

**Wave Target:** Flagship Player Experience, Runtime Hardening & Campaign Closure
**Baseline Commit SHA:** `d37406a765af964c4ee8176ccee9cc8413cd5389`
**Baseline Tests:** 7,110 passed, 0 failed
**Baseline Save Section Count:** 109 sections registered in `Ashfall.Core.Save.SaveSectionRegistry` (includes `personal_quests`, `endgame`, `shelter_fire`, advanced shelter, collectibles)
**Date:** 2026-09-05 (audit #40 refresh)

---

## 1. System Authority Matrix

| Domain | Authority / Single Source of Truth | Location | Adapter / Host Consumer | Invariants & Rules |
|---|---|---|---|---|
| **Theme / Colors / Fonts** | `Ashfall.Core.UI.Theme` | `Assets/Ashfall.Core/UI/Theme.cs` | `AshfallUiHelpers.cs`, `project.godot` | Zero engine imports in Core. Contrast meets WCAG AA body text floor. No out-of-authority color literals. |
| **Focus Policy** | `AshfallFocusPolicy` / `ModalManager` | `src/UI/AshfallFocusPolicy.cs`, `src/UI/ModalManager.cs` | All panels (`src/UI/`) | Deterministic initial focus on open, focus trap in overlays, visible focus rings, focus restoration on close. |
| **InputMap / Settings** | `AshfallInputActions` & `UserSettingsStore` | `src/Host/AshfallInputActions.cs`, `Assets/Ashfall.Core/Settings/UserSettingsData.cs` | `src/Settings/UserSettings.cs`, `SettingsPanel.cs` | Canonical verb model (`ui_close`, `ashfall_close`, etc.). Rebinding stored in user settings, decoupled from campaign saves. Safe fallback on corrupt settings. |
| **Panel Input Routing** | Shared Input Router / `AshfallInputActions` | `src/Host/AshfallInputActions.cs`, `src/UI/ModalManager.cs` | Panel `_UnhandledKeyInput` / `_UnhandledInput` | No raw physical keys (`Key.Escape`, etc.) in gameplay panels. All panel close routes through canonical close verbs. |
| **Runtime-Scale Budgets** | `RuntimeScaleGate` | `src/Host/HostCli.RuntimeScale.cs`, `scripts/ci/run-gates.py` | `--runtime-scale-selftest` | 30d/180d/360d latency and allocation limits. Byte-identical same-seed simulation outcomes strictly preserved across optimizations. |
| **Survivor Traits / Relations / Fates** | `SurvivorRelationsSystem`, `SurvivorSocialSystem`, `SurvivorFateRegistry` | `Assets/Ashfall.Core/Social/`, `Assets/Ashfall.Core/Memorial/` | `SurvivorsHostSession`, `MemorialHostSession` | Living survivor truth in `survivors`. Immutable survivor death records in `survivor_fate`. No shadow state or forked relation math. |
| **Quest Runtime & Personal Quests** | `PersonalQuestSystem` (Core) | `Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs`, `Assets/StreamingAssets/Data/personal_quests.json` | `PersonalQuestHostSession`, `QuestsAtlasPanel.cs` | Data-driven arcs, deterministic offer evaluation using `ISeededRng`, typed conditions/effects routing through owning authorities, death-interrupt to survivor-fate. |
| **Moral Ledger & Moral Choice** | `MoralChoiceSystem`, `MoralDecisionLedger` | `Assets/Ashfall.Core/Events/MoralChoiceSystem.cs` | `HostEventHostSession` | Canonical moral bands and decisions. Personal quests and endgame final acts route choices through this authority. |
| **Memorial System** | `MemorialSystem`, `SurvivorFateRegistry` | `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`, `SurvivorFateRegistry.cs` | `MemorialHostSession`, `MemorialWallPanel.cs` | Fallen survivor ledger and epitaphs. Concluded campaign chronicle references this record without mutating it. |
| **Endgame Facts & Epilogue** | `EndgameSystem`, `EndingRecord`, `EpilogueMatrixRuntime` | `Assets/Ashfall.Core/Endgame/EndgameSystem.cs`, `Assets/StreamingAssets/Data/endings.json` | `EndgameHostSession`, `ChroniclePanel.cs` | Typed, deterministic trigger families. 8 moral ending families. Sourced epilogue lines traceable to campaign facts. Immutable `EndingRecord` upon conclusion. Read-only chronicle mode. |
| **Save Section Registry** | `SaveSectionRegistry` | `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` | `CampaignEnvelopeBuilder`, `SaveStoreHub` | Single versioned campaign envelope (`campaign.json`). **109** registered sections including `personal_quests`, `endgame`, `shelter_fire`, advanced shelter, and collectible/unique-claim sections (audit waves 1–10 + #40). |

---

## 2. Invariant Compliance Checklist

- [x] **Invariant 1 — Zero Engine Coupling in Core**: `Ashfall.Core` contains zero `Godot.*` or `UnityEngine.*` references.
- [x] **Invariant 2 — Ports and Adapters**: System interfaces defined in Core ports, implemented by Host adapters.
- [x] **Invariant 3 — Cross-Host / Campaign Envelope Compatibility**: All save sections use `SaveStore<T>` / `CampaignEnvelopeBuilder` atomic envelopes.
- [x] **Invariant 4 — Determinism**: Pure functions and `ISeededRng` (xorshift64*); no `System.Random`, no `Guid.NewGuid()`.
- [x] **Invariant 5 — No Gameplay Logic in Hosts**: Host sessions and UI panels only handle presentation, input routing, and forwarding.
- [x] **Invariant 6 — Data Authority is JSON**: Authoritative content lives in `Assets/StreamingAssets/Data/` with `schema_version` and snake_case IDs.
