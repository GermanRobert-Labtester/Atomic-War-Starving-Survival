using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    /// <summary>
    /// Medical bed / triage bunk. Required for complex treatments (surgery,
    /// rad-burn debridement, heavy-metal chelation). Module id: medical_bed.
    /// </summary>
    [CreateAssetMenu(fileName = "MedicalBedModule", menuName = "ASHFALL/Shelter/Medical Bed Module")]
    public class MedicalBedModuleSO : ShelterModule
    {
        [Header("Medical Bed")]
        [Tooltip("How many concurrent complex treatments this bed supports.")]
        public int ConcurrentPatients = 1;

        [Tooltip("Multiplier on treatment speed when bed is operational (1 = baseline).")]
        public float TreatmentSpeedBonus = 1.1f;

        [Tooltip("Hygiene level 0..100; low hygiene raises infection risk after surgery.")]
        [Range(0f, 100f)]
        public float Hygiene = 100f;

        [Tooltip("Hygiene lost per hour of use.")]
        public float HygieneLossPerHour = 2f;
    }
}
