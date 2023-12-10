public class Units : Piece
{
    public override void SetMoves(bool validateMoves) { }
    public virtual int Costs() { return 0; }

    public virtual string UnitType() { return "null"; }
}
