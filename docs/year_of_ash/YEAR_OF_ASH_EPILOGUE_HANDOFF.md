# Year of Ash Epilogue Handoff

Quest outcomes are already persisted in `YearOfAshSave.quests` and can be queried by downstream
systems that have a supported history contract. No direct Plan 89 field or duplicate ending flag was
added. A future epilogue integration should consume the canonical questline ID/status/history rather
than infer an ending from prose or a UI event.
