using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Culture;
using Ashfall.Core.Diplomacy;
using Ashfall.Core.Institutions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Sanatorium;
using Ashfall.Core.Shelter;
using Ashfall.Core.SkyDefense;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship plan §13 — deterministic multi-system replay harness.
    /// Seed 42. Run A: 30 campaign days uninterrupted across all four
    /// institutions. Run B: 15 days, capture, fresh composition, restore,
    /// 15 more days. Authoritative state and future deterministic outcomes
    /// must match exactly (plan §2.6 continuation equivalence).
    /// </summary>
    public class LateGameInstitutionReplayTests
    {
        private static string DataDir
        {
            get
            {
                if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out string found))
                    return found;
                throw new InvalidOperationException("data dir not found");
            }
        }

        private sealed class FakeConditions : ISurvivorConditionPort
        {
            public readonly HashSet<string> Sick = new(StringComparer.Ordinal) { "survivor_patient_e" };
            public readonly Dictionary<string, int> Acute = new(StringComparer.Ordinal) { ["survivor_patient_e"] = 800 };
            public readonly List<string> Suppressed = new();
            public bool HasCondition(string survivorId, string conditionId) => Sick.Contains(survivorId);
            public int GetAcuteStressPermille(string survivorId) => Acute.GetValueOrDefault(survivorId, 0);
            public void ApplyAcuteStressReduction(string survivorId, int permille) =>
                Acute[survivorId] = Math.Clamp(Acute.GetValueOrDefault(survivorId) - permille, 0, 1000);
            public void ApplyRecoveryProgress(string survivorId, int progress) { }
            public void SuppressReversibleCondition(string survivorId, string conditionId) =>
                Suppressed.Add($"{survivorId}/{conditionId}");
            public int GetRelationshipTrust(string therapistId, string patientId) => 50;
        }

        private sealed class StaticSkills : ISurvivorSkillsPort
        {
            public bool HasSkill(string survivorId, string skillId) =>
                survivorId == "survivor_therapist" && skillId == "skill_watchful";
        }

        private sealed class StaticFactions : IFactionContextPort
        {
            public string? GetFactionTag(string factionId) => factionId is "faction_a" or "faction_b" ? "militia" : null;
            public bool IsHostile(string factionId) => false;
        }

        private sealed class RecordingStanding : IFactionStandingPort
        {
            public readonly Dictionary<string, float> Standing = new(StringComparer.Ordinal);
            public float GetStanding(string factionId) => Standing.GetValueOrDefault(factionId, 50f);
            public void AdjustStanding(string factionId, float delta, string reasonCode) =>
                Standing[factionId] = GetStanding(factionId) + delta;
        }

        private sealed class Campaign
        {
            public InstitutionAssignmentLedger Ledger = new();
            public Inventory.Inventory Inventory = new();
            public FakeConditions Conditions = new();
            public SkyLayerArmorSystem Armor = new();
            public OrbitalHarrowTelemetrySystem Telemetry;
            public CulturalArchiveVaultSystem Culture;
            public DiplomaticSummitSystem Diplomacy;
            public RecordingStanding Standing = new();
            public SkyDefenseBatterySystem Defense;
            public PsychologicalSanatoriumSystem Sanatorium;
            public readonly List<string> EventLog = new();

            public Campaign(int seed = 42)
            {
                Telemetry = new OrbitalHarrowTelemetrySystem(Armor, new SeededRng(seed));
                Culture = new CulturalArchiveVaultSystem(Inventory, availability: Ledger);
                Diplomacy = new DiplomaticSummitSystem(seed, inventory: Inventory,
                    availability: Ledger, standing: Standing, factions: new StaticFactions());
                Defense = new SkyDefenseBatterySystem(seed, inventory: Inventory,
                    telemetry: Telemetry, availability: Ledger, skills: new StaticSkills());
                Sanatorium = new PsychologicalSanatoriumSystem(seed, inventory: Inventory,
                    availability: Ledger, skills: new StaticSkills(), conditions: Conditions);

                var json = new SystemTextJsonSerializer();
                var files = new FileSystemIO();
                Culture.LoadTomeCatalog(CulturalArchiveTomeCatalogLoader.Load(DataDir, files, json));
                Diplomacy.LoadTreatyCatalog(DiplomaticTreatyCatalogLoader.Load(DataDir, files, json));
                Defense.LoadOrdnanceCatalog(SkyDefenseOrdnanceCatalogLoader.Load(DataDir, files, json));
                Sanatorium.LoadTherapyCatalog(PsychologicalTherapyCatalogLoader.Load(DataDir, files, json));
                Defense.EnsureDefaultTurret();

                // event ownership: one consumer per event, all logged for comparison
                Culture.OnDocumentRestored += id => EventLog.Add($"restored:{id}");
                Culture.OnMicroficheCreated += id => EventLog.Add($"microfiche:{id}");
                Culture.OnTomeTranscribed += id => EventLog.Add($"transcribed:{id}");
                Culture.OnDocumentLost += id => EventLog.Add($"lost:{id}");
                Culture.OnSalonStarted += d => EventLog.Add($"salon_start:{d}");
                Culture.OnSalonEnded += d => EventLog.Add($"salon_end:{d}");
                Culture.OnChronicleEntryAdded += e => EventLog.Add($"chronicle:{e.chronicle_id}");
                Diplomacy.OnSummitScheduled += s => EventLog.Add($"summit:{s.summit_id}");
                Diplomacy.OnTreatyRatified += t => EventLog.Add($"ratified:{t.treaty_id}");
                Diplomacy.OnTreatyViolationRecorded += v => EventLog.Add($"violation:{v.violation_id}");
                Diplomacy.OnTreatyEnded += (t, r) => EventLog.Add($"treaty_end:{t.treaty_id}:{r}");
                Diplomacy.OnGuaranteeExchanged += g => EventLog.Add($"guarantee:{g.guarantee_id}");
                Diplomacy.OnGuaranteeReleased += g => EventLog.Add($"guarantee_release:{g.guarantee_id}");
                Defense.OnOrbitalTrackAcquired += t => EventLog.Add($"track:{t.track_id}");
                Defense.OnVolleyFired += (t, a, m) => EventLog.Add($"volley:{t}:{m}");
                Defense.OnInterceptResolved += (t, a, ok, r) => EventLog.Add($"intercept:{t}:{ok}");
                Defense.OnServiced += t => EventLog.Add($"serviced:{t}");
                Sanatorium.OnPatientAdmitted += p => EventLog.Add($"admitted:{p.survivor_id}");
                Sanatorium.OnTherapyStarted += (p, t) => EventLog.Add($"therapy:{p.survivor_id}:{t}");
                Sanatorium.OnTherapyCompleted += (p, t) => EventLog.Add($"therapy_done:{p.survivor_id}:{t}");
                Sanatorium.OnPatientRelapsed += (s, c, d) => EventLog.Add($"relapse:{s}:{d}");
                Sanatorium.OnPatientDischarged += p => EventLog.Add($"discharged:{p.survivor_id}");

                foreach (var id in new[]
                         {
                             "scrap_chemical", "clean_water", "paper_stock", "microfiche_film",
                             "acetate_blank_disc", "machine_oil", "bandage", "sedative_draught",
                             "item_preservation_salt", "fuel", "battery",
                         })
                    Inventory.TryProduce(id, 12);
                Inventory.TryProduce("ammo_76mm_he_flak", 12);
            }

            /// <summary>The scripted 30-day campaign every run executes.</summary>
            public void RunDays(int firstDay, int lastDay)
            {
                for (int day = firstDay; day <= lastDay; day++)
                {
                    if (day == 1)
                    {
                        Culture.TryStartTranscription("tome_children_primers", "survivor_scholar_a");
                        Culture.TryStartSalon(1);
                        Culture.TryRestoreDocument("tome_mechanics_handbook_1974");
                        Sanatorium.TryAdmitPatient("survivor_patient_e", "condition_combat_ptsd", 1);
                        Sanatorium.TryStartTherapy("survivor_patient_e", "therapy_trauma_desensitization",
                            "survivor_therapist", 1);
                    }
                    if (day == 5)
                    {
                        Sanatorium.TryAdministerSedative("survivor_patient_e", 5);
                        Culture.TryCreateMicroficheCopy("tome_stoic_meditations", "survivor_archivist");
                    }
                    if (day == 8)
                    {
                        Defense.TryLoadMagazine(SkyDefenseBatterySystem.DefaultTurretId, "ammo_76mm_he_flak");
                        Telemetry.ScheduleImpact(day + 4, gridX: 3, energyMj: 10f); // warning → track
                    }
                    if (day is >= 9 and <= 11)
                    {
                        Defense.TryFireVolley(SkyDefenseBatterySystem.DefaultTurretId, "custom_impact");
                    }
                    if (day == 12 && Defense.GetTurret(SkyDefenseBatterySystem.DefaultTurretId)!.volleys_since_service > 0)
                    {
                        Defense.TryServiceHydraulics(SkyDefenseBatterySystem.DefaultTurretId);
                    }
                    if (day == 15)
                    {
                        Diplomacy.TryScheduleSummit(DiplomaticSummitSystem.NeutralSummitSiteId,
                            new[] { "faction_a", "faction_b" }, new[] { "survivor_envoy" },
                            "treaty_non_aggression_compact", 15);
                    }
                    if (day >= 16 && day <= 24)
                    {
                        var summit = Diplomacy.Summits.FirstOrDefault(s => s.status == "negotiating");
                        if (summit != null)
                        {
                            Diplomacy.AdvanceNegotiation(summit.summit_id, offerConcession: day % 2 == 0);
                            if (summit.negotiation_stability >= DiplomaticSummitSystem.RatificationThreshold)
                                Diplomacy.TryRatifyTreaty(summit.summit_id, day);
                        }
                    }
                    if (day == 25)
                    {
                        var treaty = Diplomacy.Treaties.FirstOrDefault(t => t.status == "active");
                        if (treaty != null)
                            Diplomacy.TryExchangeGuarantee(treaty.treaty_id, "survivor_envoy", "faction_a", 25);
                    }
                    if (day == 27)
                    {
                        Diplomacy.ReportArmedPatrol("faction_a", "high_scarp_ridgeline", 27);
                    }
                    if (day == 20)
                    {
                        Culture.TryRecordChronicleEntry("orbital_strike_weathered", 20,
                            "chronicle.strike_weathered", new[] { "survivor_clerk" });
                    }

                    // daily tick fan-out (host order: foundations before institutions)
                    Culture.TickDay(day);
                    Diplomacy.TickDay(day);
                    Defense.TickDay(day);
                    Sanatorium.TickDay(day);
                    Telemetry.TickDay(day);
                }
            }
        }

        private static string Fingerprint(Campaign c)
        {
            var lines = new List<string>();

            foreach (var d in c.Culture.Documents)
                lines.Add($"doc:{d.document_id}:{d.physical_degradation_permille}:{d.transcription_permille}:{d.status}:{d.knowledge_preserved}");
            foreach (var r in c.Culture.Recordings)
                lines.Add($"rec:{r.recording_id}");
            foreach (var e in c.Culture.Chronicle)
                lines.Add($"chr:{e.chronicle_id}");
            lines.Add($"salon:{c.Culture.Salon.active}:{c.Culture.Salon.cooldown_until_day}");

            foreach (var t in c.Diplomacy.Treaties)
                lines.Add($"treaty:{t.treaty_id}:{t.status}:{t.stability}:{t.violation_count}:{t.expiry_day}");
            foreach (var g in c.Diplomacy.Guarantees)
                lines.Add($"guarantee:{g.guarantee_id}:{g.status}:{g.release_day}");
            foreach (var v in c.Diplomacy.Violations)
                lines.Add($"viol:{v.violation_id}:{v.kind}:{v.day}");
            lines.Add("standing:" + string.Join(",",
                c.Standing.Standing.OrderBy(kv => kv.Key, StringComparer.Ordinal).Select(kv => $"{kv.Key}={kv.Value}")));

            foreach (var t in c.Defense.Turrets)
                lines.Add($"turret:{t.turret_id}:{t.barrel_heat}:{t.magazine_count}:{t.radar_calibration}:{t.hydraulic_condition}:{t.volleys_since_service}");
            lines.Add($"interceptions:{c.Defense.TotalInterceptions}:volleys:{c.Defense.TotalVolleys}");
            foreach (var t in c.Defense.Tracks)
                lines.Add($"track:{t.track_id}:{t.volleys_fired}");

            foreach (var p in c.Sanatorium.Patients)
                lines.Add($"patient:{p.survivor_id}:{p.status}:{p.treatment_progress}:{p.completed_therapy_count}:{p.relapse_risk_permille}:{p.discharge_day}");
            lines.Add($"acute:{c.Conditions.Acute["survivor_patient_e"]}");
            lines.Add("suppressed:" + string.Join(",", c.Conditions.Suppressed));

            lines.Add("inv:" + string.Join(",",
                c.Inventory.Slots
                    .Where(s => s.Item != null && s.Amount > 0)
                    .GroupBy(s => s.Item.id, StringComparer.Ordinal)
                    .OrderBy(g => g.Key, StringComparer.Ordinal)
                    .Select(g => $"{g.Key}={g.Sum(x => x.Amount)}")));
            lines.Add("claims:" + string.Join(",",
                c.Ledger.Claims.OrderBy(kv => kv.Key, StringComparer.Ordinal).Select(kv => $"{kv.Key}={kv.Value}")));

            lines.AddRange(c.EventLog);
            return string.Join("\n", lines);
        }

        [Fact]
        public void ThirtyDayCampaign_RestoredContinuation_MatchesUninterrupted()
        {
            // Run A — uninterrupted 30 days.
            var runA = new Campaign();
            runA.RunDays(1, 30);

            // Run B — 15 days, save, fresh composition, restore, 15 more days.
            var runB = new Campaign();
            runB.RunDays(1, 15);

            var cultureSave = runB.Culture.CaptureState();
            var diplomacySave = runB.Diplomacy.CaptureState();
            var defenseSave = runB.Defense.CaptureState();
            var sanatoriumSave = runB.Sanatorium.CaptureState();
            int invWaterBefore = runB.Inventory.CountById("clean_water");

            var fresh = new Campaign();
            fresh.Culture.RestoreState(cultureSave);
            fresh.Diplomacy.RestoreState(diplomacySave);
            fresh.Defense.RestoreState(defenseSave);
            fresh.Sanatorium.RestoreState(sanatoriumSave);
            fresh.RunDays(16, 30);

            runA.RunDays(16, 30);

            // NOTE: the fresh campaign's inventory is NOT restored here (the
            // inventory section belongs to the global inventory save, outside
            // this harness); run A and fresh both execute the same script over
            // full stock, so authoritative institution state must still match.
            Assert.Equal(Fingerprint(runA), Fingerprint(fresh));
        }

        [Fact]
        public void SameSeed_CampaignsProduceIdenticalTraces()
        {
            var a = new Campaign();
            a.RunDays(1, 30);
            var b = new Campaign();
            b.RunDays(1, 30);
            Assert.Equal(Fingerprint(a), Fingerprint(b));
        }
    }
}
