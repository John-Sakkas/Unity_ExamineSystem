using UnityEngine;

namespace Items.ItemFunctions
{
    public class ItemResize : MonoBehaviour
    {
        public static void ResizeObject(GameObject resizeObject , Vector3 parentSize)
        {
            if (resizeObject.GetComponent<MeshFilter>() == null) return;
            while (resizeObject.transform.GetComponent<MeshFilter>().GetComponent<Renderer>().bounds.extents.x > parentSize.x ||
                   resizeObject.transform.GetComponent<MeshFilter>().GetComponent<Renderer>().bounds.extents.y > parentSize.y ||
                   resizeObject.transform.GetComponent<MeshFilter>().GetComponent<Renderer>().bounds.extents.z > parentSize.z)
            {
                resizeObject.transform.localScale *= 0.9f;
            }
        }
    }
}
