using System;
using TMPro;
using UnityEngine;

public class MoedasTextController : MonoBehaviour
{
    private TMP_Text moedasText;

    private void OnValidate()
    {
        if (moedasText == null)
        {
            moedasText = GetComponent<TMP_Text>();
        }
    }

    private void OnEnable()
    {
        //Adiciona o inscrito para o evento de mudança de moedas
        PlayerObserverManager.OnMoedasChanged += AtualizarMoedas;
    }

    private void OnDisable()
    {
        PlayerObserverManager.OnMoedasChanged -= AtualizarMoedas;
    }

    private void AtualizarMoedas(int coins)
    {
        moedasText.text = "Moedas:" + coins.ToString();
    }
}
