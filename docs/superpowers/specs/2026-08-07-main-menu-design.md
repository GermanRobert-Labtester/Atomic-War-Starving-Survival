# Main Menu ("LAST STATIC") — Design Spec

Date: 2026-08-07
Status: Approved by user, ready for implementation planning.

## Problem

`Assets/Scenes/StartScreen.unity` — the game's actual boot scene (first in
`EditorBuildSettings`, ahead of `SampleScene`) — is a placeholder: a single
`RawImage` showing `Assets/UI_StyleReference_01.jpg` fullscreen, plus a
`StartScreenController` that only listens for Escape-to-quit. There are no
buttons, no navigation, and no way to start or continue a game except by
loading `SampleScene` directly in the editor. The controller's own doc
comment says as much: "the actual main menu ... is authored separately and
wired onto this scene."

## Source material

`Figma-UI/` in the repo root contains a Figma Make export. Its
`make_repos/ox92e.zip` is a real git bundle (not just an image) containing a
complete, working React/CSS prototype of the intended menu — not just a
mock, but exact colors, layout percentages, hover/focus/disabled states,
keyboard navigation, and dialog copy. The prototype's own AI chat brief
(`Figma-UI/ai_chat.json`) states it is explicitly "intended for later
implementation in Unity UI Toolkit," which matches the doc comment already
present in `Assets/_Game/UI/HUD.cs` ("UI Toolkit architecture") even though
no UI Toolkit assets exist anywhere in the project yet.

Extracted prototype source (for implementation reference, not committed —
recreate by unzipping `Figma-UI/make_repos/ox92e.zip` as a git bundle if
needed): `src/App.tsx`, `src/index.css`. Key facts captured from it:

- Theme name: **"LAST STATIC"** ("A TRANSMISSION FROM" eyebrow above the
  title). Aesthetic: scavenged radio terminal in a ruined apartment
  (the existing `UI_StyleReference_01.jpg` background, already in
  `Assets/`, at 5504×3072).
- Colors (CSS custom properties → to become USS custom properties):
  `--ink:#090b0c`, `--panel: rgba(12,15,15,.82)`, `--line: rgba(217,196,152,.27)`,
  `--warm:#d3aa62`, `--hot:#f4c875`, `--pale:#e6e0d2`, `--muted:#938f84`.
- Type: **Barlow Condensed** (400/500/600/700, plus 500 italic) for display
  text, **Share Tech Mono** for all-caps HUD/label chrome. Both OFL-licensed
  Google Fonts; static TTFs already downloaded to the scratchpad
  (`BarlowCondensed-{Regular,Medium,SemiBold,Bold,MediumItalic}.ttf`,
  `ShareTechMono-Regular.ttf`, plus their `OFL.txt` license files) and need
  to land in `Assets/Fonts/`.
