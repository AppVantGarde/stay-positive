using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public SharedInt playerScore;

    public GameEvent levelCompletedEvent;
    public RectTransform lineStartRect;
    public Image flashImage;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer ringSpriteRenderer;
    public Sprite sadFaceSprite;
    public Sprite superHappySprite;
    public CircleCollider2D circleCollider;

    public Key key;
    public Target target;
    public GameObject door;

    private bool _hasKey;
    private int _prevScore;

    private AudioCue _winAudioCue;
    private AudioCue _loseAudioCue;

    public void Start( )
    {
        playerScore.value = 0;

        _prevScore = 0;
        Vector3 worldToViewPort = Camera.main.WorldToViewportPoint( transform.position );
        worldToViewPort.y -= 0.4625f;

        lineStartRect.anchorMin = new Vector2( worldToViewPort.x, worldToViewPort.y );
        lineStartRect.anchorMax = new Vector2( worldToViewPort.x, worldToViewPort.y );
        lineStartRect.anchoredPosition = Vector2.zero;
 
        lineStartRect.DOAnchorPos( Vector2.zero, 0.25f );

        transform.localScale = new Vector3( 0.28f, 0.28f, 1 );

        _winAudioCue = Resources.Load<AudioCue>( "Audio/Win" );
        _loseAudioCue = Resources.Load<AudioCue>( "Audio/Lose" );
    }

    public void Update( )
    {
        if(_prevScore != playerScore.value)
        {
            if(playerScore.value < 0)
            {
                spriteRenderer.color = Color.white;
                spriteRenderer.sprite = sadFaceSprite;

            }

            if(_prevScore <= 0 && playerScore.value > 0)
            {
                spriteRenderer.color = Color.yellow;
                spriteRenderer.sprite = superHappySprite;
                ringSpriteRenderer.transform.localScale = Vector3.zero;
                ringSpriteRenderer.color = Color.green;
                ringSpriteRenderer.transform.DOScale( 2, 0.5f );
                ringSpriteRenderer.DOColor( new Color( 0, 1, 0, 0 ), 0.5f ).SetDelay( 0.25f );
            }

            _prevScore = playerScore.value;
        }
    }

    public void OnTriggerEnter2D( Collider2D collision )
    {
        //if(collision.gameObject.tag == "Perfect")
        //{
        //    _perfectCount++;
        //}

        if(collision.gameObject.tag == "Target")
        {
            if(playerScore.value < 0)//!_hasKey)
            {
                _loseAudioCue.Play( );
                spriteRenderer.color = Color.white;
                spriteRenderer.sprite = sadFaceSprite;

                circleCollider.enabled = false;
                transform.DOKill( );
                transform.DOShakePosition( 0.5f, new Vector3( 0.1f, 0.1f, 0 ) );

                flashImage.DOColor( new Color( 1, 0, 0, 1 ), 0.05f ).SetEase( Ease.OutCubic );
                flashImage.DOColor( new Color( 1, 0, 0, 0 ), 0.15f ).SetEase( Ease.InOutCubic ).SetDelay( 0.05f );

                //DOVirtual.DelayedCall( 1f, ( ) =>
                //{
                //    SceneManager.LoadScene( SceneManager.GetActiveScene( ).buildIndex );
                //} );

                levelCompletedEvent.Trigger( );
                return;
            }

            _winAudioCue.Play( );
            iOSHapticFeedback.Instance.Trigger( iOSHapticFeedback.iOSFeedbackType.Success );

            spriteRenderer.color = Color.yellow;
            spriteRenderer.sprite = superHappySprite;

            transform.DOKill( );
            //transform.DOMove( collision.gameObject.transform.position, 0.3f );

           // key.transform.parent = target.transform;
            Sequence s = DOTween.Sequence( );
            //s.Append( key.transform.DOLocalMove( Vector3.zero, 0.25f ) );
            //s.Append( key.transform.DOShakePosition( 0.5f, new Vector3( 0.2f, 0.2f, 0 ) ) );
            s.AppendCallback( ( ) =>
            {
                //key.gameObject.SetActive( false );
                target.shadowSprite.gameObject.SetActive( false );
                target.transform.DOScale( target.transform.localScale * 1 * 2f, 0.25f );
                Color color = target.spriteRenderer.color;
                color.a = 0;
                target.spriteRenderer.DOColor( color, 0.2f );
            } );
            s.AppendInterval( 0.25f );
            s.Append( door.transform.DOScale( 1.5f, 0.25f ) );
            s.AppendCallback( ( ) =>
            {
                transform.parent = door.transform;
                transform.DOLocalMove( Vector3.zero, 0.25f );
                transform.DOScale( 0, 0.25f );
            } );
            s.AppendInterval( 0.25f );
            s.Append( door.transform.DOScale( 0, 0.25f ) );
            s.AppendCallback( ( ) =>
            {
                levelCompletedEvent.Trigger( );
            } );
 
        }

        if(collision.gameObject.tag == "Obstacle")
        {
            iOSHapticFeedback.Instance.Trigger( iOSHapticFeedback.iOSFeedbackType.ImpactHeavy );
            spriteRenderer.color = Color.white;
            spriteRenderer.sprite = sadFaceSprite;

            //circleCollider.enabled = false;
            //transform.DOKill( );
            //transform.DOShakePosition( 0.5f, new Vector3(0.1f, 0.1f, 0) );

            flashImage.DOColor( new Color( 1, 0, 0, 1 ), 0.05f ).SetEase( Ease.OutCubic );
            flashImage.DOColor( new Color( 1, 0, 0, 0 ), 0.15f ).SetEase( Ease.InOutCubic ).SetDelay( 0.05f );

            //DOVirtual.DelayedCall( 1f, ( ) =>
            //{
            //    SceneManager.LoadScene( SceneManager.GetActiveScene( ).buildIndex );
            //} );
        }

        //if(collision.gameObject.tag == "Key")
        //{
        //    _hasKey = true;
        //    key.circleCollider.enabled = false;
        //    key.transform.parent = transform;
        //    key.transform.DOLocalMove( new Vector3( 0.6f, 0.6f, 0 ), 0.3f );
        //    key.transform.DORotate( new Vector3( 0, 0, 45 ), 0.3f );
        //}
    }
}
