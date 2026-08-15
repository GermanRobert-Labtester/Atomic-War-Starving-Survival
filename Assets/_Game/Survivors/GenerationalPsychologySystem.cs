using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    public struct ComingOfAgeEvent
    {
        public string SurvivorId;
        public string DisplayName;
        public int Age;
        public bool IsBunkerBorn;

        public ComingOfAgeEvent(string survivorId, string displayName, int age, bool isBunkerBorn)
        {
            SurvivorId = survivorId;
            DisplayName = displayName;
            Age = age;
            IsBunkerBorn = isBunkerBorn;
        }
    }

    public struct ArtifactReverenceBrawlEvent
    {
        public string InstigatorId;
        public string ItemId;

        public ArtifactReverenceBrawlEvent(string instigatorId, string itemId)
        {
            InstigatorId = instigatorId;
            ItemId = itemId;
        }
    }

    /// <summary>
    /// Expansion IV — Chapter 38.2 Generational Psychology & The Bunker-Born.
    /// Tracks DaysSinceExchange and Survivor.Age on the daily tick.
    /// When a child ages up, fires Event_ComingOfAge with the BunkerBorn payload,
    /// permanently altering their Trait and Perk arrays.
    /// </summary>
    public class GenerationalPsychologySystem
    {
        public const string Trait_BunkerBorn = "trait_bunker_born";
        public const string Trait_AgoraphobiaSevere = "trait_agoraphobia_severe";
        public const string Trait_ArtifactReverence = "trait_artifact_reverence";
        public const string MentalBreak_PanicAttack = "mental_break_panic_attack";

        public const string Item_CassetteTape = "item_cassette_tape";
        public const string Item_PreWarPhotoAlbum = "item_pre_war_photo_album";
        public const string Item_VinylCollection = "item_vinyl_collection";

        private NeedsSystem _needsSystem;
        private MentalBreakSystem _mentalBreakSystem;

        public event Action<Survivor, ComingOfAgeEvent> OnComingOfAge;
        public event Action<ComingOfAgeEvent> OnComingOfAgeEventBus;
        public event Action<ArtifactReverenceBrawlEvent> OnArtifactReverenceBrawlEventBus;
        public event Action<Survivor> OnAgoraphobicPanicAttack;

        /// <summary>
        /// Most recent coming-of-age description, ready for the HUD generational readout footer.
        /// Cleared by the HUD controller after display. Thread-safe for poll reads on the main thread.
        /// </summary>
        public string LastComingOfAgeEvent { get; private set; } = string.Empty;

        private int _comingOfAgeCount;
        private int _agoraphobicPanicCount;
        private int _artifactReverenceBrawlCount;

        public int ComingOfAgeCount => _comingOfAgeCount;
        public int AgoraphobicPanicCount => _agoraphobicPanicCount;
        public int ArtifactReverenceBrawlCount => _artifactReverenceBrawlCount;

        public void BindDependencies(NeedsSystem needsSystem, MentalBreakSystem mentalBreakSystem)
        {
            _needsSystem = needsSystem;
            _mentalBreakSystem = mentalBreakSystem;
        }

        /// <summary>
        /// Daily tick evaluating survivor age transitions and coming of age events.
        /// </summary>
        public void DailyTick(int daysSinceExchange, IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return;

            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;

                // Daily age progression check (e.g. 1 year per 365 days or milestone transition)
                if (daysSinceExchange > 0 && (daysSinceExchange % 365 == 0))
                {
                    sv.Age++;
                }

                // Transition when child reaches adult age (age 12-18 or when marked child)
                if (sv.IsChild && sv.Age >= 12)
                {
                    sv.IsChild = false;
                    sv.IsBunkerBorn = true;

                    if (!sv.HasTrait(Trait_BunkerBorn))
                        sv.Traits.Add(Trait_BunkerBorn);

                    if (!sv.HasTrait(Trait_AgoraphobiaSevere))
                        sv.Traits.Add(Trait_AgoraphobiaSevere);

                    if (!sv.HasTrait(Trait_ArtifactReverence))
                        sv.Traits.Add(Trait_ArtifactReverence);

                    // Add perk bonuses for bunker nav
                    sv.ProgressionSurvivalBonus = Mathf.Max(sv.ProgressionSurvivalBonus, 0.2f);

                    var comingOfAgePayload = new ComingOfAgeEvent(sv.Id, sv.DisplayName, sv.Age, true);
                    LastComingOfAgeEvent = $"{sv.DisplayName} (age {sv.Age}) has grown up inside. They have never seen the sky.";
                    _comingOfAgeCount++;
                    OnComingOfAge?.Invoke(sv, comingOfAgePayload);
                    OnComingOfAgeEventBus?.Invoke(comingOfAgePayload);
                }
            }
        }

        /// <summary>
        /// Evaluates forcing a bunker-born survivor onto a surface expedition.
        /// Triggers MentalBreak_PanicAttack if they suffer from agoraphobia.
        /// </summary>
        public bool EvaluateExpeditionDeployment(Survivor survivor)
        {
            if (survivor == null || !survivor.IsAlive) return true;

            if (survivor.IsBunkerBorn || survivor.HasTrait(Trait_AgoraphobiaSevere))
            {
                if (_mentalBreakSystem != null)
                {
                    survivor.currentMentalBreakId = MentalBreak_PanicAttack;
                }

                if (_needsSystem != null)
                {
                    _needsSystem.Modify(survivor, NeedKind.Morale, -35f);
                }

                OnAgoraphobicPanicAttack?.Invoke(survivor);
                _agoraphobicPanicCount++;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Calculates bonus trade value for pre-war relics for Bunker-Born survivors.
        /// </summary>
        public float GetRelicTradeValueMultiplier(Survivor survivor, string itemId)
        {
            if (survivor == null || !IsRelicItem(itemId)) return 1.0f;
            if (survivor.IsBunkerBorn || survivor.HasTrait(Trait_ArtifactReverence))
            {
                return 3.5f; // Massive reverence bonus
            }
            return 1.0f;
        }

        /// <summary>
        /// Interacting with or dismantling pre-war relics. If dismantled, triggers Event_Brawl among reverence survivors.
        /// </summary>
        public void EvaluateRelicDismantle(Survivor worker, string itemId, IReadOnlyList<Survivor> roomSurvivors)
        {
            if (worker == null || !IsRelicItem(itemId) || roomSurvivors == null) return;

            for (int i = 0; i < roomSurvivors.Count; i++)
            {
                var sv = roomSurvivors[i];
                if (sv == null || !sv.IsAlive) continue;

                if (sv.HasTrait(Trait_ArtifactReverence) || sv.IsBunkerBorn)
                {
                    if (_needsSystem != null)
                    {
                        _needsSystem.Modify(sv, NeedKind.Morale, -30f);
                    }

                    _artifactReverenceBrawlCount++;
                    OnArtifactReverenceBrawlEventBus?.Invoke(new ArtifactReverenceBrawlEvent(sv.Id, itemId));
                }
            }
        }

        public static bool IsRelicItem(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            return itemId == Item_CassetteTape || itemId == Item_PreWarPhotoAlbum || itemId == Item_VinylCollection;
        }

        public object CaptureState()
        {
            return new GenerationalPsychologySave
            {
                LastComingOfAgeEvent = LastComingOfAgeEvent,
                ComingOfAgeCount = _comingOfAgeCount,
                AgoraphobicPanicCount = _agoraphobicPanicCount,
                ArtifactReverenceBrawlCount = _artifactReverenceBrawlCount
            };
        }

        public void RestoreState(object state)
        {
            if (state is not GenerationalPsychologySave save) return;
            LastComingOfAgeEvent = save.LastComingOfAgeEvent ?? string.Empty;
            _comingOfAgeCount = save.ComingOfAgeCount;
            _agoraphobicPanicCount = save.AgoraphobicPanicCount;
            _artifactReverenceBrawlCount = save.ArtifactReverenceBrawlCount;
        }
    }

    [Serializable]
    public class GenerationalPsychologySave
    {
        public string LastComingOfAgeEvent;
        public int ComingOfAgeCount;
        public int AgoraphobicPanicCount;
        public int ArtifactReverenceBrawlCount;
    }
}
