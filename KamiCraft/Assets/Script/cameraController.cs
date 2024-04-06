using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {

    public bool isFixed;
    public GameObject objTarget;
    public Vector3 offset;

    void Start()
    {
        updatePostion();
    }

    void LateUpdate()
    {
        updatePostion();
    }

    void updatePostion()
    {
        if (!isFixed)
        {
            Vector3 pos = objTarget.transform.position;
            transform.localPosition = pos + offset;
        }
    }
}
