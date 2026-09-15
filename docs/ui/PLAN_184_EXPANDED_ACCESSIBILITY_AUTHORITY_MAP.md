# Plan 184 — Expanded accessibility authority map

**Status:** ACCEPTED — §3 signed 2026-09-12 (colorblind IN, sequenced after Path α)  
**Package:** `DEBT-184-EXPANDED-A11Y-AUTHORITY-MAP`  
**Batch:** `BATCH-2026-09-12-DEBT-184-A11Y-MAP`  
**Rebase source:** `ACCESSIBILITY-PREFERENCE-APPLICATION-DESIGN` in `docs/foreman/PARTIAL_IMPLEMENTATION_REBASE.md`  
**Date:** 2026-09-12  
**Sign-off:** Approve all §3 recommended defaults; colorblind modes moved from DEFER to **IN** as a package after the bounded four-flag bridge (Path α).

**Related (not this package):**

- Bounded Path α (four existing flags only): `DEBT-184-A11Y-PREFERENCE-BRIDGE` (ACCEPTED debt; claim separately).  
- Structural floors already live: Plan 80 `docs/ACCESSIBILITY.md` (focus, typographic floors, Theme WCAG tokens, keyboard model, honest AT limitation).  
- Historical proposal (stale premise): `Next-steps-plans/Plan_184_Accessibility_Options_System.md` — claims zero HighContrast systems; **do not treat as current evidence**.

---

## 1. Premise (current evidence)

### 1.1 Preference authority already exists

| Layer | Path | Role |
|---|---|---|
| DTO | `Assets/Ashfall.Core/Settings/UserSettingsData.cs` | `high_contrast`, `hazard_text_labels`, `reduced_motion`, `large_fonts` (+ display/audio/locale/gameplay) |
| Codec | `Assets/Ashfall.Core/Settings/UserSettingsCodec.cs` | Serialize / recover / sanitize |
| Host store | `src/Settings/UserSettings.cs` (`UserSettingsStore`) | `user://settings.json` load/save; `Apply` to engine |
| Panel | `src/UI/SettingsPanel.cs` | Exposes the four a11y toggles; **APPLY & SAVE** commits |

### 1.2 Thin runtime bridge already exists (audit correction)

Foreman finding `F-184-PREFERENCE-APPLICATION-GAP` (“no consumer”) is **stale**. Current consumers:

| Flag | Consumer | Effect today |
|---|---|---|
| `large_fonts` | `UserSettingsStore.Apply` | `ContentScaleFactor = clamp(UiScale × 1.15)` |
| `high_contrast` | `UserSettingsStore.Apply` | extra ×1.05 scale + first root `CanvasItem.Modulate` brighten |
| `hazard_text_labels` | `AshfallUiHelpers.FormatDoseSource` | off → raw source id; on → catalog display name |
| `reduced_motion` | `AudioManager` | snap occlusion; skip concussion tween; skip loop fades |

Startup applies settings before UI build (`Main.Application`).  
`OnSettingsApplied` today only drives onboarding `TutorialMode` — **no named refresh owner** for open dose/radiation panels.

### 1.3 Plan 80 owns structural accessibility (not user toggles)

`docs/ACCESSIBILITY.md` + `Theme.cs` + `AshfallFocusPolicy` + `AccessibilitySourceAuditTests` own:

- focus open/trap/restore  
- WCAG Theme token contrasts  
- typographic floors  
- keyboard navigation model  
- honest statement: **Godot 4.x has no native screen-reader bridge for custom GUI nodes**

Expanded Plan 184 must **extend user preference policy**, not fork Plan 80 floors into a second Theme authority.

### 1.4 Why full historical Plan 184 is blocked without this map

Historical Plan 184 proposes a new Core `AccessibilitySettingsSystem`, colorblind modes, font scaling DTO, screen reader, input remapping, audio descriptions, cognitive-load reduction, and a separate panel. That would:

- create a **second preference authority** beside `UserSettings*`  
- contradict sealed rebase non-goals for the first 184 package  
- collide with Plan 80 AT honesty and Plan 169 audio accessibility  
- collide with input-map / Plan 37 remapping territory  

