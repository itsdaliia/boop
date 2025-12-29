namespace boop.Core.Input;

public interface ITextInput {
    event Action<char> OnCharTyped;
}