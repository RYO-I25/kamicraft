using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    enum StateType
    {
        Moveing,
        Cutting,
        Miss
    }

    public enum FlagType
    {
        GateHitting = 1 << 0
    }

    public struct Flag
    {
        FlagType flag;

        public void Set(FlagType type, bool flaged)
        {
            if (flaged)
            {
                flag = flag | type;
            }
            else
            {
                flag &= ~type;
            }
        }

        public bool Get(FlagType type)
        {
            return (flag & type) == type;
        }

        public void Reset()
        {
            flag = 0;
        }
    }

    //private
    private Rigidbody m_rigidbody;
    private Vector3 m_direction;
    private bool m_isCutting = false;
    private StateType stateType = StateType.Moveing;
    private Flag m_flag;
    //public
    public float moveSpeed = 1.0f;

    // Use this for initialization
    void Start () {
        m_rigidbody = GetComponent<Rigidbody>();

        m_flag.Reset();
    }

    // Update is called once per frame
    void Update() {

        //移動挙動
        float front = 0.0f;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
        {
            front = Input.GetAxis("Horizontal");
        }

        float up = 0.0f;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            up = Input.GetAxis("Vertical");
            
        }
        Vector3 moveDirection = new Vector3(front, up, 0.0f);
        m_rigidbody.position += moveDirection * moveSpeed;
    }

 

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private void OnTriggerStay(Collider collision)
    {
    }

    private void OnTriggerExit(Collider collision)
    {
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Gate"))
        {
            gameObject.SetActive(false);
            m_flag.Set(FlagType.GateHitting, true);
        }
    }
}
