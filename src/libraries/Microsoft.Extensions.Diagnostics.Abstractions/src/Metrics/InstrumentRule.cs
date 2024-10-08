﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics.Metrics;

namespace Microsoft.Extensions.Diagnostics.Metrics
{
    /// <summary>
    /// Contains a set of parameters used to determine which instruments are enabled for which listeners. Unspecified
    /// parameters match anything.
    /// </summary>
    /// <remarks>
    /// <para>The most specific rule that matches a given instrument will be used. The priority of parameters is as follows:</para>
    /// <para>- MeterName, either an exact match, or the longest prefix match. See <see cref="Meter.Name">Meter.Name</see>.</para>
    /// <para>- InstrumentName, an exact match. <see cref="Instrument.Name">Instrument.Name</see>.</para>
    /// <para>- ListenerName, an exact match. <see cref="IMetricsListener.Name">IMetricsListener.Name</see>.</para>
    /// <para>- Scopes</para>
    /// </remarks>
    /// <param name="meterName">The <see cref="Meter.Name">Meter.Name</see> or prefix.</param>
    /// <param name="instrumentName">The <see cref="Instrument.Name">Instrument.Name</see>.</param>
    /// <param name="listenerName">The <see cref="IMetricsListener.Name">IMetricsListener.Name</see>.</param>
    /// <param name="scopes">A bitwise combination of the enumeration values that specifies the scopes to consider.</param>
    /// <param name="enable"><see langword="true" /> to enable the matched instrument for this listener; otherwise, <see langword="false" />.</param>
    public class InstrumentRule(string? meterName, string? instrumentName, string? listenerName, MeterScope scopes, bool enable)
    {
        /// <summary>
        /// Gets the <see cref="Meter.Name">Meter.Name</see>, either an exact match or the longest prefix match. Only full segment matches are considered.
        /// </summary>
        /// <value>
        /// The meter name. If <see langword="null" />, all meters are matched.
        /// </value>
        public string? MeterName { get; } = meterName;

        /// <summary>
        /// Gets the <see cref="Instrument.Name">Instrument.Name</see>, an exact match.
        /// </summary>
        /// <value>
        /// The instrument name. If <see langword="null" />, all instruments for the meter are matched.
        /// </value>
        public string? InstrumentName { get; } = instrumentName;

        /// <summary>
        /// Gets the <see cref="IMetricsListener.Name">IMetricsListener.Name</see>, an exact match.
        /// </summary>
        /// <value>
        /// The listener name. If <see langword="null" />, all listeners are matched.
        /// </value>
        public string? ListenerName { get; } = listenerName;

        /// <summary>
        /// Gets the <see cref="MeterScope"/>.
        /// </summary>
        /// <remarks>
        /// This property is used to distinguish between meters created via <see cref="Meter"/> constructors (<see cref="MeterScope.Global"/>)
        /// and those created via Dependency Injection with <see cref="IMeterFactory.Create(MeterOptions)"/> (<see cref="MeterScope.Local"/>).
        /// </remarks>
        public MeterScope Scopes { get; } = scopes == MeterScope.None
            ? throw new ArgumentOutOfRangeException(nameof(scopes), scopes, "The MeterScope must be Global, Local, or both.")
            : scopes;

        /// <summary>
        /// Gets a value that indicates whether the instrument should be enabled for the listener.
        /// </summary>
        public bool Enable { get; } = enable;
    }
}
