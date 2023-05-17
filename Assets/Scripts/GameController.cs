using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Singleton
    public static GameController Instance { get; private set; }
    #endregion

    [SerializeField] private BaseCar _car;
    private DependencyContainer _container;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _container = new DependencyContainer();

        IEngine standardEngine = new StandardEngine(100f, 10f, 10f);
        _container.Register(standardEngine);

        var sedan = _car as Sedan;
        sedan.Initialize(_container);
    }
}