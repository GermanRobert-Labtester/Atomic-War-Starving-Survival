using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Utility AI: wash down a pet's contaminated fur before it keeps radiating
    /// the room it sits in (Prompt #217). Scores on the dirtiest living pet's
    /// fur contamination; any survivor can do it.
    /// </summary>
    [CreateAssetMenu(fileName = "Action_DecontaminatePet", menuName = "ASHFALL/AI Actions/Decontaminate Pet Action")]
    public class DecontaminatePetActionSO : SurvivorAction
    {
        /// <summary>Fur contamination below this is not worth a survivor's time.</summary>
        public const float FurContaminationScoreThreshold = 5f;

        public DecontaminatePetActionSO()
        {
            id = "action_decontaminate_pet";
            displayName = "Decontaminate Pet";
            description = "Hose down the fur before the whole bunker starts glowing.";
            basePriority = 0.2f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return 0f;
            var pet = FindDirtiestPet(context);
            if (pet == null) return 0f;
            return Mathf.Clamp01(pet.FurContamination / 40f);
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.PetSystem == null) return;
            var pet = FindDirtiestPet(context);
            if (pet == null) return;

            // Re-resolve by id rather than trusting the cached reference.
            var resolved = context.PetSystem.GetPet(pet.Id);
            if (resolved == null) return;
            context.PetSystem.Decontaminate(resolved);
        }

        private static PetState FindDirtiestPet(AIContext context)
        {
            var pets = context?.PetSystem?.Pets;
            if (pets == null) return null;

            PetState worst = null;
            for (int i = 0; i < pets.Count; i++)
            {
                var p = pets[i];
                if (p == null || !p.IsAlive || p.FurContamination <= FurContaminationScoreThreshold)
                    continue;
                if (worst == null || p.FurContamination > worst.FurContamination)
                    worst = p;
            }
            return worst;
        }
    }
}
