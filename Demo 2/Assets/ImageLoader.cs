using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ImageLoader : MonoBehaviour
{
    public string url = "https://i.pinimg.com/originals/9e/1d/d6/9e1dd6458c89b03c506b384f537423d9.jpg";
    public Renderer thisRenderer;

    // automatically called when game started
    void Start()
    {
        StartCoroutine(LoadFromLikeCoroutine()); // execute the section independently

        // the following will be called even before the load finished
        thisRenderer.material.color = Color.red;
    }

    // this section will be run independently
    private IEnumerator LoadFromLikeCoroutine()


    {

        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("https://i.pinimg.com/originals/9e/1d/d6/9e1dd6458c89b03c506b384f537423d9.jpg"))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);
                Debug.Log("Loaded");
                //thisRenderer.material.color = Color.white;              // set white
                // thisRenderer.material.mainTexture = texture;  // set loaded image*/
                gameObject.GetComponent<Renderer>().material.mainTexture = texture;
            }
        }

        /*Debug.Log("Loading ....");
        UnityWebRequest wwwLoader = new UnityWebRequest(url);   // create WWW object pointing to the url
        yield return wwwLoader;         // start loading whatever in that url ( delay happens here )

        Debug.Log("Loaded");
        thisRenderer.material.color = Color.white;              // set white
        thisRenderer.material.mainTexture = wwwLoader.texture;  // set loaded image*/
    }
}