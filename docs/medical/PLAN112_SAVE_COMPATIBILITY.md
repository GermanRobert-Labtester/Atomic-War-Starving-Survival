# Plan 112 save compatibility

No save schema or checksum algorithm changed.

## Existing save path

`DiseaseSystem.CaptureState()` persists the disease rows, patient state,
immunity records, vector protocol flags, outbreak counters, and RNG seed inside
the existing checksummed expansion/campaign save envelope. The catalog remains
authoritative data, not mutable save data.

## Forward compatibility

- A fresh catalog binds 20 rows.
- A pre-Plan-112 state with 16 disease rows restores normally.
- `BindCatalog` then idempotently appends missing simulation rows for the four
  new definitions.
- Existing patient rows, counters, protocol flags, immunity records, and RNG
  position are not reordered or rewritten.
- A save written after the expansion includes all 20 rows and is still checked
  by the existing checksum envelope.

The append-only catalog order is pinned by
`DiseaseCatalogExpansionTests.Catalog_ReconcilesToTwenty_AndPreservesTheLiveSixteenPrefix`.
Checksum, migration, and RNG continuation coverage remains in
`DiseaseSystemTests`.
