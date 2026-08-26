# GAME CREATION APPLICATIONS & TOOLCHAINS
## Comprehensive Inventory of Installed Game Development Software
**System:** Linux x86_64 | **Generated:** 2026-08-18

---

## 1. Game Engines & Runtimes

| Application / Tool | Version / Flavor | Binary Path / Command | Primary Use in Game Creation |
|---|---|---|---|
| **Godot Engine (.NET/Mono)** | 4.7.1 Stable Mono | `/home/robertsrff/Applications/Godot/mono/Godot_v4.7.1-stable_mono_linux_x86_64/Godot_v4.7.1-stable_mono_linux.x86_64` (alias `godot`, `godot-mono`) | 2D & 3D Game Engine with C# (.NET 8.0/9.0) and GDScript support. Authoritative engine for ASHFALL. |
| **Godot Engine (Standard)** | 4.7.1 Stable Standard | `/home/robertsrff/Applications/Godot/standard/Godot_v4.7.1-stable_linux.x86_64` (alias `godot-standard`) | Lightweight GDScript / C++ GDExtension game engine build. |
| **Unity Hub** | 3.20.1 (Flatpak) | `flatpak run com.unity.UnityHub` | Management suite for Unity editor versions, project templates, and package modules. |

---

## 2. 2D Art, Sprites, Textures & Graphic Design

| Application / Tool | Version / Details | Binary Path / Command | Primary Use in Game Creation |
|---|---|---|---|
| **Pixelorama** | 1.2 (Flatpak) | `flatpak run com.orama_interactive.Pixelorama` | Dedicated 2D pixel art editor, animated spritesheet creation, custom color palettes, and frame-by-frame animation tool. |
| **Krita** | 5.3.2.1 (Flatpak) | `flatpak run org.kde.krita` | Digital painting workstation, concept art illustration, seamless texture painting, and raster 2D animation. |
| **ImageMagick** | 7.1.2-27 Q16-HDRI | `/usr/bin/magick`, `/usr/bin/convert` | Batch texture resizing, sprite sheet packing, format conversion (PNG/JPG/WEBP), color channel extraction, and CI asset processing. |
| **FontTools & TTX** | Latest | `~/.local/bin/ttx`, `fonttools`, `pyftsubset` | Game typography pipeline: font conversion, glyph subsetting, XML table editing, and TrueType/OpenType font optimization. |
| **PNG Suite CLI Tools** | Latest | `~/.local/bin/prichunkpng`, `pricolpng`, `priditherpng`, `priforgepng`, `pripalpng` | Low-level PNG chunk modification, dithering, palette quantization, and transparent sprite optimization. |

---

## 3. 3D Modeling, Texturing, Rigging & Rendering

| Application / Tool | Version / Details | Binary Path / Command | Primary Use in Game Creation |
|---|---|---|---|
| **Blender** | 5.2.0 LTS | `/usr/bin/blender` | Complete 3D pipeline: polygonal modeling, sculpting, UV mapping, PBR material creation, character rigging, skeletal animation, physics simulation, and glTF / OBJ / FBX export for Godot & Unity. |

---

## 4. Level Design & World Building

| Application / Tool | Version / Details | Binary Path / Command | Primary Use in Game Creation |
|---|---|---|---|
| **Tiled Map Editor** | 1.12.2 | `/usr/bin/tiled` | 2D tilemap design (orthogonal, isometric, hexagonal grids), tile collision layer tagging, custom object properties, and direct export to Godot/JSON map formats. |

---

## 5. Audio, Music Production & Media Processing

| Application / Tool | Version / Details | Binary Path / Command | Primary Use in Game Creation |
|---|---|---|---|
| **EasyEffects** | 8.2.8 | `/usr/bin/easyeffects` | Real-time audio DSP effects, multi-band equalization, dynamic compression, limiting, and spatial sound tuning for game audio monitoring. |
| **FFmpeg & FFprobe** | 8.1.2 | `/usr/bin/ffmpeg`, `/usr/bin/ffprobe` | Game sound and video encoding, multi-channel audio conversion (OGG Vorbis, WAV, MP3, FLAC), video cutscene encoding, and audio format batch pipeline. |
| **Mutagen & Audio Tag Tools** | Latest | `~/.local/bin/mid3v2`, `mutagen-inspect`, `moggsplit` | Audio stream splitting, OGG metadata tagging, and sound asset cataloging. |

---

## 6. IDEs, Code Editors & AI Development Environments

