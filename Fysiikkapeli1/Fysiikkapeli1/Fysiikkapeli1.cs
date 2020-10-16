using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

public class Fysiikkapeli1 : PhysicsGame
{
    private PlatformCharacter pelaaja;
    private const double HYPPYNOPEUS = 800;
    public override void Begin()
    {
        Gravity = new Vector(0, -1000);
        LuoKentta();
        LisaaPelaaja();
        LisaaNappaimet();
        Camera.Follow(pelaaja);
        Camera.StayInLevel = true;

    }
    private void LuoKentta()
    {
        Surface alaReuna = Surface.CreateBottom(Level);
        Add(alaReuna);
        LisaaTaso(0, Level.Bottom);
        Level.Background.CreateGradient(Color.Wheat, Color.DarkOrange);

    }
    private void LisaaPelaaja()
    {
        pelaaja = new PlatformCharacter(50, 50);
        pelaaja.X = -250;
        pelaaja.Y = Level.Bottom;
        pelaaja.Shape = Shape.Circle;
        Add(pelaaja);
    }

    private void LisaaTaso(double x, double y)
    {
        PhysicsObject taso = PhysicsObject.CreateStaticObject(Screen.Width, 400);
        taso.Color = Color.DarkOrange;
        taso.X = x;
        taso.Y = y;
        Add(taso );
    }

    private void LisaaNappaimet()
    {
        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Up, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", pelaaja, HYPPYNOPEUS);
        ControllerOne.Listen(Button.Back, ButtonState.Pressed, Exit, "Poistu pelistä");
    }
    private void Hyppaa(PlatformCharacter pelaaja, double nopeus)
    {
        pelaaja.Jump(nopeus);
    }
}
