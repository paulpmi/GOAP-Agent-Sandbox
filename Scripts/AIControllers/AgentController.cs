using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<AILogic> aILogics;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        isAgentInCombat();
    }

    private void isAgentInCombat() {
        for (int i = 0; i < aILogics.Count; i++) {
            if (aILogics[i].isInCombat)
            {
                findNearestWaypointToEnemy(i);
                break;
            }
                
        }
    }

    private void findNearestWaypointToEnemy(int indexOfEngaged) {
        GameObject[] covers = GameObject.FindGameObjectsWithTag("cover");
        float minim = Mathf.Infinity;
        GameObject closestCover = covers[0];

        foreach (var cover in covers) {
            var distance = Vector3.Distance(cover.transform.position, aILogics[indexOfEngaged].closestEnemyPosition.transform.position);
            if ( distance < minim) {
                minim = distance;
                closestCover = cover;
            }
        }

        NavMeshPath navMeshPath = new NavMeshPath();
        for(int i = 0; i < aILogics.Count; i++)
        {
            if (i != indexOfEngaged)
            {
                aILogics[i].agent.CalculatePath(closestCover.transform.position, navMeshPath);
                aILogics[i].agent.path = navMeshPath;
                aILogics[i].path = navMeshPath;
            }
        }
    }

    private void SetOneActionToAll() {

    }

    private void SetOneActionToOne() {

    }
}
