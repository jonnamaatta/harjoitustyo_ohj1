using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;

public class harjoitustyo : PhysicsGame
{
    private PlatformCharacter pelaaja;
    public override void Begin()
    {
        Gravity = new Vector(0, -1000);
        LuoKentta();
        Camera.Follow(pelaaja);
        Camera.ZoomFactor = 1.2;
        Camera.StayInLevel = true;
    }
    private void LuoKentta()
    {
        Level.CreateBorders();
        Level.Background.CreateGradient(Color.Wheat, Color.DarkOrange);
    }

}
