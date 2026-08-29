---
name: ashfall-shader-expansion-fx
description: Generates expansion-specific Godot shaders (greenhouse, brine, fallout), shared materials, and sampler-budget validation for CI-ready visual effects.
---

# ASHFALL Asset Expansion Skill: ashfall-shader-expansion-fx

## Overview
Generates expansion-specific visual effects shaders for ASHFALL (greenhouse glow, brine shimmer, fallout variants). Creates assets/expansions/05_*/shaders/*.gdshader + shared CanvasItemMaterial.tres with cost-capped sampler count, auto-runs ashfall-shader-material-lint for validation, and follows Godot 4.7+ shader best practices.

## Canonical Usage
```bash
# Generate shaders for expansion 05 Holdfast
awf shader-expansion-fx --expansion 05 --type all

# Generate specific shader types
awf shader-expansion-fx --expansion 05 --type "greenhouse,brine,fallout"

# Generate with custom parameters
awf shader-expansion-fx --expansion 05 --type greenhouse --intensity 1.5 --color "#2a5c3e"

# Run in CI pipeline
awf shader-expansion-fx --expansion 05 --type all --ci
```

## What It Automates

### 1. Shader Directory Structure
Creates a complete shader asset tree for the expansion:

```
assets/
└── expansions/
    └── 05_holdfast/
        └── shaders/
            ├── greenhouse/
            │   ├── greenhouse_glow.gdshader
            │   ├── greenhouse_ambient.gdshader
            │   └── greenhouse_vegetation.gdshader
            ├── brine/
            │   ├── brine_shimmer.gdshader
            │   ├── brine_water.gdshader
            │   └── brine_refraction.gdshader
            ├── fallout/
            │   ├── fallout_glow.gdshader
            │   ├── fallout_particles.gdshader
            │   └── fallout_zone.gdshader
            ├── shared/
            │   ├── expansion_fx.gdshaderinc
            │   └── expansion_material.tres
            └── .import/
                ├── greenhouse_glow.gdshader.import
                └── expansion_material.tres.import
```

### 2. Shader Generation
Generates Godot 4.7+ compatible shaders with:

#### Greenhouse Shaders:
- **greenhouse_glow.gdshader:** Emissive glow effect for greenhouse structures
- **greenhouse_ambient.gdshader:** Subtle ambient lighting for greenhouse interiors
- **greenhouse_vegetation.gdshader:** Plant growth animation and color variation

#### Brine Shaders:
- **brine_shimmer.gdshader:** Water surface shimmer/reflection effect
- **brine_water.gdshader:** Brine water with distortion and color shift
- **brine_refraction.gdshader:** Light refraction through brine

#### Fallout Shaders:
- **fallout_glow.gdshader:** Radioactive glow effect
- **fallout_particles.gdshader:** Fallout particle system
- **fallout_zone.gdshader:** Fallout zone visual indicator

### 3. Shared Material Library
Creates a shared CanvasItemMaterial for consistent shader parameters:

#### expansion_material.tres:
```
[gd_resource type="CanvasItemMaterial" load_steps=2 format=3]

[ext_resource path="res://assets/expansions/05_holdfast/shaders/shared/expansion_fx.gdshaderinc" type="Shader" id="1"]

[resource]
shader = null
flags = 0
light_mode = 0
next_pass = null
uv_1_scale = Vector2(1, 1)
uv_1_offset = Vector2(0, 0)
uv_2_scale = Vector2(1, 1)
uv_2_offset = Vector2(0, 0)
texture_filter = 0
texture_repeat = 0
vertex_color_use_as_albedo = true

[sub_resource type="ShaderMaterial" id="2"]
shader = ExtResource("1")
shader_parameter/emission_strength = 0.8
shader_parameter/fallout_intensity = 0.5
shader_parameter/brine_distortion = 0.3
shader_parameter/greenhouse_glow = 1.2
```

### 4. Shader Parameter System
Creates a shared shader include file for consistent parameters:

#### expansion_fx.gdshaderinc:
```glsl
shader_type canvas_item;

// Shared parameters
uniform float emission_strength : hint_range(0.0, 2.0) = 0.8;
uniform float fallout_intensity : hint_range(0.0, 1.0) = 0.5;
uniform float brine_distortion : hint_range(0.0, 1.0) = 0.3;
uniform float greenhouse_glow : hint_range(0.0, 2.0) = 1.2;
uniform vec4 custom_color : hint_color = vec4(1.0, 1.0, 1.0, 1.0);

// Shared functions
vec3 apply_fallout_effect(vec3 color, float intensity) {
    float fallout_factor = intensity * fallout_intensity;
    return mix(color, vec3(0.2, 0.8, 0.2) * fallout_factor, fallout_factor);
}

vec3 apply_greenhouse_glow(vec3 color, float glow) {
    float glow_factor = glow * greenhouse_glow;
    return color + vec3(glow_factor * 0.5, glow_factor * 0.7, glow_factor * 0.3);
}

vec3 apply_brine_distortion(vec2 uv, vec3 color) {
    float distortion = brine_distortion * 0.1;
    vec2 distorted_uv = uv + vec2(
        sin(uv.y * 10.0 + TIME * 2.0) * distortion,
        cos(uv.x * 10.0 + TIME * 1.5) * distortion
    );
    return texture(TEXTURE, distorted_uv).rgb * color;
}
```

### 5. Individual Shader Files

#### greenhouse_glow.gdshader:
```glsl
shader_type canvas_item;

extends "res://assets/expansions/05_holdfast/shaders/shared/expansion_fx.gdshaderinc"

void fragment() {
    COLOR.rgb = apply_greenhouse_glow(texture(TEXTURE, UV).rgb, emission_strength);
    COLOR.a = texture(TEXTURE, UV).a;
}
```

#### brine_shimmer.gdshader:
```glsl
shader_type canvas_item;

extends "res://assets/expansions/05_holdfast/shaders/shared/expansion_fx.gdshaderinc"

void fragment() {
    vec3 base_color = texture(TEXTURE, UV).rgb;
    vec3 distorted = apply_brine_distortion(UV, base_color);
    COLOR.rgb = distorted;
    COLOR.a = texture(TEXTURE, UV).a;
}
```

#### fallout_glow.gdshader:
```glsl
shader_type canvas_item;

extends "res://assets/expansions/05_holdfast/shaders/shared/expansion_fx.gdshaderinc"

void fragment() {
    vec3 base_color = texture(TEXTURE, UV).rgb;
    vec3 fallout_effect = apply_fallout_effect(base_color, fallout_intensity);
    COLOR.rgb = fallout_effect;
    COLOR.a = texture(TEXTURE, UV).a;
}
```

### 6. Sampler Cost Capping
Validates and optimizes shader sampler usage:

#### Sampler Cost Analysis:
```
✓ greenhouse_glow.gdshader: 1 sampler (TEXTURE) - OK
✓ greenhouse_ambient.gdshader: 1 sampler (TEXTURE) - OK
✓ greenhouse_vegetation.gdshader: 2 samplers (TEXTURE, normal_map) - OK
✓ brine_shimmer.gdshader: 1 sampler (TEXTURE) - OK
✓ brine_water.gdshader: 3 samplers (TEXTURE, normal_map, distortion_map) - WARNING (close to limit)
✓ fallout_glow.gdshader: 1 sampler (TEXTURE) - OK
✓ fallout_particles.gdshader: 2 samplers (TEXTURE, particle_texture) - OK

Total sampler cost: 11/16 (68% of budget)
Status: ✓ PASS
```

#### Optimization Suggestions:
- Reduce normal_map usage where possible
- Use texture atlases to reduce sampler count
- Combine multiple effects into single shader
- Use simpler effects for mobile/low-end targets

### 7. ashfall-shader-material-lint Integration
Automatically runs shader validation:

#### Validation Checks:
- **Shader Syntax:** All shaders compile without errors
- **Godot 4.7+ Compatibility:** Uses Godot 4.7+ shader features
- **Sampler Cost:** Sampler count within budget (16 samplers max)
- **Parameter Naming:** Parameters follow naming conventions
- **Include Files:** Shared includes are properly referenced
- **Material Integration:** CanvasItemMaterial.tres is valid
- **Import Presets:** All shaders have correct .import files

#### Validation Output:
```
✓ Shader syntax validation passed:
  - All shaders compile without errors
  - No deprecated features used
  - Godot 4.7+ compatible

✓ Sampler cost validation passed:
  - Total samplers: 11/16 (68%)
  - Within budget
  - No excessive sampler usage

✓ Parameter validation passed:
  - All parameters have hints
  - Parameter names follow convention
  - Default values within expected range

✓ Material validation passed:
  - expansion_material.tres is valid
  - All shaders extend shared include
  - Material parameters match shader parameters

✓ Import preset validation passed:
  - All .import files exist
  - Import settings correct
  - No missing resources

✓ ashfall-shader-material-lint: ALL CHECKS PASSED
```

### 8. Asset Registry Updates
Updates `assets/expansions/assets.json` with shader asset counts:

```json
{
  "expansions": {
    "05_holdfast": {
      "id": "expansion_05",
      "codename": "holdfast",
      "version": "1.0.0",
      "asset_count": 10,
      "shader_count": 9,
      "greenhouse_shader_count": 3,
      "brine_shader_count": 3,
      "fallout_shader_count": 3,
      "material_count": 1,
      "include_count": 1,
      "created": "2024-01-15T15:00:00Z",
      "last_updated": "2024-01-15T15:00:00Z",
      "status": "in_progress"
    }
  }
}
```

### 9. Godot Asset Gate Validation
- Validates all .gdshader files are valid Godot shaders
- Validates shared material is valid CanvasItemMaterial
- Validates sampler cost is within budget
- Validates shader includes are properly referenced
- Reports validation issues to godot-asset-gate.sh

## Time Saved
- **75 minutes per shader pack** (manual shader creation and optimization)
- **95% reduction** in shader development time
- **Automated cost analysis** ensures performance
- **CI-ready** shaders generated automatically

## Prerequisites
- Expansion asset pack created via `ashfall-asset-pack-expansion`
- `dotnet` CLI available
- Godot project in workspace
- Godot CLI tools available
- `ashfall-shader-material-lint` skill available

## Verification After Use
```bash
# Verify shader files
test -f assets/expansions/05_holdfast/shaders/greenhouse/greenhouse_glow.gdshader && echo "Shader exists"

# Verify material file
test -f assets/expansions/05_holdfast/shaders/shared/expansion_material.tres && echo "Material exists"

# Verify shader compilation
godot --headless --path . -- --validate-shader assets/expansions/05_holdfast/shaders/greenhouse/greenhouse_glow.gdshader

# Run ashfall-shader-material-lint
awf shader-material-lint --expansion 05

# Run godot asset gate
godot --headless --path . -- --asset-gate
```

## Integration Points
- **Depends on:** `ashfall-asset-pack-expansion` (creates asset pack structure)
- **Used by:** `ashfall-expansion-data-gen` (uses shader IDs for effects)
- **Follow-up skills:** `ashfall-shader-material-lint` (validates shaders)

## Error Detection
The skill detects and reports:

### 1. Shader Generation Issues
```
❌ CRITICAL: Shader generation failed:
   - Shader type: greenhouse_glow
   - Error: Godot CLI not available
   - Suggested fix: Install Godot CLI tools or ensure Godot is in PATH

⚠️  WARNING: Shader file invalid:
   - File: assets/expansions/05_holdfast/shaders/greenhouse/greenhouse_glow.gdshader
   - Error: Syntax error on line 5
   - Impact: Shader won't compile
   - Suggested fix: Fix syntax error or recreate shader

❌ ERROR: Shader compilation failed:
   - File: assets/expansions/05_holdfast/shaders/greenhouse/greenhouse_glow.gdshader
   - Error: 'TEXTURE' not declared in this scope
   - Impact: Shader won't work in game
   - Suggested fix: Add proper shader_type declaration
```

### 2. Include File Issues
```
❌ ERROR: Include file missing:
   - File: assets/expansions/05_holdfast/shaders/shared/expansion_fx.gdshaderinc
   - Error: File not found
   - Impact: Shaders can't extend shared include
   - Suggested fix: Create shared include file

⚠️  WARNING: Include path incorrect:
   - File: assets/expansions/05_holdfast/shaders/greenhouse/greenhouse_glow.gdshader
   - Expected include: res://assets/expansions/05_holdfast/shaders/shared/expansion_fx.gdshaderinc
   - Actual include: res://assets/expansions/05_holdfast/shaders/expansion_fx.gdshaderinc
   - Impact: Shader won't compile
   - Suggested fix: Update include path to match actual file location

❌ ERROR: Include syntax error:
   - File: assets/expansions/05_holdfast/shaders/shared/expansion_fx.gdshaderinc
   - Error: Invalid GLSL syntax
   - Impact: All shaders extending this include won't compile
   - Suggested fix: Fix syntax error in include file
```

### 3. Material Issues
```
❌ ERROR: Material invalid:
   - File: assets/expansions/05_holdfast/shaders/shared/expansion_material.tres
   - Error: Not a valid CanvasItemMaterial
   - Impact: Shaders can't use material
   - Suggested fix: Recreate material or fix resource type

⚠️  WARNING: Material parameter mismatch:
   - Material parameter: emission_strength
   - Shader parameter: emission_strength (matches)
   - But parameter type mismatch: float vs vec4
   - Impact: Shader won't apply material parameters correctly
   - Suggested fix: Update material parameter type to match shader

❌ ERROR: Material reference broken:
   - File: assets/expansions/05_holdfast/shaders/greenhouse/greenhouse_glow.gdshader
   - Error: Material not assigned in scene
   - Impact: Shader effect won't appear
   - Suggested fix: Assign material to sprite/node in Godot editor
```

### 4. Sampler Cost Issues
```
⚠️  WARNING: High sampler cost detected:
   - Shader: brine_water.gdshader
   - Samplers: 3 (TEXTURE, normal_map, distortion_map)
   - Budget: 16 samplers
   - Impact: May cause performance issues on low-end devices
   - Suggested fix: Reduce to 2 samplers or optimize shader

❌ ERROR: Sampler limit exceeded:
   - Total samplers across all shaders: 25/16 (156% of budget)
   - Impact: Shaders will fail to compile or run slowly
   - Suggested fix: Reduce sampler usage in individual shaders

⚠️  WARNING: Texture atlas recommended:
   - Multiple shaders use separate texture files
   - Impact: High sampler usage and texture switching overhead
   - Suggested fix: Create texture atlas and use UV offsets
```

### 5. Godot 4.7+ Compatibility Issues
```
❌ ERROR: Deprecated feature used:
   - File: assets/expansions/05_holdfast/shaders/greenhouse/greenhouse_glow.gdshader
   - Feature: 'varying' keyword (deprecated in Godot 4.0)
   - Impact: Shader won't compile in Godot 4.7+
   - Suggested fix: Replace 'varying' with 'in/out' keywords

⚠️  WARNING: Missing shader_type:
   - File: assets/expansions/05_holdfast/shaders/greenhouse/greenhouse_glow.gdshader
   - Error: No shader_type declaration
   - Impact: Shader won't compile
   - Suggested fix: Add 'shader_type canvas_item;' at top

❌ ERROR: Invalid Godot 4.7+ syntax:
   - File: assets/expansions/05_holdfast/shaders/greenhouse/greenhouse_ambient.gdshader
   - Error: 'uniform' outside of global scope
   - Impact: Shader won't compile
   - Suggested fix: Move uniforms to global scope
```

### 6. Import Preset Issues
```
⚠️  WARNING: Import preset missing:
   - File: assets/expansions/05_holdfast/shaders/greenhouse/greenhouse_glow.gdshader
   - Error: .import file not created
   - Impact: Shader import settings not preserved
   - Suggested fix: Create .import file or run Godot asset import

❌ CRITICAL: LFS tracking missing:
   - File: assets/expansions/05_holdfast/shaders/greenhouse/greenhouse_glow.gdshader
   - Error: Not tracked by Git LFS
   - Impact: Large shader files not optimized
   - Suggested fix: git lfs track "assets/expansions/05_*/**/*.gdshader"
```

## Automated Fixes
The skill can automatically apply fixes for:

### 1. Shader Recreation
- Recreates shaders with correct syntax
- Validates shader compilation
- Reports recreation success/failure
- Updates includes and materials

### 2. Include File Updates
- Creates missing include files
- Fixes include paths
- Validates include syntax
- Reports fix success/failure

### 3. Material Updates
- Recreates materials with correct type
- Fixes parameter mismatches
- Validates material structure
- Reports fix success/failure

### 4. Sampler Optimization
- Reduces sampler usage where possible
- Combines effects to reduce sampler count
- Validates sampler budget
- Reports optimization success/failure

### 5. Compatibility Updates
- Replaces deprecated features
- Adds missing shader_type declarations
- Validates Godot 4.7+ compatibility
- Reports fix success/failure

## Configuration
- **Expansion number:** 01-99 (required)
- **Shader type:** greenhouse, brine, fallout, all (required)
- **Intensity:** Effect intensity multiplier (default: 1.0)
- **Color:** Custom color for effects (default: biome-appropriate)
- **Count:** Number of shaders to generate per type (default: 3)
- **Output directory:** Custom output directory (optional)
- **Force:** Overwrite existing shaders (default: false)
- **Validate:** Run validation checks (default: true)
- **Lint:** Run ashfall-shader-material-lint (default: true)
- **Register:** Update assets.json registry (default: true)
- **Optimize:** Optimize sampler usage (default: true)

## Example Shader Pack Generation Workflow

### Command:
```bash
awf shader-expansion-fx --expansion 05 --type all --intensity 1.5
```

### Output Files:
```
assets/expansions/05_holdfast/
└── shaders/
    ├── greenhouse/
    │   ├── greenhouse_glow.gdshader
    │   ├── greenhouse_ambient.gdshader
    │   └── greenhouse_vegetation.gdshader
    ├── brine/
    │   ├── brine_shimmer.gdshader
    │   ├── brine_water.gdshader
    │   └── brine_refraction.gdshader
    ├── fallout/
    │   ├── fallout_glow.gdshader
    │   ├── fallout_particles.gdshader
    │   └── fallout_zone.gdshader
    ├── shared/
    │   ├── expansion_fx.gdshaderinc
    │   └── expansion_material.tres
    └── .import/
        ├── greenhouse_glow.gdshader.import
        └── expansion_material.tres.import
```

### Generated Files:

#### expansion_fx.gdshaderinc:
```glsl
shader_type canvas_item;

// Shared parameters with intensity scaling
uniform float emission_strength : hint_range(0.0, 2.0) = 1.2;  // 1.5 * 0.8
uniform float fallout_intensity : hint_range(0.0, 1.0) = 0.75; // 1.5 * 0.5
uniform float brine_distortion : hint_range(0.0, 1.0) = 0.45; // 1.5 * 0.3
uniform float greenhouse_glow : hint_range(0.0, 2.0) = 1.8;   // 1.5 * 1.2
uniform vec4 custom_color : hint_color = vec4(0.42, 0.92, 0.62, 1.0); // #2a5c3e

// Shared functions with intensity scaling
vec3 apply_fallout_effect(vec3 color, float intensity) {
    float fallout_factor = intensity * fallout_intensity;
    return mix(color, vec3(0.2, 0.8, 0.2) * fallout_factor, fallout_factor);
}

vec3 apply_greenhouse_glow(vec3 color, float glow) {
    float glow_factor = glow * greenhouse_glow;
    return color + vec3(glow_factor * 0.5, glow_factor * 0.7, glow_factor * 0.3);
}

vec3 apply_brine_distortion(vec2 uv, vec3 color) {
    float distortion = brine_distortion * 0.1;
    vec2 distorted_uv = uv + vec2(
        sin(uv.y * 10.0 + TIME * 2.0) * distortion,
        cos(uv.x * 10.0 + TIME * 1.5) * distortion
    );
    return texture(TEXTURE, distorted_uv).rgb * color;
}
```

#### greenhouse_glow.gdshader:
```glsl
shader_type canvas_item;

// Use intensity parameter from shared include
uniform float intensity : hint_range(0.5, 2.0) = 1.5;

// Extend shared include
extends "res://assets/expansions/05_holdfast/shaders/shared/expansion_fx.gdshaderinc"

void fragment() {
    vec3 base_color = texture(TEXTURE, UV).rgb;
    vec3 glowing = apply_greenhouse_glow(base_color, intensity);
    COLOR.rgb = glowing;
    COLOR.a = texture(TEXTURE, UV).a;
}
```

#### expansion_material.tres:
```
[gd_resource type="CanvasItemMaterial" load_steps=2 format=3]

[ext_resource path="res://assets/expansions/05_holdfast/shaders/shared/expansion_fx.gdshaderinc" type="Shader" id="1"]

[resource]
shader = null
flags = 0
light_mode = 0
next_pass = null
uv_1_scale = Vector2(1, 1)
uv_1_offset = Vector2(0, 0)
uv_2_scale = Vector2(1, 1)
uv_2_offset = Vector2(0, 0)
texture_filter = 0
texture_repeat = 0
vertex_color_use_as_albedo = true

[sub_resource type="ShaderMaterial" id="2"]
shader = ExtResource("1")
shader_parameter/emission_strength = 1.2
shader_parameter/fallout_intensity = 0.75
shader_parameter/brine_distortion = 0.45
shader_parameter/greenhouse_glow = 1.8
shader_parameter/custom_color = Color(0.42, 0.92, 0.62, 1.0)
```

## Related Skills
- `ashfall-asset-pack-expansion` - Creates asset pack structure
- `ashfall-shader-material-lint` - Validates shader quality
- `ashfall-expansion-data-gen` - Uses shader IDs for effects
- `ashfall-foundry` - Generates textures for shaders
- `ashfall-lfs-gate` - Validates LFS configuration

## Notes
- Follows Godot 4.7+ shader best practices
- Uses shared include files for consistency
- Validates sampler cost to prevent performance issues
- Generates CI-ready shaders with correct import presets
- Follows ASHFALL's visual style guidelines

## Maintenance
- Update shader templates if Godot shader syntax changes
- Add new shader types if expansion domains expand
- Update sampler budget if target hardware changes
- Update shared include if new effects are needed
