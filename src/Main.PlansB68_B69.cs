// SPDX-License-Identifier: MIT
using Godot;
using System;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    // ─────────────────────────────────────────────────────────────────
    // Plans B68 / B69 — Geological Seismic Monitoring & Cryo Vault.
    // Host wiring only: the Core systems own all simulation. This partial
    // constructs, ticks, saves and cross-wires them (Scenario E: severe
    // quake → cryo vault breach request).
    // ─────────────────────────────────────────────────────────────────
    public partial class Main
    {
        private SeismicDynamicsSystem? _seismicDynamics;
        private bool _seismicDirty;

        private CryoVaultSystem? _cryoVault;
        private bool _cryoVaultDirty;

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

        // ── Plan B69: Cryo Vault ────────────────────────────────────

        private void SetupCryoVault()
        {
            if (_cryoVault != null) return;
            SetupInventory();
            SetupPowerGrid();

            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork("cryo_vault_b69")
                : new SeededRng(69);

            // Radiation exposure: canonical survivor dose, normalized to the
            // vault's 0..1 daily severity port (host presentation mapping).
            // Power: the cryo vault room participates in the canonical grid —
            // brownout or breaker/trip on the vault room destabilizes storage.
            _cryoVault = new CryoVaultSystem(
                rng,
                _inventory?.Inventory,
                radiationExposureProvider: () => Math.Clamp((_holdfastRuntime?.Radiation ?? 0f) / 50f, 0f, 1f),
                powerAvailableProvider: IsCryoVaultPowered,
                log: new GodotLog());

            string cultivarsPath = System.IO.Path.Combine(_dataDir, CryoCultivarCatalogLoader.FileName);
            if (System.IO.File.Exists(cultivarsPath))
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                _cryoVault.LoadCatalogContent(CryoCultivarCatalogLoader.Load(_dataDir, files, json));
            }

            var saved = CryoVaultSaveStore.TryLoad();
            if (saved != null) _cryoVault.RestoreState(saved);

            _cryoVault.OnVaultWarning += _ => _cryoVaultDirty = true;
            _cryoVault.OnBreachStarted += _ => _cryoVaultDirty = true;
            _cryoVault.OnSampleReleased += (_, _, _, _) => _cryoVaultDirty = true;
            _cryoVault.OnCultivarReleased += OnCryoCultivarReleased;
            _cryoVault.OnSampleFailed += _ => _cryoVaultDirty = true;
            _cryoVault.OnStateChanged += () => _cryoVaultDirty = true;

            GD.Print("[Ashfall Godot] Cryo vault host ready (plan B69, " +
                     _cryoVault.Catalog.Count + " cultivars).");
        }

        private void OnCryoCultivarReleased(string canisterId, CryoCultivarDef def, int viability)
        {
            _cryoVaultDirty = true;
            if (def == null) return;

            // 1. Hydroponic trait overlay (stabilized traits)
            if (def.traits != null && def.traits.Length > 0)
            {
                var hydro = _hydroponicBiomes;
                if (hydro != null)
                {
                    foreach (var trait in def.traits)
                    {
                        if (string.Equals(trait, "frost_hardy", StringComparison.OrdinalIgnoreCase))
                            hydro.UnlockStabilizedTrait("Trait_Cold_Hardy");
                        else if (string.Equals(trait, "radiation_tolerant", StringComparison.OrdinalIgnoreCase))
                            hydro.UnlockStabilizedTrait("Trait_Drought_Resistant");
                        else if (string.Equals(trait, "rapid_growth", StringComparison.OrdinalIgnoreCase))
                            hydro.UnlockStabilizedTrait("Trait_Double_Harvest");
                    }
                    _hydroponicBiomesDirty = true;
                }
            }

            // 2. Agriculture strain unlocking
            if (_agriculture != null && _agriculture.System != null)
            {
                var agri = _agriculture.System;
                var catalog = agri.Catalog;
                if (catalog?.strains != null)
                {
                    foreach (var strain in catalog.strains)
                    {
                        if (strain != null && string.Equals(strain.seed_item_id, def.recovery_item_id, StringComparison.Ordinal))
                        {
                            agri.UnlockStrain(strain.id);
                            _agricultureDirty = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The vault is powered when the grid is healthy and the cryo room's
        /// breaker is closed. Rooms persisted before plan B69 lack the cryo
        /// room; the brownout state is then the authoritative signal.
        /// </summary>
        private bool IsCryoVaultPowered()
        {
            var grid = _powerGrid?.System;
            if (grid == null) return true;
            bool hasRoom = grid.Rooms.Any(r => r.RoomId == "room_cryo_vault");
            if (!hasRoom) return !grid.IsBrownout;
            return grid.IsRoomPowered("room_cryo_vault");
        }

        private void SaveCryoVault()
        {
            if (_cryoVault == null) return;
            if (CaptureSection(CryoVaultSaveStore.SectionName,
                    CryoVaultSaveStore.TryCapturePersisted(_cryoVault.CaptureState())))
                _cryoVaultDirty = false;
        }
    }
}
