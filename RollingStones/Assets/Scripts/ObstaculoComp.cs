using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObstaculoComp : MonoBehaviour {
    /// <summary>
    /// Variavel referencia para o jogador
    /// </summary>
    private GameObject jogador;

    [Tooltip("Quanto tempo antes de reiniciar o jogo")]
    public float tempoEspera = 2.0f;

    [SerializeField]
    [Tooltip("Referencia para a explosao")]
    private GameObject explosao;

    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision) {       
        if (collision.gameObject.GetComponent<JogadorComportamento>()) {
            //Esconde o jogador ao invés de destruir
            collision.gameObject.SetActive(false);
            jogador = collision.gameObject;

            //Chama função de reset depois de um tempo
            Invoke("ResetaJogo", tempoEspera);
        }
    }

    ///Reinicia o level
    void ResetaJogo() {
        //Faz o MenuGameOver aparecer
        var gameOverMenu = GetGameOverMenu();
        gameOverMenu.SetActive(true);

        //Busca os botoes do MenuGameOver
        var botoes = gameOverMenu.transform.GetComponentsInChildren<Button>();
        Button botaoContinue = null;

        //Varre todos os botoes, em busca do botao continue. 
        foreach (var botao in botoes) {
            if (botao.gameObject.name.Equals("BotaoContinuar")) {
                botaoContinue = botao;//Salva uma referencia para o botao continue
                break;
            }
        }
        //Verifica se o botaoContinue eh diferente de null
        if (botaoContinue) {
#if UNITY_ADS
            //Se o botao continue for clicado, iremos tocar o anúncio
            StartCoroutine(ShowContinue(botaoContinue));
#else
            //Se nao existe add, nao precisa mostrar o botao Continue
            botaoContinue.gameObject.SetActive(false);
#endif
        }
    }

    /// <summary>
    /// Busca o MenuGameOver
    /// </summary>
    /// <returns>O GameObject MenuGameOver</returns>
    GameObject GetGameOverMenu() {
        return GameObject.Find("Canvas").transform.Find("MenuGameOver").gameObject;
    }

    /// <summary>
    /// Faz o reset do jogo
    /// </summary>
    public void Continue() {
        var go = GetGameOverMenu();
        go.SetActive(false);
        jogador.SetActive(true);
        //Exploda essa obstaculo, caso o jogador resolvar apertar o Continue.
        ObjetoTocado();
    }

    /// <summary>
    /// Metodo invocado através do SendMessage() para detectar que este objeto foi tocado
    /// </summary>
    public void ObjetoTocado() {
        if (explosao) {
            //Cria o efeito de explosao
            var particulas = Instantiate(explosao, transform.position, Quaternion.identity);
            //Destroi as particulas
            Destroy(particulas, 1.0f);
            //Emite som
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        }
        //Destroi o obstaculo
        Destroy(this.gameObject);
    }

    public IEnumerator ShowContinue(Button botaoContinue) {
        var btnText = botaoContinue.GetComponentInChildren<Text>();
        while (true) {
            if (UnityAdControle.proxTempoReward.HasValue &&
            (DateTime.Now < UnityAdControle.proxTempoReward.Value)) {
                botaoContinue.interactable = false;
                print("Aqui" + Time.timeScale);
                print(MenuPauseComp.pausado);
                TimeSpan restante = UnityAdControle.proxTempoReward.Value -
                    DateTime.Now;

                var contagemRegressiva = string.Format("{0:D2}:{1:D2}",
                                                       restante.Minutes,
                                                       restante.Seconds);
                btnText.text = contagemRegressiva;
                yield return new WaitForSeconds(1f);
            } else {
                botaoContinue.interactable = true;
                botaoContinue.onClick.AddListener(UnityAdControle.ShowRewardAd);
                UnityAdControle.obstaculo = this;
                btnText.text = "Continue (Ver Ad)";
                break;
            }
        }
    }
}
