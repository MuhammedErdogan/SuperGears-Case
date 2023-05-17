using UnityEngine;
using System.Collections.Generic;

public class DependencyContainer : MonoBehaviour
{
    private Dictionary<System.Type, object> dependencies = new Dictionary<System.Type, object>();

    public void Register<T>(T dependency)
    {
        dependencies[typeof(T)] = dependency;
    }

    public T Resolve<T>()
    {
        return (T)dependencies[typeof(T)];
    }
}