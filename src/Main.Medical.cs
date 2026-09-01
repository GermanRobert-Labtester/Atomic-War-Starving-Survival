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

            // Plan 60 / D6 — a vigil the player keeps is care, and care must be
            // recorded where the campaign remembers it: the consequence ledger already
            // rides the save, so no new persistence is introduced. The names worth
            // reciting are the dead this holdfast has already kept.
            SetupMemorial();
            _medical.BindVigilContext(
                () => _simDay,
                _consequenceLedger,
                () =>
                {
                    var remembered = new List<string>();
                    if (_memorial?.Entries != null)
                    {
                        for (int i = 0; i < _memorial.Entries.Count && remembered.Count < 6; i++)
                        {
                            var entry = _memorial.Entries[i];
                            if (entry == null || string.IsNullOrEmpty(entry.SurvivorId)) continue;
                            remembered.Add(FormatSurvivorName(entry.SurvivorId));
                        }
                    }
                    return remembered;
                });

            GD.Print("[Ashfall Godot] Medical host ready.");
        }

        /// <summary>
        /// Task #133: construct and bind the unified medical pipeline once the
        /// inventory, survivors, and Phase-0 sessions exist. Idempotent.
        /// </summary>
        private void EnsureMedicalPipeline()
        {
            SetupMedical();
            if (_medical.Pipeline != null) return;
            SetupSurvivors();
            SetupInventory();
            SetupPhase0();

            var pipeline = new Ashfall.Core.Medical.MedicalPipelineCoordinator(
                _inventory.Inventory,
                new Ashfall.Core.Medical.DiagnosisKnowledgeStore(),
                new Ashfall.Core.Medical.MedicalReservationLedger(),
                new Ashfall.Core.Medical.MedicalProcedureSchedule(),
                sv => ResolvePatientAvailability(sv.Value),
                () => _simDay);

            var respiratoryDef = new Ashfall.Core.Medical.AfflictionId(Ashfall.Core.Medical.MedicalTreatmentCatalog.RespiratoryDegenerationId);
            var radiationDef = new Ashfall.Core.Medical.AfflictionId(Ashfall.Core.Medical.MedicalTreatmentCatalog.RadiationSicknessId);

            pipeline.RegisterHandler(new Ashfall.Core.Medical.RespiratoryAfflictionHandler(_phase0.Respiratory));
            pipeline.RegisterHandler(new Ashfall.Core.Medical.RadiationSicknessAfflictionHandler(
                getDose: id => _survivors.RadStateFor(id)?.RadiationDose ?? 0f,
                getPhaseName: GetRadiationPhaseName,
                hasAcuteSickness: id => _survivors.RadStateFor(id)?.HasAcuteRadiationSickness ?? false,
                applyIodine: id => { _survivors.AdministerIodine(id); return true; },
                applyAntiRad: (id, rads) => { _survivors.AdministerAntiRad(id, rads); return true; }));
            pipeline.RegisterHandler(new Ashfall.Core.Medical.HealthDeficitAfflictionHandler(
                getHealth: id => _survivors.Find(id)?.Health ?? 100f,
                getMaxHealth: id => _survivors.Find(id)?.MaxHealthCap ?? 100f,
                applyHeal: (id, amount) => { _survivors.HealSurvivor(id, amount); return true; }));

            // Task #133 P1b — chemical-dependency detox starts flow through
            // the pipeline; the shared engine keeps every withdrawal clock.
            pipeline.RegisterHandler(new Ashfall.Core.Medical.ChemicalDependencyAfflictionHandler(_medical.Engine));

            // Task #133 P1c — observe-only psychology projection: Phase-0
            // combat trauma, flashbacks, and guilt insomnia surface as
            // read-only patient rows. The handlers write nothing and own no
            // clock; phase0_psychology keeps every rule and tick.
            pipeline.RegisterHandler(new Ashfall.Core.Medical.GuiltInsomniaAfflictionHandler(_phase0.Guilt));
            pipeline.RegisterHandler(new Ashfall.Core.Medical.SomaticFlashbackAfflictionHandler(_phase0.Flashbacks));
            pipeline.RegisterHandler(new Ashfall.Core.Medical.CombatTraumaAfflictionHandler(_phase0.CombatTrauma));

            // Task #133 P1 — disease write-path: one handler per authored
            // disease plus the four camp-wide vector protocols. The disease
            // domain keeps every clinical rule; the pipeline owns the
            // validate → consume → apply transaction.
            SetupDisease();
            if (_disease != null)
            {
                Ashfall.Core.Medical.DiseaseAfflictionHandler.RegisterAll(pipeline, _disease.Engine, _disease.Catalog);
                Ashfall.Core.Medical.DiseaseProtocolHandler.RegisterAll(pipeline, _disease.Engine, () => _simDay);

                // Auto-suspect on live infection (never confirms — the player
                // identifies the illness explicitly through the examination).
                _disease.Engine.OnInfection += (survivorId, diseaseId) =>
                {
                    if (Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var sv)
                        && Ashfall.Core.Medical.AfflictionId.IsValid(diseaseId, out _))
                        pipeline.SuspectFromEvidence(sv, new Ashfall.Core.Medical.AfflictionId(diseaseId), _simDay, "infection_event");
                };

                MigrateDiseaseSuspicions(pipeline);
            }

            // Auto-suspect: domain threshold crossings raise the shelter's
            // knowledge to Suspected (never Confirmed — confirmation is explicit).
            _phase0.Respiratory.OnSevereCoughStarted += survivorId =>
            {
                if (Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var sv))
                    pipeline.SuspectFromEvidence(sv, respiratoryDef, _simDay, "severe_cough_threshold");
            };
            _phase0.RadiationPhase.OnPhaseChanged += (survivorId, oldPhase, newPhase) =>
            {
                if (newPhase != Ashfall.Core.Radiation.RadiationSicknessPhase.Healthy &&
                    Ashfall.Core.Survivors.SurvivorId.TryParse(survivorId, out var sv))
                    pipeline.SuspectFromEvidence(sv, radiationDef, _simDay, "radiation_phase_" + newPhase);
            };

            _medical.BindPipeline(pipeline);
            // Task #133 P1b: share the pipeline with the ward and chem-dep
            // sessions when already constructed (they backfill from
            // _medical.Pipeline otherwise, covering every setup order).
            if (_medicalWardSession != null) _medicalWardSession.Pipeline = pipeline;
            if (_chemicalDependency != null) _chemicalDependency.Pipeline = pipeline;
            MigrateLegacyMedicalDiagnoses();
            GD.Print("[Ashfall Godot] Medical pipeline bound (Task #133).");
        }

        /// <summary>
        /// Task #133 P1: restored infections that carry no diagnosis knowledge
        /// are raised to Suspected (never Confirmed) so a load does not wipe
        /// the suspicion trail. Idempotent — SuspectFromEvidence only moves
        /// Unknown episodes.
        /// </summary>
        private void MigrateDiseaseSuspicions(Ashfall.Core.Medical.MedicalPipelineCoordinator pipeline)
        {
            if (_disease == null || _survivors == null) return;
            var catalog = _disease.Catalog;
            for (int i = 0; i < _survivors.RosterState.Count; i++)
            {
                var survivor = _survivors.RosterState[i];
                if (survivor == null || !survivor.IsAlive) continue;
                for (int d = 0; d < catalog.Diseases.Count; d++)
                {
                    var disease = catalog.Diseases[d];
                    if (disease == null || string.IsNullOrEmpty(disease.id)) continue;
                    if (!Ashfall.Core.Medical.AfflictionId.IsValid(disease.id, out _)) continue;
                    if (!_disease.Engine.TryGetInfection(survivor.Id, disease.id, out int _, out bool _)) continue;
                    pipeline.SuspectFromEvidence(
                        new Ashfall.Core.Survivors.SurvivorId(survivor.Id),
                        new Ashfall.Core.Medical.AfflictionId(disease.id),
                        _simDay, "restored_infection");
                }
            }
        }

        /// <summary>Canonical lifecycle gate for the pipeline (Task #132 semantics; roster is the live authority until the entity store is host-mounted).</summary>
        private Ashfall.Core.Medical.PatientAvailability ResolvePatientAvailability(string survivorId)
        {
            var survivor = _survivors?.Find(survivorId);
            if (survivor == null) return Ashfall.Core.Medical.PatientAvailability.Blocked("patient_unknown");
            if (!survivor.IsAlive) return Ashfall.Core.Medical.PatientAvailability.Blocked("patient_dead");
            return Ashfall.Core.Medical.PatientAvailability.Ok();
        }

        private string GetRadiationPhaseName(string survivorId)
        {
            var phase = _phase0?.RadiationPhase;
            if (phase != null && phase.Survivors.TryGetValue(survivorId, out var state))
                return state.Phase.ToString();
            return "Healthy";
        }

        /// <summary>
        /// Legacy-save migration: the pre-pipeline game displayed these
        /// conditions openly, so restored episodes arrive Confirmed. New
        /// progression starts Unknown/Suspected; this runs once per load.
        /// </summary>
        private void MigrateLegacyMedicalDiagnoses()
        {
            var pipeline = _medical.Pipeline;
            if (pipeline == null) return;
            var respiratoryDef = new Ashfall.Core.Medical.AfflictionId(Ashfall.Core.Medical.MedicalTreatmentCatalog.RespiratoryDegenerationId);
            var radiationDef = new Ashfall.Core.Medical.AfflictionId(Ashfall.Core.Medical.MedicalTreatmentCatalog.RadiationSicknessId);
            for (int i = 0; i < _survivors.RosterState.Count; i++)
            {
                var survivor = _survivors.RosterState[i];
                if (survivor == null) continue;
                if (_phase0.Respiratory.RespiratoryDegradation(survivor.Id) > 0f)
                    pipeline.ConfirmForLegacySave(new Ashfall.Core.Survivors.SurvivorId(survivor.Id), respiratoryDef, _simDay);
                if (!string.Equals(GetRadiationPhaseName(survivor.Id), "Healthy", StringComparison.Ordinal)
                    || (_survivors.RadStateFor(survivor.Id)?.HasAcuteRadiationSickness ?? false))
                    pipeline.ConfirmForLegacySave(new Ashfall.Core.Survivors.SurvivorId(survivor.Id), radiationDef, _simDay);
            }
        }

        private void SaveMedicalPipeline()
        {
            if (_medical?.Pipeline == null) return;
            var save = _medical.CapturePipelineSave();
            if (save == null) return;
            if (CaptureSection("medical_pipeline", MedicalPipelineSaveStore.TryCapturePersisted(save)))
                GD.Print("[Ashfall Godot] Medical pipeline save written.");
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
            // Task #133 P1b: share the pipeline when already bound; otherwise
            // EnsureMedicalPipeline backfills this reference once it runs.
            _medicalWardSession.Pipeline = _medical?.Pipeline;
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

            // Plan 60 / D2 + D6 — the bed is where the clinical note, the authorised
            // treatment, and the vigil belong, so all three are offered from the one
            // surface the player is already looking at.
            SetupDisease();
            SetupMedical();
            _medicalWardPanel.BindDisease(_disease);
            _medicalWardPanel.BindVigil(_medical);
        }

        private void SaveMedicalWard()
        {
            if (_medicalWardSession == null || _medicalWard == null) return;
            try
            {
                _medicalWardSession.SimDay = _simDay;
                var save = new Ashfall.Core.Medical.MedicalWardSave
                {
                    simDay = _medicalWardSession.SimDay,
                    Beds = new List<Ashfall.Core.Medical.MedicalBedSave>(),
                    Procedures = new List<Ashfall.Core.Medical.MedicalProcedureDef>(_medicalWardSession.Procedures),
                    State = _medicalWard.CaptureState()
                };
                foreach (var bed in _medicalWard.Beds)
                {
                    save.Beds.Add(new Ashfall.Core.Medical.MedicalBedSave
                    {
                        BedId = bed.BedId,
                        DisplayName = bed.DisplayName,
                        Category = (int)bed.Category,
                        Isolation = bed.Isolation
                    });
                }

                if (CaptureSection("medical_ward", MedicalWardSaveStore.TryCapturePersisted(save)))
                {
                    _medicalWardDirty = false;
                    _medicalWardSession.ClearDirty();
                }
            }
            catch (Exception e)
            {
                GD.PushWarning("[Ashfall Godot] MedicalWard save failed: " + e.Message);
                CaptureSection("medical_ward", string.Empty);
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
            // Plan 60 / D4 — protocols are armed with the day they are applied, so
            // the authored window counts from the moment the work is done.
            _disease.BindDayProvider(() => _simDay);
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

            // Plan 60 / D3 — treatment has to spend from the one item authority the
            // rest of the game already uses, and a dose has to be recorded where the
            // player can read it back. The ward UI could not otherwise tell a cured
            // patient from one that merely got company.
            _disease.BindSupply((itemId, count) =>
            {
                if (count <= 0 || string.IsNullOrEmpty(itemId)) return false;
                SetupInventory();
                if (_inventory?.Inventory == null) return false;
                bool spent = _inventory.Inventory.TryConsume(itemId, count);
                if (spent) SaveInventory();
                return spent;
            });
            _disease.Engine.OnTreatmentApplied += (survivorId, diseaseId, itemId, role, day) =>
            {
                SetupJournal();
                _journal?.TryAddRawEntry(
                    $"treatment_{survivorId}_{day}_{diseaseId}",
                    $"{survivorId}: {role} treatment with {itemId} for {diseaseId}.",
                    null!, day);
            };

            GD.Print("[Ashfall Godot] Disease Expansion ward ready (contagion · quarantine · outbreak · treatment).");

            // The bed inspector is where a player stands when someone in their ward
            // is dying, so treatment is offered there rather than on a parallel
            // disease screen. Idempotent: BindDisease just re-points and refreshes.
            SetupMedicalWard();
            _medicalWardPanel?.BindDisease(_disease);
        }

        private void CloseMedicalPanel()
        {
            _medicalPanel.Visible = false;
        }

    }
}
