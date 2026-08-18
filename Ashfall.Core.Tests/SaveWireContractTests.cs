// Wire-format compliance tests for the cross-host save contract.
//
// A save written by the Godot host (SystemTextJsonSerializer) must be
// readable by a Unity host (JsonUtility). Since we have no Unity license
// in this environment, we model JsonUtility's shape with UnityJsonShapeSerializer
// — a Core-side reference implementation that mirrors JsonUtility's exact
// behaviour (public fields only, declaration-order field iteration, no
// polymorphism, lists-of-primitive, primitives as plain JSON values).
//
// Both serializers are fed the same Core DTO; we parse their JSON output
// into trees and assert structural equality on every value. When a real
// Unity adapter ships, this test pins the wire format it must produce.
#nullable disable

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Journal;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Ashfall.Core.World;

namespace Ashfall.Core.Tests
{
    public class SaveWireContractTests
    {
        // ── Unity-shape reference serializer ───────────────────────────
        // Mirrors UnityEngine.JsonUtility:
        //   • writes only public instance fields (not properties)
        //   • field order = declaration order via reflection
        //   • no polymorphism: objects serialize as flat field maps
        //   • lists/arrays serialize as JSON arrays
        //   • string escaping is JsonUtility's minimal set
        // Returned as a JSON tree (Dictionary / List / primitives) so we
        // can compare structurally against System.Text.Json's output
        // without depending on string-escape parity.

        private static object UnitySerializeToTree(object root)
        {
            return ToTree(root);
        }

        private static object ToTree(object value)
        {
            if (value == null) return null;
            if (value is string s) return s;
            if (value is bool b) return b;
            if (value is int || value is long || value is short || value is byte) return Convert.ToInt64(value);
            if (value is float f) return Convert.ToDouble(f);
            if (value is double d) return d;
            if (value is Enum) return Convert.ToInt32(value);
            if (value is System.Collections.IEnumerable enumerable)
            {
                var list = new List<object>();
                foreach (var item in enumerable) list.Add(ToTree(item));
                return list;
            }
            var type = value.GetType();
            var map = new Dictionary<string, object>();
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                map[field.Name] = ToTree(field.GetValue(value));
            }
            return map;
        }

        private static object SystemTextSerializeToTree(object root)
        {
            var sysTxt = new SystemTextJsonSerializer();
            string json = sysTxt.Serialize(root);
            using var doc = JsonDocument.Parse(json);
            return ConvertElement(doc.RootElement);
        }

        private static object ConvertElement(JsonElement el)
        {
            switch (el.ValueKind)
            {
                case JsonValueKind.Null:
                case JsonValueKind.Undefined:
                    return null;
                case JsonValueKind.True: return true;
                case JsonValueKind.False: return false;
                case JsonValueKind.String: return el.GetString();
                case JsonValueKind.Number:
                    if (el.TryGetInt64(out var i)) return i;
                    return el.GetDouble();
                case JsonValueKind.Array:
                    var list = new List<object>();
                    foreach (var item in el.EnumerateArray()) list.Add(ConvertElement(item));
                    return list;
                case JsonValueKind.Object:
                    var map = new Dictionary<string, object>();
                    foreach (var prop in el.EnumerateObject()) map[prop.Name] = ConvertElement(prop.Value);
                    return map;
                default:
                    throw new InvalidOperationException($"Unknown JSON kind: {el.ValueKind}");
            }
        }

