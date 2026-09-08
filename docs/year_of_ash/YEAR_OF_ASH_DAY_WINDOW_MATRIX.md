# Year of Ash Day Window Matrix

`minDay` and `maxDay` are inclusive absolute campaign-day bounds for questline availability.

| Questline | Window |
|---|---:|
| Amnesty | 195–275 |
| Pilgrimage | 215–320 |
| Irrigation | 225–320 |
| Water Tax | 245–345 |
| Blackmail | 265–345 |
| Mutiny | 285–350 |
| Seed Failure | 300–355 |

The new windows are staggered across the late campaign rather than sharing one start day. They do
overlap by design; the current `QuestlineSystem` exposes all available definitions and does not own
a separate queue-arbitration or starvation policy. The host UI decides which playable definition to
start. Across all 15 definitions, inclusive-window overlap peaks at 10 eligible questlines around
Day 270 and again around Day 305; the pre-expansion peak was 5. This is an offer-list density, not a
new event frequency: the host resumes one active questline and otherwise presents the available
offers. This is documented rather than simulated as a new scheduler.
