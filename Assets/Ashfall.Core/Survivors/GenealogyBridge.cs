// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Plan 217 — Survivor Genealogy & Family Tree Bridge.
    /// Connects GenerationalLineageExtension to the survivor lifecycle:
    /// child birth, marriage, divorce, adoption, death, and kinship affinity calculations.
    /// </summary>
    public sealed class GenealogyBridge
    {
        private readonly GenerationalLineageExtension _lineage;
        private readonly ILog _log;

        public GenerationalLineageExtension Lineage => _lineage;

        public event Action<string, string, string>? OnGenealogyEventLogged; // eventType, dwellerId, description

        public GenealogyBridge(GenerationalLineageExtension lineage, ILog? log = null)
        {
            _lineage = lineage ?? throw new ArgumentNullException(nameof(lineage));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult OnChildBorn(string parentId, string childId, int day = 1)
        {
            if (string.IsNullOrWhiteSpace(parentId) || string.IsNullOrWhiteSpace(childId))
                return ActionResult.Failed("invalid_ids", "genealogy.invalid_dweller_ids");

            _lineage.TickDay(day);
            var result = _lineage.EstablishLineage(parentId, childId, "parent");

            if (result.Status == ActionResult.StatusKind.Success)
            {
                // Inherit or generate family name
                string inheritedName = _lineage.InheritFamilyName(childId, parentId);
                var unit = _lineage.GetFamilyUnit(parentId);
                if (unit != null)
                {
                    if (!unit.memberIds.Contains(childId, StringComparer.OrdinalIgnoreCase))
                    {
                        unit.memberIds.Add(childId);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(inheritedName))
                {
                    _lineage.FormFamilyUnit(inheritedName, day, new[] { parentId, childId });
                }

                OnGenealogyEventLogged?.Invoke("birth", childId, $"Born to {parentId}.");
            }

            return result;
        }

        public ActionResult OnAdoption(string adoptiveParentId, string childId, int day = 1)
        {
            if (string.IsNullOrWhiteSpace(adoptiveParentId) || string.IsNullOrWhiteSpace(childId))
                return ActionResult.Failed("invalid_ids", "genealogy.invalid_dweller_ids");

            _lineage.TickDay(day);
            var result = _lineage.EstablishLineage(adoptiveParentId, childId, "adopted");

            if (result.Status == ActionResult.StatusKind.Success)
            {
                _lineage.InheritFamilyName(childId, adoptiveParentId);
                _lineage.RecordFamilyEvent("adoption", day, new[] { adoptiveParentId, childId },
                    $"{childId} formally adopted by {adoptiveParentId}.", "major");

                var unit = _lineage.GetFamilyUnit(adoptiveParentId);
                if (unit != null && !unit.memberIds.Contains(childId, StringComparer.OrdinalIgnoreCase))
                {
                    unit.memberIds.Add(childId);
                }

                OnGenealogyEventLogged?.Invoke("adoption", childId, $"Adopted by {adoptiveParentId}.");
            }

            return result;
        }

        public bool OnMarriage(string dwellerA, string dwellerB, int day = 1)
        {
            if (string.IsNullOrWhiteSpace(dwellerA) || string.IsNullOrWhiteSpace(dwellerB)) return false;
            if (string.Equals(dwellerA, dwellerB, StringComparison.OrdinalIgnoreCase)) return false;

            _lineage.TickDay(day);
            bool success = _lineage.SetSpouse(dwellerA, dwellerB);
            if (!success) return false;

            var unitA = _lineage.GetFamilyUnit(dwellerA);
            var unitB = _lineage.GetFamilyUnit(dwellerB);

            if (unitA != null && unitB != null && unitA != unitB)
            {
                // Merge units into unitA
                foreach (var member in unitB.memberIds)
                {
                    if (!unitA.memberIds.Contains(member, StringComparer.OrdinalIgnoreCase))
                    {
                        unitA.memberIds.Add(member);
                    }
                }
                unitB.status = "merged";
            }
            else if (unitA != null && unitB == null)
            {
                unitA.memberIds.Add(dwellerB);
            }
            else if (unitB != null && unitA == null)
            {
                unitB.memberIds.Add(dwellerA);
            }
            else
            {
                string familyName = _lineage.GetFamilyName(dwellerA);
                if (string.IsNullOrWhiteSpace(familyName))
                {
                    familyName = _lineage.GetFamilyName(dwellerB);
                }
                if (string.IsNullOrWhiteSpace(familyName))
                {
                    familyName = "Union_" + dwellerA;
                }

                _lineage.FormFamilyUnit(familyName, day, new[] { dwellerA, dwellerB });
            }

            OnGenealogyEventLogged?.Invoke("marriage", dwellerA, $"Married {dwellerB}.");
            return true;
        }

        public bool OnDivorce(string dwellerA, string dwellerB, int day = 1)
        {
            if (string.IsNullOrWhiteSpace(dwellerA) || string.IsNullOrWhiteSpace(dwellerB)) return false;

            _lineage.TickDay(day);
            string spouseA = _lineage.GetSpouse(dwellerA);
            if (!string.Equals(spouseA, dwellerB, StringComparison.OrdinalIgnoreCase))
                return false;

            var recA = _lineage.State.lineages.FirstOrDefault(l => string.Equals(l.childId, dwellerA, StringComparison.OrdinalIgnoreCase));
            var recB = _lineage.State.lineages.FirstOrDefault(l => string.Equals(l.childId, dwellerB, StringComparison.OrdinalIgnoreCase));

            if (recA != null) recA.spouseId = string.Empty;
            if (recB != null) recB.spouseId = string.Empty;

            _lineage.RecordFamilyEvent("divorce", day, new[] { dwellerA, dwellerB },
                $"Union dissolved between {dwellerA} and {dwellerB}.", "major");

            OnGenealogyEventLogged?.Invoke("divorce", dwellerA, $"Divorced {dwellerB}.");
            return true;
        }

        public void OnSurvivorDied(string dwellerId, int day = 1)
        {
            if (string.IsNullOrWhiteSpace(dwellerId)) return;

            _lineage.TickDay(day);
            var unit = _lineage.GetFamilyUnit(dwellerId);

            _lineage.RecordFamilyEvent("death", day, new[] { dwellerId },
                $"{dwellerId} passed away.", "major");

            OnGenealogyEventLogged?.Invoke("death", dwellerId, "Deceased.");
        }

        public float GetKinshipAffinity(string dwellerA, string dwellerB)
        {
            return _lineage.GetKinshipAffinityBonus(dwellerA, dwellerB);
        }

        public IReadOnlyList<string> GetFamilyMembers(string dwellerId)
        {
            var unit = _lineage.GetFamilyUnit(dwellerId);
            return unit != null ? unit.memberIds : Array.Empty<string>();
        }
    }
}
