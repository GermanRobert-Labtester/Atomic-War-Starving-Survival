#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion IV — Quest: The Iron Worm. Desperation creates religion.
    /// Religion creates blind spots. Blind spots get you killed.
    ///
    /// The Lightless believe the rumble of the pre-war trains will return
    /// if they stack enough scrap metal on the tracks. They built a shrine.
    /// They are waiting for the "Iron Worm" to take them to the coast.
    ///
    /// Stage 1: The Rumble (Day 30+) — Discover the shrine
    /// Stage 2: The Offering (Day 35+) — Need pump room parts, choose violence or deception
    /// Stage 3: The Collapse (Day 40+) — If deceived, tunnel collapses
    /// Stage 4: The Survivor (Day 45+) — A blind Lightless child appears at your hatch
    /// </summary>
    public class Quest_TheIronWorm
    {
        public const string QuestId = "quest_the_iron_worm";

        // ── Stage thresholds ──────────────────────────────────────────
        public const int Stage1_TriggerDay = 30;
        public const int Stage2_TriggerDay = 35;
        public const int Stage3_TriggerDay = 40;
        public const int Stage4_TriggerDay = 45;
        public const int CollapseDelayDays = 4;

        // ── Stage ids ─────────────────────────────────────────────────
        public const string Stage1_Id = "stage_1_the_rumble";
        public const string Stage2_Id = "stage_2_the_offering";
        public const string Stage3_Id = "stage_3_the_collapse";
        public const string Stage4_Id = "stage_4_the_survivor";

        // ── Choice ids ────────────────────────────────────────────────
        public const string Choice_Violence = "choice_violence";
        public const string Choice_Deception = "choice_deception";
        public const string Choice_OpenHatch = "choice_open_hatch";
        public const string Choice_LeaveSupplies = "choice_leave_supplies";

        // ── Item/loot ids ─────────────────────────────────────────────
        public const string Loot_GeneratorAlternator = "generator_alternator";
        public const string Loot_CopperWire10m = "copper_wire_10m_of_10m";
        public const string Item_FamilyPhotograph = "family_photograph";
        public const string Item_ScrapMetal = "scrap_metal";

        // ── Affliction ids ────────────────────────────────────────────
        public const string MentalBreakId = "mental_break_grief_cascade";

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnStageReached;
        public event Action<string, string> OnChoiceMade;         // stageId, choiceId
        public event Action<string> OnQuestCompleted;
        public event Action<string> OnChildAppearsAtHatch;
        public event Action<int> OnLightlessKilled;
        public event Action<string> OnMentalBreakTriggered;

        private string _currentStage;
        private string _stage2Choice;
        private bool _violenceUsed;
        private bool _deceptionUsed;
        private bool _tunnelCollapsed;
        private bool _childAppeared;
        private bool _childAccepted;
        private bool _suppliesLeft;
        private int _lightlessKilledCount;
        private int _collapseDay;

        public string CurrentStage => _currentStage;
        public bool IsActive => !string.IsNullOrEmpty(_currentStage);
        public bool IsViolenceUsed => _violenceUsed;
        public bool IsDeceptionUsed => _deceptionUsed;
        public bool IsTunnelCollapsed => _tunnelCollapsed;
        public bool IsChildAccepted => _childAccepted;
        public int LightlessKilled => _lightlessKilledCount;

        // ── Stage progression ─────────────────────────────────────────

        /// <summary>
        /// Check if the quest should advance. Called daily by the host.
        /// </summary>
        public string CheckStageAdvancement(int currentDay)
        {
            if (_currentStage == null && currentDay >= Stage1_TriggerDay)
            {
                _currentStage = Stage1_Id;
                OnStageReached?.Invoke(Stage1_Id);
                return Stage1_Id;
            }

            if (_currentStage == Stage1_Id && currentDay >= Stage2_TriggerDay)
            {
                _currentStage = Stage2_Id;
                OnStageReached?.Invoke(Stage2_Id);
                return Stage2_Id;
            }

            if (_currentStage == Stage2_Id && !string.IsNullOrEmpty(_stage2Choice)
                && currentDay >= Stage3_TriggerDay)
            {
                if (_deceptionUsed)
                {
                    _currentStage = Stage3_Id;
                    _collapseDay = currentDay + CollapseDelayDays;
                    OnStageReached?.Invoke(Stage3_Id);
                    return Stage3_Id;
                }
                // Violence path: no collapse, quest resolves differently
                _currentStage = Stage4_Id;
                OnStageReached?.Invoke(Stage4_Id);
                return Stage4_Id;
            }

            if (_currentStage == Stage3_Id && currentDay >= _collapseDay)
            {
                _tunnelCollapsed = true;
                _lightlessKilledCount = 80;
                OnLightlessKilled?.Invoke(_lightlessKilledCount);
                _currentStage = Stage4_Id;
                OnStageReached?.Invoke(Stage4_Id);
                return Stage4_Id;
            }

            if (_currentStage == Stage4_Id && !_childAppeared && currentDay >= Stage4_TriggerDay)
            {
                _childAppeared = true;
                OnChildAppearsAtHatch?.Invoke("lightless_child");
                return Stage4_Id;
            }

            return null;
        }

        // ── Stage 2: The Offering ─────────────────────────────────────

        /// <summary>
        /// Make a choice in Stage 2: violence or deception.
        /// Violence: Use flare_white to blind the Lightless and take the parts.
        /// Deception: Broadcast a train horn through the tunnel PA system.
        /// </summary>
        public bool MakeStage2Choice(string choiceId)
        {
            if (_currentStage != Stage2_Id) return false;

            _stage2Choice = choiceId;

            if (choiceId == Choice_Violence)
            {
                _violenceUsed = true;
                _lightlessKilledCount = 12;
                OnLightlessKilled?.Invoke(_lightlessKilledCount);
                OnChoiceMade?.Invoke(Stage2_Id, Choice_Violence);
            }
            else if (choiceId == Choice_Deception)
            {
                _deceptionUsed = true;
                OnChoiceMade?.Invoke(Stage2_Id, Choice_Deception);
            }

            return true;
        }

        // ── Stage 4: The Survivor ─────────────────────────────────────

        /// <summary>
        /// Make the final choice: open the hatch and take the child,
        /// or leave supplies outside and close the door.
        /// </summary>
        public bool MakeStage4Choice(string choiceId)
        {
            if (_currentStage != Stage4_Id || !_childAppeared) return false;

            if (choiceId == Choice_OpenHatch)
            {
                _childAccepted = true;
                OnChoiceMade?.Invoke(Stage4_Id, Choice_OpenHatch);
            }
            else if (choiceId == Choice_LeaveSupplies)
            {
                _suppliesLeft = true;
                OnChoiceMade?.Invoke(Stage4_Id, Choice_LeaveSupplies);
            }

            OnQuestCompleted?.Invoke(QuestId);
            return true;
        }

        // ── Moral Chronicle entries ───────────────────────────────────

        public string GetMoralChronicleEntry()
        {
            if (_violenceUsed)
                return "Day 35. We blinded them with flares and took the parts. " +
                       "Twelve malnourished, blind civilians. The pump room was empty.";

            if (_deceptionUsed && _tunnelCollapsed)
                return "Day 40. The tunnel collapsed. Eighty Lightless crushed. " +
                       "The alternator we stole is the only reason the lights are still on. " +
                       "Every time they flicker, I hear the hum.";

            if (_childAccepted)
                return "Day 45. We opened the hatch. The child does not speak. " +
                       "They sort electronic scrap with terrifying speed. " +
                       "They never look at the lights.";

            if (_suppliesLeft)
                return "Day 45. You fed the ghost. You did not open the door. " +
                       "The door is heavy. The door is safe.";

            return "The Iron Worm never came. The tracks are empty.";
        }

        /// <summary>Get the final frame description based on quest outcome.</summary>
        public string GetFinalFrameDescription()
        {
            if (_childAccepted)
                return "The table has a single, bright red tomato on it. " +
                       "The lighting is harsh, artificial, and cold. " +
                       "A child sits in the corner, sorting scrap metal by touch.";
            if (_suppliesLeft)
                return "The table is empty, save for a single, unlit candle. " +
                       "Outside the hatch, the supplies are gone. " +
                       "The ash filled the footprints in seconds.";
            return "The bunker is empty. The wind writes the rest.";
        }

        // ── Save / Load ───────────────────────────────────────────────

        public IronWormSave CaptureState()
        {
            return new IronWormSave
            {
                CurrentStage = _currentStage,
                Stage2Choice = _stage2Choice,
                ViolenceUsed = _violenceUsed,
                DeceptionUsed = _deceptionUsed,
                TunnelCollapsed = _tunnelCollapsed,
                ChildAppeared = _childAppeared,
                ChildAccepted = _childAccepted,
                SuppliesLeft = _suppliesLeft,
                LightlessKilledCount = _lightlessKilledCount,
                CollapseDay = _collapseDay
            };
        }

        public void RestoreState(IronWormSave save)
        {
            _currentStage = null;
            _stage2Choice = null;
            _violenceUsed = false;
            _deceptionUsed = false;
            _tunnelCollapsed = false;
            _childAppeared = false;
            _childAccepted = false;
            _suppliesLeft = false;
            _lightlessKilledCount = 0;
            _collapseDay = 0;
            if (save == null) return;
            _currentStage = save.CurrentStage;
            _stage2Choice = save.Stage2Choice;
            _violenceUsed = save.ViolenceUsed;
            _deceptionUsed = save.DeceptionUsed;
            _tunnelCollapsed = save.TunnelCollapsed;
            _childAppeared = save.ChildAppeared;
            _childAccepted = save.ChildAccepted;
            _suppliesLeft = save.SuppliesLeft;
            _lightlessKilledCount = save.LightlessKilledCount;
            _collapseDay = save.CollapseDay;
        }
    }

    [Serializable]
    public class IronWormSave
    {
        public string CurrentStage;
        public string Stage2Choice;
        public bool ViolenceUsed;
        public bool DeceptionUsed;
        public bool TunnelCollapsed;
        public bool ChildAppeared;
        public bool ChildAccepted;
        public bool SuppliesLeft;
        public int LightlessKilledCount;
        public int CollapseDay;
    }
}
