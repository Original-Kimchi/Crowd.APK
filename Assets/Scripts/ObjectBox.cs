﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectBox
{
    private static Queue<Transform> Objects = new Queue<Transform>();

    public static void Enqueue(Transform something)
    {
        Objects.Enqueue(something);

        something.gameObject.SetActive(false);
    }

    public static Transform Dequeue()
    {
        Transform something;

        something = Objects.Dequeue();

        return something;
    }
}
