namespace Bejeweled
{
    class Tile
    {
        //Tile type
        string tileType;
        public Tile()
        {
            tileType = "-";
        }
        //Gives back if it equals to another tile
        public bool equals(Tile t)
        {
            return tileType == t.TileType;
        }
        //Getter-Setter to the type
        public string TileType
        {
            get { return tileType; }
            set { tileType = value; }
        }
    }
}
