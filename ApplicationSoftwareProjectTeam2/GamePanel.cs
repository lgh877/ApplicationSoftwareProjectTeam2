using ApplicationSoftwareProjectTeam2.entities;
using ApplicationSoftwareProjectTeam2.entities.creatures;
using ApplicationSoftwareProjectTeam2.entities.items;
using ApplicationSoftwareProjectTeam2.items;
using ApplicationSoftwareProjectTeam2.resources.sounds;
using System;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Media;
using System.Numerics;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Xml.Linq;
using WMPLib;
using ApplicationSoftwareProjectTeam2.network;


namespace ApplicationSoftwareProjectTeam2
{
    public enum Direction
    {
        UpperRight = 0,
        UpRight,
        Right,
        DownRight,
        LowerRight,
        LowerLeft,
        DownLeft,
        Left,
        UpLeft,
        UpperLeft
    }
    public partial class GamePanel : Form
    {
        public bool isMultiplayer = true;// GameMenu에서 전달받음

        //캐릭터 객체들을 관리하는 리스트로, 업데이트가 비교적 드물어 그냥 리스트로 선언함
        public List<LivingEntity?> livingentities = new List<LivingEntity?>();
        //투사체, 기술 등에 쓰이는 객체들을 관리하는 리스트로, 리스트에 삽입 / 삭제가 자주 발생하므로 링크드 리스트 사용
        public LinkedList<Entity?> entities = new LinkedList<Entity?>();
        public LinkedList<Entity?> shopentities = new LinkedList<Entity?>();
        //객체 업데이트 및 렌더링에 사용되는 리스트
        List<Entity?> allentities = new List<Entity?>();
        public int currentWidth, currentHeight, mouseX, mouseY, occupiedIndexCount, currentRound = 0;
        public bool handleMouseEvent, grabbed = false, isGameRunning = false, gameOverDetected = false;
        public const int worldWidth = 1000, worldHeight = 500;
        public CrossPlatformRandom random;
        public Random usualRandom = new Random();
        public ulong randomSeed;
        private BufferedGraphicsContext bufferContext;
        private BufferedGraphics buffer;
        private Graphics panelGraphics;
        public Player clientPlayer;
        public int[] leftCount = [0, 0];

        public Client gameClient; //클라이언트
        private int pingTick = 0;

        List<LeftLifeEntity> leftlives = new List<LeftLifeEntity>(4);
        List<NumberEntity> currentGold = new List<NumberEntity>(4);

        public List<(int, int, bool)> valueTupleList = new List<(int, int, bool)>()
        {
            (40, 5, false),
            (-40, 5, false),
            (-120, 5, false),
            (-200, 5, false),
            (-280, 5, false),
            (-360, 5, false),
            (-440, 5, false),
            (40, 105, false),
            (-40, 105, false),
            (-120, 105, false),
            (-200, 105, false),
            (-280, 105, false),
            (-360, 105, false),
            (-440, 105, false)
        };
        public List<(int, int)> shopValueTupleList = new List<(int, int)>()
        {
            (150, 5),
            (230, 5),
            (310, 5),
            (390, 5),
            (470, 5),
            (150, 105),
            (230, 105),
            (310, 105),
            (390, 105),
            (470, 105)
        };

