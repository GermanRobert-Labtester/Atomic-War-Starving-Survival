# Vitals Panel Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Render day/time, radiation dose and the four core needs in the diegetic HUD, so the simulation the player is running is actually visible.

**Architecture:** Add one panel to the existing three-part diegetic HUD (`DiegeticHud.uxml` declares the tree, `DiegeticHudView` binds and paints it, `DiegeticHudController` drives it). All data is already live in `NeedsBar` and `DosimeterHUD`; the missing piece is a repaint on need/dose change, which `HUD` gets at the tail of the two methods those events already call.

**Tech Stack:** Unity 6000.5.5f1, C#, UI Toolkit (UXML/USS), NUnit + Unity Test Framework.

**Spec:** `docs/superpowers/specs/2026-08-09-vitals-panel-design.md`

## Global Constraints

- Unity editor: `/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity`.
- **Never pass `-quit` alongside `-runTests`.** It kills the editor mid-run and exits 0 with an empty result file.
- Re-read the diff for bugs *before* launching a Unity run; runs cost 5–8 minutes and must not exceed 10.
- `Gameplay.unity` is generated. Never hand-edit it. Note it churns ~480 lines per rebuild (Unity renumbers fileIDs), so only rebuild it when the builder actually changed.
- Element-name constants on `DiegeticHudView` are the contract between the UXML and `Build()`. Anything added to one goes in the other, in the same commit.
- Delete run artifacts (`em.xml`, `em.log`, `pm.xml`, `pm.log`) before committing.

---

## File Structure

| File | Responsibility |
| --- | --- |
| `Assets/_Game/UI/DiegeticHudView.cs` | Vitals element constants, `Build`, `BindExisting`, `PaintVitals`. |
| `Assets/_Game/UI/DiegeticHud.uxml` | Declarative mirror of the vitals panel. |
| `Assets/_Game/UI/DiegeticHud.uss` | Bar fill and vitals row styling. |
| `Assets/_Game/UI/DiegeticHudController.cs` | Forward `PaintVitals` from the models. |
| `Assets/_Game/UI/HUD.cs` | Repaint on need/dose change. |
| `Assets/Tests/EditMode/DiegeticHudVitalsTests.cs` | View-level coverage. |
| `Assets/Tests/PlayMode/GameplaySceneSmokeTests.cs` | Live-data assertion in the real scene. |

---

### Task 1: The vitals panel in the view

**Files:**
- Modify: `Assets/_Game/UI/DiegeticHudView.cs`
- Test: `Assets/Tests/EditMode/DiegeticHudVitalsTests.cs` (create)

**Interfaces:**
- Produces: `DiegeticHudView.PaintVitals(int day, float hour, float dose, float rate, IReadOnlyDictionary<string, NeedBarData> needs)`, and the constants `VitalsPanelName`, `VitalsClockName`, `VitalsDoseName`, `VitalsNeedsName`.
- Consumes: `NeedBarData` from `AtomicWar._Game.UI` (fields `DisplayName`, `CurrentValue`, `MaxValue`, `IsCritical`).

- [ ] **Step 1: Write the failing tests**

Create `Assets/Tests/EditMode/DiegeticHudVitalsTests.cs`:

