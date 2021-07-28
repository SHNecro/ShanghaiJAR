using Common.OpenGL;
using Common.Vectors;
using MapEditor.ViewModels;
using NSBattle;
using System.Drawing;
using System.Linq;

namespace MapEditor.Rendering
{
    public static partial class BattleEventRenderer
    {
        private static void DrawField()
        {
            var startPoint = new Point(0, 160 - (32 + 24 + 24) - 10);
            for (int y = 0; y < 3; y++)
            {
                var renderPass = y * RenderPassPadding;
                for (int x = 0; x < 6; x++)
                {
                    int panelTexY = 0;
                    switch (BattleEventRenderer.CurrentEncounter.Panels[y, x])
                    {
                        case 0:
                            panelTexY = 32 * (int)BattleEventRenderer.CurrentEncounter.PrimaryPanel;
                            break;
                        case 1:
                            panelTexY = 32 * (int)BattleEventRenderer.CurrentEncounter.SecondaryPanel;
                            break;
                    }

                    var topLeft = new Point(startPoint.X + x * 40, startPoint.Y + y * 24);

                    BattleEventRenderer.FieldRenderer.Draw(new Sprite
                    {
                        Position = topLeft,
                        TexX = x < 3 ? 0 : 40,
                        TexY = panelTexY,
                        Width = 40,
                        Height = 32,
                        Texture = $"battleobjects"
                    }.WithTopLeftPosition(), renderPass);

                    if (BattleEventRenderer.CurrentEncounter.Panels[y, x] == 3)
                    {
                        var rockTopLeft = topLeft;
                        rockTopLeft.Offset(0, -18);
                        BattleEventRenderer.FieldRenderer.Draw(new Sprite
                        {
                            Position = rockTopLeft,
                            TexX = 0,
                            TexY = 144,
                            Width = 40,
                            Height = 40,
                            Texture = $"objects1"
                        }.WithTopLeftPosition(), renderPass);
                    }

                    if ((BattleEventRenderer.CurrentEncounter.Panels[y, x] == 0 && BattleEventRenderer.IsHoveringPrimaryPanel) || (BattleEventRenderer.CurrentEncounter.Panels[y, x] == 1 && BattleEventRenderer.IsHoveringSecondaryPanel))
                    {
                        var bottomRight = topLeft;
                        bottomRight.Offset(40, 24);
                        BattleEventRenderer.FieldRenderer.DrawQuad(new Quad
                        {
                            A = new Point(bottomRight.X, topLeft.Y),
                            B = new Point(bottomRight.X, bottomRight.Y),
                            C = new Point(topLeft.X, bottomRight.Y),
                            D = new Point(topLeft.X, topLeft.Y),
                            Color = Color.FromArgb(128, Color.AliceBlue),
                            Type = DrawType.Outline | DrawType.Fill
                        }, renderPass + 1);
                    }
                }
            }

            for (int i = 0; i < BattleEventRenderer.CurrentEncounter.Enemies.Length; i++)
            {
                var enemy = BattleEventRenderer.CurrentEncounter.Enemies[i];

                var topLeft = new Point(startPoint.X + enemy.X * 40, startPoint.Y + enemy.Y * 24);
                var bottomRight = topLeft;
                bottomRight.Offset(40, 24);

                var renderPass = enemy.Y * RenderPassPadding;
                BattleEventRenderer.FieldRenderer.DrawQuad(new Quad
                {
                    A = new Point(bottomRight.X, topLeft.Y),
                    B = new Point(bottomRight.X, bottomRight.Y),
                    C = new Point(topLeft.X, bottomRight.Y),
                    D = new Point(topLeft.X, topLeft.Y),
                    Color = Color.FromArgb(128, Color.MediumVioletRed),
                    Type = DrawType.Outline | DrawType.Fill
                }, renderPass + 1);

                var enemyDefinition = enemy.EnemyDefinition;
                if (enemyDefinition != null)
                {
                    foreach (var drawCall in enemyDefinition.DrawCalls)
                    {
                        var drawnSprite = new Sprite
                        {
                            Position = drawCall.Position,
                            TexX = drawCall.TexturePosition.X,
                            TexY = drawCall.TexturePosition.Y,
                            Width = drawCall.Size.Width,
                            Height = drawCall.Size.Height,
                            Texture = drawCall.TextureName,
                            Scale = new Vector2((float)(drawCall.IsReversed ? -drawCall.Scale : drawCall.Scale), (float)drawCall.Scale),
                            Rotate = (float)drawCall.Rotate
                        };
                        if (drawCall.IsFromTopLeft)
                        {
                            drawnSprite = drawnSprite.WithTopLeftPosition();
                        }
                        BattleEventRenderer.FieldRenderer.Draw(drawnSprite, renderPass + RenderPassPadding);
                    }
                }
            }
            
            if (BattleEventRenderer.FieldMousePosition != null)
            {
                var hoverPanel = new Point((BattleEventRenderer.FieldMousePosition.Value.X - startPoint.X) / 40, (BattleEventRenderer.FieldMousePosition.Value.Y - startPoint.Y) / 24);

                if (hoverPanel.X >= 3 && hoverPanel.X < 6 && hoverPanel.Y >= 0 && hoverPanel.Y < 3)
                {
                    var hoveredEnemy = CurrentEncounter.Enemies.FirstOrDefault(e => e.X == hoverPanel.X && e.Y == hoverPanel.Y);
                    if (hoveredEnemy != null)
                    {
                        var topLeft = new Point(startPoint.X + hoverPanel.X * 40, startPoint.Y + hoverPanel.Y * 24);
                        var bottomRight = topLeft;
                        bottomRight.Offset(40, 24);

                        var renderPass = startPoint.Y;

                        var color = BattleEventRenderer.FieldMouseDownPosition.HasValue ? Color.FromArgb(128, Color.DarkBlue) : Color.FromArgb(128, Color.White);

                        BattleEventRenderer.FieldRenderer.DrawQuad(new Quad
                        {
                            A = new Point(bottomRight.X, topLeft.Y),
                            B = new Point(bottomRight.X, bottomRight.Y),
                            C = new Point(topLeft.X, bottomRight.Y),
                            D = new Point(topLeft.X, topLeft.Y),
                            Color = color,
                            Type = DrawType.Outline | DrawType.Fill
                        }, renderPass + 1);
                    }
                    else if (BattleEventRenderer.FieldMouseDragEnemy != null)
                    {
                        BattleEventRenderer.FieldMouseDragEnemy.X = hoverPanel.X;
                        BattleEventRenderer.FieldMouseDragEnemy.Y = hoverPanel.Y;
                        BattleEventRenderer.FieldMouseDragEnemy.RefreshEnemyDefinition();
                    }
                }
            }

            if (BattleEventRenderer.FieldMouseDownPosition.HasValue && BattleEventRenderer.FieldMouseDragEnemy == null)
            {
                var downPanel = new Point((BattleEventRenderer.FieldMouseDownPosition.Value.X - startPoint.X) / 40, (BattleEventRenderer.FieldMouseDownPosition.Value.Y - startPoint.Y) / 24);
                var downEnemy = CurrentEncounter.Enemies.FirstOrDefault(e => e.X == downPanel.X && e.Y == downPanel.Y);
                BattleEventRenderer.FieldMouseDragEnemy = downEnemy;
            }

            if (BattleEventRenderer.FieldMouseDragReleasePosition.HasValue)
            {
                var dragPanel = new Point((BattleEventRenderer.FieldMouseDragReleasePosition.Value.X - startPoint.X) / 40, (BattleEventRenderer.FieldMouseDragReleasePosition.Value.Y - startPoint.Y) / 24);
                var dragEnemy = CurrentEncounter.Enemies.FirstOrDefault(e => e.X == dragPanel.X && e.Y == dragPanel.Y);
                if (dragEnemy != null)
                {
                    BattleEventRenderer.CurrentEnemy = dragEnemy;
                }
                BattleEventRenderer.FieldMouseDragEnemy = null;
                BattleEventRenderer.FieldMouseDragReleasePosition = null;
            }

            if (BattleEventRenderer.CurrentEnemy != null)
            {
                var topLeft = new Point(startPoint.X + BattleEventRenderer.CurrentEnemy.X * 40, startPoint.Y + BattleEventRenderer.CurrentEnemy.Y * 24);
                var bottomRight = topLeft;
                bottomRight.Offset(40, 24);

                var renderPass = startPoint.Y;

                BattleEventRenderer.FieldRenderer.DrawQuad(new Quad
                {
                    A = new Point(bottomRight.X, topLeft.Y),
                    B = new Point(bottomRight.X, bottomRight.Y),
                    C = new Point(topLeft.X, bottomRight.Y),
                    D = new Point(topLeft.X, topLeft.Y),
                    Color = Color.FromArgb(128, Color.Wheat),
                    Type = DrawType.Outline | DrawType.Fill
                }, renderPass + 1);
            }

            BattleEventRenderer.FieldRenderer.Render();
			BattleEventRenderer.DrawEnemy();
        }

