# Bone, Horn & Antler Producer Map

Discovery uses the existing Journal discovery seam. The primary producer is the manifest producer_id; producer_ids contains only explicit alternate contexts. Opening a surface calls the existing discovery method and does not change simulation state.

| Producer context | Route | Records exposed | Meaning |
|---|---|---:|---|
| room_workshop | Main.OpenCraftingPanel workshop route | 28 | general low-resource craft archive; no production action |
| room_workshop_heavy | Main.OpenCraftingPanel heavy-workshop route | 11 | saw/large-material context; no tool requirement |
| room_workshop_precision | Main.OpenCraftingPanel precision route | 14 | finishing and assay context; no quality modifier |
| loc_automated_abattoir | Main.OpenMapDetailPanel location inspection | 15 | authored bone-stock history at a proven location; no carcass harvest |
| loc_forestry_compound | Main.OpenMapDetailPanel location inspection | 14 | saw and material-work context at a proven maintenance site |
| loc_st_brigids_almshouse | Main.OpenMapDetailPanel location inspection | 6 | historical needle/awl context near a proven medical-history location; no medical use |
| government_bunker | archive desk route | 2 | shelter archive fallback; no global unlock |

The seven contexts above are all backed by existing room or location IDs. room_foundry is queried by the crafting surface for consistency with the shared route, but the current Plan 160 manifest assigns it no records. No new scavenging, hunting, butchery, carcass or expedition producer was introduced. Existing location inspection is a discovery context only.

Every activated record is also queryable by its stable discovery ID and is rendered in JournalCodex Events after discovery. The codex shows family, historical identity status, authored measurement label and facility/material label. It does not render a current inventory, animal or equipment state from the text.
