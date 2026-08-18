namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL — canonical expansion definition.
    /// Engine-agnostic identity used by the host aggregate gate, the
    /// completeness regression tests, and the hub UI. Never fork numbering
    /// here: this list is the single source of truth for which expansions are
    /// canonical and must appear in <c>--expansions-selftest</c>.
    /// </summary>
    public sealed class ExpansionDefinition
    {
        public int Number;
        public string Id = string.Empty;
        public string Name = string.Empty;
    }

    /// <summary>
    /// ASHFALL — canonical expansion registry (01–10).
    /// The <c>--expansions-selftest</c> aggregate is REQUIRED to cover every id
    /// in <see cref="Canonical"/>. A missing delegate for any canonical id fails
    /// the gate; <see cref="ExpansionAggregateCompletenessTests"/> in the xUnit
    /// suite pins this contract.
    /// </summary>
    public static class ExpansionSuite
    {
        public const int CanonicalCount = 10;

        public static readonly ExpansionDefinition[] Canonical =
        {
            new ExpansionDefinition { Number = 1,  Id = "expansion_01_holdfast",        Name = "The Holdfast" },
            new ExpansionDefinition { Number = 2,  Id = "expansion_02_duty_roster",     Name = "The Duty Roster" },
            new ExpansionDefinition { Number = 3,  Id = "expansion_03_standing_record", Name = "The Standing Record" },
            new ExpansionDefinition { Number = 4,  Id = "expansion_04_nobodys_charter", Name = "Nobody's Charter" },
            new ExpansionDefinition { Number = 5,  Id = "expansion_05_year_of_ash",     Name = "The Year of Ash" },
            new ExpansionDefinition { Number = 6,  Id = "expansion_06_muster",          Name = "The Muster" },
            new ExpansionDefinition { Number = 7,  Id = "expansion_07_the_dose",        Name = "The Dose / The Vigil" },
            new ExpansionDefinition { Number = 8,  Id = "expansion_08_the_verdict",     Name = "The Verdict" },
            new ExpansionDefinition { Number = 9,  Id = "expansion_09_black_flotilla",  Name = "The Black Flotilla" },
            new ExpansionDefinition { Number = 10, Id = "expansion_10_silent_foundry",  Name = "The Silent Foundry" },
        };

        /// <summary>True when the id is one of the ten canonical expansion ids.</summary>
        public static bool IsCanonical(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            for (int i = 0; i < Canonical.Length; i++)
                if (Canonical[i].Id == id) return true;
            return false;
        }
    }
}
