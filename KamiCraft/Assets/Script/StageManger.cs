using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManger : MonoBehaviour {

    public GameObject mainCamera;
    public GameObject subCamera;
    public GameObject[] allStages;
    private GameObject[,] stages = new GameObject[3,3];
    private Vector3[,] stagePositions = new Vector3[3,3];

    private bool m_isModeChange = false;
    private GameObject m_player;
    private int m_keyUpNum = 0;
    private int m_keySideNum = 0;
    private int m_firstKeyUpNum = 0;
    private int m_firstKeySideNum = 0;
    public bool m_firstStageSelected = false;
    private GameObject m_firstSelectedStage;
    private GameObject m_firstStage;
    private GameObject m_secondStage;

    // Use this for initialization
    void Start () {
        m_player = GameObject.FindGameObjectWithTag("Player");
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                stages[i, j] = allStages[i * 3 + j];
                stagePositions[i,j] = stages[i,j].transform.position;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (m_isModeChange)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                ResetStageChangeMode();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                m_keySideNum++;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                m_keySideNum--;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                m_keyUpNum--;
            }
            else if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                m_keyUpNum++;
            }
            m_keySideNum = Mathf.Clamp(m_keySideNum, 0, 2);
            m_keyUpNum = Mathf.Clamp(m_keyUpNum, 0, 2);


            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!m_firstStageSelected)
                {
                    m_firstSelectedStage = stages[m_keyUpNum, m_keySideNum];
                    m_firstStageSelected = true;
                    m_firstKeyUpNum = m_keyUpNum;
                    m_firstKeySideNum = m_keySideNum;
                    Debug.Log(m_keyUpNum + ":" + m_keySideNum);
                }
                else
                {
                    Vector3 position = m_firstSelectedStage.transform.localPosition;
                    GameObject secondStage = stages[m_keyUpNum, m_keySideNum];
                    m_firstSelectedStage.transform.localPosition = stages[m_keyUpNum, m_keySideNum].transform.localPosition;
                    stages[m_keyUpNum, m_keySideNum].transform.localPosition = position;

                    stages[m_keyUpNum, m_keySideNum] = m_firstSelectedStage;
                    stages[m_firstKeyUpNum, m_firstKeySideNum] = secondStage; 

                    m_firstStageSelected = false;
                    Debug.Log(m_keyUpNum + ":" + m_keySideNum);
                }
            }
        }
	}

    public void SetStageChangeMode()
    {
        m_player.SetActive(false);
        mainCamera.SetActive(false);
        subCamera.SetActive(true);
        m_isModeChange = true;
        m_firstStageSelected = false;
        m_keySideNum = 0;
        m_keyUpNum = 0;
        Debug.Log("ステージ変えれるモード");
    }

    public void ResetStageChangeMode()
    {
        m_player.SetActive(true);
        mainCamera.SetActive(true);
        subCamera.SetActive(false);
        m_isModeChange = false;
        m_firstStageSelected = false;
        Debug.Log("通常モード");
    }
}
