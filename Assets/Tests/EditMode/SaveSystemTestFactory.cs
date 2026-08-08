using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// qlty flagged this exact SaveSystem.CoreDeps scaffold (NeedsProfile/NeedsSystem/
    /// WeatherSystem/TemperatureSystem/RadiationSystem + a throwaway temp save dir) as
    /// duplicated near-verbatim across 20+ wiring/round-trip test files. Centralized here
    /// so new save-slot tests don't need to hand-roll it again.
    /// </summary>
    public static class SaveSystemTestFactory
    {
        public static string TempDir(string tag)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_" + tag + "_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        public static SaveSystem MakeSave(string dir, Action<SaveSystem> wire = null)
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var weather = new WeatherSystem(null, 3);
            var temp = new TemperatureSystem(null, weather);
            var rad = new RadiationSystem(needs);
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
            wire?.Invoke(ss);
            return ss;
        }
    }
}
