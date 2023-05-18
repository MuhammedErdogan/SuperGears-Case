using System;
using System.Collections.Generic;
using System.Reflection;

public class DependencyContainer
{
    private Dictionary<Type, object> dependencies = new Dictionary<Type, object>();

    public void Register<T>(T dependency)
    {
        dependencies[typeof(T)] = dependency;
    }

    public T Resolve<T>()
    {
        var instance = (T)dependencies[typeof(T)];
        InjectDependencies(instance);
        return instance;
    }

    public void InjectDependencies(object instance)
    {
        Type type = instance.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            InjectAttribute attribute = field.GetCustomAttribute<InjectAttribute>();
            if (attribute != null)
            {
                Type dependencyType = field.FieldType;
                if (dependencies.ContainsKey(dependencyType))
                {
                    object dependency = dependencies[dependencyType];
                    field.SetValue(instance, dependency);
                }
                else
                {
                    throw new Exception($"No registered dependency for type {dependencyType}");
                }
            }
        }
    }
}

public class InjectAttribute : Attribute
{
}
