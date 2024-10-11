using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentController : MonoBehaviour
{
    Rigidbody2D _rb;
    Vector2 _agentInput;
    public float _speed = 60f;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();   
    }

    private void Update()
    {
        _agentInput = new Vector2(Input.GetAxisRaw("Horizontal") * _speed, Input.GetAxisRaw("Vertical") * _speed);
    }
    private void FixedUpdate()
    {
        _rb.AddForce(_agentInput);
    }

}
