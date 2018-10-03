using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameboardTile : MonoBehaviour
{
    public SharedInt playerScore;
    public SharedInt sharedMinTileValue;
    public SharedInt sharedMaxTileValue;
    public SharedColor gameboardColor;
    public SharedColor scoredTileColor;

    public Image tileImage;
    public TMP_Text tileText;
    public BoxCollider2D tileCollider;

    public int constantValue;

    private int _value;
    private bool _awardedValue;

    public void Start( )
    {
        tileImage.color = gameboardColor.value;

        if(constantValue == -1)
        {
            tileImage.color = new Color( 0.4f, 0.4f, 0.4f, 1 );
            tileText.text = "";
        }
        else if(constantValue == 0)
        {
            _value = Random.Range( sharedMinTileValue.value, sharedMaxTileValue.value );
        }
        else
        {
            _value = constantValue;
        }

        if(_value == -1)
        {
            tileImage.color = new Color( 0.4f, 0.4f, 0.4f, 1 );
            tileText.text = "";
        }
        else
            tileText.SetText( _value.ToString( ) );
    }

    public void OnTriggerEnter2D( Collider2D collision )
    {
        if(_value <= 0)
            return;

        if(_awardedValue)
            return;

        if(collision.gameObject.tag == "Player")
        {
            playerScore.value = playerScore.value + _value;

            tileImage.DOColor( scoredTileColor.value, 0.25f ).SetEase( Ease.InOutCubic );

            _awardedValue = true;
        }

    }

    public int GetValue( ) { return _value; }
}
