using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// in this case of the tutorial, we want everything to be manually generated.
// we supply a 2D array of characters that specify which objects to place where.
// 0 = open floor
// 1 = wall
// 2 = player
// 3 = stairs
public static class TutorialData {
  public static readonly int tutorialRows = 9;
  public static readonly int tutorialColumns = 15;
  public static readonly int[,] TutorialLevelOneData = {
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
    { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
    { 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1 },
    { 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 },
    { 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 3, 0, 1 },
    { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1 },
    { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
    { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
  };

  public static readonly int[,] TutorialLevelTwoData = {
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
    { 1, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
    { 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 },
    { 1, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1 },
    { 1, 0, 2, 0, 0, 0, 0, 0, 0, 1, 4, 0, 3, 0, 1 },
    { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1 },
    { 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
    { 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
  };

  public static readonly int[,] TutorialLevelThreeData = {
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
    { 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
    { 1, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 },
    { 1, 0, 0, 0, 0, 0, 0, 0, 5, 1, 0, 0, 0, 0, 1 },
    { 1, 2, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 3, 0, 1 },
    { 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1 },
    { 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
    { 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
  };
  

  public static readonly string[] TutorialStepText = 
  {
    "You've woken up in a strange forest. Explore with WASD.", // 0
    "Stairs! I wonder where they lead...", // 1
    "Looks like more forest. Let's keep exploring.", // 2
    "That pillbug looks hostile. When it gets to an adjacent cell, select \"Attack\" with [1] and click on the enemy to attack it.", // 3
    "Nice! You attacked it. You should be able to take it out without dying.", // 4
    "Woah! You absorbed the pillbug's soul, restoring your health and stealing its abilities and stats.", // 5
    "Let's try out the pillbug's roll. Select your ability with [2], and use it by clicking in any direction.", // 6 
    "Nice! This will be useful for dealing with any more threats.", // 7
    "A new enemy! Let's try and use our roll ability and basic attack to take it out.", // 8
    "", // 9 (skipped)
    "Nice job! You've absorbed the eye's soul. Different enemies have different abilities and stats", // 10
    "In a pinch, you can revert to the entity you were previously. Select revert mode with [3], and click anywhere to activate it.", // 11
    "Nice! Let's keep exploring the forest!" // 12
  };
}