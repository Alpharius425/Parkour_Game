using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawnPosition : MonoBehaviour
{
    public static ProjectileSpawnPosition Instance;

    private void Awake()
    {
        Instance = this;
    }
}
