// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Inventory
{
    // ── DTOs (snake_case JSON authority) ────────────────────────────────────

    public sealed class SalvageComponentDto
    {
        [JsonPropertyName("item_id")] public string item_id { get; set; } = string.Empty;
        [JsonPropertyName("amount")] public int amount { get; set; }
    }

    public sealed class SalvageRecipeDto
    {
        [JsonPropertyName("id")] public string id { get; set; } = string.Empty;
        [JsonPropertyName("source_item_id")] public string source_item_id { get; set; } = string.Empty;
        [JsonPropertyName("display_name")] public string display_name { get; set; } = string.Empty;
        [JsonPropertyName("components")] public List<SalvageComponentDto> components { get; set; } = new();
        [JsonPropertyName("required_tool_id")] public string? required_tool_id { get; set; }
        [JsonPropertyName("tool_wear")] public float tool_wear { get; set; }
    }

    // ── Domain ──────────────────────────────────────────────────────────────

    /// <summary>One yielded stack of a teardown.</summary>
    public readonly struct SalvageComponent
    {
        public SalvageComponent(string itemId, int amount) { ItemId = itemId; Amount = amount; }
        public string ItemId { get; }
        public int Amount { get; }
    }

    /// <summary>Authored teardown recipe: one surplus item → serviceable parts.</summary>
    public sealed class SalvageRecipe
    {
        public SalvageRecipe(string id, string sourceItemId, string displayName,
            IReadOnlyList<SalvageComponent> components, string? requiredToolId, float toolWear)
        {
            Id = id;
            SourceItemId = sourceItemId;
            DisplayName = displayName;
            Components = components;
            RequiredToolId = requiredToolId;
            ToolWear = toolWear;
        }

        public string Id { get; }
        public string SourceItemId { get; }
        public string DisplayName { get; }
        public IReadOnlyList<SalvageComponent> Components { get; }
        public string? RequiredToolId { get; }
        public float ToolWear { get; }
        public bool RequiresTool => !string.IsNullOrEmpty(RequiredToolId) && ToolWear > 0f;
    }

    public enum SalvageOutcome
    {
        Success,
        NoRecipe,
        InsufficientSource,
        MissingTool,
        NoCapacity,
    }

    public readonly struct SalvageResult
    {
        public SalvageResult(SalvageOutcome outcome, string message,
            IReadOnlyList<SalvageComponent> yielded, string wornToolId, float toolWear)
        {
            Outcome = outcome;
            Message = message;
            Yielded = yielded ?? Array.Empty<SalvageComponent>();
            WornToolId = wornToolId ?? string.Empty;
            ToolWear = toolWear;
        }

        public SalvageOutcome Outcome { get; }
        public string Message { get; }
        public IReadOnlyList<SalvageComponent> Yielded { get; }
        public string? WornToolId { get; }
        public float ToolWear { get; }
        public bool IsSuccess => Outcome == SalvageOutcome.Success;
    }

    /// <summary>
    /// Engine-agnostic teardown catalog. Loads the authored
    /// <c>salvage_teardown.json</c> wrapped list; no engine or host deps.
    /// </summary>
    public sealed class SalvageTeardownCatalog
    {
        public const string FileName = "salvage_teardown.json";

        private readonly Dictionary<string, SalvageRecipe> _bySource =
            new(StringComparer.OrdinalIgnoreCase);
        private readonly List<SalvageRecipe> _all = new();

        public IReadOnlyList<SalvageRecipe> All => _all;
        public int Count => _all.Count;
        public IReadOnlyList<string> Errors { get; private set; } = Array.Empty<string>();

        public bool TryGet(string sourceItemId, out SalvageRecipe recipe)
        {
            recipe = null!;
            if (string.IsNullOrEmpty(sourceItemId)) return false;
            return _bySource.TryGetValue(sourceItemId, out recipe!);
        }

        public static SalvageTeardownCatalog Load(string json)
        {
            var catalog = new SalvageTeardownCatalog();
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(json))
            {
                catalog.Errors = new[] { "salvage teardown catalog is empty" };
                return catalog;
            }

            List<SalvageRecipeDto>? dtos;
            try
            {
                dtos = CatalogLocator.LoadWrappedList<SalvageRecipeDto>(json, SystemTextJsonSerializer.Options);
            }
            catch (Exception ex)
            {
                errors.Add($"salvage teardown catalog parse failed: {ex.Message}");
                catalog.Errors = errors;
                return catalog;
            }

            foreach (var dto in dtos)
            {
                if (dto == null || string.IsNullOrWhiteSpace(dto.source_item_id))
                {
                    errors.Add("salvage recipe without source_item_id skipped");
                    continue;
                }
                if (dto.components == null || dto.components.Count == 0)
                {
                    errors.Add($"salvage recipe '{dto.id}' has no components; skipped");
                    continue;
                }

                var components = new List<SalvageComponent>(dto.components.Count);
                bool valid = true;
                foreach (var c in dto.components)
                {
                    if (c == null || string.IsNullOrWhiteSpace(c.item_id) || c.amount <= 0)
                    {
                        errors.Add($"salvage recipe '{dto.id}' has an invalid component; skipped");
                        valid = false;
                        break;
                    }
                    components.Add(new SalvageComponent(c.item_id, c.amount));
                }
                if (!valid) continue;

                var recipe = new SalvageRecipe(
                    string.IsNullOrWhiteSpace(dto.id) ? $"teardown_{dto.source_item_id}" : dto.id,
                    dto.source_item_id,
                    string.IsNullOrWhiteSpace(dto.display_name) ? dto.source_item_id : dto.display_name,
                    components,
                    string.IsNullOrWhiteSpace(dto.required_tool_id) ? null : dto.required_tool_id,
                    Math.Max(0f, dto.tool_wear));

                if (catalog._bySource.ContainsKey(recipe.SourceItemId))
                {
                    errors.Add($"duplicate teardown recipe for '{recipe.SourceItemId}' skipped");
                    continue;
                }
                catalog._bySource[recipe.SourceItemId] = recipe;
                catalog._all.Add(recipe);
            }

            catalog.Errors = errors;
            return catalog;
        }
    }

    /// <summary>
    /// Salvage teardown bench: consumes one surplus item and yields its
    /// components. Mutates only the canonical <see cref="InventoryContainer"/>
    /// (items + slot durability) — no new save section, no parallel state.
    /// Tool wear uses the existing per-slot durability owner.
    /// </summary>
    public sealed class SalvageTeardownSystem
    {
        private readonly SalvageTeardownCatalog _catalog;
        private readonly Func<string, ItemDefinition?>? _resolveItem;

        /// <param name="resolveItem">Optional catalog resolver so yielded
        /// components keep their authored definitions (display name, weight).
        /// When null, components are added by id with a minimal definition.</param>
        public SalvageTeardownSystem(SalvageTeardownCatalog catalog,
            Func<string, ItemDefinition?>? resolveItem = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _resolveItem = resolveItem;
        }

        public SalvageTeardownCatalog Catalog => _catalog;

        public bool CanTeardown(string sourceItemId) => _catalog.TryGet(sourceItemId, out _);

        public SalvageResult TryTeardown(InventoryContainer inventory, string sourceItemId, int count = 1)
        {
            if (inventory == null) throw new ArgumentNullException(nameof(inventory));
            if (!_catalog.TryGet(sourceItemId, out var recipe))
                return new SalvageResult(SalvageOutcome.NoRecipe,
                    $"{sourceItemId}: no teardown recipe.", Array.Empty<SalvageComponent>(), string.Empty, 0f);

            count = Math.Max(1, count);
            if (!inventory.HasSufficient(recipe.SourceItemId, count))
                return new SalvageResult(SalvageOutcome.InsufficientSource,
                    $"{recipe.DisplayName}: need {count} in storage.", Array.Empty<SalvageComponent>(), string.Empty, 0f);

            InventorySlot? toolSlot = null;
            if (recipe.RequiresTool)
            {
                toolSlot = inventory.FindSlot(recipe.RequiredToolId!);
                if (toolSlot == null || toolSlot.Amount <= 0)
                    return new SalvageResult(SalvageOutcome.MissingTool,
                        $"{recipe.DisplayName}: requires {recipe.RequiredToolId} at the bench.",
                        Array.Empty<SalvageComponent>(), string.Empty, 0f);
            }

            // Capacity pre-check keeps the operation atomic (no consume without yield).
            var yielded = new List<SalvageComponent>(recipe.Components.Count);
            foreach (var component in recipe.Components)
            {
                int total = component.Amount * count;
                var def = _resolveItem?.Invoke(component.ItemId);
                bool canAdd = def != null
                    ? inventory.CanAdd(def, total)
                    : inventory.CanAddById(component.ItemId, total);
                if (!canAdd)
                    return new SalvageResult(SalvageOutcome.NoCapacity,
                        $"{recipe.DisplayName}: no room for {component.ItemId} ×{total}.",
                        Array.Empty<SalvageComponent>(), string.Empty, 0f);
                yielded.Add(new SalvageComponent(component.ItemId, total));
            }

            if (!inventory.TryConsume(recipe.SourceItemId, count))
                return new SalvageResult(SalvageOutcome.InsufficientSource,
                    $"{recipe.DisplayName}: storage changed before teardown.", Array.Empty<SalvageComponent>(),
                    string.Empty, 0f);

            foreach (var component in yielded)
            {
                var def = _resolveItem?.Invoke(component.ItemId);
                if (def != null) inventory.Add(def, component.Amount);
                else inventory.AddById(component.ItemId, component.Amount);
            }

            float wear = 0f;
            if (toolSlot != null)
            {
                wear = recipe.ToolWear * count;
                float durability = toolSlot.GetDurability();
                toolSlot.CurrentDurability = Math.Max(0f, durability - wear);
            }

            return new SalvageResult(SalvageOutcome.Success,
                $"{recipe.DisplayName} → {yielded.Count} component stack(s).",
                yielded,
                toolSlot != null ? recipe.RequiredToolId! : string.Empty,
                wear);
        }
    }
}
