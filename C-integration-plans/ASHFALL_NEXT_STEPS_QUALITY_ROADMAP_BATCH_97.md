# ASHFALL — Quality Roadmap Batch 97

## Theme: End-to-End Smoke Test Suite — Automated UI Flow Verification in Headless Godot

| Field | Value |
|-------|-------|
| **Priority** | HIGH |
| **Risk** | Medium — requires headless Godot test infrastructure; a partial precedent exists (see below) but has not been proven at the full panel count |
| **Blocking** | No existing tests cover Godot host UI end-to-end; crashes and binding errors are invisible until manual play |
| **Depends on** | Stable `--headless` CLI entry point (`Main.cs`), all **83** panels registered in host sessions |
| **Estimated effort** | 5–7 task blocks |

---

## Problem Statement

**[CORRECTED — see Review Notes]** The Godot host UI layer contains **83** programmatically-built C# panels (`find src/UI -iname "*Panel.cs"` → 83, verified against the live tree; this document previously said 85, which does not match the codebase and does not match wave-2's corrected count either). None of these panels have automated *behavioral* coverage (existence/binding/interaction/navigation) — the exact count of "Core tests" is a separate, unverified claim (see Review Notes) and is not load-bearing for this plan; drop it or re-verify it with `dotnet test` before citing a number. A panel can:

- Crash on open (null reference because a host session field is uninitialized)
- Display stale/wrong values (data binding wired to the wrong property)
- Fail to respond to input (signal not connected, button callback not registered)
- Break navigation (opening Panel A then pressing Back lands on a dead screen)

Manual testing of 83 panels across all game states is impractical. Godot's headless mode provides scene-tree access from C#, and this is **not purely an assumption for this project**: `src/Host/HostCli.PanelTests.cs` (`RunUiLayoutSelfTest`, wired to `--ui-layout-selftest` / `--layout-selftest` in `src/Main.cs`) already instantiates 5 real panels (`MainMenuPanel`, `GameDashboardPanel`, `SettingsPanel`, `InventoryPanel`, `SurvivorsPanel`) headlessly today, sets their size, and adds them to the tree without a display. That proves feasibility for a 5-panel subset under one specific host wiring path, not for all 83 panels — several panels take constructor dependencies, are nested/child-only panels never `new`'d directly by host code, or require a live host session to reach `_Ready()` without throwing. Step 1 and Step 2 below must explicitly reuse/extend this existing precedent rather than reinvent a parallel framework, and must budget for panels that don't fit the parameterless-constructor pattern `RunUiLayoutSelfTest` relies on.

---

## Architecture Overview

```
godot --headless --path . -- --ui-smoke-test
        │
        ▼
┌─────────────────────────┐
│  UiSmokeTestRunner      │  (entry point, registered in Main.cs CLI dispatch)
│  - discovers all panels │
│  - runs test phases     │
│  - reports pass/fail    │
└────────┬────────────────┘
         │
         ▼
┌─────────────────────────┐     ┌──────────────────────────┐
│  MockHostSessions       │────▶│  Panel Under Test        │
│  (stub data, no I/O)   │     │  (instantiated into tree) │
└─────────────────────────┘     └──────────────────────────┘
```

All UI test code lives in `src/Tests/UI/` (Godot host layer — NOT in `Ashfall.Core.Tests`).

---

## Step 1 — Design UI Test Framework (GodotTestRunner)

### Goal

Define the testing infrastructure that boots Godot headlessly, constructs a minimal scene tree, and provides a test API for panel instantiation, data injection, and interaction simulation.

### Implementation

**New files:**

| File | Purpose |
|------|---------|
| `src/Tests/UI/UiSmokeTestRunner.cs` | Top-level runner: discovers tests, executes, reports |
| `src/Tests/UI/UiTestContext.cs` | Provides scene tree, mock sessions, timing utilities |
| `src/Tests/UI/UiTestResult.cs` | Pass/Fail/Error result DTO with panel name + message |
| `src/Tests/UI/IUiTest.cs` | Interface all UI tests implement |

**`UiSmokeTestRunner` design:**

```csharp
namespace AtomicWar.GodotApp.Tests.UI;

public partial class UiSmokeTestRunner : Node
{
    private readonly List<UiTestResult> _results = new();

    public override void _Ready()
    {
        var context = new UiTestContext(GetTree());
        var tests = DiscoverTests();
        foreach (var test in tests)
        {
            var result = RunSafe(test, context);
            _results.Add(result);
        }
        ReportResults(_results);
        GetTree().Quit(_results.Any(r => r.Status == TestStatus.Failed) ? 1 : 0);
    }

    private UiTestResult RunSafe(IUiTest test, UiTestContext ctx)
    {
        try
        {
            test.Execute(ctx);
            return UiTestResult.Pass(test.PanelName);
        }
        catch (Exception ex)
        {
            return UiTestResult.Fail(test.PanelName, ex.Message);
        }
    }
}
```

**`UiTestContext` design:**

