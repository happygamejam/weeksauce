namespace Rooms.RochesColorees
{
    public struct Step
    {
        public int[] StepDirection;
        public int NbSteps;
        public char Symbole;
        public int Case;
        public int Id;
        
        public Step(int id, char symbole, int newCase, int[] direction, int nbSteps)
        {
            Id = id;
            StepDirection = direction;
            NbSteps = nbSteps;
            Symbole = symbole;
            Case = newCase;
        }

        public override string ToString()
        {
            return "Step direction = (" + StepDirection[0] + "," + StepDirection[1] + ") \n NbSteps=" + NbSteps + " \n Symbole = " + Symbole;
        }
    }
}