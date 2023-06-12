using UnityEngine;

public enum BlockColor
{
    red, yellow, green
}
public class Block : MonoBehaviour
{
    public int BlockWidht;
    public int BlockHeight;
    public bool[,] Blocks;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _denyColor;
    [SerializeField] private Material _allowColor;
    [SerializeField] private Material _startColor;
    public BlockColor BlockColor;
    [SerializeField] private bool _11;
    [SerializeField] private bool _12;
    [SerializeField] private bool _13;
    [SerializeField] private bool _21;
    [SerializeField] private bool _22;
    [SerializeField] private bool _23;
    [SerializeField] private bool _31;
    [SerializeField] private bool _32;
    [SerializeField] private bool _33;
    void Start()
    {
        Blocks = new bool[BlockWidht, BlockHeight];
        if (_11)
            Blocks[0, 0] = _11;
        if (_12)
            Blocks[0, 1] = _12;
        if (_13)
            Blocks[0, 2] = _13;
        if (_21)
            Blocks[1, 0] = _21;
        if (_22)
            Blocks[1, 1] = _22;
        if (_23)
            Blocks[1, 2] = _23;
        if (_31)
            Blocks[2, 0] = _31;
        if (_32)
            Blocks[2, 1] = _32;
        if (_33)
            Blocks[2, 2] = _33;
    }
    public void SetColor(bool avaliable)
    {
        if (avaliable)
            _renderer.material = _startColor;
        else
            _renderer.material = _denyColor;
    }

    public void SetStartColor()
    {
        _renderer.material = _startColor;
    }
}
