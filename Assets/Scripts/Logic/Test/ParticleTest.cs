using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetComponent<ParticleSystem>().ActiveEmit(true);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            GetComponent<ParticleSystem>().ActiveEmit(false);
        }
    }
}
