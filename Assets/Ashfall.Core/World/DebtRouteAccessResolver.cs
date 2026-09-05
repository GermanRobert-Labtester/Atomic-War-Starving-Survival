using System;
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
    }
}
