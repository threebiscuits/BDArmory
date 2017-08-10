﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BDArmory.UI
{
    
    public class BDGUIComboBox
    {
        private static bool forceToUnShow = false;
        private static int useControlID = -1;
        private bool isClickedComboButton = false;
        private int selectedItemIndex = -1;

        private Rect rect;
        private GUIContent buttonContent;
        private GUIContent[] listContent;
        private GUIStyle listStyle;
        private Vector2 scrollViewVector;
        private float comboxbox_height;

        public BDGUIComboBox(Rect rect, GUIContent buttonContent, GUIContent[] listContent, float combo_height, GUIStyle listStyle)
        {
            this.rect = rect;
            this.buttonContent = buttonContent;
            this.listContent = listContent;
            this.listStyle = listStyle;
            this.comboxbox_height = combo_height;
        }


        public int Show()
        {
            if (forceToUnShow)
            {
                forceToUnShow = false;
                isClickedComboButton = false;
            }

            bool done = false;
            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            switch (Event.current.GetTypeForControl(controlID))
            {
                case EventType.mouseUp:
                    {
                        if (isClickedComboButton)
                        {
                            done = true;
                        }
                    }
                    break;
            }

            if (selectedItemIndex > -1)
                buttonContent.text = listContent[selectedItemIndex].text;

            if (GUI.Button(rect, buttonContent, HighLogic.Skin.button))
            {
                if (useControlID == -1)
                {
                    useControlID = controlID;
                    isClickedComboButton = false;
                }

                if (useControlID != controlID)
                {
                    forceToUnShow = true;
                    useControlID = controlID;
                }
                isClickedComboButton = true;
            }

            if (isClickedComboButton)
            {
                float items_height = listStyle.CalcHeight(listContent[0], 1.0f) * (listContent.Length + 5);
                Rect listRect = new Rect(rect.x, rect.y + listStyle.CalcHeight(listContent[0], 1.0f), rect.width, items_height);

                scrollViewVector = GUI.BeginScrollView(new Rect(rect.x, rect.y + rect.height, rect.width * 1.1f, comboxbox_height), scrollViewVector,
                                                        new Rect(rect.x, rect.y, rect.width, items_height + rect.height), false, false, HighLogic.Skin.horizontalScrollbar, HighLogic.Skin.verticalScrollbar);

                GUI.Box(new Rect(rect.x, rect.y, rect.width, items_height + rect.height), "", HighLogic.Skin.box);

                int newSelectedItemIndex = GUI.SelectionGrid(listRect, selectedItemIndex, listContent, 1, listStyle);
                if (newSelectedItemIndex != selectedItemIndex)
                {
                    selectedItemIndex = newSelectedItemIndex;
                    done = true;
                }

                GUI.EndScrollView();
            }

            if (done)
                isClickedComboButton = false;

            return selectedItemIndex;
        }

        public int SelectedItemIndex
        {
            get
            {
                return selectedItemIndex;
            }
            set
            {
                selectedItemIndex = value;
            }
        }
    }
}