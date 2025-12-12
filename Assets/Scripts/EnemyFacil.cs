using UnityEngine;

public class EnemyFacil : EnemyBase
{

    private bool isChasing = false;

    protected override void RunAI(bool canSeePlayer)
    {

        sightAreaRenderer.material = sightAreaMaterials[canSeePlayer ? 1 : 0];

        if (canSeePlayer)
        {
            isChasing = true;
            navAgent.SetDestination(lastPlayerPosition);
        }
        else
        {
            if (isChasing)
            {
                isChasing = false;
                ReturnToStart();
            }

            PatrolLogic();
        }
    }
}