        private static void DrawPatterns()
        {
            for (int pattern = 0; pattern < NSShanghaiEXE.Common.Constants.PanelLayouts.Count; pattern++)
            {
                var startPoint = new Point(10 + (pattern % 4) * 260, 10 + (pattern / 4) * 100);
                var panels = NSShanghaiEXE.Common.Constants.PanelLayouts[pattern];
                for (int y = 0; y < 3; y++)
                {
                    var renderPass = y;
                    for (int x = 0; x < 6; x++)
                    {
                        int panelTexY = 0;
                        switch (panels[y, x])
                        {
                            case 0:
                                panelTexY = 32 * (int)BattleEventRenderer.CurrentEncounter.PrimaryPanel;
                                break;
                            case 1:
                                panelTexY = 32 * (int)BattleEventRenderer.CurrentEncounter.SecondaryPanel;
                                break;
                        }
                        BattleEventRenderer.PatternRenderer.Draw(new Sprite
                        {
                            Position = new Point(startPoint.X + x * 40, startPoint.Y + y * 24),
                            TexX = x < 3 ? 0 : 40,
                            TexY = panelTexY,
                            Width = 40,
                            Height = 32,
                            Texture = $"battleobjects"
                        }.WithTopLeftPosition(), y);
                        if (panels[y, x] == 3)
                        {
                            BattleEventRenderer.PatternRenderer.Draw(new Sprite
                            {
                                Position = new Point(startPoint.X + x * 40, startPoint.Y + y * 24 - 18),
                                TexX = 0,
                                TexY = 144,
                                Width = 40,
                                Height = 40,
                                Texture = $"objects1"
                            }.WithTopLeftPosition(), y);
                        }
                    }
                }
            }

            {
                var selectedPattern = BattleEventRenderer.CurrentEncounter.PanelPatternNumber;
                var topLeft = new Point(260 * (selectedPattern % 4), 100 * (selectedPattern / 4));
                var bottomRight = new Point(topLeft.X + 260, topLeft.Y + 100);
                BattleEventRenderer.PatternRenderer.DrawQuad(new Quad
                {
                    A = new Point(bottomRight.X, topLeft.Y),
                    B = new Point(bottomRight.X, bottomRight.Y),
                    C = new Point(topLeft.X, bottomRight.Y),
                    D = new Point(topLeft.X, topLeft.Y),
                    Color = Color.FromArgb(125, Color.Wheat),
                    Type = DrawType.Outline | DrawType.Fill
                }, 4);
            }

            var selectedPosition = BattleEventRenderer.PatternMouseClickConfirmPosition ?? BattleEventRenderer.PatternMouseClickPosition ?? BattleEventRenderer.PatternMousePosition;
            if (selectedPosition != null)
            {
                var highlightedPattern = (selectedPosition.Value.X / 260) + 4 * (selectedPosition.Value.Y / 100);
                if (highlightedPattern < NSShanghaiEXE.Common.Constants.PanelLayouts.Count)
                {
                    var topLeft = new Point(260 * (highlightedPattern % 4), 100 * (highlightedPattern / 4));
                    var bottomRight = new Point(topLeft.X + 260, topLeft.Y + 100);
                    var color = BattleEventRenderer.PatternMouseClickPosition.HasValue ? Color.FromArgb(128, Color.DarkBlue) : Color.FromArgb(128, Color.AliceBlue);
                    BattleEventRenderer.PatternRenderer.DrawQuad(new Quad
                    {
                        A = new Point(bottomRight.X, topLeft.Y),
                        B = new Point(bottomRight.X, bottomRight.Y),
                        C = new Point(topLeft.X, bottomRight.Y),
                        D = new Point(topLeft.X, topLeft.Y),
                        Color = color,
                        Type = DrawType.Outline | DrawType.Fill
                    }, 4);
                }
                if (BattleEventRenderer.PatternMouseClickConfirmPosition.HasValue && BattleEventRenderer.PatternMouseClickPosition.HasValue)
                {
                    var selectedPattern = (BattleEventRenderer.PatternMouseClickPosition.Value.X / 260) + 4 * (BattleEventRenderer.PatternMouseClickPosition.Value.Y / 100);
                    var confirmPattern = (BattleEventRenderer.PatternMouseClickConfirmPosition.Value.X / 260) + 4 * (BattleEventRenderer.PatternMouseClickConfirmPosition.Value.Y / 100);
                    if (confirmPattern < NSShanghaiEXE.Common.Constants.PanelLayouts.Count && confirmPattern == selectedPattern)
                    {
                        BattleEventRenderer.CurrentEncounter.PanelPatternNumber = highlightedPattern;
                        BattleEventRenderer.PatternWindow.Hide();
                    }
                    BattleEventRenderer.PatternMouseClickPosition = null;
                    BattleEventRenderer.PatternMouseClickConfirmPosition = null;
                }
            }
            BattleEventRenderer.PatternRenderer.Render();
        }

