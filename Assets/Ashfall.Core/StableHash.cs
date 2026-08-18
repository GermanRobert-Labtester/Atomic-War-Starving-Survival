namespace Ashfall.Core
{
    /// <summary>
    /// Deterministic, runtime-stable string hashing for simulation keys and
    /// dedup ids. djb2/x33 — deliberately NOT string.GetHashCode(), which is
    /// randomized per process in modern .NET and would break the cross-host
    /// determinism invariant (same seed ⇒ same simulation in both engines).
    /// Engine-agnostic; safe for save-derived keys.
    /// </summary>
    public static class StableHash
    {
        public static int Of(string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;
            unchecked
            {
                int h = 5381;
                for (int i = 0; i < value.Length; i++)
                    h = ((h << 5) + h) ^ value[i];
                return h;
            }
        }
    }
}
