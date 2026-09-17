# ASHFALL — Quality Roadmap Batch 112

## Theme: Security Hardening — Save Tampering Prevention & Input Sanitization

**Priority:** MEDIUM (singleplayer game, but save integrity matters for achievements/leaderboards)
**Risk:** Low-Medium — additive security layer, no existing behavior removed
**Batch:** 112
**Depends on:** None (can run in parallel with other batches; benefits from Batch 104 achievement system if present)
**Branch pattern:** `quality/batch-112-security-hardening`

---

## Context

ASHFALL saves are JSON files with integrity checksums (`SaveChecksum.Compute` in `Assets/Ashfall.Core/SaveChecksum.cs`). Verified directly against the source: the current checksum is an **unkeyed SHA256** over a reflection-walked canonical form of the object graph (public instance fields, ordinal name order, normalized null/empty strings and collections, culture-invariant numeric formatting). It is not an HMAC and has no key today — `SaveChecksum.Compute(object root)` takes only the object to hash, nothing else. This prevents accidental corruption and cross-host serializer drift (its actual, documented purpose per the class's own doc comment) but offers zero resistance to intentional tampering, because anyone can recompute the same SHA256 over an edited save with the same public algorithm.

**Current security posture:**

| Aspect | Status | Risk |
|--------|--------|------|
| Save integrity | Unkeyed SHA256 of a reflection-walked canonical field dump (`Assets/Ashfall.Core/SaveChecksum.cs`) — confirmed, not "reflection-based checksum" as a vague description | Algorithm and hash are both fully public; anyone can recompute a valid checksum for a modified save, so today's checksum stops corruption, not tampering |
| Save encryption | None — plaintext JSON | Player can read/modify any value |
| Input bounds | None — catalog loaders trust all JSON values | Huge strings/arrays → OOM crash |
| JSON depth | Default System.Text.Json (max 64) | Deep nesting → stack overflow |
| Player text input | No sanitization | Control characters, injection vectors |
| Save file size | Unlimited | Memory bomb via inflated save |
| Key storage | N/A (no signing today) | — |

**Threat model for a singleplayer game:**
1. **Save editors/trainers** — players modify saves to unlock achievements, skip content, inflate resources.
2. **Corrupted data** — modded/hand-edited JSON with invalid values causes crashes or undefined behavior.
3. **Malicious mods** — community content with oversized strings, deep nesting, or pathological data structures.
4. **Leaderboard integrity** — if the game ever ships competitive features (permadeath runs, timed challenges), tampered saves invalidate rankings.

**Philosophy:** Defense in depth. No single measure stops a determined reverser, but layered protections raise the effort significantly and catch casual tampering.

**Honest framing, stated upfront rather than only at the end of this document:** everything in Step 2 below (HMAC signing with an obfuscated, split-XOR key baked into the shipped binary) is **deterrence, not real security**, for a singleplayer, client-only game. There is no server the player doesn't control and no secret the player's own machine doesn't already possess — the signing key, however obfuscated, ships inside the executable the player runs. A player with a decompiler (dnSpy, ILSpy, dotPeek — all free, all trivial to point at a .NET assembly) or a debugger can recover `KeyDerivation.DeriveSigningKey()`'s output directly from a running process, at which point they can forge signatures indistinguishable from genuine ones. Splitting the key across two XOR'd byte arrays does not change this; it only changes how many minutes the attacker spends before finding it, not whether they can find it. This is a legitimate, common tradeoff for single-player games (it's what most single-player achievement/anti-cheat schemes do), but this plan must not describe it as "security" without that caveat, and no Done-when criterion in Step 2 should imply the key is actually protected from a motivated player. See `## Review Notes (Corrected)` for the specific language changes made throughout this document to keep that distinction explicit at every point it matters, not just in a footnote.

---

## Step 1 — Evaluate Threat Model & Define Security Policy

### Goal
Document the specific threats, acceptable risk levels, and the security boundary for ASHFALL saves and data loading.

### Implementation

Create `docs/security-policy.md`:

```markdown
# ASHFALL Security Policy

## Scope
This policy covers save file integrity, data loading safety, and player input
sanitization. It does NOT cover network security (game is offline/singleplayer),
DRM, or anti-cheat for real-time gameplay.

## Threat Categories

### T1 — Casual Save Tampering
- **Actor:** Player with a text editor
- **Goal:** Modify resource counts, unlock items, skip time
- **Impact:** Breaks intended game balance, trivializes achievements
- **Mitigation:** HMAC signing (key obfuscated, not plaintext) — **deterrence only**
- **Residual risk:** A player with a free .NET decompiler (dnSpy/ILSpy/dotPeek) or a debugger attached to the running game can extract the derived key directly from process memory or the assembly. Obfuscating the key (splitting it, XOR'ing parts together, deriving via HKDF) raises the *time* cost of extraction, not whether extraction is possible — for a client-side-only secret, it never becomes actually secure, only slower to break. This mitigation should not be described as closing T1; it raises the bar against copy-paste save editors that don't bother computing a matching signature, while doing effectively nothing against anyone willing to spend an afternoon in a decompiler.

### T2 — Corrupted/Malformed Data
- **Actor:** Buggy mod, corrupted disk, hand-edited JSON
- **Goal:** Unintentional — data doesn't conform to schema
- **Impact:** Crash, undefined behavior, silent data loss
- **Mitigation:** Input bounds validation, size limits, depth limits

### T3 — Malicious Mod Content
- **Actor:** Hostile mod author
- **Goal:** Crash game, exploit memory, inject content
- **Impact:** DoS (memory exhaustion), UI injection
- **Mitigation:** String length caps, array size caps, content sanitization

### T4 — Achievement/Leaderboard Fraud
- **Actor:** Player wanting fake achievements
- **Goal:** Forge valid save with impossible stats
- **Impact:** Devalues achievements for legitimate players
- **Mitigation:** HMAC alone does not solve this for an offline game — see T1 residual risk. Real protection for T4 requires a server-side validation step the client cannot forge (e.g. a backend that recomputes state from a replay log, or an online leaderboard service that never trusts client-submitted final values). If ASHFALL never ships online features, T4 should be documented as an accepted, unmitigated risk rather than implied-solved by client-side signing.

## Security Principles
1. **Defense in depth** — multiple layers, no single point of failure
2. **Fail closed** — reject invalid data, don't attempt repair
3. **No security through obscurity alone** — obfuscation raises the bar but isn't the only defense
4. **Graceful degradation** — security failures produce clear error messages, not crashes
5. **Backward compatibility** — legacy saves (pre-HMAC) load with a warning flag, not rejection
6. **Honest labeling** — anything that is deterrence rather than real security (client-side key obfuscation, HMAC on a save the player's own machine can both write and verify) must be labeled as such in code comments, docs, and Done-when criteria. Do not let "HMAC-SHA256" read as cryptographically strong access control when the trust boundary (the player's own machine) makes that framing misleading.
```

**Define constants** in `Assets/Ashfall.Core/Security/SecurityConstants.cs`:

```csharp
namespace Ashfall.Core.Security;

public static class SecurityConstants
{
    /// <summary>Maximum allowed save file size in bytes (16 MB).</summary>
    public const long MaxSaveFileSizeBytes = 16 * 1024 * 1024;

    /// <summary>Maximum JSON nesting depth for deserialization.</summary>
    public const int MaxJsonDepth = 32;

    /// <summary>Maximum length for any single string field in data files.</summary>
    public const int MaxStringFieldLength = 8192;

    /// <summary>Maximum array/list element count in data files.</summary>
    public const int MaxArrayElementCount = 10_000;

    /// <summary>Maximum length for player-entered text (survivor names, notes).</summary>
    public const int MaxPlayerTextLength = 256;

    /// <summary>HMAC algorithm identifier.</summary>
    public const string HmacAlgorithm = "HMACSHA256";

    /// <summary>Current save security version.</summary>
    public const int SaveSecurityVersion = 1;
}
```

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
# SecurityConstants compiles without engine references
grep -r "UnityEngine\|Godot\." Assets/Ashfall.Core/Security/
# Expected: 0 results
```

### Risk & Rollback
- Risk: None — this step is documentation plus a static-constants file with no behavior.
- Rollback: delete `docs/security-policy.md` and `Assets/Ashfall.Core/Security/SecurityConstants.cs`.

### Done when
- [ ] `docs/security-policy.md` created with threat model, including the T1/T4 residual-risk language above stating plainly that client-side HMAC key obfuscation is deterrence, not access control
- [ ] `Assets/Ashfall.Core/Security/SecurityConstants.cs` created
- [ ] Constants compile cleanly in Core (no engine references) — confirmed via `dotnet build`, not assumed
- [ ] Security policy reviewed and accepted, specifically including sign-off on the "this is deterrence, not real security" framing so later steps aren't built on a false premise

---

## Step 2 — Add HMAC-Based Save Signing (Deterrence Layer — Not Real Access Control)

### Goal
Replace the forge-able, unkeyed SHA256 checksum with an HMAC (keyed hash) that raises the effort required to modify a save undetected. **This does not prevent tampering by a motivated player** — see the Context section's honest framing above. It stops copy-paste save editing and naive text-editor tampering; it does not stop anyone willing to decompile the game or attach a debugger. State this plainly in the code's own doc comments, not just in this plan. Legacy saves without HMAC still load (backward compatibility) but are flagged as unsigned.

### Implementation

**Create `Assets/Ashfall.Core/Security/SaveSigner.cs`:**

```csharp
using System;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core.Security;

