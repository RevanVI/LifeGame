using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GestureEvent: UnityEvent<InputController.GestureData>
{ }

public class KeyEvent: UnityEvent<KeyCode>
{ }

public class InputController : MonoBehaviour
{
    private static InputController _instance;
    public static InputController Instance
    {
        get
        {
            return _instance;
        }
    }

    public class GestureData
    {
        public int fingerId;
        public GestureType Type;
        public Vector2 startPosition;
        public Vector2 endPosition;
        public Vector2 deltaPosition;
        public float time;
    }

    public enum GestureType
    {
        None = 0,
        Tap = 1,
        Swipe = 2,
        Drag = 3,
    }

    [SerializeField]
    private bool _isMoving;
    private float minDistance = 50f;
    private float maxTime = 0.6f;

    private List<GestureData> _gestures;
    public int count = 0;

    public GestureEvent OnTap;
    public GestureEvent OnDrag;
    public GestureEvent OnSwipe;
    public KeyEvent OnEsc;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        OnTap = new GestureEvent();
        OnDrag = new GestureEvent();
        OnSwipe = new GestureEvent();
        OnEsc = new KeyEvent();
    }

    void Start()
    {
        _gestures = new List<GestureData>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnEsc?.Invoke(KeyCode.Escape);
        }

        if (Input.touchCount == 0)
            _isMoving = false;
        else
        {
            foreach (var touch in Input.touches)
            {
                //register new input
                if (touch.phase == TouchPhase.Began)
                {
                    //prevent UI taps
                    if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        continue;
                    //prevent taps while drag
                    if (_isMoving)
                        continue;
                    GestureData gestureData = new GestureData();
                    gestureData.fingerId = touch.fingerId;
                    gestureData.Type = GestureType.None;
                    gestureData.startPosition = touch.position;
                    gestureData.endPosition = touch.position;
                    gestureData.deltaPosition = Vector2.zero;
                    gestureData.time = 0;
                    _gestures.Add(gestureData);
                    ++count;
                }
                else //process existing input
                {
                    int index = FindTouchIndexById(touch.fingerId);
                    if (index == -1)
                        continue;
                    _gestures[index].endPosition = touch.position;
                    _gestures[index].deltaPosition = touch.deltaPosition;
                    _gestures[index].time += touch.deltaTime;

                    float distance = (_gestures[index].endPosition - _gestures[index].startPosition).magnitude;
                    if (touch.phase == TouchPhase.Moved)
                    {
                        if (distance > minDistance || _isMoving)
                        {
                            _isMoving = true;
                            if (_gestures[index].time > maxTime)
                            {
                                _gestures[index].Type = GestureType.Drag;
                                OnDrag.Invoke(_gestures[index]);
                            }
                        }
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        if (distance < minDistance && _gestures[index].time < maxTime)
                        {
                            OnTap.Invoke(_gestures[index]);
                        }
                        else if (distance > minDistance && _gestures[index].time < maxTime)
                        {
                            _isMoving = false;
                            OnSwipe.Invoke(_gestures[index]);
                        }
                        else if (distance > minDistance && _gestures[index].time > maxTime)
                        {
                            _isMoving = false;
                            OnDrag.Invoke(_gestures[index]);
                        }
                        _gestures.RemoveAt(index);
                        --count;
                    }
                } //end proccess existing input
            }
        }
    }

    public int FindTouchIndexById(int fingerId)
    {
        for (int i = 0; i < _gestures.Count; ++i)
        {
            if (_gestures[i].fingerId == fingerId)
                return i;
        }
        return -1;
    }
}
