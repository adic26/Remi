using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using TsdLib.Configuration.Common;
using TsdLib.Configuration.Connections;
using TsdLib.Configuration.Managers;

namespace TsdLib.Configuration
{
    /// <summary>
    /// Contains static methods used to synchronize design-time sequence files with run-time sequence config.
    /// </summary>
    public class SequenceSync
    {
        /// <summary>
        /// Syncronize a group of test sequence class files with sequence config objects.
        /// </summary>
        /// <param name="testDetails">A test details object containing information required to locate the test sequences.</param>
        /// <param name="sharedConfigConnection">A connection object to manage the database connection.</param>
        /// <param name="sequenceFolder">Full path to the folder where sequence class files are located.</param>
        /// <param name="storeInDatabase">True to store the sequence configuration in the shared config location.</param>
        /// <param name="updateVsSequenceFiles">True to update the class files in the sequenceFolder.</param>
        public static void SynchronizeSequences(ITestDetails testDetails, IConfigConnection sharedConfigConnection, string sequenceFolder, bool storeInDatabase, bool updateVsSequenceFiles)
        {
            ConfigManager<SequenceConfigCommon> sequenceConfigManager = new ConfigManager<SequenceConfigCommon>(testDetails, sharedConfigConnection);

            HashSet<string> assemblyReferences = new HashSet<string>(AppDomain.CurrentDomain.GetAssemblies().Select(asy => Path.GetFileName(asy.GetName().CodeBase)), StringComparer.InvariantCultureIgnoreCase) { Path.GetFileName(Assembly.GetEntryAssembly().GetName().CodeBase) };
            foreach (string fileName in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").Select(Path.GetFileName))
                assemblyReferences.Add(fileName);

            if (updateVsSequenceFiles)
            {
                foreach (SequenceConfigCommon sequence in sequenceConfigManager.GetConfigGroup().Where(seq => !seq.IsDefault))
                {
                    string vsFile = Path.Combine(sequenceFolder, sequence.Name + ".cs");
                    if (!File.Exists(vsFile))
                        File.WriteAllText(vsFile, sequence.SourceCode);
                }
            }
            //TODO: improve this - only try to create a sequence if it is a valid sequence class - could run into problems if there are helper classes or extra files in the directory
            //how to tell, since the sequences can derive from different base classes (SequentialTestSequence, ParallelTestSequence, etc)
            //use an attribute? Requires changes to all client code
            //maybe better to stick with one-sequence-per-file
            foreach (string seqFile in Directory.EnumerateFiles(sequenceFolder))
            {
                Trace.WriteLine("Found " + seqFile);
                //TODO: only replace if newer?
                sequenceConfigManager.Add(new SequenceConfigCommon(seqFile, storeInDatabase, assemblyReferences));
            }
            sequenceConfigManager.Save();
        }

    }
}
