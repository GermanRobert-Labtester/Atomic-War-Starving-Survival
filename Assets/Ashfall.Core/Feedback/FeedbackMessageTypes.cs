using System;
using System.Collections.Generic;

namespace Ashfall.Core.Feedback
{
    public enum FeedbackSeverity
    {
        Info = 0,
        Success = 1,
        Warning = 2,
        Error = 3,
        Critical = 4
    }

    [Serializable]
    public class FeedbackMessageTemplate
    {
        public string key { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public string severity { get; set; } = "info";
        public string template { get; set; } = string.Empty;
        public int parameter_count { get; set; }
        public float display_duration_seconds { get; set; } = 3.0f;

        public FeedbackSeverity GetSeverity()
        {
            if (Enum.TryParse<FeedbackSeverity>(severity, true, out var parsed))
                return parsed;
            return FeedbackSeverity.Info;
        }
    }

    [Serializable]
    public class FeedbackMessageContainer
    {
        public int schema_version { get; set; } = 1;
        public List<FeedbackMessageTemplate> messages { get; set; } = new List<FeedbackMessageTemplate>();
    }
}