        public GamePanel()
        {
            InitializeComponent();
            bufferContext = BufferedGraphicsManager.Current;
            buffer = bufferContext.Allocate(panelPlayScreen.CreateGraphics(), panelPlayScreen.DisplayRectangle);
            buffer.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            buffer.Graphics.PixelOffsetMode = PixelOffsetMode.None;
            buffer.Graphics.SmoothingMode = SmoothingMode.None;
            buffer.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            panelGraphics = panelPlayScreen.CreateGraphics();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            random = new CrossPlatformRandom();
            randomSeed = 0;
            this.Width += 1;
            currentWidth = this.Width - 50;
            this.currentHeight = (int)(currentWidth * 0.55);
            panelPlayScreen.Width = currentWidth; panelPlayScreen.Height = currentHeight;
            for (int i = 0; i < 10; i++)
            {
                LivingEntity test = CreateEntity((byte)(new Random().Next(10)), clientPlayer.playerName);
                for (int j = 0; j < test.entityLevel; j++) test.scaleEntity(1.2f);
                test.setPosition(shopValueTupleList[i].Item1, shopValueTupleList[i].Item2);
                test.hasAi = false;
                //test.deckIndex = 1;
                shopentities.AddFirst(test);
            }
            for (int i = 0; i < 4; i++)
            {
                LeftLifeEntity leftLife = new LeftLifeEntity(this, 550 - i * 40);
                addFreshEntity(leftLife);
                leftlives.Add(leftLife);
            }
            for (int i = 0; i < 3; i++)
            {
                NumberEntity number = new NumberEntity(this, -590 + i * 18, 620, 2, 0);
                addFreshEntity(number);
                currentGold.Add(number);
            }
            NumberEntity number2 = new NumberEntity(this, -590 + 80, 620, 2, 10);
            addFreshEntity(number2);
            currentGold.Add(number2);
            modifyGold(16);

            if (isMultiplayer)
            {
                gameClient = new Client(clientPlayer.playerName);
            } //클라이언트 초기화

        }
        public void playSound(WindowsMediaPlayer sound)
        {
            try
            {
                sound.settings.volume = 33;
                sound.controls.play();
            }
            catch (Exception)
            {
                // 예외 발생 시 무시합니다.
            }

        }
        public void addFreshLivingEntity(LivingEntity? entity)
        {
            livingentities.Add(entity);
        }
        public void addFreshEntity(Entity? entity)
        {
            entities.AddFirst(entity);
        }
        public void createNumberEntity(int number, int x, int y, int z)
        {
            number = int.Min(number, 999); // 0~999 범위로 제한
            if (number < 10)
            {
                FloatingNumberEntity numberEntity = new FloatingNumberEntity(this, x, y, z, number);
                addFreshEntity(numberEntity);
            }
            else if(number < 100)
            {
                FloatingNumberEntity numberEntity1 = new FloatingNumberEntity(this, x - 9, y, z, (number / 10) % 10);
                addFreshEntity(numberEntity1);
                FloatingNumberEntity numberEntity2 = new FloatingNumberEntity(this, x + 9, y, z, number % 10);
                addFreshEntity(numberEntity2);
            }
            else
            {
                FloatingNumberEntity numberEntity = new FloatingNumberEntity(this, x - 18, y, z, number / 100);
                addFreshEntity(numberEntity);
                FloatingNumberEntity numberEntity1 = new FloatingNumberEntity(this, x, y, z, (number / 10) % 10);
                addFreshEntity(numberEntity1);
                FloatingNumberEntity numberEntity2 = new FloatingNumberEntity(this, x + 18, y, z, number % 10);
                addFreshEntity(numberEntity2);
            }
        }
        public void createNumberEntity(int number, int x, int y, int z, float scalefactor)
        {
            float scale = (float)Math.Sqrt(scalefactor);
            number = int.Min(number, 999); // 0~999 범위로 제한
            if (number < 10)
            {
                FloatingNumberEntity numberEntity = new FloatingNumberEntity(this, x, y, z, number);
                numberEntity.visualSize = scalefactor;
                addFreshEntity(numberEntity);
            }
            else if (number < 100)
            {
                FloatingNumberEntity numberEntity1 = new FloatingNumberEntity(this, (int)(x - 9 * scale), y, z, (number / 10) % 10);
                numberEntity1.visualSize = scalefactor;
                addFreshEntity(numberEntity1);
                FloatingNumberEntity numberEntity2 = new FloatingNumberEntity(this, (int)(x + 9 * scale), y, z, number % 10);
                numberEntity2.visualSize = scalefactor;
                addFreshEntity(numberEntity2);
            }
            else
            {
                FloatingNumberEntity numberEntity = new FloatingNumberEntity(this, (int)(x - 18 * scale), y, z, number / 100);
                numberEntity.visualSize = scalefactor;
                addFreshEntity(numberEntity);
                FloatingNumberEntity numberEntity1 = new FloatingNumberEntity(this, x, y, z, (number / 10) % 10);
                numberEntity1.visualSize = scalefactor;
                addFreshEntity(numberEntity1);
                FloatingNumberEntity numberEntity2 = new FloatingNumberEntity(this, (int)(x + 18 * scale), y, z, number % 10);
                numberEntity2.visualSize = scalefactor;
                addFreshEntity(numberEntity2);
            }
        }
        private void logicTick_Tick(object sender, EventArgs e)
        {
            string detectTeam = clientPlayer.playerName;
            for (int i = livingentities.Count - 1; i != -1; i--)
            {
                LivingEntity livingentity = livingentities[i];
                livingentity.tick();
                if (livingentity.shouldRemove)
                {
                    livingentity = null;
                    livingentities.RemoveAt(i);
                    if (isGameRunning && !gameOverDetected && (leftCount[0] == 0 || leftCount[1] == 0))
                    {
                        gameOverDetected = true;
                        foreach (var entity in livingentities)
                        {
                            if (entity.isAlive())
                            {
                                if (entity.team.Equals(detectTeam))
                                {
                                    addFreshEntity(new backGroundNoiseEntity(this, 30));
                                    modifyGold(8);
                                    playSound(SoundCache.gameVictory);
                                    addFreshEntity(new VictoryMessageEntity(this));
                                }
                                else
                                {
                                    addFreshEntity(new backGroundNoiseEntity(this, clientPlayer.lifeLeft != 1 ? 30 : int.MaxValue));
                                    modifyGold(16);
                                    playSound(SoundCache.gameLost);
                                    clientPlayer.lifeLeft--;
                                    leftlives[0].discard();
                                    leftlives.RemoveAt(0);
                                    addFreshEntity(new LostMessageEntity(this));
                                }
                                break; // 해당 팀의 엔티티를 찾으면 루프 종료
                            }
                        }
                    }
                }
            }
            var node = entities.First;
            while (node != null)
            {
                var nextNode2 = node.Next;
                node.Value.tick();

                if (node.Value.shouldRemove)
                {
                    node.Value = null;
                    entities.Remove(node);
                }

                node = nextNode2;
            }
            node = shopentities.First;
            while (node != null)
            {
                var nextNode2 = node.Next;
                node.Value.tick();

                if (node.Value.shouldRemove)
                {
                    node.Value = null;
                    shopentities.Remove(node);
                }

                node = nextNode2;
            }
            allentities.Clear();
            allentities.Capacity = livingentities.Count + entities.Count + shopentities.Count;
            allentities.AddRange(livingentities);
            foreach (var entity in entities) allentities.Add(entity);
            foreach (var entity in shopentities) allentities.Add(entity);
            allentities.Sort((a, b) => a.z.CompareTo(b.z));
            if (handleMouseEvent)
            {
                handleMouseEvent = false;
                // 마우스 클릭 이벤트 처리
                float scale = (float)currentWidth / worldWidth;
                if (mouseX > currentWidth - 80 * scale && mouseY > currentHeight - 60 * scale
                    && clientPlayer.Gold >= 1)
                {
                    modifyGold(-1);
                    createNumberEntity(1, 570, 10, 30);
                    playSound(SoundCache.reroll);
                    foreach (var entity in shopentities)
                    {
                        entity.discard();
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        LivingEntity test = CreateEntity((byte)(new Random().Next(10)), clientPlayer.playerName);
                        //for (int j = 0; j < test.entityLevel; j++) test.scaleEntity(1.2f);
                        test.setPosition(shopValueTupleList[i].Item1, shopValueTupleList[i].Item2);
                        test.hasAi = false;
                        //test.deckIndex = 1;
                        shopentities.AddFirst(test);
                    }
                }
                else
                {
                    foreach (var entity in allentities)
                    {
                        if (entity is LivingEntity livingEntity && livingEntity.isAlive())
                        {
                            if (livingEntity.canStartTask()) continue; // 게임이 진행 중이고 AI가 있는 경우 클릭 무시
                            float x = livingEntity.x;
                            float y = livingEntity.y;
                            float z = livingEntity.z;
                            double scale2 = z > 200 ? 6.184 / Math.Cbrt(z + 250) : 0.823654768;
                            int screenX = currentWidth / 2 + (int)(x * scale * scale2);
                            int screenY = (int)(currentHeight - z * scale * scale2);
                            screenY -= (int)(y * scale * scale2);
                            int width = (int)(livingEntity.width * scale * scale2);
                            if (mouseX >= screenX - width / 2 && mouseX <= screenX + width / 2 &&
                                mouseY >= screenY - width && mouseY <= screenY)
                            {
                                livingEntity.grabOccurred();
                                break; // 클릭된 엔티티를 찾으면 루프 종료
                            }
                        }
                    }
                }
            }
            renderEntities();

            // ping 전송 (멀티플레이일 때만)
            pingTick++;
            if (gameClient != null && pingTick % 50 == 0)
            {
                gameClient.SendPing();
            }
        }

