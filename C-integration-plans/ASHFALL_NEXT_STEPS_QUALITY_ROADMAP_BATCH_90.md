# ASHFALL — Quality Roadmap Batch 90

## Theme: Audio System Architecture — Sound Design Pipeline for Godot Host

**Priority:** ~~MEDIUM~~ **CORRECTED — this batch's premise is false; see Review Notes.**<br>
**Risk:** ~~Low — additive system, no existing audio code to break~~ **CORRECTED — see below.**<br>
**Batch:** 90<br>
**Depends on:** Godot host functional (Main.cs, tick pipeline, settings infrastructure)<br>
**Blocked by:** Nothing — entirely new subsystem ~~**— FALSE, see below**~~

> **CORRECTED (see Review Notes at bottom):** This entire document was written on the premise
> that "zero audio infrastructure exists" beyond raw imported files. That premise is false. A
> complete, working, tested audio playback system already exists in `src/Audio/`:
> `AudioManager.cs` (bus topology, pooled one-shot players, music crossfade, ambience looping,
> cooldown/dedup, settings application), `AudioCueCatalog.cs` (45+ registered cues mapping stable
> snake_case IDs → resource paths, bus, volume, cooldown, loop, fallback), `AudioSettings.cs`
> (versioned, persisted, atomic-write user preferences with migration and effective-volume
> calculation), `AudioEventBridge.cs` (subscribes to Core domain events — `RadiationSystem`,
> `WeatherSystem` — and forwards to `AudioManager.PlayCue()`), and `AudioSelfTest.cs` (a ~100+
> assertion self-test covering cue coverage, bus topology, resource resolution, settings
> round-trip, cooldown, and lifecycle safety, run via `--audio-selftest`, already wired into
> `src/Host/HostCli.cs`). This batch has been rewritten below to (a) stop proposing duplicate
> infrastructure and (b) identify the actual, narrow, real gaps that remain.

---

## Motivation (CORRECTED)

The original Motivation section claimed the six `.wav` files below live flat in `assets/audio/`
and that nothing plays them. Both claims are false:

| File (as originally claimed) | Actual location | Actual status |
|------|-----------------|--------|
| `bunker_ambience.wav` | `assets/audio/ambience/bunker_ambience.wav` | Registered as cue `amb_bunker`, played via `AudioManager.StartBunkerAmbience()` / `PlayCue()`, looped, -3dB |
| `gameplay_underscore.wav` | `assets/audio/music/gameplay_underscore.wav` | Registered as cue `music_gameplay`, played via `AudioManager.PlayGameplayMusic()`, crossfaded, -8dB |
| `geiger.wav` | `assets/audio/sfx/geiger.wav` | Registered as cue `rad_geiger_loop`, played via `AudioManager.StartGeiger()`, looped, -10dB |
| `main_menu.wav` | `assets/audio/music/main_menu.wav` | Registered as cue `music_menu`, played via `AudioManager.PlayMainMenuMusic()`, crossfaded, -6dB |
| `radiation_alert.wav` | *(no such flat file — actual radiation alert cues use `.mp3` sources: `sfx_radiation_alarm.mp3` for acute/chronic)* | Registered as `rad_alert_acute`/`rad_alert_chronic`, 5-10s cooldown, wired to `RadiationSystem.OnStatusGained` via `AudioEventBridge` |
| `radio_static_hiss.wav` | `assets/audio/radio/radio_static_hiss.wav` | Registered as cue `radio_static`, played via `AudioManager.PlayRadioStatic()`, 0.5s cooldown |

`assets/audio/` is organized into subdirectories (`ambience/`, `music/`, `radio/`, `sfx/`, `ui/`),
not a flat directory as the original document implied, and contains dozens of files beyond the
six named (confirmed: `AudioCueCatalog` registers 45+ distinct cues across UI, radiation, weather,
ambience, music, radio, shelter, action, medical, danger, and game-flow categories).

**What actually exists today (verified against `src/Audio/*.cs`):**
- A sound manager/audio controller: `AudioManager.cs` (`Node`, singleton via `Instance`)
- Volume controls and audio settings: `AudioSettings.cs`, persisted to `user://audio_settings.json`
  with versioned migration, atomic writes, and a `GetEffectiveVolume()` helper
- Event-driven sound triggers (gameplay → audio): `AudioEventBridge.cs`, subscribed to
  `RadiationSystem.OnStatusGained` and `WeatherSystem.OnWeatherChanged` today
- A rudimentary music state machine: `PlayMainMenuMusic()` / `PlayGameplayMusic()` with
  `AudioManager`'s internal two-player (`_musicPlayerA`/`_musicPlayerB`) crossfade
- Audio bus routing: `AudioBusNames` defines `Master`/`Music`/`Ambience`/`SFX`/`UI`/`Voice`/`Alerts`,
  created and routed to `Master` in `AudioManager.SetupBuses()`
- Pool management for simultaneous sounds: a bounded 16-player one-shot pool with reclaim-on-finish

**What genuinely does NOT exist yet (the real, narrow gaps this batch should target instead):**
1. **No engine-agnostic port in Core.** `AudioManager`/`AudioCueCatalog`/`AudioEventBridge` are
   all `AtomicWar.GodotApp.Audio` — 100% Godot-side. Core has no `IAudioPort`-shaped interface;
   it exposes ad-hoc string-typed hooks instead (`WeatherAtmosphereMap.AudioCueId`/`AudioGain` on
   `WeatherKind` mapping, and `SomaticFlashbackSystem.OnAudioEvent(audioEventId, noiseSeverity)`,
   which is a *reverse-direction* hook — audio triggers a gameplay flashback check, not gameplay
   requesting playback). This is a real Invariant-1/2 gap (Core should own the port contract) but
   it is much smaller than "build IAudioPort from scratch" implies, since the actual playback
   engine, catalog, and bridge patterns already exist and work — the job is to give Core a formal
   seam, not invent the whole pipeline a second time.
