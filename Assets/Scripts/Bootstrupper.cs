using UnityEngine;

public class Bootstrupper : MonoBehaviour
{
    [SerializeField] private Management _management;


    public void Start()
    {
        for (int i = 0; i < _management.Platforms.Length; i++)
        {
            _management.Platforms[i].Init(_management);
        }

        for (int i = 0; i < _management.Blocks.Length; i++)
        {
            _management.Blocks[i].Init(_management);
        }
    }
}
