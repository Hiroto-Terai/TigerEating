using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
  public Text ScoreText;
  public int Cnt = 0;

  // 爆弾のイメージ
  public Sprite BombImage;

  // 草のイメージ
  public Sprite GrassImage;

  // 食べ物オブジェクトリスト
  public List<GameObject> eatObjectList = new List<GameObject>();

  bool isCalledOnce;

  Animator anim;

  public AudioClip correctSound;

  public AudioClip errorSound;

  void Start()
  {
    // リストの子オブジェクトのスプライトにランダムに絵を設定
    for (int i = 0; i <= 4; i++)
    {
      GameObject childObjectStart = eatObjectList[i].transform.Find("Image" + i.ToString()).gameObject;
      Image[] childImageStart = childObjectStart.GetComponents<Image>();
      int rand = Random.Range(0, 2);
      if (rand == 0)
      {
        childImageStart[0].sprite = GrassImage;
      }
      if (rand == 1)
      {
        childImageStart[0].sprite = BombImage;
      }
    }
    isCalledOnce = false;

    anim = transform.Find("Tiger").gameObject.GetComponent<Animator>();
  }

  public void OnClick(string buttonColor)
  {
    // カウントダウン中
    if (GameObject.Find("CountText").GetComponent<Text>().text == "")
    {
      isCalledOnce = true;
    }
    else
    {
      return;
    }
    string leftSpriteName = eatObjectMove();
    ScoreCount(buttonColor, leftSpriteName);
  }

  public string eatObjectMove()
  {
    // 画像差し替える前の最左の画像を取得し、返り値として持っておく
    GameObject childObjectLeft = eatObjectList[0].transform.Find("Image0").gameObject;
    Image[] childImageLeft = childObjectLeft.GetComponents<Image>();
    string leftSpriteName = childImageLeft[0].sprite.name;

    // 右手前の画像を取得し、左の画像に差し込む
    // 最右の画像以外の画像差し込み処理
    for (int i = 0; i <= 3; i++)
    {
      GameObject childObject = eatObjectList[i].transform.Find("Image" + i.ToString()).gameObject;
      Image[] childImage = childObject.GetComponents<Image>();
      GameObject childObjectFrom = eatObjectList[i + 1].transform.Find("Image" + (i + 1).ToString()).gameObject;
      Image[] childImageFrom = childObjectFrom.GetComponents<Image>();

      // 残像処理
      // 残像用オブジェクトに子オブジェクトの画像をセット
      GameObject childAfterObject = eatObjectList[i + 1].transform.Find("AfterImage" + (i + 1).ToString()).gameObject;
      Image[] childAfterImage = childAfterObject.GetComponents<Image>();
      childAfterImage[0].sprite = childImageFrom[0].sprite;

      // 残像オブジェクトアクティブ化、子オブジェクト非アクティブ化
      childAfterObject.SetActive(true);
      childObject.SetActive(false);

      // 画像差し替え
      childImage[0].sprite = childImageFrom[0].sprite;

      // 残像オブジェクト非アクティブ化、子オブジェクトアクティブ化(時間差使用)
      StartCoroutine(DelayMethod(0.1f, () =>
      {
        childAfterObject.SetActive(false);
        childObject.SetActive(true);

      }));
    }

    // 最右の画像差し込み処理。ランダムに入れる
    // 今は2つしか絵がないので0,1だけ
    GameObject childObjectRight = eatObjectList[4].transform.Find("Image4").gameObject;
    Image[] childImageRight = childObjectRight.GetComponents<Image>();

    // 残像オブジェクト取得
    GameObject childAfterObjectRight = eatObjectList[4].transform.Find("AfterImage4".ToString()).gameObject;
    Image[] childAfterImageRight = childAfterObjectRight.GetComponents<Image>();
    childAfterImageRight[0].sprite = childImageRight[0].sprite;

    //残像オブジェクトアクティブ化、子オブジェクト非アクティブ化
    childAfterObjectRight.SetActive(true);
    childObjectRight.SetActive(false);

    // 乱数出力
    int rand = Random.Range(0, 2);
    if (rand == 0)
    {
      childImageRight[0].sprite = GrassImage;

      // 残像オブジェクト非アクティブ化、子オブジェクトアクティブ化(時間差使用)
      StartCoroutine(DelayMethod(0.1f, () =>
      {
        childAfterObjectRight.SetActive(false);
        childObjectRight.SetActive(true);

      }));
    }
    if (rand == 1)
    {
      childImageRight[0].sprite = BombImage;

      // 残像オブジェクト非アクティブ化、子オブジェクトアクティブ化(時間差使用)
      StartCoroutine(DelayMethod(0.1f, () =>
      {
        childAfterObjectRight.SetActive(false);
        childObjectRight.SetActive(true);
      }));
    }

    anim.SetBool("isClick", true);
    StartCoroutine(DelayMethod(0.08f, () =>
      {
        anim.SetBool("isClick", false);
      }));
    return leftSpriteName;
  }

  public void ScoreCount(string buttonColor, string leftSpriteName)
  {
    switch (buttonColor)
    {
      // 赤いボタンを押した時
      // 最左のオブジェクトが爆弾だったら1加算
      // 最左のオブジェクトが草だったら3点マイナス
      case "red":
        {
          if (leftSpriteName == "bomb")
          {
            AudioSource.PlayClipAtPoint(correctSound, GameObject.Find("Main Camera").transform.position);
            Cnt++;
            ScoreText.text = "スコア: " + Cnt;
          }
          else
          {
            AudioSource.PlayClipAtPoint(errorSound, GameObject.Find("Main Camera").transform.position); Cnt -= 3;
            ScoreText.text = "スコア: " + Cnt;
          }
          break;
        }
      // 緑のボタンを押した時
      // 最左のオブジェクトが草だったら1加算
      // 最左のオブジェクトが爆弾だったら3点マイナス
      case "green":
        {
          if (leftSpriteName == "grass")
          {
            AudioSource.PlayClipAtPoint(correctSound, GameObject.Find("Main Camera").transform.position); Cnt++;
            ScoreText.text = "スコア: " + Cnt;
          }
          else
          {
            AudioSource.PlayClipAtPoint(errorSound, GameObject.Find("Main Camera").transform.position); Cnt -= 3;
            Cnt -= 3;
            ScoreText.text = "スコア: " + Cnt;
          }
          break;
        }
      default:
        break;
    }
  }

  private IEnumerator DelayMethod(float waitTime, Action action)
  {
    yield return new WaitForSeconds(waitTime);
    action();
  }

  private void moveObject(GameObject childObject)
  {
    childObject.transform.position = new Vector2(-10f, 0f);
  }
}
