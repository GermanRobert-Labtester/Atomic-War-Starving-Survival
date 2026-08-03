using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Shelter power grid: generation, load shedding by priority, bicycle pedaling, UI.
    /// Acceptance: 50 W gen + 30 W filter P1 + 40 W heater P2 → heater shed, filter on.
    /// </summary>
    [TestFixture]
    public class PowerGridTests
    {
        private const float Eps = 1e-3f;
        private PowerSourceSO _diesel;
        private PowerSourceSO _bike;

        [SetUp]
        public void SetUp()
        {
            _diesel = PowerSourceSO.CreateDieselGenerator(50f);
            _bike = PowerSourceSO.CreateBicycleGenerator(20f);
        }

        [TearDown]
        public void TearDown()
        {
            if (_diesel != null) Object.DestroyImmediate(_diesel);
            if (_bike != null) Object.DestroyImmediate(_bike);
            _diesel = null;
            _bike = null;
        }

        private PowerNetwork MakeBareGrid(float dieselWatts, float dieselFuel)
        {
            var net = new PowerNetwork();
            var diesel = PowerSourceSO.CreateDieselGenerator(dieselWatts);
            // Keep refs for teardown via local destroy in callers that need extra SOs;
            // for bare grids we destroy after use in each test via network only.
            // Re-use class-level _diesel if watts match.
            if (Mathf.Approximately(dieselWatts, 50f))
            {
                Object.DestroyImmediate(diesel);
                diesel = _diesel;
            }
            net.RegisterSourceDefinition(diesel);
            net.AddSource(new PowerSourceInstance(diesel, dieselFuel));
            return net;
        }

        [Test]
        public void LoadShed_50W_Gen_30W_Filter_P1_40W_Heater_P2_ShedsHeater_KeepsFilter()
        {
            // Acceptance scenario from the prompt.
            var net = MakeBareGrid(50f, 100f);
            net.AddConsumer(new PowerConsumer("air_filtration", "Air Filter", 30f, 1));
            net.AddConsumer(new PowerConsumer("heater", "Heater", 40f, 2));

            Assert.That(net.TotalGeneration, Is.EqualTo(50f).Within(Eps));
            Assert.That(net.RequestedDraw, Is.EqualTo(70f).Within(Eps));
            Assert.That(net.TotalDraw, Is.EqualTo(30f).Within(Eps),
                "After load-shed, only the 30 W filter should draw");

            var filter = net.GetConsumer("air_filtration");
            var heater = net.GetConsumer("heater");
            Assert.That(filter.IsPowered, Is.True, "Priority-1 filter must remain on");
            Assert.That(filter.IsShed, Is.False);
            Assert.That(heater.IsPowered, Is.False, "Priority-2 heater must be auto-disabled");
            Assert.That(heater.IsShed, Is.True);
            Assert.That(net.IsLoadShedding, Is.True);
            Assert.That(net.IsBlackout, Is.False, "Partial shed is not a full blackout");
        }

        [Test]
        public void ApplyToShelter_DisablesShedModules()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance("air_filtration", 1) { FilterHealth = 100f });
            shelter.AddModule(new ShelterModuleInstance("heater", 1) { Fuel = 10f });

            var net = MakeBareGrid(50f, 100f);
            net.AddConsumer(new PowerConsumer("air_filtration", "Air Filter", 30f, 1));
            net.AddConsumer(new PowerConsumer("heater", "Heater", 40f, 2));
            net.ApplyToShelter(shelter);

            Assert.That(shelter.GetModule("air_filtration").IsEnabled, Is.True);
            Assert.That(shelter.GetModule("heater").IsEnabled, Is.False);
            Assert.That(shelter.GetModule("air_filtration").IsOperational, Is.True);
            Assert.That(shelter.GetModule("heater").IsOperational, Is.False);
        }

        [Test]
        public void Blackout_WhenFuelDepleted_AndNoPedaler()
        {
            var net = MakeBareGrid(50f, 0.01f);
            net.AddConsumer(new PowerConsumer("air_filtration", "Air Filter", 30f, 1));

            // Burn remaining fuel.
            net.Tick(1f);
            Assert.That(net.GetSource("diesel_generator").Fuel, Is.EqualTo(0f).Within(Eps));
            Assert.That(net.TotalGeneration, Is.EqualTo(0f).Within(Eps));
            Assert.That(net.IsBlackout, Is.True);
            Assert.That(net.GetConsumer("air_filtration").IsPowered, Is.False);
        }

        [Test]
        public void Diesel_EmitsCarbonMonoxide()
        {
            var net = MakeBareGrid(50f, 50f);
            net.AddConsumer(new PowerConsumer("air_filtration", "Air Filter", 10f, 1));
            float before = net.CarbonMonoxidePpm;
            net.Tick(1f);
            Assert.That(net.CarbonMonoxidePpm, Is.GreaterThan(before));
            Assert.That(net.GetSource("diesel_generator").Fuel, Is.LessThan(50f));
        }

        [Test]
        public void Bicycle_Pedaling_ProducesPower_AndDrainsFatigue()
        {
            var net = new PowerNetwork();
            net.RegisterSourceDefinition(_bike);
            net.AddSource(new PowerSourceInstance(_bike, 0f));
            net.AddConsumer(new PowerConsumer("air_filtration", "Air Filter", 15f, 1));

            Assert.That(net.TotalGeneration, Is.EqualTo(0f).Within(Eps),
                "Bike produces nothing without a pedaler");

            var sv = new Survivor { Id = "sv_pedal", DisplayName = "Pedaler" };
            sv.Needs.Fatigue = 20f;
            sv.Needs.Hunger = 20f;

            net.AssignPedaler("bicycle_generator", sv.Id);
            Assert.That(net.TotalGeneration, Is.EqualTo(20f).Within(Eps));
            Assert.That(net.GetConsumer("air_filtration").IsPowered, Is.True);

            net.Tick(1f, weatherName: null, tryApplyPedalCost: (id, fat, hun) =>
            {
                if (id != sv.Id) return false;
                if (sv.Needs.Fatigue >= 95f) return false;
                sv.Needs.Fatigue = Mathf.Clamp(sv.Needs.Fatigue + fat, 0f, 100f);
                sv.Needs.Hunger = Mathf.Clamp(sv.Needs.Hunger + hun, 0f, 100f);
                return true;
            });
            Assert.That(sv.Needs.Fatigue, Is.GreaterThan(20f));
            Assert.That(sv.Needs.Hunger, Is.GreaterThan(20f));
        }

        [Test]
        public void PedalGeneratorAction_ClaimsBike_WhenFuelScarce()
        {
            var net = new PowerNetwork();
            net.RegisterSourceDefinition(_diesel);
            net.RegisterSourceDefinition(_bike);
            net.AddSource(new PowerSourceInstance(_diesel, 0f)); // empty diesel
            net.AddSource(new PowerSourceInstance(_bike, 0f));
            net.AddConsumer(new PowerConsumer("air_filtration", "Air Filter", 15f, 1));

            var sv = new Survivor { Id = "sv_rider", DisplayName = "Rider" };
            sv.Needs.Fatigue = 10f;
            sv.Needs.Hunger = 10f;

            var action = ScriptableObject.CreateInstance<PedalGeneratorActionSO>();
            try
            {
                var ctx = new AIContext(sv) { PowerNetwork = net };
                float score = action.EvaluateRaw(ctx);
                Assert.That(score, Is.GreaterThan(0.2f), "Should want to pedal during blackout/no fuel");
                action.Execute(ctx);
                Assert.That(net.GetSource("bicycle_generator").PedalingSurvivorId, Is.EqualTo(sv.Id));
                Assert.That(net.TotalGeneration, Is.EqualTo(20f).Within(Eps));
                Assert.That(net.IsBlackout, Is.False);
            }
            finally
            {
                Object.DestroyImmediate(action);
            }
        }

        [Test]
        public void PriorityToggle_ChangesShedOrder()
        {
            var net = MakeBareGrid(50f, 100f);
            net.AddConsumer(new PowerConsumer("air_filtration", "Air Filter", 30f, 1));
            net.AddConsumer(new PowerConsumer("heater", "Heater", 40f, 2));

            // Swap: make filter lower priority than heater.
            net.SetPriority("air_filtration", 5);
            net.SetPriority("heater", 1);

            Assert.That(net.GetConsumer("heater").IsPowered, Is.True);
            Assert.That(net.GetConsumer("air_filtration").IsShed, Is.True);
        }

        [Test]
        public void PowerGridHUD_ShowsBudget_AndPriorityCycle()
        {
            var net = MakeBareGrid(50f, 100f);
            net.AddConsumer(new PowerConsumer("air_filtration", "Air Filter", 30f, 1));
            net.AddConsumer(new PowerConsumer("heater", "Heater", 40f, 2));

            var go = new GameObject("PowerGridHUD_Test");
            try
            {
                var hud = go.AddComponent<PowerGridHUD>();
                hud.Bind(net);
                hud.Open();
                string panel = hud.BuildPanelText();
                Assert.That(panel, Does.Contain("POWER"));
                Assert.That(panel, Does.Contain("50"));
                Assert.That(panel, Does.Contain("Air Filter"));
                Assert.That(panel, Does.Contain("LOAD SHED").Or.Contain("SHED"));

                int next = hud.CyclePriority("heater");
                Assert.That(next, Is.EqualTo(3));
                Assert.That(net.GetConsumer("heater").Priority, Is.EqualTo(3));
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void CreateDefault_HasDieselBikeAndStandardLoads()
        {
            var net = PowerNetwork.CreateDefault(40f);
            Assert.That(net.GetSource("diesel_generator"), Is.Not.Null);
            Assert.That(net.GetSource("bicycle_generator"), Is.Not.Null);
            Assert.That(net.GetConsumer("air_filtration").Watts, Is.EqualTo(30f).Within(Eps));
            Assert.That(net.GetConsumer("heater").Watts, Is.EqualTo(40f).Within(Eps));
            // Default: filter + heater requested → load shed heater
            Assert.That(net.GetConsumer("air_filtration").IsPowered, Is.True);
            Assert.That(net.GetConsumer("heater").IsShed, Is.True);
        }

        [Test]
        public void SaveRestore_PreservesFuelPriorityAndPedaler()
        {
            var net = PowerNetwork.CreateDefault(22f);
            net.SetPriority("grow_light", 5);
            net.SetRequested("grow_light", true);
            net.AssignPedaler("bicycle_generator", "sv_elena");

            var save = net.CaptureState();
            var restored = new PowerNetwork();
            restored.RegisterSourceDefinition(_diesel);
            restored.RegisterSourceDefinition(_bike);
            restored.RestoreState(save);

            Assert.That(restored.GetSource("diesel_generator").Fuel, Is.EqualTo(22f).Within(Eps));
            Assert.That(restored.GetConsumer("grow_light").Priority, Is.EqualTo(5));
            Assert.That(restored.GetConsumer("grow_light").IsRequested, Is.True);
            Assert.That(restored.GetSource("bicycle_generator").PedalingSurvivorId, Is.EqualTo("sv_elena"));
        }
    }
}
