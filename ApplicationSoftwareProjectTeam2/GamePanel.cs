using ApplicationSoftwareProjectTeam2.entities;
using ApplicationSoftwareProjectTeam2.entities.creatures;
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
        //ĳ���� ��ü���� �����ϴ� ����Ʈ��, ������Ʈ�� ���� �幰�� �׳� ����Ʈ�� ������
        public List<LivingEntity?> livingentities = new List<LivingEntity?>();
        //����ü, ��� � ���̴� ��ü���� �����ϴ� ����Ʈ��, ����Ʈ�� ���� / ������ ���� �߻��ϹǷ� ��ũ�� ����Ʈ ���
        public LinkedList<Entity?> entities = new LinkedList<Entity?>();
        public LinkedList<Entity?> shopentities = new LinkedList<Entity?>();
        //��ü ������Ʈ �� �������� ���Ǵ� ����Ʈ
        List<Entity?> allentities = new List<Entity?>();
        public int currentWidth, currentHeight, mouseX, mouseY, occupiedIndexCount;
        public bool handleMouseEvent, grabbed = false, isGameRunning = false;
        public const int worldWidth = 1000, worldHeight = 500;
        public CrossPlatformRandom random;
        public ulong randomSeed;
        private BufferedGraphicsContext bufferContext;
        private BufferedGraphics buffer;
        private Graphics panelGraphics;
        public Player clientPlayer;
        //WindowsMediaPlayer soundEffectPlayer = new WindowsMediaPlayer();
        LinkedList<WindowsMediaPlayer> soundList = new LinkedList<WindowsMediaPlayer>();

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

            for (int i = 0; i < 6; i++)
            {
                LivingEntity test = CreateEntity((byte)(random.Next(4)), getRandomInteger(450).ToString()); //"Enemy");
                test.setPosition(getRandomInteger(500), getRandomInteger(450) + 200);
                addFreshLivingEntity(test);
            }
            currentWidth = this.Width - 50;
            this.currentHeight = (int)(currentWidth * 0.55);
            panelPlayScreen.Width = currentWidth; panelPlayScreen.Height = currentHeight;
        }
        public void playSound(WindowsMediaPlayer sound)
        {
            sound.settings.volume = 100; // ���� ���� (0-100)
            sound.controls.play();
        }
        public void addFreshLivingEntity(LivingEntity? entity)
        {
            livingentities.Add(entity);
        }
        public void addFreshEntity(Entity? entity)
        {
            entities.AddFirst(entity);
        }
        private void logicTick_Tick(object sender, EventArgs e)
        {
            bool isGameRunning = true;
            string detectTeam = clientPlayer.playerName;
            for (int i = livingentities.Count - 1; i != -1; i--)
            {
                LivingEntity livingentity = livingentities[i];
                livingentity.tick();
                if (livingentity.shouldRemove)
                {
                    livingentity = null;
                    livingentities.RemoveAt(i);
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
                // ���콺 Ŭ�� �̺�Ʈ ó��
                float scale = (float)currentWidth / worldWidth;
                if (mouseX > currentWidth - 80 * scale && mouseY > currentHeight - 60 * scale
                    && clientPlayer.Gold >= 1)
                {
                    clientPlayer.Gold--;
                    label1.Text = $"Gold: {clientPlayer.Gold}";
                    foreach (var entity in shopentities)
                    {
                        entity.discard();
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        LivingEntity test = CreateEntity((byte)(new Random().Next(6)), clientPlayer.playerName);
                        for (int j = 0; j < test.entityLevel; j++) test.scaleEntity(1.2f);
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
                            if (livingEntity.canStartTask()) continue; // ������ ���� ���̰� AI�� �ִ� ��� Ŭ�� ����
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
                                break; // Ŭ���� ��ƼƼ�� ã���� ���� ����
                            }
                        }
                    }
                }
            }
            renderEntities();
        }



        private Stopwatch _renderWatch = new Stopwatch();
        private void renderEntities()
        {
            _renderWatch.Restart();
            Graphics g = buffer.Graphics;

            float scale = (float)currentWidth / (float)worldWidth;
            //RenderBackground();
            g.DrawImage(Properties.Resources.�ʵ�1, 0, 0, currentWidth, currentHeight);

            if (allentities.Count > 0)
            {
                for (int i = allentities.Count - 1; i != -1; i--)
                {
                    Entity e = allentities[i];
                    if (e.renderType == 0) continue;
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
                            int shadowSize = (int)(e.width * scale * scale2); // ��ƼƼ ũ�� (�ȼ�)
                            using (Brush shadowBrush = new SolidBrush(Color.FromArgb((int)(120 * scale3), clientPlayer.playerName.Equals(e.team) ? Color.Black : Color.Firebrick)))
                            {
                                int shadowWidth = (int)(shadowSize * 1.2 / scale3);
                                int shadowHeight = (int)(shadowSize * 0.4 / scale3);
                                int shadowX = screenX - shadowWidth / 2;
                                int shadowY = screenY - (int)(shadowHeight * 0.75); // �ణ ���� �ø�

                                g.FillEllipse(shadowBrush, shadowX, shadowY, shadowWidth, shadowHeight);
                            }
                            if (e.renderType == 2) allentities.RemoveAt(i); // ��ƼƼ ����
                            break;
                        case 3:
                            int width = (int)(e.Image.Width * e.visualSize * scale * scale2); // ��ƼƼ ũ�� (�ȼ�)
                            int height = (int)(e.Image.Height * e.visualSize * scale * scale2); // ��ƼƼ ũ�� (�ȼ�)
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
                    float x = e.x;
                    float y = e.y;
                    float z = e.z;
                    double scale2 = z > 200 ? 6.184 / Math.Cbrt(z + 250) : 0.823654768;
                    double scale3 = 3.6363 / Math.Cbrt(y + 50);
                    // ��ƼƼ�� world ��ǥ(entity.x, entity.y)�� renderPanel ��ǥ�� ��ȯ
                    int screenX = currentWidth / 2 + (int)(x * scale * scale2);
                    int screenY = (int)(currentHeight - z * scale * scale2);
                    int width = (int)(e.Image.Width * e.visualSize * scale * scale2); // ��ƼƼ ũ�� (�ȼ�)
                    int height = (int)(e.Image.Height * e.visualSize * scale * scale2); // ��ƼƼ ũ�� (�ȼ�)
                    /*
                    int shadowSize = (int)(e.width * scale * scale2); // ��ƼƼ ũ�� (�ȼ�)
                    using (Brush shadowBrush = new SolidBrush(Color.FromArgb((int)(80 * scale3), Color.Black)))
                    {
                        int shadowWidth = (int)(shadowSize * 1.2 / scale3);
                        int shadowHeight = (int)(shadowSize * 0.4 / scale3);
                        int shadowX = screenX - shadowWidth / 2;
                        int shadowY = screenY - (int)(shadowHeight * 0.75); // �ణ ���� �ø�

                        g.FillEllipse(shadowBrush, shadowX, shadowY, shadowWidth, shadowHeight);
                    }
                    */
                    screenY -= (int)(y * scale * scale2);
                    g.DrawImage(e.Image,
                        screenX - width / 2, screenY - height,
                        width, height);
                }
            }
            buffer.Render(panelGraphics);
            _renderWatch.Stop();
            int elapsed = (int)_renderWatch.ElapsedMilliseconds;

            // 3) ���� ���͹� ���
            int nextInterval = 42 - elapsed;
            if (nextInterval < 1)
                nextInterval = 1;               // �ּ� 1ms
            else if (nextInterval > 42)
                nextInterval = 42; // �ִ� 16ms

            // 4) Ÿ�̸ӿ� �ݿ�
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
            AllocateBuffer(); // ũ�� ���� �� ���� ���Ҵ�
            renderEntities(); // ȭ�� �ٽ� �׸���
        }
        private void AllocateBuffer()
        {
            if (buffer != null)
            {
                buffer.Dispose(); // ���� ���� ����
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
            this.Owner?.Show(); // �θ� ���� �ٽ� ������
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
        //���̵� �Է¹����� �ű⿡ �����ϴ� ĳ���͸� ��ȯ���ִ� �޼���
        public LivingEntity CreateEntity(byte type, string name)
        {
            #region ���̵� ��ü Ÿ�� ����ǥ
            /*
             * 0 : WeirdGuy
             * 1 : Skels
             * 2 : GiantWeirdGuy
             * 3 : SkelsBig
             */
            #endregion
            return type switch
            {
                0 => new WeirdGuy(this) { team = name },
                1 => new Skels(this) { team = name },
                2 => new SkelsBig(this) { team = name },
                3 => new Skulls(this) { team = name },
                4 => new GiantWeirdGuy(this) { team = name },
                5 => new ItemTest(this) { team = name },
                _ => throw new ArgumentException("�������� �ʴ� ĳ���� Ÿ���Դϴ�.")
            };
        }

        private void btnGameStart_Click(object sender, EventArgs e)
        {
            isGameRunning = true;
        }
    }
}
