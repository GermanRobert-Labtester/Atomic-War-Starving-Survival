using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Medical
{
    [Serializable]
    public class TreatmentIngredient
    {
        public ItemDefinition item;
        public string itemId;
        public int amount = 1;

        public string ResolvedId =>
            !string.IsNullOrEmpty(itemId) ? itemId : (item != null ? item.id : null);
    }

    /// <summary>
    /// Items + time required to cure (or fully treat) a specific affliction.
    /// High Medical skill shortens time and may spare secondary ingredients.
    /// </summary>
    [CreateAssetMenu(fileName = "TreatmentRecipe", menuName = "ASHFALL/Medical/Treatment Recipe")]
    public class TreatmentRecipeSO : ScriptableObject
    {
        [Header("Identity")]
        public string id;
        public string displayName;

        [Tooltip("Affliction id this recipe treats (snake_case).")]
        public string targetAfflictionId;

        [Header("Requirements")]
        public List<TreatmentIngredient> ingredients = new List<TreatmentIngredient>();
        public float baseTreatmentHours = 2f;
        public bool requiresMedicalBed;
        public bool requiresPatientRest = true;

        [Header("Outcomes")]
        [Tooltip("Health restored when treatment completes successfully.")]
        public float healthRestoreOnCure = 15f;

        [Tooltip("If true, treatment only halts progression (does not remove affliction).")]
        public bool haltOnly;

        [Tooltip("Surgical procedure: Steady Hands shortens time and suppresses accidental Bleeding.")]
        public bool isSurgical;
    }
}
