using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

namespace ThirdPerson_InspectSystem
{
    public class InspectSystem : MonoBehaviour
    {
        public LayerMask layerMask;
        public float raycastDistance = 2.0f;
        public float rotationSpeed;
        public float zoomSpeed;
        public float minZoomDistance;
        public float maxZoomDistance;

        public bool _isExamineOn;
        private Camera _inspectCamera;
        private Vector3 _parentSize;
        private Vector3 _startPosition;
        private Vector3 _direction;
        private Transform _playerHeadPoint;
        private Transform _inspectPosition;
        private GameObject _player;
        private GameObject _examineIndicatorUI;
        private GameObject _examineIndicatorOptionsUI;
        private RaycastHit _hit;
        private readonly ItemOldValues _oldTransformValues = new();
        private InteractableItem _examinedItem;
        private PostProcessVolume _postProcessVolume;

        private void Start()
        {
            _inspectPosition = gameObject.GetComponentInChildren<ItemInspectorPosition>().transform;
            _inspectCamera = gameObject.GetComponentInChildren<ItemInspectorCamera>(true).GetComponent<Camera>();
            _postProcessVolume = gameObject.GetComponent<PostProcessVolume>();
            _player = GameObject.FindWithTag("Player");
            _playerHeadPoint = FindObjectOfType<PointerStartPosition>().transform;
            _examineIndicatorUI = FindObjectOfType<ExamineUI>(true).gameObject;
            _examineIndicatorOptionsUI = FindObjectOfType<ExamineUIOptions>(true).gameObject;

            _parentSize = _inspectPosition.transform.GetComponent<BoxCollider>().bounds.size / 2;
        }

        private void Update()
        {
            _startPosition = _playerHeadPoint.position;

            _direction = transform.forward;

            // Perform the raycast
            if (Physics.Raycast(_startPosition, _direction, out _hit, raycastDistance, layerMask))
            {
                if(_hit.collider.gameObject.GetComponent<InteractableItem>() != null)
                {
                    _examineIndicatorUI.SetActive(true);
                    _examinedItem = _hit.collider.gameObject.GetComponent<InteractableItem>();
                    if(Input.GetKeyDown(KeyCode.E) && !_isExamineOn)
                    {
                        _isExamineOn = true;
                        var examinedItemTransform = _examinedItem.transform;
                    
                        //keep the old values 
                        _oldTransformValues.OldValuesSet(examinedItemTransform.position,examinedItemTransform.rotation,examinedItemTransform.localScale,
                            _examinedItem.gameObject.layer,_inspectPosition.localPosition,_inspectPosition.localRotation);

                        //Change to the new values
                        _examinedItem.gameObject.layer = LayerMask.NameToLayer("Examine");
                        examinedItemTransform.SetParent(_inspectPosition.transform);
                        examinedItemTransform.localPosition = Vector3.zero;
                        examinedItemTransform.localRotation = Quaternion.identity;

                        _examineIndicatorOptionsUI.SetActive(true);
                        _inspectCamera.gameObject.SetActive(true);
                        _postProcessVolume.enabled = true;
                        _player.SetActive(false);

                        ItemResize.ResizeObject(_examinedItem.gameObject, _parentSize);
                    }
                }
            }
            else
                _examineIndicatorUI.SetActive(false);

            if(_isExamineOn)
            {
                if(Input.GetKeyDown(KeyCode.R))
                {
                    _inspectPosition.localPosition = _oldTransformValues.defaultInspectPosition;
                    _inspectPosition.localRotation = _oldTransformValues.defaultInspectRotation;
                }
                
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    ResetItem();
                }

                if (Input.GetMouseButton(0))
                {
                    var rotX = Input.GetAxis("Mouse X") * rotationSpeed;
                    var rotY = Input.GetAxis("Mouse Y") * rotationSpeed;
                    _inspectPosition.Rotate(Vector3.up, -rotX, Space.World);
                    _inspectPosition.Rotate(Vector3.right, rotY, Space.World);
                }

                if (Input.GetAxis("Mouse ScrollWheel") != 0f)
                {
                    var cameraTransform = gameObject.transform;
                
                    var zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
                    var zoomDirection = cameraTransform.forward * zoom;
                    var cameraDistance = Vector3.Distance(cameraTransform.position, _inspectPosition.transform
                    .position);

                    // Clamp the zoom to stay within min and max distances
                    if ((cameraDistance >= minZoomDistance || zoom > 0) && (cameraDistance <= maxZoomDistance || zoom < 0))
                    {
                        _inspectPosition.transform.position += zoomDirection;
                    }
                }
            }

            // Optional: Draw the ray in the Scene view for debugging
            Debug.DrawLine(_startPosition, _startPosition + _direction * raycastDistance, Color.red);
        }

        private void ResetItem()
        {
            _isExamineOn = false;
            
            _examinedItem.gameObject.transform.parent = null;
            _examinedItem.transform.position = _oldTransformValues.oldPosition;
            _examinedItem.transform.rotation = _oldTransformValues.oldRotation;
            _examinedItem.transform.localScale = _oldTransformValues.oldScale;
            _examinedItem.gameObject.layer = _oldTransformValues.layer;

            _inspectPosition.localPosition = _oldTransformValues.defaultInspectPosition;
            _inspectPosition.localRotation = _oldTransformValues.defaultInspectRotation;

            _examineIndicatorOptionsUI.SetActive(false);
            _inspectCamera.gameObject.SetActive(false);
            _postProcessVolume.enabled = false;
            _player.SetActive(true);
        }
    }
}