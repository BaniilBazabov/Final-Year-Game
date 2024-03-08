using Godot;
using System;
public interface IEnemy //interface for enemies that they all share
{
    void Damage(float amount);
}

//Collision stuff cuz this file be empty ;D
//Player: itself -> Layer: 1,16, Mask  1,16
//    attackZone -> Layer: 7, Mask 7;

//Enemies: itself -> Layer 7, Mask 16;
//         hitbox -> Layer 16, Mask 16;



