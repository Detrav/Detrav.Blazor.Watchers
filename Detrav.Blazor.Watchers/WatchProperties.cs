using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace Detrav.Blazor.Watchers
{
    public class WatchProperties : Watcher
    {
        private int throttle;
        private INotifyPropertyChanged viewModel;
        private IDisposable disposableSubscription;

        /// <summary>
        /// List of properties for subscribe changes, if not set, all properties will be checked
        /// </summary>
        [Parameter]
        public string[] Properties { get; set; }


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
               || Properties == null
               || Properties.Contains(m.EventArgs.PropertyName)
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
