using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] private float fire_rate = 2f;

    private Weapon weapon;

    private float deltaTimeFire = 0;
    public bool isDead = false;

    void Awake()
    {
        weapon = GetComponent<Weapon>();
    }

    void Start()
    {
        //StartCoroutine(Shooting());
    }

    void Update()
    {
        if (isDead) return;

        deltaTimeFire += Time.deltaTime;

        if (deltaTimeFire > fire_rate)
        {
            weapon.Shoot();
            deltaTimeFire = 0;
        }
    }

    public override void TakeDamage()
    {
        base.TakeDamage();

        if(hearts == 0)
        {
            Destroy(gameObject);
        }
    }

}
