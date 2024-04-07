using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ThunderPulse.Navigation
{
    public class Follower
    {
        public Transform tr;
        public int currentPoint = 0;
    }
    public class PathFollowerB : MonoBehaviour
    {
        public int followersCount = 1;
        public Transform[] path;
        public List<Follower> followers = new List<Follower>();
        [Range(0,5)]
        public float speed = 5.0f;
        public float reachDist = 1.0f;
        public GameObject prefab;

        public void Start()
        {
            StartCoroutine(creator());
        }
        IEnumerator creator()
        {
            for (int n = 0; n < followersCount; n++)
            {
                yield return new WaitForSeconds(.3f);
                var newObj = Instantiate(prefab).transform;
                newObj.position = path[0].position;
                newObj.SetParent(transform);
                followers.Add(new Follower()
                {
                    currentPoint = 0,
                    tr = newObj
                });
            }
        }
        void Update()
        {
            for (int n = 0; n < followers.Count; n++)
            {
                Follow(followers[n]);
            }
        }
        public void Follow(Follower follower)
        {
            Vector3 diff = path[follower.currentPoint].position - follower.tr.position;
            Quaternion targetLook = Quaternion.LookRotation(diff.normalized, transform.up);
            follower.tr.rotation = Quaternion.Slerp(follower.tr.rotation, targetLook, .1f);
            Vector3 acc = follower.tr.forward * Time.deltaTime * speed;
            follower.tr.position += acc;

            if (diff.magnitude <= reachDist ||
                reachDist < acc.magnitude)
            {
                if (speed > 0)
                {
                    follower.currentPoint++;

                    if (follower.currentPoint >= path.Length)
                    {
                        follower.currentPoint = 0;
                    }
                }
                else
                {
                    follower.currentPoint--;

                    if (follower.currentPoint < 0)
                    {
                        follower.currentPoint = path.Length-1;
                    }
                }
            }
        }

        void OnDrawGizmos()
        {
            if (path.Length > 0)
                for (int i = 0; i < path.Length; i++)
                {
                    if (path[i] != null)
                    {
                        Gizmos.DrawSphere(path[i].position, .1f);
                    }
                }
        }
    }
}
