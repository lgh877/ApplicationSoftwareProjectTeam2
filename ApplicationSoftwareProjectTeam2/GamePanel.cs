using ApplicationSoftwareProjectTeam2.entities;

namespace ApplicationSoftwareProjectTeam2
{
    public partial class GamePanel : Form
    {
        //ĳ���� ��ü���� �����ϴ� ����Ʈ��, ������Ʈ�� ���� �幰�� �׳� ����Ʈ�� ������
        List<LivingEntity> livingentities;
        //����ü, ��� � ���̴� ��ü���� �����ϴ� ����Ʈ��, ����Ʈ�� ���� / ������ ���� �߻��ϹǷ� ��ũ�� ����Ʈ ���
        LinkedList<Entity> entities;
        //��ü ������Ʈ �� �������� ���Ǵ� ����Ʈ
        List<Entity> allentities;
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
            random = new CrossPlatformRandom();
            randomSeed = 0;
            levelTickCount = 0;
            livingentities = new List<LivingEntity>();
            allentities = new List<Entity>();
            entities = new LinkedList<Entity>();

            currentWidth = this.Width - 50;
            this.currentHeight = (int)(currentWidth * 0.5);
            panelPlayScreen.Width = currentWidth; panelPlayScreen.Height = currentHeight;

            for (int i = 0; i < 20; i++)
            {
                LivingEntity test = new LivingEntity(this);
                test.setPosition(getRandomInteger(1000) - 500, getRandomInteger(450));
                test.team = getRandomInteger(4).ToString();
                addFreshLivingEntity(ref test);
            }
        }

        public void addFreshLivingEntity(ref LivingEntity? entity)
        {
            livingentities.Add(entity);
        }
        public void addFreshEntity(ref Entity entity)
        {
            entities.AddFirst(entity);
        }

        private void logicTick_Tick(object sender, EventArgs e)
        {
            int totalSize = livingentities.Count + entities.Count;
            allentities = new List<Entity>(totalSize);
            allentities.AddRange(livingentities);
            foreach (var entity in entities) allentities.Add(entity);
            if (++levelTickCount == 2)
            {
                levelTickCount = 0;
                foreach (Entity entity in allentities) entity.tick();
            }
            allentities.Sort((a, b) => a.z.CompareTo(b.z));
            renderEntities();
        }

        public int getRandomInteger(int max)
        {
            random.setSeed(randomSeed++);
            return random.Next(max);
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
                foreach (Entity entity in allentities)
                {
                    float x = Lerp(entity.xold, entity.x);
                    float z = Lerp(entity.zold, entity.z);
                    double scale2 = 6.184 / Math.Cbrt(z + 250);
                    // ��ƼƼ�� world ��ǥ(entity.x, entity.y)�� renderPanel ��ǥ�� ��ȯ
                    int screenX = panelPlayScreen.Width / 2 + (int)(x * scale * scale2);
                    int screenY = (int)(currentHeight - z * scale * scale2);

                    // ���÷� ��ƼƼ�� ������ ǥ�� (50% �߽� ����)
                    int size = (int)(entity.visualSize * scale * scale2); // ��ƼƼ ũ�� (�ȼ�)

                    g.DrawImage(entity.Image, screenX - size / 2, screenY - size, size, size);

                    //entity.Width = size; entity.Height = size;
                    //entity.Location = new Point(screenX - size / 2, screenY - size);
                    //e.Graphics.FillEllipse(Brushes.White, screenX - (size / 2), screenY - (size / 2), size, size);
                }
            }
            buffer.Render(panelPlayScreen.CreateGraphics());
        }

        private void panelPlayScreen_Paint(object sender, PaintEventArgs e)
        {
            // ��� Ŭ����
            //e.Graphics.Clear(Color.Gray);
            Graphics g = e.Graphics;

            if (allentities.Count > 0)
            {
                // renderPanel�� ClientSize�� �������� ������ ���
                float scale = (float)panelPlayScreen.Width / (float)worldWidth;

                // �� ��ƼƼ�� ȭ�鿡 �׸��ϴ�.
                foreach (Entity entity in allentities)
                {
                    float x = 500 + Lerp(entity.xold, entity.x);
                    float z = Lerp(entity.zold, entity.z);
                    // ��ƼƼ�� world ��ǥ(entity.x, entity.y)�� renderPanel ��ǥ�� ��ȯ
                    int screenX = (int)(x * scale * 6.184 / Math.Cbrt(z + 250));
                    int screenY = (int)(currentHeight - z * scale * 6.184 / Math.Cbrt(z + 250));

                    // ���÷� ��ƼƼ�� ������ ǥ�� (50% �߽� ����)
                    int size = (int)(40 * scale * 6.184 / Math.Cbrt(z + 250)); // ��ƼƼ ũ�� (�ȼ�)

                    g.DrawImage(entity.Image, screenX - size / 2, screenY - size, size, size);

                    //entity.Width = size; entity.Height = size;
                    //entity.Location = new Point(screenX - size / 2, screenY - size);
                    //e.Graphics.FillEllipse(Brushes.White, screenX - (size / 2), screenY - (size / 2), size, size);
                }
            }
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
