using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerTargeting : MonoBehaviour
{
    static PlayerTargeting instance;
    public static PlayerTargeting Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerTargeting>();

                if (instance == null)
                {
                    var instanceContainer = new GameObject("PlayerTargeting");
                    instance = instanceContainer.AddComponent<PlayerTargeting>();
                }
            }
            return instance;
        }
    }

    public bool getATarget = false;
    float currentDist = 0;
    float closetDist = 100f;
    float TargetDist = 100f;
    int closeDistIndex = 0;
    public int TargetIndex = -1;
    int prevTargetIndex = 0;
    public LayerMask layerMask;

    public float atkSpd = 1f;

    public List<GameObject> MonsterList = new List<GameObject>();

    public GameObject PlayerBolt;
    public Transform AttackPoint;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetTarget();
        AtkTarget();
        //PlayerMovement.Instance.animator.SetBool("Attack", true);
    }

    void Attack()
    {
        PlayerMovement.Instance.animator.SetFloat("AttackSpeed", atkSpd);
        Instantiate(PlayerBolt, AttackPoint.position, transform.rotation);
    }

    void SetTarget()
    {
        if (MonsterList.Count != 0)
        {
            prevTargetIndex = TargetIndex;
            currentDist = 0f;
            closeDistIndex = 0;
            TargetIndex = -1;

            for (int i = 0; i < MonsterList.Count; i++)
            {
                if (MonsterList[i] == null) { return; }
                currentDist = Vector3.Distance(transform.position, MonsterList[i].transform.GetChild(0).position);

                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position,
                    out hit, 20f, layerMask);

                //Debug.Log($"SetTarget : {isHit}");

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    if (TargetDist >= currentDist)
                    {
                        TargetIndex = i;
                        TargetDist = currentDist;
                        if (JoyStickMovement.Instance.isPlayerMoving && prevTargetIndex != TargetIndex)
                        {
                            TargetIndex = prevTargetIndex;
                        }
                    }
                }

                if (closetDist >= currentDist)
                {
                    closeDistIndex = i;
                    closetDist = currentDist;
                }
            }
            if (TargetIndex == -1)
            {
                TargetIndex = closeDistIndex;
            }
            closetDist = 100f;
            TargetDist = 100f;
            getATarget = true;
        }
    }

    void AtkTarget()
    {
        if (TargetIndex == -1 || MonsterList.Count == 0)
        {
            PlayerMovement.Instance.animator.SetBool("Attack", false);
            return;
        }

        if (getATarget && !JoyStickMovement.Instance.isPlayerMoving && MonsterList.Count != 0)
        {
            //Debug.Log("lookat :" + MonsterList[TargetIndex].transform.GetChild(0));
            transform.LookAt(MonsterList[TargetIndex].transform.GetChild(0).position);

            if (PlayerMovement.Instance.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                PlayerMovement.Instance.animator.SetBool("Idle", false);
                PlayerMovement.Instance.animator.SetBool("Walk", false);
                PlayerMovement.Instance.animator.SetBool("Attack", true);
            }
        }

        else if (JoyStickMovement.Instance.isPlayerMoving)
        {
            if (!PlayerMovement.Instance.animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            {
                PlayerMovement.Instance.animator.SetBool("Attack", false);
                PlayerMovement.Instance.animator.SetBool("Idle", false);
                PlayerMovement.Instance.animator.SetBool("Walk", true);
            }
            else
            {
                PlayerMovement.Instance.animator.SetBool("Attack", false);
                PlayerMovement.Instance.animator.SetBool("Idle", true);
                PlayerMovement.Instance.animator.SetBool("Walk", false);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (getATarget)
        {
            for (int i = 0; i < MonsterList.Count; i++)
            {
                if (MonsterList[i] == null)
                {
                    return;
                }
                RaycastHit hit;
                bool isHit = Physics.Raycast(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position,
                    out hit, 20f, layerMask);

                //Debug.Log($"DrawGizmos : {isHit}");

                if (isHit && hit.transform.CompareTag("Monster"))
                {
                    Gizmos.color = Color.black;
                }

                else
                {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawRay(transform.position, MonsterList[i].transform.GetChild(0).position - transform.position);

            }
        }
    }
}
