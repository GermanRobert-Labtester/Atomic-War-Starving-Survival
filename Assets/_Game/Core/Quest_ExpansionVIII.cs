using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion VIII — Quest: The Transit Pass (The Forger's Masterpiece).
    /// A piece of paper can stop a bullet, but the ink requires blood.
    ///
    /// Stage 1: The Checkpoint — Garrison blockade, heavy expedition casualties
    /// Stage 2: The Ink — Need stamp, indelible ink (blood-based), and O-Negative blood
    /// Stage 3: The Crossing — Deception check at the checkpoint
    /// </summary>
    public class Quest_TheTransitPass
    {
        public const string QuestId = "quest_the_transit_pass";

        public const string Stage1_Id = "stage_1_the_checkpoint";
        public const string Stage2_Id = "stage_2_the_ink";
        public const string Stage3_Id = "stage_3_the_crossing";

        public const string Item_StampMinistry = "stamp_ministry_official";
        public const string Item_InkIndelible = "ink_indelible";
        public const string Item_TransitPassForged = "transit_pass_forged";
        public const string Item_BloodBag = "blood_bag";

        public event Action<string> OnStageReached;
        public event Action<string, bool> OnCrossingResolved;
        public event Action<string> OnQuestCompleted;

        private string _currentStage;
        private string _forgerId;
        private bool _hasStamp;
        private bool _hasInk;
        private bool _hasBlood;
        private bool _crossingSuccess;
        private float _forgeryQuality;

        public string CurrentStage => _currentStage;
        public bool IsActive => !string.IsNullOrEmpty(_currentStage);

        public bool TriggerCheckpoint(string forgerId, int currentDay)
        {
            if (_currentStage != null) return false;
            _currentStage = Stage1_Id;
            _forgerId = forgerId;
            OnStageReached?.Invoke(Stage1_Id);
            return true;
        }

        public bool CollectInkMaterials(bool hasStamp, bool hasInk, bool hasBlood)
        {
            _hasStamp = hasStamp;
            _hasInk = hasInk;
            _hasBlood = hasBlood;
            if (_hasStamp && _hasInk && _hasBlood)
            {
                _currentStage = Stage2_Id;
                OnStageReached?.Invoke(Stage2_Id);
                return true;
            }
            return false;
        }

        public bool CraftPass(float intellectSkill)
        {
            if (_currentStage != Stage2_Id) return false;
            _forgeryQuality = Mathf.Clamp01(intellectSkill);
            _currentStage = Stage3_Id;
            OnStageReached?.Invoke(Stage3_Id);
            return true;
        }

        public bool ResolveCrossing(System.Random rng)
        {
            if (_currentStage != Stage3_Id) return false;

            float roll = (float)rng.NextDouble();
            _crossingSuccess = roll < _forgeryQuality;

            OnCrossingResolved?.Invoke(QuestId, _crossingSuccess);
            OnQuestCompleted?.Invoke(QuestId);
            return _crossingSuccess;
        }

        public TransitPassSave CaptureState()
        {
            return new TransitPassSave
            {
                CurrentStage = _currentStage,
                ForgerId = _forgerId,
                HasStamp = _hasStamp,
                HasInk = _hasInk,
                HasBlood = _hasBlood,
                CrossingSuccess = _crossingSuccess,
                ForgeryQuality = _forgeryQuality
            };
        }

        public void RestoreState(TransitPassSave save)
        {
            _currentStage = null;
            _forgerId = null;
            _hasStamp = false;
            _hasInk = false;
            _hasBlood = false;
            _crossingSuccess = false;
            _forgeryQuality = 0f;
            if (save == null) return;
            _currentStage = save.CurrentStage;
            _forgerId = save.ForgerId;
            _hasStamp = save.HasStamp;
            _hasInk = save.HasInk;
            _hasBlood = save.HasBlood;
            _crossingSuccess = save.CrossingSuccess;
            _forgeryQuality = save.ForgeryQuality;
        }
    }

    [Serializable]
    public class TransitPassSave
    {
        public string CurrentStage;
        public string ForgerId;
        public bool HasStamp;
        public bool HasInk;
        public bool HasBlood;
        public bool CrossingSuccess;
        public float ForgeryQuality;
    }

    /// <summary>
    /// Expansion VIII — Quest: The Value of Breath (The Actuary's Equation).
    /// When the air runs out, math replaces mercy.
    ///
    /// Stage 1: The Failure — Air filtration catastrophic failure, 48 hours to asphyxiation
    /// Stage 2: The Equation — Actuary ranks survivors by Caloric-to-Utility Ratio
    /// Stage 3: The Audit — Accept the math (euthanize 2) or reject it (jury-rig fan)
    /// </summary>
    public class Quest_TheValueOfBreath
    {
        public const string QuestId = "quest_the_value_of_breath";

        public const string Stage1_Id = "stage_1_the_failure";
        public const string Stage2_Id = "stage_2_the_equation";
        public const string Stage3_Id = "stage_3_the_audit";

        public const int HoursToAsphyxiation = 48;
        public const int SurvivorsCanSupport = 4;
        public const float HypoxiaHealthDecayPerHour = 3f;
        public const float BrainDamageThreshold = 10f;

        public event Action<string> OnStageReached;
        public event Action<string, string> OnChoiceMade;
        public event Action<string> OnQuestCompleted;
        public event Action<string> OnEuthanasiaPerformed;
        public event Action<string> OnHypoxiaTriggered;

        private string _currentStage;
        private string _actuaryId;
        private int _deadlineHour;
        private bool _mathAccepted;
        private bool _mathRejected;
        private List<string> _euthanizedIds = new List<string>();
        private int _brainDamageCount;

        public string CurrentStage => _currentStage;
        public bool IsActive => !string.IsNullOrEmpty(_currentStage);
        public bool WasMathAccepted => _mathAccepted;

        public bool TriggerAirFailure(string actuaryId, int currentHour)
        {
            if (_currentStage != null) return false;
            _currentStage = Stage1_Id;
            _actuaryId = actuaryId;
            _deadlineHour = currentHour + HoursToAsphyxiation;
            OnStageReached?.Invoke(Stage1_Id);
            return true;
        }

        public bool PresentEquation(List<string> recommendedEuthanasia)
        {
            if (_currentStage != Stage1_Id) return false;
            _currentStage = Stage2_Id;
            OnStageReached?.Invoke(Stage2_Id);
            return true;
        }

        public bool MakeAuditChoice(string choiceId, List<string> euthanizedIds = null)
        {
            if (_currentStage != Stage2_Id) return false;

            if (choiceId == "accept_math")
            {
                _mathAccepted = true;
                if (euthanizedIds != null)
                {
                    _euthanizedIds.AddRange(euthanizedIds);
                    for (int i = 0; i < euthanizedIds.Count; i++)
                        OnEuthanasiaPerformed?.Invoke(euthanizedIds[i]);
                }
            }
            else if (choiceId == "reject_math")
            {
                _mathRejected = true;
                _brainDamageCount = 2; // Two survivors suffer permanent brain damage
                OnHypoxiaTriggered?.Invoke(_actuaryId);
            }

            OnChoiceMade?.Invoke(Stage3_Id, choiceId);
            OnQuestCompleted?.Invoke(QuestId);
            return true;
        }

        public ValueOfBreathSave CaptureState()
        {
            return new ValueOfBreathSave
            {
                CurrentStage = _currentStage,
                ActuaryId = _actuaryId,
                DeadlineHour = _deadlineHour,
                MathAccepted = _mathAccepted,
                MathRejected = _mathRejected,
                EuthanizedIds = _euthanizedIds.ToArray(),
                BrainDamageCount = _brainDamageCount
            };
        }

        public void RestoreState(ValueOfBreathSave save)
        {
            _currentStage = null;
            _actuaryId = null;
            _deadlineHour = 0;
            _mathAccepted = false;
            _mathRejected = false;
            _euthanizedIds.Clear();
            _brainDamageCount = 0;
            if (save == null) return;
            _currentStage = save.CurrentStage;
            _actuaryId = save.ActuaryId;
            _deadlineHour = save.DeadlineHour;
            _mathAccepted = save.MathAccepted;
            _mathRejected = save.MathRejected;
            _brainDamageCount = save.BrainDamageCount;
            if (save.EuthanizedIds != null)
                _euthanizedIds.AddRange(save.EuthanizedIds);
        }
    }

    [Serializable]
    public class ValueOfBreathSave
    {
        public string CurrentStage;
        public string ActuaryId;
        public int DeadlineHour;
        public bool MathAccepted;
        public bool MathRejected;
        public string[] EuthanizedIds;
        public int BrainDamageCount;
    }
}
