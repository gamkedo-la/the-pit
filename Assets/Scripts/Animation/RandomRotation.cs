using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animation
{
    public class RandomRotation : MonoBehaviour
    {
        void Start()
        {
            int thing = Mathf.FloorToInt(Random.value * 2);

            Debug.Log(thing.ToString());

            if (thing >= 1) gameObject.transform.Rotate(new Vector3(0, 0, 180));
        }
    }
}
