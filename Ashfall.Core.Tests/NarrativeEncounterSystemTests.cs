using System.IO;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class NarrativeEncounterSystemTests
    {
        private static NarrativeEncounterSystem NewSystem()
        {
            var sys = new NarrativeEncounterSystem();
            sys.RegisterEncounter(new EncounterDefinition
            {
                id = "enc_a",
                title = "A",
                category = "Discovery",
                baseWeight = 1f,
                minDangerLevel = 0f,
                choices = new System.Collections.Generic.List<EncounterChoiceDefinition>
                {
                    new EncounterChoiceDefinition { choiceId = "c", text = "Go", moraleDelta = 1, guiltDelta = 0 }
                }
            });
            sys.RegisterEncounter(new EncounterDefinition
            {
                id = "enc_b",
                title = "B",
                category = "Hazard",
                baseWeight = 3f,
                minDangerLevel = 2f,
                choices = new System.Collections.Generic.List<EncounterChoiceDefinition>
                {
                    new EncounterChoiceDefinition { choiceId = "c", text = "Go", moraleDelta = 2, guiltDelta = 1 }
                }
            });
            sys.RegisterEncounter(new EncounterDefinition
            {
                id = "enc_c",
                title = "C",
                category = "Social",
                baseWeight = 1f,
                minDangerLevel = 0f,
                requiredLocationId = "loc_specific",
                choices = new System.Collections.Generic.List<EncounterChoiceDefinition>
                {
                    new EncounterChoiceDefinition { choiceId = "c", text = "Go", moraleDelta = 3, guiltDelta = 0 }
                }
            });
            return sys;
        }

        [Fact]
        public void Register_NullAndDuplicateIgnored()
        {
            var sys = new NarrativeEncounterSystem();
            sys.RegisterEncounter(null);
            sys.RegisterEncounter(new EncounterDefinition());
            Assert.Empty(sys.Catalog);
            sys.RegisterEncounter(new EncounterDefinition { id = "enc_a" });
            sys.RegisterEncounter(new EncounterDefinition { id = "enc_a" });
            Assert.Single(sys.Catalog);
        }

        [Fact]
        public void Select_WeightsPreferHigherBaseWeight()
        {
            var sys = NewSystem();
            int a = 0, b = 0;
            var rng = new SeededRng(123);
            for (int i = 0; i < 200; i++)
            {
                var picked = sys.SelectEncounter("Stealth", 3f, null, rng);
                if (picked != null && picked.id == "enc_a") a++;
                if (picked != null && picked.id == "enc_b") b++;
            }
            Assert.True(b > a, $"enc_b (weight 3) should beat enc_a (weight 1): b={b} a={a}");
        }

        [Fact]
        public void Select_FiltersByDangerAndLocation()
        {
            var sys = NewSystem();
            // danger 1 excludes enc_b (min 2); location filter excludes enc_c everywhere but loc_specific.
            for (int i = 0; i < 50; i++)
            {
                var picked = sys.SelectEncounter("Stealth", 1f, "loc_elsewhere", new SeededRng(i));
                if (picked != null)
                {
                    Assert.NotEqual("enc_b", picked.id);
                    Assert.NotEqual("enc_c", picked.id);
                }
            }
            // At loc_specific, enc_c is eligible.
            bool sawC = false;
            for (int i = 0; i < 100 && !sawC; i++)
            {
                var picked = sys.SelectEncounter("Stealth", 1f, "loc_specific", new SeededRng(i));
                if (picked != null && picked.id == "enc_c") sawC = true;
            }
            Assert.True(sawC);
        }

        [Fact]
        public void StanceMultipliers_MatchUnityValues()
        {
            var sys = NewSystem();
            var b = sys.Find("enc_b");
            // Unity: stealth x0.5 -> 1.5, speed x1.5 -> 4.5, base 3.0.
            Assert.Equal(1.5f, b.GetEffectiveWeight("Stealth", 3f, null));
            Assert.Equal(4.5f, b.GetEffectiveWeight("Speed", 3f, null));
            Assert.Equal(3.0f, b.GetEffectiveWeight("Stealth", 3f, null) / 0.5f);
        }

        [Fact]
        public void Select_ReturnsNullWhenNothingEligible()
        {
            var sys = NewSystem();
            // danger below minDangerLevel for everything except... enc_b min 2, a/c min 0.
            // Force nothing eligible: location lockout only affects enc_c; use danger 0 + null location.
            var picked = sys.SelectEncounter("Stealth", 0f, null, new SeededRng(1));
            Assert.NotNull(picked);
            // Danger way below the floor of enc_b is still fine for a/c. Build a system with a floor:
            var strict = new NarrativeEncounterSystem();
            strict.RegisterEncounter(new EncounterDefinition { id = "enc_x", baseWeight = 1f, minDangerLevel = 5f });
            Assert.Null(strict.SelectEncounter("Stealth", 1f, null, new SeededRng(1)));
        }

        [Fact]
        public void Resolve_RecordsHistoryAndTotals()
        {
            var sys = NewSystem();
            int resolved = 0;
            sys.OnEncounterResolved += r => resolved++;
            Assert.True(sys.Resolve("enc_a", "c", "loc_x", 40));
            Assert.Equal(1, resolved);
            Assert.Equal(1, sys.TotalResolved);
            Assert.Equal("c", sys.State.history[0].choiceId);
            Assert.Equal(40, sys.State.history[0].day);
        }

        [Fact]
        public void Resolve_UnknownEncounterOrChoiceRejected()
        {
            var sys = NewSystem();
            Assert.False(sys.Resolve("enc_missing", "c", null, 40));
            Assert.False(sys.Resolve("enc_a", "missing_choice", null, 40));
            Assert.Equal(0, sys.TotalResolved);
        }

        [Fact]
        public void CaptureState_ReturnsSnapshotNotLiveState()
        {
            var sys = NewSystem();
            sys.Resolve("enc_a", "c", "loc_x", 40);
            var snapshot = sys.CaptureState();
            snapshot.history[0].moraleDelta = 99;
            snapshot.cumulativeMorale = 999;
            Assert.Equal(1, sys.State.cumulativeMorale);
            Assert.Equal(1, sys.State.history[0].moraleDelta);
        }

        [Fact]
        public void CaptureState_EmitsInOrdinalOrder()
        {
            var sys = NewSystem();
            sys.Resolve("enc_b", "c", "loc", 45);
            sys.Resolve("enc_a", "c", "loc", 40);
            var snapshot = sys.CaptureState();
            Assert.Equal(40, snapshot.history[0].day);
            Assert.Equal("enc_a", snapshot.history[0].encounterId);
            Assert.Equal("enc_b", snapshot.history[1].encounterId);
        }

        [Fact]
        public void SaveLoad_RoundTripsAllState()
        {
            var sys = NewSystem();
            sys.Resolve("enc_a", "c", "loc_x", 40);
            sys.Resolve("enc_b", "c", "loc_y", 41);
            var restored = new NarrativeEncounterSystem();
            restored.RestoreState(sys.CaptureState());
            Assert.Equal(2, restored.TotalResolved);
            Assert.Equal(2, restored.State.history.Count);
            Assert.Equal(sys.State.cumulativeMorale, restored.State.cumulativeMorale);
        }

        [Fact]
        public void SaveLoad_ChecksumStable()
        {
            var sys = NewSystem();
            sys.Resolve("enc_a", "c", "loc", 40);
            sys.Resolve("enc_c", "c", "loc_specific", 41);
            string before = SaveChecksum.Compute(sys.CaptureState());
            var restored = new NarrativeEncounterSystem();
            restored.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());
            Assert.Equal(before, after);
        }

        // ── Data catalog ───────────────────────────────────────────────

        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }

        [Fact]
        public void Catalog_LoadsTheThreeUnityEncounters()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var defs = NarrativeEncounterCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(62, defs.Count);
            Assert.Contains(defs, d => d.id == "enc_dead_letter_office");
            Assert.Contains(defs, d => d.id == "enc_weather_station");
            Assert.Contains(defs, d => d.id == "enc_pianist");
            for (int i = 0; i < defs.Count; i++)
            {
                var d = defs[i];
                Assert.False(string.IsNullOrEmpty(d.title));
                Assert.False(string.IsNullOrEmpty(d.description));
                Assert.True(d.choices.Count >= 2);
                for (int j = 0; j < d.choices.Count; j++)
                {
                    var c = d.choices[j];
                    Assert.False(string.IsNullOrEmpty(c.choiceId));
                }
            }
        }

        [Fact]
        public void Catalog_UnityWeightParity()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var defs = NarrativeEncounterCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var deadLetter = defs.Find(d => d.id == "enc_dead_letter_office");
            Assert.NotNull(deadLetter);
            // Unity: base 2.0, stealth x0.5 = 1.0, speed x1.5 = 3.0.
            Assert.Equal(1.0f, deadLetter.GetEffectiveWeight("Stealth", 1f, null));
            Assert.Equal(3.0f, deadLetter.GetEffectiveWeight("Speed", 1f, null));
            // minDangerLevel 0 keeps it eligible at any danger.
            Assert.True(deadLetter.GetEffectiveWeight("Speed", 5f, null) > 0f);
        }
    }
}
