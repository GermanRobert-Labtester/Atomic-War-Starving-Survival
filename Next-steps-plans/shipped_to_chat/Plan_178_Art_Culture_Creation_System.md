# Plan 178 — Art & Culture Creation System

## Goal

Create an art and culture creation system where survivors can create art, music, literature, and cultural artifacts that boost morale, express personality, preserve memories, and define the shelter's cultural identity. Currently no art creation system exists — survivors consume culture (radio, books) but never create it. `ApicultureSystem.cs` handles beekeeping but no human creative output. This plan adds cultural depth and creative expression to the shelter.

## Why

**Repository evidence:** Grep for `ArtSystem`, `CultureSystem`, `CreativeWork`, `ArtCreation`, `CulturalArtifact` in Core returns only `ApicultureSystem.cs` (beekeeping — not human art). No art creation, no music composition, no writing system, no cultural artifact creation. Survivors are consumers, not creators. The shelter has no culture beyond what players imagine.

**What is missing:** No art creation. No music composition. No writing/poetry. No cultural artifacts. No creative expression mechanics. No shelter cultural identity. Survivors have no creative outlets.

**Why existing plans don't solve it:** Plan 161 (hobbies/leisure) adds personal pastimes but not cultural creation. Plan 162 (archive) records history but doesn't create art. No plan addresses creative expression or cultural artifact creation.