2. **`WeatherAtmosphereMap.AudioCueId`/`AudioGain` are not consumed anywhere in `src/`.** Confirmed
   by search — no code reads these fields today. This is a real, small, wireable gap: Core already
   computes the audio cue ID and gain per weather kind; nothing forwards it to `AudioEventBridge`/
   `AudioManager`. This is legitimate, scoped work, unlike duplicating the whole architecture.
3. **`AudioEventBridge` only subscribes to `RadiationSystem` and `WeatherSystem`.** Needs, hunger/
   thirst-critical alerts, shelter enter/exit ambience swaps, and inventory action sounds
   (pickup/craft/consume) are not yet wired, even though matching cues already exist in
   `AudioCueCatalog` (`ActionItemPickup`, `ActionCrafting`, etc. are registered but nothing calls
   `PlayCue` for them from `NeedsSystem` or `InventorySystem`).
4. **No settings UI panel** — `AudioSettings` persistence and `GetEffectiveVolume()` exist, but
   there's no evidence of a settings screen with sliders bound to it (not verified either way in
   this pass — flagged as unconfirmed, check `src/UI/` before assuming this is missing or present).

The game's documented tone is "cold, exhausted, human, restrained." The *existing* system already
reflects this — cue volumes bias low (-3 to -15dB), alerts use cooldowns to avoid spam, and there
is no bombastic-score infrastructure. This batch does not need to establish that design intent; it
already governs the real implementation.

---

## Step 1: Design Audio Port (IAudioPort in Core, implemented by the existing AudioManager)

### Goal
Define the engine-agnostic audio contract in Core so gameplay systems can request playback
without referencing Godot types — **wrapping the existing, working `AudioManager`/`AudioCueCatalog`
pipeline in `src/Audio/`, not replacing it.** The port pattern (like `ILog`, `IFileIO`) keeps Core
clean; it does not require rebuilding playback, pooling, crossfade, or settings, all of which
already exist and work.

> **CORRECTED:** the original Step 1 proposed a `NullAudioPort`, a 5-method interface, and a
> from-scratch `AudioCategory` enum as if none of this existed. The interface below is scoped
> down accordingly: it should be a thin adapter *in front of* `AudioManager.PlayCue()` and the
> existing `AudioBusNames`, not a parallel new taxonomy. Cue IDs should be `AudioCueCatalog`'s
> existing snake_case IDs (`"rad_geiger_loop"`, `"ui_click"`, etc.) — do not invent a second ID
> namespace (`"audio_geiger_tick"` etc., as the original Step 2 proposed) that has to be kept in
> sync with the one that already exists.

### Implementation

**File:** `Assets/Ashfall.Core/Ports/IAudioPort.cs`

```csharp
namespace Ashfall.Core
{
    /// <summary>
    /// Engine-agnostic audio event emission port.
    /// Systems call this to request sound playback; the host adapter (GodotAudioPort,
    /// wrapping the existing AtomicWar.GodotApp.Audio.AudioManager) decides how/whether to play.
    /// Core never references audio files, buses, AudioStreamPlayer, or Godot's AudioServer —
    /// only the stable cue IDs already defined in AudioCueCatalog on the host side.
    /// </summary>
    public interface IAudioPort
    {
        /// <summary>Play a cue by its stable ID (matches AudioCueCatalog IDs on the host).
        /// Cooldown, looping, bus routing, and volume are all resolved host-side from the
        /// existing catalog entry — Core does not decide any of that.</summary>
        void PlayCue(string cueId);

        /// <summary>Stop whichever loop is currently associated with an ambience/music slot.
        /// Maps to AudioManager.StopAmbience()/StopMusic() — do not add new host-side state
        /// beyond what those two methods already track.</summary>
        void StopAmbience();

        void StopMusic();
    }

    /// <summary>No-op audio port for tests and headless mode. AudioManager itself already
    /// no-ops safely in headless mode (see AudioManager._headless / DisplayServer.WindowCanDraw
    /// check), so this is mainly for Core-level unit tests that should not need any host at all.</summary>
    public sealed class NullAudioPort : IAudioPort
    {
        public void PlayCue(string cueId) { }
        public void StopAmbience() { }
        public void StopMusic() { }
    }
}
```

**Design decisions (corrected):**

- `IAudioPort` is intentionally minimal — 3 methods, not 5 — because it is a thin Core-facing seam
  over an already-complete host implementation, not a new subsystem's full contract
- Cue IDs are **the existing `AudioCueCatalog` snake_case IDs** (`ui_click`, `rad_alert_acute`,
  `amb_bunker`, `music_gameplay`, `radio_static`, etc.) — reuse them exactly; do not define a
  parallel `audio_` -prefixed ID space
- No `AudioCategory` enum is introduced in Core; bus routing is already fully solved by
  `AudioBusNames` on the host side and doesn't need a Core-side mirror for this batch's scope
- No `SetVolume`/`StartLoop`/`StopLoop` generic API — those already exist as concrete, working
  methods on `AudioManager` (`StartGeiger`, `StartBunkerAmbience`, `StartSurfaceAmbience`,
  `StopAmbience`, `StopMusic`, `ApplySettings`); the port only needs to expose what Core-side
  systems actually need to call, which today is just "play this cue" and "stop this loop"

**Host adapter (new — this is the actual net-new file this step produces):**

**File:** `src/Audio/GodotAudioPort.cs`

```csharp
using Ashfall.Core;

namespace AtomicWar.GodotApp.Audio
{
    /// <summary>
    /// Thin IAudioPort implementation over the existing AudioManager singleton.
    /// Adds zero new playback logic — forwards directly to AudioManager.PlayCue()
    /// and the existing StopAmbience()/StopMusic() methods.
    /// </summary>
    public sealed class GodotAudioPort : IAudioPort
    {
        public void PlayCue(string cueId) => AudioManager.Instance?.PlayCue(cueId);
        public void StopAmbience() => AudioManager.Instance?.StopAmbience();
        public void StopMusic() => AudioManager.Instance?.StopMusic();
    }
}
```

