using Ashfall.Core;
using Ashfall.Core.Disease;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WildlifeDiseaseBridgeTests
    {
        private static WildlifeTrappingSystem CreateTrapping() => new WildlifeTrappingSystem(new SeededRng(42));

        private static DiseaseSystem CreateDisease()
        {
            var state = new DiseaseSystemState();
            var sys = new DiseaseSystem(state, new SeededRng(99));
            // Bind minimal catalog with zoonotic flu
            var catalog = new DiseaseCatalog();
            // Ensure zoonotic flu entry exists — use EnsureEntry via BindCatalog with a minimal catalog
            // Instead, directly ensure entry via reflection-like: add disease via Infect will auto-create? No, need catalog.
            // Create a minimal DiseaseCatalog with zoonotic flu def
            var def = new DiseaseDefinition { id = DiseaseIds.ZoonoticFlu, vector = DiseaseVectorNames.Air, infectivity = 1f, illness_days = 5, lethality = 0f };
            catalog.Diseases.Add(def);
            sys.BindCatalog(catalog);
            return sys;
        }

        private static void Wire(WildlifeTrappingSystem trap, DiseaseSystem disease, int day)
        {
            trap.OnButcheryCompleted += (siteId, butcherId, species, isToxic) =>
            {
                if (string.IsNullOrEmpty(butcherId)) return;
                bool hasSterile = butcherId.IndexOf("sterile", System.StringComparison.OrdinalIgnoreCase) >= 0;
                if (hasSterile) return;
                int seed = StableHash.Of(butcherId) ^ day;
                var rng = new SeededRng(seed);
                if (rng.NextDouble() < 0.30)
                    disease.Infect(butcherId, DiseaseIds.ZoonoticFlu, day);
            };
        }

        [Fact]
        public void Butchery_WithoutSterile_HasChanceToInfect()
        {
            var trap = CreateTrapping();
            var disease = CreateDisease();
            const int day = 10;
            Wire(trap, disease, day);

            trap.SetTrap("site_a", "meat", "hunter_a");
            // Force a catch
            trap.State.trapSites[0].hasCatch = true;
            trap.State.trapSites[0].catchSpecies = "rabbit";
            trap.State.trapSites[0].isToxic = false;

            // Determine expected outcome via same RNG
            int seed = StableHash.Of("hunter_a") ^ day;
            bool shouldInfect = new SeededRng(seed).NextDouble() < 0.30;

            trap.Butcher("site_a", "hunter_a");

            bool infected = disease.IsInfected("hunter_a", DiseaseIds.ZoonoticFlu);
            Assert.Equal(shouldInfect, infected);
        }

        [Fact]
        public void Butchery_WithSterileTechnique_NeverInfects()
        {
            var trap = CreateTrapping();
            var disease = CreateDisease();
            const int day = 10;
            Wire(trap, disease, day);

            trap.SetTrap("site_b", "meat", "hunter_sterile");
            trap.State.trapSites[0].hasCatch = true;
            trap.State.trapSites[0].catchSpecies = "rat";
            trap.State.trapSites[0].isToxic = true;

            trap.Butcher("site_b", "hunter_sterile_trait");

            Assert.False(disease.IsInfected("hunter_sterile_trait", DiseaseIds.ZoonoticFlu));
        }

        [Fact]
        public void Butchery_NoButcherId_DoesNotInfect()
        {
            var trap = CreateTrapping();
            var disease = CreateDisease();
            Wire(trap, disease, 10);

            trap.SetTrap("site_c", "meat", "hunter_c");
            trap.State.trapSites[0].hasCatch = true;
            trap.State.trapSites[0].catchSpecies = "rabbit";

            trap.Butcher("site_c", "");

            Assert.Equal(0, disease.State.diseases.Find(d => d.disease_id == DiseaseIds.ZoonoticFlu)?.infected.Count ?? 0);
        }

        [Fact]
        public void DiseaseSaveRoundTrip_PreservesInfection()
        {
            var disease = CreateDisease();
            disease.Infect("survivor_a", DiseaseIds.ZoonoticFlu, 5);
            var state = disease.CaptureState();
            var restored = new DiseaseSystem(new DiseaseSystemState(), new SeededRng(99));
            var catalog = new DiseaseCatalog();
            catalog.Diseases.Add(new DiseaseDefinition { id = DiseaseIds.ZoonoticFlu, vector = DiseaseVectorNames.Air });
            restored.BindCatalog(catalog);
            restored.RestoreState(state);
            Assert.True(restored.IsInfected("survivor_a", DiseaseIds.ZoonoticFlu));
        }
    }
}
