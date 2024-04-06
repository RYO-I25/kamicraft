using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour {

    public enum EnemyType
    {
        Stay,
        Round,
        Follow,
        Jump,
        Roll
    }

    public EnemyType enemyType = EnemyType.Stay;
    public bool bulletAttack = false;
    public RoundSetteing roundSetteing = new RoundSetteing();
    public FollowSetteing followSetteing = new FollowSetteing();
    public JumpSetteing jumpSetteing = new JumpSetteing();
    public BulletSetteing bulletSetteing = new BulletSetteing();
    public RollSetteing rollSetteing = new RollSetteing();

    [System.Serializable]
    public class RoundSetteing
    {
        public float speed;
        public Vector3 roundPos;
        public Vector3 posOffset;
    }

    [System.Serializable]
    public class FollowSetteing
    {
        public float speed;
    }

    [System.Serializable]
    public class JumpSetteing
    {
        public float speed;
        public float jumpPower;
    }

    [System.Serializable]
    public class BulletSetteing
    {
        public float speed;
        public float interval;
        public Vector3 shotNozzlePos;
    }

    [System.Serializable]
    public class RollSetteing
    {
        public float rollSpeed;
        public float AtaackTime;
        public float StayTime;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
