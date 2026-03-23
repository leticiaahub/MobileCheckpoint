using UnityEngine;

public class PlayerController : MonoBehaviour  
{
    private Touch touch;
    private Vector2 touchStart;
    private Vector2 touchEnd;

    Player player;

    
    private float minSwipe = 50f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            SwipeTouch();
        }

        
        #if UNITY_EDITOR || UNITY_STANDALONE
        SwipeMouse();
        #endif
    }

    private void SwipeTouch()
    {
        touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            touchStart = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            touchEnd = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            Swipe();
        }
    }

    private void SwipeMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            touchEnd = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Swipe();
        }
    }

    private void Swipe()
    {
        Vector2 touchValue = (touchEnd - touchStart);

       
        if (touchValue.magnitude < minSwipe)
        {
            return;
        }

        if (Mathf.Abs(touchValue.x) > Mathf.Abs(touchValue.y))
        {
           
            if (touchStart.x < touchEnd.x)
            {
                player.ChangeLane(1);
            }
            else
            {
                player.ChangeLane(-1);
            }
        }
        else
        {
           
            if (touchStart.y < touchEnd.y)
            {
               
                player.Jump();
            }
            else
            {
                player.Caindo();
            }
        }
    }
}