namespace StandardLibrary;

public class ElanException : Exception {
    public ElanException(string message) : base(message) { }

    public ElanException(Exception e) : base(e.Message) { }

    public string message => Message;
}