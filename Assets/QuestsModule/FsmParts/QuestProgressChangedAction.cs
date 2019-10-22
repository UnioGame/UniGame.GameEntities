using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TO DO
/// </summary>
public class QuestProgressChangedAction
{
    public readonly float oldProgress;
    public readonly float newProgress;

    public QuestProgressChangedAction(float oldVal, float newVal){
        oldProgress = oldVal;
        newProgress = newVal;
    }
}
