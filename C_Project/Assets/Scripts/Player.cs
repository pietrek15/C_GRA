using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Controller2D))]   // teraz z posiomu Unity nie da sie usunac tego komponentu bo jest wymagany 
public class Player : MonoBehaviour {

    public float jumpHeight = 4;            // wysokosc skoku
    public float timeToJumpApex = .4f;      // czas w sek do osiągniecia szczytu
    float accelerationTimeAirbone = .2f;    // czas do osiagnieca max predkosci w locie
    float accelerationTimeGrounded = .1f;   // czas do osiagnieca max predkosci na ziemi
    public float moveSpeed = 10;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    Controller2D controller;

	//---------------------------------------------------------------------//
	void Start () {                                                 //dzieje sie to TYLKO RAZ
        controller = GetComponent<Controller2D>();

        gravity =- (2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);         // wyliczanie grawitacji
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity" + gravity + " Jump Velocity " + jumpVelocity);          // w konsoli printuje te wartosci
	}
	
	
    void Update()                                                               // dzieje sie co klatke
    {
        if (controller.collisions.above || controller.collisions.below)         // skutek vertykalnej kolizji
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //-------------------------------------------------------------------------------------------//
        if(Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)                                      // !!!WAZNE wprzycisk odpowiedzialny za SKOK!!!
        {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;                                                                                                                            // powolne rozpoczecie ruchu
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirbone); 
        velocity.y += gravity * Time.deltaTime;                                                                                                                                 // sposob dzialania grawitacji
        controller.Move(velocity * Time.deltaTime);
    }

}