        private static void DrawEnemy()
        {
			var enemy = BattleEventRenderer.CurrentEnemy;
            var enemyDef = enemy.EnemyDefinition;

            if (enemyDef == null)
            {
                BattleEventRenderer.EnemyRenderer.DrawQuad(new Quad
                {
                    A = new Point(68, 5),
                    B = new Point(78, 15),
                    C = new Point(18, 75),
                    D = new Point(8, 65),
                    Color = Color.Red,
                    Type = DrawType.Fill
                }, 1);

                BattleEventRenderer.EnemyRenderer.Render();
                return;
            }

            foreach (var drawCall in enemyDef.DrawCalls)
            {
                var drawnSprite = new Sprite
                {
                    Position = new Point(drawCall.Position.X - (40 * enemy.X) + 24, drawCall.Position.Y - (24 * enemy.Y) - 20),
                    TexX = drawCall.TexturePosition.X,
                    TexY = drawCall.TexturePosition.Y,
                    Width = drawCall.Size.Width,
                    Height = drawCall.Size.Height,
                    Texture = drawCall.TextureName,
                    Scale = new Vector2((float)(drawCall.IsReversed ? -drawCall.Scale : drawCall.Scale), (float)drawCall.Scale),
                    Rotate = (float)drawCall.Rotate
                };
                if (drawCall.IsFromTopLeft)
                {
                    drawnSprite = drawnSprite.WithTopLeftPosition();
                }
                BattleEventRenderer.EnemyRenderer.Draw(drawnSprite, 1);
            }

            BattleEventRenderer.EnemyRenderer.Render();
        }

