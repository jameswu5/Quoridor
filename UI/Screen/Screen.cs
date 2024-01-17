using System;

namespace Quoridor;

public abstract class Screen
{
    public System.Action<Quoridor.Action> clickAction;

    public abstract void Display();

    protected void AddButtonAction(RectangularButton button, Action action)
    {
        button.OnClick += () => clickAction(action);
    }
}