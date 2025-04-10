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
public class MainScript : MonoBehaviour{
    private string url = "http://localhost:5000";
    // private string url = "http://192.168.0.107:5000";
    private int shots=0;
    public Rigidbody sphere;
    private SpherePos spherepos;
    public Animator animator;
    private bool loadscene = false;
    // public  Vector3 offset=new Vector3(0,0,0);
    public Transform cameraTransform;
    // private bool cameraArrives = false;
    // private float delayTimer = 0f;
    // private bool isDelaying = false;
    private bool isFollowing = false;
    public GameObject[] pins;
    public TMP_Text scoreText;
    public TMP_Text highscoreText;
    public Image scoreImage;
    static int score = 0;
    private Vector3 textOriginalPos;
    public bool gameStarted = false;
    private Vector3 originalCameraPosition = new Vector3(-15.18f, 6.89f, 0.05f);


    public class SpherePos{
        public float VX;
        public float VZ;
        public float Xpos;
        public bool BShot = false;

    }

     void Start()
    {
        
        // followPS.Stop(); // Ensure the particles are off at the start
        // fireWorksPS.Stop();

        if(!gameStarted)
        {
            scoreText.text = "";
            scoreImage.enabled = false;
        }
        // // highscoreText.text = "";
        // CameraShaker.Instance.ShakeOnce(2f, 4f, 4f, 0.1f);
        // print(cameraTransform.position);
        // originalCameraPosition = cameraTransform.position;
        textOriginalPos = scoreText.transform.position;
        StartCoroutine(SendRequestEverySecond());
    }
    void Update(){

        // camera.position = sphere.transform.position - offset;
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     Debug.Log("Q key pressed");
           
            
        // }
        // cameraTransform.position = sphere.transform.position + new Vector3(-5f, 2f, 0);
        // print(cameraTransform.position);
        // print(sphere.transform.position.x);
        
        if (isFollowing)
        {
            // Follow the ball while it's moving
            if (sphere.transform.position.x < 30f)
            {
                cameraTransform.position = new Vector3(sphere.transform.position.x - 5, 4.6f, 0f);
            }
            else
            {
                // Start the delay when the ball moves far enough
                isFollowing = false;
                gameStarted = true;
                StartCoroutine(DelayAndCalculateFallenPins(2f));
                StartCoroutine(DelayAndResetCamera(7f));
            }
        }

        // if(cameraTransform.position.x<27f)
        // cameraTransform.position = new Vector3(sphere.transform.position.x-5, 4.6f, 0f) ;
        // else{
        //     // Debug.Log("Delay started");
        //     if(!cameraArrives){
        //         cameraArrives = true;
        //         StartCoroutine(DelayAndResetCamera(5f));
        //         // print("delay starts");
        //         // delayTime(5f);
        //         // print("Delay finished");

        //         // StartCoroutine(delay(7f));
        //     }
        // }
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
                print("spherePos");
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
        isFollowing = true;
        // animator.speed = spherepos.VX/250;
        // if(!loadscene){
            // animator.Play("Following1");
        // }
        // else{
            // animator.Play("Following2");
        // }
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
            Debug.Log("Received JSON: " + jsonResponse);
            
            callback?.Invoke(jsonResponse);
        }
        else
        {
            Debug.LogError("Request failed: " + webRequest.error);
        }
    }
    void ProcessJsonResponse(string json)
    {
        // Debug.Log("Received JSON: " + json);
        spherepos = JsonUtility.FromJson<SpherePos>(json);
        
    }

     void setPs(float XPos)
    {
        sphere.transform.position = new Vector3(0, 3,-XPos/50);
    }
     void Run(float Xspeed, float Zspeed , float Xpos)
    {
        setPs(Xpos);
        sphere.AddForce(Xspeed*35, 0, -Zspeed*5);
        shots++;
        // print("Zspeed="+Zspeed);
        // print("Xspeed="+Xspeed);

    }
    IEnumerator delay(float seconds)
    {
        Debug.Log("before delay");
        yield return new WaitForSeconds(seconds); // Wait for 7 seconds
        Debug.Log("after delay"); // Wait for 2 seconds (or any duration you prefer)
    }
    void delayTime(float seconds){
        float delayTimer = 0f;
        while(delayTimer<=seconds){
            delayTimer += Time.deltaTime;
        }
    }
    IEnumerator DelayAndResetCamera(float seconds)
    {
        // isDelaying = true;
        yield return new WaitForSeconds(seconds);
        if(shots >= 2 || FallenPins()==10){
            SceneManager.LoadScene(0);
        }
        cameraTransform.position = originalCameraPosition;
        // isDelaying = false;
        sphere.velocity = Vector3.zero;
        sphere.angularVelocity = Vector3.zero;
        sphere.transform.position = new Vector3 (-0.07999943f,2.613499f,-3.125679e-07f);
        StartCoroutine(SendRequestEverySecond()); // Start waiting for the ball to be shot again
    }
    IEnumerator DelayAndCalculateFallenPins(float seconds)
    {
        // isDelaying = true;
        yield return new WaitForSeconds(seconds);
        int numOfFallenPins = FallenPins();
        scoreImage.enabled = true;
        scoreText.transform.position = textOriginalPos;
        scoreText.text = numOfFallenPins.ToString() + " POINTS";
        animator.speed = 1;
        animator.Play("ScoreShowing");
        StartCoroutine(HideScoreTextAfterDelay());
    }
    IEnumerator HideScoreTextAfterDelay()
    {
        yield return new WaitForSeconds(5f); // Wait for 2 seconds (or any duration you prefer)
        scoreText.text = ""; // Clear the score text
        gameStarted = false;

    }
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
}