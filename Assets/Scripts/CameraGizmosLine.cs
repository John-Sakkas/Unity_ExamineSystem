using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;

public class CameraGizmosLine : MonoBehaviour
{
    public float distance = 2.0f;
    public Color lineColor = Color.red;
    public GameObject _interacablePosition;
    public Camera _inspectCamera;
    public PostProcessVolume _postProcessVolume;
    public GameObject _player;

    public InteractableItem _examinetedItem;
    public OldTransformValues _oldTransformValues = new();
    public bool _isExamineOn = false;
    public Vector3 parentSize;

    void OnDrawGizmos()
    {
        Gizmos.color = lineColor;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + transform.forward * distance;

        Gizmos.DrawLine(startPosition, endPosition);
    }

    public LayerMask layerMask;  // Optional: specify which layers to detect

    void Start()
    {
        parentSize = _interacablePosition.transform.GetComponent<BoxCollider>().bounds.size / 2;
    }

    void Update()
    {
        // Start position of the ray (camera position)
        Vector3 startPosition = transform.position;

        // End position, 2 meters in front of the camera
        Vector3 direction = transform.forward;

        // Store hit information
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(startPosition, direction, out hit, distance, layerMask))
        {
            if(hit.collider.gameObject.GetComponent<InteractableItem>() != null)
            {
                if(Input.GetButton("Jump"))
                {
                    _isExamineOn = true;
                    _examinetedItem = hit.collider.gameObject.GetComponent<InteractableItem>();

                    //keep the old values 
                    _oldTransformValues.oldPosition = _examinetedItem.transform.position;
                    _oldTransformValues.oldRotation = _examinetedItem.transform.rotation;
                    _oldTransformValues.oldScale = _examinetedItem.transform.localScale;

                    _oldTransformValues._layer = _examinetedItem.gameObject.layer;

                    //Change to the new values
                    _examinetedItem.transform.SetParent(_interacablePosition.transform);
                    _examinetedItem.gameObject.layer = LayerMask.NameToLayer("Examine");
                    _examinetedItem.transform.localPosition = Vector3.zero;
                    _examinetedItem.transform.localRotation = Quaternion.identity;

                    _inspectCamera.gameObject.SetActive(true);
                    _postProcessVolume.enabled = true;
                    _player.SetActive(false);

                    ResizeObject(_examinetedItem.gameObject);
                    Debug.Log("Hit object: " + _examinetedItem.itemData.Interact());
                }
            }
        }

        if(Input.GetButton("Cancel") && _isExamineOn)
        {
            _isExamineOn =false;
            _examinetedItem.gameObject.transform.parent = null;
            _examinetedItem.transform.position = _oldTransformValues.oldPosition;
            _examinetedItem.transform.rotation = _oldTransformValues.oldRotation;
            _examinetedItem.transform.localScale = _oldTransformValues.oldScale;
            _examinetedItem.gameObject.layer = _oldTransformValues._layer;

            _inspectCamera.gameObject.SetActive(false);
            _postProcessVolume.enabled = false;
            _player.SetActive(true);
        }

        // Optional: Draw the ray in the Scene view for debugging
        Debug.DrawLine(startPosition, startPosition + direction * distance, Color.red);
    }

    public void ResizeObject(GameObject Resize_object)
    {
        Camera.main.transform.localPosition = new Vector3(0, 0.8f, 0);
        Camera.main.transform.localRotation = new Quaternion(0, 0, 0, 0);


        if (Resize_object.GetComponent<MeshFilter>() != null)
        { 
            while (Resize_object.transform.GetComponent<MeshFilter>().GetComponent<Renderer>().bounds.extents.x > parentSize.x ||
                  Resize_object.transform.GetComponent<MeshFilter>().GetComponent<Renderer>().bounds.extents.y > parentSize.y ||
                  Resize_object.transform.GetComponent<MeshFilter>().GetComponent<Renderer>().bounds.extents.z > parentSize.z)
            {
                    Resize_object.transform.localScale *= 0.9f;
            }
        }
    }
}

public class OldTransformValues
{
    public Vector3 oldPosition;
    public Quaternion oldRotation;
    public Vector3 oldScale;
    public LayerMask _layer;
}