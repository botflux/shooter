using UnityEngine;

public interface IDamageable
{
    void TakeHit(float damage, Vector3 point, Vector3 direction);
    void TakeDamage(float damage);
}