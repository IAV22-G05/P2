/*    
   Copyright (C) 2020 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
namespace UCM.IAV.Movimiento
{

    using UnityEngine;

    /// <summary>
    /// Clara para el comportamiento de agente que consiste en ser el jugador
    /// </summary>
    public class ControlJugador: ComportamientoAgente
    {
        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>


        Rigidbody rb;
        MovimientoAutomatico mov;
        Animator anim;
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            mov = GetComponent<MovimientoAutomatico>();
            anim = GetComponent<Animator>();
        }
        public override Direccion GetDireccion()
        {

            //Movimiento
            //Vector3 direccion = new Vector3();
            //direccion.x = Input.GetAxis("Horizontal");
            //direccion.z = Input.GetAxis("Vertical");
            //direccion.Normalize();
            //direccion *= 10;
            //rb.velocity = direccion;

            Direccion direccion = new Direccion();
            direccion.lineal.x = Input.GetAxis("Horizontal");
            direccion.lineal.z = Input.GetAxis("Vertical");
            direccion.lineal.Normalize();
            direccion.lineal *= agente.aceleracionMax;

            //Animacion
            if (direccion.lineal.magnitude > 0)
                anim.SetBool("walk", true);
            else
                anim.SetBool("walk", false);


            //Orientacion
            //direccion.y = 0;
            agente.transform.rotation = Quaternion.LookRotation(direccion.lineal, Vector3.up);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                mov.enabled = true;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                mov.enabled = false;
            }


            return direccion;
        }
    }
}