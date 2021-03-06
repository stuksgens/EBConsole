﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace ExpressoBits.Console.UI
{
    [AddComponentMenu(menuName:"UI/Visual Console")]
    public class VisualConsole : MonoBehaviour
    {
        private Commander m_Commander;
        private Logs m_Logs;
        
        public ConsoleAlign align;
        
        [Header("UI Prefabs")]
        public InputField consoleInputPrefab;
        public LogPanel messagePanel;
        public LogMessage uiLogPrefab;

        public Font font;
        
        [HideInInspector]
        public InputField consoleInput;
        
        private LogPanel m_LogPanel;

        private void Awake()
        {
            m_Commander = GetComponentInParent<Commander>();
            

            SetupConsoleInput();

            consoleInput.onValueChanged.AddListener(delegate
            {
                if (!consoleInput.text.Contains("\n")) return;
                var text = (consoleInput.text).Remove(consoleInput.text.LastIndexOf("\n", StringComparison.Ordinal));
                consoleInput.text = text;
                m_Commander.ProcessCommand(text);
            });

            consoleInput.GetComponentInChildren<Text>().font = font;

            consoleInput.gameObject.SetActive(false);
            
            m_Commander.onCloseCommander.AddListener(delegate {
                consoleInput.gameObject.SetActive(false);
                //consoleInput.DeactivateInputField();
            });
            
            m_Commander.onOpenCommander.AddListener(delegate
            {
                consoleInput.gameObject.SetActive(true);
                consoleInput.ActivateInputField();
            });
            
            m_Commander.onFinishProcessCommand.AddListener(delegate
            {
                consoleInput.text = string.Empty;
            });
    
            // Check if logs exists
            m_Logs = GetComponentInParent<Logs>();
            if (m_Logs)
            {
                SetupLogPanel();

                m_Commander.onCloseCommander.AddListener(delegate
                {
                    m_LogPanel.logPanelScroll.SetActive(false);
                    m_LogPanel.logPanelToast.SetActive(true);
                });
                
                m_Commander.onOpenCommander.AddListener(delegate
                {
                    m_LogPanel.logPanelScroll.SetActive(true);
                    m_LogPanel.logPanelToast.SetActive(false);
                });
            }

        }

        private void SetupLogPanel()
        {
            m_LogPanel = Instantiate(messagePanel, transform);
            if (align != ConsoleAlign.Bottom) return;
            var r = m_LogPanel.GetComponent<RectTransform>();
            r.offsetMin = new Vector2(r.offsetMin.x, 24);
            r.offsetMax = new Vector2(r.offsetMax.x, 0);

            var verticalLayoutGroup = m_LogPanel.logPanelToast.GetComponent<VerticalLayoutGroup>();
            verticalLayoutGroup.childAlignment = TextAnchor.LowerCenter;

            var verticalLayoutGroup2 = m_LogPanel.logScrollContent.GetComponent<VerticalLayoutGroup>();
            verticalLayoutGroup2.childAlignment = TextAnchor.LowerCenter;
            var r3 = m_LogPanel.logScrollContent.GetComponent<RectTransform>();
            r3.pivot = new Vector2(r3.pivot.x, 0);
            r3.anchorMin = new Vector2(r3.anchorMin.x, 0);
            r3.anchorMax = new Vector2(r3.anchorMax.x, 0);

            var r2 = m_LogPanel.logScrollContent.GetComponent<RectTransform>();
            r2.pivot = new Vector2(r2.pivot.x, 0);
            r2.anchorMin = new Vector2(r2.anchorMin.x, 0);
            r2.anchorMax = new Vector2(r2.anchorMax.x, 0);
        }

        private void SetupConsoleInput()
        {
            consoleInput = Instantiate(consoleInputPrefab, transform);

            if (align != ConsoleAlign.Bottom) return;
            var r = consoleInput.GetComponent<RectTransform>();
            //r.pivot = Vector2.down;
            r.pivot = new Vector2(r.pivot.x,0);
            r.anchorMin = new Vector2(r.anchorMin.x,0);
            r.anchorMax = new Vector2(r.anchorMax.x,0);


        }

        public bool IsEnableInput()
        {
            return consoleInput.gameObject.activeSelf;
        }

        public LogMessage InstantiateLogsAndReturnToastLog(string logText,float timer, Sprite sprite, Color color)
        {
            var staticLog = Instantiate(uiLogPrefab, m_LogPanel.logPanelToast.transform);
            if(align == ConsoleAlign.Top)staticLog.transform.SetSiblingIndex(0);
            staticLog.Setup(logText,font, timer, sprite, color);
            
            var toastLog = Instantiate(uiLogPrefab, m_LogPanel.logScrollContent.transform);
            if(align == ConsoleAlign.Top)toastLog.transform.SetSiblingIndex(0);
            toastLog.Setup(logText, font, sprite, color);

            return toastLog;
        }
    }

}

