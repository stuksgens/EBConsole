﻿using UnityEngine;

namespace ExpressoBits.Console.Commands
{
    [CreateAssetMenu(fileName = "Log Warning Command", menuName = "Expresso Bits/Console/Log Warning Command")]
    public class LogWarningCommand : ConsoleCommand
    {
        public override bool Process(string[] args)
        {
            string logText = string.Join(" ", args);

            if (logText.Length <= 0) return false;

            Debug.LogWarning(logText);
            if (Commander.Instance.GetComponent<Logger>() != null) Logger.Instance.LogWarning(logText);

            return true;
        }
    }
}