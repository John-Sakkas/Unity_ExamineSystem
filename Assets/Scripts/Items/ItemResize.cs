using UnityEngine;

public class ItemResize : MonoBehaviour
{
    public static void ResizeObject(GameObject Resize_object , Vector3 parentSize)
    {
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
