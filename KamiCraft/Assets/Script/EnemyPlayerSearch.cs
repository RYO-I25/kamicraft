using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerSearch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.GetComponentInParent<EnemyController>().ChangeMode(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.GetComponentInParent<EnemyController>().ChangeMode(false);
        }
    }
}
