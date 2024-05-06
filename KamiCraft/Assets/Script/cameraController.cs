using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

     public struct CameraParam{
         public Vector3 cameraPos;
     }

    public enum CameraMode{
        Down,
        Normal,
        Up
    }

    public CameraParam donwParam;
    public CameraParam upParam;
    
    void Start()
    {
        
    }

    void updatePostion()
    {
        
    }

    public void ChangeCameraMode(CameraMode mode)
    {
        
    }
}
