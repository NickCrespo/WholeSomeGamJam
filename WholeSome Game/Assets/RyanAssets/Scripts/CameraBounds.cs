using UnityEngine;

[ExecuteInEditMode]
public class CameraBounds : MonoBehaviour
{
    [SerializeField] Vector3 center;
    public Vector3 size;
    [SerializeField] Vector3 min;
    [SerializeField] Vector3 max;

    public Vector3 Center
    {
        get
        {
            return transform.position + center;
        }
        private set
        {
            center = value;
        }
    }
    public Vector3 Extends
    {
        get;
        private set;
    }
    public Vector3 Min
    {
        get
        {
            return transform.position + min;
        }
        private set
        {
            min = value;
        }
    }
    public Vector3 Max
    {
        get
        {
            return transform.position + max;
        }
        private set
        {
            max = value;
        }
    }

    void Update()
    {
        UpdateVectors();
    }
    void OnDrawGizmosSelected()
    {
        // transform
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1);

        // center and size cube
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Center, size);

        // center
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(Center, .5f);

        // min
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Min, .3f);

        // max
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Max, .3f);
    }

    void UpdateVectors()
    {
        Extends = size / 2f;
        Min = center - Extends;
        Max = center + Extends;
    }
}
