using UnityEngine;

/// <summary>
/// Basic lifetime class to automatically return an object to the pool after X time.
/// Requires https://github.com/akbiggs/UnityTimer
/// </summary>

public class Lifetime : PooledMonoBehaviour
{
    [SerializeField] float lifetime = 0.1f;

    protected override void Start()
    {
        base.Start();

        this.AttachTimer(lifetime, ReturnToPool);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();

        this.AttachTimer(lifetime, ReturnToPool);
    }
}
