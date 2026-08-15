# ASHFALL — Expansion 09: The Black Flotilla
## Master Expansion Design Bible & Integration Plan

**Expansion 09** is the maritime expansion: coastal wreck salvage, 4-room stealth dive
instances, procedural scavenge with environmental degradation, psychological contamination
from horrific locations, and deep-lore location content. It is the first expansion that
reaches beyond the shelter's walking radius into the water.

**Sister packs:** Expansions 1–8 (Holdfast, Duty Roster, Standing Record, Nobody's Charter,
Year of Ash, Muster, Dose, Verdict). This expansion reads their flags if present. It does
not reopen District 8, the Crossing, or the Tempest sites.

---

## §1 — Current State (Code Audit 2026-08-15)

### What exists

| File | Layer | State | Issues |
|---|---|---|---|
| `Ashfall.Core/Maritime/StealthDiveInstance.cs` | Core | ✅ Functional | Uses `Math.Clamp` (not `MathfCompat`); no `ISeededRng`; no catalog loader for `dive_sites.json`; no tests |
| `Assets/_Game/Core/ExpansionIXItemCatalog.cs` | Unity | ✅ 14 items | Unity `ScriptableObject`-only; not in Core; no Godot host registration |
| `Assets/_Game/Core/ProceduralScavengeSystem.cs` | Unity | ⚠️ Functional | Uses `System.Random` (non-deterministic); `UnityEngine.Mathf`; not ported to Core |
| `Assets/_Game/Survivors/PsychologicalContaminationSystem.cs` | Unity | ⚠️ Functional | Not in Core; no save/load envelope; no Godot host |
| `Assets/_Game/World/DeepLoreLocationCatalog.cs` | Unity | ⚠️ Data-only | Static class with hardcoded loot tables; not data-driven; not in Core |
| `Assets/StreamingAssets/Data/dive_sites.json` | Data | ⚠️ Stub | Only 1 site defined; no loader consumes it |
| Narrative refs in `jrnl_templates_cycle_c.json`, `regional_treaty_protocols.json` | Data | ✅ Present | Cross-references exist |

### What is missing

1. **No plan doc** (this document)
2. **No Godot host session** — no `BlackFlotillaHostSession`, no save store, no selftest
3. **No Core port** of ProceduralScavenge, PsychologicalContamination, or DeepLoreLocation
4. **No catalog loader** for `dive_sites.json`
5. **No tests** for any Exp 09 system
6. **No map seeder** — dive sites not on the wasteland graph
7. **No UI** — no dive HUD, no contamination panel, no scavenge log
8. **No integration** with existing expedition/loot systems
9. **Non-deterministic RNG** — `System.Random` in ProceduralScavenge violates determinism invariant
10. **Only 1 dive site** — content is a stub

---

## §2 — Architecture Decisions

### D1 — Core-first migration

All gameplay logic moves to `Ashfall.Core/Maritime/` as engine-agnostic plain C#.
The Unity-side classes become thin wrappers. The Godot host gets a session + save store.

**Systems to port:**
- `ProceduralScavengeSystem` → `Ashfall.Core/Maritime/ProceduralScavengeSystem.cs`
- `PsychologicalContaminationSystem` → `Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs`
- `DeepLoreLocationCatalog` → data-driven via `dive_sites.json` + `deep_lore_locations.json`

### D2 — Deterministic RNG

All RNG in Core ports uses `ISeededRng` (the same pattern as Utility AI, Reckoning,
Greenhouse). `System.Random` is banned from Core. Same seed ⇒ same scavenge yields,
same contamination rolls, same dive outcomes.

### D3 — Data-driven locations

`DeepLoreLocationCatalog` (static hardcoded class) becomes a JSON-loaded catalog:
- `deep_lore_locations.json` — location defs with variable loot tables
- `dive_sites.json` — expanded to 4+ sites with room hazard profiles
- Loader: `DeepLoreLocationCatalogLoader` in Core

### D4 — Save envelope

One `BlackFlotillaSave` envelope covering:
- `StealthDiveSaveState` (existing)
- `ProceduralScavengeState` (new — visit counts, degradation state)
- `PsychologicalContaminationState` (new — per-survivor contamination entries)
- Checksummed via `SaveChecksum` (same pattern as Verdict/Holdfast/Dose)

### D5 — Integration with existing systems

- **Expedition system**: dive sites become expedition targets; `StealthDiveInstance`
  runs as a sub-instance of an expedition
- **Inventory**: Exp 09 items register through `InventoryHostSession` (same as Verdict)
- **Needs/Morale**: psychological contamination feeds into morale/health via events
- **Journal**: contamination events produce journal entries via `JournalSystem`

---

## §3 — Implementation Phases

### Phase 0: Code Audit & Bug Fix (this pass)

**Goal:** Fix all bugs, warnings, silent errors in existing Exp 09 code before building on it.

- [ ] Fix `Math.Clamp` → `MathfCompat.Clamp` in `StealthDiveInstance`
- [ ] Replace `System.Random` with `ISeededRng` in `ProceduralScavengeSystem`
- [ ] Add `CaptureState`/`RestoreState` to `PsychologicalContaminationSystem`
- [ ] Expand `dive_sites.json` from 1 to 4+ sites
- [ ] Fix all compiler warnings in Exp 09 files
- [ ] Write unit tests for `StealthDiveInstance` (Core, already exists)

### Phase 1: Core Ports

**Goal:** Move Unity-only systems into `Ashfall.Core/Maritime/`.

- [ ] Port `ProceduralScavengeSystem` to Core with `ISeededRng`
- [ ] Port `PsychologicalContaminationSystem` to Core
- [ ] Create `DeepLoreLocationCatalogLoader` (JSON-driven)
- [ ] Create `deep_lore_locations.json` from the hardcoded catalog
- [ ] Expand `dive_sites.json` with full room hazard data
- [ ] All ports: `CaptureState`/`RestoreState`, events, `ISeededRng`

