# Wave 30 — Summary (Plans 211–215)

## Wave Overview

Five non-duplicative, implementation-ready plans covering internal communication, temporal legacy, informal economics, visitor management, and resource crisis response. This wave focuses on **shelter as community** — the systems that make the shelter function as a living social organism with information flow, economic agency, guest management, and crisis response.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 211 — Internal Communication Network | Bulletin boards, intercom announcements, internal mail, notice systems for information flow between survivors and from leadership. | Plan 157 covers external radio infrastructure. Plan 131/203 cover intelligence/rumor networks. But NO internal shelter communication — no bulletin boards, no intercom, no internal mail, no leadership broadcasts. Verified: ZERO matches for `CommunicationNetwork`, `BulletinBoard`, `InternalMail` in Core. | LOW | DutyRosterSystem, SurvivorRelationsSystem, ShelterScheduleSystem, LeadershipSystem |
| 212 — Time Capsule & Legacy Messages | Survivors create messages/packages for future discovery — cross-generational communication within the campaign, with date/survivor/event-based opening conditions. | Plan 140 covers cross-campaign meta-progression. Plan 206 covers inheritance on death. Plan 162 covers shelter archive. But NO time capsules, no legacy messages, no "message in a bottle" mechanics. Verified: ZERO matches for `TimeCapsule`, `LegacyMessage`, `MessageToFuture` in Core. | LOW | MemorialSystem, SurvivorFateSystem, ISimClock, EventSystem |
| 213 — Survivor Barter & Informal Economy | Survivor-to-survivor trading of items, favors, and services with informal pricing, negotiation, trade reputation, and dispute resolution. | `MarketSystem` handles external trade. Plan 155 covers black market. Plan 192 covers trade routes. But NO internal survivor-to-survivor barter, no informal economy, no personal negotiation, no favor-trading. Verified: ZERO matches for `SurvivorBarter`, `InformalEconomy`, `InternalTrade` in Core. | LOW | MarketSystem, Inventory, SurvivorRelationsSystem, PersonalBelongingsSystem |
| 214 — Visitor Integration & Housing | Processing pipeline for admitted visitors — temporary housing, integration tasks, departure tracking, monitoring of suspicious visitors. | `AirlockSecuritySystem` (227 lines) handles visitor arrival decisions. Plan 138 mentions "refugee integration" but doesn't implement. But NO visitor processing, no housing assignment, no integration period, no departure tracking. Verified: ZERO matches for `VisitorIntegration`, `VisitorHousing`, `VisitorProcessing` in Core. | LOW | AirlockSecuritySystem, SurvivorCatalog, ShelterScheduleSystem, NeedsSystem |
| 215 — Shelter Resource Rationing & Crisis Management | Rationing protocols with priority groups, crisis declaration/response, resource allocation under scarcity, rationing enforcement and violations. | `DutyRosterSystem` has single `mutationRationProtocol` boolean. Plan 158 covers acute disasters. But NO comprehensive rationing system, no priority groups, no crisis protocols, no rationing tiers. Verified: ZERO matches for `RationingSystem`, `CrisisProtocol`, `PriorityGroup` in Core. | LOW | DutyRosterSystem, NeedsSystem, Inventory, KitchenNutritionSystem, WaterTreatmentSystem |

## Strongest Plan to Implement First

