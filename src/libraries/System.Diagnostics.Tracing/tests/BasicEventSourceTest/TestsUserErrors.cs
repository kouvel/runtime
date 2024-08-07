// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Collections.Generic;
using Xunit;

namespace BasicEventSourceTests
{
    /// <summary>
    /// Tests the user experience for common user errors.
    /// </summary>
    public partial class TestsUserErrors
    {
        /// <summary>
        /// Try to pass a user defined class (even with EventData)
        /// to a manifest based eventSource
        /// </summary>
        [Fact]
        public void Test_BadTypes_Manifest_UserClass()
        {
            var badEventSource = new BadEventSource_Bad_Type_UserClass();
            Test_BadTypes_Manifest(badEventSource);
        }

        private void Test_BadTypes_Manifest(EventSource source)
        {
            try
            {
                using (var listener = new EventListenerListener())
                {
                    var events = new List<Event>();
                    Debug.WriteLine("Adding delegate to onevent");
                    listener.OnEvent = delegate (Event data) { events.Add(data); };

                    listener.EventSourceCommand(source.Name, EventCommand.Enable);

                    listener.Dispose();

                    // Confirm that we get exactly one event from this whole process, that has the error message we expect.
                    Assert.Equal(1, events.Count);
                    Event _event = events[0];
                    Assert.Equal("EventSourceMessage", _event.EventName);

                    string message = _event.PayloadString(0, "message");
                    // expected message: "ERROR: Exception in Command Processing for EventSource BadEventSource_Bad_Type_ByteArray: Unsupported type Byte[] in event source. "
                    Assert.Contains("Unsupported type", message);
                }
            }
            finally
            {
                source.Dispose();
            }
        }

        /// <summary>
        /// Test the
        /// </summary>
        [ConditionalFact(typeof(PlatformDetection), nameof(PlatformDetection.IsNotWindowsNanoServer))] // ActiveIssue: https://github.com/dotnet/runtime/issues/26197
        public void Test_BadEventSource_MismatchedIds()
        {
            TestUtilities.CheckNoEventSourcesRunning("Start");
            var onStartups = new bool[] { false, true };

            var listenerGenerators = new List<Func<Listener>>();
            listenerGenerators.Add(() => new EventListenerListener());

            var settings = new EventSourceSettings[] { EventSourceSettings.Default, EventSourceSettings.EtwSelfDescribingEventFormat };

            // For every interesting combination, run the test and see that we get a nice failure message.
            foreach (bool onStartup in onStartups)
            {
                foreach (Func<Listener> listenerGenerator in listenerGenerators)
                {
                    foreach (EventSourceSettings setting in settings)
                    {
                        Test_Bad_EventSource_Startup(onStartup, listenerGenerator(), setting);
                    }
                }
            }

            TestUtilities.CheckNoEventSourcesRunning("Stop");
        }

        /// <summary>
        /// A helper that can run the test under a variety of conditions
        /// * Whether the eventSource is enabled at startup
        /// * Whether the listener is ETW or an EventListern
        /// * Whether the ETW output is self describing or not.
        /// </summary>
        private void Test_Bad_EventSource_Startup(bool onStartup, Listener listener, EventSourceSettings settings)
        {
            var eventSourceName = typeof(BadEventSource_MismatchedIds).Name;
            Debug.WriteLine("***** Test_BadEventSource_Startup(OnStartUp: " + onStartup + " Listener: " + listener + " Settings: " + settings + ")");

            // Activate the source before the source exists (if told to).
            if (onStartup)
                listener.EventSourceCommand(eventSourceName, EventCommand.Enable);

            var events = new List<Event>();
            listener.OnEvent = delegate (Event data) { events.Add(data); };

            using (var source = new BadEventSource_MismatchedIds(settings))
            {
                Assert.Equal(eventSourceName, source.Name);
                // activate the source after the source exists (if told to).
                if (!onStartup)
                    listener.EventSourceCommand(eventSourceName, EventCommand.Enable);
                source.Event1(1);       // Try to send something.
            }
            listener.Dispose();

            // Confirm that we get exactly one event from this whole process, that has the error message we expect.
            Assert.Equal(1, events.Count);
            Event _event = events[0];
            Assert.Equal("EventSourceMessage", _event.EventName);
            string message = _event.PayloadString(0, "message");
            Debug.WriteLine(string.Format("Message=\"{0}\"", message));
            // expected message: "ERROR: Exception in Command Processing for EventSource BadEventSource_MismatchedIds: Event Event2 was assigned event ID 2 but 1 was passed to WriteEvent. "
            if (!PlatformDetection.IsNetFramework) // .NET Framework has typo
                Assert.Contains("Event Event2 was assigned event ID 2 but 1 was passed to WriteEvent", message);

            // Validate the details of the EventWrittenEventArgs object
            if (_event is EventListenerListener.EventListenerEvent elEvent)
            {
                EventWrittenEventArgs ea = elEvent.Data;
                Assert.NotNull(ea);
                Assert.Equal(EventSource.CurrentThreadActivityId, ea.ActivityId);
                Assert.Equal(EventChannel.None, ea.Channel);
                Assert.Equal(0, ea.EventId);
                Assert.Equal("EventSourceMessage", ea.EventName);
                Assert.NotNull(ea.EventSource);
                Assert.Equal(EventKeywords.None, ea.Keywords);
                Assert.Equal(EventLevel.LogAlways, ea.Level);
                Assert.Equal((EventOpcode)0, ea.Opcode);
                Assert.NotNull(ea.Payload);
                Assert.NotNull(ea.PayloadNames);
                Assert.Equal(ea.PayloadNames.Count, ea.Payload.Count);
                Assert.Equal(Guid.Empty, ea.RelatedActivityId);
                Assert.Equal(EventTags.None, ea.Tags);
                Assert.Equal(EventTask.None, ea.Task);
                Assert.InRange(ea.TimeStamp, DateTime.MinValue, DateTime.MaxValue);
                Assert.Equal(0, ea.Version);
            }
        }

