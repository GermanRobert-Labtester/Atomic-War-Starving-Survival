# Broadcast State Provenance & Information Policy

> **Document Status:** Authoritative Information Boundaries
> **Authority:** Plan 24 (Task 24J, 24AL, 24AR)

---

## 1. Information Tier Architecture

Radio in ASHFALL is an imperfect, diegetic communication channel. Broadcasts reflect what the *speaker* knows, believes, or wants listeners to believe. Under no circumstances should raw broadcast strings serve as authoritative campaign truth flags without corroboration.

```text
[ Authoritative World State ] (Core Simulation / Systems)
          |
          +---> Public Events (Visible across wasteland)
          |         |
          |         +---> Civilian / Public Broadcasts (Accurate within horizon)
          |         +---> Faction Propaganda (Spun / exaggerated / sanitized)
          |
          +---> Faction Private State (Unit movements, armory shortages)
          |         |
          |         +---> Faction Tactical Radio (Honest within own network)
          |         +---> Intercepted Wiretaps (High intel value)
          |
          +---> Shelter Private State (Internal food, sick survivors, secret choices)
                    |
                    +---X [ FORBIDDEN: External radio cannot know internal shelter state ]
```

---

## 2. Knowledge Boundary Rules

1. **No Accidental Omniscience:** External broadcasters (e.g. Iron Garrison, Hydro-Barons, Civil Defense) cannot reference private events occurring inside the player's shelter unless the player dispatched a courier, broadcasted a beacon, or interacted with an emissary.
2. **Propaganda vs Reality:** Faction broadcasts claiming "Zero casualties sustained" or "Enemy completely routed" represent partisan morale spin. The true outcome must be verified on the map or via physical salvage.
3. **Evidence Authentication:** In Verdict tribunal gameplay (Expansion 08), radio recordings must possess verifiable provenance (e.g. verified frequency timestamp, recognized officer voice, or official machine-register certificate) to serve as legal evidence.
4. **Contradictory Reporting:** When two rival factions clash in Sector 4, both will broadcast conflicting battle reports on their respective frequencies (`88.4 MHz` vs `104.2 MHz`). The player who monitors both frequencies learns the location of the engagement and the political bias of each side.
