// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.World
{
    /// <summary>
    /// F19: Resolves whether a debt repayment route is currently blocked by a weather gate
    /// that carries weather_delay_debt eligibility.
    /// Pure; deterministic; non-mutating.
    /// </summary>
    public static class DebtRouteAccessResolver
    {
        public static bool IsDebtRepaymentRouteBlocked(
            DebtContract debt,
            WeatherGateContextResult gateResult,
            RouteGateContext routeContext)
        {
            if (debt == null || gateResult == null || routeContext == null)
                return false;

            if (!gateResult.IsBlocked || !gateResult.WeatherDelayDebtEligible)
                return false;

            if (string.IsNullOrEmpty(debt.creditorId))
                return false;

            bool matchesCreditor = string.Equals(routeContext.ControllerFactionId, debt.creditorId, StringComparison.OrdinalIgnoreCase) ||
                (routeContext.CreditorFactionIdsReachable != null && routeContext.CreditorFactionIdsReachable.Contains(debt.creditorId, StringComparer.OrdinalIgnoreCase));

            return matchesCreditor;
        }

        /// <summary>
        /// The id of the first delay-eligible gate that blocks repayment of
        /// <paramref name="debt"/> under <paramref name="weather"/>, or null.
        /// Route-specific: only gates whose route reaches the creditor count.
        /// </summary>
        public static string? FindBlockingGateId(
            DebtContract debt,
            IEnumerable<WeatherGate> gates,
            IRouteContextResolver routes,
            WeatherKind weather,
            Func<string, bool> holdsItem)
        {
            if (debt == null || gates == null || routes == null) return null;
            foreach (var gate in gates)
            {
                if (gate == null || !gate.WeatherDelayDebt) continue;
                var held = new List<string>();
                if (!string.IsNullOrEmpty(gate.OverrideItem) && holdsItem != null && holdsItem(gate.OverrideItem))
                    held.Add(gate.OverrideItem);
                var result = WeatherGateContextEvaluator.Evaluate(gate, new WeatherGateEvaluationContext
                {
                    TargetId = gate.TargetId,
                    RouteId = gate.TargetId,
                    CurrentWeather = weather,
                    InventoryItems = held
                });
                if (IsDebtRepaymentRouteBlocked(debt, result, routes.Resolve(gate.TargetId)))
                    return gate.Id;
            }
            return null;
        }
    }
}
