using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe para controlar a parte principal do jogo
/// </summary>
public class ControladorJogo : MonoBehaviour {
    [Tooltip("Referencia para o TileBasico")]
    public Transform tile;

    [Tooltip("Referencia para o Obstaculo")]
    public Transform obstaculo;

    [Tooltip("Ponto para colocar o TileBasicoInicial")]
    public Vector3 pontoInicial = new Vector3(0, 0, -5);

    [Tooltip("Quantidade de tiles iniciais")]
    [Range(1, 20)]
    public int numSpawnIni;

    [Tooltip("Quantidade de tiles sem obstaculos")]
    [Range(1, 4)]
    public int numTileSemOBS = 4;

    /// <summary>
    /// Local para o spawn do próximo tile
    /// </summary>
    private Vector3 proxTilePos;

    /// <summary>
    /// Rotacao do proximo tile
    /// </summary>
    private Quaternion proxTileRot;

	// Use this for initialization
	void Start () {
        //Preparando o ponto inicial
        proxTilePos = pontoInicial;
        proxTileRot = Quaternion.identity;

        for (int i = 0; i < numSpawnIni; i++) {
            print(i);
            SpawnProxTile(i >= numTileSemOBS);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnProxTile(bool spawnObstaculos = true) {
        print("oi");
        var novoTile = Instantiate(tile, proxTilePos, proxTileRot);

        //Detectar qual o local de spawn do prox tile
        var proxTile = novoTile.Find("PontoSpawn");
        proxTilePos = proxTile.position;
        proxTileRot = proxTile.rotation;
        
        //Verifica se ja podemos criar Tiles com obstaculos
        if (!spawnObstaculos) {
            return;
        }

        //Podemos criar obstaculos
        //Primeiro devemos buscar todos os locais possiveis
        var pontosObstaculo = new List<GameObject>();

        //Varrer os GOs filhos buscando os pontos de spawn
        foreach (Transform filho in novoTile) {
            //Vamos verificar se possui a TAG PontoSpawn
            if (filho.CompareTag("ObsSpawn")) {
                //Se for adicionamos na lista como potencial ponto de spawn
                pontosObstaculo.Add(filho.gameObject);
            }
        }

        //Garantir que existe pelo menos um spawn point disponível
        if (pontosObstaculo.Count > 0) {
            //Vamos pegar um ponto aleatorio
            var pontoSpawn = pontosObstaculo[Random.Range(0, pontosObstaculo.Count)];

            //Vamos guardar a posicao desse ponto de spawn
            var obsSpawnPos = pontoSpawn.transform.position;

            //Cria um novo obstaculo
            var novoObs = Instantiate(obstaculo, obsSpawnPos, Quaternion.identity);

            //Faz ele ser filho de TileBasico.PontoSpawn (centro, esq, dir)
            novoObs.SetParent(pontoSpawn.transform);
        }
    }
}
