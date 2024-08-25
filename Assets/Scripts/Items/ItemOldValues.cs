using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOldValues
{
    public Vector3 oldPosition;
    public Quaternion oldRotation;
    public Vector3 oldScale;
    public LayerMask layer;

    public Vector3 defaultInspectPosition;
    public Quaternion defaultInspectRotation;

    public void OldValuesSet(Vector3 _oldPosition , Quaternion _oldRotation, 
        Vector3 _oldScale, LayerMask _layer, Vector3 _defaultInspectPosition, Quaternion _defaultInspectRotation)
    {
        oldPosition = _oldPosition;
        oldRotation = _oldRotation;
        oldScale = _oldScale;
        layer = _layer;
        defaultInspectPosition = _defaultInspectPosition;
        defaultInspectRotation = _defaultInspectRotation;
    }
}
