using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1. 본인 위치의 RectTransform 컴포넌트를 가져온 후 시작 위치를 저장.
// 2. 조이스틱 터치 반경의 반지름을 지정.
// 3. 패드 버튼을 눌렀는지 확인하는 변수를 선언.
// 4. 모바일 환경에서 조이스틱 터치 여부를 확인하는 변수를 선언하여 판단.

public class JoyStick_TouchPad : MonoBehaviour
{
    [SerializeField][Tooltip("JoyStick_TouchPad의 RectTransform 컴포넌트")]
    private RectTransform touchPad_Tr;
    [SerializeField][Tooltip("JoyStick_TouchPad의 시작 위치")]
    private Vector3 startPos;
    [SerializeField][Tooltip("조이스틱 터치 반경의 반지름")]
    private float dragRadius = 150.0f;
    [SerializeField][Tooltip("Player클래스 연결")]
    private Player player;
    private bool isTouch = false;
    private int touchID = -1;           // 터치 여부를 확인하는 변수
    public Vector3 differ;              // 시작 위치와 현재 위치의 차이. 실제 벡터 최종 값.
    public Vector3 normalDiffer;        // 정규화된 벡터 값.

    void Start()
    {
        touchPad_Tr = GetComponent<RectTransform>();
        startPos = touchPad_Tr.position;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void FixedUpdate()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            HandleTouchInput();
        }
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            HandleInput(Input.mousePosition);
        }
    }
    
    public void ButtonDown()    // 버튼을 눌렀을 때
    {
        isTouch = true;
    }
    public void ButtonUp()      // 버튼을 뗐을 때
    {
        isTouch = false;
        HandleInput(startPos);
        touchPad_Tr.position = startPos;
    }
    void HandleTouchInput()     // 모바일 환경에서 조이스틱 터치 여부를 확인
    {
        int i = 0;
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                i++;
                Vector3 touchPos = new Vector3(touch.position.x, touch.position.y);
                if (touch.phase == TouchPhase.Began)
                {
                    if (touch.position.x <= startPos.x + dragRadius && touch.position.y <= startPos.y + dragRadius &&
                        touch.position.x >= startPos.x - dragRadius && touch.position.y >= startPos.y - dragRadius)
                    {
                        touchID = i;    // 터치한 위치가 조이스틱 터치 반경 내에 있을 때
                    }
                }
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if (touchID == i)   // 터치 중일 때
                    {
                        HandleInput(touchPos);
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    if (touchID == i)
                    {
                        touchID = -1;
                    }
                }
            }
        }
    }
    void HandleInput(Vector3 input)     // 실제 조이스틱 터치 위치를 적용
    {
        if (isTouch)
        {
            Vector3 differVector = (input - startPos);
            if (differVector.sqrMagnitude < dragRadius * dragRadius)    // 조이스틱 터치 반경 내에 있을 때
            {
                touchPad_Tr.position = startPos + differVector;
            }
            else                                                        // 조이스틱 터치 반경 밖에 있을 때
            {
                differVector = differVector.normalized * dragRadius;
                touchPad_Tr.position = startPos + differVector;
            }
        }
        else
        {
            touchPad_Tr.position = startPos;
        }
        differ = touchPad_Tr.position - startPos;   // 시작 위치와 현재 위치의 차이. 실제 벡터 최종 값
        normalDiffer = new Vector3(differ.x / dragRadius, differ.y / dragRadius);   // 정규화된 벡터 값

        if (player != null)    // Player 클래스의 OnStickPos 함수에 정규화된 벡터 값을 전달
        {
            player.OnStickPos(normalDiffer);
        }
    }


}
