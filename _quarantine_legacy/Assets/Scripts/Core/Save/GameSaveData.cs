using System;
using System.Collections.Generic;

namespace AtomicWar.Core.Save
{
    public interface ISavable
    {
        string SaveKey { get; }
        object CaptureState();
        void RestoreState(object state);
    }

    [Serializable]
    public class GameSaveData
    {
        public int DayNumber;
        public string CurrentPhase;
        public List<SurvivorSaveData> Survivors = new List<SurvivorSaveData>();
        public List<InventoryItemSaveData> InventoryItems = new List<InventoryItemSaveData>();
        public Dictionary<string, string> CustomSystemData = new Dictionary<string, string>();
    }

    [Serializable]
    public class SurvivorSaveData
    {
        public string Id;
        public string DefinitionId;
        public float Hunger;
        public float Fatigue;
        public float Health;
        public float Morale;
        public string Status;
    }

    [Serializable]
    public class InventoryItemSaveData
    {
        public string ItemId;
        public int Amount;
    }
}
