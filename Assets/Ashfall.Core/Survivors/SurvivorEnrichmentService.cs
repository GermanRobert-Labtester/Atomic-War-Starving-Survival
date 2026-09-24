// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Pure C# read-only projection model for survivor identity.
    /// Composes canonical definition metadata with authored static enrichment
    /// (professions, beliefs, keepsakes, phantom backgrounds).
    /// Adheres to Plan 137: non-mechanical, descriptive, zero stat inflation.
    /// </summary>
    public sealed class SurvivorEnrichmentView
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string ProfessionId { get; set; } = string.Empty;
        public string ProfessionLabel { get; set; } = string.Empty;
        public string BeliefProfileId { get; set; } = string.Empty;
        public string BeliefProfileLabel { get; set; } = string.Empty;
        public string PersonalKeepsakeItemId { get; set; } = string.Empty;
        public string KeepsakeItemLabel { get; set; } = string.Empty;
        public string PhantomBackgroundId { get; set; } = string.Empty;
        public string PhantomBackgroundLabel { get; set; } = string.Empty;
        public string PhilosophicalStance { get; set; } = string.Empty;
        public string ManifestoLawCode { get; set; } = string.Empty;
        public IReadOnlyList<string> KeepsakeItemTags { get; set; } = Array.Empty<string>();
        public string PrimaryDutyAffinity { get; set; } = string.Empty;
        public int DutyComfortBonusPermille { get; set; }
        public string IdeologicalTensionBeliefId { get; set; } = string.Empty;
        public IReadOnlyList<string> CorePersonalityTraits { get; set; } = Array.Empty<string>();
        public bool IsEnriched { get; set; }
    }

    /// <summary>
    /// C3-174: Bounded mechanical origin modifier resolved from enriched survivor identity.
    /// Provides deterministic starting skill affinity, trade specialty, or keepsake item grant
    /// without fabricating identity or duplicating progression systems.
    /// </summary>
    public sealed class SurvivorOriginModifier
    {
        public string SurvivorId { get; }
        public string PrimarySkillId { get; }
        public int SkillBonus { get; }
        public string TradeSpecialtyId { get; }
        public string GrantedKeepsakeItemId { get; }
        public bool HasMechanicalOrigin { get; }

        public SurvivorOriginModifier(
            string survivorId,
            string primarySkillId,
            int skillBonus,
            string tradeSpecialtyId,
            string grantedKeepsakeItemId,
            bool hasMechanicalOrigin)
        {
            SurvivorId = survivorId ?? string.Empty;
            PrimarySkillId = primarySkillId ?? string.Empty;
            SkillBonus = skillBonus;
            TradeSpecialtyId = tradeSpecialtyId ?? string.Empty;
            GrantedKeepsakeItemId = grantedKeepsakeItemId ?? string.Empty;
            HasMechanicalOrigin = hasMechanicalOrigin;
        }

        public static SurvivorOriginModifier Empty(string survivorId) =>
            new SurvivorOriginModifier(survivorId, string.Empty, 0, string.Empty, string.Empty, false);
    }

    /// <summary>
    /// C2[17] / Plan 40A: Rich diegetic observability read model for survivor identity,
    /// declared item tags, duty affinities, and interpersonal ideological tensions.
    /// Exposes truthful presentation without omniscience or runtime RNG.
    /// </summary>
    public sealed class SurvivorIdentityObservabilitySlate
    {
        public string SurvivorId { get; }
        public string DisplayName { get; }
        public string ProfessionLabel { get; }
        public string BeliefProfileLabel { get; }
        public string KeepsakeItemLabel { get; }
        public IReadOnlyList<string> KeepsakeItemTags { get; }
        public string PrimaryDutyAffinity { get; }
        public int DutyComfortBonusPermille { get; }
        public string IdeologicalTensionBeliefId { get; }
        public string FrictionOpponentLabel { get; }
        public IReadOnlyList<string> CorePersonalityTraits { get; }
        public string PhilosophicalStance { get; }
        public string ManifestoLawCode { get; }
        public bool IsEnriched { get; }

        public SurvivorIdentityObservabilitySlate(
            string survivorId,
            string displayName,
            string professionLabel,
            string beliefProfileLabel,
            string keepsakeItemLabel,
            IReadOnlyList<string> keepsakeItemTags,
            string primaryDutyAffinity,
            int dutyComfortBonusPermille,
            string ideologicalTensionBeliefId,
            string frictionOpponentLabel,
            IReadOnlyList<string> corePersonalityTraits,
            string philosophicalStance,
            string manifestoLawCode,
            bool isEnriched)
        {
            SurvivorId = survivorId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            ProfessionLabel = professionLabel ?? string.Empty;
            BeliefProfileLabel = beliefProfileLabel ?? string.Empty;
            KeepsakeItemLabel = keepsakeItemLabel ?? string.Empty;
            KeepsakeItemTags = keepsakeItemTags ?? Array.Empty<string>();
            PrimaryDutyAffinity = primaryDutyAffinity ?? string.Empty;
            DutyComfortBonusPermille = dutyComfortBonusPermille;
            IdeologicalTensionBeliefId = ideologicalTensionBeliefId ?? string.Empty;
            FrictionOpponentLabel = frictionOpponentLabel ?? string.Empty;
            CorePersonalityTraits = corePersonalityTraits ?? Array.Empty<string>();
            PhilosophicalStance = philosophicalStance ?? string.Empty;
            ManifestoLawCode = manifestoLawCode ?? string.Empty;
            IsEnriched = isEnriched;
        }

        public static SurvivorIdentityObservabilitySlate Unenriched(string survivorId, string displayName = "Unknown") =>
            new SurvivorIdentityObservabilitySlate(
                survivorId,
                displayName,
                "Unspecified",
                "Undeclared",
                "None",
                Array.Empty<string>(),
                "General Labor",
                0,
                string.Empty,
                "None",
                Array.Empty<string>(),
                string.Empty,
                string.Empty,
                false);
    }

    /// <summary>
    /// Engine-agnostic read-only projection service.
    /// Composes static definition metadata, campaign identity, and authored
    /// enrichment into presentation-ready views without mutating gameplay authorities.
    /// </summary>
    public sealed class SurvivorEnrichmentService
    {
        private readonly ExpansionEnrichmentCatalog _enrichment;
        private readonly Dictionary<string, SurvivorEnrichmentView> _viewCache =
            new Dictionary<string, SurvivorEnrichmentView>(StringComparer.Ordinal);

        public ExpansionEnrichmentCatalog Catalog => _enrichment;

        public SurvivorEnrichmentService(ExpansionEnrichmentCatalog? enrichment = null)
        {
            _enrichment = enrichment ?? new ExpansionEnrichmentCatalog();
        }

        public SurvivorEnrichmentView GetView(string survivorId, SurvivorDefinition? def = null)
        {
            if (string.IsNullOrEmpty(survivorId))
                return new SurvivorEnrichmentView { DisplayName = "Unknown" };

            if (_viewCache.TryGetValue(survivorId, out var cached))
                return cached;

            var fields = _enrichment.GetSurvivorFields(survivorId);
            var view = Project(survivorId, def, fields, _enrichment);
            _viewCache[survivorId] = view;
            return view;
        }

        /// <summary>
        /// C2[17] / Plan 40A: Projects rich diegetic identity observability slate for UI panels and host sessions.
        /// Unenriched survivors receive an unadorned truthful slate without simulated omniscience.
        /// </summary>
        public SurvivorIdentityObservabilitySlate GetObservabilitySlate(string survivorId, SurvivorDefinition? def = null)
        {
            var view = GetView(survivorId, def);
            if (!view.IsEnriched)
                return SurvivorIdentityObservabilitySlate.Unenriched(survivorId, view.DisplayName);

            var (_, frictionLabel) = ResolveFrictionBelief(view.BeliefProfileId);

            return new SurvivorIdentityObservabilitySlate(
                view.SurvivorId,
                view.DisplayName,
                view.ProfessionLabel,
                view.BeliefProfileLabel,
                view.KeepsakeItemLabel,
                view.KeepsakeItemTags,
                view.PrimaryDutyAffinity,
                view.DutyComfortBonusPermille,
                view.IdeologicalTensionBeliefId,
                frictionLabel,
                view.CorePersonalityTraits,
                view.PhilosophicalStance,
                view.ManifestoLawCode,
                view.IsEnriched);
        }

        /// <summary>
        /// C3-174: Resolves bounded mechanical origin effects (skill bonus, trade specialty, keepsake)
        /// from the survivor's enriched identity. Unenriched survivors receive an empty modifier.
        /// </summary>
        public SurvivorOriginModifier GetOriginModifier(string survivorId, SurvivorDefinition? def = null)
        {
            var view = GetView(survivorId, def);
            if (!view.IsEnriched)
                return SurvivorOriginModifier.Empty(survivorId);

            string skillId = ResolveSkillFromProfession(view.ProfessionId);
            int skillBonus = !string.IsNullOrEmpty(skillId) ? 1 : 0;
            string tradeSpecialty = ResolveTradeSpecialty(view.ProfessionId, view.PhantomBackgroundId);
            string keepsakeItem = !string.IsNullOrEmpty(view.PersonalKeepsakeItemId)
                ? view.PersonalKeepsakeItemId
                : string.Empty;

            bool hasOrigin = skillBonus > 0 || !string.IsNullOrEmpty(tradeSpecialty) || !string.IsNullOrEmpty(keepsakeItem);

            return new SurvivorOriginModifier(
                survivorId,
                skillId,
                skillBonus,
                tradeSpecialty,
                keepsakeItem,
                hasOrigin);
        }

        public static string ResolveSkillFromProfession(string profId) => profId switch
        {
            "nurse" => "medical",
            "machinist" => "mechanic",
            "metallurgist" => "mechanic",
            "electrician" => "electronics",
            "teacher" => "research",
            "farmer" => "agriculture",
            "miner" => "excavation",
            "soldier" or "former_soldier" => "combat",
            _ => string.Empty
        };

        public static string ResolveTradeSpecialty(string profId, string phantomId)
        {
            if (profId == "machinist" || phantomId == "machinist") return "tools";
            if (profId == "metallurgist") return "tools";
            if (profId == "nurse" || phantomId == "nurse") return "medical";
            if (profId == "electrician" || phantomId == "electrician") return "components";
            if (profId == "farmer") return "food";
            return string.Empty;
        }

        public static (string Affinity, int ComfortBonusPermille) ResolveDutyAffinity(string profId) => profId switch
        {
            "nurse" => ("Medical Ward", 150),
            "machinist" => ("Shelter Workshop", 150),
            "electrician" => ("Microgrid Maintenance", 150),
            "farmer" => ("Glasshouse Agriculture", 150),
            "miner" => ("Excavation & Shoring", 150),
            "teacher" => ("Archive & Instruction", 150),
            "soldier" or "former_soldier" => ("Perimeter Guard", 150),
            _ => ("General Labor", 0)
        };

        public static (string TensionBeliefId, string TensionLabel) ResolveFrictionBelief(string beliefId) => beliefId switch
        {
            "atheist_rationalist" => ("religious_faith", "Spiritual Faith"),
            "religious_faith" => ("atheist_rationalist", "Material Rationalism"),
            "military_discipline" => ("pacifist", "Nonviolent Pacifism"),
            "pacifist" => ("military_discipline", "Military Discipline"),
            "collectivist_solidarity" => ("pragmatic_individualism", "Pragmatic Individualism"),
            "pragmatic_individualism" => ("collectivist_solidarity", "Collectivist Solidarity"),
            "superstitious_traditional" => ("atheist_rationalist", "Material Rationalism"),
            _ => (string.Empty, "None")
        };

        public static IReadOnlyList<string> ResolvePersonalityTraits(string profId, string beliefId, string phantomId)
        {
            var traits = new List<string>(3);

            switch (profId)
            {
                case "nurse":
                    traits.Add("Compassionate");
                    break;
                case "machinist":
                    traits.Add("Methodical");
                    break;
                case "electrician":
                    traits.Add("Analytical");
                    break;
                case "teacher":
                    traits.Add("Inquisitive");
                    break;
                case "farmer":
                    traits.Add("Patient");
                    break;
                case "miner":
                    traits.Add("Enduring");
                    break;
                case "soldier" or "former_soldier":
                    traits.Add("Vigilant");
                    break;
                default:
                    traits.Add("Pragmatic");
                    break;
            }

            switch (beliefId)
            {
                case "atheist_rationalist":
                    traits.Add("Empirical");
                    break;
                case "religious_faith":
                    traits.Add("Devout");
                    break;
                case "military_discipline":
                    traits.Add("Disciplined");
                    break;
                case "pacifist":
                    traits.Add("Harm-Averse");
                    break;
                case "collectivist_solidarity":
                    traits.Add("Communal");
                    break;
                case "pragmatic_individualism":
                    traits.Add("Self-Reliant");
                    break;
                case "superstitious_traditional":
                    traits.Add("Traditionalist");
                    break;
                default:
                    traits.Add("Adaptable");
                    break;
            }

            switch (phantomId)
            {
                case "child_refugee":
                    traits.Add("Wary");
                    break;
                case "former_soldier":
                    traits.Add("Guarded");
                    break;
                case "nurse":
                    traits.Add("Attentive");
                    break;
                case "machinist":
                    traits.Add("Resourceful");
                    break;
                default:
                    traits.Add("Resilient");
                    break;
            }

            return traits;
        }

        private static SurvivorEnrichmentView Project(
            string survivorId,
            SurvivorDefinition? def,
            ExpansionSurvivorFields? fields,
            ExpansionEnrichmentCatalog? catalog = null)
        {
            string name = !string.IsNullOrEmpty(def?.displayName) ? def.displayName : FormatId(survivorId);
            bool isEnriched = fields != null;

            // Profession resolution: explicit pre-war profession ID, fallback to definition's profession string
            string profId = fields?.pre_war_profession_id ?? string.Empty;
            string profLabel;
            if (!string.IsNullOrEmpty(profId))
            {
                profLabel = FormatProfession(profId);
            }
            else if (!string.IsNullOrEmpty(def?.profession))
            {
                profLabel = def.profession;
            }
            else
            {
                profLabel = "Unspecified";
            }

            // Belief profile resolution
            string beliefId = fields?.belief_profile_id ?? string.Empty;
            string beliefLabel = !string.IsNullOrEmpty(beliefId)
                ? FormatBelief(beliefId)
                : "Undeclared";

            // Keepsake resolution
            string keepsakeId = fields?.personal_keepsake_item_id ?? string.Empty;
            string keepsakeLabel = !string.IsNullOrEmpty(keepsakeId)
                ? FormatId(keepsakeId)
                : "None";

            var itemTags = catalog != null && !string.IsNullOrEmpty(keepsakeId)
                ? catalog.GetItemTags(keepsakeId)?.tags ?? new List<string>()
                : new List<string>();

            // Phantom background resolution
            string phantomId = fields?.phantom_background_id ?? string.Empty;
            string phantomLabel = !string.IsNullOrEmpty(phantomId)
                ? FormatPhantom(phantomId)
                : "Survivor";

            var (dutyAffinity, dutyComfort) = ResolveDutyAffinity(profId);
            var (frictionBeliefId, _) = ResolveFrictionBelief(beliefId);
            var personalityTraits = ResolvePersonalityTraits(profId, beliefId, phantomId);

            return new SurvivorEnrichmentView
            {
                SurvivorId = survivorId,
                DisplayName = name,
                ProfessionId = profId,
                ProfessionLabel = profLabel,
                BeliefProfileId = beliefId,
                BeliefProfileLabel = beliefLabel,
                PersonalKeepsakeItemId = keepsakeId,
                KeepsakeItemLabel = keepsakeLabel,
                KeepsakeItemTags = itemTags,
                PhantomBackgroundId = phantomId,
                PhantomBackgroundLabel = phantomLabel,
                PhilosophicalStance = fields?.philosophical_stance ?? string.Empty,
                ManifestoLawCode = fields?.manifesto_law_code ?? string.Empty,
                PrimaryDutyAffinity = dutyAffinity,
                DutyComfortBonusPermille = dutyComfort,
                IdeologicalTensionBeliefId = frictionBeliefId,
                CorePersonalityTraits = personalityTraits,
                IsEnriched = isEnriched
            };
        }

        public static string FormatProfession(string profId) => profId switch
        {
            "nurse" => "Registered Nurse",
            "machinist" => "Industrial Machinist",
            "electrician" => "High-Voltage Electrician",
            "teacher" => "Secondary Educator",
            _ => FormatId(profId)
        };

        public static string FormatBelief(string beliefId) => beliefId switch
        {
            "atheist_rationalist" => "Material Rationalism",
            "collectivist_solidarity" => "Collectivist Solidarity",
            "military_discipline" => "Military Discipline",
            "pacifist" => "Nonviolent Pacifism",
            "pragmatic_individualism" => "Pragmatic Individualism",
            "religious_faith" => "Spiritual Faith",
            "superstitious_traditional" => "Traditional Observance",
            _ => FormatId(beliefId)
        };

        public static string FormatPhantom(string phantomId) => phantomId switch
        {
            "child_refugee" => "Displaced Youth",
            "driver" => "Transport Driver",
            "electrician" => "Grid Systems",
            "former_soldier" => "Defense Veteran",
            "generic" => "Civilian",
            "machinist" => "Tool & Die Crafts",
            "miner" => "Deep Ore Extraction",
            "nurse" => "Clinical Care",
            "teacher" => "Instruction & Lore",
            _ => FormatId(phantomId)
        };

        public static string FormatId(string id)
        {
            if (string.IsNullOrEmpty(id)) return string.Empty;
            var parts = id.Split('_');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                    parts[i] = char.ToUpperInvariant(parts[i][0]) + parts[i].Substring(1).ToLowerInvariant();
            }
            return string.Join(" ", parts);
        }
    }
}
