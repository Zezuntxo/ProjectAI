using UnityEngine;

// Herda de EnemyBase
public class EnemyMedio : EnemyBase
{

    private enum State { Patrolling, Chasing, Investigating }
    private State currentState = State.Patrolling;

    // Variáveis exclusivas do Médio
    private float waitTimer = 0f;
    private bool arrivedAtInvestigationPoint = false;

    protected override void RunAI(bool canSeePlayer)
    {

        // 1. Prioridade Máxima: Visão
        if (canSeePlayer)
        {
            currentState = State.Chasing;
            sightAreaRenderer.material = sightAreaMaterials[1]; // Vermelho
            navAgent.SetDestination(lastPlayerPosition);
            arrivedAtInvestigationPoint = false;
            return;
        }

        // 2. Lógica de Investigação (Behavior Tree simulada)
        if (currentState == State.Chasing || currentState == State.Investigating)
        {
            currentState = State.Investigating;
            sightAreaRenderer.material = sightAreaMaterials[2]; // Amarelo

            // Vai até onde viu o player pela última vez
            if (!arrivedAtInvestigationPoint)
            {
                navAgent.SetDestination(lastPlayerPosition);

                if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
                {
                    arrivedAtInvestigationPoint = true;
                    waitTimer = 0f;
                }
            }
            // Já chegou? Agora espera e roda
            else
            {
                waitTimer += Time.deltaTime;
                transform.Rotate(0, 100 * Time.deltaTime, 0); // Gira para procurar

                if (waitTimer > 2.0f)
                {
                    // Desiste e volta à patrulha
                    currentState = State.Patrolling;
                    sightAreaRenderer.material = sightAreaMaterials[0]; // Verde
                    ReturnToStart();
                }
            }
            return;
        }

        // 3. Patrulha Normal
        PatrolLogic();
    }
}