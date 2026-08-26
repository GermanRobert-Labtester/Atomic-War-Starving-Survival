using System.Text.Json;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Greenhouse;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Regression tests proving that an unchanged tick does not mutate state
    /// or emit events. The host session layer (GreenhouseHostSession.TickDay)
    /// must not mark dirty / write saves / raise StateChanged when the Core
    /// system's tick is a no-op. These tests pin the Core-level invariant.
    /// </summary>
    public class DirtyFlushNoOpRegressionTests
    {
        private static readonly JsonSerializerOptions Opts = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true
        };

        private static string Serialize(object o) => JsonSerializer.Serialize(o, Opts);

        // ── Greenhouse: all plots fallow ──────────────────────────────

        [Fact]
        public void Greenhouse_FallowPlots_TickDay_DoesNotMutateState()
        {
            var sys = new GreenhouseSystem(seed: 42);
            sys.EnsurePlots(4);

            var before = sys.CaptureState();
            string jsonBefore = Serialize(before);

            sys.TickDay(1, growLightHours: 6f, ashContaminationRate: 0.05f);
            sys.TickDay(2, growLightHours: 6f, ashContaminationRate: 0.05f);
            sys.TickDay(3, growLightHours: 6f, ashContaminationRate: 0.05f);

            var after = sys.CaptureState();
            string jsonAfter = Serialize(after);

            Assert.Equal(jsonBefore, jsonAfter);
        }

        [Fact]
        public void Greenhouse_FallowPlots_TickDay_FiresNoEvents()
        {
            var sys = new GreenhouseSystem(seed: 42);
            sys.EnsurePlots(4);

            int eventCount = 0;
            sys.OnCropPlanted += (_, _, _) => eventCount++;
            sys.OnCropMatured += (_, _) => eventCount++;
            sys.OnCropHarvested += _ => eventCount++;
            sys.OnBlightOutbreak += _ => eventCount++;
            sys.OnPlotDriedOut += _ => eventCount++;
            sys.OnCropFailed += _ => eventCount++;

            sys.TickDay(1, growLightHours: 6f, ashContaminationRate: 0.05f);
            sys.TickDay(2, growLightHours: 6f, ashContaminationRate: 0.05f);
            sys.TickDay(3, growLightHours: 6f, ashContaminationRate: 0.05f);

            Assert.Equal(0, eventCount);
        }

        // ── Apiculture: no hives ─────────────────────────────────────

        [Fact]
        public void Apiculture_NoHives_TickDaily_DoesNotMutateState()
        {
            var sys = new ApicultureSystem();

            var before = sys.CaptureState();
            string jsonBefore = Serialize(before);

            sys.TickDaily(day: 1, greenhouseTemperatureC: 22f,
                greenhouseContamination: 0f, radiationLevel: 2f,
                rng: new SeededRng(1986 + 1));
            sys.TickDaily(day: 2, greenhouseTemperatureC: 22f,
                greenhouseContamination: 0f, radiationLevel: 2f,
                rng: new SeededRng(1986 + 2));
            sys.TickDaily(day: 3, greenhouseTemperatureC: 22f,
                greenhouseContamination: 0f, radiationLevel: 2f,
                rng: new SeededRng(1986 + 3));

            var after = sys.CaptureState();
            string jsonAfter = Serialize(after);

            Assert.Equal(jsonBefore, jsonAfter);
        }

        [Fact]
        public void Apiculture_NoHives_TickDaily_FiresNoDomainEvents()
        {
            var sys = new ApicultureSystem();

            int domainEvents = 0;
            sys.OnHiveInstalled += _ => domainEvents++;
            sys.OnColonyDied += _ => domainEvents++;
            sys.OnColonySwarming += _ => domainEvents++;
            sys.OnColonyStressed += _ => domainEvents++;

            sys.TickDaily(day: 1, greenhouseTemperatureC: 22f,
                greenhouseContamination: 0f, radiationLevel: 2f,
                rng: new SeededRng(1986 + 1));

            Assert.Equal(0, domainEvents);
        }

        // ── Greenhouse with active crop: state DOES change ───────────

        [Fact]
        public void Greenhouse_ActiveCrop_TickDay_DoesMutateState()
        {
            var sys = new GreenhouseSystem(seed: 42);
            sys.EnsurePlots(2);
            sys.Plant(0, GreenhouseExpansionCatalog.Items.SeedMushroom, currentDay: 1, out _);
            sys.Water(0, 60f, tainted: false);

            var before = sys.CaptureState();
            string jsonBefore = Serialize(before);

            sys.TickDay(2, growLightHours: 6f, ashContaminationRate: 0.05f);

            var after = sys.CaptureState();
            string jsonAfter = Serialize(after);

            Assert.NotEqual(jsonBefore, jsonAfter);
        }
    }
}
