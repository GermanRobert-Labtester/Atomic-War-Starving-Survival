using System;

namespace Ashfall.Core.Journal
{
    /// <summary>
    /// One diegetic journal line written by a survivor when the bunker
    /// discovers something new. Text + timestamp + author — no UI chrome.
    /// </summary>
    [Serializable]
    public class JournalEntry
    {
        public string Id;
        /// <summary>Body text in the author's voice (risk-bias flavoured).</summary>
        public string Text;
        /// <summary>Display timestamp, e.g. "Day 32".</summary>
        public string Timestamp;
        public string AuthorName;
        public string AuthorId;
        /// <summary>Knowledge key that triggered this entry (snake_case).</summary>
        public string KnowledgeKey;
        public int Day;
        public float Hour;

        public override string ToString()
        {
            string who = string.IsNullOrEmpty(AuthorName) ? "Someone" : AuthorName;
            string when = string.IsNullOrEmpty(Timestamp) ? $"Day {Day}" : Timestamp;
            return $"{when} — {who}: {Text}";
        }
    }
}
