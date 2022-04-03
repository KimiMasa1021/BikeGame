using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
[RequireComponent(typeof(Animator))]
public class IKtest : MonoBehaviour
{
    protected Animator animator;

    [SerializeField]
    private Transform _leftHandIkTarget;
    [SerializeField]
    private Transform _rightHandIkTarget;
    [SerializeField]
    private Transform _leftfootIkTarget;
    [SerializeField]
    private Transform _rightfootIkTarget;
    public bool LFootFlg;
    static public IKtest instance;

    void Start()
    {
        animator = GetComponent<Animator>();
        LFootFlg = true;
        if(instance==null)
            instance = this;
    }

    private void OnAnimatorIK()
    {


        if (LFootFlg)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, _leftfootIkTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, _leftfootIkTarget.rotation);

        }

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandIkTarget.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandIkTarget.rotation);

        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandIkTarget.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, _rightHandIkTarget.rotation);

        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
        animator.SetIKPosition(AvatarIKGoal.RightFoot, _rightfootIkTarget.position);
        animator.SetIKRotation(AvatarIKGoal.RightFoot, _rightfootIkTarget.rotation);

    }

    public async void LFootChange()
    {
        LFootFlg = false;
        animator.SetBool("Sliding", true);
        await Task.Delay(600);
        LFootFlg = true;
        animator.SetBool("Sliding", false);

    }

    /*public void LFootReturn()
    {
        LFootFlg = true;
        animator.SetBool("Sliding", false);
    }*/
}


