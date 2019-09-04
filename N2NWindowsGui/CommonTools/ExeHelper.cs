using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
    public class ExeHelper
    {
        public static Process CreateCmdProcess(string command, string arguments, string workDirPath = "")
        {
            ProcessStartInfo processInfo = new ProcessStartInfo(command, arguments);

            processInfo.UseShellExecute = false;

            processInfo.RedirectStandardOutput = true;

            processInfo.RedirectStandardError = true;

            processInfo.CreateNoWindow = true;

            if (!string.IsNullOrWhiteSpace(workDirPath))
            {
                processInfo.WorkingDirectory = workDirPath;
            }

            Process process = new Process();

            process.StartInfo = processInfo;

            return process;
        }

        /// <summary>
        /// 使用cmd执行命令。并检测命令执行时的输出是否包含指定字符
        /// </summary>
        /// <param name="command">要执行的命令</param>
        /// <param name="workDirPath">工作目录</param>
        /// <param name="resultstr">执行正确时输出应该包含的字符串</param>
        /// <returns>是否包含指定字符串</returns>
        public static bool RunCmdCommand(string command, string arguments, string workDirPath = "", string resultstr = "")
        {
            bool installed = true;

            ProcessStartInfo processInfo = null;

            Process process = null;

            try
            {
                processInfo = new ProcessStartInfo(command, arguments);

                processInfo.UseShellExecute = false;

                processInfo.RedirectStandardOutput = true;

                processInfo.RedirectStandardError = true;

                processInfo.CreateNoWindow = true;

                if (!string.IsNullOrWhiteSpace(workDirPath))
                {
                    processInfo.WorkingDirectory = workDirPath;
                }

                LogHelper.Instance.WriteInfo("Command Start Run");

                process = new Process();

                process.StartInfo = processInfo;

                process.Start();

                string str = process.StandardOutput.ReadToEnd();

                string err = process.StandardError.ReadToEnd();

                int exitCode = process.ExitCode;

                LogHelper.Instance.WriteInfo("Command Exit Code = {0}", exitCode);

                if (!string.IsNullOrWhiteSpace(str))
                {
                    LogHelper.Instance.WriteInfo("Command Output Info is:\n {0}", str);
                    if (str.IndexOf(resultstr) == -1)
                    {
                        installed = false;
                    }
                }

                if (!string.IsNullOrWhiteSpace(err))
                {
                    LogHelper.Instance.WriteInfo("Command Error Output Info is:\n {0}", err);
                }

                LogHelper.Instance.WriteInfo("Command Finished");

            }
            catch (Exception e)
            {
                LogHelper.Instance.WriteError("Command Run WithOut Exception");
                LogHelper.Instance.WriteError(e);
            }
            finally
            {
                processInfo = null;

                if (process != null)
                {
                    process.Close();
                    process = null;
                }
            }

            return installed;
        }
    }
}
