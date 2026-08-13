# Ashfall — Deep Code Audit #2 (post-Bridge)

Date: 2026-08-14 (second pass, after the UnityEngine compatibility bridge landed).
Supersedes the status table in `ASHFALL_DEEP_CODE_AUDIT_2026-08-14.md`.

## Shape

| Area | Size |
|---|---|
| Unity `_Game` | 228,605 LOC / 1,309 files |
| Godot host `src/` | 4,649 LOC / 24 files |
| — of which `src/Bridge/` shim | **1,721 LOC / 6 files** |
| `Ashfall.Core` (`Assets/Ashfall.Core/`) | 4,515 LOC / 24 files |
| Core test suite | 14 files / **109 tests** |
| `Ashfall.dll` | 5.1 MB |

## Verified healthy

| Check | Result |
|---|---|
| `dotnet build Ashfall.csproj` | **0 errors** |
| `dotnet test` | **109/109 pass** |
| `--journal-selftest` | **20/20 PASS** |
| `--ice-road-selftest` | **PASS 21/21** |
| `--bridge-selftest` | **18/18 PASS** (added during H3 remediation) |
| `WeatherKind` definitions | **1** (unification held) |
| `Ashfall.Core` consumption | Real — Godot host + 6 Unity asmdefs + tests |
| Core purity | Compiler-enforced via `noEngineReferences: true` |
| Bridge `JsonUtility` | **Genuinely implemented** (System.Text.Json), not a stub |
| Save DTOs | All plain fields, 0 properties — deliberate and correct |
| "Thin MonoBehaviour" claim | **True** — only 11 `Update()` across 158 MonoBehaviours |

---

## CRITICAL

### C1 — Compiling is not porting. Zero of the 228k LOC executes.

`Assets/_Game/**` now compiles into `Ashfall.dll`. It is never **instantiated**. The Godot host's
entire reference surface is:

```
using Ashfall.Core;      using AtomicWar.Journal;    using Godot;
using System.*;
```

There is no `new GameBootstrap(...)`, no reference to `AtomicWar._Game.*` anywhere in `src/`
outside the bridge itself. 228,605 lines are linked into a 5.1 MB assembly and **not one line runs**.

This is the most dangerous finding, because a green build on the whole codebase *looks* like a
finished port. It is a **type-check milestone**: it proves `_Game` compiles against the shim's
API surface, not that it behaves. Every finding below is currently latent for exactly this reason —
they become live bugs the moment anything calls into `_Game`.

**Recommendation:** state the Godot host's true scope in `GODOT_MIGRATION_STATUS.md` as
"Journal + IceRoad/Holdfast core slice", not "228k LOC ported". Then wire one real system end to
end and let its failures surface the bridge gaps honestly.

### C2 — Cross-host saves hard-fail on checksum (invariant violated) — **fixed**

`AGENTS.md`: *"A save written by one host MUST load in the other."* It cannot.

`SaveSystem.IO.cs` SHA256s the **serialized JSON text**, then re-serializes on load to verify:

```csharp
// save                                    // load  (VerifyChecksum)
string body = JsonUtility.ToJson(snap,true);   data.Checksum = "";
string checksum = ComputeChecksum(body);       string body = JsonUtility.ToJson(data, true);
                                               computed = ComputeChecksum(body);
                                               return computed == saved;   // byte-exact
```

Because the hash covers formatting, any serializer difference breaks it. Verified empirically —
the bridge's `ToJson` is `JsonSerializer.Serialize(..., WriteIndented: true, IncludeFields: true)`:

| | Unity `JsonUtility` | Bridge (System.Text.Json) |
|---|---|---|
| Indent | 4 spaces | **2 spaces** (verified) |
| null string | `""` | **`null`** (verified) |
| Properties | never emitted | emitted (moot here — DTOs are all fields) |

Different bytes → different SHA256 → `VerifyChecksum` fails → `return (false, null, null)`.
The save is **hard-rejected as corrupt**, not warned. Round-tripping within one host is fine; the
break is strictly cross-host.

**Fixed.** `Assets/Ashfall.Core/SaveChecksum.cs` hashes the *state*, not the *text*: a reflection
walk over public instance fields in ordinal name order, values written in a self-delimiting
invariant-culture form. Nothing in the result depends on indent width, key order, or which
serializer produced the object.

