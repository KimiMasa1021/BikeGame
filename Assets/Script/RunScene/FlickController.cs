using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlickController : MonoBehaviour
{
    private Vector3 touchStartPos;
    private Vector3 touchEndPos;
    public string Direction;
    static public FlickController instance;


    private void Start()
    {
        if (instance == null)
            instance = this;
    }
    private void Update()
    {
        Flick();
    }
    public void Flick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchStartPos = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Input.mousePosition.z
            );
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            touchEndPos = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Input.mousePosition.z
            );

            GetDirection();
        }
    }

    void GetDirection()
    {
        float directionX = touchEndPos.x - touchStartPos.x;
        float directionY = touchEndPos.y - touchStartPos.y;

        if (Mathf.Abs(directionY) < Mathf.Abs(directionX))
        {
            if (30 < directionX)
            {
                Direction = "right";
            }
            else if (-30 > directionX)
            {
                Direction = "left";
            }
        }
        else if (Mathf.Abs(directionX) < Mathf.Abs(directionY))
        {
            if (30 < directionY)
            {
                Direction = "up";
            }
            else if (-30 > directionY)
            {
                Direction = "down";
            }
        }
        else
        {
            Direction = "touch";
        }

        switch (Direction)
        {
            case "right":
                if (BikeContller.instance.freeOJ != "right")
                {
                    BikeContller.instance.RmConstraint();
                    BikeContller.instance.RightMove();
                }
                return;
            case "left":
                if (BikeContller.instance.freeOJ != "left")
                {
                    BikeContller.instance.RmConstraint();
                    BikeContller.instance.LeftMove();
                }
                return;
            case "up":
                BikeContller.instance.Jump();
                return;
            case "down":
                if (!BikeContller.instance.angleLFlg && !BikeContller.instance.angleRFlg)
                {
                    IKtest.instance.LFootChange();
                    BikeContller.instance.slidingIvent();
                }
                return;
            default:
                return;
        }
    }
}
