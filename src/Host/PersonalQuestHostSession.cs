// SPDX-License-Identifier: MIT
// ASHFALL survivor personal quest host session (Plan 83 / Task B24).

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Quests;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host-side session manager for survivor personal quests.
    /// Tracks survivor quest progression, choices, and state persistence.
    /// </summary>
    public sealed class PersonalQuestHostSession : HostSessionBase
    {
        private readonly PersonalQuestSystem _system;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IFileIO _fileIO;
        private readonly string _dataDir;

        public PersonalQuestSystem System => _system;

        public PersonalQuestHostSession(IJsonSerializer jsonSerializer, IFileIO fileIO, string dataDir, ISeededRng? rng = null, ILog? log = null)
        {
            _jsonSerializer = jsonSerializer;
            _fileIO = fileIO;
            _dataDir = dataDir;
            _system = new PersonalQuestSystem(rng, log);

            _system.OnQuestStarted += _ => RaiseStateChanged();
            _system.OnStageAdvanced += (_, _) => RaiseStateChanged();
            _system.OnChoiceMade += (_, _) => RaiseStateChanged();
            _system.OnQuestCompleted += _ => RaiseStateChanged();
            _system.OnQuestFailed += (_, _) => RaiseStateChanged();

            LoadCatalog();
        }

        public static PersonalQuestHostSession Create(string dataDir, ISeededRng? rng = null, ILog? log = null)
        {
            var serializer = new SystemTextJsonSerializer();
            var fileIO = new FileSystemIO();
            return new PersonalQuestHostSession(serializer, fileIO, dataDir, rng, log);
        }

        private void LoadCatalog()
        {
            string path = Path.Combine(_dataDir, "personal_quests.json");
            if (_fileIO.FileExists(path))
            {
                string json = _fileIO.ReadAllText(path);
                _system.LoadCatalog(json, _jsonSerializer);
            }
        }

        public PersonalQuestInstance? GetActiveQuest(string survivorId)
        {
            return _system.GetActiveQuest(survivorId);
        }

        public bool TryTriggerQuest(string survivorId, string trait, int day)
        {
            return _system.TryTriggerQuest(survivorId, trait, day);
        }

        public bool ProgressRequirement(string survivorId, string requirementKind, int amount, string? targetId = null)
        {
            return _system.ProgressRequirement(survivorId, requirementKind, amount, targetId);
        }

        public bool ChooseOption(string survivorId, string choiceId, int currentDay, out PersonalQuestChoiceDef? chosenDef)
        {
            return _system.ChooseOption(survivorId, choiceId, currentDay, out chosenDef);
        }

        public bool FailQuest(string survivorId, string reason, int currentDay)
        {
            return _system.FailQuest(survivorId, reason, currentDay);
        }

        public void TickDay(int day, IList<DayStateChangeEvent>? events = null)
        {
            _system.TickDay(day, events);
            RaiseStateChanged();
        }

        public PersonalQuestSaveState CaptureState() => _system.CaptureState();
        public void RestoreState(PersonalQuestSaveState state) => _system.RestoreState(state);

        public bool TrySave() => PersonalQuestSaveStore.TrySave(_system.CaptureState());
        public bool TryLoad()
        {
            var loaded = PersonalQuestSaveStore.TryLoad();
            if (loaded != null)
            {
                _system.RestoreState(loaded);
                RaiseStateChanged();
                return true;
            }
            return false;
        }

        public string TryCapturePersisted() => PersonalQuestSaveStore.TryCapturePersisted(_system.CaptureState());
    }
}