| Application / Tool | Version / Details | Binary Path / Command | Primary Use in Game Creation |
|---|---|---|---|
| **Cursor IDE** | 3.16.17 | `/usr/bin/cursor`, `~/.local/bin/cursor-cli` | AI-accelerated IDE for gameplay programming (C#, GDScript, Python, shaders, JSON catalogs). |
| **Zed Editor** | 1.15.0 | `/home/robertsrff/.local/zed.app/bin/zed` | Ultra-fast GPU-accelerated code editor with multi-language support (Rust, C#, C++, Python, JSON). |
| **Antigravity IDE** | Latest | `~/.local/bin/antigravity-ide`, `antigravity` | Agentic AI development workstation and game architecture orchestrator. |
| **Void AI Editor** | Latest | `/usr/share/void/void` | AI-assisted code editor and developer workspace. |
| **ZCode** | 3.7.7 (AppImage) | `/home/robertsrff/Applications/ZCode-3.7.7-linux-x64.AppImage` | Standalone script editor and developer environment. |
| **Neovim (nvim)** | Latest | `~/.local/bin/nvim` | Fast terminal modal editor for rapid configuration, shader editing, and script maintenance. |
| **Alacritty & Foot** | 0.17.0 | `/usr/bin/alacritty`, `/usr/bin/foot` | GPU-accelerated terminal emulators for headless engine testing, CLI game builds, and asset batch runners. |
| **Devin Desktop & LobeHub** | Latest | `/usr/bin/devin-desktop`, `~/Applications/LobeHub-2.2.14.AppImage` | AI assistant workstations for workflow automation, rapid prototyping, and LLM orchestration. |

---

## 7. Compilers, SDKs, Runtimes & Build Systems

| Toolchain / Runtime | Version | Path / Command | Game Creation Capabilities |
|---|---|---|---|
| **.NET SDK (`dotnet`)** | 10.0 / 9.0 / 8.0 | `/usr/bin/dotnet`, `~/.dotnet/` | C# compilation, Roslyn source generators, MSBuild engine, and xUnit testing framework for Godot C# scripts. |
| **Rust (`rustc` / `cargo`)** | 1.97.1 | `/home/robertsrff/.cargo/bin/rustc`, `cargo` | Systems programming language for building high-performance Godot GDExtension native plugins and Rust game engines (Bevy, macroquad). |
| **GCC (C/C++)** | 16.1.1 (Red Hat) | `/usr/bin/gcc`, `/usr/bin/g++` | Native C and C++ compiler for building game modules, C libraries, and native physics/math extensions. |
| **CMake & GNU Make & Ninja** | CMake 4.3.0, Make 4.4.1, Ninja 1.13.2 | `/usr/bin/cmake`, `make`, `ninja` | Industry-standard build system generators for compiling native C/C++ game engines and dependencies. |
| **Python 3 (`python3`)** | 3.14.6 / 3.13 / 3.12 | `/usr/bin/python3`, `~/.local/bin/uv`, `poetry` | Game asset validation scripts, procedural generation, catalog integrity tools, and narrative pipeline scripts. |
| **Node.js & npm / pnpm** | Node v24.19.0, npm 12.0.2 | `/usr/bin/node`, `/usr/bin/npm` | Web game development (HTML5 / Canvas / WebGL / Three.js / Phaser) and web-based asset tools. |
| **Cython** | Latest | `~/.local/bin/cython`, `cythonize` | Compiling Python algorithms into native C extensions for high-speed simulation loops. |

---

## 8. Compatibility, Testing & Graphics Hardware Diagnostics

| Application / Utility | Version / Details | Binary Path / Command | Primary Use in Game Creation |
|---|---|---|---|
| **Wine (Staging)** | 11.0 | `/usr/bin/wine`, `winecfg` | Windows compatibility runtime for playtesting Windows `.exe` standalone game builds and running Windows-only game tools. |
| **Vulkan Diagnostics** | Latest | `/usr/bin/vulkaninfo` | Hardware validation of Vulkan 1.3 / Forward+ rendering capabilities for Godot & modern 3D pipelines. |
| **OpenGL Diagnostics** | Latest | `/usr/bin/glxinfo` | Validation of OpenGL / GL Compatibility rendering mode for 2D games and web export targets. |

---

## 9. Version Control & Cloud Deployment

| Tool | Version | Path / Command | Primary Use in Game Creation |
|---|---|---|---|
| **Git** | 2.55.0 | `/usr/bin/git` | Source code and project version control. |
| **Git LFS** | 3.7.1 | `/home/robertsrff/.local/bin/git-lfs` | Git Large File Storage for tracking textures, audio files, 3D meshes, and binary game assets. |
| **GitHub CLI (`gh`)** | 2.97.0 | `/usr/bin/gh` | Managing GitHub repositories, pull requests, automated releases, and CI/CD pipelines. |
| **Google Cloud SDK (`gcloud`)** | 579.0.0 | `~/google-cloud-sdk/bin/gcloud` | Cloud backend infrastructure, online game servers, and asset storage buckets. |

---
