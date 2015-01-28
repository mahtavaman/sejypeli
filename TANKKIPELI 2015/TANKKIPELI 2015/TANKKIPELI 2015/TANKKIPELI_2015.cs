using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class TANKKIPELI_2015 : PhysicsGame
{
    public override void Begin()
    {
        // TODO: Kirjoita ohjelmakoodisi tähän

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
    void LuoKentta()
    {
    
        //1. Luetaan kuva uuteen ColorTileMappiin, kuvan nimen perässä ei .png-päätettä.
        ColorTileMap ruudut = ColorTileMap.FromLevelAsset("kentta1");

        //2. Kerrotaan mitä aliohjelmaa kutsutaan, kun tietyn värinen pikseli tulee vastaan kuvatiedostossa.
        ruudut.SetTileMethod(Color.Green, LuoPelaaja);
        ruudut.SetTileMethod(Color.Blue, LuoPelaaja);

        //3. Execute luo kentän
        //   Parametreina leveys ja korkeus
        ruudut.Execute(20, 20);
    }
void LuoPelaaja(Vector paikka, double leveys, double korkeus)
{
  PhysicsObject pelaaja = new PhysicsObject(10, 10);
  pelaaja.Position = paikka;
  AddCollisionHandler(pelaaja, "tahti", TormaaTahteen);
  Add(pelaaja);
}

void LuoTaso(Vector paikka, double leveys, double korkeus)
{
  PhysicsObject taso = PhysicsObject.CreateStaticObject(leveys, korkeus);
  taso.Position = paikka;
  taso.Image = groundImage;
  taso.CollisionIgnoreGroup = 1;
  Add(taso);
}
}
      
  