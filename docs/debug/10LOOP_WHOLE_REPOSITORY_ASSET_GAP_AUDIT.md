# ASHFALL Whole-Repository Asset and Gap Audit

Date: 2026-09-03
Mode: read-only forensic audit; no gameplay, data, asset, or import-setting repairs performed
Audit anchor: c09c3e67a6e88920690767899b586ab85fecb84c
Starting revision observed: d14b87580ac6e5fe52250f8e1a12ffd90077dafc
Ending revision observed: 4e92c0994e93f21a15e46a815639ed1674a0401a

## Scope and snapshot warning

This audit covered Core, the Godot host, JSON authority, save/load, deterministic execution, player surfaces, event lifecycles, audio, visual assets, asset tooling, Git LFS, and the canonical verification path.

The repository was already dirty when inspection began and changed concurrently during the audit, including a HEAD advance and new combat, crafting, economy, JSON, audio, and host edits. Findings therefore describe the observed working snapshot, not a clean-revision certification. Existing changes were preserved. The SHA above is the exact synthesis anchor.

## Executive verdict

The engine-agnostic Core and persistence architecture are substantially healthy: all 6,498 tests passed at the initial audit snapshot, both builds were clean, save/load gates passed, catalog integrity passed, and no Unity tooling was invoked.

Release confidence is nevertheless blocked by four confirmed failures:

1. The current asset import scan crashes Godot with exit 134, including outside the sandbox.
2. Expedition location catalogs with decimal-form dangerLevel values are rejected by an integer DTO; the file-level catch silently drops about 127 unique expedition/location definitions.
3. Expansion audio listeners are re-added every frame and never removed, producing unbounded duplicate playback callbacks and retaining stale objects.
4. Full visual coverage is only 512 of 1,295 catalog IDs (39.54%), while the gating self-test samples only 50 IDs and remains green.

The dominant systemic risk is false-green validation: several gates prove that a file exists, a type can be constructed, or a small sample resolves, but do not prove that the full asset can be imported, decoded correctly, reached by gameplay, or bound to campaign state.

## Consolidated findings

