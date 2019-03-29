using HedraLibrary;
using HedraLibrary.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleIK : MonoBehaviour {
    [Serializable]
    public struct Bone {
        public Transform boneBase;
        public Transform boneEnd;
    }
    public Transform pivot;
    public Transform target;

    public List<Bone> bones = new List<Bone>();

    void Update() {
        if (bones.Count < 3) {
            return;
        }

        LookAt(bones[0], target);
        bones[2].boneBase.position = target.position - (bones[2].boneEnd.position - bones[2].boneBase.position);




        /*
        LookAt(bones[2], pivot);
        LookAt(bones[2], target);
        bones[2].boneBase.position = target.position - bones[2].boneEnd.localPosition;
        
        LookAt(bones[1], bones[2].boneBase);

        bones[2].boneBase.position = bones[1].boneEnd.position;

        LookAt(bones[0], pivot);*/
    }

    void LookAt(Bone subject, Transform target) {
        Vector3 from = subject.boneEnd.localPosition;
        Vector3 to = target.position - subject.boneBase.position;
        subject.boneBase.rotation = Quaternion.FromToRotation(from, to);
    }

    bool TooFarAway() {
        float length = 0f;
        for (int i = 0; i < bones.Count; i++) {
            length += bones[i].boneEnd.localPosition.magnitude;
        }
        return length < Vector3.Distance(bones[0].boneBase.position, target.position);
    }

    void ResetBones() {
        for (int i = 0; i < bones.Count; i++) {
            bones[i].boneBase.rotation = Quaternion.Euler(Vector3.zero);
        }

        for (int i = 1; i < bones.Count; i++) {
            bones[i].boneBase.localPosition = bones[i-1].boneEnd.localPosition;
        }
    }

    void OnDrawGizmos() {
        for (int i = 0; i < bones.Count; i++) {
            Gizmos.DrawWireSphere(bones[i].boneBase.position, 0.02f);
            Gizmos.DrawLine(bones[i].boneBase.position, bones[i].boneEnd.position);
        }
    }
}
