// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class ShelterOriginDef
    {
        public string origin_id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public List<string> starting_bonuses = new List<string>();
        public List<string> starting_drawbacks = new List<string>();
        public int radiation_shielding_bp;
        public int ventilation_bonus_bp;
        public int space_modifier_bp;
        public string flavor_text = string.Empty;
    }

    [Serializable]
    public sealed class ShelterOriginsCatalogJson
    {
        public int schema_version = 1;
        public List<ShelterOriginDef> origins = new List<ShelterOriginDef>();
    }

    [Serializable]
    public sealed class ShelterIdentityState
    {
        public int schema_version = 1;
        public string shelter_name = "The Shelter";
        public string origin_id = string.Empty;
        public int founding_day = 1;
        public string founder_survivor_id = string.Empty;
        public string motto = string.Empty;
        public string emblem_color = "amber";
        public string emblem_symbol = "shield";
        public Dictionary<string, int> reputation_by_faction = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public int infamy_score; // 0 to 100
        public int reputation_points_trade;
        public int reputation_points_raid;
        public int reputation_points_medical;
        public int reputation_points_isolation;
    }

    /// <summary>
    /// Plan 166 / C1[28] / DEC-108: Shelter Identity & Naming System.
    /// Governs community name, founding origin traits, emblems, mottos,
    /// faction reputations, infamy, and emergent known-for reputation tags.
    /// Pure domain engine, completely engine-free.
    /// </summary>
    public sealed class ShelterIdentitySystem
    {
        public const string SystemId = "shelter_identity";
        public const string DefaultShelterName = "The Shelter";

        private readonly Dictionary<string, ShelterOriginDef> _origins =
            new Dictionary<string, ShelterOriginDef>(StringComparer.OrdinalIgnoreCase);

        private readonly ShelterIdentityState _state = new ShelterIdentityState();

        public event Action<string>? OnShelterNamed; // newName
        public event Action<string>? OnOriginSelected; // originId
        public event Action<string, int>? OnFactionReputationChanged; // factionId, newRep
        public event Action<int>? OnInfamyChanged; // newInfamy

        public ShelterIdentityState State => _state;
        public IReadOnlyDictionary<string, ShelterOriginDef> Origins => _origins;
        public string ShelterName => string.IsNullOrWhiteSpace(_state.shelter_name) ? DefaultShelterName : _state.shelter_name;
        public string OriginId => _state.origin_id;
        public string Motto => _state.motto;
        public int Infamy => _state.infamy_score;

        public ShelterIdentitySystem(string? catalogJson = null, IJsonSerializer? serializer = null)
        {
            if (!string.IsNullOrWhiteSpace(catalogJson) && serializer != null)
            {
                LoadCatalog(catalogJson, serializer);
            }
            else
            {
                RegisterDefaultOrigins();
            }
        }

        public void LoadCatalog(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(json) || serializer == null) return;
            try
            {
                var cat = serializer.Deserialize<ShelterOriginsCatalogJson>(json);
                if (cat?.origins != null)
                {
                    foreach (var o in cat.origins)
                    {
                        RegisterOrigin(o);
                    }
                }
            }
            catch
            {
                RegisterDefaultOrigins();
            }
        }

        public void RegisterOrigin(ShelterOriginDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.origin_id)) return;
            _origins[def.origin_id] = def;
        }

        private void RegisterDefaultOrigins()
        {
            RegisterOrigin(new ShelterOriginDef
            {
                origin_id = "origin_government_bunker",
                display_name = "Government Continuity Bunker",
                description = "A deep, reinforced civil defense installation designed for continuity of government.",
                starting_bonuses = new List<string> { "radiation_shielding_bonus", "air_filtration_bonus" },
                starting_drawbacks = new List<string> { "cramped_quarters", "restricted_surface_access" },
                radiation_shielding_bp = 2500,
                ventilation_bonus_bp = 1500,
                space_modifier_bp = -1000,
                flavor_text = "Heavy blast doors and thick lead-lined concrete offer superior protection against fallout."
            });

            RegisterOrigin(new ShelterOriginDef
            {
                origin_id = "origin_mining_facility",
                display_name = "Subterranean Mining Facility",
                description = "An expansive industrial extraction complex cut deep into bedrock.",
                starting_bonuses = new List<string> { "mining_yield_bonus", "subterranean_space_bonus" },
                starting_drawbacks = new List<string> { "poor_ventilation", "scarce_groundwater" },
                radiation_shielding_bp = 1000,
                ventilation_bonus_bp = -1500,
                space_modifier_bp = 2500,
                flavor_text = "Vast hollows and drill tracks provide boundless space for expansion."
            });

            RegisterOrigin(new ShelterOriginDef
            {
                origin_id = "origin_school_basement",
                display_name = "Community School Shelter",
                description = "A repurposed reinforced public school basement where communal solidarity runs high.",
                starting_bonuses = new List<string> { "education_speed_bonus", "communal_morale_bonus" },
                starting_drawbacks = new List<string> { "light_shielding", "limited_defenses" },
                radiation_shielding_bp = -1500,
                ventilation_bonus_bp = 0,
                space_modifier_bp = 1000,
                flavor_text = "Chalkboards and book crates foster an ethos of collective duty."
            });

            RegisterOrigin(new ShelterOriginDef
            {
                origin_id = "origin_private_vault",
                display_name = "Executive Private Vault",
                description = "A luxury pre-war redoubt engineered with high-grade components.",
                starting_bonuses = new List<string> { "luxury_stock_bonus", "advanced_components_bonus" },
                starting_drawbacks = new List<string> { "minimal_footprint", "suspicious_nomads" },
                radiation_shielding_bp = 1500,
                ventilation_bonus_bp = 1000,
                space_modifier_bp = -2000,
                flavor_text = "Finely engineered filtration and brass fittings ensure luxury."
            });

            RegisterOrigin(new ShelterOriginDef
            {
                origin_id = "origin_improvised_cellar",
                display_name = "Improvised Storm Cellar",
                description = "A hastily fortified underground root cellar bolstered by salvaged corrugated sheeting.",
                starting_bonuses = new List<string> { "rapid_surface_access", "scavenger_ingenuity_bonus" },
                starting_drawbacks = new List<string> { "fragile_insulation", "minimal_rations" },
                radiation_shielding_bp = -2000,
                ventilation_bonus_bp = 500,
                space_modifier_bp = -500,
                flavor_text = "Built on grit and scavenged scrap, rapid access lets scavengers deploy swiftly."
            });

            RegisterOrigin(new ShelterOriginDef
            {
                origin_id = "origin_military_outpost",
                display_name = "Hardened Military Outpost",
                description = "A decommissioned perimeter outpost built for defensive containment.",
                starting_bonuses = new List<string> { "perimeter_defense_bonus", "ordnance_cache_bonus" },
                starting_drawbacks = new List<string> { "austere_quarters", "scarce_medical_supplies" },
                radiation_shielding_bp = 1500,
                ventilation_bonus_bp = 500,
                space_modifier_bp = 500,
                flavor_text = "Stout firing slits and reinforced perimeter emplacements repel raiders."
            });
        }

        // ── Identity Operations ───────────────────────────────────

        public ActionResult SetShelterName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return ActionResult.Blocked("empty_name", "shelter_identity.empty_name");

            string trimmed = name.Trim();
            if (trimmed.Length < 2 || trimmed.Length > 40)
                return ActionResult.Blocked("invalid_length", "shelter_identity.invalid_length");

            _state.shelter_name = trimmed;
            OnShelterNamed?.Invoke(trimmed);
            return ActionResult.Success("shelter_identity.name_updated");
        }

        public ActionResult SetMotto(string motto)
        {
            string clean = (motto ?? string.Empty).Trim();
            if (clean.Length > 120)
                return ActionResult.Blocked("motto_too_long", "shelter_identity.motto_too_long");

            _state.motto = clean;
            return ActionResult.Success("shelter_identity.motto_updated");
        }

        public ActionResult SetEmblem(string symbol, string color)
        {
            if (string.IsNullOrWhiteSpace(symbol) || string.IsNullOrWhiteSpace(color))
                return ActionResult.Blocked("invalid_emblem", "shelter_identity.invalid_emblem");

            _state.emblem_symbol = symbol.Trim().ToLowerInvariant();
            _state.emblem_color = color.Trim().ToLowerInvariant();
            return ActionResult.Success("shelter_identity.emblem_updated");
        }

        public ActionResult SelectOrigin(string originId, int day = 1, string founderSurvivorId = "")
        {
            if (string.IsNullOrEmpty(originId))
                return ActionResult.Blocked("missing_origin", "shelter_identity.missing_origin");

            if (!_origins.ContainsKey(originId))
                return ActionResult.Blocked("unknown_origin", "shelter_identity.unknown_origin");

            _state.origin_id = originId;
            _state.founding_day = Math.Max(1, day);
            _state.founder_survivor_id = founderSurvivorId ?? string.Empty;

            OnOriginSelected?.Invoke(originId);
            return ActionResult.Success("shelter_identity.origin_selected");
        }

        public ShelterOriginDef? GetSelectedOrigin()
        {
            if (string.IsNullOrEmpty(_state.origin_id)) return null;
            _origins.TryGetValue(_state.origin_id, out var def);
            return def;
        }

        // ── Reputation & Community Legacy ─────────────────────────

        public void RecordFactionReputation(string factionId, int delta)
        {
            if (string.IsNullOrEmpty(factionId) || delta == 0) return;

            int current = _state.reputation_by_faction.TryGetValue(factionId, out var val) ? val : 0;
            int updated = Math.Max(-1000, Math.Min(1000, current + delta));
            _state.reputation_by_faction[factionId] = updated;

            OnFactionReputationChanged?.Invoke(factionId, updated);
        }

        public int GetFactionReputation(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return 0;
            return _state.reputation_by_faction.TryGetValue(factionId, out var val) ? val : 0;
        }

        public void RecordCommunityAction(string actionType, int magnitude = 1)
        {
            if (string.IsNullOrEmpty(actionType) || magnitude <= 0) return;

            switch (actionType.ToLowerInvariant())
            {
                case "trade":
                case "barter":
                    _state.reputation_points_trade += magnitude;
                    break;
                case "raid":
                case "combat":
                    _state.reputation_points_raid += magnitude;
                    AdjustInfamy(magnitude * 2);
                    break;
                case "medical":
                case "heal":
                case "rescue":
                    _state.reputation_points_medical += magnitude;
                    AdjustInfamy(-magnitude);
                    break;
                case "isolation":
                case "refusal":
                    _state.reputation_points_isolation += magnitude;
                    break;
            }
        }

        public void AdjustInfamy(int delta)
        {
            int prev = _state.infamy_score;
            _state.infamy_score = Math.Max(0, Math.Min(100, _state.infamy_score + delta));

            if (prev != _state.infamy_score)
            {
                OnInfamyChanged?.Invoke(_state.infamy_score);
            }
        }

        public List<string> GetKnownForTags()
        {
            var tags = new List<string>();

            if (_state.reputation_points_trade >= 5)
                tags.Add("Traders");
            if (_state.reputation_points_medical >= 5)
                tags.Add("Healers");
            if (_state.reputation_points_raid >= 5)
                tags.Add("Raiders");
            if (_state.reputation_points_isolation >= 5)
                tags.Add("Hermits");

            if (tags.Count == 0)
            {
                tags.Add("Survivors");
            }

            return tags;
        }

        // ── Text Token Formatting ─────────────────────────────────

        public string FormatText(string template)
        {
            if (string.IsNullOrEmpty(template)) return string.Empty;

            var origin = GetSelectedOrigin();
            string originName = origin != null ? origin.display_name : "Unknown Origin";

            return template
                .Replace("{shelter_name}", ShelterName)
                .Replace("{motto}", _state.motto)
                .Replace("{origin_name}", originName)
                .Replace("{founder}", string.IsNullOrEmpty(_state.founder_survivor_id) ? "Founding Council" : _state.founder_survivor_id);
        }

        // ── Save / Load ───────────────────────────────────────────

        public ShelterIdentityState CaptureState()
        {
            var copy = new ShelterIdentityState
            {
                schema_version = _state.schema_version,
                shelter_name = _state.shelter_name,
                origin_id = _state.origin_id,
                founding_day = _state.founding_day,
                founder_survivor_id = _state.founder_survivor_id,
                motto = _state.motto,
                emblem_color = _state.emblem_color,
                emblem_symbol = _state.emblem_symbol,
                reputation_by_faction = new Dictionary<string, int>(_state.reputation_by_faction, StringComparer.OrdinalIgnoreCase),
                infamy_score = _state.infamy_score,
                reputation_points_trade = _state.reputation_points_trade,
                reputation_points_raid = _state.reputation_points_raid,
                reputation_points_medical = _state.reputation_points_medical,
                reputation_points_isolation = _state.reputation_points_isolation
            };
            return copy;
        }

        public void RestoreState(ShelterIdentityState? saved)
        {
            if (saved == null) return;

            _state.schema_version = saved.schema_version;
            _state.shelter_name = string.IsNullOrWhiteSpace(saved.shelter_name) ? DefaultShelterName : saved.shelter_name;
            _state.origin_id = saved.origin_id ?? string.Empty;
            _state.founding_day = saved.founding_day;
            _state.founder_survivor_id = saved.founder_survivor_id ?? string.Empty;
            _state.motto = saved.motto ?? string.Empty;
            _state.emblem_color = saved.emblem_color ?? "amber";
            _state.emblem_symbol = saved.emblem_symbol ?? "shield";
            _state.infamy_score = Math.Max(0, Math.Min(100, saved.infamy_score));
            _state.reputation_points_trade = saved.reputation_points_trade;
            _state.reputation_points_raid = saved.reputation_points_raid;
            _state.reputation_points_medical = saved.reputation_points_medical;
            _state.reputation_points_isolation = saved.reputation_points_isolation;

            _state.reputation_by_faction.Clear();
            if (saved.reputation_by_faction != null)
            {
                foreach (var kvp in saved.reputation_by_faction)
                {
                    _state.reputation_by_faction[kvp.Key] = kvp.Value;
                }
            }
        }
    }
}
