# Godot Migration Status

**Direction:** Unity 6 → Godot 4.7 (.NET/C#). Unity stays usable and supported throughout.
**Strategy:** Strangler — shrink the Unity-coupled surface by moving logic into engine-agnostic
plain C#, then add a thin Godot host. No big-bang rewrite.

Baseline measured 2026-08-14; headline updated 2026-08-14 (closing pass, Loop 7).
Re-measure with the commands at the bottom; do not hand-edit numbers.

---

## Headline

| Metric | Value |
|---|---|
| Unity gameplay code (`Assets/_Game`) | 232,602 LOC / 1,337 `.cs` files |
| Godot host code (`src/`) | 10,151 LOC / 47 `.cs` files |
| Engine-agnostic core (`Assets/Ashfall.Core`) | 16,065 LOC / 75 `.cs` files (shared single source of truth) |
| Godot host share of total C# | **~4.0%** (host only; +6.2% engine-agnostic core both engines share) |
| Host files consuming `Ashfall.Core` | **36** (was 0 — Core is no longer orphaned) |
| Subsystems with a Godot host | **12** — Journal, Duty Roster, Standing Record, Crossing/Arbitration, Holdfast (ice road, census, brine, cluster, endings), Year of Ash, Muster, Dose, Phantom Memory |

> The per-subsystem table below uses a `using`-line scan, which reports 19.5% and is **optimistic**:
> 11 files hide fully-qualified `UnityEngine.` references with no `using` (e.g.
> `Events/JournalSystem.cs` calls `UnityEngine.Mathf.Clamp` inline). The strict count above also
> catches `MonoBehaviour` / `ScriptableObject` / `[SerializeField]`. See
> `ASHFALL_DEEP_CODE_AUDIT_2026-08-14.md`.

**Read this honestly:** the port is a beachhead with a working loop, not a full migration. The
Unity-coupled surface still dominates presentation, but the simulation surface is now shared:
every expansion since the baseline runs its logic from `Ashfall.Core` under a thin Godot host,
with headless verification for all of it. The 18-selftest Godot battery is the regression gate.

## Verified working

- Cold rebuild (`rm -rf .godot/mono/temp Ashfall.Core/bin Ashfall.Core/obj` + `dotnet build
  Ashfall.csproj`) → **0 errors, 0 warnings**; `dotnet test` → **408 passed / 0 failed**.
- `godot --headless --path . -- --data-integrity-selftest` → catalog cross-reference gate,
  **0 errors** across 59 JSON catalogs.
- `godot --headless --path . -- --expansions-selftest` → **236/236 GREEN**.
- 18-selftest battery (bridge, duty-roster, standing-record, crossing, arbitration,
  ledger-debt, greenhouse, ice-road, census, brine, cluster, endings, holdfast, caravan,
  journal, dose-ledger, muster + data gate) — all PASS.
- UI smokes: `--muster-uitest` (roster/approaches/coalition camp/witnesses/epilogue matrix)
  and `--dose-uitest` (4-tab Dose Register surface) — PASS, deterministic across runs.
- Parity audit (2026-08-14): Unity's JsonUtility binds snake_case JSON to snake_case DTOs;
  Godot's SystemTextJsonSerializer is case-insensitive only. Three live divergences found
  and fixed — YOA loader DTOs rewritten to the real file schema, radio terminal was
  rendering blank broadcasts (dead fields + a wrong-typed `signalStrength` that zeroed
  all 37 entries), dose_registers snake_case fields unbound. Binding-assertion regression
  gates added across every loaded catalog. All loaders now match their files.
- Utility AI port (2026-08-15): NPC decision core in Ashfall.Core/UtilityAI
  (UtilityActionDef + ResponseCurve, AIActionContext, UtilityActionScorer
  with trait veto matrix, UtilityAiSystem with deterministic ISeededRng
  noise); fixed latent Unity defect A9 (vetoed actions can no longer win
  selection); utility_actions.json (4 crossing companion actions, Unity
  parity numbers); host session + panel + menu + uitest; debug loop 01-05
  (3 defects fixed: unsorted curves, tuning loader IncludeFields, null
  bundle lists); audit docs/audits/utility_ai_AUDIT.md; 29 unit tests + 13
  probes + 7-check selftest + uitest; cross-process hash f58c6a54.
- Economy port (2026-08-14): market/pricing core in Ashfall.Core/Economy
  (GoodsCatalog + validation, MarketSystem with Unity-parity demand clamps,
  deterministic ISeededRng walk, versioned MarketState, whole-unit barter
  with explicit remainder); economy_goods.json (12 goods); host slice wired
  to the main menu + EconomyMarketPanel with AssetRegistry icons; save slot
  has a checksum envelope with legacy bare-save migration; debug-loop
  hardened (2 defects fixed: tamper acceptance, legacy drop); 27 unit tests
  + 14 adversarial probes + 11-check selftest + uitest; cross-process
  determinism hash efb2fbd6.
- Encounters port (2026-08-14): expedition travel/looting/inbound core moved into
  `Ashfall.Core/Expeditions` (tick machine, stances, push-luck, capacity, stamina collapse,
  encounter rolls on every leg, save/load safe) with a thin Godot host session + save store
  + `--expedition-selftest` (10/10) + 17 unit tests. Documented deviations from the Unity
  host: night-scavenge +0.1 loot and bicycle +0.5 inbound are port additions; stamina-0 is
  an immediate fail (Unity drops loot + health instead); `hasFlashlight` stored but not yet
  read; save shape (List<ExpeditionState>) differs from Unity's — a Unity-side adoption
  step is still pending for cross-host save parity of this system.
- Godot reads the shared JSON catalogs from `res://Assets/StreamingAssets/Data` — data is NOT
  forked per engine, which is what makes the incremental migration viable.

## Per-subsystem readiness

`agnostic/total` = files with no `UnityEngine`/`UnityEditor`/`TMPro`/`Unity.*` import. Higher is
closer to portable. Sorted by how cheap the port is.

| Subsystem | Agnostic/Total | % | Godot host |
|---|---|---|---|
| Encounters | 24/56 | 43% | — |
| Medical | 18/42 | 43% | — |
| Economy | 2/5 | 40% | — |
| Narrative | 21/52 | 40% | — |
| Events | 8/22 | 36% | — |
| Utilities | 8/15 | 53% | — |
| Survivors | 26/90 | 29% | — |
| World | 18/61 | 30% | — |
| Inventory | 22/88 | 25% | — |
| Core | 51/258 | 20% | — |
| AI | 16/94 | 17% | — |
| Shelter | 18/124 | 15% | — |
| Environment | 5/41 | 12% | — |
| Factions | 6/74 | 8% | — |
| Endgame | 2/21 | 10% | — |
| Data | 3/41 | 7% | — |
| Radiation | 1/15 | 7% | — |
| Simulation | 1/6 | 17% | — |
| UI | 5/165 | 3% | — |
| Crafting | 0/5 | 0% | — |
| Quests | 0/12 | 0% | — |
| Editor | 0/19 | 0% | Unity-only by nature |
| Settings | 0/1 | 0% | — |
| Journal | — | — | **✅ ported** (`src/Journal/`) |

Notes:
- **Holdfast S1 (Ice & paper)** now persists from the Godot host: `Ashfall.Core.HoldfastSaveCodec`
  (checksummed cross-host envelope) + `src/Host/HoldfastSaveStore.cs` (user:// JSON). The host
  restores on boot, autosaves on state change, and ships a headless selftest gate. Unity side:
  the shape is port-based, so the same file loads in either host once Unity adopts the codec.
- **Holdfast S2 (Salt & steam)**: BrineWaterSystem is hosted in the Godot dev session
  (plant unlock, membrane repair, outfall shift, brine status line). The save envelope is now
  **v2** (adds brineWater) with a v1→v2 migration: a Sprint 1 save validates against the frozen
  v1 shape and upgrades in place, brine starting fresh.
- **Holdfast S3 (Cluster & claim)**: Order 12-C is host-wired (menu button, questline status
  line). The save envelope is now **v3** (adds the HoldfastQuestSystem snapshot) with a chain
  migration v1→v2→v3; HoldfastQuestSystem.RestoreState deep-copies like its siblings.
- **Holdfast S4 (Shelf & endings)**: `Ashfall.Core.HoldfastEndings` is the master id list
  (five endings, mutually exclusive). The Godot host guards SetEnding with IsKnown and shows
  the armed ending in the status line; `--endings-selftest` proves exclusivity + roundtrip.
- **Year of Ash (Days 180-360)**: the save envelope is now **v2**. v1 persisted only timeline,
  encounters and factionWar while `YearOfAshHostSession` ticked six systems, so deep-freeze
  thermal state, radon (scrubber wear + cumulative alpha dose) and questline progress silently
  reset on every reload — a save-scum path out of a radon crisis. v2 adds those three sections
  with a v1→v2 migration validated against the frozen v1 shape, exactly like `HoldfastSave`.
  `YearOfAshDeepFreezeSystem` and `YearOfAshRadonSystem` gained `CaptureState`/`RestoreState`;
  `QuestlineSystem` had `CaptureState` but no `RestoreState`. `--year-of-ash-save-selftest`
  now asserts all six systems (12 → 19 checks).
- **UI (5/165, 3%)** is the hardest wall and the largest single subsystem after Core. Expect this
  to be rewritten against Godot Control nodes rather than ported. Plan for it explicitly.
- **Editor (0/19)** is Unity authoring tooling. It does not need to migrate.
- **Core (51/258)** is the highest-value target: it is the widest dependency and 20% is already clean.

## Recommended order

1. **Utilities → Economy → Events** — small, already >35% agnostic. Proves the port loop cheaply.
2. **Encounters + Medical + Narrative** — best size-to-portability ratio, real gameplay value.
3. **Core** — do it once the loop is proven; everything else depends on it.
4. **UI last** — treat as a Godot-native rebuild, not a port.

## Invariants

- Moving logic into engine-agnostic C# is progress. A Godot-only reimplementation of logic that
  still exists in Unity is a **regression** — it forks the source of truth.
- A save written by one host must load in the other.
- Same seed ⇒ same simulation in both engines (invariant culture, stable collection ordering).
- JSON in `Assets/StreamingAssets/Data` stays the single authority for both engines.

## Re-measure

```bash
# Godot host size
find src scripts -name '*.cs' | xargs wc -l | tail -1

# Unity size
find Assets/_Game -name '*.cs' | xargs wc -l | tail -1

# Portable fraction, per subsystem
for d in Assets/_Game/*/; do n=$(basename "$d"); t=$(find "$d" -name '*.cs' | wc -l);
  [ "$t" -eq 0 ] && continue;
  c=$(grep -LE "^using (UnityEngine|UnityEditor|TMPro|Unity\.)" $(find "$d" -name '*.cs') | wc -l);
  printf "%-14s %3s/%-4s\n" "$n" "$c" "$t"; done
```

## 2026-08-14 late pass — Survival-loop core ports

Additional Unity subsystems migrated this session into `Ashfall.Core` (engine-agnostic,
0 Unity/Godot imports) with Godot host wiring:

- **Inventory** (`Assets/Ashfall.Core/Inventory/`) — full port of `_Game/Inventory`:
  `Inventory.cs` (stack/weight/capacity, all-or-nothing Add, transfer w/ rollback,
  equip/unequip/swap, worn-gear protection, device battery/calibration, consume,
  deep-copy save state), `ItemDefinitions.cs` (ItemDefinition + EquipSlots canonical/
  alias parser), `DeviceState.cs`/`InstrumentDevice.cs`, `ProceduralItemInstance.cs`,
  `ItemCatalog`. Godot host: `src/Host/InventoryHostSession.cs` + checksummed
  `InventorySaveStore` + `src/Inventory/InventoryPanel.cs`; menu wired in `Main.cs`.
- **Survivors needs** (`Assets/Ashfall.Core/Survivors/NeedsSystem.cs`) — port of
  `_Game/Survivors/NeedsSystem` (hunger/thirst/fatigue/warmth/morale/health/hygiene,
  critical thresholds, cold/hunger/thirst health loss, death evaluation with defer
  gate). Host: `src/Host/SurvivorsHostSession.cs` (demo roster) + `SurvivorsSaveStore`.
- **Radiation** (`Assets/Ashfall.Core/Radiation/RadiationSystem.cs`) — port of
  `_Game/Radiation/RadiationSystem` (exposure model zone−gear−shielding, iodine
  resistance windows, anti-rad, acute/chronic thresholds, dosimeters, worn-gear
  degrade, radiotrophic hook). Operates on engine-agnostic `SurvivorRadState`.
- **Shelter shielding** (`Assets/Ashfall.Core/Shelter/MaterialShieldingSystem.cs`) —
  port of `_Game/Shelter/MaterialShieldingSystem` (#127 ceilings: Wood/Dirt/Concrete/
  Lead attenuation; weakest-roof governs bleed) and wired into the survivors host's
  exposure context.
- Shared `MathfCompat` (`Assets/Ashfall.Core/MathfCompat.cs`) replaces
  `UnityEngine.Mathf` everywhere the ports need clamp/max/min/lerp.

Verification: 488/488 core tests (incl. new InventorySystemTests 12, NeedsRadiation
16, MaterialShielding 6), `--inventory-uitest` PASS, `--survivors-uitest` PASS,
expansions 236/236, muster/dose/muster-uitest PASS, build 0 warnings/errors.

Remaining Unity-coupled surfaces (future ports): AI, Crafting/Workbench, Economy/Trade,
Narrative, Events, Endgame, Encounters, Medical afflictions, Factions, Shelter full
(hatch defense, degradation), World map, Settings. The stranglehold continues —
simulation surface now covers the core survival loop (inventory ↔ needs ↔ radiation ↔
shelter shielding) end to end.
