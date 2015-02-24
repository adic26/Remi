using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TsdLib.Configuration.TestCases
{
    public class TestCaseProvider
    {
        public string TestCaseFile { get; private set; }

        public TestCaseProvider(string testSystemName)
        {
            var directory = SpecialFolders.Configuration
                .CreateSubdirectory(testSystemName);

            TestCaseFile = Path.Combine(directory.FullName, "TestCases.xml");
            if (!File.Exists(TestCaseFile))
                new XDocument(new XElement("TestCases")).Save(TestCaseFile);
        }

        public void Save(ITestCase testCase)
        {
            var testCaseElement = new XElement("TestCase", new XAttribute("Name", testCase.Name),
                new XElement("TestConfigs",
                    testCase.TestConfigs.Select(tcName => new XElement("TestConfig", new XAttribute("Name", tcName)))),
                new XElement("Sequences",
                    testCase.Sequences.Select(seq => new XElement("Sequence", new XAttribute("Name", seq)))));

            var testCasesXDoc = getTestCasesXDocument();
            var existingTestCaseElement = testCasesXDoc.Root.Elements().FirstOrDefault(tce => (string)tce.Attribute("Name") == testCase.Name);
            if (existingTestCaseElement != null)
                existingTestCaseElement.Remove();
            testCasesXDoc.Root.Add(testCaseElement);

            testCasesXDoc.Save(TestCaseFile);
        }

        public IEnumerable<ITestCase> Load()
        {
            List<ITestCase> testCases = new List<ITestCase>();

            foreach (XElement testCaseElement in getTestCasesXDocument().Root.Elements("TestCase"))
            {
                string testCaseName = (string)testCaseElement.Attribute("Name");
                IEnumerable<string> testConfigNames = testCaseElement
                    .Element("TestConfigs")
                        .Elements("TestConfig")
                        .Select(tCfg => (string)tCfg.Attribute("Name"));
                IEnumerable<string> sequenceNames = testCaseElement
                    .Element("Sequences")
                        .Elements("Sequence")
                        .Select(seq => (string)seq.Attribute("Name"));

                testCases.Add(new TestCase(testCaseName, testConfigNames, sequenceNames));
            }

            return testCases;
        }

        private XDocument getTestCasesXDocument()
        {
            XDocument testCasesDoc = XDocument.Load(TestCaseFile);
            var root = testCasesDoc.Root;
            if (root == null)
                throw new Exception("Invalid Test Cases file. No root element.");

            return testCasesDoc;
        }
    }
}
