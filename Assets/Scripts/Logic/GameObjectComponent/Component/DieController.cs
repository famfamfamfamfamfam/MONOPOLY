using System;
using System.Collections.Generic;
using UnityEngine;

public class DieController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    LayerMask dieDotsMask;

    [SerializeField]
    LayerMask _dieType;

    public LayerMask dieType { get => _dieType; }
    public Action onBeginRoll { get; set; }
    public Action<int, LayerMask> onFinishRoll { get; set; }
    int point;

    float minBounceForce = 3f;
    float maxBounceForce = 5f;
    float minTorqueForce = 540f;
    float maxTorqueForce = 720f;

    RaycastHit[] dotsHit;
    RaycastHit[] dieHit;
    float maxDistanceToCheckDots = 0.5f;
    float maxDistanceToCheckDie = 30f;
    Dictionary<Collider, int> pointDictionary;
    readonly int pointDictBestSize = 6;

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
        dotsHit = new RaycastHit[1];
        dieHit = new RaycastHit[1];
        pointDictionary = new Dictionary<Collider, int>(pointDictBestSize * 100 / 75 + 1);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.RaycastNonAlloc(
                Camera.main.ScreenPointToRay(Input.mousePosition),
                    dieHit, maxDistanceToCheckDie, _dieType) > 0)
            {
                Roll();
            }
        }
        CountPointIfDieHasStopped();
        ResetToRoll();
    }

    void Roll()
    {
        if (isOnGround)
        {
            onBeginRoll.Invoke();
            rb.isKinematic = false;
            rb.AddForce(UnityEngine.Random.Range(minBounceForce, maxBounceForce) * Vector3.up, ForceMode.Impulse);
            rb.AddTorque(UnityEngine.Random.Range(minTorqueForce, maxTorqueForce) * UnityEngine.Random.onUnitSphere, ForceMode.Impulse);
        }
    }

    void CountPointIfDieHasStopped()
    {
        if (rb.IsSleeping() && !rb.isKinematic)
        {
            rb.isKinematic = true;
            GetPoint();
        }
    }

    void GetPoint()
    {
        if (Physics.RaycastNonAlloc(
            transform.position, Vector3.up, dotsHit, maxDistanceToCheckDots,
            dieDotsMask, QueryTriggerInteraction.Collide) > 0 )
        {
            if (pointDictionary.TryGetValue(dotsHit[0].collider, out int value))
            {
                point = value;
                return;
            }
            point = dotsHit[0].collider.GetComponent<DieSidePointProperty>().point;
            pointDictionary.Add(dotsHit[0].collider, point);
            if (pointDictionary.Count == pointDictBestSize)
            {
                pointDictionary.TrimExcess();
            }
        }
    }

    void ResetToRoll()
    {
        if (rb.IsSleeping() && rb.isKinematic && !isOnGround)
        {
            onFinishRoll.Invoke(point, _dieType);
            isOnGround = true;
            enabled = false;
        }
    }
}