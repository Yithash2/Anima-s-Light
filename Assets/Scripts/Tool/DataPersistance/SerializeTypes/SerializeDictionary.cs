using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class SerializeDictionary<Tkey, Tvalue> : Dictionary<Tkey, Tvalue>, ISerializationCallbackReceiver
{

    [SerializeField] private List<Tkey> keys = new List<Tkey>();
    [SerializeField] private List<Tvalue> values = new List<Tvalue>();
    public void OnBeforeSerialize(){
        keys.Clear();
        values.Clear();

        foreach(KeyValuePair<Tkey, Tvalue> pair in this){
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize(){
        this.Clear();

        if(keys.Count != values.Count){
            Debug.LogError("Tried to deserealize a Serializable Dictionary, but the amouunt of keys (" 
            + keys.Count + ") does not match the number of values (" 
            + values.Count + ") witch indicates that something went wrong");
        }

        for(int i = 0; i < keys.Count; i++){
            this.Add(keys[i], values[i]);
        }
    }

    public void TPCLear(){
        this.Clear();
        keys.Clear();
        values.Clear();

    }
}
