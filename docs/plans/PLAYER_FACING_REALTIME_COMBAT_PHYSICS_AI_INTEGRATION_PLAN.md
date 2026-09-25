# PLAYER-FACING REAL-TIME COMBAT TETRAD — INTEGRATION PLAN

**Package ID:** `PFGL-RT-COMBAT-TETRAD-2026-09-25`
**Evidence HEAD:** `1678c074`
**Save pin (evidence):** `266`
**Mode:** Planning only until approval + signed DEC
**Framing:** True real-time combat rewrite (user-locked)
**On approval copy to:** `docs/plans/PLAYER_FACING_REALTIME_COMBAT_PHYSICS_AI_INTEGRATION_PLAN.md`

---

## 0. Executive summary

ASHFALL already has a deep combat stack — catalogs, ballistics, weapon wear, breaching, stealth noise, chem exposure, expedition ambush entry, and an Interactive `combat` panel — but the **live fight clock is turn-based** (`CombatPhase.PlayerTurn` / `EnemyTurn` / `EndTurn`). The player asked for **live combat** with survivor locomotion (walk / climb / run / aim / shoot) and the concrete risk of **getting shot while running away**.

This plan redesigns the encounter clock into a **deterministic fixed-tick real-time combat runtime** owned by Core, presented by Godot, without inventing a second combat authority, without putting gameplay physics rules into `CharacterBody2D`, and without Unity.

### Four packages (dependency order)

| # | Package ID | Mechanic focus | One-line outcome |
|---|---|---|---|
| W1 | `PFGL-RT-W1-COMBAT-CLOCK-LOCOMOTION` | Physics + survivor motion | Replace turn phases with fixed-tick Active combat; walk / run / climb / cover as Core motion modes |
| W2 | `PFGL-RT-W2-AIM-SHOOT-BALLISTICS` | Aiming + shooting | Continuous aim cone + fire cadence through existing `BallisticsSystem`; jam/reload/suppress live |
| W3 | `PFGL-RT-W3-ENEMY-AI-LIVE` | Enemy AI | Catalog `AiSpecialMove` becomes continuous timed behaviors (Burrow/Flank/Spore/Charge/SuppressiveFire/TacticalRetreat) |
| W4 | `PFGL-RT-W4-FLEE-INTERRUPT` | Run-away risk | Flee is a live extract corridor; enemies keep shooting; mid-escape hits can drop or pin fleeing survivors |

**Hard gate:** W1 Core clock work does not start until a new decision (`DEC-RT-COMBAT-REWRITE`, proposed name) is **SIGNED** in `docs/governance/DECISION_REGISTER.md`. Planning and forensic re-rg may proceed; production mutation of `TacticalCombatSystem` phase model waits for that signature.

---

## 1. Objective

Deliver a player-operable **real-time tactical firefight** in Godot where:

1. Survivors move continuously (walk / run / climb / take cover) on an encounter plane.
2. Players aim and shoot with live cadence, recoil/noise, and ballistics resolution.
3. Enemies act continuously with authored special moves that feel distinct.
4. Choosing to run away keeps the squad under fire until extract succeeds or fails — **getting shot while fleeing is a first-class outcome**, not a single end-of-action coin flip only.

Non-goals for this tetrad:

- Full open-world FPS traversal of the wasteland map.
- Replacing expedition graph travel with CharacterBody navigation.
- A second combat save section or parallel `LiveCombatSystem` beside `TacticalCombatSystem`.
- Unity restoration.
- Rewriting shelter interior `SurvivorActorView` room-seek into combat authority.
- Ballistic-shield / sky-defense / perimeter polish packages (runner-ups).

---

## 2. Current reality (evidence)

### 2.1 Live authority today

