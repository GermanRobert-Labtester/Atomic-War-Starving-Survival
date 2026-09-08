# Holdfast Dispatch Consumption Contract

## 1. Architecture Overview
- **Storage:** `Assets/StreamingAssets/Data/holdfast_flavor.json`
- **Loader:** `AtomicWar.GodotApp.HoldfastFlavorCatalog.Load(string dataDirectory, ILog log)`
- **Lookup Dictionary:** `Dictionary<string, FactionVoice> FactionVoices` with `StringComparer.Ordinal`.
- **Consumer:** `AtomicWar.GodotApp.HoldfastDispatchLog`
- **UI Surface:** `HoldfastTerminalPanel` (renders entries in `_dispatchLog` Label).

## 2. Dispatch Call Sites & Field Mapping
The table below maps each method in `HoldfastDispatchLog` to the `FactionVoice` field consumed:

| Method | Trigger Condition | Consumed Field | Output Format |
|---|---|---|---|
| `OnFirstPurchase` | First buy in terminal session | `voice.voice` | `First requisition this session: {qty} × {item} for {totalValue}. {voice.voice}` |
| `OnPurchase` | Player buys item | `voice.voice` | `{qty} × {item} released. {totalValue} deducted. {voice.voice}` |
| `OnSale` | Player sells item | `voice.sold` | `{qty} × {item} accepted. {totalValue} credited. {voice.sold}` |
| `OnHoldingEmptied` | Player sells last held item | `voice.voice` | `The last {item} has left the shelf. {voice.voice}` |
| `OnStockLow` | Merchant stock drops to 1 | `voice.voice` | `Stock of {item} is now {remaining}. {voice.voice}` |
| `OnStockEmpty` | Merchant stock reaches 0 | `voice.voice` | `No holdings of {item} remain. {voice.voice}` |
| `OnRejected` | Buy or sell trade rejected | `voice.rejected` | `Requisition refused: {detail} {voice.rejected}` |

## 3. Register Field Contract
- `register` is a short descriptive descriptor (e.g. `bureaucratic`, `salvage`, `maritime`, `privateer`, `allocation`, `logistics`, `monopoly`, `foundry`) indicating the procedural tone of the institution's paperwork.
- It is deserialized into `FactionVoice.register`. It is not evaluated by runtime comparison switches.

## 4. Fallback Contract
If `factionId` is null, empty, or not found in `FactionVoices`, `HoldfastFlavorCatalog.GetFactionVoice` returns:
```csharp
public static readonly FactionVoice NeutralFactionVoice = new FactionVoice
{
    register = "neutral",
    voice = "The counterparty has no recorded voice.",
    rejected = "Transaction declined.",
    sold = "Item accepted."
};
```
This fallback is deterministic, safe, and preserved.
