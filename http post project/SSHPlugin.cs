using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using UBotPlugin;

namespace SSHPlugin
{
    public class PluginInfo
    {
        public static string HashCode => "54a3be28eaf3867e547f025af631a7b415f9fa33";
    }

    internal class SshCommands : IUBotFunction
    {
        private readonly List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();
        private string _returnValue;

        public SshCommands()
        {
            _parameters.Add(new UBotParameterDefinition("Address", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Port", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Username", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Password", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Command to Execute", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Path to plink.exe", UBotType.String));
        }


        public string Category => "Botguru.net SSH";

        public string FunctionName => "$SshFunction";


        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {
            var taddy = parameters["Address"];
            var tport = parameters["Port"];
            var tuser = parameters["Username"];
            var tpass = parameters["Password"];
            var tcommand = parameters["Command to Execute"];
            var tplink = parameters["Path to plink.exe"];

            try
            {
                var strReturn = "";

                var p = new Process { StartInfo = { FileName = @"" + tplink } };

                if (tpass.Length == 0 || tuser.Length == 0 || taddy.Length == 0 || tcommand.Length == 0 ||
                    tport.Length == 0)
                {
                    _returnValue = new Exception("SSHClient: Invalid parameteres for SSHClient.").ToString();
                }

                //var param = "-ssh -pw " + tpass + " " + tuser + "@" + taddy + " -P " + tport + " " + tcommand;
                var param = "-ssh -pw " + tpass + " -t " + tuser + "@" + taddy + " -P " + tport + " " + tcommand;

                if (File.Exists(@"" + tplink) == false)
                {
                    _returnValue =
                        new Exception(
                            "SSHClient: plink.exe not found. Make sure plink.exe is in the same folder as YOUR EXE.")
                            .ToString();
                }
                else
                {
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.Arguments = param;

                    p.Start();
                    var standerOut = p.StandardOutput;

                    while (!p.HasExited)
                    {
                        if (!standerOut.EndOfStream)
                        {
                            strReturn += standerOut.ReadLine() + Environment.NewLine;
                        }
                    }
                }

                _returnValue = strReturn;
            }
            catch (Exception exp)
            {
                _returnValue = new Exception("SSHClient:", exp).ToString();
            }
        }

        public object ReturnValue => _returnValue;

        public UBotType ReturnValueType => UBotType.String;


        public IEnumerable<UBotParameterDefinition> ParameterDefinitions => _parameters;

        public UBotVersion UBotVersion => UBotVersion.Standard;
    }

    internal class SshcOmmandsKey : IUBotFunction
    {
        private readonly List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();
        private string _returnValue;

        public SshcOmmandsKey()
        {
            _parameters.Add(new UBotParameterDefinition("Address", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Port", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Username", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Arguments", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Command", UBotType.String));
            _parameters.Add(new UBotParameterDefinition("Path to plink.exe", UBotType.String));
        }


        public string Category => "Botguru.net SSH";

        public string FunctionName => "$SshFunction Custom";


        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {
            var taddy = parameters["Address"];
            var tport = parameters["Port"];
            var tuser = parameters["Username"];
            var targuments = parameters["Arguments"];
            var tcommand = parameters["Command"];
            var tplink = parameters["Path to plink.exe"];


            try
            {
                var strReturn = "";

                var p = new Process { StartInfo = { FileName = @"" + tplink } };

                if (targuments.Length == 0 || tuser.Length == 0 || taddy.Length == 0 || tport.Length == 0)
                {
                    _returnValue = new Exception("SSHClient: Invalid parameteres for SSHClient.").ToString();
                }

                var param = targuments + " " + tuser + "@" + taddy + " -P " + tport + " " + tcommand;

                if (File.Exists(@"" + tplink) == false)
                {
                    _returnValue =
                        new Exception(
                            "SSHClient: plink.exe not found. Make sure plink.exe is in the same folder as YOUR EXE.")
                            .ToString();
                }
                else
                {
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.Arguments = param;

                    p.Start();
                    var standerOut = p.StandardOutput;

                    while (!p.HasExited)
                    {
                        if (!standerOut.EndOfStream)
                        {
                            strReturn += standerOut.ReadLine() + Environment.NewLine;
                        }
                    }
                }

                _returnValue = strReturn;
            }
            catch (Exception exp)
            {
                _returnValue = new Exception("SSHClient:", exp).ToString();
            }
        }

        public object ReturnValue => _returnValue;

        public UBotType ReturnValueType => UBotType.String;

        public IEnumerable<UBotParameterDefinition> ParameterDefinitions => _parameters;

        public UBotVersion UBotVersion => UBotVersion.Standard;
    }



    internal class DownloadLatestPlink : IUBotCommand
    {
        private readonly List<UBotParameterDefinition> _parameters = new List<UBotParameterDefinition>();

        public DownloadLatestPlink()
        {
            _parameters.Add(new UBotParameterDefinition("Path", UBotType.String));
        }

        public string Category => "Botguru.net SSH";

        public string CommandName => "Download Plink.exe";

        public bool IsContainer => false;

        public IEnumerable<UBotParameterDefinition> ParameterDefinitions => _parameters;

        public UBotVersion UBotVersion => UBotVersion.Standard;

        public void Execute(IUBotStudio ubotStudio, Dictionary<string, string> parameters)
        {

            var folder = parameters["Path"];

            DownloadFileToDisk(folder);
        }

        private static void DownloadFileToDisk(string localPath)
        {
            var myWebClient = new WebClient();
            var useragent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36";

            try
            {
                myWebClient.Headers.Add("user-agent", useragent);
                myWebClient.DownloadFile("https://the.earth.li/~sgtatham/putty/latest/x86/plink.exe", localPath + "\\plink.exe");
            }
            catch (Exception)
            {
                //Ignored
            }
        }


    }


}