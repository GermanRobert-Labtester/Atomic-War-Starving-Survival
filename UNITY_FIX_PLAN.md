# Unity Fix Plan — Atomic War

**Project:** `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`
**Editor:** Unity 6.5 (6000.5.5f1) · Linux · License: Unity Student (valid, entitlements OK)
**Generated:** Pass 2 investigation — all findings from `Logs/Editor.log`, source inspection, and manifest audit.
**Status:** PLAN ONLY — nothing modified yet.

---

## Executive Summary

| # | Severity | Issue | Fix |
|---|---|---|---|
| 1 | 🔴 Critical | 2× CS0246 errors break **all** compilation (`SimplePlayerController2D`, `SimplePlayerAnimator` missing) | Delete broken Cinemachine 2D Platformer sample |
| 2 | 🟠 High (data loss) | 3× `Dictionary<…>` + 1× nullable enum fields silently **dropped from saves** (UAC1009/UAC1001) | Replace with serializable structures |
| 3 | 🟠 High (env) | 34× bee_backend IPC failures from flatpak-Hub `TMPDIR` | Launch editor natively (direct binary) |
| 4 | 🟡 Medium | 38 NREs + 20 KeyNotFound on Shader Graph sample imports | Delete unused Shader Graph samples (162 MB) |
| 5 | 🟡 Medium | 580× CS0067 unused events, CS0414, CS0162 | Wire or suppress per catalog below |
| 6 | 🟡 Medium | cloud-diagnostics deprecated package (unused in code) | Remove from manifest |
| 7 | ⚪ Benign | Licensing 505 handshake (self-healed), HDRP asmref, Test Framework asmdefs, gRPC log spam, _FORWARD_PLUS | Document as noise / auto-resolved by Phase 2 |
| 8 | ⚪ Env | 7.1 GiB RAM (1.3 GiB free), zram swap 14.2G active | Lightweight editor usage guidance |

**Target end state:** `Tundra build succeeded`, zero `error CS*`, zero `NullReferenceException`/`KeyNotFoundException` in log, UAC/CS warnings in project code reduced to zero (sample noise eliminated).

---

## Phase 0 — Environment (do first, low risk)

### 0.1 Fix editor launch (bee IPC failures)
The editor is launched via **flatpak Unity Hub**, so `TMPDIR` resolves to `~/.var/app/com.unity.UnityHub/cache/tmp/…` and bee_backend sub-processes intermittently can't connect (`Failed IPC_Client_InitializeAndConnectToParent` ×34).

**Action:** launch the editor binary directly, never through the flatpak Hub:

```bash
TMPDIR=/tmp "/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity" \
  -projectPath "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War"
```

- Use the flatpak Hub **only** for downloading/updating editors, not for launching.
- Verify in the next log: `grep -c "Failed IPC_Client" Logs/Editor.log` → 0.

### 0.2 RAM headroom
7.1 GiB total, zram swap active (14.2G). Unity 6 needs ~8 GB; before opening the editor:
- Close browsers / heavy apps (target ≥ 4 GiB free).
- Avoid running tests + import + Play Mode simultaneously.

### 0.3 Licensing noise
`HandshakeResponse … Unsupported protocol version '1.18.1'` self-healed (editor spawned its own LicensingClient). **No action** unless the editor refuses to open a project — then kill stale `Unity.Licensing.Client` processes and relaunch.

---

## Phase 1 — 🔴 CRITICAL: Restore Compilation

### 1.1 Delete the broken Cinemachine 2D Platformer sample

**Files (in `Assets/Samples/Cinemachine/3.1.7/2D Samples/`):**

```
2D Samples/
├── 2D Platformer.unity(.meta)
├── GameControl.cs(.meta)          ← error CS0246: SimplePlayerController2D
├── Platformer Camera 2D.cs(.meta) ← error CS0246: SimplePlayerAnimator
└── Environment/…
```

These two scripts reference Unity **Standard Assets** scripts (`SimplePlayerController2D`, `SimplePlayerAnimator`) that don't exist in this project. Because they sit in `Assets/Samples` with **no asmdef**, they compile into `Assembly-CSharp` and fail the whole build (last 17 Tundra builds failed, ExitCode 3).

**Decision: DELETE** (recommended):
- Project code (`Assets/_Game`) has **zero references** to Cinemachine.
- Alternative (rejected): importing Unity Standard Assets 2D would add legacy code just to make a sample compile.
- Keep the `com.unity.cinemachine@3.1.7` **package** in the manifest (harmless, may be used later).

```bash
rm -rf "Assets/Samples/Cinemachine/3.1.7/2D Samples" "Assets/Samples/Cinemachine/3.1.7/2D Samples.meta"
```

### 1.2 Verify
Open the editor once (native launch, Phase 0.1), let it import, then:

