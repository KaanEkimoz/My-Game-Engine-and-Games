using Engine;
using Microsoft.Xna.Framework;

namespace PenguinPairs.LevelObjects
{
    class MovableAnimalSelector : GameObjectList
    {
        Arrow[] arrows;
        Point[] directions;
        MovableAnimal selectedAnimal;
        public MovableAnimal SelectedAnimal
        {
            get { return selectedAnimal; }
            set
            {
                selectedAnimal = value;
                Visible = (selectedAnimal != null);
            }
        }

        public MovableAnimalSelector()
        {
            directions = new Point[4];
            directions[0] = new Point(1, 0);
            directions[1] = new Point(0, -1);
            directions[2] = new Point(-1, 0);
            directions[3] = new Point(0, 1);

            arrows = new Arrow[4];
            for (int i = 0; i < 4; i++)
            {
                arrows[i] = new Arrow(i);
                arrows[i].LocalPosition = new Vector2(
                directions[i].X * arrows[i].Width,
                directions[i].Y * arrows[i].Height);
                AddChild(arrows[i]);
            }
            SelectedAnimal = null;
        }
        public override void HandleInput(InputHelper inputHelper)
        {
            if (SelectedAnimal == null)
                return;
            base.HandleInput(inputHelper);

            for (int i = 0; i < 4; i++)
            {
                if (arrows[i].Pressed)
                {
                    SelectedAnimal.TryMoveInDirection(directions[i]);
                    return;
                }
            }
            if (inputHelper.MouseLeftButtonPressed())
                SelectedAnimal = null;
        }
        public override void Update(GameTime gameTime)
        {
            if (SelectedAnimal == null)
                return;
            base.Update(gameTime);

            if (SelectedAnimal != null)
            {
                LocalPosition = selectedAnimal.LocalPosition;
                for (int i = 0; i < 4; i++)
                    arrows[i].Visible = SelectedAnimal.CanMoveInDirection(directions[i]);
            }
        }
        public override void Reset()
        {
            base.Reset();
            SelectedAnimal = null; 
        }
    }
}
