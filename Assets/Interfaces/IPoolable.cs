using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    public bool IsActive { get; }
    public void Activate();
    public void Deactivate();
}

//public abstract class PoolableObject : IPoolable
//{
//    public bool IsActive { get; private set; }

//    public abstract void Activate();

//    public abstract void Deactivate();
//}