**Plan 215 — Shelter Resource Rationing & Crisis Management.** It addresses the most fundamental survival challenge — what happens when resources run low. Rationing creates meaningful strategic decisions (who gets what when there's not enough), adds realism (crises require management), and integrates with every resource system in the game. It's the plan that makes scarcity a gameplay layer rather than just a death timer.

## Dependencies Between the 5 Plans

- **Plan 211 (Communication) is standalone** — adds information flow layer.
- **Plan 212 (Time Capsules) is standalone** — adds temporal depth.
- **Plan 213 (Barter) integrates with Plan 211** — trade notices on bulletin boards.
- **Plan 214 (Visitors) is standalone** — adds visitor management layer.
- **Plan 215 (Rationing) integrates with Plan 211** — rationing announcements via intercom.

## Recommended Implementation Order

1. **Plan 215** — Resource Rationing & Crisis Management (fundamental survival, broadest integration)
2. **Plan 214** — Visitor Integration & Housing (operational depth, extends airlock security)
3. **Plan 211** — Internal Communication Network (community building, enables other plans)
4. **Plan 213** — Survivor Barter & Informal Economy (economic agency, uses communication)
5. **Plan 212** — Time Capsule & Legacy Messages (emotional depth, standalone)

## Rejected Candidates (Considered but Not Selected)

- **Recycling & Resource Recovery** — Plan 201 (Sanitation & Waste Management) covers waste processing and water recycling. Too much overlap.
- **Survivor Art & Cultural Expression** — Plan 178 (Art & Culture Creation System) covers creative expression. Not a gap.
- **Bunk Assignment & Sleeping Arrangements** — `ShelterScheduleSystem` (240 lines) handles bed assignments. Too thin for standalone plan.
- **Emergency Protocol & Evacuation** — Plan 158 (Disaster & Emergency Response) covers emergency protocols. Not a gap.
- **Library & Knowledge Management** — `LibraryStudySystem` (206 lines) exists as study mechanic. Not a clean gap.
- **Shelter Cleanliness & Hygiene** — Plan 201 (Sanitation) covers hygiene management. Too much overlap.
- **Shelter Census & Population Tracking** — `CensusClaimSystem` and `VoluntaryRegisterSystem` both exist. Not a gap.

## Post-Recon Corrections

Both recon agents validated findings:

- **Plan 211 (Communication)**: confirmed ZERO internal communication systems. Only string constant `KindIntercomOffice` in `ShelterEncounterSystem`. Plan 157 covers external radio, not internal comms.
- **Plan 212 (Time Capsules)**: confirmed ZERO time capsule systems. No legacy message mechanics exist.
- **Plan 213 (Barter)**: confirmed ZERO survivor-to-survivor trading. `MarketSystem` handles external trade only.
- **Plan 214 (Visitors)**: confirmed `AirlockSecuritySystem` handles arrival decisions only. No processing pipeline, no housing, no integration.
- **Plan 215 (Rationing)**: confirmed `DutyRosterSystem` has single boolean toggle only. No comprehensive rationing system.

## Why This Wave Materially Expands ASHFALL

These five plans transform the shelter from a resource-consumption machine into a functioning community. Internal communication lets survivors share information and leadership broadcast announcements. Time capsules add temporal depth — survivors leave messages for the future. Survivor barter creates an informal economy where individuals trade, negotiate, and build trust. Visitor integration makes the shelter a place that receives, processes, and houses guests. Resource rationing adds strategic crisis management when supplies run low. Together, these plans make the shelter feel like a real community — with information flow, economic agency, guest hospitality, and crisis response.

## Cumulative Wave Themes (Waves 17–30)

| Wave | Theme | Plans |
| ---- | ----- | ----- |
| 17 | Radiation, memory, friction, achievements, romance | 146–150 |
| 18 | Animals, vehicles, espionage, education, black market | 151–155 |
| 19 | Shelter, communications, disasters, governance, colonies | 156–160 |
| 20 | Hobbies, archive, cartography, nuclear winter, modding | 161–165 |
| 21 | Identity, tunnels, propaganda, audio, celebrations | 166–170 |
| 22 | Dynamic quests, mutations, radio, backstories, meta-progression | 171–175 |
| 23 | Aging, dreams, art, psychology, certifications | 176–180 |
| 24 | Difficulty, relationship decay, child development, accessibility, memory decay | 181–185 |
| 25 | Shelter maintenance, bestiary, survivor routines, water sources, item lore | 186–190 |
| 26 | Item identification, trade routes, chronic conditions, emergency alerts, survivor roles | 191–195 |
| 27 | Food types, diplomacy, health records, seasonal migration, personal quests | 196–200 |
| 28 | Sanitation, interpersonal conflict, intelligence, recruitment, noise discipline | 201–205 |
| 29 | Death/inheritance, shelter reputation, leadership succession, security, personal belongings | 206–210 |
| **30** | **Communication, time capsules, barter, visitor integration, resource rationing** | **211–215** |

**Total: 85 plans across 17 waves (131–215), plus 17 wave summaries.**

## Milestone Note

Wave 30 reaches Plan 215 — 85 plans in 17 waves since Plan 131. The planning has now covered: basic systems, content/narrative, integration/depth, societal complexity, individual identity, and community operations. Each wave builds on the last, creating an increasingly detailed vision of ASHFALL as a game where the shelter is a living community with communication, economy, guests, and crisis management.