### Phase 2: Godot Host

**Goal:** Thin Godot session + save + selftest.

- [ ] `BlackFlotillaHostSession.cs` — wraps dive + scavenge + contamination
- [ ] `BlackFlotillaSaveStore.cs` — checksummed save to `user://`
- [ ] `--black-flotilla-selftest` in `HostCli.cs` (10+ checks)
- [ ] Register Exp 09 items in `InventoryHostSession.cs`
- [ ] Wire into `Main.cs` menu

### Phase 3: Tests

**Goal:** Full test coverage for all Core systems.

- [ ] `StealthDiveInstanceTests` — dive lifecycle, air supply, noise, compromise, save roundtrip
- [ ] `ProceduralScavengeTests` — Poisson rolling, degradation, determinism, contamination
- [ ] `PsychologicalContaminationTests` — application, expiry, work restrictions, mental breaks, save roundtrip
- [ ] `DeepLoreLocationCatalogTests` — loader, loot table validation, site integrity
- [ ] Integration test: dive → loot → contamination → journal

### Phase 4: UI & Presentation (deferred)

**Goal:** Player-facing surfaces.

- [ ] Dive HUD (air gauge, room progress, noise meter)
- [ ] Contamination panel (survivor status, work restrictions)
- [ ] Scavenge log (variable loot results, degradation notifications)
- [ ] Map nodes for dive sites and deep-lore locations

### Phase 5: Content Expansion (deferred)

**Goal:** Fill out the maritime content.

- [ ] 4+ dive sites with unique room layouts
- [ ] 10+ deep-lore locations with variable loot
- [ ] Dive-specific door encounters
- [ ] Keeper of Logs questline (referenced in `dive_sites.json`)
- [ ] Radio broadcasts on maritime frequencies

---

## §4 — Key Bugs Found (Phase 0)

### Bug 1: `Math.Clamp` in StealthDiveInstance

**File:** `StealthDiveInstance.cs`, `AdvanceToNextRoom()`
**Issue:** Uses `Math.Clamp(value, 0, 100)` which is .NET Core 2.1+ but not available in
`netstandard2.1` without a polyfill. The project uses `MathfCompat` for cross-platform
compat. **Fix:** Replace with `MathfCompat.Clamp(value, 0, 100)`.

### Bug 2: Non-deterministic RNG in ProceduralScavengeSystem

**File:** `ProceduralScavengeSystem.cs`
**Issue:** Uses `System.Random` which violates the determinism invariant ("same seed ⇒
same simulation in both engines"). **Fix:** Replace with `ISeededRng` port, add seed to
save state.

### Bug 3: No save/load on PsychologicalContaminationSystem

**File:** `PsychologicalContaminationSystem.cs`
**Issue:** Has `_bySurvivor` dictionary with contamination entries but no
`CaptureState`/`RestoreState`. Contamination silently resets on reload. **Fix:** Add
save/load envelope.

### Bug 4: Unity-only item catalog

**File:** `ExpansionIXItemCatalog.cs`
**Issue:** Uses `ScriptableObject.CreateInstance<ItemDefinition>()` — Unity-only.
Items not available in Godot host. **Fix:** Register items in `InventoryHostSession.cs`
(same pattern as Verdict items).

### Bug 5: dive_sites.json is a stub

**File:** `dive_sites.json`
**Issue:** Only 1 site defined (`ss_sovereign`). The `StealthDiveInstance` hardcodes
4 rooms regardless. No loader consumes this file. **Fix:** Expand to 4+ sites, create
loader.

### Bug 6: DeepLoreLocationCatalog is hardcoded

**File:** `DeepLoreLocationCatalog.cs`
**Issue:** Static class with hardcoded loot tables. Not data-driven. Cannot be loaded
by the Core or Godot. **Fix:** Extract to `deep_lore_locations.json`, create loader.

---

## §5 — File Manifest (planned)

### New Core files
- `Ashfall.Core/Maritime/ProceduralScavengeSystem.cs` (ported from Unity)
- `Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` (ported from Unity)
- `Ashfall.Core/Maritime/DeepLoreLocationCatalogLoader.cs` (new)
- `Ashfall.Core/Maritime/BlackFlotillaSave.cs` (new save envelope)

### New data files
- `Assets/StreamingAssets/Data/deep_lore_locations.json` (extracted from static class)
- `Assets/StreamingAssets/Data/dive_sites.json` (expanded from 1 to 4+ sites)

### New Godot host files
- `src/Host/BlackFlotillaHostSession.cs`
- `src/Host/BlackFlotillaSaveStore.cs`

### New test files
- `Ashfall.Core.Tests/BlackFlotillaTests.cs`

### Modified files
- `Assets/Ashfall.Core/Maritime/StealthDiveInstance.cs` (Math.Clamp fix)
- `src/Host/InventoryHostSession.cs` (+14 Exp 09 items)
- `src/Host/HostCli.cs` (+selftest)
- `src/Main.cs` (+menu button)

---

## §6 — Verification Gates

Each phase must pass before the next begins:

| Phase | Gate |
|---|---|
| 0 | `dotnet build Ashfall.csproj` 0 errors; `StealthDiveInstance` tests pass |
| 1 | `dotnet test Ashfall.Core.Tests` all pass; Core ports have save roundtrip tests |
| 2 | `godot --headless -- --black-flotilla-selftest` PASS |
| 3 | 30+ new tests, all pass |
| 4 | UI smoke test (manual) |
| 5 | Content validation (all sites reachable, all items registered) |
