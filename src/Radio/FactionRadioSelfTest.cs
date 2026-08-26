using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Radio;
using AtomicWar.GodotApp.Radio;

namespace AtomicWar.GodotApp
{
    public class MockFactionRadioProvider : IFactionRadioProvider
    {
        public List<string> Factions = new() { "faction_alpha", "faction_bravo" };
        public Dictionary<string, float> Frequencies = new() { { "faction_alpha", 99.5f }, { "faction_bravo", 102.1f } };
        public Dictionary<string, string> Callsigns = new() { { "faction_alpha", "ALPHA ACT" }, { "faction_bravo", "BRAVO CP" } };
        public RadioIntercept? NextIntercept;

        public RadioIntercept GetBroadcastAtFrequency(float frequencyMhz, int day, ISeededRng rng)
        {
            if (NextIntercept.HasValue) return NextIntercept.Value;
            return new RadioIntercept("none", "DEAD AIR", frequencyMhz, RadioEventKind.Silence, "Static...", 1, day);
        }

        public RadioIntercept GetFactionEvent(string factionId, RadioEventKind kind, int day, ISeededRng rng) => default;
        public string TryFindFactionAtFrequency(float frequencyMhz, float toleranceMhz = 1.5f) => null;
        public float GetFactionFrequency(string factionId) => Frequencies.GetValueOrDefault(factionId, 100f);
        public string GetFactionCallsign(string factionId) => Callsigns.GetValueOrDefault(factionId, "UNKNOWN");
        public IReadOnlyList<string> GetAllFactions() => Factions;
    }

    public class MockSeededRng : ISeededRng
    {
        public int Seed => 42;
        public int Next(int minInclusive, int maxExclusive) => minInclusive;
        public float NextFloat() => 0.5f;
        public double NextDouble() => 0.5;
    }

    public static class FactionRadioSelfTest
    {
        public static int Run()
        {
            int passed = 0;
            int total = 0;

            void Check(bool condition, string name)
            {
                total++;
                if (condition)
                {
                    passed++;
                    GD.Print($"  [PASS] {name}");
                }
                else
                {
                    GD.Print($"  [FAIL] {name}");
                }
            }

            GD.Print("[FactionRadioSelfTest] begin");

            try
            {
                var panel = new FactionRadioHudPanel();
                panel._Ready();

                var provider = new MockFactionRadioProvider();
                provider.NextIntercept = new RadioIntercept("faction_alpha", "ALPHA ACT", 99.5f, RadioEventKind.InterceptChatter, "Testing bind.", 8, 1);
                var rng = new MockSeededRng();

                panel.BindProvider(provider, rng, 1);

                Check(panel.LogCount == 1, "LogCount reflects the first intercept after bind.");
                Check(panel.TunedFrequency == 88.4f, "Initial tuned frequency is kept if not explicitly changed before bind.");

                panel.TuneToFrequency(99.5f);
                Check(panel.TunedFrequency == 99.5f, "TuneToFrequency updates the tuned frequency.");
                Check(panel.LogCount == 2, "TuneToFrequency triggers another intercept and updates log.");

                panel.QueueFree();
            }
            catch (Exception ex)
            {
                GD.Print($"  [FAIL] Exception during test: {ex.Message}");
                Check(false, "No exceptions");
            }

            bool ok = passed == total;
            GD.Print($"[FactionRadioSelfTest] result: {passed}/{total} PASS, FAIL count {total - passed}");
            GD.Print(ok ? "FACTION_RADIO_SELFTEST PASS" : "FACTION_RADIO_SELFTEST FAIL");
            return ok ? 0 : 1;
        }
    }
}