```csharp
public class UiTestContext
{
    public SceneTree Tree { get; }
    public Control UiRoot { get; }
    public MockHostSessions Sessions { get; }

    public UiTestContext(SceneTree tree)
    {
        Tree = tree;
        UiRoot = new Control();
        tree.Root.AddChild(UiRoot);
        Sessions = new MockHostSessions();
    }

    /// Adds panel to tree and waits one frame for _Ready to fire.
    public async Task<T> InstantiatePanel<T>() where T : Control, new()
    {
        var panel = new T();
        UiRoot.AddChild(panel);
        await Tree.ToSignal(Tree, SceneTree.SignalName.ProcessFrame);
        return panel;
    }

    /// Simulates a button press by emitting its Pressed signal.
    public void PressButton(BaseButton button)
    {
        button.EmitSignal(BaseButton.SignalName.Pressed);
    }

    /// Cleans up all children from UiRoot between tests.
    public void Cleanup()
    {
        foreach (var child in UiRoot.GetChildren())
        {
            child.QueueFree();
        }
    }
}
```

**CLI integration in `Main.cs`:**

```csharp
case "--ui-smoke-test":
    var runner = new UiSmokeTestRunner();
    GetTree().Root.AddChild(runner);
    return; // runner handles quit
```

### Verification

- `UiSmokeTestRunner` compiles as part of `dotnet build Ashfall.csproj` (0 errors)
- Running `godot --headless --path . -- --ui-smoke-test` with zero registered tests exits 0
- Framework does not import `UnityEngine.*` or touch `Assets/Ashfall.Core/`

### Done-when

- [ ] `src/Tests/UI/` directory exists with the four framework files
- [ ] `--ui-smoke-test` CLI verb recognized and dispatches to runner
- [ ] Empty test suite exits 0; a deliberately-failing test exits 1
- [ ] No Godot namespace leaks into Core

---

## Step 2 — Panel Existence Tests (Instantiation Without Crash)

### Goal

For every one of the 83 registered panels, verify it can be instantiated and added to the scene tree without throwing. This catches null references in constructors, missing theme lookups, and initialization order bugs.

### Implementation

**New file:** `src/Tests/UI/PanelExistenceTests.cs`

**Panel registry approach:** Create a static manifest listing all panel types. **[CORRECTED]** The example type names below were placeholders that do not all match real classes in `src/UI/` — `DashboardPanel` does not exist (real name: `GameDashboardPanel`), `SurvivorListPanel` does not exist (real name: `SurvivorsPanel`), `TradePanel` does not exist (real name: `TradeDetailPanel`), `RadiationPanel` does not exist (real names: `RadiationDetailPanel` / `RadiationHistoryPanel`). Before writing `PanelManifest.cs`, generate the list mechanically — do not hand-type panel names:

```bash
find src/UI -iname "*Panel.cs" ! -iname "*.uid" | xargs -n1 basename -s .cs | sort
```

This must be run and the manifest cross-checked against its output (83 entries) as part of Step 2, not assumed from memory:

```csharp
namespace AtomicWar.GodotApp.Tests.UI;

public static class PanelManifest
{
    /// All 83 programmatic UI panel types, in registration order.
    /// Generated from: find src/UI -iname "*Panel.cs" ! -iname "*.uid" | xargs -n1 basename -s .cs | sort
    public static readonly Type[] AllPanels = new[]
    {
        typeof(GameDashboardPanel),
        typeof(InventoryPanel),
        typeof(SurvivorsPanel),
        typeof(CraftingPanel),
        typeof(MapPanel),
        typeof(TradeDetailPanel),
        typeof(MedicalPanel),
        typeof(RadiationDetailPanel),
        typeof(ShelterPanel),
        typeof(ExpeditionPanel),
        // ... all 83 entries — verify count equals `find` output before merging
    };
}
```

Note also that some panels in `src/UI/` are not top-level, independently-constructible screens — a handful are detail/child panels (e.g. `SurvivorDetailPanel`, `AchievementDetailPanel`) instantiated by a parent panel rather than by host/menu code directly, and some may take non-default constructor arguments. `PanelExistenceTest`'s `Activator.CreateInstance(_panelType)!` assumes a public parameterless constructor for every one of the 83 — confirm this holds for all of them during Step 2, and carve out an explicit sub-list (with a documented instantiation path) for any panel that doesn't.

**Existence test implementation:**

```csharp
public class PanelExistenceTest : IUiTest
{
    private readonly Type _panelType;
    public string PanelName => _panelType.Name;

    public PanelExistenceTest(Type panelType) => _panelType = panelType;

    public void Execute(UiTestContext ctx)
    {
        var panel = (Control)Activator.CreateInstance(_panelType)!;
        ctx.UiRoot.AddChild(panel);
        // If _Ready throws, the test fails via RunSafe catch
        // Wait one frame to ensure deferred calls execute
        ctx.Tree.ToSignal(ctx.Tree, SceneTree.SignalName.ProcessFrame).AsTask().Wait();
        panel.QueueFree();
    }
}
```

**Discovery in runner:**

```csharp
private IEnumerable<IUiTest> DiscoverTests()
{
    // Phase 1: existence
    foreach (var type in PanelManifest.AllPanels)
        yield return new PanelExistenceTest(type);
    // Phase 2, 3, 4 follow...
}
```

### Verification