/// <summary>
/// Signs and verifies save data using HMAC-SHA256.
/// The signing key is derived from an obfuscated seed baked into the shipped binary.
///
/// SECURITY NOTE: this is a deterrence mechanism, not access control. The key this class uses
/// ships inside the game's own executable/assembly, which the player's machine both runs and
/// fully controls. A player with a free .NET decompiler (dnSpy, ILSpy, dotPeek) or a debugger
/// attached to the running process can recover the derived key and forge signatures
/// indistinguishable from genuine ones. This class raises the effort required for casual save
/// tampering (blocks naive text-editor edits and copy-paste save-editor tools that don't bother
/// computing a matching signature); it does not and cannot prevent tampering by anyone willing
/// to reverse-engineer the binary. Do not represent HMAC verification results to players or in
/// telemetry as proof a save was not tampered with by a technically capable user.
/// </summary>
public sealed class SaveSigner
{
    private readonly byte[] _key;

    public SaveSigner(byte[] key)
    {
        if (key == null || key.Length < 32)
            throw new ArgumentException("Signing key must be at least 32 bytes.");
        _key = key;
    }

    /// <summary>
    /// Compute HMAC-SHA256 signature for the given save payload.
    /// </summary>
    public string Sign(string jsonPayload)
    {
        if (string.IsNullOrEmpty(jsonPayload))
            throw new ArgumentException("Payload must not be null or empty.");

        using var hmac = new HMACSHA256(_key);
        byte[] payloadBytes = Encoding.UTF8.GetBytes(jsonPayload);
        byte[] hash = hmac.ComputeHash(payloadBytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Verify that the signature matches the payload.
    /// Uses constant-time comparison to prevent timing attacks.
    /// </summary>
    public bool Verify(string jsonPayload, string signature)
    {
        if (string.IsNullOrEmpty(jsonPayload) || string.IsNullOrEmpty(signature))
            return false;

        string expected = Sign(jsonPayload);
        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(expected),
            Encoding.UTF8.GetBytes(signature));
    }
}
```

**Create `Assets/Ashfall.Core/Security/KeyDerivation.cs`:**

```csharp
using System;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core.Security;

/// <summary>
/// Derives the signing key from an obfuscated seed.
/// The seed is split across multiple constants and XOR'd at runtime
/// to avoid a single plaintext key string that shows up in a naive binary string-search.
///
/// SECURITY NOTE: splitting/XOR'ing the seed defeats a plaintext string search of the compiled
/// assembly (e.g. `strings Ashfall.dll | grep -i key`), which is the specific, narrow threat this
/// technique addresses. It does NOT defeat decompilation (the XOR and HKDF calls are themselves
/// visible in decompiled IL/C#, and the resulting derived key can be dumped from a running
/// process with a debugger regardless of how obfuscated the source constants are). Treat this as
/// closing one specific, cheap attack (grep-the-binary) rather than as key secrecy in any general
/// sense.
/// </summary>
public static class KeyDerivation
{
    // These are XOR'd together at runtime to produce the actual seed.
    // Splitting prevents trivial string search in the binary.
    private static readonly byte[] Part1 = { /* 32 random bytes */ };
    private static readonly byte[] Part2 = { /* 32 random bytes */ };
    // Additional entropy from build-time or install-time salt (optional).

