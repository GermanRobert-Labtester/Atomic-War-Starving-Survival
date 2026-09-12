// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Crafting
{
    /// <summary>
    /// Cross-catalog integrity for trap-crafting recipes (flagship trapping
    /// tranche, workstream D). A recipe whose identity marks it as a trap
    /// (recipe id "craft_trap_*" or result item id "trap_*") must resolve to:
    ///
    ///   1. a real item definition (guaranteed by RecipeCatalogLoader strict
    ///      mode for loaded recipes, re-checked here for synthetic fixtures);
    ///   2. a trap definition in the wildlife trapping catalog whose
    ///      <see cref="Ashfall.Core.TrapDefinition.trap_id"/> equals the
    ///      result item id — the identity chain
    ///      Recipe → Crafted Item → Trap Definition must not break, or the
    ///      crafted item can never be deployed through TrySetTrap().
    ///
    /// Pure static validation; no engine coupling (Invariant 1). Used by the
    /// test suite and available to host selftests as a first-line defense
    /// alongside CatalogIntegrityValidator.
    /// </summary>
    public static class TrapRecipeIntegrity
    {
        /// <summary>Item-id prefix marking a deployable trap item.</summary>
        public const string TrapItemPrefix = "trap_";

        /// <summary>Recipe-id prefix marking a trap-crafting recipe.</summary>
        public const string TrapRecipePrefix = "craft_trap_";

        /// <summary>
        /// Validate every trap-marked recipe against the trap definition
        /// catalog. Returns one human-readable error per violation; empty
        /// list means the chain is intact.
        /// </summary>
        public static List<string> Validate(
            IEnumerable<Recipe>? recipes,
            IReadOnlyDictionary<string, TrapDefinition>? trapCatalog)
        {
            var errors = new List<string>();
            if (recipes == null) return errors;

            foreach (var recipe in recipes)
            {
                if (recipe == null) continue;
                if (!IsTrapRecipe(recipe)) continue;

                string resultId = recipe.result?.id ?? string.Empty;
                if (string.IsNullOrEmpty(resultId))
                {
                    errors.Add(
                        $"recipe '{recipe.id}' is a trap recipe but its result item does not resolve " +
                        "(result item missing from items.json)");
                    continue;
                }

                if (trapCatalog == null || !trapCatalog.ContainsKey(resultId))
                {
                    errors.Add(
                        $"recipe '{recipe.id}' result item '{resultId}' resolves in items.json but has no " +
                        "matching trap definition in wildlife_trapping_catalog.json — the crafted trap " +
                        "could never be deployed (trap definition identity mismatch)");
                }
            }

            return errors;
        }

        /// <summary>True when the recipe identity marks it as a trap recipe.</summary>
        public static bool IsTrapRecipe(Recipe recipe)
        {
            if (recipe == null) return false;
            return (!string.IsNullOrEmpty(recipe.id)
                        && recipe.id.StartsWith(TrapRecipePrefix, StringComparison.Ordinal))
                    || (recipe.result?.id != null
                        && recipe.result.id.StartsWith(TrapItemPrefix, StringComparison.Ordinal));
        }
    }
}
