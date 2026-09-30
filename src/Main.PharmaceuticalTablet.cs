// SPDX-License-Identifier: MIT
// PLAN-PHARMACEUTICAL-TRUTH-167 — tablet works host wiring.
// Reagents/tablets stay canonical inventory items; this partial owns press
// condition, the active batch, and the output buffer with its own save key.

using System;
using Godot;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PharmaceuticalTabletHostSession? _tabletWorks;
        private bool _tabletWorksDirty;

        public PharmaceuticalTabletHostSession? TabletWorks => _tabletWorks;

        public void SetupPharmaceuticalTablet()
        {
            if (_tabletWorks != null) return;
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("pharmaceutical_tablet") : new Ashfall.Core.SeededRng(167);
            _tabletWorks = new PharmaceuticalTabletHostSession(rng);
            _tabletWorks.LoadCatalog(_dataDir);
            var saved = PharmaceuticalTabletSaveStore.TryLoad();
            if (saved != null) _tabletWorks.RestoreState(saved);

            BindTabletInventory();

            _tabletWorks.StateChanged += () => _tabletWorksDirty = true;
        }

        private void BindTabletInventory()
        {
            if (_tabletWorks == null || _inventory == null) return;
            _tabletWorks.BindInventory(
                id => _inventory.Inventory.CountById(id),
                (id, qty) => true,
                (id, qty) => _inventory.Inventory.AddById(id, qty),
                (id, qty) => _inventory.Inventory.RemoveById(id, qty));
        }

        public void SavePharmaceuticalTablet()
        {
            if (_tabletWorks == null) return;
            var state = _tabletWorks.CaptureState();
            if (CaptureSection("pharmaceutical_tablet", PharmaceuticalTabletSaveStore.TryCapturePersisted(state)))
            {
                _tabletWorksDirty = false;
            }
        }

        public void FlushPharmaceuticalTabletIfDirty()
        {
            if (_tabletWorksDirty) SavePharmaceuticalTablet();
        }

        public void ResetPharmaceuticalTablet()
        {
            _tabletWorks = null;
            _tabletWorksDirty = false;
        }

        public void TickPharmaceuticalTablet(int day)
        {
            _tabletWorks?.TickDay(day);
        }
    }
}
