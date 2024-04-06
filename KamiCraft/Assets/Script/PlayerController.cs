using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {


    struct LineInfo
    {
        public Vector3 pos;
        public int id;
    }

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
    private int m_HP = 3;
    private Rigidbody m_rigidbody;
    private bool m_isDashed = false;
    private float m_timer = 0.0f;
    private float m_dashStopTimer = 0.0f;
    private Vector3 m_direction;
    private Vector3[] m_linePositions;
    private bool[] m_isCheckedList = new bool[0];
    private List<LineInfo> m_lineList = new List<LineInfo>();
    private List<int> m_scoreList = new List<int>();
    private Vector3[] m_startLinePos = new Vector3[0];
    private bool m_isCutting = false;
    private StateType stateType = StateType.Moveing;
    private Flag m_flag;
    private float m_missTimer;
    //public
    public Slider HPSlider;
    public float minDownSpeed = -8.0f;
    public GameObject scissor;
    public GameObject nozzle;
    public float moveSpeed = 1.0f;
    public float cutMoveSpeed = 1.0f;
    public float jumpPower = 4.5f;
    public float dashPower = 4.0f;
    public float dashTime = 0.4f;
    public LineRenderer myLineLender;
    public LineRenderer targetLineLender;
    public Text text;

    // Use this for initialization
    void Start () {
        m_rigidbody = GetComponent<Rigidbody>();

        text.text = "Score: 0";
        m_flag.Reset();
    }

    // Update is called once per frame
    void Update() {

        if (m_isCheckedList.Length == 0)
        {
            m_isCheckedList = new bool[targetLineLender.positionCount];
        }

        if (m_startLinePos.Length == 0)
        {
            m_startLinePos = new Vector3[targetLineLender.positionCount];
        }

        //移動挙動
        float front = 0.0f;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A))
        {
            front = Input.GetAxis("Horizontal");
        }
        else if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.D))
        {
            front = Input.GetAxis("Horizontal");
        }

        float up = 0.0f;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            up = Input.GetAxis("Vertical");
            
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            up = Input.GetAxis("Vertical");
        }

        if (up != 0.0f && front != 0.0f)
        {
            if (Mathf.Abs(front) >= Mathf.Abs(up))
            {
                if (front > 0.0f)
                {
                    if (up > 0.0f)
                    {
                        up = front;
                    }
                    else
                    {
                        up = -front;
                    }
                }
                else if (front < 0.0f)
                {
                    if (up > 0.0f)
                    {
                        up = -front;
                    }
                    else if (up < 0.0f)
                    {
                        up = front;
                    }
                }
                //up = front;
            }
            else
            {

                if (front > 0.0f)
                {
                    if (up > 0.0f)
                    {
                        front = up;
                    }
                    else
                    {
                        front = -up;
                    }
                }
                else if (front < 0.0f)
                {
                    if (up > 0.0f)
                    {
                        front = -up;
                    }
                    else if (up < 0.0f)
                    {
                        front = up;
                    }
                }



            }

            up *= 0.75f;
            front *= 0.75f;
        }

        Vector3 moveDirection = new Vector3(front, up, 0.0f);

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1.0f;//マウス座標はz軸がないのでカメラから少し話した数値にしないと正常に動作しないため数値を一時的に設定.

        Vector3 rotateDirection = (Camera.main.ScreenToWorldPoint(mousePos) - transform.position).normalized;
        rotateDirection.z = 0.0f;
        if(rotateDirection.x < 0.0f)
        {
            rotateDirection.x = 0.0f;
        }
        //rotateDirection.x = Mathf.Max(rotateDirection.x, 0.0f);

        if (Vector3.zero != rotateDirection && rotateDirection.x >= 0.0f)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.right, rotateDirection);
        }



        if (!m_isCutting)
        {
            
            if (Vector3.zero != moveDirection && !m_isDashed)
            {
                m_rigidbody.position += moveDirection.normalized * Time.deltaTime * moveSpeed;
            }
            gameObject.transform.parent = null;
        }
        else
        {
            gameObject.transform.parent = targetLineLender.gameObject.transform;
            gameObject.transform.position += rotateDirection.normalized * Time.deltaTime * cutMoveSpeed;
            //rotateDirection = Vector3.Normalize(rotateDirection.normalized + Vector3.right * 1.75f);

        }

        //if (Vector3.zero != rotateDirection && rotateDirection.x >= 0.0f)
        //{
        //    transform.rotation = Quaternion.FromToRotation(Vector3.right, rotateDirection);
        //}

        if (m_flag.Get(FlagType.GateHitting)){
            m_missTimer += Time.deltaTime;
            if(m_missTimer >= 3.0f)
            {
                m_missTimer = 0.0f;
                m_flag.Set(FlagType.GateHitting, false);
                gameObject.SetActive(true);
                gameObject.transform.position = Vector3.zero;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < m_isCheckedList.Length; i++)
            {
                m_isCheckedList[i] = false;
                myLineLender.positionCount = 0;
                m_lineList.Clear();
                m_scoreList.Clear();
            }
            m_isCutting = true;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3[] linePositions = new Vector3[targetLineLender.positionCount];
            targetLineLender.GetPositions(linePositions);
            for(int i = 0; i < linePositions.Length; i++)
            {

                if (transform.position.x > linePositions[i].x + targetLineLender.transform.position.x - 0.1f && transform.position.x < linePositions[i].x + targetLineLender.transform.position.x + 0.1f)
                {
                    if (!m_isCheckedList[i])
                    {
                        LineInfo info;
                        info.pos = new Vector3(linePositions[i].x, transform.position.y, transform.position.z);
                        info.id = i;

                        m_lineList.Add(info);
                        m_isCheckedList[i] = true;
                        myLineLender.positionCount = m_lineList.ToArray().Length;
                        for (int j = 0; j < m_lineList.ToArray().Length; j++)
                        {
                            myLineLender.SetPosition(j, m_lineList[j].pos);
                        }
                        

                        int num = (int)(transform.position.y - linePositions[i].y);
                        num = (int)Mathf.Clamp(num, -5.0f, 5.0f);
                        if (num < 0)
                        {
                            num = -10;
                        }
                        else
                        {
                            num = 5 - num;
                        }

                        targetLineLender.gameObject.GetComponent<MainLine>().SetScore(i, num);
                        m_scoreList.Add(num);

                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            int sumScore = 0;
            foreach(int score in m_scoreList)
            {
                sumScore += score;
            }
            //text.text = "Score: " + sumScore.ToString();

            for (int i = 0; i < m_lineList.ToArray().Length; i++)
            {
                targetLineLender.gameObject.GetComponent<MainLine>().CuttingBlock(m_lineList[i].id, m_lineList[i].pos.y);
            }

            

            m_isCutting = false;
        }

        if (m_isDashed)
        {
            m_rigidbody.velocity = m_direction * dashPower * m_dashStopTimer;

            m_timer += Time.deltaTime;
            if(m_timer > dashTime)
            {
                m_dashStopTimer -= Time.deltaTime;

                m_dashStopTimer = Mathf.Max(0.0f, m_dashStopTimer);
                if (m_dashStopTimer <= 0.5f)
                {
                    m_rigidbody.velocity = Vector3.zero;
                    m_isDashed = false;
                    m_timer = 0.0f;
                }
            }
        }

        UpdateNozzle();

        
        if(m_rigidbody.velocity.y <= minDownSpeed)
        {
            m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, minDownSpeed, m_rigidbody.velocity.z);
        }

        if (m_HP <= 0)
        {
            Scene loadScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(loadScene.name);
        }

        HPSlider.value =  ((float)m_HP / 3.0f);
        Vector3 offset = new Vector3(0.0f, 0.5f, 0.0f);
        HPSlider.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position + offset);
    }

    public void UpdateNozzle()
    {
        Vector3 dist = transform.rotation * Vector3.forward;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {

            }
            else
            {
                dist = Vector3.zero;
            }

            dist += Vector3.up;

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {

            }
            else
            {
                dist = Vector3.zero;
            }

            dist += Vector3.down;
        }

        dist.x = 0.0f;
        //m_direction = Vector3.Normalize(dist);
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

    public void Damage()
    {
        m_HP--;
    }
}
