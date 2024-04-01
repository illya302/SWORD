using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonAnimations : MonoBehaviour
{

    Coroutine sizeChangeCoroutine;

    bool isRotating;


    public void SizeChange(float size)
    {
        if (sizeChangeCoroutine != null)
        {
            StopCoroutine(sizeChangeCoroutine);
        }

        sizeChangeCoroutine = StartCoroutine(ChangeSizeCoroutine(size, 5));
    }

    public void RotateAroundY(float angle)
    {
        Rotation(Vector3.up, angle, 500);
    }
    public void RotateAroundX(float angle)
    {
        Rotation(Vector3.right, angle, 500);
    }
    public void RotateAroundZ(float angle)
    {
        Rotation(Vector3.forward, angle, 500);
    }

    private void Rotation(Vector3 rotationAxis, float rotationAngle, float rotationSpeed)
    {
        if (isRotating)
            return;
        
        StartCoroutine(RotationCoroutine(rotationAxis, rotationAngle, rotationSpeed / Time.timeScale));
    }

    IEnumerator ChangeSizeCoroutine(float size, float changeSpeed)
    {
        Vector3 sizeVector = new Vector3(size, size);
        changeSpeed = changeSpeed / Time.timeScale;
        float totalSizeChange = 0;
        float diff = Mathf.Abs(transform.localScale.x - sizeVector.x);

        while (totalSizeChange <= diff)
        {
            Vector3 sizeChangeVector = new Vector2(changeSpeed, changeSpeed) * Time.deltaTime;
            totalSizeChange += sizeChangeVector.x;

            if (sizeVector.x > transform.localScale.x)
                transform.localScale += sizeChangeVector;
            else if (sizeVector.x < transform.localScale.x)
                transform.localScale -= sizeChangeVector;

            yield return null;
        }

        transform.localScale = sizeVector;
    }

    IEnumerator RotationCoroutine(Vector3 rotationAxis, float rotationAngle, float rotationSpeed)
    {
        isRotating = true;

        float totalRotationDeg = 0;

        while (totalRotationDeg <= rotationAngle)
        {
            float rotation = rotationSpeed * Time.deltaTime;
            totalRotationDeg += rotation;
            transform.Rotate(rotationAxis, rotation);
            yield return null;
        }

        transform.rotation = new Quaternion(0,0,0,0);

        isRotating = false;
    }
}
