# Keepsake Ownership & Recognition Contract

## 1. Core Principle: Association vs. Possession
A personal keepsake in ASHFALL is an **emotional association**, NOT a physical item injection into a survivor's pocket or inventory.

1. **Association (Static/Catalog):** Authored in `expansion_survivor_fields.json` via `personal_keepsake_item_id`. Represents the memory or heirloom precious to that individual.
2. **Possession (Dynamic/Runtime):** Tracked through the settlement's canonical `Inventory`. The item exists in the world only if scavenged, crafted, or brought on an expedition.
3. **Recognition (Idempotent Event):** Triggered when the physical item arrives at the holdfast while the associated survivor is alive in the roster.

---

## 2. Invariant Guarantees

| Invariant | Guarantee | Enforcement Mechanism |
|---|---|---|
| **Zero Auto-Spawning** | Starting a campaign never auto-populates survivor inventories with high-value keepsakes. | Verified by unit test `Keepsake_Association_Does_Not_Imply_Inventory_Possession`. |
| **No Exclusive Lockout** | Any survivor can carry or use a keepsake item if it has physical tool properties (e.g. `worn_stethoscope`, `pipe_wrench`). | Core `Inventory` remains generic without per-character item exclusivity locks. |
| **Idempotent Discovery** | Keepsake arrival logs to the journal exactly once per survivor-item pair. | Gated by `_journal.Knowledge.Discover($"keepsake_recognized:{sid}:{itemId}")`. |
| **Clean Fallbacks** | Keepsakes without physical item records (narrative mementos) do not cause null references in inventory. | `ItemInspectionModel` validates item existence before attempting inspection. |

---

## 3. Physical Keepsake Catalog Items
The following 8 keepsake IDs represent tangible items in `StreamingAssets/Data/items/`:

| Item ID | Associated Survivor | Physical Category | Inspection Suitability |
|---|---|---|---|
| `worn_stethoscope` | `elena_vasquez` | Medical / Diagnostic | `personal_keepsake_candidate` |
| `tarnished_pocket_watch` | `marcus_olejnik` | Pre-War Relic | `personal_keepsake_candidate` |
| `family_heirloom_seeds` | `vera_mikhailov` | Agricultural / Botanic | `personal_keepsake_candidate` |
| `silver_scalpel` | `aris_thorne` | Surgical Instrument | `personal_keepsake_candidate` |
| `childs_drawing` | `tatyana_voronova` | Paper Memento | `personal_keepsake_candidate` |
| `teddy_bear` | `lydia_karpova` | Toy / Comfort Object | `personal_keepsake_candidate` |
| `wedding_ring` | `nikolai_fedorov` | Precious Jewelry | `personal_keepsake_candidate` |
| `pipe_wrench` | `dmitri_volkov` | Maintenance Tool | `personal_keepsake_candidate` |

The remaining 64 keepsakes represent narrative heirlooms (letters, faded medals, religious icons, pressed dried flowers) tracked in survivor memory and lore logs.

---

## 4. Lifecycle Sequence

1. **Roster Enrollment:** Survivor enters settlement. `SurvivorEnrichmentService` exposes `PersonalKeepsakeItemId` for UI display.
2. **Scavenge / Discovery:** An expedition returns with `worn_stethoscope`. Item enters Holdfast `Inventory`.
3. **Recognition Check (`Main.Enrichment.cs`):**
   ```csharp
   public void CheckKeepsakeRecognition(string itemId)
   {
       if (_survivors == null || _journal == null || _enrichment == null) return;
       foreach (var survivor in _survivors.Roster)
       {
           string keepsakeId = _enrichment.GetKeepsakeItemId(survivor.Id);
           if (string.Equals(keepsakeId, itemId, StringComparison.Ordinal))
           {
               string discoveryKey = $"keepsake_recognized:{survivor.Id}:{itemId}";
               if (_journal.Knowledge.Discover(discoveryKey))
               {
                   _journal.TryAddRawEntry(new JournalEntry(
                       day: _clock?.CurrentDay ?? 1,
                       text: $"{survivor.DisplayName} recognized the {itemId.Replace('_', ' ')} from their past.",
                       category: JournalCategory.Personal));
               }
           }
       }
   }
   ```
4. **Save Persistence:** The discovery key is preserved in `JournalSaveStore` within the canonical save envelope.
