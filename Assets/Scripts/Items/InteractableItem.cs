using UnityEngine;
using Assets.Scripts.Items;
using Unity.VisualScripting;

public class InteractableItem : MonoBehaviour
{
    public ScriptableObjectCreation itemData;    

    public void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }
}
