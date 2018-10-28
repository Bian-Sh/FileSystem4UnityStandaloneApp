
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
        Debug.Log(args.Aggregate("CommandLineArgs : ", (a, b) => a + " , " + b));
        Debug.Log(args.Aggregate("cmdChain : ", (a, b) => a + " " + b));

        if (args.Length > 1)
        {
            if (!string.IsNullOrEmpty(args[1]))
            {
                MatchCollection match = Regex.Matches(cmdChain, @"\.exe ([A-Za-z]:.+\.bian)", RegexOptions.Singleline);
                //for (int i = 0; i < match.Count; i++)
                //{
                //    for (int j = 0; j < match[i].Groups.Count; j++)
                //    {
                //        Debug.Log(string.Format("match {0},groups {1}, value {2}",i,j,match[i].Groups[j]));
                //    }
                //}

                if (match.Count > 0 && match[0].Groups.Count > 0)
                {
                    text.text = match[0].Groups[1].Value;
                }
            }
        }

    }

}