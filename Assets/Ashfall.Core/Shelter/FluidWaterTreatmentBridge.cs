// SPDX-License-Identifier: MIT
// ASHFALL Core: explicit bulk-water to distribution transfer boundary.

using System.Collections.Generic;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Moves already-owned WaterTreatmentSystem volume into a distribution
    /// reservoir. The transfer validates both sides before mutation and refunds
    /// the treatment tank if the network commit unexpectedly fails.
    /// </summary>
    public static class FluidWaterTreatmentBridge
    {
        public static ActionResult TransferTreatedWater(
            WaterTreatmentSystem treatment,
            FluidLogisticsSystem network,
            WaterType waterType,
            string reservoirNodeId,
            float amount,
            FluidQuality quality)
        {
            if (treatment == null || network == null || amount <= 0f)
                return ActionResult.Blocked("invalid_transfer", "fluid.invalid_transfer");
            if (treatment.GetWater(waterType) < amount)
                return ActionResult.Blocked("insufficient_treatment_water", "fluid.insufficient_treatment_water");
            if (!network.CanAcceptWater(reservoirNodeId, amount))
                return ActionResult.Blocked("reservoir_capacity", "fluid.reservoir_capacity");

            var removed = treatment.RemoveWater(waterType, amount);
            if (!removed.IsSuccess)
                return ActionResult.Blocked("treatment_transfer_failed", "fluid.transfer_failed");
            if (network.InjectWater(reservoirNodeId, amount, quality))
            {
                return ActionResult.Success("fluid.transfer_complete", new Dictionary<string, double>
                {
                    { "water_transferred", amount }
                });
            }

            treatment.AddWater(waterType, amount);
            return ActionResult.Failed("network_transfer_failed", "fluid.transfer_failed");
        }
    }
}
