using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    static PlayerMovement instance;

    public static PlayerMovement Instance
    {
        get
        {
            if(instance == null)
            {
                instance= FindObjectOfType<PlayerMovement>();

                if(instance == null)
                {
                    var instanceContainer = new GameObject("PlayerMovement");
                    instance = instanceContainer.AddComponent<PlayerMovement>();
                }
            }
            return instance;
        }
    }
    Rigidbody rb;
    public float moveSpeed = 25f;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(moveHorizontal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

        if (JoyStickMovement.Instance.joyVec.x != 0 || JoyStickMovement.Instance.joyVec.y != 0)
        {
            rb.velocity = new Vector3(JoyStickMovement.Instance.joyVec.x, rb.velocity.y, JoyStickMovement.Instance.joyVec.y) * moveSpeed;
            rb.rotation = Quaternion.LookRotation(new Vector3(JoyStickMovement.Instance.joyVec.x, 0, JoyStickMovement.Instance.joyVec.y));
        }
        else
        {
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.transform.CompareTag("NextRoom"))
        {
            StageMgr.Instance.NextStage();
        }

        if(other.transform.CompareTag("HpBooster"))
        {
            PlayerHpBar.Instance.GetHpBoost();
            Destroy(other.gameObject);
        }

        if(other.transform.CompareTag("MeleeAtk"))
        {
            other.transform.parent.GetComponent<EnemyDuck>().meleeAtkArea.SetActive(false);
            PlayerHpBar.Instance.currentHp -= other.transform.parent.GetComponent<EnemyDuck>().damage *2f;

            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit"))
            {
                animator.SetTrigger("GetHit");
                Instantiate(EffectSet.Instance.PlayerDmgEffect, PlayerTargeting.Instance.AttackPoint.position, Quaternion.Euler(90,0,0));
            }
        }
    }
}
