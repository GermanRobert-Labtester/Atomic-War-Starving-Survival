// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : ChemicalPlumeHostSession
// Purpose      : PLAN-CHEMICAL-RECON-TRUTH-183 — host the chemical plume
//                dispersion authority. Owns plume state, advances dispersion
//                from the canonical weather vector, and evaluates shelter-air
//                and respirator protection. Persists under `chemical_plume`.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.Combat;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public sealed class ChemicalPlumeSaveState
    {
        public int schema_version { get; set; } = 1;
        public List<ChemicalPlumeState> plumes { get; set; } = new();
    }

    public static class ChemicalPlumeSaveStore
    {
        public const string FileName = "chemical_plume_save.json";
        public const string SectionName = "chemical_plume";
        private static readonly SaveStore<ChemicalPlumeSaveState> s_store =
            SaveStoreHub.Checksummed<ChemicalPlumeSaveState>(FileName, nameof(ChemicalPlumeSaveStore));
        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static string TryCapturePersisted(ChemicalPlumeSaveState state) => s_store.CaptureBare(state);
        public static ChemicalPlumeSaveState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(ChemicalPlumeSaveState state) => s_store.TrySave(state);
        public static ChemicalPlumeSaveState? TryLoad() => s_store.TryLoad();
    }

    public sealed class ChemicalPlumeHostSession : HostSessionBase
    {
        public List<ChemicalPlumeState> Plumes { get; } = new();
        public Func<WeatherDispersionVector>? WeatherProvider { get; set; }
        public string LastEvent { get; private set; } = string.Empty;

        public int ActivePlumeCount
        {
            get
            {
                int n = 0;
                for (int i = 0; i < Plumes.Count; i++)
                    if (Plumes[i].DensityPermille > 0 && Plumes[i].RemainingLifespanTicks > 0) n++;
                return n;
            }
        }

        public ChemicalPlumeState Spawn(
            string plumeId, string agentId, int sectorX, int sectorY,
            int densityPermille = 500, int lifespanTicks = 24,
            PlumeToxicityTier tier = PlumeToxicityTier.Elevated)
        {
            var plume = new ChemicalPlumeState
            {
                PlumeId = plumeId,
                AgentId = agentId,
                SectorX = sectorX,
                SectorY = sectorY,
                DensityPermille = Math.Clamp(densityPermille, 0, ChemicalPlumeDispersionEngine.MaxDensityPermille),
                RemainingLifespanTicks = Math.Max(0, lifespanTicks),
                ToxicityTier = tier
            };
            Plumes.Add(plume);
            LastEvent = $"Plume '{plumeId}' released in sector {sectorX},{sectorY}.";
            RaiseStateChanged();
            return plume;
        }

        public void AdvanceDay()
        {
            var weather = WeatherProvider?.Invoke() ?? new WeatherDispersionVector(0, 0, 0);
            for (int i = 0; i < Plumes.Count; i++)
                ChemicalPlumeDispersionEngine.AdvancePlumeDispersion(Plumes[i], weather);
            Plumes.RemoveAll(p => p.DensityPermille <= 0 || p.RemainingLifespanTicks <= 0);
            RaiseStateChanged();
        }

        public ShelterAirQualityResult EvaluateShelterAir(bool filtrationPowered, int filterConditionPermille)
        {
            int outdoor = MaxOutdoorDensity(out PlumeToxicityTier tier);
            return ChemicalPlumeDispersionEngine.EvaluateShelterAirInfiltration(outdoor, tier, filtrationPowered, filterConditionPermille);
        }

        public RespiratorProtectionResult EvaluateRespirator(int ambientDensityPermille, PlumeToxicityTier tier, int canisterConditionPermille)
            => ChemicalPlumeDispersionEngine.EvaluateRespiratorProtection(ambientDensityPermille, tier, canisterConditionPermille);

        private int MaxOutdoorDensity(out PlumeToxicityTier tier)
        {
            int best = 0; tier = PlumeToxicityTier.Trace;
            for (int i = 0; i < Plumes.Count; i++)
            {
                if (Plumes[i].DensityPermille > best)
                {
                    best = Plumes[i].DensityPermille;
                    tier = Plumes[i].ToxicityTier;
                }
            }
            return best;
        }

        public ChemicalPlumeSaveState CaptureState() => new ChemicalPlumeSaveState { plumes = new List<ChemicalPlumeState>(Plumes) };

        public void RestoreState(ChemicalPlumeSaveState? state)
        {
            Plumes.Clear();
            if (state?.plumes != null) Plumes.AddRange(state.plumes);
            RaiseStateChanged();
        }
    }
}
