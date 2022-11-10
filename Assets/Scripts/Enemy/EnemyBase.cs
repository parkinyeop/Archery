using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    public float maxHp = 1000f;
    public float currentHp = 1000f;

    public float damage = 100f;

    protected float playerRealizeRange = 10f;
    protected float attackRange = 5f;
    protected float attackCoolTime = 5f;
    protected float attackCoolTimeCacl = 5f;
    protected bool canAtk = true;

    protected float moveSpeed = 2f;

    protected GameObject Player;
    protected NavMeshAgent agent;
    protected float distance;

    protected GameObject parentRoom;


    protected Animator Anim;
    protected Rigidbody rb;

    public LayerMask layerMask;
    protected Vector3 targetDir;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        Anim = GetComponent<Animator>();

        parentRoom = transform.parent.transform.parent.gameObject;

        StartCoroutine(CalcCoolTime());
    }

    protected bool CanAtkStateFun()
    {
        targetDir = new Vector3(Player.transform.position.x - transform.position.x, 0f, Player.transform.position.z - transform.position.z);

        //RaycastHit hit;
        //Physics.Raycast(new Vector3(transform.position.x, 0.5f, transform.position.z), targetDir, out hit, 30f, layerMask);
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 10f,
               transform.forward, 30f, LayerMask.GetMask("Player"));
        distance = Vector3.Distance(Player.transform.position, transform.position);
        
        if (rayHits == null)
        {
            Debug.Log("hit.transform == null");
            return false;
        }
        if (rayHits.Length > 0 && distance <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    IEnumerator CalcCoolTime()
    {
        while (true)
        {
            yield return null;

            if (!canAtk)
            {
                attackCoolTimeCacl -= Time.deltaTime;
                if (attackCoolTimeCacl <= 0)
                {
                    attackCoolTimeCacl = attackCoolTime;
                    canAtk = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
