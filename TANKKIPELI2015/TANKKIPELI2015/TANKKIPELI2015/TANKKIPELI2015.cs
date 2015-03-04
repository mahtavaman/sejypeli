using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class TANKKIPELI2015 : PhysicsGame
{
    Tank pelaaja;
    Vector nopeusYlos = new Vector(0, 200);
    Vector nopeusAlas = new Vector(0, -200);
    Vector nopeusVasen = new Vector(-200, 0);
    Vector nopeusoikea = new Vector(200, 0);
    Image neliomiehenkuva = LoadImage("neliömies");

    public override void Begin()
    {
        LuoKentta();
        AsetaOhjaimet();

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }

    void LuoPelaaja(Vector paikka, double leveys, double korkeus)
    {
        //PhysicsObject pelaaja = new PhysicsObject(10, 10);
        pelaaja = new Tank(90, 70);
        pelaaja.Position = paikka;

        Add(pelaaja);
    }
    void LuoKentta()
    {
        //1. Luetaan kuva uuteen ColorTileMappiin, kuvan nimen perässä ei .png-päätettä.
        ColorTileMap ruudut = ColorTileMap.FromLevelAsset("kentta1");

        //2. Kerrotaan mitä aliohjelmaa kutsutaan, kun tietyn värinen pikseli tulee vastaan kuvatiedostossa.
        ruudut.SetTileMethod(Color.FromHexCode("00FF21"), LuoPelaaja);
        ruudut.SetTileMethod(Color.FromHexCode("0026FF"), LuoNeliomies);
        ruudut.SetTileMethod(Color.Black, LuoTaso);


        //3. Execute luo kentän
        //   Parametreina leveys ja korkeus
        ruudut.Execute(20, 20);
    }
    void LuoTaso(Vector paikka, double leveys, double korkeus)
    {
        PhysicsObject taso = PhysicsObject.CreateStaticObject(leveys, korkeus);
        taso.Position = paikka;
        taso.Color = Color.Brown;
        taso.CollisionIgnoreGroup = 1;
        Add(taso);
    }
    void AsetaOhjaimet()
    {
        Keyboard.Listen(Key.Space, ButtonState.Down, Ammunta, "ampuu tankilla");
        Keyboard.Listen(Key.Up, ButtonState.Down, TankkiLiikkuu, "liikkuu ylös",nopeusYlos);
        Keyboard.Listen(Key.Up, ButtonState.Released, TankkiLiikkuu, "pysähtyy",Vector.Zero);
        Keyboard.Listen(Key.Down, ButtonState.Down, TankkiLiikkuu, "liikkuu alas",nopeusAlas);
        Keyboard.Listen(Key.Down, ButtonState.Released, TankkiLiikkuu, "pysähtyy",Vector.Zero);
        Keyboard.Listen(Key.Left, ButtonState.Down, TankkiLiikkuu, "liikkuu vasemmalle",nopeusVasen);
        Keyboard.Listen(Key.Left, ButtonState.Released, TankkiLiikkuu, "pysähtyy",Vector.Zero);
        Keyboard.Listen(Key.Right, ButtonState.Down, TankkiLiikkuu, "liikkuu oikealle",nopeusoikea);
        Keyboard.Listen(Key.Right, ButtonState.Released, TankkiLiikkuu, "pysähtyy",Vector.Zero);                               
    }
    void Ammunta()
    {
        pelaaja.Shoot();
    }
    void TankkiLiikkuu(Vector nopeus)
    {
        pelaaja.Velocity = nopeus;
    }


    void LuoNeliomies(Vector paikka, double leveys, double korkeus)
    {
        pelaaja = new Tank(90, 70);
        pelaaja.Position = paikka;

        Add(pelaaja);
    }

}