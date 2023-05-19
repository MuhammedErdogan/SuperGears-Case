using System;
using System.Collections.Generic;
using System.Reflection;

namespace Manager
{
    public class DependencyManager
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

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
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

}
public class InjectAttribute : Attribute
{
}