using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public abstract class ItemBase : ScriptableObject
    {
        public string itemName;
        public string description;
        public bool isCollectable;
    }
}