Two normalizations do the actual cross-host work, because the *in-memory* objects differ after
parsing the same file — JsonUtility yields `""` and `[]` where System.Text.Json yields `null` and
`null`:

- a null string hashes identically to `""`
- a null collection hashes identically to an empty one

Call-site changes in `SaveSystem.IO.cs`:

- Save: `snapshot.Checksum = SaveChecksum.Compute(snapshot)` then serialize once. The old
  serialize-then-string-splice of `"Checksum": ""` is gone, along with its dependency on
  `JsonUtility` emitting that exact placeholder text.
- Load: verify state-based first, then fall back to `VerifyLegacyTextChecksum` so pre-existing
  saves still open. A legacy save still only re-verifies on the host that wrote it — inherent to
  the old scheme — but nothing that used to load stops loading, and the next Save rewrites the slot
  with a portable checksum. The legacy path now restores `data.Checksum` in a `finally`; the old
  code blanked the caller's field and would have left it blanked if serialization threw.

Gate: **18 xunit tests** in `Ashfall.Core.Tests/SaveChecksumTests.cs`, including an explicit
`CrossHostRoundTripAgrees` modelling both hosts' parse results, culture independence, field-swap
detection, delimiter forgery, and a reference-cycle guard.

**Caveat:** these prove the *algorithm*. The full `SaveSystem` round trip is still unexercised for
the same reason as everything else in `_Game` — see C1, nothing there is instantiated.

### C3 — Eight duplicated types now compile into one assembly

| Type | Copies | Drift |
|---|---|---|
| `IceRoadSystem` | `_Game/Core` + `Ashfall.Core` | **522 lines** |
| `JournalSystem` | `_Game/Events` + `src/Journal` | **170 lines** |
| `KnowledgeBase` | `_Game/Events` + `src/Journal` | 47 lines |
| `JournalSave`, `JournalEntry`, `JournalVoice`, `JournalCodex` | `_Game` + `src/Journal` | — |
| `HoldfastLocationEntry` | `Ashfall.Core` + `_Game/Data` | — |

These no longer sit in separate assemblies — they are all in `Ashfall.dll`, distinguished only by
namespace. Same-namespace resolution silently picks the local copy over the `using`-imported one,
so **the duplication cannot produce a compile error**. It will only ever show up as behavioural
divergence. `IceRoadSystem` was deleted during the previous remediation and restored; it has since
drifted 522 lines.

---

## HIGH

### H1 — `MonoBehaviour` has no lifecycle pump; coroutines never execute

The bridge's `MonoBehaviour` is `enabled` plus two no-ops. There is **no** `Awake`, `Start`,
`Update`, `FixedUpdate`, `LateUpdate`, `OnEnable` or `OnDestroy` dispatch.

```csharp
public Coroutine StartCoroutine(IEnumerator routine) => new Coroutine(routine); // never runs it
public void StopCoroutine(Coroutine routine) { }                                // no-op
public void StopAllCoroutines() { }                                             // no-op
```

`StartCoroutine` constructs an object and **discards the iterator without executing it**.

Blast radius in `_Game` — smaller than 158 MonoBehaviours suggests, because the thin-host
architecture is genuinely followed, but non-zero:

| Hook | Count that would never fire |
|---|---|
| `OnEnable` | 30 |
| `Awake` | 11 |
| `Update` | 11 |
| `OnDestroy` | 8 |
| `Start` | 1 |
| `StartCoroutine` call sites | 3 |

### H2 — Bridge RNG is unseeded and cannot be seeded

```csharp
private static readonly System.Random _rng = new System.Random();  // clock-seeded
```

`Random.InitState` — Unity's seeding entry point — **does not exist in the bridge**. So the
determinism invariant (*"same seed => same simulation in both engines"*) is unsatisfiable for the
4 `UnityEngine.Random` call sites in `_Game`.

Mitigating: `Ashfall.Core` has a proper `ISeededRng` / `SeededRng`, and the core systems use it.
The exposure is limited to those 4 sites — but they are silently non-reproducible.

### H3 — 111 of 820 bridge members are hollow — **partly fixed**

79 empty method bodies (`) { }`) and 32 `=> default / null / 0 / false` expression bodies out of
820 public members (**~13.5%**). These satisfy the compiler and do nothing at runtime. Because the
call sites are in 228k LOC that currently never executes (C1), none of them are caught by any test.

This is the bridge's structural hazard: a shim that returns plausible defaults converts *compile
errors* (loud, cheap) into *behavioural bugs* (silent, expensive).

