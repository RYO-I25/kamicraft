using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HC.Debug;

public class EnemyController : MonoBehaviour
{

    public enum MoveType
    {
        Stay,
        Roll
    }

    public GameObject bullet;
    public GameObject muzzle;
    public float attackTime = 1.5f;

    private bool m_isAttacking = false;
    private GameObject m_player;
    private float m_bulletTimer = 0.0f;
    private float m_rollTimer = 0.0f;
    public GameObject[] parts;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float needDist;
    private EnemyStatus enemyStatus;
    private Vector3 direction;
    private Vector3 basePos;
    private bool isFirstMove = false;
    private MoveType m_moveType = MoveType.Roll;

    [SerializeField, Tooltip("可視コライダーの色")]
    private ColliderVisualizer.VisualizerColorType _visualizerColor;

    [SerializeField, Tooltip("メッセージ")]
    private string _message;

    [SerializeField, Tooltip("フォントサイズ")]
    private int _fontSize = 36;


    // Use this for initialization
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        enemyStatus = GetComponent<EnemyStatus>();
        if (enemyStatus)
        {

            switch (enemyStatus.enemyType)
            {
                case EnemyStatus.EnemyType.Stay:
                    {

                    }
                    break;
                case EnemyStatus.EnemyType.Round:
                    {
                        targetPos = transform.position + enemyStatus.roundSetteing.roundPos;
                        startPos = transform.position + enemyStatus.roundSetteing.posOffset;
                        needDist = Vector3.Distance(transform.position, targetPos);
                        direction = enemyStatus.roundSetteing.roundPos.normalized;
                        basePos = transform.position;
                    }
                    break;
                case EnemyStatus.EnemyType.Follow:
                    {
                    }
                    break;
                case EnemyStatus.EnemyType.Jump:
                    {
                        GameObject search = transform.Find("PlayerSearch").gameObject;
                        search.SetActive(false);
                        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>() as Rigidbody;
                        rigidbody.freezeRotation = true;
                    }
                    break;

            }

        }
        gameObject.AddComponent<ColliderVisualizer>().Initialize(_visualizerColor, _message, _fontSize);
    }

    // Update is called once per frame
    void Update()
    {


        if (enemyStatus)
        {
            switch (enemyStatus.enemyType)
            {
                case EnemyStatus.EnemyType.Stay:
                    {

                    }
                    break;
                case EnemyStatus.EnemyType.Round:
                    {
                        transform.Translate(direction * enemyStatus.roundSetteing.speed * Time.deltaTime);
                        float nowDist = isFirstMove ? Vector3.Distance(startPos, transform.position) : Vector3.Distance(basePos, transform.position);
                        if (nowDist >= needDist)
                        {
                            Vector3 tempPos = targetPos;
                            targetPos = startPos;
                            startPos = tempPos;
                            needDist = Vector3.Distance(startPos, targetPos);
                            direction = (targetPos - startPos).normalized;
                            isFirstMove = true;
                        }
                    }
                    break;
                case EnemyStatus.EnemyType.Follow:
                    {
                        if (m_isAttacking)
                        {
                            direction = (m_player.transform.position - transform.position).normalized;
                            transform.Translate(direction * enemyStatus.followSetteing.speed * Time.deltaTime);

                        }
                    }
                    break;
                case EnemyStatus.EnemyType.Jump:
                    {
                        transform.Translate(Vector3.back * enemyStatus.jumpSetteing.speed * Time.deltaTime);
                    }
                    break;
                case EnemyStatus.EnemyType.Roll:
                    {
                        m_rollTimer += Time.deltaTime;
                        if (m_moveType == MoveType.Stay)
                        {
                            if(m_rollTimer >= enemyStatus.rollSetteing.StayTime)
                            {
                                m_moveType = MoveType.Roll;
                                enemyStatus.bulletAttack = true;
                                m_rollTimer = 0.0f;
                            }
                        }
                        else
                        {
                            if (m_rollTimer >= enemyStatus.rollSetteing.AtaackTime)
                            {
                                m_moveType = MoveType.Stay;
                                enemyStatus.bulletAttack = false;
                                m_rollTimer = 0.0f;
                            }
                            else
                            {
                                Vector3 muzzleDirection = muzzle.transform.position - transform.position;
                                Vector3 targetDirection = m_player.transform.position - transform.position;

                                if (Vector3.Cross(muzzleDirection, targetDirection).x > 0.0f)
                                {
                                    transform.rotation = transform.rotation * Quaternion.AngleAxis(enemyStatus.rollSetteing.rollSpeed, Vector3.forward);
                                }
                                else
                                {
                                    transform.rotation = transform.rotation * Quaternion.AngleAxis(-1.0f * enemyStatus.rollSetteing.rollSpeed, Vector3.forward);
                                }
                                
                            }
                        }
                    }
                    break;


            }

            if (enemyStatus.bulletAttack)
            {
                m_bulletTimer += Time.deltaTime;
                if (m_bulletTimer > enemyStatus.bulletSetteing.interval)
                {
                    m_bulletTimer = 0.0f;
                    GameObject gun = Instantiate(bullet, muzzle.transform.position, transform.transform.rotation) as GameObject;
                    if (gun.GetComponent<ballet>())
                    {
                        Vector3 targetDirection = m_player.transform.position - muzzle.transform.position;
                        gun.GetComponent<ballet>().SetVelocity(targetDirection.normalized * enemyStatus.bulletSetteing.speed * Time.deltaTime);
                    }
                }
            }
        }


    }

    public void ChangeMode(bool isAttacking)
    {
        m_isAttacking = isAttacking;
    }

    public void Die()
    {
        gameObject.SetActive(false);
        foreach (GameObject part in parts)
        {
            GameObject newPart = Instantiate(part, gameObject.transform.position, Quaternion.identity) as GameObject;
            Rigidbody rigidbody = newPart.GetComponent<Rigidbody>();
            rigidbody.velocity += new Vector3(Random.Range(-0.7f, 0.7f), Random.Range(3.0f, 5.0f), Random.Range(-0.7f, 0.7f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Player"))
        //{
        //    other.GetComponent<PlayerController>().Damage();
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enemyStatus.enemyType == EnemyStatus.EnemyType.Jump)
        {
            if (collision.collider.tag == "Ground")
            {
                Rigidbody rigidbody = GetComponent<Rigidbody>();
                if (rigidbody)
                {
                    rigidbody.velocity += Vector3.up * enemyStatus.jumpSetteing.jumpPower;
                }

            }
        }
    }
}
