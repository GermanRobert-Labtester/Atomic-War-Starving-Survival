namespace AtomicWar._Game.UI
{
    /// <summary>
    /// One row of an event prompt, flattened for drawing. Keeps DiegeticHudView
    /// independent of the Events namespace: the view needs a string and whether
    /// the row can be pressed, not a GameEvent graph.
    /// </summary>
    public readonly struct EventChoiceLine
    {
        public readonly string Text;
        public readonly bool IsEnabled;

        public EventChoiceLine(string text, bool isEnabled)
        {
            Text = text;
            IsEnabled = isEnabled;
        }
    }
}
