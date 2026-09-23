# PLAN-ORPHAN-SEAL-01 — Appendix C: Integration Pattern Catalogue

**Generated:** 2026-09-21 · **Purpose:** the concrete, reusable patterns a
builder copies when wiring an orphan authority. Every pattern cites a real
example in this repository; every *anti-pattern* cites a failure the ledger
records. Use with Appendix A (dossiers) and Appendix B (wave packages).

---

## C.1 Host session (`src/Host/<Domain>HostSession.cs`)

**When:** a stateful Core authority needs an owner, catalog binding, day tick,
or save hook.

**Shape**
```csharp
public sealed class ExampleHostSession : IDisposable
{
    public ExampleSystem System { get; }
    public bool IsDirty { get; private set; }

    public static ExampleHostSession Create(string dataDir, ILog log)
    {
        var catalog = ExampleCatalog.LoadFromDirectory(dataDir);
        var system  = new ExampleSystem(catalog, log);
        var saved   = ExampleSaveStore.TryLoad();
        if (saved != null) system.RestoreState(saved);
        return new ExampleHostSession(system);
    }

    public void TickDay(int day, ISeededRng rng) { System.TickDay(day, rng); IsDirty = true; }
    public ExampleSaveState Capture() => System.CaptureState();
    public void Dispose() { /* unsubscribe everything here; idempotent */ }
}
```

**Real examples:** `src/Host/BlackMarketHostSession.cs`, `src/Host/AutopsyHostSession.cs`,
`src/Host/ApprenticeshipHostSession.cs`, `src/Host/AirlockSecurityHostSession.cs`.

**Checklist:** idempotent create; catalog missing → dormant, never throw at
boot; restore old-save defaults; one instance per campaign; dispose unsubscribes
and is idempotent.

**Anti-patterns:** constructing inside a panel; two sessions for one system;
dispose that throws (`ShelterOperationsAudioBridge` pre-fix).

---

## C.2 Save store (`src/Host/<Domain>SaveStore.cs`)

**When:** the authority owns durable state that no existing section models.

**Shape:** `SaveStoreHub.FromCodec` + `SchemaVersionedEnvelope`; `TryLoad`,
`Save`, `Delete`; checksum verify; failure returns null (never throws at boot).

