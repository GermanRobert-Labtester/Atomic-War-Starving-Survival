// SPDX-License-Identifier: MIT
using Godot;
using Ashfall.Core;
using Ashfall.Core.Spiritual;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Plan 30 spiritual-meaning host: catalog load, death→mourning arcs,
    /// vigil-rite mapping onto MemorialSystem.Mourn, daily stage ticks, save.
    /// Does not add a faith/piety meter.
    /// </summary>
    public partial class Main
    {
        private SpiritualMeaningCoordinator? _spiritual;
        private SpiritualCatalog? _spiritualCatalog;
        private bool _spiritualDeathWired;

        public SpiritualMeaningCoordinator? SpiritualCoordinator => _spiritual;

        /// <summary>CORE-MECH W9: the loaded catalog is also the authored source of
        /// belief→faction affinities (BeliefStanceBridge). Retained, not reloaded.</summary>
        public SpiritualCatalog? SpiritualCatalogRef => _spiritualCatalog;

        private void SetupSpiritual()
        {
            if (_spiritual != null)
            {
                WireSpiritualDeathFeed();
                return;
            }

            string dataDir = string.IsNullOrEmpty(_dataDir) ? CatalogPath.ResolveDataDir() : _dataDir;
            var catalog = SpiritualCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            _spiritualCatalog = catalog;
            _spiritual = new SpiritualMeaningCoordinator(catalog);

            var saved = SpiritualSaveStore.TryLoad();
            if (saved != null)
                _spiritual.RestoreState(saved);

            WireSpiritualDeathFeed();
            GD.Print("[Ashfall Godot] Spiritual meaning coordinator ready.");
        }

        /// <summary>
        /// CORE-MECH W10 — a performed memorial rite leaves a trace in the
        /// campaign record. Subscribed at the SPIRITUAL OWNER (not at the call
        /// site in the shared Main.Campaign.cs) so every rite performance is
        /// covered, whoever performs it.
        ///
        /// Once-per-(mourner, rite) is enforced by the W11 one-shot primitive over
        /// the campaign consequence ledger, so a repeated vigil cannot inflate
        /// the register and the guard survives save/load. Fails closed: no verdict
        /// owner means the rite still happened, it simply left no mark.
        /// </summary>
        private void WireMemorialRiteTrace()
        {
            if (_memorialRiteTraceWired || _spiritual == null) return;
            _memorialRiteTraceWired = true;
            _spiritual.OnMemorialRitePerformed += (deceasedId, riteId) =>
            {
                if (string.IsNullOrEmpty(deceasedId) || string.IsNullOrEmpty(riteId)) return;

                var triggers = RiteTraceTriggers();
                if (!triggers.TryFire("rite." + deceasedId + "." + riteId, _simDay)) return;

                SetupVerdict();
                if (_verdict == null) return;
                _verdict.Reckoning.EnrollRiteTrace(1);
                _verdictDirty = true;

                SetupJournal();
                _journal?.TryAddRawEntry(
                    $"rite_trace_{deceasedId}_{riteId}_{_simDay}",
                    "A memorial act was entered in the register.",
                    null!, _simDay);
            };
        }

        /// <summary>W10 one-shot guard over the campaign consequence ledger.</summary>
        private Ashfall.Core.Flags.OneShotTriggerLedger RiteTraceTriggers()
            => _riteTraceTriggers ??= new Ashfall.Core.Flags.OneShotTriggerLedger(_consequenceLedger);

        private Ashfall.Core.Flags.OneShotTriggerLedger? _riteTraceTriggers;
        private bool _memorialRiteTraceWired;

        private void WireSpiritualDeathFeed()
        {
            if (_spiritualDeathWired || _spiritual == null) return;
            WireMemorialRiteTrace();
            SetupSurvivorFate();
            if (_spiritualDeathWired || _survivorFate == null) return;

            _survivorFate.OnSurvivorFate += OnSurvivorFateForSpiritual;
            foreach (var fate in _survivorFate.Fates)
            {
                if (fate == null || string.IsNullOrEmpty(fate.survivorId)) continue;
                _spiritual.RegisterDeath(fate.survivorId, fate.day);
            }
            _spiritualDeathWired = true;
        }

        private void OnSurvivorFateForSpiritual(SurvivorFateEvent fate)
        {
            if (_spiritual == null || fate == null || string.IsNullOrEmpty(fate.survivorId)) return;
            _spiritual.RegisterDeath(fate.survivorId, fate.day);
        }

        private void SaveSpiritual()
        {
            if (_spiritual == null) return;
            CaptureSection("spiritual_meaning", SpiritualSaveStore.TryCapturePersisted(_spiritual.CaptureState()));
        }

        public void TickSpiritualDay(int day)
        {
            SetupSpiritual();
            _spiritual?.TickMourning(day);
        }
    }
}
