// Copyright (c) itsdaliia <me@daliia.ch>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence.

namespace boop.Core.Terminal;

public interface ITerminalSession {
    public event Action<string>? OnOutput;
    public event Func<string, bool>? CommandInterceptor;

    public void Send(string cmd);
}