        [ActiveIssue("https://github.com/dotnet/runtime/issues/105293")]
        [Fact]
        public void Test_Bad_WriteRelatedID_ParameterName()
        {
            Guid oldGuid;
            Guid newGuid = Guid.NewGuid();
            Guid newGuid2 = Guid.NewGuid();
            EventSource.SetCurrentThreadActivityId(newGuid, out oldGuid);

            using (var bes = new BadEventSource_IncorrectWriteRelatedActivityIDFirstParameter())
            using (var listener = new EventListenerListener())
            {
                var events = new List<Event>();
                listener.OnEvent = delegate (Event data) { events.Add(data); };

                listener.EventSourceCommand(bes.Name, EventCommand.Enable);

                bes.RelatedActivity(newGuid2, "Hello", 42, "AA", "BB");

                // Confirm that we get exactly one event from this whole process, that has the error message we expect.
                Assert.Equal(1, events.Count);
                Event _event = events[0];
                Assert.Equal("EventSourceMessage", _event.EventName);
                string message = _event.PayloadString(0, "message");
                // expected message: "EventSource expects the first parameter of the Event method to be of type Guid and to be named "relatedActivityId" when calling WriteEventWithRelatedActivityId."
                Assert.Contains("EventSource expects the first parameter of the Event method to be of type Guid and to be named \"relatedActivityId\" when calling WriteEventWithRelatedActivityId.", message);
            }
        }
    }

    /// <summary>
    /// This EventSource has a common user error, and we want to make sure EventSource
    /// gives a reasonable experience in that case.
    /// </summary>
    internal class BadEventSource_MismatchedIds : EventSource
    {
        public BadEventSource_MismatchedIds(EventSourceSettings settings) : base(settings) { }
        public void Event1(int arg) { WriteEvent(1, arg); }
        // Error Used the same event ID for this event.
        public void Event2(int arg) { WriteEvent(1, arg); }
    }

    public sealed class BadEventSource_IncorrectWriteRelatedActivityIDFirstParameter : EventSource
    {
        public void E2()
        {
            this.Write("sampleevent", new { a = "a string" });
        }

        [Event(7, Keywords = Keywords.Debug, Message = "Hello Message 7", Channel = EventChannel.Admin, Opcode = EventOpcode.Send)]

        public void RelatedActivity(Guid guid, string message, int value, string componentName, string instanceId)
        {
            WriteEventWithRelatedActivityId(7, guid, message, value, componentName, instanceId);
        }

        public class Keywords
        {
            public const EventKeywords Debug = (EventKeywords)0x0002;
        }
    }

    [EventData]
    public class UserClass
    {
        public int i;
    };

    /// <summary>
    /// A manifest based provider with a bad type (only supported in self describing)
    /// </summary>
    internal class BadEventSource_Bad_Type_UserClass : EventSource
    {
        public void Event1(UserClass myClass) { WriteEvent(1, myClass); }
    }
}
