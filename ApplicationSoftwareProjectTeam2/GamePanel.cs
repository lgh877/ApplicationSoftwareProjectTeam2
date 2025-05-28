using ApplicationSoftwareProjectTeam2.entities;
using ApplicationSoftwareProjectTeam2.entities.weirdos;
using System;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Xml.Linq;

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
        //��ü ������Ʈ �� �������� ���Ǵ� ����Ʈ
        List<Entity?> allentities = new List<Entity?>();
        int currentWidth, currentHeight;
        const int worldWidth = 1000, worldHeight = 500;
        public CrossPlatformRandom random;
        public ulong randomSeed;
        private BufferedGraphicsContext bufferContext;
        private BufferedGraphics buffer;
        private Graphics panelGraphics;


        public GamePanel()
        {
            InitializeComponent();
            bufferContext = BufferedGraphicsManager.Current;
            buffer = bufferContext.Allocate(panelPlayScreen.CreateGraphics(), panelPlayScreen.DisplayRectangle);
            buffer.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            buffer.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            buffer.Graphics.SmoothingMode = SmoothingMode.None;
            buffer.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            panelGraphics = panelPlayScreen.CreateGraphics();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            random = new CrossPlatformRandom();
            randomSeed = 0;

            this.Width += 1;

            for (int i = 0; i < 32; i++)
            {
                WeirdGuy test = new WeirdGuy(this);
                test.attackDamage = 0;
                test.scaleEntity((getRandomInteger(20) + 10) * 0.1f);
                test.setPosition(getRandomInteger(1000) - 500, getRandomInteger(450) + 200);
                test.team = getRandomInteger(101).ToString();
                addFreshLivingEntity(test);
            }

            currentWidth = this.Width - 50;
            this.currentHeight = (int)(currentWidth * 0.55);
            panelPlayScreen.Width = currentWidth; panelPlayScreen.Height = currentHeight;
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
            allentities.Clear();
            allentities.Capacity = livingentities.Count + entities.Count;
            allentities.AddRange(livingentities);
            foreach (var entity in entities) allentities.Add(entity);
            allentities.Sort((a, b) => a.z.CompareTo(b.z));
            renderEntities();
        }



        private Stopwatch _renderWatch = new Stopwatch();
        private void renderEntities()
        {
            _renderWatch.Restart();
            Graphics g = buffer.Graphics;
            g.Clear(panelPlayScreen.BackColor);

            // renderPanel�� ClientSize�� �������� ������ ���
            float scale = (float)currentWidth / (float)worldWidth;

            if (allentities.Count > 0)
            {
                for (int i = allentities.Count - 1; i != -1; i--)
                {
                    Entity e = allentities[i];
                    float x = e.x;
                    float y = e.y;
                    float z = e.z;
                    double scale2 = 6.184 / Math.Cbrt(z + 250);
                    double scale3 = 3.6363 / Math.Cbrt(y + 50);
                    // ��ƼƼ�� world ��ǥ(entity.x, entity.y)�� renderPanel ��ǥ�� ��ȯ
                    int screenX = currentWidth / 2 + (int)(x * scale * scale2);
                    int screenY = (int)(currentHeight - z * scale * scale2);
                    int width = (int)(e.Image.Width * e.visualSize * scale * scale2); // ��ƼƼ ũ�� (�ȼ�)
                    int height = (int)(e.Image.Height * e.visualSize * scale * scale2); // ��ƼƼ ũ�� (�ȼ�)
                    int shadowSize = (int)(e.width * scale * scale2); // ��ƼƼ ũ�� (�ȼ�)
                    using (Brush shadowBrush = new SolidBrush(Color.FromArgb((int)(80 * scale3), Color.Black)))
                    {
                        int shadowWidth = (int)(shadowSize * 1.2 / scale3);
                        int shadowHeight = (int)(shadowSize * 0.4 / scale3);
                        int shadowX = screenX - shadowWidth / 2;
                        int shadowY = screenY - (int)(shadowHeight * 0.75); // �ణ ���� �ø�

                        g.FillEllipse(shadowBrush, shadowX, shadowY, shadowWidth, shadowHeight);
                    }
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
            int nextInterval = 32 - elapsed;
            if (nextInterval < 1)
                nextInterval = 1;               // �ּ� 1ms
            else if (nextInterval > 32)
                nextInterval = 32; // �ִ� 16ms

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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            logicTick.Enabled = !logicTick.Enabled;
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
    }
}
