namespace Ashfall.Core.Journal
{
    /// <summary>
    /// A survivor's characteristic bias in how they interpret radiation risk.
    /// Same world state, different felt danger — this is what makes two survivors
    /// in the same bunker act differently. See BeliefSystem.
    ///
    /// This is the complete set. The Godot port previously carried its own copy that
    /// stopped at <see cref="Fatalist"/>, so a survivor persisted as Empath or Sociopath
    /// deserialized into a value that host could not interpret. Members are persisted by
    /// ordinal — only ever append, never reorder or remove.
    /// </summary>
    public enum RiskBiasTrait
    {
        Paranoid,
        Cautious,
        Realist,
        Reckless,
        Denialist,
        Fatalist,

        /// <summary>Gains/loses morale based on the bunker's average morale.
        /// A fragile barometer for the group.</summary>
        Empath,

        /// <summary>Suffers zero morale loss when another survivor dies.
        /// Terrifies others but makes a perfect cold-blooded scavenger.</summary>
        Sociopath
    }

    /// <summary>
    /// Lightweight author view used by the journal. This decouples the journal from the full
    /// survivor class; anything with an id, a display name and a risk bias can write entries.
    /// </summary>
    public interface ISurvivorAuthor
    {
        string Id { get; }
        string DisplayName { get; }
        RiskBiasTrait RiskBias { get; }
    }
}
