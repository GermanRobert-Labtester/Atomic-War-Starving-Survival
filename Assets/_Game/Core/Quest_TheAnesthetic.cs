using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion VII — Quest: The Anesthetic. The brutal logistics of mercy.
    /// You need to perform a life-saving surgery, but you have no anesthesia.
    /// The math says you must synthesize it. The synthesis requires a descent
    /// into the black market.
    ///
    /// Stage 1: The Fever — Mechanic needs amputation, no anesthesia
    /// Stage 2: The Poppy Fields — Infiltrate Warlord opium greenhouse
    /// Stage 3: The Synthesis & The Toll — Crude anesthetic, 40% heart-stop risk
    /// Stage 4: The Ghost in the Marrow — Narcotic withdrawal hallucinations
    /// </summary>
    public class Quest_TheAnesthetic
    {
        public const string QuestId = "quest_the_anesthetic";

        // ── Stage ids ─────────────────────────────────────────────────
        public const string Stage1_Id = "stage_1_the_fever";
        public const string Stage2_Id = "stage_2_the_poppy_fields";
        public const string Stage3_Id = "stage_3_the_synthesis";
        public const string Stage4_Id = "stage_4_the_ghost";

        // ── Choice ids ────────────────────────────────────────────────
        public const string Choice_AdministerAnesthetic = "choice_administer_anesthetic";
        public const string Choice_UseAlcoholLeather = "choice_use_alcohol_leather";

        // ── Item ids ──────────────────────────────────────────────────
        public const string Item_PoppyLatex = "item_poppy_latex";
        public const string Item_CrudeAnesthetic = "item_crude_anesthetic";
        public const string Item_OpiumRaw = "item_opium_raw";

        // ── Affliction ids ────────────────────────────────────────────
        public const string Affliction_Gangrene = "affliction_gangrene";
        public const string Affliction_NerveDamage = "affliction_nerve_damage";
        public const string Affliction_RadHallucinations = "affliction_rad_hallucinations";

        // ── Constants ─────────────────────────────────────────────────
        public const int Stage1_DaysToLive = 3;
        public const float AnestheticHeartStopChance = 0.40f;
        public const float AlcoholScreamAcousticVolume = 90f;
        public const int Stage4_RestraintDays = 5;

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnStageReached;
        public event Action<string, string> OnChoiceMade;
        public event Action<string> OnQuestCompleted;
        public event Action<string> OnMechanicSurvived;
        public event Action<string> OnMechanicHeartStopped;
        public event Action<string> OnScreamBroadcast;
        public event Action<string> OnNarcoticWithdrawal;
        public event Action<string> OnWaterPipeBreached;

        private string _currentStage;
        private string _mechanicId;
        private string _surgeonId;
        private string _chemistId;
        private int _stage1DeadlineDay;
        private bool _anestheticUsed;
        private bool _alcoholUsed;
        private bool _mechanicSurvived;
        private bool _heartStopped;
        private bool _screamBroadcast;
        private bool _withdrawalPhase;
        private int _restraintDaysRemaining;

        public string CurrentStage => _currentStage;
        public bool IsActive => !string.IsNullOrEmpty(_currentStage);
        public bool IsAnestheticUsed => _anestheticUsed;
        public bool DidMechanicSurvive => _mechanicSurvived;

        // ── Stage 1: The Fever ────────────────────────────────────────

        /// <summary>
        /// Trigger: Mechanic suffers compound fracture + gangrene.
        /// Without amputation in 3 days, he dies. Without anesthesia, the
        /// shock kills him on the table.
        /// </summary>
        public bool TriggerFever(string mechanicId, string surgeonId, string chemistId,
            int currentDay)
        {
            if (_currentStage != null) return false;
            _currentStage = Stage1_Id;
            _mechanicId = mechanicId;
            _surgeonId = surgeonId;
            _chemistId = chemistId;
            _stage1DeadlineDay = currentDay + Stage1_DaysToLive;
            OnStageReached?.Invoke(Stage1_Id);
            return true;
        }

        /// <summary>Check if the mechanic has died from untreated gangrene.</summary>
        public bool CheckDeadline(int currentDay)
        {
            return _currentStage == Stage1_Id && currentDay >= _stage1DeadlineDay;
        }

        // ── Stage 2: The Poppy Fields ─────────────────────────────────

        /// <summary>
        /// Advance to Stage 2: The Reporter intercepts a broadcast about
        /// Warlord opium greenhouse.
        /// </summary>
        public bool AdvanceToPoppyFields()
        {
            if (_currentStage != Stage1_Id) return false;
            _currentStage = Stage2_Id;
            OnStageReached?.Invoke(Stage2_Id);
            return true;
        }

        /// <summary>Secure the poppy latex from the greenhouse.</summary>
        public bool SecurePoppyLatex(string survivorId)
        {
            if (_currentStage != Stage2_Id) return false;
            OnStageReached?.Invoke(Stage3_Id);
            _currentStage = Stage3_Id;
            return true;
        }

        // ── Stage 3: The Synthesis & The Toll ─────────────────────────

        /// <summary>
        /// The chemist synthesizes crude anesthetic. 40% chance to stop
        /// the Mechanic's heart.
        /// </summary>
        public bool SynthesizeAnesthetic()
        {
            if (_currentStage != Stage3_Id) return false;
            return true; // Synthesis succeeds, choice remains
        }

        /// <summary>
        /// Make the choice: administer toxic anesthetic or use alcohol + leather.
        /// </summary>
        public bool MakeStage3Choice(string choiceId, System.Random rng)
        {
            if (_currentStage != Stage3_Id) return false;

            if (choiceId == Choice_AdministerAnesthetic)
            {
                _anestheticUsed = true;
                bool heartStop = rng.NextDouble() < AnestheticHeartStopChance;

                if (heartStop)
                {
                    _heartStopped = true;
                    _mechanicSurvived = false;
                    OnMechanicHeartStopped?.Invoke(_mechanicId);
                    OnChoiceMade?.Invoke(Stage3_Id, choiceId);
                    OnQuestCompleted?.Invoke(QuestId);
                    return true;
                }

                // Mechanic survives but with nerve damage
                _mechanicSurvived = true;
                _currentStage = Stage4_Id;
                OnMechanicSurvived?.Invoke(_mechanicId);
                OnStageReached?.Invoke(Stage4_Id);
            }
            else if (choiceId == Choice_UseAlcoholLeather)
            {
                _alcoholUsed = true;
                _mechanicSurvived = true;
                _screamBroadcast = true;
                OnScreamBroadcast?.Invoke(_mechanicId);
                OnChoiceMade?.Invoke(Stage3_Id, choiceId);
                OnQuestCompleted?.Invoke(QuestId);
            }

            return true;
        }

        // ── Stage 4: The Ghost in the Marrow ──────────────────────────

        /// <summary>
        /// The drug was cut with Warlord impurities. Narcotic withdrawal
        /// hallucinations. The Mechanic hears "roots" growing through concrete.
        /// </summary>
        public bool TriggerWithdrawalPhase()
        {
            if (_currentStage != Stage4_Id) return false;
            _withdrawalPhase = true;
            _restraintDaysRemaining = Stage4_RestraintDays;
            OnNarcoticWithdrawal?.Invoke(_mechanicId);
            return true;
        }

        /// <summary>
        /// Tick the restraint phase. Returns true if the Mechanic is restrained
        /// and treatment is ongoing. Returns false if he breaches a water pipe.
        /// </summary>
        public bool TickRestraint(float gameDays)
        {
            if (!_withdrawalPhase || _restraintDaysRemaining <= 0) return false;

            _restraintDaysRemaining -= Mathf.RoundToInt(gameDays);

            if (_restraintDaysRemaining <= 0)
            {
                _withdrawalPhase = false;
                OnQuestCompleted?.Invoke(QuestId);
                return true; // Treatment complete
            }

            // Random water pipe breach attempt
            if (UnityEngine.Random.value < 0.10f * gameDays)
            {
                OnWaterPipeBreached?.Invoke(_mechanicId);
                return false; // Pipe breached
            }

            return true;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public AnestheticSave CaptureState()
        {
            return new AnestheticSave
            {
                CurrentStage = _currentStage,
                MechanicId = _mechanicId,
                SurgeonId = _surgeonId,
                ChemistId = _chemistId,
                Stage1DeadlineDay = _stage1DeadlineDay,
                AnestheticUsed = _anestheticUsed,
                AlcoholUsed = _alcoholUsed,
                MechanicSurvived = _mechanicSurvived,
                HeartStopped = _heartStopped,
                ScreamBroadcast = _screamBroadcast,
                WithdrawalPhase = _withdrawalPhase,
                RestraintDaysRemaining = _restraintDaysRemaining
            };
        }

        public void RestoreState(AnestheticSave save)
        {
            _currentStage = null;
            _mechanicId = null;
            _surgeonId = null;
            _chemistId = null;
            _stage1DeadlineDay = 0;
            _anestheticUsed = false;
            _alcoholUsed = false;
            _mechanicSurvived = false;
            _heartStopped = false;
            _screamBroadcast = false;
            _withdrawalPhase = false;
            _restraintDaysRemaining = 0;
            if (save == null) return;
            _currentStage = save.CurrentStage;
            _mechanicId = save.MechanicId;
            _surgeonId = save.SurgeonId;
            _chemistId = save.ChemistId;
            _stage1DeadlineDay = save.Stage1DeadlineDay;
            _anestheticUsed = save.AnestheticUsed;
            _alcoholUsed = save.AlcoholUsed;
            _mechanicSurvived = save.MechanicSurvived;
            _heartStopped = save.HeartStopped;
            _screamBroadcast = save.ScreamBroadcast;
            _withdrawalPhase = save.WithdrawalPhase;
            _restraintDaysRemaining = save.RestraintDaysRemaining;
        }
    }

    [Serializable]
    public class AnestheticSave
    {
        public string CurrentStage;
        public string MechanicId;
        public string SurgeonId;
        public string ChemistId;
        public int Stage1DeadlineDay;
        public bool AnestheticUsed;
        public bool AlcoholUsed;
        public bool MechanicSurvived;
        public bool HeartStopped;
        public bool ScreamBroadcast;
        public bool WithdrawalPhase;
        public int RestraintDaysRemaining;
    }
}
