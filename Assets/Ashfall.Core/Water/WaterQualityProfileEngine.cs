// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Water
{
    public enum WaterSourcePurityTier
    {
        RawSurfaceRunoff = 1,
        SumpBrine = 2,
        DeepAquifer = 3,
        CharcoalFiltered = 4,
        ReverseOsmosis = 5,
        DistilledPotable = 6,
    }

    public readonly struct WaterContaminantProfile
    {
        public int ParticulatePpm { get; }
        public int HeavyMetalsPpm { get; }
        public int RadIsotopesBqL { get; }
        public int BioPathogensCfu { get; }

        public WaterContaminantProfile(int particulatePpm, int heavyMetalsPpm, int radIsotopesBqL, int bioPathogensCfu)
        {
            ParticulatePpm = Math.Max(0, particulatePpm);
            HeavyMetalsPpm = Math.Max(0, heavyMetalsPpm);
            RadIsotopesBqL = Math.Max(0, radIsotopesBqL);
            BioPathogensCfu = Math.Max(0, bioPathogensCfu);
        }
    }

    public readonly struct WaterTreatmentYield
    {
        public int YieldFractionPermille { get; }
        public int FilterWearPermille { get; }
        public WaterSourcePurityTier ResultingPurityTier { get; }
        public int PathogenRiskPermille { get; }
        public int HeavyMetalRiskPermille { get; }

        public WaterTreatmentYield(
            int yieldFractionPermille,
            int filterWearPermille,
            WaterSourcePurityTier resultingPurityTier,
            int pathogenRiskPermille,
            int heavyMetalRiskPermille)
        {
            YieldFractionPermille = Math.Max(0, Math.Min(1000, yieldFractionPermille));
            FilterWearPermille = Math.Max(0, filterWearPermille);
            ResultingPurityTier = resultingPurityTier;
            PathogenRiskPermille = Math.Max(0, Math.Min(1000, pathogenRiskPermille));
            HeavyMetalRiskPermille = Math.Max(0, Math.Min(1000, heavyMetalRiskPermille));
        }
    }

    /// <summary>
    /// Expansion 22: Clean Flow — Water Treatment Quality & Source Profile Engine.
    /// Pure domain engine modeling water source contamination profiles, filtration yields,
    /// and pathogen/heavy-metal health risks for shelter consumption without unseeded RNG or parallel stores.
    /// </summary>
    public static class WaterQualityProfileEngine
    {
        public static WaterContaminantProfile EvaluateSourceContamination(
            WaterSourcePurityTier tier,
            int floodContaminationPermille = 0)
        {
            int particulate;
            int heavyMetals;
            int radIsotopes;
            int bioPathogens;

            switch (tier)
            {
                case WaterSourcePurityTier.RawSurfaceRunoff:
                    particulate = 450;
                    heavyMetals = 80;
                    radIsotopes = 120;
                    bioPathogens = 650;
                    break;

                case WaterSourcePurityTier.SumpBrine:
                    particulate = 600;
                    heavyMetals = 150;
                    radIsotopes = 200;
                    bioPathogens = 400;
                    break;

                case WaterSourcePurityTier.DeepAquifer:
                    particulate = 50;
                    heavyMetals = 30;
                    radIsotopes = 10;
                    bioPathogens = 20;
                    break;

                case WaterSourcePurityTier.CharcoalFiltered:
                    particulate = 40;
                    heavyMetals = 20;
                    radIsotopes = 50;
                    bioPathogens = 150;
                    break;

                case WaterSourcePurityTier.ReverseOsmosis:
                    particulate = 5;
                    heavyMetals = 5;
                    radIsotopes = 15;
                    bioPathogens = 10;
                    break;

                case WaterSourcePurityTier.DistilledPotable:
                default:
                    particulate = 0;
                    heavyMetals = 0;
                    radIsotopes = 0;
                    bioPathogens = 0;
                    break;
            }

            // Flooding exacerbates contaminants
            if (floodContaminationPermille > 0)
            {
                int floodFactor = 1000 + floodContaminationPermille;
                particulate = (particulate * floodFactor) / 1000;
                bioPathogens = (bioPathogens * floodFactor) / 1000;
            }

            return new WaterContaminantProfile(particulate, heavyMetals, radIsotopes, bioPathogens);
        }

        public static WaterTreatmentYield CalculateTreatmentYield(
            WaterSourcePurityTier sourceTier,
            TreatmentMode mode,
            int filterIntegrityPermille = 1000)
        {
            int yieldPermille;
            int wearPermille;
            WaterSourcePurityTier resultingTier;
            int pathogenRisk;
            int heavyMetalRisk;

            switch (mode)
            {
                case TreatmentMode.CharcoalFiltration:
                    yieldPermille = 950;
                    wearPermille = 25;
                    resultingTier = WaterSourcePurityTier.CharcoalFiltered;
                    pathogenRisk = filterIntegrityPermille > 500 ? 120 : 350;
                    heavyMetalRisk = 80;
                    break;

                case TreatmentMode.ReverseOsmosis:
                    yieldPermille = 800; // 20% brine reject
                    wearPermille = 40;
                    resultingTier = WaterSourcePurityTier.ReverseOsmosis;
                    pathogenRisk = filterIntegrityPermille > 500 ? 10 : 150;
                    heavyMetalRisk = 15;
                    break;

                case TreatmentMode.Distillation:
                    yieldPermille = 700; // fuel/evaporation loss
                    wearPermille = 10;
                    resultingTier = WaterSourcePurityTier.DistilledPotable;
                    pathogenRisk = 0;
                    heavyMetalRisk = 5;
                    break;

                case TreatmentMode.Decontamination:
                    yieldPermille = 850;
                    wearPermille = 50;
                    resultingTier = WaterSourcePurityTier.ReverseOsmosis;
                    pathogenRisk = 20;
                    heavyMetalRisk = 20;
                    break;

                case TreatmentMode.Idle:
                default:
                    yieldPermille = 1000;
                    wearPermille = 0;
                    resultingTier = sourceTier;
                    pathogenRisk = 500;
                    heavyMetalRisk = 200;
                    break;
            }

            return new WaterTreatmentYield(
                yieldPermille,
                wearPermille,
                resultingTier,
                pathogenRisk,
                heavyMetalRisk);
        }

        public static int CalculateHealthRiskPermille(WaterContaminantProfile profile)
        {
            // Weighted risk calculation in permille
            int risk = 0;
            risk += Math.Min(300, profile.BioPathogensCfu / 2);
            risk += Math.Min(300, profile.HeavyMetalsPpm * 2);
            risk += Math.Min(400, profile.RadIsotopesBqL * 2);

            return Math.Min(1000, risk);
        }
    }
}
