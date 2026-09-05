using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Ashfall.Core.Catalogs
{
    /// <summary>
    /// Thrown when an institution catalog (culture / diplomacy / sky defense /
    /// sanatorium) fails structural or numeric validation. Carries every
    /// aggregate finding, deterministically ordered, so malformed data fails
    /// loudly at load time instead of surfacing as runtime drift.
    /// </summary>
    public sealed class InstitutionCatalogException : Exception
    {
        public IReadOnlyList<string> Findings { get; }

        public InstitutionCatalogException(string catalogName, IReadOnlyList<string> findings)
            : base(Format(catalogName, findings))
        {
            Findings = findings;
        }

        private static string Format(string catalogName, IReadOnlyList<string> findings)
        {
            var sb = new StringBuilder();
            sb.Append(catalogName).Append(": ").Append(findings.Count).Append(" validation finding(s):");
            for (int i = 0; i < findings.Count; i++)
                sb.Append("\n  - ").Append(findings[i]);
            return sb.ToString();
        }
    }

    /// <summary>
    /// Shared parse/validation helpers for the four flagship institution
    /// catalogs. All findings are aggregated and sorted deterministically
    /// (entry id, then field) before a single loud failure.
    /// </summary>
    public static class InstitutionCatalogParse
    {
        /// <summary>Accumulates validation findings for one catalog load.</summary>
        public sealed class Findings
        {
            private readonly List<(string Id, string Field, string Message)> _items = new();

            public void Add(string id, string field, string message) =>
                _items.Add((id ?? string.Empty, field ?? string.Empty, message ?? string.Empty));

            public void RequireNonEmpty(string id, string field, string value)
            {
                if (string.IsNullOrWhiteSpace(value))
                    Add(id, field, "must be a non-empty string");
            }

            public void RequirePositive(int id, string field, int value, string unit) =>
                RequirePositive(id.ToString(CultureInfo.InvariantCulture), field, value, unit);

            public void RequirePositive(string id, string field, int value, string unit)
            {
                if (value <= 0)
                    Add(id, field, $"must be > 0 {unit}, got {value}");
            }

            public void RequireAtLeastZero(string id, string field, float value)
            {
                if (float.IsNaN(value) || float.IsInfinity(value))
                    Add(id, field, "must be a finite number");
                else if (value < 0f)
                    Add(id, field, $"must be >= 0, got {value.ToString(CultureInfo.InvariantCulture)}");
            }

            public void RequireRange(string id, string field, float value, float min, float max)
            {
                if (float.IsNaN(value) || float.IsInfinity(value))
                {
                    Add(id, field, "must be a finite number");
                    return;
                }
                if (value < min || value > max)
                    Add(id, field, $"must be within [{min.ToString(CultureInfo.InvariantCulture)}, {max.ToString(CultureInfo.InvariantCulture)}], got {value.ToString(CultureInfo.InvariantCulture)}");
            }

            public void RequireRange(string id, string field, int value, int min, int max)
            {
                if (value < min || value > max)
                    Add(id, field, $"must be within [{min}, {max}], got {value}");
            }

            public void RequireCostItems(string id, string field, List<CatalogCostEntry>? costs)
            {
                if (costs == null) return;
                for (int i = 0; i < costs.Count; i++)
                {
                    var cost = costs[i];
                    if (string.IsNullOrWhiteSpace(cost.item_id))
                        Add(id, $"{field}[{i}].item_id", "must be a non-empty item id");
                    if (cost.amount <= 0)
                        Add(id, $"{field}[{i}].amount", $"must be > 0 for item '{cost.item_id}', got {cost.amount}");
                }
            }

            /// <summary>Throws if any finding exists; orders findings by (id, field) first.</summary>
            public void ThrowIfAny(string catalogName)
            {
                if (_items.Count == 0) return;
                _items.Sort((a, b) =>
                {
                    int c = string.CompareOrdinal(a.Id, b.Id);
                    return c != 0 ? c : string.CompareOrdinal(a.Field, b.Field);
                });
                var messages = new List<string>(_items.Count);
                foreach (var (id, field, message) in _items)
                    messages.Add($"{id}/{field}: {message}");
                throw new InstitutionCatalogException(catalogName, messages);
            }
        }

        /// <summary>A single {item_id, amount} cost line shared by all institution catalogs.</summary>
        public sealed class CatalogCostEntry
        {
            public string item_id = string.Empty;
            public int amount = 1;
        }

        /// <summary>Canonical snake_case id check used for authored definition ids.</summary>
        public static bool IsCanonicalSnakeCase(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            if (id != id.ToLowerInvariant()) return false;
            foreach (char c in id)
            {
                bool ok = (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_';
                if (!ok) return false;
            }
            return true;
        }
    }
}
