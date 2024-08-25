using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;

public class CameraGizmosLine : MonoBehaviour
{
    public float distance = 2.0f;
    public Color lineColor = Color.red;
    public LayerMask _layerMask;

    private bool _isExamineOn = false;
    private Camera _inspectCamera;
    private Vector3 _parentSize;
    private Vector3 _startPosition;
    private Vector3 _direction;
    public Transform _playerHeadPoint;
    private GameObject _inspectPosition;
    private GameObject _player;
    private GameObject _examineIndicatorTxt;
    private RaycastHit hit;
    private ItemOldValues _oldTransformValues = new();
    private InteractableItem _examinetedItem;
    private PostProcessVolume _postProcessVolume;

    public float rotationSpeed;
    public float zoomSpeed;
    public float minZoomDistance;  // Minimum distance from the camera
    public float maxZoomDistance;    // Maximum distance from the camera

    void Start()
    {
        _inspectPosition = GameObject.Find("InspectedPosition");
        _inspectCamera = GameObject.Find("InspectCamera").GetComponent<Camera>();
        _postProcessVolume = gameObject.GetComponent<PostProcessVolume>();
        _player = GameObject.FindWithTag("Player");
        _playerHeadPoint = GameObject.Find("PlayerCameraRoot").transform;
        _examineIndicatorTxt = GameObject.Find("ExamineIndicator");

        _parentSize = _inspectPosition.transform.GetComponent<BoxCollider>().bounds.size / 2;
    }

    void Update()
    {
        _startPosition = _playerHeadPoint.position;

        _direction = transform.forward;

        // Perform the raycast
        if (Physics.Raycast(_startPosition, _direction, out hit, distance, _layerMask))
        {
            if(hit.collider.gameObject.GetComponent<InteractableItem>() != null)
            {
                _examineIndicatorTxt.SetActive(true);
                if(Input.GetKeyDown(KeyCode.E) && !_isExamineOn)
                {
                    _isExamineOn = true;
                    _examinetedItem = hit.collider.gameObject.GetComponent<InteractableItem>();

                    //keep the old values 
                    _oldTransformValues.OldValuesSet(_examinetedItem.transform.position,_examinetedItem.transform.rotation,_examinetedItem.transform.localScale,
                        _examinetedItem.gameObject.layer,_inspectPosition.transform.localPosition,_inspectPosition.transform.localRotation);

                    //Change to the new values
                    _examinetedItem.transform.SetParent(_inspectPosition.transform);
                    _examinetedItem.gameObject.layer = LayerMask.NameToLayer("Examine");
                    _examinetedItem.transform.localPosition = Vector3.zero;
                    _examinetedItem.transform.localRotation = Quaternion.identity;

                    _inspectCamera.gameObject.SetActive(true);
                    _postProcessVolume.enabled = true;
                    _player.SetActive(false);

                    ItemResize.ResizeObject(_examinetedItem.gameObject, _parentSize);
                }
            }
        }
        else
            _examineIndicatorTxt.SetActive(false);

        if(_isExamineOn)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                _isExamineOn =false;
                _examinetedItem.gameObject.transform.parent = null;
                _examinetedItem.transform.position = _oldTransformValues.oldPosition;
                _examinetedItem.transform.rotation = _oldTransformValues.oldRotation;
                _examinetedItem.transform.localScale = _oldTransformValues.oldScale;
                _examinetedItem.gameObject.layer = _oldTransformValues.layer;

                _inspectPosition.transform.localPosition = _oldTransformValues.defaultInspectPosition;
                _inspectPosition.transform.localRotation = _oldTransformValues.defaultInspectRotation;

                _inspectCamera.gameObject.SetActive(false);
                _postProcessVolume.enabled = false;
                _player.SetActive(true);
            }

            if (Input.GetMouseButton(0))
            {
                float rotX = Input.GetAxis("Mouse X") * rotationSpeed;
                float rotY = Input.GetAxis("Mouse Y") * rotationSpeed;
                _inspectPosition.transform.Rotate(Vector3.up, -rotX, Space.World);
                _inspectPosition.transform.Rotate(Vector3.right, rotY, Space.World);
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
                Vector3 zoomDirection = Camera.main.transform.forward * zoom;
                float distance = Vector3.Distance(Camera.main.transform.position, _inspectPosition.transform.position);

                // Clamp the zoom to stay within min and max distances
                if ((distance >= minZoomDistance || zoom > 0) && (distance <= maxZoomDistance || zoom < 0))
                {
                    _inspectPosition.transform.position += zoomDirection;
                }
            }
        }

        // Optional: Draw the ray in the Scene view for debugging
        Debug.DrawLine(_startPosition, _startPosition + _direction * distance, Color.red);
    }
}