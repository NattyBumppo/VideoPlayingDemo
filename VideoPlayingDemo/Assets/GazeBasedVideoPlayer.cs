using UnityEngine;
using System.Collections;

public class GazeBasedVideoPlayer : MonoBehaviour
{
    public Camera mainCamera;
    public VideoPlayManager playManager;

    public float startDistance;
    public float startArc;
    //public Transform highlightPlane;
    //private float highlightPlaneDistanceOffset = 0.01f;

    public Material movieMaterial;
    public Material movieStillMaterial;
    public Material movieStillMaterialHighlighted;

    private Renderer myRenderer;
    private MeshCollider myCollider;
    private AudioSource myAudioSource;
    public MovieTexture movieTexture;

    private bool hadPlayerGazeLastFrame = false;
    private float mostRecentGazeAcquisitionTime = 0.0f;

    public float gazeTimeUntilPlayStarts;

    void Start()
	{
        // Position at startDistance from camera, at startArc along
        // a circular arc, and reorient to face the camera
        Vector3 planePosOffset = new Vector3(startDistance * Mathf.Sin(startArc * Mathf.Deg2Rad), 0.0f, startDistance * Mathf.Cos(startArc * Mathf.Deg2Rad));
        transform.position = mainCamera.transform.position + planePosOffset;
        transform.rotation = Quaternion.LookRotation(mainCamera.transform.position - transform.position) * Quaternion.Euler(90.0f, 0.0f, 0.0f);

        //// Move the highlight plane just in front of the movie texture plane
        //highlightPlane.position = planePosOffset + highlightPlaneDistanceOffset * (mainCamera.transform.position - transform.position);
        //highlightPlane.rotation = Quaternion.LookRotation(mainCamera.transform.position - transform.position) * Quaternion.Euler(90.0f, 0.0f, 0.0f);

        // Set material to unselected still
        myRenderer = GetComponent<Renderer>();
        myRenderer.material = movieStillMaterial;

        myCollider = GetComponent<MeshCollider>();
        myAudioSource = GetComponent<AudioSource>();
    }
	
	void Update()
	{
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
                        //playManager.StopAllPlayback();
                        StartPlaying();
                    }
                }
            }
            else
            {
                // Just got player gaze; mark timestamp
                mostRecentGazeAcquisitionTime = Time.fixedTime;
                hadPlayerGazeLastFrame = true;

                myRenderer.material = movieStillMaterialHighlighted;

                Debug.Log("Just got player gaze on " + this.name);
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
        Ray cameraRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hitInfo;

        return (myCollider.Raycast(cameraRay, out hitInfo, Mathf.Infinity));
    }

    private void StartPlaying()
    {
        Debug.Log("Starting play for " + this.name);

        myRenderer.material = movieMaterial;
        movieTexture.Play();
        myAudioSource.UnPause();
    }

    public void PausePlaying()
    {
        Debug.Log("Pausing play for " + this.name);

        myRenderer.material = movieStillMaterial;
        movieTexture.Pause();
        myAudioSource.Pause();
    }

    public void StopPlaying()
    {
        Debug.Log("Stopping play for " + this.name);

        myRenderer.material = movieStillMaterial;
        movieTexture.Stop();
        myAudioSource.Stop();
    }
}