| ID | Severity | Status | Finding | Evidence |
|---|---|---|---|---|
| F-01 | BLOCKER | CONFIRMED | Godot import crashes on the current snapshot. | godot --headless --path . --import exited 134 both in the sandbox and after approved unrestricted retry; fatal memory/vector/thread errors occurred during the filesystem scan. |
| F-02 | CRITICAL | CONFIRMED | Two location catalogs are discarded by ExpeditionCatalogLoader. | ExpeditionJsonDto.dangerLevel is int at Assets/Ashfall.Core/Expeditions/ExpeditionCatalogLoader.cs:14. locations.json contains 42 decimal values and holdfast_locations.json contains 38; one conversion error is caught around the whole file at line 119. Runtime collection logged both failures. |
| F-03 | CRITICAL | CONFIRMED | Expansion audio subscriptions multiply once per frame. | AudioManager calls RefreshDomainBindings from _Process and calls ExpansionAudioBridge.SubscribeAll at src/Audio/AudioManager.cs:308. SubscribeAll adds four anonymous lambdas; Dispose removes none. |
| F-04 | HIGH | CONFIRMED | The official visual gate is a small-sample gate, not a coverage gate. | AssetRegistrySelfTest.Run defaults to 50 and splits that count across three categories at src/Host/AssetRegistry.cs:549-639. The full sweep explicitly remains report-only at lines 1045-1147. |
| F-05 | HIGH | CONFIRMED | Current catalog visual coverage is 39.54%. | Full report: 1,295 IDs, 512 resolved, 783 missing. Item 46.48%; portrait 42.19%; location 16.18%; faction 77.08%. |
| F-06 | HIGH | CONFIRMED | WAV fallback loading feeds a complete RIFF file into a raw PCM data field. | AudioManager.LoadDirectStream at src/Audio/AudioManager.cs:603 constructs AudioStreamWav and assigns File.ReadAllBytes to Data instead of parsing the WAV container. |
| F-07 | HIGH | CONFIRMED | Three live expansion events resolve to unregistered/silent audio cues. | action_interrogation_slam and hazard_toxic_sizzle have source assets but no catalog registrations; train_screech_crash has neither a registration nor a source. bio_mutation_pulse is the fourth non-hardcoded constant and does resolve through audio_cues.json. |
| F-08 | HIGH | CONFIRMED | New audio mastering is unsafe at source level. | All 52 untracked WAV sources peak at exactly 0.000 dBFS; 41 exceed 0 dBTP. Across all 150 sources, 69 exceed 0 dBTP and 21 have absolute DC offset at or above 0.01. |
| F-09 | HIGH | CONFIRMED | Tracked weather/alert assets contain severe DC offset and source overs. | EMP DC +0.3532; danger klaxon +0.3288; blizzard +0.2999; corrosive +0.2963; surface storm +0.2951; glass +0.2921; chronic radiation +0.2915; black rain +0.2648. |
| F-10 | HIGH | CONFIRMED | Runtime seeds use process-randomized string hashes. | safeId.GetHashCode at src/Host/MaritimeHostSession.cs:254,267,280 and _incidentId.GetHashCode at src/UI/FireIncidentPanel.cs:165 violate cross-process deterministic replay. |
| F-11 | HIGH | CONFIRMED | Fire and faction panels are bound to fresh, disconnected state. | src/Main.PlayerSurfaces.cs:389 creates a new empty ShelterFireHazardSystem; lines 428 and 433 create separate blank FactionStanceEngine instances. |
| F-12 | MEDIUM | CONFIRMED | Fire panel event unsubscription cannot work. | Bind and _ExitTree subtract newly-created lambdas at src/UI/FireIncidentPanel.cs:39 and 220, which cannot equal the originally subscribed lambda. |
| F-13 | MEDIUM | CONFIRMED | One tracked PNG is not a PNG. | assets/sprites/Map/marker_safe.png is ASCII base64 text beginning iVBOR; its Godot import sidecar records valid=false. It has no current production reference. |
| F-14 | MEDIUM | CONFIRMED | Active visuals contain extensive placeholder duplication. | Excluding quarantine: 71 exact-hash groups, 786 files in duplicate groups, 715 redundant copies, about 3.64 MB. Several unrelated catalog IDs resolve to identical flat placeholder art. |
| F-15 | MEDIUM | CONFIRMED | The live assets tree contains a large quarantine corpus. | assets/quarantine holds 1,099 source visuals and about 149.2 MiB, 62.3% of visual-source bytes; no .gdignore or export exclusion was found. |
| F-16 | MEDIUM | CONFIRMED | Content utilization remains unresolved but passes its gate. | Scan: 490 catalogs; 145 gameplay, 279 codex, 26 optional, 0 orphaned, 40 unresolved. The gate fails on new ORPHANED/regressions, not existing UNRESOLVED entries. |
| F-17 | MEDIUM | CONFIRMED | At least ten sizable catalogs have no literal production reference. | Includes three large quest catalogs, codex_entries.json, micro_locations.json, weather_route_gates.json, anomalous_expedition_encounters.json, and bunker_graffiti_postings.json. |
| F-18 | MEDIUM | TOOLING DEFECT | Content utilization reports false NO_LOADER results. | Thirty of its forty NO_LOADER files have direct filename references in source, so the scanner misses real loader evidence while still passing. |
| F-19 | MEDIUM | TOOLING DEFECT | Audio catalog check is date-volatile and omits a data-loaded cue. | generate-audio-catalog.py --check differed only by Last Verified changing from 2026-09-02 to 2026-09-03. It sees 144 Reg calls but not bio_mutation_pulse loaded from JSON. |
| F-20 | MEDIUM | TOOLING DEFECT | Visual audit helpers disagree with production resolution. | visual_wiring_trace.py strips prefixes that AssetRegistry does not strip and reports a much higher stale resolution count. visual_asset_audit.py category matching compares full subpaths with individual Path.parts. |
| F-21 | LOW | LIKELY | Twenty hardcoded cue registrations are statically unreachable. | No source/data references were found outside catalog/self-test code. Dynamic/reflection use can hide reachability, so these require runtime confirmation before removal. |

