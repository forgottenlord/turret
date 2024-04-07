using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class Turret : MonoBehaviour {

    public Transform turret;
    public Transform cannon;
    public float yawSpeed = 30f;
    public float pitchSpeed = 30f;
    public float yawLimit = 90f;
    public float pitchLimit = 90f;
    public Vector3 target;

    private Quaternion turretRotation;
    private Quaternion turrentRotation;

    public virtual void Start()
    {
        this.turretRotation = this.turret.localRotation;
        this.turrentRotation = this.cannon.localRotation;
    }

    public virtual void Update()
    {
        Targeting(this);
    }
    public static void Targeting(Turret turret)
    {
        float angle = 0.0f;
        Vector3 targetRelative = default(Vector3);
        Quaternion targetRotation = default(Quaternion);
        if (turret.turret && (turret.yawLimit != 0f))
        {
            targetRelative = turret.turret.InverseTransformPoint(turret.target);
            angle = Mathf.Atan2(targetRelative.x, targetRelative.z) * Mathf.Rad2Deg;
            if (angle >= 180f)
                angle = 180f - angle;
            if (angle <= -180f)
                angle = -180f + angle;
            targetRotation = turret.turret.rotation * Quaternion.Euler(0f, Mathf.Clamp(angle, -turret.yawSpeed * Time.deltaTime, turret.yawSpeed * Time.deltaTime), 0f);
            if ((turret.yawLimit < 360f) && (turret.yawLimit > 0f))
                turret.turret.rotation = Quaternion.RotateTowards(turret.turret.parent.rotation * turret.turretRotation, targetRotation, turret.yawLimit);
            else
                turret.turret.rotation = targetRotation;
        }
        if (turret.cannon && (turret.pitchLimit != 0f))
        {
            targetRelative = turret.cannon.InverseTransformPoint(turret.target);
            angle = -Mathf.Atan2(targetRelative.y, targetRelative.z) * Mathf.Rad2Deg;
            if (angle >= 180f)
                angle = 180f - angle;
            if (angle <= -180f)
                angle = -180f + angle;
            targetRotation = turret.cannon.rotation * Quaternion.Euler(Mathf.Clamp(angle, -turret.pitchSpeed * Time.deltaTime, turret.pitchSpeed * Time.deltaTime), 0f, 0f);
            if ((turret.pitchLimit < 360f) && (turret.pitchLimit > 0f))
                turret.cannon.rotation = Quaternion.RotateTowards(turret.cannon.parent.rotation * turret.turrentRotation, targetRotation, turret.pitchLimit);
            else
                turret.cannon.rotation = targetRotation;
        }
        Debug.DrawLine(turret.cannon.position, turret.target, Color.red);
        Debug.DrawRay(turret.cannon.position, turret.cannon.forward * (turret.target - turret.cannon.position).magnitude, Color.green);
    }

    public virtual void SetTarget(Vector3 target)
    {
        this.target = target;
    }
}