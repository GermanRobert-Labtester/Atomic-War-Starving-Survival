# MODEL ROUTING

Assigned production-model availability checked 2026-08-13. Production allocation is exactly 80 Recraft assets plus 326 SeaArt assets; Adobe Firefly is fallback-only.

## Allocation

| Model | Assets |
|---|---:|
| FLUX.2 Text-to-Image Pro | 237 |
| GPT Image 2 | 24 |
| Nano Banana 2 | 27 |
| Nano Banana Pro Image | 3 |
| Recraft V4.1 | 8 |
| Recraft V4.1 Pro | 2 |
| Recraft V4.1 Utility | 58 |
| Recraft V4.1 Utility Pro | 12 |
| Seedream 5.0 Pro | 35 |
| **Recraft total** | **80** |
| **SeaArt total** | **326** |
| **Production total** | **406** |

## Routing rules

- **Recraft V4.1 Utility:** simple, predictable isolated product views and coherent object families.
- **Recraft V4.1 / Pro:** expressive keepsakes or intricate objects that benefit from richer texture; Pro is reserved for high-detail masters.
- **Recraft Utility Pro:** intricate product assemblies that still need calm framing. Vector variants are not used because the locked item medium is textured raster illustration.
- **GPT Image 2:** multi-component benches, generators and devices where exact object relationships matter.
- **Nano Banana Pro Image:** three controlled edits that must preserve the existing object while removing text/background.
- **Nano Banana 2:** damage, fill-state and tier derivatives that change one thing from a family anchor.
- **Seedream 5.0 Pro:** protective equipment, evidence props and difficult mixed-material objects requiring controlled final composition.
- **FLUX.2 Text-to-Image Pro:** the remaining single-object food, material, tool and inert weapon illustrations.
- **Kling, Grok Imagine, Qwen Image, Z-Image:** considered as requested but not assigned. No production decision depends on a current SeaArt SKU from these families.
- **Community models:** no assignment; no specialist checkpoint provides a documented benefit worth licensing and consistency risk.

## Recraft suitability score

Every live item is scored in `ASSET_MANIFEST.md` before routing. The 0–5 score measures silhouette clarity, single-object composition, family consistency and design-illustration advantage. Existing bases are `n/a`; controlled edits score 1; reference derivatives 2.5; complex scenes and weapons 2–3; clean devices, tools, medical supplies and protective objects score 4–4.5. Priority and non-duplication break ties for the 80 available slots.

## Firefly fallback

No production asset is allocated to Firefly. A compact `FIREFLY_FALLBACK` can be derived for simple materials, tools, medical supplies, protective gear, comfort objects and barter objects using: subject, centered isolated framing, visible wear, top-left light, restrained palette, no text. Queue prompts remain the source of truth.

## Verified model sources

- [Recraft V4.1 family documentation](https://www.recraft.ai/docs/recraft-models/recraft-v4-1)
- [Recraft API model list and Pro/Utility/Vector variants](https://www.recraft.ai/docs/api-reference/getting-started)
- [SeaArt GPT Image 2](https://www.seaart.ai/models/detail/d7h08hte878c738qgpdg)
- [SeaArt Nano Banana Pro Image](https://www.seaart.ai/models/detail/d49btu5e878c73avuqfg)
- [SeaArt Nano Banana 2](https://www.seaart.ai/models/detail/d6ggttle878c739bpf50)
- [SeaArt Seedream 5.0 and 5.0 Pro](https://www.seaart.ai/model/seedream-5-0)
- [SeaArt FLUX.2 Text-to-Image Pro](https://www.seaart.ai/indo/models/detail/d4jcc3le878c73fka850)

## Platform settings

- Request 1:1. Use 1024×1024 for standard drafts and 2048×2048 for selected Recraft Pro masters.
- Keep an opaque black field; do not request generated transparency. Human cleanup may produce alpha later only if the UI treatment changes.
- Generate one object per file and one asset ID per output.
- Generate family anchors before referenced derivatives.
- Raw output never ships directly; human paintover is mandatory.
