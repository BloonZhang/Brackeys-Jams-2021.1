using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //////// Singleton shenanigans ////////
    private static CameraController _instance;
    public static CameraController Instance { get {return _instance;} }
    //////// Singleton shenanigans continue in Awake() ////

    // GameObjects
    private Camera mainCamera;

    // definition variables
    //private float horizontalMoveDistance = 16f;
    private float verticalMoveDistance = 10f;
    //private float horizontalCameraSpeed = 1f;
    private float verticalCameraTime = 0.5f;
    private int coroutineSteps = 30;
    private float pixelUnits = 64f;
    // TODO: don't hardcode
    private float leftEdge = 0f;    // left and right constrants of camera
    private float rightEdge = 10f;

    // helper variabes


    void Awake()
    {
        // Singleton shenanigans
        if (_instance != null && _instance != this) {Destroy(this.gameObject);} // no duplicates
        else {_instance = this;}
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        //Debug.Log(mainCamera.ScreenToWorldPoint(new Vector3(10f,0,0)));
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: constrain to edges
        /*
        mainCamera.transform.position = new Vector3(PlayerController.Instance.transform.position.x,
                                                    mainCamera.transform.position.y,
                                                    mainCamera.transform.position.z);
        */
        // Lock to pixel position
        float cameraXPosition = Mathf.Round(PlayerController.Instance.transform.position.x * pixelUnits) / pixelUnits;
        cameraXPosition = Mathf.Max(leftEdge, cameraXPosition); cameraXPosition = Mathf.Min(cameraXPosition, rightEdge);
        mainCamera.transform.position = new Vector3(cameraXPosition,
                                                    Mathf.Round(mainCamera.transform.position.y * pixelUnits) / pixelUnits,
                                                    mainCamera.transform.position.z);    }


    // helper methods
    void PauseGame()
    {
        // TODO: pause game
    }

    void PlayGame()
    {
        // TODO: play game
    }

    // camera moving methods
    // vertical move: false for down, true for up
    public void MoveCamera(bool vertical)
    {
        StopAllCoroutines();
        if (vertical) { StartCoroutine(MoveUp()); }
        else { StartCoroutine(MoveDown()); }
    }

    private IEnumerator MoveUp()
    {
        float destinationY = mainCamera.transform.position.y + verticalMoveDistance;
        while (mainCamera.transform.position.y < destinationY)
        {
            mainCamera.transform.position += new Vector3(0, verticalMoveDistance / (float)coroutineSteps, 0);
            yield return new WaitForSeconds( verticalCameraTime / (float)coroutineSteps );
        }
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x,
                                                    destinationY,
                                                    mainCamera.transform.position.z);
        yield return null;
    }

    private IEnumerator MoveDown()
    {
        float destinationY = mainCamera.transform.position.y - verticalMoveDistance;
        while (mainCamera.transform.position.y > destinationY)
        {
            mainCamera.transform.position -= new Vector3(0, verticalMoveDistance / (float)coroutineSteps, 0);
            yield return new WaitForSeconds( verticalCameraTime / (float)coroutineSteps );
        }
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x,
                                                    destinationY,
                                                    mainCamera.transform.position.z);
        yield return null;
    }
}
