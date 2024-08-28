using UnityEngine;

namespace Items.ItemFunctions
{
    public class ItemOldValues
    {
        public Vector3 OldPosition;
        public Quaternion OldRotation;
        public Vector3 OldScale;
        public LayerMask Layer;

        public Vector3 DefaultInspectPosition;
        public Quaternion DefaultInspectRotation;

        public void OldValuesSet(Transform oldValues, LayerMask layer, Transform  defaultInspectPosition)
        {
            OldPosition = oldValues.position;
            OldRotation = oldValues.rotation;
            OldScale = oldValues.localScale;
            Layer = layer;
            DefaultInspectPosition = defaultInspectPosition.localPosition;
            DefaultInspectRotation = defaultInspectPosition.localRotation;
        }
    }
}