```csharp
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.UIElements;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// The vitals panel is the only on-screen report of the core loop: time,
    /// dose and the four needs. DiegeticHudView builds a VisualElement tree with
    /// no UIDocument, so it can be painted and read back here.
    /// </summary>
    [TestFixture]
    public class DiegeticHudVitalsTests
    {
        static Dictionary<string, NeedBarData> Needs(params (string id, float value)[] entries)
        {
            var d = new Dictionary<string, NeedBarData>();
            foreach (var (id, value) in entries)
                d[id] = new NeedBarData { NeedId = id, DisplayName = id.ToUpperInvariant(), CurrentValue = value, MaxValue = 100f };
            return d;
        }

        [Test]
        public void Build_CreatesTheVitalsPanel()
        {
            var view = new DiegeticHudView();
            view.Build();

            Assert.IsNotNull(view.VitalsPanel, "vitals panel should exist");
            Assert.IsNotNull(view.VitalsClock, "clock label should exist");
            Assert.IsNotNull(view.VitalsDose, "dose label should exist");
        }

        [Test]
        public void PaintVitals_ShowsDayTimeAndDose()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintVitals(3, 4f, 0.42f, 1.5f, Needs(("hunger", 62f)));

            StringAssert.Contains("DAY 3", view.VitalsClock.text);
            StringAssert.Contains("04:00", view.VitalsClock.text);
            StringAssert.Contains("0.42", view.VitalsDose.text);
        }

        [Test]
        public void PaintVitals_RendersOneRowPerNeed()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintVitals(1, 0f, 0f, 0f,
                Needs(("hunger", 62f), ("thirst", 48f), ("fatigue", 25f), ("warmth", 71f)));

            Assert.AreEqual(4, view.VitalsNeeds.childCount, "one row per need");
        }

        /// <summary>
        /// Zero is a meaningful reading. A need the model does not carry is not
        /// the same as a need at zero, and must not be drawn as starvation.
        /// </summary>
        [Test]
        public void PaintVitals_RendersAbsentNeedAsPlaceholder_NotZero()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintVitals(1, 0f, 0f, 0f, Needs(("hunger", 0f)));

            var row = view.VitalsNeeds.Q<Label>("vitals-need-thirst-value");
            Assert.IsNotNull(row, "every core need gets a row even when absent from the model");
            Assert.AreEqual("--", row.text, "absent need must not read as 0%");
        }

        [Test]
        public void PaintVitals_WithNulls_DoesNotThrow()
        {
            var view = new DiegeticHudView();
            view.Build();

            Assert.DoesNotThrow(() => view.PaintVitals(1, 0f, 0f, 0f, null));
        }

        /// <summary>
        /// Drift guard. The UXML and Build() describe the same tree twice; if the
        /// UXML loses an element, BindExisting must fail so the controller falls
        /// back to Build() rather than binding a half-tree and rendering nothing.
        /// </summary>
        [Test]
        public void BindExisting_FailsWhenTheVitalsPanelIsMissing()
        {
            var root = new VisualElement { name = DiegeticHudView.RootName };
            var view = new DiegeticHudView();

            Assert.IsFalse(view.BindExisting(root),
                "a tree without the vitals panel must not bind successfully");
        }
    }
}
```

- [ ] **Step 2: Run and confirm they fail**

```bash
cd "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War"
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics \
  -projectPath . -runTests -testPlatform EditMode \
  -testResults "$(pwd)/em.xml" -logFile "$(pwd)/em.log"
```

Expected: compile error — `DiegeticHudView` has no `VitalsPanel`. That counts as RED.

- [ ] **Step 3: Add the constants and fields**

In `DiegeticHudView`, beside the existing name constants:

```csharp
        public const string VitalsPanelName = "vitals-panel";
        public const string VitalsClockName = "vitals-clock";
        public const string VitalsDoseName = "vitals-dose";
        public const string VitalsNeedsName = "vitals-needs";

        /// <summary>Core needs, in fixed display order. Fixed so the rows do not
        /// reshuffle between paints as the model's dictionary ordering changes.</summary>
        public static readonly string[] CoreNeedIds = { "hunger", "thirst", "fatigue", "warmth" };
```

and beside the existing properties:

```csharp
        public VisualElement VitalsPanel { get; private set; }
        public Label VitalsClock { get; private set; }
        public Label VitalsDose { get; private set; }
        public VisualElement VitalsNeeds { get; private set; }
```

- [ ] **Step 4: Build the panel**

In `Build`, before the hatch panel (the vitals panel is always visible and reads
first), insert:

```csharp
            VitalsPanel = MakePanel(VitalsPanelName, "vitals-panel");
            VitalsClock = MakeLabel(VitalsClockName, "diegetic-title");
            VitalsDose = MakeLabel(VitalsDoseName, "diegetic-status");
            VitalsNeeds = new VisualElement { name = VitalsNeedsName };
            VitalsNeeds.AddToClassList("vitals-needs");
            VitalsPanel.Add(VitalsClock);
            VitalsPanel.Add(VitalsDose);
            VitalsPanel.Add(VitalsNeeds);
            VitalsPanel.Add(MakeHint("vitals-hint", "[F1] eat  ·  [F2] drink  ·  [SPACE] pause  ·  [F5] save"));
            Root.Add(VitalsPanel);
```

The vitals panel is deliberately absent from the `SetVisible(..., false)` calls
at the end of `Build` — it is the one panel with no toggle.

- [ ] **Step 5: Bind and gate**

In `BindExisting`, add before the return:

