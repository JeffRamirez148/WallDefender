using UnityEngine;
using System.Collections;
using System;

public class SwipeSkill : BaseSkills
{
    public float waveMoveTime;
    public GameObject waveToClone;
    protected bool canSpawn = true;

    public void Start()
    {
    }

    public override void CheckDurationTimer()
    {
        if (!canSpawn)
        {
            timer += Time.deltaTime;
            if (timer > duration)
            {
                timer = 0.0f;
                canSpawn = true;
            }
        }
    }

    public override void ActivateSkill(Vector3 position)
    {
        if (canSpawn)
        {
            //clone swipe thing
            GameObject wave = GameObject.Instantiate(waveToClone, new Vector3(position.x, waveToClone.transform.position.y, waveToClone.transform.position.z), Quaternion.identity);
            wave.GetComponent<Wave>().StartSwipe(waveMoveTime);
            canSpawn = false;
        }
    }
}