---

## 2. Ownership table (proposed)

| Concern | Authority | Boundary |
|---|---|---|
| User preference persistence | **`UserSettingsData` + `UserSettingsStore`** | Sole settings file `user://settings.json`. No second accessibility save store. |
| Structural floors (focus, font minima, Theme WCAG tokens, keyboard model) | **Plan 80 / `Theme` / `AshfallFocusPolicy`** | Not preference-scoped; always on. Expanded 184 must not lower floors. |
| Applying engine-visible preferences | **`UserSettingsStore.Apply`** | Owns scale, modulate, audio buses, display, locale. Expanded visual modes must plug in here or via a thin helper it calls. |
| Construction-time presentation helpers | **`AshfallUiHelpers`** | May read `Current` for new controls; cannot alone re-skin existing trees. |
| Presentation refresh after committed Apply | **Named refresh owner** (see §3.3) | Must refresh open surfaces that cache formatted strings (dose/radiation). |
| Reduced-motion gate | **Shared predicate reading `Current.ReducedMotion`** | AudioManager today; future UI motion must use the same predicate — not a parallel flag. |
| Color / palette policy if colorblind modes are signed in | **Theme tokens via a preference-aware mapper** | Must not replace `Theme.cs` constants; map at presentation (`ToColor` / Apply) only. |
| Input remapping if signed in | **Existing input-map / rebind owner** (Plan 37 territory) | Must not invent a second action→key store inside accessibility. |
| Screen reader / AT if signed in | **Blocked by engine** until Godot AT matures | Plan 80 limitation remains authoritative; do not claim store-page AT support. |
| Audio descriptions if signed in | **Audio / Plan 169 mix legibility owners** | Prefer extending audio accessibility, not a parallel description bus in settings Core. |

---

## 3. Recommended defaults (AWAITING SIGN-OFF)

Amend freely; **code for expanded modalities is forbidden until this section is signed.**

### 3.1 Preference authority

1. **`UserSettingsData` / `UserSettingsStore` remain the sole preference authority.**  
   Do **not** create `AccessibilitySettingsSystem` or a second settings JSON.

2. **Plan 80 remains structural floors; Plan 184 is user toggles only.**  
   Floors always apply; toggles never disable focus rings, font minima, or Theme contrast gates.

### 3.2 Modality intake (first expanded wave)

| Modality | Recommended | Rationale |
|---|---|---|
| Existing four flags (HC / hazard / reduce motion / large fonts) | **IN — baseline (Path α first)** | Already persisted + thinly applied; seal/harden before colorblind |
| Colorblind / color-correction modes | **IN — next after Path α** | Preference-aware Theme/`ToColor` mapper + refresh lifecycle; new field(s) only in that package |
| Input remapping | **OUT of Plan 184** | Belongs to input-map / Plan 37; reference only |
| Screen reader / platform AT | **OUT until Godot AT exists** | Plan 80 honest limitation; no fake claims |
| Audio descriptions | **OUT of Plan 184** | Coordinate under Plan 169 / audio mix; not a settings Core fork |
| Cognitive-load / pause-anywhere | **OUT** | Gameplay/simulation policy; needs separate product decision |
| New preference fields in `UserSettingsData` | **None in Path α**; colorblind field(s) only in colorblind package | Harden existing four first, then add typed colorblind mode enum/string |

### 3.3 Presentation policy

3. **Refresh owner:** `SettingsPanel.OnSettingsApplied` → single Main/host handler that refreshes open dose/radiation (and later preference-sensitive) panels. `UserSettingsStore.Apply` remains engine authority.  
4. **HighContrast depth (near-term):** keep viewport modulate + scale nudge; **do not** rewrite Theme token constants. Optional later package: preference-aware `ToColor` boost.  
5. **ReducedMotion depth (near-term):** keep audio snap/skip; extract shared `MotionAllowed` predicate before claiming UI motion coverage (`CreateTween` in `src/` is audio-only today).  
6. **Apply timing:** **APPLY & SAVE only** for accessibility toggles (confirmed). Live preview stays limited to mute/volume.

