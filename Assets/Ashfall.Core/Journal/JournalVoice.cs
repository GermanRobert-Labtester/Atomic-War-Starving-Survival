namespace Ashfall.Core.Journal
{
    /// <summary>
    /// Trait-driven diegetic copy for journal discoveries. Cold, exhausted,
    /// human — no fourth-wall tutorial language. Tone shifts with RiskBiasTrait.
    /// </summary>
    public static class JournalVoice
    {
        private static JournalVoiceProseCatalog? _catalog;

        public static void BindCatalog(JournalVoiceProseCatalog? catalog)
        {
            _catalog = catalog;
        }

        public static JournalVoiceProseCatalog? GetCatalog() => _catalog;

        /// <summary>
        /// Compose body text (without leading "Day N.") for a knowledge key.
        /// Falls back to a default message if the catalog is missing or the key is unknown.
        /// </summary>
        public static string ComposeBody(string knowledgeKey, RiskBiasTrait bias)
        {
            if (_catalog != null && _catalog.HasKey(knowledgeKey))
            {
                return _catalog.GetProse(knowledgeKey, bias);
            }

            // Fallback: catalog not bound or key not found
            return "Something changed. I wrote it down so I would not forget.";
        }

        /// <summary>
        /// Full entry text: "Day N. …" matching the acceptance example shape.
        /// </summary>
        public static string ComposeFullText(string knowledgeKey, RiskBiasTrait bias, int day)
        {
            string body = ComposeBody(knowledgeKey, bias);
            if (string.IsNullOrEmpty(body)) body = "I marked the day. That is all.";
            int d = day > 0 ? day : 1;
            // Body may already start with a sentence; prefix day stamp once.
            if (body.StartsWith("Day "))
                return body;
            return $"Day {d}. {body}";
        }

        public static string FormatTimestamp(int day, float hour = -1f)
        {
            int d = day > 0 ? day : 1;
            if (hour < 0f) return $"Day {d}";
            int h = (int)hour;
            if (h < 0) h = 0;
            if (h > 23) h = h % 24;
            return $"Day {d}, {h:00}h";
        }
    }
}
