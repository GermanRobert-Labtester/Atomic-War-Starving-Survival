// GameBootstrap.TwitchAPI.cs — Prompt #865 offline-safe Twitch poll host hooks.
using AtomicWar._Game.Environment;
using UnityEngine;
using AtomicWar._Game.Utilities;
using Ashfall.Core;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Prompt #865 — construct Twitch integration (no network; chat simulated).
        /// </summary>
        private void BootTwitchAPI()
        {
            TwitchAPI = new System_TwitchAPI();
            WireTwitchAPI();
            GameLog.Log("[GameBootstrap] TwitchAPI ready (offline stub; connect via ConnectOffline).");
        }

        /// <summary>
        /// Prompt #865 — map poll outcomes to weather / hatch raid window / cistern supply.
        /// </summary>
        private void WireTwitchAPI()
        {
            if (TwitchAPI == null) return;

            TwitchAPI.OnConnected += channel =>
                GameLog.Log($"[GameBootstrap] TWITCH: connected to '{channel}'");
            TwitchAPI.OnDisconnected += () =>
                GameLog.Log("[GameBootstrap] TWITCH: disconnected");

            TwitchAPI.OnEventSpawned += eventId =>
            {
                if (string.IsNullOrEmpty(eventId)) return;

                if (eventId == "weather_blizzard")
                {
                    WeatherSystem?.ForceWeather(WeatherKind.Blizzard);
                    GameLog.Log("[GameBootstrap] TWITCH: viewers forced a blizzard.");
                }
                else if (eventId == "weather_heatwave")
                {
                    // No Heatwave weather kind — Ashfall is the harsh "hot ash" stand-in.
                    WeatherSystem?.ForceWeather(WeatherKind.Ashfall);
                    GameLog.Log("[GameBootstrap] TWITCH: viewers forced a heatwave (ashfall).");
                }
                else if (eventId == System_TwitchAPI.OptRaid)
                {
                    // Open bandage/raid window without forcing a full ResolveRaid (offline-safe).
                    HatchDefenseSystem?.OpenRaidWindow();
                    GameLog.Log("[GameBootstrap] TWITCH: viewers opened a raid window.");
                }
            };

            TwitchAPI.OnSupplyDrop += crateId =>
            {
                // Offline-safe: a few liters of clean water into the cistern.
                WaterStorage?.AddClean(5f);
                GameLog.Log($"[GameBootstrap] TWITCH: supply drop '{crateId}' → +5 L clean water.");
            };

            TwitchAPI.OnPollClosed += (pollId, winner, total) =>
                GameLog.Log($"[GameBootstrap] TWITCH: poll '{pollId}' closed → '{winner}' ({total} votes)");
        }

        /// <summary>
        /// Prompt #865 — real-time second ticks while connected (poll timers / cooldown).
        /// </summary>
        private void TickTwitchAPI(float unscaledDelta)
        {
            TwitchAPI?.Tick(unscaledDelta);
        }
    }
}
