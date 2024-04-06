using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public AudioSource audio;
    public AudioClip[] clip = new AudioClip[3];
    private int m_clipNum = 0;

	// Use this for initialization
	void Start () {
        audio.clip = clip[0];
        audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
        if (!audio.isPlaying)
        {
            m_clipNum++;

            if(m_clipNum >= clip.Length)
            {
                m_clipNum = 0;
            }
            audio.clip = clip[m_clipNum];
            audio.Play();
        }
	}
}
