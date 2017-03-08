using UnityEngine;
using System.Collections;

public class ViewRecticle : MonoBehaviour
{

    public Texture2D reticleTexture;
    private Rect centerRect;
    public bool reticleOn;

	void Start()
	{
	    centerRect = new Rect((Screen.width - reticleTexture.width) / 2, (Screen.height - reticleTexture.height) / 2, reticleTexture.width, reticleTexture.height);
    }
	
	void OnGUI()
	{
	    if (reticleOn)
        {
            GUI.DrawTexture(centerRect, reticleTexture);
        }
	}
}
