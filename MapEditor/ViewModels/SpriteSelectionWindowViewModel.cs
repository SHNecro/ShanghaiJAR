using MapEditor.Core;
using MapEditor.Models;
using MapEditor.Rendering;
using System;
using System.Linq;

namespace MapEditor.ViewModels
{
    public class SpriteSelectionWindowViewModel : ViewModelBase
    {
        public MapEventPage CurrentPage
        {
            get
            {
                return SpriteSelectionRenderer.CurrentPage;
            }
            set
            {
                SpriteSelectionRenderer.CurrentPage = value;
                this.OnPropertyChanged(nameof(this.TexX));
                this.OnPropertyChanged(nameof(this.TexY));
                this.OnPropertyChanged(nameof(this.TexW));
                this.OnPropertyChanged(nameof(this.TexH));
                this.OnPropertyChanged(nameof(this.Frames));
                this.OnPropertyChanged(nameof(this.Speed));
                this.OnPropertyChanged(nameof(this.RendType));
                this.OnPropertyChanged(nameof(this.IsCharacter));
                this.OnPropertyChanged(nameof(this.CharacterIndex));
                this.OnPropertyChanged(nameof(this.Angle));
                this.OnPropertyChanged(nameof(this.GraphicsIndex));
                this.OnPropertyChanged(nameof(this.CurrentPage));
            }
        }

        public int TexX
        {
            get
            {
                return this.CurrentPage.TexX;
            }
            set
            {
                this.CurrentPage.TexX = value;
                this.OnPropertyChanged(nameof(this.TexX));
                this.PageSetterAction(this.CurrentPage);
            }
        }

        public int TexY
        {
            get
            {
                return this.CurrentPage.TexY;
            }
            set
            {
                this.CurrentPage.TexY = value;
                this.OnPropertyChanged(nameof(this.TexY));
                this.PageSetterAction(this.CurrentPage);
            }
        }

        public int TexW
        {
            get
            {
                return this.CurrentPage.TexW;
            }
            set
            {
                this.CurrentPage.TexW = value;
                this.OnPropertyChanged(nameof(this.TexW));
                this.PageSetterAction(this.CurrentPage);
            }
        }

        public int TexH
        {
            get
            {
                return this.CurrentPage.TexH;
            }
            set
            {
                this.CurrentPage.TexH = value;
                this.OnPropertyChanged(nameof(this.TexH));
                this.PageSetterAction(this.CurrentPage);
            }
        }

        public int Frames
        {
            get
            {
                return this.CurrentPage.Frames;
            }
            set
            {
                this.CurrentPage.Frames = value;
                this.OnPropertyChanged(nameof(this.Frames));
                this.PageSetterAction(this.CurrentPage);
            }
        }

        public int Speed
        {
            get
            {
                return this.CurrentPage.Speed;
            }
            set
            {
                this.CurrentPage.Speed = value;
                this.OnPropertyChanged(nameof(this.Speed));
                this.PageSetterAction(this.CurrentPage);
            }
        }

        public int RendType
        {
            get
            {
                return this.CurrentPage.RendType;
            }
            set
            {
                this.CurrentPage.RendType = value;
                this.OnPropertyChanged(nameof(this.RendType));
                this.PageSetterAction(this.CurrentPage);
            }
        }

        public bool IsCharacter
        {
            get
            {
                return this.CurrentPage.IsCharacter;
            }
            set
            {
                var newIndices = value ? SpriteSelectionRenderer.CharacterSheetIndices : SpriteSelectionRenderer.ObjectSheetIndices;
                if (!newIndices.Contains(this.GraphicsIndex))
                {
                    this.GraphicsIndex = newIndices.OrderBy(i => Math.Abs(i - this.GraphicsIndex)).First();
                }

                this.CurrentPage.IsCharacter = value;
                this.OnPropertyChanged(nameof(this.IsCharacter));
                this.PageSetterAction(this.CurrentPage);
            }
        }

        public int CharacterIndex
        {
            get
            {
                return this.CurrentPage.CharacterIndex;
            }
            set
            {
                this.CurrentPage.CharacterIndex = value;
                this.OnPropertyChanged(nameof(this.CharacterIndex));
                this.PageSetterAction(this.CurrentPage);
            }
        }

        public int Angle
        {
            get
            {
                return this.CurrentPage.Angle;
            }
            set
            {
                this.CurrentPage.Angle = value;
                this.OnPropertyChanged(nameof(this.Angle));
                this.PageSetterAction(this.CurrentPage);
            }
        }

        public int GraphicsIndex
        {
            get
            {
                return this.CurrentPage.GraphicsIndex;
            }
            set
            {
                this.CurrentPage.GraphicsIndex = value;
                this.OnPropertyChanged(nameof(this.GraphicsIndex));
                this.PageSetterAction(this.CurrentPage);
            }
        }

        public Action<MapEventPage> PageSetterAction { get; set; }
    }
}
