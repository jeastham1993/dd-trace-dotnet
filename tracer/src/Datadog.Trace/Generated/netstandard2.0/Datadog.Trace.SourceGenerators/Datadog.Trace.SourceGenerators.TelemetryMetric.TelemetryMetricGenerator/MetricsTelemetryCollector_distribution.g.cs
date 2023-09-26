﻿// <auto-generated/>
#nullable enable

using System.Threading;

namespace Datadog.Trace.Telemetry;
internal partial class MetricsTelemetryCollector
{
    public void RecordDistributionInitTime(Datadog.Trace.Telemetry.Metrics.MetricTags.InitializationComponent tag, double value)
    {
        var index = 0 + (int)tag;
        _buffer.Distribution[index].TryEnqueue(value);
    }

    public void RecordDistributionCIVisibilityEndpointPayloadBytes(Datadog.Trace.Telemetry.Metrics.MetricTags.CIVisibilityEndpoints tag, double value)
    {
        var index = 14 + (int)tag;
        _buffer.Distribution[index].TryEnqueue(value);
    }

    public void RecordDistributionCIVisibilityEndpointPayloadRequestsMs(Datadog.Trace.Telemetry.Metrics.MetricTags.CIVisibilityEndpoints tag, double value)
    {
        var index = 16 + (int)tag;
        _buffer.Distribution[index].TryEnqueue(value);
    }

    public void RecordDistributionCIVisibilityEndpointPayloadEventsCount(Datadog.Trace.Telemetry.Metrics.MetricTags.CIVisibilityEndpoints tag, double value)
    {
        var index = 18 + (int)tag;
        _buffer.Distribution[index].TryEnqueue(value);
    }

    public void RecordDistributionCIVisibilityEndpointEventsSerializationMs(Datadog.Trace.Telemetry.Metrics.MetricTags.CIVisibilityEndpoints tag, double value)
    {
        var index = 20 + (int)tag;
        _buffer.Distribution[index].TryEnqueue(value);
    }

    public void RecordDistributionCIVisibilityGitCommandMs(Datadog.Trace.Telemetry.Metrics.MetricTags.CIVisibilityCommands tag, double value)
    {
        var index = 22 + (int)tag;
        _buffer.Distribution[index].TryEnqueue(value);
    }

    public void RecordDistributionCIVisibilityGitRequestsSearchCommitsMs(double value)
    {
        _buffer.Distribution[31].TryEnqueue(value);
    }

    public void RecordDistributionCIVisibilityGitRequestsObjectsPackMs(double value)
    {
        _buffer.Distribution[32].TryEnqueue(value);
    }

    public void RecordDistributionCIVisibilityGitRequestsObjectsPackBytes(double value)
    {
        _buffer.Distribution[33].TryEnqueue(value);
    }

    public void RecordDistributionCIVisibilityGitRequestsObjectsPackFiles(double value)
    {
        _buffer.Distribution[34].TryEnqueue(value);
    }

    public void RecordDistributionCIVisibilityGitRequestsSettingsMs(double value)
    {
        _buffer.Distribution[35].TryEnqueue(value);
    }

    public void RecordDistributionCIVisibilityITRSkippableTestsRequestMs(double value)
    {
        _buffer.Distribution[36].TryEnqueue(value);
    }

    public void RecordDistributionCIVisibilityITRSkippableTestsResponseBytes(double value)
    {
        _buffer.Distribution[37].TryEnqueue(value);
    }

    public void RecordDistributionCIVisibilityCodeCoverageFiles(double value)
    {
        _buffer.Distribution[38].TryEnqueue(value);
    }
}