using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RotateWithParent : MonoBehaviour
{
    private void Update()
    {
        if (transform.parent.parent != null) 
        {
            Vector3 rotation = new Vector3(transform.parent.parent.rotation.x, transform.parent.parent.rotation.y, transform.parent.parent.rotation.z);
            //GetComponent<VisualEffect>().
        } 
    }
}