- All 83 panels listed in `PanelManifest.AllPanels` (count must match `find src/UI -iname "*Panel.cs" ! -iname "*.uid" | wc -l` exactly, checked in CI or a pre-commit script, not just at authoring time)
- `godot --headless --path . -- --ui-smoke-test` reports 83 existence tests
- Any panel that crashes on instantiation is caught and reported (not swallowed)
- Fix any immediately-crashing panels discovered during this step
- Any panel lacking a public parameterless constructor is documented with its own instantiation path (not silently skipped)

### Done-when

- [ ] `PanelManifest.cs` enumerates all 83 panel types, generated/cross-checked against `find src/UI -iname "*Panel.cs"`, not hand-typed
- [ ] `PanelExistenceTests.cs` generates one test per panel
- [ ] Runner reports per-panel pass/fail for existence phase
- [ ] 0 panels crash on headless instantiation (fix blocking crashes)
- [ ] Panels without a parameterless constructor are enumerated and have an explicit test strategy (not silently excluded from the 83)

---

## Step 3 — Panel Data Binding Tests (Labels Populated From Mock Data)

### Goal

Verify that when a panel opens with valid session data, its visible labels/fields reflect that data — not default text, not empty strings, not placeholder values.

### Implementation

**New files:**

| File | Purpose |
|------|---------|
| `src/Tests/UI/MockHostSessions.cs` | Provides deterministic stub data for all host sessions |
| `src/Tests/UI/PanelBindingTests.cs` | Opens each panel with mock data, asserts labels populated |
| `src/Tests/UI/BindingAssertions.cs` | Helpers: `AssertNoPlaceholders`, `AssertLabelNotEmpty` |

**`MockHostSessions` design:**

```csharp
public class MockHostSessions
{
    public SurvivorsHostSession Survivors { get; }
    public InventoryHostSession Inventory { get; }
    public EconomyHostSession Economy { get; }
    public MedicalHostSession Medical { get; }
    public ExpeditionHostSession Expedition { get; }
    public NarrativeHostSession Narrative { get; }
    // ... all host sessions with deterministic test data

    public MockHostSessions()
    {
        var rng = new CoreSeededRng(seed: 12345);
        // Build minimal valid state for each session
        Survivors = BuildMockSurvivors(rng);
        Inventory = BuildMockInventory(rng);
        // ...
    }
}
```

**Binding assertion approach:**

```csharp
public static class BindingAssertions
{
    private static readonly string[] Placeholders = { "TODO", "PLACEHOLDER", "???", "Label", "0/0" };

    public static void AssertNoPlaceholders(Control panel)
    {
        var labels = panel.FindChildren("*", "Label", recursive: true);
        foreach (Label label in labels)
        {
            if (string.IsNullOrWhiteSpace(label.Text))
                throw new Exception($"Empty label: {label.Name} in {panel.Name}");
            if (Placeholders.Any(p => label.Text.Contains(p, StringComparison.OrdinalIgnoreCase)))
                throw new Exception($"Placeholder text in {label.Name}: '{label.Text}'");
        }
    }

    public static void AssertLabelContains(Control panel, string labelName, string expected)
    {
        var label = panel.FindChild(labelName, recursive: true) as Label
            ?? throw new Exception($"Label '{labelName}' not found in {panel.Name}");
        if (!label.Text.Contains(expected))
            throw new Exception($"Label '{labelName}' expected to contain '{expected}', got '{label.Text}'");
    }
}
```

**Per-panel binding test example:**

```csharp
public class InventoryPanelBindingTest : IUiTest
{
    public string PanelName => "InventoryPanel";

    public void Execute(UiTestContext ctx)
    {
        var panel = new InventoryPanel();
        panel.Bind(ctx.Sessions.Inventory); // inject mock session
        ctx.UiRoot.AddChild(panel);
        ctx.Tree.ToSignal(ctx.Tree, SceneTree.SignalName.ProcessFrame).AsTask().Wait();

        BindingAssertions.AssertNoPlaceholders(panel);
        // Verify specific data appears
        BindingAssertions.AssertLabelContains(panel, "ItemCountLabel", "12"); // mock has 12 items
        panel.QueueFree();
    }
}
```

### Verification

- Mock sessions provide known deterministic values (seed 12345)
- Every panel with data bindings has a corresponding binding test
- Tests fail if label shows placeholder or empty text after binding
- `godot --headless --path . -- --ui-smoke-test` includes binding phase results

### Done-when

- [ ] `MockHostSessions` provides valid stub data for all sessions panels consume
- [ ] At least 40 of 83 panels have binding tests (prioritize: inventory, survivors, medical, economy, expedition)
- [ ] `BindingAssertions` catches empty labels and known placeholder strings
- [ ] All binding tests pass with mock data

---

## Step 4 — Panel Interaction Tests (Button Press Verification)

### Goal

Verify that interactive elements (buttons, toggles, sliders) in panels actually fire their callbacks. Catch disconnected signals, dead buttons, and handlers that crash on invocation.

### Implementation

**New files:**

| File | Purpose |
|------|---------|
| `src/Tests/UI/PanelInteractionTests.cs` | Simulates clicks/toggles, verifies callbacks fire |
| `src/Tests/UI/InteractionRecorder.cs` | Records callback invocations for assertion |

**`InteractionRecorder` design:**

