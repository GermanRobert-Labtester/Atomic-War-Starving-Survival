// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Normalized expectation record for authoritative wildlife catalog prey.
    /// Used across Tasks 5-8 to verify disease resolution, contamination doses,
    /// and catalog-to-runtime mapping.
    /// </summary>
    public sealed class WildlifeCatalogExpectation
    {
        public string SpeciesId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public float DiseaseRisk { get; set; }
        public string ExplicitDiseaseId { get; set; } = string.Empty;
        public string ExpectedResolvedDiseaseId { get; set; } = string.Empty;
        public float ContaminationDose { get; set; }
        public bool IsAuthoredDose { get; set; }
        public bool HasExplicitDisease => !string.IsNullOrEmpty(ExplicitDiseaseId);
        public bool IsLowRisk => DiseaseRisk <= 0.10f;
        public string Basis { get; set; } = string.Empty;
    }

    /// <summary>
    /// Shared test fixture for Tasks 5-8:
    /// - Resolves authoritative Data directory
    /// - Loads real wildlife_trapping_catalog.json
    /// - Exposes the authoritative 15-prey expectation matrix
    /// - Provides independent test-side disease oracle
    /// - Provides CountingSeededRng for deterministic draw tracking
    /// </summary>
    public static class WildlifeTrappingCatalogTestFixture
    {
        public const string FallbackDiseaseId = "disease_zoonotic_flu";
        public const float FallbackContaminationDose = 2.0f;
        public const float LowRiskThreshold = 0.10f;

        public static string FindDataDir()
        {
            var dir = Directory.GetCurrentDirectory();
            for (int i = 0; i < 10; i++)
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                candidate = Path.Combine(dir, "assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                dir = Path.GetDirectoryName(dir) ?? dir;
            }
            return "Assets/StreamingAssets/Data";
        }

        public static WildlifeTrappingCatalog LoadCatalog()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = WildlifeTrappingCatalogLoader.Load(FindDataDir(), fileIO, json);
            if (catalog == null)
                throw new InvalidOperationException("Failed to load authoritative wildlife_trapping_catalog.json");
            return catalog;
        }

        /// <summary>
        /// Independent test-side disease oracle (Section 46):
        /// - If explicit diseaseId is present, return it.
        /// - If diseaseRisk &lt;= 0.1, return empty (no disease).
        /// - Otherwise, return "disease_zoonotic_flu".
        /// Never calls production ResolveDiseaseId() to avoid circular logic.
        /// </summary>
        public static string ExpectedDisease(PreyDefinition? prey)
        {
            if (prey == null) return string.Empty;
            return ExpectedDisease(prey.diseaseRisk, prey.diseaseId);
        }

        public static string ExpectedDisease(float diseaseRisk, string? explicitDiseaseId = null)
        {
            if (!string.IsNullOrEmpty(explicitDiseaseId))
                return explicitDiseaseId;
            if (diseaseRisk <= LowRiskThreshold)
                return string.Empty;
            return FallbackDiseaseId;
        }

        /// <summary>
        /// The authoritative 15-prey matrix verified against wildlife_trapping_catalog.json.
        /// Exactly 15 prey: 3 low-risk (no disease), 7 medium-risk fallback (zoonotic flu),
        /// and 5 high-risk explicit overrides.
        /// </summary>
        public static readonly IReadOnlyList<WildlifeCatalogExpectation> Authoritative15Prey = new List<WildlifeCatalogExpectation>
        {
            new WildlifeCatalogExpectation
            {
                SpeciesId = "rabbit",
                DisplayName = "Ash Rabbit",
                DiseaseRisk = 0.10f,
                ExplicitDiseaseId = "",
                ExpectedResolvedDiseaseId = "",
                ContaminationDose = 2.0f,
                IsAuthoredDose = false,
                Basis = "fallback boundary (risk <= 0.1)"
            },
            new WildlifeCatalogExpectation
            {
                SpeciesId = "cotton_hare",
                DisplayName = "Cotton Hare",
                DiseaseRisk = 0.10f,
                ExplicitDiseaseId = "",
                ExpectedResolvedDiseaseId = "",
                ContaminationDose = 2.0f,
                IsAuthoredDose = false,
                Basis = "fallback boundary (risk <= 0.1)"
            },
            new WildlifeCatalogExpectation
            {
                SpeciesId = "deer",
                DisplayName = "Wasteland Mule Deer",
                DiseaseRisk = 0.15f,
                ExplicitDiseaseId = "",
                ExpectedResolvedDiseaseId = FallbackDiseaseId,
                ContaminationDose = 2.0f,
                IsAuthoredDose = false,
                Basis = "medium fallback (risk > 0.1)"
            },
            new WildlifeCatalogExpectation
            {
                SpeciesId = "boar",
                DisplayName = "Razorback Boar",
                DiseaseRisk = 0.20f,
                ExplicitDiseaseId = "",
                ExpectedResolvedDiseaseId = FallbackDiseaseId,
                ContaminationDose = 2.0f,
                IsAuthoredDose = false,
                Basis = "medium fallback (risk > 0.1)"
            },
            new WildlifeCatalogExpectation
            {
                SpeciesId = "fox",
                DisplayName = "Barren Fox",
                DiseaseRisk = 0.20f,
                ExplicitDiseaseId = "",
                ExpectedResolvedDiseaseId = FallbackDiseaseId,
                ContaminationDose = 2.0f,
                IsAuthoredDose = false,
                Basis = "medium fallback (risk > 0.1)"
            },
            new WildlifeCatalogExpectation
            {
                SpeciesId = "rat",
                DisplayName = "Irradiated Rat",
                DiseaseRisk = 0.35f,
                ExplicitDiseaseId = "disease_typhoid_waterborne",
                ExpectedResolvedDiseaseId = "disease_typhoid_waterborne",
                ContaminationDose = 4.0f,
                IsAuthoredDose = true,
                Basis = "explicit catalog override (typhoid)"
            },
            new WildlifeCatalogExpectation
            {
                SpeciesId = "pheasant",
                DisplayName = "Ash Pheasant",
                DiseaseRisk = 0.15f,
                ExplicitDiseaseId = "",
                ExpectedResolvedDiseaseId = FallbackDiseaseId,
                ContaminationDose = 2.0f,
                IsAuthoredDose = false,
                Basis = "medium fallback (risk > 0.1)"
            },
            new WildlifeCatalogExpectation
            {
                SpeciesId = "ash_crow",
                DisplayName = "Three-Eyed Sentry Crow",
                DiseaseRisk = 0.25f,
                ExplicitDiseaseId = "disease_blood_fever",
                ExpectedResolvedDiseaseId = "disease_blood_fever",
                ContaminationDose = 6.0f,
                IsAuthoredDose = true,
                Basis = "explicit catalog override (blood fever)"
            },
            new WildlifeCatalogExpectation
            {
                SpeciesId = "mirror_carp",
                DisplayName = "Mirror Carp",
                DiseaseRisk = 0.10f,
                ExplicitDiseaseId = "",
                ExpectedResolvedDiseaseId = "",
                ContaminationDose = 2.0f,
                IsAuthoredDose = false,
                Basis = "fallback boundary (risk <= 0.1)"
            },
            new WildlifeCatalogExpectation
            {
                SpeciesId = "ash_pike",
                DisplayName = "Ash Pike",
                DiseaseRisk = 0.12f,
                ExplicitDiseaseId = "",
                ExpectedResolvedDiseaseId = FallbackDiseaseId,
                ContaminationDose = 2.0f,
                IsAuthoredDose = false,
                Basis = "medium fallback (risk > 0.1)"
            },
            new WildlifeCatalogExpectation
            {
                SpeciesId = "irradiated_squirrel",
                DisplayName = "Irradiated Squirrel",
                DiseaseRisk = 0.30f,
                ExplicitDiseaseId = "disease_spore_blight",
                ExpectedResolvedDiseaseId = "disease_spore_blight",
                ContaminationDose = 20.0f,
                IsAuthoredDose = true,
                Basis = "explicit catalog override (spore blight)"
            },
            new WildlifeCatalogExpectation
            {
                SpeciesId = "contaminated_fowl",
                DisplayName = "Contaminated Fowl",
                DiseaseRisk = 0.40f,
                ExplicitDiseaseId = FallbackDiseaseId,
                ExpectedResolvedDiseaseId = FallbackDiseaseId,
                ContaminationDose = 12.0f,
                IsAuthoredDose = true,
                Basis = "explicit catalog override (zoonotic flu)"
            },
            new WildlifeCatalogExpectation
            {
                SpeciesId = "rad_dog",
                DisplayName = "Rad Dog",
                DiseaseRisk = 0.30f,
                ExplicitDiseaseId = FallbackDiseaseId,
                ExpectedResolvedDiseaseId = FallbackDiseaseId,
                ContaminationDose = 8.0f,
                IsAuthoredDose = true,
                Basis = "explicit catalog override (zoonotic flu)"
            },
            new WildlifeCatalogExpectation
            {
                SpeciesId = "muskrat",
                DisplayName = "Marsh Muskrat",
                DiseaseRisk = 0.20f,
                ExplicitDiseaseId = "",
                ExpectedResolvedDiseaseId = FallbackDiseaseId,
                ContaminationDose = 2.0f,
                IsAuthoredDose = false,
                Basis = "medium fallback (risk > 0.1)"
            },
            new WildlifeCatalogExpectation
            {
                SpeciesId = "hedgehog",
                DisplayName = "Ash Hedgehog",
                DiseaseRisk = 0.15f,
                ExplicitDiseaseId = "",
                ExpectedResolvedDiseaseId = FallbackDiseaseId,
                ContaminationDose = 2.0f,
                IsAuthoredDose = false,
                Basis = "medium fallback (risk > 0.1)"
            }
        };
    }

    /// <summary>
    /// Test-only counting wrapper around ISeededRng.
    /// Tracks exact number of random draws without touching framework internals.
    /// </summary>
    public sealed class CountingSeededRng : ISeededRng
    {
        private readonly ISeededRng _inner;
        public int DrawCount { get; private set; }

        public CountingSeededRng(int seed) : this(new SeededRng(seed)) { }

        public CountingSeededRng(ISeededRng inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public int Seed => _inner.Seed;

        public int Next(int minInclusive, int maxExclusive)
        {
            DrawCount++;
            return _inner.Next(minInclusive, maxExclusive);
        }

        public float NextFloat()
        {
            DrawCount++;
            return _inner.NextFloat();
        }

        public double NextDouble()
        {
            DrawCount++;
            return _inner.NextDouble();
        }

        public void ResetCount() => DrawCount = 0;
    }

    /// <summary>
    /// Test session adapter that executes the exact disease and contamination resolution
    /// contracts for headless xUnit test execution without depending on Godot.
    /// </summary>
    public sealed class TestWildlifeTrappingSession
    {
        public WildlifeTrappingSystem System { get; }
        public WildlifeTrappingCatalog? Catalog { get; set; }
        public Action<string, string, int>? ApplyDisease { get; set; }
        public Action<string, float>? ApplyContamination { get; set; }
        public Func<PreyDefinition, string>? DiseaseResolver { get; set; }

        public TestWildlifeTrappingSession(WildlifeTrappingSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public ActionResult Butcher(string siteId, string butcherId = "")
        {
            var res = System.Butcher(siteId, butcherId);
            if (res.IsSuccess)
            {
                var site = System.State.trapSites.Find(s => s.siteId == siteId);
                PreyDefinition? preyDef = null;
                if (site != null)
                {
                    if (Catalog != null && Catalog.Prey.TryGetValue(site.catchSpecies, out var pd))
                        preyDef = pd;
                    else if (System.GetPreyDefinitionCatalog().TryGetValue(site.catchSpecies, out var pd2))
                        preyDef = pd2;
                }

                if (site != null && preyDef != null)
                {
                    string survivor = string.IsNullOrEmpty(butcherId) ? "unknown" : butcherId;
                    int day = site.setDay > 0 ? site.setDay : 1;

                    var resolver = DiseaseResolver ?? PreyDefinition.ResolveDiseaseId;
                    string diseaseId = !string.IsNullOrEmpty(site.diseaseId)
                        ? site.diseaseId
                        : (System.RollDiseaseRisk(preyDef.diseaseRisk) ? resolver(preyDef) : string.Empty);

                    if (ApplyDisease != null && !string.IsNullOrEmpty(diseaseId))
                    {
                        ApplyDisease(survivor, diseaseId, day);
                    }

                    float dose = site.contaminationDose > 0f
                        ? site.contaminationDose
                        : (System.RollContaminationRisk(preyDef.contaminationRisk)
                            ? (preyDef.contaminationDose > 0f ? preyDef.contaminationDose : PreyDefinition.FallbackContaminationDose)
                            : 0f);

                    if (ApplyContamination != null && dose > 0f)
                    {
                        ApplyContamination(survivor, dose);
                    }
                }
            }
            return res;
        }
    }
}