```bash
grep "Tundra build" Logs/Editor.log | tail -1   # expect: succeeded
grep -c "error CS" Logs/Editor.log               # expect: 0
```

---

## Phase 2 — Sample Cleanup (removes the majority of log noise)

Samples total **282 MB**; `Assets/_Game` references **none** of them. Every NRE/KeyNotFound/CS0618/asmdef-noise entry in the log originates from `Assets/Samples/`.

| Sample folder | Size | Noise it causes | Recommendation |
|---|---|---|---|
| `Shader Graph/17.5.0` | 162 MB | 38 NREs + 20 KeyNotFound + `_FORWARD_PLUS` deprecated spam | **Delete** |
| `Cinemachine/3.1.7` | 796 KB | CS0246 (Phase 1) + HDRP asmref warnings | **Delete `2D Samples`** (rest harmless) |
| `Test Framework/1.7.0` | 1.4 MB | 69 "asmdef has no scripts" warnings | **Delete** (keep the package itself) |
| `Splines/2.9.0`, `2D SpriteShape/15.0.3` extras | small | CS0618 obsolete API ×23 | **Delete** if unused |
| `2D Pixel Perfect`, `2D Common`, `2D PSD Importer`, `2D Tooling`, `2D Tilemap Extras`, `Input System`, `Timeline`, `Searcher`, `Settings Manager`, `Scriptable Render Pipeline Core`, `Universal Render Pipeline`, `AI Navigation` | ~110 MB | none/baseline | **User decision** — ask which demos the art pipeline needs; default keep `2D Animation` (art reference) |

**Execute:**

```bash
cd "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War"
rm -rf "Assets/Samples/Shader Graph" "Assets/Samples/Shader Graph.meta"
rm -rf "Assets/Samples/Test Framework" "Assets/Samples/Test Framework.meta"
# plus any folders approved by the user from the table above
```

**After cleanup, log should show:** no NRE, no KeyNotFound, no CS0618, no "asmdef has no scripts", no `_FORWARD_PLUS`.

---

## Phase 3 — Project Code Fixes (the real work)

### 3.1 🟠 UAC1009 — Dictionaries silently dropped from saves (DATA LOSS)

**Confirmed by source inspection:** these classes are `[Serializable]` and saved via JsonUtility, which **ignores** `Dictionary` fields. Data in them is lost on save **today**.

#### 3.1.1 `FactionIntelligenceSaveState.TributeDemands` — `Dictionary<string, float>`
`Assets/_Game/Factions/FactionIntelligenceSystem.cs:42`
Consumers: `FactionIntelligenceSystem.cs:123` (write), `GameBootstrap.ExpansionSaveables.cs:85-87` (merge).

```csharp
[Serializable] public class TributeDemandEntry { public string FactionId; public string ResourceType; public float Amount; }
[Serializable] public class FactionIntelligenceSaveState
{
    public List<IntelEntry> ActiveIntel = new List<IntelEntry>();
    public List<DoubleAgentState> ActiveAgents = new List<DoubleAgentState>();
    public List<TributeDemandEntry> TributeDemands = new List<TributeDemandEntry>(); // was Dictionary<string,float>
    public List<string> AlliedFactionIds = new List<string>();
    public bool InformantNetworkActive;
}
```
- Write site: `_state.TributeDemands.Add(new TributeDemandEntry { FactionId = factionId, ResourceType = resourceType, Amount = amount });` (key format was `$"{factionId}_{resourceType}"`).
- Merge site: rebuild by `FactionId + "_" + ResourceType` key.
- **Save migration:** old saves lack the field → empty list loads clean. New saves won't be readable by old builds (acceptable, single-dev project).

#### 3.1.2 `NarrativeArcSaveState.ArcMilestones` / `ArcBranches`
`Assets/_Game/Narrative/SurvivorNarrativeArcSystem.cs:24-25` — `Dictionary<string,int>` / `Dictionary<string,string>`

```csharp
[Serializable] public class ArcMilestoneEntry { public string SurvivorId; public int Milestone; }
[Serializable] public class ArcBranchEntry     { public string SurvivorId; public string Branch; }
// in NarrativeArcSaveState:
public List<ArcMilestoneEntry> ArcMilestones = new List<ArcMilestoneEntry>();
public List<ArcBranchEntry>     ArcBranches    = new List<ArcBranchEntry>();
```
- Update all read/write sites in `SurvivorNarrativeArcSystem.cs` accordingly.

#### 3.1.3 `PolypharmSave.ValuesJagged` — `float[][]`
`Assets/_Game/Simulation/SimulationSystems.Medical.cs:152`
Already documented as "in-memory/legacy only — JsonUtility drops float[][]". The flat arrays (`Keys`/`Counts`/`ValuesFlat`) are the persisted path. **Fix = make intent explicit:**

