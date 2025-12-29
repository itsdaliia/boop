namespace boop.Core.Input;

public interface IInputHandler {
    event Action<Key> OnKeyDown;
}