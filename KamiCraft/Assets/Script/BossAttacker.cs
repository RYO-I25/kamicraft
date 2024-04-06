using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttacker : MonoBehaviour {

    public enum State
    {
        Stay,
        Drop,
        Grond
    }

    public bool IsDroped = false;
    public float speed = 0.1f;
    private State m_state = State.Stay;

	// Use this for initialization
	void Start () {
        if (IsDroped)
        {
            m_state = State.Drop;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (m_state == State.Drop)
        {
            transform.Translate(Vector3.down * speed);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(m_state == State.Drop)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().Damage();
                GetComponent<BoxCollider>().isTrigger = true;
            }
        }

        if (other.CompareTag("Ground"))
        {
            m_state = State.Grond;
            GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.tag == "Ground")
        {
            m_state = State.Grond;
            GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    public void SetState(State state)
    {
        m_state = state;
    }
}
