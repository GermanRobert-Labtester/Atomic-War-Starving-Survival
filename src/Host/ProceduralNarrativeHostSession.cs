// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Quests;

namespace AtomicWar.GodotApp
{
    /// <summary>Thin Godot adapter that loads procedural templates and registers drafts.</summary>
    public sealed class ProceduralNarrativeHostSession : HostSessionBase
    {
        public ProceduralNarrativeSystem System { get; }
        public QuestRuntimeCoordinator QuestRuntime { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public static ProceduralNarrativeHostSession Create(string dataDir, QuestRuntimeCoordinator? questRuntime = null,
            ProceduralNarrativeSystem? system = null)
        {
            var session = new ProceduralNarrativeHostSession(system ?? new ProceduralNarrativeSystem(new GodotLog()),
                questRuntime ?? new QuestRuntimeCoordinator(new GodotLog()));
            if (!string.IsNullOrWhiteSpace(dataDir))
            {
                var templates = QuestTemplateCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
                session.System.LoadTemplateCatalog(templates);
                session.LastEvent = $"Procedural template catalog loaded: {templates.Count} templates";
            }
            return session;
        }

        public ProceduralNarrativeHostSession(ProceduralNarrativeSystem system, QuestRuntimeCoordinator questRuntime)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            QuestRuntime = questRuntime ?? throw new ArgumentNullException(nameof(questRuntime));
            System.OnQuestGenerated += quest =>
            {
                LastEvent = "Procedural quest generated: " + quest.instanceId;
                RaiseStateChanged();
            };
            QuestRuntime.OnQuestExpired += quest =>
            {
                LastEvent = "Procedural quest expired: " + quest.instanceId;
                RaiseStateChanged();
            };
        }

        public bool GenerateAndRegister(NarrativeWorldSnapshot snapshot, ISeededRng rng)
        {
            if (!DynamicQuestGenerator.TryGenerateAndRegisterCanonicalCandidate(System, QuestRuntime, snapshot, rng, out var draft))
            {
                LastEvent = "Procedural generation rejected: " + draft.rejectionReason;
                RaiseStateChanged();
                return false;
            }
            LastEvent = "Procedural quest registered: " + draft.quest.instanceId;
            RaiseStateChanged();
            return true;
        }

        public void AdvanceDay(int day)
        {
            QuestRuntime.Tick(day);
            RaiseStateChanged();
        }

        public ProceduralNarrativeState CaptureNarrativeState() => System.CaptureState();
        public QuestRuntimeState CaptureQuestState() => QuestRuntime.CaptureState();
        public void RestoreState(ProceduralNarrativeState narrative, QuestRuntimeState quests)
        {
            System.RestoreState(narrative);
            QuestRuntime.RestoreState(quests);
            RaiseStateChanged();
        }
    }
}