### Verification
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — compiles with new port
- grep `IAudioPort.cs` for `UnityEngine`, `Godot`, `GodotSharp` — zero hits
- `NullAudioPort` usable in all existing tests (inject where needed, does nothing)
- Interface surface is minimal — 3 methods, no complex state, no duplicate taxonomy of the
  existing `AudioCueCatalog`/`AudioBusNames`
- `GodotAudioPort` compiles against the real, existing `AudioManager.Instance` (not a stub) —
  `dotnet build Ashfall.csproj` must pass with this file added

### Done-when
- `IAudioPort` exists in `Assets/Ashfall.Core/Ports/` (or alongside existing `Ports.cs`)
- `NullAudioPort` exists for tests/headless
- `GodotAudioPort` exists in `src/Audio/` and forwards to the real `AudioManager.Instance`
- Zero engine coupling in all new Core files
- No new cue-ID namespace, bus enum, or catalog was introduced — confirmed by diff review that
  this step only added a seam, not a parallel system

---

## Step 2: Wire the Real Gaps into AudioEventBridge (was: "Define Audio Event Taxonomy")

### Goal
The original Step 2 proposed a brand-new `AudioEventIds` static class (30+ constants) and a new
`audio_manifest.json` file duplicating what `AudioCueCatalog.cs` already does in code (45+ cues,
already mapped to files, buses, volumes, cooldowns, loop flags). **That duplication is dropped.**
Instead, this step closes the two real, confirmed gaps found during review:

1. `WeatherAtmosphereMap.AudioCueId`/`AudioGain` (`Assets/Ashfall.Core/World/WeatherAtmosphereMap.cs`)
   are computed by Core per `WeatherKind` but never consumed anywhere in `src/` (confirmed by
   search — zero references outside the file that defines them). Wire them into
   `AudioEventBridge.SubscribeWeather` so weather-driven ambience/cue changes actually reach
   `AudioManager`.
2. `AudioEventBridge` does not yet subscribe to needs-critical (hunger/thirst), inventory action
   (pickup/craft/consume), or shelter enter/exit events, even though matching cues already exist
   in `AudioCueCatalog` (`ActionItemPickup`, `ActionCrafting`, `ShelterDoorOpen`, etc.) unused by
   any subscriber today. This is genuinely new wiring, not new architecture.

### Implementation

**Do not create** `Assets/Ashfall.Core/Audio/AudioEventIds.cs` or
`Assets/StreamingAssets/Data/audio_manifest.json` — reuse the existing `AudioCueCatalog` constants
(`AudioCueCatalog.UiClick`, `AudioCueCatalog.RadAlertAcute`, `AudioCueCatalog.ActionItemPickup`,
etc.) directly from the host-side bridge code. If Core-side systems need to reference a cue ID by
name (e.g., `NeedsSystem` raising a hunger-critical event), have Core raise a plain string ID via
its own existing event pattern (a C# `event Action<string>` on the system, matching the existing
`SomaticFlashbackSystem`/`WeatherSystem` convention) and let the host-side subscriber map that
string to the correct `AudioCueCatalog` constant — Core does not need to know the cue's bus,
volume, or cooldown, all of which the catalog already owns.

**Extend `AudioEventBridge.SubscribeAll`:**

```csharp
public void SubscribeAll(
    RadiationSystem? radiation = null,
    WeatherSystem? weather = null,
    NeedsSystem? needs = null,          // NEW
    InventorySystem? inventory = null)  // NEW
{
    if (_subscribed) return;
    _subscribed = true;

    if (radiation != null) SubscribeRadiation(radiation);
    if (weather != null) SubscribeWeather(weather);
    if (needs != null) SubscribeNeeds(needs);         // NEW
    if (inventory != null) SubscribeInventory(inventory); // NEW
}

private void SubscribeWeather(WeatherSystem weather)
{
    weather.OnWeatherChanged += kind =>
    {
        // EXISTING behavior, unchanged:
        string? cueId = kind switch
        {
            WeatherKind.FalloutStorm => AudioCueCatalog.WeatherFalloutStorm,
            WeatherKind.BlackRain => AudioCueCatalog.WeatherBlackRain,
            WeatherKind.Blizzard => AudioCueCatalog.WeatherBlizzard,
            _ => null
        };
        if (cueId != null) _audio.PlayCue(cueId);

        // NEW: consume the Core-computed AudioCueId/AudioGain that WeatherAtmosphereMap
        // already produces but nothing reads today. Requires confirming the exact call site
        // that resolves WeatherAtmosphereMap for the current WeatherKind before wiring —
        // check whether WeatherSystem itself exposes the resolved AtmosphereMap entry, or
        // whether this bridge needs to call WeatherAtmosphereMap.Resolve(kind, headless) itself.
    };
}
```

**`NeedsSystem`/`InventorySystem` must actually expose events to subscribe to.** Before writing
`SubscribeNeeds`/`SubscribeInventory`, confirm both systems already raise C# events on
hunger/thirst-critical and item add/craft/consume (per `AGENTS.md`'s stated event-raising
convention: "every public system raises C# events on state change"). If they don't yet, adding
those events is itself real, separate work — do not assume the event exists just because the
convention says it should.

### Verification
- Every cue ID referenced by the bridge resolves via `AudioCueCatalog.Contains(id)` — no new
  string constants invented outside the existing catalog
- `dotnet build Ashfall.csproj` — `AudioEventBridge.cs` compiles with the new subscriptions
- `godot --headless --path . -- --audio-selftest` still exits 0 (this step must not regress the
  existing self-test, which is the actual verification authority for this system, not a new one)
- Manual/host test: trigger a hunger-critical state, verify `AlertHungerCritical`-equivalent cue
  fires — **note: no such cue exists in `AudioCueCatalog` today; check whether one needs to be
  added to the catalog (a one-line `Reg(...)` call) before this wiring can work end-to-end**

### Done-when
- `AudioEventBridge.SubscribeAll` accepts optional `NeedsSystem`/`InventorySystem` parameters
- Weather bridge consumes `WeatherAtmosphereMap.AudioCueId`/`AudioGain` where a resolution path
  exists, or the plan explicitly documents why it doesn't yet (e.g., `WeatherSystem` doesn't
  expose the resolved atmosphere map — that finding is itself a valid Done-when if wiring turns
  out to require a `WeatherSystem` change first)