**Player value:** Creates emotional depth (survivors express themselves), adds morale mechanics (art boosts morale), provides creative variety (different art forms), generates emergent stories (masterworks, artistic disputes), and makes the shelter feel culturally alive.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — skill system
- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — morale tracking
- `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs` — decor placement
- `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` — crafting system
- NEW: `Assets/Ashfall.Core/Culture/ArtCreationSystem.cs`
- NEW: `Assets/StreamingAssets/Data/art_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `ArtCreationSystem.cs` in `Assets/Ashfall.Core/Culture/`
2. Define `ArtForm` DTO: `formId`, `formName` (painting/sculpture/music/poetry/storytelling/theater), `requiredSkill` (art skill level), `requiredMaterials` (list of items), `creationTime` (ticks), `moraleBoost` (base value), `culturalValue` (0-100), `description`
3. Define `Artwork` DTO: `artworkId`, `artworkName`, `artForm`, `creatorId` (survivor), `creationDay`, `quality` (0-100), `theme` (hope/loss/resistance/nature/love/death), `location` (where displayed/performed), `culturalValue` (accumulated), `description`
4. Define `CulturalEvent` DTO: `eventId`, `eventType` (exhibition/concert/reading/performance/workshop), `artworks` (list of artwork IDs), `participants` (list of survivor IDs), `audience` (list), `moraleBoost`, `culturalImpact`
5. Define `ArtCreationState` DTO: list of artworks created, list of cultural events held, shelter cultural identity tags, total cultural value, art skill per survivor
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define art forms:
   - **Painting**: visual art, requires canvas/pigments, displayed on walls
   - **Sculpture**: 3D art, requires clay/stone/metal, displayed in rooms
   - **Music**: composed music, requires instruments, performed or recorded
   - **Poetry**: written verse, requires paper/ink, read aloud or posted
   - **Storytelling**: oral narrative, no materials, performed for groups
   - **Theater**: dramatic performance, requires multiple participants, performed
8. Define art themes:
   - **Hope**: optimistic art, strong morale boost
   - **Loss**: melancholic art, moderate morale, emotional depth
   - **Resistance**: defiant art, morale + faction standing
   - **Nature**: beauty-focused art, calm morale boost
   - **Love**: relationship art, bond strengthening
   - **Death**: memorial art, grief processing
9. Define creation mechanics:
   - Survivor with art skill creates artwork
   - Creation consumes materials and time
   - Quality based on skill + materials + inspiration
   - Artwork receives theme and description
   - Artwork can be displayed/performed
10. Define display/performance:
    - Visual art displayed in shelter rooms (integrates with ShelterDecorSystem)
    - Music performed in common areas
    - Poetry read aloud or posted
    - Stories told in gatherings
    - Theater performed for audience
    - Displayed art provides ongoing morale bonus
11. Define cultural events:
    - **Exhibition**: display multiple artworks, shelter-wide morale boost
    - **Concert**: musical performance, large morale boost
    - **Reading**: poetry/story reading, moderate morale boost
    - **Performance**: theater/drama, large morale boost
    - **Workshop**: teach art skills, skill transfer
12. Define cultural identity:
    - Shelter develops cultural tags based on art created
    - "Artistic" shelter: many artworks
    - "Musical" shelter: many musical works
    - "Literary" shelter: many written works
    - Cultural identity affects visitor perceptions
13. Add deterministic seeding: art creation uses `ISeededRng`
14. Wire into `GameBootstrap`: `SetupArtCreation`, `TickArtCreation`, `SaveArtCreation`
15. Create `ArtTemplateCatalogLoader` for art form definitions

## Main Task 2 — Implementation / Creation / Display / Events / Culture

1. Implement art creation:
   - Player selects art form and creator
   - Materials consumed
   - Creation time passes
   - Artwork generated (quality, theme, description)
   - Artwork added to collection
2. Implement artwork display:
   - Visual art placed in rooms (decor integration)
   - Displayed art provides ongoing morale bonus
   - Art can be moved between rooms
   - Art can be traded with other settlements
3. Implement performances:
   - Music/theater requires performance space
   - Performance scheduled and executed
   - Audience attends (morale boost)
   - Performance quality based on skill
   - Performance logged in cultural record
4. Implement cultural events:
   - Exhibition: gather artworks for showing
   - Concert: schedule musical performance
   - Reading: gather for poetry/stories
   - Performance: stage theater piece
   - Workshop: teach art skills
5. Implement cultural identity:
   - Tags develop based on art created
   - Identity affects visitor perceptions
   - Identity affects faction interactions
   - Identity displayed in shelter info
6. Implement art trading:
   - Artworks can be traded with factions
   - Art value based on quality and cultural significance
   - Trading builds faction standing
   - Some factions value specific art forms
7. Implement art education:
   - Art skill increases with creation
   - Workshops transfer art skill
   - Master artists create highest quality
   - Art skill can be inherited (Plan 154 integration)
8. Create art events:
   - "The Masterwork" — high-quality artwork created
   - "The Exhibition" — art exhibition held
   - "The Concert" — musical performance
   - "The Reading" — poetry reading
   - "The Performance" — theater piece staged
   - "The Workshop" — art skills taught
   - "The Trade" — artwork traded with faction
9. Add art quest hooks:
    - "The Artist" — create first artwork
    - "The Master" — create masterwork (quality 90+)
    - "The Exhibition" — hold art exhibition
    - "The Concert" — perform musical concert
    - "The Culture" — establish shelter cultural identity
    - "The Teacher" — teach art to 5 survivors
    - "The Legacy" — create 20 artworks
10. Implement art UI:
    - Gallery panel: view all artworks
    - Creation panel: create new art
    - Performance panel: schedule performances
    - Cultural identity display: shelter culture tags
    - Art detail: quality, theme, creator, location
11. Add art journal: automatic log of art events
12. Implement art tutorial: first creation explains system
13. Add art tooltips: hover over artwork shows details
14. Create 15 art templates in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `ShelterDecorSystem`: visual art displayed as decor
2. Connect to `NeedsSystem`: art provides morale boost
3. Integrate with `SkillProgressionSystem`: art skill progression
4. Connect to `FactionBranchCoordinator`: art trading affects standing
5. Wire into `CraftingSystem`: art materials from crafting
6. Connect to `SeasonalEventSystem` (Plan 170): cultural events at festivals
7. Implement old-save compatibility: existing saves get empty art state
8. Add deterministic seeding: creation uses `ISeededRng`
9. Create exploit prevention: art requires materials and time
10. Add tests: art creation, display, performances, events, trading, save round-trip
11. Verify catalog integrity: all art form/template IDs resolve
12. Test edge cases: no art (no culture), extensive art (rich culture)
13. Verify headless behavior: art processes correctly without UI
14. Add data-integrity-selftest: art templates validate against item catalogs
15. Create `--art-creation-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --art-creation-selftest
```

## Risk

**LOW** — Art creation is straightforward with clear inputs (materials, skill, time) and outputs (artworks, morale). Risk of art feeling like busywork. Mitigation: make art meaningful (morale boost, cultural identity), show quality variation, and integrate with existing systems.

## Definition of Done

- `ArtCreationSystem.cs` exists with full `CaptureState/RestoreState`
- 6 art forms implemented (painting, sculpture, music, poetry, storytelling, theater)
- 6 art themes (hope, loss, resistance, nature, love, death)
- Art creation mechanics functional
- Artwork display in shelter rooms
- Performance system (music, theater)
- Cultural events (exhibition, concert, reading, performance, workshop)
- Cultural identity system
- Art trading with factions
- Art events and quest hooks
- Save/load round-trip tested
- Old saves load without error
- 15 art templates in data authority
- UI gallery and creation panels
- Cross-system integration (decor, needs, skills, factions, crafting, seasons)

## Follow-On Opportunities

- Art competitions (shelter art contests)
- Art legacy (famous artworks remembered)
- Art quests (create specific artworks)
- Art trading posts (dedicated art trade)
- Art education (formal art training)
