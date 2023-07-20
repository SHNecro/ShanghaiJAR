using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using MapEditor.Core;
using Moq;
using NSBattle;
using NSShanghaiEXE.InputOutput.Audio;
using NSShanghaiEXE.InputOutput.Rendering;
using NSEnemy;
using Common.Vectors;

namespace MapEditor.Models.Elements
{
    public class EnemyDefinition
	{
        private EnemyDefinition() { }

        public static EnemyDefinition GetEnemyDefinition(int id, int x, int y, int rank, int hp = 1, int chipID1 = 1, int chipID2 = 1, int chipID3 = 1, string name = "")
        {
            var bindFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var enemyBase = new EnemyBase(null, null, x, y, 0, Panel.COLOR.blue, (byte)rank);
            if (id == Constants.NormalNaviID)
            {
                var type = typeof(EnemyBase).Assembly.GetType("NSEnemy.NormalNavi");
                var constructorTypes = new Type[] {
                    typeof(IAudioEngine),
                    typeof(SceneBattle),
                    typeof(int),
                    typeof(int),
                    typeof(byte),
                    typeof(Panel.COLOR),
                    typeof(byte),
                    typeof(int),
                    typeof(int),
                    typeof(int),
                    typeof(int),
                    typeof(string)
                };
                var constructor = type.GetConstructor(bindFlags, null, constructorTypes, null);
                var normalNavi = (EnemyBase)constructor.Invoke(new object[] { null, null, x, y, (byte)0, Panel.COLOR.blue, (byte)rank, hp, chipID1, chipID2, chipID3, name });
                enemyBase = (EnemyBase)normalNavi;
            }
            var enemyMade = EnemyBase.EnemyMake(id, enemyBase, false);
            if (enemyMade == null)
            {
                return null;
            }
            enemyMade.PositionDirectSet();

            var definition = new EnemyDefinition
            {
                DrawCalls = new List<DrawCall>()
            };

            var blankParentObject = new SceneBattle { nowscene = SceneBattle.BATTLESCENE.battle };
            enemyMade.parent = blankParentObject;

            try
            {
                enemyMade.InitAfter();
            }
            catch { }
            var mockRenderDg = new Mock<IRenderer>(MockBehavior.Strict);
            mockRenderDg.Setup(msdg => msdg.DrawImage(It.IsAny<IRenderer>(), It.IsAny<string>(), It.IsAny<Rectangle>(), It.IsAny<bool>(), It.IsAny<Vector2>(), It.IsAny<Color>()))
                .Callback((IRenderer dg, string tex, Rectangle texRect, bool topLeft, Vector2 point, Color color) =>
                {
                    definition.AddDrawCall(dg, tex, texRect, topLeft, point, 1.0f, 0.0f, false, Color.White);
                });
            mockRenderDg.Setup(msdg => msdg.DrawImage(It.IsAny<IRenderer>(), It.IsAny<string>(), It.IsAny<Rectangle>(), It.IsAny<bool>(), It.IsAny<Vector2>(), It.IsAny<bool>(), It.IsAny<Color>()))
                .Callback((IRenderer dg, string tex, Rectangle texRect, bool topLeft, Vector2 point, bool reversed, Color color) =>
                {
                    definition.AddDrawCall(dg, tex, texRect, topLeft, point, 1.0f, 0.0f, reversed, Color.White);
                });
            mockRenderDg.Setup(msdg => msdg.DrawImage(It.IsAny<IRenderer>(), It.IsAny<string>(), It.IsAny<Rectangle>(), It.IsAny<bool>(), It.IsAny<Vector2>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<Color>()))
                .Callback((IRenderer dg, string tex, Rectangle texRect, bool topLeft, Vector2 point, float scale, float rotate, Color color) =>
                {
                    definition.AddDrawCall(dg, tex, texRect, topLeft, point, scale, rotate, false, Color.White);
                });
            mockRenderDg.Setup(msdg => msdg.DrawImage(It.IsAny<IRenderer>(), It.IsAny<string>(), It.IsAny<Rectangle>(), It.IsAny<bool>(), It.IsAny<Vector2>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<bool>(), It.IsAny<Color>()))
                .Callback((IRenderer dg, string tex, Rectangle texRect, bool topLeft, Vector2 point, float scale, float rotate, bool reversed, Color color) =>
                {
                    definition.AddDrawCall(dg, tex, texRect, topLeft, point, scale, rotate, reversed, Color.White);
                });

            try
            {
                enemyMade.Render(mockRenderDg.Object);
                enemyMade.HPRend(mockRenderDg.Object);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().ToString()}: {e.Message}");
            }

