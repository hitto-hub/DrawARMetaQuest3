using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static OVRPlugin;

public class Droawing : MonoBehaviour
{
    [SerializeField] GameObject lineObjectPrefab; //�v���n�u��������̃I�u�W�F�N�g�iLine�j������
    private GameObject currentLineObject = null;

    [SerializeField] private GameObject handObject; //�����������̎������
    private OVRHand _hand;�@//�n���h�g���b�L���O�����Ă��邩�̔���Ɏg��
    private OVRSkeleton _skeleton; //Bone���
    [SerializeField] private float touchDistanceThreshold = 0.01f; //�l�����w�Ɛe�w�����̒l�ȏ�߂Â������������

    private void Start()
    {
        _hand = handObject.GetComponent<OVRHand>();
        _skeleton = handObject.GetComponent<OVRSkeleton>();
    }

    void Update()
    {
        if (!_hand.IsTracked) //�n���h�g���b�L���O���Ă��邩�̊m�F
        {
            return;
        }

        var indexTipPos = _skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position; //�l�����w��[�̈ʒu�擾
        var thumbTipPos = _skeleton.Bones[(int)OVRSkeleton.BoneId.Hand_ThumbTip].Transform.position; //�e�w��[�̈ʒu�擾

        float distanceR = Vector3.Distance(indexTipPos, thumbTipPos); //�l�����w��[�Ɛe�w��[�̋���

        if (distanceR < touchDistanceThreshold) //������������
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
        else //���������Ȃ����̏���
        {
            if (currentLineObject != null)
            {
                currentLineObject = null;
            }
        }
    }
}