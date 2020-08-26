using System;

public class Button
{
    public readonly string Name;
    public readonly Action Action;
    public Button(string name, Action action)
    {
        Name = name;
        Action = action;
    }
}
