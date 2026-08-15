using Xunit;

// The suite shares process-level state across classes (catalog registries,
// culture, seeded rngs in WIP systems). Parallel class execution produced
// intermittent cross-class failures under load; serialize the assembly.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
