using System.IO;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class MusterContentCatalogTests : CatalogTestBase
    {
        public MusterContentCatalogTests()
        {
            EnsureJournalVoiceBound();
        }

        private static void EnsureJournalVoiceBound()
        {
            if (JournalVoice.GetCatalog() != null) return;
            string dataDir = FindDataDir();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var loader = new JournalVoiceProseCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
                var catalog = loader.Load(dataDir);
                JournalVoice.BindCatalog(catalog);
            }
        }

        private static string FindDataDir()
        {
            string dataDir = string.Empty;
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) { dataDir = candidate; break; }
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return dataDir;
        }

        [Fact]
        public void WitnessCatalog_LoadsThreeAccounts()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var witnesses = WitnessCatalogLoader.LoadWitnesses(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(3, witnesses.Count);
            Assert.Contains(witnesses, w => w.id == "witness_1_checkpoint_conscript");
            Assert.Contains(witnesses, w => w.id == "witness_2_quartermaster_paperwork");
            Assert.Contains(witnesses, w => w.id == "witness_3_signals_intercept");
            foreach (var w in witnesses)
            {
                Assert.False(string.IsNullOrEmpty(w.knowledgeKey));
                Assert.False(string.IsNullOrEmpty(w.locationId));
                Assert.False(string.IsNullOrEmpty(w.body));
            }
        }

        [Fact]
        public void EpilogueMatrix_LoadsAllEightOutcomes()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var epilogues = EpilogueMatrixLoader.LoadEpilogues(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(epilogues.Count >= 8, $"Expected >= 8 endings, got {epilogues.Count}");
            var keys = new System.Collections.Generic.HashSet<string>();
            foreach (var e in epilogues)
            {
                keys.Add(e.endingKey);
                Assert.False(string.IsNullOrEmpty(e.title));
                Assert.False(string.IsNullOrEmpty(e.prose));
            }
            Assert.Contains("the_open_muster", keys);
            Assert.Contains("the_amnesty", keys);
            Assert.Contains("the_corridor", keys);
            Assert.Contains("the_blood_price", keys);
            Assert.Contains("the_rate_card_revised", keys);
            Assert.Contains("the_administrator", keys);
            Assert.Contains("the_measured_truth_contested", keys);
            Assert.Contains("unwritten", keys);
        }

        [Fact]
        public void EpilogueMatrix_EveryResolvableEndingKeyHasProse()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var epilogues = EpilogueMatrixLoader.LoadEpilogues(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var byKey = new System.Collections.Generic.Dictionary<string, EndingDefinition>();
            foreach (var e in epilogues) byKey[e.endingKey] = e;

            // Every approach in the founding catalog must resolve to a key
            // the matrix can prose (unless the questline is deliberately
            // outside the matrix, e.g. long walk / guild mid-game questlines).
            var sys = new MusterSystem();
            foreach (var def in sys.Catalog)
            {
                foreach (var a in def.approaches)
                {
                    if (string.IsNullOrEmpty(a.endingKey)) continue;
                    Assert.True(byKey.ContainsKey(a.endingKey),
                        $"approach {a.approach} of {def.questlineId} resolves to missing matrix key '{a.endingKey}'");
                }
            }
        }

        [Fact]
        public void JournalVoice_MusterWitnessKeysAreBiasWeighted()
        {
            var paranoid = JournalVoice.ComposeBody(
                KnowledgeKeys.CheckpointConscriptsConfession, RiskBiasTrait.Paranoid);
            var denialist = JournalVoice.ComposeBody(
                KnowledgeKeys.CheckpointConscriptsConfession, RiskBiasTrait.Denialist);
            Assert.NotEqual(paranoid, denialist);
            Assert.Contains("own staff", paranoid);
            Assert.Contains("drunk boy", denialist);

            // Empath does not write the dark accounts down.
            var empath = JournalVoice.ComposeBody(
                KnowledgeKeys.InterceptedCipher, RiskBiasTrait.Empath);
            Assert.DoesNotContain("cipher", empath);

            // All nine keys resolve to non-default text.
            string[] keys =
            {
                KnowledgeKeys.ContinuityReclamationDecree,
                KnowledgeKeys.HydroBaronRateCardOrigin,
                KnowledgeKeys.DeserterCoalitionFounding,
                KnowledgeKeys.ColdCountBeforeTheLab,
                KnowledgeKeys.ProvisionedAdvanceKnowledge,
                KnowledgeKeys.CheckpointConscriptsConfession,
                KnowledgeKeys.QuartermastersPaperwork,
                KnowledgeKeys.InterceptedCipher,
                KnowledgeKeys.LedgerNobodySigned
            };
            foreach (var k in keys)
            {
                var text = JournalVoice.ComposeBody(k, RiskBiasTrait.Realist);
                Assert.NotEqual("Something changed. I wrote it down so I would not forget.", text);
                Assert.False(string.IsNullOrEmpty(text));
            }
        }

        [Fact]
        public void WitnessFraming_IsKeyedToTheAuthorNotTheWitness()
        {
            // Section III: the same account reads differently in a different
            // hand — a Paranoid leans into the assassination reading even for
            // the quartermaster's paperwork; a Denialist downplays it.
            string key = KnowledgeKeys.QuartermastersPaperwork;
            string paranoid = JournalVoice.ComposeBody(key, RiskBiasTrait.Paranoid);
            string denialist = JournalVoice.ComposeBody(key, RiskBiasTrait.Denialist);
            string realist = JournalVoice.ComposeBody(key, RiskBiasTrait.Realist);
            Assert.Contains("bury something", paranoid);
            Assert.Contains("drunk kid", denialist);
            Assert.Contains("Plausible, ordinary", realist);
            Assert.NotEqual(paranoid, denialist);
            Assert.NotEqual(denialist, realist);
        }

        [Fact]
        public void WitnessFraming_SociopathRecordsTransactions()
        {
            string key = KnowledgeKeys.CheckpointConscriptsConfession;
            string sociopath = JournalVoice.ComposeBody(key, RiskBiasTrait.Sociopath);
            Assert.Contains("Source at the checkpoint", sociopath);
            Assert.Contains("Unverified", sociopath);
        }

        [Fact]
        public void MusterSystem_EndingKeyForAny_DetectsResolvedMatrixKey()
        {
            var sys = new MusterSystem();
            Assert.False(sys.EndingKeyForAny("the_corridor"));
            sys.SelectApproach(QuestApproach.C);
            Assert.True(sys.EndingKeyForAny("the_corridor"));
            Assert.False(sys.EndingKeyForAny("the_amnesty"));
        }

        [Fact]
        public void RadioCatalog_HarvenDecreeBroadcastParses()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string raw = fileIO.ReadAllText(fileIO.Combine(dataDir, "year_of_ash_radio.json"));
            Assert.False(string.IsNullOrWhiteSpace(raw));
            var container = json.Deserialize<RadioContainer>(raw);
            Assert.NotNull(container);
            Assert.True(container.broadcasts.Count >= 37);
            var decree = container.broadcasts.Find(b => b.id == "radio_harven_succession_decree");
            Assert.NotNull(decree);
            Assert.Equal(240, decree.dayTrigger);
            Assert.False(string.IsNullOrEmpty(decree.message));
            Assert.Contains("Colonel Harven", decree.message);
        }

        private class RadioContainer
        {
            public System.Collections.Generic.List<RadioEntry> broadcasts = new System.Collections.Generic.List<RadioEntry>();
        }

        private class RadioEntry
        {
            public string id = string.Empty;
            public string frequency = string.Empty;
            public int dayTrigger = default;
            public string source = string.Empty;
            public string message = string.Empty;
        }

        [Fact]
        public void QuestCatalog_MusterQuestlinesHaveRealStages()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var questSystem = new QuestlineSystem();
            int loaded = YearOfAshCatalogLoader.LoadAndRegisterQuests(questSystem, dataDir, fileIO, json);
            Assert.True(loaded >= 32, $"Expected >= 32 external quests, got {loaded}");

            string[] musterQuests =
            {
                "quest_the_muster_uprising", "quest_the_rate_card_war", "quest_the_unsigned_order",
                "quest_four_names_on_the_roster", "quest_the_second_winter",
                "quest_the_eleven_month_circuit", "quest_the_second_color_ledger", "quest_nothing_to_offer"
            };
            foreach (var qid in musterQuests)
            {
                var def = questSystem.FindDefinition(qid);
                Assert.NotNull(def);
                Assert.True(def.stages.Count >= 3, $"{qid} has {def.stages.Count} stages");
                foreach (var s in def.stages)
                    Assert.False(string.IsNullOrEmpty(s.narrativePrompt));
            }
        }
    }
}