- No new cue ID namespace, manifest file, or bus enum was created
- `--audio-selftest` still passes with 0 regressions

---

## Step 3: (DROPPED) Do not create a second Core-side event bus

### Original goal (for context)
The original Step 3 proposed a new `Ashfall.Core.Audio.AudioEventBus` plus a new
`Ashfall.Core.Audio.AudioEventBridge` class, with the bridge subscribing to the bus and forwarding
to `IAudioPort`.

### Why this step is dropped

> **CORRECTED — direct name collision + unnecessary layer.** A class named `AudioEventBridge`
> **already exists** at `src/Audio/AudioEventBridge.cs` (namespace `AtomicWar.GodotApp.Audio`).
> Creating a second, differently-behaved `AudioEventBridge` in `Ashfall.Core.Audio` is exactly the
> kind of confusing duplicate-name architecture this review is meant to catch — two developers
> six months from now searching "AudioEventBridge" will find two classes with the same name doing
> conceptually similar but structurally different things (one bridges Core events → `AudioManager`
> directly today; the plan's version would bridge a new bus → `IAudioPort` → `GodotAudioPort` →
> `AudioManager`, a longer chain for no behavioral gain).
>
> More fundamentally, **the extra pub/sub layer is unneeded.** The existing pattern — confirmed in
> `RadiationSystem` (`OnStatusGained` event) and `WeatherSystem` (`OnWeatherChanged` event) — is
> that Core systems already raise plain C# events on state change, per `AGENTS.md`'s own stated
> convention ("every public system raises C# events on state change"). The real
> `AudioEventBridge` already subscribes to those events directly and calls `AudioManager.PlayCue()`.
> Inserting an `AudioEventBus` in between adds a hop (system → bus → bridge → port → AudioManager)
> where today it's (system → bridge → AudioManager), with no new capability — the "fire into void
> if no subscriber" and "null-safe in tests" properties the original step wanted are **already
> true of plain C# events**: an event with no subscriber is a no-op, and a system that never gets
> an `AudioEventBridge` constructed against it never touches audio at all.
>
> **What Step 2 (as corrected above) already covers instead:** extending
> `AudioEventBridge.SubscribeAll` to also subscribe to `NeedsSystem`/`InventorySystem` events,
> using the exact same direct-event-to-`PlayCue`-call pattern the bridge already uses for
> `RadiationSystem`/`WeatherSystem`. No new bus type is needed for that.
>
> If a future need arises for many-Core-systems-to-one-audio-adapter fan-out that plain events
> can't express, revisit this idea then, under a name that does not collide with the existing
> `AudioEventBridge`, and only after confirming plain events are actually insufficient.

---

## Step 4: (DROPPED) Do not build GodotAudioManager — AudioManager already is this

### Original goal (for context)
The original Step 4 proposed a new `src/Audio/GodotAudioManager.cs`: a Godot `Node` implementing
`IAudioPort` with an SFX player pool, per-category loop players, crossfade via `Tween`, a
manifest loader, and `AudioServer` volume control.

### Why this step is dropped

> **CORRECTED — this is a near-total duplicate of the existing `AudioManager.cs`.** Line for
> line, the original step's design maps onto code that already exists and already runs:
>
> | Original Step 4 proposal | Already exists as |
> |---|---|
> | SFX pool (8 `AudioStreamPlayer`, round-robin) | `AudioManager._pool`/`_activeOneShots`, bounded at 16 (`MaxOneShotPlayers`), reclaim-on-finish in `_Process` |
> | Loop players per category | `_musicPlayerA`/`_musicPlayerB` (two-slot crossfade) + `_ambiencePlayer` |
> | Cooldown dictionary (`event_id → next_allowed_time`) | `AudioManager._cooldowns`, decremented every `_Process` tick |
> | Manifest loader (`audio_manifest.json`) | `AudioCueCatalog.RegisterAll()` — in-code, not JSON, but the same lookup role |
> | Crossfade via `Tween` | `AudioManager`'s `_musicCrossfade`/`_crossfading` float-lerp crossfade in `_Process` (no `Tween` node, but the same audible effect, already shipped) |
> | `SetVolume(category, volume)` via `AudioServer` | `AudioManager.ApplySettings()` / `SetBusVolume()`, already calls `AudioServer.SetBusVolumeDb`/`SetBusMute` |
> | Bus layout (Master/Music/Ambient/Sfx/Alert/UiFeedback) | `AudioBusNames` + `AudioManager.SetupBuses()` (Master/Music/Ambience/SFX/UI/Voice/Alerts — 7 buses, one more than proposed, including Voice for radio VO) |
>
> Writing a second implementation under a different class name (`GodotAudioManager` vs
> `AudioManager`) would create exactly the kind of dual-authority problem `AGENTS.md` warns about
> elsewhere in this project (see Invariant 6 / dual ScriptableObject-vs-JSON authority) — two
> Godot `Node`s that both think they own bus volume and player pooling, with no clear rule for
> which one a new caller should use.
>
> **What this step becomes instead:** confirm `GodotAudioPort` (from corrected Step 1) forwards
> cleanly to the *existing* `AudioManager.Instance`, and that no second manager, pool, or bus
> setup is created. That is a ~10-line adapter, not a new subsystem — already covered by Step 1.
>
> **One real, narrow gap worth keeping from the original step's spirit:** the existing crossfade
> is a fixed-rate float lerp in `_Process` (`_musicCrossfade += dt * 0.5f`, i.e. always a 2-second
> linear fade), not parameterized per-cue like the original step's "ambient loops get a slower
> 2s crossfade, alerts get no fade" idea. If differentiated fade timing per cue is actually wanted,
> that is a small, legitimate enhancement to `AudioManager.PlayMusicStream`/`PlayLoopStream` — but
> scope it as "add a `fadeSeconds` field to `AudioCueDef` and thread it through the two existing
> fade code paths," not as a new manager class.

