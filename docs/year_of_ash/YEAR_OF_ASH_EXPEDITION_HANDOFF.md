# Year of Ash Expedition Handoff

`unlockEncounterId` is a supported choice field and the new data uses existing door-encounter IDs for
pilgrimage, hydro, black-ops, garrison, and Rebuilder pressure. However, the current
`Main.YearOfAsh` choice path does not consume `result.unlockedEncounterId` into the expedition system.

This is therefore a valid staged data handoff, not a claim of live expedition unlock wiring. No new
destination registry or expedition state was added. Future wiring should route through the canonical
encounter/expedition authority and preserve the existing IDs.
