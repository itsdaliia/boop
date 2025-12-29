// Copyright (c) itsdaliia <me@daliia.ch>. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full licence.

namespace boop.Core.Input;

public interface IInputHandler {
    event Action<Key> OnKeyDown;
    event Action<Key> OnKeyUp;
}