        public void modifyGold(int amount)
        {
            clientPlayer.Gold = int.Min(clientPlayer.Gold + amount, 255);
            currentGold[0].Image = NumberEntity.images[clientPlayer.Gold / 100];
            currentGold[1].Image = NumberEntity.images[(clientPlayer.Gold / 10) % 10];
            currentGold[2].Image = NumberEntity.images[clientPlayer.Gold % 10];
        }
        /*
        public void createNumberEntity(int number, int x, int y, int z)
        {
            NumberEntity numberEntity = new NumberEntity(this, number);
            numberEntity.setPosition(x, y, z);
            addFreshEntity(numberEntity);
        }
        */
        private Stopwatch _renderWatch = new Stopwatch();
        private void renderEntities()
        {
            _renderWatch.Restart();
            Graphics g = buffer.Graphics;

            float scale = (float)currentWidth / (float)worldWidth;
            //RenderBackground();
            g.DrawImage(Properties.Resources.필드1, 0, 0, currentWidth, currentHeight);

            if (allentities.Count > 0)
            {
                for (int i = allentities.Count - 1; i != -1; i--)
                {
                    Entity e = allentities[i];
                    if (e.renderType < 1) continue;
                    float x = e.x;
                    float y = e.y;
                    float z = e.z;
                    double scale2 = z > 200 ? 6.184 / Math.Cbrt(z + 250) : 0.823654768;
                    double scale3 = 3.6363 / Math.Cbrt(y + 50);
                    int screenX = currentWidth / 2 + (int)(x * scale * scale2);
                    int screenY = (int)(currentHeight - z * scale * scale2);
                    switch (e.renderType)
                    {
                        case 1:
                        case 2:
                            int shadowSize = (int)(e.width * scale * scale2); // 엔티티 크기 (픽셀)
                            using (Brush shadowBrush = new SolidBrush(Color.FromArgb((int)(120 * scale3), clientPlayer.playerName.Equals(e.team) ? Color.Black : Color.Firebrick)))
                            {
                                int shadowWidth = (int)(shadowSize * 1.2 / scale3);
                                int shadowHeight = (int)(shadowSize * 0.4 / scale3);
                                int shadowX = screenX - shadowWidth / 2;
                                int shadowY = screenY - (int)(shadowHeight * 0.75); // 약간 위로 올림

                                g.FillEllipse(shadowBrush, shadowX, shadowY, shadowWidth, shadowHeight);
                            }
                            if (e.renderType == 2) allentities.RemoveAt(i); // 엔티티 제거
                            break;
                        case 3:
                            int width = (int)(e.Image.Width * e.visualSize * scale * scale2); // 엔티티 크기 (픽셀)
                            int height = (int)(e.Image.Height * e.visualSize * scale * scale2); // 엔티티 크기 (픽셀)
                            screenY -= (int)(y * scale * scale2);
                            g.DrawImage(e.Image,
                                screenX - width / 2, screenY - height,
                                width, height);
                            break;
                    }
                }
                for (int i = allentities.Count - 1; i != -1; i--)
                {
                    Entity e = allentities[i];
                    if (e.renderType == -1) continue;
                    float angle = e.desiredAngle;
                    float x = e.x;
                    float y = e.y;
                    float z = e.z;
                    double scale2 = z > 200 ? 6.184 / Math.Cbrt(z + 250) : 0.823654768;
                    double scale3 = 3.6363 / Math.Cbrt(y + 50);
                    // 엔티티의 world 좌표(entity.x, entity.y)를 renderPanel 좌표로 변환
                    int screenX = currentWidth / 2 + (int)(x * scale * scale2);
                    int screenY = (int)(currentHeight - z * scale * scale2);
                    int width = (int)(e.Image.Width * e.visualSize * scale * scale2); // 엔티티 크기 (픽셀)
                    int height = (int)(e.Image.Height * e.visualSize * scale * scale2); // 엔티티 크기 (픽셀)
                    screenY -= (int)(y * scale * scale2);
                    if (angle == 0)
                        g.DrawImage(e.Image,
                            screenX - width / 2, screenY - height,
                            width, height);
                    else
                    {
                        // 회전된 이미지를 그리기 위해 Graphics 객체를 사용
                        g.TranslateTransform(screenX, screenY);
                        g.RotateTransform(angle);
                        g.DrawImage(e.Image, -width / 2, -height, width, height);
                        g.ResetTransform(); // 변환을 초기화하여 다음 이미지에 영향을 주지 않도록 함
                    }
                }
            }
            buffer.Render(panelGraphics);
            _renderWatch.Stop();
            int elapsed = (int)_renderWatch.ElapsedMilliseconds;

            // 3) 다음 인터벌 계산
            int nextInterval = 42 - elapsed;
            if (nextInterval < 1)
                nextInterval = 1;               // 최소 1ms
            else if (nextInterval > 42)
                nextInterval = 42; // 최대 16ms

            // 4) 타이머에 반영
            logicTick.Interval = nextInterval;
        }
        public int getRandomInteger(int max)
        {
            random.setSeed(randomSeed++);
            return random.Next(max);
        }

