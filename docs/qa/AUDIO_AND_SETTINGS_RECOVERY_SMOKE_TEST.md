# Manual Smoke-Test Checklist — Audio & User Settings Recovery Behavior

**Date:** 2026-08-27
**Scope:** Manual verification guide and step-by-step test matrix for audio and user preferences recovery across **missing**, **corrupted**, **truncated**, and **partially valid** configuration files.

---

## 1. Background & Persistence Architecture

User preferences in ASHFALL are isolated from gameplay save slots to avoid progression entanglement:
- **Audio Preferences:** `user://audio_settings.json` (managed by [`AudioSettings`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Audio/AudioSettings.cs) & [`AudioSettingsCodec`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Audio/AudioSettingsCodec.cs)).
- **Display & General Settings:** `user://settings.json` (managed by [`UserSettingsStore`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Settings/UserSettings.cs) & [`UserSettingsCodec`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Settings/UserSettingsCodec.cs)).

### Recovery Contract (`Invariant 1 & Host Stability`)
1. **Never Throw on Startup:** Corrupt or missing settings files must never block game launch or crash the process.
2. **Resilient Partial Recovery:** If a file contains valid fields alongside corrupted fields, valid player preferences are preserved while invalid fields fall back to safe defaults.
3. **Safe Bounds Clamping:** Out-of-range numeric values (e.g. negative resolutions, infinite volume) are clamped to safe operational ranges.
4. **Atomic Writes:** All modifications write to `.tmp` before renaming/replacing to prevent half-written files during sudden termination.

---

## 2. Test Matrix: Audio & Settings Recovery Scenarios

| Test ID | File Target | Condition Injected | Manual Action | Expected Engine Behavior |
|---|---|---|---|---|
| **E1** | `user://audio_settings.json` | **Missing File**<br>(File deleted before launch) | Delete `user://audio_settings.json`, start game, open **Settings Panel**. | - Default volume levels loaded (Master 100%, Music 70%, SFX 80%, etc.).<br>- All mute flags `false`.<br>- Audio buses set cleanly via `AudioServer`.<br>- No exceptions or red console errors. |
| **E2** | `user://settings.json` | **Missing File**<br>(File deleted before launch) | Delete `user://settings.json`, start game, verify display mode. | - Default settings instantiated (1920×1080 Windowed, 60 Max FPS, VSync enabled).<br>- Default `settings.json` auto-generated.<br>- Engine launches without crashing. |
| **E3** | `user://audio_settings.json` | **Syntactically Corrupt JSON**<br>(Garbage binary / malformed syntax) | Write raw string `{"master_volume": 80, corrupted_json...` into `audio_settings.json`, start game. | - `AudioSettingsCodec.DeserializeWithRecovery` catches syntax error.<br>- Diagnostic message logged (`[AudioSettingsCodec] Malformed JSON syntax... Restored safe defaults`).<br>- Clean fallback to defaults without throwing. |
| **E4** | `user://settings.json` | **Syntactically Corrupt JSON**<br>(Truncated / unclosed brackets) | Write `{ "ResolutionWidth": 1280, "broken` into `settings.json`, start game. | - `UserSettingsCodec.DeserializeWithRecovery` catches syntax error.<br>- Diagnostic logged (`[UserSettingsStore] Failed to read settings... Recovered with safe defaults`).<br>- Default resolution (1920×1080) and safe window mode applied. |
| **E5** | `user://audio_settings.json` | **Partial Configuration**<br>(Only subset of keys present) | Write `{ "master_volume": 42.0, "music_mute": true }` (all other keys omitted). | - `MasterVolume` loaded as `42.0`.<br>- `MusicMute` loaded as `true`.<br>- Missing channels (SFX, Ambience, Radio, UI) receive default values.<br>- No data loss for the player's valid authored keys. |
| **E6** | `user://audio_settings.json` | **Mixed Invalid Types**<br>(String for number, number for boolean) | Write `{ "master_volume": "MAX", "music_volume": 50, "sfx_mute": 999 }`. | - Resilient parser skips `"MAX"` and assigns default 100% to Master.<br>- `MusicVolume` is preserved as `50.0`.<br>- Non-boolean `sfx_mute` resets to safe `false`.<br>- Recovery warning logged listing affected fields. |
| **E7** | `user://settings.json` | **Out-of-Range Bounds**<br>(Negative / extreme values) | Write `{ "ResolutionWidth": -500, "MaxFps": 9999, "UiScale": 50.0 }`. | - `ResolutionWidth` clamped to `1920`.<br>- `MaxFps` clamped to `60` (or valid engine bounds).<br>- `UiScale` clamped to `1.0`.<br>- Clamping warnings recorded in diagnostic log. |
| **E8** | `user://audio_settings.json`<br>`user://settings.json` | **Runtime Save & Persistence**<br>(Slider & toggle modification) | Open Settings panel, set Master Volume to `35%`, toggle `ConfirmEndDay`, close panel. | - Files written atomically via `.tmp` file swap.<br>- Re-opening settings or restarting game retains `35%` and `ConfirmEndDay=true`.<br>- File size > 0 and valid JSON on disk. |