    /// <summary>
    /// Derive a 32-byte signing key using HKDF (HMAC-based Key Derivation).
    /// </summary>
    public static byte[] DeriveSigningKey(string context = "ashfall-save-v1")
    {
        byte[] seed = new byte[32];
        for (int i = 0; i < 32; i++)
            seed[i] = (byte)(Part1[i] ^ Part2[i]);

        // HKDF-SHA256: extract + expand
        byte[] prk = HKDF.Extract(HashAlgorithmName.SHA256, seed,
            Encoding.UTF8.GetBytes(context));
        byte[] key = HKDF.Expand(HashAlgorithmName.SHA256, prk, 32,
            Encoding.UTF8.GetBytes("save-signing"));

        // Clear intermediate material
        Array.Clear(seed, 0, seed.Length);
        Array.Clear(prk, 0, prk.Length);

        return key;
    }
}
```

**Modify save envelope format:**

The existing save envelope includes a `Checksum` field. **Correction:** the original draft said "used by all 22 save stores" — a direct search of the codebase found **30** distinct `*SaveStore` classes today (`CaravanSaveStore`, `CombatSaveStore`, `CraftingSaveStore`, `DailyBriefingSaveStore`, `DoseLedgerSaveStore`, `DutyRosterSaveStore`, `EconomySaveStore`, `ExpansionHubSaveStore`, `ExpeditionSaveStore`, `GreenhouseSaveStore`, `HoldfastSaveStore`, `HoldfastTradeSaveStore`, `InventorySaveStore`, `JournalSaveStore`, `MaritimeSaveStore`, `MedicalSaveStore`, `MedicalWardSaveStore`, `MemorialSaveStore`, `MusterSaveStore`, `NarrativeSaveStore`, `PhantomMemorySaveStore`, `Phase0SaveStore`, `PowerGridSaveStore`, `RadioSaveStore`, `ShelterAssignmentSaveStore`, `StartingLevelSaveStore`, `SurvivorsSaveStore`, `VerdictSaveStore`, `WorldSaveStore`, `YearOfAshSaveStore`), not all of which necessarily share one envelope base class. Before writing the envelope change, confirm whether all 30 actually deserialize through one shared `SaveEnvelope<T>` type or whether some (e.g. `GreenhouseSaveStore`, which was seen using its own inline `JsonSerializerOptions` rather than a shared codec) have their own ad hoc envelope shape — the number and scope of files this step touches depends on that answer and was not verified further in this review. Add the new fields alongside the existing one:

```csharp
public sealed class SaveEnvelope<T>
{
    public int Version { get; set; }
    public string Checksum { get; set; }       // Existing — structural integrity
    public string Hmac { get; set; }           // NEW — tamper detection
    public int SecurityVersion { get; set; }   // NEW — tracks signing scheme
    public T State { get; set; }
}
```

**Backward compatibility rules:**
- If `Hmac` is null/empty and `SecurityVersion` is 0 or missing → legacy save, load with warning flag `SaveLoadResult.UnsignedLegacy`.
- If `Hmac` is present but invalid → reject with `SaveLoadResult.TamperDetected`.
- If `Hmac` is present and valid → load normally.
- New saves always include HMAC.

**Files created:**
- `Assets/Ashfall.Core/Security/SaveSigner.cs`
- `Assets/Ashfall.Core/Security/KeyDerivation.cs`

**Files modified:**
- Save envelope class (location depends on current implementation — likely in each save store or a shared base)

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
# No engine references in Security/
grep -r "UnityEngine\|Godot\.\|GodotSharp" Assets/Ashfall.Core/Security/
# Expected: 0
```

### Risk & Rollback
- Risk: Low-Medium. Extending a shared save envelope is a cross-cutting change touching every save store — a mistake in the envelope DTO (e.g. a non-nullable `Hmac` field breaking legacy-save deserialization) could break loading for every existing save file, not just new ones. Mitigate by making `Hmac` and `SecurityVersion` nullable/optional with explicit defaults, and by testing the legacy (no-HMAC) load path before the forged-HMAC-rejection path, since the former is the higher-blast-radius regression.
- Risk: the key-derivation and signing code is genuinely new attack surface (a new `Exception`-throwing path, a new `HKDF` dependency) even though its security value is limited — bugs in `Verify`'s constant-time comparison or in `KeyDerivation`'s XOR/HKDF logic are ordinary C# bugs, not cryptographic weaknesses, and should be tested as such.
- Rollback: this step is additive to the envelope (new optional fields) and adds new files (`SaveSigner.cs`, `KeyDerivation.cs`) without modifying `SaveChecksum.cs`. Rollback = stop calling `SaveSigner`/`KeyDerivation` from save stores and delete the two new files; existing saves remain loadable via the unchanged `SaveChecksum` path throughout, since nothing about the existing checksum mechanism is removed or altered by this step.

### Done when
- [ ] `SaveSigner` class created with `Sign` and `Verify` methods
- [ ] Constant-time comparison used (no timing side-channel) for the `Verify` method specifically — confirmed via code review that `CryptographicOperations.FixedTimeEquals` is used, not `==` or `string.Equals` on the signature strings
- [ ] `KeyDerivation` uses split-XOR + HKDF — explicitly documented in its own doc comment (not just this plan) that this defeats plaintext binary string search only, not decompilation or runtime key extraction
- [ ] Save envelope extended with `Hmac` and `SecurityVersion` fields, confirmed nullable/optional so existing legacy saves still deserialize without a null-reference or missing-field error
- [ ] Legacy saves (no HMAC) load with warning, not rejection — tested against at least one real pre-existing save file from this repo's test fixtures, not just a hand-written test payload
- [ ] Forged HMAC rejected (tested in Step 7)
- [ ] No engine references in `Assets/Ashfall.Core/Security/`
- [ ] Confirmed which of the (at least 30, see correction above) save stores actually route through the shared envelope type being modified here, and scoped the "Files modified" list to that confirmed set rather than assuming uniform envelope usage across all stores

---

## Step 3 — Add Input Bounds Validation to Catalog Loaders

### Goal
Prevent memory exhaustion and undefined behavior from oversized or malformed data in JSON catalog files. Every string, array, and numeric value loaded from `Assets/StreamingAssets/Data/` is bounds-checked.

### Implementation

**Create `Assets/Ashfall.Core/Security/InputValidator.cs`:**

```csharp
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Security;

/// <summary>
/// Validates input values from external data sources (JSON catalogs, saves, mods).
/// All methods throw <see cref="DataValidationException"/> on violation.
/// </summary>
public static class InputValidator
{
    /// <summary>
    /// Validate string length. Truncates or throws based on policy.
    /// </summary>
    public static string ValidateString(string value, string fieldName,
        int maxLength = SecurityConstants.MaxStringFieldLength,
        ValidationPolicy policy = ValidationPolicy.Throw)
    {
        if (value == null) return null;
        if (value.Length <= maxLength) return value;

        return policy switch
        {
            ValidationPolicy.Truncate => value[..maxLength],
            ValidationPolicy.Throw => throw new DataValidationException(
                $"Field '{fieldName}' exceeds max length {maxLength} (actual: {value.Length})"),
            _ => throw new ArgumentOutOfRangeException(nameof(policy))
        };
    }

    /// <summary>
    /// Validate array/list element count.
    /// </summary>
    public static IReadOnlyList<T> ValidateArray<T>(IReadOnlyList<T> list, string fieldName,
        int maxCount = SecurityConstants.MaxArrayElementCount)
    {
        if (list == null) return null;
        if (list.Count <= maxCount) return list;

        throw new DataValidationException(
            $"Array '{fieldName}' exceeds max element count {maxCount} (actual: {list.Count})");
    }

    /// <summary>
    /// Validate numeric value within expected range.
    /// </summary>
    public static int ValidateRange(int value, string fieldName, int min, int max)
    {
        if (value >= min && value <= max) return value;
        throw new DataValidationException(
            $"Field '{fieldName}' value {value} outside range [{min}, {max}]");
    }

    /// <summary>
    /// Validate float value within expected range.
    /// </summary>
    public static float ValidateRange(float value, string fieldName, float min, float max)
    {
        if (value >= min && value <= max) return value;
        if (float.IsNaN(value) || float.IsInfinity(value))
            throw new DataValidationException(
                $"Field '{fieldName}' is NaN or Infinity");
        throw new DataValidationException(
            $"Field '{fieldName}' value {value} outside range [{min}, {max}]");
    }

    /// <summary>
    /// Validate that an ID string matches expected format (snake_case, known prefix).
    /// </summary>
    public static string ValidateId(string id, string fieldName)
    {
        if (string.IsNullOrEmpty(id)) return id;

        // Max ID length: 128 characters
        if (id.Length > 128)
            throw new DataValidationException(
                $"ID '{fieldName}' exceeds max length 128 (actual: {id.Length})");

        // Only lowercase, digits, underscores
        foreach (char c in id)
        {
            if (c != '_' && !char.IsLetterOrDigit(c))
                throw new DataValidationException(
                    $"ID '{fieldName}' contains invalid character '{c}'");
        }

        return id;
    }
}

public enum ValidationPolicy
{
    Throw,
    Truncate
}

public class DataValidationException : Exception
{
    public DataValidationException(string message) : base(message) { }
}
```

**Integration with catalog loaders:**

Each catalog loader in Core should call `InputValidator` after deserialization. Example pattern for `ItemCatalog`:

```csharp
public ItemDefinition ValidateItem(ItemDefinition raw)
{
    InputValidator.ValidateId(raw.Id, "item.id");
    InputValidator.ValidateString(raw.DisplayName, "item.displayName", maxLength: 128);
    InputValidator.ValidateString(raw.Description, "item.description", maxLength: 1024);
    InputValidator.ValidateRange(raw.Weight, "item.weight", min: 0f, max: 10000f);
    InputValidator.ValidateRange(raw.StackMax, "item.stackMax", min: 1, max: 9999);
    InputValidator.ValidateArray(raw.Tags, "item.tags", maxCount: 32);
    return raw;
}
```

**Apply to all data-loading paths:**
- `CatalogIntegrityValidator.cs` already walks all definitions — add bounds checks to its tier-1 validation pass.
- Each domain's catalog loader: items, locations, factions, recipes, events, NPCs, afflictions, etc.
- Narrative JSON loader (196 files): validate dialogue string lengths, choice arrays.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless --path . -- --data-integrity-selftest
# Must still pass (existing data within bounds)
```

### Risk & Rollback
- Risk: Low-Medium. Adding bounds checks to catalog loaders that currently accept anything means previously-tolerated data (even if technically out of an intended range) could start failing to load. Since `CatalogIntegrityValidator` and the ~280 JSON files it validates already have known gaps (only 35 of ~280 files have `schema_version`, mixed camelCase/snake_case), there's a real chance some legitimate existing data sits outside a newly chosen bound (e.g. a narrative string longer than 8192 chars, an item tag array longer than 32 elements) purely because no limit existed before. Run `--data-integrity-selftest` after wiring each loader, not just once at the end, so a violation is attributed to the specific loader/bound that introduced it rather than discovered in a single end-of-step sweep.
- Rollback: each catalog loader's `InputValidator` calls are additive and isolated per loader; remove the added validation calls from a specific loader to roll back just that one without affecting the others. `InputValidator.cs`/`DataValidationException` themselves are net-new, unused files if nothing calls them.

### Done when
- [ ] `InputValidator` class created with string, array, range, and ID validation
- [ ] `DataValidationException` defined for validation failures
- [ ] At least 5 catalog loaders wired to use `InputValidator` — list which 5 explicitly when this step is executed, since "at least 5" without naming them invites picking the 5 easiest rather than the 5 most exposed to untrusted/modded input
- [ ] Existing valid data passes all bounds checks (data-integrity-selftest still green) — run after each loader is wired, not only at the end
- [ ] Oversized test input correctly rejected (tested in Step 7)
- [ ] NaN/Infinity float values rejected

---

## Step 4 — Add Save File Size Limits

### Goal
Prevent memory exhaustion from inflated or malicious save files by enforcing a maximum file size before deserialization begins.

### Implementation

**Create `Assets/Ashfall.Core/Security/SaveFileGuard.cs`:**

```csharp
using System;
using System.IO;

namespace Ashfall.Core.Security;

/// <summary>
/// Guards save file loading with size and structural checks
/// performed BEFORE full deserialization.
/// </summary>
public static class SaveFileGuard
{
    /// <summary>
    /// Validate a save file path before loading.
    /// Checks: file exists, size within limits, basic JSON structure.
    /// </summary>
    /// <returns>The file content if valid.</returns>
    /// <exception cref="SaveSecurityException">If any check fails.</exception>
    public static string ValidateAndRead(string filePath, IFileIO fileIO)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new SaveSecurityException("Save file path is null or empty.");

        if (!fileIO.FileExists(filePath))
            throw new SaveSecurityException($"Save file not found: {filePath}");

        // CORRECTED: the real IFileIO port (Assets/Ashfall.Core/Ports.cs, confirmed by reading it)
        // has exactly 5 methods — DirectoryExists, FileExists, ReadAllText, WriteAllText, Combine.
        // There is no GetFileSize, and no stream-based API to check size before reading content
        // into memory. The original draft invented a 6th method that doesn't exist.
        //
        // Given the real port surface, a true "check size before reading" guard is not possible
        // without either (a) adding a new port method (an interface change every host adapter
        // must implement — see the corrected port extension below), or (b) accepting that
        // ReadAllText has already loaded the full file into memory by the time size is checked,
        // which weakens but does not eliminate the size guard's value (it still stops a bloated
        // save from being handed to the JSON deserializer and expanded further, and it still
        // rejects the save before the more expensive deserialize step runs).
        //
        // This implementation takes approach (b) as the lower-risk default, since it requires no
        // interface change and no adapter updates across hosts:
        string content = fileIO.ReadAllText(filePath);

        if (string.IsNullOrEmpty(content))
            throw new SaveSecurityException("Save file is empty (0 bytes).");

        // Measuring content.Length (UTF-16 char count) against a byte-size constant is an
        // approximation, not exact — a save file's on-disk byte size and its in-memory .NET
        // string Length are not the same unit. For ASCII-heavy JSON (typical for these saves)
        // this under-counts multi-byte UTF-8 sequences, so treat MaxSaveFileSizeBytes as an
        // upper bound approximation when applied this way, not an exact byte-for-byte cap. If an
        // exact byte count matters, use fileIO.ReadAllText's caller to check the file via
        // System.IO.FileInfo (Godot host) before calling into Core — but that reintroduces an
        // engine-specific code path, which conflicts with Invariant 1 (zero engine coupling in
        // Core). Prefer the approximation here and document the tradeoff rather than violating
        // the ports-and-adapters boundary for a size check.
        if (content.Length > SecurityConstants.MaxSaveFileSizeBytes)
            throw new SaveSecurityException(
                $"Save file exceeds maximum size ({content.Length} chars > " +
                $"{SecurityConstants.MaxSaveFileSizeBytes} chars). " +
                "File may be corrupted or tampered with.");

