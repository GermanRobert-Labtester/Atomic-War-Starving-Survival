using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WildlifeDiseaseBridgeTests
    {
        private const string SanitizationExpertTrait = "skill_sanitization_expert";

        private static WildlifeTrappingSystem CreateTrapping() => new WildlifeTrappingSystem(new SeededRng(42));

        private static DiseaseSystem CreateDisease()
        {
            var state = new DiseaseSystemState();
            var sys = new DiseaseSystem(state, new SeededRng(99));
            var catalog = new DiseaseCatalog();
            var def = new DiseaseDefinition { id = DiseaseIds.ZoonoticFlu, vector = DiseaseVectorNames.Air, infectivity = 1f, illness_days = 5, lethality = 0f };
            catalog.Diseases.Add(def);
            sys.BindCatalog(catalog);
            return sys;
        }

        private static SurvivorRosterSystem CreateRosterWithTrait(string survivorId, string traitId)
        {
            var roster = new SurvivorRosterSystem();
            roster.RegisterDefinition(new SurvivorDefinition
            {
                id = survivorId,
                displayName = survivorId,
                traitIds = new List<string> { traitId }
            });
            return roster;
        }

        private static void Wire(WildlifeTrappingSystem trap, DiseaseSystem disease, SurvivorRosterSystem roster, int day = 10)
        {
            trap.OnButcheryCompleted += (siteId, butcherId, species, isToxic) =>
            {
                if (string.IsNullOrEmpty(butcherId)) return;
                var def = roster.FindDefinition(butcherId);
                if (def != null && def.traitIds != null && def.traitIds.Contains(SanitizationExpertTrait))
                    return;
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
            var roster = new SurvivorRosterSystem();
            const int day = 10;
            Wire(trap, disease, roster, day);

            trap.SetTrap("site_a", "meat", "hunter_a");
            trap.State.trapSites[0].hasCatch = true;
            trap.State.trapSites[0].catchSpecies = "rabbit";
            trap.State.trapSites[0].isToxic = false;

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
            var roster = CreateRosterWithTrait("the_surgeon", SanitizationExpertTrait);
            const int day = 10;
            Wire(trap, disease, roster, day);

            trap.SetTrap("site_b", "meat", "the_surgeon");
            trap.State.trapSites[0].hasCatch = true;
            trap.State.trapSites[0].catchSpecies = "rat";
            trap.State.trapSites[0].isToxic = true;

            trap.Butcher("site_b", "the_surgeon");

            Assert.False(disease.IsInfected("the_surgeon", DiseaseIds.ZoonoticFlu));
        }

        [Fact]
        public void Butchery_NoButcherId_DoesNotInfect()
        {
            var trap = CreateTrapping();
            var disease = CreateDisease();
            var roster = new SurvivorRosterSystem();
            Wire(trap, disease, roster, 10);

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
