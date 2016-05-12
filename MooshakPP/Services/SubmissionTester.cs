using System;
using System.Web;
using System.Collections.Generic;
using MooshakPP.Models.Entities;
using System.Configuration;
using System.IO;
using System.Diagnostics;


namespace MooshakPP.Services
{
    public class SubmissionTester
    {
        public void InitCompiler(ref Process compiler, string workingFolder)
        {
            compiler.StartInfo.FileName = "cmd.exe";
            compiler.StartInfo.WorkingDirectory = workingFolder;
            compiler.StartInfo.RedirectStandardInput = true;
            compiler.StartInfo.RedirectStandardOutput = true;
            compiler.StartInfo.UseShellExecute = false;
        }

        // Compile C++ code
        public result CompileCPP(ref Process compiler, string cppFileName)
        {
            try
            {
                // Provide the directory containing vcvars32.bat and cl.exe
                string compilerFolder = ConfigurationManager.AppSettings["C++CompilerFolder"];
                compiler.Start();
                compiler.StandardInput.WriteLine("\"" + compilerFolder + "vcvars32.bat" + "\"");
                compiler.StandardInput.WriteLine("cl.exe /nologo /EHsc " + cppFileName);
                compiler.StandardInput.WriteLine("exit");
                compiler.WaitForExit();
                compiler.Close();
            }
            catch(Exception)
            {
                // Any uncaught exception in the compilation process will be caught here
                return result.compError;
            }
            return result.none;
        }

        // Compile C# code
        public result CompileCS(ref Process compiler, string csFileName)
        {
            try
            {
                // Provide the vsvars32.bat plugin path
                string vsvarsPath = ConfigurationManager.AppSettings["C#PluginPath"];
                string cscPath = ConfigurationManager.AppSettings["C#CompilerPathRelative"];
                // Make the path absolute
                cscPath = HttpContext.Current.Server.MapPath(cscPath);
                compiler.Start();
                compiler.StandardInput.WriteLine("\"" + vsvarsPath + "\"");
                compiler.StandardInput.WriteLine(cscPath + " /nologo /out:Program.exe " + csFileName);
                compiler.StandardInput.WriteLine("exit");
                compiler.WaitForExit();
                compiler.Close();
            }
            catch(Exception)
            {
                // Any uncaught exception in the compilation process will be caught here
                return result.compError;
            }
            
            return result.none;
        }

        public void InitTester(ref ProcessStartInfo processInfoExe)
        {
            processInfoExe.UseShellExecute = false;
            processInfoExe.RedirectStandardOutput = true;
            processInfoExe.RedirectStandardInput = true;
            processInfoExe.RedirectStandardError = true;
            processInfoExe.CreateNoWindow = false;
        }

        public result TestSubmission(ref ProcessStartInfo processInfoExe, ref List<TestCase> testCases)
        {
            //count passed tests
            try
            {
                int passCount = 0;
                foreach (TestCase test in testCases)
                {
                    string input;
                    // Load test case input file
                    using (StreamReader sr = new StreamReader(test.inputUrl))
                    {
                        input = sr.ReadToEnd();
                    }
                    List<string> output = new List<string>();
                    // Create a new process with a limited lifespan
                    using (Process processExe = new Process())
                    {
                        processExe.StartInfo = processInfoExe;
                        processExe.Start();
                        processExe.StandardInput.WriteLine(input);

                        // Read the program output
                        while (!processExe.StandardOutput.EndOfStream)
                        {
                            output.Add(processExe.StandardOutput.ReadLine());
                        }

                        // End process
                        processExe.Close();
                    }


                    //Read the expected output of current test case
                    using (StreamReader sr = new StreamReader(test.outputUrl))
                    {
                        List<string> expected = new List<string>();
                        while (!sr.EndOfStream)
                        {
                            expected.Add(sr.ReadLine());
                        }

                        // Compare expected and obtained output
                        bool mismatchFound = false;
                        for (int line = 0; line < expected.Count; line++)
                        {
                            if (line < output.Count)
                            {
                                // Output does not match expected
                                if (expected[line] != output[line])
                                {
                                    mismatchFound = true;
                                }
                            }
                            // Output stopped early
                            else
                            {
                                mismatchFound = true;
                            }
                        }
                        // Test passed
                        if (!mismatchFound)
                        {
                            passCount++;
                        }
                        else
                        {
                            return result.wrongAnswer;
                        }
                    }
                }
                // All tests passsed
                if (passCount == testCases.Count)
                {
                    return result.Accepted;
                }
                else
                {   //Safeguard, should never happen
                    return result.runError;
                }
            }
            catch(Exception)
            {
                return result.runError;
            }

        }
    }
}