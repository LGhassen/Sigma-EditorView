﻿using UnityEngine;


namespace SigmaEditorViewPlugin
{
    internal class FlareCamera : MonoBehaviour
    {
        LensFlare flare;
        float maxBrightness;
        bool hidden = false;
        static int layerMask = 1 << 00 | 1 << 01 | 1 << 02 | 1 << 03 | 1 << 04 | 1 << 05 | 1 << 06 | 1 << 07 | 1 << 08 | 1 << 09 |
                               1 << 10 | 1 << 11 | 1 << 12 | 1 << 13 | 1 << 14 | 1 << 15 | 1 << 16 | 1 << 17 | 1 << 18 | 1 << 19 |
                               1 << 20 | 1 << 21 | 1 << 22 | 1 << 23 | 1 << 24 | 1 << 25 | 1 << 26 | 1 << 27 | 1 << 28 | 1 << 29 |
                               1 << 30 | 1 << 31;

        void Start()
        {
            Debug.Log("FlareCamera.Start");

            flare = GetComponent<LensFlare>();
            maxBrightness = flare.brightness;

            Debug.Log("FlareCamera.Start", "maxBrightness = " + maxBrightness);

            FlareRemover remover = GetComponent<FlareRemover>();
            if (remover != null)
            {
                DestroyImmediate(remover);
            }
        }

        void LateUpdate()
        {
            CheckHidden();

            if (hidden)
            {
                Hide();
            }
            else
            {
                Show();
            }

            Track();
        }

        void CheckHidden()
        {
            hidden = Physics.Raycast(EditorCamera.Instance.transform.position, flare.transform.forward.normalized * -5000, Mathf.Infinity, layerMask);
        }

        void Show()
        {
            if (flare.brightness < maxBrightness)
                flare.brightness += flare.fadeSpeed * Time.deltaTime;

            if (flare.brightness > maxBrightness)
                flare.brightness = maxBrightness;
        }

        void Hide()
        {
            if (flare.brightness > 0)
                flare.brightness -= flare.fadeSpeed * Time.deltaTime;

            if (flare.brightness < 0)
                flare.brightness = 0;
        }

        LineRenderer line;

        void Track()
        {
            if (Debug.debug)
            {
                if (line == null)
                {
                    GameObject myLine = new GameObject("lineRenderer");
                    line = myLine.AddOrGetComponent<LineRenderer>();
                    line.transform.position = Camera.main.transform.position + Camera.main.transform.forward.normalized;
                    line.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
                    line.startWidth = 0.02f;
                    line.endWidth = 25f;
                }

                if (Camera.main?.transform != null)
                    line.SetPosition(0, Camera.main.transform.position + Camera.main.transform.forward.normalized);

                if (transform != null)
                    line.SetPosition(1, transform.forward.normalized * -5000);

                line.startColor = line.endColor = hidden ? Color.red : Color.green;
            }
        }
    }
}
