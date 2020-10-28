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
    private PhysicsObject este;
    private PhysicsObject taso;
    private const double HYPPYNOPEUS = 850;
    public override void Begin()
    {
        Gravity = new Vector(0, -1000);
        LuoKentta();
        LisaaPelaaja();
        LisaaEste();
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
        AddCollisionHandler(pelaaja, "vihu", PelaajaOsuu);
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

    private void LisaaEste()
    {
        Image[] kuvat = new Image[2];
        kuvat[0] = LoadImage("aita");
        kuvat[1] = LoadImage("zombi");
        
        for (int i = 0; i < 1000; i+=2)
        {
            este = new PhysicsObject(40, 100);
            este.X = RandomGen.NextDouble(600*i,600*(i+1));
            este.Y = -150;
            este.IgnoresPhysicsLogics = true;
            este.Shape = Shape.Rectangle;
            int n = RandomGen.NextInt(kuvat.Length);
            este.Image = kuvat[n];
            este.Mass = 1000;
            este.Tag = "vihu";
            Add(este);
            este.MoveTo(new Vector(-800, -150), 200);
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

    private void PelaajaOsuu(PhysicsObject pelaaja, PhysicsObject este)
    {
       pelaaja.Destroy();
       MessageDisplay.Add("Hävisit pelin!");
    }

  
    // private void LisaaNopeutta()
    //{

    //}



}
