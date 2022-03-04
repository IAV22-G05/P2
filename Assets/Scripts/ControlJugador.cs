using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlJugador : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody rb;
    MovimientoAutomatico mov;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mov = GetComponent<MovimientoAutomatico>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direccion = new Vector3();
        direccion.x = Input.GetAxis("Horizontal");
        direccion.z = Input.GetAxis("Vertical");
        direccion.Normalize();
        direccion *= 10;
        //rb.AddForce(direccion);
        rb.velocity = direccion;


        //Orientacion
        //direccion.y = 0;
        transform.rotation = Quaternion.LookRotation(direccion, Vector3.up);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            mov.enabled = true;
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            mov.enabled = false;
        }
    }
}
