using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// ScriptableObject catalog of all item definitions; the runtime source of
    /// truth for items. Loaded from / imported from StreamingAssets/Data/items.json.
    /// </summary>
    [CreateAssetMenu(fileName = "NewItemCatalog", menuName = "ASHFALL/Data/Item Catalog")]
    public class ItemCatalogSO : ScriptableObject
    {
        public List<ItemDefinition> items = new List<ItemDefinition>();

        /// <summary>Look up an item definition by its snake_case id.</summary>
        public ItemDefinition GetById(string id) => throw new System.NotImplementedException();
    }
}
