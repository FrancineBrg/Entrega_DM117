using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FimTileComportamento : MonoBehaviour {
    [Tooltip("Tempo esperado antes de destruir o TileBasico")]
    public float tempoDestruir = 2.0f;

    private static int numSpawnLevel = 15;
    private static int numSpawn = 0;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        //Verifica se a bola passou pelo fim do TileBasico
        if (other.GetComponent<JogadorComportamento>()) {
            if (numSpawn <= numSpawnLevel) {
                numSpawn++;
                //Como foi a bola, vamos criar um TileBasico no proximo ponto
                //Mas esse ponto esta depois do ultimo TileBasico presente na cena
                GameObject.FindObjectOfType<ControladorJogo>().SpawnProxTile();

                //Agora destroi esse TileBasico
                Destroy(transform.parent.gameObject, tempoDestruir);
            } else {
                numSpawn = 0;
                if (SceneManager.GetActiveScene().name.Equals("Level1")) {
                    SceneManager.LoadScene("TelaVitoriaLevel1");
                } else if (SceneManager.GetActiveScene().name.Equals("Level2")) {
                    SceneManager.LoadScene("TelaVitoriaLevel2");
                }
            }
        }
    }
}
