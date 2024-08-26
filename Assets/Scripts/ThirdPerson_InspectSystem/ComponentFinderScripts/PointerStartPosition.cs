using UnityEngine;

namespace ThirdPerson_InspectSystem
{
    public class PointerStartPosition : MonoBehaviour
    {
        //This script is only so the InspectSystem can find the object using the GetComponent<>()

        //You need to add this script to the GameObject that you want to be the
        //start point of the RayCast in the InspectSystem.cs.
    }
}
