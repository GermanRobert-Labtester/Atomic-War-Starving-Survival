// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text.Json;
using Ashfall.Core.Endgame;
using Ashfall.Core.Flags;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ChapterProfileTests
    {
        private static readonly string DataDir = ResolveDataDir();

        private static string ResolveDataDir()
        {
            string candidate = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data"));
            if (Directory.Exists(candidate)) return candidate;
            candidate = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data"));
            if (Directory.Exists(candidate)) return candidate;
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate Assets/StreamingAssets/Data");
        }

        [Fact]
        public void Catalog_LoadsFromAuthoritativeData_ValidatesCleanly()
        {
            string path = Path.Combine(DataDir, "chapter_profiles.json");
            Assert.True(File.Exists(path), $"Catalog missing at {path}");

            string json = File.ReadAllText(path);
            var catalog = ChapterProfileCatalog.LoadFromJson(json);

            Assert.True(catalog.IsValid, $"Validation failed: {string.Join("; ", catalog.ValidationErrors)}");
            Assert.Empty(catalog.ValidationErrors);
            Assert.True(catalog.Profiles.Count >= 6, $"Expected >= 6 profiles, got {catalog.Profiles.Count}");
            Assert.True(catalog.Profiles.ContainsKey("profile_base_v1"));
            Assert.True(catalog.Profiles.ContainsKey("profile_military"));
            Assert.True(catalog.Profiles.ContainsKey("profile_rebel"));
            Assert.True(catalog.Profiles.ContainsKey("profile_independent"));
            Assert.True(catalog.Profiles.ContainsKey("profile_muster"));
            Assert.True(catalog.Profiles.ContainsKey("profile_standing_d"));
        }

        [Fact]
        public void Catalog_ProfileBaseV1_MatchesLegacyYearOneBaseline()
        {
            string path = Path.Combine(DataDir, "chapter_profiles.json");
            string json = File.ReadAllText(path);
            var catalog = ChapterProfileCatalog.LoadFromJson(json);

            var profile = catalog.Profiles["profile_base_v1"];
            Assert.Equal(0, profile.reckoning_offset);
            Assert.Equal(160, profile.knowing_day);
            Assert.Equal(210, profile.culpable_day);
            Assert.Equal(240, profile.counted_day);
            Assert.Equal(360, profile.reading_day);
            Assert.Equal("legacy_context", profile.year_one_ending_source);
            Assert.Equal("fixed_day", profile.close_rule);
            Assert.Equal(-1, profile.waive_evidence_gate_after_day);
            Assert.Equal("standing_base_neutral", profile.default_standing_modifier);
        }

        [Fact]
        public void Catalog_All135BranchEndings_ResolveToStandingModifier()
        {
            string path = Path.Combine(DataDir, "chapter_profiles.json");
            string json = File.ReadAllText(path);
            var catalog = ChapterProfileCatalog.LoadFromJson(json);

            string[] branchFiles = { "military_faction_branch.json", "rebel_faction_branch.json", "independent_faction_branch.json" };
            int totalEndingsChecked = 0;

            foreach (var branchFile in branchFiles)
            {
                string bfPath = Path.Combine(DataDir, branchFile);
                if (!File.Exists(bfPath)) continue;

                using var doc = JsonDocument.Parse(File.ReadAllText(bfPath));
                if (doc.RootElement.TryGetProperty("branches", out var branchesArr) && branchesArr.ValueKind == JsonValueKind.Array)
                {
                    foreach (var branchEl in branchesArr.EnumerateArray())
                    {
                        if (branchEl.TryGetProperty("endings", out var endingsArr) && endingsArr.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var endingEl in endingsArr.EnumerateArray())
                            {
                                if (endingEl.TryGetProperty("ending_id", out var eidProp) && eidProp.GetString() is string endingId)
                                {
                                    totalEndingsChecked++;
                                    foreach (var profile in catalog.Profiles.Values)
                                    {
                                        string modId = catalog.ResolveStandingModifier(endingId, profile.profile_id);
                                        Assert.False(string.IsNullOrWhiteSpace(modId));
                                        Assert.True(catalog.StandingModifiers.ContainsKey(modId), $"Modifier '{modId}' not in standing modifiers.");
                                        var mod = catalog.StandingModifiers[modId];
                                        Assert.Equal(modId, mod.modifier_id);
                                        Assert.False(string.IsNullOrWhiteSpace(mod.display_name));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Assert.Equal(135, totalEndingsChecked);
        }

        [Theory]
        [InlineData("military", "profile_military")]
        [InlineData("rebel", "profile_rebel")]
        [InlineData("independent", "profile_independent")]
        [InlineData("muster", "profile_muster")]
        [InlineData("standing_d", "profile_standing_d")]
        [InlineData("neutral", "profile_base_v1")]
        public void Resolver_ResolvesProfileId_AcrossAllArchetypes(string archetype, string expectedProfileId)
        {
            var ctx = new ChapterProfileResolutionContext();
            switch (archetype)
            {
                case "military":
                    ctx.FactionBranchKind = Ashfall.Core.Factions.FactionBranchKind.Military;
                    break;
                case "rebel":
                    ctx.FactionBranchKind = Ashfall.Core.Factions.FactionBranchKind.Rebel;
                    break;
                case "independent":
                    ctx.FactionBranchKind = Ashfall.Core.Factions.FactionBranchKind.Independent;
                    break;
                case "muster":
                    ctx.HasMusterApproach = true;
                    break;
                case "standing_d":
                    ctx.IsStandingD = true;
                    break;
                case "neutral":
                default:
                    break;
            }

            string resolved = ChapterProfileResolver.ResolveProfileId(ctx);
            Assert.Equal(expectedProfileId, resolved);
        }

        [Fact]
        public void CloseRule_FixedDay_ReturnsReadingDay()
        {
            var profile = new ChapterProfileDef
            {
                close_rule = "fixed_day",
                reading_day = 360,
                floor_day = 300,
                ceiling_day = 400
            };

            Assert.Equal(360, profile.EvaluateCloseDay(false, 0));
            Assert.Equal(360, profile.EvaluateCloseDay(true, 280));
            Assert.Equal(360, profile.EvaluateCloseDay(true, 390));
        }

        [Fact]
        public void CloseRule_EndingResolved_AppliesSettleWindowAndClamps()
        {
            var profile = new ChapterProfileDef
            {
                close_rule = "ending_resolved",
                reading_day = 360,
                settle_window_days = 14,
                floor_day = 300,
                ceiling_day = 400
            };

            // Unresolved defaults to ceiling_day
            Assert.Equal(400, profile.EvaluateCloseDay(false, 0));

            // Resolved at 310 + 14 = 324 (within [300, 400])
            Assert.Equal(324, profile.EvaluateCloseDay(true, 310));

            // Resolved early at 250 + 14 = 264 -> clamped to floor 300
            Assert.Equal(300, profile.EvaluateCloseDay(true, 250));

            // Resolved late at 395 + 14 = 409 -> clamped to ceiling 400
            Assert.Equal(400, profile.EvaluateCloseDay(true, 395));
        }

        [Fact]
        public void EndgameSystem_EvaluateEnding_WithProfileAndBranchEnding()
        {
            var endgame = new EndgameSystem();
            endgame.SetProfileId("profile_military");

            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 360,
                LivingSurvivors = 8,
                DeceasedSurvivors = 1,
                AverageMorale = 70f,
                ExpeditionsCount = 5
            };

            var profile = new ChapterProfileDef
            {
                profile_id = "profile_military",
                display_name = "Military Garrison Authority",
                year_one_ending_source = "faction_branch",
                family = "garrison",
                floor_day = 300
            };

            var ending = endgame.EvaluateEnding(ctx, profile, "branch_military_command_citadel");
            Assert.NotNull(ending);
            Assert.Equal("branch_military_command_citadel", ending.id);
            Assert.Equal("faction_branch", ending.category);
            Assert.Equal("garrison", ending.factions_alignment);
        }

        [Theory]
        [InlineData("ending_typo", "legacy_context", "close_rule")]
        [InlineData("", "legacy_context", "close_rule")]
        [InlineData("fixed_day", "faction_typo", "year_one_ending_source")]
        [InlineData("fixed_day", "", "year_one_ending_source")]
        public void InvalidBehaviorEnumsRejectCatalog(string closeRule, string source, string field)
        {
            var catalog = ChapterProfileCatalog.LoadFromJson(
                "{\"schema_version\":1,\"profiles\":[{\"profile_id\":\"profile_base_v1\","
                + "\"year_one_ending_source\":\"" + source + "\",\"close_rule\":\"" + closeRule + "\"}]}"
            );
            Assert.False(catalog.IsValid);
            Assert.Contains(catalog.ValidationErrors, e => e.Contains("unsupported " + field));
        }

        [Fact]
        public void EndgameSystem_SaveState_PreservesProfileId()
        {
            var endgame = new EndgameSystem();
            endgame.SetProfileId("profile_muster");

            var state = endgame.CaptureState();
            Assert.Equal("profile_muster", state.profileId);

            var restoredEndgame = new EndgameSystem();
            restoredEndgame.RestoreState(state);
            Assert.Equal("profile_muster", restoredEndgame.ProfileId);
        }
    }
}
