// Copyright (c) itsdaliia <me@daliia.ch>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence.

namespace boop.Core.Terminal;

public class TerminalBuffer {
    public readonly List<string> Lines = [];

    public void Print(string line) {
        Lines.Add(line);
        if (Lines.Count > 15) {
            Lines.RemoveAt(0);
        }
    }
}
