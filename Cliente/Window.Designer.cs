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
            this.listBoxMensagens = new System.Windows.Forms.ListBox();
            this.textBoxMensagem = new System.Windows.Forms.TextBox();
            this.buttonEnviar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonLigar = new System.Windows.Forms.Button();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxMensagens
            // 
            this.listBoxMensagens.FormattingEnabled = true;
            this.listBoxMensagens.Location = new System.Drawing.Point(12, 12);
            this.listBoxMensagens.Name = "listBoxMensagens";
            this.listBoxMensagens.Size = new System.Drawing.Size(554, 303);
            this.listBoxMensagens.TabIndex = 0;
            // 
            // textBoxMensagem
            // 
            this.textBoxMensagem.Location = new System.Drawing.Point(12, 321);
            this.textBoxMensagem.Name = "textBoxMensagem";
            this.textBoxMensagem.Size = new System.Drawing.Size(554, 20);
            this.textBoxMensagem.TabIndex = 1;
            // 
            // buttonEnviar
            // 
            this.buttonEnviar.Location = new System.Drawing.Point(572, 319);
            this.buttonEnviar.Name = "buttonEnviar";
            this.buttonEnviar.Size = new System.Drawing.Size(90, 23);
            this.buttonEnviar.TabIndex = 2;
            this.buttonEnviar.Text = "Enviar";
            this.buttonEnviar.UseVisualStyleBackColor = true;
            this.buttonEnviar.Click += new System.EventHandler(this.buttonEnviar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonLigar);
            this.groupBox1.Controls.Add(this.textBoxIP);
            this.groupBox1.Location = new System.Drawing.Point(572, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(90, 78);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Servidor";
            // 
            // buttonLigar
            // 
            this.buttonLigar.Location = new System.Drawing.Point(7, 49);
            this.buttonLigar.Name = "buttonLigar";
            this.buttonLigar.Size = new System.Drawing.Size(75, 23);
            this.buttonLigar.TabIndex = 1;
            this.buttonLigar.Text = "Ligar";
            this.buttonLigar.UseVisualStyleBackColor = true;
            this.buttonLigar.Click += new System.EventHandler(this.buttonLigar_Click);
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(7, 20);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(75, 20);
            this.textBoxIP.TabIndex = 0;
            this.textBoxIP.Text = "localhost";
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 348);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonEnviar);
            this.Controls.Add(this.textBoxMensagem);
            this.Controls.Add(this.listBoxMensagens);
            this.Name = "Window";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Window_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxMensagens;
        private System.Windows.Forms.TextBox textBoxMensagem;
        private System.Windows.Forms.Button buttonEnviar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonLigar;
        private System.Windows.Forms.TextBox textBoxIP;
    }
}

