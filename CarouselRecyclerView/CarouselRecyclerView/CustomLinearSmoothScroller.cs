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

namespace com.arsena.SnappyCarouselRecyclerView
{
    public class CustomLinearSmoothScroller : LinearSmoothScroller
    {
        SnappyLinearLayoutManager _manager;

        public void SetManager(SnappyLinearLayoutManager manager)
        {
            _manager = manager;
        }

        public CustomLinearSmoothScroller(Context context)
            : base(context)
        {

        }
        // I want a behavior where the scrolling always snaps to the beginning of 
        // the list. Snapping to end is also trivial given the default implementation. 
        // If you need a different behavior, you may need to override more
        // of the LinearSmoothScrolling methods.
        protected override int HorizontalSnapPreference
        {
            get
            {
                return LinearSmoothScroller.SnapToStart;
            }
        }

        protected override int VerticalSnapPreference
        {
            get
            {
                return LinearSmoothScroller.SnapToStart;
            }
        }

        public override Android.Graphics.PointF ComputeScrollVectorForPosition(int targetPosition)
        {
            return _manager.ComputeScrollVectorForPosition(targetPosition);
        }
    }
}