```csharp
            VitalsPanel = Root.Q<VisualElement>(VitalsPanelName);
            VitalsClock = Root.Q<Label>(VitalsClockName);
            VitalsDose = Root.Q<Label>(VitalsDoseName);
            VitalsNeeds = Root.Q<VisualElement>(VitalsNeedsName);
```

and extend the return so a UXML missing the panel falls back to `Build()`
instead of binding a half-tree:

```csharp
            return HatchPanel != null && EncounterPanel != null
                && StoresPanel != null && VitalsPanel != null;
```

- [ ] **Step 6: Paint**

```csharp
        /// <summary>
        /// Paint the core-loop readout. Rows are emitted for every id in
        /// <see cref="CoreNeedIds"/> whether or not the model carries it, so the
        /// panel keeps a stable height and an absent need reads as "--" rather
        /// than as zero -- zero means starving, absent means unknown.
        /// </summary>
        public void PaintVitals(
            int day, float hour, float cumulativeDose, float currentRate,
            IReadOnlyDictionary<string, NeedBarData> needs)
        {
            if (VitalsPanel == null) return;

            int h = Mathf.Clamp(Mathf.FloorToInt(hour), 0, 23);
            int m = Mathf.Clamp(Mathf.FloorToInt((hour - h) * 60f), 0, 59);
            if (VitalsClock != null)
                VitalsClock.text = $"DAY {day}   {h:00}:{m:00}";

            if (VitalsDose != null)
                VitalsDose.text = $"☢ {cumulativeDose:0.00} Sv   ({currentRate:0.0}/hr)";

            if (VitalsNeeds == null) return;
            VitalsNeeds.Clear();

            for (int i = 0; i < CoreNeedIds.Length; i++)
            {
                string id = CoreNeedIds[i];
                NeedBarData data = null;
                needs?.TryGetValue(id, out data);
                VitalsNeeds.Add(MakeNeedRow(id, data));
            }
        }

        private static VisualElement MakeNeedRow(string id, NeedBarData data)
        {
            var row = new VisualElement { name = "vitals-need-" + id };
            row.AddToClassList("vitals-row");

            var label = new Label(data?.DisplayName ?? id.ToUpperInvariant())
            {
                name = "vitals-need-" + id + "-label"
            };
            label.AddToClassList("vitals-row__label");
            row.Add(label);

            var track = new VisualElement { name = "vitals-need-" + id + "-track" };
            track.AddToClassList("vitals-row__track");
            var fill = new VisualElement { name = "vitals-need-" + id + "-fill" };
            fill.AddToClassList("vitals-row__fill");
            if (data != null && data.MaxValue > 0f)
                fill.style.width = Length.Percent(Mathf.Clamp01(data.CurrentValue / data.MaxValue) * 100f);
            else
                fill.style.width = Length.Percent(0f);
            fill.EnableInClassList("critical", data != null && data.IsCritical);
            track.Add(fill);
            row.Add(track);

            var value = new Label(data == null
                ? "--"
                : Mathf.RoundToInt(data.CurrentValue).ToString() + "%")
            {
                name = "vitals-need-" + id + "-value"
            };
            value.AddToClassList("vitals-row__value");
            row.Add(value);

            return row;
        }
```

`Mathf` and `Length` need `using UnityEngine;` and `using UnityEngine.UIElements;`, both already present.

- [ ] **Step 7: Run the EditMode suite**

Same command as Step 2. Expected: the 6 new tests pass, existing tests unaffected.

- [ ] **Step 8: Commit**

```bash
rm -f em.xml em.log
git add Assets/_Game/UI/DiegeticHudView.cs Assets/Tests/EditMode/DiegeticHudVitalsTests.cs
git commit -m "feat(ui): render time, dose and needs in the diegetic view"
```

---

### Task 2: Mirror it in the UXML and style it

**Files:**
- Modify: `Assets/_Game/UI/DiegeticHud.uxml`
- Modify: `Assets/_Game/UI/DiegeticHud.uss`

**Interfaces:**
- Consumes: the element names from Task 1. The UXML is the runtime tree; `Build()` is the fallback.

- [ ] **Step 1: Add the panel to the UXML**

Inside `diegetic-root`, as the first child (before `hatch-panel`):

