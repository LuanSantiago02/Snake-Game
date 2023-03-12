using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace SNAKE
{
    internal class Game
    {
        public Keys Direction { get; set; }
        public Keys Arrow { get; set; }
        private Timer Frame { get; set; }
        private Label lbPontos { get; set; }
        private Panel pnTela { get; set; }

        private int pontos = 0;

        private Food Food;

        private Snake Snake;

        private Bitmap offScreenBitmap;

        private Graphics bitmapGraph;

        private Graphics ScreenGraph;

        public Game(ref Timer timer, ref Label label, ref Panel panel)
        {
            //Recebendo os Valores
            pnTela = panel;
            Frame = timer;
            lbPontos = label;

            //Criando as Medidas da nossa tela
            offScreenBitmap = new Bitmap(472,472);

            //Iniciando as classes Food e Snake
            Snake = new Snake();
            Food = new Food();

            //Setando a direcao que vai iniciar a cobra
            Direction = Keys.Right;
            Arrow = Direction;
        }

        public void StartGame()
        {
            Snake.Reset();
            Food.CreateFood();
            Direction = Keys.Right;
            bitmapGraph = Graphics.FromImage(offScreenBitmap);
            ScreenGraph = pnTela.CreateGraphics();
            Frame.Enabled= true;
        }

        public void Tick()
        {
           if (((Arrow == Keys.Left) && (Direction != Keys.Right)) ||
               ((Arrow == Keys.Right) && (Direction != Keys.Left)) ||
               ((Arrow == Keys.Up) && (Direction != Keys.Down)) ||
               ((Arrow == Keys.Down) && (Direction != Keys.Up)))
            {
                Direction = Arrow;
            }

           switch(Direction)
            {
                case Keys.Left:
                    Snake.Left();
                    break;
                case Keys.Right:
                    Snake.Right();
                    break;
                case Keys.Up:
                    Snake.Up();
                    break;
                case Keys.Down:
                    Snake.Down();
                    break;
            }

            bitmapGraph.Clear(Color.White);
            bitmapGraph.DrawImage(Properties.Resources.pngfind_com_maa_png_4612413, (Food.Location.X *15), (Food.Location.Y * 15),15,15);

            bool gameOver = false;

            for(int i= 0; i < Snake.Lenght; i++)
            {
                if (i == 0)
                {
                  bitmapGraph.FillEllipse(new SolidBrush(ColorTranslator.FromHtml("#228B22")), (Snake.Location[i].X * 15), (Snake.Location[i].Y * 15), 15, 15);
                }

                else
                {
                  bitmapGraph.FillEllipse(new SolidBrush(ColorTranslator.FromHtml("#32CD32")), (Snake.Location[i].X * 15), (Snake.Location[i].Y * 15), 15, 15);
                }

                if ((Snake.Location[i] == Snake.Location[0]) && (i > 0))
                {
                  gameOver = true;
                }

                ScreenGraph.DrawImage(offScreenBitmap, 0, 0);

                CheckCollision();

                if (gameOver)
                {
                    GameOver();
                }
            }
        }

        public void CheckCollision()
        {
            if (Snake.Location[0] == Food.Location)
            {
                Snake.Eat();
                Food.CreateFood();
                pontos += 10;
                lbPontos.Text = "PONTOS:" + pontos;
            }
        }

        public void GameOver()
        {
            Frame.Enabled = false;
            bitmapGraph.Dispose();
            ScreenGraph.Dispose();
            lbPontos.Text = "PONTOS: 0";
            pontos = 0;
            MessageBox.Show("Game Over");
        }
    }
}
