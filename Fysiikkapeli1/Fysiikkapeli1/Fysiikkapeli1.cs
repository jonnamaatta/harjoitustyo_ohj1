using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Widgets;
using Microsoft.Xna.Framework.Graphics;

/// @author  Jonna Määttä
/// @version 24.10.2020
///
/// <summary>
/// Peli, jossa hypitään aitojen ja zombien yli.
/// </summary>

public class Fysiikkapeli1 : PhysicsGame
{
    private PlatformCharacter pelaaja;
    private PhysicsObject aita;
    private PhysicsObject zombi;
    private PhysicsObject taso;
    private const double HYPPYNOPEUS = 850;
    public override void Begin()
    {
        Gravity = new Vector(0, -1000);
        LuoKentta();
        LisaaPelaaja();
        LisaaAidat();
        LisaaZombit();
        LisaaNappaimet();
        Camera.Follow(pelaaja);
        Camera.StayInLevel = true;
       // Timer ajastin = new Timer();
       // ajastin.Interval = 1.5;
       // ajastin.Timeout += LisaaNopeutta;
       //  ajastin.Start();
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
        pelaaja.Y = -150;
        pelaaja.Shape = Shape.Circle;
        pelaaja.Restitution = 1.0;
        Add(pelaaja);
        AddCollisionHandler(pelaaja, "muuri", PelaajaOsuuAitaan);
        AddCollisionHandler(pelaaja, "pahis", PelaajaOsuuZombiin);
    }

    /// <summary>
    /// Aliohjelmassa lisätään peliin taso, jossa pelaaja liikkuu.
    /// </summary>
    /// <param name="x">Tason sijainti vaakasuunnassa.</param>
    /// <param name="y">Tason sijainti pystysuunnassa.</param>
    private void LisaaTaso(double x, double y)
    {
        taso = PhysicsObject.CreateStaticObject(Screen.Width, 400);
        taso.Color = Color.DarkOrange;
        taso.X = x;
        taso.Y = y;
        Add(taso);
    }

    private void LisaaAidat()
    {
        for (int i = 0; i < 1000; i+=2)
        {
            aita = new PhysicsObject(40, 100);
            aita.X = RandomGen.NextDouble(600*i,600*(i+1));
            aita.Y = -150;
            aita.IgnoresPhysicsLogics = true;
            aita.Shape = Shape.Rectangle;
            aita.Image = LoadImage("aita");
            aita.Mass = 1000;
            aita.Tag = "muuri";
            Add(aita);
            aita.MoveTo(new Vector(-800, -150), 200);
        }
    }

    private void LisaaZombit()
    {
        for (int i = 1; i < 1000; i += 2)
        {
            zombi = new PhysicsObject(30, 80);
            zombi.X = RandomGen.NextDouble(aita.Y + 900*i, aita.Y + 900 * (i+1));
            zombi.Y = -160;
            zombi.IgnoresPhysicsLogics = true;
            zombi.Shape = Shape.Rectangle;
            zombi.Mass = 1000;
            zombi.Tag = "pahis";
            Add(zombi);
            zombi.MoveTo(new Vector(-800, -160), 200);
        }
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

    private void PelaajaOsuuAitaan(PhysicsObject pelaaja, PhysicsObject aita)
    {
       pelaaja.Destroy();
       MessageDisplay.Add("Hävisit pelin! Et päässyt aidan yli ja zombi sai napattua sinut.");
    }

    private void PelaajaOsuuZombiin(PhysicsObject pelaaja, PhysicsObject zombi)
    {
        pelaaja.Destroy();
        MessageDisplay.Add("Hävisit pelin! Muutuit zombiksi.");
    }

    // private void LisaaNopeutta()
    //{

    //}



}
