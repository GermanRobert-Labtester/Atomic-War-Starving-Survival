using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Small, explicit first-30-day heuristic. This is a balance evidence
    /// harness, not a second survival authority: it consumes authored
    /// profiles and reports relative pressure only.
    /// </summary>
    public sealed class StartingCohortBalanceSimulationTests
    {
        private readonly string _dataDir = ResolveDataDir();

        [Fact]
        public void FirstThirtyDays_AreDeterministicForEveryProfile()
        {
            var catalog = LoadCatalog();
            var canonical = LoadCanonical();

            foreach (var profile in catalog.Profiles)
            {
                var first = Simulate(profile, canonical, seed: 138);
                var second = Simulate(profile, canonical, seed: 138);

                Assert.Equal(first.Fingerprint, second.Fingerprint);
                Assert.InRange(first.MinimumHealthAtDay30, 0f, 100f);
                Assert.InRange(first.MinimumWarmthAtDay30, 0f, 100f);
                Assert.InRange(first.TotalInitialDose, 0f, float.MaxValue);
                Assert.True(first.FirstCriticalHungerDay is null or > 0);
                Assert.True(first.FirstCriticalThirstDay is null or > 0);
                Assert.True(first.MinimumHealthAtDay30 > 0f,
                    $"{profile.profile_id} enters an unrecoverable state before day 30.");
            }
        }

        [Fact]
        public void FirstThirtyDays_StandardBaselineAndSpecializationsRemainBounded()
        {
            var catalog = LoadCatalog();
            var canonical = LoadCanonical();
            var snapshots = catalog.Profiles
                .ToDictionary(
                    profile => profile.profile_id,
                    profile => Simulate(profile, canonical, seed: 138),
                    StringComparer.Ordinal);

            var standard = snapshots[StartingCohortCatalog.StandardProfileId];
            Assert.Equal(2, standard.MedicalCoverage);
            Assert.Equal(0, standard.FieldCoverage);
            Assert.Equal(0, standard.FoodCoverage);
            Assert.Equal(0, standard.RepairCoverage);

            Assert.Equal(2, snapshots["cohort_triage_ward"].MedicalCoverage);
            Assert.Equal(1, snapshots["cohort_triage_ward"].RepairCoverage);
            Assert.Equal(3, snapshots["cohort_repair_crew"].RepairCoverage);
            Assert.Equal(3, snapshots["cohort_growers_stewards"].FoodCoverage);
            Assert.Equal(3, snapshots["cohort_convoy_remnant"].FieldCoverage);
            Assert.Equal(2, snapshots["cohort_civilian_improvisers"].SocialCoverage);

            foreach (var pair in snapshots)
            {
                if (pair.Key == StartingCohortCatalog.StandardProfileId) continue;
                var alternate = pair.Value;

                // No profile is strictly better than Standard on every
                // pressure/coverage dimension in this conservative scenario.
                bool dominatesStandard =
                    alternate.MinimumHealthAtDay30 >= standard.MinimumHealthAtDay30 &&
                    alternate.MinimumWarmthAtDay30 >= standard.MinimumWarmthAtDay30 &&
                    (alternate.FirstCriticalHungerDay ?? 0) >=
                        (standard.FirstCriticalHungerDay ?? 0) &&
                    (alternate.FirstCriticalThirstDay ?? 0) >=
                        (standard.FirstCriticalThirstDay ?? 0) &&
                    alternate.MedicalCoverage >= standard.MedicalCoverage &&
                    alternate.RepairCoverage >= standard.RepairCoverage &&
                    alternate.FoodCoverage >= standard.FoodCoverage &&
                    alternate.FieldCoverage >= standard.FieldCoverage &&
                    alternate.SocialCoverage >= standard.SocialCoverage;
                Assert.False(dominatesStandard, $"{pair.Key} dominates Standard Holdfast.");
            }
        }

        private StartingCohortCatalog LoadCatalog()
        {
            var result = StartingCohortCatalogLoader.LoadDetailed(
                _dataDir,
                new FileSystemIO(),
                new SystemTextJsonSerializer(),
                LoadCanonical());
            Assert.True(result.IsSuccess, string.Join(Environment.NewLine, result.Errors));
            return result.Catalog;
        }

        private List<SurvivorDefinition> LoadCanonical() =>
            SurvivorCatalogLoader.Load(
                _dataDir,
                new FileSystemIO(),
                new SystemTextJsonSerializer());

        private static SimulationSnapshot Simulate(
            StartingCohortProfile profile,
            IReadOnlyList<SurvivorDefinition> canonical,
            int seed)
        {
            // The fixed seed samples only a tiny scenario stress factor. It
            // keeps repeated runs honest without mutating campaign RNG.
            var rng = new SeededRng(seed);
            float stressFactor = 0.98f + rng.NextFloat() * 0.04f;
            var canonicalById = canonical.ToDictionary(
                definition => definition.id,
                definition => definition,
                StringComparer.Ordinal);

            float minimumHealth = 100f;
            float minimumWarmth = 100f;
            int? firstHungerCritical = null;
            int? firstThirstCritical = null;
            float totalDose = 0f;
            int medical = 0;
            int repair = 0;
            int food = 0;
            int field = 0;
            int social = 0;
            var trajectory = new List<string>();

            foreach (var member in profile.members)
            {
                var profession = canonicalById.TryGetValue(member.id, out var definition)
                    ? definition.profession.ToLowerInvariant()
                    : string.Empty;
                if (ContainsAny(profession, "medic", "nurse", "paramedic", "surgeon", "doctor"))
                    medical++;
                if (ContainsAny(profession, "engineer", "mechanic", "builder", "carpenter", "technician"))
                    repair++;
                if (ContainsAny(profession, "farmer", "botanist", "cook", "grower"))
                    food++;
                if (ContainsAny(profession, "scout", "scavenger", "guard", "soldier", "gunner", "convoy"))
                    field++;
                if (ContainsAny(profession, "teacher", "storyteller", "therapist", "leader"))
                    social++;
                totalDose += member.lifetimeDose;
            }

            for (int day = 1; day <= 30; day++)
            {
                foreach (var member in profile.members)
                {
                    float hunger = member.hunger + day * 2.5f * stressFactor;
                    float thirst = member.thirst + day * 3.0f * stressFactor;
                    float warmth = Math.Max(0f, member.warmth - day * 1.5f * stressFactor);
                    float health = member.health;

                    if (hunger >= 80f)
                        firstHungerCritical ??= day;
                    if (thirst >= 80f)
                        firstThirstCritical ??= day;
                    if (hunger >= 80f || thirst >= 80f)
                        health -= (day - Math.Min(firstHungerCritical ?? day, firstThirstCritical ?? day) + 1) * 1.0f;
                    if (warmth <= 20f)
                        health -= (20f - warmth) * 0.05f;

                    minimumHealth = Math.Min(minimumHealth, health);
                    minimumWarmth = Math.Min(minimumWarmth, warmth);
                }

                trajectory.Add(
                    $"{day}:{minimumHealth:F2}:{minimumWarmth:F2}:" +
                    $"{firstHungerCritical?.ToString() ?? "-"}:" +
                    $"{firstThirstCritical?.ToString() ?? "-"}");
            }

            return new SimulationSnapshot
            {
                MinimumHealthAtDay30 = minimumHealth,
                MinimumWarmthAtDay30 = minimumWarmth,
                FirstCriticalHungerDay = firstHungerCritical,
                FirstCriticalThirstDay = firstThirstCritical,
                TotalInitialDose = totalDose,
                MedicalCoverage = medical,
                RepairCoverage = repair,
                FoodCoverage = food,
                FieldCoverage = field,
                SocialCoverage = social,
                Fingerprint = string.Join("|", trajectory)
            };
        }

        private static bool ContainsAny(string value, params string[] tokens) =>
            tokens.Any(token => value.Contains(token, StringComparison.Ordinal));

        private static string ResolveDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            return Path.GetFullPath(Path.Combine(
                baseDir,
                "..", "..", "..", "..",
                "Assets", "StreamingAssets", "Data"));
        }

        private sealed class SimulationSnapshot
        {
            public float MinimumHealthAtDay30;
            public float MinimumWarmthAtDay30;
            public int? FirstCriticalHungerDay;
            public int? FirstCriticalThirstDay;
            public float TotalInitialDose;
            public int MedicalCoverage;
            public int RepairCoverage;
            public int FoodCoverage;
            public int FieldCoverage;
            public int SocialCoverage;
            public string Fingerprint = string.Empty;
        }
    }
}
