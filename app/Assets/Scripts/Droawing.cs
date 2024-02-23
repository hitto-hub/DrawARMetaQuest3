using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static OVRPlugin;

public class Droawing : MonoBehaviour
{
    [SerializeField] GameObject lineObjectPrefab; //プレハブ化した空のオブジェクト（Line）を入れる
    private GameObject currentLineObject = null;

    [SerializeField] private GameObject handObject; //書きたい方の手を入れる
    private OVRHand _hand;　//ハンドトラッキングをしているかの判定に使う
    private OVRSkeleton _skeleton; //Bone情報
    [SerializeField] private float touchDistanceThreshold = 0.01f; //人差し指と親指がこの値以上近づいたら線を書く

    private void Start()
    {
        _hand = handObject.GetComponent<OVRHand>();
        _skeleton = handObject.GetComponent<OVRSkeleton>();
    }

    void Update()
    {
        if (!_hand.IsTracked) //ハンドトラッキングしているかの確認
        {
            return;
        }

        var indexTipPos = _skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position; //人差し指先端の位置取得
        var thumbTipPos = _skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_ThumbTip].Transform.position; //親指先端の位置取得

        float distanceR = Vector3.Distance(indexTipPos, thumbTipPos); //人差し指先端と親指先端の距離

        if (distanceR < touchDistanceThreshold) //線を書く処理
        {
            if (currentLineObject == null)
            {
                currentLineObject = Instantiate(lineObjectPrefab, Vector3.zero, Quaternion.identity);
            }
            LineRenderer lineRenderer = currentLineObject.GetComponent<LineRenderer>();

            int nextPositionIndex = lineRenderer.positionCount;
            lineRenderer.positionCount = nextPositionIndex + 1;
            lineRenderer.SetPosition(nextPositionIndex, indexTipPos);
        }
        else //線を書かない時の処理
        {
            if (currentLineObject != null)
            {
                currentLineObject = null;
            }
        }
    }
}