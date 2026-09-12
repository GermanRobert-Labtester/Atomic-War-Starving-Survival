// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private ExpansionEnrichmentCatalog _enrichment = null!;
        private SurvivorEnrichmentService _enrichmentService = null!;

        private void SetupEnrichment()
        {
            if (_enrichment != null) return;
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new ExpansionEnrichmentCatalogLoader(fileIO, json, new GodotLog());
            _enrichment = loader.Load(_dataDir);
            _enrichmentService = new SurvivorEnrichmentService(_enrichment);
        }

        private void CheckKeepsakeRecognition(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || _enrichment == null || _journal == null || _survivors?.Roster == null) return;
            foreach (var sid in _enrichment.GetEnrichedSurvivorIds())
            {
                var fields = _enrichment.GetSurvivorFields(sid);
                if (fields != null && string.Equals(fields.personal_keepsake_item_id, itemId, StringComparison.Ordinal))
                {
                    var surv = _survivors.Roster.Find(sid);
                    if (surv != null && surv.isAlive)
                    {
                        string discKey = $"keepsake_recognized:{sid}:{itemId}";
                        if (_journal.Knowledge.Discover(discKey))
                        {
                            var def = _survivors.Roster.FindDefinition(sid);
                            string survName = !string.IsNullOrEmpty(def?.displayName) ? def.displayName : sid;
                            var itemDef = _inventory?.Catalog?.Get(itemId);
                            string itemName = !string.IsNullOrEmpty(itemDef?.displayName) ? itemDef.displayName : SurvivorEnrichmentService.FormatId(itemId);
                            _journal.TryAddRawEntry(discKey, $"{survName} recognized an associated keepsake in the bunker: {itemName}.", null!, _simDay);
                            _journalDirty = true;
                        }
                    }
                }
            }
        }
    }
}
