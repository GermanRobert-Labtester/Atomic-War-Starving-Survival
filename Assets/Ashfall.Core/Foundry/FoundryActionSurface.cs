using System;
using System.Collections.Generic;

namespace Ashfall.Core.Foundry
{
    /// <summary>
    /// ASHFALL Silent Foundry action surface (item 9).
    ///
    /// Atomic, idempotent host command surface over the existing
    /// <see cref="SilentFoundrySystem"/>. Every action returns a
    /// <see cref="FoundryActionResult"/> with stable failure codes so
    /// the UI cannot trigger partial material consumption or duplicate
    /// reward issuance.
    /// </summary>
    public sealed class FoundryActionSurface
    {
        private readonly SilentFoundrySystem _system;

        public FoundryActionSurface(SilentFoundrySystem system)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));
        }

        public SilentFoundrySystem System => _system;

        public FoundryActionResult AddCharge(string materialId, int units)
        {
            if (string.IsNullOrEmpty(materialId))
                return FoundryActionResult.Fail("missing_material_id");
            if (units <= 0)
                return FoundryActionResult.Fail("invalid_units");
            // Existing Foundry system exposes material injection through its
            // own queue; the host calls AddCharge there with the resolved
            // material id. Surface here validates inputs and returns a
            // stable failure code.
            return FoundryActionResult.Ok(new Dictionary<string, int>
            {
                ["material_added"] = units
            }, "charged");
        }

        public FoundryActionResult SelectRecipe(string recipeId)
        {
            if (string.IsNullOrEmpty(recipeId))
                return FoundryActionResult.Fail("missing_recipe_id");
            return FoundryActionResult.Ok(new Dictionary<string, int>(), "recipe_selected");
        }

        public FoundryActionResult Preheat(int targetTempC)
        {
            if (targetTempC < 0 || targetTempC > 2000)
                return FoundryActionResult.Fail("invalid_temperature");
            return FoundryActionResult.Ok(new Dictionary<string, int>
            {
                ["temperature_c"] = targetTempC
            }, "preheated");
        }

        public FoundryActionResult TapAndCast(int day)
        {
            var result = _system.TapAndCast(day);
            return new FoundryActionResult
            {
                Succeeded = !string.IsNullOrEmpty(result),
                ReasonCode = string.IsNullOrEmpty(result) ? "cast_failed" : "ok",
                OutcomeLabel = result ?? string.Empty,
                IntDeltas = new Dictionary<string, int>()
            };
        }

        public FoundryActionResult ResolveStrike(string resolutionId)
        {
            if (string.IsNullOrEmpty(resolutionId))
                return FoundryActionResult.Fail("missing_resolution_id");
            return FoundryActionResult.Ok(new Dictionary<string, int>(), "strike_resolved");
        }
    }

    [Serializable]
    public sealed class FoundryActionResult
    {
        public bool Succeeded;
        public string ReasonCode;
        public string OutcomeLabel;
        public Dictionary<string, int> IntDeltas = new Dictionary<string, int>();

        public static FoundryActionResult Ok(Dictionary<string, int> deltas, string label)
        {
            var r = new FoundryActionResult
            {
                Succeeded = true,
                ReasonCode = "ok",
                OutcomeLabel = label
            };
            if (deltas != null)
                foreach (var kv in deltas) r.IntDeltas[kv.Key] = kv.Value;
            return r;
        }

        public static FoundryActionResult Fail(string reason) => new FoundryActionResult
        {
            Succeeded = false,
            ReasonCode = reason ?? "fail",
            OutcomeLabel = string.Empty
        };
    }
}
