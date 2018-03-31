using UnityEngine;
using System.Collections;

public abstract class BaseSkills : MonoBehaviour
{
    public float duration = 1.0f;
    protected float timer = 0.0f;
    public Collider2D coll;

    public virtual void ActivateSkill(Vector3 position)
    {
        this.transform.localPosition = position;
        coll.enabled = true;
        timer = 0.0f;
        gameObject.SetActive(true);
    }

    public virtual void CheckDurationTimer()
    {
        if (coll.enabled)
        {
            timer += Time.deltaTime;
            if (timer > duration)
            {
                coll.enabled = false;
                timer = 0.0f;
                this.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CheckDurationTimer();
    }
}
