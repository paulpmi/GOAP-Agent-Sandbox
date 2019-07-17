using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILogic2 : MonoBehaviour, IGoap
{
    //AILogic is actually the Head of the character probably with a bone attached with which it can rotate

    public List<Transform> possibleWaypoints = new List<Transform>();
    public bool isPathSelected = false;
    public NavMeshAgent agent;

    public float viewRadius = 100f;
    [Range(0, 360)]
    public float viewAngle = 180;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public float dangerDistance = 20f;

    public float speed = 2f;
    public float maxRotation = 45f;
    public NavMeshPath path;

    public bool isPathSafe;

    // Start is called before the first frame update
    void Start()
    {
        path = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();



        //goals.Add(GoapGoal.Goals.CHANGE_POSITION + '1', changePosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.hasPath)
        {
            Vector3 toTarget = agent.steeringTarget - this.transform.position;
            float turnAngle = Vector3.Angle(this.transform.forward, toTarget);
            agent.acceleration = turnAngle * agent.speed;
        }
    }


    public void CollisionDetected(Collision collision)
    {
        foreach (var c in collision.contacts)
        {
            if (c.otherCollider.transform.tag == "cover")
                possibleWaypoints.Add(c.otherCollider.transform);
        }
    }

    public void DangerCollision()
    {
        Debug.Log("Found Danger");
        isPathSafe = false;
    }

    public void FindPosition()
    {
        isPathSelected = false;
        //Decover()
        Scan();
        //Cover()
        //StopScan();
        FindWaypoint();
    }

    public void GoToPosition()
    {
        if (path.corners != null)
        {
            int index = path.corners.Length - 1;
            if (index < 0)
                index = 0;
            agent.path = path;
            //agent.SetDestination(path.corners[index]);
        }
        else
            Debug.Log("..+No path exists");
        //agent.Move();        
    }

    private void Scan()
    {

        possibleWaypoints.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        //Debug.Log("..+Targets in Radius: " + targetsInViewRadius.Length);
        //transform.rotation = Quaternion.Euler(maxRotation * Mathf.Sin(Time.time * speed), 0f, 0f);
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            //Debug.Log("..+Tag Found: " + targetsInViewRadius[i].transform.tag);
            if (targetsInViewRadius[i].transform.tag == "cover")
            {
                //Debug.Log(".+FOUND COVER");
                possibleWaypoints.Add(targetsInViewRadius[i].transform);
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                    {
                        //possibleWaypoints.Add(target);
                    }
                }
            }
        }

    }

    private void StopScan()
    {


    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void FindWaypoint()
    {
        SortedDictionary<int, NavMeshPath> dict = new SortedDictionary<int, NavMeshPath>();
        List<int> keys = new List<int>();

        foreach (var w in possibleWaypoints)
        {
            if (CheckPath(w))
            {
                if (keys.Contains(path.corners.Length))
                {
                    keys.Remove(path.corners.Length);
                    dict.Remove(path.corners.Length);
                }
                keys.Add(path.corners.Length);
                dict.Add(path.corners.Length, path);
            }
        }

        if (dict.Keys.Count > 0)
        {
            isPathSelected = true;
            path = dict[keys[0]];
        }
        else
        {
            isPathSelected = false;
        }

        possibleWaypoints.Clear();
    }

    private bool CheckPath(Transform waypoint)
    {
        isPathSafe = true;
        NavMesh.CalculatePath(transform.position, waypoint.position, NavMesh.AllAreas, path);
        Debug.Log("CHECKING");

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("player");

        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(waypoint.transform.position, enemy.transform.position) < dangerDistance)
            {
                Debug.Log("Too Dangerous");
                isPathSafe = false;
                return false;
            }
        }

        return true;
        /*
        List<GameObject> gameObjects = new List<GameObject>();
        for (int i = 0; i < path.corners.Length; i++) {
            gameObjects.Add( Instantiate(PathColliderObject, path.corners[i], Quaternion.identity) );
        }

        
       
        for (int i = 0; i < gameObjects.Count; i++)
            Destroy( gameObjects[i] );
        gameObjects.Clear();
        */
        //return false;


        //Debug.Log("IS PATH SAFE: " + isPathSafe);
        //return isPathSafe;

    }

    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        HashSet<KeyValuePair<string, object>> wordData = new HashSet<KeyValuePair<string, object>>();
        wordData.Add(new KeyValuePair<string, object>("has_waypoint", false));

        return wordData;
    }

    public HashSet<KeyValuePair<string, object>> CreateGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("changePosition", true));
        //goal.Add(new KeyValuePair<string, object>("has_waypoint", true));

        return goal;
    }

    public void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
        //foreach (var kv in failedGoal)
        //Debug.Log("Plan Failed: " + kv);
    }

    public void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
    {
        //Debug.Log("Found Plan");
    }

    public void ActionsFinished()
    {
        //Debug.Log("Action Finished");
    }

    public void PlanAborted(GoapAction aborter)
    {
        //Debug.Log("PLAN ABORTED: " + aborter.name );
    }

    public bool MoveAgent(GoapAction nextAction)
    {
        if (path.corners[0] == nextAction.target.transform.position)
        {
            nextAction.setInRange(true);
            return true;
        }

        agent.SetDestination(nextAction.target.transform.position);

        if (agent.hasPath && agent.remainingDistance < 2)
        {
            nextAction.setInRange(true);
            path.corners[0] = nextAction.target.transform.position;
            return true;
        }

        return false;
    }
}
