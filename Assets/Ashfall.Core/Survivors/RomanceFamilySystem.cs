// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ashfall.Core.Random;

namespace Ashfall.Core.Survivors
{
    public enum RomanceStage
    {
        Attraction = 0,
        Courtship = 1,
        Partnership = 2,
        Bonded = 3
    }

    public sealed class RomanticRelationship
    {
        public string SurvivorA { get; set; } = string.Empty;
        public string SurvivorB { get; set; } = string.Empty;
        public RomanceStage Stage { get; set; } = RomanceStage.Attraction;
        public int RomanceScore { get; set; } = 0; // 0..100
        public float Compatibility { get; set; } = 50.0f; // 0..100
        public int StartDay { get; set; } = 1;
        public int LastInteractionDay { get; set; } = 1;
        public int BondedDaysCount { get; set; } = 0;
        public bool IsSoulmate { get; set; } = false;
        public string? CohabitationQuarters { get; set; }

        public bool Matches(string idA, string idB)
        {
            return (string.Equals(SurvivorA, idA, StringComparison.OrdinalIgnoreCase) && string.Equals(SurvivorB, idB, StringComparison.OrdinalIgnoreCase)) ||
                   (string.Equals(SurvivorA, idB, StringComparison.OrdinalIgnoreCase) && string.Equals(SurvivorB, idA, StringComparison.OrdinalIgnoreCase));
        }

        public string GetOther(string id)
        {
            return string.Equals(SurvivorA, id, StringComparison.OrdinalIgnoreCase) ? SurvivorB : SurvivorA;
        }
    }

    public sealed class FamilyUnit
    {
        public string FamilyId { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public List<string> ParentIds { get; set; } = new List<string>();
        public List<string> ChildIds { get; set; } = new List<string>();
        public List<string> ExtendedFamilyIds { get; set; } = new List<string>();
        public float FamilyBond { get; set; } = 50.0f; // 0..100

        public bool ContainsMember(string survivorId)
        {
            return ParentIds.Exists(p => string.Equals(p, survivorId, StringComparison.OrdinalIgnoreCase)) ||
                   ChildIds.Exists(c => string.Equals(c, survivorId, StringComparison.OrdinalIgnoreCase)) ||
                   ExtendedFamilyIds.Exists(e => string.Equals(e, survivorId, StringComparison.OrdinalIgnoreCase));
        }
    }

    public sealed class CourtshipEventDef
    {
        public string EventId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int MinAffinity { get; set; } = 20;
        public float BaseSuccessRate { get; set; } = 0.70f;
        public int ScoreGain { get; set; } = 8;
        public float StressReduction { get; set; } = 5.0f;
        public string Description { get; set; } = string.Empty;
    }

    public sealed class RomanceCourtshipCatalog
    {
        public List<CourtshipEventDef> CourtshipEvents { get; } = new List<CourtshipEventDef>();
        public float AgePenaltyPerExcessYear { get; set; } = 2.0f;
        public int AgeDifferenceSoftLimit { get; set; } = 12;
        public float BeliefAlignmentBonus { get; set; } = 15.0f;
        public float OpposingBeliefPenalty { get; set; } = 20.0f;
        public float SharedTraumaBondBonus { get; set; } = 10.0f;
        public float BaseCompatibilityFloor { get; set; } = 20.0f;

        /// <summary>
        /// Binds rows that have already been validated by
        /// <see cref="RomanceCourtshipCatalogLoader"/>. This is the seam the
        /// host uses, so the lenient <see cref="LoadFromJson"/> parser can
        /// never turn an authored typo into a live courtship event. Replaces
        /// any previously bound rows.
        /// </summary>
        public void BindValidatedEvents(IEnumerable<CourtshipEventDef> events)
        {
            CourtshipEvents.Clear();
            if (events == null) return;
            foreach (var evt in events)
            {
                if (evt != null) CourtshipEvents.Add(evt);
            }
        }