### 3.4 Verification & honesty

7. **Gates:** extend `--settings-selftest` to assert LargeFonts scale, HighContrast modulate, HazardTextLabels formatting, ReducedMotion snap before claiming the four-flag bridge sealed; any new modality needs its own focused assertion.  
8. **Store / docs honesty:** omit modalities without a passing gate from accessibility statements (Plan 80 AT limitation pattern).

### 3.5 Non-goals (this map + first follow-on packages)

- Second preference persistence file or Core accessibility save section  
- Scene-by-scene panel rewrite  
- Claiming screen-reader support on current Godot  
- Absorbing input remapping into Plan 184  
- Treating historical `Next-steps-plans/Plan_184_*.md` as current architecture

---

## 4. Contract sketch (implement packages — not authorized by draft map alone)

### 4.1 Path α — Bounded preference bridge (authorized by rebase; separate claim)

Package id: `DEBT-184-A11Y-PREFERENCE-BRIDGE`  
Exact seams: `UserSettingsStore.Apply`, `AccessibilityPresentation` (`MotionAllowed` / `ResolveContentScaleFactor`), `AshfallUiHelpers.FormatDoseSource`, `AudioManager`, `Main.RefreshAccessibilityPreferenceSurfaces` via `OnSettingsApplied`, `--settings-selftest` extensions.  
No new fields. No colorblind/remap/AT.

### 4.2 Path β — Colorblind modes (after Path α sealed)

Package id: `DEBT-184-A11Y-COLORBLIND`  
**Status:** SEALED 2026-09-12.  
`colorblind_mode` under sole `UserSettings*`; Core `ColorblindColorMapper` (Viénot-style); `AshfallUiHelpers.ToColor` maps at read time; SettingsPanel OptionButton; `--settings-selftest` + recovery asserts. Theme constant floors unchanged. Remap/AT still OUT.

### 4.3 Further modalities

Anything still OUT in §3.2 stays out until a new signed amendment to this map.

---

## 5. Exact paths (this map package)

| Path | Role |
|---|---|
| `docs/ui/PLAN_184_EXPANDED_ACCESSIBILITY_AUTHORITY_MAP.md` | This contract |
| `KNOWN_DEBT.md` | `DEBT-184-EXPANDED-A11Y-AUTHORITY-MAP`, `DEBT-184-A11Y-PREFERENCE-BRIDGE` |
| `INTEGRATION_PLANS.md` | Active batch row |
| `WORKTREE_OWNERSHIP.md` | `claim-debt-184-a11y-map-2026-09-12` |

**Read-only evidence (not claimed for edit in this package):**

- `Assets/Ashfall.Core/Settings/UserSettingsData.cs`  
- `Assets/Ashfall.Core/Settings/UserSettingsCodec.cs`  
- `src/Settings/UserSettings.cs`  
- `src/UI/SettingsPanel.cs`  
- `src/UI/AshfallUiHelpers.cs`  
- `src/Audio/AudioManager.cs`  
- `docs/ACCESSIBILITY.md`  
- `docs/foreman/PARTIAL_IMPLEMENTATION_REBASE.md`  
- `Ashfall.Core.Tests/Settings/UserSettingsRecoveryTests.cs`  
- `src/Host/HostCli.PanelTests.cs` (`RunSettingsSelfTest`)

---

## 6. Sign-off checklist

- [x] §3.1 preference authority (sole `UserSettings*`)  
- [x] §3.1 Plan 80 vs 184 boundary  
- [x] §3.2 modality table — colorblind **IN after Path α**; remap/AT/audio-desc/cognitive **OUT**  
- [x] §3.3 refresh owner + HighContrast + ReducedMotion + Apply timing  
- [x] §3.4 verification + honesty rules  
- [x] §3.5 non-goals  

**Signed 2026-09-12.** Path α (`DEBT-184-A11Y-PREFERENCE-BRIDGE`) and Path β (`DEBT-184-A11Y-COLORBLIND`) sealed. Further modalities stay OUT until a new signed amendment.