**Real examples:** `src/Host/SanitationSaveStore.cs`, `src/Host/BionicsSaveStore.cs`;
registry: `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (204 sections);
matrix: `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` (generated).

**Rule:** prefer riding an existing section. A new section needs an integrator
signature, a named state list, a migration default, and a matrix/count update
(PLAN-SAVE-GOVERNANCE-12).

**Anti-pattern:** a store per entity; a second backup mechanism
(`SessionDurabilityManager` remains the recovery owner).

---

## C.3 Day owner (`src/Main.CampaignOwners.cs`)

**When:** the system mutates after load on a campaign day boundary.

**Shape:** register one owner with `ownerId`, phase, order, and a `TickDay`
that is exactly-once per day; emit a day event; never mutate in `_Process`.

**Real examples:** `HygieneDayOwner` (phase 3), `EconomyMarketDayOwner`,
`UnderworldMarketDayOwner`, `SubterraneanDayOwner`.

**Rule:** declare ordering before the package merges; the owner-order probe
fails on silent changes (PLAN-TEMPORAL-AUTHORITY-33).

**Anti-patterns:** polling in `_Ready`; two owners ticking the same authority;
RNG without a declared stream.

---

## C.4 Manifest entry (`SubsystemManifest`)

**When:** the subsystem must be discoverable by the integration selftest and
the bootstrap.

**Shape:** `new("<id>", "<Name>", LifecyclePhase.X, "<save_section|->",
"<day_owner|->", "<panel_route|->", HasDedicatedSetup, "<description>")` plus a
host-bound `SetupAction`.

**Rule:** the manifest is integrator-owned; entries are additive; the kit
selftest proves setup/session/save/route/selftest per entry
(PLAN-INTEGRATION-KIT-02).

**Anti-pattern:** a subsystem that exists only in a `SetupX()` method and never
in the manifest — the exact gap that produced the 99 orphans.

---

## C.5 Panel route (read surface only)

**When:** the player must see or command the system.

**Shape:** descriptor in `Main.PlayerSurfaces.cs`, open/close handlers, refresh
from a read-only projection, dispose on close; panel calls **commands**, never
holds state.

**Real examples:** `ChroniclePanel` (routed read-only surface, 2026-09-20),
`AchievementsPanel` (data-backed conditions), `DoseGeographyPanel`.

**Gates:** `PanelRouteGateTests`, `PlayerSurfaceCoverageGateTests`,
`PlayerSurfaceLivenessGateTests`, `--panel-bind-lifecycle-selftest`.

**Anti-patterns:** a panel that mutates gameplay; a route without a descriptor
(`low_background_metrology` pre-fix); 100 orphan controls at shutdown.

---

## C.6 Journal key (player feedback seam)

**When:** a fact must be visible without a new panel.

**Shape:** `JournalSystem.TryAddRawEntry(kind, text, payload, day)` with a
stable dedup key; per-severity keys, not free text.

**Real examples:** `grain_milling_archive_<id>`, `sleep_phantom_pain`,
`chronicle` entries.

**Rule:** one entry per fact/severity; never journal every tick; the journal is
the single feedback strip.

---

## C.7 Audio cue binding

**When:** a critical event must be audible/visible.

**Shape:** catalog cue (`audio_cues.json` / `shelter_audio_cues.json`) + event
producer + bridge binding (`AudioManager.RefreshDomainBindings`,
`IShelterOperationsAudioProvider` pattern) + Plan 169 visual notification for
critical cues.

**Real examples:** `ShelterOperationsAudioBridge` (post-fix), Plan 169
`AudioAccessibilityCoordinator`, `MachineTellAudioSyncTests`.

**Anti-patterns:** a bridge never instantiated; dispose throwing; a cue with no
producer event.

---

## C.8 CLI probe (`--<domain>-selftest`)

**When:** the system is headless-provable or the only surface is diagnostic.

**Shape:** register in `HostCliRegistry`; dispatch in a `HostCli.*` partial;
assert counts; emit `[HOST_SELFTEST_SUMMARY]`/`[HOST_SELFTEST_JSON]`; fail
non-zero on defect.

**Real examples:** `HostCli.DynamicWorld.cs`, `HostCli.Plans162_165.cs`,
`HostCli.Mods.cs`.

**Rule:** a probe must be able to fail (PLAN-SELFTEST-TRUTH-23); a registered
verb must have a handler.

---

## C.9 Data catalog loader (`Assets/Ashfall.Core/**/*CatalogLoader.cs`)

**When:** authored JSON reaches gameplay.

**Shape:** schema envelope (`schema_version` int), snake_case DTOs with
`JsonPropertyName`, nullable fields, collected errors, invariant culture,
loaded through `CatalogPath`/`IFileSystem` — never a raw path in Core
(PLAN-ARCHITECTURE-BOUNDARY-31).

**Real examples:** `ResearchKnowledgeCatalogLoader`, `DebtTemplateCatalog`,
`WastelandMapCatalogLoader`, `SanityShellCatalogLoader` family.

**Rule:** a catalog row without a consumer is an orphan; the field-consumption
gate flags unread fields (PLAN-DATA-CONSUMER-22).

---

## C.10 Read-model projection

**When:** a panel needs state without authority.

**Shape:** a pure function over canonical state (`ProjectCanonicalMap`,
`RehabilitationSlateProjection`, `UndergroundEconomyPressure`,
`CompletionHistorySummary`), immutable, deterministic, no mutation, no store.

**Rule:** projections never write; a projection that caches live state becomes
a second authority.

---

## C.11 Seeded RNG fork

**When:** the system rolls dice.

**Shape:** registered `CampaignStreamIds` constant (snake_case); `Fork(stream,
day, salt)`; fork-per-day granularity; documented salt rule; replay test.

**Real examples:** `CampaignRngStream.cs` (~35 streams),
`CampaignRngManager.Fork(CampaignStreamIds.BlackMarketStock, day)`.

**Anti-patterns:** `System.Random`, `GetHashCode`, wall-clock seeds;
re-numbering an existing stream.

---

## C.12 Event seam

**When:** a Core fact must reach a host effect.

**Shape:** `public event Action<T>? OnFact;` raised after the mutation; host
adapter subscribes and applies presentation/persistence; unsubscribe on
dispose.

**Real examples:** `RelationshipDecaySystem.BondDriftBridge`,
`MemorialSystem.OnMemorialized`, `ExcavationHazardSystem.OnMethaneIgnition`.

**Audit:** 84 Core events had zero subscribers in the 2026-09-21 audit
(PLAN-EVENT-WIRING-21); new events need a producer **and** a consumer.

---

## C.13 Failure path (required for every wired system)

**Shape:** catalog missing → dormant; save corrupt → backup/default + visible
notice; player command fails → typed `ActionResult` + UI message; optional
subsystem fails → safe-mode skip.

**Real examples:** economy catalog missing → legacy v1 path; mod invalid →
typed rejection; save failure selftest.

---

## C.14 Handoff evidence (per package)

```text
Package: ORPHAN-SEAL-W<N>-<domain>-<nnn>
Outcome:
Files changed:
Current contract used (authority/save/event):
Reachability before/after (gate output):
Verification commands and results:
Tests reused / added:
Known limitation or debt:
Shared files intentionally untouched:
Ready for sweep: yes/no
```
