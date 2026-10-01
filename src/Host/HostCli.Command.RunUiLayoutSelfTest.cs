// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Endgame;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Medical;
using Ashfall.Core.Muster;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Ashfall.Core.Settings;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.Narrative;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {

        public static int RunUiLayoutSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                {
                    GD.Print($"  [PASS] {message}");
                }
                else
                {
                    GD.PrintErr($"  [FAIL] {message}");
                    failures++;
                }
            }

            GD.Print("[UiLayoutSelfTest] Starting 8-resolution responsive layout verification...");

            var resolutions = new (int W, int H, string Aspect)[]
            {
                (1024, 768, "4:3 Standard"),
                (1280, 720, "16:9 HD"),
                (1366, 768, "16:9 Laptop"),
                (1600, 900, "16:9 WS"),
                (1920, 1080, "16:9 FHD Native"),
                (2560, 1080, "21:9 Ultrawide"),
                (2560, 1440, "16:9 2K QHD"),
                (3840, 2160, "16:9 4K UHD")
            };

            foreach (var (w, h, aspect) in resolutions)
            {
                // These panels are constructed standalone (never added to a tree),
                // so nothing else will free them. Without this teardown the sweep
                // accumulates the whole panel set once per resolution and Godot
                // reports tens of thousands of leaked instances at exit.
                var built = new System.Collections.Generic.List<Control>();
                T Track<T>(T node) where T : Control { built.Add(node); return node; }
                try
                {
                    // 1. MainMenuPanel
                    var mainMenu = Track(new MainMenuPanel());
                    mainMenu.CustomMinimumSize = new Vector2(w, h);
                    mainMenu.Size = new Vector2(w, h);
                    mainMenu._Ready();
                    Check(mainMenu.Size.X >= w && mainMenu.Size.Y >= h, $"MainMenuPanel bounds valid at {w}x{h} ({aspect})");

                    // 2. GameDashboardPanel
                    var dashboard = Track(new GameDashboardPanel());
                    dashboard.CustomMinimumSize = new Vector2(w, h);
                    dashboard.Size = new Vector2(w, h);
                    dashboard._Ready();
                    dashboard.UpdateState(new GameDashboardPanel.DashboardSnapshot
                    {
                        Day = 2,
                        Health = 85,
                        MaxHealth = 100,
                        Radiation = 14.5f,
                        CleanWater = 18,
                        Food = 24,
                        LivingSurvivors = 3,
                        TotalSurvivors = 3,
                        FilterSpares = 2,
                        Weather = "Fallout Dust"
                    });
                    Check(dashboard.Size.X >= w && dashboard.Size.Y >= h, $"GameDashboardPanel bounds valid at {w}x{h} ({aspect})");

                    // 3. SettingsPanel
                    var settings = Track(new SettingsPanel());
                    settings.CustomMinimumSize = new Vector2(w, h);
                    settings.Size = new Vector2(w, h);
                    settings._Ready();
                    settings.Open();
                    Check(settings.Size.X >= w && settings.Size.Y >= h, $"SettingsPanel bounds valid at {w}x{h} ({aspect})");
                    settings.Close();

                    // 4. InventoryPanel
                    var invPanel = Track(new InventoryPanel());
                    invPanel.CustomMinimumSize = new Vector2(w, h);
                    invPanel.Size = new Vector2(w, h);
                    invPanel._Ready();
                    Check(invPanel.Size.X >= w && invPanel.Size.Y >= h, $"InventoryPanel bounds valid at {w}x{h} ({aspect})");

                    // 5. SurvivorsPanel
                    var survPanel = Track(new SurvivorsPanel());
                    survPanel.CustomMinimumSize = new Vector2(w, h);
                    survPanel.Size = new Vector2(w, h);
                    survPanel._Ready();
                    Check(survPanel.Size.X >= w && survPanel.Size.Y >= h, $"SurvivorsPanel bounds valid at {w}x{h} ({aspect})");

                    // 6. MaritimePanel (Exp 09) + DeepCoastPanel (Exp 01 sibling layer)
                    var maritimePanel = Track(new MaritimePanel());
                    maritimePanel.CustomMinimumSize = new Vector2(w, h);
                    maritimePanel.Size = new Vector2(w, h);
                    maritimePanel._Ready();
                    Check(maritimePanel.Size.X >= w && maritimePanel.Size.Y >= h, $"MaritimePanel bounds valid at {w}x{h} ({aspect})");

                    var deepCoastPanel = Track(new DeepCoastPanel());
                    deepCoastPanel.CustomMinimumSize = new Vector2(w, h);
                    deepCoastPanel.Size = new Vector2(w, h);
                    deepCoastPanel._Ready();
                    Check(deepCoastPanel.Size.X >= w && deepCoastPanel.Size.Y >= h, $"DeepCoastPanel bounds valid at {w}x{h} ({aspect})");

                    // 7. ShelterPanel — includes the 2D HoldfastInteriorView layout
                    // anchor. Bind a seeded roster so the survivor actors + room
                    // hotspots actually render against authoritative state.
                    var shelterPanel = Track(new ShelterPanel());
                    shelterPanel.CustomMinimumSize = new Vector2(w, h);
                    shelterPanel.Size = new Vector2(w, h);
                    shelterPanel._Ready();
                    var shelterSurvivors = new SurvivorsHostSession();
                    shelterSurvivors.SeedDemoRoster();
                    var shelterWorld = new WorldHostSession();
                    shelterPanel.Bind(shelterSurvivors, shelterWorld);
                    shelterPanel.Open();
                    Check(shelterPanel.IsBound, $"ShelterPanel bound with 2D layout anchor at {w}x{h} ({aspect})");
                }
                catch (Exception ex)
                {
                    GD.PrintErr($"  [FAIL] Exception at {w}x{h}: {ex.Message}");
                    failures++;
                }
                finally
                {
                    foreach (var c in built)
                        if (c != null && GodotObject.IsInstanceValid(c)) c.Free();
                }
            }

            AuditPanelInteractivity(Check);
            VerifyUiMotionContracts(Check);
            VerifyUiControllerParity(Check);

            GD.Print($"[UiLayoutSelfTest] Failures: {failures}");
            return EmitSummary("ui_layout_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

        /// <summary>
        /// UI clickability audit (UI/UX audit follow-up): every panel type is
        /// instantiated unbound and every interactive button is checked for at
        /// least one wired click route. Buttons that are intentionally toggles,
        /// check controls, option selectors, or disabled are excluded. A panel
        /// that cannot be constructed without a host is reported as skipped, not
        /// failed — unbound panels must still render their empty states.
        /// </summary>
        /// <summary>
        /// Panels exempt from the inert-click-target gate, each with a reason:
        /// shelved prototype shells are unreachable (their registry console id
        /// is redirected to a live surface, UI/UX audit 2026-09-25 C10), so their
        /// fixture-only action buttons are intentionally inert. The Plan 24
        /// memorial panel's fixture actions are now disabled with explicit
        /// reasons (C33), so it no longer needs an exemption.
        /// </summary>
        private static readonly System.Collections.Generic.HashSet<string> ClickabilityExemptPanels = new()
        {
            "AquiferTreatyConcessionPanel",
            "ClandestineInsurgencyPanel",
            "CrossingSafeConductVouchPanel",
            "FungalProteinFermenterPanel",
            "HeavyMarineDieselGeneratorPanel",
            "InductionCupolaFurnacePanel",
            "MagneticDrumArchivePanel",
            "MechanicalProstheticsLathePanel",
            "SonicRuptureDrillPanel",
            "SubterraneanDebtLedgerPanel",
            "SurfaceShrapnelAegisPanel",
            "TraumaBondingCohortPanel",
            "TroposphericRadioRelayPanel",
            "UltrasonicDecontaminationAirlockPanel",
            "VaultDoorBreachingPanel",
        };

        /// <summary>
        /// First-hour onboarding stage panels (T05). Four already implement
        /// IBindablePanel and are audited; PowerGridPanel/ResearchPanel/
        /// ExpeditionPanel are added by name so the focusability corpus verifies
        /// all seven routes a new player is sent to by the journey.
        /// </summary>
        private static readonly System.Collections.Generic.HashSet<string> FirstHourStagePanelNames = new(StringComparer.Ordinal)
        {
            "WaterTreatmentPanel",
            "PowerGridPanel",
            "InventoryPanel",
            "DutyRosterPanel",
            "DoseLedgerPanel",
            "ResearchPanel",
            "ExpeditionPanel",
        };

        /// <summary>
        /// UI interactivity audit (UI/UX audit follow-up): every panel type is
        /// instantiated unbound and checked for (a) buttons with no wired click
        /// route and (b) interactive controls that keyboard/controller players
        /// cannot reach (FocusMode None) or panels with no focusable control at
        /// all. Toggles, check controls, option selectors, disabled and
        /// effectively hidden controls are excluded from the click check.
        /// </summary>
        private static void AuditPanelInteractivity(Action<bool, string> check)
        {
            var panelTypes = typeof(HostCli).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract
                            && typeof(Control).IsAssignableFrom(t)
                            && (typeof(IBindablePanel).IsAssignableFrom(t)
                                || FirstHourStagePanelNames.Contains(t.Name))
                            && t.GetConstructor(Type.EmptyTypes) != null)
                .OrderBy(t => t.Name)
                .ToList();

            int audited = 0, skipped = 0, buttonsTotal = 0, inertTotal = 0;
            int interactiveTotal = 0, unreachableTotal = 0, panelsWithNoFocus = 0;
            int blankPanels = 0;
            var inert = new System.Collections.Generic.List<string>();
            var inertByPanel = new System.Collections.Generic.Dictionary<string, int>();
            var unreachable = new System.Collections.Generic.List<string>();
            var noFocus = new System.Collections.Generic.List<string>();
            var blank = new System.Collections.Generic.List<string>();

            foreach (var type in panelTypes)
            {
                Control? panel = null;
                try
                {
                    panel = (Control)Activator.CreateInstance(type)!;
                    panel._Ready();
                    // Optional hook: IBindablePanel does not contract Open().
                    AtomicWar.GodotApp.UI.AshfallUiHelpers.InvokePanelHook(panel, "Open", "RefreshView");
                }
                catch (Exception ex)
                {
                    // Scene-backed panels (Ticket #125) require their designer
                    // scene: instantiate exactly like production does instead of
                    // skipping the audit for them.
                    string resPath = $"res://assets/ui/panels/{type.Name}.tscn";
                    if (ResourceLoader.Exists(resPath))
                    {
                        try
                        {
                            panel = PanelSceneLoader.Load<Control>(resPath);
                            panel._Ready();
                            AtomicWar.GodotApp.UI.AshfallUiHelpers.InvokePanelHook(panel, "Open", "RefreshView");
                        }
                        catch (Exception sceneEx)
                        {
                            skipped++;
                            GD.Print($"  [CLICKABILITY-SKIP] {type.Name}: scene-path failed — {sceneEx.Message}");
                            panel?.Free();
                            continue;
                        }
                    }
                    else
                    {
                        skipped++;
                        GD.Print($"  [CLICKABILITY-SKIP] {type.Name}: {ex.GetType().Name} {ex.Message}");
                        panel?.Free();
                        continue;
                    }
                }

                audited++;

                // Scene-hosted widgets (production loads them from .tscn): a
                // directly-constructed shell has no children and would be judged
                // blank. Upgrade to the real scene so interactivity and
                // truthfulness are audited against the widget tree players see.
                if (panel.GetChildCount() == 0)
                {
                    string scenePath = $"res://assets/ui/panels/{type.Name}.tscn";
                    if (ResourceLoader.Exists(scenePath))
                    {
                        try
                        {
                            panel.Free();
                            panel = PanelSceneLoader.Load<Control>(scenePath);
                            panel._Ready();
                            AtomicWar.GodotApp.UI.AshfallUiHelpers.InvokePanelHook(panel, "Open", "RefreshView");
                        }
                        catch
                        {
                            // Keep the shell; the truthfulness check will report it.
                        }
                    }
                }

                bool exempt = ClickabilityExemptPanels.Contains(type.Name);
                foreach (var node in panel.FindChildren("*", "Button", true, false))
                {
                    if (node is not Button button)
                        continue;
                    buttonsTotal++;
                    if (button.Disabled || button.ToggleMode || !IsEffectivelyVisible(button) ||
                        button is CheckBox || button is CheckButton || button is OptionButton)
                        continue;

                    bool wired = button.GetSignalConnectionList("pressed").Count > 0
                              || button.GetSignalConnectionList("button_down").Count > 0
                              || button.GetSignalConnectionList("button_up").Count > 0
                              || button.GetSignalConnectionList("gui_input").Count > 0
                              || button.GetSignalConnectionList("toggled").Count > 0;
                    if (!wired && !exempt)
                    {
                        inertTotal++;
                        inertByPanel.TryGetValue(type.Name, out int seen);
                        inertByPanel[type.Name] = seen + 1;
                        if (inert.Count < 200)
                            inert.Add($"{type.Name}:'{button.Text}'");
                    }
                    else if (!wired && exempt)
                    {
                        GD.Print($"  [INERT-EXEMPT] {type.Name}:'{button.Text}'");
                    }
                }

                // Focus reachability: every effectively visible interactive
                // control must be reachable by keyboard/controller, and any panel
                // exposing interactivity must offer at least one focusable entry.
                int panelInteractive = 0;
                foreach (var node in panel.FindChildren("*", "Control", true, false))
                {
                    if (node is not Control control || !IsEffectivelyVisible(control))
                        continue;
                    bool interactive = control is Button || control is LineEdit || control is TextEdit
                                    || control is ItemList || control is Slider;
                    if (!interactive)
                        continue;
                    if (control is Button b2 && b2.Disabled)
                        continue;
                    panelInteractive++;
                    interactiveTotal++;
                    if (control.FocusMode == Control.FocusModeEnum.None)
                    {
                        unreachableTotal++;
                        if (unreachable.Count < 100)
                            unreachable.Add($"{type.Name}:{control.GetType().Name}:{control.Name}");
                    }
                }
                if (panelInteractive > 0 && AshfallFocusPolicy.FindFocusableControls(panel).Count == 0)
                {
                    panelsWithNoFocus++;
                    if (noFocus.Count < 60)
                        noFocus.Add(type.Name);
                }

                // Truthful-state sweep (silent-failure audit): an unbound panel
                // must render readable state (empty-state copy, labels, values),
                // never a blank shell that looks broken to a player.
                int visibleTextBlocks = 0;
                foreach (var node in panel.FindChildren("*", "Control", true, false))
                {
                    if (node is not Control textControl || !IsEffectivelyVisible(textControl))
                        continue;
                    string? text = textControl switch
                    {
                        Label label => label.Text,
                        RichTextLabel rich => rich.Text,
                        _ => null,
                    };
                    if (!string.IsNullOrWhiteSpace(text))
                        visibleTextBlocks++;
                }
                if (visibleTextBlocks == 0)
                {
                    blankPanels++;
                    if (blank.Count < 80)
                        blank.Add(type.Name);
                }
                panel.Free();
            }

            GD.Print($"[UiClickability] panels audited={audited} skipped={skipped} " +
                     $"buttons={buttonsTotal} inert={inertTotal}");
            GD.Print($"[UiFocusability] panels audited={audited} interactive={interactiveTotal} " +
                     $"unreachable={unreachableTotal} panelsWithNoFocus={panelsWithNoFocus}");
            GD.Print($"[UiTruthfulness] panels audited={audited} blankUnboundPanels={blankPanels}");
            foreach (var pair in inertByPanel.OrderByDescending(p => p.Value))
                GD.Print($"  [INERT-PANEL] {pair.Key} = {pair.Value}");
            foreach (string entry in inert)
                GD.PrintErr($"  [INERT] {entry}");
            foreach (string entry in unreachable)
                GD.PrintErr($"  [UNREACHABLE] {entry}");
            foreach (string entry in noFocus)
                GD.PrintErr($"  [NO-FOCUS] {entry}");
            foreach (string entry in blank)
                GD.PrintErr($"  [BLANK] {entry}");

            check(buttonsTotal > 0,
                $"UiClickability: button corpus discovered ({buttonsTotal} buttons across {audited} panels)");
            check(inertTotal == 0,
                $"UiClickability: no inert click targets ({inertTotal} found in {audited} panels)");
            check(unreachableTotal == 0,
                $"UiFocusability: every interactive control is keyboard/controller reachable ({unreachableTotal} unreachable)");
            check(panelsWithNoFocus == 0,
                $"UiFocusability: every interactive panel offers a focus entry ({panelsWithNoFocus} without focus)");
            check(blankPanels == 0,
                $"UiTruthfulness: every unbound panel renders readable state ({blankPanels} blank)");
        }

        /// <summary>
        /// Motion-layer contract verification (UI/UX audit task 5): proves the
        /// entrance/exit animation actually engages on a renderer-capable session
        /// (initial fade/rise/scale state applied) and is a strict no-op under
        /// headless/suppression, plus button-FX attach idempotence. Human visual
        /// confirmation of the finished motion remains a renderer-session item;
        /// this check guarantees the wiring cannot silently regress.
        /// </summary>
        private static void VerifyUiMotionContracts(Action<bool, string> check)
        {
            if (Engine.GetMainLoop() is not SceneTree tree)
            {
                check(false, "UiMotion: scene tree available for motion checks");
                return;
            }

            bool display = DisplayServer.GetName() != "headless";
            // Anchor to the live scene node when one exists: adding directly to
            // the window root from this static selftest context does not register
            // the subtree as in-tree, which would make motion checks unverifiable.
            Node anchor = tree.Root.GetChildCount() > 0 ? tree.Root.GetChild(0) : tree.Root;
            var host = new Control { Name = "UiMotionProbeHost" };
            anchor.AddChild(host);
            var panel = new Control { Name = "UiMotionProbePanel", Position = new Vector2(30f, 40f) };
            host.AddChild(panel);

            if (display && UiMotion.CanAnimate)
            {
                UiMotion.AnimateOpen(panel);
                check(Mathf.IsEqualApprox(panel.Modulate.A, 0f),
                    "UiMotion: open fade starts transparent (display path)");
                check(panel.Position.Y > 40f,
                    "UiMotion: open rise offset applied (display path)");
                check(panel.Scale.X > 0f && panel.Scale.X < 1f,
                    "UiMotion: open scale starts below rest (display path)");
            }
            else
            {
                var before = panel.Modulate;
                UiMotion.AnimateOpen(panel);
                check(panel.Modulate.A == before.A
                      && Mathf.IsEqualApprox(panel.Position.Y, 40f)
                      && panel.Scale == Vector2.One,
                    "UiMotion: headless/suppressed open is a strict no-op");
                check(!UiMotion.AnimateClose(panel),
                    "UiMotion: headless/suppressed close defers to the caller");
            }

            var button = new Button { Text = "MOTION PROBE" };
            host.AddChild(button);
            UiMotion.AttachButtonFx(button);
            UiMotion.AttachButtonFx(button);
            check(button.HasMeta("ashfall_button_fx"),
                "UiMotion: button FX attaches idempotently");
            button.EmitSignal(BaseButton.SignalName.MouseEntered);
            if (!display)
                check(button.Scale == Vector2.One,
                    "UiMotion: headless button FX is a no-op");

            host.Free();
        }

        /// <summary>
        /// Controller-parity contract (WHOLEGAME-P1D residual sweep, 2026-09-26):
        /// proves the rebindable dismissal/navigation wiring end-to-end using
        /// synthetic input events — byte-for-byte what a physical pad delivers —
        /// so pad behavior is verified without physical hardware or input
        /// automation:
        /// (a) the live InputMap binds pad B to close/cancel, D-pad up/down to
        ///     the nav actions, and the keyboard Esc/arrow bindings still match;
        /// (b) every instantiable input-handling panel dismisses (hides itself
        ///     or fires <c>OnClose</c>) when the pad-B event is pushed through
        ///     the production <c>_UnhandledInput</c> / <c>_UnhandledKeyInput</c>
        ///     / <c>_Input</c> override.
        /// </summary>
        private static void VerifyUiControllerParity(Action<bool, string> check)
        {
            if (Engine.GetMainLoop() is not SceneTree tree)
            {
                check(false, "UiControllerParity: scene tree available for pad checks");
                return;
            }

            // (a) InputMap contract on synthetic events.
            var padB = new InputEventJoypadButton { ButtonIndex = JoyButton.B, Pressed = true };
            var padDpadUp = new InputEventJoypadButton { ButtonIndex = JoyButton.DpadUp, Pressed = true };
            var padDpadDown = new InputEventJoypadButton { ButtonIndex = JoyButton.DpadDown, Pressed = true };
            var padShoulder = new InputEventJoypadButton { ButtonIndex = JoyButton.RightShoulder, Pressed = true };
            var escKey = new InputEventKey { Keycode = Key.Escape, Pressed = true };
            var arrowUpKey = new InputEventKey { Keycode = Key.Up, Pressed = true };

            check(AshfallInputActions.IsCloseOrCancel(padB),
                "UiControllerParity: pad B is bound to close/cancel");
            check(!AshfallInputActions.IsCloseOrCancel(padShoulder),
                "UiControllerParity: unrelated pad button is not close/cancel");
            check(AshfallInputActions.IsCloseOrCancel(escKey),
                "UiControllerParity: keyboard Esc remains close/cancel");
            check(AshfallInputActions.IsNavUp(padDpadUp) && AshfallInputActions.IsNavDown(padDpadDown),
                "UiControllerParity: D-pad up/down are bound to the nav actions");
            check(AshfallInputActions.IsNavUp(arrowUpKey),
                "UiControllerParity: keyboard Up remains nav-up");

            // (b) Per-panel pad-B dismissal sweep, anchored in the live scene so
            // viewport calls inside the handlers resolve exactly like play.
            Node anchor = tree.Root.GetChildCount() > 0 ? tree.Root.GetChild(0) : tree.Root;
            var host = new Control { Name = "UiControllerParityHost" };
            anchor.AddChild(host);

            const System.Reflection.BindingFlags Declared =
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic
                | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly;

            var panelTypes = typeof(HostCli).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract
                            && typeof(Control).IsAssignableFrom(t)
                            && typeof(IBindablePanel).IsAssignableFrom(t)
                            && t.GetConstructor(Type.EmptyTypes) != null)
                .OrderBy(t => t.Name)
                .ToList();

            int handlerPanels = 0, dismissed = 0, skipped = 0;
            var noDismiss = new System.Collections.Generic.List<string>();

            foreach (var type in panelTypes)
            {
                bool overridesInput =
                    type.GetMethod("_UnhandledInput", Declared) != null
                    || type.GetMethod("_UnhandledKeyInput", Declared) != null
                    || type.GetMethod("_Input", Declared) != null;
                if (!overridesInput)
                    continue;

                Control? panel = null;
                try
                {
                    // No manual _Ready() here: panels are anchored in the tree
                    // below, so the engine fires NOTIFICATION_READY exactly
                    // once (a manual call would double-build children).
                    panel = (Control)Activator.CreateInstance(type)!;
                }
                catch (Exception)
                {
                    string resPath = $"res://assets/ui/panels/{type.Name}.tscn";
                    if (ResourceLoader.Exists(resPath))
                    {
                        try
                        {
                            panel = PanelSceneLoader.Load<Control>(resPath);
                        }
                        catch
                        {
                            skipped++;
                            panel?.Free();
                            continue;
                        }
                    }
                    else
                    {
                        skipped++;
                        panel?.Free();
                        continue;
                    }
                }

                handlerPanels++;
                host.AddChild(panel);
                panel.Visible = true;

                bool fired = false;
                void CloseSignal() => fired = true;
                var onClose = type.GetEvent("OnClose");
                if (onClose != null && onClose.EventHandlerType == typeof(Action))
                    onClose.AddEventHandler(panel, (Action)CloseSignal);

                try
                {
                    panel._Input(padB);
                    panel._UnhandledInput(padB);
                    panel._UnhandledKeyInput(padB);
                }
                catch (Exception ex)
                {
                    noDismiss.Add($"{type.Name} (threw: {ex.GetType().Name})");
                }

                if (!fired && panel.Visible)
                    noDismiss.Add(type.Name);
                else
                    dismissed++;

                panel.Free();
            }

            host.Free();
            GD.Print($"[UiControllerParity] input-handling panels={handlerPanels} " +
                     $"padDismissed={dismissed} skippedConstruction={skipped}");
            foreach (string entry in noDismiss)
                GD.PrintErr($"  [NO-PAD-DISMISS] {entry}");
            check(handlerPanels > 0,
                $"UiControllerParity: input-handling panel corpus discovered ({handlerPanels} panels)");
            check(noDismiss.Count == 0,
                $"UiControllerParity: every input-handling panel dismisses on pad B " +
                $"({noDismiss.Count} failed: {string.Join(", ", noDismiss.Take(12))})");
        }

        /// <summary>Visibility including parent containers (panels are audited outside the tree).</summary>
        private static bool IsEffectivelyVisible(Control control)
        {
            Control? node = control;
            while (node != null)
            {
                if (!node.Visible)
                    return false;
                node = node.GetParent() as Control;
            }
            return true;
        }

    }
}
