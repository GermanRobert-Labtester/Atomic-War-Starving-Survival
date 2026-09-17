// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Wave 8 B2 — Dynamic Questline player route
// Subsystems   : presentation glue only. The Core authority
//                (DynamicQuestlineSystem) and its host driver
//                (Main.Plans46_49.cs event triggers + daily tick + save) already
//                exist; this file adds the read-only player board. No command
//                authority is introduced.
// ============================================================================
using Godot;
using Ashfall.Core.Quests;

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
    }
}