        public static RomanceCourtshipCatalog LoadFromJson(string json)
        {
            var catalog = new RomanceCourtshipCatalog();
            if (string.IsNullOrWhiteSpace(json)) return catalog;

            int eventsStart = json.IndexOf("\"courtship_events\"", StringComparison.Ordinal);
            if (eventsStart >= 0)
            {
                int arrStart = json.IndexOf('[', eventsStart);
                int arrEnd = json.IndexOf(']', arrStart);
                if (arrStart >= 0 && arrEnd > arrStart)
                {
                    string arrayText = json.Substring(arrStart + 1, arrEnd - arrStart - 1);
                    int idx = 0;
                    while (idx < arrayText.Length)
                    {
                        int objStart = arrayText.IndexOf('{', idx);
                        if (objStart < 0) break;
                        int depth = 0;
                        int objEnd = -1;
                        for (int i = objStart; i < arrayText.Length; i++)
                        {
                            if (arrayText[i] == '{') depth++;
                            else if (arrayText[i] == '}')
                            {
                                depth--;
                                if (depth == 0) { objEnd = i; break; }
                            }
                        }
                        if (objEnd < 0) break;

                        string objText = arrayText.Substring(objStart, objEnd - objStart + 1);
                        var evt = new CourtshipEventDef
                        {
                            EventId = ExtractString(objText, "event_id"),
                            Name = ExtractString(objText, "name"),
                            MinAffinity = ExtractInt(objText, "min_affinity", 20),
                            BaseSuccessRate = ExtractFloat(objText, "base_success_rate", 0.70f),
                            ScoreGain = ExtractInt(objText, "score_gain", 8),
                            StressReduction = ExtractFloat(objText, "stress_reduction", 5.0f),
                            Description = ExtractString(objText, "description")
                        };
                        if (!string.IsNullOrEmpty(evt.EventId))
                        {
                            catalog.CourtshipEvents.Add(evt);
                        }
                        idx = objEnd + 1;
                    }
                }
            }

            return catalog;
        }

        private static string ExtractString(string json, string key)
        {
            string pattern = $"\"{key}\"";
            int keyIdx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (keyIdx < 0) return string.Empty;
            int colon = json.IndexOf(':', keyIdx + pattern.Length);
            if (colon < 0) return string.Empty;
            int quoteStart = json.IndexOf('"', colon + 1);
            if (quoteStart < 0) return string.Empty;
            int quoteEnd = json.IndexOf('"', quoteStart + 1);
            if (quoteEnd < 0) return string.Empty;
            return json.Substring(quoteStart + 1, quoteEnd - quoteStart - 1).Trim();
        }

        private static int ExtractInt(string json, string key, int defaultValue)
        {
            string pattern = $"\"{key}\"";
            int keyIdx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (keyIdx < 0) return defaultValue;
            int colon = json.IndexOf(':', keyIdx + pattern.Length);
            if (colon < 0) return defaultValue;
            int start = colon + 1;
            while (start < json.Length && (char.IsWhiteSpace(json[start]) || json[start] == '"')) start++;
            int end = start;
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '-')) end++;
            if (end > start && int.TryParse(json.Substring(start, end - start), out int val))
            {
                return val;
            }
            return defaultValue;
        }

