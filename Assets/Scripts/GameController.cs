using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public enum EGameMode
    {
        Auto = 0,
        Manual = 1,
    }

    public enum ETapMode
    {
        Normal = 0,
        Spawn = 1,
    }

    public static GameController Instance;
    public EGameMode GameMode;
    public ETapMode TapMode;
    private bool _nextStepRequested;
    public LifeHandler LifeHandlerRef;
    public InputController InputControllerRef;
    public GameUI GameUIRef;
    public ControlPanel ControlPanelRef;
    public GameObject LifePrefab;
    public Camera MainCamera;
    public int Years;

    private bool _isFirstUpdate = true;
    private bool _isCameraMoving = false;
    private bool _stopCamera = false;
    [SerializeField]
    private float _basicCameraSpeed;
    [SerializeField]
    private AnimationCurve _cameraSpeedCurve;
    [SerializeField]
    private float _swipeCameraTime;

    public NodeInfoPanel NodeInfo;

    public UnityEvent OnStep;

    //debug
    public Text touchText;
    //---------------

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        Years = 0;
        LifeHandlerRef.Initialize();
        OnStep = new UnityEvent();

        GameUIRef.OnModeButtonClick.AddListener(ChangeGameMode);
        GameUIRef.OnStepButtonClick.AddListener(RequestNextStep);
        InputControllerRef.OnTap.AddListener(ProcessTap);
        InputControllerRef.OnSwipe.AddListener(ProcessSwipe);
        InputControllerRef.OnDrag.AddListener(ProcessDrag);
        NodeInfo.gameObject.SetActive(false);
        OnStep.AddListener(NodeInfo.UpdateInfo);

    }

    // Update is called once per frame
    void Update()
    {
        if (_isFirstUpdate)
        {
            StartCoroutine(GameLoop());
            _isFirstUpdate = false;
        }

        //input
        if (Input.touchCount > 0)
        {

        }
    }

    public IEnumerator GameLoop()
    {
        while (true)
        {
            if (GameMode == EGameMode.Auto || _nextStepRequested)
            {
                _nextStepRequested = false;
                CalculateStep();
                yield return null;
                ProcessStep();
                OnStep.Invoke();
                ++Years;
                yield return new WaitForSeconds(0.5f);
            }
            yield return null;
        }
    }

    public void CalculateStep()
    {
        foreach (Vector3Int pos in GridSystem.Instance.MainTilemap.cellBounds.allPositionsWithin)
        {
            LifeNode currentNode = GridSystem.Instance.GetNode(pos) as LifeNode;
            List<LifeNode> nearNodes = GridSystem.Instance.GetNearNodes(pos);
            int neigboursCount = GetNeighboursCount(nearNodes);

            if (neigboursCount == 3 ||
                (currentNode.Status == LifeNode.NodeStatus.Occupied && neigboursCount == 2))
                currentNode.NewStatus = LifeNode.NodeStatus.Occupied;
            else
                currentNode.NewStatus = LifeNode.NodeStatus.Empty;
        }
    }

    public void ProcessStep()
    {
        foreach (Vector3Int pos in GridSystem.Instance.MainTilemap.cellBounds.allPositionsWithin)
        {
            LifeNode currentNode = GridSystem.Instance.GetNode(pos) as LifeNode;
            if (currentNode.Status == LifeNode.NodeStatus.Empty && currentNode.NewStatus == LifeNode.NodeStatus.Occupied)
            {
                LifeDot life = LifeHandlerRef.TakeLife();
                life.transform.position = GridSystem.Instance.GetWorldCoordsFromTilemap(pos);
                currentNode.AddLife(life);
                life.gameObject.SetActive(true);
            }
            else if (currentNode.Status == LifeNode.NodeStatus.Occupied && currentNode.NewStatus == LifeNode.NodeStatus.Empty)
            {
                LifeDot life = currentNode.RemoveLife();
                LifeHandlerRef.ReleaseLife(life);
            }
            else if (currentNode.Status == LifeNode.NodeStatus.Occupied && currentNode.NewStatus == LifeNode.NodeStatus.Occupied)
            {
                LifeDot life = currentNode.ObjectsOnTile[0].GetComponent<LifeDot>();
                ++life.Age;
            }
        }
    }

    private int GetNeighboursCount(List<LifeNode> nearNodes)
    {
        int count = 0;
        foreach (var node in nearNodes)
        {
            if (node.Status == LifeNode.NodeStatus.Occupied)
                ++count;
        }
        return count;
    }

    public void RegisterLife(LifeDot life)
    {
        GridSystem.Instance.AddLifeToGraph(life);
    }

    public void ChangeGameMode()
    {
        if (GameMode == EGameMode.Auto)
            GameMode = EGameMode.Manual;
        else
            GameMode = EGameMode.Auto;
        GameUIRef.ChangeMode();

    }

    public void RequestNextStep()
    {
        _nextStepRequested = true;
    }

    //input processing
    protected void ProcessTap(InputController.GestureData gestureData)
    {
        touchText.text = $"Tap. ID: {gestureData.fingerId}, Position: {gestureData.endPosition}, Time: {gestureData.time}";
        if (_isCameraMoving)
        {
            _stopCamera = true;
        }
        else
        {
            LifeNode buf = GridSystem.Instance.GetNode(GridSystem.Instance.GetTilemapCoordsFromScreen(gestureData.endPosition)) as LifeNode;
            if (buf != null)
            {
                NodeInfo.gameObject.SetActive(false);
                NodeInfo.transform.position = gestureData.endPosition;
                NodeInfo.SetNode(buf);
                NodeInfo.gameObject.SetActive(true);
            }
        }
    }

    protected void ProcessDrag(InputController.GestureData gestureData)
    {
        Vector3 lastPosition = MainCamera.ScreenToWorldPoint(new Vector3(gestureData.endPosition.x - gestureData.deltaPosition.x, 
                                                                          gestureData.endPosition.y - gestureData.deltaPosition.y, 
                                                                          MainCamera.nearClipPlane));
        Vector3 endPos = MainCamera.ScreenToWorldPoint(new Vector3(gestureData.endPosition.x, gestureData.endPosition.y, MainCamera.nearClipPlane));

        MainCamera.transform.position += (lastPosition - endPos);
        touchText.text = $"Drag. ID: {gestureData.fingerId}, Position: {gestureData.endPosition}, Time: {gestureData.time}\n" +
            $"LastWorldPosition: {lastPosition}, EndWorldPosition: {endPos}, CameraPosition: {MainCamera.transform.position}";
    }

    protected void ProcessSwipe(InputController.GestureData gestureData)
    {
        touchText.text = $"Swipe. ID: {gestureData.fingerId}, Position: {gestureData.endPosition}, Time: {gestureData.time}";
        Vector3 direction = new Vector3(gestureData.startPosition.x - gestureData.endPosition.x, gestureData.startPosition.y - gestureData.endPosition.y, 0).normalized;
        _isCameraMoving = true;
        StartCoroutine(CameraSwipe(direction));
    }

    protected IEnumerator CameraSwipe(Vector3 direction)
    {
        float curTime = 0;
        bool end = false;
        while (!end)
        {
            if (_stopCamera)
            {
                _stopCamera = false;
                yield break;
            }
            curTime += Time.deltaTime;
            if (curTime >= _swipeCameraTime)
            {
                curTime = _swipeCameraTime;
                end = true;
            }
            float timeValue = curTime / _swipeCameraTime;
            float speedModif = _cameraSpeedCurve.Evaluate(timeValue);
            float speed = _basicCameraSpeed * speedModif;
            MainCamera.transform.position += direction * speed * Time.deltaTime;
            yield return null;
        }
        _isCameraMoving = false;
    }

    public void ProcessSpawnModeButtonClick(bool status)
    {
        if (status == true)
        {
            GameMode = EGameMode.Manual;
            GameUIRef.ChangeMode();
            TapMode = ETapMode.Spawn;
        }

    }
}

