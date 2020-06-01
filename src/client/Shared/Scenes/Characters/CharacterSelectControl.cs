using Mir.Client.MyraCustom;
using Mir.Models;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mir.Client.Scenes.Characters
{
    public class PlayerBoxInfo : SingleItemContainer<VerticalStackPanel>
    {
        private Label _nameLabel;
        private Label _levelLabel;
        private Label _classLabel;

        public PlayerBoxInfo()
        {
            InternalChild = new VerticalStackPanel();

            InternalChild.Widgets.Add(_nameLabel = new Label { Text = "N/a", HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 10, 0, 0) });
            InternalChild.Widgets.Add(_levelLabel = new Label { Text = "Level: N/a", HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 10, 0, 0) });
            InternalChild.Widgets.Add(_classLabel = new Label { Text = "N/a", HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 10, 0, 0) });
        }

        public void Set(Character character)
        {
            _nameLabel.Text = character.Name;
            _levelLabel.Text = character.Level.ToString();
            _classLabel.Text = character.Class.Name;
        }
    }

    public class CharacterPlayer : SingleItemContainer<Panel>
    {
        private MirImage _body;
        private MirImage _shadow;
        private int _startIndex;
        private int _shadowStartIndex;

        public CharacterPlayer()
        {
            InternalChild = new Panel();
            InternalChild.Widgets.Add(_body = new MirImage
            {
                Library = LibraryType.Interface1c,
                UseOffset = true
            });

            InternalChild.Widgets.Add(_shadow = new MirImage
            {
                Library = LibraryType.Interface1c,
                UseOffset = true,
                Opacity = 0.5f
            });
        }

        public void Set(Character character)
        {
            int frameCount = 0;
            TimeSpan frameTime = TimeSpan.FromMilliseconds(1500);

            switch (character.Class.Id)
            {
                case 0:
                    frameCount = 13;
                    break;
                case 1:
                    frameCount = character.Gender.Id == 0 ? 10 : 15;
                    break;
                case 2:
                    frameCount = character.Gender.Id == 0 ? 15 : 10;
                    break;
                case 3:
                    frameCount = character.Gender.Id == 0 ? 16 : 10;
                    break;
            }

            _startIndex = 200 + (character.Class.Id * 500) + (character.Gender.Id * 200);
            _shadowStartIndex = 220 + (character.Class.Id * 500) + (character.Gender.Id * 200);

            _body.Index = _startIndex;

            _body.ClearAnimations()
                .WithAnimation((c, i) => _body.Index = i, _startIndex, _startIndex + frameCount - 1, frameTime, true);

            _shadow.ClearAnimations()
               .WithAnimation((c, i) => _shadow.Index = i, _shadowStartIndex, _shadowStartIndex + frameCount - 1, frameTime, true);
        }
    }

    public class CharacterSelectControl : SingleItemContainer<VerticalStackPanel>
    {
        private PlayerBoxInfo _box;
        private CharacterPlayer _player;

        public bool Selected { get; set; }

        public CharacterSelectControl()
        {
            InternalChild = new VerticalStackPanel();
            InternalChild.Widgets.Add(_box = new PlayerBoxInfo());
            InternalChild.Widgets.Add(_player = new CharacterPlayer());
        }

        public void Set(Character character)
        {
            _box.Set(character);
            _player.Set(character);
        }
    }
}
