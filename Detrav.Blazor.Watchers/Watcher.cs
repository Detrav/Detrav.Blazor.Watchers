using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;

namespace Detrav.Blazor.Watchers
{
    public class Watcher : ComponentBase, IDisposable
    {
        
        private MethodInfo parentStateHasChanged = typeof(ComponentBase).GetMethod(nameof(StateHasChanged), BindingFlags.Instance | BindingFlags.NonPublic);

        [Parameter]
        public ComponentBase For { get; set; }

        protected void ParentStateHasChanged()
        {
            if (For != null)
            {
                parentStateHasChanged.Invoke(For, null);
            }
        }

        protected virtual void Dispose(bool disposing)
        {

        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
