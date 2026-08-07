using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #865 — Twitch API: connect stub, poll/vote, cooldown, Capture/Restore, SaveSystem.
    /// </summary>
    [TestFixture]
    public class TwitchAPITests
    {
        private const float Eps = 1e-3f;

        [Test]
        public void ConnectOffline_SetsConnectedAndChannel()
        {
            var api = new System_TwitchAPI();
            Assert.IsFalse(api.IsConnected);

            string ch = null;
            api.OnConnected += c => ch = c;
            api.ConnectOffline("test_channel");

            Assert.IsTrue(api.IsConnected);
            Assert.AreEqual("test_channel", api.ChannelName);
            Assert.AreEqual("test_channel", ch);
            Assert.AreEqual("system_twitch_api", api.SystemId);

            api.Disconnect();
            Assert.IsFalse(api.IsConnected);
            Assert.AreEqual(0, api.ActivePollCount);
        }

        [Test]
        public void Connect_RejectsEmptyTokenOrChannel()
        {
            var api = new System_TwitchAPI();
            api.Connect("", System_TwitchAPI.OfflineToken);
            Assert.IsFalse(api.IsConnected);
            api.Connect("chan", "");
            Assert.IsFalse(api.IsConnected);
            api.Connect("chan", System_TwitchAPI.OfflineToken);
            Assert.IsTrue(api.IsConnected);
        }

        [Test]
        public void PollVote_TallyAndExecute_SpawnsWeatherAndCooldown()
        {
            var api = new System_TwitchAPI();
            api.ConnectOffline();

            string closedWinner = null;
            string spawned = null;
            api.OnPollClosed += (_, winner, __) => closedWinner = winner;
            api.OnEventSpawned += id => spawned = id;

            api.StartPoll("weather", new[]
            {
                System_TwitchAPI.OptBlizzard,
                System_TwitchAPI.OptHeatwave
            }, 60);

            Assert.AreEqual(1, api.ActivePollCount);

            api.ReceiveVote("viewer_a", "weather", System_TwitchAPI.OptBlizzard);
            api.ReceiveVote("viewer_b", "weather", System_TwitchAPI.OptBlizzard);
            api.ReceiveVote("viewer_c", "weather", System_TwitchAPI.OptHeatwave);

            Assert.AreEqual(0, api.TallyVotes("weather")); // blizzard wins

            api.ExecuteResult("weather");
            Assert.AreEqual(System_TwitchAPI.OptBlizzard, closedWinner);
            Assert.AreEqual("weather_blizzard", spawned);
            Assert.AreEqual(0, api.ActivePollCount);
            Assert.AreEqual(System_TwitchAPI.EventCooldownSeconds, api.CooldownSeconds, Eps);

            // Cooldown blocks new polls.
            api.StartPoll("weather2", new[] { System_TwitchAPI.OptRaid }, 10);
            Assert.AreEqual(0, api.ActivePollCount);
        }

        [Test]
        public void TickSecond_AutoClosesPoll_AndDecaysCooldown()
        {
            var api = new System_TwitchAPI();
            api.ConnectOffline();

            string supplyId = null;
            api.OnSupplyDrop += id => supplyId = id;

            api.StartPoll("supply", new[] { System_TwitchAPI.OptSupply, "pass" }, 2);
            api.ReceiveVote("v1", "supply", System_TwitchAPI.OptSupply);

            api.TickSecond(); // remaining 1
            Assert.AreEqual(1, api.ActivePollCount);
            api.TickSecond(); // remaining 0 → ExecuteResult
            Assert.AreEqual(0, api.ActivePollCount);
            Assert.IsNotNull(supplyId);
            Assert.IsTrue(supplyId.Contains("crate"));
            Assert.Greater(api.CooldownSeconds, 0f);

            float before = api.CooldownSeconds;
            api.TickSecond();
            Assert.AreEqual(before - 1f, api.CooldownSeconds, Eps);
        }

        [Test]
        public void ProcessChatCommand_Raid_VotesAndHostCanOpenWindow()
        {
            var api = new System_TwitchAPI();
            api.ConnectOffline();

            string spawned = null;
            api.OnEventSpawned += id => spawned = id;

            Assert.IsTrue(api.ProcessChatCommand("mod_viewer", "!raid"));
            Assert.AreEqual(1, api.ActivePollCount);

            // Force close immediately.
            api.ExecuteResult("raid");
            Assert.AreEqual(System_TwitchAPI.OptRaid, spawned ?? "");
            // Host pattern (mirrors GameBootstrap.WireTwitchAPI):
            // HatchDefenseSystem?.OpenRaidWindow() — offline-safe side effect.
        }

        [Test]
        public void HostPattern_WeatherEvent_ForcesBlizzard()
        {
            // Mirrors GameBootstrap.WireTwitchAPI weather branch.
            var weather = new WeatherSystem(null, 3);
            weather.ForceWeather(WeatherKind.Clear);
            Assert.AreEqual(WeatherKind.Clear, weather.Current);

            var api = new System_TwitchAPI();
            api.ConnectOffline();
            api.OnEventSpawned += eventId =>
            {
                if (eventId == "weather_blizzard")
                    weather.ForceWeather(WeatherKind.Blizzard);
                else if (eventId == "weather_heatwave")
                    weather.ForceWeather(WeatherKind.Ashfall);
            };

            api.StartPoll("weather", new[] { System_TwitchAPI.OptBlizzard, System_TwitchAPI.OptHeatwave }, 5);
            api.ReceiveVote("chat", "weather", System_TwitchAPI.OptBlizzard);
            api.ExecuteResult("weather");

            Assert.AreEqual(WeatherKind.Blizzard, weather.Current);
        }

        [Test]
        public void CaptureRestore_PreservesConnectionPollsAndCooldown()
        {
            var a = new System_TwitchAPI();
            a.ConnectOffline("save_chan");
            a.SetViewerCount(42);
            a.StartPoll("weather", new[] { System_TwitchAPI.OptBlizzard, System_TwitchAPI.OptHeatwave }, 30);
            a.ReceiveVote("v", "weather", System_TwitchAPI.OptHeatwave);

            var save = a.CaptureState();
            Assert.AreEqual("system_twitch_api", save.system_id);
            Assert.IsTrue(save.is_connected);
            Assert.AreEqual("save_chan", save.channel_name);
            Assert.AreEqual(42, save.viewer_count);
            Assert.AreEqual(1, save.active_polls.Count);
            Assert.AreEqual(1, save.active_polls[0].total_votes);

            // Mutate after capture.
            a.Disconnect();
            Assert.IsTrue(save.is_connected);
            Assert.AreEqual(1, save.active_polls.Count);

            var b = new System_TwitchAPI();
            b.RestoreState(save);
            Assert.IsTrue(b.IsConnected);
            Assert.AreEqual("save_chan", b.ChannelName);
            Assert.AreEqual(42, b.ViewerCount);
            Assert.AreEqual(1, b.ActivePollCount);
            Assert.AreEqual(1, b.GetActivePolls()[0].total_votes);
            Assert.AreEqual(System_TwitchAPI.OptHeatwave, b.GetActivePolls()[0].options[1]);

            b.RestoreState(null);
            Assert.IsFalse(b.IsConnected);
            Assert.AreEqual(0, b.ActivePollCount);
        }

        [Test]
        public void SaveSystemAdapter_TwitchApiSlot_RoundTrip()
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_twitch_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var weather = new WeatherSystem(null, 3);
                var temp = new TemperatureSystem(null, weather);
                var rad = new RadiationSystem(needs);

                var apiA = new System_TwitchAPI();
                apiA.ConnectOffline("round_trip");
                apiA.SetViewerCount(7);
                apiA.StartPoll("supply", new[] { System_TwitchAPI.OptSupply, "pass" }, 40);
                apiA.ReceiveVote("viewer", "supply", System_TwitchAPI.OptSupply);

                SaveSystem Make(System_TwitchAPI api)
                {
                    var ss = new SaveSystem(new SaveSystem.CoreDeps
                    {
                        GameState = new GameState(),
                        WeatherSystem = weather,
                        TemperatureSystem = temp,
                        NeedsSystem = needs,
                        RadiationSystem = rad,
                        Shelter = new ShelterClass(),
                        GetSurvivors = () => new List<Survivor>(),
                        ItemLookup = id => null,
                        ModuleLookup = id => null,
                        SavesDir = dir
                    });
                    ss.SetTwitchApiSystem(api);
                    return ss;
                }

                Assert.IsTrue(Make(apiA).Save("twitch_slot"));

                var apiB = new System_TwitchAPI();
                Assert.IsTrue(Make(apiB).Load("twitch_slot"));

                Assert.IsTrue(apiB.IsConnected);
                Assert.AreEqual("round_trip", apiB.ChannelName);
                Assert.AreEqual(7, apiB.ViewerCount);
                Assert.AreEqual(1, apiB.ActivePollCount);
                Assert.AreEqual("supply", apiB.GetActivePolls()[0].poll_id);
                Assert.AreEqual(1, apiB.GetActivePolls()[0].total_votes);
                Assert.AreEqual("system_twitch_api", apiB.SystemId);

                UnityEngine.Object.DestroyImmediate(profile);
            }
            finally
            {
                try { if (Directory.Exists(dir)) Directory.Delete(dir, true); } catch { /* best-effort */ }
            }
        }
    }
}
