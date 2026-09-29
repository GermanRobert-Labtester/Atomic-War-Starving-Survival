// SPDX-License-Identifier: MIT
// Plan 49 (C1[16]) depth passes — the four orphaned catalogs are bound
// through their existing Core loaders into constructed owners.
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Ashfall.Core.Tests.Content
{
    public sealed class Plan49DepthPassWiringTests
    {
        private static string RepoRoot()
        {
            var directory = new DirectoryInfo(Path.GetFullPath(AppContext.BaseDirectory));
            while (directory != null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "src", "Main.Plan49DepthPass.cs")))
                    return directory.FullName;
                directory = directory.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        [Fact]
        public void CoreSeams_LoadAuthoredCatalogs_AndServeQueries()
        {
            string data = Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");
            var audio = new AudioConditionSystem();
            audio.LoadAudioLogCatalog(File.ReadAllText(Path.Combine(data, "audio_logs_expansion_05.json")));
            Assert.True(audio.AudioLogCount > 0);
            Assert.Contains("Meridian Compact", audio.GetAudioLogBody("audio_log_radio_message_day_35"));

            var memorials = new Ashfall.Core.Memorial.MemorialSystem(new Ashfall.Core.Memorial.MemorialState());
            memorials.LoadMemorialTexts(File.ReadAllText(Path.Combine(data, "memorials_expansion_05.json")));
            Assert.True(memorials.MemorialTextCount > 0);
            Assert.Contains("water flowing", memorials.GetMemorialText("memorial_marcus_olejnik"));

            var heirlooms = new Ashfall.Core.Phantoms.HeirloomCatalog();
            heirlooms.Load(File.ReadAllText(Path.Combine(data, "phantom_heirlooms.json")), new Ashfall.Core.SystemTextJsonSerializer());
            Assert.True(heirlooms.AllHeirlooms.Count > 0);
            Assert.NotNull(heirlooms.GetByBaseItemId("dosimeter"));
        }

        [Theory]
        [InlineData("audio_log_scavenger_meeting_day_65", "Let's make this work.")]
        [InlineData("audio_log_medical_update_day_95", "bring them to the medical bay immediately.")]
        [InlineData("audio_log_scavenger_ambush_day_112", "Now!")]
        [InlineData("audio_log_raider_attack_day_170", "We need to prepare for the next attack.")]
        [InlineData("audio_log_technology_experiment_day_180", "We don't know what this thing can do.")]
        [InlineData("audio_log_food_storage_theft_day_150", "before it causes a bigger problem.")]
        [InlineData("audio_log_raider_siege_day_240", "Raiders are still out there.")]
        [InlineData("audio_log_medical_training_day_160", "Don't miss this opportunity.")]
        [InlineData("audio_log_radio_signal_day_72", "We are real.")]
        [InlineData("audio_log_technology_sharing_day_220", "if you want to see the blueprints.")]
        [InlineData("audio_log_leadership_vote_day_105", "This is your chance to have a say in our future.")]
        [InlineData("audio_log_survivor_disappearance_day_140", "before it's too late.")]
        [InlineData("audio_log_survivor_confession_day_88", "No parent should watch their child starve.")]
        [InlineData("audio_log_art_project_day_210", "bring some color back into our lives.")]
        [InlineData("audio_log_black_flotilla_trade_day_130", "if you want to see the devices.")]
        [InlineData("audio_log_survivor_diary_day_50", "I'm going to try to make contact tomorrow.")]
        [InlineData("audio_log_survivor_exile_day_190", "We should have helped them.")]
        [InlineData("audio_log_survivor_romance_day_250", "meet me in the common area after dinner.")]
        [InlineData("audio_log_radiation_storm_day_120", "We'll get through this together.")]
        [InlineData("audio_log_fuel_crisis_day_260", "meet me in the workshop tomorrow.")]
        [InlineData("audio_log_technology_breakthrough_day_230", "Come see it in the workshop.")]
        [InlineData("audio_log_power_crisis_day_280", "meet me in the generator room immediately.")]
        [InlineData("audio_log_black_flotilla_offer_day_80", "We don't wait for anyone.")]
        [InlineData("audio_log_winter_preparations_day_290", "Meet in the common area tomorrow.")]
        [InlineData("audio_log_medical_alert_day_58", "mandatory evacuation of all non-essential areas.")]
        [InlineData("audio_log_scavenger_radio_day_42", "This is between us. Over.")]
        [InlineData("audio_log_fuel_expedition_day_270", "Keep your fingers crossed.")]
        [InlineData("audio_log_memory_loss_day_200", "but I don't know how to help either.")]
        public void AudioLogListeningNotes_AreAdditive_AndKeepSourceBody(string id, string bodyTail)
        {
            string data = Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");
            var audio = new AudioConditionSystem();
            audio.LoadAudioLogCatalog(File.ReadAllText(Path.Combine(data, "audio_logs_expansion_05.json")));
            Assert.EndsWith(bodyTail, audio.GetAudioLogBody(id));
            Assert.False(string.IsNullOrWhiteSpace(audio.GetAudioLogListeningNote(id)));
            Assert.Null(audio.GetAudioLogListeningNote("audio_log_radio_message_day_35"));
        }

        [Theory]
        [InlineData("radio_d480_span44_automated_loop", "Remain in shelter.")]
        [InlineData("radio_d481_garrison_continuity_bulletin", "at the Continuity Office's discretion.")]
        [InlineData("radio_d484_exchange_roster_wire_rebuttal", "Come count them yourselves.")]
        [InlineData("radio_d487_unsigned_supply_figures", "on that board.")]
        [InlineData("radio_d488_garrison_grain_rebuttal", "with static.")]
        [InlineData("radio_d490_ash_sign_shrine_transmission", "hands that bleed.")]
        [InlineData("radio_d496_understory_clean_strike", "That's the whole arrangement.")]
        [InlineData("radio_d504_garrison_conscription_notice", "effective immediately.")]
        [InlineData("radio_d507_exchange_roster_wire_conscription", "Just don't expect a full counter.")]
        [InlineData("radio_d516_ash_sign_warning", "chooses what it chooses.")]
        [InlineData("radio_d518_garrison_almshouse_bulletin", "at this time.")]
        [InlineData("radio_d522_ash_sign_reading_shift", "We are watching too.")]
        [InlineData("radio_d542_understory_something_coming", "maybe be there.")]
        [InlineData("radio_d546_garrison_plaza_communique", "by the responsible party.")]
        [InlineData("radio_d510_understory_span44_standoff", "briefly, listened.")]
        [InlineData("radio_d525_exchange_roster_wire_prices", "than guessed at it.")]
        [InlineData("radio_d534_garrison_exchange_order", "an occupation.")]
        [InlineData("radio_d547_rebuilders_plaza_communique", "own weight.")]
        [InlineData("radio_d559_lima_november_burst", "LN74 out.")]
        [InlineData("radio_d579_ash_sign_shrine_anomaly", "what the numbers are actually saying.")]
        public void FactionWarRadioListeningNotes_AreAdditive_AndKeepMessage(string id, string messageTail)
        {
            string data = Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");
            var catalog = new Ashfall.Core.Radio.RadioBroadcastCatalog();
            catalog.LoadFactionWarRadioJson(File.ReadAllText(Path.Combine(data, "faction_war_radio.json")));
            var b = catalog.GetById(id);
            Assert.NotNull(b);
            Assert.EndsWith(messageTail, b!.Message);
            Assert.False(string.IsNullOrWhiteSpace(b.ListeningNote));
        }

        [Fact]
        public void AudioLogs_AreDated_AndServedPerDay_InStableOrder()
        {
            string data = Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");
            var audio = new AudioConditionSystem();
            audio.LoadAudioLogCatalog(File.ReadAllText(Path.Combine(data, "audio_logs_expansion_05.json")));

            var day35 = audio.GetAudioLogsForDay(35);
            Assert.Contains(day35, l => l.id == "audio_log_radio_message_day_35");
            Assert.All(day35, l => Assert.Equal(35, l.day));
            Assert.Empty(audio.GetAudioLogsForDay(0));
            Assert.Empty(audio.GetAudioLogsForDay(-3));
            Assert.Equal(day35.Select(l => l.id).OrderBy(x => x, StringComparer.Ordinal), day35.Select(l => l.id));
        }

        [Fact]
        public void AudioLogs_ReachThePlayerThroughTheJournal_OnTheirDay()
        {
            string root = RepoRoot();
            string main = File.ReadAllText(Path.Combine(root, "src", "Main.Plan49DepthPass.cs"));
            string owners = File.ReadAllText(Path.Combine(root, "src", "Main.CampaignOwners.cs"));
            Assert.Contains("_depthPass.GetAudioLogsForDay(day)", main);
            Assert.Contains("_journal.TryAddRawEntry(\"audio_log_\" + log.id", main);
            Assert.Contains("log.listening_note", main);
            Assert.Contains("_campaignDay.Register(\"audio_logs\", new AudioLogsDayOwner(this)", owners);
        }

        [Fact]
        public void HostWiring_BindsAllFourCatalogs_AtSetup_NoSaveSection()
        {
            string host = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Host", "Plan49DepthPassHostSession.cs"));
            string main = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.Plan49DepthPass.cs"));
            string orchestrator = File.ReadAllText(Path.Combine(RepoRoot(), "src", "Main.SaveOrchestrator.cs"));
            string registry = File.ReadAllText(Path.Combine(RepoRoot(), "Assets", "Ashfall.Core", "Save", "SaveSectionRegistry.cs"));

            Assert.Contains("Heirlooms.Load(", host);
            Assert.Contains("TradeScreenScenarioLoader.LoadFromJson(", host);
            Assert.Contains("LoadAudioLogCatalog(", host);
            Assert.Contains("LoadMemorialTexts(", host);
            Assert.Contains("SetupPlan49DepthPass();", orchestrator);
            Assert.Contains("Plan49DepthPassHostSession.Create(_dataDir)", main);
            Assert.DoesNotContain("\"plan49_depth\"", registry); // authored data, no new save section
        }
    }
}
