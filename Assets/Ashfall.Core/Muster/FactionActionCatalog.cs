using System.Collections.Generic;
#pragma warning disable CS0649
#pragma warning disable CS8618

namespace Ashfall.Core.Muster
{
    /// <summary>Standing band a faction-action variant is authored for. Computed
    /// from each faction system's own persisted scalar — never a second standing store.</summary>
    public static class FactionActionBands
    {
        public const string Hostile = "hostile";
        public const string Poor = "poor";
        public const string Neutral = "neutral";
        public const string Good = "good";
        public const string Allied = "allied";

        /// <summary>Deterministic fallthrough used by the board's band computation.</summary>
        public static readonly string[] All = { Hostile, Poor, Neutral, Good, Allied };
    }

    /// <summary>Mutual effects one choice applies when resolved. Faction systems
    /// ignore fields that do not apply to them (trust for guild/hydro, aggression
    /// for raiders, members/lockout for the coalition camp).</summary>
    public class FactionActionEffects
    {
        public float trustDelta;
        public float aggressionDelta;
        public int membersDelta;
        public int lockoutDelta;
        public string itemId = string.Empty;
        public int itemAmount;
        public List<string> flags = new List<string>();
        public string journal = string.Empty;
    }

    /// <summary>One selectable response inside a variant.</summary>
    public class FactionActionChoice
    {
        public string choiceId = string.Empty;
        public string text = string.Empty;
        public FactionActionEffects effects = new FactionActionEffects();
    }

    /// <summary>A standing-band-specific presentation of an action with its own choices.</summary>
    public class FactionActionVariant
    {
        public string band = FactionActionBands.Neutral;
        public string text = string.Empty;
        public List<FactionActionChoice> choices = new List<FactionActionChoice>();
    }

    /// <summary>One authored peacetime faction action (muster_faction_actions.json).</summary>
    public class FactionActionDefinition
    {
        public string id = string.Empty;
        public string factionId = string.Empty;
        public string title = string.Empty;
        public string text = string.Empty;
        public int minDay;
        public int maxDay;                 // 0 = unbounded
        public bool once;
        public int cooldownDays;
        public List<string> requiresFlags = new List<string>();
        public List<string> forbidsFlags = new List<string>();
        public List<FactionActionVariant> variants = new List<FactionActionVariant>();
    }

    /// <summary>
    /// Engine-agnostic loader for muster_faction_actions.json — Plan 25's authored
    /// peacetime faction ecology. Consumed only by FactionActionBoard. Missing file
    /// → empty list (optional catalog by design); parse failures route through
    /// CatalogDiagnostics; a schema_version beyond the known one is rejected.
    /// </summary>
    public static class FactionActionCatalogLoader
    {
        public const string FileName = "muster_faction_actions.json";
        public const int CurrentSchemaVersion = 1;

        public static List<FactionActionDefinition> LoadActions(
            string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<FactionActionDefinition>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return result;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return result;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return result;

            try
            {
                var root = json.Deserialize<FactionActionRoot>(raw);
                if (root == null) return result;
                if (root.schema_version > CurrentSchemaVersion)
                    return result;
                var entries = root.actions;
                if (entries == null) return result;
                for (int i = 0; i < entries.Count; i++)
                {
                    var e = entries[i];
                    if (e == null || string.IsNullOrEmpty(e.id) || string.IsNullOrEmpty(e.faction_id))
                        continue;
                    var def = new FactionActionDefinition
                    {
                        id = e.id,
                        factionId = e.faction_id,
                        title = e.title ?? string.Empty,
                        text = e.text ?? string.Empty,
                        minDay = e.min_day,
                        maxDay = e.max_day,
                        once = e.once,
                        cooldownDays = e.cooldown_days > 0 ? e.cooldown_days : 0
                    };
                    CopyList(e.requires_flags, def.requiresFlags);
                    CopyList(e.forbids_flags, def.forbidsFlags);
                    if (e.variants != null)
                    {
                        for (int v = 0; v < e.variants.Count; v++)
                        {
                            var ve = e.variants[v];
                            if (ve == null) continue;
                            var variant = new FactionActionVariant
                            {
                                band = string.IsNullOrEmpty(ve.band) ? FactionActionBands.Neutral : ve.band,
                                text = ve.text ?? string.Empty
                            };
                            if (ve.choices != null)
                            {
                                for (int c = 0; c < ve.choices.Count; c++)
                                {
                                    var ce = ve.choices[c];
                                    if (ce == null || string.IsNullOrEmpty(ce.choice_id)) continue;
                                    var fx = ce.effects ?? new EffectsEntry();
                                    variant.choices.Add(new FactionActionChoice
                                    {
                                        choiceId = ce.choice_id,
                                        text = ce.text ?? string.Empty,
                                        effects = new FactionActionEffects
                                        {
                                            trustDelta = fx.trust_delta,
                                            aggressionDelta = fx.aggression_delta,
                                            membersDelta = fx.members_delta,
                                            lockoutDelta = fx.lockout_delta,
                                            itemId = fx.item_id ?? string.Empty,
                                            itemAmount = fx.item_amount,
                                            journal = fx.journal ?? string.Empty
                                        }
                                    });
                                    CopyList(fx.flags, variant.choices[variant.choices.Count - 1].effects.flags);
                                }
                            }
                            def.variants.Add(variant);
                        }
                    }
                    result.Add(def);
                }
            }
            catch (System.Exception ex_CATDIAG)
            {
                Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "FactionActionRoot", ex_CATDIAG);
                return result;
            }
            return result;
        }

        private static void CopyList(List<string> source, List<string> target)
        {
            if (source == null) return;
            for (int i = 0; i < source.Count; i++)
                if (!string.IsNullOrEmpty(source[i])) target.Add(source[i]);
        }

        /// <summary>Schema-envelope root for muster_faction_actions.json.</summary>
        private class FactionActionRoot
        {
            public int schema_version = 1;
            public List<ActionEntry> actions = new List<ActionEntry>();
        }

        private class ActionEntry
        {
            public string id;
            public string faction_id;
            public string title;
            public string text;
            public int min_day;
            public int max_day;
            public bool once;
            public int cooldown_days;
            public List<string> requires_flags;
            public List<string> forbids_flags;
            public List<VariantEntry> variants;
        }

        private class VariantEntry
        {
            public string band;
            public string text;
            public List<ChoiceEntry> choices;
        }

        private class ChoiceEntry
        {
            public string choice_id;
            public string text;
            public EffectsEntry effects;
        }

        private class EffectsEntry
        {
            public float trust_delta;
            public float aggression_delta;
            public int members_delta;
            public int lockout_delta;
            public string item_id;
            public int item_amount;
            public List<string> flags;
            public string journal;
        }
    }
}
