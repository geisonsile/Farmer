using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    [SerializeField] GameObject panelEndGame;
    [SerializeField] AudioClip endGameSFX;

    void Start()
    {
        
    }

    public IEnumerator ShowPanelEndGame(float time, PlayerController playerController)
    { 
        yield return new WaitForSeconds(time);
        
        playerController.MovePlayer(false);
        playerController.thisRigidbody.isKinematic = true;
        playerController.thisAnimator.SetFloat("fVelocity", 0);

        panelEndGame.SetActive(true);
        AudioSource.PlayClipAtPoint(endGameSFX, Camera.main.transform.position);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Farm");
    }
}
