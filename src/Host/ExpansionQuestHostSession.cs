using Godot;
using Ashfall.Core;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL — Expansion Quest Host Session
    /// Host-side wrapper for the ExpansionQuestSystem.
    /// </summary>
    public sealed class ExpansionQuestHostSession : HostSessionBase
    {
        private ExpansionQuestSystem _system;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IFileIO _fileIO;
        private readonly string _dataDir;

        public ExpansionQuestSystem System => _system;

        public ExpansionQuestHostSession(IJsonSerializer jsonSerializer, IFileIO fileIO, string dataDir)
        {
            _jsonSerializer = jsonSerializer;
            _fileIO = fileIO;
            _dataDir = dataDir;
            _system = new ExpansionQuestSystem();
            
            // Wire up state change events to raise host session StateChanged
            _system.OnStateChanged += _ => RaiseStateChanged();
            
            LoadCatalog();
        }

        public static ExpansionQuestHostSession Create(string dataDir)
        {
            var serializer = new SystemTextJsonSerializer();
            var fileIO = new FileSystemIO();
            return new ExpansionQuestHostSession(serializer, fileIO, dataDir);
        }

        private void LoadCatalog()
        {
            var catalog = ExpansionQuestCatalogLoader.Load(_dataDir, _fileIO, _jsonSerializer);
            _system.BindCatalog(catalog);
        }

        /// <summary>
        /// Gets a list of available expansion quests for the current day.
        /// </summary>
        public List<ExpansionQuestEntry> GetAvailableQuests(int currentDay)
        {
            return _system.GetAvailableQuests(currentDay);
        }

        /// <summary>
        /// Starts an expansion quest.
        /// </summary>
        public void StartQuest(string questId, int currentDay)
        {
            _system.StartQuest(questId, currentDay);
        }

        /// <summary>
        /// Completes an expansion quest with the chosen choice.
        /// </summary>
        public void CompleteQuest(string questId, string choiceId, int currentDay)
        {
            _system.MakeChoice(questId, choiceId, currentDay);
            _system.CompleteQuest(questId, currentDay);
        }

        /// <summary>
        /// Fails an expansion quest.
        /// </summary>
        public void FailQuest(string questId, int currentDay)
        {
            _system.FailQuest(questId, currentDay);
        }

        /// <summary>
        /// Gets the quest definition by ID.
        /// </summary>
        public ExpansionQuestEntry GetQuestDef(string questId)
        {
            return _system.GetDefinition(questId);
        }

        /// <summary>
        /// Gets the choices for a quest.
        /// </summary>
        public List<ExpansionQuestChoice> GetQuestChoices(string questId)
        {
            return _system.GetChoices(questId);
        }

        /// <summary>
        /// Checks if a quest is available.
        /// </summary>
        public bool IsQuestAvailable(string questId, int currentDay)
        {
            return _system.IsAvailable(questId, currentDay);
        }

        /// <summary>
        /// Checks if a quest is started.
        /// </summary>
        public bool IsQuestStarted(string questId)
        {
            return _system.IsStarted(questId);
        }

        /// <summary>
        /// Checks if a quest is completed.
        /// </summary>
        public bool IsQuestCompleted(string questId)
        {
            return _system.IsCompleted(questId);
        }

        /// <summary>
        /// Checks if a quest is failed.
        /// </summary>
        public bool IsQuestFailed(string questId)
        {
            return _system.IsFailed(questId);
        }

        /// <summary>
        /// Tick the quest system for a new day.
        /// </summary>
        public string TickDay(int day)
        {
            return _system.TickDay(day);
        }

        public ExpansionQuestSystemState CaptureState()
        {
            return _system.CaptureState();
        }

        public void RestoreState(ExpansionQuestSystemState state)
        {
            _system.RestoreState(state);
        }
    }
}
