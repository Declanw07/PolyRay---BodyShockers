using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInput : MonoBehaviour {

    public LayerMask touchInputMask;

    private List<GameObject> touchList = new List<GameObject>();
    private GameObject[] touchesOld;

    private RaycastHit hit;

    public float orthoZoomSpeed = 0.5f;
    public float perspectiveZoomSpeed = 0.5f;
    public float camMoveSpeed;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

#if UNITY_EDITOR

        //
        //
        //  MOUSE INPUT CODE
        //
        //

        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
        {

            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();


                Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, touchInputMask))
                {

                    GameObject objectTouched = hit.transform.gameObject;

                    touchList.Add(objectTouched);

                    if (Input.GetMouseButtonDown(0))
                    {
                        objectTouched.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
                        objectTouched.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (Input.GetMouseButton(0))
                    {
                        objectTouched.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                    }

                }

            foreach (GameObject obj in touchesOld)
            {

                if (!touchList.Contains(obj))
                {

                    obj.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);

                }

            }

        }



#endif

        //
        //
        //  TOUCH INPUT CODE
        //
        //

        if (Input.touchCount > 0)
        {

            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();

            foreach (Touch touch in Input.touches)
            {

                Ray ray = GetComponent<Camera>().ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit, touchInputMask))
                {

                    //objectTouched is the object the user has touched.
                    GameObject objectTouched = hit.transform.gameObject;

                    //Adding the touched object to a list of objects touched.
                    touchList.Add(objectTouched);

                    //Different TouchPhase events, each TouchPhase represents a different phase of a touch, for example Began is when the touch has initiated.

                    //Event sent out "OnTouchDown" when the user begins the touch/is in touchphase.Began
                    if (touch.phase == TouchPhase.Began)
                    {
                        objectTouched.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        objectTouched.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Stationary)
                    {
                        objectTouched.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Moved)
                    {
                        objectTouched.SendMessage("OnTouchMove", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Canceled)
                    {
                        objectTouched.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                    }


                }


            }

            //Checking the list of touches from last frame.
            foreach(GameObject obj in touchesOld)
            {
                //If the list of touches from last frame is missing a particular touch invoke event "OnTouchExit" passing the touch position.
                if (!touchList.Contains(obj))
                {

                    obj.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);

                }

            }

        }

        //
        //
        //  CAMERA CODE
        //
        //


        //Camera movement code, executed if there is a touch happening and also if the touch was moved from last frame.
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {

            //touchDeltaPosition is the change in position from the initial touch from the previous frame.
            //Then translate the camera position using the deltaPosition of the touch from the previous frame.
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * camMoveSpeed * Time.deltaTime, 0, -touchDeltaPosition.y * camMoveSpeed * Time.deltaTime);

        }

        //Pinch-to-Zoom code, executed if there exactly 2 touches.
        if (Input.touchCount == 2)
        {

            //Create temporary holders for both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            //Create vector for each touch, this is equal to their previous/initial position.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            //Create floats for deltaMagnitudes equal to previous touches and current touches.
            float prevTouchDeltaMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMagnitude = (touchZero.position - touchOne.position).magnitude;

            //Find the difference in deltaMagnitudes.
            float deltaMagnitudeDifferential = prevTouchDeltaMagnitude - touchDeltaMagnitude;

            //Code for Orthographic camera pinch-to-zoom solution.
            if (GetComponent<Camera>().orthographic)
            {

                GetComponent<Camera>().orthographicSize += deltaMagnitudeDifferential * orthoZoomSpeed;

                GetComponent<Camera>().orthographicSize = Mathf.Max(GetComponent<Camera>().orthographicSize, 0.1f);

            }

            //Code for Perspective camera pinch-to-zoom solution.
            else
            {

                GetComponent<Camera>().fieldOfView += deltaMagnitudeDifferential * perspectiveZoomSpeed;

                GetComponent<Camera>().fieldOfView = Mathf.Clamp(GetComponent<Camera>().fieldOfView, 0.1f, 179.9f);
            }

        }

        //Lock camera Y position, has to be done in code due to nature of camera not having rigidbody.
        Vector3 cameraPos = transform.position;
        cameraPos.y = 70.0f;
        transform.position = cameraPos;

    }
}
