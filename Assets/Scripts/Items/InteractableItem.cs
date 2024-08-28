using Items.ScriptableItem;
using UnityEngine;

namespace Items
{
    public class InteractableItem : MonoBehaviour
    {
        public NewInteractableItem itemData;    

        public void Start()
        {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }
    }
}
