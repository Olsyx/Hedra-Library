using System;
using System.Collections;
using System.Collections.Generic;
using HedraLibrary.Shapes.Polygons;
using UnityEngine;

namespace HedraLibrary.Components {
    public abstract class PolygonCollider : MonoBehaviour, IPolygonColliderManager {
        [SerializeField] protected LayerMask collisionMask;

        [SerializeField] private string id;
        public string Id { get { return id; } }

        protected Polygon polygon;
        public Polygon Polygon { get { return polygon; } }
        
        void Awake() {
            Init();
        }

        protected virtual void Init() {
            RegisterToManager();
        }

        void Update() {
            polygon.Center = (Vector2)transform.position;
            polygon.Rotation = transform.rotation.eulerAngles.z;
        }

        void OnDestroy() {
            RemoveFromManager();
        }

        #region Manager
        void IPolygonColliderManager.SetId(string newId) {
            id = newId;
        }

        void RegisterToManager() {
            PolygonManager.Register(this);
        }

        void RemoveFromManager() {
            PolygonManager.Remove(this);
        }

        #endregion

        void OnDrawGizmos() {
            if (!Application.isPlaying) {
                Init();
            }

            Gizmos.DrawWireSphere(polygon.Center, 0.02f);
            polygon.DrawEdges();
        }
    }
}