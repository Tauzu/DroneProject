using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public AudioClip defaultBGM;
    //public AudioClip goalBGM;
    //public AudioClip winBGM;

    AudioSource audioSource;

    // ScoreManager SMscript;

 //   public GameObject goalObj;
	//public GameObject winObj;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();

        //PlayBGM(defaultBGM);

        // SMscript = this.GetComponent<ScoreManager>();

    }

    // Update is called once per frame
    void Update()
    {
        //if((goalObj.activeSelf) && !(winObj.activeSelf))
        //{
        //    PlayBGM(goalBGM);

        //}else if(winObj.activeSelf)
        //{
        //    PlayBGM(winBGM);
        //}

    }

    public void PlayBGM(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
        
    }

    public void PlayDefaultBGM()
    {
        audioSource.clip = defaultBGM;
        audioSource.Play();

    }
}
