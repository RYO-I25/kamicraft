using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperCutGameManager : MonoBehaviour {

    public GameObject player;
    private float m_missTimer = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!player.activeSelf)
        {
            m_missTimer += Time.deltaTime;
            if(m_missTimer >= 3.0f)
            {
                player.SetActive(true);
                m_missTimer = 0.0f;
                player.transform.position = Vector3.zero;
            }
        }
	}
}