| Concern | Owner | Evidence |
|---|---|---|
| Encounter owner | `TacticalCombatSystem` + `CombatHostSession` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem*.cs`, `src/Host/CombatHostSession.cs` (~771 lines) |
| Space model | 3 lanes (`CombatLane`) + stances | `CombatTypes.cs`, stance mods in `TacticalCombatSystem.cs` |
| Shot resolution | `BallisticsSystem.Resolve` | Called from `PlayerFire` path; `BarrierMaterial` currently forced `null!` in fire context |
| Enemy turn | Generic fire × `AiAccuracyMod` / `AiDamageMod` | `TacticalCombatSystem.Damage.cs` `EndTurn` — **does not branch on `AiSpecialMove`** |
| AI move catalog | `CombatAiMove` enum + `CombatAiMoves.AllowedNames` | Burrow, Flank, Spore, Charge, SuppressiveFire, TacticalRetreat |
| Retreat | `PlayerRetreat` mobility roll | Clean extract **or** discrete injury: `"Retreat disrupted — someone is hit."` (`TacticalCombatSystem.Actions.cs` ~476–523) |
| Panel | `CombatPanel` Interactive | Fire / Suppress / stances / jam / repair / retreat / end turn; jam/repair/last-stand hardcode `"survivor_yuki"` |
| Host verbs | `ActionFire`…`ActionEndTurn`, `ActionMoveLane`, `ActionBandage` | **No** `ActionReload`; MoveLane/Bandage not on panel |
| Stealth bridge | `ActionFire` → `Stealth.ApplyWeaponNoise` | Live |
| Shelter presentation physics | `SurvivorActorView : CharacterBody2D` | Gravity + seek + `MoveAndSlide`; **presentation only** — Core owns room assignment |
| Save | section `combat` | Related: `expedition_stealth`, `ballistics_workbench`, `ballistic_shield`, `chem_warfare`, `sound_ranging`, `perimeter_defense`, `sky_defense_battery`, `settlement_defenses` |
| Manifest | `combat` = InteractiveCommands; `combat_hud` / `stealth` / `vault_door_breaching` = ReadOnlyObservational | `docs/player_surface_manifest.json` |

### 2.2 Fantasy verbs → today’s analogs → required live mapping

| Player fantasy | Today | Required live mapping |
|---|---|---|
| Walk | `PlayerMoveLane` (discrete, panel-missing) | Continuous walk speed toward cover / lane node |
| Run | Stance `Retreat` mobility / flee roll | Sprint speed + stamina/fatigue drain via existing needs bridge |
| Climb | Missing as combat verb; breach is barrier clear | Climb motion mode on vertical edges / breach ledges (encounter geometry tags) |
| Aim | Stance accuracy mods | Aim vector / cone held while moving or braced |
| Shoot | `PlayerFire` / Suppress | Fire cadence, ADS brace bonus, continuous ammo/jam |
| Get shot running away | Failed `PlayerRetreat` one-shot injury | Flee phase under continuous enemy fire until extract radius |

### 2.3 Why a rewrite needs DEC

Replacing `PlayerTurn`/`EnemyTurn`/`EndTurn` with a continuous clock changes the combat contract for every consumer: preflight guards (`Phase != PlayerTurn`), UI End Turn button, CLI demos, expedition ambush harnesses, chem post-turn exposure, save phase enums, and tests. That is an architecture decision under Rule 10 — **stop and sign**, then implement.

No signed DEC currently authorizes a real-time combat clock (register scan at evidence HEAD shows combat DECs for catalog, standing bridge, needs cascade, etc., not a realtime rewrite).

---

## 3. Required delta

### Existing behavior
Turn-based tactical binder: player spends actions, presses End Turn, enemies fire once each, retreat is a single mobility check that either extracts cleanly or applies one injury event.

### Requested behavior
Live firefight: survivors move and shoot in continuous time; enemies hunt and use special moves continuously; fleeing keeps the squad targetable until extract.

### Delta (minimum safe)
1. Evolve **one** Core encounter owner into a fixed-tick realtime runtime (do not fork a second system).
2. Add motion modes + encounter geometry (walk/run/climb/cover/flee) as Core state.
3. Drive aim/fire through existing ballistics + weapon condition.
4. Resolve `AiSpecialMove` every AI think interval.
5. Replace discrete retreat coin-flip primacy with a **flee corridor** that still can resolve to `CombatPhase.Retreated` / injury / loss.
6. Host presents with Godot bodies that **mirror** Core poses; Core stays engine-free.
7. Keep aftermath, faction standing, stealth noise, chem exposure, and `combat` save ownership.

---

## 4. Evidence index (re-verify at claim time)

Live code wins over this plan. Re-rg before each package claim:

```bash
git rev-parse --short HEAD
rg -n "enum CombatPhase|EndTurn|PlayerFire|PlayerMoveLane|PlayerRetreat|AiSpecialMove" Assets/Ashfall.Core/Combat -g '*.cs'
rg -n "ActionFire|ActionEndTurn|ActionMoveLane|DefaultPlayerSubjectId|survivor_yuki" src/Host/CombatHostSession.cs src/UI/CombatPanel.cs
rg -n "CharacterBody2D|MoveAndSlide" src/World/SurvivorActorView.cs src/Main.GameFlow.cs
rg -n "\"combat\"|InteractiveCommands" docs/player_surface_manifest.json
```

Pinned findings at `1678c074`:

- `EndTurn` enemy loop ignores `AiSpecialMove` (accuracy/damage mods only).
- `CombatPanel` hardcodes `survivor_yuki` for jam/repair/last-stand.
- `PlayerRetreat` already encodes “hit while fleeing” as failed mobility → `ApplyDamage` + `"Retreat disrupted — someone is hit."`
- `SurvivorActorView` documents presentation-only physics.
- `BarrierMaterial = null!` in fire context (breaching/ballistics feed gap; secondary to clock rewrite).

---

## 5. Existing extension seams (reuse, do not duplicate)

| Seam | Reuse plan |
|---|---|
| `TacticalCombatSystem` | Evolve into realtime tick owner; keep name or additive partial `TacticalCombatSystem.Realtime.cs` |
| `BallisticsSystem` | Per-shot resolve from live aim context |
| `WeaponConditionSystem` + `WeaponEquipmentBridge` | Jam/wear/ammo on live fire |
| `CombatAiMoves` | Behavior table keys for W3 |
| `StealthSystem.ApplyWeaponNoise` | Keep on successful live shots |
| `ChemWarfareSystem.EvaluateActorExposure` | Shift from post-`EndTurn` to periodic exposure tick |
| `CombatHostSession` | Replace ActionEndTurn primacy with Start/Stop tick pump + input frames |
| `CombatPanel` / future `CombatArenaHud` | Input surface for move/aim/fire/flee |
| `combat` save section | Extend DTO; avoid new section unless DEC says otherwise |
| `EnemyCompositionSelector` | Keep ambush/raid composition entry |
| `NeedsPerformanceBridge` | Accuracy/mobility multipliers during live tick |
| `SurvivorActorView` patterns | **Pattern only** for Godot body presentation — combat arena view is a separate node family |

---

## 6. Proposed architecture

### 6.1 One authority rule

```
TacticalCombatSystem (Core)     = sole mutable encounter authority
CombatHostSession (Host)        = input pump, seed, presentation bind, noise/chem bridges
CombatArenaView (Godot)         = mirrors Core poses; no damage math
BallisticsSystem                = pure shot resolve (unchanged ownership)
combat save section             = sole persistence for encounter state
```

Forbidden parallels: `RealtimeCombatSystem`, host-side HP ledgers, FPS controller that applies damage locally, vault-door prototype as breach authority, second AI manager.

### 6.2 Realtime clock (W1)

```
CombatPhase (evolved):
  Setup → ActiveRealtime → (Won | Lost | Retreated)

ActiveRealtime:
  Host pumps fixed dt (e.g. 1/20 or 1/30 sim second) via
    CombatHostSession.PumpRealtime(dt, CombatInputFrame)
  → TacticalCombatSystem.TickRealtime(dt, input, rng)

Tick order (deterministic):
  1. Apply player input (motion intent, aim, fire, reload, flee request)
  2. Integrate motion (walk/run/climb/cover) with stamina/fatigue gates
  3. Resolve pending player shots (cadence timers)
  4. Enemy perception + AI special-move state machines (W3)
  5. Enemy shots / melee / spore ticks
  6. Flee extract progress + interrupt hits (W4)
  7. Bleed / pin / chem exposure periodic
  8. CheckResolution