```csharp
public class InteractionRecorder
{
    private readonly List<string> _invocations = new();

    public void Record(string callbackName) => _invocations.Add(callbackName);
    public bool WasCalled(string callbackName) => _invocations.Contains(callbackName);
    public int CallCount(string callbackName) => _invocations.Count(c => c == callbackName);
    public void Clear() => _invocations.Clear();
}
```

**Interaction test pattern:**

```csharp
public class CraftingPanelInteractionTest : IUiTest
{
    public string PanelName => "CraftingPanel_Interactions";

    public void Execute(UiTestContext ctx)
    {
        var panel = new CraftingPanel();
        panel.Bind(ctx.Sessions.Inventory);
        ctx.UiRoot.AddChild(panel);
        ctx.Tree.ToSignal(ctx.Tree, SceneTree.SignalName.ProcessFrame).AsTask().Wait();

        // Find the "Craft" button
        var craftButton = panel.FindChild("CraftButton", recursive: true) as Button
            ?? throw new Exception("CraftButton not found in CraftingPanel");

        // Verify button is not disabled when valid recipe selected
        if (craftButton.Disabled)
            throw new Exception("CraftButton disabled with valid recipe selected");

        // Simulate press — should not throw
        ctx.PressButton(craftButton);

        // Wait one frame for handler to execute
        ctx.Tree.ToSignal(ctx.Tree, SceneTree.SignalName.ProcessFrame).AsTask().Wait();

        // If we reach here without exception, the callback executed safely
        panel.QueueFree();
    }
}
```

**Discoverable button audit helper:**

```csharp
public class AllButtonsRespondTest : IUiTest
{
    private readonly Type _panelType;
    public string PanelName => $"{_panelType.Name}_AllButtons";

    public void Execute(UiTestContext ctx)
    {
        var panel = (Control)Activator.CreateInstance(_panelType)!;
        ctx.UiRoot.AddChild(panel);
        ctx.Tree.ToSignal(ctx.Tree, SceneTree.SignalName.ProcessFrame).AsTask().Wait();

        var buttons = panel.FindChildren("*", "BaseButton", recursive: true);
        foreach (BaseButton button in buttons)
        {
            if (!button.Disabled)
            {
                // Press every enabled button — must not throw
                ctx.PressButton(button);
                ctx.Tree.ToSignal(ctx.Tree, SceneTree.SignalName.ProcessFrame).AsTask().Wait();
            }
        }
        panel.QueueFree();
    }
}
```

### Verification

- Every panel's enabled buttons can be pressed without crash in headless mode
- Targeted interaction tests verify specific callbacks for critical panels (craft, trade, medical)
- `InteractionRecorder` used where callback-fire confirmation is needed beyond "no crash"
- Results integrated into `--ui-smoke-test` report

### Done-when

- [ ] `AllButtonsRespondTest` runs against all 83 panels (generic: press every enabled button)
- [ ] 15+ critical panels have targeted interaction tests (CraftingPanel, TradeDetailPanel, MedicalPanel, ExpeditionPanel, etc.)
- [ ] No enabled button crashes when pressed with mock session data
- [ ] Interaction phase results appear in test report

---

## Step 5 — Navigation Flow Tests (Screen-to-Screen Traversal)

### Goal

Verify that the primary navigation paths through the UI complete without errors: menus open, game starts, panels open from dashboard, back-navigation returns to expected screen, and no orphaned nodes accumulate.

### Implementation

**New file:** `src/Tests/UI/NavigationFlowTests.cs`

**Flow definitions:**

```csharp
public static class NavigationFlows
{
    /// Critical happy-path flows that must never break.
    public static readonly NavigationFlow[] CriticalFlows = new[]
    {
        new NavigationFlow("MainMenu → NewGame → Dashboard",
            new[] { "OpenMainMenu", "ClickNewGame", "WaitForDashboard", "AssertDashboardVisible" }),

        new NavigationFlow("Dashboard → InventoryPanel → Back → Dashboard",
            new[] { "OpenDashboard", "ClickInventory", "WaitForPanel", "ClickBack", "AssertDashboardVisible" }),

        new NavigationFlow("Dashboard → MapPanel → SelectLocation → ExpeditionPanel → Back → Map → Back → Dashboard",
            new[] { "OpenDashboard", "ClickMap", "SelectLocation", "ClickExpedition",
                     "ClickBack", "AssertMapVisible", "ClickBack", "AssertDashboardVisible" }),

        new NavigationFlow("Dashboard → SurvivorsPanel → SelectSurvivor → DetailPanel → Back → List",
            new[] { "OpenDashboard", "ClickSurvivors", "SelectFirst", "AssertDetailVisible",
                     "ClickBack", "AssertListVisible" }),

        new NavigationFlow("Dashboard → SettingsPanel → Back → Dashboard",
            new[] { "OpenDashboard", "ClickSettings", "AssertSettingsVisible",
                     "ClickBack", "AssertDashboardVisible" }),
    };
}
```

**Flow executor:**

