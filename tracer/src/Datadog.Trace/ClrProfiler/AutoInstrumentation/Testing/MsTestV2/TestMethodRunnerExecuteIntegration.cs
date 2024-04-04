// <copyright file="TestMethodRunnerExecuteIntegration.cs" company="Datadog">
// Unless explicitly stated otherwise all files in this repository are licensed under the Apache 2 License.
// This product includes software developed at Datadog (https://www.datadoghq.com/). Copyright 2017 Datadog, Inc.
// </copyright>
#nullable enable

using System;
using System.Collections;
using System.ComponentModel;
using Datadog.Trace.Ci;
using Datadog.Trace.ClrProfiler.CallTarget;
using Datadog.Trace.DuckTyping;

namespace Datadog.Trace.ClrProfiler.AutoInstrumentation.Testing.MsTestV2;

/// <summary>
/// Microsoft.VisualStudio.TestPlatform.MSTest.TestAdapter.Execution.TestMethodRunner.Execute calltarget instrumentation
/// </summary>
[InstrumentMethod(
    AssemblyName = "Microsoft.VisualStudio.TestPlatform.MSTest.TestAdapter",
    TypeName = "Microsoft.VisualStudio.TestPlatform.MSTest.TestAdapter.Execution.TestMethodRunner",
    MethodName = "Execute",
    ReturnTypeName = "Microsoft.VisualStudio.TestPlatform.MSTest.TestAdapter.ObjectModel.UnitTestResult[]",
    MinimumVersion = "14.0.0",
    MaximumVersion = "14.*.*",
    IntegrationName = MsTestIntegration.IntegrationName)]
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class TestMethodRunnerExecuteIntegration
{
    /// <summary>
    /// OnMethodEnd callback
    /// </summary>
    /// <typeparam name="TTarget">Type of the target</typeparam>
    /// <typeparam name="TReturn">Type of the return value</typeparam>
    /// <param name="instance">Instance value, aka `this` of the instrumented method.</param>
    /// <param name="returnValue">Return value</param>
    /// <param name="exception">Exception instance in case the original code threw an exception.</param>
    /// <param name="state">Calltarget state value</param>
    /// <returns>A response value, in an async scenario will be T of Task of T</returns>
    internal static CallTargetReturn<TReturn> OnMethodEnd<TTarget, TReturn>(TTarget instance, TReturn returnValue, Exception exception, in CallTargetState state)
        where TTarget : ITestMethodRunner
    {
        if (!MsTestIntegration.IsEnabled)
        {
            return new CallTargetReturn<TReturn>(returnValue);
        }

        if (returnValue is IList { Count: > 0 } lstResults)
        {
            foreach (var unitTestResultObject in lstResults)
            {
                if (unitTestResultObject.TryDuckCast<UnitTestResultStruct>(out var unitTestResult))
                {
                    if (unitTestResult.Outcome is UnitTestResultOutcome.Inconclusive or UnitTestResultOutcome.NotRunnable or UnitTestResultOutcome.Ignored)
                    {
                        if (!MsTestIntegration.ShouldSkip(instance.TestMethodInfo, out _, out _))
                        {
                            // This instrumentation catches all tests being ignored
                            MsTestIntegration.OnMethodBegin(instance.TestMethodInfo, instance.GetType())?
                               .Close(TestStatus.Skip, TimeSpan.Zero, unitTestResult.ErrorMessage);
                        }
                    }
                    else if (unitTestResult.Outcome is UnitTestResultOutcome.Error or UnitTestResultOutcome.Failed)
                    {
                        // We need to check if the test is failing because a Class initialization error
                        if (instance.TestMethodInfo.Parent.Instance.TryDuckCast<ClassInfoExceptionsStruct>(out var classInfoExceptionsStruct))
                        {
                            if (classInfoExceptionsStruct.ClassInitializationException is { } classInitializationException &&
                                MsTestIntegration.OnMethodBegin(instance.TestMethodInfo, instance.GetType()) is { } test)
                            {
                                test.SetErrorInfo(classInitializationException);
                                test.Close(TestStatus.Fail);
                            }

                            if (classInfoExceptionsStruct.ClassCleanupException is { } classCleanupException &&
                                MsTestIntegration.GetOrCreateTestSuiteFromTestClassInfo(instance.TestMethodInfo.Parent) is { } suite)
                            {
                                suite.SetErrorInfo(classCleanupException);
                            }
                        }

                        // We need to check if the test is failing because a Assembly initialization error
                        if (instance.TestMethodInfo.Parent.Parent.Instance.TryDuckCast<AssemblyInfoExceptionsStruct>(out var assemblyInfoExceptionsStruct))
                        {
                            if (assemblyInfoExceptionsStruct.AssemblyInitializationException is { } assemblyInitializationException &&
                                MsTestIntegration.OnMethodBegin(instance.TestMethodInfo, instance.GetType()) is { } test)
                            {
                                test.SetErrorInfo(assemblyInitializationException);
                                test.Close(TestStatus.Fail);
                            }
                        }
                    }
                }
            }
        }

        return new CallTargetReturn<TReturn>(returnValue);
    }

    [DuckCopy]
    internal struct ClassInfoExceptionsStruct
    {
        public Exception? ClassInitializationException;
        public Exception? ClassCleanupException;
    }

    [DuckCopy]
    internal struct AssemblyInfoExceptionsStruct
    {
        public Exception? AssemblyInitializationException;
    }
}
