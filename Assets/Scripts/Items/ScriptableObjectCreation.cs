using UnityEngine;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "NewSpecificItem", menuName = "Inventory/SpecificItem")]
    public class ScriptableObjectCreation : ItemBase
    {
        public override string Interact()
        {
            // Custom interaction logic for this specific item
            return "Inspecting " + itemName;
        }
    }
}
