using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class ParticleSystemEx
{
    public static GameObject PlayPath(this ParticleSystem particlesystem, string path)
    {
        return Res.LoadObjWithObjectPool(path);
    }

    public static void ActiveEmit(this ParticleSystem particleSystem,bool isOpen)
    {
        ParticleSystem.EmissionModule em = particleSystem.emission;
        em.enabled = isOpen;
    }
}
