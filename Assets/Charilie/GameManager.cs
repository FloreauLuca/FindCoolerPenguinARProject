using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    enum GameState {
        START,
        GENERATING,
        PLAY,
        END
    }

    [SerializeField] GameObject panelStart;
    [SerializeField] GameObject panelGame;
    [SerializeField] GameObject panelEnd;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject panelGenerating;
    [SerializeField] TextMeshProUGUI generatingText;
    float timer;
    [SerializeField] float minAnchor = 100;
    [SerializeField] TextMeshProUGUI endTimerText;
    GameState currentState = GameState.START;
    
    ARAnchorManager anchorManager;
    ARPlaneManager arPlaneManager;
    [SerializeField] GameObject penguinPrefab;
    [SerializeField] GameObject coolerPenguinPrefab;
    // Start is called before the first frame update
    void Start()
    {
        anchorManager = FindObjectOfType<ARAnchorManager>();
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        arPlaneManager.requestedDetectionMode = PlaneDetectionMode.None;
        panelStart.SetActive(true);
        panelGenerating.SetActive(false);
        panelGame.SetActive(false);
        panelEnd.SetActive(false);
        currentState = GameState.START;
    }

    public void Play(int nbCharacter)
    {
        minAnchor = nbCharacter;
        panelStart.SetActive(false);
        panelGenerating.SetActive(true);
        panelGame.SetActive(false);
        panelEnd.SetActive(false);
        currentState = GameState.GENERATING;
        arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Horizontal;
    }

    public void Launch()
    {
        ARPlane arPlane = FindObjectOfType<ARPlane>();
        Pose hitPose;
        var spawnPoint = arPlane.center + new Vector3(Random.Range(-arPlane.extents.x, arPlane.extents.x), 0, Random.Range(-arPlane.extents.y, arPlane.extents.y));
        hitPose.position = spawnPoint;
        hitPose.rotation = Quaternion.Euler(0, Random.Range(-180, 180), 0);
        anchorManager.anchorPrefab = coolerPenguinPrefab;
        anchorManager.AttachAnchor(arPlane, hitPose);
        anchorManager.anchorPrefab = penguinPrefab;
        arPlaneManager.requestedDetectionMode = PlaneDetectionMode.None;
        //FindObjectOfType<ARPointCloud>().enabled = false;
        panelStart.SetActive(false);
        panelGenerating.SetActive(false);
        panelGame.SetActive(true);
        panelEnd.SetActive(false);
        currentState = GameState.PLAY;
    }

    public void EndGame()
    {
        panelStart.SetActive(false);
        panelGenerating.SetActive(false);
        panelGame.SetActive(false);
        panelEnd.SetActive(true);
        currentState = GameState.END;
        
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState) {
            case GameState.START:
                break;
            case GameState.GENERATING:
                generatingText.text = "Generating... " + anchorManager.trackables.count.ToString() + "/" + minAnchor;
                if (anchorManager.trackables.count >= minAnchor)
                {
                    Launch();
                }
                break;
            case GameState.PLAY:
                //arPlaneManager.SetTrackablesActive(false);
                timer += Time.deltaTime;
                timerText.text = "Timer : " + timer.ToString();
                break;
            case GameState.END:
                endTimerText.text = timer.ToString();
                break;
            default:
                break;
        }
    }

    public void IncreaseTimer()
    {
        timer += 10;
        StopCoroutine(ChangeTextColor());
        StartCoroutine(ChangeTextColor());
    }

    IEnumerator ChangeTextColor()
    {
        Color baseColor = timerText.color;
        timerText.color = Color.red;
        yield return new WaitForSeconds(1.0f);
        timerText.color = baseColor;
    }
}