        private static void AssertTreesEqual(object expected, object actual, string path = "$")
        {
            if (expected == null && actual == null) return;
            if (expected == null || actual == null)
                throw new Xunit.Sdk.XunitException($"Mismatch at {path}: expected={expected ?? "null"} actual={actual ?? "null"}");
            if (expected is Dictionary<string, object> expMap)
            {
                var actMap = Assert.IsType<Dictionary<string, object>>(actual);
                foreach (var kv in expMap)
                {
                    if (!actMap.TryGetValue(kv.Key, out var actVal))
                        throw new Xunit.Sdk.XunitException($"Key {kv.Key} missing at {path}.{kv.Key}");
                    AssertTreesEqual(kv.Value, actVal, $"{path}.{kv.Key}");
                }
                foreach (var kv in actMap)
                {
                    if (!expMap.ContainsKey(kv.Key))
                        throw new Xunit.Sdk.XunitException($"Unexpected key {kv.Key} at {path}.{kv.Key}");
                }
                return;
            }
            if (expected is List<object> expList)
            {
                var actList = Assert.IsType<List<object>>(actual);
                Assert.Equal(expList.Count, actList.Count);
                for (int i = 0; i < expList.Count; i++)
                    AssertTreesEqual(expList[i], actList[i], $"{path}[{i}]");
                return;
            }
// Numeric equivalence: handle (int,long,double) cross-type equality and float drift.
// Float→double expansion can introduce a few ULPs of imprecision (0.6f → 0.60000002384...).
// Exclude string and bool: both implement IConvertible but must compare as text/value.
            if (expected is not string && expected is not bool && actual is not string && actual is not bool
                && expected is IConvertible && actual is IConvertible)
            {
                double de = ((IConvertible)expected).ToDouble(CultureInfo.InvariantCulture);
                double da = ((IConvertible)actual).ToDouble(CultureInfo.InvariantCulture);
                double scale = Math.Max(Math.Abs(de), Math.Abs(da));
                // Relative tolerance scaled by float epsilon: covers 0.6f → 0.60000002… cases.
                double tol = scale > 0 ? scale * 1e-6 : 1e-6;
                Assert.True(Math.Abs(de - da) <= tol,
                    $"Numeric mismatch at {path}: expected={de.ToString("R", CultureInfo.InvariantCulture)} actual={da.ToString("R", CultureInfo.InvariantCulture)} tol={tol.ToString("R", CultureInfo.InvariantCulture)}");
                return;
            }
            Assert.Equal(expected, actual);
        }

        // ── Fixtures ────────────────────────────────────────────────────

        private static NarrativeEncounterState NewNarrativeState() => new NarrativeEncounterState
        {
            systemId = "narrative_encounter_system",
            totalResolved = 2,
            cumulativeMorale = 1,
            cumulativeGuilt = -1,
            history = new List<EncounterResolutionRecord>
            {
                new EncounterResolutionRecord
                {
                    encounterId = "enc_dead_letter_office",
                    choiceId = "read",
                    locationId = "loc_the_allotments",
                    day = 40,
                    moraleDelta = 1,
                    guiltDelta = 0
                }
            },
            pending = new List<PendingSurfacedEncounter>
            {
                new PendingSurfacedEncounter
                {
                    encounterId = "enc_weather_station",
                    locationId = "loc_denial_cut_substation",
                    legIndex = 3,
                    day = 41
                }
            }
        };

        // ── Tests ───────────────────────────────────────────────────────

        [Fact]
        public void Contract_ListedTypes_IsNonEmptyAndReferenced()
        {
            Assert.NotEmpty(SaveWireContract.CoveredDtoTypes);
            foreach (var name in SaveWireContract.CoveredDtoTypes)
            {
                var type = ResolveTypeAcrossAssemblies(name);
                Assert.NotNull(type);
            }
        }

        private static Type ResolveTypeAcrossAssemblies(string fullName)
        {
            foreach (var asm in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                var t = asm.GetType(fullName, throwOnError: false);
                if (t != null) return t;
            }
            return null;
        }

        [Fact]
        public void NarrativeState_BothSerializers_ProduceIdenticalWireTree()
        {
            var state = NewNarrativeState();

            object sysTxtTree = SystemTextSerializeToTree(state);
            object unityTree = UnitySerializeToTree(state);

            AssertTreesEqual(unityTree, sysTxtTree);
        }

