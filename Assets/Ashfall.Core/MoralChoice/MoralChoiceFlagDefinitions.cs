using System.Collections.Generic;

namespace Ashfall.Core.MoralChoice
{
    /// <summary>
    /// Wire shape for moral_choice_flags.json — flag definitions that gate
    /// quest access and branch locking in the moral choice system.
    /// </summary>
    public sealed class MoralChoiceFlagDefinitions
    {
        public List<MoralFlagDefinition> Flags { get; set; } = new List<MoralFlagDefinition>();
    }

    public sealed class MoralFlagDefinition
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
