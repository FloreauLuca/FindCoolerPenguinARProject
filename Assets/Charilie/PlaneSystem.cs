using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ARPlane))]
public class PlaneSystem : MonoBehaviour
{
    ARAnchorManager anchorManager;
    ARPlane arPlane;
    List<ARAnchor> anchors = new List<ARAnchor>();
    const int characterNbByMagnitude = 10;
    float magnitude;
    // Start is called before the first frame update
    void Awake()
    {
        arPlane = GetComponent<ARPlane>();
        anchorManager = FindObjectOfType<ARAnchorManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnBoundaryChanged(ARPlaneBoundaryChangedEventArgs eventArgs)
    {
        var boundary = arPlane.boundary;
        Debug.LogWarning("Charlie OnBoundaryChanged: " + magnitude + "; " + arPlane.size.magnitude);
        magnitude = arPlane.size.magnitude;
        SpawnCharacters();
    }
    void OnEnable()
    {
        arPlane.boundaryChanged += OnBoundaryChanged;
        OnBoundaryChanged(default(ARPlaneBoundaryChangedEventArgs));
    }

    void OnDisable()
    {
        arPlane.boundaryChanged -= OnBoundaryChanged;
    }


    public void SpawnCharacters()
    {
        ARAnchor anchor = null;
        Debug.LogWarning("Charlie ARPlane alignement: " + arPlane.alignment);
        Debug.LogWarning("Charlie ARPlane: " + arPlane.center + arPlane.extents);
        if (arPlane.alignment == PlaneAlignment.HorizontalUp)
        {
            Pose hitPose;
            for (var i = anchors.Count; i < magnitude * characterNbByMagnitude; i++)
            {
                var spawnPoint = arPlane.center + new Vector3(Random.Range(-arPlane.extents.x, arPlane.extents.x), 0, Random.Range(-arPlane.extents.y, arPlane.extents.y));
                hitPose.position = spawnPoint;
                hitPose.rotation = Quaternion.Euler(0, Random.Range(-180, 180), 0);
                anchor = anchorManager.AttachAnchor(arPlane, hitPose);
                anchors.Add(anchor);
            }
        }
    }
}
