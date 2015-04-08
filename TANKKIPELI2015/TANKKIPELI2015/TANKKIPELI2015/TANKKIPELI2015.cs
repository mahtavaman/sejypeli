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
    PhysicsObject o;
    Vector nopeusYlos = new Vector(0, 200);
    Vector nopeusAlas = new Vector(0, -200);
    Vector nopeusVasen = new Vector(-200, 0);
    Vector nopeusoikea = new Vector(200, 0);
    Image neliomiehenkuva = LoadImage("neliömies");
    IntMeter pisteLaskuri;
    IntMeter ElamaLaskuri;
    FollowerBrain seuraajanAivot;
    Image taustaKuva = LoadImage("kenttatausta");
    private Animation sairaannopee;
    PhysicsObject m;

    public override void Begin()
    {
        sairaannopee = LoadAnimation("sairaannopee");
        LuoKentta();
        AsetaOhjaimet();

        PhoneBackButton.Listen(ConfirmExit, "Lopeta peli");
        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
        LuoPistelaskuri();
        pisteLaskuri.AddTrigger(0, TriggerDirection.Up, TuhoaTankki);
        LuoElamaLaskuri();
        Level.Background.Image = taustaKuva;
        Level.Background.FitToLevel();
        
    }

    void LuoPelaaja(Vector paikka, double leveys, double korkeus)
    {
        //PhysicsObject pelaaja = new PhysicsObject(10, 10);
        pelaaja = new Tank(90, 70);
        pelaaja.Position = paikka;
        AddCollisionHandler(pelaaja, PelaajaTormasi);
        pelaaja.Cannon.ProjectileCollision = AmmusOsui;
        pelaaja.Tag = "v";

        pelaaja.Cannon.Ammo.Value = 1000000;

        pelaaja.Restitution = 1.0;

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
        ruudut.SetTileMethod(Color.FromHexCode ("FFD800"), LuoPikkunelio);

        //3. Execute luo kentän
        //   Parametreina leveys ja korkeus
        ruudut.Execute(20, 20);
        seuraajanAivot = new FollowerBrain(pelaaja);
        o.Brain = seuraajanAivot;
        seuraajanAivot.Speed = 90; 
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
        Keyboard.Listen(Key.K, ButtonState.Down, NeliomiesKuolee, "kuolee");
                      
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
        o = new PhysicsObject (90, 70);
        o.Position = paikka;
        o.Image = neliomiehenkuva;
        o.Tag ="p";

        o.Restitution = 1.0;
        Add(o);
       
    

    }
   
    
    void PelaajaTormasi(PhysicsObject tormaaja, PhysicsObject kohde)
    {
        if(kohde.Tag.ToString() == "p")
        { 
            ElamaLaskuri.Value -= 1;
            if (ElamaLaskuri.Value == 0) 
            { 
            tormaaja.Destroy();
            }

           
            
        }
    }

    void NeliomiesKuolee()
    {
        o.Destroy();
    
    
    }
    void AmmusOsui(PhysicsObject ammus, PhysicsObject kohde)
    {
       ammus.Destroy();
        if (kohde.Tag.ToString() != "pikku") 
        {       
            
          
          Explosion rajahdys = new Explosion(50);
          rajahdys.Position = kohde.Position;
            Add(rajahdys);
           rajahdys.Speed = 500.0;
           rajahdys.Force = 1000;
        }
        if (kohde.Tag.ToString() == "p")
        { 
            
           pisteLaskuri.Value += 1;
           if (pisteLaskuri.Value == 7)
           {
               m.Animation.Start(4);
           }
            seuraajanAivot.Speed += 5;
           // if (pisteLaskuri.Value == 10)
            //{
             //   seuraajanAivot.Speed = 180;
            //}
        }


    }


    

    void LuoPistelaskuri()
    {
        pisteLaskuri = new IntMeter(0);

        Label pisteNaytto = new Label();
        pisteNaytto.X = Screen.Left + 100;
        pisteNaytto.Y = Screen.Top - 100;
        pisteNaytto.TextColor = Color.Black;
        pisteNaytto.Color = Color.White;

        pisteNaytto.BindTo(pisteLaskuri);
        Add(pisteNaytto);
    }

    void TuhoaTankki()
    {
        pelaaja.Destroy();
        ElamaLaskuri.Value -= 1;
        if(ElamaLaskuri.Value==0)
        {
            pelaaja.Destroy();
        } 
            
    }

    void LuoElamaLaskuri()
    {
        ElamaLaskuri = new IntMeter(5);

        Label pisteNaytto = new Label();
        pisteNaytto.X = Screen.Right - 100;
        pisteNaytto.Y = Screen.Top - 100;
        pisteNaytto.TextColor = Color.Black;
        pisteNaytto.Color = Color.White;

        pisteNaytto.BindTo(ElamaLaskuri);
        Add(pisteNaytto);
    }

    void LuoPikkunelio(Vector paikka, double leveys, double korkeus)
    {
        m = PhysicsObject.CreateStaticObject(90, 70);
        m.Position = paikka;
        Add(m);
        m.Color = Color.Blue;
        m.Tag = "pikku";
        m.Animation = new Animation(sairaannopee);
          
       // m.Animation.Stop();
        m.Animation.FPS = 10;
    }
}