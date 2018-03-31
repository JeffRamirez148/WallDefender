using UnityEngine;
using System.Collections;
using TMPro;

public class WallBehaviour : MonoBehaviour
{
    public int health = 100;
    public TextMeshPro heathMesh;
    public bool oob = false; 

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (heathMesh)
        {
            if (health > 0)
            {
                heathMesh.text = health.ToString();
            }
            else
            {
                heathMesh.text = "Game Over!!!";
            }
        }
    }
}
