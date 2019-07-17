using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : GoapAction
{

    private bool isComplete = false;
    private AILogic aILogic;
    public List<GameObject> enemiesInImmediateDistance;
    public float immediateDangerDistance = 100f;
    public float timeBetweenAttacks = 100f;
    public float stopAndShootTime = 50f;

    public AttackEnemy()
    {
        //addPrecondition("has_waypoint", true);
        //addEffect("is_moving", true);
        //addEffect("has_waypoint", false);
        addEffect("defend", true);
        addEffect("is_safe", false);
        cost = 100f;

        //aILogic = GetComponent<AILogic>();
        name = "Mover";
        timeBetweenAttacks = 100f;
        stopAndShootTime = 50f;
        enemiesInImmediateDistance = new List<GameObject>();

    }


    public void Start()
    {

        aILogic = GetComponent<AILogic>();
    }

    public void Update() {

        CheckEnemies();
    }

    private void CheckEnemies()
    {

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        Debug.Log("1. Enemies: " + enemies.Length);
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < immediateDangerDistance)
            {
                if (!enemiesInImmediateDistance.Contains(enemy))
                {
                    aILogic.isSafe = false;
                    enemiesInImmediateDistance.Add(enemy);
                    cost = enemiesInImmediateDistance.Count * 2f;
                    aILogic.isInCombat = true;
                    aILogic.closestEnemyPosition = enemy;
                }
            }
            else
                if (enemiesInImmediateDistance.Contains(enemy))
            {
                enemiesInImmediateDistance.Remove(enemy);
                cost = enemiesInImmediateDistance.Count * 2f;
            }
        }
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
        if (timeBetweenAttacks <= 0f)
            timeBetweenAttacks -= Time.deltaTime;
        else
        {
            aILogic.agent.isStopped = true;

            if (stopAndShootTime > 0f)
            {
                Debug.Log("...ATTACKING");
                for (int i = 0; i < enemiesInImmediateDistance.Count; i++)
                    aILogic.Attack(enemiesInImmediateDistance[i].transform);
                isComplete = true;
                aILogic.agent.isStopped = false;
                return true;
            }
            else
                stopAndShootTime -= Time.deltaTime;
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