        private static void DrawEnemies()
        {
            var cols = 10;
            var startPoint = new Point(10, 10);
            var margin = 5;
            var optionsCount = LoadingWindowViewModel.Settings.EnemyCount;

            for (int enemyID = 0; enemyID < optionsCount; enemyID++)
            {
                if (enemyID == 0)
                {
                    var offX = 5;
                    var offY = -3;
                    BattleEventRenderer.EnemySelectionRenderer.DrawQuad(new Quad
                    {
                        A = new Point(70 + offX, 15 + offY),
                        B = new Point(80 + offX, 25 + offY),
                        C = new Point(20 + offX, 85 + offY),
                        D = new Point(10 + offX, 75 + offY),
                        Color = Color.Red,
                        Type = DrawType.Fill
                    }, 1);

                    continue;
                }

                var x = (enemyID % cols);
                var y = (enemyID / cols);

                var renderPass = y * RenderPassPadding;

                var enemyDef = Constants.BaseEnemyDefinitions[enemyID];

                if (enemyDef == null)
                {
                    continue;
                }
                
                foreach (var drawCall in enemyDef.DrawCalls)
                {
                    var drawnSprite = new Sprite
                    {
                        Position = new Point(drawCall.Position.X + x * (80 + margin * 2) + 40 - 10, drawCall.Position.Y + y * (80 + margin * 2) - 20),
                        TexX = drawCall.TexturePosition.X,
                        TexY = drawCall.TexturePosition.Y,
                        Width = drawCall.Size.Width,
                        Height = drawCall.Size.Height,
                        Texture = drawCall.TextureName,
                        Scale = new Vector2((float)(drawCall.IsReversed ? -drawCall.Scale : drawCall.Scale), (float)drawCall.Scale),
                        Rotate = (float)drawCall.Rotate
                    };
                    if (drawCall.IsFromTopLeft)
                    {
                        drawnSprite = drawnSprite.WithTopLeftPosition();
                    }
                    BattleEventRenderer.EnemySelectionRenderer.Draw(drawnSprite, renderPass);
                }
            }

            if (BattleEventRenderer.CurrentEnemy != null)
            {
                var selectedEnemy = BattleEventRenderer.CurrentEnemy.ID;
                var topLeft = new Point(startPoint.X - margin + (selectedEnemy % cols) * (80 + margin * 2), startPoint.Y - margin + (selectedEnemy / cols) * (80 + margin * 2));
                var bottomRight = new Point(topLeft.X + 80 + margin * 2, topLeft.Y + 80 + margin * 2);
                BattleEventRenderer.EnemySelectionRenderer.DrawQuad(new Quad
                {
                    A = new Point(bottomRight.X, topLeft.Y),
                    B = new Point(bottomRight.X, bottomRight.Y),
                    C = new Point(topLeft.X, bottomRight.Y),
                    D = new Point(topLeft.X, topLeft.Y),
                    Color = Color.FromArgb(125, Color.Wheat),
                    Type = DrawType.Outline | DrawType.Fill
                }, 4);
            }

            var selectedPosition = BattleEventRenderer.EnemySelectionMouseClickConfirmPosition ?? BattleEventRenderer.EnemySelectionMouseClickPosition ?? BattleEventRenderer.EnemySelectionMousePosition;
            if (selectedPosition != null)
            {
                var highlightedEnemy = (selectedPosition.Value.X / (80 + 2 * margin)) + cols * (selectedPosition.Value.Y / (80 + 2 * margin));
                if (highlightedEnemy < optionsCount && (highlightedEnemy == 0 || Constants.BaseEnemyDefinitions[highlightedEnemy] != null))
                {
                    var topLeft = new Point(startPoint.X - margin + (highlightedEnemy % cols) * (80 + margin * 2), startPoint.Y - margin + (highlightedEnemy / cols) * (80 + margin * 2));
                    var bottomRight = new Point(topLeft.X + 80 + margin * 2, topLeft.Y + 80 + margin * 2);
                    var color = BattleEventRenderer.EnemySelectionMouseClickPosition.HasValue ? Color.FromArgb(128, Color.DarkBlue) : Color.FromArgb(128, Color.AliceBlue);
                    BattleEventRenderer.EnemySelectionRenderer.DrawQuad(new Quad
                    {
                        A = new Point(bottomRight.X, topLeft.Y),
                        B = new Point(bottomRight.X, bottomRight.Y),
                        C = new Point(topLeft.X, bottomRight.Y),
                        D = new Point(topLeft.X, topLeft.Y),
                        Color = color,
                        Type = DrawType.Outline | DrawType.Fill
                    }, 4);
                }
                if (BattleEventRenderer.EnemySelectionMouseClickConfirmPosition.HasValue && BattleEventRenderer.EnemySelectionMouseClickPosition.HasValue)
                {
                    var selectedEnemy = (BattleEventRenderer.EnemySelectionMouseClickPosition.Value.X / (80 + 2 * margin)) + cols * (BattleEventRenderer.EnemySelectionMouseClickPosition.Value.Y / (80 + 2 * margin));
                    var confirmEnemy = (BattleEventRenderer.EnemySelectionMouseClickConfirmPosition.Value.X / (80 + 2 * margin)) + cols * (BattleEventRenderer.EnemySelectionMouseClickConfirmPosition.Value.Y / (80 + 2 * margin));
                    if (confirmEnemy < optionsCount && confirmEnemy == selectedEnemy && (highlightedEnemy == 0 || Constants.BaseEnemyDefinitions[highlightedEnemy] != null))
                    {
                        BattleEventRenderer.CurrentEnemy.ID = highlightedEnemy;
                        BattleEventRenderer.DrawField();
                        BattleEventRenderer.DrawEnemy();
                    }
                    BattleEventRenderer.EnemySelectionMouseClickPosition = null;
                    BattleEventRenderer.EnemySelectionMouseClickConfirmPosition = null;
                }
            }
            BattleEventRenderer.EnemySelectionRenderer.Render();
        }

