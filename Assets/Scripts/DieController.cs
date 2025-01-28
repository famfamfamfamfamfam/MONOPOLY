using UnityEngine;

public class DieController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    LayerMask dieDotsMask;

    float minBounceForce = 3f;
    float maxBounceForce = 5f;
    float minTorqueForce = 540f;
    float maxTorqueForce = 720f;

    RaycastHit[] hit;
    float maxDistance = 0.5f;

    bool isOnGround = true;

    void OnCollisionEnter(Collision collision)
    {
        isOnGround = true;
    }
    void OnCollisionExit(Collision collision)
    {
        isOnGround = false;
    }

    void Start()
    {
        hit = new RaycastHit[1];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Roll();
        }
        CountPointIfDieHasStopped(out int point);
    }

    void Roll()
    {
        if (isOnGround)
        {
            rb.isKinematic = false;
            rb.AddForce(Random.Range(minBounceForce, maxBounceForce) * Vector3.up, ForceMode.Impulse);
            rb.AddTorque(Random.Range(minTorqueForce, maxTorqueForce) * Random.onUnitSphere, ForceMode.Impulse);
        }
    }

    void CountPointIfDieHasStopped(out int point)
    {
        point = 0;
        if (rb.IsSleeping() && !rb.isKinematic)
        {
            rb.isKinematic = true;
            GetPoint(ref point);
        }
    }

    void GetPoint(ref int point)
    {
        if (Physics.RaycastNonAlloc(transform.position, Vector3.up, hit, maxDistance, dieDotsMask, QueryTriggerInteraction.Collide) > 0)
        {
            point = hit[0].collider.GetComponent<IPointOnSide>().point;
        }
    }
}
