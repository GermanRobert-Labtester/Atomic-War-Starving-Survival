using UnityEditor;
using UnityEngine;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Automatically configures AI generated images placed in Resources/Art/ to import as 2D Sprites.
    /// This saves having to manually change the Texture Type for 1,000+ generated AI assets.
    /// </summary>
    public class AIAssetImporter : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            if (assetPath.Contains("Resources/Art/"))
            {
                TextureImporter textureImporter = (TextureImporter)assetImporter;
                
                // Only change if it's the default type, to respect manual overrides
                if (textureImporter.textureType == TextureImporterType.Default)
                {
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.spriteImportMode = SpriteImportMode.Single;
                    textureImporter.alphaIsTransparency = true;
                    textureImporter.mipmapEnabled = false;
                    textureImporter.filterMode = FilterMode.Bilinear;
                }
            }
        }
    }
}