- Menu items (in order): **CONTINUE** (disabled when no save exists — "NO
  ACTIVE FIELD LOG"), **NEW EXPEDITION**, **SETTINGS**, **CREDITS**, **EXIT**.
  Each row: index (`01`.."05"), label, small detail line, arrow that appears
  on hover/focus/selected.
- Bug in the prototype to fix, not replicate: the **CREDITS** button's
  `action` opens the `settings` dialog instead of its own content.
- Keyboard nav: ↑/W and ↓/S move selection and call `.focus()` on the
  corresponding button; Enter/Space activates (native button semantics);
  Escape opens the quit-confirmation dialog (or closes an open dialog).
- Three dialog kinds, each: eyebrow, title, body copy, and (for New Game
  only) a two-option difficulty row (OPERATIVE/STANDARD selected by
  default, VETERAN/SCARCE RESOURCES), plus BACK / primary-action buttons.
- Status readout (bottom right): "RELAY // ONLINE" with a glowing dot,
  build string, sector label. Footer: copyright line + "ESC SESSION
  OPTIONS" hint.
- Visual chrome: dark vignette gradient over the background photo,
  full-bleed CRT scanline overlay (`mix-blend-mode: overlay`, low opacity),
  responsive breakpoints for narrower/ultrawide windows.

## Decisions (confirmed with user)

1. **Framework: Unity UI Toolkit** (UXML/USS/`UIDocument`), not legacy
   uGUI. Matches the documented intent in `HUD.cs` and the prototype's own
   brief. The CSS ports close to 1:1 into USS (same box model, flexbox,
   custom properties, `:hover`/`:focus`/`:disabled` selectors).
2. **Functional depth: everything wired for real**, not just a visual
   port:
   - CONTINUE actually reflects save state and loads it.
   - SETTINGS actually controls master volume, fullscreen mode, and
     resolution — new minimal infrastructure since none of these exist in
     the project yet (real, not deferred).
   - CREDITS shows real content: "Made by Roberts the Atomic-war_Dev" (a
     small "Built with Unity" line alongside it), not the prototype's
     placeholder "NORTHSTAR INTERACTIVE" or a re-skinned Settings dialog.
3. **Explicitly out of scope** (would be scope creep beyond what was
   asked): a real difficulty/gameplay-modifier system (no such system
   exists in the codebase; the New Expedition dialog's difficulty picker
   stays a UI-level choice recorded for a future system to consume),
   controller/gamepad rebinding, and sourcing new UI sound-effect assets
   (hover/select audio gets optional `AudioClip` hook fields on the
   controller that no-op when unassigned).

## Architecture

### New files

- `Assets/_Game/UI/MainMenu/MainMenu.uxml` — brand block, menu list (5
  buttons), status readout, footer, and 3 `<dialog>`-style overlay panels
  (New Expedition, Settings, Credits), all hidden by default.
- `Assets/_Game/UI/MainMenu/MainMenu.uss` — port of `index.css`: USS custom
  properties for the palette, Barlow Condensed / Share Tech Mono font
  usage, absolute-position layout matching the prototype's percentage-based
  placement, `:hover`/`:focus`/`:disabled` states, scanline + vignette
  overlay, dialog transitions.
- `Assets/_Game/UI/MainMenu/MainMenuController.cs` — `MonoBehaviour` on a
  `UIDocument`. Builds the 5 menu entries data-driven (mirrors the
  prototype's `MenuItem[]` array), owns keyboard nav/selection state,
  opens/closes the 3 dialogs, and dispatches the actual game actions below.
  Replaces `StartScreenController` (whose Escape-to-quit / cursor-visible
  behavior gets folded in).
- `Assets/_Game/UI/MainMenu/PendingGameLoad.cs` — static bridge (survives
  the `StartScreen` → `SampleScene` scene load within the same process):
  `SlotId` (string, null = fresh game) and `Difficulty` (enum/string,
  recorded but not yet consumed by gameplay systems).
- `Assets/_Game/Settings/GameSettings.cs` — PlayerPrefs-backed
  read/apply/persist for master volume, fullscreen mode, and resolution.
- `Assets/Audio/UIAudioMixer.mixer` — single `Master` group; `GameSettings`
  drives it via `SetFloat` with a log-scaled slider value.
- `Assets/Fonts/BarlowCondensed-{Regular,Medium,SemiBold,Bold,MediumItalic}.ttf`,
  `Assets/Fonts/ShareTechMono-Regular.ttf`, plus their `OFL.txt` license
  files, imported as Unity `Font` assets referenced from `MainMenu.uss`.

### Changed files

- `Assets/Scenes/StartScreen.unity` — add a `UIDocument` + `PanelSettings`
  driving `MainMenu.uxml`, replace `StartScreenController` with
  `MainMenuController`. The existing fullscreen background `RawImage`
  either becomes the UXML's background image element or stays as a raw
  Unity background layer under the `UIDocument` (implementation detail for
  the plan) — either way `UI_StyleReference_01.jpg` is reused as-is, not
  regenerated.
- `Assets/_Game/Core/GameBootstrap.Lifecycle.cs` — in `Awake()`, after
  `InitializeSystems()`, check `PendingGameLoad.SlotId`; if set, call the
  existing `LoadGame(slotId)` (same path as the F9 quickload keybind in
  `PlayerInputHandler.cs`) to overwrite the freshly-initialized world, then
  clear the pending flag.

### Save/Continue logic

- `SaveSystem` slot files live at
  `Application.persistentDataPath/saves/save_{slotId}.json`. Two slots are
  already in active use: `"quicksave"` (F5 manual save via
  `PlayerInputHandler`) and `"autosave"` (written whenever `GameState.Phase`
  becomes `Running`, per `GameBootstrap.InitLate.cs`).
- On `MainMenuController` init (in `StartScreen`, before any `SaveSystem`
  instance exists), check `File.Exists` directly for both slot paths under
  `Application.persistentDataPath/saves/`. If neither exists, CONTINUE stays
  disabled with the prototype's "NO ACTIVE FIELD LOG" copy. If one or both
  exist, CONTINUE is enabled and remembers whichever file has the newer
  `File.GetLastWriteTimeUtc`.
- CONTINUE click → `PendingGameLoad.SlotId = <that slot>` →
  `SceneManager.LoadScene("SampleScene")`.
- NEW EXPEDITION confirm click → `PendingGameLoad.SlotId = null`,
  `PendingGameLoad.Difficulty = <picked>` → load `SampleScene` (fresh game,
  same as today's default `Awake` behavior).
- EXIT → opens the quit-confirmation dialog (prototype already binds Esc to
  this same dialog); confirming calls the existing
  `StartScreenController.Quit()` logic (moved onto `MainMenuController`).

### Settings

- Master volume: USS/UXML slider bound to `GameSettings.MasterVolume01`
  (0..1), applied immediately via `AudioMixer.SetFloat("MasterVolume",
  Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20)`, persisted to PlayerPrefs on
  change.
- Fullscreen toggle: bound to `Screen.fullScreenMode` (`FullScreenWindow` /
  `Windowed`), applied immediately, persisted.
- Resolution dropdown: populated from `Screen.resolutions` (deduplicated by
  width×height), applied via `Screen.SetResolution`, persisted.
- All three apply live; the prototype's "APPLY SETTINGS" button becomes a
  confirmation/close action rather than a no-op-until-pressed gate.

### Credits

Dedicated dialog content (distinct from Settings, fixing the prototype's
bug): "Made by Roberts the Atomic-war_Dev" plus a short "Built with Unity"
line. The footer's placeholder studio name is replaced with the same
credit line (kept short so it fits the existing footer layout).

### Visual polish (beyond a literal port)

All achievable with USS transitions/animation on existing elements — no new
art or audio assets required:

- Slow Ken-Burns drift on the background layer (subtle scale/pan over
  roughly a minute) instead of a fully static frame.
- Staggered fade/slide-in for the menu list on scene load.
- Scale+fade transition when dialogs open/close.
- Optional hover/select SFX hook points (`AudioClip` serialized fields on
  `MainMenuController`) that simply do nothing if left unassigned — real
  sound design is left for later.

## Testing

- EditMode test(s) under `Assets/Tests/EditMode/` (matching the existing
  pattern, e.g. `CoreFamiliesWiringTests.cs`) covering:
  - `PendingGameLoad` default state (null slot) and set/clear behavior.
  - The "pick newer of two save files" logic used to decide the Continue
    slot, exercised against temp files with controlled
    `LastWriteTimeUtc` rather than real `Application.persistentDataPath`
    I/O.
  - `GameSettings` PlayerPrefs round-trip (set → persisted value read back)
    for volume/fullscreen/resolution.
- Manual verification in the Editor: boot `StartScreen`, confirm Continue
  is disabled with no save, becomes enabled after a `SampleScene` run
  writes a save, and that Continue actually restores that state
  (mirrors the existing F5/F9 quicksave/quickload flow already used
  in-game).
