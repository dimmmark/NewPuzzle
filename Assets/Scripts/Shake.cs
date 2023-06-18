using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOShakeScale(1,.3f,7,60);
    }
    
}
