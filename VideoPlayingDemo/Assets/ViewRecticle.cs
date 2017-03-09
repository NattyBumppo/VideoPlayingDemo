using UnityEngine;
using System.Collections;

public class ViewRecticle : MonoBehaviour
{

    public Texture2D reticleTexture;
    private Rect centerRect;
    public bool reticleOn;

    public int recticleSize;

	void Start()
	{
	    centerRect = new Rect((Screen.width - recticleSize) / 2, (Screen.height - recticleSize) / 2, recticleSize, recticleSize);
    }
	
	void OnGUI()
	{
	    if (reticleOn)
        {
            GUI.DrawTexture(centerRect, reticleTexture);
        }
	}
}
