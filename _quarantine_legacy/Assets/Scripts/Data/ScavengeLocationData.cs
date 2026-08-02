using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar.Data
{
    public enum DangerLevel
    {
        Safe,
        Cautious,
        Dangerous,
        Lethal
    }

    [CreateAssetMenu(fileName = "NewScavengeLocation", menuName = "AtomicWar/Data/ScavengeLocationData")]
    public class ScavengeLocationData : ScriptableObject
    {
        public string Id;
        public string LocationName;
        [TextArea] public string Description;
        public Sprite LocationImage;

        public DangerLevel Danger;
        public List<ItemIngredient> PossibleLootPool = new List<ItemIngredient>();
        public bool HasHostiles;
        public bool HasTraders;
    }
}
