# Detrav.Blazor.Watchers

This is a library for tracking changes in the `INotifyPropertyChanged` properties and a items of` INotifyCollectionChanged` list. The goal is to add syntactic sugar to track changes in the View Model.

## DEMO

[Demo page](https://detrav.github.io/Detrav.Blazor.Watchers/Demo/) on the Github!

## Usage

To make the Blazor component track model changes, add a child watcher component to it:

```
<WatchProperty For="@this" Model="@User" Property="@nameof(UserVM.Name)" />
```

| Attribute | Type                   | Description                                                  |
| --------- | ---------------------- | ------------------------------------------------------------ |
| For       | Component              | This is the component for which properties are tracked.      |
| Model     | INotifyPropertyChanged | This is the View Model that watcher will subscribe to changes. |
| Property  | String                 | The name of the property to which the changes will be subscribed. If the property is not set, then the watcher will keep track of all properties. |
| Throttle  | Int32                  | The time that the watcher will wait before updating the component. If there are several changes during this period, the watcher will skip the first ones. |

```
<WatchProperties For="@this" Model="@User" Properties="new[] { nameof(UserVM.Name) }" />
```

| Attribute  | Type                   | Description                                                  |
| ---------- | ---------------------- | ------------------------------------------------------------ |
| For        | Component              | This is the component for which properties are tracked.      |
| Model      | INotifyPropertyChanged | This is the View Model that watcher will subscribe to changes. |
| Properties | String[]               | The names of the properties to which the changes will be subscribed. If the attribute is not set, then the watcher will keep track of all properties. |
| Throttle   | Int32                  | The time that the watcher will wait before updating the component. If there are several changes during this period, the watcher will skip the first ones. |

```
<WatchItems For="@this" Source="Users?.Items" />
```

| Attribute | Type                     | Description                                                  |
| --------- | ------------------------ | ------------------------------------------------------------ |
| For       | Component                | This is the component for which properties are tracked.      |
| Source    | INotifyCollectionChanged | This is the View Model that watcher will subscribe to changes. |
| Throttle  | Int32                    | The time that the watcher will wait before updating the component. If there are several changes during this period, the watcher will skip the first ones. |

## Be careful!

I believe this is the wrong way to component programming. When the child triggers changes (such as StateHasChanged) on the parent. I am in favor of unidirectional flow in component programming.

However, this approach is one of the fastest for writing a large number of working components with complex connections.

## PS

Without Nuget for now, copy the sources into your project! Fork me!

## License

Copyright by Detrav / Witaly Ezepchuk / Vitaliy Ezepchuk.

Licensed under the MIT license, see license text in LICENSE file.