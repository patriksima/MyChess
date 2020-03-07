namespace MyChess
{
    using ChessBoard;
    using UnityEngine;

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private Board board;
        [SerializeField] private InfoPanel infoPanel;
        [SerializeField] private PieceManager pieceManager;

        public InfoPanel InfoPanel => infoPanel;

        public GameData GameData { get; set; } = new GameData();

        // Start is called before the first frame update
        private void Start()
        {
            board.Create();
            pieceManager.Setup(board);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKey("escape"))
            {
                Quit();
            }
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}