﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using Cinemachine;

public class WallMerge : MonoBehaviour
{
    public AudioClip inWall;
    private Animator playerAnimator;
    private CharacterController playerController;
    private MovementInput playerMovement;
    private Vector3 closestCorner;
    private Vector3 nextCorner;
    private Vector3 previousCorner;
    private Vector3 chosenCorner;
    private float playerZScale;
    [Header("Parameters")]
    public float transitionTime = .8f;

    private float time = 1.5f;

    [Space]

    [Header("Public References")]
    public ProjectorMovement decalMovement;
    public Transform frameQuad;
    public Transform pivotAndPoints;
    private Renderer frameRenderer;
    private CinemachineImpulseSource impulseSource;

    [Space]
    [Header("Frame Settings")]
    public Color frameLitColor;
    public Light spotLight;

    [Space]

    [Header("Cameras")]
    public GameObject gameCam;
    public GameObject wallCam;

    [Space]

    [Header("Post Processing")]
    public Volume dofVolume;
    public Volume zoomVolume;
    CinemachineBrain brain;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<MovementInput>();
        playerController = GetComponent<CharacterController>();
        brain = Camera.main.GetComponent<CinemachineBrain>();
        playerZScale = transform.GetChild(0).localScale.z;
        frameRenderer = frameQuad.GetComponent<Renderer>();
        impulseSource = Camera.main.GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        time -= Time.deltaTime;
        if(time <= -1) time = -1;
        if (Input.GetKeyDown(KeyCode.Space) && time < 0)
        {
            time = 1.5f;
            if (Physics.Raycast(transform.position + (Vector3.up * .1f), transform.forward, out RaycastHit hit, 1))
            {
                if(hit.transform.GetComponentInChildren<RaySearch>() != null)
                {
                    AudioSource.PlayClipAtPoint(inWall,transform.position,1);
                    //store raycasted object's RaySearch component
                    RaySearch search = hit.transform.GetComponentInChildren<RaySearch>();
                    //create a new list of all the corner positions
                    List<Vector3> cornerPoints = new List<Vector3>();

                    for (int i = 0; i < search.cornerPoints.Count; i++)
                        cornerPoints.Add(search.cornerPoints[i].position);

                    //find the closest corner position and index
                    closestCorner = GetClosestPoint(cornerPoints.ToArray(), hit.point);
                    int index = search.cornerPoints.FindIndex(x => x.position == closestCorner);

                    //determine the adjacent corners
                    nextCorner = (index < search.cornerPoints.Count - 1) ? search.cornerPoints[index + 1].position : search.cornerPoints[0].position;
                    previousCorner = (index > 0) ? search.cornerPoints[index - 1].position : search.cornerPoints[search.cornerPoints.Count - 1].position;

                    //choose a corner to be the target
                    chosenCorner = Vector3.Dot((closestCorner - hit.point), (nextCorner - hit.point)) > 0 ? previousCorner : nextCorner;
                    bool nextCornerIsRight = isRightSide(-hit.normal, chosenCorner - closestCorner, Vector3.up);

                    //find the distance from the origin point
                    float distance = Vector3.Distance(closestCorner, chosenCorner);
                    float playerDis = Vector3.Distance(chosenCorner, hit.point);

                    //quick fix so that we don't allow the player to start in a corner;
                    if (playerDis > (distance - decalMovement.distanceToTurn))
                        playerDis = distance - decalMovement.distanceToTurn;
                    if(playerDis < decalMovement.distanceToTurn)
                        playerDis = decalMovement.distanceToTurn;

                    //find it's normalized position in the distance of the origin and target
                    float positionLerp = Mathf.Abs(distance - playerDis) / ((distance + playerDis) / 2);

                    //start the MovementScript
                    decalMovement.SetPosition(closestCorner, chosenCorner, positionLerp, search, nextCornerIsRight, hit.normal);

                    //transition logic
                    Transition(true, Vector3.Lerp(closestCorner, chosenCorner, positionLerp), hit.normal);

                    decalMovement.transform.SetParent(hit.transform);
                    pivotAndPoints.SetParent(hit.transform);
                }
            }
        }
        #region test
        // checkTime -= Time.deltaTime;
        // if(checkTime <= 0)
        // {
        //     checkTime = 0.4f;
        //     Debug.Log(decalMovement.transform.parent);
        //     if(decalMovement.transform.parent != null)
        //     {
        //         Debug.Log("parent != null");
        //         if(decalMovement.transform.parent.GetComponentInChildren<RaySearch>() != null)
        //         {
        //             Debug.Log("RaySearch != null");
        //             //store raycasted object's RaySearch component
        //             RaySearch search = decalMovement.transform.parent.GetComponentInChildren<RaySearch>();
                    
        //             //create a new list of all the corner positions
        //             List<Vector3> cornerPoints = new List<Vector3>();

        //             for (int i = 0; i < search.cornerPoints.Count; i++)
        //                 cornerPoints.Add(search.cornerPoints[i].position);

        //             //find the closest corner position and index
        //             closestCorner = GetClosestPoint(cornerPoints.ToArray(), decalMovement.transform.position);
        //             int index = search.cornerPoints.FindIndex(x => x.position == closestCorner);

        //             //determine the adjacent corners
        //             nextCorner = (index < search.cornerPoints.Count - 1) ? search.cornerPoints[index + 1].position : search.cornerPoints[0].position;
        //             previousCorner = (index > 0) ? search.cornerPoints[index - 1].position : search.cornerPoints[search.cornerPoints.Count - 1].position;

