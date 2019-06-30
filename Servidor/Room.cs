using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servidor
{
    // Estados possíveis para o jogo
    enum State
    {
        Waiting,
        Player1Turn,
        Player2Turn,
        Finish
    }

    class Room
    {
        private string roomName;
        private User player1;
        private User player2;
        private int[] player1Play;
        private int[] player2Play;
        private State gameState;

        public Room(string roomName, User player1)
        {
            this.roomName = roomName;
            this.player1 = player1;
            this.player2 = new User();
            this.player1Play = new int[2] { 0, 0 };
            this.player2Play = new int[2] { 0, 0 };
            this.gameState = State.Waiting;
        }

        public State GetGameState()
        {
            return gameState;
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
        }

        public string GetPlayer1Name()
        {
            return player1.Username;
        }

        public string GetPlayer2Name()
        {
            return player2.Username;
        }
    }
}
