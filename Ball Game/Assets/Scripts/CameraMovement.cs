using System;
using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    Vector3 direction;
    Transform playerTransform;
    Renderer playerMat;
    RaycastHit hitInfo;
    Camera myCamera;

    Vector3[] clipPoints = new Vector3[5];

    //Check some death script
    [HideInInspector]
    public bool cameraLocked;
    [HideInInspector]
    public static bool gamePaused;
    //Movement script
    [HideInInspector]
    public static bool setTopDown;
    public float height;
    public float zoomSpeed = 100.0f;
    //Check optionsMenuBehaviour
    bool stop;
    bool zoom;
    bool isHitting;
    bool[] isHittingTerrain = new bool[5];

    int layerMaskTerrain;

    public float step;
    float minDistance;
    float horizontalInput;
    float distance = 10.0f;
    float startingZoomSpeedVal;
    float playHitDist;
    float clipPlaneDist;
    float[] minDistances = new float[5];

    //Camera top down variables
    public float topHeight;
    bool ignoreZoom;
    float startingHeight;

    // Use this for initialization
    void Start ()
    { 
        myCamera = GetComponent<Camera>();
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        playerMat = playerTransform.gameObject.GetComponent<Renderer>();
        Cursor.visible = false;
        startingZoomSpeedVal = zoomSpeed;
        clipPlaneDist = myCamera.nearClipPlane;
        startingHeight = height;

        //Bit shift for layerMask
        layerMaskTerrain = 1 << 8;
    }

    void Update()
    {
        GetInput();
        SetTopDown();

        if (!ignoreZoom)
        {
            GetClipPoints();
            Zoom();
        }

        //If the player is alive
        if (!DeathBehaviour.hasDied)
        {
            //Disable mesh renderer
            if (distance < 1)
                playerMat.material.color = new Color(playerMat.material.color.r, playerMat.material.color.g, playerMat.material.color.b, 0.5f);
            else
                playerMat.material.color = new Color(playerMat.material.color.r, playerMat.material.color.g, playerMat.material.color.b, 1.0f);
        }
    }

    void GetInput()
    {
        if (!cameraLocked && !gamePaused)
        {
            horizontalInput += Input.GetAxis("Mouse X");
        }
    }

    void SetTopDown()
    {
        if (setTopDown)
        {
            ignoreZoom = true;
            StartCoroutine(increaseHeight(true));
            distance = 10.0f;
        }
        else
        {
            ignoreZoom = false;
            StartCoroutine(increaseHeight(false));
        }
    }

    IEnumerator increaseHeight(bool increase)
    {
        while (increase && height < topHeight)
        {
            height += 1.0f;
            yield return new WaitForSeconds(step);

        }

        while(!increase && height > startingHeight)
        {
            height -= 1.0f;
            yield return new WaitForSeconds(step);
        }

    }

    void GetClipPoints()
    {
        clipPoints[0] = myCamera.ViewportToWorldPoint(new Vector3(0.5f,0.5f, -clipPlaneDist));
        clipPoints[1] = myCamera.ViewportToWorldPoint(new Vector3(0, 0, clipPlaneDist));
        clipPoints[2] = myCamera.ViewportToWorldPoint(new Vector3(1, 0, clipPlaneDist));
        clipPoints[3] = myCamera.ViewportToWorldPoint(new Vector3(0, 1, clipPlaneDist));
        clipPoints[4] = myCamera.ViewportToWorldPoint(new Vector3(1, 1, clipPlaneDist));

        Debug.DrawRay(playerTransform.position, clipPoints[0] - playerTransform.position);
        Debug.DrawRay(playerTransform.position, clipPoints[1] - playerTransform.position);
        Debug.DrawRay(playerTransform.position, clipPoints[2] - playerTransform.position);
        Debug.DrawRay(playerTransform.position, clipPoints[3] - playerTransform.position);
        Debug.DrawRay(playerTransform.position, clipPoints[4] - playerTransform.position);
    }

    void Zoom()
    {
        //Check if point 0 is clipping
        if (Physics.Raycast(playerTransform.position, clipPoints[0] - playerTransform.position, out hitInfo, 10.0f, layerMaskTerrain))
        {
            isHittingTerrain[0] = true;
            minDistances[0] = Vector3.Distance(playerTransform.position, hitInfo.point);
        }
        else
            isHittingTerrain[0] = false;

        //Check if point 1 is clipping
        if (Physics.Raycast(playerTransform.position, clipPoints[1] - playerTransform.position, out hitInfo, 10.0f, layerMaskTerrain))
        {
            isHittingTerrain[1] = true;
            minDistances[1] = Vector3.Distance(playerTransform.position, hitInfo.point);
        }
        else
            isHittingTerrain[1] = false;

        //Check if point 2 is clipping
        if (Physics.Raycast(playerTransform.position, clipPoints[2] - playerTransform.position, out hitInfo, 10.0f, layerMaskTerrain))
        {
            isHittingTerrain[2] = true;
            minDistances[2] = Vector3.Distance(playerTransform.position, hitInfo.point);
        }
        else
            isHittingTerrain[2] = false;

        //Check if point 3 is clipping
        if (Physics.Raycast(playerTransform.position, clipPoints[3] - playerTransform.position, out hitInfo, 10.0f, layerMaskTerrain))
        {
            isHittingTerrain[3] = true;
            minDistances[3] = Vector3.Distance(playerTransform.position, hitInfo.point);
        }
        else
            isHittingTerrain[3] = false;

        //Check if point 4 is clipping
        if (Physics.Raycast(playerTransform.position, clipPoints[4] - playerTransform.position, out hitInfo, 10.0f, layerMaskTerrain))
        {
            isHittingTerrain[4] = true;
            minDistances[4] = Vector3.Distance(playerTransform.position, hitInfo.point);
        }
        else
            isHittingTerrain[4] = false;


        //If at least one is clipping, then isHitting = true;
        if (isHittingTerrain[0] || isHittingTerrain[1] || isHittingTerrain[2] || isHittingTerrain[3] || isHittingTerrain[4])
            isHitting = true;
        else
            isHitting = false;


        if(isHitting)
        {
            FindMaxDist(minDistances);

            if(zoom)
            {
                //If i need to zoom in, ask if the distance will be smaller than it needs to be with next zoom. If so, snap it, else just zoom in
                if(distance - zoomSpeed * Time.deltaTime <= minDistance )
                {
                    distance = minDistance;
                    zoom = false;
                }
                else
                    distance -= zoomSpeed * Time.deltaTime;
            }
            else
                distance = minDistance;
        }
        else
        {
            //Zoom out
            if(distance + zoomSpeed * Time.deltaTime < 10.0f)
                distance += zoomSpeed * Time.deltaTime;
            zoom = true;
        }
    }

    void FindMaxDist(float[] maxDistances)
    {
        //This finds the minDistance from the player to the clip point
        minDistance = 100f;
        for (int i = 0; i < maxDistances.Length; i++)
        {
            if (maxDistances[i] == 0)
                continue;

            if (maxDistances[i] < minDistance)
                minDistance = maxDistances[i];
        }
    }

    void FixedUpdate ()
    {
        //Get the cool camera follow after death effect
        if (!cameraLocked && !gamePaused)
        {
            Vector3 dir = new Vector3(0, 0, -distance);

            //Quaternion rotation = Quaternion.Euler(verticalInput, horizontalInput, 0.0f);
            Quaternion rotation = Quaternion.Euler(height + playerTransform.position.y, horizontalInput, 0.0f);

            transform.position = playerTransform.position + rotation * dir;
        }
        transform.LookAt(playerTransform);
    }
}