**[FLAGGED — unscoped dependency]** The code below calls `new TestNavigator(ctx)` and `navigator.Execute(step)`, but `TestNavigator` is never defined anywhere in this plan, and the flows above reference ~15 distinct string steps (`"ClickNewGame"`, `"ClickMap"`, `"SelectLocation"`, `"ClickExpedition"`, `"ClickSurvivors"`, `"SelectFirst"`, `"ClickSettings"`, etc.). Each string step requires `TestNavigator` to know which button/control to find and click for that specific screen state — this is not boilerplate, it is per-flow, per-panel wiring logic that must be designed and estimated as its own deliverable. Before Step 5 is started, add a `TestNavigator` design (a `Dictionary<string, Action<UiTestContext>>` or a small interpreter) as an explicit sub-task, and budget extra time: this is the step already flagged High risk in the Summary Table, and the missing `TestNavigator` implementation is the concrete reason why.

```csharp
public class NavigationFlowTest : IUiTest
{
    private readonly NavigationFlow _flow;
    public string PanelName => $"Nav_{_flow.Name}";

    public void Execute(UiTestContext ctx)
    {
        var navigator = new TestNavigator(ctx); // NOT YET DESIGNED — see flag above
        foreach (var step in _flow.Steps)
        {
            navigator.Execute(step);
            // Wait one frame between navigation steps
            ctx.Tree.ToSignal(ctx.Tree, SceneTree.SignalName.ProcessFrame).AsTask().Wait();
        }
        // Assert no leaked nodes
        var orphans = ctx.UiRoot.GetChildCount();
        if (orphans > 1) // only current screen should remain
            throw new Exception($"Navigation leak: {orphans} children remain after flow '{_flow.Name}'");
    }
}
```

**Node leak detection:**

```csharp
public class NodeLeakDetector
{
    private int _baselineCount;

    public void TakeBaseline(SceneTree tree)
    {
        _baselineCount = CountAllNodes(tree.Root);
    }

    public void AssertNoLeak(SceneTree tree, int tolerance = 5)
    {
        var current = CountAllNodes(tree.Root);
        var leaked = current - _baselineCount;
        if (leaked > tolerance)
            throw new Exception($"Node leak detected: {leaked} nodes above baseline");
    }

    private int CountAllNodes(Node root)
    {
        int count = 1;
        foreach (var child in root.GetChildren())
            count += CountAllNodes(child);
        return count;
    }
}
```

### Verification

- All 5+ critical navigation flows complete without exceptions
- No node leaks detected (tolerance: 5 nodes for deferred-free timing)
- Back navigation always returns to the expected parent screen
- Flow tests run as part of `--ui-smoke-test`

### Done-when

- [ ] `TestNavigator` designed and implemented (maps each flow-step string to a concrete find-control + click/select action against the real panel tree; this is new scope not covered elsewhere in this document)
- [ ] 5 critical navigation flows defined and passing, where "passing" means: every step's target control was found (not silently skipped), every click completed without an exception, and the final screen matches the flow's expected end state (verified via an explicit assertion per flow, e.g. `AssertDashboardVisible`, not just "no exception thrown")
- [ ] `NodeLeakDetector` catches leaked nodes above tolerance
- [ ] No navigation path lands on a null/freed screen
- [ ] Navigation flow results in test report

---

## Step 6 — CLI Verb and Reporting (`--ui-smoke-test`)

### Goal

Provide a single CLI verb that runs all UI smoke tests headlessly and produces a structured, CI-friendly report (per-panel pass/fail, timings, summary).

### Implementation

**Report format (stdout):**

```
═══════════════════════════════════════════════════════════
  ASHFALL UI SMOKE TEST — Headless Godot
═══════════════════════════════════════════════════════════

Phase 1: Panel Existence (83 tests)
  [PASS] GameDashboardPanel ............... 12ms
  [PASS] InventoryPanel .................. 8ms
  [FAIL] CraftingPanel ................... 3ms
         → NullReferenceException: _recipeList was null in _Ready
  ...

Phase 2: Data Binding (43 tests)
  [PASS] InventoryPanel_Binding .......... 15ms
  ...

Phase 3: Interactions (100 tests)
  [PASS] CraftingPanel_AllButtons ........ 22ms
  ...

Phase 4: Navigation Flows (5 tests)
  [PASS] MainMenu → NewGame → Dashboard . 45ms
  ...

═══════════════════════════════════════════════════════════
  SUMMARY: 231 tests | 229 passed | 2 failed | 0 errors
  Total time: 3.2s
  Exit code: 1 (failures present)
═══════════════════════════════════════════════════════════
```

**Machine-readable output (optional `--ui-smoke-test-json`):**

```csharp
if (args.Contains("--ui-smoke-test-json"))
{
    var json = SystemTextJsonSerializer.Serialize(_results);
    System.IO.File.WriteAllText("ui-smoke-results.json", json);
}
```

**Integration in `Main.cs` CLI dispatch:**

```csharp
"--ui-smoke-test" or "--ui-smoke-test-json" => RunUiSmokeTests(args),
```

**Exit codes:**
- `0` — all tests passed
- `1` — one or more tests failed
- `2` — framework error (could not boot scene tree)

### Verification

- `godot --headless --path . -- --ui-smoke-test` runs all phases and exits with correct code
- Report is readable in terminal and parseable by CI
- Timing per test is reported for performance regression detection
- JSON output (when requested) is valid and machine-parseable

### Done-when

