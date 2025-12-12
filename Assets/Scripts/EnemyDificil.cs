using UnityEngine;
using System.Collections.Generic;

public class EnemyDificil : EnemyBase
{
    public static List<EnemyDificil> redSquad = new List<EnemyDificil>();

    public enum HardState { Patrulha, Ataque, Defesa, Investigacao }
    public HardState CurrentState = HardState.Patrulha;

    [Header("--- CONFIGURAÇÃO HARD ---")]
    [Tooltip("Arrasta aqui TODAS as saídas")]
    public Transform[] exitPoints;

    public Transform MyTargetExit = null;

    private float myTimeAtPost = 0f;
    private float defenseDuration = 15.0f; 

    private float searchTimer = 0f;

    void Awake() { redSquad.Add(this); }
    void OnDestroy() { redSquad.Remove(this); }

    protected override void RunAI(bool canSeePlayer)
    {

        if (canSeePlayer)
        {
         
            EnemyBase.GlobalAlarm = true;
            EnemyBase.globalLastPlayerPos = lastPlayerPosition;

 
            if (CurrentState != HardState.Ataque) ChangeState(HardState.Ataque);

            navAgent.SetDestination(EnemyBase.globalLastPlayerPos);
            sightAreaRenderer.material = sightAreaMaterials[1];
            return;
        }

    
        switch (CurrentState)
        {
            case HardState.Patrulha:
                if (EnemyBase.GlobalAlarm)
                {
                
                    UpdateRole();
                }
                else
                {
                    sightAreaRenderer.material = sightAreaMaterials[0]; 
                    PatrolLogic();
                }
                break;

            case HardState.Ataque:
                UpdateRole();
                break;

            case HardState.Defesa:
                sightAreaRenderer.material = sightAreaMaterials[1]; 

                if (MyTargetExit != null)
                {
                    navAgent.SetDestination(MyTargetExit.position);

                    if (!navAgent.pathPending && navAgent.remainingDistance < 2.0f)
                    {
                    
                        LookAt180(EnemyBase.globalLastPlayerPos);

                  
                        myTimeAtPost += Time.deltaTime;

                        if (myTimeAtPost > defenseDuration)
                        {
                            ChangeState(HardState.Investigacao);
                        }
                    }
                    else
                    {
                 
                    }
                }
                else
                {
  
                    ChangeState(HardState.Investigacao);
                }
                break;

            case HardState.Investigacao:
                sightAreaRenderer.material = sightAreaMaterials[2]; 
                navAgent.SetDestination(EnemyBase.globalLastPlayerPos);

                if (!navAgent.pathPending && navAgent.remainingDistance < 1.5f)
                {
                    transform.Rotate(0, 60 * Time.deltaTime, 0);

                    searchTimer += Time.deltaTime;
                    if (searchTimer > 5.0f)
                    {
                        EnemyBase.GlobalAlarm = false;
                        ChangeState(HardState.Patrulha);
                        ReturnToStart();
                    }
                }
                break;
        }
    }

    void ChangeState(HardState newState)
    {
        CurrentState = newState;
     
        myTimeAtPost = 0f;
        searchTimer = 0f;
    }

    void UpdateRole()
    {
        if (MyTargetExit != null)
        {
            ChangeState(HardState.Defesa);
        }
        else
        {
            ChangeState(HardState.Investigacao);
        }
    }


    public static void RecalculateSquadTactics()
    {
   
        foreach (var enemy in redSquad)
        {
            enemy.MyTargetExit = null;
            enemy.myTimeAtPost = 0f; 
        }

        if (redSquad.Count == 0) return;
        Transform[] allExits = redSquad[0].exitPoints;

     
        List<EnemyDificil> defenders = new List<EnemyDificil>();
  
        foreach (var enemy in redSquad) defenders.Add(enemy);

  
        foreach (Transform exit in allExits)
        {
            if (exit == null) continue;

            EnemyDificil bestCandidate = null;
            float minDistance = Mathf.Infinity;

            foreach (EnemyDificil enemy in defenders)
            {
  
                if (enemy.MyTargetExit != null) continue;

                float d = Vector3.Distance(enemy.transform.position, exit.position);
                if (d < minDistance)
                {
                    minDistance = d;
                    bestCandidate = enemy;
                }
            }

            if (bestCandidate != null)
            {
                bestCandidate.MyTargetExit = exit;
            
                if (bestCandidate.CurrentState != HardState.Ataque)
                {
                    bestCandidate.UpdateRole();
                }
            }
        }

  
        foreach (var enemy in defenders)
        {
            if (enemy.MyTargetExit == null && enemy.CurrentState != HardState.Ataque)
            {
                enemy.UpdateRole();
            }
        }
    }

    void LookAt180(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.y = 0;
        if (dir != Vector3.zero)
        {
            float angle = Mathf.Sin(Time.time * 2.0f) * 60.0f;
            Quaternion baseRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Euler(0, baseRot.eulerAngles.y + angle, 0);
        }
    }
}