**Fixed for the runtime-critical files.** `src/Bridge/BridgeGap.cs` now defines an explicit failure
policy, and every hollow member in `UnityEngineCore.cs` and `UnityEngineSceneManagement.cs` has been
classified into one of three buckets:

| Bucket | Behaviour | Applied to |
|---|---|---|
| **Semantic** — silence makes game logic wrong | `throw NotImplementedException` with the consequence spelled out | 12 members |
| **Cosmetic** — audio/visual only, absent by design in a headless host | log once per member, then continue | 8 members |
| **Correctly inert** — doing nothing *is* the right headless behaviour | left as-is, with a comment saying why | `Input.GetKey*`, `Application.isEditor`, `Destroy`, `DontDestroyOnLoad`, `GetActiveScene` |

A binary throw/silent split was rejected: crashing the headless host because a sound effect played
would make it useless for the simulation work it exists to run. Conversely `BridgeGap.Cosmetic` is
deliberately *not* available for anything that feeds data back to game logic.

Notable conversions and why they matter:

- `Object.Instantiate<T>` returned **the original instance**, so every write to the "clone" aliased
  through to the source object. 2 call sites.
- `MonoBehaviour.StartCoroutine` constructed a handle and dropped the iterator un-executed (H1).
- `SceneManager.LoadScene` did nothing while the caller proceeded as if the scene had changed — the
  live example is `GameBootstrap.Hud.cs:261`, a return-to-main-menu that silently wouldn't.
- `PlayerPrefs.Save` reported success over a process-lifetime `Dictionary`.
- `Texture2D.EncodeToPNG` returned an empty array that would be written to disk as a 0-byte `.png`.

`BridgeGap.ThrowOnSemanticGap = false` switches throwing off so a newly wired-up system can be swept
in one run and every gap it hits collected via `BridgeGap.Reported`, rather than fixed one crash at
a time. That is the intended tool for audit priority 5.

Gate: **`--bridge-selftest`, 18/18 PASS** — asserts semantic members throw, cosmetic members stay
quiet, inert members keep answering, and cosmetic reporting is once-per-member not once-per-call.
This exists so a later "cleanup" cannot quietly restore a plausible default.

**Still unclassified** (deliberately deferred, none of it on a runtime path):
`UnityEditorBridge.cs` (25 members — editor-only tooling that cannot execute under Godot),
`UnityEngineGUI.cs` (25 — legacy IMGUI, never pumped), `UnityEngineUIElements.cs` (14).

---

## MEDIUM

| # | Finding |
|---|---|
| M1 | `Time.deltaTime` is **hardcoded `0.016666f`** — a constant, not real frame time (9 uses in `_Game`). Any accumulator advances at a fictional fixed rate. |
| M2 | `Time.timeScale` is a settable property **nothing reads** (2 uses). Pausing/slow-mo silently does nothing. |
| M3 | **304 CS0649** warnings — JSON DTO fields never assigned in code. Correct today (the deserializer populates them), but this is precisely the signature a catalog-loading regression would hide behind. Worth one guard test asserting a non-empty catalog load. |
| M4 | `<NoWarn>` in `Ashfall.csproj` masks CS8618/CS8602/CS8603/CS8604 and 8 more. Nullability problems are suppressed, not resolved; 171 warnings remain even with the mask on. |
| M5 | `Camera.main => null` unconditionally. Any `Camera.main.X` is a guaranteed NRE the moment it runs. |

---

## Priorities

1. **Reframe the milestone** (C1). Correct the migration doc so "compiles" is not read as "ported".
2. ~~**Fix the save checksum** (C2)~~ — **done**; see C2.
3. ~~**Make bridge gaps loud** (H3)~~ — **done for the runtime files**; see H3. The remaining work
   is `MonoBehaviour` lifecycle (H1) and `Random.InitState` (H2), which now fail loudly rather than
   silently but are still unimplemented.
4. **Resolve the 8 duplicate types** (C3), `IceRoadSystem` first — it is the worst-drifted at 522
   lines and has already survived one deletion.
5. Then wire one real `_Game` system end to end under Godot. That is the only thing that converts
   this from a type-check into a port.

## Standing rule (reaffirmed)

> A green build across `_Game` proves the shim's **surface** is complete, not its **behaviour**.
> Until a system is instantiated and exercised, treat its port status as unproven.
