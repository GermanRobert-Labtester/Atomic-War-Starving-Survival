// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 198-201 Host Wire & Orchestration
// Subsystems   : CBRN Hazard Warfare, Comms Array, Wasteland Ceremonies, Robotics
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Combat;
using Ashfall.Core.Crafting;
using Ashfall.Core.Narrative;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private ChemWarfareSystem? _chemWarfare;
        private CommsArraySystem? _commsArray;
        private CeremonySystem? _ceremonySystem;
        private RoboticsSystem? _robotics;

        private bool _chemWarfareDirty;
        private bool _commsArrayDirty;
        private bool _ceremonyDirty;
        private bool _roboticsDirty;

        // ── Plan 198: Biological Weapons & Chemical Warfare ─────────────

        public ChemWarfareSystem EnsureChemWarfare()
        {
            if (_chemWarfare != null) return _chemWarfare;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("chem_warfare") : new SeededRng(198);
            _chemWarfare = new ChemWarfareSystem(rng, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/chemical_weapons.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    _chemWarfare.LoadCatalog(json);
                }
            }

            var saved = ChemWarfareSaveStore.TryLoad();
            if (saved != null)
            {
                _chemWarfare.RestoreState(saved);
            }

            _chemWarfare.OnShelterResidueCreated += (sector, severity) =>
            {
                _journal?.TryAddRawEntry("chem_hazard_breach", $"Toxic chemical residue detected in {sector} (Severity: {severity})!", null!, _simDay);
            };

            _chemWarfare.OnStateChanged += () => _chemWarfareDirty = true;
            return _chemWarfare;
        }

        private void SetupChemWarfare()
        {
            EnsureChemWarfare();
        }

        private void SaveChemWarfare()
        {
            if (_chemWarfare != null)
            {
                CaptureSection("chem_warfare", ChemWarfareSaveStore.TryCapturePersisted(_chemWarfare.CaptureState()));
                _chemWarfareDirty = false;
            }
        }

        // ── Plan 199: Communications Arrays & Distant Contact ───────────

        public CommsArraySystem EnsureCommsArray()
        {
            if (_commsArray != null) return _commsArray;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("comms_array") : new SeededRng(199);
            _commsArray = new CommsArraySystem(rng, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/comms_targets.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    _commsArray.LoadCatalog(json);
                }
            }

            var saved = CommsArraySaveStore.TryLoad();
            if (saved != null)
            {
                _commsArray.RestoreState(saved);
            }

            _commsArray.OnContactEstablished += (target, lockState) =>
            {
                _journal?.TryAddRawEntry("comms_contact_locked", $"Long-range carrier lock established: {target.DisplayName} ({target.FrequencyKhz} kHz)", null!, _simDay);
            };

            _commsArray.OnStrategicStrikeRequested += (targetId, code) =>
            {
                _journal?.TryAddRawEntry("strategic_strike_uplink", $"CRITICAL: Strategic orbital uplink transmission authorized! Target: {targetId} [AUTH: {code}]", null!, _simDay);
            };

            _commsArray.OnStateChanged += () => _commsArrayDirty = true;
            return _commsArray;
        }

        private void SetupCommsArray()
        {
            EnsureCommsArray();
        }

        private void SaveCommsArray()
        {
            if (_commsArray != null)
            {
                CaptureSection("comms_array", CommsArraySaveStore.TryCapturePersisted(_commsArray.CaptureState()));
                _commsArrayDirty = false;
            }
        }

        // ── Plan 200: Wasteland Festivals & Ceremonies ───────────────────

        public CeremonySystem EnsureCeremonySystem()
        {
            if (_ceremonySystem != null) return _ceremonySystem;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("ceremony_system") : new SeededRng(200);
            _ceremonySystem = new CeremonySystem(rng, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/ceremonies.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    _ceremonySystem.LoadCatalog(json);
                }
            }

            var saved = CeremonySaveStore.TryLoad();
            if (saved != null)
            {
                _ceremonySystem.RestoreState(saved);
            }

            _ceremonySystem.OnTruceRequested += (factionId, days) =>
            {
                _journal?.TryAddRawEntry("ceremony_truce_declared", $"Festival truce negotiated with {factionId} for {days} days.", null!, _simDay);
            };

            _ceremonySystem.OnCeremonyDisaster += (ceremonyId, disasterId) =>
            {
                _journal?.TryAddRawEntry("ceremony_disaster_event", $"Incident during festival celebration: {disasterId}!", null!, _simDay);
            };

            _ceremonySystem.OnStateChanged += () => _ceremonyDirty = true;
            return _ceremonySystem;
        }

        private void SetupCeremony()
        {
            EnsureCeremonySystem();
        }

        private void SaveCeremony()
        {
            if (_ceremonySystem != null)
            {
                CaptureSection("ceremony", CeremonySaveStore.TryCapturePersisted(_ceremonySystem.CaptureState()));
                _ceremonyDirty = false;
            }
        }

        // ── Plan 201: Advanced Robotics & Pre-War AI ─────────────────────

        public RoboticsSystem EnsureRobotics()
        {
            if (_robotics != null) return _robotics;

            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("robotics") : new SeededRng(201);
            _robotics = new RoboticsSystem(rng, new GodotLog());

            string catalogPath = "res://Assets/StreamingAssets/Data/robotics.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    _robotics.LoadCatalog(json);
                }
            }

            var saved = RoboticsSaveStore.TryLoad();
            if (saved != null)
            {
                _robotics.RestoreState(saved);
            }

            _robotics.OnRogueEventTriggered += (unit) =>
            {
                _journal?.TryAddRawEntry("robot_rogue_event", $"WARNING: Automaton unit {unit.UnitId} logic core corrupted! Unit is unresponsive and rogue.", null!, _simDay);
            };

            _robotics.OnStateChanged += () => _roboticsDirty = true;
            return _robotics;
        }

        private void SetupRobotics()
        {
            EnsureRobotics();
        }

        private void SaveRobotics()
        {
            if (_robotics != null)
            {
                CaptureSection("robotics", RoboticsSaveStore.TryCapturePersisted(_robotics.CaptureState()));
                _roboticsDirty = false;
            }
        }

        // ── Daily Tick Orchestration for Plans 198-201 ──────────────────

        private void TickPlans198_201(int day, List<DayStateChangeEvent> events)
        {
            float gridWatts = _powerGrid?.System != null ? _powerGrid.System.GenerationWatts : 1000f;
            bool gridPowered = _powerGrid?.System == null || !_powerGrid.System.IsBrownout;

            if (_chemWarfare != null)
            {
                var weather = _world != null ? _world.Weather.Current : WeatherKind.Clear;
                _chemWarfare.TickCombat(weather, 0, 1);
                _chemWarfareDirty = true;
            }

            if (_commsArray != null)
            {
                _commsArray.SetPowerState(gridPowered, gridWatts);
                _commsArray.TickScan(day, 12, 0.5f);
                _commsArrayDirty = true;
            }

            if (_ceremonySystem != null)
            {
                _ceremonySystem.TickDay(day, out _);
                _ceremonyDirty = true;
            }

            if (_robotics != null)
            {
                _robotics.TickLabor(24, gridPowered, gridWatts);
                _roboticsDirty = true;
            }
        }
    }
}
