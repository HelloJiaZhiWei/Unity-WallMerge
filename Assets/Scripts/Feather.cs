using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Feather : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 localposition;
    public string sceneName;

    public AudioClip collectSound;//拾取声音
    public Text feathertext; //羽毛数
    public static int totalscore; //分数
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            totalscore++;
            feathertext.text=totalscore.ToString()+"/10";//修改分数显示
            AudioSource.PlayClipAtPoint(collectSound,transform.position);//播放拾取声音
            // transform.DOMoveY(transform.position.y+2,0.5f).SetEase(Ease.OutBack).OnComplete
            // transform.DOJump(new Vector3(transform.position.x,transform.position.y+1,transform.position.z),1,1,1)
            transform.DOMoveY(transform.position.y+2,0.5f).SetEase(Ease.OutBack).OnComplete
            (()=>{
                    Destroy(gameObject);
                    if(totalscore == 10)
                    {
                        SceneManager.LoadScene(sceneName);
                    }
            });//羽毛向上消失
        }        
    }
    
}
