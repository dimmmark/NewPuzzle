using UnityEngine;

public enum BlockColor
{
    red, yellow, green, blue, violet
}
public class Block : MonoBehaviour
{
    public BlockColor BlockColor;
    public int BlockWidht;
    public int BlockHeight;
    public bool[,] Blocks;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _denyColor;
    [SerializeField] private Material _allowColor;
    [SerializeField] private Material _startColor;
    [SerializeField] private Material _redColor;
    [SerializeField] private Material _yellowColor;
    [SerializeField] private Material _greenColor;
    [SerializeField] private Material _blueColor;
    [SerializeField] private Material _violetColor;
    [SerializeField] private bool _11;
    [SerializeField] private bool _12;
    [SerializeField] private bool _13;
    [SerializeField] private bool _21;
    [SerializeField] private bool _22;
    [SerializeField] private bool _23;
    [SerializeField] private bool _31;
    [SerializeField] private bool _32;
    [SerializeField] private bool _33;
    //[SerializeField] private Collider[] _colliders;
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
    public void Init(Management management)
    {
        management.InstallBlock((int)transform.position.x, (int)transform.position.z, this);
    }
    public void SetColor(bool avaliable)
    {
        if (avaliable)
        {
            _renderer.material = _startColor;
            //for (int i = 0; i < _colliders.Length; i++)
            //{
            //    _colliders[i].enabled = true;
            //}
        }
        else
        {
            _renderer.material = _denyColor;
        //    for (int i = 0; i < _colliders.Length; i++)
        //    {
        //        _colliders[i].enabled = false;
        //    }
        }
    }

    public void SetStartColor()
    {
        _renderer.material = _startColor;
    }
    [ContextMenu("SetColorPrefs")]
    public void SetPrefColor()
    {
        var value = BlockColor;
        switch (value)
        {
            case BlockColor.red:
                _renderer.material = _redColor;
                _startColor = _redColor;
                break;
            case BlockColor.yellow:
                _renderer.material = _yellowColor;
                _startColor = _yellowColor;
                break;
            case BlockColor.blue:
                _renderer.material = _blueColor;
                _startColor = _blueColor;
                break;
            case BlockColor.green:
                _renderer.material = _greenColor;
                _startColor = _greenColor;
                break;
            case BlockColor.violet:
                _renderer.material = _violetColor;
                _startColor = _violetColor;
                break;
        }
    }

}