        private static void DrawPrimaryPanel()
        {
            var topLeft = new Point(2, 2);
            var bottomRight = new Point(42, 34);
            BattleEventRenderer.PrimaryPanelRenderer.Draw(new Sprite
            {
                Position = topLeft,
                TexX = 0,
                TexY = 32 * (int)BattleEventRenderer.CurrentEncounter.PrimaryPanel,
                Width = 40,
                Height = 32,
                Texture = $"battleobjects"
            }.WithTopLeftPosition(), 0);

            if (BattleEventRenderer.IsHoveringPrimaryPanel)
            {
                BattleEventRenderer.PrimaryPanelRenderer.DrawQuad(new Quad
                {
                    A = new Point(bottomRight.X, topLeft.Y),
                    B = new Point(bottomRight.X, bottomRight.Y),
                    C = new Point(topLeft.X, bottomRight.Y),
                    D = new Point(topLeft.X, topLeft.Y),
                    Color = Color.FromArgb(128, Color.AliceBlue),
                    Type = DrawType.Outline | DrawType.Fill
                }, 1);
            }

            if (BattleEventRenderer.IsPrimaryPanelMouseDown)
            {
                BattleEventRenderer.PrimaryPanelRenderer.DrawQuad(new Quad
                {
                    A = new Point(bottomRight.X, topLeft.Y),
                    B = new Point(bottomRight.X, bottomRight.Y),
                    C = new Point(topLeft.X, bottomRight.Y),
                    D = new Point(topLeft.X, topLeft.Y),
                    Color = Color.FromArgb(128, Color.DarkBlue),
                    Type = DrawType.Outline | DrawType.Fill
                }, 1);
            }

            BattleEventRenderer.PrimaryPanelRenderer.Render();
        }

