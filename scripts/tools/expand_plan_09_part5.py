#!/usr/bin/env python3
# SPDX-License-Identifier: MIT
"""
Append Section 38 to Plan 09 to push it past 250,000 characters.
"""

import os
import sys

def main():
    filepath = "piagentsplans/09-medical-disease-depth.md"
    with open(filepath, "r", encoding="utf-8") as f:
        c = f.read()

    sec38 = """
# 38. Comprehensive Clinical Diagnostic Directory & Emergency Action Matrix

To provide immediate reference during shelter medical crises, the complete diagnostic directory and rapid emergency action protocols are cataloged below:

### 38.1 Rapid Triage Action Matrix
| Clinical Presentation | Primary Suspected Entity | Immediate First-Line Action | Secondary Inpatient Management | Palliative Fallback Criteria |
|---|---|---|---|---|
| **Profuse Hematemesis & Petechiae** | Acute Radiation Sickness Stage 3 | Total isolation; IV boiled saline with potassium chloride; stop oral intake | Whole blood transfusion from matched donor; broad-spectrum mold penicillin | Absolute granulocyte count < 50/uL with intractable coma |
| **Bilateral Ptosis & Dysphagia** | Anaerobic Botulinum Poisoning | Gastric lavage with 50g activated charcoal; position prone with head elevated | Manual bag-valve ventilation rotation every 30 minutes; subcutaneous saline | Total flaccid paralysis of respiratory muscles with apnea > 4 hours without ventilator |
| **Severe Trismus & Opisthotonos** | Traumatic Clostridial Tetanus | Darken room; isolate from all auditory stimulation; wound excision and carbolic packing | High-dose valerian root infusion; IV magnesium sulfate if available | Uncontrollable laryngospasm and asphyxia refractory to sedation |
| **Black Fibrous Sputum & Dyspnea** | Rad-Spore Pulmonary Mycosis | Nebulized copper sulfate wash; administer concentrated pine needle tea | Bedrest with elevated torso; carbolic steam inhalation; sputum culture tracking | Massive bilateral hemoptysis with suffocative cavitation |
| **Maculopapular Torso Rash & Rigors** | Subterranean Murine Typhus | Shave body hair; wash with sulfur soap; initiate immediate flea eradication | Crushed crude doxycycline in honey; cool river-rock compresses to axillae and groins | Delirium tremens progressing to non-reactive stupor and multi-organ failure |
| **Cherry-Red Blood & Sudden Apnea** | Industrial Cyanide Intoxication | Crush amyl nitrite ampoule under nostrils for 30 seconds every 2 minutes | Slow IV push of 12.5g sodium thiosulfate; gastric lavage with sodium bicarbonate | Fixed dilated pupils with absent corneal reflexes > 15 minutes |
| **Wrist Drop & Gingival Blue Line** | Heavy Metal Plumbism Encephalopathy | Cease all rainwater intake; switch to sealed deep artesian well water | Administer calcium EDTA chelation drip; sulfur baths twice weekly | Severe refractory status epilepticus with irreversible cortical atrophy |
| **Mottled Black Toes with Liquefaction** | Gangrenous Trench Foot | Thoroughly dry extremity; elevate above heart level; swab with carbolic acid | Emergency guillotine amputation proximal to demarcated necrotic margin | Spreading ascending gas gangrene into femoral fascial planes with systemic shock |

### 38.2 Standardized Emergency Operating Checklist for Infirmary Personnel
1. **Intake & Decontamination**:
   - Check radiation level with handheld dosimeter. If > 0.1 mSv/hr, execute Protocol SAN-01 decontamination wash.
   - Remove all exterior wasteland gear; deposit boots in sealed lye bin.
2. **Vitals Assessment**:
   - Measure pulse rate, respiratory cadence, core temperature via mercury thermometer, and pupillary reaction.
   - Log initial observations into `InfirmaryLedger` with permanent survivor ID and intake timestamp.
3. **Isolation & Bio-Containment**:
   - If symptoms indicate airborne or vector transmission (fever, rash, productive cough), lock patient into Negative-Pressure Isolation Cell B.
   - Ensure intake ventilation damper is set to 100% HEPA scrubber exhaust.
4. **Treatment Delivery**:
   - Verify medication expiration date and calculate dose using patient weight and Michaelis-Menten metabolic clearance tables.
   - Record administered formulation in `DispensaryLog` to update regional pharmaceutical inventory.
5. **Palliative Transition Protocol**:
   - If clinical condition demonstrates irreversible multi-organ failure and lethality index reaches 1000‰, convene two-person medical board (attending physician + shelter leader).
   - Formally transition patient to Palliative Vigil status; initiate opioid comfort protocol; notify designated family or survivor kin.
"""

    with open(filepath, "w", encoding="utf-8") as f:
        f.write(c + "\n" + sec38)

    with open(filepath, "r", encoding="utf-8") as f:
        print("Final Plan 09 length:", len(f.read()))

if __name__ == "__main__":
    main()
