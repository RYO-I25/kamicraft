using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scissor : MonoBehaviour {

    private bool IsAttackingEnemy = false;
    public GameObject hitObj = null;
    public GameObject player = null;
    public Vector3 hitpos;
    public Vector3 ownpos;
    public float distance;
    RaycastHit rayHit;
    public LayerMask mask;
    public GameObject[] objs;
    public Image scissorUI;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //ownpos = GetComponent<BoxCollider>().ClosestPoint(transform.localPosition);
        //Ray ray = new Ray(transform.position - transform.forward * 0.25f, transform.forward);
        //RaycastHit hit;
        //// Rayが衝突したかどうか
        //if (Physics.Raycast(ray, out hit, 1.0f, mask))
        //{
        //    if (hit.collider.CompareTag("Enemy"))
        //    {
        //        ownpos = hit.transform.position;
        //        hitpos = hit.point;
        //        distance = hit.distance;
        //    }
        //}
        //else
        //{
        //    ownpos = Vector3.zero;
        //}

        //Debug.DrawRay(ray.origin, ray.direction * 1.0f, Color.red);

        ownpos = GetComponent<BoxCollider>().ClosestPoint(transform.position + transform.right * 30.0f);
        hitpos = GetComponent<BoxCollider>().ClosestPoint(transform.position - transform.right * 30.0f);

        if(player.transform.rotation.y > 0.0f)
        {
            scissorUI.transform.localPosition = new Vector3(0.6f, 0.0f, 0.5f);
        }
        else
        {
            scissorUI.transform.localPosition = new Vector3(0.6f, 0.0f, -0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Core"))
        {
            IsAttackingEnemy = true;
            hitObj = other.gameObject;
            if (scissorUI)
            {
                scissorUI.color = Color.red;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Core"))
        {
            IsAttackingEnemy = false;
            hitObj = null;
            if (scissorUI)
            {
                scissorUI.color = Color.white;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("Enemy"))
        //{
        //    Vector3 ownFrontpos = GetComponent<BoxCollider>().ClosestPoint(transform.position + transform.forward * 30.0f);
        //    //Vector3 ownBackpos = GetComponent<BoxCollider>().ClosestPoint(transform.position - transform.forward * 30.0f);

        //    Vector3 hitFrontpos = other.ClosestPoint(other.transform.position + transform.forward * 30.0f);
        //    //Vector3 hitBackpos = other.ClosestPoint(other.transform.position - transform.forward * 30.0f);

        //    if(ownFrontpos.z > hitFrontpos.z)
        //    {
        //        IsAttackingEnemy = true;
        //        hitObj = other.gameObject;
        //    }
        //}
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.tag =="Enemy"){
            
        }
    }

    public bool IsAttick()
    {
        return IsAttackingEnemy;
    }

    public void DestroyEnemy()
    {
        if (hitObj)
        {
            hitObj.GetComponentInParent<EnemyController>().Die();
            hitObj = null;
            if (scissorUI)
            {
                scissorUI.color = Color.white;
            }
            IsAttackingEnemy = false;
        }
    }
}
