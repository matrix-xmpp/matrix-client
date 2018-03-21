/****************************** Module Header ******************************\
* Module Name:  HighlightTextBlock.cs
* Project:      ListBox_HighlightMatchString
* Copyright (c) Microsoft Corporation.
*
* Custom control class to enable options of highlighting.
*
*
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/en-us/openness/resources/licenses.aspx#MPL.
* All other rights reserved.
*
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;

namespace MatrixClient.Controls
{
    public class HighlightTextBlock : TextBlock
    {
        #region DependencyProperty

        // Background color for highlighting matching text.
        public static DependencyProperty HighlightBackgroundProperty =
            DependencyProperty.Register("HighlightBackground", typeof(Brush),
            typeof(HighlightTextBlock));

        public Brush HighlightBackground
        {
            get
            {
                return (Brush)GetValue(HighlightBackgroundProperty);
            }
            set
            {
                SetValue(HighlightBackgroundProperty, value);
            }
        }

        // Foreground color for highlighting matching text.
        public static DependencyProperty HighlightForegroundProperty =
            DependencyProperty.Register("HighlightForeground", typeof(Brush),
            typeof(HighlightTextBlock));

        public Brush HighlightForeground
        {
            get
            {
                return (Brush)GetValue(HighlightForegroundProperty);
            }
            set
            {
                SetValue(HighlightForegroundProperty, value);
            }
        }

        // Text to be highlighted.
        public static DependencyProperty HighlightTextroperty =
            DependencyProperty.Register("HighlightText", typeof(String),
            typeof(HighlightTextBlock),
           new PropertyMetadata(new PropertyChangedCallback(HighlightTextChanged)));

        public String HighlightText
        {
            get
            {
                return (String)GetValue(HighlightTextroperty);
            }
            set
            {
                SetValue(HighlightTextroperty, value);
            }
        }

        //Is text matching case sensitive.
        public static DependencyProperty MatchCaseProperty =
            DependencyProperty.Register("MatchCase", typeof(Boolean),
            typeof(HighlightTextBlock));

        public Boolean MatchCase
        {
            get
            {
                return (Boolean)GetValue(MatchCaseProperty);
            }
            set
            {
                SetValue(MatchCaseProperty, value);
            }
        }

        //Should we match the complete word.
        public static DependencyProperty MatchWholeWordProperty =
            DependencyProperty.Register("MatchWholeWord", typeof(Boolean),
            typeof(HighlightTextBlock),
            new PropertyMetadata(new PropertyChangedCallback(MatchWholeWordPropertyChanged)));

        public Boolean MatchWholeWord
        {
            get
            {
                return (Boolean)GetValue(MatchWholeWordProperty);
            }
            set
            {
                SetValue(MatchWholeWordProperty, value);
            }
        }


        #endregion

        #region Property

        bool Highlighted;

        #endregion

        #region Method

        /// <summary>
        /// Callback method when user search is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void HighlightTextChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            //getting the current instance of Textblock
            HighlightTextBlock tb = sender as HighlightTextBlock;

            string completeText = tb.Text.Trim();
            string searchText = e.NewValue.ToString().Trim();

            //Check if string case hase to be considered.
            if (!tb.MatchCase)
            {
                completeText = completeText.ToLower();
                searchText = searchText.ToLower();
            }

            //Check if we need to compare the whole word.
            if (tb.MatchWholeWord)
            {
                if (completeText != string.Empty)
                {
                    //when the condition is true, 
                    //set the highlight color specified by customer.
                    if (completeText == searchText)
                    {
                        // we swap the colors so reset when no search text is passed.
                        Swap(tb);
                        tb.Highlighted = true;
                    }
                    else
                    {
                        if (tb.Highlighted)
                        {
                            Swap(tb);
                            tb.Highlighted = false;
                        }
                    }
                }
            }
            else
            {
                int endIndex = completeText.Length;
                int highlightStartIndex = completeText.IndexOf(searchText);

                completeText = tb.Text.Trim();

                tb.Inlines.Clear();
                if (highlightStartIndex >= 0)
                {
                    int highlightTextLength = searchText.Length;
                    int highlightEndIndex = highlightStartIndex + highlightTextLength;

                    tb.Inlines.Add(completeText.Substring(0, highlightStartIndex));
                    tb.Inlines.Add(new Run()
                    {
                        Text = completeText.Substring(highlightStartIndex, highlightTextLength),
                        Foreground = tb.HighlightForeground,
                        Background = tb.HighlightBackground
                    });
                    tb.Inlines.Add(completeText.Substring
                        (highlightEndIndex,
                        endIndex - highlightEndIndex));

                }
                else
                {
                    tb.Inlines.Add(completeText);
                }
            }       
        }

        private static void Swap(HighlightTextBlock tb)
        {
            Brush temp;

            temp = tb.Background;
            tb.Background = tb.HighlightBackground;
            tb.HighlightBackground = temp;

            temp = tb.Foreground;
            tb.Foreground = tb.HighlightForeground;
            tb.HighlightForeground = temp;

            temp = null;
        }

        private static void MatchWholeWordPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            //getting the current instance of Textblock
            HighlightTextBlock tb = sender as HighlightTextBlock;

            if (!(bool)e.NewValue)
            {
                if (tb.Highlighted)
                {
                    Swap(tb);
                    tb.Highlighted = false;
                }
            }
        }

        #endregion
    }
}
