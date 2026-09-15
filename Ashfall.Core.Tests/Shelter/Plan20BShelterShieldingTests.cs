// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Plan20BShielding
{
    /// <summary>
    /// C2 / Plan 20B — shelter shielding model + catalog. Nominal (healthy,
    /// dry, unclogged) inputs must reproduce the legacy interior math exactly;
    /// each contributor must move the interior rate in the authored direction,
    /// bounded by the data-authored caps. No UI or read model may recompute
    /// this (plan §3.3/§22.3).
    /// </summary>
    public sealed class ShelterShieldingModelTests
    {
        private static ShelterShieldingModel Nominal(float attenuation, float baseline = 2.0f)
        {
            return new ShelterShieldingModel
            {
                StructuralAttenuationProvider = () => attenuation
            };
        }

        [Fact]
        public void NominalInputs_ReproduceLegacyInteriorMath()
        {
            // Legacy: interior = zone × (1 − attenuation).
            Assert.Equal(1.2f, Nominal(0.4f).ComputeInteriorRad(2.0f), 3);
            Assert.Equal(0f, Nominal(1.0f).ComputeInteriorRad(2.0f), 3);
            Assert.Equal(2.0f, Nominal(0f).ComputeInteriorRad(2.0f), 3);
        }

        [Fact]
        public void UnboundModel_IsNominalForAnyBaseline()
        {
            var model = new ShelterShieldingModel();
            Assert.Equal(2.0f, model.ComputeInteriorRad(2.0f), 3); // attenuation 0 → full bleed
        }

        [Fact]
        public void CloggedFilter_RaisesInteriorRate()
        {
            var model = Nominal(0.5f);
            float clean = model.ComputeInteriorRad(2.0f); // 1.0
            model.FilterHealthPercentProvider = () => 0f;
            float clogged = model.ComputeInteriorRad(2.0f);
            Assert.Equal(1.0f, clean, 3);
            Assert.Equal(1.0f + 1.0f * 0.6f, clogged, 3); // penalty 0.6 → ×1.6
        }

        [Fact]
        public void VentilationDefects_RaiseInteriorRate_CappedByAuthoredMultiplier()
        {
            var model = Nominal(0.5f);
            model.VentilationDuctIntegrityProvider = () => 0f;
            model.VentilationFilterSaturationProvider = () => 100f;
            model.VentilationRecirculationProvider = () => true;
            model.AirlockSealProvider = () => 0f;
            model.AirlockIncidentProvider = () => true;
            model.FilterHealthPercentProvider = () => 0f;
            float interior = model.ComputeInteriorRad(2.0f);
            // Raw penalty sum 0.6+0.4+0.3+0.25+0.8+0.4 = 2.75 → ingress 3.75;
            // interior 1.0 × 3.75 = 3.75 (below the 5× cap and 25 cap).
            Assert.Equal(3.75f, interior, 2);
        }

        [Fact]
        public void IngressMultiplier_IsCapped()
        {
            var config = new ShelterShieldingCatalog
            {
                filter_clog_max_ingress_penalty = 5f,
                max_ingress_multiplier = 2f
            };
            var model = new ShelterShieldingModel(config)
            {
                StructuralAttenuationProvider = () => 0.5f,
                FilterHealthPercentProvider = () => 0f
            };
            Assert.Equal(2.0f, model.ComputeInteriorRad(2.0f), 3); // 1.0 × cap 2
        }

        [Fact]
        public void Weather_AmplifiesExistingDefectsOnly()
        {
            var intact = Nominal(0.5f);
            intact.WeatherRadModifierProvider = () => 150f;
            Assert.Equal(1.0f, intact.ComputeInteriorRad(2.0f), 3); // no defects → no weather ingress

            var open = Nominal(0.5f);
            open.WeatherRadModifierProvider = () => 150f;
            open.AirlockSealProvider = () => 0f; // penalty 0.8
            // amplified: 0.8 × (1 + 0.01×150) = 2.0 → interior 1.0 × 3.0
            Assert.Equal(3.0f, open.ComputeInteriorRad(2.0f), 3);
        }

        [Fact]
        public void Radon_AddsInternalFloor_IndependentOfOutdoorShielding()
        {
            var model = Nominal(1.0f); // fully shielded: external 0
            model.IndoorRadonProvider = () => 120f;
            Assert.Equal(0.6f, model.ComputeInteriorRad(2.0f), 3); // 120 × 0.005
        }

        [Fact]
        public void FloodingAndShelterContamination_Contribute()
        {
            var model = Nominal(1.0f);
            model.FloodingContaminationProvider = () => 0.5f;   // × 4.0 → 2.0
            model.ShelterContaminationProvider = () => 0.1f;   // × 8.0 → 0.8
            Assert.Equal(2.8f, model.ComputeInteriorRad(2.0f), 2);
        }

        [Fact]
        public void DeconActive_ReducesInternalSourcesOnly()
        {
            var model = Nominal(0.5f); // external 1.0
            model.ShelterContaminationProvider = () => 0.1f; // internal 0.8
            float before = model.ComputeInteriorRad(2.0f);
            model.DeconActiveProvider = () => true;
            float active = model.ComputeInteriorRad(2.0f);
            Assert.Equal(1.8f, before, 2);
            Assert.Equal(1.0f + 0.8f * 0.5f, active, 2); // internal halved; external untouched
        }

        [Fact]
        public void InteriorRate_IsClampedToAuthoredMaximum()
        {
            var config = new ShelterShieldingCatalog { max_interior_rad_rate = 5f };
            var model = new ShelterShieldingModel(config)
            {
                StructuralAttenuationProvider = () => 0f,
                IndoorRadonProvider = () => 5000f // 25 raw
            };
            Assert.Equal(5f, model.ComputeInteriorRad(2.0f), 3);
        }

        [Fact]
        public void MissingOrInvalidProviderValues_DoNotProduceNaN()
        {
            var model = Nominal(0.5f);
            model.FilterHealthPercentProvider = () => float.NaN;
            model.IndoorRadonProvider = () => float.NaN;
            model.WeatherRadModifierProvider = () => float.NaN;
            model.AirlockSealProvider = () => float.NaN;
            float interior = model.ComputeInteriorRad(float.NaN);
            Assert.False(float.IsNaN(interior));
            Assert.Equal(0f, interior, 3);
        }

        [Fact]
        public void Compute_IsPure_RepeatedCallsIdentical()
        {
            var model = Nominal(0.4f);
            model.FilterHealthPercentProvider = () => 60f;
            model.IndoorRadonProvider = () => 120f;
            Assert.Equal(model.ComputeInteriorRad(2.0f), model.ComputeInteriorRad(2.0f));
        }
    }

    /// <summary>Catalog loading + integrity validation (plan §27.1).</summary>
    public sealed class ShelterShieldingCatalogTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        [Fact]
        public void RealData_LoadsCleanly()
        {
            var catalog = ShelterShieldingCatalog.LoadFromDirectory(GetDataDir(), new Ashfall.Core.FileSystemIO());
            Assert.True(catalog.IsValid, string.Join("; ", catalog.Errors));
            Assert.Equal(1, catalog.schema_version);
            Assert.Equal(2.0f, catalog.interior_baseline_rad_rate, 3);
        }

        [Fact]
        public void InvalidRanges_AreReported()
        {
            var errors = new List<string>();
            ShelterShieldingCatalog.Validate(new ShelterShieldingCatalog
            {
                filter_clog_max_ingress_penalty = -1f,
                radon_rad_per_bqm3 = float.NaN,
                max_ingress_multiplier = 99f,
                decon_internal_reduction_fraction = 2f
            }, errors);
            Assert.Contains(errors, e => e.Contains("filter_clog_max_ingress_penalty"));
            Assert.Contains(errors, e => e.Contains("radon_rad_per_bqm3 must be finite"));
            Assert.Contains(errors, e => e.Contains("max_ingress_multiplier"));
            Assert.Contains(errors, e => e.Contains("decon_internal_reduction_fraction"));
        }

        [Fact]
        public void MissingFile_IsReported_NotSilentlyDefaulted()
        {
            var catalog = ShelterShieldingCatalog.LoadFromDirectory(
                Path.Combine(Path.GetTempPath(), "ashfall_missing_dir"), new Ashfall.Core.FileSystemIO());
            Assert.False(catalog.IsValid);
            Assert.Contains(catalog.Errors, e => e.Contains("file missing"));
        }
    }

    /// <summary>
    /// Source gates (plan §23.4/§27): the model's filter input must be the same
    /// numeric field the air-hazard warning reads (no drifting warning flag),
    /// and the host binding must consume canonical owners only.
    /// </summary>
    public sealed class ShelterShieldingSourceGateTests
    {
        private static string RepoRoot
        {
            get
            {
                string dir = AppContext.BaseDirectory;
                for (int i = 0; i < 8; i++)
                {
                    if (File.Exists(Path.Combine(dir, "Ashfall.csproj"))) return dir;
                    dir = Path.GetDirectoryName(dir)!;
                }
                throw new InvalidOperationException("repository root not found");
            }
        }

        private static string Read(string relativePath)
            => File.ReadAllText(Path.Combine(RepoRoot, relativePath.Replace('/', Path.DirectorySeparatorChar)));

        [Fact]
        public void FilterWarning_AndModelInput_ShareTheSameNumericField()
        {
            string hostBinding = Read("src/Main.Survivors.cs");
            Assert.Contains("State.airFilterHealthPercent", hostBinding, StringComparison.Ordinal);

            string startingLevel = Read("Assets/Ashfall.Core/StartingLevel/StartingLevelSystem.cs");
            Assert.Contains("airFilterHealthPercent < 50f", startingLevel, StringComparison.Ordinal);
        }

        [Fact]
        public void HostBinding_ConsumesCanonicalOwnersOnly()
        {
            string src = Read("src/Main.Survivors.cs");
            Assert.Contains("GetWeakestCeilingAttenuation", src, StringComparison.Ordinal);
            Assert.Contains("_ventilation?.State", src, StringComparison.Ordinal);
            Assert.Contains("doorState", src, StringComparison.Ordinal);
            Assert.Contains("shelterContaminationLevel", src, StringComparison.Ordinal);
        }

        [Fact]
        public void ShelterShieldingData_IsGovernedByTheIntegrityGate()
        {
            string validator = Read("Assets/Ashfall.Core/CatalogIntegrityValidator.cs");
            Assert.Contains("ShelterShieldingCatalog", validator, StringComparison.Ordinal);
        }
    }

    /// <summary>
    /// End-to-end seam: a shelter interior context bound through
    /// ExposureEnvironmentResolver + RadiationSystem must consume the model's
    /// interior query (plan §12/§22), and the UI read model must agree.
    /// </summary>
    public sealed class ShelterShieldingSeamTests
    {
        private static SurvivorRadState TickOnce(
            ExposureEnvironmentResolver resolver, float hours = 1f)
        {
            var state = new SurvivorRadState { Id = "seam" };
            var sys = new RadiationSystem(exposureContext: _ =>
                resolver.ResolveForEnvironment(SurvivorExposureLocation.ShelterInterior, "")
                    .ToExposureContext());
            sys.Register(state);
            sys.Tick(hours);
            return state;
        }

        [Fact]
        public void ResolverWithModel_InteriorQueryDrivesDose_AndBreakdownAgrees()
        {
            var model = new ShelterShieldingModel
            {
                StructuralAttenuationProvider = () => 0.5f, // legacy 1.0
                IndoorRadonProvider = () => 100f            // +0.5
            };
            var resolver = new ExposureEnvironmentResolver
            {
                ShelterAttenuationProvider = () => 0.5f,
                ShelterInteriorRadQuery = zone => model.ComputeInteriorRad(zone)
            };

            var env = resolver.ResolveForEnvironment(SurvivorExposureLocation.ShelterInterior, "");
            Assert.NotNull(env.InteriorRadQuery);
            Assert.Equal(1.5f, env.InteriorRadQuery!(env.EffectiveZoneRadLevel), 3);

            var state = TickOnce(resolver);
            Assert.Equal(1.5f, state.LifetimeRadiationExposure, 3);

            var breakdown = ExposureBreakdown.Build(env, 0f, state.RadiationDose,
                state.LifetimeRadiationExposure, interiorRads: env.InteriorRadQuery(env.EffectiveZoneRadLevel));
            Assert.Equal(1.5f, breakdown.EffectiveExposurePerHour, 3);
        }

        [Fact]
        public void ResolverWithoutModel_LegacyPathUnchanged()
        {
            var resolver = new ExposureEnvironmentResolver { ShelterAttenuationProvider = () => 0.5f };
            var env = resolver.ResolveForEnvironment(SurvivorExposureLocation.ShelterInterior, "");
            Assert.Null(env.InteriorRadQuery);
            Assert.Null(env.ToExposureContext().ShelterRadQuery);
            var state = TickOnce(resolver);
            Assert.Equal(1.0f, state.LifetimeRadiationExposure, 3); // 2 × (1 − 0.5)
        }

        [Fact]
        public void NonShelterLocations_NeverCarryInteriorQuery()
        {
            var resolver = new ExposureEnvironmentResolver
            {
                ShelterInteriorRadQuery = zone => 999f
            };
            Assert.Null(resolver.ResolveForEnvironment(SurvivorExposureLocation.WastelandOutdoors, "")
                .InteriorRadQuery);
            Assert.Null(resolver.ResolveForEnvironment(SurvivorExposureLocation.Expedition, "loc_x")
                .InteriorRadQuery);
        }
    }
}