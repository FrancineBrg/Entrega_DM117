using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPauseComp : MonoBehaviour {
    public static bool pausado;

    [Tooltip("Referencia para o GO. Usado para ligar/desligar")]
    [SerializeField]
    private GameObject menuPause;

    // Use this for initialization
    void Start () {
#if !UNITY_ADS
        SetPauseMenu(false);
#endif
    }

    /// <summary>
    /// Metodo para reiniciar a Scene, reiniciando o jogo
    /// </summary>
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SetPauseMenu(false);
    }

    ///Metodo para pausar o jogo
    ///param = isPausado
    public void SetPauseMenu(bool isPausado) {
        pausado = isPausado;
        //Se o jogo estiver pausado, timescale recebe 0
        Time.timeScale = (pausado) ? 0 : 1;
        //Habilita/desabilita o menu pause
        menuPause.SetActive(pausado);
    }

    ///Metodo para carregar uma scene
    public void CarregaScene(string nomeScene) {
        SceneManager.LoadScene(nomeScene);
    }
}