```

Determinism: sim uses **fixed dt** and `ISeededRng` from `CampaignStreamIds.Combat`. Wall-clock only schedules how many pumps the host applies; replay/tests call `TickRealtime` directly with the same dt sequence.

### 6.3 Encounter geometry (physics without Godot in Core)

Core stores an engine-free arena:

- Nodes: cover points, lane spines, climb segments, extract volume, barrier volumes.
- Combatant pose: `PositionX/Y`, `Velocity`, `Facing`, `MotionMode`, `AimAngle`, `Stamina01`.
- Collision: simple AABB / segment tests in Core (no Godot types).
- Host `CombatArenaView` spawns `CharacterBody2D` (or kinematic mirrors) that lerp/snap to Core poses for juice; **hit detection for gameplay uses Core**, not Godot contact.

Climb: authored climb segments on barriers/ledges; motion mode `Climb` consumes stamina and ignores horizontal sprint until segment complete — this is the live stand-in for “climbing” without a parkour engine.

### 6.4 Aim + shoot (W2)

- Input: aim angle or aim-at combatant id; brace/ADS flag while walk-speed or stationary.
- Fire request sets a cadence timer from weapon catalog (extend `combat_catalog` / weapon defs if ROF missing — prefer existing fields).
- Each shot builds `BallisticsContext` from live pose, cover, optional `BarrierMaterial` when LOS clips a barrier.
- Suppress = longer cadence / pin application (reuse suppress semantics).
- Reload / clear jam / field repair become realtime channel actions (still Core verbs; host wraps `PlayerReload` which exists in Core but lacks host Action today).

### 6.5 Enemy AI live (W3)

Per enemy: `AiSpecialMove` selects a behavior tree/state machine ticked on an interval:

| Move | Live behavior sketch |
|---|---|
| Burrow | Temporary untargetable / cover regen; emerge with flank reposition |
| Flank | Path along outer lane spine to side cover; accuracy bonus when behind player facing |
| Spore | Periodic chem hazard seed via existing chem seam (explicit handoff, no new toxin ledger) |
| Charge | Sprint toward player; melee or close-range damage pulse on contact radius |
| SuppressiveFire | High cadence, low accuracy, applies pin |
| TacticalRetreat | Enemy seeks extract / deep cover when below `FleeThreshold` |

Generic fire remains fallback when move is `None` or behavior completes.

### 6.6 Flee interrupt (W4)

Expand today’s retreat into a **Flee motion mode**:

1. Player issues Flee (replaces instant `PlayerRetreat` success path as primary).
2. Squad motion mode → `Flee` toward extract volume; sprint speed; reduced aim (hip fire only or no fire — DEC chooses; recommend hip-fire allowed at heavy accuracy penalty so “running gunfight” exists).
3. Enemies keep AI + shooting every tick.
4. On hit during Flee: apply damage; chance to trip/pin (`IsPinned`) delaying extract; downed survivors need bandage or are abandoned per existing downed rules.
5. When all living players enter extract volume for `ExtractHoldSeconds` → `CombatPhase.Retreated`, aftermath `-2f` (preserve current aftermath tone) or tuned value.
6. Legacy single-roll `PlayerRetreat` remains as a **compat/debug** path or is redirected to begin Flee mode (prefer redirect so one authority).

This preserves the user’s explicit requirement: **possibility of getting shot while running away**.

---

## 7. Ownership matrix

| Concern | Owner |
|---|---|
| Encounter mutation / HP / pose / phase | `TacticalCombatSystem` |
| Shot math | `BallisticsSystem` |
| Weapon wear tokens | `WeaponConditionSystem` → equipment via bridge |
| AI move legality strings | `CombatAiMoves` |
| Stealth noise | `StealthSystem` (host calls on shot) |
| Chem exposure | `ChemWarfareSystem` |
| Composition pick | `EnemyCompositionSelector` |
| Input + pump + seeds | `CombatHostSession` |
| Presentation bodies / camera / VFX | Godot `CombatArenaView` (+ HUD) |
| Authored arena + combatant defs | JSON under `Assets/StreamingAssets/Data/` |
| Persistence | existing `combat` save section (+ additive realtime fields) |
| Decision to replace turn clock | `DEC-RT-COMBAT-REWRITE` (proposed) |

---

## 8. Data flow

```
Player input (keyboard/pad)
  → CombatArenaHud / CombatPanel
  → CombatHostSession.BuildInputFrame()
  → PumpRealtime(dt, frame)
  → TacticalCombatSystem.TickRealtime
  → BallisticsSystem / AI / Flee / Bleed
  → Domain events (shot, hit, flee_hit, extract, resolved)
  → Host bridges (stealth noise, chem, journal hooks)
  → UI refresh + arena pose sync
  → Save capture on pause/exit if ActiveRealtime allows mid-fight save (see §12)
