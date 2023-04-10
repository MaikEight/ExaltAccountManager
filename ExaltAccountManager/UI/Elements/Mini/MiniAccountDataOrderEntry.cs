using System;
using System.Windows.Forms;

namespace ExaltAccountManager.UI.Elements.Mini
{
    public sealed partial class MiniAccountDataOrderEntry : UserControl
    {
        public string EntryName { get; internal set; } = string.Empty;
        public string ValueExample { get; internal set; } = string.Empty;

        public FrmMain frm;

        public bool isSelected = false;

        public event EventHandler SelectedChanged;

        public MiniAccountDataOrderEntry(FrmMain _frm, string entryName, string valueExample)
        {
            InitializeComponent();
            frm = _frm;
            UpdateUI(entryName, valueExample);
        }
        public MiniAccountDataOrderEntry()
        {
            InitializeComponent();
            UpdateUI("SampleEntryName", "SampleValueExample");
        }

        public void UpdateUI(string entryName, string valueExample)
        {
            EntryName = entryName;
            ValueExample = valueExample;

            LoadUI();
        }
        public void UpdateUI() 
        {
            LoadUI();
            SelectionOverride(isSelected);
        }

        private void LoadUI()
        {
            lEntryName.Text = EntryName;
            lValueExample.Text = ValueExample;

            if (lEntryName.Width > lValueExample.Width)
            {
                lEntryName.Left = 5;
                this.Width = lEntryName.Width + 10;
                lValueExample.Left = (this.Width - lValueExample.Width) / 2;
            }
            else
            {
                lValueExample.Left = 5;
                this.Width = lValueExample.Width + 10;
                lEntryName.Left = (this.Width - lEntryName.Width) / 2;
            }
        }

        public void SelectionOverride(bool _isSelected)
        {
            isSelected = _isSelected;
            this.BackColor = isSelected ? ColorScheme.GetColorThird(frm.UseDarkmode) : ColorScheme.GetColorDef(frm.UseDarkmode);
        }

        private void MiniAccountDataOrderEntry_Click(object sender, EventArgs e)
        {
            SelectionOverride(!isSelected);

            if(SelectedChanged != null)
                SelectedChanged(this, new EventArgs());
        }
    }
}
