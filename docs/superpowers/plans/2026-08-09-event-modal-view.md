# Event Modal View Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Draw the event prompt and its choices, closing the last gap in a complete gameplay loop.

**Architecture:** `EventRunner` already fires events, `EventModalUI` already resolves the body text and the visible choices, and `PlayerInputHandler` already routes keys 1/2/3 to `SelectChoice`. Every part of the loop exists except the drawing. This adds one more hide/show panel to the diegetic HUD, following the `hatch-panel` pattern.

**Tech Stack:** Unity 6000.5.5f1, C#, UI Toolkit, NUnit + Unity Test Framework.

## Global Constraints

- Unity editor: `/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity`.
- **Never pass `-quit` alongside `-runTests`.**
- Re-read the diff before launching a run; runs cost 5–8 min and must not exceed 10.
- Element-name constants are the contract between the UXML and `Build()`. Anything added to one goes in the other, same commit.
- Delete run artifacts before committing.

## Two decisions, with reasons

**No UI Toolkit focus or modal blocking.** The spec for the vitals panel guessed
the event modal would need to take keyboard focus and block input. It does not:
`PlayerInputHandler.Update` reads `Input.GetKeyDown(Alpha1/2/3)` directly and
routes to `SelectChoice`, and it already gives workbench and horror panels
priority over event choices. Adding a UI Toolkit focus trap would create a
second, competing input path for the same keys. So this is a panel that shows
and hides, nothing more.

**Repaint by change detection in `HUD.Update`, not by subscribing to
`OnEventTriggered`.** Both `EventModalUI` and `HUD` would be subscribers of the
same runner event, and the modal's state must be updated before the HUD paints
it. That ordering holds only because `BindEventRunner` runs before the HUD would
subscribe — a registration-order dependency that no test would catch when it
broke. Comparing `IsOpen` and the active event id against the previous frame
costs two comparisons and depends on nothing.

---

### Task 1: Paint the event panel in the view

**Files:**
- Modify: `Assets/_Game/UI/DiegeticHudView.cs`
- Test: `Assets/Tests/EditMode/DiegeticHudEventModalTests.cs` (create)

**Interfaces:**
- Produces: `EventChoiceLine` (readonly struct: `Text`, `IsEnabled`), and
  `DiegeticHudView.PaintEventModal(bool open, string title, string body, IReadOnlyList<EventChoiceLine> choices)`,
  plus constants `EventPanelName`, `EventTitleName`, `EventBodyName`, `EventChoicesName`.
- Consumes: nothing new. `EventChoiceLine` deliberately keeps the view free of
  the `Events` namespace, so the tests construct two fields rather than a
  `GameEvent` graph.

- [ ] **Step 1: Write the failing tests**

Create `Assets/Tests/EditMode/DiegeticHudEventModalTests.cs`:

```csharp
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.UIElements;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// The event panel completes the loop: EventRunner fires, EventModalUI
    /// resolves the choices, PlayerInputHandler routes 1/2/3. Only the drawing
    /// was missing.
    /// </summary>
    [TestFixture]
    public class DiegeticHudEventModalTests
    {
        static List<EventChoiceLine> Choices(params (string text, bool enabled)[] rows)
        {
            var list = new List<EventChoiceLine>();
            foreach (var (text, enabled) in rows)
                list.Add(new EventChoiceLine(text, enabled));
            return list;
        }

        [Test]
        public void Build_CreatesTheEventPanelHidden()
        {
            var view = new DiegeticHudView();
            view.Build();

            Assert.IsNotNull(view.EventPanel, "event panel should exist");
            Assert.AreEqual(DisplayStyle.None, view.EventPanel.style.display.value,
                "the event panel starts hidden -- there is no event on boot");
        }

        [Test]
        public void PaintEventModal_ShowsTitleBodyAndChoices()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintEventModal(true, "A knock at the hatch", "Someone is outside.",
                Choices(("Open the hatch", true), ("Stay silent", true)));

            Assert.AreEqual(DisplayStyle.Flex, view.EventPanel.style.display.value);
            Assert.AreEqual("A knock at the hatch", view.EventTitle.text);
            Assert.AreEqual("Someone is outside.", view.EventBody.text);
            Assert.AreEqual(2, view.EventChoices.childCount);
        }

        /// <summary>
        /// The numbers are the control scheme: PlayerInputHandler maps Alpha1 to
        /// visible index 0. A row that does not show its number is unusable.
        /// </summary>
        [Test]
        public void PaintEventModal_NumbersChoicesFromOne()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintEventModal(true, "t", "b", Choices(("First", true), ("Second", true)));

            var first = view.EventChoices[0] as Label;
            var second = view.EventChoices[1] as Label;
            StringAssert.StartsWith("[1]", first.text);
            StringAssert.StartsWith("[2]", second.text);
        }

        [Test]
        public void PaintEventModal_MarksUnavailableChoices()
        {
            var view = new DiegeticHudView();
            view.Build();

            view.PaintEventModal(true, "t", "b", Choices(("Bribe them", false)));

            var row = view.EventChoices[0];
            Assert.IsTrue(row.ClassListContains("event-choice--disabled"),
                "an unavailable choice must look different from one you can press");
        }

        [Test]
        public void PaintEventModal_ClosedHidesThePanel()
        {
            var view = new DiegeticHudView();
            view.Build();
            view.PaintEventModal(true, "t", "b", Choices(("a", true)));

            view.PaintEventModal(false, null, null, null);

            Assert.AreEqual(DisplayStyle.None, view.EventPanel.style.display.value);
        }

        [Test]
        public void PaintEventModal_WithNulls_DoesNotThrow()
        {
            var view = new DiegeticHudView();
            view.Build();

            Assert.DoesNotThrow(() => view.PaintEventModal(true, null, null, null));
        }

        [Test]
        public void BindExisting_FailsWhenTheEventPanelIsMissing()
        {
            var root = new VisualElement { name = DiegeticHudView.RootName };
            var view = new DiegeticHudView();

            Assert.IsFalse(view.BindExisting(root));
        }
    }
}
```

