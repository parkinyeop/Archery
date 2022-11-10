using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Slider hpSlider;
    public Slider backHpSlider;
    public bool backHphit = false;

    public Transform enemy;
    public float maxHp = 1000f;
    public float currentHp = 1000f;
    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.parent.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = enemy.position;
        hpSlider.value = Mathf.Lerp(hpSlider.value,currentHp/maxHp,Time.deltaTime*5f);

        if(backHphit)
        {
            backHpSlider.value = Mathf.Lerp(backHpSlider.value, currentHp / maxHp, Time.deltaTime * 10f);
            if(hpSlider.value >= backHpSlider.value -0.01f)
            {
                backHphit = false;
                backHpSlider.value = hpSlider.value;
            }
        }
    }

    public void Dmg()
    {
        //currentHp -= 300f;
        Invoke("BackHP", 0.5f);
    }

    void BackHP()
    {
        backHphit = true;
    }
}
