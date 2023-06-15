using DG.Tweening;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Management : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private Block _selectedBlock;
    private Ray rayCol;
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
    private void Start()
    {
        _mainCamera = Camera.main;
      
    }

    public void NextLevel()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex+1;
        SceneManager.LoadScene(nextLevel);
    }

    void Update()
    {
        rayCol = _mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(rayCol, out hit))
            {
                if (hit.collider.TryGetComponent<Block>(out Block block))
                {
                    _selectedBlock = block;
                block.transform.DOMoveY(.38f, .4f);
                    RemoveFromDictionary(block);
                    _startPosition = _selectedBlock.transform.position;


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
            var basePlane = new Plane(Vector3.up, Vector3.zero);

            if (basePlane.Raycast(rayCol, out float position))
            {
                Vector3 worldPosition = rayCol.GetPoint(position);
                x = Mathf.RoundToInt(worldPosition.x);
                y = Mathf.RoundToInt(worldPosition.z);



                _selectedBlock.transform.position = new Vector3(worldPosition.x, _selectedBlock.transform.position.y, worldPosition.z);
            }

            if (CheckAllow(x, y, _selectedBlock))
            {
                _selectedBlock.SetColor(true);
                if (Input.GetMouseButtonUp(0))
                {
                   
                    _selectedBlock.transform.DOMove(new Vector3(x, 0, y), .3f);
                    InstallBlock(x, y, _selectedBlock);


                }
            }
            else
            {
                _selectedBlock.SetColor(false);
                if (Input.GetMouseButtonUp(0))
                {
                    // _selectedBlock.transform.position = _startPosition;
                    _selectedBlock.transform.DOMove(_startPosition, .3f);
                    _selectedBlock.SetStartColor();
                    InstallBlock((int)_startPosition.x, (int)_startPosition.z, _selectedBlock);
                     _selectedBlock = null;
                    
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
                Debug.Log("LOSE");
                return false;
            }
        }
        Debug.Log("WIN");
        _winScreen.SetActive(true);
        return true;
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

}
