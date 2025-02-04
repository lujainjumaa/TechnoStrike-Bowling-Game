using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine.Networking;
using System;
using System.Threading;
using UnityEngine.SceneManagement;
using EZCameraShake;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using TMPro;


public class ReceiveAndMove : MonoBehaviour
{
    //  private string url = "http://localhost:5000";
    private string url = "http://192.168.0.107:5000";

    public Rigidbody sphere;
    private SpherePos spherepos;
    public ReplayButton replayButton;
    public bool ReplayButton=false;
    public Transform cameraTransform;
    public ParticleSystem followPS;
    public ParticleSystem fireWorksPS;
    public Animator animator;
    public GameObject[] pins;
    public bool shakeOnce = false;
    // public static int score = 0;
    bool loadscene = false;
    public TMP_Text scoreText;
    public TMP_Text highscoreText;
    public Image scoreImage;
    static int score = 0;
    public static bool gameStarted = false;
    // int highscore = 0;

    // public float stopX = 40f; // The x position where the camera should stop following
    // public float XPosMap;
    // public Vector3 MainCameraPosition = new Vector3(-44.44f,17.4f, 1f);
    public class Player{
        public static int playerCounter = 1;
        public int number;
        Vector<int> Scores;
        public Player(){
            number = playerCounter;
            playerCounter++;
        }
    }

    public class SpherePos{
        public float VX;
        public float VZ;
        public float Xpos;
        public bool BShot = false;

    }
    IEnumerator SendRequestEverySecond()
    {
        // if(ReplayButton)
        // {
        //     print(ReplayButton);
        //     yield return new WaitForSeconds(5);
        // }
        
        while (true)
        {
            yield return StartCoroutine(GetRequest(url, ProcessJsonResponse));

            // if(ReplayButton){
            //     print(PreVx);
            //     print(PreVz);
            //     Run(PreVx,PreVz,PreXpos);
            //     break;
            // }

            if (spherepos.BShot){
                break;
            }
        }
        // PRINTING:

        // print("spherepos.Xpos="+spherepos.Xpos);
        // print("spherepos.VX="+spherepos.VX);
        // print("spherepos.VZ="+spherepos.VZ);

        // PreVx=spherepos.VX;
        // PreVz=spherepos.VZ;
        // PreXpos=spherepos.Xpos;

        Run(spherepos.VX,spherepos.VZ,spherepos.Xpos);
        animator.speed = spherepos.VX/250;
        if(!loadscene){
            animator.Play("Following1");
        }
        else{
            animator.Play("Following2");
        }
        // yield return new WaitForSeconds(5);
          
    }
    IEnumerator GetRequest(string uri, System.Action<string> callback)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(uri);

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.Success)
        {
            // Print response headers for debugging
            string responseHeaders = webRequest.GetResponseHeaders().ToString();
            // Debug.Log("Response Headers: " + responseHeaders);
            
            // Print the response to see what we're getting
            string jsonResponse = webRequest.downloadHandler.text;
            // Debug.Log("Received JSON: " + jsonResponse);
            
            callback?.Invoke(jsonResponse);
        }
        else
        {
            // Debug.LogError("Request failed: " + webRequest.error);
        }
    }
    void ProcessJsonResponse(string json)
    {
        // Debug.Log("Received JSON: " + json);
        spherepos = JsonUtility.FromJson<SpherePos>(json);
        
    }

    void Start()
    {
        if (followPS != null)
        {
            followPS.Stop(); // Ensure the particles are off at the start
        }
        fireWorksPS.Stop();
        if(gameStarted)
        {
            scoreImage.enabled = true;
            scoreText.text = score.ToString() + " POINTS";
            animator.Play("ScoreShowing");
            StartCoroutine(HideScoreTextAfterDelay());
        }
        else 
        {
            scoreText.text = "";
            scoreImage.enabled = false;
        }
        // highscoreText.text = "";
        
        StartCoroutine(SendRequestEverySecond());
    }
    void setPs(float XPos)
    {
        sphere.transform.position = new Vector3(0, 3,-XPos/50);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)){
            FindObjectOfType<AudioManager>().Play("22");
            Run(140, -30, 0);
            // animator.speed = 200/250;
            animator.Play("Following2");
        }
        if(Input.GetKeyDown(KeyCode.E)){
            FindObjectOfType<AudioManager>().Play("22");
            Run(150, 0, 0);
            // animator.speed = 200/250;
            animator.Play("Following2");
        }
        

// == new Vector3(11.66f, 6.38f, 0.40f)
        // if(cameraTransform.position.x > 11.2f){
         if(Input.GetKeyDown(KeyCode.Space)){
                // score += FallenPins();
                // scoreText.text = FallenPins().ToString() + " POINTS";
                if(FallenPins()==10){
                    animator.Play("BackToMenu");
                    loadscene = true;
                }else{
                    loadscene = true;
                    StartCoroutine(SendRequestEverySecond());
                }
        }

        if (FallenPins()==10)
        {
            // Perform your specific action when all pins have fallen
            if(!shakeOnce){
                shakeOnce=true;
                fireWorksPS.Play();
                }
            Debug.Log("All pins have fallen!");
        }
        
        {//dana home trying
            // if(sphere.transform.position.x>30f){
        //     if(!shakeOnce){
        //         score = FallenPins();
        //         if(FallenPins()==10){
        //             SceneManager.LoadSceneAsync(0);
        //         }
        //         print("Score : " + score);
        //         shakeOnce=true;
        //     }
        // }

        // if(spherepos.BShot && !loadscene){
        //     loadscene = true;
        //     StartCoroutine(SendRequestEverySecond());
        // }
        // score = FallenPins();
        // print("Score : " + score);
        }
        if (spherepos != null && spherepos.BShot)
        {
            // scoreText.text = FallenPins().ToString() + " POINTS";
            gameStarted = true;
            if(loadscene && cameraTransform.position.y >= 17.4)
            {
            // print("loadscene");
            score = FallenPins();
            // scoreText.text = score.ToString() + " POINTS";
            // print(score+"\\1");
            SceneManager.LoadSceneAsync(0);
            loadscene=false;
            }
        }
        
       followPS.transform.position = sphere.transform.position - new Vector3(1f, 0.5f, 0);

    }
    
    // void Repaly(){
    //     if( GameHadStarted ){
    //         setPs(OldZpos);
    //         Run(OldVx,OldVz);
    //     }
    // }

    int FallenPins()
    {
        int NumOfPins = 0;
        foreach (GameObject pin in pins)
        {
            // Check if the pin's rotation on the X or Z axis exceeds the threshold
            if(pin.transform.forward.y<=0.7)
            {
                NumOfPins++;
            }
        }
        return NumOfPins;
    }
    void Run(float Xspeed, float Zspeed , float Xpos)
    {
        setPs(Xpos);
        sphere.AddForce(Xspeed*35, 0, -Zspeed*5);
        // print("Zspeed="+Zspeed);
        // print("Xspeed="+Xspeed);

    }
    IEnumerator HideScoreTextAfterDelay()
    {
        yield return new WaitForSeconds(5f); // Wait for 2 seconds (or any duration you prefer)
        scoreText.text = ""; // Clear the score text
    }

    public static float MapRange(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        if (oldMax - oldMin == 0)
        {
            print("The input range cannot have zero length");
        }

        value = Math.Max(oldMin, Math.Min(value, oldMax));

        float newValue = (value - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
        
        return newValue;
    }

}