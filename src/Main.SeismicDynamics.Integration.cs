// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Godot;
using System;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SeismicDynamicsSystem? _seismicDynamics;
        private bool _seismicDirty;

        // ── Plan B68: Seismic Dynamics ──────────────────────────────

        private void SetupSeismicDynamics()
        {
            if (_seismicDynamics != null) return;
            SetupPowerGrid();
            SetupShelterThermal();
            SetupExcavationHazards();

            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork("seismic_dynamics_b68")
                : new SeededRng(68);
            _seismicDynamics = new SeismicDynamicsSystem(
                rng,
                _shelterThermal?.System,
                _excavationHazards,
                _inventory?.Inventory,
                new GodotLog());

            // Authored fault catalog overrides/extends the built-in defaults.
            string faultCatalog = System.IO.Path.Combine(_dataDir, "seismic_fault_catalog.json");
            if (System.IO.File.Exists(faultCatalog))
            {
                string json = System.IO.File.ReadAllText(faultCatalog);
                if (!string.IsNullOrWhiteSpace(json)) _seismicDynamics.LoadCatalog(json);
            }

            var saved = SeismicDynamicsSaveStore.TryLoad();
            if (saved != null) _seismicDynamics.RestoreState(saved);

            _seismicDynamics.OnQuakeOccurred += q =>
            {
                _seismicDirty = true;
                // Plan Scenario E: a severe quake requests a cryo vault breach.
                // The vault applies bounded degradation with triage time —
                // never an instant wipe.
                if (q.magnitude >= SeismicBreachMagnitudeThreshold)
                    _cryoVault?.TriggerBreach($"seismic event day {q.day}: {q.description}");
            };
            _seismicDynamics.OnEarlyWarning += (_, _) => _seismicDirty = true;
            _seismicDynamics.OnSeismicStateChanged += () => _seismicDirty = true;

            GD.Print("[Ashfall Godot] Seismic dynamics host ready (plan B68, " +
                     _seismicDynamics.Catalog.Count + " faults).");
        }

        private void SaveSeismicDynamics()
        {
            if (_seismicDynamics == null) return;
            if (CaptureSection(SeismicDynamicsSaveStore.SectionName,
                    SeismicDynamicsSaveStore.TryCapturePersisted(_seismicDynamics.CaptureState())))
                _seismicDirty = false;
        }

        /// <summary>Authored severity gate for seismic→cryo breach handoff (Scenario E).</summary>
        private const float SeismicBreachMagnitudeThreshold = 5.5f;

    }
}
