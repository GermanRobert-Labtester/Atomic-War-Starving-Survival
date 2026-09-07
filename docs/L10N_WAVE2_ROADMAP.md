# Localization Wave 2 roadmap

Wave 1 intentionally limits runtime conversion to two pilot surfaces.
Prioritize the remaining UI in this order:

1. Settings and save/load overlays, because they own user preferences and
   recovery messages.
2. Dashboard, inventory, survivors, medical, and research-adjacent panels,
   because they are visible during the first hour.
3. Weather, radio, expedition, and trade panels, because they contain
   state-driven templates and signal terminology.
4. Atlas and expansion panels after their domain-specific workflows are
   complete.

Each wave must preserve the key contract, add English and secondary-locale
rows, update the inventory, and run the drift and snapshot gates. Narrative,
quest, item, radio, and catalog prose remain deferred until sidecar data
translation ownership is approved.
