using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISerializeble
{
    JObject Serialize();
    public void Desirialize(string jsonString);
    string GetJsonKey();
}
