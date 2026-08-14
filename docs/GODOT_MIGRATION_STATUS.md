# Godot Migration Status

**Direction:** Unity 6 → Godot 4.7 (.NET/C#). Unity stays usable and supported throughout.
**Strategy:** Strangler — shrink the Unity-coupled surface by moving logic into engine-agnostic
plain C#, then add a thin Godot host. No big-bang rewrite.

Baseline measured 2026-08-14. Re-measure with the commands at the bottom; do not hand-edit numbers.

---

## Headline

| Metric | Value |
|---|---|
| Unity gameplay code (`Assets/_Game`) | 231,683 LOC / 1,307 `.cs` files |
| Godot host code (`src/`, `scripts/`) | 5,628 LOC / 27 `.cs` files |
| Godot share of total C# | **~2.4%** |
| Unity files that are already engine-agnostic | **244 / 1307 (18.7%, strict)** |
| Subsystems with a Godot host | **1 of 24** (Journal) |
| Subsystems consuming `Ashfall.Core` | **0 — Core is orphaned** |

> The per-subsystem table below uses a `using`-line scan, which reports 19.5% and is **optimistic**:
> 11 files hide fully-qualified `UnityEngine.` references with no `using` (e.g.
> `Events/JournalSystem.cs` calls `UnityEngine.Mathf.Clamp` inline). The strict count above also
> catches `MonoBehaviour` / `ScriptableObject` / `[SerializeField]`. See
> `ASHFALL_DEEP_CODE_AUDIT_2026-08-14.md`.

**Read this honestly:** the port is a beachhead, not a migration in progress. One subsystem
(Journal) runs under Godot. The other 23 are Unity-only. The 19.5% agnostic figure is the real
asset here — that code needs no porting at all, only a host.

## Verified working

- `dotnet build Ashfall.csproj` → **0 errors**, 56 nullability warnings.
- `godot --headless --path . --quit-after 2` → boots, prints the Ashfall init banner.
- `godot --headless --path . -- --holdfast-save-selftest` → S1 save write → reload →
  restore → checksum/tamper checks, all PASS.
- `godot --headless --path . -- --brine-selftest` → S2 BrineWaterHeadlessDemo
  (dormant-gate, daily load, outfall shift, 48h steam-trip clock, resin repair,
  haul loss, state roundtrip) → 21/21 PASS.
- `godot --headless --path . -- --cluster-selftest` → S3 Cluster12CHeadlessDemo
  (12-C dormant → refuse-levy activation, Second List gate, v3 envelope with
  quest snapshot roundtrip) → 19/19 PASS.
- `godot --headless --path . -- --endings-selftest` → S4 EndingsHeadlessDemo
  (five master-list endings arm, second overwrites the first, unknown ids refused,
  ending survives the v3 roundtrip) → 11/11 PASS.
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