- [ ] `--ui-smoke-test` verb runs all 4 test phases
- [ ] Structured report printed to stdout with per-test timings
- [ ] Exit code reflects pass (0) vs fail (1) vs error (2)
- [ ] Optional `--ui-smoke-test-json` writes machine-readable results
- [ ] Verb documented in project verification checklist

---

## Step 7 — CI Pipeline Integration

### Goal

Add UI smoke tests to the CI pipeline so that any commit breaking a UI panel is caught before merge. The test must run in the existing `godot --headless` CI environment.

### Implementation

**CI script addition (`scripts/ci/ui-smoke-gate.sh`):**

```bash
#!/usr/bin/env bash
set -euo pipefail

echo "=== UI Smoke Test Gate ==="

# Run headless UI smoke tests
godot --headless --path . -- --ui-smoke-test-json
EXIT_CODE=$?

# Parse results
if [ $EXIT_CODE -eq 0 ]; then
    echo "UI Smoke Tests: ALL PASSED"
elif [ $EXIT_CODE -eq 1 ]; then
    echo "UI Smoke Tests: FAILURES DETECTED"
    echo "See ui-smoke-results.json for details"
    # Print failed tests
    python3 -c "
import json, sys
results = json.load(open('ui-smoke-results.json'))
failed = [r for r in results if r['status'] == 'Failed']
for f in failed:
    print(f'  FAIL: {f[\"panelName\"]} — {f[\"message\"]}')
print(f'  {len(failed)} failures out of {len(results)} tests')
" 2>/dev/null || cat ui-smoke-results.json
    exit 1
elif [ $EXIT_CODE -eq 2 ]; then
    echo "UI Smoke Tests: FRAMEWORK ERROR (could not boot scene tree)"
    exit 2
fi
```

**Integration with existing `godot-asset-gate.sh`:**

**[CORRECTED]** The existing asset gate script (`scripts/ci/godot-asset-gate.sh`, read directly) runs these headless gates in sequence: `--asset-registry-selftest`, `--data-integrity-selftest`, `--disease-selftest`, `--expansions-selftest`, `--black-flotilla-selftest`, `--radio-selftest`. There is no `--bridge-selftest` gate inside this script — `--bridge-selftest` is a separate, standalone verification step listed in the project's top-level verification checklist (`AGENTS.md`), not part of `godot-asset-gate.sh`. This document previously described the script's gate list as "asset-registry + data-integrity + bridge + disease + expansions," which matches neither the script nor the checklist. Add UI smoke as a new gate step reusing the script's existing `run`/loop pattern:

```bash
# In godot-asset-gate.sh, add to the `for gate in ...` loop or as its own step:
echo "--- UI Smoke Tests ---"
if godot --headless --path . -- --ui-smoke-test; then
    echo "GATE PASS: --ui-smoke-test"
else
    echo "GATE FAIL: --ui-smoke-test" >&2
    fail=1
fi
```

**Failure policy:**
- New panels MUST be added to `PanelManifest.AllPanels` in the same commit
- A panel that crashes on instantiation blocks merge
- Binding test failures are warnings for first 2 weeks (grace period), then blocking — **[FLAGGED]** this grace period has no owner, no tracking mechanism, and no defined start date. Either name who monitors it and where the warning-vs-block toggle lives (a flag in `HotReloadConfig`-style config, a dated comment, a tracked issue), or drop the grace period and make binding failures blocking from day one, consistent with the existence-test policy above.

**Performance budget:**
- All tests must complete in < 30 seconds headless — **[FLAGGED]** the "233+" test count baked into this budget (85 existence + 40–85 binding + 100 interaction + 5 navigation) is derived from the incorrect 85-panel figure and from binding/interaction counts ("40–85", "100+") that are themselves round-number guesses, not measured. Recompute the actual total once Steps 2–5 land on the real 83-panel base, and treat the 30s budget as provisional until a real timing run exists — do not treat it as a hard commitment yet.
- If budget exceeded, split into `--ui-smoke-test-fast` (existence only) and `--ui-smoke-test-full`

### Verification

- CI pipeline runs UI smoke tests on every commit
- Failing panel blocks merge (exit 1)
- Test results available as CI artifact (JSON file)
- No GPU required (headless execution confirmed)

### Done-when

- [ ] `scripts/ci/ui-smoke-gate.sh` exists and is executable
- [ ] UI smoke tests integrated into `godot-asset-gate.sh`
- [ ] CI catches panel crashes before merge
- [ ] Performance budget documented and enforced (< 30s)
- [ ] New-panel checklist updated: "add to PanelManifest"

---

## Summary Table

