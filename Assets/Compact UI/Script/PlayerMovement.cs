using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    ConstantForce2D consForce;

    void Start()
    {
        consForce = GetComponentInChildren<ConstantForce2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            player.transform.Translate(new Vector3(-0.1f, 0, 0));
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            player.transform.Translate(new Vector3(0.1f, 0, 0));
        }
    }
}
