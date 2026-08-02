using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar.Data
{
    [Serializable]
    public struct EventChoice
    {
        public string ChoiceText;
        [TextArea] public string OutcomeText;
        public List<ItemIngredient> ItemCosts;
        public List<ItemIngredient> ItemRewards;
        public float MoraleChange;
        public float HealthChange;
    }

    [CreateAssetMenu(fileName = "NewEvent", menuName = "AtomicWar/Data/EventData")]
    public class EventData : ScriptableObject
    {
        public string Id;
        public string Title;
        [TextArea] public string Description;
        public Sprite Image;

        public bool TriggersAtDay;
        public bool TriggersAtNight;

        public List<EventChoice> Choices = new List<EventChoice>();
    }
}