---

## Step 5: Wire the Remaining Gameplay Events to the Existing Bridge (was: "Wire Gameplay Events to Audio")

### Goal
Connect the gameplay systems that aren't yet wired to `AudioEventBridge` so meaningful state
changes produce sound. No gameplay logic changes — only audio call sites added, using the
existing `AudioManager`/`AudioCueCatalog`, not a new bus.

> **CORRECTED — wiring table checked against reality.** The original table listed 15 wiring
> points as if none existed. Two (`RadiationSystem` alerts, `WeatherSystem` storm cues) are
> **already wired** in `AudioEventBridge.SubscribeRadiation`/`SubscribeWeather`. The corrected
> table below marks status per row. Cue names use the real `AudioCueCatalog` constants, not the
> invented `AudioEventIds`/`AlertXxx` names from the original draft.

| System | Event | Real cue (AudioCueCatalog) | Status |
|--------|-------|------|--------|
| `RadiationSystem` | `AcuteRadiationSickness` status gained | `RadAlertAcute` | **Already wired** (`AudioEventBridge.SubscribeRadiation`) |
| `RadiationSystem` | `ChronicIllness` status gained | `RadAlertChronic` | **Already wired** |
| `RadiationSystem` | Geiger proximity per-tick | `RadGeigerLoop` / `RadGeigerBurst` | Not wired — `AudioManager.StartGeiger()` exists but nothing calls it from `RadiationSystem`; needs a proximity signal from Core (confirm one exists before wiring) |
| `NeedsSystem` | Hunger hits critical | *(no matching cue exists yet)* | Not wired, **and no cue exists** — add one to `AudioCueCatalog` first (e.g. `need_hunger_critical`) or reuse `DangerAlarmKlaxon` deliberately, don't invent an ID that isn't registered |
| `NeedsSystem` | Thirst hits critical | *(no matching cue exists yet)* | Same as above |
| `WeatherSystem` | `FalloutStorm`/`BlackRain`/`Blizzard` changed | `WeatherFalloutStorm`/`WeatherBlackRain`/`WeatherBlizzard` | **Already wired** (`AudioEventBridge.SubscribeWeather`) |
| `WeatherSystem` | Storm ends | *(no explicit "stop" cue path)* | Not wired — confirm whether `WeatherKind` transitions back to `Clear`/`Overcast` should call `StopAmbience()`; check current behavior before assuming silence is a bug |
| Shelter state | Enter bunker | `AmbBunker` via `AudioManager.StartBunkerAmbience()` | Method exists; confirm whether shelter-enter/exit host code actually calls it (not verified either way in this pass — check `src/Host/` shelter session before wiring) |
| Shelter state | Exit to outdoors | `AmbSurface` via `AudioManager.StartSurfaceAmbience()` | Same — method exists, call-site unconfirmed |
| `InventorySystem` | Item added | `ActionItemPickup` | Registered in catalog, not called from `InventorySystem` today — real gap |
| `InventorySystem` | Item crafted | `ActionCrafting` | Registered, not called — real gap |
| `InventorySystem` | Item consumed | *(no exact match — `ActionPillBottle`/`ActionInjection` exist for specific item types)* | Registered for specific cases, not generic consume — check whether a generic consume cue is wanted or per-item-type cues are correct as-is |
| Radio system | Radio tuned | `RadioTune` + `RadioStatic` | Registered, `PlayRadioStatic()` exists; confirm host radio-tuning code calls it |
| Scene transitions | Enter main menu | `MusicMenu` via `AudioManager.PlayMainMenuMusic()` | Method exists; confirm `Main.cs` main-menu entry actually calls it |
| Scene transitions | Enter gameplay | `MusicGameplay` via `AudioManager.PlayGameplayMusic()` | Method exists; confirm call site |
| Combat/tension | High danger state | *(no `MusicTension` cue exists)* | Not wired, **and no cue exists** — this is genuinely new work if wanted, not just wiring |

**Before implementing this step, an implementer must grep `PlayMainMenuMusic\|PlayGameplayMusic\|StartBunkerAmbience\|StartSurfaceAmbience\|PlayRadioStatic` across `src/` to find which of these already-existing methods are already called from somewhere, and which are dead code waiting for a caller.** Do not assume every row above is unwired without checking — this document does not have that answer for every row and says so explicitly rather than guessing.

**Implementation pattern (using the real bridge, not a bus):**

```csharp
// Extending AudioEventBridge (src/Audio/AudioEventBridge.cs) — NOT a change to RadiationSystem
// itself. Systems should not take a constructor-injected audio bus; the existing bridge pattern
// subscribes to systems' existing public C# events from the outside, keeping Core untouched.
private void SubscribeInventory(InventorySystem inventory)
{
    inventory.OnItemAdded += _ => _audio.PlayCue(AudioCueCatalog.ActionItemPickup);
    inventory.OnItemCrafted += _ => _audio.PlayCue(AudioCueCatalog.ActionCrafting);
}
```

If `InventorySystem` does not yet expose `OnItemAdded`/`OnItemCrafted` events, adding them is
itself the real prerequisite work — confirm before estimating this step's size.

**Rules for audio wiring (unchanged from original intent, still correct):**
- Never add audio logic that affects gameplay decisions (audio is presentation only)
- One-shots for discrete events; loops for continuous states
- Alert-style cues already carry cooldowns in `AudioCueCatalog` (5-10s) — reuse that mechanism,
  don't build a second cooldown system
- Ambient loop crossfade on location change already exists as a side effect of
  `PlayLoopStream`/the two-player music crossfade — confirm it also applies cleanly to ambience
  (currently `_ambiencePlayer` is a single player with no crossfade slot, unlike music's two-player
  setup — switching bunker↔outdoors ambience today would cut, not crossfade; flag this as a real,
  small gap if smooth ambience transitions are wanted)

**Music state machine:** a `MusicTension`/danger-state cue does not exist today. If wanted, this
is new catalog + wiring work, not just wiring an existing cue — size it separately from the rest
of this step.

