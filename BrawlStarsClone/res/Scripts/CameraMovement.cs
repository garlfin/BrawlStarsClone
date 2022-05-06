﻿using BrawlStarsClone.Engine.Component;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Silk.NET.Maths;

namespace BrawlStarsClone.res.Scripts;

public class CameraMovement : Behavior
{
    
    public float CameraSpeed = 4f;

    public override void OnLoad()
    {
        var entityTransform = Owner.GetComponent<Transform>();
        entityTransform.Rotation = new Vector3D<float>(-59, -90, 0);
        entityTransform.Location = new Vector3D<float>(8.5f, 25, 0);
    }

    public override void OnUpdate(float gameTime)
    {
        var entityTransform = Owner.GetComponent<Transform>();

        var input = Owner.Window.Input;
        if (input.IsKeyDown(Keys.W)) entityTransform.Location -= Vector3D<float>.UnitZ * CameraSpeed * gameTime; // Forward
        if (input.IsKeyDown(Keys.S)) entityTransform.Location += Vector3D<float>.UnitZ * CameraSpeed * gameTime; // Backwards
    }
}