using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuPrincipal : MonoBehaviour {

    public void CarregaScene(string nomeScene) {
        SceneManager.LoadScene(nomeScene);

#if UNITY_ADS
        if (UnityAdControle.showAds) {
            //Mostra um anuncio
            UnityAdControle.showAdd();
        }
#endif
    }
}