## Loop evidence

### Loop 1 — Structural and stub inspection

Core contains 622 C# files, the Godot host 493, and tests 548. Main has already been decomposed into partials, so the AGENTS.md description of one 6.5k-line file is stale; the partial family still totals about 18,044 lines and remains a broad composition surface. No empty Core CaptureState/RestoreState implementations were found. Remaining stub markers were intentionally scoped or UI scene placeholders.

### Loop 2 — Reachability and content utilization

The content scan passed with 40 unresolved catalogs. Manual source reconciliation found direct loader references for 30 of those, exposing scanner false negatives. Ten files had no literal production reference; the most consequential are quests_faction_branching.json, quests_massive_expansion_200.json, and quests_moral_branching.json. Their combined size makes silent non-consumption more than cosmetic debt.

### Loop 3 — State transition inspection

ExpansionAudioBridge subscription count grows with frames rather than domain changes. Fire and faction player surfaces construct fresh systems on each bind, so UI actions and readings do not operate on authoritative campaign state. FireIncidentPanel rebinds also accumulate callbacks because anonymous delegate removal is ineffective.

### Loop 4 — Persistence and restore

The campaign-envelope, save-store checksum, legacy bridge, and save/load UI failure gates passed. No empty persistence implementation was found. Persistence is not a primary risk in this snapshot.

### Loop 5 — Determinism

Core did not contain active System.Random or Guid.NewGuid gameplay paths. Four host/UI seeds use string.GetHashCode, whose values are randomized between .NET processes. Replaying the same safe or fire incident across launches can therefore diverge.

### Loop 6 — Data authority and parsing

Catalog integrity reported 0 errors across 10,037 IDs and 208 catalogs, but schema-valid JSON is not necessarily DTO-loadable. ExpeditionCatalogLoader expects an integer dangerLevel even though authoritative location data uses JSON values such as 8.0. Because deserialization covers an entire list within one catch, the first numeric mismatch rejects the complete file.

Theoretical unique definitions across the expedition and location inputs are 263. The observed strict-load path retains about 136 and drops about 127 when locations.json and holdfast_locations.json fail.

### Loop 7 — Events and lifecycle

ExpansionAudioBridge owns no stable delegate references, provider identity, or unsubscribe path. FireIncidentPanel repeats the same anonymous-lambda error. These are silent amplification/leak defects rather than compile failures.

### Loop 8 — Player observability

The fire panel always opens against a new system with incident id inc_default but does not create an incident, so its normal player-visible state is “No active fire incidents.” The two faction views each receive an unrelated blank engine, preventing them from truthfully reflecting campaign standings.

### Loop 9 — Tests, gates, and false greens

The five canonical checks passed at the initial audit snapshot, as did asset registry, asset sidecar, LFS, scene binding, accessibility, expansions, playable shell, day-one, seven-day, save checksum, and save/load UI gates. However:

- the asset gate checks only the top 50 references;
- audio presence accepts File.Exists without decode;
- audio loop testing accepts the defective direct WAV fallback by runtime type;
- content utilization tolerates unresolved catalogs;
- asset orphan checking validates source/sidecar pairs, not payload validity;
- full asset coverage is report-only.

### Loop 10 — Cross-system synthesis

The same failure mode crosses data, audio, visuals, and UI: inventories and smoke tests are green while integration semantics are not. Location files exist but fail DTO conversion; audio files exist but can use an invalid fallback loader; visuals exist but are placeholders or cover only 39.54% of IDs; panels exist but bind to fresh state. Release gating should move from existence and samples toward full-load, full-resolution, live-state, and lifecycle assertions.

## Asset inventories

### Visual

