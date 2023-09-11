// Copyright (c) 2019-2023 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;

namespace ReactiveMarbles.Mvvm.SourceGenerator.Tests;

public static class TestUtilities
{
    public static string GetTestPath(string relativePath)
    {
        var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().Location);
        var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
        var dirPath = Path.GetDirectoryName(codeBasePath);

        if (dirPath == null)
        {
            return string.Empty;
        }

        return Path.Combine(dirPath, "TestFiles", relativePath);
    }
}
