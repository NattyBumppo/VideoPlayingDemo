using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VideoPlayManager : MonoBehaviour
{

    public List<GazeBasedVideoPlayer> videoPlayers;

    public MeshRenderer cursorRenderer;

	void Start()
	{
	
	}
	
	void Update()
	{
        HideCursorWhenPlaying();
        EnableCollidersBasedOnCameraDirection();
    }

    private void EnableCollidersBasedOnCameraDirection()
    {
        for (int i = 0; i < videoPlayers.Count; i++)
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

        for (int i = 0; i < videoPlayers.Count; i++)
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
        for (int i = 0; i < videoPlayers.Count; i++)
        {
            videoPlayers[i].PausePlaying();
        }
    }

    public void StopAllPlayback()
    {
        for (int i = 0; i < videoPlayers.Count; i++)
        {
            videoPlayers[i].StopPlaying();
        }
    }
}
