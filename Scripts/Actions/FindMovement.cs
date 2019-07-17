using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMovement : GoapAction
{
    private bool isComplete = false;
    private AILogic aILogic;
    float startTime = 0;
    public float workDuration = 2;

    public FindMovement() {
        //add precondition to be in danger
        //addPrecondition("is_moving", false);
        addPrecondition("has_waypoint", false);
        addEffect("has_waypoint", true);

        //name = "Finder";
        
        name = "Finder";
        cost = 2f;
    }

    public void Start() {

        aILogic = GetComponent<AILogic>();
    }


    public override bool checkProceduralPrecondition(GameObject agent)
    {
        return true;
    }

    public override bool isDone()
    {
        Debug.Log("...Ending Finding");
        return isComplete;
    }

    public override bool perform(GameObject agent)
    {
        Debug.Log("...Starting Finding");
        aILogic.FindPosition();
        if (aILogic.isPathSafe)
        {
            isComplete = true;

            return true;
        }
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
