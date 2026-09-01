# Black Flotilla Radio Matrix (Plan 23 / Task 23A)

Authority: `faction_radio_corpus.json` → new band `faction_black_flotilla` on
frequency **124.2 MHz** (≥1.5 MHz from every other band, pinned by test), callsign
**MOORING WATCH / BLACK FLOTILLA**. Delivery is exclusively `FactionRadioEngine`
(`LoadFromJson` → `RegisterChannel`); selection is deterministic
(day, frequency, seeded RNG — `PickIndex`), pinned by tests.

## The eight authored broadcasts

| # | Required category | Engine pool | Message (gist) | State consumed |
|---|---|---|---|---|
| 1 | Weather/current advisory | intercept_chatter | Falling glass, long south-east swell; shelf claims in by dark | weather/current advisory voice |
| 2 | Salvage-claim warning | intercept_chatter | Mail-steamer off the cape re-marked; step marks on the tide line | real site/claim lore |
| 3 | Convoy/escort challenge | parley_resolution | Heave to, show the bar-and-dot ribbon, stow weapons | escort encounter grammar |
| 4 | Missing-diver notice | intercept_chatter | Black-ribbon code; honest air and line; recovery payment | deep-site danger (23B) |
| 5 | Coded deep-dive status | intercept_chatter | "grey door stood open, black door is shut, third ribbon is out" | deep-site state; ribbon code |
| 6 | Trade bulletin | trade_reaction | Wants dry cloth/needles; pays for inland carry | Flotilla trade needs |
| 7 | Standing-sensitive invitation | parley_resolution | "mooring berths you tonight" — trusted-tier berth offer | standing ≥ trust tier |
| 8 | Aftermath of major event | raid_warning (+aftermath chatter) | "all marks suspended until the convoy answers" — convoy war aftermath | war/convoy state (23D bridge) |

## Code vocabulary (stable, repeated)

| Term | Meaning | Introduced |
|---|---|---|
| bar-and-dot | escort challenge mark; show or be boarded | broadcast 3 |
| black ribbon | diver lost / line lost | broadcast 4, structure doc |
| third ribbon | a deep mark earned per lost crew on one hull | broadcast 5, 8 |
| step marks | tide-exposed survey markers | broadcast 2 |
| mooring | the Flotilla itself / safe berth | 1, 6, 7 |

Comprehensibility rule: every broadcast carries one operational fact (window, claim,
challenge, price) in plain words beside the code. Code terms repeat across broadcasts;
two broadcasts re-read differently once the player owns a ribbon
(`item_escort_challenge_ribbon`, `item_deep_service_ribbon`) — meaning shifts by
context, not by a second decryption runtime (no new radio runtime; 23A.8 rule).

## Gates

- Delivery: `FactionRadioEngine.LoadFromJson` registers the band; pools non-empty.
- Determinism: same faction+kind+day+seed ⇒ same message.
- Frequency: 124.2 MHz ≥ 1.5 MHz from every other band.
- Corpus vocabulary test: `claim`, `ribbon`, `mooring` present (identity + payoff).
