using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EreaChanger : MonoBehaviour {

    public GameObject[] searchEreas;
    public GameObject emptyErea;
    public Vector3 ereaPos;
    public Vector3 emptyPos; 
    private bool isMoving = false;
    private float slideSpeed = 0.075f;

    public Vector3 _displace;
    public Vector3 _direciton;
    public Vector3 _position;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isMoving)
        {
            bool isbothMoving = true;
            {
                Vector3 displace = emptyPos - transform.position;
                Vector3 direciton = Vector3.Normalize(displace);

                if (Vector3.Distance(emptyPos, transform.position) > 0.01f)
                {
                    transform.position += direciton * slideSpeed;

                }
                else
                {
                    isbothMoving = false;
                }
            }

            {
                Vector3 displace = ereaPos - emptyErea.transform.position;
                Vector3 direciton = Vector3.Normalize(displace);

                if (Vector3.Distance(ereaPos, emptyErea.transform.position) > 0.01f)
                {
                    emptyErea.transform.position += direciton * slideSpeed;
                }
                else
                {
                    if (!isbothMoving)
                    {
                        isMoving = false;
                    }
                }
            }
        }
	}

    public bool MoveErea()
    {
        for(int i = 0; i < searchEreas.Length; i++)
        {
            if(searchEreas[i].GetComponent<SearchErea>().IsSearchEmptyErea())
            {
                emptyErea = searchEreas[i].GetComponent<SearchErea>().GetEmptyErea();
                emptyPos = emptyErea.transform.position;
                ereaPos = transform.position;
                isMoving = true;
                return true;
            }
        }
        return false;
    }
}
