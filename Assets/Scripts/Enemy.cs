using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int tapDamage = 1;
    public int maxNumOfHits = 3;
    public List<Color> hitColors = new List<Color>();

    [HideInInspector]
    public int numOfHits;

    abstract public void TakeDamage(float multiplier);
    abstract public void Death();
}
