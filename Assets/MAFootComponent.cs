using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAFootComponent : MonoBehaviour {
    // Start is called before the first frame update
    [HideInInspector]
    public bool isGrounded;

    public Rigidbody mainRB;


    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnCollisionStay(Collision collision) {
        if (this.mainRB.velocity.y <= 0) {
            this.isGrounded = true;
        }

        this.isGrounded = true;
    }
}