        // Quick structural check: must start with { and end with }
        string trimmed = content.AsSpan().Trim().ToString();
        if (!trimmed.StartsWith("{") || !trimmed.EndsWith("}"))
            throw new SaveSecurityException(
                "Save file does not contain valid JSON object structure.");

        return content;
    }

    /// <summary>
    /// Validate raw save content (for in-memory operations, testing).
    /// </summary>
    public static void ValidateContent(string content)
    {
        if (string.IsNullOrEmpty(content))
            throw new SaveSecurityException("Save content is null or empty.");

        if (content.Length > SecurityConstants.MaxSaveFileSizeBytes)
            throw new SaveSecurityException(
                $"Save content exceeds maximum size ({content.Length} chars).");
    }
}

public class SaveSecurityException : Exception
{
    public SaveSecurityException(string message) : base(message) { }
}
```

**Optional port extension (only if the approximation above is judged insufficient):**

If an exact pre-read byte-size check is actually required (e.g. because a malicious save could be large enough to cause memory pressure just from `ReadAllText` itself, before the length check runs), extend the real `IFileIO` port — which has exactly 5 methods today: `DirectoryExists(string)`, `FileExists(string)`, `ReadAllText(string)`, `WriteAllText(string, string)`, `Combine(params string[])` (confirmed by reading `Assets/Ashfall.Core/Ports.cs`) — with a 6th method:

```csharp
// In Assets/Ashfall.Core/Ports.cs — this is a breaking interface change:
// every existing IFileIO implementation (at minimum the Godot host adapter) must add this method
// or the solution will fail to compile. Search for ": IFileIO" before making this change to find
// every implementation that needs updating.
public interface IFileIO
{
    bool DirectoryExists(string path);
    bool FileExists(string path);
    string ReadAllText(string path);
    void WriteAllText(string path, string contents);
    string Combine(params string[] parts);
    long GetFileSize(string path);  // NEW — breaking change, requires updating every adapter
}
```

Treat this as the higher-risk, higher-precision option and the length-based approximation above as the default; only take this path if Step 7's testing shows the approximation is inadequate in practice.

**Wire into save stores:**

Each save store's `TryLoad` method should call `SaveFileGuard.ValidateAndRead` before deserialization:

```csharp
public bool TryLoad(string path, out TState state)
{
    try
    {
        string content = SaveFileGuard.ValidateAndRead(path, _fileIO);
        // ... existing deserialization logic
    }
    catch (SaveSecurityException ex)
    {
        _log.Warn($"Save rejected: {ex.Message}");
        state = default;
        return false;
    }
}
```

**Save stores to update:** the original draft's list of "22" was wrong — a direct search found **30** `*SaveStore` classes in the codebase today: `CaravanSaveStore`, `CombatSaveStore`, `CraftingSaveStore`, `DailyBriefingSaveStore`, `DoseLedgerSaveStore`, `DutyRosterSaveStore`, `EconomySaveStore`, `ExpansionHubSaveStore`, `ExpeditionSaveStore`, `GreenhouseSaveStore`, `HoldfastSaveStore`, `HoldfastTradeSaveStore`, `InventorySaveStore`, `JournalSaveStore`, `MaritimeSaveStore`, `MedicalSaveStore`, `MedicalWardSaveStore`, `MemorialSaveStore`, `MusterSaveStore`, `NarrativeSaveStore`, `PhantomMemorySaveStore`, `Phase0SaveStore`, `PowerGridSaveStore`, `RadioSaveStore`, `ShelterAssignmentSaveStore`, `StartingLevelSaveStore`, `SurvivorsSaveStore`, `VerdictSaveStore`, `WorldSaveStore`, `YearOfAshSaveStore`. Several names in the original draft's list do not exist as written (`WeatherSaveStore`, `RadiationSaveStore`, `NeedsSaveStore`, `RecipeSaveStore`, `QuestSaveStore`, `FactionSaveStore`) — those systems may persist state through a different store name or through another store's combined state; verify each before assuming it needs a separate wiring change. Confirm the real count and names again at implementation time, since this list can drift as new expansions land (per this project's own `AGENTS.md`, save-store counts have changed before, e.g. H2/H10 gaps).

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj

# Verify existing saves still load (they should be well under 16 MB)
godot --headless --path . -- --bridge-selftest
```

### Risk & Rollback
- Risk: Medium if the port-extension path (`IFileIO.GetFileSize`) is taken — it is a breaking interface change requiring every `IFileIO` implementation to add the method, and missing even one implementation breaks that host's build entirely. Low if the length-based approximation is used instead, since it requires no interface change.
- Risk: wiring ~30 save stores to a new guard is a wide, repetitive change — a copy-paste error in one store's `try/catch` (e.g. swallowing an exception type other than `SaveSecurityException`, or forgetting the `catch` entirely) could mask real deserialization failures as silent `false` returns. Test each store's legacy-load path individually, not just as a batch.
- Rollback: `SaveFileGuard` and `SaveSecurityException` are new, self-contained files — deleting them and reverting the `TryLoad` wiring in each save store rolls this back cleanly. If the `IFileIO` port extension path was taken, rolling back also means reverting every adapter implementation that added `GetFileSize`.

### Done when
- [ ] `SaveFileGuard` class created with size (length-based approximation, or true byte-size if the port was extended) and structure checks
- [ ] Explicitly decided and documented which approach was taken — approximation via `ReadAllText().Length`, or a real `IFileIO.GetFileSize` port extension — and why
- [ ] If `IFileIO.GetFileSize` was added: every existing `IFileIO` implementation updated and confirmed to compile (search for `: IFileIO` to find all of them; do not assume there is only one)
- [ ] The real, confirmed set of save stores (verify the actual count and names at implementation time rather than trusting "22" or the corrected "30" list above without re-checking) call `SaveFileGuard.ValidateAndRead` before deserialization
- [ ] Oversized save file rejected with clear error message (tested in Step 7)
- [ ] Empty/malformed save file rejected gracefully
- [ ] Existing valid saves still load without issue

---

## Step 5 — Add JSON Depth Limit to Deserialization

### Goal
Prevent stack overflow attacks from deeply nested JSON structures by enforcing a maximum nesting depth during deserialization.

### Implementation

**Configure `System.Text.Json` options globally:**

Create `Assets/Ashfall.Core/Security/SecureJsonOptions.cs`:

```csharp
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Security;

/// <summary>
/// Secure JSON serialization options for all ASHFALL data loading.
/// Enforces depth limits, size limits, and safe defaults.
/// </summary>
public static class SecureJsonOptions
{
    /// <summary>
    /// Options for loading save files — strict security, moderate depth.
    /// </summary>
    public static JsonSerializerOptions SaveLoad { get; } = new()
    {
        MaxDepth = SecurityConstants.MaxJsonDepth,  // 32
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        // Prevent deserialization of unknown polymorphic types
        // (no JsonDerivedType without explicit opt-in)
    };

    /// <summary>
    /// Options for loading catalog/data files — slightly more permissive for nested data.
    /// </summary>
    public static JsonSerializerOptions CatalogLoad { get; } = new()
    {
        MaxDepth = SecurityConstants.MaxJsonDepth,  // 32
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
    };

    /// <summary>
    /// Options for writing save files — deterministic output.
    /// </summary>
    public static JsonSerializerOptions SaveWrite { get; } = new()
    {
        MaxDepth = SecurityConstants.MaxJsonDepth,
        WriteIndented = true,  // Human-readable saves (debugging)
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        // CORRECTED: the original draft used JsonNamingPolicy.SnakeCaseLower here. That API was
        // added to System.Text.Json in .NET 9 (confirmed against Microsoft's "What's new in
        // System.Text.Json in .NET 9" announcement). This file lives in Assets/Ashfall.Core/,
        // which is compiled by Ashfall.Core.csproj — confirmed net8.0 — and by Ashfall.csproj
        // (the Godot host) — also confirmed net8.0. Using SnakeCaseLower here would fail to
        // compile in both of the two projects that actually build this code; it would only
        // compile in Ashfall.Core.Tests.csproj (net9.0), which doesn't matter because Tests
        // doesn't ship this code, it just references the same source via ProjectReference to
        // Ashfall.Core.csproj, which is the net8.0 build that actually fails.
        //
        // If the save-writing snake_case convention described elsewhere in this project (see
        // AGENTS.md's data-authority section, which calls for snake_case JSON) needs to apply
        // to save files specifically, either:
        //   (a) write a small custom JsonNamingPolicy subclass (a handful of lines, works on any
        //       .NET version with System.Text.Json), or
        //   (b) confirm whether the existing save DTOs already control their own property names
        //       via [JsonPropertyName] attributes, in which case no global naming policy is
        //       needed here at all.
        // Do not reach for .NET 9-only APIs inside Ashfall.Core; confirm target framework
        // compatibility for every System.Text.Json API used here against net8.0, not against
        // whatever SDK happens to be installed on the authoring machine.
    };
}
```

**Files/package note:** `Ashfall.Core.csproj` already references `System.Text.Json` version `8.0.5` explicitly (confirmed by reading the file) rather than relying on the framework-provided version, since `net8.0`'s in-box `System.Text.Json` predates some later API additions. Any new `System.Text.Json` API used in this step must be checked against the 8.0.5 package surface, not against the latest docs — the .NET 9 `SnakeCaseLower` issue above is exactly this class of mistake.

