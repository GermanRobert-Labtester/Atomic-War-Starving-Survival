# TANNING LEATHER ITEM CROSSWALK — Plan 159 (Workstreams B & C)

> Audit of hide/leather material references in the 30-record corpus against
> the item authority (`Assets/StreamingAssets/Data/items.json`, 660 items).
> Item authority is never modified by this plan. Classifications below are
> proven at registration time by `LeatherworkArchiveSystem` (fail-closed).

## 1. Canonical item identity audit (Workstream B)

Search patterns swept: raw hide, leather, cured hide, harness/strap/belt,
boots/gloves/protective clothing, animal skins, bark/tannin, oils, tanning
chemicals, repair kits.

### 1.1 Proven canonical item links (registered, fail-closed)

| Record | Canonical item | Evidence (both sides) |
|---|---|---|
| `mineral_tan_potassium_alum_white_tawing` | `gas_mask` | Record prose: "tailored into soft inner facepiece liners for long-duration gas masks." Item: full-face respirator with rubber seal. |
| `rawhide_bate_salt_stain_calcium_phosphate_speck` | `item_preservation_salt` | Item description: "Essential for curing meats, brining vegetables, and **curing hides**." Record: rock-salt hide-curing defect. |
| `leather_harness_neatsfoot_oil_cold_stuffing` | `leather_strap` | Item: "a strap cut from a satchel handle or a **bridle**… The leather remembers leather." Record: harness-trace currying. |
| `leather_harness_sulfur_gas_red_rot_powdering` | `leather_strap` | Record prose: "vegetable-tanned mule **harness straps**… snapped during heavy incline hauling." |

Pinned by `LeatherworkArchiveTests.CanonicalLinks_AllResolve_InItemAuthority`
and `DiscoverForItem_FirstInspection_DiscoversLinkedRecords` (viewing
`leather_strap` first-discovers both strap records; viewing `gas_mask`
discovers the alum tawing record; viewing `item_preservation_salt` discovers
the salt-curing record).

### 1.2 Corpus material references — classification

| Corpus reference (records) | Classification | Disposition |
|---|---|---|
| bull/steer hides (#1, #2, #10, #13, #24) | historical material label | no canonical raw-hide item exists; provenance-only. **No item invented** |
| goat skins (#9), sheepskins (#14), hog hair (#19), horse hide (#21) | historical material label | provenance-only; no livestock/hide items |
| sole leather (#1), boot leather (#6, #29), pale saddlery leather (#5) | canonical **category** adjacency | `item_insulated_boots` is felt/rubber, not leather — **not linked**; `leather_strap` covers strap-class goods only |
| split-cowhide (#-adjacent, item text) | canonical item | `mechanic_gloves` ("split-cowhide") — inspected item exists, but no corpus record describes glove currying specifically; **deliberately unlinked** (no proven record↔item identity) |
| chestnut/white/english oak bark, sumac, willow bark (#1–#6) | historical material label | bark/tannin is not a gatherable item; **no harvestable plant created** (Rule 5.7) |
| quebracho extract (#3), hemlock (#7) | historical material label | provenance-only |
| neatsfoot oil, tallow, suet, lard, cod-liver oil, dubbin (#24–#29) | historical material label | `fatliquor_compound_formula` is a batch label, **not a recipe** (Rule 5.6; Plan 55 remains recipe authority) |
| alum, chrome sulfate, zirconium, ferrous sulfate, sodium sulfide/sulfate, dichromate, syntan (#9–#16, #17–#20) | historical chemical label | **never converted to chemical inventory, exposure mechanics or poisons** (Rule 5.3; Workstream J) |
| rock salt cure (#22) | canonical item (linked) | see 1.1 |
| linen thread, beeswax, rosin (#28) | historical material label | provenance-only |
| braided rawhide drive belt (#30) | historical artifact | no belt item exists; provenance-only; **no durability stat derived from 7,800 PSI** (Rule 5.5) |

### 1.3 Item-side inventory (leather goods that exist but have NO proven record link)

`item_snow_goggles_improvised` (scrap leather), `tobacco_pouch`,
`midwife_satchel`, `radio_headset` (cracked leather cups),
`item_pre_war_photo_album` (leather-bound), `prosthetic_wooden_arm`
(leather straps), `engineers_slide_rule` (leather sheath),
`family_apartment_key` (leather fob), `item_collectible_prayer_book`.

These remain viewable without provenance lines — no corpus record describes
their manufacture, and no link is fabricated to enrich them.

## 2. Hide/skin production-loop audit (Workstream C)

**Question:** does any existing system produce hides/skins from hunted
wildlife, livestock, carcass salvage, trade, or scavenged pre-war leather?

| Candidate source | Finding |
|---|---|
| Hunting/wildlife drops | `WildlifeMigrationSystem` and expedition encounters produce **meat/protein** (e.g., `item_smoked_meat` — "surface-trapped animal protein"); no hide/skin/pelt drop was found in `items.json` starting supplies, expedition loot tables, or wildlife outputs |
| Livestock | No livestock husbandry system exists (no animals owned/raised); greenhouse/crop economy is botanical |
| Carcass salvage | No butchery/carcass-processing mechanic exists; abattoir is a deep-lore *location*, not a processing loop |
| Trade | Merchants sell finished goods (masks, boots, straps); no raw-hide trade stock found |
| Scavenged pre-war leather | Canonical finished leather items (`leather_strap`, `mechanic_gloves`, footwear) exist as loot/components — scavenging finished leather is real; tanning *inputs* are not |

**Conclusion (recorded gap, per Plan 159 §9):** no hide-production loop exists
today. Plan 159 therefore remains **discovery/provenance content only** —
item-inspection provenance describes where historical leather *came from*;
it never manufactures hides. A dedicated future plan would be required to
introduce hide economy (out of scope; §5 Non-Negotiable Rule 1).

Negative tests pinning this disposition:
`Firewall_ViewingRecords_MutatesNothingButDiscovery` (viewing records mutates
nothing), plus the inventory-consumption invariance documented in the
completion report.
