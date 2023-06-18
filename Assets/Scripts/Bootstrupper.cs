using UnityEngine;

public class Bootstrupper : MonoBehaviour
{
    [SerializeField] private Management _management;
    public Platform[] Platforms;
    public Block[] Blocks;

    public void Awake()
    {
        Platforms = FindObjectsOfType<Platform>();
        Blocks = FindObjectsOfType<Block>();
        Invoke(nameof(Init), .25f);
    }
    private void Init()
    {
        _management.Init(Platforms, Blocks);

        for (int i = 0; i < _management.Platforms.Length; i++)
        {
            Platforms[i].Init(_management);
        }

        for (int i = 0; i < _management.Blocks.Length; i++)
        {
            Blocks[i].Init(_management);
        }

    }
}