```xml
        <ui:VisualElement name="vitals-panel" class="diegetic-panel vitals-panel">
            <ui:Label name="vitals-clock" class="diegetic-title" text="DAY 1   00:00" />
            <ui:Label name="vitals-dose" class="diegetic-status" text="" />
            <ui:VisualElement name="vitals-needs" class="vitals-needs" />
            <ui:Label name="vitals-hint" class="diegetic-hint" text="[F1] eat  ·  [F2] drink  ·  [SPACE] pause  ·  [F5] save" />
        </ui:VisualElement>
```

No `hidden` class: this panel is always visible.

- [ ] **Step 2: Add the styles**

Append to `DiegeticHud.uss`:

```css
.vitals-panel {
    min-width: 320px;
}

.vitals-needs {
    flex-direction: column;
    margin-top: 4px;
}

.vitals-row {
    flex-direction: row;
    align-items: center;
    margin-bottom: 2px;
}

.vitals-row__label {
    width: 84px;
    -unity-font-style: bold;
}

.vitals-row__track {
    flex-grow: 1;
    height: 8px;
    background-color: rgba(255, 255, 255, 0.08);
}

.vitals-row__fill {
    height: 8px;
    background-color: rgb(51, 204, 77);
}

.vitals-row__fill.critical {
    background-color: rgb(230, 51, 51);
}

.vitals-row__value {
    width: 48px;
    -unity-text-align: middle-right;
}
```

- [ ] **Step 3: Verify the drift guard passes**

The `BindExisting_FailsWhenTheVitalsPanelIsMissing` test covers the negative
case. For the positive case, run the EditMode suite again — nothing should
change, since that suite does not load the UXML. The UXML/`Build()` agreement
is proven in Task 3's PlayMode test, which loads the real document.

- [ ] **Step 4: Commit**

```bash
git add Assets/_Game/UI/DiegeticHud.uxml Assets/_Game/UI/DiegeticHud.uss
git commit -m "feat(ui): declare and style the vitals panel"
```

---

### Task 3: Connect the repaint and prove it live

**Files:**
- Modify: `Assets/_Game/UI/DiegeticHudController.cs`
- Modify: `Assets/_Game/UI/HUD.cs`
- Test: `Assets/Tests/PlayMode/GameplaySceneSmokeTests.cs`

**Interfaces:**
- Produces: `DiegeticHudController.PaintVitals(int, float, float, float, IReadOnlyDictionary<string, NeedBarData>)`, `HUD.SetClock(int day, float hour)`.

- [ ] **Step 1: Write the failing PlayMode test**

Append to `GameplaySceneSmokeTests`:

```csharp
        /// <summary>
        /// The vitals panel is the only on-screen report of the core loop. Its
        /// repaint hangs off need/dose events rather than the discrete actions
        /// that drive the other panels, so this asserts the labels actually hold
        /// live values after some frames -- a panel wired but never repainted
        /// looks identical to one that works, until you watch it.
        /// </summary>
        [UnityTest]
        public IEnumerator Vitals_ShowLiveValuesAfterFrames()
        {
            var hud = Object.FindAnyObjectByType<HUD>();
            Assert.IsNotNull(hud, "Gameplay scene must contain a HUD");

            for (int i = 0; i < 120; i++)
                yield return null;

            var view = hud.DiegeticHud != null ? hud.DiegeticHud.View : null;
            Assert.IsNotNull(view, "diegetic view should exist");
            Assert.IsNotNull(view.VitalsClock, "vitals clock label should be bound");

            StringAssert.Contains("DAY", view.VitalsClock.text,
                "clock should have been painted with a real reading");
            Assert.AreEqual(DiegeticHudView.CoreNeedIds.Length, view.VitalsNeeds.childCount,
                "one row per core need should be painted");
        }
```

- [ ] **Step 2: Run and confirm it fails**

```bash
cd "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War"
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics \
  -projectPath . -runTests -testPlatform PlayMode \
  -testResults "$(pwd)/pm.xml" -logFile "$(pwd)/pm.log"
```

Expected: FAIL — the clock label is empty, because nothing calls `PaintVitals`.

- [ ] **Step 3: Forward the paint from the controller**

In `DiegeticHudController`, beside the other paint forwarders:

```csharp
        /// <summary>Forward a vitals paint. Kept separate from Paint() because it
        /// fires on every need change, while Paint() fires on discrete actions.</summary>
        public void PaintVitals(
            int day, float hour, float cumulativeDose, float currentRate,
            IReadOnlyDictionary<string, NeedBarData> needs)
        {
            EnsureBuilt();
            if (_view == null || _view.Root == null) return;
            _view.PaintVitals(day, hour, cumulativeDose, currentRate, needs);
        }
```

