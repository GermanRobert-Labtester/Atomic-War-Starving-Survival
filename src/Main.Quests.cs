using Godot;
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Thirdonary;
using AtomicWar.GodotApp.Host;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private EventsHostSession _eventsHost = null!;
        private ExpansionQuestHostSession _expansionQuests = null!;
        private ThirdonaryHostSession? _thirdonary;
        private bool _expansionQuestsDirty;
        private bool _thirdonaryDirty;

        private void SetupEventsHost()
        {
            if (_eventsHost != null) return;

            // Catalog/read-model only: dynamic event progress is owned by
            // HostEventAdapter and captured under the host_event campaign key.
            _eventsHost = new EventsHostSession(new Ashfall.Core.SystemTextJsonSerializer(), new Ashfall.Core.FileSystemIO());
            AddChild(_eventsHost);
        }

        private void OpenEventsLogPanel()
        {
            if (_eventsLogPanel == null)
            {
                _eventsLogPanel = new EventsLogPanel();
                _eventsLogPanel.OnClose += () => _eventsLogPanel.Visible = false;
                AddChild(_eventsLogPanel);
            }
            _eventsLogPanel.Bind(_eventsHost);
            _eventsLogPanel.Open();
        }

        private void SetupExpansionQuests()
        {
            if (_expansionQuests != null) return;
            _expansionQuests = ExpansionQuestHostSession.Create(_dataDir);
            _expansionQuests.StateChanged += () => _expansionQuestsDirty = true;

            _expansionQuests.System.OnQuestCompleted += def =>
            {
                if (string.Equals(def.id, "quest_exp09_sunken_submarine", StringComparison.Ordinal))
                {
                    _memorial?.Memorialize(new Ashfall.Core.Memorial.MemorialInput
                    {
                        SurvivorId = "barrik_war_grave_crew",
                        Cause = "war_grave",
                        Day = _simDay,
                        BirthDay = 0,
                        Epitaph = "The Half-Submerged Barrik. Lost with all hands. Recorded by the living.",
                        Outcome = Ashfall.Core.Memorial.MemorialOutcome.WallEntry,
                        DeathQuality = Ashfall.Core.Memorial.DeathQuality.Unattended
                    });
                }
            };

            var save = ExpansionQuestSaveStore.TryLoad();
            if (save != null)
            {
                _expansionQuests.RestoreState(save.state);
            }
        }

        private void SaveExpansionQuests()
        {
            if (_expansionQuests == null) return;
            var state = _expansionQuests.CaptureState();
            var envelope = new ExpansionQuestSaveEnvelope
            {
                version = ExpansionQuestSaveEnvelope.CurrentVersion,
                state = state,
                checksum = SaveChecksum.Compute(state)
            };
            if (CaptureSection("expansion_quest", ExpansionQuestSaveStore.TryCapturePersisted(envelope)))
                _expansionQuestsDirty = false;
        }

        private void FlushExpansionQuestsIfDirty()
        {
            if (_expansionQuestsDirty) SaveExpansionQuests();
        }

        private void SetupThirdonary()
        {
            if (_thirdonary != null) return;
            _thirdonary = ThirdonaryHostSession.Create(_dataDir);
            _thirdonary.StateChanged += () => _thirdonaryDirty = true;

            var save = ThirdonarySaveStore.TryLoad();
            if (save != null)
            {
                _thirdonary.RestoreState(save.state);
            }
        }

        private void SaveThirdonary()
        {
            if (_thirdonary == null) return;
            var state = _thirdonary.CaptureState();
            var envelope = new ThirdonarySaveEnvelope
            {
                version = ThirdonarySaveEnvelope.CurrentVersion,
                state = state,
                checksum = SaveChecksum.Compute(state)
            };
            if (CaptureSection("thirdonary", ThirdonarySaveStore.TryCapturePersisted(envelope)))
                _thirdonaryDirty = false;
        }

        private void FlushThirdonaryIfDirty()
        {
            if (_thirdonaryDirty) SaveThirdonary();
        }
    }
}
