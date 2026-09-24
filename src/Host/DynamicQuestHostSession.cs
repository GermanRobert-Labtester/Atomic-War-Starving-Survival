// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 171 — Dynamic Quest Generation host session.
// Loads the authored dynamic_quest_templates.json through the strict loader and
// binds it to the Core DynamicQuestGenerator. Per the sealed Plan 171
// disposition the generator is the authored template + deterministic candidate
// authority; the canonical QuestRuntimeCoordinator owns the accepted-quest
// lifecycle and its save, so this session creates no second quest lifecycle and
// no second save section.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Quests;

namespace AtomicWar.GodotApp
{
    public sealed class DynamicQuestHostSession : HostSessionBase
    {
        private readonly DynamicQuestGenerator _generator = new();
        private string _lastEvent = string.Empty;

        public DynamicQuestGenerator Generator => _generator;
        public DynamicQuestGeneratorCensus Census => _generator.GetCensus();
        public string LastEvent => _lastEvent;
        public IReadOnlyList<ProceduralQuestTemplate> Templates { get; private set; } = Array.Empty<ProceduralQuestTemplate>();

        public DynamicQuestHostSession()
        {
            _generator.OnQuestGenerated += quest =>
            {
                _lastEvent = $"Generated dynamic candidate {quest.QuestId} ({quest.Type})";
                RaiseStateChanged();
            };
            _generator.OnQuestCompleted += quest =>
            {
                _lastEvent = $"Completed dynamic candidate {quest.QuestId}";
                RaiseStateChanged();
            };
            _generator.OnQuestExpired += quest =>
            {
                _lastEvent = $"Dynamic candidate {quest.QuestId} is now {quest.Status}";
                RaiseStateChanged();
            };
        }

        public static DynamicQuestHostSession Create(string dataDir)
        {
            var session = new DynamicQuestHostSession();
            if (!string.IsNullOrEmpty(dataDir))
                session.LoadCatalog(dataDir);
            return session;
        }

        /// <summary>
        /// Loads and binds the authored dynamic quest templates through the strict
        /// loader; the built-in defaults are replaced so they cannot mask authoring.
        /// </summary>
        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;

            string path = Path.Combine(dataDir, "dynamic_quest_templates.json");
            if (!File.Exists(path)) return;

            var templates = DynamicQuestTemplateCatalogLoader.LoadFromJson(File.ReadAllText(path));
            Templates = templates;
            _generator.BindAuthoredTemplates(templates);
            _lastEvent = $"Loaded {templates.Count} authored dynamic quest templates.";
            RaiseStateChanged();
        }

        /// <summary>Deterministic candidate projection over the authored templates.</summary>
        public IReadOnlyList<ProceduralQuest> GenerateCandidates(int day, ISeededRng? rng = null) =>
            _generator.GenerateQuests(day, rng);

        public bool AcceptCandidate(string questId, string survivorId = "") =>
            _generator.AcceptQuest(questId, survivorId);

        public bool ProgressCandidate(string questId, int amount = 1) =>
            _generator.ProgressQuest(questId, amount);

        public bool CompleteCandidate(string questId, int day) =>
            _generator.CompleteQuest(questId, day);

        public void CheckDeadlines(int day) => _generator.CheckDeadlines(day);

        public DynamicQuestGeneratorState CaptureState() => _generator.CaptureState();
        public void RestoreState(DynamicQuestGeneratorState state) => _generator.RestoreState(state);
    }
}
