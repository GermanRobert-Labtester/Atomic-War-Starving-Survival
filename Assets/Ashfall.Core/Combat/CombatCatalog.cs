using System;
using System.Collections.Generic;
using Ashfall.Core.IO;
#pragma warning disable CS0649
#pragma warning disable CS8618
using Ashfall.Core;

namespace Ashfall.Core.Combat
{
    /// <summary>Data-driven weapon definition. snake_case ids per the master list.</summary>
    [Serializable]
    public class CombatWeaponDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public float accuracy = 0.6f;        // base chance to connect
        public float damage = 12f;
        public float range = 1f;             // range modifier (1 = standard)
        public string caliber = string.Empty;// ammo id the weapon takes
        public int burst = 1;                // rounds per trigger pull
        public bool isJuryRigged;            // pipe_ / improvised firearm
        public bool isSuppressionCapable;    // rifle/LMG can lay suppressive fire
        public float degradePerShot = 0.015f;// condition loss per shot fired
        public float jamBase = 0.04f;        // base jam chance at pristine condition
        public int scrapRepairCost = 3;      // scrap metal to field-repair to full
        public float conditionThreshold = 0.25f; // below this, jam risk rises steeply
    }

    /// <summary>Data-driven ammunition definition.</summary>
    [Serializable]
    public class CombatAmmoDefinition
    {
        public string id = string.Empty;     // caliber id
        public string displayName = string.Empty;
        public float damageMod = 1f;
        public float rangeMod = 1f;
        public bool isMilitaryTier;          // military ammo in a jury weapon risks burst failure
    }

    /// <summary>Data-driven cover / armor / barrier material properties.</summary>
    [Serializable]
    public class CombatMaterialDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string kind = "cover";        // cover | armor | barrier
        public float armorReduction;         // fraction of damage absorbed
        public float ricochetChance;         // chance to deflect a hit
        public float ricochetEnergyRetained = 0.6f;
    }

    /// <summary>
    /// Data-driven enemy archetype (Plan 10A — combatants system). Combatant
    /// definitions specify the lanes, AI stances, special moves, surrender /
    /// flee thresholds and accuracy modifiers an encounter setup routine
    /// reads when instantiating a <see cref="CombatantState"/>. Independent
    /// from the runtime CombatantState DTO so AI traits live in JSON and
    /// combat runtime state stays a clean engine-agnostic DTO.
    ///
    /// All id strings start with the canonical prefix "combatant_".
    /// faction_id, when present, must match an entry in faction_lore.json
    /// (loader enforces). preferred_lane is 0=Left, 1=Center, 2=Right per
    /// <see cref="Ashfall.Core.Combat.CombatLane"/>. ai_special_move drives
    /// the encounter AI's per-turn behavior choice (Burrow/Spore/Flank/None…).
    /// surrender_threshold / flee_threshold of -1 mean "neversurrender/neversflee".
    /// </summary>
    [Serializable]
    public class CombatantDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string kind = "human";            // human | mutant | fauna
        public string factionId = string.Empty;  // canonical faction id (faction_*), blank = unaligned
        public string description = string.Empty;
        public float baseHealth = 100f;
        public float baseArmorRating;            // 0..1
        public float baseCoverRating;            // 0..1
        public int preferredLane;                // CombatLane value
        public string aiStancePreference = "HoldPosition"; // TacticalStance name
        public string aiSpecialMove = "None";    // None | Burrow | Flank | Spore | Charge | SuppressiveFire | TacticalRetreat
        public float aiAccuracyMod = 1f;         // multiplier on weapon accuracy
        public float aiDamageMod = 1f;           // multiplier on outgoing damage
        public float surrenderThreshold = -1f;   // -1 = never; otherwise 0..1
        public float fleeThreshold = -1f;        // -1 = never; otherwise 0..1
        public int journalKey = 0;               // optional narrative hook id
    }

    /// <summary>
    /// Registry of combat definitions so the core stays data-free while hosts can
    /// register JSON-loaded catalogs. Mirrors ExpeditionDefinitionRegistry.
    /// </summary>
    public static class CombatCatalog
    {
        private static readonly Dictionary<string, CombatWeaponDefinition> s_weapons =
            new Dictionary<string, CombatWeaponDefinition>(StringComparer.Ordinal);
        private static readonly Dictionary<string, CombatAmmoDefinition> s_ammo =
            new Dictionary<string, CombatAmmoDefinition>(StringComparer.Ordinal);
        private static readonly Dictionary<string, CombatMaterialDefinition> s_materials =
            new Dictionary<string, CombatMaterialDefinition>(StringComparer.Ordinal);
        private static readonly Dictionary<string, CombatantDefinition> s_combatants =
            new Dictionary<string, CombatantDefinition>(StringComparer.Ordinal);

        public static void SeedDefaults()
        {
            // Data authority is JSON (Assets/StreamingAssets/Data/combat_catalog.json).
            // This populates the registry from that file so code never owns the
            // values (Invariant #6). Idempotent: a host that already loaded the
            // JSON is never clobbered.
            if (s_weapons.Count > 0 || s_ammo.Count > 0 || s_materials.Count > 0
                || s_combatants.Count > 0)
                return;

            string dataDir = null;
            if (!CatalogLocator.TryFindDataDirectory(Environment.CurrentDirectory, out dataDir)
                && !CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir))
                dataDir = null;

            if (!string.IsNullOrEmpty(dataDir))
                CombatCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            // Crash-avoidance fallback for environments with no data directory
            // (the committed JSON is the authority and is always found in a repo
            // checkout; this exists only to avoid a null weapon set in odd hosts).
            if (s_weapons.Count == 0 && s_ammo.Count == 0 && s_materials.Count == 0
                && s_combatants.Count == 0)
                SeedMinimalFallback();
        }

        private static void SeedMinimalFallback()
        {
            Register(new CombatWeaponDefinition
            {
                id = "weapon_pipe_rifle", displayName = "Pipe Rifle",
                accuracy = 0.46f, damage = 12f, caliber = "ammo_357", burst = 1,
                isJuryRigged = true, isSuppressionCapable = false,
                degradePerShot = 0.022f, jamBase = 0.055f, scrapRepairCost = 3,
                conditionThreshold = 0.30f
            });
            Register(new CombatAmmoDefinition { id = "ammo_357", displayName = ".357", damageMod = 1.0f, rangeMod = 1.0f, isMilitaryTier = false });
            Register(new CombatMaterialDefinition { id = "material_wood", displayName = "Rotted Wood", kind = "cover", armorReduction = 0.30f, ricochetChance = 0.05f, ricochetEnergyRetained = 0.5f });
        }

        public static void Register(CombatWeaponDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            s_weapons[def.id] = def;
        }

        public static void Register(CombatAmmoDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            s_ammo[def.id] = def;
        }

        public static void Register(CombatMaterialDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            s_materials[def.id] = def;
        }

        public static CombatWeaponDefinition GetWeapon(string id)
        {
            return !string.IsNullOrEmpty(id) && s_weapons.TryGetValue(id, out var d) ? d : null;
        }

        public static CombatAmmoDefinition GetAmmo(string id)
        {
            return !string.IsNullOrEmpty(id) && s_ammo.TryGetValue(id, out var d) ? d : null;
        }

        public static CombatMaterialDefinition GetMaterial(string id)
        {
            return !string.IsNullOrEmpty(id) && s_materials.TryGetValue(id, out var d) ? d : null;
        }

        public static CombatantDefinition? GetCombatant(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return s_combatants.TryGetValue(id, out var d) ? d : null;
        }

        public static bool HasWeapon(string id) => GetWeapon(id) != null;
        public static bool HasAmmo(string id) => GetAmmo(id) != null;
        public static bool HasMaterial(string id) => GetMaterial(id) != null;
        public static bool HasCombatant(string id) => GetCombatant(id) != null;

        public static IReadOnlyCollection<string> WeaponIds => s_weapons.Keys;
        public static IReadOnlyCollection<string> AmmoIds => s_ammo.Keys;
        public static IReadOnlyCollection<string> MaterialIds => s_materials.Keys;
        public static IReadOnlyCollection<string> CombatantIds => s_combatants.Keys;

        public static void Register(CombatantDefinition def)
        {
            if (def == null || string.IsNullOrEmpty(def.id)) return;
            s_combatants[def.id] = def;
        }

        public static void Clear()
        {
            s_weapons.Clear();
            s_ammo.Clear();
            s_materials.Clear();
            s_combatants.Clear();
        }
    }

    // ---------------------------------------------------------------------
    // Combatant factory — bridge from combatant_* catalog entries to live
    // CombatantState runtime rows. This is the single sanctioned conversion
    // point: encounter setup hands the factory a catalog id and receives a
    // populated CombatantState whose AI trait fields (AiStancePreference,
    // AiSpecialMove, AiAccuracyMod, AiDamageMod, SurrenderThreshold,
    // FleeThreshold, CatalogId) carry the catalog-derived values into the
    // simulation. Transient state (current Health, IsPinned, weapon
    // assignment) is intentionally per-encounter and now styled here.
    // ---------------------------------------------------------------------

    /// <summary>
    /// Result of a transient failure in <see cref="CombatantFactory.TrySpawnFromCatalog"/>.
    /// </summary>
    public enum CombatantSpawnStatus
    {
        Success = 0,
        UnknownCombatantId = 1,
        NotRegistered = 2
    }

    /// <summary>
    /// Engine-agnostic factory that materialises a <see cref="CombatantState"/>
    /// from a <c>combatant_*</c> catalog entry. The factory is the only path
    /// that copies CombatantDefinition values into a runtime CombatantState
    /// row; the legacy hand-coded <c>new CombatantState</c> paths in
    /// listen-only demonstrations and historical save loads continue to
    /// initialise their fields to safe defaults.
    ///
    /// Clamps the lane to 0..2, armour/cover to [0,1], preserves the catalog
    /// id as <see cref="CombatantState.CatalogId"/> for downstream AI
    /// reasoning, and leaves call-site-owned transient fields (current
    /// Health, weapon assignment, IsDowned) untouched so an encounter can
    /// override them before BeginEncounter commits the row.
    /// </summary>
    public static class CombatantFactory
    {
        public const string SystemId = "combatant_factory";

        /// <summary>
        /// Materialise a runtime <see cref="CombatantState"/> from a registered
        /// combatant_* id. Returns null when the id is unknown so the caller
        /// can fall back to its legacy hand-coded enemy block.
        /// </summary>
        public static CombatantState? SpawnFromCatalog(string combatantId)
        {
            if (string.IsNullOrEmpty(combatantId)) return null;
            var def = CombatCatalog.GetCombatant(combatantId);
            if (def == null) return null;
            return Build(def);
        }

        /// <summary>
        /// Strict variant of <see cref="SpawnFromCatalog(string)"/>: throws
        /// <see cref="System.Collections.Generic.KeyNotFoundException"/> on
        /// unknown id so a calling test or host can fail loudly rather than
        /// silently fall back. This is the canonical invalid-id behaviour.
        /// </summary>
        public static CombatantState SpawnFromCatalogOrThrow(string combatantId)
        {
            var def = CombatCatalog.GetCombatant(combatantId)
                ?? throw new System.Collections.Generic.KeyNotFoundException(
                    "CombatantFactory: no registered combatant with id '" + combatantId + "'.");
            return Build(def);
        }

        /// <summary>
        /// Tries to populate <paramref name="result"/> without throwing.
        /// Returns true on success, false on unknown id (with a clear status).
        /// </summary>
        public static bool TrySpawnFromCatalog(string combatantId, out CombatantState? result, out CombatantSpawnStatus status)
        {
            result = null;
            status = CombatantSpawnStatus.UnknownCombatantId;
            if (string.IsNullOrEmpty(combatantId))
                return false;
            var def = CombatCatalog.GetCombatant(combatantId);
            if (def == null) return false;
            result = Build(def);
            status = CombatantSpawnStatus.Success;
            return true;
        }

        private static CombatantState Build(CombatantDefinition def)
        {
            // Id is intentionally left empty: callers (encounter setup,
            // host seed) hand the row a stable, deterministic id. The
            // legacy hand-coded `new CombatantState { Id = "enemy_..." }`
            // token inside TacticalCombatSystem.cs and src/Host/CombatHostSession.cs
            // already does this; the factory must not insert
            // nondeterministic ids into the simulation (Invariant 4).
            return new CombatantState
            {
                Id = string.Empty,
                Name = def.displayName ?? def.id,
                IsPlayer = false,
                FactionId = def.factionId ?? string.Empty,
                Lane = def.preferredLane < 0 || def.preferredLane > 2 ? (int)CombatLane.Center : def.preferredLane,
                Health = def.baseHealth,
                MaxHealth = def.baseHealth,
                ArmorRating = def.baseArmorRating < 0f ? 0f : (def.baseArmorRating > 1f ? 1f : def.baseArmorRating),
                CoverRating = def.baseCoverRating < 0f ? 0f : (def.baseCoverRating > 1f ? 1f : def.baseCoverRating),
                AiStancePreference = string.IsNullOrEmpty(def.aiStancePreference) ? "HoldPosition" : def.aiStancePreference,
                AiSpecialMove = string.IsNullOrEmpty(def.aiSpecialMove) ? "None" : def.aiSpecialMove,
                AiAccuracyMod = def.aiAccuracyMod <= 0f ? 1f : (def.aiAccuracyMod > 2f ? 2f : def.aiAccuracyMod),
                AiDamageMod = def.aiDamageMod <= 0f ? 1f : (def.aiDamageMod > 2f ? 2f : def.aiDamageMod),
                SurrenderThreshold = def.surrenderThreshold,
                FleeThreshold = def.fleeThreshold,
                CatalogId = def.id
            };
        }
    }

    // ---------------------------------------------------------------------
    // Combat data authority loader — reads Assets/StreamingAssets/Data/combat_catalog.json
    // ---------------------------------------------------------------------

    [Serializable]
    internal sealed class CombatWeaponJson
    {
        public string id;
        public string display_name;
        public float accuracy;
        public float damage;
        public float range;
        public string caliber;
        public int burst;
        public bool is_jury_rigged;
        public bool is_suppression_capable;
        public float degrade_per_shot;
        public float jam_base;
        public int scrap_repair_cost;
        public float condition_threshold;
    }

    [Serializable]
    internal sealed class CombatAmmoJson
    {
        public string id;
        public string display_name;
        public float damage_mod;
        public float range_mod;
        public bool is_military_tier;
    }

    [Serializable]
    internal sealed class CombatMaterialJson
    {
        public string id;
        public string display_name;
        public string kind;
        public float armor_reduction;
        public float ricochet_chance;
        public float ricochet_energy_retained;
    }

    [Serializable]
    internal sealed class CombatantJson
    {
        public string id;
        public string display_name;
        public string kind;             // human | mutant | fauna
        public string faction_id;       // optional
        public string description;
        public float base_health;
        public float base_armor_rating;
        public float base_cover_rating;
        public int preferred_lane;      // 0/1/2
        public string ai_stance_preference;
        public string ai_special_move;
        public float ai_accuracy_mod;
        public float ai_damage_mod;
        public float surrender_threshold;
        public float flee_threshold;
        public int journal_key;
    }

    [Serializable]
    internal sealed class CombatCatalogRoot
    {
        public int schema_version = 1;
        public string collection_id = "combat_catalog";
        public List<CombatWeaponJson> weapons = new List<CombatWeaponJson>();
        public List<CombatAmmoJson> ammo = new List<CombatAmmoJson>();
        public List<CombatMaterialJson> materials = new List<CombatMaterialJson>();
        public List<CombatantJson> combatants = new List<CombatantJson>();
    }

    /// <summary>
    /// Engine-agnostic loader for the Combat data authority
    /// (combat_catalog.json, snake_case). Maps onto the camelCase runtime
    /// definitions. Returns false (leaving the registry untouched) when the
    /// file is absent; surfaces parse/schema errors as exceptions so a bad
    /// catalog never silently slips in.
    /// </summary>
    public static class CombatCatalogLoader
    {
        public const string FileName = "combat_catalog.json";
        public const int CurrentSchemaVersion = 2;

        /// <summary>
        /// Load + validate the combat catalog. Following Invariant #6 and the
        /// save-migration convention: a future schema throws (never silently
        /// guessed), while canonical-id and cross-reference violations throw so
        /// a malformed catalog can never slip into the simulation.
        /// </summary>
        public static bool Load(string dataDirectory, IFileIO files, IJsonSerializer json)
        {
            string path = files.Combine(dataDirectory, FileName);
            if (!files.FileExists(path)) return false;

            var root = json.Deserialize<CombatCatalogRoot>(files.ReadAllText(path));
            if (root == null) return false;

            if (root.schema_version > CurrentSchemaVersion)
                throw new System.IO.InvalidDataException(
                    $"{FileName} schema {root.schema_version} is newer than supported {CurrentSchemaVersion}.");

            CombatCatalog.Clear();

            if (root.weapons != null)
            {
                for (int i = 0; i < root.weapons.Count; i++)
                {
                    var w = root.weapons[i];
                    if (w == null || string.IsNullOrEmpty(w.id)) continue;
                    CombatCatalog.Register(new CombatWeaponDefinition
                    {
                        id = w.id,
                        displayName = w.display_name ?? string.Empty,
                        accuracy = w.accuracy,
                        damage = w.damage,
                        range = w.range,
                        caliber = w.caliber ?? string.Empty,
                        burst = w.burst,
                        isJuryRigged = w.is_jury_rigged,
                        isSuppressionCapable = w.is_suppression_capable,
                        degradePerShot = w.degrade_per_shot,
                        jamBase = w.jam_base,
                        scrapRepairCost = w.scrap_repair_cost,
                        conditionThreshold = w.condition_threshold
                    });
                }
            }

            if (root.ammo != null)
            {
                for (int i = 0; i < root.ammo.Count; i++)
                {
                    var a = root.ammo[i];
                    if (a == null || string.IsNullOrEmpty(a.id)) continue;
                    CombatCatalog.Register(new CombatAmmoDefinition
                    {
                        id = a.id,
                        displayName = a.display_name ?? string.Empty,
                        damageMod = a.damage_mod,
                        rangeMod = a.range_mod,
                        isMilitaryTier = a.is_military_tier
                    });
                }
            }

            if (root.materials != null)
            {
                for (int i = 0; i < root.materials.Count; i++)
                {
                    var m = root.materials[i];
                    if (m == null || string.IsNullOrEmpty(m.id)) continue;
                    CombatCatalog.Register(new CombatMaterialDefinition
                    {
                        id = m.id,
                        displayName = m.display_name ?? string.Empty,
                        kind = m.kind ?? "cover",
                        armorReduction = m.armor_reduction,
                        ricochetChance = m.ricochet_chance,
                        ricochetEnergyRetained = m.ricochet_energy_retained
                    });
                }
            }

            if (root.combatants != null)
            {
                for (int i = 0; i < root.combatants.Count; i++)
                {
                    var c = root.combatants[i];
                    if (c == null || string.IsNullOrEmpty(c.id)) continue;
                    CombatCatalog.Register(new CombatantDefinition
                    {
                        id = c.id,
                        displayName = c.display_name ?? string.Empty,
                        kind = c.kind ?? "human",
                        factionId = c.faction_id ?? string.Empty,
                        description = c.description ?? string.Empty,
                        baseHealth = c.base_health,
                        baseArmorRating = c.base_armor_rating,
                        baseCoverRating = c.base_cover_rating,
                        preferredLane = c.preferred_lane,
                        aiStancePreference = c.ai_stance_preference ?? "HoldPosition",
                        aiSpecialMove = c.ai_special_move ?? "None",
                        aiAccuracyMod = c.ai_accuracy_mod == 0f ? 1f : c.ai_accuracy_mod,
                        aiDamageMod = c.ai_damage_mod == 0f ? 1f : c.ai_damage_mod,
                        surrenderThreshold = c.surrender_threshold,
                        fleeThreshold = c.flee_threshold,
                        journalKey = c.journal_key
                    });
                }
            }

            ValidateRegistered(root, CombatCatalog.WeaponIds, CombatCatalog.AmmoIds,
                CombatCatalog.MaterialIds, CombatCatalog.CombatantIds, dataDirectory, files, json);
            return true;
        }

        /// <summary>Enforce canonical snake_case id prefixes and cross-references.</summary>
        private static void ValidateRegistered(
            CombatCatalogRoot root,
            System.Collections.Generic.IReadOnlyCollection<string> weaponIds,
            System.Collections.Generic.IReadOnlyCollection<string> ammoIds,
            System.Collections.Generic.IReadOnlyCollection<string> materialIds,
            System.Collections.Generic.IReadOnlyCollection<string> combatantIds,
            string dataDirectory,
            IFileIO files,
            IJsonSerializer json)
        {
            var errors = new System.Collections.Generic.List<string>();
            var ammoSet = new HashSet<string>(ammoIds, StringComparer.Ordinal);
            var weaponSet = new HashSet<string>(weaponIds, StringComparer.Ordinal);
            var materialSet = new HashSet<string>(materialIds, StringComparer.Ordinal);
            var combatantSet = new HashSet<string>(combatantIds, StringComparer.Ordinal);

            // Provisionally load faction ids so combatant cross-refs can be
            // validated. Faction lore lives in faction_lore.json (the same
            // authority the Warlord catalog validator uses); only loaded
            // here when at least one combatant is present so legacy test
            // fixtures (which only ship weapons/ammo/materials) keep loading.
            var factionIds = combatantSet.Count > 0
                ? LoadFactionIdAuthority(dataDirectory, files, json)
                : new HashSet<string>(StringComparer.Ordinal);

            if (root.weapons != null)
            for (int i = 0; i < root.weapons.Count; i++)
            {
                var w = root.weapons[i];
                if (w == null) continue;
                if (string.IsNullOrEmpty(w.id) || !w.id.StartsWith("weapon_", StringComparison.Ordinal))
                    errors.Add("weapon#" + i + " id must be canonical (weapon_*): " + (w.id ?? "<null>"));
                else if (!string.IsNullOrEmpty(w.caliber) && !ammoSet.Contains(w.caliber))
                    errors.Add("weapon " + w.id + " references unknown caliber " + w.caliber);
            }
            if (root.ammo != null)
            for (int i = 0; i < root.ammo.Count; i++)
            {
                var a = root.ammo[i];
                if (a == null) continue;
                if (string.IsNullOrEmpty(a.id) || !a.id.StartsWith("ammo_", StringComparison.Ordinal))
                    errors.Add("ammo#" + i + " id must be canonical (ammo_*): " + (a.id ?? "<null>"));
            }
            if (root.materials != null)
            for (int i = 0; i < root.materials.Count; i++)
            {
                var m = root.materials[i];
                if (m == null) continue;
                bool canon = !string.IsNullOrEmpty(m.id)
                    && (m.id.StartsWith("material_", StringComparison.Ordinal)
                        || m.id.StartsWith("armor_", StringComparison.Ordinal));
                if (!canon)
                    errors.Add("material#" + i + " id must be canonical (material_*/armor_*): " + (m.id ?? "<null>"));
            }

            if (root.combatants != null)
            {
                for (int i = 0; i < root.combatants.Count; i++)
                {
                    var c = root.combatants[i];
                    if (c == null) continue;
                    if (string.IsNullOrEmpty(c.id) || !c.id.StartsWith("combatant_", StringComparison.Ordinal))
                        errors.Add("combatant#" + i + " id must be canonical (combatant_*): " + (c.id ?? "<null>"));
                    else if (c.preferred_lane < 0 || c.preferred_lane > 2)
                        errors.Add("combatant " + c.id + " preferred_lane " + c.preferred_lane + " is outside 0..2 (CombatLane enum range)");
                    else if (!string.IsNullOrEmpty(c.ai_stance_preference)
                        && !IsKnownStance(c.ai_stance_preference))
                        errors.Add("combatant " + c.id + " ai_stance_preference '" + c.ai_stance_preference + "' is not a TacticalStance name");
                    else if (!string.IsNullOrEmpty(c.ai_special_move)
                        && !IsKnownAiMove(c.ai_special_move))
                        errors.Add("combatant " + c.id + " ai_special_move '" + c.ai_special_move + "' is not in KnownAiMoves");
                    else if (c.ai_accuracy_mod < 0f || c.ai_accuracy_mod > 2f)
                        errors.Add("combatant " + c.id + " ai_accuracy_mod outside 0..2: " + c.ai_accuracy_mod);
                    else if (c.ai_damage_mod < 0f || c.ai_damage_mod > 2f)
                        errors.Add("combatant " + c.id + " ai_damage_mod outside 0..2: " + c.ai_damage_mod);
                    else if (c.surrender_threshold != -1f && (c.surrender_threshold < 0f || c.surrender_threshold > 1f))
                        errors.Add("combatant " + c.id + " surrender_threshold outside -1 or 0..1: " + c.surrender_threshold);
                    else if (c.flee_threshold != -1f && (c.flee_threshold < 0f || c.flee_threshold > 1f))
                        errors.Add("combatant " + c.id + " flee_threshold outside -1 or 0..1: " + c.flee_threshold);
                    else if (!string.IsNullOrEmpty(c.faction_id) && !factionIds.Contains(c.faction_id))
                        errors.Add("combatant " + c.id + " faction_id '" + c.faction_id + "' not found in faction_lore.json");
                }
            }

            if (errors.Count > 0)
                throw new FormatException("combat_catalog.json failed validation:\n" + string.Join("\n", errors));
        }

        /// <summary>
        /// Loads the canonical faction ids from faction_lore.json so a
        /// combatant's faction_id field can be cross-referenced. Returns an
        /// empty set when the lookup fails so we still surface a clean
        /// FormatException on the combatant validation step (rather than a
        /// file-not-found swallowed silently).
        /// </summary>
        private static HashSet<string> LoadFactionIdAuthority(string dataDirectory, IFileIO files, IJsonSerializer json)
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            if (string.IsNullOrEmpty(dataDirectory) || files == null || json == null) return ids;
            string path = files.Combine(dataDirectory, "faction_lore.json");
            if (!files.FileExists(path)) return ids;
            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw)) return ids;
            try
            {
                var list = CatalogLocator.LoadWrappedList<FactionIdProbe>(raw, SystemTextJsonSerializer.Options);
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                        if (list[i] != null && !string.IsNullOrEmpty(list[i].faction_id))
                            ids.Add(list[i].faction_id);
                }
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "FactionIdAuthority", ex_CATDIAG);
            }
            return ids;
        }

        [Serializable]
        private sealed class FactionIdProbe
        {
            public string faction_id = string.Empty;
        }

        private static bool IsKnownStance(string name)
        {
            if (string.IsNullOrEmpty(name)) return true;
            // Enum-driven: accept only names declared on TacticalStance (the
            // single authority). Mirrors CircusProject: never widen accepted
            // values by accident — the enum is the source of truth.
            foreach (var v in System.Enum.GetNames(typeof(TacticalStance)))
                if (v == name) return true;
            return false;
        }

        private static bool IsKnownAiMove(string name)
        {
            if (string.IsNullOrEmpty(name)) return true;
            // Centralised in CombatAiMoves (Combat/CombatAiMove.cs) so we
            // never re-roll the accepted set in two places. None is always
            // accepted as a sentinel; the enum's ClosedName set is the
            // source of truth.
            return CombatAiMoves.IsAllowed(name);
        }
    }
}
