using UnityEngine;

// Herda de EnemyBase
public class EnemyFacil : EnemyBase
{

    private bool isChasing = false;

    // Apenas implementa a lógica simples (FSM)
    protected override void RunAI(bool canSeePlayer)
    {

        // Visual Debug (Muda cor do cone)
        sightAreaRenderer.material = sightAreaMaterials[canSeePlayer ? 1 : 0];

        if (canSeePlayer)
        {
            // ESTADO: PERSEGUIR
            isChasing = true;
            navAgent.SetDestination(lastPlayerPosition);
        }
        else
        {
            // Se perdeu a visão, volta imediatamente à patrulha
            if (isChasing)
            {
                isChasing = false;
                ReturnToStart();
            }

            // Lógica normal de andar nos waypoints
            PatrolLogic();
        }
    }
}