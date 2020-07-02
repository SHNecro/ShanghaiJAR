using MapEditor.Core;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MapEditor.ViewModels
{
    public class BGMDataViewModel : StringRepresentation
    {
        private ObservableCollection<BGMViewModel> bgm;

        private BGMViewModel selectedBGM;

        public BGMDataViewModel()
        {
            var loopPointContents = System.IO.File.ReadAllText("music/looppoint.txt", Encoding.GetEncoding("Shift_JIS"));
            this.SetFromStringValue(loopPointContents);
        }

        public ObservableCollection<BGMViewModel> BGM
        {
            get { return this.bgm; }
            set { this.SetValue(ref this.bgm, value); }
        }

        public BGMViewModel SelectedBGM
        {
            get { return this.selectedBGM; }
            set { this.SetValue(ref this.selectedBGM, value); }
        }

        protected override string GetStringValue()
        {
            return string.Join("\r\n", this.BGM.Select(re => re.StringValue));
        }

        protected override void SetStringValue(string value)
        {
            this.SetFromStringValue(value);
        }

        private void SetFromStringValue(string value)
        {
            var newBgm = value.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Where(res => res != string.Empty)
                .Select(res => new BGMViewModel { StringValue = res }).ToList();
            this.AddChildErrors(null, newBgm);

            if (!this.HasErrors)
            {
                this.bgm = new ObservableCollection<BGMViewModel>(newBgm);
            }
        }
    }
}
