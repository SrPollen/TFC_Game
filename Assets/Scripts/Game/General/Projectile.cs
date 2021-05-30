using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //hay que cambiar interpolate y collision Detection(continuos) en el rb para que no atraviesen las paredes
    
    
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsPlayer;

    //stats
    [Range(0f, 1f)] public float bounciness;
    public bool useGravity;

    //Damage
    public int explosionDamage;
    public float explosionRange;

    //Lifetime
    public int maxCollisions;
    public float maxLifeTime;
    public bool explodeOnTouch = true;

    private int _collisions;
    private PhysicMaterial _physicMaterial;

    private bool hasExploded = false;

    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        maxLifeTime -= Time.deltaTime;
        if (hasExploded) return;
        if (_collisions > maxCollisions || maxLifeTime<= 0) Explode();
    }

    private void Explode()
    {
        Debug.Log("Explode");
        hasExploded = true;
        //Instanciate explosion
        //if (explosion != null)
        GameObject explosionParticles = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosionParticles, 3f);
        
        // Check target (440)
        Collider[] players = Physics.OverlapSphere(transform.position, explosionRange, whatIsPlayer);
        foreach (var player in players)
        {
            player.GetComponent<PlayerController>().TakeDamage(explosionDamage);
        }
        
        //delay to see if all works correctly
        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        _collisions++;
        //Explode if bullet hits player ang explodeontouch is activated
        if(other.collider.CompareTag("Player") && explodeOnTouch) Explode();
    }

    private void SetUp()
    {
        //Create a new physisc material
        _physicMaterial = new PhysicMaterial();
        _physicMaterial.bounciness = bounciness;
        _physicMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
        _physicMaterial.bounceCombine = PhysicMaterialCombine.Maximum;

        //assign material to collider
        GetComponent<SphereCollider>().material = _physicMaterial;

        //set gravity
        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}