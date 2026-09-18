// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Commitments
{
    /// <summary>
    /// ASHFALL — Authored Commitment / Deadline Definition (Plan 38 §38C).
    /// Represents an obligation with start day, due window, counterparty,
    /// required quantity, and consequence routing.
    /// </summary>
    [Serializable]
    public sealed class CommitmentDefinition
    {
        public string id = string.Empty;
        public string type = string.Empty;
        public string title = string.Empty;
        public string counterparty = string.Empty;
        public int start_day = 1;
        public int due_day = 10;
        public int warning_lead_days = 3;
        public int target_quantity = 1;
        public string target_id = string.Empty;
        public string condition_type = "resource_delivery";
        public string consequence_class = "faction_standing_penalty";
        public string consequence_target = string.Empty;
        public int consequence_magnitude = -10;
    }

    [Serializable]
    public sealed class CommitmentCatalogData
    {
        public int schema_version = 1;
        public List<CommitmentDefinition> commitments = new List<CommitmentDefinition>();
    }
}
