using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDomain", menuName = "Scriptable Objects/EnemyDomain")]
public class EnemyDomain : ScriptableObject
{
    bool canCreatePath = false;

    public void allowPathCreation()
    {
        canCreatePath = true;

    }

    public void blockPathCreation()
    {
        canCreatePath = false;

    }

    public bool returnCanCreatePath()
    {

        return canCreatePath;
    }
}
