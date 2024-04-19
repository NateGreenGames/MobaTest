using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawnBehavior : MonoBehaviour
{
    public float movementSpeed = 1;
    private Rigidbody m_rb;
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 tranformedVector = input.z * transform.right + -input.x * transform.forward;
        if (input != Vector3.zero) m_rb.AddForce(tranformedVector * movementSpeed);
    }
}
