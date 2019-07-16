using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

[System.Serializable]
public class Save
{
  public List<RacerScore> topLaptimes = new List<RacerScore>();
  public List<RacerScore> topTotaltimes = new List<RacerScore>();
}