| Metric | Result |
|---|---:|
| Visual source files | 3,757 |
| PNG / JPG / SVG | 1,083 / 2,627 / 47 |
| Total source bytes | 250,886,103 |
| Active files excluding quarantine | 2,658 |
| Active bytes excluding quarantine | 94,484,710 |
| Quarantine source files | 1,099 |
| Quarantine bytes | 156,401,393 |
| Exact duplicate groups, all assets | 131 |
| Redundant files, all assets | 1,498 |
| Exact duplicate groups, active only | 71 |
| Redundant files, active only | 715 |
| Confirmed corrupt raster payloads | 1 |

The audit tool reported 48 load errors; 47 were SVG/Pillow limitations, not corrupt assets. marker_safe.png was the sole confirmed corrupt raster.

### Audio

| Metric | Result |
|---|---:|
| Decodable source files | 150 |
| WAV / MP3 / OGG | 82 / 63 / 5 |
| Total bytes | 16,499,025 |
| Total duration | 501.532 seconds |
| Mono / stereo | 94 / 56 |
| Runtime cue count | 145 |
| Catalog paths missing on disk | 0 |
| Source files not cataloged | 6 |
| Decode failures in independent scan | 0 |
| Exact binary duplicate groups | 0 |
| Sources over 0 dBTP | 69 |
| Sources at or over -0.1 dBFS sample peak | 81 |
| Sources with absolute DC at least 0.01 | 21 |

The six uncataloged sources are vo_kind_hatch.wav, vo_kind_parley.wav, radiation_alert.wav, weather_alert.wav, sfx_hazard_toxic_sizzle.mp3, and sfx_interrogation_slam.mp3. The first four appear superseded; the last two correspond to live but currently silent expansion events.

## Verification matrix

| Check | Result |
|---|---|
| dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj | PASS — 0 warnings, 0 errors |
| dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-build | PASS — 6,498 passed |
| final full-suite rerun during concurrent edits | PASS — 6,580 passed |
| dotnet build Ashfall.csproj | PASS — 0 warnings, 0 errors |
| godot --headless --path . -- --data-integrity-selftest | PASS — 0 findings |
| godot --headless --path . -- --bridge-selftest | PASS |
| legacy asset path gate | PASS |
| LFS health check | PASS — 3,747 LFS files, fsck clean |
| asset source/sidecar orphan sweep | PASS — 0 pair orphans |
| asset registry self-test | PASS — sampled 50/50 |
| full asset coverage report | REPORT — 512/1,295 resolved |
| content utilization self-test | PASS — 40 unresolved |
| audio self-test | PASS — 490/490, with 22 unique ResourceLoader cache failures hidden by fallback |
| godot --headless --path . --import | FAIL — exit 134 |

Because the repository changed during execution, the canonical five checks must be rerun against a stable chosen revision before release certification.

One intermediate full-suite run observed 1 failure among 6,547 tests in Plan45EnemyCompositionTests.Encounter_HighDangerAmbushIsFightableAndResolvable. The same test passed in isolation and the next full run passed all 6,580 tests. Because test sources and data were being added concurrently, this is recorded as a transient observation rather than a reproducible defect.

## Remediation order

1. Stabilize a clean snapshot and make godot --import complete successfully.
2. Change ExpeditionCatalogLoader numeric handling and add file-level load-count tests for all five inputs.
3. Make expansion and fire event subscriptions idempotent and disposable; add repeated-refresh/rebind tests.
4. Repair WAV fallback parsing and require actual ResourceLoader/direct-decoder success in audio tests.
5. Master the new 52 WAVs with headroom and DC removal; re-audit tracked alert/weather files.
6. Turn full visual coverage into a thresholded gate and validate file signatures/importability.
7. Replace string.GetHashCode seeds with stable ordinal hashing.
8. Bind fire and faction panels to campaign-owned systems.
9. Move quarantine out of the live import tree or exclude it explicitly after confirming intended retention.
10. Reconcile scanner/tool behavior with production loaders before using generated coverage numbers as authority.

No deletion, normalization, migration, or source repair was performed by this audit.