**Replace all raw `JsonSerializer.Deserialize` calls** with calls through `SecureJsonOptions`:

Before:
```csharp
var data = JsonSerializer.Deserialize<CatalogData>(json);
```

After:
```csharp
var data = JsonSerializer.Deserialize<CatalogData>(json, SecureJsonOptions.CatalogLoad);
```

**Audit `SystemTextJsonSerializer`** (the Core `IJsonSerializer` adapter — confirmed to live in `Assets/Ashfall.Core/HostDefaults.cs`, not a hypothetical location):

The real class (confirmed by reading `HostDefaults.cs`) is:

```csharp
public sealed class SystemTextJsonSerializer : IJsonSerializer
{
    public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
    {
        IncludeFields = true,
        PropertyNameCaseInsensitive = true,
        WriteIndented = false
    };

    public string Serialize<T>(T value) => JsonSerializer.Serialize(value, Options);

    public T Deserialize<T>(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return default;
        return JsonSerializer.Deserialize<T>(json, Options);
    }
}
```

**Correction from the original draft:** the draft's "audit" example invented a constructor overload (`SystemTextJsonSerializer(JsonSerializerOptions options = null)`) that does not exist on the real class — the real `Options` field is `public static readonly`, not instance-level and not constructor-injected. Changing this to an instance field with a constructor parameter is itself an API change with its own blast radius (every existing call site that does `new SystemTextJsonSerializer()` would still compile, but anything relying on the static `Options` field directly — confirm via `grep -r "SystemTextJsonSerializer.Options"` before touching this — would break). The minimal, lower-risk change that achieves this step's actual goal (enforcing `MaxDepth` on this adapter) is to add `MaxDepth = SecurityConstants.MaxJsonDepth` directly to the existing static `Options` initializer, not to restructure the class's construction model:

```csharp
public static readonly JsonSerializerOptions Options = new JsonSerializerOptions
{
    IncludeFields = true,
    PropertyNameCaseInsensitive = true,
    WriteIndented = false,
    MaxDepth = SecurityConstants.MaxJsonDepth,  // NEW — the actual minimal fix for this step
};
```

Note also that the real `Options` sets `IncludeFields = true` — the save DTOs in this project are "deliberately all plain public fields" (per `SaveChecksum.cs`'s own doc comment), so any replacement `JsonSerializerOptions` used for save (de)serialization must preserve `IncludeFields = true` or saves will silently stop round-tripping their field data. `SecureJsonOptions.SaveLoad`/`SaveWrite` as drafted above do **not** set `IncludeFields = true` — this is a real bug in the original draft, not just a style gap: if `SecureJsonOptions.SaveLoad` replaces the real `Options` for save loading without adding `IncludeFields = true`, existing saves will silently deserialize into empty/default DTOs. Add `IncludeFields = true` to both `SaveLoad` and `SaveWrite` in `SecureJsonOptions` before wiring it in anywhere.

**Handle depth violation gracefully:**

`System.Text.Json` throws `JsonException` when depth exceeds `MaxDepth`. Wrap this at the save-store level:

```csharp
try
{
    var envelope = _serializer.Deserialize<SaveEnvelope<TState>>(content);
}
catch (JsonException ex) when (ex.Message.Contains("depth"))
{
    _log.Error($"Save file has excessive nesting depth (max {SecurityConstants.MaxJsonDepth}): {ex.Message}");
    state = default;
    return false;
}
```

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless --path . -- --data-integrity-selftest
# All existing data must be within depth 32 (it should be — typical game data is 3-5 levels deep)

# Specifically confirm save round-trips still work with IncludeFields = true carried over —
# this is the one regression this step can plausibly cause silently (empty DTOs on load):
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~SaveWireContract|FullyQualifiedName~SaveChecksum"
```

### Risk & Rollback
- Risk: Medium. The `IncludeFields = true` gap identified above is a real, silent-failure risk — if missed, save loading would not throw an error, it would just produce empty/default DTOs, which is a much worse failure mode than a loud exception (per this project's own "fail closed" security principle from Step 1 — a silent empty-load is the opposite of fail-closed). Treat the `SaveWireContractTests`/`SaveChecksumTests` suites as the primary regression guard for this step, since they exist specifically to catch cross-serializer, cross-host state drift.
- Risk: Low for the depth-limit enforcement itself — `MaxDepth = 32` matches `SaveChecksum.MaxDepth` (also 32, confirmed in `SaveChecksum.cs`), so this step is aligning JSON parsing with a limit the checksum code already assumes, not introducing a new arbitrary number.
- Rollback: revert the `MaxDepth`/`IncludeFields` additions to `SystemTextJsonSerializer.Options`; delete `SecureJsonOptions.cs` if newly introduced call sites are also reverted. Because the minimal fix (editing the existing static `Options` field in place) touches one file, rollback is a single-file revert rather than an API migration.

### Done when
- [ ] `SecureJsonOptions` class created with three preset configurations, **all three including `IncludeFields = true`** to match the existing `SystemTextJsonSerializer.Options` behavior — verified explicitly, not assumed, since this is the step's main silent-failure risk
- [ ] `MaxDepth = 32` enforced on all deserialization paths
- [ ] `SystemTextJsonSerializer`'s existing static `Options` field updated in place with `MaxDepth` (preferred minimal fix) rather than restructured into a new constructor-injected shape, unless a documented reason requires the restructure
- [ ] Deeply nested JSON (>32 levels) rejected with clear error
- [ ] Existing data and saves still load (all under depth 32) — confirmed via `SaveWireContractTests`/`SaveChecksumTests`, not just the data-integrity selftest
- [ ] No `JsonSerializer.Deserialize` calls without options in Core

---

## Step 6 — Add Content Sanitization for Player-Entered Text

### Goal
Strip control characters, excessive whitespace, and potentially dangerous content from all player-entered text (survivor names, journal notes, custom labels) before persistence.

### Implementation

**Create `Assets/Ashfall.Core/Security/TextSanitizer.cs`:**

```csharp
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Ashfall.Core.Security;

/// <summary>
/// Sanitizes player-entered text to prevent injection, corruption,
/// and rendering issues in save files and UI.
/// </summary>
public static class TextSanitizer
{
    // Control characters except newline (\n) and tab (\t)
    private static readonly Regex ControlChars = new(
        @"[\x00-\x08\x0B\x0C\x0E-\x1F\x7F]",
        RegexOptions.Compiled);

    // Collapse multiple spaces/tabs into one
    private static readonly Regex ExcessiveWhitespace = new(
        @"[ \t]{2,}",
        RegexOptions.Compiled);

    // Collapse multiple newlines into max 2
    private static readonly Regex ExcessiveNewlines = new(
        @"\n{3,}",
        RegexOptions.Compiled);

    /// <summary>
    /// Sanitize a player-entered name (survivor names, location labels).
    /// Strict: no newlines, no special chars, length-limited.
    /// </summary>
    public static string SanitizeName(string input, int maxLength = 64)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Strip all control characters including newlines
        string clean = ControlChars.Replace(input, "");
        clean = clean.Replace("\n", "").Replace("\r", "");

        // Collapse whitespace
        clean = ExcessiveWhitespace.Replace(clean, " ");

        // Trim
        clean = clean.Trim();

        // Length limit
        if (clean.Length > maxLength)
            clean = clean[..maxLength];

        return clean;
    }

    /// <summary>
    /// Sanitize player-entered freeform text (journal entries, notes).
    /// Permissive: allows newlines and basic formatting, strips dangerous chars.
    /// </summary>
    public static string SanitizeText(string input,
        int maxLength = SecurityConstants.MaxPlayerTextLength)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Normalize line endings
        string clean = input.Replace("\r\n", "\n").Replace("\r", "\n");

        // Strip control characters (keep \n and \t)
        clean = ControlChars.Replace(clean, "");

        // Collapse excessive whitespace and newlines
        clean = ExcessiveWhitespace.Replace(clean, " ");
        clean = ExcessiveNewlines.Replace(clean, "\n\n");

        // Trim
        clean = clean.Trim();

        // Length limit
        if (clean.Length > maxLength)
            clean = clean[..maxLength];

        return clean;
    }

    /// <summary>
    /// Validate that text contains no null bytes or binary content.
    /// Returns false if the text appears to be binary data.
    /// </summary>
    public static bool IsPlainText(string input)
    {
        if (string.IsNullOrEmpty(input)) return true;

        foreach (char c in input)
        {
            if (c == '\0') return false;  // Null byte = binary
        }

        return true;
    }

    /// <summary>
    /// Strip Unicode bidirectional override characters that could
    /// be used for text rendering attacks (e.g., filename spoofing).
    /// </summary>
    public static string StripBidiOverrides(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        var sb = new StringBuilder(input.Length);
        foreach (char c in input)
        {
            // Skip RTL/LTR override, embedding, isolate characters
            if (c is '\u200E' or '\u200F' or '\u202A' or '\u202B'
                or '\u202C' or '\u202D' or '\u202E' or '\u2066'
                or '\u2067' or '\u2068' or '\u2069')
                continue;
            sb.Append(c);
        }
        return sb.ToString();
    }
}
```

**Wire into survivor naming:**

Wherever survivor names are set (character creation, rename):

```csharp
public void SetSurvivorName(int survivorId, string rawName)
{
    string sanitized = TextSanitizer.SanitizeName(rawName, maxLength: 32);
    if (string.IsNullOrEmpty(sanitized))
    {
        _log.Warn("Survivor name rejected (empty after sanitization)");
        return;
    }
    _survivors[survivorId].Name = sanitized;
}
```

**Wire into journal system:**

```csharp
public void AddJournalEntry(string rawText)
{
    string sanitized = TextSanitizer.SanitizeText(rawText,
        maxLength: SecurityConstants.MaxPlayerTextLength);
    if (string.IsNullOrEmpty(sanitized)) return;
    _entries.Add(new JournalEntry
    {
        Day = _clock.Day,  // CORRECTED: IClock (Assets/Ashfall.Core/Ports.cs) exposes `Day`, not
                            // `CurrentDay` — confirmed by reading the interface directly.
        Text = sanitized
    });
}
```

**Apply bidi stripping to all loaded text** that will be rendered in UI (prevents RTL override attacks in modded content):

```csharp
// In catalog loading pipeline
displayText = TextSanitizer.StripBidiOverrides(displayText);
```

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj

# Verify no engine references
grep -r "UnityEngine\|Godot\." Assets/Ashfall.Core/Security/TextSanitizer.cs
# Expected: 0
```

