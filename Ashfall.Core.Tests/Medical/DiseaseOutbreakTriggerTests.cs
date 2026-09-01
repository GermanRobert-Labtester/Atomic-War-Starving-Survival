// SPDX-License-Identifier: MIT
// Plan 09 / 9A follow-up — IDiseaseOutbreakSource port +
// DiseaseSystem.TriggerOutbreak(...) deterministic entry. Until this
// commit, diseases landed only via Infect(...). World events (flood
// receded, deep dig completed) could describe the disease in their
// source_note but had no Core entry point. This file pins the new
// contract: contract-checked by disease id, seeded by RNG, idempotent
// on null/empty candidate pools, audit-event surface preserved
// through OnOutbreakTriggered.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class DiseaseOutbreakTriggerTests
    {
        // // ── Test source adapters (Core-side, no host deps) ──────────

        private sealed class FloodSource : IDiseaseOutbreakSource
        {
            public string SourceId => "sump_flooding_test";
            public IReadOnlyList<string> AuthoredDiseaseIds { get; }
                = new[] { "disease_silt_jaundice" };
        }

        private sealed class DigSource : IDiseaseOutbreakSource
        {
            public string SourceId => "excavation_test";
            public IReadOnlyList<string> AuthoredDiseaseIds { get; }
                = new[] { "disease_deep_excavation_mold_lung" };
        }

        private sealed class BogusSource : IDiseaseOutbreakSource
        {
            public string SourceId => "unauthorized_test";
            public IReadOnlyList<string> AuthoredDiseaseIds { get; } = Array.Empty<string>();
        }

        private static string LocateDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException(
                "Assets/StreamingAssets/Data not found from " + start);
        }

        private static DiseaseSystem NewSystemWithFullCatalog()
        {
            var sys = new DiseaseSystem(rng: new SeededRng(1013));
            var catalog = DiseaseCatalogLoader.Load(
                LocateDataDir(),
                new FileSystemIO(),
                new SystemTextJsonSerializer());
            sys.BindCatalog(catalog);
            return sys;
        }

        // // ── Happy paths ────────────────────────────────────────────────

        [Fact]
        public void FloodSource_TriggerOutbreak_SeedsDiseaseOnFirstCandidate()
        {
            var sys = NewSystemWithFullCatalog();
            var src = new FloodSource();
            var capture = new InfectionCapture();
            sys.OnOutbreakTriggered += capture.OnTrigger;
            var pool = new[] { "survivor_a", "survivor_b", "survivor_c" };

            var result = sys.TriggerOutbreak(src, "disease_silt_jaundice", 100, pool);

            Assert.Equal(1, result.InfectionsApplied);
            Assert.Equal(0, result.RejectedByContract);
            Assert.Equal(0, result.UnknownDisease);
            Assert.Equal("disease_silt_jaundice", capture.LastDiseaseId);
            Assert.Equal("applied", capture.LastReason);
            // The system snapshot reveals the actual picked survivor.
            var picked = sys.GetSnapshot().patients
                .Where(p => p.disease_id == "disease_silt_jaundice")
                .Select(p => p.survivor_id)
                .ToList();
            Assert.Single(picked);
            Assert.Contains(picked[0], pool);
        }

        [Fact]
        public void DigSource_TriggerOutbreak_SeedsMoldLung()
        {
            // Matches Plan 09 9A data responsibility: a deep dig completes →
            // the stratosphere lane opens → deep_excavation_mold_lung seeds
            // a survivor from the assigned diggers (here: a single survivor).
            var sys = NewSystemWithFullCatalog();
            var src = new DigSource();
            var capture = new InfectionCapture();
            sys.OnOutbreakTriggered += capture.OnTrigger;
            var pool = new[] { "survivor_digger" };

            var result = sys.TriggerOutbreak(src, "disease_deep_excavation_mold_lung", 250, pool);

            Assert.Equal(1, result.InfectionsApplied);
            var picked = sys.GetSnapshot().patients
                .Where(p => p.disease_id == "disease_deep_excavation_mold_lung")
                .Select(p => p.survivor_id)
                .ToList();
            Assert.Single(picked);
            Assert.Equal("survivor_digger", picked[0]);
        }

        // // ── Contract enforcement ──────────────────────────────────────

        [Fact]
        public void FloodSource_CannotSeedSporeDisease_RejectedByContract()
        {
            // A flood source is contracted to silt_jaundice only. Even if
            // the catalog knows disease_spore_blight, a flood event cannot
            // seed it — this is the entire point of the contract.
            var sys = NewSystemWithFullCatalog();
            var src = new FloodSource();

            var result = sys.TriggerOutbreak(src, "disease_spore_blight", 50,
                new[] { "survivor_a" });

            Assert.Equal(0, result.InfectionsApplied);
            Assert.Equal(1, result.RejectedByContract);
            Assert.Equal(0, result.UnknownDisease);
            Assert.False(sys.IsInfected("survivor_a", "disease_spore_blight"));
        }

        [Fact]
        public void BogusSource_WithEmptyContract_AlwaysRejects()
        {
            var sys = NewSystemWithFullCatalog();
            var src = new BogusSource();

            var result = sys.TriggerOutbreak(src, "disease_cholera", 50,
                new[] { "survivor_a", "survivor_b" });

            Assert.Equal(0, result.InfectionsApplied);
            Assert.Equal(1, result.RejectedByContract);
        }

        [Fact]
        public void TriggerOutbreak_RejectsUnknownDiseaseId()
        {
            // Source is contracted for the id but the catalog doesn't
            // know it — describe as "unknown disease" not "rejected by
            // contract" so the host can distinguish "wrong author" from
            // "drift in data".
            var sys = NewSystemWithFullCatalog();
            var src = new FloodSource();

            var result = sys.TriggerOutbreak(src, "disease_does_not_exist", 50,
                new[] { "survivor_a" });

            Assert.Equal(0, result.InfectionsApplied);
            Assert.Equal(0, result.RejectedByContract);
            Assert.Equal(1, result.UnknownDisease);
        }

        // // ── Empty candidate pool ──────────────────────────────────────

        [Fact]
        public void EmptyCandidatePool_ProducesNoCandidates_BumpsNoState()
        {
            var sys = NewSystemWithFullCatalog();
            var src = new FloodSource();

            var resultEmptyList = sys.TriggerOutbreak(src, "disease_silt_jaundice", 50,
                Array.Empty<string>());
            var resultNull = sys.TriggerOutbreak(src, "disease_silt_jaundice", 50, null);

            Assert.Equal(0, resultEmptyList.InfectionsApplied);
            Assert.Equal(1, resultEmptyList.NoCandidates);
            Assert.Equal(0, resultNull.InfectionsApplied);
            Assert.Equal(1, resultNull.NoCandidates);
        }

        // // ── Audit event surface ──────────────────────────────────────

        [Fact]
        public void Trigger_AlwaysRaisesOnOutbreakTriggered_RegardlessOfResult()
        {
            var sys = NewSystemWithFullCatalog();
            int auditCount = 0;
            int appliedCount = 0, rejectedCount = 0, noCandidatesCount = 0, unknownCount = 0;
            sys.OnOutbreakTriggered += (diseaseId, sourceId, reason, applied) =>
            {
                auditCount++;
                if (reason == "applied") appliedCount++;
                else if (reason == "rejected_by_contract") rejectedCount++;
                else if (reason == "no_candidates") noCandidatesCount++;
                else if (reason == "unknown_disease") unknownCount++;
            };

            // Three events: applied, rejected by contract, no candidates.
            sys.TriggerOutbreak(new FloodSource(), "disease_silt_jaundice", 50,
                new[] { "survivor_a" });
            sys.TriggerOutbreak(new BogusSource(), "disease_cholera", 50,
                new[] { "survivor_a" });
            sys.TriggerOutbreak(new FloodSource(), "disease_silt_jaundice", 50,
                Array.Empty<string>());

            Assert.Equal(3, auditCount);
            Assert.Equal(1, appliedCount);
            Assert.Equal(1, rejectedCount);
            Assert.Equal(1, noCandidatesCount);
            Assert.Equal(0, unknownCount);
        }

        // // ── Determinism (SaveChecksum-friendly) ───────────────────────

        [Fact]
        public void TriggerOutbreak_SameSeedSamePool_PicksSameSurvivor()
        {
            var sys1 = NewSystemWithFullCatalog();
            var sys2 = NewSystemWithFullCatalog();
            var src = new FloodSource();
            var pool = new[] { "alpha", "bravo", "charlie", "delta", "echo" };

            sys1.TriggerOutbreak(src, "disease_silt_jaundice", 100, pool);
            sys2.TriggerOutbreak(src, "disease_silt_jaundice", 100, pool);

            // Both systems get the same random pick — the seed is shared
            // and the candidate list is order-stable. This is the
            // SaveChecksum gate: pre- and post- save DeterminismReplay must
            // produce the same outcome.
            var infected1 = sys1.GetSnapshot().patients
                .Where(p => p.disease_id == "disease_silt_jaundice")
                .Select(p => p.survivor_id).ToList();
            var infected2 = sys2.GetSnapshot().patients
                .Where(p => p.disease_id == "disease_silt_jaundice")
                .Select(p => p.survivor_id).ToList();
            Assert.Equal(infected1, infected2);
            Assert.Single(infected1);
        }

        // // ── One-shot event integration ───────────────────────────────

        [Fact]
        public void Trigger_DoesNotQueueForLater_WhenNoCandidates()
        {
            // When the flood recedes during a roster gap, the trigger
            // should not silently queue the outbreak for some future day.
            // The contract is one-shot: re-fires from the host when the
            // roster returns.
            var sys = NewSystemWithFullCatalog();
            var src = new FloodSource();
            sys.TriggerOutbreak(src, "disease_silt_jaundice", 50, Array.Empty<string>());
            // Now a survivor shows up. Start a fresh tick daily — the
            // disease entry has no survival queue to drain.
            for (int day = 51; day <= 60; day++)
                sys.TickDaily(day, new[] { "survivor_a" });
            Assert.False(sys.IsInfected("survivor_a", "disease_silt_jaundice"));
        }

        // // ── Helper ────────────────────────────────────────────────────

        private sealed class InfectionCapture
        {
            public string LastDiseaseId = string.Empty;
            public string LastReason = string.Empty;
            public int LastApplied = -1;
            public void OnTrigger(string diseaseId, string sourceId, string reason,
                int infectionsApplied)
            {
                LastDiseaseId = diseaseId;
                LastReason = reason;
                LastApplied = infectionsApplied;
            }
        }
    }
}
