namespace Rooms.RochesColorees
{
    public struct Step
    {
        public int[] StepDirection;
        public int NbSteps;
        public char Symbol;
        public int Tile;
        public int Id;
        
        public Step(int id, char symbol, int newCase, int[] direction, int nbSteps)
        {
            Id = id;
            StepDirection = direction;
            NbSteps = nbSteps;
            Symbol = symbol;
            Tile = newCase;
        }

        public override string ToString()
        {
            return "Step direction = (" + StepDirection[0] + "," + StepDirection[1] + ") \n NbSteps=" + NbSteps + " \n Symbole = " + Symbol;
        }
    }
}