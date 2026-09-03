using System;
using System.Collections.Generic;
using Ashfall.Core.Combat;
using Ashfall.Core.Narrative;

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Plan 45 phase 2 — binds hostile travel-encounter choices to tactical
    /// combat composition. A resolved travel-encounter choice escalates to
    /// combat exactly when it is neither <c>is_nonviolent</c> nor
    /// <c>is_avoidance</c>, and only for combat-capable categories:
    /// Creature encounters field the wildlife pack for their
    /// <c>combatant_tag</c>; Human encounters field raid crews (danger ≥ 5)
    /// or ambush patrols (below); Environmental / Chained choices never
    /// spawn combat. Non-hostile choices bind nothing — the caller skips
    /// combat spawn. Composition always comes from
    /// <see cref="EnemyCompositionSelector"/> — the single binding authority.
    /// </summary>
    public static class TravelEncounterCombatBinder
    {
        /// <summary>True when the choice is a hostile (fight) resolution.</summary>
        public static bool IsHostileChoice(TravelEncounterChoice choice)
        {
            if (choice == null) return false;
            return !choice.IsNonviolent && !choice.IsAvoidance;
        }

        /// <summary>
        /// Bind combat composition for a resolved encounter choice. Returns
        /// false (empty <paramref name="combatantIds"/>) when the choice is
        /// not hostile or the category cannot field combat — callers must
        /// not spawn a fight from an empty bind.
        /// </summary>
        public static bool TryBind(
            TravelEncounterDefinition? definition,
            TravelEncounterChoice? choice,
            int dangerLevel,
            int enemyCount,
            out IReadOnlyList<string> combatantIds,
            ISeededRng? rng = null)
        {
            combatantIds = Array.Empty<string>();
            if (definition == null || choice == null) return false;
            if (!IsHostileChoice(choice)) return false;

            var ids = EnemyCompositionSelector.SelectForHostileEncounter(
                definition.Category, definition.CombatantTag, dangerLevel, enemyCount, rng);
            if (ids.Count == 0) return false;

            combatantIds = ids;
            return true;
        }
    }
}
