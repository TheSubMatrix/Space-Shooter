using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent<uint> OnScoreUpdated = new UnityEvent<uint>();
    uint _score;
    public uint Score
    {
        get 
        { 
            return _score; 
        }
        set 
        {
            _score = value;
            OnScoreUpdated.Invoke(_score);
        }
    }
    private void Start()
    {
        OnScoreUpdated.Invoke(_score);
    }
}
