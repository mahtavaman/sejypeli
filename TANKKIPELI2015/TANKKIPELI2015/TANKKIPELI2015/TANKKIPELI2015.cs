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
        Keyboard.Listen(Key.Up, ButtonState.Down, Ylosliikunta, "liikkuu ylös");
        Keyboard.Listen(Key.Down, ButtonState.Down, Alasliikunta, "liikkuu alas");
        Keyboard.Listen(Key.Left, ButtonState.Down, Vasemmalleliikunta, "liikkuu vasemmalle");
        Keyboard.Listen(Key.Right, ButtonState.Down, Oikealleliikunta, "liikkuu oikealle");
    }
    void Ammunta()
    {
        pelaaja.Shoot();
    }
    void LiikutaPelaajaaYlos()
    {
         pelaaja.
    }
}