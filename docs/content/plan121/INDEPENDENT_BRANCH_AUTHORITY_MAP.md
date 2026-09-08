# Independent Branch Authority Map

| Fact / Subsystem | Canonical Authority | Plan 121 Role |
|---|---|---|
| Branch Definitions | `independent_faction_branch.json` | Expand from 8 to 15 branches |
| Branch ID & PoNR Whitelist | `IndependentBranchIds.cs` | Register 7 new branches, flags, endings |
| Moral Band Values & Ordering | `MoralChoiceSystem.cs` (`MoralPathBand`) | Reference only |
| Player Morality Score & Band | `MoralChoiceSystem` runtime state | Query only |
| Branch Commitment Lifecycle | `IndependentBranchSystem.CommitBranch` | Consume definitions & soft gates |
| PoNR Lock & Flag Ledger | `IndependentBranchSystem.LockPointOfNoReturn` | Set durable flag & trigger event |
| Ending Resolution | `IndependentBranchSystem.ResolveEnding` | Provide complete 7-band partitions |
| UI Aggregation & Selection | `FactionBranchCoordinator.GetBranchOptions` | Enumerate all 31 faction branches |
| Campaign Save / Persistence | `IndependentBranchSaveCodec` | Checksummed round-trip verification |
| Catalog Integrity & Linting | `CatalogIntegrityValidator.cs` | Validate prefix rules (`branch_`, `flag_`, `ending_`) |
| Asset Registry | `scripts/ci/generate-asset-registry.py` | Sync manifest |
