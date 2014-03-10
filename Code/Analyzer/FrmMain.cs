using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StructureMap;
using Subasta.Client.Common;
using Subasta.Domain.Deck;
using Subasta.Infrastructure.Domain;

namespace Analyzer
{
	public partial class FrmMain : Form
	{
		public IGameSimulator CurrentSimulation { get; private set; }

		public FrmMain()
		{
			InitializeComponent();
		}

		private void NewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openGameFile.ShowDialog() == DialogResult.OK)
			{
				try
				{
					Stream stream=null;
					if ((stream = openGameFile.OpenFile()) != null)
					{
    					CurrentSimulation= ObjectFactory.GetInstance<IGameSimulator>();

						using (stream)
						using(var sr=new StreamReader(stream))
						{
							string line;
							var index = 0;
							var cards = new ICard[4][];

							while( (line = sr.ReadLine())!=null)
							{
								string[] strings = line.Split(' ');
								cards[index++]=strings.Select(x => new Card(x)).ToArray();
							}
							CurrentSimulation.Player1.Cards = cards[0];
							CurrentSimulation.Player2.Cards = cards[1];
							CurrentSimulation.Player3.Cards = cards[2];
							CurrentSimulation.Player4.Cards = cards[3];
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
			}
		}

		private void startToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

	}
}
