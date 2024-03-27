using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TMPscale : MonoBehaviour
{
    // Start is called before the first frame updatess
    private float  timer;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    
        transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 2f).SetDelay(0.5f).SetLoops(-1, LoopType.Restart);

            


    }
    private void heart()
    {
        transform.DOScale(new Vector3(1.4f, 1.4f, 1.4f), 2f).SetDelay(0.5f).SetLoops(-1, LoopType.Restart);
    }
}