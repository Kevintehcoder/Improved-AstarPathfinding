using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public class ObjectScript : MonoBehaviour
{
    [SerializeField] EnemyDomain enemyDomain;
    [SerializeField] Transform target;


    private List<Vector2> optimizedPath = new();

    Rigidbody2D rb2d;
    LayerMask wallLayer = 1 << 6;

    

    Vector2 nodeTarget;
    Vector2 dir;

    int currentIndex;
    [SerializeField] float moveSpeed = 5f;
    float deltaTargetTransform = 0.5f;

    bool hasPath = false;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(checkTarget());
    }

    private void Update()
    {
        if (enemyDomain.returnCanCreatePath() && Vector2.Distance(transform.position, target.position) > deltaTargetTransform)
        {
            if (hasPath == false)
            {
                startPathFind();
                hasPath = true;
            } 
        }

        if (enemyDomain.returnCanCreatePath() == false)
        {
            if (optimizedPath.Count > 0)
            {
                hasPath = false;
                optimizedPath.Clear();
            }
        }


    }

    private void FixedUpdate()
    {
        if (optimizedPath != null && optimizedPath.Count > 0)
        {
            if (PastNode())
            {
                currentIndex++;
                if (currentIndex < optimizedPath.Count)
                {
                    nodeTarget = optimizedPath[currentIndex];
                }
               //Debug.Log("New Node Target: " + nodeTarget);
            }


            if (currentIndex >= optimizedPath.Count)
            {
                hasPath = false;
                optimizedPath.Clear();
                dir = Vector2.zero;

            }
            else
            {
                setDirTowards(nodeTarget);
                
            }
        }
        else
        {
            dir = Vector2.zero;
        }

        rb2d.linearVelocity = dir * moveSpeed;
    }

    [ContextMenu("FollowPath")]
    void startPathFind()
    {
        //Debug.Log("Starting Path Find");
        currentIndex = 0;
        buildPath();

        if (optimizedPath != null && optimizedPath.Count > 0)
        {
            nodeTarget = optimizedPath[0];

            setDirTowards(nodeTarget);

            //Debug.Log("Node Target: " + nodeTarget);
        }

    }

    void buildPath()
    {
        optimizedPath.Clear();
        Nodes[] path = AStarPathfinding.instance.pathFind(transform, target.transform);

        if (path == null)
        {
            //Debug.LogWarning("No valid path");
            return;
        }

        int currentIndex = 0;
        float enemyRadius = 0.4f;

        while (currentIndex < path.Length)
        {
            for (int i = path.Length - 1; i > currentIndex; i--)
            {
                float distance = Vector2.Distance(path[currentIndex].transform.position, path[i].transform.position);

                if (distance < 2f) 
                {
                    RaycastHit2D hit = Physics2D.CircleCast(
                        path[currentIndex].transform.position,
                        enemyRadius,
                        (path[i].transform.position - path[currentIndex].transform.position).normalized,
                        distance,
                        wallLayer
                    );

                    if (!hit)
                    {
                        currentIndex = i;
                        break;
                    }
                }
            }
            
            optimizedPath.Add(path[currentIndex].transform.position);

            currentIndex++;

        }

       //Debug.Log("Optimized path node count: " + optimizedPath.Count);
    }

    void setDirTowards(Vector2 pos)
    {
        Vector2 targetDir = (pos - (Vector2)transform.position).normalized;

        dir = Vector3.Slerp(dir, targetDir, moveSpeed * Time.fixedDeltaTime);
    }

    bool PastNode()
    {
        if (Vector2.Distance(transform.position, nodeTarget) < 0.5f)
        {
                return true;
            
        }
        return false;
    }

    IEnumerator checkTarget()
    {
        WaitForSeconds seconds = new WaitForSeconds(0.1f);
        while (true)
        {
            Vector2 PrevPos = target.position;

            yield return seconds;

            if (Vector2.Distance(PrevPos, target.position) > deltaTargetTransform)
            {
                hasPath = false;
            }

            
        }
    }
   
}

