# Plan 138 Save Compatibility

Existing saves do not need migration because profile definitions are catalog
input only. The actual survivor roster, needs, radiation, death state, and
other section state remain in the normal campaign envelope.

The selected profile ID is transient initialization input and is not required
for restore. A restored campaign never reconstructs its roster from the
current profile catalog. Missing profile metadata in old saves is therefore
harmless and cannot overwrite actual survivor state.

New Game allocates a fresh slot instead of deleting `slot_1`. Old campaign
envelopes, manifests, projections, and section files remain available for
explicit load. A failed restore preserves the live session and cannot enter
fresh initialization.
