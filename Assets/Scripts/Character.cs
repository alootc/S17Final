using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int hearts;

    public virtual void TakeDamage()
    {
        hearts = Mathf.Clamp(hearts = 1,0, hearts);
    }
}
