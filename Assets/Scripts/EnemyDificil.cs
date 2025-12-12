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

    // Destino atribuído
    public Transform MyTargetExit = null;

    // --- TEMPOS ---
    // Timer LOCAL: Conta quanto tempo EU estive na porta
    private float myTimeAtPost = 0f;
    private float defenseDuration = 15.0f; // 15 Segundos a contar depois de chegar

    // Timer LOCAL: Conta o 360 da investigação
    private float searchTimer = 0f;

    void Awake() { redSquad.Add(this); }
    void OnDestroy() { redSquad.Remove(this); }

    protected override void RunAI(bool canSeePlayer)
    {
        // 1. VISÃO (ATAQUE IMEDIATO)
        if (canSeePlayer)
        {
            // Atualiza global
            EnemyBase.GlobalAlarm = true;
            EnemyBase.globalLastPlayerPos = lastPlayerPosition;

            // Ataca (Prioridade Máxima)
            // Nota: O Recalculate já foi chamado pelo EnemyBase se a visão mudou
            if (CurrentState != HardState.Ataque) ChangeState(HardState.Ataque);

            navAgent.SetDestination(EnemyBase.globalLastPlayerPos);
            sightAreaRenderer.material = sightAreaMaterials[1]; // Vermelho
            return;
        }

        // 2. MÁQUINA DE ESTADOS
        switch (CurrentState)
        {
            case HardState.Patrulha:
                if (EnemyBase.GlobalAlarm)
                {
                    // Alarme tocou, assume papel atribuído pelo Recalculate
                    UpdateRole();
                }
                else
                {
                    sightAreaRenderer.material = sightAreaMaterials[0]; // Verde
                    PatrolLogic();
                }
                break;

            case HardState.Ataque:
                // Se deixei de ver, vou para o meu posto
                UpdateRole();
                break;

            case HardState.Defesa:
                sightAreaRenderer.material = sightAreaMaterials[1]; // Vermelho

                if (MyTargetExit != null)
                {
                    navAgent.SetDestination(MyTargetExit.position);

                    // --- LÓGICA DO TEMPO (CORREÇÃO) ---
                    // O tempo SÓ conta se já cheguei ao sítio!
                    if (!navAgent.pathPending && navAgent.remainingDistance < 2.0f)
                    {
                        // Vigia (180º)
                        LookAt180(EnemyBase.globalLastPlayerPos);

                        // Incrementa o timer local
                        myTimeAtPost += Time.deltaTime;

                        // Se já fiquei aqui 15s -> Vai Investigar
                        if (myTimeAtPost > defenseDuration)
                        {
                            ChangeState(HardState.Investigacao);
                        }
                    }
                    else
                    {
                        // Se ainda estou a correr para a porta, o timer não avança
                        // Ele tem de garantir 15s de defesa efetiva
                    }
                }
                else
                {
                    // Se não tenho porta (sobra), vai investigar
                    ChangeState(HardState.Investigacao);
                }
                break;

            case HardState.Investigacao:
                sightAreaRenderer.material = sightAreaMaterials[2]; // Amarelo
                navAgent.SetDestination(EnemyBase.globalLastPlayerPos);

                if (!navAgent.pathPending && navAgent.remainingDistance < 1.5f)
                {
                    transform.Rotate(0, 60 * Time.deltaTime, 0); // 360

                    searchTimer += Time.deltaTime;
                    if (searchTimer > 5.0f)
                    {
                        // Fim do Ciclo
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
        // Reset aos timers locais ao mudar de estado
        myTimeAtPost = 0f;
        searchTimer = 0f;
    }

    // Decide se vai para Defesa ou Investigação com base na atribuição atual
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

    // --- CÉREBRO MINMAX GLOBAL (CHAMADO PELO ENEMYBASE) ---
    public static void RecalculateSquadTactics()
    {
        // 1. Reset Total: Limpa as portas de todos e ZERA OS TIMERS
        foreach (var enemy in redSquad)
        {
            enemy.MyTargetExit = null;
            enemy.myTimeAtPost = 0f; // <-- IMPORTANTE: Obriga a ficar +15s se ganhar porta
        }

        if (redSquad.Count == 0) return;
        Transform[] allExits = redSquad[0].exitPoints;

        // 2. Quem está livre? (Quem não vê o player agora)
        List<EnemyDificil> defenders = new List<EnemyDificil>();
        // Usamos uma verificação simples: quem está em Ataque está ocupado
        // Mas como esta função é chamada na transição, assumimos disponibilidade
        foreach (var enemy in redSquad) defenders.Add(enemy);

        // 3. Distribuição Gulosa (Mais perto ganha)
        foreach (Transform exit in allExits)
        {
            if (exit == null) continue;

            EnemyDificil bestCandidate = null;
            float minDistance = Mathf.Infinity;

            foreach (EnemyDificil enemy in defenders)
            {
                // Se já tem porta ou está a ver o player (Ataque), salta
                // Nota: Verificamos a navAgent.destination ou estado para saber se está ocupado a atacar
                if (enemy.MyTargetExit != null) continue;

                // Se o inimigo ESTÁ a ver o player (Attack), ele não deve ser desviado para portas
                // Ele vai continuar a atacar no Update dele
                // Mas aqui marcamo-lo como "sem porta"
                // O estado dele no Update vai sobrepor tudo.

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
                // Força atualização de estado se não estiver a atacar
                if (bestCandidate.CurrentState != HardState.Ataque)
                {
                    bestCandidate.UpdateRole();
                }
            }
        }

        // Quem sobrou sem porta atualiza para Investigação
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