        private static float ExtractFloat(string json, string key, float defaultValue)
        {
            string pattern = $"\"{key}\"";
            int keyIdx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (keyIdx < 0) return defaultValue;
            int colon = json.IndexOf(':', keyIdx + pattern.Length);
            if (colon < 0) return defaultValue;
            int start = colon + 1;
            while (start < json.Length && (char.IsWhiteSpace(json[start]) || json[start] == '"')) start++;
            int end = start;
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '.' || json[end] == '-')) end++;
            if (end > start && float.TryParse(json.Substring(start, end - start), NumberStyles.Float, CultureInfo.InvariantCulture, out float val))
            {
                return val;
            }
            return defaultValue;
        }
    }

    /// <summary>
    /// Plan 150: Romance & Family Dynamics System.
    /// Manages survivor romantic relationship lifecycles (Attraction -> Courtship -> Partnership -> Bonded),
    /// family units, adoption, mutual support bonuses, grief impacts, and persistent state.
    /// Engine-neutral, deterministic, and save/load persistent.
    /// </summary>
    public sealed class RomanceFamilySystem
    {
        public delegate void RomanceStageAdvancedDelegate(string survivorA, string survivorB, RomanceStage newStage);
        public delegate void FamilyUnitEstablishedDelegate(string familyId, IReadOnlyList<string> parentIds);
        public delegate void PartnershipDissolvedDelegate(string survivorA, string survivorB, string reason);
        public delegate void ChildWelcomedDelegate(string familyId, string childId, bool isAdopted);

        public event RomanceStageAdvancedDelegate? OnRomanceStageAdvancedSeam;
        public event FamilyUnitEstablishedDelegate? OnFamilyUnitEstablishedSeam;
        public event PartnershipDissolvedDelegate? OnPartnershipDissolvedSeam;
        public event ChildWelcomedDelegate? OnChildWelcomedToFamilySeam;

        private readonly RomanceCourtshipCatalog _catalog;
        private readonly List<RomanticRelationship> _relationships = new List<RomanticRelationship>();
        private readonly List<FamilyUnit> _familyUnits = new List<FamilyUnit>();

        /// <summary>Save schema this system writes and accepts.</summary>
        public const int StateSchemaVersion = 1;

        public IReadOnlyList<RomanticRelationship> Relationships => _relationships;
        public IReadOnlyList<FamilyUnit> FamilyUnits => _familyUnits;
        public RomanceCourtshipCatalog Catalog => _catalog;

        /// <summary>
        /// Read-only summary of live romance/family state. Mirrors the census
        /// contract the integrated systems expose for the architecture
        /// scanner and the host probe.
        /// </summary>
        public RomanceFamilyCensus GetCensus()
        {
            int bonded = 0;
            int partnered = 0;
            foreach (var rel in _relationships)
            {
                if (rel.Stage == RomanceStage.Bonded) bonded++;
                if (rel.Stage == RomanceStage.Partnership) partnered++;
            }

            int children = 0;
            foreach (var family in _familyUnits) children += family.ChildIds.Count;

            return new RomanceFamilyCensus(
                _relationships.Count,
                bonded,
                partnered,
                _familyUnits.Count,
                children,
                _catalog.CourtshipEvents.Count);
        }

        public RomanceFamilySystem(RomanceCourtshipCatalog? catalog = null)
        {
            _catalog = catalog ?? new RomanceCourtshipCatalog();
        }

        public float CalculateCompatibility(int ageA, int ageB, string beliefA, string beliefB, bool sharedTrauma)
        {
            float score = 50.0f;

            int ageDiff = Math.Abs(ageA - ageB);
            if (ageDiff > _catalog.AgeDifferenceSoftLimit)
            {
                int excess = ageDiff - _catalog.AgeDifferenceSoftLimit;
                score -= excess * _catalog.AgePenaltyPerExcessYear;
            }

            if (!string.IsNullOrEmpty(beliefA) && !string.IsNullOrEmpty(beliefB))
            {
                if (string.Equals(beliefA, beliefB, StringComparison.OrdinalIgnoreCase))
                {
                    score += _catalog.BeliefAlignmentBonus;
                }
                else if (AreOpposingBeliefs(beliefA, beliefB))
                {
                    score -= _catalog.OpposingBeliefPenalty;
                }
            }

            if (sharedTrauma)
            {
                score += _catalog.SharedTraumaBondBonus;
            }

            return Math.Max(_catalog.BaseCompatibilityFloor, Math.Min(100.0f, score));
        }

        private static bool AreOpposingBeliefs(string a, string b)
        {
            string pair1 = $"{a.ToLowerInvariant()}:{b.ToLowerInvariant()}";
            string pair2 = $"{b.ToLowerInvariant()}:{a.ToLowerInvariant()}";
            return pair1 == "collectivism:individualism" || pair2 == "collectivism:individualism" ||
                   pair1 == "militarism:pacifism" || pair2 == "militarism:pacifism" ||
                   pair1 == "technocracy:traditionalism" || pair2 == "technocracy:traditionalism";
        }

        public RomanticRelationship? GetRelationship(string survivorA, string survivorB)
        {
            return _relationships.FirstOrDefault(r => r.Matches(survivorA, survivorB));
        }

        public RomanticRelationship? GetRomanticPartner(string survivorId)
        {
            return _relationships.FirstOrDefault(r => (string.Equals(r.SurvivorA, survivorId, StringComparison.OrdinalIgnoreCase) ||
                                                       string.Equals(r.SurvivorB, survivorId, StringComparison.OrdinalIgnoreCase)) &&
                                                      r.Stage >= RomanceStage.Partnership);
        }

        public FamilyUnit? GetFamilyForSurvivor(string survivorId)
        {
            return _familyUnits.FirstOrDefault(f => f.ContainsMember(survivorId));
        }

        public bool TryInitiateAttraction(
            string survivorA,
            string survivorB,
            float affinity,
            int ageA,
            int ageB,
            string beliefA,
            string beliefB,
            bool sharedTrauma,
            ISeededRng rng,
            int currentDay = 1,
            bool force = false)
        {
            if (string.Equals(survivorA, survivorB, StringComparison.OrdinalIgnoreCase))
                return false;

            if (GetRelationship(survivorA, survivorB) != null)
                return false;

            // Cannot initiate if already in a committed partnership
            if (GetRomanticPartner(survivorA) != null || GetRomanticPartner(survivorB) != null)
                return false;

            float compatibility = CalculateCompatibility(ageA, ageB, beliefA, beliefB, sharedTrauma);

            // Minimum affinity requirement for attraction
            if (!force && affinity < 20.0f)
                return false;

            float attractionChance = (affinity / 100.0f) * (compatibility / 100.0f);
            if (!force && rng.NextFloat() > attractionChance)
                return false;

            var rel = new RomanticRelationship
            {
                SurvivorA = survivorA,
                SurvivorB = survivorB,
                Stage = RomanceStage.Attraction,
                RomanceScore = 15,
                Compatibility = compatibility,
                StartDay = currentDay,
                LastInteractionDay = currentDay
            };

            _relationships.Add(rel);
            OnRomanceStageAdvancedSeam?.Invoke(survivorA, survivorB, RomanceStage.Attraction);
            return true;
        }

        public bool ConductCourtshipEvent(
            string survivorA,
            string survivorB,
            string eventId,
            float currentAffinity,
            ISeededRng rng,
            int currentDay,
            bool forceSuccess = false)
        {
            var rel = GetRelationship(survivorA, survivorB);
            if (rel == null) return false;

            var evt = _catalog.CourtshipEvents.FirstOrDefault(e => string.Equals(e.EventId, eventId, StringComparison.OrdinalIgnoreCase));
            if (evt == null)
            {
                // Fallback default courtship event
                evt = new CourtshipEventDef
                {
                    EventId = eventId,
                    Name = "Courtship Talk",
                    MinAffinity = 20,
                    BaseSuccessRate = 0.75f,
                    ScoreGain = 8,
                    StressReduction = 5.0f
                };
            }

            if (currentAffinity < evt.MinAffinity && !forceSuccess)
                return false;

            float successChance = evt.BaseSuccessRate * (rel.Compatibility / 100.0f);
            bool success = forceSuccess || rng.NextFloat() <= successChance;

            rel.LastInteractionDay = currentDay;

            if (success)
            {
                rel.RomanceScore = Math.Min(100, rel.RomanceScore + evt.ScoreGain);
                CheckStageProgression(rel);
                return true;
            }
            else
            {
                // Slight dip on awkward encounter
                rel.RomanceScore = Math.Max(0, rel.RomanceScore - 2);
                return false;
            }
        }

        private void CheckStageProgression(RomanticRelationship rel)
        {
            RomanceStage oldStage = rel.Stage;
            RomanceStage newStage = oldStage;

            if (rel.RomanceScore >= 75)
            {
                newStage = RomanceStage.Bonded;
            }
            else if (rel.RomanceScore >= 50)
            {
                newStage = RomanceStage.Partnership;
            }
            else if (rel.RomanceScore >= 25)
            {
                newStage = RomanceStage.Courtship;
            }
            else
            {
                newStage = RomanceStage.Attraction;
            }

            if (newStage > oldStage)
            {
                rel.Stage = newStage;
                OnRomanceStageAdvancedSeam?.Invoke(rel.SurvivorA, rel.SurvivorB, newStage);
            }
        }

        public void AdvanceDay(int currentDay)
        {
            foreach (var rel in _relationships)
            {
                if (rel.Stage == RomanceStage.Bonded)
                {
                    rel.BondedDaysCount++;
                    if (rel.BondedDaysCount >= 30 && !rel.IsSoulmate)
                    {
                        rel.IsSoulmate = true;
                    }
                }
            }
        }

        public bool DissolvePartnership(string survivorA, string survivorB, string reason = "mutual_drift")
        {
            var rel = GetRelationship(survivorA, survivorB);
            if (rel == null) return false;

            _relationships.Remove(rel);
            OnPartnershipDissolvedSeam?.Invoke(survivorA, survivorB, reason);
            return true;
        }

        public FamilyUnit FormFamilyUnit(string survivorA, string survivorB, string familyName)
        {
            var rel = GetRelationship(survivorA, survivorB);
            string fId = $"family_{survivorA}_{survivorB}_{_familyUnits.Count + 1}";

            var unit = new FamilyUnit
            {
                FamilyId = fId,
                FamilyName = familyName,
                ParentIds = new List<string> { survivorA, survivorB },
                FamilyBond = rel != null ? (float)rel.RomanceScore : 75.0f
            };

            _familyUnits.Add(unit);
            OnFamilyUnitEstablishedSeam?.Invoke(fId, unit.ParentIds);
            return unit;
        }

        public bool AddChildToFamily(string familyId, string childId, bool isAdopted)
        {
            var family = _familyUnits.FirstOrDefault(f => string.Equals(f.FamilyId, familyId, StringComparison.OrdinalIgnoreCase));
            if (family == null) return false;

            if (!family.ChildIds.Exists(c => string.Equals(c, childId, StringComparison.OrdinalIgnoreCase)))
            {
                family.ChildIds.Add(childId);
                family.FamilyBond = Math.Min(100.0f, family.FamilyBond + 5.0f);
                OnChildWelcomedToFamilySeam?.Invoke(familyId, childId, isAdopted);
                return true;
            }

            return false;
        }

        public string CaptureState()
        {
            var relEntries = new List<string>();
            foreach (var r in _relationships)
            {
                string cohab = r.CohabitationQuarters != null ? $"\"{r.CohabitationQuarters}\"" : "null";
                relEntries.Add($"{{\"a\":\"{r.SurvivorA}\",\"b\":\"{r.SurvivorB}\",\"stage\":{(int)r.Stage},\"score\":{r.RomanceScore},\"compat\":{r.Compatibility.ToString("F1", CultureInfo.InvariantCulture)},\"start\":{r.StartDay},\"last\":{r.LastInteractionDay},\"bonded_days\":{r.BondedDaysCount},\"soulmate\":{(r.IsSoulmate ? "true" : "false")},\"cohab\":{cohab}}}");
            }

            var famEntries = new List<string>();
            foreach (var f in _familyUnits)
            {
                string parents = string.Join(",", f.ParentIds.Select(p => $"\"{p}\""));
                string children = string.Join(",", f.ChildIds.Select(c => $"\"{c}\""));
                string ext = string.Join(",", f.ExtendedFamilyIds.Select(e => $"\"{e}\""));
                famEntries.Add($"{{\"id\":\"{f.FamilyId}\",\"name\":\"{f.FamilyName}\",\"parents\":[{parents}],\"children\":[{children}],\"ext\":[{ext}],\"bond\":{f.FamilyBond.ToString("F1", CultureInfo.InvariantCulture)}}}");
            }

            return $"{{\"schema_version\":1,\"relationships\":[{string.Join(",", relEntries)}],\"families\":[{string.Join(",", famEntries)}]}}";
        }

        public void RestoreState(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            // Schema-gated: a payload written by an unknown schema is rejected
            // outright rather than silently half-applied. A payload that omits
            // the field is the original v1 shape and still loads, which is what
            // keeps pre-integration saves working.
            int version = ExtractInt(json, "schema_version", StateSchemaVersion);
            if (version != StateSchemaVersion)
            {
                throw new InvalidOperationException(
                    $"RomanceFamilySystem.RestoreState: unsupported schema_version {version} (expected {StateSchemaVersion}).");
            }

            _relationships.Clear();
            _familyUnits.Clear();

            // Restore relationships
            int relStart = json.IndexOf("\"relationships\"", StringComparison.Ordinal);
            if (relStart >= 0)
            {
                int arrStart = json.IndexOf('[', relStart);
                int arrEnd = json.IndexOf(']', arrStart);
                if (arrStart >= 0 && arrEnd > arrStart)
                {
                    string arrText = json.Substring(arrStart + 1, arrEnd - arrStart - 1);
                    int idx = 0;
                    while (idx < arrText.Length)
                    {
                        int objStart = arrText.IndexOf('{', idx);
                        if (objStart < 0) break;
                        int objEnd = arrText.IndexOf('}', objStart);
                        if (objEnd < 0) break;

                        string obj = arrText.Substring(objStart, objEnd - objStart + 1);
                        var rel = new RomanticRelationship
                        {
                            SurvivorA = ExtractString(obj, "a"),
                            SurvivorB = ExtractString(obj, "b"),
                            Stage = (RomanceStage)ExtractInt(obj, "stage", 0),
                            RomanceScore = ExtractInt(obj, "score", 0),
                            Compatibility = ExtractFloat(obj, "compat", 50.0f),
                            StartDay = ExtractInt(obj, "start", 1),
                            LastInteractionDay = ExtractInt(obj, "last", 1),
                            BondedDaysCount = ExtractInt(obj, "bonded_days", 0),
                            IsSoulmate = obj.Contains("\"soulmate\":true")
                        };

                        // CaptureState writes cohab; before this the quarters
                        // assignment was silently dropped on every load.
                        string cohab = ExtractString(obj, "cohab");
                        rel.CohabitationQuarters = string.IsNullOrEmpty(cohab) ? null : cohab;

                        if (!string.IsNullOrEmpty(rel.SurvivorA) && !string.IsNullOrEmpty(rel.SurvivorB))
                        {
                            _relationships.Add(rel);
                        }

                        idx = objEnd + 1;
                    }
                }
            }

            // Restore families
            int famStart = json.IndexOf("\"families\"", StringComparison.Ordinal);
            if (famStart >= 0)
            {
                int arrStart = json.IndexOf('[', famStart);
                int arrEnd = json.LastIndexOf(']');
                if (arrStart >= 0 && arrEnd > arrStart)
                {
                    string arrText = json.Substring(arrStart + 1, arrEnd - arrStart - 1);
                    int idx = 0;
                    while (idx < arrText.Length)
                    {
                        int objStart = arrText.IndexOf('{', idx);
                        if (objStart < 0) break;
                        int depth = 0;
                        int objEnd = -1;
                        for (int i = objStart; i < arrText.Length; i++)
                        {
                            if (arrText[i] == '{') depth++;
                            else if (arrText[i] == '}')
                            {
                                depth--;
                                if (depth == 0) { objEnd = i; break; }
                            }
                        }
                        if (objEnd < 0) break;

                        string obj = arrText.Substring(objStart, objEnd - objStart + 1);
                        var fam = new FamilyUnit
                        {
                            FamilyId = ExtractString(obj, "id"),
                            FamilyName = ExtractString(obj, "name"),
                            FamilyBond = ExtractFloat(obj, "bond", 50.0f)
                        };

                        // Extract parents
                        fam.ParentIds = ExtractStringList(obj, "parents");
                        fam.ChildIds = ExtractStringList(obj, "children");
                        fam.ExtendedFamilyIds = ExtractStringList(obj, "ext");

                        if (!string.IsNullOrEmpty(fam.FamilyId))
                        {
                            _familyUnits.Add(fam);
                        }

                        idx = objEnd + 1;
                    }
                }
            }
        }

        private static List<string> ExtractStringList(string json, string key)
        {
            var list = new List<string>();
            string pattern = $"\"{key}\"";
            int keyIdx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (keyIdx < 0) return list;
            int startArr = json.IndexOf('[', keyIdx);
            if (startArr < 0) return list;
            int endArr = json.IndexOf(']', startArr);
            if (endArr <= startArr) return list;

            string content = json.Substring(startArr + 1, endArr - startArr - 1);
            string[] items = content.Split(',');
            foreach (var item in items)
            {
                string cleaned = item.Trim().Trim('"');
                if (!string.IsNullOrEmpty(cleaned))
                {
                    list.Add(cleaned);
                }
            }
            return list;
        }

        private static string ExtractString(string json, string key)
        {
            string pattern = $"\"{key}\"";
            int keyIdx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (keyIdx < 0) return string.Empty;
            int colon = json.IndexOf(':', keyIdx + pattern.Length);
            if (colon < 0) return string.Empty;
            int quoteStart = json.IndexOf('"', colon + 1);
            if (quoteStart < 0) return string.Empty;
            int quoteEnd = json.IndexOf('"', quoteStart + 1);
            if (quoteEnd < 0) return string.Empty;
            return json.Substring(quoteStart + 1, quoteEnd - quoteStart - 1).Trim();
        }

        private static int ExtractInt(string json, string key, int defaultValue)
        {
            string pattern = $"\"{key}\"";
            int keyIdx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (keyIdx < 0) return defaultValue;
            int colon = json.IndexOf(':', keyIdx + pattern.Length);
            if (colon < 0) return defaultValue;
            int start = colon + 1;
            while (start < json.Length && (char.IsWhiteSpace(json[start]) || json[start] == '"')) start++;
            int end = start;
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '-')) end++;
            if (end > start && int.TryParse(json.Substring(start, end - start), out int val))
            {
                return val;
            }
            return defaultValue;
        }

        private static float ExtractFloat(string json, string key, float defaultValue)
        {
            string pattern = $"\"{key}\"";
            int keyIdx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (keyIdx < 0) return defaultValue;
            int colon = json.IndexOf(':', keyIdx + pattern.Length);
            if (colon < 0) return defaultValue;
            int start = colon + 1;
            while (start < json.Length && (char.IsWhiteSpace(json[start]) || json[start] == '"')) start++;
            int end = start;
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '.' || json[end] == '-')) end++;
            if (end > start && float.TryParse(json.Substring(start, end - start), NumberStyles.Float, CultureInfo.InvariantCulture, out float val))
            {
                return val;
            }
            return defaultValue;
        }
    }

    /// <summary>
    /// Read-only census of live romance/family state (Plan 150). Exposed for
    /// the architecture scanner and the host self-test probe.
    /// </summary>
    public struct RomanceFamilyCensus
    {
        public int TotalRelationships { get; }
        public int BondedCount { get; }
        public int PartnershipCount { get; }
        public int TotalFamilies { get; }
        public int TotalChildren { get; }
        public int LoadedCourtshipEvents { get; }

        public RomanceFamilyCensus(
            int totalRelationships,
            int bondedCount,
            int partnershipCount,
            int totalFamilies,
            int totalChildren,
            int loadedCourtshipEvents)
        {
            TotalRelationships = totalRelationships;
            BondedCount = bondedCount;
            PartnershipCount = partnershipCount;
            TotalFamilies = totalFamilies;
            TotalChildren = totalChildren;
            LoadedCourtshipEvents = loadedCourtshipEvents;
        }
    }
}
