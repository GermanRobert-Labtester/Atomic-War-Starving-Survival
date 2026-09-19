// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Catalogs;
using Ashfall.Core.Memorial;
using Ashfall.Core.Culture;
using Ashfall.Core.Diplomacy;
using Ashfall.Core.Lifecycle;
using Ashfall.Core.Survivors;
using Ashfall.Core.Institutions;
using Ashfall.Core.Sanatorium;
using Ashfall.Core.Shelter;
using Ashfall.Core.SkyDefense;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Flagship institutions (Tasks 5-8) host wiring: composition, catalog
    /// loading, save-section triads (SetupXxx/SaveXxx), daily tick owner and
    /// availability claim re-registration after restore (plan §10 step 11).
    /// Presentation-only rules respected: gameplay lives in Core; this partial
    /// only constructs, binds, ticks, persists.
    /// </summary>
    public partial class Main
    {
        private const int FlagshipMasterSeed = 42;

        private InstitutionAssignmentLedger? _institutionLedger;
        private CulturalArchiveVaultSystem? _culturalArchive;
        private DiplomaticSummitSystem? _diplomaticSummit;
        private SkyDefenseBatterySystem? _skyDefense;
        private PsychologicalSanatoriumSystem? _sanatorium;

        private bool _culturalArchiveDirty;
        private bool _diplomaticSummitDirty;
        private bool _skyDefenseDirty;
        private bool _sanatoriumDirty;

        private static string FlagshipDataDir =>
            System.IO.Path.Combine(
                ProjectSettings.GlobalizePath("res://"), "Assets", "StreamingAssets", "Data");

        private InstitutionAssignmentLedger EnsureInstitutionLedger() =>
            _institutionLedger ??= new InstitutionAssignmentLedger();

        private bool _flagshipLifecycleRegistered;

        /// <summary>
        /// Registers the flagship sessions with the lifecycle registry so a
        /// load-game reset nulls them (fields re-Ensure from disk on the next
        /// Setup call). Idempotent; safe to call from every Ensure.
        /// </summary>
        private void EnsureFlagshipLifecycleRegistration()
        {
            if (_flagshipLifecycleRegistered) return;
            if (_lifecycleRegistry == null) return;
            _flagshipLifecycleRegistered = true;
            _lifecycleRegistry.Register(new DelegateSessionParticipant(
                "flagship_institutions",
                dependsOn: new[] { "inventory" },
                saveSectionKey: null,
                onReset: () =>
                {
                    _institutionLedger = null;
                    _culturalArchive = null;
                    _diplomaticSummit = null;
                    _skyDefense = null;
                    _sanatorium = null;
                    _culturalArchiveDirty = false;
                    _diplomaticSummitDirty = false;
                    _skyDefenseDirty = false;
                    _sanatoriumDirty = false;
                }));
        }

        /// <summary>
        /// The live campaign telemetry (world-owned). Sky defense consumes the
        /// SAME instance the radio/warning UIs consume — never a parallel one
        /// (plan §9.7).
        /// </summary>
        private OrbitalHarrowTelemetrySystem EnsureOrbitalHarrowTelemetry()
        {
            var telemetry = _world?.WeatherIntelligence.Orbital;
            if (telemetry == null)
                telemetry = new OrbitalHarrowTelemetrySystem(new SkyLayerArmorSystem(), new SeededRng(FlagshipMasterSeed));
            return telemetry;
        }

        private void MarkCulturalArchiveDirty() => _culturalArchiveDirty = true;
        private void MarkDiplomaticSummitDirty() => _diplomaticSummitDirty = true;
        private void MarkSkyDefenseDirty() => _skyDefenseDirty = true;
        private void MarkSanatoriumDirty() => _sanatoriumDirty = true;

        // -----------------------------------------------------------------
        // Composition (idempotent; each system constructed exactly once)
        // -----------------------------------------------------------------

        private CulturalArchiveVaultSystem EnsureCulturalArchive()
        {
            EnsureFlagshipLifecycleRegistration();
            if (_culturalArchive != null) return _culturalArchive;
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // No authoritative shelter-humidity authority exists (plan §5.9
            // "if available") — the deep vault reads as a dry environment.
            _culturalArchive = new CulturalArchiveVaultSystem(
                _inventory.Inventory,
                availability: EnsureInstitutionLedger());
            _culturalArchive.LoadTomeCatalog(
                CulturalArchiveTomeCatalogLoader.Load(FlagshipDataDir, fileIO, json));
            _culturalArchive.OnDocumentRestored += _ => MarkCulturalArchiveDirty();
            _culturalArchive.OnMicroficheCreated += _ => MarkCulturalArchiveDirty();
            _culturalArchive.OnTomeTranscribed += _ => MarkCulturalArchiveDirty();
            _culturalArchive.OnDocumentLost += _ => MarkCulturalArchiveDirty();
            _culturalArchive.OnArchiveRecordingCreated += (_, _) => MarkCulturalArchiveDirty();
            _culturalArchive.OnSalonStarted += _ => MarkCulturalArchiveDirty();
            _culturalArchive.OnSalonEnded += _ => MarkCulturalArchiveDirty();
            _culturalArchive.OnChronicleEntryAdded += _ => MarkCulturalArchiveDirty();
            _culturalArchive.OnDocumentationChanged += () => MarkCulturalArchiveDirty();
            BindCultureCrossDomainEvents();

            var saved = CulturalArchiveSaveStore.TryLoad();
            if (saved != null)
                _culturalArchive.RestoreState(saved);
            return _culturalArchive;
        }

        /// <summary>
        /// Plans 178/190 — creation command into the culture vault (DEBT-178-CREATION-TO-VAULT).
        /// The vault owns append + dedup; this only routes a real campaign
        /// milestone into it and surfaces a journal line so the entry is
        /// player-reachable. No second archive save, no art/lore ledger.
        /// </summary>
        public string RecordArchiveChronicle(
            string eventType,
            string summaryKey,
            IReadOnlyList<string>? participants = null,
            string authorId = "")
        {
            var vault = EnsureCulturalArchive();
            if (vault == null) return "The culture vault is unavailable.";

            var result = vault.TryRecordChronicleEntry(eventType, _simDay, summaryKey, participants, authorId);
            if (result.Status == ActionResult.StatusKind.Success)
            {
                _journal?.TryAddRawEntry(
                    $"archive_chronicle:{summaryKey}",
                    $"Entered in the vault chronicle: {summaryKey}.",
                    null!,
                    _simDay);
            }
            return result.Status == ActionResult.StatusKind.Success
                ? "Chronicle entry recorded."
                : $"Chronicle entry not recorded ({result.FailureCode}).";
        }

        /// <summary>
        /// Death is the campaign's most significant archival event: the name
        /// enters the vault chronicle beside the memorial wall projection.
        /// </summary>
        private void OnMemorializedForArchiveChronicle(MemorialEntry entry)
        {
            if (entry == null || string.IsNullOrEmpty(entry.SurvivorId)) return;
            RecordArchiveChronicle(
                Ashfall.Core.Culture.ArchiveChronicleMilestones.Memorial,
                Ashfall.Core.Culture.ArchiveChronicleMilestones.MemorialKey(entry.SurvivorId),
                new[] { entry.SurvivorId });
        }

        private DiplomaticSummitSystem EnsureDiplomaticSummit()
        {
            EnsureFlagshipLifecycleRegistration();
            if (_diplomaticSummit != null) return _diplomaticSummit;
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            _standingPort ??= new HostFactionStandingPort(this);
            _diplomaticSummit = new DiplomaticSummitSystem(
                FlagshipMasterSeed,
                inventory: _inventory.Inventory,
                availability: EnsureInstitutionLedger(),
                standing: _standingPort);
            _diplomaticSummit.LoadTreatyCatalog(
                DiplomaticTreatyCatalogLoader.Load(FlagshipDataDir, fileIO, json));
            _diplomaticSummit.OnSummitScheduled += _ => MarkDiplomaticSummitDirty();
            _diplomaticSummit.OnTreatyRatified += _ => MarkDiplomaticSummitDirty();
            _diplomaticSummit.OnTreatyViolationRecorded += _ => MarkDiplomaticSummitDirty();
            _diplomaticSummit.OnTreatyEnded += (_, _) => MarkDiplomaticSummitDirty();
            _diplomaticSummit.OnGuaranteeExchanged += _ => MarkDiplomaticSummitDirty();
            _diplomaticSummit.OnGuaranteeReleased += _ => MarkDiplomaticSummitDirty();

            var saved = DiplomaticSummitSaveStore.TryLoad();
            if (saved != null)
                _diplomaticSummit.RestoreState(saved);
            return _diplomaticSummit;
        }

        private SkyDefenseBatterySystem EnsureSkyDefense()
        {
            EnsureFlagshipLifecycleRegistration();
            if (_skyDefense != null) return _skyDefense;
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            _skyDefense = new SkyDefenseBatterySystem(
                FlagshipMasterSeed,
                inventory: _inventory.Inventory,
                telemetry: EnsureOrbitalHarrowTelemetry(),
                availability: EnsureInstitutionLedger());
            _skyDefense.LoadOrdnanceCatalog(
                SkyDefenseOrdnanceCatalogLoader.Load(FlagshipDataDir, fileIO, json));
            _skyDefense.OnOrbitalTrackAcquired += _ => MarkSkyDefenseDirty();
            _skyDefense.OnVolleyFired += (_, _, _) => MarkSkyDefenseDirty();
            _skyDefense.OnInterceptResolved += (_, _, _, _) => MarkSkyDefenseDirty();
            _skyDefense.OnServiced += _ => MarkSkyDefenseDirty();
            _skyDefense.OnMaintenanceDue += _ => MarkSkyDefenseDirty();

            var saved = SkyDefenseBatterySaveStore.TryLoad();
            if (saved != null)
                _skyDefense.RestoreState(saved);
            _skyDefense.EnsureDefaultTurret();
            return _skyDefense;
        }

        private PsychologicalSanatoriumSystem EnsureSanatorium()
        {
            EnsureFlagshipLifecycleRegistration();
            if (_sanatorium != null) return _sanatorium;
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            _skillsPort ??= new HostSurvivorSkillsPort(this);
            _conditionPort ??= new HostSurvivorConditionPort(this);
            _sanatorium = new PsychologicalSanatoriumSystem(
                FlagshipMasterSeed,
                inventory: _inventory.Inventory,
                availability: EnsureInstitutionLedger(),
                skills: _skillsPort,
                conditions: _conditionPort);
            _sanatorium.LoadTherapyCatalog(
                PsychologicalTherapyCatalogLoader.Load(FlagshipDataDir, fileIO, json));
            _sanatorium.OnPatientAdmitted += _ => MarkSanatoriumDirty();
            _sanatorium.OnTherapyStarted += (_, _) => MarkSanatoriumDirty();
            _sanatorium.OnTherapyCompleted += (_, _) => MarkSanatoriumDirty();
            _sanatorium.OnPatientRelapsed += (_, _, _) => MarkSanatoriumDirty();
            _sanatorium.OnPatientDischarged += _ => MarkSanatoriumDirty();
            BindSanatoriumCrossDomainEvents();

            var saved = PsychologicalSanatoriumSaveStore.TryLoad();
            if (saved != null)
                _sanatorium.RestoreState(saved);
            return _sanatorium;
        }

        // -----------------------------------------------------------------
        // Setup triad (restore-order chain) + claim re-registration
        // -----------------------------------------------------------------

        private void SetupCulturalArchive()
        {
            var culturalArchive = EnsureCulturalArchive();
            foreach (var doc in culturalArchive.Documents)
                if (!string.IsNullOrEmpty(doc.active_scholar_id))
                    EnsureInstitutionLedger().TryClaim(
                        doc.active_scholar_id, CulturalArchiveVaultSystem.InstitutionId, "scholar");
        }

        private void SetupDiplomaticSummit()
        {
            var diplomaticSummit = EnsureDiplomaticSummit();
            foreach (var g in diplomaticSummit.Guarantees.Where(g => g.status == "exchanged"))
                EnsureInstitutionLedger().TryClaim(
                    g.survivor_id, DiplomaticSummitSystem.InstitutionId, "guarantee");
        }

        private void SetupSkyDefense()
        {
            var skyDefense = EnsureSkyDefense();
            foreach (var turret in skyDefense.Turrets)
                foreach (var crew in turret.assigned_crew_ids)
                    EnsureInstitutionLedger().TryClaim(
                        crew, SkyDefenseBatterySystem.InstitutionId, "gunner");
        }

        private void SetupSanatorium()
        {
            var sanatorium = EnsureSanatorium();
            foreach (var p in sanatorium.Patients.Where(p => p.status == "admitted"))
                EnsureInstitutionLedger().TryClaim(
                    p.survivor_id, PsychologicalSanatoriumSystem.InstitutionId, "patient");
        }

        /// <summary>All four institutions in dependency order (after world/telemetry bases).</summary>
        private void SetupFlagshipInstitutions()
        {
            SetupSkyDefense();
            SetupCulturalArchive();
            SetupDiplomaticSummit();
            SetupSanatorium();
        }

        // -----------------------------------------------------------------
        // Save triad
        // -----------------------------------------------------------------

        private void SaveCulturalArchive()
        {
            if (_culturalArchive == null) return;
            if (!CaptureSection("cultural_archives",
                    CulturalArchiveSaveStore.TryCapturePersisted(_culturalArchive.CaptureState())))
                return;
            _culturalArchiveDirty = false;
        }

        private void SaveDiplomaticSummit()
        {
            if (_diplomaticSummit == null) return;
            if (!CaptureSection("diplomatic_summits",
                    DiplomaticSummitSaveStore.TryCapturePersisted(_diplomaticSummit.CaptureState())))
                return;
            _diplomaticSummitDirty = false;
        }

        private void SaveSkyDefense()
        {
            if (_skyDefense == null) return;
            if (!CaptureSection("sky_defense_battery",
                    SkyDefenseBatterySaveStore.TryCapturePersisted(_skyDefense.CaptureState())))
                return;
            _skyDefenseDirty = false;
        }

        private void SaveSanatorium()
        {
            if (_sanatorium == null) return;
            if (!CaptureSection("psychological_sanatorium",
                    PsychologicalSanatoriumSaveStore.TryCapturePersisted(_sanatorium.CaptureState())))
                return;
            _sanatoriumDirty = false;
        }

        // Composite orchestration only; each institution owns its registered
        // save section through the SaveXxx methods above.
        private void PersistFlagshipInstitutionsIfDirty()
        {
            if (_culturalArchiveDirty) SaveCulturalArchive();
            if (_diplomaticSummitDirty) SaveDiplomaticSummit();
            if (_skyDefenseDirty) SaveSkyDefense();
            if (_sanatoriumDirty) SaveSanatorium();
        }

        // -----------------------------------------------------------------
        // Port adapters (canonical authorities; null-safe before their setups)
        // -----------------------------------------------------------------

        private HostFactionStandingPort? _standingPort;
        private HostSurvivorSkillsPort? _skillsPort;
        private HostSurvivorConditionPort? _conditionPort;

        internal sealed class HostFactionStandingPort : IFactionStandingPort
        {
            private readonly Main _m;
            public HostFactionStandingPort(Main m) => _m = m;
            public float GetStanding(string factionId) =>
                _m._yearOfAsh?.FactionWar.GetStanding(factionId) ?? 0f;
            public void AdjustStanding(string factionId, float delta, string reasonCode) =>
                _m._yearOfAsh?.FactionWar.ModifyStanding(factionId, (int)Math.Round(delta));
        }

        internal sealed class HostSurvivorSkillsPort : ISurvivorSkillsPort
        {
            private readonly Main _m;
            public HostSurvivorSkillsPort(Main m) => _m = m;
            public bool HasSkill(string survivorId, string skillId) =>
                _m.EnsureSharedSkillProgression().HasActiveSkill(survivorId, skillId);
        }

        /// <summary>
        /// Maps authored condition ids onto the canonical Phase-0 trauma
        /// surfaces (plan §9.9): hypervigilance ← CombatTraumaSystem,
        /// flashback ← SomaticFlashbackSystem, guilt-insomnia ←
        /// GuiltInsomniaSystem. Siege paranoia reads as extreme
        /// hypervigilance. Relapse (negative reduction) deliberately does not
        /// re-escalate canonical surfaces — no canonical re-escalation API
        /// exists; the sanatorium's own risk ledger tracks it.
        /// </summary>
        internal sealed class HostSurvivorConditionPort : ISurvivorConditionPort
        {
            private readonly Main _m;
            public HostSurvivorConditionPort(Main m) => _m = m;

            private Phase0HostSession? P0
            {
                get
                {
                    if (_m._phase0 == null) _m.SetupPhase0();
                    return _m._phase0;
                }
            }

            public bool HasCondition(string survivorId, string conditionId)
            {
                // Plan 164: breakdown-arc conditions live in the arc system;
                // the port composes them with the Phase0 trauma conditions.
                if (!string.IsNullOrEmpty(conditionId)
                    && conditionId.StartsWith("arc_", System.StringComparison.Ordinal))
                {
                    return _m._psychologyArcs?.System.HasArc(survivorId, conditionId) == true;
                }
                var p0 = P0;
                if (p0 == null || string.IsNullOrEmpty(survivorId)) return false;
                switch (conditionId)
                {
                    case "condition_combat_ptsd":
                        return p0.CombatTrauma.IsTracked(survivorId)
                            && p0.CombatTrauma.GetHypervigilanceLevel(survivorId) >= 0.25f;
                    case "condition_chronic_hypervigilance":
                        return p0.CombatTrauma.GetHypervigilanceLevel(survivorId) >= 0.4f;
                    case "condition_paranoid_psychosis":
                        return p0.CombatTrauma.GetHypervigilanceLevel(survivorId) >= 0.6f;
                    case "condition_flash_blindness_shock":
                        return p0.Flashbacks.HasActiveFlashback(survivorId)
                            || p0.Flashbacks.GetSusceptibility(survivorId) >= 0.2f;
                    case "condition_severe_survivor_guilt":
                        return p0.Guilt.GetGuiltSourceCount(survivorId) > 0
                            && p0.Guilt.GetInsomniaSeverity(survivorId) >= 0.3f;
                    case "condition_guilt_insomnia_loop":
                        return p0.Guilt.GetInsomniaSeverity(survivorId) >= 0.4f;
                    default:
                        return false;
                }
            }

            public int GetAcuteStressPermille(string survivorId)
            {
                var p0 = P0;
                if (p0 == null) return 0;
                float hv = p0.CombatTrauma.GetHypervigilanceLevel(survivorId);
                float fb = p0.Flashbacks.GetSusceptibility(survivorId);
                float gi = p0.Guilt.GetInsomniaSeverity(survivorId);
                return (int)Math.Clamp(Math.Max(hv, Math.Max(fb, gi)) * 1000f, 0f, 1000f);
            }

            public void ApplyAcuteStressReduction(string survivorId, int permille)
            {
                if (permille <= 0) return;
                float fraction = Math.Clamp(permille / 1000f, 0f, 1f);
                var p0 = P0;
                if (p0 == null) return;
                p0.CombatTrauma.ApplyTherapyRelief(survivorId, fraction);
                p0.Flashbacks.ReduceSusceptibility(survivorId, fraction);
                p0.Guilt.ApplyTherapyRelief(survivorId, fraction);
            }

            public void ApplyRecoveryProgress(string survivorId, int progress)
            {
                // Recovery progress is tracked inside the sanatorium's patient
                // state; the canonical surface is relieved through
                // ApplyAcuteStressReduction at outcome time. Plan 164: active
                // breakdown arcs additionally advance their arc recovery here.
                _m._psychologyArcs?.System.ApplyTreatmentProgress(survivorId, progress);
            }

            public void SuppressReversibleCondition(string survivorId, string conditionId)
            {
                var p0 = P0;
                if (p0 == null) return;
                switch (conditionId)
                {
                    case "condition_flash_blindness_shock":
                        p0.Flashbacks.ReduceSusceptibility(survivorId, 1f);
                        break;
                    case "condition_chronic_hypervigilance":
                    case "condition_combat_ptsd":
                        p0.CombatTrauma.ApplyTherapyRelief(survivorId, 1f);
                        break;
                    case "condition_guilt_insomnia_loop":
                    case "condition_severe_survivor_guilt":
                        p0.Guilt.ApplyTherapyRelief(survivorId, 1f);
                        break;
                }
            }

            public int GetRelationshipTrust(string therapistId, string patientId) => 50;
        }

        // -----------------------------------------------------------------
        // Cross-domain event bindings (bound once inside the Ensure methods)
        // -----------------------------------------------------------------

        private void BindCultureCrossDomainEvents()
        {
            // Salon morale → canonical needs authority (single consumer).
            _culturalArchive!.OnSalonMoraleTick += delta =>
            {
                if (_survivors == null) return;
                foreach (var sv in _survivors.RosterState)
                {
                    if (sv.Health <= 0f) continue;
                    _survivors.Needs.Modify(sv, NeedKind.Morale, (float)delta);
                }
            };

            // Archive disc cut → vinyl media catalog (single consumer; media
            // playback morale stays owned by VinylMoraleSystem).
            _culturalArchive!.OnArchiveRecordingCreated += (_, def) =>
            {
                if (_vinylMorale == null) SetupVinylMorale();
                var system = _vinylMorale?.System;
                if (system == null) return;
                system.MergeRecord(new VinylRecordDefinition
                {
                    record_id = def.record_id,
                    display_name = def.display_name,
                    genre = def.genre,
                    morale_daily_bonus = def.morale_daily_bonus,
                    flashback_suppression = def.flashback_suppression,
                    audio_cue_id = def.audio_cue_id,
                    description = def.description,
                });
                system.AcquireRecord(def.record_id);
            };
        }

        private void BindSanatoriumCrossDomainEvents()
        {
            // Dream transcription / testimony (§9.2): one completion → one
            // archive oral-history disc; the culture system's duplicate guard
            // makes this idempotent across re-fires.
            _sanatorium!.OnTherapeuticJournalCompleted += (survivorId, _) =>
            {
                int day = _core?.Clock.Day ?? 0;
                _culturalArchive?.TryCutArchiveDisc(
                    $"archive_disc_dream_{survivorId}", "oral_history", survivorId, day);
            };
        }

        // -----------------------------------------------------------------
        // Daily tick owner (phase 5 — after survivors/medical, before final)
        // -----------------------------------------------------------------

        private sealed class FlagshipInstitutionsDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public FlagshipInstitutionsDayOwner(Main m) => _m = m;

            public void CapturePreDaySnapshot(int day) { }

            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _m.SetupSkyDefense();
                _m.SetupCulturalArchive();
                _m.SetupDiplomaticSummit();
                _m.SetupSanatorium();

                _m._skyDefense!.TickDay(day);
                _m._culturalArchive!.TickDay(day);
                _m._diplomaticSummit!.TickDay(day);
                _m._sanatorium!.TickDay(day);

                _m.PersistFlagshipInstitutionsIfDirty();
                events.Add(new DayStateChangeEvent("flagship_institutions_ticked",
                    "flagship_institutions", null, null, day));
            }
        }

        /// <summary>Called from RegisterProductionCampaignOwners (phase 5).</summary>
        private void RegisterFlagshipInstitutionsOwner()
        {
            if (_campaignDay == null) return;
            _campaignDay.Register("flagship_institutions", new FlagshipInstitutionsDayOwner(this), phase: 5);
        }
    }
}
