using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MainLine : MonoBehaviour
{
    const float FIX_DISTANCE = 0.1f;

    [SerializeField]
    private LineRenderer _lineRenderer;
    /// <summary>
    /// 波の周期
    /// </summary>
    /// 

    private AudioSource source;

    private float m_moveTimer = 0.0f;

    private float[] m_data = new float[1000];
    private Vector3[] m_startPos = new Vector3[1000];
    private GameObject[] blocks = new GameObject[1000];
    private int[] scores = new int[1000];
    private bool[] isScored = new bool[1000];
    private int sumScore;

    public GameObject cube;
    public GameObject lineEditor;
    public Text text;
    public bool useDubug = true;
    public int[] counts = new int[4];
    public GameObject[] noiseLineEditors;
    public GameObject noiseLineObj;

    private void Start()
    {
        if (useDubug)
        {
            var source = GetComponent<AudioSource>();
            var clip = source.clip;
            source.clip.GetData(m_data, source.timeSamples);

            Vector2[] points = new Vector2[1000];

            _lineRenderer.positionCount = m_data.Length;

            for (var i = 0; i < m_data.Length; i++)
            {
                var pos = Vector3.right * i * FIX_DISTANCE;
                pos.y = m_data[i] * 2.0f;
                _lineRenderer.SetPosition(i, pos);
                m_startPos[i] = pos;
                points[i] = new Vector2(_lineRenderer.GetPosition(i).x, _lineRenderer.GetPosition(i).y);
                blocks[i] = Instantiate(cube, new Vector3(points[i].x + 7.0f, 0.0f, 1.1f), Quaternion.identity) as GameObject;
                blocks[i].transform.parent = transform;
            }
        }
        else
        {
            {
                List<Vector3> lineList = lineEditor.GetComponent<LineEditor>().GetLineList();
                _lineRenderer.positionCount = (int)(Mathf.Floor((lineList[lineList.ToArray().Length - 1].x - lineList[0].x) / FIX_DISTANCE));
                Vector2[] points = new Vector2[(int)(Mathf.Floor((lineList[lineList.ToArray().Length - 1].x - lineList[0].x) / FIX_DISTANCE))];
                counts = new int[lineList.ToArray().Length];

                for (var i = 0; i < lineList.ToArray().Length - 1; i++)
                {
                    counts[i] = (int)Mathf.Floor((lineList[i + 1].x - lineList[i].x) / FIX_DISTANCE);
                    var count = 0;

                    for (var j = 0; j < i; j++)
                    {
                        count += counts[j];
                    }

                    var maxCount = counts[i];

                    if (i == lineList.ToArray().Length - 2)
                    {
                        maxCount = counts[i] + 1;
                    }

                    for (var j = 0; j < maxCount; j++)
                    {
                        if (points.Length > count + j)
                        {
                            var ratio = (float)j / (float)counts[i];
                            var lerpPos = Vector3.Lerp(lineList[i], lineList[i + 1], ratio);

                            var pos = Vector3.right * (lineList[i].x + j * FIX_DISTANCE);
                            pos.y = lerpPos.y;
                            _lineRenderer.SetPosition(count + j, pos);
                            m_startPos[count + j] = pos;
                            points[count + j] = new Vector2(_lineRenderer.GetPosition(count + j).x, _lineRenderer.GetPosition(count + j).y);
                            blocks[count + j] = Instantiate(cube, new Vector3(points[count + j].x + 7.0f, 0.0f, 1.1f), Quaternion.identity) as GameObject;
                            blocks[count + j].transform.parent = transform;
                        }
                    }
                }
            }

            {
                for(int index = 0;index < noiseLineEditors.Length; ++index)
                {
                    List<Vector3> lineList = noiseLineEditors[index].GetComponent<LineEditor>().GetLineList();
                    GameObject lineObj = Instantiate(noiseLineObj);
                    lineObj.transform.parent = transform;
                    LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
                    lineRenderer.positionCount = (int)(Mathf.Floor((lineList[lineList.ToArray().Length - 1].x - lineList[0].x) / FIX_DISTANCE));
                    Vector2[] points = new Vector2[(int)(Mathf.Floor((lineList[lineList.ToArray().Length - 1].x - lineList[0].x) / FIX_DISTANCE))];
                    counts = new int[lineList.ToArray().Length];

                    for (var i = 0; i < lineList.ToArray().Length - 1; i++)
                    {
                        counts[i] = (int)Mathf.Floor((lineList[i + 1].x - lineList[i].x) / FIX_DISTANCE);
                        var count = 0;

                        for (var j = 0; j < i; j++)
                        {
                            count += counts[j];
                        }

                        var maxCount = counts[i];

                        if (i == lineList.ToArray().Length - 2)
                        {
                            maxCount = counts[i] + 1;
                        }

                        for (var j = 0; j < maxCount; j++)
                        {
                            if (points.Length > count + j)
                            {
                                var ratio = (float)j / (float)counts[i];
                                var lerpPos = Vector3.Lerp(lineList[i], lineList[i + 1], ratio);

                                var pos = Vector3.right * (lineList[i].x + j * FIX_DISTANCE);
                                pos.y = lerpPos.y;
                                lineRenderer.SetPosition(count + j, pos);
                                m_startPos[count + j] = pos;
                                points[count + j] = new Vector2(lineRenderer.GetPosition(count + j).x, lineRenderer.GetPosition(count + j).y);
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < scores.Length;i++)
        {
            scores[i] = -1;
            isScored[i] = false;
        }

        //collider.points = points;
    }

    private void FixedUpdate()
    {

        transform.position += Vector3.left * Time.deltaTime * 0.9f;

        if(transform.position.x < -6.0f)
        {
            int i = (int)(Mathf.Abs(transform.position.x + 6.0f) / FIX_DISTANCE);

            if (!isScored[i])
            {
                isScored[i] = true;
                sumScore += scores[i];
                text.text = "Score: " + sumScore.ToString();
            }
        }

    }

    public void CuttingBlock(int id, float Ypos)
    {
        if (blocks[id])
        {
            blocks[id].transform.position = new Vector3(blocks[id].transform.position.x, (Ypos - 7.0f / 2.0f) / 2.0f, blocks[id].transform.position.z);
            blocks[id].transform.localScale = new Vector3(FIX_DISTANCE, 7.0f / 2.0f + Ypos , 0.01f);
        }
    }

    public void SetScore(int id, int score)
    {
        scores[id] = score;
    }
}
