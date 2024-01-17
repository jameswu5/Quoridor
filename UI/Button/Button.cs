using System;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Quoridor.Settings;

namespace PlayerAlbum;

public abstract class Button
{
    public event System.Action OnClick;

    protected string name;
    protected string text;
    protected Color textColour;
    protected int fontSize;

    // Deactivated buttons still display, but cannot be clicked or hovered
    protected bool activated;

    public Button(string? name = null, string? text = null, Color? textColour = null, int? fontSize = null)
    {
        this.name = name ?? "";
        this.text = text ?? "";
        this.textColour = textColour ?? DefaultDarkTextColour;
        this.fontSize = fontSize ?? DefaultFontSize;
        activated = true;
    }

    public void Render()
    {
        if (!activated)
        {
            Display();
            return;
        }

        if (IsHovered(GetMouseX(), GetMouseY()))
        {
            if (IsMouseButtonDown(0))
            {
                PressedDisplay();                
            }
            else
            {
                HoverDisplay();
            }

            if (IsMouseButtonReleased(0))
            {
                Click();
            }   
        }
        else
        {
            Display();
        }
    }

    protected abstract bool IsHovered(float x, float y);

    protected virtual void Display() {}

    protected virtual void DisplayText() {}

    protected virtual void HoverDisplay() => Display();

    protected virtual void PressedDisplay() => HoverDisplay();

    protected virtual void Click() => OnClick.Invoke();

    public void Activate() => activated = true;

    public void Deactivate() => activated = false;
}