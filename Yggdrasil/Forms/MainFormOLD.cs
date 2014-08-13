using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Yggdrasil.Helpers;

namespace Yggdrasil.Forms
{
    /* Started August 5th 2014,  4:05pm, to "Battlefield - Storm" by Yuzo Koshiro, from Etrian Odyssey IV: Super Arrange Version */
    /* Stopped August 5th 2014,  9:59pm, note: CLEAN UP THIS MESS! */
    /* Started August 6th 2014,  2:40am, quick fix to w/ message reading/dumping ... (I should put this on Github soon) */
    /* Stopped August 6th 2014,  3:45am, some other message-related things... did take a break inbetween tho, playing Digimon Adventure PSP :P */
    /* August 6th 2014, uh, afternoon... TBB TBL1 reading, EquipItemData parser thingy, dumps equipable stuffs to HTML file! */
    /* Started August 7th 2014,  9:08pm, more font rendering work, tho in hindsight... that's kinda stupid anyway. I should try to find the VWF width table, maybe? */
    /* Stopped August 7th 2014, 11:20pm, MiscItemData parser & dumping out all non-equipment items to HTML~ */
    /* August 8th 2014, after midnight, some EtrianString stuff (cast to string etc)... */
    /* Stopped August 8th 2014,  2:40am, housekeeping and generalization via interfaces, ugly HTML dumping code moved out of GameData, etc..... */
    /* August 8th 2014, mid-evening, table parsers now derived from BaseParser, also does fancy property changed notification event stuff~ */
    /* August 8th 2014, uhm, later that evening, finished table parsers rewrite, RIP automatic getters/setters :( */
    /* Stopped August 9th 2014,  3:47am, lots of rewriting GameData, got some useful reflection going, table parsers now semi-automatic w/ ParserUsage attrib on parser classes */
    /* (August 10th 2014, evening, some research on laptop while not at home...) */
    /* Started August 11th 2014, 6:15pm, implementing some of the research from yesterday~ */
    /* Stopped August 11th 2014, 6:46pm, ItemCompoundData & dumper complete! aka what items to sell how many of to get access to new stuff */
    /* Stopped August 11th 2014, 10:26pm, refactoring, lots and lots of it... or maybe just some; also general nicetynessifying blah blub shit */
    /* Stopped August 12th 2014, 5:24am, so much more refactoring, data access simplifying, first steps towards a UI, etc etc etc... TIRED NOW */
    /* Started August 12th 2014, ~3:30pm, PropertyGrid it is, I think, so I'm adding attributes etc. to the parsers... */
    /* Stopped August 12th 2014, 7:56pm, a LOT of messing with the PropertyGrid later, it's working well for EquipItemData; also documented usability for chara classes */

    public partial class MainFormOLD : Form
    {
        GameDataManager game;

        Bitmap tmp;

        public MainFormOLD()
        {
            InitializeComponent();

            dataGridView1.DoubleBuffered(true);

            game = new GameDataManager();
            game.ReadGameDirectory(@"E:\Translations\NDS Etrian Odyssey Hacking\unpack\");

            tmp = game.FontRenderer.RenderString((game.GetMessageFile("GovernmentMissionMess").Tables[0] as FileTypes.TBB.MTBL).Messages[0], 192, 1);
            //tmp.Save(@"E:\Translations\NDS Etrian Odyssey Hacking\_font\TEST.png");

            //Dumpers.MessageDumper.DumpToDirectory(game, @"E:\Translations\NDS Etrian Odyssey Hacking\messages\", true);
            Dumpers.ItemDataDumper.DumpToDirectory(game, @"E:\Translations\NDS Etrian Odyssey Hacking\itemdata\eo1items.htm");
            Dumpers.ItemCompoundDumper.DumpToDirectory(game, @"E:\Translations\NDS Etrian Odyssey Hacking\itemdata\eo1compounds.htm");

            /*System.IO.Directory.CreateDirectory(@"E:\Translations\NDS Etrian Odyssey Hacking\_font\");
            game.FontRenderer.FontImage.Save(@"E:\Translations\NDS Etrian Odyssey Hacking\_font\font.png");
            foreach (FontRenderer.Character character in game.FontRenderer.Characters)
            {
                character.Image.Save(string.Format(@"E:\Translations\NDS Etrian Odyssey Hacking\_font\{0:X4}.png", character.ID));
            }
            */
            dataGridView1.DataSource = new BindingList<Yggdrasil.TableParsers.EquipItemData>(game.GetParsedData<Yggdrasil.TableParsers.EquipItemData>());
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (column.ReadOnly) column.DefaultCellStyle.ForeColor = SystemColors.GrayText;
            }

            comboBox1.DisplayMember = "Name";
            comboBox1.DataSource = game.GetParsedData<Yggdrasil.TableParsers.EquipItemData>();

            propertyGrid1.SelectedObject = ((dynamic)comboBox1.DataSource)[0];

            Application.Idle += ((s, e) => { pictureBox1.Invalidate(); });
            pictureBox1.Paint += new PaintEventHandler((s, e) =>
            {
                e.Graphics.Clear(Color.Black);
                e.Graphics.DrawImageUnscaled(tmp, 0, 0);
            });
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = (sender as ComboBox).SelectedItem;
        }
    }
}
