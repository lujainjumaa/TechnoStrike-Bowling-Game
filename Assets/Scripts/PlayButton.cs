using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public Animator animator;
    // This function will be called when the button is clicked
    public void OnButtonClick()
    {
        // SceneManager.LoadSceneAsync(0);
        // print("play button pressed");
        // animator.Play("CameraPlay");
    }

    void Update()
    {
        // Check if the mouse is clicked
        // if (Input.GetMouseButtonDown(0))
        // {
        //     // print("mouse is down from play");
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //     // print(ray);

        //     // Check if the raycast hits a collider (your button)
        //     if (Physics.Raycast(ray, out hit))
        //     {
        //         // print("you hit a colider with your mouse");
        //         // If the clicked object has a specific tag or name, trigger an event
        //         if (hit.transform == transform)  // Replace with your button's name
        //         {
        //             OnButtonClick();
        //         }
        //     }
        // }
    }
}
