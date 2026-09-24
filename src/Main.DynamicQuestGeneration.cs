// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 171 — Dynamic Quest Generation host wiring.
// The authored dynamic_quest_templates.json is loaded into the Core
// DynamicQuestGenerator (the authored template + candidate authority). The
// canonical QuestRuntimeCoordinator remains the accepted-quest lifecycle owner,
// so this host exposes the candidate authority without a second lifecycle or a
// second save section.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Quests;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private DynamicQuestHostSession? _dynamicQuestGeneration;

        public DynamicQuestHostSession? DynamicQuestGeneration => _dynamicQuestGeneration;

        public void SetupDynamicQuestGeneration()
        {
            if (_dynamicQuestGeneration != null) return;
            _dynamicQuestGeneration = DynamicQuestHostSession.Create(_dataDir);
        }

        public DynamicQuestGeneratorCensus GetDynamicQuestGenerationCensus() =>
            _dynamicQuestGeneration?.Census ?? default;

        public void ResetDynamicQuestGeneration()
        {
            _dynamicQuestGeneration = null;
        }
    }
}
