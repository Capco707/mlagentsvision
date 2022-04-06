using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class GetPerception : MonoBehaviour
{
    private CameraSensorComponent[] testSensors;

    private enum cameraRecord
    {
        img = 0,
        id = 1,
        layer = 2,
        depth = 3,
        tag = 6,
    }

    public Camera mainCamera;
    private GetRenderTexture RenderTextureScripts;
    // Start is called before the first frame update
    void Start()
    {
        
        RenderTextureScripts = mainCamera.GetComponent<GetRenderTexture>();
        testSensors = gameObject.GetComponents<CameraSensorComponent>();

        testSensors[0].Camera = RenderTextureScripts.capturePasses[(int)cameraRecord.img].camera;
        testSensors[1].Camera = RenderTextureScripts.capturePasses[(int)cameraRecord.id].camera;
        testSensors[2].Camera = RenderTextureScripts.capturePasses[(int)cameraRecord.depth].camera;
        

    }

    // Update is called once per frame
    void Update()
    {
        for (int count = 0; count < testSensors.Length; count ++)
        {
            //Debug.Log(testSensors[count].Camera);
        }
    }
}
