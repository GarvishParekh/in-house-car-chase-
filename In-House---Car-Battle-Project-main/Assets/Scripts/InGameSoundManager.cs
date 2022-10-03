using UnityEngine;

public class InGameSoundManager : MonoBehaviour
{
    public enum Weapons
    {
        SMG,
        LMG,
        Sniper
    }

    [Header("Components")]
    [SerializeField] Weapons weapons;
    [SerializeField] AudioSource soundSoruce;

    [Header("Weapon sounds")]
    [SerializeField] AudioClip LMG_clip;
    [SerializeField] AudioClip SMG_clip;
    [SerializeField] AudioClip Sniper_Clip;

    [Header("Car crash Sound")]
    [SerializeField] AudioClip[] carCrash_Clips;
    [SerializeField] AudioClip[] carExplosion_Clip;

    private void OnEnable()
    {
        ShootingSystem.Shoot += PlaySound;
        PlayerCarTrigger.OnPlayerCarDamage += CarCrashSound;
        AI_CarHealth.AIDestroy += CarExplosion;
    }

    private void OnDisable()
    {
        ShootingSystem.Shoot -= PlaySound;
        PlayerCarTrigger.OnPlayerCarDamage -= CarCrashSound;
        AI_CarHealth.AIDestroy += CarExplosion;
    }

    void PlaySound(Vector3 v)
    {
        if (weapons == Weapons.SMG)
        {
            soundSoruce.PlayOneShot(SMG_clip);
        }
        else if (weapons == Weapons.LMG)
        {
            soundSoruce.PlayOneShot(LMG_clip);
        }
        else if (weapons == Weapons.Sniper)
        {
            soundSoruce.PlayOneShot(Sniper_Clip);
        }
    }

    void CarCrashSound(int i)
    {
        int _soundCount = Random.Range(0, carCrash_Clips.Length);
        soundSoruce.PlayOneShot(carCrash_Clips[_soundCount]);
    }

    void CarExplosion (Transform t)
    {
        int _soundCount = Random.Range(0, carCrash_Clips.Length);
        soundSoruce.PlayOneShot(carExplosion_Clip[_soundCount]);
    }
}
