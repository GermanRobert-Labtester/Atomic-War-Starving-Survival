// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Package I — authored-data binding helpers.
//
// Four Core owners already expose a designed bind/load seam for authored game
// data, and nothing called them. Each method below feeds the EXISTING JSON
// authority into the EXISTING owner. No new system, catalog, ledger, save
// section, or gameplay rule is introduced here (AGENTS.md rules 3 and 5).
//
//   shelter_insulation_catalog.json -> ShelterThermalSystem.LoadInsulationCatalog
//   family_name_templates.json      -> GenerationalLineageExtension.LoadFamilyNameCatalog
//   relationship_bands.json         -> SurvivorRelationsState.LoadBandsCatalog
//   items.json tradeValue           -> CaravanTradeNetworkSystem.SetItemValueResolver
// ============================================================================

using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.IO;
using Ashfall.Core.Relations;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        /// <summary>Reads one authored data file as text. Read-only; nothing is cached.</summary>
        private string ReadAuthoredJson(string fileName)
        {
            try
            {
                string dir = string.IsNullOrEmpty(_dataDir) ? CatalogPath.ResolveDataDir() : _dataDir;
                string path = Path.Combine(dir, fileName);
                if (!File.Exists(path)) return string.Empty;
                return File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Ashfall Godot] authored data unreadable ({fileName}): {ex.Message}");
                return string.Empty;
            }
        }

        // ── 1. Storm sealing: authored insulation catalog ───────────────

        /// <summary>
        /// Loads the authored insulation table into the live thermal owner.
        ///
        /// The owner registers four built-in defs in its constructor, but
        /// shelter_insulation_catalog.json authors FIVE — including
        /// "insul_storm_sealing", the id ShelterThermalHostSession's storm-sealing
        /// routine passes to RetrofitInsulation(). With the catalog unloaded that
        /// call returned Failed("unknown_insulation") for every room, so the live
        /// routine sealed nothing. Loading the authored rows also makes the four
        /// existing tiers answer to data instead of to built-in literals.
        /// </summary>
        /// <returns>How many authored rows the owner accepted.</returns>
        public int BindAuthoredInsulationCatalog()
        {
            if (_shelterThermal == null) return 0;
            string json = ReadAuthoredJson("shelter_insulation_catalog.json");
            if (string.IsNullOrWhiteSpace(json)) return 0;

            int before = _shelterThermal.System.InsulationCatalog.Count;
            _shelterThermal.System.LoadInsulationCatalog(json);
            int after = _shelterThermal.System.InsulationCatalog.Count;
            GD.Print($"[Ashfall Godot] insulation catalog: {after - before} authored row(s) added " +
                     $"(total {after})");
            return after - before;
        }

        // ── 2. Genealogy: authored family-name templates ────────────────

        /// <summary>
        /// Loads the authored cultural archetypes and naming templates into the
        /// lineage owner the genealogy session already wraps. The extension reads
        /// this catalog when assigning surnames, but nothing ever populated it, so
        /// authored culture data could not influence names at all.
        /// </summary>
        public bool BindAuthoredFamilyNames()
        {
            if (_genealogy == null) return false;
            string json = ReadAuthoredJson("family_name_templates.json");
            if (string.IsNullOrWhiteSpace(json)) return false;

            try
            {
                _genealogy.Lineage.LoadFamilyNameCatalog(json);
                GD.Print("[Ashfall Godot] family-name catalog bound to the lineage owner");
                return true;
            }
            catch (Exception ex)
            {
                // The loader validates schema_version and throws on a bad table.
                // Surface it; never silently fall back to a different name source.
                GD.PrintErr($"[Ashfall Godot] family-name catalog rejected: {ex.Message}");
                return false;
            }
        }

        // ── 3. Relationships: authored affinity bands ───────────────────

        /// <summary>
        /// Replaces the relations owner's built-in band table with the authored
        /// one. Bands are live gameplay authority: the owner's band loop turns
        /// affinity into caregiving/training/morale modifiers, and until now those
        /// thresholds lived only as C# literals even though relationship_bands.json
        /// was shipped alongside them.
        /// </summary>
        public int BindAuthoredRelationshipBands()
        {
            if (_survivorRelations == null) return 0;
            string json = ReadAuthoredJson("relationship_bands.json");
            if (string.IsNullOrWhiteSpace(json)) return 0;

            try
            {
                var catalog = new SystemTextJsonSerializer()
                    .Deserialize<RelationshipBandsCatalog>(json);
                if (catalog?.Bands == null || catalog.Bands.Count == 0) return 0;

                _survivorRelations.System.LoadBandsCatalog(catalog);
                int count = _survivorRelations.System.Bands.Count;
                GD.Print($"[Ashfall Godot] relationship bands: {count} authored band(s) authoritative");
                return count;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Ashfall Godot] relationship band catalog rejected: {ex.Message}");
                return 0;
            }
        }

        // ── 4. Caravan trade: canonical item value ──────────────────────

        /// <summary>
        /// Points the caravan network's designed value-provider seam at the item
        /// catalog, which is the actual price authority. Without a resolver the
        /// owner falls back to a hardcoded switch of item ids — six of which do not
        /// exist in items.json at all — so caravan prices silently ignored authored
        /// tradeValue. Multipliers stay in the caravan owner; only the base value's
        /// source changes, and a stale duplicate is removed in the process.
        /// </summary>
        public bool BindCanonicalCaravanItemValue()
        {
            if (_caravanTradeNetwork == null) return false;
            var itemCatalog = _inventory?.Catalog;
            if (itemCatalog == null) return false;

            _caravanTradeNetwork.SetItemValueResolver(itemId =>
            {
                var def = itemCatalog.Get(itemId);
                return def != null ? def.tradeValue : 0f;
            });
            GD.Print("[Ashfall Godot] caravan base prices now read the canonical item catalog");
            return true;
        }

        /// <summary>Binds every Package I authored-data seam. Idempotent.</summary>
        public void BindPackageIAuthoredData()
        {
            BindAuthoredInsulationCatalog();
            BindAuthoredFamilyNames();
            BindAuthoredRelationshipBands();
            BindCanonicalCaravanItemValue();
        }

        public string PackageIBindingStatusLine()
        {
            int insulation = _shelterThermal?.System.InsulationCatalog.Count ?? 0;
            int bands = _survivorRelations?.System.Bands.Count ?? 0;
            bool names = _genealogy != null;
            bool prices = _caravanTradeNetwork != null;
            return $"insulation {insulation} · bands {bands} · family-names {names} · caravan-value {prices}";
        }
    }
}
