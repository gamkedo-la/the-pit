using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ResolutionTest : MonoBehaviour
{
    private PixelPerfectCamera ppf;
    private readonly List<(KeyCode, int, int)> keys = new()
    {
        (KeyCode.Alpha1, 320, 240),
        (KeyCode.Alpha2, 640, 480),
        (KeyCode.Alpha3, 800, 600),
        (KeyCode.Alpha4, 1024, 768),
        (KeyCode.Q, 16, 0),
        (KeyCode.W, 32, 0),
        (KeyCode.E, 64, 0),
    };

    private void Start()
    {
        ppf = GetComponent <PixelPerfectCamera>();
    }

    private void Update()
    {
        foreach (var tuple in keys)
        {
            var (code, x, y) = tuple;

            if (Input.GetKeyDown(code))
            {
                if (y == 0)
                {
                    ppf.assetsPPU = x;
                }
                else
                {
                    ppf.refResolutionX = x;
                    ppf.refResolutionY = y;
                }
            }
        }
    }
}