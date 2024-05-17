using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Unity.VisualScripting;
using System.Runtime.InteropServices;

public class Follower : MonoBehaviour
{
   [SerializeField] private PathCreator[] pathCreator;
   [SerializeField] private float speed;
   [SerializeField] private bool stop;
   [SerializeField] private float maxDistance;
   [SerializeField] private bool rightSide;
   private float saveSpeed;
   private float distanceTravelled;
   private PathCreator thisPath;
   private bool seen = false;
   private bool gone = true;
   private TrafficLoad trafficLoad;


   void Start() {
      if (Random.Range(0,10) >= 5) {
         thisPath = pathCreator[0];
      } else{
       thisPath = pathCreator[1];
      }

      saveSpeed = speed;
   }
   void Update() {
    
    distanceTravelled += speed * Time.deltaTime;

    
    transform.position = thisPath.path.GetPointAtDistance(distanceTravelled);
    transform.rotation = thisPath.path.GetRotationAtDistance(distanceTravelled);

    Ray ray = new Ray(transform.position, transform.forward);

    
      if (Physics.Raycast(ray, out RaycastHit hit, maxDistance)) {
         
         if(hit.collider.gameObject.layer == 9 || hit.collider.gameObject.name == "Plane" || hit.collider.gameObject.layer == 7 || hit.collider.gameObject.layer == 14
            || hit.collider.gameObject.layer == 15) {
            
            IncreaseSpeed();

            if(hit.collider.gameObject.layer == 14 && !seen) {
               // Debug.Log("Adding");
               if(hit.collider.GetComponent<TrafficLoad>() != null) {
               trafficLoad = hit.collider.GetComponent<TrafficLoad>();
               trafficLoad.Increment();
               seen = true;
               gone = false;
               }
            }
         
            if(hit.collider.gameObject.layer == 15 && !gone) {
               if(trafficLoad != null) {
               Debug.Log("Decreasing");
               trafficLoad.Decrement();
               gone = true;
               }
            }
         }
         else if (hit.collider.gameObject.layer == 6) {
            Debug.Log("DESTROY1!");
            Destroy(gameObject);
         }
         else {

         if(hit.collider.gameObject.layer == 10 && rightSide) {
            IncreaseSpeed();
         } else if (hit.collider.gameObject.layer == 12 && rightSide)
          IncreaseSpeed();
         else 
          DecreaseSpeed();

         //Debug.Log(hit.collider.gameObject.tag);
         //Debug.Log(hit.collider.gameObject.name + " Was hit");

            }
         }
         else {
            IncreaseSpeed();
         }
   
    }

    public void IncreaseSpeed() {
      speed += 0.4f * saveSpeed *Time.deltaTime;
            if (speed > saveSpeed) {
            speed = saveSpeed;
         }
    }

    public void DecreaseSpeed() {
      speed -= saveSpeed*0.1f;
         if (speed < 0) {speed = 0;}
    }
    

    private void OnTriggerEnter(Collider other) {
      Debug.Log(other.name);
    }
   }

