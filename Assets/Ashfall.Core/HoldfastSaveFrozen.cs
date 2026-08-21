using System;

namespace Ashfall.Core
{
    /// <summary>
    /// FROZEN copy of <see cref="IceRoadSystemState"/> as it existed in envelope
    /// versions 1-3. Envelope DTOs are NOT frozen by declaring version alone:
    /// the nested state DTOs drift, and any drift silently hard-rejects every
    /// legitimate older save (SaveChecksum hashes all public fields). So the
    /// moment a nested DTO changes, the old shape must be frozen here and the
    /// frozen envelopes must use these copies. Do not modify after creation —
    /// copy the class instead.
    /// </summary>
    [Serializable]
    public class IceRoadSystemStateV1toV3
    {
        public string systemId;
        public bool expansionUnlocked;
        public bool isOpen;
        public float iceThicknessM;
        public bool cuttersAccess = true;
        public bool southBeaconLit = true;
        public int windowDaysRemaining;
        public int windowLengthDays;
        public int lampsOutCountdown;
        public int windowsCompleted;
        public int accidentCount;
        public int lastOpenDay = -1;
        public int lastCloseDay = -1;
        public bool clerkStarted;
        public bool yaraWithdrewPermanently;
        public int seedSalt;

        public static IceRoadSystemStateV1toV3? From(IceRoadSystemState src)
        {
            if (src == null) return null;
            return new IceRoadSystemStateV1toV3
            {
                systemId = src.systemId,
                expansionUnlocked = src.expansionUnlocked,
                isOpen = src.isOpen,
                iceThicknessM = src.iceThicknessM,
                cuttersAccess = src.cuttersAccess,
                southBeaconLit = src.southBeaconLit,
                windowDaysRemaining = src.windowDaysRemaining,
                windowLengthDays = src.windowLengthDays,
                lampsOutCountdown = src.lampsOutCountdown,
                windowsCompleted = src.windowsCompleted,
                accidentCount = src.accidentCount,
                lastOpenDay = src.lastOpenDay,
                lastCloseDay = src.lastCloseDay,
                clerkStarted = src.clerkStarted,
                yaraWithdrewPermanently = src.yaraWithdrewPermanently,
                seedSalt = src.seedSalt
            };
        }

        public IceRoadSystemState ToCurrent()
        {
            return new IceRoadSystemState
            {
                systemId = systemId,
                expansionUnlocked = expansionUnlocked,
                isOpen = isOpen,
                iceThicknessM = iceThicknessM,
                cuttersAccess = cuttersAccess,
                southBeaconLit = southBeaconLit,
                windowDaysRemaining = windowDaysRemaining,
                windowLengthDays = windowLengthDays,
                lampsOutCountdown = lampsOutCountdown,
                windowsCompleted = windowsCompleted,
                accidentCount = accidentCount,
                lastOpenDay = lastOpenDay,
                lastCloseDay = lastCloseDay,
                clerkStarted = clerkStarted,
                yaraWithdrewPermanently = yaraWithdrewPermanently,
                seedSalt = seedSalt
            };
        }
    }
}
