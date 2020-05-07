﻿using UnityEngine;

namespace ExpressoBits.Console.Commands
{
    [CreateAssetMenu(fileName = "Log Command", menuName = "Expresso Bits/Console/Log Command")]
    public class LogCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            string logText = string.Join(" ", args);

            Debug.Log(logText);
            MessageLogger.Instance.Log(logText);

            return true;
        }
    }
}