### Risk & Rollback
- Risk: Low-Medium. `SanitizeName`/`SanitizeText` are only useful if actually wired into every player-text entry point — the illustrative `SetSurvivorName`/`AddJournalEntry` snippets above are examples of the *pattern*, not a confirmed list of real call sites in this codebase (neither method name was found in a codebase search during this review). Before implementing, search for the real methods that currently set survivor names and journal text and confirm the actual signatures before writing wiring code against invented method names.
- Risk: retroactively sanitizing already-persisted player text (existing saves with names/journal entries written before this step existed) is out of scope as drafted — decide explicitly whether old saves get sanitized on next load/save, or only new input going forward. The draft is silent on this; silence here would mean old control-character-containing text persists indefinitely even after this step ships, which may or may not be acceptable — flag to the user rather than assume.
- Rollback: `TextSanitizer.cs` is a new, self-contained file; rollback means removing its call sites (each an isolated one-line wrap around existing input handling) and deleting the file.

### Done when
- [ ] `TextSanitizer` class created with `SanitizeName`, `SanitizeText`, `IsPlainText`, `StripBidiOverrides`
- [ ] Control characters (0x00–0x1F except \n, \t) stripped from all player text
- [ ] The real (not illustrative) survivor-naming and journal-entry code paths identified in this codebase and wired to `SanitizeName`/`SanitizeText` respectively — confirm actual method/class names before wiring, since the draft's examples were illustrative pseudocode, not verified real call sites
- [ ] Bidirectional override characters stripped from rendered text
- [ ] Existing game text unaffected (sanitization only on player input paths)
- [ ] Explicit decision recorded on whether pre-existing saved player text gets sanitized retroactively or only new input going forward

---

## Step 7 — Write Security Tests

### Goal
Comprehensive test coverage for all security mechanisms: HMAC signing, input validation, save size limits, depth limits, and text sanitization. Tests serve as regression guards and documentation of security contracts.

### Implementation

**Create `Ashfall.Core.Tests/Security/SaveSignerTests.cs`:**

```csharp
using Ashfall.Core.Security;
using Xunit;

namespace Ashfall.Core.Tests.Security;

public class SaveSignerTests
{
    private readonly SaveSigner _signer;

    public SaveSignerTests()
    {
        byte[] testKey = new byte[32];
        for (int i = 0; i < 32; i++) testKey[i] = (byte)(i + 1);
        _signer = new SaveSigner(testKey);
    }

    [Fact]
    public void Sign_ProducesNonEmptySignature()
    {
        string sig = _signer.Sign("{\"health\":100}");
        Assert.False(string.IsNullOrEmpty(sig));
    }

    [Fact]
    public void Verify_ValidSignature_ReturnsTrue()
    {
        string payload = "{\"health\":100,\"day\":42}";
        string sig = _signer.Sign(payload);
        Assert.True(_signer.Verify(payload, sig));
    }

    [Fact]
    public void Verify_TamperedPayload_ReturnsFalse()
    {
        string original = "{\"health\":100}";
        string sig = _signer.Sign(original);
        string tampered = "{\"health\":999}";
        Assert.False(_signer.Verify(tampered, sig));
    }

    [Fact]
    public void Verify_ForgedSignature_ReturnsFalse()
    {
        string payload = "{\"health\":100}";
        string forged = Convert.ToBase64String(new byte[32]); // Wrong HMAC
        Assert.False(_signer.Verify(payload, forged));
    }

    [Fact]
    public void Verify_EmptySignature_ReturnsFalse()
    {
        Assert.False(_signer.Verify("{}", ""));
        Assert.False(_signer.Verify("{}", null));
    }

    [Fact]
    public void Verify_EmptyPayload_ReturnsFalse()
    {
        Assert.False(_signer.Verify("", "abc123"));
        Assert.False(_signer.Verify(null, "abc123"));
    }

    [Fact]
    public void Sign_DifferentPayloads_DifferentSignatures()
    {
        string sig1 = _signer.Sign("{\"a\":1}");
        string sig2 = _signer.Sign("{\"a\":2}");
        Assert.NotEqual(sig1, sig2);
    }

    [Fact]
    public void Sign_SamePayload_SameSignature()
    {
        string payload = "{\"deterministic\":true}";
        Assert.Equal(_signer.Sign(payload), _signer.Sign(payload));
    }

    [Fact]
    public void Constructor_RejectsShortKey()
    {
        Assert.Throws<ArgumentException>(() => new SaveSigner(new byte[16]));
    }
}
```

**Create `Ashfall.Core.Tests/Security/InputValidatorTests.cs`:**

```csharp
using Ashfall.Core.Security;
using Xunit;

namespace Ashfall.Core.Tests.Security;

public class InputValidatorTests
{
    [Fact]
    public void ValidateString_WithinLimit_PassesThrough()
    {
        string result = InputValidator.ValidateString("hello", "field", maxLength: 10);
        Assert.Equal("hello", result);
    }

    [Fact]
    public void ValidateString_ExceedsLimit_Throws()
    {
        string longStr = new string('x', 10000);
        Assert.Throws<DataValidationException>(() =>
            InputValidator.ValidateString(longStr, "field", maxLength: 8192));
    }

    [Fact]
    public void ValidateString_ExceedsLimit_Truncates_WhenPolicyIsTruncate()
    {
        string longStr = new string('x', 100);
        string result = InputValidator.ValidateString(longStr, "field",
            maxLength: 50, policy: ValidationPolicy.Truncate);
        Assert.Equal(50, result.Length);
    }

    [Fact]
    public void ValidateArray_WithinLimit_PassesThrough()
    {
        var list = new int[] { 1, 2, 3 };
        var result = InputValidator.ValidateArray(list, "items", maxCount: 100);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void ValidateArray_ExceedsLimit_Throws()
    {
        var hugeList = new int[20_000];
        Assert.Throws<DataValidationException>(() =>
            InputValidator.ValidateArray(hugeList, "items", maxCount: 10_000));
    }

    [Fact]
    public void ValidateRange_Int_WithinRange_PassesThrough()
    {
        Assert.Equal(50, InputValidator.ValidateRange(50, "hp", 0, 100));
    }

    [Fact]
    public void ValidateRange_Int_OutOfRange_Throws()
    {
        Assert.Throws<DataValidationException>(() =>
            InputValidator.ValidateRange(-1, "hp", 0, 100));
        Assert.Throws<DataValidationException>(() =>
            InputValidator.ValidateRange(101, "hp", 0, 100));
    }

    [Fact]
    public void ValidateRange_Float_NaN_Throws()
    {
        Assert.Throws<DataValidationException>(() =>
            InputValidator.ValidateRange(float.NaN, "weight", 0f, 100f));
    }

    [Fact]
    public void ValidateRange_Float_Infinity_Throws()
    {
        Assert.Throws<DataValidationException>(() =>
            InputValidator.ValidateRange(float.PositiveInfinity, "weight", 0f, 100f));
    }

    [Fact]
    public void ValidateId_ValidSnakeCase_PassesThrough()
    {
        Assert.Equal("item_water_filter", InputValidator.ValidateId("item_water_filter", "id"));
    }

    [Fact]
    public void ValidateId_TooLong_Throws()
    {
        string longId = new string('a', 200);
        Assert.Throws<DataValidationException>(() =>
            InputValidator.ValidateId(longId, "id"));
    }
}
```

**Create `Ashfall.Core.Tests/Security/SaveFileGuardTests.cs`:**

