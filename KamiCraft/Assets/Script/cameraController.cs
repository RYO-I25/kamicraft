using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

     public struct CameraParam{
         public Vector3 cameraPos;
     }

    public enum CameraMode : int{
        Down,
        Normal,
        Up,
        Num
    }
    public Vector3 nowPosition;
    public CameraParam donwParam;
    public CameraParam upParam;
    private Vector3 beforePosition;
    private Vector3 nextPosition;
    private Vector3 beforeEulerRotation;
    private Vector3 nextEulerRotation;
    private Vector3[] cameraPos = new Vector3[(int)CameraMode.Num] { new Vector3(-0.2f, 0.3f, -0.5f), new Vector3(0.0f, 0.0f, -0.6f), new Vector3(-0.2f, -0.3f, -0.5f) };
    private Vector3[] qulerRot = new Vector3[(int)CameraMode.Num] { new Vector3(30.0f, 20.0f, 0.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(-30.0f, 20.0f, 0.0f) };
    private float easingTimer = 0.0f;
    private bool isEasing = false;


    void Start()
    {
        
    }

    private void LateUpdate()
    {
        nowPosition = transform.localPosition;

        if (isEasing)
        {
            easingTimer += Time.deltaTime;

            transform.localPosition = Vector3.Lerp(beforePosition, nextPosition, easingTimer);
            transform.localRotation = Quaternion.Lerp(Quaternion.Euler(beforeEulerRotation), Quaternion.Euler(nextEulerRotation), easingTimer);
            if (easingTimer >= 1.0f)
            {
                isEasing = false;
                easingTimer = 0.0f;
            }
        }

    }

    public void ChangeCameraMode(CameraMode mode)
    {
        switch (mode) {
            case CameraMode.Down:
                //transform.localPosition = new Vector3(-0.2f, 0.3f, -0.5f);
                //transform.LookAt(transform.parent.transform);
                break;
            case CameraMode.Normal:
                //transform.localPosition = new Vector3(-0.0f, 0.0f, -0.6f);
                //transform.rotation = Quaternion.identity;

                break;
            case CameraMode.Up:
                //transform.localPosition = new Vector3(-0.2f, -0.3f, -0.5f);
                //transform.LookAt(transform.parent.transform);
                break;

        }
        beforePosition = transform.localPosition;
        nextPosition = cameraPos[(int)mode];
        beforeEulerRotation = transform.localRotation.eulerAngles;
        nextEulerRotation = qulerRot[(int)mode]; ;
        isEasing = true;

    }
}
