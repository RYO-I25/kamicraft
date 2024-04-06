using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class LineEditor : MonoBehaviour {

    public List<GameObject> referObjs = new List<GameObject>();
    public List<Vector3> m_lineList = new List<Vector3>();

    // Use this for initialization
    private void Start () {
        Vector3 defaultPos = Vector3.zero;
        foreach (GameObject referObj in referObjs)
        {
            if (referObj)
            {
                for (int i = 0; i < referObj.transform.childCount; i++)
                {
                    Vector3 pos = referObj.transform.GetChild(i).transform.position + defaultPos;
                    m_lineList.Add(pos);
                }
                defaultPos = m_lineList[m_lineList.ToArray().Length - 1];
            }
        
        }
	}
	
	// Update is called once per frame
	void Update () {
    }

    public List<Vector3> GetLineList()
    {
        return m_lineList;
    }
}
