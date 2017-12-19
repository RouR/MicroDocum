# MicroDocum

[![Build status](https://ci.appveyor.com/api/projects/status/2x7gb0ggy8qac5lw/branch/master?svg=true)](https://ci.appveyor.com/project/RouR/microdocum/branch/master)
[![AppVeyor tests](https://img.shields.io/appveyor/tests/RouR/microdocum.svg)](https://ci.appveyor.com/project/RouR/microdocum/build/tests)
[![Quality Gate](https://sonarcloud.io/api/badges/gate?key=microdocum)](https://sonarcloud.io/dashboard?id=microdocum)
[![Quality Gate](https://sonarcloud.io/api/badges/measure?key=microdocum&metric=code_smells)](https://sonarcloud.io/dashboard?id=microdocum)
[![Quality Gate](https://sonarcloud.io/api/badges/measure?key=microdocum&metric=bugs)](https://sonarcloud.io/dashboard?id=microdocum)
[![Quality Gate](https://sonarcloud.io/api/badges/measure?key=microdocum&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=microdocum)
[![Quality Gate](https://sonarcloud.io/api/badges/measure?key=microdocum&metric=sqale_debt_ratio)](https://sonarcloud.io/dashboard?id=microdocum)
[![NuGet](https://img.shields.io/nuget/v/MicroDocum.Analyzers.svg)](https://www.nuget.org/packages/MicroDocum.Analyzers/)
[![NuGet](https://img.shields.io/nuget/v/MicroDocum.Graphviz.svg)](https://www.nuget.org/packages/MicroDocum.Graphviz/)
[![NuGet](https://img.shields.io/nuget/v/MicroDocum.Themes.svg)](https://www.nuget.org/packages/MicroDocum.Themes/)

[comment]: # (https://github.com/QualInsight/qualinsight-plugins-sonarqube-badges/wiki/Measure-badges)

Tool for automatic generation of documentation in the form of images. 
Using Reflections. Find all the classes, which have a custom attribute. Get their links in the form of a graph. Generate file for GraphViz. 
Visualize your links between DTO. Customize colors and labels.

# Install

# Getting started

1. Implement your theme (IGraphvizTheme<T>) or get DefaultTheme (IGraphvizTheme<DefaultLinkStyle>) from MicroDocum.Themes.DefaultTheme

2. Mark your DTO with your custom attributes and custom interfaces

```
[LabelAlt("altLabel")]
[Tags("someTag")]
[TagsAlt("altTag")]
[TTL(10)]
[ServiceName("Test")]
public class Struct1 : IProduce<IInterface1>
{
}
```

3. Analize your assembly
```
var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
var asm = AppDomain.CurrentDomain.GetAssemblies();
var graph = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(_classname) ?? false);
```

4. Generate Graphviz file
```
var graphwizFileData = new GraphvizDotGenerator<DefaultLinkStyle>(theme);
```

5. Visualize by online tool  https://dreampuf.github.io/GraphvizOnline/ or generate image locally 
```
Install-Package GraphViz.NET
Install-Package GraphViz
```

```
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;

var getStartProcessQuery = new GetStartProcessQuery();
var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
var wrapper = new GraphGeneration(getStartProcessQuery,
    getProcessStartInfoQuery,
    registerLayoutPluginCommand);
byte[] bytes = wrapper.GenerateGraph(graphwizFileData, GraphVizWrapper.Enums.GraphReturnType.Png);
SaveResultImage(bytes, $"./{TestContext.CurrentContext.Test.FullName}.png");
```
