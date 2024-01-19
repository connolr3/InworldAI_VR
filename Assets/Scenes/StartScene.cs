using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartScene : MonoBehaviour
{
    [SerializeField] int nextSceneIndex;

    public void startScene(){
        SceneManager.LoadScene(nextSceneIndex);
    }

}
