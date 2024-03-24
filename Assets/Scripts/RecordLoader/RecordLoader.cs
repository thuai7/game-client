using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordLoader : MonoBehaviour
{
    public string Folderpath { get; private set; }
    public List<string> RecordLists { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Record loader started.");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetRecordList(string path)
    {
        // TODO: Implement
        throw new System.NotImplementedException();
    }
}
