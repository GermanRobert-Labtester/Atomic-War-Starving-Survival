// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Bestiary;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private BestiaryHostSession? _bestiary;
        private bool _bestiaryDirty;

        public BestiaryHostSession EnsureBestiary()
        {
            if (_bestiary != null) return _bestiary;
            SetupBestiary();
            return _bestiary!;
        }

        private void SetupBestiary()
        {
            if (_bestiary != null) return;

            string dataDir = CatalogPath.ResolveDataDir();
            var saved = BestiarySaveStore.TryLoad();
            _bestiary = BestiaryHostSession.Create(dataDir, saved);

            _bestiary.StateChanged += () =>
            {
                _bestiaryDirty = true;
            };
        }

        private void SaveBestiary()
        {
            if (_bestiary == null) return;

            var state = _bestiary.System.CaptureState();
            BestiarySaveStore.TrySave(state);
            string? payload = BestiarySaveStore.TryCapturePersisted(state);
            if (!string.IsNullOrEmpty(payload))
            {
                CaptureSection(BestiarySaveStore.SectionName, payload);
            }
            _bestiaryDirty = false;
        }

        public void FlushBestiarySave()
        {
            if (_bestiaryDirty)
            {
                SaveBestiary();
            }
        }

        public void ResetBestiary()
        {
            _bestiary = null;
            _bestiaryDirty = false;
        }

        public CreatureDiscoveryRecord RecordCreatureEncounter(string creatureId, string locationId = "", string witnessId = "")
        {
            var session = EnsureBestiary();
            var record = session.RecordEncounter(creatureId, _simDay, locationId, witnessId);

            _journal?.TryAddRawEntry(
                "creature_encountered",
                $"Encountered {creatureId} at {(string.IsNullOrEmpty(locationId) ? "the wasteland" : locationId)} (Witness: {(string.IsNullOrEmpty(witnessId) ? "scouts" : witnessId)}).",
                null!,
                _simDay);

            return record;
        }

        public void RecordCreatureKill(string creatureId, string locationId = "")
        {
            var session = EnsureBestiary();
            session.RecordKill(creatureId, _simDay, locationId);

            _journal?.TryAddRawEntry(
                "creature_killed",
                $"Subdued {creatureId} in combat at {(string.IsNullOrEmpty(locationId) ? "the wasteland" : locationId)}.",
                null!,
                _simDay);
        }

        public void RecordCreatureButcher(string creatureId)
        {
            var session = EnsureBestiary();
            session.RecordButcher(creatureId, _simDay);

            _journal?.TryAddRawEntry(
                "creature_harvested",
                $"Harvested biological specimens and resources from {creatureId}.",
                null!,
                _simDay);
        }

        public BestiaryCensus GetBestiaryCensus()
        {
            return EnsureBestiary().GetCensus();
        }
    }
}
