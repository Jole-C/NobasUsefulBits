using UnityEngine;
using UnityEngine.Events;

///<Summary>
/// Item class supporting physics and usable/interactable items.
///</Summary>

[RequireComponent(typeof(Rigidbody))]

public class Item : MonoBehaviour
{
    [Header("Behaviour")]
    [SerializeField] protected bool canPickUp = true;
    [SerializeField] protected bool canDrop = true;
    [SerializeField] protected bool hasUse = true;
    [SerializeField] protected bool hasAltUse = true;

    public bool CanPickUp { get { return canPickUp; } }
    public bool CanDrop { get { return canDrop; } }
    public bool HasUse { get { return hasUse; } }
    public bool HasAltUse { get { return hasAltUse; } }

    [Header("Uses")]
    [SerializeField] int numberOfUses = 0;

    protected GameObject attachmentPoint;

    protected Collider collider;
    protected Rigidbody rigidbody;

    public bool IsHeld { get; protected set; }
    public bool Spent { get; protected set; } = false;
    public UnityEvent OnPickedUp { get; private set; } = new UnityEvent();
    public UnityEvent OnDropped { get; private set; } = new UnityEvent();

    GameObject ownerPlayer;

    protected virtual void Start()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        
    }

    public virtual bool Use(GameObject player)
    {
        return HasUse;
    }

    public virtual bool AltUse(GameObject player)
    {
        return HasAltUse;
    }

    public virtual bool Pickup(GameObject newAttachmentPoint, GameObject player = null)
    {
        if (CanPickUp || !IsHeld)
        {
            OnPickedUp.Invoke();
            ownerPlayer = player;

            IsHeld = true;

            SetAttachmentPoint(newAttachmentPoint);

            return true;
        }

        return false;
    }

    public virtual bool Drop(GameObject player = null)
    {
        if (CanDrop)
        {
            OnDropped.Invoke();
            ownerPlayer = null;

            IsHeld = false;

            SetAttachmentPoint(null);

            return true;
        }

        return false;
    }

    protected virtual void SetAttachmentPoint(GameObject newAttachmentPoint)
    {
        attachmentPoint = newAttachmentPoint;

        SetAttachmentState();
    }

    protected void SetAttachmentState()
    {
        if (attachmentPoint)
        {
            rigidbody.isKinematic = true;
            collider.enabled = false;

            gameObject.transform.parent = attachmentPoint.transform;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
        }
        else
        {
            rigidbody.isKinematic = false;
            collider.enabled = true;

            transform.parent = null;
        }
    }

    public void SetActive(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Enable()
    {
        rigidbody.isKinematic = false;

        gameObject.SetActive(true);

        transform.parent = null;
    }

    public void Disable()
    {
        rigidbody.isKinematic = transform;

        gameObject.SetActive(false);
    }
}
