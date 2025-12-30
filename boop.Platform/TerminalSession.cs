// Copyright (c) itsdaliia <me@daliia.ch>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence.

using System.Diagnostics;
using boop.Core.Terminal;

namespace boop.Platform;

public sealed class TerminalSession : ITerminalSession, IDisposable {
    public event Action<string>? OnOutput;
    public event Func<string, bool>? CommandInterceptor;

    private Process? _process;

    public void Start() {
        _process = new Process {
            StartInfo = new ProcessStartInfo {
                FileName = "/bin/bash",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        _process.OutputDataReceived += (_, args) => {
            if (args.Data != null) {
                OnOutput?.Invoke(args.Data);
            }
        };

        _process.ErrorDataReceived += (_, args) => {
            if (args.Data != null) {
                OnOutput?.Invoke("[err] " + args.Data);
            }
        };

        _process.Start();
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();
    }

    public void Send(string cmd) {
        if (CommandInterceptor?.Invoke(cmd) == true) {
            return;
        }

        _process?.StandardInput.WriteLine(cmd);
        _process?.StandardInput.Flush();
    }

    public void Dispose() {
        _process?.Kill();
        _process?.Dispose();
    }
}