```csharp
[NonSerialized] public float[][] ValuesJagged; // legacy in-memory; intentionally never saved
```
`[NonSerialized]` clears the UAC1009 warning and prevents accidental future "fixes" that break save compat.

### 3.2 🟠 UAC1001 — `Survivor.ActiveChronicIllness` nullable enum not saved (DATA LOSS)
`Assets/_Game/Survivors/Survivor.cs:484` — `public ChronicIllnessKind? ActiveChronicIllness;`
`ChronicIllnessKind` enum itself **is** `[Serializable]` (`ChronicIllnessKind.cs:7`), but `Nullable<T>` is not — the field is **skipped on save** today (24 references in code, e.g. lines 497, 510, 521 use `.HasValue`).

**Fix: split nullable into enum + flag**

```csharp
public bool HasChronicIllness;                    // new — persisted
public ChronicIllnessKind ActiveChronicIllness;   // was ChronicIllnessKind?
```
Update the ~4 usage sites:
- `IsChronicIllnessManaged` / `FatigueDrainMultiplier`: `!ActiveChronicIllness.HasValue` → `!HasChronicIllness`
- `ActiveChronicIllness.Value == X` → `ActiveChronicIllness == X`
- Property setter(s) assigning `null` → set `HasChronicIllness = false`.

### 3.3 CS0162 — Unreachable code (const comparison)
`Assets/_Game/World/Location_SaltFlatsConvoy.cs:73-75`
`SafeTravelStartHour = 22f` and `SafeTravelEndHour = 4f` are **`const`** → `if (22f > 4f)` is always true → line 75 unreachable.

```csharp
public bool IsSafeTravelHour(float currentHour)
{
    // 22:00–04:00 wrap-around (constants: SafeTravelStartHour=22, SafeTravelEndHour=4)
    return currentHour >= SafeTravelStartHour || currentHour < SafeTravelEndHour;
}
```

### 3.4 CS0414 — `_autoDismissSeconds` never read
`Assets/_Game/UI/FalloutStormWarningBanner.cs:19` — assigned 12f, never consumed.
**Fix:** wire it into the auto-dismiss coroutine (`yield return new WaitForSeconds(_autoDismissSeconds);`) — intended designer-facing feature. If auto-dismiss is currently hardcoded elsewhere, replace that literal with the field.

### 3.5 CS0067 — 580 "event never used"
Affected (top): `NPC_*.cs` faction classes (`AshWidows`, `BurnedPatrol`, `FeralChildren`, `Osteophages`, `SunSeekers`, `SurgeonsCaravan`, `TheCollector`, `TheTollman`), `FactionIntelligenceSystem`, `DoctrineSystem`, `PeaceTreatySystem`, `DebtAndFavorEconomy`, `Quest_TheIronWorm`, and ~15 HUD classes.

These are **intended public API hooks** (events raised for future listeners). Two acceptable resolutions:

1. **Wire them** where a listener exists in design docs (encounter/UI systems) — preferred for gameplay-critical ones (`OnEncounterStarted`, `OnEncounterResolved`).
2. **Suppress per file** for pure API-surface events — add at top of each file:

```csharp
#pragma warning disable CS0067 // event is public API surface; subscribers arrive with feature wiring
```
Catalog every file first:
```bash
grep -rl "public event" Assets/_Game | while read f; do echo "== $f"; grep -n "public event" "$f"; done
```
> Decision checkpoint with user: which events are pending-wiring (keep warning until wired) vs pure API (suppress).

---

## Phase 4 — Package & Dependencies

| Package | Action | Reason |
|---|---|---|
| `com.unity.services.cloud-diagnostics@1.0.12` | **Remove** from `Packages/manifest.json` | Deprecated by Unity; **zero usage** in `Assets/`. Use 6.2+ Diagnostics if ever needed. |
| `com.unity.cinemachine@3.1.7` | Keep package, remove samples only | Package harmless; HDRP asmref warning lives in the package (immutable) — ignore. |
| `com.unity.sentis@2.2.0` built-in | Do **not** add to manifest | It's an 8 KB stub ("Sentis is now Inference Engine") — adding it would be broken. |
| Inference Engine (`com.unity.inferenceengine`) | Optional Pass 3: try "Add package by name" in Package Manager | Registry resolution only verifiable inside the editor; not bundled. |
| Unity Muse / Unity AI Assistant | Not possible | Zero Muse entitlements on this Student license; not in this editor build (verified Pass 1.5). |

After removing cloud-diagnostics: re-open editor → Package Manager regenerates `packages-lock.json` → commit both.

---

## Phase 5 — Validation Loop (run after each phase)

