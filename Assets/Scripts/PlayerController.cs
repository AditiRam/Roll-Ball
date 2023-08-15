using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    
    private Rigidbody rb;
    private int pickupCount;
    private bool gameOver = false;
    public GameObject resetPoint;
    private bool resetting = false;
    Color originalColour;


    //Controllers
    GameController gameController;
    private Timer timer;
    public Timer speedRunTimer;
   


    [Header("UI")]
    public GameObject inGamePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public TMP_Text winTimeText;
    public GameObject pausePanel;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Get the number of pickups in out scene
        pickupCount = GameObject.FindGameObjectsWithTag("Pick up").Length;
        //Run the check pickups function
        SetCountText();
        //Get the timer object and start the timer
        timer = FindObjectOfType<Timer>();
        timer.StartTimer();
        //turn on our in game panel
        inGamePanel.SetActive(true);
        //turn off our win panel
        winPanel.SetActive(false);
        //turn off out lose panel 
        losePanel.SetActive(false);
        //turn off the pause panel
        pausePanel.SetActive(false);

        resetPoint = GameObject.Find("Reset Point");
        originalColour = GetComponent<Renderer>().material.color;

        gameController = FindObjectOfType<GameController>();
        speedRunTimer = FindObjectOfType<Timer>();
        if (gameController.gameType == GameType.SpeedRun)
        {

        }

    

    }
    private void Update()
    {
        timerText.text = timer.GetTime().ToString("F2"); 

         if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (resetting)
            return;

        if (gameOver == true)
        {
            return;
        }
        if (gameController.controlType == ControlType.WorldTilt)
            return;

            

        float moveHorizontal = Input.GetAxis("Horizontal");
       float moveVertical = Input.GetAxis("Vertical");
       Vector3 movement =  new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pick up")
        {
            Destroy(other.gameObject);
            //Decrement the pickup count
            pickupCount -= 1;
            //Run the check pickups function
            SetCountText();
        }
        if (other.tag == "Obstacle")
        {
            //trigger loose panel player collides with obstacle
            LoseGame(); 
        }
    }

    void SetCountText()
    {
        //Display the amount of pickups left in our scene
        scoreText.text = "Pickups Left: " + pickupCount;

        if (pickupCount == 0)
        {
            WinGame();
           

        }
    }
    void WinGame()
    {
        //set game over to true
        gameOver = true; 
        //stop the timer
        timer.StopTimer();
        //turn on out win panel
        winPanel.SetActive(true);
        //turn off our in game panel
        inGamePanel.SetActive(false);
        //Display the timer on the win time text
        winTimeText.text = "your time was:" + timer.GetTime().ToString("F2"); 
        print("Yay! You Win, Your time was: " + timer.GetTime());

        //set the velocity of the rigidbody to zero
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    

    }
    void LoseGame()
    {
        //set game over to true
        gameOver = true; 
        //stop the timer
        timer.StopTimer();
        //turn on out win panel
        losePanel.SetActive(true);
        //turn off our in game panel
        inGamePanel.SetActive(false);
        //Display the timer on the win time text
        winTimeText.text = "your time was:" + timer.GetTime().ToString("F2"); 
        print("Yay! You Win, Your time was: " + timer.GetTime());

        //set the velocity of the rigidbody to zero
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

    }

    public void TogglePause()
    {//stop the timer
        timer.StopTimer();
        //turn on pause panel
        pausePanel.SetActive(true);
        //set the velocity of the rigidbody to zero
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;


    }
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
    }


    //Temporary
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Respawn"))
        {
            StartCoroutine(ResetPlayer());
        }
        
    }
    public IEnumerator ResetPlayer()
    {
        resetting = true;
        GetComponent<Renderer>().material.color = Color.black;
        rb.velocity = Vector3.zero;
        Vector3 startPos = transform.position;
        float resetSpeed = 2f;
        var i = 0.0f;
        var rate = 1.0f / resetSpeed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.position = Vector3.Lerp(startPos, resetPoint.transform.position, i);
            yield return null;
        }
        GetComponent<Renderer>().material.color = originalColour;
        resetting = false;

    }
}
