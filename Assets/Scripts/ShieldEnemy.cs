using UnityEngine;
using System.Collections;

public class ShieldEnemy : DefaultEnemy
{
    public Collider2D shield;
    public float delayOnShieldSpawn = 3.0f;
    protected float shieldSpawnTimer = 0.0f;
    protected bool spawnedShield = false;

    // Update is called once per frame
    protected override void Update()
    {
        if (!spawnedShield)
        {
            shieldSpawnTimer += Time.deltaTime;
            if (shieldSpawnTimer >= delayOnShieldSpawn)
            {
                spawnedShield = true;
                shield.enabled = true;
                shield.gameObject.SetActive(true);
            }
        }
        base.Update();
    }

}
