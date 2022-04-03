using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public const int StageTipSize = 200;

    int currentTipIndex;
    public GameObject[] stageTips;
    public int startTipIndex;

    public int preInstantiate;
    List<GameObject> generatedStageList = new List<GameObject>();
    void Start()
    {
        //初期化処理
        currentTipIndex = startTipIndex - 1;
        UpdateStage(preInstantiate);
    }
    void Update()
    {
        int charaPositionIndex = ((int)(this.transform.position.z) / StageTipSize);
        if (charaPositionIndex + preInstantiate > currentTipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        };

    }
    void UpdateStage(int toTipIndex)
    {
        if (toTipIndex <= currentTipIndex) return;
        for (int i = currentTipIndex; i < toTipIndex; i++)
        {

            GameObject stageObject = RundomMapGene(i);
            generatedStageList.Add(stageObject);
        }

        while (generatedStageList.Count > preInstantiate) DestroyOldestStage();

        currentTipIndex = toTipIndex;
    }
    GameObject RundomMapGene(int tipIndex)
    {
        int nextStageTip = Random.Range(0, stageTips.Length -1);

        GameObject stageObject = (GameObject)Instantiate(
            stageTips[nextStageTip],
            new Vector3(0, 0, tipIndex * StageTipSize),
            Quaternion.identity);

        return stageObject;
    }

    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }
}