```csharp
using Ashfall.Core.Security;
using Xunit;

namespace Ashfall.Core.Tests.Security;

public class SaveFileGuardTests
{
    [Fact]
    public void ValidateContent_NullContent_Throws()
    {
        Assert.Throws<SaveSecurityException>(() =>
            SaveFileGuard.ValidateContent(null));
    }

    [Fact]
    public void ValidateContent_EmptyContent_Throws()
    {
        Assert.Throws<SaveSecurityException>(() =>
            SaveFileGuard.ValidateContent(""));
    }

    [Fact]
    public void ValidateContent_OversizedContent_Throws()
    {
        string huge = new string(' ', (int)SecurityConstants.MaxSaveFileSizeBytes + 1);
        Assert.Throws<SaveSecurityException>(() =>
            SaveFileGuard.ValidateContent(huge));
    }

    [Fact]
    public void ValidateContent_ValidJson_Passes()
    {
        SaveFileGuard.ValidateContent("{\"version\":1,\"state\":{}}");
        // No exception = pass
    }
}
```

**Create `Ashfall.Core.Tests/Security/TextSanitizerTests.cs`:**

```csharp
using Ashfall.Core.Security;
using Xunit;

namespace Ashfall.Core.Tests.Security;

public class TextSanitizerTests
{
    [Fact]
    public void SanitizeName_StripsControlChars()
    {
        string dirty = "Survivor\x00\x01\x02Name";
        Assert.Equal("SurvivorName", TextSanitizer.SanitizeName(dirty));
    }

    [Fact]
    public void SanitizeName_StripsNewlines()
    {
        Assert.Equal("John Doe", TextSanitizer.SanitizeName("John\nDoe"));
    }

    [Fact]
    public void SanitizeName_TruncatesToMaxLength()
    {
        string longName = new string('A', 100);
        string result = TextSanitizer.SanitizeName(longName, maxLength: 32);
        Assert.Equal(32, result.Length);
    }

    [Fact]
    public void SanitizeName_CollapsesWhitespace()
    {
        Assert.Equal("John Doe", TextSanitizer.SanitizeName("John    Doe"));
    }

    [Fact]
    public void SanitizeName_EmptyInput_ReturnsEmpty()
    {
        Assert.Equal(string.Empty, TextSanitizer.SanitizeName(""));
        Assert.Equal(string.Empty, TextSanitizer.SanitizeName(null));
    }

    [Fact]
    public void SanitizeText_PreservesNewlines()
    {
        string input = "Line one\nLine two";
        Assert.Equal("Line one\nLine two", TextSanitizer.SanitizeText(input));
    }

    [Fact]
    public void SanitizeText_CollapsesExcessiveNewlines()
    {
        string input = "Para one\n\n\n\n\nPara two";
        Assert.Equal("Para one\n\nPara two", TextSanitizer.SanitizeText(input));
    }

    [Fact]
    public void SanitizeText_StripsControlCharsKeepsTabs()
    {
        string input = "Item:\t100\x03units";
        Assert.Contains("\t", TextSanitizer.SanitizeText(input));
        Assert.DoesNotContain("\x03", TextSanitizer.SanitizeText(input));
    }

    [Fact]
    public void StripBidiOverrides_RemovesRtlOverride()
    {
        string malicious = "normal\u202Eesrever";  // RTL override
        string clean = TextSanitizer.StripBidiOverrides(malicious);
        Assert.DoesNotContain("\u202E", clean);
        Assert.Contains("normal", clean);
    }

    [Fact]
    public void IsPlainText_WithNullByte_ReturnsFalse()
    {
        Assert.False(TextSanitizer.IsPlainText("binary\x00data"));
    }

    [Fact]
    public void IsPlainText_NormalText_ReturnsTrue()
    {
        Assert.True(TextSanitizer.IsPlainText("Normal journal entry."));
    }
}
```

**Create `Ashfall.Core.Tests/Security/SecureJsonOptionsTests.cs`:**

```csharp
using System.Text.Json;
using Ashfall.Core.Security;
using Xunit;

namespace Ashfall.Core.Tests.Security;

public class SecureJsonOptionsTests
{
    [Fact]
    public void DeeplyNestedJson_Throws_JsonException()
    {
        // Create JSON nested 40 levels deep (exceeds MaxJsonDepth of 32)
        string deep = new string('[', 40) + "1" + new string(']', 40);
        Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<object>(deep, SecureJsonOptions.SaveLoad));
    }

    [Fact]
    public void NormalDepthJson_Parses_Successfully()
    {
        // 5 levels deep — well within limit
        string normal = "{\"a\":{\"b\":{\"c\":{\"d\":{\"e\":1}}}}}";
        var result = JsonSerializer.Deserialize<object>(normal, SecureJsonOptions.SaveLoad);
        Assert.NotNull(result);
    }

    [Fact]
    public void SaveLoad_MaxDepth_Is32()
    {
        Assert.Equal(32, SecureJsonOptions.SaveLoad.MaxDepth);
    }

    [Fact]
    public void CatalogLoad_MaxDepth_Is32()
    {
        Assert.Equal(32, SecureJsonOptions.CatalogLoad.MaxDepth);
    }
}
```

**Test count target:** the specific per-file counts below (9/11/4/11/4 = 30) are a target based on the illustrative test bodies shown above, not a hard contract — implement thorough coverage for each mechanism and report the actual count achieved, don't force exactly 30 if fewer well-designed tests cover the same ground or if edge cases surface that need more:
- HMAC sign/verify (~9 tests)
- Input bounds validation (~11 tests)
- Save file size guard (~4 tests)
- Text sanitization (~11 tests)
- JSON depth limits (~4 tests)

**Note on `ValidateContent_OversizedContent_Throws`:** the illustrative test allocates a string of `MaxSaveFileSizeBytes + 1` characters (16 MB + 1). This works but is a genuinely large single-test allocation; if test run time becomes noticeable, consider testing the size-check logic against a smaller injected limit (e.g. construct with a test-only small `maxLength` parameter) rather than the full 16 MB production constant, so the test suite doesn't carry a 16 MB allocation just to prove a `>` comparison works.

### Verification
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Security"
# All security tests pass — record the actual count, do not assert "30+" without counting

dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
# Full suite still passes (no regressions) — get the real before/after total test count via
# --list-tests, per the Batch 111 Step 4 correction about not trusting carried-over test counts

godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

