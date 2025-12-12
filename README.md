# ProjectAI

#### Stealth Game em Unity com apliçação de 3 métodos principais de Inteligência Artificial

##### Engenharia e Desenvolvimento de Jogos Digitais - Inteligência Artificial Aplicada a Jogos

Game created as a project, where you are the thief trying to steal goods from the warehouse, but you must beware of guards patrolling this building. Once they see you, they will try to catch you. If they get to you, you lose. Can you steal everything?

# __Realizado Por:__

| Nome            | Número  |
|----------------|--------|
| **Gabriel Rosas** | 27943  |
| **Miguel Freitas** | 29562  |

# __Indíce__
1. [__Introdução__](#Introdução)
2. [__Estrutura do Projeto__](#Estrutura)
3. [__Controlos__](#Controlos)
5. [__Métodos de IA__](#Métodos)
6. [__Tecnologias__](#Tecnologias)
7. [__Dificuldades__](#Dificuldades)
8. [__Conclusão__](#Conclusão)

<a name="Introdução"></a>
# __Introdução__

Este trabalho tem como objetivo principal o desenvolvimento de um jogo no motor de jogo Unity, com foco na criação de agentes inteligentes autónomos capazes de navegar e tomar decisões em tempo real. A arquitetura do sistema baseia-se na integração de diferentes paradigmas de Inteligência Artificial para controlar o comportamento dos NPCs (Non-Player Characters). Para a movimentação no ambiente, recorreu-se ao sistema nativo de NavMesh do Unity, garantindo um pathfinding eficiente. No que toca à lógica de decisão e controlo de estados, foram implementadas três métodos diferentes sendo eles, Finite State Machine (FSM) e Behavior Tree (BT), complementadas pela utilização do algoritmo Minimax para o planeamento estratégico e antecipação de jogadas.


<a name="Estrutura"></a>
# __Estrutura do Projeto__

```plaintext
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

## __FSM:__

A Finite State Machine (FSM) é um modelo matemático utilizado para representar diversos comportamentos e as respetivas transições entre eles num programa.

  - **`Funcionamento`**: O sistema é composto fundamentalmente por estados e transições. A característica principal de uma FSM é que o sistema (neste caso, o NPC) apenas pode estar num único estado num determinado momento no tempo.
  - **`Transições`**: Quando a máquina se encontra num estado, ela aguarda que condições específicas sejam atingidas para transitar para outro estado. A saída ou comportamento do NPC depende, portanto, do estado das entradas naquele momento específico.
  - **`Aplicação`**: São muito utilizadas por serem intuitivas, flexíveis e exigirem baixo processamento. No entanto, podem tornar-se difíceis de gerir em esquemas muito complexos.

## __BT:__

A Behavior Tree representa uma evolução na arquitetura de IA, fornecendo aos NPCs a capacidade de selecionar e executar comportamentos através de uma estrutura hierárquica de nós.
- **`Funcionamento`**: Ao contrário da FSM, a BT funciona como uma árvore onde as folhas contêm os comandos reais (Ações) que controlam a entidade, enquanto os ramos são nós utilitários que controlam o fluxo de decisão.
- **`Tipo de Tarefas`**: Existem essencialmente dois tipos de tarefas

  - **`Condições`**: Verificam o que ocorre na cena (ex: "verificar se o player está no campo de visão") sem realizar mudanças.
  - **`Ações`**: Realizam mudanças efetivas no sistema (ex: "atacar", "patrulhar").
- **`Controlo de Fluxo`**:A navegação na árvore é gerida por nós compostos, como as Sequências (que executam subtarefas em ordem até uma falhar) e os Seletores (que executam subtarefas até uma obter sucesso).

## __MiniMax:__

O Minimax é um algoritmo de decisão estratégica utilizado para minimizar a perda possível num cenário de "pior caso" (perda máxima).
- **`Funcionamento`**: O algoritmo identifica qual o caminho a seguir para vencer, assumindo sempre que o oponente tomará a decisão mais desfavorável para nós. É tipicamente aplicável em jogos de soma zero (não cooperativos), onde a vitória de um implica a derrota do outro.
- **`Árvore de Decisão`**: O algoritmo expande uma árvore de estados onde:

  - **`Max`**: Os níveis MAX representam a vez do agente (que tenta maximizar a sua utilidade).
  - **`Min`**: Os níveis MIN representam a vez do adversário (que tenta minimizar a utilidade do agente).
- **`Otimização`**: Como o Minimax gera um espaço de procura exponencial, utiliza-se frequentemente a Poda Alfa-Beta. Esta técnica permite ignorar grandes partes da árvore que não influenciam a decisão final, tornando o processo mais eficiente sem perder precisão.
- **`Heurísticas`**: Em situações onde não é possível calcular até ao fim do jogo (decisões imperfeitas ou tempo limitado), a função de utilidade final é substituída por uma Função de Avaliação (Heurística), que estima a probabilidade de vitória a partir de um determinado estado.

<a name="Controlos"></a>
# __Controlos__

- WSAD para mover
- E para interagir com objetos
- R para recomeçar (apenas após falhar ou succeder)



