using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {


        /// <summary>Resync the pooled inventory icon strip from live stock.</summary>

        /// <summary>
        /// Push corpse / fire / coma / contaminated-food state into InternalHorrorHUD.
        /// Safe when systems are not yet constructed.
        /// </summary>

        /// <summary>Wire Internal Horror HUD action callbacks once.</summary>

        /// <summary>
        /// Inventory strip activate (click / Enter): corpse stacks open dispose UI.
        /// </summary>





        /// <summary>Close corpse dispose and/or fire panels (Esc).</summary>

        /// <summary>
        /// Apply layout-specific modifications to the shelter: rooms, starting
        /// modules, anomalies, and traits (Prompts #79-#84).
        /// </summary>
        /// <summary>
        /// Sprinkle the 10 quest location nodes onto the generated map (Prompts #85-#94).
        /// </summary>



        /// <summary>
        /// Mental-break sabotage: disable or degrade a random shelter module.
        /// Hosted in Core so Survivors does not reference Shelter.
        /// </summary>


        /// <summary>
        /// One-shot: stable RNGs + non-allocating callbacks for the hourly tick.
        /// Safe to call more than once (idempotent).
        /// </summary>

        /// <summary>Allocation-free weather name for PowerNetwork (no Enum.ToString).</summary>


        /// <summary>Record a resolved moral dilemma for the endgame tally.</summary>

        /// <summary>
        /// Audit C-1: per-hour clothing degradation tick. Drives
        /// <see cref="ClothingSystem.Tick"/> for each living survivor. The
        /// humidity source is the survivor's current room (if the
        /// atmosphere system reports a humidity value) or 0.5f default.
        /// </summary>



        /// <summary>
        /// Runtime item defs for tests / missing catalog entries (engine, parts, fuel).
        /// </summary>

        private void EndGame(string reason, string outcome)
        {
            // Legacy path — prefer VictoryProject triggers.
            IsGameOver = true;
            GameOverReason = reason;
            GameState.Phase = GamePhase.GameOver;
            Debug.Log($"[GameBootstrap] GAME OVER ({outcome}): {reason}");
        }

        public void PauseGame()
        {
            GameState.IsPaused = true;
            GameState.Phase = GamePhase.Paused;
        }

        public void ResumeGame()
        {
            GameState.IsPaused = false;
            GameState.Phase = GamePhase.Running;
        }

        /// <summary>Toggle fast-forward: 1x <-> 3x (keybind F). Simulation-scaled only; Unity's Time.timeScale is untouched.</summary>
        public void ToggleFastForward()
        {
            if (TimeSystem == null) return;
            TimeSystem.SetTimeScale(TimeSystem.TimeScale > 1.5f ? 1f : FastForwardScale);
        }

        /// <summary>Explicit simulation speed (clamped by TimeSystem). For UI buttons/tests.</summary>
        public void SetTimeScale(float scale)
        {
            TimeSystem?.SetTimeScale(scale);
        }

        public void SaveGame(string slotId = "quicksave")
        {
            SnapshotRadioHudToInterceptSystem();
            SaveSystem.Save(slotId);
            _diagnosticsOverlay?.NotifySave(slotId);
        }

        public void LoadGame(string slotId = "quicksave")
        {
            if (SaveSystem.Load(slotId))
            {
                // Restore endgame terminal state from VictoryProject if present.
                if (VictoryProject != null && VictoryProject.IsTerminal)
                {
                    IsGameOver = true;
                    GameOverReason = VictoryProject.TerminalReason;
                    if (GameState != null) GameState.Phase = GamePhase.GameOver;
                    if (VictoryProject.LastSummary != null)
                        PushEndgameSummaryToHud(VictoryProject.LastSummary);
                }
                else
                {
                    IsGameOver = false;
                    GameOverReason = null;
                    _hud?.EnsureEndgameSummary()?.Clear();
                }
                // Intercept log + open/unread/tuner restored — refresh HUD strip.
                SyncRadioInterceptHudFromLog();
                SyncJournalBookFromSystem();
                // Corpse counts / fire rooms / care urgency after atmosphere+inventory restore.
                RefreshInventoryStrip();
            }
        }
    }
}
