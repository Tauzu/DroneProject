using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--これがないとTextクラスを扱えない

public abstract class Mission : MonoBehaviour
{
    public AudioClip missionBGM;
    public GameObject nextMission;

    protected bool complete = false;

    protected Text message;
    protected AudioSource audioSrc;

    CompleteEffect compEffect;

    protected virtual void Start()
    {
        message = GameObject.Find("MessageText").GetComponent<Text>();

        audioSrc = GameObject.Find("BGMAudio").GetComponent<AudioSource>();

        if (missionBGM != null)
        {
            audioSrc.clip = missionBGM;
            audioSrc.Play();
        }

        GameObject compEffectObj = (GameObject)Resources.Load("CompleteEffect");
        GameObject clone = Instantiate(compEffectObj);
        clone.transform.parent = this.transform;
        compEffect = clone.GetComponent<CompleteEffect>();
    }

    protected virtual void Update()
    {
        if (complete && !compEffect.start)
        {
            message.text = "";

            if (nextMission) { compEffect.StartEffect(); } else { compEffect.StartFinalEffect(); }
        }

        if (compEffect.finish) { FinalProcedure(); }
    }

    void FinalProcedure()
    {
        if (nextMission) nextMission.SetActive(true);
        Destroy(this.gameObject);
    }


}
