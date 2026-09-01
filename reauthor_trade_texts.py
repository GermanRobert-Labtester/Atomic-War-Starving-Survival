import json

file_path = "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/Assets/StreamingAssets/Data/trade_texts.json"
with open(file_path, "r") as f:
    data = json.load(f)

# Traders Array
new_traders = [
    {
        "id": "trader_perimeter_scavenger",
        "display_name": "Perimeter Scavenger",
        "profile": "A localized asset operating on the margins of the contamination zone. Gear is functional, prioritizing radiation shielding over mobility. Does not engage in non-transactional dialogue.",
        "greetings": {
            "hostile": "Keep your hands away from your belt. State your deficit.",
            "wary": "I don't open my pack until I see your barter.",
            "neutral": "I have surplus copper and antibiotics. What is your deficit?",
            "warm": "You survived the ash storm. Let us balance our inventories."
        },
        "item_examinations": {
            "valuable": "High utility. I can offset a significant caloric cost for this.",
            "worthless": "Degraded mass. Zero operational value.",
            "interesting": "Uncommon structure. I will require time to assess its utility.",
            "dangerous": "Volatile. Keep it away from my pack."
        },
        "offers": {
            "fair": "Equivalency established. I accept this exchange.",
            "generous": "My surplus allows a favorable ratio. Take it.",
            "stingy": "My inventory is restricted. This is the maximum allocation.",
            "desperate": "My caloric deficit is critical. Take what you want for the rations."
        },
        "counter_offers": {
            "accept": "Adjustments accepted. Equivalency met.",
            "reject": "Negative. The ratio is unacceptable.",
            "negotiate": "Re-evaluate your offer. Include more processed calories.",
            "insulted": "You waste my time and my heat. Exchange terminated."
        }
    },
    {
        "id": "trader_logistics_broker",
        "display_name": "Logistics Broker",
        "profile": "A centralized exchange officer representing a larger settlement. Prioritizes bulk transactions and raw materials for infrastructure repair. Records all exchanges in a waterproof ledger.",
        "greetings": {
            "hostile": "My guards are armed. Conclude your business rapidly.",
            "wary": "Place your items on the tarp. Step back.",
            "neutral": "We require structural steel and heavy fuels. State your surplus.",
            "warm": "Your previous deliveries met quota. Proceed."
        },
        "item_examinations": {
            "valuable": "Acceptable tolerance. This meets our infrastructure requirements.",
            "worthless": "Corroded beyond recovery. Remove it from the tarp.",
            "interesting": "Non-standard, but potentially useful for the boiler room.",
            "dangerous": "Unstable isotopes detected. Secure that immediately."
        },
        "offers": {
            "fair": "The ledger balances. Transaction approved.",
            "generous": "We are subsidizing this exchange to maintain supply lines.",
            "stingy": "Current settlement quotas restrict my outflow.",
            "desperate": "The condenser is failing. I will trade the medical reserve for those parts."
        },
        "counter_offers": {
            "accept": "Re-calculated. Approved.",
            "reject": "The numbers do not align. Offer denied.",
            "negotiate": "Offset the difference with clean water.",
            "insulted": "You are insulting the settlement's intelligence. Depart."
        }
    }
]

# Procedural mapping for the bloat fields
bloat_templates = {
    "joke": {
        "description": "Humor is a caloric waste. Traders use structural paradoxes to signal in-group status without aggressive posturing.",
        "trader_text": "They say the water purifier filter tastes better than the water.",
        "player_text": "A dry observation confirming shared hardship."
    },
    "myth": {
        "description": "Unverified historical data. Often weaponized to inflate the perceived value of pre-war salvage.",
        "trader_text": "I heard a settlement down south still has a working hydroelectric dam. Just a rumor, of course.",
        "player_text": "I require material goods, not geographical fiction."
    },
    "ceremon": {
        "description": "Ritualized exchange protocols designed strictly to prevent sudden kinetic escalation during barter.",
        "trader_text": "We place our weapons on the table first. It is the rule of the exchange.",
        "player_text": "My rifle remains slung, but my hands are visible."
    },
    "riddle": {
        "description": "Cognitive stress tests used to gauge a counterpart's mental acuity and potential caloric deficit.",
        "trader_text": "What has a valve but no pipe, and beats but makes no sound? (A heart, checking for humanity).",
        "player_text": "A mechanical pump in a vacuum. Let us return to the transaction."
    },
    "agreement": {
        "description": "Temporary suspension of hostilities contingent on mutual resource exchange and enforced by localized violence.",
        "trader_text": "We agree on the exchange rate for a solar cycle. Violation means embargo.",
        "player_text": "The terms are logged and accepted."
    },
    "skill": {
        "description": "Specialized operational behaviors optimized for extracting maximum value from desperate entities.",
        "trader_text": "I can appraise a rusted bearing by its weight alone.",
        "player_text": "Your appraisal is noted. The price remains the same."
    },
    "disaster": {
        "description": "Logistical failures caused by environmental degradation, resulting in immediate price spikes and starvation events.",
        "trader_text": "The northern route collapsed under a mudslide. Lead prices are tripling.",
        "player_text": "I will adjust my acquisition priorities accordingly."
    },
    "etiquette": {
        "description": "Strict behavioral codes that minimize the risk of a misunderstanding escalating into a fatal firefight.",
        "trader_text": "Never reach into your pack without announcing the item first.",
        "player_text": "I am retrieving the payment module now."
    },
    "superstition": {
        "description": "Irrational risk-avoidance behaviors developed in response to invisible radiological and chemical hazards.",
        "trader_text": "I never trade items wrapped in red plastic. It attracts the ash-blind.",
        "player_text": "Color has no bearing on utility. We trade."
    },
    "history": {
        "description": "Degraded accounts of past logistical networks, largely irrelevant to current survival parameters.",
        "trader_text": "Before the Exchange, this road was choked with metal transports carrying endless food.",
        "player_text": "The past holds no calories. What is your current inventory?"
    }
}

# Apply updates
data["traders"] = new_traders

# Iterate through all other keys and replace them if they are dicts with description/trader_text
for key in data.keys():
    if key in ["schema_version", "collection_id", "traders"]:
        continue

    val = data[key]
    if isinstance(val, dict):
        matched = False
        for template_key, template_val in bloat_templates.items():
            if template_key in key.lower():
                data[key] = template_val
                matched = True
                break

        if not matched:
            # Generic cold bureaucratic replacement
            data[key] = {
                "description": f"Standardized logistical protocol regarding {key.replace('_', ' ')}.",
                "trader_text": "The logistical parameters are set. Conform to the exchange protocol.",
                "player_text": "Acknowledged. Proceeding with resource allocation."
            }

with open(file_path, "w") as f:
    json.dump(data, f, indent=4)