        public float Lerp(float f1, float f2, float f3)
        {
            return f1 + (f2 - f1) * f3;
        }

        private void GamePanel_Resize(object sender, EventArgs e)
        {
            currentWidth = this.Width - 50;
            this.currentHeight = (int)(currentWidth * 0.55);
            panelPlayScreen.Width = currentWidth; panelPlayScreen.Height = currentHeight;
            AllocateBuffer(); // 크기 변경 시 버퍼 재할당
            renderEntities(); // 화면 다시 그리기
        }
        private void AllocateBuffer()
        {
            if (buffer != null)
            {
                buffer.Dispose(); // 기존 버퍼 제거
            }

            bufferContext = BufferedGraphicsManager.Current;
            buffer = bufferContext.Allocate(panelPlayScreen.CreateGraphics(), panelPlayScreen.DisplayRectangle);
            buffer.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            buffer.Graphics.PixelOffsetMode = PixelOffsetMode.None;
            buffer.Graphics.SmoothingMode = SmoothingMode.None;
            buffer.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logicTick.Enabled = !logicTick.Enabled;
        }

        private void panelPlayScreen_MouseClick(object sender, MouseEventArgs e)
        {
            if (grabbed)
            {
                grabbed = false; return;
            }
            mouseX = e.X; mouseY = e.Y;
            handleMouseEvent = true;
        }

