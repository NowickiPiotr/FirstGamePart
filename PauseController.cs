using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{

    public Image PausePanel;
    public Image BackToMenu;
    public Image StartGame;
    public Image MainMenu;
    public GameObject PlayerController;

    public Button MainMenuButton;
    public Text TimerText;
    public Rigidbody PlayerRigidBody;
   

    private int _timeLeft = 3;
    private PlayerMovement _playerMovement;
    private float _speedHelper = 0;

    private bool _isPaused;
    public bool IsPaused
    {
        get { return _isPaused; }
        private set { _isPaused = value; }
    }

    void Awake()
    {
        PlayerController = GameObject.FindGameObjectWithTag(TagHelper.Player.ToString());
        PlayerRigidBody = PlayerController.GetComponent<Rigidbody>();
        _playerMovement = new PlayerMovement();
        
        BackToMenu.enabled = false;
        StartGame.enabled = false;
        PausePanel.enabled = false;
       
    }
    

    void Update()
    {
        TimerText.text = ("" + _timeLeft);
        Time.timeScale = 1;

        if (PlayerController.activeInHierarchy == false)
        {
            BackToMenu.enabled = true;
            MainMenu.enabled = false;
        }

        if (_timeLeft <= 0)
        {
            BackToMenu.enabled = false;
            MainMenuButton.interactable = true;
            StopCoroutine("LoseTime");
            _playerMovement.speed = _speedHelper;

            TimerText.enabled = false;
            _timeLeft = 3;

           PlayerRigidBody.constraints = RigidbodyConstraints.FreezeRotationX |
RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsPaused)
            {
                PauseToggle();
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
       
    }

    IEnumerator Starter()
    {
        yield return new WaitForSeconds(1);
    }

    IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _timeLeft--;
            if(_timeLeft == 0)
            {
                IsPaused = false;
            }
        }
    }

    public void Play()
    {
        MainMenuButton.interactable = false;
        PausePanel.enabled = false;

        MainMenu.enabled = true;
        BackToMenu.enabled = false;
        StartGame.enabled = false;
        TimerText.enabled = true;
        StartCoroutine("LoseTime");
        Time.timeScale = 1;
    }

    public void PauseToggle()
    {
        if (!IsPaused)
        {
            IsPaused = true;
            _speedHelper = _playerMovement.speed;
            _playerMovement.speed = 0;
            PlayerRigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ
               | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;


            Time.timeScale = 0;
            _timeLeft = 3;
            PausePanel.enabled = true;
            BackToMenu.enabled = true;
            StartGame.enabled = true;
            MainMenuButton.enabled = true;
            MainMenu.enabled = false;
        }
    }

    public void SceneChanger(string scene)
    {
        SceneManager.LoadScene(scene);
    }

}
