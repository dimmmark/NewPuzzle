using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Management : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private Block _selectedBlock;
    private Ray rayCol;
    private Plane basePlane = new Plane(Vector3.up, Vector3.zero);
    public Dictionary<Vector2Int, Platform> PlatformsDictionary = new Dictionary<Vector2Int, Platform>();
    public Dictionary<Vector2Int, Block> BlocksDictionary = new Dictionary<Vector2Int, Block>();
    private int x;
    private int y;
    public Platform[] Platforms;
    public Block[] Blocks;
    Platform platform;
    private Vector3 _startPosition;
    [SerializeField] private GameObject _winScreen;
    private Vector3 _offset;
    private bool _isWin;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private ParticleSystem[] _particleSystems;
    private float _clickDalay = .6f;
    private float _timer;
    private bool _isShadow;
    [SerializeField] private List<GameObject> _shadows = new List<GameObject>();
    Vector2Int oldPositionShadow = new Vector2Int(0, 0);
    bool firstCompare = true;
    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public void NextLevel()
    {
        DOTween.KillAll();
        int levels = SceneManager.sceneCountInBuildSettings - 1;
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevel > levels)
            SceneManager.LoadScene(1);
        else
            SceneManager.LoadScene(nextLevel);
    }

    void Update()
    {
        rayCol = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        _timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && !_isWin && _clickDalay < _timer)
        {
            _timer = 0;
            if (Physics.Raycast(rayCol, out hit))
            {
                if (hit.collider.TryGetComponent<Block>(out Block block))
                {
                    _selectedBlock = block;
                    basePlane.Raycast(rayCol, out float position);
                    Vector3 worldPosition = rayCol.GetPoint(position);
                    _offset = _selectedBlock.transform.position - worldPosition;
                    block.transform.DOMoveY(.38f, .25f).SetRelative();
                    block.transform.DORotate(new Vector3(0, 0, 12f), .075f).SetLoops(2, LoopType.Yoyo);
                    RemoveFromDictionary(block);
                    _startPosition = _selectedBlock.transform.position;
                    _soundManager.Play("Up");
                    for (int i = 0; i < Platforms.Length; i++)
                    {
                        for (int j = 0; j < Platforms[i].PlatformList.Count; j++)
                        {
                            if (Platforms[i].PlatformList[j] == block)
                            {
                                platform = Platforms[i];
                                break;
                            }
                        }
                    }
                    platform.PlatformList.RemoveAll(x => x == block);
                }
            }
        }
        if (_selectedBlock != null)
        {
            if (basePlane.Raycast(rayCol, out float position))
            {
                Vector3 worldPosition = rayCol.GetPoint(position);

                x = Mathf.RoundToInt(worldPosition.x + _offset.x);
                y = Mathf.RoundToInt(worldPosition.z + _offset.z);
                _selectedBlock.transform.position = new Vector3(worldPosition.x,
                    _selectedBlock.transform.position.y, worldPosition.z) + _offset;

                Vector2Int newPosition = new Vector2Int(x, y);
               // Vector2Int oldPositionShadow = new Vector2Int(0,0);
                //bool firstCompare = true;
                if (oldPositionShadow == newPosition || firstCompare)
                {

                oldPositionShadow = newPosition;
                    firstCompare = false;
                }
                else
                {
                    oldPositionShadow = newPosition;
                    RemoveShadow();
                }

            }
            if (CheckAllow(x, y, _selectedBlock))
            {
                _selectedBlock.SetColor(true);
                if (!_isShadow)
                {
                    ShowShadow(x, y, _selectedBlock);
                    _isShadow = true;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    _selectedBlock.transform.DOMove(new Vector3(x, 0, y), .3f);
                    _soundManager.Play("Down");
                    InstallBlock(x, y, _selectedBlock);
                    RemoveShadow();
                }
            }
            else
            {
                _selectedBlock.SetColor(false);

                RemoveShadow();


                if (Input.GetMouseButtonUp(0))
                {
                    _selectedBlock.transform.DOMove(_startPosition, .3f);
                    _selectedBlock.SetStartColor();
                    InstallBlock((int)_startPosition.x, (int)_startPosition.z, _selectedBlock);
                    _selectedBlock = null;
                    _soundManager.Play("Down");
                }
            }
        }
    }

    private bool CheckAllow(int xPosition, int zPosition, Block block)
    {
        for (int x = 0; x < block.BlockWidht; x++)
        {
            for (int z = 0; z < block.BlockHeight; z++)
            {
                if (block.Blocks[x, z])
                {
                    Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                    Vector3 shadowCoordinate = new Vector3(coordinate.x, .15f, coordinate.y);
                    if (PlatformsDictionary.ContainsKey(coordinate) && (!BlocksDictionary.ContainsKey(coordinate)))
                    {

                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private void ShowShadow(int xPosition, int zPosition, Block block)
    {

        for (int x = 0; x < block.BlockWidht; x++)
        {
            for (int z = 0; z < block.BlockHeight; z++)
            {
                if (block.Blocks[x, z])
                {
                    Vector3 shadowCoordinate = new Vector3(xPosition + x, .15f, zPosition + z);

                   GameObject shadow = Pooler.Instance.SpawnFromPool("Shadow", shadowCoordinate, Quaternion.identity);
                    _shadows.Add(shadow);
                }
            }
        }
    }
    private void RemoveShadow()
    {
        foreach (var shadow in _shadows)
        {
            shadow.SetActive(false);
            _isShadow = false;
        }
    }
    public void InstallPlatform(int xPosition, int zPosition, Platform platform)
    {
        for (int x = 0; x < platform.Widht; x++)
        {
            for (int z = 0; z < platform.Height; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                PlatformsDictionary.Add(coordinate, platform);
            }
        }
    }

    public void InstallBlock(int xPosition, int zPosition, Block block)
    {
        for (int x = 0; x < block.BlockWidht; x++)
        {
            for (int z = 0; z < block.BlockHeight; z++)
            {
                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
                if (block.Blocks[x, z])
                {
                    BlocksDictionary.Add(coordinate, block);
                    Platform platform = PlatformsDictionary[coordinate];
                    platform.PlatformList.Add(block);
                }
            }
        }

        _selectedBlock = null;


        CheckWin();
    }
    private bool CheckWin()
    {
        for (int i = 0; i < Platforms.Length; i++)
        {
            if (!Platforms[i].Check())
            {
                // Debug.Log("LOSE");
                return false;
            }
        }
        // Debug.Log("WIN");
        // _winScreen.SetActive(true);
        _isWin = true;
        Invoke(nameof(ShowWin), .25f);
        return true;
    }
    private void ShowWin()
    {
        // DOTween.KillAll();
        _winScreen.SetActive(true);
        MakeWinEffect();
        LevelIndex++;
    }
    private void MakeWinEffect()
    {
        foreach (var block in Blocks)
        {
            block.transform.DOPunchPosition(Vector3.up, 1f, 1, 1);
        }
        _soundManager.Play("Complete");
        foreach (var ps in _particleSystems)
        {
            ps.Play();
        }
    }
    private void RemoveFromDictionary(Block block)
    {
        List<Vector2Int> keysToRemove = new List<Vector2Int>();

        foreach (KeyValuePair<Vector2Int, Block> entry in BlocksDictionary)
        {
            if (entry.Value.Equals(block))
            {
                keysToRemove.Add(entry.Key);
            }
        }

        foreach (Vector2Int key in keysToRemove)
        {
            BlocksDictionary.Remove(key);
        }
    }
    public void Init(Platform[] platforms, Block[] blocks)
    {
        Platforms = platforms;
        Blocks = blocks;
    }

    public int LevelIndex
    {
        get => PlayerPrefs.GetInt("LevelIndex", 1);

        private set
        {
            PlayerPrefs.SetInt("LevelIndex", value);
            PlayerPrefs.Save();
        }

    }
}
