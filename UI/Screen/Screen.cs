using System;

namespace Quoridor;

public abstract class Screen
{
    public System.Action<Quoridor.Action> clickAction;

    public abstract void Display();
}