            var chipChecks = 100;
            var chipOccurances = new Dictionary<int, Chip>();
            for (int i = 0; i < chipChecks; i++)
            {
                var chipCheckEnemy = EnemyBase.EnemyMake(id, enemyBase, false);
                var chips = chipCheckEnemy.dropchips.Select(c => new Chip { ID = c.chip.number, CodeNumber = c.codeNo }).ToArray();
                for (int ii = 0; ii < chips.Length; ii++)
                {
                    if (chipOccurances.ContainsKey(ii))
                    {
                        if (chipOccurances[ii].ID == chips[ii].ID && chipOccurances[ii].CodeNumber == chips[ii].CodeNumber)
                        {
                            chipOccurances[ii].RandomChance += 1.0 / chipChecks;
                        }
                        else
                        {
                            if (chipOccurances[ii].RandomAlternatives == null)
                            {
                                chipOccurances[ii].RandomAlternatives = new List<Chip>();
                            }

                            chipOccurances[ii].IsRandom = true;
                            var alternateChip = chipOccurances[ii].RandomAlternatives.FirstOrDefault(c => c.ID == chips[ii].ID && c.CodeNumber == chips[ii].CodeNumber);
                            if (alternateChip == null)
                            {
                                alternateChip = chips[ii];
                                chipOccurances[ii].RandomAlternatives.ForEach(c => c.RandomAlternatives.Add(alternateChip));
                                alternateChip.RandomAlternatives = new List<Chip>();
                                alternateChip.RandomAlternatives.Add(chipOccurances[ii]);
                                alternateChip.RandomAlternatives.AddRange(chipOccurances[ii].RandomAlternatives);
                                chipOccurances[ii].RandomAlternatives.Add(alternateChip);
                            }

                            alternateChip.IsRandom = true;
                            alternateChip.RandomChance += 1.0 / chipChecks;
                        }
                    }
                    else
                    {
                        chipOccurances[ii] = chips[ii];
                    }
                }
            }

            definition.Chips = chipOccurances.Values.ToArray();
            definition.HP = enemyMade.Hp;
            if (id == Constants.NormalNaviID)
            {
                if (Constants.TranslationService.CanTranslate(enemyMade.Name))
                {
                    definition.Name = Constants.TranslationService.Translate(enemyMade.Name);
                    definition.NameKey = enemyMade.Name;
                }
                else
                {
                    definition.Name = $"Invalid key: \"{enemyMade.Name}\"";
                    definition.NameKey = null;
                }
            }
            else
            {
                definition.Name = enemyMade.Name;
                var a = Constants.TranslationCallKeys;
                if (Constants.TranslationCallKeys.ContainsKey(definition.Name))
                {
                    definition.NameKey = Constants.TranslationCallKeys[definition.Name];
                }
                else
                {
                    definition.NameKey = null;
                }
            }

            definition.IsNavi = enemyMade.race == EnemyBase.ENEMY.navi && id != 42;

            Constants.TranslationCallKeys.Clear();

            return definition;
		}

        public Chip[] Chips { get; set; }

        public int HP { get; set; }

        public string Name { get; private set; }

        public string NameKey { get; private set; }

        public bool IsNavi { get; private set; }

        public List<DrawCall> DrawCalls { get; private set; }

        private void AddDrawCall(IRenderer dg, string tex, Rectangle texRect, bool topLeft, Vector2 point, float scale, float rotate, bool reversed, Color color)
        {
            this.DrawCalls.Add(new DrawCall
            {
                TextureName = tex,
                TexturePosition = new Point(texRect.X, texRect.Y),
                IsFromTopLeft = topLeft,
                Position = new Point((int)point.X, (int)point.Y),
                Size = new Size(texRect.Width, texRect.Height),
                Scale = scale,
                Rotate = rotate,
                IsReversed = reversed,
                Color = color
            });
        }
    }
}
