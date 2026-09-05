using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Centralized UI Focus Policy (Plan 80 / Task B21).
    /// Provides deterministic initial focus, modal focus trapping, visible focus styling,
    /// and focus restoration on overlay dismissal.
    /// </summary>
    public static class AshfallFocusPolicy
    {
        private static StyleBoxFlat? _cachedFocusBox;

        /// <summary>
        /// Creates or returns the canonical high-contrast focus indicator stylebox
        /// with a 2px Hot Amber border and brutalist sharp corners.
        /// </summary>
        public static StyleBoxFlat MakeFocusVisibleStyleBox()
        {
            if (_cachedFocusBox != null)
                return _cachedFocusBox;

            _cachedFocusBox = new StyleBoxFlat
            {
                BgColor = new Color(DesignTheme.Hot.r, DesignTheme.Hot.g, DesignTheme.Hot.b, 0.08f),
                BorderColor = AshfallUiHelpers.ToColor(DesignTheme.Hot),
                DrawCenter = true,
            };
            _cachedFocusBox.SetBorderWidthAll(2);
            _cachedFocusBox.SetCornerRadiusAll(0);
            return _cachedFocusBox;
        }

        /// <summary>
        /// Applies the visible focus style and sets FocusMode to All on the given control.
        /// </summary>
        public static void ApplyFocusVisibleStyle(Control control)
        {
            if (control == null || !GodotObject.IsInstanceValid(control))
                return;

            control.FocusMode = Control.FocusModeEnum.All;
            var sb = MakeFocusVisibleStyleBox();
            control.AddThemeStyleboxOverride("focus", sb);
        }

        /// <summary>
        /// Gathers all active, visible focusable controls within the given root container in tree order.
        /// </summary>
        public static List<Control> FindFocusableControls(Control root)
        {
            var result = new List<Control>();
            if (root == null || !GodotObject.IsInstanceValid(root))
                return result;

            CollectFocusablesRecursive(root, result);
            return result;
        }

        private static void CollectFocusablesRecursive(Control current, List<Control> result)
        {
            if (current == null || !GodotObject.IsInstanceValid(current) || !current.Visible)
                return;

            if (current.FocusMode != Control.FocusModeEnum.None &&
                (current is Button || current is LineEdit || current is TextEdit ||
                 current is OptionButton || current is CheckButton || current is ItemList ||
                 current is AshfallDataGrid || current is Slider))
            {
                if (current is Button btn && btn.Disabled)
                {
                    // Disabled buttons don't receive focus navigation
                }
                else
                {
                    result.Add(current);
                }
            }

            foreach (var child in current.GetChildren())
            {
                if (child is Control ctrlChild)
                {
                    CollectFocusablesRecursive(ctrlChild, result);
                }
            }
        }

        /// <summary>
        /// Finds the first interactive focusable child control within root.
        /// </summary>
        public static Control? FindFirstFocusable(Control root)
        {
            var list = FindFocusableControls(root);
            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// Deterministically focuses the overlay upon opening.
        /// If initial is provided and valid, it is focused. Otherwise, the first focusable control is focused.
        /// Records opener on root for subsequent restoration.
        /// </summary>
        public static void OpenWithFocus(Control root, Control? initial = null, Control? opener = null)
        {
            if (root == null || !GodotObject.IsInstanceValid(root))
                return;

            if (opener != null && GodotObject.IsInstanceValid(opener))
            {
                root.SetMeta("_ashfall_focus_opener", opener);
            }

            Control? target = initial;
            if (target == null || !GodotObject.IsInstanceValid(target) || !target.Visible || !target.IsInsideTree())
            {
                target = FindFirstFocusable(root);
            }

            if (target != null && GodotObject.IsInstanceValid(target) && target.IsInsideTree())
            {
                target.CallDeferred(Control.MethodName.GrabFocus);
            }
        }

        /// <summary>
        /// Traps keyboard focus cycling (Tab / Shift+Tab) inside root so focus cannot leak to background panels.
        /// Returns true if the event was trapped and handled.
        /// </summary>
        public static bool TrapFocus(Control root, InputEvent @event)
        {
            if (root == null || !GodotObject.IsInstanceValid(root) || !root.Visible)
                return false;

            if (@event is InputEventKey key && key.Pressed && !key.Echo)
            {
                if (key.Keycode == Key.Tab)
                {
                    var focusables = FindFocusableControls(root);
                    if (focusables.Count == 0)
                        return false;

                    var currentFocus = root.GetViewport()?.GuiGetFocusOwner();
                    int currentIndex = currentFocus != null ? focusables.IndexOf(currentFocus) : -1;

                    if (key.ShiftPressed)
                    {
                        // Shift+Tab: backwards cycle
                        int prevIndex = currentIndex <= 0 ? focusables.Count - 1 : currentIndex - 1;
                        focusables[prevIndex].GrabFocus();
                        root.GetViewport()?.SetInputAsHandled();
                        return true;
                    }
                    else
                    {
                        // Tab: forward cycle
                        int nextIndex = currentIndex < 0 || currentIndex >= focusables.Count - 1 ? 0 : currentIndex + 1;
                        focusables[nextIndex].GrabFocus();
                        root.GetViewport()?.SetInputAsHandled();
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Restores focus to the opener control that initiated this modal or panel.
        /// </summary>
        public static void RestoreFocus(Control? opener)
        {
            if (opener != null && GodotObject.IsInstanceValid(opener) && opener.IsInsideTree() && opener.Visible)
            {
                opener.CallDeferred(Control.MethodName.GrabFocus);
            }
        }

        /// <summary>
        /// Restores focus using the recorded opener metadata on root, if present.
        /// </summary>
        public static void RestoreFocusFromRoot(Control root)
        {
            if (root == null || !GodotObject.IsInstanceValid(root))
                return;

            if (root.HasMeta("_ashfall_focus_opener"))
            {
                var val = root.GetMeta("_ashfall_focus_opener");
                if (val.VariantType == Variant.Type.Object)
                {
                    var opener = val.As<Control>();
                    RestoreFocus(opener);
                }
            }
        }
    }
}
