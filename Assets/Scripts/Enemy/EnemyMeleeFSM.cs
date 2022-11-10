using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMeleeFSM : EnemyBase
{
    //public RoomContion condition;
    public enum State
    {
        Idle,
        Move,
        Attack,
    };

    public State currentState = State.Idle;

    WaitForSeconds Delay500 = new WaitForSeconds(0.5f);
    WaitForSeconds Delay250 = new WaitForSeconds(0.25f);

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //condition = GameObject.FindObjectOfType<RoomContion>();
        StartCoroutine(FSM());
    }

    protected virtual void InitMonster() { }

    protected virtual void AtkRffect() { }

    protected virtual IEnumerator FSM()
    {
        yield return null;

        //while (!condition.playerInThisRoom)
        while (!parentRoom.GetComponent<RoomContion>().playerInThisRoom)
        {
            yield return Delay500;
        }
        InitMonster();

        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }

    protected virtual IEnumerator Idle()
    {
        yield return null;
        if (!Anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Debug.Log(Anim.GetCurrentAnimatorStateInfo(0));
            Anim.SetTrigger("Idle");
        }

        if (CanAtkStateFun())
        {
            if (canAtk)
            {
                currentState = State.Attack;
            }
            else
            {
                currentState = State.Idle;
                transform.LookAt(Player.transform.position);
            }
        }
        else
        {
            currentState = State.Move;
        }
    }

    protected virtual IEnumerator Attack()
    {
        yield return null;

        agent.stoppingDistance = 0f;
        agent.isStopped = true;
        agent.SetDestination(Player.transform.position);
        yield return Delay500;

        agent.isStopped = false;
        agent.speed = 30f;
        canAtk = false;

        if(!Anim.GetCurrentAnimatorStateInfo(0).IsName("stun"))
        {
            Anim.SetTrigger("Attack");
        }
        AtkRffect();
        yield return Delay500;

        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange;
        currentState = State.Idle;
    }

    protected virtual IEnumerator Move()
    {
        yield return null;
        if(!Anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
        {
            Anim.SetTrigger("Walk");
        }
        if(CanAtkStateFun()&&canAtk)
        {
            currentState=State.Attack;
        }
        else if(distance > playerRealizeRange)
        {
            agent.SetDestination(transform.parent.position - Vector3.forward * 5f);
            //Debug.Log($"{distance}, {transform.parent.position}");
        }
        else
        {
            agent.SetDestination(Player.transform.position);
        }
    }
}