### Risk & Rollback
- Risk: None net-new from the tests themselves (tests don't ship). The risk is entirely in what Step 7 might reveal about Steps 2-6 — e.g. if `SecureJsonOptionsTests.SaveLoad_MaxDepth_Is32` fails because `IncludeFields` wasn't preserved (see Step 5 correction), that's this step doing its job, not a problem with this step.
- Rollback: delete the new test files; no production code changes happen in this step.

### Done when
- [ ] `SaveSignerTests`, `InputValidatorTests`, `SaveFileGuardTests`, `TextSanitizerTests`, `SecureJsonOptionsTests` all created and passing, with the actual test count per class recorded (targets above are illustrative, not contractual)
- [ ] All existing tests still pass (no regressions) — confirmed via an actual `--list-tests` count comparison before/after, not assumed
- [ ] Full 5-step verification checklist passes, run for real in this session (not assumed from earlier steps having passed individually)

---

## Summary Table

| Step | Title | Key Deliverable | Risk | Estimated Effort |
|------|-------|----------------|------|-----------------|
| 1 | Evaluate Threat Model | `docs/security-policy.md` + `SecurityConstants.cs` | None | 1 hour |
| 2 | HMAC Save Signing (deterrence only — see Context) | `SaveSigner.cs`, `KeyDerivation.cs`, envelope extension | Low-Medium | 3-4 hours |
| 3 | Input Bounds Validation | `InputValidator.cs`, wired to 5+ catalog loaders | Low-Medium | 2-3 hours |
| 4 | Save File Size Limits | `SaveFileGuard.cs`, wired to the real ~30 save stores (verify exact count/names at implementation time) | Low-Medium (Medium if `IFileIO` is extended — breaking interface change) | 2 hours |
| 5 | JSON Depth Limits | `SecureJsonOptions.cs`, audit all deserialization calls, preserve `IncludeFields = true` | Medium (silent-failure risk if `IncludeFields` is dropped) | 1-2 hours |
| 6 | Content Sanitization | `TextSanitizer.cs`, wired to the real (verified, not assumed) survivor-name and journal call sites | Low-Medium | 2 hours |
| 7 | Security Tests | Comprehensive tests across 5 test classes (~30 tests, not a hard target) | None | 2-3 hours |

**Total estimated effort:** 13-16 hours (unchanged as a rough order-of-magnitude estimate, but treat Step 4's port-extension path as a possible additional hour or two if every `IFileIO` adapter needs updating)
**Expected outcome:** Layered defense against casual save tampering, input-based DoS, and rendering attacks. All security code engine-agnostic in `Ashfall.Core/Security/`. **This is not real security against a motivated player on a client-only game — it is deterrence, clearly labeled as such throughout this document and in the shipped code's own doc comments.**

---

## Verification Checklist (Batch 112 Complete)

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # currently FAILS from a clean state today — see Batch 111's Review Notes for the pre-existing CS0523 in untracked Assets/Ashfall.Core/ActionResult.cs; resolve before starting this batch, it blocks every step's verification
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # PASS expected once build succeeds — confirm actual test count via --list-tests; do not cite "1941" from memory, it was not independently re-verified for this review
dotnet build Ashfall.csproj                                  # currently FAILS from a clean state today, same root cause
godot --headless --path . -- --data-integrity-selftest       # PASS (0 errors) — not re-verified in this review; run before relying on it
godot --headless --path . -- --bridge-selftest               # PASS (exit 0) — not re-verified in this review; run before relying on it
```

All security code lives in `Assets/Ashfall.Core/Security/` — zero engine references, fully testable without Unity or Godot.

---

## Security Architecture Diagram

```
┌─────────────────────────────────────────────────────────┐
│                    SAVE FILE (JSON)                       │
├─────────────────────────────────────────────────────────┤
│  Layer 1: File Size Guard (SaveFileGuard)                │
│    → Reject content whose length exceeds the configured   │
│      limit (approximation via ReadAllText().Length; see   │
│      Step 4 correction — not a true pre-read byte check    │
│      unless IFileIO is extended with GetFileSize)          │
├─────────────────────────────────────────────────────────┤
│  Layer 2: JSON Depth Limit (SecureJsonOptions)           │
│    → Reject nesting > 32 levels during parse             │
├─────────────────────────────────────────────────────────┤
│  Layer 3: HMAC Verification (SaveSigner) — DETERRENCE     │
│    → Reject payloads with a mismatched signature; does     │
│      NOT stop a player willing to decompile the game and   │
│      extract the key (see Context/Review Notes)            │
├─────────────────────────────────────────────────────────┤
│  Layer 4: Input Bounds (InputValidator)                   │
│    → Reject oversized strings, arrays, out-of-range nums │
├─────────────────────────────────────────────────────────┤
│  Layer 5: Content Sanitization (TextSanitizer)           │
│    → Strip control chars, bidi overrides from text fields│
├─────────────────────────────────────────────────────────┤
│  Layer 6: Structural Checksum (SaveChecksum — existing,   │
│    unkeyed SHA256, unchanged by this batch)               │
│    → Detect accidental corruption, not tampering          │
└─────────────────────────────────────────────────────────┘
```

**Correction on layer independence:** the original draft claimed "each layer is independent... fail-fast, fail-closed" for all six layers. This is not quite accurate: Layer 3 (HMAC verification) and Layer 4 (input bounds) both operate on the *parsed* object graph, which means they inherently depend on Layer 2 (JSON depth/parse) having already succeeded — a save that fails to parse never reaches Layers 3-6 by construction, not because of independent enforcement. The layers are ordered and short-circuiting (each earlier failure prevents later layers from running at all), which is the correct fail-closed behavior, but "independent" overstates it — Layers 3-6 cannot run without Layer 2 having succeeded first. Describe this as an ordered pipeline with early-exit, not as independent parallel checks.



---

## Review Notes (Corrected)

This batch was adversarially reviewed against the real repository state on the date of this edit. Corrections made:

1. **`SaveChecksum` was mischaracterized.** The original draft called it "reflection-based hashing" without specifying the algorithm. Verified directly against `Assets/Ashfall.Core/SaveChecksum.cs`: it is an **unkeyed SHA256** over a reflection-walked canonical text form of the object graph (`SaveChecksum.Compute(object root)` takes no key parameter at all). There is no HMAC, no encryption, and no keyed hash anywhere in the codebase today — confirmed by reading the full file. The Context section and Step 2's introduction now state this precisely rather than vaguely.

2. **`IFileIO`'s real signature was confirmed and a fabricated method was removed.** The real interface (`Assets/Ashfall.Core/Ports.cs`, confirmed by reading it) has exactly 5 methods: `bool DirectoryExists(string path)`, `bool FileExists(string path)`, `string ReadAllText(string path)`, `void WriteAllText(string path, string contents)`, `string Combine(params string[] parts)`. The original draft's Step 4 invented a 6th method, `long GetFileSize(string path)`, and used it as if it already existed ("Extend `IFileIO` port (if not already present)" — it was not present, and the draft's own `ValidateAndRead` code called it unconditionally, which would not compile). This has been corrected to either (a) a length-based approximation using the real `ReadAllText` return value, which requires no interface change, or (b) an explicit, clearly-flagged breaking interface change if true pre-read byte-size checking is later judged necessary — with an explicit warning that extending `IFileIO` requires updating every implementation (confirmed via `HostDefaults.cs`'s `FileSystemIO : IFileIO` as at least one concrete adapter that would need the new method).

3. **The "22 save stores" figure was wrong.** A direct codebase search found **30** distinct `*SaveStore` classes today, with several names in the original draft's enumerated list (`WeatherSaveStore`, `RadiationSaveStore`, `NeedsSaveStore`, `RecipeSaveStore`, `QuestSaveStore`, `FactionSaveStore`) not matching any real class, while real ones (`CaravanSaveStore`, `CraftingSaveStore`, `DailyBriefingSaveStore`, `DoseLedgerSaveStore`, `ExpansionHubSaveStore`, `GreenhouseSaveStore`, `HoldfastTradeSaveStore`, `MedicalWardSaveStore`, `MemorialSaveStore`, `PhantomMemorySaveStore`, `Phase0SaveStore`, `PowerGridSaveStore`, `RadioSaveStore`, `ShelterAssignmentSaveStore`, `StartingLevelSaveStore`) were missing from the list entirely. Steps 2 and 4 now cite the corrected count and name list, with an explicit instruction to re-verify at implementation time since this count can drift as expansions land.

4. **A .NET version incompatibility was found in the sample code.** Step 5's `SecureJsonOptions.SaveWrite` used `JsonNamingPolicy.SnakeCaseLower`, an API added to `System.Text.Json` in **.NET 9** (confirmed against Microsoft's .NET 9 `System.Text.Json` announcement). This file lives in `Assets/Ashfall.Core/`, compiled by both `Ashfall.Core.csproj` and `Ashfall.csproj` — both confirmed **`net8.0`** — so this line would fail to compile in the two projects that actually ship this code, and only "accidentally" compile in the `net9.0` Tests project, masking the real failure until someone tried to build the Godot host or Core standalone. This has been replaced with guidance to write a custom `JsonNamingPolicy` subclass or confirm existing `[JsonPropertyName]` usage instead.

5. **A silent-data-loss risk was found by comparing the plan's proposed JSON options against the real serializer.** The real `SystemTextJsonSerializer.Options` (`Assets/Ashfall.Core/HostDefaults.cs`, confirmed by reading it) sets `IncludeFields = true`, which matters because ASHFALL's save DTOs are "deliberately all plain public fields" per `SaveChecksum.cs`'s own doc comment. The plan's drafted `SecureJsonOptions.SaveLoad`/`SaveWrite` did **not** set `IncludeFields = true`. If those options replaced the real serializer's options without this flag, saves would not throw an error on load — they would silently deserialize into empty/default DTOs, which is a worse failure mode than a loud exception and directly violates this project's own "fail closed" security principle from Step 1. Step 5 now calls this out explicitly and requires `IncludeFields = true` on both new option sets, and recommends editing the real `Options` field in place rather than restructuring `SystemTextJsonSerializer`'s constructor (which the original draft's "audit" example did, incorrectly, by inventing a constructor overload that doesn't exist on the real class).

6. **A minor factual error in illustrative code** (`_clock.CurrentDay` in the journal-wiring example) was corrected to `_clock.Day`, matching the real `IClock` interface (`Assets/Ashfall.Core/Ports.cs`, confirmed by reading it).

7. **The HMAC-as-deterrence caveat is now stated at every point it matters, not only as a footnote.** The original draft's only acknowledgment that "obfuscated, not plaintext" key storage is weak appeared once, in a single "Residual risk" bullet in the threat model. This review adds the same caveat to: the Context section (stated upfront before Step 2 is even reached), `SaveSigner`'s own doc comment, `KeyDerivation`'s own doc comment, the T1 and T4 threat-model entries, the Security Principles list (a new principle 6, "Honest labeling"), the Summary Table's Step 2 row, the closing "Expected outcome" line, and the architecture diagram's Layer 3 label. The goal is that this caveat survives even if only one section of this document is read in isolation — a common failure mode for security docs is that the honest caveat lives in one paragraph nobody re-reads once implementation starts.

8. **Vague or overstated Done-when criteria were tightened throughout**, in the same style as the Batch 111 corrections: "at least 5 catalog loaders" now asks for the specific 5 to be named; "All 22 save stores" became "the real, confirmed set"; test-count targets ("9 tests," "11 tests," etc.) are now explicitly illustrative rather than contractual; and the closing verification checklist's "PASS" assertions were corrected to reflect that a clean build currently fails in this repository for an unrelated, pre-existing reason (see Batch 111's Review Notes for the `ActionResult.cs` CS0523 finding, which blocks this batch's verification exactly as it blocks Batch 111's).

9. **The "each layer is independent" architecture claim was corrected.** Layers 3-6 in the security architecture diagram all operate on data that must have already survived Layer 2's JSON parse; they are not independent, they are an ordered, short-circuiting pipeline. This is still correctly fail-closed behavior, but the original wording overstated the layers' independence from one another.

10. **Risk & Rollback subsections were added to every step**, matching the Batch 111 corrections — the original draft had one header-level "Risk: Low-Medium" line and nothing per-step, despite this batch introducing a breaking-interface-change option (Step 4's `IFileIO.GetFileSize`), a wide fan-out change touching ~30 save stores (Step 4), and a genuine silent-data-loss risk (Step 5's `IncludeFields` gap) that deserved explicit rollback guidance.
