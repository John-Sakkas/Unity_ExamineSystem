using Items;
using Items.ItemFunctions;
using ThirdPerson_InspectSystem.ComponentFinderScripts;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace ThirdPerson_InspectSystem
{
    public class ExamineSystem : MonoBehaviour
    {
        public LayerMask layerMask;
        public float raycastDistance = 2.0f;

        private bool _isExamineOn;
        private Vector3 _parentSize;
        private Vector3 _startPosition;
        private Vector3 _direction;
        private Transform _playerHeadPoint;
        private Transform _inspectPosition;
        private GameObject _player;
        private GameObject _examineIndicatorUI;
        private GameObject _examineIndicatorOptionsUI;
        private RaycastHit _hit;
        private InteractableItem _examinedItem;
        private PostProcessVolume _postProcessVolume;
        private readonly ItemOldValues _oldTransformValues = new();

        private void Start()
        {
            _inspectPosition = gameObject.GetComponentInChildren<ItemInspectorPosition>().transform;
            _postProcessVolume = gameObject.GetComponent<Camera>().GetComponent<PostProcessVolume>();
            _player = GameObject.FindWithTag(ConstantValues.PlayerTag);
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
                        _oldTransformValues.OldValuesSet(examinedItemTransform,_examinedItem.gameObject.layer, _inspectPosition.transform);

                        //Change to the new values
                        _examinedItem.gameObject.layer = LayerMask.NameToLayer(ConstantValues.ExamineLayer);
                        examinedItemTransform.SetParent(_inspectPosition.transform);
                        examinedItemTransform.localPosition = Vector3.zero;
                        examinedItemTransform.localRotation = Quaternion.identity;

                        _examineIndicatorOptionsUI.SetActive(true);
                        _postProcessVolume.enabled = true;
                        _player.SetActive(false);

                        Debug.Log("Examine item : " +_examinedItem.itemData.itemName);
                        
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
                    _examinedItem.ResetItem(_oldTransformValues);
                }
                
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    _isExamineOn = false;

                    _examinedItem.ExitExamine(_oldTransformValues);
                    
                    _examineIndicatorOptionsUI.SetActive(false);
                    _postProcessVolume.enabled = false;
                    _player.SetActive(true);
                }

                if (Input.GetMouseButton(0))
                {
                    _examinedItem.ItemRotation();
                }

                if (Input.GetAxis(ConstantValues.ScrollWheel) != 0f)
                {
                    _examinedItem.ItemZoom(gameObject.transform);
                }
            }

            // Optional: Draw the ray in the Scene view for debugging
            Debug.DrawLine(_startPosition, _startPosition + _direction * raycastDistance, Color.red);
        }
    }
}