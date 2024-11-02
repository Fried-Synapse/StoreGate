using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace StoreGate.Common
{
    public class ProcessRunner
    {
        public ProcessRunner(ILogger logger, string fileName)
        {
            Logger = logger;
            Process = new Process();
            Process.StartInfo.FileName = fileName;
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.OutputDataReceived += OnOutputDataReceived;
            Process.StartInfo.RedirectStandardError = true;
            Process.ErrorDataReceived += OnErrorDataReceived;
            Process.StartInfo.CreateNoWindow = true;
        }

        private ILogger Logger { get; }
        private Process Process { get; }

        public ProcessRunner AddOption(string option, string? value = null)
        {
            Process.StartInfo.Arguments += $" {option}{GetValue(value)}";
            return this;

            static string GetValue(string? value)
            {
                if (value == null)
                {
                    return "";
                }

                //TODO add "" if needed
                return $" {value}";
            }
        }

        //TODO this shouldn't be in here, but an extension method
        public ProcessRunner ExecuteMethod(string method, params string[] args) 
            => AddOption("-executeMethod", $"StoreGate.StoreGateHeadless.{method} {string.Join(' ', args.Select(a => $"\"{a}\""))}");

        public async Task RunAsync()
        {
            Process.Start();
            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
            await Process.WaitForExitAsync();
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }

            if (e.Data.Contains("Error: "))
            {
                Logger.LogError(e.Data);
            }
            else
            {
                Logger.LogInformation(e.Data);
            }
        }

        private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }

            Logger.LogError(e.Data);
        }
    }
}