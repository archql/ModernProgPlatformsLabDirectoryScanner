using NUnit.Framework;

using lab3DirectoryScanner.DirectoryScanner;
using System;
using System.IO;

namespace lab3DirectoryScanner.Tests
{
    public class Tests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestEmptyFolder()
        {
            var dirScanner = new DirScanner("../../../TestsFolders/TestEmpty");
            dirScanner.Start(5);
            dirScanner.WaitForCompletion();

            var result = dirScanner.Result();
            result.CalcSize();

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);

                var head = result.Head();
                Assert.IsNotNull(head);
                Assert.That(head.TotalSize, Is.EqualTo(0));
                Assert.IsEmpty(head.Childs);
            });

            Assert.Pass();
        }

        [Test]
        public void TestFolderWithEmptyFolder()
        {
            var dirScanner = new DirScanner("../../../TestsFolders/TestFolded");
            dirScanner.Start(5);
            dirScanner.WaitForCompletion();

            var result = dirScanner.Result();
            result.CalcSize();

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);

                var head = result.Head();
                Assert.That(Path.GetFileName(head.Path), Is.EqualTo("TestFolded"));
                Assert.That(head.TotalSize, Is.EqualTo(0));
                Assert.IsNotEmpty(head.Childs);

                Console.Out.WriteLine(head.Childs.Count);

                var innerNode = head.Childs[0];
                Assert.That(Path.GetFileName(innerNode.Path), Is.EqualTo("A"));
                Assert.That(innerNode.TotalSize, Is.EqualTo(0));
                Assert.IsEmpty(innerNode.Childs);
            });
        }

        [Test]
        public void TestMultiple()
        {
            var dirScanner = new DirScanner("../../../TestsFolders/TestMultiple");
            dirScanner.Start(5);
            dirScanner.WaitForCompletion();

            var result = dirScanner.Result();
            result.CalcSize();

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);

                var head = result.Head();
                Assert.That(Path.GetFileName(head.Path), Is.EqualTo("TestMultiple"));
                Assert.That(head.TotalSize, Is.EqualTo(761106));
                Assert.That(head.Childs.Count, Is.EqualTo(4));
            });
        }

        [Test]
        public void TestFolderWithFile()
        {
            var dirScanner = new DirScanner("../../../TestsFolders/TestFoldedFile");
            dirScanner.Start(5);
            dirScanner.WaitForCompletion();

            var result = dirScanner.Result();
            result.CalcSize();

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(result);

                var head = result.Head();
                Assert.That(head.TotalSize, Is.EqualTo(4));
                Assert.IsNotNull(head.Childs);

                var innerNode = head.Childs[0];
                Assert.That(Path.GetFileName(innerNode.Path), Is.EqualTo("single file.txt"));
                Assert.That(innerNode.TotalSize, Is.EqualTo(4));
                Assert.IsEmpty(innerNode.Childs);
            });
        }

        [Test]
        public void NotFolder()
        {
            var dirScanner = new DirScanner("../../../TestsFolders/Test/NotFolder.NotFolder");
            Assert.Throws(typeof(DirectoryNotFoundException), () => dirScanner.Start(5));
        }

        [Test]
        public void NonexistentFolder()
        {
            var dirScanner = new DirScanner("../../../TestsFolders/Test/DirectoryNotFoundException");
            Assert.Throws(typeof(DirectoryNotFoundException), () => dirScanner.Start(5));
        }
    }
}