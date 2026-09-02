using System;
using System.Collections.Generic;

namespace Ashfall.Core.UI
{
    public enum CrisisSeverity
    {
        None = 0,
        Advisory = 1,
        Elevated = 2,
        Warning = 3,
        Severe = 4,
        Critical = 5,
        Catastrophic = 6,
        Terminal = 7
    }

    public sealed class CrisisPresentationSnapshot
    {
        public string CrisisId { get; set; } = "";
        public string Kind { get; set; } = "";
        public CrisisSeverity Severity { get; set; } = CrisisSeverity.None;
        public bool IsActive { get; set; }

        public string Title { get; set; } = "";
        public string HeaderText
        {
            get => Title;
            set => Title = value;
        }

        public string Summary { get; set; } = "";
        public string SubheaderText
        {
            get => Summary;
            set => Summary = value;
        }

        public string Cause { get; set; } = "";
        public string CauseText
        {
            get => Cause;
            set => Cause = value;
        }

        public string EffectText { get; set; } = "";

        public float Progress01 { get; set; }
        public float? SecondsRemaining { get; set; }

        public List<CrisisMetricView> Metrics { get; set; } = new List<CrisisMetricView>();

        public List<CrisisAffectedEntityView> Affected { get; set; } = new List<CrisisAffectedEntityView>();
        public List<CrisisAffectedEntityView> AffectedEntities
        {
            get => Affected;
            set => Affected = value;
        }

        public List<CrisisActionView> Actions { get; set; } = new List<CrisisActionView>();
        public List<CrisisLogEntryView> Log { get; set; } = new List<CrisisLogEntryView>();

        public string AudioStateId { get; set; } = "";
    }

    public sealed class CrisisMetricView
    {
        public string Label { get; set; } = "";
        public string ValueText { get; set; } = "";
        public string Trend { get; set; } = "";
        public bool IsFailing { get; set; }
    }

    public sealed class CrisisAffectedEntityView
    {
        public string Name { get; set; } = "";
        public string Role { get; set; } = "";
        public string Status { get; set; } = "";
        public bool IsCritical { get; set; }
        public string NavigationTarget { get; set; } = "";
    }

    public sealed class CrisisActionView
    {
        public string ActionId { get; set; } = "";
        public string Label { get; set; } = "";
        public string CostText { get; set; } = "";
        public string ExpectedEffect { get; set; } = "";
        public string Warning { get; set; } = "";
        public string Shortcut { get; set; } = "";
        public bool IsEnabled { get; set; } = true;
    }

    public sealed class CrisisLogEntryView
    {
        public string Timestamp { get; set; } = "";
        public string Message { get; set; } = "";
        public bool IsError { get; set; }
    }
}
