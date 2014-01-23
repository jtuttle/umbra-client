using UnityEngine;
using System.Collections;

public class Killable : MonoBehaviour {
    public int Health;
    public bool Hittable;
    public float HitRecoverSeconds;

    private TimeKeeper _hitRecoverTimer;

    protected void Awake() {
        Health = 5;
        Hittable = true;
        HitRecoverSeconds = 0.8f;

        _hitRecoverTimer = TimeKeeper.GetTimer(HitRecoverSeconds, 1, "HitRecoverTimer");
        _hitRecoverTimer.OnTimerComplete += OnHitRecoverTimer;
        _hitRecoverTimer.transform.parent = gameObject.transform;
    }

    public void TakeDamage(int damage) {
        if(!Hittable) return;

        UpdateHealth(-damage);

        if(Health > 0) {
            Hittable = false;
            UnityUtils.SetTransparency(gameObject, 0.7f);
            _hitRecoverTimer.StartTimer();
        }
    }

    public void RestoreHealth(int health) {
        UpdateHealth(health);
    }

    private void UpdateHealth(int delta) {
        Health = Mathf.Max(0, Health + delta);
        
        if(Health <= 0) Die();
    }

    private void Die() {
        GameObject.Destroy(gameObject);
    }

    private void OnHitRecoverTimer(TimeKeeper timer) {
        UnityUtils.SetTransparency(gameObject, 1.0f);
        Hittable = true;
    }
}
