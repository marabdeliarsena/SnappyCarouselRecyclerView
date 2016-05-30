using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Util;

namespace SnappyCarouselRecyclerView
{
    [Register("com.arsena.SnappyCarouselRecyclerView.RecyclerView")]
    public class CustomRecyclerView : Android.Support.V7.Widget.RecyclerView
    {
        int screenWidth = 0;
        public int ItemCount { get; set; }

        int _currentItem;
        public int CurrentItem
        {
            get { return _currentItem; }
            set
            {
                if (value<=0)
                {
                    _currentItem = 0;
                }
                else
                {
                    if (value > ItemCount-1)
                    {
                        _currentItem = ItemCount - 1;
                    }
                    else
                    {
                        _currentItem = value;
                    }
                    
                }
            }
        }

        public event EventHandler<FlingEventArgs> OnFling;
        public event EventHandler<FlingEventArgs> BeforeFling;

        public CustomRecyclerView(Context context):base(context)
        {
            CurrentItem = 0;
        }

        public CustomRecyclerView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {

        }

        protected CustomRecyclerView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {

        }
        public CustomRecyclerView(Context context, IAttributeSet attrs, int defStyle):base(context,attrs,defStyle)
        {

        }

        public void SetScreenWidth(int width)
        {
            screenWidth = width;
        }

       public override bool Fling(int velocityX, int velocityY)
        {
            if (ItemCount < 1)
            {
                return true;
            }
            FlingEventArgs flingEventArgs;
            if (velocityX > 0)
            {
                flingEventArgs = new FlingEventArgs(FlingDirection.Right, CurrentItem);
                if (CurrentItem == ItemCount - 1)
                {
                    return true;
                }
            }
            else
            {
                flingEventArgs = new FlingEventArgs(FlingDirection.Left, CurrentItem);
            }

            if (BeforeFling != null)
            {
                BeforeFling(this, flingEventArgs);
            }
           LinearLayoutManager linearLayoutManager = (LinearLayoutManager)GetLayoutManager();
       
           //these four variables identify the views you see on screen.
           int lastVisibleView = linearLayoutManager.FindLastVisibleItemPosition();
           int firstVisibleView = linearLayoutManager.FindFirstVisibleItemPosition();
           View firstView = linearLayoutManager.FindViewByPosition(firstVisibleView);
           View lastView = linearLayoutManager.FindViewByPosition(lastVisibleView);
       
           //these variables get the distance you need to scroll in order to center your views.
           //my views have variable sizes, so I need to calculate side margins separately.     
           //note the subtle difference in how right and left margins are calculated, as well as
           //the resulting scroll distances.
           int leftMargin = (screenWidth - lastView.Width) / 2;
           int rightMargin = (screenWidth - firstView.Width) / 2 + firstView.Width;
           int leftEdge = lastView.Left;
           int rightEdge = firstView.Right;
           int scrollDistanceLeft = leftEdge - leftMargin;
           int scrollDistanceRight = rightMargin - rightEdge;
       
           //if(user swipes to the left) 
           if (velocityX > 0)
            {
                CurrentItem = lastVisibleView-1;
               SmoothScrollBy(scrollDistanceLeft, 0);
           }
           else
            {
                CurrentItem = firstVisibleView-1;
               SmoothScrollBy(-scrollDistanceRight, 0);
           }

           if (OnFling != null)
           {
                
                OnFling(this, flingEventArgs);
           }
           return true;
       }

    }

    public enum FlingDirection
    {
        Left,
        Right
    }

    public class FlingEventArgs : EventArgs
    {
        public FlingDirection Direction
        {
            get;
            set;
        }
        public int CurrentItem
        {
            get;
            set;
        }
        public FlingEventArgs(FlingDirection direction, int currentItem)
        {
            Direction = direction;
            CurrentItem = currentItem;
        }
    }
}