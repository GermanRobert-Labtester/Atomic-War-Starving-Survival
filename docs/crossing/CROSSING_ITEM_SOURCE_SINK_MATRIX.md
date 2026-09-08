# Plan 126 Source/Sink Matrix

The canonical definition source for all fourteen additions is `crossing_items.json`, and all fourteen are registered by the global `ItemCatalogLoader`. The current repository does not expose a Crossing item-source registry that maps these new IDs to acquisition flows.

| Item family | Definition source | Live item-level acquisition path found | Live sink/use found | Status |
| --- | --- | --- | --- | --- |
| Bread / Water | `crossing_items.json` | generic inventory catalog only | generic Food/Water use | available to generic systems; no Crossing-specific source authored |
| Medicine | `crossing_items.json` | generic inventory catalog only | generic Medical use via `healthEffect` | available to generic systems; no black-market source authored |
| Fuel | `crossing_items.json` | generic inventory catalog only | generic Fuel classification | no lamp-specific consumer found |
| Tokens / bands / receipts | `crossing_items.json` | no exact-ID faction source; factions use macro tags | local catalog and possible encounter cost field | staged pending a live resolver |
| Ledger / notices / map / pouch / draft | `crossing_items.json` | no exact-ID source in current Crossing catalogs | local catalog only | staged pending quest/encounter/loot consumers |

This is an intentional architecture boundary. Adding fake sources or unsupported grants would make the catalog appear utilized while leaving the runtime without a real way to acquire or consume the objects.

No acquisition deadlock was introduced because no new item was made a prerequisite for existing content.
