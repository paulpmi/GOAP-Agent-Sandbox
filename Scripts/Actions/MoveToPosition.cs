using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosition : GoapAction
{

    private bool isComplete = false;
    private AILogic aILogic;
    public MoveToPosition() {
        addPrecondition("has_waypoint", true);
        //addEffect("is_moving", true);
        //addEffect("has_waypoint", false);
        addEffect("is_safe", true);

        //aILogic = GetComponent<AILogic>();
        name = "Mover";
        cost = 9f;
    }


    public void Start()
    {

        aILogic = GetComponent<AILogic>();
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        return true;
    }

    public override bool isDone()
    {
        return isComplete;
    }

    public override bool perform(GameObject agent)
    {
        Debug.Log("...Moving to new position");
        if (aILogic.isPathSafe)
        {
            aILogic.GoToPosition();
            isComplete = true;
            return true;
        }
        else
            return false;
    }

    public override bool requiresInRange()
    {
        return false;
    }

    public override void reset()
    {
        isComplete = false;
    }
}
