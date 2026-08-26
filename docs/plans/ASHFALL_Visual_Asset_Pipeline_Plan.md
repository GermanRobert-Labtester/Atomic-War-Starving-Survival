# ASHFALL: Atomic War — Starving Survival
## Visual / Shader / Audio Expansion & 7-Day Asset Production Sprint

Engine target: **Godot 4.7.1 (.NET 8, C# 12)**. All shaders written as Godot `CanvasItem` GLSL (`shader_type canvas_item;`). All paths respect the existing repo layout (`src/UI/`, `src/Shaders/`, `Assets/StreamingAssets/Data/`).

---

## 1. TECHNICAL ART & SHADER ARCHITECTURE

### 1.1 File & folder convention
```
src/Shaders/
  Weather/
    ashfall_particles.gdshader
    black_rain_streaks.gdshader
    rad_hail_distortion.gdshader
    emp_static_flicker.gdshader
    biofog_volumetric.gdshader
  Terminal/
    crt_curvature_scanline.gdshader
    phosphor_bleed.gdshader
  Lighting/
    amber_emergency_vignette.gdshader
    blackout_falloff.gdshader
```
Each shader is a full-screen `CanvasLayer` quad (`ColorRect` covering viewport) bound via a `ShaderMaterial`, driven by a `WeatherFXController.cs` script that reads `WeatherAtmosphereMap.cs` / `WeatherKind.cs` and pushes uniforms (`weather_intensity`, `wind_dir`, `time`) every `_Process`.

### 1.2 Atmospheric screen-space shaders

**Ashfall particle drift** (`ashfall_particles.gdshader`) — scrolling multi-octave value-noise mapped to a soot-grey sprite, additive-subtractive blend, no bloom.
```glsl
shader_type canvas_item;
uniform float intensity : hint_range(0.0,1.0) = 0.4;
uniform vec2 wind_dir = vec2(0.15, 0.9);
uniform float time_scale = 0.6;
uniform sampler2D noise_tex : filter_linear_mipmap, repeat_enable;

float hash(vec2 p){ return fract(sin(dot(p,vec2(41.3,289.1)))*43758.5453); }

void fragment(){
    vec2 uv = SCREEN_UV;
    vec2 flow = uv + wind_dir * TIME * time_scale;
    float n1 = texture(noise_tex, flow*2.0).r;
    float n2 = texture(noise_tex, flow*5.3 + vec2(3.1,1.7)).r;
    float soot = clamp((n1*0.65 + n2*0.35) - 0.35, 0.0, 1.0) * intensity;
    vec3 ash_color = vec3(0.32,0.30,0.28); // cold cast-iron grey, never warm
    COLOR = vec4(ash_color, soot*0.55);
}
```

**Black Rain oily streaks** (`black_rain_streaks.gdshader`) — vertical UV-shear streak function, darkened contaminant tint (no glow, desaturated near-black-brown).
```glsl
shader_type canvas_item;
uniform float streak_speed : hint_range(0.0,4.0) = 1.8;
uniform float density : hint_range(0.0,1.0) = 0.5;
uniform sampler2D noise_tex : filter_linear_mipmap, repeat_enable;

void fragment(){
    vec2 uv = SCREEN_UV;
    float col_id = floor(uv.x * 180.0);
    float offset = fract(sin(col_id*12.98)*4375.5);
    float y = fract(uv.y + TIME*streak_speed*0.1 + offset);
    float streak = smoothstep(0.0,0.03,y) * smoothstep(0.09,0.03,y);
    float mask = texture(noise_tex, vec2(col_id*0.01, 0.0)).r;
    float alpha = streak * step(1.0-density, mask) * 0.5;
    vec3 tint = vec3(0.05,0.045,0.03);
    COLOR = vec4(tint, alpha);
}
```

**Rad-Hail screen distortion** (`rad_hail_distortion.gdshader`) — refracts `SCREEN_TEXTURE` with impact-ripple UV offset + faint chromatic fringing to sell ionizing static, plus falling grey pellet sprites drawn by a separate `GPUParticles2D`.
```glsl
shader_type canvas_item;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear;
uniform float distort_amount : hint_range(0.0,0.05) = 0.012;

void fragment(){
    vec2 uv = SCREEN_UV;
    float ripple = sin(uv.y*90.0 + TIME*20.0) * sin(uv.x*60.0 - TIME*14.0);
    vec2 offset = vec2(ripple) * distort_amount;
    vec3 base = textureLod(SCREEN_TEXTURE, uv + offset, 0.0).rgb;
    vec3 fringed_r = textureLod(SCREEN_TEXTURE, uv + offset*1.4, 0.0).rgb;
    base.r = mix(base.r, fringed_r.r, 0.3);
    COLOR = vec4(base, 1.0);
}
```

**EMP static flicker** (`emp_static_flicker.gdshader`) — full desaturation pulse + horizontal tearing bands driven by a `pulse_trigger` uniform fired from `AudioEventBridge.cs` on EMP Storm onset.
```glsl
shader_type canvas_item;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear;
uniform float pulse_trigger : hint_range(0.0,1.0) = 0.0;

float rand(vec2 c){ return fract(sin(dot(c, vec2(12.9,78.2)))*43758.5); }

void fragment(){
    vec2 uv = SCREEN_UV;
    float tear = step(0.985, rand(vec2(floor(uv.y*260.0), floor(TIME*30.0))));
    uv.x += tear * (rand(vec2(floor(uv.y*260.0), TIME))-0.5)*0.08*pulse_trigger;
    vec3 base = textureLod(SCREEN_TEXTURE, uv, 0.0).rgb;
    float gray = dot(base, vec3(0.3,0.59,0.11));
    base = mix(base, vec3(gray), pulse_trigger*0.7);
    float noise_line = rand(vec2(uv.y*400.0, TIME*60.0));
    base += vec3(noise_line) * pulse_trigger * 0.15;
    COLOR = vec4(base,1.0);
}
```

**Bio-Fog volumetric dissipation** (`biofog_volumetric.gdshader`) — low-lying screen-bottom-anchored FBM fog, sickly desaturated olive-grey, alpha weighted by screen-space Y so it hugs the floor.
```glsl
shader_type canvas_item;
uniform sampler2D noise_tex : filter_linear_mipmap, repeat_enable;
uniform float fog_height : hint_range(0.0,1.0) = 0.35;
uniform float density : hint_range(0.0,1.0) = 0.5;

void fragment(){
    vec2 uv = SCREEN_UV;
    vec2 flow = uv*vec2(1.5,3.0) + vec2(TIME*0.02, TIME*0.01);
    float fbm = texture(noise_tex, flow).r*0.6 + texture(noise_tex, flow*2.1).r*0.4;
    float floor_mask = smoothstep(fog_height, 0.0, uv.y);
    float alpha = fbm * floor_mask * density;
    vec3 fog_color = vec3(0.34,0.36,0.30);
    COLOR = vec4(fog_color, alpha*0.6);
}
```

### 1.3 CRT / terminal shader (bound to `RadioPanel.cs`, `VerdictDashboardPanel.cs`, `PowerGridPanel.cs`)

`crt_curvature_scanline.gdshader` combines barrel curvature, scanlines, phosphor bleed and refresh flicker in one pass so each terminal `Control` panel only needs one `ShaderMaterial`.
```glsl
shader_type canvas_item;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear;
uniform float curvature : hint_range(0.0,0.5) = 0.15;
uniform float scanline_intensity : hint_range(0.0,1.0) = 0.35;
uniform float bleed_amount : hint_range(0.0,1.0) = 0.25;
uniform vec3 phosphor_tint = vec3(0.45, 0.85, 0.35); // amber/green CRT
uniform float flicker_speed = 6.0;

vec2 barrel(vec2 uv){
    vec2 c = uv*2.0-1.0;
    float r2 = dot(c,c);
    c *= 1.0 + curvature*r2;
    return c*0.5+0.5;
}

void fragment(){
    vec2 uv = barrel(SCREEN_UV);
    if (uv.x<0.0||uv.x>1.0||uv.y<0.0||uv.y>1.0){ COLOR=vec4(0,0,0,1); return; }
    vec3 col = textureLod(SCREEN_TEXTURE, uv, 0.0).rgb;
    vec3 bleed = textureLod(SCREEN_TEXTURE, uv+vec2(0.0015,0.0), 0.0).rgb;
    col = mix(col, bleed, bleed_amount*0.4);
    float scan = sin(uv.y*800.0)*0.5+0.5;
    col *= 1.0 - scan*scanline_intensity*0.5;
    float flicker = 0.97 + 0.03*sin(TIME*flicker_speed) + 0.01*sin(TIME*113.0);
    col *= flicker;
    float lum = dot(col, vec3(0.3,0.59,0.11));
    col = mix(vec3(lum), phosphor_tint*lum, 0.55);
    COLOR = vec4(col,1.0);
}
```
Bind in C#: `radioPanelRoot.Material = (ShaderMaterial)GD.Load<Shader>("res://src/Shaders/Terminal/crt_curvature_scanline.gdshader")`, set per-panel `phosphor_tint` — amber `(0.85,0.55,0.2)` for `PowerGridPanel.cs`, green `(0.45,0.85,0.35)` for `RadioPanel.cs`.

### 1.4 Dynamic lighting & vignette

`amber_emergency_vignette.gdshader` — for blackout / low-wattage rooms: a `CanvasModulate`-layer radial falloff from door/terminal light sources plus a global desaturating vignette.
```glsl
shader_type canvas_item;
uniform float light_radius : hint_range(0.0,1.0) = 0.35;
uniform vec2 light_center = vec2(0.5,0.4);
uniform vec3 amber_color = vec3(0.9,0.55,0.15);
uniform float blackout_level : hint_range(0.0,1.0) = 0.0; // 1.0 = full blackout

void fragment(){
    vec2 uv = SCREEN_UV;
    float d = distance(uv, light_center);
    float falloff = smoothstep(light_radius, 0.0, d);
    float vignette = smoothstep(0.9,0.4, distance(uv, vec2(0.5)));
    vec3 base_dark = vec3(0.02,0.02,0.025);
    vec3 lit = mix(base_dark, amber_color*0.6, falloff*vignette);
    lit = mix(lit, base_dark*0.3, blackout_level);
    COLOR = vec4(lit, 1.0 - falloff*0.85*(1.0-blackout_level));
}
```
Integration hook: attach to a full-viewport `ColorRect` child under each shelter room's `Control` root in `src/UI/`, toggled by `PowerGridPanel.cs` grid-load events (brownout → `blackout_level` interpolates 0→1 over 1.5s).

---

## 2. UI & SPRITE DESIGN SYSTEM

### 2.1 9-patch texture schemas
All 9-patch source art authored at **192×192px**, exported as PNG-8 indexed (≤32 colors) for deterministic 2D batching. Margins below are in source pixels (scale 1×; Godot `NinePatchRect` patch margins set identically).

| Frame type | Canvas | Margin (L/T/R/B) | Palette | Target |
|---|---|---|---|---|
| Riveted steel border | 192×192 | 24/24/24/24 | `#2b2b28,#4a4a44,#181816,#6b6b5f` (rivet highlight) | `src/UI/Frames/steel_frame_9p.png` |
| Heavy industrial frame (thick) | 256×256 | 40/40/40/40 | `#232320,#3d3d38,#12100e` | `src/UI/Frames/industrial_heavy_9p.png` |
| Paper ledger | 160×160 | 16/16/16/16 | `#d8cba8,#b8a878,#8a7a52` + noise overlay | `src/UI/Frames/paper_ledger_9p.png` |
| Terminal frame (CRT bezel) | 192×160 | 20/28/20/24 | `#1a1a18,#0e0e0c,phosphor accent` | `src/UI/Frames/terminal_bezel_9p.png` |

Production command (ImageMagick slice validation, ensures corner tiles are non-stretched):
```bash
for f in steel_frame_9p industrial_heavy_9p paper_ledger_9p terminal_bezel_9p; do
  convert "raw/${f}.png" -colors 32 -dither FloydSteinberg PNG8:"src/UI/Frames/${f}.png"
done
```

### 2.2 Survivor portrait & badging matrix (`UiAssetManifest.cs`)

Layered compositing order (bottom → top), each layer a separate PNG at **256×256**, straight alpha, additive layering resolved at runtime by `UiAssetManifest.cs`:

1. `base_portrait_[id].png` — Gemini-generated survivor headshot, Picsart-cutout, neutral lighting.
2. `overlay_gasmask_[variant].png` — worn rubber/canister gas mask, 3 wear variants (clean/scuffed/cracked-lens).
3. `overlay_burns_[severity].png` — chemical/thermal burn decals, severity 1–3, blended `multiply` at 0.6–1.0 alpha.
4. `overlay_ars_pallor.png` — acute radiation syndrome desaturation + patchy hair-loss mask, blended `multiply`, driven by `RadiationDoseTracker` value.
5. `overlay_frostbite.png` — blue-grey extremity/facial mottling, `multiply` blend.
6. `overlay_bandage_[location].png` — head/arm/torso surgical wrap, `normal` blend on top.

Manifest schema addition (`Assets/StreamingAssets/Data/UiAssetManifest.json`):
```json
{
  "survivorId": "SUR_0042",
  "basePortrait": "portraits/base/SUR_0042.png",
  "overlays": [
    { "type": "gasmask", "variant": "scuffed", "opacity": 1.0, "sortOrder": 10 },
    { "type": "ars_pallor", "intensity": 0.72, "sortOrder": 20 },
    { "type": "bandage_head", "opacity": 1.0, "sortOrder": 30 }
  ]
}
```
All overlay PNGs are pre-masked to the exact base-portrait UV silhouette in Picsart before export, so no runtime masking cost — Godot just stacks `TextureRect` nodes.

### 2.3 Item & iconography standard (16 categories)

- **Resolution:** 64×64 canonical (2× export at 128×128 for high-DPI, downsampled via ImageMagick Lanczos then re-quantized).
- **Color depth:** PNG-8, ≤16 colors per icon, forced shared 64-color master palette across all categories for atlas coherence.
- **Palette convention:** desaturated base hue per category + one accent:
  - Medical ampoules — cool slate `#8a9a94` + red-cross accent `#a83232`.
  - Munitions — gunmetal `#4a4a48` + brass accent `#8a7238`.
  - Foundry ingots — soot-grey `#3a3833` + molten accent `#b5602a` (used sparingly, edge-only, never glowing fill).
  - Raw scrap — rust-brown `#6b4a2e`.
  - Crops — muted olive `#6a6f42`.
- **Silhouette rule:** every icon must read correctly at 32×32 in pure black silhouette (validated via ImageMagick threshold pass, see Day 4).
- **Border:** 1px dark outline (`#111110`) baked into art, consistent across all 16 categories for icon-grid cohesion.
- **Target:** `Assets/StreamingAssets/Data/Icons/[category]/[item_id].png` referenced by `ItemCatalog.json` entries via an `iconPath` field.

---

## 3. PROCEDURAL & SYNTHESIZED SFX SUITE

All output: 44.1kHz/16-bit mono WAV normalized to −16 LUFS integrated (UI) / −12 LUFS (impact FX), stored in `Assets/StreamingAssets/Audio/SFX/[category]/`.

**Geiger counter clicks, graded by mSv band** — impulse click synthesized from filtered noise burst, band controls click density (Poisson-ish via random silence gaps):
```bash
# base click transient
sox -n click_base.wav synth 0.006 noise band 3500 2500 fade 0 0.006 0.004 gain -6

gen_band () {
  local band=$1 cps=$2 outfile=$3
  python3 - "$cps" "$outfile" <<'PY'
import sys, subprocess, random
cps, out = float(sys.argv[1]), sys.argv[2]
dur = 10.0
t = 0.0
events = []
while t < dur:
    events.append(t)
    t += random.expovariate(cps)
with open("/tmp/geiger_times.txt","w") as f:
    f.write("\n".join(f"{e:.4f}" for e in events))
PY
  sox -n -r 44100 "$outfile" trim 0 10 2>/dev/null
  while read -r ts; do
    sox click_base.wav /tmp/pad.wav pad "${ts}" 0
    sox -m "$outfile" /tmp/pad.wav /tmp/mix.wav && mv /tmp/mix.wav "$outfile"
  done < /tmp/geiger_times.txt
}
gen_band 0.5 3   geiger_band1_background.wav   # <0.1 mSv/h
gen_band 0.5 12  geiger_band2_elevated.wav      # 0.1-1 mSv/h
gen_band 0.5 40  geiger_band3_dangerous.wav     # 1-10 mSv/h
gen_band 0.5 90  geiger_band4_lethal.wav        # >10 mSv/h
```
Bound to `AudioEventBridge.cs` as a looping `AudioStreamPlayer` whose band swaps on `RadiationZone` threshold crossing.

**Shortwave radio tuner** — three linked assets:
```bash
# tuning whistle: swept sine through heterodyne range
sox -n tuner_whistle.wav synth 2.2 sine 400-2200 fade 0.1 2.2 0.3

# static burst: bandpass-filtered white noise emulating shortwave hiss
sox -n radio_static.wav synth 3.0 whitenoise sinc 300-3400 gain -4 fade 0.05 3.0 0.3

# SNR lock tone: clean dual-tone confirming signal acquisition (utilitarian, not musical)
sox -n snr_lock.wav synth 0.4 sine 1000 sine 1500 fade 0.02 0.4 0.05 gain -8
```

**Heavy airlock pneumatic hiss + bolt slam:**
```bash
# pneumatic hiss: filtered noise with slow amplitude envelope
sox -n airlock_hiss.wav synth 1.8 whitenoise sinc 200-1200 gain -3 fade 0.3 1.8 0.6

# iron bolt slam: layered low-frequency thump + metallic clank (needs two synth passes mixed)
sox -n bolt_thump.wav synth 0.3 sine 60 fade 0 0.3 0.25 gain -2
sox -n bolt_clank.wav synth 0.15 square 900 fade 0 0.15 0.1 gain -10 highpass 500
sox -m bolt_thump.wav bolt_clank.wav airlock_bolt_slam.wav
sox airlock_hiss.wav airlock_bolt_slam.wav airlock_full_sequence.wav  # concatenate: hiss then slam
```

**Silent Foundry (furnace roar, crucible tap, slag quench):**
```bash
# cupola furnace roar: low rumbling filtered noise, sustained loop-ready
sox -n furnace_roar.wav synth 6.0 brownnoise sinc 40-800 gain -2 fade 0.5 6.0 0.5

# crucible tapping hiss: short bright hiss burst (molten pour)
sox -n crucible_tap.wav synth 1.2 whitenoise sinc 1500-6000 gain -6 fade 0.02 1.2 0.4

# slag quench: hiss burst + steam crackle tail, pitch-bent down
sox -n quench_hiss.wav synth 0.8 whitenoise sinc 2000-8000 gain -4 fade 0 0.8 0.5
sox quench_hiss.wav slag_quench.wav bend 0,-1200,0.8
```

**Tactical ballistics (crack, ricochet, jam):**
```bash
# supersonic crack: extremely short high-passed transient (sonic boom impulse, not gunshot bang)
sox -n bullet_crack.wav synth 0.008 whitenoise highpass 4000 fade 0 0.008 0.006 gain +2

# ricochet: crack + pitched metallic resonance sweep down
sox -n ricochet_ring.wav synth 0.35 sine 3000-800 fade 0 0.35 0.3 gain -8
sox -m bullet_crack.wav ricochet_ring.wav ricochet_full.wav

# feed jam click: dry mechanical double-click, very short, no reverb
sox -n jam_click.wav synth 0.02 square 2000 fade 0 0.02 0.015 gain -10
sox jam_click.wav jam_click.wav feed_jam_double.wav  # duplicate = double-click cadence
```

All FX get a final batch normalize pass:
```bash
for f in Assets/StreamingAssets/Audio/SFX/**/*.wav; do
  sox "$f" -r 44100 "${f%.wav}_norm.wav" gain -n -1 && mv "${f%.wav}_norm.wav" "$f"
done
```

---

## 4. 7-DAY ASSET PRODUCTION SPRINT

### Day 1 — Weather & Atmosphere Shaders + Ash Particle Textures
**Target assets:** 6 `.gdshader` files (from Section 1.2/1.4); 4 seamless noise textures (512×512, PNG, greyscale) for ash/fog/rain sampling → `Assets/Textures/Noise/`.
**Pipeline:** ChatGPT authors/refines all `.gdshader` GLSL (iterate on compile errors pasted back) → Gemini generates 4 reference noise/texture concept plates ("soot particle field, top-down, monochrome, industrial grit, no color") → Picsart desaturates + levels-adjusts → ImageMagick converts to tileable greyscale + repacks as seamless.
**Commands:**
```bash
mkdir -p Assets/Textures/Noise
convert gemini_noise_raw.png -colorspace Gray -level 10%,90% -resize 512x512 \
  Assets/Textures/Noise/ash_fbm_01.png
convert Assets/Textures/Noise/ash_fbm_01.png -filter Gaussian -blur 0x1 \
  -define png:compression-level=9 Assets/Textures/Noise/ash_fbm_01_seamless.png
```
**Godot hook:** shaders placed in `src/Shaders/Weather/`, noise textures assigned to their `noise_tex` uniform via `.tres` `ShaderMaterial` resources referenced by `WeatherFXController.cs`, itself driven by `Assets/Ashfall.Core/World/WeatherAtmosphereMap.cs`.

### Day 2 — CRT Terminal Shader + Terminal 9-Patch Frames
**Target assets:** `crt_curvature_scanline.gdshader` (finalized); 1 terminal bezel 9-patch (192×160 PNG-8) → `src/UI/Frames/terminal_bezel_9p.png`.
**Pipeline:** ChatGPT writes/tunes shader → Gemini generates terminal bezel concept art (rusted steel CRT housing, amber/green dial) → Picsart crops to exact 9-patch canvas + grades cool slate/amber palette → ImageMagick slices/validates 9-patch margins and quantizes.
**Commands:**
```bash
convert picsart_bezel.png -crop 192x160+0+0 +repage bezel_raw.png
convert bezel_raw.png -colors 24 -dither FloydSteinberg PNG8:src/UI/Frames/terminal_bezel_9p.png
identify -verbose src/UI/Frames/terminal_bezel_9p.png | grep -i "geometry\|colors"
```
**Godot hook:** `ShaderMaterial` assigned in `RadioPanel.cs`, `VerdictDashboardPanel.cs`, `PowerGridPanel.cs` `_Ready()`; 9-patch assigned to each panel's `NinePatchRect` background with margins 20/28/20/24.

### Day 3 — Survivor Base Portraits (Batch 1: 60 of 174)
**Target assets:** 60 base portraits, 256×256 PNG (RGBA, cutout), → `Assets/StreamingAssets/Data/Portraits/base/`.
**Pipeline:** ChatGPT builds a portrait prompt template varying age/occupation/fatigue tags → Gemini batch-renders (multi-modal, 60 variations) → Picsart background-removal + cutout (batch mode) → ImageMagick standardizes canvas/alpha.
**Prompt template (ChatGPT-authored, fed to Gemini):**
```
"Cold War-era Eastern Bloc survivor portrait, [age]-year-old [occupation], gaunt exhausted expression,
utilitarian grey/olive clothing, harsh single-source overhead lighting, desaturated documentary photo style,
grainy 1980s film, neutral studio background for cutout, no fantasy elements, no glow, no color grading."
```
**Commands:**
```bash
for f in raw_portraits/*.png; do
  convert "$f" -resize 256x256^ -gravity center -extent 256x256 \
    -background none PNG32:"Assets/StreamingAssets/Data/Portraits/base/$(basename "$f")"
done
```
**Godot hook:** referenced by `UiAssetManifest.json` `basePortrait` field, consumed by survivor roster UI panels in `src/UI/`.

### Day 4 — Item Iconography (16 categories, ~10 icons/category = 160 icons)
**Target assets:** 160 icons, 64×64 PNG-8 → `Assets/StreamingAssets/Data/Icons/[category]/`.
**Pipeline:** ChatGPT writes procedural SVG bases per category (geometric silhouettes matching palette table in Section 2.3) → batch-rasterize → ImageMagick quantize/dither + silhouette validation.
**Commands:**
```bash
mkdir -p Assets/StreamingAssets/Data/Icons/{medical,munitions,foundry,scrap,crops}
for svg in svg_icons/*.svg; do
  base=$(basename "$svg" .svg)
  convert -background none -density 300 "$svg" -resize 128x128 "tmp/${base}_128.png"
  convert "tmp/${base}_128.png" -resize 64x64 -colors 16 -dither FloydSteinberg \
    PNG8:"Assets/StreamingAssets/Data/Icons/${base}.png"
  # silhouette legibility check at 32x32
  convert "Assets/StreamingAssets/Data/Icons/${base}.png" -resize 32x32 -threshold 50% \
    "tmp/${base}_silhouette_check.png"
done
```
**Godot hook:** `iconPath` field added to each entry across the relevant JSON catalogs in `Assets/StreamingAssets/Data/` (e.g., `ItemCatalog.json`, `MedicalCatalog.json`).

### Day 5 — Wasteland Map Nodes & Journal Plates (Batch 1: 40 nodes + 20 journal pages)
**Target assets:** 40 map node illustrations (512×384 PNG) → `Assets/StreamingAssets/Data/MapNodes/`; 20 journal/codex parchment plates (768×1024 PNG) → `src/Journal/Plates/`.
**Pipeline:** Gemini multi-modal ideation (location illustration) → Picsart LUT grading (cool slate/amber duotone) + paper-decay/scratch overlay for journal plates → ImageMagick batch downsampling and format conversion.
**Commands:**
```bash
mogrify -path Assets/StreamingAssets/Data/MapNodes -resize 512x384 -colors 64 -format png raw_nodes/*.png
for f in raw_journal/*.png; do
  convert "$f" -resize 768x1024 -modulate 90,60,100 \
    \( paper_grain_overlay.png -resize 768x1024 \) -compose multiply -composite \
    "src/Journal/Plates/$(basename "$f")"
done
```
**Godot hook:** map nodes bound to world-map `Control` scenes referencing node IDs in `Assets/StreamingAssets/Data/MapCatalog.json`; journal plates loaded by `src/Journal/JournalBookUI.cs` per `JournalEntry` document ID (of 196).

### Day 6 — SFX Suite Generation & Batch Processing
**Target assets:** ~30 SFX WAVs across Geiger/radio/airlock/foundry/ballistics categories → `Assets/StreamingAssets/Audio/SFX/[category]/`.
**Pipeline:** ChatGPT authors SoX/FFmpeg recipes (Section 3) → execute locally → FFmpeg batch-applies radio bandpass simulation to any diegetic-radio-filtered lines/SFX → SoX normalizes all output.
**Commands:**
```bash
mkdir -p Assets/StreamingAssets/Audio/SFX/{geiger,radio,airlock,foundry,ballistics}
# apply 300Hz-3.4kHz shortwave bandpass to any radio-transmitted SFX
ffmpeg -i input_dialogue.wav -af "highpass=f=300,lowpass=f=3400,acompressor=threshold=-18dB:ratio=4" \
  -ar 44100 Assets/StreamingAssets/Audio/SFX/radio/dialogue_radio_filtered.wav
# batch normalize
for f in Assets/StreamingAssets/Audio/SFX/**/*.wav; do
  sox "$f" -r 44100 "${f%.wav}_n.wav" gain -n -1 && mv "${f%.wav}_n.wav" "$f"
done
```
**Godot hook:** all files registered as `AudioStream` resources triggered by `Assets/Ashfall.Core/AudioEventBridge.cs` event map (e.g., `RadiationZoneEnter`, `AirlockCycle`, `WeaponFire`).

### Day 7 — Survivor Badging Overlays, Emergency Lighting, Full Integration Pass
**Target assets:** 6 overlay sets (gasmask ×3, burns ×3, ars_pallor, frostbite, bandage ×3 locations) at 256×256 → `Assets/StreamingAssets/Data/Portraits/overlays/`; final `amber_emergency_vignette.gdshader` wired into all shelter rooms.
**Pipeline:** ChatGPT authors overlay compositing spec + `UiAssetManifest.json` schema updates → Gemini/Picsart produce masked overlay art aligned to base-portrait UV space (Picsart precision cutout) → ImageMagick alpha-thresholds and packs overlay atlas → integration test pass in Godot (compile shaders, verify 9-patch margins, verify JSON catalog `iconPath`/`basePortrait` resolution).
**Commands:**
```bash
convert overlay_gasmask_raw.png -alpha extract -threshold 30% overlay_gasmask_mask.png
convert overlay_gasmask_raw.png overlay_gasmask_mask.png -alpha off -compose CopyOpacity -composite \
  Assets/StreamingAssets/Data/Portraits/overlays/gasmask_scuffed.png
# atlas pack all overlays for batched draw calls
montage Assets/StreamingAssets/Data/Portraits/overlays/*.png -tile 4x2 -geometry 256x256+0+0 \
  Assets/StreamingAssets/Data/Portraits/overlays/_atlas_overlays.png
```
**Godot hook:** `UiAssetManifest.cs` reads `overlays[]` array per survivor and stacks `TextureRect` nodes in sort order; emergency vignette `ColorRect` added as child of shelter-room `Control` roots, toggled by `PowerGridPanel.cs` brownout/blackout state.

---

## Summary Table: Daily Output vs. Directory Bindings

| Day | Primary Output | Count | Target Directory |
|---|---|---|---|
| 1 | Weather shaders + noise textures | 6 shaders + 4 textures | `src/Shaders/Weather/`, `Assets/Textures/Noise/` |
| 2 | CRT shader + terminal 9-patch | 1 shader + 1 frame | `src/Shaders/Terminal/`, `src/UI/Frames/` |
| 3 | Survivor base portraits | 60 | `Assets/StreamingAssets/Data/Portraits/base/` |
| 4 | Item icons | 160 | `Assets/StreamingAssets/Data/Icons/` |
| 5 | Map nodes + journal plates | 40 + 20 | `Assets/StreamingAssets/Data/MapNodes/`, `src/Journal/Plates/` |
| 6 | SFX suite | ~30 | `Assets/StreamingAssets/Audio/SFX/` |
| 7 | Portrait overlays + lighting shader | 6 sets | `Assets/StreamingAssets/Data/Portraits/overlays/`, `src/Shaders/Lighting/` |

This is a rolling pipeline — Days 3, 4, and 5 batches (60 portraits, 160 icons, 40 nodes) represent only the Sprint-1 tranche; the remaining 114 survivors, additional icons, and 221 map nodes follow the identical tool pipeline in subsequent weekly sprints.
