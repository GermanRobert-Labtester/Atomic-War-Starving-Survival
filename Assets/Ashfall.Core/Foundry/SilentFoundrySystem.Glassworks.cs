namespace Ashfall.Core.Foundry
{
    public sealed partial class SilentFoundrySystem
    {
        /// <summary>
        /// Bind B100's authored glassworks roster into the existing Silent
        /// Foundry heat machine. No second production authority is created.
        /// </summary>
        public void BindGlassworksCatalog(GlassworksCatalog catalog)
        {
            if (catalog == null) return;
            var projected = new System.Collections.Generic.List<FoundryProductEntry>();
            foreach (var recipe in catalog.Recipes)
                projected.Add(recipe.ToProductEntry());
            _catalog.MergeGlassworksRecipes(projected);
        }
    }
}
