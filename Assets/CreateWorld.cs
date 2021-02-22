using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;
using UnityEditor;
using System.IO;
using UnityEngine.Android;
using AndroidNativeCore;
using System.Linq;
//using System;

public class CreateWorld : MonoBehaviour
{
    // Start is called before the first frame update
   // GameObject RectHori = GameObject.Find("Box");
    public UnityEngine.Video.VideoClip videoClip;
    public VideoPlayer videoPlayer;
    private GameObject ecran;
    private GameObject rectangleReferenceHorizontal1;
    private GameObject rectangleReferenceHorizontal2;
    private GameObject rectangleReferenceVertical;
    private List<string> pathList;
    private int ligne;
    private float intervalle;
    private int index;
    private int currentIndexVideo = 0;
    private int countVideo = 0;
    private bool stopCreate = false;
    
    void Start()
    {

        string directory;
        directory = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        //directory = Application.persistentDataPath;
        Debug.Log("Path : "+ directory);

        //Toast.make(Application.persistentDataPath, Toast.LENGTH_LONG);
        
        DirectoryInfo dir = new DirectoryInfo(directory);
        FileInfo[] info = dir.GetFiles("*.mp4", SearchOption.AllDirectories);
        // Store Path in a List
        pathList = new List<string>();
        foreach (FileInfo f in info)
        {
            Debug.Log("Video file found : " + f.DirectoryName + "/" + f.Name);
            pathList.Add(f.DirectoryName +"/"+f.Name);
        }
        //Toast.make(pathList.Count.ToString(), Toast.LENGTH_LONG);
        pathList = Shuffle(pathList);
        //Debug.Log("Path List 0 : " + pathList[0]);
        Debug.Log("compteur : " + index + " count : " + pathList.Count);

        rectangleReferenceHorizontal1 = GameObject.Find("HBox1");
        rectangleReferenceHorizontal2 = GameObject.Find("HBox2");
        rectangleReferenceVertical = GameObject.Find("VBox");
        ligne = 0;
        intervalle = 0.3f;
        index = 0;

        createVideo();

    }
    public List<string> Shuffle(List<string> items)
    {
        return items.Distinct().OrderBy(x => System.Guid.NewGuid().ToString()).ToList();
    }
    void createVideo()
    {
        
        Random rd = new Random();
       
        var rotation = new Quaternion(0, 0, 180, 1);
        
        //for (var i=0; i<pathList.Count; i++)
        //for (var i = 0; i < 20; i++)
        
            if (ligne == 0)
            {
                ecran = Instantiate(rectangleReferenceHorizontal1, new Vector3(intervalle * 6, 4, -8), rectangleReferenceHorizontal1.transform.rotation);
                ligne = 1;
            }
            else
            {
                ecran = Instantiate(rectangleReferenceHorizontal2, new Vector3(intervalle * 6 + 5 , 4, -18), rectangleReferenceHorizontal2.transform.rotation);
                ligne = 0;
                intervalle++;
            }

            ecran.name = index.ToString();
            GameObject video = GameObject.Find(index.ToString());
            videoPlayer = video.AddComponent<UnityEngine.Video.VideoPlayer>();
            videoPlayer.playOnAwake = false;
            videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
            videoPlayer.targetCameraAlpha = 0.5F;
            // videoPlayer.url = "C:/Users/Wilson/Videos/M4H02726_converted.mp4";
            videoPlayer.url = pathList[index];
            //videoPlayer.frame = Random.Range(0, 10000);
            videoPlayer.isLooping = true;
            videoPlayer.loopPointReached += EndReached;
            videoPlayer.SetDirectAudioVolume(0, 0);
            videoPlayer.prepareCompleted += PrepareCompleted;
            videoPlayer.Prepare();

            //videoPlayer.Play();
        

    }
    void PrepareCompleted(VideoPlayer vp)
    {
        float videoWidth = vp.texture.width;
        float videoHeight = vp.texture.height;
        int videoFrameMax = System.Convert.ToInt32(vp.frameCount);
        
        if (videoWidth-videoHeight < 0)
            {
               // ecran = Instantiate(rectangleReferenceVertical, new Vector3(i * 11, 4, -8), rotation);
            }
        int randnum = Random.Range(0, videoFrameMax);
        vp.frame = randnum;
        
        if(index < pathList.Count-1 && index<8)
        {
            index++;
            createVideo();
        } else if (stopCreate == false)
        {
            index++;
           InvokeRepeating("controlVideo", 0, 0.3f);
        }
        else
        {
            index++;
        }
        // vp.Play();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public VideoPlayer findCloseVideos()
    {
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
        return vpClose;
    }

    public void controlVideo()
    {
        stopCreate = true;
        VideoPlayer vpClose = findCloseVideos();
        int vpCloseIndex = int.Parse(vpClose.name.ToString()); //get index of closest video
        Debug.Log("controle index video : " + vpCloseIndex);
        if(vpCloseIndex - 2 >= 0 && vpCloseIndex < pathList.Count)
        {
            if(currentIndexVideo != vpCloseIndex)
            {
                Debug.Log("Enter " + countVideo);
                deleteVideo(countVideo);
                createVideo();
                
                countVideo++;

                currentIndexVideo = vpCloseIndex;
            }   
            
        }
    }

    public void deleteVideo(int indexNumber)
    {
        GameObject video = GameObject.Find(indexNumber.ToString());
        Destroy(video);
    }

    public void onFocusVideo()
    {
        //videoPlayer.Pause();
    }

    public void onExitVideo()
    {
        //videoPlayer.Play();
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        vp.playbackSpeed = vp.playbackSpeed / 10.0F;
    }

}

