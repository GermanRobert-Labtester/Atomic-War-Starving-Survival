using System;
using System.Collections.Generic;
using AtomicWar._Game.Events;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// "Into the Ash" expansion — multi-stage narrative quest chains (Part III).
    ///
    /// Six questlines spanning 8 locations. Each advances through distinct stages
    /// triggered by expeditions, item discoveries, faction interactions, and daily
    /// time-gated events. All string identifiers (flags, events, items, locations)
    /// are canonicalised in <see cref="ExpansionQuestConstants"/> so new quests
    /// can be added by extending the constants classes and wiring new stage handlers.
    ///
    /// Architecture: plain C#, save/load safe, event-bus driven for chronicle
    /// entries (<c>EventBus.Raise(new MoralChronicleEntryRequested(...))</c>).
    /// </summary>
    public class ExpansionQuestChainsSystem
    {
        // ── State ──────────────────────────────────────────────────────────
        private readonly Dictionary<string, ExpansionQuestState> _quests
            = new Dictionary<string, ExpansionQuestState>();

        // ── Host wiring (set once at boot via Wire) ────────────────────────
        private Func<int> _getDay;
        private Func<string, bool> _hasFlag;
        private Action<string, bool> _setFlag;
        private Func<string, int> _countItem;
        private Func<string, int, bool> _consumeItem;
        private Func<string, bool> _hasItem;

        // ── Events (subscribed by host) ────────────────────────────────────
        public event Action<string, int, string> OnQuestStageChanged;
        public event Action<string, string> OnQuestCompleted;
        public event Action<string, string> OnQuestFailed;
        public event Action<string> OnRequestExpedition;
        public event Action<string, string, int> OnRequestBunkerEvent;

        // ── Delegate callbacks (assigned by host at boot) ──────────────────
        public Func<string, float> GetFactionTrust;
        public Action<string, float> ModifyFactionTrust;
        public Action<float> ModifyRaidProbability;
        public Action<float> ApplyGlobalMorale;
        public Action<string, float> ApplySurvivorMorale;

        /// <summary>Ledger disposal choices.</summary>
        public static class LedgerChoices
        {
            public const string Burn         = "burn";
            public const string Keep         = "keep";
            public const string GiveMilitia  = "give_militia";
            public const string GiveCult     = "give_cult";
        }

        // ───────────────────────────────────────────────────────────────────
        // Construction & wiring
        // ───────────────────────────────────────────────────────────────────

        public ExpansionQuestChainsSystem()
        {
            SeedQuestDefinitions();
        }

        /// <summary>Wire host delegates. Must be called once before any other method.</summary>
        public void Wire(
            Func<int> getDay,
            Func<string, bool> hasFlag,
            Action<string, bool> setFlag,
            Func<string, int> countItem,
            Func<string, int, bool> consumeItem,
            Func<string, bool> hasItem)
        {
            _getDay = getDay;
            _hasFlag = hasFlag;
            _setFlag = setFlag;
            _countItem = countItem;
            _consumeItem = consumeItem;
            _hasItem = hasItem;
        }

        // ───────────────────────────────────────────────────────────────────
        // Public query API
        // ───────────────────────────────────────────────────────────────────

        /// <summary>Get quest state by id (never null for valid ids).</summary>
        public ExpansionQuestState GetQuest(string questId)
        {
            if (string.IsNullOrEmpty(questId)) return null;
            _quests.TryGetValue(questId, out var state);
            return state;
        }

        /// <summary>Whether a quest is active (started, not completed/failed/terminated).</summary>
        public bool HasActiveQuest(string questId)
        {
            var q = GetQuest(questId);
            return q != null && !q.IsCompleted && !q.IsFailed && !q.IsTerminated && q.CurrentStage > 0;
        }

        // ───────────────────────────────────────────────────────────────────
        // Core state transitions
        // ───────────────────────────────────────────────────────────────────

        /// <summary>
        /// Advance a quest to a new stage. Fires <see cref="OnQuestStageChanged"/>
        /// and, if the stage reaches <see cref="ExpansionQuestState.MaxStages"/>,
        /// fires <see cref="OnQuestCompleted"/>. Returns true if the quest completed.
        /// </summary>
        public bool AdvanceStage(string questId, int newStage, string description = null)
        {
            var q = GetQuest(questId);
            if (q == null || q.IsCompleted || q.IsFailed || q.IsTerminated) return false;
            if (newStage <= q.CurrentStage) return false;

            q.CurrentStage = newStage;
            q.StageDescription = description ?? string.Empty;
            RecordChronicle(description ?? $"Quest {questId}: Stage {newStage}");
            OnQuestStageChanged?.Invoke(questId, newStage, description);

            if (newStage >= q.MaxStages)
            {
                q.IsCompleted = true;
                OnQuestCompleted?.Invoke(questId, q.RewardDescription);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Terminate a quest early (player closed the path). Fires
        /// <see cref="OnQuestStageChanged"/> with stage = -1 but does NOT
        /// fire <see cref="OnQuestCompleted"/>.
        /// </summary>
        public void TerminateQuest(string questId, string description)
        {
            var q = GetQuest(questId);
            if (q == null || q.IsCompleted || q.IsFailed || q.IsTerminated) return;
            q.IsTerminated = true;
            q.IsCompleted = true;
            q.StageDescription = description ?? string.Empty;
            RecordChronicle(description ?? $"Quest terminated: {questId}");
            OnQuestStageChanged?.Invoke(questId, -1, description);
        }

        /// <summary>
        /// Mark a quest as failed. Blocked if already completed or terminated.
        /// </summary>
        public void FailQuest(string questId, string reason)
        {
            var q = GetQuest(questId);
            if (q == null || q.IsCompleted || q.IsTerminated) return;
            q.IsFailed = true;
            OnQuestFailed?.Invoke(questId, reason);
            RecordChronicle($"Quest failed: {questId}. {reason}");
        }

        // ───────────────────────────────────────────────────────────────────
        // Expedition-return dispatcher (one stage per quest per expedition)
        // ───────────────────────────────────────────────────────────────────

        /// <summary>
        /// Called by the host when an expedition returns. Scans the node id and
        /// items found against all active quests. Uses else-if chains so at most
        /// ONE stage advances per quest per expedition call.
        /// </summary>
        public void OnExpeditionReturned(string nodeId, List<string> itemsFound)
        {
            int day = Day;
            bool hasItems = itemsFound != null && itemsFound.Count > 0;

            HandleCheckpointKilo(day, hasItems, itemsFound);
            HandleDistrictCoordOffice(day, hasItems, itemsFound);
            HandleMartaFarmhouse(day, hasItems, itemsFound);
            HandleMilitiaGrainExchange(day, hasItems, itemsFound);
            HandleGlowChapel(day, hasItems, itemsFound);
            HandleTollHouse(day, hasItems, itemsFound);
            HandleStMarenAnnex(day, hasItems, itemsFound);
            HandleRadioTowerSeven(day, hasItems, itemsFound);
        }

        private void HandleCheckpointKilo(int day, bool hasItems, List<string> itemsFound)
        {
            var q = GetQuest(QuestIds.CheckpointKiloTruth);
            if (q == null || q.IsCompleted || q.IsTerminated) return;

            if (q.CurrentStage == 0 && hasItems && itemsFound.Contains(QuestTriggerItems.PrivateMarenJournal))
                AdvanceStage(QuestIds.CheckpointKiloTruth, 1,
                    $"Day {day}. You found Private Maren's journal. 89 mounds. A helmet on a stick. The truth in a dead soldier's handwriting.");
            else if (q.CurrentStage == 1 && hasItems && itemsFound.Contains(QuestTriggerItems.Battery))
            {
                AdvanceStage(QuestIds.CheckpointKiloTruth, 2,
                    $"Day {day}. The intercom played the last three seconds: 'Remain calm. Help is—' Then static. Then nothing.");
                ApplyGlobalMorale?.Invoke(-5f);
            }
        }

        private void HandleDistrictCoordOffice(int day, bool hasItems, List<string> itemsFound)
        {
            var q = GetQuest(QuestIds.CheckpointKiloTruth);
            if (q != null && q.CurrentStage >= 4 && !q.IsCompleted && !q.IsTerminated
                && hasItems && itemsFound.Contains(QuestTriggerItems.ComplianceLedger))
            {
                SetFlag(QuestFlags.ComplianceLedgerFound);
            }
        }

        private void HandleMartaFarmhouse(int day, bool hasItems, List<string> itemsFound)
        {
            var q = GetQuest(QuestIds.GrainWar);
            if (q != null && q.CurrentStage == 0 && hasItems && itemsFound.Contains(QuestTriggerItems.MartaReceipt))
            {
                AdvanceStage(QuestIds.GrainWar, 1,
                    $"Day {day}. You found Marta's kitchen. The meal was interrupted. The children are gone. The receipt is on the table.");
                ApplyGlobalMorale?.Invoke(-3f);
                SetFlag(QuestFlags.MartaReceiptFound);
            }
        }

        private void HandleMilitiaGrainExchange(int day, bool hasItems, List<string> itemsFound)
        {
            var q = GetQuest(QuestIds.GrainWar);
            if (q == null || q.IsCompleted || q.IsTerminated) return;

            if (q.CurrentStage == 2 && hasItems && itemsFound.Contains(QuestTriggerItems.RequisitionLedger))
                AdvanceStage(QuestIds.GrainWar, 4,
                    $"Day {day}. Voss's ledger. 340 entries. The math says the villages will starve in 40 days.");
            else if (q.CurrentStage >= 4 && hasItems && itemsFound.Contains(QuestTriggerItems.MilitiaCharterFeedSack))
                AdvanceStage(QuestIds.GrainWar, 6,
                    $"Day {day}. The founding charter. 'We protect the land. We protect the people on the land.' The ink is faded. The words are still true. The words are also a lie.");
        }

        private void HandleGlowChapel(int day, bool hasItems, List<string> itemsFound)
        {
            var q = GetQuest(QuestIds.GlowCommunion);
            if (q != null && q.CurrentStage == 4 && hasItems && itemsFound.Contains(QuestTriggerItems.BrotherOrinJournal))
                AdvanceStage(QuestIds.GlowCommunion, 5,
                    $"Day {day}. Orin's journal. 'The light is not real. But the grinding stops. Is that not enough? Is that not mercy?'");
        }

        private void HandleTollHouse(int day, bool hasItems, List<string> itemsFound)
        {
            var q = GetQuest(QuestIds.TributeBreak);
            if (q == null || q.IsCompleted || q.IsTerminated) return;

            if (q.CurrentStage == 1 && hasItems && itemsFound.Contains(QuestTriggerItems.TributeLedger))
                AdvanceStage(QuestIds.TributeBreak, 2,
                    $"Day {day}. The tribute ledger. 47 shelters. You are Entry 48.");
            else if (q.CurrentStage == 2 && hasItems && itemsFound.Contains(QuestTriggerItems.SableJournal))
                AdvanceStage(QuestIds.TributeBreak, 3,
                    $"Day {day}. Sable's journal. 'Kael would have let the child go. Kael is dead. I am not dead. The math keeps me alive.'");
        }

        private void HandleStMarenAnnex(int day, bool hasItems, List<string> itemsFound)
        {
            var q = GetQuest(QuestIds.StMarenLastShift);
            if (q == null || q.IsCompleted || q.IsTerminated) return;

            if (q.CurrentStage == 0)
                AdvanceStage(QuestIds.StMarenLastShift, 1,
                    $"Day {day}. The beds are made. The pharmacy is locked. The shift schedule still reads Day 19. The nurse's name is 'Lena.'");
            else if (q.CurrentStage == 1 && hasItems && itemsFound.Contains(QuestTriggerItems.HeadNurseCombinationCard))
                AdvanceStage(QuestIds.StMarenLastShift, 2,
                    $"Day {day}. The pharmacy is open. The supplies are for the living now. The dead don't need morphine.");
            else if (q.CurrentStage == 2 && hasItems && itemsFound.Contains(QuestTriggerItems.PatientRecordsAnnex))
                AdvanceStage(QuestIds.StMarenLastShift, 3,
                    $"Day {day}. Thirty-eight patient records. Each one a life that ended in this building.");
        }

        private void HandleRadioTowerSeven(int day, bool hasItems, List<string> itemsFound)
        {
            var q = GetQuest(QuestIds.TowerSevenBroadcast);
            if (q == null || q.IsCompleted || q.IsTerminated) return;

            if (q.CurrentStage == 0)
                AdvanceStage(QuestIds.TowerSevenBroadcast, 1,
                    $"Day {day}. The tower is down. The base station is intact. The bunker is sealed. 'Is anyone there?'");
            else if (q.CurrentStage == 1 && hasItems && itemsFound.Contains(QuestTriggerItems.FrequencyLogSealed))
            {
                AdvanceStage(QuestIds.TowerSevenBroadcast, 2,
                    $"Day {day}. The frequency log contains the bunker code. A page is torn out: 'TRANSMIT ORDER 7741. CONFIRMED. GOD FORGIVE US.'");
                SetFlag(QuestFlags.TowerSevenCodeKnown);
            }
            else if (q.CurrentStage == 2 && hasItems && itemsFound.Contains(QuestTriggerItems.CaptainVennKeycard))
            {
                AdvanceStage(QuestIds.TowerSevenBroadcast, 3,
                    $"Day {day}. The bunker is open. Captain Venn's final log: 'The ceasefire has not been confirmed. The frequencies are quiet.'");
                SetFlag(QuestFlags.TowerSevenBunkerOpen);
            }
        }

        // ───────────────────────────────────────────────────────────────────
        // Quest-specific triggers (called by host event handlers)
        // ───────────────────────────────────────────────────────────────────

        /// <summary>Militia runner arrives with trade offer (Grain War Stage 2).</summary>
        public void TriggerMilitiaOffer()
        {
            var q = GetQuest(QuestIds.GrainWar);
            if (q == null || q.CurrentStage != 1) return;
            RequestBunkerEvent(QuestEventIds.MilitiaSilenceOffer, QuestIds.GrainWar);
        }

        /// <summary>Player accepts militia bribe — quest terminates.</summary>
        public void AcceptMilitiaSilenceTrade()
        {
            var q = GetQuest(QuestIds.GrainWar);
            if (q == null || q.CurrentStage != 1) return;
            TerminateQuest(QuestIds.GrainWar, "The militia's silence was bought with flour and seeds.");
            RecordChronicle("You accepted the militia's trade. The flour will feed your people. The silence will feed theirs.",
                MoralChronicleEntryKind.FactionChoice);
        }

        /// <summary>Player refuses militia bribe — quest continues to stage 2.</summary>
        public void RefuseMilitiaSilenceTrade()
        {
            var q = GetQuest(QuestIds.GrainWar);
            if (q == null || q.CurrentStage != 1) return;
            AdvanceStage(QuestIds.GrainWar, 2,
                "You refused the militia's offer. The next patrol passes without stopping.");
        }

        /// <summary>Children extracted from militia youth program. Requires stealth check.
        /// Spec: Stealth 70%. Sets flag regardless of outcome; only advances stage if at
        /// stage 2 AND stealth check passes.</summary>
        public bool ChildrenExtractedFromMilitia(float stealthSkill)
        {
            var q = GetQuest(QuestIds.GrainWar);
            if (q == null || q.IsCompleted || q.IsTerminated) return false;
            if (q.CurrentStage < 1) return false;
            SetFlag(QuestFlags.MartaChildrenExtracted);
            if (q.CurrentStage == 2 && stealthSkill >= 0.7f)
            {
                AdvanceStage(QuestIds.GrainWar, 3,
                    "Tomas and Lena are in the bunker. Tomas said, 'Are you from Mama's house?' The militia will notice in 3 days.");
                return true;
            }
            return false;
        }

        /// <summary>Marta arrives at the bunker hatch (Grain War Stage 5).</summary>
        public void MartaArrivesAtBunker()
        {
            var q = GetQuest(QuestIds.GrainWar);
            if (q == null || !HasFlag(QuestFlags.MartaChildrenExtracted)) return;
            AdvanceStage(QuestIds.GrainWar, 5,
                "A woman at the hatch. Thin. Cracked hands. 'I'm looking for my children. Are they here? Please.'");
            RequestBunkerEvent(QuestEventIds.MartaReunion, QuestIds.GrainWar);
        }

        /// <summary>Cult members arrive with invitation (Glow Communion Stage 1).</summary>
        public void TriggerCultInvitation()
        {
            var q = GetQuest(QuestIds.GlowCommunion);
            if (q == null || q.CurrentStage > 0) return;
            AdvanceStage(QuestIds.GlowCommunion, 1,
                "Two Cult members at the hatch. Unarmed. Calm. 'We heard you lost someone. We'd like to sit with you.' They hum for six hours. Then they leave.");
            ApplyGlobalMorale?.Invoke(2f);
            SetFlag(QuestFlags.CultVisitedBunker);
        }

        /// <summary>Allow the cult ceremony (Glow Communion Stage 2).</summary>
        public void AllowCultCeremony()
        {
            var q = GetQuest(QuestIds.GlowCommunion);
            if (q == null || q.CurrentStage != 1) return;
            AdvanceStage(QuestIds.GlowCommunion, 2,
                "The cult lit candles and sang. The song was simple. The song was warm. They knew the names of your dead.");
            ApplyGlobalMorale?.Invoke(5f);
            SetFlag(QuestFlags.CultCeremonyAllowed);
        }

        /// <summary>Refuse the cult ceremony — quest terminates.</summary>
        public void RefuseCultCeremony()
        {
            var q = GetQuest(QuestIds.GlowCommunion);
            if (q == null || q.CurrentStage != 1) return;
            TerminateQuest(QuestIds.GlowCommunion,
                "The cult left the candles at the hatch. The bunker smelled like tallow and something sweet.");
            SetFlag(QuestFlags.CultCeremonyRefused);
        }

        /// <summary>A survivor drinks from the radioactive Cup (Glow Communion Stage 3).</summary>
        public void SurvivorDrinksFromCup(string survivorName)
        {
            if (string.IsNullOrEmpty(survivorName)) return;
            var q = GetQuest(QuestIds.GlowCommunion);
            if (q == null || q.CurrentStage != 2) return;
            AdvanceStage(QuestIds.GlowCommunion, 3,
                $"{survivorName} drank from the Cup. +10 Morale, +5 mSv. They feel warm. They feel held. They feel chosen.");
            SetFlag(QuestFlags.CultCupDrunk);
            SetFlag(QuestFlags.CultConvertNamePrefix + survivorName);
            SetFlag(QuestFlags.CultConvertExists);
        }

        /// <summary>Player pours out and tests the Cup water (Glow Communion Stage 3, alt).</summary>
        public void TestAndRejectCupWater()
        {
            var q = GetQuest(QuestIds.GlowCommunion);
            if (q == null || q.CurrentStage != 2) return;
            AdvanceStage(QuestIds.GlowCommunion, 3,
                "You poured the water out and tested it. 15 mSv/L. The cult's smile didn't change. 'The Glow is not poison. The Glow is presence.' They won't return for 10 days.");
            SetFlag(QuestFlags.CultCupTested);
        }

        /// <summary>First warlord tribute collection (Tribute Break Stage 1).</summary>
        public void TriggerFirstThursday()
        {
            var q = GetQuest(QuestIds.TributeBreak);
            if (q == null || q.CurrentStage > 0) return;
            AdvanceStage(QuestIds.TributeBreak, 1,
                $"Day {Day}. Thursday. They came. They took. They left a flashlight. Thursday comes again.");
            SetFlag(QuestFlags.WarlordFirstThursday);
        }

        /// <summary>Player frees the child from the Toll House holding cell.</summary>
        public void FreeChildFromTollHouse()
        {
            var q = GetQuest(QuestIds.TributeBreak);
            if (q == null || q.CurrentStage != 2) return;
            AdvanceStage(QuestIds.TributeBreak, 3,
                "The child ran. The warlords noticed. Next Thursday, the tribute doubles.");
            SetFlag(QuestFlags.TollHouseChildFreed);
        }

        /// <summary>Player successfully breaks the tribute (Tribute Break Stage 5).</summary>
        public void TributeSuccessfullyBroken()
        {
            var q = GetQuest(QuestIds.TributeBreak);
            if (q == null || q.CurrentStage != 3) return;
            AdvanceStage(QuestIds.TributeBreak, 5,
                "Sable returned alone. No enforcers. No crowbar. 'You broke the math. The math will rebuild. It always does. But today, the math is yours.'");
            SetFlag(QuestFlags.TributeBroken);
        }

        /// <summary>The flashlight finale — Sable's last visit (Tribute Break Stage 6).</summary>
        public void FlashlightFinale()
        {
            var q = GetQuest(QuestIds.TributeBreak);
            if (q == null || q.IsCompleted || q.IsTerminated) return;
            if (q.CurrentStage != 5) return;
            AdvanceStage(QuestIds.TributeBreak, 6,
                $"Day {Day}. Sable placed the flashlight on the threshold. 'Kael's code. Always leave one thing.' She walked away. The math rebuilt. The flashlight was still on the table.");
        }

        /// <summary>Lena the nurse is found and talks (St. Maren Stage 3).
        /// Spec: Empathy 50%. Returns true if the check passed.</summary>
        public bool LenaFoundAndTalked(float empathySkill)
        {
            var q = GetQuest(QuestIds.StMarenLastShift);
            if (q == null || q.CurrentStage < 2) return false;
            if (empathySkill < 0.5f) return false;
            AdvanceStage(QuestIds.StMarenLastShift, 3,
                "Lena talked. 'Agnes said thank you. She said thank you and I walked out. I should have looked back.' She gave you her nurse's pin.");
            SetFlag(QuestFlags.LenaNurseFound);
            return true;
        }

        /// <summary>The 38 patient names are written in the bunker journal (St. Maren Stage 4).</summary>
        public void WriteThe38Names()
        {
            var q = GetQuest(QuestIds.StMarenLastShift);
            if (q == null || q.CurrentStage != 3) return;
            AdvanceStage(QuestIds.StMarenLastShift, 4,
                $"Day {Day}. You wrote their names. 38 names. They were patients. They were people. The ash does not write.");
            SetFlag(QuestFlags.AnnexNamesWritten);
        }

        /// <summary>A seed is planted in the Annex garden (St. Maren Stage 5).
        /// Spec: 20% chance the seed grows. Returns true if stage advanced, false otherwise.
        /// The host provides the RNG roll; values >= 0.8f mean the seed grew.</summary>
        public bool PlantSeedInAnnexGarden(float rngRoll)
        {
            var q = GetQuest(QuestIds.StMarenLastShift);
            if (q == null || q.CurrentStage != 4) return false;
            bool grew = rngRoll >= 0.8f; // 20% chance
            string desc = grew
                ? "You cleared a square metre of ash. You dug. You planted. The seed grew. Green in the grey. +5 Morale."
                : "You cleared a square metre of ash. You dug. You planted. The seed didn't grow. You water the dirt anyway.";
            AdvanceStage(QuestIds.StMarenLastShift, 5, desc);
            SetFlag(QuestFlags.AnnexGardenPlanted);
            if (grew) ApplyGlobalMorale?.Invoke(5f);
            return grew;
        }

        /// <summary>Escort Lena to the Annex (St. Maren Stage 6).</summary>
        public void EscortLenaToAnnex()
        {
            var q = GetQuest(QuestIds.StMarenLastShift);
            if (q == null || q.IsCompleted || q.IsFailed || q.IsTerminated) return;
            if (!HasFlag(QuestFlags.LenaNurseFound)) return;
            if (q.CurrentStage < 3) return;
            AdvanceStage(QuestIds.StMarenLastShift, 6,
                "Lena stood in Room 12. 'I'm sorry I walked out.' She said it to the empty bed. This time, not looking back is the mercy.");
            SetFlag(QuestFlags.LenaVisitedAnnex);
        }

        /// <summary>Player broadcasts the Checkpoint Kilo intercom recording (Kilo Truth Stage 3).</summary>
        public void BroadcastIntercomRecording()
        {
            var q = GetQuest(QuestIds.CheckpointKiloTruth);
            if (q == null || q.CurrentStage != 2) return;
            AdvanceStage(QuestIds.CheckpointKiloTruth, 3,
                "You broadcast the recording on every frequency. Everyone hears what the garrison did. The garrison cannot un-kill the 89.");
            SetFlag(QuestFlags.KiloIntercomBroadcast);
        }

        /// <summary>Player encounters Private Maren during an expedition (Kilo Truth Stage 4).
        /// Returns true if the empathy OR therapy check succeeded and the stage advanced.
        /// Spec: Empathy 60% or Therapy 50%.</summary>
        public bool EncounterPrivateMaren(float empathySkill, float therapySkill)
        {
            var q = GetQuest(QuestIds.CheckpointKiloTruth);
            if (q == null || q.CurrentStage != 3) return false;
            if (empathySkill >= 0.6f || therapySkill >= 0.5f)
            {
                AdvanceStage(QuestIds.CheckpointKiloTruth, 4,
                    "Maren sat down. 'I loaded them. Eighty-nine. I loaded them and I drove and I buried them and I filed the report.' He asked if anyone remembers.");
                return true;
            }
            return false;
        }

        /// <summary>Player places stones/candles at the trench memorial (Kilo Truth Stage 5).</summary>
        public void PlaceMemorialAtTrench()
        {
            var q = GetQuest(QuestIds.CheckpointKiloTruth);
            if (q == null || q.CurrentStage != 4) return;
            AdvanceStage(QuestIds.CheckpointKiloTruth, 5,
                $"Day {Day}. You remembered them. All of them. The ash does not.");
            ApplyGlobalMorale?.Invoke(-15f);
        }

        /// <summary>Player chooses what to do with the compliance ledger (Kilo Truth Stage 6).</summary>
        public void HandleComplianceLedger(string choice)
        {
            if (string.IsNullOrEmpty(choice)) return;
            var q = GetQuest(QuestIds.CheckpointKiloTruth);
            if (q == null || q.CurrentStage != 5) return;
            switch (choice)
            {
                case LedgerChoices.Burn:
                    AdvanceStage(QuestIds.CheckpointKiloTruth, 6,
                        "You burned the ledger. The record is gone. The evidence is ash. The 89 are still dead.");
                    break;
                case LedgerChoices.Keep:
                    AdvanceStage(QuestIds.CheckpointKiloTruth, 6,
                        "You kept the ledger. Every number. Every compliance rating. The ledger is the garrison's memory. You are the memory's keeper now.");
                    break;
                case LedgerChoices.GiveMilitia:
                    AdvanceStage(QuestIds.CheckpointKiloTruth, 6,
                        "You gave the ledger to the Militia. The garrison's numbers will be the militia's ammunition.");
                    break;
                case LedgerChoices.GiveCult:
                    AdvanceStage(QuestIds.CheckpointKiloTruth, 6,
                        "You gave the ledger to the Cult. Brother Orin will read every name. The 89 will be remembered in ash and tallow.");
                    break;
            }
        }

        // ── Glow Communion Stage 6 methods ────────────────────────────────

        /// <summary>Give a survivor to the Cult as a permanent convert.</summary>
        public void GiveConvertToCult(string survivorName)
        {
            if (string.IsNullOrEmpty(survivorName)) return;
            var q = GetQuest(QuestIds.GlowCommunion);
            if (q == null || q.IsCompleted || q.IsTerminated || q.CurrentStage < 4) return;
            AdvanceStage(QuestIds.GlowCommunion, 6,
                $"Day {Day}. You gave them {survivorName}. The supplies arrived. The convert is held. The convert does not come back.");
        }

        /// <summary>Refuse the Cult's demand for a permanent convert.</summary>
        public void RefuseCultConvertDemand()
        {
            var q = GetQuest(QuestIds.GlowCommunion);
            if (q == null || q.IsCompleted || q.IsTerminated || q.CurrentStage < 4) return;
            AdvanceStage(QuestIds.GlowCommunion, 6,
                "You refused. The Cult withdrew protection. The warlords noticed. The absence is the weapon.");
            ModifyRaidProbability?.Invoke(0.2f);
        }

        /// <summary>Offer yourself to the Cult as a permanent convert.</summary>
        public void OfferSelfToCult(string survivorName)
        {
            if (string.IsNullOrEmpty(survivorName)) return;
            var q = GetQuest(QuestIds.GlowCommunion);
            if (q == null || q.IsCompleted || q.IsTerminated || q.CurrentStage < 4) return;
            AdvanceStage(QuestIds.GlowCommunion, 6,
                $"Day {Day}. You gave yourself. The supplies arrived. The bunker lost {survivorName}. The math is the same as the militia. The math is always the same.");
        }

        // ───────────────────────────────────────────────────────────────────
        // Daily tick (time-gated triggers with guard flags)
        // ───────────────────────────────────────────────────────────────────

        /// <summary>
        /// Called once per game day by the host. Evaluates all time-gated quest
        /// triggers. Each trigger uses a <c>day >= X</c> window plus a guard flag
        /// so it fires at most once, even if the triggering conditions are met late.
        /// </summary>
        public void TickDaily()
        {
            int d = Day;

            // Glow Communion
            if (d >= QuestDayTriggers.CultReturns  && HasFlag(QuestFlags.CultVisitedBunker)   && !HasFlag(QuestFlags.CultSecondVisitFired))
            { RequestBunkerEvent(QuestEventIds.CultSecondVisit, QuestIds.GlowCommunion); SetFlag(QuestFlags.CultSecondVisitFired); }

            if (d >= QuestDayTriggers.CupOffered   && HasFlag(QuestFlags.CultCeremonyAllowed) && !HasFlag(QuestFlags.CultCupOfferedFired))
            { RequestBunkerEvent(QuestEventIds.CultCupOffered, QuestIds.GlowCommunion); SetFlag(QuestFlags.CultCupOfferedFired); }

            if (d >= QuestDayTriggers.ConvertRequest && HasFlag(QuestFlags.CultCupDrunk)      && !HasFlag(QuestFlags.CultConvertRequested))
            {
                var q = GetQuest(QuestIds.GlowCommunion);
                if (q != null && q.CurrentStage == 3)
                {
                    AdvanceStage(QuestIds.GlowCommunion, 4,
                        $"Day {d}. The convert asks to visit the Chapel. 'The bunker feels cold. The Chapel feels warm.'");
                    SetFlag(QuestFlags.CultConvertRequested);
                }
            }

            if (d >= QuestDayTriggers.PermanentConvert && HasFlag(QuestFlags.CultConvertExists) && !HasFlag(QuestFlags.CultPermanentDemandFired))
            { RequestBunkerEvent(QuestEventIds.CultPermanentConvert, QuestIds.GlowCommunion); SetFlag(QuestFlags.CultPermanentDemandFired); }

            // Grain War
            if (d >= QuestDayTriggers.MilitiaOffer && HasFlag(QuestFlags.MartaReceiptFound) && !HasFlag(QuestFlags.MilitiaOfferFired))
            { TriggerMilitiaOffer(); SetFlag(QuestFlags.MilitiaOfferFired); }

            if (d >= QuestDayTriggers.MartaReturns && HasFlag(QuestFlags.MartaChildrenExtracted) && !HasFlag(QuestFlags.MartaArrivalTriggered))
            { MartaArrivesAtBunker(); SetFlag(QuestFlags.MartaArrivalTriggered); }

            // Tribute Break
            if (d == QuestDayTriggers.FirstThursday && !HasFlag(QuestFlags.WarlordFirstThursday))
                TriggerFirstThursday();

            if (d >= QuestDayTriggers.WarlordEscalation && HasFlag(QuestFlags.TollHouseChildFreed) && !HasFlag(QuestFlags.WarlordEscalationFired))
            { RequestBunkerEvent(QuestEventIds.WarlordEscalation, QuestIds.TributeBreak); SetFlag(QuestFlags.WarlordEscalationFired); }

            // Kilo Truth
            if (d >= QuestDayTriggers.MilitiaOffer && HasFlag(QuestFlags.KiloIntercomBroadcast) && !HasFlag(QuestFlags.GarrisonRespondedToKilo))
            { RequestBunkerEvent(QuestEventIds.GarrisonKiloResponse, QuestIds.CheckpointKiloTruth); SetFlag(QuestFlags.GarrisonRespondedToKilo); }

            // Tower Seven
            if (d >= QuestDayTriggers.TowerSevenBreach && HasFlag(QuestFlags.TowerSevenCodeKnown) && !HasFlag(QuestFlags.TowerSevenBunkerOpen))
                RequestBunkerEvent(QuestEventIds.TowerSevenBreachAttempt, QuestIds.TowerSevenBroadcast);

            // St. Maren
            if (d >= QuestDayTriggers.LenaAnnexVisit && HasFlag(QuestFlags.LenaNurseFound) && !HasFlag(QuestFlags.LenaVisitedAnnex))
                RequestBunkerEvent(QuestEventIds.LenaAnnexVisit, QuestIds.StMarenLastShift);
        }

        // ───────────────────────────────────────────────────────────────────
        // Save / Load
        // ───────────────────────────────────────────────────────────────────

        public ExpansionQuestChainsSave CaptureState()
        {
            var entries = new QuestSaveEntry[_quests.Count];
            int i = 0;
            foreach (var kv in _quests)
            {
                entries[i++] = new QuestSaveEntry
                {
                    QuestId = kv.Value.QuestId,
                    CurrentStage = kv.Value.CurrentStage,
                    IsCompleted = kv.Value.IsCompleted,
                    IsFailed = kv.Value.IsFailed,
                    IsTerminated = kv.Value.IsTerminated,
                    StageDescription = kv.Value.StageDescription
                };
            }
            return new ExpansionQuestChainsSave { Quests = entries };
        }

        public void RestoreState(ExpansionQuestChainsSave save)
        {
            _quests.Clear();
            SeedQuestDefinitions();
            if (save?.Quests == null) return;
            for (int i = 0; i < save.Quests.Length; i++)
            {
                var e = save.Quests[i];
                if (e == null || string.IsNullOrEmpty(e.QuestId)) continue;
                if (_quests.TryGetValue(e.QuestId, out var q))
                {
                    int stage = e.CurrentStage;
                    if (stage < 0) stage = 0;
                    if (stage > q.MaxStages) stage = q.MaxStages;
                    q.CurrentStage = stage;
                    q.IsCompleted = e.IsCompleted;
                    q.IsFailed = e.IsFailed;
                    q.IsTerminated = e.IsTerminated;
                    q.StageDescription = e.StageDescription ?? string.Empty;
                }
            }
        }

        // ───────────────────────────────────────────────────────────────────
        // Internal helpers
        // ───────────────────────────────────────────────────────────────────

        private int Day => _getDay?.Invoke() ?? 0;

        private bool HasFlag(string flag) => _hasFlag?.Invoke(flag) == true;

        private void SetFlag(string flag) => _setFlag?.Invoke(flag, true);

        private void RequestBunkerEvent(string eventId, string questId) =>
            OnRequestBunkerEvent?.Invoke(eventId, questId, Day);

        private void RecordChronicle(string description,
            MoralChronicleEntryKind kind = MoralChronicleEntryKind.ExpeditionMoralFind,
            string survivorName = null)
        {
            if (string.IsNullOrEmpty(description)) return;
            EventBus.Raise(new MoralChronicleEntryRequested(Day, description, kind, survivorName));
        }

        // ───────────────────────────────────────────────────────────────────
        // Quest definition seeding
        // ───────────────────────────────────────────────────────────────────

        private void SeedQuestDefinitions()
        {
            _quests[QuestIds.CheckpointKiloTruth] = new ExpansionQuestState
            {
                QuestId = QuestIds.CheckpointKiloTruth, DisplayName = "The 89",
                Description = "The garrison killed 89 civilians by sealing a door.",
                CurrentStage = 0, MaxStages = 6,
                RewardDescription = "The 89 are not forgotten. The trench looks different now.",
                PrimaryLocation = LocationIds.CheckpointKilo
            };
            _quests[QuestIds.GrainWar] = new ExpansionQuestState
            {
                QuestId = QuestIds.GrainWar, DisplayName = "Marta's Potatoes",
                Description = "The militia liberated the villages and then ate them.",
                CurrentStage = 0, MaxStages = 6,
                RewardDescription = "The militia's tax dropped by 5%. It is not enough. It is something.",
                PrimaryLocation = LocationIds.MartaFarmhouse
            };
            _quests[QuestIds.GlowCommunion] = new ExpansionQuestState
            {
                QuestId = QuestIds.GlowCommunion, DisplayName = "The Cup",
                Description = "The Cult offers comfort. The comfort is real. The leash is also real.",
                CurrentStage = 0, MaxStages = 6,
                RewardDescription = "The Cult's smile is the same whether you join or refuse.",
                PrimaryLocation = LocationIds.GlowChapel
            };
            _quests[QuestIds.TributeBreak] = new ExpansionQuestState
            {
                QuestId = QuestIds.TributeBreak, DisplayName = "Thursday",
                Description = "The warlords come every Thursday. They take. They leave one thing.",
                CurrentStage = 0, MaxStages = 6,
                RewardDescription = "The flashlight is still on the table. The math rebuilds.",
                PrimaryLocation = LocationIds.TollHouse
            };
            _quests[QuestIds.StMarenLastShift] = new ExpansionQuestState
            {
                QuestId = QuestIds.StMarenLastShift, DisplayName = "Agnes",
                Description = "The hospital staff stayed. The patients died. The staff walked out.",
                CurrentStage = 0, MaxStages = 6,
                RewardDescription = "Lena's hands are steadier now. Room 12 is quieter.",
                PrimaryLocation = LocationIds.StMarenAnnex
            };
            _quests[QuestIds.TowerSevenBroadcast] = new ExpansionQuestState
            {
                QuestId = QuestIds.TowerSevenBroadcast, DisplayName = "The Frequency",
                Description = "The operators transmitted the order that killed Tessarat.",
                CurrentStage = 0, MaxStages = 3,
                RewardDescription = "The frequencies are still broadcasting. The survivor voice is still asking. You can answer now.",
                PrimaryLocation = LocationIds.RadioTowerSeven
            };
        }
    }

    // ──────────────────────────────────────────────────────────────────────
    // Data types
    // ──────────────────────────────────────────────────────────────────────

    [Serializable]
    public class ExpansionQuestState
    {
        public string QuestId;
        public string DisplayName;
        public string Description;
        public int CurrentStage;
        public int MaxStages;
        public string RewardDescription;
        public string PrimaryLocation;
        public string StageDescription;
        public bool IsCompleted;
        public bool IsFailed;
        public bool IsTerminated;
    }

    [Serializable]
    public class ExpansionQuestChainsSave
    {
        public QuestSaveEntry[] Quests;
    }

    [Serializable]
    public class QuestSaveEntry
    {
        public string QuestId;
        public int CurrentStage;
        public bool IsCompleted;
        public bool IsFailed;
        public bool IsTerminated;
        public string StageDescription;
    }
}
