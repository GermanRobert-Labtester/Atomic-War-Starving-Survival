# Plan 167 — Espionage Closeout

`EspionageSystem` is a separate faction subdomain. It canonicalizes faction IDs, keeps spy networks and faction-security state outside `FactionWarSystemState`, reserves agents through a host availability callback, advances missions deterministically, tiers intel facts, emits abstract bounded consequence intents, and models capture, ransom, and staged compromise state.

The mission catalog is `Assets/StreamingAssets/Data/espionage_missions.json`. `EspionageHostSession` loads it and is enrolled in the Godot campaign composition root, campaign-day coordinator, lifecycle reset, and campaign envelope capture.

Focused verification: `Plan167EspionageTests` passed 6/6. The completion path now updates both mission and network status before resolving duration, and ransom removes the captured mission into history so a returned agent cannot remain committed by stale active state.

Remaining integration work includes the player-facing intelligence map, canonical faction consequence adapters, radio presentation, and rescue-quest trigger routing through the shared quest runtime.
