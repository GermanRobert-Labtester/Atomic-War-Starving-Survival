using System;
using System.IO;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests;

public class FieldGuidePersistenceTests
{
    private static string DataDir =>
        Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");

    [Fact]
    public void RoundTrip_PreservesUnlockedIds()
    {
        var catalog = FieldGuideCatalog.LoadFromDirectory(DataDir, new FileSystemIO());

        catalog.UnlockEntry("field_guide_scat_predator_marking");
        catalog.UnlockEntry("field_guide_browsing_stripped_bark");

        var state = catalog.CaptureState();
        Assert.Equal(2, state.UnlockedEntryIds.Count);

        var catalog2 = FieldGuideCatalog.LoadFromDirectory(DataDir, new FileSystemIO());
        catalog2.RestoreState(state);

        Assert.True(catalog2.IsUnlocked("field_guide_scat_predator_marking"));
        Assert.True(catalog2.IsUnlocked("field_guide_browsing_stripped_bark"));
        Assert.False(catalog2.IsUnlocked("field_guide_birdsong_silence_omen"));
    }

    [Fact]
    public void Restore_NullState_ClearsUnlocked()
    {
        var catalog = FieldGuideCatalog.LoadFromDirectory(DataDir, new FileSystemIO());

        catalog.UnlockEntry("field_guide_scat_predator_marking");
        catalog.RestoreState(null);

        Assert.Empty(catalog.CaptureState().UnlockedEntryIds);
    }

    [Fact]
    public void Restore_FiltersInvalidIds()
    {
        var catalog = FieldGuideCatalog.LoadFromDirectory(DataDir, new FileSystemIO());

        var state = new FieldGuideState
        {
            UnlockedEntryIds = new() { "field_guide_scat_predator_marking", "nonexistent_entry_id" }
        };
        catalog.RestoreState(state);

        Assert.True(catalog.IsUnlocked("field_guide_scat_predator_marking"));
        Assert.False(catalog.IsUnlocked("nonexistent_entry_id"));
    }
}
