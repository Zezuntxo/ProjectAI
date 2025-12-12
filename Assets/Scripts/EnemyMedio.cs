using UnityEngine;


public class EnemyMedio : EnemyBase
{

    private enum State { Patrolling, Chasing, Investigating }
    private State currentState = State.Patrolling;

    private float waitTimer = 0f;
    private bool arrivedAtInvestigationPoint = false;

    protected override void RunAI(bool canSeePlayer)
    {

        if (canSeePlayer)
        {
            currentState = State.Chasing;
            sightAreaRenderer.material = sightAreaMaterials[1]; 
            navAgent.SetDestination(lastPlayerPosition);
            arrivedAtInvestigationPoint = false;
            return;
        }

        if (currentState == State.Chasing || currentState == State.Investigating)
        {
            currentState = State.Investigating;
            sightAreaRenderer.material = sightAreaMaterials[2]; 

      
            if (!arrivedAtInvestigationPoint)
            {
                navAgent.SetDestination(lastPlayerPosition);

                if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    arrivedAtInvestigationPoint = true;
                    waitTimer = 0f;
                }
            }
       
            else
            {
                waitTimer += Time.deltaTime;
                transform.Rotate(0, 100 * Time.deltaTime, 0); 

                if (waitTimer > 2.0f)
                {
                   
                    currentState = State.Patrolling;
                    sightAreaRenderer.material = sightAreaMaterials[0]; 
                    ReturnToStart();
                }
            }
            return;
        }

     
        PatrolLogic();
    }
}