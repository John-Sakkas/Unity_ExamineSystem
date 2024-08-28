using UnityEngine;

namespace ThirdPerson_InspectSystem.ComponentFinderScripts
{
    public class ItemInspectorCamera : MonoBehaviour
    {
        //This script is only so the InspectSystem can find the object using the GetComponent<>()

        //You need to add this script to the inspect Camera or use the prefab Camera.
        //Make sure to change the option in Camera to Depth Only and to Culling Mask.
    }
}
