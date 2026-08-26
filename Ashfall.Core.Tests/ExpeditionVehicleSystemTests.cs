using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ExpeditionVehicleSystemTests
    {
        [Fact] public void AcquireVehicle_FromCatalog_CreatesInstance()
        {
            var vs = Create();
            vs.LoadCatalog(new VehicleCatalog { vehicles = new System.Collections.Generic.List<VehicleDefinition> {
                new VehicleDefinition { vehicle_id = "armored_sled", display_name = "Armored Sled", max_fuel = 50f, speed_multiplier = 0.6f }
            }});
            var r = vs.AcquireVehicle("armored_sled");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.NotNull(vs.GetVehicle("armored_sled"));
        }

        [Fact] public void AcquireVehicle_Unknown_Fails()
        {
            var vs = Create();
            var r = vs.AcquireVehicle("nonexistent");
            Assert.Equal(ActionResult.StatusKind.Failed, r.Status);
        }

        [Fact] public void Refuel_IncreasesFuel()
        {
            var vs = CreateWithSled();
            var v = vs.GetVehicle("armored_sled")!;
            float before = v.fuel;
            vs.Refuel("armored_sled", 10f);
            Assert.True(v.fuel > before);
        }

        [Fact] public void Repair_RestoresCondition()
        {
            var vs = CreateWithSled();
            var v = vs.GetVehicle("armored_sled")!;
            v.condition = 40f;
            vs.Repair("armored_sled", 30f);
            Assert.Equal(70f, v.condition);
        }

        [Fact] public void PrepareForExpedition_ConsumesFuel()
        {
            var vs = CreateWithSled();
            var v = vs.GetVehicle("armored_sled")!;
            v.fuel = 50f;
            var (fuelCost, _, _) = vs.PrepareForExpedition("armored_sled", 10f);
            Assert.True(fuelCost > 0);
            Assert.True(v.fuel < 50f);
        }

        [Fact] public void CaptureRestoreState_PreservesVehicles()
        {
            var vs = CreateWithSled();
            var state = vs.CaptureState();
            Assert.Single(state.ownedVehicles);

            var vs2 = Create();
            vs2.RestoreState(state);
            Assert.Single(vs2.State.ownedVehicles);
        }

        private static ExpeditionVehicleSystem CreateWithSled()
        {
            var vs = Create();
            vs.LoadCatalog(new VehicleCatalog { vehicles = new System.Collections.Generic.List<VehicleDefinition> {
                new VehicleDefinition { vehicle_id = "armored_sled", display_name = "Armored Sled", max_fuel = 50f, speed_multiplier = 0.6f }
            }});
            vs.AcquireVehicle("armored_sled");
            return vs;
        }

        private static ExpeditionVehicleSystem Create() => new ExpeditionVehicleSystem(new SeededRng(42));
    }
}
