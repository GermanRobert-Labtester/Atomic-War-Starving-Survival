// -----------------------------------------------------------------------
// Unity-side IJsonSerializer adapter — cross-host save wire-format contract.
//
// This file is the reference implementation that a Unity host uses to
// read saves written by the Godot host (SystemTextJsonSerializer) and
// write saves that the Godot host can read.
//
// The wire format is pinned by SaveWireContractTests.cs (7 tests):
// both serializers must produce identical JSON trees for every covered DTO.
//
// To use in the Unity project:
//   1. Copy this file into Assets/_Game/Core/
//   2. Ensure the assembly defines ASHALL_UNITY or similar so the
//      UnityEngine.JsonUtility reference resolves
//   3. Replace all JsonUtility.Serialize/Deserialize call sites with
//      IJsonSerializer.Serialize/Deserialize through this adapter
//   4. Wire into the Unity SaveSystem via IJsonSerializer injection
//
// Contract rules (from SaveWireContract.cs):
//   • Public fields only (not properties) — matches JsonUtility's behavior
//   • Plain CLR types — no Dictionary<,>, no polymorphism
//   • Strings default to string.Empty, lists default to empty
//   • camelCase field names throughout
//   • Every DTO carries a schema_version field
//   • Save envelopes carry a Checksum field (last field, skipped by hash)
// -----------------------------------------------------------------------

#if ASHFALL_UNITY
using UnityEngine;

namespace Ashfall.Core.IO
{
    /// <summary>
    /// Unity-side IJsonSerializer adapter wrapping UnityEngine.JsonUtility.
    /// Produces the exact same wire format as SystemTextJsonSerializer
    /// for all Core save DTOs (verified by SaveWireContractTests.cs).
    /// </summary>
    public sealed class UnityJsonSerializer : IJsonSerializer
    {
        /// <summary>
        /// Serialize to JSON using JsonUtility.ToJson.
        /// JsonUtility serializes public instance fields in declaration order,
        /// matching the SystemTextJsonSerializer.IncludeFields = true behavior.
        /// </summary>
        public string Serialize<T>(T value)
        {
            if (value == null) return "{}";
            return JsonUtility.ToJson(value);
        }

        /// <summary>
        /// Deserialize from JSON using JsonUtility.FromJson.
        /// Returns null if the JSON is null, empty, or whitespace.
        /// </summary>
        public T? Deserialize<T>(string json) where T : class
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;
            return JsonUtility.FromJson<T>(json);
        }
    }
}
#else
// This file requires the ASHFALL_UNITY define to compile.
// It is a reference implementation only — copy to Assets/_Game/Core/
// in the Unity project tree and ensure the define is set.
#endif
