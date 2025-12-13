# ProjectAI

#### Stealth Game em Unity com apliçação de 3 métodos principais de Inteligência Artificial

##### Engenharia e Desenvolvimento de Jogos Digitais - Inteligência Artificial Aplicada a Jogos

Este projeto é um jogo stealth em que o player é um ladrão a tentar roubar os pertences de um armazém, no entanto, deves ter cuidado com os guardas a patrulhar o edifício. Assim que eles te virem, eles tentarão te apanhar. Se te apanharem perdes. Consegues roubar tudo?

# __Realizado Por:__

| Nome            | Número  |
|----------------|--------|
| **Gabriel Rosas** | 27943  |
| **Miguel Freitas** | 29562  |

# __Indíce__
1. [__Introdução__](#Introdução)
2. [__Estrutura do Projeto__](#Estrutura)
3. [__Controlos__](#Controlos)
4. [__Métodos de IA__](#Métodos)
   * [__FSM__](#FSM)
   * [__BT__](#BT)
   * [__MiniMax__](#MiniMax)
   * [__PathFinding__](#PathFinding)
5. [__Código__](#Código)
9. [__Conclusão__](#Conclusão)

<a name="Introdução"></a>
# __Introdução__

Este trabalho tem como objetivo principal o desenvolvimento de um jogo no motor de jogo Unity, com foco na criação de agentes inteligentes autónomos capazes de navegar e tomar decisões em tempo real. A arquitetura do sistema baseia-se na integração de diferentes paradigmas de Inteligência Artificial para controlar o comportamento dos NPCs (Non-Player Characters). Para a movimentação no ambiente, recorreu-se ao sistema nativo de NavMesh do Unity, garantindo um pathfinding eficiente. No que toca à lógica de decisão e controlo de estados, foram implementadas três métodos diferentes sendo eles, Finite State Machine (FSM) e Behavior Tree (BT), complementadas pela utilização do algoritmo Minimax para o planeamento estratégico e antecipação de jogadas.


<a name="Estrutura"></a>
# __Estrutura do Projeto__

```
ProjectAI/
├── Assets/
│   ├── Scripts/
│   │   ├── EnemyBase.cs          
│   │   ├── EnemyDificil.cs      
│   │   ├── EnemyFacil.cs         
│   │   ├── EnemyMedio.cs          
│   │   ├── GameController.cs                
│   │   └── Player.cs        
│   ├── Scenes/                    
│   ├── Prefabs/                   
│   ├── Models/                   
│   ├── Materials/                 
│   └── Audio/                     
└── ProjectSettings/
```

<a name="Métodos"></a>
# __Métodos de IA__

<a name="FSM"></a>
## __FSM:__

A Finite State Machine (FSM) é um modelo matemático utilizado para representar diversos comportamentos e as respetivas transições entre eles num programa.

  - **`Funcionamento`**: O sistema é composto fundamentalmente por estados e transições. A característica principal de uma FSM é que o sistema (neste caso, o NPC) apenas pode estar num único estado num determinado momento no tempo.
  - **`Transições`**: Quando a máquina se encontra num estado, ela aguarda que condições específicas sejam atingidas para transitar para outro estado. A saída ou comportamento do NPC depende, portanto, do estado das entradas naquele momento específico.
  - **`Aplicação`**: São muito utilizadas por serem intuitivas, flexíveis e exigirem baixo processamento. No entanto, podem tornar-se difíceis de gerir em esquemas muito complexos.

<a name="BT"></a>
## __BT:__

A Behavior Tree representa uma evolução na arquitetura de IA, fornecendo aos NPCs a capacidade de selecionar e executar comportamentos através de uma estrutura hierárquica de nós.
- **`Funcionamento`**: Ao contrário da FSM, a BT funciona como uma árvore onde as folhas contêm os comandos reais (Ações) que controlam a entidade, enquanto os ramos são nós utilitários que controlam o fluxo de decisão.
- **`Tipo de Tarefas`**: Existem essencialmente dois tipos de tarefas

  - **`Condições`**: Verificam o que ocorre na cena (ex: "verificar se o player está no campo de visão") sem realizar mudanças.
  - **`Ações`**: Realizam mudanças efetivas no sistema (ex: "atacar", "patrulhar").
- **`Controlo de Fluxo`**:A navegação na árvore é gerida por nós compostos, como as Sequências (que executam subtarefas em ordem até uma falhar) e os Seletores (que executam subtarefas até uma obter sucesso).

<a name="MiniMax"></a>
## __MiniMax:__

O Minimax é um algoritmo de decisão estratégica utilizado para minimizar a perda possível num cenário de "pior caso" (perda máxima).
- **`Funcionamento`**: O algoritmo identifica qual o caminho a seguir para vencer, assumindo sempre que o oponente tomará a decisão mais desfavorável para nós. É tipicamente aplicável em jogos de soma zero (não cooperativos), onde a vitória de um implica a derrota do outro.
- **`Árvore de Decisão`**: O algoritmo expande uma árvore de estados onde:

  - **`Max`**: Os níveis MAX representam a vez do agente (que tenta maximizar a sua utilidade).
  - **`Min`**: Os níveis MIN representam a vez do adversário (que tenta minimizar a utilidade do agente).
- **`Otimização`**: Como o Minimax gera um espaço de procura exponencial, utiliza-se frequentemente a Poda Alfa-Beta. Esta técnica permite ignorar grandes partes da árvore que não influenciam a decisão final, tornando o processo mais eficiente sem perder precisão.
- **`Heurísticas`**: Em situações onde não é possível calcular até ao fim do jogo (decisões imperfeitas ou tempo limitado), a função de utilidade final é substituída por uma Função de Avaliação (Heurística), que estima a probabilidade de vitória a partir de um determinado estado.

<a name="PathFinding"></a>
## __PathFinding:__

Pathfinding é o processo algorítmico utilizado para determinar a rota mais eficiente entre um ponto de origem e um ponto de destino num ambiente virtual, contornando obstáculos.O seu funcionamento baseia-se em três princípios fundamentais.
- **`Representação em Grafo`**: O espaço navegável (como uma NavMesh) é interpretado pelo computador como um grafo matemático composto por nós (locais) e arestas (ligações entre locais).
- **`Cálculo de Custos (Algoritmo A)`**: O algoritmo mais comum, o A (A-Star), avalia os nós vizinhos calculando o menor custo total ($F$) através da fórmula $F = G + H$, onde:
  
  - **`G`**: Distância já percorrida desde o início.
  - **`H (Heurística)`**: Estimativa da distância restante até ao destino.
- **`Exploração Otimizada`**: Em vez de testar todos os caminhos possíveis (o que seria lento), o algoritmo prioriza explorar os nós com o menor custo $F$, encontrando o caminho mais curto de forma rápida e eficiente.

<a name="Código"></a>
# __Código__

## __EnemyBase.cs__

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    // --- DADOS GLOBAIS ---
    public static bool GlobalAlarm = false;
    public static Vector3 globalLastPlayerPos;

    // --- CONFIGURAÇÕES ---
    public enum IdleType { Estatico, Vigia }
    [Header("--- COMPORTAMENTO ---")]
    public IdleType tipoDeEspera = IdleType.Estatico;
    public float velocidadeRotacaoIdle = 2.0f;
    public float anguloVigia = 25f;

    [Header("--- VISÃO ---")]
    [Range(2, 250)] public int amountOfRays = 250;
    [Range(10f, 70f)] public float angleSpread = 20f;
    [Range(5, 30)] public int raycastDistance = 10;
    public MeshFilter enemyView;
    public Renderer sightAreaRenderer;
    public Material[] sightAreaMaterials;

    protected NavMeshAgent navAgent;
    protected Vector3 startPos;
    protected Quaternion startRotation;

    [Header("--- PATRULHA ---")]
    public GameObject[] waypointList;
    protected int waypointNow = 0;

    private Mesh mesh;
    private Vector3[] raysPoints;
    private int[] triangles;
    private Vector3 raycastOffset = new Vector3(0f, -0.5f, 0f);
    protected Vector3 lastPlayerPosition;

    private bool wasSeeingPlayer = false;

    protected virtual void Start()
    {
        mesh = new Mesh();
        enemyView.mesh = mesh;
        startPos = transform.position;
        startRotation = transform.rotation;
        navAgent = GetComponent<NavMeshAgent>();

        if (waypointList.Length > 0)
            navAgent.SetDestination(waypointList[waypointNow].transform.position);
    }

    protected virtual void FixedUpdate()
    {
        bool canSee = HandleVision();

        // ---  MUDANÇA DE VISÃO ---
        if (canSee != wasSeeingPlayer)
        {
       
            EnemyDificil.RecalculateSquadTactics();
        }

        wasSeeingPlayer = canSee;

        RunAI(canSee);
        UpdateMeshVisuals();
    }

    protected virtual void RunAI(bool canSeePlayer) { }

    protected void PatrolLogic()
    {
        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            if (waypointList.Length > 0)
            {
                waypointNow++;
                if (waypointNow >= waypointList.Length) waypointNow = 0;
                navAgent.SetDestination(waypointList[waypointNow].transform.position);
            }
            else
            {
                ApplyIdleBehavior(startRotation);
            }
        }
    }

    protected void ReturnToStart()
    {
        if (waypointList.Length > 0)
            navAgent.SetDestination(waypointList[waypointNow].transform.position);
        else
            navAgent.SetDestination(startPos);

        if (!navAgent.pathPending && navAgent.remainingDistance <= 0.5f)
            ApplyIdleBehavior(startRotation);
    }

    private void ApplyIdleBehavior(Quaternion baseRotation)
    {
        if (tipoDeEspera == IdleType.Estatico)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, baseRotation, Time.deltaTime * 2.0f);
        }
        else if (tipoDeEspera == IdleType.Vigia)
        {
            float oscilacao = Mathf.Sin(Time.time * velocidadeRotacaoIdle);
            float anguloAtual = oscilacao * anguloVigia;
            Quaternion rotacaoAlvo = baseRotation * Quaternion.Euler(0, anguloAtual, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, Time.deltaTime * 5.0f);
        }
    }

    private bool HandleVision()
    {
        Vector3 raycastPos = transform.position + raycastOffset;
        RaycastHit hit;
        bool isPlayerHit = false;

        raysPoints = new Vector3[amountOfRays + 1];
        triangles = new int[amountOfRays * 3 - 3];
        raysPoints[0] = raycastOffset * 2;
        float degreeBetweenRays = angleSpread / (float)(amountOfRays - 1);

        for (int i = 0; i < amountOfRays; i++)
        {
            Quaternion t_quaternion = Quaternion.Euler(0f, -angleSpread + i * 2f * degreeBetweenRays, 0);
            Vector3 rayDir = t_quaternion * transform.forward;
            int layerObjects = (1 << 8) + (1 << 9);
            int layerWalls = 1 << 8;

            if (Physics.Raycast(raycastPos, rayDir, out hit, raycastDistance, layerObjects))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    lastPlayerPosition = hit.collider.transform.position;
                    globalLastPlayerPos = lastPlayerPosition;
                    GlobalAlarm = true;
                    isPlayerHit = true;

                    if (Physics.Raycast(raycastPos, rayDir, out hit, raycastDistance, layerWalls))
                        raysPoints[i + 1] = transform.InverseTransformPoint(hit.point) + raycastOffset;
                    else
                        raysPoints[i + 1] = t_quaternion * Vector3.forward * raycastDistance + raycastOffset * 2;
                }
                else
                {
                    raysPoints[i + 1] = transform.InverseTransformPoint(hit.point) + raycastOffset;
                }
            }
            else
            {
                raysPoints[i + 1] = t_quaternion * Vector3.forward * raycastDistance + raycastOffset * 2;
            }

            if (i < amountOfRays - 1)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        if (isPlayerHit) FindObjectOfType<AudioManager>()?.Play("Hmm");
        return isPlayerHit;
    }

    private void UpdateMeshVisuals()
    {
        mesh.Clear();
        mesh.vertices = raysPoints;
        mesh.triangles = triangles;
    }
}

```
Este script funciona como o "cérebro" dos inimigos. Ele não toma decisões complexas, mas fornece os dados necessários para os outros scripts.

- **`Visão (Raycasting)`**: A IA utiliza um sistema de "Raycasting" (lançamento de raios invisíveis) em forma de leque para detetar o jogador.
  - Se um raio atingir o jogador sem bater numa parede antes, a variável canSeePlayer torna-se verdadeira.
  - Isto atualiza a posição global do jogador (globalLastPlayerPos) e ativa o alarme global (GlobalAlarm).
  - Sempre que a visão muda (o inimigo passa a ver ou deixa de ver o jogador), ele força o esquadrão de inimigos difíceis a recalcular a sua tática (EnemyDificil.RecalculateSquadTactics()).

- **`Movimento`**: Gere a navegação básica entre pontos de patrulha (PatrolLogic) e o retorno à posição inicial (ReturnToStart) usando o NavMeshAgent do Unity.

## __EnemyFácil.cs__

```
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
```
O EnemyFácil implementa uma Máquina de Estados Finitos (FSM) muito rudimentar com apenas dois comportamentos e sem memória a longo prazo.

- **`Comportamento`**

  - **`Patrulha`**: Se não vê o jogador, fica de vigia.
  - **`Perseguição`**: Se vê o jogador, define o destino para a posição atual do jogador (isChasing = true).
  - **`Limitação`**: Assim que perde o jogador de vista, ele desiste imediatamente. A variável isChasing torna-se falsa e ele chama ReturnToStart(). Não existe procura ou investigação; é uma lógica puramente reativa "Vê = Persegue, Não Vê = Volta".

## __EnemyMédio.cs__

```
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
```
O EnemyMédio introduz o conceito de persistência e uma estrutura que simula uma "Behavior Tree" (Árvore de Comportamento) simplificada através de estados.

- **`Estados`**: Patrulha, Perseguição e Investigação.

- **`Lógica de Investigação`**: Ao contrário do Fácil, quando o Médio perde o jogador de vista, ele não desiste. Ele entra no estado Investigating:
   - Desloca-se até à última posição conhecida do jogador (lastPlayerPosition).
   - Ao chegar lá, inicia um temporizador (waitTimer).
   - Durante 2 segundos, ele roda sobre si mesmo (transform.Rotate) para "procurar" visualmente o jogador.
   - Só após esse tempo esgotar sem reencontrar o jogador é que ele desiste e volta à patrulha.

## __EnemyDifícil.cs__

```
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
```
O EnemyDifícil é o mais complexo, utilizando uma IA de Coordenação de Grupo. Ele não age apenas individualmente; age como parte de um esquadrão (redSquad) para cercar o jogador.

- **`Lógica Individual (FSM Avançada)`**: Este inimigo possui estados táticos específicos:
   - Ataque: Perseguição direta (prioridade máxima se tiver visão).
   - Defesa: O inimigo desloca-se para uma saída específica (MyTargetExit) para bloquear a fuga do jogador. Ao chegar, fica de vigia por 15 segundos.
   - Investigação: Se não tiver uma saída atribuída ou se o tempo de defesa acabar, ele vai à última posição do jogador, faz uma busca de 360º durante 5 segundos e só depois volta à patrulha.

- **`Distribuição de Tarefas`**: A função estática RecalculateSquadTactics funciona como um comandante central. Sempre que a situação de visão muda, ela redistribui as tarefas de todos os inimigos difíceis ativos:
  - Análise: Identifica todos os inimigos disponíveis (que não estão ocupados a atacar diretamente) e todas as saídas (exitPoints).
  - (Greedy): Calcula a distância de cada inimigo a cada saída.
  - Atribuição: Os inimigos mais próximos das saídas recebem a ordem de Defesa (bloquear a porta). Os inimigos que "sobram" (não são necessários para bloquear portas) recebem a ordem de Investigação (caçar o jogador ativamente).

<a name="Controlos"></a>
# __Controlos__

- WSAD para mover
- E para interagir com objetos
- R para recomeçar (apenas após falhar ou succeder)



