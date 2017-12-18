using System;
using System.Configuration;
using System.IO;
using NUnit.Framework;
using GraphVizWrapper;
using GraphVizWrapper.Commands;
using GraphVizWrapper.Queries;

namespace MicroDocum.Graphviz.Tests
{
    [TestFixture]
    public class GraphVizWrapperTests
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var dir = Path.GetDirectoryName(typeof(GraphVizWrapperTests).Assembly.Location);
            if (dir != null)
                Environment.CurrentDirectory = dir;
            Console.WriteLine(Directory.GetCurrentDirectory());
        }


        private static GraphGeneration GetWrapper()
        {
            var getStartProcessQuery = new GetStartProcessQuery();
            var getProcessStartInfoQuery = new GetProcessStartInfoQuery();
            var registerLayoutPluginCommand = new RegisterLayoutPluginCommand(getProcessStartInfoQuery, getStartProcessQuery);
            var wrapper = new GraphGeneration(getStartProcessQuery,
                getProcessStartInfoQuery,
                registerLayoutPluginCommand);
            return wrapper;
        }

        private static void SaveResultImage(byte[] image, string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                stream.Write(image, 0, image.Length);
                Console.WriteLine("saved to " + stream.Name);
            }
        }

        protected void DrawAnSave(string graphwizFileData)
        {
            Assert.That(string.IsNullOrWhiteSpace(graphwizFileData) == false);
            var wrapper = GetWrapper();
            byte[] bytes = wrapper.GenerateGraph(graphwizFileData, GraphVizWrapper.Enums.GraphReturnType.Png);
            Assert.That(bytes.Length > 0, "Not valid graphviz file - it can`t be parsed.");
            SaveResultImage(bytes, $"./{TestContext.CurrentContext.Test.FullName}.png");
        }


        [Test]
        public void GraphVizWrapper_Should_GenerateSample()
        {
            Assume.That(File.Exists(ConfigurationManager.AppSettings["graphVizLocation"]+"dot.exe"), "ReSharper Unit Test not run in bin directory OR Graphviz not installed");
            //Given
            var defaultImage1 = "digraph{a -> b; b -> c; c -> a;}";
            var defaultImage2 = "digraph G {\n\n  subgraph cluster_0 {\n    style=filled;\n    color=lightgrey;\n    node [style=filled,color=white];\n    a0 -> a1 -> a2 -> a3;\n    label = \"process #1\";\n  }\n\n  subgraph cluster_1 {\n    node [style=filled];\n    b0 -> b1 -> b2 -> b3;\n    label = \"process #2\";\n    color=blue\n  }\n  start -> a0;\n  start -> b0;\n  a1 -> b3;\n  b2 -> a3;\n  a3 -> a0;\n  a3 -> end;\n  b3 -> end;\n\n  start [shape=Mdiamond];\n  end [shape=Msquare];\n}";
            //When
            var wrapper = GetWrapper();

            byte[] output1 = wrapper.GenerateGraph(defaultImage1, GraphVizWrapper.Enums.GraphReturnType.Png);
            byte[] output2 = wrapper.GenerateGraph(defaultImage2, GraphVizWrapper.Enums.GraphReturnType.Jpg);
            //Then
            Assert.NotNull(output1);
            Assert.NotNull(output2);
            SaveResultImage(output1, "./1.png");
            SaveResultImage(output2, "./2.jpg");
        }

        
    }
}
