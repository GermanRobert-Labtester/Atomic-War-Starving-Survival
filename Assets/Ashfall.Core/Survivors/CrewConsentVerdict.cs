// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Plan 43 / C1[13]: Typed result for crew consent, duty refusal, and governance agreements.
    /// Eliminates silent consent and boolean-only flags in assignment and policy decisions.
    /// </summary>
    [Serializable]
    public sealed class CrewConsentVerdict
    {
        public bool Consented { get; set; }
        public bool Warning { get; set; }
        public string RefusalReason { get; set; } = string.Empty;
        public string WarningReason { get; set; } = string.Empty;

        public static CrewConsentVerdict Accepted() =>
            new CrewConsentVerdict { Consented = true, Warning = false };

        public static CrewConsentVerdict AcceptedWithWarning(string warning) =>
            new CrewConsentVerdict { Consented = true, Warning = true, WarningReason = warning ?? string.Empty };

        public static CrewConsentVerdict Refused(string reason) =>
            new CrewConsentVerdict { Consented = false, Warning = false, RefusalReason = reason ?? "refused" };
    }
}