        private void panelPlayScreen_MouseMove(object sender, MouseEventArgs e)
        {
            if (grabbed)
            {
                mouseX = e.X; mouseY = e.Y;
            }
        }

        private void GamePanel_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (gameClient != null)
                gameClient.SendLeave();
            this.Owner?.Show(); // 부모 폼을 다시 보여줌
        }

        public List<T> getAllLivingEntities<T>() where T : LivingEntity
        {
            List<T> result = new List<T>(livingentities.Count);
            foreach (var entity in livingentities)
            {
                if (entity is T)
                {
                    result.Add(entity as T);
                }
            }
            return result;
        }

        public List<T> getAllEntities<T>() where T : Entity
        {
            List<T> result = new List<T>(entities.Count);
            foreach (var entity in entities)
            {
                if (entity is T)
                {
                    result.Add(entity as T);
                }
            }
            return result;
        }
        public void RecoverPlayerEntities()
        {
            foreach (var entity in livingentities)
            {
                entity.discard();
            }
            clientPlayer.entitiesofplayer.Clear();
            leftCount[0] = 0; leftCount[1] = 0;
            foreach (var se in serialized)
            {
                LivingEntity? entity = TranslateAndCreateLivingEntity(se.Id, clientPlayer.playerName);
                entity.entityLevel = se.EntityLevel;
                for (int i = 0; i < entity.entityLevel; i++) entity.scaleEntity(1.2f);
                entity.setPosition(se.X, 2, se.Z);
                if (se.ItemId1 != 0)
                {
                    Item? item1 = TranslateAndCreateItem(se.ItemId1);
                    item1.ApplyTo(entity);
                }
                if (se.ItemId2 != 0)
                {
                    Item? item2 = TranslateAndCreateItem(se.ItemId2);
                    item2.ApplyTo(entity);
                }
                if (se.ItemId3 != 0)
                {
                    Item? item3 = TranslateAndCreateItem(se.ItemId3);
                    item3.ApplyTo(entity);
                }
                entity.isPurchased = true; // 아이템이 장착된 상태로 설정
                entity.direction = Direction.Right; // 기본 방향 설정
                //entity.EquippedItems[1] = TranslateAndCreateItem(se.ItemId2);
                //entity.EquippedItems[2] = TranslateAndCreateItem(se.ItemId3);
                addFreshLivingEntity(entity);
                clientPlayer.entitiesofplayer.Add(entity);
                leftCount[0]++;
            }
        }
        public void PlaceOpponentEntities(List<SerializedEntity> sentities, string enemy)
        {
            foreach (var se in sentities)
            {
                LivingEntity? entity = TranslateAndCreateLivingEntity(se.Id, enemy);
                entity.entityLevel = se.EntityLevel;
                for (int i = 0; i < entity.entityLevel; i++) entity.scaleEntity(1.2f);
                entity.setPosition(-se.X, 2, se.Z);
                if (se.ItemId1 != 0)
                {
                    Item? item1 = TranslateAndCreateItem(se.ItemId1);
                    item1.ApplyTo(entity);
                }
                if (se.ItemId2 != 0)
                {
                    Item? item2 = TranslateAndCreateItem(se.ItemId2);
                    item2.ApplyTo(entity);
                }
                if (se.ItemId3 != 0)
                {
                    Item? item3 = TranslateAndCreateItem(se.ItemId3);
                    item3.ApplyTo(entity);
                }
                entity.isPurchased = true; // 아이템이 장착된 상태로 설정
                entity.direction = Direction.Right; // 기본 방향 설정
                //entity.EquippedItems[1] = TranslateAndCreateItem(se.ItemId2);
                //entity.EquippedItems[2] = TranslateAndCreateItem(se.ItemId3);
                addFreshLivingEntity(entity);
                clientPlayer.entitiesofplayer.Add(entity);
                leftCount[1]++;
            }
        }
        //서버에서 전송된 캐릭터 타입을 받아서 해당 캐릭터를 생성하는 메서드
        public LivingEntity TranslateAndCreateLivingEntity(byte type, string name)
        {
            return type switch
            {
                1 => new WeirdGuy(this) { team = name },
                2 => new GiantWeirdGuy(this) { team = name },
                10 => new Skels(this) { team = name },
                11 => new SkelsBig(this) { team = name },
                12 => new Skulls(this) { team = name },
                20 => new Boxer(this) { team = name },
                21 => new FlyingEye1(this) { team = name },
                22 => new Ghost1(this) { team = name },
                23 => new Mushroom1(this) { team = name },
                _ => throw new ArgumentException("존재하지 않는 캐릭터 타입입니다.")
            };
        }
        public Item? TranslateAndCreateItem(byte type)
        {
            return type switch
            {
                1 => new EjectionDeviceItem(),
                2 => new ExplosiveGasVesselItem(),
                3 => new EjectionShoesItem(),
                4 => new FlamingGloveItem(),
                5 => new ChainsawItem(),
                _ => throw new ArgumentException("존재하지 않는 아이템 타입입니다.")
            };
        }
        //아이디값 입력받으면 거기에 상응하는 캐릭터를 반환해주는 메서드
        //상점에서 사용되는 것임. 서버에서는 다른 것을 사용할 예정
        public LivingEntity CreateEntity(byte type, string name)
        {
            #region 아이디별 객체 타입 열람표
            /*
             * 0 : WeirdGuy
             * 1 : Skels
             * 2 : SkelsBig
             * 3 : Skulls
             * 4 : 
             */
            #endregion
            return type switch
            {
                0 => CreateItemEntity((byte)(new Random().Next(5)), name),
                1 => new Skels(this) { team = name },
                2 => new SkelsBig(this) { team = name },
                3 => new Skulls(this) { team = name },
                4 => new GiantWeirdGuy(this) { team = name },
                5 => new WeirdGuy(this) { team = name }, // 체인톱 아이템
                6 => new Ghost1(this) { team = name },
                7 => new Boxer(this) { team = name },
                8 => new Mushroom1(this) { team = name },
                9 => new FlyingEye1(this) { team = name },
                _ => throw new ArgumentException("존재하지 않는 캐릭터 타입입니다.")
            };
        }
        public ItemEntity CreateItemEntity(byte type, string name)
        {
            return type switch
            {
                0 => new ChainsawItemEntity(this) { team = name },
                1 => new EjectionDeviceItemEntity(this) { team = name },
                2 => new ExplosiveGasVesselItemEntity(this) { team = name },
                3 => new EjectionShoesItemEntityItemEntity(this) { team = name },
                4 => new FlamingGloveItemEntity(this) { team = name },
            };
        }
        public List<SerializedEntity> serialized;
        private async void btnGameStart_Click(object sender, EventArgs e)
        {
            if (isGameRunning || leftCount[0] == 0) return;
            //유닛 정보 서버 전송
            serialized = new List<SerializedEntity>();
            foreach (var unit in clientPlayer.entitiesofplayer)
            {
                SerializedEntity se = new SerializedEntity
                {
                    Id = unit.getLivingEntityId(), // LivingEntity의 고유 ID 사용
                    EntityLevel = unit.entityLevel, // LivingEntity의 고유 ID 사용
                    X = (int)unit.x,                          // x 좌표
                    Z = (int)unit.z,                           // y 좌표
                };
                for (int i = 0; i < unit.EquippedItems.Count; i++)
                {
                    switch(i)
                    {
                        case 0 : 
                            se.ItemId1 = (byte)unit.EquippedItems[0].Id; break;
                        case 1:
                            se.ItemId2 = (byte)unit.EquippedItems[1].Id; break;
                        case 2:
                            se.ItemId3 = (byte)unit.EquippedItems[2].Id; break;
                    }
                }
                serialized.Add(se);
            }
            if (gameClient != null)
            {
                List<SerializedEntity> enemies;
                ulong seed;
                gameClient.isReady = true;
                // 빈 덱·seed=0 반환되면 재요청
                gameClient.SendEntities(serialized);
                do
                {
                    (enemies, seed) =
                        await gameClient.RequestOpponentEntities();
                    if (enemies.Count == 0 || seed == 0UL)
                        await Task.Delay(200);
                } while (enemies.Count == 0 || seed == 0UL);

                // 서버가 내려준 seed 사용!
                randomSeed = seed;

                PlaceOpponentEntities(enemies, "Enemy");
                isGameRunning = true;
                gameOverDetected = false;

            }
            else
            {
                randomSeed = (ulong)(new Random().Next(int.MaxValue));
                isGameRunning = true; gameOverDetected = false;
                int factor1 = (int)Math.Cbrt(currentRound), factor2 = currentRound / 4, factor3 = (int) Math.Log(currentRound, 3);
                for (int i = 0; i < 3 + factor2; i++)
                {
                    LivingEntity test = CreateEntity((byte)(getRandomInteger(9) + 1), "Enemy");
                    test.setPosition(getRandomInteger(500), getRandomInteger(450) + 200);
                    if (currentRound > 0 && getRandomInteger(10 + factor1) < factor1)
                    {
                        test.entityLevel = (byte)(int.Min(getRandomInteger(1 + factor3) + 1, 255));
                        for (int j = 0; j < test.entityLevel; j++) test.scaleEntity(1.2f);
                    }
                    if (getRandomInteger(100 + currentRound) < currentRound)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            Item item = TranslateAndCreateItem((byte)(getRandomInteger(5) + 1));
                            bool canMerge = true;
                            foreach (var equippedItem in test.EquippedItems)
                            {
                                if (equippedItem.Id == item.Id)
                                {
                                    canMerge = false; // 이미 같은 아이템이 장착되어 있으면 조합 불가
                                    break;
                                }
                            }
                            if (canMerge) item.ApplyTo(test);
                        }
                    }
                    addFreshLivingEntity(test);
                    leftCount[1]++;
                }
            }
            currentRound++;
            createNumberEntity(currentRound, 0, 575, 1, 3);
        }
    }
}
