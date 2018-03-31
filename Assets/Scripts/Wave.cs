using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class Wave : MonoBehaviour
{
    public Collider2D coll;

    public void StartSwipe(float duration)
    {
        this.gameObject.SetActive(true);
        coll.enabled = true;
        Vector3 goToPos = this.gameObject.transform.localPosition;
        goToPos.y = 5.19f;
        LTDescr tween = LeanTween.move(gameObject, goToPos, duration);
        tween.setOnComplete(Destroy);
        tween.setEaseLinear();
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

}

