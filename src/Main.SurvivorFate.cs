// SPDX-License-Identifier: MIT
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Ashfall.Core.Feedback;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Unified survivor-death pipeline wiring (Task 121).
    ///
    /// SetupSurvivorFate constructs the Core <see cref="SurvivorFateSystem"/>
    /// against the live subsystem sessions and subscribes every death source
    /// (needs, radiation, disease, combat, expeditions, scripted) to it. The
    /// fate system is the idempotency authority: one survivor id runs one
    /// cascade, ever. Setup/Save/Flush triad per the house pattern; section
    /// key "survivor_fate".
    /// </summary>
    public partial class Main : Control
    {
        private SurvivorFateSystem _survivorFate = null!;
        private bool _survivorFateDirty;
        private SurvivorsHostSession? _survivorFateSurvivorsSource;
        private DiseaseHostSession? _survivorFateDiseaseSource;
        private System.Action<SurvivorFateEvent>? _survivorFateChangedHandler;
        private System.Action<string, Ashfall.Core.Survivors.SurvivorDeathCause, string>? _survivorFateNeedsHandler;
        private System.Action<string, string>? _survivorFateDiseaseHandler;

        private void SetupSurvivorFate()
        {
            if (_survivorFate != null) return;

            // P014 — every Game Over path renders the per-survivor loss ledger
            // through the panel's provider (no caller has to pass it).
            if (_gameOver != null) _gameOver.LedgerProvider = BuildSurvivorLossLedger;

            // Lane dependencies — all lazy-setup so ordering is safe.
            SetupSurvivors();
            SetupMemorial();
            SetupCampaignDay();
            SetupNarrative();          // _journal
            SetupDutyRoster();         // _dutyRoster
            SetupMedicalWard();        // _medicalWard
            SetupSurvivorSocial();     // _survivorSocial
            SetupPhase0();             // _phase0.FinalWish

            _survivorFate = new SurvivorFateSystem(
                roster: _survivors.Roster,
                needs: _survivors.Needs,
                dutyRoster: _dutyRoster.Roster,
                caregiving: _caregiving?.System,
                medicalWard: _medicalWard,
                social: _survivorSocial,
                finalWish: _phase0.FinalWish,
                memorial: _memorial,
                journal: _journal,
                flags: _consequenceLedger,
                getDay: () => _simDay,
                displayNameFor: FormatSurvivorName,
                expeditionRecall: id =>
                {
                    // Recall the dead survivor's active expedition (if any) so
                    // it is no longer ticked as a live sortie.
                    if (_expeditions?.Engine != null && _expeditions.Engine.Active.ContainsKey(id))
                        _expeditions.Engine.Retreat(id);
                });

            _survivorFateChangedHandler = fate =>
            {
                _survivorFateDirty = true;
                _apprenticeship?.System.NotifyMentorDeath(fate.survivorId);
                // Plan 42 — the fate owner's death signal is the canonical
                // "survivor_perished" voice trigger.
                TriggerSurvivorVoicePerished(fate.survivorId);
                // Plan 46 — the fate owner's death signal is recorded in the local
                // play-session stream (audit read model only).
                RecordPlayMetricSurvivorPerished(fate.survivorId);
                // Memorial/journal/duty/roster save lanes are flagged by their
                // own OnMemorialized/OnEntryAdded/OnAssignmentChanged handlers;
                // this handler only marks the fate lane dirty.
                var name = FormatSurvivorName(fate.survivorId);
                FeedbackMessages.Emit(new FeedbackEvent(
                    key: "survivor_lost",
                    arguments: new object[] { name },
                    category: "failure",
                    dedupeKey: $"survivor_lost_{fate.survivorId}"
                ));
            };
            _survivorFate.OnSurvivorFate += _survivorFateChangedHandler;
            _survivorFate.OnLastSurvivorDied += OnLastSurvivorDied;
            // Plan 210: the fate owner is the only death/inheritance trigger;
            // belongings keep only ownership metadata and never move inventory
            // stacks behind its back.
            _survivorFate.OnSurvivorFate += HandlePersonalBelongingsInheritance;

            // ── Death-source feeds ─────────────────────────────────────
            // Needs + radiation (survival loop).
            _survivorFateSurvivorsSource = _survivors;
            _survivorFateNeedsHandler = (id, cause, detail) =>
                _survivorFate?.ReportDeath(id, cause, detail, source: "survivors_needs");
            _survivorFateSurvivorsSource.OnSurvivorDied += _survivorFateNeedsHandler;

            // Disease (lethal outcome).
            if (_disease != null)
            {
                _survivorFateDiseaseSource = _disease;
                _survivorFateDiseaseHandler = (id, diseaseId) =>
                    _survivorFate?.ReportDeath(id, SurvivorDeathCause.Disease, diseaseId, source: "disease");
                _survivorFateDiseaseSource.OnSurvivorDied += _survivorFateDiseaseHandler;
            }

            // The player/avatar death feed is wired in SetupHoldfastRuntime
            // (OnPlayerDied → avatar ReportDeath → campaign loss), which owns
            // the avatar binding. Scripted deaths enter via ReportScriptedDeath.

            // ── Restore + legacy reconcile ─────────────────────────────
            var save = SurvivorFateSaveStore.TryLoad();
            if (save != null && save.State != null)
                _survivorFate.RestoreState(save.State);

            // Pre-pipeline saves: roster entries already dead with no fate
            // record get a synthesized fate so the ledger is complete.
            int synthesized = _survivorFate.ReconcileFromRoster();
            if (_apprenticeship != null)
            {
                foreach (var pair in _apprenticeship.System.State.activePairs)
                    if (_survivorFate.HasFate(pair.mentorId))
                        _apprenticeship.System.NotifyMentorDeath(pair.mentorId);
            }
            if (synthesized > 0)
            {
                _survivorFateDirty = true;
                GD.Print($"[Ashfall Godot] Survivor-fate reconcile synthesized {synthesized} legacy death record(s).");
            }

            SetupSpiritual();
        }

        /// <summary>Report a scripted / narrative death into the unified pipeline.</summary>
        public void ReportScriptedDeath(string survivorId, string narrativeReason)
        {
            SetupSurvivorFate();
            _survivorFate?.ReportDeath(survivorId, SurvivorDeathCause.Scripted, narrativeReason, source: "narrative");
        }

        /// <summary>Campaign loss: every roster member is dead. Distinct from a single survivor death.</summary>
        private void OnLastSurvivorDied(SurvivorFateEvent fate)
        {
            GD.Print($"[Ashfall Godot] Last survivor died ({fate.survivorId}, {fate.cause}). Campaign terminal.");
            // Finalize the run as a loss. ShowGameOver performs the terminal
            // save and marks the slot; it must not resurrect continuation.
            SetupHoldfastRuntime();
            string cause = $"The last of the Holdfast has fallen. {FormatSurvivorName(fate.survivorId)} {SurvivorFateSystem.DescribeCause(fate)}.";
            string stats = $"The Holdfast is silent. Day {_simDay}. " +
                           $"{_survivorFate.DeathCount} souls lost.";
            ShowGameOver(cause, stats);
        }

        /// <summary>
        /// P014 — per-survivor cause-of-death chain for the Game Over screen:
        /// every fated survivor in death order (name · day · cause), followed by
        /// the survivors still standing. Read-only projection of the fate owner
        /// and roster; it fabricates nothing and adds no state.
        /// </summary>
        private string BuildSurvivorLossLedger()
        {
            SetupSurvivorFate();
            var lines = new System.Collections.Generic.List<string>();
            if (_survivorFate != null)
            {
                var fates = _survivorFate.Fates;
                for (int i = 0; i < fates.Count; i++)
                {
                    var f = fates[i];
                    if (f == null || string.IsNullOrEmpty(f.survivorId)) continue;
                    lines.Add($"{FormatSurvivorName(f.survivorId)} — Day {f.day}: {SurvivorFateSystem.DescribeCause(f)}");
                }
            }
            if (_survivors?.RosterState != null)
            {
                for (int i = 0; i < _survivors.RosterState.Count; i++)
                {
                    var s = _survivors.RosterState[i];
                    if (s == null || !s.IsAliveState) continue;
                    if (_survivorFate?.HasFate(s.Id) == true) continue;
                    lines.Add($"{FormatSurvivorName(s.Id)} — still standing");
                }
            }
            return string.Join("\n", lines);
        }

        private void SaveSurvivorFate()
        {
            if (_survivorFate == null) return;
            try
            {
                var save = new SurvivorFateSave
                {
                    simDay = _simDay,
                    State = _survivorFate.CaptureState()
                };
                if (CaptureSection("survivor_fate", SurvivorFateSaveStore.TryCapturePersisted(save)))
                    _survivorFateDirty = false;
            }
            catch (System.Exception e)
            {
                GD.PushWarning("[Ashfall Godot] Survivor-fate save failed: " + e.Message);
            }
        }

        public void ResetSurvivorFate()
        {
            if (_survivorFate != null)
            {
                if (_survivorFateChangedHandler != null)
                    _survivorFate.OnSurvivorFate -= _survivorFateChangedHandler;
                _survivorFate.OnLastSurvivorDied -= OnLastSurvivorDied;
                _survivorFate.OnSurvivorFate -= HandlePersonalBelongingsInheritance;
            }
            if (_survivorFateSurvivorsSource != null && _survivorFateNeedsHandler != null)
                _survivorFateSurvivorsSource.OnSurvivorDied -= _survivorFateNeedsHandler;
            if (_survivorFateDiseaseSource != null && _survivorFateDiseaseHandler != null)
                _survivorFateDiseaseSource.OnSurvivorDied -= _survivorFateDiseaseHandler;

            _survivorFate = null!;
            _survivorFateDirty = false;
            _survivorFateSurvivorsSource = null;
            _survivorFateDiseaseSource = null;
            _survivorFateChangedHandler = null;
            _survivorFateNeedsHandler = null;
            _survivorFateDiseaseHandler = null;
        }
    }
}
