using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.AI.Actions
{
    /// <summary>
    /// Utility AI: a Paramedic drags a Death's-Door teammate back with an EpiPen
    /// (Prompt #205 / #284). Unlike a self-administered EpiPen, the Paramedic's
    /// training means no adrenaline crash afterward. Top priority — a dying
    /// teammate must win over routine chores.
    /// </summary>
    [CreateAssetMenu(fileName = "Action_AdministerAdrenaline", menuName = "ASHFALL/AI Actions/Administer Adrenaline Action")]
    public class AdministerAdrenalineActionSO : SurvivorAction
    {
        /// <summary>Same item id as the world-catalog "Epi-Pen" (Medical, "Emergency adrenaline").</summary>
        public const string AdrenalineItemId = "epi_pen";

        public AdministerAdrenalineActionSO()
        {
            id = "action_administer_adrenaline";
            displayName = "Administer Adrenaline";
            description = "Jam the needle in. Drag them back from the door.";
            basePriority = 0.95f;
        }

        public override float EvaluateRaw(AIContext context)
        {
            if (context?.Survivor == null || !context.Survivor.IsAlive) return 0f;
            if (context.MedicalPerks == null || context.GetSurvivors == null) return 0f;
            if (!context.MedicalPerks.HasParamedic(context.Survivor)) return 0f;
            if (context.Inventory == null || context.Inventory.CountById(AdrenalineItemId) <= 0)
                return 0f;
            if (FindDeathsDoorPatient(context) == null) return 0f;

            return 0.97f;
        }

        public override void Execute(AIContext context)
        {
            if (context?.Survivor == null || context.MedicalPerks == null
                || context.GetSurvivors == null || context.Inventory == null)
                return;
            if (!context.MedicalPerks.HasParamedic(context.Survivor)) return;

            var patient = FindDeathsDoorPatient(context);
            if (patient == null) return;

            var inv = context.Inventory;
            context.MedicalPerks.TryAdministerAdrenaline(
                context.Survivor, patient,
                tryConsumeAdrenaline: () => inv.RemoveById(AdrenalineItemId, 1));
        }

        private static Survivor FindDeathsDoorPatient(AIContext context)
        {
            var survivors = context.GetSurvivors();
            if (survivors == null) return null;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || sv.Id == context.Survivor.Id) continue;
                if (context.MedicalPerks.IsOnDeathsDoor(sv)) return sv;
            }
            return null;
        }
    }
}
