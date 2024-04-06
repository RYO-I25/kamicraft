using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchErea : MonoBehaviour {

    public bool m_isHit;
    public GameObject emptyGameObject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool IsSearchEmptyErea()
    {
        return m_isHit;
    }

    public GameObject GetEmptyErea()
    {
        return emptyGameObject;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.tag == "EmptyErea")
        {
            m_isHit = true;
            emptyGameObject = collision.gameObject;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "EmptyErea")
        {
            m_isHit = true;
            emptyGameObject = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "EmptyErea")
        {
            emptyGameObject = null;
            m_isHit = false;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "EmptyErea")
        {
            emptyGameObject = null;
            m_isHit = false;
        }
    }
}
