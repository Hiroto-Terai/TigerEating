using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
  // カウントダウンのテキスト
  public Text CountTextStart;
  public Text CountTextPlay;

  // カウント用の数値
  private float startCountDown;

  private float playCountDown;
  int countStart;
  int countPlay;
  bool isCalledOnceForStart;
  bool isCalledOnceForPlay;

  public GameObject LoadSceneCanvas;

  public AudioClip GameOverSound;

  bool isCalledOnce = false;

  public GameObject Music;

  bool isGameOver = false;

  void Start()
  {
    // update関数内で一回だけ呼び出すためのフラグを初期化
    isCalledOnceForStart = false;
    isCalledOnceForPlay = false;
    startCountDown = 4f;
    playCountDown = 31f;
  }

  // Update is called once per frame
  void Update()
  {
    // スタート3秒カウント
    if (startCountDown >= 1)
    {
      startCountDown -= Time.deltaTime;
      countStart = (int)startCountDown;
      CountTextStart.text = countStart.ToString();

      if (startCountDown <= 1 && !isCalledOnceForStart)
      {
        isCalledOnceForStart = true;
        CountTextStart.text = "";
        return;
      }
    }

    else if (playCountDown >= 0)
    {
      // プレイ60秒カウント
      playCountDown -= Time.deltaTime;
      countPlay = (int)playCountDown;
      CountTextPlay.text = "タイム: " + countPlay.ToString();
      if (playCountDown <= 0 && !isCalledOnceForPlay)
      {
        isCalledOnceForPlay = true;
        CountTextPlay.text = "GAME OVER";
        // BGMの停止
        Music.SetActive(false);
        // ゲームクリアのSEを鳴らす
        if (!isCalledOnceForPlay)
        {
          isCalledOnceForPlay = true;
          if (OptionManager.isSePlaying == true)
          {
            AudioSource.PlayClipAtPoint(GameOverSound, transform.position);
          }
        }
        isGameOver = true;
        if (isGameOver == true)
        {
          LoadSceneCanvas.SetActive(true);
        }
        return;
      }
    }
  }
}
