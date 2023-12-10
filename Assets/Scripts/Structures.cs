public class Structures : Piece
{
    public override void SetMoves(bool validateMoves) { }
    public virtual int GetMoneyPerTurn() { return 0; }

    public virtual string StructType() { return "null"; }
}