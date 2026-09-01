# Plan 40 — Trade Credit Handoff

## Credit Offer Integration
At least 3 debt templates are reachable through existing trade encounters.

## Eligible Trade Scenarios
1. **Food crisis**: Player cannot pay for rations → Supply Corps offers `debt_supply_corps_rations`
2. **Fuel shortage**: Player cannot pay for fuel → Railway Guild offers `debt_railway_guild_fuel`
3. **Medical emergency**: Player cannot pay for medicine → Supply Corps offers `debt_supply_corps_medical`

## Credit Offer Requirements
- Player must be unable to pay upfront (insufficient trade value)
- Creditor faction must not be hostile (standing > -50)
- No existing unpaid debt from same creditor
- Template must be eligible for the current campaign state

## Offer Presentation
Where UI architecture supports it:
- Creditor name
- Principal (item + quantity)
- Due time (term days)
- Interest rate
- Forfeit description
- Default consequence summary

## Player Choice
- Credit acceptance is explicit (player must accept)
- No auto-acceptance of credit offers