`EnsureBuilt` is the existing method guarding `_built`; check its exact name at
`DiegeticHudController.cs` around line 136 and match it. Add
`using System.Collections.Generic;` if absent.

- [ ] **Step 4: Repaint from HUD on the events that already fire**

Add to `HUD`:

```csharp
        /// <summary>Latest clock reading, pushed in rather than pulled: HUD holds
        /// no bootstrap reference and every other value it shows is pushed too.</summary>
        private int _day = 1;
        private float _hour;

        public void SetClock(int day, float hour)
        {
            _day = day;
            _hour = hour;
            RepaintVitals();
        }

        /// <summary>
        /// The diegetic panels repaint on discrete actions via RefreshDiegeticHud.
        /// Vitals cannot: needs and dose change continuously, and a panel painted
        /// only on mission events would sit frozen while the player starved.
        /// </summary>
        private void RepaintVitals()
        {
            EnsureWidgetReferences();
            if (_diegeticHud == null || _needsBar == null) return;

            _diegeticHud.PaintVitals(
                _day,
                _hour,
                _dosimeterHud != null ? _dosimeterHud.CumulativeDose : 0f,
                _dosimeterHud != null ? _dosimeterHud.CurrentRate : 0f,
                _needsBar.NeedBars);
        }
```

then call `RepaintVitals();` as the last line of both `Bind(Survivor)` and
`OnRadiationUpdated(float, float)`.

- [ ] **Step 5: Push the clock from the bootstrap**

`HUD.SetClock` needs a caller. In `GameBootstrap.Hud.cs`, beside the existing
`_onNeedChanged` wiring, subscribe to the clock:

```csharp
            // Vitals shows day/time, and HUD holds no TimeSystem reference.
            _onHourChanged = () => _hud.SetClock(TimeSystem.CurrentDay, TimeSystem.CurrentHour);
            TimeSystem.OnHourChanged += _onHourChanged;
```

Check `TimeSystem`'s actual event and property names before writing this —
`grep -n "public event\|public int CurrentDay\|CurrentHour" Assets/_Game/Core/TimeSystem.cs`
— and match them, including the delegate signature. Declare `_onHourChanged`
with the matching type beside the other cached handlers, and unsubscribe it
wherever `_onNeedChanged` is unsubscribed.

- [ ] **Step 6: Run both suites**

```bash
cd "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War"
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics \
  -projectPath . -runTests -testPlatform EditMode \
  -testResults "$(pwd)/em.xml" -logFile "$(pwd)/em.log" && \
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics \
  -projectPath . -runTests -testPlatform PlayMode \
  -testResults "$(pwd)/pm.xml" -logFile "$(pwd)/pm.log"
```

Expected: EditMode 1851 passed, PlayMode 81 passed, 0 failed.

- [ ] **Step 7: Commit**

```bash
rm -f em.xml em.log pm.xml pm.log
git add Assets/_Game/UI/DiegeticHudController.cs Assets/_Game/UI/HUD.cs \
        Assets/_Game/Core/GameBootstrap.Hud.cs Assets/Tests/PlayMode/GameplaySceneSmokeTests.cs
git commit -m "feat(ui): repaint vitals when needs and dose change"
```

---

## Verification

Run both suites as in Task 3 Step 6, then confirm by eye that the panel reads
plausibly — this is a visual feature and the tests only prove it is painted, not
that it is legible:

```bash
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics -quit \
  -projectPath . -executeMethod AtomicWar._Game.Editor.BuildScript.PerformBuildPipeline \
  -logFile "$(pwd)/build.log"
./Builds/Linux/ASHFALL.x86_64   # click NEW EXPEDITION, watch the bars move
rm -f build.log
```

## Known gaps left open

- The other 24 model classes still have no view.
- The event modal needs keyboard focus and modal blocking, which this panel's
  pattern does not cover. It needs its own design.
- No localization: the labels here are inline literals, consistent with the rest
  of the project.
- The bar colours are hardcoded in USS rather than read from `NeedBarData`'s
  `NormalColor`/`CriticalColor`. Those fields stay unused by this panel; if the
  designer wants per-need colours, the fill should be styled from them instead.
