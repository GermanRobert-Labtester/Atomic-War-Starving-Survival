using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private SurvivorSocialCoordinator _survivorSocial = null!;
        private bool _survivorSocialDirty;

        private void SetupSurvivorSocial()
        {
            if (_survivorSocial != null) return;

            SetupSurvivors();
            SetupSurvivorRelations();
            SetupDutyRoster();

            var rng = new SeededRng(1986);
            _survivorSocial = new SurvivorSocialCoordinator(
                rng,
                _survivors.Needs,
                _survivorRelationsCore,
                _dutyRoster.Roster,
                () => _simDay,
                new GodotLog());

            _survivorSocial.RationPolicy = Ashfall.Core.StartingLevel.RationPolicy.Standard;

            // Register beliefs from survivor catalog traits (best-effort mapping).
            if (_survivors?.Roster?.Roster != null)
            {
                foreach (var entry in _survivors.Roster.Roster)
                {
                    if (entry == null || !entry.isAlive) continue;
                    var def = _survivors.Roster.FindDefinition(entry.definitionId);
                    string belief = InferBeliefProfile(def);
                    if (!string.IsNullOrEmpty(belief))
                        _survivorSocial.RegisterBelief(entry.survivorId, belief);
                }
            }

            var save = SurvivorSocialSaveStore.TryLoad();
            if (save != null)
            {
                _survivorSocial.RestoreState(save);
                GD.Print("[Ashfall Godot] Survivor-social state restored.");
            }

            // Push the read model to the survivor-relations panel.
            if (_survivorRelationsPanel != null)
                _survivorRelationsPanel.SetSocialReadModel(_survivorSocial.BuildReadModel());
        }

        private static string InferBeliefProfile(Ashfall.Core.Survivors.SurvivorDefinition? def)
        {
            if (def == null || def.traitIds == null) return string.Empty;
            for (int i = 0; i < def.traitIds.Count; i++)
            {
                string t = def.traitIds[i];
                if (string.IsNullOrEmpty(t)) continue;
                if (t.Contains("military", StringComparison.OrdinalIgnoreCase))
                    return "military_discipline";
                if (t.Contains("religious", StringComparison.OrdinalIgnoreCase) || t.Contains("faith", StringComparison.OrdinalIgnoreCase))
                    return "religious_faith";
                if (t.Contains("rationalist", StringComparison.OrdinalIgnoreCase) || t.Contains("scientist", StringComparison.OrdinalIgnoreCase))
                    return "atheist_rationalist";
                if (t.Contains("collectivist", StringComparison.OrdinalIgnoreCase) || t.Contains("communal", StringComparison.OrdinalIgnoreCase))
                    return "collectivist_solidarity";
                if (t.Contains("individualist", StringComparison.OrdinalIgnoreCase) || t.Contains("pragmatic", StringComparison.OrdinalIgnoreCase))
                    return "pragmatic_individualism";
                if (t.Contains("pacifist", StringComparison.OrdinalIgnoreCase))
                    return "pacifist";
                if (t.Contains("superstitious", StringComparison.OrdinalIgnoreCase) || t.Contains("traditional", StringComparison.OrdinalIgnoreCase))
                    return "superstitious_traditional";
            }
            return string.Empty;
        }

        private void SaveSurvivorSocial()
        {
            if (_survivorSocial == null) return;
            if (CaptureSection("survivor_social", SurvivorSocialSaveStore.TryCapturePersisted(_survivorSocial.CaptureState())))
            {
                _survivorSocialDirty = false;
                GD.Print("[Ashfall Godot] Survivor-social save written.");
            }
        }

        private void FlushSurvivorSocialIfDirty()
        {
            if (_survivorSocialDirty) SaveSurvivorSocial();
        }

        /// <summary>
        /// Advance the survivor-social cluster by one day. Called from
        /// <see cref="TickSimDay"/> after survivors and duty-roster tick.
        /// Feeds real needs, duty shifts, ration policy, and skill morale.
        /// </summary>
        private void TickSurvivorSocial(int day)
        {
            SetupSurvivorSocial();
            if (_survivorSocial == null) return;

            // Ration policy from the real starting-level state.
            SetupStartingLevel();
            if (_startingLevel?.System?.State != null)
                _survivorSocial.RationPolicy = _startingLevel.System.State.rationPolicy;

            // Forward leader death/injury events from the needs system.
            // (The host wires OnDied → coordinator.OnSurvivorDied elsewhere.)

            _survivorSocial.TickDay(day, _survivors.RosterState);
            _survivorSocialDirty = true;

            // Push the read model to the survivor-relations panel.
            if (_survivorRelationsPanel != null && _survivorSocial != null)
                _survivorRelationsPanel.SetSocialReadModel(_survivorSocial.BuildReadModel());
        }
    }
}
