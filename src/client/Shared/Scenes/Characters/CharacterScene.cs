using Mir.Client.MyraCustom;
using Mir.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Myra.Graphics2D.UI;

namespace Mir.Client.Scenes.Characters
{
    public class CharacterScene : BaseScene
    {
        private CharacterSelectControl[] _characters;

        public CharacterScene()
        {
            Background = new MirImageBrush
            {
                Index = 50,
                Library = LibraryType.Interface1c
            };

            Widgets.Add(new MirImage
            {
                Id = "Header",
                Library = LibraryType.Interface1c,
                Index = 51
            });

            Widgets.Add(new MirImage
            {
                Id = "Footer",
                Library = LibraryType.Interface1c,
                Index = 52,
                Top = 3,
                VerticalAlignment = VerticalAlignment.Bottom
            });

            Widgets.Add(new MirImage
            {
                Id = "Lights",
                Library = LibraryType.Interface1c,
                UseOffset = true,
                Blend = true
            }.WithAnimation(WidgetAnimation.Create()
                .WithCallback((s, e) => ((MirImage)s).Index = e)
                .From(2800).To(2816)
                .Elapse(TimeSpan.FromSeconds(2))
                .WithLoop()
            ));

            Widgets.Add(new MirImage
            {
                Id = "Torches",
                Library = LibraryType.Interface1c,
                UseOffset = true,
                Blend = true,
                Left = 20,
                Top = 25
            }.WithAnimation(WidgetAnimation.Create()
                .WithCallback((s, e) => ((MirImage)s).Index = e)
                .From(2900).Count(16)
                .Elapse(TimeSpan.FromSeconds(2))
                .WithLoop()
            ));


            var player = new MirImage()
            {
                Top = 100,
                Left = 100,
                Library = LibraryType.Interface1c,
                Index = 200,
                UseOffset = true,
            };

            Widgets.Add(player);

            var playerEffect = new MirImage()
            {
                Top = 100,
                Left = 100,
                Library = LibraryType.Interface1c,
                Index = 200,
                UseOffset = true,
                Blend = true
            };

            Widgets.Add(playerEffect);

            player.WithAnimation(WidgetAnimation.Create()
                .WithCallback((s, e) =>
                {
                    player.Index = e;
                    playerEffect.Index = e + 100;
                })
                .From(240).Count(21)
                .Elapse(TimeSpan.FromSeconds(2))
                .OnEnd((c) =>
                {
                    playerEffect.Enabled = false;

                    player.WithAnimation(WidgetAnimation.Create()
                        .WithCallback((s, e) => ((MirImage)s).Index = e)
                        .From(300).Count(12)
                        .Elapse(TimeSpan.FromSeconds(2))
                        .WithLoop());

                })
            );

            //_characters = new CharacterSelectControl[4];

            //for (short i = 0; i < _characters.Length; i++)
            //{
            //    Widgets.Add(_characters[i] = new CharacterSelectControl
            //    {
            //        Id = "Character_" + i,
            //        Selected = i == 0,
            //        Left = (short)(100 + (i * 210)),
            //        Top = 0,
            //        Visible = false,
            //    });
            //    _characters[i].TouchUp += Character_Click;
            //}

            //var createCharacterButton = new MirButton()
            //{
            //    Text = "Create character",
            //    Left = 300,
            //    Top = -15,
            //    Width = 100,
            //    HorizontalAlignment = HorizontalAlignment.Center,
            //    VerticalAlignment = VerticalAlignment.Bottom
            //};
            //createCharacterButton.Click += CreateCharacterButton_Click;
            //Widgets.Add(createCharacterButton);


            //_characters[0].Visible = true;
            //_characters[0].Set(new Character
            //{
            //    Class = new MirClass { Id = 0, Name = "Warrior" },
            //    Gender = new MirGender { Id = 0, Name = "Male" },
            //    Level = 50,
            //    Name = "Test",
            //    Id = 1
            //});
        }

        private void Character_Click(object sender, EventArgs e)
        {

        }

        private void CreateCharacterButton_Click(object sender, EventArgs e)
        {

        }
    }
}