```bash
cd "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War"

# 1. Native editor batch compile check (no GUI):
TMPDIR=/tmp "/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity" \
  -batchmode -nographics -quit -projectPath "$PWD"

# 2. Assert clean log:
grep "Tundra build" Logs/Editor.log | tail -1            # must say "succeeded"
echo "errors:   $(grep -c 'error CS'   Logs/Editor.log)"
echo "nre:      $(grep -c 'NullReferenceException' Logs/Editor.log)"
echo "keynf:    $(grep -c 'KeyNotFoundException'  Logs/Editor.log)"
echo "ipc:      $(grep -c 'Failed IPC_Client'     Logs/Editor.log)"
echo "warnings: $(grep -c 'warning UAC\|warning CS' Logs/Editor.log)"   # target: 0 for UAC*, CS* only from approved suppressions

# 3. Run the existing test suite (project has playmode/editmode harness):
#    via editor Test Runner, or the project's existing CLI (see test-results-playmode-latest.xml workflow)

# 4. Git checkpoint per phase:
git add -A && git status --short   # review before committing
```

**Gate per phase:** compile success + log assertions before moving on. Commit after Phases 1, 2, 3, 4 separately (each is independently revertible).

---

## Risk Register

| Risk | Mitigation |
|---|---|
| Save-format changes (3.1, 3.2) break existing saves | Old saves load fine (missing fields default); test with a copy of a real save; JSON diff before/after |
| Sample deletion removes art pipeline references | Confirmed zero `Assets/Samples` refs in `Assets/_Game`; keep `2D Animation` samples by default; user approval gate in Phase 2 |
| `[NonSerialized]` on `ValuesJagged` changes runtime in-memory merge behavior | None — attribute only affects serializer; runtime code untouched |
| Native editor launch bypasses Hub licensing | License client auto-launches (already observed working); entitlements cached and valid to 2027 |
| Suppressing CS0067 hides a real missing wiring | Review each file in the catalog; wire gameplay-critical events first (Phase 3.5 gate) |

---

## Suggested Execution Order

1. **Phase 0** — environment (launch natively, free RAM) — 5 min
2. **Phase 1** — delete Cinemachine sample, verify compile — 10 min
3. **Phase 2** — sample cleanup per user approval — 15 min
4. **Phase 3.1–3.4** — data-loss fixes + dead code (highest value) — 1–2 h
5. **Phase 3.5** — CS0067 catalog decision + wiring/suppression — 1 h
6. **Phase 4** — remove cloud-diagnostics — 5 min
7. **Phase 5** — full validation: batch compile, tests, clean-log assertions — 30 min

---

# ✅ EXECUTION STATUS (Pass 2 — completed)

## Done
| Item | Result |
|---|---|
| Phase 1 — Cinemachine 2D Platformer sample | Deleted → CS0246 gone |
| Phase 2 — Sample cleanup | 282 MB → 23 MB (kept 2D Animation only); all sample noise gone (NREs, KeyNotFound, CS0618, asmdef warnings) |
| Phase 3 — Data-loss fixes | TributeDemands, ArcMilestones/ArcBranches → List entries; ActiveChronicIllness → enum + HasChronicIllness flag; ValuesJagged [NonSerialized]; CS0162 dead branch removed; CS0414 wired; CS0067 suppressed on 34 files |
| Bonus — Load-path NRE | `_hud` null-guard in `PaintPhase11InitialState` (was added after last green run; fixed 4 EditMode failures) |
| Phase 4 — cloud-diagnostics | Removed from manifest |
| Phase 4 — Inference Engine | `com.unity.ai.inference@2.2.1` installed (API: `Unity.InferenceEngine`; ONNX runtime; 12 importable samples incl. "Run a model", "Quantize a model") |
| Phase 5 — Validation | **Tundra build: success. 0 errors, 0 warnings. 0 NRE, 0 IPC failures.** EditMode 2348/2351. PlayMode 132/132. |
| Git | 2fe73e72 (code fixes) + edfe7cd7 (packages + this plan) |

## Remaining (NOT caused by this work — game-design decisions for the owner)
1. `RegistryDispatchWiringTests.ConstructedSystems_AreRegistered_NoC1Gaps` — ~20 systems constructed but never ticked (new expansion systems). Ratchet baseline must NOT be regenerated blindly: these systems are non-functional until wired into SystemRegistry. Decide per system: wire or baseline.
2. `SaveStateCompletenessTests.RestoreState_IsNeverAPureStub` — MemorialWallUI.RestoreState empty + RationAllocationDial captures-but-never-consumes (pre-existing).
3. `UntickedSystemsBaselineTests.RegenerateBaselineFile` — fails by design; run deliberately after wiring decisions, review the diff.

## Environment notes
- Editor must be launched natively (not via flatpak Hub) to avoid bee IPC failures: `TMPDIR=/tmp ~/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -projectPath ...`
- RAM: 7.1 GiB total. The GUI editor wedged (6.7 GB swapped) during services import; keep heavy apps closed while importing. Batchmode validations ran fine.
