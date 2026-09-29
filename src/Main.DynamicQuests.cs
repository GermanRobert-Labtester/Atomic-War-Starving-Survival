// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Wave 8 B2 — Dynamic Questline player route
// Subsystems   : presentation glue only. The Core authority
//                (DynamicQuestlineSystem) and its host driver
//                (this partial's event triggers and save + SubsystemComposition daily tick) already
//                exist; this file adds the read-only player board. No command
//                authority is introduced.
// ============================================================================
using Godot;
using Ashfall.Core.Quests;

using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Excavation;
using Ashfall.Core.IO;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private UI.DynamicQuestlinePanel? _dynamicQuestlinePanel;
        private DynamicQuestlineSystem? _dynamicQuestlinePanelBoundSystem;

        private void EnsureDynamicQuestlinePanel()
        {
            var system = EnsureDynamicQuests();

            if (_dynamicQuestlinePanel == null)
            {
                _dynamicQuestlinePanel = new UI.DynamicQuestlinePanel();
                _dynamicQuestlinePanel.Bind(system);
                _dynamicQuestlinePanelBoundSystem = system;
                _dynamicQuestlinePanel.Visible = false;
                AddChild(_dynamicQuestlinePanel);
                return;
            }

            if (!ReferenceEquals(_dynamicQuestlinePanelBoundSystem, system))
            {
                _dynamicQuestlinePanel.Bind(system);
                _dynamicQuestlinePanelBoundSystem = system;
            }
        }

        private void OpenDynamicQuestlinePanel()
        {
            EnsureDynamicQuestlinePanel();
            if (_dynamicQuestlinePanel != null)
            {
                _dynamicQuestlinePanel.Visible = true;
                _dynamicQuestlinePanel.RefreshView();
            }
        }
        private DynamicQuestlineSystem? _dynamicQuests;
        private bool _dynamicQuestsDirty;

        // ── Emergency Dynamic Quests ───────────────────────────────────

        public DynamicQuestlineSystem EnsureDynamicQuests()
        {
            if (_dynamicQuests != null) return _dynamicQuests;

            _dynamicQuests = new DynamicQuestlineSystem(new GodotLog());
            var saved = DynamicQuestSaveStore.TryLoad();
            if (saved != null)
            {
                _dynamicQuests.RestoreState(saved);
            }

            _dynamicQuests.OnStateChanged += () => _dynamicQuestsDirty = true;
            return _dynamicQuests;
        }

        private void SetupDynamicQuests()
        {
            EnsureDynamicQuests();
        }

        private void SaveDynamicQuests()
        {
            if (_dynamicQuests != null)
            {
                CaptureSection("dynamic_quests", DynamicQuestSaveStore.TryCapturePersisted(_dynamicQuests.CaptureState()));
                _dynamicQuestsDirty = false;
            }
        }

    }
}
