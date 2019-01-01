using System;
using System.Linq;
using MicroDocum.Analyzers.Models;
using NUnit.Framework;

namespace MicroDocum.Analyzers.Tests
{

    [TestFixture]
    public class Chain
    {
       

        [Test]
        public void Chain_Should_DetectSingles()
        {
            //Given
            var chain = new Chain<int,string>();
            //When
            chain.AddNode(new ChainNode<int>("1",1));
            chain.AddNode(new ChainNode<int>("2",2));
            //Then
            Assert.AreEqual(2, chain.GetSingles().Count);
            Assert.AreEqual(0, chain.GetHeads().Count);
            Assert.AreEqual(0, chain.GetLeafs().Count);
        }

        [Test]
        public void Chain_Should_DetectDupKey()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("1", 1));
            //When
            Assert.Throws<DuplicateNodeException>(() => chain.AddNode(new ChainNode<int>("1", 2)));
            Assert.Throws<DuplicateNodeException>(() => chain.AddNode(new ChainNode<int>("1", 1)));
        }

        [Test]
        public void Chain_Should_DetectNullNode()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("1", 1));
            //When
            Assert.Throws<ArgumentNullException>(() => chain.AddNode(null));
        }

        [Test]
        public void Chain_Should_DetectUnknownNodeEdge()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("1", 1));
            chain.AddNode(new ChainNode<int>("2", 2));
            //When
            var ex = Assert.Throws<UnknownNodeException>(() => chain.AddDirectedEdge(chain["1"], new ChainNode<int>("bad_key", 2),string.Empty));
            Assert.That(ex.Message.Contains("bad_key"));
        }

        [Test]
        public void Chain_Should_DetectHeads()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("1", 1));
            chain.AddNode(new ChainNode<int>("11", 11));
            chain.AddNode(new ChainNode<int>("21", 2));
            chain.AddNode(new ChainNode<int>("31", 2));
            chain.AddNode(new ChainNode<int>("031", 2));
            //When
            chain.AddDirectedEdge(chain["1"],chain["11"], string.Empty);
            chain.AddDirectedEdge(chain["21"],chain["11"], string.Empty);
            chain.AddDirectedEdge(chain["031"], chain["31"], string.Empty);
            chain.AddDirectedEdge(chain["31"],chain["11"], string.Empty);
            //Then
            Assert.AreEqual(3, chain.GetHeads().Count);
            Assert.AreEqual(0, chain.GetSingles().Count);
            Assert.AreEqual(1, chain.GetLeafs().Count);
        }

        [Test]
        public void Chain_Should_DetectLeafs()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("1", 1));
            chain.AddNode(new ChainNode<int>("11", 11));
            chain.AddNode(new ChainNode<int>("12", 2));
            chain.AddNode(new ChainNode<int>("13", 2));
            chain.AddNode(new ChainNode<int>("121", 2));
            //When
            chain.AddDirectedEdge(chain["1"], chain["11"], string.Empty);
            chain.AddDirectedEdge(chain["1"], chain["12"], string.Empty);
            chain.AddDirectedEdge(chain["1"], chain["13"], string.Empty);
            chain.AddDirectedEdge(chain["12"], chain["121"], string.Empty);
            //Then
            Assert.AreEqual(3, chain.GetLeafs().Count);
            Assert.AreEqual(0, chain.GetSingles().Count);
            Assert.AreEqual(1, chain.GetHeads().Count);
            
        }

        [Test]
        public void Chain_Should_DetectLeafs_Loop2()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("1", 1));
            chain.AddNode(new ChainNode<int>("11", 11));
            chain.AddNode(new ChainNode<int>("12", 2));
            chain.AddNode(new ChainNode<int>("13", 2));
            chain.AddNode(new ChainNode<int>("121", 2));
            //When
            chain.AddDirectedEdge(chain["1"], chain["11"], string.Empty);
            chain.AddDirectedEdge(chain["1"], chain["12"], string.Empty);
            chain.AddDirectedEdge(chain["1"], chain["13"], string.Empty);
            chain.AddDirectedEdge(chain["12"], chain["121"], string.Empty);
            chain.AddDirectedEdge(chain["121"], chain["1"], string.Empty);
            //Then
            Assert.That(chain["121"].IsLoop);
            Assert.AreEqual(2, chain.GetLeafs().Count, "GetLeafs 11 13");
            Assert.AreEqual(0, chain.GetSingles().Count);
            Assert.AreEqual(1, chain.GetHeads().Count, "GetHeads 1");

        }



        [Test]
        public void Chain_Should_Detect_Parallel1()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("start", 1));
            chain.AddNode(new ChainNode<int>("a1", 1));
            chain.AddNode(new ChainNode<int>("a2", 1));
            chain.AddNode(new ChainNode<int>("a3", 1));
            chain.AddNode(new ChainNode<int>("b1", 1));
            chain.AddNode(new ChainNode<int>("b2", 1));
            chain.AddNode(new ChainNode<int>("b3", 1));
            chain.AddNode(new ChainNode<int>("end", 1));
            //When
            chain.AddDirectedEdge(chain["start"], chain["a1"], string.Empty);
            chain.AddDirectedEdge(chain["a1"], chain["a2"], string.Empty);
            chain.AddDirectedEdge(chain["a2"], chain["a3"], string.Empty);
            chain.AddDirectedEdge(chain["a3"], chain["end"], string.Empty);
            chain.AddDirectedEdge(chain["start"], chain["b1"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["b2"], string.Empty);
            chain.AddDirectedEdge(chain["b2"], chain["b3"], string.Empty);
            chain.AddDirectedEdge(chain["b3"], chain["end"], string.Empty);
            //Then
            Assert.That(chain.Nodes.All(x=>!x.IsLoop));
            Assert.AreEqual(1, chain.GetLeafs().Count, "GetLeafs");
            Assert.AreEqual(0, chain.GetSingles().Count);
            Assert.AreEqual(1, chain.GetHeads().Count, "GetHeads");

        }
        [Test]
        public void Chain_Should_Detect_Parallel2()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("start", 1));
            chain.AddNode(new ChainNode<int>("a1", 1));
            chain.AddNode(new ChainNode<int>("a2", 1));
            chain.AddNode(new ChainNode<int>("a3", 1));
            chain.AddNode(new ChainNode<int>("b1", 1));
            chain.AddNode(new ChainNode<int>("b2", 1));
            chain.AddNode(new ChainNode<int>("b3", 1));
            chain.AddNode(new ChainNode<int>("end", 1));
            chain.AddDirectedEdge(chain["start"], chain["a1"], string.Empty);
            chain.AddDirectedEdge(chain["a1"], chain["a2"], string.Empty);
            chain.AddDirectedEdge(chain["a2"], chain["a3"], string.Empty);
            chain.AddDirectedEdge(chain["a3"], chain["end"], string.Empty);
            chain.AddDirectedEdge(chain["start"], chain["b1"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["b2"], string.Empty);
            chain.AddDirectedEdge(chain["b2"], chain["b3"], string.Empty);
            chain.AddDirectedEdge(chain["b3"], chain["end"], string.Empty);
            //When
            chain.AddDirectedEdge(chain["a1"], chain["b2"], string.Empty);
            chain.AddDirectedEdge(chain["a2"], chain["b3"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["a3"], string.Empty);
            //Then
            Assert.That(chain.Nodes.All(x => !x.IsLoop));
            Assert.AreEqual(1, chain.GetLeafs().Count, "GetLeafs");
            Assert.AreEqual(0, chain.GetSingles().Count);
            Assert.AreEqual(1, chain.GetHeads().Count, "GetHeads");

        }

        [Test]
        public void Chain_Should_DetectLeafs_Loop1()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("1", 1));
            chain.AddNode(new ChainNode<int>("12", 11));
            chain.AddNode(new ChainNode<int>("123", 2));
            chain.AddNode(new ChainNode<int>("1234", 2));
            chain.AddNode(new ChainNode<int>("123c", 2));
            chain.AddNode(new ChainNode<int>("123cc", 2));
            chain.AddNode(new ChainNode<int>("s", 2));
            //When
            chain.AddDirectedEdge(chain["1"], chain["12"], string.Empty);
            chain.AddDirectedEdge(chain["12"], chain["123"], string.Empty);
            chain.AddDirectedEdge(chain["123cc"], chain["12"], string.Empty);
            chain.AddDirectedEdge(chain["123"], chain["123c"], string.Empty);
            chain.AddDirectedEdge(chain["123c"], chain["123cc"], string.Empty);
            chain.AddDirectedEdge(chain["123"], chain["1234"], string.Empty);
            //Then
            Assert.AreEqual(1, chain.GetLeafs().Count, "GetLeafs 1234");
            Assert.AreEqual(1, chain.GetSingles().Count, "GetSingles");
            Assert.AreEqual(1, chain.GetHeads().Count, "GetHeads 1");
            Assert.That(chain["123cc"].IsLoop);

        }


        [Test]
        public void Chain_Should_DetectLeafs_Loop4()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("1", 1));
            chain.AddNode(new ChainNode<int>("12", 11));
            chain.AddNode(new ChainNode<int>("123", 2));
            chain.AddNode(new ChainNode<int>("1234", 2));
            chain.AddNode(new ChainNode<int>("123c", 2));
            chain.AddNode(new ChainNode<int>("123cc", 2));
            chain.AddNode(new ChainNode<int>("123ccc", 2));
            chain.AddNode(new ChainNode<int>("s", 2));
            //When
            chain.AddDirectedEdge(chain["1"], chain["12"], string.Empty);
            chain.AddDirectedEdge(chain["12"], chain["123"], string.Empty);
            chain.AddDirectedEdge(chain["123cc"], chain["123ccc"], string.Empty);
            chain.AddDirectedEdge(chain["123ccc"], chain["12"], string.Empty);
            chain.AddDirectedEdge(chain["123"], chain["123c"], string.Empty);
            chain.AddDirectedEdge(chain["123c"], chain["123cc"], string.Empty);
            chain.AddDirectedEdge(chain["123"], chain["1234"], string.Empty);
            //Then
            Assert.AreEqual(1, chain.GetLeafs().Count, "GetLeafs 1234");
            Assert.AreEqual(1, chain.GetSingles().Count, "GetSingles");
            Assert.AreEqual(1, chain.GetHeads().Count, "GetHeads 1");
            Assert.That(chain["123ccc"].IsLoop);
        }

        [Test]
        public void Chain_Should_DetectLeafs_Loop5()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("1", 1));
            chain.AddNode(new ChainNode<int>("2", 11));
            chain.AddNode(new ChainNode<int>("3", 2));
            //When
            chain.AddDirectedEdge(chain["1"], chain["2"], string.Empty);
            chain.AddDirectedEdge(chain["1"], chain["3"], string.Empty);
            chain.AddDirectedEdge(chain["2"], chain["3"], string.Empty);
            chain.AddDirectedEdge(chain["3"], chain["1"], string.Empty);
            //Then
            Assert.AreEqual(0, chain.GetLeafs().Count, "GetLeafs");
            Assert.AreEqual(0, chain.GetSingles().Count, "GetSingles");
            Assert.AreEqual(1, chain.GetHeads().Count, "GetHeads 1");
            Assert.That(chain["3"].IsLoop);
        }

        [Test]
        public void Chain_Should_SearchLeafsFromHead()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("1", 1));
            chain.AddNode(new ChainNode<int>("11", 11));
            chain.AddNode(new ChainNode<int>("12", 2));
            chain.AddNode(new ChainNode<int>("13", 2));
            chain.AddNode(new ChainNode<int>("121", 2));
            chain.AddNode(new ChainNode<int>("2", 2));
            chain.AddNode(new ChainNode<int>("21", 2));
            chain.AddDirectedEdge(chain["1"], chain["11"], string.Empty);
            chain.AddDirectedEdge(chain["1"], chain["12"], string.Empty);
            chain.AddDirectedEdge(chain["1"], chain["13"], string.Empty);
            chain.AddDirectedEdge(chain["12"], chain["121"], string.Empty);
            chain.AddDirectedEdge(chain["2"], chain["21"], string.Empty);
            chain.AddDirectedEdge(chain["21"], chain["12"], string.Empty);
            Assume.That(chain.GetSingles(), Has.Exactly(0).Items);
            Assume.That(chain.GetHeads(), Has.Exactly(2).Items);
            Assume.That(chain.GetLeafs(), Has.Exactly(3).Items);
            //When
            var result = chain.SearchLeafs(chain["2"]);
            //Then
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("121", result[0].Id);
        }


        [Test]
        public void Chain_Should_SearchHeadsFromLeaf()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("1", 1));
            chain.AddNode(new ChainNode<int>("11", 11));
            chain.AddNode(new ChainNode<int>("12", 2));
            chain.AddNode(new ChainNode<int>("13", 2));
            chain.AddNode(new ChainNode<int>("121", 2));
            chain.AddNode(new ChainNode<int>("2", 2));
            chain.AddNode(new ChainNode<int>("21", 2));
            chain.AddNode(new ChainNode<int>("a1", 2));
            chain.AddNode(new ChainNode<int>("a2", 2));
            chain.AddDirectedEdge(chain["1"], chain["11"], string.Empty);
            chain.AddDirectedEdge(chain["1"], chain["12"], string.Empty);
            chain.AddDirectedEdge(chain["1"], chain["13"], string.Empty);
            chain.AddDirectedEdge(chain["12"], chain["121"], string.Empty);
            chain.AddDirectedEdge(chain["2"], chain["21"], string.Empty);
            chain.AddDirectedEdge(chain["21"], chain["12"], string.Empty);
            chain.AddDirectedEdge(chain["a1"], chain["a2"], string.Empty);
            chain.AddDirectedEdge(chain["a1"], chain["13"], string.Empty);
            Assume.That(chain.GetSingles(), Has.Exactly(0).Items);
            Assume.That(chain.GetHeads(), Has.Exactly(3).Items);
            Assume.That(chain.GetLeafs(), Has.Exactly(4).Items);
            //When
            var result = chain.SearchHeads(chain["121"]);
            //Then
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("1", result[0].Id);
            Assert.AreEqual("2", result[1].Id);
        }

        [Test]
        public void Chain_Should_DetectLeafs_Loop3()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("1", 1));
            chain.AddNode(new ChainNode<int>("11", 11));
            chain.AddNode(new ChainNode<int>("12", 2));
            chain.AddNode(new ChainNode<int>("13", 2));
            chain.AddNode(new ChainNode<int>("121", 2));
            chain.AddNode(new ChainNode<int>("2", 2));
            chain.AddNode(new ChainNode<int>("21", 2));
            chain.AddDirectedEdge(chain["1"], chain["11"], string.Empty);
            chain.AddDirectedEdge(chain["1"], chain["12"], string.Empty);
            chain.AddDirectedEdge(chain["1"], chain["13"], string.Empty);
            chain.AddDirectedEdge(chain["12"], chain["121"], string.Empty);
            chain.AddDirectedEdge(chain["2"], chain["21"], string.Empty);
            chain.AddDirectedEdge(chain["21"], chain["12"], string.Empty);
            //When
            chain.AddDirectedEdge(chain["121"], chain["2"], string.Empty);
            //Then
            Assert.That(chain["21"].IsLoop, "chain['21'].IsLoop");
            Assert.That(chain["121"].IsLoop == false, "chain['121'].IsLoop == false");
            Assert.That(chain.GetHeads(), Has.Exactly(1).Items, "GetHeads 1");
            Assert.That(chain.GetLeafs(), Has.Exactly(2).Items, "GetLeafs 11 13");
            Assert.That(chain.GetSingles(), Has.Exactly(0).Items);
        }
        [Test]
        public void Chain_Should_SearchLeafsNotFromHead_Loop()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("1", 1));
            chain.AddNode(new ChainNode<int>("11", 11));
            chain.AddNode(new ChainNode<int>("12", 2));
            chain.AddNode(new ChainNode<int>("13", 2));
            chain.AddNode(new ChainNode<int>("121", 2));
            chain.AddNode(new ChainNode<int>("2", 2));
            chain.AddNode(new ChainNode<int>("21", 2));
            chain.AddDirectedEdge(chain["1"], chain["11"], string.Empty);
            chain.AddDirectedEdge(chain["1"], chain["12"], string.Empty);
            chain.AddDirectedEdge(chain["1"], chain["13"], string.Empty);
            chain.AddDirectedEdge(chain["12"], chain["121"], string.Empty);
            chain.AddDirectedEdge(chain["2"], chain["21"], string.Empty);
            chain.AddDirectedEdge(chain["21"], chain["12"], string.Empty);
            chain.AddDirectedEdge(chain["121"], chain["2"], string.Empty);
            Assume.That(chain.GetSingles(), Has.Exactly(0).Items);
            Assume.That(chain.GetHeads(), Has.Exactly(1).Items, "GetHeads 1");
            Assume.That(chain.GetLeafs(), Has.Exactly(2).Items, "GetLeafs 11 13");
            //When
            var result = chain.SearchLeafs(chain["2"]);
            //Then
            Assert.That(chain["21"].IsLoop);
            Assert.AreEqual(0, result.Count, "result.Count");
        }



        [Test]
        public void Chain_Should_SplitChains1()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("a1", 1));
            chain.AddNode(new ChainNode<int>("a2", 11));
            chain.AddNode(new ChainNode<int>("b1", 2));
            chain.AddNode(new ChainNode<int>("b2", 2));
            chain.AddNode(new ChainNode<int>("s", 2));
            chain.AddDirectedEdge(chain["a1"], chain["a2"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["b2"], string.Empty);
            Assume.That(chain.GetSingles(), Has.Exactly(1).Items);
            Assume.That(chain.GetHeads(), Has.Exactly(2).Items);
            Assume.That(chain.GetLeafs(), Has.Exactly(2).Items);
            //When
            var result = chain.SplitChains();
            //Then
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public void Chain_Should_SplitChains1_Edges()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("a1", 1));
            chain.AddNode(new ChainNode<int>("a2", 11));
            chain.AddNode(new ChainNode<int>("b1", 2));
            chain.AddNode(new ChainNode<int>("b2", 2));
            chain.AddNode(new ChainNode<int>("s", 2));
            chain.AddDirectedEdge(chain["a1"], chain["a2"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["b2"], string.Empty);
            Assume.That(chain.GetSingles(), Has.Exactly(1).Items);
            Assume.That(chain.GetHeads(), Has.Exactly(2).Items);
            Assume.That(chain.GetLeafs(), Has.Exactly(2).Items);
            //When
            var result = chain.SplitChains();
            //Then
            Assume.That(3 == result.Count);
            Assert.AreEqual(1, result.Single(x=>x.Nodes.Any(n=>n.Id == "a1")).Edges.Count);
            Assert.AreEqual(1, result.Single(x => x.Nodes.Any(n => n.Id == "b1")).Edges.Count);
            Assert.AreEqual(0, result.Single(x => x.Nodes.Any(n => n.Id == "s")).Edges.Count);
        }


        [Test]
        public void Chain_Should_SplitChains2()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("a1", 1));
            chain.AddNode(new ChainNode<int>("a2", 11));
            chain.AddNode(new ChainNode<int>("b1", 2));
            chain.AddNode(new ChainNode<int>("b2", 2));
            chain.AddNode(new ChainNode<int>("s", 2));
            chain.AddDirectedEdge(chain["a1"], chain["s"], string.Empty);
            chain.AddDirectedEdge(chain["s"], chain["a2"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["s"], string.Empty);
            chain.AddDirectedEdge(chain["s"], chain["b2"], string.Empty);
            Assume.That(chain.GetSingles(), Has.Exactly(0).Items);
            Assume.That(chain.GetHeads(), Has.Exactly(2).Items);
            Assume.That(chain.GetLeafs(), Has.Exactly(2).Items);
            //When
            var result = chain.SplitChains();
            //Then
            Assert.AreEqual(1, result.Count);
        }


        [Test]
        public void Chain_Should_SplitChains2_Edges()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("a1", 1));
            chain.AddNode(new ChainNode<int>("a2", 11));
            chain.AddNode(new ChainNode<int>("b1", 2));
            chain.AddNode(new ChainNode<int>("b2", 2));
            chain.AddNode(new ChainNode<int>("s", 2));
            chain.AddDirectedEdge(chain["a1"], chain["s"], string.Empty);
            chain.AddDirectedEdge(chain["s"], chain["a2"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["s"], string.Empty);
            chain.AddDirectedEdge(chain["s"], chain["b2"], string.Empty);
            Assume.That(chain.GetSingles(), Has.Exactly(0).Items);
            Assume.That(chain.GetHeads(), Has.Exactly(2).Items);
            Assume.That(chain.GetLeafs(), Has.Exactly(2).Items);
            //When
            var result = chain.SplitChains();
            //Then
            Assume.That(1 == result.Count);
            Assert.AreEqual(4, result.Single(x => x.Nodes.Any(n => n.Id == "a1")).Edges.Count);
        }

        [Test]
        public void Chain_Should_SplitChains3()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("a1", 1));
            chain.AddNode(new ChainNode<int>("a2", 11));
            chain.AddNode(new ChainNode<int>("a3", 11));
            chain.AddNode(new ChainNode<int>("b1", 2));
            chain.AddNode(new ChainNode<int>("b2", 2));
            chain.AddNode(new ChainNode<int>("b3", 2));
            chain.AddNode(new ChainNode<int>("c1", 2));
            chain.AddNode(new ChainNode<int>("c2", 2));
            chain.AddNode(new ChainNode<int>("c3", 2));
            chain.AddDirectedEdge(chain["a1"], chain["a2"], string.Empty);
            chain.AddDirectedEdge(chain["a2"], chain["a3"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["b2"], string.Empty);
            chain.AddDirectedEdge(chain["b2"], chain["b3"], string.Empty);
            chain.AddDirectedEdge(chain["c1"], chain["c2"], string.Empty);
            chain.AddDirectedEdge(chain["c2"], chain["c3"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["a2"], string.Empty);
            Assume.That(chain.GetSingles(), Has.Exactly(0).Items);
            Assume.That(chain.GetHeads(), Has.Exactly(3).Items);
            Assume.That(chain.GetLeafs(), Has.Exactly(3).Items);
            //When
            var result = chain.SplitChains();
            //Then
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void Chain_Should_SplitChains3_Edges()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("a1", 1));
            chain.AddNode(new ChainNode<int>("a2", 11));
            chain.AddNode(new ChainNode<int>("a3", 11));
            chain.AddNode(new ChainNode<int>("b1", 2));
            chain.AddNode(new ChainNode<int>("b2", 2));
            chain.AddNode(new ChainNode<int>("b3", 2));
            chain.AddNode(new ChainNode<int>("c1", 2));
            chain.AddNode(new ChainNode<int>("c2", 2));
            chain.AddNode(new ChainNode<int>("c3", 2));
            chain.AddDirectedEdge(chain["a1"], chain["a2"], string.Empty);
            chain.AddDirectedEdge(chain["a2"], chain["a3"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["b2"], string.Empty);
            chain.AddDirectedEdge(chain["b2"], chain["b3"], string.Empty);
            chain.AddDirectedEdge(chain["c1"], chain["c2"], string.Empty);
            chain.AddDirectedEdge(chain["c2"], chain["c3"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["a2"], string.Empty);
            Assume.That(chain.GetSingles(), Has.Exactly(0).Items);
            Assume.That(chain.GetHeads(), Has.Exactly(3).Items);
            Assume.That(chain.GetLeafs(), Has.Exactly(3).Items);
            //When
            var result = chain.SplitChains();
            //Then
            Assume.That(2 == result.Count);
            Assert.AreEqual(5, result.Single(x => x.Nodes.Any(n => n.Id == "b1")).Edges.Count);
            Assert.AreEqual(2, result.Single(x => x.Nodes.Any(n => n.Id == "c1")).Edges.Count);
        }

        [Test]
        public void Chain_Should_SplitChains_parallel_Edges()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("a1", 1));
            chain.AddNode(new ChainNode<int>("a2", 11));
            chain.AddNode(new ChainNode<int>("a3", 11));
            chain.AddNode(new ChainNode<int>("b1", 2));
            chain.AddNode(new ChainNode<int>("b2", 2));
            chain.AddNode(new ChainNode<int>("b3", 2));
            chain.AddNode(new ChainNode<int>("c1", 2));
            chain.AddNode(new ChainNode<int>("c2", 2));
            chain.AddNode(new ChainNode<int>("c3", 2));
            chain.AddDirectedEdge(chain["a1"], chain["a2"], string.Empty);
            chain.AddDirectedEdge(chain["a2"], chain["a3"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["b2"], string.Empty);
            chain.AddDirectedEdge(chain["b2"], chain["b3"], string.Empty);
            chain.AddDirectedEdge(chain["c1"], chain["c2"], string.Empty);
            chain.AddDirectedEdge(chain["c2"], chain["c3"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["a2"], string.Empty);
            chain.AddDirectedEdge(chain["c1"], chain["b2"], string.Empty);
            chain.AddDirectedEdge(chain["b3"], chain["a1"], string.Empty);
            chain.AddDirectedEdge(chain["c3"], chain["a1"], string.Empty);
            Assume.That(chain.GetSingles(), Has.Exactly(0).Items);
            Assume.That(chain.GetHeads(), Has.Exactly(2).Items, "GetHeads b1 c1");
            Assume.That(chain.GetLeafs(), Has.Exactly(1).Items, "GetLeafs a3");
            //When
            var result = chain.SplitChains();
            //Then
            Assume.That(result[0]["a1"].IsLoop == false);
            Assume.That(result[0]["b3"].IsLoop == false);
            Assume.That(result[0]["c3"].IsLoop == false);
            Assume.That(1 == result.Count);
            Assert.AreEqual(10, result.Single(x => x.Nodes.Any(n => n.Id == "b1")).Edges.Count);
        }

        [Test]
        public void Chain_Should_SplitChains_parallel()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("a1", 1));
            chain.AddNode(new ChainNode<int>("a2", 11));
            chain.AddNode(new ChainNode<int>("a3", 11));
            chain.AddNode(new ChainNode<int>("b1", 2));
            chain.AddNode(new ChainNode<int>("b2", 2));
            chain.AddNode(new ChainNode<int>("b3", 2));
            chain.AddNode(new ChainNode<int>("c1", 2));
            chain.AddNode(new ChainNode<int>("c2", 2));
            chain.AddNode(new ChainNode<int>("c3", 2));
            chain.AddDirectedEdge(chain["a1"], chain["a2"], string.Empty);
            chain.AddDirectedEdge(chain["a2"], chain["a3"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["b2"], string.Empty);
            chain.AddDirectedEdge(chain["b2"], chain["b3"], string.Empty);
            chain.AddDirectedEdge(chain["c1"], chain["c2"], string.Empty);
            chain.AddDirectedEdge(chain["c2"], chain["c3"], string.Empty);
            chain.AddDirectedEdge(chain["b1"], chain["a2"], string.Empty);
            chain.AddDirectedEdge(chain["c1"], chain["b2"], string.Empty);
            chain.AddDirectedEdge(chain["b3"], chain["a1"], string.Empty);
            chain.AddDirectedEdge(chain["c3"], chain["a1"], string.Empty);
            Assume.That(chain.GetSingles(), Has.Exactly(0).Items);
            Assume.That(chain.GetHeads(), Has.Exactly(2).Items, "GetHeads b1 c1");
            Assume.That(chain.GetLeafs(), Has.Exactly(1).Items, "GetLeafs a3");
            //When
            var result = chain.SplitChains();
            //Then
            Assert.AreEqual(1, result.Count);
            Assert.That(result[0]["a1"].IsLoop == false);
            Assert.That(result[0]["b3"].IsLoop == false);
            Assert.That(result[0]["c3"].IsLoop == false);
        }

        [Test]
        public void Chain_Should_SplitChains_Loop()
        {
            //Given
            var chain = new Chain<int, string>();
            chain.AddNode(new ChainNode<int>("C", 1));
            chain.AddNode(new ChainNode<int>("S", 11));
            chain.AddNode(new ChainNode<int>("I", 12));
            chain.AddDirectedEdge(chain["C"], chain["S"], string.Empty);
            chain.AddDirectedEdge(chain["S"], chain["I"], string.Empty);
            chain.AddDirectedEdge(chain["C"], chain["I"], string.Empty);
            chain.AddDirectedEdge(chain["I"], chain["S"], string.Empty);
            Assert.That(chain.GetSingles(), Has.Exactly(0).Items);
            Assert.That(chain.GetHeads(), Has.Exactly(1).Items, "GetHeads C");
            Assert.That(chain.GetLeafs(), Has.Exactly(0).Items, "GetLeafs");
            //When
            var result = chain.SplitChains();
            //Then
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(3, result[0].Nodes.Count);
            Assert.That(result[0]["I"].IsLoop);
            Assert.That(result[0]["C"].IsLoop == false);
            Assert.That(result[0]["S"].IsLoop == false);
            Assert.AreEqual(4, result.Single(x => x.Nodes.Any(n => n.Id == "I")).Edges.Count, "Edges.Count");
        }

    }
}