        //             //choose a corner to be the target
        //             chosenCorner = Vector3.Dot((closestCorner - decalMovement.transform.position), (nextCorner - decalMovement.transform.position)) > 0 ? previousCorner : nextCorner;
        //             bool nextCornerIsRight = isRightSide(-hitNormal, chosenCorner - closestCorner, Vector3.up);

        //             //find the distance from the origin point
        //             float distance = Vector3.Distance(closestCorner, chosenCorner);
        //             float playerDis = Vector3.Distance(chosenCorner, decalMovement.transform.position);

        //             //quick fix so that we don't allow the player to start in a corner;
        //             if (playerDis > (distance - decalMovement.distanceToTurn))
        //                 playerDis = distance - decalMovement.distanceToTurn;
        //             if(playerDis < decalMovement.distanceToTurn)
        //                 playerDis = decalMovement.distanceToTurn;

        //             //find it's normalized position in the distance of the origin and target
        //             float positionLerp = Mathf.Abs(distance - playerDis) / ((distance + playerDis) / 2);

        //             //start the MovementScript
        //             decalMovement.SetPosition(closestCorner, chosenCorner, positionLerp, search, nextCornerIsRight, hitNormal);
        //             Debug.Log("doing");
        //         }  
        //     }
        // }
        #endregion
    }

    public void Transition(bool merge, Vector3 point, Vector3 normal)
    {

        Vector3 finalNormal = merge ? -normal : normal;
        Vector3 finalPosition = merge ? point - new Vector3(0, .9f, 0) : point;
        string animatorStatus = merge ? "turn" : "normal";
        float scale = merge ? .01f : playerZScale;
        float finalTransition = merge ? .5f : .3f;

        if (merge == true)
            FrameMovement(normal, finalPosition, finalTransition);

        transform.forward = finalNormal;
        playerAnimator.SetTrigger(animatorStatus);
        PlayerActivation(merge);
        MergeSequence(merge, finalPosition, scale, finalTransition);

        //Effects
        float dofDelay = merge ? finalTransition + .3f : 0;
        float dofAmount = merge ? 1 : 0;
        if(dofVolume != null) DOVirtual.Float(dofVolume.weight, dofAmount, finalTransition, DofPostVolume).SetDelay(dofDelay).OnComplete(() => {
                DofPostVolume(0);
            });;
        if (merge && zoomVolume != null)
            DOVirtual.Float(zoomVolume.weight, 1, .7f, ZoomVolume).OnComplete(() => {
                DOVirtual.Float(zoomVolume.weight, 0, .3f, ZoomVolume);
            });
    }

    void PlayerActivation(bool active)
    {
        if (active == true)
        {
            playerMovement.enabled = false;
            playerController.enabled = false;
            spotLight.enabled = true;
        }
        else
        {
            playerMovement.gameObject.SetActive(true);
        }
    }

    void FrameMovement(Vector3 normal, Vector3 finalPosition, float finalTransition)
    {
        frameQuad.position = transform.position + new Vector3(0, .85f, 0) - (transform.forward * .5f);
        frameQuad.forward = -normal;
        frameRenderer.material.SetColor("_BaseColor", Color.clear);
        frameRenderer.material.DOColor(frameLitColor, "_BaseColor", 1f).SetDelay(.3f);
        frameQuad.DOMove(finalPosition + new Vector3(0, .85f, 0) - (transform.forward * .05f), finalTransition).SetEase(Ease.InBack).SetDelay(.2f);
    }

    Vector3 GetClosestPoint(Vector3[] points, Vector3 currentPoint)
    {
        Vector3 pMin = Vector3.zero;
        float minDist = Mathf.Infinity;

        foreach (Vector3 p in points)
        {
            float dist = Vector3.Distance(p, currentPoint);
            if (dist < minDist)
            {
                pMin = p;
                minDist = dist;
            }
        }
        return pMin;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position + (Vector3.up*.1f), transform.forward);
        Gizmos.DrawSphere(closestCorner, .2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(previousCorner, .2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(nextCorner, .2f);

    }

    //https://forum.unity.com/threads/left-right-test-function.31420/
    public bool isRightSide(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 right = Vector3.Cross(up.normalized, fwd.normalized);        // right vector
        float dir = Vector3.Dot(right, targetDir.normalized);
        return dir > 0f;
    }

    public void DofPostVolume(float x)
    {
        dofVolume.weight = x;
    }

    public void ZoomVolume(float x)
    {
        zoomVolume.weight = x;
    }

    Sequence MergeSequence(bool merge, Vector3 finalPosition, float scale, float finalTransition)
    {
        Sequence s = DOTween.Sequence();

        if (merge)
            s.AppendInterval(.2f);
        else
            s.AppendCallback(() => decalMovement.exitParticle?.Play());
        s.AppendCallback(() => gameCam.SetActive(!merge));
        s.AppendCallback(() => wallCam.SetActive(merge));
        s.Append(transform.DOMove(finalPosition, finalTransition).SetEase(Ease.InBack));
        s.Join(transform.GetChild(0).DOScaleZ(scale, finalTransition).SetEase(Ease.InSine));
        if (merge)
            s.AppendCallback(() => playerMovement.gameObject.SetActive(false));
        s.AppendCallback(() => decalMovement.transform.GetChild(0).gameObject.SetActive(merge));
        s.AppendCallback(() => decalMovement.mergeParticle?.Play());
        if (merge == true)
            s.AppendCallback(() => impulseSource.GenerateImpulse());
        if (merge == false)
        {
            s.AppendCallback(() => playerMovement.enabled = true);
            s.AppendCallback(() => playerController.enabled = true);
        }
        s.AppendCallback(() => decalMovement.isActive = merge);
        s.Append(frameRenderer.material.DOColor(Color.clear, "_BaseColor", 1));
        return s;
    }
}
