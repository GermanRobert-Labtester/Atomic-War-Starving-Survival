namespace Ashfall.Core.Institutions
{
    /// <summary>
    /// Shared survivor-availability authority for the flagship institutions
    /// (archive scholars, summit delegates, sky-defense crews, sanatorium
    /// therapists/patients). One survivor holds one institution claim at a
    /// time unless an implementation explicitly allows stacking.
    ///
    /// Implementations are runtime-derived: each institution persists its own
    /// claims inside its own save section and re-registers them on restore,
    /// so the ledger itself needs no save section (plan §10 restore order).
    /// Null implementations mean "permissive" for headless tests.
    /// </summary>
    public interface IInstitutionAvailability
    {
        /// <summary>True when the survivor holds no blocking claim.</summary>
        bool IsAvailable(string survivorId);

        /// <summary>
        /// Atomically claims the survivor for one institution role.
        /// Returns false when already claimed elsewhere. Idempotent for the
        /// exact same (survivor, institution, role) triple.
        /// </summary>
        bool TryClaim(string survivorId, string institutionId, string roleId);

        /// <summary>Releases a claim; unknown triples are ignored.</summary>
        void Release(string survivorId, string institutionId, string roleId);
    }

    /// <summary>Permissive no-op implementation for headless tests.</summary>
    public sealed class PermissiveInstitutionAvailability : IInstitutionAvailability
    {
        public static readonly PermissiveInstitutionAvailability Instance = new();
        public bool IsAvailable(string survivorId) => true;
        public bool TryClaim(string survivorId, string institutionId, string roleId) => true;
        public void Release(string survivorId, string institutionId, string roleId) { }
    }
}
