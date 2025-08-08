using System;
using System.Collections;
using UnityEngine;

namespace TheDeveloperTrain.SciFiGuns
{

    public class Gun : MonoBehaviour
    {

        public FireMode currentFireMode;

        /// <summary>
        /// The particle systems for the gun, if any
        /// </summary>
        [Tooltip("The particle systems for the gun, if any")]
        public ParticleSystem[] gunParticleSystems;

        [SerializeField] private GameObject bulletPrefab;

        /// <summary>
        /// The Transform point of the muzzle, AKA where the bullet prefab is spawned. the bullet inherits its rotation as well
        /// </summary>
        [Tooltip("The Transform point of the muzzle, AKA where the bullet prefab is spawned. the bullet inherits its rotation as well")]
        [SerializeField] private Transform muzzleTransform;

        public GunStats stats;

        /// <summary>
        /// The number of bullets currently left, before the gun has to reload
        /// </summary>
        [HideInInspector] public int currentBulletCount;
        private int currentMagLeft;

        [HideInInspector] public bool isReloading = false;
        public bool IsInShotCooldown { get; private set; } = false;

        /// <summary>
        /// Called when the bullet is actually created, AKA after the shoot delay.
        /// </summary>
        public Action onBulletShot;

        public Action onLastBulletShotInBurst;

        public Action onGunReloadStart;

        /// <summary>
        /// Called as soon as the gun starts it's shooting procedure, if the gun is ready to be fired
        /// </summary>
        public Action onGunShootingStart;
        public int baseDamage = 20;
        public AudioSource audioSource;
        public AudioClip shootSound;

        void Start()
        {
            currentBulletCount = stats.magazineSize;
            currentMagLeft = stats.totalAmmo;
            currentFireMode = stats.fireMode;
        }

        public void Shoot()
        {
            if (currentBulletCount > 0 && !isReloading && !IsInShotCooldown)
            {
                IsInShotCooldown = true;
                onGunShootingStart?.Invoke();
                foreach (var particleSystem in gunParticleSystems)
                {
                    particleSystem.Play();
                }
                if (currentFireMode == FireMode.Single)
                {
                    currentBulletCount--;
                    audioSource.Play();
                    Invoke(nameof(SpawnBullet), stats.shootDelay);
                    StartCoroutine(nameof(ResetGunShotCooldown));


                    if (currentBulletCount == 0)
                    {
                        Reload();
                    }
                }
                else if (currentFireMode == FireMode.Burst)
                {
                    StartCoroutine(nameof(FireBulletsInBurst));
                }
            }
        }

        private void SpawnBullet()
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab);
            bullet.transform.SetPositionAndRotation(muzzleTransform.position, muzzleTransform.rotation);
            bullet.GetComponent<Bullet>().speed = stats.bulletSpeed;

            BulletDamage bulletDamage = bullet.GetComponent<BulletDamage>();
            if (bulletDamage != null)
            {
                bulletDamage.damage = baseDamage;
            }

            onBulletShot?.Invoke();

        }

        public void Reload()
        {
            StartCoroutine(nameof(ReloadGun));
        }

        private IEnumerator ReloadGun()
        {
            if (!isReloading)
            {
                onGunReloadStart?.Invoke();
                isReloading = true;
                yield return new WaitForSeconds(stats.reloadDuration + (1 / stats.fireRate));
                if (currentMagLeft != 0)
                {
                    if (currentMagLeft - (stats.magazineSize - currentBulletCount) >= 0)
                    {
                        currentMagLeft -= (stats.magazineSize - currentBulletCount);
                        currentBulletCount = stats.magazineSize;
                    }
                    else
                    {
                        currentBulletCount += currentMagLeft;
                        currentMagLeft = 0;
                    }
                }
                isReloading = false;
            }
        }
        private IEnumerator ResetGunShotCooldown()
        {
            yield return new WaitForSeconds(1 / stats.fireRate - stats.shootDelay);
            IsInShotCooldown = false;
        }
        private IEnumerator FireBulletsInBurst()
        {
            yield return new WaitForSeconds(stats.shootDelay);

            for (int i = 0; i < stats.burstCount; i++)
            {
                SpawnBullet();
                currentBulletCount--;
                audioSource.Play();
                if (currentBulletCount == 0)
                {
                    Reload();
                    break;
                }
                onBulletShot?.Invoke();
                yield return new WaitForSeconds(stats.burstInterval);

            }
            onLastBulletShotInBurst?.Invoke();
            yield return new WaitForSeconds(1 / stats.fireRate - (stats.shootDelay + (stats.burstCount * stats.burstInterval)));
            IsInShotCooldown = false;
        }

        public void ResetFireMode()
        {
            currentFireMode = stats.fireMode;
        }

    }

}