using ApplicationSoftwareProjectTeam2.entities;
using System.Xml.Linq;

namespace ApplicationSoftwareProjectTeam2
{
    public partial class GamePanel : Form
    {
        //ĳ���� ��ü���� �����ϴ� ����Ʈ��, ������Ʈ�� ���� �幰�� �׳� ����Ʈ�� ������
        public List<LivingEntity?> livingentities = new List<LivingEntity?> ();
        //����ü, ��� � ���̴� ��ü���� �����ϴ� ����Ʈ��, ����Ʈ�� ���� / ������ ���� �߻��ϹǷ� ��ũ�� ����Ʈ ���
        public LinkedList<Entity?> entities = new LinkedList<Entity?> ();
        //��ü ������Ʈ �� �������� ���Ǵ� ����Ʈ
        List<Entity?> allentities = new List<Entity?> ();
        int levelTickCount, currentWidth, currentHeight;
        const int worldWidth = 1000, worldHeight = 500;
        public CrossPlatformRandom random;
        public ulong randomSeed;
        private BufferedGraphicsContext bufferContext;
        private BufferedGraphics buffer;



        public GamePanel()
        {
            InitializeComponent();
            bufferContext = BufferedGraphicsManager.Current;
            buffer = bufferContext.Allocate(panelPlayScreen.CreateGraphics(), panelPlayScreen.DisplayRectangle);
            this.DoubleBuffered = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //bufferContext = BufferedGraphicsManager.Current;
            //buffer = bufferContext.Allocate(panelPlayScreen.CreateGraphics(), panelPlayScreen.DisplayRectangle);
            //this.DoubleBuffered = true;
            random = new CrossPlatformRandom();
            randomSeed = 0;
            levelTickCount = 0;
            //livingentities = new List<LivingEntity?>();
            //allentities = new List<Entity?>();
            //entities = new LinkedList<Entity?>();

            this.Width += 1;

            for (int i = 0; i < 200; i++)
            {
                LivingEntity test = new LivingEntity(this);
                test.setPosition(getRandomInteger(1000) - 500, getRandomInteger(450));
                test.team = getRandomInteger(101).ToString();
                addFreshLivingEntity(test);
            }

            currentWidth = this.Width - 50;
            this.currentHeight = (int)(currentWidth * 0.5);
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
            if (++levelTickCount == 2)
            {
                levelTickCount = 0;
                for (int i = livingentities.Count - 1; i != -1; i--)
                {
                    livingentities[i].tick();
                    if (livingentities[i].shouldRemove)
                    {
                        livingentities[i] = null;
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
            }
            allentities.Clear();
            allentities.Capacity = livingentities.Count + entities.Count;
            allentities.AddRange(livingentities);
            foreach (var entity in entities) allentities.Add(entity);
            allentities.Sort((a, b) => a.z.CompareTo(b.z));
            renderEntities();
        }

        private void renderEntities()
        {
            Graphics g = buffer.Graphics;
            g.Clear(panelPlayScreen.BackColor);

            if (allentities.Count > 0)
            {
                // renderPanel�� ClientSize�� �������� ������ ���
                float scale = (float)panelPlayScreen.Width / (float)worldWidth;

                // �� ��ƼƼ�� ȭ�鿡 �׸��ϴ�.
                for (int i = allentities.Count - 1; i != -1; i--)
                {
                    float x = Lerp(allentities[i].xold, allentities[i].x);
                    float z = Lerp(allentities[i].zold, allentities[i].z);
                    double scale2 = 6.184 / Math.Cbrt(z + 250);
                    // ��ƼƼ�� world ��ǥ(entity.x, entity.y)�� renderPanel ��ǥ�� ��ȯ
                    int screenX = panelPlayScreen.Width / 2 + (int)(x * scale * scale2);
                    int screenY = (int)(currentHeight - z * scale * scale2);

                    // ���÷� ��ƼƼ�� ������ ǥ�� (50% �߽� ����)
                    int size = (int)(allentities[i].visualSize * scale * scale2); // ��ƼƼ ũ�� (�ȼ�)

                    g.DrawImage(allentities[i].Image, screenX - size / 2, screenY - size, size, size);

                    //entity.Width = size; entity.Height = size;
                    //entity.Location = new Point(screenX - size / 2, screenY - size);
                    //e.Graphics.FillEllipse(Brushes.White, screenX - (size / 2), screenY - (size / 2), size, size);
                }
            }
            buffer.Render(panelPlayScreen.CreateGraphics());
        }

        public int getRandomInteger(int max)
        {
            random.setSeed(randomSeed++);
            return random.Next(max);
        }

        public float Lerp(float f1, float f2)
        {
            return f1 + (f2 - f1) * (float)levelTickCount * 0.5f;
        }

        private void GamePanel_Resize(object sender, EventArgs e)
        {
            currentWidth = this.Width - 50;
            this.currentHeight = (int)(currentWidth * 0.5);
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
