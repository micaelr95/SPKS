namespace Cliente
{
    partial class Window
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBoxChat = new System.Windows.Forms.GroupBox();
            this.listBoxChat = new System.Windows.Forms.ListBox();
            this.TextBoxMensagemEnviar = new System.Windows.Forms.TextBox();
            this.buttonEnviar = new System.Windows.Forms.Button();
            this.groupBoxAutenticacao = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonEntrar = new System.Windows.Forms.Button();
            this.textBoxNome = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.buttonCantoInferiorDireito = new System.Windows.Forms.Button();
            this.buttonCentroBaixo = new System.Windows.Forms.Button();
            this.buttonCantoInferiorEsquerdo = new System.Windows.Forms.Button();
            this.buttonCantoSuperiorDireito = new System.Windows.Forms.Button();
            this.buttonCentroCima = new System.Windows.Forms.Button();
            this.buttonCantoSuperiorEsquerdo = new System.Windows.Forms.Button();
            this.labelPlayer2Ponto5 = new System.Windows.Forms.Label();
            this.labelPlayer2Ponto4 = new System.Windows.Forms.Label();
            this.labelPlayer2Ponto3 = new System.Windows.Forms.Label();
            this.labelPlayer2Ponto2 = new System.Windows.Forms.Label();
            this.labelPlayer2Ponto1 = new System.Windows.Forms.Label();
            this.labelPlayer1Ponto5 = new System.Windows.Forms.Label();
            this.labelPlayer1Ponto4 = new System.Windows.Forms.Label();
            this.labelPlayer1Ponto3 = new System.Windows.Forms.Label();
            this.labelPlayer1Ponto2 = new System.Windows.Forms.Label();
            this.labelPlayer1Ponto1 = new System.Windows.Forms.Label();
            this.labelPlayer2Name = new System.Windows.Forms.Label();
            this.labelPlayer1Name = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBoxJogo = new System.Windows.Forms.GroupBox();
            this.groupBoxChat.SuspendLayout();
            this.groupBoxAutenticacao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBoxJogo.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // groupBoxChat
            // 
            this.groupBoxChat.Controls.Add(this.listBoxChat);
            this.groupBoxChat.Controls.Add(this.TextBoxMensagemEnviar);
            this.groupBoxChat.Controls.Add(this.buttonEnviar);
            this.groupBoxChat.Enabled = false;
            this.groupBoxChat.Location = new System.Drawing.Point(477, 178);
            this.groupBoxChat.Name = "groupBoxChat";
            this.groupBoxChat.Size = new System.Drawing.Size(248, 308);
            this.groupBoxChat.TabIndex = 58;
            this.groupBoxChat.TabStop = false;
            this.groupBoxChat.Text = "Chat da sala";
            // 
            // listBoxChat
            // 
            this.listBoxChat.FormattingEnabled = true;
            this.listBoxChat.Location = new System.Drawing.Point(11, 18);
            this.listBoxChat.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxChat.Name = "listBoxChat";
            this.listBoxChat.ScrollAlwaysVisible = true;
            this.listBoxChat.Size = new System.Drawing.Size(227, 251);
            this.listBoxChat.TabIndex = 9;
            // 
            // TextBoxMensagemEnviar
            // 
            this.TextBoxMensagemEnviar.Location = new System.Drawing.Point(11, 274);
            this.TextBoxMensagemEnviar.Multiline = true;
            this.TextBoxMensagemEnviar.Name = "TextBoxMensagemEnviar";
            this.TextBoxMensagemEnviar.Size = new System.Drawing.Size(162, 22);
            this.TextBoxMensagemEnviar.TabIndex = 1;
            // 
            // buttonEnviar
            // 
            this.buttonEnviar.Location = new System.Drawing.Point(179, 272);
            this.buttonEnviar.Name = "buttonEnviar";
            this.buttonEnviar.Size = new System.Drawing.Size(59, 24);
            this.buttonEnviar.TabIndex = 2;
            this.buttonEnviar.Text = "Enviar";
            this.buttonEnviar.UseVisualStyleBackColor = true;
            this.buttonEnviar.Click += new System.EventHandler(this.buttonEnviar_Click_1);
            // 
            // groupBoxAutenticacao
            // 
            this.groupBoxAutenticacao.Controls.Add(this.label3);
            this.groupBoxAutenticacao.Controls.Add(this.buttonEntrar);
            this.groupBoxAutenticacao.Controls.Add(this.textBoxNome);
            this.groupBoxAutenticacao.Controls.Add(this.label4);
            this.groupBoxAutenticacao.Controls.Add(this.label1);
            this.groupBoxAutenticacao.Controls.Add(this.textBox2);
            this.groupBoxAutenticacao.Controls.Add(this.textBoxIP);
            this.groupBoxAutenticacao.Location = new System.Drawing.Point(477, 15);
            this.groupBoxAutenticacao.Name = "groupBoxAutenticacao";
            this.groupBoxAutenticacao.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.groupBoxAutenticacao.Size = new System.Drawing.Size(248, 135);
            this.groupBoxAutenticacao.TabIndex = 57;
            this.groupBoxAutenticacao.TabStop = false;
            this.groupBoxAutenticacao.Text = "Autenticação";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Servidor:";
            // 
            // buttonEntrar
            // 
            this.buttonEntrar.Location = new System.Drawing.Point(9, 98);
            this.buttonEntrar.Name = "buttonEntrar";
            this.buttonEntrar.Size = new System.Drawing.Size(231, 31);
            this.buttonEntrar.TabIndex = 35;
            this.buttonEntrar.Text = "Autenticar";
            this.buttonEntrar.UseVisualStyleBackColor = true;
            this.buttonEntrar.Click += new System.EventHandler(this.buttonEntrar_Click);
            // 
            // textBoxNome
            // 
            this.textBoxNome.Location = new System.Drawing.Point(70, 46);
            this.textBoxNome.Name = "textBoxNome";
            this.textBoxNome.Size = new System.Drawing.Size(168, 20);
            this.textBoxNome.TabIndex = 29;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "Password:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Nome:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(70, 72);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(168, 20);
            this.textBox2.TabIndex = 33;
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(70, 20);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(168, 20);
            this.textBoxIP.TabIndex = 31;
            this.textBoxIP.Text = "localhost";
            // 
            // buttonCantoInferiorDireito
            // 
            this.buttonCantoInferiorDireito.Location = new System.Drawing.Point(307, 200);
            this.buttonCantoInferiorDireito.Name = "buttonCantoInferiorDireito";
            this.buttonCantoInferiorDireito.Size = new System.Drawing.Size(76, 65);
            this.buttonCantoInferiorDireito.TabIndex = 56;
            this.buttonCantoInferiorDireito.UseVisualStyleBackColor = true;
            // 
            // buttonCentroBaixo
            // 
            this.buttonCentroBaixo.Location = new System.Drawing.Point(190, 200);
            this.buttonCentroBaixo.Name = "buttonCentroBaixo";
            this.buttonCentroBaixo.Size = new System.Drawing.Size(76, 65);
            this.buttonCentroBaixo.TabIndex = 55;
            this.buttonCentroBaixo.UseVisualStyleBackColor = true;
            // 
            // buttonCantoInferiorEsquerdo
            // 
            this.buttonCantoInferiorEsquerdo.Location = new System.Drawing.Point(75, 200);
            this.buttonCantoInferiorEsquerdo.Name = "buttonCantoInferiorEsquerdo";
            this.buttonCantoInferiorEsquerdo.Size = new System.Drawing.Size(76, 65);
            this.buttonCantoInferiorEsquerdo.TabIndex = 54;
            this.buttonCantoInferiorEsquerdo.UseVisualStyleBackColor = true;
            // 
            // buttonCantoSuperiorDireito
            // 
            this.buttonCantoSuperiorDireito.Location = new System.Drawing.Point(307, 109);
            this.buttonCantoSuperiorDireito.Name = "buttonCantoSuperiorDireito";
            this.buttonCantoSuperiorDireito.Size = new System.Drawing.Size(76, 65);
            this.buttonCantoSuperiorDireito.TabIndex = 53;
            this.buttonCantoSuperiorDireito.TabStop = false;
            this.buttonCantoSuperiorDireito.UseVisualStyleBackColor = true;
            // 
            // buttonCentroCima
            // 
            this.buttonCentroCima.Location = new System.Drawing.Point(190, 109);
            this.buttonCentroCima.Name = "buttonCentroCima";
            this.buttonCentroCima.Size = new System.Drawing.Size(76, 65);
            this.buttonCentroCima.TabIndex = 52;
            this.buttonCentroCima.UseVisualStyleBackColor = true;
            // 
            // buttonCantoSuperiorEsquerdo
            // 
            this.buttonCantoSuperiorEsquerdo.Location = new System.Drawing.Point(75, 109);
            this.buttonCantoSuperiorEsquerdo.Name = "buttonCantoSuperiorEsquerdo";
            this.buttonCantoSuperiorEsquerdo.Size = new System.Drawing.Size(76, 65);
            this.buttonCantoSuperiorEsquerdo.TabIndex = 51;
            this.buttonCantoSuperiorEsquerdo.UseVisualStyleBackColor = true;
            // 
            // labelPlayer2Ponto5
            // 
            this.labelPlayer2Ponto5.AutoSize = true;
            this.labelPlayer2Ponto5.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer2Ponto5.Location = new System.Drawing.Point(399, 415);
            this.labelPlayer2Ponto5.Name = "labelPlayer2Ponto5";
            this.labelPlayer2Ponto5.Size = new System.Drawing.Size(48, 42);
            this.labelPlayer2Ponto5.TabIndex = 50;
            this.labelPlayer2Ponto5.Text = "O";
            // 
            // labelPlayer2Ponto4
            // 
            this.labelPlayer2Ponto4.AutoSize = true;
            this.labelPlayer2Ponto4.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer2Ponto4.Location = new System.Drawing.Point(345, 415);
            this.labelPlayer2Ponto4.Name = "labelPlayer2Ponto4";
            this.labelPlayer2Ponto4.Size = new System.Drawing.Size(48, 42);
            this.labelPlayer2Ponto4.TabIndex = 49;
            this.labelPlayer2Ponto4.Text = "O";
            // 
            // labelPlayer2Ponto3
            // 
            this.labelPlayer2Ponto3.AutoSize = true;
            this.labelPlayer2Ponto3.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer2Ponto3.Location = new System.Drawing.Point(291, 415);
            this.labelPlayer2Ponto3.Name = "labelPlayer2Ponto3";
            this.labelPlayer2Ponto3.Size = new System.Drawing.Size(48, 42);
            this.labelPlayer2Ponto3.TabIndex = 48;
            this.labelPlayer2Ponto3.Text = "O";
            // 
            // labelPlayer2Ponto2
            // 
            this.labelPlayer2Ponto2.AutoSize = true;
            this.labelPlayer2Ponto2.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer2Ponto2.Location = new System.Drawing.Point(237, 415);
            this.labelPlayer2Ponto2.Name = "labelPlayer2Ponto2";
            this.labelPlayer2Ponto2.Size = new System.Drawing.Size(48, 42);
            this.labelPlayer2Ponto2.TabIndex = 47;
            this.labelPlayer2Ponto2.Text = "O";
            // 
            // labelPlayer2Ponto1
            // 
            this.labelPlayer2Ponto1.AutoSize = true;
            this.labelPlayer2Ponto1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer2Ponto1.Location = new System.Drawing.Point(183, 415);
            this.labelPlayer2Ponto1.Name = "labelPlayer2Ponto1";
            this.labelPlayer2Ponto1.Size = new System.Drawing.Size(48, 42);
            this.labelPlayer2Ponto1.TabIndex = 46;
            this.labelPlayer2Ponto1.Text = "O";
            // 
            // labelPlayer1Ponto5
            // 
            this.labelPlayer1Ponto5.AutoSize = true;
            this.labelPlayer1Ponto5.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer1Ponto5.Location = new System.Drawing.Point(399, 362);
            this.labelPlayer1Ponto5.Name = "labelPlayer1Ponto5";
            this.labelPlayer1Ponto5.Size = new System.Drawing.Size(48, 42);
            this.labelPlayer1Ponto5.TabIndex = 45;
            this.labelPlayer1Ponto5.Text = "O";
            // 
            // labelPlayer1Ponto4
            // 
            this.labelPlayer1Ponto4.AutoSize = true;
            this.labelPlayer1Ponto4.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer1Ponto4.Location = new System.Drawing.Point(345, 362);
            this.labelPlayer1Ponto4.Name = "labelPlayer1Ponto4";
            this.labelPlayer1Ponto4.Size = new System.Drawing.Size(48, 42);
            this.labelPlayer1Ponto4.TabIndex = 44;
            this.labelPlayer1Ponto4.Text = "O";
            // 
            // labelPlayer1Ponto3
            // 
            this.labelPlayer1Ponto3.AutoSize = true;
            this.labelPlayer1Ponto3.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer1Ponto3.Location = new System.Drawing.Point(291, 362);
            this.labelPlayer1Ponto3.Name = "labelPlayer1Ponto3";
            this.labelPlayer1Ponto3.Size = new System.Drawing.Size(48, 42);
            this.labelPlayer1Ponto3.TabIndex = 43;
            this.labelPlayer1Ponto3.Text = "O";
            // 
            // labelPlayer1Ponto2
            // 
            this.labelPlayer1Ponto2.AutoSize = true;
            this.labelPlayer1Ponto2.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer1Ponto2.Location = new System.Drawing.Point(237, 362);
            this.labelPlayer1Ponto2.Name = "labelPlayer1Ponto2";
            this.labelPlayer1Ponto2.Size = new System.Drawing.Size(48, 42);
            this.labelPlayer1Ponto2.TabIndex = 42;
            this.labelPlayer1Ponto2.Text = "O";
            // 
            // labelPlayer1Ponto1
            // 
            this.labelPlayer1Ponto1.AutoSize = true;
            this.labelPlayer1Ponto1.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer1Ponto1.Location = new System.Drawing.Point(183, 362);
            this.labelPlayer1Ponto1.Name = "labelPlayer1Ponto1";
            this.labelPlayer1Ponto1.Size = new System.Drawing.Size(48, 42);
            this.labelPlayer1Ponto1.TabIndex = 41;
            this.labelPlayer1Ponto1.Text = "O";
            // 
            // labelPlayer2Name
            // 
            this.labelPlayer2Name.AutoSize = true;
            this.labelPlayer2Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer2Name.Location = new System.Drawing.Point(6, 415);
            this.labelPlayer2Name.Name = "labelPlayer2Name";
            this.labelPlayer2Name.Size = new System.Drawing.Size(174, 42);
            this.labelPlayer2Name.TabIndex = 40;
            this.labelPlayer2Name.Text = "Player 2:";
            // 
            // labelPlayer1Name
            // 
            this.labelPlayer1Name.AutoSize = true;
            this.labelPlayer1Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer1Name.Location = new System.Drawing.Point(6, 362);
            this.labelPlayer1Name.Name = "labelPlayer1Name";
            this.labelPlayer1Name.Size = new System.Drawing.Size(174, 42);
            this.labelPlayer1Name.TabIndex = 39;
            this.labelPlayer1Name.Text = "Player 1:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(6, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(446, 340);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 38;
            this.pictureBox1.TabStop = false;
            // 
            // groupBoxJogo
            // 
            this.groupBoxJogo.Controls.Add(this.buttonCantoInferiorDireito);
            this.groupBoxJogo.Controls.Add(this.buttonCentroBaixo);
            this.groupBoxJogo.Controls.Add(this.buttonCantoSuperiorEsquerdo);
            this.groupBoxJogo.Controls.Add(this.buttonCentroCima);
            this.groupBoxJogo.Controls.Add(this.buttonCantoInferiorEsquerdo);
            this.groupBoxJogo.Controls.Add(this.buttonCantoSuperiorDireito);
            this.groupBoxJogo.Controls.Add(this.pictureBox1);
            this.groupBoxJogo.Controls.Add(this.labelPlayer1Name);
            this.groupBoxJogo.Controls.Add(this.labelPlayer2Name);
            this.groupBoxJogo.Controls.Add(this.labelPlayer1Ponto1);
            this.groupBoxJogo.Controls.Add(this.labelPlayer1Ponto2);
            this.groupBoxJogo.Controls.Add(this.labelPlayer1Ponto3);
            this.groupBoxJogo.Controls.Add(this.labelPlayer1Ponto4);
            this.groupBoxJogo.Controls.Add(this.labelPlayer1Ponto5);
            this.groupBoxJogo.Controls.Add(this.labelPlayer2Ponto1);
            this.groupBoxJogo.Controls.Add(this.labelPlayer2Ponto5);
            this.groupBoxJogo.Controls.Add(this.labelPlayer2Ponto2);
            this.groupBoxJogo.Controls.Add(this.labelPlayer2Ponto4);
            this.groupBoxJogo.Controls.Add(this.labelPlayer2Ponto3);
            this.groupBoxJogo.Enabled = false;
            this.groupBoxJogo.Location = new System.Drawing.Point(12, 15);
            this.groupBoxJogo.Name = "groupBoxJogo";
            this.groupBoxJogo.Size = new System.Drawing.Size(459, 471);
            this.groupBoxJogo.TabIndex = 59;
            this.groupBoxJogo.TabStop = false;
            this.groupBoxJogo.Text = "Jogo";
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 491);
            this.Controls.Add(this.groupBoxJogo);
            this.Controls.Add(this.groupBoxChat);
            this.Controls.Add(this.groupBoxAutenticacao);
            this.Name = "Window";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Window_FormClosing);
            this.groupBoxChat.ResumeLayout(false);
            this.groupBoxChat.PerformLayout();
            this.groupBoxAutenticacao.ResumeLayout(false);
            this.groupBoxAutenticacao.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBoxJogo.ResumeLayout(false);
            this.groupBoxJogo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBoxChat;
        private System.Windows.Forms.ListBox listBoxChat;
        private System.Windows.Forms.TextBox TextBoxMensagemEnviar;
        private System.Windows.Forms.Button buttonEnviar;
        private System.Windows.Forms.GroupBox groupBoxAutenticacao;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonEntrar;
        private System.Windows.Forms.TextBox textBoxNome;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Button buttonCantoInferiorDireito;
        private System.Windows.Forms.Button buttonCentroBaixo;
        private System.Windows.Forms.Button buttonCantoInferiorEsquerdo;
        private System.Windows.Forms.Button buttonCantoSuperiorDireito;
        private System.Windows.Forms.Button buttonCentroCima;
        private System.Windows.Forms.Button buttonCantoSuperiorEsquerdo;
        private System.Windows.Forms.Label labelPlayer2Ponto5;
        private System.Windows.Forms.Label labelPlayer2Ponto4;
        private System.Windows.Forms.Label labelPlayer2Ponto3;
        private System.Windows.Forms.Label labelPlayer2Ponto2;
        private System.Windows.Forms.Label labelPlayer2Ponto1;
        private System.Windows.Forms.Label labelPlayer1Ponto5;
        private System.Windows.Forms.Label labelPlayer1Ponto4;
        private System.Windows.Forms.Label labelPlayer1Ponto3;
        private System.Windows.Forms.Label labelPlayer1Ponto2;
        private System.Windows.Forms.Label labelPlayer1Ponto1;
        private System.Windows.Forms.Label labelPlayer2Name;
        private System.Windows.Forms.Label labelPlayer1Name;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBoxJogo;
    }
}

