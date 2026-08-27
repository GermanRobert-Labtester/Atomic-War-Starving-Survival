using System;
using System.Collections.Generic;

using Ashfall.Core.IO;
namespace Ashfall.Core
{
    /// <summary>One dose-band vocabulary row (dose_registers.json).</summary>
    public class DoseBandDef
    {
        public string id = string.Empty;
        public string label = string.Empty;
        public float threshold_msv;
        public string disposition = string.Empty;
    }

    /// <summary>One palliative-plan vocabulary row.</summary>
    public class DosePlanDef
    {
        public string id = string.Empty;
        public string label = string.Empty;
        public string cost = string.Empty;
        public string note = string.Empty;
    }

    /// <summary>One cohort guess vocabulary row.</summary>
    public class DoseGuessDef
    {
        public string id = string.Empty;
        public string label = string.Empty;
        public bool pencil;
        public string note = string.Empty;
    }

    /// <summary>One chaired antagonist row (PART B: the four accountants).</summary>
    public class DoseRegisterNpcDef
    {
        public string id = string.Empty;
        public string name = string.Empty;
        public string register = string.Empty;
        public string disposition = string.Empty;
        public string action_label = string.Empty;
        public string action = string.Empty;
    }

    /// <summary>The dose_registers.json vocabulary (A4) — display strings only,
    /// so the host never hardcodes band/plan/guess text.</summary>
    public class DoseRegistersCatalog
    {
        public List<DoseBandDef> bands = new List<DoseBandDef>();
        public List<DosePlanDef> plans = new List<DosePlanDef>();
        public List<DoseGuessDef> guesses = new List<DoseGuessDef>();
        public List<DoseRegisterNpcDef> npcs = new List<DoseRegisterNpcDef>();
    }

    /// <summary>Engine-agnostic loader for dose_registers.json.</summary>
    public static class DoseRegistersCatalogLoader
    {
        public const string FileName = "dose_registers.json";

        public static DoseRegistersCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var catalog = new DoseRegistersCatalog();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return catalog;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return catalog;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return catalog;

            try
            {
                var parsed = json.Deserialize<DoseRegistersCatalog>(raw);
                if (parsed != null)
                {
                    if (parsed.bands != null) catalog.bands = parsed.bands;
                    if (parsed.plans != null) catalog.plans = parsed.plans;
                    if (parsed.guesses != null) catalog.guesses = parsed.guesses;
                    if (parsed.npcs != null) catalog.npcs = parsed.npcs;
                }
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "DoseRegistersCatalog", ex_CATDIAG);
                return catalog;
            }
            return catalog;
        }

        public static string BandLabel(DoseRegistersCatalog catalog, int band)
        {
            for (int i = 0; i < catalog.bands.Count; i++)
                if (catalog.bands[i].id == BandIdFor(band))
                    return catalog.bands[i].label;
            return "Band " + band;
        }

        public static string BandIdFor(int band)
        {
            switch (band)
            {
                case 0: return "band_green";
                case 1: return "band_amber";
                case 2: return "band_red";
                case 3: return "band_black";
                default: return "band_green";
            }
        }
    }
}
