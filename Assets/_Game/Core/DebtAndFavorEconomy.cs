#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion VII — The Favor Economy. Bartering scrap for antibiotics is for
    /// strangers. Factions deal in Favors. When a faction saves your bunker,
    /// you incur a Blood Debt. The collection is always worse than the debt.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class DebtAndFavorEconomy
    {
        // ── Favor types ───────────────────────────────────────────────
        public const string Favor_TitheOfHands = "favor_tithe_of_hands";
        public const string Favor_IronLeash = "favor_iron_leash";
        public const string Favor_GlowsEmbrace = "favor_glows_embrace";
        public const string Favor_WarlordsCoin = "favor_warlords_coin";

        // ── Collection constants ──────────────────────────────────────
        public const int TitheOfHands_LaborDays = 14;
        public const string TitheOfHands_Affliction = "affliction_trench_foot";

        public const string IronLeash_QuestId = "quest_assassination_deserter";

        public const string GlowsEmbrace_RiteLocation = "location_cathedral_st_jude";

        public const string WarlordsCoin_TapeItemId = "cassette_tape";

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnFavorIncurred;
        public event Action<string> OnFavorCollected;
        public event Action<string> OnFavorRefused;
        public event Action<string, string> OnCollectionConsequence;

        private readonly List<FavorEntry> _activeFavors = new List<FavorEntry>();
        private readonly List<FavorEntry> _collectedFavors = new List<FavorEntry>();

        public IReadOnlyList<FavorEntry> ActiveFavors => _activeFavors;
        public IReadOnlyList<FavorEntry> CollectedFavors => _collectedFavors;
        public int TotalFavorsIncurred => _activeFavors.Count + _collectedFavors.Count;

        // ── Favor incurrence ──────────────────────────────────────────

        /// <summary>
        /// Incur a Blood Debt with a faction. They saved you; now you owe them.
        /// </summary>
        public FavorEntry IncurFavor(string favorType, string factionId,
            string description, int currentDay)
        {
            var entry = new FavorEntry
            {
                FavorType = favorType,
                FactionId = factionId,
                Description = description,
                IncurredDay = currentDay,
                IsCollected = false
            };

            _activeFavors.Add(entry);
            OnFavorIncurred?.Invoke(favorType);
            return entry;
        }

        // ── Favor collection ──────────────────────────────────────────

        /// <summary>
        /// The Tithe of Hands: Militia demands 2 survivors for 14 days forced labor.
        /// They return with TrenchFoot and Trauma.
        /// </summary>
        public TitheResult CollectTitheOfHands(List<string> survivorIds, int currentDay)
        {
            var favor = FindActive(Favor_TitheOfHands);
            if (favor == null) return new TitheResult { Success = false };

            MarkCollected(favor);

            return new TitheResult
            {
                Success = true,
                SurvivorIds = survivorIds,
                LaborDays = TitheOfHands_LaborDays,
                AfflictionOnReturn = TitheOfHands_Affliction,
                Message = "The Militia arrives. They don't want grain. They want hands. " +
                    "Two survivors are marched to the grain exchange. They will return " +
                    "in 14 days with trench foot and a thousand-yard stare."
            };
        }

        /// <summary>
        /// The Iron Leash: Garrison demands you act as their Hounds.
        /// Accept the assassination quest or the tracker in your armor detonates.
        /// </summary>
        public IronLeashResult CollectIronLeash(bool acceptQuest, int currentDay)
        {
            var favor = FindActive(Favor_IronLeash);
            if (favor == null) return new IronLeashResult { Success = false };

            MarkCollected(favor);

            if (acceptQuest)
            {
                return new IronLeashResult
                {
                    Success = true,
                    QuestAssigned = IronLeash_QuestId,
                    Message = "The Garrison's Hounds. You hunt their deserters. " +
                        "The armor you wear has a tracker. They know where you sleep."
                };
            }
            else
            {
                return new IronLeashResult
                {
                    Success = true,
                    TrackerDetonated = true,
                    Message = "The Garrison remotely detonates the tracker in your armor. " +
                        "The explosion is small. The shrapnel is not."
                };
            }
        }

        /// <summary>
        /// The Glow's Embrace: Cult demands the child for the Rite of Ash.
        /// Refuse and the child tries to sneak out.
        /// </summary>
        public GlowResult CollectGlowsEmbrace(bool surrenderChild, int currentDay)
        {
            var favor = FindActive(Favor_GlowsEmbrace);
            if (favor == null) return new GlowResult { Success = false };

            MarkCollected(favor);

            if (surrenderChild)
            {
                return new GlowResult
                {
                    Success = true,
                    ChildTaken = true,
                    Message = "The child is brought to the Cathedral. The Rite of Ash. " +
                        "The child returns changed. They do not speak of what happened. " +
                        "They hum the Cult's hymns in their sleep."
                };
            }
            else
            {
                return new GlowResult
                {
                    Success = true,
                    ChildAttemptsEscape = true,
                    Message = "The Cult marks your bunker. The child, now a teenager, " +
                        "will attempt to sneak out of the hatch to 'return to the light.'"
                };
            }
        }

        /// <summary>
        /// The Warlord's Coin: Warlord demands a hostage.
        /// Day 80: cassette tape returned with defenses listed.
        /// </summary>
        public WarlordResult CollectWarlordsCoin(string hostageId, int currentDay)
        {
            var favor = FindActive(Favor_WarlordsCoin);
            if (favor == null) return new WarlordResult { Success = false };

            MarkCollected(favor);

            return new WarlordResult
            {
                Success = true,
                HostageId = hostageId,
                TapeReturnDay = currentDay + 20,
                Message = "The Warlord's runner takes " + hostageId + ". " +
                    "They are dragged into the ash. On Day " + (currentDay + 20) +
                    ", a cassette tape is left at the hatch. It's the survivor, " +
                    "reading your defenses, begging you to open the door."
            };
        }

        // ── Refusal consequences ──────────────────────────────────────

        /// <summary>Refuse to pay a favor. The faction retaliates.</summary>
        public bool RefuseFavor(string favorType)
        {
            var favor = FindActive(favorType);
            if (favor == null) return false;

            favor.WasRefused = true;
            MarkCollected(favor);
            OnFavorRefused?.Invoke(favorType);
            return true;
        }

        // ── Queries ───────────────────────────────────────────────────

        public bool HasActiveFavor(string favorType) => FindActive(favorType) != null;
        public int ActiveFavorCount => _activeFavors.Count;

        private FavorEntry FindActive(string favorType)
        {
            for (int i = 0; i < _activeFavors.Count; i++)
                if (_activeFavors[i].FavorType == favorType && !_activeFavors[i].IsCollected)
                    return _activeFavors[i];
            return null;
        }

        private void MarkCollected(FavorEntry entry)
        {
            entry.IsCollected = true;
            _activeFavors.Remove(entry);
            _collectedFavors.Add(entry);
            OnFavorCollected?.Invoke(entry.FavorType);
        }

        // ── Save / Load ───────────────────────────────────────────────

        public FavorEconomySave CaptureState()
        {
            var active = new FavorEntrySave[_activeFavors.Count];
            for (int i = 0; i < _activeFavors.Count; i++)
                active[i] = SaveEntry(_activeFavors[i]);
            var collected = new FavorEntrySave[_collectedFavors.Count];
            for (int i = 0; i < _collectedFavors.Count; i++)
                collected[i] = SaveEntry(_collectedFavors[i]);
            return new FavorEconomySave { ActiveFavors = active, CollectedFavors = collected };
        }

        public void RestoreState(FavorEconomySave save)
        {
            _activeFavors.Clear();
            _collectedFavors.Clear();
            if (save == null) return;
            if (save.ActiveFavors != null)
                for (int i = 0; i < save.ActiveFavors.Length; i++)
                    if (save.ActiveFavors[i] != null)
                        _activeFavors.Add(LoadEntry(save.ActiveFavors[i]));
            if (save.CollectedFavors != null)
                for (int i = 0; i < save.CollectedFavors.Length; i++)
                    if (save.CollectedFavors[i] != null)
                        _collectedFavors.Add(LoadEntry(save.CollectedFavors[i]));
        }

        private static FavorEntrySave SaveEntry(FavorEntry e) => new FavorEntrySave
        {
            FavorType = e.FavorType, FactionId = e.FactionId, Description = e.Description,
            IncurredDay = e.IncurredDay, IsCollected = e.IsCollected, WasRefused = e.WasRefused
        };

        private static FavorEntry LoadEntry(FavorEntrySave s) => new FavorEntry
        {
            FavorType = s.FavorType, FactionId = s.FactionId, Description = s.Description,
            IncurredDay = s.IncurredDay, IsCollected = s.IsCollected, WasRefused = s.WasRefused
        };
    }

    public class FavorEntry
    {
        public string FavorType;
        public string FactionId;
        public string Description;
        public int IncurredDay;
        public bool IsCollected;
        public bool WasRefused;
    }

    [Serializable]
    public class TitheResult
    {
        public bool Success;
        public List<string> SurvivorIds;
        public int LaborDays;
        public string AfflictionOnReturn;
        public string Message;
    }

    [Serializable]
    public class IronLeashResult
    {
        public bool Success;
        public string QuestAssigned;
        public bool TrackerDetonated;
        public string Message;
    }

    [Serializable]
    public class GlowResult
    {
        public bool Success;
        public bool ChildTaken;
        public bool ChildAttemptsEscape;
        public string Message;
    }

    [Serializable]
    public class WarlordResult
    {
        public bool Success;
        public string HostageId;
        public int TapeReturnDay;
        public string Message;
    }

    [Serializable]
    public class FavorEconomySave
    {
        public FavorEntrySave[] ActiveFavors;
        public FavorEntrySave[] CollectedFavors;
    }

    [Serializable]
    public class FavorEntrySave
    {
        public string FavorType;
        public string FactionId;
        public string Description;
        public int IncurredDay;
        public bool IsCollected;
        public bool WasRefused;
    }
}
