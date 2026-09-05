using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.NpcArcs;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 52 — recurring-NPC arc coverage: catalog contract, deterministic
    /// resolution precedence (death &gt; recruited &gt; terminal &gt; branch
    /// &gt; day fallback), choice memory, recruitment/death terminality via
    /// the existing roster authority, save round-trips through the quest +
    /// roster stores, encounter→quest bridge, distress suppression, and
    /// multi-NPC isolation.
    /// </summary>
    public class NpcArcSystemTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        // ── helpers ────────────────────────────────────────────────────

        private sealed class Fixture
        {
            public ExpansionQuestSystem Quests = new ExpansionQuestSystem();
            public SurvivorRosterSystem Roster = new SurvivorRosterSystem();
            public int Day;
            public NpcArcSystem Arcs = null!;

            /// <summary>Minimal bound quest catalog so TickDay auto-starts
            /// the test quests exactly like the host's day owner does.</summary>
            private static List<ExpansionQuestEntry> DefaultQuestCatalog() =>
                new List<ExpansionQuestEntry>
                {
                    new ExpansionQuestEntry
                    {
                        id = "quest_arc_mara_test_01",
                        minDay = 1,
                        maxDay = 365,
                        choices = new List<ExpansionQuestChoice>
                        {
                            new ExpansionQuestChoice { id = "help" },
                            new ExpansionQuestChoice { id = "exploit" }
                        }
                    },
                    new ExpansionQuestEntry { id = "quest_contact", minDay = 1, maxDay = 365 },
                    new ExpansionQuestEntry { id = "quest_arc_ext_test_death", minDay = 1, maxDay = 365 }
                };

            public static Fixture Create(NpcArcCatalog catalog)
            {
                var f = new Fixture();
                f.Quests.BindCatalog(DefaultQuestCatalog());
                f.Arcs = new NpcArcSystem(catalog, () => f.Day, f.Quests, f.Roster);
                return f;
            }

            /// <summary>Simulate the host path: quest auto-starts in its
            /// window (ExpansionQuestSystem.TickDay), then an encounter
            /// decision completes it with a chosen choice.</summary>
            public void Choose(string questId, string choiceId)
            {
                Quests.TickDay(Day);
                Quests.MakeChoice(questId, choiceId, Day);
                if (!Quests.IsCompleted(questId))
                    Quests.CompleteQuest(questId, Day);
            }

            public void Recruit(string npcId, string displayName)
            {
                Roster.RegisterDefinition(new SurvivorDefinition
                {
                    id = npcId,
                    displayName = displayName,
                    baseHealth = 100f
                });
                Assert.True(Roster.Join(npcId, Day), "roster join must succeed for a fresh definition");
            }
        }

        private static NpcArcStateDefinition State(
            string id, string kind = "initial", int minDay = 0, int maxDay = 0, int precedence = 10,
            bool terminal = false, bool whenRecruited = false, bool whenDead = false,
            List<string>? requiresCompleted = null, List<string>? excludesCompleted = null,
            List<NpcArcChoiceCondition>? requiresChoice = null,
            string role = "", string locationId = "")
        {
            return new NpcArcStateDefinition
            {
                id = id,
                kind = kind,
                min_day = minDay,
                max_day = maxDay,
                precedence = precedence,
                terminal = terminal,
                when_recruited = whenRecruited,
                when_dead = whenDead,
                requires_completed = requiresCompleted ?? new List<string>(),
                excludes_completed = excludesCompleted ?? new List<string>(),
                requires_choice = requiresChoice ?? new List<NpcArcChoiceCondition>(),
                role = role,
                location_id = locationId
            };
        }

        private static NpcArcCatalog SingleArcCatalog(string npcId, params NpcArcStateDefinition[] states)
        {
            var catalog = new NpcArcCatalog();
            catalog.Register(new NpcArcDefinition
            {
                npc_id = npcId,
                display_name = npcId,
                states = new List<NpcArcStateDefinition>(states)
            });
            return catalog;
        }

        /// <summary>Canonical trader arc shape (authored conventions mirrored
        /// from the real npc_arcs.json data).</summary>
        private static NpcArcCatalog TraderCatalog()
        {
            var catalog = new NpcArcCatalog();
            catalog.Register(new NpcArcDefinition
            {
                npc_id = "npc_mara_test",
                display_name = "Mara Test",
                flagship = true,
                states = new List<NpcArcStateDefinition>
                {
                    State("dead", kind: "terminal", precedence: 100, terminal: true, whenDead: true),
                    State("recruited", kind: "terminal", precedence: 60, terminal: true, whenRecruited: true),
                    State("late_embargo", kind: "late", minDay: 70, maxDay: 0, precedence: 30,
                        requiresChoice: new List<NpcArcChoiceCondition>
                        {
                            new NpcArcChoiceCondition { quest_id = "quest_arc_mara_test_01", choice_id = "exploit" }
                        },
                        role: "Embargo Organizer", terminal: false),
                    State("late_ally", kind: "late", minDay: 70, maxDay: 0, precedence: 30,
                        requiresChoice: new List<NpcArcChoiceCondition>
                        {
                            new NpcArcChoiceCondition { quest_id = "quest_arc_mara_test_01", choice_id = "help" }
                        },
                        role: "Trade Coordinator"),
                    State("evolved_injured", kind: "evolved", minDay: 35, maxDay: 69, precedence: 20,
                        requiresCompleted: new List<string> { "quest_arc_mara_test_01" },
                        role: "Injured Caravaner"),
                    State("initial", kind: "initial", minDay: 12, maxDay: 34, precedence: 10,
                        role: "Waystation Trader")
                }
            });
            return catalog;
        }

        // ── catalog contract ───────────────────────────────────────────

        [Fact]
        public void Catalog_MissingFileYieldsEmptyCatalog()
        {
            var catalog = NpcArcCatalog.Load("/nonexistent_dir_that_does_not_exist");
            Assert.Empty(catalog.Arcs);
        }

        [Fact]
        public void Resolve_UnknownNpcIsUnresolvedNotCrash()
        {
            var arcs = Fixture.Create(new NpcArcCatalog()).Arcs;
            var r = arcs.Resolve("npc_nobody");
            Assert.False(r.ArcFound);
            Assert.Equal(string.Empty, r.StateId);
        }

        // ── day windows ────────────────────────────────────────────────

        [Fact]
        public void DayWindows_BeforeWindowResolvesNothing()
        {
            var f = Fixture.Create(TraderCatalog());
            f.Day = 5;
            Assert.Equal(string.Empty, f.Arcs.Resolve("npc_mara_test").StateId);
        }

        [Fact]
        public void DayWindows_InsideWindowResolvesInitial()
        {
            var f = Fixture.Create(TraderCatalog());
            f.Day = 20;
            var r = f.Arcs.Resolve("npc_mara_test");
            Assert.Equal("initial", r.StateId);
            Assert.Equal("Waystation Trader", r.Role);
        }

        [Fact]
        public void DayWindows_InjuredRangeRequiresContactCompleted()
        {
            var f = Fixture.Create(TraderCatalog());
            f.Day = 40;
            // No contact yet: evolved branch condition unmet → nothing in window.
            Assert.Equal(string.Empty, f.Arcs.Resolve("npc_mara_test").StateId);

            f.Choose("quest_arc_mara_test_01", "help");
            Assert.Equal("evolved_injured", f.Arcs.Resolve("npc_mara_test").StateId);
        }

        [Fact]
        public void DayWindows_ExactBoundaryIsInclusive()
        {
            var f = Fixture.Create(TraderCatalog());
            f.Day = 12;
            Assert.Equal("initial", f.Arcs.Resolve("npc_mara_test").StateId);
            f.Day = 34;
            Assert.Equal("initial", f.Arcs.Resolve("npc_mara_test").StateId);
            f.Day = 35;
            Assert.NotEqual("initial", f.Arcs.Resolve("npc_mara_test").StateId);
        }

        // ── choice memory branches ─────────────────────────────────────

        [Fact]
        public void ChoiceMemory_HelpBranchResolvesLateAlly()
        {
            var f = Fixture.Create(TraderCatalog());
            f.Day = 20;
            f.Choose("quest_arc_mara_test_01", "help");
            f.Day = 80;
            var r = f.Arcs.Resolve("npc_mara_test");
            Assert.Equal("late_ally", r.StateId);
            Assert.False(r.Terminal);
        }

        [Fact]
        public void ChoiceMemory_ExploitBranchResolvesLateEmbargo()
        {
            var f = Fixture.Create(TraderCatalog());
            f.Day = 20;
            f.Choose("quest_arc_mara_test_01", "exploit");
            f.Day = 80;
            Assert.Equal("late_embargo", f.Arcs.Resolve("npc_mara_test").StateId);
        }

        [Fact]
        public void ChoiceMemory_BranchesAreMutuallyExclusive()
        {
            var f = Fixture.Create(TraderCatalog());
            f.Day = 20;
            f.Choose("quest_arc_mara_test_01", "help");
            f.Day = 80;
            Assert.NotEqual("late_embargo", f.Arcs.Resolve("npc_mara_test").StateId);
        }

        // ── recruitment / death terminality ────────────────────────────

        [Fact]
        public void Recruitment_RecruitedNpcLeavesExternalStates()
        {
            var f = Fixture.Create(TraderCatalog());
            f.Day = 20;
            f.Choose("quest_arc_mara_test_01", "help");
            f.Recruit("npc_mara_test", "Mara Test");
            f.Day = 80;
            var r = f.Arcs.Resolve("npc_mara_test");
            Assert.Equal("recruited", r.StateId);
            Assert.True(r.Recruited);
            Assert.True(r.Terminal);
        }

        [Fact]
        public void Recruitment_PersistsThroughRosterSaveRoundTrip()
        {
            var f = Fixture.Create(TraderCatalog());
            f.Day = 30;
            f.Recruit("npc_mara_test", "Mara Test");

            var saved = f.Roster.CaptureState();
            var restored = new SurvivorRosterSystem();
            restored.RestoreState(saved);

            var arcs = new NpcArcSystem(TraderCatalog(), () => 90, f.Quests, restored);
            var r = arcs.Resolve("npc_mara_test");
            Assert.Equal("recruited", r.StateId);
        }

        [Fact]
        public void Death_RosterDeathIsTerminalAndOutranksDay()
        {
            var f = Fixture.Create(TraderCatalog());
            f.Day = 20;
            f.Choose("quest_arc_mara_test_01", "help");
            f.Recruit("npc_mara_test", "Mara Test");
            Assert.True(f.Roster.Die("npc_mara_test", "collapse"));
            f.Day = 200;
            var r = f.Arcs.Resolve("npc_mara_test");
            Assert.Equal("dead", r.StateId);
            Assert.True(r.Dead);
            Assert.True(r.Terminal);
        }

        [Fact]
        public void Death_FutureDaysDoNotResurrect()
        {
            var f = Fixture.Create(TraderCatalog());
            f.Day = 20;
            f.Recruit("npc_mara_test", "Mara Test");
            f.Roster.Die("npc_mara_test", "fever");
            foreach (int day in new[] { 21, 40, 80, 200, 500 })
            {
                f.Day = day;
                Assert.Equal("dead", f.Arcs.Resolve("npc_mara_test").StateId);
            }
        }

        [Fact]
        public void Death_AuthoredTerminalQuestOverridesBranches()
        {
            var catalog = new NpcArcCatalog();
            catalog.Register(new NpcArcDefinition
            {
                npc_id = "npc_ext_test",
                states = new List<NpcArcStateDefinition>
                {
                    State("late", kind: "late", minDay: 50, precedence: 30,
                        requiresCompleted: new List<string> { "quest_contact" }),
                    State("gone", kind: "terminal", precedence: 90, terminal: true,
                        requiresCompleted: new List<string> { "quest_arc_ext_test_death" }),
                    State("initial", kind: "initial", minDay: 5, precedence: 10)
                }
            });
            var f = Fixture.Create(catalog);
            f.Day = 20;
            f.Choose("quest_contact", "go");
            f.Choose("quest_arc_ext_test_death", "found_dead");
            f.Day = 120;
            var r = f.Arcs.Resolve("npc_ext_test");
            Assert.Equal("gone", r.StateId);
            Assert.True(r.Terminal);
        }

        // ── determinism / isolation ────────────────────────────────────

        [Fact]
        public void Determinism_SameFactsResolveSameState()
        {
            var f1 = Fixture.Create(TraderCatalog());
            var f2 = Fixture.Create(TraderCatalog());
            foreach (var f in new[] { f1, f2 })
            {
                f.Day = 20;
                f.Choose("quest_arc_mara_test_01", "help");
                f.Day = 80;
            }
            var a = f1.Arcs.Resolve("npc_mara_test");
            var b = f2.Arcs.Resolve("npc_mara_test");
            Assert.Equal(a.StateId, b.StateId);
            Assert.Equal(a.Role, b.Role);
        }

        [Fact]
        public void Determinism_QuestSaveRoundTripPreservesResolution()
        {
            var f = Fixture.Create(TraderCatalog());
            f.Day = 20;
            f.Choose("quest_arc_mara_test_01", "exploit");

            var saved = f.Quests.CaptureState();
            var reloaded = new ExpansionQuestSystem();
            reloaded.RestoreState(saved);

            var arcs = new NpcArcSystem(TraderCatalog(), () => 80, reloaded, f.Roster);
            Assert.Equal("late_embargo", arcs.Resolve("npc_mara_test").StateId);
        }

        [Fact]
        public void MultiNpc_Isolation_OneArcDoesNotMutateAnother()
        {
            var catalog = TraderCatalog();
            catalog.Register(new NpcArcDefinition
            {
                npc_id = "npc_oskar_test",
                states = new List<NpcArcStateDefinition>
                {
                    State("initial", kind: "initial", minDay: 10, precedence: 10)
                }
            });
            var f = Fixture.Create(catalog);
            f.Day = 20;
            f.Choose("quest_arc_mara_test_01", "help");
            f.Day = 80;
            Assert.Equal("initial", f.Arcs.Resolve("npc_oskar_test").StateId);
            Assert.Equal("late_ally", f.Arcs.Resolve("npc_mara_test").StateId);
        }

        [Fact]
        public void ResolveAll_IsSortedByNpcId()
        {
            var f = Fixture.Create(TraderCatalog());
            var all = f.Arcs.ResolveAll();
            Assert.True(all.Count >= 1);
            for (int i = 1; i < all.Count; i++)
                Assert.True(string.CompareOrdinal(all[i - 1].NpcId, all[i].NpcId) < 0);
        }

        // ── encounter → quest bridge ───────────────────────────────────

        [Fact]
        public void EncounterBridge_ChoiceRecordsArcDecision()
        {
            var quests = new ExpansionQuestSystem();
            quests.BindCatalog(new List<ExpansionQuestEntry>
            {
                new ExpansionQuestEntry
                {
                    id = "quest_arc_mara_test_01",
                    title = "Waystation Favor",
                    minDay = 5,
                    maxDay = 365,
                    choices = new List<ExpansionQuestChoice>
                    {
                        new ExpansionQuestChoice { id = "help" },
                        new ExpansionQuestChoice { id = "exploit" }
                    }
                }
            });
            var engine = new NarrativeEncounterSystem { QuestLink = quests };
            engine.RegisterEncounter(new EncounterDefinition
            {
                id = "enc_mara_test",
                title = "Waystation Trader",
                choices = new List<EncounterChoiceDefinition>
                {
                    new EncounterChoiceDefinition
                    {
                        choiceId = "share_medicine",
                        text = "Hand over the antibiotics.",
                        completesQuestId = "quest_arc_mara_test_01",
                        completesQuestChoiceId = "help"
                    }
                }
            });

            Assert.True(engine.Resolve("enc_mara_test", "share_medicine", "loc_x", 20));
            Assert.True(quests.IsCompleted("quest_arc_mara_test_01"));
            Assert.Equal("help", quests.GetProgress("quest_arc_mara_test_01")!.currentChoiceId);

            var arcs = new NpcArcSystem(TraderCatalog(), () => 40, quests, new SurvivorRosterSystem());
            Assert.Equal("evolved_injured", arcs.Resolve("npc_mara_test").StateId);
        }

        [Fact]
        public void EncounterBridge_NullQuestLinkLeavesResolutionUntouched()
        {
            var engine = new NarrativeEncounterSystem();
            engine.RegisterEncounter(new EncounterDefinition
            {
                id = "enc_mara_test",
                choices = new List<EncounterChoiceDefinition>
                {
                    new EncounterChoiceDefinition
                    {
                        choiceId = "share_medicine",
                        completesQuestId = "quest_arc_mara_test_01",
                        completesQuestChoiceId = "help"
                    }
                }
            });
            Assert.True(engine.Resolve("enc_mara_test", "share_medicine", "loc_x", 20));
            Assert.Equal(1, engine.TotalResolved);
        }

        // ── distress suppression ───────────────────────────────────────

        [Fact]
        public void Distress_SuppressedOnceArcIsTerminal()
        {
            var f = Fixture.Create(TraderCatalog());
            Assert.False(f.Arcs.IsSignalSuppressed("npc_mara_test"));

            f.Recruit("npc_mara_test", "Mara Test");
            Assert.True(f.Arcs.IsSignalSuppressed("npc_mara_test"));
        }

        [Fact]
        public void Distress_InterceptRejectsSuppressedNpcSignal()
        {
            var f = Fixture.Create(TraderCatalog());
            f.Recruit("npc_mara_test", "Mara Test");

            var distress = new RadioDistressSystem
            {
                NpcSignalSuppressionFilter = f.Arcs.IsSignalSuppressed
            };
            distress.RegisterSignal(new DistressSignalDefinition
            {
                FrequencyId = "freq_arc_mara_test",
                FrequencyMhzStr = "121.5",
                SourceName = "Waystation relay",
                DaysToTrace = 3,
                NpcId = "npc_mara_test",
                ResolveQuestId = "quest_arc_mara_test_signal"
            });
            Assert.False(distress.Intercept("freq_arc_mara_test", 30));
            Assert.Equal(DistressSignalStatus.Inactive, distress.GetActiveState("freq_arc_mara_test")!.Status);
        }

        [Fact]
        public void Distress_AnonymousSignalsNeverSuppressed()
        {
            var f = Fixture.Create(TraderCatalog());
            f.Recruit("npc_mara_test", "Mara Test");
            var distress = new RadioDistressSystem
            {
                NpcSignalSuppressionFilter = f.Arcs.IsSignalSuppressed
            };
            distress.RegisterSignal(new DistressSignalDefinition
            {
                FrequencyId = "freq_anon",
                FrequencyMhzStr = "118.0",
                SourceName = "Automated beacon",
                DaysToTrace = 3
            });
            Assert.True(distress.Intercept("freq_anon", 30));
        }
    }
}
