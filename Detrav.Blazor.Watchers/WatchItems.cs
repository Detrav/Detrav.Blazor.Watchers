using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive.Linq;

namespace Detrav.Blazor.Watchers
{
    public class WatchItems : Watcher
    {
        private int throttle;
        private INotifyCollectionChanged viewModel;
        private IDisposable disposableSubscription;

        [Parameter]
        public INotifyCollectionChanged Source
        {
            get => viewModel;
            set => SubscribeNewModel(value, throttle);
        }


        private void SubscribeNewModel(INotifyCollectionChanged viewModel, int throttle)
        {
            if (EqualityComparer<INotifyCollectionChanged>.Default.Equals(this.viewModel, viewModel)
                && this.throttle == throttle)
                return;

            if (disposableSubscription != null)
            {
                disposableSubscription.Dispose();
                disposableSubscription = null;
            }

            this.viewModel = viewModel;
            this.throttle = throttle;

            var observable = Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                    h => viewModel.CollectionChanged += h,
                    h => viewModel.CollectionChanged -= h);

            //observable = observable.Where(m =>
            //   m.EventArgs.PropertyName == null
            //   || Properties == null
            //   || Properties.Contains(m.EventArgs.PropertyName)
            //);

            if (throttle > 0)
            {
                observable = observable.Throttle(TimeSpan.FromMilliseconds(throttle));
            }

            observable = observable.Do(_ => ParentStateHasChanged());

            disposableSubscription = observable.Subscribe();
        }


        /// <summary>
        /// Change throttle of viewmodel, set throttle = 0 for disable, (ms)<br/>
        /// By default: is disabled
        /// </summary
        [Parameter]
        public int Throttle
        {
            get => throttle;
            set => SubscribeNewModel(viewModel, value);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            disposableSubscription?.Dispose();
        }
    }
}
