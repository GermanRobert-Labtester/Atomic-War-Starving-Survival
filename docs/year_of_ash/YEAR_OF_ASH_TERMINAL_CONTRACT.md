# Year of Ash Terminal Contract

Terminal status is represented by `isTerminal` plus the existing `terminalOutcome` enum. The live
JSON enum values are `2` for Completed and `3` for Failed. A choice transitions to a terminal stage;
the quest system then records the resolved status. Terminal stages authored by Plan 114 have no
choices and do not point onward.

No new outcome vocabulary, terminal flag, or terminal save field was introduced.
