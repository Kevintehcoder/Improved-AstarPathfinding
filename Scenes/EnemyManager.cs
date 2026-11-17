using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] EnemyDomain enemyDomain;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyDomain.allowPathCreation();
        //Debug.Log("Path Creation Allowed");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemyDomain.blockPathCreation();
        //Debug.Log("Path Creation Blocked");
    }
}
