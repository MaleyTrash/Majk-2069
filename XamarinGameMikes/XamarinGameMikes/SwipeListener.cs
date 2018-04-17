﻿using System;
using Xamarin.Forms;

namespace XamarinGameMikes
{
    public class SwipeListener : PanGestureRecognizer
    {
        private ISwipeCallBack mISwipeCallback;
        private double translatedX = 0, translatedY = 0;

        public SwipeListener(View view, ISwipeCallBack iSwipeCallBack)
        {
            mISwipeCallback = iSwipeCallBack;
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            view.GestureRecognizers.Add(panGesture);
        }

        void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {

            View Content = (View)sender;

            switch (e.StatusType)
            {

                case GestureStatus.Running:

                    try
                    {
                        translatedX = e.TotalX;
                        translatedY = e.TotalY;
                    }
                    catch (Exception err)
                    {
                        System.Diagnostics.Debug.WriteLine("" + err.Message);
                    }
                    break;

                case GestureStatus.Completed:

                    //System.Diagnostics.Debug.WriteLine("translatedX : " + translatedX);
                    //System.Diagnostics.Debug.WriteLine("translatedY : " + translatedY);

                    if (translatedX < 0 && Math.Abs(translatedX) > Math.Abs(translatedY))
                    {
                        mISwipeCallback.onLeftSwipeAsync(Content);
                    }
                    else if (translatedX > 0 && translatedX > Math.Abs(translatedY))
                    {
                        mISwipeCallback.onRightSwipeAsync(Content);
                    }
                    else if (translatedY < 0 && Math.Abs(translatedY) > Math.Abs(translatedX))
                    {
                        mISwipeCallback.onTopSwipeAsync(Content);
                    }
                    else if (translatedY > 0 && translatedY > Math.Abs(translatedX))
                    {
                        mISwipeCallback.onBottomSwipeAsync(Content);
                    }
                    else
                    {
                        mISwipeCallback.onNothingSwiped(Content);
                    }

                    break;

            }
        }

    }
}