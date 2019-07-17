using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCollider : MonoBehaviour
{
    // Start is called before the first frame update

    public float timer = 2f;
    void Start()
    {
        Destroy(this, timer);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Found Collision: CHILD");
        foreach (var c in collision.contacts)
        {
            if (c.otherCollider.transform.tag == "enemy" || c.otherCollider.transform.tag == "danger")
                transform.parent.GetComponent<AILogic>().DangerCollision();
        }
    }
}
