# Moral Flag Definition Authority

`MoralChoiceFlagDefinitions.cs` is handwritten DTO code, not generated output and not a whitelist. It contains `MoralChoiceFlagDefinitions.Flags` and `MoralFlagDefinition` (`Id`, `DisplayName`) only.

The JSON catalog is the authored vocabulary. `MoralChoiceIds` is the existing compile-time ID surface used by tests and branch code; Plan 125 adds the 15 new constants there and expands `AllFlags` from 11 to 26. The 26th entry is the existing external `flag_moral_messenger_kept` marker, which is intentionally not a catalog record.

No generator exists for this definition DTO. No generated file was edited. Parity is enforced by the Plan 125 tests: every catalog ID is present in `MoralChoiceIds.AllFlags`, and the catalog has exactly 25 unique records.
