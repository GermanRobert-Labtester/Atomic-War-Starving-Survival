using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Inventory
{
    [Serializable]
    internal sealed class ItemJsonDto
    {
        public string id { get; set; } = string.Empty;
        public string displayName { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string iconPath { get; set; } = string.Empty;
        public string type { get; set; } = string.Empty;
        public int stackMax { get; set; } = 1;
        public float weight { get; set; }
        public float radProtection { get; set; }
        public float durability { get; set; }
        public float degradeRate { get; set; }
        public float degrade_rate { get; set; }
        public bool isEquipable { get; set; }
        public string equipSlot { get; set; } = string.Empty;
        public float contamination { get; set; }
        public float hungerRestore { get; set; }
        public float thirstRestore { get; set; }
        public float healthEffect { get; set; }
        public float radCleanse { get; set; }
        public float moraleEffect { get; set; }
        public float decorLocalizedMoraleDelta { get; set; }
        public bool empShielded { get; set; }
        public float tradeValue { get; set; }
        public int tradeTier { get; set; }
        public float disassembleYieldFraction { get; set; } = 0.5f;
        public List<ScrapYieldDto>? scrapValue { get; set; }
        public RepairRecipeDto? repairRecipe { get; set; }
    }

    [Serializable]
    internal sealed class ScrapYieldDto
    {
        public string materialId { get; set; } = string.Empty;
        public int amount { get; set; } = 1;
    }

    [Serializable]
    internal sealed class RepairRecipeDto
    {
        public List<ScrapYieldDto>? costs { get; set; }
        public float hours { get; set; } = 0.5f;
        public bool requiresTools { get; set; } = true;
    }

    [Serializable]
    internal sealed class StartingSupplyJsonDto
    {
        public string itemId { get; set; } = string.Empty;
        public int amount { get; set; } = 1;
    }

    [Serializable]
    internal sealed class StartingSuppliesProfileJsonDto
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public List<StartingSupplyJsonDto> supplies { get; set; } =
            new List<StartingSupplyJsonDto>();
    }

    [Serializable]
    internal sealed class StartingSuppliesRootJsonDto
    {
        public int schema_version { get; set; } = 1;
        public string default_profile_id { get; set; } = string.Empty;
        public List<StartingSupplyJsonDto>? starting_supplies { get; set; }
        public List<StartingSuppliesProfileJsonDto>? profiles { get; set; }
    }

    public enum StartingSuppliesLoadStatus
    {
        Success,
        MissingFile,
        EmptyFile,
        ParseFailure,
        UnsupportedSchema,
        InvalidRow,
        DuplicateRow,
        UnknownItem,
        InvalidProfile
    }

    public sealed class StartingSuppliesLoadResult
    {
        public StartingSuppliesLoadStatus Status { get; set; } = StartingSuppliesLoadStatus.Success;
        public string ErrorMessage { get; set; } = string.Empty;
        public string SelectedProfileId { get; set; } = StartingSuppliesCatalog.StandardProfileId;
        public List<(string itemId, int amount)> Supplies { get; } = new List<(string itemId, int amount)>();
        public int AcceptedRowCount => Supplies.Count;
        public bool IsSuccess => Status == StartingSuppliesLoadStatus.Success;
    }

    public sealed class StartingSuppliesCatalogLoadResult
    {
        public StartingSuppliesCatalog Catalog { get; internal set; } =
            new StartingSuppliesCatalog(
                new[] { StartingSuppliesCatalog.CreateLegacyFallbackProfile() },
                StartingSuppliesCatalog.StandardProfileId);
        public List<string> Errors { get; } = new List<string>();
        public List<string> Warnings { get; } = new List<string>();
        public bool UsedLegacyFallback { get; internal set; }
        public bool IsUsable => Catalog.Profiles.Count > 0;
    }

    /// <summary>
    /// Shared Core loader for item definitions and starting supplies from JSON (authority).
    /// Zero engine dependencies; adheres to Invariant 1 and Invariant 6.
    /// </summary>
    public static class ItemCatalogLoader
    {
        public const string PrimaryFileName = "items.json";
        public const string StartingSuppliesFileName = "starting_supplies.json";
        public const string ItemDescriptionsFileName = ItemDescriptionCatalogLoader.PrimaryFileName;

        private static readonly string[] SecondaryItemFiles =
        {
            "holdfast_items.json",
            "black_flotilla_items.json",
            "verdict_items.json",
            "greenhouse_items.json",
            "foundry_items.json",
            "crossing_items.json",
            "dose_items.json",
            "chemical_dependency_items.json",
            "year_of_ash_items.json",
            "agriculture_items.json"
        };

        public static ItemCatalog LoadCatalog(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            var result = LoadCatalogWithResult(dataDir, fileIO, serializer);
            result.ThrowIfFatal();
            return result.Entries.Count > 0 ? result.Entries[0] : new ItemCatalog();
        }

        public static List<ItemDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            var result = LoadCatalogWithResult(dataDir, fileIO, serializer);
            result.ThrowIfFatal();
            var catalog = result.Entries.Count > 0 ? result.Entries[0] : new ItemCatalog();
            var list = new List<ItemDefinition>();
            foreach (var id in catalog.Ids)
            {
                var def = catalog.Get(id);
                if (def != null) list.Add(def);
            }
            return list;
        }

        public static ItemDescriptionCatalog LoadDescriptionCatalog(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            return ItemDescriptionCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);
        }

        public static CatalogLoadResult<ItemDescriptionCatalog> LoadDescriptionCatalogWithResult(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            return ItemDescriptionCatalogLoader.LoadCatalogWithResult(dataDir, fileIO, serializer);
        }

        public static CatalogLoadResult<ItemCatalog> LoadCatalogWithResult(
            string dataDir, IFileIO fileIO, IJsonSerializer serializer, ItemCatalog? targetCatalog = null)
        {
            if (fileIO == null || serializer == null || string.IsNullOrEmpty(dataDir))
            {
                var failResult = new CatalogLoadResult<ItemCatalog>(
                    PrimaryFileName,
                    "ItemCatalog",
                    CatalogClassification.Required);
                failResult.AddFatal("Required dependencies are null or dataDir is empty");
                return failResult;
            }

            var result = new CatalogLoadResult<ItemCatalog>(
                fileIO.Combine(dataDir, PrimaryFileName),
                "ItemCatalog",
                CatalogClassification.Required);

            try
            {
                var catalog = targetCatalog ?? new ItemCatalog();

                // 1. Primary items.json
                string primaryPath = fileIO.Combine(dataDir, PrimaryFileName);
                if (!LoadFileIntoWithResult(catalog, primaryPath, fileIO, serializer, result, isPrimary: true))
                {
                    if (!result.HasFatalErrors)
                        result.AddFatal("Primary items catalog failed to load: " + primaryPath);
                    return result;
                }

                // 2. Secondary expansion item files (optional - warnings only)
                for (int i = 0; i < SecondaryItemFiles.Length; i++)
                {
                    string path = fileIO.Combine(dataDir, SecondaryItemFiles[i]);
                    if (fileIO.FileExists(path))
                    {
                        var secondaryResult = new CatalogLoadResult<ItemCatalog>(
                            path, "ItemCatalog", CatalogClassification.Optional);
                        LoadFileIntoWithResult(catalog, path, fileIO, serializer, secondaryResult, isPrimary: false);
                        // For optional files, just track warnings but don't fail
                        foreach (var msg in secondaryResult.Messages)
                        {
                            if (msg.Severity >= CatalogLoadSeverity.Warning)
                                result.AddMessage(msg.Severity, msg.FilePath, msg.Shape, msg.Message, msg.Exception);
                        }
                    }
                }

                result.AddEntry(catalog);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn("ItemCatalogLoader", "<root>", ex);
                result.AddFatal("Failed to build item catalog: " + ex.Message, ex);
            }

            return result;
        }

        public static void LoadInto(ItemCatalog catalog, string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            if (catalog == null || fileIO == null || serializer == null || string.IsNullOrEmpty(dataDir))
                return;

            LoadCatalogWithResult(dataDir, fileIO, serializer, catalog);
        }

        public static List<(string itemId, int amount)> LoadStartingSupplies(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            var res = LoadStartingSuppliesDetailed(
                dataDir,
                fileIO,
                serializer,
                catalog: null,
                profileId: StartingSuppliesCatalog.StandardProfileId);
            return res.IsSuccess ? res.Supplies : new List<(string itemId, int amount)>();
        }

        public static StartingSuppliesCatalog LoadStartingSuppliesCatalog(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer serializer,
            ItemCatalog? catalog = null)
        {
            return LoadStartingSuppliesCatalogDetailed(dataDir, fileIO, serializer, catalog).Catalog;
        }

        public static StartingSuppliesCatalogLoadResult LoadStartingSuppliesCatalogDetailed(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer serializer,
            ItemCatalog? catalog = null)
        {
            var result = new StartingSuppliesCatalogLoadResult();
            var fallback = StartingSuppliesCatalog.CreateLegacyFallbackProfile();
            string path = string.IsNullOrEmpty(dataDir) || fileIO == null
                ? StartingSuppliesFileName
                : fileIO.Combine(dataDir, StartingSuppliesFileName);

            if (fileIO == null || serializer == null || string.IsNullOrEmpty(dataDir))
            {
                result.Errors.Add("fileIO, serializer, or dataDir is null or empty.");
                result.UsedLegacyFallback = true;
                return result;
            }

            if (!fileIO.FileExists(path))
            {
                result.Warnings.Add($"Authoritative starting supplies file missing: {path}");
                result.UsedLegacyFallback = true;
                return result;
            }

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                result.Warnings.Add($"Authoritative starting supplies file is empty: {path}");
                result.UsedLegacyFallback = true;
                return result;
            }

            StartingSuppliesRootJsonDto? root;
            try
            {
                root = serializer.Deserialize<StartingSuppliesRootJsonDto>(raw);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn("ItemCatalogLoader", StartingSuppliesFileName, ex);
                result.Errors.Add($"Failed to parse starting supplies from {path}: {ex.Message}");
                result.UsedLegacyFallback = true;
                return result;
            }

            if (root == null)
            {
                result.Errors.Add($"Starting supplies root is null: {path}");
                result.UsedLegacyFallback = true;
                return result;
            }

            if (root.schema_version == 1 &&
                (root.profiles == null || root.profiles.Count == 0) &&
                root.starting_supplies != null)
            {
                var standard = new StartingSuppliesProfile
                {
                    id = StartingSuppliesCatalog.StandardProfileId,
                    display_name = "Standard Holdfast",
                    description = "The unchanged legacy opening."
                };
                if (!TryPopulateProfile(
                    standard,
                    root.starting_supplies,
                    catalog,
                    path,
                    "starting_supplies",
                    result))
                {
                    result.UsedLegacyFallback = true;
                    return result;
                }

                result.Catalog = new StartingSuppliesCatalog(
                    new[] { standard },
                    StartingSuppliesCatalog.StandardProfileId);
                return result;
            }

            if (root.schema_version != 2)
            {
                result.Errors.Add(
                    $"Unsupported starting supplies schema_version {root.schema_version}; expected 2.");
                result.UsedLegacyFallback = true;
                return result;
            }

            if (root.profiles == null || root.profiles.Count == 0)
            {
                result.Errors.Add("Starting supplies v2 has no profiles.");
                result.UsedLegacyFallback = true;
                return result;
            }

            string defaultProfileId = string.IsNullOrEmpty(root.default_profile_id)
                ? StartingSuppliesCatalog.StandardProfileId
                : root.default_profile_id;
            var profiles = new List<StartingSuppliesProfile>();
            var seenProfileIds = new HashSet<string>(StringComparer.Ordinal);
            bool standardAccepted = false;

            for (int i = 0; i < root.profiles.Count; i++)
            {
                var dto = root.profiles[i];
                if (dto == null || string.IsNullOrEmpty(dto.id))
                {
                    result.Warnings.Add($"Profile[{i}] has no id and was skipped.");
                    continue;
                }

                if (!seenProfileIds.Add(dto.id))
                {
                    result.Warnings.Add($"Duplicate profile id '{dto.id}' was skipped.");
                    continue;
                }

                var profile = new StartingSuppliesProfile
                {
                    id = dto.id,
                    display_name = dto.display_name ?? string.Empty,
                    description = dto.description ?? string.Empty
                };
                bool valid = TryPopulateProfile(
                    profile,
                    dto.supplies,
                    catalog,
                    path,
                    dto.id,
                    result);

                if (!valid)
                {
                    if (string.Equals(
                        dto.id,
                        StartingSuppliesCatalog.StandardProfileId,
                        StringComparison.Ordinal))
                    {
                        result.Errors.Add(
                            "Standard Holdfast profile is invalid; legacy fallback will be used.");
                    }
                    continue;
                }

                if (string.IsNullOrWhiteSpace(profile.display_name) ||
                    string.IsNullOrWhiteSpace(profile.description))
                {
                    result.Warnings.Add(
                        $"Profile '{dto.id}' has incomplete presentation text and was skipped.");
                    continue;
                }

                profiles.Add(profile);
                if (string.Equals(
                    dto.id,
                    StartingSuppliesCatalog.StandardProfileId,
                    StringComparison.Ordinal))
                    standardAccepted = true;
            }

            if (!standardAccepted)
            {
                profiles.Insert(0, fallback);
                result.UsedLegacyFallback = true;
                defaultProfileId = StartingSuppliesCatalog.StandardProfileId;
            }

            if (profiles.Count == 0)
            {
                result.Errors.Add("No valid starting supplies profiles were found.");
                result.UsedLegacyFallback = true;
                return result;
            }

            bool defaultExists = false;
            for (int i = 0; i < profiles.Count; i++)
            {
                if (string.Equals(profiles[i].id, defaultProfileId, StringComparison.Ordinal))
                {
                    defaultExists = true;
                    break;
                }
            }
            if (!defaultExists)
            {
                result.Warnings.Add(
                    $"Unknown default profile '{defaultProfileId}'; Standard Holdfast selected.");
                defaultProfileId = StartingSuppliesCatalog.StandardProfileId;
                result.UsedLegacyFallback = true;
            }

            result.Catalog = new StartingSuppliesCatalog(profiles, defaultProfileId);
            return result;
        }

        public static StartingSuppliesLoadResult LoadStartingSuppliesDetailed(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer serializer,
            ItemCatalog? catalog = null,
            string? profileId = null)
        {
            var result = new StartingSuppliesLoadResult();
            result.SelectedProfileId = profileId ?? StartingSuppliesCatalog.StandardProfileId;
            var loaded = LoadStartingSuppliesCatalogDetailed(dataDir, fileIO, serializer, catalog);
            var selected = loaded.Catalog.ResolveOrDefault(result.SelectedProfileId);
            result.SelectedProfileId = selected.id;
            for (int i = 0; i < selected.supplies.Count; i++)
                result.Supplies.Add(selected.supplies[i]);

            result.Status = StartingSuppliesLoadStatus.Success;
            if (loaded.UsedLegacyFallback)
            {
                result.Status = StartingSuppliesLoadStatus.ParseFailure;
                result.ErrorMessage = loaded.Errors.Count > 0
                    ? string.Join(" ", loaded.Errors)
                    : string.Join(" ", loaded.Warnings);
            }
            return result;
        }

        private static bool TryPopulateProfile(
            StartingSuppliesProfile profile,
            List<StartingSupplyJsonDto>? rows,
            ItemCatalog? catalog,
            string path,
            string profileLabel,
            StartingSuppliesCatalogLoadResult result)
        {
            if (rows == null || rows.Count == 0)
            {
                result.Warnings.Add($"Profile '{profileLabel}' has no supply rows.");
                return false;
            }

            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < rows.Count; i++)
            {
                var dto = rows[i];
                if (dto == null || string.IsNullOrWhiteSpace(dto.itemId))
                {
                    result.Warnings.Add(
                        $"Profile '{profileLabel}' row {i + 1} has no itemId in {path}.");
                    return false;
                }

                if (dto.amount <= 0)
                {
                    result.Warnings.Add(
                        $"Profile '{profileLabel}' row {i + 1} has non-positive amount {dto.amount}.");
                    return false;
                }

                if (!seenIds.Add(dto.itemId))
                {
                    result.Warnings.Add(
                        $"Profile '{profileLabel}' repeats itemId '{dto.itemId}'.");
                    return false;
                }

                string canonicalId = ItemAliases.ToCanonical(dto.itemId);
                if (catalog != null && !catalog.Contains(dto.itemId) &&
                    !catalog.Contains(canonicalId))
                {
                    result.Warnings.Add(
                        $"Profile '{profileLabel}' references unknown item '{dto.itemId}'.");
                    return false;
                }

                profile.supplies.Add((dto.itemId, dto.amount));
            }

            return true;
        }

        private static void LoadFileInto(ItemCatalog catalog, string path, IFileIO fileIO, IJsonSerializer serializer)
        {
            if (!fileIO.FileExists(path)) return;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw)) return;

            try
            {
                var dtos = CatalogLocator.LoadWrappedList<ItemJsonDto>(raw, SystemTextJsonSerializer.Options);
                for (int i = 0; i < dtos.Count; i++)
                {
                    var dto = dtos[i];
                    if (dto == null || string.IsNullOrEmpty(dto.id)) continue;
                    if (catalog.Contains(dto.id)) continue;

                    var def = ConvertDto(dto);
                    catalog.Register(def);
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn("ItemCatalogLoader", path, ex);
            }
        }

        private static bool LoadFileIntoWithResult(
            ItemCatalog catalog, string path, IFileIO fileIO, IJsonSerializer serializer,
            CatalogLoadResult<ItemCatalog> result, bool isPrimary)
        {
            if (!fileIO.FileExists(path))
            {
                if (isPrimary)
                {
                    result.AddFatal("Required catalog file not found: " + path);
                }
                else
                {
                    result.AddInfo("Optional catalog file not found (ok): " + path);
                }
                return false;
            }

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                if (isPrimary)
                {
                    result.AddError("Primary catalog file is empty: " + path);
                }
                else
                {
                    result.AddWarning("Optional catalog file is empty: " + path);
                }
                return false;
            }

            try
            {
                var dtos = CatalogLocator.LoadWrappedList<ItemJsonDto>(raw, SystemTextJsonSerializer.Options);
                for (int i = 0; i < dtos.Count; i++)
                {
                    var dto = dtos[i];
                    if (dto == null || string.IsNullOrEmpty(dto.id)) continue;
                    if (catalog.Contains(dto.id))
                    {
                        result.AddInfo($"Duplicate item id '{dto.id}' encountered in '{path}'. Primary definition retained (provenance collision).");
                        continue;
                    }

                    var def = ConvertDto(dto);
                    catalog.Register(def);
                }
                return true;
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn("ItemCatalogLoader", "LoadItemsFile", ex);
                if (isPrimary)
                {
                    result.AddFatal("Failed to load items: " + ex.Message, ex);
                }
                else
                {
                    result.AddError("Failed to load items (optional): " + ex.Message, ex);
                }
                return false;
            }
        }

        internal static ItemDefinition ConvertDto(ItemJsonDto dto)
        {
            var def = new ItemDefinition
            {
                id = dto.id,
                displayName = !string.IsNullOrEmpty(dto.displayName) ? dto.displayName : dto.id,
                description = dto.description ?? string.Empty,
                iconPath = dto.iconPath ?? string.Empty,
                type = ParseItemType(dto.type),
                stackMax = dto.stackMax > 0 ? dto.stackMax : 1,
                weight = dto.weight,
                radProtection = dto.radProtection,
                durability = dto.durability,
                degradeRate = dto.degradeRate > 0f ? dto.degradeRate : dto.degrade_rate,
                isEquipable = dto.isEquipable,
                equipSlot = EquipSlots.Parse(dto.equipSlot),
                contamination = dto.contamination,
                hungerRestore = dto.hungerRestore,
                thirstRestore = dto.thirstRestore,
                healthEffect = dto.healthEffect,
                radCleanse = dto.radCleanse,
                moraleEffect = dto.moraleEffect,
                decorLocalizedMoraleDelta = dto.decorLocalizedMoraleDelta,
                empShielded = dto.empShielded,
                tradeValue = dto.tradeValue,
                tradeTier = dto.tradeTier,
                disassembleYieldFraction = dto.disassembleYieldFraction > 0f ? dto.disassembleYieldFraction : 0.5f
            };

            if (dto.scrapValue != null)
            {
                for (int j = 0; j < dto.scrapValue.Count; j++)
                {
                    var sc = dto.scrapValue[j];
                    if (sc != null && !string.IsNullOrEmpty(sc.materialId))
                        def.scrapValue.Add(new ScrapYield(sc.materialId, sc.amount));
                }
            }

            if (dto.repairRecipe != null)
            {
                def.repairRecipe = new RepairRecipe
                {
                    hours = dto.repairRecipe.hours,
                    requiresTools = dto.repairRecipe.requiresTools
                };
                if (dto.repairRecipe.costs != null)
                {
                    for (int k = 0; k < dto.repairRecipe.costs.Count; k++)
                    {
                        var c = dto.repairRecipe.costs[k];
                        if (c != null && !string.IsNullOrEmpty(c.materialId))
                            def.repairRecipe.costs.Add(new ScrapYield(c.materialId, c.amount));
                    }
                }
            }

            return def;
        }

        private static ItemType ParseItemType(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return ItemType.Material;
            if (Enum.TryParse<ItemType>(raw, true, out var type)) return type;
            string lower = raw.Trim().ToLowerInvariant();
            switch (lower)
            {
                case "antirad":
                case "anti_rad":
                    return ItemType.AntiRad;
                case "irradiatedwater":
                case "irradiated_water":
                    return ItemType.IrradiatedWater;
                case "contaminatedfood":
                case "contaminated_food":
                    return ItemType.ContaminatedFood;
                case "component":
                case "ammo":
                    return ItemType.Material;
                case "container":
                    return ItemType.Tool;
                case "document":
                    return ItemType.Quest;
                case "equipment":
                    return ItemType.Protective;
                case "media":
                    return ItemType.Comfort;
                default:
                    return ItemType.Material;
            }
        }
    }
}
