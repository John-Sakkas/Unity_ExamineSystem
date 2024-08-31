using Items.ItemFunctions;
using Items.ScriptableItem;
using ThirdPerson_InspectSystem.ComponentFinderScripts;
using UnityEngine;

namespace Items
{
    public class InteractableItem : MonoBehaviour
    {
        private Transform _inspectTransform;
        public NewInteractableItem itemData;   
        public float rotationSpeed = 1;
        public float zoomSpeed = 2;
        public float minZoomDistance = 2;
        public float maxZoomDistance = 4;

        public void Start()
        {
            _inspectTransform = FindObjectOfType<ItemInspectorPosition>().transform;
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }

        public void ItemRotation()
        {
            var rotX = Input.GetAxis("Mouse X") * rotationSpeed;
            var rotY = Input.GetAxis("Mouse Y") * rotationSpeed;
            _inspectTransform.Rotate(Vector3.up, -rotX, Space.World);
            _inspectTransform.Rotate(Vector3.right, rotY, Space.World);
        }

        public void ItemZoom(Transform mainCamera)
        {
            var zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            var zoomDirection = mainCamera.forward * zoom;
            var cameraDistance = Vector3.Distance(mainCamera.position, _inspectTransform.transform
                .position);

            // Clamp the zoom to stay within min and max distances
            if ((cameraDistance >= minZoomDistance || zoom > 0) && (cameraDistance <= maxZoomDistance || zoom < 0))
            {
                _inspectTransform.transform.position += zoomDirection;
            }
        }
        
        public void ExitExamine(ItemOldValues oldTransformValues)
        {
            gameObject.transform.parent = null;
            transform.position = oldTransformValues.OldPosition;
            transform.rotation = oldTransformValues.OldRotation;
            transform.localScale = oldTransformValues.OldScale;
            gameObject.layer = oldTransformValues.Layer;

            _inspectTransform.localPosition = oldTransformValues.DefaultInspectPosition;
            _inspectTransform.localRotation = oldTransformValues.DefaultInspectRotation;
        }

        public void ResetItem(ItemOldValues oldTransformValues)
        {
            _inspectTransform.localPosition = oldTransformValues.DefaultInspectPosition;
            _inspectTransform.localRotation = oldTransformValues.DefaultInspectRotation;
        }
    }
}
