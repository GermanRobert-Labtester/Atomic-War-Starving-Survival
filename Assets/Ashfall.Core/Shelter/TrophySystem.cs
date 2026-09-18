// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL: Trophy Mount Pipeline (Plan 14E / C1.6).
// Engine-free Core domain authority for rare quarry trophy mounts.
// Observes trapping / hunting harvests, awards trophy crafting opportunities
// exactly once, manages the award ledger, and links to ShelterDecorSystem.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Declarative trophy definition. Authoritative catalog data lives in trophies.json.
    /// </summary>
    [Serializable]
    public sealed class TrophyDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("species_id")]
        public string SpeciesId { get; set; } = string.Empty;

        [JsonPropertyName("trophy_item_id")]
        public string TrophyItemId { get; set; } = string.Empty;

        [JsonPropertyName("recipe_id")]
        public string RecipeId { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("localized_morale_delta")]
        public float LocalizedMoraleDelta { get; set; }

        [JsonPropertyName("recommended_room_id")]
        public string RecommendedRoomId { get; set; } = string.Empty;

        [JsonPropertyName("condition_kind")]
        public string ConditionKind { get; set; } = "quarry_harvested";
    }

    [Serializable]
    public sealed class TrophyCatalogRoot
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("collection_id")]
        public string CollectionId { get; set; } = "trophies_c1_6";

        [JsonPropertyName("trophies")]
        public List<TrophyDefinition> Trophies { get; set; } = new List<TrophyDefinition>();
    }

    /// <summary>
    /// Record emitted when a trophy is first awarded/unlocked.
    /// </summary>
    public sealed class TrophyAwardRecord
    {
        public string TrophyId { get; set; } = string.Empty;
        public string SpeciesId { get; set; } = string.Empty;
        public string RecipeId { get; set; } = string.Empty;
        public string ItemId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public float LocalizedMoraleDelta { get; set; }
        public int DayAwarded { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    /// <summary>
    /// Persistent state for the trophy award ledger.
    /// </summary>
    [Serializable]
    public sealed class TrophySaveState
    {
        public const string SystemId = "trophies";

        public string systemId = SystemId;
        public int schemaVersion = 1;
        public List<string> awardedTrophyIds = new List<string>();
        public List<string> unlockedRecipeIds = new List<string>();
    }

    /// <summary>
    /// Trophy system authority: manages the catalog, condition evaluation,
    /// and the exactly-once award ledger.
    /// </summary>
    public sealed class TrophySystem
    {
        public const string SystemId = "trophies";

        private readonly List<TrophyDefinition> _catalog = new List<TrophyDefinition>();
        private readonly Dictionary<string, TrophyDefinition> _byId = new Dictionary<string, TrophyDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, TrophyDefinition> _bySpecies = new Dictionary<string, TrophyDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, TrophyDefinition> _byItem = new Dictionary<string, TrophyDefinition>(StringComparer.Ordinal);

        private readonly TrophySaveState _state = new TrophySaveState();

        /// <summary>Fired only on the first award of a trophy opportunity (never on restore).</summary>
        public event Action<TrophyDefinition, TrophyAwardRecord>? OnTrophyReady;

        public IReadOnlyList<TrophyDefinition> Catalog => _catalog;
        public IReadOnlyList<string> AwardedTrophyIds => _state.awardedTrophyIds;
        public IReadOnlyList<string> UnlockedRecipeIds => _state.unlockedRecipeIds;

        public TrophySystem()
        {
            RegisterBaselineTrophies();
        }

        public void ClearCatalog()
        {
            _catalog.Clear();
            _byId.Clear();
            _bySpecies.Clear();
            _byItem.Clear();
        }

        public void RegisterTrophy(TrophyDefinition trophy)
        {
            if (trophy == null || string.IsNullOrWhiteSpace(trophy.Id)) return;
            if (_byId.TryGetValue(trophy.Id, out var existing))
            {
                int index = _catalog.IndexOf(existing);
                if (index >= 0) _catalog[index] = trophy;
            }
            else
            {
                _catalog.Add(trophy);
            }

            _byId[trophy.Id] = trophy;
            if (!string.IsNullOrEmpty(trophy.SpeciesId))
                _bySpecies[trophy.SpeciesId] = trophy;
            if (!string.IsNullOrEmpty(trophy.TrophyItemId))
                _byItem[trophy.TrophyItemId] = trophy;
        }

        public bool LoadCatalogFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return false;
            try
            {
                var root = JsonSerializer.Deserialize<TrophyCatalogRoot>(json);
                if (root?.Trophies == null) return false;
                ClearCatalog();
                foreach (var t in root.Trophies)
                {
                    RegisterTrophy(t);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public TrophyDefinition? GetTrophy(string trophyId)
        {
            if (string.IsNullOrEmpty(trophyId)) return null;
            return _byId.TryGetValue(trophyId, out var def) ? def : null;
        }

        public TrophyDefinition? GetTrophyForSpecies(string speciesId)
        {
            if (string.IsNullOrEmpty(speciesId)) return null;
            return _bySpecies.TryGetValue(speciesId, out var def) ? def : null;
        }

        public TrophyDefinition? GetTrophyByItemId(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            return _byItem.TryGetValue(itemId, out var def) ? def : null;
        }

        public string GetTrophyRecipeForSpecies(string speciesId)
        {
            var def = GetTrophyForSpecies(speciesId);
            return def?.RecipeId ?? string.Empty;
        }

        public float GetTrophyMoraleModifier(string itemId)
        {
            var def = GetTrophyByItemId(itemId);
            return def?.LocalizedMoraleDelta ?? 0f;
        }

        public bool IsAwarded(string trophyId)
        {
            if (string.IsNullOrEmpty(trophyId)) return false;
            return _state.awardedTrophyIds.Contains(trophyId);
        }

        /// <summary>
        /// Evaluates a quarry harvest event. If the species maps to an unearned
        /// trophy, records the award exactly once, unlocks the recipe, and fires OnTrophyReady.
        /// </summary>
        public TrophyAwardRecord? RecordQuarryPreserved(string speciesId, int day = 1)
        {
            var def = GetTrophyForSpecies(speciesId);
            if (def == null) return null;

            if (_state.awardedTrophyIds.Contains(def.Id))
            {
                // Exactly-once: already awarded
                return null;
            }

            _state.awardedTrophyIds.Add(def.Id);
            if (!string.IsNullOrEmpty(def.RecipeId) && !_state.unlockedRecipeIds.Contains(def.RecipeId))
            {
                _state.unlockedRecipeIds.Add(def.RecipeId);
            }

            var record = new TrophyAwardRecord
            {
                TrophyId = def.Id,
                SpeciesId = def.SpeciesId,
                RecipeId = def.RecipeId,
                ItemId = def.TrophyItemId,
                DisplayName = def.DisplayName,
                LocalizedMoraleDelta = def.LocalizedMoraleDelta,
                DayAwarded = Math.Max(1, day),
                Reason = $"Preserved rare {def.DisplayName} quarry specimen"
            };

            OnTrophyReady?.Invoke(def, record);
            return record;
        }

        public TrophySaveState CaptureState()
        {
            return new TrophySaveState
            {
                systemId = SystemId,
                schemaVersion = 1,
                awardedTrophyIds = new List<string>(_state.awardedTrophyIds),
                unlockedRecipeIds = new List<string>(_state.unlockedRecipeIds)
            };
        }

        public void RestoreState(TrophySaveState? saved)
        {
            _state.awardedTrophyIds.Clear();
            _state.unlockedRecipeIds.Clear();

            if (saved?.awardedTrophyIds != null)
            {
                foreach (var id in saved.awardedTrophyIds)
                {
                    if (!string.IsNullOrEmpty(id) && !_state.awardedTrophyIds.Contains(id))
                        _state.awardedTrophyIds.Add(id);
                }
            }

            if (saved?.unlockedRecipeIds != null)
            {
                foreach (var rId in saved.unlockedRecipeIds)
                {
                    if (!string.IsNullOrEmpty(rId) && !_state.unlockedRecipeIds.Contains(rId))
                        _state.unlockedRecipeIds.Add(rId);
                }
            }
        }

        private void RegisterBaselineTrophies()
        {
            RegisterTrophy(new TrophyDefinition
            {
                Id = "trophy_wolf_head",
                SpeciesId = "wolf",
                TrophyItemId = "item_decor_trophy_wolf_head",
                RecipeId = "recipe_trophy_wolf_head",
                DisplayName = "Two-Headed Steppe Wolf Trophy",
                Description = "Taxidermied twin heads of an apex steppe predator mounted on charred timbers. A grim testament to survivor vigilance and trapping skill.",
                LocalizedMoraleDelta = 3.0f,
                RecommendedRoomId = "crafting",
                ConditionKind = "quarry_harvested"
            });
            RegisterTrophy(new TrophyDefinition
            {
                Id = "trophy_deer_antlers",
                SpeciesId = "deer",
                TrophyItemId = "item_decor_trophy_deer_antlers",
                RecipeId = "recipe_trophy_deer_antlers",
                DisplayName = "Wasteland Mule Deer Antlers",
                Description = "Sweeping calcified antlers polished and secured to a dark hardwood shield. Evokes memories of open country before the fallout.",
                LocalizedMoraleDelta = 2.0f,
                RecommendedRoomId = "common",
                ConditionKind = "quarry_harvested"
            });
            RegisterTrophy(new TrophyDefinition
            {
                Id = "trophy_boar_tusks",
                SpeciesId = "boar",
                TrophyItemId = "item_decor_trophy_boar_tusks",
                RecipeId = "recipe_trophy_boar_tusks",
                DisplayName = "Razorback Boar Tusks",
                Description = "Curved yellowed tusks banded in brass and bolted to salvage cedar. Displays the brute strength required to bring down irradiated quarry.",
                LocalizedMoraleDelta = 3.0f,
                RecommendedRoomId = "common",
                ConditionKind = "quarry_harvested"
            });
            RegisterTrophy(new TrophyDefinition
            {
                Id = "trophy_fox_pelt",
                SpeciesId = "fox",
                TrophyItemId = "item_decor_trophy_fox_pelt",
                RecipeId = "recipe_trophy_fox_pelt",
                DisplayName = "Barren Fox Pelt",
                Description = "A thick russet-gray winter coat cured with borax and hung from copper rings. Brings warmth and a touch of comfort to cold bunker walls.",
                LocalizedMoraleDelta = 2.0f,
                RecommendedRoomId = "sleeping",
                ConditionKind = "quarry_harvested"
            });
            RegisterTrophy(new TrophyDefinition
            {
                Id = "trophy_beetle_carapace",
                SpeciesId = "slag_beetle",
                TrophyItemId = "item_decor_trophy_beetle_carapace",
                RecipeId = "recipe_trophy_beetle_carapace",
                DisplayName = "Titan Slag-Back Beetle Carapace",
                Description = "Iridescent chitin plates cleaned with acid and lacquered against dust. Serves as a trophy and a study in wasteland armor biology.",
                LocalizedMoraleDelta = 2.0f,
                RecommendedRoomId = "workshop",
                ConditionKind = "quarry_harvested"
            });
            RegisterTrophy(new TrophyDefinition
            {
                Id = "trophy_molerat_skull",
                SpeciesId = "molerat",
                TrophyItemId = "item_decor_trophy_molerat_skull",
                RecipeId = "recipe_trophy_molerat_skull",
                DisplayName = "Tessarat Blind Mole-Rat Skull",
                Description = "The heavy, reinforced cranium and chisel incisors of a subterranean pest. Warns all who see it of what burrows beneath the concrete.",
                LocalizedMoraleDelta = 1.0f,
                RecommendedRoomId = "medical",
                ConditionKind = "quarry_harvested"
            });
            RegisterTrophy(new TrophyDefinition
            {
                Id = "trophy_crow_feathers",
                SpeciesId = "ash_crow",
                TrophyItemId = "item_decor_trophy_crow_feathers",
                RecipeId = "recipe_trophy_crow_feathers",
                DisplayName = "Three-Eyed Sentry Crow Feathers",
                Description = "A dark fan of glossy black-and-silver plumage bound with wire. Reminds observers of the sentinels watching from scorched power poles.",
                LocalizedMoraleDelta = 1.0f,
                RecommendedRoomId = "radio",
                ConditionKind = "quarry_harvested"
            });
            RegisterTrophy(new TrophyDefinition
            {
                Id = "trophy_pheasant_plume",
                SpeciesId = "pheasant",
                TrophyItemId = "item_decor_trophy_pheasant_plume",
                RecipeId = "recipe_trophy_pheasant_plume",
                DisplayName = "Ash Pheasant Plume",
                Description = "Vibrant ember-tinted tail feathers preserved behind a salvage glass bezel. A rare splash of vivid color amidst the gray ruins.",
                LocalizedMoraleDelta = 1.0f,
                RecommendedRoomId = "greenhouse",
                ConditionKind = "quarry_harvested"
            });
            RegisterTrophy(new TrophyDefinition
            {
                Id = "trophy_ash_hound_pelt",
                SpeciesId = "species_ash_hound",
                TrophyItemId = "item_decor_trophy_ash_hound_pelt",
                RecipeId = "recipe_trophy_ash_hound_pelt",
                DisplayName = "Ash Hound Pelt",
                Description = "A dense cinder-colored hide taken from an ash hound pack hunter. Softened with fat and mounted for warmth.",
                LocalizedMoraleDelta = 2.0f,
                RecommendedRoomId = "common",
                ConditionKind = "quarry_harvested"
            });
            RegisterTrophy(new TrophyDefinition
            {
                Id = "trophy_gulden_wolf",
                SpeciesId = "species_dust_lynx",
                TrophyItemId = "item_decor_trophy_gulden_wolf",
                RecipeId = "recipe_trophy_gulden_wolf",
                DisplayName = "Apex Dust Lynx Trophy",
                Description = "The formidable head and fangs of a dust lynx apex ambush hunter. A sign of rare survival against predator ambush.",
                LocalizedMoraleDelta = 3.0f,
                RecommendedRoomId = "sleeping",
                ConditionKind = "quarry_harvested"
            });
            RegisterTrophy(new TrophyDefinition
            {
                Id = "trophy_kestrel_wings",
                SpeciesId = "species_iron_crow",
                TrophyItemId = "item_decor_trophy_kestrel_wings",
                RecipeId = "recipe_trophy_kestrel_wings",
                DisplayName = "Iron Crow Wings Trophy",
                Description = "Broad slate-colored wingspan from an iron crow scav-scout mounted on salvage copper plate.",
                LocalizedMoraleDelta = 1.0f,
                RecommendedRoomId = "radio",
                ConditionKind = "quarry_harvested"
            });
        }
    }
}