- [ ] **Step 2: Add `EventChoiceLine`**

New file `Assets/_Game/UI/EventChoiceLine.cs`:

```csharp
namespace AtomicWar._Game.UI
{
    /// <summary>
    /// One row of an event prompt, flattened for drawing. Keeps DiegeticHudView
    /// independent of the Events namespace: the view needs a string and whether
    /// the row can be pressed, not a GameEvent graph.
    /// </summary>
    public readonly struct EventChoiceLine
    {
        public readonly string Text;
        public readonly bool IsEnabled;

        public EventChoiceLine(string text, bool isEnabled)
        {
            Text = text;
            IsEnabled = isEnabled;
        }
    }
}
```

- [ ] **Step 3: Extend the view**

Constants beside the others:

```csharp
        public const string EventPanelName = "event-panel";
        public const string EventTitleName = "event-title";
        public const string EventBodyName = "event-body";
        public const string EventChoicesName = "event-choices";
```

Properties:

```csharp
        public VisualElement EventPanel { get; private set; }
        public Label EventTitle { get; private set; }
        public Label EventBody { get; private set; }
        public VisualElement EventChoices { get; private set; }
```

In `Build`, after the vitals panel:

```csharp
            EventPanel = MakePanel(EventPanelName, "event-panel");
            EventTitle = MakeLabel(EventTitleName, "diegetic-title");
            EventBody = MakeLabel(EventBodyName, "diegetic-body");
            EventChoices = new VisualElement { name = EventChoicesName };
            EventChoices.AddToClassList("event-choices");
            EventPanel.Add(EventTitle);
            EventPanel.Add(EventBody);
            EventPanel.Add(EventChoices);
            Root.Add(EventPanel);
```

and add `SetVisible(EventPanel, false);` beside the other two at the end of `Build`.

In `BindExisting`, before the return:

```csharp
            EventPanel = Root.Q<VisualElement>(EventPanelName);
            EventTitle = Root.Q<Label>(EventTitleName);
            EventBody = Root.Q<Label>(EventBodyName);
            EventChoices = Root.Q<VisualElement>(EventChoicesName);
```

and extend the return with `&& EventPanel != null`.

Paint:

```csharp
        /// <summary>
        /// Draw the event prompt. The row numbers are the control scheme, not
        /// decoration: PlayerInputHandler maps Alpha1 to visible index 0, so a
        /// row that does not show its number cannot be chosen.
        /// </summary>
        public void PaintEventModal(
            bool open, string title, string body, IReadOnlyList<EventChoiceLine> choices)
        {
            if (EventPanel == null) return;
            SetVisible(EventPanel, open);
            if (!open) return;

            if (EventTitle != null) EventTitle.text = title ?? string.Empty;
            if (EventBody != null) EventBody.text = body ?? string.Empty;
            if (EventChoices == null) return;

            EventChoices.Clear();
            if (choices == null) return;

            for (int i = 0; i < choices.Count; i++)
            {
                var row = new Label($"[{i + 1}] {choices[i].Text}")
                {
                    name = "event-choice-" + i
                };
                row.AddToClassList("event-choice");
                row.EnableInClassList("event-choice--disabled", !choices[i].IsEnabled);
                EventChoices.Add(row);
            }
        }
```

- [ ] **Step 4: Run EditMode**

```bash
cd "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War"
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics \
  -projectPath . -runTests -testPlatform EditMode \
  -testResults "$(pwd)/em.xml" -logFile "$(pwd)/em.log"
```

Expected: 1858 passed (1851 + 7), 0 failed.

---

### Task 2: Declare it in the UXML and style it

