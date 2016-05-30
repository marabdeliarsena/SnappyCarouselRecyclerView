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
using System.Threading.Tasks;
using Android.Util;
using System.Timers;
using Android.Graphics;
using System.Diagnostics;
using Android.Views.Animations;
using Android;

namespace SnappyCarouselRecyclerView
{

    public class RecyclerAdapter : RecyclerView.Adapter
    {
        private const int VIEW_TYPE_PADDING = 1;
        private const int VIEW_TYPE_ITEM = 2;

        private List<string> _greetings;
        private CustomRecyclerView mRecyclerView;

        int flingCounter;
        ProgressDialog _progressDialog;

        public RecyclerAdapter(CustomRecyclerView recyclerView, List<string> greetings)
        {
            _greetings = greetings;

            flingCounter = 0;
            mRecyclerView = recyclerView;
            mRecyclerView.CurrentItem = 0;
            mRecyclerView.OnFling += OnRecyclerViewFling;
            mRecyclerView.BeforeFling += BeforeRecyclerViewFling;
        }

        #region RecyclerView Controls

        private void BeforeRecyclerViewFling(object sender, FlingEventArgs e)
        {
            
        }

        private void OnRecyclerViewFling(object sender, FlingEventArgs e)
        {

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            if (viewType == VIEW_TYPE_ITEM)
            {
                View raw = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.RecyclerViewItemLayout, parent, false);

                TextView txtHello = raw.FindViewById<TextView>(Resource.Id.txtHello);

                RecyclerViewHolder view = new RecyclerViewHolder(raw)
                    {
                        TxtHello = txtHello
                    };
                return view;
            }
            else
            {
                View row = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.list_item_padding, parent, false);
                return new PaddingViewHolder(row);
            }

        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (GetItemViewType(position) == VIEW_TYPE_ITEM)
            {
                int indexPosition = position - 1;

                ((RecyclerViewHolder)holder).TxtHello.Text = string.Format("{0}. {1}", indexPosition, _greetings[indexPosition]);
            }
        }

        public override int ItemCount
        {
            get { return _greetings.Count + 2; } // +2 is for padding views 
        }

        public override int GetItemViewType(int position)
        {
            if (position == 0 || position == ItemCount - 1)
            {
                return VIEW_TYPE_PADDING;
            }
            return VIEW_TYPE_ITEM;
        }

        #endregion
    }

    public class RecyclerViewHolder : RecyclerView.ViewHolder
    {
        public TextView TxtHello { get; set; }

        public RecyclerViewHolder(View view) : base(view)
        {            
        }
    }

    public class PaddingViewHolder : RecyclerView.ViewHolder
    {
        public PaddingViewHolder(View v) : base(v)
        {

        }
    }
}