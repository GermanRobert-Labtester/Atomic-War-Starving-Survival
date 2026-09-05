// SPDX-License-Identifier: MIT
// ASHFALL chemical synthesis host triad (save enrollment for chemical_synthesis section).

using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.IO;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ChemicalSynthesisHostSession? _chemicalSynthesis;
        private bool _chemicalSynthesisDirty;

        private void SetupChemicalSynthesis()
        {
            if (_chemicalSynthesis != null) return;

            SetupInventory();
            var fileIO = CatalogPath.CreateFileIOForDataDir(_dataDir);
            var json = new SystemTextJsonSerializer();
            var catalog = ChemicalSynthesisCatalogLoader.Load(_dataDir, fileIO, json)
                          ?? new ChemicalSynthesisCatalog(null);
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("chemical_synthesis") : new SeededRng(87);
            var system = new ChemicalSynthesisSystem(
                _inventory?.Inventory ?? new Ashfall.Core.Inventory.Inventory(),
                catalog,
                rng,
                new GodotLog());

            var saved = ChemicalSynthesisSaveStore.TryLoad();
            if (saved != null)
                system.RestoreState(saved);

            _chemicalSynthesis = new ChemicalSynthesisHostSession(system, catalog);
            _chemicalSynthesis.StateChanged += () => _chemicalSynthesisDirty = true;
        }

        private void SaveChemicalSynthesis()
        {
            if (_chemicalSynthesis == null) return;
            if (CaptureSection(
                    "chemical_synthesis",
                    ChemicalSynthesisSaveStore.TryCapturePersisted(_chemicalSynthesis.System.CaptureState())))
            {
                _chemicalSynthesisDirty = false;
            }
        }

        private void FlushChemicalSynthesisIfDirty()
        {
            if (_chemicalSynthesisDirty) SaveChemicalSynthesis();
        }
    }
}
