using Godot;
using Ashfall.Core;
using Ashfall.Core.Thirdonary;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL — Thirdonary Quest Host Session
    /// Thin Godot host wrapper for the ThirdonaryQuestSystem.
    /// Follows the ExpansionQuestHostSession pattern exactly.
    /// </summary>
    public sealed class ThirdonaryHostSession : HostSessionBase
    {
        private ThirdonaryQuestSystem _system;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IFileIO _fileIO;
        private readonly string _dataDir;

        public ThirdonaryQuestSystem System => _system;

        public ThirdonaryHostSession(IJsonSerializer jsonSerializer, IFileIO fileIO, string dataDir)
        {
            _jsonSerializer = jsonSerializer;
            _fileIO = fileIO;
            _dataDir = dataDir;
            _system = new ThirdonaryQuestSystem();

            _system.OnStateChanged += _ => RaiseStateChanged();

            LoadCatalog();
        }

        public static ThirdonaryHostSession Create(string dataDir)
        {
            var serializer = new SystemTextJsonSerializer();
            var fileIO = new FileSystemIO();
            return new ThirdonaryHostSession(serializer, fileIO, dataDir);
        }

        private void LoadCatalog()
        {
            var catalog = ThirdonaryCatalogLoader.Load(_dataDir, _fileIO, _jsonSerializer);
            _system.BindCatalog(catalog);
        }

        public List<string> TickDay(ThirdonaryWorldState worldState)
        {
            return _system.TickDay(worldState);
        }

        public void StartQuest(string questId, int day)
        {
            _system.StartQuest(questId, day);
        }

        public void CompleteQuest(string questId, string choiceId, int day)
        {
            _system.MakeChoice(questId, choiceId, day);
            _system.CompleteQuest(questId, day);
        }

        public void FailQuest(string questId, int day)
        {
            _system.FailQuest(questId, day);
        }

        public ThirdonaryQuestDef? GetQuestDef(string questId)
        {
            return _system.GetDefinition(questId);
        }

        public List<ThirdonaryChoice> GetQuestChoices(string questId)
        {
            var def = _system.GetDefinition(questId);
            return def?.choices ?? new List<ThirdonaryChoice>();
        }

        public bool IsQuestStarted(string questId) => _system.IsStarted(questId);
        public bool IsQuestCompleted(string questId) => _system.IsCompleted(questId);
        public bool IsQuestFailed(string questId) => _system.IsFailed(questId);

        public List<ThirdonaryQuestDef> GetAvailableQuests(ThirdonaryWorldState worldState)
        {
            return _system.GetAvailableQuests(worldState);
        }

        public List<ThirdonaryQuestDef> GetActiveQuests()
        {
            return _system.GetActiveQuests();
        }

        public ThirdonaryState CaptureState()
        {
            return _system.CaptureState();
        }

        public void RestoreState(ThirdonaryState state)
        {
            _system.RestoreState(state);
        }
    }
}
