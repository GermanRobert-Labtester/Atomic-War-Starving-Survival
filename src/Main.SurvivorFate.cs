using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;

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

        private void SetupSurvivorFate()
        {
            if (_survivorFate != null) return;

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

            _survivorFate.OnSurvivorFate += fate =>
            {
                _survivorFateDirty = true;
                // Memorial/journal/duty/roster save lanes are flagged by their
                // own OnMemorialized/OnEntryAdded/OnAssignmentChanged handlers;
                // this handler only marks the fate lane dirty.
            };
            _survivorFate.OnLastSurvivorDied += OnLastSurvivorDied;

            // ── Death-source feeds ─────────────────────────────────────
            // Needs + radiation (survival loop).
            _survivors.OnSurvivorDied += (id, cause, detail) =>
                _survivorFate.ReportDeath(id, cause, detail, source: "survivors_needs");

            // Disease (lethal outcome).
            if (_disease != null)
                _disease.OnSurvivorDied += (id, diseaseId) =>
                    _survivorFate.ReportDeath(id, SurvivorDeathCause.Disease, diseaseId, source: "disease");

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
            if (synthesized > 0)
            {
                _survivorFateDirty = true;
                GD.Print($"[Ashfall Godot] Survivor-fate reconcile synthesized {synthesized} legacy death record(s).");
            }
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

        private void FlushSurvivorFateIfDirty()
        {
            if (_survivorFateDirty) SaveSurvivorFate();
        }
    }
}
