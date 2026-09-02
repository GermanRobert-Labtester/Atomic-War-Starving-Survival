// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 182-185 Host Wire & Orchestration
// Subsystems   : Aviation, Forced Labor, Chemical Engineering & Narcotics,
//                Elections & Settlement Politics
// ============================================================================
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Factions;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private AviationSystem? _aviation;
        private ForcedLaborSystem? _forcedLabor;
        private NarcoticsSystem? _narcotics;
        private PoliticsSystem? _politics;

        // ── Plan 182: Aviation & Jury-Rigged Aircraft ─────────────────────

        public AviationSystem EnsureAviation()
        {
            if (_aviation != null) return _aviation;

            _aviation = new AviationSystem();

            string catalogPath = "res://Assets/StreamingAssets/Data/aircraft_parts.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    _aviation.LoadCatalog(json, new SystemTextJsonSerializer());
                }
            }

            var saved = AviationSaveStore.TryLoad();
            if (saved != null)
            {
                _aviation.RestoreState(saved);
            }

            _aviation.OnFlightCrashed += (plan, reason) =>
            {
                _journal?.TryAddRawEntry("aviation_crash", $"CRITICAL: Flight {plan.flightId} has crashed! Reason: {reason}. Rescue requested.", null!, _simDay);
            };

            return _aviation;
        }

        private void SetupAviation()
        {
            EnsureAviation();
        }

        private void SaveAviation()
        {
            if (_aviation != null)
            {
                CaptureSection("aviation", AviationSaveStore.TryCapturePersisted(_aviation.CaptureState()));
            }
        }

        // ── Plan 183: Forced Labor, Captivity & Rebellion ─────────────────

        public ForcedLaborSystem EnsureForcedLabor()
        {
            if (_forcedLabor != null) return _forcedLabor;

            _forcedLabor = new ForcedLaborSystem();

            string catalogPath = "res://Assets/StreamingAssets/Data/labor_camps.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    _forcedLabor.LoadCatalog(json, new SystemTextJsonSerializer());
                }
            }

            var saved = ForcedLaborSaveStore.TryLoad();
            if (saved != null)
            {
                _forcedLabor.RestoreState(saved);
            }

            _forcedLabor.OnRebellionTriggered += (msg) =>
            {
                _journal?.TryAddRawEntry("penal_rebellion", $"ALERT: Captive rebellion erupted! {msg}", null!, _simDay);
            };

            return _forcedLabor;
        }

        private void SetupForcedLabor()
        {
            EnsureForcedLabor();
        }

        private void SaveForcedLabor()
        {
            if (_forcedLabor != null)
            {
                CaptureSection("forced_labor", ForcedLaborSaveStore.TryCapturePersisted(_forcedLabor.CaptureState()));
            }
        }

        // ── Plan 184: Chemical Engineering & Narcotics ────────────────────

        public NarcoticsSystem EnsureNarcotics()
        {
            if (_narcotics != null) return _narcotics;

            _narcotics = new NarcoticsSystem();

            string catalogPath = "res://Assets/StreamingAssets/Data/narcotics.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    _narcotics.LoadCatalog(json, new SystemTextJsonSerializer());
                }
            }

            var saved = NarcoticsSaveStore.TryLoad();
            if (saved != null)
            {
                _narcotics.RestoreState(saved);
            }

            _narcotics.OnOverdoseEmergency += (survivorId, msg) =>
            {
                _journal?.TryAddRawEntry("chem_overdose", $"MEDICAL EMERGENCY: Survivor {survivorId} suffered an overdose! {msg}", null!, _simDay);
            };

            return _narcotics;
        }

        private void SetupNarcotics()
        {
            EnsureNarcotics();
        }

        private void SaveNarcotics()
        {
            if (_narcotics != null)
            {
                CaptureSection("narcotics", NarcoticsSaveStore.TryCapturePersisted(_narcotics.CaptureState()));
            }
        }

        // ── Plan 185: Elections, Leadership & Settlement Politics ─────────

        public PoliticsSystem EnsurePolitics()
        {
            if (_politics != null) return _politics;

            _politics = new PoliticsSystem();

            string catalogPath = "res://Assets/StreamingAssets/Data/political_policies.json";
            if (Godot.FileAccess.FileExists(catalogPath))
            {
                using var file = Godot.FileAccess.Open(catalogPath, Godot.FileAccess.ModeFlags.Read);
                if (file != null)
                {
                    string json = file.GetAsText();
                    _politics.LoadCatalog(json, new SystemTextJsonSerializer());
                }
            }

            var saved = PoliticsSaveStore.TryLoad();
            if (saved != null)
            {
                _politics.RestoreState(saved);
            }

            // Bind leadership designation to LeadershipSystem
            _politics.OnLeaderDesignated += (newLeaderId) =>
            {
                if (_survivorSocial != null)
                {
                    _survivorSocial.DesignateLeader(newLeaderId);
                }
            };

            _politics.OnCoupTriggered += (msg) =>
            {
                _journal?.TryAddRawEntry("political_coup", $"CRISIS: Armed coup d'etat in progress! {msg}", null!, _simDay);
            };

            return _politics;
        }

        private void SetupPolitics()
        {
            EnsurePolitics();
        }

        private void SavePolitics()
        {
            if (_politics != null)
            {
                CaptureSection("settlement_politics", PoliticsSaveStore.TryCapturePersisted(_politics.CaptureState()));
            }
        }

        // ── Daily Simulation Advance Hook ─────────────────────────────────

        public void TickPlans182_185(int currentDay)
        {
            var rng = _campaignDay != null ? _campaignDay.Rng.Fork("plans182_185") : new SeededRng(182);

            // Plan 182: Aviation tick for active flights
            var av = EnsureAviation();
            var active = new List<FlightPlan>(av.ActiveFlights);
            foreach (var flight in active)
            {
                av.AdvanceFlightTick(flight.flightId, 4.0f, 15.0f, 0.8f, -5.0f, 0.2f, rng);
            }

            // Plan 183: Forced Labor daily shift
            EnsureForcedLabor().AdvanceDailyShift(rng);

            // Plan 184: Narcotics medical tick (24h tick)
            EnsureNarcotics().AdvanceMedicalTick(24.0f, rng);

            // Plan 185: Politics daily progression
            float foodSat = 0.8f;
            float secSat = 0.75f;
            float cruelty = EnsureForcedLabor().CrueltyIndex;
            EnsurePolitics().AdvanceDailyPolitics(foodSat, secSat, cruelty, 0, rng);
        }
    }
}
