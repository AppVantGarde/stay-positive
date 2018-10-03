using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Perfect : MonoBehaviour
{
    public SpriteRenderer ringSpriteRenderer;

    public void OnTriggerEnter2D( Collider2D collision )
    {
        if(collision.gameObject.tag == "Player")
        {
            ringSpriteRenderer.transform.DOScale( 2.25f, 1.5f );
            ringSpriteRenderer.DOColor( new Color( 1, 1, 1, 0 ), 1.2f );
        }
    }
}
