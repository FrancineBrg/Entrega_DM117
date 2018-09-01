using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleCamera : MonoBehaviour {
    [Tooltip("O alvo a ser acompanhado pela camera")]
    public Transform alvo;

    [Tooltip("Offset da camera em relacao ao alvo")]
    public Vector3 offset = new Vector3(0,3,-6);


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (alvo != null) {
            //Altera posicao da camera
            transform.position = alvo.position + offset;
            //Altera rotacao da camera em relacao
            transform.LookAt(alvo);
        }
	}
}
