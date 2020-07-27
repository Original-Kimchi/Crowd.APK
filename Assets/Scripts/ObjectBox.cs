using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectBox
{

    private static Queue<Transform> Objects = new Queue<Transform>();
    public static bool ObjectExist => Objects.Count != 0;

    public static void Enqueue(Transform something)
    {
        Objects.Enqueue(something);
        something.gameObject.SetActive(false);
    }

    public static Transform Dequeue()
    {
        Transform something;

        something = Objects.Dequeue();
        something.gameObject.SetActive(true);
        return something;
    }
}
