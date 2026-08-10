# Player Input In Scene Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Put `PlayerInputHandler` into the generated gameplay scene so key presses reach the simulation, and add the structural gate that would have caught its absence.

**Architecture:** `PlayerInputHandler` is already written and already tested in EditMode — the tests `AddComponent` it onto a throwaway `GameObject`, which is precisely why nobody noticed it is in no scene. It resolves its dependency with `GetComponent<GameBootstrap>()` in `Awake`, so it must share a GameObject with `GameBootstrap`. `GameplaySceneBuilder` adds it there; the smoke tests assert both its presence and the co-location its `Awake` depends on.

**Tech Stack:** Unity 6000.5.5f1, C#, NUnit + Unity Test Framework, legacy `Input` (`UnityEngine.Input.GetKeyDown`).

## Global Constraints

- Unity editor: `/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity`.
- **Never pass `-quit` alongside `-runTests`.** It kills the editor mid-run and exits 0 with an empty result file — a green light from a suite that never ran.
- Unity runs take 5–8 minutes. Re-read the diff for bugs *before* launching one; a bug found after costs a full window.
- `Gameplay.unity` is generated. Never hand-edit it — change `GameplaySceneBuilder` and re-run `Tools/ASHFALL/Build Gameplay Scene`. CI's `regenerate` job fails on drift.
- Delete run artifacts (`em.xml`, `em.log`, `pm.xml`, `pm.log`) before committing.

## Why this is not testable by simulating a keypress

`PlayerInputHandler.Update` reads `UnityEngine.Input.GetKeyDown` directly. The legacy input module has no public injection point, so a test cannot press a key without either the new Input System's `InputTestFixture` (this project does not use `InputSystem` for gameplay) or refactoring `Update` behind an injectable input source.

That refactor is not in scope here and would not have caught this bug anyway. The defect is *structural*: the component is absent from the scene. So the test asserts the structure — the component exists, and it sits on the GameObject its `Awake` requires. The key-dispatch logic itself is already covered by the EditMode suite.

---

## File Structure

| File | Responsibility |
| --- | --- |
| `Assets/_Game/Editor/GameplaySceneBuilder.cs` | Add `PlayerInputHandler` to the `GameBootstrap` GameObject. |
| `Assets/Scenes/Gameplay.unity` | Regenerated output. Not hand-edited. |
| `Assets/Tests/PlayMode/GameplaySceneSmokeTests.cs` | Assert the handler is present and co-located. |

---

### Task 1: Put the input handler in the scene

**Files:**
- Modify: `Assets/_Game/Editor/GameplaySceneBuilder.cs`
- Modify: `Assets/Tests/PlayMode/GameplaySceneSmokeTests.cs`
- Regenerate: `Assets/Scenes/Gameplay.unity`

**Interfaces:**
- Consumes: `AtomicWar._Game.Core.PlayerInputHandler` (existing `MonoBehaviour`, no serialized object references — only `KeyCode` fields, so `AssertWired` has nothing to check on it).
- Produces: nothing consumed downstream.

- [ ] **Step 1: Write the failing test**

Add to `Assets/Tests/PlayMode/GameplaySceneSmokeTests.cs`:

```csharp
        /// <summary>
        /// PlayerInputHandler was fully implemented, covered by EditMode tests that
        /// AddComponent it onto a throwaway GameObject, and present in no scene at
        /// all -- so no key the player pressed reached the simulation. Its Awake
        /// does GetComponent&lt;GameBootstrap&gt;(), so being in the scene is not
        /// enough: it has to be on the bootstrap's own GameObject.
        /// </summary>
        [UnityTest]
        public IEnumerator Input_IsWiredToTheBootstrapItDrives()
        {
            var bootstrap = Bootstrap();

            var input = Object.FindAnyObjectByType<PlayerInputHandler>();
            Assert.IsNotNull(input, "Gameplay scene must contain a PlayerInputHandler");

            Assert.AreSame(bootstrap.gameObject, input.gameObject,
                "PlayerInputHandler resolves its bootstrap with GetComponent, so it " +
                "must share the GameBootstrap GameObject");

            yield return null;
        }
```

`PlayerInputHandler` lives in `AtomicWar._Game.Core`, which the fixture already imports — no new `using` needed.

- [ ] **Step 2: Run it and confirm it fails**

```bash
cd "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War"
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics \
  -projectPath . -runTests -testPlatform PlayMode \
  -testResults "$(pwd)/pm.xml" -logFile "$(pwd)/pm.log"
```

Expected: `Input_IsWiredToTheBootstrapItDrives` FAILS on "Gameplay scene must contain a PlayerInputHandler". The other 5 smoke tests still pass.

- [ ] **Step 3: Add the component in the builder**

In `GameplaySceneBuilder.BuildGameplayScene`, the bootstrap line becomes:

```csharp
            var bootstrap = new GameObject("GameBootstrap").AddComponent<GameBootstrap>();

            // Same GameObject, not a child: PlayerInputHandler.Awake resolves its
            // bootstrap with GetComponent, and it silently no-ops when that returns
            // null (Update early-returns on a null bootstrap).
            bootstrap.gameObject.AddComponent<PlayerInputHandler>();
```

- [ ] **Step 4: Regenerate the scene**

```bash
cd "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War"
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics -quit \
  -projectPath . -executeMethod AtomicWar._Game.Editor.GameplaySceneBuilder.BuildGameplayScene \
  -logFile "$(pwd)/scene.log"
grep -E "\[ASHFALL\]|SceneBuildException|error CS" scene.log
```

Expected: `[ASHFALL] Built Assets/Scenes/Gameplay.unity with a fully wired GameBootstrap.`

- [ ] **Step 5: Run the PlayMode suite and confirm it passes**

Same command as Step 2. Expected: 80 passed, 0 failed.

- [ ] **Step 6: Commit**

```bash
rm -f pm.xml pm.log scene.log
git add Assets/_Game/Editor/GameplaySceneBuilder.cs Assets/Scenes/Gameplay.unity \
        Assets/Tests/PlayMode/GameplaySceneSmokeTests.cs
git commit -m "feat(scene): let the player actually press keys"
```

---

## Verification

```bash
cd "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War"
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics \
  -projectPath . -runTests -testPlatform PlayMode \
  -testResults "$(pwd)/pm.xml" -logFile "$(pwd)/pm.log"
rm -f pm.xml pm.log
```

Expected: PlayMode 80 passed (79 + 1), 0 failed. EditMode is untouched by this change.

## Known gaps left open

- **No view layer.** Only 3 of 28 classes under `Assets/_Game/UI/` contain any draw
  code (`DiegeticHudController`, `DiegeticHudView`, and the `UtilityAIDebugHUD`
  IMGUI debug overlay). The other 25 are formatting models. So pressing F1 to eat
  will change simulation state that nothing on screen reports yet, beyond what the
  diegetic HUD already shows. Giving those models a UI Toolkit view is the next
  increment and needs its own spec — it is a design problem, not a wiring one.
- Key dispatch remains untestable without an injectable input source. If input
  behaviour ever needs test coverage at the scene level, that refactor comes first.
