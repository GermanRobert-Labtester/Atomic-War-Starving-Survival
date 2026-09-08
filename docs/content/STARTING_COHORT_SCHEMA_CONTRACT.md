# Starting Cohort Schema Contract

## Authority

`Assets/StreamingAssets/Data/starting_survivors.json` remains the legacy
default authority. Alternate profiles live in
`starting_survivor_cohorts.json`.

```json
{
  "schema_version": 1,
  "default_profile_id": "cohort_standard_holdfast",
  "profiles": [
    {
      "profile_id": "cohort_standard_holdfast",
      "display_name": "Standard Holdfast",
      "description": "The unchanged legacy opening.",
      "members": [
        {
          "id": "survivor_dr_sarah_chen",
          "health": 90,
          "hunger": 20,
          "thirst": 25,
          "warmth": 85,
          "morale": 70,
          "lifetimeDose": 14,
          "acuteRad": false,
          "joinedDay": 0
        }
      ]
    }
  ]
}
```

The member shape is the existing `StartingSurvivorDefinition` shape. Profiles
contain no items, skills, traits, research, faction standing, bonuses, or
quest effects.

## Validation

- profile IDs are non-empty and unique;
- the explicit default exists;
- each profile has exactly three ordered members;
- member IDs are unique within a profile;
- every member resolves to `survivors.json`;
- members with an active survivor questline are rejected;
- initial values are finite and within existing supported ranges;
- Standard Holdfast must match the legacy file exactly;
- invalid alternates are skipped without replacing the valid legacy default.

Profile and member order is authored order and is stable. No random selection
or preview RNG is permitted.