### Verification
- All modified/new bridge subscriptions still let existing tests pass — `AudioEventBridge` is
  host-side only and untested by `Ashfall.Core.Tests`, so this must be verified via
  `dotnet build Ashfall.csproj` plus `godot --headless --path . -- --audio-selftest`
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — confirm no Core-side test file was
  touched by this step (it shouldn't be — this step is host-only wiring); if the count of existing
  passing tests changes at all, that's a signal something in Core was touched that shouldn't be
- Manual: trigger each newly-wired gameplay event, verify correct cue plays via
  `AudioManager.PlayCue()` (check Godot console output — `AudioManager` logs missing assets via
  `LogMissingOnce`, which will surface a wrong/missing cue ID immediately)
- Manual: verify headless mode stays silent (`AudioManager._headless` already gates all playback —
  confirmed in `AudioManager.PlayCueDef`: `if (_headless) return;`)

### Done-when
- Every row in the corrected wiring table above is resolved to one of: "confirmed already wired,"
  "wired in this step," or "explicitly deferred with a stated reason" — no row is silently ignored
- No new cue ID invented without first adding it to `AudioCueCatalog` via a `Reg(...)` call
- No gameplay logic changes — audio calls are added only to `AudioEventBridge` or equivalent
  host-side subscriber code, not to Core systems' decision logic
- `--audio-selftest` still passes with 0 regressions
- Existing `dotnet test` count for `Ashfall.Core.Tests` is unchanged (proves no Core coupling)

---

## Step 6: Confirm/Build the Settings UI Panel (was: "Add Audio Settings")

### Goal
Player-facing audio settings: per-category volume sliders, master mute toggle, persistence
across sessions.

> **CORRECTED — `AudioSettings` already exists and is more complete than the original proposal.**
> `src/Audio/AudioSettings.cs` already provides: versioned persistence to
> `user://audio_settings.json` with atomic writes and malformed-file recovery, per-category
> volume (`MasterVolume`, `MusicVolume`, `AmbienceVolume`, `SfxVolume`, `UiVolume`, `VoiceVolume`,
> `AlertVolume` — 7 categories, one more than the original 6-category proposal, including Voice),
> per-category mute flags, `GetEffectiveVolume(categoryVolume, categoryMute)`, `ClampVolume`,
> `PercentToDb`, `ResetToDefaults()`, and a change-notification event (`OnSettingsChanged`).
> `AudioManager.ApplySettings()` already calls `AudioServer.SetBusVolumeDb`/`SetBusMute` for every
> bus from this settings object, and re-applies automatically via the `OnSettingsChanged`
> subscription set up in `AudioManager._Ready()`. **Do not create a second `AudioSettings` class,
> and do not create a new `Ashfall.Core.Audio.AudioSettings` — the existing one already lives at
> `AtomicWar.GodotApp.Audio.AudioSettings` and there is no evidence Core needs its own copy for
> this batch's scope (it's host-only user preference state, not gameplay state, and has no
> `CaptureState`/`RestoreState` requirement under Invariant 3 since it isn't part of a game save).**

**What is NOT confirmed either way — the actual remaining gap for this step:**

A settings UI panel with bound sliders was not found or ruled out during this review (search
scope was `src/Audio/` and `src/Host/`; `src/UI/` was not exhaustively checked in this pass).
**Before doing any work here, check `src/UI/` for an existing settings panel that might already
bind to `AudioSettings.Instance`.** If one exists, this step is "confirm it's complete" +
"add a Test Sound button if missing." If one does not exist, this step is genuinely new UI work:// building sliders bound to the 7 existing `AudioSettings` properties, calling
`.Save()` and `.NotifyChanged()` on change, plus a mute-all toggle and a "Test Sound" button that
calls `AudioManager.Instance.PlayCue(AudioCueCatalog.UiConfirm)` as a preview.

### Verification
- `AudioSettingsTests`-equivalent coverage already exists inside `AudioSelfTest.cs`'s
  "Settings Persistence" section (default values, round-trip, clamp, effective volume, reset) —
  confirm this step doesn't duplicate those checks in a new test file
- If a UI panel is built: manual test that moving a slider calls `.Save()` and audibly changes
  volume without an "Apply" button, matching the existing `OnSettingsChanged` push model
- If a UI panel already exists: `godot --headless --path . -- --audio-selftest` should already be
  passing on `AudioSettings`; this step doesn't need to touch it

### Done-when
- Confirmed whether a settings panel exists in `src/UI/`, with the answer stated explicitly in
  this document (not left as an open question after implementation)
- If missing: sliders exist for all 7 `AudioSettings` categories (not 6 — Voice is real and
  already persisted, don't drop it), bound to `AudioSettings.Instance`, calling `.Save()` on change
- Mute toggle(s) wired to the existing per-category and master mute fields
- "Test Sound" button plays a cue via the existing `AudioManager.PlayCue`, not a new playback path
- No second `AudioSettings` class was created anywhere in the codebase

---

---

## Step 7: Extend Coverage — `AudioSelfTest.cs` and, if genuinely needed, Core-side port tests

### Goal
Verify audio wiring behaves correctly without requiring real audio playback in CI.

> **CORRECTED — a comprehensive audio test suite already exists.** `src/Audio/AudioSelfTest.cs`
> already covers, per its own section headers: cue catalog coverage (count ≥45, every cue resolves
> or has a valid fallback, required cues present, unknown/empty/null cue returns null safely), bus
> topology (every cue uses a valid bus name), resource resolution (18+ key asset paths checked
> against `ResourceLoader.Exists`/`File.Exists`), settings persistence (defaults, round-trip via
> `JsonSerializer`, malformed-file recovery, volume-helper math, effective-volume math including
> mute-override, reset-to-defaults), cooldown/dedup (alert and UI cues carry cooldowns), event-to-
> cue coverage (game-flow/radiation/weather/shelter/action/radio cue presence), and lifecycle
> safety (catalog non-null, settings singleton safe). This runs via
> `godot --headless --path . -- --audio-selftest`, already dispatched from `HostCli.cs`, and is
> the correct place to add coverage for *this batch's* real new work (Step 2's weather-cue wiring,
> Step 5's newly-wired systems, Step 6's settings panel if built) — not a second, parallel test
> file with a new `RecordingAudioPort`/`IAudioPort`/`AudioCategory` stack that doesn't match what
> Step 1 actually proposed (a 3-method port, not 5, with no `AudioCategory` enum in Core).

**What this step becomes:**

1. **Extend `AudioSelfTest.cs`**, not replace it, with checks for whatever Step 2 and Step 5
   actually wire (e.g., a check that `WeatherAtmosphereMap.AudioCueId` values, once consumed, map
   to real registered cues; a check that any newly-added `NeedsSystem`/`InventorySystem`
   subscriptions in `AudioEventBridge` don't throw when the underlying system is null).
2. **If and only if** Step 1's `IAudioPort`/`GodotAudioPort`/`NullAudioPort` land in Core, add a
   small, matching `Ashfall.Core.Tests` suite for them — scoped to the corrected 3-method
   interface, not the original 5-method/`AudioCategory` design:

```csharp
// Ashfall.Core.Tests/Audio/RecordingAudioPortTests.cs — ONLY if IAudioPort actually lands.
// Matches the corrected, minimal 3-method IAudioPort from Step 1 — do not test methods
// (StartLoop/StopLoop/SetVolume/StopAll) that Step 1 no longer defines.
namespace Ashfall.Core.Tests.Audio
{
    public sealed class RecordingAudioPort : IAudioPort
    {
        public List<string> PlayedCues { get; } = new();
        public int StopAmbienceCount { get; private set; }
        public int StopMusicCount { get; private set; }

        public void PlayCue(string cueId) => PlayedCues.Add(cueId);
        public void StopAmbience() => StopAmbienceCount++;
        public void StopMusic() => StopMusicCount++;
    }

    public class NullAudioPortTests
    {
        [Fact]
        public void All_calls_are_no_ops()
        {
            var port = new NullAudioPort();
            port.PlayCue("anything");   // must not throw
            port.StopAmbience();        // must not throw
            port.StopMusic();           // must not throw
        }
    }
}
```

### Verification
- `godot --headless --path . -- --audio-selftest` — extended checks pass, existing checks
  unregressed (compare pass/fail counts before and after; the self-test already prints a
  pass/fail summary line, so a before/after diff is trivial)
- If `IAudioPort` lands: `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "Audio"` —
  new tests pass, scoped to the 3-method interface only
- No test requires Godot runtime for the Core-side `IAudioPort` tests specifically (they test an
  interface and a `NullAudioPort`/`RecordingAudioPort`, neither of which touches Godot types)
- No duplicate test file was created for functionality `AudioSelfTest.cs` already covers

### Done-when
- `AudioSelfTest.cs` gained checks for whatever this batch's Step 2/5/6 actually changed, with
  the pass count increasing by at least that many new `Check(...)` calls
- If `IAudioPort` landed in Core: a small, correctly-scoped (3-method) test suite exists for it
  and for `NullAudioPort`
- No second, parallel test file duplicates existing `AudioSelfTest.cs` coverage
- All tests pass; `dotnet test` count for `Ashfall.Core.Tests` only increases if `IAudioPort`
  actually landed in Core this batch

---

## Summary Table (CORRECTED)

| Step | Original claim | Actual status after review | Corrected deliverable |
|------|------|------|------|
| 1 | Build `IAudioPort` from scratch | Real, small gap — no Core-side port exists | Thin 3-method `IAudioPort` + `NullAudioPort` (Core) + `GodotAudioPort` (host, wraps existing `AudioManager.Instance`) |
| 2 | Build `AudioEventIds` + `audio_manifest.json` from scratch | **False premise** — `AudioCueCatalog.cs` already provides this (45+ cues, in-code) | Wire the 2 confirmed-unconsumed gaps: `WeatherAtmosphereMap.AudioCueId`/`AudioGain`, and extend `AudioEventBridge.SubscribeAll` with `NeedsSystem`/`InventorySystem` |
| 3 | Build `AudioEventBus` + a second `AudioEventBridge` | **Dropped** — name collision with existing `AudioEventBridge`; unneeded layer over existing plain-C#-event pattern | No new files |
| 4 | Build `GodotAudioManager` from scratch | **Dropped — near-total duplicate** of existing, working `AudioManager.cs` (pool, crossfade, cooldown, buses all already implemented) | No new files beyond Step 1's `GodotAudioPort` adapter |
| 5 | Wire 15 gameplay events to a new bus | 2 of 15 rows **already wired**; several rows reference cues that don't exist yet | Wire only the confirmed real gaps (table in corrected Step 5), using the existing bridge/catalog |
| 6 | Build `AudioSettings` from scratch | **False premise** — `AudioSettings.cs` already exists, more complete than proposed (7 categories incl. Voice, atomic persistence, migration) | Confirm/build only a settings UI panel, if one doesn't already exist in `src/UI/` (unconfirmed in this review) |
| 7 | Build `RecordingAudioPort` + 15 new tests | `AudioSelfTest.cs` already covers ~100+ assertions across the equivalent surface | Extend `AudioSelfTest.cs` for new wiring; add a small Core-side test only if Step 1's `IAudioPort` lands |

**Total new files (corrected):** ~3-5 — `IAudioPort.cs`, `NullAudioPort.cs` (may be same file),
`GodotAudioPort.cs`, and an optional `RecordingAudioPort`/test file if Step 1 lands. This is
**not** the original ~15 — the vast majority of originally-proposed files duplicated code that
already exists and works.<br>
**Modified files (corrected):** ~2-4 — `AudioEventBridge.cs` (new subscriptions), `AudioCueCatalog.cs`
(only if a genuinely new cue like a hunger-critical alert or tension music is wanted),
`AudioSelfTest.cs` (extended checks), and possibly a settings UI file if one doesn't exist.<br>
**Estimated effort (corrected):** 1-2 sessions, not 3-4 — most of the original scope evaporates
once the existing system is accounted for. The one item that could expand this is Step 6's
settings panel, if `src/UI/` genuinely has none — that should be estimated separately once
confirmed, not folded into this batch's headline estimate.<br>
**Breaking changes:** None — every corrected step is either additive (`IAudioPort` seam) or wires
already-registered cues into the already-existing bridge; nothing about `AudioManager`,
`AudioCueCatalog`, or `AudioSettings` needs to change shape.<br>
**Performance impact:** Negligible, same reasoning as the original draft — but note the *actual*
mechanism (plain C# events + existing pooled players) is already zero-alloc-on-hot-path in
practice, not a projected property of a not-yet-built system.<br>
**Audio files needed beyond current:** Only if Step 5 decides new cues (hunger/thirst-critical
alert, tension music) are wanted — flagged per-row in the corrected Step 5 table, not assumed.

---

## Review Notes (Corrected)

This document was adversarially reviewed against the actual ASHFALL codebase
(`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`). The original document's central
claim — "no audio playback system exists beyond raw files in assets/audio/" — is **false**, and
every step built on that false premise needed correction:

1. **Audio file locations were wrong.** The original table listed all six files as living flat in
   `assets/audio/`. In reality `assets/audio/` has five subdirectories (`ambience/`, `music/`,
   `radio/`, `sfx/`, `ui/`); the six named files live at `assets/audio/ambience/bunker_ambience.wav`,
   `assets/audio/music/gameplay_underscore.wav`, `assets/audio/sfx/geiger.wav`,
   `assets/audio/music/main_menu.wav`, `assets/audio/radio/radio_static_hiss.wav`, and there is no
   flat `radiation_alert.wav` at all — the real radiation-alert cues (`rad_alert_acute`,
   `rad_alert_chronic`) use `.mp3` sources (`sfx_radiation_alarm.mp3`).
2. **A complete, working audio system already exists in `src/Audio/`:** `AudioManager.cs` (bus
   topology, pooled one-shot players with reclaim, two-player music crossfade, single ambience
   player, cooldown/dedup, headless detection, settings application), `AudioCueCatalog.cs` (45+
   registered cues with stable snake_case IDs, resource paths, bus, volume, cooldown, loop,
   fallback), `AudioSettings.cs` (versioned + persisted + atomic-write settings with migration and
   effective-volume math), `AudioEventBridge.cs` (already subscribes `RadiationSystem` and
   `WeatherSystem` events to `AudioManager.PlayCue()`), and `AudioSelfTest.cs` (a ~100+ assertion
   self-test already dispatched via `--audio-selftest` in `src/Host/HostCli.cs`). Confirmed by
   direct file reads, not inference.
3. **`grep -r AudioStreamPlayer src/` does NOT return zero hits**, contrary to the review brief's
   assumption going in — it returns exactly the fields in `AudioManager.cs`
   (`_musicPlayerA`/`_musicPlayerB`/`_ambiencePlayer`/pool `Stack<AudioStreamPlayer>`). This
   review's own verification step disproved the premise it was asked to confirm, and the document
   has been corrected accordingly rather than forcing the false premise through.
4. **Steps 1, 3, 4, 6 as originally written would have created direct duplicates** of
   `AudioManager`/`AudioCueCatalog`/`AudioSettings`, and Step 3 specifically would have created a
   **second class literally named `AudioEventBridge`** in a different namespace — a naming
   collision this review flags as a serious "two systems with the same name" risk on its own,
   independent of the duplication problem. Steps 3 and 4 are dropped entirely; Steps 1 and 6 are
   narrowed to the genuinely-missing pieces (a Core-side port seam; a possible settings UI panel).
5. **Step 2's "audio event taxonomy"** duplicated `AudioCueCatalog`'s existing taxonomy under a
   different, incompatible ID scheme (`audio_sfx_geiger_tick` vs. the real `rad_geiger_loop`).
   Corrected to reuse the existing catalog IDs and instead wire the two real, confirmed-unconsumed
   gaps: `WeatherAtmosphereMap.AudioCueId`/`AudioGain` (computed in Core, never read in `src/`),
   and missing `NeedsSystem`/`InventorySystem` subscriptions on the existing bridge.
6. **Step 5's wiring table was audited row-by-row.** 2 of the original 15 rows (radiation alerts,
   weather cues) are already implemented; several referenced cues that don't exist in
   `AudioCueCatalog` at all (`AlertHungerCritical`, `MusicTension`) and would need new catalog
   entries before wiring could work, which the original document didn't flag. The corrected table
   marks every row's true status and explicitly calls out cues that don't exist yet as separate,
   uncosted work rather than silently bundling them into "wiring."
7. **Step 7's proposed `IAudioPort`-based test double no longer matches Step 1's corrected,
   minimal interface** (3 methods, no `AudioCategory`). Rewritten to match, and redirected to
   extend the already-comprehensive `AudioSelfTest.cs` rather than build a parallel test file for
   functionality that's already ~100+ assertions deep in the existing self-test.
8. **Effort/risk estimates were inflated by the false premise.** Original: ~15 new files, ~10
   modified, 3-4 sessions, "Priority: MEDIUM." Corrected: ~3-5 new files, ~2-4 modified, 1-2
   sessions. The one item that could grow this is an unconfirmed settings-UI-panel gap (Step 6),
   which should be scoped separately once `src/UI/` is actually checked — this review did not
   exhaustively check that directory and says so rather than guessing.
9. **Risk/rollback:** because the corrected batch is almost entirely additive (a thin port seam,
   a few new bridge subscriptions, possibly a settings panel), rollback is low-cost — revert the
   `AudioEventBridge.cs` diff and/or delete the new `IAudioPort`/`GodotAudioPort` files; neither
   touches `AudioManager`, `AudioCueCatalog`, or `AudioSettings`, so there is no risk of
   regressing the already-working playback system as long as implementers follow this corrected
   document instead of the original's instruction to rebuild it.
