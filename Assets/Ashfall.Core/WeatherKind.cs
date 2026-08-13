namespace Ashfall.Core
{
    /// <summary>
    /// Kinds of weather, including fallout storms that spike radiation exposure.
    /// Integer values match Assets/_Game/Environment/WeatherSystem.cs so Unity
    /// can consume this enum later without remapping saves.
    /// </summary>
    public enum WeatherKind
    {
        Clear,
        Rain,
        Overcast,
        Ashfall,
        FalloutStorm,
        Blizzard,
        BlackRain,
        AcidSnow,
        BioFog,
        BlackSnow,
        BloodRain,
        EMPStorm,
        GlassStorm,
        RadHail,
        AlgaeBloom,
        AshLightning,
        ParticulateFog,
        ThermalInversion,
        IceStorm,
        Silence,
        FalseSpring,
        SilentSpring
    }
}
