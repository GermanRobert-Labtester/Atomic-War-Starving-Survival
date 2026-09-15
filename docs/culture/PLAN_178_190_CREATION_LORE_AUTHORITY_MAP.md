# Plans 178 / 190 — Culture creation + item lore authority map

**Status:** ACCEPTED — §3 signed 2026-09-12  
**Package:** `DEBT-170-199-REMAINING-FAMILY-MAPS`  
**Rebase:** 178 culture creation; 190 item lore  
**Date:** 2026-09-12

**Stale historical prose:** `Plan_178_Art_Culture_Creation_System.md`, `Plan_190_Item_Lore_Provenance_Tracking.md`. Live `Main.Plans178_181` is child development (**number drift**).

---

## 1. Premise

| Concern | Owner |
|---|---|
| Immutable archive tomes + vault (degrade, transcribe, salon) | `CulturalArchiveVaultSystem` — save `cultural_archives` — `cultural_archive_tomes.json` |
| Player-cut recordings / chronicle rows | Same vault (`ArchiveRecordingState`, `ArchiveChronicleEntry`) |
| Scribing / inks | `ArchiveDeskSystem` — `archive_desk` |
| Oral lore first-heard IDs | `OralLorePerformanceSystem` — `oral_lore` |
| Hobbies / optional output item | `SurvivorDowntimeSystem` — `recreation` |
| Decor placement | `ShelterDecorSystem` |
| Inventory mutation DTO | `InventoryProvenanceRecord` — **not a persisted history** |
| Runtime instance id | `ProceduralItemInstance.InstanceId` — condition/calories; **no lore chain** |
| Static relic dossiers | `RelicProvenanceCatalog` — **tests only**, no host load |
| Discovery / unique claims | `CollectibleDiscoveryState`, `UniqueItemClaimRegistry` |
| `ArtCreationSystem` / `ItemLoreSystem` | **ABSENT** |

---

## 2. Ownership (proposed)

Keep three identities separate:

1. **Immutable catalog lore** — tomes, relic dossiers, item descriptions  
2. **Instance id** — `ProceduralItemInstance.InstanceId`  
3. **Discovery/claim ledgers** — collectible / unique-claim IDs  

Creation may only append vault recordings/chronicles or hobby `output_item_id` into those owners.

---

## 3. Recommended defaults

| Item | Default |
|---|---|
| `ArtCreationSystem` / `ItemLoreSystem` / `art_templates.json` / `lore_templates.json` | **OUT** |
| Treating `InventoryProvenance` as heirloom history | **OUT** |
| Promoting relic dossiers to runtime instance state without a host load package | **OUT** |
| Second archive save beside `cultural_archives` | **OUT** |
| Creation → existing vault/hobby/decor interface | **IN** (implement later) |
| Lore attached only to instance id or unique-claim id, never stacked `itemId` | **IN** |

**Honesty:** a tome in the vault is not player-made art. Provenance DTO is not a biography.

**Next implement (separate claim):** `DEBT-178-CREATION-TO-VAULT` — one creation command that appends a chronicle/recording on `CulturalArchiveVaultSystem`. 190 stays mapped until instance-lore is separately signed.

---

## 4. Exact paths (read-only evidence)

`Culture/CulturalArchiveVaultSystem.cs`, `cultural_archive_tomes.json`, `Inventory/InventoryProvenance.cs`, `Inventory/ProceduralItemInstance.cs`, `Narrative/RelicProvenanceCatalog.cs`, `CollectibleDiscoveryState.cs`, `Recreation/SurvivorDowntimeSystem.cs`

**Signed 2026-09-12.** Implement packages listed in this map stay unclaimed until separately promoted.
