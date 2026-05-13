using UnityEngine;

public class TPS_TwoStepHitscanWeapon : MonoBehaviour
{
    [SerializeField] private Camera aimCamera;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float aimRange = 100f;
    [SerializeField] private float shotRange = 100f;
    [SerializeField] private int damage = 10;

    [SerializeField] private LayerMask aimMask;
    [SerializeField] private LayerMask shotMask;
    [SerializeField] private LayerMask muzzleBlockMask;

    [SerializeField] private float shotRadius = 0f;

}
