using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    [SerializeField] GameObject panelEndGame;

    void Start()
    {
        
    }

    public void CheckFinish(PlayerController playerController)
    {
        GameManager gm = GameManager.instance;
        
        if (gm.qtdBeets >= gm.totalItens && gm.qtdPumpkins >= gm.totalItens)
        {
            //playerController.MovePlayer(false);
            //playerController.stateMachine.ChangeState(playerController.idleState);
            StartCoroutine(ShowPanelEndGame(2f, playerController));
        }
    }

    IEnumerator ShowPanelEndGame(float time, PlayerController playerController)
    {
        yield return new WaitForSeconds(time);
        playerController.thisAnimator.SetFloat("fVelocity", 0);
        playerController.MovePlayer(false);
        panelEndGame.SetActive(true);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Farm");
    }
}
