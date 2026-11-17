using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class TrackerNode : Nodes
{
    CircleCollider2D Circlecollider;
    void Start()
    {

        Circlecollider = GetComponent<CircleCollider2D>();

        Circlecollider.radius = detectionRange;


    }


    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & detectionLayer) != 0)
        {
            RaycastHit2D isWall = Physics2D.Linecast(transform.position, collision.transform.position, wallLayer);
            if (!isWall)
            {
                if (collision.gameObject.TryGetComponent(out Nodes node))
                {
                    SurroundingNodes.Add(node);
                    node.SurroundingNodes.Add(this);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & detectionLayer) != 0)
        {
          
            if (collision.gameObject.TryGetComponent(out Nodes node))
            {
                SurroundingNodes.Remove(node);
                node.SurroundingNodes.Remove(this);
            }
            
        }
    }

}
