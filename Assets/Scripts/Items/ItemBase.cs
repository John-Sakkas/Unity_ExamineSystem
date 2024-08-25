using UnityEngine;

namespace Assets.Scripts.Items
{
    public abstract class ItemBase : ScriptableObject
    {
        public string itemName;
        public string description;
        public bool isCollectable;

        // Method to handle interaction (can be overridden in derived classes)
        public abstract string Interact();
    }
}