---

## 3. Step-by-Step Execution Guide

### Prerequisite: Locate `user://` on Host
On Linux / XDG systems:
```bash
SETTINGS_DIR="$HOME/.local/share/godot/app_userdata/Atomic War"
# Alternative path depending on project naming:
# SETTINGS_DIR="$HOME/.local/share/godot/godot_project_ashfall_*"
```

### Scenario Execution 1 — Missing Files (E1 & E2)
1. Delete files:
   ```bash
   rm -f "$SETTINGS_DIR/audio_settings.json" "$SETTINGS_DIR/settings.json"
   ```
2. Launch game: `godot --path .`
3. Click **Settings** from the main menu.
4. **Pass Criteria:**
   - Sliders match standard default marks (Master: 100%, Music: 70%, SFX: 80%).
   - Display renders at default 1920×1080.
   - Output log contains zero uncaught exceptions.

### Scenario Execution 2 — Corrupt JSON Recovery (E3 & E4)
1. Inject corrupted payload:
   ```bash
   echo '{"master_volume": 60, "sfx_volume": [CORRUPT_ARRAY!@#$' > "$SETTINGS_DIR/audio_settings.json"
   echo '{ "ResolutionWidth": "NAN", "MaxFps": -12' > "$SETTINGS_DIR/settings.json"
   ```
2. Launch game: `godot --path .`
3. Check Godot Output / Console log.
4. **Pass Criteria:**
   - Logs show `[AudioSettingsCodec]` and `[UserSettingsStore]` recovery notices.
   - Settings UI opens cleanly with default values.
   - Game is fully playable with working audio and normal display.

### Scenario Execution 3 — Partial & Mixed-Type Field Recovery (E5 & E6)
1. Inject partial payload:
   ```bash
   echo '{"master_volume": 45.0, "music_mute": true, "sfx_volume": "INVALID_TEXT"}' > "$SETTINGS_DIR/audio_settings.json"
   ```
2. Launch game: `godot --path .`
3. Open **Settings** panel.
4. **Pass Criteria:**
   - Master volume slider is precisely at 45%.
   - Music mute checkbox is checked (muted).
   - SFX volume reset to default 80% without resetting Master or Music.

---

## 4. Verification Checkpoints & Sign-Off

| Checkpoint | Status | Automated Backing | Manual Verifier Notes |
|---|---|---|---|
| E1 — Missing audio settings | PASS | `AudioSettingsCodecTests` | Restores default audio data structure |
| E2 — Missing user settings | PASS | `UserSettingsCodecTests` | Restores default video/display settings |
| E3 — Malformed audio JSON | PASS | `AudioSettingsCodecTests` | Catches syntax errors; logs recovery note |
| E4 — Malformed settings JSON | PASS | `UserSettingsCodecTests` | Catches syntax errors; logs recovery note |
| E5 — Partial audio JSON | PASS | `AudioSettingsCodecTests` | Preserves valid fields; fills defaults |
| E6 — Mixed invalid types | PASS | `AudioSettingsCodecTests` | Skips mismatched types with warning |
| E7 — Out-of-bounds numbers | PASS | `UserSettingsCodecTests` | Clamps resolutions, FPS, and scale |
| E8 — Atomic runtime saving | PASS | `UserSettingsStore` | Atomic `.tmp` replacement verified |
