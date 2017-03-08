using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VideoPlayManager : MonoBehaviour
{

    public List<GazeBasedVideoPlayer> videoPlayers;

	void Start()
	{
	
	}
	
	void Update()
	{
	
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
