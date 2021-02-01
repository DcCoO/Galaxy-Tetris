using System;
using UnityEngine;

public class TimeController : MonoBehaviour, IReset
{
    public static TimeController instance;

    public event Action onTick;
    
    public TimeSettings settings;
    private float tickTimer;
    private float tickDuration;
    private float elapsedTime;
    private float minMaxInterval;

    private bool isPlaying;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        elapsedTime = tickTimer = 0;
        minMaxInterval = Mathf.Abs(settings.beginTickDuration - settings.endTickDuration);
    }

    void Update()
    {
        if (!GameController.instance.isPlaying) return;
        
        Time.timeScale = Input.GetKey(KeyCode.Space) ? 3 : 1;
        
        elapsedTime += Time.deltaTime;
        tickTimer += Time.deltaTime;
        if (tickTimer > tickDuration)
        {
            tickTimer = 0;
            Tick();
        }
    }

    private void Tick()
    {
        float ratio = Mathf.Min(1, elapsedTime / settings.tickTransitionTime);
        tickDuration = settings.endTickDuration + (1 - ratio) * minMaxInterval;
        onTick?.Invoke();
    }
}
