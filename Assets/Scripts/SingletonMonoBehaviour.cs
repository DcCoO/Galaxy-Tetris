using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    public static T instance { get; protected set; }

    protected virtual void Awake()
    {
        if (!ReferenceEquals(instance, null) && !ReferenceEquals(instance, this))
            Destroy(instance.gameObject);
        instance = (T) this;
    }
}