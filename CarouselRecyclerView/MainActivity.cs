using Android.App;
using Android.Widget;
using Android.OS;
using com.arsena.SnappyCarouselRecyclerView;
using Android.Views;
using Android.Graphics;
using Android.Support.V7.Widget;
using System.Collections.Generic;

namespace SnappyCarouselRecyclerView
{
    [Activity(Label = "CarouselRecyclerView", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            var mRecyclerView = FindViewById<CustomRecyclerView>(Resource.Id.recyclerView);

            Display display = WindowManager.DefaultDisplay;
            Point size = new Point();
            display.GetSize(size);
            int width = size.X;
            int height = size.Y;

            mRecyclerView.SetScreenWidth(width);
            //mRecyclerView.OnFling += OnRecyclerViewFling;

            var mLayoutManager = new SnappyLinearLayoutManager(this, LinearLayoutManager.Horizontal, false);
            mRecyclerView.HorizontalFadingEdgeEnabled = true;
            mRecyclerView.SetFadingEdgeLength(60);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            List<string> greetings = new List<string>
            {
                "Hi",
                "Hello",
                "Wazzup",
                "Ola",
                "Omana",
                "Sigariga"
            };

            mRecyclerView.ItemCount = greetings.Count;
            var mAdapter = new RecyclerAdapter(mRecyclerView, greetings);
            mRecyclerView.SetAdapter(mAdapter);
        }
    }
}


