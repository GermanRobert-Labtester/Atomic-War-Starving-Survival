using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Events
{
    /// <summary>
    /// Expansion VI — At-Home Encounters (The Micro-Tragedies). Not grand quests.
    /// Tuesday in the end of the world. These conversational side-objectives pop
    /// up during Day Operations. They test your administration of human dignity
    /// versus resource math. Handled by BunkerSocialSystems and EventRunner.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class BunkerMicroNarrativeSystem
    {
        // ── Encounter ids ─────────────────────────────────────────────
        public const string Enc_WrongBirthday = "enc_the_wrong_birthday";
        public const string Enc_ContrabandFrequency = "enc_the_contraband_frequency";
        public const string Enc_LastBook = "enc_the_last_book";
        public const string Enc_PhantomKnock = "enc_the_phantom_knock";

        // ── Choice ids ────────────────────────────────────────────────
        // Birthday
        public const string Choice_CorrectThem = "choice_correct_them";
        public const string Choice_LieAndBake = "choice_lie_and_bake";
        public const string Choice_CompromiseToy = "choice_compromise_toy";

        // Contraband
        public const string Choice_ConfiscateSmash = "choice_confiscate_smash";
        public const string Choice_LookOtherWay = "choice_look_other_way";
        public const string Choice_ListenWithThem = "choice_listen_with_them";

        // Last Book
        public const string Choice_ReadIt = "choice_read_it";
        public const string Choice_BurnIt = "choice_burn_it";
        public const string Choice_TearInHalf = "choice_tear_in_half";

        // Phantom Knock
        public const string Choice_SendMechanic = "choice_send_mechanic";
        public const string Choice_SealVent = "choice_seal_vent";
        public const string Choice_IgnoreIt = "choice_ignore_it";

        // ── Item ids ──────────────────────────────────────────────────
        public const string Item_AshCakeSweet = "ash_cake_sweet";
        public const string Item_Sugar = "sugar";
        public const string Item_WheatFlour = "wheat_flour";
        public const string Item_Book = "book";
        public const string Item_TornBookPages = "torn_book_pages";
        public const string Item_CarvedWoodenAnimal = "carved_wooden_animal";

        // ── Effect constants ──────────────────────────────────────────
        public const float Birthday_InnocenceDrop = -15f;
        public const float Birthday_MoraleBoost = 15f;
        public const float Birthday_FierceMotherMorale = 10f;
        public const float Birthday_FuelCost = 12f; // hours of heater fuel

        public const float Contraband_MoraleBoost = 15f;
        public const float Contraband_IodineDemand = 1f;
        public const float Listen_LeadershipDrop = -5f;

        public const float ReadIt_MoraleBoost = 15f;
        public const float ReadIt_FrostbiteRisk = 10f;
        public const float BurnIt_SurvivorsGuilt = 1f;
        public const float TearInHalf_MoraleDrop = -5f;

        public const float PhantomKnock_ExhaustionPenalty = 10f;
        public const float SealVent_AirflowDrop = 0.05f;
        public const float SealVent_MoraleDrop = -3f;
        public const float IgnoreIt_FriendlyFireChance = 0.15f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnEncounterTriggered;
        public event Action<string, string> OnChoiceMade;
        public event Action<string, float> OnResourceConsumed;
        public event Action<string> OnMoralChronicleEntry;

        private readonly System.Random _rng;
        private readonly HashSet<string> _triggeredEncounters = new HashSet<string>();
        private readonly HashSet<string> _completedEncounters = new HashSet<string>();
        private readonly List<EncounterRecord> _encounterLog = new List<EncounterRecord>();
        private int _booksBurned;

        public int BooksBurned => _booksBurned;
        public IReadOnlyList<EncounterRecord> EncounterLog => _encounterLog;

        public BunkerMicroNarrativeSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(1111);
        }

        // ── Encounter triggers ────────────────────────────────────────

        /// <summary>Check if an encounter should trigger today.</summary>
        public string RollDailyEncounter(int currentDay, bool hasChild, bool hasInsomniac,
            bool hasTeacher, bool hasMechanic)
        {
            // Only one encounter per day, max 4 per run
            if (_completedEncounters.Count >= 4) return null;

            // Each encounter has conditions
            if (hasChild && !_completedEncounters.Contains(Enc_WrongBirthday)
                && _rng.NextDouble() < 0.08f)
                return Enc_WrongBirthday;

            if (hasInsomniac && !_completedEncounters.Contains(Enc_ContrabandFrequency)
                && currentDay >= 15 && _rng.NextDouble() < 0.06f)
                return Enc_ContrabandFrequency;

            if (hasTeacher && !_completedEncounters.Contains(Enc_LastBook)
                && currentDay >= 30 && _rng.NextDouble() < 0.05f)
                return Enc_LastBook;

            if (!_completedEncounters.Contains(Enc_PhantomKnock)
                && currentDay >= 20 && _rng.NextDouble() < 0.07f)
                return Enc_PhantomKnock;

            return null;
        }

        /// <summary>Trigger an encounter.</summary>
        public bool TriggerEncounter(string encounterId)
        {
            if (string.IsNullOrEmpty(encounterId)) return false;
            if (_completedEncounters.Contains(encounterId)) return false;
            if (!_triggeredEncounters.Add(encounterId)) return false;
            OnEncounterTriggered?.Invoke(encounterId);
            return true;
        }

        // ── Encounter resolutions ─────────────────────────────────────

        /// <summary>Resolve The Wrong Birthday.</summary>
        public BirthdayResult ResolveBirthday(string choiceId, string childId,
            string fierceMotherId)
        {
            var result = new BirthdayResult { Choice = choiceId };

            switch (choiceId)
            {
                case Choice_CorrectThem:
                    result.InnocenceChange = Birthday_InnocenceDrop;
                    result.MoraleChange = 0f;
                    result.Message = "The child stops drawing with chalk. The wall stays bare.";
                    OnMoralChronicleEntry?.Invoke("You told them the truth. The calendar was wrong.");
                    break;

                case Choice_LieAndBake:
                    result.InnocenceChange = 0f;
                    result.MoraleChange = Birthday_MoraleBoost;
                    result.FierceMotherMoraleChange = Birthday_FierceMotherMorale;
                    result.ResourcesConsumed = "1x sugar, 1x wheat_flour, 1x fuel";
                    result.Message = "The child's Morale maxes out. The Fierce Mother smiles for the first time in weeks.";
                    OnResourceConsumed?.Invoke("sugar", 1);
                    OnResourceConsumed?.Invoke("wheat_flour", 1);
                    OnMoralChronicleEntry?.Invoke("Day " + 0 + ". You burned 12 hours of heater fuel for a lie. The child smiled.");
                    break;

                case Choice_CompromiseToy:
                    result.InnocenceChange = -5f;
                    result.MoraleChange = 8f;
                    result.Message = "The child is placated. The Hoarder notices and demands a finder's fee.";
                    result.TriggersSocialFeud = true;
                    break;
            }

            _completedEncounters.Add(Enc_WrongBirthday);
            RecordEncounter(Enc_WrongBirthday, choiceId);
            OnChoiceMade?.Invoke(Enc_WrongBirthday, choiceId);
            return result;
        }

        /// <summary>Resolve The Contraband Frequency.</summary>
        public ContrabandResult ResolveContraband(string choiceId, string offenderId)
        {
            var result = new ContrabandResult { Choice = choiceId };

            switch (choiceId)
            {
                case Choice_ConfiscateSmash:
                    result.OffenderMoraleChange = -20f;
                    result.TriggersWithdrawal = true;
                    result.TriggersViolentOutburst = true;
                    result.WorkRefusalDays = 2;
                    result.Message = "The radio is smashed. The offender refuses to work for 2 days.";
                    break;

                case Choice_LookOtherWay:
                    result.OffenderMoraleChange = Contraband_MoraleBoost;
                    result.NoiseLeakDetected = true;
                    result.Message = "The offender's Morale +15. The Hypochondriac hears the leak and demands iodine.";
                    break;

                case Choice_ListenWithThem:
                    result.IntelGained = true;
                    result.LeadershipChange = Listen_LeadershipDrop;
                    result.TriggersMutinyPlot = true;
                    result.Message = "You gain intel. The General sees you fraternizing and begins plotting mutiny.";
                    break;
            }

            _completedEncounters.Add(Enc_ContrabandFrequency);
            RecordEncounter(Enc_ContrabandFrequency, choiceId);
            OnChoiceMade?.Invoke(Enc_ContrabandFrequency, choiceId);
            return result;
        }

        /// <summary>Resolve The Last Book.</summary>
        public LastBookResult ResolveLastBook(string choiceId, string teacherId,
            string mechanicId)
        {
            var result = new LastBookResult { Choice = choiceId };

            switch (choiceId)
            {
                case Choice_ReadIt:
                    result.MoraleChange = ReadIt_MoraleBoost;
                    result.FrostbiteRisk = true;
                    result.Message = "The Teacher reads a chapter. The heater runs cold tonight.";
                    OnMoralChronicleEntry?.Invoke("The Teacher reads. The heater dies. The Elderly shiver.");
                    break;

                case Choice_BurnIt:
                    result.MoraleChange = 0f;
                    result.HeaterBoosted = true;
                    result.TeacherSurvivorsGuilt = true;
                    _booksBurned++;
                    result.Message = "The heater glows brighter. The Teacher faces the wall.";
                    OnMoralChronicleEntry?.Invoke("Day " + 0 + ". You burned the last story to keep the pipes from bursting.");
                    break;

                case Choice_TearInHalf:
                    result.MoraleChange = TearInHalf_MoraleDrop;
                    result.StoryRuined = true;
                    result.FireWeak = true;
                    result.Message = "Both sides are furious. The story is ruined. The fire is weak. Everyone loses.";
                    break;
            }

            _completedEncounters.Add(Enc_LastBook);
            RecordEncounter(Enc_LastBook, choiceId);
            OnChoiceMade?.Invoke(Enc_LastBook, choiceId);
            return result;
        }

        /// <summary>Resolve The Phantom Knock.</summary>
        public PhantomKnockResult ResolvePhantomKnock(string choiceId, string mechanicId)
        {
            var result = new PhantomKnockResult { Choice = choiceId };

            switch (choiceId)
            {
                case Choice_SendMechanic:
                    result.MechanicExhaustion = PhantomKnock_ExhaustionPenalty;
                    result.FoundRat = true;
                    result.Message = "It's just a rat. The Mechanic is exhausted tomorrow.";
                    break;

                case Choice_SealVent:
                    result.AirflowDrop = SealVent_AirflowDrop;
                    result.MoraleChange = SealVent_MoraleDrop;
                    result.MaterialsUsed = "scrap_metal, duct_tape";
                    result.Message = "The vent is sealed. Airflow drops 5%. Mild headaches for everyone.";
                    break;

                case Choice_IgnoreIt:
                    result.InsomniacFatigueMaxed = true;
                    result.TriggersParanoia = true;
                    result.FriendlyFireChance = IgnoreIt_FriendlyFireChance;
                    result.Message = "The Insomniac's Fatigue maxes out. They might discharge a weapon in the bunkroom.";
                    break;
            }

            _completedEncounters.Add(Enc_PhantomKnock);
            RecordEncounter(Enc_PhantomKnock, choiceId);
            OnChoiceMade?.Invoke(Enc_PhantomKnock, choiceId);
            return result;
        }

        // ── Record keeping ────────────────────────────────────────────

        private void RecordEncounter(string encounterId, string choiceId)
        {
            _encounterLog.Add(new EncounterRecord
            {
                EncounterId = encounterId,
                ChoiceId = choiceId,
                DayRecorded = 0
            });
        }

        // ── Save / Load ───────────────────────────────────────────────

        public MicroNarrativeSave CaptureState()
        {
            var triggered = new string[_triggeredEncounters.Count];
            _triggeredEncounters.CopyTo(triggered);
            var completed = new string[_completedEncounters.Count];
            _completedEncounters.CopyTo(completed);
            var log = new EncounterRecordSave[_encounterLog.Count];
            for (int i = 0; i < _encounterLog.Count; i++)
                log[i] = new EncounterRecordSave
                {
                    EncounterId = _encounterLog[i].EncounterId,
                    ChoiceId = _encounterLog[i].ChoiceId,
                    DayRecorded = _encounterLog[i].DayRecorded
                };
            return new MicroNarrativeSave
            {
                TriggeredEncounters = triggered,
                CompletedEncounters = completed,
                BooksBurned = _booksBurned,
                EncounterLog = log
            };
        }

        public void RestoreState(MicroNarrativeSave save)
        {
            _triggeredEncounters.Clear();
            _completedEncounters.Clear();
            _encounterLog.Clear();
            _booksBurned = 0;
            if (save == null) return;
            _booksBurned = save.BooksBurned;
            if (save.TriggeredEncounters != null)
                for (int i = 0; i < save.TriggeredEncounters.Length; i++)
                    if (!string.IsNullOrEmpty(save.TriggeredEncounters[i]))
                        _triggeredEncounters.Add(save.TriggeredEncounters[i]);
            if (save.CompletedEncounters != null)
                for (int i = 0; i < save.CompletedEncounters.Length; i++)
                    if (!string.IsNullOrEmpty(save.CompletedEncounters[i]))
                        _completedEncounters.Add(save.CompletedEncounters[i]);
            if (save.EncounterLog != null)
                for (int i = 0; i < save.EncounterLog.Length; i++)
                    if (save.EncounterLog[i] != null)
                        _encounterLog.Add(new EncounterRecord
                        {
                            EncounterId = save.EncounterLog[i].EncounterId,
                            ChoiceId = save.EncounterLog[i].ChoiceId,
                            DayRecorded = save.EncounterLog[i].DayRecorded
                        });
        }
    }

    // ── Result types ──────────────────────────────────────────────────

    [Serializable]
    public class BirthdayResult
    {
        public string Choice;
        public float InnocenceChange;
        public float MoraleChange;
        public float FierceMotherMoraleChange;
        public string ResourcesConsumed;
        public bool TriggersSocialFeud;
        public string Message;
    }

    [Serializable]
    public class ContrabandResult
    {
        public string Choice;
        public float OffenderMoraleChange;
        public bool TriggersWithdrawal;
        public bool TriggersViolentOutburst;
        public int WorkRefusalDays;
        public bool NoiseLeakDetected;
        public bool IntelGained;
        public float LeadershipChange;
        public bool TriggersMutinyPlot;
        public string Message;
    }

    [Serializable]
    public class LastBookResult
    {
        public string Choice;
        public float MoraleChange;
        public bool FrostbiteRisk;
        public bool HeaterBoosted;
        public bool TeacherSurvivorsGuilt;
        public bool StoryRuined;
        public bool FireWeak;
        public string Message;
    }

    [Serializable]
    public class PhantomKnockResult
    {
        public string Choice;
        public float MechanicExhaustion;
        public bool FoundRat;
        public float AirflowDrop;
        public float MoraleChange;
        public string MaterialsUsed;
        public bool InsomniacFatigueMaxed;
        public bool TriggersParanoia;
        public float FriendlyFireChance;
        public string Message;
    }

    public class EncounterRecord
    {
        public string EncounterId;
        public string ChoiceId;
        public int DayRecorded;
    }

    [Serializable]
    public class MicroNarrativeSave
    {
        public string[] TriggeredEncounters;
        public string[] CompletedEncounters;
        public int BooksBurned;
        public EncounterRecordSave[] EncounterLog;
    }

    [Serializable]
    public class EncounterRecordSave
    {
        public string EncounterId;
        public string ChoiceId;
        public int DayRecorded;
    }
}
