﻿using System.Collections;
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
        Remove = 2,
    }

    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            return _instance;
        }
    }

    public EGameMode GameMode;
    public ETapMode TapMode;
    private bool _nextStepRequested;
    private bool _isPaused;
    public bool Paused
    {
        get
        {
            return _isPaused;
        }
        set
        {
            _isPaused = value;
        }
    }

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
    [SerializeField]
    private BoxCollider2D _mapBounds;

    public NodeInfoPanel NodeInfo;
    public PowerBar PowerBarRef;

    [SerializeField]
    private float _stepTime = 1f;

    [SerializeField]
    private int _maxPower;
    public int MaxPower
    {
        get
        {
            return _maxPower;
        }
    }

    private int _power;
    public int Power
    {
        get
        {
            return _power;
        }
    }

    public UnityEvent OnStep;
    public UnityEvent OnSpawn;
    public UnityEvent OnRemove;
    
    //debug
    public Text touchText;
    //---------------

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
        OnSpawn = new UnityEvent();
        OnRemove = new UnityEvent();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerDataController.Initialize();

        Years = 0;
        LifeHandlerRef.Initialize();
        OnStep = new UnityEvent();

        GameUIRef.OnModeToggleClick.AddListener(ChangeGameMode);
        GameUIRef.OnStepButtonClick.AddListener(RequestNextStep);
        InputControllerRef.OnTap.AddListener(ProcessTap);
        InputControllerRef.OnSwipe.AddListener(ProcessSwipe);
        InputControllerRef.OnDrag.AddListener(ProcessDrag);
        NodeInfo.OnButtonsClick.AddListener(ProcessNodeInfoPanelButtonClick);
        NodeInfo.gameObject.SetActive(false);
        PowerBarRef.Initialize(_maxPower);
        OnStep.AddListener(NodeInfo.UpdateInfo);
        ControlPanelRef.OnSpawnButtonClick.AddListener(ProcessSpawnModeButtonClick);
        ControlPanelRef.OnRemoveButtonClick.AddListener(ProcessRemoveModeButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFirstUpdate)
        {
            StartCoroutine(GameLoop());
            _isFirstUpdate = false;
        }
    }

    public IEnumerator GameLoop()
    {
        int counter = 0;
        while (true)
        {
            if ((GameMode == EGameMode.Auto || _nextStepRequested) && !_isPaused)
            {
                _nextStepRequested = false;
                CalculateStep();
                yield return null;
                ++counter;
                if (counter == 2)
                {
                    counter = 0;
                    ChangePower(1);
                }
                ProcessStep();
                OnStep.Invoke();
                ++Years;
                yield return new WaitForSeconds(_stepTime);
            }
            yield return null;
        }
    }

    public void CalculateStep()
    {
        foreach (Vector3Int pos in GridSystem.Instance.MainTilemap.cellBounds.allPositionsWithin)
        {
            LifeNode currentNode = GridSystem.Instance.GetNode(pos) as LifeNode;
            if (currentNode is null || currentNode.Status == LifeNode.NodeStatus.Blocked)
                continue;
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
            if (currentNode is null)
                continue;
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
                life.Die();
                LifeHandlerRef.ReleaseLife(life);
            }
            else if (currentNode.Status == LifeNode.NodeStatus.Occupied && currentNode.NewStatus == LifeNode.NodeStatus.Occupied)
            {
                LifeDot life = currentNode.ObjectsOnTile[0].GetComponent<LifeDot>();
                life.Grow();
            }
        }
        GameUIRef.UpdateYearsText(Years);
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

    public void ChangeGameMode(bool isManual)
    {
        if (GameMode == EGameMode.Auto)
            GameMode = EGameMode.Manual;
        else
        {
            GameMode = EGameMode.Auto;
            if (TapMode != ETapMode.Normal)
            {
                ControlPanelRef.ClearToggles();
            }
        }
        GameUIRef.UpdateMode();
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
            LifeNode node = GridSystem.Instance.GetNode(GridSystem.Instance.GetTilemapCoordsFromScreen(gestureData.endPosition)) as LifeNode;
            if (node != null)
            {
                if (TapMode == ETapMode.Spawn)
                {
                    SpawnLife(node);
                }
                else if (TapMode == ETapMode.Remove)
                {
                    RemoveLife(node);
                }
                else
                {
                    NodeInfo.gameObject.SetActive(false);
                    NodeInfo.transform.position = gestureData.endPosition;
                    NodeInfo.SetNode(node);
                    NodeInfo.gameObject.SetActive(true);
                }
            }
        }
    }

    protected void ProcessDrag(InputController.GestureData gestureData)
    {
        Vector3 lastPosition = MainCamera.ScreenToWorldPoint(new Vector3(gestureData.endPosition.x - gestureData.deltaPosition.x, 
                                                                          gestureData.endPosition.y - gestureData.deltaPosition.y, 
                                                                          MainCamera.nearClipPlane));
        Vector3 endPos = MainCamera.ScreenToWorldPoint(new Vector3(gestureData.endPosition.x, gestureData.endPosition.y, MainCamera.nearClipPlane));
        MainCamera.transform.position = CheckMapBounds(MainCamera.transform.position + (lastPosition - endPos));
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
                break;
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
            MainCamera.transform.position = CheckMapBounds(MainCamera.transform.position + direction * speed * Time.deltaTime);
            yield return null;
        }
        _isCameraMoving = false;
    }

    private Vector3 CheckMapBounds(Vector3 newPos)
    {
        newPos.x = Mathf.Clamp(newPos.x, _mapBounds.bounds.min.x + MainCamera.orthographicSize * MainCamera.aspect, _mapBounds.bounds.max.x - MainCamera.orthographicSize * MainCamera.aspect);
        newPos.y = Mathf.Clamp(newPos.y, _mapBounds.bounds.min.y + MainCamera.orthographicSize, _mapBounds.bounds.max.y - MainCamera.orthographicSize);
        return newPos;
    }

    public void ProcessSpawnModeButtonClick(bool status)
    {
        if (status == true)
        {
            GameMode = EGameMode.Manual;
            GameUIRef.UpdateMode(true);
            TapMode = ETapMode.Spawn;
        }
        else
        {
            TapMode = ETapMode.Normal;
        }
    }

    public void ProcessRemoveModeButtonClick(bool status)
    {
        if (status == true)
        {
            GameMode = EGameMode.Manual;
            GameUIRef.UpdateMode(true);
            TapMode = ETapMode.Remove;
        }
        else
        {
            TapMode = ETapMode.Normal;
        }
    }

    public void ProcessNodeInfoPanelButtonClick(int buttonNo)
    {
        if (buttonNo == 0)
            NodeInfo.Close();
        else
        {
            GameMode = EGameMode.Manual;
            GameUIRef.UpdateMode(true);
            if (buttonNo == 1)
                SpawnLife(NodeInfo.NodeRef);
            else
                RemoveLife(NodeInfo.NodeRef);
        }
        NodeInfo.UpdateInfo();
    }

    public bool ChangePower(int value)
    {
        int count = 0;
        if (_power + value > _maxPower)
        {
            count = _maxPower - _power;
            _power = _maxPower;
        }
        else if (_power + value >= 0)
        {
            count = value;
            _power += value;
        }

        if (count > 0)
            PowerBarRef.AddPower(count);
        else
            PowerBarRef.RemovePower(-count);
        return (count != 0);
    }

    protected void SpawnLife(LifeNode node)
    {
        if (node.GetLife() != null)
            return;
        if (ChangePower(-1))
        {
            LifeDot life = LifeHandlerRef.TakeLife();
            life.transform.position = GridSystem.Instance.GetWorldCoordsFromTilemap(node.Coords);
            node.AddLife(life);
            life.gameObject.SetActive(true);
            OnSpawn.Invoke();
        }
    }

    protected void RemoveLife(LifeNode node)
    {
        if (node.GetLife() == null)
            return;
        if (ChangePower(-1))
        {
            LifeDot lifeDot = node.RemoveLife();
            LifeHandlerRef.ReleaseLife(lifeDot);
            OnRemove.Invoke();
        }
    }
}

