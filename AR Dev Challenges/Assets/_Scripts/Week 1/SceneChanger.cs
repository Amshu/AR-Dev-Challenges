using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneChanger : MonoBehaviour
{
    // check if scene is in the build settings. Return the index if so, otherwise return -1
    static int findSceneNameInBuild(string name)
    {
        for(int i = 0; i <SceneManager.sceneCountInBuildSettings; i++)
        {
            if (Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)).Equals(name))
            {
                return i;
            }
        }
        return -1;
    }


    public static bool changeScene(string destSceneName)
    {
        if(findSceneNameInBuild(destSceneName) < 0)
        {
            return false;
        }
        SceneManager.LoadScene(destSceneName);
        return true;
    }
}
