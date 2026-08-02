using System.IO;
using UnityEngine;

namespace AtomicWar.Core.Save
{
    public class SaveSystem
    {
        private readonly string _saveFilePath;

        public SaveSystem(string filename = "savegame.json")
        {
            _saveFilePath = Path.Combine(Application.persistentDataPath, filename);
        }

        public void Save(GameSaveData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(_saveFilePath, json);
            Debug.Log($"[SaveSystem] Saved state to {_saveFilePath}");
        }

        public GameSaveData Load()
        {
            if (!File.Exists(_saveFilePath))
            {
                Debug.LogWarning("[SaveSystem] No save file found.");
                return null;
            }

            string json = File.ReadAllText(_saveFilePath);
            var data = JsonUtility.FromJson<GameSaveData>(json);
            Debug.Log($"[SaveSystem] Loaded state from {_saveFilePath}");
            return data;
        }

        public bool SaveExists() => File.Exists(_saveFilePath);
    }
}
