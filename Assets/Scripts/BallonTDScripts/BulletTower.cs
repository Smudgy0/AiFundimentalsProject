using UnityEngine;

public class BulletTower : MonoBehaviour
{
    bool detectedBloon;

    [SerializeField] Transform cannonPivot;
    [SerializeField] Transform firingPoint;

    [SerializeField] bool fired;
    [SerializeField] float firingDelay;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float bulletDamage = 1;
    [SerializeField] float bulletSpeed = 5;
    [SerializeField] float lookAheadAmount = 5;

    Vector3 lookAheadDirection;
    [SerializeField] TDBallon1Script cloestBloonTDB;

    private void Update()
    {
        if(detectedBloon)
        {
            lookAheadDirection = cloestBloonTDB.moveDelta * lookAheadAmount;
            cannonPivot.LookAt(cloestBloonTDB.transform.position + lookAheadDirection);

            if (!fired)
            {
                Fire();
            }
        }
    }

    void Fire()
    {
        fired = true;
        GameObject bulletClone = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        bulletClone.GetComponent<Rigidbody>().linearVelocity = firingPoint.forward * bulletSpeed;
        Destroy(bulletClone, 5);
        Invoke("FireCooldown", firingDelay);
    }

    void FireCooldown()
    {
        fired = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Ballon"))
        {
            detectedBloon = true;
            cloestBloonTDB = other.GetComponent<TDBallon1Script>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ballon"))
        {
            detectedBloon = false;
            cloestBloonTDB = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(cloestBloonTDB.transform.position, Vector3.one);
    }
}
