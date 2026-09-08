# Plan 126 Content Utilization

`crossing_items.json` remains a gameplay-consumed catalog through `CrossingCatalog` and is also registered by the global `ItemCatalogLoader`. The content-utilization selftest passes with zero orphaned catalogs.

The current scanner reports catalog consumption, not per-item acquisition reachability. Because the live Crossing faction schema uses macro tags and no active encounter resolver was found for `cost_items`, item-level sources/sinks for the fourteen additions are staged rather than fabricated.

This distinction is deliberate:

```text
catalog load != item acquisition/use coverage
```

The source/sink gap is recorded in `CROSSING_ITEM_SOURCE_SINK_MATRIX.md` and should be closed by a future plan that lands a real faction, encounter, quest, or loot consumer.
