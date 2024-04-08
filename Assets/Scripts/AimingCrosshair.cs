using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimingCrosshair : MonoBehaviour
{
    Image[] _aimingCorsshairComponents;
    bool _locked;
    public bool Locked
    {
        get
        {
            return _locked;
        }
        set 
        {
            if (value)
            {
                CurrentColor = LockedColor;
            }
            else
            {
                CurrentColor = UnlockedColor;
            }
            _locked = value;
        }
    }
    Color _currentColor;
    Color CurrentColor
    {
        get 
        { 
            return _currentColor; 
        }
        set 
        {
            _currentColor = value;
            foreach(Image image in _aimingCorsshairComponents)
            {
                image.color = _currentColor;
            }
        }
    }
    public Color LockedColor;
    public Color UnlockedColor;
    private void Start()
    {
        _aimingCorsshairComponents = GetComponentsInChildren<Image>();
    }
}
