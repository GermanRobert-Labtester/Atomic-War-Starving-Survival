using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Mutated Ecosystem — Fallout Flora & Fauna (Prompt #67). Fallout weather
    /// causes wild vegetation and creatures outdoors to mutate over radiation
    /// exposure days. Scavengers encounter mutated flora (toxic yield vs rare
    /// chem ingredients) and hostile irradiated fauna. The outdoors ecosystem
    /// degrades and becomes increasingly dangerous over time.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class MutatedEcosystemSystem
    {
        /// <summary>Days of cumulative radiation exposure before Stage 1 mutations appear.</summary>
        public const int Stage1MutationDays = 10;

        /// <summary>Days before Stage 2 (aggressive fauna).</summary>
        public const int Stage2MutationDays = 25;

        /// <summary>Days before Stage 3 (lethal ecosystem).</summary>
        public const int Stage3MutationDays = 50;

        /// <summary>Encounter id for mutated flora discovery.</summary>
        public const string MutatedFloraEncounterId = "enc_mutated_flora";

        /// <summary>Encounter id for hostile fauna attack.</summary>
        public const string HostileFaunaEncounterId = "enc_hostile_fauna";

        /// <summary>Encounter id for late-stage apex predator.</summary>
        public const string ApexPredatorEncounterId = "enc_apex_predator";

        /// <summary>Item id for toxic mutated plant (dangerous).</summary>
        public const string ToxicFloraItemId = "toxic_mutated_flora";

        /// <summary>Item id for rare chem from mutated plant (valuable).</summary>
        public const string RareChemItemId = "mutagenic_extract";

        /// <summary>Chance of encountering mutated flora per expedition tick (base, Stage 1+).</summary>
        public const float FloraEncounterChancePerTick = 0.08f;

        /// <summary>Chance of hostile fauna per tick (Stage 2+).</summary>
        public const float FaunaEncounterChancePerTick = 0.12f;

        /// <summary>Chance of apex predator per tick (Stage 3).</summary>
        public const float ApexEncounterChancePerTick = 0.04f;

        /// <summary>Health damage from hostile fauna attack.</summary>
        public const float FaunaAttackHealthDamage = 20f;

        /// <summary>Health damage from apex predator attack.</summary>
        public const float ApexAttackHealthDamage = 45f;

        /// <summary>Fatigue cost of fleeing fauna.</summary>
        public const float FaunaFleeFatigueCost = 15f;

        /// <summary>Toxic flora contamination added to gathered loot.</summary>
        public const float ToxicFloraContamination = 0.4f;

        /// <summary>Days of post-exchange radiation accumulated.</summary>
        private float _radiationExposureDays;

        /// <summary>Current mutation stage (0=none, 1=flora, 2=fauna, 3=apex).</summary>
        public int MutationStage
        {
            get
            {
                if (_radiationExposureDays >= Stage3MutationDays) return 3;
                if (_radiationExposureDays >= Stage2MutationDays) return 2;
                if (_radiationExposureDays >= Stage1MutationDays) return 1;
                return 0;
            }
        }

        /// <summary>Cumulative radiation exposure days.</summary>
        public float RadiationExposureDays => _radiationExposureDays;

        /// <summary>True when the ecosystem has begun mutating.</summary>
        public bool IsMutated => MutationStage >= 1;
        public bool HasHostileFauna => MutationStage >= 2;
        public bool HasApexPredators => MutationStage >= 3;

        private readonly System.Random _rng;
        private RadiationSystem _radiation;

        // -- Events --
        public event Action<int> OnMutationStageAdvanced;   // newStage
#pragma warning disable CS0067 // Event is part of the public API; consumers may subscribe in future.
        public event Action<ExpeditionState> OnFloraEncountered;
#pragma warning restore CS0067
        public event Action<ExpeditionState, bool> OnFaunaEncountered; // exp, wasApex

        public MutatedEcosystemSystem(System.Random rng = null)
        {
            _rng = rng ?? AtomicWar._Game.Utilities.SeededRandom.CreateFixed("mutated_ecosystem");
        }

        public void BindRadiation(RadiationSystem radiation) => _radiation = radiation;

        /// <summary>
        /// Advance mutation stage based on radiation days. Call daily.
        /// </summary>
        public void TickDaily(float outdoorRadLevel, bool hasExchangeTriggered)
        {
            if (!hasExchangeTriggered) return;

            // Accumulate radiation exposure days weighted by outdoor rad level.
            float dayWeight = Mathf.Clamp01(outdoorRadLevel / 50f);
            _radiationExposureDays += dayWeight;

            // Stage transition events (fire once).
            int prevStage = MutationStage;
            // Re-evaluate after accumulation.
            int newStage = _radiationExposureDays >= Stage3MutationDays ? 3
                : _radiationExposureDays >= Stage2MutationDays ? 2
                : _radiationExposureDays >= Stage1MutationDays ? 1 : 0;

            if (newStage > prevStage)
            {
                for (int s = prevStage + 1; s <= newStage; s++)
                    OnMutationStageAdvanced?.Invoke(s);
            }
        }

        /// <summary>
        /// Roll for ecosystem encounter during an expedition tick. Called from
        /// ExpeditionSystem.ProcessSingleTick.
        /// Returns: 0 = no encounter, 1 = flora, 2 = fauna, 3 = apex.
        /// </summary>
        public int RollEcosystemEncounter()
        {
            int stage = MutationStage;
            if (stage < 1) return 0;

            // Stage 3: apex predators possible.
            if (stage >= 3 && _rng.NextDouble() < ApexEncounterChancePerTick)
                return 3;

            // Stage 2+: hostile fauna.
            if (stage >= 2 && _rng.NextDouble() < FaunaEncounterChancePerTick)
                return 2;

            // Stage 1+: mutated flora.
            if (_rng.NextDouble() < FloraEncounterChancePerTick)
                return 1;

            return 0;
        }

        /// <summary>
        /// Process a mutated flora encounter. Returns harvested items.
        /// 60% chance toxic (contamination), 40% chance rare chem extract.
        /// </summary>
        public Inventory.ItemDefinition HarvestFlora()
        {
            bool isToxic = _rng.NextDouble() < 0.6f;
            string itemId = isToxic ? ToxicFloraItemId : RareChemItemId;
            string displayName = isToxic ? "Toxic Mutated Flora" : "Mutagenic Extract";

            var item = ScriptableObject.CreateInstance<Inventory.ItemDefinition>();
            item.id = itemId;
            item.displayName = displayName;
            item.description = isToxic
                ? "Glowing plant matter. Beautiful, but the geiger screams at it."
                : "A rare biochemical compound. Pre-war labs would have killed for this.";
            item.type = isToxic ? Inventory.ItemType.Material : Inventory.ItemType.Medical;
            item.stackMax = isToxic ? 10 : 3;
            item.weight = isToxic ? 2f : 0.5f;
            item.contamination = isToxic ? ToxicFloraContamination : 0.05f;
            item.tradeValue = isToxic ? 2f : 25f;
            return item;
        }

        /// <summary>
        /// Process a hostile fauna encounter. Applies damage and fatigue.
        /// Returns true if the survivor survived.
        /// </summary>
        public bool ProcessFaunaAttack(ExpeditionState exp, bool isApex = false)
        {
            if (exp?.Survivor == null || !exp.Survivor.IsAlive) return false;

            float damage = isApex ? ApexAttackHealthDamage : FaunaAttackHealthDamage;
            SurvivorNeedWrite.AdjustHealth(exp.Survivor, -damage);
            exp.Survivor.Needs.Fatigue = Mathf.Clamp(
                exp.Survivor.Needs.Fatigue + FaunaFleeFatigueCost, 0f, 100f);

            // Small rad exposure from the creature's irradiated bite/claws.
            // MISC-007 — fauna bite only through injected RadiationSystem (no direct dose write).
            float biteRads = isApex ? 15f : 5f;
            if (_radiation != null)
                _radiation.Expose(exp.Survivor, biteRads, 1f);

            // Chance of dropping loot when defeating fauna.
            bool defeated = _rng.NextDouble() < (isApex ? 0.3f : 0.6f);
            if (defeated && exp.CollectedLoot != null)
            {
                var meat = ScriptableObject.CreateInstance<Inventory.ItemDefinition>();
                meat.id = isApex ? "mutated_meat_large" : "mutated_meat";
                meat.displayName = isApex ? "Apex Predator Meat" : "Mutated Meat";
                meat.description = "Irradiated but edible. The body adapted; the meat did not.";
                meat.type = Inventory.ItemType.Food;
                meat.stackMax = 5;
                meat.weight = 4f;
                meat.contamination = isApex ? 0.5f : 0.25f;
                meat.tradeValue = isApex ? 10f : 3f;
                exp.TryAddLoot(meat);
            }

            OnFaunaEncountered?.Invoke(exp, isApex);
            return exp.Survivor.IsAlive;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public EcosystemSave CaptureState()
        {
            return new EcosystemSave
            {
                RadiationExposureDays = _radiationExposureDays
            };
        }

        public void RestoreState(EcosystemSave save)
        {
            _radiationExposureDays = save?.RadiationExposureDays ?? 0f;
        }
    }

    [Serializable]
    public class EcosystemSave
    {
        public float RadiationExposureDays;
    }
}
