using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform followTransform;

    // Start is called before the first frame update


    // Update is called once per frame
    void FixedUpdate()
    {
    }

    private void LateUpdate()
    {
        this.transform.position = new Vector3(followTransform.position.x + 5.5f, followTransform.position.y + 1.5f, -10);

    }
}