```

---

## 9. State model (additive)

Proposed additive fields on combatant / encounter state (names illustrative — implementer matches local style):

**CombatantState additions**

- `float PosX, PosY`
- `float VelX, VelY`
- `float FacingRad`
- `float AimRad`
- `byte MotionMode` — Idle/Walk/Run/Climb/Cover/Flee/Downed
- `float Stamina01`
- `float FireCooldown`
- `float AiThinkCooldown`
- `string AiBehaviorPhase` — behavior-local phase id
- `float ExtractProgress01` — flee hold

**CombatState additions**

- `bool RealtimeActive`
- `float SimTime`
- `int SimTick`
- `string ArenaId`
- Keep `Phase` but ActiveRealtime replaces PlayerTurn/EnemyTurn during fights; migrate saves carefully (§12)

**CombatInputFrame** (host → Core, not persisted)

- Move axis, sprint, climb request, aim, fire, reload, suppress, flee, subject id

Invariants:

- Only one MotionMode at a time.
- Downed ⇒ no voluntary motion except crawl if already supported; else immobile.
- RealtimeActive false outside ActiveRealtime.
- Core never references Godot types.

---

## 10. API / contracts

### Core (evolve)

```
void BeginEncounter(... existing ...) // seeds poses from lane defaults
void TickRealtime(float dt, CombatInputFrame input, ISeededRng rng)
ActionPreflight EvaluateFlee()
CombatActionResult RequestFlee() // enters Flee mode; does not instantly resolve
CombatActionResult RequestReload(string subjectId)
// Keep PlayerFire etc. as internal helpers called from TickRealtime OR thin wrappers
```

Deprecate as **player-facing primary** (retain temporarily for tests/CLI migration):

- `EndTurn` as the main cadence driver
- Instant-success path of `PlayerRetreat` without flee corridor (redirect)

### Host

```
void SetRealtimePumpEnabled(bool)
void PumpRealtime(float dt) // reads last input frame
void SetInputFrame(CombatInputFrame)
string ActionReload(string subjectId)
string ActionRequestFlee()
// ActionEndTurn → either no-op with message "realtime combat" or advances one AI think for debug
```

### UI

- Promote arena + HUD: move stick/buttons, aim, fire, reload, flee.
- Remove or demote End Turn as primary (keep debug).
- Replace `survivor_yuki` hardcode with `DefaultPlayerSubjectId()` / subject picker (still required even under realtime).

---

## 11. Data changes

| File | Change |
|---|---|
| `combat_catalog.json` | Optional per-weapon `rounds_per_minute` / `aim_brace_bonus` if missing; keep ai_special_move |
| New `combat_arenas.json` (proposed) | Arena ids, cover nodes, climb segments, extract volumes, spawn anchors |
| `breaching_equipment_catalog.json` | Unchanged ownership; barriers can tag climb/breach volumes |
| `camouflage_gear.json` / stealth | Unchanged; prep loop remains separate (optional follow-on) |
| `needs_performance.json` | Optional sprint drain multipliers if not already covered |
| Schemas / integrity validators | Register new arena catalog |

Prefer extending catalogs over hardcoding speeds in C#.

---

## 12. Save / load

- **Prefer:** mid-fight save allowed — persist poses, cooldowns, AiBehaviorPhase, SimTick, ArenaId additively on existing `combat` DTO / `CombatSaveStore`.
- **Fallback if DEC rejects mid-fight save:** auto-resolve or force Retreated/Lost on save request during ActiveRealtime; document clearly.
- Version bump inside combat save codec; old saves without poses: on restore, place combatants on lane default anchors and set MotionMode Idle.
- Pin 266: additive fields inside existing section **should not** require pin bump; new section would. Avoid new section.
- Round-trip tests mandatory for pose + flee progress + AI phase.

---

## 13. Determinism

- Fixed `dt` in tests (e.g. `1f/20f`).
- Host may use rendered frame time only to decide **how many** fixed pumps to run (accumulator pattern); never feed variable dt into damage math.
- All RNG via `CampaignStreamIds.Combat` forks (`RollSeed` pattern already in `CombatHostSession`).
- Sort enemy tick order by stable combatant id.
- No `System.Random`.
- Presentation interpolation must not affect Core.

---

## 14. System / event wiring

| Event / hook | When |
|---|---|
| `enemy_fire` / new `player_fire` | On shot resolve |
| `flee_start`, `flee_hit`, `flee_extract` | W4 |
| `ai_special_*` | W3 phase changes |
| `OnEncounterEnded` | Unchanged terminal |
| Stealth noise | On successful player shot |
| Chem exposure | Every N ticks or every 1.0s sim |
| Faction standing aftermath | Unchanged on resolve |

Expedition ambush / muster raid entry keeps calling `StartCombat` / `BeginEncounter`; after begin, host enables realtime pump instead of expecting End Turn.

---

## 15. Godot integration

1. `CombatArenaView` (new) under `src/World/` or `src/UI/Combat/`:
   - Spawns mirrored bodies for combatants.
   - Camera framing.
   - Reads Core snapshot poses each frame.
2. Input: map walk/run/climb/aim/fire/reload/flee; keep keyboard/controller close/back.
3. `CombatPanel`: becomes command chrome + status, or splits into arena + chrome.
4. Do **not** use shelter `SurvivorActorView` as the combat pawn authority.
5. Headless: CLI pump N ticks without rendering; extend combat selftest.

Accessibility: pause pump on panel focus loss if needed; readable hit feedback; never soft-lock without Flee/Retreat path.

---

## 16. Narrative / content integration

- Journal/combat log lines for flee hits and special moves.
- Bestiary combat tactics unlocks can later cite live move names (no requirement to change bestiary this tetrad).
- Diegetic copy stays restrained; no real-world army names.

---

## 17. Failure modes

| Failure | Expected behavior |
|---|---|
| DEC unsigned | Block W1 Core clock mutation; report blocker |
| Arena catalog missing | Fall back to 3-lane spine procedural arena from lane enums |
| dt storm / hitch | Accumulator caps max pumps per frame (e.g. 5) to avoid spiral |
| All players downed mid-flee | `Lost` as today |
| Extract with downed left behind | Document policy: extract living only; downed → Lost or captured — **DEC sub-clause**; recommend living extract + downed count as casualties in aftermath |
| Climb with 0 stamina | Reject climb; message |
| Fire while climbing | Blocked or heavy penalty — recommend blocked |
| Old save mid PlayerTurn | Migrate into ActiveRealtime at lane anchors |
| Host forgets to pump | Encounter frozen; UI shows paused; no silent resolve |
| Parallel FPS damage in Godot | Forbidden; tests assert Core HP authority |
| Spore without chem system | Skip spore effect; log once; do not invent toxin store |

---

## 18. Test strategy

Focused only (`bash scripts/run_test.sh …`); no full suite by default.

### W1
- TickRealtime moves combatant Walk→position change deterministic
- Run faster than Walk for same dt×N
- Climb along segment completes; blocked without segment
- Phase machine Setup→ActiveRealtime→Won/Lost/Retreated
- Save round-trip poses

### W2
- Fire cadence: N ticks → expected shot count
- Ballistics still called; ammo decrements; jam path
- ActionReload host wrap
- Aim brace improves accuracy vs hip (assert via seeded scenario)

### W3
- Each `AiSpecialMove` enters distinct behavior phase within K ticks
- Charge closes distance; SuppressiveFire applies pin; TacticalRetreat increases distance when below flee threshold
- Catalog illegal move rejected at load (existing)

### W4
- Flee under fire: forced enemy accuracy scenario scores ≥1 `flee_hit` before extract in adversarial fixture
- Clean extract with enemies wiped or far → Retreated without hit
- Failed extract hold interrupted by pin
- Compatibility: RequestFlee from former Retreat button

### Host / headless
- PumpRealtime N ticks selftest
- Panel subject id ≠ hardcoded yuki
- Godot headless arena smoke only if arena scene lands

---

## 19. Dependency-ordered phases

### Phase −1 — DEC gate (blocker)

Write and obtain signature for `DEC-RT-COMBAT-REWRITE` covering:

- Single authority remains `TacticalCombatSystem`
- Fixed-tick realtime replaces PlayerTurn/EnemyTurn as live fight clock
- Godot presentation mirrors Core
- Flee corridor includes mid-escape hits
- Save strategy (mid-fight vs force-resolve)
- Downed-on-flee policy

**Completion gate:** SIGNED row in decision register.
**Must not:** merge Core phase-model PRs before signature.

### Phase 0 — Verification baseline

Re-rg APIs; run existing combat-focused tests; snapshot `PlayerRetreat` fail-hit behavior; claim paths in `WORKTREE_OWNERSHIP.md`.

### Phase 1 — W1 Core clock + locomotion

Files likely: `TacticalCombatSystem.cs`, new `TacticalCombatSystem.Realtime.cs`, `CombatTypes.cs`, arena catalog loader, tests.
Gate: deterministic Walk/Run/Climb + ActiveRealtime phase green.

### Phase 2 — W1 Host pump + arena mirror stub

`CombatHostSession` pump; minimal arena view; demote End Turn.
Gate: headless pump moves state; UI shows positions or debug overlay.

### Phase 3 — W2 Aim / shoot cadence

Wire fire from TickRealtime; host ActionReload; panel/HUD aim-fire; stealth noise retained.
Gate: live shooting scenario test + panel operable without End Turn.

### Phase 4 — W3 Enemy AI live

Behavior table for six moves; replace EndTurn generic loop.
Gate: per-move tests; encounters feel distinct in CLI pump script.

### Phase 5 — W4 Flee interrupt

RequestFlee corridor; flee_hit events; extract hold; Retreat button → Flee.
Gate: adversarial “shot while running” test passes; clean extract path passes.

### Phase 6 — Persistence + migration

Additive save fields; old save placement; round-trips.
Gate: focused persistence tests.

### Phase 7 — Polish / chem / breach feed (bounded)

Periodic chem exposure; optional BarrierMaterial LOS feed; remove yuki hardcode everywhere.
Gate: no new section; focused tests.

### Phase 8 — Acceptance battery

Package-focused tests + combat selftest ticks + manifest actionCoverage updates for combat HUD if it becomes interactive.

---

## 20. File impact map

| Path | Action | Reason | Risk |
|---|---|---|---|
| `docs/governance/DECISION_REGISTER.md` | MODIFY | Sign DEC-RT-COMBAT-REWRITE | Process |
| `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` | MODIFY | Phase model / entry | High |
| `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Realtime.cs` | CREATE | TickRealtime / motion | High |
| `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Damage.cs` | MODIFY | Retire EndTurn primacy; AI tick | High |
| `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Actions.cs` | MODIFY | Flee redirect; reload use | High |
| `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Persistence.cs` | MODIFY | Pose fields | Med |
| `Assets/Ashfall.Core/Combat/CombatTypes.cs` | MODIFY | MotionMode, phases | Med |
| `Assets/Ashfall.Core/Combat/CombatAiMove.cs` | READ/optional MODIFY | Behavior keys | Low |
| `Assets/Ashfall.Core/Combat/BallisticsSystem.cs` | READ / small MODIFY | Context from poses | Med |
| `Assets/StreamingAssets/Data/combat_arenas.json` | CREATE | Geometry | Med |
| `Assets/StreamingAssets/Data/combat_catalog.json` | MODIFY | ROF / motion params if needed | Med |
| `src/Host/CombatHostSession.cs` | MODIFY | Pump + input + reload/flee | High |
| `src/UI/CombatPanel.cs` | MODIFY | Live controls; drop yuki | High |
| `src/World/CombatArenaView.cs` (name flexible) | CREATE | Presentation | Med |
| `Ashfall.Core.Tests/Combat/*Realtime*` | CREATE | Focused tests | Low |
| `docs/player_surface_manifest.json` | MODIFY | Coverage truth | Low |
| `WORKTREE_OWNERSHIP.md` | MODIFY | Claims | Process |
| `INTEGRATION_PLANS.md` | MODIFY | Foreman/integrator only | Process |
| `src/World/SurvivorActorView.cs` | READ ONLY | Pattern reference | — |
| `VaultDoorBreachingPanel` | READ ONLY / later | False parallel | — |

---

## 21. Risks

| Risk | Mitigation |
|---|---|
| Scope explosion into full FPS RPG | Bound arena size; no wasteland navmesh; climb = segment tags only |
| Determinism break from variable dt | Fixed step + accumulator cap |
| Dual combat clocks during migration | Feature flag `RealtimeActive`; CLI demos updated in same package |
| AI too expensive | Think intervals 0.25–0.5s sim; simple steering |
| Player confusion (management game → action combat) | Keep pauseable pump; shelter loops unchanged; combat is encounter modality |
| Save pin accidents | Additive DTO only |
| Chem/Stealth regressions | Keep existing host bridges; add tick hooks carefully |
| Claim collisions with Triad B / other agents | Claim combat files before edit; Triad B owns sanitation/exercise/dream — disjoint if claims honored |

---

## 22. Out of scope

- Stealth travel mode Interactive panel (strong follow-on; Core verbs already live).
- Ballistic shield phalanx UI.
- Full breaching player loop (can feed BarrierMaterial once realtime LOS exists; own package).
- Perimeter / sky defense / sound ranging classification debt.
- Shelter room-seek CharacterBody redesign.
- Unity.
- New medical trauma system.
- Netcode / multiplayer.

---

## 23. Rollback strategy

1. DEC rejected → plan archives; no Core clock PR.
2. W1 unstable → feature-flag `RealtimeActive=false` restores EndTurn path behind `#if` or runtime switch for one release.
3. Save fields additive → old builds ignore unknown fields if codec tolerant; otherwise migration revert script.
4. Small commits: DEC → Core tick skeleton → host pump → aim/fire → AI → flee → save.
5. Never force-push shared main; use work branch per git workflow topic.

---

## 24. Definition of Done

- [ ] `DEC-RT-COMBAT-REWRITE` SIGNED
- [ ] Player can walk, run, and climb (segment) in an active encounter without End Turn
- [ ] Player can aim and shoot with live cadence; reload reachable
- [ ] Enemies execute catalog special moves as distinct live behaviors
- [ ] Fleeing squad can be hit mid-escape; clean extract still possible
- [ ] Core remains engine-free; Godot mirrors poses
- [ ] One combat authority; one `combat` save section
- [ ] Focused tests green for W1–W4; CLI pump selftest PASS
- [ ] `survivor_yuki` hardcode removed from combat actions
- [ ] Manifest + ownership ledger updated by integrator
- [ ] Handoff written per `AI_AGENT_WORKFLOW.md`

---

## 25. Implementation handoff

### MUST PRESERVE
- `TacticalCombatSystem` as sole encounter authority
- `BallisticsSystem`, weapon condition bridge, stealth noise on fire, chem exposure owner
- Aftermath / faction standing / expedition StartCombat entry
- Engine-free Core
- Deterministic seeded RNG
- Existing `combat` save section identity

### MUST ADD
- Signed DEC before clock rewrite
- `TickRealtime` + motion modes + arena catalog
- Host input pump + arena presentation
- Live AI behaviors for `CombatAiMoves`
- Flee corridor with mid-escape hits
- Focused tests proving shot-while-fleeing

### MUST NOT DO
- Second combat system or FPS damage in Godot
- Unity
- Full wasteland physics traversal
- New save section without DEC
- Invent AI move names outside catalog
- Treat CLI PASS as player-operable proof without UI/effect tests
- Start Core phase-model edits before DEC signature / WORKTREE claim

### VERIFY WITH
```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/<new_realtime_tests>
# plus existing combat focus files touched
godot --headless <combat arena smoke if added>
```

### FIRST SAFE IMPLEMENTATION STEP
1. Draft and sign `DEC-RT-COMBAT-REWRITE`.
2. Claim `TacticalCombatSystem*`, `CombatHostSession`, `CombatPanel`, arena view path in `WORKTREE_OWNERSHIP.md`.
3. Land `TickRealtime` skeleton that still can resolve encounters, with Walk motion only, behind explicit RealtimeActive flag.
4. Add one deterministic test: N ticks of Walk changes PosX.
5. Stop and re-rg before W2.

---

## 26. Package dossiers (quick cards)

### W1 — `PFGL-RT-W1-COMBAT-CLOCK-LOCOMOTION`
- **Gap:** Turn clock cannot express live walk/run/climb.
- **Delta:** ActiveRealtime + poses + motion modes + arena catalog + host pump.
- **DEC:** Required (parent DEC).
- **Claim:** Core combat partials, host session, new arena view, data arena JSON.

### W2 — `PFGL-RT-W2-AIM-SHOOT-BALLISTICS`
- **Gap:** Fire is action-atomic; reload host-missing; aim is stance-only.
- **Delta:** Cadence fire from TickRealtime; aim vector; ActionReload; HUD controls.
- **Depends on:** W1 pump.
- **DEC:** Covered by parent if cadence params stay in catalog.

### W3 — `PFGL-RT-W3-ENEMY-AI-LIVE`
- **Gap:** `AiSpecialMove` authored but inert in EndTurn.
- **Delta:** Continuous behavior phases for six moves.
- **Depends on:** W1 (poses); benefits from W2 (incoming fire).
- **DEC:** Parent; Spore→chem handoff note.

### W4 — `PFGL-RT-W4-FLEE-INTERRUPT`
- **Gap:** Retreat is one roll; user wants live run-away fire risk.
- **Delta:** Flee mode + extract volume + flee_hit under enemy fire.
- **Depends on:** W1 motion + W3 enemy shooting during flee.
- **DEC:** Parent includes downed-on-flee policy sub-clause.

---

## 27. Relationship to prior PFGL plans

| Plan | Relationship |
|---|---|
| PFGL master W1–W10 | Orthogonal shelter/social loops; do not block |
| Triad B (Exercise/Dream/Sanitation) | Disjoint claims if ownership honored; integrate in parallel only with separate claims |
| Turn-based panel seal ideas | Superseded as primary approach by this realtime rewrite; still steal “remove yuki hardcode” and ActionReload |

---

## 28. Integrator notes (mimo / gemini / human)

1. Re-rg live APIs at claim time — plan text is not authority.
2. No full test suite; focused `scripts/run_test.sh` only.
3. CLI selftest PASS ≠ player-operable; exercise arena HUD.
4. Claim → implement → focused verify → handoff.
5. If DEC is refused, stop; do not silently ship a parallel realtime island.

---

## 29. One-line verdict

ASHFALL’s combat content is rich but **clocked wrong for the requested fantasy**; this tetrad signs a DEC, evolves `TacticalCombatSystem` into a fixed-tick realtime owner, and seals **locomotion physics, aim/shoot, live enemy AI, and shot-while-fleeing** as four player-facing packages under one authority.



---

## 30. Deep package — W1 Combat clock & locomotion

### 30.1 Existing behavior
Encounters flip `CombatPhase` between `PlayerTurn` and `EnemyTurn`. Motion is discrete `PlayerMoveLane`. Shelter `SurvivorActorView` already proves Godot `CharacterBody2D` presentation patterns but explicitly refuses gameplay authority.

### 30.2 Requested behavior
While an encounter is active, simulation time advances in fixed ticks. Survivors occupy continuous positions. Players issue walk / run / climb intents and see bodies move. Ending a turn is no longer required to let enemies act — enemies act on their own think intervals inside the same tick loop.

### 30.3 Delta
- Add `ActiveRealtime` (or map `PlayerTurn`/`EnemyTurn` into a single active bucket with subclock — prefer explicit `ActiveRealtime` for clarity).
- Add pose + `MotionMode` fields.
- Add `TickRealtime`.
- Author `combat_arenas.json` with at least one default arena (`arena_lane_spine_default`) generated from the three-lane mental model so old content still makes sense.
- Host accumulator pump.

### 30.4 Motion mode table

| Mode | Max speed (catalog) | Stamina | Aim allowed | Notes |
|---|---|---|---|---|
| Idle | 0 | regen | yes brace | cover bonus if in cover volume |
| Walk | `walk_speed` | neutral | yes | default move |
| Run | `run_speed` | drain | hip only | noise bump optional via stealth bridge later |
| Climb | `climb_speed` | drain | no | requires segment overlap |
| Cover | 0 or creep | regen+ | brace+ | must be inside cover volume |
| Flee | `flee_speed` ≥ run | heavy drain | hip only / DEC | W4 |
| Downed | 0 | — | no | bleed rules unchanged |

Suggested default numbers (tune in JSON, not code): walk 2.4 u/s, run 4.8, climb 1.6, flee 5.2 on a normalized arena width ~24 units (8 units per lane spine).

### 30.5 Arena schema sketch

```json
{
  "schema_version": 1,
  "arenas": [
    {
      "id": "arena_lane_spine_default",
      "width": 24.0,
      "height": 10.0,
      "lane_spines": [
        {"lane": 0, "x": 4.0},
        {"lane": 1, "x": 12.0},
        {"lane": 2, "x": 20.0}
      ],
      "cover_nodes": [
        {"id": "cover_p_left", "x": 3.0, "y": 2.0, "radius": 1.0, "cover_rating": 0.35}
      ],
      "climb_segments": [
        {"id": "climb_barrier_a", "x0": 10.0, "y0": 0.0, "x1": 10.0, "y1": 3.0}
      ],
      "extract_volume": {"x": 0.5, "y": 1.0, "w": 2.0, "h": 8.0},
      "player_spawns": [{"lane": 1, "x": 6.0, "y": 1.0}],
      "enemy_spawns": [{"lane": 1, "x": 18.0, "y": 1.0}]
    }
  ]
}
```

Lane remains a derived property from nearest spine for ballistics lane-match legacy rules during migration.

### 30.6 BeginEncounter pose seeding
On begin:
1. Resolve `ArenaId` (location mapping table or default).
2. Place each player on `player_spawns` by roster order.
3. Place enemies on `enemy_spawns` / composition count with jitter from seeded rng (±0.3).
4. Set `RealtimeActive=true`, `Phase=ActiveRealtime`, `SimTick=0`.

### 30.7 Host pump pseudocode

```
_accum += wall_dt
pumps = 0
while _accum >= SimDt && pumps < MaxPumps:
    Engine.TickRealtime(SimDt, _input, rng)
    _accum -= SimDt
    pumps++
SyncArenaViews(Engine.BuildRealtimeSnapshot())
```

### 30.8 W1 phases detail

0. DEC signed + claims
1. Types + MotionMode enum + pose fields
2. Arena loader + integrity validation
3. TickRealtime motion-only
4. Tests Walk/Run/Climb
5. Host pump + debug draw
6. Panel: movement buttons or axis; hide End Turn behind Debug
7. Save additive poses

### 30.9 W1 acceptance commands

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/RealtimeLocomotionTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/CombatArenaCatalogTests.cs
```

### 30.10 W1 non-goals
No shooting cadence yet beyond optional stub; no AI special behaviors yet; no flee corridor yet; no shelter physics edits.

---

## 31. Deep package — W2 Aim & shoot

### 31.1 Existing behavior
`PlayerFire(targetId, rng)` resolves one shot through ballistics when phase is PlayerTurn. Suppress pins. Jam/repair exist. Reload exists in Core (`PlayerReload`) without host `ActionReload`. Panel Fire uses selected target; jam path hardcodes yuki.

### 31.2 Requested behavior
Hold aim, tap/hold fire, bullets emit on cadence, moving accuracy suffers, braced accuracy improves, reload and clear-jam are live channel actions, getting shot is orthogonal (enemies fire in W3).

### 31.3 Delta
- `CombatInputFrame.FireHeld`, `AimRad` / `AimTargetId`, `Brace`.
- Weapon ROF → `FireCooldown` refill.
- Internal call path: TickRealtime → try fire → build context from poses → `BallisticsSystem.Resolve` → apply damage.
- Host `ActionReload`.
- Remove yuki hardcode (shared hygiene with all waves).

### 31.4 Ballistics context from poses

| Field | Source |
|---|---|
| Shooter accuracy | stance/brace + needs performance + weapon condition |
| Range | distance(shooter pose, target pose) |
| Cover | target in cover volume → cover rating |
| BarrierMaterial | first barrier segment intersecting LOS (fix null!) |
| Lane match | derived lanes equal → legacy bonus |

### 31.5 Cadence algorithm

```
if FireHeld and FireCooldown<=0 and not Climbing and weapon ready:
    resolve one shot
    FireCooldown = 60 / max(rpm, 1)
else:
    FireCooldown = max(0, FireCooldown - dt)
```

Semi-auto weapons: require rising edge of FireHeld (host tracks edge) OR Core stores `FirePressedEdge`.

### 31.6 UI mapping (survivor fantasy)

| Fantasy | Control |
|---|---|
| Aim | right stick / mouse aim; or soft-lock next hostile |
| Shoot | face button / LMB |
| ADS / brace | trigger / RMB when Walk or Idle |
| Reload | button; calls ActionReload(subject) |
| Clear jam | existing button with DefaultPlayerSubjectId |

### 31.7 W2 acceptance

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/RealtimeFireCadenceTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/RealtimeReloadHostTests.cs
```

Prove: identical seed + dt sequence → identical ammo remaining and hit log.

### 31.8 W2 non-goals
Projectile entities with travel time optional v2; hitscan acceptable for v1 if ballistics already assumes instant resolve. Do not build a separate ammo ledger.

---

## 32. Deep package — W3 Enemy AI live

### 32.1 Existing behavior
Catalog writes `AiSpecialMove` onto combatants. `EndTurn` fires generic shots using only accuracy/damage mods. Behavior matrix docs promise distinct archetypes.

### 32.2 Requested behavior
During ActiveRealtime, each living enemy runs a special-move behavior that changes positioning, firing pattern, or status — so a Charge boar does not play like a Burrow mite.

### 32.3 Behavior contracts (normative sketches)

**Burrow**
- Enter subsurface for `burrow_hide_seconds` (untargetable flag or max cover).
- Reappear at flank spine offset; short accuracy debuff then normal.

**Flank**
- Steer toward outer spine opposite player facing.
- When |x_enemy - x_player| lateral threshold met, gain `flank_accuracy_bonus` for `T` seconds.

**Spore**
- Every `spore_period`, call chem handoff to seed a short-lived hazard at enemy pose (existing `ChemWarfareSystem` API — re-rg exact method at implement time; if none fits, emit event for host and stop — do not invent toxin store).

**Charge**
- Set MotionMode Run toward nearest living player.
- On distance < `melee_radius`, apply charge damage once per cooldown; briefly stun self.

**SuppressiveFire**
- FireHeld virtual true; rpm high; accuracy low; successful hit applies pin turns/time.

**TacticalRetreat**
- When `Health/MaxHealth < FleeThreshold` (and threshold ≠ -1), steer to backline / extract-opposite; reduced fire rate.

### 32.4 Think interval
`AiThinkCooldown` default 0.35s. Movement steering updates every tick; decisions every think.

### 32.5 Ordering
Stable sort enemies by `Id` ordinal before AI tick to keep determinism.

### 32.6 Tests
One fixture per move with locked seed asserting a unique observable (position delta sign, pin flag, untargetable period, chem event, retreat x decrease).

### 32.7 Non-goals
GOAP planner; navmesh; animation root motion authority; new move names.

---

## 33. Deep package — W4 Flee interrupt (shot while running away)

### 33.1 Existing behavior
`PlayerRetreat`:
- Blocked in last stand.
- Success chance = mobility + doctrine bonus.
- Success → all players `HasFled`, phase Retreated, aftermath.
- Failure → damage first living player, message `"Retreat disrupted — someone is hit."`, encounter may continue.

This already encodes the fantasy as a **single check**. User wants it **live**.

### 33.2 Requested behavior
Press Flee → squad sprints toward extract volume while enemies continue shooting / charging. Hits during flee are normal damage events tagged `flee_hit`. Extract requires holding inside volume. Death/down during flee can doom extract.

### 33.3 State machine

```
Idle/other --RequestFlee--> FleeAlign --> FleeRun --> ExtractHold --> Retreated
                     \-> (hit) may Pin or Down --> FleeRun delayed / Lost
```

### 33.4 Rules (proposed DEC sub-clauses)

1. Flee sets MotionMode Flee for all living non-downed players (or only selected subject — recommend squad-wide for management tone).
2. Enemies remain ActiveRealtime hostile.
3. Player hip-fire allowed at ×0.5 accuracy; brace disabled.
4. ExtractHoldSeconds default 1.25s sim.
5. If any player living still outside volume, hold meter pauses (squad extract).
6. Downed during flee: not required inside volume; living members can still extract; downed become casualties in aftermath (prefer) — **must be in DEC**.
7. Last stand blocks RequestFlee (preserve).
8. UI Retreat button calls RequestFlee (not old instant roll). Keep old roll as `DebugInstantRetreat` only if tests need it.

### 33.5 Proving “shot while running away”

Adversarial test:
- Arena short extract path.
- One enemy with accuracy forced high / aimbot test port.
- Player RequestFlee immediately.
- Assert ≥1 event type `flee_hit` OR HP decreased after flee start and before Retreated.
- Second test: enemies all downed → Flee extracts with zero flee_hit.

### 33.6 Acceptance commands

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/RealtimeFleeInterruptTests.cs
```

### 33.7 Player-facing copy (tone)
- Start: “Break contact — keep low, move to the egress.”
- Hit: “Rounds snap past as you run — someone is hit.”
- Success: “You clear the kill zone and break contact.”
Reuse restrained tone; no real military unit names.

---

## 34. DEC draft text (for register)

**ID:** `DEC-RT-COMBAT-REWRITE`
**Title:** Real-Time Tactical Combat Clock under TacticalCombatSystem
**Status:** PROPOSED (unsigned at plan time)
**Decision:**

1. ASHFALL combat encounters use a deterministic fixed-tick realtime simulation owned exclusively by `TacticalCombatSystem`.
2. Godot provides presentation and input only; CharacterBody2D contact is not gameplay authority.
3. Turn phases `PlayerTurn`/`EnemyTurn`/`EndTurn` are retired as the primary live cadence after migration; save compatibility places legacy mid-turn fights into ActiveRealtime at lane anchors.
4. Survivor motion modes include Walk, Run, Climb (segment), Cover, Flee.
5. Enemy `AiSpecialMove` values from `CombatAiMoves` are live behaviors.
6. Flee is a continuous extract under fire; mid-escape hits are required-capable outcomes.
7. Persistence remains the `combat` save section with additive pose fields; no second combat section in this DEC.
8. Downed-on-flee policy: living squad may extract; downed count as combat casualties in aftermath.
9. Unity remains retired.

**Non-decisions deferred:** projectile travel-time ballistics v2; stealth Interactive prep panel; breaching full player loop; ballistic shield UI.

---

## 35. WORKTREE claim template (do not apply until approval + DEC)

```
| claim-pfgl-rt-combat-w1-... | PFGL-RT-W1-COMBAT-CLOCK-LOCOMOTION | <agent> | Core TacticalCombatSystem*; CombatTypes; combat_arenas.json; CombatHostSession pump; CombatArenaView; RealtimeLocomotionTests | ACTIVE |
```

W2–W4 claim overlapping host/UI only after W1 releases shared files or same owner continues the stack serially (recommended: **one owner through W4** because shared hubs collide).

---

## 36. Migration matrix (legacy → realtime)

| Legacy API | Migration |
|---|---|
| `EndTurn` | Debug/single AI burst OR no-op message |
| `PlayerMoveLane` | Snap/steer toward lane spine x |
| `SetStance` | Maps to brace/cover/flee intents where possible |
| `PlayerRetreat` | `RequestFlee` |
| `EvaluateEndTurn` | `Evaluate` pump paused / not applicable |
| Phase PlayerTurn checks | `RealtimeActive && !Resolved` action legality |
| CombatPanel End Turn button | Hidden or Debug |
| Headless demos looping EndTurn | Rewrite to Pump N ticks |
| `Main.UiTests.RealCampaignJourney` combat loop | Update to realtime pump / flee |

---

## 37. Performance bounds

- Target sim 20 Hz; render 15 FPS if Godot session used for verification (project rule default).
- Max 12 combatants v1 for AI cost.
- Arena static geometry; no dynamic nav rebuild.
- Event log ring buffer capped (existing log patterns).

---

## 38. Accessibility & input

- Pause: stop host pump; show paused banner.
- Controller: left stick move, sprint modifier, climb interact, south fire, west reload, east flee confirm, right stick aim.
- Keyboard: WASD, Shift run, Space climb, mouse aim, LMB fire, R reload, V flee.
- Confirm flee to avoid accidental extract.
- Colorblind-safe hit flashes (shape + text, not color alone).

---

## 39. Content utilization notes

Presence of `ai_special_move` in JSON becomes **reachable** only after W3. Plan should update content-utilization expectations / selftest allowlists if they assert combat move reachability.

`camouflage_gear.json` remains prep-loop content (optional stealth follow-on). Realtime combat does not consume camo mid-fight unless already applied to expedition stealth state before ambush — keep that bridge as read-only modifier on detection/start positioning if already present.

---

## 40. Detailed verification matrix

| ID | Claim | Proof |
|---|---|---|
| V1 | Core engine-free | `rg Godot Assets/Ashfall.Core/Combat` empty |
| V2 | One authority | no new `*CombatSystem` type for realtime |
| V3 | Walk works | RealtimeLocomotionTests |
| V4 | Run faster | speed assert |
| V5 | Climb needs segment | negative + positive tests |
| V6 | Fire cadence deterministic | RealtimeFireCadenceTests |
| V7 | Reload hosted | host test |
| V8 | AI Charge closes gap | AI test |
| V9 | AI Suppress pins | AI test |
| V10 | Flee can be hit | RealtimeFleeInterruptTests adversarial |
| V11 | Flee can clean extract | zero-hit fixture |
| V12 | Save poses | persistence test |
| V13 | No yuki hardcode | `rg survivor_yuki src/UI/CombatPanel.cs` empty for actions |
| V14 | Stealth noise retained | fire still calls ApplyWeaponNoise |
| V15 | Pin stays 266 | SaveSectionRegistry count unchanged |

---

## 41. Open questions resolved by this plan

| Question | Resolution |
|---|---|
| Turn-based vs realtime | Realtime rewrite (user lock) |
| FPS open world? | No — encounter arenas |
| Physics in Core or Godot? | Kinematics in Core; Godot mirrors |
| Shot while fleeing | W4 flee corridor under fire |
| DEC needed? | Yes before W1 Core clock |
| Parallel LiveCombatSystem? | Forbidden |
| Instant retreat roll? | Demoted; Flee mode primary |

---

## 42. Suggested serial schedule (after DEC)

Day/block 1–2: W1 Core + tests
Block 3: W1 host pump + arena stub
Block 4–5: W2 fire/aim/reload
Block 6–7: W3 AI behaviors
Block 8: W4 flee interrupt
Block 9: save + migration + yuki purge + acceptance

Single integrator preferred due to hub overlap.

---

## 43. Appendix — evidence quotes (short)

EndTurn enemy path (generic fire only) — `TacticalCombatSystem.Damage.cs` ~87–125.
Retreat fail hit — `TacticalCombatSystem.Actions.cs` ~510–522 message `"Retreat disrupted — someone is hit."`.
AI move enum — `CombatAiMove.cs` Burrow…TacticalRetreat.
Panel yuki — `CombatPanel.cs` ClearJam/Repair/LastStand.
Presentation physics — `SurvivorActorView.cs` summary comment “Movement is presentation only”.
Host MoveLane live — `CombatHostSession.ActionMoveLane`.
Core Reload live — `PlayerReload` in Actions.cs.
Manifest combat Interactive — `player_surface_manifest.json`.

---

## 44. Appendix — rejected alternatives

1. **Keep turns + juice animations** — Rejected by user (true realtime).
2. **Godot-only FPS controller applying damage** — Violates Core authority + determinism.
3. **New LiveCombatSystem beside tactical** — Dual authority; forbidden.
4. **Realtime only for flee, turns otherwise** — User chose full rewrite; hybrid overlay was offered and declined.
5. **Climb as pure UI animation** — Must affect pose/LOS to be a mechanic.

---

## 45. Closing checklist for approver

- [ ] Agree DEC draft §34
- [ ] Agree four package IDs W1–W4
- [ ] Agree single-owner serial integration
- [ ] Agree encounter-arena scope (not open world)
- [ ] Agree flee downed policy
- [ ] Approve copy to `docs/plans/PLAYER_FACING_REALTIME_COMBAT_PHYSICS_AI_INTEGRATION_PLAN.md`
- [ ] Explicitly authorize implementation (planning-only until then)

**Approval phrase suggestion:** “Approve PFGL-RT-COMBAT-TETRAD-2026-09-25 and sign DEC-RT-COMBAT-REWRITE.”