        private static void DrawSecondaryPanel()
        {
            var topLeft = new Point(2, 2);
            var bottomRight = new Point(42, 34);
            BattleEventRenderer.SecondaryPanelRenderer.Draw(new Sprite
            {
                Position = topLeft,
                TexX = 0,
                TexY = 32 * (int)BattleEventRenderer.CurrentEncounter.SecondaryPanel,
                Width = 40,
                Height = 32,
                Texture = $"battleobjects"
            }.WithTopLeftPosition(), 0);

            if (BattleEventRenderer.IsHoveringSecondaryPanel)
            {
                BattleEventRenderer.SecondaryPanelRenderer.DrawQuad(new Quad
                {
                    A = new Point(bottomRight.X, topLeft.Y),
                    B = new Point(bottomRight.X, bottomRight.Y),
                    C = new Point(topLeft.X, bottomRight.Y),
                    D = new Point(topLeft.X, topLeft.Y),
                    Color = Color.FromArgb(128, Color.AliceBlue),
                    Type = DrawType.Outline | DrawType.Fill
                }, 1);
            }

            if (BattleEventRenderer.IsSecondaryPanelMouseDown)
            {
                BattleEventRenderer.SecondaryPanelRenderer.DrawQuad(new Quad
                {
                    A = new Point(bottomRight.X, topLeft.Y),
                    B = new Point(bottomRight.X, bottomRight.Y),
                    C = new Point(topLeft.X, bottomRight.Y),
                    D = new Point(topLeft.X, topLeft.Y),
                    Color = Color.FromArgb(128, Color.DarkBlue),
                    Type = DrawType.Outline | DrawType.Fill
                }, 1);
            }

            BattleEventRenderer.SecondaryPanelRenderer.Render();
        }

