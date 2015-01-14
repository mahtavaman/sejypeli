using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;

public class pong : PhysicsGame
{
    PhysicsObject pallo;
    public override void Begin()
    {
        // TODO: Kirjoita ohjelmakoodisi tähä
        LuoKentta();
        Vector impulssi = new Vector(500.0, 0.0);
        pallo.Hit(impulssi);


        Keyboard.Listen(Key.Escape, ButtonState.Pressed, ConfirmExit, "Lopeta peli");
    }
    void LuoKentta()
    {
        pallo = new PhysicsObject(40.0, 40.0);
        pallo.Shape = Shape.Circle;
        Add(pallo);
        PhysicsObject maila = PhysicsObject.CreateStaticObject(20.0, 100.0);
        maila.Shape = Shape.Rectangle;
        maila.X = Level.Left + 20.0;
        maila.Y = 0.0;
        maila.Restitution =  1.0;
        Add(maila);

        

        Level.CreateBorders(1.0, false);
        pallo.X = -200.0;
        pallo.Y = 0.0;
        pallo.Restitution = 1.0;
        Level.Background.Color = Color.Black;
        Camera.ZoomToLevel();
    }
    void AloitaPeli()
    {
    }
}
     
     