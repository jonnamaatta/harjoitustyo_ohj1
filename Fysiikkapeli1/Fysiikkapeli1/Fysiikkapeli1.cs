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
    private const double HYPPYNOPEUS = 800;
    private static Image[] ukkelinJuoksu = LoadImages("ukko1", "ukko2", "ukko3", "ukko4");
    private static Image[] ukkelinHyppy = LoadImages("ukkojump", "ukko1");
    private Animation juoksuanimaatio = new Animation(ukkelinJuoksu);
    private Animation hyppyanimaatio = new Animation(ukkelinHyppy);
    public override void Begin()
    {
        MultiSelectWindow alkuValikko = new MultiSelectWindow("Apocalypse Run",
        "Aloita peli", "Lopeta peli");
        Add(alkuValikko);
        alkuValikko.AddItemHandler(0, AloitaPeli);
        alkuValikko.AddItemHandler(1, Exit);
    }

    private void AloitaPeli()
    {
        Gravity = new Vector(0, -1000);
        LuoKentta();
        LisaaPelaaja();
        LisaaEste();
        LisaaNappaimet();
        Camera.StayInLevel = true;
        LuoPistelaskuri();
    }

    /// <summary>
    /// Luodaan kenttä.
    /// </summary>
    private void LuoKentta()
    {
        Surface alaReuna = Surface.CreateBottom(Level);
        Add(alaReuna);
        LisaaTaso(0, Level.Bottom);
        Level.Background.CreateGradient(Color.Wheat, Color.DarkOrange);
    }

    /// <summary>
    /// Luodaan ohjattava pelaaja.
    /// </summary>
    private void LisaaPelaaja()
    {
        PlatformCharacter pelaaja = new PlatformCharacter(80, 80);
        pelaaja.X = -250;
        pelaaja.Y = -150;
        pelaaja.Shape = Shape.Circle;
        pelaaja.Restitution = 1.0;
        Add(pelaaja);
        AddCollisionHandler(pelaaja, "vihu", PelaajaOsuu);
        pelaaja.AnimIdle = juoksuanimaatio;
        pelaaja.AnimJump = hyppyanimaatio;
        Keyboard.Listen(Key.Space, ButtonState.Pressed, Hyppaa, "Pelaaja hyppää", pelaaja, HYPPYNOPEUS);
    }

    /// <summary>
    /// Aliohjelmassa lisätään peliin taso, jossa pelaaja liikkuu.
    /// </summary>
    /// <param name="x">Tason sijainti vaakasuunnassa.</param>
    /// <param name="y">Tason sijainti pystysuunnassa.</param>
    private void LisaaTaso(double x, double y)
    {
        PhysicsObject taso = PhysicsObject.CreateStaticObject(Screen.Width, 400);
        taso.Color = Color.DarkOrange;
        taso.X = x;
        taso.Y = y;
        Add(taso);
    }

    /// <summary>
    /// Luodaan esteitä, jonka päältä pelaaja hyppii. Spawnataan joko zombi tai aita.
    /// </summary>
    private void LisaaEste()
    {
        Image[] kuvat = new Image[2];
        kuvat[0] = LoadImage("aita");
        kuvat[1] = LoadImage("zombi");
        int nopeus = 300;
        
        for (int i = 0; i < 1000; i+=2)
        {
            PhysicsObject este = new PhysicsObject(50, 100);
            este.X = RandomGen.NextDouble(600*i,600*(i+1));
            este.Y = -150;
            este.IgnoresPhysicsLogics = true;
            este.Shape = Shape.Rectangle;
            int n = RandomGen.NextInt(kuvat.Length);
            este.Image = kuvat[n];
            este.Mass = 1000;
            este.Tag = "vihu";
            Add(este);
            este.MoveTo(new Vector(-800, -150), nopeus);
            nopeus += 4;
        }
    }
 

    /// <summary>
    /// Lisätään peliin näppäinkomennot.
    /// </summary>
    private void LisaaNappaimet()
    {
        Keyboard.Listen(Key.F1, ButtonState.Pressed, ShowControlHelp, "Näytä ohjeet");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        ControllerOne.Listen(Button.Back, ButtonState.Pressed, Exit, "Poistu pelistä");
    }

    /// <summary>
    /// Lisätään pelaajalle hyppy.
    /// </summary>
    /// <param name="pelaaja">Ohjattava pelaaja.</param>
    /// <param name="nopeus">Annettu nopeus.</param>
    private void Hyppaa(PlatformCharacter pelaaja, double nopeus)
    {
        pelaaja.Jump(nopeus);
    }

    /// <summary>
    /// Lisätään näytölle teksti, kun pelaaja osuu esteeseen.
    /// </summary>
    /// <param name="pelaaja">Ohjattava pelaaja.</param>
    /// <param name="este">Objekti, johon pelaaja törmää. Joko zombi tai aita.</param>
    private void PelaajaOsuu(PhysicsObject pelaaja, PhysicsObject este)
    {
       pelaaja.Destroy();
       MessageDisplay.Add("Hävisit pelin!");
       AloitaAlusta();
    }

    /// <summary>
    /// Lisätään toiminto, että peli alkaa alusta, kun pelaaja törmää esteeseen.
    /// </summary>
    private void AloitaAlusta()
    {
        ClearAll();
        MultiSelectWindow alkuValikko = new MultiSelectWindow("Hävisit pelin!",
        "Yritä Uudelleen", "Lopeta peli");
        Add(alkuValikko);
        alkuValikko.AddItemHandler(0, AloitaPeli);
        alkuValikko.AddItemHandler(1, Exit);
    }

    /// <summary>
    /// Luo peliin pistelaskurin.
    /// </summary>
    private void LuoPistelaskuri()
    {
        IntMeter pisteLaskuri = new IntMeter(0, 0, int.MaxValue);
        Label pisteNaytto = new Label();
        pisteNaytto.X = Screen.Right - 100;
        pisteNaytto.Y = Screen.Top - 50;
        pisteNaytto.TextColor = Color.Black;
        pisteNaytto.Color = Color.White;
        pisteNaytto.Title = "Pisteet";
        pisteNaytto.BindTo(pisteLaskuri);
        Add(pisteNaytto);
        pisteLaskuri.AddOverTime(9999, 300);
        if (pisteLaskuri.Value > 9999)
        {
            PeliLoppuu();
        }
    }

    /// <summary>
    /// Jos pelaaja saa enemmän kuin 9999 pistettä, niin peli loppuu tällä tavalla.
    /// </summary>
    private void PeliLoppuu()
    {
        ClearAll();
        MultiSelectWindow alkuValikko = new MultiSelectWindow("Onnittelut! Sait parhaimman mahdollisen tuloksen.",
        "Yritä Uudelleen", "Lopeta peli");
        Add(alkuValikko);
        alkuValikko.AddItemHandler(0, AloitaPeli);
        alkuValikko.AddItemHandler(1, Exit);
    }
}