| Step | Deliverable | Key Files | Tests Added | Risk |
|------|-------------|-----------|-------------|------|
| 1 | UI test framework | `src/Tests/UI/UiSmokeTestRunner.cs`, `UiTestContext.cs`, `IUiTest.cs`, `UiTestResult.cs` | Framework scaffolding (0 game tests yet) | Low — pure infrastructure |
| 2 | Panel existence tests | `src/Tests/UI/PanelManifest.cs`, `PanelExistenceTests.cs` | 83 existence tests | Medium — may discover crashing panels; may discover panels without parameterless constructors |
| 3 | Panel data binding tests | `src/Tests/UI/MockHostSessions.cs`, `PanelBindingTests.cs`, `BindingAssertions.cs` | 40–83 binding tests (estimate, not measured) | Medium — mock sessions must match real shape |
| 4 | Panel interaction tests | `src/Tests/UI/PanelInteractionTests.cs`, `InteractionRecorder.cs` | 100+ interaction tests (estimate, not measured) | Medium — button discovery may find dead handlers |
| 5 | Navigation flow tests | `src/Tests/UI/NavigationFlowTests.cs` | 5+ flow tests | High — navigation stack must be testable |
| 6 | CLI verb and reporting | Updates to `src/Main.cs`, report formatting | 0 (infra) | Low |
| 7 | CI integration | `scripts/ci/ui-smoke-gate.sh`, update `godot-asset-gate.sh` | 0 (infra) | Low — extends existing CI pattern |

**Total new automated tests:** ~230+ (83 existence + 40–83 binding + 100 interaction + 5 navigation) — **this total is a planning estimate, not a commitment; recompute after Step 2–4 land on real panel/button/label counts.**

---

## Dependencies and Sequencing

```
Step 1 (framework) ──┬── Step 2 (existence) ──┬── Step 6 (CLI/reporting)
                     │                         │
                     ├── Step 3 (binding) ─────┤
                     │                         │
                     └── Step 4 (interaction) ─┘── Step 7 (CI)
                                               │
                         Step 5 (navigation) ───┘
```

Steps 2, 3, 4, 5 can proceed in parallel once Step 1 is complete. Steps 6 and 7 integrate the results.

---

## Risks and Mitigations

| Risk | Impact | Mitigation |
|------|--------|------------|
| Panels require GPU for layout | Tests fail in headless | Godot `gl_compatibility` supports headless; test node existence not pixel positions. `--ui-layout-selftest` (existing, `HostCli.PanelTests.cs`) already proves 5 panels construct headlessly under this renderer — extend that precedent instead of re-verifying from zero |
| Mock sessions drift from real sessions | False passes | Generate mocks from same interfaces as real host sessions; shared shape |
| 83 panels too many for single test run | Timeout | Budget 30s (provisional, unmeasured — see Step 7); split into fast/full if needed |
| Panel discovery out of sync with code | Tests miss new panels | Require `PanelManifest` update in same commit as new panel; add a CI check that fails if `find src/UI -iname "*Panel.cs"` count != `PanelManifest.AllPanels.Length` |
| Headless Godot scene tree behaves differently | Tests pass but real UI fails | Supplement with occasional manual smoke test; headless covers crash-level bugs |
| Panel lacks a public parameterless constructor | `Activator.CreateInstance` throws, false-fails an otherwise-healthy panel | Audit constructors in Step 2 before writing `PanelExistenceTest`; document exceptions with a dedicated instantiation path |
| A test that crashes the runner itself (not the panel) hangs the CI job | CI job times out instead of failing fast, burning the timeout budget on every run | Wrap `UiSmokeTestRunner._Ready` execution in a top-level watchdog/timeout in `Main.cs` dispatch, or run the framework's own bring-up (Step 1) against zero tests first in isolation before any panel test is added |

**Rollback plan:** every deliverable in this batch is additive (new files under `src/Tests/UI/` and `Ashfall.Core.Tests/`, plus a new CLI verb and a new CI gate step). If Step 2 or later discovers panel crashes that block merges and cannot be fixed within the batch's timebox, the CI gate addition from Step 7 can be reverted or made non-blocking (warning-only) independently of the test framework itself — the framework, manifest, and test files can stay in the repo unused without affecting existing gameplay, save, or data-integrity gates. No existing panel code is modified by this batch except crash fixes discovered along the way, which should land as their own reviewable commits (per the "one system per task" rule) rather than bundled into the test-infrastructure commits.

---

## Success Criteria

- [ ] `godot --headless --path . -- --ui-smoke-test` exits 0 with all 83 panels passing
- [ ] Zero panels crash on headless instantiation
- [ ] CI blocks merges that break UI panels (existence-test failures); binding-test failure policy — warning window vs. immediately blocking — is decided and documented before Step 7 ships, not left as an undated "grace period"
- [ ] New panel creation checklist includes manifest + test requirement, enforced by a CI count check (not just documentation)
- [ ] Test suite runs in < 30 seconds, re-measured against the real (not estimated) test count once Steps 2–5 are complete


---

## Review Notes (Corrected)

