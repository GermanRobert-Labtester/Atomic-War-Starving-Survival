// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Hygiene
{
    /// <summary>
    /// Explicit categorization of repository artifacts preventing untracked weight and root clutter.
    /// </summary>
    public enum RepositoryArtifactCategory
    {
        SourceCode = 0,
        DataAuthority = 1,
        ShippedAssets = 2,
        DesignMockups = 3,
        GeneratedArtifacts = 4,
        TestProject = 5,
        Ephemera = 6,
        ProhibitedOrphan = 7
    }

    /// <summary>
    /// Budget specification for an asset family (e.g. art, ui, sprites, audio, fonts).
    /// </summary>
    public sealed class AssetFamilyBudget
    {
        public string FamilyName { get; set; } = string.Empty;
        public long MaxBudgetBytes { get; set; }
        public long CurrentSizeBytes { get; set; }

        public bool IsWithinBudget => CurrentSizeBytes <= MaxBudgetBytes;
        public double UsagePercentage => MaxBudgetBytes > 0 ? Math.Round(((double)CurrentSizeBytes / MaxBudgetBytes) * 100.0, 2) : 0.0;

        public AssetFamilyBudget() { }

        public AssetFamilyBudget(string familyName, long maxBudgetBytes, long currentSizeBytes = 0)
        {
            FamilyName = familyName ?? string.Empty;
            MaxBudgetBytes = Math.Max(0, maxBudgetBytes);
            CurrentSizeBytes = Math.Max(0, currentSizeBytes);
        }
    }

    /// <summary>
    /// Path classifier and hygiene validator for repository assets, artifacts, and root files.
    /// Pure domain authority without engine dependencies.
    /// </summary>
    public sealed class RepositoryClassificationPolicy
    {
        private readonly Dictionary<string, AssetFamilyBudget> _familyBudgets =
            new Dictionary<string, AssetFamilyBudget>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Delegate seam fired whenever a path is classified.</summary>
        public Action<string, RepositoryArtifactCategory>? OnPathClassifiedSeam { get; set; }

        /// <summary>Delegate seam fired whenever a hygiene violation is detected.</summary>
        public Action<string, string>? OnHygieneViolationSeam { get; set; }

        public RepositoryClassificationPolicy()
        {
            RegisterDefaultBudgets();
        }

        public void RegisterBudget(AssetFamilyBudget budget)
        {
            if (budget == null || string.IsNullOrWhiteSpace(budget.FamilyName))
                throw new ArgumentException("Budget must have a valid family name.", nameof(budget));

            _familyBudgets[budget.FamilyName] = budget;
        }

        public bool TryGetBudget(string familyName, out AssetFamilyBudget? budget)
        {
            if (string.IsNullOrWhiteSpace(familyName))
            {
                budget = null;
                return false;
            }
            return _familyBudgets.TryGetValue(familyName, out budget);
        }

        /// <summary>
        /// Classifies a project-relative file path into an artifact category.
        /// Strictly identifies prohibited root files and misplaced mockups.
        /// </summary>
        public RepositoryArtifactCategory ClassifyPath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return RepositoryArtifactCategory.ProhibitedOrphan;

            string normalized = relativePath.Replace('\\', '/').TrimStart('/');

            // 1. Prohibited Root Files (stray XML results, legacy fix scripts, root test junk)
            if (!normalized.Contains('/'))
            {
                if (normalized.EndsWith(".xml", StringComparison.OrdinalIgnoreCase) ||
                    normalized.StartsWith("fix_", StringComparison.OrdinalIgnoreCase) ||
                    normalized.StartsWith("safe_fix", StringComparison.OrdinalIgnoreCase) ||
                    normalized.Equals("test_parse.py", StringComparison.OrdinalIgnoreCase) ||
                    normalized.Equals("export_code.py", StringComparison.OrdinalIgnoreCase))
                {
                    OnHygieneViolationSeam?.Invoke(normalized, "Prohibited stray file at repository root.");
                    OnPathClassifiedSeam?.Invoke(normalized, RepositoryArtifactCategory.ProhibitedOrphan);
                    return RepositoryArtifactCategory.ProhibitedOrphan;
                }
            }

            // 2. Data Authority
            if (normalized.StartsWith("Assets/StreamingAssets/Data/", StringComparison.OrdinalIgnoreCase))
            {
                OnPathClassifiedSeam?.Invoke(normalized, RepositoryArtifactCategory.DataAuthority);
                return RepositoryArtifactCategory.DataAuthority;
            }

            // 3. Source Code
            if (normalized.StartsWith("Assets/Ashfall.Core/", StringComparison.OrdinalIgnoreCase) ||
                normalized.StartsWith("src/", StringComparison.OrdinalIgnoreCase))
            {
                OnPathClassifiedSeam?.Invoke(normalized, RepositoryArtifactCategory.SourceCode);
                return RepositoryArtifactCategory.SourceCode;
            }

            // 4. Test Project
            if (normalized.StartsWith("Ashfall.Core.Tests/", StringComparison.OrdinalIgnoreCase))
            {
                OnPathClassifiedSeam?.Invoke(normalized, RepositoryArtifactCategory.TestProject);
                return RepositoryArtifactCategory.TestProject;
            }

            // 5. Ephemera (.godot, obj, bin, TestResults)
            if (normalized.StartsWith(".godot/", StringComparison.OrdinalIgnoreCase) ||
                normalized.Contains("/obj/") ||
                normalized.Contains("/bin/") ||
                normalized.Contains("TestResults/"))
            {
                OnPathClassifiedSeam?.Invoke(normalized, RepositoryArtifactCategory.Ephemera);
                return RepositoryArtifactCategory.Ephemera;
            }

            // 6. Generated Artifacts
            if (normalized.StartsWith("artifacts/", StringComparison.OrdinalIgnoreCase) ||
                normalized.StartsWith("snapshots/", StringComparison.OrdinalIgnoreCase) ||
                normalized.StartsWith("snapshot-capture/", StringComparison.OrdinalIgnoreCase))
            {
                OnPathClassifiedSeam?.Invoke(normalized, RepositoryArtifactCategory.GeneratedArtifacts);
                return RepositoryArtifactCategory.GeneratedArtifacts;
            }

            // 7. Design Mockups
            if (normalized.StartsWith("docs/design/", StringComparison.OrdinalIgnoreCase) ||
                normalized.StartsWith("docs/archive/", StringComparison.OrdinalIgnoreCase) ||
                normalized.StartsWith("assets/ui/HtmlBundles/", StringComparison.OrdinalIgnoreCase) ||
                normalized.StartsWith("assets/ui/Screens/", StringComparison.OrdinalIgnoreCase))
            {
                OnPathClassifiedSeam?.Invoke(normalized, RepositoryArtifactCategory.DesignMockups);
                return RepositoryArtifactCategory.DesignMockups;
            }

            // 8. Shipped Assets
            if (normalized.StartsWith("assets/", StringComparison.OrdinalIgnoreCase))
            {
                OnPathClassifiedSeam?.Invoke(normalized, RepositoryArtifactCategory.ShippedAssets);
                return RepositoryArtifactCategory.ShippedAssets;
            }

            OnPathClassifiedSeam?.Invoke(normalized, RepositoryArtifactCategory.SourceCode);
            return RepositoryArtifactCategory.SourceCode;
        }

        private void RegisterDefaultBudgets()
        {
            // Plan 56 asset budgets
            RegisterBudget(new AssetFamilyBudget("art", 85L * 1024 * 1024));     // 85 MB
            RegisterBudget(new AssetFamilyBudget("ui", 35L * 1024 * 1024));      // 35 MB
            RegisterBudget(new AssetFamilyBudget("sprites", 12L * 1024 * 1024)); // 12 MB
            RegisterBudget(new AssetFamilyBudget("audio", 15L * 1024 * 1024));   // 15 MB
            RegisterBudget(new AssetFamilyBudget("fonts", 5L * 1024 * 1024));    // 5 MB
        }
    }
}