**Files:**
- Modify: `Assets/_Game/UI/DiegeticHud.uxml`, `Assets/_Game/UI/DiegeticHud.uss`

- [ ] **Step 1: UXML**, after `vitals-panel`:

```xml
        <ui:VisualElement name="event-panel" class="diegetic-panel event-panel hidden">
            <ui:Label name="event-title" class="diegetic-title" text="" />
            <ui:Label name="event-body" class="diegetic-body" text="" />
            <ui:VisualElement name="event-choices" class="event-choices" />
        </ui:VisualElement>
```

- [ ] **Step 2: USS**, appended:

```css
.event-panel {
    min-width: 420px;
    max-width: 560px;
}

.event-choices {
    flex-direction: column;
    margin-top: 6px;
}

.event-choice {
    margin-bottom: 3px;
    white-space: normal;
}

.event-choice--disabled {
    opacity: 0.45;
}
```

---

### Task 3: Drive it from the HUD

**Files:**
- Modify: `Assets/_Game/UI/DiegeticHudController.cs`, `Assets/_Game/UI/HUD.cs`
- Test: `Assets/Tests/PlayMode/GameplaySceneSmokeTests.cs`

- [ ] **Step 1: PlayMode test**

```csharp
        /// <summary>
        /// No event is guaranteed to fire inside a smoke test, so this asserts
        /// the panel is bound and correctly hidden -- the state that proves the
        /// wiring exists without depending on the event schedule.
        /// </summary>
        [UnityTest]
        public IEnumerator EventPanel_IsBoundAndHiddenUntilAnEventFires()
        {
            var hud = Object.FindAnyObjectByType<HUD>();
            Assert.IsNotNull(hud);

            for (int i = 0; i < 30; i++)
                yield return null;

            var view = hud.DiegeticHud != null ? hud.DiegeticHud.View : null;
            Assert.IsNotNull(view.EventPanel, "event panel should be bound from the UXML");

            bool modalOpen = hud.EventModalUI != null && hud.EventModalUI.IsOpen;
            Assert.AreEqual(
                modalOpen ? UnityEngine.UIElements.DisplayStyle.Flex
                          : UnityEngine.UIElements.DisplayStyle.None,
                view.EventPanel.style.display.value,
                "panel visibility must track EventModalUI.IsOpen");
        }
```

Check `HUD` exposes `EventModalUI`; if the property has another name, match it.

- [ ] **Step 2: Controller forwarder**

```csharp
        /// <summary>Forward an event-prompt paint.</summary>
        public void PaintEventModal(
            bool open, string title, string body, IReadOnlyList<EventChoiceLine> choices)
        {
            EnsureBuilt();
            if (_view == null || _view.Root == null) return;
            _view.PaintEventModal(open, title, body, choices);
        }
```

- [ ] **Step 3: HUD change detection**

Add fields and extend `Update`:

```csharp
        private bool _lastModalOpen;
        private string _lastModalEventId;
```

In `Update`, after the debug-key check:

```csharp
            RepaintEventModalIfChanged();
```

and:

```csharp
        /// <summary>
        /// Repaint the event prompt when it opens, closes, or swaps to a
        /// different event. Deliberately polled rather than driven from
        /// EventRunner.OnEventTriggered: EventModalUI subscribes to that same
        /// event and must update its state before this paints it, and relying on
        /// subscriber registration order is a dependency no test would catch
        /// when it broke.
        /// </summary>
        private void RepaintEventModalIfChanged()
        {
            if (_eventModalUi == null || _diegeticHud == null) return;

            bool open = _eventModalUi.IsOpen;
            string id = open && _eventModalUi.ActiveEvent != null
                ? _eventModalUi.ActiveEvent.Id
                : null;

            if (open == _lastModalOpen && id == _lastModalEventId) return;
            _lastModalOpen = open;
            _lastModalEventId = id;

            List<EventChoiceLine> lines = null;
            if (open && _eventModalUi.VisibleChoices != null)
            {
                lines = new List<EventChoiceLine>(_eventModalUi.VisibleChoices.Count);
                foreach (var c in _eventModalUi.VisibleChoices)
                    lines.Add(new EventChoiceLine(c.Text, c.IsAvailable && !c.IsGrayedOut));
            }

            _diegeticHud.PaintEventModal(
                open,
                open && _eventModalUi.ActiveEvent != null ? _eventModalUi.ActiveEvent.Title : null,
                open ? _eventModalUi.DisplayBodyText : null,
                lines);
        }
```

Check `GameEvent`'s id property name (`Id` vs `id`) before writing this and match it.

- [ ] **Step 4: Run both suites, then commit**

Expected: EditMode 1858, PlayMode 82, 0 failed.

## Known gaps left open

- No event is forced in the PlayMode test, so the *populated* panel is only
  covered in EditMode. Forcing one would need a deterministic trigger the
  runner does not currently expose.
- The remaining 23 model classes still have no view.
