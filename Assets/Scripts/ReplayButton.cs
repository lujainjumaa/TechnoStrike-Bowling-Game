using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayButton : MonoBehaviour
{
    public ReceiveAndMove receiveAndMove;
    public Animator animator;
    private float preVx;
    private float preVz;
    private float preXpos;
    public void OnButtonClick()
    {
        print("replay clicked");
        animator.Play("CameraPlay");
        print("ani done");
        receiveAndMove.ReplayButton=true;
        // receiveAndMove.PreVx=144.9494978289678f;
        // receiveAndMove.PreXpos=45.40059347181008f;
        // receiveAndMove.PreVz=-2.5497065280241267f;
    }

    void Update()
    {
        // Check if the mouse is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // print("mouse is down from replay");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // print(ray);

            // Check if the raycast hits a collider (your button)
            if (Physics.Raycast(ray, out hit))
            {
                // print("you hit a colider with your mouse");
                // If the clicked object has a specific tag or name, trigger an event
                if (hit.transform == transform)  // Replace with your button's name
                {
                    OnButtonClick();
                }
            }
        }
    }
}
