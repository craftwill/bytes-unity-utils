using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitializable
{
    void Initialization(params object[] parameters);
}
