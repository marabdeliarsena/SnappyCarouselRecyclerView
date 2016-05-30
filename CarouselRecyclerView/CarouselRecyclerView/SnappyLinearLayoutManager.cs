/* 
 *
 *  Source was ported from: http://stackoverflow.com/a/26445064/3423468 
 *
 */

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
using Android.Hardware;
using Android.Graphics;

namespace com.arsena.SnappyCarouselRecyclerView
{
    public class SnappyLinearLayoutManager : LinearLayoutManager, ISnappyLayoutManager
    {
        // These variables are from android.widget.Scroller, which is used, via ScrollerCompat, by
        // Recycler View. The scrolling distance calculation logic originates from the same place. Want
        // to use their variables so as to approximate the look of normal Android scrolling.
        // Find the Scroller fling implementation in android.widget.Scroller.fling().
        private const float INFLEXION = 0.35f; // Tension lines cross at (INFLEXION, 1)
        private float DECELERATION_RATE = (float)(Math.Log(0.78) / Math.Log(0.9));
        private double FRICTION = 0.84;

        private double deceleration;

        public SnappyLinearLayoutManager(Context context)
            : base(context)
        {

            CalculateDeceleration(context);
        }

        public SnappyLinearLayoutManager(Context context, int orientation, bool reverseLayout)
            : base(context, orientation, reverseLayout)
        {

            CalculateDeceleration(context);
        }

        private void CalculateDeceleration(Context context)
        {
            deceleration = SensorManager.GravityEarth // g (m/s^2)
                    * 39.3700787 // inches per meter
                // pixels per inch. 160 is the "default" dpi, i.e. one dip is one pixel on a 160 dpi
                // screen
                    * context.Resources.DisplayMetrics.Density * 160.0f * FRICTION;
        }

        public int GetPositionForVelocity(int velocityX, int velocityY)
        {
            if (ChildCount == 0)
            {
                return 0;
            }
            if (Orientation == 0)
            {
                return CalcPosForVelocity(velocityX, GetChildAt(0).Left, GetChildAt(0).Width,
                        GetPosition(GetChildAt(0)));
            }
            else
            {
                return CalcPosForVelocity(velocityY, GetChildAt(0).Top, GetChildAt(0).Height,
                        GetPosition(GetChildAt(0)));
            }
        }

        private int CalcPosForVelocity(int velocity, int scrollPos, int childSize, int currPos)
        {
            double v = Math.Sqrt(velocity * velocity);
            double dist = GetSplineFlingDistance(v);

            double tempScroll = scrollPos + (velocity > 0 ? dist : -dist);

            if (velocity < 0)
            {
                // Not sure if I need to lower bound this here.
                return (int)Math.Max(currPos + tempScroll / childSize, 0);
            }
            else
            {
                return (int)(currPos + (tempScroll / childSize) + 1);
            }
        }

        public override void SmoothScrollToPosition(Android.Support.V7.Widget.RecyclerView recyclerView, Android.Support.V7.Widget.RecyclerView.State state, int position)
        {
            CustomLinearSmoothScroller linearSmoothScroller = new CustomLinearSmoothScroller(recyclerView.Context);
            linearSmoothScroller.SetManager(this);
            linearSmoothScroller.TargetPosition = position;
            StartSmoothScroll(linearSmoothScroller);
        }

        private double GetSplineFlingDistance(double velocity)
        {
            double l = GetSplineDeceleration(velocity);
            double decelMinusOne = DECELERATION_RATE - 1.0;
            return ViewConfiguration.ScrollFriction * deceleration
                    * Math.Exp(DECELERATION_RATE / decelMinusOne * l);
        }

        private double GetSplineDeceleration(double velocity)
        {
            return Math.Log(INFLEXION * Math.Abs(velocity)
                    / (ViewConfiguration.ScrollFriction * deceleration));
        }

        /**
         * This implementation obviously doesn't take into account the direction of the 
         * that preceded it, but there is no easy way to get that information without more
         * hacking than I was willing to put into it.
         */
        public int GetFixScrollPos()
        {
            if (this.ChildCount == 0)
            {
                return 0;
            }

            View child = GetChildAt(0);
            int childPos = GetPosition(child);

            if (Orientation == Horizontal && Math.Abs(child.Left) > child.MeasuredWidth / 2)
            {
                // Scrolled first view more than halfway offscreen
                return childPos + 1;
            }
            else if (Orientation == Vertical
                  && Math.Abs(child.Top) > child.MeasuredWidth / 2)
            {
                // Scrolled first view more than halfway offscreen
                return childPos + 1;
            }
            return childPos;
        }

        public Android.Graphics.PointF ComputeScrollVectorForPosition(int targetPosition)
        {
            if (ChildCount == 0)
            {
                return null;
            }
            int firstChildPos = 0;
            int direction = targetPosition < firstChildPos != ReverseLayout ? -1 : 1;
            if (Orientation == Horizontal)
            {
                return new PointF(direction, 0);
            }
            else
            {
                return new PointF(0, direction);
            }
        }
    }
}