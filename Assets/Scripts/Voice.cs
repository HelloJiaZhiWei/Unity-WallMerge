using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voice : MonoBehaviour
{
    private bool left=true;
    // 计时器
    private float footstepsTimer = 0f;
    private float timeSet = 0.4f;
    // 播放间隔

    [Header("Footsteps Paramenters")]
    // 声源，哪个声源播放音频
    [SerializeField] private AudioSource footstepsAudioSource = default;
    // 不同地面类型，播放不同的音频列表

    void Update()
    {
        
        if ((Input.GetAxisRaw("Horizontal")!=0||Input.GetAxisRaw("Vertical")!=0)&&footstepsTimer>=timeSet)
        {
            if(left)
            {
                timeSet = 0.3f;
                footstepsAudioSource.pitch = 0.7f;
                left = !left;
            }else
            {
                timeSet = 0.4f;
                footstepsAudioSource.pitch = 0.9f;
                left = !left;
            }
            footstepsAudioSource.PlayOneShot(footstepsAudioSource.clip);
            footstepsTimer=0;
        }else
        {
            footstepsTimer+=Time.deltaTime;
        }
    }
    
}
