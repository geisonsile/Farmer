using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    [SerializeField] GameObject panelItens;
    [SerializeField] List<GameObject> lstTutorialSteps;  /*!< Lista com todos os passos do tutorial usando o prefab StepTutorial_Sys */
    [SerializeField] GameObject btnPrevious;           /*!< Botão passo anterior */
    [SerializeField] GameObject btnNext;              /*!< Botão próximo passo */
    [SerializeField] GameObject btnStart;             /*!< Botão para começar o jogo */ 

    int indexStep = 0;              /*!< Índice do passo atual */

    [SerializeField] UnityEvent StartGame; /*!< Evento para chamar por algum script do jogo */


    private void Start()
    {
        /*! Verifica se o tutorial deve ser exibido */
        if (GameManager.Instance.tutorialShow)
        {
            for (int i = 1; i < lstTutorialSteps.Count; i++)
            {
                lstTutorialSteps[i].SetActive(false);
            }

            panelItens.SetActive(true);
            btnPrevious.SetActive(false);
        }
        else
        {
            Close();
        }
    }

    /*! Método do botão passo anterior */
    public void PreviousStep()
    {
        if (indexStep > 0)
        { 
            lstTutorialSteps[indexStep].SetActive(false);
            indexStep--;
            ActualStep();
        }
    }

    /*! Método do botão próximo passo */
    public void NextStep()
    {
        if (indexStep < lstTutorialSteps.Count-1)
        {
            lstTutorialSteps[indexStep].SetActive(false);
            indexStep++;
            ActualStep();
        }
    }

    /*! Ações referentes ao passo atual */
    private void ActualStep()
    {
        lstTutorialSteps[indexStep].SetActive(true);

        //GetComponentInChildren<TextMeshProUGUI>().text = lstStepsTxt[indexStep];

        btnPrevious.GetComponentInChildren<TextMeshProUGUI>().text = (indexStep).ToString();
        btnNext.GetComponentInChildren<TextMeshProUGUI>().text = (indexStep + 2).ToString();

        /*! Se o estiver no primeiro passo, não exibe o botão de passo anterior */
        if (indexStep == 0)
        {
            btnPrevious.GetComponent<RectTransform>().localScale = new Vector2(1,1);
            btnPrevious.SetActive(false);
        }
        else
        {
            btnPrevious.SetActive(true);
        }

        /*! Se o estiver no último passo, não exibe o botão de próximo passo e exibe o botão de iniciar o Jogo */
        if (indexStep == lstTutorialSteps.Count - 1)
        {
            btnNext.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            btnNext.SetActive(false);
            btnStart.SetActive(true);
        }
        else
        {
            btnNext.SetActive(true);
            btnStart.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            btnStart.SetActive(false);
        }
    }

    /*! Usado pelos botões Pular e Começar */
    public void Close()
    {
        panelItens.SetActive(false); 
        StartGame.Invoke();
    }
   
}
