using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// click on button, 
/// generate URL, 
/// invoke event, --> add URL to the queue 
/// check if previous event is done,
/// remove from queue
/// destory texture ?
/// </summary>
public class Load : MonoBehaviour
{

    private Queue<string> links = new Queue<string>();
    private Image image;
    private bool done = true;
    private bool clicked = false;
    private string[] url =  { "https://freeiconshop.com/wp-content/uploads/edd/coffee-takeaway-outline-filled.png", "https://freeiconshop.com/wp-content/uploads/edd/ice-cream-cone-outline-filled.png" };
    public static event System.Action<string> generate; // creating an event queue to take care of generating the next link to display 
    private Texture2D texture2d;
    void Start(){

        image = GetComponent<Image>();
      
    }
    void Update() {

        if (clicked)
        {
            CheckQueue();
            clicked = false;
        }
    }


    public void GenerateURL(){

        clicked = true;
        //generate a random number between 0 and 1 to randomly choose between the two URLS
        var rnd = new System.Random();
        int num = rnd.Next(0, 2);

        //when the button is clicked the event is invoked 
        generate.Invoke(url[num]);
      
        
        UnityEngine.Debug.Log("CountClick: " + links.Count);
    }

    private void OnEnable() {
        //subscribe to event 
        generate += AddToQ;
    
    }
    private void OnDisable() {
        //unsubscribe to event 
        generate -= AddToQ;

    }
    private void AddToQ(string url) {

        links.Enqueue(url);
    }
    void CheckQueue(){

        

        if (done)
        {
            done = false; // reset flag
            UnityEngine.Debug.Log("Count: " + links.Count);
            if (links.Count > 0)
            {
                Destroy(texture2d);
                DisplayImage(links.Dequeue()); // get the first link in the queue

            }
        }
    }

    void DisplayImage(string url) {

        StartCoroutine(downloadImage(url));
    }
    IEnumerator downloadImage(string url){

       

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        DownloadHandler handle = www.downloadHandler;

        //Send request and wait
        yield return www.SendWebRequest();

        if ((www.result == UnityWebRequest.Result.ConnectionError) || (www.result == UnityWebRequest.Result.ProtocolError))
        {
            UnityEngine.Debug.Log("Error while Receiving: " + www.error);
        }
        else
        {
            UnityEngine.Debug.Log("Success");

            //Load Image
            texture2d = DownloadHandlerTexture.GetContent(www);
          

            Sprite sprite = null;
            sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);

            if (sprite != null)
            {
                image.sprite = sprite;
                done = true;
            }
        }
        // clean up resources used by web request 
        www.Dispose();
       
    }



}
