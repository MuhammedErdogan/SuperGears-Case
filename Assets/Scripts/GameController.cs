using UnityEngine;

public class GameController : MonoBehaviour
{
    private DependencyContainer container;

    private void Awake()
    {
        container = new DependencyContainer();

        IEngine standardEngine = new StandardEngine(100f, 10f, 10f);
        container.Register<IEngine>(standardEngine);

        BaseCar car = new BaseCar(container.Resolve<IEngine>());
    }
}