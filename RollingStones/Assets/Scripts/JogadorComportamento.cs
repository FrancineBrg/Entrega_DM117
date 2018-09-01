using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JogadorComportamento : MonoBehaviour {
    
    /// <summary>
    /// Referencia para o corpo rigido da bola
    /// </summary>
    private Rigidbody rb;
    
    [SerializeField]
    [Tooltip("Controla a velocidade de rolamento")]
    [Range(1, 20)]
    private float velocidadeRolamento = 5.0f;

    [SerializeField]
    [Tooltip("Controla a velocidade de esquiva")]
    [Range(1, 20)]
    private float velocidadeEsquiva = 5.0f;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if (MenuPauseComp.pausado)
            return;

        //Detectando uso das teclas A e D
        float velocidadeHorizontal = Input.GetAxis("Horizontal") * velocidadeEsquiva;

        //Verifica se houve clique com o mouse na tela atraves do botao direito
        if (Input.GetMouseButton(0)) {
            TocarObjeto(Input.mousePosition);
        }

        var forcaMovimento = new Vector3(velocidadeHorizontal, 0, velocidadeRolamento);

        //Time.deltaTime retorna o tempo gasto no frame anterior
        //Assim sabemos o quanto nosso jogo está atrasado.
        //Isso permite um game loop suave
        forcaMovimento *= (Time.deltaTime * 60);

        rb.AddForce(forcaMovimento);
    }
    
    /// <summary>
    /// Metodo para identificar se objetos foram tocados 
    /// </summary>
    /// <param name="pos">A posicao clicada na tela</param>
    private static void TocarObjeto(Vector3 pos) {
        Ray posicaoTelaRay = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(posicaoTelaRay, out hit)) {
            hit.transform.SendMessage("ObjetoTocado", SendMessageOptions.DontRequireReceiver);
        }
    }
}