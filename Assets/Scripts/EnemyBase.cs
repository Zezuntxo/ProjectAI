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

        // --- GATILHO: MUDANÇA DE VISÃO ---
        if (canSee != wasSeeingPlayer)
        {
            // Se a situação mudou (Vi -> Não Vi, ou Não Vi -> Vi)
            // Obriga os vermelhos a resetarem os seus timers e posições
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