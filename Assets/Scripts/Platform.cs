using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int Capasity;
    [HideInInspector] public List<Block> PlatformList = new List<Block>();
    public int Widht;
    public int Height;
    public void Init(Management management)
    {
        Capasity = Widht * Height;
        management.InstallPlatform((int)transform.position.x, (int)transform.position.z, this);
    }

    public bool Check()
    {
        if (PlatformList.Count == 0)
        {
            return true;
        }
        
        else if (PlatformList.Count == Capasity)
        {
            var startCheckColor = PlatformList[0].BlockColor;

            for (int i = 0; i < PlatformList.Count; i++)
            {
                if (PlatformList[i].BlockColor == startCheckColor)
                {

                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}
