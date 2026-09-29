// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Godot;
using System;
using System.Collections.Generic;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {

        // ── Barter acquisition route: ShelterBarterSystem (Plan 54 core, Plan 147 host wire) ──

        private ShelterBarterSystem? _shelterBarter;
        private UI.ShelterBarterPanel? _shelterBarterPanel;

        public UI.ShelterBarterPanel EnsureShelterBarterPanel()
        {
            if (_shelterBarterPanel != null) return _shelterBarterPanel;

            _shelterBarterPanel = new UI.ShelterBarterPanel();
            _shelterBarterPanel.Visible = false;
            _shelterBarterPanel.OnClose += () => _shelterBarterPanel.Visible = false;
            AddChild(_shelterBarterPanel);
            return _shelterBarterPanel;
        }

        public void OpenShelterBarterPanel()
        {
            var panel = EnsureShelterBarterPanel();
            var barter = EnsureShelterBarter();
            SetupInventory();
            SetupJournal();
            panel.Bind(
                barter,
                _inventory?.Inventory ?? new Inventory(),
                _journal,
                id => _inventory?.Catalog?.Get(id));
            // Wire appraisal skill. The survivor skill system (SkillProgressionState)
            // is not yet connected to the barter panel; default to 0 until the
            // trade-discipline skill lookup is wired through SurvivorsHostSession.
            panel.SetAppraisalSkill(0);
            panel.Open();
        }

        /// <summary>
        /// The shelter barter host: four legacy Plan-54 caravans plus the
        /// contraband broker (built from the contraband activation map — the
        /// same gate authority as the stash route). Caravan arrivals and
        /// departures surface through the journal; stock is pinned per
        /// arrival and persisted, so reopening any surface cannot reroll it.
        /// </summary>
        public ShelterBarterSystem EnsureShelterBarter()
        {
            if (_shelterBarter != null) return _shelterBarter;

            // Barter trades through the campaign inventory authority; compose it
            // instead of binding a fabricated empty inventory (INV-16.3).
            SetupInventory();
            // D19a determinism contract: When _campaignDay is active, fork deterministically
            // from the campaign RNG; in pre-campaign/offline setup, use a fixed seed (147)
            // so stock generation remains deterministic and never uses unseeded System.Random.
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("shelter_barter") : new SeededRng(147);
            var inv = _inventory!.Inventory;
            var itemCatalog = _inventory!.Catalog;

            _shelterBarter = new ShelterBarterSystem(
                rng, inv,
                thermalSystem: null, // airlock-freeze gating is a Plan-54 refinement; the broker route does not depend on it
                log: new GodotLog(),
                itemLookup: id => itemCatalog?.Get(id));

            // The contraband broker — registration precedes state restore.
            _shelterBarter.RegisterCaravan(ContrabandBrokerCaravan.Build(
                LoadContrabandCatalogForHost(), ContrabandStashSystem.DefaultActivations()));

            var saved = ShelterBarterSaveStore.TryLoad();
            if (saved != null)
            {
                _shelterBarter.RestoreState(saved);
            }

            _shelterBarter.OnCaravanArrived += caravan =>
            {
                _journal?.TryAddRawEntry(
                    $"shelter_barter_arrival_{caravan.caravan_id}",
                    $"{caravan.name} is at the airlock. {caravan.description}",
                    null!, _simDay);
            };
            _shelterBarter.OnCaravanDeparted += caravan =>
            {
                _journal?.TryAddRawEntry(
                    $"shelter_barter_departure_{caravan.caravan_id}",
                    $"{caravan.name} moved on.",
                    null!, _simDay);
            };

            return _shelterBarter;
        }

        private void SetupShelterBarter()
        {
            EnsureShelterBarter();
        }

        private void SaveShelterBarter()
        {
            if (_shelterBarter != null)
            {
                CaptureSection(
                    "shelter_barter",
                    ShelterBarterSaveStore.TryCapturePersisted(_shelterBarter.CaptureState()));
            }
        }

        /// <summary>Daily barter tick: caravan arrivals, departures, restocks (day gates evaluated here).</summary>
        private void TickShelterBarterDay(int day)
        {
            _shelterBarter?.TickDay(day);
        }

    }
}
