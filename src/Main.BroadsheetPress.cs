// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Expansion 30 — The Press: broadsheet/almanac host wiring.
// The signed pure PublicBroadsheetPressEngine is the physical and editorial
// authority for movable type, ink, paper, and print runs. This host owns no
// truth of its own: the shelter population comes from the canonical survivor
// roster, compositor skill from the canonical skill progression owner, morale
// stabilization is applied through the survivors' needs authority, and a
// pamphlet's debunk correction is applied to the canonical RumorSystem record.
// ============================================================================

using System;
using System.Linq;
using Godot;
using Ashfall.Core.Print;
using Ashfall.Core.Radiation;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private BroadsheetPressHostSession? _broadsheetPress;
        private bool _broadsheetPressDirty;

        public BroadsheetPressHostSession? BroadsheetPress => _broadsheetPress;

        public void SetupBroadsheetPress()
        {
            if (_broadsheetPress != null) return;

            var saved = BroadsheetPressSaveStore.TryLoad();
            _broadsheetPress = BroadsheetPressHostSession.Create(saved);
            _broadsheetPress.StateChanged += () => _broadsheetPressDirty = true;
        }

        /// <summary>Living shelter residents — the population the press measures reach against.</summary>
        private int GetBroadsheetPressPopulation()
        {
            SetupSurvivors();
            if (_survivors == null) return 0;
            return _survivors.RosterState.Count(s => s != null && s.IsAliveState);
        }

        /// <summary>
        /// Resolves compositor skill from the canonical skill progression owner
        /// (crafting discipline). A neutral 500 is used when no worker is named or
        /// the owner has no record; the press never keeps its own skill ledger.
        /// </summary>
        private int ResolveCompositorSkillPermille(string? workerSurvivorId)
        {
            if (string.IsNullOrWhiteSpace(workerSurvivorId)) return 500;
            try
            {
                var skills = EnsureSharedSkillProgression();
                float progress = skills.GetDisciplineProgress01(workerSurvivorId!, "crafting");
                return Math.Clamp((int)Math.Round(progress * 1000f), 0, 1000);
            }
            catch (Exception ex)
            {
                GD.PrintErr("[Ashfall Godot] Press compositor skill lookup skipped: " + ex.Message);
                return 500;
            }
        }

        /// <summary>
        /// Prints one publication and routes its consequences through the canonical
        /// owners: morale stabilization to living residents' needs, nothing else.
        /// Returns the engine result so callers can inspect the shortage case.
        /// </summary>
        public PrintRunResult RunBroadsheetPress(
            PublicationKind kind,
            int targetCopies,
            string? workerSurvivorId = null,
            string? headline = null,
            string? publicationId = null)
        {
            SetupBroadsheetPress();
            if (_broadsheetPress == null)
                return new PrintRunResult(0, 0, 0, 0, 0, 0, true, "press unavailable");

            int population = GetBroadsheetPressPopulation();
            if (population <= 0)
                return new PrintRunResult(0, 0, 0, 0, 0, 0, true, "no living residents to reach");

            int skill = ResolveCompositorSkillPermille(workerSurvivorId);
            var result = _broadsheetPress.Publish(
                publicationId, kind, targetCopies, skill, population, headline, _simDay);

            if (!result.BlockedByShortage && result.MoraleStabilizationPermille > 0)
                ApplyBroadsheetMorale(result.MoraleStabilizationPermille);

            return result;
        }

        /// <summary>
        /// Applies the engine's morale stabilization to living interior residents
        /// through the canonical needs authority (same route the narrative arc
        /// consequence adapter uses). The press keeps no morale counter.
        /// </summary>
        private void ApplyBroadsheetMorale(int moraleStabilizationPermille)
        {
            SetupSurvivors();
            if (_survivors == null) return;

            // 500‰ stabilization is the engine's ceiling; scale it to morale units.
            float delta = Math.Clamp(moraleStabilizationPermille, 0, 500) / 100f;
            if (delta <= 0f) return;

            foreach (var survivor in _survivors.RosterState
                .Where(s => s != null && s.IsAliveState &&
                    _survivors.GetSurvivorLocation(s.Id).Kind == SurvivorExposureLocation.ShelterInterior)
                .OrderBy(s => s.Id, StringComparer.Ordinal))
            {
                _survivors.Needs.Modify(survivor, NeedKind.Morale, delta);
            }
        }

        /// <summary>
        /// Prints a debunking pamphlet and raises the canonical rumor's truthfulness
        /// by the correction the engine computed. The press never creates, decays,
        /// or stores a rumor — the RumorSystem record remains the only authority.
        /// </summary>
        /// <returns>The credibility correction actually applied, in permille.</returns>
        public int DebunkRumorWithPamphlet(
            string rumorId,
            int targetCopies,
            int evidenceQualityPermille,
            string? workerSurvivorId = null)
        {
            SetupBroadsheetPress();
            if (_broadsheetPress == null || string.IsNullOrWhiteSpace(rumorId)) return 0;

            int population = GetBroadsheetPressPopulation();
            if (population <= 0) return 0;

            int skill = ResolveCompositorSkillPermille(workerSurvivorId);
            var run = _broadsheetPress.Publish(
                null, PublicationKind.Pamphlet, targetCopies, skill, population,
                $"Debunk: {rumorId}", _simDay);
            if (run.BlockedByShortage || run.CopiesPrinted <= 0) return 0;

            EnsureRumorNetwork();
            var rumor = _rumorNetwork.Rumors.FirstOrDefault(r =>
                string.Equals(r.RumorId, rumorId, StringComparison.Ordinal));
            if (rumor == null)
            {
                GD.PrintErr($"[Ashfall Godot] Press pamphlet printed but rumor '{rumorId}' is unknown to the canonical rumor owner.");
                return 0;
            }

            // Rumor strength is the inverse of the canonical truthfulness the owner already tracks.
            int rumorStrengthPermille = (int)Math.Round(Math.Clamp(1f - rumor.Truthfulness, 0f, 1f) * 1000f);
            int correction = _broadsheetPress.CalculateDebunkCorrection(
                rumorStrengthPermille, run.AudienceReachPermille, evidenceQualityPermille);
            if (correction <= 0) return 0;

            rumor.Truthfulness = Math.Clamp(rumor.Truthfulness + correction / 1000f, 0f, 1f);
            _rumorNetworkDirty = true;
            GD.Print($"[Ashfall Godot] Press pamphlet raised rumor '{rumorId}' truthfulness to {rumor.Truthfulness:0.000}.");
            return correction;
        }

        public void RestoreBroadsheetTypeTray(int freshTypePiecesAdded)
        {
            SetupBroadsheetPress();
            _broadsheetPress?.RestoreTypeTray(freshTypePiecesAdded);
        }

        public void RestockBroadsheetPress(int inkPermille, int paperPermille, int freshTypePieces)
        {
            SetupBroadsheetPress();
            _broadsheetPress?.RestockConsumables(inkPermille, paperPermille, freshTypePieces);
        }

        public BroadsheetPressCensus GetBroadsheetPressCensus() =>
            _broadsheetPress?.Census ?? default;

        public void SaveBroadsheetPress()
        {
            if (_broadsheetPress == null) return;
            var state = _broadsheetPress.CaptureState();
            BroadsheetPressSaveStore.TrySave(state);
            if (CaptureSection(BroadsheetPressSaveStore.SectionName, BroadsheetPressSaveStore.TryCapturePersisted(state)))
                _broadsheetPressDirty = false;
        }

        public void FlushBroadsheetPressIfDirty()
        {
            if (_broadsheetPressDirty)
                SaveBroadsheetPress();
        }

        public void ResetBroadsheetPress()
        {
            _broadsheetPress = null;
            _broadsheetPressDirty = false;
        }
    }
}
