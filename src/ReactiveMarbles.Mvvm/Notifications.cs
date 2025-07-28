// Copyright (c) 2019-2025 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

using DynamicData;

namespace ReactiveMarbles.Mvvm;

[SuppressMessage("StyleCop", "SA1401", Justification = "Deliberate use of private field")]
internal class Notifications
{
    public readonly SourceList<RxPropertyChangedEventArgs<IRxObject>> PropertyChangedEvents = new();
    public readonly SourceList<RxPropertyChangingEventArgs<IRxObject>> PropertyChangingEvents = new();
    public long ChangeNotificationsDelayed;
    public long ChangeNotificationsSuppressed;
}
