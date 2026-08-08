using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Expedition / wasteland milestone perks (Prompts #206–#210).
    /// Earned through max-weight returns, trap/sneak work, city surveys,
    /// night expeditions without light, and forest/swamp scavenging —
    /// not XP grind. Plain C#, save/load safe. Inventory-free
    /// (Survivors asmdef has no Inventory/Core ref).
    /// </summary>
    public class ExpeditionPerkSystem
    {
        // ── Perk ids ─────────────────────────────────────────────────────
        public const string PackMuleId = "perk_pack_mule";
        public const string LightStepId = "perk_light_step";
        public const string UrbanPathfinderId = "perk_urban_pathfinder";
        public const string NightTerrorId = "perk_night_terror";
        public const string ForagerId = "perk_forager";

        // ── Thresholds ───────────────────────────────────────────────────
        public const int MaxWeightReturnsForPackMule = 5;
        public const int TrapsOrSneaksForLightStep = 5;
        public const int CitySurveysForUrbanPathfinder = 10;
        public const int NightNoFlashlightForNightTerror = 5;
        public const int ForestSwampScavengesForForager = 5;

        // ── Effect constants ─────────────────────────────────────────────
        /// <summary>Pack Mule: extra base carry weight (kg).</summary>
        public const float PackMuleCarryBonusKg = 10f;

        /// <summary>Pack Mule: over-encumbrance stamina penalty multiplier (halved).</summary>
        public const float PackMuleOverEncumberPenaltyMult = 0.5f;

        /// <summary>Urban Pathfinder: City/Ruin travel time multiplier (−30%).</summary>
        public const float UrbanPathfinderTravelMult = 0.70f;

        /// <summary>Night Terror: combat damage / success multiplier at night.</summary>
        public const float NightTerrorCombatBonus = 1.50f;

        /// <summary>Night Terror: stealth success / detection avoidance at night.</summary>
        public const float NightTerrorStealthBonus = 1.50f;

        /// <summary>Forager: min/max wild food items when loot table is empty.</summary>
        public const int ForagerMinFood = 1;
        public const int ForagerMaxFood = 2;

        public const string RootsItemId = "roots";
        public const string BerriesItemId = "berries";

        // ── Encounter / terrain tags ──────────────────────────────────────
        public const string EncFeralDogs = "enc_feral_dogs";
        public const string EncFeralDogPack = "enc_dog_pack";
        public const string EncSleepingGhoul = "enc_sleeping_ghoul";

        public const string TagCity = "city";
        public const string TagRuin = "ruin";
        public const string TagUrban = "urban";
        public const string TagForest = "forest";
        public const string TagSwamp = "swamp";

        private SkillProgressionSystem _progression;
        private readonly Dictionary<string, ExpeditionCounters> _bySurvivor =
            new Dictionary<string, ExpeditionCounters>();

        public event Action<Survivor, string> OnExpeditionPerkEarned;
        public event Action<Survivor, string, int> OnMilestoneProgress;

        public void Bind(SkillProgressionSystem progression)
        {
            _progression = progression;
            _progression?.RegisterExpeditionPerks();
        }

        public void RegisterCatalog() => _progression?.RegisterExpeditionPerks();

        // ── Queries ──────────────────────────────────────────────────────

        public bool Has(string survivorId, string perkId)
        {
            if (_progression == null || string.IsNullOrEmpty(survivorId)) return false;
            return _progression.HasActivePerk(survivorId, perkId);
        }

        public bool Has(Survivor sv, string perkId) =>
            sv != null && Has(sv.Id, perkId);

        public ExpeditionCounters GetCounters(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return new ExpeditionCounters();
            return GetOrCreate(survivorId).Clone();
        }

        // ── #206 Pack Mule ───────────────────────────────────────────────

        /// <summary>
        /// Record a completed expedition return. Counts toward Pack Mule when
        /// current weight is at (or over) maximum carrying capacity.
        /// </summary>
        public void RecordMaxWeightReturn(
            Survivor sv,
            float currentWeight,
            float carryingCapacity,
            int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            if (carryingCapacity <= 0f) return;
            // At capacity: weight within 0.01 kg of cap, or at/over (push-your-luck full packs).
            if (currentWeight + 0.01f < carryingCapacity) return;

            var c = GetOrCreate(sv.Id);
            c.MaxWeightReturns++;
            OnMilestoneProgress?.Invoke(sv, "max_weight_returns", c.MaxWeightReturns);
            if (c.MaxWeightReturns >= MaxWeightReturnsForPackMule)
                TryGrant(sv, PackMuleId, currentDay);
        }

        public bool HasPackMule(Survivor sv) => Has(sv, PackMuleId);

        /// <summary>Base capacity plus Pack Mule +10 kg when earned.</summary>
        public float GetCarryCapacityBonus(Survivor sv) =>
            HasPackMule(sv) ? PackMuleCarryBonusKg : 0f;

        /// <summary>
        /// Multiplier on the over-encumbrance portion of stamina drain.
        /// Pack Mule → 0.5 (halved penalty).
        /// </summary>
        public float GetOverEncumberPenaltyMultiplier(Survivor sv) =>
            HasPackMule(sv) ? PackMuleOverEncumberPenaltyMult : 1f;

        // ── #207 Light Step ──────────────────────────────────────────────

        /// <summary>Successful wasteland trap disarm counts toward Light Step.</summary>
        public void RecordTrapDisarmed(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.TrapsDisarmed++;
            c.TrapsOrSneaks = c.TrapsDisarmed + c.SneaksPast;
            OnMilestoneProgress?.Invoke(sv, "traps_or_sneaks", c.TrapsOrSneaks);
            if (c.TrapsOrSneaks >= TrapsOrSneaksForLightStep)
                TryGrant(sv, LightStepId, currentDay);
        }

        /// <summary>Successfully sneaking past an encounter counts toward Light Step.</summary>
        public void RecordSneakPast(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.SneaksPast++;
            c.TrapsOrSneaks = c.TrapsDisarmed + c.SneaksPast;
            OnMilestoneProgress?.Invoke(sv, "traps_or_sneaks", c.TrapsOrSneaks);
            if (c.TrapsOrSneaks >= TrapsOrSneaksForLightStep)
                TryGrant(sv, LightStepId, currentDay);
        }

        public bool HasLightStep(Survivor sv) => Has(sv, LightStepId);

        /// <summary>Light Step scavengers never raise scavenging Noise events.</summary>
        public bool SuppressesScavengeNoise(Survivor sv) => HasLightStep(sv);

        /// <summary>
        /// Completely bypass Feral Dog / Sleeping Ghoul encounters (no stealth roll).
        /// </summary>
        public bool CanBypassEncounter(Survivor sv, string encounterId)
        {
            if (!HasLightStep(sv) || string.IsNullOrEmpty(encounterId)) return false;
            return IsLightStepBypassEncounter(encounterId);
        }

        public static bool IsLightStepBypassEncounter(string encounterId)
        {
            if (string.IsNullOrEmpty(encounterId)) return false;
            return string.Equals(encounterId, EncFeralDogs, StringComparison.OrdinalIgnoreCase)
                   || string.Equals(encounterId, EncFeralDogPack, StringComparison.OrdinalIgnoreCase)
                   || string.Equals(encounterId, EncSleepingGhoul, StringComparison.OrdinalIgnoreCase)
                   || encounterId.IndexOf("feral_dog", StringComparison.OrdinalIgnoreCase) >= 0
                   || encounterId.IndexOf("sleeping_ghoul", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        // ── #208 Urban Pathfinder ────────────────────────────────────────

        /// <summary>Fully surveying a City map node counts toward Urban Pathfinder.</summary>
        public void RecordCityNodeSurvey(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.CityNodesSurveyed++;
            OnMilestoneProgress?.Invoke(sv, "city_nodes_surveyed", c.CityNodesSurveyed);
            if (c.CityNodesSurveyed >= CitySurveysForUrbanPathfinder)
                TryGrant(sv, UrbanPathfinderId, currentDay);
        }

        public bool HasUrbanPathfinder(Survivor sv) => Has(sv, UrbanPathfinderId);

        /// <summary>
        /// Travel-time multiplier for City/Ruin nodes (0.7 with perk, else 1).
        /// Stacks with Bicycle (#68) — callers multiply both.
        /// </summary>
        public float GetCityRuinTravelMultiplier(Survivor sv, bool isCityOrRuin)
        {
            if (!isCityOrRuin || !HasUrbanPathfinder(sv)) return 1f;
            return UrbanPathfinderTravelMult;
        }

        public static bool IsCityOrRuinTags(IList<string> tags)
        {
            if (tags == null) return false;
            for (int i = 0; i < tags.Count; i++)
            {
                string t = tags[i];
                if (string.IsNullOrEmpty(t)) continue;
                if (string.Equals(t, TagCity, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(t, TagRuin, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(t, TagUrban, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public static bool IsCityOrRuinNode(IList<string> tags, string ringName = null)
        {
            if (IsCityOrRuinTags(tags)) return true;
            if (string.IsNullOrEmpty(ringName)) return false;
            return ringName.IndexOf("city", StringComparison.OrdinalIgnoreCase) >= 0
                   || ringName.IndexOf("outskirt", StringComparison.OrdinalIgnoreCase) >= 0
                   || ringName.IndexOf("ruin", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        // ── #209 Night Terror ────────────────────────────────────────────

        /// <summary>
        /// Surviving a night expedition without a flashlight counts toward Night Terror.
        /// </summary>
        public void RecordNightExpeditionNoFlashlight(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.NightNoFlashlightSurvived++;
            OnMilestoneProgress?.Invoke(sv, "night_no_flashlight", c.NightNoFlashlightSurvived);
            if (c.NightNoFlashlightSurvived >= NightNoFlashlightForNightTerror)
                TryGrant(sv, NightTerrorId, currentDay);
        }

        public bool HasNightTerror(Survivor sv) => Has(sv, NightTerrorId);

        /// <summary>Combat multiplier at night (1.5 with Night Terror).</summary>
        public float GetNightCombatMultiplier(Survivor sv, bool isNight)
        {
            if (!isNight || !HasNightTerror(sv)) return 1f;
            return NightTerrorCombatBonus;
        }

        /// <summary>Stealth multiplier at night (1.5 with Night Terror).</summary>
        public float GetNightStealthMultiplier(Survivor sv, bool isNight)
        {
            if (!isNight || !HasNightTerror(sv)) return 1f;
            return NightTerrorStealthBonus;
        }

        /// <summary>Zero morale penalty from total darkness / Listless drain.</summary>
        public bool IgnoresDarknessMorale(Survivor sv) => HasNightTerror(sv);

        // ── #210 Forager ─────────────────────────────────────────────────

        /// <summary>Scavenging a Forest or Swamp map node counts toward Forager.</summary>
        public void RecordForestOrSwampScavenge(Survivor sv, int currentDay = 0)
        {
            if (sv == null || !sv.IsAlive) return;
            var c = GetOrCreate(sv.Id);
            c.ForestSwampScavenges++;
            OnMilestoneProgress?.Invoke(sv, "forest_swamp_scavenges", c.ForestSwampScavenges);
            if (c.ForestSwampScavenges >= ForestSwampScavengesForForager)
                TryGrant(sv, ForagerId, currentDay);
        }

        public bool HasForager(Survivor sv) => Has(sv, ForagerId);

        /// <summary>
        /// When loot is empty, Forager still brings 1–2 Roots or Berries.
        /// Returns how many food items to add (0 if no perk / loot already present).
        /// </summary>
        public int GetForagerGuaranteedFoodCount(
            Survivor sv,
            int existingLootCount,
            System.Random rng = null)
        {
            if (!HasForager(sv) || existingLootCount > 0) return 0;
            rng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("expeditionperksystem");
            return rng.Next(ForagerMinFood, ForagerMaxFood + 1);
        }

        public static bool IsForestOrSwampTags(IList<string> tags)
        {
            if (tags == null) return false;
            for (int i = 0; i < tags.Count; i++)
            {
                string t = tags[i];
                if (string.IsNullOrEmpty(t)) continue;
                if (string.Equals(t, TagForest, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(t, TagSwamp, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>Pick roots or berries id for a forager food roll.</summary>
        public static string PickForagerFoodId(System.Random rng)
        {
            rng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("expeditionperksystem");
            return rng.NextDouble() < 0.5 ? RootsItemId : BerriesItemId;
        }

        // ── Grant helper ─────────────────────────────────────────────────

        private bool TryGrant(Survivor sv, string perkId, int currentDay)
        {
            if (_progression == null || sv == null) return false;
            if (_progression.HasActivePerk(sv.Id, perkId)
                || _progression.HasDormantPerk(sv.Id, perkId))
                return false;

            bool granted = _progression.TryGrantPerk(sv, perkId, currentDay);
            if (granted)
                OnExpeditionPerkEarned?.Invoke(sv, perkId);
            return granted;
        }

        private ExpeditionCounters GetOrCreate(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var c))
            {
                c = new ExpeditionCounters();
                _bySurvivor[survivorId] = c;
            }
            return c;
        }

        // ── Save / Load ──────────────────────────────────────────────────

        public ExpeditionPerkSave CaptureState()
        {
            var save = new ExpeditionPerkSave { Entries = new List<ExpeditionCounterSave>() };
            foreach (var kv in _bySurvivor)
            {
                var c = kv.Value;
                save.Entries.Add(new ExpeditionCounterSave
                {
                    SurvivorId = kv.Key,
                    MaxWeightReturns = c.MaxWeightReturns,
                    TrapsDisarmed = c.TrapsDisarmed,
                    SneaksPast = c.SneaksPast,
                    CityNodesSurveyed = c.CityNodesSurveyed,
                    NightNoFlashlightSurvived = c.NightNoFlashlightSurvived,
                    ForestSwampScavenges = c.ForestSwampScavenges
                });
            }
            return save;
        }

        public void RestoreState(ExpeditionPerkSave save)
        {
            _bySurvivor.Clear();
            if (save?.Entries == null) return;
            for (int i = 0; i < save.Entries.Count; i++)
            {
                var e = save.Entries[i];
                if (e == null || string.IsNullOrEmpty(e.SurvivorId)) continue;
                _bySurvivor[e.SurvivorId] = new ExpeditionCounters
                {
                    MaxWeightReturns = e.MaxWeightReturns,
                    TrapsDisarmed = e.TrapsDisarmed,
                    SneaksPast = e.SneaksPast,
                    TrapsOrSneaks = e.TrapsDisarmed + e.SneaksPast,
                    CityNodesSurveyed = e.CityNodesSurveyed,
                    NightNoFlashlightSurvived = e.NightNoFlashlightSurvived,
                    ForestSwampScavenges = e.ForestSwampScavenges
                };
            }
        }

        public sealed class ExpeditionCounters
        {
            public int MaxWeightReturns;
            public int TrapsDisarmed;
            public int SneaksPast;
            public int TrapsOrSneaks;
            public int CityNodesSurveyed;
            public int NightNoFlashlightSurvived;
            public int ForestSwampScavenges;

            public ExpeditionCounters Clone() => new ExpeditionCounters
            {
                MaxWeightReturns = MaxWeightReturns,
                TrapsDisarmed = TrapsDisarmed,
                SneaksPast = SneaksPast,
                TrapsOrSneaks = TrapsOrSneaks,
                CityNodesSurveyed = CityNodesSurveyed,
                NightNoFlashlightSurvived = NightNoFlashlightSurvived,
                ForestSwampScavenges = ForestSwampScavenges
            };
        }
    }

    [Serializable]
    public class ExpeditionPerkSave
    {
        public List<ExpeditionCounterSave> Entries = new List<ExpeditionCounterSave>();
    }

    [Serializable]
    public class ExpeditionCounterSave
    {
        public string SurvivorId;
        public int MaxWeightReturns;
        public int TrapsDisarmed;
        public int SneaksPast;
        public int CityNodesSurveyed;
        public int NightNoFlashlightSurvived;
        public int ForestSwampScavenges;
    }
}
