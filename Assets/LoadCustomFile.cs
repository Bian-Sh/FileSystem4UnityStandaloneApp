
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class LoadCustomFile : MonoBehaviour
{
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
        string[] args = System.Environment.GetCommandLineArgs();
        string cmdChain = args.Aggregate((a, b) => a + " " + b);
        Debug.Log(args.Aggregate("CommandLineArgs : ",(a, b) => a + " , " + b));
        Debug.Log(args.Aggregate("cmdChain : ", (a, b) => a + " " + b));

        if (args.Length > 1)
        {
            if (!string.IsNullOrEmpty(args[1]))
            {
                MatchCollection match = Regex.Matches(cmdChain, @"\.exe ([A-Za-z]:.+\.bian)", RegexOptions.Singleline);
                text.text = match[1].Value;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}