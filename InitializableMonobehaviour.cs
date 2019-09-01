using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializableMonobehaviour : MonoBehaviour
{
    public virtual void Initialization(params object[] parameters) { }
}
