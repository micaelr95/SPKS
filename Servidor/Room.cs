using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Servidor
{
    // Estados possíveis para o jogo
    enum State
    {
        Waiting,
        Player1Turn,
        Player2Turn,
        GameOver
    }

    class Room
    {
        private string roomName;
        private User player1;
        private User player2;
        private string player1Play;
        private string player2Play;
        private int player1Pontos;
        private int player2Pontos;
        private State gameState;

        public string Player1Play { set => player1Play = value; }
        public string Player2Play { set => player2Play = value; }
        internal State GameState { get => gameState; }
        public int Player1Pontos { get => player1Pontos; }
        public int Player2Pontos { get => player2Pontos; }

        public Room(string roomName, User player1)
        {
            this.roomName = roomName;
            this.player1 = player1;
            this.player2 = new User();
            this.player1Play = "";
            this.player2Play = "";
            this.gameState = State.Waiting;
        }   

        public override string ToString()
        {
            return roomName;
        }

        public void ChangeTurn()
        {
            if (gameState == State.Player1Turn)
                gameState = State.Player2Turn;
            else
                gameState = State.Player1Turn;
        }

        public void AddPlayer2(User player2)
        {
            this.player2 = player2;
            gameState = State.Player1Turn;
            new Thread(Loop).Start();
        }

        public string GetPlayer1Name()
        {
            return player1.Username;
        }

        public string GetPlayer2Name()
        {
            return player2.Username;
        }

        /// <summary>
        /// Verifica de quem é o ponto
        /// </summary>
        private void VerificaPonto()
        {
            if (GameState == State.Player1Turn && player1Play == player2Play)
            {
                player2Pontos += 0;
            }
            else if (GameState == State.Player2Turn && player1Play == player2Play)
            {
                player1Pontos += 0;
            }
            else if (GameState == State.Player1Turn && player1Play != player2Play)
            {
                player1Pontos += 1;
            }
            else if (GameState == State.Player2Turn && player1Play != player2Play)
            {
                player2Pontos += 1;
            }
        }

        private void Loop()
        {
            while (true)
            {
                if(player1Play != "" && player2Play != "")
                {
                    VerificaPonto();
                    player1Play = "";
                    player2Play = "";
                    ChangeTurn();
                }
            }
        }
    }
}
