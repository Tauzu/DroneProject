using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--これがないとTextクラスを扱えない

/// <summary>
/// ミッションの抽象クラス。
/// これを継承してからミッションの実装を行う。
/// </summary>
public abstract class Mission : MonoBehaviour
{
    public AudioClip missionBGM;
    public GameObject nextMission;

    protected bool complete = false;    //ミッション達成フラグ

    protected Text message;
    protected AudioSource audioSrc;

    CompleteEffect compEffect;

    /// <summary>
    /// ミッションの初期化。
    /// メッセージやBGMの取得のほか、クリア時演出プレハブのインスタンス化も行う。
    /// </summary>
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

    /// <summary>
    /// クミッション達成フラグをチェックし、フラグが立っていればクリア時演出を行う。
    /// クリア時演出が完了すれば、ミッション終了メソッドを呼び出す。。
    /// </summary>
    protected virtual void Update()
    {
        if (complete && !compEffect.start)
        {
            message.text = "";

            if (nextMission) { compEffect.StartEffect(); } else { compEffect.StartFinalEffect(); }
        }

        if (compEffect.finish) { FinalProcedure(); }
    }

    /// <summary>
    /// ミッション終了時メソッド。
    /// 次のミッションをアクティブにしたのち、自身を破壊する。
    /// </summary>
    void FinalProcedure()
    {
        if (nextMission) nextMission.SetActive(true);
        Destroy(this.gameObject);
    }


}
