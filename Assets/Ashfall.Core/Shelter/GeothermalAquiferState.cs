// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Shelter
{
    [Serializable]
    public sealed class GeothermalAquiferState
    {
        public string systemId = GeothermalAquiferSystem.SystemId;
        public float currentDepthMeters = 0f;
        public float drillBitCondition = 100f;
        public float steamPressurePsi = 0f;
        public float mineralScaling = 0f;
        public float activeTurbineOutput = 0f;
        public string currentStrataId = string.Empty;
        public float installedCasingDepth = 0f;
        public bool turbineCommissioned = false;
        public bool aquiferTapped = false;
        public float pressureReliefState = 0f; // 0=closed, 1=fully vented
        public float generatorHealth = 100f;
        public bool projectActive = false;
        public int lastProcessedDay = -1;
        public List<string> crossedStrataIds = new List<string>();
    }
}
