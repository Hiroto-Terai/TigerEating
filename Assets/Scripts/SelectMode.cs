using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SelectMode : MonoBehaviour
{
  public string nextScene;
  public AudioClip ButtonSound;
  public void goToNextScene()
  {
    if (OptionManager.isSePlaying == true)
    {
      AudioSource.PlayClipAtPoint(ButtonSound, GameObject.Find("Main Camera").transform.position);
    }
    FadeManager.Instance.LoadScene(nextScene, 0.3f);
  }
}