This document was adversarially reviewed against the live codebase at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` and edited in place. Summary of findings:

### Factual corrections applied

1. **Panel count was wrong.** The original document claimed 85 panels. `find src/UI -iname "*Panel.cs" ! -iname "*.uid"` returns **83**. All references to "85" throughout the document (problem statement, dependencies, Steps 2/4, summary table, report examples, risks, success criteria) have been changed to 83. This matches the wave-2 corrected figure cited in the review brief.
2. **".tscn scenes" claim is directionally correct but was unstated/unverified in the original.** The repo has exactly 5 `.tscn` files total: `scenes/Main.tscn`, `scenes/CSharpTest.tscn`, `scenes/HoldfastInterior.tscn`, `scenes/WastelandMap.tscn`, and `src/World/MapLocationMarkerView.tscn`. None of the 83 `*Panel.cs` files have a corresponding `.tscn` — the panels genuinely are built entirely in C# (`Control`/`Node` subclasses constructed and composed in code), consistent with the "programmatically-built" framing. This is now noted explicitly rather than left implicit.
3. **Fabricated example panel names.** The `PanelManifest.AllPanels` example listed `DashboardPanel`, `SurvivorListPanel`, `TradePanel`, and `RadiationPanel` — none of these classes exist. Real names are `GameDashboardPanel`, `SurvivorsPanel`, `TradeDetailPanel`, and `RadiationDetailPanel`/`RadiationHistoryPanel`. Fixed, and the manifest-generation instructions now tell the implementer to derive the list mechanically from `find` rather than transcribe it by hand.
4. **"1941 Core tests" was unverifiable and irrelevant.** This number appears nowhere in the actual xUnit output I could check without running `dotnet test` (out of scope for this pass — I did not fabricate a replacement number). It is also not load-bearing for the plan (Batch 97 is about the *Godot host UI* layer, not Core), so it has been flagged as an unverified claim to drop or re-check, rather than asserted as either correct or a specific wrong number.
5. **`godot-asset-gate.sh` gate list was wrong.** The document said the script runs "asset-registry + data-integrity + bridge + disease + expansions." Reading the script directly, it runs `--asset-registry-selftest`, `--data-integrity-selftest`, `--disease-selftest`, `--expansions-selftest`, `--black-flotilla-selftest`, `--radio-selftest` — no `--bridge-selftest` (that verb lives in the separate top-level `AGENTS.md` verification checklist, not this script), and two extra gates (`black-flotilla`, `radio`) were omitted from the description. The CI integration section's suggested patch also used a `FAILURES` counter variable that doesn't match the script's actual `fail=1` pattern; the patch snippet was corrected to match the script's real style (a `for gate in ...; do ... done` loop with `fail=1`).

### Feasibility claim: headless UI construction — was assumed, is now grounded

The review brief specifically asked whether "Godot headless mode can actually construct UI nodes without a display" was verified as feasible or merely assumed. In the original document, **it was assumed** — no existing precedent was cited anywhere in the plan, and the Risks table just asserted "Godot `gl_compatibility` supports headless" without pointing at any proof in this codebase.

There **is** real precedent: `src/Host/HostCli.PanelTests.cs` (`RunUiLayoutSelfTest`), wired to `--ui-layout-selftest` / `--layout-selftest` in `src/Main.cs`, already constructs `MainMenuPanel`, `GameDashboardPanel`, `SettingsPanel`, `InventoryPanel`, and `SurvivorsPanel` headlessly, sizes them, and adds them to a tree, across 8 resolutions, without a display. This proves feasibility **for a 5-panel subset that happens to have parameterless constructors and no host-session dependency at construction time** — it does not prove it for all 83 panels. Some panels are detail/child panels normally instantiated by a parent (not directly by menu/host code), and some may have non-default constructors. The document now cites this precedent explicitly (Problem Statement + Risks table) and requires Step 2 to audit constructor signatures for all 83 panels rather than assume `Activator.CreateInstance(type)` works uniformly.

### Gaps found and fixed

- **`TestNavigator` was referenced but never designed.** Step 5's `NavigationFlowTest.Execute` calls `new TestNavigator(ctx)` and `navigator.Execute(step)` against ~15 distinct string steps (`"ClickMap"`, `"SelectLocation"`, `"ClickExpedition"`, etc.), but no `TestNavigator` implementation, interface, or even a design sketch appears anywhere in the document. This is exactly the kind of unscoped dependency that causes a "High risk" step (as the Summary Table already flags Step 5) to blow its estimate. Added an explicit flag plus a new Done-when item requiring `TestNavigator` to be designed as its own sub-task before the flow tests can be written.
- **Vague Done-when for navigation flows.** "5 critical navigation flows defined and passing" did not say what "passing" means beyond "no exception." Tightened to require an explicit per-flow end-state assertion (e.g., `AssertDashboardVisible`), not just absence of a thrown exception.
- **Missing risk/rollback section.** The original had a Risks table but no rollback plan. Added one: this batch is fully additive (new files + a new CLI verb + a new CI gate step), so if panel crashes discovered during Step 2 can't be fixed in-timebox, the CI gate can be made non-blocking or reverted independently of the test framework, which can sit unused without touching gameplay/save/data-integrity gates. Also added two risks that were missing: panels without parameterless constructors, and a runner-level hang/timeout risk in CI.
- **Test-count estimates ("40–85 binding", "100+ interaction", "233+ total") were round-number guesses presented as if measured.** Now explicitly labeled as unmeasured estimates to be recomputed once the real panel/button/label counts are known, and the grace-period policy for binding-test failures (previously a bare "2 weeks, then blocking" with no owner) is flagged as needing an explicit owner/mechanism or removal.

### Not changed

- The overall step sequencing (framework → existence → binding/interaction/navigation in parallel → CLI/reporting → CI) is logical and was left as-is.
- The core architecture (`UiSmokeTestRunner`, `UiTestContext`, `IUiTest`, mock sessions) is reasonable and was not restructured — only the factual and scoping issues above were addressed.