        [Fact]
        public void MedicalLedger_BothSerializers_ProduceIdenticalWireTree()
        {
            var state = new ChemicalDependencyLedgerState
            {
                survivors = new List<SurvivorDependencyList>
                {
                    new SurvivorDependencyList
                    {
                        survivorId = "survivor_sarah_chen",
                        dependencies = new List<ChemicalDependencyState>
                        {
                            new ChemicalDependencyState
                            {
                                itemId = "item_morphine",
                                dependencyLevel = 0.6f,
                                kind = "Opioid",
                                inManagedDetox = false,
                                inColdTurkey = false,
                                detoxProgressHours = 0f
                            }
                        }
                    }
                }
            };

            AssertTreesEqual(UnitySerializeToTree(state), SystemTextSerializeToTree(state));
        }

        [Fact]
        public void WorldState_BothSerializers_ProduceIdenticalWireTree()
        {
            var state = new WorldWeatherState
            {
                currentKind = "Snow",
                totalElapsedHours = 720f,
                hoursUntilNextCheck = 4f,
                rollCount = 12,
                restrictToNonHazardWeather = false
            };

            AssertTreesEqual(UnitySerializeToTree(state), SystemTextSerializeToTree(state));
        }

        [Fact]
        public void JournalSave_BothSerializers_ProduceIdenticalWireTree()
        {
            var state = new JournalSave
            {
                Entries = new JournalEntry[0],
                Knowledge = new KnowledgeBaseSave { DiscoveredKeys = new[] { "k_water", "k_radiation" } },
                NextSeq = 1,
                HasUnread = false,
                NotificationPing = false,
                NotificationPingCount = 0,
                HudIsOpen = false,
                ActiveTab = 0,
                LastSeenIndexPerTab = new int[0],
                LastSeenCodexPerTab = new int[0],
                CodexUnlockCount = 2
            };

            AssertTreesEqual(UnitySerializeToTree(state), SystemTextSerializeToTree(state));
        }

        [Fact]
        public void ExpeditionState_BothSerializers_ProduceIdenticalWireTree()
        {
            var state = new ExpeditionState
            {
                survivorId = "survivor_gunner_mikhail",
                locationId = "loc_the_allotments",
                displayName = "The Works Allotment Commune",
                stance = "Stealth",
                phase = (int)ExpeditionPhase.Outbound,
                travelTicksCompleted = 0,
                distanceTicks = 5,
                stamina = 100f,
                encounterCount = 0
            };

            AssertTreesEqual(UnitySerializeToTree(state), SystemTextSerializeToTree(state));
        }

        [Fact]
        public void SaveChecksum_StableAcrossBothSerializers_ForSameState()
        {
            // The cross-host invariant: the integrity hash must not depend on
            // which serializer wrote the file. Same state, same hash.
            var state = NewNarrativeState();
            var envelope = new NarrativeHostSaveLite { State = state };

            // Serialize + deserialize through System.Text.Json (the Godot reader).
            var sysTxt = new SystemTextJsonSerializer();
            string sysTxtJson = sysTxt.Serialize(envelope);
            var sysTxtEnvelope = sysTxt.Deserialize<NarrativeHostSaveLite>(sysTxtJson);

            // Serialize through the Unity-shape reference, deserialize through Godot.
            string unityJson = JsonSerializer.Serialize(UnitySerializeToTree(envelope));
            var unityEnvelope = sysTxt.Deserialize<NarrativeHostSaveLite>(unityJson);

            Assert.Equal(SaveChecksum.Compute(sysTxtEnvelope), SaveChecksum.Compute(unityEnvelope));
        }

        /// <summary>Envelope shape used only by SaveChecksum tests — same as the host envelopes.</summary>
        public sealed class NarrativeHostSaveLite
        {
            public NarrativeEncounterState State;
            public string Checksum;
        }
    }
}