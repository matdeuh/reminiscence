using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using AndroidNativeCore;


public class move : MonoBehaviour
{
    public float thrust = 2.0f;
    public VideoPlayer vp;
    public bool slow = true;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("speedToSlow", 0, 75f);
        InvokeRepeating("ActivateVideo", 0, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {


        //ActivateVideo();
        // Debug.Log("move : " + transform.position.x);
        if(slow == false)
        {
           // transform.position += new Vector3(transform.position.x * 2f * 0.05f * Time.deltaTime / 2, 0, 0);
        }
        else
        {
         //   transform.position += new Vector3(transform.position.x * 2f * 0.05f * Time.deltaTime / 2, 0, 0);
        }
        
        //transform.position += new Vector3(transform.position.x + 0.01f, 0, 0);
        transform.position += Camera.main.transform.forward * thrust * Time.deltaTime;


    }

    void speedToSlow()
    {
        if (slow == true)
        {
            slow = false;
        }
        else
        {
            slow = true;
        }
    }

    void ActivateVideo()
    {
        //Toast.make(transform.position.x.ToString(), Toast.LENGTH_LONG);
        float distanceToClosestVideo = Mathf.Infinity;
        VideoPlayer vpClose = null;
        VideoPlayer[] allVideo = GameObject.FindObjectsOfType<UnityEngine.Video.VideoPlayer>();
        //VideoPlayer[] videoToPlay = null;
        //Debug.Log(allVideo[1]);
        //Debug.Log("this position : " + this.transform.position);
        foreach (VideoPlayer currentVp in allVideo)
        {
            float distanceToVideo = (currentVp.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToVideo < distanceToClosestVideo)
            {
                distanceToClosestVideo = distanceToVideo;
                vpClose = currentVp;

            }
        }

        int vpCloseIndex = int.Parse(vpClose.name.ToString()); //get index of closest video
        int nbactivateVideos = 3;
        //Debug.Log("proche index : " + vpCloseIndex);
        //Debug.Log("Longeur tableau : " + allVideo.Length);
        for (var i = 0; i < allVideo.Length-1; i++)
        {
            if(int.Parse(allVideo[i].name.ToString()) > (vpCloseIndex - nbactivateVideos) && int.Parse(allVideo[i].name.ToString()) < (vpCloseIndex + nbactivateVideos))
            {
              //  if(allVideo[i].isPlaying == false)
               // {
                    allVideo[i].Play();
               // }
            }
            else
            {
                //if(allVideo[i].isPaused == false)
               // {
                    allVideo[i].Pause();
                //}
                
            }


        }
    }
}