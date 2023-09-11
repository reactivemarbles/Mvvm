// Copyright (c) 2019-2023 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using NuGet.LibraryModel;
using NuGet.Versioning;
using ReactiveMarbles.NuGet.Helpers;
using ReactiveMarbles.SourceGenerator.TestNuGetHelper.Compilation;
using VerifyTests;
using VerifyXunit;
using Xunit.Abstractions;

namespace ReactiveMarbles.Mvvm.SourceGenerator.Tests
{
    public sealed class TestHelper : IDisposable
    {
#pragma warning disable CS0618 // Type or member is obsolete
        private static readonly LibraryRange _mvvmLibrary =
            new("ReactiveMarbles.Mvvm", VersionRange.AllStableFloating, LibraryDependencyTarget.Package);
#pragma warning restore CS0618 // Type or member is obsolete

        public TestHelper(ITestOutputHelper testOutput) =>
            TestOutputHelper = testOutput ?? throw new ArgumentNullException(nameof(testOutput));

        private EventBuilderCompiler? EventCompiler { get; set; }

        private ITestOutputHelper TestOutputHelper { get; }

        public async Task InitializeAsync()
        {
            var targetFrameworks = "netstandard2.0".ToFrameworks();

            var inputGroup = await NuGetPackageHelper
                                   .DownloadPackageFilesAndFolder(
                                       _mvvmLibrary,
                                       targetFrameworks,
                                       packageOutputDirectory: null)
                                   .ConfigureAwait(false);

            var framework = targetFrameworks[0];
            EventCompiler = new(inputGroup, inputGroup, framework);
        }

        public Task TestFail<TSourceGenerator>(
            string source,
            string contractParameter,
            Type callerType,
            [CallerFilePath] string file = "",
            [CallerMemberName] string memberName = "")
            where TSourceGenerator : ISourceGenerator, new()
        {
            if (EventCompiler is null)
            {
                throw new InvalidOperationException("Must have valid compiler instance.");
            }

            var utility = new SourceGeneratorUtility(x => TestOutputHelper.WriteLine(x));

            GeneratorDriver? driver = null;

            Assert.Throws<InvalidOperationException>(
                () => utility.RunGenerator<TSourceGenerator>(EventCompiler, out _, out _, out driver, source));

            return RunVerify(file, memberName, callerType, driver, contractParameter);
        }

        public Task TestPass<TSourceGenerator>(
            string source,
            string contractParameter,
            Type callerType,
            [CallerFilePath] string file = "",
            [CallerMemberName] string memberName = "")
            where TSourceGenerator : ISourceGenerator, new()
        {
            var driver = Generate<TSourceGenerator>(source);
            return RunVerify(file, memberName, callerType, driver, contractParameter);
        }

        public Task TestPass<TSourceGenerator>(
            string source,
            string contractParameter,
            LazyThreadSafetyMode mode,
            Type callerType,
            [CallerFilePath] string file = "",
            [CallerMemberName] string memberName = "")
        where TSourceGenerator : ISourceGenerator, new()
        {
            var driver = Generate<TSourceGenerator>(source);

            return RunVerify(file, memberName, callerType, driver, contractParameter, mode);
        }

        public void Dispose() => EventCompiler?.Dispose();

        private static Task RunVerify(
            string file,
            string callerMember,
            Type type,
            GeneratorDriver? driver,
            params object[] parameters)
        {
            var parametersString = string.Join("_", parameters);
            VerifySettings settings = new();
            settings.DisableRequireUniquePrefix();

            if (!string.IsNullOrWhiteSpace(parametersString))
            {
                settings.UseTextForParameters(parametersString);
            }

            settings.UseTypeName(type.Name);
            settings.UseMethodName(callerMember);
            return Verifier.Verify(driver, settings, file);
        }

        private GeneratorDriver Generate<TSourceGenerator>(string source)
            where TSourceGenerator : ISourceGenerator, new()
        {
            if (EventCompiler is null)
            {
                throw new InvalidOperationException("Must have valid compiler instance.");
            }

            var utility = new SourceGeneratorUtility(x => TestOutputHelper.WriteLine(x));

            utility.RunGenerator<TSourceGenerator>(EventCompiler, out _, out _, out var driver, source);

            return driver;
        }
    }
}
