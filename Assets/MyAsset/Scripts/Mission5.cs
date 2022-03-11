using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission5 : Mission
{
    public GameObject specialAmezonPrefab;
    GameObject specialAmezonClone;

    GameObject dragonObj;
    Transform playerTf;

    Light dLight;
    Color defaultLightColor;
    float defaultLightIntensity;

    Material defaultSky;
    public Material nightSky;

    IEnumerator enumerator;

    bool defeatFlag = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        message.text = "ドラゴン襲来！\nどうにかして倒そう！";

        playerTf = GameObject.Find("Player").transform;

        dragonObj = this.transform.Find("Dragon").gameObject;
        dragonObj.SetActive(true);

        enumerator = MainCoroutine();
        StartCoroutine(enumerator);

        defaultSky = RenderSettings.skybox;
        RenderSettings.skybox = nightSky;
        // RenderSettings.ambientIntensity = 0.1f; // 空の明るさを設定できるかと思ったが、射光の強さだった

        dLight = GameObject.Find("Directional Light").GetComponent<Light>();
        defaultLightColor = dLight.color;
        defaultLightIntensity = dLight.intensity;
        dLight.color = Color.blue;
        dLight.intensity = 0.5f;

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (dragonObj == null && !defeatFlag)
        {
            audioSrc.Stop();
            defeatFlag = true;

            StopCoroutine(enumerator);
            if (specialAmezonClone != null) Destroy(specialAmezonClone);

            RenderSettings.skybox = defaultSky;
            dLight.color = defaultLightColor;
            dLight.intensity = defaultLightIntensity;

            complete = true;
        }

        // if(GameObject.FindGameObjectsWithTag("coin").Length == 0){
        //     CP.ClearNotify();
        // }
    }

    IEnumerator MainCoroutine()
    {
        while (true) {

            if(specialAmezonClone == null)
            {
                specialAmezonClone = Instantiate(specialAmezonPrefab);
                specialAmezonClone.transform.position
                    = playerTf.position + 2f*playerTf.forward + 5f*Vector3.up;
            }
            else if (specialAmezonClone.transform.position.y < -1f)
            {
                Destroy(specialAmezonClone);
            }
            //待機
            yield return new WaitForSeconds(10f);

        }
        
    }
}
