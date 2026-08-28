using Godot;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using AtomicWar.Journal;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Economy;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;
using AtomicWar.GodotApp.Economy;
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.GodotApp.Muster;
using AtomicWar.GodotApp.Dose;
using AtomicWar.GodotApp.UtilityAI;
using AtomicWar.GodotApp.Radio;
using AtomicWar.GodotApp.Audio;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        // ── Medical fields (GAP-ARCH-01 Phase 1) ──
        private MedicalHostSession _medical = null!;
        private bool _medicalDirty;
        private AtomicWar.GodotApp.DiseaseHostSession _disease = null!;

        private string DiseaseStatusLine()
        {
            if (_expansions?.Disease == null) return "DISEASE WARD: offline";
            if (_disease == null) SetupDisease();
            if (_disease == null) return "DISEASE WARD: offline";
            var s = _disease.Engine.GetSnapshot();
            return $"——— DISEASE WARD ———\n" +
                $"infections {s.total_infected} · quarantined {s.total_quarantined} · " +
                $"outbreaks {s.total_outbreaks} (prevented {s.total_outbreaks_prevented}) · " +
                $"recovered {s.total_recovered} · deaths {s.total_deaths}" +
                (s.total_contagious > 0 ? "  ★ " + s.total_contagious + " CONTAGIOUS UNISOLATED" : "");
        }

        private void FlushMedicalIfDirty()
        {
            if (_medicalDirty) SaveMedical();
        }

        private void SetupMedical()
        {
            if (_medical != null) return;
            _medical = MedicalHostSession.Create(_dataDir);
            _medical.StateChanged += () =>
            {
                _medicalDirty = true;
                _medicalPanel?.RefreshView();
            };
            GD.Print("[Ashfall Godot] Medical host ready.");
        }

        private void SaveMedical()
        {
            if (_medical == null) return;
            if (CaptureSection("medical", MedicalSaveStore.TryCapturePersisted(_medical.CaptureSave())))
            {
                _medicalDirty = false;
                GD.Print("[Ashfall Godot] Medical save written.");
            }
        }

        private MedicalWardHostSession _medicalWardSession = null!;
        private MedicalWardPanel _medicalWardPanel = null!;

        private void SetupMedicalWard()
        {
            if (_medicalWardSession != null) return;
            var beds = new List<Ashfall.Core.Medical.MedicalBed>
            {
                new Ashfall.Core.Medical.MedicalBed("bed_general_a", "General A", Ashfall.Core.Medical.MedicalBedCategory.General),
                new Ashfall.Core.Medical.MedicalBed("bed_general_b", "General B", Ashfall.Core.Medical.MedicalBedCategory.General),
                new Ashfall.Core.Medical.MedicalBed("bed_surgical", "Surgical", Ashfall.Core.Medical.MedicalBedCategory.Surgical),
                new Ashfall.Core.Medical.MedicalBed("bed_isolation", "Isolation", Ashfall.Core.Medical.MedicalBedCategory.Isolation, isolation: true),
                new Ashfall.Core.Medical.MedicalBed("bed_chelation", "Chelation", Ashfall.Core.Medical.MedicalBedCategory.Chelation)
            };
            var procs = new List<Ashfall.Core.Medical.MedicalProcedureDef>
            {
                new Ashfall.Core.Medical.MedicalProcedureDef("proc_bandage", "Bandage", "MedicalSystem"),
                new Ashfall.Core.Medical.MedicalProcedureDef("proc_chelation", "Chelation", "DoseLedgerSystem"),
                new Ashfall.Core.Medical.MedicalProcedureDef("proc_surgery", "Surgery", "MedicalSystem")
            };
            _medicalWard = new Ashfall.Core.Medical.MedicalWardSystem(
                new Ashfall.Core.Medical.MedicalWardState(), beds, procs);
            _medicalWardSession = new MedicalWardHostSession(_medicalWard);
            _medicalWardSession.Procedures = procs;
            _medicalWardSession.SimDay = _simDay;
            _medicalWardSession.StateChanged += () => _medicalWardDirty = true;
            _medicalWard.OnWardChanged += _ => _medicalWardDirty = true;
            LoadMedicalWard();
            if (_medicalWardPanel == null)
            {
                _medicalWardPanel = new MedicalWardPanel();
                _medicalWardPanel.Bind(_medicalWardSession);
                _medicalWardPanel.Visible = false;
                AddChild(_medicalWardPanel);
            }
        }

        private void SaveMedicalWard()
        {
            if (_medicalWardSession == null) return;
            try
            {
                _medicalWardSession.SimDay = _simDay;
                _medicalWardSession.Save();
                _medicalWardDirty = false;
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] MedicalWard save failed: " + e.Message);
            }
        }

        private void LoadMedicalWard()
        {
            try
            {
                var loaded = MedicalWardSaveStore.TryLoad();
                if (loaded != null)
                {
                    _medicalWardSession?.RestoreSave(loaded);
                    if (_medicalWardSession != null)
                        _medicalWard = _medicalWardSession.System;
                }
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] MedicalWard load failed: " + e.Message);
            }
        }

        private void SaveDisease()
        {
            if (_disease == null) return;
            try
            {
                CaptureSection("disease", DiseaseSaveStore.TryCapturePersisted(_disease.Engine.CaptureState()));
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] Disease save failed: " + e.Message);
            }
        }

        private void SetupDisease()
        {
            if (_disease != null) return;
            SetupExpansions();
            var engine = _expansions.Disease;
            if (engine == null)
            {
                GD.PrintErr("[Ashfall Godot] Disease Expansion missing from expansion hub; ward offline.");
                return;
            }
            _disease = new AtomicWar.GodotApp.DiseaseHostSession(engine, _expansions.DiseaseData);
            // The exposure pool is the people actually in the shelter tonight
            // (duty-roster home occupants). Pure presentation wiring — the
            // engine owns all rules.
            _disease.BindPopulationProvider(() =>
            {
                var occupants = BuildHomeOccupantSnapshot();
                var ids = new List<string>();
                for (int i = 0; i < occupants.Count; i++)
                {
                    var o = occupants[i];
                    if (o != null && !string.IsNullOrEmpty(o.survivorId))
                        ids.Add(o.survivorId);
                }
                return ids;
            });
            // Ward state rides the expansion-hub save (restored above); any
            // change marks the hub dirty so nothing is lost at day end.
            _disease.StateChanged += () => { _expansionHubDirty = true; };
            GD.Print("[Ashfall Godot] Disease Expansion ward ready (contagion · quarantine · outbreak).");
        }

        private void CloseMedicalPanel()
        {
            _medicalPanel.Visible = false;
        }

    }
}
