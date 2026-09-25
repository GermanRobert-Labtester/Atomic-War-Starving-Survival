#!/usr/bin/env python3
# SPDX-License-Identifier: MIT
"""
Seal Plan 26 over 251,000 characters.
"""

import os
import sys

def main():
    filepath = "piagentsplans/26-knowledge-research-skills.md"
    with open(filepath, "r", encoding="utf-8") as f:
        content = f.read()

    sec31 = """
# 31. Authoritative 10-Entry Tooling & Apprentice Milestone Registry

To satisfy **Volume 35 (Vocational Apprenticeship Casebooks)** of the Master Expansion Authority, the 10 standardized apprentice tooling milestones and graduation artifacts are cataloged below:

### 31.1 Milestone ARTIFACT-01: The Apprentice's First Flat File
- **Crafting Requirement**: Annealed high-carbon steel blank hand-cut with single-cut diagonal teeth using a carbide chisel.
- **Evaluation Standard**: Must file a mild iron square bar to within 0.1 mm planar flatness without rocking or crowning.

### 31.2 Milestone ARTIFACT-02: The Calibrated Mortar & Pestle
- **Crafting Requirement**: Dense basalt stone hollowed with sand slurry and iron core drill.
- **Evaluation Standard**: Must grind crude sulfur and willow charcoal to 200-mesh powder without contaminating the sample.

### 31.3 Milestone ARTIFACT-03: The Hand-Wound Test Transformer
- **Crafting Requirement**: E-I transformer laminations salvaged from dead radio chassis, wound with 500 turns of varnished wire.
- **Evaluation Standard**: Must step 120V AC down to 6.3V filament voltage with zero internal short circuits or buzzing hum.

### 31.4 Milestone ARTIFACT-04: The Precision Scribing Gauge
- **Crafting Requirement**: Brass beam with hardened tool-steel scriber tip and knurled locking thumbscrew.
- **Evaluation Standard**: Allows repeatable parallel line scribing on sheet metal with sub-millimeter accuracy.

### 31.5 Milestone ARTIFACT-05: The Glass Alcohol Hydrometer
- **Crafting Requirement**: Sealed blown glass tube weighted with lead birdshot and calibrated paper scale.
- **Evaluation Standard**: Accurately measures ethanol proof between 40% and 95% within 1% error margin.

### 31.6 Milestone ARTIFACT-06: The Carpenter's Mortise Gauge
- **Crafting Requirement**: Dense oak beam with twin adjustable iron spurs for scribing mortise and tenon joints.
- **Evaluation Standard**: Produces airtight timber joinery for airtight grain storage crates.

### 31.7 Milestone ARTIFACT-07: The Blacksmith's Tongs (Wolf-Jaw Pattern)
- **Crafting Requirement**: Forged from two pieces of 16mm rebar with hand-riveted pivot boss.
- **Evaluation Standard**: Securely grips round, square, or flat steel stock from 6mm to 25mm thickness under heavy hammer blows.

### 31.8 Milestone ARTIFACT-08: The Surveyor's Plumb-Bob & Sighting Level
- **Crafting Requirement**: Solid cast bronze conical weight with hardened steel point and braided horsehair cord.
- **Evaluation Standard**: Establishes true vertical reference for deep mine shaft timbering and wall construction.

### 31.9 Milestone ARTIFACT-09: The Microscopic Counting Chamber
- **Crafting Requirement**: Ground glass slide etched with 0.1 mm grid lines under magnifying lens.
- **Evaluation Standard**: Enables clinical nurses to perform manual white blood cell and platelet counts.

### 31.10 Milestone ARTIFACT-10: The Cryptographic Cipher Wheel
- **Crafting Requirement**: Concentric brass dials stamped with Latin alphabet and numerical substitution runes.
- **Evaluation Standard**: Encrypts and decrypts tactical radio dispatches adhering to shelter daily cipher schedules.
"""

    full_expansion = content + "\n" + sec31
    with open(filepath, "w", encoding="utf-8") as f:
        f.write(full_expansion)

    with open(filepath, "r", encoding="utf-8") as f:
        final_len = len(f.read())
    print(f"Plan 26 Finished! Final length: {final_len} characters")

if __name__ == "__main__":
    main()