        private static void DrawPanels()
        {
            var cols = 4;
            var startPoint = new Point(10, 10);
            var margin = 5;
            var optionsCount = 10;

            for (int panel = 0; panel < optionsCount; panel++)
            {
                var topLeft = new Point(startPoint.X + (panel % cols) * (40 + margin * 2), startPoint.Y + (panel / cols) * (32 + margin * 2));

                BattleEventRenderer.PanelSelectionRenderer.Draw(new Sprite
                {
                    Position = new Point(topLeft.X, topLeft.Y),
                    TexX = 0,
                    TexY = 32 * panel,
                    Width = 40,
                    Height = 32,
                    Texture = $"battleobjects"
                }.WithTopLeftPosition(), 0);
            }

            {
                var selectedPanel = (int)(BattleEventRenderer.IsSettingPrimaryPanel ? BattleEventRenderer.CurrentEncounter.PrimaryPanel : BattleEventRenderer.CurrentEncounter.SecondaryPanel);
                var topLeft = new Point(startPoint.X - margin + (selectedPanel % cols) * (40 + margin * 2), startPoint.Y - margin + (selectedPanel / cols) * (32 + margin * 2));
                var bottomRight = new Point(topLeft.X + 40 + margin * 2, topLeft.Y + 32 + margin * 2);
                BattleEventRenderer.PanelSelectionRenderer.DrawQuad(new Quad
                {
                    A = new Point(bottomRight.X, topLeft.Y),
                    B = new Point(bottomRight.X, bottomRight.Y),
                    C = new Point(topLeft.X, bottomRight.Y),
                    D = new Point(topLeft.X, topLeft.Y),
                    Color = Color.FromArgb(125, Color.Wheat),
                    Type = DrawType.Outline | DrawType.Fill
                }, 4);
            }

            var selectedPosition = BattleEventRenderer.PanelSelectionMouseClickConfirmPosition ?? BattleEventRenderer.PanelSelectionMouseClickPosition ?? BattleEventRenderer.PanelSelectionMousePosition;
            if (selectedPosition != null)
            {
                var highlightedPanel = (selectedPosition.Value.X / (40 + 2 * margin)) + cols * (selectedPosition.Value.Y / (32 + 2 * margin));
                if (highlightedPanel < optionsCount)
                {
                    var topLeft = new Point(startPoint.X - margin + (highlightedPanel % cols) * (40 + margin * 2), startPoint.Y - margin + (highlightedPanel / cols) * (32 + margin * 2));
                    var bottomRight = new Point(topLeft.X + 40 + margin * 2, topLeft.Y + 32 + margin * 2);
                    var color = BattleEventRenderer.PanelSelectionMouseClickPosition.HasValue ? Color.FromArgb(128, Color.DarkBlue) : Color.FromArgb(128, Color.AliceBlue);
                    BattleEventRenderer.PanelSelectionRenderer.DrawQuad(new Quad
                    {
                        A = new Point(bottomRight.X, topLeft.Y),
                        B = new Point(bottomRight.X, bottomRight.Y),
                        C = new Point(topLeft.X, bottomRight.Y),
                        D = new Point(topLeft.X, topLeft.Y),
                        Color = color,
                        Type = DrawType.Outline | DrawType.Fill
                    }, 4);
                }
                if (BattleEventRenderer.PanelSelectionMouseClickConfirmPosition.HasValue && BattleEventRenderer.PanelSelectionMouseClickPosition.HasValue)
                {
                    var selectedPanel = (BattleEventRenderer.PanelSelectionMouseClickPosition.Value.X / (40 + 2 * margin)) + cols * (BattleEventRenderer.PanelSelectionMouseClickPosition.Value.Y / (32 + 2 * margin));
                    var confirmPanel = (BattleEventRenderer.PanelSelectionMouseClickConfirmPosition.Value.X / (40 + 2 * margin)) + cols * (BattleEventRenderer.PanelSelectionMouseClickConfirmPosition.Value.Y / (32 + 2 * margin));
                    if (confirmPanel < optionsCount && confirmPanel == selectedPanel)
                    {
                        if (BattleEventRenderer.IsSettingPrimaryPanel)
                        {
                            BattleEventRenderer.CurrentEncounter.PrimaryPanel = (Panel.PANEL)highlightedPanel;
                        }
                        else
                        {
                            BattleEventRenderer.CurrentEncounter.SecondaryPanel = (Panel.PANEL)highlightedPanel;
                        }
                        BattleEventRenderer.DrawField();
                        BattleEventRenderer.DrawPrimaryPanel();
                        BattleEventRenderer.DrawSecondaryPanel();
                    }
                    BattleEventRenderer.PanelSelectionMouseClickPosition = null;
                    BattleEventRenderer.PanelSelectionMouseClickConfirmPosition = null;
                }
            }
            BattleEventRenderer.PanelSelectionRenderer.Render();
        }
    }
}
