using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPad : MonoBehaviour
{
    private RectTransform _touchPad;
    private int _touchId = -1;
    private Vector3 _startPos = Vector3.zero;
    public float _dragRadius = 60f;
    //방향 컨트롤러가 원으로 움직이는 반지름 입니다.
    public PlayerMovement _palyer;
    private bool _buttonPressed = false;
	void Start ()
    {
        _touchPad = GetComponent<RectTransform>();
        _startPos = _touchPad.position;
	}
	public void ButtonDown()
    {
        _buttonPressed = true;
    }
    public void ButtonUp()
    {
        _buttonPressed = false;
        HandleInput(_startPos);
    }
	void FixedUpdate ()
    {
       
        
            HandleTouchInput();

   #if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_WEBPLAYER

        HandleInput(Input.mousePosition);
   #endif

    }
    void HandleTouchInput()
    {
        int i = 0;
        if(Input.touchCount> 0)
        {
            foreach(Touch touch in Input.touches)
            {
                i++;
                Vector3 touchPos = new Vector3(touch.position.x, touch.position.y);
                if(touch.phase == TouchPhase.Began)
                {
                    if(touch.position.x <= (_startPos.x +_dragRadius))
                    {
                        _touchId = i;

                    }
                }
                if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if(_touchId == i)
                    {
                        HandleInput(touchPos);
                    }

                }
                if(touch.phase ==TouchPhase.Ended)
                {
                    if(_touchId ==i)
                    {
                        _touchId = -1;
                    }

                }


            }
        }

    }
    void HandleInput(Vector3 input)
    {
        if(_buttonPressed)
        {
            Vector3 diffVector = (input - _startPos);
            if (diffVector.sqrMagnitude > _dragRadius * _dragRadius)
            {
                diffVector.Normalize();
                _touchPad.position = _startPos + diffVector * _dragRadius;
            }
            else
            {
                _touchPad.position = input;
            }


        }
        else
        {
            _touchPad.position = _startPos;
        }
        //방향키와 기준지점의 차이를 구합니다.
        Vector3 diff = _touchPad.position - _startPos;
        //방향키의 방향을 유지한채로 거리를 나누어 방향만 구합니다.
        Vector2 normDiff = new Vector3(diff.x / _dragRadius, diff.y / _dragRadius);
        if(_palyer != null)
        {
            _palyer.OnStickChanged(normDiff);
        }
    }
}
