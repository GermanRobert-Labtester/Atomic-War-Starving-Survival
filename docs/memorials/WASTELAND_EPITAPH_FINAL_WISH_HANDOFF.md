# Wasteland Grave Epitaphs — Final Wish System Handoff

**Originating Plan:** Plan 65 (`FinalWishSystem`)
**Core Authority:** `Assets/Ashfall.Core/FinalWishSystem.cs`
**Ledger Integration:** `MemorialSystem.MemorialEntry.FinalWishResolved`

---

## 1. System Boundary

1. **Survivor-Specific vs Environmental:** Plan 65 manages specific dying wishes for named camp survivors (e.g., requesting a keepsake, seeing a specific friend, asking for a certain burial spot).
2. **Epitaph Separation:** The lines in `wasteland_grave_epitaphs.json` are general environmental marks for graves found in the wastes. They do not overwrite or replace survivor-specific final wishes.
3. **Memorial Metadata:** `MemorialEntry` records `bool FinalWishResolved`. When a shelter dweller dies, their memorial entry preserves wish completion status independently of the assigned generic or custom epitaph.
