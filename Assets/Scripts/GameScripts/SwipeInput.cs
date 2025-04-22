// using UnityEngine;

// public class SwipeInput : MonoBehaviour
// {
//     private Vector2 startTouchPos;
//     private Vector2 endTouchPos;
//     public PlayerController player;

//     private float minSwipeDistance = 50f;

//     void Update()
//     {
//         if (Input.touchCount > 0)
//         {
//             Touch touch = Input.GetTouch(0);

//             switch (touch.phase)
//             {
//                 case TouchPhase.Began:
//                     startTouchPos = touch.position;
//                     break;

//                 case TouchPhase.Ended:
//                     endTouchPos = touch.position;
//                     Vector2 swipe = endTouchPos - startTouchPos;

//                     if (swipe.magnitude > minSwipeDistance)
//                     {
//                         if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
//                         {
//                             if (swipe.x > 0)
//                                 player.OnSwipe(Vector3.right);  
//                             else
//                                 player.OnSwipe(Vector3.left);  
//                         }
//                         else
//                         {
//                             if (swipe.y > 0)
//                                 player.OnSwipe(Vector3.forward); 
//                             else
//                                 player.OnSwipe(Vector3.back);   
//                         }
//                     }
//                     break;
//             }
//         }
//     }
// }
