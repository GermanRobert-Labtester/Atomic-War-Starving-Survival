using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Flags;
using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class FactionActionBoardTests : IDisposable
    {
        private readonly string _tempDir;

        public FactionActionBoardTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "ashfall_faction_actions_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            try { Directory.Delete(_tempDir, true); } catch { /* best effort */ }
        }

        // ── helpers ────────────────────────────────────────────────────

        private static FactionActionDefinition Def(
            string id, string factionId, int minDay = 0, int maxDay = 0,
            bool once = false, int cooldownDays = 0,
            string[] requires = null, string[] forbids = null,
            params FactionActionVariant[] variants)
        {
            var def = new FactionActionDefinition
            {
                id = id,
                factionId = factionId,
                title = id,
                minDay = minDay,
                maxDay = maxDay,
                once = once,
                cooldownDays = cooldownDays
            };
            if (requires != null) def.requiresFlags.AddRange(requires);
            if (forbids != null) def.forbidsFlags.AddRange(forbids);
            def.variants.AddRange(variants.Length > 0 ? variants : new[]
            {
                new FactionActionVariant
                {
                    band = "neutral",
                    text = id + " neutral",
                    choices =
                    {
                        new FactionActionChoice { choiceId = "accept", text = "Accept" }
                    }
                }
            });
            return def;
        }

        private sealed class CountingSink : IFactionActionItemSink
        {
            public string LastItemId;
            public int LastAmount;
            public int Calls;
            public bool Deliver(string itemId, int amount)
            {
                Calls++;
                LastItemId = itemId;
                LastAmount = amount;
                return true;
            }
        }

        // ── band computation ───────────────────────────────────────────

        [Theory]
        [InlineData(0f, "hostile")]
        [InlineData(2f, "poor")]
        [InlineData(5f, "neutral")]
        [InlineData(11f, "good")]
        [InlineData(20f, "allied")]
        public void BandForTrust_FollowsContractThresholds(float trust, string expected)
        {
            Assert.Equal(expected, FactionActionBoard.BandForTrust(trust));
        }

        [Fact]
        public void GuildBand_TracksTheGuildsOwnTrust()
        {
            var guild = new ScavengerGuildSystem();
            var board = new FactionActionBoard(guild: guild);
            Assert.Equal("hostile", board.ComputeBand(FactionActionBoard.FactionScavengerGuild));
            guild.AdjustTrust(5f);
            Assert.Equal("neutral", board.ComputeBand(FactionActionBoard.FactionScavengerGuild));
        }

        [Fact]
        public void RaiderBand_ReadsAggressionAndVisibility()
        {
            var raiders = new IronRaidersSystem();
            var board = new FactionActionBoard(raiders: raiders);
            Assert.Equal("neutral", board.ComputeBand(FactionActionBoard.FactionIronRaiders)); // dormant ≠ hostile
            raiders.SetAggressionLevel(0.8f);
            Assert.Equal("hostile", board.ComputeBand(FactionActionBoard.FactionIronRaiders));
            raiders.SetAggressionLevel(0.4f);
            Assert.Equal("neutral", board.ComputeBand(FactionActionBoard.FactionIronRaiders));
            raiders.SetAggressionLevel(0.05f);
            raiders.FortifyApproachRoutes(80f); // visibility 1.0 -> 0.2
            Assert.Equal("allied", board.ComputeBand(FactionActionBoard.FactionIronRaiders));
        }

        [Fact]
        public void CampBand_UnformedIsHostileFormedNeutralMembersRaiseIt()
        {
            var camp = new CoalitionCampSystem();
            var board = new FactionActionBoard(camp: camp);
            Assert.Equal("hostile", board.ComputeBand(FactionActionBoard.FactionDeserterCoalition));
            camp.Form(260);
            Assert.Equal("neutral", board.ComputeBand(FactionActionBoard.FactionDeserterCoalition));
            for (int i = 0; i < 6; i++) camp.RallyDeserter(); // 15 members
            Assert.Equal("allied", board.ComputeBand(FactionActionBoard.FactionDeserterCoalition));
            camp.AdjustLockoutRisk(65);
            Assert.Equal("hostile", board.ComputeBand(FactionActionBoard.FactionDeserterCoalition));
        }

        [Fact]
        public void UnknownFaction_BandsNeutral()
        {
            var board = new FactionActionBoard();
            Assert.Equal("neutral", board.ComputeBand("faction_nobody"));
        }

        // ── loader ─────────────────────────────────────────────────────

        [Fact]
        public void Loader_ParsesAuthoredEntriesAndRejectsFutureSchema()
        {
            string json = @"{""schema_version"":1,""actions"":[{""id"":""act_test_claim"",""faction_id"":""faction_scavenger_guild"",
""title"":""Test Claim"",""text"":""A test."",""min_day"":60,""max_day"":300,""once"":true,""cooldown_days"":7,
""requires_flags"":[""flag_a""],""forbids_flags"":[""flag_b""],
""variants"":[{""band"":""neutral"",""text"":""The registrar waits."",""choices"":[
{""choice_id"":""pay"",""text"":""Pay the fee."",""effects"":{""trust_delta"":2.5,""item_id"":""item_scrap"",""item_amount"":-3,""flags"":[""flag_grievance_test""],""journal"":""journal_test""}}]}]}]}";
            File.WriteAllText(Path.Combine(_tempDir, FactionActionCatalogLoader.FileName), json);
            var actions = FactionActionCatalogLoader.LoadActions(_tempDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Single(actions);
            var def = actions[0];
            Assert.Equal("act_test_claim", def.id);
            Assert.Equal(FactionActionBoard.FactionScavengerGuild, def.factionId);
            Assert.Equal(60, def.minDay);
            Assert.Equal(300, def.maxDay);
            Assert.True(def.once);
            Assert.Equal(7, def.cooldownDays);
            Assert.Equal("flag_a", def.requiresFlags[0]);
            Assert.Equal("flag_b", def.forbidsFlags[0]);
            var fx = def.variants[0].choices[0].effects;
            Assert.Equal(2.5f, fx.trustDelta);
            Assert.Equal("item_scrap", fx.itemId);
            Assert.Equal(-3, fx.itemAmount);
            Assert.Contains("flag_grievance_test", fx.flags);

            File.WriteAllText(Path.Combine(_tempDir, FactionActionCatalogLoader.FileName),
                @"{""schema_version"":99,""actions"":[{""id"":""x"",""faction_id"":""faction_scavenger_guild""}]}");
            var future = FactionActionCatalogLoader.LoadActions(_tempDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Empty(future);
        }

        [Fact]
        public void Loader_MissingFileIsEmpty()
        {
            var actions = FactionActionCatalogLoader.LoadActions(_tempDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Empty(actions);
        }

        // ── availability ───────────────────────────────────────────────

        [Fact]
        public void Availability_RespectsDayWindowOnceCooldownAndFlags()
        {
            var board = new FactionActionBoard();
            board.SetCatalog(new[]
            {
                Def("act_a", FactionActionBoard.FactionScavengerGuild, minDay: 60, maxDay: 100),
                Def("act_b", FactionActionBoard.FactionScavengerGuild, once: true),
                Def("act_c", FactionActionBoard.FactionScavengerGuild, cooldownDays: 10),
                Def("act_d", FactionActionBoard.FactionScavengerGuild, requires: new[] { "flag_gate_open" }),
                Def("act_e", FactionActionBoard.FactionScavengerGuild, forbids: new[] { "flag_gate_closed" }),
                Def("act_f", FactionActionBoard.FactionHydroBarons, minDay: 60,
                    variants: new[]
                    {
                        new FactionActionVariant
                        {
                            band = "neutral",
                            text = "producer",
                            choices =
                            {
                                new FactionActionChoice
                                {
                                    choiceId = "accept",
                                    text = "Accept.",
                                    effects = new FactionActionEffects { flags = { "flag_gate_open", "flag_gate_closed" } }
                                }
                            }
                        }
                    })
            });

            var early = board.AvailableActions(59);              // act_a/act_f window not open yet
            Assert.Equal(3, early.Count);                        // b, c, e (no day floor)
            Assert.DoesNotContain(early, o => o.Definition.id == "act_a");
            Assert.DoesNotContain(early, o => o.Definition.id == "act_f");
            Assert.Equal(5, board.AvailableActions(60).Count);   // a..f minus flag-gated d
            board.Resolve("act_b", "accept", 60);
            Assert.DoesNotContain(board.AvailableActions(61), o => o.Definition.id == "act_b"); // once
            board.Resolve("act_c", "accept", 60);
            Assert.DoesNotContain(board.AvailableActions(65), o => o.Definition.id == "act_c"); // cooldown
            Assert.Contains(board.AvailableActions(75), o => o.Definition.id == "act_c");       // elapsed

            Assert.True(board.Resolve("act_f", "accept", 60));   // produces both gate flags
            Assert.Contains(board.AvailableActions(61), o => o.Definition.id == "act_d");       // requires satisfied
            Assert.DoesNotContain(board.AvailableActions(61), o => o.Definition.id == "act_e"); // forbids now closed
        }

        [Fact]
        public void Availability_OrdinalOrderIsDeterministic()
        {
            var board = new FactionActionBoard();
            board.SetCatalog(new[]
            {
                Def("act_zulu", FactionActionBoard.FactionScavengerGuild),
                Def("act_alpha", FactionActionBoard.FactionScavengerGuild),
                Def("act_mike", FactionActionBoard.FactionHydroBarons)
            });
            var offers = board.AvailableActions(100);
            Assert.Equal(new[] { "act_alpha", "act_mike", "act_zulu" },
                new[] { offers[0].Definition.id, offers[1].Definition.id, offers[2].Definition.id });
        }

        [Fact]
        public void CoalitionActions_RequireFormedCamp()
        {
            var camp = new CoalitionCampSystem();
            var board = new FactionActionBoard(camp: camp);
            board.SetCatalog(new[] { Def("act_camp", FactionActionBoard.FactionDeserterCoalition) });
            Assert.Empty(board.AvailableActions(300));
            camp.Form(260);
            Assert.Single(board.AvailableActions(300));
        }

        // ── variant selection ──────────────────────────────────────────

        [Fact]
        public void VariantSelection_MatchesBandThenNeutralFallback()
        {
            var def = Def("act_v", FactionActionBoard.FactionScavengerGuild, variants: new[]
            {
                new FactionActionVariant { band = "good", text = "friendly terms", choices = { new FactionActionChoice { choiceId = "a" } } },
                new FactionActionVariant { band = "hostile", text = "escorted out", choices = { new FactionActionChoice { choiceId = "b" } } }
            });
            Assert.Equal("good", FactionActionBoard.SelectVariant(def, "good").band);
            Assert.Equal("hostile", FactionActionBoard.SelectVariant(def, "hostile").band);
            Assert.Null(FactionActionBoard.SelectVariant(def, "neutral")); // no neutral, not single-variant

            var single = Def("act_w", FactionActionBoard.FactionScavengerGuild, variants: new[]
            {
                new FactionActionVariant { band = "poor", text = "wary", choices = { new FactionActionChoice { choiceId = "a" } } }
            });
            Assert.Equal("poor", FactionActionBoard.SelectVariant(single, "allied").band); // single-variant fallback
        }

        // ── resolution ─────────────────────────────────────────────────

        [Fact]
        public void Resolve_AppliesTrustThroughTheGuildSeamAndRecordsOnce()
        {
            var guild = new ScavengerGuildSystem();
            var ledger = new InMemoryFlagLedger();
            var board = new FactionActionBoard(guild: guild, ledger: ledger);
            board.SetCatalog(new[]
            {
                Def("act_pay", FactionActionBoard.FactionScavengerGuild, once: true,
                    variants: new[]
                    {
                        new FactionActionVariant
                        {
                            band = "neutral",
                            text = "The registrar names a fee.",
                            choices =
                            {
                                new FactionActionChoice
                                {
                                    choiceId = "pay",
                                    text = "Pay.",
                                    effects = new FactionActionEffects
                                    {
                                        trustDelta = 3f,
                                        itemId = "item_scrap",
                                        itemAmount = -3,
                                        flags = { "flag_favor_scavenger_fee_paid" },
                                        journal = "journal_fee_paid"
                                    }
                                },
                                new FactionActionChoice { choiceId = "refuse", text = "Refuse." }
                            }
                        }
                    })
            });

            var sink = new CountingSink();
            Assert.True(board.Resolve("act_pay", "pay", 100, sink));
            Assert.Equal(3f, guild.Trust);
            Assert.Equal("item_scrap", sink.LastItemId);
            Assert.Equal(-3, sink.LastAmount);
            Assert.True(board.IsFlagSet("flag_favor_scavenger_fee_paid"));
            Assert.True(ledger.IsSet("flag_favor_scavenger_fee_paid"));
            Assert.True(board.HasResolved("act_pay", "pay"));
            Assert.False(board.HasResolved("act_pay", "refuse"));

            // once-only: re-resolution is refused even on a later day
            Assert.False(board.Resolve("act_pay", "refuse", 200, sink));
            Assert.Equal(3f, guild.Trust);
            Assert.Equal(1, sink.Calls);

            // unknown action / unknown choice / day window
            Assert.False(board.Resolve("act_missing", "pay", 100));
        }

        [Fact]
        public void Resolve_RaiderAggressionDeltaClampsThroughTheSystem()
        {
            var raiders = new IronRaidersSystem();
            raiders.SetAggressionLevel(0.4f);
            var board = new FactionActionBoard(raiders: raiders);
            board.SetCatalog(new[]
            {
                Def("act_parley", FactionActionBoard.FactionIronRaiders,
                    variants: new[]
                    {
                        new FactionActionVariant
                        {
                            band = "neutral",
                            text = "Parley under their code.",
                            choices =
                            {
                                new FactionActionChoice
                                {
                                    choiceId = "exchange",
                                    text = "Exchange.",
                                    effects = new FactionActionEffects { aggressionDelta = -0.35f, flags = { "flag_favor_raider_parley_honored" } }
                                }
                            }
                        }
                    })
            });
            Assert.True(board.Resolve("act_parley", "exchange", 90));
            Assert.Equal(0.05f, raiders.AggressionLevel, 3);
            raiders.FortifyApproachRoutes(80f); // visibility 1.0 -> 0.2 (<= 0.3)
            Assert.Equal("allied", board.ComputeBand(FactionActionBoard.FactionIronRaiders));
        }

        [Fact]
        public void Resolve_CooldownBlocksEarlyRepetitionThenAllowsIt()
        {
            var board = new FactionActionBoard();
            board.SetCatalog(new[] { Def("act_toll", FactionActionBoard.FactionHydroBarons, cooldownDays: 7) });
            Assert.True(board.Resolve("act_toll", "accept", 100));
            Assert.False(board.Resolve("act_toll", "accept", 103));
            Assert.True(board.Resolve("act_toll", "accept", 107));
            Assert.Equal(2, board.State.resolved.Count);
        }

        // ── save / load ────────────────────────────────────────────────

        [Fact]
        public void CaptureRestore_RoundTripsAndSnapshotIsolates()
        {
            var board = new FactionActionBoard();
            board.SetCatalog(new[] { Def("act_hist", FactionActionBoard.FactionScavengerGuild, once: true) });
            board.Resolve("act_hist", "accept", 42);
            board.Resolve("act_hist", "accept", 41); // refused: once — state unchanged

            var snapshot = board.CaptureState();
            Assert.Single(snapshot.resolved);
            Assert.Equal("act_hist", snapshot.resolved[0].actionId);

            var restored = new FactionActionBoard();
            restored.SetCatalog(new[] { Def("act_hist", FactionActionBoard.FactionScavengerGuild, once: true) });
            restored.RestoreState(snapshot);
            Assert.True(restored.HasResolved("act_hist"));
            Assert.Empty(restored.AvailableActions(100)); // once honored after load

            // snapshot is a copy: mutating it does not touch the live board
            snapshot.resolved.Clear();
            snapshot.producedFlags.Add("flag_contamination");
            Assert.True(board.HasResolved("act_hist"));
            Assert.False(board.IsFlagSet("flag_contamination"));
        }

        [Fact]
        public void RestoreState_ToleratesNullAndLegacyShape()
        {
            var board = new FactionActionBoard();
            board.RestoreState(null);
            var legacy = new FactionActionBoardState { resolved = null, producedFlags = null };
            board.RestoreState(legacy);
            Assert.Empty(board.AvailableActions(1));
        }

        [Fact]
        public void CaptureState_OrdersResolvedDeterministically()
        {
            var board = new FactionActionBoard();
            board.SetCatalog(new[]
            {
                Def("act_b", FactionActionBoard.FactionHydroBarons, cooldownDays: 1),
                Def("act_a", FactionActionBoard.FactionScavengerGuild, cooldownDays: 1)
            });
            board.Resolve("act_b", "accept", 10);
            board.Resolve("act_a", "accept", 10);
            board.Resolve("act_a", "accept", 12);
            var captured = board.CaptureState();
            Assert.Equal(
                new[] { "act_a", "act_b", "act_a" },
                new[] { captured.resolved[0].actionId, captured.resolved[1].actionId, captured.resolved[2].actionId });
            Assert.Equal(10, captured.resolved[0].day);
            Assert.Equal(12, captured.resolved[2].day);
        }
    }
}
