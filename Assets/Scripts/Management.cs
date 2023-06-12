using System;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        Platforms = FindObjectsOfType<Platform>();
        _mainCamera = Camera.main;
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
                    RemoveFromDictionary(block);
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
                _selectedBlock.transform.position = new Vector3(x, 0, y);
            }

            if (CheckAllow(x, y, _selectedBlock))
            {
                _selectedBlock.SetColor(true);
                if (Input.GetMouseButtonUp(0))
                {
                    InstallBlock(x, y, _selectedBlock);
                }
            }
            else
            {
                _selectedBlock.SetColor(false);
            }
        }
    }
    private bool CheckAllow(int xPosition, int zPosition, Block block)
    {
        for (int x = 0; x < block.BlockWidht; x++)
        {
            for (int z = 0; z < block.BlockHeight; z++)
            {
                if (block.Blocks[x,z])
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
        Console.Clear();
        CheckWin();
    }
    private void CheckWin()
    {
        for (int i = 0; i < Platforms.Length; i++)
        {
            if (Platforms[i].Check())
            {
                
            }
            else
            {
                Debug.Log("Lose");
                break;
            }
        }
        Debug.Log("WIN");

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

}














//void Update()
//{
//    rayCol = _mainCamera.ScreenPointToRay(Input.mousePosition);
//    RaycastHit hit;
//    if (Input.GetMouseButtonDown(0))
//    {

//        if (Physics.Raycast(rayCol, out hit))
//        {
//            if (hit.collider.TryGetComponent<Block>(out Block block))
//            {
//                _selectedBlock = block;
//                RemoveFromDictionary(block);
//            }
//        }
//    }
//    if (_selectedBlock != null)
//    {
//        var basePlane = new Plane(Vector3.up, Vector3.zero);

//        if (basePlane.Raycast(rayCol, out float position))
//        {
//            Vector3 worldPosition = rayCol.GetPoint(position);

//            x = Mathf.RoundToInt(worldPosition.x);
//            y = Mathf.RoundToInt(worldPosition.z);
//            _selectedBlock.transform.position = new Vector3(x, 0, y);
//        }
//    }
//    if (Input.GetMouseButtonUp(0))
//    {
//        if (_selectedBlock)
//        {
//            InstallBlock(x, y, _selectedBlock);
//            _selectedBlock = null;
//        }
//    }
//}
//private void InstallBlock(int xPosition, int zPosition, Block block)
//{
//    for (int x = 0; x < _selectedBlock._blockWidht; x++)
//    {
//        for (int z = 0; z < _selectedBlock._blockHeight; z++)
//        {
//            if (_selectedBlock.Blocks[x, z])
//            {
//                Vector2Int coordinate = new Vector2Int(xPosition + x, zPosition + z);
//                BlocksDictionary.Add(coordinate, _selectedBlock);
//            }
//        }
//    }
//    foreach (var item in BlocksDictionary)
//    {
//        Debug.Log(item);
//    }
//}
//private void RemoveFromDictionary(Block block)
//{
//    List<Vector2Int> keysToRemove = new List<Vector2Int>();

//    foreach (KeyValuePair<Vector2Int, Block> entry in BlocksDictionary)
//    {
//        if (entry.Value.Equals(block))
//        {
//            keysToRemove.Add(entry.Key);
//        }
//    }

//    foreach (Vector2Int key in keysToRemove)
//    {
//        BlocksDictionary.Remove(key);
//    }
//}

