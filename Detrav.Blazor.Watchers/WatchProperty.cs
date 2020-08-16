using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Text;

namespace Detrav.Blazor.Watchers
{
    public class WatchProperty : Watcher
    {
        private int throttle;
        private INotifyPropertyChanged viewModel;
        private IDisposable disposableSubscription;

        /// <summary>
        /// Property name changing subscribe, if not set, will check all properties
        /// </summary>
        [Parameter]
        public string Property { get; set; }


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


        /// <summary>
        /// View model for subscribe changes
        /// </summary>
        [Parameter]
        public INotifyPropertyChanged Model
        {
            get => viewModel;
            set => SubscribeNewModel(value, throttle);
        }


        private void SubscribeNewModel(INotifyPropertyChanged viewModel, int throttle)
        {
            if (EqualityComparer<INotifyPropertyChanged>.Default.Equals(this.viewModel, viewModel)
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
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    h => viewModel.PropertyChanged += h,
                    h => viewModel.PropertyChanged -= h);

            observable = observable.Where(m =>
               m.EventArgs.PropertyName == null
               || Property == null
               || m.EventArgs.PropertyName == Property
            );

            if (throttle > 0)
            {
                observable = observable.Throttle(TimeSpan.FromMilliseconds(throttle));
            }

            observable = observable.Do(_ => ParentStateHasChanged());

            disposableSubscription = observable.Subscribe();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            disposableSubscription?.Dispose();
        }
    }
}
