using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public struct AngleHeightElevation
{
    public float angle;
    public float elevation;
    public float distance;
}

public class VideoPlayManager : MonoBehaviour
{
    public Transform videoClipParent;
    public GazeBasedVideoPlayer[] videoPlayers;

    public MeshRenderer cursorRenderer;

    public float videoPanelStartElevation;
    public float videoPanelElevationStep;
    public float videoPanelMaxElevation;
    public float videoPanelAngleStep;
    public float videoPanelMaxAngle;
    public float videoPanelStartDistance;
    public float videoPanelDistanceStep;
    public float videoPanelPerDistanceLevelOffset;

    private float currentVideoPanelElevation;
    private float currentVideoPanelAngle;
    private float currentVideoPanelDistance;

    void Awake()
    {
        videoPlayers = videoClipParent.GetComponentsInChildren<GazeBasedVideoPlayer>();

        currentVideoPanelAngle = 0.0f;
        currentVideoPanelElevation = videoPanelStartElevation;
        currentVideoPanelDistance = videoPanelStartDistance;
    }

    void Update()
	{
        HideCursorWhenPlaying();
        EnableCollidersBasedOnCameraDirection();
    }

    public AngleHeightElevation GetAvailableAngleAndHeightSlot()
    {
        AngleHeightElevation angleHeightElevation = new AngleHeightElevation();

        angleHeightElevation.angle = currentVideoPanelAngle;
        angleHeightElevation.elevation = currentVideoPanelElevation;
        angleHeightElevation.distance = currentVideoPanelDistance;

        currentVideoPanelAngle += videoPanelAngleStep;
        if (currentVideoPanelAngle > videoPanelMaxAngle)
        {
            currentVideoPanelAngle = 0.0f;
            currentVideoPanelElevation += videoPanelElevationStep;

            if (currentVideoPanelElevation > videoPanelMaxElevation)
            {
                currentVideoPanelDistance += videoPanelDistanceStep;
                currentVideoPanelElevation = videoPanelStartElevation;
                // Add an offset for non-zero distances
                int distanceIndex = Mathf.RoundToInt((currentVideoPanelDistance - videoPanelStartDistance) / videoPanelDistanceStep);

                currentVideoPanelAngle = (float)distanceIndex * videoPanelPerDistanceLevelOffset;
            }
        }

        return angleHeightElevation;
    }

    private void EnableCollidersBasedOnCameraDirection()
    {
        for (int i = 0; i < videoPlayers.Length; i++)
        {
            if (Vector3.Dot(videoPlayers[i].transform.up, Camera.main.transform.forward) < -0.4f)
            {
                // Enable collider
                videoPlayers[i].myCollider.enabled = true;
            }
            else
            {
                // Disable collider
                videoPlayers[i].myCollider.enabled = false;
            }
        }
    }

    private void HideCursorWhenPlaying()
    {
        bool currentlyPlaying = false;

        for (int i = 0; i < videoPlayers.Length; i++)
        {
            if (videoPlayers[i].movieTexture.isPlaying)
            {
                currentlyPlaying = true;
            }
        }

        cursorRenderer.enabled = !currentlyPlaying;
    }

    public void PauseAllPlayback()
    {
        for (int i = 0; i < videoPlayers.Length; i++)
        {
            videoPlayers[i].PausePlaying();
        }
    }

    public void StopAllPlayback()
    {
        for (int i = 0; i < videoPlayers.Length; i++)
        {
            videoPlayers[i].StopPlaying();
        }
    }
}
