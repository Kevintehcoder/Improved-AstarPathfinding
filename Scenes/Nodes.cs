using System.Collections.Generic;
using UnityEngine;

public class Nodes : MonoBehaviour
{
    public float fScore { get; private set; } = 0;


[HideInInspector]
    public float gscore{ get; private set; } = 0;

    float hscore;

    protected static float detectionRange = 6;
    protected LayerMask detectionLayer = 1 << 3;
    protected LayerMask wallLayer = 1 << 6;

    public Nodes CameFrom { get; private set; }

    public List<Nodes> SurroundingNodes = new();


    void Start()
    {

        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, detectionRange, detectionLayer);
        foreach (Collider2D col in hit)
        {
            RaycastHit2D isWall = Physics2D.Linecast(transform.position, col.transform.position, wallLayer);
            if (!isWall)
            {
                Nodes neighborNodes = col.GetComponent<Nodes>();
                if (neighborNodes != this)
                {
                    SurroundingNodes.Add(neighborNodes);
                }
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.white;
        foreach (Nodes node in SurroundingNodes)
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }

    }


    public float findGscore(Transform focus, float currentGscore)
    {
        return currentGscore + Vector2.Distance(transform.position, focus.transform.position);
    }

    public void findFscore()
    {
        fScore = gscore + hscore;
    }

    public void setGscore(float value)
    {
        gscore = value;
    }

    public void Setorigin(Nodes origin)
    {
        CameFrom = origin;
    }

    public void resetNode(Transform end)
    {
        fScore = 0;
        gscore = float.MaxValue;
        hscore = Vector3.Distance(transform.position, end.position);
        CameFrom = null;
    }
}
