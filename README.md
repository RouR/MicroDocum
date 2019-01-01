# MicroDocum

[![Build status](https://ci.appveyor.com/api/projects/status/2x7gb0ggy8qac5lw/branch/master?svg=true)](https://ci.appveyor.com/project/RouR/microdocum/branch/master)
[![AppVeyor tests](https://img.shields.io/appveyor/tests/RouR/microdocum.svg)](https://ci.appveyor.com/project/RouR/microdocum/build/tests)

[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=microdocum&metric=code_smells)](https://sonarcloud.io/dashboard?id=microdocum)
[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=microdocum&metric=bugs)](https://sonarcloud.io/dashboard?id=microdocum)
[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=microdocum&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=microdocum)
[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=microdocum&metric=security_rating)](https://sonarcloud.io/dashboard?id=microdocum)
[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=microdocum&metric=sqale_index)](https://sonarcloud.io/dashboard?id=microdocum)
[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=microdocum&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=microdocum)

[![NuGet](https://img.shields.io/nuget/v/MicroDocum.Analyzers.svg)](https://www.nuget.org/packages/MicroDocum.Analyzers/) MicroDocum.Analyzers 

[![NuGet](https://img.shields.io/nuget/v/MicroDocum.Graphviz.svg)](https://www.nuget.org/packages/MicroDocum.Graphviz/) MicroDocum.Graphviz 

[![NuGet](https://img.shields.io/nuget/v/MicroDocum.Themes.svg)](https://www.nuget.org/packages/MicroDocum.Themes/) MicroDocum.Themes 

[comment]: # (https://github.com/QualInsight/qualinsight-plugins-sonarqube-badges/wiki/Measure-badges)

Tool for automatic generation of documentation in the form of images. 
Using Reflections. Find all the classes, which have a custom attribute. Get their links in the form of a graph. Generate file for GraphViz. 
Visualize your links between DTO. Customize colors and labels.

# Install

# Getting started

1. Implement your theme (IGraphvizTheme<T>) or get DefaultTheme (IGraphvizTheme<DefaultLinkStyle>) from MicroDocum.Themes.DefaultTheme

2. Mark your DTO with your custom attributes and custom interfaces

```cs
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
```cs
var a = new AssemblyAnalizer<DefaultLinkStyle>(theme);
var asm = AppDomain.CurrentDomain.GetAssemblies();
var graph = a.Analize(asm, theme.GetAvailableThemeAttributes(), t => t.FullName?.StartsWith(_classname) ?? false);
```

4. Generate Graphviz file
```cs
var graphwizFileData = new GraphvizDotGenerator<DefaultLinkStyle>(theme);
```

5. Visualize by online tool  https://dreampuf.github.io/GraphvizOnline/ or generate image locally 
```PowerShell
Install-Package GraphViz.NET
Install-Package GraphViz
```

```cs
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

# Example
https://github.com/RouR/ToDo-ToBuy/blob/ff367c92ce21d1bf9ebea40438965d6fa1c9d23d/build/Microdocum.cs#L37
https://github.com/RouR/ToDo-ToBuy/blob/ff367c92ce21d1bf9ebea40438965d6fa1c9d23d/DTO_routing.png
![example](https://raw.githubusercontent.com/RouR/ToDo-ToBuy/ff367c92ce21d1bf9ebea40438965d6fa1c9d23d/DTO_routing.png)
