// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Spatial UI Focus Navigation (Plan 37 / Continuity Wave 5).
    /// Directs keyboard nav keys (arrow keys) and controller D-pad / stick input
    /// to move focus spatially across focusable controls, enforcing grid consistency.
    /// </summary>
    public static class AshfallFocusNavigator
    {
        private static double _stickRepeatCooldown;
        private const double StickRepeatCadenceSeconds = 0.15;

        /// <summary>
        /// Move focus within root in the given direction.
        /// Finds the nearest focusable whose center lies in the requested half-plane,
        /// preferring same-row/column band (grid rule).
        /// Returns true if focus was successfully moved.
        /// </summary>
        public static bool MoveDirection(Control root, Vector2I direction)
        {
            if (root == null || !GodotObject.IsInstanceValid(root) || !root.Visible)
                return false;

            var focusables = AshfallFocusPolicy.FindFocusableControls(root);
            if (focusables.Count == 0)
                return false;

            var currentFocus = root.GetViewport()?.GuiGetFocusOwner();
            if (currentFocus == null || !focusables.Contains(currentFocus))
            {
                focusables[0].GrabFocus();
                return true;
            }

            var currentRect = currentFocus.GetGlobalRect();
            var currentCenter = currentRect.GetCenter();

            Control? bestCandidate = null;
            float bestScore = float.MaxValue;

            foreach (var candidate in focusables)
            {
                if (candidate == currentFocus) continue;
                var candRect = candidate.GetGlobalRect();
                var candCenter = candRect.GetCenter();
                var delta = candCenter - currentCenter;

                // Check direction half-plane
                if (direction.X > 0 && delta.X <= 0) continue; // Right
                if (direction.X < 0 && delta.X >= 0) continue; // Left
                if (direction.Y > 0 && delta.Y <= 0) continue; // Down
                if (direction.Y < 0 && delta.Y >= 0) continue; // Up

                float primaryDist = MathF.Abs(direction.X != 0 ? delta.X : delta.Y);
                float secondaryDist = MathF.Abs(direction.X != 0 ? delta.Y : delta.X);

                // Grid rule: prefer candidates overlapping in perpendicular band
                bool perpendicularOverlap = direction.X != 0
                    ? (candRect.Position.Y < currentRect.End.Y && candRect.End.Y > currentRect.Position.Y)
                    : (candRect.Position.X < currentRect.End.X && candRect.End.X > currentRect.Position.X);

                float score = primaryDist + (perpendicularOverlap ? secondaryDist * 1.5f : secondaryDist * 5.0f);

                if (score < bestScore)
                {
                    bestScore = score;
                    bestCandidate = candidate;
                }
            }

            if (bestCandidate != null)
            {
                bestCandidate.GrabFocus();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Routes the directional navigation actions (ashfall_nav_up/down/left/right)
        /// to MoveDirection within the active focus scope root.
        /// Returns true when an action was handled.
        /// </summary>
        public static bool HandleNavInput(Control scopeRoot, InputEvent @event)
        {
            if (scopeRoot == null || !GodotObject.IsInstanceValid(scopeRoot) || !scopeRoot.Visible)
                return false;

            if (AshfallInputActions.IsNavUp(@event))
                return MoveDirection(scopeRoot, new Vector2I(0, -1));
            if (AshfallInputActions.IsNavDown(@event))
                return MoveDirection(scopeRoot, new Vector2I(0, 1));
            if (AshfallInputActions.IsNavLeft(@event))
                return MoveDirection(scopeRoot, new Vector2I(-1, 0));
            if (AshfallInputActions.IsNavRight(@event))
                return MoveDirection(scopeRoot, new Vector2I(1, 0));

            return false;
        }

        /// <summary>
        /// Converts held stick/dpad motion into repeated directional steps on a 150ms cadence.
        /// </summary>
        public static void TickStickRepeat(Control scopeRoot, double delta)
        {
            if (scopeRoot == null || !GodotObject.IsInstanceValid(scopeRoot) || !scopeRoot.Visible)
                return;

            _stickRepeatCooldown -= delta;
            if (_stickRepeatCooldown > 0)
                return;

            Vector2I dir = Vector2I.Zero;
            if (Input.IsActionPressed(AshfallInputActions.NavUp)) dir.Y -= 1;
            if (Input.IsActionPressed(AshfallInputActions.NavDown)) dir.Y += 1;
            if (Input.IsActionPressed(AshfallInputActions.NavLeft)) dir.X -= 1;
            if (Input.IsActionPressed(AshfallInputActions.NavRight)) dir.X += 1;

            if (dir != Vector2I.Zero)
            {
                MoveDirection(scopeRoot, dir);
                _stickRepeatCooldown = StickRepeatCadenceSeconds;
            }
        }
    }
}
