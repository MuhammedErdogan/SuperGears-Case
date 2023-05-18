using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Singleton
    public static GameController Instance { get; private set; }
    #endregion

    [SerializeField] private BaseCar _car;

    public float SpeedMs { get; private set; }
    public float SpeedKmh { get; private set; }
    public float SpeedMph { get; private set; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        var container = new DependencyContainer();

        IEngine standardEngine = new StandardEngine(100f, 10f, 20f, 5f, 5);
        container.Register(standardEngine);

        var sedan = _car as Sedan;
        container.InjectDependencies(_car);

        var speedoMeter = FindObjectOfType<Speedometer>();
        container.InjectDependencies(speedoMeter);

        EventManager.TriggerEvent(EventManager.CarInitialized);
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventManager.OnCarMove, CalculateSpeed);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventManager.OnCarMove, CalculateSpeed);
    }

    public void CalculateSpeed(Rigidbody rb)
    {
        SpeedMs = rb.velocity.magnitude;
        SpeedKmh = SpeedMs * 3.6f;  // 1 m/s = 3.6 km/h
        SpeedMph = SpeedMs * 2.237f;  // 1 m/s = 2.237 mph

        EventManager.TriggerEvent(EventManager.NotifySpeed, SpeedKmh);
    }
}