using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DoFade : MonoBehaviour
{
    private Image image;
    void Start()
    {
        image = GetComponent<Image>();        
    }

    // Update is called once per frame
    void Update()
    {
        image.DOFade(0,3f).From();
    }
}
