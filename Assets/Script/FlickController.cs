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
    public Text flic;


    private void Start() {
        if(instance == null)
            instance = this;
    }
    private void Update() {
        Flick();
    }
    public void Flick()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchStartPos = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Input.mousePosition.z
            );
        }

        if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            touchEndPos = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Input.mousePosition.y
            );

            GetDirection();
        }
    }

    void GetDirection()
    {
        float directionX = touchEndPos.x - touchStartPos.x;
        float directionY = touchEndPos.y - touchStartPos.y;

        if(Mathf.Abs(directionY) < Mathf.Abs(directionX))
        {
            if(1 < directionX)
            {
                Direction = "right";
            }else if(-1 > directionX)
            {
                Direction = "left";
            }
        }else if(Mathf.Abs(directionX) < Mathf.Abs(directionY))
        {
            if(1 < directionY)
            {
                Direction = "up";
            }else if(-1 > directionY)
            {
                Direction = "down";
            }
        }else
        {
            Direction = "touch";
        }
        flic.text = Direction;

        switch (Direction)
        {
            case "right":
                BikeContller.instance.RightMove();
                return;
            case "left":
                BikeContller.instance.LeftMove();
                return;
            case "up":
                BikeContller.instance.Jump();
                return;
            case "down":
                IKtest.instance.LFootChange();
                BikeContller.instance.slidingIvent();
                return;
            default:
                return;
        }
    }
}
