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

namespace com.arsena.SnappyCarouselRecyclerView
{
    interface ISnappyLayoutManager
    {
        /**
        * @param velocityX
        * @param velocityY
        * @return the resultant position from a fling of the given velocity.
        */
        int GetPositionForVelocity(int velocityX, int velocityY);

        /**
         * @return the position this list must scroll to to fix a state where the 
         * views are not snapped to grid.
         */
        int GetFixScrollPos();
    }
}