# Expedition Stat Derivation Standards

## 1. Tick-to-Hour Conversion

In ASHFALL, simulation and travel time are standardized on half-hour travel increments:

$$\text{distanceTicks} = \max(1, \text{round}(\text{travelHours} \times 2))$$

### Verification against Canonical Records:
- `loc_the_allotments`: $\text{travelHours} = 2.5 \implies \text{round}(2.5 \times 2) = 5\text{ ticks}$. (Exact match)
- `loc_denial_cut_substation`: $\text{travelHours} = 4.0 \implies \text{round}(4.0 \times 2) = 8\text{ ticks}$. (Exact match)
- `suburban_house`: $\text{travelHours} = 1.0 \implies 2\text{ ticks}$.
- `location_the_dead_hand_core`: $\text{travelHours} = 9.0 \implies 18\text{ ticks}$.

## 2. Danger Level Mapping

`dangerLevel` is projected directly from the authoritative `locations.json` integer value ($1..10$).
- Danger 1–3: Scavenge Tier (Common threats, wild vermin, localized decay).
- Danger 4–5: Standard Tier (Raider scouts, wild beasts, light automated sentries).
- Danger 6–7: Hazardous Tier (Organized bands, irradiated beasts, heavy traps).
- Danger 8–10: Deep Tier (Autonomous defense complexes, lethal mutant predators, extreme fallout).

## 3. Encounter Chance Derivation

Encounter probability per travel tick is derived to scale smoothly without encounter spamming:

$$\text{encounterChancePerTick} = \text{Clamp}(0.10 + \text{dangerLevel} \times 0.02, 0.05, 0.50)$$

- Danger 2: $0.14$ ($14\%$ per tick)
- Danger 4: $0.18$ ($18\%$ per tick)
- Danger 7: $0.24$ ($24\%$ per tick)
- Danger 10: $0.30$ ($30\%$ per tick)

### Expected Encounter Count per Trip:
$$\mathbb{E}[\text{Encounters}] = 2 \times \text{distanceTicks} \times \text{encounterChancePerTick}$$
- Near trip (2 ticks outbound, Danger 2): $2 \times 2 \times 0.14 = 0.56$ encounters average.
- Deep trip (18 ticks outbound, Danger 10): $2 \times 18 \times 0.30 = 10.8$ encounters average across an 18-hour journey (necessitating armed squad + vehicle logistics).

## 4. Stamina Drain Derivation

Hourly stamina drain is derived as:

$$\text{baseStaminaDrainPerHour} = \text{Clamp}(1.5 + \text{dangerLevel} \times 0.25, 1.0, 5.0)$$

- Danger 2: $2.0\text{ drain/hr}$
- Danger 4: $2.5\text{ drain/hr}$
- Danger 7: $3.2\text{ drain/hr}$
- Danger 10: $4.0\text{ drain/hr}$
