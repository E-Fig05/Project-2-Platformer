using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UiControllerScript : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private TMP_Text ScoreDisplay;
    [SerializeField] private TMP_Text Text;
    [SerializeField] private TMP_Text Restart;
    [SerializeField] private TMP_Text YouWin;
    //[SerializeField] private Scene MainScene;

    private void Start()
    {
        Text.enabled = false;
        Restart.enabled = false;
        YouWin.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlatformerPlayer PlayerControllerScript = Player.GetComponent<PlatformerPlayer>();

        if(PlayerControllerScript != null)
        {
            ScoreDisplay.text = "Coins: " + PlayerControllerScript.money;
            
            if(Player.activeSelf == false)
            {
                Text.enabled = true;
                Restart.enabled = true;
                if(Input.GetKeyDown("r"))
                {
                    SceneManager.LoadScene("SampleScene");
                }
            }

            if(PlayerControllerScript.EnemiesKilled >3)
            {
                YouWin.enabled = true;
                Restart.enabled = true;
                PlayerControllerScript.canMove = false;
                if (Input.GetKeyDown("r"))
                {
                    SceneManager.LoadScene("SampleScene");
                }
            }
        }
    }
}
