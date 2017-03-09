using UnityEngine;
using UnityEngine.VR.WSA.Input;
using System.Collections;

public class GazeBasedVideoPlayer : MonoBehaviour
{
    public VideoPlayManager playManager;

    public float startDistance;
    public float startArc;
    //public Transform highlightPlane;
    //private float highlightPlaneDistanceOffset = 0.01f;

    public Material movieMaterial;
    public Material movieStillMaterial;
    public Material movieStillMaterialHighlighted;

    private Renderer myRenderer;
    public BoxCollider myCollider;
    private AudioSource myAudioSource;
    public MovieTexture movieTexture;
    public MeshRenderer checkmarkRenderer;

    private bool hadPlayerGazeLastFrame = false;
    private float mostRecentGazeAcquisitionTime = 0.0f;

    public float gazeTimeUntilPlayStarts;

    public bool shouldBePlaying = false;

    void Start()
	{
        // Position at startDistance from camera, at startArc along
        // a circular arc, and reorient to face the camera
        Vector3 planePosOffset = new Vector3(startDistance * Mathf.Sin(startArc * Mathf.Deg2Rad), 0.0f, startDistance * Mathf.Cos(startArc * Mathf.Deg2Rad));
        transform.position = Camera.main.transform.position + planePosOffset;
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position) * Quaternion.Euler(90.0f, 0.0f, 0.0f);

        //// Move the highlight plane just in front of the movie texture plane
        //highlightPlane.position = planePosOffset + highlightPlaneDistanceOffset * (Camera.main.transform.position - transform.position);
        //highlightPlane.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position) * Quaternion.Euler(90.0f, 0.0f, 0.0f);

        // Set material to unselected still
        myRenderer = GetComponent<Renderer>();
        myRenderer.material = movieStillMaterial;

        myCollider = GetComponent<BoxCollider>();

        myAudioSource = GetComponent<AudioSource>();
    }
	
	void Update()
	{
        //if (movieTexture.isPlaying)
        //{
        //    Debug.Log("playing");
        //}

	    if (HavePlayerGaze())
        {
            if (hadPlayerGazeLastFrame)
            {
                // We still have the player's gaze; check the time elapsed
                float elapsedTimeWithGaze = Time.fixedTime - mostRecentGazeAcquisitionTime;
                if (elapsedTimeWithGaze >= gazeTimeUntilPlayStarts)
                {
                    if (!movieTexture.isPlaying)
                    {
                        if (shouldBePlaying)
                        {
                            // Looks like we've stopped! Mark as stopped
                            MarkAsViewed();
                            StopPlaying();
                        }
                        else
                        {
                            // We need to start up playback
                            StartPlaying();
                        }
                        //playManager.StopAllPlayback();
                        
                    }
                    else
                    {
                        // Already playing
                    }
                }
            }
            else
            {
                // Just got player gaze; mark timestamp
                mostRecentGazeAcquisitionTime = Time.fixedTime;
                hadPlayerGazeLastFrame = true;

                myRenderer.material = movieStillMaterialHighlighted;

                //Debug.Log("Just got player gaze on " + this.name);
            }
        }
        else
        {
            myRenderer.material = movieStillMaterial;
            hadPlayerGazeLastFrame = false;

            PausePlaying();
        }
	}

    private bool HavePlayerGaze()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 20.0f))
        {
            //Debug.Log("collision: " + hitInfo.collider.name);
            if (hitInfo.collider.name == this.name)
            {
                // Check that we're actually facing the object in question
                //Debug.Log("Dot with " + this.name + ": " + Vector3.Dot(hitInfo.collider.transform.up, Camera.main.transform.forward));
                return true;
            }
        }

        return false;
    }

    private void StartPlaying()
    {
        //Debug.Log("Starting play for " + this.name);

        myRenderer.material = movieMaterial;
        movieTexture.Play();
        myAudioSource.Play();

        shouldBePlaying = true;
    }

    public void PausePlaying()
    {
        //Debug.Log("Pausing play for " + this.name);

        myRenderer.material = movieStillMaterial;
        movieTexture.Pause();
        myAudioSource.Pause();

        shouldBePlaying = false;
    }

    public void StopPlaying()
    {
        //Debug.Log("Stopping play for " + this.name);

        myRenderer.material = movieStillMaterial;
        movieTexture.Stop();
        myAudioSource.Stop();

        //shouldBePlaying = false;
    }

    private void MarkAsViewed()
    {
        checkmarkRenderer.enabled = true;
    }
}
